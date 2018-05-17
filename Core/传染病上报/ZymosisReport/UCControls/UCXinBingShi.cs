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
    public partial class UCXinBingShi : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        public UCXinBingShi()
        {
            try
            {
                InitializeComponent();
                //InitValue("1,有");
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        #region IZymosisReport 成员


        public void SetFocus()
        {
            chkYou.Focus();
        }

        public void InitValue(string strValues)
        {
            try
            {
                if (string.IsNullOrEmpty(strValues)) return;

                string[] strValueList = strValues.Split(',');
                if (strValueList[0] == chkW.Tag.ToString())
                {
                    chkW.Checked = true;
                }
                else if (strValueList[0] == chKBuXian.Tag.ToString())
                {
                    chKBuXian.Checked = true;
                }
                else if (strValueList[0] == chkYou.Tag.ToString())
                {
                    chkYou.Checked = true;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        string IZymosisReport.GetValue()
        {
            try
            {
             
                string valueStr="";
                if (chkYou.Checked)
                {
                    valueStr = chkYou.Tag.ToString() + "," + chkYou.Text;
                }
                else if (chkW.Checked)
                {
                    valueStr = chkW.Tag.ToString() + "," + chkW.Text;
                    
                }
                else if (chKBuXian.Checked)
                {
                    valueStr = chKBuXian.Tag.ToString() + "," + chKBuXian.Text;
                }
                return valueStr;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion
    }
}
