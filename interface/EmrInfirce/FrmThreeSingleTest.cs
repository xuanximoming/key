using DrectSoft.Common.Eop;
using DrectSoft.Core;
using DrectSoft.Core.NursingDocuments;
using DrectSoft.Emr.QcManager;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using EmrInsert;
using System;
using System.Data;
using System.Windows.Forms;

namespace EmrInfirce
{
    /// <summary>
    /// 医生写病历
    /// </summary>
    public partial class FrmThreeSingleTest : Form, IEmrHost
    {
        EmrDataHelper emrHelper = null;
        Inpatient _currentPat;
        private decimal _noOfInpat;
        private const string str_queryPatByID = "select * from InPatient where NOOFINPAT={0}";

        public static IEmrHost _EmrHost = null;

        public string InpRegId = "";
        public DrectSoft.MainFrame.FormMain _Formain = null;
        DrectSoft.MainFrame.FormMain Formain
        {
            get
            {
                if (_Formain == null)
                {
                    _Formain = new DrectSoft.MainFrame.FormMain(false, "file.menu", true);
                    _Formain.isLG = null;
                }
                return _Formain;
            }
        }
        public FrmThreeSingleTest()
        {
            InitializeComponent();
            // BindEmr(4983);
        }

        public void BindEmr(decimal noOfInpat)
        {
            _noOfInpat = noOfInpat;
            EmrLogin();
            AddControls();
        }

