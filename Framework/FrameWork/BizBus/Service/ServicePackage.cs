using DrectSoft.FrameWork.Config.Check;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DrectSoft.FrameWork.BizBus.Service
{
    /// <summary>
    /// 服务包
    /// </summary>
    public class ServicePackage : IServicePackage
    {
        private Dictionary<ServiceKey, ServiceDesc> serviceDic;     //服务列表
        private Dictionary<Type, ServiceKey> defaultService;        //缺省服务列表

        /// <summary>
        /// 初始化业务总线
        /// </summary>
        public ServicePackage()
        {
            serviceDic = new Dictionary<ServiceKey, ServiceDesc>();
            defaultService = new Dictionary<Type, ServiceKey>();
        }

        #region IServiceManager Members

        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="desc"></param>
        public void AddService(ServiceDesc desc)
        {
            ConfigValidator.ValidateServiceDesc(desc);

            ServiceKey servicekey = new ServiceKey(desc);
            serviceDic.Add(servicekey, desc);
        }

        /// <summary>
        /// 释放服务
        /// </summary>
        /// <param name="key">服务关键字</param>
        public bool ReleaseService(ServiceKey key)
        {
            if (!serviceDic.Remove(key))
                return false;
            if (defaultService.ContainsValue(key))
                defaultService.Remove(key.ServiceType);

            return true;
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>服务描述</returns>
        public ServiceDesc GetService(ServiceKey key)
        {
            if (!serviceDic.ContainsKey(key))
                return null;
            return serviceDic[key];
        }

        /// <summary>
        /// 设置缺省服务
        /// </summary>
        /// <param name="key"></param>
        public void SetDefaultService(ServiceKey key)
        {
            if (!serviceDic.ContainsKey(key))
                throw new ArgumentException("没有该服务类型");
            if (!defaultService.ContainsKey(key.ServiceType))
                defaultService.Add(key.ServiceType, key);
            else
                defaultService[key.ServiceType] = key;
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public ServiceDesc GetDefaultService(Type serviceType)
        {
            if (defaultService.ContainsKey(serviceType))
                return serviceDic[defaultService[serviceType]];
            return null;
        }

        /// <summary>
        /// 获取服务数量
        /// </summary>
        public int ServiceCount
        {
            get { return this.serviceDic.Count; }
        }

        /// <summary>
        /// 清空所有服务
        /// </summary>
        public void Clear()
        {
            this.serviceDic.Clear();
        }

        /// <summary>
        /// 获取服务列表
        /// </summary>
        public ReadOnlyCollection<ServiceDesc> ReadOnlyServices
        {
            get
            {
                ServiceDesc[] sds = new ServiceDesc[serviceDic.Values.Count];
                serviceDic.Values.CopyTo(sds, 0);
                return new ReadOnlyCollection<ServiceDesc>(sds);
            }
        }

        /// <summary>
        /// 获取只读的默认服务集合
        /// </summary>
        public ReadOnlyCollection<ServiceKey> ReadOnlyDefaultServices
        {
            get
            {
                ServiceKey[] keys = new ServiceKey[defaultService.Values.Count];
                defaultService.Values.CopyTo(keys, 0);
                return new ReadOnlyCollection<ServiceKey>(keys);
            }
        }

        #endregion

        //#region IServiceManager Members 外部实现按照实现类型标记服务

        //ServiceDesc IServicePackage.GetService(Type impleType, Type serviceType)
        //{
        //   return GetService(new ServiceKey(serviceType,impleType.FullName));
        //}

        //void IServicePackage.AddService(Type serviceType, Type impleType)
        //{
        //   String key = impleType.FullName;
        //   AddService(new ServiceDesc(serviceType,key,impleType,false));
        //}

        //void IServicePackage.SetDefaultService(Type serviceType, Type impleType)
        //{
        //   if (!serviceDic.ContainsKey(new ServiceKey(serviceType, impleType.FullName)))
        //      throw new ArgumentException("没有该服务类型");
        //   if (!defaultService.ContainsKey(serviceType))
        //      defaultService.Add(serviceType, new ServiceKey(serviceType, impleType.FullName));
        //   else
        //      defaultService[serviceType] = new ServiceKey(serviceType, impleType.FullName);
        //}

        //#endregion

    }
}