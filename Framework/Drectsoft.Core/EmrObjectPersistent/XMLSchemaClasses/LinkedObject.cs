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
   /// 需要为每一种可能出现的相关类指定列映射关系
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public partial class LinkedObject
   {
      private Collection<ColumnToColumn> _columnToColumns;

      private string _className;

      /// <remarks/>
      [System.Xml.Serialization.XmlElementAttribute("ColumnToColumn")]
      public Collection<ColumnToColumn> ColumnToColumns
      {
         get
         {
            return _columnToColumns;
         }
         set
         {
            _columnToColumns = value;
         }
      }

      /// <remarks/>
      [System.Xml.Serialization.XmlAttributeAttribute("Class")]
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
      public LinkedObject Clone()
      {
         LinkedObject obj = new LinkedObject();
         obj.ClassName = ClassName;
         obj.ColumnToColumns = new Collection<ColumnToColumn>();
         foreach (ColumnToColumn colCol in ColumnToColumns)
            obj.ColumnToColumns.Add(colCol.Clone());
         return obj;
      }
   }
}
