using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections.ObjectModel;

using System.Globalization;
using DrectSoft.Core;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 院内科室
   /// TODO: 还缺部分属性
   /// </summary>
   public class Department : EPBaseObject
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
      /// 所属一级科室代码
      /// TODO: 需补充一级科室类
      /// </summary>
      public string LevelADeptCode
      {
         get { return _levelADeptCode; }
         set { _levelADeptCode = value; }
      }
      private string _levelADeptCode;


      /// <summary>
      /// 所属二级科室代码
      /// TODO: 需补充二级科室类
      /// </summary>
      public string LevelBDeptCode
      {
         get { return _levelBDeptCode; }
         set { _levelBDeptCode = value; }
      }
      private string _levelBDeptCode;

      /// <summary>
      /// 科室类别
      /// </summary>
      public DepartmentKind Kind
      {
         get { return _kind; }
         set { _kind = value; }
      }
      private DepartmentKind _kind;

      /// <summary>
      /// 科室标志
      /// </summary>
      public DepartmentClinicKind DeptAttribute
      {
         get { return _deptAttribute; }
         set { _deptAttribute = value; }
      }
      private DepartmentClinicKind _deptAttribute;

      /// <summary>
      /// 对应病区代码列表
      /// </summary>
      public Collection<String> CorrespondingWards
      {
         get
         {
            if (_correspondingWards == null)
               _correspondingWards = new Collection<String>();

            return _correspondingWards;
         }
      }
      private Collection<string> _correspondingWards;

      /// <summary>
      /// 对应病区过滤条件
      /// </summary>
      public string CorrespondingWardsCondition
      {
         get
         {
            StringBuilder result = new StringBuilder(" WardID in (");
            if (CorrespondingWards.Count == 0)
               result.Append("''");
            else
               foreach (string ward in CorrespondingWards)
               {
                  result.AppendFormat(CultureInfo.CurrentCulture, " '{0}'", ward.Trim());
                  if (result.Length > 0)
                     result.Append(',');
               }
            result.Append(')');
            return result.ToString();
         }
      }

      /// <summary>
      /// 与实体类匹配的、读取DB中数据的SQL语句
      /// </summary>
      public override string InitializeSentence
      {
         get { return GetQuerySentenceFromXml("SelectDepartment"); }
      }

      /// <summary>
      /// 在DataTable中按主键值过滤记录的条件
      /// </summary>
      public override string FilterCondition
      {
          get { return FormatFilterString("DeptID", Code); }
      }
      #endregion

      #region ctor
      /// <summary>
      /// 
      /// </summary>
      public Department()
         : base()
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      public Department(string code)
         : base(code)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="code"></param>
      /// <param name="name"></param>
      public Department(string code, string name)
         : base(code, name)
      { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sourceRow"></param>
      public Department(DataRow sourceRow)
         : base(sourceRow)
      { }

      #endregion

      #region public method
      /// <summary>
      /// 初始化科室对应病区数据
      /// </summary>
      /// <param name="sqlExecutor"></param>
      public void InitializeCorrespondingWards(IDataAccess sqlExecutor)
      {
         if ((_correspondingWards == null) || (_correspondingWards.Count == 0))
         {
            _correspondingWards = new Collection<string>();
            // 查找对应病区
            DataRow[] rows = sqlExecutor.GetRecords(
                 PersistentObjectFactory.GetQuerySentenceByName("SelectDepartmentWardMappings")
               , String.Format(CultureInfo.CurrentCulture, "DeptID = '{0}'", Code)
               , true);

            foreach (DataRow row in rows)
                _correspondingWards.Add(row["WardID"].ToString());
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
