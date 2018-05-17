using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
#pragma warning disable 0618
namespace DrectSoft.Core
{
    /// <summary>
    /// Sql数据访问
    /// </summary>
    public class SqlDataAccess : IDataAccess
    {
        private LoggingSettings LoggingSetting
        {
            get
            {
                if (_loggingSetting == null)
                    _loggingSetting = GetLoggingSettings();
                return _loggingSetting;
            }
        }
        private LoggingSettings _loggingSetting;

        private Database m_DBFactory;
        private Hashtable m_CacheDataTable = new Hashtable();
        private string SelectIdentity = "select @@identity";
        private bool m_UseSingleConnection;
        private DbConnection m_SingleConnection;
        private SHA1 m_SHA1;
        private UnicodeEncoding m_UniEncoding;
        private bool m_UseTransaction;
        private DbTransaction m_Transaction;


        //private log4net.ILog _log;

        #region ctors
        /// <summary>
        /// Ctor
        /// </summary>
        public SqlDataAccess()
        {
            m_DBFactory = DatabaseFactory.CreateDatabase();
            //_log = log4net.LogManager.GetLogger(typeof(SqlDataAccess));
        }

        /// <summary>
        /// Ctor2
        /// </summary>
        public SqlDataAccess(string dbName)
        {
            m_DBFactory = DatabaseFactory.CreateDatabase(dbName);
            //_log = log4net.LogManager.GetLogger(typeof(SqlDataAccess));
        }
        #endregion

        #region IDataAccess Members

        /// <summary>
        /// 使用单一Connection执行语句前调用(Update方法不支持)
        /// </summary>
        public void BeginUseSingleConnection()
        {
            m_UseSingleConnection = true;
            if (m_SingleConnection != null)
                m_SingleConnection.Close();

            m_SingleConnection = m_DBFactory.CreateConnection();
            m_SingleConnection.Open();
        }

        /// <summary>
        /// 开始事务，批量执行多个语句或表更新前使用。需要手工提交或回滚事务
        /// </summary>
        public void BeginTransaction()
        {
            m_UseTransaction = true;
            BeginUseSingleConnection();
            m_Transaction = m_SingleConnection.BeginTransaction();
        }

        /// <summary>
        /// 批量执行出错时手工回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            if (m_Transaction != null)
                m_Transaction.Rollback();
            m_UseTransaction = false;
            EndUseSingleConnection();
        }

        /// <summary>
        /// 批量执行成功后手工提交事务
        /// </summary>
        public void CommitTransaction()
        {
            if (m_Transaction != null)
                m_Transaction.Commit();
            m_UseTransaction = false;
            EndUseSingleConnection();
        }

        /// <summary>
        /// 使用单一Connection执行语句完成后调用
        /// </summary>
        public void EndUseSingleConnection()
        {
            if ((m_UseSingleConnection) && (m_SingleConnection != null))
                m_SingleConnection.Close();

            m_UseSingleConnection = false;
        }

        #region ExecuteDataTable

