using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    /// <summary>
    /// mfb
    /// 电子病历文本文档的文本块对象.
    /// 因为继承自<c>ZYTextContainer</c>,为一个容器对象.
    /// <para>主要包含的内容有标题(Title),折叠开关(bolExpended),ToolTip,包围边框(BoxRect)</para>
    /// </summary>
    public class ZYTextDiv : ZYTextContainer
    {
        //private string	strKey			= null;
        ///// <summary>
        ///// 是否折叠
        ///// </summary>
        //private bool bolExpended = true;


        //System.Drawing.Rectangle BoxRect = System.Drawing.Rectangle.Empty;
        //private string strToolTip = null;

        ///// <summary>
        ///// div文本块标题
        ///// </summary>
        //private string strTitle = null;
        private ZYTextParagraph para;
        public ZYTextDiv()
        {
            para = new ZYTextParagraph();
            para.Parent = this;
            myChildElements.Add(para);

            #region mfb 测试单元格
            //TPTextTable table = new TPTextTable();
            //table.Parent = this;
            //table.Width = 2000;
            //table.Height = 500;


            //TPTextRow rows = new TPTextRow();
            //rows.Parent = table;

            //rows[0] = new TPTextCell();
            //rows[0].Parent = rows;
            //rows[1] = rows[0];
            //rows[2] = rows[0];
            //rows[3] = rows[0];


            //table[0] = rows;
            //table[1] = rows;
            //table[2] = rows;
            //table[3] = rows;
            //table[4] = rows;

            //myChildElements.Add(table);

            #endregion
        }

        /// <summary>
        /// Gets the owner div.
        /// </summary>
        /// <param name="myElement">My element.</param>
        /// <returns></returns>
        public static ZYTextDiv GetOwnerDiv(ZYTextElement myElement)
        {
            ZYTextDiv myDiv = null;
            while (myElement != null)
            {
                myDiv = myElement as ZYTextDiv;
                if (myDiv == null)
                    myElement = myElement.Parent;
                else
                    return myDiv;
            }
            return myDiv;
        }
        /// <summary>
        /// 可以包含文档内容
        /// </summary>
        public bool NoContent
        {
            get { return myAttributes.GetBool(ZYTextConst.c_NoContent); }
            set { myAttributes.SetValue(ZYTextConst.c_NoContent, value); }
        }

        #region override 基类方法和属性群

        #region left, height, width
        public override int Left
        {
            get
            {
                return 0;//myOwnerDocument.ContainerIndent;
            }
            set
            {
                if (myParent == null)
                    intLeft = value;
                else
                    intLeft = 0;
                //base.Left = value;
            }
        }

        #region 修改过

        public override int Height
        {
            get
            {
                //if (bolExpended)
                return base.Height;
                //else

                //return myOwnerDocument.DefaultRowHeight;
                //return (int) myLineHeight[0] ;
            }
        }

        public override System.Drawing.Rectangle GetContentBounds()
        {
            ResetBoxRect();
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(this.RealLeft, this.RealTop, base.intClientWidth, this.Height);
            int dx = rect.Left - BoxRect.Left;
            rect.X -= dx;
            rect.Width += dx;
            return rect;
        }

        /// <summary>
        /// Resets the box rect.
        /// </summary>
        private void ResetBoxRect()
        {
            BoxRect = myOwnerDocument.View.GetExpendHandleRect(this.RealLeft, this.RealTop, myOwnerDocument.DefaultRowHeight);
        }

        System.Drawing.Rectangle BoxRect = System.Drawing.Rectangle.Empty;

        #endregion

        public override int Width
        {
            get
            {
                //if( bolExpended )
                //	return base.Width ;
                //else
                if (myParent == null)
                    return myOwnerDocument.Pages.StandardWidth;// .View.Width ;// - this.Left - 5 ;
                else
                    return myParent.Width - this.Left;
            }
            set
            {
                base.Width = value;
            }
        }
        #endregion


        public override void AddElementToList(System.Collections.ArrayList myList, bool ResetFlag)
        {
            if (myList != null)
            {
                foreach (ZYTextElement myElement in myChildElements)
                {
                    if (!ResetFlag)
                    {
                        myElement.Visible = false;
                        myElement.Index = -1;
                    }
                    if (myOwnerDocument.isVisible(myElement) && myElement is ZYTextContainer)
                    {
                        myElement.Visible = true;
                        if (myElement is ZYTextParagraph)
                        {
                            (myElement as ZYTextParagraph).AddElementToList(myList, ResetFlag);
                        }
                        if (myElement is TPTextTable)
                        {
                            (myElement as TPTextTable).AddElementToList(myList, ResetFlag);
                        }
                    }
                }
            }
        }

        public override bool Locked
        {
            get
            {
                if (myOwnerDocument.Loading)
                    return false;
                //if( myOwnerDocument.Locked)
                //    return true;
                if (this.NoContent == true && myOwnerDocument.Info.DesignMode == false)
                    return true;
                return false;
            }
            set
            {
                base.Locked = value;
            }
        }

        /// <summary>
        /// 已重载,返回XML名称
        /// </summary>
        /// <returns></returns>
        public override string GetXMLName()
        {
            return ZYTextConst.c_Div;
        }

        public override bool OwnerWholeLine()
        {
            return true;
        }
        /// <summary>
        /// 容器对象默认进行强制换行
        /// </summary>
        /// <returns></returns>
        public override bool isNewLine()
        {
            return true;
        }
        /// <summary>
        /// 本对象是否强制开始新段落
        /// </summary>
        /// <returns></returns>
        public override bool isNewParagraph()
        {
            return true;
        }

        public override bool isField()
        {
            return true;
        }

        public override bool isTextElement()
        {
            return false;
        }

        #region RefreshView, RefreshLine

        /// <summary>
        /// 刷新界面,重新绘制对象
        /// </summary>
        /// <returns>是否进行了刷新操作</returns>
        public override bool RefreshView()
        {
            //return 
            base.RefreshView();

            //画部分编辑区域,红色框,
            if (this.OwnerDocument.OwnerControl.ActiveEditArea != null)
            {
                Debug.WriteLine("画编辑部分 画红框 ");
                Rectangle rectx = new Rectangle();
                //rectx.Location = this.OwnerDocument.OwnerControl.ActiveEditArea.TopElement.Bounds.Location;
                rectx.Location = new Point(0, this.OwnerDocument.OwnerControl.ActiveEditArea.Top);
                rectx.Width = this.OwnerDocument.OwnerControl.Pages.StandardWidth + 40;
                rectx.Height = this.OwnerDocument.OwnerControl.ActiveEditArea.End - this.OwnerDocument.OwnerControl.ActiveEditArea.Top;
                rectx.Offset(-20, 0);

                Pen p = new Pen(Brushes.Red);
                p.Width = 1;

                this.OwnerDocument.OwnerControl.EMRDoc.View.Graph.DrawRectangle(p, rectx);
            }

            return true;
        }
        /// <summary>
        /// 元素重新分行
        /// </summary>
        /// <returns></returns>
        public override ArrayList RefreshLine()
        {
            return base.RefreshLine();
        }
        #endregion

        public override bool HandleClick(int x, int y, System.Windows.Forms.MouseButtons Button)
        {
            return base.HandleClick(x, y, Button);
        }

        public override bool HandleMouseDown(int x, int y, System.Windows.Forms.MouseButtons Button)
        {
            return base.HandleMouseDown(x, y, Button);
        }

        public override bool HandleMouseMove(int x, int y, System.Windows.Forms.MouseButtons Button)
        {
            return base.HandleMouseMove(x, y, Button);
        }


        public override string ToEMRString()
        {
            //if (StringCommon.isBlankString(this.Title) == false)
            //    return this.Title + base.ToEMRString();
            //else
            return base.ToEMRString();
        }
        public override string ToString()
        {
            return "ZYTextDiv Name:" + myAttributes.GetString(ZYTextConst.c_Name)
                + " Childs:" + myChildElements.Count
                + " [" + this.RealLeft
                + "-:" + this.RealTop
                + " " + this.Width
                + "-" + this.Height;
        }

        #endregion

    }
}