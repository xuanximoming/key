using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace DrectSoft.Common.Eop
{
   /// <summary>
   /// 输出信息结构体，包括输出的文本、区域以及字体
   /// </summary>
   public struct OutputInfoStruct
   {
      #region properties
      /// <summary>
      /// 输出文本(如果类型是分组标记，则输出文本为空)
      /// </summary>
      public string Text
      {
         get 
         {
            if (_text == null)
               return "";
            return _text; 
         }
         set { _text = value; }
      }
      private string _text;

      /// <summary>
      /// 输出范围(或者是分组标记矩形框的范围)
      /// </summary>
      public Rectangle Bounds
      {
         get { return _bounds; }
         set { _bounds = value; }
      }
      private Rectangle _bounds;

      /// <summary>
      /// 输出使用的字体
      /// </summary>
      public Font Font
      {
         get { return _font; }
         set { _font = value; }
      } 
      private Font _font;

      /// <summary>
      /// 输出内容类型
      /// </summary>
      public OrderOutputTextType OutputType
      {
         get { return _outputType; }
         set { _outputType = value; }
      }
      private OrderOutputTextType _outputType;
      #endregion

      #region ctor
      /// <summary>
      /// 
      /// </summary>
      /// <param name="text"></param>
      /// <param name="bounds"></param>
      /// <param name="font"></param>
      /// <param name="outputType"></param>
      public OutputInfoStruct(string text, Rectangle bounds, Font font, OrderOutputTextType outputType)
      {
         _text = text;
         _bounds = bounds;
         _font = font;
         _outputType = outputType;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="text"></param>
      /// <param name="outputType"></param>
      public OutputInfoStruct(string text, OrderOutputTextType outputType)
      {
         _text = text;
         _outputType = outputType;
         _bounds = new Rectangle();
         _font = new Font("SimSun", 10.5F, FontStyle.Regular, GraphicsUnit.Point
            , ((byte)(134)));
      }
      #endregion

      #region public method
      /// <summary>
      /// 确定两个对象是否具有相同的值
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public override bool Equals(object obj)
      {
         if (obj is OutputInfoStruct)
         {
            OutputInfoStruct aimObj = (OutputInfoStruct)obj;
            return ((aimObj.Text == Text) && (aimObj.Bounds == Bounds)
               && (aimObj.Font == Font)
               && (aimObj.OutputType == OutputType));
         }
         return false;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="output1"></param>
      /// <param name="output2"></param>
      /// <returns></returns>
      public static bool operator ==(OutputInfoStruct output1, OutputInfoStruct output2)
      {
         return output1.Equals(output2);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="output1"></param>
      /// <param name="output2"></param>
      /// <returns></returns>
      public static bool operator !=(OutputInfoStruct output1, OutputInfoStruct output2)
      {
         return !(output1 == output2);
      }

      /// <summary>
      /// 返回该实例的哈希代码
      /// </summary>
      /// <returns></returns>
      public override int GetHashCode()
      {
         return Text.GetHashCode();
      }

      /// <summary>
      /// 获取对象的 Expression（如果存在的话）
      /// </summary>
      /// <returns></returns>
      public override string ToString()
      {
         return Text;
      }
      #endregion
   }
}
