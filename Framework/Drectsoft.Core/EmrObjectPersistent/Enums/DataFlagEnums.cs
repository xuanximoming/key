using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using DrectSoft.Core;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 科室类别标记
   /// </summary>
   public enum DepartmentKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 临床
      /// </summary>
      Clinic = 101,
      /// <summary>
      /// 医技
      /// </summary>
      Technic = 102,
      /// <summary>
      /// 药剂
      /// </summary>
      Druggery = 103,
      /// <summary>
      /// 机关
      /// </summary>
      Service = 104,
      /// <summary>
      /// 其他
      /// </summary>
      Other = 105
   }

   /// <summary>
   /// 科室标志(针对临床科室)
   /// </summary>
   public enum DepartmentClinicKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 普通
      /// </summary>
      Normal = 201,
      /// <summary>
      /// 手术室
      /// </summary>
      Theater = 202,
      /// <summary>
      /// 产房
      /// </summary>
      Delivery = 203,
      /// <summary>
      /// ICU|CCU
      /// </summary>
      ICU = 204,
      /// <summary>
      /// 儿科
      /// </summary>
      Pediatrics = 205
   }

   /// <summary>
   /// 病区标志
   /// </summary>
   public enum WardKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 普通
      /// </summary>
      Normal = 300,
      /// <summary>
      /// 急观
      /// </summary>
      Emergency = 301,
      /// <summary>
      /// 产房
      /// </summary>
      Delivery = 302,
      /// <summary>
      /// ICU|CCU
      /// </summary>
      ICU = 303
   }

   /// <summary>
   /// 职工类别标记
   /// </summary>
   public enum EmployeeKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 普通医生
      /// </summary>
      Doctor = 400,
      /// <summary>
      /// 专家医生
      /// </summary>
      Specialist = 401,
      /// <summary>
      /// 护士
      /// </summary>
      Nurse = 402,
      /// <summary>
      /// 麻醉师
      /// </summary>
      Anaesthetist = 403,
      /// <summary>
      /// 其他
      /// </summary>
      Others = 404
   }

   /// <summary>
   /// 医院等级标记
   /// </summary>
   public enum HospitalGrade
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 一级
      /// </summary>
      Level1 = 501,
      /// <summary>
      /// 二级
      /// </summary>
      Level2 = 502,
      /// <summary>
      /// 三级
      /// </summary>
      Level3 = 503
   }

   /// <summary>
   /// 医院类别标记
   /// </summary>
   public enum HospitalKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 当前系统使用医院
      /// </summary>
      Current = 600,
      /// <summary>
      /// 协作医院
      /// </summary>
      Cooperation = 601
   }

   /// <summary>
   /// 病种类别标记(用在病种分类库中)
   /// </summary>
   public enum DiseaseKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 单病种
      /// </summary>
      Single = 700,
      /// <summary>
      /// 疾病分类
      /// </summary>
      Disease = 701,
      /// <summary>
      /// 院内分类
      /// </summary>
      Hospital = 702
   }

   /// <summary>
   /// 手术级别标记
   /// </summary>
   public enum OperationGrade
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 特大
      /// </summary>
      Super = 800,
      /// <summary>
      /// 大
      /// </summary>
      Large = 801,
      /// <summary>
      /// 中
      /// </summary>
      Middle = 802,
      /// <summary>
      /// 小
      /// </summary>
      Small = 803
   }

   ///// <summary>
   ///// 疾病其他类别标记(标记病种属性,病案中用)
   ///// </summary>
   //public enum DiseaseTag
   //{
   //   None = 0,
   //   /// <summary>
   //   /// 急性驰缓性麻痹病
   //   /// </summary>
   //   Paralytic = 901,
   //   /// <summary>
   //   /// 新生儿感染疾病
   //   /// </summary>
   //   Baby = 902,
   //   /// <summary>
   //   /// 妊娠内科合并症
   //   /// </summary>
   //   Gestation = 903
   //}

   /// <summary>
   /// 地区级别标记
   /// </summary>
   public enum ZoneGrade
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 省、直辖市
      /// </summary>
      Province = 1000,
      /// <summary>
      /// 区县
      /// </summary>
      Country = 1001
   }

   /// <summary>
   /// 床位类型标记
   /// </summary>
   public enum BedType
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 男
      /// </summary>
      Male = 1100,
      /// <summary>
      /// 女
      /// </summary>
      Female = 1101,
      /// <summary>
      /// 混
      /// </summary>
      Mix = 1102
   }

   /// <summary>
   /// 床位编制类型标记
   /// </summary>
   public enum BedKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 在编
      /// </summary>
      Normal = 1200,
      /// <summary>
      /// 非编
      /// </summary>
      Extra = 1201,
      /// <summary>
      /// 加床
      /// </summary>
      AddIn = 1202
   }

   /// <summary>
   /// 床位使用状态
   /// </summary>
   public enum BedState
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 空床
      /// </summary>
      No = 1300,
      /// <summary>
      /// 占床
      /// </summary>
      Yes = 1301,
      /// <summary>
      /// 包床
      /// </summary>
      Wrapped = 1302
   }

   /// <summary>
   /// 系统标志(指明数据项适用的系统范围)
   /// </summary>
   public enum SystemApplyRange
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 门诊、住院
      /// </summary>
      All = 1400,
      /// <summary>
      /// 门诊部门
      /// </summary>
      OutpatientDept = 1401,
      /// <summary>
      /// 住院部门
      /// </summary>
      InpatientDept = 1402
   }

   /// <summary>
   /// 住院病人状态标记
   /// </summary>
   public enum InpatientState
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 入院登记
      /// </summary>
      New = 1500,
      /// <summary>
      /// 病区分床
      /// </summary>
      InWard = 1501,
      /// <summary>
      /// 病区出院
      /// </summary>
      OutWard = 1502,
      /// <summary>
      /// 病人出院
      /// </summary>
      Balanced = 1503,
      /// <summary>
      /// 取消结算
      /// </summary>
      CancleBalanced = 1504,
      /// <summary>
      /// 进入ICU
      /// </summary>
      InICU = 1505,
      /// <summary>
      /// 进入产房
      /// </summary>
      InDeliveryRoom = 1506,
      /// <summary>
      /// 转科状态(待转出,对方还没有接收)
      /// </summary>
      ShiftDept = 1507,
      /// <summary>
      /// 数据转出(数据已迁移到历史库中)
      /// </summary>
      DataDumped = 1508,
      /// <summary>
      /// 作废记录
      /// </summary>
      Outdated = 1509
   }

   ///// <summary>
   ///// 医保自负比例标志标记(这部分标记在EMR中应该用不到,只是为了导入医保类型数据使用)
   ///// </summary>
   //public enum InsurancePayRateFlags
   //{
   //   //[Description("缺省自费比例")]
   //   // = 1700,
   //   //[Description("缺省优惠比例")]
   //   // = 1701,
   //   //[Description("特殊自费比例")]
   //   // = 1702,
   //   //[Description("特殊优惠比例")]
   //   // = 1703,
   //   //[Description("全自费病人标志")]
   //   // = 1704,
   //}

   ///// <summary>
   ///// 帐户标志(这部分标记在EMR中应该用不到,只是为了导入医保类型数据使用)
   ///// </summary>
   //public enum PatientAccountFlags
   //{
   //   /// <summary>
   //   /// 对应的数据类别
   //   /// </summary>
   //   Catalog = 18,
   //   //[Description("无个人帐户")]
   //   // = 1800,
   //   //[Description("有个人账户")]
   //   // = 1801,
   //   //[Description("允许欠费")]
   //   // = 1802,
   //}

   ///// <summary>
   ///// 医保计算方式标记(这部分标记在EMR中应该用不到,只是为了导入医保类型数据使用)
   ///// </summary>
   //public enum InsuranceCalcFlags
   //{
   //   /// <summary>
   //   /// 对应的数据类别
   //   /// </summary>
   //   Catalog = 19,
   //   //[Description("按医保总额计算")]
   //   // = 1900,
   //   //[Description("按医保支付计算")]
   //   // = 1901,
   //}

   /// <summary>
   /// 医生级别标记
   /// </summary>
   public enum DoctorGrade
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 主任医师
      /// </summary>
      Chief = 2000,
      /// <summary>
      /// 副主任医师
      /// </summary>
      AssociateChief = 2001,
      /// <summary>
      /// 主治医师
      /// </summary>
      Attending = 2002,
      /// <summary>
      /// 住院医师
      /// </summary>
      Resident = 2003,
      /// <summary>
      ///护士 
      /// </summary>
      Nurse = 2004
   }

   /// <summary>
   /// SNOMED概念状态标记
   /// </summary>
   public enum SnomedConceptState
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 正常-有效
      /// </summary>
      Natural = 2100,
      /// <summary>
      /// 退休-停用
      /// </summary>
      Retired = 2101,
      /// <summary>
      /// 重复-停用
      /// </summary>
      Repeated = 2102,
      /// <summary>
      /// 过期-停用
      /// </summary>
      Outdated = 2103,
      /// <summary>
      /// 多义-停用
      /// </summary>
      Multivocal = 2104,
      /// <summary>
      /// 错误-停用
      /// </summary>
      Wrong = 2105,
      /// <summary>
      /// 有限-停用
      /// </summary>
      Limited = 2106,
      /// <summary>
      /// 已转移-停用
      /// </summary>
      Transferred = 2110,
      /// <summary>
      /// 待移动-有效
      /// </summary>
      Transferring = 2111,
      /// <summary>
      /// 自定义-有效
      /// </summary>
      Custom = 2150,
      /// <summary>
      /// 作废自定义码
      /// </summary>
      Cancellation = 2151
   }

   /// <summary>
   /// SNOMED别名状态标记
   /// </summary>
   public enum SnomedAliasState
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 正常-有效
      /// </summary>
      Natural = 2200,
      /// <summary>
      /// 退休-停用
      /// </summary>
      Retired = 2201,
      /// <summary>
      /// 重复-停用
      /// </summary>
      Repeated = 2202,
      /// <summary>
      /// 过期-停用
      /// </summary>
      Outdated = 2203,
      /// <summary>
      /// 错误-停用
      /// </summary>
      Wrong = 2205,
      /// <summary>
      /// 有限-停用
      /// </summary>
      Limited = 2206,
      /// <summary>
      /// 不合适-停用
      /// </summary>
      Unseemliness = 2207,
      /// <summary>
      /// 概念已停用
      /// </summary>
      ConceptCeased = 2208,
      /// <summary>
      /// 已转移-停用
      /// </summary>
      Transferred = 2210,
      /// <summary>
      /// 待移动-有效
      /// </summary>
      Transferring = 2211
   }

   /// <summary>
   /// SNOMED术语类型标记
   /// </summary>
   public enum SnomedTermKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 未指明
      /// </summary>
      Unspecified = 2300,
      /// <summary>
      /// 首选
      /// </summary>
      Preferred = 2301,
      /// <summary>
      /// 同义词
      /// </summary>
      Synonymous = 2302,
      /// <summary>
      /// 全称
      /// </summary>
      FullName = 2303
   }

   /// <summary>
   /// 项目类别标记
   /// </summary>
   public enum ItemKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 西药
      /// </summary>
      WesternMedicine = 2401,
      /// <summary>
      /// 成药
      /// </summary>
      PatentMedicine = 2402,
      /// <summary>
      /// 草药
      /// </summary>
      HerbalMedicine = 2403,
      /// <summary>
      /// 治疗
      /// </summary>
      Cure = 2404,
      /// <summary>
      /// 手术
      /// </summary>
      Operation = 2405,
      /// <summary>
      /// 麻醉
      /// </summary>
      Anesthesia = 2406,
      /// <summary>
      /// 膳食
      /// </summary>
      Meal = 2407,
      /// <summary>
      /// 输血
      /// </summary>
      Transfusion = 2408,
      /// <summary>
      /// 护理
      /// </summary>
      Care = 2409,
      /// <summary>
      /// 床位
      /// </summary>
      BedFee = 2410,
      /// <summary>
      /// 检查
      /// </summary>
      Examination = 2411,
      /// <summary>
      /// 检验
      /// </summary>
      Assay = 2412,
      /// <summary>
      /// 输液
      /// </summary>
      Infusion = 2413,
      /// <summary>
      /// 挂号
      /// </summary>
      Registration = 2414,
      /// <summary>
      /// 材料
      /// </summary>
      Meterial = 2415,
      /// <summary>
      /// 诊疗(注意与治疗的区别)
      /// </summary>
      Diagnosis = 2416,
      /// <summary>
      /// 其它
      /// </summary>
      Other = 2417,
      /// <summary>
      /// 糖
      /// </summary>
      Sugar = 2420,
      /// <summary>
      /// 危重级别
      /// </summary>
      DangerLevel = 2421,
      /// <summary>
      /// 隔离种类
      /// </summary>
      IsolationCatalog = 2422,
      /// <summary>
      /// 体位
      /// </summary>
      BodyPosition = 2423,
      /// <summary>
      /// 临床项目
      /// </summary>
      ClinicItem = 2430
   }

   /// <summary>
   /// 报销类别
   /// </summary>
   public enum SubmitAccountKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 全报
      /// </summary>
      PayFull = 2500,
      /// <summary>
      /// 部分报
      /// </summary>
      PayPart = 2501,
      /// <summary>
      /// 自费
      /// </summary>
      PaySelf = 2502,
      /// <summary>
      /// 其它
      /// </summary>
      Other = 2503
   }

   /// <summary>
   /// 药品类别
   /// </summary>
   public enum DruggeryKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 普通
      /// </summary>
      Normal = 2600,
      /// <summary>
      /// 麻醉
      /// </summary>
      Anesthetics = 2601,
      /// <summary>
      /// 精神
      /// </summary>
      Psychosis = 2602,
      /// <summary>
      /// 剧毒
      /// </summary>
      Virulent = 2603,
      /// <summary>
      /// 危险
      /// </summary>
      Danger = 2604,
      /// <summary>
      /// 化试
      /// </summary>
      Reagent = 2605,
      /// <summary>
      /// 胰岛素
      /// </summary>
      Insulin = 2606,
      /// <summary>
      /// 抗菌素
      /// </summary>
      Antibiotics = 2609
   }

   /// <summary>
   /// 医嘱管理标志
   /// </summary>
   public enum OrderManagerKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 普通
      /// </summary>
      Normal = 2700,
      /// <summary>
      /// 不用于医嘱
      /// </summary>
      NotUse = 2701,
      /// <summary>
      /// 只用于临时医嘱
      /// </summary>
      ForTemp = 2702,
      /// <summary>
      /// 只用于长期医嘱
      /// </summary>
      ForLong = 2703
   }

   /// <summary>
   /// 项目使用范围标记
   /// </summary>
   public enum ItemApplyRange
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 不限制
      /// </summary>
      Unlimited = 2800,
      /// <summary>
      /// 自费使用
      /// </summary>
      ForNormal = 2801,
      /// <summary>
      /// 医保使用
      /// </summary>
      ForInsurance = 2802
   }

   /// <summary>
   /// 使用范围控制标记(数据的应用范围)
   /// </summary>
   public enum DataApplyRange
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 全院
      /// </summary>
      All = 2900,
      /// <summary>
      /// 科室
      /// </summary>
      Department = 2901,
      /// <summary>
      /// 病区
      /// </summary>
      Ward = 2902,
      /// <summary>
      /// 个人
      /// </summary>
      Individual = 2903
   }

   /// <summary>
   /// 药品单位类别
   /// </summary>
   public enum DruggeryUnitKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 剂量单位
      /// </summary>
      Dosage = 3000,
      /// <summary>
      /// 药库单位
      /// </summary>
      Depot = 3001,
      /// <summary>
      /// 门诊单位
      /// </summary>
      Policlinic = 3002,
      /// <summary>
      /// 住院单位
      /// </summary>
      Ward = 3003,
      /// <summary>
      /// 进货单位
      /// </summary>
      Stock = 3004,
      /// <summary>
      /// 儿科单位
      /// </summary>
      Paediatrics = 3005,
      /// <summary>
      /// 规格单位
      /// </summary>
      Specification = 3006,
      /// <summary>
      /// 最小单位
      /// </summary>
      Min = 3007
   }

   /// <summary>
   /// 医嘱类别标记
   /// </summary>
   [TypeConverter(typeof(LocalizedEnumConverter))]
   public enum OrderContentKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 药品医嘱
      /// </summary>
      Druggery = 3100,
      /// <summary>
      /// 收费项目医嘱
      /// </summary>
      ChargeItem = 3101,
      /// <summary>
      /// 常规医嘱
      /// </summary>
      GeneralItem = 3102,
      /// <summary>
      /// 临床项目医嘱
      /// </summary>
      ClinicItem = 3103,
      /// <summary>
      /// 出院带药
      /// </summary>
      OutDruggery = 3104,
      /// <summary>
      /// 手术医嘱
      /// </summary>
      Operation = 3105,
      /// <summary>
      /// 停长期医嘱
      /// </summary>
      CeaseLong = 3109,
      /// <summary>
      /// 纯医嘱(普通)
      /// </summary>
      TextNormal = 3110,
      /// <summary>
      /// 纯医嘱(转科)
      /// </summary>
      TextShiftDept = 3111,
      /// <summary>
      /// 纯医嘱(术后)
      /// </summary>
      TextAfterOperation = 3112,
      /// <summary>
      /// 纯医嘱(出院)
      /// </summary>
      TextLeaveHospital = 3113
   }

   /// <summary>
   /// 医嘱状态标记
   /// </summary>
   public enum OrderState
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 全部
      /// </summary>
      All = 9999,
      /// <summary>
      /// 录入
      /// </summary>
      New = 3200,
      /// <summary>
      /// 已审核
      /// </summary>
      Audited = 3201,
      /// <summary>
      /// 已执行
      /// </summary>
      Executed = 3202,
      /// <summary>
      /// 被取消
      /// </summary>
      Cancellation = 3203,
      /// <summary>
      /// 已停止
      /// </summary>
      Ceased = 3204
   }

   /// <summary>
   /// 长期医嘱停止原因标记
   /// </summary>
   public enum OrderCeaseReason
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 正常停
      /// </summary>
      Natural = 3300,
      /// <summary>
      /// 术后
      /// </summary>
      AfterOperation = 3301,
      /// <summary>
      /// 产后
      /// </summary>
      AfterDelivery = 3302,
      /// <summary>
      /// 转科
      /// </summary>
      ShiftDept = 3303,
      /// <summary>
      /// 新常规
      /// </summary>
      NewGeneral = 3305,
      /// <summary>
      /// 出院
      /// </summary>
      LeaveHospital = 3306
   }

   /// <summary>
   /// 医嘱执行周期(单位)类别
   /// </summary>
   public enum OrderExecPeriodUnitKind
   {
      /// <summary>
      /// 未知
      /// </summary>
      None = 0,
      /// <summary>
      /// 周
      /// </summary>
      Week = 3400,
      /// <summary>
      /// 天
      /// </summary>
      Day = 3401,
      /// <summary>
      /// 小时
      /// </summary>
      Hour = 3402,
      /// <summary>
      /// 分钟
      /// </summary>
      Minute = 3403
   }

   /// <summary>
   /// 分组位置类型
   /// </summary>
   public enum GroupPositionKind
   {
      /// <summary>
      /// 无法识别
      /// </summary>
      None = 0,
      /// <summary>
      /// 单条医嘱
      /// </summary>
      SingleOrder = 3500,
      /// <summary>
      /// 组开始
      /// </summary>
      GroupStart = 3501,
      /// <summary>
      /// 组中间
      /// </summary>
      GroupMiddle = 3502,
      /// <summary>
      /// 组结束
      /// </summary>
      GroupEnd = 3599
   }

   /// <summary>
   /// 打印状态
   /// </summary>
   public enum PrintState
   {
      /// <summary>
      /// 无法识别
      /// </summary>
      None = 0,
      /// <summary>
      /// 未打印
      /// </summary>
      IsNotPrinted = 3600,
      /// <summary>
      /// 已打印
      /// </summary>
      HadPrinted = 3601,
      /// <summary>
      /// 打印后修改
      /// </summary>
      ChangedAfterPrint = 3602
   }

   /// <summary>
   /// 消息状态
   /// </summary>
   public enum MessageState
   {
      /// <summary>
      /// 无法识别
      /// </summary>
      None = 0,
      /// <summary>
      /// 初始待处理
      /// </summary>
      Waiting = 3700,
      /// <summary>
      /// 处理中
      /// </summary>
      Handling = 3701,
      /// <summary>
      /// 处理完成
      /// </summary>
      Handled = 3702
   }

   /// <summary>
   /// 病历模型类型
   /// </summary>
   public enum ModelKind
   {
      /// <summary>
      /// 原子数据
      /// </summary>
      Atom = 3800,
      /// <summary>
      /// 元数据
      /// </summary>
      MetaData = 3801,
      /// <summary>
      /// 基础模板
      /// </summary>
      BaseModel = 3802,
      /// <summary>
      /// 文件结构
      /// </summary>
      FileStructure = 3803,
      /// <summary>
      /// 病历文件
      /// </summary>
      File = 3804,
      /// <summary>
      /// 病历模板
      /// </summary>
      FullModel = 3805,
      /// <summary>
      /// 表格
      /// </summary>
      Grid = 3806,
      /// <summary>
      /// 图像
      /// </summary>
      Picture = 3807,
      /// <summary>
      /// 病历文件夹
      /// </summary>
      Folder = 3808,
      /// <summary>
      /// 个人模板
      /// </summary>
      PersonalModel = 3809
   }

   /// <summary>
   /// 医技确认状态
   /// </summary>
   public enum TechAffirmState
   {
      /// <summary>
      /// 新增
      /// </summary>
      New = 4100,
      /// <summary>
      /// 审核
      /// </summary>
      Audited = 4101,
      /// <summary>
      /// 执行
      /// </summary>
      Executed = 4102,
      /// <summary>
      /// 医技确认
      /// </summary>
      Affirmed = 4103,
      /// <summary>
      /// 医技作废
      /// </summary>
      Abolished = 4104,
      /// <summary>
      /// 医技拒绝
      /// </summary>
      Rejected = 4105,
      /// <summary>
      /// 审核后取消
      /// </summary>
      Cancelled = 4106
   }

   /// <summary>
   /// 打印原因
   /// </summary>
   public enum PrintReason
   {
      /// <summary>
      /// 无
      /// </summary>
      None = 0,
      /// <summary>
      /// 续打
      /// </summary>
      Continue = 4200,
      /// <summary>
      /// 完整打印
      /// </summary>
      All = 4201,
      /// <summary>
      /// 第一次打印
      /// </summary>
      FirstTime = 4202,
      /// <summary>
      /// 纸张损坏
      /// </summary>
      PageDamaged = 4203,
      /// <summary>
      /// 已打印部分被修改
      /// </summary>
      PrintedChanged = 4204,
      /// <summary>
      /// 预览打印
      /// </summary>
      PreviewPrint = 4205,
      /// <summary>
      /// 其他理由
      /// </summary>
      Other = 4206
   }

   /// <summary>
   /// 数据操作类型
   /// </summary>
   public enum DataOperator
   {
      /// <summary>
      /// 创建
      /// </summary>
      New = 4301,
      /// <summary>
      /// 修改
      /// </summary>
      Modiffy = 4302,
      /// <summary>
      /// 删除
      /// </summary>
      Delete = 4303,
      /// <summary>
      /// 提交
      /// </summary>
      Submit = 4304,
      /// <summary>
      /// 审核
      /// </summary>
      Audit = 4305,
      /// <summary>
      /// 撤销提交
      /// </summary>
      DischargeSubmit = 4306
   }

   /// <summary>
   /// 体征数据类别
   /// </summary>
   public enum PhysicalSignKind
   {
      /// <summary>
      /// 体温
      /// </summary>
      Temperature = 4401,
      /// <summary>
      /// 脉搏
      /// </summary>
      Sphygmus = 4402,
      /// <summary>
      /// 心率
      /// </summary>
      HeartRate = 4403,
      /// <summary>
      /// 血压
      /// </summary>
      BloodPressure = 4404,
      /// <summary>
      /// 呼吸次数
      /// </summary>
      Breathe = 4405,
      /// <summary>
      /// 大便次数
      /// </summary>
      Stool = 4406,
      /// <summary>
      /// 身高
      /// </summary>
      Height = 4407,
      /// <summary>
      /// 体重
      /// </summary>
      Weight = 4408,
      /// <summary>
      /// 腹围
      /// </summary>
      GirthOfPaunch = 4409,
      /// <summary>
      /// 总出量
      /// </summary>
      TotalOutput = 4410,
      /// <summary>
      /// 尿量
      /// </summary>
      UrinaQuantity = 4411,
      /// <summary>
      /// 痰量
      /// </summary>
      SputumQuantity = 4412,
      /// <summary>
      /// 呕吐量
      /// </summary>
      VomitQuantity = 4413,
      /// <summary>
      /// 引流量
      /// </summary>
      ConductionQuantity = 4414,
      /// <summary>
      /// 总入量
      /// </summary>
      TotalInput = 4419,
      /// <summary>
      /// 舌象
      /// </summary>
      TongueMark = 4430,
      /// <summary>
      /// 脉象
      /// </summary>
      SphygmusMark = 4431
   }

   /// <summary>
   /// 药品用法类别
   /// </summary>
   public enum DragUsageKind
   {
      /// <summary>
      /// 普通
      /// </summary>
      Normal = 4500,
      /// <summary>
      /// 口服
      /// </summary>
      PO = 4501,
      /// <summary>
      /// 输液
      /// </summary>
      Transfusion = 4502,
      /// <summary>
      /// 针剂
      /// </summary>
      Ampule = 4503
   }

   /// <summary>
   /// 审阅状态
   /// </summary>
   public enum ExamineState
   {
      /// <summary>
      /// 未提交,
      /// </summary>
      NotSubmit = 4600,
      /// <summary>
      /// 提交但未审核,
      /// </summary>
      SubmitButNotExamine = 4601,
      /// <summary>
      /// 主治审核,
      /// </summary>
      FirstExamine = 4602,
      /// <summary>
      /// 主任审核
      /// </summary>
      SecondExamine = 4603,
      /// <summary>
      /// 已删除
      /// </summary>
      Deleted = 4604,
      /// <summary>
      /// 第三次审核
      /// </summary>
      ThirdExamine = 4610,
      /// <summary>
      /// 第四次审核
      /// </summary>
      FouthExamine = 4611,
      /// <summary>
      /// 第五次审核,
      /// </summary>
      FifthExamine = 4612,
      /// <summary>
      /// 最终审核
      /// </summary>
      Final = 4613
   }
}
