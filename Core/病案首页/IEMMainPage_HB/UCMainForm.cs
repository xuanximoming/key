using DevExpress.Utils;
using DevExpress.XtraTab;
using DrectSoft.Common;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using System;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
#pragma warning disable 0618

namespace DrectSoft.Core.IEMMainPage
{
    public partial class UCMainForm : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        #region var
        IEmrHost m_Host;
        DrawMainPageUtil util;
        IemMainPageManger manger;
        IemMainPageInfo info;

        private ShowUC ShowUCForm
        {
            get
            {
                if (_showUcForm == null)
                {
                    _showUcForm = new ShowUC(m_Host);
                }
                return _showUcForm;
            }
        }
        private ShowUC _showUcForm;

        private UCIemBasInfo m_UCIemBasInfo;
        private UCIemDiagnose m_UCIemDiagnose;
        private UCIemOperInfo m_UCIemOperInfo;
        private UCObstetricsBaby m_UCObstetricsBaby;
        private UCOthers m_UCOthers;
        private Inpatient CurrentInpatient;
        //病案室专用
        private bool editFlag = false;///是否可编辑病案首页
        private bool IsShowBackRecord = false;///是否提示归档
        #endregion
        public string TIP = string.Empty;
        public UCMainForm()
        {
            InitializeComponent();
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(141)))), ((int)(((byte)(189)))));
            //DataHelper help = new DataHelper();
            //string cansee = help.GetConfigValueByKey("EmrInputConfig");
            //XmlDocument doc1 = new XmlDocument();
            //doc1.LoadXml(cansee);
            //int m_AnOtherHeight = 0;
            //if (doc1.GetElementsByTagName("IemPageContorlVisable")[0].InnerText == "1")//可见诊断符合的一些栏位
            //{ }
        }

        public UCMainForm(string noofinpat)
            : this()
        {
            CurrentNoofinpat = noofinpat;
        }

        void BindParentFormCloseEvent()
        {
            if (this.Parent != null && this.Parent.Parent != null && this.Parent.Parent.Parent != null && this.Parent.Parent.Parent.Parent != null)
            {
                Form form = this.Parent.Parent.Parent.Parent as Form;
                if (form != null)
                {
                    form.FormClosed += new FormClosedEventHandler(form_FormClosed);
                }

                XtraTabPage tabPage = this.Parent as XtraTabPage;
                if (tabPage != null)
                {
                    tabPage.ControlRemoved += new ControlEventHandler(tabPage_ControlRemoved);
                }
            }
        }

        void tabPage_ControlRemoved(object sender, ControlEventArgs e)
        {
            DeleteMetaFile();
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            DeleteMetaFile();
        }

        void DeleteMetaFile()
        {
            if (util != null)
            {
                if (File.Exists(util.FilePath1))
                {
                    util.MF1.Dispose();
                    File.Delete(util.FilePath1);
                }
                if (File.Exists(util.FilePath2))
                {
                    util.MF2.Dispose();
                    File.Delete(util.FilePath2);
                }
            }
        }

        int GetPageWidth()
        {
            return Convert.ToInt32(util.m_PageWidth * 0.93f);
        }
        int GetPageHeight()
        {
            return Convert.ToInt32(util.m_PageHeight * 0.93f);
        }

        private void LoadForm()
        {
            if (manger == null)
            {
                manger = new IemMainPageManger(m_Host, CurrentInpatient);
            }
            info = manger.GetIemInfo();
            DeleteMetaFile();
            util = new DrawMainPageUtil(info);

            pictureBox1.Width = GetPageWidth();
            pictureBox1.Height = GetPageHeight();
            pictureBox2.Width = GetPageWidth();
            pictureBox2.Height = GetPageHeight();

            pictureBox1.BackgroundImage = util.MF1;
            pictureBox2.BackgroundImage = util.MF2;

            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;

            ReLocationPicture();
            BindParentFormCloseEvent();

            if (this.Parent != null)
            {
                if (this.Parent.AccessibleName != null
                    && this.Parent.AccessibleName.Trim() != "")//父窗体的AccessibleName不为空则表示没有编辑的权限
                {
                    simpleButton1.Visible = false;
                    simpleButton2.Visible = false;
                    simpleButton3.Visible = false;
                    simpleButton4.Visible = false;
                    simpleButton5.Visible = false;
                    simpleButton6.Visible = false;
                }
            }

            Employee emp = new Employee(m_Host.User.Id);
            emp.ReInitializeProperties();
            DataHelper help = new DataHelper();
            string hoscode = help.GetConfigValueByKey("HosCode");
            if (hoscode == "1")
            {
                string static_save = help.GetStatic_SaveValue(info.IemBasicInfo.NoOfInpat);
                if (static_save == "1")
                {
                    simpleButton1.Enabled = false;
                    simpleButton2.Enabled = false;
                    simpleButton3.Enabled = false;
                    simpleButton4.Enabled = false;
                    simpleButton5.Enabled = false;
                    simpleButton6.Enabled = false;
                }
            }
            if (emp.Grade.Trim() != "")
            {
                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), emp.Grade);
                if (grade == DoctorGrade.Nurse)
                {
                    simpleButton3.Enabled = false;
                    simpleButton4.Enabled = false;
                    simpleButton5.Enabled = false;
                    simpleButton6.Enabled = false;
                }
            }
            else
            {
                simpleButton1.Enabled = false;
                simpleButton2.Enabled = false;
                simpleButton3.Enabled = false;
                simpleButton4.Enabled = false;
                simpleButton5.Enabled = false;
                simpleButton6.Enabled = false;
            }
        }

        private void LoadForm2()
        {
            if (manger == null)
            {
                manger = new IemMainPageManger(m_Host, CurrentInpatient);
            }
            info = manger.GetIemInfo();
            DeleteMetaFile();
            util = new DrawMainPageUtil(info);

            pictureBox1.Width = GetPageWidth();
            pictureBox1.Height = GetPageHeight();
            pictureBox2.Width = GetPageWidth();
            pictureBox2.Height = GetPageHeight();

            pictureBox1.BackgroundImage = util.MF1;
            pictureBox2.BackgroundImage = util.MF2;

            ReLocationPicture();
            BindParentFormCloseEvent();
        }

        /// <summary>
        /// 重新定位PictureBox
        /// </summary>
        private void ReLocationPicture()
        {
            if (util == null) return;

            pictureBox1.Location = new Point((this.Width - pictureBox1.Width + this.simpleButton1.Width) / 2 - 8, this.AutoScrollPosition.Y);
            pictureBox2.Location = new Point((this.Width - pictureBox1.Width + this.simpleButton1.Width) / 2 - 8, pictureBox1.Location.Y + GetPageHeight() + 20);

            ReLocationEditButton();
        }

        #region 编辑Button

        //const float percentHeight1 = 0.1334f;
        const float percentHeight1 = 0f;
        //const float percentHeight2 = 0.397f;
        //const float percentHeight3 = 0.603f;
        //const float percentHeight4 = 0.38f;
        //const float percentHeight5 = 0.154f;
        //const float percentHeight6 = 0.466f;
        const float percentHeight2 = 0.38f;
        const float percentHeight3 = 0.62f;
        const float percentHeight4 = 0.210f;
        const float percentHeight5 = 0.140f;
        const float percentHeight6 = 0.650f;
        private void ReLocationEditButton()
        {
            DataHelper help = new DataHelper();
            string cansee = help.GetConfigValueByKey("EmrInputConfig");
            XmlDocument doc1 = new XmlDocument();
            doc1.LoadXml(cansee);
            int m_AnOtherHeight = 0;
            string changeHeight = doc1.GetElementsByTagName("IemPageContorlVisable")[0].InnerText;//可见诊断符合的一些栏位


            simpleButton1.Visible = false;
            simpleButton1.Height = Convert.ToInt32(percentHeight1 * GetPageHeight());
            simpleButton1.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y);

            if (changeHeight == "1")
            {
                simpleButton2.Height = 389;
                simpleButton2.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y + simpleButton1.Height);
            }
            else
            {
                simpleButton2.Height = Convert.ToInt32(percentHeight2 * GetPageHeight());
                simpleButton2.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y + simpleButton1.Height);
            }

            if (changeHeight == "1")
            {
                //simpleButton3.Height = 634;
                simpleButton3.Height = Convert.ToInt32(GetPageHeight() - simpleButton2.Height);
                simpleButton3.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y + simpleButton1.Height + simpleButton2.Height);
            }
            else
            {
                simpleButton3.Height = Convert.ToInt32(percentHeight3 * GetPageHeight());
                simpleButton3.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y + simpleButton1.Height + simpleButton2.Height);
            }

            if (changeHeight == "1")
            {
                simpleButton4.Height = 214;
                simpleButton4.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y);
            }
            else
            {
                simpleButton4.Height = Convert.ToInt32(percentHeight4 * GetPageHeight());
                simpleButton4.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y);
            }

            if (info.IemObstetricsBaby != null)
            {
                //simpleButton5.Visible = true;
                if (changeHeight == "1")//可见修改，修改病案管理阅览时的BUG  edit by ywk  2013年3月16日10:29:01 
                {
                    //simpleButton5.Visible = true;
                    simpleButton5.Height = 144;
                }
                else
                {
                    simpleButton5.Height = Convert.ToInt32(percentHeight5 * GetPageHeight());
                }
                simpleButton5.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y + simpleButton4.Height);

                //simpleButton6.Height = Convert.ToInt32(percentHeight6 * GetPageHeight());
                simpleButton6.Height = Convert.ToInt32(GetPageHeight() - simpleButton4.Height - simpleButton5.Height);
                simpleButton6.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y + simpleButton4.Height + simpleButton5.Height);
            }
            else
            {
                simpleButton5.Visible = false;
                simpleButton6.Height = Convert.ToInt32(GetPageHeight() - simpleButton4.Height);
                simpleButton6.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y + simpleButton4.Height);
            }

        }

        private bool IsFocusButton()
        {
            if (simpleButton1.Focused)
            {
                return true;
            }
            if (simpleButton2.Focused)
            {
                return true;
            }
            if (simpleButton3.Focused)
            {
                return true;
            }
            if (simpleButton4.Focused)
            {
                return true;
            }
            if (simpleButton5.Focused)
            {
                return true;
            }
            if (simpleButton6.Focused)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region IEMREditor 成员
        public Control DesignUI
        {
            get { return this; }
        }

        public new void Load(IEmrHost app)
        {
            try
            {
                m_Host = app;
                if (!string.IsNullOrEmpty(CurrentNoofinpat))
                {
                    CurrentInpatient = new Common.Eop.Inpatient(Convert.ToDecimal(CurrentNoofinpat));
                }
                else if (m_Host.CurrentPatientInfo != null)
                {
                    CurrentInpatient = m_Host.CurrentPatientInfo;
                }
                else
                {
                    return;
                }
                CurrentInpatient.ReInitializeAllProperties();

                manger = new IemMainPageManger(m_Host, CurrentInpatient);
                info = manger.GetIemInfo();

                LoadForm();

                //病案室人员拥有编辑病案首页的权限(未归档病历)
                InitFirstPageEditFlag(null == CurrentInpatient ? "" : CurrentInpatient.NoOfFirstPage.ToString());
                if (editFlag)
                {
                    SetButtonsEditState(true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Save()
        {
            manger.SaveData(info);
        }

        public string Title
        {
            get { return "病案首页"; }
        }

        public string GetConfigValueByKey(string key)
        {
            string sql1 = " select * from appcfg where configkey = '" + key + "'  ";
            DataTable dt = m_Host.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
            string config = string.Empty;
            if (dt.Rows.Count > 0)
            {
                config = dt.Rows[0]["value"].ToString();
            }
            return config;
        }

        public void Print()
        {
            //PrintForm printForm = new PrintForm(info);
            UCMainForm.IsSpecialPrintAgeOpen = true;   //判断是否打印开启

            if (GetConfigValueByKey("SpecialIEMMainPageAgePrint") == "1")
            {
                util = new DrawMainPageUtil(info);
            }
            PrintForm printForm = new PrintForm(util);
            printForm.WindowState = FormWindowState.Maximized;
            printForm.ShowDialog();
            UCMainForm.IsSpecialPrintAgeOpen = false;
        }

        public string CurrentNoofinpat
        {
            get;
            set;
        }
        //是否处于打印状态
        static Boolean _isSpecialPrintAgeOpen = false;
        public static Boolean IsSpecialPrintAgeOpen
        {
            get
            {
                return _isSpecialPrintAgeOpen;
            }
            set
            {
                _isSpecialPrintAgeOpen = value;
            }
        }

        public bool ReadOnlyControl
        {
            get { return m_ReadOnlyControl; }
            set { m_ReadOnlyControl = value; }
        }
        private bool m_ReadOnlyControl = false;
        #endregion

        /// <summary>
        /// 病案室人员编辑首页(未归档)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-05</date>
        /// </summary>
        /// <param name="noofinpat">首页序号</param>
        /// <return></return>
        private void InitFirstPageEditFlag(string noofinpat)
        {
            try
            {
                if (!string.IsNullOrEmpty(m_Host.FloderState) && (FloderState)Enum.Parse(typeof(FloderState), m_Host.FloderState) == FloderState.FirstPage)
                {
                    editFlag = DS_BaseService.CheckIfQuatityControlPerson(DS_Common.currentUser.Id);
                    ///如果是病案人员则继续判断是否为未归档病历
                    if (editFlag)
                    {
                        IsShowBackRecord = DS_BaseService.CheckRecordRebacked(noofinpat);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UCMainForm_Resize(object sender, EventArgs e)
        {
            if (!IsFocusButton())
            {
                ReLocationPicture();
            }
        }

        private void UCMainForm_Click(object sender, EventArgs e)
        {
            int verticalScrollValue = this.VerticalScroll.Value;
            //Point pt = this.AutoScrollPosition;
            simpleButtonTemp.Focus();
            this.VerticalScroll.Value = verticalScrollValue;
            //this.AutoScrollPosition = pt;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!IsFocusButton())
            {
                simpleButtonTemp.Focus();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!IsFocusButton())
            {
                simpleButtonTemp.Focus();
            }
        }

        private void RefreshForm()
        {
            //this.info = pageInfo;
            LoadForm2();
        }

        #region 编辑按钮事件

        /// <summary>
        /// 编辑基本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                //add by cyq 2013-03-04
                if (CheckOutHosCardCheckPassed((int)CurrentInpatient.NoOfFirstPage))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人的出院卡片已在HIS系统中审核通过，不可以修改。");
                    return;
                }

                SetWaitDialogCaption("正在加载数据");
                if (m_UCIemBasInfo == null)
                {
                    m_UCIemBasInfo = new UCIemBasInfo(this);
                }
                //add by cyq 2012-12-05 病案室人员编辑首页(状态改为归档)
                m_UCIemBasInfo.editFlag = IsShowBackRecord;
                ShowUCForm.ShowUCIemBasInfo(m_UCIemBasInfo, info);
                HideWaitDialog();
                ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
                if (ShowUCForm.ShowDialog() == DialogResult.OK)
                {
                    info = ShowUCForm.m_info;
                }
                RefreshForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑诊断信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                //add by cyq 2013-03-04
                if (CheckOutHosCardCheckPassed((int)CurrentInpatient.NoOfFirstPage))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人的出院卡片已在HIS系统中审核通过，不可以修改。");
                    return;
                }

                SetWaitDialogCaption("正在加载数据");
                if (m_UCIemDiagnose == null)
                {
                    m_UCIemDiagnose = new UCIemDiagnose();
                }
                //add by cyq 2012-12-05 病案室人员编辑首页(状态改为归档)
                m_UCIemDiagnose.editFlag = IsShowBackRecord;
                ShowUCForm.ShowUCIemDiagnose(m_UCIemDiagnose, info);
                ShowUCForm.Height = 670;
                ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
                HideWaitDialog();
                if (ShowUCForm.ShowDialog() == DialogResult.OK)
                {
                    info = ShowUCForm.m_info;
                }
                RefreshForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑手术信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                //add by cyq 2013-03-04
                if (CheckOutHosCardCheckPassed((int)CurrentInpatient.NoOfFirstPage))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人的出院卡片已在HIS系统中审核通过，不可以修改。");
                    return;
                }

                SetWaitDialogCaption("正在加载数据");
                if (m_UCIemOperInfo == null)
                {
                    m_UCIemOperInfo = new UCIemOperInfo();
                }
                //add by cyq 2012-12-05 病案室人员编辑首页(状态改为归档)
                m_UCIemOperInfo.editFlag = IsShowBackRecord;
                ShowUCForm.ShowUCIemOperInfo(m_UCIemOperInfo, info);
                ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
                HideWaitDialog();
                if (ShowUCForm.ShowDialog() == DialogResult.OK)
                {
                    info = ShowUCForm.m_info;
                }
                RefreshForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑产妇婴儿信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                //add by cyq 2013-03-04
                if (CheckOutHosCardCheckPassed((int)CurrentInpatient.NoOfFirstPage))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人的出院卡片已在HIS系统中审核通过，不可以修改。");
                    return;
                }

                SetWaitDialogCaption("正在加载数据");
                if (m_UCObstetricsBaby == null)
                {
                    m_UCObstetricsBaby = new UCObstetricsBaby();
                }
                //add by cyq 2012-12-05 病案室人员编辑首页(状态改为归档)
                m_UCObstetricsBaby.editFlag = IsShowBackRecord;
                ShowUCForm.ShowUCObstetricsBaby(m_UCObstetricsBaby, info);
                ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
                HideWaitDialog();
                if (ShowUCForm.ShowDialog() == DialogResult.OK)
                {
                    info = ShowUCForm.m_info;
                }
                RefreshForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑费用信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                //add by cyq 2013-03-04
                if (CheckOutHosCardCheckPassed((int)CurrentInpatient.NoOfFirstPage))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人的出院卡片已在HIS系统中审核通过，不可以修改。");
                    return;
                }

                SetWaitDialogCaption("正在加载数据");
                if (m_UCOthers == null)
                {
                    m_UCOthers = new UCOthers();
                }
                //add by cyq 2012-12-05 病案室人员编辑首页(状态改为归档)
                m_UCOthers.editFlag = IsShowBackRecord;
                ShowUCForm.ShowUCOthers(m_UCOthers, info);
                ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
                HideWaitDialog();
                if (ShowUCForm.ShowDialog() == DialogResult.OK)
                {
                    info = ShowUCForm.m_info;
                }
                RefreshForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region public methods of WaitDialog

        /// <summary>
        /// 界面中调用的等待窗体
        /// </summary>
        private WaitDialogForm m_WaitDialog;
        public void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog == null)
            {
                m_WaitDialog = new WaitDialogForm("正在加载数据", "请稍等...");
            }

            if (!m_WaitDialog.Visible)
                m_WaitDialog.Visible = true;
            m_WaitDialog.Caption = caption;
        }
        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
        #endregion

        /// <summary>
        /// 设置病案首页编辑状态
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-04</date>
        /// </summary>
        /// <param name="flag"></param>
        public void SetButtonsEditState(bool flag)
        {
            try
            {
                simpleButton1.Visible = false;
                simpleButton2.Visible = flag;
                simpleButton3.Visible = flag;
                simpleButton4.Visible = flag;
                simpleButton5.Visible = flag;
                simpleButton6.Visible = flag;

                simpleButton1.Enabled = false;
                simpleButton2.Enabled = flag;
                simpleButton3.Enabled = flag;
                simpleButton4.Enabled = flag;
                simpleButton5.Enabled = flag;
                simpleButton6.Enabled = flag;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查HIS系统中出院卡片是否已审核通过
        /// 注：功能未启用、未配置、配置信息错误也视为未通过
        /// </summary>
        ///  <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-04</date>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public static bool CheckOutHosCardCheckPassed(int noofinpat)
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("AuditOutHosCardInHIS");
                if (string.IsNullOrEmpty(config))
                {
                    return false;
                }
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(config);

                #region 1、出科检查按钮是否显示
                XmlNodeList isOutHosCheckBtnOpenList = doc.GetElementsByTagName("IsOutHosCheckBtnOpen");
                if (null == isOutHosCheckBtnOpenList || isOutHosCheckBtnOpenList.Count == 0)
                {
                    return false;
                }
                if (null != isOutHosCheckBtnOpenList[0].InnerText && isOutHosCheckBtnOpenList[0].InnerText.Trim() != "1")
                {
                    return false;
                }
                #endregion

                #region 2、此功能是否启用
                XmlNodeList isOpenList = doc.GetElementsByTagName("IsOpen");
                if (null == isOpenList || isOpenList.Count == 0)
                {
                    return false;
                }
                if (null != isOpenList[0].InnerText && isOpenList[0].InnerText.Trim() != "1")
                {
                    return false;
                }
                #endregion

                #region 3、出科检查是否已通过
                ///3.1、EMR中病人基本信息表
                XmlNodeList emrBasicInfoTableNameList = doc.GetElementsByTagName("EmrBasicInfoTableName");
                if (null == emrBasicInfoTableNameList || emrBasicInfoTableNameList.Count == 0)
                {
                    return false;
                }
                string emrBasicInfoTableName = null == emrBasicInfoTableNameList[0].InnerText ? string.Empty : emrBasicInfoTableNameList[0].InnerText.Trim();
                if (string.IsNullOrEmpty(emrBasicInfoTableName))
                {
                    return false;
                }
                ///3.2、EMR病人基本信息表中对应的出院检查标识字段
                XmlNodeList emrOutHosCheckFlagFieldNameList = doc.GetElementsByTagName("EmrOutHosCheckFlagFieldName");
                if (null == emrOutHosCheckFlagFieldNameList || emrOutHosCheckFlagFieldNameList.Count == 0)
                {
                    return false;
                }
                string emrOutHosCheckFlagFieldName = null == emrOutHosCheckFlagFieldNameList[0].InnerText ? string.Empty : emrOutHosCheckFlagFieldNameList[0].InnerText.Trim();
                if (string.IsNullOrEmpty(emrOutHosCheckFlagFieldName))
                {
                    return false;
                }
                ///3.3、EMR病人基本信息表中与病人表关联的字段
                XmlNodeList relatedNoofinpatFieldNameList = doc.GetElementsByTagName("RelatedNoofinpatFieldName");
                if (null == relatedNoofinpatFieldNameList || relatedNoofinpatFieldNameList.Count == 0)
                {
                    return false;
                }
                string relatedNoofinpatFieldName = null == relatedNoofinpatFieldNameList[0].InnerText ? string.Empty : relatedNoofinpatFieldNameList[0].InnerText.Trim();
                if (string.IsNullOrEmpty(relatedNoofinpatFieldName))
                {
                    return false;
                }
                ///3.4、EMR病人基本信息表中出院检查标识字段对应的检查通过的值
                XmlNodeList emrOutHosCheckPassedValueList = doc.GetElementsByTagName("EmrOutHosCheckPassedValue");
                if (null == emrOutHosCheckPassedValueList || emrOutHosCheckPassedValueList.Count == 0)
                {
                    return false;
                }
                string emrOutHosCheckPassedValue = null == emrOutHosCheckPassedValueList[0].InnerText ? string.Empty : emrOutHosCheckPassedValueList[0].InnerText.Trim();
                if (string.IsNullOrEmpty(emrOutHosCheckPassedValue))
                {
                    return false;
                }
                string checkFlagValue = DS_SqlService.GetFieldContent(DS_Common.DeleteSpecialCharacter(emrBasicInfoTableName), DS_Common.DeleteSpecialCharacter(emrOutHosCheckFlagFieldName), DS_Common.DeleteSpecialCharacter(relatedNoofinpatFieldName), DS_Common.DeleteSpecialCharacter(noofinpat.ToString()).Trim());
                ///3.5、判断出科检查是否已通过
                if (checkFlagValue != emrOutHosCheckPassedValue)
                {
                    return false;
                }
                #endregion

                #region 4、EMR中病人表与HIS关联的字段
                ///4.1、EMR中病人表与HIS关联的字段
                XmlNodeList emrRelatedFieldNameList = doc.GetElementsByTagName("EmrRelatedFieldName");
                if (null == emrRelatedFieldNameList || emrRelatedFieldNameList.Count == 0)
                {
                    return false;
                }
                string emrRelatedFieldName = null == emrRelatedFieldNameList[0].InnerText ? string.Empty : emrRelatedFieldNameList[0].InnerText.Trim();
                if (string.IsNullOrEmpty(emrRelatedFieldName))
                {
                    return false;
                }
                ///4.2、EMR中病人表与HIS关联的字段的值
                DataTable dt = DS_SqlService.GetInpatientByID(noofinpat, 1);
                emrRelatedFieldName = DS_Common.DeleteSpecialCharacter(emrRelatedFieldName);
                if (null == dt || dt.Rows.Count == 0 || !dt.Columns.Contains(emrRelatedFieldName.ToUpper()))
                {
                    return false;
                }
                string emrRelatedFieldValue = dt.Rows[0][emrRelatedFieldName].ToString();
                if (string.IsNullOrEmpty(emrRelatedFieldValue))
                {
                    return false;
                }
                #endregion

                #region 5、该病人的出院卡片在HIS系统中是否已经审核通过
                ///5.1、获取HIS审核状态表
                XmlNodeList auditTableNameList = doc.GetElementsByTagName("AuditTableName");
                if (null == auditTableNameList || auditTableNameList.Count == 0)
                {
                    return false;
                }
                string auditTableName = null == auditTableNameList[0].InnerText ? string.Empty : auditTableNameList[0].InnerText.Trim();
                if (string.IsNullOrEmpty(auditTableName))
                {
                    return false;
                }
                ///5.2、获取HIS审核状态字段
                XmlNodeList auditFieldNameList = doc.GetElementsByTagName("AuditFieldName");
                if (null == auditFieldNameList || auditFieldNameList.Count == 0)
                {
                    return false;
                }
                string auditFieldName = null == auditFieldNameList[0].InnerText ? string.Empty : auditFieldNameList[0].InnerText.Trim();
                if (string.IsNullOrEmpty(auditFieldName))
                {
                    return false;
                }
                ///5.3、获取HIS审核状态字段对应的审核通过的值(配置项)
                XmlNodeList auditPassedValueList = doc.GetElementsByTagName("AuditPassedValue");
                if (null == auditPassedValueList || auditPassedValueList.Count == 0)
                {
                    return false;
                }
                string auditPassedValue = null == auditPassedValueList[0].InnerText ? string.Empty : auditPassedValueList[0].InnerText.Trim();
                if (string.IsNullOrEmpty(auditPassedValue))
                {
                    return false;
                }
                ///5.4、获取HIS中与EMR病人表关联的字段
                XmlNodeList hisRelatedFieldNameList = doc.GetElementsByTagName("HisRelatedFieldName");
                if (null == hisRelatedFieldNameList || hisRelatedFieldNameList.Count == 0)
                {
                    return false;
                }
                string hisRelatedFieldName = null == hisRelatedFieldNameList[0].InnerText ? string.Empty : hisRelatedFieldNameList[0].InnerText.Trim();
                if (string.IsNullOrEmpty(hisRelatedFieldName))
                {
                    return false;
                }
                using (OracleConnection conn = new OracleConnection(DataAccessFactory.GetSqlDataAccess("HISDB").GetDbConnection().ConnectionString))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    OracleCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.Text;

                    ///5.5、该病人的出院卡片在HIS系统中是否已经审核通过
                    auditTableName = DS_Common.DeleteSpecialCharacter(auditTableName);
                    auditFieldName = DS_Common.DeleteSpecialCharacter(auditFieldName);
                    auditPassedValue = DS_Common.DeleteSpecialCharacter(auditPassedValue);
                    hisRelatedFieldName = DS_Common.DeleteSpecialCharacter(hisRelatedFieldName);
                    cmd.CommandText = " select " + auditFieldName + " from " + auditTableName + " where " + hisRelatedFieldName + " = :emrRelatedFieldValue ";
                    cmd.Parameters.Clear();
                    OracleParameter para = new OracleParameter("emrRelatedFieldValue", OracleType.Char);
                    para.Value = emrRelatedFieldValue;
                    cmd.Parameters.Add(para);
                    object obj = cmd.ExecuteOracleScalar();
                    if (null != obj && obj.ToString().Trim() == auditPassedValue.Trim())
                    {
                        return true;
                    }
                }
                #endregion

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 指定的区域变色
        bool m_IsDrawButton1Region = false;
        private void simpleButton1_MouseMove(object sender, MouseEventArgs e)
        { }

        bool m_IsDrawButton2Region = false;
        private void simpleButton2_MouseEnter(object sender, EventArgs e)
        {
            m_IsDrawButton2Region = true;
            pictureBox1.Refresh();
        }

        private void simpleButton2_MouseLeave(object sender, EventArgs e)
        {
            m_IsDrawButton2Region = false;
            pictureBox1.Refresh();
        }

        bool m_IsDrawButton3Region = false;
        private void simpleButton3_MouseEnter(object sender, EventArgs e)
        {
            m_IsDrawButton3Region = true;
            pictureBox1.Refresh();
        }
        private void simpleButton3_MouseLeave(object sender, EventArgs e)
        {
            m_IsDrawButton3Region = false;
            pictureBox1.Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (m_IsDrawButton2Region)
            {
                Color c = Color.FromArgb(50, 0, 128, 0);//第一个参数是设置alpha通道值，也就是不透明度。100为完全不透明，0为完全透明
                e.Graphics.FillRectangle(new SolidBrush(c),
                    new Rectangle(0, 0, GetPageWidth(), simpleButton2.Height));
            }
            else if (m_IsDrawButton3Region)
            {
                Color c = Color.FromArgb(50, 0, 128, 0);//第一个参数是设置alpha通道值，也就是不透明度。100为完全不透明，0为完全透明
                e.Graphics.FillRectangle(new SolidBrush(c),
                    new Rectangle(0, simpleButton2.Height, GetPageWidth(), simpleButton3.Height));
            }
        }

        bool m_IsDrawButton4Region = false;
        private void simpleButton4_MouseEnter(object sender, EventArgs e)
        {
            m_IsDrawButton4Region = true;
            pictureBox2.Refresh();
        }
        private void simpleButton4_MouseLeave(object sender, EventArgs e)
        {
            m_IsDrawButton4Region = false;
            pictureBox2.Refresh();
        }

        bool m_IsDrawButton5Region = false;
        private void simpleButton5_MouseEnter(object sender, EventArgs e)
        {
            m_IsDrawButton5Region = true;
            pictureBox2.Refresh();
        }
        private void simpleButton5_MouseLeave(object sender, EventArgs e)
        {
            m_IsDrawButton5Region = false;
            pictureBox2.Refresh();
        }

        bool m_IsDrawButton6Region = false;
        private void simpleButton6_MouseEnter(object sender, EventArgs e)
        {
            m_IsDrawButton6Region = true;
            pictureBox2.Refresh();
        }
        private void simpleButton6_MouseLeave(object sender, EventArgs e)
        {
            m_IsDrawButton6Region = false;
            pictureBox2.Refresh();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            if (m_IsDrawButton4Region)
            {
                Color c = Color.FromArgb(50, 0, 128, 0);//第一个参数是设置alpha通道值，也就是不透明度。100为完全不透明，0为完全透明
                e.Graphics.FillRectangle(new SolidBrush(c),
                    new Rectangle(0, 0, GetPageWidth(), simpleButton4.Height));
            }
            else if (m_IsDrawButton5Region)
            {
                Color c = Color.FromArgb(50, 0, 128, 0);//第一个参数是设置alpha通道值，也就是不透明度。100为完全不透明，0为完全透明
                e.Graphics.FillRectangle(new SolidBrush(c),
                    new Rectangle(0, simpleButton4.Height, GetPageWidth(), simpleButton5.Height));
            }
            else if (m_IsDrawButton6Region)
            {
                Color c = Color.FromArgb(50, 0, 128, 0);//第一个参数是设置alpha通道值，也就是不透明度。100为完全不透明，0为完全透明
                if (simpleButton5.Visible)
                {
                    e.Graphics.FillRectangle(new SolidBrush(c),
                        new Rectangle(0, simpleButton4.Height + simpleButton5.Height, GetPageWidth(), simpleButton6.Height));
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(c),
                        new Rectangle(0, simpleButton4.Height, GetPageWidth(), simpleButton6.Height));
                }
            }
        }
        #endregion
    }
}
