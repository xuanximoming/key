using DrectSoft.Common.Ctrs.DLG;
using System;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCHospitalStatus : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-19 是否住院
        /// </summary>
        public UCHospitalStatus()
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
                        dt_zhuyuan.Text = str1[1].Split(',')[1];
                        dt_chuyuan.Text = str1[2].Split(',')[1];
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
                    string strZhuYuan = string.IsNullOrEmpty(dt_zhuyuan.Text) ? "空" : dt_zhuyuan.Tag.ToString();
                    strValue = chk_shi.Tag.ToString() + "," + chk_shi.Text + ";" + strZhuYuan + "," + dt_zhuyuan.Text + ";" + dt_chuyuan.Tag.ToString() + "," + dt_chuyuan.Text;
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
                dt_zhuyuan.Enabled = true;
                dt_chuyuan.Enabled = true;
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
                dt_zhuyuan.Enabled = false;
                dt_chuyuan.Enabled = false;
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }
    }
}
