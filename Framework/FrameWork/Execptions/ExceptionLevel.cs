using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.FrameWork.Execptions {
    /// <summary>
    /// 错误警报级别
    /// </summary>
    public enum ExceptionLevel {
        /// <summary>
        ///低等级错误，只写日志
        /// </summary>
        Low = 0,
        /// <summary>
        /// 中等级错误，写日志并且前台报警
        /// </summary>
        Middle = 1,
        /// <summary>
        /// 高等级错误，系统报错
        /// </summary>
        High = 2,
    }
}
