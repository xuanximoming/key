using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using DrectSoft.FrameWork.util;
using DrectSoft.FrameWork.BizBus.Service;

namespace DrectSoft.FrameWork.Config {
    /// <summary>
    /// 服务包装载器
    /// </summary>
    [Serializable]
    public class ServicePackageLoader {
        /// <summary>
        /// application路径
        /// </summary>
        public static string ApplicationPath = "";

        /// <summary>
        /// 从配置文件装载服务包信息，并通过Assembly验证
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public ServicePackageInfo GetPackageInfoAssemblyValidate(XmlElement elem) {
            ServicePackageInfo bizinfo = PackageInfoLoader.GetPackageInfo(elem);
            ParseAssembly pa = new ParseAssembly();
            string infAssemblyPath = Global.defaultBizFolder + bizinfo.InfAssemblyName;
            string impleAssemblyPath = Global.defaultBizFolder + bizinfo.ImpleAssemblyName;
            ServicePackageInfo assemblyinfo = pa.GetAssemblyInfo(infAssemblyPath, impleAssemblyPath);

            ServicePackageInfo result = new ServicePackageInfo();
            result.ImpleAssemblyName = bizinfo.ImpleAssemblyName;
            result.InfAssemblyName = bizinfo.InfAssemblyName;
            foreach (ServiceInfo service in bizinfo.Services) {
                if (FindService(assemblyinfo.Services, service) != null)
                    result.Services.Add(service);
            }
            return result;
        }

        /// <summary>
        /// 寻找指定服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public ServiceInfo FindService(List<ServiceInfo> services, ServiceInfo service) {
            foreach (ServiceInfo info in services) {
                if (info.ServiceTypeName == service.ServiceTypeName &&
                   info.ImpleTypeName == service.ImpleTypeName)
                    return info;
            }
            return null;
        }

        //public IServicePackage GetServicePackage(XmlElement elem)
        //{
        //   ServicePackageInfo info = PackageInfoLoader.GetPackageInfo(elem);
        //   return Convert2Package(info);
        //}

        /// <summary>
        /// 服务包裹创建
        /// </summary>
        /// <returns></returns>
        public static ServicePackageLoader Create() {
            return new ServicePackageLoader();
        }
    }
}