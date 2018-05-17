using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 更新内容标记（哪些内容被更新了）
   /// </summary>
   [Flags]
   internal enum UpdateContentFlag
   {
      /// <summary>
      /// 开始时间
      /// </summary>
      StartDate = 0x01, 
      /// <summary>
      /// 医嘱内容
      /// </summary>
      Content = 0x02,   
      /// <summary>
      /// 停止时间
      /// </summary>
      CeaseDate = 0x04  
   }

   /// <summary>
   /// 编辑操作标志
   /// </summary>
   [Flags]
   internal enum EditProcessFlag
   {
      /// <summary>
      /// 删除
      /// </summary>
      Delete = 0x01, 
      /// <summary>
      /// 取消
      /// </summary>
      Cancel = 0x02, 
      /// <summary>
      /// 停止
      /// </summary>
      Cease = 0x04,  
      /// <summary>
      /// 上移
      /// </summary>
      MoveUp = 0x08, 
      /// <summary>
      /// 下移
      /// </summary>
      MoveDown = 0x10,  
      /// <summary>
      /// 成组
      /// </summary>
      SetGroup = 0x20,  
      /// <summary>
      /// 组开始 
      /// </summary>
      GroupStart = 0x40,   
      /// <summary>
      /// 组结束
      /// </summary>
      GroupEnd = 0x80,  
      /// <summary>
      /// 取消分组
      /// </summary>
      CancelGroup = 0x100, 
      /// <summary>
      /// 保存
      /// </summary>
      Save = 0x200,  
      /// <summary>
      /// 审核
      /// </summary>
      Audit = 0x400, 
      /// <summary>
      /// 执行
      /// </summary>
      Execute = 0x800,   
      /// <summary>
      /// 可以多选(未用)
      /// </summary>
      StartMultiSelect = 0x1000,
      /// <summary>
      /// 允许剪切
      /// </summary>
      Cut = 0x2000,
      /// <summary>
      /// 允许复制
      /// </summary>
      Copy = 0x4000,
      /// <summary>
      /// 允许粘贴
      /// </summary>
      Paste = 0x8000,
      /// <summary>
      /// 是草药汇总记录
      /// </summary>
      IsHerbSummary = 0x10000,
      /// <summary>
      /// 是草药明细记录
      /// </summary>
      IsHerbDetail = 0x20000
   }

   /// <summary>
   /// 医嘱内容各属性对应的编辑器是否可用标志
   /// </summary>
   [Flags]
   internal enum EditorEnableFlag
   { 
      /// <summary>
      /// 可以选择项目
      /// </summary>
      CanChoiceItem = 0x01, 
      /// <summary>
      /// 可以选择用法
      /// </summary>
      CanChoiceUsage = 0x02, 
      /// <summary>
      /// 可以选择频次(不能选频次时，默认为不需要用法，数量也默认为1)
      /// </summary>
      CanChoiceFrequency = 0x04, 
      /// <summary>
      /// 需要输入数量
      /// </summary>
      NeedInputAmount = 0x08,
      /// <summary>
      /// 需要出院带药的信息
      /// </summary>
      NeedOutDruggeryInfo =0x10, 
      /// <summary>
      /// 需要转科的信息
      /// </summary>
      NeedShiftDeptInfo = 0x20, 
      /// <summary>
      /// 需要手术信息
      /// </summary>
      NeedOperationInfo = 0x40, 
      /// <summary>
      /// 需要出院时间
      /// </summary>
      NeedLeaveHospitalTime = 0x80, 
      /// <summary>
      /// 可以输入嘱托
      /// </summary>
      CanInputEntrust = 0x100,
      /// <summary>
      /// 可以设置执行天数
      /// </summary>
      CanSetExecuteDays = 0x200
   }

   /// <summary>
   /// 被选中记录所具有的特性
   /// </summary>
   [Flags]
   internal enum AttributeOfSelectedFlag
   {
      /// <summary>
      /// 包含已取消医嘱
      /// </summary>
      HasCancelled = 0x001, 
      /// <summary>
      /// 包含已停止医嘱
      /// </summary>
      HasCeased = 0x002, 
      /// <summary>
      /// 医嘱需要是连续的
      /// </summary>
      NumIsSerial = 0x004, 
      /// <summary>
      /// 包含已分组的记录
      /// </summary>
      HasGrouped = 0x008, 
      /// <summary>
      /// 包含了一组中的部分记录，用来判断同组的记录被全部选中
      /// </summary>
      HasPieceOfGroup = 0x010, 
      /// <summary>
      /// 有处于所有新增记录中的第一条的，在第一条新增之前也认为true
      /// </summary>
      HasFirstNew = 0x020,
      /// <summary>
      /// 有处于所有新增记录中的最后一条的
      /// </summary>
      HasLastNew = 0x040,
      /// <summary>
      /// 包含特殊医嘱(术后、转科、出院、草药汇总信息等),影响到医嘱是否能移位
      /// </summary>
      HasSpecial = 0x080,
      /// <summary>
      /// 包含出院医嘱
      /// </summary>
      HasLeaveHospital = 0x100, 
      /// <summary>
      /// 出院带药医嘱是否全部选中了
      /// </summary>
      SelectedAllOutDurg = 0x200, 
      /// <summary>
      /// 全部是草药内容
      /// </summary>
      AllIsHerbDruggery = 0x400,
      /// <summary>
      /// 全部是西药和成药内容
      /// </summary>
      AllIsOtherDruggery = 0x800,
      /// <summary>
      /// 有设过停止时间的
      /// </summary>
      HasCeaseInfo = 0x1000,
      /// <summary>
      /// 有关联申请单的
      /// </summary>
      HasLinkToApply = 0x2000,
      /// <summary>
      /// 有草药汇总信息记录
      /// </summary>
      HasHerbSummary = 0x4000,
      /// <summary>
      /// 在同一组中
      /// </summary>
      InSameGroup = 0x8000
   }

   /// <summary>
   /// OrderGrid Band的名称
   /// </summary>
   [SerializableAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public enum OrderGridBandName
   {
      /// <remarks/>
      bandBeginInfo,
      /// <remarks/>
      bandAuditInfo,
      /// <remarks/>
      bandExecuteInfo,
      /// <remarks/>
      bandCeaseInfo
   }

   /// <summary>
   /// 皮试结果标志
   /// </summary>
   public enum SkinTestResultKind
   { 
      /// <summary>
      /// 阳性
      /// </summary>
      Plus,
      /// <summary>
      /// 阴性
      /// </summary>
      Minus,
      /// <summary>
      /// 阴性(3天有效)
      /// </summary>
      MinusTreeDay
   }

   /// <summary>
   /// 记录状态（用来表示数据编辑动作类型）
   /// </summary>
   public enum RecordState
   { 
      /// <summary>
      /// 增加
      /// </summary>
      Added,
      /// <summary>
      /// 修改
      /// </summary>
      Modified,
      /// <summary>
      /// 删除
      /// </summary>
      Deleted,
      /// <summary>
      /// 取消
      /// </summary>
      Cancelled
   }

   /// <summary>
   /// 数据提交类型
   /// </summary>
   public enum DataCommitType
   { 
      /// <summary>
      /// 添加
      /// </summary>
      Add,
      /// <summary>
      /// 插入
      /// </summary>
      Insert,
      /// <summary>
      /// 修改
      /// </summary>
      Modify
   }

   /// <summary>
   /// 医嘱编辑器调用模式
   /// </summary>
   public enum EditorCallModel
   { 
      /// <summary>
      /// 编辑病人医嘱
      /// </summary>
      EditOrder,
      /// <summary>
      /// 查询病人医嘱
      /// </summary>
      Query,
      /// <summary>
      /// 编辑成套医嘱内容
      /// </summary>
      EditSuite
   }
}
