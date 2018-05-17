using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Windows.Forms;
using System.Data;
using System.Configuration;

namespace app
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public class DbConect
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string _ConnStr = string.Empty;
        private static DbConect _Instance = null;

        /// <summary>
        /// 获取实例
        /// </summary>
        internal static DbConect getInstance()
        {
            if (_Instance == null)
                _Instance = new DbConect();
            return _Instance;
        }

        /// <summary>
        /// 设置连接数据库字符串
        /// </summary>
        internal void ResetSetConnect()
        {
            try
            {
                _ConnStr = @"Data Source=192.168.1.202/zhhis;User Id=hthis;Password=""rb087eb"";";
                            //@"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= 192.168.1.202)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=hczyy)));User Id=hthis;Password=""rb087eb"";";
            }
            catch { _ConnStr = ""; }
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        internal DbConect()
        {
            ResetSetConnect();            
        }

        public DbConect(string connectString)
        {
            _ConnStr = connectString;
        }
        /// <summary>
        /// 测试连接数据库
        /// </summary>
        /// <returns></returns>
        internal bool TestConnect()
        {
            try
            {
                OracleConnection OraConn = new OracleConnection(_ConnStr);
                OraConn.Open();
                OraConn.Close();
                return true;
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message, "提示");
                return false;
            }
        }

        internal DataTable ExecuteQuery(string SQLString, params OracleParameter[] cmdParms)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OracleConnection conn = new OracleConnection(_ConnStr))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    OracleCommand cmd = new OracleCommand(SQLString, conn);
                    if (cmdParms != null)
                        cmd.Parameters.Add(cmdParms);
                    OracleDataAdapter oda = new OracleDataAdapter(cmd);

                    oda.Fill(dt);
                    conn.Close();
                    cmd.Dispose();

                    return dt;
                }
            }
            catch (OracleException e)
            {
                MessageBox.Show(e.Message, "ExecuteQuery");
                return dt;
            }
        }
    }
}