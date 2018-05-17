using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// 任务动作基类
   /// </summary>
   public abstract class BaseJobAction : IJobAction
   {
      #region properties
      /// <summary>
      /// 
      /// </summary>
      public Job Parent
      {
         get { return _parent; }
         set { _parent = value; }
      }
      private Job _parent;

      /// <summary>
      /// 任务状态
      /// </summary>
      public SynchState SynchState
      {
         get { return _synchState; }
         protected set { _synchState = value; }
      }
      private SynchState _synchState = SynchState.Stop;

      /// <summary>
      /// 有自己的配置参数,默认无
      /// </summary>
      public virtual bool HasPrivateSettings { get { return false; } }

      /// <summary>
      /// 有初始化动作,默认无
      /// </summary>
      public virtual bool HasInitializeAction { get { return false; } }
      #endregion

      public BaseJobAction()
      { }

      #region public IJobAction 成员
      /// <summary>
      /// 执行初始化动作
      /// </summary>
      public virtual void ExecuteDataInitialize()
      {
      }

      /// <summary>
      /// 执行
      /// </summary>
      public abstract void Execute();

      /// <summary>
      /// 停止
      /// </summary>
      public virtual void Stop()
      {
      }

      /// <summary>
      /// 暂停
      /// </summary>
      public virtual void Suspend()
      {
      }

      /// <summary>
      /// 继续任务的执行
      /// </summary>
      public virtual void Resume()
      {
      }

      /// <summary>
      /// 刷新任务自己的参数设置
      /// </summary>
      public virtual void RefreshPrivateSettings() { }
      #endregion
   }
}
