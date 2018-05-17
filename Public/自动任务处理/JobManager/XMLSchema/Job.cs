using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// 任务定义
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public class Job
   {
      #region properties
      /// <summary>
      /// 任务名称
      /// </summary>
      [XmlAttributeAttribute()]
      public string Name
      {
         get { return nameField; }
         set { nameField = value; }
      }
      private string nameField;

      /// <summary>
      /// 任务描述
      /// </summary>
      [XmlAttributeAttribute()]
      public string Description
      {
         get { return descriptionField; }
         set { descriptionField = value; }
      }
      private string descriptionField;

      /// <summary>
      /// 任务是否启用
      /// </summary>
      [XmlAttributeAttribute()]
      public bool Enable
      {
         get { return enableField; }
         set { enableField = value; }
      }
      private bool enableField;

      /// <summary>
      /// 在UI中是否可见
      /// </summary>
      [XmlAttributeAttribute()]
      public bool Visible
      {
         get { return visibleField; }
         set { visibleField = value; }
      }
      private bool visibleField;

      /// <summary>
      /// 任务的启动类全名
      /// </summary>
      [XmlAttributeAttribute()]
      public string Class
      {
         get { return classField; }
         set { classField = value; }
      }
      private string classField;

      /// <summary>
      /// 任务的DLL全名
      /// </summary>
      [XmlAttributeAttribute()]
      public string Library
      {
         get { return libraryField; }
         set { libraryField = value; }
      }
      private string libraryField;

      /// <summary>
      /// 图标名称
      /// </summary>
      [XmlAttributeAttribute()]
      public string IconName
      {
         get { return iconNameField; }
         set { iconNameField = value; }
      }
      private string iconNameField;

      /// <summary>
      /// 日志目录路径
      /// </summary>
      [XmlAttributeAttribute()]
      public string LogDirectory
      {
         get { return logDirectoryField; }
         set { logDirectoryField = value; }
      }
      private string logDirectoryField;

      /// <summary>
      /// 任务调用计划配置
      /// </summary>
      public JobPlan JobSchedule
      {
         get { return jobScheduleField; }
         set { jobScheduleField = value; }
      }
      private JobPlan jobScheduleField;

      /// <summary>
      /// 任务的核心处理逻辑
      /// </summary>
      [XmlIgnore()]
      public IJobAction Action
      {
         get { return _action; }
         set { _action = value; }
      }
      private IJobAction _action;

      #endregion
   }
}
