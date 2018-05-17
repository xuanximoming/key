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
    public partial class UCCaseType : DevExpress.XtraEditors.XtraUserControl,IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-19 病例分类(根据病情)
        /// </summary>
        public UCCaseType()
        {
            try
            {
                InitializeComponent();
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
                string[] strValueList = strValues.Split(',');
                foreach (var item in this.Controls)
                {
                    CheckEdit checkEdit = item as CheckEdit;
                    if (checkEdit != null && checkEdit.Tag.ToString() == strValueList[0])
                    {
                        checkEdit.Checked = true;
                        break;
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
                        strValue = checkEdit.Tag.ToString() + "," + checkEdit.Text;
                        break;
                    }
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
            chk_qing.Focus();
        }

        #endregion
    }
}
