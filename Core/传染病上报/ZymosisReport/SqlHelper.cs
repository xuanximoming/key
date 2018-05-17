using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Data.Common;

namespace DrectSoft.Core.ZymosisReport
{

    /// <summary>
    /// 传染病上报卡状态
    /// </summary>
    public enum ReportState
    {
        /// <summary>
        /// 新增单据
        /// </summary>
        None = 0,

        /// <summary>
        /// 保存
        /// </summary>
        Save = 1,

        /// <summary>
        /// 提交
        /// </summary>
        Submit = 2,

        /// <summary>
        /// 填卡人撤回
        /// </summary>
        Withdraw = 3,

        /// <summary>
        /// 审核通过
        /// </summary>
        Approv = 4,

        /// <summary>
        /// 退回，审核未通过
        /// </summary>
        UnPassApprove = 5,

        /// <summary>
        /// 上报
        /// </summary>
        Report = 6,

        /// <summary>
        /// 作废
        /// </summary>
        Cancel = 7
    }

    public class SqlHelper
    {
        IDataAccess m_IDataAccess;
        public SqlHelper(IDataAccess iDataAccess)
        {
            m_IDataAccess = iDataAccess;
        }

        private string m_SqlGetPatientListForNew = @"select i.name, i.sexid,
                    (select name from dictionary_detail where detailid = i.sexid and categoryid = '3') as gender,
                    i.agestr, i.patid, i.outbed as bedno, i.status,
                    (select d.name from diagnosis d where d.icd = i.admitdiagnosis and d.valid = '1') as diagnosis,
                    i.noofinpat
                    from inpatient i 
                    where (i.admitdept = '{0}' or i.outhosdept = '{0}') --and i.status in ('1501', '1502', '1504', '1505', '1506', '1507')
                    order by i.outbed";

        private string m_SqlGetAllPatientListForNew = @"select i.name, i.sexid,
                    (select name from dictionary_detail where detailid = i.sexid and categoryid = '3') as gender,
                    i.agestr, i.patid, i.outbed as bedno, i.status,
                    (select d.name from diagnosis d where d.icd = i.admitdiagnosis and d.valid = '1') as diagnosis,
                    i.noofinpat
                    from inpatient i 
                    order by i.outbed";

        private string m_SqlDeptList = @"select distinct ID, NAME from department a ,dept2ward b where a.id = b.deptid ";

        /// <summary>
        /// 获得新增病人时弹出的病人列表
        /// </summary>
        /// <param name="deptNO"></param>
        /// <returns></returns>
        public DataTable GetPatientListForNew(string deptNO)
        {
            string sql = string.Format(m_SqlGetPatientListForNew, deptNO);
            DataTable dt = m_IDataAccess.ExecuteDataTable(sql);
            return dt;
        }

        /// <summary>
        /// 获得新增病人时弹出的所有病人的列表
        /// </summary>
        /// <param name="deptNO"></param>
        /// <returns></returns>
        public DataTable GetAllPatientListForNew()
        {
            DataTable dt = m_IDataAccess.ExecuteDataTable(m_SqlGetAllPatientListForNew);
            return dt;
        }

