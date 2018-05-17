using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Globalization;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 项目单位结构体
   /// </summary>
   public struct ItemUnit
   {
      #region properties
      /// <summary>
      /// 单位名称
      /// </summary>
      public string Name
      {
         get 
         {
            if (_name == null)
               return "";
            return _name; 
         }
      }
      private string _name;

      /// <summary>
      /// 单位系数(数量/此系数=相对于最小单位的数量)
      /// </summary>
      public decimal Quotiety
      {
         get { return _quotiety; }
      }
      private decimal _quotiety;

      /// <summary>
      /// 单位类别
      /// </summary>
      public DruggeryUnitKind Kind
      {
         get { return _kind; }
      }
      private DruggeryUnitKind _kind;

      /// <summary>
      /// 标记单位结构体是否被正确初始化
      /// </summary>
      public bool IsEmpty
      {
         get 
         {
            if ((_name == null) || (_name.Length == 0)
               || (_quotiety <= 0))
               return true;
            else
               return false;
         }
      }
      #endregion

      #region ctor
      /// <summary>
      /// 用传入的代码和名称初始化CodeItem
      /// </summary>
      /// <param name="name">单位名称</param>
      /// <param name="quotiety">单位系数(相对于最小单位)</param>
      /// <param name="kind">单位类别</param>
      public ItemUnit(string name, decimal quotiety, DruggeryUnitKind kind)
      {
         if (String.IsNullOrEmpty(name))
            name = "";
            // throw new ArgumentNullException("name", MessageStringManager.GetString("CommonParameterIsNull", "单位名称"));
         _name = name.TrimEnd();

         if (kind == DruggeryUnitKind.Specification) // 规格单位保存系数的倒数
            _quotiety = 1 / quotiety;
         else
            _quotiety = quotiety;

         _kind = kind;
      }

      /// <summary>
      /// 用传入的代码和名称对象初始化CodeItem
      /// </summary>
      /// <param name="name">单位名称</param>
      /// <param name="quotiety">单位系数(相对于最小单位)</param>
      /// <param name="kind">单位类型</param>
      [SpecialMethod(MethodSpecialKind.DefaultCtor)]
      public ItemUnit(object name, object quotiety, object kind)
      {
         if ((name == null) || (String.IsNullOrEmpty(_name = name.ToString().Trim())))
            name = "";

         if (kind == null)
            kind = DruggeryUnitKind.Min;

         _name = name.ToString().TrimEnd();

         _kind = (DruggeryUnitKind)Enum.Parse(typeof(DruggeryUnitKind), kind.ToString());
         _quotiety = Convert.ToDecimal(quotiety, CultureInfo.CurrentCulture);
         if (_kind == DruggeryUnitKind.Specification) // 规格单位保存系数的倒数
         {
            //hrr:解决某些情况下_quotiety为0时异常的问题
            if (_quotiety != 0)
               _quotiety = 1 / _quotiety;
            else
               _quotiety = decimal.MaxValue;
         }
      }

      /// <summary>
      /// 用传入的ItemUnit创建新的ItemUnit
      /// </summary>
      /// <param name="source"></param>
      public ItemUnit(ItemUnit source) : this(source.Name, source.Quotiety, source.Kind)
      { }
      #endregion

      #region public method
      /// <summary>
      /// 将以“当前单位”为单位的数量转换成以“最小单位”为单位的数量
      /// </summary>
      /// <param name="quantity">相对于当前单位的数量</param>
      /// <returns>相对于最小单位的数量</returns>
      public decimal Convert2BaseUnit(decimal quantity)
      { 
         return (quantity * _quotiety);
      }

      /// <summary>
      /// 将以“最小单位”为单位的数量转换成以“当前单位”为单位的数量
      /// </summary>
      /// <param name="quantity">相对于最小单位的数量</param>
      /// <returns>相对于当前单位的数量</returns>
      public decimal Convert2CurrentUnit(decimal quantity)
      {
         return (quantity / _quotiety);
      }

      /// <summary>
      /// 确定两个对象是否具有相同的值
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public override bool Equals(object obj)
      {
         if (obj is ItemUnit)
         {
            ItemUnit aimObj = (ItemUnit)obj;
            return (aimObj.Name == Name);
         }
         return false;
      }

      /// <summary>
      /// 返回该实例的哈希代码
      /// </summary>
      /// <returns></returns>
      public override int GetHashCode()
      {
         return Name.GetHashCode();
      }

      /// <summary>
      /// 获取对象的 Expression（如果存在的话）
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return Name;
         //return String.Format(CultureInfo.CurrentCulture
         //   , "{0}({1:D}, {2:#.##})", Name, Kind, Quotiety) ;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="unit1"></param>
      /// <param name="unit2"></param>
      /// <returns></returns>
      public static bool operator ==(ItemUnit unit1, ItemUnit unit2)
      {
         return unit1.Equals(unit2);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="unit1"></param>
      /// <param name="unit2"></param>
      /// <returns></returns>
      public static bool operator !=(ItemUnit unit1, ItemUnit unit2)
      {
         return !(unit1 == unit2);
      }

      #endregion
   }
}
