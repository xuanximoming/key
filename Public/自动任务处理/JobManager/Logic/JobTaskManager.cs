using DrectSoft.Core;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;

namespace DrectSoft.JobManager
{
    /// <summary>
    /// 任务管理器，最外层的逻辑管理器
    /// </summary>
    public class JobTaskManager
    {

        #region properties
        /// <summary>
        /// 配置文件中定义的系统分类
        /// </summary>
        public JobConfig Systems
        {
            get
            {
                if (_systems == null)
                    InitializeConfig();
                return _systems;
            }
        }
        private JobConfig _systems;

        /// <summary>
        /// 所有的任务
        /// </summary>
        public Collection<Job> AllJobs
        {
            get
            {
                if (_allJobs == null)
                    InitializeConfig();
                return _allJobs;
            }
        }
        private Collection<Job> _allJobs;
        #endregion

        #region fields
        private JobDespatch m_MissionDespatch;
        private const string m_JobtaskInfo = "JobTaskConfig";
        #endregion

        #region ctor
        public JobTaskManager()
        {
            // 确保EMR的数据库已经启动，否则创建任务会失败
            //TestSqlServiceHadStarted();

            // 根据任务配置信息创建每个任务的Action实现
            CreateJobAction();
        }
        #endregion

        #region public & internal method

        public void WriteAllLog()
        {
            foreach (Job job in AllJobs)
            {
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "主应用程序关闭", TraceLevel.Info));
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="e"></param>
        public void WriteLog(JobExecuteInfoArgs e)
        {
            JobLogHelper.WriteLog(e);
        }

        /// <summary>
        /// 保存对任务的修改
        /// </summary>
        public void SaveJobConfig()
        {
            if (Systems != null)
            {
                FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\" + "DataSynchConfig.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(typeof(JobConfig));
                serializer.Serialize(file, Systems);
                file.Close();
            }
        }
        #endregion

        #region private methods
        private void InitializeConfig()
        {
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\" + "DataSynchConfig.xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlSerializer serializer = new XmlSerializer(typeof(JobConfig));
            _systems = (JobConfig)serializer.Deserialize(file);
            file.Close();

            _allJobs = new Collection<Job>();
            foreach (SystemsJobDefine system in _systems.JobsOfSystem)
            {
                foreach (Job job in system.Jobs)
                    _allJobs.Add(job);
            }
        }

        public void RegisterMissions()
        {

            m_MissionDespatch = new JobDespatch(this);
            m_MissionDespatch.Start();
        }


        public void StopMissions()
        {
            if (m_MissionDespatch != null)
                m_MissionDespatch.Stop();
        }

        /// <summary>
        /// 测试sql服务是否已经启动
        /// </summary>
        /// <returns></returns>
        private void TestSqlServiceHadStarted()
        {
            int testTimes = 60;
            do
            {
                try
                {
                    IDataAccess sqlHelp = DataAccessFactory.DefaultDataAccess;
                    sqlHelp.ExecuteDataTable("select * from Users where 1=2");
                    return;
                }
                catch
                {
                    testTimes--;
                    Thread.Sleep(30000); // 30秒做一次
                }
            } while (testTimes > 0);

            throw new ApplicationException("无法连接数据库");
        }

        private void CreateJobAction()
        {
            for (int i = AllJobs.Count - 1; i >= 0; i--)
            {
                AllJobs[i].Action = CreateActionInstance(AllJobs[i].Class, AllJobs[i].Library);
                if (AllJobs[i].Action != null)
                    AllJobs[i].Action.Parent = AllJobs[i];
            }
        }

        private IJobAction CreateActionInstance(string className, string assemblyName)
        {
            try
            {
                Assembly assembly = Assembly.Load(Path.GetFileNameWithoutExtension(assemblyName));
                Type actionType = assembly.GetType(className, true, true);

                return Activator.CreateInstance(actionType) as IJobAction;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
