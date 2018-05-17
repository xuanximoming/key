using DrectSoft.Emr.QCTimeLimit;
using DrectSoft.JobManager;
using System;

[assembly: Job("时限质量数据更新", "定期更新病历书写时限情况", "电子病历", true, typeof(TimeQcManager))]
namespace DrectSoft.JobManager
{
    /// <summary>
    /// 时限质量数据更新
    /// </summary>
    public class TimeQcManager : BaseJobAction
    {
        #region ctor
        public TimeQcManager()
        {
        }
        #endregion

        #region public IJobAction 成员

        public override void Execute()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                QCTimeLimitInnerService service = new QCTimeLimitInnerService();
                service.MainProcess();
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
        #endregion
    }
}
