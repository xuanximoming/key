using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Core.ReportManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace DrectSoft.Core.DSReportManager
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

        #region 获取字典分类明细库
        /// <summary>
        /// 获取字典分类明细库
        /// </summary>
        /// <param name="lookUp">下拉框控件名</param>
        /// <param name="?">下拉框内容类别</param>
        public void GetDictionarydetail(LookUpEdit lookUp, string Type, string CategoryID, string Pro)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@PatNoOfHis", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = "0";
            sqlParam[2].Value = CategoryID;

            DataTable frmDateTable = m_IDataAccess.ExecuteDataTable(Pro, sqlParam, CommandType.StoredProcedure);

            lookUp.Properties.Columns.Clear();
            lookUp.Properties.DataSource = frmDateTable;
            lookUp.Properties.DisplayMember = "NAME";
            lookUp.Properties.ValueMember = "DetailID".ToUpper();
            lookUp.Properties.ShowHeader = false;
            lookUp.Properties.ShowFooter = false;

            LookUpColumnInfoCollection coll = lookUp.Properties.Columns;
            coll.Add(new LookUpColumnInfo("CategoryID".ToUpper()));
            coll.Add(new LookUpColumnInfo("DetailID".ToUpper()));
            coll.Add(new LookUpColumnInfo("NAME".ToUpper()));
            coll.Add(new LookUpColumnInfo("Py".ToUpper()));
            coll.Add(new LookUpColumnInfo("Wb".ToUpper()));

            lookUp.Properties.Columns["NAME".ToUpper()].Visible = true;
            lookUp.Properties.Columns["CategoryID".ToUpper()].Visible = false;
            lookUp.Properties.Columns["DetailID".ToUpper()].Visible = false;
            lookUp.Properties.Columns["Py".ToUpper()].Visible = false;
            lookUp.Properties.Columns["Wb".ToUpper()].Visible = false;
            lookUp.Properties.BestFit();

            lookUp.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookUp.Properties.AutoSearchColumnIndex = 2 | 3;
        }
        #endregion

        #region 获取地区信息
        /// <summary>
        /// 获取地区信息
        /// </summary>
        /// <param name="lookUp">下拉框控件名</param>
        /// <param name="CategoryID">下拉框内容类别</param>
        public void GetAreas(LookUpEdit lookUp, string Type, string CategoryID)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@PatNoOfHis", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = "0";
            sqlParam[2].Value = CategoryID;

            DataTable frmDateTable = m_IDataAccess.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);

            lookUp.Properties.Columns.Clear();
            lookUp.Properties.DataSource = frmDateTable;
            lookUp.Properties.DisplayMember = "NAME";
            lookUp.Properties.ValueMember = "ID";
            lookUp.Properties.ShowHeader = false;
            lookUp.Properties.ShowFooter = false;

            LookUpColumnInfoCollection coll = lookUp.Properties.Columns;
            coll.Add(new LookUpColumnInfo("ParentID"));
            coll.Add(new LookUpColumnInfo("ID"));
            coll.Add(new LookUpColumnInfo("NAME"));
            coll.Add(new LookUpColumnInfo("Py"));
            coll.Add(new LookUpColumnInfo("Wb"));

            lookUp.Properties.Columns["NAME"].Visible = true;
            lookUp.Properties.Columns["ParentID"].Visible = false;
            lookUp.Properties.Columns["ID"].Visible = false;
            lookUp.Properties.Columns["Py"].Visible = false;
            lookUp.Properties.Columns["Wb"].Visible = false;
            lookUp.Properties.BestFit();

            lookUp.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoComplete;
            lookUp.Properties.AutoSearchColumnIndex = 2 | 3;

        }
        #endregion

        #region 读取病人信息
        /// <summary>
        /// 读取病人信息
        /// </summary>
        /// <param name="PatNoOfHis">首页序号</param>
        public DataTable GetRedactPatientInfoFrm(string Type, string CategoryID, string PatNoOfHis)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@PatNoOfHis", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = Type;
            sqlParam[1].Value = PatNoOfHis;
            sqlParam[2].Value = CategoryID;

            return m_IDataAccess.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);


        }
        #endregion


        /// <summary>
        /// 根据Noofinpat获得病人和他孩子的信息
        /// add ywk
        /// </summary>
        /// <param name="NOOfINPAT"></param>
        /// <returns></returns>
        internal DataTable GetPatAndBaby(string NOOfINPAT)
        {
            string sql = string.Format(@"select a.noofinpat,a.name as patname,a.birth,a.age,a.agestr,a.sexid,
                        b.name as sexname,a.isbaby  from inpatient a    JOIN dictionary_detail b
                        on  b.detailid = a.sexid
                        where b.categoryid = '3' and (a.noofinpat='{0}' or a.mother='{0}') 
                        order by a.isbaby asc,a.birth desc ", NOOfINPAT);
            return m_IDataAccess.ExecuteDataTable(sql, CommandType.Text);
        }


        /// <summary>
        /// 根据病人的首页序号，得到她的婴儿个数，返回显示内容
        /// add by ywk 2012年6月8日 09:47:53
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public string GetPatsBabyContent(string noofinpat)
        {
            string Result = string.Empty;//最终要返回的内容
            string sql = string.Format(@"select babycount,name from inpatient where noofinpat='{0}'", noofinpat);

            DataTable dt = m_IDataAccess.ExecuteDataTable(sql, CommandType.Text);
            if (string.IsNullOrEmpty(dt.Rows[0]["babycount"].ToString()))
            {
                Result = dt.Rows[0]["Name"].ToString();

            }
            else
            {
                if (dt.Rows[0]["babycount"].ToString() == "0")
                {
                    Result = dt.Rows[0]["Name"].ToString();
                }
                else
                {
                    Result = dt.Rows[0]["Name"].ToString() + "【" + dt.Rows[0]["babycount"].ToString() + "个婴儿】";
                }
            }
            return Result;
        }

        /// <summary>
        /// 根据父母首页序列号，返回父母的住院号
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public string GetPatsBabyMother(string noofinpat)
        {
            string Result = string.Empty;//最终要返回的内容
            string sql = string.Format(@"select patid,name from inpatient where noofinpat='{0}'", noofinpat);

            DataTable dt = m_IDataAccess.ExecuteDataTable(sql, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                Result = dt.Rows[0]["patid"].ToString();
            }
            return Result;
        }

        /// <summary>
        /// /根据病人首页序号获得她孩子的个数
        /// add ywk
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public bool HasBaby(string noofinpat)
        {
            string sql = string.Format(@"select babycount from inpatient where noofinpat='{0}'", noofinpat);
            DataTable dt = m_IDataAccess.ExecuteDataTable(sql, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["babycount"].ToString() == "0")
                {
                    return false;
                }
                if (Int32.Parse(dt.Rows[0]["babycount"].ToString() == "" ?
                    "0" : dt.Rows[0]["babycount"].ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
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
        /// 得到所有有效的诊断
        /// </summary>
        /// <returns></returns>
        public DataTable GetDiagnosisTo(string categoryid, string type)
        {
            SqlParameter[] sqlParam = new SqlParameter[] { new SqlParameter("@categoryid", SqlDbType.VarChar) };
            sqlParam[0].Value = categoryid;
            DataTable dt = null;
            if (type == "zy")
            {
                dt = m_IDataAccess.ExecuteDataTable("emr_zymosis_report.usp_GetDiagnosisTo_ZY", sqlParam, CommandType.StoredProcedure);

            }
            else
            {
                string diagsource = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("GetDiagnosisType");
                if (diagsource == "1")//中心醫院的取自HIS上的視圖yd_diagnosis
                {
                    try
                    {
                        //配置為1的話，從HIS取出來後，還要剔除現在診斷病種表已經維護進去的一些診斷編碼
                        DataTable dt_zydiag = new DataTable();//存放現有zymosis_diagnosis中已經存進去的
                        dt_zydiag = GetZYDiagnosisBYCate(categoryid);
                        using (OracleConnection conn = new OracleConnection(DataAccessFactory.GetSqlDataAccess("HISDB").GetDbConnection().ConnectionString))
                        {
                            if (conn.State != ConnectionState.Open)
                            {
                                conn.Open();
                            }
                            dt = new DataTable();
                            OracleCommand cmd = conn.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = " SELECT    NAME, py , WB, memo,  ICD  FROM yd_diagnosis ";
                            OracleDataAdapter myoadapt = new OracleDataAdapter(cmd.CommandText, conn);
                            myoadapt.Fill(dt);
                        }
                        //string sk = "SELECT  MarkId as  ID,  NAME, py , WB, memo,  icd  FROM diagnosis2";
                        //dt = m_IDataAccess.ExecuteDataTable(sk, CommandType.Text);
                        dt.Merge(dt_zydiag);
                        dt = dt.DefaultView.ToTable(true, new string[] { "ICD", "NAME", "PY", "WB", "MEMO" });

                    }
                    catch (Exception ex)
                    {
                        dt = m_IDataAccess.ExecuteDataTable("emr_zymosis_report.usp_GetDiagnosisTo", sqlParam, CommandType.StoredProcedure);
                        MessageBox.Show("诊断取自HIS出錯，信息為：" + ex.Message);
                    }

                }
                else
                {
                    dt = m_IDataAccess.ExecuteDataTable("emr_zymosis_report.usp_GetDiagnosisTo", sqlParam, CommandType.StoredProcedure);
                }
            }
            return dt;
        }

        /// <summary>
        ///  獲得當前報告卡對應診斷表中的各类别的诊断编码集合
        /// </summary>MarkId as  ID,  NAME, py , WB, memo,  icd 
        /// <param name="categoryid"></param>
        /// <returns></returns>
        private DataTable GetZYDiagnosisBYCate(string categoryid)
        {
            try
            {
                DataTable dt = null;
                string sqlseach = string.Format(@" select  NAME, py , WB, memo,  ICD  from  ZYMOSIS_DIAGNOSIS t
 where t.categoryid='{0}' and valid=1; ", categoryid);
                dt = m_IDataAccess.ExecuteDataTable(sqlseach, CommandType.Text);
                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 得到传染病病种信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetDisease2To(string categoryid)
        {
            SqlParameter[] sqlParam = new SqlParameter[] { new SqlParameter("@categoryid", SqlDbType.VarChar) };
            sqlParam[0].Value = categoryid;
            DataTable dt = m_IDataAccess.ExecuteDataTable("emr_zymosis_report.usp_GetDisease2To", sqlParam, CommandType.StoredProcedure);
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
        public void SaveZymosisDiagnosisAdd(DataRow dr, string categoryid)
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
                new SqlParameter("@upcount",SqlDbType.Int),
                 new SqlParameter("@categoryid",SqlDbType.Int)
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
            sqlParam[10].Value = Convert.ToInt32(dr.Table.Columns.Contains("upcount") ? dr["upcount"].ToString() : "1");
            sqlParam[11].Value = Convert.ToInt32(categoryid);
            // Convert.ToInt32(dr["upcount"].ToString());
            m_IDataAccess.ExecuteNoneQuery("emr_zymosis_report.usp_SaveZymosisDiagnosisTo", sqlParam, CommandType.StoredProcedure);
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
                new SqlParameter("@upcount",SqlDbType.Int),
                 new SqlParameter("@categoryid",SqlDbType.Int)
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
            sqlParam[10].Value = Convert.ToInt32(dr.Table.Columns.Contains("upcount") ? dr["upcount"].ToString() : "1");
            sqlParam[11].Value = Convert.ToInt32(dr["categoryid"].ToString());
            // Convert.ToInt32(dr["upcount"].ToString());
            m_IDataAccess.ExecuteNoneQuery("emr_zymosis_report.usp_SaveZymosisDiagnosisTo", sqlParam, CommandType.StoredProcedure);
        }

        public void DeleteZymosisDiagnosis(string icd, string categoryid)
        {
            string sql = "delete from zymosis_diagnosis where icd = '" + icd + "' and categoryid=" + Convert.ToInt32(categoryid) + "";
            m_IDataAccess.ExecuteNoneQuery(sql, CommandType.Text);
        }

        // <summary>
        /// 获得新增病人时弹出的病人列表
        /// </summary>
        /// <param name="deptNO"></param>
        /// <returns></returns>
        public DataTable GetPatientListForNew(string deptNO)
        {
            string m_SqlGetPatientListForNew = @"select i.name, i.sexid,
                    (select name from dictionary_detail where detailid = i.sexid and categoryid = '3') as gender,
                    i.agestr, i.patid, i.outbed as bedno, i.status,
                    (select d.name from diagnosis d where d.icd = i.admitdiagnosis and d.valid = '1') as diagnosis,
                    i.noofinpat
                    from inpatient i 
                    where (i.admitdept = '{0}' or i.outhosdept = '{0}') --and i.status in ('1501', '1502', '1504', '1505', '1506', '1507')
                    
                    order by i.outbed";
            string sql = string.Format(m_SqlGetPatientListForNew, deptNO);
            DataTable dt = m_IDataAccess.ExecuteDataTable(sql);
            return dt;
        }

        // <summary>
        /// 获得新增病人时弹出的病人列表
        /// </summary>
        /// <param name="deptNO"></param>
        /// <returns></returns>
        public DataTable GetPatientListForNewTo(string deptNO)
        {
            //            string m_SqlGetPatientListForNew = @"select i.name, i.sexid,
            //                    (select name from dictionary_detail where detailid = i.sexid and categoryid = '3') as gender,
            //                    i.agestr, i.patid, i.outbed as bedno, i.status,
            //                    (select d.name from diagnosis d where d.icd = i.admitdiagnosis and d.valid = '1') as diagnosis,
            //                    i.noofinpat
            //                    from inpatient i 
            //                    where (i.admitdept = '{0}' or i.outhosdept = '{0}') 
            //                    AND exists(select 1 from {1} imb where imb.noofinpat=i.noofinpat AND  imb.valide=1)
            //                    order by i.outbed";
            //            string m_SqlGetPatientListForNew = @"
            //                                           SELECT i.name, i.sexid,
            //                                                            (select name from dictionary_detail where detailid = i.sexid and categoryid = '3') as gender,
            //                                                            i.agestr, i.patid, i.outbed as bedno, i.status,
            //                                                            (select d.name from diagnosis d where d.icd = i.admitdiagnosis and d.valid = '1') as diagnosis,
            //                                                            i.noofinpat  ,imb.iem_mainpage_no                    
            //                                                             FROM {1}  imb left join inpatient i on i.noofinpat=imb.noofinpat
            //                                           where imb.valide=1 and (i.admitdept = '{0}' or i.outhosdept = '{0}') 
            //                                            AND  exists
            //                                                (
            //                                                select * from {2} ie    
            //                                                left join zymosis_diagnosis z on ie.diagnosis_code=z.icd  and z.CATEGORYID=2                     
            //                                                where  z.valid=1                       
            //                                                and ie.valide=1  and imb.valide=1  and  ie.iem_mainpage_no=imb.iem_mainpage_no  
            //                                                AND ( SELECT COUNT(*) FROM theriomareportcard zr where 1=1 and zr.diagicd10=z.icd  and zr.REPORT_NOOFINPAT=imb.noofinpat and zr.diagicd10=z.icd and zr.vaild=1)< z.upcount
            //                                                )
            //                                           order by i.outbed
            //                                        ";

            string m_SqlGetPatientListForNew = @"
                        select distinct i.name, i.sexid, dd1.name gender, i.agestr, i.patid, i.outbed bedno, i.status, 
                               nvl(d.icd, dc.id) icd, d.tumorid, 
                               nvl(d.name, dc.name) diagnosis, i.noofinpat, imb.iem_mainpage_no
                        from {1} imb 
                        left outer join inpatient i on i.noofinpat = imb.noofinpat
                        left outer join dictionary_detail dd1 on dd1.detailid = i.sexid and dd1.categoryid = '3' and dd1.valid = '1'
                        left outer join {2} imd on imd.iem_mainpage_no = imb.iem_mainpage_no and imd.valide = '1'
                        left outer join diagnosis d on d.icd = imd.diagnosis_code and d.valid = '1'
                        left outer join diagnosisofchinese dc on dc.id = imd.diagnosis_code and dc.valid = '1' 
                        where imb.valide = 1 and (i.admitdept = '{0}' or i.outhosdept = '{0}') 
                        and exists
                        (
                            select 1 from zymosis_diagnosis z 
                             where z.icd = imd.diagnosis_code and z.categoryid = 2 and z.valid = 1 and imd.diagnosis_type_id<>13
                               and (
                                        select count(1) from theriomareportcard zr 
                                         where zr.report_icd10 = z.icd and zr.report_noofinpat = i.noofinpat 
                                           and zr.vaild = 1 and zr.state != '7') < z.upcount
                        )
                        order by name";

            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string sql = string.Format(m_SqlGetPatientListForNew, deptNO, "iem_mainpage_basicinfo_2012", "iem_mainpage_diagnosis_2012");

            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sql = string.Format(m_SqlGetPatientListForNew, deptNO, "iem_mainpage_basicinfo_sx", "iem_mainpage_diagnosis_sx");
            }
            DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();

            DataTable datatable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql); //m_IDataAccess.ExecuteDataTable(sql);
            return datatable;
        }
        // <summary>
        /// 获得新增病人时弹出的病人列表————出生缺陷  add  jxh
        /// </summary>
        /// <param name="deptNO"></param>
        /// <returns></returns>
        public DataTable GetPatientListForNewToother(string deptNO)
        {
            string valueString = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("IsChildDept");
            string m_SqlGetPatientListForNew = @"
                        select distinct i.name, i.sexid, dd1.name gender, i.agestr, i.patid, i.outbed bedno, i.status, 
                               nvl(d.icd, dc.id) icd, d.tumorid, 
                               nvl(d.name, dc.name) diagnosis, i.noofinpat, imb.iem_mainpage_no
                        from {1} imb 
                        left outer join inpatient i on i.noofinpat = imb.noofinpat and ROUND(TO_NUMBER(to_date(i.ADMITDATE, 'yyyy-MM-dd hh24:mi:ss') - to_date(i.BIRTH, 'yyyy-MM-dd hh24:mi:ss')))<42 and i.OUTHOSDEPT in ({3})
                        left outer join dictionary_detail dd1 on dd1.detailid = i.sexid and dd1.categoryid = '3' and dd1.valid = '1'
                        left outer join {2} imd on imd.iem_mainpage_no = imb.iem_mainpage_no and imd.valide = '1'
                        left outer join diagnosis d on d.icd = imd.diagnosis_code and d.valid = '1'
                        left outer join diagnosisofchinese dc on dc.id = imd.diagnosis_code and dc.valid = '1' 
                        where imb.valide = 1 and (i.admitdept in ({3}) or i.outhosdept in ({3})) 
                        and exists
                        (
                            select 1 from zymosis_diagnosis z 
                             where z.icd = imd.diagnosis_code and z.categoryid = 3 and z.valid = 1 and imd.diagnosis_type_id<>13
                               and (
                                        select count(1) from birthdefectscard zr 
                                         where zr.DIAG_CODE = z.icd and zr.REPORT_NOOFINPAT = i.noofinpat 
                                           and zr.VAILD = 1 and zr.STATE != '7') < z.upcount
                        )
                        order by name";

            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string sql = string.Format(m_SqlGetPatientListForNew, deptNO, "iem_mainpage_basicinfo_2012", "iem_mainpage_diagnosis_2012", valueString);

            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sql = string.Format(m_SqlGetPatientListForNew, deptNO, "iem_mainpage_basicinfo_sx", "iem_mainpage_diagnosis_sx", valueString);
            }
            DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();

            DataTable datatable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql); //m_IDataAccess.ExecuteDataTable(sql);
            return datatable;
        }

        /// <summary>
        /// 获取新增病人时弹出的病人列表（脑卒中报告卡）
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataTable GetPatientListForCARDIOVAS(string deptid)
        {
            string m_SqlGetPatientListForNew = @"
                        select distinct i.name, i.sexid, dd1.name gender, i.agestr, i.patid, i.outbed bedno, i.status, 
                               nvl(d.icd, dc.id) icd, d.tumorid, 
                               nvl(d.name, dc.name) diagnosis, i.noofinpat, imb.iem_mainpage_no
                        from {1} imb 
                        left outer join inpatient i on i.noofinpat = imb.noofinpat
                        left outer join dictionary_detail dd1 on dd1.detailid = i.sexid and dd1.categoryid = '3' and dd1.valid = '1'
                        left outer join {2} imd on imd.iem_mainpage_no = imb.iem_mainpage_no and imd.valide = '1'
                        left outer join diagnosis d on d.icd = imd.diagnosis_code and d.valid = '1'
                        left outer join diagnosisofchinese dc on dc.id = imd.diagnosis_code and dc.valid = '1' 
                        where imb.valide = 1 and (i.admitdept = '{0}' or i.outhosdept = '{0}') 
                        and exists
                        (
                            select 1 from zymosis_diagnosis z 
                             where z.icd = imd.diagnosis_code and z.categoryid = 4 and z.valid = 1 and imd.diagnosis_type_id<>13
                               and (
                                        select count(1) from cardiovascularcard cr
                                         where cr.icd = z.icd
                                           and cr.noofinpat = i.noofinpat
                                           and cr.VAILD = 1
                                           and cr.STATE != '7'
                                    ) < z.upcount
                        )
                        order by name";

            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string sql = string.Format(m_SqlGetPatientListForNew, deptid, "iem_mainpage_basicinfo_2012", "iem_mainpage_diagnosis_2012");

            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sql = string.Format(m_SqlGetPatientListForNew, deptid, "iem_mainpage_basicinfo_sx", "iem_mainpage_diagnosis_sx");
            }
            DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();

            DataTable datatable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql); //m_IDataAccess.ExecuteDataTable(sql);
            return datatable;
        }

        /// <summary>
        /// 获得新增病人时弹出的所有病人的列表
        /// </summary>
        /// <param name="deptNO"></param>
        /// <returns></returns>
        public DataTable GetAllPatientListForNew()
        {
            string m_SqlGetAllPatientListForNew = @"select i.name, i.sexid,
                    (select name from dictionary_detail where detailid = i.sexid and categoryid = '3') as gender,
                    i.agestr, i.patid, i.outbed as bedno, i.status,
                    (select d.name from diagnosis d where d.icd = i.admitdiagnosis and d.valid = '1') as diagnosis,
                    i.noofinpat
                    from inpatient i 
                    order by i.outbed";
            DataTable dt = m_IDataAccess.ExecuteDataTable(m_SqlGetAllPatientListForNew);
            return dt;
        }


        public DataTable GetAllPatientListForNewTo()
        {
            //            string m_SqlGetAllPatientListForNew = @"select i.name, i.sexid,
            //                    (select name from dictionary_detail where detailid = i.sexid and categoryid = '3') as gender,
            //                    i.agestr, i.patid, i.outbed as bedno, i.status,
            //                    (select d.name from diagnosis d where d.icd = i.admitdiagnosis and d.valid = '1') as diagnosis,
            //                    i.noofinpat
            //                    from inpatient i 
            //                    WHERE  exists(select 1 from {0} imb where imb.noofinpat=i.noofinpat AND  imb.valide=1)
            //                    order by i.outbed";
            //            string m_SqlGetAllPatientListForNew = @"
            //                                           SELECT i.name, i.sexid,
            //                                                            (select name from dictionary_detail where detailid = i.sexid and categoryid = '3') as gender,
            //                                                            i.agestr, i.patid, i.outbed as bedno, i.status,
            //                                                            (select d.name from diagnosis d where d.icd = i.admitdiagnosis and d.valid = '1') as diagnosis,
            //                                                            i.noofinpat  ,imb.iem_mainpage_no                    
            //                                                             FROM {0}  imb left join inpatient i on i.noofinpat=imb.noofinpat
            //                                           where imb.valide=1
            //                                               AND  exists
            //                                                   (
            //                                                   select * from {1} ie    
            //                                                   left join zymosis_diagnosis z on ie.diagnosis_code=z.icd  and z.CATEGORYID=2                     
            //                                                   where  z.valid=1                       
            //                                                   and ie.valide=1  and imb.valide=1  and  ie.iem_mainpage_no=imb.iem_mainpage_no  
            //                                                   AND ( SELECT COUNT(*) FROM theriomareportcard zr where 1=1 and zr.diagicd10=z.icd  and zr.REPORT_NOOFINPAT=imb.noofinpat and zr.diagicd10=z.icd and zr.vaild=1)< z.upcount
            //                                                   )
            //                                           order by i.outbed
            //                                        ";
            string m_SqlGetAllPatientListForNew = @"
                        select distinct i.name, i.sexid, dd1.name gender, i.agestr, i.patid, i.outbed bedno, i.status, 
                               nvl(d.icd, dc.id) icd, d.tumorid, 
                               nvl(d.name, dc.name) diagnosis, i.noofinpat, imb.iem_mainpage_no
                        from {1} imb 
                        left outer join inpatient i on i.noofinpat = imb.noofinpat
                        left outer join dictionary_detail dd1 on dd1.detailid = i.sexid and dd1.categoryid = '3' and dd1.valid = '1'
                        left outer join {2} imd on imd.iem_mainpage_no = imb.iem_mainpage_no and imd.valide = '1'
                        left outer join diagnosis d on d.icd = imd.diagnosis_code and d.valid = '1'
                        left outer join diagnosisofchinese dc on dc.id = imd.diagnosis_code and dc.valid = '1'
                        where imb.valide = 1 and (i.admitdept = '{0}' or i.outhosdept = '{0}') 
                        and exists
                        (
                            select 1 from zymosis_diagnosis z 
                             where z.icd = imd.diagnosis_code and z.categoryid = 2 and z.valid = 1
                               and (
                                        select count(1) from theriomareportcard zr 
                                         where zr.report_icd10 = z.icd and zr.report_noofinpat = i.noofinpat 
                                           and zr.vaild = 1 and zr.state != '7'
                                    ) < z.upcount
                        )";

            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string sql = string.Format(m_SqlGetAllPatientListForNew, "iem_mainpage_basicinfo_2012", "iem_mainpage_diagnosis_2012");
            //iem_mainpage_diagnosis_sx
            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sql = string.Format(m_SqlGetAllPatientListForNew, "iem_mainpage_basicinfo_sx", "iem_mainpage_diagnosis_sx");
            }

            DataTable dt = m_IDataAccess.ExecuteDataTable(sql);
            return dt;
        }


        /// <summary>
        /// 返回报告卡类别列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetReportCategoryList()
        {
            DataTable dt = m_IDataAccess.ExecuteDataTable("SELECT ID,Name,TABLENAME FROM reportcategory where VALID=1 ORDER BY ID");
            return dt;
        }

        #region 返回肿瘤报告卡实体ReportCardEntity
        /// <summary>
        /// 肿瘤报告卡序号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReportCardEntity GetReportCardEntity(string id)
        {
            ReportCardEntity _ReportCardEntity = new ReportCardEntity();
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@report_ID",SqlDbType.VarChar)
                };
            sqlParam[0].Value = id;
            string SqlText = string.Format(@"select tr.*,dd.name martialName,dt.name departName,u.name userName,dd1.name as occuName,dt1.name as DIAGUNIT from THERIOMAREPORTCARD tr 
                                               left join dictionary_detail dd on dd.categoryid='4' and dd.detailid=tr.martial
                                                    left join department dt on dt.id=tr.reportinfunit and dt.valid='1'
left join department dt1 on dt1.id=tr.REPORTDIAGFUNIT and dt1.valid='1'
                                                       left join users u on u.id=tr.reportdoctor and u.valid='1'
                                                       left join dictionary_detail dd1 on dd1.detailid=tr.occupation and dd1.categoryid='41' and dd1.valid='1'
                                              WHERE 1=1 AND tr.REPORT_ID=@report_ID");
            DataTable dataTable = m_IDataAccess.ExecuteDataTable(SqlText, sqlParam, CommandType.Text);
            if (dataTable.Rows.Count > 0)
            {
                _ReportCardEntity.Report_No = dataTable.Rows[0]["REPORT_NO"].ToString();
                _ReportCardEntity.Report_Id = dataTable.Rows[0]["REPORT_ID"].ToString();
                _ReportCardEntity.Report_Noofinpat = dataTable.Rows[0]["REPORT_NOOFINPAT"].ToString();

                _ReportCardEntity.Report_DistrictId = dataTable.Rows[0]["REPORT_DISTRICTID"].ToString();
                _ReportCardEntity.Report_DistrictName = dataTable.Rows[0]["REPORT_DISTRICTNAME"].ToString();
                _ReportCardEntity.Report_ICD10 = dataTable.Rows[0]["REPORT_ICD10"].ToString();
                _ReportCardEntity.Report_ICD0 = dataTable.Rows[0]["REPORT_ICD0"].ToString();
                _ReportCardEntity.Report_ClinicId = dataTable.Rows[0]["REPORT_CLINICID"].ToString();
                _ReportCardEntity.Report_PatId = dataTable.Rows[0]["REPORT_PATID"].ToString();
                _ReportCardEntity.Report_INDO = dataTable.Rows[0]["REPORT_INDO"].ToString();
                _ReportCardEntity.Report_InpatName = dataTable.Rows[0]["REPORT_INPATNAME"].ToString();
                _ReportCardEntity.SexId = dataTable.Rows[0]["SEXID"].ToString();
                _ReportCardEntity.RealAge = dataTable.Rows[0]["REALAGE"].ToString();
                _ReportCardEntity.BirthDate = dataTable.Rows[0]["BIRTHDATE"].ToString();
                _ReportCardEntity.NationId = dataTable.Rows[0]["NATIONID"].ToString();
                _ReportCardEntity.NationName = dataTable.Rows[0]["NATIONNAME"].ToString();
                _ReportCardEntity.ContactTel = dataTable.Rows[0]["CONTACTTEL"].ToString();
                _ReportCardEntity.Martial = dataTable.Rows[0]["MARTIAL"].ToString();
                _ReportCardEntity.Occupation = dataTable.Rows[0]["OCCUPATION"].ToString();
                _ReportCardEntity.OfficeAddress = dataTable.Rows[0]["OFFICEADDRESS"].ToString();
                /*户口所在地*/
                _ReportCardEntity.OrgProvinceId = dataTable.Rows[0]["ORGPROVINCEID"].ToString();
                _ReportCardEntity.OrgCityId = dataTable.Rows[0]["ORGDISTRICTID"].ToString();
                _ReportCardEntity.OrgDistrictId = dataTable.Rows[0]["ORGDISTRICTID"].ToString();
                _ReportCardEntity.OrgTownId = dataTable.Rows[0]["ORGTOWNID"].ToString();
                _ReportCardEntity.OrgVillage = dataTable.Rows[0]["ORGVILLIAGE"].ToString();

                _ReportCardEntity.OrgProvinceName = dataTable.Rows[0]["ORGPROVINCENAME"].ToString();
                _ReportCardEntity.OrgCityName = dataTable.Rows[0]["ORGCITYNAME"].ToString();
                _ReportCardEntity.OrgDistrictName = dataTable.Rows[0]["ORGDISTRICTNAME"].ToString();
                _ReportCardEntity.OrgTown = dataTable.Rows[0]["ORGTOWN"].ToString();
                _ReportCardEntity.OrgVillageName = dataTable.Rows[0]["ORGVILLAGENAME"].ToString();

                /*现住地址*/
                _ReportCardEntity.XZZProvinceId = dataTable.Rows[0]["XZZPROVINCEID"].ToString();
                _ReportCardEntity.XZZCityId = dataTable.Rows[0]["XZZCITYID"].ToString();
                _ReportCardEntity.XZZDistrictId = dataTable.Rows[0]["XZZDISTRICTID"].ToString();
                _ReportCardEntity.XZZTownId = dataTable.Rows[0]["XZZTOWNID"].ToString();
                _ReportCardEntity.XZZVillageId = dataTable.Rows[0]["XZZVILLIAGEID"].ToString();

                _ReportCardEntity.XZZProvince = dataTable.Rows[0]["XZZPROVINCE"].ToString();
                _ReportCardEntity.XZZCity = dataTable.Rows[0]["XZZCITY"].ToString();
                _ReportCardEntity.XZZDistrict = dataTable.Rows[0]["XZZDISTRICT"].ToString();
                _ReportCardEntity.XZZTown = dataTable.Rows[0]["XZZTOWN"].ToString();
                _ReportCardEntity.XZZVillage = dataTable.Rows[0]["XZZVILLIAGE"].ToString();


                _ReportCardEntity.Report_Diagnosis = dataTable.Rows[0]["REPORT_DIAGNOSIS"].ToString();
                _ReportCardEntity.PathologicalType = dataTable.Rows[0]["PATHOLOGICALTYPE"].ToString();
                _ReportCardEntity.PathologicalId = dataTable.Rows[0]["PATHOLOGICALID"].ToString();

                _ReportCardEntity.QZDigTime_T = dataTable.Rows[0]["QZDIAGTIME_T"].ToString();
                _ReportCardEntity.QZDigTime_M = dataTable.Rows[0]["QZDIAGTIME_M"].ToString();
                _ReportCardEntity.QZDiaTime_N = dataTable.Rows[0]["QZDIAGTIME_N"].ToString();

                _ReportCardEntity.FirstDiaDate = dataTable.Rows[0]["FIRSTDIADATE"].ToString();

                _ReportCardEntity.ReportInfunit = dataTable.Rows[0]["REPORTINFUNIT"].ToString();
                _ReportCardEntity.ReportDoctor = dataTable.Rows[0]["REPORTDOCTOR"].ToString();
                _ReportCardEntity.ReportDate = dataTable.Rows[0]["REPORTDATE"].ToString();
                _ReportCardEntity.DeathDate = dataTable.Rows[0]["DEATHDATE"].ToString();
                _ReportCardEntity.DeathReason = dataTable.Rows[0]["DEATHREASON"].ToString();
                _ReportCardEntity.ReJest = dataTable.Rows[0]["REJEST"].ToString();

                _ReportCardEntity.Report_YdiagNosis = dataTable.Rows[0]["REPORT_YDIAGNOSIS"].ToString();
                _ReportCardEntity.Report_YdiagNosisData = dataTable.Rows[0]["REPORT_YDIAGNOSISDATA"].ToString();
                _ReportCardEntity.Report_DiagNosisBased = dataTable.Rows[0]["REPORT_DIAGNOSISBASED"].ToString();


                _ReportCardEntity.State = dataTable.Rows[0]["State"].ToString();


                _ReportCardEntity.CreateUsercode = dataTable.Rows[0]["create_UserCode"].ToString();
                _ReportCardEntity.CreateUsername = dataTable.Rows[0]["create_UserName"].ToString();
                _ReportCardEntity.CreateDeptcode = dataTable.Rows[0]["create_deptCode"].ToString();
                _ReportCardEntity.CreateDeptname = dataTable.Rows[0]["create_deptName"].ToString();
                _ReportCardEntity.ModifyDate = dataTable.Rows[0]["Modify_date"].ToString();

                _ReportCardEntity.ModifyUsercode = dataTable.Rows[0]["Modify_UserCode"].ToString();
                _ReportCardEntity.ModifyUsername = dataTable.Rows[0]["Modify_UserName"].ToString();
                _ReportCardEntity.ModifyDeptcode = dataTable.Rows[0]["Modify_deptCode"].ToString();
                _ReportCardEntity.ModifyDeptname = dataTable.Rows[0]["Modify_deptName"].ToString();
                _ReportCardEntity.AuditDate = dataTable.Rows[0]["Audit_date"].ToString();

                _ReportCardEntity.AuditUsercode = dataTable.Rows[0]["Audit_UserCode"].ToString();
                _ReportCardEntity.AuditUsername = dataTable.Rows[0]["Audit_UserName"].ToString();
                _ReportCardEntity.AuditDeptcode = dataTable.Rows[0]["Audit_deptCode"].ToString();
                _ReportCardEntity.AuditDeptname = dataTable.Rows[0]["Audit_deptName"].ToString();
                _ReportCardEntity.Vaild = dataTable.Rows[0]["Vaild"].ToString();
                _ReportCardEntity.DIAGICD10 = dataTable.Rows[0]["DIAGICD10"].ToString();
                _ReportCardEntity.CANCELREASON = dataTable.Rows[0]["CANCELREASON"].ToString();
                _ReportCardEntity.MartialText = dataTable.Rows[0]["MARTIALNAME"].ToString();
                _ReportCardEntity.ReportInfunitName = dataTable.Rows[0]["DEPARTNAME"].ToString();
                _ReportCardEntity.ReportDoctorName = dataTable.Rows[0]["USERNAME"].ToString();
                _ReportCardEntity.OccuName = dataTable.Rows[0]["OCCUNAME"].ToString();

                //报告卡类型（死亡或者发病）0是发病 1是死亡
                _ReportCardEntity.ReportCardType = dataTable.Rows[0]["CARDTYPE"].ToString();

                _ReportCardEntity.ReportDiagfunit = dataTable.Rows[0]["REPORTDIAGFUNIT"].ToString();
                _ReportCardEntity.ReportDiagfunitName = dataTable.Rows[0]["DIAGUNIT"].ToString();
                _ReportCardEntity.ClinicalStages = dataTable.Rows[0]["clinicalstages"].ToString();

            }
            return _ReportCardEntity;
        }
        #endregion

        #region 肿瘤报告卡添加
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ReportCardEntity">返回新的报告卡ID</param>
        /// <returns></returns>
        public string AddReportCard(ReportCardEntity _ReportCardEntity)
        {
            SqlParameter[] sqlParam = GetSqlParameter(_ReportCardEntity, "1");

            return m_IDataAccess.ExecuteDataSet("Emr_Zymosis_Report.usp_EditTherioma_Report", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();


        }


        public string AddBirthDefectsReportCard(ReportCardEntity1 _ReportCardEntity)
        {
            SqlParameter[] sqlParam = GetSqlParameters(_ReportCardEntity, "1");

            return m_IDataAccess.ExecuteDataSet("Emr_Zymosis_Report.usp_EditTbirthdefects_Report", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();


        }

        public ReportCardEntity1 GetBirthDefectsReportCardEntity(string id)
        {
            ReportCardEntity1 _ReportCardEntity = new ReportCardEntity1();
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@ID",SqlDbType.VarChar)
                };
            sqlParam[0].Value = id;
            string SqlText = string.Format(@"select * from birthdefectscard                                            
                                              WHERE vaild=1 AND ID=@ID");
            DataTable dataTable = m_IDataAccess.ExecuteDataTable(SqlText, sqlParam, CommandType.Text);
            if (dataTable.Rows.Count > 0)
            {
                _ReportCardEntity.Id = dataTable.Rows[0]["ID"].ToString();
                _ReportCardEntity.ReportNoofinpat = dataTable.Rows[0]["REPORT_NOOFINPAT"].ToString();
                _ReportCardEntity.ReportId = dataTable.Rows[0]["REPORT_ID"].ToString();
                _ReportCardEntity.DiagCode = dataTable.Rows[0]["DIAG_CODE"].ToString();
                _ReportCardEntity.ReportProvince = dataTable.Rows[0]["REPORT_PROVINCE"].ToString();
                _ReportCardEntity.ReportCity = dataTable.Rows[0]["REPORT_CITY"].ToString();
                _ReportCardEntity.ReportTown = dataTable.Rows[0]["REPORT_TOWN"].ToString();
                _ReportCardEntity.ReportVillage = dataTable.Rows[0]["REPORT_VILLAGE"].ToString();
                _ReportCardEntity.ReportHospital = dataTable.Rows[0]["REPORT_HOSPITAL"].ToString();
                _ReportCardEntity.ReportNo = dataTable.Rows[0]["REPORT_NO"].ToString();
                _ReportCardEntity.MotherPatid = dataTable.Rows[0]["MOTHER_PATID"].ToString();
                _ReportCardEntity.MotherName = dataTable.Rows[0]["MOTHER_NAME"].ToString();
                _ReportCardEntity.MotherAge = dataTable.Rows[0]["MOTHER_AGE"].ToString();
                _ReportCardEntity.National = dataTable.Rows[0]["NATIONAL"].ToString();
                _ReportCardEntity.AddressPost = dataTable.Rows[0]["ADDRESS_POST"].ToString();
                _ReportCardEntity.Pregnantno = dataTable.Rows[0]["PREGNANTNO"].ToString();
                _ReportCardEntity.Productionno = dataTable.Rows[0]["PRODUCTIONNO"].ToString();
                _ReportCardEntity.Localadd = dataTable.Rows[0]["LOCALADD"].ToString();
                _ReportCardEntity.Percapitaincome = dataTable.Rows[0]["PERCAPITAINCOME"].ToString();
                _ReportCardEntity.Educationlevel = dataTable.Rows[0]["EDUCATIONLEVEL"].ToString();
                _ReportCardEntity.ChildPatid = dataTable.Rows[0]["CHILD_PATID"].ToString();
                _ReportCardEntity.ChildName = dataTable.Rows[0]["CHILD_NAME"].ToString();
                _ReportCardEntity.Isbornhere = dataTable.Rows[0]["ISBORNHERE"].ToString();
                _ReportCardEntity.ChildSex = dataTable.Rows[0]["CHILD_SEX"].ToString();
                _ReportCardEntity.BornYear = dataTable.Rows[0]["BORN_YEAR"].ToString();
                _ReportCardEntity.BornMonth = dataTable.Rows[0]["BORN_MONTH"].ToString();
                _ReportCardEntity.BornDay = dataTable.Rows[0]["BORN_DAY"].ToString();
                _ReportCardEntity.Gestationalage = dataTable.Rows[0]["GESTATIONALAGE"].ToString();
                _ReportCardEntity.Weight = dataTable.Rows[0]["WEIGHT"].ToString();
                _ReportCardEntity.Births = dataTable.Rows[0]["BIRTHS"].ToString();
                _ReportCardEntity.Isidentical = dataTable.Rows[0]["ISIDENTICAL"].ToString();
                _ReportCardEntity.Outcome = dataTable.Rows[0]["OUTCOME"].ToString();
                _ReportCardEntity.Inducedlabor = dataTable.Rows[0]["INDUCEDLABOR"].ToString();
                _ReportCardEntity.Diagnosticbasis = dataTable.Rows[0]["DIAGNOSTICBASIS"].ToString();
                _ReportCardEntity.Diagnosticbasis1 = dataTable.Rows[0]["DIAGNOSTICBASIS1"].ToString();
                _ReportCardEntity.Diagnosticbasis2 = dataTable.Rows[0]["DIAGNOSTICBASIS2"].ToString();
                _ReportCardEntity.Diagnosticbasis3 = dataTable.Rows[0]["DIAGNOSTICBASIS3"].ToString();
                _ReportCardEntity.Diagnosticbasis4 = dataTable.Rows[0]["DIAGNOSTICBASIS4"].ToString();
                _ReportCardEntity.Diagnosticbasis5 = dataTable.Rows[0]["DIAGNOSTICBASIS5"].ToString();
                _ReportCardEntity.Diagnosticbasis6 = dataTable.Rows[0]["DIAGNOSTICBASIS6"].ToString();
                _ReportCardEntity.Diagnosticbasis7 = dataTable.Rows[0]["DIAGNOSTICBASIS7"].ToString();
                _ReportCardEntity.DiagAnencephaly = dataTable.Rows[0]["DIAG_ANENCEPHALY"].ToString();
                _ReportCardEntity.DiagSpina = dataTable.Rows[0]["DIAG_SPINA"].ToString();
                _ReportCardEntity.DiagPengout = dataTable.Rows[0]["DIAG_PENGOUT"].ToString();
                _ReportCardEntity.DiagHydrocephalus = dataTable.Rows[0]["DIAG_HYDROCEPHALUS"].ToString();
                _ReportCardEntity.DiagCleftpalate = dataTable.Rows[0]["DIAG_CLEFTPALATE"].ToString();
                _ReportCardEntity.DiagCleftlip = dataTable.Rows[0]["DIAG_CLEFTLIP"].ToString();
                _ReportCardEntity.DiagCleftmerger = dataTable.Rows[0]["DIAG_CLEFTMERGER"].ToString();
                _ReportCardEntity.DiagSmallears = dataTable.Rows[0]["DIAG_SMALLEARS"].ToString();
                _ReportCardEntity.DiagOuterear = dataTable.Rows[0]["DIAG_OUTEREAR"].ToString();
                _ReportCardEntity.DiagEsophageal = dataTable.Rows[0]["DIAG_ESOPHAGEAL"].ToString();
                _ReportCardEntity.DiagRectum = dataTable.Rows[0]["DIAG_RECTUM"].ToString();
                _ReportCardEntity.DiagHypospadias = dataTable.Rows[0]["DIAG_HYPOSPADIAS"].ToString();
                _ReportCardEntity.DiagBladder = dataTable.Rows[0]["DIAG_BLADDER"].ToString();
                _ReportCardEntity.DiagHorseshoefootleft = dataTable.Rows[0]["DIAG_HORSESHOEFOOTLEFT"].ToString();
                _ReportCardEntity.DiagHorseshoefootright = dataTable.Rows[0]["DIAG_HORSESHOEFOOTRIGHT"].ToString();
                _ReportCardEntity.DiagManypointleft = dataTable.Rows[0]["DIAG_MANYPOINTLEFT"].ToString();
                _ReportCardEntity.DiagManypointright = dataTable.Rows[0]["DIAG_MANYPOINTRIGHT"].ToString();
                _ReportCardEntity.DiagLimbsupperleft = dataTable.Rows[0]["DIAG_LIMBSUPPERLEFT"].ToString();
                _ReportCardEntity.DiagLimbsupperright = dataTable.Rows[0]["DIAG_LIMBSUPPERRIGHT"].ToString();
                _ReportCardEntity.DiagLimbslowerleft = dataTable.Rows[0]["DIAG_LIMBSLOWERLEFT"].ToString();
                _ReportCardEntity.DiagLimbslowerright = dataTable.Rows[0]["DIAG_LIMBSLOWERRIGHT"].ToString();
                _ReportCardEntity.DiagHernia = dataTable.Rows[0]["DIAG_HERNIA"].ToString();
                _ReportCardEntity.DiagBulgingbelly = dataTable.Rows[0]["DIAG_BULGINGBELLY"].ToString();
                _ReportCardEntity.DiagGastroschisis = dataTable.Rows[0]["DIAG_GASTROSCHISIS"].ToString();
                _ReportCardEntity.DiagTwins = dataTable.Rows[0]["DIAG_TWINS"].ToString();
                _ReportCardEntity.DiagTssyndrome = dataTable.Rows[0]["DIAG_TSSYNDROME"].ToString();
                _ReportCardEntity.DiagHeartdisease = dataTable.Rows[0]["DIAG_HEARTDISEASE"].ToString();
                _ReportCardEntity.DiagOther = dataTable.Rows[0]["DIAG_OTHER"].ToString();
                _ReportCardEntity.DiagOthercontent = dataTable.Rows[0]["DIAG_OTHERCONTENT"].ToString();
                _ReportCardEntity.Fever = dataTable.Rows[0]["FEVER"].ToString();
                _ReportCardEntity.Virusinfection = dataTable.Rows[0]["VIRUSINFECTION"].ToString();
                _ReportCardEntity.Illother = dataTable.Rows[0]["ILLOTHER"].ToString();
                _ReportCardEntity.Sulfa = dataTable.Rows[0]["SULFA"].ToString();
                _ReportCardEntity.Antibiotics = dataTable.Rows[0]["ANTIBIOTICS"].ToString();
                _ReportCardEntity.Birthcontrolpill = dataTable.Rows[0]["BIRTHCONTROLPILL"].ToString();
                _ReportCardEntity.Sedatives = dataTable.Rows[0]["SEDATIVES"].ToString();
                _ReportCardEntity.Medicineother = dataTable.Rows[0]["MEDICINEOTHER"].ToString();
                _ReportCardEntity.Drinking = dataTable.Rows[0]["DRINKING"].ToString();
                _ReportCardEntity.Pesticide = dataTable.Rows[0]["PESTICIDE"].ToString();
                _ReportCardEntity.Ray = dataTable.Rows[0]["RAY"].ToString();
                _ReportCardEntity.Chemicalagents = dataTable.Rows[0]["CHEMICALAGENTS"].ToString();
                _ReportCardEntity.Factorother = dataTable.Rows[0]["FACTOROTHER"].ToString();
                _ReportCardEntity.Stillbirthno = dataTable.Rows[0]["STILLBIRTHNO"].ToString();
                _ReportCardEntity.Abortionno = dataTable.Rows[0]["ABORTIONNO"].ToString();
                _ReportCardEntity.Defectsno = dataTable.Rows[0]["DEFECTSNO"].ToString();
                _ReportCardEntity.Defectsof1 = dataTable.Rows[0]["DEFECTSOF1"].ToString();
                _ReportCardEntity.Defectsof2 = dataTable.Rows[0]["DEFECTSOF2"].ToString();
                _ReportCardEntity.Defectsof3 = dataTable.Rows[0]["DEFECTSOF3"].ToString();
                _ReportCardEntity.Ycdefectsof1 = dataTable.Rows[0]["YCDEFECTSOF1"].ToString();
                _ReportCardEntity.Ycdefectsof2 = dataTable.Rows[0]["YCDEFECTSOF2"].ToString();
                _ReportCardEntity.Ycdefectsof3 = dataTable.Rows[0]["YCDEFECTSOF3"].ToString();
                _ReportCardEntity.Kinshipdefects1 = dataTable.Rows[0]["KINSHIPDEFECTS1"].ToString();
                _ReportCardEntity.Kinshipdefects2 = dataTable.Rows[0]["KINSHIPDEFECTS2"].ToString();
                _ReportCardEntity.Kinshipdefects3 = dataTable.Rows[0]["KINSHIPDEFECTS3"].ToString();
                _ReportCardEntity.Cousinmarriage = dataTable.Rows[0]["COUSINMARRIAGE"].ToString();
                _ReportCardEntity.Cousinmarriagebetween = dataTable.Rows[0]["COUSINMARRIAGEBETWEEN"].ToString();
                _ReportCardEntity.Preparer = dataTable.Rows[0]["PREPARER"].ToString();
                _ReportCardEntity.Thetitle1 = dataTable.Rows[0]["THETITLE1"].ToString();
                _ReportCardEntity.Filldateyear = dataTable.Rows[0]["FILLDATEYEAR"].ToString();
                _ReportCardEntity.Filldatemonth = dataTable.Rows[0]["FILLDATEMONTH"].ToString();
                _ReportCardEntity.Filldateday = dataTable.Rows[0]["FILLDATEDAY"].ToString();
                _ReportCardEntity.Hospitalreview = dataTable.Rows[0]["HOSPITALREVIEW"].ToString();
                _ReportCardEntity.Thetitle2 = dataTable.Rows[0]["THETITLE2"].ToString();
                _ReportCardEntity.Hospitalauditdateyear = dataTable.Rows[0]["HOSPITALAUDITDATEYEAR"].ToString();
                _ReportCardEntity.Hospitalauditdatemonth = dataTable.Rows[0]["HOSPITALAUDITDATEMONTH"].ToString();
                _ReportCardEntity.Hospitalauditdateday = dataTable.Rows[0]["HOSPITALAUDITDATEDAY"].ToString();
                _ReportCardEntity.Provincialview = dataTable.Rows[0]["PROVINCIALVIEW"].ToString();
                _ReportCardEntity.Thetitle3 = dataTable.Rows[0]["THETITLE3"].ToString();
                _ReportCardEntity.Provincialviewdateyear = dataTable.Rows[0]["PROVINCIALVIEWDATEYEAR"].ToString();
                _ReportCardEntity.Provincialviewdatemonth = dataTable.Rows[0]["PROVINCIALVIEWDATEMONTH"].ToString();
                _ReportCardEntity.Provincialviewdateday = dataTable.Rows[0]["PROVINCIALVIEWDATEDAY"].ToString();
                _ReportCardEntity.Feverno = dataTable.Rows[0]["FEVERNO"].ToString();
                _ReportCardEntity.Isvirusinfection = dataTable.Rows[0]["ISVIRUSINFECTION"].ToString();
                _ReportCardEntity.Isdiabetes = dataTable.Rows[0]["ISDIABETES"].ToString();
                _ReportCardEntity.Isillother = dataTable.Rows[0]["ISILLOTHER"].ToString();
                _ReportCardEntity.Issulfa = dataTable.Rows[0]["ISSULFA"].ToString();
                _ReportCardEntity.Isantibiotics = dataTable.Rows[0]["ISANTIBIOTICS"].ToString();
                _ReportCardEntity.Isbirthcontrolpill = dataTable.Rows[0]["ISBIRTHCONTROLPILL"].ToString();
                _ReportCardEntity.Issedatives = dataTable.Rows[0]["ISSEDATIVES"].ToString();
                _ReportCardEntity.Ismedicineother = dataTable.Rows[0]["ISMEDICINEOTHER"].ToString();
                _ReportCardEntity.Isdrinking = dataTable.Rows[0]["ISDRINKING"].ToString();
                _ReportCardEntity.Ispesticide = dataTable.Rows[0]["ISPESTICIDE"].ToString();
                _ReportCardEntity.Isray = dataTable.Rows[0]["ISRAY"].ToString();
                _ReportCardEntity.Ischemicalagents = dataTable.Rows[0]["ISCHEMICALAGENTS"].ToString();
                _ReportCardEntity.Isfactorother = dataTable.Rows[0]["ISFACTOROTHER"].ToString();
                _ReportCardEntity.State = dataTable.Rows[0]["STATE"].ToString();
                _ReportCardEntity.CreateDate = dataTable.Rows[0]["CREATE_DATE"].ToString();
                _ReportCardEntity.CreateUsercode = dataTable.Rows[0]["CREATE_USERCODE"].ToString();
                _ReportCardEntity.CreateUsername = dataTable.Rows[0]["CREATE_USERNAME"].ToString();
                _ReportCardEntity.CreateDeptcode = dataTable.Rows[0]["CREATE_DEPTCODE"].ToString();
                _ReportCardEntity.CreateDeptname = dataTable.Rows[0]["CREATE_DEPTNAME"].ToString();
                _ReportCardEntity.ModifyDate = dataTable.Rows[0]["MODIFY_DATE"].ToString();
                _ReportCardEntity.ModifyUsercode = dataTable.Rows[0]["MODIFY_USERCODE"].ToString();
                _ReportCardEntity.ModifyUsername = dataTable.Rows[0]["MODIFY_USERNAME"].ToString();
                _ReportCardEntity.ModifyDeptcode = dataTable.Rows[0]["MODIFY_DEPTCODE"].ToString();
                _ReportCardEntity.ModifyDeptname = dataTable.Rows[0]["MODIFY_DEPTNAME"].ToString();
                _ReportCardEntity.AuditDate = dataTable.Rows[0]["AUDIT_DATE"].ToString();
                _ReportCardEntity.AuditUsercode = dataTable.Rows[0]["AUDIT_USERCODE"].ToString();
                _ReportCardEntity.AuditUsername = dataTable.Rows[0]["AUDIT_USERNAME"].ToString();
                _ReportCardEntity.AuditDeptcode = dataTable.Rows[0]["AUDIT_DEPTCODE"].ToString();
                _ReportCardEntity.AuditDeptname = dataTable.Rows[0]["AUDIT_DEPTNAME"].ToString();
                _ReportCardEntity.Vaild = dataTable.Rows[0]["VAILD"].ToString();
                _ReportCardEntity.Cancelreason = dataTable.Rows[0]["CANCELREASON"].ToString();
                _ReportCardEntity.Prenatal = dataTable.Rows[0]["PRENATAL"].ToString();
                _ReportCardEntity.Prenatalno = dataTable.Rows[0]["PRENATALNO"].ToString();
                _ReportCardEntity.Postpartum = dataTable.Rows[0]["POSTPARTUM"].ToString();
                _ReportCardEntity.Andtoshowleft = dataTable.Rows[0]["ANDTOSHOWLEFT"].ToString();
                _ReportCardEntity.Andtoshowright = dataTable.Rows[0]["ANDTOSHOWRIGHT"].ToString();



            }
            return _ReportCardEntity;
        }
        #endregion

        #region 肿瘤报告卡删除
        public void DeleReportCate(string id)
        {

            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@report_ID",id)
                };

            m_IDataAccess.ExecuteNoneQuery(string.Format(@"DELETE FROM theriomareportcard WHERE report_id=@report_ID", sqlParam, CommandType.Text));
        }
        #endregion

        #region 肿瘤报告卡修改
        public string EditReportCard(ReportCardEntity _ReportCardEntity)
        {

            SqlParameter[] sqlParam = GetSqlParameter(_ReportCardEntity, "2");

            return m_IDataAccess.ExecuteDataSet("Emr_Zymosis_Report.usp_EditTherioma_Report", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();

        }
        #endregion

        #region ReportCardEntity 实体
        private SqlParameter[] GetSqlParameter(ReportCardEntity _ReportCardEntity, string editType)
        {
            #region
            SqlParameter[] sqlParam = new SqlParameter[] {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_ID",SqlDbType.VarChar),

                    new SqlParameter("@REPORT_DISTRICTID",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_DISTRICTNAME",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_ICD10",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_ICD0",SqlDbType.VarChar),

                    new SqlParameter("@REPORT_CLINICID",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_PATID",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_INDO",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_INPATNAME",SqlDbType.VarChar),

                    new SqlParameter("@SEXID",SqlDbType.VarChar),
                    new SqlParameter("@REALAGE",SqlDbType.VarChar),
                    new SqlParameter("@BIRTHDATE",SqlDbType.VarChar),
                    new SqlParameter("@NATIONID",SqlDbType.VarChar),
                    new SqlParameter("@NATIONNAME",SqlDbType.VarChar),

                    new SqlParameter("@CONTACTTEL",SqlDbType.VarChar),
                    new SqlParameter("@MARTIAL",SqlDbType.VarChar),
                    new SqlParameter("@OCCUPATION",SqlDbType.VarChar),
                    new SqlParameter("@OFFICEADDRESS",SqlDbType.VarChar),

                    new SqlParameter("@ORGPROVINCEID",SqlDbType.VarChar),
                    new SqlParameter("@ORGCITYID",SqlDbType.VarChar),
                    new SqlParameter("@ORGDISTRICTID",SqlDbType.VarChar),
                    new SqlParameter("@ORGTOWNID",SqlDbType.VarChar),
                    new SqlParameter("@ORGVILLIAGE",SqlDbType.VarChar),

                    new SqlParameter("@ORGPROVINCENAME",SqlDbType.VarChar),
                    new SqlParameter("@ORGCITYNAME",SqlDbType.VarChar),
                    new SqlParameter("@ORGDISTRICTNAME",SqlDbType.VarChar),
                    new SqlParameter("@ORGTOWN",SqlDbType.VarChar),
                    new SqlParameter("@ORGVILLAGENAME",SqlDbType.VarChar),

                    new SqlParameter("@XZZPROVINCEID",SqlDbType.VarChar),
                    new SqlParameter("@XZZCITYID",SqlDbType.VarChar),
                    new SqlParameter("@XZZDISTRICTID",SqlDbType.VarChar),
                    new SqlParameter("@XZZTOWNID",SqlDbType.VarChar),
                    new SqlParameter("@XZZVILLIAGEID",SqlDbType.VarChar),

                    new SqlParameter("@XZZPROVINCE",SqlDbType.VarChar),
                    new SqlParameter("@XZZCITY",SqlDbType.VarChar),
                    new SqlParameter("@XZZDISTRICT",SqlDbType.VarChar),
                    new SqlParameter("@XZZTOWN",SqlDbType.VarChar),
                    new SqlParameter("@XZZVILLIAGE",SqlDbType.VarChar),

                    new SqlParameter("@REPORT_DIAGNOSIS",SqlDbType.VarChar),
                    new SqlParameter("@PATHOLOGICALTYPE",SqlDbType.VarChar),
                    new SqlParameter("@PATHOLOGICALID",SqlDbType.VarChar),

                    new SqlParameter("@QZDIAGTIME_T",SqlDbType.VarChar),
                    new SqlParameter("@QZDIAGTIME_N",SqlDbType.VarChar),
                    new SqlParameter("@QZDIAGTIME_M",SqlDbType.VarChar),

                    new SqlParameter("@FIRSTDIADATE",SqlDbType.VarChar),

                    new SqlParameter("@REPORTINFUNIT",SqlDbType.VarChar),
                    new SqlParameter("@REPORTDOCTOR",SqlDbType.VarChar),
                    new SqlParameter("@REPORTDATE",SqlDbType.VarChar),

                    new SqlParameter("@DEATHDATE",SqlDbType.VarChar),
                    new SqlParameter("@DEATHREASON",SqlDbType.VarChar),

                    new SqlParameter("@REJEST",SqlDbType.VarChar),

                    /*原诊断begin */  
                    new SqlParameter("@REPORT_YDIAGNOSIS",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_YDIAGNOSISDATA",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_DIAGNOSISBASED",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_NO",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_NOOFINPAT",SqlDbType.VarChar),//患者ID\
                    /*end*/
                    //报告状态
                    new SqlParameter("@STATE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DATE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_USERCODE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_USERNAME",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DEPTCODE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DEPTNAME",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_DATE",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_USERCODE",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_USERNAME",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_DEPTCODE",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_DEPTNAME",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_DATE",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_USERCODE",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_USERNAME",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_DEPTCODE",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_DEPTNAME",SqlDbType.VarChar),
                    new SqlParameter("@VAILD",SqlDbType.VarChar),
                    new SqlParameter("@DIAGICD10",SqlDbType.VarChar),
                    new SqlParameter("@CANCELREASON",SqlDbType.VarChar),
                    new SqlParameter("@CARDTYPE",SqlDbType.VarChar),//类型
                    new SqlParameter("@clinicalstages",SqlDbType.VarChar),//临床分期0.I-IV期
                    new SqlParameter("@ReportDiagfunit",SqlDbType.VarChar)//诊断单位
                    //end
            };
            sqlParam[0].Value = editType;

            sqlParam[1].Value = _ReportCardEntity.Report_Id.Trim();
            sqlParam[2].Value = _ReportCardEntity.Report_DistrictId.Trim();
            sqlParam[3].Value = _ReportCardEntity.Report_DistrictName.Trim();
            sqlParam[4].Value = _ReportCardEntity.Report_ICD10.Trim();
            sqlParam[5].Value = _ReportCardEntity.Report_ICD0.Trim();
            sqlParam[6].Value = _ReportCardEntity.Report_ClinicId.Trim();
            sqlParam[7].Value = _ReportCardEntity.Report_PatId.Trim();
            sqlParam[8].Value = _ReportCardEntity.Report_INDO.Trim();
            sqlParam[9].Value = _ReportCardEntity.Report_InpatName.Trim();
            sqlParam[10].Value = _ReportCardEntity.SexId.Trim();

            sqlParam[11].Value = _ReportCardEntity.RealAge.Trim();
            sqlParam[12].Value = _ReportCardEntity.BirthDate.Trim();
            sqlParam[13].Value = _ReportCardEntity.NationId.Trim();
            sqlParam[14].Value = _ReportCardEntity.NationName.Trim();
            sqlParam[15].Value = _ReportCardEntity.ContactTel.Trim();
            sqlParam[16].Value = _ReportCardEntity.Martial.Trim();
            sqlParam[17].Value = _ReportCardEntity.Occupation.Trim();
            sqlParam[18].Value = _ReportCardEntity.OfficeAddress.Trim();

            /*户口所在地*/
            sqlParam[19].Value = _ReportCardEntity.OrgProvinceId.Trim();
            sqlParam[20].Value = _ReportCardEntity.OrgCityId.Trim();



            sqlParam[21].Value = _ReportCardEntity.OrgDistrictId.Trim();
            sqlParam[22].Value = _ReportCardEntity.OrgTownId.Trim();
            sqlParam[23].Value = _ReportCardEntity.OrgVillage.Trim();

            sqlParam[24].Value = _ReportCardEntity.OrgProvinceName.Trim();
            sqlParam[25].Value = _ReportCardEntity.OrgCityName.Trim();
            sqlParam[26].Value = _ReportCardEntity.OrgDistrictName.Trim();
            sqlParam[27].Value = _ReportCardEntity.OrgTown.Trim();
            sqlParam[28].Value = _ReportCardEntity.OrgVillageName.Trim();


            /*现住地址*/
            sqlParam[29].Value = _ReportCardEntity.XZZProvinceId.Trim();
            sqlParam[30].Value = _ReportCardEntity.XZZCityId.Trim();


            sqlParam[31].Value = _ReportCardEntity.XZZDistrictId.Trim();
            sqlParam[32].Value = _ReportCardEntity.XZZTownId.Trim();
            sqlParam[33].Value = _ReportCardEntity.XZZVillageId.Trim();


            sqlParam[34].Value = _ReportCardEntity.XZZProvince.Trim();
            sqlParam[35].Value = _ReportCardEntity.XZZCity.Trim();
            sqlParam[36].Value = _ReportCardEntity.XZZDistrict.Trim();
            sqlParam[37].Value = _ReportCardEntity.XZZTown.Trim();
            sqlParam[38].Value = _ReportCardEntity.XZZVillage.Trim();


            sqlParam[39].Value = _ReportCardEntity.Report_Diagnosis.Trim();
            sqlParam[40].Value = _ReportCardEntity.PathologicalType.Trim();


            sqlParam[41].Value = _ReportCardEntity.PathologicalId.Trim();

            sqlParam[42].Value = _ReportCardEntity.QZDigTime_T.Trim();
            sqlParam[43].Value = _ReportCardEntity.QZDiaTime_N.Trim();
            sqlParam[44].Value = _ReportCardEntity.QZDigTime_M.Trim();

            sqlParam[45].Value = _ReportCardEntity.FirstDiaDate.Trim();

            sqlParam[46].Value = _ReportCardEntity.ReportInfunit.Trim();
            sqlParam[47].Value = _ReportCardEntity.ReportDoctor.Trim();
            sqlParam[48].Value = _ReportCardEntity.ReportDate.Trim();

            sqlParam[49].Value = _ReportCardEntity.DeathDate.Trim();
            sqlParam[50].Value = _ReportCardEntity.DeathReason.Trim();

            sqlParam[51].Value = _ReportCardEntity.ReJest.Trim();

            sqlParam[52].Value = _ReportCardEntity.Report_YdiagNosis.Trim();
            sqlParam[53].Value = _ReportCardEntity.Report_YdiagNosisData.Trim();
            sqlParam[54].Value = _ReportCardEntity.Report_DiagNosisBased.Trim();

            sqlParam[55].Value = _ReportCardEntity.Report_No.Trim();
            sqlParam[56].Value = _ReportCardEntity.Report_Noofinpat.Trim();

            sqlParam[57].Value = _ReportCardEntity.State.Trim();

            sqlParam[58].Value = _ReportCardEntity.CreateDate.Trim();
            sqlParam[59].Value = _ReportCardEntity.CreateUsercode.Trim();
            sqlParam[60].Value = _ReportCardEntity.CreateUsername.Trim();

            sqlParam[61].Value = _ReportCardEntity.CreateDeptcode.Trim();
            sqlParam[62].Value = _ReportCardEntity.CreateDeptname.Trim();

            sqlParam[63].Value = _ReportCardEntity.ModifyDate.Trim();
            sqlParam[64].Value = _ReportCardEntity.ModifyUsercode.Trim();
            sqlParam[65].Value = _ReportCardEntity.ModifyUsername.Trim();
            sqlParam[66].Value = _ReportCardEntity.ModifyDeptcode.Trim();
            sqlParam[67].Value = _ReportCardEntity.ModifyDeptname.Trim();

            sqlParam[68].Value = _ReportCardEntity.AuditDate.Trim();
            sqlParam[69].Value = _ReportCardEntity.AuditUsercode.Trim();
            sqlParam[70].Value = _ReportCardEntity.AuditUsername.Trim();

            sqlParam[71].Value = _ReportCardEntity.AuditDeptcode.Trim();
            sqlParam[72].Value = _ReportCardEntity.AuditDeptname.Trim();
            sqlParam[73].Value = _ReportCardEntity.Vaild.Trim();
            sqlParam[74].Value = _ReportCardEntity.DIAGICD10.Trim();
            sqlParam[75].Value = _ReportCardEntity.CANCELREASON.Trim();

            sqlParam[76].Value = _ReportCardEntity.ReportCardType.Trim();
            sqlParam[77].Value = _ReportCardEntity.ClinicalStages.Trim();
            sqlParam[78].Value = _ReportCardEntity.ReportDiagfunit.Trim();
            return sqlParam;


            #endregion
        }
        #endregion


        public ReportCardEntity GetInpatientByNoofinpat(string noofinpat)
        {
            ReportCardEntity _ReportCardEntity = new ReportCardEntity();
            SqlParameter[] sqlParam = new SqlParameter[] { new SqlParameter("@Noofinpat", SqlDbType.VarChar) };

            sqlParam[0].Value = noofinpat;
            string sqlText = "select i.iem_mainpage_no,i.XZZ_TEL, i.NOOFINPAT,i.NAME,i.SEXID,i.BIRTH,i.MARITAL,i.JOBID,i.NATIONID,i.IDNO,i.ORGANIZATION,i.OFFICEPLACE,i.OFFICETEL,i.XZZ_PROVINCEID,i.XZZ_CITYID,i.XZZ_DISTRICTID,i.XZZ_PROVINCENAME, XZZ_CITYNAME,XZZ_DISTRICTNAME,HKDZ_PROVINCEID,HKDZ_CITYID,HKDZ_DISTRICTID,HKDZ_PROVINCENAME,HKDZ_CITYNAME,HKDZ_DISTRICTNAME,i.OUTHOSTYPE,n.NOOFCLINIC ,n.patid   from iem_mainpage_basicinfo_2012 i left join inpatient n on i.noofinpat=n.noofinpat  WHERE i.NOOFINPAT=@Noofinpat AND i.valide=1 ";
            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sqlText = "select i.iem_mainpage_no,i.XZZ_TEL, i.NOOFINPAT,i.NAME,i.SEXID,i.BIRTH,i.MARITAL,i.JOBID,i.NATIONID,i.IDNO,i.ORGANIZATION,i.OFFICEPLACE,i.OFFICETEL,i.XZZ_PROVINCEID,i.XZZ_CITYID,i.XZZ_DISTRICTID,i.XZZ_PROVINCENAME, XZZ_CITYNAME,XZZ_DISTRICTNAME,HKDZ_PROVINCEID,HKDZ_CITYID,HKDZ_DISTRICTID,HKDZ_PROVINCENAME,HKDZ_CITYNAME,HKDZ_DISTRICTNAME,i.OUTHOSTYPE,n.NOOFCLINIC ,n.patid   from iem_mainpage_basicinfo_sx i left join inpatient n on i.noofinpat=n.noofinpat  WHERE i.NOOFINPAT=@Noofinpat AND i.valide=1 ";

            }
            DataTable dt = m_IDataAccess.ExecuteDataTable(sqlText, sqlParam, CommandType.Text);

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    _ReportCardEntity.Report_Noofinpat = dr["NOOFINPAT"].ToString();//患者编号
                    _ReportCardEntity.Report_ClinicId = dr["NOOFCLINIC"].ToString(); //报告卡门诊号
                    _ReportCardEntity.Report_PatId = dr["PATID"].ToString();//住院号
                    _ReportCardEntity.Report_INDO = dr["IDNO"].ToString();//身份证号
                    _ReportCardEntity.Report_InpatName = dr["NAME"].ToString();//姓名
                    _ReportCardEntity.SexId = dr["sexid"].ToString();//性别
                    //_ReportCardEntity.RealAge = dr["age"].ToString();//年龄
                    _ReportCardEntity.BirthDate = dr["birth"].ToString();//生日
                    _ReportCardEntity.NationId = dr["NATIONID"].ToString();//名族
                    _ReportCardEntity.ContactTel = dr["XZZ_TEL"].ToString();//家庭电话
                    _ReportCardEntity.Martial = dr["MARITAL"].ToString();//婚姻
                    _ReportCardEntity.OfficeAddress = dr["OFFICEPLACE"].ToString();//地址
                    _ReportCardEntity.Occupation = dr["JOBID"].ToString();//职业

                    _ReportCardEntity.XZZProvinceId = dr["XZZ_PROVINCEID"].ToString();
                    _ReportCardEntity.XZZProvince = dr["XZZ_PROVINCENAME"].ToString();//
                    _ReportCardEntity.XZZCityId = dr["XZZ_CITYID"].ToString();
                    _ReportCardEntity.XZZCity = dr["XZZ_CITYNAME"].ToString();
                    _ReportCardEntity.XZZDistrictId = dr["XZZ_DISTRICTID"].ToString();
                    _ReportCardEntity.XZZDistrict = dr["XZZ_DISTRICTNAME"].ToString();


                    _ReportCardEntity.OrgProvinceId = dr["HKDZ_PROVINCEID"].ToString();
                    _ReportCardEntity.OrgProvinceName = dr["HKDZ_PROVINCENAME"].ToString();
                    _ReportCardEntity.OrgCityId = dr["HKDZ_CITYID"].ToString();
                    _ReportCardEntity.OrgCityName = dr["HKDZ_CITYNAME"].ToString();
                    _ReportCardEntity.OrgDistrictId = dr["HKDZ_DISTRICTID"].ToString();
                    _ReportCardEntity.OrgDistrictName = dr["HKDZ_DISTRICTNAME"].ToString();
                    _ReportCardEntity.Report_OuthosType = dr["OUTHOSTYPE"].ToString();
                    _ReportCardEntity.Report_DistrictId = dr["HKDZ_DISTRICTID"].ToString();
                    _ReportCardEntity.Report_DistrictName = dr["HKDZ_DISTRICTNAME"].ToString();
                }
                return _ReportCardEntity;
            }
        }

        //  add  jxh   新增页面显示
        public ReportCardEntity1 GetInpatientByNoofinpat1(string noofinpat)
        {
            ReportCardEntity1 _ReportCardEntity = new ReportCardEntity1();
            SqlParameter[] sqlParam = new SqlParameter[] { new SqlParameter("@Noofinpat", SqlDbType.VarChar) };

            sqlParam[0].Value = noofinpat;
            string sqlText = "select i.iem_mainpage_no,i.AGE,i.XZZ_TEL, i.NOOFINPAT,i.NAME,i.SEXID,i.BIRTH,i.MARITAL,i.JOBID,i.NATIONID,i.IDNO,i.ORGANIZATION,i.OFFICEPLACE,i.OFFICETEL,i.XZZ_PROVINCEID,i.XZZ_CITYID,i.XZZ_DISTRICTID,i.XZZ_PROVINCENAME, XZZ_CITYNAME,XZZ_DISTRICTNAME,HKDZ_PROVINCEID,HKDZ_CITYID,HKDZ_DISTRICTID,HKDZ_PROVINCENAME,HKDZ_CITYNAME,HKDZ_DISTRICTNAME,i.OUTHOSTYPE,n.NOOFCLINIC ,n.patid,n.isbaby,n.mother   from iem_mainpage_basicinfo_2012 i left join inpatient n on i.noofinpat=n.noofinpat  WHERE i.NOOFINPAT=@Noofinpat AND i.valide=1 ";
            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sqlText = "select i.iem_mainpage_no,i.XZZ_TEL, i.NOOFINPAT,i.NAME,i.SEXID,i.BIRTH,i.MARITAL,i.JOBID,i.NATIONID,i.IDNO,i.ORGANIZATION,i.OFFICEPLACE,i.OFFICETEL,i.XZZ_PROVINCEID,i.XZZ_CITYID,i.XZZ_DISTRICTID,i.XZZ_PROVINCENAME, XZZ_CITYNAME,XZZ_DISTRICTNAME,HKDZ_PROVINCEID,HKDZ_CITYID,HKDZ_DISTRICTID,HKDZ_PROVINCENAME,HKDZ_CITYNAME,HKDZ_DISTRICTNAME,i.OUTHOSTYPE,n.NOOFCLINIC ,n.patid   from iem_mainpage_basicinfo_sx i left join inpatient n on i.noofinpat=n.noofinpat  WHERE i.NOOFINPAT=@Noofinpat AND i.valide=1 ";

            }
            DateTime dtbirth;
            DataTable dt = m_IDataAccess.ExecuteDataTable(sqlText, sqlParam, CommandType.Text);

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                if (dt.Rows[0]["isbaby"].ToString() == "1")
                {
                    string sql = string.Empty;
                    sql = "select i.iem_mainpage_no,i.AGE,i.XZZ_TEL, i.NOOFINPAT,i.NAME,i.SEXID,i.BIRTH,i.MARITAL,i.JOBID,i.NATIONID,i.IDNO,i.ORGANIZATION,i.OFFICEPLACE,i.OFFICETEL,i.XZZ_PROVINCEID,i.XZZ_CITYID,i.XZZ_DISTRICTID,i.XZZ_PROVINCENAME, XZZ_CITYNAME,XZZ_DISTRICTNAME,HKDZ_PROVINCEID,HKDZ_CITYID,HKDZ_DISTRICTID,HKDZ_PROVINCENAME,HKDZ_CITYNAME,HKDZ_DISTRICTNAME,i.OUTHOSTYPE,n.NOOFCLINIC ,n.patid   from iem_mainpage_basicinfo_2012 i left join inpatient n on i.noofinpat=n.noofinpat  WHERE n.Noofinpat={0} AND i.valide=1 ";
                    DataTable dtmother = m_IDataAccess.ExecuteDataTable(string.Format(sql, dt.Rows[0]["mother"]));
                    _ReportCardEntity.MotherName = dtmother.Rows[0]["NAME"].ToString();
                    _ReportCardEntity.MotherPatid = dtmother.Rows[0]["PATID"].ToString();
                    _ReportCardEntity.National = dtmother.Rows[0]["NATIONID"].ToString();
                    _ReportCardEntity.MotherAge = dtmother.Rows[0]["AGE"].ToString();
                    _ReportCardEntity.AddressPost = dtmother.Rows[0]["XZZ_PROVINCENAME"].ToString() + dtmother.Rows[0]["XZZ_CITYNAME"].ToString() + dtmother.Rows[0]["XZZ_DISTRICTNAME"].ToString();
                }

                foreach (DataRow dr in dt.Rows)
                {
                    _ReportCardEntity.ReportNoofinpat = dr["NOOFINPAT"].ToString();//患者编号
                    _ReportCardEntity.ReportId = dr["NOOFCLINIC"].ToString(); //报告卡门诊号
                    _ReportCardEntity.ChildPatid = dr["PATID"].ToString();//住院号                  
                    _ReportCardEntity.ChildName = dr["NAME"].ToString();//姓名
                    if (dr["SEXID"].ToString() != "")
                    {
                        _ReportCardEntity.ChildSex = dr["SEXID"].ToString();
                    }
                    dtbirth = DateTime.Parse(dr["birth"].ToString());
                    _ReportCardEntity.BornYear = dtbirth.Year.ToString();//出生年
                    _ReportCardEntity.BornMonth = dtbirth.Month.ToString();//出生月
                    _ReportCardEntity.BornDay = dtbirth.Day.ToString();//出生日                   
                }
                return _ReportCardEntity;
            }
        }



        public DataTable GetReportCartDataTable(string id, string begintime, string endtime)
        {
            DataTable dataTable = m_IDataAccess.ExecuteDataTable("SELECT t.report_inpatname as Name,t.report_id as keyID,t.report_noofinpat,t.report_no,i.noofinpat as ParentID FROM THERIOMAREPORTCARD t left join inpatient i on t.report_noofinpat=i.noofinpat ORDER BY t.report_noofinpat DESC ");


            return dataTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public List<ReportConfig> GetReportCart(string id, string begintime, string endtime, string state)
        {
            List<ReportConfig> listConfig = new List<ReportConfig>();
            string SqlText = "SELECT i.noofinpat,i.Name FROM inpatient i where 1=1 ";
            if (id == "2")
            {
                SqlText += " AND exists(select * from theriomareportcard t where t.report_noofinpat=i.noofinpat ";
            }
            if (id == "3")
            {
                SqlText += " AND exists(select * from birthdefectscard t where t.report_noofinpat=i.noofinpat ";
            }
            if (id == "4")//脑卒中、冠心病病例 edit by ywk 2013年8月22日 08:51:24
            {
                SqlText += " AND exists(select * from CARDIOVASCULARCARD t where t.NOOFINPAT=i.noofinpat ";
            }

            //if (!string.IsNullOrEmpty(id))
            //{
            //    SqlText += " AND t.REPORT_ID="+"";
            //}
            if (!string.IsNullOrEmpty(begintime))
            {
                SqlText += " AND TO_DATE(t.CREATE_DATE, 'yyyy-mm-dd hh24:mi:ss')>=TO_DATE('" + Convert.ToDateTime(begintime).ToString("yyyy-MM-dd") + " 00:00:00', 'yyyy-mm-dd hh24:mi:ss')";
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                SqlText += " AND TO_DATE(t.CREATE_DATE, 'yyyy-mm-dd hh24:mi:ss')<=TO_DATE('" + Convert.ToDateTime(endtime).ToString("yyyy-MM-dd") + " 23:59:59', 'yyyy-mm-dd hh24:mi:ss')";
            }

            if (!string.IsNullOrEmpty(state))
            {
                switch (state)
                {
                    case "已保存":
                        SqlText += " AND t.state=1";
                        break;
                    case "已提交":
                        SqlText += " AND t.state=2";
                        break;
                    case "已审核":
                        SqlText += " AND t.state=4";
                        break;
                    case "已否决":
                        SqlText += " AND t.state=5";
                        break;

                }
            }

            SqlText += ")  ORDER BY  i.noofinpat DESC";
            DataTable dataTable = m_IDataAccess.ExecuteDataTable(SqlText);
            foreach (DataRow dr in dataTable.Rows)
            {
                ReportConfig config = new ReportConfig();
                config.Name = dr["Name"].ToString();
                List<ReportCategory> listCategory = new List<ReportCategory>();

                if (id == "2")//肿瘤报告卡
                {
                    DataTable dataTableInfo = m_IDataAccess.ExecuteDataTable("SELECT t.report_inpatname as Name,t.report_id as ID,t.report_noofinpat,t.report_no,t.state FROM THERIOMAREPORTCARD t where   t.report_noofinpat=" + dr["noofinpat"].ToString() + " ORDER BY  CREATE_DATE ASC");
                    ReportCategory listC = new ReportCategory();
                    listC.NAME = "肿瘤报告卡";
                    List<ReportCategoryInfo> listRci = new List<ReportCategoryInfo>();
                    for (int i = 0; i < dataTableInfo.Rows.Count; i++)
                    {
                        ReportCategoryInfo info = new ReportCategoryInfo();
                        info.NAME = "第" + (i + 1).ToString() + "次";
                        info.ID = dataTableInfo.Rows[i]["ID"].ToString();
                        info.Noofinpat = dataTableInfo.Rows[i]["report_noofinpat"].ToString();
                        info.State = StateName(dataTableInfo.Rows[i]["state"].ToString());
                        listRci.Add(info);
                    }
                    listC.ReportCategoryInfo = listRci;
                    listCategory.Add(listC);
                    config.ReportCategory = listCategory;
                    listConfig.Add(config);
                }
                if (id == "3")//出生缺陷报告卡
                {
                    DataTable dataTableInfo = m_IDataAccess.ExecuteDataTable("SELECT t.mother_name as Name,t.id as ID,t.report_noofinpat,t.report_id,t.state FROM birthdefectscard t where   t.report_noofinpat=" + dr["noofinpat"].ToString() + " ORDER BY  CREATE_DATE ASC");
                    ReportCategory listC = new ReportCategory();
                    listC.NAME = "出生缺陷报告卡";
                    List<ReportCategoryInfo> listRci = new List<ReportCategoryInfo>();
                    for (int i = 0; i < dataTableInfo.Rows.Count; i++)
                    {
                        ReportCategoryInfo info = new ReportCategoryInfo();
                        info.NAME = "第" + (i + 1).ToString() + "次";
                        info.ID = dataTableInfo.Rows[i]["ID"].ToString();
                        info.Noofinpat = dataTableInfo.Rows[i]["report_noofinpat"].ToString();
                        info.State = StateName(dataTableInfo.Rows[i]["state"].ToString());
                        listRci.Add(info);
                    }
                    listC.ReportCategoryInfo = listRci;
                    listCategory.Add(listC);
                    config.ReportCategory = listCategory;
                    listConfig.Add(config);
                }
                if (id == "4")
                {
                    //脑卒中、冠心病病例
                    DataTable dataTableInfo = m_IDataAccess.ExecuteDataTable("SELECT t.name as Name,t.id as ID,t.noofinpat,t.REPORT_NO,t.state FROM CARDIOVASCULARCARD t where   t.noofinpat=" + dr["noofinpat"].ToString() + " ORDER BY  CREATE_DATE ASC");
                    ReportCategory listC = new ReportCategory();
                    listC.NAME = "心脑血管病发病报告卡";
                    List<ReportCategoryInfo> listRci = new List<ReportCategoryInfo>();
                    for (int i = 0; i < dataTableInfo.Rows.Count; i++)
                    {
                        ReportCategoryInfo info = new ReportCategoryInfo();
                        info.NAME = "第" + (i + 1).ToString() + "次";
                        info.ID = dataTableInfo.Rows[i]["ID"].ToString();
                        info.Noofinpat = dataTableInfo.Rows[i]["noofinpat"].ToString();
                        info.State = StateName(dataTableInfo.Rows[i]["state"].ToString());
                        listRci.Add(info);
                    }
                    listC.ReportCategoryInfo = listRci;
                    listCategory.Add(listC);
                    config.ReportCategory = listCategory;
                    listConfig.Add(config);
                }
            }
            return listConfig;
        }

        public static string StateName(string state)
        {
            string states = string.Empty;
            switch (state)
            {
                case "1": states = "新增保存"; break;
                case "2": states = "提交"; break;
                case "3": states = "撤回"; break;
                case "4": states = "审核通过"; break;
                case "5": states = "否决"; break;
                case "6": states = "上报"; break;
                case "7": states = "作废"; break;
            }
            return states;
        }

        public DataTable GetReportCardInfo(string reportID)
        {
            string sql = @"SELECT * FROM THERIOMAREPORTCARD WHERE THERIOMAREPORTCARD.report_id = '{0}'";
            return m_IDataAccess.ExecuteDataTable(string.Format(sql, reportID));
        }
        /// <summary>
        /// 返回符合肿瘤病的列表
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetDiagnosis(string noofinpat)
        {
            //            string m_SqlGetAllDiagnosis = @"
            //                                          select * from {1} ie    
            //                                            left join zymosis_diagnosis z on ie.diagnosis_code=z.icd  and z.CATEGORYID=2  
            //                                            left join {0} imb on ie.iem_mainpage_no=imb.iem_mainpage_no                    
            //                                            where 1=1  and z.valid=1  and imb.valide=1 and ie.valide=1 
            //                                            AND ( SELECT COUNT(*) FROM theriomareportcard zr where 1=1 and zr.diagicd10=z.icd  
            //                                            and zr.REPORT_NOOFINPAT=imb.noofinpat and zr.diagicd10=z.icd and zr.vaild=1)< z.upcount
            //                                            AND  imb.noofinpat={2}
            //                                        ";

            string m_SqlGetAllDiagnosis = @"
                select * from {1} ie    
                left join zymosis_diagnosis z on ie.diagnosis_code=z.icd  and z.CATEGORYID=2 and valid = '1'
                left join {0} imb on ie.iem_mainpage_no=imb.iem_mainpage_no and imb.valide = '1'
                where z.valid = 1 and imb.valide = 1 and ie.valide = 1 
                AND ( 
                    select count(1) from theriomareportcard zr where zr.diagicd10=z.icd  
                    and zr.REPORT_NOOFINPAT=imb.noofinpat and zr.diagicd10=z.icd and zr.vaild=1 and zr.state != '7'
                    )< z.upcount
                AND  imb.noofinpat={2}
            ";

            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string sql = string.Format(m_SqlGetAllDiagnosis, "iem_mainpage_basicinfo_2012", "iem_mainpage_diagnosis_2012", noofinpat);
            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sql = string.Format(m_SqlGetAllDiagnosis, "iem_mainpage_basicinfo_sx", "iem_mainpage_diagnosis_sx", noofinpat);
            }
            return m_IDataAccess.ExecuteDataTable(sql);
        }

        //  add  by  jxh 
        private SqlParameter[] GetSqlParameters(ReportCardEntity1 _ReportCardEntity, string editType)
        {
            #region
            SqlParameter[] sqlParam = new SqlParameter[] {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.Decimal),
                    new SqlParameter("@REPORT_NOOFINPAT",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_ID",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_CODE",SqlDbType.VarChar), 
                    new SqlParameter("@STRING3",SqlDbType.VarChar), 
                    new SqlParameter("@STRING4",SqlDbType.VarChar), 
                    new SqlParameter("@STRING5",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_PROVINCE",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_CITY",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_TOWN",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_VILLAGE",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_HOSPITAL",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_NO",SqlDbType.VarChar),
                    new SqlParameter("@MOTHER_PATID",SqlDbType.VarChar),
                    new SqlParameter("@MOTHER_NAME",SqlDbType.VarChar),
                    new SqlParameter("@MOTHER_AGE",SqlDbType.VarChar),
                    new SqlParameter("@NATIONAL",SqlDbType.VarChar),
                    new SqlParameter("@ADDRESS_POST",SqlDbType.VarChar),
                    new SqlParameter("@PREGNANTNO",SqlDbType.VarChar),
                    new SqlParameter("@PRODUCTIONNO",SqlDbType.VarChar),
                    new SqlParameter("@LOCALADD",SqlDbType.VarChar),
                    new SqlParameter("@PERCAPITAINCOME",SqlDbType.VarChar),
                    new SqlParameter("@EDUCATIONLEVEL",SqlDbType.VarChar),
                    new SqlParameter("@CHILD_PATID",SqlDbType.VarChar),
                    new SqlParameter("@CHILD_NAME",SqlDbType.VarChar),
                    new SqlParameter("@ISBORNHERE",SqlDbType.VarChar),
                    new SqlParameter("@CHILD_SEX",SqlDbType.VarChar),
                    new SqlParameter("@BORN_YEAR",SqlDbType.VarChar),
                    new SqlParameter("@BORN_MONTH",SqlDbType.VarChar),
                    new SqlParameter("@BORN_DAY",SqlDbType.VarChar),
                    new SqlParameter("@GESTATIONALAGE",SqlDbType.VarChar),
                    new SqlParameter("@WEIGHT",SqlDbType.VarChar),
                    new SqlParameter("@BIRTHS",SqlDbType.VarChar),
                    new SqlParameter("@ISIDENTICAL",SqlDbType.VarChar),
                    new SqlParameter("@OUTCOME",SqlDbType.VarChar),
                    new SqlParameter("@INDUCEDLABOR",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNOSTICBASIS",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNOSTICBASIS1",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNOSTICBASIS2",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNOSTICBASIS3",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNOSTICBASIS4",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNOSTICBASIS5",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNOSTICBASIS6",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNOSTICBASIS7",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_ANENCEPHALY",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_SPINA",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_PENGOUT",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_HYDROCEPHALUS",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_CLEFTPALATE",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_CLEFTLIP",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_CLEFTMERGER",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_SMALLEARS",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_OUTEREAR",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_ESOPHAGEAL",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_RECTUM",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_HYPOSPADIAS",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_BLADDER",SqlDbType.VarChar),
                    /*原诊断begin */  
                    new SqlParameter("@DIAG_HORSESHOEFOOTLEFT",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_HORSESHOEFOOTRIGHT",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_MANYPOINTLEFT",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_MANYPOINTRIGHT",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_LIMBSUPPERLEFT",SqlDbType.VarChar),//患者ID\
                    /*end*/
                    //报告状态
                    new SqlParameter("@DIAG_LIMBSUPPERRIGHT",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_LIMBSLOWERLEFT",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_LIMBSLOWERRIGHT",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_HERNIA",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_BULGINGBELLY",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_GASTROSCHISIS",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_TWINS",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_TSSYNDROME",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_HEARTDISEASE",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_OTHER",SqlDbType.VarChar),
                    new SqlParameter("@DIAG_OTHERCONTENT",SqlDbType.VarChar),
                    new SqlParameter("@FEVER",SqlDbType.VarChar),
                    new SqlParameter("@VIRUSINFECTION",SqlDbType.VarChar),
                    new SqlParameter("@ILLOTHER",SqlDbType.VarChar),
                    new SqlParameter("@SULFA",SqlDbType.VarChar),
                    new SqlParameter("@ANTIBIOTICS",SqlDbType.VarChar),
                    new SqlParameter("@BIRTHCONTROLPILL",SqlDbType.VarChar),
                    new SqlParameter("@SEDATIVES",SqlDbType.VarChar),
                    new SqlParameter("@MEDICINEOTHER",SqlDbType.VarChar),
                    new SqlParameter("@DRINKING",SqlDbType.VarChar),
                    new SqlParameter("@PESTICIDE",SqlDbType.VarChar),
                    new SqlParameter("@RAY",SqlDbType.VarChar),
                    new SqlParameter("@CHEMICALAGENTS",SqlDbType.VarChar),
                    new SqlParameter("@FACTOROTHER",SqlDbType.VarChar),
                    new SqlParameter("@STILLBIRTHNO",SqlDbType.VarChar),
                    new SqlParameter("@ABORTIONNO",SqlDbType.VarChar),
                    new SqlParameter("@DEFECTSNO",SqlDbType.VarChar),
                    new SqlParameter("@DEFECTSOF1",SqlDbType.VarChar),
                    new SqlParameter("@DEFECTSOF2",SqlDbType.VarChar),
                    new SqlParameter("@DEFECTSOF3",SqlDbType.VarChar),
                    new SqlParameter("@YCDEFECTSOF1",SqlDbType.VarChar),
                    new SqlParameter("@YCDEFECTSOF2",SqlDbType.VarChar),
                    new SqlParameter("@YCDEFECTSOF3",SqlDbType.VarChar),
                    new SqlParameter("@KINSHIPDEFECTS1",SqlDbType.VarChar),
                    new SqlParameter("@KINSHIPDEFECTS2",SqlDbType.VarChar),
                    new SqlParameter("@KINSHIPDEFECTS3",SqlDbType.VarChar),
                    new SqlParameter("@COUSINMARRIAGE",SqlDbType.VarChar),
                    new SqlParameter("@COUSINMARRIAGEBETWEEN",SqlDbType.VarChar),
                    new SqlParameter("@PREPARER",SqlDbType.VarChar),
                    new SqlParameter("@THETITLE1",SqlDbType.VarChar),
                    new SqlParameter("@FILLDATEYEAR",SqlDbType.VarChar),
                    new SqlParameter("@FILLDATEMONTH",SqlDbType.VarChar),
                    new SqlParameter("@FILLDATEDAY",SqlDbType.VarChar),
                    new SqlParameter("@HOSPITALREVIEW",SqlDbType.VarChar),

                    new SqlParameter("@THETITLE2",SqlDbType.VarChar),
                    new SqlParameter("@HOSPITALAUDITDATEYEAR",SqlDbType.VarChar),
                    new SqlParameter("@HOSPITALAUDITDATEMONTH",SqlDbType.VarChar),
                    new SqlParameter("@HOSPITALAUDITDATEDAY",SqlDbType.VarChar),
                    new SqlParameter("@PROVINCIALVIEW",SqlDbType.VarChar),
                    new SqlParameter("@THETITLE3",SqlDbType.VarChar),
                    new SqlParameter("@PROVINCIALVIEWDATEYEAR",SqlDbType.VarChar),
                    new SqlParameter("@PROVINCIALVIEWDATEMONTH",SqlDbType.VarChar),
                    new SqlParameter("@PROVINCIALVIEWDATEDAY",SqlDbType.VarChar),
                    new SqlParameter("@FEVERNO",SqlDbType.VarChar),
                    new SqlParameter("@ISVIRUSINFECTION",SqlDbType.VarChar),
                    new SqlParameter("@ISDIABETES",SqlDbType.VarChar),
                    new SqlParameter("@ISILLOTHER",SqlDbType.VarChar),
                    new SqlParameter("@ISSULFA",SqlDbType.VarChar),
                    new SqlParameter("@ISANTIBIOTICS",SqlDbType.VarChar),
                    new SqlParameter("@ISBIRTHCONTROLPILL",SqlDbType.VarChar),
                    new SqlParameter("@ISSEDATIVES",SqlDbType.VarChar),
                    new SqlParameter("@ISMEDICINEOTHER",SqlDbType.VarChar),
                    new SqlParameter("@ISDRINKING",SqlDbType.VarChar),
                    new SqlParameter("@ISPESTICIDE",SqlDbType.VarChar),
                    new SqlParameter("@ISRAY",SqlDbType.VarChar),
                    new SqlParameter("@ISCHEMICALAGENTS",SqlDbType.VarChar),

                    new SqlParameter("@ISFACTOROTHER",SqlDbType.VarChar),
                    new SqlParameter("@STATE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DATE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_USERCODE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_USERNAME",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DEPTCODE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DEPTNAME",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_DATE",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_USERCODE",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_USERNAME",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_DEPTCODE",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_DEPTNAME",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_DATE",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_USERCODE",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_USERNAME",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_DEPTCODE",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_DEPTNAME",SqlDbType.VarChar),
                    new SqlParameter("@VAILD",SqlDbType.VarChar),
                    new SqlParameter("@CANCELREASON",SqlDbType.VarChar),
                    new SqlParameter("@PRENATAL",SqlDbType.VarChar),
                    new SqlParameter("@PRENATALNO",SqlDbType.VarChar),
                    new SqlParameter("@POSTPARTUM",SqlDbType.VarChar),
                    new SqlParameter("@ANDTOSHOWLEFT",SqlDbType.VarChar),
                    new SqlParameter("@ANDTOSHOWRIGHT",SqlDbType.VarChar)
            };
            sqlParam[0].Value = editType;
            sqlParam[1].Value = _ReportCardEntity.Id.Trim();
            sqlParam[2].Value = _ReportCardEntity.ReportNoofinpat.Trim();
            sqlParam[3].Value = _ReportCardEntity.ReportId.Trim();
            sqlParam[4].Value = _ReportCardEntity.DiagCode.Trim();
            sqlParam[5].Value = _ReportCardEntity.String3.Trim();
            sqlParam[6].Value = _ReportCardEntity.String4.Trim();
            sqlParam[7].Value = _ReportCardEntity.String5.Trim();
            sqlParam[8].Value = _ReportCardEntity.ReportProvince.Trim();

            sqlParam[9].Value = _ReportCardEntity.ReportCity.Trim();
            sqlParam[10].Value = _ReportCardEntity.ReportTown.Trim();
            sqlParam[11].Value = _ReportCardEntity.ReportVillage.Trim();
            sqlParam[12].Value = _ReportCardEntity.ReportHospital.Trim();
            sqlParam[13].Value = _ReportCardEntity.ReportNo.Trim();
            sqlParam[14].Value = _ReportCardEntity.MotherPatid.Trim();
            sqlParam[15].Value = _ReportCardEntity.MotherName.Trim();
            sqlParam[16].Value = _ReportCardEntity.MotherAge.Trim();

            sqlParam[17].Value = _ReportCardEntity.National.Trim();
            sqlParam[18].Value = _ReportCardEntity.AddressPost.Trim();
            sqlParam[19].Value = _ReportCardEntity.Pregnantno.Trim();
            sqlParam[20].Value = _ReportCardEntity.Productionno.Trim();
            sqlParam[21].Value = _ReportCardEntity.Localadd.Trim();
            sqlParam[22].Value = _ReportCardEntity.Percapitaincome.Trim();
            sqlParam[23].Value = _ReportCardEntity.Educationlevel.Trim();
            sqlParam[24].Value = _ReportCardEntity.ChildPatid.Trim();


            sqlParam[25].Value = _ReportCardEntity.ChildName.Trim();
            sqlParam[26].Value = _ReportCardEntity.Isbornhere.Trim();

            sqlParam[27].Value = _ReportCardEntity.ChildSex.Trim();
            sqlParam[28].Value = _ReportCardEntity.BornYear.Trim();
            sqlParam[29].Value = _ReportCardEntity.BornMonth.Trim();

            sqlParam[30].Value = _ReportCardEntity.BornDay.Trim();
            sqlParam[31].Value = _ReportCardEntity.Gestationalage.Trim();
            sqlParam[32].Value = _ReportCardEntity.Weight.Trim();
            sqlParam[33].Value = _ReportCardEntity.Births.Trim();
            sqlParam[34].Value = _ReportCardEntity.Isidentical.Trim();

            sqlParam[35].Value = _ReportCardEntity.Outcome.Trim();
            sqlParam[36].Value = _ReportCardEntity.Inducedlabor.Trim();


            sqlParam[37].Value = _ReportCardEntity.Diagnosticbasis.Trim();
            sqlParam[38].Value = _ReportCardEntity.Diagnosticbasis1.Trim();
            sqlParam[39].Value = _ReportCardEntity.Diagnosticbasis2.Trim();


            sqlParam[40].Value = _ReportCardEntity.Diagnosticbasis3.Trim();
            sqlParam[41].Value = _ReportCardEntity.Diagnosticbasis4.Trim();
            sqlParam[42].Value = _ReportCardEntity.Diagnosticbasis5.Trim();
            sqlParam[43].Value = _ReportCardEntity.Diagnosticbasis6.Trim();
            sqlParam[44].Value = _ReportCardEntity.Diagnosticbasis7.Trim();


            sqlParam[45].Value = _ReportCardEntity.DiagAnencephaly.Trim();
            sqlParam[46].Value = _ReportCardEntity.DiagSpina.Trim();


            sqlParam[47].Value = _ReportCardEntity.DiagPengout.Trim();

            sqlParam[48].Value = _ReportCardEntity.DiagHydrocephalus.Trim();
            sqlParam[49].Value = _ReportCardEntity.DiagCleftpalate.Trim();
            sqlParam[50].Value = _ReportCardEntity.DiagCleftlip.Trim();

            sqlParam[51].Value = _ReportCardEntity.DiagCleftmerger.Trim();

            sqlParam[52].Value = _ReportCardEntity.DiagSmallears.Trim();
            sqlParam[53].Value = _ReportCardEntity.DiagOuterear.Trim();
            sqlParam[54].Value = _ReportCardEntity.DiagEsophageal.Trim();

            sqlParam[55].Value = _ReportCardEntity.DiagRectum.Trim();
            sqlParam[56].Value = _ReportCardEntity.DiagHypospadias.Trim();

            sqlParam[57].Value = _ReportCardEntity.DiagBladder.Trim();

            sqlParam[58].Value = _ReportCardEntity.DiagHorseshoefootleft.Trim();
            sqlParam[59].Value = _ReportCardEntity.DiagHorseshoefootright.Trim();
            sqlParam[60].Value = _ReportCardEntity.DiagManypointleft.Trim();

            sqlParam[61].Value = _ReportCardEntity.DiagManypointright.Trim();
            sqlParam[62].Value = _ReportCardEntity.DiagLimbsupperleft.Trim();

            sqlParam[63].Value = _ReportCardEntity.DiagLimbsupperright.Trim();

            sqlParam[64].Value = _ReportCardEntity.DiagLimbslowerleft.Trim();
            sqlParam[65].Value = _ReportCardEntity.DiagLimbslowerright.Trim();
            sqlParam[66].Value = _ReportCardEntity.DiagHernia.Trim();

            sqlParam[67].Value = _ReportCardEntity.DiagBulgingbelly.Trim();
            sqlParam[68].Value = _ReportCardEntity.DiagGastroschisis.Trim();

            sqlParam[69].Value = _ReportCardEntity.DiagTwins.Trim();
            sqlParam[70].Value = _ReportCardEntity.DiagTssyndrome.Trim();
            sqlParam[71].Value = _ReportCardEntity.DiagHeartdisease.Trim();
            sqlParam[72].Value = _ReportCardEntity.DiagOther.Trim();
            sqlParam[73].Value = _ReportCardEntity.DiagOthercontent.Trim();

            sqlParam[74].Value = _ReportCardEntity.Fever.Trim();
            sqlParam[75].Value = _ReportCardEntity.Virusinfection.Trim();
            sqlParam[76].Value = _ReportCardEntity.Illother.Trim();

            sqlParam[77].Value = _ReportCardEntity.Sulfa.Trim();
            sqlParam[78].Value = _ReportCardEntity.Antibiotics.Trim();
            sqlParam[79].Value = _ReportCardEntity.Birthcontrolpill.Trim();
            sqlParam[80].Value = _ReportCardEntity.Sedatives.Trim();
            sqlParam[81].Value = _ReportCardEntity.Medicineother.Trim();

            sqlParam[82].Value = _ReportCardEntity.Drinking.Trim();
            sqlParam[83].Value = _ReportCardEntity.Pesticide.Trim();
            sqlParam[84].Value = _ReportCardEntity.Ray.Trim();

            sqlParam[85].Value = _ReportCardEntity.Chemicalagents.Trim();

            sqlParam[86].Value = _ReportCardEntity.Factorother.Trim();
            sqlParam[87].Value = _ReportCardEntity.Stillbirthno.Trim();
            sqlParam[88].Value = _ReportCardEntity.Abortionno.Trim();

            sqlParam[89].Value = _ReportCardEntity.Defectsno.Trim();

            sqlParam[90].Value = _ReportCardEntity.Defectsof1.Trim();
            sqlParam[91].Value = _ReportCardEntity.Defectsof2.Trim();
            sqlParam[92].Value = _ReportCardEntity.Defectsof3.Trim();

            sqlParam[93].Value = _ReportCardEntity.Ycdefectsof1.Trim();
            sqlParam[94].Value = _ReportCardEntity.Ycdefectsof2.Trim();

            sqlParam[95].Value = _ReportCardEntity.Ycdefectsof3.Trim();

            sqlParam[96].Value = _ReportCardEntity.Kinshipdefects1.Trim();
            sqlParam[97].Value = _ReportCardEntity.Kinshipdefects2.Trim();
            sqlParam[98].Value = _ReportCardEntity.Kinshipdefects3.Trim();

            sqlParam[99].Value = _ReportCardEntity.Cousinmarriage.Trim();
            sqlParam[100].Value = _ReportCardEntity.Cousinmarriagebetween.Trim();

            sqlParam[101].Value = _ReportCardEntity.Preparer.Trim();

            sqlParam[102].Value = _ReportCardEntity.Thetitle1.Trim();
            sqlParam[103].Value = _ReportCardEntity.Filldateyear.Trim();
            sqlParam[104].Value = _ReportCardEntity.Filldatemonth.Trim();

            sqlParam[105].Value = _ReportCardEntity.Filldateday.Trim();
            sqlParam[106].Value = _ReportCardEntity.Hospitalreview.Trim();

            sqlParam[107].Value = _ReportCardEntity.Thetitle2.Trim();
            sqlParam[108].Value = _ReportCardEntity.Hospitalauditdateyear.Trim();
            sqlParam[109].Value = _ReportCardEntity.Hospitalauditdatemonth.Trim();
            sqlParam[110].Value = _ReportCardEntity.Hospitalauditdateday.Trim();
            sqlParam[111].Value = _ReportCardEntity.Provincialview.Trim();

            sqlParam[112].Value = _ReportCardEntity.Thetitle3.Trim();
            sqlParam[113].Value = _ReportCardEntity.Provincialviewdateyear.Trim();
            sqlParam[114].Value = _ReportCardEntity.Provincialviewdatemonth.Trim();

            sqlParam[115].Value = _ReportCardEntity.Provincialviewdateday.Trim();
            sqlParam[116].Value = _ReportCardEntity.Feverno.Trim();
            sqlParam[117].Value = _ReportCardEntity.Isvirusinfection.Trim();
            sqlParam[118].Value = _ReportCardEntity.Isdiabetes.Trim();
            sqlParam[119].Value = _ReportCardEntity.Isillother.Trim();
            sqlParam[120].Value = _ReportCardEntity.Issulfa.Trim();
            sqlParam[121].Value = _ReportCardEntity.Isantibiotics.Trim();
            sqlParam[122].Value = _ReportCardEntity.Isbirthcontrolpill.Trim();
            sqlParam[123].Value = _ReportCardEntity.Issedatives.Trim();
            sqlParam[124].Value = _ReportCardEntity.Ismedicineother.Trim();


            sqlParam[125].Value = _ReportCardEntity.Isdrinking.Trim();
            sqlParam[126].Value = _ReportCardEntity.Ispesticide.Trim();

            sqlParam[127].Value = _ReportCardEntity.Isray.Trim();
            sqlParam[128].Value = _ReportCardEntity.Ischemicalagents.Trim();
            sqlParam[129].Value = _ReportCardEntity.Isfactorother.Trim();

            sqlParam[130].Value = _ReportCardEntity.State.Trim();
            sqlParam[131].Value = _ReportCardEntity.CreateDate.Trim();
            sqlParam[132].Value = _ReportCardEntity.CreateUsercode.Trim();
            sqlParam[133].Value = _ReportCardEntity.CreateUsername.Trim();
            sqlParam[134].Value = _ReportCardEntity.CreateDeptcode.Trim();

            sqlParam[135].Value = _ReportCardEntity.CreateDeptname.Trim();
            sqlParam[136].Value = _ReportCardEntity.ModifyDate.Trim();


            sqlParam[137].Value = _ReportCardEntity.ModifyUsercode.Trim();
            sqlParam[138].Value = _ReportCardEntity.ModifyUsername.Trim();
            sqlParam[139].Value = _ReportCardEntity.ModifyDeptcode.Trim();


            sqlParam[140].Value = _ReportCardEntity.ModifyDeptname.Trim();
            sqlParam[141].Value = _ReportCardEntity.AuditDate.Trim();
            sqlParam[142].Value = _ReportCardEntity.AuditUsercode.Trim();
            sqlParam[143].Value = _ReportCardEntity.AuditUsername.Trim();
            sqlParam[144].Value = _ReportCardEntity.AuditDeptcode.Trim();


            sqlParam[145].Value = _ReportCardEntity.AuditDeptname.Trim();
            sqlParam[146].Value = _ReportCardEntity.Vaild.Trim();


            sqlParam[147].Value = _ReportCardEntity.Cancelreason.Trim();

            sqlParam[148].Value = _ReportCardEntity.Prenatal.Trim();
            sqlParam[149].Value = _ReportCardEntity.Prenatalno.Trim();
            sqlParam[150].Value = _ReportCardEntity.Postpartum.Trim();

            sqlParam[151].Value = _ReportCardEntity.Andtoshowleft.Trim();

            sqlParam[152].Value = _ReportCardEntity.Andtoshowright.Trim();
            return sqlParam;


            #endregion
        }
        // add  jxh 缺陷卡修改
        public string EditbirthdefectsReportCard(ReportCardEntity1 _ReportCardEntity)
        {

            SqlParameter[] sqlParam = GetSqlParameters(_ReportCardEntity, "2");

            return m_IDataAccess.ExecuteDataSet("Emr_Zymosis_Report.usp_EditTbirthdefects_Report", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();

        }
        #region 脑卒中报告卡数据库相关操作

        /// <summary>
        /// 根据病人首页序号，获得病人基本信息，预先填进脑卒中报告卡栏位
        ///  add by ywk 2013年8月20日20:41:12 
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public CardiovasularEntity GetCardInpatientByNoofinpat(string noofinpat)
        {

            CardiovasularEntity m_CardiovasularEntity = new CardiovasularEntity();

            //先针对湖北首页
            SqlParameter[] sqlParam = new SqlParameter[] { new SqlParameter("@Noofinpat", SqlDbType.VarChar) };

            sqlParam[0].Value = noofinpat;
            string sqlText = "select i.iem_mainpage_no,i.ORGANIZATION,i.CONTACTTEL,i.XZZ_TEL, i.NOOFINPAT,i.NAME,i.SEXID,i.BIRTH,i.MARITAL,i.JOBID,i.NATIONID,i.IDNO,i.ORGANIZATION,i.OFFICEPLACE,i.OFFICETEL,i.XZZ_PROVINCEID,i.XZZ_CITYID,i.XZZ_DISTRICTID,i.XZZ_PROVINCENAME, XZZ_CITYNAME,XZZ_DISTRICTNAME,HKDZ_PROVINCEID,HKDZ_CITYID,HKDZ_DISTRICTID,HKDZ_PROVINCENAME,HKDZ_CITYNAME,HKDZ_DISTRICTNAME,i.OUTHOSTYPE,n.NOOFCLINIC ,n.patid,n.agestr   from iem_mainpage_basicinfo_2012 i left join inpatient n on i.noofinpat=n.noofinpat  WHERE i.NOOFINPAT=@Noofinpat AND i.valide=1 ";
            DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sqlText, sqlParam, CommandType.Text);
            if (dt != null && dt.Rows.Count > 0)
            {
                m_CardiovasularEntity.NOOFINPAT = dt.Rows[0]["NOOFINPAT"].ToString();
                m_CardiovasularEntity.AGE = dt.Rows[0]["AGESTR"].ToString();
                m_CardiovasularEntity.NOOFCLINIC = dt.Rows[0]["NOOFCLINIC"].ToString();
                m_CardiovasularEntity.PATID = dt.Rows[0]["PATID"].ToString();
                m_CardiovasularEntity.NAME = dt.Rows[0]["NAME"].ToString();
                m_CardiovasularEntity.IDNO = dt.Rows[0]["IDNO"].ToString();
                m_CardiovasularEntity.SEXID = dt.Rows[0]["SEXID"].ToString();
                m_CardiovasularEntity.BIRTH = dt.Rows[0]["BIRTH"].ToString();
                m_CardiovasularEntity.NationId = dt.Rows[0]["NATIONID"].ToString();
                m_CardiovasularEntity.JOBID = dt.Rows[0]["JOBID"].ToString();
                m_CardiovasularEntity.OFFICEPLACE = dt.Rows[0]["ORGANIZATION"].ToString();
                m_CardiovasularEntity.CONTACTTEL = dt.Rows[0]["CONTACTTEL"].ToString();//待核实
            }

            return m_CardiovasularEntity;

        }
        /// <summary>
        /// 根据ID主键查询靠谱 脑卒中报告卡
        /// add by ywk 2013年8月20日20:01:17 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CardiovasularEntity GetCardiovasularEntity(string id)
        {

            CardiovasularEntity m_CardiovasularEntity = new CardiovasularEntity();
            //if (m_CardiovasularEntity != null)
            //{
            //    m_CardiovasularEntity = null;//如果原来实体有值，先清空
            //}
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@report_ID",SqlDbType.VarChar)
                };
            sqlParam[0].Value = id;
            string SqlText = string.Format(@"select card.*,
                                           u.name reportuserName,
                                           dd1.name as jobname
                                      from cardiovascularcard card
                                      left join users u on u.id = card.reportusercode
                                                       and u.valid = '1'
                                      left join dictionary_detail dd1 on dd1.detailid = card.jobid
                                      and dd1.categoryid = '41'
                                      and dd1.valid = '1'
                                     WHERE 1 = 1
                                       AND card.id=@report_ID");
            DataTable dataTable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(SqlText, sqlParam, CommandType.Text);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                m_CardiovasularEntity.AGE = dataTable.Rows[0]["AGE"].ToString();
                m_CardiovasularEntity.AUDIT_DATE = dataTable.Rows[0]["AUDIT_DATE"].ToString();
                m_CardiovasularEntity.AUDIT_DEPTCODE = dataTable.Rows[0]["AUDIT_DEPTCODE"].ToString();
                m_CardiovasularEntity.AUDIT_DEPTNAME = dataTable.Rows[0]["AUDIT_DEPTNAME"].ToString();
                m_CardiovasularEntity.AUDIT_USERCODE = dataTable.Rows[0]["AUDIT_USERCODE"].ToString();
                m_CardiovasularEntity.AUDIT_USERNAME = dataTable.Rows[0]["AUDIT_USERNAME"].ToString();
                m_CardiovasularEntity.BIRTH = dataTable.Rows[0]["BIRTH"].ToString();
                m_CardiovasularEntity.CANCELREASON = dataTable.Rows[0]["CANCELREASON"].ToString();
                m_CardiovasularEntity.CARDPARAM1 = dataTable.Rows[0]["CARDPARAM1"].ToString();
                m_CardiovasularEntity.CARDPARAM2 = dataTable.Rows[0]["CARDPARAM2"].ToString();
                m_CardiovasularEntity.CARDPARAM3 = dataTable.Rows[0]["CARDPARAM3"].ToString();
                m_CardiovasularEntity.CARDPARAM4 = dataTable.Rows[0]["CARDPARAM4"].ToString();
                m_CardiovasularEntity.CARDPARAM5 = dataTable.Rows[0]["CARDPARAM5"].ToString();
                m_CardiovasularEntity.CONTACTTEL = dataTable.Rows[0]["CONTACTTEL"].ToString();
                m_CardiovasularEntity.CREATE_DATE = dataTable.Rows[0]["CREATE_DATE"].ToString();
                m_CardiovasularEntity.CREATE_DEPTCODE = dataTable.Rows[0]["CREATE_DEPTCODE"].ToString();
                m_CardiovasularEntity.CREATE_DEPTNAME = dataTable.Rows[0]["CREATE_DEPTNAME"].ToString();
                m_CardiovasularEntity.CREATE_USERCODE = dataTable.Rows[0]["CREATE_USERCODE"].ToString();
                m_CardiovasularEntity.CREATE_USERNAME = dataTable.Rows[0]["CREATE_USERNAME"].ToString();
                m_CardiovasularEntity.DIAGHOSPITAL = dataTable.Rows[0]["DIAGHOSPITAL"].ToString();
                m_CardiovasularEntity.DIAGJXXJGS = dataTable.Rows[0]["DIAGJXXJGS"].ToString();
                m_CardiovasularEntity.DIAGNCX = dataTable.Rows[0]["DIAGNCX"].ToString();
                m_CardiovasularEntity.DIAGNGS = dataTable.Rows[0]["DIAGNGS"].ToString();
                m_CardiovasularEntity.DIAGNOSEDATE = dataTable.Rows[0]["DIAGNOSEDATE"].ToString();
                m_CardiovasularEntity.DIAGNOSISBASED = dataTable.Rows[0]["DIAGNOSISBASED"].ToString();
                m_CardiovasularEntity.DIAGWFLNZZ = dataTable.Rows[0]["DIAGWFLNZZ"].ToString();
                m_CardiovasularEntity.DIAGXXCS = dataTable.Rows[0]["DIAGXXCS"].ToString();
                m_CardiovasularEntity.DIAGZWMXQCX = dataTable.Rows[0]["DIAGZWMXQCX"].ToString();
                m_CardiovasularEntity.DIEDATE = dataTable.Rows[0]["DIEDATE"].ToString();
                m_CardiovasularEntity.HKADDRESSID = dataTable.Rows[0]["HKADDRESSID"].ToString();
                m_CardiovasularEntity.HKCITY = dataTable.Rows[0]["HKCITY"].ToString();
                m_CardiovasularEntity.HKPROVICE = dataTable.Rows[0]["HKPROVICE"].ToString();
                m_CardiovasularEntity.HKSTREET = dataTable.Rows[0]["HKSTREET"].ToString();
                m_CardiovasularEntity.ICD = dataTable.Rows[0]["ICD"].ToString();
                m_CardiovasularEntity.IDNO = dataTable.Rows[0]["IDNO"].ToString();
                m_CardiovasularEntity.ISFIRSTSICK = dataTable.Rows[0]["ISFIRSTSICK"].ToString();
                m_CardiovasularEntity.JOBID = dataTable.Rows[0]["JOBID"].ToString();
                m_CardiovasularEntity.JOBNAME = "";//暂不
                m_CardiovasularEntity.MODIFY_DATE = dataTable.Rows[0]["MODIFY_DATE"].ToString();
                m_CardiovasularEntity.MODIFY_DEPTCODE = dataTable.Rows[0]["MODIFY_DEPTCODE"].ToString();
                m_CardiovasularEntity.MODIFY_DEPTNAME = dataTable.Rows[0]["MODIFY_DEPTNAME"].ToString();
                m_CardiovasularEntity.NAME = dataTable.Rows[0]["NAME"].ToString();
                m_CardiovasularEntity.NationId = dataTable.Rows[0]["NATIONID"].ToString();
                m_CardiovasularEntity.NOOFCLINIC = dataTable.Rows[0]["NOOFCLINIC"].ToString();
                m_CardiovasularEntity.NOOFINPAT = dataTable.Rows[0]["NOOFINPAT"].ToString();
                m_CardiovasularEntity.OFFICEPLACE = dataTable.Rows[0]["OFFICEPLACE"].ToString();
                m_CardiovasularEntity.OUTFLAG = dataTable.Rows[0]["OUTFLAG"].ToString();
                m_CardiovasularEntity.PATID = dataTable.Rows[0]["PATID"].ToString();
                m_CardiovasularEntity.REPORTDATE = dataTable.Rows[0]["REPORTDATE"].ToString();
                m_CardiovasularEntity.REPORTDEPT = dataTable.Rows[0]["REPORTDEPT"].ToString();
                m_CardiovasularEntity.REPORTUSERCODE = dataTable.Rows[0]["REPORTUSERCODE"].ToString();
                m_CardiovasularEntity.REPORTUSERNAME = dataTable.Rows[0]["REPORTUSERNAME"].ToString();
                m_CardiovasularEntity.SEXID = dataTable.Rows[0]["SEXID"].ToString();
                m_CardiovasularEntity.STATE = dataTable.Rows[0]["STATE"].ToString();
                m_CardiovasularEntity.XZZADDRESSID = dataTable.Rows[0]["XZZADDRESSID"].ToString();
                m_CardiovasularEntity.XZZCITY = dataTable.Rows[0]["XZZCITY"].ToString();
                m_CardiovasularEntity.XZZCOMMITTEES = dataTable.Rows[0]["XZZCOMMITTEES"].ToString();
                m_CardiovasularEntity.XZZPARM = dataTable.Rows[0]["XZZPARM"].ToString();
                m_CardiovasularEntity.XZZPROVICE = dataTable.Rows[0]["XZZPROVICE"].ToString();
                m_CardiovasularEntity.XZZSTREET = dataTable.Rows[0]["XZZSTREET"].ToString();
                m_CardiovasularEntity.ReportID = dataTable.Rows[0]["ID"].ToString();
            }
            return m_CardiovasularEntity;
        }
        /// <summary>
        /// 根据实体赋值给参数 脑卒中报告卡
        /// add by ywk 2013年8月20日16:32:36
        /// </summary>
        /// <param name="m_CardiovasularEntity"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public SqlParameter[] GetCardSqlParameter(CardiovasularEntity m_CardiovasularEntity, string editType)
        {
            try
            {
                SqlParameter[] sqlParam = new SqlParameter[] {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@REPORT_NO",SqlDbType.VarChar),
                    new SqlParameter("@NOOFCLINIC",SqlDbType.VarChar),
                    new SqlParameter("@PATID",SqlDbType.VarChar),
                    new SqlParameter("@NAME",SqlDbType.VarChar),
                    new SqlParameter("@IDNO",SqlDbType.VarChar),
                    new SqlParameter("@SEXID",SqlDbType.VarChar),
                    new SqlParameter("@BIRTH",SqlDbType.VarChar),
                    new SqlParameter("@AGE",SqlDbType.VarChar),
                    new SqlParameter("@NATIONID",SqlDbType.VarChar),
                    new SqlParameter("@JOBID",SqlDbType.VarChar),
                    new SqlParameter("@OFFICEPLACE",SqlDbType.VarChar),
                    new SqlParameter("@CONTACTTEL",SqlDbType.VarChar),
                    new SqlParameter("@HKPROVICE",SqlDbType.VarChar),
                    new SqlParameter("@HKCITY",SqlDbType.VarChar),
                    new SqlParameter("@HKSTREET",SqlDbType.VarChar),
                    new SqlParameter("@HKADDRESSID",SqlDbType.VarChar),
                    new SqlParameter("@XZZPROVICE",SqlDbType.VarChar),
                    new SqlParameter("@XZZCITY",SqlDbType.VarChar),
                    new SqlParameter("@XZZSTREET",SqlDbType.VarChar),
                    new SqlParameter("@XZZADDRESSID",SqlDbType.VarChar),
                    new SqlParameter("@XZZCOMMITTEES",SqlDbType.VarChar),
                    new SqlParameter("@XZZPARM",SqlDbType.VarChar),
                    new SqlParameter("@ICD",SqlDbType.VarChar),
                    new SqlParameter("@DIAGZWMXQCX",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNCX",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNGS",SqlDbType.VarChar),
                    new SqlParameter("@DIAGWFLNZZ",SqlDbType.VarChar),
                    new SqlParameter("@DIAGJXXJGS",SqlDbType.VarChar),
                    new SqlParameter("@DIAGXXCS",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNOSISBASED",SqlDbType.VarChar),
                    new SqlParameter("@DIAGNOSEDATE",SqlDbType.VarChar),
                    new SqlParameter("@ISFIRSTSICK",SqlDbType.VarChar),
                    new SqlParameter("@DIAGHOSPITAL",SqlDbType.VarChar),
                    new SqlParameter("@OUTFLAG",SqlDbType.VarChar),
                    new SqlParameter("@DIEDATE",SqlDbType.VarChar),
                    new SqlParameter("@REPORTDEPT",SqlDbType.VarChar),
                    new SqlParameter("@REPORTUSERCODE",SqlDbType.VarChar),
                    new SqlParameter("@REPORTUSERNAME",SqlDbType.VarChar),
                    new SqlParameter("@REPORTDATE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DATE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_USERCODE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_USERNAME",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DEPTCODE",SqlDbType.VarChar),
                    new SqlParameter("@CREATE_DEPTNAME",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_DATE",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_USERCODE",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_USERNAME",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_DEPTCODE",SqlDbType.VarChar),
                    new SqlParameter("@MODIFY_DEPTNAME",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_DATE",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_USERCODE",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_USERNAME",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_DEPTCODE",SqlDbType.VarChar),
                    new SqlParameter("@AUDIT_DEPTNAME",SqlDbType.VarChar),
                    new SqlParameter("@VAILD",SqlDbType.VarChar),
                    new SqlParameter("@CANCELREASON",SqlDbType.VarChar),
                    new SqlParameter("@STATE",SqlDbType.VarChar),
                    new SqlParameter("@CARDPARAM1",SqlDbType.VarChar),
                    new SqlParameter("@CARDPARAM2",SqlDbType.VarChar),
                    new SqlParameter("@CARDPARAM3",SqlDbType.VarChar),
                    new SqlParameter("@CARDPARAM4",SqlDbType.VarChar),
                    new SqlParameter("@CARDPARAM5",SqlDbType.VarChar),
                    new SqlParameter("@NOOFINPAT",SqlDbType.VarChar),
                    new SqlParameter("@REPORTID",SqlDbType.VarChar)
                };
                sqlParam[0].Value = editType;
                sqlParam[1].Value = m_CardiovasularEntity.ReportNo;
                sqlParam[2].Value = m_CardiovasularEntity.NOOFCLINIC;
                sqlParam[3].Value = m_CardiovasularEntity.PATID;
                sqlParam[4].Value = m_CardiovasularEntity.NAME;
                sqlParam[5].Value = m_CardiovasularEntity.IDNO;
                sqlParam[6].Value = m_CardiovasularEntity.SEXID;
                sqlParam[7].Value = m_CardiovasularEntity.BIRTH;
                sqlParam[8].Value = m_CardiovasularEntity.AGE;
                sqlParam[9].Value = m_CardiovasularEntity.NationId;
                sqlParam[10].Value = m_CardiovasularEntity.JOBID;
                sqlParam[11].Value = m_CardiovasularEntity.OFFICEPLACE;
                sqlParam[12].Value = m_CardiovasularEntity.CONTACTTEL;
                sqlParam[13].Value = m_CardiovasularEntity.HKPROVICE;
                sqlParam[14].Value = m_CardiovasularEntity.HKCITY;
                sqlParam[15].Value = m_CardiovasularEntity.HKSTREET;
                sqlParam[16].Value = m_CardiovasularEntity.HKADDRESSID;
                sqlParam[17].Value = m_CardiovasularEntity.XZZPROVICE;
                sqlParam[18].Value = m_CardiovasularEntity.XZZCITY;
                sqlParam[19].Value = m_CardiovasularEntity.XZZSTREET;
                sqlParam[20].Value = m_CardiovasularEntity.XZZADDRESSID;
                sqlParam[21].Value = m_CardiovasularEntity.XZZCOMMITTEES;
                sqlParam[22].Value = m_CardiovasularEntity.XZZPARM;
                sqlParam[23].Value = m_CardiovasularEntity.ICD;
                sqlParam[24].Value = m_CardiovasularEntity.DIAGZWMXQCX;
                sqlParam[25].Value = m_CardiovasularEntity.DIAGNCX;
                sqlParam[26].Value = m_CardiovasularEntity.DIAGNGS;
                sqlParam[27].Value = m_CardiovasularEntity.DIAGWFLNZZ;
                sqlParam[28].Value = m_CardiovasularEntity.DIAGJXXJGS;
                sqlParam[29].Value = m_CardiovasularEntity.DIAGXXCS;
                sqlParam[30].Value = m_CardiovasularEntity.DIAGNOSISBASED;
                sqlParam[31].Value = m_CardiovasularEntity.DIAGNOSEDATE;
                sqlParam[32].Value = m_CardiovasularEntity.ISFIRSTSICK;
                sqlParam[33].Value = m_CardiovasularEntity.DIAGHOSPITAL;
                sqlParam[34].Value = m_CardiovasularEntity.OUTFLAG;
                sqlParam[35].Value = m_CardiovasularEntity.DIEDATE;
                sqlParam[36].Value = m_CardiovasularEntity.REPORTDEPT;
                sqlParam[37].Value = m_CardiovasularEntity.REPORTUSERCODE;
                sqlParam[38].Value = m_CardiovasularEntity.REPORTUSERNAME;
                sqlParam[39].Value = m_CardiovasularEntity.REPORTDATE;
                sqlParam[40].Value = m_CardiovasularEntity.CREATE_DATE;
                sqlParam[41].Value = m_CardiovasularEntity.CREATE_USERCODE;
                sqlParam[42].Value = m_CardiovasularEntity.CREATE_USERNAME;
                sqlParam[43].Value = m_CardiovasularEntity.CREATE_DEPTCODE;
                sqlParam[44].Value = m_CardiovasularEntity.CREATE_DEPTNAME;
                sqlParam[45].Value = m_CardiovasularEntity.MODIFY_DATE;
                sqlParam[46].Value = m_CardiovasularEntity.MODIFY_USERCODE;
                sqlParam[47].Value = m_CardiovasularEntity.MODIFY_USERNAME;
                sqlParam[48].Value = m_CardiovasularEntity.MODIFY_DEPTCODE;
                sqlParam[49].Value = m_CardiovasularEntity.MODIFY_DEPTNAME;
                sqlParam[50].Value = m_CardiovasularEntity.AUDIT_DATE;
                sqlParam[51].Value = m_CardiovasularEntity.AUDIT_USERCODE;
                sqlParam[52].Value = m_CardiovasularEntity.AUDIT_USERNAME;
                sqlParam[53].Value = m_CardiovasularEntity.AUDIT_DEPTCODE;
                sqlParam[54].Value = m_CardiovasularEntity.AUDIT_DEPTNAME;
                sqlParam[55].Value = m_CardiovasularEntity.VAILD;
                sqlParam[56].Value = m_CardiovasularEntity.CANCELREASON;
                sqlParam[57].Value = m_CardiovasularEntity.STATE;
                sqlParam[58].Value = m_CardiovasularEntity.CARDPARAM1;
                sqlParam[59].Value = m_CardiovasularEntity.CARDPARAM2;
                sqlParam[60].Value = m_CardiovasularEntity.CARDPARAM3;
                sqlParam[61].Value = m_CardiovasularEntity.CARDPARAM4;
                sqlParam[62].Value = m_CardiovasularEntity.CARDPARAM5;
                sqlParam[63].Value = m_CardiovasularEntity.NOOFINPAT;
                sqlParam[64].Value = m_CardiovasularEntity.ReportID;
                return sqlParam;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show("出错信息为" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 编辑脑卒中报告卡操作 
        /// </summary>
        /// <param name="m_CardiovasularEntity"></param>
        /// <returns></returns>
        public string EditcardiovascularCard(CardiovasularEntity m_CardiovasularEntity)
        {

            SqlParameter[] sqlParam = GetCardSqlParameter(m_CardiovasularEntity, "2");

            return m_IDataAccess.ExecuteDataSet("Emr_Zymosis_Report.usp_EditCardiovacular_Report", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();

        }


        /// <summary>
        /// 根据实体，将所填的栏位值插入数据库中
        /// add by ywk 2013年8月20日 13:21:14 
        /// </summary>
        /// <param name="m_CardiovasularEntity"></param>
        public string SaveCardiovacular(CardiovasularEntity m_CardiovasularEntity)
        {
            try
            {
                if (m_CardiovasularEntity == null)
                {
                    MyMessageBox.Show("请选择一条病人信息或补录心脑血管病发病报告信息");
                    return "";
                }
                SqlParameter[] sqlParam = GetCardSqlParameter(m_CardiovasularEntity, "1");
                return m_IDataAccess.ExecuteDataTable("Emr_Zymosis_Report.usp_EditCardiovacular_Report", sqlParam, CommandType.StoredProcedure).Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                MyMessageBox.Show("出错信息为" + ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 获得病人的脑卒中报告卡相关诊断信息 
        /// add by ywk 2013年8月26日 16:22:08
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetDiagWithVascular(string noofinpat)
        {
            string m_SqlGetAllDiagnosis = @"
                select * from {1} ie    
                left join zymosis_diagnosis z on ie.diagnosis_code=z.icd  and z.CATEGORYID=4 and valid = '1'
                left join {0} imb on ie.iem_mainpage_no=imb.iem_mainpage_no and imb.valide = '1'
                where z.valid = 1 and imb.valide = 1 and ie.valide = 1 
                AND ( 
                    select count(1) from cardiovascularcard cr where cr.icd=z.icd  
                    and cr.noofinpat=imb.noofinpat  and cr.vaild='1' and cr.state != '7'
                    )< z.upcount
                AND  imb.noofinpat={2}
            ";

            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string sql = string.Format(m_SqlGetAllDiagnosis, "iem_mainpage_basicinfo_2012", "iem_mainpage_diagnosis_2012", noofinpat);
            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sql = string.Format(m_SqlGetAllDiagnosis, "iem_mainpage_basicinfo_sx", "iem_mainpage_diagnosis_sx", noofinpat);
            }
            return m_IDataAccess.ExecuteDataTable(sql);
        }

        /// <summary>
        /// 获得病人的出生缺陷报告卡相关诊断信息 
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetDiagWithBirthDefects(string noofinpat)
        {
            string m_SqlGetAllDiagnosis = @"
                select * from {1} ie    
                left join zymosis_diagnosis z on ie.diagnosis_code=z.icd  and z.CATEGORYID=3 and valid = '1'
                left join {0} imb on ie.iem_mainpage_no=imb.iem_mainpage_no and imb.valide = '1'
                where z.valid = 1 and imb.valide = 1 and ie.valide = 1 
                AND ( 
                    select count(1) from birthdefectscard cr where cr.DIAG_CODE=z.icd  
                    and cr.REPORT_NOOFINPAT=imb.noofinpat  and cr.vaild='1' and cr.state != '7'
                    )< z.upcount
                AND  imb.noofinpat={2}
            ";

            string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string sql = string.Format(m_SqlGetAllDiagnosis, "iem_mainpage_basicinfo_2012", "iem_mainpage_diagnosis_2012", noofinpat);
            if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
            {
                sql = string.Format(m_SqlGetAllDiagnosis, "iem_mainpage_basicinfo_sx", "iem_mainpage_diagnosis_sx", noofinpat);
            }
            return m_IDataAccess.ExecuteDataTable(sql);
        }
        #endregion




    }
}
