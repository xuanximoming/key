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
    public partial class DlgBloodPressuredf1 : DevBaseForm, IDlg
    {
        public DlgBloodPressuredf1(string val)
        {
            InitializeComponent();
            EditValue = val;
        }

        public string EditValue
        {
            get
            {
                if (this.textBox3.Text.Trim() == ""
                    && this.textBox4.Text.Trim() == ""
                    && this.textBox1.Text.Trim() == ""
                    && this.textBox2.Text.Trim() == "")
                {
                    return "";
                }
                else if (this.textBox3.Text.Trim() == "" && this.textBox4.Text.Trim() == "")
                {
                    return this.textBox1.Text.Trim() + "/" + this.textBox2.Text.Trim();
                }
                else if (this.textBox1.Text.Trim() == "" && this.textBox2.Text.Trim() == "")
                {
                    return "|" + this.textBox3.Text.Trim() + "/" + this.textBox4.Text.Trim();
                }
                else
                {
                    return this.textBox1.Text.Trim() + "/" + this.textBox2.Text.Trim() + "|" + this.textBox3.Text.Trim() + "/" + this.textBox4.Text.Trim();
                }
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
                    string[] vals = val.Split('|');
                    if (vals.Length == 2)
                    {
                        string[] items = vals[0].Split('/');
                        if (items.Length == 2)
                        {
                            textBox1.Text = items[0];
                            textBox2.Text = items[1];
                        }
                        string[] items1 = vals[1].Split('/');
                        if (items1.Length == 2)
                        {
                            textBox3.Text = items1[0];
                            textBox4.Text = items1[1];
                        }
                    }
                    else if (vals.Length == 1)
                    {
                        string[] items = vals[0].Split('/');
                        if (items.Length == 2)
                        {
                            textBox1.Text = items[0];
                            textBox2.Text = items[1];
                        }
                    }
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
