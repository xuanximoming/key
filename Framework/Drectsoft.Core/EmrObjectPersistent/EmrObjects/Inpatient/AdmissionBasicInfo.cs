using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using DrectSoft.Common.Eop;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 住院基本信息类
   /// </summary>
   public class AdmissionBasicInfo : EPBaseObject
   {
      #region properties
      /// <summary>
      /// 科室
      /// </summary>
      public Department CurrentDepartment
      {
         get { return _currentDepartment; }
         set { _currentDepartment = value; }
      }
      private Department _currentDepartment;

      /// <summary>
      /// 病区
      /// </summary>
      public Ward CurrentWard
      {
         get { return _currentWard; }
         set { _currentWard = value; }
      }
      private Ward _currentWard;

      /// <summary>
      /// 床号
      /// </summary>
      public string BedNo
      {
         get { return _bedNo; }
         set { _bedNo = value; }
      }
      private string _bedNo;

      /// <summary>
      /// 进入第一阶段的时间(入院或出区)
      /// </summary>
      public DateTime StepOneDate
      {
         get { return _stepOneDate; }
         set { _stepOneDate = value; }
      }
      private DateTime _stepOneDate;

      /// <summary>
      /// 进入第二阶段的时间(入区或出院)
      /// </summary>
      public DateTime StepTwoDate
      {
         get { return _stepTwoDate; }
         set { _stepTwoDate = value; }
      }
      private DateTime _stepTwoDate;

      /// <summary>
      /// 诊断
      /// </summary>
      public ICDDiagnosis Diagnosis
      {
         get { return _diagnosis; }
         set { _diagnosis = value; }
      }
      private ICDDiagnosis _diagnosis;

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
      /// 初始化空实例
      /// </summary>
      public AdmissionBasicInfo()
         : base()
      { }

      /// <summary>
      /// 用DataRow初始化实例
      /// </summary>
      /// <param name="sourceRow"></param>
      public AdmissionBasicInfo(DataRow sourceRow)
         : base(sourceRow)
      { }
      #endregion

      /// <summary>
      /// 初始化所有的属性，包括引用类型的属性自己的属性
      /// </summary>
      public override void ReInitializeAllProperties()
      {
          if (CurrentDepartment != null)
          {
              CurrentDepartment = new Department();
              CurrentDepartment.ReInitializeAllProperties();
          }
         if (CurrentWard != null)
            CurrentWard.ReInitializeAllProperties();
         if (Diagnosis != null)
            Diagnosis.ReInitializeAllProperties();
      }
   }
}
