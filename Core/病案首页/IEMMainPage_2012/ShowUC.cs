using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class ShowUC : DevBaseForm
    {

        private IEmrHost m_app;

        public IemMainPageInfo m_info;

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
    }
}