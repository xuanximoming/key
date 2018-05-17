using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ZymosisReport
{
    enum ZymosisReportEnum : int
    {
        /// <summary>
        /// 保存
        /// </summary>
        Save = 1,
        /// <summary>
        /// 提交
        /// </summary>
        Submit = 2,
        /// <summary>
        /// 撤销
        /// </summary>
        WithDraw = 3,
        /// <summary>
        /// 审核通过
        /// </summary>
        Pass = 4,
        /// <summary>
        /// 审核否决
        /// </summary>
        Reject = 5,
        /// <summary>
        /// 上报
        /// </summary>
        Report = 6,
        /// <summary>
        /// 作废
        /// </summary>
        Cancel = 7
    }
}
