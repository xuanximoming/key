using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 条件分类
    /// </summary>
    public enum QCConditionType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0, 

        /// <summary>
        /// 病人状态改变
        /// </summary>
        [Description("病人状态改变")]
        PatStateChange = 1,

        /// <summary>
        /// 病历文件改变
        /// </summary>
        [Description("病历文件改变")]
        EmrChange = 2,
        
        /// <summary>
        /// 医嘱改变
        /// </summary>
        [Description("医嘱改变")]
        AdviceChange = 3,
    }
}
