using DrectSoft.Library.EmrEditor.Src.Gui;
using System;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    public class ZYTextEOF : ZYTextElement
    {
        public ZYTextEOF()
        {

        }
        public override bool CanBeLineHead()
        {
            return true;
        }
        public override bool isNewLine()
        {
            return true;
        }
        public override bool isNewParagraph()
        {
            return true;
        }

        private System.Drawing.Font myFont = null;
        /// <summary>
        /// 字体大小
        /// </summary>
        public float FontSize
        {
            get
            {
                return myAttributes.GetFloat(ZYTextConst.c_FontSize);
            }
            set
            {
                myAttributes.SetValue(ZYTextConst.c_FontSize, value);
                myFont = null;
            }
        }

        public override string GetXMLName()
        {
            return ZYTextConst.c_PEOF;
        }
        public override string ToEMRString()
        {
            return "\r\n";
        }

        private static System.Drawing.StringFormat myMeasureFormat = null;
        public override bool RefreshSize()
        {
            if (this.Parent != null && this == this.Parent.FirstElement)
            {
                if (myMeasureFormat == null)
                {
                    myMeasureFormat = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);
                    myMeasureFormat.FormatFlags = System.Drawing.StringFormatFlags.FitBlackBox | System.Drawing.StringFormatFlags.MeasureTrailingSpaces;
                }

                if (myFont == null)
                    myFont = myOwnerDocument.View._CreateFont
                        ("宋体",
                        this.FontSize,
                        false,
                        false,
                        false);
                System.Drawing.SizeF CharSize = myOwnerDocument.View.Graph.MeasureString("_", myFont, 10000, myMeasureFormat);

                intWidth = (int)CharSize.Width;
                intHeight = (int)Math.Ceiling(myFont.GetHeight(myOwnerDocument.View.Graph));
            }
            else
            {
                intWidth = myOwnerDocument.PixelToDocumentUnit(11);
                intHeight = myOwnerDocument.DefaultRowHeight;
                if (this.OwnerLine != null)
                {
                    if (myOwnerDocument.DefaultRowHeight > this.OwnerLine.Height)
                    {
                        intHeight = this.OwnerLine.Height;
                    }
                }
            }
            return true;
        }

        public override bool RefreshView()
        {
            if (myOwnerDocument.Info.Printing)
                return true;
            RefreshSize();

            int y = this.RealTop + (this.Height / 2);
            int x = 0;
            if (WeiWenProcess.weiwen)
            {
                //修正因为字号过大引起的回车在行首光标位置错位的问题 add by Ukey zhang 2017-11-11
                if (this.RealLeft + this.Width > this.OwnerLine.ContentWidth)
                    this.Left = this.OwnerLine.ContentWidth - this.Width;
                x = this.RealLeft + this.Width;
            }
            else
                x = this.RealLeft;


            if (myOwnerDocument.Info.ShowParagraphFlag)
                myOwnerDocument.View.DrawParagraphFlag(
                    x,
                   y,
                    GraphicsUnitConvert.GetRate(myOwnerDocument.DocumentGraphicsUnit, System.Drawing.GraphicsUnit.Pixel));
            return true;
        }


    }
}
