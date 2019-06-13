using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.FrameWork.WinForm;
using System.IO;
using DevExpress.XtraTab;
using DevExpress.Utils;
using YidanSoft.Common.Eop;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class UCMainForm : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        #region var
        IYidanEmrHost m_Host;
        DrawMainPageUtil util;
        IemMainPageManger manger;
        IemMainPageInfo info;

        private ShowUC ShowUCForm
        {
            get
            {
                if (_showUcForm == null)
                    _showUcForm = new ShowUC(m_Host);
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

        #endregion

        public UCMainForm()
        {
            InitializeComponent();
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(141)))), ((int)(((byte)(189)))));
        }

        public UCMainForm(string noofinpat)
            :this()
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

        private void LoadForm()
        {
            //manger = new IemMainPageManger(m_Host);
            //info = manger.GetIemInfo();
            DeleteMetaFile();
            util = new DrawMainPageUtil(info);

            pictureBox1.Width = util.m_PageWidth;
            pictureBox1.Height = util.m_PageHeight;
            pictureBox2.Width = util.m_PageWidth;
            pictureBox2.Height = util.m_PageHeight;

            pictureBox1.BackgroundImage = util.MF1;
            pictureBox2.BackgroundImage = util.MF2;

            ReLocationPicture();
            BindParentFormCloseEvent();

            if (this.Parent != null)
            {
                if (this.Parent.AccessibleName.Trim() != "")//父窗体的AccessibleName不为空则表示没有编辑的权限
                {
                    simpleButton1.Visible = false;
                    simpleButton2.Visible = false;
                    simpleButton3.Visible = false;
                    simpleButton4.Visible = false;
                    simpleButton5.Visible = false;
                    simpleButton6.Visible = false;
                }
            }
        }

        private void LoadForm2()
        {
            manger = new IemMainPageManger(m_Host,CurrentInpatient);
            DeleteMetaFile();
            util = new DrawMainPageUtil(info);

            pictureBox1.Width = util.m_PageWidth;
            pictureBox1.Height = util.m_PageHeight;
            pictureBox2.Width = util.m_PageWidth;
            pictureBox2.Height = util.m_PageHeight;

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

            pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, this.AutoScrollPosition.Y);
            pictureBox2.Location = new Point((this.Width - pictureBox1.Width) / 2, pictureBox1.Location.Y + util.m_PageHeight + 20);

            ReLocationEditButton();
        }


        #region 编辑Button

        //const float percentHeight1 = 0.1334f;
        const float percentHeight1 = 0f;

        const float percentHeight2 = 0.2586f + 0.1334f;
        const float percentHeight3 = 0.6081f;

        const float percentHeight4 = 0.1905f;
        const float percentHeight5 = 0.4286f - 0.1905f;
        const float percentHeight6 = 0.3519f;
        private void ReLocationEditButton()
        {
            simpleButton1.Visible = false;
            simpleButton1.Height = Convert.ToInt32(percentHeight1 * util.m_PageHeight);
            simpleButton1.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y);

            simpleButton2.Height = Convert.ToInt32(percentHeight2 * util.m_PageHeight);
            simpleButton2.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y + simpleButton1.Height);

            simpleButton3.Height = Convert.ToInt32(percentHeight3 * util.m_PageHeight);
            simpleButton3.Location = new Point(pictureBox1.Location.X - simpleButton1.Width, pictureBox1.Location.Y + simpleButton1.Height + simpleButton2.Height);

            simpleButton4.Height = Convert.ToInt32(percentHeight4 * util.m_PageHeight);
            simpleButton4.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y);

            if (info.IemObstetricsBaby != null)
            {
                simpleButton5.Visible = true;
                simpleButton5.Height = Convert.ToInt32(percentHeight5 * util.m_PageHeight);
                simpleButton5.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y + simpleButton4.Height);

                simpleButton6.Height = Convert.ToInt32(percentHeight6 * util.m_PageHeight);
                simpleButton6.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y + simpleButton4.Height + simpleButton5.Height);
            }
            else
            {
                simpleButton5.Visible = false;
                simpleButton6.Height = Convert.ToInt32(util.m_PageHeight - simpleButton4.Height);
                simpleButton6.Location = new Point(pictureBox2.Location.X - simpleButton1.Width, pictureBox2.Location.Y + simpleButton4.Height + simpleButton5.Height);
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

        public new void Load(IYidanEmrHost app)
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
            PrintForm printForm = new PrintForm(info);
            printForm.WindowState = FormWindowState.Maximized;
            printForm.ShowDialog();
        }

        public string CurrentNoofinpat
        {
            get;
            set;
        }

        #endregion

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
            SetWaitDialogCaption("正在加载数据");
            if (m_UCIemBasInfo == null)
                m_UCIemBasInfo = new UCIemBasInfo();

            ShowUCForm.ShowUCIemBasInfo(m_UCIemBasInfo, info);
            HideWaitDialog();
            ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
            if (ShowUCForm.ShowDialog() == DialogResult.OK)
            {
                info = ShowUCForm.m_info;
            }

            RefreshForm();
        }

        /// <summary>
        /// 编辑诊断信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            SetWaitDialogCaption("正在加载数据");
            if (m_UCIemDiagnose == null)
                m_UCIemDiagnose = new UCIemDiagnose();


            ShowUCForm.ShowUCIemDiagnose(m_UCIemDiagnose, info);
            ShowUCForm.StartPosition = FormStartPosition.CenterScreen;

            HideWaitDialog();
            if (ShowUCForm.ShowDialog() == DialogResult.OK)
            {
                info = ShowUCForm.m_info;
            }
            RefreshForm();
        }

        /// <summary>
        /// 编辑手术信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            SetWaitDialogCaption("正在加载数据");
            if (m_UCIemOperInfo == null)
                m_UCIemOperInfo = new UCIemOperInfo();
            ShowUCForm.ShowUCIemOperInfo(m_UCIemOperInfo, info);
            ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
            HideWaitDialog();
            if (ShowUCForm.ShowDialog() == DialogResult.OK)
            {
                info = ShowUCForm.m_info;
            }
            RefreshForm();
        }

        /// <summary>
        /// 编辑产妇婴儿信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            SetWaitDialogCaption("正在加载数据");
            if (m_UCObstetricsBaby == null)
                m_UCObstetricsBaby = new UCObstetricsBaby();
            ShowUCForm.ShowUCObstetricsBaby(m_UCObstetricsBaby, info);
            ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
            HideWaitDialog();
            if (ShowUCForm.ShowDialog() == DialogResult.OK)
            {
                info = ShowUCForm.m_info;
            }
            RefreshForm();
        }

        /// <summary>
        /// 编辑费用信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            SetWaitDialogCaption("正在加载数据");
            if (m_UCOthers == null)
                m_UCOthers = new UCOthers();
            ShowUCForm.ShowUCOthers(m_UCOthers, info);
            ShowUCForm.StartPosition = FormStartPosition.CenterScreen;
            HideWaitDialog();
            if (ShowUCForm.ShowDialog() == DialogResult.OK)
            {
                info = ShowUCForm.m_info;
            }
            RefreshForm();
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
                    new Rectangle(0, 0, util.m_PageWidth, simpleButton2.Height));
            }
            else if (m_IsDrawButton3Region)
            {
                Color c = Color.FromArgb(50, 0, 128, 0);//第一个参数是设置alpha通道值，也就是不透明度。100为完全不透明，0为完全透明
                e.Graphics.FillRectangle(new SolidBrush(c),
                    new Rectangle(0, simpleButton2.Height, util.m_PageWidth, simpleButton3.Height));
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
                    new Rectangle(0, 0, util.m_PageWidth, simpleButton4.Height));
            }
            else if (m_IsDrawButton5Region)
            {
                Color c = Color.FromArgb(50, 0, 128, 0);//第一个参数是设置alpha通道值，也就是不透明度。100为完全不透明，0为完全透明
                e.Graphics.FillRectangle(new SolidBrush(c),
                    new Rectangle(0, simpleButton4.Height, util.m_PageWidth, simpleButton5.Height));
            }
            else if (m_IsDrawButton6Region)
            {
                Color c = Color.FromArgb(50, 0, 128, 0);//第一个参数是设置alpha通道值，也就是不透明度。100为完全不透明，0为完全透明
                e.Graphics.FillRectangle(new SolidBrush(c),
                    new Rectangle(0, simpleButton4.Height + simpleButton5.Height, util.m_PageWidth, simpleButton6.Height));
            }
        }
        #endregion
    }
}
