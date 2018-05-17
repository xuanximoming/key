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
    public partial class UCALT : DevExpress.XtraEditors.XtraUserControl,IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-19 本次ALT
        /// </summary>
        public UCALT()
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
                txt_alt.Text = strValues;
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
                strValue = txt_alt.Text;
                return strValue;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public void SetFocus()
        {
            txt_alt.Focus();
        }

        #endregion

        private void txt_alt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar != '\b')//这是允许输入退格键
                {
                    if ((e.KeyChar < '0') || (e.KeyChar > '9')||txt_alt.Text.Length>6)//这是允许输入0-9数字
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
