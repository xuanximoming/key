//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Transactions;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System.Text;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql
{
    /// <summary>
    /// <para>Represents a SQL Server database.</para>
    /// </summary>
    /// <remarks> 
    /// <para>
    /// Internally uses SQL Server .NET Managed Provider from Microsoft (System.Data.SqlClient) to connect to the database.
    /// </para>  
    /// </remarks>
    [SqlClientPermission(SecurityAction.Demand)]
    [DatabaseAssembler(typeof(SqlDatabaseAssembler))]
    [ContainerPolicyCreator(typeof(SqlDatabasePolicyCreator))]
    public class SqlDatabase : Database
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabase"/> class with a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlDatabase(string connectionString)
            : base(connectionString, SqlClientFactory.Instance)
        {
        }

        /// <summary>
        /// <para>Gets the parameter token used to delimit parameters for the SQL Server database.</para>
        /// </summary>
        /// <value>
        /// <para>The '@' symbol.</para>
        /// </value>
        protected override char ParameterToken
        {
            get { return '@'; }
        }

        /// <summary>
        /// <para>Executes the <see cref="SqlCommand"/> and returns a new <see cref="XmlReader"/>.</para>
        /// </summary>
        /// <remarks>
        ///		Unlike other Execute... methods that take a <see cref="DbCommand"/> instance, this method
        ///		does not set the command behavior to close the connection when you close the reader.
        ///		That means you'll need to close the connection yourself, by calling the
        ///		command.Connection.Close() method.
        ///		<para>
        ///			There is one exception to the rule above. If you're using <see cref="TransactionScope"/> to provide
        ///			implicit transactions, you should NOT close the connection on this reader when you're
        ///			done. Only close the connection if <see cref="Transaction"/>.Current is null.
        ///		</para>
        /// </remarks>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="XmlReader"/> object.</para>
        /// </returns>
        public XmlReader ExecuteXmlReader(DbCommand command)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            ConnectionWrapper wrapper = GetOpenConnection(false);
            PrepareCommand(command, wrapper.Connection);
            return DoExecuteXmlReader(sqlCommand);
        }

        /// <summary>
        /// <para>Executes the <see cref="SqlCommand"/> in a transaction and returns a new <see cref="XmlReader"/>.</para>
        /// </summary>
        /// <remarks>
        ///		Unlike other Execute... methods that take a <see cref="DbCommand"/> instance, this method
        ///		does not set the command behavior to close the connection when you close the reader.
        ///		That means you'll need to close the connection yourself, by calling the
        ///		command.Connection.Close() method.
        /// </remarks>
        /// <param name="command">
        /// <para>The <see cref="SqlCommand"/> to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="XmlReader"/> object.</para>
        /// </returns>
        public XmlReader ExecuteXmlReader(DbCommand command, DbTransaction transaction)
        {
            SqlCommand sqlCommand = CheckIfSqlCommand(command);

            PrepareCommand(sqlCommand, transaction);
            return DoExecuteXmlReader(sqlCommand);
        }

        /// <devdoc>
        /// Execute the actual XML Reader call.
        /// </devdoc>        
        private XmlReader DoExecuteXmlReader(SqlCommand sqlCommand)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                XmlReader reader = sqlCommand.ExecuteXmlReader();
                instrumentationProvider.FireCommandExecutedEvent(startTime);
                return reader;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(sqlCommand.CommandText, ConnectionStringNoCredentials, e);
                throw;
            }
        }

        private static SqlCommand CheckIfSqlCommand(DbCommand command)
        {
            SqlCommand sqlCommand = command as SqlCommand;
            if (sqlCommand == null) throw new ArgumentException(Resources.ExceptionCommandNotSqlCommand, "command");
            return sqlCommand;
        }

        /// <devdoc>
        /// Listens for the RowUpdate event on a dataadapter to support UpdateBehavior.Continue
        /// </devdoc>
        private void OnSqlRowUpdated(object sender, SqlRowUpdatedEventArgs rowThatCouldNotBeWritten)
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
        /// <remarks>The <see cref="DbCommand"/> must be a <see cref="SqlCommand"/> instance.</remarks>
        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
            SqlCommandBuilder.DeriveParameters((SqlCommand)discoveryCommand);
        }

        /// <summary>
        /// Returns the starting index for parameters in a command.
        /// </summary>
        /// <returns>The starting index for parameters in a command.</returns>
        protected override int UserParametersStartIndex()
        {
            return 1;
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
            ((SqlDataAdapter)adapter).RowUpdated += OnSqlRowUpdated;
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
        public virtual void AddParameter(DbCommand command, string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>        
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>    
        public void AddParameter(DbCommand command, string name, SqlDbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        /// <summary>
        /// Adds a new Out <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the out parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>        
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>        
        public void AddOutParameter(DbCommand command, string name, SqlDbType dbType, int size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the in parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>                
        /// <remarks>
        /// <para>This version of the method is used when you can have the same parameter object multiple times with different values.</para>
        /// </remarks>        
        public void AddInParameter(DbCommand command, string name, SqlDbType dbType)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The commmand to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddInParameter(DbCommand command, string name, SqlDbType dbType, object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the value.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        public void AddInParameter(DbCommand command, string name, SqlDbType dbType, string sourceColumn, DataRowVersion sourceVersion)
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
        protected DbParameter CreateParameter(string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            SqlParameter param = CreateParameter(name) as SqlParameter;
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        /// <summary>
        /// Configures a given <see cref="DbParameter"/>.
        /// </summary>
        /// <param name="param">The <see cref="DbParameter"/> to configure.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="SqlDbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        protected virtual void ConfigureParameter(SqlParameter param, string name, SqlDbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            param.SqlDbType = dbType;
            param.Size = size;
            param.Value = (value == null) ? DBNull.Value : value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }

        /// <summary>
        /// 获取指定SQL表所有列的详细定义
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override DataTable GetTableColumnDefinitions(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "SQLTableName");

            DbCommand storedProcCmd = GetStoredProcCommand("sp_MShelpcolumns");
            AddInParameter(storedProcCmd, "tablename", DbType.String, tableName);
            AddInParameter(storedProcCmd, "flags", DbType.String);
            AddInParameter(storedProcCmd, "orderby", DbType.String, "id");
            AddInParameter(storedProcCmd, "flags2", DbType.String, 1);

            DataSet columnDS = ExecuteDataSet(storedProcCmd);
            // 更改部分列的名称
            columnDS.Tables[0].Columns["col_identity"].ColumnName = "zzl";
            columnDS.Tables[0].Columns["col_null"].ColumnName = "isnullable";
            columnDS.Tables[0].Columns["col_name"].ColumnName = "lm";
            columnDS.Tables[0].Columns["col_id"].ColumnName = "colid";
            columnDS.Tables[0].Columns["col_prec"].ColumnName = "prec";
            columnDS.Tables[0].Columns["col_scale"].ColumnName = "scale";
            columnDS.Tables[0].Columns["col_basetypename"].ColumnName = "type";
            columnDS.Tables[0].Columns["col_typename"].ColumnName = "usertype";
            columnDS.Tables[0].Columns["col_len"].ColumnName = "length";
            // 增加表示是否属于主键的列
            DataColumn keyColumn = new DataColumn("iskey", Type.GetType("System.Int32"), "CONVERT(col_flags / 4, 'System.Int32') % 2");
            columnDS.Tables[0].Columns.Add(keyColumn);

            return columnDS.Tables[0];
        }

        /// <summary>
        /// 获取与Database一致的Command对象
        /// </summary>
        /// <returns></returns>
        public override DbCommand NewCommand()
        {
            return new SqlCommand();
        }

        #region plug for do log

        /// <summary>
        /// 存储过程输出参数的标记
        /// </summary>
        protected override string ProcedureOutputFlag
        {
            get { return "output"; }
        }

        /// <summary>
        /// 将CommandText转换成FormatString
        /// </summary>
        /// <param name="currentCmd">包含CommandText和必要参数的DbCommand</param>
        /// <returns>FormatString</returns>
        public override string GetCommandTextFormatString(DbCommand currentCmd)
        {
            // 将命令中的输入参数按替换成对应的参数序号(因为是Text型Command，所以不考虑有输出参数的情况)
            StringBuilder newCommand = new StringBuilder();

            // 对CommandText进行预处理，保证每个参数后都有一个空格
            //string[] texts = currentCmd.CommandText.Split(new char[] { ParameterToken });
            StringBuilder paraName = new StringBuilder();
            bool composePara = false;
            int paraPos;
            foreach (char c in currentCmd.CommandText)
            {
                if (c == ParameterToken) // 找到参数的开始标记
                {
                    composePara = true;
                    paraName = new StringBuilder();
                    paraName.Append(ParameterToken);
                }
                else if (composePara)
                {
                    // 拼参数，直到非法字符(目前参数中允许出现下划线、字母、数字)
                    switch (CharUnicodeInfo.GetUnicodeCategory(c))
                    {
                        case UnicodeCategory.DashPunctuation:
                        case UnicodeCategory.DecimalDigitNumber:
                        case UnicodeCategory.LowercaseLetter:
                        case UnicodeCategory.UppercaseLetter:
                            paraName.Append(c);
                            break;
                        default:
                            // 替换参数为占位符,加入command
                            composePara = false;
                            paraPos = currentCmd.Parameters.IndexOf(paraName.ToString());
                            if (paraPos >= 0)
                                newCommand.Append("{" + paraPos + "}");
                            else
                                newCommand.Append(paraName.ToString());
                            newCommand.Append(c);
                            break;
                    }
                }
                else
                    newCommand.Append(c);
            }

            if (composePara)
            {
                paraPos = currentCmd.Parameters.IndexOf(paraName.ToString());
                if (paraPos >= 0)
                    newCommand.Append("{" + paraPos + "}");
                else
                    newCommand.Append(paraName.ToString());
            }

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
            foreach (IDbDataParameter para in parameters)
            {
                // 暂不处理返回参数
                if (para.Direction == ParameterDirection.ReturnValue)
                    continue;

                if ((para.Direction == ParameterDirection.Input)
                   || (para.Direction == ParameterDirection.InputOutput))
                {
                    // 输入参数对
                    inputs.AppendFormat(" {0} = {1} "
                       , para.ParameterName
                       , FormatParameterValue(para));
                }
                else
                {
                    // 输出参数
                    inputs.AppendFormat(" {0} ", para.ParameterName);
                }

                // 输出参数要加上output关键字
                if ((para.Direction == ParameterDirection.Output)
                   || (para.Direction == ParameterDirection.InputOutput))
                    inputs.Append(ProcedureOutputFlag);

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
            return "exec " + command.CommandText + GetProcedureParametersString(command.Parameters);
        }
        #endregion
    }
}
