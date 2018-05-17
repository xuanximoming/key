using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DrectSoft.Core;
using DrectSoft.Core.IEMMainPage;
using DrectSoft.DSSqlHelper;

namespace DrectSoft.Emr.QcManager
{
    abstract class DataAccess
    {
        static IDataAccess m_SqlHelper;
        private static IEmrHost m_App;
        public static IEmrHost App
        {
            get
            {
                return m_App;
            }
            set
            {
                m_App = value;
                m_SqlHelper = m_App.SqlHelper;
            }
        }
        /// <summary>
        /// 得到病案首页评分标准
        /// </summary>
        /// <returns>iem_mainpage_qc首页评分标准表</returns>
        public static DataTable GetIemMainPageQC()
        {
            string sql = "select * from iem_mainpage_qc where valide = 1";
            return (m_SqlHelper.ExecuteDataTable(sql));
        }
        /// <summary>
        /// 操作病案首页评分标准表
        /// </summary>
        /// <returns>iem_mainpage_qc首页评分标准表</returns>
        public static DataTable OperIemMainPageQC(IemMainpageQC qc, OperType opertype)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@OPERTYPE",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar),
                    new SqlParameter("@TABLETYPE",SqlDbType.VarChar),
                    new SqlParameter("@FIELDS",SqlDbType.VarChar),
                    new SqlParameter("@FIELDSVALUE",SqlDbType.VarChar),
                    new SqlParameter("@CONDITIONTABLETYPE",SqlDbType.VarChar),
                    new SqlParameter("@CONDITIONFIELDS",SqlDbType.VarChar),
                    new SqlParameter("@CONDITIONFIELDSVALUE",SqlDbType.VarChar),
                    new SqlParameter("@REDUCTSCORE",SqlDbType.VarChar),
                    new SqlParameter("@REDUCTREASON",SqlDbType.VarChar),
                    new SqlParameter("@VALIDE",SqlDbType.VarChar)
                };
            sqlParam[0].Value = ((int)opertype).ToString();
            sqlParam[1].Value = qc.ID;
            sqlParam[2].Value = qc.TableType;
            sqlParam[3].Value = qc.Fields;
            sqlParam[4].Value = qc.FieldsValue;
            sqlParam[5].Value = qc.ConditionTableType;
            sqlParam[6].Value = qc.ConditionFields;
            sqlParam[7].Value = qc.ConditionValues;
            sqlParam[8].Value = qc.ReductScore.ToString();
            sqlParam[9].Value = qc.ReductReason;
            sqlParam[10].Value = qc.Valide.ToString();
            return (m_SqlHelper.ExecuteDataTable("iem_main_page.usp_operiem_mainpage_qc", sqlParam, CommandType.StoredProcedure));
        }

        public static string GetConfigValueByKey(string key)
        {
            string sql1 = string.Format(@" select * from appcfg where configkey = '{0}'", key);
            DataTable dt = DS_SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
            string config = string.Empty;
            if (dt.Rows.Count > 0)
            {
                config = dt.Rows[0]["value"].ToString();
            }
            return config;
        }

        /// <summary>
        /// 得到病人病案首页信息
        /// </summary>
        /// <param name="type">表类型0基本信息1诊断2手术3婴儿4病人</param>
        /// <param name="noOfInpat">病人首页序号</param>
        /// <returns>病案首页信息</returns>
        public static DataTable GetIemMainpageInfo(string noOfInpat, int type)
        {
            try
            {
                string sql = "";

                string tablename = GetConfigValueByKey("AutoScoreMainpage");
                string[] mainpage = tablename.Split(',');

                string sql_iem_mainpage_no = string.Format(@"select iem_mainpage_no from {0} where noofinpat = '{1}' and valide = 1", mainpage[0], noOfInpat);

                DataTable dt = m_SqlHelper.ExecuteDataTable(sql_iem_mainpage_no);
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                string iem_mainpage_no = dt.Rows[0][0].ToString();
                switch (type)
                {
                    case 0:
                        sql = string.Format(@"select * from {0} where noofinpat = '{1}'  and valide = 1", mainpage[1], noOfInpat);
                        return m_SqlHelper.ExecuteDataTable(sql);
                    case 1:

                        sql = string.Format(@"select * from {0} where iem_mainpage_no = '{1}' and valide = 1", mainpage[2], iem_mainpage_no);
                        return m_SqlHelper.ExecuteDataTable(sql);
                    case 2:
                        sql = string.Format(@"select * from {0} where iem_mainpage_no = '{1}' and valide = 1", mainpage[3], iem_mainpage_no);
                        return m_SqlHelper.ExecuteDataTable(sql);
                    case 3:
                        sql = "select * from iem_mainpage_obstetricsbaby where iem_mainpage_no = '" + iem_mainpage_no + "' and valide = 1";
                        return m_SqlHelper.ExecuteDataTable(sql);
                    case 4:
                        sql = "select * from inpatient  where noofinpat = '" + noOfInpat + "'";
                        return m_SqlHelper.ExecuteDataTable(sql);
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 得到病人信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="noOfInpat"></param>
        /// <returns></returns>
        public static DataTable GetRedactPatientInfoFrm(string type, string noOfInpat)
        {
            string categoryID = "";
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@FrmType", SqlDbType.VarChar),
                new SqlParameter("@NoOfInpat", SqlDbType.VarChar),
                new SqlParameter("@CategoryID", SqlDbType.VarChar)
            };
            sqlParam[0].Value = type;
            sqlParam[1].Value = noOfInpat;
            sqlParam[2].Value = categoryID;

            return m_SqlHelper.ExecuteDataTable("usp_RedactPatientInfoFrm", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 根据类别得到LookUpEditor中的数据源
        /// </summary>
        /// <param name="noOfInpat"></param>
        /// <param name="typeID"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        public static DataTable GetConsultationData(string noOfInpat, string typeID, string param1)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@NoOfInpat", SqlDbType.Decimal),
                new SqlParameter("@TypeID", SqlDbType.Decimal),
                new SqlParameter("@Param1", SqlDbType.VarChar)
            };

            if (noOfInpat.Trim() == "")
            {
                sqlParam[0].Value = 0f;
            }
            else
            {
                sqlParam[0].Value = Convert.ToDecimal(noOfInpat);
            }
            if (typeID.Trim() == "")
            {
                sqlParam[1].Value = 0f;
            }
            else
            {
                sqlParam[1].Value = Convert.ToDecimal(typeID);
            }
            sqlParam[2].Value = param1;

            return m_SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetConsultationData", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 得到会诊信息
        /// </summary>
        /// <param name="noOfInpat"></param>
        /// <param name="typeID"></param>
        /// <param name="param1"></param>
        /// <returns></returns>
        public static DataSet GetConsultationDataSet(string consultApplySn, string typeID)//, string param1)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ConsultApplySn", SqlDbType.Int),
                new SqlParameter("@TypeID", SqlDbType.Decimal)//,
                //new SqlParameter("@Param1", SqlDbType.VarChar)
            };

            if (consultApplySn.Trim() == "")
            {
                sqlParam[0].Value = 0f;
            }
            else
            {
                sqlParam[0].Value = Convert.ToDecimal(consultApplySn);
            }
            if (typeID.Trim() == "")
            {
                sqlParam[1].Value = 0f;
            }
            else
            {
                sqlParam[1].Value = Convert.ToDecimal(typeID);
            }
            //sqlParam[2].Value = param1;

            return m_SqlHelper.ExecuteDataSet("EMR_CONSULTATION.usp_GetConsultationData", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 得到待审核清单, 会诊清单
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="consultTime"></param>
        /// <param name="consultTypeID"></param>
        /// <param name="urgencyTypeID"></param>
        /// <param name="name"></param>
        /// <param name="patID"></param>
        /// <param name="bedID"></param>
        /// <returns></returns>
        public static DataTable GetConsultationData(string typeID, string consultTimeBegin, string consultTimeEnd, string consultTypeID
            , string urgencyTypeID, string name, string patID, string bedID, string deptID)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@TypeID", SqlDbType.VarChar),                
                new SqlParameter("@ConsultTimeBegin", SqlDbType.VarChar),                
                new SqlParameter("@ConsultTimeEnd", SqlDbType.VarChar),                
                new SqlParameter("@ConsultTypeID", SqlDbType.Decimal),                
                new SqlParameter("@UrgencyTypeID", SqlDbType.Decimal),                
                new SqlParameter("@Name", SqlDbType.VarChar),
                new SqlParameter("@PatID", SqlDbType.VarChar),
                new SqlParameter("@BedID", SqlDbType.VarChar),
                new SqlParameter("@DeptID", SqlDbType.VarChar)
            };

            sqlParam[0].Value = typeID;
            sqlParam[1].Value = consultTimeBegin;
            sqlParam[2].Value = consultTimeEnd;

            if (consultTypeID.Trim() == "")
            {
                sqlParam[3].Value = 0;
            }
            else
            {
                sqlParam[3].Value = consultTypeID;
            }

            if (urgencyTypeID.Trim() == "")
            {
                sqlParam[4].Value = 0;
            }
            else
            {
                sqlParam[4].Value = urgencyTypeID;
            }
            sqlParam[5].Value = name;
            sqlParam[6].Value = patID;
            sqlParam[7].Value = bedID;
            sqlParam[8].Value = deptID;

            return m_SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetConsultationData", sqlParam, CommandType.StoredProcedure);
        }

        ///// <summary>
        ///// 得到待会诊清单
        ///// </summary>
        ///// <param name="typeID"></param>
        ///// <param name="consultTime"></param>
        ///// <param name="consultTypeID"></param>
        ///// <param name="urgencyTypeID"></param>
        ///// <param name="name"></param>
        ///// <param name="patID"></param>
        ///// <param name="bedID"></param>
        ///// <returns></returns>
        //public static DataTable GetConsultationData(string typeID, string consultTime, string consultTypeID
        //    , string urgencyTypeID, string name, string patID, string bedID)
        //{
        //    SqlParameter[] sqlParam = new SqlParameter[] 
        //    { 
        //        new SqlParameter("@TypeID", SqlDbType.VarChar),                            
        //        new SqlParameter("@ConsultTime", SqlDbType.VarChar),                
        //        new SqlParameter("@ConsultTypeID", SqlDbType.Decimal),                
        //        new SqlParameter("@UrgencyTypeID", SqlDbType.Decimal),                
        //        new SqlParameter("@Name", SqlDbType.VarChar),
        //        new SqlParameter("@PatID", SqlDbType.VarChar),
        //        new SqlParameter("@BedID", SqlDbType.VarChar)
        //    };

        //    sqlParam[0].Value = typeID;
        //    sqlParam[1].Value = consultTime;

        //    if (consultTypeID.Trim() == "")
        //    {
        //        sqlParam[2].Value = 0;
        //    }
        //    else
        //    {
        //        sqlParam[2].Value = consultTypeID;
        //    }

        //    if (urgencyTypeID.Trim() == "")
        //    {
        //        sqlParam[3].Value = 0;
        //    }
        //    else
        //    {
        //        sqlParam[3].Value = urgencyTypeID;
        //    }
        //    sqlParam[4].Value = name;
        //    sqlParam[5].Value = patID;
        //    sqlParam[6].Value = bedID;

        //    return m_SqlHelper.ExecuteDataTable("usp_GetConsultationData", sqlParam, CommandType.StoredProcedure);
        //}

        public static string InsertConsultationApply(string typeID, string consultApplySN, string noOfInpat, string urgencyTypeID, string consultTypeID, string abstractContent,
            string purpose, string applyUser, string applyTime, string director, string consultTime, string consultLocation, string stateID,
            string createUser, string createTime, string mydept)//,string applydept,string ward,string bed
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@TypeID", SqlDbType.Int),
                new SqlParameter("@ConsultApplySN", SqlDbType.Int),
                new SqlParameter("@NoOfInpat", SqlDbType.Decimal),
                new SqlParameter("@UrgencyTypeID", SqlDbType.Int),
                new SqlParameter("@ConsultTypeID", SqlDbType.Int),
                new SqlParameter("@Abstract", SqlDbType.VarChar),
                new SqlParameter("@Purpose", SqlDbType.VarChar),
                new SqlParameter("@ApplyUser", SqlDbType.VarChar),
                new SqlParameter("@ApplyTime", SqlDbType.VarChar),
                new SqlParameter("@Director", SqlDbType.VarChar),
                new SqlParameter("@ConsultTime", SqlDbType.VarChar),
                new SqlParameter("@ConsultLocation", SqlDbType.VarChar),
                new SqlParameter("@StateID", SqlDbType.Int),
                new SqlParameter("@CreateUser", SqlDbType.VarChar),
                new SqlParameter("@CreateTime", SqlDbType.VarChar),
                 new SqlParameter("@mydept", SqlDbType.VarChar)//,
                //new SqlParameter("@APPLYDEPT", SqlDbType.VarChar),
                //new SqlParameter("@WARD", SqlDbType.VarChar),
                //new SqlParameter("@BED", SqlDbType.VarChar)
            };

            sqlParam[0].Value = typeID;
            if (consultApplySN.Trim() == "")
            {
                sqlParam[1].Value = 0;
            }
            else
            {
                sqlParam[1].Value = consultApplySN;
            }
            if (noOfInpat.Trim() == "")
            {
                sqlParam[2].Value = 0f;
            }
            else
            {
                sqlParam[2].Value = Convert.ToDecimal(noOfInpat);
            }
            if (urgencyTypeID.Trim() == "")
            {
                sqlParam[3].Value = 0;
            }
            else
            {
                sqlParam[3].Value = Convert.ToInt32(urgencyTypeID);
            }
            if (consultTypeID.Trim() == "")
            {
                sqlParam[4].Value = 0;
            }
            else
            {
                sqlParam[4].Value = Convert.ToInt32(consultTypeID);
            }
            sqlParam[5].Value = abstractContent;
            sqlParam[6].Value = purpose;
            sqlParam[7].Value = applyUser;
            sqlParam[8].Value = applyTime;
            sqlParam[9].Value = director;
            sqlParam[10].Value = consultTime;
            sqlParam[11].Value = consultLocation;
            if (stateID.Trim() == "")
            {
                sqlParam[12].Value = 0;
            }
            else
            {
                sqlParam[12].Value = Convert.ToInt32(stateID);
            }
            sqlParam[13].Value = createUser;
            sqlParam[14].Value = createTime;
            //sqlParam[15].Value = applydept;
            //sqlParam[16].Value = ward;
            //sqlParam[17].Value = bed;
            sqlParam[15].Value = mydept;

            DataTable dt = m_SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_InsertConsultationApply", sqlParam, CommandType.StoredProcedure);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ConsultApplySn"].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 在会诊记录填写界面保存时使用
        /// </summary>
        /// <param name="typeID"></param>
        /// <param name="consultApplySN"></param>
        /// <param name="noOfInpat"></param>
        /// <param name="consultSuggestion"></param>
        /// <param name="finishTime"></param>
        /// <returns></returns>
        public static string InsertConsultationApply(string typeID, string consultApplySN, string noOfInpat, string consultSuggestion, string finishTime, string stateID)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@TypeID", SqlDbType.Int),
                new SqlParameter("@ConsultApplySN", SqlDbType.Int),
                new SqlParameter("@NoOfInpat", SqlDbType.Decimal),
                new SqlParameter("@ConsultSuggestion", SqlDbType.VarChar),
                new SqlParameter("@FinishTime", SqlDbType.VarChar),
                new SqlParameter("@StateID", SqlDbType.VarChar)
            };

            sqlParam[0].Value = typeID;
            if (consultApplySN.Trim() == "")
            {
                sqlParam[1].Value = 0;
            }
            else
            {
                sqlParam[1].Value = consultApplySN;
            }
            if (noOfInpat.Trim() == "")
            {
                sqlParam[2].Value = 0f;
            }
            else
            {
                sqlParam[2].Value = Convert.ToDecimal(noOfInpat);
            }
            sqlParam[3].Value = consultSuggestion;
            sqlParam[4].Value = finishTime;
            sqlParam[5].Value = stateID;

            DataTable dt = m_SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_InsertConsultationApply", sqlParam, CommandType.StoredProcedure);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ConsultApplySn"].ToString();
            }
            else
            {
                return "";
            }
        }

        public static void InsertConsultationApplyDept(string consultApplySn, string orderValue, string hospitalCode, string departmentCode,
            string departmentName, string employeeCode, string employeeName, string employeeLevelID, string createUser, string createTime)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ConsultApplySn", SqlDbType.Decimal),
                new SqlParameter("@OrderValue", SqlDbType.Decimal),
                new SqlParameter("@HospitalCode", SqlDbType.VarChar),
                new SqlParameter("@DepartmentCode", SqlDbType.VarChar),
                new SqlParameter("@DepartmentName", SqlDbType.VarChar),
                new SqlParameter("@EmployeeCode", SqlDbType.VarChar),
                new SqlParameter("@EmployeeName", SqlDbType.VarChar),
                new SqlParameter("@EmployeeLevelID", SqlDbType.VarChar),
                new SqlParameter("@CreateUser", SqlDbType.VarChar),
                new SqlParameter("@CreateTime", SqlDbType.VarChar),
            };

            if (consultApplySn.Trim() == "")
            {
                sqlParam[0].Value = 0f;
            }
            else
            {
                sqlParam[0].Value = Convert.ToInt32(consultApplySn);
            }
            if (orderValue.Trim() == "")
            {
                sqlParam[1].Value = 0;
            }
            else
            {
                sqlParam[1].Value = Convert.ToInt32(orderValue);
            }
            sqlParam[2].Value = hospitalCode;
            sqlParam[3].Value = departmentCode;
            sqlParam[4].Value = departmentName;
            sqlParam[5].Value = employeeCode;
            sqlParam[6].Value = employeeName;
            sqlParam[7].Value = employeeLevelID;
            sqlParam[8].Value = createUser;
            sqlParam[9].Value = createTime;

            m_SqlHelper.ExecuteNoneQuery("EMR_CONSULTATION.usp_InsertConsultationApplyD", sqlParam, CommandType.StoredProcedure);
        }

        public static void InsertConsultationRecordDept(string consultApplySn, string orderValue, string hospitalCode, string departmentCode,
            string departmentName, string employeeCode, string employeeName, string employeeLevelID, string createUser, string createTime)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ConsultApplySn", SqlDbType.Decimal),
                new SqlParameter("@OrderValue", SqlDbType.Decimal),
                new SqlParameter("@HospitalCode", SqlDbType.VarChar),
                new SqlParameter("@DepartmentCode", SqlDbType.VarChar),
                new SqlParameter("@DepartmentName", SqlDbType.VarChar),
                new SqlParameter("@EmployeeCode", SqlDbType.VarChar),
                new SqlParameter("@EmployeeName", SqlDbType.VarChar),
                new SqlParameter("@EmployeeLevelID", SqlDbType.VarChar),
                new SqlParameter("@CreateUser", SqlDbType.VarChar),
                new SqlParameter("@CreateTime", SqlDbType.VarChar),
            };

            if (consultApplySn.Trim() == "")
            {
                sqlParam[0].Value = 0f;
            }
            else
            {
                sqlParam[0].Value = Convert.ToInt32(consultApplySn);
            }
            if (orderValue.Trim() == "")
            {
                sqlParam[1].Value = 0;
            }
            else
            {
                sqlParam[1].Value = Convert.ToInt32(orderValue);
            }
            sqlParam[2].Value = hospitalCode;
            sqlParam[3].Value = departmentCode;
            sqlParam[4].Value = departmentName;
            sqlParam[5].Value = employeeCode;
            sqlParam[6].Value = employeeName;
            sqlParam[7].Value = employeeLevelID;
            sqlParam[8].Value = createUser;
            sqlParam[9].Value = createTime;

            m_SqlHelper.ExecuteNoneQuery("EMR_CONSULTATION.usp_InsertConsultationRecord", sqlParam, CommandType.StoredProcedure);
        }

        public static void ModifyConsultationData(string consultApplySn, string typeID, string stateID, string rejectReason)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ConsultApplySn", SqlDbType.Decimal),
                new SqlParameter("@TypeID", SqlDbType.Decimal),
                new SqlParameter("@StateID", SqlDbType.VarChar),
                new SqlParameter("@RejectReason", SqlDbType.VarChar)
            };

            sqlParam[0].Value = consultApplySn;
            sqlParam[1].Value = typeID;
            sqlParam[2].Value = stateID;
            sqlParam[3].Value = rejectReason;

            m_SqlHelper.ExecuteNoneQuery("EMR_CONSULTATION.usp_UpdateConsultationData", sqlParam, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 根据申请单编号查询会诊信息
        /// </summary>
        /// <param name="consultationSn"></param>
        /// <returns></returns>
        public static DataTable GetConsultationTable(string consultationSn)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ConsultApplySn", SqlDbType.Decimal)
            };

            sqlParam[0].Value = consultationSn;
            return m_SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetConsultationBySN", sqlParam, CommandType.StoredProcedure);

        }
        /// <summary>
        /// 根据会诊编号以及状态判断 当前会诊信息是否与数据库信息一致
        /// </summary>
        /// <param name="consultationSn"></param>
        /// <param name="stateid"></param>
        /// <returns></returns>
        public bool CheckState(string consultationSn, string stateid)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ConsultApplySn", SqlDbType.Decimal)
            };

            sqlParam[0].Value = consultationSn;
            DataTable dt = m_SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetConsultationBySN", sqlParam, CommandType.StoredProcedure);

            if (dt == null || dt.Rows.Count == 0)
                return false;
            if (!dt.Rows[0]["stateid"].Equals(stateid))
                return false;
            return true;
        }
        /// <summary>
        /// 取消会诊单据,操作的consultapply表
        /// 2012年7月3日 14:04:54
        /// </summary>
        /// <param name="consultApplySn"></param>
        /// <param name="typeID"></param>
        /// <param name="stateID"></param>
        /// <param name="rejectReason"></param>
        public static void CancelConsultationData(string consultApplySn, string stateID)
        {
            string updatesql = string.Format(@" UPDATE consultapply
         SET stateid = '{0}', canceluser='{1}',canceltime='{2}'
       WHERE consultapplysn = '{3}'
         AND valid = '1' ", stateID, m_App.User.Id, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), consultApplySn);

            m_SqlHelper.ExecuteNoneQuery(updatesql, CommandType.Text);
        }
        /// <summary>
        /// ###########签到操作 操作consultapplydepartment表
        /// </summary>
        /// <param name="consultApplySn"></param>
        public static void ConsultInfoSignIn(string consultApplySn, string doctorid, string reachTime)
        {
            string updatesql = string.Format(@" UPDATE consultapplydepartment
             SET issignin='1' ,reachtime='{0}'  WHERE consultapplysn = '{1}'  and (employeecode='{2}' or  employeecode
            is null)     AND valid = '1' ", reachTime, consultApplySn, doctorid);
            m_SqlHelper.ExecuteNoneQuery(updatesql, CommandType.Text);

        }
        /// <summary>
        ///  ########受邀医生签到时往consultrecorddepartment表里插入会诊到达时间
        /// </summary>
        /// <param name="consultapplysn"></param>
        internal static void ConsultDepRecordSignIn(string consultapplysn, string doctorid, string reachTime)
        {
            string updatesql = string.Format(@" UPDATE consultrecorddepartment
             SET  issignin='1',reachtime='{0}'  WHERE consultapplysn = '{1}'  and (employeecode='{2}' or                          employeecode  is null)  AND valid = '1' ", reachTime, consultapplysn, doctorid);
            m_SqlHelper.ExecuteNoneQuery(updatesql, CommandType.Text);
        }

        /// <summary>
        /// 护士工作站中打印会诊单，往consultapply插入会诊单打印时间
        /// 打印时间作为护士已通知到医生会诊的时间
        /// </summary>
        /// <param name="consultapplysn"></param>
        public static void InsertPrintTime(string consultapplysn, IEmrHost m_App)
        {
            string sql = string.Format(@"update consultapply set PrintConsultTime='{0}' where consultapplysn='{1}'
             and  valid='1' ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), consultapplysn);
            if (m_SqlHelper == null)
            {
                m_SqlHelper = m_App.SqlHelper;
            }
            m_SqlHelper.ExecuteNoneQuery(sql, CommandType.Text);
        }


    }
}
