using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace DrectSoft.FrameWork.BizBus.Service
{
   /// <summary>
   /// 服务包接口，加载和保存服务信息
   /// 有添加服务，释放服务，获取服务，设置服务的缺省实现，获取服务列表等功能
   /// </summary>
   public interface IServicePackage
   {
      /// <summary>
      /// 添加服务
      /// </summary>
      /// <param name="servicedesc">服务描述</param>
      void AddService(ServiceDesc servicedesc);

      /// <summary>
      /// 释放服务
      /// </summary>
      /// <param name="key">服务关键字</param>
      bool ReleaseService(ServiceKey key);

      /// <summary>
      /// 获取服务
      /// </summary>
      /// <param name="key">服务关键字</param>
      /// <returns></returns>
      ServiceDesc GetService(ServiceKey key);

      /// <summary>
      /// 设置缺省服务
      /// </summary>
      /// <param name="key">服务关键字</param>
      void SetDefaultService(ServiceKey key);

      /// <summary>
      /// 获取服务
      /// </summary>
      /// <param name="serviceType">服务类型</param>
      /// <returns></returns>
      ServiceDesc GetDefaultService(Type serviceType);

      /// <summary>
      /// 获取服务数量
      /// </summary>
      int ServiceCount { get;}

      /// <summary>
      /// 清空所有服务
      /// </summary>
      /// <returns></returns>
      void Clear();

      /// <summary>
      /// 获得只读服务列表
      /// </summary>
      ReadOnlyCollection<ServiceDesc> ReadOnlyServices { get;}

      /// <summary>
      /// 获取只读缺省服务
      /// </summary>
      ReadOnlyCollection<ServiceKey> ReadOnlyDefaultServices { get;}

      //#region by impleType

      ///// <summary>
      ///// 获取服务
      ///// </summary>
      ///// <param name="impleType"></param>
      ///// <param name="serviceType"></param>
      ///// <returns></returns>
      //ServiceDesc GetService(Type impleType, Type serviceType);

      ///// <summary>
      ///// 添加服务
      ///// </summary>
      ///// <param name="serviceType">服务类型</param>
      ///// <param name="impleType">实现类型</param>
      //void AddService(Type serviceType, Type impleType);

      ///// <summary>
      ///// 设置缺省服务
      ///// </summary>
      ///// <param name="serviceType"></param>
      ///// <param name="impleType"></param>
      //void SetDefaultService(Type serviceType, Type impleType);

      //#endregion

      ///// <summary>
      ///// 获取服务列表
      ///// </summary>
      
   }
}