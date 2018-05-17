using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DrectSoft.FrameWork.Plugin
{
    /// <summary>
    /// 插件信息类，所有插件信息父类
    /// 现有插件有如下：
    /// 业务插件
    /// 外部插件    
    /// </summary>
    public abstract class PluginInfo
    {
        private string _assemblyName;        //插件
        private string _assemblyFullName;    //

        /// <summary>
        /// 初始化插件信息类
        /// </summary>
        public PluginInfo()
        {
        }

        /// <summary>
        /// 获取或设置插件的程序集文件名称
        /// </summary>
        /// <value></value>
        public string AssemblyName
        {
            get { return _assemblyName; }
            set { _assemblyName = value; }
        }

        /// <summary>
        /// 获取或设置插件的程序集全名
        /// </summary>
        public string AssemblyFullName
        {
            get { return _assemblyFullName; }
            set { _assemblyFullName = value; }
        }
    }
}
