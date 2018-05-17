using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using OracleDataAccessClient = Oracle.DataAccess.Client;


namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Convert the sql of SqlServer to the sql of Oracle
    /// 由于原来是适合SqlServer数据库而编写的sql，代码中有部分通过sql直接访问数据库的方式，所以这里要把sql从SqlServer转为Oracle
    /// </summary>
    public class CommandSqlServer2Oracle
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        enum DBType
        {
            /// <summary>
            /// unknown db
            /// </summary>
            Null,

            /// <summary>
            /// sql server db
            /// </summary>
            SqlServer,

            /// <summary>
            /// oracle db
            /// </summary>
            Oracle
        }

        private const string RefCursorName = "o_result";//默认输出名称都是o_result开头的

        /// <summary>
        /// .ctor
        /// </summary>
        public CommandSqlServer2Oracle()
        { }

        #region sql from sqlserver ===> oracle
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public DbCommand ConvertToOracle(Database db, DbCommand command)
        {
            string commandText = command.CommandText.Trim().TrimEnd(';');

            if (GetDBType(db) == DBType.Oracle)
            {
                command.CommandText = ConvertSql(commandText);
                command.CommandText = ConvertParameter(command);
            }

            return command;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public string ConvertToOracle(Database db, string commandText)
        {
            commandText = commandText.Trim().TrimEnd(';');

            if (GetDBType(db) == DBType.Oracle)
            {
                commandText = ConvertSql(commandText);
            }

            return commandText;
        }


        /// <summary>
        /// 由于有的sql存在参数@..的样式，所以这里将@..换成具体参数的值的形式
        /// </summary>
        /// <returns></returns>
        private string ConvertParameter(DbCommand command)
        {
            string commandText = command.CommandText;

            if (commandText.IndexOf('@') < 0)
            {
                return commandText;
            }

            List<DbParameter> listParameter = new List<DbParameter>();

            foreach (DbParameter parameter in command.Parameters)
            {
                listParameter.Add(parameter);
            }

            #region old
            //string strParameter = string.Empty;
            //bool beginAddFlag = false;

            //foreach (char c in commandText)
            //{
            //    if (c == '@')
            //    {
            //        strParameter = "";
            //        beginAddFlag = true;
            //    }
            //    if (beginAddFlag)
            //    {
            //        if (c != ' ')
            //        {
            //            strParameter += c;
            //        }
            //        else
            //        {
            //            beginAddFlag = false;
            //            if (strParameter != "")
            //            {
            //                if (!listParameterName.Contains(strParameter))
            //                {
            //                    listParameterName.Add(strParameter);
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion

            foreach (DbParameter parameter in listParameter)
            {
                string parameterName = parameter.ParameterName;
                string parameterValue = parameter.Value.ToString();
                string name = parameterName.Substring(2, parameterName.Length - 2);
                commandText = commandText.Replace("@" + name, "'" + parameterValue + "'");
            }

            command.Parameters.Clear();

            return commandText;
        }

        private string ConvertSql(string commandText)
        {
            string sql = "SELECT SYSDATE FROM DUAL";
            commandText = commandText.Replace("select getdate()", sql);
            sql = "TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS')";
            commandText = commandText.Replace("convert(varchar, getdate(), 120)", sql);
            sql = "TO_CHAR(SYSDATE, 'YYYY.MM.DD')";
            commandText = commandText.Replace("convert(varchar, getdate(), 102)", sql);
            sql = "TO_CHAR(SYSDATE, 'YYYY-MM-DD')";
            commandText = commandText.Replace("convert(varchar, getdate(), 23)", sql);
            commandText = commandText.Replace("dbo.", "");
            commandText = commandText.Replace("(nolock)", "");

            commandText = commandText.Replace("isnull", "nvl");
            commandText = commandText.Replace("substring", "substr");
            commandText = commandText.Replace("\r", "");
            commandText = commandText.Replace("\n", "");
            commandText = commandText.Replace("[", "");
            commandText = commandText.Replace("]", "");
            commandText = commandText.Replace("getdate()", "sysdate");

            return commandText;
        }

        /// <summary>
        /// 判断连接的数据库类型
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        private DBType GetDBType(Database db)
        {
            if (db is OracleDatabase)
            {
                return DBType.Oracle;
            }
            else if (db is SqlDatabase)
            {
                return DBType.SqlServer;
            }
            return DBType.Null;
        }
        #endregion

        #region out parameter for ref cursor
        /// <summary>
        /// 根据存储过程的名字，判断需要ref cursor类型的输出参数的个数
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns></returns>
        public List<string> GetRefCursorList(string procedureName)
        {
            procedureName = procedureName.ToUpper();
            List<string> refCursorList = new List<string>();
            if (GetRefCursorListInner2().Contains(procedureName))
            {
                refCursorList.Add(RefCursorName);
                refCursorList.Add(RefCursorName + "1");
            }
            else if (GetRefCursorListInner3().Contains(procedureName))
            {
                refCursorList.Add(RefCursorName);
                refCursorList.Add(RefCursorName + "1");
                refCursorList.Add(RefCursorName + "2");
            }
            else if (GetRefCursorListInner4().Contains(procedureName))
            {
                refCursorList.Add(RefCursorName);
                refCursorList.Add(RefCursorName + "1");
                refCursorList.Add(RefCursorName + "2");
                refCursorList.Add(RefCursorName + "3");
            }
            else if (GetRefCursorListInner5().Contains(procedureName))
            {
                refCursorList.Add(RefCursorName);
                refCursorList.Add(RefCursorName + "1");
                refCursorList.Add(RefCursorName + "2");
                refCursorList.Add(RefCursorName + "3");
                refCursorList.Add(RefCursorName + "4");
            }
            else
            {
                //默认情况下输出参数是o_result
                refCursorList.Add(RefCursorName);
            }
            return refCursorList;
        }

        /// <summary>
        /// 有两个ref cursor类型输出参数的存储过程
        /// </summary>
        /// <returns></returns>
        private List<string> GetRefCursorListInner2()
        {
            List<string> refCursorList = new List<string>();
            //包名 + 存储过程名
            refCursorList.Add("EMRPROC.usp_GetTemplatePersonGroup".ToUpper());
            refCursorList.Add("EMRPROC.usp_GetUserInfo".ToUpper());
            refCursorList.Add("EMRPROC.usp_QueryHistoryPatients".ToUpper());
            refCursorList.Add("EMR_CommonNote.usp_GetCommonNoteSite".ToUpper());

            refCursorList.Add("EMRPROC.usp_selectallusers2".ToUpper());
            return refCursorList;
        }

        /// <summary>
        /// 有三个ref cursor类型输出参数的存储过程
        /// </summary>
        /// <returns></returns>
        private List<string> GetRefCursorListInner3()
        {
            List<string> refCursorList = new List<string>();
            //包名 + 存储过程名
            refCursorList.Add("EMR_CONSULTATION.usp_GetConsultationData".ToUpper());
            refCursorList.Add("EMR_CommonNote.usp_GetDetailCommonNote".ToUpper());
            refCursorList.Add("EMR_CommonNote.usp_GetDetailInCommonNote".ToUpper());
            return refCursorList;
        }

        /// <summary>
        /// 有四个ref cursor类型输出参数的存储过程
        /// </summary>
        /// <returns></returns>
        private List<string> GetRefCursorListInner4()
        {
            List<string> refCursorList = new List<string>();
            //包名 + 存储过程名
            refCursorList.Add("EMRPROC.usp_GetIemInfo".ToUpper());
            refCursorList.Add("IEM_MAIN_PAGE.usp_getieminfo_new".ToUpper());
            return refCursorList;
        }


        /// <summary>
        /// 有五个ref cursor类型输出参数的存储过程
        /// </summary>
        /// <returns></returns>
        private List<string> GetRefCursorListInner5()
        {
            List<string> refCursorList = new List<string>();
            //包名 + 存储过程名
            refCursorList.Add("IEM_MAIN_PAGE.usp_getieminfo_2012".ToUpper());
            refCursorList.Add("IEM_MAIN_PAGE_SX.usp_getieminfo_sx".ToUpper());
            return refCursorList;
        }
        #endregion

        #region Convert OracleType

        /// <summary>
        /// 转换参数
        /// </summary>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="param"></param>
        public void ConvertParameter(Database db, IDbCommand command, OracleParameter param)
        {
            ConvertParameterType(db, param);
            ConvertParameterType2(db, command, param);
        }

        private void ConvertParameterType(Database db, OracleParameter param)
        {
            if (param == null)
            {
                return;
            }
            if (GetDBType(db) == DBType.Oracle)
            {
                if (param.DbType == DbType.Binary)
                {
                    //param.DbType = DbType.Object;
                    param.OracleType = OracleType.Blob;
                }
            }
        }

        private void ConvertParameterType2(Database db, IDbCommand command, OracleParameter param)
        {
            if (command == null || param == null)
            {
                return;
            }
            if (command.CommandText.Trim().ToUpper() == "EMRPROC.USP_INSERTIMAGE")
            {
                if (param.ParameterName.Trim().ToUpper() == "V_CONTENT")
                {
                    param.OracleType = OracleType.Clob;
                }
            }
            else if (command.CommandText.Trim().ToUpper() == "EMRPROC.USP_EMR_PATRECFILE")
            {
                if (param.ParameterName.Trim().ToUpper() == "V_FILECONTENT")
                {
                    param.OracleType = OracleType.Clob;
                }
            }
        }

        /// <summary>
        /// 针对包含Blob字段的特殊处理
        /// 【由于使用微软企业库导致使用了Blob字段的存储过程报错：不能将System.Byte[] 绑定到 Blob。。。。，所以这里要对含有Blob字段的存储过程做特殊处理】
        /// </summary>
        /// <param name="db"></param>
        /// <param name="command"></param>
        /// <param name="rowsAffected">受影响的行数</param>
        /// <returns></returns>
        public bool ExecuteContainBlob(Database db, IDbCommand command, out int rowsAffected)
        {
            bool isContainBlob = false;
            rowsAffected = 0;
            if (db is OracleDatabase)
            {
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    if (((OracleParameter)command.Parameters[i]).OracleType.ToString() == "Blob")
                    {
                        isContainBlob = true;
                        break;
                    }
                }

                if (isContainBlob && command.CommandType == CommandType.StoredProcedure)
                {
                    //OracleDataAccessClient.OracleConnection conn = new OracleDataAccessClient.OracleConnection(db.ConnectionString);
                    //conn.Open();
                    using (OracleDataAccessClient.OracleConnection conn = new OracleDataAccessClient.OracleConnection(db.ConnectionString))
                    {
                        OracleDataAccessClient.OracleCommand commandNew = new OracleDataAccessClient.OracleCommand(command.CommandText, conn);
                        commandNew.CommandType = CommandType.StoredProcedure;
                        CreateOracleParameter(command, commandNew);
                        rowsAffected = commandNew.ExecuteNonQuery();
                        //commandNew.Dispose();
                        //conn.Close();
                    }
                }
            }
            return isContainBlob;
        }

        private void CreateOracleParameter(IDbCommand command, OracleDataAccessClient.OracleCommand commandNew)
        {
            foreach (OracleParameter para in command.Parameters)
            {
                OracleDataAccessClient.OracleDbType oracleType = ConvertOracleType(para.OracleType);
                OracleDataAccessClient.OracleParameter paraNew = new OracleDataAccessClient.OracleParameter(para.ParameterName, oracleType);
                paraNew.Value = para.Value;
                commandNew.Parameters.Add(paraNew);
            }
        }

        #endregion

        OracleDataAccessClient.OracleDbType ConvertOracleType(System.Data.OracleClient.OracleType type)
        {
            switch (type)
            {
                case System.Data.OracleClient.OracleType.Int16:
                case System.Data.OracleClient.OracleType.UInt16:
                    return global::Oracle.DataAccess.Client.OracleDbType.Int16;
                case System.Data.OracleClient.OracleType.Int32:
                case System.Data.OracleClient.OracleType.UInt32:
                    return global::Oracle.DataAccess.Client.OracleDbType.Int32;
                case System.Data.OracleClient.OracleType.Double:
                    return global::Oracle.DataAccess.Client.OracleDbType.Double;
                case System.Data.OracleClient.OracleType.Float:
                    return global::Oracle.DataAccess.Client.OracleDbType.Double;
                case System.Data.OracleClient.OracleType.Byte:
                    return global::Oracle.DataAccess.Client.OracleDbType.Byte;
                case System.Data.OracleClient.OracleType.Char:
                    return global::Oracle.DataAccess.Client.OracleDbType.Char;
                case System.Data.OracleClient.OracleType.Blob:
                    return global::Oracle.DataAccess.Client.OracleDbType.Blob;
                case System.Data.OracleClient.OracleType.BFile:
                    return global::Oracle.DataAccess.Client.OracleDbType.BFile;
                case System.Data.OracleClient.OracleType.Clob:
                    return global::Oracle.DataAccess.Client.OracleDbType.Clob;
                case System.Data.OracleClient.OracleType.Cursor:
                    return global::Oracle.DataAccess.Client.OracleDbType.RefCursor;
                case System.Data.OracleClient.OracleType.DateTime:
                    return global::Oracle.DataAccess.Client.OracleDbType.Date;
                case System.Data.OracleClient.OracleType.LongRaw:
                    return global::Oracle.DataAccess.Client.OracleDbType.LongRaw;
                case System.Data.OracleClient.OracleType.LongVarChar:
                    return global::Oracle.DataAccess.Client.OracleDbType.Long;
                case System.Data.OracleClient.OracleType.NChar:
                    return global::Oracle.DataAccess.Client.OracleDbType.NChar;
                case System.Data.OracleClient.OracleType.NClob:
                    return global::Oracle.DataAccess.Client.OracleDbType.NClob;
                case System.Data.OracleClient.OracleType.Number:
                    return global::Oracle.DataAccess.Client.OracleDbType.Decimal;
                case System.Data.OracleClient.OracleType.NVarChar:
                    return global::Oracle.DataAccess.Client.OracleDbType.NVarchar2;
                case System.Data.OracleClient.OracleType.Raw:
                    return global::Oracle.DataAccess.Client.OracleDbType.Raw;
                case System.Data.OracleClient.OracleType.RowId:
                    return global::Oracle.DataAccess.Client.OracleDbType.NVarchar2;
                case System.Data.OracleClient.OracleType.Timestamp:
                    return global::Oracle.DataAccess.Client.OracleDbType.TimeStamp;
                case System.Data.OracleClient.OracleType.VarChar:
                    return global::Oracle.DataAccess.Client.OracleDbType.Varchar2;
                default:
                    throw new NotSupportedException();
            }
        }
    }

}
