using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCSamplesource : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 样本来源控件
        /// </summary>
        public UCSamplesource()
        {
            try
            {
                InitializeComponent();
                //InitValue("12,有偿供血(浆)人员检测");
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
                foreach (CheckEdit item in this.Controls)
                {
                    if(item.Tag.ToString()==strValueList[0])
                    {
                        item.Checked = true;
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
            chk_shuqian.Focus();
        }

        #endregion
    }
}
