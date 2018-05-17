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
    public partial class UCMaritalStatus : DevExpress.XtraEditors.XtraUserControl,IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 婚姻状况控件
        /// </summary>
        public UCMaritalStatus()
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
                
                if (strValueList[0] == chk_yihun.Tag.ToString())
                {
                    chk_yihun.Checked = true;
                }
                else if (strValueList[0] == chk_weihun.Tag.ToString())
                {
                    chk_weihun.Checked = true;
                }
                else if (strValueList[0] == chk_liyi.Tag.ToString())
                {
                    chk_liyi.Checked = true;
                }
                else if (strValueList[0] == chk_buxiang.Tag.ToString())
                {
                    chk_buxiang.Checked = true;
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
                if(chk_buxiang.Checked)
                {
                    strValue = chk_buxiang.Tag + "," + chk_buxiang.Text;
                }else if(chk_liyi.Checked)
                {
                    strValue = chk_liyi.Tag + "," + chk_liyi.Text;
                }
                else if(chk_weihun.Checked)
                {
                    strValue = chk_weihun.Tag + "," + chk_weihun.Text;
                }
                else if(chk_yihun.Checked)
                {
                    strValue = chk_yihun.Tag + ","+chk_yihun.Text;
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
            chk_weihun.Focus();
        }

        #endregion
    }
}