        /// <summary>
        /// 返回DataTable,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return DoExecuteDataTable(GetCommand(commandText, commandType)).Tables[0];
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, false);
        }

        /// <summary>
        /// 返回DataTable, 未缓存
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, SqlParameter[] parameters)
        {
            return ExecuteDataTable(commandText, parameters, CommandType.Text);
        }

        /// <summary>
        /// 返回数据表,带参数,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            return DoExecuteDataTable(GetCommand(commandText, commandType, parameters)).Tables[0];
        }

        /// <summary>
        /// 指定是否重新从数据库读取数据
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <param name="cached">重新加载缓存的信息</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, bool cached)
        {
            return ExecuteDataTable(commandText, cached, CommandType.Text);
        }

        /// <summary>
        /// 指定是否重新从数据库读取数据,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="cached"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, bool cached, CommandType commandType)
        {
            DataTable dt = null;
            string key = "";
            bool canBeCached = false;
            if (cached)
            {
                key = ComputeHashCodeOfString(commandText);
                if (m_CacheDataTable.ContainsKey(key))
                    dt = m_CacheDataTable[key] as DataTable;
                else
                    canBeCached = true;
            }

            if (dt == null)
            {
                dt = DoExecuteDataTable(GetCommand(commandText, commandType)).Tables[0];
                //if (cached && (!m_CacheDataTable.ContainsKey(commandText)))
                if (canBeCached && !m_CacheDataTable.ContainsKey(key)) //xll 2013-06-08  判断不包含key时添加
                {
                    m_CacheDataTable.Add(key, dt);
                }
            }

            return dt;
        }

        /// <summary>
        /// 需要反复使用RowFilter对同一DataTable进行过滤时可以使用此方法。
        /// 系统将对每次调用返回的DataTable进行缓存
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="rowFilter"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, string rowFilter)
        {
            if (rowFilter == null)
                rowFilter = "";
            string key1 = ComputeHashCodeOfString(commandText + rowFilter);
            if (m_CacheDataTable.ContainsKey(key1))
                return m_CacheDataTable[key1] as DataTable;
            else
            {
                DataTable dt;
                string key2 = ComputeHashCodeOfString(commandText);
                if (m_CacheDataTable.ContainsKey(key2))
                {
                    dt = m_CacheDataTable[key2] as DataTable;
                }
                else
                {
                    dt = DoExecuteDataTable(GetCommand(commandText, CommandType.Text)).Tables[0];
                    m_CacheDataTable.Add(key2, dt);
                }
                if (!String.IsNullOrEmpty(rowFilter))
                {
                    dt.DefaultView.RowFilter = rowFilter;
                    dt = dt.DefaultView.ToTable();
                    m_CacheDataTable.Add(key1, dt);
                }
                return dt;
            }
        }
        #endregion

        #region ExecuteNoneQuery

        /// <summary>
        /// 取得满足条件的记录
        /// </summary>
        /// <param name="commandText">取得数据集的Sql语句</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="cached">是否缓存</param>
        /// <returns></returns>
        public DataRow GetRecord(string commandText, string filter, bool cached)
        {
            DataTable dt = ExecuteDataTable(commandText, cached);
            DataRow[] dv = dt.Select(filter);
            if (dv.Length == 0)
                return null;
            else
                return dv[0];
        }

        /// <summary>
        /// 取得满足条件的记录集
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="filter"></param>
        /// <param name="cached"></param>
        /// <returns></returns>
        public DataRow[] GetRecords(string commandText, string filter, bool cached)
        {
            DataTable dt = ExecuteDataTable(commandText, cached);
            DataRow[] dv = dt.Select(filter);
            return dv;
        }

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="commandText"></param>
        public void ExecuteNoneQuery(string commandText)
        {
            ExecuteNoneQuery(commandText, CommandType.Text);
        }

        /// <summary>
        /// 执行Sql语句,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        public void ExecuteNoneQuery(string commandText, CommandType commandType)
        {
            DoExecuteNoneQuery(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// 执行Sql语句,带参数,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        public void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            if (parameters == null)
            {
                parameters = new SqlParameter[] { };
            }
            DoExecuteNoneQuery(GetCommand(commandText, commandType, parameters));
        }

        /// <summary>
        /// 执行Sql语句,带参数
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public void ExecuteNoneQuery(string commandText, SqlParameter[] parameters)
        {
            ExecuteNoneQuery(commandText, parameters, CommandType.Text);
        }

        /// <summary>
        /// 执行Sql语句,带参数,指定CommandType,返回Identity
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="identityValue"></param>
        public void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, CommandType commandType, out int identityValue)
        {
            if (parameters == null)
            {
                parameters = new SqlParameter[] { };
            }

            string plusCommandText = commandText + "\r\n" + SelectIdentity;

            identityValue = int.Parse(DoExecuteDataTable(GetCommand(plusCommandText, commandType, parameters)).Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 执行Sql语句,并返回Identity值
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="identityValue"></param>
        public void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, out int identityValue)
        {
            ExecuteNoneQuery(commandText, parameters, CommandType.Text, out identityValue);
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 返回数据集,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return DoExecuteDataTable(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, CommandType.Text);
        }

        /// <summary>
        /// 返回数据集,带参数,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            if (parameters == null)
            {
                parameters = new SqlParameter[] { };
            }
            return DoExecuteDataTable(GetCommand(commandText, commandType, parameters));
        }

        /// <summary>
        /// 返回数据集2
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, SqlParameter[] parameters)
        {
            return ExecuteDataSet(commandText, parameters, CommandType.Text);
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// 返回DataReader, 指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return DoExecuteReader(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, CommandType.Text);
        }

        /// <summary>
        /// 返回DataReader2, 指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            if (parameters == null)
            {
                parameters = new SqlParameter[] { };
            }
            return DoExecuteReader(GetCommand(commandText, commandType, parameters));
        }

        /// <summary>
        /// 返回DataReader 2
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, SqlParameter[] parameters)
        {
            return ExecuteReader(commandText, parameters, CommandType.Text);
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Scalar, 指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return DoExecuteScalar(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// Scalar
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, CommandType.Text);
        }

        /// <summary>
        /// Scalar2, 指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            if (parameters == null)
            {
                return ExecuteScalar(commandText);
            }
            return DoExecuteScalar(GetCommand(commandText, CommandType.Text, parameters));
        }

        /// <summary>
        /// Scalar 2
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, SqlParameter[] parameters)
        {
            return ExecuteScalar(commandText, parameters, CommandType.Text);
        }

        #endregion

        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="changedTable"></param>
        /// <param name="sqlTableName"></param>
        /// <param name="needUpdateSchema"></param>
        /// <returns></returns>
        public int UpdateTable(DataTable changedTable, string sqlTableName, bool needUpdateSchema)
        {
            if (changedTable == null)
                return -1;
            if (String.IsNullOrEmpty(sqlTableName))
                return -1;
            DataTable updateTable = changedTable.Copy();

            if (needUpdateSchema)
            {
                ResetTableSchema(updateTable, sqlTableName);
            }

            return DoUpdateTable(updateTable, sqlTableName);
        }

        /// <summary>
        /// 更新表,此方法专用于科室匹配中的更新操作 
        /// </summary>
        /// <param name="changedTable"></param>
        /// <param name="sqlTableName"></param>
        /// <param name="needUpdateSchema"></param>
        /// <returns></returns>
        public int UpdateTable(DataTable changedTable, string sqlTableName, bool needUpdateSchema, DataTable DeleteDt)
        {
            if (changedTable == null)
                return -1;
            if (String.IsNullOrEmpty(sqlTableName))
                return -1;
            DataTable updateTable = changedTable.Copy();

            if (needUpdateSchema)
            {
                ResetTableSchema(updateTable, sqlTableName);
            }

            return DoUpdateTable(updateTable, sqlTableName, DeleteDt);
        }


        /// <summary>
        /// ResetSchema
        /// </summary>
        /// <param name="originalTable"></param>
        /// <param name="sqlTableName"></param>
        public void ResetTableSchema(DataTable originalTable, string sqlTableName)
        {
            if (originalTable == null)
                return;
            if (String.IsNullOrEmpty(sqlTableName))
                return;
            // 在同步结构前默认Table是区分大小写的
            originalTable.CaseSensitive = true;
            m_DBFactory.ResetTableSchema(originalTable, sqlTableName);
            // 如果DataTable中包含自增列,则要设置自增列的种子
            foreach (DataColumn col in originalTable.Columns)
            {
                if (col.AutoIncrement)
                {
                    // 通过遍历的方法找到最大序号
                    long maxNo = 0;
                    long tempNo;
                    foreach (DataRow row in originalTable.Rows)
                    {
                        tempNo = Convert.ToInt64(row[col.ColumnName], CultureInfo.CurrentCulture);
                        if (tempNo > maxNo)
                            maxNo = tempNo;
                    }
                    col.AutoIncrementSeed = maxNo + 1;

                    break;
                }
            }
        }

        /// <summary>
        /// GetTableColumnDefinitions
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetTableColumnDefinitions(string tableName)
        {
            return m_DBFactory.GetTableColumnDefinitions(tableName);
        }

        /// <summary>
        /// 取得指定的数据连接的配置信息
        /// </summary>
        /// <param name="dbName">数据连接名</param>
        /// <returns>保存配置信息的Hashtable</returns>
        public static Hashtable GetConnectionInfo(string dbName)
        {
            IConfigurationSource source = new SystemConfigurationSource();
            DatabaseConfigurationView configView = new DatabaseConfigurationView(source);

            return ParseConnectionString(
               configView.GetConnectionStringSettings(dbName).ConnectionString);
        }

        /// <summary>
        /// 取得数据连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetDbConnection()
        {
            return m_DBFactory.CreateConnection();
        }

        /// <summary>
        /// 取得服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return (DateTime)ExecuteScalar("select getdate()");
        }

        /// <summary>
        /// 得到Database
        /// </summary>
        /// <returns></returns>
        public Database GetDatabase()
        {
            return m_DBFactory;
        }

        #endregion

        #region private method
        private LoggingSettings GetLoggingSettings()
        {
            IConfigurationSource source = new SystemConfigurationSource();
            return LoggingSettings.GetLoggingSettings(source);
        }

        private DataSet DoExecuteDataTable(DbCommand command)
        {
            try
            {
                return m_DBFactory.ExecuteDataSet(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                LogSqlStatements(command);
                if (!m_UseTransaction)//Add By wwj for Close Connection
                {
                    command.Connection.Close();
                }
            }
        }

        private void DoExecuteNoneQuery(DbCommand command)
        {
            try
            {
                m_DBFactory.ExecuteNonQuery(command);
            }
            catch
            {
                throw;
            }
            finally
            {
                LogSqlStatements(command);
                if (!m_UseTransaction)//Add By wwj for Close Connection
                {
                    command.Connection.Close();
                }
            }
        }

        private IDataReader DoExecuteReader(DbCommand command)
        {
            try
            {
                return m_DBFactory.ExecuteReader(command);
            }
            catch
            {
                throw;
            }
            finally
            {
                LogSqlStatements(command);
            }
        }

        private object DoExecuteScalar(DbCommand command)
        {
            try
            {
                return m_DBFactory.ExecuteScalar(command);
            }
            catch
            {
                throw;
            }
            finally
            {
                LogSqlStatements(command);
                if (!m_UseTransaction)//Add By wwj for Close Connection
                {
                    command.Connection.Close();
                }
            }
        }
        /// <summary>
        /// 科室匹配中的，对EMRDEPT2HIS表的增删改操作的方法 
        /// edit by ywk 2012年4月16日10:27:08
        /// </summary>
        /// <param name="changedTable"></param>
        /// <param name="sqlTable"></param>
        /// <returns></returns>
        private int DoUpdateTable(DataTable changedTable, string sqlTable)
        {
            DbCommand insertCommand =
               m_DBFactory.GetSqlStringCommand(changedTable, sqlTable, SqlStatementKind.Insert);

            DbCommand updateCommand =
               m_DBFactory.GetSqlStringCommand(changedTable, sqlTable, SqlStatementKind.Update);

            DbCommand deleteCommand =
               m_DBFactory.GetSqlStringCommand(changedTable, sqlTable, SqlStatementKind.Delete);


            DataSet changedDataSet = new DataSet();
            changedDataSet.Locale = System.Globalization.CultureInfo.CurrentCulture;
            changedDataSet.Tables.Add(changedTable.Copy());

            //记录错误信息
            string errorMessage = string.Empty;
            string operation = string.Empty;

            try
            {
                //return m_DBFactory.UpdateDataSet(changedDataSet
                //                                , changedTable.TableName
                //                                , insertCommand, updateCommand, deleteCommand
                //                                , UpdateBehavior.Standard);

                int rowCount = 0;
                foreach (DataRow dr in changedTable.Rows)
                {
                    try
                    {
                        if (dr.RowState == DataRowState.Added)//Insert
                        {
                            operation = "Add TableName:" + sqlTable;

                            string sql = insertCommand.CommandText;

                            List<SqlParameter> paraList = new List<SqlParameter>();
                            foreach (DataColumn dc in changedTable.Columns)
                            {
                                if (dc.AutoIncrement) continue;
                                SqlParameter para = new SqlParameter("@" + dc.ColumnName, SqlDbType.VarChar);
                                para.Value = dr[dc.ColumnName].ToString();
                                paraList.Add(para);
                            }

                            foreach (DataColumn dc in changedTable.PrimaryKey)
                            {
                                operation += " " + dc.ColumnName + ":" + dr[dc.ColumnName].ToString();
                            }

                            this.ExecuteNoneQuery(sql, paraList.ToArray(), CommandType.Text);
                            rowCount++;
                        }
                        else if (dr.RowState == DataRowState.Modified)//Update
                        {
                            operation = "Update TableName:" + sqlTable;

                            string sql = updateCommand.CommandText;

                            List<SqlParameter> paraList = new List<SqlParameter>();
                            foreach (DataColumn dc in changedTable.Columns)
                            {
                                if (dc.AutoIncrement) continue;
                                SqlParameter para = new SqlParameter("@" + dc.ColumnName, SqlDbType.VarChar);
                                para.Value = dr[dc.ColumnName].ToString();
                                paraList.Add(para);
                            }

                            foreach (DataColumn dc in changedTable.PrimaryKey)
                            {
                                SqlParameter para = new SqlParameter("@Original_" + dc.ColumnName, SqlDbType.VarChar);
                                para.Value = dr[dc.ColumnName].ToString();
                                paraList.Add(para);

                                operation += " " + dc.ColumnName + ":" + dr[dc.ColumnName].ToString();
                            }

                            this.ExecuteNoneQuery(sql, paraList.ToArray(), CommandType.Text);
                            rowCount++;
                        }

                        //edit by ywk 2012年4月16日10:42:34
                        else if (dr.RowState == DataRowState.Deleted)//delete 暂时不处理
                        {
                            ////获得已经被标记为删除的行的数据  
                            //DataTable dtDeletedData=changedTable.GetChanges(DataRowState.Deleted);

                            //operation = "Delete TableName:" + sqlTable;
                            //string sql = deleteCommand.CommandText;
                            //List<SqlParameter> paraList = new List<SqlParameter>();
                            //foreach (DataColumn dc in changedTable.Columns)
                            //{
                            //    if (dc.AutoIncrement) continue;
                            //    SqlParameter para = new SqlParameter("@" + dc.ColumnName, SqlDbType.VarChar);
                            //    para.Value = dr[dc.ColumnName].ToString();
                            //    paraList.Add(para);
                            //}

                            //foreach (DataColumn dc in changedTable.PrimaryKey)
                            //{
                            //    SqlParameter para = new SqlParameter("@Original_" + dc.ColumnName, SqlDbType.VarChar);
                            //    para.Value = dr[dc.ColumnName].ToString();
                            //    paraList.Add(para);

                            //    operation += " " + dc.ColumnName + ":" + dr[dc.ColumnName].ToString();
                            //}
                            //this.ExecuteNoneQuery(sql, paraList.ToArray(), CommandType.Text);
                            //rowCount++;

                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage += ex.Message + operation + " ******* ";
                    }
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    throw new Exception(errorMessage);
                }

                return rowCount;
            }
            catch
            {
                throw;
            }
            finally
            {
                //LogSqlStatements(changedDataSet, changedTable.TableName
                //   , insertCommand, updateCommand, deleteCommand);

                if (!m_UseTransaction)//Add By wwj for Close Connection
                {
                    if (insertCommand.Connection != null) insertCommand.Connection.Close();
                    if (updateCommand.Connection != null) updateCommand.Connection.Close();
                    if (deleteCommand.Connection != null) deleteCommand.Connection.Close();
                }
            }
        }


        /// <summary>
        /// 科室匹配中的，对EMRDEPT2HIS表的增删改操作的方法 
        /// 为更新方法加个重载！
        /// edit by ywk 2012年4月16日11:46:50 
        /// </summary>
        /// <param name="changedTable"></param>
        /// <param name="sqlTable"></param>
        /// <returns></returns>
        private int DoUpdateTable(DataTable changedTable, string sqlTable, DataTable DeleteData)
        {
            DbCommand insertCommand =
               m_DBFactory.GetSqlStringCommand(changedTable, sqlTable, SqlStatementKind.Insert);

            DbCommand updateCommand =
               m_DBFactory.GetSqlStringCommand(changedTable, sqlTable, SqlStatementKind.Update);

            DbCommand deleteCommand =
               m_DBFactory.GetSqlStringCommand(changedTable, sqlTable, SqlStatementKind.Delete);

            DataSet changedDataSet = new DataSet();
            changedDataSet.Locale = System.Globalization.CultureInfo.CurrentCulture;
            changedDataSet.Tables.Add(changedTable.Copy());

            //记录错误信息
            string errorMessage = string.Empty;
            string operation = string.Empty;
            try
            {
                int rowCount = 0;
                foreach (DataRow dr in changedTable.Rows)
                {
                    try
                    {
                        if (dr.RowState == DataRowState.Added)//Insert
                        {
                            operation = "Add TableName:" + sqlTable;

                            string sql = insertCommand.CommandText;

                            List<SqlParameter> paraList = new List<SqlParameter>();
                            foreach (DataColumn dc in changedTable.Columns)
                            {
                                if (dc.AutoIncrement) continue;
                                SqlParameter para = new SqlParameter("@" + dc.ColumnName, SqlDbType.VarChar);
                                para.Value = dr[dc.ColumnName].ToString();
                                paraList.Add(para);
                            }

                            foreach (DataColumn dc in changedTable.PrimaryKey)
                            {
                                operation += " " + dc.ColumnName + ":" + dr[dc.ColumnName].ToString();
                            }

                            this.ExecuteNoneQuery(sql, paraList.ToArray(), CommandType.Text);
                            rowCount++;
                        }
                        else if (dr.RowState == DataRowState.Modified)//Update
                        {
                            operation = "Update TableName:" + sqlTable;

                            string sql = updateCommand.CommandText;

                            List<SqlParameter> paraList = new List<SqlParameter>();
                            foreach (DataColumn dc in changedTable.Columns)
                            {
                                if (dc.AutoIncrement) continue;
                                SqlParameter para = new SqlParameter("@" + dc.ColumnName, SqlDbType.VarChar);
                                para.Value = dr[dc.ColumnName].ToString();
                                paraList.Add(para);
                            }

                            foreach (DataColumn dc in changedTable.PrimaryKey)
                            {
                                SqlParameter para = new SqlParameter("@Original_" + dc.ColumnName, SqlDbType.VarChar);
                                para.Value = dr[dc.ColumnName].ToString();
                                paraList.Add(para);

                                operation += " " + dc.ColumnName + ":" + dr[dc.ColumnName].ToString();
                            }

                            this.ExecuteNoneQuery(sql, paraList.ToArray(), CommandType.Text);
                            rowCount++;
                        }

                        //edit by ywk 2012年4月16日10:42:34
                        else if (dr.RowState == DataRowState.Deleted)//delete 暂时不处理
                        {
                            //获得已经被标记为删除的行的数据  此法获不到数据
                            //DataTable dtDeletedData = changedTable.GetChanges(DataRowState.Deleted);
                            operation = "Delete TableName:" + sqlTable;
                            string sql = deleteCommand.CommandText;
                            List<SqlParameter> paraList = new List<SqlParameter>();
                            //删除配对科室和配对模板分开处理
                            if (sqlTable == "EMRDEPT2HIS")//配对科室数据表
                            {
                                for (int i = 0; i < DeleteData.Rows.Count; i++)
                                {
                                    string delsql = string.Format(@"delete from {0} where EMR_DEPT_ID='{1}' and HIS_DEPT_ID='{2}' ",
                                        sqlTable, DeleteData.Rows[i]["EMR_DEPT_ID"], DeleteData.Rows[i]["HIS_DEPT_ID"]);
                                    this.ExecuteNoneQuery(delsql);
                                    rowCount++;
                                }
                            }

                            if (sqlTable == "TEMPLET2HISDEPT")//配对模板表
                            {
                                for (int i = 0; i < DeleteData.Rows.Count; i++)
                                {
                                    string delsql = string.Format(@"delete from {0} where TEMPLETID='{1}' and HIS_DEPT_ID='{2}' ",
                                           sqlTable, DeleteData.Rows[i]["TEMPLETID"], DeleteData.Rows[i]["HIS_DEPT_ID"]);
                                    this.ExecuteNoneQuery(delsql);
                                    rowCount++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage += ex.Message + operation + " ******* ";
                    }
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    throw new Exception(errorMessage);
                }

                return rowCount;
            }
            catch
            {
                throw;
            }
            finally
            {
                //LogSqlStatements(changedDataSet, changedTable.TableName
                //   , insertCommand, updateCommand, deleteCommand);

                if (!m_UseTransaction)//Add By wwj for Close Connection
                {
                    if (insertCommand.Connection != null) insertCommand.Connection.Close();
                    if (updateCommand.Connection != null) updateCommand.Connection.Close();
                    if (deleteCommand.Connection != null) deleteCommand.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 创建合适DbCommandWrapper
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>符合命令类型的DBCommandWrapper</returns>
        private DbCommand GetCommand(string commandText, CommandType commandType)
        {
            DbCommand command;

            if (commandType == CommandType.Text)
                command = m_DBFactory.GetSqlStringCommand(commandText);
            else if (commandType == CommandType.StoredProcedure)
            {
                command = m_DBFactory.GetStoredProcCommand(commandText);
            }
            else
                throw new ArgumentException("该方法不支持Table型的Command");

            if (m_UseSingleConnection)
                command.Connection = m_SingleConnection;
            return command;
        }

        /// <summary>
        /// 创建带参数的DBCommandWrapper
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>符合命令类型的DBCommandWrapper</returns>
        private DbCommand GetCommand(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            DbCommand command = GetCommand(commandText, commandType);

            foreach (DbParameter para in parameters)
                m_DBFactory.AddParameter(command, para.ParameterName, para.DbType, para.Direction
                   , para.SourceColumn, para.SourceVersion, para.Value);

            return command;
        }

        /// <summary>
        /// 解析连接字符串的参数
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static Hashtable ParseConnectionString(string connectionString)
        {
            Hashtable pairs = new Hashtable();

            string[] splitString = connectionString.Split(';');
            for (int index = 0; index < splitString.Length; ++index)
            {
                string[] nameValuePair = splitString[index].Split('=');
                if (nameValuePair.Length == 2)
                    pairs.Add(nameValuePair[0], nameValuePair[1]);
            }
            return pairs;
        }

        private string ComputeHashCodeOfString(string source)
        {
            if (source == null)
                source = "";
            if (m_SHA1 == null)
                m_SHA1 = new SHA1CryptoServiceProvider();
            if (m_UniEncoding == null)
                m_UniEncoding = new UnicodeEncoding();
            byte[] result = m_SHA1.ComputeHash(m_UniEncoding.GetBytes(source));
            return m_UniEncoding.GetString(result);
        }
        #endregion

        #region
        /// <summary>
        /// 判断是否可以连接到数据库
        /// </summary>
        /// <returns>true：可以连接上数据库 false: 不能连接上数据库</returns>
        public bool IsConnectDB()
        {
            bool isConnectDB = true;
            if (m_DBFactory.ConnectionString.Trim() != "")
            {
                if (!PingHost.CmdPing(GetIP(m_DBFactory.ConnectionString)))
                {
                    isConnectDB = false;
                }
                else
                {
                    if (m_DBFactory is OracleDatabase)//Oracle数据库
                    {
                        OracleConnection conn;

                        conn = new OracleConnection(m_DBFactory.ConnectionString);
                        try
                        {
                            conn.Open();
                        }
                        catch (Exception ex)
                        {
                            isConnectDB = false;
                        }
                        finally
                        {
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    else if (m_DBFactory is SqlDatabase)
                    {
                        SqlConnection conn;

                        conn = new SqlConnection(m_DBFactory.ConnectionString);
                        try
                        {
                            conn.Open();
                        }
                        catch (Exception ex)
                        {
                            isConnectDB = false;
                        }
                        finally
                        {
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                }
            }
            else
            {
                isConnectDB = false;
            }
            return isConnectDB;
        }

        private string GetIP(string connectionString)
        {
            string ip = string.Empty;
            int beginIndex = connectionString.IndexOf("HOST=") + "HOST=".Length;
            if (beginIndex > 0)
            {
                //endIndex == -1 表示连接字符串连接的是SqlServer数据库 
                //例如：Database=HisServer;Server=192.168.2.182;user id=sa;password=sasasa
                //
                //endIndex >= 0 表示连接字符串连接的是Oracle数据库 
                //例如：Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=emr)));User Id=dba;Password=sa;
                int endIndex = connectionString.IndexOf(')', beginIndex);
                if (endIndex == -1)
                {
                    endIndex = connectionString.IndexOf("Server=", beginIndex);
                    beginIndex = endIndex + "Server=".Length;
                    endIndex = connectionString.IndexOf(";", endIndex);
                }
                ip = connectionString.Substring(beginIndex, endIndex - beginIndex).Trim();
            }
            return ip;
        }
        #endregion

        #region do sql logging

        /// <summary>
        /// 将IDbCommand转换成SQL语句记录下来
        /// </summary>
        /// <param name="command"></param>
        private void LogSqlStatements(DbCommand command)
        {
            if (CheckNeedLog())
            {
                if (command.CommandType == CommandType.Text)
                {
                    DoLogger(m_DBFactory.ConvertCommandText2Sql(command));
                }
                else if (command.CommandType == CommandType.StoredProcedure)
                {
                    DoLogger(m_DBFactory.ConvertStoredProcedure2Sql(command));
                }
            }
        }

        /// <summary>
        /// 将更新数据集的操作转换成SQL语句记录下来
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <param name="insertCommand"></param>
        /// <param name="updateCommand"></param>
        /// <param name="deleteCommand"></param>
        private void LogSqlStatements(DataSet dataSet, string tableName, DbCommand insertCommand,
           DbCommand updateCommand, DbCommand deleteCommand)
        {
            DataTable table = dataSet.Tables[tableName];

            StringBuilder insertStrings = new StringBuilder();
            StringBuilder updateStrings = new StringBuilder();
            StringBuilder deleteStrings = new StringBuilder();

            string insertFormat = m_DBFactory.GetCommandTextFormatString(insertCommand);
            string updateFormat = m_DBFactory.GetCommandTextFormatString(updateCommand);
            string deleteFormat = m_DBFactory.GetCommandTextFormatString(deleteCommand);

            foreach (DataRow row in table.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        insertStrings.AppendFormat(insertFormat
                           , m_DBFactory.GetValueArrayFromParameters(insertCommand.Parameters, row));
                        insertStrings.Append("\r\n");
                        break;
                    case DataRowState.Modified:
                        updateStrings.AppendFormat(updateFormat
                           , m_DBFactory.GetValueArrayFromParameters(updateCommand.Parameters, row));
                        updateStrings.Append("\r\n");
                        break;
                    case DataRowState.Deleted:
                        deleteStrings.AppendFormat(deleteFormat
                           , m_DBFactory.GetValueArrayFromParameters(deleteCommand.Parameters, row));
                        deleteStrings.Append("\r\n");
                        break;
                }
            }

            DoLogger(insertStrings.ToString()
               + updateStrings.ToString()
               + deleteStrings.ToString());
        }

        /// <summary>
        /// 执行日志记录操作
        /// </summary>
        /// <param name="message"></param>
        private void DoLogger(string message)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.EventId = 100;
            logEntry.Priority = 2;
            logEntry.Message = message;
            logEntry.Categories.Add("SQLTrace");

            Logger.Write(logEntry);
        }

        /// <summary>
        /// 检查是否需要记录执行的SQL语句
        /// </summary>
        /// <returns></returns>
        private bool CheckNeedLog()
        {
            if (LoggingSetting != null)
                return LoggingSetting.TracingEnabled;
            else
                return false;
        }
        #endregion
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public void ExecuteSqlTran(ArrayList SQLStringList)
        {
            //  IDbCommand cmd = _DbConnection.CreateCommand();
            string connectionString = m_DBFactory.ConnectionString;
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection;
                OracleTransaction tx = connection.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (Exception E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。正确返回"OK",错误返回错误信息
        /// </summary>
        /// <param name="al"></param>
        /// <returns>正确返回"OK",错误返回错误信息</returns>
        public string ExecuteSqlTran2(ArrayList SQLStringList)
        {
            string connectionString = m_DBFactory.ConnectionString;
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = connection;
                OracleTransaction tx = connection.BeginTransaction();
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return "OK";
                }
                catch (Exception E)
                {
                    tx.Rollback();
                    return E.Message;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
    }
}


