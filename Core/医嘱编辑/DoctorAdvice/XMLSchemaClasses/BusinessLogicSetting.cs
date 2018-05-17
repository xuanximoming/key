using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 业务逻辑设置
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [DesignerCategoryAttribute("code")]
   [XmlTypeAttribute(AnonymousType = true)]
   [XmlRootAttribute(Namespace = "http://medical.DrectSoft.com", IsNullable = false)]
   public partial class BusinessLogicSetting
   {
      #region bussiness
      /// <summary>
      /// 默认的临时医嘱频次(ST)代码
      /// </summary>
      [Category("业务"), DisplayName("临时医嘱默认频次代码"), Description("来自HIS中的医嘱频次代码")]
      public string TempOrderFrequencyCode
      {
         get { return _tempOrderFrequencyCode; }
         set { _tempOrderFrequencyCode = value; }
      }
      private string _tempOrderFrequencyCode;

      /// <summary>
      /// 新医嘱的开始时间在当前时间之前多长时间就进行提示。以小时为单位
      /// </summary>
      [Category("业务"), DisplayName("医嘱最早开始时间"), Description("开设新医嘱时允许开始时间早于当前时间，但如果超过的设定的值，则系统会在保存医嘱时进行提示。以小时为单位")]
      public int StartDateWarningHours
      {
         get { return _startDateWarningHours; }
         set { _startDateWarningHours = value; }
      }
      private int _startDateWarningHours;

      /// <summary>
      /// 是否允许在长期医嘱中输入草药
      /// </summary>
      [Category("业务"), DisplayName("允许在长期医嘱中输入草药"), Description("")]
      public bool AllowLongHerbOrder
      {
         get { return _allowLongHerbOrder; }
         set { _allowLongHerbOrder = value; }
      }
      private bool _allowLongHerbOrder;

      /// <summary>
      /// 启用项目使用年龄警告
      /// </summary>
      [Category("业务"), DisplayName("启用项目使用年龄警告"), Description("对特殊项目提醒注意使用年龄")]
      public bool EnableItemAgeWarning
      {
         get { return _enableItemAgeWarning; }
         set { _enableItemAgeWarning = value; }
      }
      private bool _enableItemAgeWarning;

      /// <summary>
      /// 项目使用年龄警告上限(在上限以下的都警告)
      /// </summary>
      [Category("业务"), DisplayName("项目使用年龄警告上限"), Description("启用项目使用年龄警告才有效，在上限以下的都警告")]
      public int MaxWarningAge
      {
         get { return _maxWarningAge; }
         set { _maxWarningAge = value; }
      }
      private int _maxWarningAge;

      /// <summary>
      /// 需要进行使用年龄控制的项目代码，多个时用"，"隔开
      /// </summary>
      [Category("业务"), DisplayName("进行使用年龄控制的项目代码"), Description("启用项目使用年龄警告才有效，多个时用','隔开")]
      public string WaringItem
      {
         get { return _waringItem; }
         set { _waringItem = value; }
      }
      private string _waringItem;

      /// <summary>
      /// 病区所有病人都使用电子医嘱
      /// </summary>
      [Category("业务"), DisplayName("病区所有病人都使用电子医嘱"), Description("设为否时，医嘱同步过程将忽略对EMR中没有医嘱数据的病人的处理"), DefaultValue(true)]
      public bool UsedForAllPatient
      {
         get { return _usedForAllPatient; }
         set { _usedForAllPatient = value; }
      }
      private bool _usedForAllPatient;

      /// <summary>
      /// 领药截止时间
      /// </summary>
      [Category("业务"), DisplayName("领药截止时间"), Description("领药截止时间(处理”明起“的长期医嘱时需要此参数)"), DefaultValue(8)]
      public int BlockingTimeOfTakeDrug
      {
         get { return _blockingTimeOfTakeDrug; }
         set
         {
            if ((value < 0) || (value > 23))
               _blockingTimeOfTakeDrug = 8;
            _blockingTimeOfTakeDrug = value;
         }
      }
      private int _blockingTimeOfTakeDrug = 8;

      /// <summary>
      /// 启用电子病历系统的医嘱模块
      /// </summary>
      [Category("业务"), DisplayName("启用电子病历系统的医嘱模块"), Description("若选否，则查询医嘱时从HIS读取数据"), DefaultValue(false)]
      public bool EnableEmrOrderModul
      {
         get { return _enableEmrOrderModul; }
         set { _enableEmrOrderModul = value; }
      }
      private bool _enableEmrOrderModul;
      #endregion

      #region interface
      /// <summary>
      /// 是否连接到HIS
      /// </summary>
      [Category("接口"), DisplayName("与HIS联网"), Description("")]
      public bool ConnectToHis
      {
         get { return _connectToHis; }
         set { _connectToHis = value; }
      }
      private bool _connectToHis;

      /// <summary>
      /// 保存新医嘱时是否自动同步到HIS（其它修改仍自动同步）。为否时要手工点提交按钮
      /// </summary>
      [Category("接口"), DisplayName("保存时自动发送到HIS"), Description("为否时,需要点提交按钮才会将修改后的医嘱发送到HIS")]
      public bool AutoSyncData
      {
         get { return _autoSyncData; }
         set { _autoSyncData = value; }
      }
      private bool _autoSyncData;

      /// <summary>
      /// 启用处方规则(需要HIS提供相应设置数据)
      /// </summary>
      [Category("接口"), DisplayName("启用处方规则"), Description("与THIS4相连才有效。使用的是HIS中设置的处方规则数据")]
      public bool EnableOrderRules
      {
         get { return _enableOrderRules; }
         set { _enableOrderRules = value; }
      }
      private bool _enableOrderRules;

      /// <summary>
      /// 设置受限制明细(启用处方规则时才有效)
      /// </summary>
      [Category("接口"), DisplayName("设置受限制明细"), Description("启用处方规则时才有效")]
      public bool SetLimitedDetail
      {
         get { return _setLimitedDetail; }
         set { _setLimitedDetail = value; }
      }
      private bool _setLimitedDetail;
      #endregion

      #region UI
      /// <summary>
      /// 医嘱类别输入模式。Fasle: 使用LookUpEditor  True: 使用单选框模式
      /// </summary>
      [Category("界面"), DisplayName("单选框方式选择医嘱类别"), Description("编辑新医嘱时选择医嘱类别的方式, Ture:单选框模式, False:选择模式")]
      public bool UseRadioCatalogInputStyle
      {
         get { return _useRadioCatalogInputStyle; }
         set { _useRadioCatalogInputStyle = value; }
      }
      private bool _useRadioCatalogInputStyle;

      /// <summary>
      /// 是否自动隐藏医嘱中的草药明细(隐藏后将显示成一条汇总记录)
      /// </summary>
      [Category("界面"), DisplayName("自动隐藏医嘱中的草药明细"), Description("若自动隐藏，则默认将一组草药合并成一条汇总记录来显示")]
      public bool AutoHideHerbDetail
      {
         get { return _autoHideHerbDetail; }
         set { _autoHideHerbDetail = value; }
      }
      private bool _autoHideHerbDetail;
      #endregion

      #region others
      /// <summary>
      /// 标记是否使用美康的合理用药插件
      /// </summary>
      [Category("其它"), DisplayName("启用美康的合理用药插件"), Description("")]
      public bool UseMedicomPlug
      {
         get { return _useMedicomPlug; }
         set { _useMedicomPlug = value; }
      }
      private bool _useMedicomPlug;

      /// <summary>
      /// 使用空白的医嘱打印模板
      /// </summary>
      [Category("其它"), DisplayName("使用空白的医嘱打印模板"), Description("设定在空白纸张上打印医嘱还是用套打方式打印医嘱")]
      public bool UseEmptyOrderTemplate
      {
         get { return _useEmptyOrderTemplate; }
         set { _useEmptyOrderTemplate = value; }
      }
      private bool _useEmptyOrderTemplate;
      #endregion
   }
}