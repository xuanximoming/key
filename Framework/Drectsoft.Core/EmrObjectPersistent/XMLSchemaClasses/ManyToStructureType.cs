using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Text;
using System.Collections.ObjectModel;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 一组字段映射到结构体
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public partial class ManyToStructureType : PropertyWithClassType
   {
      private Collection<StructPropertyMap> _propertyToColumns;

      /// <remarks/>
      [XmlElementAttribute("PropertyToColumn")]
      public Collection<StructPropertyMap> PropertyToColumn
      {
         get
         {
            return _propertyToColumns;
         }
         set
         {
            _propertyToColumns = value;
         }
      }

      /// <summary>
      /// 复制对象
      /// </summary>
      /// <returns></returns>
      public new ManyToStructureType Clone()
      {
         ManyToStructureType obj = new ManyToStructureType();
         obj.Property = Property;
         obj.ClassName = ClassName;
         obj.PropertyToColumn = new Collection<StructPropertyMap>();
         foreach (StructPropertyMap map in PropertyToColumn)
            obj.PropertyToColumn.Add(map.Clone());
         return obj;
      }
   }
}
