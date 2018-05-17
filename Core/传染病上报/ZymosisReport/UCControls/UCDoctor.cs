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
    public partial class UCDoctor : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 报告人控件
        /// </summary>
        public UCDoctor()
        {
            try
            {
                InitializeComponent();
                InitValue(null);
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
                if (string.IsNullOrEmpty(strValues))
                {
                    if (DrectSoft.Common.DS_Common.currentUser != null)
                    {
                        strValues = DrectSoft.Common.DS_Common.currentUser.DoctorName;
                    }
                }
                txt_yisheng.Text = strValues;
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
                //txt_yisheng.Text = DrectSoft.Common.DS_Common.currentUser.Name;
                return txt_yisheng.Text;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            txt_yisheng.Focus();
        }

        #endregion
    }
}
