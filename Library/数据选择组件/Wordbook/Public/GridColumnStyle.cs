using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DrectSoft.Wordbook
{
   /// <summary>
   /// Grid中Column的配置信息(包含要ColumnName、显示名称、宽度等)
   /// </summary>
   [CLSCompliantAttribute(true)]
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public class GridColumnStyle
   {
      /// <summary>
      /// Grid中Column的ColumnName
      /// </summary>
      public string FieldName
      {
         get { return _fieldName; }
         set { _fieldName = value; }
      }
      private string _fieldName;

      /// <summary>
      /// Grid中Column的显示名称
      /// </summary>
      public string Caption
      {
         get { return _caption; }
         set { _caption = value; }
      }
      private string _caption;

      /// <summary>
      /// Grid中Column的初始宽度
      /// </summary>
      public int Width
      {
         get { return _width; }
         set { _width = value; }
      }
      private int _width;

      /// <summary>
      /// 创建Grid中列显示样式对象
      /// </summary>
      public GridColumnStyle()
      { }

      /// <summary>
      /// 创建Grid中列显示样式对象
      /// </summary>
      /// <param name="column"></param>
      /// <param name="title"></param>
      /// <param name="colWidth"></param>
      public GridColumnStyle(string column, string title, int colWidth)
      {
         _fieldName = column;
         _caption = title;
         _width = colWidth;
      }
   }
}
