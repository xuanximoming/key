﻿using DrectSoft.Library.EmrEditor.Src.Gui;

namespace DrectSoft.Library.EmrEditor.Src.Print
{
    public class XPaperSize
    {
        public XPaperSize()
        {
        }
        public XPaperSize(System.Drawing.Printing.PaperSize size)
        {
            intKind = size.Kind;
            intWidth = size.Width;
            intHeight = size.Height;
        }
        public XPaperSize(System.Drawing.Printing.PaperKind vKind, int vWidth, int vHeight)
        {
            intKind = vKind;
            intWidth = vWidth;
            intHeight = vHeight;
        }
        private System.Drawing.Printing.PaperKind intKind = System.Drawing.Printing.PaperKind.A4;
        public System.Drawing.Printing.PaperKind Kind
        {
            get { return intKind; }
            set { intKind = value; }
        }

        private float _fontsize;
        /// <summary>
        /// 字体大小
        /// </summary>
        public float fontsize
        {
            get { return _fontsize; }
            set { _fontsize = value; }
        }
        private int intWidth = 0;
        /// <summary>
        /// 纸张宽度 单位百分之一英寸
        /// </summary>
        public int Width
        {
            get { return intWidth; }
            set
            { intWidth = value; }
        }
        public static double GetRate(System.Drawing.GraphicsUnit unit)
        {
            return GraphicsUnitConvert.GetRate(unit, System.Drawing.GraphicsUnit.Document) / 3.0;
        }
        public double GetWidth(System.Drawing.GraphicsUnit unit)
        {
            return GraphicsUnitConvert.Convert(intWidth * 3, System.Drawing.GraphicsUnit.Document, unit);
        }
        public void SetWidth(double vWidth, System.Drawing.GraphicsUnit unit)
        {
            intWidth = (int)(GraphicsUnitConvert.Convert(vWidth, unit, System.Drawing.GraphicsUnit.Document) / 3.0);
        }
        public double GetHeight(System.Drawing.GraphicsUnit unit)
        {
            return (int)GraphicsUnitConvert.Convert(intHeight * 3, System.Drawing.GraphicsUnit.Document, unit);
        }
        public void SetHeight(double vHeight, System.Drawing.GraphicsUnit unit)
        {
            intHeight = (int)(GraphicsUnitConvert.Convert(vHeight, unit, System.Drawing.GraphicsUnit.Document) / 3.0);
        }

        private int intHeight = 0;
        /// <summary>
        /// 纸张高度 单位百分之一英寸
        /// </summary>
        public int Height
        {
            get { return intHeight; }
            set
            { intHeight = value; }
        }
        public System.Drawing.Printing.PaperSize StdSize
        {
            get
            {
                return new System.Drawing.Printing.PaperSize(intKind.ToString(), this.intWidth, this.intHeight);
            }
            set
            {
                if (value != null)
                {
                    intKind = value.Kind;
                    intWidth = value.Width;
                    intHeight = value.Height;
                }
            }
        }
        public override string ToString()
        {
            return this.intKind.ToString();
        }

    }//public class XPaperSize
}
