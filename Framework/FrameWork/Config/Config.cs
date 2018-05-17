using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using DrectSoft.FrameWork.BizBus;

namespace DrectSoft.FrameWork.Config
{
    /// <summary>
    /// 应用程序配置
    /// </summary>
    public class AppSettingConfig
    {
        /// <summary>
        /// 获取远程访问URL
        /// </summary>
        /// <returns></returns>
        public static string getRemoteUriPath()
        {
            return ConfigurationManager.AppSettings["BaseUriPath"];
        }

        /// <summary>
        /// 访问本地配置文件
        /// </summary>
        /// <returns></returns>
        public static string getLocalConfigPath()
        {
            return ConfigurationManager.AppSettings["BizConfigPath"];
        }
        /// <summary>
        /// 获取框架加载模式
        /// </summary>
        /// <returns></returns>
        public static CallMode GetCallMode()
        {
            string callmode = ConfigurationManager.AppSettings["CallMode"];
            if (("Remote").Equals(callmode, StringComparison.CurrentCultureIgnoreCase))
                return CallMode.Remote;
            return CallMode.Local;
        }
    }
}
