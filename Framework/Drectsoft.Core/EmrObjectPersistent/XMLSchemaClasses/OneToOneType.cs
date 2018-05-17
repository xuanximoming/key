using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Text;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 属性和字段直接对应
   /// </summary>
   [XmlIncludeAttribute(typeof(OneToStateType))]
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(Namespace = "http://www.DrectSoft.com.cn/orm")]
   public partial class OneToOneType
   {
      private string _property;

      private string _column;

      /// <summary>
      /// 属性名
      /// </summary>
      [XmlAttributeAttribute()]
      public string Property
      {
         get
         {
            return _property;
         }
         set
         {
            _property = value;
         }
      }

      /// <summary>
      /// 列名
      /// </summary>
      [XmlAttributeAttribute()]
      public string Column
      {
         get
         {
            return _column;
         }
         set
         {
            _column = value;
         }
      }

      /// <summary>
      /// 复制对象
      /// </summary>
      /// <returns></returns>
      public OneToOneType Clone()
      {
         OneToOneType obj = new OneToOneType();
         obj.Property = Property;
         obj.Column = Column;
         return obj;
      }
   }
}

