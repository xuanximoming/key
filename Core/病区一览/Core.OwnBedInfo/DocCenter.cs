using Consultation.NEW;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Core.Consultation;
using DrectSoft.Core.QCDeptReport;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using MedicalRecordManage.UCControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.OwnBedInfo
{
    /// <summary>
    /// 医生工作站界面
    /// 
    /// </summary>
    public partial class DocCenter : DevExpress.XtraEditors.XtraForm
    {
        public delegate void del_ShowVigals(string noofinpat);
        public event del_ShowVigals ShowVigalsHandle;
        public void OnShowVigals(string noofinpat)
        {
            if (ShowVigalsHandle != null)
            {
                ShowVigalsHandle(noofinpat);
            }
        }
        NursingRecordForm nursingRecordForm;

        private IEmrHost m_App;
        /// <summary>
        /// 病患列表
        /// </summary>
        private UserControlAllListBedInfo m_UserControlAllListIno;

        private UCFail m_UCFail;
        private UCTran m_UCTran;
        private MyMedicalRecordList mymedicalrecordlist;
        private DrectSoft.Emr.QcManager.QualityMedicalRecord qualityDept;
        private ReplenishPatRec repl;

        private DataTable myPats;

        private DataManager m_DataManager;

        /// <summary>
        /// 是否允许设置我的患者
        /// </summary>
        public bool NeedUndoMyInpatient { get; set; }
        /// <summary>
        /// 科室代码
        /// </summary>
        private string m_DeptId;
        /// <summary>
        /// 病区代码
        /// </summary>
        private string m_WardId;

        private ImageList m_ImageListzifei;

        private Employee m_Employee;

        private WaitDialogForm m_WaitDialog;

        private UCConsultationForDocCenter m_UCConsultationForDocCenter;

        #region constructor
        public DocCenter()
            : this(null)
        {
            if (!this.DesignMode)
            {
                DS_SqlHelper.CreateSqlHelper();
            }
        }

        public DocCenter(IEmrHost app)
        {
            m_WaitDialog = new WaitDialogForm("正在创建用户界面...", "请稍候");

            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;//解决第三方控件异步报错的问题
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
            DS_SqlHelper.CreateSqlHelper();
            monthCalendar1.DateTime = DateTime.Now;
            monthCalendar1.TodayButton.Text = "今天";
            m_App = app;
            m_DeptId = m_App.User.CurrentDeptId;
            m_WardId = m_App.User.CurrentWardId;
            m_DataManager = new DataManager(m_App, m_DeptId, m_WardId);
            AddUCConsultationForDocCenter();
            monthCalendar1.EditDateModified += new EventHandler(monthCalendar1_EditDateModified);
            gridViewGridWardPat.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(gridViewGridWardPat_CustomDrawRowIndicator);
            BindTaskInfo();//窗体加载就加载出要看到的会诊信息 add 2012年6月14日 09:52:02

            //edit by Yanqiao.Cai 2012-11-08
            //CopntrolModPatient();


            //Application.AddMessageFilter(new DrectSoft.FrameWork.Filter.KeyMessageFilter());
            textEditPATID.Focus();


        }
        /// <summary>
        /// 显示序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridViewGridWardPat_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            DS_Common.AutoIndex(e);
        }
        #endregion

        private void FormUserControlShow_Load(object sender, EventArgs e)
        {
            try
            {

                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在读取病人数据...");
                ucTimeQcInfo1.App = m_App;//使用了新的病历提醒功能xlb2013-01-11
                BtnReset();
                this.cmbQueryType.SelectedIndex = 0;
                InitConfig();
                SetButtonState();
                InitializeImage();
                InitMyInpatient();
                InitUndoMyInpatient();
                SetPageCheckVisible();
                DS_Common.HideWaitDialog(m_WaitDialog);
                groupQCTiXing.Visible = false;
                MedicalRecordManage.UI.MedicalRecordManageForm fm = new MedicalRecordManage.UI.MedicalRecordManageForm();
                fm.Run(m_App);
                if (!isdeptManager(m_App.User.CurrentDeptId, m_App.User.Id))
                {
                    DeptQc.PageVisible = false;
                }
                //if (m_App.CurrentPatientInfo != null)
                //{
                //    m_App.ChoosePatient(decimal.Parse(m_App.CurrentPatientInfo.NoOfFirstPage.ToString()));
                //    m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private string m_ConfigKeyBase = string.Empty;
        private string m_ConfigKeyHis = string.Empty;
        private string m_ConfigKeySimple = string.Empty;
        private string m_ConfigClinical = string.Empty;

        private void InitConfig()
        {
            try
            {
                ///基本信息维护功能（是否手动维护：1-是；0-否）
                m_ConfigKeyBase = GetConfigValueByKey("ManualMaintainBasicInfo");
                ///从his查病人信息功能（1-是；0-否）
                m_ConfigKeyHis = GetConfigValueByKey("GetInpatientForHis");
                ///简版工作站
                m_ConfigKeySimple = GetConfigValueByKey("SimpleDoctorCentor");
                m_ConfigClinical = GetConfigValueByKey("IsShowClinicalButton");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 设置入院日期为入院时间还是入科日期（依据配置）
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-14</date>
        public void SetInHosOrInWardDate()
        {
            try
            {
                //获取入院日期（配置） edit by cyq 2013-03-13
                string config = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                if (!string.IsNullOrEmpty(config))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(config);
                    XmlNodeList nodeList = doc.GetElementsByTagName("InHosTimeType");
                    if (null != nodeList && nodeList.Count > 0)
                    {
                        string cfgValue = null == nodeList[0].InnerText ? "" : nodeList[0].InnerText.Trim();
                        if (cfgValue == "1")
                        {//入科
                            gridViewGridWardPat.Columns[9].FieldName = "INWARDDATE";//我的病人
                            //gridViewHistoryInfo.Columns[6].FieldName = "INWARDDATE";//科室历史病人  xll 科室历史病人去出院时间 20130820
                        }
                        else
                        {//入院
                            gridViewGridWardPat.Columns[9].FieldName = "ADMITDATE";//我的病人
                            //gridViewHistoryInfo.Columns[6].FieldName = "ADMITDATE";//科室历史病人 xll 科室历史病人去出院时间 20130820
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Add by wwj 2013-03-07 增加右侧会诊的列表
        /// </summary>
        private void AddUCConsultationForDocCenter()
        {
            try
            {
                m_UCConsultationForDocCenter = new UCConsultationForDocCenter(m_App, navBarGroupConsultation);
                navBarGroupControlContainer4.Controls.Add(m_UCConsultationForDocCenter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 得到配置信息  wyt 2012年8月27日
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigValueByKey(string key)
        {

            string sql1 = " select * from appcfg where configkey = '" + key + "'  ";
            DataTable dt = m_App.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
            string config = string.Empty;
            if (dt != null && dt.Rows.Count > 0)//add by xlb 2012-12-25
            {
                config = dt.Rows[0]["value"].ToString();
            }
            return config;
        }

        /// <summary>
        /// 质控提醒的设置
        /// </summary>
        private void GetQCTiXong()
        {
            //DataTable dtQC = m_DataManager.GetQCTiXong();
            //gridControlQCTiXing.DataSource = dtQC;正式上线仁和的再使用 add by ywk 2012年8月14日 13:28:54
        }

        /// <summary>
        /// 判断“撤销我的病人”功能的显示与否
        /// edit by xlb 2012-12-25
        /// 调用方法得到配置信息
        /// </summary>
        private void InitUndoMyInpatient()
        {
            //string config = m_App.SqlHelper.ExecuteScalar("select value from appcfg where configkey = 'IsOpenSetMyInpatient'", CommandType.Text).ToString();
            //string IsOpenSetMyInpatient = "IsOpenSetMyInpatient";
            string config = GetConfigValueByKey("IsOpenSetMyInpatient");
            NeedUndoMyInpatient = !config.Equals("0");
            barButtonItemUndoMyInpatient.Visibility = NeedUndoMyInpatient.Equals(true) ? BarItemVisibility.Always : BarItemVisibility.Never;
        }

        #region methods

        /// <summary>
        /// 初始化 病患列表 usercontrol
        /// </summary>
        private void InitAllListView()
        {
            if (m_UserControlAllListIno == null)
            {
                m_UserControlAllListIno = new UserControlAllListBedInfo(m_App);
                m_UserControlAllListIno.Dock = DockStyle.Fill;
                m_UserControlAllListIno.NeedUndoMyInpatient = NeedUndoMyInpatient;
                this.AllInpTabPage.Controls.Add(m_UserControlAllListIno);
            }
            else
            {
                m_UserControlAllListIno.RefreshInpatientList();
            }
        }

        /// <summary>
        /// 点击刷新时,控制在显示的usercontrol控件刷新
        /// </summary>
        public void ControlRefresh()
        {
            if (this.xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的病人
            {
                InitMyInpatient();
            }
            else if (this.xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
            {
                if (AllInpTabPage.Controls.Count > 1)
                    AllInpTabPage.Controls.Clear();
                InitAllListView();
            }
            else if (this.xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//历史病人查询
            {
                Reset();
                btnQuery_Click(null, null);
            }
            else if (this.xtraTabControl1.SelectedTabPage == FailTabPage)//出院未归档
            {
                if (FailTabPage.Controls.Count > 0)
                {
                    Reset();
                    m_UCFail.refreshQuery();
                    return;
                }
                m_UCFail = new QCDeptReport.UCFail(m_App);
                m_UCFail.Dock = DockStyle.Fill;
                FailTabPage.Controls.Add(m_UCFail);
            }
            else if (this.xtraTabControl1.SelectedTabPage == FailTabPage)// 转科病人查看
            {

                if (paintTran.Controls.Count > 0)
                {
                    Reset();
                    m_UCTran.refreshQuery();
                    return;
                }
                m_UCTran = new QCDeptReport.UCTran(m_App);
                m_UCTran.Dock = DockStyle.Fill;
                paintTran.Controls.Add(m_UCTran);
            }
            else if (this.xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
            {
                BindEmrPoint();
            }
            else if (this.xtraTabControl1.SelectedTabPage == Replenish)//补写病历
            {
                //Replenish.Controls.Clear();
                if (Replenish.Controls.Count == 0)
                {
                    repl = new ReplenishPatRec();
                    repl.Dock = DockStyle.Fill;
                    Replenish.Controls.Add(repl);
                }
                repl.LoadData(m_App);
            }
            else if (this.xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
            {
                DataTable dt = m_DataManager.GetThreeLevelCheckList(m_App.User.CurrentDeptId);
                this.gridControlThreeLevelCheck.DataSource = dt;
                GetThreeLevelCheckEmrDoc(dt);
            }
            else if (this.xtraTabControl1.SelectedTabPage == myinplist)//病历
            {
                mymedicalrecordlist = new MyMedicalRecordList(m_App);
                mymedicalrecordlist.Dock = DockStyle.Fill;
                myinplist.Controls.Add(mymedicalrecordlist);
            }
            else if (this.xtraTabControl1.SelectedTabPage == DeptQc)//科室质控
            {
                qualityDept = new DrectSoft.Emr.QcManager.QualityMedicalRecord(m_App);
                qualityDept.IsDeptQc = true;
                qualityDept.Dock = DockStyle.Fill;
                DeptQc.Controls.Add(qualityDept);
            }
            BindTaskInfo();
        }

        bool m_IsLoadMyInpTabPage = false;
        bool m_IsLoadAllInpTabPage = false;
        bool m_IsLoadHistoryInpTabPage = false;
        bool m_IsLoadFailTabPage = false;
        bool m_IsLoadXtraTabPagePoint = false;
        bool m_IsLoadReplenish = false;
        bool m_IsLoadXtraTabPageCheck = false;
        bool m_myinplist = false;
        bool m_DeptQc = false;
        bool m_IsLoadTranPage = false;

        /// <summary>
        /// 点击刷新时,控制在显示的usercontrol控件刷新
        /// </summary>
        public void ControlRefreshForSelectedPageChanged()
        {
            try
            {
                if (!m_IsLoadMyInpTabPage && this.xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的病人
                {
                    m_IsLoadMyInpTabPage = true;
                    InitMyInpatient();
                }
                else if (!m_IsLoadAllInpTabPage && this.xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                {
                    m_IsLoadAllInpTabPage = true;
                    if (AllInpTabPage.Controls.Count > 1)
                        AllInpTabPage.Controls.Clear();
                    InitAllListView();
                }
                else if (!m_IsLoadHistoryInpTabPage && this.xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//历史病人查询
                {
                    m_IsLoadHistoryInpTabPage = true;
                    Reset();
                    btnQuery_Click(null, null);
                }

                else if (!m_IsLoadFailTabPage && this.xtraTabControl1.SelectedTabPage == FailTabPage)//出院未归档
                {
                    m_IsLoadFailTabPage = true;
                    if (FailTabPage.Controls.Count > 0)
                    {
                        Reset();
                        m_UCFail.refreshQuery();
                        return;
                    }
                    m_UCFail = new QCDeptReport.UCFail(m_App);
                    m_UCFail.Dock = DockStyle.Fill;
                    FailTabPage.Controls.Add(m_UCFail);
                }
                else if (!m_IsLoadTranPage && this.xtraTabControl1.SelectedTabPage == paintTran)//出院未归档
                {
                    m_IsLoadTranPage = true;
                    if (paintTran.Controls.Count > 0)
                    {
                        Reset();
                        m_UCTran.refreshQuery();
                        return;
                    }
                    m_UCTran = new QCDeptReport.UCTran(m_App);
                    m_UCTran.Dock = DockStyle.Fill;
                    paintTran.Controls.Add(m_UCTran);
                }
                else if (!m_IsLoadXtraTabPagePoint && this.xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
                {
                    m_IsLoadXtraTabPagePoint = true;
                    BindEmrPoint();
                }
                else if (!m_IsLoadReplenish && this.xtraTabControl1.SelectedTabPage == Replenish)//补写病历
                {
                    m_IsLoadReplenish = true;
                    if (Replenish.Controls.Count == 0)
                    {
                        repl = new ReplenishPatRec();
                        repl.Dock = DockStyle.Fill;
                        Replenish.Controls.Add(repl);
                    }
                    repl.LoadData(m_App);
                }
                else if (!m_IsLoadXtraTabPageCheck && this.xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
                {
                    m_IsLoadXtraTabPageCheck = true;
                    DataTable dt = m_DataManager.GetThreeLevelCheckList(m_App.User.CurrentDeptId);
                    this.gridControlThreeLevelCheck.DataSource = dt;
                    GetThreeLevelCheckEmrDoc(dt);
                }
                else if (!m_myinplist && this.xtraTabControl1.SelectedTabPage == myinplist)//病历查询
                {
                    m_myinplist = true;
                    if (myinplist.Controls.Count > 0)
                    {
                        Reset();
                        mymedicalrecordlist.refreshQuery();
                        return;
                    }
                    mymedicalrecordlist = new MyMedicalRecordList(m_App);
                    mymedicalrecordlist.Dock = DockStyle.Fill;
                    myinplist.Controls.Add(mymedicalrecordlist);
                }
                else if (!m_DeptQc && this.xtraTabControl1.SelectedTabPage == DeptQc)//科室质控
                {
                    m_DeptQc = true;
                    if (DeptQc.Controls.Count > 0)
                    {
                        Reset();
                        qualityDept.refreshQuery();
                        return;
                    }
                    qualityDept = new DrectSoft.Emr.QcManager.QualityMedicalRecord(m_App);
                    qualityDept.IsDeptQc = true;
                    qualityDept.Dock = DockStyle.Fill;
                    DeptQc.Controls.Add(qualityDept);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 初始化图片
        /// </summary>
        private void InitializeImage()
        {
            try
            {
                ImageHelper.GetImageListIllness();
                imageListCustomwzjb = ImageHelper.GetImageListIllness();
                repItemImageComboBoxwzjb.SmallImages = imageListCustomwzjb;
                DataTable dt = null;
                if ((dt == null) || (dt.Rows.Count <= 0))
                {
                    ImageComboBoxItem item1 = new ImageComboBoxItem("一般病人", "0", 0);
                    ImageComboBoxItem item2 = new ImageComboBoxItem("危重病人", "1", 1);
                    ImageComboBoxItem item3 = new ImageComboBoxItem("病重病人", "2", 2);
                    repItemImageComboBoxwzjb.Items.AddRange(new ImageComboBoxItem[] { item1, item2, item3 });
                }
                else
                {
                    ImageComboBoxItem[] imageCombo = new ImageComboBoxItem[dt.Rows.Count];
                    for (int index = 0; index < imageCombo.Length; index++)
                    {
                        ImageComboBoxItem item = new ImageComboBoxItem(dt.Rows[index]["name"].ToString().Trim(), dt.Rows[index]["mxdm"].ToString().Trim(), Convert.ToInt16(dt.Rows[index]["mxdm"].ToString().Trim()));
                        imageCombo[index] = item;
                    }
                    repItemImageComboBoxwzjb.Items.AddRange(imageCombo);
                }
                imageListcwdm = ImageHelper.GetImageListBedNum();
                m_ImageListzifei = ImageHelper.GetImageListPay();
                imageListBrxb = ImageHelper.GetImageListBrxb();
                repItemImageComboBoxBrxb.SmallImages = imageListBrxb;
                ImageComboBoxItem ImageComboItemMale = new ImageComboBoxItem("男", "1", 1);
                ImageComboBoxItem ImageComboItemFemale = new ImageComboBoxItem("女", "2", 0);
                ImageComboBoxItem ImageComboItemUnknow = new ImageComboBoxItem("未知", "3", 1);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemMale);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemFemale);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemUnknow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化图片
        /// </summary>
        private void InitializeImage2()
        {
            try
            {
                ImageHelper.GetImageListIllness();
                imageListCustomwzjb = ImageHelper.GetImageListIllness();
                repositoryItemImageWZJB.SmallImages = imageListCustomwzjb;
                DataTable dt = null;
                if ((dt == null) || (dt.Rows.Count <= 0))
                {
                    ImageComboBoxItem item1 = new ImageComboBoxItem("一般病人", "0", 0);
                    ImageComboBoxItem item2 = new ImageComboBoxItem("危重病人", "1", 1);
                    ImageComboBoxItem item3 = new ImageComboBoxItem("病重病人", "2", 2);
                    repositoryItemImageWZJB.Items.AddRange(new ImageComboBoxItem[] { item1, item2, item3 });
                }
                else
                {
                    ImageComboBoxItem[] imageCombo = new ImageComboBoxItem[dt.Rows.Count];
                    for (int index = 0; index < imageCombo.Length; index++)
                    {
                        ImageComboBoxItem item = new ImageComboBoxItem(dt.Rows[index]["name"].ToString().Trim(), dt.Rows[index]["mxdm"].ToString().Trim(), Convert.ToInt16(dt.Rows[index]["mxdm"].ToString().Trim()));
                        imageCombo[index] = item;
                    }
                    repositoryItemImageWZJB.Items.AddRange(imageCombo);
                }
                imageListBrxb = ImageHelper.GetImageListBrxb();
                repositoryItemImageXB.SmallImages = imageListBrxb;
                ImageComboBoxItem ImageComboItemMale = new ImageComboBoxItem("男", "1", 1);
                ImageComboBoxItem ImageComboItemFemale = new ImageComboBoxItem("女", "2", 0);
                ImageComboBoxItem ImageComboItemUnknow = new ImageComboBoxItem("未知", "3", 1);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemMale);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemFemale);
                repItemImageComboBoxBrxb.Items.Add(ImageComboItemUnknow);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 第二行MENU

        /// <summary>
        /// 医嘱一览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemOrder_ItemClick(object sender, ItemClickEventArgs e)
        {
            m_App.LoadPlugIn("DrectSoft.Core.DoctorTasks.dll", "DrectSoft.Core.DoctorTasks.FormInpatientOrder");
        }

        /// <summary>
        /// 病历一览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemWriteView_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadRecordInput();
        }

        /// <summary>
        /// 病历书写
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemWrite_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadRecordInput();
        }

        /// <summary>
        /// 进入文书录入
        /// </summary>
        private void LoadRecordInput()
        {
            m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
        }
        #endregion


        #region 待办事项

        /// <summary>
        /// 绑定右侧工具栏中的数据
        /// edit by xlb 2013-01-11
        /// 使用了QCTimeLimit新模块
        /// </summary>
        private void BindTaskInfo()
        {
            try//因此数据不停刷新，要TRY   add by ywk 2012年9月27日 11:19:55 
            {
                //绑定会诊信息
                m_UCConsultationForDocCenter.BindConsultationThread();

                //绑定时限信息
                ucTimeQcInfo1.App = m_App;
                ucTimeQcInfo1.CheckDoctorLimitThread(m_App.User.DoctorId);
            }
            catch (Exception ex)
            {
                //throw ex;
                //MyMessageBox.Show(1, ex);
            }

            //GetQCTiXong();
        }

        private void SetPageCheckVisible()
        {
            //设置是否显示审核界面
            if ((m_Employee == null) || (!m_Employee.Code.Equals(m_App.User.Id)))
            {
                m_Employee = new Employee(m_App.User.Id);
                m_Employee.ReInitializeProperties();
            }
            if (m_Employee.DoctorGradeNumber < 1)
            {
                xtraTabControl1.TabPages.Remove(xtraTabPageCheck);
            }
            else
            {
                if (!xtraTabControl1.TabPages.Contains(xtraTabPageCheck))
                {
                    xtraTabControl1.TabPages.Add(xtraTabPageCheck);
                }
            }
        }

        private void GetThreeLevelCheckEmrDoc(DataTable dataTable)
        {
            Employee emp = new Employee(m_App.User.Id);
            emp.ReInitializeProperties();

            if (emp.Grade.Trim() != "")
            {
                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), emp.Grade);
                if (grade == DoctorGrade.Attending) //主治医师
                {
                    string filter = GetEmrCanAudit(grade);
                    dataTable.DefaultView.RowFilter = " hassubmit = '4601' " + filter; //提交但未审核
                }
                else if (grade == DoctorGrade.Chief || grade == DoctorGrade.AssociateChief)//主任和副主任
                {
                    string filter = GetEmrCanAudit(grade);
                    dataTable.DefaultView.RowFilter = " hassubmit in ('4601', '4602') " + filter; //提交但未审核,主治审核
                }
            }
            else
            {
                xtraTabPageCheck.PageVisible = false;
            }
        }

        const string c_GetThreeLevelCheck = @"select count(1) from THREE_LEVEL_CHECK ";
        const string c_Resident = " where resident_id = '{0}' ";
        const string c_Attend = " where resident_id = '{0}' and attend_id = '{1}' ";
        const string c_Chief = " where resident_id = '{0}' and chief_id = '{1}' ";

        /// <summary>
        /// 判断是否有审核的权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true：有审核权限 false：没有审核权限</returns>
        private bool GetThreeLevelCheck(string hassubmit, string owner, DoctorGrade grade)
        {
            bool result = true;
            string num = m_App.SqlHelper.ExecuteScalar(c_GetThreeLevelCheck + string.Format(c_Resident, owner), System.Data.CommandType.Text).ToString();
            if (num != "0") //设置了指定人员的三级检诊
            {
                switch (grade)
                {
                    case DoctorGrade.Attending:
                        num = m_App.SqlHelper.ExecuteScalar(c_GetThreeLevelCheck + string.Format(c_Attend, owner, m_App.User.Id), System.Data.CommandType.Text).ToString();
                        if (num == "0")
                        {
                            result = false;
                        }
                        break;
                    case DoctorGrade.AssociateChief:
                    case DoctorGrade.Chief:
                        num = m_App.SqlHelper.ExecuteScalar(c_GetThreeLevelCheck + string.Format(c_Chief, owner, m_App.User.Id), System.Data.CommandType.Text).ToString();
                        if (num == "0")
                        {
                            result = false;
                        }
                        break;
                }
            }
            return result;
        }

        private string GetEmrCanAudit(DoctorGrade grade)
        {
            DataTable dt = gridControlThreeLevelCheck.DataSource as DataTable;
            string id = string.Empty;
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string hassubmit = dr["hassubmit"].ToString();
                    string owner = dr["owner"].ToString();
                    if (!GetThreeLevelCheck(hassubmit, owner, grade))
                    {
                        id += dr["id"].ToString() + ",";
                    }
                }
            }
            if (id != string.Empty)
            {
                id = " and id not in (" + id.Trim(',') + ")";
            }
            return id;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        private DataSet GetTaskToday()
        {
            try
            {
                DataSet dataSet = new DataSet();
                SqlParameter[] sqlParams = new SqlParameter[] { 
                new SqlParameter("@Wardid", SqlDbType.VarChar, 12),
                new SqlParameter("@Deptids", SqlDbType.VarChar, 255),
                new SqlParameter("@UserID", SqlDbType.VarChar,4),
                new SqlParameter("@Time", SqlDbType.VarChar,10)
            };
                sqlParams[0].Value = m_App.User.CurrentWardId.Trim(); // 病区代码
                sqlParams[1].Value = m_App.User.CurrentDeptId.Trim(); // 科室代码
                sqlParams[2].Value = m_App.User.Id.Trim(); // USERID
                sqlParams[3].Value = this.monthCalendar1.SelectionEnd.ToString("yyyy-MM-dd").Trim(); // 选择日期
                dataSet = m_App.SqlHelper.ExecuteDataSet("usp_GetDoctorTaskInfo", sqlParams, CommandType.StoredProcedure);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private enum TaskType
        {
            /// <summary>
            /// 会诊
            /// </summary>
            Consultation = 0
        }

        #endregion

        /// <summary>
        /// 切换时间调用绑定医生任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void monthCalendar1_EditDateModified(object sender, EventArgs e)
        {
            BindTaskInfo();
        }

        /// <summary>
        /// tab页切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                //add by Yanqiao.Cai 2012-11-08
                SetButtonState();

                ControlRefreshForSelectedPageChanged();//Add by wwj 2013-06-04 保证在切换Page时不会重复捞取数据
                Reset();
                FocusControl();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FocusControl()
        {
            if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
            {
                this.textEditPATBEDNO.Focus();
            }
            else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
            {
                m_UserControlAllListIno.FocusFirstControl();
            }
            else if (xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//科室历史病人
            {
                textEditPatientSN.Focus();
            }
            else if (xtraTabControl1.SelectedTabPage == FailTabPage)//已出院未归档
            {
                m_UCFail.FocusFirstControl();
                FailTabPage.Controls[0].Focus();
            }
            else if (xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
            { }
            else if (xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
            { }
            else if (xtraTabControl1.SelectedTabPage == Replenish)//补写病历
            {
                //repl.FocusFirstControl();
                Replenish.Controls[0].Focus();
            }
        }

        /// <summary>
        /// 设置工具栏图片按钮和右键按钮的显示或隐藏
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-08</date>
        /// </summary>
        private void SetButtonState()
        {
            try
            {
                string configKeyBase = m_ConfigKeyBase;
                string configKeyHis = m_ConfigKeyHis;
                string configKeySimple = m_ConfigKeySimple;
                string configClinical = m_ConfigClinical;

                #region 基本信息维护 &&从His查询病人信息
                if (configKeyBase == "1")
                {///手动维护
                    //病人信息 - 工具栏
                    this.barLargeButtonItemPatientInfo.Visibility = BarItemVisibility.Always;
                    //病人信息 - 右键
                    this.barButtonItem_PersonalInfo.Visibility = BarItemVisibility.Always;
                    //入院登记 - 工具栏 edit by Ukey 2016-11-14 9:54 显示入院登记
                    this.barLargeButtonItemInHosLogin.Visibility = BarItemVisibility.Always;
                    //入院登记 - 右键
                    this.barButtonItem_inHosLogin.Visibility = BarItemVisibility.Always;
                    //编辑病人信息 - 工具栏
                    this.barLargeButtonItemPatiEdit.Visibility = BarItemVisibility.Always;
                    //编辑病人信息 - 右键
                    this.barButtonItemChange.Visibility = BarItemVisibility.Always;
                    //病人出院 - 工具栏 edit by Ukey 2016-11-14 9:54 显示病人出院
                    this.barLargeButtonItem2.Visibility = BarItemVisibility.Always;
                    //病人出院 - 右键
                    this.barButtonItemOut.Visibility = BarItemVisibility.Always;
                }
                else
                {///非手动维护
                    if (configKeyHis == "1" && configKeySimple != "1")
                    {///从His查询病人信息 && 不是简版工作站
                        //病人信息 - 工具栏
                        //this.barLargeButtonItemPatientInfo.Visibility = BarItemVisibility.Always;
                        //病人信息 - 右键
                        this.barButtonItem_PersonalInfo.Visibility = BarItemVisibility.Always;
                    }
                    else
                    {
                        //病人信息 - 工具栏
                        //this.barLargeButtonItemPatientInfo.Visibility = BarItemVisibility.Never;
                        //病人信息 - 右键
                        this.barButtonItem_PersonalInfo.Visibility = BarItemVisibility.Never;
                    }
                    //入院登记 - 工具栏
                    this.barLargeButtonItemInHosLogin.Visibility = BarItemVisibility.Always;
                    //入院登记 - 右键
                    this.barButtonItem_inHosLogin.Visibility = BarItemVisibility.Never;
                    //编辑病人信息 - 工具栏
                    this.barLargeButtonItemPatiEdit.Visibility = BarItemVisibility.Never;
                    //编辑病人信息 - 右键
                    this.barButtonItemChange.Visibility = BarItemVisibility.Never;
                    //病人出院 - 工具栏
                    this.barLargeButtonItem2.Visibility = BarItemVisibility.Never;
                    //病人出院 - 右键
                    this.barButtonItemOut.Visibility = BarItemVisibility.Never;
                }
                #endregion

                #region 简版工作站
                if (configKeySimple == "1")
                {///简版工作站
                    //会诊申请 - 工具栏
                    this.barLargeButtonItemConsultationApply.Visibility = BarItemVisibility.Never;
                    //会诊申请 - 右键
                    this.btn_AppConsult.Visibility = BarItemVisibility.Never;
                    //院内会诊 - 工具栏
                    this.barLargeButtonItemConsultation.Visibility = BarItemVisibility.Never;
                    //院内会诊 - 右键
                    this.barButtonItem_consultation.Visibility = BarItemVisibility.Never;
                    //病案借阅 - 工具栏
                    this.barLargeButtonItemEmrApply.Visibility = BarItemVisibility.Never;
                    //病案借阅 - 右键
                    this.barButtonItem_emrApply.Visibility = BarItemVisibility.Never;
                    //传染病上报 - 工具栏
                    this.barLargeButtonItemReportCard.Visibility = BarItemVisibility.Never;
                    //传染病上报 - 右键
                    this.barButtonItemReportCard.Visibility = BarItemVisibility.Never;
                    //临床路径 - 工具栏
                    this.barLargeButtonItemClinical.Visibility = BarItemVisibility.Never;
                    //临床路径 - 右键
                    this.barButtonItem7.Visibility = BarItemVisibility.Never;
                    //待办事项 - 右侧模块
                    this.dockPanel1.Visibility = DockVisibility.Hidden;
                    //病历审核 - tab页
                    this.xtraTabPageCheck.PageVisible = false;
                    //病历评分- tab页
                    this.xtraTabPagePoint.PageVisible = false;
                }
                else
                {
                    //会诊申请 - 工具栏
                    this.barLargeButtonItemConsultationApply.Visibility = BarItemVisibility.Always;
                    //会诊申请 - 右键
                    this.btn_AppConsult.Visibility = BarItemVisibility.Always;
                    //院内会诊 - 工具栏
                    this.barLargeButtonItemConsultation.Visibility = BarItemVisibility.Always;
                    //院内会诊 - 右键
                    this.barButtonItem_consultation.Visibility = BarItemVisibility.Always;
                    //病案借阅 - 工具栏
                    this.barLargeButtonItemEmrApply.Visibility = BarItemVisibility.Always;
                    //病案借阅 - 右键
                    this.barButtonItem_emrApply.Visibility = BarItemVisibility.Always;
                    //传染病上报 - 工具栏
                    this.barLargeButtonItemReportCard.Visibility = BarItemVisibility.Always;
                    //传染病上报 - 右键
                    this.barButtonItemReportCard.Visibility = BarItemVisibility.Always;
                    //临床路径 - 工具栏
                    this.barLargeButtonItemClinical.Visibility = BarItemVisibility.Always;
                    //临床路径 - 右键
                    this.barButtonItem7.Visibility = BarItemVisibility.Always;
                    //待办事项 - 右侧模块
                    this.dockPanel1.Visibility = DockVisibility.Visible;
                    //病历审核 - tab页
                    this.xtraTabPageCheck.PageVisible = true;
                    //病历评分- tab页
                    this.xtraTabPagePoint.PageVisible = true;
                }
                #endregion

                #region tab页切换的不同显示
                //特别注明：右键按钮为 我的患者Tab页专用，其它Tab页无需显示
                if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
                {
                    //撤销我的病人 - 工具栏
                    this.barLargeButtonItemCancelMyPati.Visibility = BarItemVisibility.Always;
                    //撤销我的病人 - 右键
                    this.barButtonItemUndoMyInpatient.Visibility = BarItemVisibility.Always;
                    //设为我的病人 - 工具栏
                    this.barLargeButtonItemSetMyPati.Visibility = BarItemVisibility.Never;
                    //设为我的病人 - 右键 ---> 在全部患者页面添加
                    //设定婴儿 - 工具栏
                    this.barLargeButtonItemSetBaby.Visibility = BarItemVisibility.Always;
                    //设定婴儿 - 右键
                    this.barButtonItemBaby.Visibility = BarItemVisibility.Always;
                    //文书录入- 工具栏
                    this.barLargeButtonItemDocumentWrite.Visibility = BarItemVisibility.Always;
                    //文书录入- 右键
                    this.barButtonItem_Record.Visibility = BarItemVisibility.Always;
                }
                else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                {
                    //撤销我的病人 - 工具栏
                    this.barLargeButtonItemCancelMyPati.Visibility = BarItemVisibility.Never;
                    //撤销我的病人 - 右键
                    this.barButtonItemUndoMyInpatient.Visibility = BarItemVisibility.Never;
                    //根据配置项来设置是否显示为工具栏中设为我的病人按钮 Add by xlb 2013-06-09
                    if (DS_SqlService.GetConfigValueByKey("IsOpenSetMyInpatient").Equals("1"))
                    {
                        //设为我的病人 - 工具栏
                        this.barLargeButtonItemSetMyPati.Visibility = BarItemVisibility.Always;
                    }
                    else
                    {
                        this.barLargeButtonItemSetMyPati.Visibility = BarItemVisibility.Never;
                    }
                    //设为我的病人 - 右键 ---> 在全部患者页面添加
                    //设定婴儿 - 工具栏
                    this.barLargeButtonItemSetBaby.Visibility = BarItemVisibility.Always;
                    //设定婴儿 - 右键
                    this.barButtonItemBaby.Visibility = BarItemVisibility.Always;
                    //文书录入- 工具栏
                    this.barLargeButtonItemDocumentWrite.Visibility = BarItemVisibility.Always;
                    //文书录入- 右键
                    this.barButtonItem_Record.Visibility = BarItemVisibility.Never;
                }
                else if (xtraTabControl1.SelectedTabPage == FailTabPage) //已出院未完成查询
                {
                    //编辑病人信息 - 工具栏
                    this.barLargeButtonItemPatiEdit.Visibility = BarItemVisibility.Always;
                    //编辑病人信息 - 右键
                    this.barButtonItemChange.Visibility = BarItemVisibility.Always;
                    //病人出院 - 工具栏
                    this.barLargeButtonItem2.Visibility = BarItemVisibility.Never;
                    //病人出院 - 右键
                    this.barButtonItemOut.Visibility = BarItemVisibility.Never;
                    //撤销我的病人 - 工具栏
                    this.barLargeButtonItemCancelMyPati.Visibility = BarItemVisibility.Never;
                    //撤销我的病人 - 右键
                    this.barButtonItemUndoMyInpatient.Visibility = BarItemVisibility.Never;
                    //设为我的病人 - 工具栏
                    this.barLargeButtonItemSetMyPati.Visibility = BarItemVisibility.Never;
                    //设为我的病人 - 右键 ---> 在全部患者页面添加
                    //设定婴儿 - 工具栏
                    this.barLargeButtonItemSetBaby.Visibility = BarItemVisibility.Never;
                    //设定婴儿 - 右键
                    this.barButtonItemBaby.Visibility = BarItemVisibility.Never;
                    //文书录入- 工具栏
                    this.barLargeButtonItemDocumentWrite.Visibility = BarItemVisibility.Never;
                    //文书录入- 右键
                    this.barButtonItem_Record.Visibility = BarItemVisibility.Never;
                    //入院登记 - 工具栏
                    this.barLargeButtonItemInHosLogin.Visibility = BarItemVisibility.Always;
                    //入院登记 - 右键
                    this.barButtonItem_inHosLogin.Visibility = BarItemVisibility.Never;
                    //会诊申请 - 工具栏
                    this.barLargeButtonItemConsultationApply.Visibility = BarItemVisibility.Never;
                    //会诊申请 - 右键
                    this.btn_AppConsult.Visibility = BarItemVisibility.Never;
                    //临床路径 - 工具栏
                    this.barLargeButtonItemClinical.Visibility = BarItemVisibility.Never;
                    //临床路径 - 右键
                    this.barButtonItem7.Visibility = BarItemVisibility.Never;
                }
                else
                {
                    //编辑病人信息 - 工具栏
                    this.barLargeButtonItemPatiEdit.Visibility = BarItemVisibility.Never;
                    //编辑病人信息 - 右键
                    this.barButtonItemChange.Visibility = BarItemVisibility.Never;
                    //病人出院 - 工具栏
                    this.barLargeButtonItem2.Visibility = BarItemVisibility.Never;
                    //病人出院 - 右键
                    this.barButtonItemOut.Visibility = BarItemVisibility.Never;
                    //撤销我的病人 - 工具栏
                    this.barLargeButtonItemCancelMyPati.Visibility = BarItemVisibility.Never;
                    //撤销我的病人 - 右键
                    this.barButtonItemUndoMyInpatient.Visibility = BarItemVisibility.Never;
                    //设为我的病人 - 工具栏
                    this.barLargeButtonItemSetMyPati.Visibility = BarItemVisibility.Never;
                    //设为我的病人 - 右键 ---> 在全部患者页面添加
                    //设定婴儿 - 工具栏
                    this.barLargeButtonItemSetBaby.Visibility = BarItemVisibility.Never;
                    //设定婴儿 - 右键
                    this.barButtonItemBaby.Visibility = BarItemVisibility.Never;
                    //文书录入- 工具栏
                    this.barLargeButtonItemDocumentWrite.Visibility = BarItemVisibility.Never;
                    //文书录入- 右键
                    this.barButtonItem_Record.Visibility = BarItemVisibility.Never;
                    //入院登记 - 工具栏
                    this.barLargeButtonItemInHosLogin.Visibility = BarItemVisibility.Always;
                    //入院登记 - 右键
                    this.barButtonItem_inHosLogin.Visibility = BarItemVisibility.Never;
                    //会诊申请 - 工具栏
                    this.barLargeButtonItemConsultationApply.Visibility = BarItemVisibility.Never;
                    //会诊申请 - 右键
                    this.btn_AppConsult.Visibility = BarItemVisibility.Never;
                    //临床路径 - 工具栏
                    this.barLargeButtonItemClinical.Visibility = BarItemVisibility.Never;
                    //临床路径 - 右键
                    this.barButtonItem7.Visibility = BarItemVisibility.Never;
                }
                #endregion
                if (null != configClinical && configClinical.Trim() == "0")
                {///默认显示
                    this.barLargeButtonItemClinical.Visibility = BarItemVisibility.Never;
                    this.barButtonItem7.Visibility = BarItemVisibility.Never;
                }
                //刷新 - 工具栏
                this.barLargeButtonItemRefresh.Visibility = BarItemVisibility.Always;
                //退出 - 工具栏
                this.barLargeButtonItemExit.Visibility = BarItemVisibility.Always;

                bool hasHZXT = DrectSoft.Service.DS_BaseService.FlieHasKey("HZXT");  //会诊系统模块是否存在
                if (!hasHZXT)
                {
                    //2个会诊按钮不显示 同时右边会诊列表也不显示
                    barLargeButtonItemConsultationApply.Visibility = BarItemVisibility.Never;
                    barLargeButtonItemConsultation.Visibility = BarItemVisibility.Never;
                    navBarGroupConsultation.Visible = false;
                    btn_AppConsult.Visibility = BarItemVisibility.Never;
                    barButtonItem_consultation.Visibility = BarItemVisibility.Never;

                }
                bool hasBLLL = DrectSoft.Service.DS_BaseService.FlieHasKey("BLLL");  //病历浏览模块是否存在
                if (!hasBLLL)
                {
                    barLargeButtonItemEmrApply.Visibility = BarItemVisibility.Never;
                    barButtonItem_emrApply.Visibility = BarItemVisibility.Never;
                }

                bool hasCRBSB = DrectSoft.Service.DS_BaseService.FlieHasKey("CRBSB");  //传染病模块是否存在
                if (!hasCRBSB)
                {
                    barLargeButtonItemReportCard.Visibility = BarItemVisibility.Never;
                    barButtonItemReportCard.Visibility = BarItemVisibility.Never;
                }
                //控制DockPanel 是否显示add  by ywk 2013年6月13日 10:16:50
                if (DS_BaseService.IsShowThisMD("IsShowDockPanel", "Docter"))//上方是隐藏。这个配置是为自动隐藏add by ywk 
                {
                    dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;

                }
                else
                {
                    dockPanel1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;//自动隐藏

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 重置当前Tab页查询条件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-22</date>
        /// </summary>
        private void Reset()
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
                {
                    textEditPATID.Text = "";
                    textEditPATNAME.Text = "";
                    textEditPATBEDNO.Text = "";
                    textEditInwDia.Text = string.Empty;
                }
                else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                {
                    m_UserControlAllListIno.Reset();
                }
                else if (xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//科室历史病人
                {
                    BtnReset();
                }
                else if (xtraTabControl1.SelectedTabPage == FailTabPage)//已出院未归档
                {
                    m_UCFail.Reset();//.ResetText();
                }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
                { }
                else if (xtraTabControl1.SelectedTabPage == Replenish)//补写病历
                {
                }
                else if (xtraTabControl1.SelectedTabPage == myinplist)
                {
                    mymedicalrecordlist.Reset();
                }
                else if (xtraTabControl1.SelectedTabPage == DeptQc)
                {
                    qualityDept.Reset();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 绑定病历评分表
        /// </summary>
        private void BindEmrPoint()
        {
            try
            {
                DataTable dt = m_DataManager.GetEmrPoint(m_App.User.Id);
                //add by cyq 2013-01-31 病历评分有内容时Tab页标题为红色
                if (null != dt && dt.Rows.Count > 0)
                {
                    this.xtraTabPagePoint.Appearance.HeaderActive.ForeColor = Color.Red;
                }
                gridControlPoint.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 获取出院末分配医师病患列表
        /// edit by Yanqiao.Cai 2012-11-14
        /// 1、add try ... catch
        /// 2、包含婴儿的姓名显示
        /// </summary>
        private void GetHistoryInPat()
        {
            try
            {
                //设置入院日期为入院时间还是入科日期（依据配置）add by cyq 2013-03-14
                SetInHosOrInWardDate();
                //初始化图片
                //InitializeImage2();
                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListBrxb);
                DS_Common.InitializeImage_WZJB(repositoryItemImageWZJB, imageListCustomwzjb);

                string deptID = m_App.User.CurrentDeptId;
                string wardID = m_App.User.CurrentWardId;
                string dia = textEditHistory.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");

                //DataTable dt = this.GetHistoryPat(1, deptID, wardID);
                //再次筛掉在院病人add by ywk 2013年5月29日 14:00:38
                DataTable dt = ToDataTable(this.GetHistoryPat(1, deptID, wardID).Select(" BRZT not in ('1500','1501')"));
                if (dia.Length > 0)
                {
                    dt = ToDataTable(dt.Select(string.Format(@"ZDMC like '%{0}%'", dia)));
                }
                if (dt == null || dt.Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有符合条件的数据");
                    this.gridHistoryInp.DataSource = new DataTable();
                    return;
                }
                string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ResultName = DataManager.GetPatsBabyContent(m_App, dt.Rows[i]["noofinpat"].ToString());
                    dt.Rows[i]["PatName"] = ResultName;
                }
                this.gridHistoryInp.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 数组DataRow[]转化成表DataTable
        /// 王冀 2013 1 6
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();
                foreach (DataRow row in rows)
                    tmp.ImportRow(row);
                return tmp;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 查询在院全部患者
        /// </summary>
        /// <param name="queryType"></param>
        /// <param name="deptID"></param>
        /// <param name="wardID"></param>
        /// <returns></returns>
        private DataTable GetHistoryPat(int queryType, string deptID, string wardID)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@Wardid", SqlDbType.VarChar, 8),
                  new SqlParameter("@Deptids",SqlDbType.VarChar, 8),
                  new SqlParameter("@TimeFrom",SqlDbType.VarChar,10),
                  new SqlParameter("@TimeTo",SqlDbType.VarChar,10),
                  new SqlParameter("@PatientSN",SqlDbType.VarChar,32),
                  new SqlParameter("@Name",SqlDbType.VarChar,32),
                  new SqlParameter("@QueryType", SqlDbType.Int)
                  };
            sqlParams[0].Value = wardID;
            sqlParams[1].Value = deptID;
            sqlParams[2].Value = this.dateEditFrom.DateTime.ToString("yyyy-MM-dd");
            sqlParams[3].Value = this.dateEditTo.DateTime.ToString("yyyy-MM-dd");
            sqlParams[4].Value = this.textEditPatientSN.Text.Trim();
            sqlParams[5].Value = this.textEditPatientName.Text.Trim();
            sqlParams[6].Value = queryType;
            DataTable dataTable = m_App.SqlHelper.ExecuteDataTable("usp_QueryQuitPatientNoDoctor", sqlParams, CommandType.StoredProcedure);
            return dataTable;
        }

        /// <summary>
        /// 设置我的病人列表
        /// </summary>
        public void InitMyInpatient()
        {
            Thread initMyInpatientThread = new Thread(new ThreadStart(InitMyInpatientInner));
            initMyInpatientThread.Start();
        }

        /// <summary>
        /// 获取我分管的病人
        /// </summary>
        private void InitMyInpatientInner()
        {
            try
            {
                this.myPats = m_DataManager.GetCurrentBedInfos(m_DeptId, m_WardId, QueryType.OWN);
                InitMyInpatientInnerInvoke(myPats);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitMyInpatientInnerInvoke(DataTable dtPats)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(InitMyInpatientInner));
                }
                else
                {
                    //设置入院日期为入院时间还是入科日期（依据配置）add by cyq 2013-03-14
                    SetInHosOrInWardDate();
                    this.cmbQueryType.SelectedIndex = 0;

                    //在这处理绑定的数据源，关于婴儿的操作 2012年6月11日 16:15:05
                    DataTable newDt = myPats;

                    string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
                    for (int i = 0; i < newDt.Rows.Count; i++)
                    {
                        ResultName = DataManager.GetPatsBabyContent(m_App, newDt.Rows[i]["noofinpat"].ToString());
                        newDt.Rows[i]["PatName"] = ResultName;
                    }
                    this.gridMain.DataSource = newDt;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 绑定左侧lable信息
        /// </summary>
        private void BindLabText()
        {

            string sRowFilter = AddFilter();

            if (sRowFilter == "")
                sRowFilter = "1=1 ";

            this.myPats.DefaultView.RowFilter = sRowFilter;
            this.labAllCnt.Text = gridMain.MainView.DataRowCount + " 人";

            this.myPats.DefaultView.RowFilter = sRowFilter + " and HLJB = '一级护理'";
            this.labOneCnt.Text = gridMain.MainView.DataRowCount + " 人";

            this.myPats.DefaultView.RowFilter = sRowFilter + " and HLJB = '二级护理'";
            this.labTwoCnt.Text = gridMain.MainView.DataRowCount + " 人";

            this.myPats.DefaultView.RowFilter = sRowFilter + " and HLJB = '三级护理'";
            this.labThreeCnt.Text = gridMain.MainView.DataRowCount + " 人";

            this.myPats.DefaultView.RowFilter = sRowFilter;
        }

        /// <summary>
        /// 查询事件 - 科室历史病人查询
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-14</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                GetHistoryInPat();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void cmbQueryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.cmbQueryType.SelectedText == "本人")
            //{
            //    checkEditShowShift.Checked = false;
            //    checkEditShowOutHos.Checked = false;
            //    checkEditOnlyShowShift.Checked = false;
            //    checkEditOnlyShowOutHos.Checked = false;

            //    checkEditShowShift.Enabled = true;
            //    checkEditShowOutHos.Enabled = true;
            //    checkEditOnlyShowShift.Enabled = true;
            //    checkEditOnlyShowOutHos.Enabled = true;
            //    InitAllPat(1);
            //}
            //else if (this.cmbQueryType.SelectedText == "全科")
            //{
            //    checkEditShowShift.Checked = false;
            //    checkEditShowOutHos.Checked = false;
            //    checkEditOnlyShowShift.Checked = false;
            //    checkEditOnlyShowOutHos.Checked = false;

            //    checkEditShowShift.Enabled = false;
            //    checkEditShowOutHos.Enabled = false;
            //    checkEditOnlyShowShift.Enabled = false;
            //    checkEditOnlyShowOutHos.Enabled = false;
            //    InitAllPat(0);
            //}
        }

        private string GetSelectedGridView()
        {
            if (gridViewGridWardPat.FocusedRowHandle < 0) return "";

            DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
            if (xtraTabControl1.SelectedTabPage == MyInpTabPage)
            {
                dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
            }
            else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)
            {
                dataRow = m_UserControlAllListIno.GetSelectedGridViewRow();
            }
            else if (xtraTabControl1.SelectedTabPage == HistoryInpTabPage)
            {
                dataRow = gridViewHistoryInfo.GetDataRow(gridViewHistoryInfo.FocusedRowHandle);
            }
            else if (xtraTabControl1.SelectedTabPage == FailTabPage)
            {
                if (FailTabPage.Controls.Count > 0)
                {
                    UCFail fail = FailTabPage.Controls[0] as UCFail;
                    dataRow = fail.GetSelectedGridViewRow();
                }
            }
            else if (xtraTabControl1.SelectedTabPage == xtraTabPageCheck)
            {
                dataRow = gridViewThreeLevelCheck.GetDataRow(gridViewThreeLevelCheck.FocusedRowHandle);
            }
            else if (xtraTabControl1.SelectedTabPage == paintTran)
            {
                if (paintTran.Controls.Count > 0)
                {
                    UCTran fail = FailTabPage.Controls[0] as UCTran;
                    dataRow = fail.GetSelectedGridViewRow();
                }
            }
            if (dataRow == null) return "";
            return dataRow["NoOfInpat"].ToString();
        }

        private void gridViewGridWardPat_DoubleClick(object sender, EventArgs e)
        {
            GridHitInfo hitInfo = gridViewGridWardPat.CalcHitInfo(gridMain.PointToClient(Cursor.Position));
            if (hitInfo.RowHandle < 0) return;

            decimal syxh = GetCurrentPat(gridViewGridWardPat);
            if (syxh < 0) return;

            DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
            string noofinpat = dataRow["noofinpat"].ToString();
            if (DataManager.HasBaby(noofinpat))
            {
                ChoosePatOrBaby choosepat = new ChoosePatOrBaby(m_App, noofinpat);
                choosepat.StartPosition = FormStartPosition.CenterParent;
                if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_App.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));
                    m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());

                }
            }
            else
            {
                m_App.ChoosePatient(syxh);
                m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
        }

        /// <summary>
        /// 获取当前病患
        /// </summary>
        /// <returns></returns>
        private decimal GetCurrentPat()
        {
            if (gridViewGridWardPat.FocusedRowHandle < 0)
            {
                return -1;
            }
            else
            {
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                if (dataRow == null)
                {
                    return -1;
                }
                return Convert.ToDecimal(dataRow["NoOfInpat"]);
            }

        }

        private void barButtonItem_PersonalInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                if (dataRow == null || string.IsNullOrEmpty(dataRow["NoOfInpat"].ToString()))
                {
                    return;
                }
                //to do 调用病患基本信息窗体
                Assembly AspatientInfo = Assembly.Load("DrectSoft.Core.RedactPatientInfo");
                Type TypatientInfo = AspatientInfo.GetType("DrectSoft.Core.RedactPatientInfo.XtraFormPatientInfo");
                DevExpress.XtraEditors.XtraForm patientInfo = (DevExpress.XtraEditors.XtraForm)Activator.CreateInstance(TypatientInfo, new object[] { m_App, dataRow["NoOfInpat"].ToString() });
                patientInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                //m_App.CustomMessageBox.MessageShow("查看病人信息失败！");
                MessageBox.Show("查看病人信息失败" + ex.Message);
            }
        }

        /// <summary>
        /// 文书录入事件
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_Record_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
                {
                    DocumentsWrite();
                }
                else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                {
                    m_UserControlAllListIno.DocumentsWrite();
                }
                else if (xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//科室历史病人
                { }
                else if (xtraTabControl1.SelectedTabPage == FailTabPage)//已出院未归档
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
                { }
                else if (xtraTabControl1.SelectedTabPage == Replenish)//补写病历
                { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 文书录入方法 --- 我的病人
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        private void DocumentsWrite()
        {
            try
            {
                decimal syxh = GetCurrentPat();
                if (syxh < 0) return;
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                string noofinpat = dataRow["noofinpat"].ToString();
                //m_App.ChoosePatient(syxh);
                if (DataManager.HasBaby(noofinpat))
                {
                    ChoosePatOrBaby choosepat = new ChoosePatOrBaby(m_App, noofinpat);
                    choosepat.StartPosition = FormStartPosition.CenterParent;
                    if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        m_App.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));
                        m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                    }
                }
                else
                {
                    m_App.ChoosePatient(syxh);
                    m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 显示一级护理患者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void labOneCnt_Click(object sender, EventArgs e)
        {
            //this.gridMain.DataSource = DataTableSelect(myPats, "HLJB = '一级护理'");
            //this.myPats.DefaultView.RowFilter = AddFilter();

            string sRowFilter = AddFilter();

            if (sRowFilter == "")
                sRowFilter = "1=1 ";

            this.myPats.DefaultView.RowFilter = sRowFilter + " and HLJB = '一级护理'";
            //BindLabText();
        }

        private void labTwoCnt_Click(object sender, EventArgs e)
        {
            string sRowFilter = AddFilter();

            if (sRowFilter == "")
                sRowFilter = "1=1 ";

            this.myPats.DefaultView.RowFilter = sRowFilter + " and HLJB = '二级护理'";

            //BindLabText();
        }

        private void labThreeCnt_Click(object sender, EventArgs e)
        {
            string sRowFilter = AddFilter();

            if (sRowFilter == "")
                sRowFilter = "1=1 ";
            this.myPats.DefaultView.RowFilter = sRowFilter + " and HLJB = '三级护理'";

            //BindLabText();
        }

        private void labAllCnt_Click(object sender, EventArgs e)
        {
            //this.gridMain.DataSource = myPats;
            string sRowFilter = AddFilter();

            if (sRowFilter == "")
                sRowFilter = "1=1 ";
            this.myPats.DefaultView.RowFilter = sRowFilter;
        }

        private DataTable DataTableSelect(DataTable pats, string expression)
        {
            DataTable dt = pats.Clone();
            DataRow[] rows = pats.Select(expression);
            foreach (DataRow dr in rows)
            {
                dt.Rows.Add(dr.ItemArray);
            }
            return dt;
        }

        private void popupMenu1_BeforePopup(object sender, CancelEventArgs e)
        {

        }

        /// <summary>
        /// 右键菜单
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、右键小标题无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barManager1_QueryShowPopupMenu(object sender, QueryShowPopupMenuEventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewGridWardPat.CalcHitInfo(gridMain.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    e.Cancel = true;
                    return;
                }
                if (e.Control == this.gridMain)
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 临床路径事件
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
                {
                    ClinicalPath();
                }
                else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                {
                    m_UserControlAllListIno.ClinicalPath();
                }
                else if (xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//科室历史病人
                { }
                else if (xtraTabControl1.SelectedTabPage == FailTabPage)//已出院未归档
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
                { }
                else if (xtraTabControl1.SelectedTabPage == Replenish)//补写病历
                { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 临床路径方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        private void ClinicalPath()
        {
            try
            {
                decimal syxh = GetCurrentPat();
                if (syxh < 0) return;
                m_App.ChoosePatient(syxh);
                m_App.LoadPlugIn("DrectSoft.Core.DoctorTasks.dll", "DrectSoft.Core.DoctorTasks.InpatientPathForm");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 会诊申请事件
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AppConsult_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
                {
                    ApplyConsult();
                }
                else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                {
                    m_UserControlAllListIno.ApplyConsult();
                }
                else if (xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//科室历史病人
                { }
                else if (xtraTabControl1.SelectedTabPage == FailTabPage)//已出院未归档
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
                { }
                else if (xtraTabControl1.SelectedTabPage == Replenish)//补写病历
                { }

                //刷新会诊列表中的数据
                m_UCConsultationForDocCenter.BindConsultationThread();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 会诊申请方法 Modified by wwj 2013-03-01
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        private void ApplyConsult()
        {
            #region 已注销
            //try
            //{
            //    decimal syxh = GetCurrentPat();
            //    if (syxh < 0) return;
            //    m_App.ChoosePatient(syxh);

            //    FormConsultationApply formApply = new FormConsultationApply(syxh.ToString(), m_App, true);
            //    formApply.StartPosition = FormStartPosition.CenterParent;
            //    formApply.ShowDialog();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            #endregion
            try
            {
                decimal syxh = GetCurrentPat();
                if (syxh < 0)
                {
                    return;
                }
                //调用新的会诊申请界面
                FormApplyForMultiply formApply = new FormApplyForMultiply(syxh.ToString(), m_App, "", false);
                formApply.StartPosition = FormStartPosition.CenterParent;
                formApply.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #region 定时器

        private void timerLoadData_Tick(object sender, EventArgs e)
        {
            try
            {
                m_UCConsultationForDocCenter.BindConsultationThread();
                //GetQCTiXong();
            }
            catch (Exception ex)
            {
                m_App.CustomMessageBox.MessageShow("定时器出现错误：" + ex.Message);
            }

        }
        #endregion

        #region 对会诊列表的控制

        /// <summary>
        /// 已否决
        /// </summary>
        /// <param name="dr"></param>
        private void RejectConsultion(DataRow dr)
        {
            ////已否决弹出申请页面
            //string noOfFirstPage = dr["NoOfInpat"].ToString();
            //string consultTypeID = dr["ConsultTypeID"].ToString();
            //string consultApplySn = dr["ConsultApplySn"].ToString();

            //FormApproveForMultiply formApprove = new FormApproveForMultiply(noOfFirstPage, m_App, consultApplySn);
            ////formApprove.ReadOnlyControl();
            //formApprove.StartPosition = FormStartPosition.CenterParent;
            //formApprove.ShowDialog();
            //decimal syxh = GetCurrentPat();
            //if (syxh < 0) return;
            ////m_App.ChoosePatient(syxh);

            //FormConsultationApply formApply = new FormConsultationApply(syxh.ToString(), m_App, true);
            //formApply.StartPosition = FormStartPosition.CenterParent;
            //formApply.ShowDialog();

            string noOfFirstPage = dr["NoOfInpat"].ToString();
            string consultTypeID = dr["ConsultTypeID"].ToString();
            string consultApplySn = dr["ConsultApplySn"].ToString();

            FormConsultationApply formApply = new FormConsultationApply(noOfFirstPage, m_App, consultApplySn);
            formApply.StartPosition = FormStartPosition.CenterParent;
            formApply.ShowApprove(consultApplySn);
            formApply.ShowDialog();
        }
        /// <summary>
        /// 已取消
        /// </summary>
        /// <param name="dr"></param>
        private void CancelConsultion(DataRow dr)
        {
            string noOfFirstPage = dr["NoOfInpat"].ToString();
            string consultTypeID = dr["ConsultTypeID"].ToString();
            string consultApplySn = dr["ConsultApplySn"].ToString();

            FormRecordForMultiply formRecrodForMultiply = new FormRecordForMultiply(noOfFirstPage, m_App, consultApplySn);
            formRecrodForMultiply.StartPosition = FormStartPosition.CenterParent;
            formRecrodForMultiply.ShowDialog();
        }

        /// <summary>
        /// 待审核
        /// </summary>
        /// <param name="dr"></param>
        private void ConsultionConfirm(DataRow dr)
        {
            string noOfFirstPage = dr["NoOfInpat"].ToString();
            string consultTypeID = dr["ConsultTypeID"].ToString();
            string consultApplySn = dr["ConsultApplySn"].ToString();

            FormApproveForMultiply formApprove = new FormApproveForMultiply(noOfFirstPage, m_App, consultApplySn);
            formApprove.StartPosition = FormStartPosition.CenterScreen;
            formApprove.ShowDialog();
        }

        /// <summary>
        /// 待会诊(可以双击直接填写会诊信息)
        /// </summary>
        /// <param name="dr"></param>
        private void WaitConsultaion(DataRow dr)
        {
            string noOfFirstPage = dr["NoOfInpat"].ToString();
            string consultTypeID = dr["ConsultTypeID"].ToString();
            string consultApplySn = dr["ConsultApplySn"].ToString();

            if (dr["applyuser"].ToString() == m_App.User.Id)// 当前登录人是申请人，可以进行审核操作 
            {
                FormRecordForMultiply formRecrodForMultiply = new FormRecordForMultiply(noOfFirstPage, m_App, consultApplySn);
                formRecrodForMultiply.StartPosition = FormStartPosition.CenterParent;
                formRecrodForMultiply.ShowDialog();
            }
            else
            {
                FormRecordForMultiply formRecrodForMultiply = new FormRecordForMultiply(noOfFirstPage, m_App, consultApplySn);
                formRecrodForMultiply.StartPosition = FormStartPosition.CenterParent;
                formRecrodForMultiply.ReadOnlyControl();
                formRecrodForMultiply.ShowDialog();
            }


        }
        #endregion

        /// <summary>
        /// 右面遗留任务双击进入病历编辑页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewHistory_DoubleClick(object sender, EventArgs e)
        {
            //if (gridViewHistory.FocusedRowHandle < 0)
            //    return;

            //DataRow dataRow = gridViewHistory.GetDataRow(gridViewHistory.FocusedRowHandle);
            //if (dataRow == null)
            //    return;

            //decimal syxh = Convert.ToDecimal(dataRow["noofinpat"]);
            //if (syxh < 0) return;
            //m_App.ChoosePatient(syxh);
            //m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());

        }

        private void monthCalendar1_CustomDrawDayNumberCell(object sender, DevExpress.XtraEditors.Calendar.CustomDrawDayNumberCellEventArgs e)
        {
            if (e.Date.ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                e.Graphics.FillRectangle(Brushes.Blue, e.Bounds);
                e.Graphics.DrawString(e.Date.Day.ToString(), e.Style.Font, Brushes.White, e.Bounds);
                e.Handled = true;
            }

            else if (e.Date.ToString("yyyy-MM-dd") == monthCalendar1.DateTime.ToString("yyyy-MM-dd"))
            {
                e.Graphics.FillRectangle(Brushes.Green, e.Bounds);
                e.Graphics.DrawString(e.Date.Day.ToString(), e.Style.Font, Brushes.White, e.Bounds);
                e.Handled = true;
            }
        }

        /// <summary>
        /// 双击审核列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlThreeLevelCheck_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gridViewThreeLevelCheck.FocusedRowHandle < 0)
                return;

            DataRow dataRow = gridViewThreeLevelCheck.GetDataRow(gridViewThreeLevelCheck.FocusedRowHandle);
            if (dataRow == null)
                return;

            decimal syxh = Convert.ToDecimal(dataRow["noofinpat"]);
            if (syxh < 0) return;
            m_App.CurrentSelectedEmrID = dataRow["id"].ToString();
            m_App.ChoosePatient(syxh);
            m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());

        }

        #region CheckedChanged事件

        private void checkEditShowShift_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditShowShift.Checked)
            {
                checkEditOnlyShowShift.Checked = false;
                checkEditOnlyShowOutHos.Checked = false;
            }
            this.myPats.DefaultView.RowFilter = AddFilter();
            BindLabText();
        }

        private void checkEditShowOutHos_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditShowOutHos.Checked)
            {
                checkEditOnlyShowShift.Checked = false;
                checkEditOnlyShowOutHos.Checked = false;
            }
            this.myPats.DefaultView.RowFilter = AddFilter();
            BindLabText();
        }

        private void checkEditOnlyShowShift_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditOnlyShowShift.Checked)
            {
                checkEditShowShift.Checked = false;
                checkEditShowOutHos.Checked = false;
                checkEditOnlyShowOutHos.Checked = false;
            }
            this.myPats.DefaultView.RowFilter = AddFilter();
            BindLabText();
        }

        private void checkEditOnlyShowOutHos_CheckedChanged(object sender, EventArgs e)
        {
            if (checkEditOnlyShowOutHos.Checked)
            {
                checkEditShowShift.Checked = false;
                checkEditShowOutHos.Checked = false;
                checkEditOnlyShowShift.Checked = false;
            }
            this.myPats.DefaultView.RowFilter = AddFilter();
            BindLabText();
        }

        /// <summary>
        /// 增加筛选条件
        /// </summary>
        /// <returns></returns>
        private string AddFilter()
        {
            ((DataView)gridMain.MainView.DataSource).RowFilter = "";
            string filter1 = "";
            string filter2 = "";
            string filter = "";

            //if (!checkEditShowShift.Checked)//add by ywk 2012年11月7日15:28:45 暂时不要
            //{
            //    filter1 = " extra <> '属于其他科室' ";
            //}
            //if (checkEditOnlyShowShift.Checked)
            //{
            //    filter1 = " extra = '属于其他科室' ";
            //}

            if (!checkEditShowOutHos.Checked)
            {
                filter2 = " brzt IN (1501, 1504, 1505, 1506, 1507) "; //在院患者
            }

            if (checkEditOnlyShowOutHos.Checked)
            {
                filter2 = " brzt NOT IN (1500, 1501, 1504, 1505, 1506, 1507) "; //出院患者
            }

            if (filter1.Trim() == "" && filter2.Trim() != "")
            {
                filter = filter2;
            }
            if (filter1.Trim() != "" && filter2.Trim() == "")
            {
                filter = filter1;
            }
            if (filter1.Trim() != "" && filter2.Trim() != "")
            {
                filter = filter1 + " AND " + filter2;
            }
            return filter;
        }

        #endregion

        /// <summary>
        /// 双击事件 - 科室历史病人查询
        /// edit by Yanqiao.Cai 2012-11-14
        /// 1、add try ... catch
        /// 2、功能调整(双击进入婴儿选择页面)
        /// 3、双击小标题无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridHistoryInp_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewHistoryInfo.CalcHitInfo(gridHistoryInp.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                DataRow dataRow = gridViewHistoryInfo.GetDataRow(gridViewHistoryInfo.FocusedRowHandle);
                if (dataRow == null)//add by xlb 2012-12-25
                {
                    return;
                }
                string noofinpat = dataRow["noofinpat"].ToString();
                if (DataManager.HasBaby(noofinpat))
                {
                    ChoosePatOrBaby choosepat = new ChoosePatOrBaby(m_App, noofinpat);
                    choosepat.StartPosition = FormStartPosition.CenterParent;
                    if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        m_App.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));
                        m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                    }
                }
                else
                {
                    m_App.ChoosePatient(Convert.ToDecimal(noofinpat));
                    m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                }

                #region by cyq 2012-11-14 双击进入婴儿选择页面
                //if (gridViewHistoryInfo.FocusedRowHandle < 0) return;

                //DataRowView drv = gridViewHistoryInfo.GetRow(gridViewHistoryInfo.FocusedRowHandle) as DataRowView;
                //if (drv != null)
                //{
                //    decimal syxh = GetCurrentPat(gridViewHistoryInfo);
                //    if (syxh < 0) return;

                //    if (IsLock(syxh))
                //    {
                //        //已归档，则双击进入病历借阅
                //        ApplyExamine frm = new ApplyExamine(m_App);
                //        frm.SetPatID(GetCurrentPatID(gridViewHistoryInfo));
                //        frm.ShowDialog(this.Owner);
                //    }
                //    else
                //    {
                //        //未归档，则双击进入病历编辑
                //        m_App.ChoosePatient(syxh);
                //        m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 列表加载事件 - 科室历史病人查询
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-14</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewHistoryInfo_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            try
            {
                s.Alignment = StringAlignment.Near;
                s.LineAlignment = StringAlignment.Center;
                if (e.CellValue == null)
                {
                    return;
                }
                DataRowView drv = gridViewHistoryInfo.GetRow(e.RowHandle) as DataRowView;
                //取得病人名字
                string patname = drv["patname"].ToString().Trim();
                if (e.Column.FieldName == colname.FieldName)
                {
                    if (patname.Contains("婴儿"))
                    {
                        Region oldRegion = e.Graphics.Clip;
                        e.Graphics.Clip = new Region(e.Bounds);

                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                        e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red,
                            new RectangleF(e.Bounds.Location, new SizeF(300, e.Bounds.Height)), s);

                        e.Graphics.Clip = oldRegion;
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private decimal GetCurrentPat(GridView gridview)
        {
            if (gridview.FocusedRowHandle < 0)
                return -1;
            else
            {
                DataRow dataRow = gridview.GetDataRow(gridview.FocusedRowHandle);
                if (dataRow == null) return -1;

                return Convert.ToDecimal(dataRow["NoOfInpat"]);
            }

        }

        private string GetCurrentPatID(GridView gridview)
        {
            if (gridview.FocusedRowHandle < 0)
                return "";
            else
            {
                DataRow dataRow = gridview.GetDataRow(gridview.FocusedRowHandle);
                if (dataRow == null) return "";

                return dataRow["PatID"].ToString();
            }
        }

        private bool IsLock(decimal noOfInpat)
        {
            return m_DataManager.EmrDocIsLock(noOfInpat);
        }

        private void textEditPATID_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                refreshGridView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void textEditPATNAME_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                refreshGridView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void textEditPATBEDNO_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                refreshGridView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void refreshGridView()
        {
            string PATID = textEditPATID.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
            string PATNAME = textEditPATNAME.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
            string PATBEDNO = textEditPATBEDNO.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
            string InwDia = textEditInwDia.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");

            string filter = " PATID like '%{0}%' and (PatName like '%{1}%' or PY like '%{1}%' or WB like '%{1}%') and (BedID like '%{2}%' or '{2}' is null or '{2}'='')  and (ZDMC like '%{3}%'or '{3}' is null or '{3}' = '') ";
            DataTable dt = gridMain.DataSource as DataTable;
            if (dt != null)
            {
                string rowFilter = AddFilter();
                if (rowFilter != "")
                {
                    dt.DefaultView.RowFilter = rowFilter + " and " + string.Format(filter, PATID, PATNAME, PATBEDNO, InwDia);
                }
                else
                {
                    dt.DefaultView.RowFilter = string.Format(filter, PATID, PATNAME, PATBEDNO, InwDia);
                }
            }
        }

        /// <summary>
        /// 撤销我的病人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemUndoMyInpatient_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {

                if (gridViewGridWardPat.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选择一条病人记录。");
                    return;
                }
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }

                if (MessageBox.Show("您确定要撤销病人 " + dataRow["PATNAME"].ToString() + " 吗？", "提示信息", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                //decimal syxh = GetCurrentPat();
                decimal syxh = Convert.ToDecimal(dataRow["NoOfInpat"]);
                if (syxh < 0) return;
                string sqlUndoMyInpatient = " update Doctor_AssignPatient set valid = '0' where valid = '1' and noofinpat = '{0}' ";
                m_App.SqlHelper.ExecuteNoneQuery(string.Format(sqlUndoMyInpatient, syxh), CommandType.Text);
                string sqlUndoMyInpatient2 = " update inpatient set resident = '' where noofinpat = '{0}' ";
                m_App.SqlHelper.ExecuteNoneQuery(string.Format(sqlUndoMyInpatient2, syxh), CommandType.Text);

                DataView dt = gridViewGridWardPat.DataSource as DataView;
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Table.Rows)
                    {
                        if (dr["noofinpat"].ToString() == syxh.ToString())
                        {
                            dt.Table.Rows.Remove(dr);
                            break;
                        }
                    }
                    dt.Table.AcceptChanges();
                }

                //刷新全部患者
                RefreshAllPatientList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshAllPatientList()
        {
            if (AllInpTabPage.Controls.Count > 1)
                AllInpTabPage.Controls.Clear();
            InitAllListView();
        }

        private void barButtonItemReportCard_ItemClick(object sender, ItemClickEventArgs e)
        {
            string noofinpat = GetCurrentPat().ToString();
            XtraForm form = (XtraForm)Activator.CreateInstance(
                Type.GetType("DrectSoft.Core.ZymosisReport.ReportCardApply,DrectSoft.Core.ZymosisReport"),
                new object[] { m_App, noofinpat });
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }

        /// <summary>
        /// 病人信息事件
        /// edit by Yanqiao.Cai 2012-11-09
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemPatientInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
                {
                    ViewPatientInfo();
                }
                else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                {
                    m_UserControlAllListIno.ViewPatientInfo();
                }
                else if (xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//科室历史病人
                {

                }
                else if (xtraTabControl1.SelectedTabPage == FailTabPage)//已出院未归档
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
                { }
                else if (xtraTabControl1.SelectedTabPage == Replenish)//补写病历
                { }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 病人信息方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        private void ViewPatientInfo()
        {
            try
            {
                string noOfInpat = GetSelectedGridView();
                if (!string.IsNullOrEmpty(noOfInpat))
                {
                    //to do 调用病患基本信息窗体
                    Assembly AspatientInfo = Assembly.Load("DrectSoft.Core.RedactPatientInfo");
                    Type TypatientInfo = AspatientInfo.GetType("DrectSoft.Core.RedactPatientInfo.XtraFormPatientInfo");
                    DevExpress.XtraEditors.XtraForm patientInfo = (DevExpress.XtraEditors.XtraForm)Activator.CreateInstance(TypatientInfo, new object[] { m_App, noOfInpat });
                    patientInfo.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void barLargeButtonItemAll_ItemClick(object sender, ItemClickEventArgs e)
        {
            xtraTabControl1.SelectedTabPageIndex = 1;
            if (AllInpTabPage.Controls.Count > 1)
                return;
            InitAllListView();
        }

        /// <summary>
        /// 病案借阅 --- 工具栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemEmrApply_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                MedicalRecordManage.UI.MedicalRecordApply frm = new MedicalRecordManage.UI.MedicalRecordApply(m_App);
                frm.ShowDialog(this.Owner);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 病案借阅 --- 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_emrApply_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //ApplyExamine frm = new ApplyExamine(m_App);
                //frm.ShowDialog(this.Owner);
                MedicalRecordManage.UI.MedicalRecordApply frm = new MedicalRecordManage.UI.MedicalRecordApply(m_App);
                frm.ShowDialog(this.Owner);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void barLargeButtonItemReportCard_ItemClick(object sender, ItemClickEventArgs e)
        {
            m_App.LoadPlugIn("DrectSoft.Core.ZymosisReport.dll", "DrectSoft.Core.ZymosisReport.MainForm");
        }

        private void barLargeButtonItemRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在刷新，请稍候...");
                checkEditShowShift.Checked = false;
                checkEditShowOutHos.Checked = false;
                checkEditOnlyShowShift.Checked = false;
                checkEditOnlyShowOutHos.Checked = false;

                checkEditShowShift.Enabled = true;
                checkEditShowOutHos.Enabled = true;
                checkEditOnlyShowShift.Enabled = true;
                checkEditOnlyShowOutHos.Enabled = true;
                ControlRefresh();
                Reset();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
        }

        private void gridControlPoint_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gridViewEmrPoint.FocusedRowHandle < 0)
            {
                return;
            }
            DataRow dataRow = gridViewEmrPoint.GetDataRow(gridViewEmrPoint.FocusedRowHandle);
            if (dataRow == null)
            {
                return;
            }
            decimal syxh = Convert.ToDecimal(dataRow["noofinpat"]);
            if (syxh < 0) return;
            m_App.CurrentSelectedEmrID = dataRow["recorddetailid"].ToString().Trim();
            m_App.ChoosePatient(syxh);
            m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
        }
        StringFormat s = new StringFormat();
        /// <summary>
        /// 控制带婴儿信息的显示、
        /// add by ywk 2012年6月8日 10:32:09
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewGridWardPat_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            s.Alignment = StringAlignment.Near;
            s.LineAlignment = StringAlignment.Center;
            if (e.CellValue == null) return;
            DataRowView drv = gridViewGridWardPat.GetRow(e.RowHandle) as DataRowView;
            //取得病人名字
            string patname = drv["patname"].ToString().Trim();

            if (e.Column == colname)
            {
                if (patname.Contains("婴儿"))
                {
                    Region oldRegion = e.Graphics.Clip;
                    e.Graphics.Clip = new Region(e.Bounds);

                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                    e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red,
                        new RectangleF(e.Bounds.Location, new SizeF(300, e.Bounds.Height)), s);

                    e.Graphics.Clip = oldRegion;
                    e.Handled = true;
                }

            }
        }

        private void DocCenter_Activated(object sender, EventArgs e)
        {
            FocusControl();
        }

        int oldFocusRowHandle;
        /// <summary>
        /// 设定婴儿
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemBaby_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                try
                {
                    if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
                    {
                        SetBabys();
                    }
                    else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                    {
                        m_UserControlAllListIno.SetBabys();
                    }
                    else if (xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//科室历史病人
                    { }
                    else if (xtraTabControl1.SelectedTabPage == FailTabPage)//已出院未归档
                    { }
                    else if (xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
                    { }
                    else if (xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
                    { }
                    else if (xtraTabControl1.SelectedTabPage == Replenish)//补写病历
                    { }
                }
                catch (Exception ex)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设定婴儿方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        private void SetBabys()
        {
            try
            {
                if (gridViewGridWardPat.FocusedRowHandle < 0)
                    return;
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                string syxh = dataRow["NoOfInpat"].ToString();
                string patname = dataRow["PatName"].ToString();
                if ((!string.IsNullOrEmpty(syxh)) && (syxh != "-1"))
                {
                    oldFocusRowHandle = gridViewGridWardPat.FocusedRowHandle;
                    SetPatientsBaby setBaby = new SetPatientsBaby(syxh, m_App, patname, this);
                    setBaby.StartPosition = FormStartPosition.CenterScreen;//弹出窗体在中间 
                    setBaby.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void simpleButtonRefresh_Click(object sender, EventArgs e)
        {
            GetQCTiXong();
        }

        private void simpleButtonShouSuo_Click(object sender, EventArgs e)
        {
            gridView1.CollapseAllGroups();
        }

        private void simpleButtonZhanKai_Click(object sender, EventArgs e)
        {
            gridView1.ExpandAllGroups();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string noofinpat = dataRow["noofinpat"].ToString();
            if (DataManager.HasBaby(noofinpat))
            {
                ChoosePatOrBaby choosepat = new ChoosePatOrBaby(m_App, noofinpat);
                choosepat.StartPosition = FormStartPosition.CenterParent;
                if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_App.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));
                    m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                }
            }
            else
            {
                m_App.ChoosePatient(Convert.ToDecimal(noofinpat));
                m_App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
        }

        private void gridView1_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
        {

        }

        private void btnChanged_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                string noofinpat = dataRow["noofinpat"].ToString();
                m_DataManager.SetRHQCHasXiuGai(noofinpat);
                m_App.CustomMessageBox.MessageShow("修改成功！");
                GetQCTiXong();

            }
            catch (Exception ex)
            {

                m_App.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// 入院登记 --- 工具栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemInHosLogin_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                InHosLogin();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 入院登记 --- 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_inHosLogin_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                InHosLogin();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 入院登记方法
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        private void InHosLogin()
        {
            try
            {
                //string noOfInpat = GetSelectedGridView();
                //if (!string.IsNullOrEmpty(noOfInpat))
                // {
                //to do 调用病患基本信息窗体
                //BasePatientInfo info = new BasePatientInfo(m_App);
                //info.ShowCurrentPatInfo(dataRow["NoOfInpat"].ToString());
                XtraFormInHosLogin patientInfo = new XtraFormInHosLogin(m_App, null);
                patientInfo.ShowDialog();
                //add by cyq 2012-11-15 入院登记后刷新
                if (patientInfo.refreashFlag)
                {
                    if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
                    {
                        InitMyInpatient();
                    }
                    else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                    {
                        InitAllListView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑病人信息（晋城需求）
        /// add by ywk 2012年9月4日 09:01:58
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
                {
                    EditPatientInfo();
                }
                else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                {
                    m_UserControlAllListIno.EditPatientInfo();
                }
                else if (xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//科室历史病人
                { }
                else if (xtraTabControl1.SelectedTabPage == FailTabPage)//已出院未归档
                {
                    string noofinpat = m_UCFail.EditPatientInfo();
                    if (noofinpat != "")
                    {
                        XtraFormInHosLogin patientInfo = new XtraFormInHosLogin(m_App, noofinpat);
                        patientInfo.SetEnable(false);
                        patientInfo.Text = "编辑病人信息";
                        patientInfo.ShowDialog();
                    }
                }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
                { }
                else if (xtraTabControl1.SelectedTabPage == Replenish)//补写病历
                { }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑病人信息方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        private void EditPatientInfo()
        {
            try
            {
                if (gridViewGridWardPat.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选中一条记录");
                    return;
                }
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                string syxh = dataRow["NoOfInpat"].ToString();

                XtraFormInHosLogin patientInfo = new XtraFormInHosLogin(m_App, syxh);
                patientInfo.SetEnable(false);
                patientInfo.Text = "编辑病人信息";
                patientInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 病人出院
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem12_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == MyInpTabPage)//我的患者
                {
                    SetPatientOutHos();
                }
                else if (xtraTabControl1.SelectedTabPage == AllInpTabPage)//全部患者
                {
                    m_UserControlAllListIno.SetPatientOutHos();
                }
                else if (xtraTabControl1.SelectedTabPage == HistoryInpTabPage)//科室历史病人
                { }
                else if (xtraTabControl1.SelectedTabPage == FailTabPage)//已出院未归档
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPageCheck)//病历审核
                { }
                else if (xtraTabControl1.SelectedTabPage == xtraTabPagePoint)//病历评分
                { }
                else if (xtraTabControl1.SelectedTabPage == Replenish)//补写病历
                { }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 病人出院方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// </summary>
        /// edit by xlb 2012-12-24
        private void SetPatientOutHos()
        {
            try
            {
                if (gridViewGridWardPat.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选择一条病人记录。");
                    return;
                }
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                if (dataRow == null)//add xlb 2012-12-24
                {
                    return;
                }
                string syxh = dataRow["noofinpat"].ToString();
                if ((!string.IsNullOrEmpty(syxh)))
                {
                    //DialogResult dResult = m_App.CustomMessageBox.MessageShow("确定让病人出院吗？", CustomMessageBoxKind.QuestionYesNo);
                    DialogResult dResult = MessageBox.Show("您确定让 " + dataRow["PATNAME"].ToString() + " 出院吗？", "病人出院", MessageBoxButtons.YesNo);
                    if (dResult == DialogResult.Yes)
                    {
                        //string sql = string.Format("update inpatient i set i.status=1503 and i.outhosdept='{0}' and i.outhosward='{1}' and i.outwarddate='{2}' and i.outhosdate='{3}' where inpatient.noofinpat={4}",
                        // m_App.User.CurrentDeptId,
                        //m_App.User.CurrentWardId,
                        //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 
                        //Convert.ToInt32(syxh));
                        // m_App.SqlHelper.ExecuteNoneQuery(sql, CommandType.Text);
                        //xll 添加电子病历出院标记 1  emrouthos
                        string sql = "update inpatient i set i.status=1503,i.outhosdept=@outhostdept, i.outhosward=@outhostward,i.outwarddate=@outwarddate,i.outhosdate=@outhostdate,i.emrouthos='1' where i.noofinpat=@noofinpat";
                        SqlParameter[] sps ={
                                               new SqlParameter("@outhostdept",m_App.User.CurrentDeptId),
                                               new SqlParameter("@outhostward",m_App.User.CurrentWardId),
                                               new SqlParameter("@outwarddate",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                               new SqlParameter("@outhostdate",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                               new SqlParameter("@noofinpat",Convert.ToInt32(syxh))
                                            };
                        DS_SqlHelper.ExecuteNonQuery(sql, sps, CommandType.Text);
                        RefreshData();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 院内会诊 --- 工具栏
        /// </summary>
        /// edit by Yanqiao.Cai 2012-11-09
        /// 1、add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemConsultation_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                m_App.LoadPlugIn("DrectSoft.Core.Consultation.dll", "DrectSoft.Core.Consultation.ConsultationFormStartUp");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 院内会诊 --- 右键
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-09</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_consultation_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                m_App.LoadPlugIn("DrectSoft.Core.Consultation.dll", "DrectSoft.Core.Consultation.ConsultationFormStartUp");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 重置(科室病人历史查询)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-10</date>
        /// </summary>
        private void BtnReset()
        {
            textEditPatientSN.Text = "";
            textEditPatientName.Text = "";
            dateEditFrom.DateTime = DateTime.Now.AddMonths(-1);
            dateEditTo.DateTime = DateTime.Now;
            textEditHistory.Text = "";
            this.textEditPatientSN.Focus();
        }

        /// <summary>
        /// 重置
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-10</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
        {
            BtnReset();
        }

        /// <summary>
        /// 回车切换焦点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-10-11</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                //m_App.CustomMessageBox.MessageShow(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设为我的病人事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-08</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemSetMyPati_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                m_UserControlAllListIno.SetMyPatient();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 退出事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-08</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barLargeButtonItemExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (MyMessageBox.Show("您确定要退出吗？", "退出", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 清空事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-08</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_clear_Click(object sender, EventArgs e)
        {
            try
            {
                this.textEditPATBEDNO.Text = string.Empty;
                this.textEditPATID.Text = string.Empty;
                this.textEditPATNAME.Text = string.Empty;
                this.textEditInwDia.Text = string.Empty;
                this.textEditPATBEDNO.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textEditInwDia_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                refreshGridView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void barButtonItemRecord_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                string noofinpat = dataRow["noofinpat"] == null ? null : dataRow["noofinpat"].ToString();
                if (noofinpat == null)
                {
                    return;
                }
                nursingRecordForm = NursingRecordForm.CreateInstance();
                this.ShowVigalsHandle += new del_ShowVigals(nursingRecordForm.InitNursingRecord);
                OnShowVigals(noofinpat);
                nursingRecordForm.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void gridMain_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (nursingRecordForm != null && !nursingRecordForm.IsDisposed)
                {
                    DataRow dataRow = gridViewGridWardPat.GetDataRow(gridViewGridWardPat.FocusedRowHandle);
                    string noofinpat = dataRow["noofinpat"].ToString();
                    //nursingRecordForm = NursingRecordForm.CreateInstance();
                    //this.ShowVigalsHandle += new del_ShowVigals(nursingRecordForm.InitNursingRecord);
                    OnShowVigals(noofinpat);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 到处EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ToExcelColumnForm toexcelcolumn = new ToExcelColumnForm(this.gridMain, this.gridViewGridWardPat);
                toexcelcolumn.ShowDialog();
                if (toexcelcolumn.m_iCommandFlag == 1)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Title = "导出";
                    saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
                    DialogResult dialogResult = saveFileDialog.ShowDialog(this);
                    List<DrectSoft.Core.OwnBedInfo.ToExcelColumnForm.ListItem> list = toexcelcolumn.lists;
                    if (dialogResult == DialogResult.OK)
                    {
                        DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                        toexcelcolumn.dbgrid.ExportToXls(saveFileDialog.FileName.ToString(), true);

                        if (list.Count > 0)
                        {
                            foreach (GridColumn c in this.gridViewGridWardPat.Columns)
                            {
                                foreach (DrectSoft.Core.OwnBedInfo.ToExcelColumnForm.ListItem item in list)
                                {
                                    if (item.Value == c.Name)
                                    {
                                        c.Visible = true;
                                    }
                                }

                            }
                        }
                        m_App.CustomMessageBox.MessageShow("导出成功！");
                    }
                    else
                    {
                        if (list.Count > 0)
                        {
                            foreach (GridColumn c in this.gridViewGridWardPat.Columns)
                            {
                                foreach (DrectSoft.Core.OwnBedInfo.ToExcelColumnForm.ListItem item in list)
                                {
                                    if (item.Value == c.Name)
                                    {
                                        c.Visible = true;
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private bool IsColumn(string name, List<DrectSoft.Core.OwnBedInfo.ToExcelColumnForm.ListItem> list)
        {
            foreach (DrectSoft.Core.OwnBedInfo.ToExcelColumnForm.ListItem item in list)
            {
                if (Name == item.Value)
                {
                    return false;

                }
            }
            return true;

        }

        private void barLargeButtonItemWriteUp_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                MedicalRecordManage.UI.MedicalRecordApplyBack frm = new MedicalRecordManage.UI.MedicalRecordApplyBack(m_App);
                frm.ShowDialog(this.Owner);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridControlPoint_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 判断是不是科室主任
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool isdeptManager(string deptid, string userid)
        {
            bool result = false;
            try
            {
                DataTable deptmanager = GetDirectorDoc(deptid);
                foreach (DataRow dr in deptmanager.Rows)
                {
                    if (dr["ID"].ToString() == userid)
                    {
                        result = true;

                    }
                }
                if (!result)//add Ukey zhang 2016-10-09 一个主任兼顾几个科室
                {
                    DataTable usermanager = GetDirectorDocById(userid);
                    foreach (DataRow dr in usermanager.Rows)
                    {
                        if (dr["GRADE"].ToString() == "2000")
                        {
                            result = true;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                return result;
            }
            return result;
        }
        /// <summary>
        /// 根据科室编号获得此科室下对应的主任
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public DataTable GetDirectorDoc(string deptid)
        {
            string sql = string.Format(@"  
               SELECT * FROM users u
         WHERE (u.wardid IS NOT NULL or u.wardid != '')
           AND u.valid = '1' AND u.grade = '2000'and u.deptid='{0}'  ORDER BY u.ID ", deptid);
            return m_App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 根据人员ID获取人员信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetDirectorDocById(string userid)//add Ukey zhang 2016-10-09 一个主任兼顾几个科室
        {
            string sql = string.Format(@"  
               SELECT * FROM users u
         WHERE (u.wardid IS NOT NULL or u.wardid != '')
           AND u.valid = '1' AND u.grade = '2000'and u.id='{0}'  ORDER BY u.ID ", userid);
            return m_App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
        }
    }
}
