using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.DailyForm
{
    public partial class DailyFormTitle : DevExpress.XtraEditors.XtraForm
    {
        IEmrHost m_App;
        public DailyFormTitle()
        {
            InitializeComponent();
        }

        public DailyFormTitle(string title, IEmrHost app)
            : this()
        {
            this.textEditTitleName.Text = title;
            this.m_App = app;
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            /*
            if (textEditTitleName.Text.Trim() == "")
            {
                textEditTitleName.Text = "";
                textEditTitleName.Focus();
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
            */

            if (textEditTitleName.Text.Trim() == "")
            {
                textEditTitleName.Text = "   ";
            }
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public string GetTitle()
        {
            return textEditTitleName.Text;
        }
    }
}