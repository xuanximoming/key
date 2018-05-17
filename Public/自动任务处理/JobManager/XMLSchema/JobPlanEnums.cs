using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// 任务计划的类型
   /// </summary>
   [SerializableAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public enum JobPlanType
   {
      /// <summary>
      /// 重复执行
      /// </summary>
      Repeat = 1,
      /// <summary>
      /// 仅执行一次
      /// </summary>
      JustOnce = 2
   }

   /// <summary>
   /// 任务定期执行的时间间隔单位
   /// </summary>
   [SerializableAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public enum JobExecIntervalUnit
   {
      /// <summary>
      /// 未设置
      /// </summary>
      None = 0,
      /// <summary>
      /// 分钟
      /// </summary>
      Minute = 1,
      /// <summary>
      /// 小时
      /// </summary>
      Hour = 2,
      /// <summary>
      /// 天
      /// </summary>
      Day = 3,
      /// <summary>
      /// 周
      /// </summary>
      Week = 4
   }
}
