using DrectSoft.Common.Ctrs.DLG;
using System;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCIsCure : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        public UCIsCure()
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
                if (strValues.Contains(";"))
                {
                    string[] str1 = strValues.Split(';');
                    if (str1[0].Substring(0, 1) == chk_shi.Tag.ToString())
                    {
                        chk_shi.Checked = true;
                        dt_zhiyu.Text = str1[1].Split(',')[1];
                    }
                }
                else
                {
                    string[] str2 = strValues.Split(',');
                    if (str2[0] == chk_fou.Tag.ToString())
                    {
                        chk_fou.Checked = true;
                    }
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
                if (chk_shi.Checked)
                {
                    strValue = chk_shi.Tag.ToString() + "," + chk_shi.Text + ";" + dt_zhiyu.Tag.ToString() + "," + dt_zhiyu.Text;
                }
                if (chk_fou.Checked)
                {
                    strValue = chk_fou.Tag.ToString() + "," + chk_fou.Text;
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
            chk_shi.Focus();
        }

        #endregion

        private void chk_shi_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dt_zhiyu.Enabled = true;
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        private void chk_fou_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                dt_zhiyu.Enabled = false;
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }
    }
}
