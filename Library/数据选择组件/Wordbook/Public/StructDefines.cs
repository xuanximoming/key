/*****************************************************************************\
**             Yindansoft & DrectSoft Software Co. Ltd.                          **
**                                                                           **
**             定义字典类需要使用的结构体定义                                  **
**                                                                           **
**                                                                           **
\*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace DrectSoft.Wordbook
{
   #region SingleCondition
   ///// <summary>
   ///// CustomDraw时使用的单个判断条件结构体
   ///// </summary>
   //public struct SingleCondition
   //{
   //   /// <summary>
   //   /// 列名(数据集的原始列名)
   //   /// </summary>
   //   public string ColumnName
   //   {
   //      get { return columnName; }
   //   }
   //   private string columnName;

   //   /// <summary>
   //   /// 用来比较的值(统一以字符串进行比较)
   //   /// </summary>
   //   public string Value
   //   {
   //      get { return this.value; }
   //   }
   //   private string value;

   //   /// <summary>
   //   /// 创建CustomDraw时使用的单个判断条件结构体。
   //   /// 现在没有对传入的值进行校验,请自行保证逻辑正确！！！
   //   /// </summary>
   //   /// <param name="column">数据集的原始列名</param>
   //   /// <param name="equalValue">用来比较的值</param>
   //   public SingleCondition(string column, string equalValue)
   //   {
   //      columnName = column;
   //      value = equalValue;
   //   }
   //}
   #endregion

   #region CustomDrawSetting
   ///// <summary>
   ///// CustomDraw设置信息的结构体
   ///// </summary>
   //public struct CustomDrawSetting
   //{
   //   /// <summary>
   //   /// CustomDraw需要满足的条件
   //   /// </summary>
   //   public Collection<SingleCondition> Conditions
   //   {
   //      get { return conditions; }
   //      //set
   //      //{
   //      //   conditions.Clear();
   //      //   for (int i = 0; i < value.Count; i++)
   //      //   {
   //      //      conditions.Add(value[i]);
   //      //   }
   //      //}
   //   }
   //   private Collection<SingleCondition> conditions;

   //   /// <summary>
   //   /// CustomDraw的前景色
   //   /// </summary>
   //   public Color ForeColor
   //   {
   //      get { return foreColor; }
   //      //set { foreColor = value; }
   //   }
   //   private Color foreColor;

   //   /// <summary>
   //   /// CustomDraw的背景色
   //   /// </summary>
   //   public Color BackColor
   //   {
   //      get { return backColor; }
   //      //set { backColor = value; }
   //   }
   //   private Color backColor;

   //   public CustomDrawSetting(Collection<SingleCondition> drawCondition, Color fontColor, Color cellBackColor)
   //   {
   //      conditions = new Collection<SingleCondition>();
   //      for (int i = 0; i < drawCondition.Count; i++)
   //      {
   //         conditions.Add(drawCondition[i]);
   //      }
   //      foreColor = fontColor;//SystemColors.ControlText;
   //      backColor = cellBackColor;// SystemColors.ControlLightLight;
   //   }
   //}
   #endregion

   #region WordbookInfo
   /// <summary>
   /// 字典信息的结构体
   /// </summary>
   public struct WordbookInfo
   {
      /// <summary>
      /// 所属分类名称
      /// </summary>
      public string Catalog
      {
         get { return _catalog; }
      }
      private string _catalog;

      /// <summary>
      /// 字典名称（显示名称）
      /// </summary>
      public string Name
      {
         get { return _name; }
      }
      private string _name;

      /// <summary>
      /// 对应字典类型的名称
      /// </summary>
      public string TypeName
      {
         get { return _typeName; }
      }
      private string _typeName;

      /// <summary>
      /// 用传入的信息创建一个字典信息结构体
      /// </summary>
      /// <param name="catalog">所属分类名称</param>
      /// <param name="name">字典名称</param>
      /// <param name="typeName">字典类型的名称</param>
      public WordbookInfo(string catalog, string name, string typeName)
      {
         _catalog = catalog;
         _name = name;
         _typeName = typeName;
      }

      /// <summary>
      /// 确定两个字典信息结构体是否具有相同的值
      /// </summary>
      /// <param name="obj">与当前对象比较的结构体</param>
      /// <returns></returns>
      public override bool Equals(object obj)
      {
         if (obj == null)
            return false;
         WordbookInfo p = (WordbookInfo)obj;
         if ((object)p == null)
            return false;
         // Return true if the fields match:
         return (Catalog == p.Catalog)
            && (Name == p.Name)
            && (TypeName == p.TypeName);
      }

      /// <summary>
      /// 返回该实例的哈希代码
      /// </summary>
      /// <returns></returns>
      public override int GetHashCode()
      {
         return TypeName.GetHashCode();
      }

      /// <summary>
      /// 重载运算符"=="
      /// </summary>
      /// <param name="para1"></param>
      /// <param name="para2"></param>
      /// <returns></returns>
      public static bool operator ==(WordbookInfo para1, WordbookInfo para2)
      {
         // If both are null, or both are same instance, return true.
         if (Object.ReferenceEquals(para1, para2))
            return true;
         // If one is null, but not both, return false.
         else if (((object)para1 == null) || ((object)para2 == null))
            return false;
         // Otherwise, compare values and return:
         else
         {
            return (para1.Catalog == para2.Catalog)
            && (para1.Name == para2.Name)
            && (para1.TypeName == para2.TypeName);
         }
      }

      /// <summary>
      /// 重载运算符"!="
      /// </summary>
      /// <param name="para1"></param>
      /// <param name="para2"></param>
      /// <returns></returns>
      public static bool operator !=(WordbookInfo para1, WordbookInfo para2)
      {
         return !(para1 == para2);
      }
   }
   #endregion
}
