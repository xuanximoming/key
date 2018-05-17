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
    public partial class UCLabResult : DevExpress.XtraEditors.XtraUserControl,IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-20 实验室结果
        /// </summary>
        public UCLabResult()
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
                if(string.IsNullOrEmpty(strValues))return;
                chkcb_jieguo.Text = strValues;
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
                strValue = chkcb_jieguo.Text;
                return strValue;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            chkcb_jieguo.Focus();
        }

        #endregion
    }
}