        /// <summary>
        /// 获得到上报卡列表
        /// </summary>
        /// <param name="searchDataEntity"></param>
        /// <returns></returns>
        public DataTable GetCardList(SearchDataEntity searchDataEntity, string querytype)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@report_type1",SqlDbType.VarChar),
                    new SqlParameter("@report_type2",SqlDbType.VarChar),
                    new SqlParameter("@name",SqlDbType.VarChar),
                    new SqlParameter("@patid",SqlDbType.VarChar),
                    new SqlParameter("@deptid",SqlDbType.VarChar),
                    new SqlParameter("@applicant",SqlDbType.VarChar),
                    new SqlParameter("@status",SqlDbType.VarChar),
                    new SqlParameter("@createdatestart",SqlDbType.VarChar),//上报日期开始
                    new SqlParameter("@createdateend",SqlDbType.VarChar),//上报日期结束 
                    new SqlParameter("@querytype",SqlDbType.VarChar)//查询类型（区分时间段查询语句）
                };
            sqlParam[0].Value = searchDataEntity.FirstReport;
            sqlParam[1].Value = searchDataEntity.ModifiedReport;
            sqlParam[2].Value = searchDataEntity.PatientName;
            sqlParam[3].Value = searchDataEntity.PatID;
            sqlParam[4].Value = searchDataEntity.DeptID;
            sqlParam[5].Value = searchDataEntity.Owner;
            sqlParam[6].Value = searchDataEntity.Status;
            sqlParam[7].Value = searchDataEntity.CreateDateStart;//上报日期开始
            sqlParam[8].Value = searchDataEntity.CreateDateEnd;//上报日期结束 
            sqlParam[9].Value = querytype;//查询类型
            return m_IDataAccess.ExecuteDataTable("emr_zymosis_report.usp_geteditzymosisreport", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 获得科室列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetDeptList()
        {
            DataTable dt = m_IDataAccess.ExecuteDataTable(m_SqlDeptList);
            DataRow dr = dt.NewRow();
            dr["ID"] = "";
            dr["Name"] = "全院";
            dt.Rows.InsertAt(dr, 0);
            return dt;
        }

        /// <summary>
        /// 根据传入的ZymosisReportEntity实体返回调用存储过程参数数组
        /// </summary>
        /// <param name="info"></param>
        /// <param name="editType"></param>
        /// <param name="stateName">操作类型  例：新增，</param>
        private SqlParameter[] GetSqlParameter(ZymosisReportEntity info, string editType, string stateName)
        {
            #region
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@Report_ID",SqlDbType.Int),
                    new SqlParameter("@Report_NO",SqlDbType.VarChar),
                    new SqlParameter("@Report_Type",SqlDbType.VarChar),
                    new SqlParameter("@Noofinpat",SqlDbType.VarChar),

                    new SqlParameter("@PatID",SqlDbType.VarChar),
                    new SqlParameter("@Name",SqlDbType.VarChar),
                    new SqlParameter("@ParentName",SqlDbType.VarChar),
                    new SqlParameter("@IDNO",SqlDbType.VarChar),
                    new SqlParameter("@Sex",SqlDbType.VarChar),

                    new SqlParameter("@Birth",SqlDbType.VarChar),
                    new SqlParameter("@Age",SqlDbType.VarChar),
                    new SqlParameter("@AgeUnit",SqlDbType.VarChar),
                    new SqlParameter("@Organization",SqlDbType.VarChar),
                    new SqlParameter("@OfficePlace",SqlDbType.VarChar),
                    new SqlParameter("@OfficeTEL",SqlDbType.VarChar),

                    new SqlParameter("@AddressType",SqlDbType.VarChar),
                    new SqlParameter("@HomeTown",SqlDbType.VarChar),
                    new SqlParameter("@Address",SqlDbType.VarChar),
                    new SqlParameter("@JobID",SqlDbType.VarChar),
                    new SqlParameter("@RecordType1",SqlDbType.VarChar),

                    new SqlParameter("@RecordType2",SqlDbType.VarChar),
                    new SqlParameter("@AttackDate",SqlDbType.VarChar),
                    new SqlParameter("@DiagDate",SqlDbType.VarChar),
                    new SqlParameter("@DieDate",SqlDbType.VarChar),
                    new SqlParameter("@DiagICD10",SqlDbType.VarChar),

                    new SqlParameter("@DiagName",SqlDbType.VarChar),
                    new SqlParameter("@INFECTOTHER_FLAG",SqlDbType.VarChar),
                    new SqlParameter("@Memo",SqlDbType.VarChar),
                    new SqlParameter("@Correct_flag",SqlDbType.VarChar),
                    new SqlParameter("@Correct_Name",SqlDbType.VarChar),

                    new SqlParameter("@Cancel_Reason",SqlDbType.VarChar),
                    new SqlParameter("@ReportDeptCode",SqlDbType.VarChar),
                    new SqlParameter("@ReportDeptName",SqlDbType.VarChar),
                    new SqlParameter("@ReportDocCode",SqlDbType.VarChar),
                    new SqlParameter("@ReportDocName",SqlDbType.VarChar),

                    new SqlParameter("@DoctorTEL",SqlDbType.VarChar),
                    new SqlParameter("@Report_Date",SqlDbType.VarChar),
                    new SqlParameter("@State",SqlDbType.VarChar),
                    new SqlParameter("@StateName",SqlDbType.VarChar),
                    new SqlParameter("@create_date",SqlDbType.VarChar),

                    new SqlParameter("@create_UserCode",SqlDbType.VarChar),
                    new SqlParameter("@create_UserName",SqlDbType.VarChar),
                    new SqlParameter("@create_deptCode",SqlDbType.VarChar),
                    new SqlParameter("@create_deptName",SqlDbType.VarChar),
                    new SqlParameter("@Modify_date",SqlDbType.VarChar),

                    new SqlParameter("@Modify_UserCode",SqlDbType.VarChar),
                    new SqlParameter("@Modify_UserName",SqlDbType.VarChar),
                    new SqlParameter("@Modify_deptCode",SqlDbType.VarChar),
                    new SqlParameter("@Modify_deptName",SqlDbType.VarChar),
                    new SqlParameter("@Audit_date",SqlDbType.VarChar),

                    new SqlParameter("@Audit_UserCode",SqlDbType.VarChar),
                    new SqlParameter("@Audit_UserName",SqlDbType.VarChar),
                    new SqlParameter("@Audit_deptCode",SqlDbType.VarChar),
                    new SqlParameter("@Audit_deptName",SqlDbType.VarChar),
                    new SqlParameter("@OtherDiag",SqlDbType.VarChar)
                };

            sqlParam[0].Value = editType;
            sqlParam[1].Value = info.ReportId;
            sqlParam[2].Value = info.ReportNo;
            sqlParam[3].Value = info.ReportType;
            sqlParam[4].Value = info.Noofinpat;

            sqlParam[5].Value = info.Patid;
            sqlParam[6].Value = info.Name;
            sqlParam[7].Value = info.Parentname;
            sqlParam[8].Value = info.Idno;
            sqlParam[9].Value = info.Sex;

            sqlParam[10].Value = info.Birth;
            sqlParam[11].Value = info.Age;
            sqlParam[12].Value = info.AgeUnit;
            sqlParam[13].Value = info.Organization;
            sqlParam[14].Value = info.Officeplace;

            sqlParam[15].Value = info.Officetel;
            sqlParam[16].Value = info.Addresstype;
            sqlParam[17].Value = info.Hometown;
            sqlParam[18].Value = info.Address;
            sqlParam[19].Value = info.Jobid;

            sqlParam[20].Value = info.Recordtype1;
            sqlParam[21].Value = info.Recordtype2;
            sqlParam[22].Value = info.Attackdate;
            sqlParam[23].Value = info.Diagdate;
            sqlParam[24].Value = info.Diedate;

            sqlParam[25].Value = info.Diagicd10;
            sqlParam[26].Value = info.Diagname;
            sqlParam[27].Value = info.InfectotherFlag;
            sqlParam[28].Value = info.Memo;
            sqlParam[29].Value = info.CorrectFlag;

            sqlParam[30].Value = info.CorrectName;
            sqlParam[31].Value = info.CancelReason;
            sqlParam[32].Value = info.Reportdeptcode;
            sqlParam[33].Value = info.Reportdeptname;
            sqlParam[34].Value = info.Reportdoccode;

            sqlParam[35].Value = info.Reportdocname;
            sqlParam[36].Value = info.Doctortel;
            sqlParam[37].Value = info.ReportDate;
            sqlParam[38].Value = info.State;
            sqlParam[39].Value = stateName;

            sqlParam[40].Value = info.CreateDate;
            sqlParam[41].Value = info.CreateUsercode;
            sqlParam[42].Value = info.CreateUsername;
            sqlParam[43].Value = info.CreateDeptcode;
            sqlParam[44].Value = info.CreateDeptname;

            sqlParam[45].Value = info.ModifyDate;
            sqlParam[46].Value = info.ModifyUsercode;
            sqlParam[47].Value = info.ModifyUsername;
            sqlParam[48].Value = info.ModifyDeptcode;
            sqlParam[49].Value = info.ModifyDeptname;

            sqlParam[50].Value = info.AuditDate;
            sqlParam[51].Value = info.AuditUsercode;
            sqlParam[52].Value = info.AuditUsername;
            sqlParam[53].Value = info.AuditDeptcode;
            sqlParam[54].Value = info.AuditDeptname;
            sqlParam[55].Value = info.OtherDiag;
            return sqlParam;

            #endregion
        }

