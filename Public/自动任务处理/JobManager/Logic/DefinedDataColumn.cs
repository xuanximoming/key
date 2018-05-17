using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// 预定义数据列名
   /// </summary>
   public static class DefinedDataColumn
   {
      /// <summary>
      /// 日期
      /// </summary>
       public const string LogDate = "LOGTIME";
      /// <summary>
      /// 日志类型
      /// </summary>
      public const string LogType = "日志类型";
      /// <summary>
      /// 日志源
      /// </summary>
      public const string LogSource = "JOBNAME";
      /// <summary>
      /// 源
      /// </summary>
      public const string Source = "源";
      /// <summary>
      /// 消息
      /// </summary>
      public const string Message = "CONTENT";
      /// <summary>
      /// 类别
      /// </summary>
      public const string Type = "类别";
      /// <summary>
      /// 事件
      /// </summary>
      public const string Event = "事件";
      /// <summary>
      /// 用户
      /// </summary>
      public const string User = "用户";
      /// <summary>
      /// 计算机
      /// </summary>
      public const string Computer = "计算机";

      public const string Count = "记录条数";
      public const string ChangedCount = "改变条数";
      public const string StartTime = "开始时间";
      public const string EndTime = "结束时间";
      public const string Success = "是否成功";
   }
}
