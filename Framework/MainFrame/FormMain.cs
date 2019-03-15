using DevExpress.Utils;
using DevExpress.XtraBars;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Eop;
using DrectSoft.Core;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.Plugin;
using DrectSoft.FrameWork.Plugin.Manager;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;


namespace DrectSoft.MainFrame
{
    public partial class FormMain : DevBaseForm, IEmrHost, ICustomMessageBox
    {
        public string id = null;
        #region vars
        string m_Loadmenufile = string.Empty;
        private const string s_PatientTable = "Inpatients";
        private const string s_BedTable = "Beds";
        bool m_Isconfig = false;


        IUser m_loguser = null;
        PluginManager m_manager;
        PluginMenuRegister m_register;
        private bool m_needChanged = true;
        PluginStatusBarRegister m_registerStatusbar;
        DSMessageBox m_messagebox;
        IDataAccess m_sqlHelper = null;
        /// <summary>
        /// 用于存放数据选择源的xml
        /// add by ywk 2013年4月22日13:56:13 
        /// </summary>
        private XmlDocument xmldatasource;
        public XmlDocument XmldataSource
        {
            get { return xmldatasource; }
            set { xmldatasource = value; }
        }
        private XmlDocument xmlconfigsource;
        /// <summary>
        /// 此XML是存放OracleRun下面的Emr.exe.config的XML格式文件
        /// add by ywk 2013年4月22日14:30:17 
        /// </summary>
        public XmlDocument XmlconfigSource
        {
            get { return xmlconfigsource; }
            set { xmlconfigsource = value; }
        }
        private ArrayList datasourcelist;
        /// <summary>
        /// 存放所有对接系统的连接字符串，用于回写至Emr.exe.config
        /// </summary>
        public ArrayList DatasourceList
        {
            get { return datasourcelist; }
            set { datasourcelist = value; }
        }
        /// <summary>
        /// 实际登录人的帐户
        /// </summary>
        Account m_account;

        /// <summary>
        /// 登录人的带教老师的帐户
        /// </summary>
        Account m_MasterAccount;

        private string addMainText;
        /// <summary>
        /// 接受选择数据源界面的文本，追加至主界面显示
        /// add by ywk 2013年4月23日11:25:27 
        /// </summary>
        public string AddMainText
        {
            get { return addMainText; }
            set { addMainText = value; }
        }


        bool s_InterSetTime;
        internal static FormMain Instance;
        private bool m_InvokeClosing = true;
        private string m_LockTime = string.Empty;

        private string LockTime
        {
            get
            {
                if (string.IsNullOrEmpty(m_LockTime))
                {
                    try
                    {
                        m_LockTime = BasicSettings.GetStringConfig("SYSLOCKTIME");
                    }
                    catch
                    {
                        m_LockTime = "1";
                    }
                }
                return m_LockTime;
            }
        }

        private DSMessageBox MessageBoxInstance
        {
            get
            {
                if (m_messagebox == null)
                    m_messagebox = new DSMessageBox();
                return m_messagebox;
            }
        }

        private bool m_EmrAllowEdit = true;


        PluginUtil m_PublicMethod;

        private EmrDefaultSetting m_EmrDefaultSetting;

        #endregion

        #region prop

        public Inpatient CurrentPatientInfo
        {
            get { return _currentPat; }
            set//公开属性
            {
                CancelEventArgs e = new CancelEventArgs();
                SetPluginForPatientChanging(e);
                if (e.Cancel) return;
                else
                {
                    _currentPat = value;
                    SetPluginForPatientChange();
                }
            }
        }
        Inpatient _currentPat;

        /// <summary>
        /// 新的
        /// </summary>
        public Inpatient NEWCurrentPatientInfo
        {
            get { return _newcurrentPat; }
            set//公开属性
            {
                CancelEventArgs e = new CancelEventArgs();
                SetPluginForPatientChanging(e);
                if (e.Cancel) return;
                else
                {
                    _newcurrentPat = value;
                    SetPluginForPatientChange();
                }
            }
        }
        Inpatient _newcurrentPat;


        public ICustomMessageBox CustomMessageBox
        {
            get { return this; }
        }

        public IDataAccess SqlHelper
        {
            get
            {
                if (m_sqlHelper == null)
                    DataAccessFactory.GetSqlDataAccess();

                //设置全局的数据库查询类 edit by tj 2010-10-30
                if (CommonObjects.SqlHelper == null)
                    CommonObjects.SqlHelper = DataAccessFactory.DefaultDataAccess;
                //
                return DataAccessFactory.DefaultDataAccess;
            }
        }

        public string MacAddress
        {
            get
            {
                ManagementClass mcMAC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection mocMAC = mcMAC.GetInstances();

                string macAdd = string.Empty;
                foreach (ManagementObject m in mocMAC)
                {
                    if ((bool)m["IPEnabled"])
                    {
                        macAdd = m["MacAddress"].ToString().Replace(":", "");
                        break;
                    }
                }
                return macAdd;
            }
        }

        public string Ip
        {
            get
            {
                return PublicClass.GetIPStr();
                //return System.Net.IPAddress
            }
        }


        public IAppConfigReader AppConfig
        {
            get { throw new NotImplementedException(); }
        }

        public DataSet PatientInfos
        {
            get
            {
                //if (_patientInfos == null)
                {
                    _patientInfos = GetPatientInfoData(m_loguser.CurrentDeptId, m_loguser.CurrentWardId);
                }
                return _patientInfos;
            }
        }

        private DataSet _patientInfos;

        public string RefreshPatientInfos()
        {
            _patientInfos = GetPatientInfoData(m_loguser.CurrentDeptId, m_loguser.CurrentWardId);
            return string.Empty;
        }

        public IUser User
        {
            get { return m_loguser; }
        }



        /// <summary>
        /// 有权限的MENU信息集合
        /// </summary>
        public Collection<PlugInConfiguration> PrivilegeMenu
        {
            get { return m_manager.PrivilegeMenu; }
        }


        /// <summary>
        /// 当前PLUGINMANAGER
        /// </summary>
        public PluginManager Manager
        {
            get { return m_manager; }

        }

        private void SetWaitDialogCaption(string caption)
        {
            if (m_WaitForm == null)
                m_WaitForm = new WaitDialogForm("正在读取数据...", "请稍等");
            m_WaitForm.SetCaption(caption);

        }

        private void HideWaitDialog()
        {
            m_WaitForm.Hide();
        }
        private WaitDialogForm m_WaitForm;

        /// <summary>
        /// 默认可以编辑
        /// </summary>
        public bool EmrAllowEdit
        {
            get
            {
                return m_EmrAllowEdit;
            }
            set
            {
                m_EmrAllowEdit = value;
            }
        }

        public PluginUtil PublicMethod
        {
            get
            {
                if (m_PublicMethod == null)
                {
                    m_PublicMethod = new PluginUtil(SqlHelper.GetDatabase());
                }
                return m_PublicMethod;
            }
        }

        /// <summary>
        /// 病历编辑器默认设置
        /// </summary>
        public EmrDefaultSetting EmrDefaultSettings
        {
            get
            {
                if (m_EmrDefaultSetting == null)
                {
                    m_EmrDefaultSetting = Util.InitEmrDefaultSet();
                }

                return m_EmrDefaultSetting;
            }
        }

        /// <summary>
        /// 当前选中的病历，读取后需要删除
        /// </summary>
        public string CurrentSelectedEmrID
        {
            get
            {
                return m_CurrentSelectedEmrID;
            }
            set
            {
                m_CurrentSelectedEmrID = value;
            }
        }
        private string m_CurrentSelectedEmrID = string.Empty;

