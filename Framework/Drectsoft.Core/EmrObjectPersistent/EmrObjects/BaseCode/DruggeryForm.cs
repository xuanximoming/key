using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 药品剂型类
   /// TODO: 临时编写，未完整实现
   /// </summary>
   public class DruggeryForm : EPBaseObject
   {
      #region properties

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return GetQuerySentenceFromXml("SelectDruggeryFormBook"); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
         get
         {
            return FormatFilterString("ID", Code); 
         }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public DruggeryForm()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      public DruggeryForm(string code)
         : base(code)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public DruggeryForm(string code, string name)
         : base(code, name)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public DruggeryForm(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      #region public methods

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         ReInitializeProperties();
      }
      #endregion
   }
}
