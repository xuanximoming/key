using Newtonsoft.Json;
using Oracle.DataAccess.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
#pragma warning disable 0618
/***********************************************************************************************************************别文进插件*/
namespace DrectSoft.DSSqlHelper
{
    /// <summary>
    /// 功能描述：数据库操作类
    /// 对外访问接口类
    /// 创 建 者：bwj 
    /// 创建日期：20121022
    /// </summary>
    public sealed class DS_SqlHelper
    {
        static string strPath = "./adcemr.exe";
        #region 当前IDbCommand中的参数列表，用于外部获取存储过程返回的参数  暂时只开放执行ExecuteNonQueryInTran方法中返回的参数 Add by wwj 2013-02-22
        public static List<IDataParameter> CmdParameterList
        {
            get
            {
                List<IDataParameter> dbParameterList = new List<IDataParameter>();
                foreach (IDataParameter par in ParameterCollection)
                {
                    dbParameterList.Add(par);
                }
                return dbParameterList;
            }
        }
        public static IDataParameterCollection ParameterCollection { get; set; }
        #endregion

        private static string _DefualtPakageName = "";
        /// <summary>
        /// 默认存储过程所在包
        /// </summary>
        public static string DefualtPakageName
        {
            get { return _DefualtPakageName; }
            set
            {
                _DefualtPakageName = value;
            }
        }

        private static string _DbProviderName = "";
        /// <summary>
        /// 数据库驱动类型
        /// </summary>
        public static string DbProviderName
        {
            get
            {
                return _DbProviderName;
            }
            set
            {
                _DbProviderName = value;
                AppDbProviderFactory = DbProviderFactories.GetFactory(value);
            }
        }

        private static string _strcn = "";

