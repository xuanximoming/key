using System.Xml.Serialization;
using System;
using System.Diagnostics;
using DrectSoft.Wordbook;


namespace DrectSoft.Wordbook.Schema
{
   /// <summary>
   /// 包含所有的字典类定义
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   [XmlRootAttribute(Namespace = "http://www.DrectSoft.com.cn/Wordbook", IsNullable = false)]
   public class Wordbooks
   {
      /// <summary>
      /// 字典类分类数组
      /// </summary>
      [XmlElementAttribute("WordbookCatalog")]
      public WordbookCatalog[] Catalogs
      {
         get { return _catalogs; }
         set { _catalogs = value; }
      }
      private WordbookCatalog[] _catalogs;
   }
}
