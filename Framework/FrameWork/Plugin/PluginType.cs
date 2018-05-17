using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Plugin {
    /// <summary>
    /// 插件类型
    /// </summary>
    public enum PluginType {
        /// <summary>
        /// 框架插件，用作构建框架界面
        /// </summary>
        Frame,

        /// <summary>
        /// 核心插件，负责核心业务
        /// </summary>
        Core,

        /// <summary>
        /// 业务逻辑插件
        /// </summary>
        Biz,

        /// <summary>
        /// 外部插件，用作具体业务
        /// </summary>
        External
    }

    /// <summary>
    /// 插件类型判断
    /// </summary>
    public class CPluginType {
        /// <summary>
        /// 获取插件类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PluginType GetPluginType(string name) {

            switch (name.ToUpper(System.Globalization.CultureInfo.CurrentCulture)) {
                case "COREPLUGIN":
                    return PluginType.Core;
                case "BIZPLUGIN":
                    return PluginType.Biz;
                case "FRAMEPLUGIN":
                    return PluginType.Frame;
                case "EXTERNALPLUGIN":
                    return PluginType.External;
                default:
                    return PluginType.Biz;
            }
        }
    }
}