        /// <summary>
        /// 根据传入的ZymosisReportEntity保存ZymosisReport表信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns>返回新的报告卡ID</returns>
        public string InsertZymosisReport(ZymosisReportEntity info)
        {
            SqlParameter[] sqlParam = GetSqlParameter(info, "1", "新增");

            return m_IDataAccess.ExecuteDataSet("Emr_Zymosis_Report.usp_EditZymosis_Report", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 修改报告卡内容
        /// </summary>
        /// <param name="info">传染病报告卡实体</param>
        /// <param name="stateName">传染病报告卡状态，方便记录流水</param>
        /// <returns></returns>
        public string UpdateZymosisReport(ZymosisReportEntity info, string stateName)
        {
            SqlParameter[] sqlParam = GetSqlParameter(info, "2", stateName);

            return m_IDataAccess.ExecuteDataSet("Emr_Zymosis_Report.usp_EditZymosis_Report", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 作废传染病报告卡
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string CancelZymosisReport(ZymosisReportEntity info)
        {
            SqlParameter[] sqlParam = GetSqlParameter(info, "3", "作废");

            return m_IDataAccess.ExecuteDataSet("Emr_Zymosis_Report.usp_EditZymosis_Report", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 根据传染病报告卡编号获取传染病报告卡实体
        /// </summary>
        /// <param name="zymosisID"></param>
        /// <returns></returns>
        public ZymosisReportEntity GetZymosisReportEntity(string zymosisID)
        {
            ZymosisReportEntity _ZymosisReportEntity = new ZymosisReportEntity();
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@Report_ID",SqlDbType.Int)};

            sqlParam[0].Value = "4";
            sqlParam[1].Value = zymosisID;

            DataTable dt = m_IDataAccess.ExecuteDataTable("Emr_Zymosis_Report.usp_EditZymosis_Report", sqlParam, CommandType.StoredProcedure);

            if (dt.Rows.Count == 0)
                return null;

            foreach (DataRow dr in dt.Rows)
            {
                _ZymosisReportEntity.ReportId = Convert.ToInt32(dr["Report_ID"]);
                _ZymosisReportEntity.ReportNo = dr["Report_NO"].ToString();
                _ZymosisReportEntity.ReportType = dr["Report_Type"].ToString();
                _ZymosisReportEntity.Noofinpat = dr["Noofinpat"].ToString();

                _ZymosisReportEntity.Patid = dr["PatID"].ToString();
                _ZymosisReportEntity.Name = dr["Name"].ToString();
                _ZymosisReportEntity.Parentname = dr["ParentName"].ToString();
                _ZymosisReportEntity.Idno = dr["IDNO"].ToString();
                _ZymosisReportEntity.Sex = dr["Sex"].ToString();

                _ZymosisReportEntity.Birth = dr["Birth"].ToString();
                _ZymosisReportEntity.Age = dr["Age"].ToString();
                _ZymosisReportEntity.AgeUnit = dr["Age_Unit"].ToString();
                _ZymosisReportEntity.Organization = dr["Organization"].ToString();
                _ZymosisReportEntity.Officeplace = dr["OfficePlace"].ToString();

                _ZymosisReportEntity.Officetel = dr["OfficeTel"].ToString();
                _ZymosisReportEntity.Addresstype = dr["AddressType"].ToString();
                _ZymosisReportEntity.Hometown = dr["HomeTown"].ToString();
                _ZymosisReportEntity.Address = dr["Address"].ToString();
                _ZymosisReportEntity.Jobid = dr["JobId"].ToString();

                _ZymosisReportEntity.Recordtype1 = dr["RecordType1"].ToString();
                _ZymosisReportEntity.Recordtype2 = dr["RecordType2"].ToString();
                _ZymosisReportEntity.Attackdate = dr["AttackDate"].ToString();
                _ZymosisReportEntity.Diagdate = dr["DiagDate"].ToString();
                _ZymosisReportEntity.Diedate = dr["DieDate"].ToString();

                _ZymosisReportEntity.Diagicd10 = dr["DiagICD10"].ToString();
                _ZymosisReportEntity.Diagname = dr["DiagName"].ToString();
                _ZymosisReportEntity.InfectotherFlag = dr["infectother_flag"].ToString();
                _ZymosisReportEntity.Memo = dr["Memo"].ToString();
                _ZymosisReportEntity.CorrectFlag = dr["Correct_Flag"].ToString();

                _ZymosisReportEntity.CorrectName = dr["Correct_Name"].ToString();
                _ZymosisReportEntity.CancelReason = dr["Cancel_Reason"].ToString();
                _ZymosisReportEntity.Reportdeptcode = dr["ReportDeptCode"].ToString();
                _ZymosisReportEntity.Reportdeptname = dr["ReportDeptName"].ToString();
                _ZymosisReportEntity.Reportdoccode = dr["ReportDocCode"].ToString();

                _ZymosisReportEntity.Reportdocname = dr["ReportDocName"].ToString();
                _ZymosisReportEntity.Doctortel = dr["DoctorTEL"].ToString();
                _ZymosisReportEntity.ReportDate = dr["Report_Date"].ToString();
                _ZymosisReportEntity.State = dr["State"].ToString();
                _ZymosisReportEntity.CreateDate = dr["create_date"].ToString();

                _ZymosisReportEntity.CreateUsercode = dr["create_UserCode"].ToString();
                _ZymosisReportEntity.CreateUsername = dr["create_UserName"].ToString();
                _ZymosisReportEntity.CreateDeptcode = dr["create_deptCode"].ToString();
                _ZymosisReportEntity.CreateDeptname = dr["create_deptName"].ToString();
                _ZymosisReportEntity.ModifyDate = dr["Modify_date"].ToString();

                _ZymosisReportEntity.ModifyUsercode = dr["Modify_UserCode"].ToString();
                _ZymosisReportEntity.ModifyUsername = dr["Modify_UserName"].ToString();
                _ZymosisReportEntity.ModifyDeptcode = dr["Modify_deptCode"].ToString();
                _ZymosisReportEntity.ModifyDeptname = dr["Modify_deptName"].ToString();
                _ZymosisReportEntity.AuditDate = dr["Audit_date"].ToString();

                _ZymosisReportEntity.AuditUsercode = dr["Audit_UserCode"].ToString();
                _ZymosisReportEntity.AuditUsername = dr["Audit_UserName"].ToString();
                _ZymosisReportEntity.AuditDeptcode = dr["Audit_deptCode"].ToString();
                _ZymosisReportEntity.AuditDeptname = dr["Audit_deptName"].ToString();
                _ZymosisReportEntity.OtherDiag = dr["OtherDiag"].ToString();
            }

            return _ZymosisReportEntity;
        }


        public ZymosisReportEntity GetInpatientByNoofinpat(string noofinpat)
        {
            ZymosisReportEntity _ZymosisReportEntity = new ZymosisReportEntity();
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@Noofinpat",SqlDbType.VarChar)};

            sqlParam[0].Value = noofinpat;


            DataTable dt = m_IDataAccess.ExecuteDataTable("Emr_Zymosis_Report.usp_GetInpatientByNofinpat", sqlParam, CommandType.StoredProcedure);

            if (dt.Rows.Count == 0)
                return null;

            foreach (DataRow dr in dt.Rows)
            {
                _ZymosisReportEntity.Noofinpat = dr["Noofinpat"].ToString();
                _ZymosisReportEntity.Patid = dr["patid"].ToString();
                _ZymosisReportEntity.Name = dr["name"].ToString();
                _ZymosisReportEntity.Idno = dr["idno"].ToString();
                _ZymosisReportEntity.Sex = dr["sexid"].ToString();

                _ZymosisReportEntity.Birth = dr["birth"].ToString();
                _ZymosisReportEntity.Age = dr["age"].ToString();
                _ZymosisReportEntity.AgeUnit = dr["ageUint"].ToString();
                _ZymosisReportEntity.Organization = dr["organization"].ToString();
                _ZymosisReportEntity.Officeplace = dr["officeplace"].ToString();
                _ZymosisReportEntity.Officetel = dr["officetel"].ToString();

                _ZymosisReportEntity.Reportdeptcode = dr["outhosdept"].ToString();
                _ZymosisReportEntity.Reportdoccode = dr["attend"].ToString();
                // _ZymosisReportEntity.Address = dr["address"].ToString();//原来的抓的地址
                _ZymosisReportEntity.Address = dr["nativeaddress"].ToString();//现在改为这个地址
                _ZymosisReportEntity.ReportDate = dr["reportdate"].ToString();
            }
            return _ZymosisReportEntity;
        }

        public DataTable GetReportCardInfo(string reportID)
        {
            string sql = @"SELECT * FROM zymosis_report WHERE zymosis_report.report_id = '{0}'";
            return m_IDataAccess.ExecuteDataTable(string.Format(sql, reportID));
        }

        /// <summary>
        /// 获取传染病报告卡列表
        /// </summary>
        /// <param name="report_type1"></param>
        /// <param name="report_type2"></param>
        /// <param name="recordtype1"></param>
        /// <param name="beginDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="deptid"></param>
        /// <param name="diagICD"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public DataTable GetReportList(string report_type1, string report_type2, string recordtype1, string beginDate, string EndDate, string deptid, string diagICD, string status)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@report_type1",SqlDbType.VarChar),
                    new SqlParameter("@report_type2",SqlDbType.VarChar),
                    new SqlParameter("@recordtype1",SqlDbType.VarChar),
                    new SqlParameter("@beginDate",SqlDbType.VarChar),
                    new SqlParameter("@EndDate",SqlDbType.VarChar),
                    new SqlParameter("@deptid",SqlDbType.VarChar),
                    new SqlParameter("@diagICD",SqlDbType.VarChar),
                    new SqlParameter("@status",SqlDbType.VarChar)
                };


            sqlParam[0].Value = report_type1;
            sqlParam[1].Value = report_type2;
            sqlParam[2].Value = recordtype1;
            sqlParam[3].Value = beginDate;
            sqlParam[4].Value = EndDate;
            sqlParam[5].Value = deptid;
            sqlParam[6].Value = diagICD;
            sqlParam[7].Value = status;
            return m_IDataAccess.ExecuteDataTable("emr_zymosis_report.usp_getReportBrowse", sqlParam, CommandType.StoredProcedure);
        }

        public DataTable GetDisease()
        {
            string sql = @"select zymosis_diagnosis.icd id, zymosis_diagnosis.name 
                             from zymosis_diagnosis 
                            where zymosis_diagnosis.valid = '1' 
                         order by zymosis_diagnosis.level_id";
            return m_IDataAccess.ExecuteDataTable(sql);
        }

        private DataTable m_Job;
        public DataTable GetJob()
        {
            if (m_Job == null)
            {
                string sql = "select jobid, jobname from zymosis_job";
                m_Job = m_IDataAccess.ExecuteDataTable(sql);
            }
            return m_Job;
        }

        private DataTable m_Disease;
        public DataTable Disease
        {
            get
            {
                if (m_Disease == null)
                {
                    m_Disease = GetDisease();
                }
                return m_Disease;
            }
        }

        public DataTable GetReportAnalyse(string timeFrom, string timeTo)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@beginDate",SqlDbType.VarChar),
                    new SqlParameter("@EndDate",SqlDbType.VarChar)
                };
            sqlParam[0].Value = timeFrom;
            sqlParam[1].Value = timeTo;
            DataTable dt = m_IDataAccess.ExecuteDataTable("emr_zymosis_report.usp_GetReportAnalyse", sqlParam, CommandType.StoredProcedure);
            return RemoveZeroForDataTable(dt);
        }

        /// <summary>
        /// 计算横向的总计和纵向的总计
        /// </summary>
        /// <param name="dtDisease"></param>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <returns></returns>
        public DataTable GetJobDisease(DataTable dtDisease, string dateTimeFrom, string dateTimeTo)
        {
            DataTable dtJobDisease = GetJobDiseaseInner2(dtDisease, dateTimeFrom, dateTimeTo);

            dtJobDisease.Columns.Add("SUM_DISEASE");
            dtJobDisease.Columns.Add("SUM_DIE");

            //计算第一列的总计
            foreach (DataRow dr in dtJobDisease.Rows)
            {
                int sum_disease = 0;
                int sum_die = 0;
                foreach (DataColumn dc in dtJobDisease.Columns)
                {
                    if (dc.ColumnName.EndsWith("_发病数")) //发病数
                    {
                        sum_disease += Convert.ToInt32(dr[dc.ColumnName]);
                    }
                    else if (dc.ColumnName.EndsWith("_死亡数")) //死亡数
                    {
                        sum_die += Convert.ToInt32(dr[dc.ColumnName]);
                    }
                }
                dr["sum_disease"] = sum_disease;
                dr["sum_die"] = sum_die;
            }

            //计算第一行的总计
            DataRow drFirst = dtJobDisease.NewRow();
            drFirst["JOBNAME"] = "总计";
            foreach (DataColumn dc in dtJobDisease.Columns)
            {
                if (dc.ColumnName.EndsWith("_发病数") || dc.ColumnName.EndsWith("_死亡数")
                    || dc.ColumnName == "SUM_DISEASE" || dc.ColumnName == "SUM_DIE")
                {
                    int sum = 0;
                    for (int i = 0; i < dtJobDisease.Rows.Count; i++)
                    {
                        DataRow dr = dtJobDisease.Rows[i];
                        sum += Convert.ToInt32(dr[dc.ColumnName]);
                    }
                    drFirst[dc.ColumnName] = sum;
                }
            }
            dtJobDisease.Rows.InsertAt(drFirst, 0);
            //return dtJobDisease;
            return RemoveZeroForDataTable(dtJobDisease);
        }
        /// <summary>
        /// 增加try catch
        /// add by ywk 2013年2月19日10:20:39 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable RemoveZeroForDataTable(DataTable dt)
        {
            try
            {
                DataTable newDataTable = new DataTable();
                foreach (DataColumn dc in dt.Columns)
                {
                    newDataTable.Columns.Add(dc.ColumnName, System.Type.GetType("System.String"));
                }
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow newDr = newDataTable.NewRow();
                    newDr.ItemArray = dr.ItemArray;
                    newDataTable.Rows.Add(newDr);
                }
                return newDataTable;
            }
            catch (Exception)
            {
                
                throw;
            }
          
        }

        /// <summary>
        /// 原始的方法由于查询速度较慢，已停用
        /// </summary>
        /// <param name="dtDisease"></param>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <returns></returns>
        public DataTable GetJobDiseaseInner(DataTable dtDisease, string dateTimeFrom, string dateTimeTo)
        {
            string sql = @" select a.jobid, a.jobname";
            int index = 0;
            for (int i = 0; i < dtDisease.Rows.Count; i++)
            {
                string name = dtDisease.Rows[i]["name"].ToString();
                index++;
                sql += ", count(distinct rep" + index + ".noofinpat) " + name + "_发病数";
                index++;
                sql += ", count(distinct rep" + index + ".noofinpat) " + name + "_死亡数";
            }
            sql += " FROM zymosis_job a ";
            index = 0;
            for (int i = 0; i < dtDisease.Rows.Count; i++)
            {
                string icd = dtDisease.Rows[i]["id"].ToString();
                index++;
                sql += " left join zymosis_report rep" + index +
                    " on rep" + index + ".jobid = a.jobid and rep" + index + ".diagicd10 = '" + icd + "' and rep" + index + ".vaild = '1' and rep" + index + ".attackdate >= '" + dateTimeFrom + "' and rep" + index + ".attackdate <= '" + dateTimeTo + "' and rep" + index + ".state in ('4', '6') ";
                index++;
                sql += " left join zymosis_report rep" + index +
                    " on rep" + index + ".jobid = a.jobid and rep" + index + ".diagicd10 = '" + icd + "' and rep" + index + ".vaild = '1' and rep" + index + ".diedate is not null and rep" + index + ".attackdate >= '" + dateTimeFrom + "' and rep" + index + ".attackdate <= '" + dateTimeTo + "' and rep" + index + ".state in ('4', '6') ";
            }
            sql += " GROUP BY  a.jobid, a.jobname ";
            return m_IDataAccess.ExecuteDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 增加try catch
        /// add by ywk 2013年2月19日10:20:39 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable GetJobDiseaseInner2(DataTable dtDisease, string dateTimeFrom, string dateTimeTo)
        {
            try
            {
                string diagCode = ",";
                foreach (DataRow dr in dtDisease.Rows)
                {
                    diagCode += dr["ID"].ToString() + ",";
                }

                SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@beginDate",SqlDbType.VarChar),
                    new SqlParameter("@EndDate",SqlDbType.VarChar),
                    new SqlParameter("@DiagCode",SqlDbType.VarChar)
                };
                sqlParam[0].Value = dateTimeFrom;
                sqlParam[1].Value = dateTimeTo;
                sqlParam[2].Value = diagCode;
                DataTable dt = m_IDataAccess.ExecuteDataTable("emr_zymosis_report.usp_GetJobDisease", sqlParam, CommandType.StoredProcedure);
                return ReStructDataTable(dt, dtDisease);
            }
            catch (Exception)
            {
                
                throw;
            }
          
        }

        /// <summary>
        /// 重新组合DataTable中的数据
        /// </summary>
        /// edit by ywk 2013年2月19日10:19:02 二次修改 
        /// <param name="dt"></param>
        /// <param name="dtDisease"></param>
        /// <returns></returns>
        private DataTable ReStructDataTable(DataTable dt, DataTable dtDisease)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("JOBID");
                dataTable.Columns.Add("JOBNAME");
                //对于存在重复数据的dtDisease--先把这个数据源重复数据筛选掉 
                ArrayList DiseList = new ArrayList();
                List<string> SplitDiseList = new List<string>();//无重复数据的链表 ywk 2013年2月19日10:19:23
                for (int i = 0; i < dtDisease.Rows.Count; i++)
                {
                    DiseList.Add(dtDisease.Rows[i]["NAME"].ToString());
                }
                for (int i = 0; i < DiseList.Count; i++)
                {
                    if (!SplitDiseList.Contains(DiseList[i].ToString()))
                    {
                        SplitDiseList.Add(DiseList[i].ToString());
                    }
                }
                for (int i = 0; i < SplitDiseList.Count; i++)
                {
                    dataTable.Columns.Add(SplitDiseList[i].ToString() + "_发病数", System.Type.GetType("System.String"));
                    dataTable.Columns.Add(SplitDiseList[i].ToString() + "_死亡数", System.Type.GetType("System.String"));
                }

                for (int i = 0; i < GetJob().Rows.Count; i++)
                {
                    DataRow dr = dataTable.NewRow();

                    for (int j = 0; j < dtDisease.Rows.Count; j++)
                    {
                        if (j == 0)
                        {
                            dr["jobid"] = dt.Rows[i * dtDisease.Rows.Count]["jobid"].ToString();
                            dr["jobname"] = dt.Rows[i * dtDisease.Rows.Count]["jobname"].ToString();
                        }
                        string nameDisease = dt.Rows[i * dtDisease.Rows.Count + j]["diagname"].ToString() + "_发病数";
                        string nameDie = dt.Rows[i * dtDisease.Rows.Count + j]["diagname"].ToString() + "_死亡数";
                        dr[nameDisease] = dt.Rows[i * dtDisease.Rows.Count + j]["diseasecnt"].ToString();
                        dr[nameDie] = dt.Rows[i * dtDisease.Rows.Count + j]["diecnt"].ToString();
                    }

                    dataTable.Rows.Add(dr);
                }
                return dataTable;
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        /// <summary>
        /// 得到所有有效的诊断
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiagnosis()
        {
            SqlParameter[] sqlParam = new SqlParameter[] { };
            DataTable dt = m_IDataAccess.ExecuteDataTable("emr_zymosis_report.usp_GetDiagnosis", sqlParam, CommandType.StoredProcedure);
            return dt;
        }

        /// <summary>
        /// 得到传染病病种信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDisease2()
        {
            SqlParameter[] sqlParam = new SqlParameter[] { };
            DataTable dt = m_IDataAccess.ExecuteDataTable("emr_zymosis_report.usp_GetDisease2", sqlParam, CommandType.StoredProcedure);
            return dt;
        }

        /// <summary>
        /// 得到病种级别，暂时病种级别在程序中写死
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiseaseLevel()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");
            DataRow dr = dt.NewRow();
            dr["ID"] = "1";
            dr["NAME"] = "甲类传染病";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "2";
            dr["NAME"] = "乙类传染病";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "3";
            dr["NAME"] = "丙类传染病";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["ID"] = "10";
            dr["NAME"] = "其他染病";
            dt.Rows.Add(dr);

            return dt;
        }

