using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DrectSoft.FrameWork.Config {
    /// <summary>
    /// 保存包信息
    /// </summary>
    public class PackageInfoSaver {
        /// <summary>
        /// 从配置文件装载插件信息
        /// </summary>
        /// <param name="info">服务包裹信息</param>
        /// <param name="elem">装载的插件元素</param>
        /// <returns></returns>
        public static void SavePackageInfo(ServicePackageInfo info, XmlElement elem) {
            elem.RemoveAll();
            elem.SetAttribute("Name", info.Name);
            elem.SetAttribute("ImpleAssembly", info.ImpleAssemblyName);
            elem.SetAttribute("InfAssembly", info.InfAssemblyName);
            foreach (ServiceInfo s in info.Services) {
                XmlElement childelem = elem.OwnerDocument.CreateElement("service");
                SaveServiceInfo(s, childelem);
                elem.AppendChild(childelem);
            }
        }

        private static void SaveServiceInfo(ServiceInfo srvinfo, XmlElement serviceelem) {
            serviceelem.SetAttribute("key", srvinfo.Key);
            serviceelem.SetAttribute("imple", srvinfo.ImpleTypeName);
            serviceelem.SetAttribute("inf", srvinfo.ServiceTypeName);
            serviceelem.SetAttribute("isdefault", srvinfo.IsDefault.ToString());
        }
    }
}
