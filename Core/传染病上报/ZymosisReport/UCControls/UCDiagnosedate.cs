using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCDiagnosedate : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 艾滋病确诊日期控件
        /// </summary>
        public UCDiagnosedate()
        {
            try
            {
                InitializeComponent();
                //InitValue("2012/08/09");
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
                date_quezhen.Text = strValues;
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
                return date_quezhen.Text;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            date_quezhen.Focus();
        }

        #endregion
    }
}
