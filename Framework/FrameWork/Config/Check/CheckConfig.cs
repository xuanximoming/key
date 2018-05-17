using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using DrectSoft.FrameWork.Log;
using System.Reflection.Emit;
using DrectSoft.FrameWork.Execptions;
using DrectSoft.FrameWork.Plugin;

namespace DrectSoft.FrameWork.Config {
    /// <summary>
    /// ConfigCheck
    /// </summary>
    public class CheckConfig {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyinfo"></param>
        /// <param name="errinfo"></param>
        /// <returns></returns>
        public bool ValidatePackage(ServicePackageInfo assemblyinfo, out ErrPackageInfo errinfo) {
            errinfo = new ErrPackageInfo();
            string infpath = assemblyinfo.InfAssemblyName;
            string implepath = assemblyinfo.ImpleAssemblyName;

            Assembly infAssembly = GetAssembly(ConfigUtil.GetBizPluginFullPath(infpath));
            Assembly impleAssembly = GetAssembly(ConfigUtil.GetBizPluginFullPath(implepath));

            if (!ValidateAssembly(errinfo, infAssembly, impleAssembly))
                return false;

            List<ServiceInfo> errservices = CheckServices(infAssembly, impleAssembly, assemblyinfo);

            foreach (ServiceInfo info in errservices) {
                ErrServiceInfo errserviceinfo = new ErrServiceInfo();
                errserviceinfo.ServiceKey = info.Key;
                errinfo.ErrServiceInfoList.Add(errserviceinfo);
            }

            //if (errservices.Count != 0)
            //{            
            //Console.WriteLine(errservices.Count);
            //for (int i = 0; i < errservices.Count; i++)
            //{
            //   Console.WriteLine("service: " + errservices[i].ServiceTypeName + " imple:" + errservices[i].ImpleTypeName);
            //}
            //throw new ConfigException("配置文件服务和对应Assembly内服务不符");
            //}
            return true;
        }

        private static bool ValidateAssembly(ErrPackageInfo errinfo, Assembly infAssembly, Assembly impleAssembly) {
            if (infAssembly == null || impleAssembly == null) {
                errinfo.ErrAssemblyInfo.HasAssembly = false;
                return false;
            }
            object[] infattributes = infAssembly.GetCustomAttributes(typeof(BizPluginAttribute), true);
            object[] impleattributes = impleAssembly.GetCustomAttributes(typeof(BizPluginAttribute), true);
            if (infattributes.Length == 0 || impleattributes.Length == 0) {
                errinfo.ErrAssemblyInfo.AssemblyHasAttribute = false;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取指定程序集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(string path) {
            Assembly assembly = null;
            try {
                //assembly = Assembly.ReflectionOnlyLoadFrom(path);
                assembly = Assembly.LoadFrom(path);
            }
            catch (Exception e) {
                LogFactory.Create().Write(e.Message);

                return null;
            }
            return assembly;
        }

        private List<ServiceInfo> CheckServices(Assembly infassembly, Assembly impleassembly, ServicePackageInfo assemblyinfo) {
            List<ServiceInfo> errservice = new List<ServiceInfo>();

            for (int i = 0; i < assemblyinfo.Services.Count; i++) {
                ServiceInfo info = assemblyinfo.Services[i];
                Type inftype = infassembly.GetType(info.ServiceTypeName, false);
                if (inftype == null) {
                    errservice.Add(info);
                    continue;
                }
                Type impletype = impleassembly.GetType(info.ImpleTypeName, false);
                if (impletype == null) {
                    errservice.Add(info);
                    continue;
                }
            }
            return errservice;
        }
    }

    /// <summary>
    /// 配置文件错误
    /// </summary>
    public class ConfigException : FrameWorkException {
        /// <summary>
        /// ctor
        /// </summary>
        public ConfigException()
            : base() { }
        /// <summary>
        /// ctor1
        /// </summary>
        /// <param name="message"></param>
        public ConfigException(string message)
            : base(message) { }
    }
}