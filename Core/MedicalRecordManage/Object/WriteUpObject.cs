using System;
using System.Data;
using System.Data.SqlClient;

namespace MedicalRecordManage.Object
{
    class WriteUpObject
    {
        public string m_sApply = null;
        public string m_sName = null;
        public string m_sNoOfInpat = null;
        public string m_sDepartmentName = null;
        public string m_sApplyContent = null;
        public int m_iApplyTimes = 0;
        public string m_sApplyDocId = null;
        public string m_sApproveDocId = null;
        public string m_sApproveContent = null;
        public string m_sApproveDate = null;
        public string m_sYanqiflag = null;
        public int m_iStatus = 0;
        public string m_sPatid = null;//add by zjy 2013-6-17  住院号
        public void Clear()
        {
            m_sApply = null;
            m_sName = null;
            m_sNoOfInpat = null;
            m_sDepartmentName = null;
            m_sApplyContent = null;
            m_iApplyTimes = 0;
            m_sApplyDocId = null;
            m_sApproveDocId = null;
            m_sApproveContent = null;
            m_sApproveDate = null;
            m_sYanqiflag = null;
            m_iStatus = 0;
            m_sPatid = null;
        }
        public WriteUpObject()
        {

        }
        public WriteUpObject(string id)
        {
            try
            {
                this.m_sApply = id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //是否已经存在病历补写申请记录
        /// <summary>
        /// Modify by xlb 2013-05-30
        /// </summary>
        /// <param name="applyObject"></param>
        /// <returns></returns>
        public static int IsExistApply(WriteUpObject applyObject)
        {
            try
            {
                string sql = @"select * from EMR_RECORDWRITEUP where noofinpat=@nOofinpat 
                                                               and status<2 and applydocid=@applydocId";
                SqlParameter[] sps =
                {
                new SqlParameter("@nOofinpat",applyObject==null|applyObject.m_sNoOfInpat==null?"":applyObject.m_sNoOfInpat),
                new SqlParameter("@applydocId",applyObject==null|applyObject.m_sApplyDocId==null?"":applyObject.m_sApplyDocId)
                };
                DataSet data = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataSet(sql, sps, CommandType.Text);
                if (data.Tables[0].Rows.Count > 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //判断是否允许延期,不超过延期限制次数，查询审核通过的管理记录数
        public static int IsCanDelay(WriteUpObject applyObject, int times)
        {
            try
            {
                //获取被延期记录是否也是延期记录，是获取记录的申请ID
                string delayid = WriteUpObject.GetApplyDelayId(applyObject.m_sApply);
                if (delayid == null || delayid.Equals(""))
                {
                    return 1;
                }
                else
                {
                    string sql = " select id from EMR_RECORDWRITEUP where yanqiflag = '" +
                        delayid + "'" +
                        " and status in(2,5) " +
                        " and applydocid = '" + applyObject.m_sApplyDocId + "'";
                    DataSet data = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataSet(sql);
                    if (data.Tables[0].Rows.Count < times)
                        return 1;
                    else
                        return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //获取需要延期的补写申请剩余的天数.
        public static int GetDelayedApplyTimesResidue(string applyId)
        {
            try
            {
                string sql = " SELECT APPLYTIMES,APPROVEDATE FROM EMR_RECORDWRITEUP WHERE ID = '" +
                    applyId + "'";
                DataSet data = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataSet(sql);
                string val = data.Tables[0].Rows[0]["APPROVEDATE"].ToString();
                int times = int.Parse(data.Tables[0].Rows[0]["APPLYTIMES"].ToString());
                DateTime date = DateTime.Parse(val);
                TimeSpan timeSpan = System.DateTime.Today.Date - date;
                int days = timeSpan.Days;
                days = times - days;
                return days;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //延期操作：创建新的申请记录，并保存延期的原有申请ID
        public static int Delay(WriteUpObject applyObject)
        {
            try
            {
                //将原有补写申请剩余的期限加上
                int i = WriteUpObject.GetDelayedApplyTimesResidue(applyObject.m_sApply);
                applyObject.m_iApplyTimes = applyObject.m_iApplyTimes + i;
                //原有记录存在延期申请，
                string delayid = WriteUpObject.GetApplyDelayId(applyObject.m_sApply);
                if (delayid == null || delayid.Equals(""))
                    delayid = applyObject.m_sApply;
                string sql = @"insert into EMR_RECORDWRITEUP(noofinpat,applydocid,applycontent,applytimes,
                                                            status,yanqiflag) values(@nOofinpat,@applydocid,
                                                            @applycontent,@applytimes,@applystatus,@yanqiflag)";
                SqlParameter[] sps =
                {
                new SqlParameter("@nOofinpat",applyObject==null|applyObject.m_sNoOfInpat==null?"":applyObject.m_sNoOfInpat),
                new SqlParameter("@applydocid",applyObject==null|applyObject.m_sApplyDocId==null?"":applyObject.m_sApplyDocId),
                new SqlParameter("@applycontent",applyObject==null|applyObject.m_sApplyContent==null?"":applyObject.m_sApplyContent),
                new SqlParameter("@applytimes",applyObject==null?0:applyObject.m_iApplyTimes),
                new SqlParameter("@applystatus",applyObject.m_iStatus.ToString()),
                new SqlParameter("@yanqiflag",delayid)
                };
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql, sps, CommandType.Text);
                //原有补写申请状态设置/
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //获取延期补写申请延期所指的原有补写申请的ID
        public static string GetApplyDelayId(string id)
        {
            try
            {
                string sql = " select yanqiflag from EMR_RECORDWRITEUP where id = '" + id + "'";
                return DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteScalar(sql).ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //简单更新状态
        public static void UpdateStatus(string id, int status)
        {
            try
            {
                string sql = " update EMR_RECORDWRITEUP set status = " + status.ToString() +
                    " where id = " + "'" + id + "'";

                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql);
                if (status == 5)
                {
                    string sql2 = @"update inpatient set islock='4707'  where noofinpat in (select noofinpat from EMR_RECORDWRITEUP where id=@emrId)";
                    SqlParameter[] sps2 ={
                                   
                                     new SqlParameter("@emrId",id)
                                    };
                    DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql2, sps2, CommandType.Text);
                    string sql3 = @"update recorddetail set islock='4707'  where noofinpat in (select noofinpat from EMR_RECORDWRITEUP where id=@emrId)";
                    DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql3, sps2, CommandType.Text);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Modify by xlb 2013-05-30
        /// 拼SQL特殊字符引起异常
        /// </summary>
        /// <param name="applyObject"></param>
        public static void Edit(WriteUpObject applyObject)
        {
            try
            {
                //string sql = " update EMR_RECORDWRITEUP set applytimes =" + applyObject.m_iApplyTimes +
                //    ", applycontent = " + "'" + applyObject.m_sApplyContent + "'" +
                //     ", status = " + applyObject.m_iStatus.ToString() +
                //     " where id = " + "'" + applyObject.m_sApply + "'";
                string sql = @"update EMR_RECORDWRITEUP set applytimes=@applyTimes,applycontent=@applyContent,
                               status=@applyStatus where id=@applyId";
                SqlParameter[] sps =
                {
                new SqlParameter("@applyTimes",applyObject==null?0:applyObject.m_iApplyTimes),
                new SqlParameter("@applyContent",applyObject==null|applyObject.m_sApplyContent==null?"":applyObject.m_sApplyContent),
                new SqlParameter("@applyStatus",applyObject==null?0:applyObject.m_iStatus),
                new SqlParameter("@applyId",applyObject==null|applyObject.m_sApply==null?"":applyObject.m_sApply)
                };
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 审核通过操作
        /// Modify by xlb 2013-03-28
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        public static void Approve(string id, string user)
        {
            try
            {
                #region 注销 by xlb 2013-03-29
                //string sql = " update EMR_RECORDWRITEUP set status = 2, " +
                //    " approvedocid = '" + user + "', " +
                //    " approvedate = sysdate " +
                //    " where id = " + "'" + id + "'";
                #endregion
                //string sql = string.Format(@"update EMR_RECORDWRITEUP set status = 2,
                //approvedocid='{0}',approvedate=sysdate where id='{1}'", user, id);
                string sql = @"update EMR_RECORDWRITEUP set status='2',approvedocid=@approvedocid,
                approvedate=sysdate where id=@emrId";
                SqlParameter[] sps ={
                                     new SqlParameter("@approvedocid",user),
                                     new SqlParameter("@emrId",id)
                                    };
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql, sps, CommandType.Text);
                string sql2 = @"update inpatient set islock='4700'  where noofinpat in (select noofinpat from EMR_RECORDWRITEUP where id=@emrId)";
                SqlParameter[] sps2 ={
                                   
                                     new SqlParameter("@emrId",id)
                                    };
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql2, sps2, CommandType.Text);
                string sql3 = @"update recorddetail set islock='4700'  where noofinpat in (select noofinpat from EMR_RECORDWRITEUP where id=@emrId)";
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql3, sps2, CommandType.Text);
                ApproveDealWithDelayRelatedApply(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //审核通过时将原有申请单设置为归还状态
        public static void ApproveDealWithDelayRelatedApply(string apply)
        {
            try
            {
                string delay = GetApplyDelayId(apply);
                if (delay != null)
                {
                    string sql = " update EMR_RECORDWRITEUP set status = 5 " +
                        " where ((yanqiflag = " + "'" + delay + "'" + " and status = 2) " +
                        " or (id = " + "'" + delay + "'" + " and status =2))" +
                        " and id != " + "'" + apply + "'";
                    DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Modify by xlb 2013-03-29
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public static void ApproveRef(string id, string user, string message)
        {
            try
            {
                #region 注销 by xlb 2013-03-29
                //string sql = " update EMR_RECORDWRITEUP set status = 3, " + 
                //    " approvedocid = '" + user + "', " + 
                //    " approvecontent ='" + message + "', " +
                //    " approvedate = sysdate " + 
                //    " where id = " + "'" + id + "'";
                #endregion
                string sql = @"update EMR_RECORDWRITEUP set status='3',approvedocid=@approvedocid,
               approvecontent=@approvecontent,approvedate=sysdate  where id=@emrId";
                SqlParameter[] sps = new SqlParameter[]
                                 {
                                 new SqlParameter("@approvedocid",user),
                                 new SqlParameter("@approvecontent",message),
                                 new SqlParameter("@emrId",id)
                                 };
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql, sps, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //删除操作
        public static void Delete(string id)
        {
            try
            {
                string sql = " delete from EMR_RECORDWRITEUP where id =" + "'" + id + "'";
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //自动归还操作：批准日期 + 申请期限 >= 当前日期,状态为审核通过
        //审核不通过操作
        public static void AutoFeed(string applyDocId)
        {
            try
            {

                string sql2 = @"update inpatient set islock='4707'  where noofinpat in (select noofinpat from EMR_RECORDWRITEUP   where status = 2 " +
                   " and trunc(sysdate,'DD') - trunc(approvedate,'DD') >= applytimes " +
                   " and applydocid = " + "'" + applyDocId + "')";

                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql2, CommandType.Text);
                string sql3 = @"update recorddetail set islock='4707'  where noofinpat in (select noofinpat from EMR_RECORDWRITEUP   where status = 2 " +
                   " and trunc(sysdate,'DD') - trunc(approvedate,'DD') >= applytimes " +
                   " and applydocid = " + "'" + applyDocId + "')";
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql3, CommandType.Text);
                string sql = " update EMR_RECORDWRITEUP set status = 5  " +
                   " where status = 2 " +
                   " and trunc(sysdate,'DD') - trunc(approvedate,'DD') >= applytimes " +
                   " and applydocid = " + "'" + applyDocId + "'";
                DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteNonQuery(sql);

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
