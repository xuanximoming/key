using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 公共的字符串变量
   /// </summary>
   internal class ConstNames
   {
      #region normal
      public const string OpSave = "保存";
      public const string OpCheck = "检查";
      public const string OpSend = "发送";

      public const string LongOrder = "长期医嘱";
      public const string TempOrder = "临时医嘱";

      public const string DoctorId = "医生代码";
      public const string Inpatient = "患者";

      public const string LightDemanding = "阳性";
      public const string Amount = "数量";

      public const string DoctorLevel = "职称";
      public const string Diagnose = "诊断";
      public const string Dept = "科室";
      public const string MedicalCare = "医保类型";

      public const string OrderUILogic = "界面处理逻辑";
      public const string DataAccess = "数据访问组件";

      public const string TimeOfShiftDept = "转科时间";
      public const string TimeOfOperation = "手术时间";
      public const string TimeOfOutHospital = "出院时间";
      public const string TimeOfCease = "停止时间";
      public const string TimeOfCancel = "取消时间";
      #endregion

      #region Order State
      public const string OrderStateAll = "<全部>……";
      public const string OrderStateNew = "新医嘱";
      public const string OrderStateAudited = "已审核医嘱";
      public const string OrderStateExecuted = "已执行医嘱";
      public const string OrderStateCeased = "已停止医嘱";
      public const string OrderStateCancelled = "被取消医嘱";
      #endregion

      #region Order Operation
      public const string OpDelete = "删除";
      public const string OpSetGroup = "成组";
      public const string OpCancelGroup = "取消成组";
      public const string OpAudit = "审核";
      public const string OpCancel = "取消";
      public const string OpCease = "停止医嘱";
      public const string OpMoveUp = "上移";
      public const string OpMoveDown = "下移";
      #endregion

      #region name of content kind
      public const string ContentDruggery = "药品";
      public const string ContentChargeItem = "普通项目";
      public const string ContentGeneralItem = "常规项目";
      public const string ContentClinicItem = "临床项目";
      public const string ContentOutDruggery = "出院带药";
      public const string ContentOperation = "手术";
      public const string ContentTextNormal = "纯医嘱";
      public const string ContentTextShiftDept = "转科";
      public const string ContentTextAfterOperation = "术后医嘱";
      public const string ContentTextLeaveHospital = "出院";
      #endregion

      #region name of content catalog table column
      public const string ContentCatalogId = "代码";
      public const string ContentCatalogName = "分类名称";
      public const string ContentCatalogFlag = "医嘱管理标志";
      #endregion

      #region name of order output table column
      public const string OrderOutputProductSerialNo = "药品序号";
      public const string OrderOutputDruggeryName = "药品名称";
      public const string OrderOutputAmount = "药品数量";
      public const string OrderOutputUnit = "单位名称";
      public const string OrderOutputUsageCode = "用法代码";
      public const string OrderOutputUsageName = "用法名称";
      public const string OrderOutputFrequencyCode = "频次代码";
      public const string OrderOutputFrequencyName = "频次名称";
      public const string OrderOutPutDruggeryCode = "药品代码";
      #endregion
   }
}
