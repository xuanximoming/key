using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.Consultation.NEW.Enum
{
    /// <summary>
    /// 会诊记录指定填写人
    /// </summary>
    public enum ConsultRecordForWrite
    {
        /// <summary>
        /// 单科会诊且由受邀人填写
        /// </summary>
        SingleEmployee=1000,

        /// <summary>
        /// 单科会诊由申请人填写
        /// </summary>
        SingleApply=1001,

        /// <summary>
        /// 多科会诊且由受邀人填写
        /// </summary>
        MultiEmployee=1002,

        /// <summary>
        /// 多科会诊且有申请人填写
        /// </summary>
        MultiApply=1003

    }
}
