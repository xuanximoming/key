using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Common.MediComIntf
{
   /// <summary>
   /// 美康接口药品信息类
   /// </summary>
   public struct MediIntfDrugInfo
   {
      /// <summary>
      ///医嘱唯一码，要求能唯一标记医嘱记录
      /// </summary>
      public string OrderUniqueCode;
      /// <summary>
      ///必须传，医嘱中的药品唯一码
      /// </summary>
      public string DrugCode;
      /// <summary>
      ///必须传，医嘱中的药品名称
      /// </summary>
      public string DrugName;
      /// <summary>
      ///剂量，表示每次使用剂量的数字部分
      /// </summary>
      public string SingleDose;
      /// <summary>
      ///剂量单位，表示每次服用剂量单位，要求与药品配对剂量单位完全一致单位完全一致，否则可能造成剂量审查不正确。
      /// </summary>
      public string DoseUnit;
      /// <summary>
      ///频次 ，表示药品服用频次信息。传入要求：n天m次，传"m/n"，例如：1天3次，传"3/1"；7天2次，传"2/7"。
      /// </summary>
      public string Frequency;
      /// <summary>
      ///开嘱日期，表示开立医嘱日期。格式为"yyyy-mm-dd"，例如开嘱日期为1999年3月12日，则应传入"1999-3-12"。传空时，系统默认为当前日期。
      /// </summary>
      public string StartDate;
      /// <summary>
      ///停嘱日期 表示停嘱日期，对于未停医嘱，应传为当天日期。格式为"yyyy-mm-dd"，例如停嘱日期为1999年3月12日，则应传入"1999-3-12"。传空时，系统默认为当前日期。
      /// </summary>
      public string EndDate;
      /// <summary>
      ///必须传，表示给药途径名称，例如"口服"、"静滴"等。注意，由于PASS系统审查与用法关系密切，此参数传入错误，将直接导致审查错误。
      /// </summary>
      public string RouteName;
      /// <summary>
      ///传出，医嘱警示值(可不传此值，主要用于返回审查警示值)
      /// </summary>
      public PassWarnType Warn;
      /// <summary>
      ///成组医嘱标记，不同，则认为是一组医嘱
      /// </summary>
      public string GroupTag;
      /// <summary>
      ///医嘱类型1 临时 0 长期 
      /// </summary>
      public string OrderType;
      /// <summary>
      ///医嘱医生Id
      /// </summary>
      public string OrderDoctorId;
      /// <summary>
      /// 医嘱医生名称
      /// </summary>
      public string OrderDoctorName;
   }
}
