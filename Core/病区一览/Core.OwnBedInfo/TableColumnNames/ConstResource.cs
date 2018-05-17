using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.Common.Eop;

namespace DrectSoft.Core.OwnBedInfo
{
    internal class ConstResource
    {
        //用于过敏信息的字段名
        internal const string FieldSyxh = "syxh";
        internal const string FieldGgxh = "ggxh";
        internal const string FieldJlzt = "jlzt";
        internal const string FieldYpmc = "ypmc";
        internal const string FieldYxbz = "yxbz";
        internal const string FieldKsrq = "ksrq";
        internal const string FieldJsrq = "jsrq";
        internal const string HasIrritability = "有过敏";

        internal const string FieldMrss = "明日手术";
        internal const string FieldJrss = "今日手术";
        internal const string FieldShyt = "术后1天";
        internal const string FieldShet = "术后2天";
        internal const string FieldShst = "术后3天";

        internal const string FieldJrcy = "今日出院";
        internal const string FieldMrcy = "明日出院";
        internal const string FieldThcy = "天后出院";

        internal const string FieldWbl = "无病历";

        internal const string FieldJrICU = "进入ICU";
        internal const string FieldJrcf = "进入产房";
        internal const string FieldZkzt = "转科状态";

        internal const float OweLine = 0;
        internal const string AppName = "病区一览";

        //产科病区配置key
        internal const string ObstetricWardCode = "ObstetricWardCode";
        ////自动刷新时间配置
        //internal const string AUTOREFRESHSETTING = "AutoRefreshSetting";

        //internal const string REFRESHSETTINGKEY = "BedMapping";
        //internal const string REFRESHSETTINGSECTION = "Setting";
        //internal const string REFRESHSETTINGSECTIONKEY = "Key";
        //internal const string REFRESHSETTINGSECTIONVALUE = "Value";
        internal const string BedMappingSetting = "BedMappingSetting";
        //默认为1800秒的自动刷新间隔
        internal const int DefaultRefreshInterval = 1800;

        /// <summary>
        /// 转区病人的状态代码
        /// </summary>      
        internal const int STShiftDept = (int)InpatientState.ShiftDept;
        /// <summary>
        /// 出区病人的状态代码
        /// </summary>
        internal const int STOutWard = (int)InpatientState.OutWard;
        /// <summary>
        /// 病人出院的状态代码
        /// </summary>
        internal const int STBalanced = (int)Common.Eop.InpatientState.Balanced;
        internal const int ImageCount = 40;
        internal const int TimerInterval = 300;

        internal const string UpdateStartHint = "0分钟前刷新的病人信息";

        internal const string ActionDoubleClicked = "OnDoubleClicked";
        internal const string ActionClicked = "Clicked";
    }
}
