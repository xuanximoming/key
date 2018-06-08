using DrectSoft.FrameWork;
using DrectSoft.FrameWork.Plugin.Manager;
using DrectSoft.MainFrame;
using DrectSoft.MainFrame.Login;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;
namespace MainFrame
{
    static class Program
    {
        public static string m_StrUserId;
        /// <summary>
        ///在选择了数据源后,将库名追加到主程序头部区域
        ///add by ywk 2013年4月23日11:16:17 
        /// </summary>
        public static string m_ShowMainTopText;
        /// <summary>
        /// 是否成功选择数据源，防止用户更改不合法数据 
        /// add  by ywk 2013年4月23日11:44:10 
        /// </summary>
        public static bool IsSuccessChoose;
        /// <summary>
        /// 是否跳过验证
        /// </summary>
        public static bool IsJumpCheck;
        /// <summary>
        /// 
        /// </summary>
        private static ArrayList appinfo = new System.Collections.ArrayList();
        /// <summary>
        ///The main entry point for the application.
        ///入口参数定义：/config 配置模式,不登录数据库
        ////menu:[filename] 指定启动的菜单
        /// </summary>
        /// <param name="args">打开应用程序时传入的信息（医生工作站中打开，默认传入用户id，一个参数）</param>
        [STAThread]
        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    appinfo.Add(args[i]);
                }
            }
            Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                GoSetDataAccess();//先行设定数据源
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
                if (!CkeckRegInfo())
                {
                    return;
                }

                if (IsJumpCheck)//应该跳过验证,直接登陆
                {
                    GoLogin();
                }
                //不应该跳过验证，且成功选择
                else if (!IsJumpCheck && IsSuccessChoose)//成功选择
                {
                    GoLogin();
                }
                else
                {
                    Application.Exit();
                    return;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void GoLogin()
        {
            DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
            string exeAssembly = Assembly.GetEntryAssembly().FullName;
            AppDomain ad = GetCustomAppDomain();

            ad.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            ad.CreateInstanceAndUnwrap(exeAssembly, typeof(StartupClass).FullName, false, BindingFlags.Public | BindingFlags.Instance, null, new object[] { false, "file.menu", m_ShowMainTopText, appinfo }, null, null);

        }
        /// <summary>
        /// 先行进行此动作，因系统开始就进行创建数据库连接对象，此处的选择数据源的动作就应该影响到DataAcce
        /// add by ywk 2013年4月23日11:02:54 
        /// </summary>
        public static void GoSetDataAccess()
        {
            try
            {
                //add by ywk 2013年4月22日11:10:40 
                string currentsoftpath = System.AppDomain.CurrentDomain.BaseDirectory;
                ChooseConection choose = new ChooseConection(currentsoftpath);
                if (File.Exists(currentsoftpath + "\\ConnectServer.xml"))//如果不存在的话就不弹出选择窗体
                {

                    choose.StartPosition = FormStartPosition.CenterParent;
                    choose.ShowDialog();

                    m_ShowMainTopText = choose.ChooseDataText;
                    //已经选择了连接数据源，就接下弹出登陆框
                    if (choose.IsSelectedSource)
                    {
                        //MessageBox.Show("选择了连接" + choose.SelectedSource);
                        XmlDocument XmldataSource = new XmlDocument();
                        XmldataSource.Load(currentsoftpath + "ConnectServer.xml");
                        XmlNodeList xnodeList = XmldataSource.SelectNodes("/Connection/" + choose.SelectedSource);
                        ArrayList DatasourceList = new ArrayList();
                        if (xnodeList.Count > 0)
                        {
                            foreach (XmlNode node in xnodeList)
                            {
                                for (int i = 0; i < node.ChildNodes.Count; i++)
                                {
                                    DatasourceList.Add(node.ChildNodes[i].InnerXml);
                                }
                            }
                            string newstr = string.Empty;
                            for (int i = 0; i < DatasourceList.Count; i++)
                            {
                                newstr += DatasourceList[i].ToString() + "";
                            }

                            //如果医院使用一个客户端两个库，就要限制只可以登录一个
                            //add by ywk 2013年5月6日16:22:28 
                            //先开一个新库，再开老库不让开，再开新库就可以
                            string softpath = currentsoftpath + "adcemr.exe.config";
                            if (CheckCanEnter(newstr, softpath))
                            {
                                //MessageBox.Show(newstr);
                                XmlDocument XmlconfigSource = new XmlDocument();
                                XmlconfigSource.Load(currentsoftpath + "adcemr.exe.config");
                                XmlNodeList SourcenodeList = XmlconfigSource.SelectNodes("/configuration/connectionStrings");
                                SourcenodeList[0].InnerXml = newstr;
                                XmlconfigSource.Save(currentsoftpath + "adcemr.exe.config");
                                IsSuccessChoose = true;
                                IsJumpCheck = false;
                            }
                            else
                            {
                                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("不能同时登录新旧HIS库，请先退出当前程序重新登录。");
                                IsSuccessChoose = false;
                                return;
                            }
                        }
                        else
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("未找到与(" + choose.SelectedSource + ")的数据源配置,请联系管理员。");
                            IsSuccessChoose = false;
                            return;
                        }

                    }
                }
                else
                {
                    IsJumpCheck = true;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("GoSetDataAccess方法出错" + ex.Message);
            }


        }
        /// <summary>
        /// 检测当前登录选择的数据库是否和当前一致，如果一致可以打开，如果不一致就不通过
        /// add by ywk 2013年5月7日10:46:10 
        /// </summary>
        /// <param name="newstr">当前要回写至配置的连接字符串</param>
        /// <param name="softpath">EMR 软件存放路径</param>
        /// <returns></returns>
        private static bool CheckCanEnter(string newstr, string softpath)
        {

            //应该先判断当前有没有开着EMR程序，如果开着就进下面判断，如果没开就直接true
            Process myproc = new Process();
            Process[] prc = Process.GetProcesses();
            if (Process.GetProcessesByName("adcemr").Length == 1)//没有开启多个EMR程序
            {
                return true;
            }

            XmlDocument XmlSoftConfig = new XmlDocument();
            XmlSoftConfig.Load(softpath);
            XmlNodeList SoftnodeList = XmlSoftConfig.SelectNodes("/configuration/connectionStrings");
            if (SoftnodeList[0].InnerXml.Equals(newstr))
            {
                return true;//可以登录
            }
            else
            {
                return false;//不可以再登录
            }

        }


        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            //System.Windows.Forms.MessageBox.Show("应用程序运行异常,请查看日志!" + ((Exception)e.ExceptionObject).Message);
            //if (e.IsTerminating) Application.Exit();
        }

        static AppDomain GetCustomAppDomain()
        {
            PlugInLoadHelper plugInLoadHelper = new PlugInLoadHelper(Application.StartupPath);
            DrectSoftConfigurationSectionHandler.SetConfigurationDelegate(plugInLoadHelper.ReadPlugInLoadConfiguration);
            PlugInLoadConfiguration plugInLoadConfig = (PlugInLoadConfiguration)ConfigurationManager.GetSection("plugInLoadSettings");

            AppDomainSetup ads = new AppDomainSetup();
            ads.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            ads.ApplicationName = "adcemr";
            ads.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            ads.PrivateBinPath = plugInLoadConfig.AllPath;
            if (plugInLoadConfig.UseShadowCopy)
            {
                ads.ShadowCopyFiles = "true";
                ads.CachePath = plugInLoadConfig.CachePath;
            }
            return AppDomain.CreateDomain("adcemr", null, ads);
        }
        /// <summary>
        /// 检查注册信息  包括医院名称与注册时间,和注册IP地址
        /// </summary>
        /// <returns></returns>
        public static bool CkeckRegInfo()
        {
            bool rvalue = true;
            try
            {
                string hpname = string.Empty;
                DateTime datetime = DateTime.Now;
                string secretkey = string.Empty;
                string sql = " select * from appcfg where configkey = 'CheckVersion' or configkey ='CheckTime'  ";
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                string sql_hpinfo = "select a.*,sysdate  from hospitalinfo a";
                DataTable dt_hpinf = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql_hpinfo, CommandType.Text);
                DataTable dt_client_ip = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(string.Format("select * from CLIENT_LOG where ip = '{0}'", PublicClass.GetIPStr()), CommandType.Text);
                if (dt_hpinf.Rows.Count > 0)
                {
                    hpname = dt_hpinf.Rows[0]["name"].ToString().Trim();
                    datetime = DateTime.Parse(dt_hpinf.Rows[0]["sysdate"].ToString().Trim());
                    secretkey = dt_hpinf.Rows[0]["memo"].ToString().Trim();

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            if (dt.Rows[i]["configkey"].ToString().ToLower() == "checkversion")
                            {
                                if (dt.Rows[i]["value"].ToString().Trim() != EncryptDES(hpname.Trim(), secretkey))
                                {
                                    MessageBox.Show("注册版本信息不符合！");
                                    rvalue = false;
                                }

                            }
                            if (dt.Rows[i]["configkey"].ToString().ToLower() == "checktime")
                            {
                                if (datetime > DateTime.Parse(DecryptDES(dt.Rows[i]["value"].ToString().Trim(), secretkey)))
                                {
                                    MessageBox.Show("软件已超过使用时间，请重新注册！");
                                    rvalue = false;
                                }

                            }
                        }
                        if (dt_client_ip.Rows.Count <= 0 || dt_client_ip.Rows[0]["ip_code"].ToString().Trim() != EncryptDES(PublicClass.GetIPStr().Trim(), "ip__code"))
                        {
                            FormReg FormReg = new FormReg(PublicClass.GetIPStr().Trim());
                            if (DialogResult.OK != FormReg.ShowDialog())
                                rvalue = false;
                        }

                    }
                    else
                    {
                        rvalue = false;
                    }
                }
                else
                {
                    rvalue = false;
                }

                return rvalue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false; ;
            }

        }

        #region 加密算法
        //默认密钥向量
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥</param>
        /// <returns>解密成功后返回解密后的字符串，失败返回源串</returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
        #endregion
    }


    #region 判断程序和更细模块是否运行
    /// <summary>
    /// 启动程序在新AppDomain中
    /// </summary>
    internal class StartupClass : MarshalByRefObject
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="skiplogin"></param>
        /// <param name="loadmenu"></param>
        ///  <param name="m_ShowMainTopText">追加参数传递</param>add by ywk 
        public StartupClass(bool skiplogin, string loadmenu, string m_ShowMainTopText, ArrayList appinfo)
        {

            Process myproc = new Process();
            Process[] prc = Process.GetProcesses();

            if (Process.GetProcessesByName("adcemr").Length <= 1)//没有开启多个EMR程序
            {
                if (Process.GetProcessesByName("AutoUpdate").Length == 0)
                {

                    DrectSoft.MainFrame.FormMain formain = new DrectSoft.MainFrame.FormMain(skiplogin, loadmenu, m_ShowMainTopText, appinfo);
                    Application.Run(formain);
                }
                else
                {
                    MessageBox.Show("自动更新程序已启动，请完成软件更新后重试。");
                    Application.Exit();
                }

            }
            else
            {
                MessageBox.Show("电子病历系统已启动，不能重复启动。");
                Application.Exit();
            }

        }

    }
    #endregion
}
