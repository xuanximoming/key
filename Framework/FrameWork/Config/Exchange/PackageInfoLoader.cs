using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DrectSoft.FrameWork.util;

namespace DrectSoft.FrameWork.Config
{
   /// <summary>
   /// 服务包信息装载
   /// </summary>
   public class PackageInfoLoader
   {
      /// <summary>
      /// 从配置文件装载插件信息
      /// </summary>
      /// <param name="elem">装载的插件元素</param>
      /// <returns></returns>
      public static ServicePackageInfo GetPackageInfo(XmlElement elem)
      {
         ServicePackageInfo ba = new ServicePackageInfo();
         ba.Name = XmlUtil.GetAttributeValue(elem, "Name");
         ba.ImpleAssemblyName = XmlUtil.GetAttributeValue(elem, "ImpleAssembly");
         ba.InfAssemblyName = XmlUtil.GetAttributeValue(elem, "InfAssembly");
         foreach (XmlNode node in elem.ChildNodes)
         {
            if (!(node is XmlElement))
               continue;

            XmlElement serviceelem = node as XmlElement;
            ServiceInfo srvinfo = ParseServiceInfo(serviceelem);
            ba.Services.Add(srvinfo);
         }
         return ba;
      }

      private static ServiceInfo ParseServiceInfo(XmlElement serviceelem)
      {
         ServiceInfo srvinfo = new ServiceInfo();
         srvinfo.Key = XmlUtil.GetAttributeValue(serviceelem, "key");
          //实现服务类型
         srvinfo.ImpleTypeName = XmlUtil.GetAttributeValue(serviceelem, "imple");
          //来自
         srvinfo.ServiceTypeName = XmlUtil.GetAttributeValue(serviceelem, "inf");
         string isdefault = XmlUtil.GetAttributeValue(serviceelem, "isdefault");
         if (isdefault.Equals("true", StringComparison.CurrentCultureIgnoreCase))
            srvinfo.IsDefault = true;
         else
            srvinfo.IsDefault = false;
         return srvinfo;
      }
   }
}
