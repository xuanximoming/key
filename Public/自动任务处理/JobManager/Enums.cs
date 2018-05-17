using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.JobManager
{


   /// <summary>
   /// 同步任务的状态
   /// </summary>
   public enum SynchState
   {
      /// <summary>
      /// 开始
      /// </summary>
      Start = 1,
      /// <summary>
      /// 忙
      /// </summary>
      Busy = 2,
      /// <summary>
      /// 停止
      /// </summary>
      Stop = 3,
      /// <summary>
      /// 挂起
      /// </summary>
      Suspend = 4
   }
}
