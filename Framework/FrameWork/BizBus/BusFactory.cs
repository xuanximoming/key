using DrectSoft.FrameWork.BizBus.Service;
using DrectSoft.FrameWork.Config;
using DrectSoft.FrameWork.Config.ConfigFile;
using System.Collections.Generic;
using System.ServiceModel;
using System.Xml;

namespace DrectSoft.FrameWork.BizBus
{
    /// <summary>
    /// 总线工厂
    /// </summary>
    public class BusFactory
    {
        static readonly object padlock = new object();
        static IBizBus bizbus = null;

        /// <summary>
        /// 获取或创建业务总线
        /// </summary>
        /// <returns></returns>
        public static IBizBus GetBus()
        {
            if (bizbus == null)
            {
                lock (padlock)
                {
                    if (bizbus == null)
                    {
                        CallMode callmode = AppSettingConfig.GetCallMode();
                        if (callmode == CallMode.Local)
                            InitializeClientBus();
                        else if (callmode == CallMode.Remote)
                            InitializeRemoteBus();
                    }
                }
            }
            return bizbus;
        }

        /// <summary>
        /// 初始化服务总线(本地调用不走WCF服务)
        /// </summary>
        public static void InitializeClientBus()
        {
            if (bizbus == null)
                bizbus = new ClientBizBus();

            ClientBizBus cbizbus = bizbus as ClientBizBus;

            List<ServicePackageInfo> packageinfos = ConfigLoader.GetPackageList();
            ServicePackageLoader loader = new ServicePackageLoader();
            ServicePackageList packages = new ServicePackageList();

            foreach (ServicePackageInfo info in packageinfos)
            {
                IServicePackage package = PackageConverter.Convert2Package(info);
                if (package == null)
                    continue;
                packages.Add(package);
            }
            cbizbus.packages = packages;
            cbizbus.IndexPackage();
        }

        /// <summary>
        /// 初始化服务总线(使用WCF服务)
        /// </summary>
        public static void InitializeRemoteBus()
        {
            bizbus = new RemoteBizBus();
            RemoteBizBus rbizbus = bizbus as RemoteBizBus;
            string configuri = AppSettingConfig.getRemoteUriPath() + "Config";
            string configPath = AppSettingConfig.getLocalConfigPath();
            BasicHttpBinding bhb = new BasicHttpBinding("BasicHttpBinding_Service");
            EndpointAddress epa = new EndpointAddress(configuri);
            IHostConfig hostconfig = ChannelFactory<IHostConfig>.CreateChannel(bhb, epa);
            XmlDocument doc = new XmlDocument();
            if (string.IsNullOrEmpty(configPath))
            {
                string configstring = hostconfig.GetConfigString();
                doc.LoadXml(configstring);
            }
            //else
            //{
            //    string file = AppDomain.CurrentDomain.BaseDirectory + @"\namespace DrectSoft.FrameWork.Config";
            //    doc.Load(file);
            //}

            List<ServicePackageInfo> packages = ConfigLoader.GetPackageList(doc.DocumentElement);
            rbizbus.packages = packages;
        }

        /// <summary>
        /// 添加服务包
        /// </summary>
        /// <param name="package"></param>
        public static void AddPackage(IServicePackage package)
        {
            ClientBizBus cbizbus = bizbus as ClientBizBus;
            cbizbus.packages.Add(package);
            cbizbus.IndexPackage();
        }

        /// <summary>
        /// 移除服务
        /// </summary>
        /// <param name="package"></param>
        public static void RemovePackage(IServicePackage package)
        {
            ClientBizBus cbizbus = bizbus as ClientBizBus;
            cbizbus.packages.Remove(package);
            cbizbus.IndexPackage();
        }
    }
}
