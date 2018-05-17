using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Library.EmrEditor.Src.Gui;

namespace DrectSoft.Library.EmrEditor.Src.Print
{
    /// <summary>
    /// ReportPageTransform 的摘要说明。
    /// </summary>
    public class MultiPageTransform : MultiRectangleTransform
    {
        public MultiPageTransform()
        {
        }

        protected PrintPageCollection myPages = null;
        /// <summary>
        /// 页面集合
        /// </summary>
        public PrintPageCollection Pages
        {
            get { return myPages; }
            set { myPages = value; }
        }

        /// <summary>
        /// 根据页面位置添加矩形区域转换关系
        /// </summary>
        /// <param name="myTransform">转换列表</param>
        /// <param name="ForPrint">是否为打印进行填充</param>
        public void AddPage(PrintPage page, int PageTop, float ZoomRate)
        {
            System.Drawing.Rectangle rect = System.Drawing.Rectangle.Empty;

            int leftmargin = (int)(myPages.LeftMargin * ZoomRate);
            int topmargin = (int)(myPages.TopMargin * ZoomRate);
            int rightmargin = (int)(myPages.RightMargin * ZoomRate);
            int bottommargin = (int)(myPages.BottomMargin * ZoomRate);
            int pagewidth = (int)(myPages.PaperWidth * ZoomRate);
            int pageheight = (int)(myPages.PaperHeight * ZoomRate);

            int Top = PageTop + topmargin;

            SimpleRectangleTransform item = null;
            if (myPages.HeadHeight > 0)
            {
                item = new SimpleRectangleTransform();
                item.PageIndex = page.Index;
                item.Flag2 = 0;
                item.Tag = page;
                item.DescRect = new System.Drawing.Rectangle(
                    0,
                    0,
                    page.Width,
                    myPages.HeadHeight);

                Top = SetSourceRect(item, ZoomRate, leftmargin, Top);

                this.Add(item);
            }

            item = new SimpleRectangleTransform();
            item.PageIndex = page.Index;
            item.Flag2 = 1;
            item.Tag = page;
            item.DescRect = new System.Drawing.Rectangle(
                0,
                page.Top,
                page.Width,
                page.Height);

            Top = SetSourceRect(item, ZoomRate, leftmargin, Top);

            this.Add(item);

            if (myPages.FooterHeight > 0)
            {
                item = new SimpleRectangleTransform();
                item.PageIndex = page.Index;
                item.Flag2 = 2;
                item.Tag = page;

                item.DescRect = new System.Drawing.Rectangle(
                    0,
                    myPages.DocumentHeight - myPages.FooterHeight,
                    page.Width,
                    myPages.FooterHeight);
                SetSourceRect(item, ZoomRate, leftmargin, Top);
                rect = item.SourceRect;

                Top = PageTop + pageheight - bottommargin - rect.Height;
                item.SourceRect = new System.Drawing.Rectangle(leftmargin, Top, rect.Width, rect.Height);

                this.Add(item);
            }

        }

        public void Refresh(float ZoomRate, int PageSplitBlank)
        {
            int leftmargin = (int)(myPages.LeftMargin * ZoomRate);
            int pageheight = (int)(myPages.PaperHeight * ZoomRate);

            mySourceOffsetBack = System.Drawing.Point.Empty;
            this.Clear();
            int iCount = 0;
            foreach (PrintPage page in myPages)
            {
                int PageTop = (pageheight + PageSplitBlank) * iCount + PageSplitBlank;
                iCount++;

                AddPage(page, PageTop, ZoomRate);

            }//foreach( PrintPage page in myDocument.Pages )
            //this.OffsetSource( leftmargin , 0 , false );
        }

        private int SetSourceRect(SimpleRectangleTransform item, float ZoomRate, int left, int Top)
        {
            System.Drawing.RectangleF rect = System.Drawing.Rectangle.Empty;
            rect.X = left;
            rect.Y = Top;
            rect.Width = (item.DescRectF.Width * ZoomRate);
            rect.Height = (item.DescRectF.Height * ZoomRate);
            item.SourceRectF = rect;
            return (int)Math.Ceiling(Top + rect.Height);
        }

        /// <summary>
        /// 是否使用绝对点左边转换模式
        /// </summary>
        protected bool bolUseAbsTransformPoint = false;
        /// <summary>
        /// 是否使用绝对点左边转换模式
        /// </summary>
        public bool UseAbsTransformPoint
        {
            get { return bolUseAbsTransformPoint; }
            set { bolUseAbsTransformPoint = value; }
        }
        public override System.Drawing.Point TransformPoint(int x, int y)
        {
            if (this.bolUseAbsTransformPoint)
            {
                return AbsTransformPoint(x, y);
            }
            else
            {
                return base.TransformPoint(x, y);
            }
        }

        public override bool ContainsSourcePoint(int x, int y)
        {
            if (this.bolUseAbsTransformPoint)
                return true;
            else
                return base.ContainsSourcePoint(x, y);
        }

        public System.Drawing.Point AbsTransformPoint(int x, int y)
        {
            SimpleRectangleTransform pre = null;
            SimpleRectangleTransform next = null;
            SimpleRectangleTransform cur = null;

            foreach (SimpleRectangleTransform item in this)
            {
                if (item.Enable == false)
                    continue;
                if (item.SourceRect.Contains(x, y))
                    return item.TransformPoint(x, y);

                if (y >= item.SourceRectF.Top && y <= item.SourceRectF.Bottom)
                {
                    cur = item;
                    break;
                }
                if (y < item.SourceRectF.Top)
                {
                    if (next == null || item.SourceRectF.Top < next.SourceRectF.Top)
                        next = item;
                }
                if (y > item.SourceRectF.Bottom)
                {
                    if (pre == null || item.SourceRectF.Bottom > pre.SourceRectF.Bottom)
                        pre = item;
                }
            }
            if (cur == null)
            {
                if (pre != null)
                    cur = pre;
                else
                    cur = next;
            }
            if (cur == null)
                return System.Drawing.Point.Empty;
            System.Drawing.Point p = new System.Drawing.Point(x, y);
            p = Common.RectangleCommon.MoveInto(p, cur.SourceRect);
            return cur.TransformPoint(p);
        }
    }//public class MultiPageTransform : MultiRectangleTransform
}
