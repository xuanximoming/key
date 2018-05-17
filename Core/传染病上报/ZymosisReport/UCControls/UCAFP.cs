using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCAFP : DevExpress.XtraEditors.XtraUserControl, IFuCade
    {
        /// <summary>
        /// add by ck 2013-8
        /// </summary>
        string m_reportID;
        DataRow m_dataRow;
        public UCAFP(string reportID)
        {
            try
            {
                InitializeComponent();
                ucHouseholdscope1.MyEventCheck += new EventHandler(ucHouseholdscope1_MyEventCheck);
                ucHouseholdAddress1.EventWriteAddress += new EventHandler(ucHouseholdAddress1_EventWriteAddress);
                if (!string.IsNullOrEmpty(reportID) && reportID != "0")
                {
                    this.m_reportID = reportID;
                    m_dataRow = GetDataRow(reportID);
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
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
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

        public DataRow GetDataRow(string reportID)
        {
            try
            {
                //数据库中查询出DataRow
                string sql = @"select * from zymosis_afp t where t.vaild='1' and t.reportid=" + reportID;
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
                SqlParameter sqlp1 = new SqlParameter("@AFPID", SqlDbType.VarChar);
                SqlParameter sqlp2 = new SqlParameter("@REPORTID", SqlDbType.Int);
                SqlParameter sqlp3 = new SqlParameter("@HOUSEHOLDSCOPE", SqlDbType.VarChar);
                SqlParameter sqlp4 = new SqlParameter("@HOUSEHOLDADDRESS", SqlDbType.VarChar);
                SqlParameter sqlp5 = new SqlParameter("@ADDRESS", SqlDbType.VarChar);
                SqlParameter sqlp6 = new SqlParameter("@PALSYDATE", SqlDbType.VarChar);
                SqlParameter sqlp7 = new SqlParameter("@PALSYSYMPTOM", SqlDbType.VarChar);

                SqlParameter sqlp8 = new SqlParameter("@VAILD", SqlDbType.VarChar);
                SqlParameter sqlp9 = new SqlParameter("@CREATOR", SqlDbType.VarChar);
                SqlParameter sqlp10 = new SqlParameter("@CREATEDATE", SqlDbType.VarChar);
                SqlParameter sqlp11 = new SqlParameter("@MENDER", SqlDbType.VarChar);
                SqlParameter sqlp12 = new SqlParameter("@ALTERDATE", SqlDbType.VarChar);
                SqlParameter sqlp13 = new SqlParameter("@DIAGNOSISDATE", SqlDbType.VarChar);
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
                #endregion

                if (m_dataRow != null && !string.IsNullOrEmpty(m_dataRow["AFPID"].ToString()))
                {
                    sqlp1.Value = m_dataRow["AFPID"].ToString();
                }
                else
                {
                    sqlp1.Value = Guid.NewGuid().ToString();
                }

                sqlp2.Value = m_reportID;
                sqlp9.Value = DrectSoft.Common.DS_Common.currentUser.DoctorId;
                sqlp10.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sqlp11.Value = DrectSoft.Common.DS_Common.currentUser.DoctorId;
                sqlp12.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                foreach (var item in this.Controls)
                {
                    IZymosisReport iZymosisReport = item as IZymosisReport;
                    XtraUserControl xtraUserControl = item as XtraUserControl;
                    if (iZymosisReport != null && xtraUserControl != null)
                    {
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
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery("EMR_ZYMOSIS_REPORT.usp_AddOrModAFPReport", sps.ToArray(), CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #region IFuCade 成员

        public bool ValideAndSave(string reportId)
        {
            try
            {
                m_reportID = reportId;
                if (CheckData())
                {
                    SaveData();
                    return true;
                    //MyMessageBox.Show("保存成功！");
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

        #endregion

        public Boolean CheckData()
        {
            try
            {
                Control[] controls = new Control[] { 
                    ucPalsyDate1
                };
                foreach (var item in controls)
                {
                    IZymosisReport iZymosisReport = item as IZymosisReport;
                    XtraUserControl xtraUserControl = item as XtraUserControl;
                    if (iZymosisReport != null && xtraUserControl != null)
                    {
                        if (string.IsNullOrEmpty(iZymosisReport.GetValue()))
                        {
                            MyMessageBox.Show(xtraUserControl.AccessibleName + " 不能为空！请填写完整");
                            iZymosisReport.SetFocus();
                            return false;
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
