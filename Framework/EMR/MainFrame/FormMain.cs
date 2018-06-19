using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraTab;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Eop;
using DrectSoft.Core;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.Plugin;
using DrectSoft.FrameWork.Plugin.Manager;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Resources;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace DrectSoft.MainFrame
{
    public partial class FormMain : DevBaseForm, ICustomMessageBox, IEmrHost
    {
        private delegate void DeRestart();
        private const string s_PatientTable = "Inpatients";
        private const string s_BedTable = "Beds";
        private const string str_queryInpatient = "select * from InPatient where OutHosWard ='{0}' and  Status not in (1500, 1502, 1503, 1504, 1508, 1509)";
        private const string str_queryPatByID = "select * from InPatient where NOOFINPAT={0}";
        private const string str_querywardinfo = "usp_QueryInwardPatients";
        private const string str_queryDailyInfo = "usp_QueryWardInfo";
        private string m_Loadmenufile = string.Empty;
        private bool m_Isconfig = false;
        private IUser m_loguser = null;
        private PluginManager m_manager;
        private PluginMenuRegister m_register;
        private bool m_needChanged = true;
        private PluginStatusBarRegister m_registerStatusbar;
        private MyMessageBox m_messagebox;
        private IDataAccess m_sqlHelper = null;
        private Account m_account;
        private Account m_MasterAccount;
        private bool s_InterSetTime;
        internal static FormMain Instance;
        private bool m_InvokeClosing = true;
        private string m_LockTime = string.Empty;
        private bool m_EmrAllowEdit = true;
        private PluginUtil m_PublicMethod;
        private EmrDefaultSetting m_EmrDefaultSetting;
        private Inpatient _currentPat;
        private DataSet _patientInfos;
        private WaitDialogForm m_WaitForm;
        private string m_CurrentSelectedEmrID = string.Empty;
        private WaitDialogForm m_WaitDialog;
        public bool otherUser = false;
        public FormLogin xxm_FormLogin = null;
        public bool? isLG = new bool?(false);
        private bool m_IsFristShowLogin = true;
        private SqlParameter[] _sqlParams;
        private bool m_IsEnterFormClosing = false;
        private HospitalInfo _currentHospitalInfo = null;
        private DrectSoftLog _logger;
        private UCMessageWindow m_UCMessageWindow;
        private int m_GetMessageWindownInterval = 0;
        private IContainer components = null;


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

        private string LockTime
        {
            get
            {
                if (string.IsNullOrEmpty(this.m_LockTime))
                {
                    try
                    {
                        this.m_LockTime = BasicSettings.GetStringConfig("SYSLOCKTIME");
                    }
                    catch
                    {
                        this.m_LockTime = "1";
                    }
                }
                return this.m_LockTime;
            }
        }

        private MyMessageBox MessageBoxInstance
        {
            get
            {
                if (this.m_messagebox == null)
                {
                    this.m_messagebox = new MyMessageBox();
                }
                return this.m_messagebox;
            }
        }

        public Inpatient CurrentPatientInfo
        {
            get
            {
                return this._currentPat;
            }
            set
            {
                CancelEventArgs cancelEventArgs = new CancelEventArgs();
                this.SetPluginForPatientChanging(cancelEventArgs);
                if (!cancelEventArgs.Cancel)
                {
                    this._currentPat = value;
                    this.SetPluginForPatientChange();
                }
            }
        }

        Inpatient _newcurrentPat;
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

        public ICustomMessageBox CustomMessageBox
        {
            get
            {
                return this;
            }
        }

        public IDataAccess SqlHelper
        {
            get
            {
                if (this.m_sqlHelper == null)
                {
                    DataAccessFactory.GetSqlDataAccess();
                }
                return DataAccessFactory.DefaultDataAccess;
            }
        }
        /// <summary>
        /// 获取MAC地址
        /// </summary>
        public string MacAddress
        {
            get
            {
                ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection instances = managementClass.GetInstances();
                string result = string.Empty;
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        ManagementObject managementObject = (ManagementObject)enumerator.Current;
                        if ((bool)managementObject["IPEnabled"])
                        {
                            result = managementObject["MacAddress"].ToString().Replace(":", "");
                            break;
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 获取IP地址属性
        /// </summary>
        public string Ip
        {
            get
            {
                return this.GetIPStr();
            }
        }
        public bool GetUser(string UserId)
        {
            bool result = true;
            DataTable dataTable = SqlHelper.ExecuteDataTable(string.Format("select * from users where id = '{0}'", UserId), CommandType.Text);
            if (dataTable == null || dataTable.Rows.Count <= 0)
                result = false;
            return result;


        }
        public IAppConfigReader AppConfig
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DataSet PatientInfos
        {
            get
            {
                if (this._patientInfos == null)
                {
                    this._patientInfos = this.GetPatientInfoData(this.m_loguser.CurrentDeptId, this.m_loguser.CurrentWardId);
                }
                return this._patientInfos;
            }
        }

        public IUser User
        {
            get
            {
                return this.m_loguser;
            }
        }

        public Collection<PlugInConfiguration> PrivilegeMenu
        {
            get
            {
                return this.m_manager.PrivilegeMenu;
            }
        }

        public PluginManager Manager
        {
            get
            {
                return this.m_manager;
            }
        }

        public bool EmrAllowEdit
        {
            get
            {
                return this.m_EmrAllowEdit;
            }
            set
            {
                this.m_EmrAllowEdit = value;
            }
        }

        public PluginUtil PublicMethod
        {
            get
            {
                if (this.m_PublicMethod == null)
                {
                    this.m_PublicMethod = new PluginUtil(SqlHelper.GetDatabase());
                }
                return this.m_PublicMethod;
            }
        }

        public EmrDefaultSetting EmrDefaultSettings
        {
            get
            {
                if (this.m_EmrDefaultSetting == null)
                {
                    this.m_EmrDefaultSetting = Util.InitEmrDefaultSet();
                }
                return this.m_EmrDefaultSetting;
            }
        }

        public string CurrentSelectedEmrID
        {
            get
            {
                return this.m_CurrentSelectedEmrID;
            }
            set
            {
                this.m_CurrentSelectedEmrID = value;
            }
        }

        public FormLogin m_FormLogin
        {
            get
            {
                if (this.xxm_FormLogin == null)
                {
                    this.xxm_FormLogin = new FormLogin(this);
                }
                return this.xxm_FormLogin;
            }
        }

        private SqlParameter[] SqlParams
        {
            get
            {
                if (this._sqlParams == null)
                {
                    this._sqlParams = new SqlParameter[]
					{
						new SqlParameter("Deptids", SqlDbType.VarChar, 8),
						new SqlParameter("Wardid", SqlDbType.VarChar, 8)
					};
                }
                return this._sqlParams;
            }
        }

        public HospitalInfo CurrentHospitalInfo
        {
            get
            {
                if (this._currentHospitalInfo == null)
                {
                    this.GetHospitalInfo();
                }
                return this._currentHospitalInfo;
            }
        }

        public DrectSoftLog Logger
        {
            get
            {
                if (this._logger == null)
                {
                    this._logger = new DrectSoftLog();
                }
                return this._logger;
            }
        }

        public string RefreshPatientInfos()
        {
            this._patientInfos = this.GetPatientInfoData(this.m_loguser.CurrentDeptId, this.m_loguser.CurrentWardId);
            return string.Empty;
        }

        private void SetWaitDialogCaption(string caption)
        {
            if (this.m_WaitForm == null)
            {
                this.m_WaitForm = new WaitDialogForm("正在读取数据...", "请稍等");
            }
            this.m_WaitForm.SetCaption(caption);
        }

        private void HideWaitDialog()
        {
            this.m_WaitForm.Hide();
        }

        public FormMain(bool isconfig, string menufile, bool otnehr)
        {
            this.InitializeComponent();
            this.InitImages();
            this.m_Isconfig = isconfig;
            this.m_manager = new PluginManager(this, this.Logger);
            this.m_register = new PluginMenuRegister();
            this.m_manager.SetMenuRegister(this.m_register);
            this.m_Loadmenufile = menufile;
            FormMain.Instance = this;
            if (!this.isLG.HasValue || otnehr)
            {
                this.otherUser = true;
                base.ShowIcon = false;
                base.ShowInTaskbar = false;
                base.Size = new Size(1, 1);
                base.Location = new Point(100000, 100000);
                base.Visible = false;
                base.ShowInTaskbar = false;
            }
        }

        private static void ForceSynchLocalSystemTime()
        {
            DateTime serverTime = DataAccessFactory.DefaultDataAccess.GetServerTime();
            if (serverTime.ToString("yyyy-MM-dd HH:mm") != DateTime.Now.ToString("yyyy-MM-dd HH:mm"))
            {
                serverTime.Set2LocalSystemTime();
            }
        }

        private void SystemTimeChanged(object sender, EventArgs e)
        {
            if (this.s_InterSetTime)
            {
                this.s_InterSetTime = false;
            }
            else
            {
                this.s_InterSetTime = true;
                SystemEvents.TimeChanged -= new EventHandler(this.SystemTimeChanged);
                FormMain.ForceSynchLocalSystemTime();
                SystemEvents.TimeChanged += new EventHandler(this.SystemTimeChanged);
            }
        }

        private void InitImages()
        {
            base.Icon = ResourceManager.DrectSoftLogo;
        }

        private void UpdateServerTime2LocalTime()
        {
            DateTime serverTime = this.SqlHelper.GetServerTime();
            serverTime.Set2LocalSystemTime();
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        private string GetIPStr()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry hostByName = Dns.GetHostByName(hostName);
            string result;
            if (hostByName != null && hostByName.AddressList != null && hostByName.AddressList.Length > 0)
            {
                string text = hostByName.AddressList[0].ToString();
                result = text;
            }
            else
            {
                result = "0.0.0.0";
            }
            return result;
        }

        private void InitializePlugins()
        {
            this.m_register.ClearChildRegisters();
            if (!this.m_Isconfig)
            {
            }
            if (string.IsNullOrEmpty(this.m_Loadmenufile))
            {
                this.m_manager.RegisterPlugins(Application.StartupPath, new string[0]);
            }
            else
            {
                this.m_manager.RegisterPlugins(Application.StartupPath, new string[]
				{
					this.m_Loadmenufile
				});
            }
            if (string.IsNullOrEmpty(this.m_account.User.MasterID))
            {
                this.m_manager.RegisterMenuPlugins(this.m_account);
            }
            else
            {
                this.m_manager.RegisterMenuPlugins(this.m_MasterAccount);
            }
            this.m_manager.Runner.LoadPlugIn("DrectSoft.Core.MouldList.dll", "DrectSoft.Core.MouldList.FormMouldList", true);
        }

        private void DoLock()
        {
            this.timerLock.Enabled = false;
            FormLock formLock = new FormLock();
            formLock.ShowDialog();
            if (formLock.DialogResult != DialogResult.OK)
            {
                if (formLock.DialogResult == DialogResult.Retry)
                {
                    this.m_manager.Runner.CloseAllPlugins();
                    FormLogin formLogin = new FormLogin();
                    if (DialogResult.OK == formLogin.ShowDialog())
                    {
                        this.InitializePlugins();
                    }
                }
                else
                {
                    this.Restart();
                }
            }
            this.timerLock.Enabled = true;
            this.timerGarbageCollect.Enabled = true;
        }

        private void Restart()
        {
            Application.ExitThread();
            Thread thread = new Thread(new ParameterizedThreadStart(this.ApStart));
            object executablePath = Application.ExecutablePath;
            Thread.Sleep(2000);
            thread.Start(executablePath);
        }

        private void ApStart(object obj)
        {
            new Process
            {
                StartInfo =
                {
                    FileName = obj.ToString()
                }
            }.Start();
        }

        private void RegisterStatusBar()
        {
            this.barEditItemWard.Caption = string.Concat(new string[]
			{
				this.m_loguser.CurrentDeptId,
				":",
				this.m_loguser.CurrentDeptName,
				"/",
				this.m_loguser.CurrentWardId,
				":",
				this.m_loguser.CurrentWardName
			});
            this.barButtonName.Caption = this.m_loguser.Name + "         ";
            this.barStaticItemTime.Caption = "系统时间:" + DateTime.Today.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) + " " + DateTime.Now.ToString("T", DateTimeFormatInfo.InvariantInfo);
            this.barStaticItemIP.Caption = "本机IP:" + this.Ip;
        }

        private void ChangePassword()
        {
            FormChangePassword formChangePassword = new FormChangePassword(this);
            //formChangePassword.ShowDialog();
        }

        private void SetPluginForUserChange()
        {
            base.SuspendLayout();
            this.m_manager.Runner.UserChangeExecuted.Clear();
            List<IPlugIn> pluginsWithMainFormLoaded = this.m_manager.Runner.PluginsWithMainFormLoaded;
            if (pluginsWithMainFormLoaded.Count == 1)
            {
                IPlugIn plugIn = pluginsWithMainFormLoaded[0];
                plugIn.ExecuteUsersChangeEvent(new UserArgs(this.User));
                this.m_manager.Runner.UserChangeExecuted.Add(plugIn);
            }
            else if (this.m_manager.Runner.ActivePlugIn != null)
            {
                this.m_manager.Runner.ActivePlugIn.ExecuteUsersChangeEvent(new UserArgs(this.User));
                this.m_manager.Runner.UserChangeExecuted.Add(this.m_manager.Runner.ActivePlugIn);
            }
            if (this.m_manager.Runner.StartPlugins != null && this.m_manager.Runner.StartPlugins.Count != 0)
            {
                foreach (IPlugIn current in this.m_manager.Runner.StartPlugins)
                {
                    if (this.m_manager.Runner.PlugInIndexInList(current.AssemblyFileName, current.StartClassType, this.m_manager.Runner.PatientChangeExecuted) < 0)
                    {
                        current.ExecuteUsersChangeEvent(new UserArgs(this.User));
                    }
                }
            }
            base.ResumeLayout();
        }

        private void SetPluginForPatientChange()
        {
            base.SuspendLayout();
            this.m_manager.Runner.PatientChangeExecuted.Clear();
            List<IPlugIn> pluginsWithMainFormLoaded = this.m_manager.Runner.PluginsWithMainFormLoaded;
            if (pluginsWithMainFormLoaded.Count == 1)
            {
                IPlugIn plugIn = pluginsWithMainFormLoaded[0];
                plugIn.ExecutePatientChangeEvent(new PatientArgs(this.CurrentPatientInfo));
                this.m_manager.Runner.PatientChangeExecuted.Add(plugIn);
            }
            else if (this.m_manager.Runner.PluginsLoaded != null && this.m_manager.Runner.PluginsLoaded.Count != 0)
            {
                foreach (IPlugIn current in this.m_manager.Runner.PluginsLoaded)
                {
                    if (this.m_manager.Runner.PlugInIndexInList(current.AssemblyFileName, current.StartClassType, this.m_manager.Runner.PatientChangeExecuted) < 0)
                    {
                        current.ExecutePatientChangeEvent(new PatientArgs(this.CurrentPatientInfo));
                        this.m_manager.Runner.PatientChangeExecuted.Add(current);
                    }
                }
            }
            base.ResumeLayout();
        }

        private void SetPluginForPatientChanging(CancelEventArgs e)
        {
            base.SuspendLayout();
            if (this.m_manager.Runner.ActivePlugIn != null)
            {
                this.m_manager.Runner.ActivePlugIn.ExecutePatientChangingEvent(e);
            }
            if (!e.Cancel)
            {
                foreach (IPlugIn plugIn in this.m_manager.Runner.PluginsWithMainFormLoaded)
                {
                    if (plugIn != this.m_manager.Runner.ActivePlugIn)
                    {
                        plugIn.ExecutePatientChangingEvent(e);
                        if (e.Cancel)
                        {
                            break;
                        }
                    }
                }
            }
            List<IPlugIn> pluginsWithMainFormLoaded = this.m_manager.Runner.PluginsWithMainFormLoaded;
            if (pluginsWithMainFormLoaded.Count == 1)
            {
                IPlugIn plugIn = pluginsWithMainFormLoaded[0];
                plugIn.ExecutePatientChangingEvent(e);
            }
            else if (!e.Cancel && this.m_manager.Runner.StartPlugins != null && this.m_manager.Runner.StartPlugins.Count != 0)
            {
                foreach (IPlugIn current in this.m_manager.Runner.StartPlugins)
                {
                    if (this.m_manager.Runner.PlugInIndexInList(current.AssemblyFileName, current.StartClassType, this.m_manager.Runner.PatientChangeExecuted) < 0)
                    {
                        current.ExecutePatientChangingEvent(e);
                    }
                    if (e.Cancel)
                    {
                        break;
                    }
                }
            }
            base.ResumeLayout();
        }

        private void InitFunctionBar()
        {
        }

        public bool Login()
        {
            this.m_FormLogin.ontherFlg = this.otherUser;
            this.m_FormLogin.Icon = base.Icon;
            this.m_FormLogin.ShowDialog();
            bool result;
            if (this.m_FormLogin.DialogResult == DialogResult.OK)
            {
                if (!this.isLG.HasValue)
                {
                    this.isLG = new bool?(true);
                }
                result = true;
            }
            else
            {
                if (!this.isLG.HasValue)
                {
                    this.isLG = new bool?(false);
                }
                result = false;
            }
            return result;
        }

        public void ProcessLogin()
        {
            try
            {
                this.timerLock.Enabled = true;
                this.m_account = this.m_FormLogin.CurrentAccount;
                this.m_MasterAccount = this.m_FormLogin.MasterAccount;
                this.m_loguser = this.m_account.User;
                this.m_loguser.CurrentDeptWardChanged += new EventHandler(this.m_loguser_CurrentDeptWardChanged);
                this.m_FormLogin.ChangeProgressBar(21, 25);
                this.InitializePlugins();
                this.m_FormLogin.ChangeProgressBar(26, 40);
                this.RegisterStatusBar();
                this.m_FormLogin.ChangeProgressBar(41, 45);
                this.SetPluginForUserChange();
                if (this.m_IsFristShowLogin)
                {
                    this.m_IsFristShowLogin = false;
                    this.m_FormLogin.ChangeProgressBar(46, 50);
                    FormMain.ForceSynchLocalSystemTime();
                    this.m_FormLogin.ChangeProgressBar(51, 60);
                    this.UpdateServerTime2LocalTime();
                    this.InitFunctionBar();
                    this.m_FormLogin.ChangeProgressBar(61, 70);
                    this.SetTabbedMdiHead();
                    this.m_FormLogin.ChangeProgressBar(71, 80);
                    SystemEvents.TimeChanged += new EventHandler(this.SystemTimeChanged);
                    base.WindowState = FormWindowState.Maximized;
                    this.m_FormLogin.ChangeProgressBar(81, 90);
                    base.Show();
                    this.ShowConsultationInfo();
                    this.InitTimerMessageWindow();
                }
            }
            catch
            {
            }
        }

        private DataSet GetPatientInfoData(string dept, string ward)
        {
            DataSet dataSet = new DataSet();
            if (string.IsNullOrEmpty(dept))
            {
                dept = "3225";
                ward = "2922";
            }
            this.SqlParams[0].Value = dept;
            this.SqlParams[1].Value = ward;
            DataTable dataTable = this.SqlHelper.ExecuteDataTable(string.Format("select * from InPatient where OutHosWard ='{0}' and  Status not in (1500, 1502, 1503, 1504, 1508, 1509)", ward));
            dataTable.TableName = "病人数据集";
            dataSet.Tables.Add(dataTable.Copy());
            DataTable dataTable2 = this.SqlHelper.ExecuteDataTable("usp_QueryInwardPatients", this.SqlParams, CommandType.StoredProcedure);
            dataTable2.TableName = "床位信息";
            dataTable2.Columns["BedID"].Caption = "床位";
            dataTable2.Columns["PatName"].Caption = "患者姓名";
            dataTable2.Columns["Sex"].Caption = "性别";
            dataTable2.Columns["AgeStr"].Caption = "年龄";
            dataTable2.Columns["PatID"].Caption = "住院号";
            dataTable2.Columns["AdmitDate"].Caption = "入院日期";
            dataSet.Tables.Add(dataTable2.Copy());
            return dataSet;
        }

        private DataRow GetPatInfo(decimal noOfInpat)
        {
            DataTable dataTable = this.SqlHelper.ExecuteDataTable(string.Format("select * from InPatient where NOOFINPAT={0}", noOfInpat));
            DataRow result;
            if (dataTable.Rows.Count < 1)
            {
                result = null;
            }
            else
            {
                result = dataTable.Rows[0];
            }
            return result;
        }

        public void ChoosePatient(decimal firstPageNo)
        {
            this.SetWaitDialogCaption("系统正在切换病人，请您稍候");
            this.SetPatientInfo(firstPageNo);
            this.HideWaitDialog();
        }

        public void ChoosePatient(decimal firstPageNo, string floaderState)
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
        public void ChoosePatient(string firstPageNo, out Inpatient m_Inpat)
        {

            SetWaitDialogCaption("系统正在切换病人，请稍候...");
            m_FloderState = string.Empty;
            SetPatientInfo(firstPageNo, out  m_Inpat);
            HideWaitDialog();

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


        private DataRow GetPatInfo(string noOfInpat)
        {
            DataTable table = SqlHelper.ExecuteDataTable(string.Format(str_queryPatByID, noOfInpat));

            if (table.Rows.Count < 1) return null;
            return table.Rows[0];

        }
        private void SetPatientInfo(decimal firstPageNo)
        {
            this.xtraTabbedMdiManager1.SelectedPageChanged -= new EventHandler(this.xtraTabbedMdiManager1_SelectedPageChanged);
            DataRow[] array = this.PatientInfos.Tables[0].Select("NoOfInpat = " + firstPageNo.ToString());
            if (array != null && array.Length > 0)
            {
                this.CurrentPatientInfo = new Inpatient(array[0]);
            }
            else
            {
                DataRow patInfo = this.GetPatInfo(firstPageNo);
                if (patInfo != null)
                {
                    this.CurrentPatientInfo = new Inpatient(patInfo);
                }
            }
            this.xtraTabbedMdiManager1.SelectedPageChanged += new EventHandler(this.xtraTabbedMdiManager1_SelectedPageChanged);
        }

        private void SetActivePlugInForRecordInput()
        {
            string text = "DrectSoft.Core.RecordsInput.dll";
            if (!File.Exists(text))
            {
                throw new FileNotFoundException("程序集" + text + "不存在!");
            }
            foreach (IPlugIn current in this.m_manager.Runner.PluginsLoaded)
            {
                if (text == current.AssemblyFileName)
                {
                    this.m_manager.Runner.ActivePlugIn = current;
                    break;
                }
            }
        }

        public bool LoadPlugIn(string assemblyName, string startupClassName)
        {
            bool result;
            try
            {
                result = (this.m_manager.Runner.LoadPlugIn(assemblyName, startupClassName, true) != null);
            }
            catch (Exception var_0_1E)
            {
                result = false;
            }
            return result;
        }

        public bool LoadPlugIn(string typeName)
        {
            bool result;
            try
            {
                Type type = Type.GetType(typeName, true, true);
                string name = type.Assembly.ManifestModule.Name;
                string fullName = type.FullName;
                result = (this.m_manager.Runner.LoadPlugIn(name, fullName, true) != null);
            }
            catch (Exception var_3_40)
            {
                result = false;
            }
            return result;
        }

        public DialogResult MessageShow(string message)
        {
            return this.MessageShow(message, CustomMessageBoxKind.InformationOk);
        }

        public DialogResult MessageShow(string message, CustomMessageBoxKind kind)
        {
            Collection<string> collection = new Collection<string>();
            collection.Add(message);
            this.MessageBoxInstance.SetMessageInfo(collection);
            this.MessageBoxInstance.SetButtonInfo(kind);
            return this.m_messagebox.ShowDialog();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                this.DoubleBuffered = true;
                if (!this.isLG.HasValue)
                {
                    base.ShowIcon = false;
                    base.Size = new Size(1, 1);
                    base.Location = new Point(100000, 100000);
                    base.Visible = false;
                    base.Hide();
                    this.Refresh();
                }
                else if (!this.Login())
                {
                    this.m_InvokeClosing = false;
                    base.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void m_loguser_CurrentDeptWardChanged(object sender, EventArgs e)
        {
            this.barEditItemWard.Caption = string.Concat(new string[]
			{
				this.m_loguser.CurrentDeptId,
				":",
				this.m_loguser.CurrentDeptName,
				"/",
				this.m_loguser.CurrentWardId,
				":",
				this.m_loguser.CurrentWardName
			});
            this.barButtonName.Caption = this.m_loguser.Name + "         ";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.barStaticItemTime.Caption = "系统时间:" + DateTime.Today.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) + " " + DateTime.Now.ToString("T", DateTimeFormatInfo.InvariantInfo);
        }

        private void tsmiLogin_Click(object sender, EventArgs e)
        {
        }

        private void barButtonlogin_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ReLogin();
        }

        private void ReLogin()
        {
            base.Hide();
            this.m_manager.Runner.CloseAllPlugins();
            if (!this.Login())
            {
                this.m_InvokeClosing = false;
                base.Close();
            }
            base.Activate();
            base.Show();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.otherUser)
            {
                if (!this.m_IsEnterFormClosing)
                {
                    if (!this.ExitApplication())
                    {
                        e.Cancel = true;
                    }
                }
                if (e.Cancel)
                {
                }
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            PlugInLoadHelper.UnLoadPlugIn(Application.StartupPath);
        }

        private void timerLock_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.m_LockTime))
            {
                if (Util.GetLastInputTime() > double.Parse(this.m_LockTime))
                {
                    this.DoLock();
                }
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void barButtonMin_ItemClick(object sender, ItemClickEventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
        }

        private void barItemChangeWard_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ChangeWard();
        }

        private void ChangeWard()
        {
            FormItemFunction formItemFunction = new FormItemFunction();
            if (DialogResult.OK == formItemFunction.ShowDialog())
            {
                ((Users)this.m_loguser).SetCurrentDeptWard(formItemFunction.SelectDept);
                this.RefreshPatientInfos();
            }
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            int height = Screen.PrimaryScreen.WorkingArea.Height;
            int width = Screen.PrimaryScreen.WorkingArea.Width;
            base.MaximizedBounds = new Rectangle(0, 0, width + 2, height + 2);
        }

        private void SetTabbedMdiHead()
        {
            this.xtraTabbedMdiManager1.SelectedPageChanged += new EventHandler(this.xtraTabbedMdiManager1_SelectedPageChanged);
            this.xtraTabbedMdiManager1.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeader;
            if (this.xtraTabbedMdiManager1.Pages.Count > 0)
            {
                this.xtraTabbedMdiManager1.Pages[0].ShowCloseButton = DefaultBoolean.False;
            }
            this.simpleButtonHelp.ToolTip = "帮助";
            this.simpleButtonChangeArea.ToolTip = "切换病区";
            this.simpleButtonReLogin.ToolTip = "重新登陆";
            this.simpleButtonChangePassword.ToolTip = "修改密码";
            this.simpleButtonMinimum.ToolTip = "最小化";
            this.simpleButtonNormal.ToolTip = "窗体化";
            this.simpleButtonClose.ToolTip = "退出";
            Size size = new Size(14, 14);
            this.simpleButtonHelp.Image = ResourceManager.GetImage("HelpTopRight.png");
            this.simpleButtonChangeArea.Image = ResourceManager.GetImage("ChangeWardTopRight.png");
            this.simpleButtonReLogin.Image = ResourceManager.GetImage("ReLoginTopRight.png");
            this.simpleButtonChangePassword.Image = ResourceManager.GetImage("ChangePasswordTopRight.png");
            this.simpleButtonMinimum.Image = ResourceManager.GetImage("MinimizeTopRight.png");
            this.simpleButtonClose.Image = ResourceManager.GetImage("ShutdownTopRight.png");
            this.simpleButtonNormal.Image = ResourceManager.GetImage("窗体化.bmp");
            this.simpleButtonHelp.Click += new EventHandler(this.simpleButtonHelp_Click);
            this.simpleButtonChangeArea.Click += new EventHandler(this.simpleButtonChangeArea_Click);
            this.simpleButtonReLogin.Click += new EventHandler(this.simpleButtonReLogin_Click);
            this.simpleButtonChangePassword.Click += new EventHandler(this.simpleButtonChangePassword_Click);
            this.simpleButtonMinimum.Click += new EventHandler(this.simpleButtonMinimum_Click);
            this.simpleButtonClose.Click += new EventHandler(this.simpleButtonClose_Click);
            this.simpleButtonNormal.Click += new EventHandler(this.simpleButtonNormal_Click);
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.panelControlTopRight.Location = new Point(workingArea.Width - this.panelControlTopRight.Width, 1);
            this.panelControlTopRight.Height = 21;
        }

        private void xtraTabbedMdiManager1_SelectedPageChanged(object sender, EventArgs e)
        {
            if (this.xtraTabbedMdiManager1.SelectedPage != null)
            {
                string text = this.xtraTabbedMdiManager1.SelectedPage.Text;
                foreach (IPlugIn current in this.m_manager.Runner.PluginsLoaded)
                {
                    if (text == current.MainForm.Text)
                    {
                        this.m_manager.Runner.ActivePlugIn = current;
                        break;
                    }
                }
            }
        }

        private void simpleButtonClose_Click(object sender, EventArgs e)
        {
            if (!this.otherUser)
            {
                this.ExitApplication();
            }
        }

        private bool ExitApplication()
        {
            bool result;
            if (!this.otherUser)
            {
                if (this.CustomMessageBox.MessageShow("是否要退出系统?", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                {
                    this.m_IsEnterFormClosing = true;
                    try
                    {
                        Application.Exit();
                    }
                    catch (Exception var_0_39)
                    {
                    }
                    Environment.Exit(Environment.ExitCode);
                    base.Dispose();
                    base.Close();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        private void simpleButtonMinimum_Click(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
        }

        private void simpleButtonChangePassword_Click(object sender, EventArgs e)
        {
            this.ChangePassword();
        }

        private void simpleButtonReLogin_Click(object sender, EventArgs e)
        {
            if (this.CustomMessageBox.MessageShow("是否要重新登陆?", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                this.m_manager.Runner.CloseAllPlugins();
                this.ReLogin();
            }
        }

        private void simpleButtonChangeArea_Click(object sender, EventArgs e)
        {
            this.ChangeWard();
        }

        private void simpleButtonHelp_Click(object sender, EventArgs e)
        {
            string text = Application.StartupPath.ToString() + "\\电子病历用户使用手册.chm";
            if (File.Exists(text))
            {
                Process.Start(Application.StartupPath.ToString() + "\\电子病历用户使用手册.chm");
            }
            else
            {
                this.CustomMessageBox.MessageShow("帮助文件" + text + "不存在", CustomMessageBoxKind.ErrorYes);
            }
        }

        private void simpleButtonNormal_Click(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Normal)
            {
                base.WindowState = FormWindowState.Maximized;
            }
            else
            {
                base.WindowState = FormWindowState.Normal;
            }
        }

        private void GetHospitalInfo()
        {
            try
            {
                DataTable dataTable = this.SqlHelper.ExecuteDataTable("select a.id,\r\n                                                                       a.name,\r\n                                                                       a.subname,\r\n                                                                       a.sn,\r\n                                                                       a.medicalid,\r\n                                                                       a.address,\r\n                                                                       a.yzbm,\r\n                                                                       a.tel,\r\n                                                                       nvl(a.bzcws, '0') bzcws,\r\n                                                                       a.memo\r\n                                                                  from hospitalinfo a where rownum < 2");
                HospitalInfo hospitalInfo = new HospitalInfo();
                if (dataTable.Rows.Count > 0)
                {
                    DataRow dataRow = dataTable.Rows[0];
                    hospitalInfo.Id = dataRow["ID"].ToString();
                    hospitalInfo.Name = dataRow["Name"].ToString();
                    hospitalInfo.Subname = dataRow["SubName"].ToString();
                    hospitalInfo.Sn = dataRow["SN"].ToString();
                    hospitalInfo.Medicalid = dataRow["MedicalId"].ToString();
                    hospitalInfo.Address = dataRow["Address"].ToString();
                    hospitalInfo.Yzbm = dataRow["Yzbm"].ToString();
                    hospitalInfo.Tel = dataRow["Tel"].ToString();
                    hospitalInfo.Bzcws = Convert.ToInt32(dataRow["Bzcws"].ToString());
                    hospitalInfo.Memo = dataRow["Memo"].ToString();
                }
                this._currentHospitalInfo = hospitalInfo;
            }
            catch (SqlException var_3_137)
            {
            }
        }

        public void ShowMessageWindow(DataTable dt, bool isClear)
        {
            if (this.m_UCMessageWindow == null)
            {
                string config = new AppConfigReader().GetConfig("IsOpenMessageWindow").Config;
                string[] array = config.Split(new char[]
				{
					','
				});
                if (array.Length == 3)
                {
                    this.m_UCMessageWindow = new UCMessageWindow(array[0], array[1], array[2]);
                }
            }
            if (this.m_UCMessageWindow.IsShowMessageWindow)
            {
                this.m_UCMessageWindow.ShowMessageWindow(this, dt, isClear);
            }
        }

        private void InitTimerMessageWindow()
        {
            AppConfigReader appConfigReader = new AppConfigReader();
            this.m_GetMessageWindownInterval = Convert.ToInt32(appConfigReader.GetConfig("DocCenterTimeInterval").Config);
            this.timerMessageWindow.Interval = this.m_GetMessageWindownInterval;
            this.timerMessageWindow.Tick += new EventHandler(this.timerMessageWindow_Tick);
            this.timerMessageWindow.Enabled = true;
        }

        private void timerMessageWindow_Tick(object sender, EventArgs e)
        {
            this.ShowConsultationInfo();
        }

        private void ShowConsultationInfo()
        {
            DataTable consultationInfo = this.GetConsultationInfo();
            if (consultationInfo.Rows.Count > 0)
            {
                this.ShowMessageWindow(consultationInfo, false);
            }
        }

        public DataTable GetConsultationInfo()
        {
            SqlParameter[] array = new SqlParameter[]
			{
				new SqlParameter("userid", SqlDbType.VarChar, 8)
			};
            array[0].Value = this.User.DoctorId;
            DataTable dataTable = this.SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetMessageInfo", array, CommandType.StoredProcedure);
            this.DeleteMessage(dataTable);
            return dataTable;
        }

        public void DeleteMessage(DataTable dataTable)
        {
            string format = "UPDATE NURSE_WITHINFORMATION SET valid = 0 WHERE userid = '{0}' AND id = '{1}' AND valid = 1 AND typeid = 1";
            foreach (DataRow dataRow in dataTable.Rows)
            {
                this.SqlHelper.ExecuteNoneQuery(string.Format(format, this.User.DoctorId, dataRow["ID"].ToString()), CommandType.Text);
            }
        }

        private void timerGarbageCollect_Tick(object sender, EventArgs e)
        {
            MemoryUtil.FlushMemory();
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            this.panelControlTopRight.Location = new Point(base.Width - this.panelControlTopRight.Width, 1);
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
