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
   /// 表和对象映射关系
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public partial class ORMapping
   {
      private Collection<OneToOneType> _oneOnes;

      private Collection<OneToStateType> _attributes;

      private Collection<ManyToStructureType> _structures;

      private Collection<ManyToObjectClassType> _objectClasses;

      private Collection<ManyToSubClassType> _subClasses;

      private Collection<DefaultValueType> _defaultValues;

      private string _className;

      private string _parentClass;

      private string _tableName;

      /// <summary>
      /// 
      /// </summary>
      [XmlElementAttribute("One-One")]
      public Collection<OneToOneType> OneOnes
      {
         get
         {
            return _oneOnes;
         }
         set
         {
            _oneOnes = value;
         }
      }

      /// <summary>
      /// 
      /// </summary>
      [XmlElementAttribute("State")]
      public Collection<OneToStateType> States
      {
         get
         {
            return _attributes;
         }
         set
         {
            _attributes = value;
         }
      }

      /// <summary>
      /// 
      /// </summary>
      [XmlElementAttribute("Structure")]
      public Collection<ManyToStructureType> Structures
      {
         get
         {
            return _structures;
         }
         set
         {
            _structures = value;
         }
      }

      /// <summary>
      /// 
      /// </summary>
      [XmlElementAttribute("ObjectClass")]
      public Collection<ManyToObjectClassType> ObjectClasses
      {
         get
         {
            return _objectClasses;
         }
         set
         {
            _objectClasses = value;
         }
      }

      /// <summary>
      /// 
      /// </summary>
      [XmlElementAttribute("SubClass")]
      public Collection<ManyToSubClassType> SubClasses
      {
         get
         {
            return _subClasses;
         }
         set
         {
            _subClasses = value;
         }
      }

      /// <remarks/>
      [XmlElementAttribute("DefaultValue")]
      public Collection<DefaultValueType> DefaultValues
      {
         get
         {
            return _defaultValues;
         }
         set
         {
            _defaultValues = value;
         }
      }
      
      /// <summary>
      /// 类名
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
      /// 父类名
      /// </summary>
      [XmlAttributeAttribute("ParentClass")]
      public string ParentClass
      {
         get
         {
            return _parentClass;
         }
         set
         {
            _parentClass = value;
         }
      }

      /// <summary>
      /// 对应的表名
      /// </summary>
      [XmlAttributeAttribute("Table")]
      public string TableName
      {
         get
         {
            return _tableName;
         }
         set
         {
            _tableName = value;
         }
      }
   }
}
