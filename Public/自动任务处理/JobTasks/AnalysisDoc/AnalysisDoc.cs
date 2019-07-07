using DrectSoft.Core;
using DrectSoft.JobManager;
using DrectSoft.JobTasks;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Xml;

[assembly: Job("病历解析", "定期获取病历文档解析", "电子病历", true, typeof(AnalysisDoc))]
namespace DrectSoft.JobTasks
{
    class AnalysisDoc : BaseJobAction
    {
        private IDataAccess m_EmrHelper;
        private static XmlDocument xmlDoc = new XmlDocument();
        public AnalysisDoc()
        {
            m_EmrHelper = DataAccessFactory.DefaultDataAccess;
        }
        public override void Execute()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                m_EmrHelper = DataAccessFactory.DefaultDataAccess;
                DataTable data = m_EmrHelper.ExecuteDataTable("select * from RECORDDETAIL t where t.createtime >= (select max(t.createtime) from ANALYSISDOC t )", CommandType.Text);
                int rowAnalysis = 0;
                foreach (DataRow row in data.Rows)
                {
                    int flag = 1;
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(row["content"].ToString());
                    XmlNodeList xmlNode = xmlDoc.GetElementsByTagName("selement");
                    SqlParameter[] sqlParams = new SqlParameter[]
                     { 
                        new SqlParameter("@id", SqlDbType.VarChar, 50),
                        new SqlParameter("@recid", SqlDbType.Int, 9),
                        new SqlParameter("@noofinpat", SqlDbType.Int, 9),
                        new SqlParameter("@sortid", SqlDbType.VarChar, 50),
                        new SqlParameter("@elementnames", SqlDbType.VarChar, 50),
                        new SqlParameter("@elementcode", SqlDbType.VarChar, 50),
                        new SqlParameter("@elementvalue", SqlDbType.VarChar, 50),
                        new SqlParameter("@createtime", SqlDbType.VarChar, 50),
                        new SqlParameter("@flag", SqlDbType.Int, 1)
                    };
                    sqlParams[1].Value = Decimal.Parse(row["ID"].ToString());
                    sqlParams[2].Value = int.Parse(row["NOOFINPAT"].ToString());
                    sqlParams[3].Value = row["SORTID"].ToString();
                    sqlParams[7].Value = row["CREATETIME"].ToString();
                    foreach (XmlNode node in xmlNode)
                    {
                        sqlParams[8].Value = flag;
                        sqlParams[0].Value = Guid.NewGuid().ToString();
                        sqlParams[4].Value = node.Attributes["name"].Value;
                        sqlParams[5].Value = node.Attributes["code"].Value;
                        sqlParams[6].Value = node.Attributes["text"].Value;
                        m_EmrHelper.ExecuteDataTable("AnalysisDocPak.usp_updateOrInsertAnalysisDoc", sqlParams, CommandType.StoredProcedure);
                        rowAnalysis++;
                        flag = 0;
                    }
                }
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, "AnalysisDoc", data.Rows.Count, rowAnalysis, DateTime.Now, true, String.Empty, TraceLevel.Info));
            }
            catch (Exception ex)
            {
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(this.Parent, ex.Message));
                throw ex;
            }
            finally
            {
                base.SynchState = SynchState.Stop;
            }
        }
        public override void ExecuteDataInitialize()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                m_EmrHelper = DataAccessFactory.DefaultDataAccess;
                DataTable data = m_EmrHelper.ExecuteDataTable("select * from RECORDDETAIL t where t.createtime = (select max(a.createtime) from RECORDDETAIL a )", CommandType.Text);
                int rowAnalysis = 0;
                foreach (DataRow row in data.Rows)
                {
                    int flag = 1;
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(row["content"].ToString());
                    XmlNodeList xmlNode = xmlDoc.GetElementsByTagName("selement");
                    SqlParameter[] sqlParams = new SqlParameter[]
                     { 
                        new SqlParameter("@id", SqlDbType.VarChar, 50),
                        new SqlParameter("@recid", SqlDbType.Int, 9),
                        new SqlParameter("@noofinpat", SqlDbType.Int, 9),
                        new SqlParameter("@sortid", SqlDbType.VarChar, 50),
                        new SqlParameter("@elementnames", SqlDbType.VarChar, 50),
                        new SqlParameter("@elementcode", SqlDbType.VarChar, 50),
                        new SqlParameter("@elementvalue", SqlDbType.VarChar, 50),
                        new SqlParameter("@createtime", SqlDbType.VarChar, 50),
                        new SqlParameter("@flag", SqlDbType.Int, 1)
                    };
                    sqlParams[1].Value = Decimal.Parse(row["ID"].ToString());
                    sqlParams[2].Value = int.Parse(row["NOOFINPAT"].ToString());
                    sqlParams[3].Value = row["SORTID"].ToString();
                    sqlParams[7].Value = row["CREATETIME"].ToString();
                    foreach (XmlNode node in xmlNode)
                    {
                        sqlParams[8].Value = flag;
                        sqlParams[0].Value = Guid.NewGuid().ToString();
                        sqlParams[4].Value = node.Attributes["name"].Value;
                        sqlParams[5].Value = node.Attributes["code"].Value;
                        sqlParams[6].Value = node.Attributes["text"].Value;
                        m_EmrHelper.ExecuteDataTable("AnalysisDocPak.usp_updateOrInsertAnalysisDoc", sqlParams, CommandType.StoredProcedure);
                        rowAnalysis++;
                        flag = 0;
                    }
                }
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, "AnalysisDoc", data.Rows.Count, rowAnalysis, DateTime.Now, true, String.Empty, TraceLevel.Info));
            }
            catch (Exception ex)
            {
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(this.Parent, ex.Message));
                throw ex;
            }
            finally
            {
                base.SynchState = SynchState.Stop;
            }
        }
    }
}
