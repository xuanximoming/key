using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport
{
    public partial class ReportForms : DevBaseForm
    {

        IEmrHost m_app;

        public ReportForms(IEmrHost _app)
        {
            InitializeComponent();
            m_app = _app;
        }


        private void ReportForms_Load(object sender, EventArgs e)
        {
            UCReportBrowse _UCReportBrowse = new UCReportBrowse(m_app);
            _UCReportBrowse.Dock = DockStyle.Fill;
            this.panelControl1.Controls.Add(_UCReportBrowse);
            this.Text = "传染病报表";
            this.ActiveControl = _UCReportBrowse;
        }


    }
}