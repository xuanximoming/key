using DrectSoft.Core;
using DrectSoft.JobManager;
using DrectSoft.JobTasks;
using System;
using System.Data;
using System.Diagnostics;
using System.Xml;


[assembly: Job("病历解析", "定期获取病历文档解析", "电子病历", true, typeof(ChangeDoc))]
namespace DrectSoft.JobTasks
{
    class ChangeDoc : BaseJobAction
    {
        private IDataAccess m_EmrHelper;
        private static XmlDocument xmlDoc = new XmlDocument();
        public ChangeDoc()
        {
            m_EmrHelper = DataAccessFactory.DefaultDataAccess;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
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

        /// <summary>
        /// 初始化数据
        /// </summary>
        public override void ExecuteDataInitialize()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                m_EmrHelper = DataAccessFactory.DefaultDataAccess;
                DataTable data = m_EmrHelper.ExecuteDataTable("select * from RECORDDETAIL t", CommandType.Text);
                int rowAnalysis = 0;
                foreach (DataRow row in data.Rows)
                {
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(row["content"].ToString());

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
