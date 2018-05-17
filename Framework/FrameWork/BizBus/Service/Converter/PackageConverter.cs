using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.FrameWork.Config;
using System.Reflection;

namespace DrectSoft.FrameWork.BizBus.Service
{
    /// <summary>
    /// 服务包转换器
    /// </summary>
   public class PackageConverter
   {
       /// <summary>
       /// 转换业务服务包
       /// </summary>
       /// <param name="info"></param>
       /// <returns></returns>
      public static IServicePackage Convert2Package(ServicePackageInfo info)
      {
         ServicePackage package = new ServicePackage();
         Assembly impleAssembly = getImpleAssembly(info);
         Assembly infAssembly = getInfAssembly(info);

         if (impleAssembly == null || infAssembly == null)
            return null;

         foreach (ServiceInfo service in info.Services)
         {
            Type servicetype = infAssembly.GetType(service.ServiceTypeName);
            if (servicetype != null)
            {
               Type t = Type.GetType(servicetype.AssemblyQualifiedName);
               if (t != null)
                  servicetype = t;
            }
            Type impletype = impleAssembly.GetType(service.ImpleTypeName);
            if (servicetype != null && impletype != null)
            {
               ServiceDesc desc = new ServiceDesc(servicetype, service.Key, impletype, service.IsDefault);
               package.AddService(desc);
               if (service.IsDefault)
                  package.SetDefaultService(new ServiceKey(desc));
            }
         }
         return package;
      }

      private static Assembly getImpleAssembly(ServicePackageInfo info)
      {
         string[] strs = info.ImpleAssemblyName.Split('\\');
         string names = (strs[strs.Length - 1]);

         string[] arrname = (names.Split('.'));
         int i = 1;
         string name = arrname[0];
         while (i < arrname.Length - 1)
         {
            name += "." + arrname[i];
            i++;
         }

         Assembly impleAssembly;
         try
         {
            impleAssembly = AppDomain.CurrentDomain.Load(name);
         }
         catch
         {
            impleAssembly = CheckConfig.GetAssembly(ConfigUtil.GetBizPluginFullPath(info.ImpleAssemblyName));
         }

         return impleAssembly;
      }

      private static Assembly getInfAssembly(ServicePackageInfo info)
      {
         return AssemblyHelper.LoadAssembly(info.InfAssemblyName);
      }
   }
}
