using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 项目性质(以二进制位表示)
   /// </summary>
   [Flags]
   public enum ItemAttributeFlag
   {
      /// <summary>
      /// 常规项目
      /// </summary>
      GeneralItem = 1,
      /// <summary>
      /// 文字医嘱
      /// </summary>
      TextOrder = 2
   }
}
