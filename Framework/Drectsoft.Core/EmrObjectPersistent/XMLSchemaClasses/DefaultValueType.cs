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
   /// 使用工厂类创建实例时对于具体的实例不一定会用到所有的列，所以要给列指定默认值
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(Namespace = "http://www.DrectSoft.com.cn/orm")]
   public partial class DefaultValueType
   {
      private string _column;

      private string _value;

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
      /// 默认值
      /// </summary>
      [XmlAttributeAttribute()]
      public string Value
      {
         get
         {
            return _value;
         }
         set
         {
            _value = value;
         }
      }

      /// <summary>
      /// 复制对象
      /// </summary>
      /// <returns></returns>
      public DefaultValueType Clone()
      {
         DefaultValueType obj = new DefaultValueType();
         obj.Column = Column;
         obj.Value = Value;
         return obj;
      }
   }
}
