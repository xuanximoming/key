using DrectSoft.Library.EmrEditor.Src.Gui;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
namespace DrectSoft.Library.EmrEditor.Src.Document
{
    /// <summary>
    /// 表示一块元素集合的容器对象
    /// 
    /// </summary>
    public class ZYTextBlock : ZYTextContainer
    {
        #region bwy :
        string name = "";
        public string Code = "";
        //先不用WholeElement，它使代码更加复杂
        public ZYTextBlock()
        {
        }


        /// <summary>
        /// 是否是必选项
        /// </summary>
        public bool MustClick = false;

        /// <summary>
        /// 是否被点击关注过
        /// </summary>
        public bool Clicked = false;

        public new virtual string Name
        {
            get { return name; }
            set
            {
                name = value;
                //用于引发Text的set事件,选项元素需要这样，因为改选项名字的时候，不应该再显示原来的选项内容。
                this.Text = value;
            }
        }

        public string text = "";
        public virtual string Text
        {
            get
            {
                return text;
            }

            set
            {
                this.ChildElements.Clear();
                //edit by Ukey zhang 2017-11-10 
                //foreach 无法得到前后字符所以改用for
                if (WeiWenProcess.weiwen)
                {
                    for (int iCount = 0; iCount < value.Length; iCount++)
                    {
                        char myPreChar, myFontChar;
                        if (iCount == 0)
                            myPreChar = ' ';
                        else
                            myPreChar = value[iCount - 1];

                        if (iCount == value.Length - 1)
                            myFontChar = ' ';
                        else
                            myFontChar = value[iCount + 1];

                        ZYTextChar c = new ZYTextChar();
                        c.Char = WeiWenProcess.strPase(value[iCount], myPreChar, myFontChar);

                        Attributes.CopyTo(c.Attributes);
                        c.UpdateAttrubute();

                        c.Parent = this;
                        c.OwnerDocument = this.OwnerDocument;
                        this.ChildElements.Add(c);
                    }
                }
                else
                {
                    foreach (char myc in value)
                    {
                        ZYTextChar c = new ZYTextChar();
                        c.Char = myc;

                        Attributes.CopyTo(c.Attributes);
                        c.UpdateAttrubute();

                        c.Parent = this;
                        c.OwnerDocument = this.OwnerDocument;
                        this.ChildElements.Add(c);
                    }
                }
                text = value;
            }
        }

        #endregion bwy :

        /// <summary>
        /// 是否是关键区域
        /// </summary>
        public bool KeyField
        {
            get { return myAttributes.GetString(ZYTextConst.c_KeyField) != "0"; }
            set { myAttributes.SetValue(ZYTextConst.c_KeyField, (value ? "1" : "0")); }
        }

        /// <summary>
        /// 是否显示突出显示背景
        /// </summary>
        protected bool bolStandOutBack = false;


        /// <summary>
        /// 本容器的锁定标志为父容器的锁定标志
        /// </summary>
        public override bool Locked
        {
            get { return myParent.Locked; }
            set { base.Locked = value; }
        }
        /// <summary>
        /// block属性为真
        /// </summary>
        public override bool Block
        {
            get { return true; }
        }
        /// <summary>
        /// 已重载：块对象不强制分行
        /// </summary>
        /// <returns></returns>
        public override bool isNewLine()
        {
            return false;
        }
        /// <summary>
        /// 已重载：块对象不强制分断落
        /// </summary>
        /// <returns></returns>
        public override bool isNewParagraph()
        {
            return false;
        }
        /// <summary>
        /// 已重载：块对象不占一行
        /// </summary>
        /// <returns></returns>
        public override bool OwnerWholeLine()
        {
            return false;
        }
        /// <summary>
        /// 已重载:刷新视图状态,判断是否需要绘制背景
        /// </summary>
        /// <remarks>对文本框判断是否绘制背景的条件为
        /// 1.文档不处于打印模式
        /// 2.没有选中的内容
        /// 3.文档的当前鼠标悬浮的元素为该文本框或光标在文本框中</remarks>
        public override void ResetViewState()
        {
            bolStandOutBack = false;
            if (myOwnerDocument.Content.SelectLength != 0)
                return;
            if (myOwnerDocument.Content.CurrentElement == this
                || myChildElements.Contains(myOwnerDocument.Content.CurrentElement)
                || myOwnerDocument.CurrentHoverElement == this)
                bolStandOutBack = true;
        }//void ResetViewState()

        /// <summary>
        /// 已重载：获得包含对象的最小矩形
        /// </summary>
        /// <returns></returns>
        public override System.Drawing.Rectangle GetContentBounds()
        {
            System.Drawing.Rectangle rect = System.Drawing.Rectangle.Empty;
            foreach (ZYTextElement myElement in myChildElements)
            {
                if (rect.IsEmpty)
                    rect = Bounds;
                else
                    rect = System.Drawing.Rectangle.Union(rect, myElement.Bounds);
            }
            return System.Drawing.Rectangle.Union(rect, this.Bounds);
        }


