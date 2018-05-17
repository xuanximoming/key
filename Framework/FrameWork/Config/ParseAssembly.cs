using DrectSoft.FrameWork.BizBus.Service;
using System;
using System.Reflection;

namespace DrectSoft.FrameWork.Config
{
    /// <summary>
    /// 解析业务程序集
    /// </summary>
    public class ParseAssembly
    {
        /// <summary>
        /// 获取业务插件的服务信息
        /// </summary>
        /// <param name="infpath"></param>
        /// <param name="implepath"></param>
        /// <returns></returns>
        public ServicePackageInfo GetAssemblyInfo(string infpath, string implepath)
        {
            ServicePackageInfo info = new ServicePackageInfo();
            info.InfAssemblyName = ConfigUtil.GetBizPluginAssemblyName(infpath);
            info.ImpleAssemblyName = ConfigUtil.GetBizPluginAssemblyName(implepath);
            Assembly[] ass = AppDomain.CurrentDomain.GetAssemblies();

            Assembly infassembly = AssemblyHelper.LoadAssembly(infpath);
            Assembly impleassembly = AssemblyHelper.LoadAssembly(implepath);

            Type[] inftypes = infassembly.GetTypes();
            Type[] impletypes = impleassembly.GetTypes();

            foreach (Type t in impletypes)
            {
                if (t.GetInterface("DrectSoft.FrameWork.BizBus.Service.IService") != null)
                {
                    Type[] ts = t.GetInterfaces();
                    foreach (Type parenttype in ts)
                    {
                        if (!parenttype.Equals(typeof(IService)))
                        {
                            Type servicetype = FindTypes(inftypes, parenttype);
                            if (servicetype != null)
                            {
                                ServiceInfo si = new ServiceInfo();
                                si.Key = servicetype.FullName;
                                si.ServiceTypeName = servicetype.FullName;
                                si.ImpleTypeName = t.FullName;
                                info.Services.Add(si);
                            }
                        }
                    }
                }
            }
            return info;
        }

        private Type FindTypes(Type[] types, Type type)
        {
            foreach (Type t in types)
            {
                if (t.Equals(type))
                    return t;
            }
            return null;
        }
    }
}
