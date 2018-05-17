using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Config {
    /// <summary>
    /// 配置文件根据
    /// </summary>
    public class ConfigUtil {
        /// <summary>
        /// 读取业务插件全局路径
        /// </summary>
        /// <param name="assemblypath"></param>
        /// <returns></returns>
        public static string GetBizPluginFullPath(string assemblypath) {
            if (assemblypath.IndexOf("BizPlugin", 0, StringComparison.CurrentCultureIgnoreCase) == -1)
                return @"bizplugins\" + assemblypath;
            else
                return assemblypath;
        }

        /// <summary>
        /// 读取业务插件程序集名
        /// </summary>
        /// <param name="assemblypath"></param>
        /// <returns></returns>
        public static string GetBizPluginAssemblyName(string assemblypath) {
            string[] strs = assemblypath.Split('\\');
            return strs[strs.Length - 1];
        }
    }
}
