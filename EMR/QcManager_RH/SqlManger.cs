using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DrectSoft.Core;
using System.Reflection;

namespace DrectSoft.Emr.QcManager
{
    public class SqlManger
    {
        IEmrHost m_app;
        public SqlManger(IEmrHost app)
        {
            m_app = app;
        }
        /// <summary>
        /// 评分大项基本信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetQCTypeScore()
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(SQLUtil.sql_GetQCScoreType);
            return dataTable;
        }

        /// <summary>
        /// 评分大项操作方法
        /// </summary>
        /// <param name="editType">操作类型  0：新增    1:修改  2：删除</param>
        /// <param name="qcscortype">评分大项对应实体</param>
        public void EditQCTypeScore(string editType, QCScoreType_DataEntity qcscortype)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@TypeName", SqlDbType.VarChar, 40),
                  new SqlParameter("@TypeInstruction", SqlDbType.VarChar, 60),
                  new SqlParameter("@TypeCategory", SqlDbType.Int, 1),
                  new SqlParameter("@TypeOrder", SqlDbType.Int, 4),
                  new SqlParameter("@TypeMemo", SqlDbType.VarChar, 60),
                  new SqlParameter("@TypeStatus", SqlDbType.Int, 1),
                  new SqlParameter("@TypeCode", SqlDbType.VarChar, 4)};
            sqlParams[0].Value = qcscortype.Typename;
            sqlParams[1].Value = qcscortype.Typeinstruction;
            sqlParams[2].Value = qcscortype.Typecategory;
            sqlParams[3].Value = qcscortype.Typeorder;
            sqlParams[4].Value = qcscortype.Typememo;
            sqlParams[5].Value = Convert.ToInt32(editType);
            sqlParams[6].Value = qcscortype.Typecode;

            m_app.SqlHelper.ExecuteNoneQuery("usp_QCTypeScore", sqlParams, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 评分细项基本信息
        /// </summary>
        /// <param name="typeCode">（需要参数为TypeCode，全部查询传入‘’）</param>
        /// <returns></returns>
        public DataTable GetQCItemScore(string typeCode)
        {
            DataTable dataTable = null;
            dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_GetQCScoreItem, typeCode));
            return dataTable;
        }

        /// <summary>
        /// 评分细项操作方法
        /// </summary>
        /// <param name="editType">操作类型  0：新增    1:修改  2：删除</param>
        /// <param name="qcscoritem">评分细项对应实体</param>
        public void EditQCItemScore(string editType, QCScoreItem_DataEntity qcscoritem)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@ItemName", SqlDbType.VarChar, 40),
                  new SqlParameter("@ItemInstruction", SqlDbType.VarChar, 60),
                  new SqlParameter("@ItemDefaultScore", SqlDbType.Int, 1),
                  new SqlParameter("@ItemStandardScore", SqlDbType.Int, 1),
                  new SqlParameter("@ItemCategory", SqlDbType.Int, 1),
                  new SqlParameter("@ItemDefaultTarget", SqlDbType.Int, 1),
                  new SqlParameter("@ItemTargetStandard", SqlDbType.Int, 1),
                  new SqlParameter("@ItemScoreStandard", SqlDbType.Int, 1),
                  new SqlParameter("@ItemOrder", SqlDbType.Int, 2),
                  new SqlParameter("@TypeCode", SqlDbType.VarChar, 4),
                  new SqlParameter("@ItemMemo", SqlDbType.VarChar, 60),
                  new SqlParameter("@TypeStatus", SqlDbType.Int, 1),
                  new SqlParameter("@ItemCode", SqlDbType.VarChar, 5)};
            sqlParams[0].Value = qcscoritem.Itemname;
            sqlParams[1].Value = qcscoritem.Iteminstruction;
            sqlParams[2].Value = qcscoritem.Itemdefaultscore;
            sqlParams[3].Value = qcscoritem.Itemstandardscore;
            sqlParams[4].Value = qcscoritem.Itemcategory;
            sqlParams[5].Value = qcscoritem.Itemdefaulttarget;
            sqlParams[6].Value = qcscoritem.Itemtargetstandard;
            sqlParams[7].Value = qcscoritem.Itemscorestandard;
            sqlParams[8].Value = qcscoritem.Itemorder;
            sqlParams[9].Value = qcscoritem.Typecode;
            sqlParams[10].Value = qcscoritem.Itemmemo;
            sqlParams[11].Value = Convert.ToInt32(editType);
            sqlParams[12].Value = qcscoritem.Itemcode;

            m_app.SqlHelper.ExecuteNoneQuery("usp_QCItemScore", sqlParams, CommandType.StoredProcedure);
        }


        #region Add by wwj 2011-09-19 时限质控

        public DataTable GetPatientList(string deptNo, string patID, string name, string status, string beginInTime, string endInTime)
        {
            SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@deptid", SqlDbType.VarChar),
                new SqlParameter("@patid", SqlDbType.VarChar),
                new SqlParameter("@name", SqlDbType.VarChar),
                new SqlParameter("@status", SqlDbType.VarChar),
                new SqlParameter("@admitbegindate", SqlDbType.VarChar),
                new SqlParameter("@admitenddate", SqlDbType.VarChar)
            };
            if (beginInTime.Trim() != "")
            {
                beginInTime = beginInTime.Trim().Split(' ')[0];
            }
            if (endInTime.Trim() != "")
            {
                endInTime = endInTime.Trim().Split(' ')[0];
            }

            sqlParams[0].Value = deptNo;
            sqlParams[1].Value = patID;
            sqlParams[2].Value = name;
            sqlParams[3].Value = status;
            sqlParams[4].Value = beginInTime;
            sqlParams[5].Value = endInTime;
            return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_getpatientlist", sqlParams, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 得到科室统计信息
        /// </summary>
        /// <param name="deptNo"></param>
        /// <param name="patID"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="beginInTime"></param>
        /// <param name="endInTime"></param>
        /// <returns></returns>
        public DataTable GetAllDepartmentStatInfo(string deptNo, string patID, string name, string status, string beginInTime, string endInTime, string type)
        {
            if (string.IsNullOrEmpty(status))
            {

                SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@deptid", SqlDbType.VarChar),
                new SqlParameter("@patid", SqlDbType.VarChar),
                new SqlParameter("@name", SqlDbType.VarChar),
                new SqlParameter("@admitbegindate", SqlDbType.VarChar),
                new SqlParameter("@admitenddate", SqlDbType.VarChar)
            };
                if (beginInTime.Trim() != "")
                {
                    beginInTime = beginInTime.Trim().Split(' ')[0];
                }
                if (endInTime.Trim() != "")
                {
                    endInTime = endInTime.Trim().Split(' ')[0];
                }

                sqlParams[0].Value = deptNo;
                sqlParams[1].Value = patID;
                sqlParams[2].Value = name;
                sqlParams[3].Value = beginInTime;
                sqlParams[4].Value = endInTime;
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_RHgetDepstatinfoAll", sqlParams, CommandType.StoredProcedure);

            }
            else
            {
                SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@deptid", SqlDbType.VarChar),
                new SqlParameter("@patid", SqlDbType.VarChar),
                new SqlParameter("@name", SqlDbType.VarChar),
                new SqlParameter("@status", SqlDbType.VarChar),
                new SqlParameter("@admitbegindate", SqlDbType.VarChar),
                new SqlParameter("@admitenddate", SqlDbType.VarChar)
            };
                if (beginInTime.Trim() != "")
                {
                    beginInTime = beginInTime.Trim().Split(' ')[0];
                }
                if (endInTime.Trim() != "")
                {
                    endInTime = endInTime.Trim().Split(' ')[0];
                }

                sqlParams[0].Value = deptNo;
                sqlParams[1].Value = patID;
                sqlParams[2].Value = name;
                sqlParams[3].Value = status;
                sqlParams[4].Value = beginInTime;
                sqlParams[5].Value = endInTime;
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_RHgetalldepstatinfo", sqlParams, CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// 得到科室病人信息(仁和版本)  科室指控员和质控科 科主任
        /// 20120802 xll
        /// </summary>
        /// <param name="deptNo"></param>
        /// <param name="patID"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="beginInTime"></param>
        /// <param name="endInTime"></param>
        /// <returns></returns>
        public DataTable GetDepartmentPatStatInfo(string deptNo, string patID, string name, string status, string beginInTime, string endInTime, string type)
        {
            //标识当前登录人的身份 QCDepart质控科人员  QCMANAGER 为科室指控员 CHIEF为科主任
            if (String.IsNullOrEmpty(status))
            {
                if (type == "QCDepart")  //查询入院和出院
                {
                    SqlParameter[] sqlParams = new SqlParameter[] 
                {
                new SqlParameter("@deptid", SqlDbType.VarChar),
                new SqlParameter("@patid", SqlDbType.VarChar),
                new SqlParameter("@name", SqlDbType.VarChar),
                new SqlParameter("@admitbegindate", SqlDbType.VarChar),
                new SqlParameter("@admitenddate", SqlDbType.VarChar)
           
                };
                    if (beginInTime.Trim() != "")
                    {
                        beginInTime = beginInTime.Trim().Split(' ')[0];
                    }
                    if (endInTime.Trim() != "")
                    {
                        endInTime = endInTime.Trim().Split(' ')[0];
                    }
                    sqlParams[0].Value = deptNo;
                    sqlParams[1].Value = patID;
                    sqlParams[2].Value = name;
                    sqlParams[3].Value = beginInTime;
                    sqlParams[4].Value = endInTime;
                    return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetrhDepPatStatInfoAll", sqlParams, CommandType.StoredProcedure);

                }
                else if (type == "QCMANAGER") //科室指控员 出院
                {
                    SqlParameter[] sqlParams = new SqlParameter[] 
                {
                new SqlParameter("@deptid", SqlDbType.VarChar),
                new SqlParameter("@patid", SqlDbType.VarChar),
                new SqlParameter("@name", SqlDbType.VarChar),
                new SqlParameter("@admitbegindate", SqlDbType.VarChar),
                new SqlParameter("@admitenddate", SqlDbType.VarChar)
           
                };
                    if (beginInTime.Trim() != "")
                    {
                        beginInTime = beginInTime.Trim().Split(' ')[0];
                    }
                    if (endInTime.Trim() != "")
                    {
                        endInTime = endInTime.Trim().Split(' ')[0];
                    }
                    sqlParams[0].Value = deptNo;
                    sqlParams[1].Value = patID;
                    sqlParams[2].Value = name;
                    sqlParams[3].Value = beginInTime;
                    sqlParams[4].Value = endInTime;
                    return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetrhDepPatStatInfoCY", sqlParams, CommandType.StoredProcedure);

                }
                else if (type == "CHIEF") //出院
                {
                    SqlParameter[] sqlParams = new SqlParameter[] 
                {
                new SqlParameter("@deptid", SqlDbType.VarChar),
                new SqlParameter("@patid", SqlDbType.VarChar),
                new SqlParameter("@name", SqlDbType.VarChar),
                new SqlParameter("@status", SqlDbType.VarChar),
                new SqlParameter("@admitbegindate", SqlDbType.VarChar),
                new SqlParameter("@admitenddate", SqlDbType.VarChar)
                };
                    if (beginInTime.Trim() != "")
                    {
                        beginInTime = beginInTime.Trim().Split(' ')[0];
                    }
                    if (endInTime.Trim() != "")
                    {
                        endInTime = endInTime.Trim().Split(' ')[0];
                    }
                    sqlParams[0].Value = deptNo;
                    sqlParams[1].Value = patID;
                    sqlParams[2].Value = name;
                    sqlParams[3].Value = status;
                    sqlParams[4].Value = beginInTime;
                    sqlParams[5].Value = endInTime;
                    return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetRHGetDeptinfo", sqlParams, CommandType.StoredProcedure);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                SqlParameter[] sqlParams = new SqlParameter[] 
                {
                new SqlParameter("@deptid", SqlDbType.VarChar),
                new SqlParameter("@patid", SqlDbType.VarChar),
                new SqlParameter("@name", SqlDbType.VarChar),
                new SqlParameter("@status", SqlDbType.VarChar),
                new SqlParameter("@admitbegindate", SqlDbType.VarChar),
                new SqlParameter("@admitenddate", SqlDbType.VarChar)
           
                };
                if (beginInTime.Trim() != "")
                {
                    beginInTime = beginInTime.Trim().Split(' ')[0];
                }
                if (endInTime.Trim() != "")
                {
                    endInTime = endInTime.Trim().Split(' ')[0];
                }
                sqlParams[0].Value = deptNo;
                sqlParams[1].Value = patID;
                sqlParams[2].Value = name;
                sqlParams[3].Value = status;
                sqlParams[4].Value = beginInTime;
                sqlParams[5].Value = endInTime;
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetrhDepartmentPatStatInfo", sqlParams, CommandType.StoredProcedure);
            }

        }


        //最终评分详情
        public DataTable GetPatientPinFen(string deptNo, string patID, string name, string status, string beginInTime, string endInTime)
        {

            SqlParameter[] sqlParams = new SqlParameter[] 
                {
                new SqlParameter("@deptid", SqlDbType.VarChar),
                new SqlParameter("@patid", SqlDbType.VarChar),
                new SqlParameter("@name", SqlDbType.VarChar),
                new SqlParameter("@status", SqlDbType.VarChar),
                new SqlParameter("@admitbegindate", SqlDbType.VarChar),
                new SqlParameter("@admitenddate", SqlDbType.VarChar)
                };
            if (beginInTime.Trim() != "")
            {
                beginInTime = beginInTime.Trim().Split(' ')[0];
            }
            if (endInTime.Trim() != "")
            {
                endInTime = endInTime.Trim().Split(' ')[0];
            }
            sqlParams[0].Value = deptNo;
            sqlParams[1].Value = patID;
            sqlParams[2].Value = name;
            sqlParams[3].Value = status;
            sqlParams[4].Value = beginInTime;
            sqlParams[5].Value = endInTime;
            return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_getRHdepartpatstatinfo", sqlParams, CommandType.StoredProcedure);
        }



        public DataTable GetAllEmrDocByNoofinpat(string noofinpat)
        {
            SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@noofinpat", SqlDbType.VarChar)
            };
            sqlParams[0].Value = noofinpat;
            return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetAllEmrDocByNoofinpat", sqlParams, CommandType.StoredProcedure);
        }

        public DataTable GetAllDoctorByUserID(string userID)
        {
            SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@userid", SqlDbType.VarChar)
            };
            sqlParams[0].Value = userID;
            return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetAllDoctorByUserNO", sqlParams, CommandType.StoredProcedure);
        }

        public DataTable GetAllDoctorByNoofinpat(string noofinpat)
        {
            SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@noofinpat", SqlDbType.VarChar)
            };
            sqlParams[0].Value = noofinpat;
            return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetAllDoctorByNoofinpat", sqlParams, CommandType.StoredProcedure);
        }

        public void InsertEmrPoint(RHEmrPoint emrPoint)
        {
            SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@doctorID", SqlDbType.VarChar),
                new SqlParameter("@doctorname", SqlDbType.VarChar),
                new SqlParameter("@create_user", SqlDbType.VarChar),
                new SqlParameter("@createusername", SqlDbType.VarChar),
                new SqlParameter("@problem_desc", SqlDbType.VarChar),
                new SqlParameter("@reducepoint", SqlDbType.VarChar),
                new SqlParameter("@num", SqlDbType.VarChar),
                new SqlParameter("@grade", SqlDbType.VarChar),
                new SqlParameter("@recorddetailid", SqlDbType.VarChar),
                new SqlParameter("@noofinpat", SqlDbType.VarChar),
                new SqlParameter("@recorddetailname", SqlDbType.VarChar),
                //新增评分配置表主键
                 new SqlParameter("@emrpointid", SqlDbType.Int),
                 //新增大类别编号
                 new SqlParameter("@sortid", SqlDbType.VarChar)
            };

            sqlParams[0].Value = emrPoint.DoctorID;
            sqlParams[1].Value = emrPoint.DoctorName;
            sqlParams[2].Value = emrPoint.CreateUserID;
            sqlParams[3].Value = emrPoint.CreateUserName;
            sqlParams[4].Value = emrPoint.ProblemDesc;
            sqlParams[5].Value = emrPoint.ReducePoint;
            sqlParams[6].Value = emrPoint.Num;
            sqlParams[7].Value = emrPoint.Grade;
            sqlParams[8].Value = emrPoint.RecordDetailID;
            sqlParams[9].Value = emrPoint.Noofinpat;
            sqlParams[10].Value = emrPoint.RecordDetailName;
            sqlParams[11].Value = emrPoint.EmrPointID;
            sqlParams[12].Value = emrPoint.SortID;
            m_app.SqlHelper.ExecuteNoneQuery("EMRQCMANAGER.usp_insertEmrPoint", sqlParams, CommandType.StoredProcedure);
        }

        public void InsertRHEmrPoint(RHEmrPoint emrPoint, string rhqcTableId)
        {
            string guidstr = Guid.NewGuid().ToString();
            string sqlinsert = string.Format(@"insert into emr_rhpoint values
('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}',{9},'{10}',null,null,'{11}','{12}',null,'{13}','{14}','{15}','{16}')",
guidstr, emrPoint.Noofinpat, emrPoint.RecordDetailID, emrPoint.DoctorID, emrPoint.CreateUserID,
emrPoint.CreateTime, emrPoint.ProblemDesc, emrPoint.ReducePoint, emrPoint.Grade, emrPoint.Num, emrPoint.Valid,
emrPoint.DoctorName, emrPoint.CreateUserName, emrPoint.RecordDetailName, emrPoint.EmrPointID, emrPoint.SortID, rhqcTableId);
            m_app.SqlHelper.ExecuteNoneQuery(sqlinsert);
        }

        public void CancelRHEmrPoint(RHEmrPoint emrPoint)
        {
            SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@ID", SqlDbType.VarChar),
                new SqlParameter("@cancel_user", SqlDbType.VarChar),
                new SqlParameter("@cancel_userName", SqlDbType.VarChar),
            };
            sqlParams[0].Value = emrPoint.ID;
            sqlParams[1].Value = emrPoint.CancelUserID;
            sqlParams[2].Value = emrPoint.CancelUserName;

            m_app.SqlHelper.ExecuteNoneQuery("EMRQCMANAGER.usp_cancelRHEmrPoint", sqlParams, CommandType.StoredProcedure);
        }

        public DataTable GetAllEmrPointByNoofinpat(string noofinpat)
        {
            SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@noofinpat", SqlDbType.VarChar)
            };
            sqlParams[0].Value = noofinpat;
            return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetEmrPointByNoofinpat", sqlParams, CommandType.StoredProcedure);
        }

        public DataTable GetRHAllEmrPointByNoofinpat(string rhqcTableId)
        {
            SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@rhqc_tableId", SqlDbType.VarChar)
            };
            sqlParams[0].Value = rhqcTableId;
            return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetRHPoint", sqlParams, CommandType.StoredProcedure);
        }

        public DataTable GetPatientInfo(string noofinpat)
        {
            SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@noofinpat", SqlDbType.VarChar)
            };
            sqlParams[0].Value = noofinpat;
            return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetPatientInfo", sqlParams, CommandType.StoredProcedure);
        }
        #endregion

        /// <summary>
        /// 获取病历评分配置表的信息 add by ywk 2012年3月31日17:59:46 
        /// </summary>
        /// <returns></returns>
        public DataTable GetConfigPoint()
        {
            string serchsql = string.Format(@"  
                 select A.CCODE,A.ID,A.CHILDCODE,A.CHILDNAME,A.VALID,B.CNAME,(case
                                 when a.valid = 1 then
                                  '是'
                                 else
                                  '否'
                               end) validName from Emr_ConfigPoint A join dict_catalog B 
                 on A.CCODE=B.CCODE and  A.Valid='1'  ");//order by CCODE,ChildCode
            return m_app.SqlHelper.ExecuteDataTable(serchsql);
        }
        /// <summary>
        /// 病历评分维护中绑定的大类的SQL语句
        /// </summary>
        /// <returns></returns>
        public DataTable GetParentClass()
        {
            string serchsql = string.Format(@"  
                 select CCODE,CNAME from  dict_catalog");
            return m_app.SqlHelper.ExecuteDataTable(serchsql);
        }
        /// <summary>
        /// 保存操作，更新或新增操作(类别配置里的)
        /// </summary>
        /// <param name="configEmrPoint"></param>
        /// <param name="edittype"></param>
        public string SaveData(ConfigEmrPoint configEmrPoint, string edittype)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@CCode",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar),
                    new SqlParameter("@CChildCode",SqlDbType.VarChar),
                    new SqlParameter("@CChildName",SqlDbType.VarChar),
                    new SqlParameter("@Valid",SqlDbType.VarChar)
                };
            sqlParam[0].Value = edittype;
            sqlParam[1].Value = configEmrPoint.CCODE;
            sqlParam[2].Value = configEmrPoint.ID;
            sqlParam[3].Value = configEmrPoint.CChildCode;
            sqlParam[4].Value = configEmrPoint.CChildName;
            sqlParam[5].Value = configEmrPoint.Valid;

            return m_app.SqlHelper.ExecuteDataSet("EMRQCMANAGER.usp_Edit_ConfigPoint", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 保存操作（更新或者新增）（评分配置里的）
        /// </summary>
        /// <param name="configReduction"></param>
        /// <param name="edittype"></param>
        internal string SaveReductionData(ConfigReduction configReduction, string edittype)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@REDUCEPOINT",SqlDbType.VarChar),//扣的分数
                    new SqlParameter("@PROBLEMDESC",SqlDbType.VarChar),//扣分理由
                    new SqlParameter("@CREATEUSER",SqlDbType.VarChar),//创建人
                    new SqlParameter("@CREATETIME",SqlDbType.VarChar),//创建时间
                     new SqlParameter("@ID",SqlDbType.VarChar),
                    new SqlParameter("@Valid",SqlDbType.VarChar),
                        new SqlParameter("@UserType",SqlDbType.VarChar)
                };
            sqlParam[0].Value = edittype;
            sqlParam[1].Value = configReduction.REDUCEPOINT;
            sqlParam[2].Value = configReduction.PROBLEMDESC;
            sqlParam[3].Value = configReduction.CreateUserID;
            sqlParam[4].Value = configReduction.CreateTime;
            sqlParam[5].Value = configReduction.ID;
            sqlParam[6].Value = configReduction.Valid;
            sqlParam[7].Value = configReduction.Usertype;
            return m_app.SqlHelper.ExecuteDataSet("EMRQCMANAGER.usp_Edit_RHConfigReduction", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
        }
        /// <summary>
        /// 病历评分 获取点击的根文件夹
        /// add by ywk 2012年4月1日17:04:08 写在存储过程里
        /// </summary>
        /// <returns></returns>
        public DataTable GetPointClass()
        {
            //SqlParameter[] sqlParams = new SqlParameter[] 
            //{
            //    new SqlParameter("@sortid", SqlDbType.VarChar)
            //};
            //sqlParams[0].Value = noofinpat;
            return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetPointClass", CommandType.StoredProcedure);
        }
        private string m_GetHospitalInfo = "SELECT h.ID, h.Name FROM HospitalInfo h";
        /// <summary>
        /// 获得医院名称
        /// </summary>
        /// <returns></returns>
        public string GetHospitalName()
        {
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(m_GetHospitalInfo, CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["name"].ToString();
            }
            return "";
        }
        /// <summary>
        /// 根据首页序号获得病人的相关信息
        /// </summary>
        /// <param name="m_NoOfInpat"></param>
        /// <returns></returns>
        public DataTable GetPatInfoByNo(string m_NoOfInpat)
        {
            string serchsql = string.Format(@"  
                select b.name as deptname,a.name as patname,a.patid
                ,c.name as indocname,d.name as updocname from inpatient a
               left join department b  on a.outhosdept=b.id 
               left join users c on a.resident=c.id  
               left join users d on  d.id=a.chief
                where a.noofinpat='{0}' ", m_NoOfInpat);
            return m_app.SqlHelper.ExecuteDataTable(serchsql);
        }
        /// <summary>
        /// 获得病人的病历评分信息
        /// </summary>
        /// <param name="m_NoOfInpat"></param>
        /// <returns></returns>
        public DataTable GetPainetData(string m_NoOfInpat, string rhqcTableid)
        {
            string sql = string.Format(@" select 
                           a.noofinpat,
                            a.patid as patid,
                            a.name as patname,
                             e.problem_desc as koufenliyou,
                             e.reducepoint as redpoint,
                            (case when  v.childname is null then h.cname else v.childname end) childname,
                               (case when  v.id is null then to_number(e.emrpointid) else v.id end) id,
                             b.name as deptname,
                             h.cname
                             from emr_rhpoint e
                         left join emr_configpoint v on e.emrpointid=v.id  
                         left join dict_catalog h on e.sortid=h.ccode   
                        left join inpatient  a on a.noofinpat=e.noofinpat    
    
                        left  join  department b on b.id=a.outhosdept
                         left join   users f on a.resident=f.id
                         left join users f1 on f1.id=a.chief 
                           where e.noofinpat = '{0}' and e.rhqc_table_id='{1}'
                         and e.valid = '1' order by cname ", m_NoOfInpat, rhqcTableid);
            return m_app.SqlHelper.ExecuteDataTable(sql);
        }
        /// <summary>
        /// /获得病例评分标准中的数据
        /// </summary>
        /// <returns></returns>
        internal DataTable GetReductionData()
        {
            string serchsql = string.Format(@"  
                select A.ID,A.REDUCEPOINT,A.PROBLEM_DESC,A.CREATE_USER,A.CREATE_TIME,A.VALID,(case
                                 when A.valid = 1 then
                                  '是'
                                 else
                                  '否'
                               end) validName,A.USER_TYPE from EMR_RHConfigReduction A where A.valid='1' order by A.ID ");
            return m_app.SqlHelper.ExecuteDataTable(serchsql);
        }
        /// <summary>
        /// 得到配置信息
        /// add by ywk 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigValueByKey(string key)
        {
            string sql1 = " select * from appcfg where configkey = '" + key + "'; ";
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
            string config = string.Empty;
            if (dt.Rows.Count > 0)
            {
                config = dt.Rows[0]["value"].ToString();
            }
            return config;
        }

        #region 全院质控中的相关的数据操作
        /// <summary>
        /// 获得各个科室的质控员配置情况
        /// </summary>
        /// <returns></returns>
        internal DataTable GetDeptQCManager(string deptid)
        {
            string sql = string.Empty;
            if (deptid == "")
            {
                sql = string.Format(@" select  A.id,D.NAME deptname,(case
           when A.valid = 1 then    '是' else  '否'  end) validName,U1.Name  QCMANAGERNAME, U2.NAME  CHIEFNAME 
           from emr_Configcheckpointuser  A left join department D on A.DEPTID=D.ID
           left join users U1 on U1.ID=A.QCMANAGERID  left join users U2 on   U2.ID=A.CHIEFDOCTORID where A.valid='1'");
            }
            else
            {
                sql = string.Format(@"   select  A.id,D.NAME deptname,(case
           when A.valid = 1 then    '是' else  '否'  end) validName,U1.Name    QCMANAGERNAME, U2.NAME  CHIEFNAME 
           from emr_Configcheckpointuser  A left join department D on A.DEPTID=D.ID
           left join users U1 on U1.ID=A.QCMANAGERID  left join users U2 on    U2.ID=A.CHIEFDOCTORID where A.Deptid='{0}' and A.valid='1'", deptid);
            }
            return m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
        }
        /// <summary>
        /// 根据科室编号获得此科室下对应的主任
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        internal DataTable GetDirectorDoc(string deptid)
        {
            string sql = string.Format(@"  
               SELECT * FROM users u
         WHERE (u.wardid IS NOT NULL or u.wardid != '')
           AND u.valid = '1' AND u.grade = '2000'and u.deptid='{0}'  ORDER BY u.ID ", deptid);
            return m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
        }

        /// <summary>
        /// 编辑有关质控人员配置的信息 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="id"></param>
        /// <param name="deptid"></param>
        /// <param name="qcmanagerid"></param>
        /// <param name="chiefdoctorid"></param>
        internal void InsertQCManagerUSer(string id, string deptid, string qcmanagerid, string chiefdoctorid, string valid)
        {
            string insertsql = string.Format(@"insert into emr_ConfigCheckPointUser (Id,Deptid,Qcmanagerid,Chiefdoctorid,Valid) values(seqemr_ConfigCheckPointUser_ID.Nextval,
     '{0}','{1}','{2}','1')", deptid, qcmanagerid, chiefdoctorid);
            try
            {
                m_app.SqlHelper.ExecuteNoneQuery(insertsql, CommandType.Text);
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
                //throw;
                return;
            }



        }
        /// <summary>
        /// 暂不用
        /// </summary>
        /// <param name="edittype"></param>
        /// <param name="id"></param>
        /// <param name="deptid"></param>
        /// <param name="qcmanagerid"></param>
        /// <param name="chiefdoctorid"></param>
        /// <param name="valid"></param>
        internal void EditQCManagerUSer(string edittype, string id, string deptid, string qcmanagerid, string chiefdoctorid, string valid)
        {
            SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@ID",SqlDbType.VarChar),//编号
                    new SqlParameter("@DeptID",SqlDbType.VarChar),//科室编号
                    new SqlParameter("@QcManagerID",SqlDbType.VarChar),//科室质控员编号
                    new SqlParameter("@ChiefDoctorID",SqlDbType.VarChar),//主任医生编号
                    new SqlParameter("@Valid",SqlDbType.VarChar)
                };
            sqlParam[0].Value = edittype;
            sqlParam[1].Value = id;
            sqlParam[2].Value = deptid;
            sqlParam[3].Value = qcmanagerid;
            sqlParam[4].Value = chiefdoctorid;
            sqlParam[5].Value = valid;

            try
            {
                m_app.SqlHelper.ExecuteNoneQuery("EMRQCMANAGER.usp_Edit_ConfigPointManager", sqlParam, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                //Mess
                m_app.CustomMessageBox.MessageShow(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="p"></param>
        /// <param name="deptid"></param>
        /// <param name="qcmanagerid"></param>
        /// <param name="chiefdoctorid"></param>
        /// <param name="p_2"></param>
        internal void UpdateQCManagerUSer(string p, string deptid, string qcmanagerid, string chiefdoctorid, string p_2)
        {
            string updatesql = string.Format(@"update emr_ConfigCheckPointUser set Qcmanagerid='{0}',Chiefdoctorid='{1}' where 
valid='1' and deptid='{2}'", qcmanagerid, chiefdoctorid, deptid);
            m_app.SqlHelper.ExecuteNoneQuery(updatesql, CommandType.Text);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deptid"></param>
        /// <param name="qcmanagerid"></param>
        /// <param name="chiefdoctorid"></param>
        /// <param name="valid"></param>
        internal void DeleteQCManagerUSer(string id, string deptid, string qcmanagerid, string chiefdoctorid, string valid)
        {
            string updatesql = string.Format(@"update emr_ConfigCheckPointUser set  valid='0' where 
valid='1' and id='{0}'", id);
            m_app.SqlHelper.ExecuteNoneQuery(updatesql, CommandType.Text);
        }


        /// <summary>
        /// 判断当前登录人的身份（是科室质控员还是质控科的）
        /// QCDepart质控科人员  QCMANAGER 为科室指控员 CHIEF为科主任
        /// </summary>
        /// <param name="p"></param>
        /// <returns>QCDepart质控科人员  QCMANAGER 为科室指控员 CHIEF为科主任 </returns>
        internal string JudgeIdentity(string userid, SqlManger m_SqlManger)
        {
            string Ident = string.Empty;
            string configvalue = m_SqlManger.GetConfigValueByKey("RHQCMangesConfig");
            List<string> configIDList = new List<string>();
            if (!String.IsNullOrEmpty(configvalue))
            {
                configIDList = (configvalue.Split(',')).ToList();
            }
            bool hasUserQC = false; //人员是否是质控科人员
            foreach (var item in configIDList)
            {
                if (item == userid)
                {
                    hasUserQC = true;
                }
            }
            if (hasUserQC)
                Ident = "QCDepart";
            else
            {
                string sql = string.Format("select * from emr_ConfigCheckPointUser where qcmanagerid='{0}' and valid='1'", userid);
                if (m_app.SqlHelper.ExecuteDataTable(sql).Rows.Count > 0)//是科室质控员
                {
                    Ident = "QCMANAGER";
                }
                string sql1 = string.Format("select * from emr_ConfigCheckPointUser where chiefdoctorid='{0}' and valid='1'", userid);
                if (m_app.SqlHelper.ExecuteDataTable(sql1).Rows.Count > 0)//是主任
                {
                    Ident = "CHIEF";
                }
            }
            return Ident;
        }
        #endregion

        public int GetSumPoint(string m_NoOfInpat, IEmrHost m_App)
        {
            int SumPoint = 100;
            string strSql = string.Format(@"select * from inpatient where noofinpat='{0}'", m_NoOfInpat);
            DataTable dtInPatient = m_App.SqlHelper.ExecuteDataTable(strSql);
            string strStatus = dtInPatient.Rows[0]["status"].ToString();
            if (strStatus == "1501")
            {
                SumPoint = 85;
            }
            else
            {
                SumPoint = 100;
            }
            return SumPoint;
        }

        public DataTable SetTiXiugai(string userState)
        {
            string sqlLit = "";
            string strSql = "";
            if (userState == "QCDepart")  //质控科
            {
                sqlLit = @" select d.name,i.noofinpat,qc.id rhqctable,i.name pername from emr_rhqc_table qc
                                left join  inpatient i on qc.noofinpat=i.noofinpat
                                LEFT OUTER JOIN department d ON d.ID = i.outhosdept
                                 where doctorstate='1' and create_user='{0}' and valid='1' and stateid='8705'; ";
                strSql = string.Format(sqlLit, m_app.User.DoctorId);

            }
            else if (userState == "QCMANAGER")  //科室质控员
            {
                sqlLit = @" select d.name,i.noofinpat,qc.id rhqctable,i.name pername from emr_rhqc_table qc
                                left join  inpatient i on qc.noofinpat=i.noofinpat
                                LEFT OUTER JOIN department d ON d.ID = i.outhosdept
                                 where doctorstate='1' and create_user='{0}'and d.id='{1}' and valid='1' and stateid in('8701','8703'); ";
                strSql = string.Format(sqlLit, m_app.User.DoctorId, m_app.User.CurrentDeptId);
            }
            else if (userState == "CHIEF")
            {
                sqlLit = @" select d.name,i.noofinpat,qc.id rhqctable,i.name pername from emr_rhqc_table qc
                                left join  inpatient i on qc.noofinpat=i.noofinpat
                                LEFT OUTER JOIN department d ON d.ID = i.outhosdept
                                 where doctorstate='1' and d.id='{0}' and valid='1' and stateid in('8701','8703'); ";
                strSql = string.Format(sqlLit, m_app.User.CurrentDeptId);
            }
            if (string.IsNullOrEmpty(strSql))
            {
                return new DataTable();
            }
            DataTable dtPer = m_app.SqlHelper.ExecuteDataTable(strSql, CommandType.Text);
            return dtPer;
        }

    }

}
