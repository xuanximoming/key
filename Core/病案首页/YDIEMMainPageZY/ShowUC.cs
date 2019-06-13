using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using IemMainPageExtension;
using System;
using System.Windows.Forms;

namespace DrectSoft.Core.IEMMainPageZY
{
    /// <summary>
    /// 容器窗体容纳首页信息编辑用户控件
    /// </summary>
    public partial class ShowUC : Form
    {

        private IEmrHost m_app;
        private FormExtension m_extensionForm;
        public IemMainPageInfo m_info;
        private string nOofinpat;

        public ShowUC(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        /// <summary>
        /// 显示基础信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCIemBasInfo(UCIemBasInfo info, IemMainPageInfo m_pageInfo)
        {
            //this.ResumeLayout();
            //this.Height = info.Height + 10;
            //this.Width = info.Width + 10;
            this.Text = "基础信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);
            //info.Dock = DockStyle.Fill;

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 显示诊断信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCIemDiagnose(UCIemDiagnose info, IemMainPageInfo m_pageInfo)
        {
            //this.ResumeLayout();
            //this.Height = info.Height + 10;
            //this.Width = info.Width + 10;
            this.Text = "诊断信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);
            //info.Dock = DockStyle.Fill;

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 显示手术信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCIemOperInfo(UCIemOperInfo info, IemMainPageInfo m_pageInfo)
        {
            //this.ResumeLayout();
            //this.Height = info.Height + 10;
            //this.Width = info.Width + 10;
            this.Text = "手术信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);
            //info.Dock = DockStyle.Fill;

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 显示产妇婴儿信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCObstetricsBaby(UCObstetricsBaby info, IemMainPageInfo m_pageInfo)
        {
            //this.ResumeLayout();
            //this.Height = info.Height + 10;
            //this.Width = info.Width + 10;
            this.Text = "产妇婴儿信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);
            //info.Dock = DockStyle.Fill;

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 显示其他信息页面
        /// </summary>
        /// <param name="info"></param>
        public void ShowUCOthers(UCOthers info, IemMainPageInfo m_pageInfo)
        {
            //this.ResumeLayout();
            //this.Height = info.Height + 10;
            //this.Width = info.Width + 10;
            this.Text = "其他信息编辑";
            this.Controls.Clear();
            this.Controls.Add(info);
            //info.Dock = DockStyle.Fill;

            info.FillUI(m_pageInfo, m_app);
        }

        /// <summary>
        /// 是否点击确认按钮关闭的
        /// </summary>
        /// <param name="is_OK"></param>
        public void Close(bool is_OK, IemMainPageInfo info)
        {
            if (is_OK)
            {
                m_info = info;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            this.Close();
        }

        private void ShowUC_Load(object sender, EventArgs e)
        {
            if (this.Controls.Count < 1) return;

            this.Height = this.Controls[0].Height + 20;
            this.Width = this.Controls[0].Width + 10;
        }

        /// <summary>
        /// 显示扩展维护界面
        /// Add by xlb 2013-04-16
        /// </summary>
        public void ShowUCExtension(string noofinpat, FormExtension info, IemMainPageInfo m_pageInfo)
        {
            try
            {
                this.Text = "扩展维护信息编辑";
                m_extensionForm = info;
                nOofinpat = noofinpat;
                m_info = m_pageInfo;
                this.Controls.Clear();
                this.Controls.Add(info);
                //info.FillUI();
                info.btnClose.Click += new EventHandler(btnClose_Click);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_extensionForm == null)
                {
                    m_extensionForm = new FormExtension(nOofinpat);
                }
                m_info.IemMainPageExtension.ExtensionData = m_extensionForm.ExtensionTable;
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}