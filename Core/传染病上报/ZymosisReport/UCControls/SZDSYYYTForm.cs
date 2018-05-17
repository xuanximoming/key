using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    /// <summary>
    /// xll 20130820
    /// 生殖道沙眼衣原体感染界面
    /// </summary>
    public partial class SZDSYYYTForm : DevExpress.XtraEditors.XtraUserControl, IFuCade
    {
        string m_reportID;
        DataRow m_dataRow;
        public SZDSYYYTForm(string reportID)
        {
            m_reportID = reportID;
            InitializeComponent();
            ucHouseholdscope1.MyEventCheck += new EventHandler(ucHouseholdscope1_MyEventCheck);
            ucHouseholdAddress1.EventWriteAddress += new EventHandler(ucHouseholdAddress1_EventWriteAddress);
            if (!string.IsNullOrEmpty(reportID) && reportID != "0")
            {
                this.m_reportID = reportID;
                m_dataRow = GetDateRow(reportID);
            }

            InitDate();
        }


        public DataRow GetDateRow(string reportID)
        {
            try
            {
                //数据库中查询出DataRow
                string sql = @"select * from zymosis_szdyyt t where t.vaild='1' and  t.report_id=" + reportID;
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql);
                DataRow dr = null;
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                }
                return dr;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void ucHouseholdAddress1_EventWriteAddress(object sender, EventArgs e)
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

        void ucHouseholdscope1_MyEventCheck(object sender, EventArgs e)
        {
            try
            {
                CheckEdit checkEdit = sender as CheckEdit;
                ucHouseholdAddress1.SetEnable(checkEdit.Tag.ToString());
                ucDetailAdresse1.WriteAddress("");
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 根据查出来的数据初始化界面
        /// </summary>
        private void InitDate()
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

                SqlParameter[] parameters = new SqlParameter[22];
                parameters[0] = new SqlParameter("@szdyytid", SqlDbType.VarChar);
                parameters[1] = new SqlParameter("@reportid", SqlDbType.Int);
                parameters[2] = new SqlParameter("@reportno", SqlDbType.VarChar);
                parameters[3] = new SqlParameter("@maritalstatus", SqlDbType.VarChar);
                parameters[4] = new SqlParameter("@nation", SqlDbType.VarChar);
                parameters[5] = new SqlParameter("@culturestate", SqlDbType.VarChar);
                parameters[6] = new SqlParameter("@householdscope", SqlDbType.VarChar);
                parameters[7] = new SqlParameter("@householdaddress", SqlDbType.VarChar);
                parameters[8] = new SqlParameter("@address", SqlDbType.VarChar);
                parameters[9] = new SqlParameter("@syyytgr", SqlDbType.VarChar);
                parameters[10] = new SqlParameter("@contacthistory", SqlDbType.VarChar);
                parameters[11] = new SqlParameter("@venerismhistory", SqlDbType.VarChar);
                parameters[12] = new SqlParameter("@infactway", SqlDbType.VarChar);
                parameters[13] = new SqlParameter("@samplesource", SqlDbType.VarChar);
                parameters[14] = new SqlParameter("@detectionconclusion", SqlDbType.VarChar);
                parameters[15] = new SqlParameter("@affirmdate", SqlDbType.VarChar);
                parameters[16] = new SqlParameter("@affirmlocal", SqlDbType.VarChar);
                parameters[17] = new SqlParameter("@vaild", SqlDbType.VarChar);
                parameters[18] = new SqlParameter("@creator", SqlDbType.VarChar);
                parameters[19] = new SqlParameter("@createdate", SqlDbType.VarChar);
                parameters[20] = new SqlParameter("@mender", SqlDbType.VarChar);
                parameters[21] = new SqlParameter("@alterdate", SqlDbType.VarChar);

                if (m_dataRow != null && !string.IsNullOrEmpty(m_dataRow["SZDYYT_ID"].ToString()))
                {
                    parameters[0].Value = m_dataRow["SZDYYT_ID"].ToString();
                }
                else
                {
                    parameters[0].Value = Guid.NewGuid().ToString();
                }

                parameters[1].Value = m_reportID;
                parameters[18].Value = DrectSoft.Common.DS_Common.currentUser.DoctorId;
                parameters[19].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                parameters[20].Value = DrectSoft.Common.DS_Common.currentUser.DoctorId;
                parameters[21].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                foreach (var item in this.Controls)
                {
                    IZymosisReport iZymosisReport = item as IZymosisReport;
                    XtraUserControl xtraUserControl = item as XtraUserControl;
                    if (iZymosisReport != null && xtraUserControl != null)
                    {
                        string ColName = "@" + xtraUserControl.Tag.ToString().ToLower();
                        foreach (SqlParameter itemsql in parameters)
                        {
                            if (itemsql.ParameterName == ColName)
                            {
                                itemsql.Value = iZymosisReport.GetValue();
                                break;
                            }
                        }
                    }
                }

                foreach (SqlParameter item in parameters)
                {
                    if (item.Value == null)
                    {
                        item.Value = DBNull.Value;
                    }
                }

                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery("EMR_ZYMOSIS_REPORT.usp_AddOrModSYYYTReport", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                throw ex;
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

        private Boolean CheckData()
        {
            return true;
        }

    }
}