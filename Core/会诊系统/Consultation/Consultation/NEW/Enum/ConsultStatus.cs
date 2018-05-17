using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.Consultation.NEW.Enum
{
    /// <summary>
    /// 会诊状态
    /// </summary>
    public enum ConsultStatus
    {
        /// <summary>
        /// 会诊申请保存
        /// </summary>
        ApplySave = 6710,

        /// <summary>
        /// 待签核
        /// </summary>
        WaitApprove = 6720,

        /// <summary>
        /// 待会诊
        /// </summary>
        WaitConsultation = 6730,

        /// <summary>
        /// 会诊记录保存
        /// </summary>
        RecordeSave = 6740,

        /// <summary>
        /// 会诊记录完成
        /// </summary>
        RecordeComplete = 6741,

        /// <summary>
        /// 否决
        /// </summary>
        Reject = 6750,

        /// <summary>
        /// 已取消
        /// </summary>
        CancelConsultion = 6770,

        /// <summary>
        /// 已撤销 Add by wwj 2013-03-07
        /// </summary>
        CallBackConsultation = 6780
    }
}
