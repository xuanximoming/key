using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
 

namespace DrectSoft.Core.IEMMainPageZY
{
    public partial class FormMainPage : DevExpress.XtraEditors.XtraForm
    {
        //UCMainPage m_UCMainPage;
        public FormMainPage(IEmrHost app)
        {
            InitializeComponent();
            //m_UCMainPage = new UCMainPage(app);
            //this.Controls.Add(m_UCMainPage);
            //this.Text = "病案首页";
            //m_UCMainPage.Location = new Point((this.Width - m_UCMainPage.Width)/2, 20);
        }
 
    }
}