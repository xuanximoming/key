using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.Common.Eop;
using System.Data;
using System.Globalization;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 中医诊断库类
   /// </summary>
   public class ChineseDiagnosis : EPBaseObject 
   {
      #region properties
      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return GetQuerySentenceFromXml("SelectChineseDiagnose"); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
          get { return FormatFilterString("ChDiagnoseID", Code); }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
     public ChineseDiagnosis()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      public ChineseDiagnosis(string code)
         : base(code)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public ChineseDiagnosis(string code, string name)      
         : base(code, name)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public ChineseDiagnosis(DataRow sourceRow)
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
