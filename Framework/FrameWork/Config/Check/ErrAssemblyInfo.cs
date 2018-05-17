using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Config {
    /// <summary>
    /// 异常程序集信息
    /// </summary>
    public class ErrAssemblyInfo {
        /// <summary>
        /// 找不到程序集
        /// </summary>
        public bool HasAssembly;
        /// <summary>
        /// 程序集特性
        /// </summary>
        public bool AssemblyHasAttribute;
        /// <summary>
        /// 是否是业务插件错误
        /// </summary>
        public bool IsBizPluginNameError;
    }
}