        /// <summary>
        /// 联接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return _strcn;
            }
            set
            {
                _strcn = value;

            }
        }

        //private static bool WebServ = false;
        private static DbProviderFactory _DbProviderFactory = null;
        /// <summary>
        /// 数据库工厂
        /// </summary>
        private static DbProviderFactory AppDbProviderFactory
        {
            get
            {
                return _DbProviderFactory;
            }
            set
            {
                _DbProviderFactory = value;
            }
        }

        public static void DefaultDataAccess()
        {
            try
            {
                CreateSqlHelper();
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 事务
        /// </summary>
        private static IDbTransaction _IDbTransaction = null;//事务
        /// <summary>
        /// 事务
        /// </summary>
        public static IDbTransaction AppDbTransaction
        {
            get
            {
                return _IDbTransaction;
            }

        }
        private static DbDataAccess _DbDataAccess = null;//供事务用
        /// <summary>
        /// 供事务用工厂对象
        /// </summary>
        private static DbDataAccess AppDbDataAccess
        {
            get
            {
                return _DbDataAccess;
            }
            set
            {
                _DbDataAccess = value;
            }
        }
        /// <summary>
        /// 初始化一个Sqlhelper
        /// </summary>
        /// <param name="dbProviderName"></param>
        public static void CreateSqlHelper()
        {
            try
            {
                if (System.IO.File.Exists(strPath))
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(strPath);
                    ConfigurationSection obj = config.GetSection("dataConfiguration") as ConfigurationSection;
                    string connectionStringsSection = obj.ElementInformation.Properties["defaultDatabase"].Value.ToString();
                    DbProviderName = config.ConnectionStrings.ConnectionStrings[connectionStringsSection].ProviderName;
                    ConnectionString = config.ConnectionStrings.ConnectionStrings[connectionStringsSection].ConnectionString;

                }
                else
                {
                    ConfigurationSection obj = ConfigurationManager.GetSection("dataConfiguration") as ConfigurationSection;
                    string connectionStringsSection = obj.ElementInformation.Properties["defaultDatabase"].Value.ToString();
                    DbProviderName = ConfigurationManager.ConnectionStrings[connectionStringsSection].ProviderName;
                    ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringsSection].ConnectionString;

                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 初始化一个Sqlhelper
        /// </summary>
        /// <param name="dbProviderName"></param>
        public static void CreateSqlHelper(string dbProviderName)
        {
            try
            {
                if (System.IO.File.Exists(strPath))
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(strPath);
                    ConfigurationSection obj = config.GetSection("dataConfiguration") as ConfigurationSection;
                    string connectionStringsSection = obj.ElementInformation.Properties["defaultDatabase"].Value.ToString();
                    if (dbProviderName != "")
                    {
                        DbProviderName = dbProviderName;
                    }
                    else
                    {
                        DbProviderName = config.ConnectionStrings.ConnectionStrings[connectionStringsSection].ProviderName;
                    }
                    ConnectionString = config.ConnectionStrings.ConnectionStrings[connectionStringsSection].ConnectionString;

                }
                else
                {
                    ConfigurationSection obj = ConfigurationManager.GetSection("dataConfiguration") as ConfigurationSection;
                    string connectionStringsSection = obj.ElementInformation.Properties["defaultDatabase"].Value.ToString();
                    if (dbProviderName != "")
                    {
                        DbProviderName = dbProviderName;
                    }
                    else
                    {
                        DbProviderName = ConfigurationManager.ConnectionStrings[connectionStringsSection].ProviderName;
                    }
                    ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringsSection].ConnectionString;

                }


            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 初始化一个Sqlhelper
        /// </summary>
        /// <param name="dbProviderName"></param>
        public static void CreateSqlHelper(string dbProviderName, string dbConnetionString)
        {
            try
            {
                if (System.IO.File.Exists(strPath))
                {
                    if (dbProviderName != "")
                    {
                        DbProviderName = dbProviderName;
                    }
                    else
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(strPath);
                        ConfigurationSection obj = config.GetSection("dataConfiguration") as ConfigurationSection;
                        string connectionStringsSection = obj.ElementInformation.Properties["defaultDatabase"].Value.ToString();
                        DbProviderName = config.ConnectionStrings.ConnectionStrings[connectionStringsSection].ProviderName;
                    }
                    if (dbConnetionString != "")
                    {
                        ConnectionString = dbConnetionString;
                    }
                    else
                    {
                        Configuration config = ConfigurationManager.OpenExeConfiguration(strPath);
                        ConfigurationSection obj = config.GetSection("dataConfiguration") as ConfigurationSection;
                        string connectionStringsSection = obj.ElementInformation.Properties["defaultDatabase"].Value.ToString();
                        ConnectionString = config.ConnectionStrings.ConnectionStrings[connectionStringsSection].ConnectionString;
                    }
                }
                else
                {
                    if (dbProviderName != "")
                    {
                        DbProviderName = dbProviderName;
                    }
                    else
                    {
                        ConfigurationSection obj = ConfigurationManager.GetSection("dataConfiguration") as ConfigurationSection;
                        string connectionStringsSection = obj.ElementInformation.Properties["defaultDatabase"].Value.ToString();
                        DbProviderName = ConfigurationManager.ConnectionStrings[connectionStringsSection].ProviderName;
                    }
                    if (dbConnetionString != "")
                    {
                        ConnectionString = dbConnetionString;
                    }
                    else
                    {
                        ConfigurationSection obj = ConfigurationManager.GetSection("dataConfiguration") as ConfigurationSection;
                        string connectionStringsSection = obj.ElementInformation.Properties["defaultDatabase"].Value.ToString();
                        ConnectionString = ConfigurationManager.ConnectionStrings[connectionStringsSection].ConnectionString;
                    }
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 初始化一个Sqlhelper  Add by wwj 2013-1-6
        /// </summary>
        /// <param name="dbName"></param>
        public static void CreateSqlHelperByDBName(string dbName)
        {
            try
            {
                if (System.IO.File.Exists(strPath))
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(strPath);
                    ConfigurationSection obj = config.GetSection("dataConfiguration") as ConfigurationSection;
                    DbProviderName = config.ConnectionStrings.ConnectionStrings[dbName].ProviderName;
                    ConnectionString = config.ConnectionStrings.ConnectionStrings[dbName].ConnectionString;
                }
                else
                {
                    DbProviderName = ConfigurationManager.ConnectionStrings[dbName].ProviderName;
                    ConnectionString = ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// xll 通过DA.xml来配置数据库连接字符串 2013-09-15
        /// </summary>
        /// <param name="dbName"></param>
        public static void CreateSqlHelperByDBNameFormDA(string dbName)
        {
            try
            {
                string dafile = AppDomain.CurrentDomain.BaseDirectory + @"\DA.xml";
                if (!File.Exists(dafile))
                {
                    throw new Exception(dafile + "文件丢失");
                }
                XmlDocument doc = new XmlDocument();
                doc.Load(dafile);
                XmlNodeList nodeList = doc.GetElementsByTagName("connectionStrings");
                if (nodeList == null || nodeList.Count == 0)
                {
                    throw new Exception(dafile + "文件缺失");
                }
                XmlNode node = null;
                foreach (XmlNode item in nodeList)
                {
                    if (item.Attributes["name"].Value == dbName)
                    {
                        node = item;
                        break;
                    }
                }

                string conStr = node.Attributes["conString"] == null ||
                    node.Attributes["conString"].Value == null ? "" : node.Attributes["conString"].Value;
                string conStrDriver = node.Attributes["providerName"] == null ||
                        node.Attributes["providerName"].Value == null ? "" : node.Attributes["providerName"].Value;
                if (string.IsNullOrEmpty(conStr) || string.IsNullOrEmpty(conStrDriver))
                {
                    throw new Exception("数据库连接字符串丢失");
                }
                DbProviderName = conStrDriver;
                ConnectionString = conStr;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 检查工厂类与数据库连接字符串是指定
        /// </summary>
        private static void CheckInfo()
        {
            try
            {
                if (AppDbProviderFactory == null)
                {
                    throw new Exception("DS_SqlHelper:AppDbProviderFactory为Null");
                }
                if (ConnectionString == null || ConnectionString.Trim() == String.Empty)
                {
                    throw new Exception("DS_SqlHelper:ConnectionString未曾指定");
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 执行带参的sql语句或存储过程
        /// 适用Insert,Delete,Update
        /// </summary>
        /// <param name="commandtext">Sql语句或存储过程</param>
        /// <param name="par">参数数组</param>
        /// <param name="commandtype">命令类型</param>
        /// <returns>大于0代表执行成功,小于等于0执行失败</returns>
        public static int ExecuteNonQuery(string commandText, DbParameter[] pars, CommandType commandtype)
        {
            try
            {
                //变量
                int result = 0;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteNonQuery(commandText, pars, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 执行带参查询返回结果表
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-04</date>
        /// <param name="commandText"></param>
        /// <param name="pars"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string commandText, List<DbParameter> pars, CommandType commandtype)
        {
            try
            {
                //变量
                int result = 0;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                DbParameter[] _pars = new DbParameter[pars.Count];
                for (int i = 0; i < pars.Count; i++)
                {
                    _pars[i] = pars[i] as DbParameter;
                }
                result = aDbDataAccess.ExecuteNonQuery(commandText, _pars, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 执行带参查询返回结果表
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-15</date>
        /// <param name="commandText"></param>
        /// <param name="pars"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string commandText, List<OracleParameter> pars, CommandType commandtype)
        {
            try
            {
                //变量
                int result = 0;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                DbParameter[] _pars = new DbParameter[pars.Count];
                for (int i = 0; i < pars.Count; i++)
                {
                    _pars[i] = pars[i] as DbParameter;
                }
                result = aDbDataAccess.ExecuteNonQuery(commandText, _pars, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 执行带不参的sql语句或存储过程
        /// 适用Insert,Delete,Update
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string commandText, CommandType commandtype)
        {
            try
            {
                //变量
                int result = 0;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteNonQuery(commandText, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static int ExecuteNonQuery(string commandText)
        {
            try
            {
                //变量
                int result = 0;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteNonQuery(commandText, CommandType.Text);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 执行带参查询返回结果集
        /// </summary>
        /// <param name="commandtext"></param>
        /// <param name="par"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string commandText, DbParameter[] pars, CommandType commandtype)
        {
            try
            {
                //变量
                DataSet result = new DataSet();
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteDataSet(commandText, pars, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 执行不带参查询返回结果集
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string commandText, CommandType commandtype)
        {
            try
            {
                //变量
                DataSet result = new DataSet();
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteDataSet(commandText, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static DataSet ExecuteDataSet(string commandText)
        {
            try
            {
                //变量
                DataSet result = new DataSet();
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteDataSet(commandText, CommandType.Text);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 执行不带参查询返回结果表
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText, CommandType commandtype)
        {
            try
            {
                //变量
                DataTable result = new DataTable();
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteDataTable(commandText, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public static DataTable ExecuteDataTableInTran(string commandText, CommandType commandtype)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                //变量
                DataTable result = new DataTable();

                result = AppDbDataAccess.ExecuteDataTableInTran(commandText, commandtype, AppDbTransaction);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 执行带参查询返回结果表
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="pars"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText, DbParameter[] pars, CommandType commandtype)
        {
            try
            {
                //变量
                DataTable result = new DataTable();
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteDataTable(commandText, pars, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 使用webservice执行带参查询返回结果表
        /// </summary>
        /// <param name="method"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataTable HttpPostDataTable(string ServiceIp, string method, string param)
        {

            string url = "http://" + ServiceIp + "/WebService.asmx";
            string result = string.Empty;
            byte[] bytes = null;
            Stream writer = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            bytes = Encoding.UTF8.GetBytes(param);
            request = (HttpWebRequest)HttpWebRequest.Create(url + "/" + method);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;
            try
            {
                writer = request.GetRequestStream();        //获取用于写入请求数据的Stream对象
            }
            catch (Exception ex)
            {
                return null;
            }
            writer.Write(bytes, 0, bytes.Length);       //把参数数据写入请求数据流
            writer.Close();

            try
            {
                response = (HttpWebResponse)request.GetResponse();      //获得响应
            }
            catch (WebException ex)
            {
                return null;
            }

            #region 这种方式读取到的是一个返回的结果字符串
            Stream stream = response.GetResponseStream();        //获取响应流
            XmlTextReader Reader = new XmlTextReader(stream);
            Reader.MoveToContent();
            result = Reader.ReadInnerXml();
            #endregion

            response.Close();
            Reader.Close();

            stream.Dispose();
            stream.Close();
            return JsonConvert.DeserializeObject<DataTable>(result);
        }


        /// <summary>
        /// 使用webservice执行带参查询返回结果表
        /// </summary>
        /// <param name="method"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string HttpPostData(string ServiceIp, string method, string param)
        {

            string url = "http://" + ServiceIp + "/WebService.asmx";
            string result = string.Empty;
            byte[] bytes = null;
            Stream writer = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            bytes = Encoding.UTF8.GetBytes(param);
            request = (HttpWebRequest)HttpWebRequest.Create(url + "/" + method);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;
            try
            {
                writer = request.GetRequestStream();        //获取用于写入请求数据的Stream对象
            }
            catch (Exception ex)
            {
                return null;
            }
            writer.Write(bytes, 0, bytes.Length);       //把参数数据写入请求数据流
            writer.Close();

            try
            {
                response = (HttpWebResponse)request.GetResponse();      //获得响应
            }
            catch (WebException ex)
            {
                return null;
            }

            #region 这种方式读取到的是一个返回的结果字符串
            Stream stream = response.GetResponseStream();        //获取响应流
            XmlTextReader Reader = new XmlTextReader(stream);
            Reader.MoveToContent();
            result = Reader.ReadInnerXml();
            #endregion

            response.Close();
            Reader.Close();

            stream.Dispose();
            stream.Close();
            return result;
        }

        public static DataTable ExecuteDataTableInTran(string commandText, DbParameter[] pars,
            CommandType commandtype)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                //变量
                DataTable result = new DataTable();

                result = AppDbDataAccess.ExecuteDataTableInTran(commandText, pars, commandtype, AppDbTransaction);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 执行带参查询返回结果表
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-26</date>
        /// <param name="commandText"></param>
        /// <param name="pars"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText, List<DbParameter> pars, CommandType commandtype)
        {
            try
            {
                //变量
                DataTable result = new DataTable();
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                DbParameter[] _pars = new DbParameter[pars.Count];
                for (int i = 0; i < pars.Count; i++)
                {
                    _pars[i] = pars[i] as DbParameter;
                }
                result = aDbDataAccess.ExecuteDataTable(commandText, _pars, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static DataTable ExecuteDataTable(string commandText, List<OracleParameter> pars, CommandType commandtype)
        {
            try
            {
                //变量
                DataTable result = new DataTable();
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);

                OracleParameter[] _pars = new OracleParameter[pars.Count];
                for (int i = 0; i < pars.Count; i++)
                {
                    _pars[i] = pars[i] as OracleParameter;
                }
                result = aDbDataAccess.ExecuteDataTable(commandText, pars, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static DataTable ExecuteDataTableInTran(string commandText, List<DbParameter> pars, CommandType commandtype)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                //变量
                DataTable result = new DataTable();

                DbParameter[] _pars = new DbParameter[pars.Count];
                for (int i = 0; i < pars.Count; i++)
                {
                    _pars[i] = pars[i] as DbParameter;
                }
                result = AppDbDataAccess.ExecuteDataTableInTran(commandText, _pars, commandtype, AppDbTransaction);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static DataTable ExecuteDataTable(string commandText)
        {
            try
            {
                //变量
                DataTable result = new DataTable();
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteDataTable(commandText, CommandType.Text);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static DataTable ExecuteDataTableInTran(string commandText)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                //变量
                DataTable result = new DataTable();

                result = AppDbDataAccess.ExecuteDataTableInTran(commandText, CommandType.Text, AppDbTransaction);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 执行带参查询返回DataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="pars"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static DbDataReader ExecuteDataReader(string commandText, DbParameter[] pars, CommandType commandtype)
        {
            try
            {
                DbDataReader result = null;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteDataReader(commandText, pars, commandtype) as DbDataReader;
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 执行不带参查询返回DataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static DbDataReader ExecuteDataReader(string commandText, CommandType commandtype)
        {
            try
            {
                DbDataReader result = null;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteDataReader(commandText, commandtype) as DbDataReader;
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static DbDataReader ExecuteDataReader(string commandText)
        {
            try
            {
                DbDataReader result = null;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteDataReader(commandText, CommandType.Text) as DbDataReader;
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 执行带参查询返回一行中的一列
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="pars"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static Object ExecuteScalar(string commandText, DbParameter[] pars, CommandType commandtype)
        {
            try
            {
                //变量
                Object result = null;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteScalar(commandText, pars, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static Object ExecuteScalarInTran(string commandText, DbParameter[] pars, CommandType commandtype)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                //变量
                Object result = null;

                result = AppDbDataAccess.ExecuteScalarInTran(commandText, pars, commandtype, AppDbTransaction);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-15</date>
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="pars"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static Object ExecuteScalarInTran(string commandText, List<OracleParameter> pars, CommandType commandtype)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }

                DbParameter[] _pars = new DbParameter[pars.Count];
                for (int i = 0; i < pars.Count; i++)
                {
                    _pars[i] = pars[i] as DbParameter;
                }

                //变量
                Object result = null;

                result = AppDbDataAccess.ExecuteScalarInTran(commandText, _pars, commandtype, AppDbTransaction);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 执行不带参查询返回一行中的一列
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public static Object ExecuteScalar(string commandText, CommandType commandtype)
        {
            try
            {
                //变量
                Object result = null;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteScalar(commandText, commandtype);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static Object ExecuteScalar(string commandText)
        {
            try
            {
                //变量
                Object result = null;
                CheckInfo();//检查工厂类与数据库连接字符串是指定
                DbDataAccess aDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                result = aDbDataAccess.ExecuteScalar(commandText, CommandType.Text);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static Object ExecuteScalarInTran(string commandText)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                //变量
                Object result = null;

                result = AppDbDataAccess.ExecuteScalarInTran(commandText, CommandType.Text, AppDbTransaction);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static Object ExecuteScalarInTran(string commandText, CommandType commandtype)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                //变量
                Object result = null;

                result = AppDbDataAccess.ExecuteScalarInTran(commandText, commandtype, AppDbTransaction);
                //返回结果
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 创建事务
        /// </summary>
        /// <returns></returns>
        public static bool BeginTransaction()
        {
            try
            {
                AppDbDataAccess = new DbDataAccess(AppDbProviderFactory, ConnectionString);
                if (DbDataAccess._DbConnection.State != ConnectionState.Open)
                {
                    DbDataAccess._DbConnection.Open();
                }
                _IDbTransaction = DbDataAccess._DbConnection.BeginTransaction();
                // _IDbTransaction.IsolationLevel
                return true;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 在事务中关联不带参对象
        /// </summary>
        /// <param name="commandtext"></param>
        /// <param name="commandtype"></param>
        public static void ExecuteNonQueryInTran(string commandtext, CommandType commandtype)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                AppDbDataAccess.ExecuteNonQueryInTran(commandtext, commandtype, AppDbTransaction);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static void ExecuteNonQueryInTran(string commandtext, DbParameter[] pars, CommandType commandtype)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                AppDbDataAccess.ExecuteNonQueryInTran(commandtext, pars, commandtype, AppDbTransaction);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        ///  在事务中,执行带参的sql语句或存储过程
        ///  适用Insert,Delete,Update
        ///  强调事务中不支持对大数据的操作，如Clob,Blob
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-14</date>
        /// <param name="commandtext"></param>
        /// <param name="par"></param>
        /// <param name="commandtype"></param>
        /// <param name="tran"></param>
        public static int ExecuteNonQueryInTran(string commandtext, List<OracleParameter> pars, CommandType commandtype)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }

                DbParameter[] _pars = new DbParameter[pars.Count];
                for (int i = 0; i < pars.Count; i++)
                {
                    _pars[i] = pars[i] as DbParameter;
                }
                return AppDbDataAccess.ExecuteNonQueryInTran(commandtext, _pars, commandtype, AppDbTransaction);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public static void ExecuteNonQueryInTran(IDbCommand cmd)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                cmd.Connection = DbDataAccess._DbConnection;
                cmd.Transaction = AppDbTransaction;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        public static void ExecuteNonQueryInTran(string commandtext)
        {
            try
            {
                if (AppDbDataAccess == null || AppDbTransaction == null)
                {
                    throw new Exception("请创建事务");
                }
                AppDbDataAccess.ExecuteNonQueryInTran(commandtext, CommandType.Text, AppDbTransaction);
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public static bool CommitTransaction()
        {
            try
            {

                if (_IDbTransaction == null)
                {
                    throw new Exception("并未创建事务");
                }

                _IDbTransaction.Commit();

                return true;
            }
            catch (Exception ce)
            {
                _IDbTransaction.Rollback();
                throw ce;
            }
            finally
            {
                AppDbDataAccess = null;
            }
        }

        /// <summary>
        /// 事务回滚，如果在事务的执行过程中出错，导致事务没有提交，这个时候需要调用此方法进行回滚
        /// 如果没有回滚，会导致执行相同的操作后死锁 
        /// Add by wwj 2013-03-13
        /// </summary>
        public static void RollbackTransaction()
        {
            try
            {
                if (_IDbTransaction == null)
                {
                    throw new Exception("并未创建事务");
                }

                _IDbTransaction.Rollback();
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                AppDbDataAccess = null;
            }
        }
    }

    class DbDataAccess
    {
        public static DbProviderFactory _DbProviderFactory = null;//工厂
        public static IDbConnection _DbConnection = null;//连接对象

        /// <summary>
        /// Varchar极限长度
        /// </summary>
        public enum VarCharMaxLengthType
        {
            OracleVachar2MaxBytes = 3000,//OracleVarchar极限长度
            SqlSeverVarcharMaxLength = 3000//SqlServer极限长度
        }
        public DbDataAccess()
        {

        }

        public DbDataAccess(DbProviderFactory dbProviderFactory, string connectstring)
        {
            try
            {
                if (dbProviderFactory == null)
                {
                    return;
                }
                _DbProviderFactory = dbProviderFactory;
                if (connectstring.Trim() == String.Empty || _DbProviderFactory == null)
                {
                    return;
                }

                _DbConnection = _DbProviderFactory.CreateConnection();
                _DbConnection.ConnectionString = connectstring;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 打开连接
        /// </summary>
        private void ConnectionOpen()
        {
            try
            {
                if (_DbConnection == null || _DbProviderFactory == null)
                {
                    throw new Exception("DbDataAccess中_DbProviderFactory或_DbConnection为Null");
                }

                _DbConnection.Open();
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        private void ConnectionClose()
        {
            try
            {
                if (_DbConnection != null && _DbConnection.State == ConnectionState.Open)
                {
                    _DbConnection.Close();
                    _DbConnection.Dispose();
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }


        #region ExecuteNonQuery
        /// <summary>
        /// 执行带参的sql语句或存储过程
        /// 适用Insert,Delete,Update
        /// </summary>
        /// <param name="commandtext">Sql语句或存储过程</param>
        /// <param name="par">参数数组</param>
        /// <param name="commandtype">命令类型</param>
        /// <returns>大于0代表执行成功,小于等于0执行失败</returns>
        public int ExecuteNonQuery(string commandtext, DbParameter[] par, CommandType commandtype)
        {
            Oracle.DataAccess.Client.OracleConnection acn = null;
            try
            {
                int result = 0;
                ConnectionOpen();//打开联接
                IDbCommand cmd = _DbConnection.CreateCommand();

                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);
                //xll 添加参数 sql server调用时下面不会添加参数 20130905
                if (_DbConnection.GetType().Name.ToLower().IndexOf("sql") >= 0)
                {
                    foreach (DbParameter item in par)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                if (!isHaveLob(par))
                {
                    CommonConvertParameterOracleParameter(par, cmd);
                }
                else
                {
                    LobConvertParameterOracleParameterNew(par, cmd);
                }
                result = cmd.ExecuteNonQuery();
                for (int i = 0; i < par.Length; i++)
                {
                    if (par[i].Direction == ParameterDirection.Output)
                    {
                        par[i].Value = (cmd.Parameters[i] as DbParameter).Value;
                    }
                }
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                ConnectionClose();
                if (acn != null && acn.State == ConnectionState.Open)
                {
                    acn.Close();
                }
            }

        }

        /// <summary>
        ///  在事务中,执行带参的sql语句或存储过程
        ///  适用Insert,Delete,Update
        ///  强调事务中不支持对大数据的操作，如Clob,Blob
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-15</date>
        /// <param name="commandtext"></param>
        /// <param name="par"></param>
        /// <param name="commandtype"></param>
        /// <param name="tran"></param>
        public int ExecuteNonQuery(string commandtext, List<OracleParameter> pars, CommandType commandtype)
        {
            Oracle.DataAccess.Client.OracleConnection acn = null;
            try
            {
                int result = 0;
                ConnectionOpen();//打开联接
                IDbCommand cmd = _DbConnection.CreateCommand();
                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);

                OracleParameter[] _pars = new OracleParameter[pars.Count];
                for (int i = 0; i < pars.Count; i++)
                {
                    _pars[i] = pars[i] as OracleParameter;
                }

                if (!isHaveLob(_pars))
                {
                    CommonConvertParameterOracleParameter(_pars, cmd);
                }
                else
                {
                    LobConvertParameterOracleParameterNew(_pars, cmd);
                }
                result = cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                ConnectionClose();
                if (acn != null && acn.State == ConnectionState.Open)
                {
                    acn.Close();
                }
            }
        }

        /// <summary>
        /// 执行无参sql语句或存储过程
        /// 适用于Insert,Delete,Update
        /// </summary>
        /// <param name="commandtext"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandtext, CommandType commandtype)
        {
            try
            {
                int result = 0;

                ConnectionOpen();//打开联接
                IDbCommand cmd = _DbConnection.CreateCommand();

                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);
                result = cmd.ExecuteNonQuery();

                return result;

            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                ConnectionClose();

            }

        }
        /// <summary>
        ///  在事务中,执行带参的sql语句或存储过程
        ///  适用Insert,Delete,Update
        ///  强调事务中不支持对大数据的操作，如Clob,Blob
        /// </summary>
        /// <param name="commandtext"></param>
        /// <param name="par"></param>
        /// <param name="commandtype"></param>
        /// <param name="tran"></param>
        public int ExecuteNonQueryInTran(string commandtext, DbParameter[] par, CommandType commandtype, IDbTransaction tran)
        {

            try
            {


                IDbCommand cmd = _DbConnection.CreateCommand();

                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);
                //xll 添加参数 sql server调用时下面不会添加参数 20130905
                if (_DbConnection.GetType().Name.ToLower().IndexOf("sql") >= 0)
                {
                    foreach (DbParameter item in par)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                if (!isHaveLob(par))
                {
                    CommonConvertParameterOracleParameter(par, cmd);
                }
                else
                {
                    LobConvertParameterOracleParameterNew(par, cmd);
                }
                if (tran != null)
                {
                    cmd.Transaction = tran;
                }

                //Modified by wwj 2013-2-22 用于获取执行完毕后返回的参数
                //return cmd.ExecuteNonQuery();
                int returnValue = cmd.ExecuteNonQuery();
                DS_SqlHelper.ParameterCollection = cmd.Parameters;
                for (int i = 0; i < par.Length; i++)
                {
                    if (par[i].Direction == ParameterDirection.Output)
                    {
                        par[i].Value = (cmd.Parameters[i] as DbParameter).Value;
                    }
                }
                return returnValue;
            }
            catch (Exception ce)
            {
                tran.Rollback();
                throw ce;
            }
            finally
            {

            }
        }

        /// <summary>
        ///  在事务中,执行不带参的sql语句或存储过程
        ///  适用Insert,Delete,Update
        ///  强调事务中不支持对大数据的操作，如Clob,Blob
        /// </summary>
        /// <param name="commandtext"></param>
        /// <param name="commandtype"></param>
        /// <param name="tran"></param>
        public int ExecuteNonQueryInTran(string commandtext, CommandType commandtype, IDbTransaction tran)
        {

            try
            {


                IDbCommand cmd = _DbConnection.CreateCommand();

                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);


                if (tran != null)
                {
                    cmd.Transaction = tran;
                }

                //Modified by wwj 2013-2-22 用于获取执行完毕后返回的参数
                //return cmd.ExecuteNonQuery();
                int returnValue = cmd.ExecuteNonQuery();
                DS_SqlHelper.ParameterCollection = cmd.Parameters;
                return returnValue;
            }
            catch (Exception ce)
            {
                tran.Rollback();
                throw ce;
            }
            finally
            {

            }

        }
        #endregion


        #region ExecuteDataSet
        /// <summary>
        /// 执行带参查询返回结果集
        /// </summary>
        /// <param name="commandtext"></param>
        /// <param name="par"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandtext, DbParameter[] par, CommandType commandtype)
        {
            try
            {
                DataSet result = new DataSet();

                ConnectionOpen();//打开联接

                IDbCommand cmd = _DbConnection.CreateCommand();

                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);

                //xll 添加参数 sql server调用时下面不会添加参数 20130905
                if (_DbConnection.GetType().Name.ToLower().IndexOf("sql") >= 0)
                {
                    foreach (DbParameter item in par)
                    {
                        cmd.Parameters.Add(item);
                    }
                }

                if (!isHaveLob(par))
                {
                    CommonConvertParameterOracleParameter(par, cmd);
                }
                else
                {
                    LobConvertParameterOracleParameterNew(par, cmd);
                }
                DbDataAdapter da = _DbProviderFactory.CreateDataAdapter();
                da.SelectCommand = cmd as DbCommand;

                da.Fill(result);
                for (int i = 0; i < par.Length; i++)
                {
                    if (par[i].Direction == ParameterDirection.Output)
                    {
                        par[i].Value = da.SelectCommand.Parameters[i].Value;
                    }
                }
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                ConnectionClose();
            }

        }
        public DataSet ExecuteDataSetInTran(string commandtext, DbParameter[] par, CommandType commandtype,
            IDbTransaction tran)
        {
            try
            {

                DataSet result = new DataSet();



                IDbCommand cmd = _DbConnection.CreateCommand();

                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);
                //xll 添加参数 sql server调用时下面不会添加参数 20130905
                if (_DbConnection.GetType().Name.ToLower().IndexOf("sql") >= 0)
                {
                    foreach (DbParameter item in par)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                if (!isHaveLob(par))
                {
                    CommonConvertParameterOracleParameter(par, cmd);
                }
                else
                {
                    LobConvertParameterOracleParameterNew(par, cmd);
                }
                DbDataAdapter da = _DbProviderFactory.CreateDataAdapter();
                da.SelectCommand = cmd as DbCommand;
                da.SelectCommand.Connection = _DbConnection as DbConnection;
                if (tran != null)
                {
                    da.SelectCommand.Transaction = tran as DbTransaction;
                }


                da.Fill(result);

                for (int i = 0; i < par.Length; i++)
                {
                    if (par[i].Direction == ParameterDirection.Output)
                    {
                        par[i].Value = da.SelectCommand.Parameters[i].Value;
                    }
                }
                return result;
            }
            catch (Exception ce)
            {
                tran.Rollback();
                throw ce;
            }
            finally
            {

            }

        }
        /// <summary>
        /// 执行不带参查询返回结果集
        /// </summary>
        /// <param name="commandtext"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandtext, CommandType commandtype)
        {
            try
            {
                DataSet result = new DataSet();

                ConnectionOpen();//打开联接

                IDbCommand cmd = _DbConnection.CreateCommand();

                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);

                DbDataAdapter da = _DbProviderFactory.CreateDataAdapter();
                da.SelectCommand = cmd as DbCommand;

                da.Fill(result);

                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                ConnectionClose();
            }

        }
        public DataSet ExecuteDataSetInTran(string commandtext, CommandType commandtype,
            IDbTransaction tran)
        {
            try
            {
                DataSet result = new DataSet();



                IDbCommand cmd = _DbConnection.CreateCommand();

                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);

                DbDataAdapter da = _DbProviderFactory.CreateDataAdapter();
                da.SelectCommand = cmd as DbCommand;
                if (tran != null)
                {
                    da.SelectCommand.Transaction = tran as DbTransaction;
                }

                da.Fill(result);

                return result;
            }
            catch (Exception ce)
            {
                tran.Rollback();
                throw ce;
            }
            finally
            {

            }

        }
        #endregion


        #region ExcuteDataTable
        /// <summary>
        /// 执行带参查询返回结果表
        /// </summary>
        /// <param name="commandtext"></param>
        /// <param name="par"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandtext, DbParameter[] par, CommandType commandtype)
        {
            try
            {
                DataSet result = ExecuteDataSet(commandtext, par, commandtype);

                return result.Tables[0];
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                ConnectionClose();
            }

        }
        public DataTable ExecuteDataTable(string commandtext, List<OracleParameter> pars, CommandType commandtype)
        {
            try
            {
                OracleParameter[] _pars = new OracleParameter[pars.Count];
                for (int i = 0; i < pars.Count; i++)
                {
                    _pars[i] = pars[i] as OracleParameter;
                }

                DataSet result = ExecuteDataSet(commandtext, _pars, commandtype);

                return result.Tables[0];
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                ConnectionClose();
            }

        }
        public DataTable ExecuteDataTableInTran(string commandtext, DbParameter[] par, CommandType commandtype
            , IDbTransaction tran)
        {
            try
            {

                DataSet result = ExecuteDataSetInTran(commandtext, par, commandtype, tran);

                return result.Tables[0];
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {

            }

        }
        /// <summary>
        /// 执行不带参查询返回结果表
        /// </summary>
        /// <param name="commandtext"></param>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandtext, CommandType commandtype)
        {
            try
            {
                DataSet result = ExecuteDataSet(commandtext, commandtype);

                return result.Tables[0];
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                ConnectionClose();
            }

        }
        public DataTable ExecuteDataTableInTran(string commandtext, CommandType commandtype
            , IDbTransaction tran)
        {
            try
            {
                DataSet result = ExecuteDataSetInTran(commandtext, commandtype, tran);

                return result.Tables[0];
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {

            }

        }
        #endregion


        #region ExecuteDataReader
        public IDataReader ExecuteDataReader(string commandtext, DbParameter[] par, CommandType commandtype)
        {

            try
            {
                IDataReader result = null;
                ConnectionOpen();//打开联接
                IDbCommand cmd = (_DbConnection as DbConnection).CreateCommand();
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);
                cmd.CommandType = commandtype;
                //xll 添加参数 sql server调用时下面不会添加参数 20130905
                if (_DbConnection.GetType().Name.ToLower().IndexOf("sql") >= 0)
                {
                    foreach (DbParameter item in par)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                if (!isHaveLob(par))
                {
                    CommonConvertParameterOracleParameter(par, cmd);
                }
                else
                {
                    //LobConvertParameterOracleParameter(par, cmd);
                    LobConvertParameterOracleParameterNew(par, cmd);
                }
                result = cmd.ExecuteReader();
                for (int i = 0; i < par.Length; i++)
                {
                    if (par[i].Direction == ParameterDirection.Output)
                    {
                        par[i].Value = (cmd.Parameters[i] as DbParameter).Value;
                    }
                }
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {

            }

        }
        public IDataReader ExecuteDataReader(string commandtext, CommandType commandtype)
        {

            try
            {
                IDataReader result = null;
                ConnectionOpen();//打开联接
                IDbCommand cmd = (_DbConnection as DbConnection).CreateCommand();
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);
                cmd.CommandType = commandtype;
                result = cmd.ExecuteReader();
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {

            }

        }
        #endregion


        #region ExecuteScalar
        public Object ExecuteScalar(string commandtext, DbParameter[] par, CommandType commandtype)
        {
            // Oracle.DataAccess.Client.OracleConnection acn =null;
            try
            {
                Object result = null;
                ConnectionOpen();//打开联接
                IDbCommand cmd = _DbConnection.CreateCommand();
                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);
                //xll 添加参数 sql server调用时下面不会添加参数 20130905
                if (_DbConnection.GetType().Name.ToLower().IndexOf("sql") >= 0)
                {
                    foreach (DbParameter item in par)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                if (!isHaveLob(par))
                {
                    CommonConvertParameterOracleParameter(par, cmd);

                }
                else
                {
                    LobConvertParameterOracleParameterNew(par, cmd);

                }
                result = cmd.ExecuteScalar();
                for (int i = 0; i < par.Length; i++)
                {
                    if (par[i].Direction == ParameterDirection.Output)
                    {
                        par[i].Value = (cmd.Parameters[i] as DbParameter).Value;
                    }
                }
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                ConnectionClose();
            }

        }
        public Object ExecuteScalarInTran(string commandtext, DbParameter[] par, CommandType commandtype, IDbTransaction tran)
        {

            try
            {
                Object result = null;



                IDbCommand cmd = _DbConnection.CreateCommand();

                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);

                //xll 添加参数 sql server调用时下面不会添加参数 20130905
                if (_DbConnection.GetType().Name.ToLower().IndexOf("sql") >= 0)
                {
                    foreach (DbParameter item in par)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                if (!isHaveLob(par))
                {
                    CommonConvertParameterOracleParameter(par, cmd);
                }
                else
                {
                    LobConvertParameterOracleParameterNew(par, cmd);
                }
                if (tran != null)
                {
                    cmd.Transaction = tran;
                }
                result = cmd.ExecuteScalar();

                for (int i = 0; i < par.Length; i++)
                {
                    if (par[i].Direction == ParameterDirection.Output)
                    {
                        par[i].Value = (cmd.Parameters[i] as DbParameter).Value;
                    }
                }

                return result;
            }
            catch (Exception ce)
            {
                tran.Rollback();
                throw ce;
            }
            finally
            {

            }

        }

        public Object ExecuteScalar(string commandtext, CommandType commandtype)
        {

            try
            {
                Object result = null;


                ConnectionOpen();//打开联接
                IDbCommand cmd = _DbConnection.CreateCommand();

                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);

                result = cmd.ExecuteScalar();


                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
            finally
            {
                ConnectionClose();

            }

        }

        public Object ExecuteScalarInTran(string commandtext, CommandType commandtype, IDbTransaction tran)
        {

            try
            {
                Object result = null;



                IDbCommand cmd = _DbConnection.CreateCommand();
                if (tran != null)
                {
                    cmd.Transaction = tran;
                }
                cmd.CommandType = commandtype;
                cmd.CommandText = ConvertSqlToOracle(commandtext, commandtype);

                result = cmd.ExecuteScalar();


                return result;
            }
            catch (Exception ce)
            {
                tran.Rollback();
                throw ce;
            }
            finally
            {

            }

        }

        #endregion

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public void ExecuteSqlTran(ArrayList SQLStringList)
        {
            //  IDbCommand cmd = _DbConnection.CreateCommand();
            string connectionString = _DbConnection.ConnectionString;
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
            string connectionString = _DbConnection.ConnectionString;
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
        #region 匹配转换
        /// <summary>
        /// 转换DBCommand对象为支持Oracle语法的Command对象
        /// </summary>
        /// <param name="command"></param>
        private void ConvertCommandToOracle(DbCommand command)
        {
            try
            {


                if (command.CommandText.Trim() == String.Empty)
                {
                    return;
                }
                if (_DbConnection.GetType().Name.ToLower().IndexOf("oracle") < 0)
                {
                    return;
                }

                if (command.CommandText.IndexOf('@') < 0)
                {
                    return;
                }

                command.CommandText = ConvertSqlToOracle(command.CommandText);

                foreach (DbParameter par in command.Parameters)
                {
                    par.ParameterName = par.ParameterName.Replace("@", ":");
                }


            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 参数类型转换,支持输入与输出参数
        /// 针对Oracle游标,进了了处理
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        private DbParameter[] ConvertParameterOracleParameter(DbParameter[] par)
        {
            try
            {
                if (par == null || par.Length == 0)
                {
                    return null;
                }
                if (_DbConnection.GetType().Name.ToLower().IndexOf("oracle") >= 0)
                {
                    OracleParameter[] arrPar = new OracleParameter[par.Length];
                    for (int i = 0; i < par.Length; i++)
                    {
                        arrPar[i] = new OracleParameter();
                        if (par[i].Direction == ParameterDirection.Output)
                        {
                            arrPar[i].ParameterName = par[i].ParameterName.Replace("@", "o_");
                        }
                        else
                        {
                            arrPar[i].ParameterName = par[i].ParameterName.Replace("@", "v_");
                        }
                        arrPar[i].DbType = par[i].DbType;
                        arrPar[i].Value = par[i].Value;
                        if (par[i].DbType == DbType.Object)
                        {
                            arrPar[i].OracleType = OracleType.Cursor;

                        }
                        else
                        {
                            arrPar[i].DbType = par[i].DbType;

                        }
                    }

                    return arrPar;
                }
                else
                {
                    return par;
                }



            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 不含CLob或BLob类型参数转换
        /// </summary>
        /// <param name="par"></param>
        /// <param name="command"></param>
        private void CommonConvertParameterOracleParameter(DbParameter[] par, IDbCommand command)
        {
            try
            {

                if (par == null || par.Length == 0)
                {
                    return;
                }
                if (_DbConnection.GetType().Name.ToLower().IndexOf("oracle") < 0)
                {
                    return;
                }
                command.Parameters.Clear();
                OracleParameter[] arrPar = new OracleParameter[par.Length];
                for (int i = 0; i < par.Length; i++)
                {
                    arrPar[i] = new OracleParameter();
                    arrPar[i].Direction = par[i].Direction;
                    arrPar[i].SourceColumn = par[i].SourceColumn;
                    arrPar[i].SourceColumnNullMapping = par[i].SourceColumnNullMapping;
                    arrPar[i].SourceVersion = par[i].SourceVersion;
                    arrPar[i].Value = par[i].Value;
                    arrPar[i].Size = par[i].Size;
                    if (command.CommandType == CommandType.Text)
                    {
                        arrPar[i].ParameterName = par[i].ParameterName.Replace("@", ":");

                    }
                    else
                    {
                        if (par[i].Direction == ParameterDirection.Output)
                        {
                            arrPar[i].ParameterName = par[i].ParameterName.Replace("@", "o_");
                        }
                        else
                        {
                            arrPar[i].ParameterName = par[i].ParameterName.Replace("@", "v_");
                        }
                    }


                    if (par[i].DbType == DbType.Object)
                    {
                        arrPar[i].OracleType = OracleType.Cursor;

                    }
                    else
                    {
                        arrPar[i].DbType = par[i].DbType;

                    }


                    command.Parameters.Add(arrPar[i]);
                }




            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 含Clob或Blob类型的参数转换
        /// </summary>
        /// <param name="par"></param>
        /// <param name="command"></param>
        /// <param name="_cn"></param>
        private void LobConvertParameterOracleParameter(DbParameter[] par, IDbCommand command,
            Oracle.DataAccess.Client.OracleConnection _cn)
        {
            try
            {

                if (par == null || par.Length == 0)
                {
                    return;
                }
                if (_DbConnection.GetType().Name.ToLower().IndexOf("oracle") < 0)
                {
                    return;
                }

                Oracle.DataAccess.Client.OracleParameter[] arrPar =
                    new Oracle.DataAccess.Client.OracleParameter[par.Length];
                command.Parameters.Clear();


                for (int i = 0; i < par.Length; i++)
                {
                    arrPar[i] = new Oracle.DataAccess.Client.OracleParameter();
                    arrPar[i].Direction = par[i].Direction;
                    arrPar[i].SourceColumn = par[i].SourceColumn;

                    arrPar[i].SourceVersion = par[i].SourceVersion;
                    arrPar[i].Value = par[i].Value;
                    arrPar[i].Size = par[i].Size;
                    if (command.CommandType == CommandType.Text)
                    {
                        arrPar[i].ParameterName = par[i].ParameterName.Replace("@", ":");

                    }
                    else
                    {
                        if (par[i].Direction == ParameterDirection.Output)
                        {
                            arrPar[i].ParameterName = par[i].ParameterName.Replace("@", "o_");
                        }
                        else
                        {
                            arrPar[i].ParameterName = par[i].ParameterName.Replace("@", "v_");
                        }
                    }


                    if (par[i].DbType == DbType.Object)
                    {
                        arrPar[i].OracleDbType = Oracle.DataAccess.Client.OracleDbType.RefCursor;

                    }
                    else if (par[i].DbType == DbType.String &&
                        par[i].Size > (int)VarCharMaxLengthType.OracleVachar2MaxBytes
                        && System.Text.Encoding.Default.GetByteCount(par[i].Value.ToString()) > (int)VarCharMaxLengthType.OracleVachar2MaxBytes)
                    {
                        OracleClob tmpClob = new OracleClob(
                            _cn);
                        Oracle.DataAccess.Client.OracleParameter objParam =
                            new Oracle.DataAccess.Client.OracleParameter(arrPar[i].ParameterName,
                                Oracle.DataAccess.Client.OracleDbType.Clob, tmpClob, par[i].Direction);

                        objParam.Value = par[i].Value;
                        arrPar[i] = objParam;

                    }
                    else if (par[i].DbType == DbType.Binary &&
                    par[i].Size > (int)VarCharMaxLengthType.OracleVachar2MaxBytes
                    && System.Text.Encoding.Default.GetByteCount(par[i].Value.ToString()) > (int)VarCharMaxLengthType.OracleVachar2MaxBytes)
                    {
                        OracleBlob tmpClob = new OracleBlob(
                            _cn);
                        Oracle.DataAccess.Client.OracleParameter objParam =
                            new Oracle.DataAccess.Client.OracleParameter(arrPar[i].ParameterName,
                                Oracle.DataAccess.Client.OracleDbType.Blob, tmpClob, par[i].Direction);

                        objParam.Value = par[i].Value;
                        arrPar[i] = objParam;

                    }
                    else
                    {
                        arrPar[i].DbType = par[i].DbType;

                    }

                    command.Parameters.Add(arrPar[i]);

                }


            }
            catch (Exception ce)
            {
                throw ce;
            }
        }


        private void LobConvertParameterOracleParameter(IDbDataParameter[] par, IDbCommand command)
        {
            try
            {

                if (par == null || par.Length == 0)
                {
                    return;
                }
                if (_DbConnection.GetType().Name.ToLower().IndexOf("oracle") < 0)
                {
                    return;
                }
                Oracle.DataAccess.Client.OracleParameter[] arrPar =
                    new Oracle.DataAccess.Client.OracleParameter[par.Length];
                command.Parameters.Clear();


                for (int i = 0; i < par.Length; i++)
                {
                    arrPar[i] = new Oracle.DataAccess.Client.OracleParameter();
                    arrPar[i].Direction = par[i].Direction;
                    arrPar[i].SourceColumn = par[i].SourceColumn;

                    arrPar[i].SourceVersion = par[i].SourceVersion;
                    arrPar[i].Value = par[i].Value;
                    arrPar[i].Size = par[i].Size;
                    if (command.CommandType == CommandType.Text)
                    {
                        arrPar[i].ParameterName = par[i].ParameterName.Replace("@", ":");

                    }
                    else
                    {
                        if (par[i].Direction == ParameterDirection.Output)
                        {
                            arrPar[i].ParameterName = par[i].ParameterName.Replace("@", "o_");
                        }
                        else
                        {
                            arrPar[i].ParameterName = par[i].ParameterName.Replace("@", "v_");
                        }
                    }


                    if (par[i].DbType == DbType.Object)
                    {
                        arrPar[i].OracleDbType = Oracle.DataAccess.Client.OracleDbType.RefCursor;

                    }
                    else if (par[i].DbType == DbType.String &&
                        par[i].Size > (int)VarCharMaxLengthType.OracleVachar2MaxBytes
                        && System.Text.Encoding.Default.GetByteCount(par[i].Value.ToString()) > (int)VarCharMaxLengthType.OracleVachar2MaxBytes)
                    {
                        OracleClob tmpClob = new OracleClob(
                            command.Connection as Oracle.DataAccess.Client.OracleConnection);
                        Oracle.DataAccess.Client.OracleParameter objParam =
                            new Oracle.DataAccess.Client.OracleParameter(arrPar[i].ParameterName,
                                Oracle.DataAccess.Client.OracleDbType.Clob, tmpClob, par[i].Direction);

                        objParam.Value = par[i].Value;
                        arrPar[i] = objParam;

                    }
                    else if (par[i].DbType == DbType.Binary &&
                    par[i].Size > (int)VarCharMaxLengthType.OracleVachar2MaxBytes
                    && System.Text.Encoding.Default.GetByteCount(par[i].Value.ToString()) > (int)VarCharMaxLengthType.OracleVachar2MaxBytes)
                    {
                        OracleBlob tmpClob = new OracleBlob(
                            command.Connection as Oracle.DataAccess.Client.OracleConnection);
                        Oracle.DataAccess.Client.OracleParameter objParam =
                            new Oracle.DataAccess.Client.OracleParameter(arrPar[i].ParameterName,
                                Oracle.DataAccess.Client.OracleDbType.Blob, tmpClob, par[i].Direction);

                        objParam.Value = par[i].Value;
                        arrPar[i] = objParam;

                    }
                    else
                    {
                        arrPar[i].DbType = par[i].DbType;

                    }

                    command.Parameters.Add(arrPar[i]);

                }


            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        private void LobConvertParameterOracleParameterNew(IDbDataParameter[] par, IDbCommand command)
        {
            try
            {
                if (par == null || par.Length == 0)
                {
                    return;
                }
                if (_DbConnection.GetType().Name.ToLower().IndexOf("oracle") < 0)
                {
                    return;
                }
                //Oracle.DataAccess.Client.OracleParameter[] arrPar =
                //    new Oracle.DataAccess.Client.OracleParameter[par.Length];
                System.Data.OracleClient.OracleParameter[] arrPar =
                     new System.Data.OracleClient.OracleParameter[par.Length];
                command.Parameters.Clear();

                for (int i = 0; i < par.Length; i++)
                {
                    //arrPar[i] = new Oracle.DataAccess.Client.OracleParameter();
                    arrPar[i] = new System.Data.OracleClient.OracleParameter();
                    arrPar[i].Direction = par[i].Direction;
                    arrPar[i].Size = par[i].Size;
                    if (par[i].Direction != ParameterDirection.Output)
                    {
                        arrPar[i].SourceColumn = par[i].SourceColumn;
                        arrPar[i].SourceVersion = par[i].SourceVersion;
                        arrPar[i].Value = par[i].Value;//輸出參數 提到這給值 楊偉康 二〇一三年五月三十日 17:15:56 
                    }

                    if (command.CommandType == CommandType.Text)
                    {
                        arrPar[i].ParameterName = par[i].ParameterName.Replace("@", ":");

                    }
                    else
                    {
                        if (par[i].Direction == ParameterDirection.Output)
                        {
                            arrPar[i].ParameterName = par[i].ParameterName.Replace("@", "o_");
                        }
                        else
                        {
                            arrPar[i].ParameterName = par[i].ParameterName.Replace("@", "v_");
                        }
                    }

                    if (par[i].DbType == DbType.Object)
                    {
                        //arrPar[i].OracleDbType = Oracle.DataAccess.Client.OracleDbType.RefCursor;
                        arrPar[i].OracleType = System.Data.OracleClient.OracleType.Cursor;
                    }
                    else if ((par[i].DbType == DbType.String || par[i].DbType == DbType.AnsiString
                        || par[i].DbType == DbType.AnsiStringFixedLength
                        ) &&
                        par[i].Size > (int)VarCharMaxLengthType.OracleVachar2MaxBytes
                        || System.Text.Encoding.Default.GetByteCount((par[i].Value == null ? "" : par[i].Value.ToString())) > (int)VarCharMaxLengthType.OracleVachar2MaxBytes
                        )
                    {
                        System.Data.OracleClient.OracleParameter objParam = new OracleParameter(arrPar[i].ParameterName,
                            OracleType.Clob);
                        objParam.Direction = par[i].Direction;
                        objParam.Value = par[i].Value;
                        arrPar[i] = objParam;

                    }
                    else if (par[i].DbType == DbType.Binary &&
                    par[i].Size > (int)VarCharMaxLengthType.OracleVachar2MaxBytes
                        || System.Text.Encoding.Default.GetByteCount((par[i].Value == null ? "" : par[i].Value.ToString())) > (int)VarCharMaxLengthType.OracleVachar2MaxBytes
                        )
                    {
                        System.Data.OracleClient.OracleParameter objParam = new OracleParameter(arrPar[i].ParameterName,
                            OracleType.Blob);
                        objParam.Direction = par[i].Direction;
                        objParam.Value = par[i].Value;
                        arrPar[i] = objParam;

                    }
                    else
                    {
                        arrPar[i].DbType = par[i].DbType;


                    }

                    command.Parameters.Add(arrPar[i]);
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 是否包含CLob或BLob类型的参数
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        private bool isHaveLob(DbParameter[] par)
        {
            try
            {
                bool result = false;
                result = true;
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        /// <summary>
        /// 将sql语句转成支持Oracel的sql语句
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        private string ConvertSqlToOracle(string commandText)
        {
            try
            {
                if (_DbConnection.GetType().Name.ToLower().IndexOf("oracle") < 0)
                {
                    return commandText;
                }
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
                commandText = commandText.Replace("getdate()", "sysdate");

                commandText = commandText.Replace("@", ":");
                return commandText;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 将sql语句转成支持Oracel的sql语句
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        private string ConvertSqlToOracle(string commandText, CommandType commandType)
        {
            try
            {
                if (_DbConnection.GetType().Name.ToLower().IndexOf("oracle") < 0)
                {
                    return commandText;
                }
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
                //commandText = commandText.Replace(";", "/");
                if (commandType == CommandType.Text)
                {
                    commandText = commandText.Replace("@", ":");
                }
                else
                {
                    commandText = commandText.Replace("@", "v_");
                    if (commandText.IndexOf('.') < 0)
                    {
                        commandText = DS_SqlHelper.DefualtPakageName.Trim() != "" ? (DS_SqlHelper.DefualtPakageName + "." + commandText) : commandText;
                    }
                }
                return commandText;
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        #endregion
    }

}
