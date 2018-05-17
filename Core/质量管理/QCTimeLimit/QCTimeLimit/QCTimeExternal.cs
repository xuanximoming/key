using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DrectSoft.DSSqlHelper;
using System.Data.SqlClient;
using DrectSoft.Emr.QCTimeLimit.QCEntity;

//**********************************
//********* Create by wwj **********
//**********************************

namespace DrectSoft.Emr.QCTimeLimit
{
    /// <summary>
    /// 病历时限的外部触发 【备用】
    /// </summary>
    public class QCTimeExternal
    {
        #region Const

        /// <summary>
        /// 获得病历的信息，且该病历未在QCRecord表中处理过
        /// </summary>
        const string c_SqlGetRecordDetailQCCode =
            @"SELECT r.id, e.qc_code, r.audittime createtime, r.noofinpat
                FROM recorddetail r, emrtemplet e 
               WHERE r.templateid = e.templet_id and r.id = @id 
                  and r.valid = '1' and e.qc_code is not null
                  and not exists(select 1 from qcrecord q where q.recorddetailid = r.id and q.valid = '1')";

        /// <summary>
        /// 获取病历对应的时限记录
        /// </summary>
        const string c_SqlGetQCRecordByNoofinpatQCCode =
            @"SELECT * FROM qcrecord q WHERE q.noofinpat = @noofinpat and q.qccode = @qccode and q.valid = '1' ";
        #endregion

        /// <summary>
        /// .ctor
        /// </summary>
        public QCTimeExternal()
        {
            try
            {
                DS_SqlHelper.CreateSqlHelperByDBNameFormDA("EMRDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 完成病历后，结束该份病历对应的时限记录

        /// <summary>
        /// 完成病历后，结束该份病历对应的时限记录
        /// </summary>
        public void CompleteQCTimeLimit(string recordDetailID)
        {
            try
            {
                Action<string> CompleteQCTimeLimitAction = (id) =>
                    {
                        try
                        {
                            DS_SqlHelper.BeginTransaction();

                            DataTable dt = GetRecordDetailInfo(id);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                CompleteQCTimeLimitInner(dt);
                            }

                            DS_SqlHelper.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            DS_SqlHelper.AppDbTransaction.Rollback();
                            throw ex;
                        }
                    };
                CompleteQCTimeLimitAction.BeginInvoke(recordDetailID, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 结束时限的内部逻辑
        /// </summary>
        /// <param name="recordDetailID"></param>
        void CompleteQCTimeLimitInner(DataTable dtRecordDetail)
        {
            try
            {
                string noofinpat = dtRecordDetail.Rows[0]["noofinpat"].ToString();//首页序号
                string recordDetailID = dtRecordDetail.Rows[0]["id"].ToString();//病历编号
                string qcCode = dtRecordDetail.Rows[0]["qc_code"].ToString();//病历监控代码
                string createTime = dtRecordDetail.Rows[0]["createtime"].ToString();//病历创建时间

                //找到该份病历对应的所有病历时限
                DataTable dtQCRecord = GetQCRecordByNoofinpatQCCode(noofinpat, qcCode);

                //在QCRecord表中找到最合适的一条记录与病历编号RecordDetailID匹配
                DataRow drQCRec = (from DataRow drQCRecord in dtQCRecord.Rows
                                   where drQCRecord["result"].ToString() == "0"
                                 orderby drQCRecord["realconditiontime"].ToString() descending
                                 select drQCRecord).FirstOrDefault<DataRow>();
                if (drQCRec != null)
                {
                    //更新时限记录的已完成
                    string qcRecordID = drQCRec["id"].ToString();
                    DateTime recordDetailCreateTime = Convert.ToDateTime(createTime);
                    DateTime realConditionTime = Convert.ToDateTime(drQCRec["realconditiontime"]);
                    int timeLimit = Convert.ToInt32(drQCRec["timelimit"]);
                    string foulState = "0";//违规情况 默认不违规 0：不违规 1：违规
                    if ((recordDetailCreateTime - realConditionTime).Seconds >= timeLimit)
                    {
                        foulState = "1";
                    }
                    QCRecord.UpdateResultComplete(qcRecordID, recordDetailID, foulState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得指定病历的信息
        /// </summary>
        /// <param name="recordDetailID"></param>
        /// <returns></returns>
        DataTable GetRecordDetailInfo(string recordDetailID)
        {
            try
            {
                SqlParameter[] sps = { new SqlParameter("@id", SqlDbType.NVarChar) };
                sps[0].Value = recordDetailID;
                return DS_SqlHelper.ExecuteDataTable(c_SqlGetRecordDetailQCCode, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取病历对应的时限记录
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="qcCode"></param>
        /// <returns></returns>
        DataTable GetQCRecordByNoofinpatQCCode(string noofinpat, string qcCode)
        {
            SqlParameter[] sps = { 
                                    new SqlParameter("@noofinpat", SqlDbType.NVarChar),
                                    new SqlParameter("@qccode", SqlDbType.NVarChar)
                                 };
            sps[0].Value = noofinpat;
            sps[1].Value = qcCode;
            return DS_SqlHelper.ExecuteDataTable(c_SqlGetQCRecordByNoofinpatQCCode, sps, CommandType.Text);
        }

        #endregion
    }
}
