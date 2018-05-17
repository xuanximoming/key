using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// 任务属性
   /// </summary>
   [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
   public class JobAttribute : Attribute
   {
      #region properties
      /// <summary>
      /// 任务名称
      /// </summary>
      public string Name
      {
         get { return _name; }
      }
      private string _name;

      /// <summary>
      /// 任务描述
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }
      private string _description;

      /// <summary>
      /// 启动类型的类型
      /// </summary>
      public Type StartupType
      {
         get { return _startuType; }
      }
      private Type _startuType;

      /// <summary>
      /// 显示在NAVBAR上的图标
      /// </summary>
      public string IconName
      {
         get { return _iconName; }
         set { _iconName = value; }
      }
      private string _iconName;

      /// <summary>
      /// 该任务所属的系统
      /// </summary>
      public string SystemName
      {
         get { return _systemName; }
      }
      private string _systemName;

      /// <summary>
      /// 任务在管理器里是否可见
      /// </summary>
      public bool Visible
      {
         get { return _visible; }
         set { _visible = value; }
      }
      private bool _visible;

      /// <summary>
      /// 默认是否启用
      /// </summary>
      public bool Enabled
      {
         get { return _enabled; }
      }
      private bool _enabled;

      /// <summary>
      /// 日志路径
      /// </summary>
      public string LogDirectory
      {
         get { return _logDirectory; }
         set { _logDirectory = value; }
      }
      private string _logDirectory;
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      /// <param name="missionName">任务名称</param>
      /// <param name="startupType">启动类类型</param>
      /// <param name="iconName">图标名称</param>
      /// <param name="systemName">所属系统名称</param>
      /// <param name="interval"></param>
      public JobAttribute(string jobName, string description, string systemName, bool enable, Type startupType)
      {
         _name = jobName;
         _description = description;
         _systemName = systemName;
         _enabled = enable;
         _startuType = startupType;
         _visible = true;
      }
      #endregion
   }

   /// <summary>
   /// 任务属性读取器
   /// </summary>
   public class MissionAttributeReader
   {
      private string m_AssemblyName;
      private Assembly _assemblyMission;
      private Assembly AssemblyMission
      {
         get
         {
            if (_assemblyMission == null)
            {
               try
               {
                  _assemblyMission = Assembly.LoadFrom(m_AssemblyName);
               }
               catch (Exception ex)
               {
                  throw new Exception("加载Assembly：" + m_AssemblyName + "错误！错误信息：" + ex.Message);
               }
            }
            return _assemblyMission;
         }
         set { _assemblyMission = value; }
      }

      /// <summary>
      /// ctor1
      /// </summary>
      /// <param name="assemblyName"></param>
      public MissionAttributeReader(string assemblyName)
      {
         m_AssemblyName = assemblyName;
      }

      /// <summary>
      /// ctor2
      /// </summary>
      /// <param name="domain"></param>
      /// <param name="assemblyName"></param>
      public MissionAttributeReader(AppDomain domain, string assemblyName)
      {
         try
         {
            m_AssemblyName = Path.GetFileNameWithoutExtension(assemblyName);
            if (domain != null)
               AssemblyMission = domain.Load(m_AssemblyName);
            else
               AssemblyMission = AppDomain.CurrentDomain.Load(m_AssemblyName);
         }
         catch (Exception ex)
         {
            throw new Exception("加载Assembly：" + m_AssemblyName + "错误！错误信息：" + ex.Message);
         }
      }

      /// <summary>
      /// ctor3
      /// </summary>
      /// <param name="assemblyMission"></param>
      public MissionAttributeReader(Assembly assemblyMission)
      {
         AssemblyMission = assemblyMission;
      }

      /// <summary>
      /// 取得任务的属性信息
      /// </summary>
      /// <returns></returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
      public JobAttribute[] GetJobAttributes()
      {
         if (AssemblyMission != null)
         {
            try
            {
               return (JobAttribute[])AssemblyMission.GetCustomAttributes(typeof(JobAttribute), false);
            }
            catch (Exception ex)
            {
               throw ex;
            }
         }
         else
         {
            return new JobAttribute[] { };
         }
      }
   }
}
