using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.FrameWork.BizBus.Service;

namespace DrectSoft.FrameWork.BizBus
{
   internal class PackageListSearcher
   {
      Dictionary<ServiceKey, ServiceDesc> dicPackage;
      Dictionary<Type, ServiceDesc> dicDefault;

      /// <summary>
      /// 服务包列表搜索
      /// </summary>
      public PackageListSearcher()
      {
         dicPackage = new Dictionary<ServiceKey, ServiceDesc>();
         dicDefault = new Dictionary<Type, ServiceDesc>();         
      }

      /// <summary>
      /// 查找服务包,根据关键字查找
      /// </summary>
      /// <param name="key">服务关键字</param>
      /// <returns></returns>
      public ServiceDesc SearchPackage(ServiceKey key)
      {
         if (dicPackage.ContainsKey(key))
            return dicPackage[key];
         return null;
      }

      /// <summary>
      /// 查找缺省服务
      /// </summary>
      /// <param name="type">服务类型</param>
      /// <returns></returns>
      public ServiceDesc SearchDefault(Type type)
      {
         if (dicDefault.ContainsKey(type))
            return dicDefault[type];
         return null;
      }

      /// <summary>
      /// 加入服务列表
      /// </summary>
      /// <param name="packages"></param>
      public void IndexPackageList(ServicePackageList packages)
      {
         dicPackage.Clear();
         dicDefault.Clear();

         foreach (IServicePackage package in packages)
         {
            foreach (ServiceDesc sd in package.ReadOnlyServices)
            {
               ServiceKey key = new ServiceKey(sd);
               dicPackage.Add(key, sd);
            }
            foreach (ServiceKey key in package.ReadOnlyDefaultServices)
            {
               dicDefault.Add(key.ServiceType, dicPackage[key]);
            }
         }
      }
   }
}
