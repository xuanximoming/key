using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    /// <summary>
    /// 表示一个单元格.
    /// </summary>
    public class TPTextCell : ZYTextContainer
    {
        #region 私有变量

        ///<summary> 列分割数 </summary>
        protected int colspan = 1;

        ///<summary> 行分割数 </summary>
        protected int rowspan = 1;

        ///<summary> 单元格水平对齐方式 </summary>
        protected int horizontalAlignment = -1;

        ///<summary> 单元格垂直对齐方式 </summary>
        protected int verticalAlignment = -1;

        ///<summary> 单元格选择状态 </summary>
        protected bool bolSelected = false;
        ///<summary> 是否可以操作 </summary>
        protected bool canAccess = false;

        ///<summary> 单元格宽度 </summary>
        private int cellWidth;

        ///<summary> 单元格高度 </summary>
        private int cellHeight;

        //内容宽度,除去padding的宽度
        private int contentWidth;
        //内容高度,除去padding的高度
        private int contentHeight;

        //是否是被合并了的单元格
        private bool merged = false;


        ///<summary> 单元格背景色 </summary>
        private Color backgroundColor = Color.White;


        private int paddingLeft = 20;
        private int paddingRight = 20;
        private int paddingTop = 20;
        private int paddingBottom = 10;

        #region 预留的几个字段,以防将来变化

        private string key1 = "";

        public string Key1
        {
            get { return key1; }
            set { key1 = value; }
        }
        private string key2 = "";

        public string Key2
        {
            get { return key2; }
            set { key2 = value; }
        }
        private string key3 = "";

        public string Key3
        {
            get { return key3; }
            set { key3 = value; }
        }

        #endregion

        //单元格所属行
        private TPTextRow ownerRow;

        private ZYTextParagraph para;

        private ItalicLineStyle m_ItalicLineStyle = ItalicLineStyle.NoLine;//默认其中没有斜线

        private bool m_CanInsertEnter = true;//默认单元格内可以换行 Add By wwj 2012-06-06
        public bool CanInsertEnter
        {
            get { return m_CanInsertEnter; }
            set { m_CanInsertEnter = value; }
        }
        #endregion

        #region 构造函数

        public TPTextCell()
        {
            //初始化边框
            this.BorderWidth = 1;
            this.BorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.BorderColor = Color.Black;

            para = new ZYTextParagraph();
            para.Parent = this;
            myChildElements.Add(para);
        }

        public TPTextCell(TPTextCell cell)
        {
            this.colspan = cell.Colspan;
            this.rowspan = cell.Rowspan;
            this.horizontalAlignment = cell.HorizontalAlignment;
            this.verticalAlignment = cell.VerticalAlignment;

            this.bolSelected = cell.Selected;

            this.Width = cell.Width;
            this.Height = cell.Height;

            this.backgroundColor = cell.backgroundColor;

            this.PaddingTop = cell.PaddingTop;
            this.PaddingRight = cell.PaddingRight;
            this.PaddingBottom = cell.PaddingBottom;
            this.PaddingLeft = cell.PaddingLeft;

            this.BorderWidth = cell.BorderWidth;
            this.BorderWidthTop = cell.BorderWidthTop;
            this.BorderWidthRight = cell.BorderWidthRight;
            this.BorderWidthBottom = cell.BorderWidthBottom;
            this.BorderWidthLeft = cell.BorderWidthLeft;

            this.BorderColor = cell.BorderColor;
            this.BorderColorTop = cell.BorderColorTop;
            this.BorderColorRight = cell.BorderColorRight;
            this.BorderColorBottom = cell.BorderColorBottom;
            this.BorderColorLeft = cell.BorderColorLeft;

            this.BorderStyle = cell.BorderStyle;
            this.BorderStyleTop = cell.BorderStyleTop;
            this.BorderStyleRight = cell.BorderStyleRight;
            this.BorderStyleBottom = cell.BorderStyleBottom;
            this.BorderStyleLeft = cell.BorderStyleLeft;

            this.ItalicLineStyleInCell = cell.ItalicLineStyleInCell;//Add by wwj 2012-05-29

            this.para = new ZYTextParagraph();
            para.Parent = this;
            myChildElements.Add(para);
        }

        /// <summary>
        /// 单元格深度复制 Add by wwj 2012-05-30
        /// </summary>
        /// <returns></returns>
        public TPTextCell Clone()
        {
            TPTextCell newCell = new TPTextCell();
            newCell.myOwnerDocument = this.myOwnerDocument;

            newCell.myChildElements.Clear();

            newCell.colspan = Colspan;
            newCell.rowspan = Rowspan;
            newCell.horizontalAlignment = HorizontalAlignment;
            newCell.verticalAlignment = VerticalAlignment;

            newCell.bolSelected = Selected;

            newCell.Width = Width;
            newCell.Height = Height;

            newCell.backgroundColor = backgroundColor;

            newCell.PaddingTop = PaddingTop;
            newCell.PaddingRight = PaddingRight;
            newCell.PaddingBottom = PaddingBottom;
            newCell.PaddingLeft = PaddingLeft;

            newCell.BorderWidth = BorderWidth;
            newCell.BorderWidthTop = BorderWidthTop;
            newCell.BorderWidthRight = BorderWidthRight;
            newCell.BorderWidthBottom = BorderWidthBottom;
            newCell.BorderWidthLeft = BorderWidthLeft;

            newCell.BorderColor = BorderColor;
            newCell.BorderColorTop = BorderColorTop;
            newCell.BorderColorRight = BorderColorRight;
            newCell.BorderColorBottom = BorderColorBottom;
            newCell.BorderColorLeft = BorderColorLeft;

            newCell.BorderStyle = BorderStyle;
            newCell.BorderStyleTop = BorderStyleTop;
            newCell.BorderStyleRight = BorderStyleRight;
            newCell.BorderStyleBottom = BorderStyleBottom;
            newCell.BorderStyleLeft = BorderStyleLeft;

            newCell.ContentHeight = ContentHeight;

            newCell.ItalicLineStyleInCell = ItalicLineStyleInCell;//Add by wwj 2012-05-29

            //***************************************
            newCell.ChildElements.Clear();

            for (int i = 0; i < myChildElements.Count; i++)//遍历单元格中的段落
            {
                ZYTextParagraph para = myChildElements[i] as ZYTextParagraph;
                if (para != null)
                {
                    ZYTextParagraph newPara = new ZYTextParagraph();
                    newPara.Parent = newCell;
                    newPara.OwnerDocument = newCell.OwnerDocument;
                    newCell.myChildElements.Add(newPara);
                    StringBuilder sb = new StringBuilder();
                    para.GetFinalText(sb);
                    newPara.Align = para.Align;

                    foreach (ZYTextElement myElement in para.ChildElements)//遍历段落中的没有元素
                    {
                        if (myElement is ZYTextChar)//如果文本元素
                        {
                            ZYTextChar oldChar = myElement as ZYTextChar;
                            ZYTextChar NewChar = ZYTextChar.Create(oldChar.Char);
                            NewChar.OwnerDocument = newCell.OwnerDocument;
                            NewChar.Parent = newPara;
                            newPara.InsertBefore(NewChar, newPara.LastElement);
                            NewChar.FontName = oldChar.FontName;
                            NewChar.FontSize = oldChar.FontSize;
                            NewChar.Height = oldChar.Height;
                            NewChar.Width = oldChar.Width;
                            NewChar.Sub = oldChar.Sub;
                            NewChar.Sup = oldChar.Sup;
                        }
                    }
                }
            }

            //***************************************

            newCell.UpdateBounds();
            return newCell;
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// Determines whether the specified x is contain.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        /// 	<c>true</c> if the specified x is contain; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsContain(int x, int y)
        {
            if (x >= this.RealLeft && x <= this.RealLeft + this.Width && y >= this.RealTop && y <= this.RealTop + this.Height)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 处理输入的字符，现在的情况是单元格中需要控制是否需要可以换行
        /// 如果输入的字符导致行数大于输入之前的行数，则需要删除输入的字符
        /// Add By wwj 2012-06-06
        /// </summary>
        /// <param name="PreLineCount">输入字符前的行数</param>
        /// <returns>false:表示不用处理 true:表示已经处理过了</returns>
        public override bool ProcessInsertChar(int preLineCount)
        {
            int lineCount = this.Lines.Count;
            if (!this.CanInsertEnter && preLineCount < lineCount)
            {
                while (preLineCount < lineCount)
                {
                    this.OwnerDocument._BackSpace();
                    lineCount = this.Lines.Count;
                }
                return true;
            }
            return false;
        }

        #endregion

        #region 私有方法



        #endregion

        #region 公有属性

        public int PaddingLeft
        {
            get
            {
                return paddingLeft;
            }
            set
            {
                paddingLeft = value;
            }
        }
        public int PaddingRight
        {
            get
            {
                return paddingRight;
            }
            set
            {
                paddingRight = value;
            }
        }
        public int PaddingTop
        {
            get
            {
                return paddingTop;
            }
            set
            {
                paddingTop = value;
            }
        }
        public int PaddingBottom
        {
            get
            {
                return paddingBottom;
            }
            set
            {
                paddingBottom = value;
            }
        }
        public int Padding
        {
            set
            {
                paddingBottom = value;
                paddingTop = value;
                paddingLeft = value;
                paddingRight = value;
            }
        }

        /// <summary>
        /// 获取/设置水平对齐方式
        /// </summary>
        /// <value>a value</value>
        public int HorizontalAlignment
        {
            get
            {
                return horizontalAlignment;
            }

            set
            {
                horizontalAlignment = value;
            }
        }

        /// <summary>
        /// 获取/设置垂直对齐方式
        /// </summary>
        /// <value>a value</value>
        public int VerticalAlignment
        {
            get
            {
                return verticalAlignment;
            }

            set
            {
                verticalAlignment = value;
            }
        }

        /// <summary>
        /// 获取/设置colspan.
        /// </summary>
        /// <value>a value</value>
        public int Colspan
        {
            get
            {
                return colspan;
            }

            set
            {
                colspan = value;
            }
        }

        /// <summary>
        /// 获取/设置rowspan.
        /// </summary>
        /// <value>a value</value>
        public int Rowspan
        {
            get
            {
                return rowspan;
            }

            set
            {
                rowspan = value;
            }
        }

        public bool Merged
        {
            get
            {
                return this.merged;
            }
            set
            {
                this.merged = value;
            }
        }

        /// <summary>
        /// 单元格选择标志,主要用来表示高亮的.
        /// 此值为true时,CanAccess的值也为true
        /// </summary>
        public bool Selected
        {
            get { return bolSelected; }
            set { bolSelected = value; }
        }

        /// <summary>
        /// 表示当前单元格是否可以操作,一般和Selected一样.
        /// 在单击cell时,此值为true,但是Selected为false
        /// </summary>
        public bool CanAccess
        {
            get
            {
                return canAccess;
            }
            set
            {
                canAccess = value;
            }
        }

        /// <summary>
        /// cell的总宽度
        /// </summary>
        /// <value></value>
        public override int Width
        {
            get
            {
                if (this.contentWidth == 0)
                {
                    return 0;
                }
                return this.ContentWidth + paddingLeft + paddingRight;
            }
            set
            {
                this.cellWidth = value;
                this.contentWidth = value - (paddingLeft + paddingRight);
            }
        }

        /// <summary>
        /// cell的总高度
        /// </summary>
        /// <value></value>
        public override int Height
        {
            get
            {
                if (this.contentHeight < 0 || this.contentHeight == 0/*TODO Add By wwj 2012-02-19*/)
                {
                    return 0;
                }
                return this.ContentHeight + paddingTop + paddingBottom;
            }
            set
            {
                this.cellHeight = value;

                //TODO Modified By wwj 2012-02-19
                //this.contentHeight = value - (paddingTop + paddingBottom);
                if (value - (paddingTop + paddingBottom) < 0)
                {
                    this.contentHeight = 0;
                }
                else
                {
                    this.contentHeight = value - (paddingTop + paddingBottom);
                }
            }
        }

        /// <summary>
        /// 内容宽度
        /// </summary>
        public int ContentWidth
        {
            get
            {
                return contentWidth;
            }
            set
            {
                contentWidth = value;
            }
        }

        /// <summary>
        /// 内容高度
        /// </summary>
        public int ContentHeight
        {
            get
            {
                if (this.contentHeight < 0 || this.contentHeight == 0/*TODO Add By wwj 2012-02-19*/)
                {
                    return 0;
                }

                //根据rowspan,或者lines.Count算的内容高.
                int cHeight = 0;
                int lineCount = Lines.Count;

                if (rowspan > lineCount && rowspan > 1)
                {
                    cHeight = AllLineHeight + ((rowspan - 1) * (paddingTop + paddingBottom));
                }
                else
                {
                    //TODO  **************Modified by wwj 2012-02-17 修改单元格高度的算法*****************
                    //cHeight = AllLineHeight + ((lineCount - 1) * (paddingTop + paddingBottom));
                    cHeight = AllLineHeight;
                    //**************************************END******************************************
                }

                if (cHeight > contentHeight)
                {
                    return cHeight;
                }
                else
                {
                    return contentHeight;
                }
            }
            set
            {
                contentHeight = value;
            }
        }

        /// <summary>
        /// 获取当前单元格内所有行的总高度
        /// </summary>
        internal int AllLineHeight
        {
            get
            {
                int tmpHeight = 0;
                if (myLines.Count > 0)
                {
                    //最后一行
                    ZYTextLine LastLine = (ZYTextLine)myLines[myLines.Count - 1];

                    //TODO  **************Modified by wwj 2012-02-17 修改单元格高度的算法*****************
                    //tmpHeight = LastLine.Top + LastLine.Height;
                    ZYTextLine FirstLine = (ZYTextLine)myLines[0];
                    tmpHeight = LastLine.Top + LastLine.Height - FirstLine.Top;
                    //**************************************END*****************************************
                }
                return tmpHeight;
            }
        }

        /// <summary>
        /// 当前cell所属的row
        /// </summary>
        public TPTextRow OwnerRow
        {
            get
            {
                return this.ownerRow;
            }
            set
            {
                this.ownerRow = value;
            }
        }

        public string Text
        {
            get
            {
                System.Collections.ArrayList myFinalList = new System.Collections.ArrayList();
                this.AddFinalElementToList(myFinalList);
                string strText = ZYTextElement.GetElementsText(myFinalList);
                return strText;
            }
            set
            {
                this.ChildElements.Clear();
                ZYTextParagraph para = new ZYTextParagraph();
                para.Parent = this;
                para.OwnerDocument = this.OwnerDocument;
                myChildElements.Add(para);
                foreach (char a in value)
                {
                    ZYTextChar NewChar = ZYTextChar.Create(a);
                    NewChar.OwnerDocument = this.OwnerDocument;
                    NewChar.Parent = para;
                    para.InsertBefore(NewChar, para.LastElement);
                }
                para.OwnerDocument = OwnerDocument;

            }
        }

        /// <summary>
        /// 单元格斜线
        /// </summary>
        public ItalicLineStyle ItalicLineStyleInCell
        {
            get
            {
                return m_ItalicLineStyle;
            }
            set
            {
                m_ItalicLineStyle = value;
            }
        }
        #endregion

        #region 公有的边框属性

        #region width

        public int BorderWidth
        {
            get { return this.Border.BorderWidth; }
            set { this.Border.BorderWidth = value; }
        }

        public int BorderWidthTop
        {
            get { return this.Border.topWidth; }
            set { this.Border.topWidth = value; }
        }
        public int BorderWidthRight
        {
            get { return this.Border.rightWidth; }
            set { this.Border.rightWidth = value; }
        }
        public int BorderWidthBottom
        {
            get { return this.Border.bottomWidth; }
            set { this.Border.bottomWidth = value; }
        }
        public int BorderWidthLeft
        {
            get { return this.Border.leftWidth; }
            set { this.Border.leftWidth = value; }
        }
        #endregion

        #region color

        public Color BorderColor
        {
            get { return this.Border.BorderColor; }
            set { this.Border.BorderColor = value; }
        }

        public Color BorderColorTop
        {
            get { return this.Border.topColor; }
            set { this.Border.topColor = value; }
        }
        public Color BorderColorRight
        {
            get { return this.Border.rightColor; }
            set { this.Border.rightColor = value; }
        }
        public Color BorderColorBottom
        {
            get { return this.Border.bottomColor; }
            set { this.Border.bottomColor = value; }
        }
        public Color BorderColorLeft
        {
            get { return this.Border.leftColor; }
            set { this.Border.leftColor = value; }
        }
        #endregion

        #region style

        public ButtonBorderStyle BorderStyle
        {
            get { return this.Border.BorderStyle; }
            set { this.Border.BorderStyle = value; }
        }

        public ButtonBorderStyle BorderStyleTop
        {
            get { return this.Border.topStyle; }
            set { this.Border.topStyle = value; }
        }
        public ButtonBorderStyle BorderStyleRight
        {
            get { return this.Border.rightStyle; }
            set { this.Border.rightStyle = value; }
        }
        public ButtonBorderStyle BorderStyleBottom
        {
            get { return this.Border.bottomStyle; }
            set { this.Border.bottomStyle = value; }
        }
        public ButtonBorderStyle BorderStyleLeft
        {
            get { return this.Border.leftStyle; }
            set { this.Border.leftStyle = value; }
        }
        #endregion

        #endregion

        #region Container继承override方法

        /// <summary>
        /// 获取完整的包含对象的最小矩形
        /// </summary>
        /// <returns>矩形对象</returns>
        public override Rectangle GetContentBounds()
        {
            return new Rectangle(this.RealLeft, this.RealTop, this.Width, this.Height);
        }

        /// <summary>
        /// 获得对象保存的XML节点的名称
        /// </summary>
        /// <returns></returns>
        public override string GetXMLName()
        {
            return ZYTextConst.c_Cell;
        }

        /// <summary>
        /// 该元素单独的占有一行
        /// </summary>
        /// <returns></returns>
        public override bool OwnerWholeLine()
        {
            return false;
        }
        /// <summary>
        /// 容器对象默认进行强制换行
        /// </summary>
        /// <returns></returns>
        public override bool isNewLine()
        {
            return false;
        }
        int ss = 60;

        /// <summary>
        /// 刷新界面,重新绘制对象
        /// </summary>
        /// <returns>是否进行了刷新操作</returns>
        public override bool RefreshView()
        {
            base.RefreshView();

            Rectangle rect = GetContentBounds();
            myOwnerDocument.View.DrawBorder(rect,
                this.Border.leftColor, this.Border.leftWidth, this.Border.leftStyle, //左
                this.Border.topColor, this.Border.topWidth, this.Border.topStyle,   //上
                this.Border.rightColor, this.Border.rightWidth, this.Border.rightStyle, //右
                this.Border.bottomColor, this.Border.bottomWidth, this.Border.bottomStyle); //下
            if (this.Selected == true)
            {
                if (!myOwnerDocument.EnableSelectAreaPrint)//Add By wwj 2012-04-17 开启选中区域打印时关闭元素选中颜色翻转效果
                {
                    myOwnerDocument.OwnerControl.ReversibleViewFillRect(this.GetContentBounds(), myOwnerDocument.View.Graph);
                }
            }

            //非合并单元格，且需要绘制斜线
            if (!this.Merged && this.ItalicLineStyleInCell != ItalicLineStyle.NoLine)
            {
                if (this.ItalicLineStyleInCell == ItalicLineStyle.LeftTop2RightBottom)
                    myOwnerDocument.View.DrawLine(this.BorderColor, rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                else if (this.ItalicLineStyleInCell == ItalicLineStyle.RightTop2LeftBottom)
                    myOwnerDocument.View.DrawLine(this.BorderColor, rect.X + rect.Width, rect.Y, rect.X, rect.Y + rect.Height);
                else if (this.ItalicLineStyleInCell == ItalicLineStyle.LeftTop2RightBottom2)
                {
                    myOwnerDocument.View.DrawLine(this.BorderColor, rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height / 2);
                    myOwnerDocument.View.DrawLine(this.BorderColor, rect.X, rect.Y, rect.X + rect.Width / 2, rect.Y + rect.Height);
                }
                else if (this.ItalicLineStyleInCell == ItalicLineStyle.LeftTop2RightBottom3)
                {
                    myOwnerDocument.View.DrawLine(this.BorderColor, rect.X, rect.Y + rect.Height / 2, rect.X + rect.Width, rect.Y + rect.Height);
                    myOwnerDocument.View.DrawLine(this.BorderColor, rect.X + rect.Width / 2, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                }
            }

            return true;
        }


        protected override void RefreshClientWidth()
        {
            if (myOwnerDocument != null && myOwnerDocument.Info.WordWrap)
            {
                intClientWidth = this.contentWidth;
            }
        }

        /// <summary>
        /// 重载此方法,主要是为了做padding位置的调整.
        /// 其它的东西和Contontainer中的一致
        /// </summary>
        public override void UpdateBounds()
        {
            int iLineTop = this.paddingTop; //上间距
            System.Drawing.Rectangle NewBounds = System.Drawing.Rectangle.Empty;

            foreach (ZYTextLine myLine in myLines)
            {
                myLine.Top = iLineTop;
                myLine.RealTop = this.RealTop + iLineTop;
                myLine.RealLeft = this.RealLeft + this.paddingLeft; //左间距
                if (myLine.LastElement != null && myLine.LastElement.isNewParagraph())
                {
                    myLine.LineSpacing = myOwnerDocument.Info.ParagraphSpacing;
                }
                else
                {
                    myLine.LineSpacing = myOwnerDocument.Info.LineSpacing;
                }

                iLineTop += myLine.FullHeight;

                foreach (ZYTextElement myElement in myLine.Elements)
                {

                    NewBounds = new System.Drawing.Rectangle
                        (myElement.RealLeft,
                        myElement.RealTop,
                        myElement.Width + myElement.WidthFix,
                        myElement.Height);

                    if (NewBounds.Equals(myElement.Bounds) == false)
                    {
                        if (myOwnerDocument.OwnerControl != null && !(myElement is ZYTextContainer))
                        {
                            if (myElement.Bounds.IsEmpty == false)
                                myOwnerDocument.OwnerControl.AddInvalidateRect(myElement.Bounds);
                            myOwnerDocument.OwnerControl.AddInvalidateRect(NewBounds);
                        }
                        myElement.myBounds = NewBounds;
                    }
                    if (myElement is ZYTextContainer)
                    {
                        ZYTextContainer c = (ZYTextContainer)myElement;
                        c.UpdateBounds();
                    }
                }
            }

        }



        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="myElement">My element.</param>
        /// <returns></returns>
        public override bool FromXML(System.Xml.XmlElement myElement)
        {
            if (myElement != null)
            {
                if (myElement.Attributes.Count == 0)
                {
                    this.Width = 0;
                    this.Height = 0;
                    this.ContentHeight = 0;
                    this.ContentWidth = 0;
                    this.BorderWidth = 0;
                    this.Padding = 0;
                    this.Merged = true;
                    this.ChildElements.Clear();
                }
                else
                {
                    base.FromXML(myElement);

                    //myAttributes.FromXML(myElement);

                    //Add By wwj 2012-03-22 单元格斜线保存的功能
                    if (myAttributes.Contains("line-style"))
                    {
                        this.ItalicLineStyleInCell = (ItalicLineStyle)Enum.Parse(typeof(ItalicLineStyle), myAttributes.GetString("line-style"));
                    }

                    //Add By wwj 2012-06-06 是否可以换行
                    if (myAttributes.Contains("caninsertenter"))
                    {
                        this.CanInsertEnter = bool.Parse(myElement.GetAttribute("caninsertenter"));
                    }

                    this.Colspan = myAttributes.GetInt32("colspan");
                    this.Rowspan = myAttributes.GetInt32("rowspan");

                    this.HorizontalAlignment = myAttributes.GetInt32("halign");
                    this.VerticalAlignment = myAttributes.GetInt32("valign");

                    this.BorderWidthTop = myAttributes.GetInt32("border-width-top");
                    this.BorderWidthRight = myAttributes.GetInt32("border-width-right");
                    this.BorderWidthBottom = myAttributes.GetInt32("border-width-bottom");
                    this.BorderWidthLeft = myAttributes.GetInt32("border-width-left");

                    this.Width = myAttributes.GetInt32("width");
                    this.Height = myAttributes.GetInt32("height");

                    this.ContentWidth = myAttributes.GetInt32("contentwidth");
                    this.ContentHeight = myAttributes.GetInt32("contentheight");

                    this.PaddingTop = myAttributes.GetInt32("padding-top");
                    this.PaddingRight = myAttributes.GetInt32("padding-right");
                    this.PaddingBottom = myAttributes.GetInt32("padding-bottom");
                    this.PaddingLeft = myAttributes.GetInt32("padding-left");

                    //this.backgroundColor = myAttributes.GetColor("background-color");

                    this.Key1 = myAttributes.GetString("key1");
                    this.Key2 = myAttributes.GetString("key2");
                    this.Key3 = myAttributes.GetString("key3");

                }

                return true;
            }
            return false;
        }
        /// <summary>
        /// 已重载:从XML节点加载对象数据
        /// </summary>
        /// <param name="myElement"></param>
        /// <returns></returns>
        public override bool ToXML(System.Xml.XmlElement myElement)
        {
            if (myElement != null)
            {
                myAttributes.Clear();//Add By wwj 2012-02-20 解决合并单元格后再次打开无效的Bug
                if (merged != true)
                {
                    //Add By wwj 2012-03-22 单元格斜线保存的功能
                    myAttributes.SetValue("line-style", Convert.ToInt32(this.ItalicLineStyleInCell));

                    //Add By wwj 2012-06-06 是否可以换行
                    myElement.SetAttribute("caninsertenter", this.CanInsertEnter.ToString());

                    myAttributes.SetValue("colspan", this.Colspan);
                    myAttributes.SetValue("rowspan", this.Rowspan);

                    myAttributes.SetValue("halign", this.HorizontalAlignment);
                    myAttributes.SetValue("valign", this.VerticalAlignment);

                    myAttributes.SetValue("border-width-top", this.BorderWidthTop);
                    myAttributes.SetValue("border-width-right", this.BorderWidthRight);
                    myAttributes.SetValue("border-width-bottom", this.BorderWidthBottom);
                    myAttributes.SetValue("border-width-left", this.BorderWidthLeft);

                    myAttributes.SetValue("width", this.Width);
                    myAttributes.SetValue("height", this.Height);

                    myAttributes.SetValue("contentwidth", this.ContentWidth);
                    myAttributes.SetValue("contentheight", this.ContentHeight);

                    myAttributes.SetValue("padding-top", this.PaddingTop);
                    myAttributes.SetValue("padding-right", this.PaddingRight);
                    myAttributes.SetValue("padding-bottom", this.PaddingBottom);
                    myAttributes.SetValue("padding-left", this.PaddingLeft);

                    //myAttributes.SetValue("background-color", this.backgroundColor);

                    myAttributes.SetValue("key1", this.Key1);
                    myAttributes.SetValue("key2", this.Key2);
                    myAttributes.SetValue("key3", this.Key3);
                }
                myAttributes.ToXML(myElement);
                base.ToXML(myElement);
                return true;
            }
            return false;
        }

        #endregion

        #region 鼠标事件处理,例如选择单元格,行和表格本身

        public override bool HandleMouseDown(int x, int y, MouseButtons Button)
        {
            return base.HandleMouseDown(x, y, Button);
        }


        public override bool HandleMouseMove(int x, int y, MouseButtons Button)
        {
            return base.HandleMouseMove(x, y, Button);
        }

        public override bool HandleMouseUp(int x, int y, MouseButtons Button)
        {
            return base.HandleMouseUp(x, y, Button);
        }
        #endregion

    }

    /// <summary>
    /// 单元格中斜线的枚举
    /// </summary>
    public enum ItalicLineStyle : int
    {
        /// <summary>
        /// 没有线
        /// </summary>
        NoLine = 0,

        /// <summary>
        /// 左上到右下的斜线
        /// </summary>
        LeftTop2RightBottom = 1,

        /// <summary>
        /// 右上到左下的斜线
        /// </summary>
        RightTop2LeftBottom = 2,

        /// <summary>
        /// 左上到右下（2条斜线）
        /// </summary>
        LeftTop2RightBottom2 = 3,
        /// <summary>
        /// 左上（2条斜线）到右下
        /// </summary>
        LeftTop2RightBottom3 = 4
    }
}
