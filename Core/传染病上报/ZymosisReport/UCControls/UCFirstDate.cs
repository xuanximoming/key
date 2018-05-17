using DrectSoft.Common.Ctrs.DLG;
using System;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCFirstDate : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-19 首次出现乙肝症状和体征时间
        /// </summary>
        public UCFirstDate()
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
                string[] strValueList = strValues.Split(',');
                if (strValueList[0] == dt_frist.Tag.ToString())
                {
                    dt_frist.Text = strValueList[1];
                }
                if (strValueList[0] == chk_buxiang.Tag.ToString())
                {
                    chk_buxiang.Checked = true;
                }
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
                if (chk_buxiang.Checked)
                {
                    strValue = chk_buxiang.Tag.ToString() + "," + chk_buxiang.Text;
                }
                else
                {
                    strValue = dt_frist.Tag.ToString() + "," + dt_frist.Text;
                }
                return strValue;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SetFocus()
        {
            dt_frist.Focus();
        }

        #endregion



        private void chk_buxiang_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chk_buxiang.Checked)
                {
                    dt_frist.Enabled = false;
                }
                else
                {
                    dt_frist.Enabled = true;
                }
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }
    }
}
