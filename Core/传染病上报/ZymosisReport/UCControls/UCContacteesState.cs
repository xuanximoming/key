using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using System;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCContacteesState : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 密切接触者有无相同症状控件
        /// </summary>
        public UCContacteesState()
        {
            try
            {
                InitializeComponent();
                //InitValue("1,有");
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
                string str = strValues.Substring(0, 1);
                foreach (var item in this.Controls)
                {
                    CheckEdit checkEdit = item as CheckEdit;
                    if (checkEdit.Tag.ToString() == str)
                    {
                        checkEdit.Checked = true;
                        break;
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
                foreach (var item in this.Controls)
                {
                    CheckEdit checkEdit = item as CheckEdit;
                    if (checkEdit == null) continue;
                    if (checkEdit.Checked)
                    {
                        strValue = checkEdit.Text.Substring(0, 1) + "," + checkEdit.Text.Substring(1);
                        break;
                    }
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
            chk_you.Focus();
        }

        #endregion

        private void chk_wu_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if ((sender as CheckEdit).Checked)
                {
                    txt_youwu.Text = (sender as CheckEdit).Tag.ToString();
                }
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }
    }
}
