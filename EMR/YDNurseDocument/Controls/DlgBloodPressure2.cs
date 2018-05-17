using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
namespace DrectSoft.Core.NurseDocument.Controls
{
    public partial class DlgBloodPressure2 : DevBaseForm, IDlg
    {
        public DlgBloodPressure2(string val)
        {
            InitializeComponent();
            EditValue = val;
        }

        public string EditValue
        {
            get
            {
                if (this.textBox1.Text.Trim() == "")
                {
                    return "";
                }
                return textBox1.Text.Trim();
 
            }
            set
            {
                LoadValue(value);
            }
        }

        private void LoadValue(string val)
        {
            try
            {
                if (!string.IsNullOrEmpty(val))
                {
                    textBox1.Text = val;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
