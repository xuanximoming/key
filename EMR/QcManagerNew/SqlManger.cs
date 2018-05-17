using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DrectSoft.Core;

namespace DrectSoft.Emr.QcManagerNew
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
        /// 评分细项基本信息
        /// </summary>
        /// <param name="typeCode">（需要参数为TypeCode，全部查询传入‘’）</param>
        /// <returns></returns>
        public DataTable GetQCItemScore(string typeCode)
        {
            try
            {
                DataTable dataTable = null;
                dataTable = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_GetQCScoreItem, typeCode));
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }

       


        #region Add by wwj 2011-09-19 时限质控
        /// <summary>
        /// 获取病人列表
        /// </summary>
        /// <param name="deptNo">科室代码</param>
        /// <param name="patID">病人ID</param>
        /// <param name="name">病人姓名</param>
        /// <param name="status">病人状态</param>
        /// <param name="beginInTime">开始时间</param>
        /// <param name="endInTime">结束时间</param>
        /// <returns></returns>
        public DataTable GetPatientList(string deptNo, string patID, string name, string status, string beginInTime, string endInTime)
        {
            try
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
                if (GetConfigValueByKey("QCManagerPatientListOption") == "1")
                {
                    return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_getpatientlistandpatSZ", sqlParams, CommandType.StoredProcedure);
                }
                else
                {
                    return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_getpatientlistandpat", sqlParams, CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取出院病人
        /// </summary>
        /// <param name="deptNo"></param>
        /// <param name="patID"></param>
        /// <param name="name"></param>
        /// <param name="beginInTime"></param>
        /// <param name="endInTime"></param>
        /// <returns></returns>
        public DataTable GetPatientOutList(string deptNo, string patID, string name, string beginInTime, string endInTime)
        {
            try
            {
                if (beginInTime.Trim() != "")
                {
                    beginInTime = beginInTime.Trim().Split(' ')[0];
                }
                if (endInTime.Trim() != "")
                {
                    endInTime = endInTime.Trim().Split(' ')[0];
                }
                //edit by wyt 2012-12-10 添加病人状态
                string sql = string.Format(@"SELECT inpatient.outhosdept deptno,
             department.NAME      deptname,
             inpatient.patid,
             inpatient.NAME,
             inpatient.noofinpat,
             inpatient.admitdate, 
             inpatient.status
        FROM inpatient
        LEFT OUTER JOIN department
          ON department.ID = inpatient.outhosdept
       WHERE inpatient.outhosdept LIKE '%' || '{0}' || '%'
         AND inpatient.patid LIKE '%' || '{1}' || '%'
         AND inpatient.NAME LIKE '%' || '{2}' || '%'
         AND inpatient.status in (1502,1503)
         AND (inpatient.admitdate >= '{3}' AND
             inpatient.admitdate <= '{4}');", deptNo, patID, name, beginInTime, endInTime);


                return m_app.SqlHelper.ExecuteDataTable(sql);

            }
            catch (Exception)
            {
                throw;
            }
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
        public DataTable GetAllDepartmentStatInfo(string deptNo, string patID, string name, string status, string beginInTime, string endInTime)
        {
            try
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
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetAllDepartmentStatInfo", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 根据选中的病人获取自动评分记录 王冀 2012 11 13
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetAutoMarkInfo(string noofinpat, string isAuto, Authority auth)
        {
            try
            {
                string sql = "select * from dual";
                //质控员能查看所有状态的质控记录
                if (auth == Authority.QC)
                {
                    sql = @"select rownum, t.ID ,p.name Paname, t.name Recname, t.create_time  ctime, users.name uname, t.recorddetailid recid,t.score,t.noofinpat, 
                             (case t.CHECKSTATE when '0' then '新建' when '1' then '提交未审核' when '2' then '审核通过' when '3' then '审核未通过' when '4' then '质控员质控' end) CHECKSTATE,
                            (case t.QCTYPE when '0' then '环节质控' when '1' then '终末质控' end) QCTYPE                            
                            from emr_automark_record t, inpatient p,users
                            where p.noofinpat = t.noofinpat and users.id = t.create_user and t.noofinpat ='{0}'
                              and t.isauto = '{1}' and t.isvalid = 1 order by t.create_time asc";
                }
                //非质控员只能查看非质控状态的质控记录
                else
                {
                    sql = @"select rownum, t.ID ,p.name Paname, t.name Recname, t.create_time  ctime, users.name uname, t.recorddetailid recid,t.score,t.noofinpat, 
                             (case t.CHECKSTATE when '0' then '新建' when '1' then '提交未审核' when '2' then '审核通过' when '3' then '审核未通过' when '4' then '质控员质控' end) CHECKSTATE,
                            (case t.QCTYPE when '0' then '环节质控' when '1' then '终末质控' end) QCTYPE                            
                            from emr_automark_record t, inpatient p,users
                            where p.noofinpat = t.noofinpat and users.id = t.create_user and t.noofinpat ='{0}'
                              and t.isauto = '{1}' and t.isvalid = 1 and t.checkstate <> 4 order by t.create_time asc";
                }
                //switch (auth)
                //{
                //    case Authority.DEPTQC:
                //        break;
                //    case Authority.DEPTMANAGER:
                //        break;
                //    case Authority.QC:
                //        break;
                //}

                sql = string.Format(sql, noofinpat, isAuto);
                return m_app.SqlHelper.ExecuteDataTable(sql);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 根据病人首页序号获取病案首页编号
        /// add by wyt 2012-11-20
        /// </summary>
        /// <param name="noofinpat">病人首页序号</param>
        /// <returns>病案首页编号</returns>
        public string GetIEM_MAINPAGE_NO(string noofinpat)
        {
            try
            {
                string sql = @"select IEM_MAINPAGE_NO from iem_mainpage_basicinfo_2012 where noofinpat = '" + noofinpat + "' and valide = 1";

                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count <= 0)
                {
                    return "";
                }
                else
                {
                    return dt.Rows[0][0].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 根据参数新增一条评分记录
        /// 王冀 2012 11 14       //edit by wyt 2012-11-20    //edit by wyt 2012-12-10 add qctype
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns>主表记录号</returns>
        public string InsertAutoMarkRecord(string noofinpat, string isAuto, Authority auth, QCType qctype)
        {
            try
            {
                //string sql = @"select max(to_number(id)) + 1 as id from emr_automark_record";
                string sql = @" Select seq_emr_automark_record.NextVal ID From Dual";
                string id = m_app.SqlHelper.ExecuteDataTable(sql).Rows[0]["ID"].ToString();
                string creatuser = m_app.User.Id.ToString();
                //质控员质控记录
                if (auth == Authority.QC)
                {
                    sql = @"insert into emr_automark_record(id,create_time,name,create_user,recorddetailid,noofinpat,isauto,isvalid,isused,checkstate,qctype) 
                        values('{0}',sysdate, (select inp.name|| ' 评分记录' from inpatient inp where inp.noofinpat ={2}),'{1}','','{2}','{3}','1','0','4','{4}' )";
                }
                //非质控员质控记录
                else
                {
                    sql = @"insert into emr_automark_record(id,create_time,name,create_user,recorddetailid,noofinpat,isauto,isvalid,isused,checkstate,qctype) 
                        values('{0}',sysdate, (select inp.name|| ' 评分记录' from inpatient inp where inp.noofinpat ={2}),'{1}','','{2}','{3}','1','0','0','{4}' )";
                }

                sql = string.Format(sql, id, creatuser, noofinpat, isAuto, ((int)qctype).ToString());
                m_app.SqlHelper.ExecuteNoneQuery(sql);
                return id;
            }
            catch (Exception)
            {
                return "";
                throw;
            }
        }

        /// <summary>
        /// 更新主评分记录状态
        /// </summary>
        /// <param name="chiefID"></param>
        /// <param name="checkstate"></param>
        public void UpdateAutoMarkRecord(string chiefID, string checkstate)
        {
            try
            {
                //string sql = @"select max(to_number(id)) + 1 as id from emr_automark_record";
                string sql = @"update emr_automark_record set checkstate = '{1}' where id = '{0}'";
                sql = string.Format(sql, chiefID, checkstate);
                m_app.SqlHelper.ExecuteNoneQuery(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除主评分记录状态
        /// </summary>
        /// <param name="chiefID"></param>
        /// <param name="checkstate"></param>
        public void DelAutoMarkRecord(string chiefID)
        {
            try
            {
                //string sql = @"select max(to_number(id)) + 1 as id from emr_automark_record";
                string sql = @"update emr_automark_record set ISVALID = '0' where id = '{0}'";
                sql = string.Format(sql, chiefID);
                m_app.SqlHelper.ExecuteNoneQuery(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 对自动评分记录主表进行假删除操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteAutoMarkRecord(string id)
        {
            try
            {
                string sql = string.Format(@"update emr_automark_record set isvalid='0' where id='{0}'", id);
                m_app.SqlHelper.ExecuteNoneQuery(sql);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }



        /// <summary>
        /// 设置为已评测
        /// 王冀 2012 11 19
        /// </summary>
        /// <param name="automarkrecordid"></param>
        public void SetRecordDone(string automarkrecordid)
        {
            try
            {
                string sql = string.Format(@"update emr_automark_record set isused='1' where id='{0}'", automarkrecordid);
                m_app.SqlHelper.ExecuteNoneQuery(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 得到选中自动评分项目内容信息
        /// </summary>
        /// <param name="id">自动评分主表ID</param>
        /// <returns>根据自动评分主表ID得到评分详情</returns>
        public DataTable GetPatRecordDetail(string id)
        {
            try
            {
                //                string sql = @"select t.id,t.configreductionid,t.point,t.automarkrecordid,
                //                            t.noofinpat,t.pname,t.configreductionname,s.PARENTS,s.CHILDREN,p.CHILDNAME,d.cname,t.errordoctor ,u.name errordoctorname from 
                //                            emr_automark_record_detail t,emr_configreduction2 s,emr_configpoint p,dict_catalog d ,users u
                //                            where t.automarkrecordid='{0}' and t.CONFIGREDUCTIONID = s.id and u.id(+)=t.errordoctor
                //                            and p.childcode =s.CHILDREN and d.ccode = s.PARENTS  order by t.id ";
                //                string sql = @"select t.id,
                //               t.configreductionid,
                //               t.point,
                //               t.automarkrecordid,
                //               t.noofinpat,
                //               t.pname,
                //               t.configreductionname,
                //               s.PARENTS,
                //               s.CHILDREN,
                //               p.CHILDNAME,
                //               d.cname,
                //               t.errordoctor,
                //               u.name errordoctorname
                //          from emr_automark_record_detail t,
                //               emr_configreduction2       s,
                //               emr_configpoint            p,
                //               dict_catalog               d,
                //               users                      u
                //         where t.automarkrecordid = '{0}'
                //           and t.CONFIGREDUCTIONID = s.id
                //           and u.id(+) = t.errordoctor
                //           and p.childcode = s.CHILDREN
                //           and d.ccode = s.PARENTS        
                //         order by id";

                string sql = @"select *
                  from (select t.id,
                               t.configreductionid,
                               t.point,
                               t.automarkrecordid,
                               t.noofinpat,
                               t.pname,
                               t.configreductionname,
                               s.PARENTS,
                               s.CHILDREN,
                               p.CHILDNAME,
                               d.cname,
                               t.errordoctor,
                               u.name errordoctorname
                          from emr_automark_record_detail t,
                               emr_configreduction2       s,
                               emr_configpoint            p,
                               dict_catalog               d,
                               users                      u
                         where t.automarkrecordid = '{0}'
                           and t.CONFIGREDUCTIONID = s.id
                           and u.id(+) = t.errordoctor
                           and p.childcode = s.CHILDREN
                           and d.ccode = s.PARENTS
                        union
                        select t.id,
                               t.configreductionid,
                               t.point,
                               t.automarkrecordid,
                               t.noofinpat,
                               t.pname,
                               t.configreductionname,
                               p.ccode,
                               p.childcode,
                               p.childname,
                               d.cname,
                               t.errordoctor,
                               u.name errordoctorname
                          from emr_automark_record_detail t,
                               qcrule                     q,
                               dict_catalog               d,
                               users                      u,
                               emr_configpoint            p
                         where t.automarkrecordid = '{0}'
                           and q.rulecode = t.configreductionid
                           and q.qccode = p.childcode
                           and t.CONFIGREDUCTIONID = q.rulecode
                           and u.id(+) = t.errordoctor
                           and d.ccode = p.ccode)
                 order by id";
                sql = string.Format(sql, id);
                return m_app.SqlHelper.ExecuteDataTable(sql);
            }
            catch (Exception)
            {
                throw;
            }

        }


        /// <summary>
        /// 根据自动评分记录ID找自动评分记录主表信息
        /// add by wyt 2012-11-20
        /// </summary>
        /// <param name="id">自动评分记录ID</param>
        /// <returns>自动评分记录主表信息</returns>
        public DataTable GetAutoMarkRecord(string id)
        {
            try
            {
                string sql = string.Format(@"select * from emr_automark_record  where emr_automark_record.id = '{0}'", id);
                return m_app.SqlHelper.ExecuteDataTable(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 判断是否已经评测过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsHaveDone(string id)
        {
            try
            {
                string sql = @"select t.isused from emr_automark_record t where t.id='{0}'";
                sql = string.Format(sql, id);
                string re = m_app.SqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString();
                if (re == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// 得到科室病人信息//edit by wyt 2012-12-12
        /// </summary>
        /// <param name="deptNo"></param>
        /// <param name="patID"></param>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="beginInTime"></param>
        /// <param name="sumpoint">新增的总分</param>
        /// <param name="endInTime"></param>
        /// <returns></returns>
        public DataTable GetDepartmentPatStatInfo(string deptNo, string patID, string name, string status, string beginInTime, string endInTime, string sortid, int sumpoint1, int sumpoint2, string type, string userid, Authority auth)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
            {
                new SqlParameter("@deptid", SqlDbType.VarChar),
                new SqlParameter("@patid", SqlDbType.VarChar),
                new SqlParameter("@name", SqlDbType.VarChar),
                new SqlParameter("@status", SqlDbType.VarChar),
                new SqlParameter("@admitbegindate", SqlDbType.VarChar),
                new SqlParameter("@admitenddate", SqlDbType.VarChar),
                new SqlParameter("@sortid", SqlDbType.VarChar),
                new SqlParameter("@sumpoint1", SqlDbType.Int),
                new SqlParameter("@sumpoint2", SqlDbType.Int),
                new SqlParameter("@type", SqlDbType.VarChar),
                new SqlParameter("@userid", SqlDbType.VarChar),//add by wyt 2012-12-12
                new SqlParameter("@auth", SqlDbType.VarChar)//add by wyt 2012-12-12
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
                sqlParams[6].Value = sortid;
                sqlParams[7].Value = sumpoint1;//edit by wyt 2012-12-12
                sqlParams[8].Value = sumpoint2;//edit by wyt 2012-12-12
                sqlParams[9].Value = type;//病历评分和时限质控的调取存储过程都是这个地方，现在改成分开 add by ywk 2012年11月6日10:54:50
                //edit by wyt 2012-12-12
                sqlParams[10].Value = userid;
                sqlParams[11].Value = ((int)auth).ToString();
                //return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_getdepartmentpatstatinfo", sqlParams, CommandType.StoredProcedure);
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_getdepartmentpatqcinfo", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetAllEmrDocByNoofinpat(string noofinpat)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@noofinpat", SqlDbType.VarChar)
            };
                sqlParams[0].Value = noofinpat;
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetAllEmrDocByNoofinpat", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetAllDoctorByUserID(string userID)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] 
                {
                new SqlParameter("@userid", SqlDbType.VarChar)
                };
                sqlParams[0].Value = userID;
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetAllDoctorByUserNO", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetAllDoctorByNoofinpat(string noofinpat)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                new SqlParameter("@noofinpat", SqlDbType.VarChar)
                };
                sqlParams[0].Value = noofinpat;
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetAllDoctorByNoofinpat", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 清空该次自动评分
        /// </summary>
        /// <param name="chiefID"></param>
        public void ClearEmrPoint(string chiefID)
        {
            try
            {
                string sql = "update emr_point set valid = 0 where EMR_MARK_RECORD_ID = '" + chiefID + "'";
                m_app.SqlHelper.ExecuteNoneQuery(sql);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void InsertEmrPoint(EmrPoint emrPoint)
        {
            try
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
                //new SqlParameter("@grade", SqlDbType.VarChar),   王冀 2012 11 30
                new SqlParameter("@recorddetailid", SqlDbType.VarChar),
                new SqlParameter("@noofinpat", SqlDbType.VarChar),
                new SqlParameter("@recorddetailname", SqlDbType.VarChar),
                //新增评分配置表主键
                //edit by wyt 2012-12-11
                // new SqlParameter("@emrpointid", SqlDbType.Int),
                 new SqlParameter("@emrpointid", SqlDbType.VarChar),
                 //新增大类别编号
                 new SqlParameter("@sortid", SqlDbType.VarChar),
                 new SqlParameter("@chiefID", SqlDbType.VarChar)
                };

                sqlParams[0].Value = emrPoint.DoctorID;
                sqlParams[1].Value = emrPoint.DoctorName;
                sqlParams[2].Value = emrPoint.CreateUserID;
                sqlParams[3].Value = emrPoint.CreateUserName;
                sqlParams[4].Value = emrPoint.ProblemDesc;
                sqlParams[5].Value = emrPoint.ReducePoint;
                sqlParams[6].Value = emrPoint.Num;
                //sqlParams[7].Value = emrPoint.Grade;
                sqlParams[7].Value = emrPoint.RecordDetailID;
                sqlParams[8].Value = emrPoint.Noofinpat;
                sqlParams[9].Value = emrPoint.RecordDetailName;
                sqlParams[10].Value = emrPoint.EmrPointID;
                sqlParams[11].Value = emrPoint.SortID;
                sqlParams[12].Value = emrPoint.EMR_MARK_RECORD_ID;
                m_app.SqlHelper.ExecuteNoneQuery("EMRQCMANAGER.usp_insertEmrPoint", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CancelEmrPoint(EmrPoint emrPoint)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] 
                {
                new SqlParameter("@ID", SqlDbType.VarChar),
                new SqlParameter("@cancel_user", SqlDbType.VarChar)
                };
                sqlParams[0].Value = emrPoint.ID;
                sqlParams[1].Value = emrPoint.CancelUserID;

                m_app.SqlHelper.ExecuteNoneQuery("EMRQCMANAGER.usp_cancelEmrPoint", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetEmrPointByNoofinpat(string noofinpat, string chiefID)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@noofinpat", SqlDbType.VarChar),
                new SqlParameter("@chiefid", SqlDbType.VarChar)
            };
                sqlParams[0].Value = noofinpat;
                sqlParams[1].Value = chiefID;
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetEmrPointByNoofinpat", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetPatientInfo(string noofinpat)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] 
            {
                new SqlParameter("@noofinpat", SqlDbType.VarChar)
            };
                sqlParams[0].Value = noofinpat;
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetPatientInfo", sqlParams, CommandType.StoredProcedure);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        /// <summary>
        /// 获取病历评分配置表的信息 add by ywk 2012年3月31日17:59:46 
        /// </summary>
        /// <returns></returns>
        public DataTable GetConfigPoint()
        {
            try
            {
                string serchsql = string.Format(@"  
                 select A.CCODE,A.ID,A.CHILDCODE,A.CHILDNAME,A.VALID,B.CNAME,(case
                                 when a.valid = 1 then
                                  '是'
                                 else
                                  '否'
                               end) validName from Emr_ConfigPoint A join dict_catalog B 
                 on A.CCODE=B.CCODE and  A.Valid='1'  order by a.ccode,a.childcode ");//order by CCODE,ChildCode
                return m_app.SqlHelper.ExecuteDataTable(serchsql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 病历评分维护中绑定的大类的SQL语句
        /// </summary>
        /// <returns></returns>
        public DataTable GetParentClass()
        {
            try
            {
                string serchsql = string.Format(@"  
                 select CCODE,CNAME from  dict_catalog");
                return m_app.SqlHelper.ExecuteDataTable(serchsql);
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 保存操作，更新或新增操作(类别配置里的)
        /// </summary>
        /// <param name="configEmrPoint"></param>
        /// <param name="edittype"></param>
        public string SaveData(ConfigEmrPoint configEmrPoint, string edittype)
        {
            try
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
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 保存操作（更新或者新增）（评分配置里的）
        /// edit by 王冀 2012-11-9
        /// edit by 王冀 2012-11-29
        /// </summary>
        /// <param name="configReduction"></param>
        /// <param name="edittype"></param>
        internal string SaveReductionData(ConfigReduction configReduction, string edittype)
        {
            try
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
                    new SqlParameter("@Parents",SqlDbType.VarChar),
                    new SqlParameter("@Children",SqlDbType.VarChar),
                    new SqlParameter("@Isauto",SqlDbType.VarChar),
                    new SqlParameter("@Selectcondition",SqlDbType.VarChar)
                };
                sqlParam[0].Value = edittype;
                sqlParam[1].Value = configReduction.REDUCEPOINT;
                sqlParam[2].Value = configReduction.PROBLEMDESC;
                sqlParam[3].Value = configReduction.CreateUserID;
                sqlParam[4].Value = configReduction.CreateTime;
                sqlParam[5].Value = configReduction.ID;
                sqlParam[6].Value = configReduction.Valid;
                sqlParam[7].Value = configReduction.ParentsCode;
                sqlParam[8].Value = configReduction.ChildCode;
                sqlParam[9].Value = configReduction.Isauto;
                sqlParam[10].Value = configReduction.Selectcondition;
                return m_app.SqlHelper.ExecuteDataSet("EMRQCMANAGER.usp_Edit_ConfigReduction", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 病历评分 获取点击的根文件夹
        /// add by ywk 2012年4月1日17:04:08 写在存储过程里
        /// </summary>
        /// <returns></returns>
        public DataTable GetPointClass()
        {
            try
            {
                //SqlParameter[] sqlParams = new SqlParameter[] 
                //{
                //    new SqlParameter("@sortid", SqlDbType.VarChar)
                //};
                //sqlParams[0].Value = noofinpat;
                return m_app.SqlHelper.ExecuteDataTable("EMRQCMANAGER.usp_GetPointClass", CommandType.StoredProcedure);
            }
            catch (Exception)
            {

                throw;
            }

        }
        private string m_GetHospitalInfo = "SELECT h.ID, h.Name FROM HospitalInfo h";
        /// <summary>
        /// 获得医院名称
        /// </summary>
        /// <returns></returns>
        public string GetHospitalName()
        {
            try
            {
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(m_GetHospitalInfo, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["name"].ToString();
                }
                return "";
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 根据首页序号获得病人的相关信息
        /// </summary>
        /// <param name="m_NoOfInpat"></param>
        /// <returns></returns>
        public DataTable GetPatInfoByNo(string m_NoOfInpat)
        {
            try
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
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 获得病人的病历评分信息
        /// </summary>
        /// <param name="m_NoOfInpat"></param>
        /// <returns></returns>
        public DataTable GetPainetData(string m_NoOfInpat)
        {
            try
            {
                string sql = string.Format(@" select a.noofinpat,a.patid as patid,a.name as patname,e.problem_desc as koufenliyou,  e.reducepoint as redpoint,(case when v.childname is null then h.cname else v.childname end) childname,/*(case
         when v.id is null then  e.emrpointid  else to_char(v.id) end) id,*/ v.id, b.name  as deptname, h.cname from emr_point e left join emr_configpoint v on e.emrpointid = to_char(v.id) left join dict_catalog h on e.sortid = h.ccode left join inpatient a on a.noofinpat = e.noofinpat left join department b on b.id = a.outhosdept left join users f on a.resident = f.id left join users f1  on f1.id = a.chief where e.noofinpat = '17742' and e.valid = '1' order by cname", m_NoOfInpat);
                return m_app.SqlHelper.ExecuteDataTable(sql);
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// /获得病例评分标准中的数据
        /// edit by 王冀 2012-11-8
        /// edit by 王冀 2012 12 11
        /// </summary>
        /// <returns></returns>
        internal DataTable GetReductionData()
        {
            try
            {
                //            string serchsql = string.Format(@"  
                //                select A.ID,A.REDUCEPOINT,A.PROBLEM_DESC,A.CREATE_USER,A.CREATE_TIME,A.VALID,(case
                //                                 when A.valid = 1 then
                //                                  '是'
                //                                 else
                //                                  '否'
                //                               end) validName from EMR_ConfigReduction A where A.valid='1' order by A.ID ");
                string serchsql = string.Format(@"select rownum , t.* from (select rownum a,A.ID,
          A.REDUCEPOINT,
          A.PROBLEM_DESC,
          B.CHILDNAME,
          B.childcode,
          c.CNAME,
          c.ccode,
          A.CREATE_USER,
          A.CREATE_TIME,
          A.VALID,
          decode(a.valid,1,'是','否') validName,
          A.ISAUTO,A.SELECTCONDITION
     from EMR_ConfigReduction2 A,Emr_ConfigPoint B,dict_catalog c
    where A.valid = '1'
      and B.valid = '1'
      and a.parents =c.CCODE 
      and a.children =b.childcode
    order by CNAME,CHILDNAME,a) t");
                return m_app.SqlHelper.ExecuteDataTable(serchsql);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// /获得病例评分标准中的数据
        /// edit by 王冀 2012-11-8
        /// edit by 王冀 2012 12 11
        /// </summary>
        /// <returns></returns>
        internal DataTable GetFocusReductionData(string parents, string children, string item)
        {
            try
            {
                //            string serchsql = string.Format(@"  
                //                select A.ID,A.REDUCEPOINT,A.PROBLEM_DESC,A.CREATE_USER,A.CREATE_TIME,A.VALID,(case
                //                                 when A.valid = 1 then
                //                                  '是'
                //                                 else
                //                                  '否'
                //                               end) validName from EMR_ConfigReduction A where A.valid='1' order by A.ID ");
                string serchsql = string.Format(@"select A.ID,
          A.REDUCEPOINT,
          A.PROBLEM_DESC,
          B.CHILDNAME,
          B.childcode,
          c.CNAME,
          c.ccode,
          A.CREATE_USER,
          A.CREATE_TIME,
          A.VALID,
          decode(a.valid,1,'是','否') validName,
          A.ISAUTO,A.SELECTCONDITION
     from EMR_ConfigReduction2 A,Emr_ConfigPoint B,dict_catalog c
    where A.valid = '1'
      and B.valid = '1'
      and a.parents =c.CCODE 
      and a.children =b.childcode
      and a.parents='{0}'
      and a.children='{1}'
      and A.PROBLEM_DESC='{2}'
    order by A.ID", parents, children, item);
                return m_app.SqlHelper.ExecuteDataTable(serchsql);
            }
            catch (Exception)
            {
                throw;
            }

        }
        ///// <summary>
        ///// 获取配置好的项目节点明细 王冀 2012 11 29 注释
        ///// </summary>
        ///// <returns></returns>
        //internal DataTable GetReductionDetailData()
        //{
        //    try
        //    {
        //        string serchsql = "";
        //        //                string serchsql = string.Format(@"select A.ID,
        //        //          A.REDUCEPOINT,
        //        //          A.PROBLEM_DESC,
        //        //          B.CHILDNAME,
        //        //          B.childcode,
        //        //          c.CNAME,
        //        //          c.ccode,
        //        //          A.CREATE_USER,
        //        //          A.CREATE_TIME,
        //        //          A.VALID,
        //        //          decode(a.valid,1,'是','否') validName
        //        //     from EMR_ConfigReduction2 A,Emr_ConfigPoint B,dict_catalog c
        //        //    where A.valid = '1'
        //        //      and a.parents =c.CCODE 
        //        //      and a.children =b.childcode
        //        //    order by A.ID");
        //        return m_app.SqlHelper.ExecuteDataTable(serchsql);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        /// <summary>
        /// 获取已配置好的父类
        ///  王冀 2012-11-8
        /// </summary>
        /// <returns></returns>
        internal DataTable GetReductionParents()
        {
            try
            {
                string sql = @" select  distinct a.ccode ID,b.cname Name from Emr_ConfigPoint a,dict_catalog b where  b.ccode(+)=a.ccode and A.Valid = '1' ";
                return m_app.SqlHelper.ExecuteDataTable(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取已配置好的子类
        ///  王冀 2012-11-8
        /// </summary>
        /// <returns></returns>
        internal DataTable GetReductionChild()
        {
            try
            {
                string sql = @"    select a.childname Name,a.ccode,a.childcode ID from Emr_ConfigPoint a where a.valid=1 ";
                return m_app.SqlHelper.ExecuteDataTable(sql);
            }
            catch (Exception)
            {
                throw;
            }

        }

        //internal DataTable GetReductionDetail() 王冀 2012 11 29 注释
        //{
        //    try
        //    {
        //        string sql = @"    select a.childname Name,a.ccode,a.childcode ID from Emr_ConfigPoint a where a.valid=1 ";
        //        return m_app.SqlHelper.ExecuteDataTable(sql);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        /// <summary>
        /// 获取病人病历
        /// 王冀 2012-11-8
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        internal DataTable GetPatientsEMRS(string noofinpat)
        {
            try
            {
                string sql = @"select  t.id,t.name,t.noofinpat,t.owner from recorddetail t where t.noofinpat={0}";
                sql = string.Format(sql, noofinpat);
                return m_app.SqlHelper.ExecuteDataTable(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据ID获取电子病历类别
        /// 王冀 2012-11-8
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal string GetEMRClass(string id)
        {
            try
            {
                string sql = @"select  t.sortid from recorddetail t where t.id='{0}' ";
                sql = string.Format(sql, id);
                return m_app.SqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 将扣分项目序号，扣分数，扣分内容插入数据库中
        /// 王冀 2012 - 11 - 18 
        /// 2012 11 28 添加责任医生
        /// </summary>
        /// <param name="configreductionid"></param>
        /// <param name="point"></param>
        /// <param name="automarkrecordid"></param>
        /// <param name="noofinpat"></param>
        /// <param name="pname"></param>
        /// <param name="configreductionname"></param>
        /// <returns></returns>
        public void InsertDB(int configreductionid, string point, string automarkrecordid, string noofinpat, string pname, string configreductionname, string errordoctor)
        {
            try
            {
                string sql = @"select  decode(max(id),null,0,max(id))+1 id from emr_automark_record_detail";
                int id = int.Parse(m_app.SqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString());

                sql = @"insert into emr_automark_record_detail(id,configreductionid,point,automarkrecordid,noofinpat,pname,configreductionname,ERRORDOCTOR)
                                                        values({0},{1},'{2}','{3}','{4}','{5}','{6}','{7}')";
                sql = string.Format(sql, id, configreductionid, point, automarkrecordid, noofinpat, pname, configreductionname, errordoctor);

                m_app.SqlHelper.ExecuteNoneQuery(sql);
            }
            catch (Exception)
            {
                throw;
            }

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
            try
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
            catch (Exception)
            {
                throw;
            }

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
        #endregion
        /// <summary>
        /// 取得各个大类别下面的小的评分项的评分节点
        /// </summary>
        /// <returns></returns>
        internal DataTable GetPointConfig(string mr_class)
        {
            string sql = string.Format
                ("select t.id,t.reducepoint,t.problem_desc,p.childname from emr_configreduction2 t,emr_configpoint p where t.parents='{0}'and t.children=p.childcode and t.parents=p.ccode", mr_class);
            try
            {
                return m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
