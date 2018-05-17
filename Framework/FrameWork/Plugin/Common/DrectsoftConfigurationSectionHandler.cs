using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace DrectSoft.FrameWork
{
   /// <summary>
   /// 委托：读取配置文件
   /// </summary>
   /// <param name="xmlNode"></param>
   /// <returns></returns>
   public delegate object DelegateReadConfiguration(System.Xml.XmlNode xmlNode);

   /// <summary>
   /// DrectSoftConfigurationSectionHandler
   /// </summary>
   public class DrectSoftConfigurationSectionHandler : IConfigurationSectionHandler
   {      
      private static DelegateReadConfiguration delegateFunction;

      /// <summary>
      /// 设置委托函数
      /// </summary>
      public static void SetConfigurationDelegate(DelegateReadConfiguration function)
      {
         delegateFunction = function;
      }

      /// <summary>
      /// 实现IConfigurationSectionHandler.Create
      /// </summary>
      /// <param name="parent"></param>
      /// <param name="configContext"></param>
      /// <param name="section"></param>
      /// <returns></returns>
      public object Create(object parent, object configContext, System.Xml.XmlNode section)
      {
         if (delegateFunction != null)
         {
            return delegateFunction(section);
         }
         else
         {
            return null;
         }
      }
   }
}
