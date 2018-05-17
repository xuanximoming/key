using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.BizBus {
    /// <summary>
    /// 调用模式
    /// </summary>
    public enum CallMode {
        /// <summary>
        /// //本地访问
        /// </summary>
        Local = 0,
        /// <summary>
        ///WCF远程访问
        /// </summary>
        Remote = 1,
    }
}
