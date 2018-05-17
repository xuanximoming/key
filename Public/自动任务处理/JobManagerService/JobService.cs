using System;
using System.Diagnostics;
using System.ServiceProcess;
using DrectSoft.JobManager;

namespace JobManagerService
{
    partial class JobService : ServiceBase
    {
        //private System.Timers.Timer m_TimerGuard; //by Ukey 2016-08-19
        private EventLog _jobEventLog;
        private EventLog JobEventLog
        {
            get
            {
                if (_jobEventLog == null)
                {
                    _jobEventLog = new EventLog("Application");
                    _jobEventLog.Source = ServiceName;
                }
                return _jobEventLog;
            }
        }

        private const string DataServiceName = "JobService";

        public JobService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            JobEventLog.WriteEntry(DataServiceName + " start at " + DateTime.Now.ToString());
            try
            {
                JobTaskManager jobManager = new JobTaskManager();
                jobManager.RegisterMissions();
            }
            catch (Exception ex)
            {
                JobEventLog.WriteEntry(DataServiceName + " 出现错误 at " + DateTime.Now.ToString() + ex.Message);
            }
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。

            JobEventLog.WriteEntry(DataServiceName + " stoped at " + DateTime.Now.ToString());
        }


    }
}
