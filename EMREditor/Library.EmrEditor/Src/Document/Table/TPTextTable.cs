using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    /// <summary>
    /// 表示一个表格,一个表格中可能包含多个行
    /// </summary>
    public class TPTextTable : ZYTextContainer, IEnumerable<TPTextRow>, IEnumerator<TPTextRow>
    {
        #region 私有变量

        /// <summary>表格包含多少列</summary>
        private int columns;

        /// <summary>表格包含的行列表</summary>
        private List<TPTextRow> allRows = new List<TPTextRow>();

        /// <summary>表格的宽度</summary>
        private int totalWidth = -1;
        private int totalHeight = -1;

        /// <summary>各列的百分比宽度(也可能是绝对宽度,此时和absoluteWidths相同)</summary>
        private int[] relativeWidths;

        /// <summary> 各列的绝对宽度 </summary>
        private int[] absoluteWidths;

        ///<summary> This is cellpadding. </summary>
        private int cellpadding = 20;

        ///<summary> This is cellspacing. </summary>
        private int cellspacing = 20;

        ///<summary>
        ///表格水平对齐方式 
        ///1 左对齐
        ///2 居中对齐
        ///3 右对齐
        ///</summary>
        private int horizontalAlignment = 1;

        /// <summary>
        /// true 固定列宽
        /// false 自动列宽
        /// </summary>
        private bool isFixWidth = true;

        private int defaultRowHeight = 0;
        private int defaultTableWidth = 0;

        /// <summary>
        /// 表格标题
        /// </summary>
        private string header;
        /// <summary>
        /// 是否隐藏所有边框
        /// </summary>
        private bool hiddenAllBorder = false;

        private int position = -1;

        #endregion

        #region 索引器
        /// <summary>
        /// Gets or sets the <see cref="DrectSoft.Library.EmrEditor.Src.Document.TPTextRow"/> at the specified index.
        /// </summary>
        /// <value></value>
        public TPTextRow this[int index]
        {
            get
            {
                return allRows[Index];
            }
            set
            {
                allRows.Insert(index, value);
                myChildElements.Insert(index, value);
            }
        }
        public TPTextCell this[int row, int col]
        {
            get
            {
                if (row < 0 || row >= this.allRows.Count)
                {
                    throw new IndexOutOfRangeException("超出行索引范围");
                }
                if (col < 0 || col >= this.columns)
                {
                    throw new IndexOutOfRangeException("超出列索引范围");
                }
                return allRows[row].Cells[col];
            }
            set
            {
                if ((row < 0 || row >= this.allRows.Count) || (col < 0 || col >= this.columns))
                {
                    throw new IndexOutOfRangeException("超索引范围,不存在这样的单元格");
                }
                allRows[row].Cells[col] = value;
                allRows[row].Cells[col].ChildElements = value.ChildElements;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// Initializes a new instance of the <see cref="TPTextTable"/> class.
        /// </summary>
        public TPTextTable() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TPTextTable"/> class.
        /// </summary>
        /// <param name="header">表格总标题</param>
        /// <param name="columns">列数</param>
        /// <param name="rows">行数</param>
        /// <param name="columnWidth">每一列的宽</param>
        /// <param name="defaultRowHeight">默认的行高</param>
        /// <param name="defaultTableWidth">默认的表格宽度</param>
        public TPTextTable(string header, int columns, int rows, int columnWidth, int defaultRowHeight, int defaultTableWidth)
        {
            if (columns <= 0) { throw new Exception("列数不能小于零"); }
            if (rows <= 0) { throw new Exception("行数不能小于零"); }

            //int limitColWidthMax = defaultTableWidth / columns;
            //int limitColWidthMin = GraphicsUnitConvert.Convert(11, GraphicsUnit.Pixel, GraphicsUnit.Document);

            //if (columnWidth < limitColWidthMin || columnWidth > limitColWidthMax)
            //{
            //    double min = GraphicsUnitConvert.Convert(limitColWidthMin, GraphicsUnit.Document, GraphicsUnit.Millimeter) * 10;
            //    double max = GraphicsUnitConvert.Convert(limitColWidthMax, GraphicsUnit.Document, GraphicsUnit.Millimeter) * 10;
            //    MessageBox.Show("度量值必须介于 "+ min.ToString()+" 厘米和 "+max.ToString()+" 厘米之间");
            //    return;
            //}

            this.header = header;
            this.columns = columns;

            this.defaultRowHeight = defaultRowHeight;
            this.defaultTableWidth = defaultTableWidth;
            this.totalWidth = defaultTableWidth;

            SetTotalWidth(columnWidth);

            for (int i = 0; i < rows; i++)
            {
                TPTextRow someRow = new TPTextRow(columns, absoluteWidths, defaultRowHeight, false);
                if (i == rows - 1)
                {
                    someRow = new TPTextRow(columns, absoluteWidths, defaultRowHeight, true);
                }
                someRow.Parent = this;
                someRow.OwnerTable = this;
                this.allRows.Add(someRow);
                myChildElements.Add(someRow);
            }

            CalculateHeights();
        }
        #endregion

        #region 私有方法


        #endregion

        #region 公有方法

        /// <summary>
        /// 重新设置表格中所有单元格的边框大小
        /// </summary>
        public void SetEveryCellBorderWidth()
        {
            for (int i = 0; i < this.allRows.Count; i++)
            {
                allRows[i].SetRowsBorderWidth(false);
                if (i == (this.allRows.Count - 1))
                {
                    allRows[i].SetRowsBorderWidth(true);
                }
            }
        }

        /// <summary>
        /// 根据指定的列宽设置表格的总宽
        /// 当<code>columnWidth</code>为0时,则是根据总宽算各个列宽
        /// </summary>
        /// <param name="columnWidth">列宽,如果为0,则根据表格总宽设置各列宽度</param>
        public void SetTotalWidth(int columnWidth)
        {
            this.relativeWidths = new int[columns];
            this.absoluteWidths = new int[columns];

            if (columnWidth == 0 && this.totalWidth > 0)
            {
                int tmpWidth = this.totalWidth / this.columns;
                for (int i = 0; i < columns; i++)
                {
                    if (i == columns - 1)
                    {
                        //最后一列为前面分配完后剩余的宽度
                        this.relativeWidths[i] = (this.totalWidth - (tmpWidth * (this.columns - 1)));
                    }
                    else
                    {
                        this.relativeWidths[i] = tmpWidth;
                    }
                }
            }
            if (columnWidth > 0)
            {
                this.totalWidth = 0;
                for (int i = 0; i < columns; i++)
                {
                    this.relativeWidths[i] = columnWidth;
                    this.totalWidth += columnWidth;
                }
            }
            this.CalculateWidths();
        }

        /// <summary>
        /// 设置各列的宽度
        /// </summary>
        /// <param name="relativeWidths">百分比宽度(绝对宽度)</param>
        public void SetWidth(int[] relativeWidths)
        {
            if (this.relativeWidths.Length != relativeWidths.Length)
            {
                throw new Exception("相对宽度的列数不符");
            }
            for (int k = 0; k < relativeWidths.Length; k++)
            {
                this.relativeWidths[k] = relativeWidths[k];
            }
            this.absoluteWidths = new int[relativeWidths.Length];
            CalculateWidths();
            CalculateHeights();
        }

        /// <summary>
        /// 计算各列的绝对宽度
        /// </summary>
        private void CalculateWidths()
        {
            int total = 0;
            for (int k = 0; k < this.absoluteWidths.Length; k++)
            {
                total += this.relativeWidths[k];
            }
            for (int k = 0; k < this.absoluteWidths.Length; k++)
            {
                absoluteWidths[k] = totalWidth * relativeWidths[k] / total;
            }
        }

        /// <summary>
        /// 设置表格的整体高度
        /// </summary>
        private void CalculateHeights()
        {
            int tmpHeight = 0;
            foreach (TPTextRow row in allRows)
            {
                tmpHeight += row.Height;
            }
            this.totalHeight = tmpHeight;
        }


        /// <summary>
        /// 向表格的末尾添加一列
        /// </summary>
        /// <param name="aColumns"></param>
        public void AddColumns(TPTextCell cell)
        {
            this.columns = this.absoluteWidths.Length + 1;
            this.absoluteWidths = new int[this.columns];
            SetTotalWidth(0);

            for (int i = 0; i < allRows.Count; i++)
            {
                allRows[i].AddCell(cell);
            }
        }

        /// <summary>
        /// 在指定列处插入一个新列
        /// </summary>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="cell">The cell.</param>
        public void InsertColumns(int colIndex, TPTextCell cell)
        {
            this.columns = this.absoluteWidths.Length + 1;
            this.absoluteWidths = new int[this.columns];
            SetTotalWidth(0);

            for (int i = 0; i < allRows.Count; i++)
            {
                allRows[i].InsertCell(colIndex, cell);
            }
        }

        /// <summary>
        /// 在指定索引处添加一行
        /// </summary>
        /// <param name="index"></param>
        /// <param name="row"></param>
        public void InsertRow(int index, TPTextRow row)
        {
            TPTextRow tmpRow = new TPTextRow(row);
            tmpRow.Parent = this;
            tmpRow.OwnerTable = this;

            this.allRows.Insert(index, tmpRow);
            this.myChildElements.Insert(index, tmpRow);

            tmpRow.OwnerDocument = row.OwnerDocument;
            CalculateHeights();
        }

        /// <summary>
        /// 在末尾添加一行
        /// </summary>
        /// <param name="row"></param>
        public void AddRow(TPTextRow row)
        {
            TPTextRow tmpRow = new TPTextRow(row);
            tmpRow.Parent = this;
            tmpRow.OwnerTable = this;

            this.allRows.Add(tmpRow);
            this.myChildElements.Add(tmpRow);

            tmpRow.OwnerDocument = row.OwnerDocument;
            CalculateHeights();
        }
        /// <summary>
        /// 在末尾添加若干行
        /// </summary>
        /// <param name="rowNum">要添加的行数</param>
        public void AddRow(int rowNum)
        {
            TPTextRow row = this.allRows[this.allRows.Count - 1];
            for (int k = 0; k < rowNum; k++)
            {
                this.AddRow(row);
            }
        }

        /// <summary>
        /// 删除一行
        /// </summary>
        /// <param name="row">行号</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteRow(int row)
        {
            if (row < 0 || row >= this.allRows.Count)
            {
                return false;
            }
            allRows.RemoveAt(row);
            return true;
        }

        /// <summary>
        /// 删除所有的行
        /// </summary>
        public void DeleteAllRows()
        {
            allRows.Clear();
            //allRows.Add(new TPTextRow(columns));
        }

        /// <summary>
        /// 删除最后一行
        /// </summary>
        /// <returns></returns>
        public bool DeleteLastRow()
        {
            return DeleteRow(allRows.Count - 1);
        }

        /// <summary>
        /// 删除一列
        /// </summary>
        /// <param name="column">列号</param>
        public void DeleteColumn(int column)
        {
            if ((column >= columns) || (column < 0))
            {
                throw new Exception("超出列索引范围");
            }
            foreach (TPTextRow row in this)
            {
                row.DeleteColumn(column);
            }

            int[] newWidths = new int[--columns];
            System.Array.Copy(absoluteWidths, 0, newWidths, 0, column);
            System.Array.Copy(absoluteWidths, column + 1, newWidths, column, columns - column);
            absoluteWidths = newWidths;
            relativeWidths = absoluteWidths;

            this.totalWidth = 0;
            for (int i = 0; i < absoluteWidths.Length; i++)
            {
                this.totalWidth += absoluteWidths[i];
            }
        }

        /// <summary>
        /// 获得当前元素所在的单元格(当前正在操作的cell)
        /// </summary>
        /// <returns></returns>
        internal TPTextCell GetCurrentCell()
        {
            //获取当前元素
            ZYTextElement currentEle = OwnerDocument.Content.CurrentElement;
            TPTextCell cell = OwnerDocument.Content.GetParentByElement(currentEle, ZYTextConst.c_Cell) as TPTextCell;
            if (cell != null)
            {
                return cell;
            }
            return null;
        }

        /// <summary>
        /// <para>获取当前单元格是否在被合并的cell所跨越行的范围内</para>
        /// <para>如果是则返回那个被行合并的cell,否则返回null</para>
        /// </summary>
        /// <returns></returns>
        internal TPTextCell IsInRowSpan(TPTextCell cell)
        {
            for (int row = 0; row < allRows.Count; row++)
            {
                int rowSpan = 0;
                TPTextCell spanCell = null;
                for (int col = 0; col < allRows[row].Cells.Count; col++)
                {
                    if (allRows[row].Cells[col].Rowspan > 1)
                    {
                        rowSpan = allRows[row].Cells[col].Rowspan;
                        spanCell = allRows[row].Cells[col];
                        break;
                    }
                }
                for (int i = row; i < row + rowSpan; i++)
                {
                    for (int j = 0; j < allRows[i].Cells.Count; j++)
                    {
                        if (cell == allRows[i].Cells[j])
                        {
                            return spanCell;
                        }
                    }
                }
                if (rowSpan != 0)
                {
                    row = row + rowSpan - 1;
                }
            }
            return null;
        }

        /// <summary>
        /// 获得cell在表格中的列号.
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        internal int GetColNum(TPTextCell cell)
        {
            return cell.OwnerRow.IndexOf(cell);
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 是否隐藏所有边框
        /// </summary>
        public bool HiddenAllBorder
        {
            get
            {
                return hiddenAllBorder;
            }
            set
            {
                hiddenAllBorder = value;
                if (value == true)
                {
                    foreach (TPTextRow row in this)
                    {
                        foreach (TPTextCell cell in row)
                        {
                            cell.BorderWidth = 0;
                        }
                    }
                }
                else
                {
                    SetEveryCellBorderWidth();
                }
            }
        }
        /// <summary>
        /// 各列的百分比宽度(也可能是绝对宽度,此时和absoluteWidths相同)
        /// </summary>
        internal int[] RelativeWidths
        {
            get { return relativeWidths; }
            set { relativeWidths = value; }
        }

        /// <summary>
        /// 表格各列的绝对宽度
        /// </summary>
        /// <value>The widths.</value>
        public int[] Widths
        {
            get
            {
                return this.absoluteWidths;
            }
            set
            {
                if (value.Length != columns)
                {
                    throw new Exception("列数不相符.");
                }
                for (int i = 0; i < value.Length; i++)
                {
                    this.absoluteWidths[i] = value[i];
                }
            }
        }
        /// <summary>
        /// 水平对齐方式
        /// <para>1 左对齐</para>
        /// <para>2 居中对齐</para>
        /// <para>3 右对齐</para>
        /// </summary>
        public int HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set { horizontalAlignment = value; }
        }

        /// <summary>
        /// Get/set the cellpadding.
        /// </summary>
        /// <value>the cellpadding</value>
        public int Cellpadding
        {
            get
            {
                return cellpadding;
            }

            set
            {
                this.cellpadding = value;
            }
        }

        /// <summary>
        /// Get/set the cellspacing.
        /// </summary>
        /// <value>the cellspacing</value>
        public int Cellspacing
        {
            get
            {
                return cellspacing;
            }

            set
            {
                this.cellspacing = value;
            }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Header
        {
            get { return header; }
            set { header = value; }
        }
        /// <summary>
        /// 列数
        /// </summary>
        public int IntColumns
        {
            get { return columns; }
            set { columns = value; }
        }
        /// <summary>
        /// 行数
        /// </summary>
        public int IntRows
        {
            get { return this.allRows.Count; }
        }
        /// <summary>
        /// 表示所有的行对象
        /// </summary>
        public List<TPTextRow> AllRows
        {
            get
            {
                return allRows;
            }
            set
            {
                allRows = value;
            }
        }
        /// <summary>
        /// 是否固定列宽
        /// </summary>
        public bool IsFixWidth
        {
            get { return isFixWidth; }
            set { isFixWidth = value; }
        }


        public override int Width
        {
            get
            {
                return this.totalWidth;
            }
            set
            {
                if (this.totalWidth == value)
                    return;
                this.totalWidth = value;
                //CalculateWidths();
                //CalculateHeights();
            }
        }

        public override int Height
        {
            get
            {
                CalculateHeights();
                return this.totalHeight;
            }
            set
            {
                this.totalHeight = value;
            }
        }

        #endregion

        #region Container继承override方法

        /// <summary>
        /// 获得对象保存的XML节点的名称
        /// </summary>
        /// <returns></returns>
        public override string GetXMLName()
        {
            return ZYTextConst.c_Table;
        }

        public override Rectangle GetContentBounds()
        {
            //删除表格会出错 add by ywk 2012年10月16日 15:12:26 
            if (allRows.Count > 0)
            {
                if (allRows[0].Cells.Count > 0)
                {
                    int x = allRows[0].Cells[0].RealLeft;
                    int y = allRows[0].Cells[0].RealTop;
                    return new Rectangle(x, y, this.Width, this.Height);
                }
                else
                {
                    return new Rectangle(0, 0, this.Width, this.Height);
                }
            }
            else
            {
                return new Rectangle(0, 0, this.Width, this.Height);
            }
        }

        #region IsNewLine
        public override bool OwnerWholeLine()
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

        public override bool isField()
        {
            return true;
        }

        public override bool isTextElement()
        {
            return false;
        }
        #endregion

        /// <summary>
        /// 将容器内所有的元素添加到列表对象中
        /// </summary>
        /// <param name="myList">列表对象</param>
        /// <param name="ResetFlag">是否重置元素的状态</param>
        public override void AddElementToList(ArrayList myList, bool ResetFlag)
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
                        (myElement as TPTextRow).AddElementToList(myList, ResetFlag);
                    }
                }
            }
        }



        /// <summary>
        /// 刷新界面,重新绘制对象
        /// </summary>
        /// <returns>是否进行了刷新操作</returns>
        public override bool RefreshView()
        {
            return base.RefreshView();
        }

        /// <summary>
        /// 刷新内部元素的大小
        /// </summary>
        /// <returns></returns>
        public override bool RefreshSize()
        {
            return base.RefreshSize();
        }
        /// <summary>
        /// 元素重新分行
        /// </summary>
        /// <returns></returns>
        public override System.Collections.ArrayList RefreshLine()
        {
            base.RefreshLine();

            return myLines;
        }

        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="myElement">My element.</param>
        /// <returns></returns>
        public override bool FromXML(System.Xml.XmlElement myElement)
        {
            if (null != myElement)
            {
                myAttributes.FromXML(myElement);
                this.Width = myAttributes.GetInt32("width");
                //this.Height = myAttributes.GetInt32("height");
                this.Header = myAttributes.GetString("title");
                this.horizontalAlignment = myAttributes.GetInt32("align");
                this.IsFixWidth = myAttributes.GetInt32("fixwidth") == 1 ? true : false;
                this.HiddenAllBorder = (myAttributes.GetInt32("hidden-all-border") == 1) ? true : false;

                this.allRows.Clear();
                this.ChildElements.Clear();

                for (int i = 0; i < myElement.ChildNodes.Count; i++)
                {
                    XmlElement ele = myElement.ChildNodes.Item(i) as XmlElement;
                    if (ele != null)
                    {
                        if (ele.Name == "table-column")
                        {
                            this.columns = Convert.ToInt32(ele.GetAttribute("columns-number"));
                            List<int> relwidths = new List<int>();
                            for (int j = 0; j < ele.ChildNodes.Count; j++)
                            {
                                XmlElement colwidthEle = ele.ChildNodes.Item(j) as XmlElement;
                                if (colwidthEle != null)
                                {
                                    if (colwidthEle.Name == "column-width")
                                    {
                                        relwidths.Add(Convert.ToInt32(colwidthEle.GetAttribute("width")));
                                    }
                                }
                            }
                            this.relativeWidths = new int[relwidths.Count];
                            for (int k = 0; k < relwidths.Count; k++)
                            {
                                relativeWidths[k] = relwidths[k];
                            }
                            SetWidth(relativeWidths);
                        }
                        if (ele.Name == "table-row")
                        {
                            TPTextRow row = OwnerDocument.CreateElementByXML(ele) as TPTextRow;
                            row.Widths = this.absoluteWidths;
                            row.Columns = this.IntColumns;
                            row.Width = this.Width;
                            row.OwnerTable = this;
                            row.Parent = this;

                            this.allRows.Add(row);
                            this.ChildElements.Add(row);
                        }
                    }
                }
                CalculateHeights();
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
            if (null != myElement)
            {
                myAttributes.SetValue("align", this.horizontalAlignment.ToString());
                myAttributes.SetValue("title", this.Header);
                myAttributes.SetValue("fixwidth", (this.IsFixWidth ? 1 : 0).ToString());
                myAttributes.SetValue("width", this.Width);
                myAttributes.SetValue("hidden-all-border", this.HiddenAllBorder ? 1 : 0);

                XmlElement itemColumn = myElement.OwnerDocument.CreateElement("table-column");
                itemColumn.SetAttribute("columns-number", columns.ToString());
                for (int i = 0; i < this.absoluteWidths.Length; i++)
                {
                    XmlElement item = itemColumn.OwnerDocument.CreateElement("column-width");
                    item.SetAttribute("width", absoluteWidths[i].ToString());
                    itemColumn.AppendChild(item);
                }
                myElement.AppendChild(itemColumn);

                foreach (TPTextRow row in this)
                {
                    XmlElement item = myElement.OwnerDocument.CreateElement(row.GetXMLName());
                    myElement.AppendChild(item);
                    row.ToXML(item);
                }
            }
            myAttributes.ToXML(myElement);
            return true;
        }


        #endregion

        #region 鼠标事件处理,例如选择单元格,行和表格本身

        /*
         * 这里要说明一点知识: 有关鼠标划选的执行顺序.
         *  1. MouseMove 事件。
         * 	2. MouseDown 事件。
         * 	3. MouseMove 事件。
         * 	   --这里有个状态,判断是否是点击状态下的移动,也就是划选.(myOwnerControl.CaptureMouse)
         * 	4. MouseClick 事件。
         * 	5. MouseUp 事件。
         */

        //用来保存上一次鼠标点击的位置.
        private Point LastMousePosition = new Point(-1, -1);

        //鼠标在表格中首次单击时,所处的单元格
        private TPTextCell currentCell;

        private TPTextCell dragCell;
        private List<TPTextCell> leftColCells = new List<TPTextCell>();
        private List<TPTextCell> rightColCells = new List<TPTextCell>();

        public override bool HandleMouseDown(int x, int y, MouseButtons Button)
        {
            //应该得到鼠标单击时,是单击的那个单元格.以便在MouseMove事件里,判断是否划选出了当前单元格边界
            if (Button == MouseButtons.Left)
            {
                LastMousePosition = new Point(x, y);
                //Debug.WriteLine("● Table大小=" + this.GetContentBounds().ToString()); 
                //如果是在表格内点击
                if (this.GetContentBounds().Contains(LastMousePosition) && OwnerDocument.OwnerControl.Cursor == Cursors.IBeam)
                {
                    for (int i = 0; i < this.allRows.Count; i++)
                    {
                        for (int k = 0; k < this.allRows[i].Cells.Count; k++)
                        {
                            //将已经选中的cell添加到无效区域内
                            if (this.allRows[i][k].Selected == true || this.allRows[i][k].CanAccess == true)
                            {
                                OwnerDocument.OwnerControl.AddInvalidateRect(this.allRows[i][k].GetContentBounds());
                                //使所有已经选中的cell设为没有选中的状态
                                this.allRows[i][k].Selected = false;
                                this.allRows[i][k].CanAccess = false;
                            }

                            //找到当前点击的那个cell
                            if (this.allRows[i][k].IsContain(x, y))
                            {
                                currentCell = this.allRows[i][k];
                                currentCell.CanAccess = true;
                                //Debug.WriteLine("● (" + i + "," + k +")被选择");
                                //Debug.WriteLine(IsInRowSpan(currentCell) ? "●在rowspan●" : "●no inside●");
                            }
                        }
                    }

                    //刷新无效区域
                    myOwnerDocument.OwnerControl.UpdateInvalidateRect();

                    return false;

                }
                else if (this.GetContentBounds().Contains(LastMousePosition) && OwnerDocument.OwnerControl.Cursor == Cursors.VSplit)
                {
                    //得到要拖拽的单元格，在按下鼠标的时候记录下拖拽的单元格 Modified by wwj 2012-02-16
                    SetRecordDragCell(x, y);

                    //TODO: 当鼠标出于cell的边界处时.此时点击,应该显示一条全页范围内的的纵向虚线,用来拖拽. [ 虚线暂时弄不出来,以后再改 ]

                    //down时,根绝dragCell得到要调整的两列的所有格子(点击处左右两处的列)
                    int rightNum = GetColNum(dragCell);
                    int leftNum = rightNum - 1;
                    foreach (TPTextRow row in this)
                    {
                        //在这i代表列号
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            if (i == leftNum)
                            {
                                leftColCells.Add(row.Cells[i]);
                            }
                            if (i == rightNum)
                            {
                                rightColCells.Add(row.Cells[i]);
                            }
                        }
                    }
                    Debug.WriteLine("●调整列宽前●");
                    for (int i = 0; i < absoluteWidths.Length; i++)
                    {
                        Debug.WriteLine("第" + i + "列:" + absoluteWidths[i]);
                    }
                    //Rectangle drec = new Rectangle(dragCell.RealLeft, 0, 5, OwnerDocument.OwnerControl.CurrentPage.Height);
                }
                else // 在表格外点击
                {
                    foreach (TPTextRow row in this)
                    {
                        foreach (TPTextCell cell in row)
                        {
                            myOwnerDocument.OwnerControl.AddInvalidateRect(cell.GetContentBounds());
                            cell.Selected = false;
                            cell.CanAccess = false;
                        }
                    }
                    currentCell = null;
                    myOwnerDocument.OwnerControl.UpdateInvalidateRect();
                }
            }

            return false;
        }

        public override bool HandleMouseMove(int x, int y, MouseButtons Button)
        {
            //鼠标移动时的坐标位置,随着鼠标的移动不断的变化
            Point p = new Point(x, y);
            //如果鼠标还在表格中
            if (this.GetContentBounds().Contains(p))
            {
                //如果是划选
                if (myOwnerDocument.OwnerControl.CaptureMouse && currentCell != null)
                {
                    //获取当前鼠标点击处单元格的位置和大小
                    Rectangle currentRec = currentCell.GetContentBounds();

                    #region 获得一个选择的橡皮筋矩形区域 SelectRect

                    Rectangle SelectRect = Rectangle.Empty;
                    if (p.X > LastMousePosition.X)
                    {
                        SelectRect.X = LastMousePosition.X;
                        SelectRect.Width = p.X - LastMousePosition.X;
                    }
                    else
                    {
                        SelectRect.X = p.X;
                        SelectRect.Width = LastMousePosition.X - p.X;
                    }
                    if (p.Y > LastMousePosition.Y)
                    {
                        SelectRect.Y = LastMousePosition.Y;
                        SelectRect.Height = p.Y - LastMousePosition.Y;
                    }
                    else
                    {
                        SelectRect.Y = p.Y;
                        SelectRect.Height = LastMousePosition.Y - p.Y;
                    }
                    #endregion


                    if (currentRec.Contains(p))
                    {
                        myOwnerDocument.Content.AutoClearSelection = false;
                        myOwnerDocument.Content.MoveTo(x, y);
                        ZYTextElement myElement = myOwnerDocument.Content.CurrentElement;
                        myOwnerDocument.OwnerControl.MoveCaretWithScroll = false;
                        myOwnerDocument.OwnerControl.UpdateTextCaret();
                        myOwnerDocument.OwnerControl.MoveCaretWithScroll = true;

                        return true;
                    }
                    else
                    {
                        for (int i = 0; i < this.allRows.Count; i++)
                        {
                            for (int k = 0; k < this.allRows[i].Cells.Count; k++)
                            {
                                //取得选择的区域和单元格的交集,用来确定那些单元格被选中了.
                                bool flag = SelectRect.IntersectsWith(this.allRows[i][k].GetContentBounds());
                                if (this.allRows[i][k].Selected != flag)
                                {
                                    this.allRows[i][k].Selected = flag;
                                    this.allRows[i][k].CanAccess = flag;
                                    //将此单元格添加到无效区域
                                    myOwnerDocument.OwnerControl.AddInvalidateRect(this.allRows[i][k].GetContentBounds());
                                    //马上更新无效区域
                                    myOwnerDocument.OwnerControl.UpdateInvalidateRect();
                                }
                            }
                        }

                        //此处加入测试几个单元格被选中的代码

                        return true;
                    }
                }
                else if (myOwnerDocument.OwnerControl.CaptureMouse && currentCell == null && OwnerDocument.OwnerControl.Cursor == Cursors.VSplit)
                {
                    //TODO: 此时是调整表格列宽的时候,应该让纵穿整个页面的虚线随着鼠标移动
                    ResizeToRectangle();

                    return true;
                }
                else if (myOwnerDocument.OwnerControl.CaptureMouse == false)
                {
                    //设置光标状态 Modified By wwj 2012-02-16
                    SetCursorStatus(p.X, p.Y);
                }
            }
            return false;
        }

        public override bool HandleMouseUp(int x, int y, MouseButtons Button)
        {
            bool isDragDone = false;//表示是否处理过拖拽操作 是：返回true 否：返回false
            if (OwnerDocument.OwnerControl.Cursor == Cursors.VSplit)
            {
                if (dragCell != null)
                {
                    isDragDone = true;
                    //确定鼠标要调整的宽度,为正数则是向右拖拽,为负数则是向左拖拽
                    int moveWidth = x - dragCell.RealLeft;
                    Debug.WriteLine("●●●●调整的宽度: " + moveWidth);
                    int acol = GetColNum(dragCell);
                    int[] tmpWidth = new int[absoluteWidths.Length];
                    for (int i = 0; i < absoluteWidths.Length; i++)
                    {
                        if (i == (acol - 1))
                        {
                            tmpWidth[i] = absoluteWidths[i] + moveWidth;
                        }
                        else if (i == acol)
                        {
                            tmpWidth[i] = absoluteWidths[i] - moveWidth;
                        }
                        else
                        {
                            tmpWidth[i] = absoluteWidths[i];
                        }
                    }
                    SetWidth(tmpWidth);
                    foreach (TPTextRow row in this)
                    {
                        row.Widths = tmpWidth;
                    }
                    foreach (TPTextCell cell in leftColCells)
                    {
                        if (cell.Width != 0)
                        {
                            cell.Width = cell.Width + moveWidth;
                        }
                    }
                    foreach (TPTextCell cell in rightColCells)
                    {
                        if (cell.Width != 0)
                        {
                            cell.Width = cell.Width - moveWidth;
                        }
                    }
                    Debug.WriteLine("●调整列宽后●");
                    for (int i = 0; i < absoluteWidths.Length; i++)
                    {
                        Debug.WriteLine("第" + i + "列:" + absoluteWidths[i]);
                    }

                    OwnerDocument.ContentChanged();
                    OwnerDocument.Refresh();
                }
            }
            LastMousePosition = new Point(-1, -1);
            currentCell = null;

            dragCell = null;
            leftColCells.Clear();
            rightColCells.Clear();
            return isDragDone;
        }

        public override void HandleEnter()
        {
            base.HandleEnter();
        }

        public override void HandleLeave()
        {
            base.HandleLeave();
        }

        public override bool HandleClick(int x, int y, MouseButtons Button)
        {
            return base.HandleClick(x, y, Button);
        }
        public override bool HandleDblClick(int x, int y, MouseButtons Button)
        {
            return base.HandleDblClick(x, y, Button);
        }

        #endregion


        #region IEnumerable<TPTextRow> 成员

        IEnumerator<TPTextRow> IEnumerable<TPTextRow>.GetEnumerator()
        {
            return this.allRows.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.allRows.GetEnumerator();
        }

        #endregion

        #region IEnumerator<TPTextRow> 成员

        TPTextRow IEnumerator<TPTextRow>.Current
        {
            get
            {
                try
                {
                    return this.allRows[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        #endregion

        #region IDisposable 成员

        void IDisposable.Dispose()
        {
            // 
        }

        #endregion

        #region IEnumerator 成员

        object IEnumerator.Current
        {
            get
            {
                try
                {
                    return this.allRows[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        bool IEnumerator.MoveNext()
        {
            position++;
            return (position < this.allRows.Count);
        }

        void IEnumerator.Reset()
        {
            position = -1;
        }

        #endregion

        #region Add By wwj 2012-02-16 表格拖拽相关

        #region 用于表格拖拽的判断，已经记录需要拖拽的单元格

        const string s_CursorVSplit = "VSplit";//用于拖拽竖向的光标
        const string s_CursorHSplit = "HSplit";//用于拖拽横向的光标

        /// <summary>
        /// 设置光标状态，return：是否需要记录拖拽的单元格
        /// </summary>
        /// <param name="x">鼠标X轴方向上的坐标</param>
        /// <param name="y">鼠标Y轴方向上的坐标</param>
        /// <returns>是否需要记录拖拽的单元格</returns>
        private void SetCursorStatus(int x, int y)
        {
            TPTextCell cell;
            string cursorStatus = GetCursorNeedStatus(x, y, out cell);
            if (cursorStatus == s_CursorVSplit)
            {
                if (myOwnerDocument.OwnerControl.Cursor != Cursors.VSplit)
                {
                    myOwnerDocument.OwnerControl.SetCursor(Cursors.VSplit);
                }
            }
            else if (cursorStatus == s_CursorHSplit)
            {
                //TODO: 鼠标调整高度,暂无实现;
                //myOwnerDocument.OwnerControl.SetCursor(Cursors.HSplit); 
            }
        }

        /// <summary>
        /// 设置需要记录拖拽的单元格
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetRecordDragCell(int x, int y)
        {
            TPTextCell cell;
            string cursorStatus = GetCursorNeedStatus(x, y, out cell);
            if (cursorStatus == s_CursorVSplit)
            {
                dragCell = cell;
            }
            else if (cursorStatus == s_CursorHSplit)
            {
                //TODO: 鼠标调整高度,暂无实现;
            }
        }

        /// <summary>
        /// 得到光标需要的状态VSplit或HSplit
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private string GetCursorNeedStatus(int x, int y, out TPTextCell dragCell)
        {
            Point p = new Point(x, y);
            dragCell = new TPTextCell();
            string cursorStatus = string.Empty;
            for (int i = 0; i < this.allRows.Count; i++)
            {
                //从第二列开始.cell的左边的边框总是存在的.
                for (int k = 1; k < this.allRows[i].Cells.Count; k++)
                {
                    TPTextCell cell = this.allRows[i].Cells[k];
                    if (cell.Merged == false)
                    {
                        if (p.X >= (cell.RealLeft - 5) && p.X <= (cell.RealLeft + 5) && p.Y >= (cell.RealTop) && p.Y <= (cell.RealTop + cell.Height))
                        {
                            cursorStatus = s_CursorVSplit;
                            dragCell = cell;
                        }
                        if (p.X >= (cell.RealLeft) && p.X <= (cell.RealLeft + cell.Width) && p.Y >= (cell.RealTop - 5) && p.Y <= (cell.RealTop + 5))
                        {
                            cursorStatus = s_CursorHSplit;

                            //TODO: 鼠标调整高度,暂无实现;
                        }
                    }
                }
            }
            return cursorStatus;
        }

        #endregion

        #region 绘制用来拖拽的纵向虚线

        //绘制虚线的矩形
        Rectangle m_MouseDownRect = Rectangle.Empty;

        private void ResizeToRectangle()
        {
            //绘制虚线前将原先的虚线擦除
            DrawRectangle();

            Point p = this.OwnerDocument.OwnerControl.PointToScreen(new Point(0, 0));//得到编辑器在Screen中的坐标

            //设置竖直虚线的位置
            m_MouseDownRect.X = Cursor.Position.X;
            m_MouseDownRect.Y = p.Y;
            m_MouseDownRect.Width = 1;
            m_MouseDownRect.Height = this.OwnerDocument.OwnerControl.Height;

            //绘制新的虚线
            DrawRectangle();
        }
        private void DrawRectangle()
        {
            ControlPaint.DrawReversibleFrame(m_MouseDownRect, Color.White, FrameStyle.Dashed);
        }

        #endregion

        #endregion

        /// <summary>
        /// 克隆TPTextTable，得到深层次的副本
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public TPTextTable Clone()
        {
            TPTextTable newTable = new TPTextTable();
            newTable.OwnerDocument = OwnerDocument;
            newTable.header = header;
            newTable.columns = columns;

            int defRowHeight = this.OwnerDocument.Content.CurrentElement.Height;
            int defTableWidth = totalWidth; ;

            newTable.defaultRowHeight = defRowHeight;
            newTable.defaultTableWidth = defTableWidth;
            newTable.totalWidth = totalWidth;

            newTable.SetTotalWidth(0);//自动扩展

            newTable.absoluteWidths = absoluteWidths;

            for (int i = 0; i < AllRows.Count; i++)
            {
                TPTextRow someRow = AllRows[i].Clone();
                someRow.Parent = newTable;
                someRow.OwnerTable = newTable;
                newTable.allRows.Add(someRow);
                newTable.myChildElements.Add(someRow);
            }

            newTable.CalculateHeights();
            return newTable;
        }
    }
}
