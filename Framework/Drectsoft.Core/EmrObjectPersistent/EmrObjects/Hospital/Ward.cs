using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using System.Globalization;
using DrectSoft.Core;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 病区
   /// </summary>
   public class Ward : EPBaseObject
   {
      #region properties
      /// <summary>
      /// 所属医院代码
      /// TODO: 需要补充医院类
      /// </summary>
      public string HospitalCode
      {
         get { return _hospitalCode; }
         set { _hospitalCode = value; }
      }
      private string _hospitalCode;

      /// <summary>
      /// 床位数
      /// </summary>
      public int AmountOfBeds
      {
         get { return _amountOfBeds; }
         set { _amountOfBeds = value; }
      }
      private int _amountOfBeds;

      /// <summary>
      /// 病区标志()
      /// </summary>
      public WardKind Kind
      {
         get { return _kind; }
         set { _kind = value; }
      }
      private WardKind _kind;

      /// <summary>
      /// 对应科室
      /// </summary>
      public Collection<string> CorrespondingDepts
      {
         get
         {
            if (_correspondingDepts == null)
               _correspondingDepts = new Collection<string>();

            return _correspondingDepts;
         }
      }
      private Collection<string> _correspondingDepts;

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return GetQuerySentenceFromXml("SelectWard"); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
          get { return FormatFilterString("WardID", Code); }
      }
      #endregion

      #region ctor
      /// <summary>
      /// 
      /// </summary>
      public Ward()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      public Ward(string code)
         : base(code)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public Ward(string code, string name)
         : base(code, name)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public Ward(DataRow sourceRow)
         : base(sourceRow)
      { }

      #endregion

      #region public method
      /// <summary>
      /// 初始化关联科室数据
      /// </summary>
      /// <param name="sqlExecutor"></param>
      public void InitializeCorrespondingDepts(IDataAccess sqlExecutor)
      {
         if ((_correspondingDepts == null) || (_correspondingDepts.Count == 0))
         {
            _correspondingDepts = new Collection<string>();
            // 查找对应科室
            DataRow[] rows = sqlExecutor.GetRecords(
                 PersistentObjectFactory.GetQuerySentenceByName("SelectDepartmentWardMappings")
               , String.Format(CultureInfo.CurrentCulture, "WardID = '{0}'", Code)
               , true);

            foreach (DataRow row in rows)
                _correspondingDepts.Add(row["DeptID"].ToString());
         }

      }

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
