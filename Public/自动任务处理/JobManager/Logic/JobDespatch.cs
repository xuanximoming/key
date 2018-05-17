using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Timers;

namespace DrectSoft.JobManager
{
    /// <summary>
    /// 任务调度中心
    /// </summary>
    internal class JobDespatch
    {
        #region fields
        private JobTaskManager m_MissionManager;
        private Collection<Job> m_JobQueue;
        private Object m_LockJobQueue = new Object();
        private Timer m_Timer;
        #endregion

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public JobDespatch(JobTaskManager manager)
        {
            m_MissionManager = manager;
            m_JobQueue = new Collection<Job>();
            m_Timer = new Timer(10000);
            //到达时间的时候执行事件； 
            m_Timer.AutoReset = true;
            m_Timer.Enabled = true;
            m_Timer.Elapsed += new ElapsedEventHandler(m_Timer_Elapsed);
        }


        #endregion

        #region public methods
        /// <summary>
        /// 开始轮询
        /// </summary>
        public void Start()
        {
            m_Timer.Start();
        }

        /// <summary>
        /// 停止任务检查
        /// </summary>
        public void Stop()
        {
            m_Timer.Stop();
        }
        #endregion

        #region private methods

        void m_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PollingJob();
        }

        //private void _timerMain_Tick(object sender, EventArgs e)
        //{
        //   if (MissionCollection != null)
        //   {
        //      foreach (ISynchMission synchMission in MissionCollection)
        //      {
        //         //达到进入队列条件，做移动入执行队列的操作,假如队列中已存在该任务，如果队列长度不超过限定，则允许重复插入，否则暂停任务插入队列
        //         SynchMissionConfigration config = m_MissionManager.GetConfig(synchMission.MissionName);
        //         if (config != null && config.Enable)
        //         {//如果启用了配置才移入计划队列
        //            if (m_Comparer.Warning(config.TimeSetting, DateTime.Now))
        //            {
        //               if (MissionQueue.Count <= MaxQueueLength)
        //                  MissionQueue.Add(synchMission as IJobAction);
        //            }
        //         }
        //      }

        //      foreach (ISynchMission mission in MissionQueue)
        //      {
        //         SynchMissionConfigration synchConfig = m_MissionManager.GetConfig(mission.MissionName);
        //         if (m_Comparer.Compare(synchConfig.TimeSetting, DateTime.Now))
        //         {//对队列中的任务做当前点检查，符合则开线程执行，否则继续等待，线程执行完从队列中删除该任务
        //            BackgroundWorker worker = new BackgroundWorker();
        //            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        //            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        //            bool found = false;
        //            foreach (IJobAction action in m_ActionCollection)
        //            {
        //               ISynchMission miss = action as ISynchMission;
        //               if (mission.MissionName == miss.MissionName)
        //               {
        //                  found = true;
        //                  break;
        //               }
        //            }
        //            if (!found)
        //            {
        //               m_ActionCollection.Add(mission as IJobAction);
        //               worker.RunWorkerAsync(mission);
        //            }
        //         }
        //      }
        //   }
        //}

        /// <summary>
        /// 轮询是否有需要执行的任务
        /// </summary>
        private void PollingJob()
        {
            m_Timer.Stop();
            lock (m_LockJobQueue)
            {
                // 将当前时间点需要执行的任务添加到队列
                foreach (Job job in m_MissionManager.AllJobs)
                {
                    if (job.Enable && (job.Action != null) && (job.Action.SynchState != SynchState.Busy)
                       && job.JobSchedule.NeedRunNow)
                        if (!m_JobQueue.Contains(job))
                            m_JobQueue.Add(job);
                }
            }

            foreach (Job job in m_JobQueue)
                DoJobThread(job);
            m_JobQueue.Clear();
            m_Timer.Start();
        }

        private void DoJobThread(Job job)
        {

            lock (job)
            {
                if ((job.Action != null) && (job.Action.SynchState == SynchState.Stop))
                {
                    try
                    {
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "开始", TraceLevel.Info));
                        job.Action.Execute();
                        job.JobSchedule.LastExecuteTime = DateTime.Now;
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "结束", TraceLevel.Info));
                        
                    }
                    catch (Exception exception)
                    {
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, string.Empty, exception));
                    }
                }

            }
            //// 创建线程
            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += new DoWorkEventHandler(JobThread_DoWork);
            //worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(JobThread_RunWorkerCompleted);

            //// 运行（结束后会从队列里移除任务）
            //worker.RunWorkerAsync(job);
        }

        private void JobThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Job job = e.Argument as Job;
            e.Result = e.Argument;
            lock (job)
            {
                if ((job.Action != null) && (job.Action.SynchState == SynchState.Stop))
                {
                    try
                    {
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "开始", TraceLevel.Info));
                        job.Action.Execute();
                    }
                    catch (Exception err)
                    {
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, String.Empty, err));
                    }
                }
            }
        }

        private void JobThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Job job = e.Result as Job;
            //lock (job)
            {
                if (job != null)
                {
                    if ((job.Action != null) && (job.Action.SynchState == SynchState.Stop))
                    {
                        job.JobSchedule.LastExecuteTime = DateTime.Now;
                        JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "结束", TraceLevel.Info));
                    }
                    lock (m_LockJobQueue)
                    {
                        if (m_JobQueue.Contains(job))
                            m_JobQueue.Remove(job);
                    }
                }
            }
        }
        #endregion
    }
}
