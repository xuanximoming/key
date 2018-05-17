/*****************************************************************************\
**             DrectSoft Software Co. Ltd.                          **
**                                                                           **
**             一般常用的字典类定义                                           **
**                                                                           **
**                                                                           **
\*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.Common.Eop;
using DrectSoft.Core;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// 通用字典（字典分类明细）
   /// </summary>
   public sealed class CommonBook : BaseWordbook
   {
      /// <summary>
      /// 创建通用字典对象
      /// </summary>
      public CommonBook()
         : base("Normal.CommonBook")
      { }

      /// <summary>
      /// 创建通用字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public CommonBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.CommonBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         if (CurrentRow == null)
            return null;
         return new BasicDictionnary(CurrentRow);
      }
   }

   /// <summary>
   /// 性别
   /// </summary>
   public sealed class SexBook : BaseWordbook
   {
      /// <summary>
      /// 创建性别字典对象
      /// </summary>
      public SexBook()
         : base("Normal.SexBook")
      { }

      /// <summary>
      /// 创建性别字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public SexBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.SexBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         if (CurrentRow == null)
            return null;
         return new BasicDictionnary(CurrentRow);
      }
   }

   /// <summary>
   /// 职业
   /// </summary>
   public sealed class MetierBook : BaseWordbook
   {
      /// <summary>
      /// 创建职业字典对象
      /// </summary>
      public MetierBook()
         : base("Normal.MetierBook")
      { }

      /// <summary>
      /// 创建职业字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public MetierBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.MetierBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         if (CurrentRow == null)
            return null;
         return new BasicDictionnary(CurrentRow);
      }
   }

   /// <summary>
   /// 职称
   /// </summary>
   public sealed class TechnicalTitleBook : BaseWordbook
   {
      /// <summary>
      /// 创建职称字典对象
      /// </summary>
      public TechnicalTitleBook()
         : base("Normal.TechnicalTitleBook")
      { }

      /// <summary>
      /// 创建职称字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public TechnicalTitleBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.TechnicalTitleBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         if (CurrentRow == null)
            return null;
         return new BasicDictionnary(CurrentRow);
      }
   }

   /// <summary>
   /// 联系人关系
   /// </summary>
   public sealed class RelationBook : BaseWordbook
   {
      /// <summary>
      /// 创建联系人关系字典对象
      /// </summary>
      public RelationBook()
         : base("Normal.RelationBook")
      { }

      /// <summary>
      /// 创建联系人关系字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public RelationBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.RelationBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         if (CurrentRow == null)
            return null;
         return new BasicDictionnary(CurrentRow);
      }
   }

   /// <summary>
   /// 治疗结果
   /// </summary>
   public sealed class TreatResultBook : BaseWordbook
   {
      /// <summary>
      /// 创建治疗结果字典对象
      /// </summary>
      public TreatResultBook()
         : base("Normal.TreatResultBook")
      { }

      /// <summary>
      /// 创建治疗结果字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public TreatResultBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.TreatResultBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         if (CurrentRow == null)
            return null;
         return new BasicDictionnary(CurrentRow);
      }
   }

   /// <summary>
   /// 切口等级
   /// </summary>
   public sealed class IncisionLevelBook : BaseWordbook
   {
      /// <summary>
      /// 创建切口登记字典对象
      /// </summary>
      public IncisionLevelBook()
         : base("Normal.IncisionLevelBook")
      { }

      /// <summary>
      /// 创建切口等级字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public IncisionLevelBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.IncisionLevelBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         if (CurrentRow == null)
            return null;
         return new BasicDictionnary(CurrentRow);
      }
   }

   /// <summary>
   /// 愈合类别
   /// </summary>
   public sealed class CicatrizationKindBook : BaseWordbook
   {
      /// <summary>
      /// 创建愈合类别字典对象
      /// </summary>
      public CicatrizationKindBook()
         : base("Normal.CicatrizationKindBook")
      { }

      /// <summary>
      /// 创建愈合类别字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public CicatrizationKindBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.CicatrizationKindBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         if (CurrentRow == null)
            return null;
         return new BasicDictionnary(CurrentRow);
      }
   }

   /// <summary>
   /// 切口愈合等级(由切口和愈合类别的数据组合得到)
   /// </summary>
   public sealed class IncisionCicatrizationKindBook : BaseWordbook
   {
      /// <summary>
      /// 创建愈合类别字典对象
      /// </summary>
      public IncisionCicatrizationKindBook()
         : base("Normal.IncisionCicatrizationKindBook")
      { }

      /// <summary>
      /// 创建愈合类别字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public IncisionCicatrizationKindBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.IncisionCicatrizationKindBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         return null;
      }
   }

   /// <summary>
   /// 婚姻状况
   /// </summary>
   public sealed class MarriageStateBook : BaseWordbook
   {
      /// <summary>
      /// 创建MarriageStateBook字典对象
      /// </summary>
      public MarriageStateBook()
         : base("Normal.MarriageStateBook")
      { }

      /// <summary>
      /// 创建MarriageStateBook字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public MarriageStateBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.MarriageStateBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         if (CurrentRow == null)
            return null;
         return new BasicDictionnary(CurrentRow);
      }
   }

   /// <summary>
   /// 生育状况
   /// </summary>
   public sealed class BearStateBook : BaseWordbook
   {
      /// <summary>
      /// 创建BearStateBook字典对象
      /// </summary>
      public BearStateBook()
         : base("Normal.BearStateBook")
      { }

      /// <summary>
      /// 创建BearStateBook字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public BearStateBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Normal.BearStateBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建CommonBaseCode类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         if (CurrentRow == null)
            return null;
         return new BasicDictionnary(CurrentRow);
      }
   }
}
