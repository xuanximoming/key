using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using DrectSoft.Common.Eop;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 工作单位
   /// </summary>
   public class WorkDepartment : EPBaseObject
   {
      #region properties
      /// <summary>
      /// 公司名称(工作单位)
      /// </summary>
      public string CompanyName
      {
         get { return _companyName; }
         set { _companyName = value; }
      }
      private string _companyName;

      /// <summary>
      /// 单位地址
      /// </summary>    
      public Address CompanyAddress
      {
         get { return _companyAddress; }
         set { _companyAddress = value; }
      }
      private Address _companyAddress;

      /// <summary>
      /// 职业代码 默认初始化字典
      /// </summary>
      public CommonBaseCode Occupation
      {
         get { return _occupation; }
         set { _occupation = value; }
      }
      private CommonBaseCode _occupation;

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { throw new NotImplementedException(); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
         get { throw new NotImplementedException(); }
      }
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      public WorkDepartment()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public WorkDepartment(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         if (CompanyAddress != null)
            CompanyAddress.ReInitializeAllProperties();
         if (Occupation != null)
            Occupation.ReInitializeAllProperties();
      }
   }
}