        /// <summary>
        /// 病历权限状态
        /// </summary>
        public string FloderState
        {
            get
            {
                return m_FloderState;
            }
            set
            {
                m_FloderState = value;
            }
        }
        private string m_FloderState = string.Empty;
        #endregion

        #region ctor
        WaitDialogForm m_WaitDialog;

        /// <summary>
        ///  构造主应用程序窗口
        /// </summary>
        /// <param name="isconfig"></param>
        /// <param name="menufile"></param>
        /// <param name="users"></param>
        public FormMain(bool isconfig, string menufile, string showtext, ArrayList appinfo)
        {


            //SetWaitDialogCaption("正在打开电子病历系统...");//add by wwj 2012-04-25

            InitializeComponent();
            this.Icon = DrectSoft.MainFrame.Properties.Resources.emr;

            InitImages();
            m_Isconfig = isconfig;

            m_manager = new PluginManager(this, Logger);
            m_register = new PluginMenuRegister();
            m_manager.SetMenuRegister(m_register);

            AddMainText = showtext;
            m_Loadmenufile = menufile;

            Instance = this;
            if (appinfo.Count > 0)
            {
                Appinfo = appinfo;

            }
            this.timerGarbageCollect.Enabled = true;//add by wwj 2013-01-05

        }
        #endregion

        #region private

