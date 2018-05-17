using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.QcManagerNew
{
    public partial class frmContainer : DevBaseForm
    {
        IEmrHost m_App;
        string m_noofinpat = "";
        string m_patstatus = "";
        public frmContainer(IEmrHost app, string noofinpat, string patstatus)
        {
            InitializeComponent();
            m_App = app;
            m_noofinpat = noofinpat;
            m_patstatus = patstatus;
            frmChildMark childMark = new frmChildMark(this, m_noofinpat, m_App, m_patstatus);
            childMark.Show();
            frmChild child = new frmChild(this, m_App, m_noofinpat);
            child.Show();

        }

        private void frmContainer_Load(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }




    }
}
