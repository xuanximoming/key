using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Plugin
{
   /// <summary>
   /// 业务逻辑插件特性
   /// </summary>
   [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
   public class BizPluginAttribute : PluginAttribute
   {
      BizPluginType type;
      bool isCore;

       /// <summary>
       /// ctor
       /// </summary>
       /// <param name="name"></param>
       /// <param name="type"></param>
      public BizPluginAttribute(string name, BizPluginType type)
      {
         this.type = type;
         this.isCore = false;
      }

      /// <summary>
      /// 构造函数
      /// </summary>
      /// <param name="name">插件名称</param>
      /// <param name="type">业务逻辑插件类型</param>
      /// <param name="iscore">是否是核心插件</param>
      public BizPluginAttribute(string name, BizPluginType type,bool iscore)
         : base(name)
      {
         this.type = type;
         this.isCore = iscore;
      }

      /// <summary>
      /// 获取插件类型
      /// </summary>
      public override PluginType PluginType
      {
         get { return PluginType.Biz; }
      }

      /// <summary>
      /// 获得业务逻辑插件类型
      /// </summary>
      public BizPluginType BizPluginType
      {
         get { return type; }
      }

      /// <summary>
      /// 设置或获取是否是核心插件
      /// </summary>
      public bool IsCore
      {
         get { return isCore; }
         set { isCore = value; }
      }
   }
}