        private void EmrLogin()
        {
            if (Formain.isLG == null)
            {
                EmrDataHelper emr = new EmrDataHelper();
                emr.thisLogin();
                _EmrHost = emr.Formain;
                //_Formain.Size = new Size(0, 0);
            }
            else
            {
                _EmrHost = Formain;
            }
            if (emrHelper == null)
            {
                emrHelper = new EmrDataHelper();
                emrHelper.m_SqlHelper = Formain.SqlHelper;
            }
        }
        QcManagerForm m_UCEmrInput = null;
        /// <summary>
        /// 添加控件
        /// </summary>
        private void AddControls()
        {

        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="noOfInpat">住院号</param>
        public bool Shuaxin(string inid)
        {
            try
            {
                if (_EmrHost == null)
                {
                    emrHelper = new EmrDataHelper();
                    emrHelper.thisLogin();
                    if (emrHelper.isLoginResult == false)
                    {
                        _EmrHost = null;
                        this.DialogResult = DialogResult.No;
                        return false;
                    }
                    _EmrHost = emrHelper.Formain;
                    _Formain = emrHelper.Formain;
                }

                //***************************************
                //IEMREditor editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString() });
                IEMREditor editor;
                DataTable dt = emrHelper.SelectDataBase(string.Format("select * from InPatient where PatNoOfHis='{0}'", inid));
                if (dt == null || dt.Rows.Count <= 0)
                    throw new Exception("病历数据未同步。");
                _EmrHost.ChoosePatient(Convert.ToDecimal(dt.Rows[0]["noOfInpat"]));
                RecordDal m_RecordDal = new RecordDal(_EmrHost.SqlHelper);
                MainNursingMeasure nurse = new MainNursingMeasure();
                editor = (IEMREditor)nurse;
                editor.ReadOnlyControl = false;
                editor.DesignUI.Dock = DockStyle.Fill;
                this.Controls.Add(editor.DesignUI);
                //加载控件至页面
                editor.Load(_EmrHost);
                //*****************************
            }
            catch (Exception ex)
            {
                SDT.Client.ControlsHelper.Show(ex.Message);
            }

            return true;
        }

        #region IEmrHost 成员


        /// <summary>
        /// 要自己写东西...........................构建用户信息
        /// </summary>
        DrectSoft.Core.IUser IEmrHost.User
        {
            get
            {

                return _EmrHost.User;
            }
        }

        /// <summary>
        /// 不用管此方法
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="startupClassName"></param>
        /// <returns></returns>
        bool IEmrHost.LoadPlugIn(string assemblyName, string startupClassName)
        {
            return false;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 不用管此方法
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        bool IEmrHost.LoadPlugIn(string typeName)
        {
            return false;
            //throw new NotImplementedException();
        }

        void IEmrHost.ChoosePatient(decimal firstPageNo)
        {
            //DataRow row = GetPatInfo(firstPageNo);
            //if (row != null)
            //    CurrentPatientInfo = new Inpatient(row);
            _EmrHost.ChoosePatient(firstPageNo);
        }

        //private DataRow GetPatInfo(decimal noOfInpat)
        //{
        //    //DataTable table = SqlHelper.ExecuteDataTable(string.Format(str_queryPatByID, noOfInpat));

        //    //if (table.Rows.Count < 1) return null;
        //    //return table.Rows[0];


        //}

        public Inpatient CurrentPatientInfo
        {
            get { return _currentPat; }
            set
            {
                _currentPat = value;
            }

        }

        /// <summary>
        /// 医院信息
        /// </summary>
        DrectSoft.Core.HospitalInfo IEmrHost.CurrentHospitalInfo
        {
            get
            {
                return _EmrHost.CurrentHospitalInfo;
                //HospitalInfo info = _EmrHost.CurrentHospitalInfo;
                //return info;
            }
        }

        DrectSoft.Core.ICustomMessageBox IEmrHost.CustomMessageBox
        {
            get
            {
                return _EmrHost.CustomMessageBox;
            }
        }

        DrectSoft.Core.IDataAccess IEmrHost.SqlHelper
        {
            get
            {

                return _EmrHost.SqlHelper;
            }
        }

        string IEmrHost.MacAddress
        {
            get { return _EmrHost.MacAddress; }
        }

        DrectSoft.Core.IAppConfigReader IEmrHost.AppConfig
        {
            get { return _EmrHost.AppConfig; }
        }

        DataSet IEmrHost.PatientInfos
        {
            get { return _EmrHost.PatientInfos; }
        }

        string IEmrHost.RefreshPatientInfos()
        {
            return _EmrHost.RefreshPatientInfos();
        }

        System.Collections.ObjectModel.Collection<DrectSoft.FrameWork.Plugin.Manager.PlugInConfiguration> IEmrHost.PrivilegeMenu
        {
            get { return _EmrHost.PrivilegeMenu; }
        }

        DrectSoft.FrameWork.Plugin.PluginManager IEmrHost.Manager
        {
            get { return _EmrHost.Manager; }
        }

        bool IEmrHost.EmrAllowEdit
        {
            get
            {
                return _EmrHost.EmrAllowEdit;
            }
            set
            {
                _EmrHost.EmrAllowEdit = value;
            }
        }

        PluginUtil IEmrHost.PublicMethod
        {
            get { return _EmrHost.PublicMethod; }
        }

        private EmrDefaultSetting m_EmrDefaultSetting;
        DrectSoft.Common.Eop.EmrDefaultSetting IEmrHost.EmrDefaultSettings
        {
            get
            {
                return _EmrHost.EmrDefaultSettings;
                //if (m_EmrDefaultSetting == null)
                //{
                //    m_EmrDefaultSetting =DrectSoft. Util.InitEmrDefaultSet();
                //}

                //return m_EmrDefaultSetting;
            }
        }

        private DrectSoftLog _logger;
        DrectSoft.Core.DrectSoftLog IEmrHost.Logger
        {
            get
            {
                return _EmrHost.Logger;
            }
        }

        private string m_CurrentSelectedEmrID = string.Empty;
        string IEmrHost.CurrentSelectedEmrID
        {
            get
            {
                return _EmrHost.CurrentSelectedEmrID;
            }
            set
            {
                _EmrHost.CurrentSelectedEmrID = value;
            }
        }

        void IEmrHost.ShowMessageWindow(DataTable dt, bool isClear)
        {
            // throw new NotImplementedException();
        }

        #endregion


        public void ChoosePatient(decimal firstPageNo, string FloderState)
        {
            throw new NotImplementedException();
        }

        public void ChoosePatient(string firstPageNo, out Inpatient MyInp)
        {
            throw new NotImplementedException();
        }

        public string FloderState
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public Inpatient NEWCurrentPatientInfo
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