        /// <summary>
        /// 已重载:处理鼠标进入文本域事件,重新绘制对象
        /// </summary>
        public override void HandleEnter()
        {
            RefreshForSelect();
        }
        /// <summary>
        /// 已重载:处理鼠标离开文本域事件,重新绘制对象
        /// </summary>
        public override void HandleLeave()
        {
            RefreshForSelect();
        }

        private void RefreshForSelect()
        {
            bool bolBack = this.bolStandOutBack;
            this.ResetViewState();
            if (bolBack != this.bolStandOutBack)
            {
                myOwnerDocument.RefreshElement(this);
            }
        }

        public override bool isTextElement()
        {
            return true;
        }
        StringFormat strFormat = StringFormat.GenericTypographic;
        Pen pen = new Pen(Color.Black);
        public override void DrawBackGround(ZYTextElement myElement)
        {
            if (myElement.Parent is ZYFixedText && this.OwnerDocument.Info.DocumentModel == DocumentModel.Edit)
            {
                return;
            }

            Rectangle rect = myElement.Bounds;

            if (myElement.Parent is ZYTextBlock)
            {
                if (myElement.Parent.LastElement == myElement && !WeiWenProcess.weiwen)
                    rect.Width -= 10;
            }


            //打印状态不绘制背景，其它状态绘制背景，但可能背景是透明的
            if (this.OwnerDocument.Info.Printing || this.OwnerDocument.OwnerControl.bolLockingUI)
            {

            }
            else
            {
                switch (ZYEditorControl.ElementStyle)
                {
                    case "下划线":
                        pen.Color = ZYEditorControl.ElementBackColor;

                        pen.Width = 1;//DrectSoft.Library.EmrEditor.Src.Gui.GraphicsUnitConvert.Convert(2, GraphicsUnit.Pixel,GraphicsUnit.Document );
                        this.OwnerDocument.View.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);

                        if (myElement.Parent is ZYTextBlock)
                        {
                            if (myElement.Parent.LastElement == myElement)
                            {
                                this.OwnerDocument.View.DrawLine(pen, rect.Right, rect.Bottom - 5, rect.Right, rect.Bottom + 5);
                            }

                        }

                        break;
                    case "背景色":
                        this.OwnerDocument.View.FillRectangle(ZYEditorControl.ElementBackColor, rect);
                        break;
                }
                //base.DrawBackGround(myElement);
            }


            //即使是只读状态，但如果它在激活区域中，视同编辑状态
            if (this.OwnerDocument.OwnerControl.ActiveEditArea != null)
            {
                if (this.OwnerDocument.OwnerControl.ActiveEditArea.Top <= myElement.RealTop && myElement.RealTop + this.Height <= this.OwnerDocument.OwnerControl.ActiveEditArea.End)
                {
                    switch (ZYEditorControl.ElementStyle)
                    {
                        case "下划线":
                            pen.Color = ZYEditorControl.ElementBackColor;
                            pen.Width = 2;
                            this.OwnerDocument.View.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                            break;
                        case "背景色":
                            this.OwnerDocument.View.FillRectangle(ZYEditorControl.ElementBackColor, rect);
                            break;
                    }
                }
            }
        }

