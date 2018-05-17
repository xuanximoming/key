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
   /// 属性和其类型名值对，以及类型为抽象类时需要的信息
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(Namespace = "http://www.DrectSoft.com.cn/orm")]
   public partial class PropertyWithClassComplexType : PropertyWithClassType
   {
      private string _kindColumn;

      /// <summary>
      /// 表示类别的列名（如属性的类型是抽象类时，将根据类别使用工厂类创建实例；其余情况可以不填）
      /// </summary>
      [XmlAttributeAttribute()]
      public string KindColumn
      {
         get
         {
            return _kindColumn;
         }
         set
         {
            _kindColumn = value;
         }
      }

      /// <summary>
      /// 复制对象
      /// </summary>
      /// <returns></returns>
      public new PropertyWithClassComplexType Clone()
      {
         PropertyWithClassComplexType obj = new PropertyWithClassComplexType();
         obj.Property = Property;
         obj.ClassName = ClassName;
         obj.KindColumn = KindColumn;
         return obj;
      }
   }
}

