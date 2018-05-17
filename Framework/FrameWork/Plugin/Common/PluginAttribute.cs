using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Plugin
{
   /// <summary>
   /// 插件信息特性
   /// </summary>
   [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
   public abstract class PluginAttribute : Attribute
   {
      string m_name;

      /// <summary>
      /// 构造函数
      /// </summary>
      public PluginAttribute() { }

      /// <summary>
      /// 构造函数
      /// </summary>
      /// <param name="name">插件名称</param>
      public PluginAttribute(string name)
      {
         m_name = name;
      }

      /// <summary>
      /// 获取或设置插件名称
      /// </summary>
      public string Name
      {
         get { return m_name; }
         set { m_name = value; }
      }

      /// <summary>
      /// 获取插件名称
      /// </summary>
      public abstract PluginType PluginType { get;}
   }
}