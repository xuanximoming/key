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
    public partial class UCAffirmDept : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 确认检测单位控件
        /// </summary>
        public UCAffirmDept()
        {
            InitializeComponent();
            //InitValue("人民医院");
        }

        #region IZymosisReport 成员

        public void InitValue(string strValues)
        {
            try
            {
                txt_danwei.Text = strValues;
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
                return txt_danwei.Text;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            txt_danwei.Focus();
        }

        #endregion
    }
}
