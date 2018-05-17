using System;
using System.Drawing;

namespace DrectSoft.Core.Printing
{
   /// <summary>
   /// 图形度量刻度定义类
   /// </summary>
   public class GraphicsDpis
   {


      /// <summary>
      /// 1/75 英寸
      /// </summary>
      public static readonly float Display = 75f;
      /// <summary>
      /// 文档单位（1/300 英寸）
      /// </summary>
      public static readonly float Document = 300f;
      /// <summary>
      /// 百分之一英寸
      /// </summary>
      public static readonly float HundredthsOfAnInch = 100f;
      /// <summary>
      /// 英寸
      /// </summary>
      public static readonly float Inch = 1f;
      /// <summary>
      /// 毫米
      /// </summary>
      public static readonly float Millimeter = 25.4f;
      /// <summary>
      /// 设备像素
      /// </summary>
      public static readonly float Pixel = 96f;
      /// <summary>
      /// 打印机点
      /// </summary>
      public static readonly float Point = 72f;
      /// <summary>
      /// 十分之一毫米
      /// </summary>
      public static readonly float TenthsOfAMillimeter = 254f;
      /// <summary>
      /// Twips
      /// </summary>
      public static readonly float Twips = 1440f;
      /// <summary>
      /// 厘米
      /// </summary>
      public static readonly float Centimeter = 2.54f;

      static GraphicsDpis()
      {
         Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
         Pixel = graphics.DpiX;
         graphics.Dispose();
      }

      /// <summary>
      /// 根据度量单位刻度，获得GraphicsUnit枚举值
      /// </summary>
      /// <param name="dpi"></param>
      /// <returns></returns>
      public static GraphicsUnit DpiToUnit(float dpi)
      {
         if (dpi.Equals(Display))
         {
            return GraphicsUnit.Display;
         }
         if (dpi.Equals(Inch))
         {
            return GraphicsUnit.Inch;
         }
         if (dpi.Equals(Document))
         {
            return GraphicsUnit.Document;
         }
         if (dpi.Equals(Millimeter))
         {
            return GraphicsUnit.Millimeter;
         }
         if (dpi.Equals(Pixel))
         {
            return GraphicsUnit.Pixel;
         }
         if (!dpi.Equals(Point))
         {
            throw new ArgumentException("dpi");
         }
         return GraphicsUnit.Point;
      }

      /// <summary>
      /// 获取GraphicsUnit对应的度量刻度
      /// </summary>
      /// <param name="unit"></param>
      /// <returns></returns>
      public static float UnitToDpi(GraphicsUnit unit)
      {
         switch (unit)
         {
            case GraphicsUnit.Display:
               return Display;

            case GraphicsUnit.Pixel:
               return Pixel;

            case GraphicsUnit.Point:
               return Point;

            case GraphicsUnit.Inch:
               return Inch;

            case GraphicsUnit.Document:
               return Document;

            case GraphicsUnit.Millimeter:
               return Millimeter;
         }
         throw new ArgumentException("unit");
      }
   }
}

