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
    public partial class UCPalsyDate : DevExpress.XtraEditors.XtraUserControl,IZymosisReport
    {
        public UCPalsyDate()
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
                //dt_mabi.DateTime = Convert.ToDateTime(strValues);
                dt_mabi.Text = strValues;
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
                //strValue = dt_mabi.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                strValue = dt_mabi.Text;
                return strValue;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            dt_mabi.Focus();
        }

        #endregion
    }
}
