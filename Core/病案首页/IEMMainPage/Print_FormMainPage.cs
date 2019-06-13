using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class Print_FormMainPage : DevExpress.XtraEditors.XtraForm
    {
        Print_UCMainPage m_Print_UCMainPage;
        public Print_FormMainPage(IYidanEmrHost app, IemMainPageInfo ieminfo)
        {
            InitializeComponent();
            m_Print_UCMainPage = new Print_UCMainPage(app, ieminfo);
            this.Controls.Add(m_Print_UCMainPage);
            m_Print_UCMainPage.Dock = DockStyle.Fill;
            this.Text = "病案首页打印";
            m_Print_UCMainPage.Location = new Point((this.Width - m_Print_UCMainPage.Width) / 2, 20);
        }
    }
}