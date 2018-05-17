//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// 自定义 Caché数据库
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Security.Permissions;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System.Text;
using System.Collections.ObjectModel;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Dm
{
    /// <summary>
    /// <para>Represents a  DM database.</para>
    /// </summary>
    /// <remarks> 
    /// <para>
    /// Internally uses OleDb Provider to connect to the DM database.
    /// </para>  
    /// </remarks>
    [OleDbPermission(SecurityAction.Demand)]
    [DatabaseAssembler(typeof(DmDatabaseAssembler))]
    public class DmDatabase : Database
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DmDatabase"/> class with a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DmDatabase(string connectionString)
            : base(connectionString, OleDbFactory.Instance)
        {
        }

        /// <summary>
        /// <para>Gets the parameter token used to delimit parameters for the Cache database.</para>
        /// </summary>
        /// <value>
        /// <para>The '@' symbol.</para>
        /// </value>
        protected override char ParameterToken
        {
            get { return '@'; }
        }

        /// <devdoc>
        /// Listens for the RowUpdate event on a dataadapter to support UpdateBehavior.Continue
        /// </devdoc>
        private void OnOleDbRowUpdated(object sender, OleDbRowUpdatedEventArgs rowThatCouldNotBeWritten)
        {
            if (rowThatCouldNotBeWritten.RecordsAffected == 0)
            {
                if (rowThatCouldNotBeWritten.Errors != null)
                {
                    rowThatCouldNotBeWritten.Row.RowError = Resources.ExceptionMessageUpdateDataSetRowFailure;
                    rowThatCouldNotBeWritten.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
        /// </summary>
        /// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
        /// <remarks>The <see cref="DbCommand"/> must be a <see cref="OleDbCommand"/> instance.</remarks>
        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
            OleDbCommandBuilder.DeriveParameters((OleDbCommand)discoveryCommand);
        }

        /// <summary>
        /// Returns the starting index for parameters in a command.
        /// </summary>
        /// <returns>The starting index for parameters in a command.</returns>
        protected override int UserParametersStartIndex()
        {
            return 1; // DM 存储过程第一个参数和SQL Server一样也是返回值？？？
        }

        /// <summary>
        /// Builds a value parameter name for the current database.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>A correctly formated parameter name.</returns>
        public override string BuildParameterName(string name)
        {
            if (name[0] != this.ParameterToken)
            {
                return name.Insert(0, new string(this.ParameterToken, 1));
            }
            return name;
        }

        /// <summary>
        /// Sets the RowUpdated event for the data adapter.
        /// </summary>
        /// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
        protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
        {
            ((OleDbDataAdapter)adapter).RowUpdated += new OleDbRowUpdatedEventHandler(OnOleDbRowUpdated);
        }

        /// <summary>
        /// Determines if the number of parameters in the command matches the array of parameter values.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> containing the parameters.</param>
        /// <param name="values">The array of parameter values.</param>
        /// <returns><see langword="true"/> if the number of parameters and values match; otherwise, <see langword="false"/>.</returns>
        protected override bool SameNumberOfParametersAndValues(DbCommand command, object[] values)
        {
            int returnParameterCount = 1;
            int numberOfParametersToStoredProcedure = command.Parameters.Count - returnParameterCount;
            int numberOfValuesProvidedForStoredProcedure = values.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>       
        public virtual void AddParameter(DbCommand command, string name, OleDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="OleDbType"/> values.</para></param>        
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>    
        public virtual void AddParameter(DbCommand command, string name, OleDbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        /// <summary>
        /// Adds a new Out <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the out parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="OleDbType"/> values.</para></param>        
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>        
        public void AddOutParameter(DbCommand command, string name, OleDbType dbType, int size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the in parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="OleDbType"/> values.</para></param>                
        /// <remarks>
        /// <para>This version of the method is used when you can have the same parameter object multiple times with different values.</para>
        /// </remarks>        
        public void AddInParameter(DbCommand command, string name, OleDbType dbType)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The commmand to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="OleDbType"/> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddInParameter(DbCommand command, string name, OleDbType dbType, object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="OleDbType"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the value.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        public void AddInParameter(DbCommand command, string name, OleDbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        protected DbParameter CreateParameter(string name, OleDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            OleDbParameter param = CreateParameter(name) as OleDbParameter;
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        /// <summary>
        /// Configures a given <see cref="DbParameter"/>.
        /// </summary>
        /// <param name="param">The <see cref="DbParameter"/> to configure.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="OleDbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        protected virtual void ConfigureParameter(OleDbParameter param, string name, OleDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            param.OleDbType = dbType;
            param.Size = size;
            param.Value = (value == null) ? DBNull.Value : value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }

        #region

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from a <see cref="DbCommand"/>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        public override void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
        {
            using (DbConnection connection = CreateConnection())
            {
                PrepareCommand(command, connection);
                DoLoadDataSet(command, dataSet, tableNames);
            }
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from a <see cref="DbCommand"/> in  a transaction.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command in.</para>
        /// </param>
        public override void LoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames, DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            DoLoadDataSet(command, dataSet, tableNames);
        }

        private void DoLoadDataSet(DbCommand command, DataSet dataSet, string[] tableNames)
        {
            if (tableNames == null) throw new ArgumentNullException("tableNames");
            if (tableNames.Length == 0)
            {
                throw new ArgumentException(Resources.ExceptionTableNameArrayEmpty, "tableNames");
            }
            for (int i = 0; i < tableNames.Length; i++)
            {
                if (string.IsNullOrEmpty(tableNames[i])) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, string.Concat("tableNames[", i, "]"));
            }

            using (DbDataAdapter adapter = GetDataAdapter(UpdateBehavior.Standard))
            {
                ResetCommandAndParameters(command);
                ((IDbDataAdapter)adapter).SelectCommand = command;

                try
                {
                    DateTime startTime = DateTime.Now;
                    string systemCreatedTableNameRoot = "Table";
                    for (int i = 0; i < tableNames.Length; i++)
                    {
                        string systemCreatedTableName = (i == 0)
                            ? systemCreatedTableNameRoot
                            : systemCreatedTableNameRoot + i;

                        adapter.TableMappings.Add(systemCreatedTableName, tableNames[i]);
                    }

                    adapter.Fill(dataSet);
                    instrumentationProvider.FireCommandExecutedEvent(startTime);
                    // 需要将列名改成小写
                    LowerColumnName(dataSet);
                }
                catch (Exception e)
                {
                    instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                    throw;
                }
            }
        }

        private void LowerColumnName(DataSet dataSet)
        {
            if ((dataSet == null) || (dataSet.Tables.Count == 0))
                return;

            foreach (DataTable table in dataSet.Tables)
            {
                foreach (DataColumn col in table.Columns)
                    col.ColumnName = col.ColumnName.ToLower();
            }
        }
        #endregion

        #region Add

        /// <summary>
        /// 获取指定SQL表所有列的详细定义
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override DataTable GetTableColumnDefinitions(string tableName)
        {
            // 此处要使用DM自己取表结构的存储过程！！！
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "tableName");

            DbCommand storedProcCmd = GetStoredProcCommand("usp_helpcolumns");
            AddInParameter(storedProcCmd, "tablename", DbType.String, tableName);

            DataSet columnDS = ExecuteDataSet(storedProcCmd);

            return columnDS.Tables[0];
        }

        /// <summary>
        /// 获取与Database一致的Command对象
        /// </summary>
        /// <returns></returns>
        public override DbCommand NewCommand()
        {
            return new OleDbCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        protected override void ResetCommandAndParameters(IDbCommand command)
        {
            // OleDb调用存储过程的方式和Sql一样
            if (command.CommandType == CommandType.Text)
            {
                ResetTextCommandAndParameters(command);
            }
            else
                return;
        }

        private void ResetTextCommandAndParameters(IDbCommand command)
        {
            Collection<string> allParas = DeriveAllParameterNameFromText(command, "?");

            if (!command.CommandText.Equals(allParas[allParas.Count - 1]))
                command.CommandText = allParas[allParas.Count - 1];

            // 将command现有的参数列表和检索出来的参数做对照，调整先后顺序、补充参数
            int originalIndex;
            DbParameter parameter;
            for (int index = 0; index < allParas.Count - 1; index++)
            {
                originalIndex = command.Parameters.IndexOf(allParas[index]);
                if (originalIndex == -1)
                    throw new ArgumentOutOfRangeException("未找到参数");
                if (originalIndex == index)
                    continue;

                if (originalIndex > index)
                {
                    // move up
                    parameter = command.Parameters[originalIndex] as DbParameter;
                    command.Parameters.RemoveAt(originalIndex);
                    command.Parameters.Insert(index, parameter);
                }
                else if (originalIndex < index)
                {
                    // clone
                    parameter = (DbParameter)((ICloneable)command.Parameters[originalIndex]).Clone();
                    command.Parameters.Insert(index, parameter);
                }
            }
            if (command.Parameters.Count > allParas.Count)
            {
                for (int index = command.Parameters.Count - 1; index >= allParas.Count - 1; index--)
                    command.Parameters.RemoveAt(index);
            }
        }
        #endregion

        #region plug for do log

        /// <summary>
        /// 存储过程输出参数的标记（不需要）
        /// </summary>
        protected override string ProcedureOutputFlag
        {
            get { return ""; }
        }

        /// <summary>
        /// 将CommandText转换成FormatString
        /// </summary>
        /// <param name="currentCmd">包含CommandText和必要参数的DbCommand</param>
        /// <returns>FormatString</returns>
        public override string GetCommandTextFormatString(DbCommand currentCmd)
        {
            // 将命令中的"?"按顺序替换成对应的参数序号
            string commandText = currentCmd.CommandText;
            string[] splitCommand = commandText.Split('?');

            StringBuilder newCommand = new StringBuilder();

            for (int i = 0; i <= splitCommand.Length - 2; i++)
            {
                newCommand.Append(splitCommand[i]);
                newCommand.AppendFormat("{{{0}}}", i.ToString());
            }
            newCommand.Append(splitCommand[splitCommand.GetUpperBound(0)]);
            return newCommand.ToString();
        }

        /// <summary>
        /// 生成存储过程的输入参数串
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        protected override string GetProcedureParametersString(IDataParameterCollection parameters)
        {
            if (parameters.Count == 0)
                return "";

            StringBuilder inputs = new StringBuilder();
            // 直接组合输入参数的值
            foreach (IDbDataParameter para in parameters)
            {
                // 暂不处理返回参数和输出参数
                if ((para.Direction == ParameterDirection.ReturnValue)
                   || (para.Direction == ParameterDirection.Output))
                    continue;

                inputs.Append(FormatParameterValue(para));
                inputs.Append(",");
            }
            // 去掉最后一个“,”
            if (inputs.Length > 0)
                inputs.Remove(inputs.Length - 1, 1);

            return inputs.ToString();
        }

        /// <summary>
        /// 将存储过程型的Command命令转换成合适的、可执行的SQL语法命令
        /// </summary>
        /// <param name="command">需要转换的Command</param>
        /// <returns></returns>
        public override string ConvertStoredProcedure2Sql(DbCommand command)
        {
            return String.Format("CALL {0}( {1} );", command.CommandText.Substring(2, command.CommandText.IndexOf("("))
               , GetProcedureParametersString(command.Parameters));
        }
        #endregion
    }
}