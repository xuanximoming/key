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
    public partial class UCDetailAdresse : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 详细地址控件
        /// </summary>
        public UCDetailAdresse()
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
                txt_xiangxidizhi.Text = strValues;
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
                return txt_xiangxidizhi.Text;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            txt_xiangxidizhi.Focus();
        }

        #endregion

        public void WriteAddress(string strAddress)
        {
            //edit by ck 2013-8-23
            try
            {
                if (string.IsNullOrEmpty(strAddress))
                {
                    txt_xiangxidizhi.Select();
                }
                else
                {
                    txt_xiangxidizhi.Text = strAddress;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