        public override bool HandleClick(int x, int y, MouseButtons Button)
        {
            return base.HandleClick(x, y, Button);
        }
        //双击弹出窗口
        public override bool HandleDblClick(int x, int y, MouseButtons Button)
        {
            this.Clicked = true;

            //当前字符，用于判断是否在[]{}中
            ZYTextElement curChar = this.OwnerDocument.GetElementByPos(x, y);
            //Debug.WriteLine("block handledbclick 当前元素 " + curChar);

            //选项的字符串
            StringBuilder str = new StringBuilder();
            this.GetFinalText(str);
            int m = this.ChildElements.IndexOf(curChar);

            int tmpindex = -1;

            //方括号[]的配对索引
            List<int> start = new List<int>();
            List<int> end = new List<int>();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '[')
                {
                    start.Add(i);
                }
                if (str[i] == ']')
                {
                    end.Add(i);
                }
            }

            //花括号{}的配对索引
            List<int> startm = new List<int>();
            List<int> endm = new List<int>();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '{')
                {
                    startm.Add(i);
                }
                if (str[i] == '}')
                {
                    endm.Add(i);
                }
            }

            foreach (ZYTextElement ele in this.ChildElements)
            {
                if (ele.Bounds.Contains(x, y))
                {
                    this.Clicked = true;

                    if (this is ZYSelectableElement)
                    {
                        //如果当前字是选项模板中的一个，则替换模板
                        //替换子模板
                        //并把原有选项中内容展开，把[xxx]转换成真正的模板元素
                        ArrayList al = new ArrayList();
                        if (ele == curChar)
                        {
                            #region bwy //循环[]匹配的组
                            for (int i = 0; i < start.Count; i++)
                            {
                                if (start[i] < m && m < end[i])
                                {
                                    //弹出将要展开模板的警告 
                                    if (MessageBox.Show("确定要将选项展开为模板吗？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        this.OwnerDocument.BeginUpdate();
                                        this.OwnerDocument.BeginContentChangeLog();

                                        tmpindex = i;
                                        //展开
                                        this.OwnerDocument._Delete();
                                        string f = "[]";
                                        bool isintmp = false;
                                        string tmpname = "";
                                        foreach (char c in str.ToString())
                                        {
                                            if (f.IndexOf(c) == 0)
                                            {
                                                isintmp = true;
                                                tmpname = "";
                                                continue;
                                            }
                                            if (f.IndexOf(c) == 1)
                                            {
                                                isintmp = false;
                                                //插入名为tmpname的模板
                                                ZYTemplate tmp = new ZYTemplate();
                                                tmp.Name = tmpname;
                                                tmp.Parent = this.Parent;
                                                tmp.OwnerDocument = this.OwnerDocument;
                                                this.OwnerDocument._InsertBlock(tmp);

                                                al.Add(tmp);

                                                continue;
                                            }
                                            if (isintmp)
                                            {
                                                tmpname += c;
                                            }
                                            else
                                            {
                                                this.OwnerDocument.Content.InsertString(c.ToString());
                                            }
                                        }
                                        ZYTemplate tmp2 = al[tmpindex] as ZYTemplate;
                                        this.OwnerDocument.Content.CurrentElement = tmp2.FirstElement;
                                        tmp2.HandleDblClick(tmp2.FirstElement.RealLeft, tmp2.FirstElement.RealTop, Button);

                                        this.OwnerDocument.Content.SelectLength = 0;
                                        this.OwnerDocument.EndContentChangeLog();
                                        this.OwnerDocument.EndUpdate();

                                        Debug.WriteLine("应该展开模板 " + (al[tmpindex] as ZYTemplate).Name);
                                        return true;
                                    }
                                }
                            }
                            #endregion bwy

                            #region bwy 循环{}匹配的每一组
                            for (int j = 0; j < startm.Count; j++)
                            {
                                //如果当前元素在某个提示中间
                                if (startm[j] < m && m < endm[j])
                                {
                                    //弹出录入提示
                                    string tmpname = str.ToString().Substring(startm[j] + 1, endm[j] - startm[j] - 1);
                                    //弹出名为tmpname的录入提示
                                    ZYPromptText p = new ZYPromptText();
                                    p.Name = tmpname;
                                    p.Parent = this.Parent;
                                    p.OwnerDocument = this.OwnerDocument;

                                    FormatFrm HelpWinx = new FormatFrm(p, this as ZYSelectableElement, startm[j], endm[j]);
                                    HelpWinx.Show();
                                    return true;
                                }
                            }
                            #endregion bwy



                        }

                        ImplementFrm HelpWin = new ImplementFrm((ZYSelectableElement)this);
                        HelpWin.Show();
                        //Debug.WriteLine("显示弹出窗口OK");
                        return true;
                    }
                    if (this is ZYFormatDatetime || this is ZYFormatNumber || this is ZYFormatString || this is ZYPromptText)
                    {
                        FormatFrm HelpWin = new FormatFrm(this);
                        HelpWin.Show();
                        return true;
                    }

                    if (this is ZYTemplate)
                    {
                        this.OwnerDocument.ReplaceTemplate(this.Type, this.Name);
                        return true;
                    }

                    if (this is ZYReplace)
                    {
                        TextBoxFrm TextWin = new TextBoxFrm(this);
                        TextWin.ShowDialog();
                        return true;
                    }
                    if (this is ZYLookupEditor)
                    {
                        LookupEditorForm TextWin = new LookupEditorForm(this);
                        if (TextWin.NormalWordBook == null || TextWin.NormalWordBook == "")
                            return false;
                        TextWin.ShowDialog();
                        return true;
                    }
                }
            }

            return base.HandleDblClick(x, y, Button);

        }

        public override bool HandleMouseDown(int x, int y, System.Windows.Forms.MouseButtons Button)
        {
            //MessageBox.Show("HandleMouseDown");
            if (Button == MouseButtons.Right)
            {
                contextMenu.Show(Control.MousePosition);
            }
            return base.HandleMouseDown(x, y, Button);
        }

        public override bool ToXML(XmlElement myElement)
        {
            myElement.SetAttribute("mustclick", this.MustClick.ToString());
            myElement.SetAttribute("code", this.Code);
            //myElement.SetAttribute("clicked", this.Clicked.ToString());
            return true;
            //return base.ToXML(myElement);
        }

        public override bool FromXML(XmlElement myElement)
        {
            this.MustClick = myElement.GetAttribute("mustclick") != "" ? bool.Parse(myElement.GetAttribute("mustclick")) : false;
            this.Code = myElement.GetAttribute("code");
            //this.Clicked = bool.Parse(myElement.GetAttribute("clicked"));
            return true;
            //return base.FromXML(myElement);
        }
    }// class ZYTextBlock
}