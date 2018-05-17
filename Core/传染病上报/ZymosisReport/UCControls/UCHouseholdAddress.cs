using DrectSoft.Common.Ctrs.DLG;
using System;
using System.Data;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCHouseholdAddress : DevExpress.XtraEditors.XtraUserControl, IZymosisReport
    {
        /// <summary>
        /// add by ck 2013-8-15 户籍地址用户控件
        /// </summary>
        public event EventHandler EventWriteAddress;
        public UCHouseholdAddress()
        {
            try
            {
                InitializeComponent();
                InitDate();
                InitValue(null);
            }
            catch (Exception)
            {
            }
        }


        public void InitDate()
        {
            try
            {
                string sql1 = @"select t.npccid,t.npccname from infectnpcc t where t.npccid like '%000000'";
                DataTable dt1 = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql1);
                lookUpEdit_sheng.Properties.ValueMember = "NPCCID";
                lookUpEdit_sheng.Properties.DisplayMember = "NPCCNAME";
                lookUpEdit_sheng.Properties.DataSource = dt1;
                lookUpEdit_sheng.Properties.SearchMode =
                    DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
                //lab_guobiao.Text = lookUpEdit_xian.EditValue.ToString();
                //txt_xiangxidizhi.Text = lookUpEdit_xian.Text;
                //string sql2 = string.Format(@"select t.npccid,t.npccname from infectnpcc t where t.npccid like '{0}%0000' and t.npccid!='{0}000000'",lookUpEdit_sheng.EditValue.ToString().Substring(0,2));
                //DataTable dt2 = DrectSoft.DSSqlHelper.YD_SqlHelper.ExecuteDataTable(sql2);
                //lookUpEdit_shi.Properties.ValueMember = "npccid";
                //lookUpEdit_shi.Properties.DisplayMember = "npccname";
                //lookUpEdit_shi.Properties.DataSource = dt1;
                //lookUpEdit_shi.ItemIndex = 0;
                //edit by ck 2013-8-13
                //lookUpEdit_sheng.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
                //lookUpEdit_sheng.Properties.ImmediatePopup = true;
                //lookUpEdit_sheng.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
                //lookUpEdit_sheng.Properties.AutoSearchColumnIndex = 1;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private string m_strValues;
        private string m_strAddress;

        public string StrAddress
        {
            get { return m_strAddress; }
            set { m_strAddress = value; }
        }
        #region IZymosisReport 成员

        public void InitValue(string strValues)
        {
            try
            {
                if (strValues == null)
                {
                    strValues = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("ZymosisDefaultNation");
                }
                if (strValues == String.Empty)
                {
                    lookUpEdit_sheng.EditValue = "";
                    lookUpEdit_shi.EditValue = "";
                    lookUpEdit_xian.EditValue = "";
                    lookUpEdit_xiang.EditValue = "";
                    return;
                }
                else
                {
                    m_strValues = strValues;
                    lookUpEdit_sheng.EditValue = strValues.Substring(0, 2) + "000000";
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
                return lab_guobiao.Text;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SetFocus()
        {
            lookUpEdit_sheng.Focus();
        }

        #endregion

        private void lookUpEdit_sheng_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lookUpEdit_sheng.EditValue.ToString())) return;
                string sql2 = string.Format(@"select t.npccid,t.npccname from infectnpcc t where t.npccid like '{0}%0000' and t.npccid!='{1}000000'", lookUpEdit_sheng.EditValue.ToString().Substring(0, 2), lookUpEdit_sheng.EditValue.ToString().Substring(0, 2));
                DataTable dt2 = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql2);
                lookUpEdit_shi.Properties.ValueMember = "NPCCID";
                lookUpEdit_shi.Properties.DisplayMember = "NPCCNAME";
                lookUpEdit_shi.Properties.DataSource = dt2;
                lookUpEdit_shi.Properties.SearchMode =
                    DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;

                lookUpEdit_shi.EditValue = m_strValues.Substring(0, 4) + "0000";
                lookUpEdit_xian.EditValue = "";
                if (EventWriteAddress != null)
                {
                    EventWriteAddress(sender, null);
                }

            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        private void lookUpEdit_shi_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lookUpEdit_shi.EditValue.ToString())) return;
                string sql3 = string.Format(@"select t.npccid,t.npccname from infectnpcc t where t.npccid like '{0}%00' and t.npccid!='{1}0000' and length(t.npccid)<=8", lookUpEdit_shi.EditValue.ToString().Substring(0, 4), lookUpEdit_shi.EditValue.ToString().Substring(0, 4));
                DataTable dt3 = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql3);
                lookUpEdit_xian.Properties.ValueMember = "NPCCID";
                lookUpEdit_xian.Properties.DisplayMember = "NPCCNAME";
                lookUpEdit_xian.Properties.DataSource = dt3;
                lookUpEdit_xian.Properties.SearchMode =
                    DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;

                lookUpEdit_xian.EditValue = m_strValues.Substring(0, 6) + "00";
                if (EventWriteAddress != null)
                {
                    EventWriteAddress(sender, null);
                }
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }

        }

        private void lookUpEdit_xian_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lookUpEdit_xian.EditValue.ToString())) return;
                string sql4 = string.Format(@"select t.npccid,t.npccname from infectnpcc t where t.npccid like '{0}%' and t.npccid!='{1}00'", lookUpEdit_xian.EditValue.ToString().Substring(0, 6), lookUpEdit_xian.EditValue.ToString().Substring(0, 6));
                DataTable dt4 = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql4);

                lookUpEdit_xiang.Properties.ValueMember = "NPCCID";
                lookUpEdit_xiang.Properties.DisplayMember = "NPCCNAME";
                lookUpEdit_xiang.Properties.DataSource = dt4;
                lookUpEdit_xiang.Properties.SearchMode =
                    DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;

                lookUpEdit_xiang.EditValue = m_strValues;

                lab_guobiao.Text = lookUpEdit_xian.EditValue.ToString();
                if (EventWriteAddress != null)
                {
                    EventWriteAddress(sender, null);
                }
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        public void SetEnable(string strCheck)
        {
            try
            {
                if (strCheck == "1")
                {
                    lookUpEdit_sheng.Enabled = false;
                    lookUpEdit_shi.Enabled = false;
                    lookUpEdit_xian.Enabled = false;
                    lookUpEdit_xiang.Enabled = true;
                }
                else if (strCheck == "2")
                {
                    lookUpEdit_sheng.Enabled = false;
                    lookUpEdit_shi.Enabled = false;
                    lookUpEdit_xian.Enabled = true;
                    lookUpEdit_xiang.Enabled = true;
                }
                else if (strCheck == "3")
                {
                    lookUpEdit_sheng.Enabled = false;
                    lookUpEdit_shi.Enabled = true;
                    lookUpEdit_xian.Enabled = true;
                    lookUpEdit_xiang.Enabled = true;
                }
                else if (strCheck == "4")
                {
                    lookUpEdit_sheng.Enabled = true;
                    lookUpEdit_shi.Enabled = true;
                    lookUpEdit_xian.Enabled = true;
                    lookUpEdit_xiang.Enabled = true;
                }
                else if (strCheck == "5" || strCheck == "6")
                {
                    lookUpEdit_sheng.Enabled = false;
                    lookUpEdit_shi.Enabled = false;
                    lookUpEdit_xian.Enabled = false;
                    lookUpEdit_xiang.Enabled = false;
                    lab_guobiao.Text = "";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void lookUpEdit_xiang_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lookUpEdit_xiang.EditValue.ToString())) return;
                lab_guobiao.Text = lookUpEdit_xiang.EditValue.ToString();
                if (EventWriteAddress != null)
                {
                    EventWriteAddress(sender, null);
                }
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        public string GetAddress()
        {
            string str = lookUpEdit_sheng.Text + lookUpEdit_shi.Text + lookUpEdit_xian.Text + lookUpEdit_xiang.Text;
            return str;
        }
    }
}
