using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.Common.Eop;
using System.Data;
using System.Globalization;
namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 医院诊断库类
   /// </summary>
   public class ICDDiagnosis : EPBaseObject
   {
      #region properties
      /// <summary>
      /// ICD10代码
      /// </summary>
      public string IcdCode
      {
         get { return _icdCode; }
         set { _icdCode = value; }
      }
      private string _icdCode;

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return GetQuerySentenceFromXml("SelectDiagnoseICD10"); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
          get { return FormatFilterString("MarkId", IcdCode); }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public ICDDiagnosis()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      public ICDDiagnosis(string code)
         : base(code)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public ICDDiagnosis(string code, string name)
         : base(code, name)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public ICDDiagnosis(DataRow sourceRow)
         : base(sourceRow)
      { }

      #endregion

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         ReInitializeProperties();
      }
   }
}
