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
    public partial class UCDiagnosisDate : DevExpress.XtraEditors.XtraUserControl,IZymosisReport
    {
        public UCDiagnosisDate()
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
                dt_jiuzhen.Text = strValues;
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
                strValue = dt_jiuzhen.Text;
                return strValue;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            dt_jiuzhen.Focus();
        }

        #endregion
    }
}
