using System;
using System.Data;
using System.Data.SqlClient;

namespace DrectSoft.Core.Permission
{
    class PermissionPublic
    {
        IDataAccess m_DataAccess;
        public PermissionPublic()
        {
            m_DataAccess = DataAccessFactory.GetSqlDataAccess();
        }

        /// <summary>
        /// 获取所有岗位信息
        /// </summary>
        /// <returns>岗位</returns>
        public DataSet GetDepartInfo()
        {
            try
            {
                return m_DataAccess.ExecuteDataSet("usp_SelectAllJobs", CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetDeptOrWard(string power)
        {
            try
            {
                if (power == "402")
                    return m_DataAccess.ExecuteDataSet("usp_SelectWard", CommandType.StoredProcedure);
                return m_DataAccess.ExecuteDataSet("usp_selectdepartment", CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取所有员工信息
        /// </summary>
        /// <returns>员工</returns>
        public DataSet GetUsersInfo()
        {
            try
            {
                return m_DataAccess.ExecuteDataSet("usp_SelectAllUsers2", CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetJob2Permission()
        {
            try
            {
                return m_DataAccess.ExecuteDataSet("usp_SelectPermission", CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 插入新的岗位
        /// </summary>
        /// <param name="id">岗位Id</param>
        /// <param name="title">岗位名称</param>
        public string InsertJob(string id, string title, string memo)
        {
            try
            {
                //string sql_SelectExsit = "Select * from Jobs where Id='" + id + "' ";
                //string sql_Insert = "insert into Jobs(Id,Title,Memo)values('" + id + "','" + title + "','"+memo+"')";
                string returnInfo = string.Empty;
                SqlParameter[] sqlParam1 = new SqlParameter[]
            {
                new SqlParameter("@Id",SqlDbType.VarChar)
            };
                sqlParam1[0].Value = id;
                SqlParameter[] sqlParam2 = new SqlParameter[] 
            {
                new SqlParameter("@Id",SqlDbType.VarChar),
                new SqlParameter("@Title",SqlDbType.VarChar),
                new SqlParameter("@Memo",SqlDbType.VarChar)
            };
                sqlParam2[0].Value = id;
                sqlParam2[1].Value = title;
                sqlParam2[2].Value = memo;
                if (m_DataAccess.ExecuteDataSet("usp_SelectJob", sqlParam1, CommandType.StoredProcedure).Tables[0].Rows.Count > 0)
                {
                    returnInfo = "存在岗位代码【" + id + "】";
                }
                else
                {
                    try
                    {
                        m_DataAccess.ExecuteNoneQuery("usp_InsertJob", sqlParam2, CommandType.StoredProcedure);
                        returnInfo = "岗位信息保存成功";
                    }
                    catch (Exception ex)
                    {
                        returnInfo = ex.Message;
                    }

                }
                return returnInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除选定岗位
        /// </summary>
        /// <param name="id">岗位Id</param>
        public void DelDepart(string id)
        {
            try
            {
                //string StrUpdate = "Update Users Set JobID=Replace(JobID,'" + id + ",','') WHere ID in (Select ID from Users Where JobID Like '%" + id + "%' )";
                //string StrDelPer = " Delete From Job2Permission where ID='" + id + "'";
                //string StrDelJobs = " Delete From Jobs where Id='" + id + "'";
                SqlParameter[] sqlParam = new SqlParameter[]
            {
                new SqlParameter("@ID",SqlDbType.VarChar)
            };
                sqlParam[0].Value = id;
                m_DataAccess.ExecuteNoneQuery("usp_DeleteJob", sqlParam, CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 选中岗位权限
        /// </summary>
        /// <param name="Jobid">岗位Id</param>
        /// <returns></returns>
        public DataSet GetJobPermissionInfo(string jobId)
        {
            //string strsql = "Select * from Job2Permission where ID='" + jobId + "'";
            SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@JobId", SqlDbType.VarChar)
            };
            sqlParam[0].Value = jobId;
            try
            {
                return m_DataAccess.ExecuteDataSet("usp_GetJobPermissionInfo", sqlParam, CommandType.StoredProcedure);
                //return m_DataAccess.ExecuteDataSet(strsql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// 修改岗位信息
        /// </summary>
        /// <param name="Name">岗位名称</param>
        /// <param name="id">岗位Id</param>
        /// <param name="memo">备具信息</param>
        /// <returns></returns>
        public string UpdateJob(string name, string id, string memo)
        {
            try
            {  //string strsql = "Update Jobs set Title='" + name + "',Memo='" + memo + "' where Id='" + id + "'";
                string returnInfo = string.Empty;
                SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@Id", SqlDbType.VarChar),
                new SqlParameter("@Title",SqlDbType.VarChar),
                new SqlParameter("@Memo",SqlDbType.VarChar)
            };
                sqlParam[0].Value = id;
                sqlParam[1].Value = name;
                sqlParam[2].Value = memo;

                m_DataAccess.ExecuteNoneQuery("usp_UpdateJobInfo", sqlParam, CommandType.StoredProcedure);
                returnInfo = "岗位信息更新成功";
                return returnInfo;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            //m_DataAccess.BeginTransaction();
            //SqlParameter[] test = new SqlParameter[] { };
            //m_DataAccess.ExecuteNoneQuery("", test, CommandType.StoredProcedure);
            //m_DataAccess.CommitTransaction();
        }

        /// <summary>
        /// 删除权限控制信息
        /// </summary>
        /// <param name="jobID"></param>
        /// <param name="sqlHelper"></param>
        public void DeleteJobPermission(string jobID, IDataAccess sqlHelper)
        {
            try
            {
                //string strDelsql = "DELETE Job2Permission WHERE ID='" + jobID + "'";
                SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ID", SqlDbType.VarChar)
            };
                sqlParam[0].Value = jobID;
                sqlHelper.ExecuteNoneQuery("usp_DeleteJobPermission", sqlParam, CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 给所选岗位重新赋值
        /// </summary新的权限
        /// <param name="jobid"></param>
        /// <param name="jobName"></param>
        /// <param name="jobCode"></param>
        public void SetPermission(string jobId, string jobName, string jobCode, IDataAccess sqlHelper)
        {
            try
            {
                //string returnInfo;
                //string strSelectsql = "select * from Job2Permission where ID='"+jobid+"'";
                //object returnNum= m_DataAccess.ExecuteScalar(strSelectsql);
                //if (returnNum != null)
                //{
                //    string strDelsql = "DELETE Job2Permission WHERE ID='" + jobid + "'";
                //    m_DataAccess.ExecuteNoneQuery(strDelsql);
                //}
                //string strInsertsql = "INSERT INTO Job2Permission(ID,Moduleid,Modulename) values (" + jobid + ",'" + jobCode + "','" + jobName + "') ";
                SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ID", SqlDbType.VarChar),
                new SqlParameter("@Moduleid",SqlDbType.VarChar),
                new SqlParameter("@Modulename",SqlDbType.VarChar)
            };
                sqlParam[0].Value = jobId;
                sqlParam[1].Value = jobCode;
                sqlParam[2].Value = jobName;
                sqlHelper.ExecuteNoneQuery("usp_InsertJobPermission", sqlParam, CommandType.StoredProcedure);
                //returnInfo = "权限更新成功";
                //return returnInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除某个员工信息
        /// </summary>
        /// <param name="id"></param>
        public void DelUser(string id)
        {
            try
            {
                //string StrDelUser = " DELETE Users WHERE ID ='" + id + "'";
                SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ID", SqlDbType.VarChar)
            };
                sqlParam[0].Value = id;
                m_DataAccess.ExecuteNoneQuery("usp_DeleteUserInfo", sqlParam, CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 增加新员工
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void InsertUser(string id, string name)
        {
            //TODO:
        }
        /// <summary>
        /// 更新员工信息
        /// </summary>
        /// <param name="users">员工信息实体</param>
        /// <returns></returns>
        public string UpdateUser(UsersEntity users)
        {
            try
            {
                string returnInfo = string.Empty;
                SqlParameter[] sqlParam = new SqlParameter[] 
            { 
                new SqlParameter("@ID", SqlDbType.VarChar),
                new SqlParameter("@Name", SqlDbType.VarChar),
                new SqlParameter("@DeptId", SqlDbType.VarChar),
                new SqlParameter("@WardID", SqlDbType.VarChar),
                new SqlParameter("@JobID", SqlDbType.VarChar),
                new SqlParameter("@DeptOrWard",SqlDbType.VarChar),
                new SqlParameter("@Power",SqlDbType.VarChar),

                new SqlParameter("@Py", SqlDbType.VarChar),
                new SqlParameter("@Wb", SqlDbType.VarChar),
                new SqlParameter("@Sexy", SqlDbType.VarChar),
                new SqlParameter("@Birth", SqlDbType.VarChar),
                new SqlParameter("@Marital", SqlDbType.VarChar),

                new SqlParameter("@IDNO", SqlDbType.VarChar),
                new SqlParameter("@Category", SqlDbType.VarChar),
                new SqlParameter("@Jobtitle", SqlDbType.VarChar),
                new SqlParameter("@Recipeid", SqlDbType.VarChar),
                new SqlParameter("@Recipemark", SqlDbType.VarChar),

                new SqlParameter("@Narcosismark", SqlDbType.VarChar),
                new SqlParameter("@Grade", SqlDbType.VarChar),
                new SqlParameter("@Status", SqlDbType.VarChar),
                new SqlParameter("@Memo", SqlDbType.VarChar)
            };
                sqlParam[0].Value = users.Id;
                sqlParam[1].Value = users.Name;
                sqlParam[2].Value = users.Deptid;
                sqlParam[3].Value = users.Wardid;
                sqlParam[4].Value = users.Jobid;
                sqlParam[5].Value = users.DeptOrWard;
                sqlParam[6].Value = users.Powermark;

                string[] code = GetPYWB(users.Name);
                sqlParam[7].Value = code[0];
                sqlParam[8].Value = code[1];
                sqlParam[9].Value = users.Sexy;
                sqlParam[10].Value = users.Birth;
                sqlParam[11].Value = users.Marital;

                sqlParam[12].Value = users.Idno;
                sqlParam[13].Value = users.Category;
                sqlParam[14].Value = users.Jobtitle;
                sqlParam[15].Value = users.Recipeid;
                sqlParam[16].Value = users.Recipemark;

                sqlParam[17].Value = users.Narcosismark;
                sqlParam[18].Value = users.Grade;
                sqlParam[19].Value = users.Status;
                sqlParam[20].Value = users.Memo;
                try
                {
                    m_DataAccess.ExecuteNoneQuery("usp_UpdateUserInfo", sqlParam, CommandType.StoredProcedure);
                    returnInfo = "岗位信息更新成功";
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message);
                }
                return returnInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 得到拼音和五笔
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string[] GetPYWB(string name)
        {
            try
            {
                GenerateShortCode shortCode = new GenerateShortCode(m_DataAccess);
                string[] code = shortCode.GenerateStringShortCode(name);

                //string py = code[0]; //PY
                //string wb = code[1]; //WB
                return code;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
