using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCContactHistory : DevExpress.XtraEditors.XtraUserControl,IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 接触史控件
        /// </summary>
        public UCContactHistory()
        {
            try
            {
                InitializeComponent();
                //InitValue("1,注射毒品史;11,不详");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        #region IZymosisReport 成员

        public void InitValue(string strValues)
        {
            try
            {
                if (string.IsNullOrEmpty(strValues)) return;
                string[] strValueList = strValues.Split(';');
                foreach (string item in strValueList)
                {
                    string[] str = item.Split(',');
                    foreach (CheckEdit itemChk in this.Controls)
                    {
                        if (itemChk.Tag.ToString() == str[0])
                        {
                            itemChk.Checked = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string GetValue()
        {
            try
            {
                string strValue = "";
                foreach (var item in this.Controls)
                {
                    CheckEdit checkEdit = item as CheckEdit;
                    if (checkEdit == null) continue;
                    if (checkEdit.Checked)
                    {
                        strValue += checkEdit.Tag.ToString() + "," + checkEdit.Text + ";";
                    }
                }
                if (!string.IsNullOrEmpty(strValue) && strValue.Length > 0)
                {
                    strValue = strValue.Substring(0, strValue.Length - 1);
                }
                return strValue;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            chk_dupin.Focus();
        }

        #endregion
    }
}
