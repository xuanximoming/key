using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Common.MediComIntf
{
   /// <summary>
   /// 美康接口病人信息类
   /// </summary>
   public struct MediIntfPatientInfo
   {
      /// <summary>
      ///病人ID
      /// </summary>
      public string PatientID;                 
      /// <summary>
      ///住院次数,如果传''，则系统认为是'1'
      /// </summary>
      public string VisitID;                     
      /// <summary>
      ///病人姓名
      /// </summary>
      public string PatientName;            
      /// <summary>
      ///病人性别,格式为"男"、"女"、"不详"等。
      /// </summary>
      public string Sex;                          
      /// <summary>
      ///病人出生日期,格式为"yyyy-mm-dd"
      /// </summary>
      public string Birthday;                   
      /// <summary>
      ///体重,表示病人以公斤为单位的体重值
      /// </summary>
      public string Weight;                     
      /// <summary>
      ///身高,表示病人以厘米为单位的身高值
      /// </summary>
      public string Height;                      
      /// <summary>
      ///科室名称,表示病人当前所在科室或病区名称
      /// </summary>
      public string DeptName;                 
      /// <summary>
      ///医生姓名
      /// </summary>
      public string Doctor;                      
      /// <summary>
      ///病人类型 "0"住院,"1"门诊，"2"配制中心或其它
      /// </summary>
      public string PatientType;              
      /// <summary>
      ///表示病人用药审查日期，格式为"yyyy-mm-dd"，传空时，系统默认为当前日期
      /// </summary>
      public string UseDate;                   
   }
}