        private static void ForceSynchLocalSystemTime()
        {
            // 强制同步服务器和本地的时间
            DateTime serverTime = DataAccessFactory.DefaultDataAccess.GetServerTime();
            if (serverTime.ToString("yyyy-MM-dd HH:mm") != System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
            {
                UtilsForExtension.Set2LocalSystemTime(serverTime);
            }
        }

        private static ArrayList Appinfo = new ArrayList();
        private void SystemTimeChanged(object sender, EventArgs e)
        {
            if (s_InterSetTime)
            {
                s_InterSetTime = false;
                return;
            }

            //本地时间修改后强制同步时间
            s_InterSetTime = true;
            SystemEvents.TimeChanged -= new EventHandler(SystemTimeChanged);
            ForceSynchLocalSystemTime();
            SystemEvents.TimeChanged += new EventHandler(SystemTimeChanged);
        }

        private void InitImages()
        {
            this.Icon = DrectSoft.MainFrame.Properties.Resources.emr;
        }

        void UpdateServerTime2LocalTime()
        {

            DateTime svrTime = SqlHelper.GetServerTime();
            svrTime.Set2LocalSystemTime();
        }

        //private string GetIPStr()
        //{

        //    string strHostName = Dns.GetHostName();     //得到本机的主机名   
        //    IPHostEntry ipEntry = Dns.GetHostByName(strHostName);   //取得本机IP  
        //    if (ipEntry != null && ipEntry.AddressList != null && ipEntry.AddressList.Length > 0)
        //    {
        //        string strAddr = ipEntry.AddressList[0].ToString();
        //        return (strAddr);
        //    }
        //    else
        //        return "0.0.0.0";

        //}

        private void InitializePlugins()
        {


            m_register.ClearChildRegisters();

            if (!m_Isconfig)
            {
                //todo 停止使用左侧菜单
                //m_fileMenuRegister = new FileMenuRegister(this.barLeft_Menu, this, this.barManager1);
                //m_register.AddChildRegister(m_fileMenuRegister);
            }
            else
            {
            }

            if (string.IsNullOrEmpty(m_Loadmenufile))
                m_manager.RegisterPlugins(Application.StartupPath);
            else
                m_manager.RegisterPlugins(Application.StartupPath, new string[] { m_Loadmenufile });

            //如果登录人有带教老师，则根据带教老师的权限捞取功能模块
            if (string.IsNullOrEmpty(m_account.User.MasterID))
            {
                m_manager.RegisterMenuPlugins(m_account);
            }
            else
            {
                m_manager.RegisterMenuPlugins(m_MasterAccount);
            }

            m_manager.Runner.LoadPlugIn("DrectSoft.Core.MouldList.dll", "DrectSoft.Core.MouldList.FormMouldList", true);

            //m_manager.Runner.LoadPlugIn("DrectSoft.Core.DoctorTasks.dll", "DrectSoft.Core.DoctorTasks.NewCenter", false);

            //m_registerStatusbar = new PluginStatusBarRegister(statusStrip1);
            //m_registerStatusbar.Register(m_loguser);
        }

        private delegate void DeRestart();

        private void DoLock()
        {
            this.timerLock.Enabled = false;
            FormLock formLock = new FormLock();
            formLock.ShowDialog();
            if (formLock.DialogResult == DialogResult.OK)
            {
            }
            else if (formLock.DialogResult == DialogResult.Retry)
            {
                //调用之前重新登录的方法
                m_manager.Runner.CloseAllPlugins();
                //m_manager.UnRegisterAllPlugins();
                FormLogin login = new FormLogin();

                if (DialogResult.OK == login.ShowDialog())
                {
                    InitializePlugins();
                }
            }
            else
            {
                Restart();
            }
            this.timerLock.Enabled = true;
            timerGarbageCollect.Enabled = true;
        }
        /// <summary>
        /// xjt,重新启动
        /// </summary>
        private void Restart()
        {
            Application.ExitThread();
            Thread thread = new Thread(new ParameterizedThreadStart(ApStart));
            object appName = Application.ExecutablePath;
            Thread.Sleep(2000);
            thread.Start(appName);
        }
        /// <summary>
        /// xjt,Restart调用
        /// </summary>
        /// <param name="obj"></param>
        private void ApStart(Object obj)
        {
            Process process = new Process();
            process.StartInfo.FileName = obj.ToString();
            process.Start();
        }

        private void RegisterStatusBar()
        {

            barStaticDept.Caption = "科室病区：" + m_loguser.CurrentDeptName + "(" + m_loguser.CurrentDeptId + ") / " + m_loguser.CurrentWardName + "(" + m_loguser.CurrentWardId + ")";
            barStaticUserName.Caption = "用户：" + m_loguser.Name + "(" + m_loguser.Id + ")";
            this.barStaticItemTime.Caption = "系统时间：" + DateTime.Today.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " " + DateTime.Now.ToString("T", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            this.barStaticItemIP.Caption = "本机IP：" + Ip;
        }

        /// <summary>
        /// 根改用户密码
        /// </summary>
        private void ChangePassword()
        {
            FormChangePassword formChangePassword = new FormChangePassword(this);
            formChangePassword.ShowDialog();
        }


        private void SetPluginForUserChange()
        {

            this.SuspendLayout();
            m_manager.Runner.UserChangeExecuted.Clear();
            //处理只有一个有主窗口PlugIn的时候
            List<IPlugIn> plgs = m_manager.Runner.PluginsWithMainFormLoaded;
            if (plgs.Count == 1)
            {
                IPlugIn plg = plgs[0];
                plg.ExecuteUsersChangeEvent(new UserArgs(User));
                m_manager.Runner.UserChangeExecuted.Add(plg);
            }
            else if (m_manager.Runner.ActivePlugIn != null)
            {
                m_manager.Runner.ActivePlugIn.ExecuteUsersChangeEvent(new UserArgs(User));
                m_manager.Runner.UserChangeExecuted.Add(m_manager.Runner.ActivePlugIn);
            }

            if (m_manager.Runner.StartPlugins != null && m_manager.Runner.StartPlugins.Count != 0)
            {
                foreach (IPlugIn startplugin in m_manager.Runner.StartPlugins)
                {
                    if (m_manager.Runner.PlugInIndexInList(startplugin.AssemblyFileName, startplugin.StartClassType, m_manager.Runner.PatientChangeExecuted) < 0)
                    {
                        startplugin.ExecuteUsersChangeEvent(new UserArgs(User));
                    }
                }
            }

            this.ResumeLayout();



        }

        private void SetPluginForPatientChange()
        {
            this.SuspendLayout();
            m_manager.Runner.PatientChangeExecuted.Clear();

            //处理只有一个有主窗口PlugIn的时候
            List<IPlugIn> plgs = m_manager.Runner.PluginsWithMainFormLoaded;
            if (plgs.Count == 1)
            {
                IPlugIn plg = plgs[0];
                plg.ExecutePatientChangeEvent(new PatientArgs(CurrentPatientInfo));
                m_manager.Runner.PatientChangeExecuted.Add(plg);
            }
            else
            {

                //todo modified by zhouhui 2011-6-10  此处将startplugis换成PluginsLoaded，待确认
                if (m_manager.Runner.PluginsLoaded != null && m_manager.Runner.PluginsLoaded.Count != 0)
                {
                    foreach (IPlugIn startplugin in m_manager.Runner.PluginsLoaded)
                    {
                        if (m_manager.Runner.PlugInIndexInList(startplugin.AssemblyFileName, startplugin.StartClassType, m_manager.Runner.PatientChangeExecuted) < 0)
                        {
                            startplugin.ExecutePatientChangeEvent(new PatientArgs(CurrentPatientInfo));
                            m_manager.Runner.PatientChangeExecuted.Add(startplugin);
                        }
                    }
                }
            }

            this.ResumeLayout();
        }

        private void SetPluginForPatientChanging(CancelEventArgs e)
        {
            this.SuspendLayout();

            // 首先处理当前激活的PlugIn
            if (m_manager.Runner.ActivePlugIn != null)
                m_manager.Runner.ActivePlugIn.ExecutePatientChangingEvent(e);

            if (!e.Cancel)
            {
                foreach (IPlugIn plg in m_manager.Runner.PluginsWithMainFormLoaded)
                {
                    if (plg != m_manager.Runner.ActivePlugIn)
                    {
                        plg.ExecutePatientChangingEvent(e);
                        if (e.Cancel)
                            break;
                    }
                }
            }

            List<IPlugIn> plgs = m_manager.Runner.PluginsWithMainFormLoaded;
            if (plgs.Count == 1)
            {
                IPlugIn plg = plgs[0];
                plg.ExecutePatientChangingEvent(e);
            }
            else
            {
                //    if (m_manager.Runner.ActivePlugIn != null)
                //{
                //    m_manager.Runner.ActivePlugIn.ExecutePatientChangingEvent(e);
                //}

                if (!e.Cancel && m_manager.Runner.StartPlugins != null && m_manager.Runner.StartPlugins.Count != 0)
                {
                    foreach (IPlugIn startplugin in m_manager.Runner.StartPlugins)
                    {
                        if (m_manager.Runner.PlugInIndexInList(startplugin.AssemblyFileName, startplugin.StartClassType, m_manager.Runner.PatientChangeExecuted) < 0)
                        {
                            startplugin.ExecutePatientChangingEvent(e);
                        }
                        if (e.Cancel) break;
                    }
                }
            }
            this.ResumeLayout();
        }

        /// <summary>
        /// 初始化功能设置,xjt
        /// </summary>
        private void InitFunctionBar()
        {
            //BarButtonItem barItemChangeWard = new BarButtonItem();
            //barItemChangeWard.Caption = "切换病区";
            //barItemChangeWard.ItemClick += new ItemClickEventHandler(barItemChangeWard_ItemClick);
            //barSubItem1.AddItem(barItemChangeWard);
        }


        /// <summary>
        /// 显示登录界面
        /// </summary>
        /// <returns>如果登录成功，返回true;否则返回false</returns>
        FormLogin m_FormLogin;
        Boolean m_IsFristShowLogin = true;

        private Boolean Login()
        {
            m_FormLogin = new FormLogin(this);
            HideWaitDialog();
            this.SetSkin();


            if (Appinfo.Count > 0)
            {
                string username = "";
                m_FormLogin.userid_check(Appinfo[0].ToString(), ref username);
                if (username.Trim().Length <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该用户不存在");
                    return false;

                }
                else
                {
                    m_FormLogin.ProcessNOLogin();
                    return true;
                }
            }
            else
            {
                m_FormLogin.ShowDialog();
                if (m_FormLogin.DialogResult == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        /// <summary>
        /// 在登录界面中点击确定按钮后激活ProcessLogin
        /// </summary>
        public void ProcessLogin()
        {
            //在登陆界面获取是否设置了锁屏,如果没有的话，定时器就可以禁用以节省资源
            //add by ywk 2013年3月22日9:57:01 
            m_LockTime = BasicSettings.GetStringConfig("SYSLOCKTIME");
            if (string.IsNullOrEmpty(m_LockTime))
            {
                timerLock.Enabled = false;
            }
            else
            {
                timerLock.Enabled = true;
            }
            m_account = m_FormLogin.CurrentAccount;
            m_MasterAccount = m_FormLogin.MasterAccount;
            m_loguser = m_account.User;
            CommonObjects.CurrentUser = m_loguser;//edit by tj 2012-10-30 给全局的用户属性赋值
            DS_Common.currentUser = m_loguser; //edit by cyq 2012-11-21 给通用库添加登录用户信息(以方便整个项目使用)
            m_loguser.CurrentDeptWardChanged += new EventHandler(m_loguser_CurrentDeptWardChanged);
            //this.barLeft_Menu.Manager = this.barManager1;

            //进度到25
            m_FormLogin.ChangeProgressBar(21, 25);
            InitializePlugins();

            //进度到40
            m_FormLogin.ChangeProgressBar(26, 40);
            RegisterStatusBar();

            //进度到45
            m_FormLogin.ChangeProgressBar(41, 45);
            SetPluginForUserChange();

            if (m_IsFristShowLogin == true)//只有在第一次调用登录窗体的时候才进行的操作
            {
                m_IsFristShowLogin = false;

                ////进度到50
                m_FormLogin.ChangeProgressBar(46, 50);

                ForceSynchLocalSystemTime();

                //进度到60
                m_FormLogin.ChangeProgressBar(51, 60);

                UpdateServerTime2LocalTime();
                InitFunctionBar();

                //进度到70
                m_FormLogin.ChangeProgressBar(61, 70);

                SetTabbedMdiHead();

                //进度到80
                m_FormLogin.ChangeProgressBar(71, 75);

                SystemEvents.TimeChanged += new EventHandler(SystemTimeChanged);
                //m_manager.Runner.LoadPlugIn("DrectSoft.Core.DoctorTasks.dll", "DrectSoft.Core.DoctorTasks.NewCenter", false);
                //m_manager.Runner.LoadPlugIn("DrectSoft.Core.DoctorTasks.dll", "DrectSoft.Core.DoctorTasks.TaskCenter", true);
                this.WindowState = FormWindowState.Maximized;

                //Application.DoEvents();

                //进度到90
                m_FormLogin.ChangeProgressBar(76, 90);

                this.Show();

                //m_FormLogin.Hide();

                //ShowConsultationInfo();
                InitTimerMessageWindow();
            }
            //用户登录，默认打开新闻通知模块 add by ywk 2012年8月14日 11:03:54(仁和需求)通过配置
            string IsShowNees = BasicSettings.GetStringConfig("IsShowNewCenter");
            if (IsShowNees == "1")
            {
                m_manager.Runner.LoadPlugIn("DrectSoft.Core.DoctorTasks.dll", "DrectSoft.Core.DoctorTasks.NewCenter", false);
            }

        }

        #endregion

        #region 获取病人数据
        private const string str_queryInpatient = "select * from InPatient where OutHosWard ='{0}' and  Status not in (1500, 1502, 1503, 1504, 1508, 1509)";

        private const string str_queryPatByID = "select * from InPatient where NOOFINPAT={0}";

        private const string str_querywardinfo = "usp_QueryInwardPatients";
        private const string str_queryDailyInfo = "usp_QueryWardInfo";

        private SqlParameter[] SqlParams
        {
            get
            {
                if (_sqlParams == null)
                {
                    _sqlParams = new SqlParameter[] { 
                  new SqlParameter("Deptids", SqlDbType.VarChar, 8),
                  new SqlParameter("Wardid", SqlDbType.VarChar, 8)
               };
                }

                return _sqlParams;
            }
        }
        private SqlParameter[] _sqlParams;


        private System.Data.DataSet GetPatientInfoData(string dept, string ward)
        {

            DataSet result = new DataSet();
            //test 
            if (string.IsNullOrEmpty(dept))
            {
                dept = "3225";
                ward = "2922";
            }

            SqlParams[0].Value = dept; // 科室代码
            SqlParams[1].Value = ward; // 病区代码
            // 取当前科室、病区的病人数据
            DataTable table = SqlHelper.ExecuteDataTable(string.Format(str_queryInpatient, ward));
            table.TableName = "病人数据集";
            result.Tables.Add(table.Copy());

            // 取当前科室、病区的床位信息
            DataTable table1 = SqlHelper.ExecuteDataTable(str_querywardinfo, SqlParams, CommandType.StoredProcedure);
            table1.TableName = "床位信息";
            table1.Columns["BedID"].Caption = "床位";
            table1.Columns["PatName"].Caption = "患者姓名";
            table1.Columns["Sex"].Caption = "性别";
            table1.Columns["AgeStr"].Caption = "年龄";
            table1.Columns["PatID"].Caption = "住院号";
            table1.Columns["AdmitDate"].Caption = "入院日期";
            result.Tables.Add(table1.Copy());

            return result;
        }


        private DataRow GetPatInfo(decimal noOfInpat)
        {
            DataTable table = SqlHelper.ExecuteDataTable(string.Format(str_queryPatByID, noOfInpat));

            if (table.Rows.Count < 1) return null;
            return table.Rows[0];

        }

        private DataRow GetPatInfo(string noOfInpat)
        {
            DataTable table = SqlHelper.ExecuteDataTable(string.Format(str_queryPatByID, noOfInpat));

            if (table.Rows.Count < 1) return null;
            return table.Rows[0];

        }
        private DataRow GetPatInfobypatnoofhis(string patnoofhis)
        {
            string sql = "select * from inpatient a where a.patnoofhis='" + patnoofhis + "'";
            DataTable table = SqlHelper.ExecuteDataTable(string.Format(sql));

            if (table.Rows.Count < 1) return null;
            return table.Rows[0];

        }
        #endregion

        #region public
        public void ChoosePatient(decimal firstPageNo)
        {

            SetWaitDialogCaption("系统正在切换病人，请稍候...");
            m_FloderState = string.Empty;
            SetPatientInfo(firstPageNo);
            HideWaitDialog();

        }
        /// <summary>
        /// 用于整体录入界面的跳转不改变全局Inpatient
        /// add byy ywk 2013年1月11日11:47:53 
        /// </summary>
        /// <param name="firstPageNo"></param>
        public void ChoosePatient(string firstPageNo, out Inpatient m_Inpat)
        {

            SetWaitDialogCaption("系统正在切换病人，请稍候...");
            m_FloderState = string.Empty;
            SetPatientInfo(firstPageNo, out  m_Inpat);
            HideWaitDialog();

        }

        public void ChoosePatient(decimal firstPageNo, string floaderState)
        {
            SetWaitDialogCaption("系统正在切换病人，请稍候...");
            m_FloderState = floaderState;
            SetPatientInfo(firstPageNo);
            HideWaitDialog();
        }

        #region  门诊
        public void ChooseOutPatient(decimal firstPageNo)
        {
            this.SetWaitDialogCaption("系统正在切换病人，请您稍候");
            this.SetOutPatientInfo(firstPageNo);
            this.HideWaitDialog();
        }

        public void ChooseOutPatient(decimal firstPageNo, string floaderState)
        {
            SetWaitDialogCaption("系统正在切换病人，请稍候...");
            m_FloderState = floaderState;
            SetPatientInfo(firstPageNo);
            HideWaitDialog();
        }

        /// <summary>
        /// 用于整体录入界面的跳转不改变全局Inpatient
        /// add byy ywk 2013年1月11日11:47:53 
        /// </summary>
        /// <param name="firstPageNo"></param>
        public void ChooseOutPatient(string firstPageNo, out Inpatient m_Inpat)
        {

            SetWaitDialogCaption("系统正在切换病人，请稍候...");
            m_FloderState = string.Empty;
            SetPatientInfo(firstPageNo, out  m_Inpat);
            HideWaitDialog();

        }

        private void SetOutPatientInfo(decimal firstPageNo)
        {
            this.xtraTabbedMdiManager1.SelectedPageChanged -= new EventHandler(this.xtraTabbedMdiManager1_SelectedPageChanged);
            //DataRow[] array = this.PatientInfos.Tables[0].Select("NoOfInpat = " + firstPageNo.ToString());
            //if (array != null && array.Length > 0)
            //{
            //    this.CurrentPatientInfo = new Inpatient(array[0]);
            //}
            //else
            //{
            //DataRow patInfo = this.GetOutPatInfo(firstPageNo);
            //if (patInfo != null)
            //{
            //    this.CurrentOutPatientInfo = new Outpatient(patInfo);
            //}
            ////}
            //this.xtraTabbedMdiManager1.SelectedPageChanged += new EventHandler(this.xtraTabbedMdiManager1_SelectedPageChanged);
        }
        #endregion
        /// <summary>
        /// 设置病人信息
        /// </summary>
        /// <param name="firstPageNo"></param>
        private void SetPatientInfo(decimal firstPageNo)
        {
            xtraTabbedMdiManager1.SelectedPageChanged -= new EventHandler(xtraTabbedMdiManager1_SelectedPageChanged);

            DataRow[] rows = PatientInfos.Tables[0].Select("NoOfInpat = " + firstPageNo.ToString());
            if ((rows != null) && (rows.Length > 0))
            {
                CurrentPatientInfo = new Inpatient(rows[0]);
            }
            else //数据库读取
            {
                DataRow row = GetPatInfo(firstPageNo);
                if (row != null)
                    CurrentPatientInfo = new Inpatient(row);

            }

            xtraTabbedMdiManager1.SelectedPageChanged += new EventHandler(xtraTabbedMdiManager1_SelectedPageChanged);

            //edit by tj 2012-10-26 获得当前病人
            CommonObjects.CurrentPatient = CurrentPatientInfo;
        }
        private void SetPatientInfo(string patnoofhis)
        {

            xtraTabbedMdiManager1.SelectedPageChanged -= new EventHandler(xtraTabbedMdiManager1_SelectedPageChanged);

            DataRow[] rows = PatientInfos.Tables[0].Select("patnoofhis = '" + patnoofhis + "'");
            if ((rows != null) && (rows.Length > 0))
            {

                CurrentPatientInfo = new Inpatient(rows[0]);
            }
            else //数据库读取
            {
                DataRow row = GetPatInfobypatnoofhis(patnoofhis);

                if (row != null)
                    CurrentPatientInfo = new Inpatient(row);

            }

            xtraTabbedMdiManager1.SelectedPageChanged += new EventHandler(xtraTabbedMdiManager1_SelectedPageChanged);

            //edit by tj 2012-10-26 获得当前病人
            CommonObjects.CurrentPatient = CurrentPatientInfo;
        }
        /// <summary>
        /// 用于整体录入界面的跳转不改变全局Inpatient
        /// add byy ywk 2013年1月11日11:47:53 
        /// </summary>
        /// <param name="firstPageNo"></param>
        private void SetPatientInfo(string firstPageNo, out Inpatient m_Inpat)
        {
            Inpatient resultInp = CurrentPatientInfo;
            xtraTabbedMdiManager1.SelectedPageChanged -= new EventHandler(xtraTabbedMdiManager1_SelectedPageChanged);

            DataRow[] rows = PatientInfos.Tables[0].Select("NoOfInpat = " + firstPageNo.ToString());
            if ((rows != null) && (rows.Length > 0))
            {
                resultInp = new Inpatient(rows[0]);
                //CurrentPatientInfo = new Inpatient(rows[0]);
            }
            else //数据库读取
            {
                DataRow row = GetPatInfo(firstPageNo);
                if (row != null)
                    resultInp = new Inpatient(row);

            }

            xtraTabbedMdiManager1.SelectedPageChanged += new EventHandler(xtraTabbedMdiManager1_SelectedPageChanged);
            m_Inpat = resultInp;
            //edit by tj 2012-10-26 获得当前病人
            //CommonObjects.CurrentPatient = CurrentPatientInfo;
        }

        /// <summary>
        /// 将激活的插件ActivePlugIn设置为文书录入的插件
        /// </summary>
        private void SetActivePlugInForRecordInput()
        {
            string assemblyFileName = "DrectSoft.Core.RecordsInput.dll";

            if (!System.IO.File.Exists("UIPlugins\\" + assemblyFileName))
            {
                throw new FileNotFoundException("程序集" + "UIPlugins\\" + assemblyFileName + "不存在!");
            }

            foreach (IPlugIn iPlugIn in m_manager.Runner.PluginsLoaded)
            {
                if (assemblyFileName == iPlugIn.AssemblyFileName)
                {
                    m_manager.Runner.ActivePlugIn = iPlugIn;
                    break;
                }
            }
        }


        public bool LoadPlugIn(string assemblyName, string startupClassName)
        {
            try
            {
                return (m_manager.Runner.LoadPlugIn(assemblyName, startupClassName, true) != null);
            }
            catch (Exception e)
            {
                return false;
            }
        }


        public bool LoadPlugIn(string typeName)
        {
            try
            {
                Type t = Type.GetType(typeName, true, true);
                string assemblyname = t.Assembly.ManifestModule.Name;
                string classname = t.FullName;
                return (m_manager.Runner.LoadPlugIn(assemblyname, classname, true) != null);

            }
            catch (Exception e)
            {
                return false;
            }
        }


        public DialogResult MessageShow(string message)
        {
            return MessageShow(message, CustomMessageBoxKind.InformationOk);
        }

        public DialogResult MessageShow(string message, CustomMessageBoxKind kind)
        {

            Collection<string> messageInfos = new Collection<string>();

            messageInfos.Add(message);

            MessageBoxInstance.SetMessageInfo(messageInfos);
            MessageBoxInstance.SetButtonInfo(kind);
            //MessageBoxInstance.SetIconInfo(kind);
            return m_messagebox.ShowDialog();
        }
        #endregion

        #region events
        /// <summary>
        /// 窗体加载事件 
        /// add by ywk 2013年4月22日13:55:06 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                this.DoubleBuffered = true;
                GOLogin();

                if (Appinfo.Count > 0)
                {
                    SetPatientInfo(Appinfo[1].ToString());
                    if (this.CurrentPatientInfo != null && Appinfo.Count > 0)
                    {
                        this.ChoosePatient(decimal.Parse(this.CurrentPatientInfo.NoOfFirstPage.ToString()));
                        this.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DrectSoft.Service.DS_BaseService.GetUCEmrInputPath());


                    }
                    else if (this.CurrentPatientInfo == null && Appinfo.Count > 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("病人未同步到电子病历中，请稍等！");
                    }
                    Appinfo.Clear();

                }


            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
            finally
            {
                //AppFormIconConfig.SetIcon(this,false,null); xll暂时不通过配置获取
                //改界面右上方text根据配置获得
                if (!string.IsNullOrEmpty(AddMainText))
                {
                    AddMainText = "----" + AddMainText;
                }
                //string Titletext = DrectSoft.MainFrame.PublicClass.getHosName() + AddMainText;  //add by Ukey 2016-08-25主窗体标题医院名称
                string Titletext = GetConfigValueByKey("MainTitleText") + AddMainText;//追加此版本对应的数据库 add by ywk 2013年4月23日11:30:57 
                if (Titletext != null && !string.IsNullOrEmpty(Titletext.Trim()))
                {
                    this.Text = Titletext;
                }
                //edit by tj 2012-10-26   系统会诊是否需要【审核】环节
                string result = GetConfigValueByKey("ConsultAuditConfig").Split('|')[0] == "1" ? "1" : "0";
                if (result != null && !string.IsNullOrEmpty(result.Trim()))
                {
                    if (result.Equals("1"))
                    {
                        CommonObjects.IsNeedVerifyInConsultation = true;
                    }
                    else
                    {
                        CommonObjects.IsNeedVerifyInConsultation = false;
                    }
                }
            }
            this.simpleButtonChangeArea.Focus();
            this.Icon = DrectSoft.MainFrame.Properties.Resources.emr;
        }

        public void GOLogin()
        {
            SetWaitDialogCaption("正在打开电子病历系统...");
            //登录
            if (!this.Login())
            {
                m_InvokeClosing = false;
                Close();
                return;
            }
            this.timer1.Enabled = true;
            HideWaitDialog();
        }

        public string GetConfigValueByKey(string key)
        {
            try
            {
                string sql1 = " select * from appcfg where configkey = '" + key + "'  ";
                DataTable dt = SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
                string config = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    config = dt.Rows[0]["value"].ToString();
                }
                return config;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void m_loguser_CurrentDeptWardChanged(object sender, EventArgs e)
        {
            barStaticDept.Caption = "科室病区：" + m_loguser.CurrentDeptName + "(" + m_loguser.CurrentDeptId + ") / " + m_loguser.CurrentWardName + "(" + m_loguser.CurrentWardId + ")";
            barStaticUserName.Caption = "用户：" + m_loguser.Name + "(" + m_loguser.Id + ")";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.barStaticItemTime.Caption = "系统时间：" + DateTime.Today.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " " + DateTime.Now.ToString("T", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        void tsmiLogin_Click(object sender, EventArgs e)
        {

        }



        private void barButtonlogin_ItemClick(object sender, ItemClickEventArgs e)
        {
            ReLogin();
        }

        private void ReLogin()
        {
            this.Hide();
            m_manager.Runner.CloseAllPlugins();

            if (!this.Login())
            {
                m_InvokeClosing = false;
                Close();
            };

            this.m_IsFirstGetConsultation = true;
            this.Activate();
            this.Show();
        }
        /// <summary>
        /// 判断系统是否退出
        /// </summary>
        bool m_IsEnterFormClosing = false;
        public void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_IsEnterFormClosing)
            {
                bool isExit = ExitApplication();
                if (!isExit)//系统不退出
                {
                    e.Cancel = true;
                }
            }

            //if (!m_InvokeClosing) return;

            if (e.Cancel) return;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //PacsClose();
            PlugInLoadHelper.UnLoadPlugIn(Application.StartupPath);
        }


        #region 调用PACS图像
        [DllImport("joint.dll")]
        public static extern int PacsView(int nPatientType, string lpszID, int nImageType); //Pacs调阅3.1版

        [DllImport("joint.dll")]
        public static extern bool PacsViewByPatientInfo(int nType, string str, int nPatientType);
        [DllImport("joint.dll")]
        public static extern int PacsInitialize();
        [DllImport("joint.dll")]
        public static extern void PacsRelease();

        /// <summary>
        /// 关闭Pacs调用
        /// </summary>
        public void PacsClose()
        {
            try
            {
                PacsRelease();//关闭Pacs调用
            }
            catch (Exception ex)
            {
                MessageBox.Show("DrectSoft.MainFrame.FormMain.PacsClose" + ex.Message);
            }
        }
        /// <summary>
        /// 判断调用PACS的DLL是否存在 
        /// </summary>
        private bool CheckPackIsExist()
        {

            string jointpath = Application.StartupPath + @"\joint.dll";//获取程序所在文件夹
            if (!File.Exists(jointpath))//不存在此接口DLL就不执行 
            {
                //MessageShow("调用失败，缺少程序集joint.dll");
                return false;
            }
            string connectpath = Application.StartupPath + @"\Connection.dll";//获取程序所在文件夹
            if (!File.Exists(connectpath))//不存在此接口DLL就不执行 
            {
                //MessageShow("调用失败，缺少程序集Connection.dll");
                return false;
            }
            string pacspath = Application.StartupPath + @"\PACSID.dll";//获取程序所在文件夹
            if (!File.Exists(pacspath))//不存在此接口DLL就不执行 
            {
                //MessageShow("调用失败，缺少程序集PACSID.dll");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 启动Pacs调用
        /// </summary>
        public void PacsStart()
        {
            try
            {
                if (CheckPackIsExist())
                {
                    PacsInitialize();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("DrectSoft.MainFrame.FormMain.PacsStart" + ex.Message);
            }
        }
        #endregion
        /// <summary>
        /// /锁屏定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerLock_Tick(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(m_LockTime))
            //    return;
            //拿到此取值edit by ywk 2013年3月18日20:28:05 
            try
            {
                m_LockTime = BasicSettings.GetStringConfig("SYSLOCKTIME");
                if (string.IsNullOrEmpty(m_LockTime))
                {
                    return;
                }

                if (Util.GetLastInputTime() > double.Parse(m_LockTime))
                {
                    DoLock();
                }
            }
            catch (Exception)
            {

            }


        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            //m_manager.Runner.LoadPlugIn("DrectSoft.Core.DoctorTasks.dll", "DrectSoft.Core.DoctorTasks.TaskCenter", true);
        }

        /// <summary>
        /// 最小化,xjt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonMin_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// 切换病区,xjt test
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void barItemChangeWard_ItemClick(object sender, ItemClickEventArgs e)
        {
            ChangeWard();
        }

        private void ChangeWard()
        {
            FormItemFunction formItemFunction = new FormItemFunction();
            string id = null;  //add by Ukey 2016-08-06 限制切换科室的一个人进入两个（非全院科室）权限传递ID值
            id = m_loguser.Id;
            formItemFunction.GetId(id);
            if (DialogResult.OK == formItemFunction.ShowDialog())
            {
                ((Users)m_loguser).SetCurrentDeptWard(formItemFunction.SelectDept);
                //刷新数据
                RefreshPatientInfos();
            }
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            int maxHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int maxWidth = Screen.PrimaryScreen.WorkingArea.Width;
            this.MaximizedBounds = new Rectangle(0, 0, maxWidth + 2, maxHeight + 2);
        }

        #endregion

        #region Add By wwj 控制Top部分，将原来的Top部分的BarControl隐藏，在右上角显示“切换病区”“重新登录”“修改密码”“最小化”“退出”

        /// <summary>
        /// 控制TabbedMDIHead部分
        /// </summary>
        private void SetTabbedMdiHead()
        {
            //切换TabPage，在切换之后将当前Page中的插件赋给ActivePlugin
            xtraTabbedMdiManager1.SelectedPageChanged += new EventHandler(xtraTabbedMdiManager1_SelectedPageChanged);

            //第一个TabPage不显示关闭按钮，其他的均在右侧显示关闭按钮
            xtraTabbedMdiManager1.ClosePageButtonShowMode = DevExpress.XtraTab.ClosePageButtonShowMode.InActiveTabPageHeader;
            if (xtraTabbedMdiManager1.Pages.Count > 0)
            {
                xtraTabbedMdiManager1.Pages[0].ShowCloseButton = DefaultBoolean.False;
            }

            //设置Panel中Button的背景图片
            simpleButtonHelp.ToolTip = "用户手册";
            simpleButtonChangeArea.ToolTip = "切换科室";
            simpleButtonReLogin.ToolTip = "重新登录";
            simpleButtonChangePassword.ToolTip = "修改密码";
            sbtnchooseDatabase.ToolTip = "切换数据库";//原来的最小化，没用现在改为选择数据库add by ywk 2013年5月3日9:27:52 
            simpleButtonNormal.ToolTip = "窗体化";
            simpleButtonClose.ToolTip = "退出系统";


            Size size = new Size(14, 14);
            simpleButtonHelp.Image = Resources.ResourceManager.GetImage(Resources.ResourceNames.HelpTopRight);
            simpleButtonChangeArea.Image = Resources.ResourceManager.GetImage(Resources.ResourceNames.ChangeWardTopRight);
            simpleButtonReLogin.Image = Resources.ResourceManager.GetImage(Resources.ResourceNames.ReLoginTopRight);
            simpleButtonChangePassword.Image = Resources.ResourceManager.GetImage(Resources.ResourceNames.ChangePasswordTopRight);
            sbtnchooseDatabase.Image = Resources.ResourceManager.GetImage(Resources.ResourceNames.ChangeWardTopRight);//MinimizeTopRight
            //simpleButtonClose.Image = Resources.ResourceManager.GetImage(Resources.ResourceNames.ShutdownTopRight);
            simpleButtonNormal.Image = Resources.ResourceManager.GetImage(Resources.ResourceNames.WindowStyle);

            //帮助
            simpleButtonHelp.Click += new EventHandler(simpleButtonHelp_Click);
            //切换病区
            simpleButtonChangeArea.Click += new EventHandler(simpleButtonChangeArea_Click);
            //重新登录
            simpleButtonReLogin.Click += new EventHandler(simpleButtonReLogin_Click);
            //更改密码
            simpleButtonChangePassword.Click += new EventHandler(simpleButtonChangePassword_Click);
            //最小化
            sbtnchooseDatabase.Click += new EventHandler(simpleButtonMinimum_Click);
            //退出
            simpleButtonClose.Click += new EventHandler(simpleButtonClose_Click);
            //窗体化
            simpleButtonNormal.Click += new EventHandler(simpleButtonNormal_Click);

            //根据分辨率设置Panel的位置
            Rectangle workArea = Screen.GetWorkingArea(this);
            panelControlTopRight.Location = new Point(workArea.Width - panelControlTopRight.Width, 1);
            //panelControlTopRight.Height = 21;
        }

        /// <summary>
        /// 将当前TabPage设置为激活的插件ActivePlugIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void xtraTabbedMdiManager1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (xtraTabbedMdiManager1.SelectedPage != null)
            {
                string pageName = xtraTabbedMdiManager1.SelectedPage.Text;
                foreach (IPlugIn iPlugIn in m_manager.Runner.PluginsLoaded)
                {
                    if (pageName == iPlugIn.MainForm.Text)
                    {
                        m_manager.Runner.ActivePlugIn = iPlugIn;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonClose_Click(object sender, EventArgs e)
        {
            try
            {
                ExitApplication();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public bool ExitApplication()
        {
            if (!GoChooseDataBase)
            {
                if (MyMessageBox.Show("您确定要退出系统吗？", "退出系统", MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    m_IsEnterFormClosing = true;
                    try
                    {
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("DrectSoft.MainFrame.FormMain.ExitApplication" + ex.Message);
                    }
                    System.Environment.Exit(System.Environment.ExitCode);
                    this.Dispose();
                    this.Close();
                    return true;
                }
            }
            return false;
        }
        public bool GoChooseDataBase;//用于声明是否进行选择数据库的变量。如果此变量为真，就无需进行提示是否登录

        /// <summary>
        /// **最小化**
        /// 现更改为切换数据库
        /// add by ywk 2013年5月3日9:33:40 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonMinimum_Click(object sender, EventArgs e)
        {
            try
            {

                //if (MessageBox.Show("您确定要重新选择数据库吗？", "重新选库", MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                //{
                //    this.Visible = false;
                //    Program.GoSetDataAccess();//先行设定数据源
                //    this.Visible = true;
                //    //SetPluginForUserChange();//add by ywk 
                //    m_manager.Runner.CloseAllPlugins();
                //    this.ReLogin();
                //}

                //this.WindowState = FormWindowState.Minimized;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                ChangePassword();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重新登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonReLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyMessageBox.Show("您确定要重新登录吗？", "重新登录", MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    //SetPluginForUserChange();//add by ywk 
                    m_manager.Runner.CloseAllPlugins();
                    this.ReLogin();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 切换病区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonChangeArea_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeWard();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 帮助
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonHelp_Click(object sender, EventArgs e)
        {
            try
            {
                string name = GetConfigValueByKey("ShowHelpName");
                string helpFilePath = Application.StartupPath.ToString() + "\\" + name.ToString().Trim();

                if (File.Exists(helpFilePath))
                {
                    System.Diagnostics.Process.Start(Application.StartupPath.ToString() + "\\" + name.ToString().Trim());
                }
                else
                {
                    this.CustomMessageBox.MessageShow("帮助文件" + helpFilePath + "不存在", CustomMessageBoxKind.ErrorYes);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonNormal_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region IEmrHost 成员


        public HospitalInfo CurrentHospitalInfo
        {
            get
            {
                if (_currentHospitalInfo == null)
                {
                    GetHospitalInfo();
                }
                return _currentHospitalInfo;
            }
        }

        private HospitalInfo _currentHospitalInfo = null;

        private void GetHospitalInfo()
        {
            try
            {
                //using (EMREntities entities = new EMREntities())
                //{
                //    _currentHospitalInfo = entities.HospitalInfo.First();


                //}
                DataTable dtHos = SqlHelper.ExecuteDataTable(@"select a.id,
                                                                       a.name,
                                                                       a.subname,
                                                                       a.sn,
                                                                       a.medicalid,
                                                                       a.address,
                                                                       a.yzbm,
                                                                       a.tel,
                                                                       nvl(a.bzcws, '0') bzcws,
                                                                       a.memo
                                                                  from hospitalinfo a where rownum < 2");
                HospitalInfo hospitalinfo = new HospitalInfo();
                if (dtHos.Rows.Count > 0)
                {
                    DataRow dr = dtHos.Rows[0];
                    hospitalinfo.Id = dr["ID"].ToString();
                    //hospitalinfo.Name = DrectSoft.MainFrame.PublicClass.getHosName();
                    hospitalinfo.Name = dr["Name"].ToString(); //by Ukey 2016-08-25 /*医院名称*/
                    hospitalinfo.Subname = dr["SubName"].ToString();
                    hospitalinfo.Sn = dr["SN"].ToString();
                    hospitalinfo.Medicalid = dr["MedicalId"].ToString();
                    hospitalinfo.Address = dr["Address"].ToString();
                    hospitalinfo.Yzbm = dr["Yzbm"].ToString();
                    hospitalinfo.Tel = dr["Tel"].ToString();
                    hospitalinfo.Bzcws = Convert.ToInt32(dr["Bzcws"].ToString());
                    hospitalinfo.Memo = dr["Memo"].ToString();
                }
                _currentHospitalInfo = hospitalinfo;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message);
                //todo log
            }

        }

        public DrectSoftLog Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new DrectSoftLog();

                }
                return _logger;
            }
        }
        private DrectSoftLog _logger;

        #endregion

        #region Add By wwj 2011-09-19
        UCMessageWindow m_UCMessageWindow;

        /// <summary>
        /// 在右下角动态的显示提示信息
        /// </summary>
        /// <param name="dt">添加的数据</param>
        /// <param name="isClear">是否清空原来的数据</param>
        public void ShowMessageWindow(DataTable dt, bool isClear)
        {
            try
            {
                DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;//解决第三方控件异步报错的问题
                m_UCMessageWindow.ShowMessageWindow(this, dt, isClear);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //定时抓取提醒信息
        private void InitTimerMessageWindow()
        {
            try
            {
                string openMessageWindowConfig = new AppConfigReader().GetConfig("IsOpenMessageWindow").Config;
                string[] configs = openMessageWindowConfig.Split(',');

                if (m_UCMessageWindow == null)
                {
                    if (configs.Length >= 3)
                    {
                        m_UCMessageWindow = new UCMessageWindow(this, configs[1], configs[2]);
                    }
                }

                if (configs.Length > 0)
                {
                    bool isOpenMessageWindow = configs[0] == "1" ? true : false;
                    if (isOpenMessageWindow)
                    {
                        AppConfigReader m_cfgreader = new AppConfigReader();
                        timerMessageWindow.Interval = Convert.ToInt32(m_cfgreader.GetConfig("DocCenterTimeInterval").Config);
                        timerMessageWindow.Start();
                        timerMessageWindow_Tick(null, null);
                    }
                    else
                    {
                        timerMessageWindow.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void timerGarbageCollect_Tick(object sender, EventArgs e)
        {
            MemoryUtil.FlushMemory();
        }

        /// <summary>
        /// 调整窗体大小时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Resize(object sender, EventArgs e)
        {
            try
            {
                //右上角按钮区坐标
                panelControlTopRight.Location = new Point(this.Width - panelControlTopRight.Width, 1);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawRectangle(Pens.Black, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
        }

        #region 会诊信息相关

        #region 获取会诊类型的消息， 获取完成后作废会诊提醒的记录
        /// <summary>
        /// 获取会诊类型的消息， 获取完成后作废会诊提醒的记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetConsultationInfo()
        {
            SqlParameter[] para = new SqlParameter[] { 
                  new SqlParameter("userid", SqlDbType.VarChar, 8)
            };
            para[0].Value = User.DoctorId;
            DataTable dataTable = SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetMessageInfo", para, CommandType.StoredProcedure);
            DeleteMessage(dataTable);
            return dataTable;
        }

        public void DeleteMessage(DataTable dataTable)
        {
            string sqlDeleteMessage = "UPDATE NURSE_WITHINFORMATION SET valid = 0 WHERE userid = '{0}' AND id = '{1}' AND valid = 1 ";
            foreach (DataRow dr in dataTable.Rows)
            {
                SqlHelper.ExecuteNoneQuery(string.Format(sqlDeleteMessage, User.DoctorId, dr["ID"].ToString()), CommandType.Text);
            }
        }
        #endregion


        /// <summary>
        /// edit by tj
        /// modify by xlb 2013-03-20
        /// 护士过滤记录只抓取待会诊
        /// </summary>
        /// <returns></returns>
        private DataTable GetConsultionData()
        {
            DataTable DtFilter = new DataTable();
            try
            {
                Employee emp = new Employee(this.User.Id);
                emp.ReInitializeProperties();

                if (emp.Grade.Trim() == "") return null;

                DataTable dt = new DataTable();
                SqlParameter[] sqlParams = new SqlParameter[] { 
                new SqlParameter("@Deptids", SqlDbType.VarChar, 255),
                new SqlParameter("@levelid", SqlDbType.VarChar, 255)//登录人级别
                };
                sqlParams[0].Value = this.User.CurrentDeptId.Trim(); // 科室代码
                sqlParams[1].Value = emp.Grade; // 当前登录人级别 add by ywk 2012年12月5日16:30:29
                dt = this.SqlHelper.ExecuteDataTable("emr_consultation.usp_GetConsultionMessage", sqlParams, CommandType.StoredProcedure);
                DtFilter = dt.Clone();
                if (dt.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(emp.Grade)) return null;

                    if (emp.Grade.Equals("2004")) //是护士
                    {
                        //显示所有会诊提醒
                        //return dt;
                        //var dtWait = from DataRow dr in dt.Rows where dr["stateid"].ToString().Equals("6730") select dr;
                        DataRow[] lookrow = dt.Select("stateid in ('6730')");
                        foreach (var item in lookrow)
                        {
                            DtFilter.ImportRow(item);
                        }
                    }
                    else //是医生
                    {
                        //用于记录插入表中的申请单号，对重复的记录进行过滤
                        List<string> applySnList = new List<string>();
                        List<string> userList = new List<string>();// 需要提醒的医师列表
                        #region 若会诊单中指定受邀人
                        //只显示‘待会诊’并且受邀人是本人
                        //DataRow[] Lookrow = dt.Select(" stateid in ('6730')");

                        DataRow[] Lookrow = dt.Select(string.Format(" stateid in ('6730') and (EmployeeCode='{0}' or applyuser='{0}')",
                            this.User.Id));                                                                 //取得受邀者或者申请者为本人所有记录

                        for (int i = Lookrow.Length - 1; Lookrow.Length > 0 && i >= 0; i--)
                        {
                            string applySn = Lookrow[i]["consultapplysn"].ToString();                       //申请单号
                            string shouyaoren = Lookrow[i]["shouyaoren"].ToString().Trim();                 //受邀人
                            if (applySnList.Contains(applySn))
                            {
                                continue;
                            }
                            //if (shouyaoren.Length > 0)//作为受邀医师
                            //{
                            if (!applySnList.Contains(applySn))//|| !userList.Contains(shouyaoren)
                            {
                                applySnList.Add(applySn);
                                //userList.Add(shouyaoren);
                                DtFilter.ImportRow(Lookrow[i]);
                                //continue;
                            }
                            //}
                            //string applyUserName = Lookrow[i]["applyusername"].ToString().Trim();
                            //if (applyUserName.Length > 0)//作为申请医师
                            //{
                            //    if (!applySnList.Contains(applySn) || !userList.Contains(applyUserName))//
                            //    {
                            //        applySnList.Add(applySn);
                            //        //userList.Add(applyUserName);
                            //        DtFilter.ImportRow(Lookrow[i]);
                            //    }
                            //}
                        }

                        #endregion

                        #region 会诊单中没有指定受邀人时，根据级别进行判断  //级别判断 暂时去掉
                        Lookrow = dt.Select(string.Format(" stateid in ('6730','6740','6741') and EmployeeCode is null and EmployeeLevelID = '{0}'", emp.Grade));
                        for (int i = Lookrow.Length - 1; Lookrow.Length > 0 && i >= 0; i--)
                        {
                            string applySn = Lookrow[i]["consultapplysn"].ToString();
                            if (!applySnList.Contains(applySn))
                            {
                                applySnList.Add(applySn);
                                DtFilter.ImportRow(Lookrow[i]);
                            }
                        }
                        #endregion

                        #region 已经会诊确认【签到】过的不用显示

                        string sqlGetConsultation = "select issignin from consultrecorddepartment where consultapplysn = '{0}'"
                            + " and employeecode = '" + this.User.Id + "' and valid = '1' ";

                        //排除作为受邀医师确认过会诊的
                        for (int i = DtFilter.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = DtFilter.Rows[i];
                            string consultApplySN = dr["consultapplysn"].ToString();
                            DataTable dtTemp = this.SqlHelper.ExecuteDataTable(string.Format(sqlGetConsultation, consultApplySN), CommandType.Text);
                            if (dtTemp.Rows.Count > 0)
                            {
                                if (dtTemp.Rows[0]["issignin"].ToString() == "1")//确认过会诊的排除
                                {
                                    DtFilter.Rows.Remove(dr);
                                }

                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                //MyMessageBox.Show(1, ex);//edit by ywk 2013年3月13日10:05:59  
                return null;
            }
            DataView dv = DtFilter.DefaultView;
            dv.Sort = " consulttime desc";
            return dv.ToTable();//DtFilter;
        }

        /// <summary>
        /// 第一次捞取会诊提醒的信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetConsultationInfoFirst()
        {
            try
            {
                DataTable dtTemp = new DataTable();
                DataTable dt = GetConsultionData();
                if (dt != null && dt.Rows.Count > 0)
                {
                    dtTemp.Columns.Add("ID");
                    dtTemp.Columns.Add("TYPEID");
                    dtTemp.Columns.Add("TYPENAME");
                    dtTemp.Columns.Add("CONTENT");

                    foreach (DataRow dr in dt.Rows)
                    {
                        DataRow drTemp = dtTemp.NewRow();
                        drTemp["ID"] = "0";
                        drTemp["TYPEID"] = "1";
                        drTemp["TYPENAME"] = "【会诊】";
                        drTemp["CONTENT"] = "病人：" + dr["INPATIENTNAME"].ToString() + " 时间：" + dr["CONSULTTIME"].ToString();
                        dtTemp.Rows.Add(drTemp);
                    }
                }
                return dtTemp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 是否正在处理程序，避免同时有多个程序一起运行
        /// </summary>
        bool m_IsInProcessingConsultaion = false;

        void timerMessageWindow_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!m_IsInProcessingConsultaion)
                {
                    (new MethodInvoker(ShowConsultationInfo)).BeginInvoke(null, null);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 是否是第一次捞取会诊信息
        /// 如果是第一次:   捞取医生需要会诊的记录，即未确认过的会诊记录
        /// 如果不是第一次：捞取消息表中的提醒记录 
        /// </summary>
        bool m_IsFirstGetConsultation = true;

        /// <summary>
        /// 显示会诊信息，以小窗体的形式弹出
        /// </summary>
        private void ShowConsultationInfo()
        {
            try
            {
                m_IsInProcessingConsultaion = true;
                if (m_IsFirstGetConsultation)
                {
                    DataTable dt = GetConsultationInfoFirst();
                    m_IsFirstGetConsultation = false;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ShowMessageWindow(dt, true);
                    }
                }
                else
                {
                    DataTable dt = GetConsultationInfo();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ShowMessageWindow(dt, false);
                    }
                }
                m_IsInProcessingConsultaion = false;
            }
            catch (Exception ex)
            {
                //暂时出错后不往外抛错误
            }
        }

        #endregion
    }
}