using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrectSoft.Core.ZymosisReport.UCControls
{
    public partial class UCH1N1 : DevExpress.XtraEditors.XtraUserControl, IFuCade
    {
        /// <summary>
        /// add by ck 2013-8-20 甲型H1N1流感
        /// </summary>
        string m_reportID;
        DataRow m_dataRow;
        public UCH1N1(string reportID)
        {
            try
            {
                InitializeComponent();
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

        public DataRow GetDataRow(string reportID)
        {
            try
            {
                //数据库中查询出DataRow
                string sql = @"select * from zymosis_h1n1 t where t.vaild='1' and t.reportid=" + reportID;
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

        public void InitData()
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
                SqlParameter sqlp1 = new SqlParameter("@H1N1ID", SqlDbType.VarChar);
                SqlParameter sqlp2 = new SqlParameter("@REPORTID", SqlDbType.Int);
                SqlParameter sqlp3 = new SqlParameter("@CASETYPE", SqlDbType.VarChar);
                SqlParameter sqlp4 = new SqlParameter("@HOSPITALSTATUS", SqlDbType.VarChar);
                SqlParameter sqlp5 = new SqlParameter("@ISCURE", SqlDbType.VarChar);
                SqlParameter sqlp6 = new SqlParameter("@ISOVERSEAS", SqlDbType.VarChar);
                SqlParameter sqlp7 = new SqlParameter("@VAILD", SqlDbType.VarChar);
                SqlParameter sqlp8 = new SqlParameter("@CREATOR", SqlDbType.VarChar);
                SqlParameter sqlp9 = new SqlParameter("@CREATEDATE", SqlDbType.VarChar);
                SqlParameter sqlp10 = new SqlParameter("@MENDER", SqlDbType.VarChar);
                SqlParameter sqlp11 = new SqlParameter("@ALTERDATE", SqlDbType.VarChar);
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
                #endregion

                if (m_dataRow != null && !string.IsNullOrEmpty(m_dataRow["H1N1ID"].ToString()))
                {
                    sqlp1.Value = m_dataRow["H1N1ID"].ToString();
                }
                else
                {
                    sqlp1.Value = Guid.NewGuid().ToString();
                }

                sqlp2.Value = m_reportID;
                sqlp8.Value = DrectSoft.Common.DS_Common.currentUser.DoctorId;
                sqlp9.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                sqlp10.Value = DrectSoft.Common.DS_Common.currentUser.DoctorId;
                sqlp11.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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
                //if (sqlp4.Value.ToString().Length<30)
                //{
                //    MyMessageBox.Show("住院日期 不能为空！请填写完整");
                //    return;
                //}
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery("EMR_ZYMOSIS_REPORT.usp_AddOrModH1N1Report", sps.ToArray(), CommandType.StoredProcedure);
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
                if (CheckData())
                {
                    m_reportID = reportId;
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

        #endregion


        public Boolean CheckData()
        {
            try
            {
                Control[] controls = new Control[] { 
                    ucCaseType1,ucHospitalStatus1,ucIsCure1,ucIsOverseas1
                };
                foreach (var item in controls)
                {
                    IZymosisReport iZymosisReport = item as IZymosisReport;
                    XtraUserControl xtraUserControl = item as XtraUserControl;
                    if (iZymosisReport != null && xtraUserControl != null)
                    {
                        if (xtraUserControl.Tag.ToString() == "HOSPITALSTATUS")
                        {
                            if (iZymosisReport.GetValue().Contains("空"))
                            {
                                MyMessageBox.Show("住院日期 不能为空！请填写完整");
                                return false;
                            }
                        }
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
