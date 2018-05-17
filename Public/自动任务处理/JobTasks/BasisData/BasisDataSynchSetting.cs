using System.Xml.Serialization;
using System.Diagnostics;
using System.ComponentModel;
using System;

namespace DrectSoft.JobManager
{
   /// <remarks/>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [DesignerCategoryAttribute("code")]
   [XmlTypeAttribute(AnonymousType = true)]
   [XmlRootAttribute(Namespace = "http://www.DrectSoft.com/", IsNullable = false)]
   public class BasisDataSynchSetting
   {
      private TableMapping[] tableMappingField;

      /// <summary>
      /// 数据映射设置
      /// </summary>
      [XmlElementAttribute("TableMapping")]
      public TableMapping[] TableMappings
      {
         get
         {
            return this.tableMappingField;
         }
         set
         {
            this.tableMappingField = value;
         }
      }
   }

   /// <summary>
   /// 数据映射设置
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [DesignerCategoryAttribute("code")]
   [XmlTypeAttribute(AnonymousType = true)]
   public class TableMapping
   {

      /// <summary>
      /// 需要在目标数据库预执行的语句
      /// </summary>
      public string PreHandleSentence
      {
         get { return preHandleSentence; }
         set { preHandleSentence = value; }
      }
      private string preHandleSentence;

      /// <summary>
      /// 数据来源设置
      /// </summary>
      [XmlElement("DataSource")]
      public TableMappingDataSource[] DataSources
      {
         get
         {
            return this.dataSourceField;
         }
         set
         {
            this.dataSourceField = value;
         }
      }
      private TableMappingDataSource[] dataSourceField;

      /// <summary>
      /// 公共的从源库筛选数据的SQL语句
      /// </summary>
      public string SelectSentence
      {
         get
         {
            return this.selectSentenceField;
         }
         set
         {
            this.selectSentenceField = value;
         }
      }
      private string selectSentenceField;

      /// <summary>
      /// 公共的需要在目标库另外执行的处理语句
      /// </summary>
      public string OtherSentence
      {
         get
         {
            return this.otherSentenceField;
         }
         set
         {
            this.otherSentenceField = value;
         }
      }
      private string otherSentenceField;

      /// <summary>
      /// 目标库中的表名
      /// </summary>
      [XmlAttribute()]
      public string TargetTable
      {
         get
         {
            return this.targetTableField;
         }
         set
         {
            this.targetTableField = value;
         }
      }
      private string targetTableField;

      /// <summary>
      /// 是否启用
      /// </summary>
      [XmlAttribute()]
      public bool Enabled
      {
         get { return enabledField; }
         set { enabledField = value; }
      }
      private bool enabledField;

      /// <summary>
      /// 标记本次是否需要执行
      /// </summary>
      [XmlIgnore()]
      public bool NeedRunNow
      {
         get { return Enabled && _needRunNow; }
         set { _needRunNow = value; }
      }
      private bool _needRunNow;

      /// <summary>
      /// 名称字段
      /// </summary>
      [XmlAttribute()]
      public string NameField
      {
         get { return nameFieldField; }
         set { nameFieldField = value; }
      }
      private string nameFieldField;
   }

   /// <summary>
   /// 数据来源设置
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [DesignerCategoryAttribute("code")]
   [XmlTypeAttribute(AnonymousType = true)]
   public class TableMappingDataSource
   {

      /// <summary>
      /// 需要在目标数据库预执行的语句
      /// </summary>
      public string PreHandleSentence
      {
         get { return preHandleSentence; }
         set { preHandleSentence = value; }
      }
      private string preHandleSentence;

      /// <summary>
      /// 从源库筛选数据的SQL语句(为空时使用公共设置)
      /// </summary>
      public string SelectSentence
      {
         get { return selectSentenceField; }
         set { selectSentenceField = value; }
      }
      private string selectSentenceField;

      /// <summary>
      /// 源库中的表名
      /// </summary>
      [XmlAttribute()]
      public string SourceTable
      {
         get { return sourceTableField; }
         set { sourceTableField = value; }
      }
      private string sourceTableField;

      /// <summary>
      /// 是否启用
      /// </summary>
      public bool Enabled
      {
         get { return enabled; }
         set { enabled = value; }
      }
      private bool enabled;

      /// <summary>
      /// 对源表数据进行过滤的条件
      /// </summary>
      public string FilteCondition
      {
         get { return filteCondition; }
         set { filteCondition = value; }
      }
      private string filteCondition;
   }
}