using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System;
using System.Collections.ObjectModel;

namespace DrectSoft.JobManager
{
   /// <remarks/>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   [XmlRootAttribute(Namespace = "http://www.DrectSoft.com.cn/", IsNullable = false)]
   public class JobConfig
   {
      private Collection<SystemsJobDefine> jobsOfSystemField;

      /// <summary>
      /// 各子系统所属的任务
      /// </summary>
      [XmlElementAttribute("System")]
      public Collection<SystemsJobDefine> JobsOfSystem
      {
         get { return jobsOfSystemField; }
         set { jobsOfSystemField = value; }
      }
   }

   /// <summary>
   /// 子系统任务信息
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public class SystemsJobDefine
   {
      private Collection<Job> jobsField;
      private string nameField;

      /// <summary>
      /// 任务分类名称
      /// </summary>
      [XmlAttributeAttribute()]
      public string Name
      {
         get { return nameField; }
         set { nameField = value; }
      }

      /// <summary>
      /// 包含的任务
      /// </summary>
      [XmlElementAttribute("Job")]
      public Collection<Job> Jobs
      {
         get { return jobsField; }
         set { jobsField = value; }
      }
   }
}