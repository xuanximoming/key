using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using System.Globalization;
using System.ComponentModel;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// EmrPersistentBaseObject, 基础持久类
   /// TODO: 需要再增加接口类,以优化程序
   /// </summary>
   public abstract class EPBaseObject : ISupportInitialize
   {
      #region properties
      /// <summary>
      /// 代码
      /// </summary>
      [Browsable(false), DisplayName("代码")]
      public string Code
      {
         get { return _code; }
         set
         {
            if (value == null)
               _code = String.Empty;
            else
               _code = value;
            Name = "";
         }
      }
      private string _code;

      /// <summary>
      /// 名称
      /// </summary>
      [DisplayName("名称")]
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }
      private string _name;

      /// <summary>
      /// 是否有效
      /// </summary>
      [Browsable(false)]
      public bool Enabled
      {
         get { return _enabled; }
         set { _enabled = value; }
      }
      private bool _enabled;

      /// <summary>
      /// 备注
      /// </summary>
      [DisplayName("备注")]
      public string Memo
      {
         get { return _memo; }
         set { _memo = value; }
      }
      private string _memo;

      /// <summary>
      /// 标记主键属性是否已初始化
      /// </summary>
      [Browsable(false)]
      public virtual bool KeyInitialized
      {
         get
         {
            if (String.IsNullOrEmpty(Code))
               return false;
            else
               return true;
         }
      }

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      [Browsable(false)]
      public abstract string InitializeSentence
      {
         get;
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      [Browsable(false)]
      public abstract string FilterCondition
      {
         get;
      }

      /// <summary>
      /// 供子类判断是否是在做初始化动作
      /// </summary>
      protected bool IsInit
      {
         get { return _isInit; }
      }
      private bool _isInit = false;
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      protected EPBaseObject()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      protected EPBaseObject(string code)
      {
         Code = code;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      protected EPBaseObject(string code, string name)
      {
         Code = code;
         Name = name;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      protected EPBaseObject(DataRow sourceRow)
      {
         if (sourceRow != null)
            Initialize(sourceRow);
      }
      #endregion

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sqlName"></param>
      /// <returns></returns>
      protected static string GetQuerySentenceFromXml(string sqlName)
      {
         return PersistentObjectFactory.GetQuerySentenceByName(sqlName);
      }

      /// <summary>
      /// 统一的格式化过滤条件方法
      /// </summary>
      /// <param name="fieldName"></param>
      /// <param name="codeValue"></param>
      /// <returns></returns>
      protected static string FormatFilterString(string fieldName, string codeValue)
      {
         if (String.IsNullOrEmpty(fieldName))
            return "";
         if (String.IsNullOrEmpty(codeValue))
            return fieldName.Trim() + " = ''";
         else
            return fieldName.Trim() + " = '" + codeValue.Trim() + "'";
      }

      #region public methods

      /// <summary>
      /// 用传入的DataRow初始化属性(DataRow不需要与对象的结构完全匹配，内部只初始化字段名匹配的属性)
      /// </summary>
      /// <param name="sourceRow">包含初始数据的DataRow</param>
      public void Initialize(DataRow sourceRow)
      {
         PersistentObjectFactory.InitializeObjectProperty(this, sourceRow);
      }

      ///// <summary>
      ///// 以当前对象为基础复制一个新对象，保留关键属性的值
      ///// </summary>
      ///// <returns></returns>
      //public abstract EPBaseObject Copy();

      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return String.Format(CultureInfo.CurrentCulture, "{0}[{1}]", Name, Code);
      }

      /// <summary>
      /// 克隆对象
      /// </summary>
      /// <returns></returns>
      internal Object Clone()
      {
         return PersistentObjectFactory.CloneEopBaseObject(this);
      }

      /// <summary>
      /// 由主键获取相应DataRow，并初始化其它属性
      /// </summary>
      public virtual void ReInitializeProperties()
      {
         if (PersistentObjectFactory.SqlExecutor != null)
         {
            DataRow row = PersistentObjectFactory.SqlExecutor.GetRecord(
               InitializeSentence, FilterCondition, true);
            if (row != null)
               Initialize(row);
         }
      }

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public abstract void ReInitializeAllProperties();
      #endregion

      #region ISupportInitialize 成员

      /// <summary>
      /// 
      /// </summary>
      public virtual void BeginInit()
      {
         _isInit = true;
      }

      /// <summary>
      /// 
      /// </summary>
      public virtual void EndInit()
      {
         _isInit = false;
      }

      #endregion
   }
}
