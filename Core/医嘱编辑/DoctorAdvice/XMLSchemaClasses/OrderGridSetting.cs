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
   /// 医嘱Grid的样式
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [DesignerCategoryAttribute("code")]
   [XmlTypeAttribute(AnonymousType = true)]
   [XmlRootAttribute(Namespace = "http://medical.DrectSoft.com", IsNullable = false)]
   public partial class OrderGridSetting
   {
      private TypeXmlFont _gridFont;
      private int _rowHeight;
      private bool _showBand;
      private GridSettingColumnBasic[] _columns;
      private TypeGridBand[] _longOrderSetting;
      private TypeGridBand[] _tempOrderSetting;

      /// <summary>
      /// Grid的默认字体
      /// </summary>
      [Category("字体"), DisplayName("默认字体"), Description("医嘱表格的默认字体")]
      public TypeXmlFont GridFont
      {
         get { return _gridFont; }
         set { _gridFont = value; }
      }

      /// <summary>
      /// Grid的默认行高
      /// </summary>
      [Category("显示"), DisplayName("默认行高"), Description("医嘱表格的默认行高,以像素为单位")]
      public int RowHeight
      {
         get { return _rowHeight; }
         set { _rowHeight = value; }
      }

      /// <summary>
      /// 是否显示Grid的Band
      /// </summary>
      [Category("显示"), DisplayName("显示表格的分栏"), Description("在医嘱表格中是否显示二级表头")]
      public bool ShowBand
      {
         get { return _showBand; }
         set { _showBand = value; }
      }

      /// <summary>
      /// Grid中的所有列
      /// </summary>
      [Category("显示"), DisplayName("允许显示的列"), Description("可以显示在医嘱表格中的列"), Browsable(false)]
      [XmlArrayItemAttribute("ColumnBasic", IsNullable = false)]
      public GridSettingColumnBasic[] Columns
      {
         get { return _columns; }
         set { _columns = value; }
      }

      /// <summary>
      /// 长期医嘱设置
      /// </summary>
      [Category("显示"), DisplayName("长期医嘱表头"), Description("长期医嘱表格需要显示的列及其顺序"), Browsable(false)]
      [XmlArrayItemAttribute("Band", IsNullable = false)]
      public TypeGridBand[] LongOrderSetting
      {
         get { return _longOrderSetting; }
         set { _longOrderSetting = value; }
      }

      /// <summary>
      /// 临时医嘱设置
      /// </summary>
      [Category("显示"), DisplayName("临时医嘱表头"), Description("临时医嘱表格需要显示的列及其顺序"), Browsable(false)]
      [XmlArrayItemAttribute("Band", IsNullable = false)]
      public TypeGridBand[] TempOrderSetting
      {
         get { return _tempOrderSetting; }
         set { _tempOrderSetting = value; }
      }

      /// <summary>
      /// 医嘱内容单元格的宽度
      /// </summary>
      [Category("显示"), DisplayName("医嘱内容列的宽度"), Description("以像素为单位"), Browsable(false)]
      public int WidthOfContentCell
      {
         get 
         {
            foreach (GridSettingColumnBasic col in _columns)
               if (col.Name == "UNContent")
                  return col.Width;
            return 250;
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="colName"></param>
      /// <returns></returns>
      public int GetColumnWidth(string colName)
      {
         foreach (GridSettingColumnBasic col in Columns)
            if (col.Name == colName)
               return col.Width;
         return -1;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="colName"></param>
      /// <returns></returns>
      public string GetColumnCaption(string colName)
      {
         foreach (GridSettingColumnBasic col in Columns)
            if (col.Name == colName)
               return col.Caption;
         return "";
      }
   }

   /// <summary>
   /// 列基础信息
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [DesignerCategoryAttribute("code")]
   [XmlTypeAttribute(AnonymousType = true)]
   public partial class GridSettingColumnBasic
   {
      private string _name;
      private string _caption;
      private int _width;
      private string _memo;

      /// <summary>
      /// 列名称
      /// </summary>
      [XmlAttributeAttribute()]
      [DisplayName("列名"), Description("")]
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// 列显示名称
      /// </summary>
      [XmlAttributeAttribute()]
      [DisplayName("显示名称"), Description("")]
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }      

      /// <summary>
      /// 列默认宽度
      /// </summary>
      [XmlAttributeAttribute()]
      [DisplayName("宽度"), Description("")]
      public int Width
      {
         get { return _width; }
         set { _width = value; }
      }

      /// <summary>
      /// 列说明
      /// </summary>
      [XmlAttributeAttribute()]
      [DisplayName("说明"), Description("")]
      public string Memo
      {
         get { return _memo; }
         set { _memo = value; }
      }
   }
}