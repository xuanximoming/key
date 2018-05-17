/*****************************************************************************\
**                                **
**                                                                           **
**             定义字典类需要使用的枚举型定义                                  **
**                                                                           **
**                                                                           **
\*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Xml.Serialization;

namespace DrectSoft.Wordbook
{
   #region enum type of string's characters
   /// <summary>
   /// 字符串中字符类型枚举
   /// </summary>
   [CLSCompliantAttribute(true)]
   public enum StringType
   {
      /// <summary>
      /// 空串
      /// </summary>
      Empty,
      /// <summary>
      /// 全部由英文字母
      /// </summary>
      EnglishChar,
      /// <summary>
      /// 全部由数字组成
      /// </summary>
      Numeric,
      /// <summary>
      /// 由ASCII中的字母和数组组成
      /// </summary>
      Char,
      /// <summary>
      /// 包含汉字、全角字母或其它符号
      /// </summary>
      Other
   }
   #endregion

   #region Wordbook Kind
   /// <summary>
   /// 字典的类型
   /// </summary>
   [CLSCompliantAttribute(true)]
   public enum WordbookKind
   {
      /// <summary>
      /// 普通，预定义的字典类
      /// </summary>
      Normal,
      /// <summary>
      /// 根据SQL语句生成的字典类
      /// </summary>
      Sql,
      /// <summary>
      /// 根据StringList生成的字典类
      /// </summary>
      List
   }
   #endregion

   #region CompareOperator
   /// <summary>
   /// 比较运算符名称枚举类
   /// </summary>
   [SerializableAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   [CLSCompliantAttribute(true)]
   public enum CompareOperator
   { 
      /// <summary>
      /// &lt;
      /// </summary>
      Less, 
      /// <summary>
      /// &gt;
      /// </summary>
      More, 
      /// <summary>
      /// &lt;=
      /// </summary>
      NotMore,
      /// <summary>
      /// &gt;=
      /// </summary>
      NotLess,
      /// <summary>
      /// &lt;&gt;
      /// </summary>
      NotEqual,
      /// <summary>
      /// =
      /// </summary>
      Equal,
      /// <summary>
      /// IN
      /// </summary>
      In,
      /// <summary>
      /// LIKE
      /// </summary>
      Like
   }
   #endregion
}
