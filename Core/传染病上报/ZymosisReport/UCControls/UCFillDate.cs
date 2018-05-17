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
    public partial class UCFillDate : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 填卡日期控件
        /// </summary>
        public UCFillDate()
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
                if (String.IsNullOrEmpty(strValues))
                {
                    strValues = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                dt_tianka.DateTime = Convert.ToDateTime(strValues);
               
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
                if (dt_tianka.DateTime == null || dt_tianka.DateTime == DateTime.MinValue)
                {
                    return ""; 
                }
                return dt_tianka.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            dt_tianka.Focus();
        }

        #endregion
    }
}
