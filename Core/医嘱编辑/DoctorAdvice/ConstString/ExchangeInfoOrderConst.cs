using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 有关医嘱消息处理的静态字符串
   /// </summary>
   public static class ExchangeInfoOrderConst
   {
      /// <summary>
      /// EMR系统名称
      /// </summary>
      public const string EmrSystemName = "NetWorkStudio-EMR-1.0";
      /// <summary>
      /// HIS系统名称
      /// </summary>
      public const string HisSystemName = "NetWorkStudio-HIS-4.0";
      /// <summary>
      /// 消息名：在HIS中检查医嘱
      /// </summary>
      public const string MsgCheckData = "CHECKADVISES2HIS";
      /// <summary>
      /// 消息名：保存医嘱到HIS
      /// </summary>
      public const string MsgSaveData = "SAVEADVISES2HIS";
      /// <summary>
      /// 消息名：获取病人扩展信息
      /// </summary>
      public const string MsgGetExtraInfo = "GETPATEXTRAINFO";
      /// <summary>
      /// 消息名：更新病人信息
      /// </summary>
      public const string MsgUpdatePatient = "UPDATEPATINFO";
      /// <summary>
      /// 消息名：获取医嘱打印参数设置
      /// </summary>
      public const string MsgGetPrintSettings = "GETORDERPRINTSETTINGS";
      /// <summary>
      /// 消息名：处理医嘱打印（查询和更新）
      /// </summary>
      public const string MsgProcessOrderPrint = "PROCESSORDERPRINT";
      /// <summary>
      /// 消息名：获取处方规则
      /// </summary>
      public const string MsgGetRecipeRules = "GETRECIPERULES";
      /// <summary>
      /// 消息名：从HIS读取医嘱数据
      /// </summary>
      public const string MsgGetHisOrder = "GETHISORDER";
      /// <summary>
      /// 默认的消息编码名称
      /// </summary>
      public const string EncodingName = "utf-8";

      /// <summary>
      /// 默认使用的编码格式
      /// </summary>
      public static Encoding DefaultEncoding = Encoding.GetEncoding(EncodingName);
   }
}
