using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Globalization;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 医嘱特殊标记(以二进制位表示)
   /// </summary>
   [Flags]
   public enum OrderAttributeFlag
   {
      /// <summary>
      /// 自备药标志
      /// </summary>
      Provide4Oneself = 1,
      /// <summary>
      /// 输液医嘱
      /// </summary>
      TransfusionOrder = 2,
      /// <summary>
      /// 需要打印标志
      /// </summary>
      NeedPrint = 4,
      /// <summary>
      /// 是否需要停长期医嘱(只对手术医嘱有效)
      /// </summary>
      CeaseLongOrder = 8,
      /// <summary>
      /// 需要医保审批标志
      /// </summary>
      NeedAudit = 16,

      /// <summary>
      /// 暂不启用
      /// 是否需要拷贝长嘱到临嘱
      /// </summary>
      NeedSupLongOderToTemp = 32,

      /// <summary>
      /// 医嘱“明起”标志
      /// </summary>
      UsedFromTomorrow= 64,

   }
}
