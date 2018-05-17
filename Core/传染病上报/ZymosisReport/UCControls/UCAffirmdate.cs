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
    public partial class UCAffirmdate : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 确认检测阳性时间控件
        /// </summary>
        public UCAffirmdate()
        {
            InitializeComponent();
            //InitValue("2013/8/9");
        }

        #region IZymosisReport 成员

        public void InitValue(string strValues)
        {
            try
            {
                date_queren.Text = strValues;
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
                return date_queren.Text;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            date_queren.Focus();
        }

        #endregion
    }
}
