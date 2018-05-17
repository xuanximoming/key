using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.Core.DoctorAdvice
{
   internal static class ConstMessages
   {
      #region hint
      public const string OpHintAddNewOrder = "正在输入新医嘱……";
      public const string OpHintModifyData = "正在修改医嘱……";
      //private const string OpHintDonotAllowEdit = "{0}，不能编辑医嘱";
      public const string HintInitData = "初始化基础数据……";
      public const string HintInitSkinTestResult = "读取皮试信息……";
      public const string HintInitMedicom = "初始化配伍禁忌模块……";
      public const string HintReadOrderData = "读取病人的医嘱数据……";
      #endregion

      #region exception title
      public const string ExceptionTitleOrder = "医嘱";
      public const string ExceptionTitleOrderTable = "医嘱列表";
      #endregion

      #region normal exception
      public const string ExceptionNotOrderEdit = "非医嘱模式，不能调用此方法";
      public const string ExceptionNotOrderSuitEdit = "非成套医嘱维护模式，不能调用此方法";
      public const string ExceptionCantInsertOrder = "不能向当前位置插入医嘱，请重新刷新数据";
      public const string ExceptionFailedSendDataToMedicom = "向合理用药模块传递医嘱数据出错";
      public const string ExceptionGroupSerialNoError = "分组序号有错误";
      public const string ExceptionGroupStateError = "分组状态不正确";
      public const string ExceptionNullOrderObject = "空的医嘱对象";
      public const string ExceptionNullOrderTable = "空的医嘱对象表";
      public const string ExceptionOrderIndexNotFind = "指定的医嘱序号不不存在";
      public const string ExceptionOrderNotFind = "未找到指定医嘱";
      public const string ExceptionOrginalOrderNotFind = "未找到原始记录";
      public const string ExceptionOutOfListRange = "移动的位置超出了列表的范围";
      public const string ExceptionHaveManyMatchRows = "存在多条匹配的行";
      public const string ExceptionNotAllowAddNew = "不允许添加新行";
      public const string ExceptionNotAllowDeleteOrder = "不允许删除医嘱对象";
      public const string ExceptionAddError = "添加失败";
      public const string ExceptionInsertError = "插入失败";
      public const string ExceptionCallRemoting = "调用接口服务出现异常";
      public const string ExceptionCantReadRecipteRules = "读取处方规则设置出现异常";
      #endregion

      #region format exception
      public const string ExceptionFormatNotFindBand = "未找到指定的表格分栏：{0}";
      public const string ExceptionFormatNotFindColumn = "未找到指定的列: {0}";
      public const string ExceptionFormatNullObject = "空对象：{0}";
      public const string ExceptionFormatNoValue = "{0}未赋值";
      #endregion

      #region normal message
      public const string MsgPromptingSaveData = "数据已修改，是否保存？";
      public const string MsgPromptingSendData = "上一位患者有未发送的医嘱数据，现在是否发送？";
      public const string MsgSaveDataAfterModified = "请修改医嘱后重新保存数据";
      public const string MsgSuccessSendData = "操作成功！最近修改的数据已发送到护士站";
      public const string MsgNotFindSkinTestResult = "未找到皮试信息，请联系护士做皮试";
      public const string MsgSkinRestResultIsPlus = "皮试结果是阳性，不能输入";
      public const string MsgCanntGetRecipeRuleData = "不能读取处方规则数据（可能是无法联接到HIS）";
      
      public const string FailedSaveData = "医嘱保存失败";
      public const string FailedSaveSuiteDetail = "成套医嘱明细数据保存失败";
      public const string FailedSendDataToHis = "医嘱发送失败";
      public const string FailedInitMedicom = "初始化配伍禁忌模块出错，配伍禁忌模块将不可用";

      public const string CheckSelectedTooManyExecuteDate = "选中的“执行日期”数量太多，多余的将被去掉";
      public const string CheckSelectedTooFewExecuteDate = "选中的“执行日期”数量太少，将恢复到默认值";
      public const string CheckSelectedTooManyExecuteTime = "选中的“执行时间”数量太多，多余的将被去掉";
      public const string CheckSelectedTooFewExecuteTime = "选中的“执行时间”数量太少，将恢复到默认值";
      public const string CheckStartDateNull = "开始日期不能为空";
      public const string CheckStartDateBeforPreRow = "开始日期不能在上一条新增医嘱之前";
      public const string CheckCeaseDateBeforeStartDate = "停止时间不能在当前时间或医嘱开始时间之前";
      public const string CheckCeaseDateBeforeNow = "停止时间不能小于当前日期";
      public const string CheckOnlyAllowDruggery = "出院医嘱已开，只能添加药品";
      public const string CheckCatalogNotSupport = "当前医嘱类别不支持此类型的内容";
      public const string CheckNotAllowMixCatalogInSuite = "不能在一个成套中同时添加出院带药和其它类别的医嘱";
      public const string CheckItemRepeatedInNew = "新医嘱或已审核医嘱中存在相同项目的医嘱";
      public const string CheckItemRepeatedInExecuting = "当前有效的长期医嘱中已存在相同项目的医嘱";
      public const string CheckAllIsNew = "全部是新增医嘱";
      public const string CheckDelAllOutOrder = "“出院医嘱”和“出院带药”医嘱要同时删除";
      public const string CheckOnlyOneRowInGroup = "一条医嘱不需要分组";
      public const string CheckNewInGroup = "全部是新增药品医嘱，且开始时间、用法、频次都一致";
      public const string CheckSerialInGroup = "是连续的医嘱，不包括已分组的记录";
      public const string CheckCancelGroup = "只能改变新增医嘱的分组状态";
      public const string CheckAudit = "同组的医嘱不能分开审核";
      public const string CheckAllIsAudited = "全部是已审核医嘱";
      public const string CheckCancel = "同组的医嘱必须一起取消";
      public const string CheckCeaseTimeIsNull = "只能对没有停止时间的长期医嘱设置停止时间";
      public const string CheckInsertRowInGroup = "不能向已成组的医嘱中插入数据";
      public const string CheckPropertyInGroup = "向组中插入的数据其开始时间、频次、用法必须一致";
      public const string CheckNotAllowInsertFerbInGroup = "不能向组中插入草药数据";
      public const string CheckOnlyAllowInsertFerbInGroup = "只能向组中插入草药数据";
      public const string CheckOnlyAllowDruggeryInGroup = "只能向组中插入药品数据";
      public const string CheckOrderStateBeforeSave = "医嘱数据已经发生变化，请刷新数据（刷新后最近所做的修改将丢失）\r\n"
                        + "    数据改变的原因可能是护士站已经审核或执行了新医嘱\r\n"
                        + "    其它工作站修改了改病人的申请单数据";
      public const string CheckNumberOfSynchedOrder = "要同步序号的医嘱数量不匹配";
      public const string CheckEditableOfOrderCatalog = "当前医嘱类别不允许编辑";
      public const string CheckCanInsertOrderAtSpecialState = "选择了特定状态的医嘱，不能插入新医嘱";
      public const string CheckCantAddNewAfterHasShiftDeptOrder = "转科医嘱已开，不能插入新医嘱";
      public const string CheckOnlyAllowDruggeryAfterHasOutHospitalOrder = "出院医嘱已开，只允许添加出院带药";
      public const string CheckCanInsertOrderAfterCurrent = "当前医嘱的后面不允许插入医嘱";
      public const string CheckCanInsertOrderBeforeOutHospitalOrder = "不允许在“出院医嘱”前面插入医嘱";
      public const string CheckRecipeRuleDoctorLevel = "该类药品、项目的使用权有职称限制，不能使用！";
      public const string CheckRecipeRuleDiagnose = "该类药品、项目的使用权有病人诊断限制，不能使用！";
      public const string CheckRecipeRuleDept = "本科室没有权限使用该类药品、项目！";
      public const string CheckRecipeRuleMedicalCare = "该类药品、项目的使用权有医保类型限制，不能使用！";
      public const string CheckOrderSelectionRange = "医嘱范围不正确";
      public const string CheckSelectedRangWithDataRow = "选中的行与实际记录不匹配";

      public const string CheckSuiteData = "请检查数量不为零的项目或药品数据是否正确！\r\n成组的必须同时加入";

      public const string ConditionMoveUp = "全部是新增医嘱，且记录是连续\r\n上一条也是新增医嘱";
      public const string ConditionMoveDown = "全部是新增医嘱，且记录是连续\r\n下一条也是新增医嘱";
      #endregion

      #region format message
      public const string FormatMedicomCheckNotPass = "合理用药模块检查 {0} 未通过，确定要保存数据吗？";
      public const string FormatOrderSaveWarning = "{0}中存在以下问题：\r\n {1} \r\n是否继续？";
      public const string FormatRangOfCount = "数量的范围是 {0} ～ {1}";

      public const string FormatOpError = "不能对选中行进行{0}操作，符合以下条件才能{0}：\r\n{1}";
      public const string FormatStartDateMustAfter = "开始日期必须在{0:yyyy-M-d HH:mm}之后";
      public const string FormatStartDateMustBefore = "开始日期在{0:yyyy-M-d HH:mm}之前";

      public const string FormatMoreThanControlLine = "当前数量超过控制数量(控制线:{0} {1})";
      public const string FormatMoreThanWarningLine = "当前数量超过警告数量(警告线:{0} {1})";

      public const string FormatItemAgeWarning = "对于 {0} 岁以下的病人不建议使用此项目";
      #endregion
   }
}
