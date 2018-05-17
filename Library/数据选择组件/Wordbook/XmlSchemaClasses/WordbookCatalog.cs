using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DrectSoft.Wordbook.Schema
{
   /// <summary>
   /// 字典类分组
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public class WordbookCatalog
   {
      /// <summary>
      /// 字典类数组
      /// </summary>
      [XmlElementAttribute("Wordbook")]
      public Wordbook[] Wordbooks
      {
         get { return _wordbooks; }
         set { _wordbooks = value; }
      }
      private Wordbook[] _wordbooks;

      /// <summary>
      /// 分类名称
      /// </summary>
      [XmlAttributeAttribute()]
      public string CatalogName
      {
         get { return _catalogName; }
         set { _catalogName = value; }
      }
      private string _catalogName;

      /// <summary>
      /// 分类显示名称
      /// </summary>
      [XmlAttributeAttribute()]
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }
      private string _caption;
   }
}
