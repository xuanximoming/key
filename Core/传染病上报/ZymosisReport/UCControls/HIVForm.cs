using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class HIVForm : DevExpress.XtraEditors.XtraUserControl, IFuCade
    {
        string m_reportID;
        DataRow m_dataRow;
        public HIVForm(string reportID)//string reportID
        {
            try
            {
                InitializeComponent();
                ucHouseholdscope1.MyEventCheck += new EventHandler(ucHouseholdscope1_MyEventCheck);
                ucHouseholdAddress1.EventWriteAddress += new EventHandler(ucHouseholdAddress2_EventWriteAddress);
                if (!string.IsNullOrEmpty(reportID) && reportID != "0")
                {
                    this.m_reportID = reportID;
                    m_dataRow = GetDateRow(reportID);
                }

                InitData();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void ucHouseholdscope1_MyEventCheck(object sender, EventArgs e)
        {
            try
            {
                CheckEdit checkEdit = sender as CheckEdit;
                ucHouseholdAddress1.SetEnable(checkEdit.Tag.ToString());
                ucDetailAdresse1.WriteAddress("");
                //ucHouseholdAddress1.InitValue(checkEdit.Tag.ToString());//edit by ck 2013-08-23
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        void ucHouseholdAddress2_EventWriteAddress(object sender, EventArgs e)
        {
            try
            {
                //LookUpEdit lookUpEdit = sender as LookUpEdit;
                //ucDetailAdresse1.WriteAddress(lookUpEdit.Text);
                ucDetailAdresse1.WriteAddress(ucHouseholdAddress1.GetAddress());
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        public DataRow GetDateRow(string reportID)
        {
            try
            {
                //数据库中查询出DataRow
                string sql = @"select * from zymosis_hiv t where t.vaild='1' and t.report_id=" + reportID;
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql);
                DataRow dr = null;
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                }
                //m_dataRow = dr;
                return dr;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 根据查出来的数据初始化界面
        /// </summary>
        private void InitData()
        {
            try
            {
                foreach (var item in this.Controls)
                {
                    IZymosisReport iZymosisReport = item as IZymosisReport;
                    XtraUserControl xtraUserControl = item as XtraUserControl;
                    if (iZymosisReport != null && xtraUserControl != null)
                    {
                        //通过控件tag的名称来匹配数据库字段
                        string ColName = xtraUserControl.Tag.ToString();
                        if (m_dataRow != null)
                        {
                            string ValueStr = m_dataRow[ColName].ToString();
                            iZymosisReport.InitValue(ValueStr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SaveData()
        {
            try
            {
                #region 定义参数
                List<SqlParameter> sps = new List<SqlParameter>();
                SqlParameter sqlp1 = new SqlParameter("@HIVID", SqlDbType.VarChar);
                SqlParameter sqlp2 = new SqlParameter("@REPORTID", SqlDbType.Int);
                SqlParameter sqlp3 = new SqlParameter("@REPORTNO", SqlDbType.VarChar);
                SqlParameter sqlp4 = new SqlParameter("@MARITALSTATUS", SqlDbType.VarChar);
                SqlParameter sqlp5 = new SqlParameter("@NATION", SqlDbType.VarChar);
                SqlParameter sqlp6 = new SqlParameter("@CULTURESTATE", SqlDbType.VarChar);
                SqlParameter sqlp7 = new SqlParameter("@HOUSEHOLDSCOPE", SqlDbType.VarChar);
                SqlParameter sqlp8 = new SqlParameter("@HOUSEHOLDADDRESS", SqlDbType.VarChar);
                SqlParameter sqlp9 = new SqlParameter("@ADDRESS", SqlDbType.VarChar);
                SqlParameter sqlp10 = new SqlParameter("@CONTACTHISTORY", SqlDbType.VarChar);
                SqlParameter sqlp11 = new SqlParameter("@VENERISMHISTORY", SqlDbType.VarChar);
                SqlParameter sqlp12 = new SqlParameter("@INFACTWAY", SqlDbType.VarChar);
                SqlParameter sqlp13 = new SqlParameter("@SAMPLESOURCE", SqlDbType.VarChar);
                SqlParameter sqlp14 = new SqlParameter("@DETECTIONCONCLUSION", SqlDbType.VarChar);
                SqlParameter sqlp15 = new SqlParameter("@AFFIRMDATE", SqlDbType.VarChar);
                SqlParameter sqlp16 = new SqlParameter("@AFFIRMLOCAL", SqlDbType.VarChar);
                SqlParameter sqlp17 = new SqlParameter("@DIAGNOSEDATE", SqlDbType.VarChar);
                SqlParameter sqlp18 = new SqlParameter("@DOCTOR", SqlDbType.VarChar);
                SqlParameter sqlp19 = new SqlParameter("@WRITEDATE", SqlDbType.VarChar);
                SqlParameter sqlp20 = new SqlParameter("@ALIKESYMBOL", SqlDbType.VarChar);
                SqlParameter sqlp21 = new SqlParameter("@REMARK", SqlDbType.VarChar);
                SqlParameter sqlp22 = new SqlParameter("@VAILD", SqlDbType.VarChar);
                SqlParameter sqlp23 = new SqlParameter("@CREATOR", SqlDbType.VarChar);
                SqlParameter sqlp24 = new SqlParameter("@CREATEDATE", SqlDbType.VarChar);
                SqlParameter sqlp25 = new SqlParameter("@MENDER", SqlDbType.VarChar);
                SqlParameter sqlp26 = new SqlParameter("@ALTERDATE", SqlDbType.VarChar);
                sps.Add(sqlp1);
                sps.Add(sqlp2);
                sps.Add(sqlp3);
                sps.Add(sqlp4);
                sps.Add(sqlp5);
                sps.Add(sqlp6);
                sps.Add(sqlp7);
                sps.Add(sqlp8);
                sps.Add(sqlp9);
                sps.Add(sqlp10);
                sps.Add(sqlp11);
                sps.Add(sqlp12);
                sps.Add(sqlp13);
                sps.Add(sqlp14);
                sps.Add(sqlp15);
                sps.Add(sqlp16);
                sps.Add(sqlp17);
                sps.Add(sqlp18);
                sps.Add(sqlp19);
                sps.Add(sqlp20);
                sps.Add(sqlp21);
                sps.Add(sqlp22);
                sps.Add(sqlp23);
                sps.Add(sqlp24);
                sps.Add(sqlp25);
                sps.Add(sqlp26);
                #endregion

                if (m_dataRow != null && !string.IsNullOrEmpty(m_dataRow["HIV_ID"].ToString()))
                {
                    sqlp1.Value = m_dataRow["HIV_ID"].ToString();
                }
                else
                {
                    sqlp1.Value = Guid.NewGuid().ToString();
                }

                sqlp2.Value = m_reportID;
                sqlp23.Value = DrectSoft.Common.DS_Common.currentUser.DoctorId;
                sqlp24.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sqlp25.Value = DrectSoft.Common.DS_Common.currentUser.DoctorId;
                sqlp26.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (var item in this.Controls)
                {
                    IZymosisReport iZymosisReport = item as IZymosisReport;
                    XtraUserControl xtraUserControl = item as XtraUserControl;
                    if (iZymosisReport != null && xtraUserControl != null)
                    {
                        //测试代码
                        if (xtraUserControl.Tag.ToString() == "ADDRESS")
                        {

                        }
                        string ColName = "@" + xtraUserControl.Tag.ToString();
                        foreach (SqlParameter itemsql in sps)
                        {
                            if (itemsql.ParameterName == ColName)
                            {
                                itemsql.Value = iZymosisReport.GetValue();
                                break;
                            }
                        }
                    }
                }
                foreach (var item in sps)
                {

                    if (item.Value == null && item.SqlDbType == SqlDbType.VarChar)
                        item.Value = "";
                }
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery("EMR_ZYMOSIS_REPORT.usp_AddOrModHIVReport", sps.ToArray(), CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ValideAndSave(m_reportID);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public bool ValideAndSave(string reportId)
        {
            try
            {
                m_reportID = reportId;
                if (CheckData())
                {
                    SaveData();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Boolean CheckData()
        {
            try
            {
                Control[] controls = new Control[] { 
                    ucMaritalStatus1,ucNation1,ucEducationLevel1,ucContactHistory1,ucXinBingShi1,
                    ucInfactway1,ucSamplesource1,ucDetectionconclusion1,ucAffirmdate1,
                    ucAffirmDept1
                };
                foreach (var item in controls)
                {
                    IZymosisReport iZymosisReport = item as IZymosisReport;
                    XtraUserControl xtraUserControl = item as XtraUserControl;
                    if (iZymosisReport != null && xtraUserControl != null)
                    {
                        //if (xtraUserControl.Tag.ToString() != "HOUSEHOLDSCOPE"
                        //    && xtraUserControl.Tag.ToString() != "HOUSEHOLDADDRESS" 
                        //    && xtraUserControl.Tag.ToString() != "ADDRESS" 
                        //    && xtraUserControl.Tag.ToString() != "REMARK")
                        //{
                        //测试
                        //if (xtraUserControl.Tag.ToString() == "CONTACTHISTORY") { }
                        //
                        if (string.IsNullOrEmpty(iZymosisReport.GetValue()))
                        {
                            MyMessageBox.Show(xtraUserControl.AccessibleName + " 不能为空！请填写完整");
                            iZymosisReport.SetFocus();
                            return false;
                        }
                        //}

                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // this.Close();
        }

    }
}