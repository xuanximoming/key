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
   /// 当前表中字段与关联表中字段的对应关系
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public partial class ColumnToColumn
   {
      private string _sourceColumn;
      private string _targetColumn;
      private string _defaultValue;

      /// <summary>
      /// 当前表中列名
      /// </summary>
      [XmlAttributeAttribute()]
      public string SourceColumn
      {
         get
         {
            return _sourceColumn;
         }
         set
         {
            _sourceColumn = value;
         }
      }

      /// <summary>
      /// 关联表中原始列名
      /// </summary>
      [XmlAttributeAttribute()]
      public string TargetColumn
      {
         get
         {
            return _targetColumn;
         }
         set
         {
            _targetColumn = value;
         }
      }

      /// <summary>
      /// 关联表中列的缺省值(当前表中列名为空时有效)
      /// </summary>
      [XmlAttributeAttribute()]
      public string DefaultValue
      {
         get
         {
            if (_defaultValue == null)
               _defaultValue = "";
            return _defaultValue;
         }
         set
         {
            _defaultValue = value;
         }
      }

      /// <summary>
      /// 复制对象
      /// </summary>
      /// <returns></returns>
      public ColumnToColumn Clone()
      {
         ColumnToColumn obj = new ColumnToColumn();
         obj.SourceColumn = SourceColumn;
         obj.TargetColumn = TargetColumn;
         obj.DefaultValue = DefaultValue;
         return obj;
      }
   }
}
