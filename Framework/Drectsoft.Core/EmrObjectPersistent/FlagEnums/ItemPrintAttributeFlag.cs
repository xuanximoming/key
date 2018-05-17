using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 项目打印属性
   /// </summary>
   [Flags]
   public enum ItemPrintAttributeFlag
   {
      /// <summary>
      /// 不显示频次
      /// </summary>
      NotShowFrequency = 1,
      /// <summary>
      /// 不显示用量
      /// </summary>
      NotShowAmount = 2
   }
}
