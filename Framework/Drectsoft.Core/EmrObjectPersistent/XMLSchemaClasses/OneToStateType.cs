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
   /// 对用状态位保存数据的字段特殊处理
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(Namespace = "http://www.DrectSoft.com.cn/orm")]
   public partial class OneToStateType : OneToOneType
   {
      private string _className;

      /// <summary>
      /// Attribute类名
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
      public new OneToStateType Clone()
      {
         OneToStateType obj = new OneToStateType();
         obj.Property = Property;
         obj.Column = Column;
         obj.ClassName = ClassName;
         return obj;
      }
   }
}
