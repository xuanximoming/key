using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 医嘱Grid自画所需设置
   /// </summary>
   [System.SerializableAttribute()]
   [System.Diagnostics.DebuggerStepThroughAttribute()]
   [System.ComponentModel.DesignerCategoryAttribute("code")]
   [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
   [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://medical.DrectSoft.com", IsNullable = false)]
   public partial class GridCustomDrawSetting
   {
      private TypeXmlFont _defaultFont;
      private int _minFontSize;
      private Padding _margin;
      private TypeXmlFont _fontOfCancel;
      private int _startPosOfCancel;
      private TypeCellColor _defaultColor;
      private TypeCellColor _newOrderColor;
      private TypeCellColor _auditedColor;
      private TypeCellColor _cancelledColor;
      private TypeCellColor _executedColor;
      private TypeCellColor _ceasedColor;
      private TypeCellColor _notSynchColor;
      private TypeColorPair _groupFlagColor;
      private int _groupFlagWidth;
      private bool _showRepeatInfo;
      private string _replaceOfRepeatInfo;

      /// <summary>
      /// 医嘱内容默认字体
      /// </summary>
      [Category("字体"), DisplayName("医嘱内容默认字体"), Description("根据需要单独设置医嘱内容的字体"), Browsable(true)]
      public TypeXmlFont DefaultFont
      {
         get { return _defaultFont; }
         set { _defaultFont = value; }
      }

      /// <summary>
      /// 医嘱内容最小字号
      /// </summary>
      [Category("字体"), DisplayName("医嘱内容最小字号"), Description("为尽可能完整的显示医嘱内容,对于过长的医嘱内容系统会自动缩小字号或折行显示"), Browsable(true)]
      public int MinFontSize
      {
         get { return _minFontSize; }
         set { _minFontSize = value; }
      }

      /// <summary>
      /// 允许输出的区域与单元格四周的距离
      /// </summary>
      [Category("显示"), DisplayName("医嘱内容输出区域与单元格四周的距离"), Description("为了美观,可以调整输出内容与边框的距离,以像素为单位"), Browsable(true)]
      public Padding Margin
      {
         get { return _margin; }
         set { _margin = value; }
      }

      /// <summary>
      /// "取消医嘱"的字体
      /// </summary>
      [Category("字体"), DisplayName("取消信息的字体"), Description("被取消医嘱的“取消”的字体"), Browsable(true)]
      public TypeXmlFont FontOfCancel
      {
         get { return _fontOfCancel; }
         set { _fontOfCancel = value; }
      }

      /// <summary>
      /// "取消医嘱"输出时的起始位置(相对于允许输出的区域)
      /// </summary>
      [Category("显示"), DisplayName("取消信息的输出位置"), Description("被取消医嘱的“取消”相对于原始医嘱内容的输出位置,以像素为单位"), Browsable(true)]
      public int StartPosOfCancel
      {
         get { return _startPosOfCancel; }
         set { _startPosOfCancel = value; }
      }

      /// <summary>
      /// 默认的颜色设置
      /// </summary>
      [Category("显示"), DisplayName("默认颜色"), Description("医嘱内容默认的字体颜色"), Browsable(false)]
      public TypeCellColor DefaultColor
      {
         get { return _defaultColor; }
         set { _defaultColor = value; }
      }

      /// <summary>
      /// 新医嘱的颜色设置
      /// </summary>
      [Category("显示"), DisplayName("新开医嘱的颜色"), Description(""), Browsable(false)]
      public TypeCellColor NewOrderColor
      {
         get { return _newOrderColor; }
         set { _newOrderColor = value; }
      }

      /// <summary>
      /// 已审核医嘱的颜色设置
      /// </summary>
      [Category("显示"), DisplayName("已审核医嘱的颜色"), Description(""), Browsable(false)]
      public TypeCellColor AuditedColor
      {
         get { return _auditedColor; }
         set { _auditedColor = value; }
      }

      /// <summary>
      /// 已执行医嘱的颜色设置
      /// </summary>
      [Category("显示"), DisplayName("已执行医嘱的颜色"), Description(""), Browsable(false)]
      public TypeCellColor ExecutedColor
      {
         get { return _executedColor; }
         set { _executedColor = value; }
      }

      /// <summary>
      /// 已停止医嘱的颜色设置
      /// </summary>
      [Category("显示"), DisplayName("已停止医嘱的颜色"), Description(""), Browsable(false)]
      public TypeCellColor CeasedColor
      {
         get { return _ceasedColor; }
         set { _ceasedColor = value; }
      }

      /// <summary>
      /// 未同步的数据的颜色设置
      /// </summary>
      [Category("显示"), DisplayName("未同步数据的颜色"), Description("未同步是指还没有发送到HIS"), Browsable(false)]
      public TypeCellColor NotSynchColor
      {
         get { return _notSynchColor; }
         set { _notSynchColor = value; }
      }

      /// <summary>
      /// "取消医嘱"的颜色设置
      /// </summary>
      [Category("显示"), DisplayName("被取消医嘱的颜色"), Description(""), Browsable(false)]
      public TypeCellColor CancelledColor
      {
         get { return _cancelledColor; }
         set { _cancelledColor = value; }
      }

      /// <summary>
      /// 分组标记的颜色
      /// </summary>
      [Category("显示"), DisplayName("分组标记的颜色"), Description(""), Browsable(true)]
      public TypeColorPair GroupFlagColor
      {
         get { return _groupFlagColor; }
         set { _groupFlagColor = value; }
      }

      /// <summary>
      /// 分组标记的宽度
      /// </summary>
      [Category("显示"), DisplayName("分组标记的线宽度"), Description("成套医嘱的分组线,以像素为单位"), Browsable(true)]
      public int GroupFlagWidth
      {
         get { return _groupFlagWidth; }
         set { _groupFlagWidth = value; }
      }

      /// <summary>
      /// 重复信息是否显示标记
      /// 对于开始日期、停止时间、创建者等信息，为界面简洁，连续的行如果信息重复则只需要在第一行显示完整信息，其它行可以不显示
      /// </summary>
      [Category("显示"), DisplayName("简化重复信息"), Description("启用时,对于开始日期、创建者、停止时间都相同的连续记录,除第一行外都不显示重复的信息"), Browsable(true)]
      public bool ShowRepeatInfo
      {
         get { return _showRepeatInfo; }
         set { _showRepeatInfo = value; }
      }

      /// <summary>
      /// 用来替换重复信息的字符串
      /// </summary>
      [Category("显示"), DisplayName("重复信息的替换字符串"), Description("用来替换重复信息，可以为空"), Browsable(true)]
      public string ReplaceOfRepeatInfo
      {
         get { return _replaceOfRepeatInfo; }
         set { _replaceOfRepeatInfo = value; }
      }

      /// <summary>
      /// 实际允许输出的区域尺寸(扣除了Margin)
      /// </summary>
      [XmlIgnore(), Browsable(false)]
      public Size OutputSizeOfContent
      {
         get { return _outputSizeOfContent; }
         set { _outputSizeOfContent = value; }
      }
      private Size _outputSizeOfContent;

      /// <summary>
      /// 取消医嘱的输出位置(相对于单元格的尺寸)
      /// </summary>
      [XmlIgnore(), Browsable(false)]
      public Rectangle BoundsOfCancel
      {
         get { return _boundsOfCancel; }
         set { _boundsOfCancel = value; }
      }
      private Rectangle _boundsOfCancel;
   }
}