        public void SaveZymosisDiagnosis(DataRow dr)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            {
                new SqlParameter("@markid",SqlDbType.VarChar),
                new SqlParameter("@icd",SqlDbType.VarChar),
                new SqlParameter("@name",SqlDbType.VarChar),
                new SqlParameter("@py",SqlDbType.VarChar),
                new SqlParameter("@wb",SqlDbType.VarChar),
                new SqlParameter("@levelID",SqlDbType.VarChar),
                new SqlParameter("@valid",SqlDbType.VarChar),
                new SqlParameter("@statist",SqlDbType.VarChar),
                new SqlParameter("@memo",SqlDbType.VarChar),
                new SqlParameter("@namestr",SqlDbType.VarChar),   
                new SqlParameter("@upcount",SqlDbType.Int) ,
                  new SqlParameter("@fukatype",SqlDbType.VarChar)   
            };
            sqlParam[0].Value = dr["markid"].ToString();
            sqlParam[1].Value = dr["icd"].ToString();
            sqlParam[2].Value = dr["name"].ToString();
            sqlParam[3].Value = dr["py"].ToString();
            sqlParam[4].Value = dr["wb"].ToString();
            sqlParam[5].Value = dr.Table.Columns.Contains("level_id") ? dr["level_id"].ToString() : "1";
            sqlParam[6].Value = dr.Table.Columns.Contains("valid") ? dr["valid"].ToString() : "1";
            sqlParam[7].Value = dr["statist"].ToString();
            sqlParam[8].Value = dr["memo"].ToString();
            sqlParam[9].Value = dr["namestr"].ToString();
            sqlParam[10].Value =Convert.ToInt32(dr.Table.Columns.Contains("upcount") ? dr["upcount"].ToString() : "1");// Convert.ToInt32(dr["upcount"].ToString());
            sqlParam[11].Value = dr["fukatype"].ToString();
            m_IDataAccess.ExecuteNoneQuery("emr_zymosis_report.usp_SaveZymosisDiagnosis", sqlParam, CommandType.StoredProcedure);
        }

        public void DeleteZymosisDiagnosis(string icd)
        {
            string sql = "delete from zymosis_diagnosis where icd = '" + icd + "'";
            m_IDataAccess.ExecuteNoneQuery(sql, CommandType.Text);
        }

        /// <summary>
        /// 获取服务器当前时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetSysDate()
        {
            string sql = "select to_char(sysdate,'yyyy-mm-dd') reportdate from dual;";
            string time = m_IDataAccess.ExecuteDataTable(sql, CommandType.Text).Rows[0][0].ToString();
            return Convert.ToDateTime(time);
        }

        internal static ZymosisReportEntity GetZymosisReportEntity(object p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 验证除去当前传染病类，是否还存在符合申报的，诊断
        /// </summary>
        /// <param name="Iem_Mainpage_NO">病案首页号</param>
        /// <param name="Diagicd10">诊断CID10</param>
        /// <param name="inpatientno">患者序列</param>
        /// <returns></returns>
        public Boolean GetRecordValidat(string Iem_Mainpage_NO,List<string> diagicd10list)
        {
            string diagicd10 = string.Empty;
            foreach (string str in diagicd10list)
            {
                diagicd10 += "," + str;
            }          
            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string sqlText = "select ie.iem_mainpage_no,z.icd,z.upcount,z.name from iem_mainpage_diagnosis_2012 ie    left join zymosis_diagnosis z on ie.diagnosis_code=z.icd   left join iem_mainpage_basicinfo_2012 imb on imb.iem_mainpage_no=ie.iem_mainpage_no where  z.valid=1  and ie.valide=1  and imb.valide=1  and  ie.iem_mainpage_no='" + Iem_Mainpage_NO + "'   AND (SELECT COUNT(*) FROM zymosis_report zr where 1=1 and zr.diagicd10=z.icd and zr.Noofinpat=imb.noofinpat and zr.vaild=1)< z.upcount ";

            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sqlText = "select ie.iem_mainpage_no,z.icd,z.upcount,z.name from iem_mainpage_diagnosis_sx ie    left join zymosis_diagnosis z on ie.diagnosis_code=z.icd   left join iem_mainpage_basicinfo_sx imb on imb.iem_mainpage_no=ie.iem_mainpage_no where  z.valid=1  and ie.valide=1  and imb.valide=1  and  ie.iem_mainpage_no='" + Iem_Mainpage_NO + "'  AND (SELECT COUNT(*) FROM zymosis_report zr where 1=1 and zr.diagicd10=z.icd and zr.Noofinpat=imb.noofinpat and zr.vaild=1)< z.upcount ";

            }
            if (diagicd10list.Count > 0)
            {
                sqlText += "AND  Instr('" + diagicd10 + ",',','||z.icd||',',1,1)<=0  ";
            }
            sqlText += "  group by ie.iem_mainpage_no,z.icd,z.upcount,z.name    having count(z.icd)>0 ";

            DataTable table = m_IDataAccess.ExecuteDataTable(sqlText, CommandType.Text);
            if (table != null && table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }      
    }
}
