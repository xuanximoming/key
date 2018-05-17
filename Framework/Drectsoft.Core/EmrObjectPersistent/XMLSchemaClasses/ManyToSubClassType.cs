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
   /// 用子类来映射剩余的字段（以相同的方式定义子类映射关系，子类的不会单独使用）
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public partial class ManyToSubClassType : PropertyWithClassComplexType
   {
      /// <summary>
      /// 复制对象
      /// </summary>
      /// <returns></returns>
      public new ManyToSubClassType Clone()
      {
         ManyToSubClassType obj = new ManyToSubClassType();
         obj.Property = Property;
         obj.ClassName = ClassName;
         obj.KindColumn = KindColumn;
         return obj;
      }
   }
}
