/*****************************************************************************\
**             Yindansoft & DrectSoft Software Co. Ltd.                          **
**                                                                           **
**             诊断性质的字典类定义                                           **
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
   /// 疾病诊断ICD-10
   /// </summary>
   public sealed class ICD10 : BaseWordbook
   {
      /// <summary>
      /// 创建疾病诊断字典对象
      /// </summary>
      public ICD10() 
         : base("Diagnose.ICD10")
      { }

      /// <summary>
      /// 创建疾病诊断字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public ICD10(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Diagnose.ICD10", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 Diagnose 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         throw new NotImplementedException();
      }
   }

   /// <summary>
   /// 肿瘤库
   /// </summary>
   public sealed class TumourBook : BaseWordbook
   {
      /// <summary>
      /// 创建 肿瘤库 字典对象
      /// </summary>
      public TumourBook()
         : base("Diagnose.TumourBook")
      { }

      /// <summary>
      /// 创建 肿瘤库 字典对象
      /// </summary>
      public TumourBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Diagnose.TumourBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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

   /// <summary>
   /// 损伤中毒库
   /// </summary>
   public sealed class DamnificationBook : BaseWordbook
   {
      /// <summary>
      /// 创建 损伤中毒库 字典对象
      /// </summary>
      public DamnificationBook()
         : base("Diagnose.DamnificationBook")
      { }

      /// <summary>
      /// 创建 损伤中毒库 字典对象
      /// </summary>
      public DamnificationBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Diagnose.DamnificationBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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

   /// <summary>
   /// 手术诊断
   /// </summary>
   public sealed class OperationBook : BaseWordbook
   {
      /// <summary>
      /// 创建 手术诊断 字典对象
      /// </summary>
      public OperationBook()
         : base("Diagnose.OperationBook")
      { }

      /// <summary>
      /// 创建 手术诊断 字典对象
      /// </summary>
      public OperationBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Diagnose.OperationBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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
   /// 麻醉字典
   /// </summary>
   public sealed class AnesthesiaBook : BaseWordbook
   {
      /// <summary>
      /// 创建 麻醉字典 字典对象
      /// </summary>
      public AnesthesiaBook()
         : base("Diagnose.AnesthesiaBook")
      { }

      /// <summary>
      /// 创建 麻醉字典 字典对象
      /// </summary>
      public AnesthesiaBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Diagnose.AnesthesiaBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new Anesthesia(CurrentRow);
          
      }
   }

   /// <summary>
   /// 中医诊断字典
   /// </summary>
   public sealed class ChineseDiagnosisBook : BaseWordbook
   {
      /// <summary>
      /// 创建 麻醉字典 字典对象
      /// </summary>
      public ChineseDiagnosisBook()
         : base("Diagnose.ChineseDiagnosisBook")
      { }

      /// <summary>
      /// 创建 麻醉字典 字典对象
      /// </summary>
      public ChineseDiagnosisBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Diagnose.ChineseDiagnosisBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new ChineseDiagnosis(CurrentRow);
      }
   }
}
