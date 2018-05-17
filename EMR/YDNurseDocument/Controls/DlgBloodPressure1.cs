using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.NurseDocument.Controls
{
    public partial class DlgBloodPressure1 : DevBaseForm, IDlg
    {
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

        #region 血压录入界面方法

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="val"></param>
        public DlgBloodPressure1(string val)
        {
            try
            {
                InitializeComponent();
                EditValue = val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="val"></param>
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

        #endregion

        #region 血压录入界面事件

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
               MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// Add by xlb 2013-05-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DlgBloodPressure1_Load(object sender, EventArgs e)
        {
            try
            {
                this.textBox1.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}
