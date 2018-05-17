/*****************************************************************************\
**            DrectSoft Software Co. Ltd.                          **
**                                                                           **
**             其它类别的字典类定义                                           **
**                                                                           **
**                                                                           **
\*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using DrectSoft.Common.Eop;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// 计量单位
   /// </summary>
   public sealed class MeasureUnit : BaseWordbook
   {
      /// <summary>
      /// 创建 计量单位 字典对象
      /// </summary>
      public MeasureUnit()
         : base("Other.MeasureUnit")
      { }

      /// <summary>
      /// 创建 计量单位 字典对象
      /// </summary>
      public MeasureUnit(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Other.MeasureUnit", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建  类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         throw new NotImplementedException();
      }
   }

   /// <summary>
   /// 计量单位类别
   /// </summary>
   public sealed class MeasureUnitCatalog : BaseWordbook
   {
      /// <summary>
      /// 创建 计量单位类别 字典对象
      /// </summary>
      public MeasureUnitCatalog()
         : base("Other.MeasureUnitCatalog")
      { }

      /// <summary>
      /// 创建 计量单位类别 字典对象
      /// </summary>
      public MeasureUnitCatalog(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Other.MeasureUnitCatalog", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         throw new NotImplementedException();
      }
   }
}
