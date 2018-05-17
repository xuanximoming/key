/*****************************************************************************\
**             Yindansoft & DrectSoft Software Co. Ltd.                          **
**                                                                           **
**             临床类别的字典类定义                                           **
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
   /// 局部解剖学
   /// </summary>
   public sealed class Topography : BaseWordbook
   {
      /// <summary>
      /// 创建 局部解剖学 字典对象
      /// </summary>
      public Topography()
         : base("Clinic.Topography")
      { }

      /// <summary>
      /// 创建 局部解剖学 字典对象
      /// </summary>
      public Topography(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Clinic.Topography", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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
   /// 形态学
   /// </summary>
   public sealed class Morphology : BaseWordbook
   {
      /// <summary>
      /// 创建 形态学 字典对象
      /// </summary>
      public Morphology()
         : base("Clinic.Morphology")
      { }

      /// <summary>
      /// 创建 形态学 字典对象
      /// </summary>
      public Morphology(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Clinic.Morphology", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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
   /// 生物功能
   /// </summary>
   public sealed class BiologyFunction : BaseWordbook
   {
      /// <summary>
      /// 创建 生物功能 字典对象
      /// </summary>
      public BiologyFunction()
         : base("Clinic.BiologyFunction")
      { }

      /// <summary>
      /// 创建 生物功能 字典对象
      /// </summary>
      public BiologyFunction(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Clinic.BiologyFunction", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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
   /// 活有机体
   /// </summary>
   public sealed class LivingOrganisms : BaseWordbook
   {
      /// <summary>
      /// 创建 活有机体 字典对象
      /// </summary>
      public LivingOrganisms()
         : base("Clinic.LivingOrganisms")
      { }

      /// <summary>
      /// 创建 活有机体 字典对象
      /// </summary>
      public LivingOrganisms(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Clinic.LivingOrganisms", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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
   /// 物理因素、力和活动
   /// </summary>
   public sealed class PhysicalAgents : BaseWordbook
   {
      /// <summary>
      /// 创建 物理因素、力和活动 字典对象
      /// </summary>
      public PhysicalAgents()
         : base("Clinic.PhysicalAgents")
      { }

      /// <summary>
      /// 创建 物理因素、力和活动 字典对象
      /// </summary>
      public PhysicalAgents(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Clinic.PhysicalAgents", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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
   /// 社会环境
   /// </summary>
   public sealed class SocialContext : BaseWordbook
   {
      /// <summary>
      /// 创建 社会环境 字典对象
      /// </summary>
      public SocialContext()
         : base("Clinic.SocialContext")
      { }

      /// <summary>
      /// 创建 社会环境 字典对象
      /// </summary>
      public SocialContext(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Clinic.SocialContext", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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
   /// 操作
   /// </summary>
   public sealed class Procedure : BaseWordbook
   {
      /// <summary>
      /// 创建 操作 字典对象
      /// </summary>
      public Procedure()
         : base("Clinic.Procedure")
      { }

      /// <summary>
      /// 创建 操作 字典对象
      /// </summary>
      public Procedure(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Clinic.Procedure", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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
   /// 关联词(修饰词)
   /// </summary>
   public sealed class GeneralLinkage : BaseWordbook
   {
      /// <summary>
      /// 创建 关联词 字典对象
      /// </summary>
      public GeneralLinkage()
         : base("Clinic.GeneralLinkage")
      { }

      /// <summary>
      /// 创建 关联词 字典对象
      /// </summary>
      public GeneralLinkage(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Clinic.GeneralLinkage", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
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
}
