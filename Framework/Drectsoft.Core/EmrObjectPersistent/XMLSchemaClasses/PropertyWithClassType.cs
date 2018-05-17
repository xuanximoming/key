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
   /// 属性和其类型名值对，在设置复杂的对应关系时需要
   /// </summary>
   [XmlIncludeAttribute(typeof(PropertyWithClassComplexType))]
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(Namespace = "http://www.DrectSoft.com.cn/orm")]
   public partial class PropertyWithClassType
   {
      private string _property;

      private string _className;

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
      /// 属性类型名称
      /// </summary>
      [XmlAttributeAttribute("Class")]
      public string ClassName
      {
         get
         {
            return _className;
         }
         set
         {
            _className = value;
         }
      }

      /// <summary>
      /// 复制对象
      /// </summary>
      /// <returns></returns>
      public PropertyWithClassType Clone()
      {
         PropertyWithClassType obj = new PropertyWithClassType();
         obj.Property = Property;
         obj.ClassName = ClassName;
         return obj;
      }
   }
}
