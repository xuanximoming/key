using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using DrectSoft.Common.Eop;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 联系人
   /// </summary>
   public class LinkMan : EPBaseObject
   {
      #region properties
      /// <summary>
      /// 联系关系
      /// </summary>
      public CommonBaseCode Relation
      {
         get { return _relation; }
         set { _relation = value; }
      }
      private CommonBaseCode _relation;

      /// <summary>
      /// 联系地址
      /// </summary>
      public Address ContactAddress
      {
         get { return _contactAddress; }
         set { _contactAddress = value; }
      }
      private Address _contactAddress;

      /// <summary>
      /// 联系单位
      /// </summary>
      public WorkDepartment ContactDepartment
      {
         get { return _contactDepartment; }
         set { _contactDepartment = value; }
      }
      private WorkDepartment _contactDepartment;

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get
         {
            throw new NotImplementedException();
         }
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
      /// 初始化空实例
      /// </summary>
      public LinkMan()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public LinkMan(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
         if (Relation != null)
            Relation.ReInitializeAllProperties();
         if (ContactAddress != null)
            ContactAddress.ReInitializeAllProperties();
         if (ContactDepartment != null)
            ContactDepartment.ReInitializeAllProperties();
      }
   }
}
