using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Plugin
{
   /// <summary>
    /// 核心插件特性，负责核心业务
   /// </summary>
   [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
   public class CorePluginAttribute : PluginAttribute
   {
       /// <summary>
       /// ctor
       /// </summary>
       /// <param name="name"></param>
      public CorePluginAttribute(string name):base(name)
      { 

      }

      /// <summary>
      /// 插件类别
      /// </summary>
      public override PluginType PluginType
      {
         get { return PluginType.Biz; }
      }
   }
}
