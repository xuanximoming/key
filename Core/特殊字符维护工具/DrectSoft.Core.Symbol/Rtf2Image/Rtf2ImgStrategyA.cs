using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using YidanSoft.Common.Object2Editor.Encoding;

namespace YidanSoft.Core.Symbol
{
    /// <summary>
    /// Rtf2Image转换核心-API计算
    /// </summary>
    public class Rtf2ImgStrategyA : StrImgCStrategy
    {
        #region const and fields
        private const float MinFontSize = 6;

        private RichTextBox tempRichBox;

        #endregion

        #region ctor
        /// <summary>
        /// Rtf2ImageCore构造器
        /// </summary>
        public Rtf2ImgStrategyA()
        {
            tempRichBox = new RichTextBox();
        }
        #endregion

        #region override methods
        /// <summary>
        /// 实现StrImgCStrategy中的DoStrImgConvert
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        protected override Bitmap DoStrImgConvert(StrImgConvertWrap wrap)
        {
            //重现RTF视图
            if (wrap.str.StartsWith(RtfEncoding.RtfFlag))
                tempRichBox.Rtf = wrap.str;
            else
                tempRichBox.SelectedText = wrap.str;
            //生成图像
            return Format2Image(tempRichBox, wrap.size);
        }

        #endregion

        #region private method
        /// <summary>
        /// RTF文本块转制定大小图像
        /// </summary>
        /// <param name="rtb"></param>
        /// <param name="renderSize">给定的输出区域尺寸。单位：pixel</param>
        /// <returns></returns>
        private Bitmap Format2Image(RichTextBox rtb, Size renderSize)
        {
            Bitmap map = new Bitmap(renderSize.Width, renderSize.Height);
            //实现绘制
            using (Graphics graph = Graphics.FromImage(map))
            {
                //绘制准备
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                //绘制测量，调整字体以适应渲染区域
                Rectangle zRenderRect = new Rectangle(new Point(0, 0),
                    renderSize);
                float currentFontSize = zRenderRect.Height / 2;
                do
                {
                    tempRichBox.SelectAll();
                    //RtfPrintNativeMethods.SetSelectionSize(tempRichBox, currentFontSize);
                    //调整实际渲染区域大小
                    zRenderRect.Height = tempRichBox.SelectionFont.Height;
                    if (FormatRangeCore(tempRichBox, true, graph, zRenderRect) ||
                        currentFontSize < MinFontSize)
                        break;
                    // 减小半号字体
                    currentFontSize -= 0.5f;
                } while (true);
                //实际绘制
                zRenderRect.Offset(
                    0, (renderSize.Height - tempRichBox.SelectionFont.Height) / 2);//调整实际渲染位置
                FormatRangeCore(rtb, false, graph, zRenderRect);
            }
            //透明化处理
            map.MakeTransparent(Color.White);
            return map;
        }
        #endregion

        protected Rectangle DoScaleZoom(Rectangle zoomTarget, float zoomFactor)
        {
            return base.DoRenderZoom(zoomTarget, zoomFactor, zoomFactor);
        }
    }
}