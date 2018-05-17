using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Emr.QCTimeLimit.QCEnum
{
    /// <summary>
    /// 操作方式 
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 一次性，只能触发一次
        /// </summary>
        OnlyOne = 0,

        /// <summary>
        /// 触发一次执行一次
        /// </summary>
        EveryOne = 1,

        /// <summary>
        /// 循环触发
        /// </summary>
        Circle = 2,
    }
}
