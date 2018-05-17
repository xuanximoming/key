using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 结果分类
    /// </summary>
    public enum QCResultType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 病历文件改变
        /// </summary>
        [Description("病历文件改变")]
        EmrChange = 1,

        /// <summary>
        /// 时间提示有效期
        /// </summary>
        [Description("时间提示有效期")]
        TimeChange = 2,
    }
}
