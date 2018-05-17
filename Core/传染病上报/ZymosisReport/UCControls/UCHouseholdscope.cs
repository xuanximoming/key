using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using System;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCHouseholdscope : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 户籍所在地控件
        /// </summary>
        public string strvalue = "";
        public event EventHandler MyEventCheck;
        public UCHouseholdscope()
        {
            try
            {
                InitializeComponent();
                //InitValue(null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void chk_benxianqu_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if ((sender as CheckEdit).Checked)
                {
                    txt_hujidi.Text = (sender as CheckEdit).Tag.ToString();
                    strvalue = (sender as CheckEdit).Tag.ToString() + "," + (sender as CheckEdit).Text.Substring(1);
                }
                if (MyEventCheck != null)
                {
                    MyEventCheck(sender, null);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }

        #region IZymosisReport 成员

        public void InitValue(string strValues)
        {
            try
            {
                if (string.IsNullOrEmpty(strValues))
                {
                    strValues = "1,本县区";
                }
                string strTag = strValues.Substring(0, 1);
                foreach (var item in this.Controls)
                {
                    if ((item as CheckEdit).Tag.ToString() == strTag)
                    {
                        (item as CheckEdit).Checked = true;
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
                return strvalue;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SetFocus()
        {
            chk_benxianqu.Focus();
        }

        #endregion
    }
}
