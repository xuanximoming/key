using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DrectSoft.Wordbook.Schema
{
   /// <summary>
   /// 字典类定义(序列化用，需要手工转换到BaseWordbook)
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public class Wordbook
   {
      private string _querySentence;
      private string _codeField;
      private string _nameField;
      private string _queryCodeField;
      private bool _codeFieldIsString;
      private string[] _filterFields;
      private FilterParameter[] _filterParameters;
      private GridColumnStyle[][] _viewStyles;
      private string _wordbookName;
      private string _caption;

      /// <remarks/>
      public string QuerySentence
      {
         get { return _querySentence; }
         set { _querySentence = value; }
      }

      /// <remarks/>
      public string CodeField
      {
         get { return _codeField; }
         set { _codeField = value; }
      }

      /// <remarks/>
      public string NameField
      {
         get { return _nameField; }
         set { _nameField = value; }
      }

      /// <remarks/>
      public string QueryCodeField
      {
         get { return _queryCodeField; }
         set { _queryCodeField = value; }
      }

      /// <remarks/>
      public bool CodeFieldIsString
      {
         get { return _codeFieldIsString; }
         set { _codeFieldIsString = value; }
      }

      /// <remarks/>
      [XmlArrayItemAttribute("FilterField", IsNullable = false)]
      public string[] FilterFieldCollection
      {
         get { return _filterFields; }
         set { _filterFields = value; }
      }

      /// <remarks/>
      [XmlArrayItemAttribute("Parameter", IsNullable = false)]
      public FilterParameter[] ParameterCollection
      {
         get { return _filterParameters; }
         set { _filterParameters = value; }
      }

      /// <remarks/>
      [XmlArrayItemAttribute("ColumnStyleCollection", IsNullable = false)]
      [XmlArrayItemAttribute("ColumnStyle", IsNullable = false, NestingLevel = 1)]
      public GridColumnStyle[][] ViewStyleCollection
      {
         get { return _viewStyles; }
         set { _viewStyles = value; }
      }

      /// <remarks/>
      [XmlAttributeAttribute()]
      public string WordbookName
      {
         get { return _wordbookName; }
         set { _wordbookName = value; }
      }

      /// <remarks/>
      [XmlAttributeAttribute()]
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }
   }
}
