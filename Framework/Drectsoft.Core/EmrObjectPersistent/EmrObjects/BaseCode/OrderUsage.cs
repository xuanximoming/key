using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Globalization;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 医嘱用法类
   /// </summary>
   public class OrderUsage : EPBaseObject
   {
      #region properties
      /// <summary>
      /// 只用于成套医嘱
      /// </summary>
      public bool SuitOnly
      {
         get { return _suitOnly; }
         set { _suitOnly = value; }
      }
      private bool _suitOnly;

      /// <summary>
      /// 自动分组
      /// </summary>
      public bool AutoGroup
      {
         get { return _autoGroup; }
         set { _autoGroup = value; }
      }
      private bool _autoGroup;

      /// <summary>
      /// 用法类型
      /// </summary>
      public DragUsageKind Kind
      {
         get { return _kind; }
         set { _kind = value; }
      }
      private DragUsageKind _kind;

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return GetQuerySentenceFromXml("SelectOrderUsageBook"); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
         get { return FormatFilterString("ID", Code); }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public OrderUsage()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      public OrderUsage(string code)
         : base(code)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public OrderUsage(string code, string name)
         : base(code, name)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public OrderUsage(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      #region public methods
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public new OrderUsage Clone()
      {
         return (OrderUsage)(base.Clone());
      }

      /// <summary>
      /// 确定两个对象是否具有相同的值
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public override bool Equals(object obj)
      {
         if (obj == null)
            return false;
         OrderUsage aimObj = (OrderUsage)obj;

         if (aimObj != null)
         {
            return (aimObj.Code == Code);
         }
         return false;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public override int GetHashCode()
      {
         return base.GetHashCode();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         if (String.IsNullOrEmpty(Code))
            return "";

         if (String.IsNullOrEmpty(Name))
            ReInitializeProperties();

         if (String.IsNullOrEmpty(Name))
            return String.Format(CultureInfo.CurrentCulture, "[{0}]", Code);
         else 
            return String.Format(CultureInfo.CurrentCulture, "{0}", Name.Trim());
      }

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         ReInitializeProperties();
      }
      #endregion
   }
}
