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
   /// 预定义选择语句集合
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   [XmlRootAttribute(Namespace = "http://www.DrectSoft.com.cn/orm", IsNullable = false)]
   public partial class PreDefineSqlCollection
   {
      private Collection<SelectSentence> _sentences;

      /// <summary>
      /// 预定义选择语句集合
      /// </summary>
      [XmlElementAttribute("SelectSentence")]
      public Collection<SelectSentence> Sentences
      {
         get
         {
            return _sentences;
         }
         set
         {
            _sentences = value;
         }
      }
   }

   /// <summary>
   /// 预定义选择语句
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public partial class SelectSentence
   {
      private string _querySentence;
      private string _name;

      /// <summary>
      /// 语句标识符
      /// </summary>
      [XmlAttributeAttribute("Name")]
      public string Name
      {
         get
         {
            return _name;
         }
         set
         {
            _name = value;
         }
      }

      /// <summary>
      /// 不带参数的查询语句
      /// </summary>
      [XmlElementAttribute("QuerySentence")]
      public string QuerySentence
      {
         get
         {
            return _querySentence;
         }
         set
         {
            _querySentence = value;
         }
      }
   }
}
