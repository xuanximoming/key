using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Emr.TemplateFactory;
using EmrInsert;

namespace EmrInfirce
{
    public partial class UCTemplate : DevExpress.XtraEditors.XtraUserControl
    {
        public UCTemplate()
        {
            InitializeComponent();
        }
        DrectSoft.MainFrame.FormMain _Formain = null;
        DrectSoft.MainFrame.FormMain Formain
        {
            get
            {
                if (_Formain == null)
                {
                    _Formain = new DrectSoft.MainFrame.FormMain(false, "file.menu", true);
                    _Formain.isLG = null;
                }
                return _Formain;
            }
        }
        FormMain frm = null;
        EmrDataHelper emrHelper = null;
        private void UCTemplate_Load(object sender, EventArgs e)
        {
            if (_Formain == null)
            {
                emrHelper = new EmrDataHelper();
                emrHelper.thisLogin();
                _Formain = emrHelper.Formain;
            }
            if (frm == null)
            {
                frm = new FormMain();
                frm.Run(Formain);
                frm.TopLevel = false;
                frm.Parent = this;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                frm.Show();
            }
        }
    }
}
