using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace DrectSoft.JobManager
{
    /// <summary>
    /// 任务的动作处理
    /// </summary>
    public interface IJobAction
    {
        /// <summary>
        /// 包含动作的任务
        /// </summary>
        Job Parent { get; set; }
        /// <summary>
        /// 同步任务状态
        /// </summary>
        SynchState SynchState { get; }
        /// <summary>
        /// 有自己的配置参数
        /// </summary>
        bool HasPrivateSettings { get; }
        /// <summary>
        /// 有初始化动作
        /// </summary>
        bool HasInitializeAction { get; }

        /// <summary>
        /// 执行初始化任务
        /// </summary>
        void ExecuteDataInitialize();
        /// <summary>
        /// 执行任务
        /// </summary>
        void Execute();
        /// <summary>
        /// 强制停止任务
        /// </summary>
        void Stop();
        /// <summary>
        /// 挂起任务
        /// </summary>
        void Suspend();
        /// <summary>
        /// 继续任务的执行
        /// </summary>
        void Resume();
        /// <summary>
        /// 刷新任务自己的参数设置
        /// </summary>
        void RefreshPrivateSettings();

    }
}
