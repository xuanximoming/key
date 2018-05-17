/*****************************************************************************\
**             Yindansoft & DrectSoft Software Co. Ltd.                          **
**                                                                           **
**             医院各种设置的字典类定义                                       **
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
   /// 院内科室字典
   /// </summary>
   public sealed class DepartmentBook : BaseWordbook
   {
      /// <summary>
      /// 创建院内科室字典对象
      /// </summary>
      public DepartmentBook()
         : base("Hospital.DepartmentBook")
      { }

      /// <summary>
      /// 创建院内科室字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public DepartmentBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.DepartmentBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 Department 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new Department(CurrentRow);
      }
   }

   /// <summary>
   /// 病区字典
   /// </summary>
   public sealed class WardBook : BaseWordbook
   {
      /// <summary>
      /// 创建病区字典对象
      /// </summary>
      public WardBook()
         : base("Hospital.WardBook")
      { }

      /// <summary>
      /// 创建病区字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public WardBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.WardBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 Ward 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new Ward(CurrentRow);
      }
   }

   /// <summary>
   /// 手术室
   /// </summary>
   public sealed class OpDepartmentBook : BaseWordbook
   {
      /// <summary>
      /// 创建手术室字典对象
      /// </summary>
      public OpDepartmentBook()
         : base("Hospital.OpDepartmentBook")
      { }

      /// <summary>
      /// 创建手术室字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public OpDepartmentBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.OpDepartmentBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 Department 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new Department(CurrentRow);
      }
   }

   /// <summary>
   /// 院内手术操作字典
   /// </summary>
   public sealed class CustomOperationBook : BaseWordbook
   {
      /// <summary>
      /// 创建 院内手术操作 字典对象
      /// </summary>
      public CustomOperationBook()
         : base("Hospital.CustomOperationBook")
      { }

      /// <summary>
      /// 创建 院内手术操作 字典对象
      /// </summary>
      public CustomOperationBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.CustomOperationBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 院内手术操作 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
         throw new NotImplementedException();
      }
   }

   /// <summary>
   /// 职工表
   /// </summary>
   public sealed class EmployeeBook : BaseWordbook
   {
      /// <summary>
      /// 创建职工表字典对象
      /// </summary>
      public EmployeeBook()
         : base("Hospital.EmployeeBook")
      { }

      /// <summary>
      /// 创建职工表字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public EmployeeBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.EmployeeBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 Employee 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new Employee(CurrentRow);
      }
   }

   /// <summary>
   /// 收费项目
   /// </summary>
   public sealed class ChargeItemBook : BaseWordbook
   {
      /// <summary>
      /// 创建收费项目字典对象
      /// </summary>
      public ChargeItemBook()
         : base("Hospital.ChargeItemBook")
      { }

      /// <summary>
      /// 创建收费项目字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public ChargeItemBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.ChargeItemBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 OrderItem 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          //return OrderItemFactory.CreateOrderItem(CurrentRow, null);
          return new ChargeItem(CurrentRow); ;
      }
   }

   /// <summary>
   /// 临床收费项目
   /// </summary>
   public sealed class ClinicItemBook : BaseWordbook
   {
      /// <summary>
      /// 创建临床收费项目字典对象
      /// </summary>
      public ClinicItemBook()
         : base("Hospital.ClinicItemBook")
      { }

      /// <summary>
      /// 创建临床收费项目字典对象
      /// </summary>
      /// <param name="filters"></param>
      /// <param name="gridStyleIndex"></param>
      /// <param name="filterComb"></param>
      /// <param name="extraCondition"></param>
      /// <param name="cacheTime"></param>
      public ClinicItemBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.ClinicItemBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 OrderItem 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          //return OrderItemFactory.CreateOrderItem(CurrentRow, null);
          return new ClinicItem(CurrentRow);
      }
   }

   /// <summary>
   /// 药品字典
   /// </summary>
   public sealed class DruggeryBook : BaseWordbook
   {
      /// <summary>
      /// 创建 药品字典 字典对象
      /// </summary>
      public DruggeryBook()
         : base("Hospital.DruggeryBook")
      { }

      /// <summary>
      /// 创建 药品字典 字典对象
      /// </summary>
      public DruggeryBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.DruggeryBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 Druggery 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new Druggery(CurrentRow);
      }
   }

   /// <summary>
   /// 药品分类
   /// </summary>
   public sealed class DruggeryCatalogBook : BaseWordbook
   {
      /// <summary>
      /// 创建 药品分类 字典对象
      /// </summary>
      public DruggeryCatalogBook()
         : base("Hospital.DruggeryCatalogBook")
      { }

      /// <summary>
      /// 创建 药品分类 字典对象
      /// </summary>
      public DruggeryCatalogBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.DruggeryCatalogBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 DruggeryCatalog 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new DruggeryCatalog(CurrentRow);
      }
   }

   /// <summary>
   /// 药品剂型
   /// </summary>
   public sealed class DruggeryFormBook : BaseWordbook
   {
      /// <summary>
      /// 创建 药品剂型 字典对象
      /// </summary>
      public DruggeryFormBook()
         : base("Hospital.DruggeryFormBook")
      { }

      /// <summary>
      /// 创建 药品剂型 字典对象
      /// </summary>
      public DruggeryFormBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.DruggeryFormBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 DruggeryForm 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new DruggeryForm(CurrentRow);
      }
   }

   /// <summary>
   /// 药品来源
   /// </summary>
   public sealed class DruggerySourceBook : BaseWordbook
   {
      /// <summary>
      /// 创建 药品来源 字典对象
      /// </summary>
      public DruggerySourceBook()
         : base("Hospital.DruggerySourceBook")
      { }

      /// <summary>
      /// 创建 药品来源 字典对象
      /// </summary>
      public DruggerySourceBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.DruggerySourceBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 DruggerySource 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new DruggerySource(CurrentRow);
      }
   }

   /// <summary>
   /// 医嘱用法
   /// </summary>
   public sealed class OrderUsageBook : BaseWordbook
   {
      /// <summary>
      /// 创建 医嘱用法 字典对象
      /// </summary>
      public OrderUsageBook()
         : base("Hospital.OrderUsageBook")
      { }

      /// <summary>
      /// 创建 医嘱用法 字典对象
      /// </summary>
      public OrderUsageBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.OrderUsageBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 OrderUsage 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new OrderUsage(CurrentRow);
      }
   }

   /// <summary>
   /// 医嘱频次
   /// </summary>
   public sealed class OrderFrequencyBook : BaseWordbook
   {
      /// <summary>
      /// 创建 医嘱频次 字典对象
      /// </summary>
      public OrderFrequencyBook()
         : base("Hospital.OrderFrequencyBook")
      { }

      /// <summary>
      /// 创建 医嘱频次 字典对象
      /// </summary>
      public OrderFrequencyBook(string filters, int gridStyleIndex, string filterComb, string extraCondition, int cacheTime)
         : base("Hospital.OrderFrequencyBook", filters, gridStyleIndex, filterComb, extraCondition, cacheTime)
      { }

      /// <summary>
      /// 用字典实例创建 OrderFrequency 类型持久对象
      /// </summary>
      /// <returns></returns>
      protected override EPBaseObject CreatePersistentWordbook()
      {
          if (CurrentRow == null)
              return null;
          return new OrderFrequency(CurrentRow);
      }
   }
}
