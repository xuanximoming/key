using DrectSoft.Common;
using DrectSoft.Common.Eop;
using DrectSoft.DSSqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;

namespace DrectSoft.Service
{
    /// <summary>
    /// 功能描述：通用数据操作类
    ///    数据通用方法
    ///    目的是集中常用数据操作
    /// 创 建 者：
    /// 创建日期：2012-12-03
    /// </summary>
    public static class DS_SqlService
    {
        #region 关于基础数据 (如用户、病人、字典、配置...)
        /// <summary>
        /// 获取配置信息
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-03</date>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValueByKey(string key)
        {
            try
            {

                if (!string.IsNullOrEmpty(key))
                {
                    string sqlStr = " select * from appcfg where configkey = @key ";
                    DbParameter[] sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@key",SqlDbType.Char,32)
                    };
                    sqlParams[0].Value = key;

                    DS_SqlHelper.CreateSqlHelper();
                    DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        return null == dt.Rows[0]["value"] ? string.Empty : dt.Rows[0]["value"].ToString();
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据ID获取用户信息
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public static DataTable GetUserByID(string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return new DataTable();
                }

                string sqlStr = " select * from users where valid =1 and id = @id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.Char)
                };
                sqlParams[0].Value = userID;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取所有病人信息
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-29</date>
        /// <returns></returns>
        public static DataTable GetAllPatient()
        {
            try
            {
                string sqlStr = "select * from InPatient";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据首页序号获取病人信息
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        public static Inpatient GetPatientInfo(string noofinpat)
        {
            try
            {
                Inpatient patientInfo = new Inpatient();
                if (!string.IsNullOrEmpty(noofinpat))
                {
                    string sqlStr = "select * from InPatient where 1=1 and NoOfInpat = @noofinpat ";
                    DbParameter[] sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@noofinpat",SqlDbType.Char,32)
                    };
                    sqlParams[0].Value = noofinpat;
                    DS_SqlHelper.CreateSqlHelper();
                    DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        patientInfo = new Inpatient(dt.Rows[0]);
                    }
                }
                return patientInfo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据首页序号获取病人信息
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-18</date>
        /// <param name="noofinpat">首页序号</param>
        /// <param name="flag">1、门诊，2、住院</param>
        /// <returns></returns>
        public static DataTable GetInpatientByID(int noofinpat, int flag)
        {
            try
            {
                string sqlStr = null;
                if (flag == 2)
                    sqlStr = "SELECT i.*,d.name deptname,w.name wardname,dd1.name sexname FROM inpatient i left outer join department d on d.id = i.outhosdept and d.valid = '1' left outer join ward w on w.id = i.outhosward and w.valid = '1' left outer join dictionary_detail dd1 on dd1.detailid = i.sexid and dd1.categoryid = '3' WHERE i.noofinpat = @noofinpat ";
                else
                    sqlStr = "SELECT i.*, d.name deptname, dd1.name sexname FROM inpatient_clinic i left outer join department d on d.id = i.deptid and d.valid = '1' left outer join dictionary_detail dd1 on dd1.detailid = i.sexid and dd1.categoryid = '3' WHERE i.noofinpatclinic = @noofinpat ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据首页序号获取病人信息
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-18</date>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        public static DataTable GetInpatientByIDInTran(int noofinpat)
        {
            try
            {
                string sqlStr = "select * from InPatient where NoOfInpat = @noofinpat ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;

                return DS_SqlHelper.ExecuteDataTableInTran(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据首页序号获取床号
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-07</date>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        public static string GetInpatientOutBedNo(int noofinpat)
        {
            try
            {
                string sqlStr = "select outbed from inpatient where noofinpat=@noofinpat ";
                OracleParameter[] sqlParams = new OracleParameter[]
                {
                    new OracleParameter("@noofinpat",OracleType.Int32)
                };
                sqlParams[0].Value = noofinpat;

                DS_SqlHelper.CreateSqlHelper();
                object obj = DS_SqlHelper.ExecuteScalar(sqlStr, sqlParams, CommandType.Text);
                return null == obj ? string.Empty : obj.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取科室
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// <returns></returns>
        public static DataTable GetDepartments()
        {
            try
            {
                string sqlStr = " SELECT '0000' AS ID, '全院' AS NAME, 'qy' as PY, 'wgbpf' as WB FROM DUAL UNION SELECT ID, NAME, PY, WB FROM department, dept2ward dw WHERE department.ID = dw.deptid and department.valid = 1 ORDER BY ID ";
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取科室
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-18</date>
        /// <returns></returns>
        public static DataTable GetDepartmentByID(string deptID)
        {
            try
            {
                if (string.IsNullOrEmpty(deptID))
                {
                    return new DataTable();
                }
                string deptName = string.Empty;
                string sqlStr = " select * from department where id=@deptid  ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@deptid",SqlDbType.Char)
                };
                sqlParams[0].Value = deptID;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取病区
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-07</date>
        /// <returns></returns>
        public static DataTable GetWardByID(string wardid)
        {
            try
            {
                if (string.IsNullOrEmpty(wardid))
                {
                    return new DataTable();
                }
                string deptName = string.Empty;
                string sqlStr = " select * from ward where id=@wardid  ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@wardid",SqlDbType.Char)
                };
                sqlParams[0].Value = wardid;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取权限内的科室ID和病区ID集合
        /// 注：string[0]:科室；string[1]:病区
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-18</date>
        /// <returns></returns>
        public static DataTable GetDeptAndWardInRight(string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return new DataTable();
                }
                List<string[]> list = new List<string[]>();
                string sqlStr = " select * from user2dept where userid=@userid ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@userid",SqlDbType.Char)
                };
                sqlParams[0].Value = userID;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据病人病历获取科室ID和病区ID集合
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-25</date>
        /// <returns></returns>
        public static DataTable GetDeptAndWardByRecords(int noofinpat)
        {
            try
            {
                string sqlStr = " select distinct departcode,wardcode from recorddetail where noofinpat=@noofinpat and sortid='AC' and valid=1 ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据登陆用户和病人首页序号获取权限
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-17</date>
        /// <param name="userID">用户ID</param>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        public static DataTable GetEmrRightsOfChangeInfo(string userID, int noofinpat)
        {
            try
            {
                string sqlStr = "select * from inpatientchangeinfo i inner join (select deptid,wardid from users where id=@userid ";
                List<OracleParameter> list = new List<OracleParameter>();
                OracleParameter param1 = new OracleParameter("noofinpat", OracleType.Int32);
                param1.Value = noofinpat;
                list.Add(param1);
                if (!string.IsNullOrEmpty(userID))
                {
                    sqlStr += " union select deptid,wardid from user2dept where userid=@userid ";
                    OracleParameter param2 = new OracleParameter("userid", OracleType.VarChar);
                    param2.Value = userID;
                    list.Add(param2);
                }
                sqlStr += " ) a on i.newdeptid=a.deptid and i.newwardid=a.wardid and i.noofinpat=@noofinpat and i.valid=1 and i.changestyle in('0','1','2') order by createtime ";

                return DS_SqlHelper.ExecuteDataTable(sqlStr, list, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取字典明细表类别ID获取明细记录
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-26</date>
        /// <param name="categoryID">字典类别ID</param>
        /// <returns></returns>
        public static DataTable GetDetailsByCategoryID(int categoryID)
        {
            try
            {
                if (categoryID < 0)
                {
                    return new DataTable();
                }

                string sqlStr = " select * from categorydetail where categoryid = @categoryID order by id desc ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@categoryID",SqlDbType.Int)
                };
                sqlParams[0].Value = categoryID;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据某唯一条件查询其内容
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">查询字段名(单个字段)</param>
        /// <param name="whereName">条件字段名(唯一条件)</param>
        /// <param name="whereValue">条件值</param>
        /// <returns></returns>
        public static string GetFieldContent(string tableName, string fieldName, string whereName, string whereValue)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(fieldName) || string.IsNullOrEmpty(whereName) || string.IsNullOrEmpty(whereValue))
                {
                    return string.Empty;
                }
                tableName = DS_Common.FilterSpecialCharacter(tableName);
                fieldName = DS_Common.FilterSpecialCharacter(fieldName);
                whereName = DS_Common.FilterSpecialCharacter(whereName);
                string sqlStr = " select " + fieldName + " from " + tableName + " where " + whereName + " = @whereValue ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@whereValue",SqlDbType.Char)
                };
                sqlParams[0].Value = whereValue;
                DS_SqlHelper.CreateSqlHelper();
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);

                string returnStr = string.Empty;
                if (null != dt && dt.Rows.Count > 0)
                {
                    returnStr = dt.Rows[0][0].ToString();
                }
                return returnStr;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取病人的入院记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-05</date>
        /// <param name="noofclinic"></param>
        /// <returns></returns>
        public static DataTable GetInHosDetails(int noofinpat)
        {
            try
            {
                string sqlStr = " select * from inpatient where noofclinic in(select noofclinic from inpatient where noofinpat=@noofinpat)";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取病人的入院次数
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-26</date>
        /// <param name="noofclinic"></param>
        /// <returns></returns>
        public static int GetInHosCount(int noofinpat)
        {
            try
            {
                string sqlStr = " SELECT COUNT(1) incount FROM inpatient i WHERE i.noofclinic IN (SELECT noofclinic FROM inpatient WHERE noofinpat = @noofinpat AND noofclinic IS NOT NULL) ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return 0;
                }
                return int.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取病人当前入院次数
        /// </summary>
        /// 注：IsUpdateIDNO，是否与身份证号同步。(是则以noofclinic为准，否则以patnoofhis为准)
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public static int GetInHosCountNew(int noofinpat)
        {
            try
            {
                DataTable inpDT = DS_SqlService.GetInpatientByID(noofinpat, 2);
                if (null == inpDT || inpDT.Rows.Count == 0)
                {
                    return 0;
                }

                //xll 20130903 住院次数只从病人表中incount字段中取
                //string isIdNoCfg = DS_SqlService.GetConfigValueByKey("IsUpdateIDNO");
                //string sqlStr = string.Empty;
                //if (isIdNoCfg == "0")
                //{
                //    if (inpDT.Rows[0]["patnoofhis"].ToString().Contains("-"))
                //    {
                //        return Tool.IsInt(inpDT.Rows[0]["incount"].ToString()) ? Convert.ToInt32(inpDT.Rows[0]["incount"]) : 0;
                //    }
                //    sqlStr = "select count(*) from inpatient i join (select patnoofhis,admitdate from inpatient where noofinpat=@noofinpat) a on i.patnoofhis=a.patnoofhis and i.admitdate<=a.admitdate";
                //}
                //else
                //{
                //    if (inpDT.Rows[0]["noofclinic"].ToString().Contains("-"))
                //    {
                //        return Tool.IsInt(inpDT.Rows[0]["incount"].ToString()) ? Convert.ToInt32(inpDT.Rows[0]["incount"]) : 0;
                //    }
                //    sqlStr = "select count(*) from inpatient i join (select noofclinic,admitdate from inpatient where noofinpat=@noofinpat) a on i.noofclinic=a.noofclinic and i.admitdate<=a.admitdate";
                //}
                //OracleParameter[] pars = new OracleParameter[]
                //{
                //    new OracleParameter("@noofinpat",OracleType.Int32)
                //};
                //pars[0].Value = noofinpat;
                //DS_SqlHelper.CreateSqlHelper();
                //object obj = DS_SqlHelper.ExecuteScalar(sqlStr, pars, CommandType.Text);

                //return null == obj ? 0 : Convert.ToInt32(obj);

                return Tool.IsInt(inpDT.Rows[0]["incount"].ToString()) ? Convert.ToInt32(inpDT.Rows[0]["incount"]) : 1;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取病人的转科记录
        /// </summary>
        /// 注：包含科室、病区信息
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-27</date>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public static DataTable GetInpatientChangeInfo(int noofinpat)
        {
            try
            {
                string sqlStr = "select i.*,d1.name newdeptname,w1.name newwardname from InpatientChangeInfo i left join department d1 on i.newdeptid=d1.id left join ward w1 on i.newwardid=w1.id where i.noofinpat=@noofinpat and i.changestyle in('0','1','2') order by createtime";
                OracleParameter[] paras = new OracleParameter[]
                {
                    new OracleParameter("@noofinpat",OracleType.Int32)
                };
                paras[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, paras, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据转科记录ID获取信息
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-10</date>
        /// <param name="id">转科记录ID</param>
        /// <returns></returns>
        public static DataTable GetInpChangeInfoByID(int id)
        {
            try
            {
                string sqlStr = "select * from inpatientchangeinfo where id=@id ";
                OracleParameter[] paras = new OracleParameter[]
                {
                    new OracleParameter("@id",OracleType.Int32)
                };
                paras[0].Value = id;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, paras, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据科室获取转科记录ID
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-07</date>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public static string GetInpChangeInfoID(int noofinpat, string deptCode)
        {
            try
            {
                string sqlStr = "select id from inpatientchangeinfo where noofinpat=@noofinpat and newdeptid=@newdeptid order by createtime";
                OracleParameter[] paras = new OracleParameter[]
                {
                    new OracleParameter("@noofinpat",OracleType.Int32),
                    new OracleParameter("@newdeptid",OracleType.VarChar)
                };
                paras[0].Value = noofinpat;
                paras[1].Value = deptCode;
                DS_SqlHelper.CreateSqlHelper();
                object obj = DS_SqlHelper.ExecuteScalar(sqlStr, paras, CommandType.Text);
                if (null == obj)
                {
                    return string.Empty;
                }
                return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询转科记录表的下一个ID
        /// </summary>
        /// <returns></returns>
        public static int GetNextIdFromInpChangeDept()
        {
            try
            {
                string sqlStr = "select seq_inpatientchangeinfo_id.nextval from dual";
                DS_SqlHelper.CreateSqlHelper();
                object obj = DS_SqlHelper.ExecuteScalar(sqlStr, CommandType.Text);
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入转科记录
        /// </summary>
        /// 注：所传参数名为对应数据库中字段名
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-07</date>
        /// <param name="Params">参数</param>
        /// <returns>受影响的记录数</returns>
        public static int InsertInpChangeInfo(List<OracleParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0)
                {
                    return -1;
                }
                List<OracleParameter> newParas = new List<OracleParameter>();
                string insertSQL = "insert into inpatientchangeinfo(id ";
                string valueSql = " values(seq_inpatientchangeinfo_id.nextval";

                if (Params.Any(p => p.ParameterName == "noofinpat"))
                {
                    insertSQL += ",noofinpat";
                    valueSql += ",@noofinpat";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "noofinpat"));
                }
                if (Params.Any(p => p.ParameterName == "newdeptid"))
                {
                    insertSQL += ",newdeptid";
                    valueSql += ",@newdeptid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "newdeptid"));
                }
                if (Params.Any(p => p.ParameterName == "newwardid"))
                {
                    insertSQL += ",newwardid";
                    valueSql += ",@newwardid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "newwardid"));
                }
                if (Params.Any(p => p.ParameterName == "newbedid"))
                {
                    insertSQL += ",newbedid";
                    valueSql += ",@newbedid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "newbedid"));
                }
                if (Params.Any(p => p.ParameterName == "olddeptid"))
                {
                    insertSQL += ",olddeptid";
                    valueSql += ",@olddeptid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "olddeptid"));
                }
                if (Params.Any(p => p.ParameterName == "oldwardid"))
                {
                    insertSQL += ",oldwardid";
                    valueSql += ",@oldwardid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "oldwardid"));
                }
                if (Params.Any(p => p.ParameterName == "oldbedid"))
                {
                    insertSQL += ",oldbedid";
                    valueSql += ",@oldbedid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "oldbedid"));
                }
                if (Params.Any(p => p.ParameterName == "changestyle"))
                {
                    insertSQL += ",changestyle";
                    valueSql += ",@changestyle";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "changestyle"));
                }
                if (Params.Any(p => p.ParameterName == "createuser"))
                {
                    insertSQL += ",createuser";
                    valueSql += ",@createuser";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createuser"));
                }
                if (Params.Any(p => p.ParameterName == "createtime"))
                {
                    insertSQL += ",createtime";
                    valueSql += ",@createtime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createtime"));
                }
                if (Params.Any(p => p.ParameterName == "modifyuser"))
                {
                    insertSQL += ",modifyuser";
                    valueSql += ",@modifyuser";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "modifyuser"));
                }
                if (Params.Any(p => p.ParameterName == "modifytime"))
                {
                    insertSQL += ",modifytime";
                    valueSql += ",@modifytime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "modifytime"));
                }
                if (Params.Any(p => p.ParameterName == "valid"))
                {
                    insertSQL += ",valid";
                    valueSql += ",@valid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "valid"));
                }
                insertSQL += ")";
                valueSql += ")";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(insertSQL + valueSql, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 插入转科记录
        /// </summary>
        /// 注：所传参数名为对应数据库中字段名(必须包含ID)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-07</date>
        /// <param name="Params">参数</param>
        /// <returns>受影响的记录数</returns>
        public static int InsertInpChangeInfoInTran(List<OracleParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0)
                {
                    return -1;
                }
                List<OracleParameter> newParas = new List<OracleParameter>();
                string insertSQL = "insert into inpatientchangeinfo(id ";
                string valueSql = " values(@id";
                OracleParameter para = Params.FirstOrDefault(p => p.ParameterName == "id");
                if (null == para)
                {
                    return -1;
                }
                newParas.Add(para);
                if (Params.Any(p => p.ParameterName == "noofinpat"))
                {
                    insertSQL += ",noofinpat";
                    valueSql += ",@noofinpat";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "noofinpat"));
                }
                if (Params.Any(p => p.ParameterName == "newdeptid"))
                {
                    insertSQL += ",newdeptid";
                    valueSql += ",@newdeptid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "newdeptid"));
                }
                if (Params.Any(p => p.ParameterName == "newwardid"))
                {
                    insertSQL += ",newwardid";
                    valueSql += ",@newwardid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "newwardid"));
                }
                if (Params.Any(p => p.ParameterName == "newbedid"))
                {
                    insertSQL += ",newbedid";
                    valueSql += ",@newbedid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "newbedid"));
                }
                if (Params.Any(p => p.ParameterName == "olddeptid"))
                {
                    insertSQL += ",olddeptid";
                    valueSql += ",@olddeptid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "olddeptid"));
                }
                if (Params.Any(p => p.ParameterName == "oldwardid"))
                {
                    insertSQL += ",oldwardid";
                    valueSql += ",@oldwardid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "oldwardid"));
                }
                if (Params.Any(p => p.ParameterName == "oldbedid"))
                {
                    insertSQL += ",oldbedid";
                    valueSql += ",@oldbedid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "oldbedid"));
                }
                if (Params.Any(p => p.ParameterName == "changestyle"))
                {
                    insertSQL += ",changestyle";
                    valueSql += ",@changestyle";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "changestyle"));
                }
                if (Params.Any(p => p.ParameterName == "createuser"))
                {
                    insertSQL += ",createuser";
                    valueSql += ",@createuser";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createuser"));
                }
                if (Params.Any(p => p.ParameterName == "createtime"))
                {
                    insertSQL += ",createtime";
                    valueSql += ",@createtime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createtime"));
                }
                if (Params.Any(p => p.ParameterName == "modifyuser"))
                {
                    insertSQL += ",modifyuser";
                    valueSql += ",@modifyuser";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "modifyuser"));
                }
                if (Params.Any(p => p.ParameterName == "modifytime"))
                {
                    insertSQL += ",modifytime";
                    valueSql += ",@modifytime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "modifytime"));
                }
                if (Params.Any(p => p.ParameterName == "valid"))
                {
                    insertSQL += ",valid";
                    valueSql += ",@valid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "valid"));
                }
                insertSQL += ")";
                valueSql += ")";

                return DS_SqlHelper.ExecuteNonQueryInTran(insertSQL + valueSql, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取category_detail中的婚姻状况
        /// xll 2013-07-04
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMarryDataCate()
        {
            try
            {
                string sqlStr = "select  c.detailid,c.name from dictionary_detail c where c.categoryid='4' and valid='1'";
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        #region 关于婚姻HQMS
        /// <summary>
        /// 获取婚姻信息
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-26</date>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DataTable GetMarryData()
        {
            try
            {
                string sqlStr = " select distinct rcvalue,rcdescription from HQMS_RCMAPPING where rctablename = 'RC002' order by rcvalue ";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取数据库原版婚姻代码获取医管司标准婚姻代码
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-26</date>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStandardMarryID(string systemID)
        {
            try
            {
                string resultCode = string.Empty;
                if (null != systemID && !string.IsNullOrEmpty(systemID.Trim()))
                {
                    string sqlStr = " select * from HQMS_RCMAPPING where rctablename = 'RC002' and sysrealvalue = @systemID ";
                    DbParameter[] sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@systemID",SqlDbType.Char)
                    };
                    sqlParams[0].Value = systemID;
                    DS_SqlHelper.CreateSqlHelper();
                    DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        resultCode = null != dt.Rows[0]["rcvalue"] ? dt.Rows[0]["rcvalue"].ToString().Trim() : string.Empty;
                    }
                    else
                    {
                        string sqlStr2 = " select distinct rcvalue,rcdescription from HQMS_RCMAPPING where rctablename = 'RC002' and rcdescription in('其他','其它') order by rcvalue ";
                        DS_SqlHelper.CreateSqlHelper();
                        DataTable dt2 = DS_SqlHelper.ExecuteDataTable(sqlStr2, CommandType.Text);
                        if (null != dt2 && dt2.Rows.Count > 0)
                        {
                            resultCode = null != dt2.Rows[0]["rcvalue"] ? dt2.Rows[0]["rcvalue"].ToString().Trim() : string.Empty;
                        }
                    }
                }
                return resultCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取医管司标准婚姻代码获取数据库原版婚姻代码
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-26</date>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSystemMarryID(string standardID)
        {
            try
            {
                string resultCode = string.Empty;
                if (null != standardID && !string.IsNullOrEmpty(standardID.Trim()))
                {
                    string sqlStr = " select * from HQMS_RCMAPPING where rctablename = 'RC002' and rcvalue = @standardID ";
                    DbParameter[] sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@standardID",SqlDbType.Char)
                    };
                    sqlParams[0].Value = standardID;
                    DS_SqlHelper.CreateSqlHelper();
                    DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        resultCode = null != dt.Rows[0]["sysrealvalue"] ? dt.Rows[0]["sysrealvalue"].ToString().Trim() : string.Empty;
                    }
                    else
                    {
                        //HQMS值域表中处理掉错误数据 add by ywk 2013年6月8日13:46:52 
                        string sqlStr2 = @" select distinct replace(sysrealvalue,'$NULL','')as sysrealvalue,rcdescription from HQMS_RCMAPPING where 
                        rctablename = 'RC002' and rcdescription in('其他','其它')  order by sysrealvalue desc ";
                        DS_SqlHelper.CreateSqlHelper();
                        DataTable dt2 = DS_SqlHelper.ExecuteDataTable(sqlStr2, CommandType.Text);
                        if (null != dt2 && dt2.Rows.Count > 0)
                        {
                            resultCode = null != dt2.Rows[0]["sysrealvalue"] ? dt2.Rows[0]["sysrealvalue"].ToString().Trim() : string.Empty;
                        }
                    }
                }
                return resultCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion

        #region 关于病案首页
        /// <summary>
        /// 更新病人的出科检查标识
        /// </summary>
        /// <param name="p"></param>
        /// <returns>受影响的记录数</returns>
        public static int UpdateOutDeptCheckFlag(int noofinpat, bool flag)
        {
            try
            {
                string sqlStr = @"update iem_mainpage_basicinfo_2012 set PatFlag=@patflag where noofinpat=@noofinpat and valide=1 ";

                OracleParameter[] paras = new OracleParameter[]
                {
                    new OracleParameter("@noofinpat",OracleType.Int32),
                    new OracleParameter("@patflag",OracleType.VarChar)
                };
                paras[0].Value = noofinpat;
                paras[1].Value = flag == true ? "1" : "0";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(sqlStr, paras, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 关于病历
        #region 病历相关查询
        /// <summary>
        /// 根据ID获取病历
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-19</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetRecordByID(int id)
        {
            try
            {
                if (id < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where valid = 1 and id=@id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.Int)
                };
                sqlParams[0].Value = id;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据ID获取病历(包含已删除)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-20</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetRecordByIDContainsDel(int id)
        {
            try
            {
                if (id < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where id=@id ";
                OracleParameter[] sqlParams = new OracleParameter[]
                {
                    new OracleParameter("@id",OracleType.Int32)
                };
                sqlParams[0].Value = id;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据ID获取病历
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-19</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetRecordByIDInTran(int id)
        {
            try
            {
                if (id < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where valid = 1 and id=@id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.Int)
                };
                sqlParams[0].Value = id;

                return DS_SqlHelper.ExecuteDataTableInTran(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据首页序号获取所有病历(包含非病程)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-17</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetAllRecordsByNoofinpat(int noofinpat)
        {
            try
            {
                if (noofinpat < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where valid = 1 and noofinpat=@noofinpat order by sortid,captiondatetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据首页序号获取所有病历(不包含逻辑删除)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetRecordsByNoofinpat(int noofinpat)
        {
            try
            {
                if (noofinpat < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where valid = 1 and sortid = 'AC' and noofinpat=@noofinpat order by captiondatetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据病案号查询该病人的病程记录数
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public static int GetRecordsCount(int noofinpat)
        {
            try
            {
                string sqlStr = " select count(*) from recorddetail where noofinpat=@noofinpat and sortid='AC' and valid=1 ";
                OracleParameter[] sqlParams = new OracleParameter[]
                {
                    new OracleParameter("@noofinpat",OracleType.Int32)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                object obj = DS_SqlHelper.ExecuteScalar(sqlStr, sqlParams, CommandType.Text);
                return null == obj ? 0 : Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据首页序号获取所有病历
        /// 注：只查询id,departcode,wardcode,captiondatetime,owner列
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetRecordTimesByNoofinpat(int noofinpat)
        {
            try
            {
                if (noofinpat < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select id,departcode,wardcode,captiondatetime,owner,firstdailyflag,valid from recorddetail where valid = 1 and sortid = 'AC' and noofinpat=@noofinpat order by captiondatetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据首页序号获取所有病历(包含已删除)
        /// 注：只查询id,departcode,wardcode,captiondatetime,owner列
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetRecordTimesContainsDel(int noofinpat)
        {
            try
            {
                if (noofinpat < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select id,departcode,wardcode,captiondatetime,owner,firstdailyflag,valid from recorddetail where noofinpat=@noofinpat and sortid = 'AC' order by captiondatetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据首页序号获取所有病历(不包含逻辑删除)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetRecordsByNoofinpatInTran(int noofinpat)
        {
            try
            {
                if (noofinpat < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where valid = 1 and sortid = 'AC' and noofinpat=@noofinpat order by captiondatetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                return DS_SqlHelper.ExecuteDataTableInTran(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据首页序号获取所有病历(包含逻辑删除)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetRecordsByNoofinpatContainDel(int noofinpat)
        {
            try
            {
                if (noofinpat < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where noofinpat=@noofinpat and sortid = 'AC' order by captiondatetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据首页序号获取所有病历(包含逻辑删除)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataTable GetRecordsByNoofinpatContainDelInTran(int noofinpat)
        {
            try
            {
                if (noofinpat < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where noofinpat=@noofinpat and sortid = 'AC' order by captiondatetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;

                return DS_SqlHelper.ExecuteDataTableInTran(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据ID获取病历内容
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetRecordContentsByID(int id)
        {
            try
            {
                string content = string.Empty;
                DataTable dt = GetRecordByID(id);
                if (null != dt && dt.Rows.Count > 0)
                {
                    content = dt.Rows[0]["content"].ToString();
                }
                return content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据ID获取病历内容 --- 事务专用
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetRecordContentsByIDInTran(int id)
        {
            try
            {
                string content = string.Empty;
                DataTable dt = GetRecordByIDInTran(id);
                if (null != dt && dt.Rows.Count > 0)
                {
                    content = dt.Rows[0]["content"].ToString();
                }
                return content;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据科室获取某病人的所有首次病程
        /// 默认排序：captiondatetime升序
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-19</date>
        /// <param name="noofinpat">病案号</param>
        /// <param name="deptCode">科室</param>
        /// <returns></returns>
        public static DataTable GetFirstRecordsByDept(int noofinpat, string deptCode)
        {
            try
            {
                string sqlStr = " select * from recorddetail where valid=1 and sortid='AC' and firstdailyflag='1' and noofinpat=@noofinpat ";
                DbParameter[] sqlParams = null;
                if (!string.IsNullOrEmpty(deptCode))
                {
                    sqlStr += " and departcode = @deptCode ";
                    sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@noofinpat",SqlDbType.Int),
                        new SqlParameter("@deptCode",SqlDbType.Char)
                    };
                    sqlParams[0].Value = noofinpat;
                    sqlParams[1].Value = deptCode;
                }
                else
                {
                    sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@noofinpat",SqlDbType.Int)
                    };
                    sqlParams[0].Value = noofinpat;
                }
                sqlStr += " order by captiondatetime asc ";

                DS_SqlHelper.CreateSqlHelper();

                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据病案号获取某病人的所有首次病程(病历修改标识专用)
        /// 注：只查ID、startupdateflag,endupdateflag三列
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-19</date>
        /// <param name="noofinpat">病案号</param>
        /// <returns></returns>
        public static DataTable GetFirstRecordsForSpeed(int noofinpat)
        {
            try
            {
                string sqlStr = " select id,startupdateflag,endupdateflag from recorddetail where noofinpat=@noofinpat and firstdailyflag='1' and sortid='AC' and valid=1 order by captiondatetime asc";
                DbParameter[] sqlParams = null;
                sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据科室获取某病人的所有首次病程
        /// 默认排序：captiondatetime升序
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-19</date>
        /// <param name="noofinpat">病案号</param>
        /// <param name="deptCode">科室</param>
        /// <returns></returns>
        public static DataTable GetFirstRecordsByDeptInTran(int noofinpat, string deptCode)
        {
            try
            {
                string sqlStr = " select * from recorddetail where valid=1 and sortid='AC' and firstdailyflag='1' and noofinpat=@noofinpat ";
                DbParameter[] sqlParams = null;
                if (!string.IsNullOrEmpty(deptCode))
                {
                    sqlStr += " and departcode = @deptCode ";
                    sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@noofinpat",SqlDbType.Int),
                        new SqlParameter("@deptCode",SqlDbType.Char)
                    };
                    sqlParams[0].Value = noofinpat;
                    sqlParams[1].Value = deptCode;
                }
                else
                {
                    sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@noofinpat",SqlDbType.Int)
                    };
                    sqlParams[0].Value = noofinpat;
                }
                sqlStr += " order by captiondatetime asc ";

                return DS_SqlHelper.ExecuteDataTableInTran(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据病案号获取病人某段时间内的所有病历
        /// 默认排序：captiondatetime升序
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-19</date>
        /// <param name="noofinpat">病案号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public static DataTable GetRecordsByTimeDiv(int noofinpat, DateTime? starttime, DateTime? endtime)
        {
            try
            {
                string sqlStr = " select * from recorddetail where valid=1 and sortid='AC' and noofinpat=@noofinpat ";
                List<DbParameter> parameters = new List<DbParameter>();
                SqlParameter param1 = new SqlParameter("@noofinpat", SqlDbType.Int);
                param1.Value = noofinpat;
                parameters.Add(param1);
                if (null != starttime)
                {
                    sqlStr += " and captiondatetime >= @starttime ";
                    SqlParameter param2 = new SqlParameter("@starttime", SqlDbType.Char);
                    param2.Value = null == starttime ? "1900-01-01 00:00:00" : ((DateTime)starttime).ToString("yyyy-MM-dd HH:mm:ss");
                    parameters.Add(param2);
                }
                if (null != endtime)
                {
                    sqlStr += " and captiondatetime <= @endtime ";
                    SqlParameter param3 = new SqlParameter("@endtime", SqlDbType.Char);
                    param3.Value = null == endtime ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : ((DateTime)endtime).ToString("yyyy-MM-dd HH:mm:ss");
                    parameters.Add(param3);
                }
                sqlStr += " order by captiondatetime asc ";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据病案号获取病人某段时间内的所有病历
        /// 默认排序：captiondatetime升序
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-11</date>
        /// <param name="noofinpat">病案号</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public static DataTable GetRecordsByTimeDivInTran(int noofinpat, DateTime? starttime, DateTime? endtime)
        {
            try
            {
                string sqlStr = " select * from recorddetail where valid=1 and sortid='AC' and noofinpat=@noofinpat ";
                List<DbParameter> parameters = new List<DbParameter>();
                SqlParameter param1 = new SqlParameter("@noofinpat", SqlDbType.Int);
                param1.Value = noofinpat;
                parameters.Add(param1);
                if (null != starttime)
                {
                    sqlStr += " and captiondatetime >= @starttime ";
                    SqlParameter param2 = new SqlParameter("@starttime", SqlDbType.Char);
                    param2.Value = null == starttime ? "1900-01-01 00:00:00" : ((DateTime)starttime).ToString("yyyy-MM-dd HH:mm:ss");
                    parameters.Add(param2);
                }
                if (null != endtime)
                {
                    sqlStr += " and captiondatetime <= @endtime ";
                    SqlParameter param3 = new SqlParameter("@endtime", SqlDbType.Char);
                    param3.Value = null == endtime ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : ((DateTime)endtime).ToString("yyyy-MM-dd HH:mm:ss");
                    parameters.Add(param3);
                }
                sqlStr += " order by captiondatetime asc ";

                return DS_SqlHelper.ExecuteDataTableInTran(sqlStr, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据时间获取病历
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-14</date>
        /// <param name="noofinpat">首页序号</param>
        /// <param name="theTime">yyyy-MM-dd HH:mm:ss</param>
        /// <returns></returns>
        public static DataTable GetRecordByDateTimeInTran(int noofinpat, string theTime)
        {
            try
            {
                if (noofinpat < 0 || string.IsNullOrEmpty(theTime))
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where valid = 1 and sortid = 'AC' and noofinpat = @noofinpat and captiondatetime = @theTime order by captiondatetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int),
                    new SqlParameter("@theTime",SqlDbType.Char)
                };
                sqlParams[0].Value = noofinpat;
                sqlParams[1].Value = theTime;

                return DS_SqlHelper.ExecuteDataTableInTran(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据多个时间获取病历
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-12</date>
        /// <param name="noofinpat">首页序号</param>
        /// <param name="theTimes">'yyyy-MM-dd HH:mm:ss','yyyy-MM-dd HH:mm:ss'...</param>
        /// <returns></returns>
        public static DataTable GetRecordByDateTimes(int noofinpat, string theTimes)
        {
            try
            {
                if (noofinpat < 0 || string.IsNullOrEmpty(theTimes))
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where valid = 1 and sortid = 'AC' and noofinpat=@noofinpat and captiondatetime in(" + theTimes + ") order by captiondatetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据多个时间获取病历
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-12</date>
        /// <param name="noofinpat">首页序号</param>
        /// <param name="theTimes">'yyyy-MM-dd HH:mm:ss','yyyy-MM-dd HH:mm:ss'...</param>
        /// <returns></returns>
        public static DataTable GetRecordByDateTimesInTran(int noofinpat, string theTimes)
        {
            try
            {
                if (noofinpat < 0 || string.IsNullOrEmpty(theTimes))
                {
                    return new DataTable();
                }
                string sqlStr = " select * from recorddetail where valid = 1 and sortid = 'AC' and noofinpat=@noofinpat and captiondatetime in(" + theTimes + ") order by captiondatetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                return DS_SqlHelper.ExecuteDataTableInTran(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 查询病历表的下一个ID
        /// </summary>
        /// <returns></returns>
        public static int GetNextIdFromRecordDetail()
        {
            try
            {
                string sqlStr = "select seq_recorddetail_id.nextval from dual";
                DS_SqlHelper.CreateSqlHelper();
                object obj = DS_SqlHelper.ExecuteScalar(sqlStr, CommandType.Text);
                return Convert.ToInt32(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据首页序号获取首次病程
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-17</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DataRow GetFirstDailyEmr(int noofinpat)
        {
            try
            {
                string sqlStr = " select id,noofinpat,name,sortid,owner,createtime,captiondatetime,departcode,wardcode,changeid from recorddetail where noofinpat=@noofinpat and sortid='AC' and firstdailyflag='1' and valid=1 ";
                //string sqlStr = " select id,noofinpat,name,sortid,owner,createtime,captiondatetime,departcode,wardcode,changeid from recorddetail where (noofinpat =@noofinpat  or noofinpat in(select mother from inpatient where noofinpat= @noofinpat)) and sortid='AC' and firstdailyflag='1' and valid=1 ";
                OracleParameter[] pars = new OracleParameter[]
                {
                    new OracleParameter("@noofinpat",OracleType.Int32)
                };
                pars[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, pars, CommandType.Text);
                return (null != dt && dt.Rows.Count > 0) ? dt.Rows[0] : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 病历相关操作
        /// <summary>
        /// 插入病历
        /// </summary>
        /// 注：1、所传参数名为对应数据库中字段名
        ///     2、返回值为插入病历id
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-08</date>
        /// <param name="Params">参数</param>
        /// <return>插入病历id</return>
        public static int InsertRecord(List<OracleParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0)
                {
                    return -1;
                }
                List<OracleParameter> newParas = new List<OracleParameter>();
                string insertSQL = "insert into recorddetail(id ";
                string valueSql = " values(@id";

                ///id为必备项
                OracleParameter param;
                if (!Params.Any(p => p.ParameterName == "id"))
                {
                    param = new OracleParameter("id", OracleType.Int32);
                    param.Value = GetNextIdFromRecordDetail();
                    newParas.Add(param);
                }
                else
                {
                    param = Params.FirstOrDefault(p => p.ParameterName == "id");
                    newParas.Add(param);
                }
                if (Params.Any(p => p.ParameterName == "noofinpat"))
                {
                    insertSQL += ",noofinpat";
                    valueSql += ",@noofinpat";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "noofinpat"));
                }
                if (Params.Any(p => p.ParameterName == "templateid"))
                {
                    insertSQL += ",templateid";
                    valueSql += ",@templateid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "templateid"));
                }
                if (Params.Any(p => p.ParameterName == "name"))
                {
                    insertSQL += ",name";
                    valueSql += ",@name";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "name"));
                }
                if (Params.Any(p => p.ParameterName == "recorddesc"))
                {
                    insertSQL += ",recorddesc";
                    valueSql += ",@recorddesc";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "recorddesc"));
                }
                if (Params.Any(p => p.ParameterName == "content"))
                {
                    insertSQL += ",content";
                    valueSql += ",@content";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "content"));
                }
                if (Params.Any(p => p.ParameterName == "sortid"))
                {
                    insertSQL += ",sortid";
                    valueSql += ",@sortid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "sortid"));
                }
                if (Params.Any(p => p.ParameterName == "owner"))
                {
                    insertSQL += ",owner";
                    valueSql += ",@owner";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "owner"));
                }
                if (Params.Any(p => p.ParameterName == "auditor"))
                {
                    insertSQL += ",auditor";
                    valueSql += ",@auditor";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "auditor"));
                }
                if (Params.Any(p => p.ParameterName == "createtime"))
                {
                    insertSQL += ",createtime";
                    valueSql += ",@createtime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createtime"));
                }
                if (Params.Any(p => p.ParameterName == "audittime"))
                {
                    insertSQL += ",audittime";
                    valueSql += ",@audittime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "audittime"));
                }
                if (Params.Any(p => p.ParameterName == "valid"))
                {
                    insertSQL += ",valid";
                    valueSql += ",@valid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "valid"));
                }
                if (Params.Any(p => p.ParameterName == "hassubmit"))
                {
                    insertSQL += ",hassubmit";
                    valueSql += ",@hassubmit";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassubmit"));
                }
                if (Params.Any(p => p.ParameterName == "hasprint"))
                {
                    insertSQL += ",hasprint";
                    valueSql += ",@hasprint";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hasprint"));
                }
                if (Params.Any(p => p.ParameterName == "hassign"))
                {
                    insertSQL += ",hassign";
                    valueSql += ",@hassign";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassign"));
                }
                if (Params.Any(p => p.ParameterName == "captiondatetime"))
                {
                    insertSQL += ",captiondatetime";
                    valueSql += ",@captiondatetime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "captiondatetime"));
                }
                if (Params.Any(p => p.ParameterName == "islock"))
                {
                    insertSQL += ",islock";
                    valueSql += ",@islock";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "islock"));
                }
                if (Params.Any(p => p.ParameterName == "firstdailyflag"))
                {
                    insertSQL += ",firstdailyflag";
                    valueSql += ",@firstdailyflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "firstdailyflag"));
                }
                if (Params.Any(p => p.ParameterName == "isyihuangoutong"))
                {
                    insertSQL += ",isyihuangoutong";
                    valueSql += ",@isyihuangoutong";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isyihuangoutong"));
                }
                if (Params.Any(p => p.ParameterName == "ip"))
                {
                    insertSQL += ",ip";
                    valueSql += ",@ip";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "ip"));
                }
                if (Params.Any(p => p.ParameterName == "isconfigpagesize"))
                {
                    insertSQL += ",isconfigpagesize";
                    valueSql += ",@isconfigpagesize";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isconfigpagesize"));
                }
                if (Params.Any(p => p.ParameterName == "openflag"))
                {
                    insertSQL += ",openflag";
                    valueSql += ",@openflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "openflag"));
                }
                if (Params.Any(p => p.ParameterName == "departcode"))
                {
                    insertSQL += ",departcode";
                    valueSql += ",@departcode";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "departcode"));
                }
                if (Params.Any(p => p.ParameterName == "wardcode"))
                {
                    insertSQL += ",wardcode";
                    valueSql += ",@wardcode";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "wardcode"));
                }
                if (Params.Any(p => p.ParameterName == "startupdateflag"))
                {
                    insertSQL += ",startupdateflag";
                    valueSql += ",@startupdateflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "startupdateflag"));
                }
                if (Params.Any(p => p.ParameterName == "endupdateflag"))
                {
                    insertSQL += ",endupdateflag";
                    valueSql += ",@endupdateflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "endupdateflag"));
                }
                if (Params.Any(p => p.ParameterName == "isemrcontentsplit"))
                {
                    insertSQL += ",isemrcontentsplit";
                    valueSql += ",@isemrcontentsplit";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isemrcontentsplit"));
                }
                if (Params.Any(p => p.ParameterName == "changeid"))
                {
                    insertSQL += ",changeid";
                    valueSql += ",@changeid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "changeid"));
                }
                insertSQL += ")";
                valueSql += ")";

                DS_SqlHelper.CreateSqlHelper();
                int result = DS_SqlHelper.ExecuteNonQuery(insertSQL + valueSql, newParas, CommandType.Text);
                if (result != 1)
                {
                    return -1;
                }
                return Convert.ToInt32(param.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入病历
        /// </summary>
        /// 注：1、所传参数名为对应数据库中字段名(参数中必须包含id)
        ///     2、返回值为插入病历id
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-03</date>
        /// <param name="Params">参数</param>
        /// <return>插入病历id</return>
        public static int InsertRecordNewInTran(List<OracleParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0)
                {
                    return -1;
                }
                List<OracleParameter> newParas = new List<OracleParameter>();
                string insertSQL = "insert into recorddetail(id ";
                string valueSql = " values(@id";

                ///id为必备项
                if (!Params.Any(p => p.ParameterName == "id"))
                {
                    return -1;
                }
                OracleParameter param = Params.FirstOrDefault(p => p.ParameterName == "id");
                newParas.Add(param);
                if (Params.Any(p => p.ParameterName == "noofinpat"))
                {
                    insertSQL += ",noofinpat";
                    valueSql += ",@noofinpat";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "noofinpat"));
                }
                if (Params.Any(p => p.ParameterName == "templateid"))
                {
                    insertSQL += ",templateid";
                    valueSql += ",@templateid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "templateid"));
                }
                if (Params.Any(p => p.ParameterName == "name"))
                {
                    insertSQL += ",name";
                    valueSql += ",@name";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "name"));
                }
                if (Params.Any(p => p.ParameterName == "recorddesc"))
                {
                    insertSQL += ",recorddesc";
                    valueSql += ",@recorddesc";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "recorddesc"));
                }
                if (Params.Any(p => p.ParameterName == "content"))
                {
                    insertSQL += ",content";
                    valueSql += ",@content";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "content"));
                }
                if (Params.Any(p => p.ParameterName == "sortid"))
                {
                    insertSQL += ",sortid";
                    valueSql += ",@sortid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "sortid"));
                }
                if (Params.Any(p => p.ParameterName == "owner"))
                {
                    insertSQL += ",owner";
                    valueSql += ",@owner";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "owner"));
                }
                if (Params.Any(p => p.ParameterName == "auditor"))
                {
                    insertSQL += ",auditor";
                    valueSql += ",@auditor";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "auditor"));
                }
                if (Params.Any(p => p.ParameterName == "createtime"))
                {
                    insertSQL += ",createtime";
                    valueSql += ",@createtime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createtime"));
                }
                if (Params.Any(p => p.ParameterName == "audittime"))
                {
                    insertSQL += ",audittime";
                    valueSql += ",@audittime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "audittime"));
                }
                if (Params.Any(p => p.ParameterName == "valid"))
                {
                    insertSQL += ",valid";
                    valueSql += ",@valid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "valid"));
                }
                if (Params.Any(p => p.ParameterName == "hassubmit"))
                {
                    insertSQL += ",hassubmit";
                    valueSql += ",@hassubmit";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassubmit"));
                }
                if (Params.Any(p => p.ParameterName == "hasprint"))
                {
                    insertSQL += ",hasprint";
                    valueSql += ",@hasprint";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hasprint"));
                }
                if (Params.Any(p => p.ParameterName == "hassign"))
                {
                    insertSQL += ",hassign";
                    valueSql += ",@hassign";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassign"));
                }
                if (Params.Any(p => p.ParameterName == "captiondatetime"))
                {
                    insertSQL += ",captiondatetime";
                    valueSql += ",@captiondatetime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "captiondatetime"));
                }
                if (Params.Any(p => p.ParameterName == "islock"))
                {
                    insertSQL += ",islock";
                    valueSql += ",@islock";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "islock"));
                }
                if (Params.Any(p => p.ParameterName == "firstdailyflag"))
                {
                    insertSQL += ",firstdailyflag";
                    valueSql += ",@firstdailyflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "firstdailyflag"));
                }
                if (Params.Any(p => p.ParameterName == "isyihuangoutong"))
                {
                    insertSQL += ",isyihuangoutong";
                    valueSql += ",@isyihuangoutong";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isyihuangoutong"));
                }
                if (Params.Any(p => p.ParameterName == "ip"))
                {
                    insertSQL += ",ip";
                    valueSql += ",@ip";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "ip"));
                }
                if (Params.Any(p => p.ParameterName == "isconfigpagesize"))
                {
                    insertSQL += ",isconfigpagesize";
                    valueSql += ",@isconfigpagesize";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isconfigpagesize"));
                }
                if (Params.Any(p => p.ParameterName == "openflag"))
                {
                    insertSQL += ",openflag";
                    valueSql += ",@openflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "openflag"));
                }
                if (Params.Any(p => p.ParameterName == "departcode"))
                {
                    insertSQL += ",departcode";
                    valueSql += ",@departcode";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "departcode"));
                }
                if (Params.Any(p => p.ParameterName == "wardcode"))
                {
                    insertSQL += ",wardcode";
                    valueSql += ",@wardcode";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "wardcode"));
                }
                if (Params.Any(p => p.ParameterName == "startupdateflag"))
                {
                    insertSQL += ",startupdateflag";
                    valueSql += ",@startupdateflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "startupdateflag"));
                }
                if (Params.Any(p => p.ParameterName == "endupdateflag"))
                {
                    insertSQL += ",endupdateflag";
                    valueSql += ",@endupdateflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "endupdateflag"));
                }
                if (Params.Any(p => p.ParameterName == "isemrcontentsplit"))
                {
                    insertSQL += ",isemrcontentsplit";
                    valueSql += ",@isemrcontentsplit";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isemrcontentsplit"));
                }
                if (Params.Any(p => p.ParameterName == "changeid"))
                {
                    insertSQL += ",changeid";
                    valueSql += ",@changeid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "changeid"));
                }
                insertSQL += ")";
                valueSql += ")";

                int result = DS_SqlHelper.ExecuteNonQueryInTran(insertSQL + valueSql, newParas, CommandType.Text);
                if (result != 1)
                {
                    return -1;
                }
                return Convert.ToInt32(param.Value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 插入病历
        /// </summary>
        /// 注：1、所传参数名为对应数据库中字段名
        ///     2、返回值为插入病历ID
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-14</date>
        /// <param name="Params">参数</param>
        /// <return>插入病历ID</return>
        public static int InsertRecordInTran(List<OracleParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0)
                {
                    return -1;
                }
                List<OracleParameter> newParas = new List<OracleParameter>();
                string insertSQL = "insert into recorddetail(id ";
                string valueSql = " values((select NVL(MAX(ID), 0) + 1 as id from recorddetail)";

                if (Params.Any(p => p.ParameterName == "noofinpat"))
                {
                    insertSQL += ",noofinpat";
                    valueSql += ",@noofinpat";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "noofinpat"));
                }
                if (Params.Any(p => p.ParameterName == "templateid"))
                {
                    insertSQL += ",templateid";
                    valueSql += ",@templateid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "templateid"));
                }
                if (Params.Any(p => p.ParameterName == "name"))
                {
                    insertSQL += ",name";
                    valueSql += ",@name";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "name"));
                }
                if (Params.Any(p => p.ParameterName == "recorddesc"))
                {
                    insertSQL += ",recorddesc";
                    valueSql += ",@recorddesc";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "recorddesc"));
                }
                if (Params.Any(p => p.ParameterName == "content"))
                {
                    insertSQL += ",content";
                    valueSql += ",@content";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "content"));
                }
                if (Params.Any(p => p.ParameterName == "sortid"))
                {
                    insertSQL += ",sortid";
                    valueSql += ",@sortid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "sortid"));
                }
                if (Params.Any(p => p.ParameterName == "owner"))
                {
                    insertSQL += ",owner";
                    valueSql += ",@owner";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "owner"));
                }
                if (Params.Any(p => p.ParameterName == "auditor"))
                {
                    insertSQL += ",auditor";
                    valueSql += ",@auditor";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "auditor"));
                }
                if (Params.Any(p => p.ParameterName == "createtime"))
                {
                    insertSQL += ",createtime";
                    valueSql += ",@createtime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createtime"));
                }
                if (Params.Any(p => p.ParameterName == "audittime"))
                {
                    insertSQL += ",audittime";
                    valueSql += ",@audittime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "audittime"));
                }
                if (Params.Any(p => p.ParameterName == "valid"))
                {
                    insertSQL += ",valid";
                    valueSql += ",@valid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "valid"));
                }
                if (Params.Any(p => p.ParameterName == "hassubmit"))
                {
                    insertSQL += ",hassubmit";
                    valueSql += ",@hassubmit";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassubmit"));
                }
                if (Params.Any(p => p.ParameterName == "hasprint"))
                {
                    insertSQL += ",hasprint";
                    valueSql += ",@hasprint";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hasprint"));
                }
                if (Params.Any(p => p.ParameterName == "hassign"))
                {
                    insertSQL += ",hassign";
                    valueSql += ",@hassign";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassign"));
                }
                if (Params.Any(p => p.ParameterName == "captiondatetime"))
                {
                    insertSQL += ",captiondatetime";
                    valueSql += ",@captiondatetime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "captiondatetime"));
                }
                if (Params.Any(p => p.ParameterName == "islock"))
                {
                    insertSQL += ",islock";
                    valueSql += ",@islock";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "islock"));
                }
                if (Params.Any(p => p.ParameterName == "firstdailyflag"))
                {
                    insertSQL += ",firstdailyflag";
                    valueSql += ",@firstdailyflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "firstdailyflag"));
                }
                if (Params.Any(p => p.ParameterName == "isyihuangoutong"))
                {
                    insertSQL += ",isyihuangoutong";
                    valueSql += ",@isyihuangoutong";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isyihuangoutong"));
                }
                if (Params.Any(p => p.ParameterName == "ip"))
                {
                    insertSQL += ",ip";
                    valueSql += ",@ip";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "ip"));
                }
                if (Params.Any(p => p.ParameterName == "isconfigpagesize"))
                {
                    insertSQL += ",isconfigpagesize";
                    valueSql += ",@isconfigpagesize";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isconfigpagesize"));
                }
                if (Params.Any(p => p.ParameterName == "openflag"))
                {
                    insertSQL += ",openflag";
                    valueSql += ",@openflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "openflag"));
                }
                if (Params.Any(p => p.ParameterName == "departcode"))
                {
                    insertSQL += ",departcode";
                    valueSql += ",@departcode";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "departcode"));
                }
                if (Params.Any(p => p.ParameterName == "wardcode"))
                {
                    insertSQL += ",wardcode";
                    valueSql += ",@wardcode";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "wardcode"));
                }
                if (Params.Any(p => p.ParameterName == "startupdateflag"))
                {
                    insertSQL += ",startupdateflag";
                    valueSql += ",@startupdateflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "startupdateflag"));
                }
                if (Params.Any(p => p.ParameterName == "endupdateflag"))
                {
                    insertSQL += ",endupdateflag";
                    valueSql += ",@endupdateflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "endupdateflag"));
                }
                if (Params.Any(p => p.ParameterName == "isemrcontentsplit"))
                {
                    insertSQL += ",isemrcontentsplit";
                    valueSql += ",@isemrcontentsplit";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isemrcontentsplit"));
                }
                if (Params.Any(p => p.ParameterName == "changeid"))
                {
                    insertSQL += ",changeid";
                    valueSql += ",@changeid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "changeid"));
                }

                insertSQL += ")";
                valueSql += ")";

                int num = -1;
                int result = DS_SqlHelper.ExecuteNonQueryInTran(insertSQL + valueSql, newParas, CommandType.Text);
                if (result == 1)
                {
                    object obj = DS_SqlHelper.ExecuteScalarInTran("select id from recorddetail order by id desc", CommandType.Text);
                    if (null != obj && !string.IsNullOrEmpty(obj.ToString()))
                    {
                        num = int.Parse(obj.ToString().Trim());
                    }
                }
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病历
        /// </summary>
        /// 注：1、所传参数名为对应数据库中字段名
        ///     2、只更新所传递的参数列(id为必须参数)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-15</date>
        /// <param name="Params">参数</param>
        /// <return>受影响的记录数</return>
        public static int UpdateRecord(List<OracleParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0 || !Params.Any(p => p.ParameterName == "id"))
                {
                    return 0;
                }
                List<OracleParameter> newParas = new List<OracleParameter>();
                string updateSQL = "update recorddetail set ";

                if (Params.Any(p => p.ParameterName == "noofinpat"))
                {
                    updateSQL += " noofinpat = @noofinpat,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "noofinpat"));
                }
                if (Params.Any(p => p.ParameterName == "templateid"))
                {
                    updateSQL += " templateid = @templateid,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "templateid"));
                }
                if (Params.Any(p => p.ParameterName == "name"))
                {
                    updateSQL += " name = @name,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "name"));
                }
                if (Params.Any(p => p.ParameterName == "recorddesc"))
                {
                    updateSQL += " recorddesc = @recorddesc,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "recorddesc"));
                }
                if (Params.Any(p => p.ParameterName == "content"))
                {
                    updateSQL += " content = @content,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "content"));
                }
                if (Params.Any(p => p.ParameterName == "sortid"))
                {
                    updateSQL += " sortid = @sortid,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "sortid"));
                }
                if (Params.Any(p => p.ParameterName == "owner"))
                {
                    updateSQL += " owner = @owner,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "owner"));
                }
                if (Params.Any(p => p.ParameterName == "auditor"))
                {
                    updateSQL += " auditor = @auditor,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "auditor"));
                }
                if (Params.Any(p => p.ParameterName == "createtime"))
                {
                    updateSQL += " createtime = @createtime,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createtime"));
                }
                if (Params.Any(p => p.ParameterName == "audittime"))
                {
                    updateSQL += " audittime = @audittime,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "audittime"));
                }
                if (Params.Any(p => p.ParameterName == "hassubmit"))
                {
                    updateSQL += " hassubmit = @hassubmit,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassubmit"));
                }
                if (Params.Any(p => p.ParameterName == "hasprint"))
                {
                    updateSQL += " hasprint = @hasprint,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hasprint"));
                }
                if (Params.Any(p => p.ParameterName == "hassign"))
                {
                    updateSQL += " hassign = @hassign,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassign"));
                }
                if (Params.Any(p => p.ParameterName == "captiondatetime"))
                {
                    updateSQL += " captiondatetime = @captiondatetime,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "captiondatetime"));
                }
                if (Params.Any(p => p.ParameterName == "islock"))
                {
                    updateSQL += " islock = @islock,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "islock"));
                }
                if (Params.Any(p => p.ParameterName == "firstdailyflag"))
                {
                    updateSQL += " firstdailyflag = @firstdailyflag,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "firstdailyflag"));
                }
                if (Params.Any(p => p.ParameterName == "isyihuangoutong"))
                {
                    updateSQL += " isyihuangoutong = @isyihuangoutong,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isyihuangoutong"));
                }
                if (Params.Any(p => p.ParameterName == "ip"))
                {
                    updateSQL += " ip = @ip,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "ip"));
                }
                if (Params.Any(p => p.ParameterName == "isconfigpagesize"))
                {
                    updateSQL += " isconfigpagesize = @isconfigpagesize,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isconfigpagesize"));
                }
                if (Params.Any(p => p.ParameterName == "openflag"))
                {
                    updateSQL += " openflag = @openflag,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "openflag"));
                }
                if (Params.Any(p => p.ParameterName == "departcode"))
                {
                    updateSQL += " departcode = @departcode,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "departcode"));
                }
                if (Params.Any(p => p.ParameterName == "wardcode"))
                {
                    updateSQL += " wardcode = @wardcode,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "wardcode"));
                }
                if (Params.Any(p => p.ParameterName == "startupdateflag"))
                {
                    updateSQL += " startupdateflag = @startupdateflag,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "startupdateflag"));
                }
                if (Params.Any(p => p.ParameterName == "endupdateflag"))
                {
                    updateSQL += " endupdateflag = @endupdateflag,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "endupdateflag"));
                }
                if (Params.Any(p => p.ParameterName == "isemrcontentsplit"))
                {
                    updateSQL += " isemrcontentsplit = @isemrcontentsplit,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isemrcontentsplit"));
                }
                if (Params.Any(p => p.ParameterName == "changeid"))
                {
                    updateSQL += " changeid = @changeid,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "changeid"));
                }
                if (Params.Any(p => p.ParameterName == "valid"))
                {
                    updateSQL += " valid = @valid ";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "valid"));
                }
                else
                {
                    updateSQL += " valid = 1 ";
                }
                updateSQL += " where id = @id ";
                newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "id"));

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(updateSQL, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病历
        /// </summary>
        /// 注：1、所传参数名为对应数据库中字段名
        ///     2、只更新所传递的参数列(id为必须参数)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-10</date>
        /// <param name="Params">参数</param>
        /// <return>受影响的记录数</return>
        public static int UpdateRecordInTran(List<OracleParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0 || !Params.Any(p => p.ParameterName == "id"))
                {
                    return 0;
                }
                List<OracleParameter> newParas = new List<OracleParameter>();
                string updateSQL = "update recorddetail set ";

                if (Params.Any(p => p.ParameterName == "noofinpat"))
                {
                    updateSQL += " noofinpat = @noofinpat,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "noofinpat"));
                }
                if (Params.Any(p => p.ParameterName == "templateid"))
                {
                    updateSQL += " templateid = @templateid,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "templateid"));
                }
                if (Params.Any(p => p.ParameterName == "name"))
                {
                    updateSQL += " name = @name,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "name"));
                }
                if (Params.Any(p => p.ParameterName == "recorddesc"))
                {
                    updateSQL += " recorddesc = @recorddesc,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "recorddesc"));
                }
                if (Params.Any(p => p.ParameterName == "content"))
                {
                    updateSQL += " content = @content,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "content"));
                }
                if (Params.Any(p => p.ParameterName == "sortid"))
                {
                    updateSQL += " sortid = @sortid,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "sortid"));
                }
                if (Params.Any(p => p.ParameterName == "owner"))
                {
                    updateSQL += " owner = @owner,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "owner"));
                }
                if (Params.Any(p => p.ParameterName == "auditor"))
                {
                    updateSQL += " auditor = @auditor,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "auditor"));
                }
                if (Params.Any(p => p.ParameterName == "createtime"))
                {
                    updateSQL += " createtime = @createtime,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createtime"));
                }
                if (Params.Any(p => p.ParameterName == "audittime"))
                {
                    updateSQL += " audittime = @audittime,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "audittime"));
                }
                if (Params.Any(p => p.ParameterName == "hassubmit"))
                {
                    updateSQL += " hassubmit = @hassubmit,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassubmit"));
                }
                if (Params.Any(p => p.ParameterName == "hasprint"))
                {
                    updateSQL += " hasprint = @hasprint,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hasprint"));
                }
                if (Params.Any(p => p.ParameterName == "hassign"))
                {
                    updateSQL += " hassign = @hassign,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassign"));
                }
                if (Params.Any(p => p.ParameterName == "captiondatetime"))
                {
                    updateSQL += " captiondatetime = @captiondatetime,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "captiondatetime"));
                }
                if (Params.Any(p => p.ParameterName == "islock"))
                {
                    updateSQL += " islock = @islock,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "islock"));
                }
                if (Params.Any(p => p.ParameterName == "firstdailyflag"))
                {
                    updateSQL += " firstdailyflag = @firstdailyflag,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "firstdailyflag"));
                }
                if (Params.Any(p => p.ParameterName == "isyihuangoutong"))
                {
                    updateSQL += " isyihuangoutong = @isyihuangoutong,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isyihuangoutong"));
                }
                if (Params.Any(p => p.ParameterName == "ip"))
                {
                    updateSQL += " ip = @ip,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "ip"));
                }
                if (Params.Any(p => p.ParameterName == "isconfigpagesize"))
                {
                    updateSQL += " isconfigpagesize = @isconfigpagesize,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isconfigpagesize"));
                }
                if (Params.Any(p => p.ParameterName == "openflag"))
                {
                    updateSQL += " openflag = @openflag,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "openflag"));
                }
                if (Params.Any(p => p.ParameterName == "departcode"))
                {
                    updateSQL += " departcode = @departcode,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "departcode"));
                }
                if (Params.Any(p => p.ParameterName == "wardcode"))
                {
                    updateSQL += " wardcode = @wardcode,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "wardcode"));
                }
                if (Params.Any(p => p.ParameterName == "startupdateflag"))
                {
                    updateSQL += " startupdateflag = @startupdateflag,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "startupdateflag"));
                }
                if (Params.Any(p => p.ParameterName == "endupdateflag"))
                {
                    updateSQL += " endupdateflag = @endupdateflag,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "endupdateflag"));
                }
                if (Params.Any(p => p.ParameterName == "isemrcontentsplit"))
                {
                    updateSQL += " isemrcontentsplit = @isemrcontentsplit,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isemrcontentsplit"));
                }
                if (Params.Any(p => p.ParameterName == "changeid"))
                {
                    updateSQL += " changeid = @changeid,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "changeid"));
                }
                if (Params.Any(p => p.ParameterName == "valid"))
                {
                    updateSQL += " valid = @valid ";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "valid"));
                }
                else
                {
                    updateSQL += " valid = 1 ";
                }
                updateSQL += " where id = @id ";
                newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "id"));

                return DS_SqlHelper.ExecuteNonQueryInTran(updateSQL, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病历的有效状态
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-10</date>
        /// <param name="id">病历ID</param>
        /// <param name="boo">是否有效</param>
        /// <return></return>
        public static void UpdateRecordValid(int id, bool boo)
        {
            try
            {
                string sqlStr = " update recorddetail set valid=@valid where id=@id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.Int),
                    new SqlParameter("@valid",SqlDbType.Int)
                };
                sqlParams[0].Value = id;
                sqlParams[1].Value = boo ? 1 : 0;
                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病历的更新标识
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-20</date>
        /// <param name="id">病历ID</param>
        /// <param name="type">0：开始更新标识；1：结束更新标识</param>
        /// <return></return>
        public static void UpdateRecordUpdateflag(int id, int type)
        {
            try
            {
                string sqlStr = " update recorddetail set ";
                List<OracleParameter> paramList = new List<OracleParameter>();
                OracleParameter param1 = new OracleParameter("@id", OracleType.Int32);
                param1.Value = id;
                paramList.Add(param1);
                if (type == 0)
                {
                    sqlStr += " startupdateflag=@startupdateflag ";
                    OracleParameter param2 = new OracleParameter("@startupdateflag", OracleType.Char);
                    param2.Value = Guid.NewGuid().ToString();
                    paramList.Add(param2);
                }
                else
                {
                    sqlStr += " endupdateflag=@endupdateflag ";
                    OracleParameter param2 = new OracleParameter("@endupdateflag", OracleType.Char);
                    param2.Value = Guid.NewGuid().ToString();
                    paramList.Add(param2);
                }
                sqlStr += " where id=@id ";

                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.ExecuteNonQuery(sqlStr, paramList, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病历的有效状态
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-15</date>
        /// <param name="ids">病历IDs</param>
        /// <param name="noofinpat">首页序号</param>
        /// <return>受影响记录数</return>
        public static int DeleteRecordsByIDs(int noofinpat, string ids)
        {
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    return 0;
                }
                ids = ids.Replace("'", "");
                string sqlStr = " update recorddetail set valid=0 where valid != 0 and noofinpat = @noofinpat and id in(" + ids + ") ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@noofinpat",SqlDbType.Int)
                };
                sqlParams[0].Value = noofinpat;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病历的有效状态
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-11</date>
        /// <param name="datetime">病历时间</param>
        /// <param name="deptCode">科室编码</param>
        /// <param name="wardCode">病区编码</param>
        /// <param name="boo">是否有效</param>
        /// <return></return>
        public static void UpdateRecordValidByTime(string datetime, string deptCode, string wardCode, bool boo)
        {
            try
            {
                string sqlStr = " update recorddetail set valid=@valid where departcode=@deptCode and wardcode=@wardCode and substr(nvl(captiondatetime,'1990-01-01 00:00:00'),0,19)=@datetime ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@datetime",SqlDbType.Char),
                    new SqlParameter("@deptCode",SqlDbType.Char),
                    new SqlParameter("@wardCode",SqlDbType.Char),
                    new SqlParameter("@valid",SqlDbType.Int)
                };
                sqlParams[0].Value = datetime;
                sqlParams[1].Value = deptCode;
                sqlParams[2].Value = wardCode;
                sqlParams[3].Value = boo ? 1 : 0;
                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将病历状态更新为已归档
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// </summary>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        public static int SetRecordsRebacked(string noofinpat)
        {
            try
            {
                int num = 0;
                if (!string.IsNullOrEmpty(noofinpat))
                {
                    // string sqlStr = " update RECORDDETAIL a set ISLOCK = 4701 where a.valid=1 and a.noofinpat = @noofinpat and exists(select 1 from InPatient b  where a.NoOfInpat = b.NoOfInpat and  b.Status in(1502,1503) and a.ISLOCK in(0,4700,4702,4703) or a.islock is null) ";
                    //add by zjy 2013-6-17
                    string sqlStr = " update INPATIENT  a set ISLOCK = 4701 where a.noofinpat = @noofinpat ";
                    DbParameter[] sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@noofinpat",SqlDbType.Char,32)
                    };
                    sqlParams[0].Value = noofinpat;
                    string sqlStr2 = " update recorddetail  a set ISLOCK = 4701 where a.noofinpat = @noofinpat ";


                    DS_SqlHelper.CreateSqlHelper();
                    DS_SqlHelper.ExecuteNonQuery(sqlStr2, sqlParams, CommandType.Text);
                    num = DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
                }
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static int SetRecordstate(string noofinpat, string state)
        {
            try
            {
                int num = 0;
                if (!string.IsNullOrEmpty(noofinpat))
                {
                    // string sqlStr = " update RECORDDETAIL a set ISLOCK = 4701 where a.valid=1 and a.noofinpat = @noofinpat and exists(select 1 from InPatient b  where a.NoOfInpat = b.NoOfInpat and  b.Status in(1502,1503) and a.ISLOCK in(0,4700,4702,4703) or a.islock is null) ";
                    //add by zjy 2013-6-17
                    string sqlStr = " update INPATIENT  a set ISLOCK = " + state + " where a.noofinpat = @noofinpat ";
                    DbParameter[] sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@noofinpat",SqlDbType.Char,32)
                    };
                    sqlParams[0].Value = noofinpat;
                    string sqlStr2 = " update recorddetail  a set ISLOCK = " + state + " where a.noofinpat = @noofinpat ";


                    DS_SqlHelper.CreateSqlHelper();
                    DS_SqlHelper.ExecuteNonQuery(sqlStr2, sqlParams, CommandType.Text);
                    num = DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
                }
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 将病历状态更新为撤销归档归档
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// </summary>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        public static int SetRecordsCancel(string noofinpat)
        {
            try
            {
                int num = 0;
                if (!string.IsNullOrEmpty(noofinpat))
                {
                    // string sqlStr = " update RECORDDETAIL a set ISLOCK = 4701 where a.valid=1 and a.noofinpat = @noofinpat and exists(select 1 from InPatient b  where a.NoOfInpat = b.NoOfInpat and  b.Status in(1502,1503) and a.ISLOCK in(0,4700,4702,4703) or a.islock is null) ";
                    //add by zjy 2013-6-17
                    string sqlStr = " update INPATIENT  a set ISLOCK = 4702 where a.noofinpat = @noofinpat ";
                    DbParameter[] sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@noofinpat",SqlDbType.Char,32)
                    };
                    sqlParams[0].Value = noofinpat;
                    string sqlStr2 = " update recorddetail  a set ISLOCK = 4702 where a.noofinpat = @noofinpat ";
                    DS_SqlHelper.CreateSqlHelper();
                    DS_SqlHelper.ExecuteNonQuery(sqlStr2, sqlParams, CommandType.Text);
                    num = DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
                }
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 将病历状态更新为科室质控
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// </summary>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        public static int SetRecordsCancelCommit(string noofinpat)
        {
            try
            {
                int num = 0;
                if (!string.IsNullOrEmpty(noofinpat))
                {
                    // string sqlStr = " update RECORDDETAIL a set ISLOCK = 4701 where a.valid=1 and a.noofinpat = @noofinpat and exists(select 1 from InPatient b  where a.NoOfInpat = b.NoOfInpat and  b.Status in(1502,1503) and a.ISLOCK in(0,4700,4702,4703) or a.islock is null) ";
                    //add by zjy 2013-6-17
                    string sqlStr = " update INPATIENT  a set ISLOCK = 4705 where a.noofinpat = @noofinpat ";
                    DbParameter[] sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@noofinpat",SqlDbType.Char,32)
                    };
                    sqlParams[0].Value = noofinpat;
                    string sqlStr2 = " update recorddetail  a set ISLOCK = 4705 where a.noofinpat = @noofinpat ";
                    DS_SqlHelper.CreateSqlHelper();
                    DS_SqlHelper.ExecuteNonQuery(sqlStr2, sqlParams, CommandType.Text);
                    num = DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
                }
                return num;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 插入病历历史记录
        /// </summary>
        /// 注：1、所传参数名为对应数据库中字段名
        ///     2、返回值为插入病历历史记录ID
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-07</date>
        /// <param name="Params">参数</param>
        public static void InsertHistoryRecord(List<OracleParameter> Params)
        {
            try
            {
                List<OracleParameter> newParas = new List<OracleParameter>();
                string insertSQL = "insert into operationrecorddetail(id ";
                string valueSql = " values(seq_operationrecorddetail_id.nextval";

                if (Params.Any(p => p.ParameterName == "noofinpat"))
                {
                    insertSQL += ",noofinpat";
                    valueSql += ",@noofinpat";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "noofinpat"));
                }
                if (Params.Any(p => p.ParameterName == "templateid"))
                {
                    insertSQL += ",templateid";
                    valueSql += ",@templateid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "templateid"));
                }
                if (Params.Any(p => p.ParameterName == "name"))
                {
                    insertSQL += ",name";
                    valueSql += ",@name";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "name"));
                }
                if (Params.Any(p => p.ParameterName == "content"))
                {
                    insertSQL += ",content";
                    valueSql += ",@content";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "content"));
                }
                if (Params.Any(p => p.ParameterName == "sortid"))
                {
                    insertSQL += ",sortid";
                    valueSql += ",@sortid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "sortid"));
                }
                if (Params.Any(p => p.ParameterName == "owner"))
                {
                    insertSQL += ",owner";
                    valueSql += ",@owner";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "owner"));
                }
                if (Params.Any(p => p.ParameterName == "auditor"))
                {
                    insertSQL += ",auditor";
                    valueSql += ",@auditor";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "auditor"));
                }
                if (Params.Any(p => p.ParameterName == "createtime"))
                {
                    insertSQL += ",createtime";
                    valueSql += ",@createtime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createtime"));
                }
                if (Params.Any(p => p.ParameterName == "audittime"))
                {
                    insertSQL += ",audittime";
                    valueSql += ",@audittime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "audittime"));
                }
                if (Params.Any(p => p.ParameterName == "valid"))
                {
                    insertSQL += ",valid";
                    valueSql += ",@valid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "valid"));
                }
                if (Params.Any(p => p.ParameterName == "hassubmit"))
                {
                    insertSQL += ",hassubmit";
                    valueSql += ",@hassubmit";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hassubmit"));
                }
                if (Params.Any(p => p.ParameterName == "hasprint"))
                {
                    insertSQL += ",hasprint";
                    valueSql += ",@hasprint";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hasprint"));
                }
                if (Params.Any(p => p.ParameterName == "captiondatetime"))
                {
                    insertSQL += ",captiondatetime";
                    valueSql += ",@captiondatetime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "captiondatetime"));
                }
                if (Params.Any(p => p.ParameterName == "islock"))
                {
                    insertSQL += ",islock";
                    valueSql += ",@islock";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "islock"));
                }
                if (Params.Any(p => p.ParameterName == "firstdailyflag"))
                {
                    insertSQL += ",firstdailyflag";
                    valueSql += ",@firstdailyflag";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "firstdailyflag"));
                }
                if (Params.Any(p => p.ParameterName == "isyihuangoutong"))
                {
                    insertSQL += ",isyihuangoutong";
                    valueSql += ",@isyihuangoutong";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isyihuangoutong"));
                }
                if (Params.Any(p => p.ParameterName == "ip"))
                {
                    insertSQL += ",ip";
                    valueSql += ",@ip";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "ip"));
                }
                if (Params.Any(p => p.ParameterName == "departcode"))
                {
                    insertSQL += ",departcode";
                    valueSql += ",@departcode";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "departcode"));
                }
                if (Params.Any(p => p.ParameterName == "wardcode"))
                {
                    insertSQL += ",wardcode";
                    valueSql += ",@wardcode";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "wardcode"));
                }
                if (Params.Any(p => p.ParameterName == "opertype"))
                {
                    insertSQL += ",opertype";
                    valueSql += ",@opertype";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "opertype"));
                }
                if (Params.Any(p => p.ParameterName == "optime"))
                {
                    insertSQL += ",optime";
                    valueSql += ",@optime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "optime"));
                }
                insertSQL += ")";
                valueSql += ")";
                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.ExecuteNonQuery(insertSQL + valueSql, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 历史病历相关
        /// <summary>
        /// 获取该病人历史记录(不包含无病历的入院记录)
        /// </summary>
        /// <param name="noofinpat">病案号</param>
        /// <param name="type">历史记录类型：0-病历(医生)；1-护理(护士)；其它：无权限</param>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <returns></returns>
        public static DataTable GetHistoryInpatients(int noofinpat, int type, DateTime? starttime, DateTime? endtime)
        {
            try
            {
                string sqlStr = " SELECT inp.noofinpat, inp.name, inp.admitdate, dia.name diagname FROM inpatient inp LEFT OUTER JOIN diagnosis dia ON inp.admitdiagnosis = dia.icd where inp.noofinpat in (select distinct i.noofinpat from inpatient i inner JOIN recorddetail r on i.noofinpat=r.noofinpat and i.noofclinic IN(SELECT noofclinic FROM inpatient WHERE noofinpat = @noofinpat AND noofclinic IS NOT NULL) ";
                if (type == 0)
                {
                    sqlStr += " and r.sortid not in('AI','AJ','AK') ";
                }
                else if (type == 1)
                {
                    sqlStr += " and r.sortid in('AI','AJ','AK') ";
                }
                else
                {
                    sqlStr += " and 1<>1 ";
                }
                sqlStr += " ) and inp.status in(1502,1503) and inp.noofinpat <> @noofinpat ";
                List<OracleParameter> paramList = new List<OracleParameter>();
                OracleParameter param1 = new OracleParameter("@noofinpat", OracleType.Int32);
                param1.Value = noofinpat;
                paramList.Add(param1);
                if (null != starttime)
                {
                    sqlStr += " and inp.admitdate >= @starttime ";
                    OracleParameter param2 = new OracleParameter("@starttime", OracleType.VarChar);
                    param2.Value = ((DateTime)starttime).ToString("yyyy-MM-dd HH:mm:ss");
                    paramList.Add(param2);
                }
                if (null != endtime)
                {
                    sqlStr += " and inp.admitdate <= @endtime ";
                    OracleParameter param3 = new OracleParameter("@endtime", OracleType.VarChar);
                    param3.Value = ((DateTime)endtime).ToString("yyyy-MM-dd HH:mm:ss");
                    paramList.Add(param3);
                }
                sqlStr += " ORDER BY inp.admitdate ";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, paramList, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取该病人的病历数
        /// </summary>
        /// <param name="noofinpat">病案号</param>
        /// <param name="type">历史记录类型：0-病历(医生)；1-护理(护士)；其它：无权限</param>
        /// <returns></returns>
        public static int GetRecordCounts(int noofinpat, int type)
        {
            try
            {
                string sqlStr = " select count(1) from recorddetail where noofinpat = @noofinpat and valid = '1' ";
                if (type == 0)
                {
                    sqlStr += " and sortid not in('AI','AJ','AK') ";
                }
                else if (type == 1)
                {
                    sqlStr += " and sortid in('AI','AJ','AK') ";
                }
                else
                {
                    sqlStr += " and 1<>1 ";
                }
                OracleParameter[] sqlParams = new OracleParameter[]
                {
                    new OracleParameter("@noofinpat",OracleType.Int32)
                };
                sqlParams[0].Value = noofinpat;

                DS_SqlHelper.CreateSqlHelper();
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);

                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #endregion

        #region 关于会诊
        /// <summary>
        /// 获取会诊记录
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public static DataTable GetConsultRecrod(int noofinpat)
        {
            try
            {
                string sqlStr = @"select '会诊记录' || ' ' || applytime || ' ' || (select name from users where users.id = applyuser) cname, consultapplysn 
                                        from consultapply where noofinpat=@noofinpat and stateid in('6730','6740','6741') ";
                OracleParameter[] sqlParams = new OracleParameter[]
                {
                    new OracleParameter("@noofinpat",OracleType.Int32)
                };
                sqlParams[0].Value = noofinpat;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 关于诊疗时间轴
        public static DataTable GetPatCureRecrod(int noofinpat)
        {
            try
            {
                string sqlStr = string.Format(@"select '患者' || name ||'诊疗时间轴' as  patname ,noofinpat   from inpatient where 
noofinpat='{0}' ", noofinpat);
                //OracleParameter[] sqlParams = new OracleParameter[]
                //{
                //    new OracleParameter("@noofinpat",OracleType.Int32)
                //};
                //sqlParams[0].Value = noofinpat;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 关于模板
        /// <summary>
        /// 获取小模板分类
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-28</date>
        /// <param name="deptID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable GetPersonTempleteFloder(string deptID, string userID)
        {
            try
            {
                string sqlStr = @"select * from emrtemplet_item_person_catalog where valid = 1 and (deptid=@deptid and isperson='0') or createusers=@userid order by NAME ";
                OracleParameter[] paras = new OracleParameter[]
                {
                    new OracleParameter("@deptid",OracleType.VarChar),
                    new OracleParameter("@userid",OracleType.VarChar)
                };
                paras[0].Value = deptID;
                paras[1].Value = userID;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, paras, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取小模板
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-28</date>
        /// <param name="deptID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable GetPersonTemplete(string deptID, string userID)
        {
            try
            {
                string sqlStr = @"select * from emrtemplet_item_person where (deptid=@deptid and isperson=0) or createusers=@userid order by NAME ";
                OracleParameter[] paras = new OracleParameter[]
                {
                    new OracleParameter("@deptid",OracleType.VarChar),
                    new OracleParameter("@userid",OracleType.VarChar)
                };
                paras[0].Value = deptID;
                paras[1].Value = userID;

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, paras, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 关于诊断/病种/病种组合

        #region 关于诊断/出院情况
        /// <summary>
        /// 根据诊断表ID获取出院情况
        /// </summary>
        /// <param name="iem_mainpage_diagnosis_no"></param>
        /// <returns></returns>
        public static DataTable GetOutHosConditonByID(int iem_mainpage_diagnosis_no)
        {
            try
            {
                if (iem_mainpage_diagnosis_no < 0)
                {
                    return new DataTable();
                }
                string sqlStr = " select id,iem_mainpage_diagnosis_no,iem_mainpage_no,diagnosis_code,diagnosis_name,status_id,status_name from outdiagnosiscondition where valid=1 and iem_mainpage_diagnosis_no=@id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.Int)
                };
                sqlParams[0].Value = iem_mainpage_diagnosis_no;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 插入出院情况
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-25</date>
        /// <param name="diagnosis_no"></param>
        /// <param name="mainpage_no"></param>
        /// <param name="diagnosis_code"></param>
        /// <param name="diagnosis_name"></param>
        /// <param name="status_id">出院情况ID</param>
        /// <param name="status_name">出院情况</param>
        /// <param name="create_user"></param>
        /// <param name="create_time"></param>
        public static void InsertOutHosCondition(int diagnosis_no, int mainpage_no, string diagnosis_code, string diagnosis_name, int status_id, string status_name, string create_user, string create_time)
        {
            try
            {
                string sqlStr = "insert into outdiagnosiscondition(id,iem_mainpage_diagnosis_no,iem_mainpage_no,diagnosis_code,diagnosis_name,status_id,status_name,valid,create_user,create_time) values(seq_outdiagnosiscondition_id.nextval,@diagnosis_no,@mainpage_no,@diagnosis_code,@diagnosis_name,@status_id,@status_name,1,@create_user,@create_time)";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@diagnosis_no",SqlDbType.Int),
                    new SqlParameter("@mainpage_no",SqlDbType.Int),
                    new SqlParameter("@diagnosis_code",SqlDbType.Char),
                    new SqlParameter("@diagnosis_name",SqlDbType.Char),
                    new SqlParameter("@status_id",SqlDbType.Int),
                    new SqlParameter("@status_name",SqlDbType.Char),
                    new SqlParameter("@create_user",SqlDbType.Char),
                    new SqlParameter("@create_time",SqlDbType.Char)
                };
                sqlParams[0].Value = diagnosis_no;
                sqlParams[1].Value = mainpage_no;
                sqlParams[2].Value = diagnosis_code;
                sqlParams[3].Value = diagnosis_name;
                sqlParams[4].Value = status_id < 0 ? 0 : status_id;
                sqlParams[5].Value = status_name;
                sqlParams[6].Value = create_user;
                sqlParams[7].Value = create_time;
                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据首页ID逻辑删除诊断出院情况
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-25</date>
        /// <param name="iem_mainpage_no">首页ID</param>
        public static void DeleteOutHosConditionByPageNo(int iem_mainpage_no)
        {
            try
            {
                string sqlStr = " update outdiagnosiscondition set valid = 0,cancel_user = @userID,cancel_time = @theTime where valid=1 and iem_mainpage_no = @id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.Int),
                    new SqlParameter("@userID",SqlDbType.Char),
                    new SqlParameter("@theTime",SqlDbType.Char)
                };
                sqlParams[0].Value = iem_mainpage_no;
                sqlParams[1].Value = DS_Common.currentUser.Id;
                sqlParams[2].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取所有关于婴儿的诊断
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-29</date>
        /// <returns></returns>
        public static DataTable GetBabyDiagInfo()
        {
            try
            {
                string sqlStr = string.Format("select * from zx_diagsaboutbaby where valid=1 ");
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 关于病种
        /// <summary>
        /// 获取病种
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// <returns></returns>
        public static DataTable GetDiagnosis()
        {
            try
            {
                string sqlStr = " select markid,icd,name,py,wb,memo from diagnosis where valid = 1 union select icdid markid,icdid,name,py,wb,'' memo from diagnosisothername where valid = 1  order by name";
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据ICD编码串获取病种
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="ICDs">病种ICD串 ---> 'icd1','icd2','icd3'... </param>
        /// <returns></returns>
        public static DataTable GetDiseasesByICDs(string ICDs)
        {
            try
            {
                string sqlStr = " select * from (select markid,icd,name,py,wb,memo from diagnosis where valid = 1 union select icdid markid,icdid,name,py,wb,'' memo from diagnosisothername where valid = 1  ) a where  icd in (" + ICDs + ") order by name ";
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 关于病种组合
        /// <summary>
        /// 获取所有病种组合
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <returns></returns>
        public static DataTable GetDiseaseGroups()
        {
            try
            {
                string sqlStr = " select * from diseasesgroup where valid=1 order by name ";
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据ID获取病种组合
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// <param name="id">病种组合ID</param>
        /// <returns></returns>
        public static DataTable GetDiseaseGroupByID(int id)
        {
            try
            {
                string sqlStr = " select * from diseasesgroup where valid=1 and id=@id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.Int)
                };
                sqlParams[0].Value = id;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据IDs获取病种组合(多个)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="IDs">病种ID串 ---> id1,id2,id3 ... </param>
        /// <returns></returns>
        public static DataTable GetDiseaseGroupsByIDs(string IDs)
        {
            try
            {
                if (string.IsNullOrEmpty(IDs))
                {
                    return new DataTable();
                }
                string sqlStr = " select * from DiseasesGroup where valid=1 and id in (" + IDs + ") order by name ";
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 插入病种组合
        /// </summary>
        /// 注：所传参数名为对应数据库中字段名
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-04</date>
        /// <param name="Params">参数</param>
        public static int InsertDiseaseGroup(List<DbParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0)
                {
                    return 0;
                }
                List<DbParameter> newParas = new List<DbParameter>();
                string insertSQL = "insert into diseasesgroup(id ";
                string valueSql = " values(seq_diseasesgroup_id.nextval";

                if (Params.Any(p => p.ParameterName == "@name"))
                {
                    insertSQL += ",name";
                    valueSql += ",@name";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@name"));
                }
                if (Params.Any(p => p.ParameterName == "@py"))
                {
                    insertSQL += ",py";
                    valueSql += ",@py";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@py"));
                }
                if (Params.Any(p => p.ParameterName == "@wb"))
                {
                    insertSQL += ",wb";
                    valueSql += ",@wb";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@wb"));
                }
                if (Params.Any(p => p.ParameterName == "@diseaseids"))
                {
                    insertSQL += ",diseaseids";
                    valueSql += ",@diseaseids";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@diseaseids"));
                }
                if (Params.Any(p => p.ParameterName == "@deptid"))
                {
                    insertSQL += ",deptid";
                    valueSql += ",@deptid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@deptid"));
                }
                if (Params.Any(p => p.ParameterName == "@valid"))
                {
                    insertSQL += ",valid";
                    valueSql += ",@valid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@valid"));
                }
                if (Params.Any(p => p.ParameterName == "@create_user"))
                {
                    insertSQL += ",create_user";
                    valueSql += ",@create_user";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@create_user"));
                }
                if (Params.Any(p => p.ParameterName == "@create_time"))
                {
                    insertSQL += ",create_time";
                    valueSql += ",@create_time";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@create_time"));
                }
                if (Params.Any(p => p.ParameterName == "@updateuser"))
                {
                    insertSQL += ",updateuser";
                    valueSql += ",@updateuser";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@updateuser"));
                }
                if (Params.Any(p => p.ParameterName == "@updatetime"))
                {
                    insertSQL += ",updatetime";
                    valueSql += ",@updatetime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@updatetime"));
                }
                if (Params.Any(p => p.ParameterName == "@memo"))
                {
                    insertSQL += ",memo";
                    valueSql += ",@memo";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@memo"));
                }
                if (Params.Any(p => p.ParameterName == "@memospare"))
                {
                    insertSQL += ",memospare";
                    valueSql += ",@memospare";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@memospare"));
                }
                insertSQL += ")";
                valueSql += ")";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(insertSQL + valueSql, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新病种组合
        /// </summary>
        /// 注：所传参数名为对应数据库中字段名
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="Params">参数</param>
        public static int UpdateDiseaseGroup(List<DbParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0 || !Params.Any(p => p.ParameterName == "@id"))
                {
                    return 0;
                }
                List<DbParameter> newParas = new List<DbParameter>();
                string updateSQL = "update diseasesgroup set ";

                if (Params.Any(p => p.ParameterName == "@name"))
                {
                    updateSQL += " name = @name,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@name"));
                }
                if (Params.Any(p => p.ParameterName == "@py"))
                {
                    updateSQL += " py = @py,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@py"));
                }
                if (Params.Any(p => p.ParameterName == "@wb"))
                {
                    updateSQL += " wb = @wb,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@wb"));
                }
                if (Params.Any(p => p.ParameterName == "@diseaseids"))
                {
                    updateSQL += " diseaseids = @diseaseids,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@diseaseids"));
                }
                if (Params.Any(p => p.ParameterName == "@deptid"))
                {
                    updateSQL += " deptid = @deptid,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@deptid"));
                }
                if (Params.Any(p => p.ParameterName == "@create_user"))
                {
                    updateSQL += " create_user = @create_user,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@create_user"));
                }
                if (Params.Any(p => p.ParameterName == "@create_time"))
                {
                    updateSQL += " create_time = @create_time,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@create_time"));
                }
                if (Params.Any(p => p.ParameterName == "@updateuser"))
                {
                    updateSQL += " updateuser = @updateuser,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@updateuser"));
                }
                if (Params.Any(p => p.ParameterName == "@updatetime"))
                {
                    updateSQL += " updatetime = @updatetime,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@updatetime"));
                }
                if (Params.Any(p => p.ParameterName == "@memo"))
                {
                    updateSQL += " memo = @memo,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@memo"));
                }
                if (Params.Any(p => p.ParameterName == "@memospare"))
                {
                    updateSQL += " memospare = @memospare,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@memospare"));
                }
                if (Params.Any(p => p.ParameterName == "@valid"))
                {
                    updateSQL += " valid = @valid ";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@valid"));
                }
                else
                {
                    updateSQL += " valid = 1 ";
                }
                updateSQL += " where id = @id ";
                newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@id"));

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(updateSQL, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除病种组合
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="groupID">组合ID</param>
        public static int DeleteDiseaseGroup(int groupID)
        {
            try
            {
                string sqlStr = " update diseasesgroup set valid = 0,updateuser=@updateuser,updatetime=@updatetime where valid=1 and id=@id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.Int),
                    new SqlParameter("@updateuser",SqlDbType.Char),
                    new SqlParameter("@updatetime",SqlDbType.Char)
                };
                sqlParams[0].Value = groupID;
                sqlParams[1].Value = DS_Common.currentUser.Id;
                sqlParams[2].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据用户ID获取组合信息
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="userID">用户ID</param>
        public static DataTable GetUserMatchDiseaseGroup(string userID)
        {
            try
            {
                string sqlStr = " select * from UserMatchDiseasesGroup where valid=1 and userid=@id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.Char)
                };
                sqlParams[0].Value = userID;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 插入用户匹配病种组合记录
        /// </summary>
        /// 注：所传参数名为对应数据库中字段名
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="Params">参数</param>
        public static int InsertUserMatchDiseaseGroup(List<DbParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0)
                {
                    return 0;
                }
                List<DbParameter> newParas = new List<DbParameter>();
                string insertSQL = "insert into UserMatchDiseasesGroup(id ";
                string valueSql = " values(seq_UserMatchDiseasesGroup_id.nextval";

                if (Params.Any(p => p.ParameterName == "@userid"))
                {
                    insertSQL += ",userid";
                    valueSql += ",@userid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@userid"));
                }
                if (Params.Any(p => p.ParameterName == "@username"))
                {
                    insertSQL += ",username";
                    valueSql += ",@username";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@username"));
                }
                if (Params.Any(p => p.ParameterName == "@groupids"))
                {
                    insertSQL += ",groupids";
                    valueSql += ",@groupids";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@groupids"));
                }
                if (Params.Any(p => p.ParameterName == "@valid"))
                {
                    insertSQL += ",valid";
                    valueSql += ",@valid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@valid"));
                }
                if (Params.Any(p => p.ParameterName == "@create_user"))
                {
                    insertSQL += ",create_user";
                    valueSql += ",@create_user";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@create_user"));
                }
                if (Params.Any(p => p.ParameterName == "@create_time"))
                {
                    insertSQL += ",create_time";
                    valueSql += ",@create_time";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@create_time"));
                }
                if (Params.Any(p => p.ParameterName == "@updateuser"))
                {
                    insertSQL += ",updateuser";
                    valueSql += ",@updateuser";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@updateuser"));
                }
                if (Params.Any(p => p.ParameterName == "@updatetime"))
                {
                    insertSQL += ",updatetime";
                    valueSql += ",@updatetime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@updatetime"));
                }
                if (Params.Any(p => p.ParameterName == "@memo"))
                {
                    insertSQL += ",memo";
                    valueSql += ",@memo";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@memo"));
                }
                insertSQL += ")";
                valueSql += ")";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(insertSQL + valueSql, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新用户匹配病种组合记录
        /// </summary>
        /// 注：所传参数名为对应数据库中字段名
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-06</date>
        /// <param name="Params">参数</param>
        public static int UpdateUserMatchDiseaseGroup(List<DbParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0 || !Params.Any(p => p.ParameterName == "@id"))
                {
                    return 0;
                }
                List<DbParameter> newParas = new List<DbParameter>();
                string updateSQL = "update UserMatchDiseasesGroup set ";

                if (Params.Any(p => p.ParameterName == "@userid"))
                {
                    updateSQL += " userid = @userid,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@userid"));
                }
                if (Params.Any(p => p.ParameterName == "@username"))
                {
                    updateSQL += " username = @username,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@username"));
                }
                if (Params.Any(p => p.ParameterName == "@groupids"))
                {
                    updateSQL += " groupids = @groupids,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@groupids"));
                }
                if (Params.Any(p => p.ParameterName == "@create_user"))
                {
                    updateSQL += " create_user = @create_user,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@create_user"));
                }
                if (Params.Any(p => p.ParameterName == "@create_time"))
                {
                    updateSQL += " create_time = @create_time,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@create_time"));
                }
                if (Params.Any(p => p.ParameterName == "@updateuser"))
                {
                    updateSQL += " updateuser = @updateuser,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@updateuser"));
                }
                if (Params.Any(p => p.ParameterName == "@updatetime"))
                {
                    updateSQL += " updatetime = @updatetime,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@updatetime"));
                }
                if (Params.Any(p => p.ParameterName == "@memo"))
                {
                    updateSQL += " memo = @memo,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@memo"));
                }
                if (Params.Any(p => p.ParameterName == "@valid"))
                {
                    updateSQL += " valid = @valid ";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@valid"));
                }
                else
                {
                    updateSQL += " valid = 1 ";
                }
                updateSQL += " where id = @id ";
                newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "@id"));

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(updateSQL, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 关于附卡---乙肝
        /// <summary>
        /// 根据报告卡ID获取附卡记录 - 乙肝
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-28</date>
        /// <param name="report_id">报告卡ID</param>
        /// <returns></returns>
        public static DataTable GetHepatitisBByReportID(int report_id, string diagICD)
        {
            try
            {
                string sqlStr = " select * from attachedcard_hepatitisb where valid=1 and report_id=@report_id and diagicd10=@diagICD order by createtime desc";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@report_id",SqlDbType.Int),
                    new SqlParameter("@diagICD",SqlDbType.VarChar)
                };
                sqlParams[0].Value = report_id;
                sqlParams[1].Value = diagICD;
                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 插入附卡记录 - 乙肝
        /// </summary>
        /// 注：所传参数名为对应数据库中字段名
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-12</date>
        /// <param name="Params">参数</param>
        public static int InsertCardHepatitisB(List<OracleParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0)
                {
                    return 0;
                }
                List<OracleParameter> newParas = new List<OracleParameter>();
                string insertSQL = "insert into attachedcard_hepatitisb(id ";
                string valueSql = " values(seq_attachedcard_hepatitisb_id.nextval";

                if (Params.Any(p => p.ParameterName == "report_id"))
                {
                    insertSQL += ",report_id";
                    valueSql += ",@report_id";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "report_id"));
                }
                if (Params.Any(p => p.ParameterName == "diagicd10"))
                {
                    insertSQL += ",diagicd10";
                    valueSql += ",@diagicd10";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "diagicd10"));
                }
                if (Params.Any(p => p.ParameterName == "diagname"))
                {
                    insertSQL += ",diagname";
                    valueSql += ",@diagname";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "diagname"));
                }
                if (Params.Any(p => p.ParameterName == "hbsagtimetype"))
                {
                    insertSQL += ",hbsagtimetype";
                    valueSql += ",@hbsagtimetype";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hbsagtimetype"));
                }
                if (Params.Any(p => p.ParameterName == "firstattackyear"))
                {
                    insertSQL += ",firstattackyear";
                    valueSql += ",@firstattackyear";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "firstattackyear"));
                }
                if (Params.Any(p => p.ParameterName == "firstattackmonth"))
                {
                    insertSQL += ",firstattackmonth";
                    valueSql += ",@firstattackmonth";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "firstattackmonth"));
                }
                if (Params.Any(p => p.ParameterName == "issymptomatic"))
                {
                    insertSQL += ",issymptomatic";
                    valueSql += ",@issymptomatic";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "issymptomatic"));
                }
                if (Params.Any(p => p.ParameterName == "currentalt"))
                {
                    insertSQL += ",currentalt";
                    valueSql += ",@currentalt";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "currentalt"));
                }
                if (Params.Any(p => p.ParameterName == "hbcigmresult"))
                {
                    insertSQL += ",hbcigmresult";
                    valueSql += ",@hbcigmresult";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hbcigmresult"));
                }
                if (Params.Any(p => p.ParameterName == "heparresult"))
                {
                    insertSQL += ",heparresult";
                    valueSql += ",@heparresult";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "heparresult"));
                }
                if (Params.Any(p => p.ParameterName == "hbsagandhbschange"))
                {
                    insertSQL += ",hbsagandhbschange";
                    valueSql += ",@hbsagandhbschange";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hbsagandhbschange"));
                }
                if (Params.Any(p => p.ParameterName == "valid"))
                {
                    insertSQL += ",valid";
                    valueSql += ",@valid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "valid"));
                }
                if (Params.Any(p => p.ParameterName == "createuser"))
                {
                    insertSQL += ",createuser";
                    valueSql += ",@createuser";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createuser"));
                }
                if (Params.Any(p => p.ParameterName == "createtime"))
                {
                    insertSQL += ",createtime";
                    valueSql += ",@createtime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createtime"));
                }
                if (Params.Any(p => p.ParameterName == "updateuser"))
                {
                    insertSQL += ",updateuser";
                    valueSql += ",@updateuser";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "updateuser"));
                }
                if (Params.Any(p => p.ParameterName == "updatetime"))
                {
                    insertSQL += ",updatetime";
                    valueSql += ",@updatetime";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "updatetime"));
                }
                if (Params.Any(p => p.ParameterName == "memo1"))
                {
                    insertSQL += ",memo1";
                    valueSql += ",@memo1";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "memo1"));
                }
                if (Params.Any(p => p.ParameterName == "memo2"))
                {
                    insertSQL += ",memo2";
                    valueSql += ",@memo2";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "memo2"));
                }
                if (Params.Any(p => p.ParameterName == "memo3"))
                {
                    insertSQL += ",memo3";
                    valueSql += ",@memo3";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "memo3"));
                }
                insertSQL += ")";
                valueSql += ")";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(insertSQL + valueSql, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 更新附卡记录 - 乙肝
        /// </summary>
        /// 注：所传参数名为对应数据库中字段名
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-12</date>
        /// <param name="Params">参数</param>
        public static int UpdateCardHepatitisB(List<OracleParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0 || !Params.Any(p => p.ParameterName == "id"))
                {
                    return 0;
                }
                List<OracleParameter> newParas = new List<OracleParameter>();
                string updateSQL = "update attachedcard_hepatitisb set ";

                if (Params.Any(p => p.ParameterName == "report_id"))
                {
                    updateSQL += " report_id = @report_id,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "report_id"));
                }
                if (Params.Any(p => p.ParameterName == "diagicd10"))
                {
                    updateSQL += " diagicd10 = @diagicd10,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "diagicd10"));
                }
                if (Params.Any(p => p.ParameterName == "diagname"))
                {
                    updateSQL += " diagname = @diagname,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "diagname"));
                }
                if (Params.Any(p => p.ParameterName == "hbsagtimetype"))
                {
                    updateSQL += " hbsagtimetype = @hbsagtimetype,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hbsagtimetype"));
                }
                if (Params.Any(p => p.ParameterName == "firstattackyear"))
                {
                    updateSQL += " firstattackyear = @firstattackyear,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "firstattackyear"));
                }
                if (Params.Any(p => p.ParameterName == "firstattackmonth"))
                {
                    updateSQL += " firstattackmonth = @firstattackmonth,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "firstattackmonth"));
                }
                if (Params.Any(p => p.ParameterName == "issymptomatic"))
                {
                    updateSQL += " issymptomatic = @issymptomatic,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "issymptomatic"));
                }
                if (Params.Any(p => p.ParameterName == "currentalt"))
                {
                    updateSQL += " currentalt = @currentalt,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "currentalt"));
                }
                if (Params.Any(p => p.ParameterName == "hbcigmresult"))
                {
                    updateSQL += " hbcigmresult = @hbcigmresult,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hbcigmresult"));
                }
                if (Params.Any(p => p.ParameterName == "heparresult"))
                {
                    updateSQL += " heparresult = @heparresult,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "heparresult"));
                }
                if (Params.Any(p => p.ParameterName == "hbsagandhbschange"))
                {
                    updateSQL += " hbsagandhbschange = @hbsagandhbschange,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "hbsagandhbschange"));
                }
                if (Params.Any(p => p.ParameterName == "valid"))
                {
                    updateSQL += " valid = @valid,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "valid"));
                }
                if (Params.Any(p => p.ParameterName == "createuser"))
                {
                    updateSQL += " createuser = @createuser,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createuser"));
                }
                if (Params.Any(p => p.ParameterName == "createtime"))
                {
                    updateSQL += " createtime = @createtime,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createtime"));
                }
                if (Params.Any(p => p.ParameterName == "updateuser"))
                {
                    updateSQL += " updateuser = @updateuser,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "updateuser"));
                }
                if (Params.Any(p => p.ParameterName == "updatetime"))
                {
                    updateSQL += " updatetime = @updatetime,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "updatetime"));
                }
                if (Params.Any(p => p.ParameterName == "memo1"))
                {
                    updateSQL += " memo1 = @memo1,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "memo1"));
                }
                if (Params.Any(p => p.ParameterName == "memo2"))
                {
                    updateSQL += " memo2 = @memo2,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "memo2"));
                }
                if (Params.Any(p => p.ParameterName == "memo3"))
                {
                    updateSQL += " memo3 = @memo3,";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "memo3"));
                }
                if (updateSQL.EndsWith(","))
                {
                    updateSQL = updateSQL.Substring(0, updateSQL.Length - 1);
                }

                updateSQL += " where id = @id ";
                newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "id"));

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(updateSQL, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 获取所有手术信息
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-22</date>
        /// <returns></returns>
        public static DataTable GetAllOperations()
        {
            try
            {
                string sqlStr = " select c.name sslbname, (case when a.valid = 1 then '是' else '否' end) validName, a.* from operation a left join CategoryDetail c on c.ID = a.sslb and c.CategoryID = 8 order by valid desc,a.name asc ";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 关于页眉页脚
        /// <summary>
        /// 根据ID删除页眉
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-26</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteTempletHeader(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return false;
                }
                string sqlStr = " delete EMRTEMPLETHEADER where HEADER_ID = @id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.VarChar)
                };
                sqlParams[0].Value = id;

                DS_SqlHelper.CreateSqlHelper();
                int num = DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
                return num == 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据ID删除页脚
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-26</date>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteTempletFooter(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return false;
                }
                string sqlStr = " delete emrtemplet_foot where Foot_ID = @id ";
                DbParameter[] sqlParams = new DbParameter[]
                {
                    new SqlParameter("@id",SqlDbType.VarChar)
                };
                sqlParams[0].Value = id;

                DS_SqlHelper.CreateSqlHelper();
                int num = DS_SqlHelper.ExecuteNonQuery(sqlStr, sqlParams, CommandType.Text);
                return num == 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 关于模板
        /// <summary>
        /// 插入模板
        /// </summary>
        /// 注：所传参数名为对应数据库中字段名
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-04-22</date>
        /// <param name="Params">参数</param>
        /// <returns>受影响的记录数</returns>
        public static int InsertTemplete(List<OracleParameter> Params)
        {
            try
            {
                if (null == Params || Params.Count == 0 || !Params.Any(p => p.ParameterName == "code"))
                {
                    return 0;
                }

                List<OracleParameter> newParas = new List<OracleParameter>();
                string insertSQL = "insert into emrtemplet_item_person(code ";
                string valueSql = " values(@code";
                newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "code"));

                if (Params.Any(p => p.ParameterName == "name"))
                {
                    insertSQL += ",name";
                    valueSql += ",@name";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "name"));
                }
                if (Params.Any(p => p.ParameterName == "item_content"))
                {
                    insertSQL += ",item_content";
                    valueSql += ",@item_content";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "item_content"));
                }
                if (Params.Any(p => p.ParameterName == "deptshare"))
                {
                    insertSQL += ",deptshare";
                    valueSql += ",@deptshare";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "deptshare"));
                }
                if (Params.Any(p => p.ParameterName == "parentid"))
                {
                    insertSQL += ",parentid";
                    valueSql += ",@parentid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "parentid"));
                }
                if (Params.Any(p => p.ParameterName == "container"))
                {
                    insertSQL += ",container";
                    valueSql += ",@container";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "container"));
                }
                if (Params.Any(p => p.ParameterName == "deptid"))
                {
                    insertSQL += ",deptid";
                    valueSql += ",@deptid";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "deptid"));
                }
                if (Params.Any(p => p.ParameterName == "isperson"))
                {
                    insertSQL += ",isperson";
                    valueSql += ",@isperson";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "isperson"));
                }
                if (Params.Any(p => p.ParameterName == "createusers"))
                {
                    insertSQL += ",createusers";
                    valueSql += ",@createusers";
                    newParas.Add(Params.FirstOrDefault(p => p.ParameterName == "createusers"));
                }

                insertSQL += ")";
                valueSql += ")";

                DS_SqlHelper.CreateSqlHelper();
                return DS_SqlHelper.ExecuteNonQuery(insertSQL + valueSql, newParas, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
