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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Transactions;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Represents an abstract database that commands can be run against. 
    /// </summary>
    /// <remarks>
    /// The <see cref="Database"/> class leverages the provider factory model from ADO.NET. A database instance holds 
    /// a reference to a concrete <see cref="DbProviderFactory"/> object to which it forwards the creation of ADO.NET objects.
    /// </remarks>
    [ConfigurationNameMapper(typeof(DatabaseMapper))]
    [CustomFactory(typeof(DatabaseCustomFactory))]
    public abstract class Database : IInstrumentationEventProvider
    {
        static readonly ParameterCache parameterCache = new ParameterCache();
        static readonly string VALID_PASSWORD_TOKENS = Resources.Password;
        static readonly string VALID_USER_ID_TOKENS = Resources.UserName;

        readonly ConnectionString connectionString;
        readonly DbProviderFactory dbProviderFactory;

        /// <summary>
        /// The <see cref="DataInstrumentationProvider"/> instance that defines the logical events used to instrument this <see cref="Database"/> instance.
        /// </summary>
        protected DataInstrumentationProvider instrumentationProvider;

        /// <summary>
        /// sqlserver 转 oracle 工具
        /// </summary>
        protected CommandSqlServer2Oracle m_CommandSqlServer2Oracle = new CommandSqlServer2Oracle();

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class with a connection string and a <see cref="DbProviderFactory"/>.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <param name="dbProviderFactory">A <see cref="DbProviderFactory"/> object.</param>
        protected Database(string connectionString,
                           DbProviderFactory dbProviderFactory)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "connectionString");
            if (dbProviderFactory == null) throw new ArgumentNullException("dbProviderFactory");

            this.connectionString = new ConnectionString(connectionString, VALID_USER_ID_TOKENS, VALID_PASSWORD_TOKENS);
            this.dbProviderFactory = dbProviderFactory;
            instrumentationProvider = new DataInstrumentationProvider();
        }

        /// <summary>
        /// <para>Gets the string used to open a database.</para>
        /// </summary>
        /// <value>
        /// <para>The string used to open a database.</para>
        /// </value>
        /// <seealso cref="DbConnection.ConnectionString"/>
        public string ConnectionString
        {
            get { return connectionString.ToString(); }
        }

        /// <summary>
        /// <para>Gets the connection string without the username and password.</para>
        /// </summary>
        /// <value>
        /// <para>The connection string without the username and password.</para>
        /// </value>
        /// <seealso cref="ConnectionString"/>
        protected string ConnectionStringNoCredentials
        {
            get { return connectionString.ToStringNoCredentials(); }
        }

        /// <summary>
        /// Gets the connection string without credentials.
        /// </summary>
        /// <value>
        /// The connection string without credentials.
        /// </value>
        public string ConnectionStringWithoutCredentials
        {
            get { return ConnectionStringNoCredentials; }
        }

        /// <summary>
        /// <para>Gets the DbProviderFactory used by the database instance.</para>
        /// </summary>
        /// <seealso cref="DbProviderFactory"/>
        public DbProviderFactory DbProviderFactory
        {
            get { return dbProviderFactory; }
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the in parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <remarks>
        /// <para>This version of the method is used when you can have the same parameter object multiple times with different values.</para>
        /// </remarks>        
        public void AddInParameter(DbCommand command,
                                   string name,
                                   DbType dbType)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, null);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The commmand to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public void AddInParameter(DbCommand command,
                                   string name,
                                   DbType dbType,
                                   object value)
        {
            AddParameter(command, name, dbType, ParameterDirection.Input, String.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the value.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        public void AddInParameter(DbCommand command,
                                   string name,
                                   DbType dbType,
                                   string sourceColumn,
                                   DataRowVersion sourceVersion)
        {
            AddParameter(command, name, dbType, 0, ParameterDirection.Input, true, 0, 0, sourceColumn, sourceVersion, null);
        }

        /// <summary>
        /// Adds a new Out <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the out parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>        
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>        
        public void AddOutParameter(DbCommand command,
                                    string name,
                                    DbType dbType,
                                    int size)
        {
            AddParameter(command, name, dbType, size, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value);
        }

        /// <summary>
        /// Adds a new In <see cref="DbParameter"/> object to the given <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>Avalue indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>       
        public virtual void AddParameter(DbCommand command,
                                         string name,
                                         DbType dbType,
                                         int size,
                                         ParameterDirection direction,
                                         bool nullable,
                                         byte precision,
                                         byte scale,
                                         string sourceColumn,
                                         DataRowVersion sourceVersion,
                                         object value)
        {
            DbParameter parameter = CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>        
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>    
        public void AddParameter(DbCommand command,
                                 string name,
                                 DbType dbType,
                                 ParameterDirection direction,
                                 string sourceColumn,
                                 DataRowVersion sourceVersion,
                                 object value)
        {
            AddParameter(command, name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value);
        }

        void AssignParameterValues(DbCommand command,
                                   object[] values)
        {
            int parameterIndexShift = UserParametersStartIndex(); // DONE magic number, depends on the database
            for (int i = 0; i < values.Length; i++)
            {
                IDataParameter parameter = command.Parameters[i + parameterIndexShift];

                // There used to be code here that checked to see if the parameter was input or input/output
                // before assigning the value to it. We took it out because of an operational bug with
                // deriving parameters for a stored procedure. It turns out that output parameters are set
                // to input/output after discovery, so any direction checking was unneeded. Should it ever
                // be needed, it should go here, and check that a parameter is input or input/output before
                // assigning a value to it.
                SetParameterValue(command, parameter.ParameterName, values[i]);
            }
        }

        static DbTransaction BeginTransaction(DbConnection connection)
        {
            DbTransaction tran = connection.BeginTransaction();
            return tran;
        }

        /// <summary>
        /// Builds a value parameter name for the current database.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>A correctly formated parameter name.</returns>
        public virtual string BuildParameterName(string name)
        {
            return name;
        }

        /// <summary>
        /// Clears the parameter cache. Since there is only one parameter cache that is shared by all instances
        /// of this class, this clears all parameters cached for all databases.
        /// </summary>
        public static void ClearParameterCache()
        {
            parameterCache.Clear();
        }

        static void CommitTransaction(IDbTransaction tran)
        {
            tran.Commit();
        }

        /// <summary>
        /// Configures a given <see cref="DbParameter"/>.
        /// </summary>
        /// <param name="param">The <see cref="DbParameter"/> to configure.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>Avalue indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        protected virtual void ConfigureParameter(DbParameter param,
                                                  string name,
                                                  DbType dbType,
                                                  int size,
                                                  ParameterDirection direction,
                                                  bool nullable,
                                                  byte precision,
                                                  byte scale,
                                                  string sourceColumn,
                                                  DataRowVersion sourceVersion,
                                                  object value)
        {
            param.DbType = dbType;
            param.Size = size;
            param.Value = value ?? DBNull.Value;
            param.Direction = direction;
            param.IsNullable = nullable;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
        }

        DbCommand CreateCommandByCommandType(CommandType commandType,
                                             string commandText)
        {
            DbCommand command = dbProviderFactory.CreateCommand();
            command.CommandType = commandType;
            command.CommandText = commandText;

            return command;
        }

        /// <summary>
        /// <para>Creates a connection for this database.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="DbConnection"/> for this database.</para>
        /// </returns>
        /// <seealso cref="DbConnection"/>        
        public virtual DbConnection CreateConnection()
        {
            DbConnection newConnection = dbProviderFactory.CreateConnection();
            newConnection.ConnectionString = ConnectionString;

            return newConnection;
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>Avalue indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>  
        /// <returns>A newly created <see cref="DbParameter"/> fully initialized with given parameters.</returns>
        protected DbParameter CreateParameter(string name,
                                              DbType dbType,
                                              int size,
                                              ParameterDirection direction,
                                              bool nullable,
                                              byte precision,
                                              byte scale,
                                              string sourceColumn,
                                              DataRowVersion sourceVersion,
                                              object value)
        {
            DbParameter param = CreateParameter(name);
            ConfigureParameter(param, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            return param;
        }

        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <returns><para>An unconfigured parameter.</para></returns>
        protected DbParameter CreateParameter(string name)
        {
            DbParameter param = dbProviderFactory.CreateParameter();
            param.ParameterName = BuildParameterName(name);

            return param;
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
        /// </summary>
        /// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
        protected abstract void DeriveParameters(DbCommand discoveryCommand);

        /// <summary>
        /// Discovers the parameters for a <see cref="DbCommand"/>.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> to discover the parameters.</param>
        public void DiscoverParameters(DbCommand command)
        {
            using (ConnectionWrapper wrapper = GetOpenConnection())
            {
                using (DbCommand discoveryCommand = CreateCommandByCommandType(command.CommandType, command.CommandText))
                {
                    discoveryCommand.Connection = wrapper.Connection;
                    DeriveParameters(discoveryCommand);

                    foreach (IDataParameter parameter in discoveryCommand.Parameters)
                    {
                        IDataParameter cloneParameter = (IDataParameter)((ICloneable)parameter).Clone();
                        command.Parameters.Add(cloneParameter);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the query for <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> representing the query to execute.</param>
        /// <returns>The quantity of rows affected.</returns>
        protected int DoExecuteNonQuery(DbCommand command)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                ResetCommandAndParameters(command);
                int rowsAffected;
                if (!m_CommandSqlServer2Oracle.ExecuteContainBlob(this, command, out rowsAffected))
                {
                    rowsAffected = command.ExecuteNonQuery();
                    instrumentationProvider.FireCommandExecutedEvent(startTime);
                }
                else
                {
                    rowsAffected = 0;
                }
                return rowsAffected;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                throw;
            }
        }

        IDataReader DoExecuteReader(DbCommand command,
                                    CommandBehavior cmdBehavior)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                ResetCommandAndParameters(command);
                IDataReader reader = command.ExecuteReader(cmdBehavior);
                instrumentationProvider.FireCommandExecutedEvent(startTime);
                return reader;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                throw;
            }
        }

        object DoExecuteScalar(IDbCommand command)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                ResetCommandAndParameters(command);
                object returnValue = command.ExecuteScalar();
                instrumentationProvider.FireCommandExecutedEvent(startTime);
                return returnValue;
            }
            catch (Exception e)
            {
                instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                throw;
            }
        }

        void DoLoadDataSet(IDbCommand command,
                           DataSet dataSet,
                           string[] tableNames)
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

                    //adapter.Fill(dataSet);
                    FillData(dataSet, adapter, command);//add by wwj 2011-07-26 for 多个select语句
                    instrumentationProvider.FireCommandExecutedEvent(startTime);
                }
                catch (Exception e)
                {
                    instrumentationProvider.FireCommandFailedEvent(command.CommandText, ConnectionStringNoCredentials, e);
                    throw;
                }
            }
        }

        private void FillData(DataSet dataSet, DbDataAdapter adapter, IDbCommand command)
        {
            string tableName = "table";
            string[] sqls = command.CommandText.Split(';');
            for (int i = 0; i < sqls.Length; i++)
            {
                if (sqls[i].Trim() != "")
                {
                    command.CommandText = sqls[i].Replace('\n', ' ').Replace('\r', ' ');
                    adapter.Fill(dataSet, tableName + (i + 1));
                }
            }
        }

        int DoUpdateDataSet(UpdateBehavior behavior,
                            DataSet dataSet,
                            string tableName,
                            IDbCommand insertCommand,
                            IDbCommand updateCommand,
                            IDbCommand deleteCommand,
                            int? updateBatchSize)
        {
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "tableName");
            if (dataSet == null) throw new ArgumentNullException("dataSet");

            if (insertCommand == null && updateCommand == null && deleteCommand == null)
            {
                throw new ArgumentException(Resources.ExceptionMessageUpdateDataSetArgumentFailure);
            }

            using (DbDataAdapter adapter = GetDataAdapter(behavior))
            {
                IDbDataAdapter explicitAdapter = adapter;
                if (insertCommand != null)
                {
                    ResetCommandAndParameters(insertCommand);
                    explicitAdapter.InsertCommand = insertCommand;
                }
                if (updateCommand != null)
                {
                    ResetCommandAndParameters(updateCommand);
                    explicitAdapter.UpdateCommand = updateCommand;
                }
                if (deleteCommand != null)
                {
                    ResetCommandAndParameters(deleteCommand);
                    explicitAdapter.DeleteCommand = deleteCommand;
                }

                if (updateBatchSize != null)
                {
                    adapter.UpdateBatchSize = (int)updateBatchSize;
                    if (insertCommand != null)
                        adapter.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
                    if (updateCommand != null)
                        adapter.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                    if (deleteCommand != null)
                        adapter.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
                }

                try
                {
                    DateTime startTime = DateTime.Now;
                    int rows = adapter.Update(dataSet.Tables[tableName]);
                    instrumentationProvider.FireCommandExecutedEvent(startTime);
                    return rows;
                }
                catch (Exception e)
                {
                    instrumentationProvider.FireCommandFailedEvent("DbDataAdapter.Update() " + tableName, ConnectionStringNoCredentials, e);
                    throw;
                }
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The <see cref="DbCommand"/> to execute.</para></param>
        /// <returns>A <see cref="DataSet"/> with the results of the <paramref name="command"/>.</returns>        
        public virtual DataSet ExecuteDataSet(DbCommand command)
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = CultureInfo.InvariantCulture;
            LoadDataSet(command, dataSet, "Table");
            return dataSet;
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> as part of the <paramref name="transaction" /> and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The <see cref="DbCommand"/> to execute.</para></param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>A <see cref="DataSet"/> with the results of the <paramref name="command"/>.</returns>        
        public virtual DataSet ExecuteDataSet(DbCommand command,
                                              DbTransaction transaction)
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = CultureInfo.InvariantCulture;
            LoadDataSet(command, dataSet, "Table", transaction);
            return dataSet;
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> with <paramref name="parameterValues" /> and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="DataSet"/> with the results of the <paramref name="storedProcedureName"/>.</para>
        /// </returns>
        public virtual DataSet ExecuteDataSet(string storedProcedureName,
                                              params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteDataSet(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> with <paramref name="parameterValues" /> as part of the <paramref name="transaction" /> and returns the results in a new <see cref="DataSet"/> within a transaction.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="DataSet"/> with the results of the <paramref name="storedProcedureName"/>.</para>
        /// </returns>
        public virtual DataSet ExecuteDataSet(DbTransaction transaction,
                                              string storedProcedureName,
                                              params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteDataSet(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="DataSet"/> with the results of the <paramref name="commandText"/>.</para>
        /// </returns>
        public virtual DataSet ExecuteDataSet(CommandType commandType,
                                              string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteDataSet(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> as part of the given <paramref name="transaction" /> and returns the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="DataSet"/> with the results of the <paramref name="commandText"/>.</para>
        /// </returns>
        public virtual DataSet ExecuteDataSet(DbTransaction transaction,
                                              CommandType commandType,
                                              string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteDataSet(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>       
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual int ExecuteNonQuery(DbCommand command)
        {
            using (ConnectionWrapper wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                return DoExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> within the given <paramref name="transaction" />, and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual int ExecuteNonQuery(DbCommand command,
                                           DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteNonQuery(command);
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> using the given <paramref name="parameterValues" /> and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The name of the stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>The number of rows affected</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual int ExecuteNonQuery(string storedProcedureName,
                                           params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> using the given <paramref name="parameterValues" /> within a transaction and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The name of the stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of parameters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>The number of rows affected.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual int ExecuteNonQuery(DbTransaction transaction,
                                           string storedProcedureName,
                                           params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteNonQuery(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The number of rows affected.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual int ExecuteNonQuery(CommandType commandType,
                                           string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> as part of the given <paramref name="transaction" /> and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The number of rows affected</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual int ExecuteNonQuery(DbTransaction transaction,
                                           CommandType commandType,
                                           string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteNonQuery(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public virtual IDataReader ExecuteReader(DbCommand command)
        {
            ConnectionWrapper wrapper = GetOpenConnection(false);

            try
            {
                //
                // JS-L: I moved the PrepareCommand inside the try because it can fail.
                //
                PrepareCommand(command, wrapper.Connection);

                //
                // If there is a current transaction, we'll be using a shared connection, so we don't
                // want to close the connection when we're done with the reader.
                //
                if (Transaction.Current != null)
                    return DoExecuteReader(command, CommandBehavior.Default);
                else
                    return DoExecuteReader(command, CommandBehavior.CloseConnection);
            }
            catch
            {
                wrapper.Connection.Close();
                throw;
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> within a transaction and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public virtual IDataReader ExecuteReader(DbCommand command,
                                                 DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteReader(command, CommandBehavior.Default);
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> with the given <paramref name="parameterValues" /> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>        
        /// <param name="storedProcedureName">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public IDataReader ExecuteReader(string storedProcedureName,
                                         params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteReader(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> with the given <paramref name="parameterValues" /> within the given <paramref name="transaction" /> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public IDataReader ExecuteReader(DbTransaction transaction,
                                         string storedProcedureName,
                                         params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteReader(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public IDataReader ExecuteReader(CommandType commandType,
                                         string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteReader(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> within the given 
        /// <paramref name="transaction" /> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>        
        public IDataReader ExecuteReader(DbTransaction transaction,
                                         CommandType commandType,
                                         string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteReader(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual object ExecuteScalar(DbCommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            using (ConnectionWrapper wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
                return DoExecuteScalar(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> within a <paramref name="transaction" />, and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual object ExecuteScalar(DbCommand command,
                                            DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteScalar(command);
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> with the given <paramref name="parameterValues" /> and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual object ExecuteScalar(string storedProcedureName,
                                            params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteScalar(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> with the given <paramref name="parameterValues" /> within a 
        /// <paramref name="transaction" /> and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual object ExecuteScalar(DbTransaction transaction,
                                            string storedProcedureName,
                                            params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                return ExecuteScalar(command, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" />  and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual object ExecuteScalar(CommandType commandType,
                                            string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteScalar(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> 
        /// within the given <paramref name="transaction" /> and returns the first column of the first row in the result set returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the result set.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        public virtual object ExecuteScalar(DbTransaction transaction,
                                            CommandType commandType,
                                            string commandText)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                return ExecuteScalar(command, transaction);
            }
        }

        /// <summary>
        /// Gets a DbDataAdapter with Standard update behavior.
        /// </summary>
        /// <returns>A <see cref="DbDataAdapter"/>.</returns>
        /// <seealso cref="DbDataAdapter"/>
        /// <devdoc>
        /// Created this new, public method instead of modifying the protected, abstract one so that there will be no
        /// breaking changes for any currently derived Database class.
        /// </devdoc>
        public DbDataAdapter GetDataAdapter()
        {
            return GetDataAdapter(UpdateBehavior.Standard);
        }

        /// <summary>
        /// Gets the DbDataAdapter with the given update behavior and connection from the proper derived class.
        /// </summary>
        /// <param name="updateBehavior">
        /// <para>One of the <see cref="UpdateBehavior"/> values.</para>
        /// </param>        
        /// <returns>A <see cref="DbDataAdapter"/>.</returns>
        /// <seealso cref="DbDataAdapter"/>
        protected DbDataAdapter GetDataAdapter(UpdateBehavior updateBehavior)
        {
            DbDataAdapter adapter = dbProviderFactory.CreateDataAdapter();

            if (updateBehavior == UpdateBehavior.Continue)
            {
                SetUpRowUpdatedEvent(adapter);
            }
            return adapter;
        }

        /// <summary>
        /// Returns the object to which the instrumentation events have been delegated.
        /// </summary>
        /// <returns>Object to which the instrumentation events have been delegated.</returns>
        public object GetInstrumentationEventProvider()
        {
            return instrumentationProvider;
        }

        internal DbConnection GetNewOpenConnection()
        {
            DbConnection connection = null;
            try
            {
                try
                {
                    connection = CreateConnection();
                    connection.Open();
                }
                catch (Exception e)
                {
                    instrumentationProvider.FireConnectionFailedEvent(ConnectionStringNoCredentials, e);
                    throw;
                }

                instrumentationProvider.FireConnectionOpenedEvent();
            }
            catch
            {
                if (connection != null)
                    connection.Close();

                throw;
            }

            return connection;
        }

        /// <summary>
        ///		Get's a "wrapped" connection that will be not be disposed if a transaction is
        ///		active (created by creating a <see cref="TransactionScope"/> instance). The
        ///		connection will be disposed when no transaction is active.
        /// </summary>
        /// <returns></returns>
        protected ConnectionWrapper GetOpenConnection()
        {
            //modified by zhouhui
            //访问链接在外部会手工控制
            //return GetOpenConnection(true);
            return GetOpenConnection(false);
        }

        /// <summary>
        ///		Get's a "wrapped" connection that will be not be disposed if a transaction is
        ///		active (created by creating a <see cref="TransactionScope"/> instance). The
        ///		connection can be disposed when no transaction is active.
        /// </summary>
        /// <param name="disposeInnerConnection">
        ///		Determines if the connection will be disposed when there isn't an active
        ///		transaction.
        /// </param>
        /// <returns>The wrapped connection.</returns>
        protected ConnectionWrapper GetOpenConnection(bool disposeInnerConnection)
        {
            DbConnection connection = TransactionScopeConnections.GetConnection(this);
            if (connection != null)
                return new ConnectionWrapper(connection, false);
            else
                return new ConnectionWrapper(GetNewOpenConnection(), disposeInnerConnection);
        }

        /// <summary>
        /// Gets a parameter value.
        /// </summary>
        /// <param name="command">The command that contains the parameter.</param>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        public virtual object GetParameterValue(DbCommand command,
                                                string name)
        {
            return command.Parameters[BuildParameterName(name)].Value;
        }

        /// <summary>
        /// <para>Creates a <see cref="DbCommand"/> for a SQL query.</para>
        /// </summary>
        /// <param name="query"><para>The text of the query.</para></param>        
        /// <returns><para>The <see cref="DbCommand"/> for the SQL query.</para></returns>        
        public DbCommand GetSqlStringCommand(string query)
        {
            if (string.IsNullOrEmpty(query)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "query");

            return CreateCommandByCommandType(CommandType.Text, query);
        }

        /// <summary>
        /// <para>Creates a <see cref="DbCommand"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>       
        public virtual DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            if (string.IsNullOrEmpty(storedProcedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "storedProcedureName");

            return CreateCommandByCommandType(CommandType.StoredProcedure, storedProcedureName);
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
        public virtual DbCommand GetStoredProcCommand(string storedProcedureName,
                                                      params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(storedProcedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "storedProcedureName");

            DbCommand command = CreateCommandByCommandType(CommandType.StoredProcedure, storedProcedureName);

            parameterCache.SetParameters(command, this);

            if (SameNumberOfParametersAndValues(command, parameterValues) == false)
            {
                throw new InvalidOperationException(Resources.ExceptionMessageParameterMatchFailure);
            }

            AssignParameterValues(command, parameterValues);
            return command;
        }

        /// <summary>
        /// Wraps around a derived class's implementation of the GetStoredProcCommandWrapper method and adds functionality for
        /// using this method with UpdateDataSet.  The GetStoredProcCommandWrapper method (above) that takes a params array 
        /// expects the array to be filled with VALUES for the parameters. This method differs from the GetStoredProcCommandWrapper 
        /// method in that it allows a user to pass in a string array. It will also dynamically discover the parameters for the 
        /// stored procedure and set the parameter's SourceColumns to the strings that are passed in. It does this by mapping 
        /// the parameters to the strings IN ORDER. Thus, order is very important.
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <param name="sourceColumns"><para>The list of DataFields for the procedure.</para></param>
        /// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>
        public DbCommand GetStoredProcCommandWithSourceColumns(string storedProcedureName,
                                                               params string[] sourceColumns)
        {
            if (string.IsNullOrEmpty(storedProcedureName)) throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "storedProcedureName");
            if (sourceColumns == null) throw new ArgumentNullException("sourceColumns");

            DbCommand dbCommand = GetStoredProcCommand(storedProcedureName);

            //we do not actually set the connection until the Fill or Update, so we need to temporarily do it here so we can set the sourcecolumns
            using (DbConnection connection = CreateConnection())
            {
                dbCommand.Connection = connection;
                DiscoverParameters(dbCommand);
            }

            int iSourceIndex = 0;
            foreach (IDataParameter dbParam in dbCommand.Parameters)
            {
                if ((dbParam.Direction == ParameterDirection.Input) | (dbParam.Direction == ParameterDirection.InputOutput))
                {
                    dbParam.SourceColumn = sourceColumns[iSourceIndex];
                    iSourceIndex++;
                }
            }

            return dbCommand;
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and adds a new <see cref="DataTable"></see> to the existing <see cref="DataSet"></see>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="DbCommand"/> to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to load.</para>
        /// </param>
        /// <param name="tableName">
        /// <para>The name for the new <see cref="DataTable"/> to add to the <see cref="DataSet"/>.</para>
        /// </param>        
        /// <exception cref="System.ArgumentNullException">Any input parameter was <see langword="null"/> (<b>Nothing</b> in Visual Basic)</exception>
        /// <exception cref="System.ArgumentException">tableName was an empty string</exception>
        public virtual void LoadDataSet(DbCommand command,
                                        DataSet dataSet,
                                        string tableName)
        {
            LoadDataSet(command, dataSet, new string[] { tableName });
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> within the given <paramref name="transaction" /> and adds a new <see cref="DataTable"></see> to the existing <see cref="DataSet"></see>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="DbCommand"/> to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to load.</para>
        /// </param>
        /// <param name="tableName">
        /// <para>The name for the new <see cref="DataTable"/> to add to the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>        
        /// <exception cref="System.ArgumentNullException">Any input parameter was <see langword="null"/> (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="System.ArgumentException">tableName was an empty string.</exception>
        public virtual void LoadDataSet(DbCommand command,
                                        DataSet dataSet,
                                        string tableName,
                                        DbTransaction transaction)
        {
            LoadDataSet(command, dataSet, new string[] { tableName }, transaction);
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
        public virtual void LoadDataSet(DbCommand command,
                                        DataSet dataSet,
                                        string[] tableNames)
        {
            using (ConnectionWrapper wrapper = GetOpenConnection())
            {
                PrepareCommand(command, wrapper.Connection);
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
        public virtual void LoadDataSet(DbCommand command,
                                        DataSet dataSet,
                                        string[] tableNames,
                                        DbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            DoLoadDataSet(command, dataSet, tableNames);
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> with the results returned from a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure name to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        public virtual void LoadDataSet(string storedProcedureName,
                                        DataSet dataSet,
                                        string[] tableNames,
                                        params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                LoadDataSet(command, dataSet, tableNames);
            }
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> with the results returned from a stored procedure executed in a transaction.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the stored procedure in.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure name to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        public virtual void LoadDataSet(DbTransaction transaction,
                                        string storedProcedureName,
                                        DataSet dataSet,
                                        string[] tableNames,
                                        params object[] parameterValues)
        {
            using (DbCommand command = GetStoredProcCommand(storedProcedureName, parameterValues))
            {
                LoadDataSet(command, dataSet, tableNames, transaction);
            }
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from command text.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        public virtual void LoadDataSet(CommandType commandType,
                                        string commandText,
                                        DataSet dataSet,
                                        string[] tableNames)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                LoadDataSet(command, dataSet, tableNames);
            }
        }

        /// <summary>
        /// <para>Loads a <see cref="DataSet"/> from command text in a transaction.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command in.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        public void LoadDataSet(DbTransaction transaction,
                                CommandType commandType,
                                string commandText,
                                DataSet dataSet,
                                string[] tableNames)
        {
            using (DbCommand command = CreateCommandByCommandType(commandType, commandText))
            {
                LoadDataSet(command, dataSet, tableNames, transaction);
            }
        }

        /// <summary>
        /// <para>Opens a connection.</para>
        /// </summary>
        /// <returns>The opened connection.</returns>
        [Obsolete("Use GetOpenConnection instead.")]
        protected DbConnection OpenConnection()
        {
            return GetNewOpenConnection();
        }

        /// <summary>
        /// <para>Assigns a <paramref name="connection"/> to the <paramref name="command"/> and discovers parameters if needed.</para>
        /// </summary>
        /// <param name="command"><para>The command that contains the query to prepare.</para></param>
        /// <param name="connection">The connection to assign to the command.</param>
        protected static void PrepareCommand(DbCommand command,
                                             DbConnection connection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (connection == null) throw new ArgumentNullException("connection");

            command.Connection = connection;
        }

        /// <summary>
        /// <para>Assigns a <paramref name="transaction"/> to the <paramref name="command"/> and discovers parameters if needed.</para>
        /// </summary>
        /// <param name="command"><para>The command that contains the query to prepare.</para></param>
        /// <param name="transaction">The transaction to assign to the command.</param>
        protected static void PrepareCommand(DbCommand command,
                                             DbTransaction transaction)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (transaction == null) throw new ArgumentNullException("transaction");

            PrepareCommand(command, transaction.Connection);
            command.Transaction = transaction;
        }

        static void RollbackTransaction(IDbTransaction tran)
        {
            tran.Rollback();
        }

        /// <summary>
        /// Determines if the number of parameters in the command matches the array of parameter values.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> containing the parameters.</param>
        /// <param name="values">The array of parameter values.</param>
        /// <returns><see langword="true"/> if the number of parameters and values match; otherwise, <see langword="false"/>.</returns>
        protected virtual bool SameNumberOfParametersAndValues(DbCommand command,
                                                               object[] values)
        {
            int numberOfParametersToStoredProcedure = command.Parameters.Count;
            int numberOfValuesProvidedForStoredProcedure = values.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }

        /// <summary>
        /// Sets a parameter value.
        /// </summary>
        /// <param name="command">The command with the parameter.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        public virtual void SetParameterValue(DbCommand command,
                                              string parameterName,
                                              object value)
        {
            command.Parameters[BuildParameterName(parameterName)].Value = value ?? DBNull.Value;
        }

        /// <summary>
        /// Sets the RowUpdated event for the data adapter.
        /// </summary>
        /// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
        protected virtual void SetUpRowUpdatedEvent(DbDataAdapter adapter) { }

        /// <summary>
        /// <para>Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the <see cref="DataSet"/>.</para>
        /// </summary>        
        /// <param name="dataSet"><para>The <see cref="DataSet"/> used to update the data source.</para></param>
        /// <param name="tableName"><para>The name of the source table to use for table mapping.</para></param>
        /// <param name="insertCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Added"/></para></param>
        /// <param name="updateCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Modified"/></para></param>        
        /// <param name="deleteCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Deleted"/></para></param>        
        /// <param name="updateBehavior"><para>One of the <see cref="UpdateBehavior"/> values.</para></param>
        /// <param name="updateBatchSize">The number of database commands to execute in a batch.</param>
        /// <returns>number of records affected</returns>        
        public int UpdateDataSet(DataSet dataSet,
                                 string tableName,
                                 DbCommand insertCommand,
                                 DbCommand updateCommand,
                                 DbCommand deleteCommand,
                                 UpdateBehavior updateBehavior,
                                 int? updateBatchSize)
        {
            using (ConnectionWrapper wrapper = GetOpenConnection())
            {
                if (updateBehavior == UpdateBehavior.Transactional && Transaction.Current == null)
                {
                    DbTransaction trans = BeginTransaction(wrapper.Connection);
                    try
                    {
                        int rowsAffected = UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, trans, updateBatchSize);
                        CommitTransaction(trans);
                        return rowsAffected;
                    }
                    catch
                    {
                        RollbackTransaction(trans);
                        throw;
                    }
                }
                else
                {
                    if (insertCommand != null)
                    {
                        PrepareCommand(insertCommand, wrapper.Connection);
                    }
                    if (updateCommand != null)
                    {
                        PrepareCommand(updateCommand, wrapper.Connection);
                    }
                    if (deleteCommand != null)
                    {
                        PrepareCommand(deleteCommand, wrapper.Connection);
                    }

                    return DoUpdateDataSet(updateBehavior, dataSet, tableName,
                                           insertCommand, updateCommand, deleteCommand, updateBatchSize);
                }
            }
        }

        /// <summary>
        /// <para>Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the <see cref="DataSet"/>.</para>
        /// </summary>        
        /// <param name="dataSet"><para>The <see cref="DataSet"/> used to update the data source.</para></param>
        /// <param name="tableName"><para>The name of the source table to use for table mapping.</para></param>
        /// <param name="insertCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Added"/></para></param>
        /// <param name="updateCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Modified"/></para></param>        
        /// <param name="deleteCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Deleted"/></para></param>        
        /// <param name="updateBehavior"><para>One of the <see cref="UpdateBehavior"/> values.</para></param>
        /// <returns>number of records affected</returns>        
        public int UpdateDataSet(DataSet dataSet,
                                 string tableName,
                                 DbCommand insertCommand,
                                 DbCommand updateCommand,
                                 DbCommand deleteCommand,
                                 UpdateBehavior updateBehavior)
        {
            return UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, updateBehavior, null);
        }

        /// <summary>
        /// <para>Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the <see cref="DataSet"/> within a transaction.</para>
        /// </summary>        
        /// <param name="dataSet"><para>The <see cref="DataSet"/> used to update the data source.</para></param>
        /// <param name="tableName"><para>The name of the source table to use for table mapping.</para></param>
        /// <param name="insertCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Added"/>.</para></param>
        /// <param name="updateCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Modified"/>.</para></param>        
        /// <param name="deleteCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Deleted"/>.</para></param>        
        /// <param name="transaction"><para>The <see cref="IDbTransaction"/> to use.</para></param>
        /// <param name="updateBatchSize">The number of commands that can be executed in a single call to the database. Set to 0 to
        /// use the largest size the server can handle, 1 to disable batch updates, and anything else to set the number of rows.
        /// </param>
        /// <returns>Number of records affected.</returns>        
        public int UpdateDataSet(DataSet dataSet,
                                 string tableName,
                                 DbCommand insertCommand,
                                 DbCommand updateCommand,
                                 DbCommand deleteCommand,
                                 DbTransaction transaction,
                                 int? updateBatchSize)
        {
            if (insertCommand != null)
            {
                PrepareCommand(insertCommand, transaction);
            }
            if (updateCommand != null)
            {
                PrepareCommand(updateCommand, transaction);
            }
            if (deleteCommand != null)
            {
                PrepareCommand(deleteCommand, transaction);
            }

            return DoUpdateDataSet(UpdateBehavior.Transactional,
                                   dataSet, tableName, insertCommand, updateCommand, deleteCommand, updateBatchSize);
        }

        /// <summary>
        /// <para>Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the <see cref="DataSet"/> within a transaction.</para>
        /// </summary>        
        /// <param name="dataSet"><para>The <see cref="DataSet"/> used to update the data source.</para></param>
        /// <param name="tableName"><para>The name of the source table to use for table mapping.</para></param>
        /// <param name="insertCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Added"/>.</para></param>
        /// <param name="updateCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Modified"/>.</para></param>        
        /// <param name="deleteCommand"><para>The <see cref="DbCommand"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Deleted"/>.</para></param>        
        /// <param name="transaction"><para>The <see cref="IDbTransaction"/> to use.</para></param>
        /// <returns>Number of records affected.</returns>        
        public int UpdateDataSet(DataSet dataSet,
                                 string tableName,
                                 DbCommand insertCommand,
                                 DbCommand updateCommand,
                                 DbCommand deleteCommand,
                                 DbTransaction transaction)
        {
            return UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, transaction, null);
        }
        #region

        /// <summary>
        /// <para>Gets the parameter token used to delimit parameters for the SQL Server database.</para>
        /// </summary>
        /// <value>
        /// <para>The  symbol.</para>
        /// </value>
        protected abstract char ParameterToken
        {
            get;
        }

        /// <summary>
        /// 按特定数据库自己的形式重新书写CommandText
        /// </summary>
        /// <param name="command"></param>
        protected virtual void FormatCommandText(DbCommand command)
        { }

        /// <summary>
        /// <para>根据DataTable的结构创建 <see cref="DbCommand"/> for a datatable.</para>
        /// </summary>
        /// <param name="sourceTable"><para>Supply the schema of the talbe </para></param>
        /// <param name="tableName"><para>The name of the SQL table.</para></param>
        /// <param name="commandKind"><para>The kind of behavior when the command execute.</para></param>
        /// <returns><para>The <see cref="DbCommand"/> for the SQL query.</para></returns>        
        public DbCommand GetSqlStringCommand(DataTable sourceTable
           , string tableName, SqlStatementKind commandKind)
        {
            if (sourceTable == null)
                throw new ArgumentNullException("sourceTable");

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "tableName");

            DbCommand newCommand = NewCommand();

            // 如果DataTable中不包含主键，则有可能是没有提取过实际的表结构，需要强制提取
            if (sourceTable.PrimaryKey.Length == 0)
            {
                DataSet newDataSet = new DataSet();
                newDataSet.Tables.Add(GetOriginalEmptyTable(tableName));
                newDataSet.Tables[tableName].TableName = sourceTable.TableName;
                newDataSet.Merge(sourceTable);
                InitializeSQLAndParameter(newCommand, newDataSet.Tables[0], tableName, commandKind);
            }
            else
                InitializeSQLAndParameter(newCommand, sourceTable, tableName, commandKind);

            return newCommand;
        }

        /// <summary>
        /// 设置DBCommand的CommandText和相关的参数（根据数据集生成SQL命令时调用）
        /// </summary>
        /// <param name="command">需要设置的DBCommand</param>
        /// <param name="sourceTable"></param>
        /// <param name="tableName">对应的SQL表名</param>
        /// <param name="commandKind"></param>
        protected void InitializeSQLAndParameter(DbCommand command, DataTable sourceTable
           , string tableName, SqlStatementKind commandKind)
        {
            string split = ", ";
            int columnCount = sourceTable.Columns.Count;

            StringBuilder columnList = new StringBuilder();
            StringBuilder valueList = new StringBuilder();
            StringBuilder setValueList = new StringBuilder();

            foreach (DataColumn column in sourceTable.Columns)
            {
                if (--columnCount == 0)
                    split = "";

                if (column.AutoIncrement)
                    continue;

                columnList.Append(GetColumnString(column, split, ColStringType.col));
                valueList.Append(GetColumnString(column, split, ColStringType.val));
                setValueList.Append(GetColumnString(column, split, ColStringType.setval));

                // 添加保存列当前值的参数
                if ((commandKind == SqlStatementKind.Insert)
                   || (commandKind == SqlStatementKind.Update))
                {
                    AddCurrentValueParameter(command, column);
                }
            }

            DataColumn[] conditionColumns;
            if (sourceTable.PrimaryKey.Length > 0)
            {
                conditionColumns = sourceTable.PrimaryKey;
            }
            else
            {
                conditionColumns = new DataColumn[sourceTable.Columns.Count];
                sourceTable.Columns.CopyTo(conditionColumns, 0);
            }

            StringBuilder conditionList = new StringBuilder();
            split = " AND ";
            columnCount = conditionColumns.Length;

            foreach (DataColumn queryColumn in conditionColumns)
            {
                if (--columnCount == 0)
                    split = "";

                conditionList.Append(GetColumnString(queryColumn, split, ColStringType.qry));

                // 添加保存历史值的参数
                if ((commandKind == SqlStatementKind.Update)
                   || (commandKind == SqlStatementKind.Delete))
                {
                    AddOriginalValueParameter(command, queryColumn);
                }
            }

            command.CommandText = AssembleCommandText(tableName
               , columnList.ToString()
               , valueList.ToString()
               , setValueList.ToString()
               , conditionList.ToString()
               , commandKind);
        }

        /// <summary>
        /// 将SQL语句的各部分组合程完整的语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">字段列表</param>
        /// <param name="values">插入值语句</param>
        /// <param name="setValues">设置语句</param>
        /// <param name="condition">查询条件</param>
        /// <param name="commandKind">语句类型</param>
        /// <returns>完整的SQL语句</returns>
        private string AssembleCommandText(string tableName, string columns, string values
           , string setValues, string condition, SqlStatementKind commandKind)
        {
            string sqlCommand;

            switch (commandKind)
            {
                case SqlStatementKind.Insert:
                    sqlCommand = String.Format("insert into {0}({1}) values({2})"
                       , tableName
                       , columns
                       , values);
                    break;
                case SqlStatementKind.Update:
                    sqlCommand = String.Format("update {0} set {1} where {2}"
                       , tableName
                       , setValues
                       , condition);
                    break;
                case SqlStatementKind.Delete:
                    sqlCommand = String.Format("delete from {0} where {1}"
                       , tableName
                       , condition);
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }

            return sqlCommand;
        }

        /// <summary>
        /// SQL语句中的字符片断类型
        /// </summary>
        private enum ColStringType
        {
            /// <summary>
            /// 列名
            /// </summary>
            col,
            /// <summary>
            /// insert语句中的value参数
            /// </summary>
            val,
            /// <summary>
            /// update语句中的更新子句
            /// </summary>
            setval,
            /// <summary>
            /// 查询语句中的条件
            /// </summary>
            qry
        };

        /// <summary>
        /// 为组装SQL语句提供格式化好的字符串片断
        /// </summary>
        /// <param name="column">提供基础信息的数据列</param>
        /// <param name="split">需要添加的分割字符串</param>
        /// <param name="type">字符串片断的类型</param>
        /// <returns>格式化好的字符串片断</returns>
        private string GetColumnString(DataColumn column, string split, ColStringType type)
        {
            string result;
            // 生成语句时注意要在参数名后加上空格！
            switch (type)
            {
                case ColStringType.col:
                    result = column.ColumnName;
                    break;
                case ColStringType.val:
                    result = String.Format("{0}{1} ", GetParameterToken()/*ParameterToken*/, column.ColumnName);
                    break;
                case ColStringType.setval:
                    result = String.Format("{1} = {0}{1} ", GetParameterToken()/*ParameterToken*/
                                                                                                 , column.ColumnName);
                    break;
                case ColStringType.qry:
                    if (column.AllowDBNull)
                        result = String.Format("(({1} = {0}Original_{1} ) OR ({0}Original_{1}  IS NULL"
                           + " AND {1} IS NULL))"
                           , GetParameterToken()/*ParameterToken*/, column.ColumnName);
                    else
                        result = String.Format("({1} = {0}Original_{1} )"
                           , GetParameterToken()/*ParameterToken*/, column.ColumnName);
                    break;
                default:
                    result = "";
                    break;
            };

            return result + split;
        }

        /// <summary>
        /// 返回参数Token
        /// </summary>
        /// <returns></returns>
        protected virtual string GetParameterToken()
        {
            return ParameterToken.ToString();
        }

        /// <summary>
        /// 向DBCommand中添加参数（参数值使用DataRow对应列的当前值）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="column"></param>
        private void AddCurrentValueParameter(DbCommand command, DataColumn column)
        {
            AddInParameter(command
               , column.ColumnName
               , (DbType)Enum.Parse(typeof(DbType), column.DataType.Name)
               , column.ColumnName
               , DataRowVersion.Current);
        }

        /// <summary>
        /// 向DBCommand中添加参数（参数值使用DataRow对应列的原始值）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="column"></param>
        private void AddOriginalValueParameter(DbCommand command, DataColumn column)
        {
            AddParameter(command
               , String.Format("Original_{0}", column.ColumnName)
               , (DbType)Enum.Parse(typeof(DbType), column.DataType.Name)
               , 0
               , ParameterDirection.Input
               , false
               , 0
               , 0
               , column.ColumnName
               , DataRowVersion.Original
               , null);
        }

        /// <summary>
        /// 获取Schema与指定表在数据库中定义一致的空DataTable
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>只包含结构的空DataTable</returns>
        public virtual DataTable GetOriginalEmptyTable(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "SQLTableName");

            DataTable columnTable = GetTableColumnDefinitions(tableName);
            DataTable newTable = new DataTable(tableName);
            DataColumn newColumn;

            foreach (DataRow colRow in columnTable.Rows)
            {
                newColumn = new DataColumn();
                newColumn.AutoIncrement = (Convert.ToInt16(colRow["zzl"]) == 1);
                newColumn.AllowDBNull = (Convert.ToInt16(colRow["isnullable"]) == 1);
                newColumn.Caption = colRow["lm"].ToString();
                newColumn.ColumnName = colRow["lm"].ToString();
                newColumn.DataType = ConvertDbType2SystemType(colRow["type"].ToString().ToLower());

                // 如果列原始类型是Text，则保留默认的MaxLength = -1
                // 如果列原始类型是varchar或nvarchar，且length = 0，则也保留默认的MaxLength = -1
                if ((newColumn.DataType == typeof(string))
                     && !(colRow["type"].ToString().Contains("text") || (Convert.ToInt32(colRow["length"]) == 0)))
                    newColumn.MaxLength = Convert.ToInt32(colRow["length"].ToString());
                newTable.Columns.Add(newColumn);
            }

            DataRow[] keyRows = columnTable.Select("iskey = 1");
            DataColumn[] keys = new DataColumn[keyRows.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = newTable.Columns[keyRows[i]["lm"].ToString()];
            }
            newTable.PrimaryKey = keys;

            return newTable;
        }

        /// <summary>
        /// 获取指定SQL表所有列的详细定义
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public abstract DataTable GetTableColumnDefinitions(string tableName);

        /// <summary>
        /// 以数据库中的实际表结构重设DataTable的Schema
        /// </summary>
        /// <param name="currentTable"></param>
        /// <param name="tableName">对应的SQL表名</param>
        /// <returns>更新的列数</returns>
        public int ResetTableSchema(DataTable currentTable, string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException(Resources.ExceptionNullOrEmptyString, "SQLTableName");
            if (currentTable == null)
                throw new ArgumentNullException("CurrentDataTable");

            DataTable newTable = new DataTable();
            newTable = GetOriginalEmptyTable(tableName);

            return ResetTableSchema(currentTable, newTable);
        }

        /// <summary>
        /// 用新DataTable的主键、列类型的信息更新当前DataTable的Schema
        /// </summary>
        /// <param name="currentTable"></param>
        /// <param name="newTable"></param>
        /// <returns>更新的列数</returns>
        public int ResetTableSchema(DataTable currentTable, DataTable newTable)
        {
            if (currentTable == null)
                throw new ArgumentNullException("CurrentDataTable");
            if (newTable == null)
                throw new ArgumentNullException("OriginalDataTable");

            int columnIndex;
            int updateNumber = 0;
            bool hasData = (currentTable.Rows.Count > 0);

            foreach (DataColumn column in newTable.Columns)
            {
                columnIndex = currentTable.Columns.IndexOf(column.ColumnName);
                if (columnIndex >= 0)
                {
                    updateNumber++;
                    currentTable.Columns[columnIndex].AllowDBNull = column.AllowDBNull;
                    currentTable.Columns[columnIndex].AutoIncrement = column.AutoIncrement;
                    //// 有数据时不能更改列的数据类型
                    //if (!hasData)
                    currentTable.Columns[columnIndex].DataType = column.DataType;

                    if (currentTable.Columns[columnIndex].DataType == typeof(String))
                        currentTable.Columns[columnIndex].MaxLength = column.MaxLength;
                }
            }

            DataColumn[] keys = new DataColumn[newTable.PrimaryKey.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                if (currentTable.Columns.IndexOf(newTable.PrimaryKey[i].ColumnName) < 0)
                    throw new System.FieldAccessException("当前DataTable缺少主键所需的列");

                keys[i] = currentTable.Columns[newTable.PrimaryKey[i].ColumnName];
            }
            currentTable.PrimaryKey = keys;

            return updateNumber;
        }

        /// <summary>
        /// SQL中的数据类型转换成系统数据类型
        /// </summary>
        /// <param name="dbTypeName"></param>
        /// <returns>系统类型的一个实例</returns>
        protected virtual Type ConvertDbType2SystemType(string dbTypeName)
        {
            switch (dbTypeName)
            {
                case "bigint":
                    return typeof(Int64);
                case "binary":
                case "image":
                case "varbinary":
                    return typeof(Byte);
                case "bit":
                    return typeof(Boolean);
                case "char":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "varchar":
                case "varchar2":
                case "nvarchar2":
                case "text":
                    return typeof(String);
                case "datetime":
                case "smalldatetime":
                    return typeof(DateTime);
                case "":
                case "decimal":
                case "money":
                case "numeric":
                case "smallmoney":
                case "number":
                    return typeof(Decimal);
                case "float":
                case "real":
                    return typeof(Double);
                case "int":
                    return typeof(Int32);
                case "smallint":
                case "tinyint":
                    return typeof(Int16);
                default:
                    return typeof(String);
            }
        }

        /// <summary>
        /// 获取与Database一致的Command对象
        /// </summary>
        /// <returns></returns>
        public abstract DbCommand NewCommand();

        /// <summary>
        /// 添加参数后可能要改写语句中的参数写法、添加参数等
        /// </summary>
        /// <param name="command"></param>
        protected virtual void ResetCommandAndParameters(IDbCommand command)
        { }

        /// <summary>
        /// 从CommandText类型的command中分解出所有参数名称
        /// </summary>
        /// <param name="command"></param>
        /// <param name="replaceString">用来替换命令中参数的字符串</param>
        /// <returns>第一个字符串是调整后的CommandText</returns>
        protected Collection<string> DeriveAllParameterNameFromText(IDbCommand command, string replaceString)
        {
            Collection<string> result = new Collection<string>();

            if (command == null)
                throw new ArgumentNullException("command");
            // 命令中还有ParameterToken需要处理
            if ((command.CommandType == CommandType.TableDirect)
               || (command.CommandType == CommandType.StoredProcedure)
               || (!command.CommandText.Contains(ParameterToken.ToString())))
            {
                result.Add(command.CommandText);
                return result;
            }

            bool needReplace = !String.IsNullOrEmpty(replaceString);

            // 逐个字符处理
            StringBuilder newText = new StringBuilder();
            StringBuilder newPara = new StringBuilder();
            bool partOfPara = false; // 标记当前处理的字符是否属于参数名的一部分
            bool qutoStarted = false; // 标记当前处理的字符是否在引号内
            foreach (char c in command.CommandText)
            {
                if ((c == (char)34) || (c == (char)39)) // 双引号 // 单引号 
                {
                    newText.Append(c);
                    qutoStarted = !qutoStarted;
                }
                else if (qutoStarted)
                {
                    newText.Append(c);
                }
                else if (((c >= 48) && (c <= 57))
                   || ((c >= 65) && (c <= 90))
                   || ((c >= 97) && (c <= 122))
                   || (c == '_')) // 数字、字母、下划线
                {
                    if (partOfPara)
                        newPara.Append(c);
                    else
                        newText.Append(c);
                }
                else if (c == ParameterToken) // 找到参数的标记
                {
                    if (partOfPara)
                        throw new ArgumentException("处理错误");
                    partOfPara = true;
                    newPara = new StringBuilder();
                    newPara.Append(c);
                }
                else // 其它字符
                {
                    if (partOfPara)
                    {
                        partOfPara = false;
                        result.Add(newPara.ToString());
                        if (needReplace)
                            newText.Append(replaceString);
                        else
                            newText.Append(newPara);
                    }

                    newText.Append(c);
                }
            }

            // 最后的字符都属于参数
            if (partOfPara)
            {
                partOfPara = false;
                result.Add(newPara.ToString());
                if (needReplace)
                    newText.Append(replaceString);
                else
                    newText.Append(newPara);
            }

            result.Add(newText.ToString());

            return result;
        }
        #endregion

        #region  do logs
        /// <summary>
        /// 存储过程输出参数的标记
        /// </summary>
        protected abstract string ProcedureOutputFlag { get; }

        /// <summary>
        /// 将CommandText转换成FormatString
        /// </summary>
        /// <param name="currentCmd">包含CommandText和必要参数的DbCommand</param>
        /// <returns>FormatString</returns>
        public abstract string GetCommandTextFormatString(DbCommand currentCmd);

        /// <summary>
        /// 将参数值按顺序组合成String数组
        /// </summary>
        /// <param name="parameters">参数集合</param>
        /// <returns>参数值数组</returns>      
        public string[] GetValueArrayFromParameters(IDataParameterCollection parameters)
        {
            StringBuilder values = new StringBuilder();
            foreach (IDbDataParameter para in parameters)
            {
                // 因为是用参数的值替换命令中参数的位置，所以只需要输入参数
                if ((para.Direction == ParameterDirection.Input)
                   || (para.Direction == ParameterDirection.InputOutput))
                {
                    values.Append(FormatParameterValue(para));
                    values.Append('~');
                }
            }

            return values.ToString().Split('~');
        }

        /// <summary>
        /// 用DataRow初始化参数集合，并将参数值按顺序组合成String数组
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <param name="row">用来赋值的DataRow</param>
        /// <returns>参数值数组</returns>
        public string[] GetValueArrayFromParameters(IDataParameterCollection parameters, DataRow row)
        {
            // 给参数赋值
            SetParaValueFromDataRow(parameters, row);
            return GetValueArrayFromParameters(parameters);
        }

        /// <summary>
        /// 将文本型的Command命令转换成合适的、可执行的SQL语法命令
        /// </summary>
        /// <param name="currentCmd">要转换命令的DbCommand</param>
        /// <returns>SQL String</returns>
        public string ConvertCommandText2Sql(DbCommand currentCmd)
        {
            // 用参数值替换语句中参数的位置
            return String.Format(GetCommandTextFormatString(currentCmd),
               GetValueArrayFromParameters(currentCmd.Parameters));
        }

        /// <summary>
        /// 生成存储过程的输入参数串
        /// </summary>
        /// <param name="parameters">参数列表</param>
        /// <returns></returns>
        protected abstract string GetProcedureParametersString(IDataParameterCollection parameters);

        /// <summary>
        /// 将存储过程型的Command命令转换成合适的、可执行的SQL语法命令
        /// </summary>
        /// <param name="command">需要转换的Command</param>
        /// <returns></returns>
        public abstract string ConvertStoredProcedure2Sql(DbCommand command);

        /// <summary>
        /// 通过DataRow给参数集合中的每个参数赋值
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="row"></param>
        private void SetParaValueFromDataRow(IDataParameterCollection parameters, DataRow row)
        {
            foreach (IDbDataParameter para in parameters)
            {
                if ((para.Direction == ParameterDirection.Input)
                   || (para.Direction == ParameterDirection.InputOutput))
                {
                    try
                    {
                        para.Value = row[para.SourceColumn, para.SourceVersion];
                    }
                    catch
                    {
                        para.Value = Convert.DBNull;
                    }
                }
            }
        }

        /// <summary>
        /// 将参数值格式化成正确的形式(主要是为字符型的加上引号)
        /// </summary>
        /// <param name="para">参数</param>
        /// <returns></returns>
        protected string FormatParameterValue(IDbDataParameter para)
        {
            if (para.Value == DBNull.Value)
                return "NULL";

            switch (para.DbType)
            {
                case DbType.AnsiString:
                case DbType.Guid:
                case DbType.String:
                    return String.Format("'{0}'", para.Value.ToString());
                default:
                    return para.Value.ToString();
            }
        }

        #endregion

        /// <summary>
        /// Returns the starting index for parameters in a command.
        /// </summary>
        /// <returns>The starting index for parameters in a command.</returns>
        protected virtual int UserParametersStartIndex()
        {
            return 0;
        }

        /// <summary>
        ///		This is a helper class that is used to manage the lifetime of a connection for various
        ///		Execute methods. We needed this class to support implicit transactions created with
        ///		the <see cref="TransactionScope"/> class. In this case, the various Execute methods
        ///		need to use a shared connection instead of a new connection for each request in order
        ///		to prevent a distributed transaction.
        /// </summary>
        protected class ConnectionWrapper : IDisposable
        {
            readonly DbConnection connection;
            readonly bool disposeConnection;

            /// <summary>
            ///		Create a new "lifetime" container for a <see cref="DbConnection"/> instance.
            /// </summary>
            /// <param name="connection">The connection</param>
            /// <param name="disposeConnection">
            ///		Whether or not to dispose of the connection when this class is disposed.
            ///	</param>
            public ConnectionWrapper(DbConnection connection,
                                     bool disposeConnection)
            {
                this.connection = connection;
                this.disposeConnection = disposeConnection;
            }

            /// <summary>
            ///		Gets the actual connection.
            /// </summary>
            public DbConnection Connection
            {
                get { return connection; }
            }

            /// <summary>
            ///		Dispose the wrapped connection, if appropriate.
            /// </summary>
            public void Dispose()
            {
                if (disposeConnection)
                    connection.Dispose();
            }
        }
    }
}
