using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.WinForm;
using System.IO;
using DevExpress.XtraTab;
using DevExpress.Utils;
using DrectSoft.Common.Eop;
using System.Xml;
using DrectSoft.Service;
using DrectSoft.Common;
using System.Data.Common;
using DrectSoft.DSSqlHelper;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using DrectSoft.Common.Ctrs.DLG;
using IemMainPageExtension;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class UCMainForm : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        #region var
        IEmrHost m_Host;
        DrawMainPageUtil util;
        IemMainPageManger manger;
        IemMainPageInfo info;
        private WaitDialogForm waitDialogForm;//等待窗体
        SimpleButton btnMouseEnter=null;
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
        private FormExtension m_UCExtension;
        private Inpatient CurrentInpatient;
        //病案室专用
        private bool editFlag = false;
        #endregion
        public string TIP = string.Empty;
        List<Metafile> ma = null;
        public UCMainForm()
        {
            InitializeComponent();
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(141)))), ((int)(((byte)(189)))));
        }

        public UCMainForm(string noofinpat)
            : this()
        {
            CurrentNoofinpat = noofinpat;
            waitDialogForm = new WaitDialogForm("正在加载窗体", "请稍后...");
            waitDialogForm.Hide();
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

            util = new DrawMainPageUtil(info);

            pictureBox1.Width = GetPageWidth();
            pictureBox1.Height = GetPageHeight();
            pictureBox2.Width = GetPageWidth();
            pictureBox2.Height = GetPageHeight();
            ma = util.GetPrintImage();
            pictureBox1.BackgroundImage = ma[0];
            pictureBox2.BackgroundImage = ma[1];

            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;

            ReLocationPicture();

            if (this.Parent != null)
            {
                if (this.Parent.AccessibleName != null
                    && this.Parent.AccessibleName.Trim() != "")//父窗体的AccessibleName不为空则表示没有编辑的权限
                {

                    btnBaseInfo.Visible = false;
                    btnDialogInfo.Visible = false;
                    btnOperInfo.Visible = false;
                    btnFeeInfo.Visible = false;
                    btnExcInfo.Visible = false;
                }
            }

            Employee emp = new Employee(m_Host.User.Id);
            emp.ReInitializeProperties();
            if (emp.Grade.Trim() != "")
            {
                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), emp.Grade);
                if (grade == DoctorGrade.Nurse)
                {
                    btnDialogInfo.Enabled = false;
                    btnOperInfo.Enabled = false;
                    btnFeeInfo.Enabled = false;
                    btnExcInfo.Enabled = false;
                }
            }
            else
            {
                btnBaseInfo.Enabled = false;
                btnDialogInfo.Enabled = false;
                btnOperInfo.Enabled = false;
                btnFeeInfo.Enabled = false;
                btnExcInfo.Enabled = false;
            }
        }

        private void LoadForm2()
        {
            manger = new IemMainPageManger(m_Host, CurrentInpatient);
            util = new DrawMainPageUtil(info);
            ma = util.GetPrintImage();
            pictureBox1.Width = GetPageWidth();
            pictureBox1.Height = GetPageHeight();
            pictureBox2.Width = GetPageWidth();
            pictureBox2.Height = GetPageHeight();
            pictureBox1.BackgroundImage = ma[0];
            pictureBox2.BackgroundImage = ma[1];

            ReLocationPicture();
        }


        /// <summary>
        /// 重新定位PictureBox
        /// </summary>
        private void ReLocationPicture()
        {
            if (util == null) return;

            pictureBox1.Location = new Point((this.Width - pictureBox1.Width + this.btnBaseInfo.Width) / 2 - 8, this.AutoScrollPosition.Y);
            pictureBox2.Location = new Point((this.Width - pictureBox1.Width + this.btnBaseInfo.Width) / 2 - 8, pictureBox1.Location.Y + GetPageHeight() + 20);

            ReLocationEditButton();
        }


        #region 编辑Button


        const float baseInfoHeight = 0.40f;
        const float dialogInfoHeight = 0.60f;
        const float operInfoHeight = 0.40f;
        const float feeInfoHeight = 0.415f;
        const float excInfoHeight = 0.185f;

        /// <summary>
        /// Modify by xlb
        /// 根据扩展表是否有记录来设置是否隐藏按钮
        /// 2013-06-07
        /// </summary>
        private void ReLocationEditButton()
        {

            btnBaseInfo.Height = Convert.ToInt32(baseInfoHeight * GetPageHeight());
            btnBaseInfo.Location = new Point(pictureBox1.Location.X - btnBaseInfo.Width, pictureBox1.Location.Y);

            btnDialogInfo.Height = Convert.ToInt32(dialogInfoHeight * GetPageHeight());
            btnDialogInfo.Location = new Point(pictureBox1.Location.X - btnBaseInfo.Width, pictureBox1.Location.Y + btnBaseInfo.Height);


            btnOperInfo.Height = Convert.ToInt32(operInfoHeight * GetPageHeight());
            btnOperInfo.Location = new Point(pictureBox2.Location.X - btnBaseInfo.Width, pictureBox2.Location.Y);
            if (manger == null)
            {
                manger = new IemMainPageManger(m_Host, CurrentInpatient);
            }
            if (manger.SetExetionButton())/*扩展维护表有记录则显示按钮否则隐藏编辑按钮*/
            {
                btnFeeInfo.Height = Convert.ToInt32(feeInfoHeight * GetPageHeight());
                btnFeeInfo.Location = new Point(pictureBox2.Location.X - btnBaseInfo.Width, pictureBox2.Location.Y + btnOperInfo.Height);
                btnExcInfo.Height = Convert.ToInt32(excInfoHeight * GetPageHeight());
                btnExcInfo.Location = new Point(pictureBox2.Location.X - btnBaseInfo.Width, pictureBox2.Location.Y + btnOperInfo.Height + btnFeeInfo.Height);
            }
            else
            {
                btnExcInfo.Visible = false;
                btnFeeInfo.Height = Convert.ToInt32((feeInfoHeight + excInfoHeight) * GetPageHeight());
                btnFeeInfo.Location = new Point(pictureBox2.Location.X - btnFeeInfo.Width, pictureBox2.Location.Y + btnOperInfo.Height);
            }
        }

        private bool IsFocusButton()
        {
            if (btnBaseInfo.Focused)
            {
                return true;
            }
            if (btnDialogInfo.Focused)
            {
                return true;
            }
            if (btnOperInfo.Focused)
            {
                return true;
            }

            if (btnFeeInfo.Focused)
            {
                return true;
            }
            if (btnExcInfo.Focused)
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
                //InitFirstPageEditFlag(null == CurrentInpatient ? "" : CurrentInpatient.NoOfFirstPage.ToString());
                //if (editFlag)
                //{
                //    SetButtonsEditState(true);
                //}
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


        public void Print()
        {

            UCMainForm.IsSpecialPrintAgeOpen = true;   //判断是否打印开启
            DrectSoft.DrawDriver.DrawOp.PrintView(ma);
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
                editFlag = DS_BaseService.CheckIfQuatityControlPerson(DS_Common.currentUser.Id);
                //如果是病案人员则继续判断是否为未归档病历
                if (editFlag)
                {
                    editFlag = DS_BaseService.CheckRecordRebacked(noofinpat);
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
        private void btnBaseInfo_Click(object sender, EventArgs e)
        {
            
            try
            {
                DrectSoft.Common.DS_Common.SetWaitDialogCaption(waitDialogForm, "正在加载数据");
                if (m_UCIemBasInfo == null)
                {
                    m_UCIemBasInfo = new UCIemBasInfo(this);
                }
                //add by cyq 2012-12-05 病案室人员编辑首页(状态改为归档)
                m_UCIemBasInfo.editFlag = editFlag;
                ShowUCForm.ShowUCIemBasInfo(m_UCIemBasInfo, info);

                DrectSoft.Common.DS_Common.HideWaitDialog(waitDialogForm);

                ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
                if (ShowUCForm.ShowDialog() == DialogResult.OK)
                {
                    info = ShowUCForm.m_info;
                }
                RefreshForm();

            }
            catch (Exception ex)
            {
                DrectSoft.Common.DS_Common.HideWaitDialog(waitDialogForm);
                 MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 编辑诊断信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDialogInfo_Click(object sender, EventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.SetWaitDialogCaption(waitDialogForm, "正在加载数据");
                if (m_UCIemDiagnose == null)
                {
                    m_UCIemDiagnose = new UCIemDiagnose();
                }
                //add by cyq 2012-12-05 病案室人员编辑首页(状态改为归档)
                m_UCIemDiagnose.editFlag = editFlag;
                ShowUCForm.ShowUCIemDiagnose(m_UCIemDiagnose, info);
                ShowUCForm.Height = 670;
                ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
                DrectSoft.Common.DS_Common.HideWaitDialog(waitDialogForm);
                if (ShowUCForm.ShowDialog() == DialogResult.OK)
                {
                    info = ShowUCForm.m_info;
                }
                RefreshForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.DS_Common.HideWaitDialog(waitDialogForm);
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑手术信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOperInfo_Click(object sender, EventArgs e)
        {
            try
            {

                DrectSoft.Common.DS_Common.SetWaitDialogCaption(waitDialogForm, "正在加载数据");
                if (m_UCIemOperInfo == null)
                {
                    m_UCIemOperInfo = new UCIemOperInfo();
                }
                //add by cyq 2012-12-05 病案室人员编辑首页(状态改为归档)
                m_UCIemOperInfo.editFlag = editFlag;
                ShowUCForm.ShowUCIemOperInfo(m_UCIemOperInfo, info);
                ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
                DrectSoft.Common.DS_Common.HideWaitDialog(waitDialogForm);
                if (ShowUCForm.ShowDialog() == DialogResult.OK)
                {
                    info = ShowUCForm.m_info;
                }
                RefreshForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.DS_Common.HideWaitDialog(waitDialogForm);
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑费用信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFeeInfo_Click(object sender, EventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.SetWaitDialogCaption(waitDialogForm, "正在加载数据");
                if (m_UCOthers == null)
                {
                    m_UCOthers = new UCOthers();
                }
                //add by cyq 2012-12-05 病案室人员编辑首页(状态改为归档)
                m_UCOthers.editFlag = editFlag;
                ShowUCForm.ShowUCOthers(m_UCOthers, info);
                ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
                DrectSoft.Common.DS_Common.HideWaitDialog(waitDialogForm);
                if (ShowUCForm.ShowDialog() == DialogResult.OK)
                {
                    info = ShowUCForm.m_info;
                }
                RefreshForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.DS_Common.HideWaitDialog(waitDialogForm);
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 病案扩展维护编辑事件
        /// Add by xlb 2013-04-16
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcInfo_Click(object sender, EventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.SetWaitDialogCaption(waitDialogForm, "正在加载数据");
                if (m_UCExtension == null)
                {
                    m_UCExtension = new FormExtension(CurrentInpatient.NoOfFirstPage.ToString());
                }
                ShowUCForm.ShowUCExtension(CurrentInpatient.NoOfFirstPage.ToString(),m_UCExtension, info);
                ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
                DS_Common.HideWaitDialog(waitDialogForm);
                ShowUCForm.ShowDialog();
                info = ShowUCForm.m_info;
                info.IemMainPageExtension.ExtensionData = m_UCExtension.ExtensionTable;
                RefreshForm();

            }
            catch (Exception ex)
            {
                DS_Common.HideWaitDialog(waitDialogForm);
                MyMessageBox.Show(1, ex);
            }
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

                btnBaseInfo.Visible = flag;
                btnDialogInfo.Visible = flag;
                btnOperInfo.Visible = flag;
                btnFeeInfo.Visible = flag;


                btnBaseInfo.Enabled = flag;
                btnDialogInfo.Enabled = flag;
                btnOperInfo.Enabled = flag;

                btnFeeInfo.Enabled = flag;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 指定的区域变色

     


        private void btnBaseInfo_MouseEnter(object sender, EventArgs e)
        {
            btnMouseEnter = sender as SimpleButton;
            pictureBox1.Refresh();
        }

        private void btnBaseInfo_MouseLeave(object sender, EventArgs e)
        {
            btnMouseEnter = null;
            pictureBox1.Refresh();
        }

       
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
           
                DrawRange(e, btnMouseEnter);
   
        }

        private void DrawRange(PaintEventArgs e,SimpleButton btn)
        {
            if (btn == null) return;
            Color c = Color.FromArgb(50, 0, 128, 0);//第一个参数是设置alpha通道值，也就是不透明度。100为完全不透明，0为完全透明
            int y = 0;
            if (btn == btnBaseInfo)
            {
                y = 0;
            }
            else if(btn==btnDialogInfo)
            {
                y = 0 + btnBaseInfo.Height;
            }
            else if (btn == btnOperInfo)
            {
                y = 0;
            }
            else if (btn == btnFeeInfo)
            {
                y = 0 + btnOperInfo.Height;

            }
            else if (btn == btnExcInfo)
            {
                y = 0 + btnOperInfo.Height+btnFeeInfo.Height;

            }

            e.Graphics.FillRectangle(new SolidBrush(c),
                new Rectangle(0, y, GetPageWidth(), btn.Height));
        }



       
        private void btnPic2_MouseEnter(object sender, EventArgs e)
        {
            btnMouseEnter = sender as SimpleButton;
            pictureBox2.Refresh();
        }
        private void btnPic2_MouseLeave(object sender, EventArgs e)
        {
            btnMouseEnter = null;
            pictureBox2.Refresh();
        }

    

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            DrawRange(e, btnMouseEnter);
        }
        #endregion

    }
}
