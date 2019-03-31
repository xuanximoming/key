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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
#pragma warning disable 0618
namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle
{
    /// <summary>
    /// <para>Represents an Oracle database.</para>
    /// </summary>
    /// <remarks> 
    /// <para>
    /// Internally uses Oracle .NET Managed Provider from Microsoft (<see cref="System.Data.OracleClient"/>) to connect to a Oracle 9i database.
    /// </para>  
    /// <para>
    /// When retrieving a result set, it will build the package name. The package name should be set based
    /// on the stored procedure prefix and this should be set via configuration. For 
    /// example, a package name should be set as prefix of "ENTLIB_" and package name of
    /// "pkgENTLIB_ARCHITECTURE". For your applications, this is required only if you are defining your stored procedures returning 
    /// ref cursors.
    /// </para>
    /// </remarks>
    [OraclePermission(SecurityAction.Demand)]
    [DatabaseAssembler(typeof(OracleDatabaseAssembler))]
    [ContainerPolicyCreator(typeof(OracleDatabasePolicyCreator))]
    public class OracleDatabase : Database
    {
        private const string RefCursorName = "o_result";//默认输出名称都是o_result开头的，包括：o_result、o_result1、o_result3,具体控制在CommandSqlServer2Oracle.cs中
        private readonly IList<IOraclePackage> packages;
        private static readonly IList<IOraclePackage> emptyPackages = new List<IOraclePackage>(0);
        private readonly IDictionary<string, ParameterTypeRegistry> registeredParameterTypes
            = new Dictionary<string, ParameterTypeRegistry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class with a connection string and a list of Oracle packages.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        public OracleDatabase(string connectionString)
            : this(connectionString, emptyPackages)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class with a connection string and a list of Oracle packages.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <param name="packages">A list of <see cref="IOraclePackage"/> objects.</param>
        public OracleDatabase(string connectionString, IList<IOraclePackage> packages)
            : base(connectionString, OracleClientFactory.Instance)
        {
            if (packages == null) throw new ArgumentNullException("packages");

            this.packages = packages;
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
        public override void AddParameter(DbCommand command, string name, DbType dbType, int size,
            ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn,
            DataRowVersion sourceVersion, object value)
        {
            if (DbType.Guid.Equals(dbType))
            {
                object convertedValue = ConvertGuidToByteArray(value);

                AddParameter((OracleCommand)command, name, OracleType.Raw, 16, direction, nullable, precision,
                    scale, sourceColumn, sourceVersion, convertedValue);

                RegisterParameterType(command, name, dbType);
            }
            else
            {
                base.AddParameter(command, name, dbType, size, direction, nullable, precision, scale,
                    sourceColumn, sourceVersion, value);
            }
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="OracleParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The <see cref="OracleCommand"/> to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="oracleType"><para>One of the <see cref="OracleType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddParameter(OracleCommand command, string name, OracleType oracleType, int size,
            ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn,
            DataRowVersion sourceVersion, object value)
        {
            OracleParameter param = CreateParameter(name, DbType.AnsiString, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value) as OracleParameter;

            param.OracleType = oracleType;
            command.Parameters.Add(param);
        }

        /// <summary>
        /// Creates an <see cref="OracleDataReader"/> based on the <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command wrapper to execute.</param>        
        /// <returns>An <see cref="OracleDataReader"/> object.</returns>        
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override IDataReader ExecuteReader(DbCommand command)
        {
            PrepareCWRefCursor(command);
            return new OracleDataReaderWrapper((OracleDataReader)base.ExecuteReader(command));
        }

        /// <summary>
        /// <para>Creates an <see cref="OracleDataReader"/> based on the <paramref name="command"/>.</para>
        /// </summary>        
        /// <param name="command"><para>The command wrapper to execute.</para></param>        
        /// <param name="transaction"><para>The transaction to participate in when executing this reader.</para></param>        
        /// <returns><para>An <see cref="OracleDataReader"/> object.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="transaction"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override IDataReader ExecuteReader(DbCommand command, DbTransaction transaction)
        {
            PrepareCWRefCursor(command);
            return new OracleDataReaderWrapper((OracleDataReader)base.ExecuteReader(command, transaction));
        }

        /// <summary>
        /// <para>Executes a command and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The command to execute to fill the <see cref="DataSet"/></para></param>
        /// <returns><para>A <see cref="DataSet"/> filed with records and, if necessary, schema.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override DataSet ExecuteDataSet(DbCommand command)
        {
            //delete by wwj 2011-06-17 base.ExecuteDataSet ===> LoadDataSet ===> base.LoadDataSet 
            //由于在LoadDataSet方法中已经存在PrepareCWRefCursor方法，所以在此方法中的这个方法拿掉

            //PrepareCWRefCursor(command);  
            return base.ExecuteDataSet(command);
        }

        /// <summary>
        /// <para>Executes a command and returns the result in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The command to execute to fill the <see cref="DataSet"/></para></param>
        /// <param name="transaction"><para>The transaction to participate in when executing this reader.</para></param>        
        /// <returns><para>A <see cref="DataSet"/> filed with records and, if necessary, schema.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> can not be <see langword="null"/> (<b>Nothing</b> in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="command"/> can not be <see langword="null"/> (<b>Nothing</b> in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="transaction"/> can not be <see langword="null"/> (<b>Nothing</b> in Visual Basic).</para>
        /// </exception>
        public override DataSet ExecuteDataSet(DbCommand command, DbTransaction transaction)
        {
            //delete by wwj 2011-06-17 base.ExecuteDataSet ===> LoadDataSet ===> base.LoadDataSet 
            //由于在LoadDataSet方法中已经存在PrepareCWRefCursor方法，所以在此方法中的这个方法拿掉

            //PrepareCWRefCursor(command);  
            return base.ExecuteDataSet(command, transaction);
        }

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
            PrepareCWRefCursor(command);
            base.LoadDataSet(command, dataSet, tableNames);
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from a <see cref="DbCommand"/> in a transaction.</para>
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
            PrepareCWRefCursor(command);
            base.LoadDataSet(command, dataSet, tableNames, transaction);
        }

        /// <summary>
        /// Gets a parameter value.
        /// </summary>
        /// <param name="command">The command that contains the parameter.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        public override object GetParameterValue(DbCommand command, string parameterName)
        {
            object convertedValue = base.GetParameterValue(command, parameterName);

            ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
            if (registry != null)
            {
                if (registry.HasRegisteredParameterType(parameterName))
                {
                    DbType dbType = registry.GetRegisteredParameterType(parameterName);

                    if (DbType.Guid == dbType)
                    {
                        convertedValue = ConvertByteArrayToGuid(convertedValue);
                    }
                    else if (DbType.Boolean == dbType)
                    {
                        convertedValue = Convert.ToBoolean(convertedValue, CultureInfo.InvariantCulture);
                    }
                }
            }

            return convertedValue;
        }

        /// <summary>
        /// Sets a parameter value.
        /// </summary>
        /// <param name="command">The command with the parameter.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        public override void SetParameterValue(DbCommand command, string parameterName, object value)
        {
            object convertedValue = value;

            ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
            if (registry != null)
            {
                if (registry.HasRegisteredParameterType(parameterName))
                {
                    DbType dbType = registry.GetRegisteredParameterType(parameterName);

                    if (DbType.Guid == dbType)
                    {
                        convertedValue = ConvertGuidToByteArray(value);
                    }
                }
            }

            base.SetParameterValue(command, parameterName, convertedValue);
        }

        private ParameterTypeRegistry GetParameterTypeRegistry(string commandText)
        {
            ParameterTypeRegistry registry;
            registeredParameterTypes.TryGetValue(commandText, out registry);
            return registry;
        }


        private void RegisterParameterType(DbCommand command, string parameterName, DbType dbType)
        {
            ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
            if (registry == null)
            {
                registry = new ParameterTypeRegistry(command.CommandText);
                registeredParameterTypes.Add(command.CommandText, registry);
            }

            registry.RegisterParameterType(parameterName, dbType);
        }

        private static object ConvertGuidToByteArray(object value)
        {
            return ((value is DBNull) || (value == null)) ? Convert.DBNull : ((Guid)value).ToByteArray();
        }

        private static object ConvertByteArrayToGuid(object value)
        {
            byte[] buffer = (byte[])value;
            if (buffer.Length == 0)
            {
                return DBNull.Value;
            }
            else
            {
                return new Guid(buffer);
            }
        }

        /// <devdoc>
        /// Listens for the RowUpdate event on a data adapter to support UpdateBehavior.Continue
        /// </devdoc>
        private void OnOracleRowUpdated(object sender, OracleRowUpdatedEventArgs args)
        {
            if (args.RecordsAffected == 0)
            {
                if (args.Errors != null)
                {
                    args.Row.RowError = Resources.ExceptionMessageUpdateDataSetRowFailure;
                    args.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
        /// </summary>
        /// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
        /// <remarks>
        /// The <see cref="DbCommand"/> must be an instance of a <see cref="OracleCommand"/> object.
        /// </remarks>
        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
            OracleCommandBuilder.DeriveParameters((OracleCommand)discoveryCommand);
        }

        /// <summary>
        /// <para>Creates a <see cref="DbCommand"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <param name="parameterValues"><para>The list of parameters for the procedure.</para></param>
        /// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>
        /// <remarks>
        /// <para>The parameters for the stored procedure will be discovered and the values are assigned in positional order.</para>
        /// </remarks>        
        public override DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
        {
            // need to do this before of eventual parameter discovery
            string updatedStoredProcedureName = TranslatePackageSchema(storedProcedureName);
            DbCommand command = base.GetStoredProcCommand(updatedStoredProcedureName, parameterValues);
            return command;
        }

        /// <summary>
        /// <para>Creates a <see cref="DbCommand"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>		
        /// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>
        /// <remarks>
        /// <para>The parameters for the stored procedure will be discovered and the values are assigned in positional order.</para>
        /// </remarks>        
        public override DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            // need to do this before of eventual parameter discovery
            string updatedStoredProcedureName = TranslatePackageSchema(storedProcedureName);
            DbCommand command = base.GetStoredProcCommand(updatedStoredProcedureName);
            return command;
        }

        /// <summary>
        /// Sets the RowUpdated event for the data adapter.
        /// </summary>
        /// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
        /// <remarks>The <see cref="DbDataAdapter"/> must be an <see cref="OracleDataAdapter"/>.</remarks>
        protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
        {
            ((OracleDataAdapter)adapter).RowUpdated += OnOracleRowUpdated;
        }

        /// <summary>
        /// 获取与Database一致的Command对象
        /// </summary>
        /// <returns></returns>
        public override DbCommand NewCommand()
        {
            //throw new Exception("The method or operation is not implemented.");
            return new System.Data.OracleClient.OracleCommand();
        }

        #region override

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        protected override void ResetCommandAndParameters(IDbCommand command)
        {
            OracleParameterCollection paramCollection = command.Parameters as OracleParameterCollection;
            if (paramCollection != null)
            {
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    OracleParameter param = command.Parameters[i] as OracleParameter;
                    m_CommandSqlServer2Oracle.ConvertParameter(this, command, param);
                }
            }
            base.ResetCommandAndParameters(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(DbCommand command)
        {
            m_CommandSqlServer2Oracle.ConvertToOracle(this, command);
            return base.ExecuteNonQuery(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            commandText = m_CommandSqlServer2Oracle.ConvertToOracle(this, commandText);
            return base.ExecuteNonQuery(commandType, commandText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(DbCommand command, DbTransaction transaction)
        {
            m_CommandSqlServer2Oracle.ConvertToOracle(this, command);
            return base.ExecuteNonQuery(command, transaction);
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

            DataTable dt = new DataTable(tableName);
            dt.Columns.Add("zzl");//自增列
            dt.Columns.Add("isnullable");//是否可空 0：not null 1：null
            dt.Columns.Add("lm");//列名
            dt.Columns.Add("colid");//列序号
            dt.Columns.Add("prec");//精确度 numeric（12，0）
            dt.Columns.Add("scale");//小数点范围
            dt.Columns.Add("type");//基础类型
            dt.Columns.Add("usertype");//实际类型，因为有可能为自定义类型
            dt.Columns.Add("length");//列长度
            dt.Columns.Add("iskey");//是否属于主键的列

            //得到表结构
            string sql = string.Format("select * from all_tab_columns where table_name = upper('{0}')", tableName);
            DbCommand sqlCmd = GetSqlStringCommand(sql);
            DataSet columnDS = ExecuteDataSet(sqlCmd);
            foreach (DataRow dr in columnDS.Tables[0].Rows)
            {
                DataRow drNew = dt.NewRow();
                drNew["zzl"] = 0;
                drNew["isnullable"] = dr["Nullable"].Equals("Y") ? 1 : 0;
                drNew["lm"] = dr["column_name"].ToString().ToUpper();
                drNew["colid"] = Convert.ToInt32(dr["column_id"]);
                drNew["prec"] = dr["data_precision"];
                drNew["scale"] = dr["data_scale"];
                drNew["type"] = dr["data_type"].ToString().ToUpper();
                drNew["usertype"] = dr["data_type"].ToString().ToUpper();
                drNew["length"] = dr["data_length"].ToString();
                drNew["iskey"] = 0;
                dt.Rows.Add(drNew);
            }

            //得到主键
            sql = string.Format("select  col.* " +
                 " from user_constraints con,user_cons_columns col " +
                 " where con.constraint_name=col.constraint_name and con.constraint_type='P' " +
                 " and col.table_name=upper('{0}') ", tableName);
            sqlCmd = GetSqlStringCommand(sql);
            columnDS = ExecuteDataSet(sqlCmd);
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataRow drInner in columnDS.Tables[0].Rows)
                {
                    if (dr["lm"].ToString().ToUpper() == drInner["column_name"].ToString().ToUpper())
                    {
                        dr["iskey"] = 1;
                    }
                }
            }

            //得到自增长列
            sql = string.Format("select * from seq_totablecolumn s where upper(s.tablename) = upper('{0}')", tableName);
            sqlCmd = GetSqlStringCommand(sql);
            columnDS = ExecuteDataSet(sqlCmd);
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataRow drInner in columnDS.Tables[0].Rows)
                {
                    if (dr["lm"].ToString().ToUpper() == drInner["columnname"].ToString().ToUpper())
                    {
                        dr["zzl"] = 1;
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override object ExecuteScalar(DbCommand command)
        {
            m_CommandSqlServer2Oracle.ConvertToOracle(this, command);
            return base.ExecuteScalar(command);
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="command"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public override object ExecuteScalar(DbCommand command, DbTransaction transaction)
        {
            m_CommandSqlServer2Oracle.ConvertToOracle(this, command);
            return base.ExecuteScalar(command, transaction);
        }
        #endregion


        /// <summary>
        /// 默认PackageName
        /// </summary>
        string m_DefaultPackageName = "EMRPROC";//对于程序中没有使用包名的存储过程使用
        /// <devdoc>
        /// Looks into configuration and gets the information on how the command wrapper should be updated if calling a package on this
        /// connection.
        /// </devdoc>        
        private string TranslatePackageSchema(string storedProcedureName)
        {

            string packageName = String.Empty;
            string updatedStoredProcedureName = storedProcedureName;

            if (updatedStoredProcedureName.IndexOf('.') <= 0)//保证没有使用包名的存储过程加上包名
            {
                packageName = m_DefaultPackageName;
            }

            if (0 != packageName.Length)
            {
                updatedStoredProcedureName = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", packageName, storedProcedureName);
            }
            return updatedStoredProcedureName;
        }

        #region Add Ref Cursor Parameter
        /// <summary>
        /// 判断DbCommand中是否存在Ref Cursor类型的参数
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private bool QueryProcedureNeedsCursorParameter(DbCommand command)
        {
            foreach (OracleParameter parameter in command.Parameters)
            {
                if (parameter.OracleType == OracleType.Cursor)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断如果是存储过程，并且参数中没有Ref Cursor类型的参数时自动添加Ref Cursor类型的参数，用于输出
        /// </summary>
        /// <param name="command"></param>
        private void PrepareCWRefCursor(DbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            if (CommandType.StoredProcedure == command.CommandType)
            {
                // 在command中检查Ref Cursor, 如果不存在就添加一个Ref Cursor类型的参数o_result
                if (QueryProcedureNeedsCursorParameter(command))
                {
                    List<string> refCursorNameList = m_CommandSqlServer2Oracle.GetRefCursorList(command.CommandText.Trim().ToUpper());

                    foreach (string refCursorName in refCursorNameList)
                    {
                        AddParameter(command as OracleCommand, refCursorName, OracleType.Cursor, 0, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull);
                    }
                }
            }
            m_CommandSqlServer2Oracle.ConvertToOracle(this, command);
        }
        #endregion

        #region plug for do log

        /// <summary>
        /// <para>得到参数标记用于分割参数 for oracle DataBase</para>
        /// </summary>
        /// <value>
        /// <para></para>
        /// </value>
        protected override char ParameterToken
        {
            get { return 'v'; }
        }

        /// <summary>
        /// 为当前数据库建立参数名称
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>正确格式的参数名</returns>
        public override string BuildParameterName(string name)
        {
            if (/*name[0] != this.ParameterToken && */!name.StartsWith(RefCursorName)/*排除输出参数*/)
            {
                //将符合SqlServer的参数转为Oracle支持的参数样式 
                //例如： @ID =====> v_ID
                return this.ParameterToken + "_" + name.TrimStart('@');
            }
            return name;
        }

        /// <summary>
        /// 将CommandText转换成FormatString 
        /// 例如： select * from users where id = @id and name = @name =====> select * from users where id = {0} and name = {1}
        /// </summary>
        /// <param name="currentCmd">包含CommandText和必要参数的DbCommand</param>
        /// <returns>FormatString</returns>
        public override string GetCommandTextFormatString(DbCommand currentCmd)
        {
            // 将命令中的输入参数按替换成对应的参数序号(因为是Text型Command，所以不考虑有输出参数的情况)
            StringBuilder newCommand = new StringBuilder();

            // 对CommandText进行预处理，保证每个参数后都有一个空格
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
        /// 将存储过程型的Command命令转换成合适的、可执行的SQL语法命令
        /// </summary>
        /// <param name="command">需要转换的Command</param>
        /// <returns></returns>
        public override string ConvertStoredProcedure2Sql(DbCommand command)
        {
            return "exec " + command.CommandText + GetProcedureParametersString(command.Parameters);
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
        /// 存储过程输出参数的标记
        /// </summary>
        protected override string ProcedureOutputFlag
        {
            get { return "output"; }
        }

        #endregion

        /// <summary>
        /// 返回参数Token
        /// </summary>
        /// <returns></returns>
        protected override string GetParameterToken()
        {
            return ":" + ParameterToken + "_";
        }


    }

    internal sealed class ParameterTypeRegistry
    {
        private string commandText;
        private IDictionary<string, DbType> parameterTypes;

        internal ParameterTypeRegistry(string commandText)
        {
            this.commandText = commandText;
            this.parameterTypes = new Dictionary<string, DbType>();
        }

        internal void RegisterParameterType(string parameterName, DbType parameterType)
        {
            this.parameterTypes[parameterName] = parameterType;
        }

        internal bool HasRegisteredParameterType(string parameterName)
        {
            return this.parameterTypes.ContainsKey(parameterName);
        }

        internal DbType GetRegisteredParameterType(string parameterName)
        {
            return this.parameterTypes[parameterName];
        }
    }
}
