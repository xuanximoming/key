using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 医保类型类
   /// TODO: 临时编写，未完整实现
   /// </summary>
   public class MedicareType : EPBaseObject
   {
      #region properties
      /// <summary>
      /// 凭证类型
      /// </summary>
      public string VoucherCode
      {
         get { return _voucherCode; }
         set { _voucherCode = value; }
      }
      private string _voucherCode;

      /// <summary>
      /// 凭证类型名称
      /// </summary>
      public string VoucherName
      {
         get { return _voucherName; }
         set { _voucherName = value; }
      }
      private string _voucherName;

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return GetQuerySentenceFromXml("SelectMedicareTypeBook"); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
         get
         {
            return FormatFilterString("ybdm", Code);
         }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public MedicareType()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      public MedicareType(string code)
         : base(code)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public MedicareType(string code, string name)
         : base(code, name)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public MedicareType(DataRow sourceRow)
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
