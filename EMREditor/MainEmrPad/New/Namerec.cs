using DrectSoft.Common.Ctrs.DLG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Drectsoft.Core.MainEmrPad.New
{
    public partial class Namerec : Form
    {
        public Namerec()
        {
            InitializeComponent();
        }

        public string CommitTitle
        {
            get { return textEdit_Name.Text; }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textEdit_Name.Text))
                {
                    MyMessageBox.Show("名称不能为空！", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}
