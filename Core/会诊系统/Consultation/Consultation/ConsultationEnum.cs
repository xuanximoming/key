using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.Consultation
{
    ///// <summary>
    ///// 会诊类别
    ///// </summary>
    //public enum ConsultType
    //{
    //    /// <summary>
    //    /// 单科会诊（一般会诊）
    //    /// </summary>
    //    One = 6501,

    //    /// <summary>
    //    /// 多科会诊
    //    /// </summary>    
    //    More = 6502   
    //}

    /// <summary>
    /// 紧急程度
    /// </summary>
    public enum UrgencyType
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal = 6601,

        /// <summary>
        /// 紧急
        /// </summary>
        Urgency = 6602
    }

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
        CancelConsultion=6770,
         /// <summary>
        /// 已撤销 add by ywk 二〇一三年六月十四日 15:26:51 
        /// </summary>
        UndoConsultion=6780
    }

    public enum SaveType
    {
        /// <summary>
        /// 数据库插入操作
        /// </summary>
        Insert = 1,

        /// <summary>
        /// （会诊申请界面）修改数据
        /// </summary>
        Modify = 2,

        /// <summary>
        /// (会诊记录填写界面) 修改数据
        /// </summary>
        RecordModify = 3
    }

    public enum Grade
    {
        /// <summary>
        /// 主任
        /// </summary>
        Director = 2000,

        /// <summary>
        /// 副主任
        /// </summary>
        ViceDirector = 2001,

        /// <summary>
        /// 主治医师
        /// </summary>
        StaffPhysician = 2002,

        /// <summary>
        /// 住院医师
        /// </summary>
        Resident = 2003
    }
}
