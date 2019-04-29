using DrectSoft.Library.EmrEditor.Src.Actions;
using DrectSoft.Library.EmrEditor.Src.Clipboard;
using DrectSoft.Library.EmrEditor.Src.Common;
using DrectSoft.Library.EmrEditor.Src.Gui;
using DrectSoft.Library.EmrEditor.Src.Print;
using DrectSoft.Library.EmrEditor.Src.Undo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using XDesignerPrinting;
using ZYTextDocumentLib;

namespace DrectSoft.Library.EmrEditor.Src.Document
{
    #region  EnumElementHandler  JumpDivHandler
    ///// <summary>
    ///// 光标在文本块间跳跃时的处理委托
    ///// </summary>
    ///// <param name="DivForm" type="ZYTextDocumentLib.ZYTextDiv">开始离开的文本块对象</param>
    ///// <param name="DivTo" type="ZYTextDocumentLib.ZYTextDiv">到达的文本块对象</param>
    //public delegate void JumpDivHandler(ZYTextDiv DivFrom, ZYTextDiv DivTo);
    /// <summary>
    /// 遍历文档中所有元素的处理委托
    /// </summary>
    /// <param name="Parent" type="ZYTextDocumentLib.ZYTextContainer">当前遍历的元素的父元素对象</param>
    /// <param name="Element" type="ZYTextDocumentLib.ZYTextElement">当前遍历的元素</param>
    /// <returns>是否继续遍历</returns>
    public delegate bool EnumElementHandler(ZYTextContainer Parent, ZYTextElement Element);
    /// <summary>
    /// 光标在文本块间跳跃时的处理委托
    /// </summary>
    /// <param name="DivForm" type="ZYTextDocumentLib.ZYTextDiv">开始离开的文本块对象</param>
    /// <param name="DivTo" type="ZYTextDocumentLib.ZYTextDiv">到达的文本块对象</param>
    public delegate void JumpDivHandler(ZYTextDiv DivFrom, ZYTextDiv DivTo);
    /// <summary>
    /// Add By wwj 2011-09-23 调用外部“查找”窗体
    /// </summary>
    /// <param name="selectedText"></param>
    public delegate void ShowFindFormHandler();
    /// <summary>
    /// Add By wwj 2011-09-23 调用外部“保存”功能
    /// </summary>
    public delegate void SaveEMRHandler();
    #endregion

    [Serializable]
    public class ZYTextDocument : System.IDisposable, IPageDocument, ICloneable
    {

        #region 构造函数,初始化一个文档对象,并自动添加一个根元素
        /// <summary>
        /// 构造函数,初始化一个文档对象,并自动添加一个根元素
        /// </summary>
        public ZYTextDocument()
        {
            myContent.Document = this;
            myContent.Elements = myElements;


            RootDocumentElement = new ZYTextDiv();

            RootDocumentElement.OwnerDocument = this;

            Info.OwnerDocument = this;

            //加载xml到文档对象模型中
            this.RefreshElements();
            myView.Width = myPages.StandardWidth;
            mySaveLogs.OwnerDocument = this;

        }
        #endregion

        #region ----------------------变量列表-----------------------------
        //internal int FirstMarkSaveLogIndex = 0 ;
        private System.Drawing.GraphicsUnit intDocumentGraphicsUnit = System.Drawing.GraphicsUnit.Document;
        /// <summary>
        /// 是否允许选择打印
        /// </summary>
        protected bool bolEnableSelectionPrint = false;

        /// <summary>
        /// 视图对象
        /// </summary>
        private DocumentView myView = new DocumentView();
        /// <summary>
        /// 文档内容对象
        /// </summary>
        private ZYContent myContent = new ZYContent();
        /// <summary>
        /// 视图控件对象
        /// </summary>
        private ZYEditorControl myOwnerControl = null;
        /// <summary>
        /// 根元素
        /// </summary>
        public ZYTextContainer RootDocumentElement = null;

        /// <summary>
        /// 元素列表
        /// </summary>
        private System.Collections.ArrayList myElements = new System.Collections.ArrayList();

        private ZYTextSaveLogCollection mySaveLogs = new ZYTextSaveLogCollection();
        /// <summary>
        /// 文档信息及设置对象
        /// </summary>
        private ZYDocumentInfo myInfo = new ZYDocumentInfo();

        protected System.Windows.Forms.Cursor myCursor = System.Windows.Forms.Cursors.Default;//null;
        /// <summary>
        /// 正在加载文档
        /// </summary>
        protected bool bolLoading = false;

        /// <summary>
        /// 当前处于鼠标悬浮状态的元素
        /// </summary>
        protected ZYTextElement myCurrentHoverElement = null;

        /// <summary>
        /// 所有的文本行对象的集合
        /// </summary>
        protected System.Collections.ArrayList myLines = new System.Collections.ArrayList();

        /// <summary>
        /// 文档参数列表对象
        /// </summary>
        protected NameValueList myVariables = new NameValueList();

        /// <summary>
        /// 文档元素对象树中所有的容器对象
        /// </summary>
        protected System.Collections.ArrayList myContainters = new System.Collections.ArrayList();

        /// <summary>
        /// 当前支持的文档格式的版本号
        /// </summary>
        protected const string c_EditorVersion = "1.0";

        /// <summary>
        /// 刷新全部用户界面的标志
        /// </summary>
        public bool RefreshAllFlag = false;

        /// <summary>
        /// 隐藏的元素的名称
        /// </summary>
        protected string[] strHideElementNames = null;
        /// <summary>
        /// 纸张设置
        /// </summary>
        protected System.Drawing.Printing.PageSettings myPageSettings = new System.Drawing.Printing.PageSettings();

        /// <summary>
        /// 元素列表发生改变标志，需要重新分行
        /// </summary>
        public bool ElementsDirty = false;

        /// <summary>
        /// 当选择区域发生改变时的事件处理
        /// </summary>
        public event System.EventHandler OnSelectionChanged = null;
        /// <summary>
        /// 当文档内容发生改变时的事件处理
        /// </summary>
        public event System.EventHandler OnContentChanged = null;

        /// <summary>
        /// 当光标在文本块中跳跃时的时间处理
        /// </summary>
        public event JumpDivHandler OnJumpDiv = null;

        /// <summary>
        /// Add By wwj 2011-09-23 调用外部“查找”窗体
        /// </summary>
        public event ShowFindFormHandler OnShowFindForm = null;

        /// <summary>
        /// Add By wwj 2011-09-23 调用外部“保存”功能
        /// </summary>
        public event SaveEMRHandler OnSaveEMR = null;

        /// <summary>
        /// 页眉高度
        /// </summary>
        protected int intHeadHeight = 300;
        /// <summary>
        /// 页脚高度
        /// </summary>
        protected int intFooterHeight = 100;

        bool showHeader = true;
        bool showFooter = true;

        /// <summary>
        /// 页面对象集合
        /// </summary>
        protected PrintPageCollection myPages = new PrintPageCollection();
        /// <summary>
        /// 页号修正量
        /// </summary>
        private int intPageIndexFix = 1;
        /// <summary>
        /// 当前打印的页面序号
        /// </summary>
        private int intPageIndex = 0;

        /// <summary>
        /// 是否允许续打
        /// </summary>
        protected bool bolEnableJumpPrint = false;
        /// <summary>
        /// 续打位置
        /// </summary>
        protected int intJumpPrintPosition = 0;

        /// <summary>
        /// 签名对象集合
        /// </summary>
        protected UnderWriteMarkCollection myMarks = new UnderWriteMarkCollection();
        /// <summary>
        /// 撤销操作状态 0:正常编辑状态 1:正在撤销操作 2:正在重复操作
        /// </summary>
        protected int intUndoState = 0;
        /// 文档内容是否被锁定
        /// </summary>
        private bool bolLocked = false;

        private int intLockLevel = -1;
        /// <summary>
        /// 页脚字符串
        /// </summary>
        protected string strFooterString = "<footer><p><horizontalLine lineHeight='1'></horizontalLine></p><p align='2'><span fontname='宋体' fontsize='10' >第[%pageindex%]页 / 共[%pagecount%]页</span></p></footer>";

        /// <summary>
        /// 页眉字符串
        /// </summary>
        protected string strHeadString = @"<header><p align='2'><span fontname='宋体' fontsize='14'>医院名称</span><eof /></p><p align='2'><span fontname='宋体' fontsize='14' fontbold='1'>子标题</span><eof /></p><p align='0'><span fontname='宋体' fontsize='14'>姓名：</span><macro fontname='宋体' fontsize='14' type='macro' name='姓名'>姓名</macro><span fontname='宋体' fontsize='14'>　　性别：</span><macro fontname='宋体' fontsize='14' type='macro' name='性别'>性别</macro><span fontname='宋体' fontsize='14'>　　年龄：</span><macro fontname='宋体' fontsize='14' type='macro' name='年龄'>年龄</macro><span fontname='宋体' fontsize='14'>　　民族：</span><macro fontname='宋体' fontsize='14' type='macro' name='民族'>民族</macro><eof /></p><p align='0'><horizontalLine type='' name='水平线' lineHeight='2' percent='100' /><eof /></p></header>";

        /// <summary>
        /// 实例化一个重做栈
        /// </summary>
        UndoStack undoStack = new UndoStack();
        /// <summary>
        /// 对文档的操作日志
        /// </summary>
        private A_ContentChangeLog myContentChangeLog = null;
        /// <summary>
        /// 文档更改等级
        /// </summary>
        private int intLogLevel = 0;

        /// <summary>
        /// 页脚显示的页数
        /// </summary>
        public string PageIndexForNurse
        {
            set
            {
                if (!string.IsNullOrEmpty(value.Trim()) && value != "-1")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strFooterString);
                    if (doc.InnerText.Trim() == "")
                    {
                        strFooterString = "<footer><p align='0'><horizontalLine type='' name='水平线' lineHeight='2' percent='100' /><eof /></p><p align='2'><span fontname='宋体' fontsize='10' >第 " + value + " 页</span></p></footer>";
                    }
                }
            }
        }

        #endregion ----------------------变量列表-----------------------------

        #region ----------------------属性列表-----------------------------

        /// <summary>
        /// 是否允许选择打印
        /// </summary>
        public bool EnableSelectionPrint
        {
            get { return bolEnableSelectionPrint; }
            set { bolEnableSelectionPrint = value; }
        }

        /// <summary>
        /// 文档使用的绘图单位
        /// </summary>
        public System.Drawing.GraphicsUnit DocumentGraphicsUnit
        {
            get { return intDocumentGraphicsUnit; }
            set { intDocumentGraphicsUnit = value; }
        }
        #region 页面

        /// <summary>
        /// 页面对象集合
        /// </summary>
        public PrintPageCollection Pages
        {
            get { return myPages; }
            set { myPages = value; }
        }

        /// <summary>
        /// 页号修正量
        /// </summary>
        public int PageIndexFix
        {
            get { return intPageIndexFix; }
            set { intPageIndexFix = value; }
        }

        /// <summary>
        /// 当前打印的页面序号
        /// </summary>
        public int PageIndex
        {
            get { return intPageIndex; }
            set { intPageIndex = value; }
        }
        #endregion

        #region 续打

        /// <summary>
        /// 是否允许续打
        /// </summary>
        public bool EnableJumpPrint
        {
            get { return bolEnableJumpPrint; }
            set { bolEnableJumpPrint = value; }
        }

        /// <summary>
        /// 续打位置
        /// </summary>
        public int JumpPrintPosition
        {
            get { return intJumpPrintPosition; }
            set { intJumpPrintPosition = this.FixPageLine(value); }
        }
        #endregion

        #region 选中区域打印 Add By wwj 2012-04-17  【注意：不同于上面的选择打印】

        /// <summary>
        /// 是否允许选中区域打印
        /// </summary>
        public bool EnableSelectAreaPrint
        {
            get { return bolEnableSelectAreaPrint; }
            set { bolEnableSelectAreaPrint = value; }
        }
        private bool bolEnableSelectAreaPrint = false;

        /// <summary>
        /// 选中区域打印左上角的坐标
        /// </summary>
        public Point SelectAreaPrintLeftTopPoint { get; set; }
        /// <summary>
        /// 选中区域打印右下角的坐标
        /// </summary>
        public Point SelectAreaPrintRightBottomPoint { get; set; }

        #endregion

        /// <summary>
        /// 保存记录的对象集合
        /// </summary>
        public ZYTextSaveLogCollection SaveLogs
        {
            get { return mySaveLogs; }
        }
        /// <summary>
        /// 纸张设置
        /// </summary>
        public System.Drawing.Printing.PageSettings PageSettings
        {
            get { return myPageSettings; }
            set { myPageSettings = value; }
        }
        public int HeadHeight
        {
            get
            {
                if (this.showHeader)
                    return intHeadHeight;
                else
                    return 0;
            }
            set
            {

                intHeadHeight = value;
            }
        }
        public int FooterHeight
        {
            get
            {
                if (this.ShowFooter)
                    return intFooterHeight;
                else
                    return 0;
            }
            set
            {
                intFooterHeight = value;
            }
        }
        public bool ShowHeader
        {
            get { return showHeader; }
            set
            {
                showHeader = value;
                OwnerControl.DrawTopMargin = !value;
                //this.RefreshSize();
                this.RefreshLine();
                this.Refresh();
                UpdateTextCaret();
            }
        }

        bool showHeaderLine = true;
        public bool ShowHeaderLine
        {
            get { return showHeaderLine; }
            set
            {
                showHeaderLine = value;
                this.OwnerControl.Refresh();
            }
        }

        bool showFooterLine = true;
        public bool ShowFooterLine
        {
            get { return showFooterLine; }
            set
            {
                showFooterLine = value;
                this.OwnerControl.Refresh();
            }
        }

        public bool ShowFooter
        {
            get { return showFooter; }
            set
            {
                OwnerControl.DrawBottomMargin = !value;
                showFooter = value;
                //this.RefreshSize();
                this.RefreshLine();
                this.Refresh();
                UpdateTextCaret();

            }
        }
        /// <summary>
        /// 签名对象集合
        /// </summary>
        public UnderWriteMarkCollection Marks
        {
            get { return myMarks; }
        }
        /// <summary>
        /// 返回文档参数列表对象
        /// </summary>
        public NameValueList Variables
        {
            get { return myVariables; }
            set { myVariables = value; }
        }
        /// <summary>
        /// 隐藏的元素
        /// </summary>
        public string HideElementNames
        {
            get
            {
                if (strHideElementNames == null)
                    return null;
                System.Text.StringBuilder myStr = new System.Text.StringBuilder();
                foreach (string strName in strHideElementNames)
                {
                    if (myStr.Length == 0)
                        myStr.Append(strName);
                    else
                        myStr.Append(strName + ",");
                }
                return myStr.ToString();
            }
            set { strHideElementNames = value.Split(",".ToCharArray()); }
        }

        /// <summary>
        /// 设置获得当前处于鼠标悬浮状态的元素
        /// </summary>
        public ZYTextElement CurrentHoverElement
        {
            get { return myCurrentHoverElement; }
            set
            {
                //				if( value == null)
                //					System.Console.WriteLine("aaaaaaa");
                if (myCurrentHoverElement != value)
                {
                    ZYTextElement oldElement = myCurrentHoverElement;
                    myCurrentHoverElement = value;
                    if (oldElement != null)
                        this.RefreshElement(oldElement);
                    if (myCurrentHoverElement != null)
                        this.RefreshElement(myCurrentHoverElement);
                }
            }
        }

        /// <summary>
        /// 文档中所有的文本行对象的集合
        /// </summary>
        public System.Collections.ArrayList Lines
        {
            get { return myLines; }
            set { myLines = value; }
        }
        /// <summary>
        /// 只读,文档正在加载标志
        /// </summary>
        public bool Loading
        {
            get { return bolLoading; }
            set { bolLoading = value; }
        }


        public UndoStack UndoStack
        {
            get
            {
                return undoStack;
            }
        }
        /// <summary>
        /// 返回文档对象处理撤销,重复操作的状态 0:正常编辑状态 1:正在撤销操作 2:正在重复操作
        /// </summary>
        public int UndoState
        {
            get { return intUndoState; }
        }
        /// <summary>
        /// 是否可以进行内容修改的记录
        /// </summary>
        public bool CanContentChangeLog
        {
            get
            {
                return myContentChangeLog != null && myContentChangeLog.CanLog;
            }
        }

        /// <summary>
        /// 当前用于记录元素新增或删除操作的对象
        /// </summary>
        public A_ContentChangeLog ContentChangeLog
        {
            get { return myContentChangeLog; }
        }

        public int LockLevel
        {
            get { return intLockLevel; }
        }
        /// <summary>
        /// 文档内容是否被锁住不能修改
        /// </summary>
        public bool Locked
        {
            get
            {
                //				if( myContent.GetMaxLockLevel() >= mySaveLogs.CurrentSaveLog.Level )
                //					return true;
                //				else
                return bolLocked;
            }
            set
            {
                bolLocked = value;
            }
        }
        /// <summary>
        /// 文档内容是否改变
        /// </summary>
        public bool Modified
        {
            get { return myContent.Modified; }
            set { myContent.Modified = value; }
        }

        /// <summary>
        /// 文档内容对象
        /// </summary>
        public ZYContent Content
        {
            get { return myContent; }
            set { myContent = value; }
        }

        /// <summary>
        /// 是否自动清除选中状态
        /// </summary>
        public bool AutoClearSelection
        {
            get { return myContent.AutoClearSelection; }
            set { myContent.AutoClearSelection = value; }
        }

        /// <summary>
        /// 文档对象的文档信息设置对象
        /// </summary>
        public ZYDocumentInfo Info
        {
            get { return myInfo; }
            set { myInfo = value; }
        }

        public int DefaultRowHeight
        {
            get
            {
                return this.PixelToDocumentUnit(myView.DefaultRowPixelHeight);
            }
        }
        /// <summary>
        /// 绘制文档使用的视图对象
        /// </summary>
        public DocumentView View
        {
            get { return myView; }
            set { myView = value; }
        }

        /// <summary>
        /// 页脚字符串
        /// </summary>
        public string FooterString
        {
            get { return strFooterString; }
            set
            {
                strFooterString = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 页眉字符串
        /// </summary>
        public string HeadString
        {
            get { return strHeadString; }
            set
            {
                strHeadString = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 设置页眉xmldoc文档
        /// </summary>
        public XmlDocument HeadDocument
        {
            set
            {

                XmlElement head = value.SelectSingleNode("document/body") as XmlElement;
                string str = head.OuterXml;
                str = str.Replace("<body", "<header");
                str = str.Replace("body>", "header>");
                this.HeadString = str;


                this.Refresh();
            }
        }
        /// <summary>
        /// 设置页脚xmldoc文档
        /// </summary>
        public XmlDocument FootDocument
        {
            set
            {
                XmlElement foot = value.SelectSingleNode("document/body") as XmlElement;
                string str = foot.OuterXml;
                str = str.Replace("<body", "<footer");
                str = str.Replace("body>", "footer>");
                this.FooterString = str;
                this.Refresh();
            }
        }

        public string RuntimeHeadString
        {
            get { return ExecuteVariable(strHeadString); }
        }
        public string RuntimeFooterString
        {
            get { return ExecuteVariable(strFooterString); }
        }
        /// <summary>
        /// 设置返回根元素
        /// </summary>
        public ZYTextContainer DocumentElement
        {
            get { return RootDocumentElement; }
            set { RootDocumentElement = value; myElements.Clear(); }
        }

        /// <summary>
        /// 返回所有显示的元素集合
        /// </summary>
        public System.Collections.ArrayList Elements
        {
            get { return myElements; }
            set { myElements = value; }
        }
        /// <summary>
        /// 返回编辑该文档的视图用户界面控件
        /// </summary>
        public ZYEditorControl OwnerControl
        {
            get { return myOwnerControl; }
            set
            {
                if (myOwnerControl != value)
                {
                    myOwnerControl = value;
                    if (myOwnerControl != null)
                    {
                        myOwnerControl.Document = this;
                        myView.Graph = myOwnerControl.CreateViewGraphics();
                        myOwnerControl.Pages = this.myPages;
                    }
                }
            }
        }



        #endregion ----------------------属性列表-----------------------------

        public Object Clone()
        {
            return new object();
        }
        public ZYTextElement GetElementByPos(int x, int y)
        {
            return myContent.GetElementAt(x, y);
        }

        internal void UpdateUserName()
        {
            if (myMarks.CanMark(mySaveLogs.CurrentUserName))
                bolLocked = false;
            else
                bolLocked = myInfo.LockForMark;
            this.Refresh();
        }

        public int GetMarkLevel(int savelogindex)
        {
            if (savelogindex >= 0 && savelogindex < mySaveLogs.Count)
                return mySaveLogs[savelogindex].Level;
            else
                return 0;
        }

        /// <summary>
        /// 向文本行集合添加文本行对象
        /// </summary>
        /// <param name="myLine">要添加的文本行对象</param>
        public void AddLine(ZYTextLine myLine)
        {
            myLines.Add(myLine);
        }

        /// <summary>
        /// 设置用户界面的鼠标光标
        /// </summary>
        /// <param name="vCursor">新的鼠标光标对象</param>
        internal void SetCursor(System.Windows.Forms.Cursor vCursor)
        {
            myCursor = vCursor;
            this.OwnerControl.Cursor = vCursor;
        }



        /// <summary>
        /// 向文档对象注册用于取消操作的动作对象
        /// </summary>
        /// <param name="a"></param>
        public void RegisteUndo(ZYEditorAction a)
        {
            if (intUndoState == 0 && a != null && a.isUndoable())
            {
                ZYEditorAction NewA = a.Clone();
                if (NewA != null)
                {
                    //myUndoList.Add(NewA);
                    //myRedoList.Clear();

                    UndoStack.Push(NewA);
                    UndoStack.ClearRedoStack();
                }
            }
        }

        #region 用于记录内容改变信息的函数群 **********************************************


        /// <summary>
        /// 开始记录内容的改变
        /// </summary>
        public void BeginContentChangeLog()
        {
            if (intLogLevel <= 0)
            {
                myContentChangeLog = new A_ContentChangeLog();
                myContentChangeLog.OwnerDocument = this;
                myContentChangeLog.SelectStart = myContent.SelectStart;
                myContentChangeLog.SelectLength = myContent.SelectLength;

            }
            intLogLevel++;
        }
        /// <summary>
        /// 结束记录内容的改变
        /// </summary>
        public void EndContentChangeLog()
        {
            intLogLevel--;
            if (intLogLevel <= 0)
            {
                if (myContentChangeLog != null && myContentChangeLog.isEnable())
                {
                    myContentChangeLog.SelectStart2 = myContent.SelectStart;
                    myContentChangeLog.SelectLength2 = myContent.SelectLength;
                    this.RegisteUndo(myContentChangeLog);
                    myContentChangeLog = null;
                }
                intLogLevel = 0;
            }
        }



        #endregion

        /// <summary>
        /// 判断文档内容是否可以修改
        /// </summary>
        /// <returns></returns>
        public bool CanModify()
        {
            //return !this.Locked;
            return true;
        }

        private string ExecuteVariable(string txt)
        {
            XDesignerCommon.VariableString str = new XDesignerCommon.VariableString(txt);
            str.SetVariable("pageindex", Convert.ToString(this.intPageIndex + this.intPageIndexFix));
            str.SetVariable("pagecount", (this.myPages.Count + this.intPageIndexFix - 1).ToString());//Modified by wwj 2013-03-29 总页数需要与页号修正量“intPageIndexFix”联动
            System.DateTime dtm = ZYTime.GetServerTime();//System.DateTime.Now ;
            str.SetVariable("year", dtm.Year.ToString());
            str.SetVariable("month", dtm.Month.ToString());
            str.SetVariable("dy", dtm.Day.ToString());
            str.SetVariable("hour", dtm.Hour.ToString());
            str.SetVariable("minute", dtm.Minute.ToString());
            str.SetVariable("secend", dtm.Second.ToString());
            return str.Execute();

            //return "返回运行时动态值";
        }
        public void setPatientid(string id)
        {
            myInfo.PatientID = id;
        }


        /// <summary>
        /// 获得元素在文档元素列表中从0开始的序号
        /// </summary>
        /// <param name="myElement">元素对象</param>
        /// <returns>从0开始的序号</returns>
        public int IndexOf(ZYTextElement myElement)
        {
            return myElements.IndexOf(myElement);
        }

        public int PixelToDocumentUnit(int len)
        {
            return DrectSoft.Library.EmrEditor.Src.Gui.GraphicsUnitConvert.Convert(len, System.Drawing.GraphicsUnit.Pixel, this.intDocumentGraphicsUnit);
        }
        public System.Drawing.Size PixelToDocumentUnit(System.Drawing.Size size)
        {
            return DrectSoft.Library.EmrEditor.Src.Gui.GraphicsUnitConvert.Convert(size, System.Drawing.GraphicsUnit.Pixel, this.intDocumentGraphicsUnit);
        }
        public System.Drawing.Point PixelToDocumentUnit(System.Drawing.Point p)
        {
            return DrectSoft.Library.EmrEditor.Src.Gui.GraphicsUnitConvert.Convert(p, System.Drawing.GraphicsUnit.Pixel, this.intDocumentGraphicsUnit);
        }



        #region 实现 IDisposable 接口,销毁对象,释放所占据的资源
        /// <summary>
        /// 实现 IDisposable 接口,销毁对象,释放所占据的资源
        /// </summary>
        public void Dispose()
        {
            if (myView != null)
                myView.Dispose();
            //if (myDataSource != null && myDataSource.DBConn != null)
            //    myDataSource.DBConn.Dispose();
            //if (myEMRVBEngine != null)
            //    myEMRVBEngine.Close();
            foreach (ZYTextElement myElement in myElements)
            {
                if (myElement is System.IDisposable)
                {
                    (myElement as System.IDisposable).Dispose();
                }
            }
        }
        #endregion

        #region 电子病历文本文档对象树操作函数群 **************************************************
        /// <summary>
        /// 判断元素是否处于选择状态,若处于打印状态则永远返回false
        /// </summary>
        /// <param name="myElement">元素对象</param>
        /// <returns>true 该元素处于选择状态, false 不处于选择状态</returns>
        public bool isSelected(ZYTextElement myElement)
        {
            if (myInfo.Printing || myElement == null)
                return false;
            else
            {
                if (myContent.isSelected(myElement))
                    return true;
                else if (myElement.Parent is ZYTextBlock && myContent.isSelected(myElement.Parent))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 判断元素是否可以显示和打印
        /// </summary>
        /// <remarks>本文档对象支持显示文本层次,设置的文本层次和对象的创建或逻辑删除者编号有关系
        /// 当显示文本层次有效时,当元素的创建者序号大于该文本层次则不显示,
        /// 当元素的逻辑删除者序号大于该文本层次大于该文本层次则显示
        /// 若显示文本层次无效则根据ShowAll设置和元素是否标记的逻辑删除来处理
        /// 当设置了ShowAll 属性或者 元素没有逻辑删除则显示该元素,否则不显示</remarks>
        /// <param name="myElement">元素对象</param>
        /// <returns>是否可以显示和打印</returns>
        /// <seealso>ZYTextDocumentLib.ZYTextDocument.VisibleUserLevel</seealso>
        /// <seealso>ZYTextDocumentLib.ZYTextDocument.ShowAll</seealso>
        public bool isVisible(ZYTextElement myElement)
        {
            //if( bolDesignMode == false && myElement.Parent == myDocumentElement && ! (myElement is ZYTextContainer) )				return false;
            if (myElement.CreatorIndex == -1)
                return true;
            if (myInfo.VisibleUserLevel >= 0)
            {
                if (myElement.CreatorIndex > myInfo.VisibleUserLevel)
                    return false;
                if (myElement.DeleterIndex > myInfo.VisibleUserLevel)
                    return true;
            }
            else if (myInfo.ShowAll)
            {
                return true;
            }
            return !myElement.Deleteted;
        }
        /// <summary>
        /// 使用指定的委托进行元素的遍历
        /// </summary>
        /// <param name="vHandler">遍历元素使用的委托</param>
        public void EnumElements(EnumElementHandler vHandler)
        {
            if (RootDocumentElement != null && vHandler != null)
            {
                RootDocumentElement.EnumChildElements(vHandler, true);
            }
        }

        /// <summary>
        /// 获得所有最终可显示的元素的集合,最终可显示元素就是无用户显示层次设置且不显示逻辑删除的元素时可显示的元素
        /// </summary>
        /// <returns>保存最终可显示的元素的列表</returns>
        public System.Collections.ArrayList GetFinalElements()
        {
            System.Collections.ArrayList myList = new System.Collections.ArrayList();
            int iBack = myInfo.VisibleUserLevel;
            myInfo.VisibleUserLevel = -1;
            bool bBack = myInfo.ShowAll;
            myInfo.ShowAll = false;
            RootDocumentElement.AddElementToList(myList, false);
            myInfo.ShowAll = bBack;
            myInfo.VisibleUserLevel = iBack;
            return myList;
        }
        /// <summary>
        /// 获得最终可显示的文本
        /// </summary>
        /// <returns>文本数据</returns>
        public string GetFinalText()
        {
            return ZYTextElement.GetElementsText(this.GetFinalElements());
        }

        /// <summary>
        /// 获得从指定序号开始遍历可见元素列表,返回第一个没有删除标记的元素
        /// </summary>
        /// <param name="index">开始的序号，若小于0则为当前插入点的序号</param>
        /// <returns>找到的元素，若没找到则返回空</returns>
        public ZYTextElement GetLastNotDeletedElement(int index)
        {
            if (index < 0 || index >= myElements.Count)
                index = myContent.SelectStart;
            for (int iCount = index; iCount < myElements.Count; iCount++)
            {
                ZYTextElement myElement = (ZYTextElement)myElements[iCount];
                if (myElement.Deleteted == false)
                    return myElement;
            }
            return null;
        }

        /// <summary>
        /// 删除文档中所有的空白行
        /// </summary>
        /// <returns>删除的元素个数</returns>
        public int RemoveBlankLine()
        {
            int DeleteCount = RootDocumentElement.RemoveBlankLine();
            if (DeleteCount > 0)
            {
                this.Modified = true;
                this.RefreshElements();
                this.RefreshLine();
                this.UpdateView();
            }
            return DeleteCount;
        }

        /// <summary>
        /// 删除所有的空白的关键字数据域及其周围的字符
        /// </summary>
        /// <param name="ContentLog"></param>
        /// <returns></returns>
        public int RemoveBlankKeyField(bool ContentLog)
        {
            int DeleteCount = RootDocumentElement.RemoveBlankKeyField2(ContentLog);
            if (DeleteCount > 0)
            {
                this.Modified = true;
                this.RefreshElements();
                this.RefreshLine();
                this.UpdateView();
            }
            return DeleteCount;
        }//public int RemoveBlankKeyField( bool ContentLog)

        #endregion



        /// <summary>
        /// 内部的向文档插入新元素前的预处理
        /// </summary>
        /// <remarks>本函数用于进行新增元素的预处理,若正在加载文档则立即返回true
        /// 否则对将要新增的元素进行信息设置,设置它的创建者为当用户,设置它的创建时间,
        /// 若新增的元素为字符元素,若创建者序号为1则设置文本为深蓝色,若为2则设置深红色</remarks>
        /// <param name="container">将要插入元素的父元素</param>
        /// <param name="NewElement">新插入的元素</param>
        /// <returns>是否插入新元素,若为false则取消插入</returns>
        internal bool BeforeInsertElemnt(ZYTextContainer container, ZYTextElement NewElement)
        {
            if (bolLoading == false)
            {
                //if (myContentChangeLog != null)
                //    this.myContentChangeLog.CanLog = false;
                NewElement.DeleterIndex = -1;
                //NewElement.DeleteTime = null;
                //NewElement.CreatorIndex = mySaveLogs.CurrentIndex;
                //NewElement.CreateTime = StringCommon.GetNowString12();
                //ZYTextChar myChar = NewElement as ZYTextChar;
                //if (myChar != null)
                //{
                //    if (myMarks.Count == 1)
                //    {
                //        myChar.ForeColor = System.Drawing.Color.DarkBlue;
                //    }
                //    if (myMarks.Count >= 2)
                //    {
                //        myChar.ForeColor = System.Drawing.Color.DarkRed;
                //    }
                //}
                //if (myContentChangeLog != null)
                //    this.myContentChangeLog.CanLog = true;
            }
            return true;
        }

        #region 编辑文档的通用例程 ****************************************************************

        /// <summary>
        /// 更新光标区域,暂无作用
        /// </summary>
        public void UpdateCaret()
        {
            ZYTextElement myElement = myContent.CurrentElement;
            //myOwnerControl.MoveTextCaretTo(myElement.RealLeft + myElement.Width - 2, myElement.RealTop + myElement.Height, myElement.Height);
            myOwnerControl.MoveTextCaretTo(myElement.RealLeft, myElement.RealTop + myElement.Height, myElement.Height);

        }

        /// <summary>
        /// 查找字符
        /// </summary>
        /// <remarks></remarks>
        /// <param name="strFind">要查找的字符串</param>
        public bool _Find(string strFind)
        {
            return myContent.FindText(strFind);
        }
        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="strFind">要查找的字符串</param>
        /// <param name="strReplace">替换后的字符串</param>
        public bool _Replace(string strFind, string strReplace)
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            string msg = "";
            bool isOK = myContent.ReplaceText(strFind, strReplace, out msg);
            myContentChangeLog.strUndoName = "替换";
            this.EndContentChangeLog();
            this.EndUpdate();

            //Add by wwj 2013-04-18 在替换方法结束后提醒，如果在myContent.ReplaceText方法中提醒会出现编辑器刷新的问题
            if (!string.IsNullOrEmpty(msg))
            {
                MessageBox.Show(msg, "警告");
            }

            return isOK;
        }
        /// <summary>
        /// 替换文档中所有的字符串
        /// </summary>
        /// <param name="strFind">要查找的字符串</param>
        /// <param name="strReplace">替换后的字符串</param>
        public void _ReplaceAll(string strFind, string strReplace)
        {
            //this.BeginUpdate();
            //this.BeginContentChangeLog();
            //myContent.ReplaceTextAll(strFind, strReplace);
            //myContentChangeLog.strUndoName = "替换所有";
            //this.EndContentChangeLog();
            //this.EndUpdate();
        }
        /// <summary>
        /// 选中所有的内容
        /// </summary>
        public void _SelectAll()
        {
            myContent.SelectAll();
        }
        /// <summary>
        /// 向下翻页
        /// </summary>
        public void _PageDown()
        {
            //myContent.AutoClearSelection = ! myOwnerControl.HasShiftPressing ;
            myContent.MoveStep(myOwnerControl.ClientSize.Height);
            UpdateCaret();
        }
        /// <summary>
        /// 向上翻页
        /// </summary>
        public void _PageUp()
        {
            //myContent.AutoClearSelection = ! myOwnerControl.HasShiftPressing ;
            myContent.MoveStep(0 - myOwnerControl.ClientSize.Height);
            UpdateCaret();
        }
        /// <summary>
        /// 移动到行首
        /// </summary>
        public void _MoveHome()
        {
            //myContent.AutoClearSelection = ! myOwnerControl.HasShiftPressing ;
            myContent.MoveHome();
            UpdateCaret();
        }
        /// <summary>
        /// 移动到行尾
        /// </summary>
        public void _MoveEnd()
        {
            //myContent.AutoClearSelection = ! myOwnerControl.HasShiftPressing ;
            myContent.MoveEnd();
            UpdateCaret();
        }
        /// <summary>
        /// 向上移动一行
        /// </summary>
        public void _MoveUp()
        {
            //myContent.AutoClearSelection = ! myOwnerControl.HasShiftPressing ;
            myContent.MoveUpOneLine();
            UpdateCaret();
        }
        /// <summary>
        /// 向下移动一行
        /// </summary>
        public void _MoveDown()
        {
            //myContent.AutoClearSelection = ! myOwnerControl.HasShiftPressing ;
            myContent.MoveDownOneLine();
            UpdateCaret();
        }
        /// <summary>
        /// 向左移动一个元素
        /// </summary>
        public void _MoveLeft()
        {
            //myContent.AutoClearSelection = ! myOwnerControl.HasShiftPressing ;
            myContent.MoveLeft();
            UpdateCaret();
        }
        /// <summary>
        /// 向右移动一个元素
        /// </summary>
        public void _MoveRight()
        {
            //myContent.AutoClearSelection = ! myOwnerControl.HasShiftPressing ;1
            myContent.MoveRight();
            UpdateCaret();
        }

        #region  撤销 重做 :
        ///// <summary>
        ///// 执行撤销操作
        ///// </summary>
        ///// <returns></returns>
        public bool _Undo()
        {

            if (intUndoState == 0 && UndoStack.UndoItemCount > 0)
            {
                intUndoState = 1;

                ZYEditorAction a = UndoStack.undostack.Pop();

                if (a.Undo())
                {
                    if (a.SelectStart >= 0)
                    {
                        myContent.SetSelection(a.SelectStart, a.SelectLength);
                    }
                    UndoStack.redostack.Push(a);
                    intUndoState = 0;
                    return true;
                }
                intUndoState = 0;
            }
            return false;
        }

        ///// <summary>
        ///// 执行重复操作
        ///// </summary>
        ///// <returns></returns>
        public bool _Redo()
        {
            if (intUndoState == 0 && UndoStack.RedoItemCount > 0)
            {
                intUndoState = 2;
                //UndoStack.Redo();

                ZYEditorAction a = (ZYEditorAction)UndoStack.redostack.Pop();
                //myRedoList[myRedoList.Count - 1];
                //myRedoList.RemoveAt(myRedoList.Count - 1);
                a.Redo();
                if (a.SelectStart2 >= 0)
                {
                    myContent.SetSelection(a.SelectStart2, a.SelectLength2);
                }
                //myUndoList.Add(a);
                UndoStack.undostack.Push(a);
                intUndoState = 0;
            }
            return true;
        }

        #endregion bwy :
        /// <summary>
        /// 获得当前插入点所在的段落元素
        /// </summary>
        /// <returns>获得的段落元素对象，若没有则返回空</returns>
        public ZYTextParagraph GetOwnerParagraph()
        {
            for (int iCount = myContent.SelectStart; iCount < myElements.Count; iCount++)
            {
                if ((myElements[iCount] as ZYTextElement).Parent is ZYTextParagraph)
                {
                    return ((myElements[iCount] as ZYTextElement).Parent as ZYTextParagraph);
                }
            }
            return null;
        }


        #region Insert 方法群
        /// <summary>
        /// 在文档最后添加锁定标记
        /// </summary>
        /// <returns>新增的锁定标记对象</returns>
        public ZYTextLock _InsertLock()
        {
            if (this.Locked) return null;
            this.BeginUpdate();
            this.BeginContentChangeLog();
            ZYTextLock Lock = new ZYTextLock();
            Lock.UserName = mySaveLogs.CurrentSaveLog.UserName;
            Lock.DateTime = ZYTime.GetServerTime();//DateTime.Now ;
            Lock.Level = mySaveLogs.CurrentSaveLog.Level;
            myContent.MoveSelectStart(myContent.Elements.Count - 1);
            //myContent.InsertElement( Lock );
            myContent.InsertLock(Lock);
            this.EndContentChangeLog();
            this.EndUpdate();
            return Lock;
        }
        /// <summary>
        /// 插入文本块
        /// </summary>
        /// <param name="strName">文本块名称</param>
        public void _InsertDiv(string strName)
        {
            if (this.Locked) return;
            this.BeginUpdate();
            //this.BeginContentChangeLog();
            if (myContent.HasSelected())
                myContent.DeleteSeleciton();
            else if (myOwnerControl.InsertMode == false)
                myContent.DeleteCurrentElement();
            ZYTextDiv myDiv = new ZYTextDiv();
            myDiv.Name = strName;
            //myDiv.Title = strName;
            //myDiv.HideTitle = false;
            myContent.AutoClearSelection = true;
            if ((myContent.PreElement is ZYTextParagraph) == false)
                myContent.InsertElement(new ZYTextParagraph());
            myContent.InsertElement(myDiv);
            //myContentChangeLog.strUndoName = "插入文本层";
            myContent.MoveSelectStart(myContent.SelectStart - 1);
            //this.EndContentChangeLog();
            this.EndUpdate();
        }
        /// <summary>
        /// 插入一个字符
        /// </summary>
        /// <param name="vChar">要插入的字符</param>
        /// <remarks>若要插入的字符为回车键则插入一个段落元素,否则插入一个字符元素</remarks>
        /// <returns>本操作新增的元素对象</returns>
        public ZYTextElement _InsertChar(char vChar)
        {
            ZYTextElement NewElement = null;
            this.BeginUpdate();
            this.BeginContentChangeLog();
            if (myContent.HasSelected())
            {
                if (!myContent.DeleteSeleciton())//Add by wwj 2013-02-01 删除失败后退出，防止操作失败后进行其他的操作
                {
                    this.EndContentChangeLog();
                    this.EndUpdate();
                    return null;
                }
            }
            else
            {
                if (myOwnerControl.InsertMode == false && (int)vChar != 13)
                    myContent.DeleteCurrentElement();
            }

            //记录改变前的行数 add by wwj 2012-06-06
            ZYTextContainer grandParent = this.Content.CurrentElement.Parent.Parent;
            int preLineCount = grandParent.Lines.Count;

            //如果为回车符
            if ((int)vChar == 13)//'\r'
            {

                if (WeiWenProcess.weiwen)
                {
                    WeiWenProcess.myDocument = this;
                    if (WeiWenProcess.LeftAndRightIsWeiWen())
                    {
                        WeiWenProcess.KeyEnterDown();
                    }
                }
                //************Add by wwj 2012-04-16 移除段落缩进***********************
                //string strBlank = myContent.GetCurrentLineHeadBlank();
                string strBlank = string.Empty;
                //********************************************************************

                if (myOwnerControl.HasShiftPressing())
                {
                    ZYTextLineEnd myL = new ZYTextLineEnd();
                    myL.OwnerDocument = this;
                    myContent.InsertElement(myL);
                    myContentChangeLog.strUndoName = "输入软回车";
                    NewElement = myL;
                }
                else
                {
                    ZYTextParagraph myP = new ZYTextParagraph();
                    myP.OwnerDocument = this;
                    ZYTextParagraph CurrentP = this.GetOwnerParagraph();
                    if (CurrentP != null)
                        myP.Align = CurrentP.Align;
                    myContent.InsertParagraph(myP);
                    myContentChangeLog.strUndoName = "输入硬回车";
                    NewElement = myP;

                }
                if (strBlank != null && strBlank.Length > 0)
                    myContent.InsertString(strBlank);
            }
            else
            {
                //add by Ukey zhang 2017-11-11 处理输入中，英，数字顺序
                if (WeiWenProcess.weiwen && (WeiWenProcess.isChenise(vChar) || StringCommon.IsABC123(vChar)))
                {
                    while (WeiWenProcess.isChenise(myContent.GetPreChar(1).Char) || StringCommon.IsABC123(myContent.GetPreChar(1).Char))
                    {
                        WeiWenProcess.myDocument = this;
                        WeiWenProcess.document._MoveLeft();

                    }
                }
                myContent.AutoClearSelection = true;
                NewElement = myContent.InsertChar(vChar);
                myContentChangeLog.strUndoName = "输入字符 " + vChar;

                //add by Ukey zhang 2017-11-11 处理输入中，英，数字顺序
                if (WeiWenProcess.weiwen && (WeiWenProcess.isChenise(vChar) || StringCommon.IsABC123(vChar)))
                {
                    while (WeiWenProcess.isChenise(myContent.GetFontChar(0).Char) || StringCommon.IsABC123(myContent.GetFontChar(0).Char))
                    {
                        WeiWenProcess.document._MoveRight();
                    }
                }
            }

            //add by wwj 2012-06-06 处理输入的字符，如果单元格内限定了不能换行，则对刚刚输入的字符进行处理
            grandParent.ProcessInsertChar(preLineCount);

            myContent.SelectLength = 0;
            this.EndContentChangeLog();
            this.EndUpdate();
            return NewElement;
        }

        public void _InsertImage(byte[] data)
        {
            MemoryStream ms = new System.IO.MemoryStream(data);
            Image img = System.Drawing.Image.FromStream(ms);
            _InsertImage(img);
        }

        /// <summary>
        /// 插入一个图片
        /// </summary>
        /// <param name="strSrc">图片来源</param>
        /// <param name="strType">图片类型</param>
        public void _InsertImage(string imgPath)
        {
            Image img = ZYTextConst.ImageFromURL(imgPath);
            _InsertImage(img);
        }

        public void _InsertImage(Image img)
        {
            ZYTextImage myImg = new ZYTextImage();
            myImg.Image = img;

            myImg.SaveInFile = true;
            if (myImg.Image != null)
            {
                int innerWidth = Pages.StandardWidth;
                myImg.Width = PixelToDocumentUnit(myImg.Image.Width);
                myImg.Height = PixelToDocumentUnit(myImg.Image.Height);

                if (myImg.Width > innerWidth)
                {
                    double d1 = innerWidth;
                    double d2 = myImg.Width;
                    double n = d1 / d2;//比例因数
                    myImg.Width = (int)(myImg.Width * n) - 2;
                    myImg.Height = (int)(myImg.Height * n);
                }
            }
            _InsertElement(myImg);
        }

        /// <summary>
        /// 插入一个元素
        /// </summary>
        /// <param name="NewElement">新的元素</param>
        public void _InsertElement(ZYTextElement NewElement)
        {
            if (this.Locked) return;
            this.BeginUpdate();
            this.BeginContentChangeLog();
            if (myContent.HasSelected())
                myContent.DeleteSeleciton();
            else if (myOwnerControl.InsertMode == false)
                myContent.DeleteCurrentElement();
            myContent.AutoClearSelection = true;
            myContent.InsertElement(NewElement); //--- insert
            myContentChangeLog.strUndoName = "插入元素";
            //myContent.MoveSelectStart(myContent.SelectStart + 1);
            this.EndContentChangeLog();
            this.EndUpdate();
        }
        /// <summary>
        /// 插入一个块元素
        /// </summary>
        /// <param name="NewElement"></param>
        public void _InsertBlock(ZYTextBlock NewElement)
        {

            _InsertElement(NewElement);
            int intMoveStep = (NewElement as ZYTextBlock).ChildCount;
            myContent.MoveSelectStart(myContent.SelectStart + intMoveStep - 1);
            //myContent.MoveSelectStart(myContent.SelectStart + intMoveStep - 1);//因为_InsertElement->InsertElement已经进行了加1的操作，所以这里要减1

            //因为Block中要显示的值是char，在进行插入char的方法中，已经进行 加1操作

        }
        /// <summary>
        /// 插入字符串,主要用来接受外来的字符串数据
        /// </summary>
        /// <param name="str"></param>
        public void _InserString(string str)
        {
            if (this.Locked) return;
            this.BeginUpdate();
            this.BeginContentChangeLog();

            //控制单元格中是否可以换行，可以换行则走原有的路线，否则需要一个个字符的录入 add by wwj 2012-06-07
            ZYTextElement myElement = (ZYTextElement)myElements[this.Content.SelectStart];
            TPTextCell cell = this.Content.GetParentByElement(myElement, ZYTextConst.c_Cell) as TPTextCell;
            if (cell != null && !cell.CanInsertEnter)
            {
                foreach (char c in str)
                {
                    this._InsertChar(c);
                }
            }
            else
            {

                this.Content.InsertString(str);
            }

            this.EndContentChangeLog();
            this.EndUpdate();
        }

        #endregion

        /// <summary>
        /// 删除元素,若存在选中元素则删除选中的元素,否则删除当前元素
        /// </summary>
        public void _Delete(params object[] flag)
        {
            if (this.Locked) return;

            //只有首尾都在范围内才继续执行
            if (this.OwnerControl.IsInActiveEditArea(this.Content.CurrentElement))
            {
                ZYTextElement endele = null;

                endele = this.Content.Elements[this.Content.SelectStart + this.Content.SelectLength] as ZYTextElement;
                //endele 在范围内，且不是最后一个元素
                if (this.OwnerControl.IsInActiveEditArea(endele) && endele != this.Content.GetPreElement(this.OwnerControl.ActiveEditArea.EndElement))
                {
                }
                else
                {
                    return;
                }
            }

            this.BeginUpdate();
            this.BeginContentChangeLog();
            if (myContent.HasSelected())
                myContent.DeleteSeleciton();
            else
            {
                int OldSelectStart = myContent.SelectStart;
                int intDeleteFlag = myContent.DeleteCurrentElement(flag);
                if (intDeleteFlag == 2 && myInfo.ShowAll)
                {
                    this.Content.SetSelection(OldSelectStart + 1, 0);
                }
            }
            myContentChangeLog.strUndoName = "删除元素";
            this.EndContentChangeLog();
            this.EndUpdate();
        }
        /// <summary>
        /// 处理退格键,删除插入点前的元素
        /// </summary>
        public void _BackSpace(params object[] flag)
        {
            if (this.Locked) return;

            //如果是在编辑部分区域，如果它的前一个字符不在范围内，退出
            if (this.OwnerControl.ActiveEditArea != null)
            {
                ZYTextElement preEle = this.Content.PreElement;
                ZYTextElement topEle = this.OwnerControl.ActiveEditArea.TopElement;
                //ZYTextElement endEle = this.OwnerControl.ActiveEditArea.EndElement;

                if (topEle.RealTop + topEle.Height <= preEle.RealTop && preEle.RealTop + preEle.Height <= this.OwnerControl.ActiveEditArea.End)
                {
                    //前一个元素也在范围内，继续操作
                }
                else
                {
                    //前一个元素超出范围，不操作
                    return;
                }
            }

            //判断是否可以删除选中元素,如果选中多个单元格的内容则不允许删除 Add By wwj 2012-04-24
            if (!this.Content.CanDeleteSelectElement())
            {
                return;
            }

            this.BeginUpdate();
            this.BeginContentChangeLog();
            if (myContent.HasSelected())
                myContent.DeleteSeleciton();
            else
            {
                int OldSelectStart = myContent.SelectStart;
                int intDeleteFlag = myContent.DeletePreElement(flag);
                if (intDeleteFlag == 2 && myInfo.ShowAll)
                {
                    myContent.SetSelection(OldSelectStart - 1, 0);
                }
            }
            myContentChangeLog.strUndoName = "退格删除";
            this.EndContentChangeLog();
            this.EndUpdate();
        }
        /// <summary>
        /// mfb是否是逻辑删除
        /// </summary>
        /// <returns></returns>
        public bool IsLogicDelete()
        {
            if (myInfo.AutoLogicDelete == false)
            {
                return false;
            }
            //如果当前用户的级别大于0,则是逻辑删除
            if (0 < mySaveLogs.CurrentSaveLog.Level)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// mfb确认进行删除操作
        /// </summary>
        /// <param name="myElement">将要删除的元素</param>
        /// <returns>0:确认删除元素 1:不删除该元素 2:对该元素进行逻辑删除</returns>
        public int isDeleteElement(ZYTextElement myElement)
        {
            bool ld = IsLogicDelete();
            if (ld && myElement.Parent.Locked == false)
            {
                if (ld)
                {
                    if (myElement.CreatorIndex == mySaveLogs.CurrentIndex)
                        return 0;
                }
                myElement.Deleteted = true;
                if (myOwnerControl != null)
                    myOwnerControl.AddInvalidateRect(myElement.Bounds);
                return 2;
            }
            return 0;
        }

        ///// <summary>
        ///// 修改对象大小
        ///// </summary>
        ///// <param name="myObj">要修改的对象</param>
        ///// <param name="NewWidth">新的宽度</param>
        ///// <param name="NewHeight">新的高度</param>
        ///// <returns>本操作是否成功,若没有进行修改操作则返回false</returns>
        //public bool _Resize(ZYTextObject myObj, int NewWidth, int NewHeight)
        //{
        //    if (myObj != null && myObj.CanResize && myObj.Width != NewWidth && myObj.Height != NewHeight)
        //    {
        //        myObj.Width = NewWidth;
        //        myObj.Height = NewHeight;
        //        this.RefreshLine();
        //        this.UpdateView();
        //        myContent.CurrentSelectElement = myObj;
        //        return true;
        //    }
        //    return false;
        //}

        /// <summary>
        /// 剪切文本
        /// </summary>
        public void _Cut()
        {
            if (this.Locked) return;
            this._Copy();
            this._Delete();
            //			string strText = myContent.GetSelectText();
            //			ClipboardHandler.SetTextToClipboard( strText);
            //			this.BeginContentChangeLog();
            //			myContent.DeleteSeleciton();
            //			myContentChangeLog.strUndoName = "剪切";
            //			this.EndContentChangeLog();
        }
        /// <summary>
        /// 复制文本
        /// </summary>
        public bool _Copy()
        {
            try
            {
                System.Collections.ArrayList myList = myContent.GetSelectElements();
                ZYTextElement.FixElementsForParent(myList);
                if (myList != null && myList.Count > 0)
                {
                    System.Windows.Forms.DataObject myDataObject = new System.Windows.Forms.DataObject();
                    string selText = ZYTextElement.GetElementsText(myList);
                    myDataObject.SetData(System.Windows.Forms.DataFormats.UnicodeText, selText);

                    //不允许把编辑器的文档格式粘贴到外部剪切板，如果这样就泄漏了机密
                    //myInfo.SaveMode = 0;
                    //System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
                    //myDoc.PreserveWhitespace = true;
                    //myDoc.AppendChild(myDoc.CreateElement(ZYTextConst.c_ClipboardDataFormat));

                    //if (ZYTextElement.ElementsToXML(GetRealElements(myList), myDoc.DocumentElement))
                    //{
                    //    string strXML = myDoc.DocumentElement.OuterXml;
                    //    myDataObject.SetData(ZYTextConst.c_ClipboardDataFormat, strXML);
                    //}

                    //放在内部剪切板中
                    //如果是设计模式，复制结构化数据
                    System.Windows.Forms.DataObject myInnerDataObject = new System.Windows.Forms.DataObject();
                    if (this.Info.DocumentModel == DocumentModel.Design)   //*********Modified by wwj 2012-07-24*********
                    {
                        System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
                        myDoc.PreserveWhitespace = true;
                        myDoc.AppendChild(myDoc.CreateElement(ZYTextConst.c_ClipboardDataFormat));

                        string strXML = string.Empty;
                        if (ZYTextElement.ElementsToXML(GetRealElements(myList), myDoc.DocumentElement))
                        {
                            strXML = myDoc.DocumentElement.OuterXml;
                            myInnerDataObject.SetData(ZYTextConst.c_ClipboardDataFormat, strXML);
                        }

                        //*********Modified by wwj 2012-07-24*********
                        if (this.Info.DocumentModel == DocumentModel.Design || strXML.IndexOf("<img") >= 0)
                        {
                            EmrClipboard.Data = myInnerDataObject;
                        }
                        else
                        {
                            EmrClipboard.Data = myDataObject;
                        }
                    }
                    //否则，复制文本数据

                    //*********Modified by wwj 2012-07-24*********
                    else
                    {
                        //要记录当前内容是哪个病人的
                        EmrClipboard.PatientID = this.Info.PatientID;
                        EmrClipboard.Data = myDataObject;
                    }

                    System.Windows.Forms.Clipboard.SetDataObject(myDataObject, true);
                    return true;
                }
            }
            catch (Exception ext)
            {
                //ZYErrorReport.Instance.SourceException = ext;
                //ZYErrorReport.Instance.SourceObject = this;
                //ZYErrorReport.Instance.UserMessage = "复制数据错误";
                //ZYErrorReport.Instance.ReportError();
            }
            return false;
        }

        /// <summary>
        /// 把基于单个字符的列表，转换为基于元素的列表 bwy 加
        /// </summary>
        /// <param name="elements">基于单个字符的列表</param>
        /// <returns>基于元素的列表</returns>
        public ArrayList GetRealElements(ArrayList elements)
        {
            ArrayList al = new ArrayList();
            foreach (ZYTextElement ele in elements)
            {
                if (ele.Parent is ZYTextBlock)
                {
                    if (al.Contains(ele.Parent))
                    {
                        continue;
                    }
                    else
                    {
                        al.Add(ele.Parent);
                    }
                }
                else
                {
                    al.Add(ele);
                }
            }
            return al;
        }
        /// <summary>
        /// 粘贴文本
        /// </summary>
        public void _Paste()
        {
            //TODO:需要在此先判断粘贴权限
            if (this.Locked) return;

            //Modified by wwj 2012-06-19 应老板要求此处删掉
            if (!CanPase)
            {
                MessageBox.Show("当前状态：" + this.Info.DocumentModel + "无数据，或非同一人数据不允许粘贴操作。");
                return;
            }

            //优先使用内部剪切板，如果为空，才使用系统剪切板来粘贴文本
            if (EmrClipboard.Data != null)
            {
                try
                {
                    System.Windows.Forms.IDataObject myData = EmrClipboard.Data as System.Windows.Forms.IDataObject;
                    //只有编辑状态才可以粘贴格式化数据
                    if (myData.GetDataPresent(ZYTextConst.c_ClipboardDataFormat))
                    {
                        System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
                        myDoc.PreserveWhitespace = true;
                        string strXML = (string)myData.GetData(ZYTextConst.c_ClipboardDataFormat);
                        myDoc.LoadXml(strXML);
                        System.Collections.ArrayList myList = new System.Collections.ArrayList();
                        this.LoadElementsToList(myDoc.DocumentElement, myList);
                        if (myList.Count > 0)
                        {
                            foreach (ZYTextElement myElement in myList)
                            {
                                if (myElement is ZYTextContainer)
                                    (myElement as ZYTextContainer).ClearSaveLog();
                                #region bwy
                                //myElement.RefreshSize();
                                //因为 ZYEOF的RefreshSize 出错了，所以把它注释掉了，也没发现有什么问题
                                #endregion bwy

                            }
                            this.BeginUpdate();
                            this.BeginContentChangeLog();
                            if (myContent.HasSelected())
                                myContent.DeleteSeleciton();
                            myContent.InsertRangeElements(myList);
                            this.EndContentChangeLog();
                            this.EndUpdate();
                        }
                    }
                    else
                    {
                        System.Windows.Forms.Clipboard.SetDataObject(EmrClipboard.Data, true);
                        string strText = ClipboardHandler.GetTextFromClipboard();
                        if (strText != null)
                        {
                            //控制单元格中是否可以换行，可以换行则走原有的路线，否则需要一个个字符的录入 add by wwj 2012-06-07
                            ZYTextElement myElement = (ZYTextElement)myElements[this.Content.SelectStart];
                            TPTextCell cell = this.Content.GetParentByElement(myElement, ZYTextConst.c_Cell) as TPTextCell;
                            if (cell != null && !cell.CanInsertEnter)
                            {
                                foreach (char c in strText)
                                {
                                    this._InsertChar(c);
                                }
                            }
                            else
                            {
                                this.BeginUpdate();
                                this.BeginContentChangeLog();
                                myContent.ReplaceSelection(strText);
                                this.EndContentChangeLog();
                                this.EndUpdate();
                            }
                        }
                    }

                }//try
                catch (Exception ext)
                {
                    //ZYErrorReport.Instance.DebugPrint("试图粘贴数据错误:" + ext.ToString());
                    MessageBox.Show(ext.Message);
                }
            }
            //使用系统剪切板粘贴文本数据
            else
            {
                if (this.Info.DocumentModel == DocumentModel.Design || (DrectSoft.Service.DS_SqlService.GetConfigValueByKey("CopyPasteSetinout") != null && DrectSoft.Service.DS_SqlService.GetConfigValueByKey("CopyPasteSetinout").Trim() == "1"))
                {
                    string strText = ClipboardHandler.GetTextFromClipboard();
                    if (strText != null)
                    {
                        this.BeginUpdate();
                        this.BeginContentChangeLog();
                        myContent.ReplaceSelection(strText);
                        this.EndContentChangeLog();
                        this.EndUpdate();
                    }
                }
            }
        }

        //打印旧的实现方式，现在请改用_Print,有参考价值，不要删除源码
        public bool _PrintOld()
        {
            using (XDesignerPrinting.XPrintDocument doc = new XDesignerPrinting.XPrintDocument())
            {
                doc.Document = this;

                doc.EnableJumpPrint = this.EnableJumpPrint;
                doc.JumpPrintPosition = this.JumpPrintPosition;

                bool bUnderline = this.Info.FieldUnderLine;

                this.Pages.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                using (System.Windows.Forms.PrintDialog dlg = new System.Windows.Forms.PrintDialog())
                {

                    dlg.PrinterSettings = this.Pages.PrinterSettings;


                    dlg.PrinterSettings.MinimumPage = 1;
                    dlg.PrinterSettings.MaximumPage = this.Pages.Count;
                    dlg.PrinterSettings.FromPage = 1;
                    dlg.PrinterSettings.ToPage = this.Pages.Count;
                    dlg.PrinterSettings.Duplex = Duplex.Default;
                    bool b = dlg.PrinterSettings.CanDuplex;

                    dlg.AllowSomePages = true;
                    dlg.AllowPrintToFile = true;
                    dlg.AllowCurrentPage = true;
                    dlg.AllowSelection = true;
                    dlg.ShowNetwork = true;
                    #region bwy:
                    dlg.UseEXDialog = true;

                    #endregion bwy:

                    //如果续打，不显示其它按钮
                    if (this.EnableJumpPrint || this.Content.SelectLength == 0)
                    {
                        dlg.AllowSelection = false;
                    }


                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {

                        try
                        {
                            this.Info.FieldUnderLine = false;
                            //如果是打印所有页
                            if (dlg.PrinterSettings.PrintRange == System.Drawing.Printing.PrintRange.AllPages)
                            {
                                this.Info.Printing = true;
                                doc.Print();
                                this.Info.Printing = false;

                            }
                            //打印页范围
                            else if (dlg.PrinterSettings.PrintRange == System.Drawing.Printing.PrintRange.SomePages)
                            {
                                if (dlg.PrinterSettings.FromPage >= 1 && dlg.PrinterSettings.ToPage <= this.Pages.Count)
                                {
                                    this.Info.Printing = true;
                                    for (int i = dlg.PrinterSettings.FromPage; i <= dlg.PrinterSettings.ToPage; i++)
                                    {
                                        doc.PrintSpecialPage(i - 1);
                                    }

                                    this.Info.Printing = false;
                                }
                                else
                                {
                                    System.Windows.Forms.MessageBox.Show("页码范围不正确！");
                                }
                            }
                            //打印当前页
                            else if (dlg.PrinterSettings.PrintRange == System.Drawing.Printing.PrintRange.CurrentPage)
                            {
                                //Debug.WriteLine("打印当前页");
                                this.Info.Printing = true;
                                doc.PrintSpecialPage(this.PageIndex);
                                this.Info.Printing = false;
                            }
                            //打印选择的内容
                            else if (dlg.PrinterSettings.PrintRange == System.Drawing.Printing.PrintRange.Selection)
                            {
                                //Debug.WriteLine("打印选择的内容");
                                //设置打印页面范围
                                //要计算选择范围内的元素所在的页

                                if (this.Content.SelectLength == 0)
                                {
                                    MessageBox.Show("没有选中任何内容。");
                                    return false;
                                }
                                int selstart = this.Content.SelectStart;
                                int selend = 0;
                                int sellength = this.Content.SelectLength;
                                if (sellength < 0)
                                {
                                    selend = selstart;
                                    selstart = selstart + sellength;
                                }
                                else
                                {
                                    selend = selstart + sellength - 1;
                                }


                                int pagestart = -1;
                                int pageend = -1;


                                Rectangle rectcharstart = (this.Elements[selstart] as ZYTextElement).Bounds;
                                Rectangle rectcharend = (this.Elements[selend] as ZYTextElement).Bounds;

                                Point sp = this.OwnerControl.ViewPointToClient(rectcharstart.Location);
                                Point ep = this.OwnerControl.ViewPointToClient(rectcharend.Location);
                                int pagecount = this.Pages.Count;
                                foreach (PrintPage p in this.Pages)
                                {
                                    //Debug.WriteLine("PrintPage Bounds " + p.Bounds);

                                    //Rectangle rect1 = p.Bounds;
                                    Rectangle rectp = p.ClientBounds;
                                    int left = p.Left;
                                    int top = p.Top;

                                    if (p.Bounds.Contains(rectcharstart))
                                    {
                                        pagestart = p.Index;
                                    }

                                    if (p.Bounds.Contains(rectcharend))
                                    {
                                        pageend = p.Index;
                                    }
                                }

                                this.Info.Printing = true;
                                this.EnableSelectionPrint = true;
                                for (int i = pagestart; i <= pageend; i++)
                                {
                                    doc.PrintSpecialPage(i);
                                }
                                this.EnableSelectionPrint = false;
                                this.Info.Printing = false;

                            }
                        }

                        catch (Exception ext)
                        {
                            //throw ext;
                        }
                        finally
                        {
                            this.Info.FieldUnderLine = bUnderline;
                            this.Info.Printing = false;
                        }
                    }
                }
            }

            ////恢复全部内容
            //if (this.EnableSelectionPrint)
            //{
            //    if (!innerSelectionPrintEnd()) return true;
            //}

            return true;
        }

        CustomPrintDialog.CustomPrintDialog PrintDlgProxy = null;
        //打印
        public bool _Print()
        {
            using (XDesignerPrinting.XPrintDocument doc = new XDesignerPrinting.XPrintDocument())
            {
                doc.Document = this;
                doc.EnableJumpPrint = this.EnableJumpPrint;
                doc.JumpPrintPosition = this.JumpPrintPosition;
                bool bUnderline = this.Info.FieldUnderLine;
                this.Pages.PrinterSettings = new System.Drawing.Printing.PrinterSettings();

                doc.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                doc.PrinterSettings = this.Pages.PrinterSettings;
                doc.PrinterSettings.MinimumPage = 1;
                doc.PrinterSettings.MaximumPage = this.Pages.Count;
                doc.PrinterSettings.FromPage = 1;
                doc.PrinterSettings.ToPage = this.Pages.Count;


                using (CustomPrintDialog.CustomPrintDialog dlg = new CustomPrintDialog.CustomPrintDialog())
                {
                    PrintDlgProxy = dlg;

                    dlg.ShrinkOutputToFitOnOnePage = true;
                    dlg.AllowSelection = true;
                    dlg.AllowSomePages = true;

                    dlg.pdlg.nFromPage = 1;
                    dlg.pdlg.nToPage = (ushort)this.Pages.Count;
                    dlg.pdlg.nMinPage = 1;
                    dlg.pdlg.nMaxPage = (ushort)this.Pages.Count;

                    dlg.m_Panel.checkBoxHeader.CheckedChanged += new EventHandler(CheckedChanged);
                    dlg.m_Panel.checkBoxFooter.CheckedChanged += new EventHandler(CheckedChanged);

                    //doc.PrinterSettings = this.Pages.PrinterSettings;
                    //doc.PrinterSettings.MinimumPage = 1;
                    //doc.PrinterSettings.MaximumPage = this.Pages.Count;
                    //doc.PrinterSettings.FromPage = 1;
                    //doc.PrinterSettings.ToPage = this.Pages.Count;
                    //dlg.Document = doc;
                    //dlg.AllowCurrentPage = true;

                    //如果续打，不显示其它按钮
                    if (this.EnableJumpPrint || this.Content.SelectLength == 0)
                    {
                        dlg.AllowSelection = false;
                    }

                    const Int32 PD_ALLPAGES = 0x00000000;
                    const Int32 PD_SELECTION = 0x00000001;
                    const Int32 PD_PAGENUMS = 0x00000002;
                    const Int32 PD_NOSELECTION = 0x00000004;
                    const Int32 PD_NOPAGENUMS = 0x00000008;



                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        //打印份数
                        for (int i = 1; i <= dlg.pdlg.nCopies; i++)
                        {
                            Debug.WriteLine("打印份数 " + dlg.pdlg.nCopies);
                            try
                            {
                                this.Info.FieldUnderLine = false;

                                //打印页范围
                                //if (doc.PrinterSettings.PrintRange == System.Drawing.Printing.PrintRange.SomePages)
                                if ((dlg.pdlg.Flags & PD_PAGENUMS) == PD_PAGENUMS)
                                {
                                    Debug.WriteLine("页码范围");
                                    if (dlg.pdlg.nFromPage >= 1 && dlg.pdlg.nToPage <= this.Pages.Count)
                                    {
                                        this.Info.Printing = true;
                                        PrintEx(dlg.pdlg.nFromPage - 1, dlg.pdlg.nToPage - 1, dlg.m_Panel.comboBox1.SelectedIndex, doc);
                                        this.Info.Printing = false;
                                    }
                                    else
                                    {
                                        System.Windows.Forms.MessageBox.Show("页码范围不正确！");
                                    }
                                }
                                ////打印当前页
                                //else if (doc.PrinterSettings.PrintRange == System.Drawing.Printing.PrintRange.CurrentPage)
                                //{
                                //    Debug.WriteLine("打印当前页");
                                //    this.Info.Printing = true;
                                //    doc.PrintSpecialPage(this.PageIndex);
                                //    this.Info.Printing = false;
                                //}
                                //打印选择的内容
                                //else if (doc.PrinterSettings.PrintRange == System.Drawing.Printing.PrintRange.Selection)
                                else if ((dlg.pdlg.Flags & PD_SELECTION) == PD_SELECTION)
                                {
                                    Debug.WriteLine("打印选择的内容");
                                    //设置打印页面范围
                                    //要计算选择范围内的元素所在的页

                                    if (this.Content.SelectLength == 0)
                                    {
                                        MessageBox.Show("没有选中任何内容。");
                                        return false;
                                    }
                                    int selstart = this.Content.SelectStart;
                                    int selend = 0;
                                    int sellength = this.Content.SelectLength;
                                    if (sellength < 0)
                                    {
                                        selend = selstart;
                                        selstart = selstart + sellength;
                                    }
                                    else
                                    {
                                        selend = selstart + sellength - 1;
                                    }


                                    int pagestart = -1;
                                    int pageend = -1;


                                    Rectangle rectcharstart = (this.Elements[selstart] as ZYTextElement).Bounds;
                                    Rectangle rectcharend = (this.Elements[selend] as ZYTextElement).Bounds;

                                    Point sp = this.OwnerControl.ViewPointToClient(rectcharstart.Location);
                                    Point ep = this.OwnerControl.ViewPointToClient(rectcharend.Location);
                                    int pagecount = this.Pages.Count;
                                    foreach (PrintPage p in this.Pages)
                                    {
                                        Debug.WriteLine("PrintPage Bounds " + p.Bounds);

                                        //Rectangle rect1 = p.Bounds;
                                        Rectangle rectp = p.ClientBounds;
                                        int left = p.Left;
                                        int top = p.Top;

                                        if (p.Bounds.Contains(rectcharstart))
                                        {
                                            pagestart = p.Index;
                                        }

                                        if (p.Bounds.Contains(rectcharend))
                                        {
                                            pageend = p.Index;
                                        }
                                    }

                                    this.Info.Printing = true;
                                    this.EnableSelectionPrint = true;

                                    PrintEx(pagestart, pageend, dlg.m_Panel.comboBox1.SelectedIndex, doc);

                                    this.EnableSelectionPrint = false;
                                    this.Info.Printing = false;

                                }
                                //如果是打印所有页
                                else //(doc.PrinterSettings.PrintRange == System.Drawing.Printing.PrintRange.AllPages)
                                {
                                    this.Info.Printing = true;
                                    PrintEx(0, this.Pages.Count - 1, dlg.m_Panel.comboBox1.SelectedIndex, doc);
                                    this.Info.Printing = false;
                                }
                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                this.Info.FieldUnderLine = bUnderline;
                                this.Info.Printing = false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        #region Add By wwj 2011-08-25 for 打印
        /// <summary>
        /// Add By wwj 2011-08-25 todo 全部打印 
        /// </summary>
        /// <param name="printerName">打印机</param>
        /// <returns>打印是否成功</returns>
        public bool _PrintAll(string printerName)
        {
            using (XDesignerPrinting.XPrintDocument doc = new XDesignerPrinting.XPrintDocument())
            {
                #region 初始化XPrintDocument
                doc.Document = this;
                doc.EnableJumpPrint = false;
                doc.JumpPrintPosition = 0;
                this.Pages.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                doc.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                doc.PrinterSettings = this.Pages.PrinterSettings;
                doc.PrinterSettings.PrinterName = printerName;
                doc.PrinterSettings.MinimumPage = 1;
                doc.PrinterSettings.MaximumPage = this.Pages.Count;
                doc.PrinterSettings.FromPage = 1;
                doc.PrinterSettings.ToPage = this.Pages.Count;
                #endregion

                try
                {
                    this.Info.FieldUnderLine = false;

                    //打印页范围
                    this.Info.Printing = true;
                    PrintEx(0, this.Pages.Count - 1, 0, doc);
                    this.Info.Printing = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Info.Printing = false;
                }
            }
            return true;
        }

        /// <summary>
        /// 是否正在生成图片，导出图片前为true，导出完成后为false
        /// </summary>
        public bool IsPrintImage = false;

        /// <summary>
        /// 生成打印的图片  edit by tj 2012-11-06
        /// </summary>
        public List<Bitmap> GeneratePrintImage()
        {
            IsPrintImage = true;
            List<Bitmap> imagePages = new List<Bitmap>();
            imagePages.Clear();
            DocumentPageDrawer drawer = new DocumentPageDrawer(this, this.Pages);
            drawer.DrawFooter = true;
            drawer.DrawHead = true;
            foreach (PrintPage page in this.Pages)
            {
                imagePages.Add(drawer.GetPageBmp(page, true));
            }
            IsPrintImage = false;
            return imagePages;
        }
        /// <summary>
        /// 生成打印的图片  edit by ywk 二次修改
        /// </summary>
        public List<Metafile> GeneratePrintImage2()
        {
            IsPrintImage = true;
            List<Metafile> imagePages = new List<Metafile>();
            imagePages.Clear();
            DocumentPageDrawer drawer = new DocumentPageDrawer(this, this.Pages);
            drawer.DrawFooter = true;
            drawer.DrawHead = true;
            foreach (PrintPage page in this.Pages)
            {
                imagePages.Add(drawer.GetPageBmp2(page, true));
            }
            IsPrintImage = false;
            return imagePages;
        }

        /// <summary>
        /// Add By wwj 2011-08-25 todo 打印指定页
        /// </summary>
        /// <param name="printerName">打印机</param>
        /// <param name="pageBegin"></param>
        /// <param name="pageEnd"></param>
        /// <returns></returns>
        public bool _PrintPageFromTo(string printerName, int pageBegin, int pageEnd)
        {
            using (XDesignerPrinting.XPrintDocument doc = new XDesignerPrinting.XPrintDocument())
            {
                #region 初始化XPrintDocument
                doc.Document = this;
                doc.EnableJumpPrint = false;
                doc.JumpPrintPosition = 0;
                this.Pages.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                doc.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                doc.PrinterSettings = this.Pages.PrinterSettings;
                doc.PrinterSettings.PrinterName = printerName;
                doc.PrinterSettings.MinimumPage = 1;
                doc.PrinterSettings.MaximumPage = this.Pages.Count;
                doc.PrinterSettings.FromPage = 1;
                doc.PrinterSettings.ToPage = this.Pages.Count;
                #endregion

                if (pageBegin > this.Pages.Count)
                {
                    pageBegin = this.Pages.Count;
                }
                if (pageEnd > this.Pages.Count)
                {
                    pageEnd = this.Pages.Count;
                }
                if (pageBegin > pageEnd)
                {
                    int temp = pageBegin;
                    pageBegin = pageEnd;
                    pageEnd = temp;
                }

                try
                {
                    this.Info.FieldUnderLine = false;

                    //打印页范围
                    this.Info.Printing = true;
                    PrintEx(pageBegin - 1, pageEnd - 1, 0, doc);
                    this.Info.Printing = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Info.Printing = false;
                }
            }
            return true;
        }

        /// <summary>
        /// Add By wwj 2011-08-25 todo 打印选中的文字
        /// </summary>
        /// <param name="printerName">打印机</param>
        /// <returns></returns>
        public bool _PrintSelection(string printerName)
        {
            using (XDesignerPrinting.XPrintDocument doc = new XDesignerPrinting.XPrintDocument())
            {
                #region 初始化XPrintDocument
                doc.Document = this;
                doc.EnableJumpPrint = false;
                doc.JumpPrintPosition = 0;
                this.Pages.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                doc.PrinterSettings = new System.Drawing.Printing.PrinterSettings();
                doc.PrinterSettings = this.Pages.PrinterSettings;
                doc.PrinterSettings.PrinterName = printerName;
                doc.PrinterSettings.MinimumPage = 1;
                doc.PrinterSettings.MaximumPage = this.Pages.Count;
                doc.PrinterSettings.FromPage = 1;
                doc.PrinterSettings.ToPage = this.Pages.Count;
                #endregion

                try
                {
                    this.Info.FieldUnderLine = false;
                    //设置打印页面范围
                    //要计算选择范围内的元素所在的页

                    if (this.Content.SelectLength == 0)
                    {
                        //MessageBox.Show("没有选中任何内容。");
                        return false;
                    }

                    int selstart = this.Content.SelectStart;
                    int selend = 0;
                    int sellength = this.Content.SelectLength;
                    if (sellength < 0)
                    {
                        selend = selstart;
                        selstart = selstart + sellength;
                    }
                    else
                    {
                        selend = selstart + sellength - 1;
                    }

                    int pagestart = -1;
                    int pageend = -1;

                    Rectangle rectcharstart = (this.Elements[selstart] as ZYTextElement).Bounds;
                    Rectangle rectcharend = (this.Elements[selend] as ZYTextElement).Bounds;

                    Point sp = this.OwnerControl.ViewPointToClient(rectcharstart.Location);
                    Point ep = this.OwnerControl.ViewPointToClient(rectcharend.Location);
                    int pagecount = this.Pages.Count;
                    foreach (PrintPage p in this.Pages)
                    {
                        Debug.WriteLine("PrintPage Bounds " + p.Bounds);

                        Rectangle rectp = p.ClientBounds;
                        int left = p.Left;
                        int top = p.Top;

                        if (p.Bounds.Contains(rectcharstart))
                        {
                            pagestart = p.Index;
                        }

                        if (p.Bounds.Contains(rectcharend))
                        {
                            pageend = p.Index;
                        }
                    }

                    this.Info.Printing = true;
                    this.EnableSelectionPrint = true;

                    //选择区域打印的时候不打印页眉和页脚
                    //this.PrintHeader = false;
                    //this.PrintFooter = false;

                    PrintEx(pagestart, pageend, 0, doc);

                    //this.PrintHeader = true;
                    //this.PrintFooter = true;

                    this.EnableSelectionPrint = false;
                    this.Info.Printing = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    this.Info.Printing = false;
                }
            }
            return true;
        }

        /// <summary>
        /// Add By wwj 2011-08-25 todo 续打
        /// </summary>
        /// <param name="printerName">打印机</param>
        /// <returns></returns>
        public bool _PrintJump(string printerName)
        {
            XPrintDocument doc = new XPrintDocument();
            doc.Document = this;
            doc.EnableJumpPrint = this.EnableJumpPrint;
            doc.JumpPrintPosition = this.JumpPrintPosition;
            this.Info.Printing = true;
            doc.Print();
            this.Info.Printing = false;
            return true;
        }

        /// <summary>
        /// Add By wwj 2012-04-17 打印选中区域
        /// </summary>
        /// <param name="printerName">打印机</param>
        /// <returns></returns>
        public bool _PrintSelectArea(string printerName)
        {
            XPrintDocument doc = new XPrintDocument();
            doc.Document = this;

            doc.EnableSelectAreaPrint = this.EnableSelectAreaPrint;
            doc.SelectAreaPrintLeftTopPoint = this.SelectAreaPrintLeftTopPoint;
            doc.SelectAreaPrintRightBottomPoint = this.SelectAreaPrintRightBottomPoint;

            this.Info.Printing = true;
            doc.Print();
            this.Info.Printing = false;

            return true;
        }
        #endregion

        public bool PrintHeader = true;
        public bool PrintFooter = true;
        void CheckedChanged(object sender, EventArgs e)
        {
            if (PrintDlgProxy != null)
            {
                this.PrintHeader = PrintDlgProxy.m_Panel.checkBoxHeader.Checked;
                this.PrintFooter = PrintDlgProxy.m_Panel.checkBoxFooter.Checked;
            }
        }

        /// <summary>
        /// 打印一定页范围内的文档，索引从0开始
        /// </summary>
        /// <param name="start">开始页</param>
        /// <param name="end">结束页</param>
        /// <param name="flag">0全部，1奇数页，2偶页</param>
        void PrintEx(int start, int end, int flag, XPrintDocument doc)
        {
            for (int i = start; i <= end; i++)
            {
                if (flag == 0)
                {
                    doc.PrintSpecialPage(i);
                }
                else if (i % 2 == 0 && flag == 1) //奇数页，因为索引从0开始
                {
                    doc.PrintSpecialPage(i);
                }

                else if (i % 2 == 1 && flag == 2)//偶数页，因为索引从0开始
                {
                    doc.PrintSpecialPage(i);
                }
                else if (i % 2 == 1 && flag == 3)//偶数页，双面打印，左右边距互换
                {
                    XPageSettings setOld = this.Pages.PageSettings;
                    XPageSettings set = this.Pages.PageSettings;

                    int tmp = set.Margins.Left;
                    set.Margins.Left = set.Margins.Right;
                    set.Margins.Right = tmp;

                    this.Pages.PageSettings = set;
                    //this.Pages[i].PageSettings = set;
                    //this.RefreshLine();
                    //this.RefreshPages();
                    //this.OwnerControl.UpdatePages();
                    //this.Refresh();


                    doc.PrintSpecialPage(i);
                    this.Pages.PageSettings = setOld;
                }

            }
        }


        //打印预览
        System.Windows.Forms.DataObject myDataObject = new System.Windows.Forms.DataObject();
        public bool _PrintPreview()
        {

            PrintPreviewDialog printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            XPrintDocument doc = new XPrintDocument();
            doc.Document = this;

            //if (this.EnableSelectionPrint)
            //{
            //    if (!innerSelectionPrintStart()) return false;
            //}
            //else
            //{
            //    this.myContent.SetSelection(0, 0);
            //}

            doc.EnableJumpPrint = this.EnableJumpPrint;
            doc.JumpPrintPosition = this.JumpPrintPosition;

            printPreviewDialog1.Document = doc;
            this.Info.Printing = true;
            printPreviewDialog1.ShowDialog();
            this.Info.Printing = false;


            return true;
        }

        /// <summary>
        /// 页面设置 ，2018-10-14 修改，方便修改编辑器页面设置
        /// </summary>
        /// <param name="refPageSetting">页面属性</param>
        /// <returns></returns>
        public bool _PageSetting(ref XPageSettings refPageSetting)
        {
            using (XDesignerPrinting.dlgPageSetup dlg = new XDesignerPrinting.dlgPageSetup())
            {
                dlg.PageSettings = this.Pages.PageSettings;
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.Pages.PageSettings = dlg.PageSettings;
                    this.RefreshLine();
                    this.RefreshPages();
                    this.OwnerControl.UpdatePages();
                    this.Refresh();
                    return true;
                }
                else if (result == DialogResult.Yes)
                {
                    refPageSetting = dlg.PageSettings;
                    return false;
                }
            }
            return false;
        }

        #region 设置文本属性

        /// <summary>
        /// 设置选中文本的字体名称
        /// </summary>
        /// <param name="NewFontName">要设置的字体名称</param>
        public void SetSelectioinFontName(string NewFontName)
        {
            SetSelectionAttribute(0, NewFontName);
        }
        /// <summary>
        /// 设置选中文本的字体大小
        /// </summary>
        /// <param name="NewSize">字体大小</param>
        public void SetSelectionFontSize(float NewSize)
        {
            SetSelectionAttribute(1, NewSize.ToString());
        }
        /// <summary>
        /// 设置选中文本的字体粗体
        /// </summary>
        /// <param name="NewBold">是否为粗体</param>
        public void SetSelectionFontBold(bool NewBold)
        {
            SetSelectionAttribute(2, (NewBold ? "1" : "0"));
        }
        /// <summary>
        /// 设置选中文本的字体斜体属性
        /// </summary>
        /// <param name="NewItalic">是否为斜体</param>
        public void SetSelectionFontItalic(bool NewItalic)
        {
            SetSelectionAttribute(3, (NewItalic ? "1" : "0"));
        }
        /// <summary>
        /// 设置选中文本的下划线属性
        /// </summary>
        /// <param name="UnderLine">是否有下划线</param>
        public void SetSelectionUnderLine(bool UnderLine)
        {
            SetSelectionAttribute(4, (UnderLine ? "1" : "0"));
        }
        /// <summary>
        /// 设置选中文本的文本颜色
        /// </summary>
        /// <param name="NewColor">文本颜色</param>
        public void SetSelectionColor(System.Drawing.Color NewColor)
        {
            SetSelectionAttribute(5, NewColor.ToArgb().ToString());
        }

        /// <summary>
        /// 设置选中文本的文本颜色
        /// </summary>
        /// <param name="NewColor">文本颜色</param>
        public void SetSelectionBackGroundColor(System.Drawing.Color NewColor)
        {
            SetSelectionAttribute(9, NewColor.ToArgb().ToString());
        }

        /// <summary>
        /// 设置选中文本的下标属性,暂未实现
        /// </summary>
        /// <param name="NewSub">是否是下标</param>
        public void SetSelectionSub(bool NewSub)
        {
            ResetSupsub();
            SetSelectionAttribute(7, (NewSub ? "1" : "0"));
        }
        /// <summary>
        /// 设置选中文本的上标属性,暂未实现
        /// </summary>
        /// <param name="NewSup">是否是上标</param>
        public void SetSelectionSup(bool NewSup)
        {
            ResetSupsub();
            SetSelectionAttribute(8, (NewSup ? "1" : "0"));
        }
        /// <summary>
        /// 不能同时为上标或下标，所以在设置上标或下标时，应该先清空
        /// </summary>
        void ResetSupsub()
        {
            SetSelectionAttribute(7, "0");
            SetSelectionAttribute(8, "0");
        }
        /// <summary>
        /// 设置选中的文本元素的属性
        /// </summary>
        /// <param name="index">属性的内部序号</param>
        /// <param name="strValue">属性值</param>
        private void SetSelectionAttribute(int index, string strValue)
        {
            bool bolChange = false;
            if (this.Locked) return;
            System.Collections.ArrayList myList = myContent.GetSelectElements();

            ZYTextChar myChar = null;
            ZYTextContainer myParent = null;
            //ZYTextFlag myflag = null;
            this.BeginContentChangeLog();

            //如果是一个空行，且是设置字体大小，那么，设置回车符的高度为字体高
            if (myList.Count == 0 && index == 1 && this.Content.CurrentElement is ZYTextEOF && this.Content.CurrentElement == this.Content.CurrentElement.Parent.FirstElement && this.Content.CurrentElement == this.Content.CurrentElement.Parent.LastElement)
            {
                (this.Content.CurrentElement as ZYTextEOF).FontSize = float.Parse(strValue);
                this.Content.CurrentElement.RefreshSize();
                bolChange = true;
            }

            for (int iCount = 0; iCount < myList.Count; iCount++)
            {

                myChar = myList[iCount] as ZYTextChar;
                if (myChar != null)
                {

                    if (myParent != null && myParent != myChar.Parent)
                        myParent.RefreshLine();
                    myParent = myChar.Parent;
                    switch (index)
                    {
                        case 0: // 字体名称
                            myChar.FontName = strValue;
                            break;
                        case 1: // 字体大小
                            myChar.FontSize = float.Parse(strValue);
                            break;
                        case 2: // 粗体
                            myChar.FontBold = strValue.Equals("1");
                            break;
                        case 3: // 斜体
                            myChar.FontItalic = strValue.Equals("1");
                            break;
                        case 4: // 下划线
                            myChar.FontUnderLine = strValue.Equals("1");
                            break;
                        case 5: // 文本颜色
                            myChar.ForeColor = System.Drawing.Color.FromArgb(Convert.ToInt32(strValue));
                            break;
                        case 6: // 元素的数据名称
                            //myChar.Name = strValue ;
                            break;
                        case 7: // 下标
                            myChar.Sub = strValue.Equals("1");
                            break;
                        case 8: // 上标
                            myChar.Sup = strValue.Equals("1");
                            break;
                        case 9: // 文本背景颜色
                            myChar.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(strValue));
                            break;
                        case 10: // 圈字
                            myChar.CircleFont = strValue.Equals("1");
                            break;
                        default:
                            return;
                    }
                    myChar.RefreshSize();
                    bolChange = true;

                    #region bwy :
                    //如果是ZYBlock中的字符，同时设置ZYBlock的属性
                    if (myChar.Parent is ZYTextBlock)
                    {
                        ZYTextBlock parent = (ZYTextBlock)myChar.Parent;
                        myChar.Attributes.CopyTo(parent.Attributes);
                        parent.UpdateAttrubute();
                    }

                    //if (myChar is ZYElement)
                    //{
                    //    ((ZYElement)myChar).Attributes.SetValue(ZYTextConst.c_FontName,strValue);

                    //}
                    #endregion bwy :
                }
                //如果不是ZYTextChar，可能是ZYTextBlock,ZYElement

            }
            myContentChangeLog.strUndoName = "设置属性" + strValue;

            if (bolChange)
            {
                this.RefreshLine();
                //this.UpdateView();//原来有，现在注释了,因为设置字体本光标位置跑到最左上角了
                this.Refresh();
                //myContent.Modified  = true;
            }


            //this.ContentChanged();
            this.EndContentChangeLog();


            this.UpdateTextCaret();

        }

        /// <summary>
        /// 设置圈字 Add By wwj 2012-05-31
        /// </summary>
        public void SetCircleFont(bool NewCircle)
        {
            SetSelectionAttribute(10, (NewCircle ? "1" : "0"));
        }
        #endregion

        /// <summary>
        /// 设置当前插入点所在的段落的对齐方式
        /// </summary>
        /// <param name="intAlign">对齐样式</param>
        /// <returns>本操作是否导致文档内容的修改</returns>
        public bool SetAlign(ParagraphAlignConst intAlign)
        {
            //if (this.Locked) return false;
            bool bolChange = false;
            System.Collections.ArrayList myList = myContent.GetSelectParagraph();
            this.BeginContentChangeLog();
            #region bwy :
            if (myList.Count == 0)
            {
                ZYTextParagraph p = this.Content.CurrentElement.Parent as ZYTextParagraph;
                if (p == null)
                {
                    p = this.Content.CurrentElement.Parent.Parent as ZYTextParagraph;
                }
                myList.Add(p);
            }
            #endregion bwy :
            foreach (ZYTextParagraph myP in myList)
            {
                if (myP.Align != intAlign)
                {
                    myP.Align = intAlign;
                    bolChange = true;
                }
            }

            //myContentChangeLog.strUndoName = "设置对齐方式";
            this.EndContentChangeLog();
            if (bolChange)
            {
                this.RefreshLine();
                this.UpdateView();
                this.Refresh();
                myContent.Modified = true;
            }
            return bolChange;
        }




        /// <summary>
        /// 根据一段XML字符串在文档的插入点插入新的元素
        /// </summary>
        /// <param name="strXML">XML字符串</param>
        /// <returns>操作是否成功</returns>
        public bool InsertByXML(string strXML)
        {
            //if (this.Locked) return false;
            System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
            myDoc.LoadXml(strXML);
            ZYTextElement e = this.CreateElementByXML(myDoc.DocumentElement);
            if (e != null)
                myContent.InsertElement(e);
            return true;
        }

        /// <summary>
        /// 创建一个字符对象
        /// </summary>
        /// <param name="vChar">字符</param>
        /// <returns>新建的字符对象</returns>
        public ZYTextChar CreateChar(char vChar)
        {
            ZYTextChar NewChar = ZYTextChar.Create(vChar);
            NewChar.OwnerDocument = this;
            if (!char.IsWhiteSpace(vChar))
                NewChar.CreatorIndex = mySaveLogs.CurrentIndex;
            NewChar.Deleteted = false;
            NewChar.Font = myView.DefaultFont;
            NewChar.Visible = true;
            return NewChar;
        }

        /// <summary>
        /// 根据一个字符串创建一个字符对象集合
        /// </summary>
        /// <param name="strText">字符串</param>
        /// <param name="vParent">新增的字符元素的父元素</param>
        /// <returns></returns>
        public System.Collections.ArrayList CreateChars(string strText, ZYTextContainer vParent)
        {
            System.Collections.ArrayList myList = new System.Collections.ArrayList();
            ZYTextChar NewChar = null;
            foreach (char c in strText)
            {
                if (c != '\r' || c != '\n')
                {
                    NewChar = this.CreateChar(c);
                    if (NewChar != null)
                    {
                        NewChar.Parent = vParent;
                        myList.Add(NewChar);
                    }
                    //myList.Add( this.CreateChar( c ));
                }
            }
            return myList;
        }

        /// <summary>
        /// 清除内容,删除所有元素,重置文档的设置,可用于新增文档做准备
        /// </summary>
        public void ClearContent()
        {
            //myMarks.Clear();
            //myUndoList.Clear();
            //myRedoList.Clear();
            UndoStack.ClearAll();

            //myContentChangeLog = null;
            myInfo.FileName = null;
            foreach (ZYTextElement myElement in myElements)
            {
                if (myElement is System.IDisposable)
                {
                    (myElement as System.IDisposable).Dispose();
                }
            }
            RootDocumentElement = new ZYTextDiv();
            RootDocumentElement.OwnerDocument = this;
            //mySaveLogs.Clear();
            //myDataSource.ClearQueryNames();
            //myDataSource.ClearQueryVariables();
            myInfo.Title = "";
            myInfo.ID = "";
            this.RefreshElements();
            this.RefreshSize();
            this.RefreshLine();
            myContent.SetSelection(0, 0);
            #region bwy :
            UpdateView();
            #endregion bwy :
            this.Refresh();

        }

        #endregion

        #region 对象从纯文本数据加载文档的函数群 **************************************************
        /// <summary>
        /// 从一个字符串加载文档内容
        /// </summary>
        /// <param name="strText">字符串对象</param>
        /// <returns>操作是否成功</returns>
        public bool FromText(string strText)
        {
            RootDocumentElement = new ZYTextDiv();
            RootDocumentElement.OwnerDocument = this;
            LoadTextElementsToList(strText, RootDocumentElement.ChildElements);
            return true;
        }

        /// <summary>
        /// 根据一段纯文本数据加载元素列表
        /// </summary>
        /// <param name="strText">文本数据</param>
        /// <param name="myList">保存新增的元素的列表</param>
        /// <returns>加载的元素个数，若参数错误则返回-1</returns>
        public int LoadTextElementsToList(string strText, System.Collections.ArrayList myList)
        {
            int iCount = 0;
            if (strText == null || myList == null)
                return -1;

            foreach (char vChar in strText)
            {
                if (vChar == '\n')
                {
                    myList.Add(new ZYTextParagraph());
                    iCount++;
                }
                else if (vChar != '\r')
                {
                    myList.Add(this.CreateChar(vChar));
                    iCount++;
                }
            }
            return iCount;
        }
        #endregion

        #region 对象和XML文档交换数据的函数群 *****************************************************


        /// <summary>
        /// 加载若干元素到列表中
        /// </summary>
        /// <remarks>本函数遍历XML节点,碰到文本节点则对其中每一个字符新建一个字符元素对象,
        /// 对于XML子节点则尝试新增一个元素,然后调用元素的FromXML函数加载XML数据</remarks>
        /// <param name="myElement">根XML节点</param>
        /// <param name="myList">保存加载的元素的列表</param>
        /// <returns>加载是否成功</returns>
        /// <seealso>ZYTextDocumentLib.ZYTextDocument.CreateElement</seealso>
        public bool LoadElementsToList(System.Xml.XmlElement myElement, System.Collections.ArrayList myList)
        {
            //myList.Clear();
            //myList.Add(myBlank);
            ZYTextElement NewElement = null;
            foreach (System.Xml.XmlNode myXMLNode in myElement.ChildNodes)
            {
                // 加载文本数据
                if (myXMLNode.Name == ZYTextConst.c_XMLText || myXMLNode.Name == ZYTextConst.c_Span)
                {
                    string strText = myXMLNode.InnerText;
                    if (strText != null && strText.Length > 0)
                    {
                        strText = System.Web.HttpUtility.HtmlDecode(strText);
                        strText = strText.Replace("\r", "");
                        strText = strText.Replace("\n", "");
                        if (strText.Length > 0)
                        {
                            ZYTextChar FirstChar = ZYTextChar.Create(' ');// new ZYTextChar();
                            myList.Add(FirstChar);
                            // 加载格式化文本
                            if (myXMLNode is System.Xml.XmlElement)
                                FirstChar.FromXML(myXMLNode as System.Xml.XmlElement);
                            else
                            {
                                // 加载纯文本
                                FirstChar.Char = strText[0];
                            }
                            ZYTextChar newChar = null;
                            for (int iCount = 1; iCount < strText.Length; iCount++)
                            {
                                newChar = ZYTextChar.Create(strText[iCount]);// new ZYTextChar();
                                FirstChar.Attributes.CopyTo(newChar.Attributes);
                                newChar.UpdateAttrubute();
                                newChar.CreatorIndex = FirstChar.CreatorIndex;
                                //newChar.CreateTime		= FirstChar.CreateTime ;
                                newChar.DeleterIndex = FirstChar.DeleterIndex;
                                //newChar.DeleteTime		= FirstChar.DeleteTime ;
                                //newChar.Char			= strText[iCount];
                                myList.Add(newChar);
                            }//for
                        }//if
                    }//if
                }//if

                else
                {
                    System.Xml.XmlElement myXMLE = myXMLNode as System.Xml.XmlElement;
                    if (myXMLE != null)
                    {
                        // 根据子XML节点创建一个对象
                        NewElement = this.CreateElement(myXMLE.Name);
                        if (NewElement != null)
                        {
                            // 加载对象数据
                            if (NewElement.FromXML(myXMLE))
                            {
                                myList.Add(NewElement);
                                // 拆分对象
                                //NewElement.SplitElement(myList ,-1);
                            }//if
                        }//if
                    }//if
                }//else
            }//foreach
            foreach (ZYTextElement vElement in myList)
                vElement.OwnerDocument = this;
            return true;
        }

        /// <summary>
        /// 从一个XML文档加载对象数据
        /// </summary>
        /// <param name="strURL">XML文件的URL</param>
        /// <returns>加载是否成功</returns>
        /// <seealso>ZYTextDocumentLib.ZYTextDocument.FromXML</seealso>
        public bool FromXMLFile(string strURL)
        {
            System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
            myDoc.PreserveWhitespace = true;
            myDoc.Load(strURL);

            if (FromXML(myDoc.DocumentElement))
            {
                myInfo.FileName = strURL;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 从一个XML节点加载文档数据
        /// </summary>
        /// <remarks>本函数加载文档的用户列表,文档参数,文档设置信息,并从根节点开始加载文档显示内容</remarks>
        /// <param name="RootElement">XML节点</param>
        /// <returns>加载是否成功</returns>
        /// <seealso>ZYTextDocumentLib.ZYTextDocument.CreateElementByXML</seealso>
        public bool FromXML(System.Xml.XmlElement RootElement)
        {
            // 清空对象数据
            bolLoading = true;
            UndoStack.ClearAll();
            RootElement.OwnerDocument.PreserveWhitespace = true;
            mySaveLogs.Clear();
            myMarks.Clear();
            //this.ClearContent(); mfb注释
            RootDocumentElement = null;
            string pat_id = myInfo.PatientID;
            if (myInfo.EnableSaveLog)
            {
                //myMarks.FromXML(XMLCommon.CreateChildElement(RootElement, myMarks.GetXMLName(), false));
                mySaveLogs.FromXML(XMLCommon.CreateChildElement(RootElement, mySaveLogs.GetXMLName(), false));
            }
            else
            {
                //myMarks.FromXML(null);
                mySaveLogs.FromXML(null);
            }
            myInfo.VisibleUserLevel = -1;

            // 加载文档信息 (mfb 2009-8-6 add)
            XmlElement mInfoEle = XMLCommon.GetElementByName(RootElement, ZYDocumentInfo.GetXMLName(), false);
            if (mInfoEle != null)
            {
                myInfo.FromXML(mInfoEle);
            }
            myInfo.PatientID = pat_id;
            //加载页面设置
            XmlNode pagesetting = RootElement.SelectSingleNode("pagesettings");
            if (pagesetting != null)
            {
                XmlElement ele = (pagesetting as XmlElement).SelectSingleNode("page") as XmlElement;
                XPageSettings ps = new XPageSettings();
                ps.PaperSize.Kind = (PaperKind)Enum.Parse(typeof(PaperKind), ele.GetAttribute("kind"));

                if (ps.PaperSize.Kind == PaperKind.Custom)
                {
                    ps.PaperSize.Height = int.Parse(ele.GetAttribute("height"));
                    ps.PaperSize.Width = int.Parse(ele.GetAttribute("width"));
                }

                ele = (pagesetting as XmlElement).SelectSingleNode("margins") as XmlElement;
                ps.Margins.Left = int.Parse(ele.GetAttribute("left"));
                ps.Margins.Right = int.Parse(ele.GetAttribute("right"));
                ps.Margins.Top = int.Parse(ele.GetAttribute("top"));
                ps.Margins.Bottom = int.Parse(ele.GetAttribute("bottom"));

                ele = (pagesetting as XmlElement).SelectSingleNode("landscape") as XmlElement;
                ps.Landscape = bool.Parse(ele.GetAttribute("value"));

                this.Pages.PageSettings = ps;
            }

            #region bwy :
            //加载页眉模板
            XmlNode header = RootElement.SelectSingleNode("header");
            if (header != null)
            {
                this.HeadString = header.OuterXml;
            }
            //加载页脚模板
            XmlNode footer = RootElement.SelectSingleNode("footer");
            if (footer != null)
            {
                this.FooterString = footer.OuterXml;
            }
            //行距 
            if (RootElement.HasAttribute("linespacing"))
            {
                this.Info.LineSpacing = int.Parse(RootElement.GetAttribute("linespacing"));
            }
            //加载页面设置
            //XmlNode pagesettings = 

            //XPageSettings ps = new XPageSettings();
            //double rate = XPaperSize.GetRate(this.intGraphicsUnit);
            //ps.Margins = new System.Drawing.Printing.Margins(
            //    (int)(this.intLeftMargin * rate),
            //    (int)(this.intRightMargin * rate),
            //    (int)(this.intTopMargin * rate),
            //    (int)(this.intBottomMargin * rate));
            //ps.Landscape = this.bolLanscape;
            //ps.PaperSize = this.PaperSize;

            //this.Pages.PageSettings = ps; 
            #endregion bwy :

            //遍历document里面的所有子节点
            foreach (System.Xml.XmlNode myNode in RootElement.ChildNodes)
            {
                if (myNode.Name == ZYTextConst.c_Div || myNode.Name == ZYTextConst.c_Body)
                {
                    RootDocumentElement = this.CreateElementByXML(myNode as System.Xml.XmlElement) as ZYTextContainer;
                    if (RootDocumentElement != null)
                    {
                        RootDocumentElement.OwnerDocument = this;
                        RootDocumentElement.UpdateUserLogin();

                        //myDocumentElement.UpdateBounds();

                        break;
                    }
                }
            }
            myContent.SetSelection(0, 0);
            this.RefreshElements();
            bolLoading = false;
            this.Modified = false;



            return true;
        }
        /// <summary>
        /// 根据一个XML节点创建一个文本文档元素,并填充结构化数据
        /// </summary>
        /// <param name="myElement">XML节点</param>
        /// <returns>新增的文本文档元素,若不支持该XML格式则返回空引用</returns>
        public ZYTextElement CreateElementByXML(System.Xml.XmlElement myElement)
        {
            if (myElement == null)
                return null;
            ZYTextElement NewElement = this.CreateElement(myElement.Name);
            if (NewElement != null)
            {
                //FillDataSource(myElement);
                NewElement.FromXML(myElement);
                NewElement.OwnerDocument = this;
                return NewElement;
            }
            return null;
        }
        /// <summary>
        /// 根据类型名称创建一个元素
        /// </summary>
        /// <remarks>目前支持类型名称为
        /// br		行对象 ZYTextLineEnd 
        /// p		段落对象 ZYTextParagraph
        /// div		文本块对象 ZYTextDiv
        /// select  下拉列表 ZYTextSelect
        /// img		图象对象 ZYTextImage
        /// input	输入域对象 ZYTextInput
        /// hr		水平线对象 ZYTextHRule
        /// keyword 关键字对象 ZYTextKeyWord
        /// 新增的元素将设置它的创建者为当前用户
        /// </remarks>
        /// <param name="strName">类型名称</param>
        /// <returns>新建的元素,若不支持该类型的返回空引用</returns>
        public virtual ZYTextElement CreateElement(string strName)
        {
            ZYTextElement NewElement = null;
            if (strName == null || strName.Trim().Length == 0)
                return null;
            strName = strName.Trim();
            switch (strName)
            {
                case ZYTextConst.c_Br:
                    NewElement = new ZYTextLineEnd();
                    break;
                case ZYTextConst.c_P:
                    NewElement = new ZYTextParagraph();
                    break;
                case ZYTextConst.c_Body:
                    NewElement = new ZYTextDiv();
                    break;
                case ZYTextConst.c_Div:
                    NewElement = new ZYTextDiv();
                    break;
                case ZYTextConst.c_Img:
                    NewElement = new ZYTextImage();
                    break;
                case ZYTextConst.c_PEOF:
                    NewElement = new ZYTextEOF();
                    break;
                case ZYTextConst.c_Selement:
                    NewElement = new ZYSelectableElement();
                    break;
                case ZYTextConst.c_FTimeElement:
                    NewElement = new ZYFormatDatetime();
                    break;
                case ZYTextConst.c_FNumElement:
                    NewElement = new ZYFormatNumber();
                    break;
                case ZYTextConst.c_FStrElement:
                    NewElement = new ZYFormatString();
                    break;
                case ZYTextConst.c_RoElement:
                    NewElement = new ZYFixedText();
                    break;
                case ZYTextConst.c_EMRText:
                    NewElement = new ZYText();
                    break;
                case ZYTextConst.c_BtnElement:
                    NewElement = new ZYButton();
                    break;
                case ZYTextConst.c_Helement:
                    NewElement = new ZYPromptText();
                    break;
                case ZYTextConst.c_MensesFormula://月经史公式
                    NewElement = new ZYMensesFormula();
                    break;

                case ZYTextConst.c_ToothCheck://牙齿检查 add by ywk 2012年11月27日16:55:40
                    NewElement = new ZYToothCheck();
                    break;

                case ZYTextConst.c_HorizontalLine://水平线
                    NewElement = new ZYHorizontalLine();
                    break;
                case ZYTextConst.c_Macro://宏
                    NewElement = new ZYMacro();
                    break;
                case ZYTextConst.c_Replace://可替换项
                    NewElement = new ZYReplace();
                    break;

                case ZYTextConst.c_Template://子模板
                    NewElement = new ZYTemplate();
                    break;
                case ZYTextConst.c_CheckBox://复选框
                    NewElement = new ZYCheckBox();
                    break;
                case ZYTextConst.c_PageEnd://分页符
                    NewElement = new ZYPageEnd();
                    break;
                case ZYTextConst.c_Flag://定位符
                    NewElement = new ZYFlag();
                    break;
                case ZYTextConst.c_Table://表格
                    NewElement = new TPTextTable();
                    break;
                case ZYTextConst.c_Row:
                    NewElement = new TPTextRow();
                    break;
                case ZYTextConst.c_Cell:
                    NewElement = new TPTextCell();
                    break;
                case ZYTextConst.c_LookupEditor:
                    NewElement = new ZYLookupEditor();
                    break;
            }
            if (NewElement != null)
            {
                NewElement.OwnerDocument = this;
                //NewElement.CreatorIndex = mySaveLogs.CurrentIndex;
            }
            return NewElement;
        }

        /// <summary>
        /// 保存文档内容到根XML节点中
        /// </summary>
        /// <remarks>本函数保存文档的用户列表,文档信息和配置信息以及文档内容到一个XML节点下</remarks>
        /// <param name="RootElement">XML节点</param>
        /// <returns>操作是否成功</returns>
        public bool ToXML(System.Xml.XmlElement RootElement)
        {
            if (RootElement != null)
            {
                RootElement.OwnerDocument.PreserveWhitespace = true;
                #region bwy :
                //保存文档相关信息
                //this.Info.ToXML(RootElement);
                //保存页面设置
                XmlElement pageSettings = RootElement.OwnerDocument.CreateElement("pagesettings");
                this.Pages.PageSettings.ToXML(pageSettings);
                RootElement.AppendChild(pageSettings);
                //保存页眉模板
                if (this.HeadString.Length > 0)
                {
                    XmlElement header = RootElement.OwnerDocument.CreateElement("header");
                    XmlDocument headerdoc = new XmlDocument();
                    //需要这句，否则设置新页眉时有间距，保存再打开后就没间距了
                    headerdoc.PreserveWhitespace = true;
                    headerdoc.LoadXml(this.HeadString);
                    header.InnerXml = headerdoc.DocumentElement.InnerXml;
                    RootElement.AppendChild(header);
                }

                //保存页脚模板
                if (this.FooterString.Length > 0)
                {
                    XmlElement footer = RootElement.OwnerDocument.CreateElement("footer");
                    XmlDataDocument footerdoc = new XmlDataDocument();
                    footerdoc.LoadXml(this.FooterString);
                    footer.InnerXml = footerdoc.DocumentElement.InnerXml;
                    RootElement.AppendChild(footer);
                }
                #endregion bwy :
                if (myInfo.SaveMode == 3)
                {
                    //这里的RootDocumentElement是ZYTextContainer的一个对象
                    if (RootDocumentElement != null)
                    {
                        RootDocumentElement.ToXML(RootElement);
                    }
                }
                else
                {
                    #region mfb
                    //RootElement.SetAttribute(ZYTextConst.c_Version, c_EditorVersion);
                    //RootElement.SetAttribute("checkcount", myMarks.Count.ToString());
                    //RootElement.SetAttribute("senior", myMarks.LastSenior);
                    mySaveLogs.CurrentSaveLog.SaveDateTime = ZYTime.GetServerTime();//System.DateTime.Now ;
                    if (myMarks.NewMark != null)
                        myMarks.NewMark.MarkTime = mySaveLogs.CurrentSaveLog.SaveDateTime;
                    if (myInfo.EnableSaveLog)
                    {
                        // 保存签名信息
                        myMarks.ToXML(XMLCommon.CreateChildElement(RootElement, myMarks.GetXMLName(), true));
                        // 保存保存文档记录
                        mySaveLogs.ToXML(XMLCommon.CreateChildElement(RootElement, mySaveLogs.GetXMLName(), true));

                    }
                    // 保存用户列表
                    //myUserList.ToXML( XMLCommon.CreateChildElement(RootElement , ZYUserList.c_RootXMLName ,true));
                    // 保存文档信息
                    //myInfo.ModifyTime = StringCommon.GetNowString14();
                    myInfo.ModifyTime = ZYTime.GetServerTime().ToString("yyyyMMddHHmmss");
                    myInfo.Version = c_EditorVersion;
                    myInfo.Modifier = mySaveLogs.CurrentIndex.ToString();
                    myInfo.ToXML(XMLCommon.CreateChildElement(RootElement, ZYDocumentInfo.GetXMLName(), true));
                    // 保存参数
                    //if (myVariables.Count > 0)
                    //{
                    //    System.Xml.XmlElement myValueElement = XMLCommon.CreateChildElement(RootElement, "values", true);
                    //    for (int iCount = 0; iCount < myVariables.Count; iCount++)
                    //    {
                    //        System.Xml.XmlElement myElement = RootElement.OwnerDocument.CreateElement("value");
                    //        myElement.SetAttribute("name", myVariables.GetName(iCount));
                    //        myElement.InnerText = myVariables.GetValue(iCount);
                    //        myValueElement.AppendChild(myElement);
                    //    }
                    //}
                    // 保存脚本对象
                    //myScript.ToXML(XMLCommon.CreateChildElement(RootElement, myScript.GetXMLName(), true));

                    //if (myInfo.SaveMode == 0)
                    //{
                    //    System.Xml.XmlElement TextElement = XMLCommon.CreateChildElement(RootElement, ZYTextConst.c_Text, true);
                    //    System.Text.StringBuilder myStr = new System.Text.StringBuilder();
                    //    RootDocumentElement.GetFinalText(myStr);
                    //    TextElement.InnerText = myStr.ToString();
                    //}
                    #endregion
                    if (RootDocumentElement != null)
                    {
                        RootDocumentElement.ToXML(XMLCommon.CreateChildElement(RootElement, ZYTextConst.c_Body, true));
                        //XmlElement bodyEle = XMLCommon.CreateChildElement(RootElement, ZYTextConst.c_Body, true);
                        //RootDocumentElement.ToXML(XMLCommon.CreateChildElement(bodyEle, ZYTextConst.c_BodyText, true));
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 保存文档到一个XML文件中
        /// </summary>
        /// <param name="strURL">一个XML文件名</param>
        /// <returns>操作是否成功</returns>
        public bool ToXMLFile(string strURL)
        {
            System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
            //myDoc.AppendChild(myDoc.CreateElement(ZYTextConst.c_EMRTextDoc));
            CreateRootElement(ref myDoc);

            if (ToXML(myDoc.DocumentElement))
            {
                myDoc.PreserveWhitespace = false;
                myDoc.Save(strURL);
                myInfo.FileName = strURL;
                return true;
            }
            return false;
        }

        //文档以字节数组方式返回
        public byte[] GetBinaryFile()
        {
            //获得xmldoc文档
            //System.Xml.XmlDocument myDoc = new System.Xml.XmlDocument();
            //pnlText.EMRDoc.CreateRootElement(ref myDoc);
            //ToXML(myDoc.DocumentElement);
            return new Byte[10000];
        }

        /// <summary>
        /// 保存文档对象到一个XML文档对象中
        /// </summary>
        /// <param name="myDoc">XML文档对象</param>
        /// <returns>操作是否成功</returns>
        public bool ToXMLDocument(System.Xml.XmlDocument myDoc)
        {
            //TODO: 这里改为了新的构建根元素
            myDoc.PreserveWhitespace = true;

            CreateRootElement(ref myDoc);

            return ToXML(myDoc.DocumentElement);
        }
        //mfb 添加一个根节点元素
        public void CreateRootElement(ref XmlDocument myDoc)
        {
            myDoc.LoadXml("<" + ZYTextConst.c_EMRTextDoc + "/>");
        }

        #endregion

        #region 绘制图形相关的函数群 **************************************************************

        /// <summary>
        /// 隐藏编辑器的光标
        /// </summary>
        public void HideCaret()
        {
            if (myOwnerControl != null)
                myOwnerControl.HideOwnerCaret();
        }

        /// <summary>
        /// 开始更新内容
        /// </summary>
        public void BeginUpdate()
        {
            if (myOwnerControl != null)
                myOwnerControl.BeginUpdate();
        }
        /// <summary>
        /// 结束更新内容
        /// </summary>
        public void EndUpdate()
        {
            if (myOwnerControl != null)
            {
                if (this.ElementsDirty)
                {
                    this.RefreshElements();
                    this.RefreshLine();
                }
                myOwnerControl.EndUpdate();
                if (myOwnerControl.Updating == false)
                {
                    UpdateView();
                }
            }
        }

        /// <summary>
        /// 更新视图区域
        /// </summary>
        public void UpdateView()
        {
            if (this.UpdateViewNoScroll())
            {
                UpdateTextCaret();
            }
        }

        /// <summary>
        /// 更新光标位置
        /// </summary>
        public void UpdateTextCaret()
        {
            if (myOwnerControl != null)
            {
                myOwnerControl.UpdateTextCaret();
            }
        }
        /// <summary>
        /// 更新视图区域
        /// </summary>
        public bool UpdateViewNoScroll()
        {
            if (myOwnerControl != null && myOwnerControl.Updating == false)
            {
                myOwnerControl.UpdatePages();
                myOwnerControl.UpdateInvalidateRect();
                if (RefreshAllFlag)
                {
                    myOwnerControl.Refresh();
                    RefreshAllFlag = false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断一个元素是否需要重新绘制
        /// </summary>
        /// <param name="myElement">元素对象</param>
        /// <returns>元素是否需要重绘</returns>
        public bool isNeedDraw(ZYTextElement myElement)
        {
            if (myElement == null)
                return false;
            else
                return myView.isNeedDraw(myElement.RealLeft, myElement.RealTop, myElement.Width, myElement.Height);
        }

        /// <summary>
        /// 判断一个矩形是否处于无效区域,是否需要进行重新绘制
        /// </summary>
        /// <param name="myRect">矩形对象</param>
        /// <returns>是否需要进行重绘</returns>
        public bool isNeedDraw(System.Drawing.Rectangle myRect)
        {
            //return true;
            return myView.isNeedDraw(myRect);
        }

        /// <summary>
        /// 判断一个矩形是否处于无效区域,是否需要重新进行绘制
        /// </summary>
        /// <param name="vLeft">矩形区域的左端位置</param>
        /// <param name="vTop">矩形区域的顶端位置</param>
        /// <param name="vWidth">矩形区域的宽度</param>
        /// <param name="vHeight">矩形区域的高度</param>
        /// <returns>是否需要进行重绘</returns>
        public bool isNeedDraw(int vLeft, int vTop, int vWidth, int vHeight)
        {
            return myView.isNeedDraw(vLeft, vTop, vWidth, vHeight);
        }



        public void UpdatePageSetting()
        {
            myPages.HeadHeight = this.HeadHeight;
            myPages.FooterHeight = this.FooterHeight;
        }


        internal int FixPageLine(int Pos)
        {
            //********todo 解决含有分页符的病历续打不能选中超出分页符部分的问题 Modified By wwj 2011-10-13 ***********
            /*
            PageLineInfo info = new PageLineInfo(0, 0, Pos, 0);
            FixPageLine(info);
            return info.Pos;
            */
            return Pos;
            //****************************************************************************************************
        }

        public void FixPageLine(PageLineInfo info)
        {
            int pos = info.Pos;
            int min = 10000;
            ZYTextLine MinLine = null;
            foreach (ZYTextLine myLine in myLines)
            {
                int intRealTop = myLine.RealTop;
                if (pos >= intRealTop
                    && pos - intRealTop < min
                    && pos < intRealTop + myLine.FullHeight)
                {
                    MinLine = myLine;
                    min = pos - intRealTop;
                }
                #region bwy
                if (info.LastPos <= myLine.RealTop && myLine.RealTop + myLine.FullHeight <= info.Pos && myLine.FirstElement is ZYPageEnd)
                {
                    MinLine = myLine;
                    min = pos - intRealTop;
                    info.Pos = MinLine.RealTop + myLine.FullHeight;
                    return;
                }
                #endregion bwy


            }
            if (MinLine != null)
                info.Pos = MinLine.RealTop;
            else
                info.Pos = RootDocumentElement.Top + RootDocumentElement.Height;
        }
        /// <summary>
        /// 绘制全部内容
        /// </summary>
        /// <returns>操作是否成功</returns>
        private bool DrawAll()
        {
            RootDocumentElement.RefreshView();
            return true;
        }
        #region Refresh相关方法
        /// <summary>
        /// 重新绘制整个文档
        /// </summary>
        public void Refresh()
        {
            if (myOwnerControl != null)
                myOwnerControl.Invalidate();//.Refresh();
        }

        /// <summary>
        /// 重新绘制一个元素
        /// </summary>
        /// <param name="myElement">元素对象</param>
        public void RefreshElement(ZYTextElement myElement)
        {
            if (myOwnerControl != null)
            {
                if (myElement is ZYTextContainer)
                {
                    myOwnerControl.AddInvalidateRect((myElement as ZYTextContainer).GetContentBounds());
                }
                else
                    myOwnerControl.AddInvalidateRect(myElement.RealLeft, myElement.RealTop, myElement.Width + myElement.WidthFix, myElement.OwnerLine.Height);
                myOwnerControl.UpdateInvalidateRect();
            }
        }
        public void RefreshElements()
        {
            RefreshElements(false);
        }
        /// <summary>
        /// 刷新文档全体元素列表,将所有的文本类型元素添加到元素列表中
        /// </summary>
        public void RefreshElements(bool FixNative)
        {
            myElements.Clear();
            if (RootDocumentElement != null)
            {
                RootDocumentElement.AddElementToList(myElements, true);
                //if (FixNative)
                //FixForNative(myElements);
                int index = 0;
                foreach (ZYTextElement myElement in myElements)
                {
                    myElement.Visible = true;
                    myElement.Index = index;
                    index++;
                }

                myContent.AutoClearSelection = true;
                myContent.MoveSelectStart(myContent.SelectStart);
                myContainters.Clear();
                //SetContainers(myDocumentElement);
                //myAllElement = null;
            }
        }

        /// <summary>
        /// 对全部文档重新进行排布和分行,并进行分页处理
        /// </summary>
        /// <returns></returns>
        public bool RefreshLine()
        {
            //myPages.UpdatePageSettings();

            myLines.Clear();
            this.UpdatePageSetting();
            if (RootDocumentElement != null)
            {
                RootDocumentElement.RefreshLine();
                RefreshPages();
                if (this.myOwnerControl != null)
                {
                    this.myOwnerControl.UpdatePages();
                }
                this.ElementsDirty = false;
            }
            return true;
        }
        /// <summary>
        /// 重新进行分页
        /// </summary>
        public void RefreshPages()
        {
            myPages.Clear();
            myPages.DocumentHeight = RootDocumentElement.Height;
            //myPages.Reset( g );
            myPages.HeadHeight = this.HeadHeight;
            myPages.FooterHeight = this.FooterHeight;

            myPages.Top = 0; //this.Top + this.HeadHeight ;

            int BodyHeight = RootDocumentElement.Height;
            int LastPos = myPages.Top;
            myPages.MinPageHeight = 15;
            while (myPages.Height < BodyHeight)
            {
                PrintPage page = myPages.NewPage();
                PageLineInfo info = new PageLineInfo(
                    myPages.Top,
                    LastPos,
                    page.Bottom,
                    myPages.Count);
                info.MinPageHeight = myPages.MinPageHeight;
                this.FixPageLine(info);
                page.Height = info.Pos - page.Top;
                if (page.Height < myPages.MinPageHeight)
                    page.Height = myPages.StandardHeight;
                LastPos = page.Bottom;
                myPages.Add(page);
            }//while
            if (myPages.Count > 0)
            {
                myPages.LastPage.Height =
                    myPages.LastPage.Height - (myPages.Height - BodyHeight);
            }
        }

        /// <summary>
        /// 重新计算文档元素的大小
        /// </summary>
        /// <returns>操作是否成功</returns>
        public bool RefreshSize()
        {
            if (RootDocumentElement != null)
            {
                RootDocumentElement.Left = 0;
                RootDocumentElement.Top = 0;
                RootDocumentElement.RefreshSize();
                //RootDocumentElement.RefreshLine(); mfb注释
                myView.Height = RootDocumentElement.Top + RootDocumentElement.Height;
            }
            return true;
        }

        #endregion
        /// <summary>
        /// 获得预览用的图片对象
        /// </summary>
        /// <returns>获得当前文档的BMP预览图片</returns>
        public System.Drawing.Bitmap GetPreViewImage()
        {
            return GetPreViewImage(0, 0);
        }

        /// <summary>
        /// 获得预览用的图片对象
        /// </summary>
        /// <param name="ImgWidth">指定的输出图像的宽度,若小于等于0则自动设置图像宽度</param>
        /// <param name="ImgHeight">指定的输出图像的高度,若小于等于0则自动设置图像宽度</param>
        /// <returns>获得当前文档的BMP预览图片</returns>
        public System.Drawing.Bitmap GetPreViewImage(int ImgWidth, int ImgHeight)
        {
            bool bolFlag = myInfo.ShowParagraphFlag;
            myInfo.ShowParagraphFlag = false;
            myInfo.Printing = true;
            System.Drawing.Bitmap mybmp = myView.GetBitmap(new BoolNoParameterHandler(RefreshSize), new BoolNoParameterHandler(DrawAll), ImgWidth, ImgHeight);
            myInfo.ShowParagraphFlag = bolFlag;
            myInfo.Printing = false;
            return mybmp;
        }

        /// <summary>
        /// 绘制新增的元素的背景色,目前为Windows提示文本背景色,一般为淡黄色
        /// </summary>
        /// <param name="intLevel">元素的显示层次</param>
        /// <param name="vLeft">元素左端位置</param>
        /// <param name="vTop">元素顶端位置</param>
        /// <param name="vWidth">元素宽度</param>
        /// <param name="vHeight">元素高度</param>
        public void DrawNewBackGround(int intLevel, int vLeft, int vTop, int vWidth, int vHeight)
        {
            if (myInfo.ShowMark == false) return;
            switch (intLevel)
            {
                case 0:
                    myView.FillRectangle(System.Drawing.SystemColors.Info, vLeft, vTop, vWidth, vHeight);
                    break;
                case 1:
                    myView.FillRectangle(System.Drawing.SystemColors.Info, vLeft, vTop, vWidth, vHeight);
                    myView.DrawLine(System.Drawing.SystemColors.Highlight, vLeft, vTop + vHeight - 1, vLeft + vWidth, vTop + vHeight - 1);
                    break;
                default:
                    myView.FillRectangle(System.Drawing.SystemColors.Info, vLeft, vTop, vWidth, vHeight);
                    myView.DrawLine(System.Drawing.SystemColors.Highlight, vLeft, vTop + vHeight - 1, vLeft + vWidth, vTop + vHeight - 1);
                    myView.DrawLine(System.Drawing.SystemColors.Highlight, vLeft, vTop + vHeight - 3, vLeft + vWidth, vTop + vHeight - 3);
                    break;
            }
        }

        public void DrawUnderLine(int intLevel, int vLeft, int vTop, int vWidth, int vHeight)
        {
            if (myInfo.ShowMark == false) return;
            switch (intLevel)
            {
                case 0:
                    break;
                case 1:
                    myView.DrawLine(System.Drawing.Color.Red, vLeft, vTop + vHeight - 5, vLeft + vWidth, vTop + vHeight - 5);
                    break;
                case 2:
                    myView.DrawDoubleUnderLine(vLeft, vTop, vWidth, vHeight);
                    break;
            }
        }

        /// <summary>
        /// 绘制被逻辑删除的元素的删除线
        /// </summary>
        /// <param name="intLevel">元素的显示层次</param>
        /// <param name="vLeft">元素左端位置</param>
        /// <param name="vTop">元素顶端位置</param>
        /// <param name="vWidth">元素宽度</param>
        /// <param name="vHeight">元素高度</param>
        /// <seealso>ZYCommon.DocumentView.DrawDeleteLine</seealso>
        public void DrawDeleteLine(int intLevel, int vLeft, int vTop, int vWidth, int vHeight)
        {
            if (myInfo.ShowMark == false) return;
            myView.DrawDeleteLine(vLeft, vTop, vWidth, vHeight, (intLevel <= 1 ? 1 : 2));
        }

        #endregion

        #region 实现 IEMRViewDocument 成员,以便向用户界面负责 *************************************

        /// <summary>
        /// 定时器处理
        /// </summary>
        /// <returns>是否进行了处理</returns>
        public bool ViewTimer()
        {
            if (bolLastContentChangedFlag)
            {
                bolLastContentChangedFlag = false;
                //ZYEditorAction a = this.GetActionByName("contentchanged");
                ZYEditorAction a = new A_ContentChanged();
                if (a != null && a.isEnable())
                {
                    a.Execute();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 绘制页眉
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ClipRectangle"></param>
        public void DrawHead(System.Drawing.Graphics g, System.Drawing.Rectangle ClipRectangle)
        {
            DrawExtString(this.RuntimeHeadString, g, ClipRectangle);
        }
        /// <summary>
        /// 绘制页脚
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ClipRectangle"></param>
        public void DrawFooter(System.Drawing.Graphics g, System.Drawing.Rectangle ClipRectangle)
        {
            DrawExtString(this.RuntimeFooterString, g, ClipRectangle);
        }

        //绘制页眉页脚时用此方法
        private void DrawExtString(string txt, System.Drawing.Graphics g, System.Drawing.Rectangle ClipRectangle)
        {
            //Debug.WriteLine("页眉页脚字符串 "+txt);
            if (txt.Length == 0)
            {
                return;
            }

            System.Drawing.Font DefaultFont = System.Windows.Forms.Control.DefaultFont;
            //if (txt.IndexOf("<") == -1 || txt.IndexOf(">") == -1)
            //{
            //    using (System.Drawing.StringFormat format = new System.Drawing.StringFormat())
            //    {
            //        format.Alignment = System.Drawing.StringAlignment.Near;
            //        format.LineAlignment = System.Drawing.StringAlignment.Near;
            //        g.DrawString(txt, DefaultFont, System.Drawing.Brushes.Black, new System.Drawing.RectangleF(ClipRectangle.Left, ClipRectangle.Top, ClipRectangle.Width, ClipRectangle.Height), format);
            //    }
            //    return;
            //}
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(txt);

            bool header = true;
            XmlNodeList nodes = doc.SelectNodes("header/p");
            if (nodes.Count == 0)
            {
                //Debug.WriteLine("画页脚");
                nodes = doc.SelectNodes("footer/p");
                header = false;
            }
            int TopCount = ClipRectangle.Top;
            using (System.Drawing.StringFormat format = new System.Drawing.StringFormat())
            {
                format.Alignment = System.Drawing.StringAlignment.Near;
                format.LineAlignment = System.Drawing.StringAlignment.Center;
                format.FormatFlags = System.Drawing.StringFormatFlags.NoWrap | StringFormatFlags.MeasureTrailingSpaces;
                bool resetName = false;
                int lineIndex = 0;
                float maxWidth = 0f;//存储页眉中最长字符串的宽度
                float maxX = 0f;
                foreach (System.Xml.XmlNode node in nodes)//doc.DocumentElement.ChildNodes)
                {
                    lineIndex++;
                    if (node is System.Xml.XmlElement)
                    {
                        System.Xml.XmlElement element = (System.Xml.XmlElement)node;

                        //if (element.Name == "line")
                        XmlElement realnode = element.SelectSingleNode("horizontalLine") as XmlElement;
                        //是水平线节点
                        if (realnode != null)
                        {
                            using (System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Black))
                            {
                                if (realnode.HasAttribute("lineHeight"))
                                {

                                }//("thick"))
                                pen.Width = Convert.ToInt32(realnode.GetAttribute("lineHeight"));
                                #region bwy : 是否显示页眉线，如不显示，用页面背景色画线

                                if (doc.DocumentElement.Name == "header" && !this.ShowHeaderLine)
                                {
                                    pen.Color = this.OwnerControl.PageBackColor;
                                }

                                if (doc.DocumentElement.Name == "footer" && !this.ShowFooterLine)
                                {
                                    pen.Color = this.OwnerControl.PageBackColor;
                                }

                                #endregion bwy :

                                g.DrawLine(pen, ClipRectangle.Left, TopCount, ClipRectangle.Right, TopCount);
                                TopCount = TopCount + (int)pen.Width + 20;
                            }
                        }
                        //不是水平线节点
                        else //if (element.Name == "text")
                        {
                            XmlElement span = element.SelectSingleNode("span") as XmlElement;

                            if (span == null)
                            {
                                continue;
                            }


                            string FontName = DefaultFont.Name;
                            if (span.HasAttribute("fontname"))
                                FontName = span.GetAttribute("fontname");
                            float FontSize = (float)DefaultFont.Size;
                            if (span.HasAttribute("fontsize"))
                                FontSize = float.Parse(span.GetAttribute("fontsize"));
                            System.Drawing.FontStyle style = System.Drawing.FontStyle.Regular;
                            if (span.HasAttribute("fontbold"))
                                style = style | System.Drawing.FontStyle.Bold;
                            if (span.HasAttribute("fontitalic"))
                                style = style | System.Drawing.FontStyle.Italic;
                            if (span.HasAttribute("fontunderline"))
                                style = style | System.Drawing.FontStyle.Underline;

                            format.Alignment = System.Drawing.StringAlignment.Near;
                            if (element.HasAttribute("align") && Convert.ToInt32(element.GetAttribute("align")) == 1)
                                format.Alignment = System.Drawing.StringAlignment.Near;
                            if (element.HasAttribute("align") && Convert.ToInt32(element.GetAttribute("align")) == 2)
                                format.Alignment = System.Drawing.StringAlignment.Center;
                            if (element.HasAttribute("align") && Convert.ToInt32(element.GetAttribute("align")) == 3)
                                format.Alignment = System.Drawing.StringAlignment.Far;

                            SolidBrush brush = new SolidBrush(Color.Black);
                            if (span.HasAttribute("forecolor"))
                            {
                                System.Drawing.ColorConverter a = new System.Drawing.ColorConverter();
                                System.Drawing.Color c = (System.Drawing.Color)a.ConvertFromString(span.GetAttribute("forecolor"));
                                brush.Color = c;
                            }

                            Font font = new System.Drawing.Font(FontName, FontSize, style);
                            float h = g.MeasureString("#", font, 10000, format).Height + 10;
                            System.Drawing.RectangleF rect = new System.Drawing.RectangleF(ClipRectangle.Left, TopCount, ClipRectangle.Width, h);
                            if (rect.Bottom > ClipRectangle.Bottom)
                                break;
                            string txt2 = element.InnerText;

                            if (header && !resetName)
                            {
                                if (this.Info.DocumentModel != DocumentModel.Test)
                                {
                                    //txt2 = "秦皇岛市第二医院                      ";//此处 要30个符号位置，字母、汉字都算一个
                                    txt2 = span.InnerText;//此处 要30个符号位置，字母、汉字都算一个
                                    txt2 = txt2.Trim();
                                }
                                resetName = true;
                            }

                            if (txt2 != null && txt2.Length > 0)
                            {
                                //如果是个人信息，那么分散对齐false)
                                if (lineIndex == 3)
                                //if(false)
                                {
                                    //把字符串以空格分割成数组,  姓名：姓名　　性别：性别　　年龄：年龄　　民族：民族
                                    //txt2 = txt2.Replace("\r\n","");
                                    string[] parts = txt2.Trim().Split(new char[] { ' ', ' ', '　', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    List<Size> elesize = new List<Size>();
                                    List<Rectangle> elebounds = new List<Rectangle>();

                                    //总长度
                                    int length = 0;

                                    //如果内容总长度大于页面宽度，那么缩小字体大小，以显示全部内容
                                    while ((length = GetTotalWidth(parts, g, font, format, elesize)) > this.Pages.StandardWidth)
                                    {
                                        font = new Font(font.FontFamily.Name, font.Size - 0.5f, font.Style);
                                    }
                                    //如果内容总长度小于页面宽度，那么计算要插入的空白
                                    int space = this.Pages.StandardWidth - length;
                                    if (parts.Length > 1)
                                    {
                                        space = space / (parts.Length - 1);
                                    }
                                    //修正所有元素的左边界
                                    int curleft = (int)rect.Left;
                                    for (int k = 0; k < elesize.Count; k++)
                                    {
                                        curleft = curleft + (k == 0 ? 0 : space);
                                        Rectangle r = new Rectangle(curleft, (int)rect.Y, elesize[k].Width + 5, elesize[k].Height + 5);
                                        elebounds.Add(r);
                                        curleft = curleft + elesize[k].Width;
                                    }

                                    //画各部分
                                    for (int k = 0; k < elebounds.Count; k++)
                                    {
                                        g.DrawString(parts[k], font, brush, elebounds[k], format);
                                        //g.DrawString(txt2, font, brush, rect, format);
                                    }


                                }
                                else
                                {
                                    g.DrawString(txt2, font, brush, rect, format);
                                }
                            }

                            TopCount += (int)Math.Ceiling(h);


                            if (element.InnerXml.IndexOf("<macro") >= 0) //存在<macro>节点 跳过
                            {
                                continue;
                            }
                            float _width = g.MeasureString(element.InnerText, font).Width;
                            maxWidth = maxWidth < _width ? _width : maxWidth;
                            maxX = (rect.Width - maxWidth) / 2;

                            //}//using font
                        }
                    }
                }
                //若用AutoUpdate程序运行此处，将调取不到XML及LOGO图片
                //edit by ywk 2013年2月5日13:24:22 修改
                string currentsoftpath = System.AppDomain.CurrentDomain.BaseDirectory;
                //绘制医院logo edit by tj 2012-11-5
                //if (File.Exists("HospitalLogo.xml"))
                if (File.Exists(currentsoftpath + "\\HospitalLogo.xml"))
                {
                    XmlDocument _doc = new XmlDocument();
                    _doc.Load(currentsoftpath + "\\HospitalLogo.xml");
                    XmlNode node = _doc.GetElementsByTagName("Image")[0];

                    if (node != null)
                    {
                        //edit by ywk 路径修改 2013年2月5日13:42:46 
                        if (File.Exists(currentsoftpath + node.Attributes["src"].Value))
                        {
                            float curX = float.Parse(node.Attributes["x"].Value.ToString() == "" ? "0.00" : node.Attributes["x"].Value);
                            if (node.Attributes["align"].Value.ToLower() == "center")
                            {
                                curX = maxX - float.Parse(node.Attributes["w"].Value.ToString() == "" ? "0" : node.Attributes["w"].Value.ToString());
                            }
                            //edit by ywk 路径修改 2013年2月5日13:42:46 
                            g.DrawImage(Image.FromFile(currentsoftpath + node.Attributes["src"].Value),
                               curX,
                                float.Parse(node.Attributes["y"].Value.ToString() == "" ? "0.00" : node.Attributes["y"].Value),
                                float.Parse(node.Attributes["w"].Value.ToString() == "" ? "0.00" : node.Attributes["w"].Value),
                                float.Parse(node.Attributes["h"].Value.ToString() == "" ? "0.00" : node.Attributes["h"].Value)
                               );
                        }
                    }
                }
                //-----------------------------------------------------
            }
        }
        /// <summary>
        /// 测量各部分长度
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="g"></param>
        /// <param name="font"></param>
        /// <param name="format"></param>
        /// <param name="elesize"></param>
        /// <returns></returns>
        int GetTotalWidth(string[] parts, Graphics g, Font font, StringFormat format, List<Size> elesize)
        {
            elesize.Clear();
            //总长度
            int length = 0;
            //测量各部分长度
            foreach (string str in parts)
            {
                SizeF sf = g.MeasureString(str, font, 10000, format);
                Size s = new Size((int)sf.Width, (int)sf.Height);
                elesize.Add(s);
                length += s.Width;
            }

            return length;
        }
        /// <summary>
        /// 绘制文档
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ClipRectangle"></param>
        public void DrawDocument(System.Drawing.Graphics g, System.Drawing.Rectangle ClipRectangle)
        {
            myView.ViewRect = ClipRectangle;
            System.Drawing.Graphics gBack = myView.GetInnerGraph();
            myView.Graph = g;
            //InitEventObject(ZYVBEventType.Paint);
            if (RootDocumentElement != null)
            {
                RootDocumentElement.RefreshView();
            }
            //			if(myInfo.ShowPageLine && myOwnerControl != null && myOwnerControl.PageViewMode == false)
            //			{
            //				// 绘制分页线
            //				using(System.Drawing.Pen myPen = new System.Drawing.Pen( System.Drawing.Color.Gray ,1))
            //				{
            //					myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot ;
            //					foreach(PrintPage myPage in myPages)
            //					{
            //						if( myPage.Bottom >= ClipRectangle.Top && myPage.Bottom <= ClipRectangle.Bottom )
            //						{
            //							g.DrawLine( myPen , ClipRectangle.Left , myPage.Bottom , ClipRectangle.Right , myPage.Bottom );
            //						}
            //					}
            //				}
            //			}
            myView.Graph = gBack;
        }

        /// <summary>
        /// 实现<link>ZYCommon.IEMRViewDocument.ViewPaint</link>接口,重新绘制文档的一部分
        /// </summary>
        /// <remarks>本函数设置当前无效矩形,并重新绘制文档,若编辑控件不是分页模式则绘制分页线</remarks>
        /// <param name="g">用于绘制的图形操作对象</param>
        /// <param name="ClipRect">包含文档中需要绘制的区域的矩形</param>
        /// <returns>操作是否成功</returns>
        public bool ViewPaint(System.Drawing.Graphics g, System.Drawing.Rectangle ClipRect)
        {
            myView.ViewRect = ClipRect;
            System.Drawing.Graphics gBack = myView.GetInnerGraph();
            myView.Graph = g;
            //InitEventObject(ZYVBEventType.Paint);
            if (RootDocumentElement != null)
            {
                RootDocumentElement.RefreshView();
            }
            //			if(myInfo.ShowPageLine && myOwnerControl != null && myOwnerControl.PageViewMode == false)
            //			{
            //				// 绘制分页线
            //				using(System.Drawing.Pen myPen = new System.Drawing.Pen( System.Drawing.Color.Gray ,1))
            //				{
            //					myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot ;
            //					foreach(PrintPage myPage in myPages)
            //					{
            //						if( myPage.Bottom >= ClipRect.Top && myPage.Bottom <= ClipRect.Bottom)
            //						{
            //							g.DrawLine( myPen , ClipRect.Left , myPage.Bottom  , ClipRect.Right , myPage.Bottom  );
            //						}
            //					}
            //				}
            //			}
            myView.Graph = gBack;
            return true;
        }

        /// <summary>
        /// 实现 <link>ZYCommon.IEMRViewDocument.ViewMouseDown</link> 接口,鼠标按键按下事件
        /// </summary>
        /// <param name="x">鼠标光标在文档视图区域中的X坐标</param>
        /// <param name="y">鼠标光标在文档视图区域中的X坐标</param>
        /// <param name="Button">鼠标按键</param>
        /// <returns>操作是否成功</returns>
        public bool ViewMouseDown(int x, int y, System.Windows.Forms.MouseButtons Button)
        {
            //InitEventObject(ZYVBEventType.MouseDown);


            if (Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (myContent.SelectLength == 0)
                {
                    myContent.AutoClearSelection = true;
                    myContent.MoveTo(x, y);
                }
                //return true;不能返回，否则 不能执行光标修正操作
            }
            if (RootDocumentElement != null && Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (RootDocumentElement.HandleMouseDown(x, y, Button))
                {
                    myOwnerControl.CaptureMouse = false;
                    //return true;不能返回，否则 不能执行光标修正操作
                }
            }
            myContent.AutoClearSelection = !myOwnerControl.HasShiftPressing();

            #region bwy:
            //如果当前字符在元素内，则光标应放在元素之前
            ZYTextElement ele = this.GetElementByPos(x, y);
            if (ele != null && ele.Parent is ZYTextBlock)
            {
                myOwnerControl.CaptureMouse = false;
                x = ele.Parent.FirstElement.RealLeft;
                y = ele.Parent.FirstElement.RealTop;
            }
            if (ele is ZYElement)
            {
                myOwnerControl.CaptureMouse = false;
                x = ele.RealLeft;
                y = ele.RealTop;
            }
            #endregion bwy:

            myContent.MoveTo(x, y);

            myOwnerControl.UpdateTextCaret();
            myOwnerControl.UseAbsTransformPoint = true;
            return true;
        }

        /// <summary>
        /// 实现<link>ZYCommon.IEMRViewDocument.ViewMouseMove</link>接口,鼠标移动事件
        /// 2009-7-4 00:58 孟凡博改
        /// </summary>
        /// <param name="x">鼠标光标在文档视图区域中的X坐标</param>
        /// <param name="y">鼠标光标在文档视图区域中的Y坐标</param>
        /// <param name="Button">鼠标按键</param>
        /// <returns>操作是否成功</returns>
        public bool ViewMouseMove(int x, int y, System.Windows.Forms.MouseButtons Button)
        {
            //Debug.WriteLine("◆◆◆◆" + RootDocumentElement.HandleMouseMove(x, y, Button).ToString() + "◆◆◆◆");
            if (RootDocumentElement.HandleMouseMove(x, y, Button) == false)
            {
                //Debug.WriteLine("◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆◆");
                //这里判断是否是划选,否则为普通的移动
                if (myOwnerControl.CaptureMouse)
                {
                    //Debug.WriteLine("zytextdocument ViewMouseMove myOwnerControl.CaptureMouse=true ");
                    myContent.AutoClearSelection = false;
                    myContent.MoveTo(x, y);
                    ZYTextElement myElement = myContent.CurrentElement;
                    myOwnerControl.MoveCaretWithScroll = false;
                    myOwnerControl.UpdateTextCaret();
                    myOwnerControl.MoveCaretWithScroll = true;

                }
                else
                {
                    if (Button != System.Windows.Forms.MouseButtons.None)
                        return false;

                    myCursor = System.Windows.Forms.Cursors.IBeam;//bwy Default

                    if (myCurrentHoverElement != null)
                    {
                        if (Button != System.Windows.Forms.MouseButtons.None)
                            return false;

                        myCursor = System.Windows.Forms.Cursors.IBeam;//bwy Default

                        //RootDocumentElement.HandleMouseMove(x, y, Button);

                        if (myCurrentHoverElement != null)
                        {
                            if (myCurrentHoverElement is ZYTextContainer)
                            {
                                if ((myCurrentHoverElement as ZYTextContainer).Contains(x, y) == false)
                                    this.CurrentHoverElement = null;
                            }
                            else if (myCurrentHoverElement.Bounds.Contains(x, y) == false)
                            {
                                this.CurrentHoverElement = null;
                            }
                        }
                        myOwnerControl.SetCursor(myCursor);

                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 实现<link>ZYCommon.IEMRViewDocument.ViewMouseUp</link>接口,鼠标按键松开事件
        /// </summary>
        /// <param name="x">鼠标光标在文档视图区域中的X坐标</param>
        /// <param name="y">鼠标光标在文档视图区域中的X坐标</param>
        /// <param name="Button">鼠标按键</param>
        /// <returns>操作是否成功</returns>
        public bool ViewMouseUp(int x, int y, System.Windows.Forms.MouseButtons Button)
        {
            myOwnerControl.UseAbsTransformPoint = false;
            //mfb 添加.2009-7-4 00:25
            myContent.AutoClearSelection = true;
            RootDocumentElement.HandleMouseUp(x, y, Button);
            //InitEventObject(ZYVBEventType.MouseUp);
            return true;
        }

        /// <summary>
        /// 实现 <link>ZYCommon.IEMRViewDocument.ViewMouseClick</link> 接口,鼠标单击事件
        /// </summary>
        /// <param name="x">鼠标光标在文档视图区域中的X坐标</param>
        /// <param name="y">鼠标光标在文档视图区域中的X坐标</param>
        /// <param name="Button">鼠标按键</param>
        /// <returns>操作是否成功</returns>
        public bool ViewMouseClick(int x, int y, System.Windows.Forms.MouseButtons Button)
        {

            RootDocumentElement.HandleClick(x, y, Button);
            return true;
        }

        /// <summary>
        /// 实现 <link>ZYCommon.IEMRViewDocument.ViewMouseDoubleClick</link> 接口,鼠标双击事件
        /// </summary>
        /// <param name="x">鼠标光标在文档视图区域中的X坐标</param>
        /// <param name="y">鼠标光标在文档视图区域中的X坐标</param>
        /// <param name="Button">鼠标按键</param>
        /// <returns>操作是否成功</returns>
        public bool ViewMouseDoubleClick(int x, int y, System.Windows.Forms.MouseButtons Button)
        {
            //InitEventObject(ZYVBEventType.DblClick);
            if (RootDocumentElement.HandleDblClick(x, y, Button) == false)
            {
                //Add by wwj 2013-05-29 解决在平板上使用，将单击操作模拟成双击操作时，会选中当前选中元素所在语句的问题
                if (this.myOwnerControl.IsSingleClickAsDoubleClick) return true;

                if (myContent.CurrentElement is ZYTextChar || myContent.PreElement is ZYTextChar)
                {
                    ZYTextChar myChar;
                    int NewStart = -1;
                    ZYTextContainer ctr = myContent.CurrentElement.Parent;
                    for (int index = (myContent.CurrentElement is ZYTextChar ? myContent.SelectStart : myContent.SelectStart - 1); index >= 0; index--)
                    {
                        myChar = myElements[index] as ZYTextChar;
                        if (myChar == null || char.IsLetter(myChar.Char) == false || myChar.Parent != ctr)
                        {
                            break;
                        }
                        NewStart = index;
                    }
                    if (NewStart >= 0)
                    {
                        for (int index = NewStart; index < myElements.Count; index++)
                        {
                            myChar = myElements[index] as ZYTextChar;
                            if (myChar == null || char.IsLetter(myChar.Char) == false)
                            {
                                myContent.SetSelection(index, NewStart - index);
                                break;
                            }
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 实现<link>ZYCommon.IEMRViewDocument.ViewMouseLeave</link>接口,鼠标离开文档对象
        /// </summary>
        public bool ViewMouseLeave()
        {
            //InitEventObject(ZYVBEventType.MouseLeave);
            this.CurrentHoverElement = null;
            return true;
        }
        /// <summary>
        /// 实现 <link>ZYCommon.IEMRViewDocument.ViewInsertModeChange</link> 接口,插入模式修改事件
        /// </summary>
        public void ViewInsertModeChange()
        {
            ZYTextElement myElement = myContent.CurrentElement;
            myOwnerControl.UpdateTextCaret();
        }

        #endregion

        #region ▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅ 表格相关函数群 ▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅

        /// <summary>
        /// mfb 插入一个表格.
        /// 列宽如果为0,则为自动列宽,则此时列宽是表格总宽度的平均值.
        /// </summary>
        /// <param name="header">表格名称</param>
        /// <param name="rows">行数</param>
        /// <param name="columns">列数</param>
        /// <param name="columnWidth">列宽(厘米)</param>
        public void TableInsert(string header, int rows, int columns, decimal columnWidth)
        {
            if (this.Info.DocumentModel == DocumentModel.Design || this.Info.DocumentModel == DocumentModel.Edit)
            {
                this.BeginUpdate();
                if (myContent.HasSelected())
                    myContent.DeleteSeleciton();
                else if (myOwnerControl.InsertMode == false)
                    myContent.DeleteCurrentElement();

                //用来保存列宽
                int sColumn = 0;
                //如果有固定列宽,则转换为文档的单位
                if (0 != columnWidth)
                {
                    //将厘米单位的列宽转换为文档单位,然后再乘以列数,即为表格总宽
                    sColumn = (int)GraphicsUnitConvert.Convert((Convert.ToDouble(columnWidth * 10)), System.Drawing.GraphicsUnit.Millimeter, System.Drawing.GraphicsUnit.Document);
                }

                //根据行,列,表格宽,初始化表格.
                TPTextTable table = new TPTextTable(header, columns, rows, sColumn, myContent.CurrentElement.Height, this.Pages.StandardWidth);


                //设置表格所属的文档对象模型,为RefreshView准备
                table.OwnerDocument = this;

                myContent.AutoClearSelection = true;

                myContent.InsertTable(table);

                this.EndUpdate();
            }
        }
        /// <summary>
        /// 根据表格标题获取指定的表格,如果多个表格具有相同的名称,则会返回所有具有相同名称的表格列表
        /// </summary>
        /// <param name="name">表格标题</param>
        /// <returns>表格列表</returns>
        public List<TPTextTable> GetTableByName(string name)
        {
            ArrayList divAL = RootDocumentElement.ChildElements;
            List<TPTextTable> listTable = new List<TPTextTable>();

            foreach (ZYTextElement ele in divAL)
            {
                if (ele is TPTextTable)
                {
                    TPTextTable findTalbe = ele as TPTextTable;
                    if (findTalbe.Header == name)
                    {
                        listTable.Add(findTalbe);
                    }
                }
            }
            return listTable;
        }

        /// <summary>
        /// 获取文档中所有的表格集合
        /// </summary>
        /// <returns>表格集合</returns>
        public List<TPTextTable> GetAllTables()
        {
            ArrayList divAL = RootDocumentElement.ChildElements;
            List<TPTextTable> listTable = new List<TPTextTable>();

            foreach (ZYTextElement ele in divAL)
            {
                if (ele is TPTextTable)
                {
                    TPTextTable findTalbe = ele as TPTextTable;
                    listTable.Add(findTalbe);
                }
            }
            return listTable;
        }


        /// <summary>
        /// mfb 在光标处插入行
        /// </summary>
        /// <param name="RowNum">要插入行的数目</param>
        /// <param name="IsAfter">是否在光标所在行的后面插入</param>
        public void RowInsert(int RowNum, bool IsAfter)
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;
            myContent.InsertRows(RowNum, IsAfter);
            this.EndContentChangeLog();
            this.EndUpdate();
        }


        /// <summary>
        /// mfb 在光标处插入列
        /// </summary>
        /// <param name="columnNum">要插入列的数目</param>
        /// <param name="IsAfter">是否在光标所在列的后面插入</param>
        public void ColumnInsert(int columnNum, bool IsAfter)
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;
            myContent.InsertColumns(columnNum, IsAfter);
            this.EndContentChangeLog();
            this.EndUpdate();
        }

        /// <summary>
        /// mfb 删除插入点所在表格
        /// </summary>
        public void TableDelete()
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;
            myContent.DeleteTable();
            this.EndContentChangeLog();
            this.EndUpdate();
        }

        /// <summary>
        /// mfb 删除插入点所在列
        /// </summary>
        public void TableDeleteCol()
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;
            myContent.DeleteColumns();
            this.EndContentChangeLog();
            this.EndUpdate();
        }

        /// <summary>
        /// mfb 删除插入点所在行
        /// </summary>
        public void TableDeleteRow()
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;
            myContent.DeleteRows();
            this.EndContentChangeLog();
            this.EndUpdate();
        }

        /// <summary>
        /// TODO: 选定表格
        /// </summary>
        public void TableSelect()
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;
            //获取当前元素所在的表格
            TPTextTable currentTable = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Table) as TPTextTable;
            if (currentTable != null)
            {
                foreach (TPTextRow row in currentTable)
                {
                    foreach (TPTextCell cell in row)
                    {
                        cell.Selected = true;
                        OwnerControl.AddInvalidateRect(cell.GetContentBounds());
                    }
                }
            }

            this.EndContentChangeLog();
            this.EndUpdate();
        }

        /// <summary>
        /// TODO: 选定行
        /// </summary>
        public void TableSelectRow()
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;

            //获取当前元素所在的表格
            TPTextTable currentTable = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Table) as TPTextTable;

            if (currentTable != null)
            {
                bool isSelectRow = false;
                foreach (TPTextRow row in currentTable)
                {
                    isSelectRow = false;
                    foreach (TPTextCell cell in row)
                    {
                        if (cell.CanAccess == true)
                        {
                            isSelectRow = true;
                            break;
                        }
                    }
                    if (isSelectRow)
                    {
                        foreach (TPTextCell cell in row)
                        {
                            cell.Selected = true;
                            cell.CanAccess = true;
                            OwnerControl.AddInvalidateRect(cell.GetContentBounds());
                        }
                    }
                }
            }

            this.EndContentChangeLog();
            this.EndUpdate();
        }

        /// <summary>
        /// TODO: 选定列
        /// </summary>
        public void TableSelectCol()
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;

            //获取当前元素所在的表格
            TPTextTable currentTable = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Table) as TPTextTable;

            if (currentTable != null)
            {
                //用来标示被选中的cell所在的列.
                int[] colNum = null;

                // 如果所在的行中有被选中的cell, 则为true.
                bool isFind;

                foreach (TPTextRow row in currentTable)
                {
                    isFind = false;
                    colNum = new int[row.Cells.Count];
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (row[i].CanAccess == true)
                        {
                            colNum[i] = 1;
                            isFind = true;
                        }
                    }
                    if (isFind)
                    {
                        //只要找到第一行的就行了,剩下的其他行于第一行的情况相同.
                        //及时跳出,也可以减少循环次数
                        goto wefinded;
                    }
                }
            wefinded: // 根据有选中cell的记录, 遍历所有的行,将其中所属同样列号的cell选中
                foreach (TPTextRow row in currentTable)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (colNum[i] == 1 && colNum != null)
                        {
                            row[i].Selected = true;
                            row[i].CanAccess = true;
                            OwnerControl.AddInvalidateRect(row[i].GetContentBounds());
                        }
                    }
                }
            }

            this.EndContentChangeLog();
            this.EndUpdate();
        }

        /// <summary>
        /// 合并选中的单元格
        /// </summary>
        public void TableMergeCell()
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;

            //获取当前元素所在的表格
            TPTextTable currentTable = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Table) as TPTextTable;

            if (currentTable != null)
            {
                int colSpan = 0;
                int rowSpan = 0;

                //用来存储被选中的单元格所属的行号和列号
                int[][] needCells = new int[currentTable.IntRows][];

                //所有被选中的单元格,按先行后列的顺序排列
                List<TPTextCell> needMergeCells = new List<TPTextCell>();

                for (int k = 0; k < currentTable.AllRows.Count; k++)
                {
                    bool findRow = false;
                    needCells[k] = new int[currentTable.AllRows[k].Cells.Count];

                    for (int e = 0; e < currentTable.AllRows[k].Cells.Count; e++)
                    {
                        if (currentTable.AllRows[k].Cells[e].CanAccess == true)
                        {
                            findRow = true;
                            needCells[k][e] = 1;
                            needMergeCells.Add(currentTable.AllRows[k].Cells[e]);
                        }
                    }
                    //如果本行中有被选中的cell,则rowspan加一
                    if (findRow == true)
                    {
                        rowSpan += 1;
                    }
                }
                //用来存储所选cell所属的列号
                List<int> colNum = new List<int>();
                bool findit = false;
                for (int i = 0; i < needCells.Length; i++)
                {
                    for (int j = 0; j < needCells[i].Length; j++)
                    {
                        if (needCells[i][j] == 1)
                        {
                            findit = true;
                            colNum.Add(j);
                        }
                    }
                    if (findit)
                    {
                        //如果找到某一行则不用再找其他行了,因为每一行的列数都是一样的
                        //直接跳到外面,计算列数就行了.
                        goto setcolSpan;
                    }
                }
            setcolSpan:
                colSpan = colNum.Count;

                //合并后cell的高度
                int Mergeheight = 0;
                //合并后cell的宽度
                int Mergewidth = 0;

                //int leavePadding = 0;

                for (int index = 0; index < needMergeCells.Count; index++)
                {
                    if (index < colSpan)
                    {
                        Mergewidth += needMergeCells[index].Width;
                    }
                    if (index % colSpan == 0)
                    {
                        Mergeheight += needMergeCells[index].ContentHeight;
                    }
                }
                //重新对合并后的表格内发生改变的cell进行设置
                for (int index = 0; index < needMergeCells.Count; index++)
                {
                    TPTextCell cell = needMergeCells[index];
                    //发生合并的格子一定是第一个
                    if (0 == index)
                    {
                        cell.ContentHeight = Mergeheight + ((rowSpan - 1) * (cell.PaddingTop + cell.PaddingBottom));

                        cell.Width = Mergewidth;
                        cell.Colspan = colSpan;
                        cell.Rowspan = rowSpan;
                        //设置合并后的cell的边框和最后一个cell相同
                        cell.BorderWidthTop = needMergeCells[needMergeCells.Count - 1].BorderWidthTop;
                        cell.BorderWidthRight = needMergeCells[needMergeCells.Count - 1].BorderWidthRight;
                        cell.BorderWidthBottom = needMergeCells[needMergeCells.Count - 1].BorderWidthBottom;
                        cell.BorderWidthLeft = needMergeCells[needMergeCells.Count - 1].BorderWidthLeft;
                        //将其他格子里的ChildElements加入到新的合并cell里.
                        //TODO: 加入进来可以,但是鼠标好像不能定位. 还需要检查
                        //for (int i = 1; i < needMergeCells.Count; i++)
                        //{
                        //    cell.ChildElements.AddRange(needMergeCells[i].ChildElements);
                        //}
                    }
                    else
                    {
                        cell.ChildElements.Clear();
                        cell.ContentHeight = 0;
                        cell.ContentWidth = 0;
                        cell.Height = 0;
                        cell.Width = 0;
                        cell.Padding = 0;
                        cell.BorderWidth = 0;
                        cell.Merged = true;

                    }
                }
                //刷新整个表格状态
                foreach (TPTextRow row in currentTable)
                {
                    foreach (TPTextCell cell in row)
                    {
                        cell.Selected = false;
                        cell.CanAccess = false;
                        if (cell == needMergeCells[0])
                        {
                            cell.CanAccess = true;
                        }
                        OwnerControl.AddInvalidateRect(cell.GetContentBounds());
                    }
                }
            }
            this.RefreshSize();
            this.ContentChanged();

            this.EndContentChangeLog();
            this.EndUpdate();
        }


        /// <summary>
        /// 拆分某个单元格
        /// 从最后的结果来说,拆分也可以看做是合并.
        /// 所以拆分使用合并来做.
        /// </summary>
        /// <param name="row">要拆分的行数</param>
        /// <param name="column">要拆分的列数</param>
        public void TableSplitCell(int row, int column)
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;

            //获取当前元素所在的表格
            TPTextTable currentTable = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Table) as TPTextTable;

            if (currentTable != null)
            {
                //当前要拆分的cell
                TPTextCell specell = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Cell) as TPTextCell;
                //找到要拆分的cell所属的行
                TPTextRow specRow = specell.Parent as TPTextRow;

                //要拆分的cell在table中所在的行号
                int specellRowNum = currentTable.IndexOf(specRow);
                //要拆分的cell在table中所在的列号
                int specellColNum = specRow.IndexOf(specell);

                //要拆分的cell所占的行数
                int m = specell.Rowspan;
                //要拆分的cell所占的列数
                int n = specell.Colspan;

                //表格总行数
                int tableRowCount = currentTable.AllRows.Count;
                //表格总列数
                int tableColCount = currentTable.IntColumns;

                //拆分后,表格应该有的总行数
                int newRow = (tableRowCount + row) - m;
                if (row == 1)
                {
                    newRow = tableRowCount;
                }
                //拆分后,表格应该有的总列数
                int newCol = (tableColCount + column) - n;
                if (column == 1)
                {
                    newCol = tableColCount;
                }
                //for( int i = 0; i < column; i++)
                //    currentTable.InsertColumns(specellColNum, specell);

                currentTable.IntColumns = newCol; //设置表格为最新的列数
                int[] newWidths = new int[newCol]; //一个临时的int数组,用来存储最新的每列的宽度
                for (int k = 0; k < currentTable.Widths.Length; k++)
                {
                    if (k < n)
                    {
                        newWidths[k] = currentTable.Widths[k];
                    }
                    else if (k == n)
                    {
                        int tmpwidth = currentTable.Widths[k] / column;
                        for (int e = n; e < n + column; e++)
                        {
                            newWidths[e] = tmpwidth;
                            if (e == n + column - 1)
                            {
                                newWidths[e] = currentTable.Widths[k] - (tmpwidth * (column - 1));
                            }
                        }
                    }
                    else
                    {
                        newWidths[k + column - 1] = currentTable.Widths[k];
                    }
                }
                currentTable.RelativeWidths = new int[newWidths.Length];
                //将最新的列宽设置到表格中
                currentTable.SetWidth(newWidths);

                //设置每一个单元格的变化后的宽高度和合并情况
                for (int i = 0; i < currentTable.AllRows.Count; i++)
                {
                    TPTextRow tmpRow = currentTable.AllRows[i];

                    if (i == m)
                    {
                        for (int j = 0; j < tmpRow.Cells.Count; j++)
                        {
                            TPTextCell tmpCell = tmpRow.Cells[j];
                            if (j == n)
                            {
                                tmpCell.Colspan = column;
                                for (int k = 0; k < column; k++)
                                {

                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < tmpRow.Cells.Count; j++)
                        {
                            TPTextCell tmpCell = tmpRow.Cells[j];
                            if (j == n)
                            {
                                tmpCell.Colspan = column;
                                for (int k = 0; k < column; k++)
                                {

                                }
                            }
                        }
                    }

                }

            }


            this.RefreshSize();
            this.ContentChanged();
            this.EndContentChangeLog();
            this.EndUpdate();
        }


        /// <summary>
        /// 在拆分前,要确定当前拆分的格子最多能分成多少行,多少列.
        /// <para>为0则能分成无限行,如果大于0,则最多能拆分成返回的行数</para>
        /// <para></para>
        /// </summary>
        /// <returns>("row",0),("col",0)</returns>
        public Dictionary<string, int> TableBeforeSplitCell()
        {
            Dictionary<string, int> dic = null;
            //获取当前元素所在的表格
            TPTextTable currentTable = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Table) as TPTextTable;
            if (currentTable != null)
            {
                dic = new Dictionary<string, int>();
                TPTextCell cell = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Cell) as TPTextCell;
                int row = (cell.Rowspan > 1) ? cell.Rowspan : 1;
                int col = (cell.Colspan > 1) ? cell.Colspan : 1;
                dic.Add("row", row);
                dic.Add("col", col);
            }
            return dic;
        }

        /// <summary>
        /// 判断当前选中的格子是否可以合并 
        /// <para>1 正确,可以合并</para>
        /// <para>2 不在表格中</para>
        /// <para>3 没有选中任何单元格</para>
        /// <para>4 当前只有一个单元格</para>
        /// <para>5 选中的单元格不是正规的矩形状</para>
        /// </summary>
        /// <returns>int</returns>
        public int TableIsCanMerge()
        {
            TPTextTable currentTable = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Table) as TPTextTable;
            if (currentTable != null)
            {
                List<int> rowNum = new List<int>();
                List<int> colNum = new List<int>();

                for (int k = 0; k < currentTable.AllRows.Count; k++)
                {
                    bool find = false;
                    for (int e = 0; e < currentTable.AllRows[k].Cells.Count; e++)
                    {
                        if (currentTable.AllRows[k].Cells[e].CanAccess == true)
                        {
                            find = true;
                            colNum.Add(e);
                        }
                    }
                    if (find)
                    {
                        rowNum.Add(k);
                    }
                }
                if (rowNum.Count == 0 || colNum.Count == 0)
                {
                    return 3;
                }
                //只有一行一列.也就是只有一个单元格, 则不能合并
                if (rowNum.Count == 1 && colNum.Count == 1)
                {
                    return 4;
                }
                else
                {
                    //如果不为0
                    if (colNum.Count % rowNum.Count != 0)
                    {
                        return 5;
                    }
                }
                return 1;
            }
            return 2;
        }

        /// <summary>
        /// 判断当前选中的cell是否可以进行拆分
        /// </summary>
        /// <returns></returns>
        public bool TableIsCanSplit()
        {
            //TODO: 判断当前选中的cell是否可以进行拆分
            return true;
        }

        public void SetTableProperty()
        {
            //TODO: 设置表格的各种属性,并刷新整个表格
        }

        /// <summary>
        /// 判断cell在table中所处的位置,主要是为了设置边框.
        /// TODO: 貌似有点没用的方法,暂时留着吧
        /// <para>主要有四种情况:</para>
        /// <para>1 不在最后一行,也不在最后一列</para>
        /// <para>2 不在最后一行,但是在最后一列</para>
        /// <para>3 在最后一行,但是不在最后一列</para>
        /// <para>4 在最后一行,也在最后一列</para>
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        internal void SetCellBorderWidth(TPTextCell cell)
        {
            if (cell == null) return;
            TPTextRow row = cell.OwnerRow;
            TPTextTable table = row.OwnerTable;

            int columns = table.IntColumns;
            int rows = table.IntRows;

            //不在最后一行
            if (table.IndexOf(row) < (rows - 1))
            {
                //不在最后一列
                if (row.IndexOf(cell) < (columns - 1))
                {
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthLeft = 1;
                }
                else if (row.IndexOf(cell) == (columns - 1)) //在最后一列
                {
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthRight = 1;
                    cell.BorderWidthBottom = 0;
                    cell.BorderWidthLeft = 1;
                }
            }
            else if (table.IndexOf(row) == (rows - 1)) //在最后一行
            {
                //不在最后一列
                if (row.IndexOf(cell) < (columns - 1))
                {
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthRight = 0;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthLeft = 1;
                }
                else if (row.IndexOf(cell) == (columns - 1)) //在最后一列
                {
                    cell.BorderWidthTop = 1;
                    cell.BorderWidthRight = 1;
                    cell.BorderWidthBottom = 1;
                    cell.BorderWidthLeft = 1;
                }
            }

        }

        /// <summary>
        /// 获取当前表格
        /// </summary>
        /// <returns></returns>
        public TPTextTable GetCurrentTable()
        {
            return myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Table) as TPTextTable;

        }
        /// <summary>
        /// 获取当前单元格
        /// </summary>
        /// <returns></returns>
        public TPTextCell GetCurrentCell()
        {
            return myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Cell) as TPTextCell;
        }


        #endregion ▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅ 表格相关函数群 ▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅▅

        public void ResetNative()
        {
            myInfo.ShowAll = true;
            this.RefreshElements(true);
            this.RefreshLine();
            myContent.AutoClearSelection = true;
            myContent.MoveSelectStart(0);
            myContainters.Clear();
            //SetContainers(myDocumentElement);
            //myAllElement = null;
            this.UpdateView();
        }


        /// <summary>
        /// 重载 <link>ZYCommon.IEMRContentDocument.SelectionChanged</link>接口,文档选择区域发生改变时的处理
        /// </summary>
        /// <param name="s1">发生改变的第一个区域的开始序号</param>
        /// <param name="e1">发生改变的第一个区域的结束序号</param>
        /// <param name="s2">发生改变的第二个区域的开始序号</param>
        /// <param name="e2">发生改变的第二个区域的结束序号</param>
        public void SelectionChanged(int s1, int e1, int s2, int e2)
        {
            if (myOwnerControl != null)
            {
                //myOwnerControl.HidePopupList();
                ZYTextElement myElement = null;

                // 这种情况表示,点击了另外一个位置,但并未划选
                if (e1 == 0 && e2 == 0 && s1 != s2)
                {
                    if (s1 >= 0 && s1 < myElements.Count && s2 >= 0 && s2 < myElements.Count)
                    {
                        ZYTextElement OldElement = (ZYTextElement)myElements[s1];
                        ZYTextElement NewElement = (ZYTextElement)myElements[s2];
                        if (OnJumpDiv != null && ZYTextDiv.GetOwnerDiv(OldElement) != ZYTextDiv.GetOwnerDiv(NewElement))
                        {
                            OnJumpDiv(ZYTextDiv.GetOwnerDiv(OldElement), ZYTextDiv.GetOwnerDiv(NewElement));
                        }
                    }
                }
                //往前选
                if (s1 != s2)
                {
                    #region bwy:为了避免出错
                    if (s1 < 0) s1 = 0;
                    #endregion bwy:

                    for (int iCount = s1; iCount <= s2; iCount++)
                    {
                        myElement = (ZYTextElement)myElements[iCount];
                        if (myElement is ZYTextBlock)
                            myOwnerControl.AddInvalidateRect((myElement as ZYTextBlock).GetContentBounds());
                        else
                        {
                            try
                            {
                                myOwnerControl.AddInvalidateRect(
                                    myElement.RealLeft,
                                    myElement.OwnerLine.RealTop,
                                    myElement.Width + myElement.WidthFix,
                                    myElement.OwnerLine.Height);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("SelectionChanged()方法出错了:" + ex.Message);
                            }
                        }
                    }
                }
                //往后选
                if (e1 != e2)
                {
                    for (int iCount = e1; iCount <= e2; iCount++)
                    {
                        myElement = (ZYTextElement)myElements[iCount];
                        if (myElement is ZYTextBlock)
                            myOwnerControl.AddInvalidateRect((myElement as ZYTextBlock).GetContentBounds());
                        else
                        {
                            try
                            {
                                myOwnerControl.AddInvalidateRect(
                                    myElement.RealLeft,
                                    myElement.OwnerLine.RealTop,
                                    myElement.Width + myElement.WidthFix,
                                    myElement.OwnerLine.Height);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("SelectionChanged()方法出错了:" + ex.Message);
                            }
                        }
                    }
                }
                //myOwnerControl.UpdateInvalidateRect();
                //if( s1 != e1 || s2 != e2 )
                //	myOwnerControl.Refresh();
                myOwnerControl.UpdateInvalidateRect();
                myOwnerControl.UpdateTextCaret();
                if (this.OnSelectionChanged != null)
                    this.OnSelectionChanged(this, null);
            }
        }

        public bool bolLastContentChangedFlag = false;
        /// <summary>
        /// 重载 <link>ZYCommon.IEMRContentDocument.ContentChanged</link>接口,文档内容发生改变时的处理
        /// </summary>
        public void ContentChanged()
        {
            bolLastContentChangedFlag = true;
            this.Modified = true;
            this.RefreshElements();
            if (myElements.Count == 0)
            {
                RootDocumentElement.ClearChild();
                RootDocumentElement.RefreshSize();
                this.RefreshElements();
            }
            this.RefreshLine();
            myView.Height = RootDocumentElement.Top + RootDocumentElement.Height;
            if (this.OnContentChanged != null)
                this.OnContentChanged(this, null);
        }

        /// <summary>
        /// 判断文档中指定位置是否锁定
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsLock(int index)
        {
            if (this.Locked)
                return true;
            return myContent.IsLock(index);
        }

        /// <summary>
        /// 判断指定元素是否锁定
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool IsLock(ZYTextElement element)
        {
            if (this.Locked)
                return true;
            return myContent.IsLock(element);
        }

        /// <summary>
        /// 查找文档中的所有元素.为什么不返回一个al ，因为这个方法是递归的。
        /// </summary>
        /// <param name="al">返回的列表</param>
        /// <param name="element">根元素</param>
        /// <param name="type">类型</param>
        /// <param name="name">元素名称，可为null </param>
        public void GetAllSpecElement(ArrayList al, ZYTextContainer element, ElementType type, string name)
        {

            foreach (ZYTextElement textElement in element.ChildElements)
            {
                if (textElement is ZYTextContainer && !(textElement is ZYTextBlock))
                {
                    GetAllSpecElement(al, (ZYTextContainer)textElement, type, name);
                }
                else if (type == ElementType.All)
                {
                    if (textElement is ZYTextBlock || textElement is ZYElement)
                    {
                        if (name == null || (name != null && GetElementName(textElement) == name))
                        {
                            al.Add(textElement);
                        }
                    }
                }
                else if (type == textElement.Type && (name == null || (name != null && GetElementName(textElement) == name)))
                {

                    al.Add(textElement);
                    //al.
                }
            }
        }

        string GetElementName(ZYTextElement ele)
        {
            string name = null;
            if (ele is ZYTextBlock)
            {
                name = (ele as ZYTextBlock).Name;
            }
            if (ele is ZYElement)
            {
                name = (ele as ZYElement).Name;
            }
            return name;
        }
        /// <summary>
        /// 获得可替换项数据
        /// </summary>
        /// <returns></returns>
        public string GetReplaceText(string name)
        {
            ArrayList al = new ArrayList();
            string str = name;
            GetAllSpecElement(al, RootDocumentElement, ElementType.FixedText, name);

            #region Deleted By wwj 2013-08-05
            //GetAllSpecElement(al, RootDocumentElement, ElementType.Flag, name);
            #endregion

            if (al.Count > 0)
            {
                //目标元素
                ZYTextElement ele = al[0] as ZYTextElement;

                //元素所在段
                ZYTextParagraph p = ele.Parent as ZYTextParagraph;
                //todo add by zhouhui 解决宏替换的时候将按钮文字也替换过来！ 
                //待确认此功能是否会带来连带bug
                //str = p.ToEmrString2();
                //str = str.Trim();
                //原来是直接返回String字符串，现在改为返回字典和当前目标元素进行对比
                //add by ywk  2012年11月7日8:55:14 浦口大病历要连续书写
                Dictionary<string, string> myDic = p.ToEmrString2();
                foreach (var item in myDic.Keys)
                {
                    if (ele.ToEMRString().TrimStart(':').TrimStart('：') == item)
                    {
                        str = myDic[item].ToString();
                    }
                }


                //Modified By wwj 2012-04-18 复用项目数据的处理
                ////str = str.Substring(ele.ToEMRString().TrimStart().Length);

                //string eleStr = ele.ToEMRString().TrimStart();
                //str = str.Substring(str.IndexOf(eleStr) + eleStr.Length).TrimStart(':').TrimStart('：');

                //str = str.Trim();

                //如果是固定文本，那么替换整段
                if (ele is ZYFixedText)
                {
                    int index1 = str.IndexOf(':');
                    int index2 = str.IndexOf('：');

                    if (index1 == 0 || index2 == 0)
                    {
                        str = str.Substring(1);
                    }
                }

                #region Deleted By wwj 2013-08-05
                ////如果是标题定位符，替换它所在的一句话
                //if (ele is ZYFlag)
                //{
                //    str = str.Substring(str.IndexOf('§') + 1);
                //    int indexo = str.IndexOf('。');
                //    if (indexo >= 0)
                //    {
                //        str = str.Substring(0, indexo + 1);
                //    }
                //} 
                #endregion
            }
            return str;
        }

        #region 由于导出的文档结构不正确，暂时作废 Modified By wwj 2013-08-05
        ///// <summary>
        ///// 导出电子病历结构化文档
        ///// </summary>
        ///// <returns></returns>
        //public XmlDocument ToEMRXml()
        //{
        //    //获取所有元素列表
        //    XmlDocument doc = new XmlDocument();
        //    XmlElement root = doc.CreateElement("电子病历结构化文档");
        //    root.SetAttribute("level", "0");
        //    doc.AppendChild(root);

        //    XmlElement currentelement = root;

        //    //当前元素level
        //    int curlevel = 0;
        //    ArrayList al = new ArrayList();
        //    GetAllSpecElement(al, this.RootDocumentElement, ElementType.All, null);
        //    string name = "";

        //    ArrayList alparent = new ArrayList();
        //    ArrayList alinflag = new ArrayList();

        //    XmlElement ele = null;
        //    foreach (object o in al)
        //    {
        //        ele = null;

        //        //如果遇到了定位符，则查找它后一句话中的元素，如果元素在该范围内，插入；否则 ，插入它的父级
        //        if (o is ZYFlag)
        //        {
        //            name = (o as ZYFlag).Name;
        //            name = StringCommon.GetXmlElementName(name);
        //            ele = doc.CreateElement(name);
        //            ele.SetAttribute("flag", "1");
        //            ele.SetAttribute("code", (o as ZYFlag).Code);
        //            // alinflag 里边的元素是要在定位符范围内的
        //            alparent = (o as ZYTextElement).Parent.ChildElements;
        //            alinflag.Clear();
        //            int start = alparent.IndexOf(o);
        //            for (int i = start; i < alparent.Count; i++)
        //            {
        //                if (alparent[i] is ZYTextEOF || alparent[i] is ZYTextLineEnd)
        //                {
        //                    break;
        //                }
        //                if (alparent[i] is ZYTextChar)
        //                {
        //                    if ((alparent[i] as ZYTextChar).Char == '。')
        //                        break;
        //                }

        //                if (alparent[i] is ZYTextBlock)
        //                    alinflag.Add(alparent[i]);
        //            }
        //        }
        //        else if (o is ZYFixedText)
        //        {

        //            name = (o as ZYFixedText).Text;

        //            name = StringCommon.GetXmlElementName(name);

        //            ele = doc.CreateElement(name);
        //            ele.SetAttribute("code", (o as ZYFixedText).Code);

        //            if ((o as ZYFixedText).Level != null)
        //            {
        //                curlevel = (int)(o as ZYFixedText).Level;
        //                ele.SetAttribute("level", curlevel.ToString());
        //            }

        //        }
        //        else if (o is ZYTextBlock)
        //        {
        //            name = (o as ZYTextBlock).Name;
        //            name = StringCommon.GetXmlElementName(name);
        //            ele = doc.CreateElement(name);
        //            ele.SetAttribute("code", (o as ZYTextBlock).Code);



        //            //如果是单选、多选、有无选,记录选中的项目的值域代码
        //            if (o is ZYSelectableElement)
        //            {
        //                foreach (ZYSelectableElementItem item in (o as ZYSelectableElement).SelectList)
        //                {
        //                    if (item.IsSelected)
        //                    {
        //                        XmlElement itemele = doc.CreateElement("选项");
        //                        itemele.SetAttribute("code", item.Code);
        //                        itemele.InnerText = item.InnerValue;
        //                        ele.AppendChild(itemele);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                ele.InnerText = (o as ZYTextBlock).ToEMRString();
        //            }
        //        }

        //        else if (o is ZYElement)
        //        {
        //            name = (o as ZYElement).Name;
        //            name = StringCommon.GetXmlElementName(name);
        //            ele = doc.CreateElement(name);
        //            ele.SetAttribute("code", (o as ZYElement).Code);
        //            ele.InnerText = (o as ZYElement).ToEMRString();
        //        }

        //        if (ele != null)
        //        {
        //            if (ele.GetAttribute("level") == "")
        //            {
        //                if (currentelement.GetAttribute("flag") == "1")
        //                {
        //                    if (alinflag.Contains(o))
        //                    {

        //                    }
        //                    else
        //                    {
        //                        currentelement = currentelement.ParentNode as XmlElement;
        //                    }

        //                }
        //                currentelement.AppendChild(ele);


        //                //如果当前节点无级别，它不可能做为父节点
        //                if (o is ZYFlag)
        //                {
        //                    currentelement.AppendChild(ele);
        //                    currentelement = ele;
        //                }

        //                continue;
        //            }

        //            if (int.Parse(currentelement.GetAttribute("level")) < int.Parse(ele.GetAttribute("level")))
        //            {
        //                currentelement.AppendChild(ele);
        //                currentelement = ele;
        //                continue;
        //            }

        //            if (int.Parse(currentelement.GetAttribute("level")) >= int.Parse(ele.GetAttribute("level")))
        //            {
        //                //找到上一个和它同级元素的你节点
        //                XmlElement parent = null;
        //                parent = currentelement.ParentNode as XmlElement;
        //                while (parent != null)
        //                {

        //                    if (int.Parse(parent.GetAttribute("level")) < int.Parse(ele.GetAttribute("level")))
        //                    {
        //                        parent.AppendChild(ele);
        //                        currentelement = ele;
        //                        break;
        //                    }
        //                    parent = parent.ParentNode as XmlElement;
        //                }

        //                if (parent == null)
        //                {
        //                    root.AppendChild(ele);
        //                    currentelement = ele;
        //                }
        //            }
        //        }
        //    }
        //    return doc;
        //} 
        #endregion

        /// <summary>
        /// 获得文档大纲结构
        /// </summary>
        /// <returns></returns>
        public void GetBrief(TreeView tv)
        {
            tv.Nodes.Clear();
            TreeNode currentnode = null;
            ArrayList al = new ArrayList();
            GetAllSpecElement(al, this.RootDocumentElement, ElementType.FixedText, null);
            foreach (object o in al)
            {
                string name = (o as ZYFixedText).Text;
                TreeNode node;
                if ((o as ZYFixedText).Level == null)
                {
                    continue;
                }

                else
                {
                    int curlevel = (int)(o as ZYFixedText).Level;
                    node = new TreeNode(name);
                    string pre = "";
                    for (int i = 1; i < curlevel; i++)
                    {
                        pre += " ";
                    }
                    node.Text = pre + node.Text;

                    //用这个属性保存级别
                    node.ToolTipText = curlevel.ToString();
                    node.Tag = o;


                    if (tv.Nodes.Count == 0)
                    {
                        tv.Nodes.Add(node);
                        currentnode = node;
                        continue;
                    }
                }

                if ((o as ZYFixedText).FirstElement == this.Content.CurrentElement)
                {
                    tv.SelectedNode = node;
                }

                if (int.Parse(currentnode.ToolTipText) < int.Parse(node.ToolTipText))
                {
                    currentnode.Nodes.Add(node);
                    currentnode = node;
                    continue;
                }
                else
                {
                    //找到上一个和它同级元素的节点
                    TreeNode parent = currentnode.Parent;
                    if (parent != null)
                    {
                        if (int.Parse(parent.ToolTipText) < int.Parse(node.ToolTipText))
                        {
                            parent.Nodes.Add(node);
                            currentnode = node;
                            continue;
                        }
                    }

                    tv.Nodes.Add(node);
                    currentnode = node;
                }


            }
            tv.ExpandAll();

        }

        /// <summary>
        /// 是否固定行高
        /// </summary>
        public bool EnableFixedLineHeigh = false;

        int fixedLineHeigh = 80;
        /// <summary>
        /// 固定行高度，只有值>0时才有效，默认为80 
        /// </summary>
        public int FixedLineHeigh
        {
            get { return fixedLineHeigh; }
            set
            {
                if (value > 0)
                {
                    fixedLineHeigh = value;
                }
                else
                {
                    this.EnableFixedLineHeigh = false;
                }
            }
        }
        /// <summary>
        /// 插入模板文件，在当前段之后
        /// </summary>
        /// <param name="data"></param>
        public void InsertTemplateFile(byte[] data)
        {
            XmlDocument doc = this.OwnerControl.BinaryToXml(data);
            InsertTemplateFile(doc);
        }
        /// <summary>
        /// 插入模板文件，在当前段之后
        /// </summary>
        /// <param name="path"></param>
        public void InsertTemplateFile(string path)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            InsertTemplateFile(doc);

        }

        public void InsertTemplateByteFile(string path)
        {
            //读取byte[]文件

            System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Close();
            fs.Dispose();
            InsertTemplateFile(data);
        }
        /// <summary>
        /// 插入模板文件，在当前段之后
        /// </summary>
        /// <param name="doc"></param>
        public void InsertTemplateFile(XmlDocument doc)
        {
            //获得xml,转换成元素列表

            XmlNode node = doc.SelectSingleNode("document/body");
            ArrayList myList = new ArrayList();
            this.LoadElementsToList(node as XmlElement, myList);

            //当前元素(这里为一个eof符)
            ZYTextElement myElement = this.Content.CurrentElement;

            //当前元素所属的段落对象
            ZYTextElement Paraparent = myElement.Parent;
            while (!(Paraparent is ZYTextParagraph))
            {
                Paraparent = Paraparent.Parent;
            }

            //当前元素所属的容器的容器,(这里为div)
            ZYTextContainer divParent = (Paraparent as ZYTextParagraph).Parent;

            //将元素插入到文档中
            if (myList.Count > 0)
            {
                foreach (ZYTextElement myParagraph in myList)
                {
                    myParagraph.RefreshSize();
                    divParent.InsertAfter(myParagraph, Paraparent);
                    Paraparent = myParagraph as ZYTextParagraph;
                }


            }
            this.AutoClearSelection = true;
            this.ContentChanged();
            this.EndUpdate();

            this.Content.CurrentElement = (Paraparent as ZYTextParagraph).LastElement;
        }


        public void ReplaceTemplate(ElementType type, string tname)
        {
            //打开模板文件，获得xml,参考_past方法，插入到文档
            //调用事件

            XmlDocument doc = this.OwnerControl.FireGetOuterData(type, tname);

            if (doc != null)
            {
                this.BeginUpdate();
                this.BeginContentChangeLog();

                //删除当前选项
                this._Delete();
                InsertTemplate(doc);

                myContent.SelectLength = 0;
                this.EndContentChangeLog();
                this.EndUpdate();
            }
            else
            {
                MessageBox.Show("目标不存在");
            }


        }

        /// <summary>
        /// 插入模板在内容之中
        /// </summary>
        /// <param name="data"></param>
        public void InsertTemplate(byte[] data)
        {
            XmlDocument doc = this.OwnerControl.BinaryToXml(data);
            InsertTemplate(doc);
        }

        /// <summary>
        /// 插入病程专用
        /// 待测试
        /// </summary>
        /// <param name="data"></param>
        /// <param name="daily"></param>
        public void InsertDaliyTemplate(byte[] data)
        {
            XmlDocument doc = this.OwnerControl.BinaryToXml(data);
            InsetDaliyTemplate(doc);
        }

        public void InsetDaliyTemplate(XmlDocument doc)
        {
            try
            {
                XmlNode node = doc.SelectSingleNode("document/body");
                ArrayList myList = new ArrayList();
                this.LoadElementsToList(node as XmlElement, myList);

                for (int i = 0; i < myList.Count; i++)
                {
                    if (myList[i] is ZYTextEOF)
                    {
                        myList.Remove(myList[i]);
                    }
                }
                if (myList.Count > 0)
                {
                    foreach (ZYTextElement myElement in myList)
                    {
                        if (myElement is ZYTextContainer)
                            (myElement as ZYTextContainer).ClearSaveLog();
                        myElement.RefreshSize();
                    }

                    this.Content.InsertRangeElements(myList);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 插入模板在内容之中
        /// </summary>
        /// <param name="doc"></param>
        public void InsertTemplate(XmlDocument doc)
        {
            try
            {
                XmlNode node = doc.SelectSingleNode("document/body/p");
                ArrayList myList = new ArrayList();
                this.LoadElementsToList(node as XmlElement, myList);

                for (int i = 0; i < myList.Count; i++)
                {
                    if (myList[i] is ZYTextEOF)
                    {
                        myList.Remove(myList[i]);
                    }
                }
                if (myList.Count > 0)
                {
                    foreach (ZYTextElement myElement in myList)
                    {
                        if (myElement is ZYTextContainer)
                            (myElement as ZYTextContainer).ClearSaveLog();
                        myElement.RefreshSize();
                    }

                    this.Content.InsertRangeElements(myList);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool CanPase
        {
            get
            {
                string CopyPasteSetinout = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("CopyPasteSetinout");
                string copypasteset = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("CopyPasteSet");
                if (CopyPasteSetinout == "1")
                {
                    return true;
                }
                else
                {
                    if (copypasteset.Trim() == "0")
                    {
                        //设计状态可以粘贴
                        if (this.Info.DocumentModel == DocumentModel.Design)
                        {
                            return true;
                        }
                        //编辑状态只能从内部剪切板。 粘贴同一病人的数据，且应为文本
                        else if (this.Info.DocumentModel == DocumentModel.Edit && EmrClipboard.Data != null
                            && EmrClipboard.PatientID == this.Info.PatientID && EmrClipboard.PatientID != "")
                        /*&& EmrClipboard.PatientID == this.Info.PatientID && EmrClipboard.PatientID != ""   *********Modified by wwj 2012-07-24********* */
                        {
                            return true;
                        }
                        else if (this.Info.DocumentModel == DocumentModel.Read && EmrClipboard.Data != null
                            && EmrClipboard.PatientID == this.Info.PatientID && EmrClipboard.PatientID != ""
                            /*&& EmrClipboard.PatientID == this.Info.PatientID && EmrClipboard.PatientID != ""   *********Modified by wwj 2012-07-24********* */
                            && this.OwnerControl.ActiveEditArea != null)
                        {
                            int index = this.Content.IndexOf(this.Content.CurrentElement);
                            int start = this.Content.IndexOf(this.OwnerControl.ActiveEditArea.TopElement);
                            int end = this.Content.IndexOf(this.OwnerControl.ActiveEditArea.EndElement);
                            if (this.OwnerControl.ActiveEditArea.Top >= this.Content.CurrentElement.RealTop
                                || this.Content.CurrentElement.RealTop + this.Content.CurrentElement.Height <= this.OwnerControl.ActiveEditArea.End
                                )
                            {
                                return true;
                            }
                            return false;
                        }
                        else
                        {
                            //要检查patientID
                            return false;
                        }
                    }
                    else
                    {
                        //设计状态可以粘贴
                        if (this.Info.DocumentModel == DocumentModel.Design)
                        {
                            return true;
                        }
                        //编辑状态只能从内部剪切板。 粘贴数据，且应为文本
                        else if (this.Info.DocumentModel == DocumentModel.Edit && EmrClipboard.Data != null
                            /*  && EmrClipboard.PatientID == this.Info.PatientID && EmrClipboard.PatientID != ""*/)
                        /*&& EmrClipboard.PatientID == this.Info.PatientID && EmrClipboard.PatientID != ""   *********Modified by wwj 2012-07-24********* */
                        {
                            return true;
                        }
                        else if (this.Info.DocumentModel == DocumentModel.Read && EmrClipboard.Data != null
                            /* && EmrClipboard.PatientID == this.Info.PatientID && EmrClipboard.PatientID != ""*/
                            /*&& EmrClipboard.PatientID == this.Info.PatientID && EmrClipboard.PatientID != ""   *********Modified by wwj 2012-07-24********* */
                            && this.OwnerControl.ActiveEditArea != null)
                        {
                            int index = this.Content.IndexOf(this.Content.CurrentElement);
                            int start = this.Content.IndexOf(this.OwnerControl.ActiveEditArea.TopElement);
                            int end = this.Content.IndexOf(this.OwnerControl.ActiveEditArea.EndElement);
                            if (this.OwnerControl.ActiveEditArea.Top >= this.Content.CurrentElement.RealTop
                                || this.Content.CurrentElement.RealTop + this.Content.CurrentElement.Height <= this.OwnerControl.ActiveEditArea.End
                                )
                            {
                                return true;
                            }
                            return false;
                        }
                        else
                        {
                            //要检查patientID
                            return false;
                        }
                    }

                }
            }
        }

        public bool DeleteFlag = false;//false 不删除固定文本， true 即使是固定文本，照样删除，根据删除病程记录的需求原则问题而增加

        /// <summary>
        /// Add By wwj 2011-09-23 调用外部“查找”窗体
        /// </summary>
        public void ShowFindForm()
        {
            if (OnShowFindForm != null)
            {
                OnShowFindForm();
            }
        }

        public void SaveEMR()
        {
            if (OnSaveEMR != null)
            {
                OnSaveEMR();
            }
        }

        #region Add By wwj 2012-02-16 单元格中设置斜线

        /// <summary>
        /// 单元格中设置斜线
        /// </summary>
        /// <param name="italicLineStyle"></param>
        /// <returns></returns>
        public string SetItalicLineInCell(int italicLineStyle)
        {
            #region 找到选中的单元格
            //获取当前元素所在的表格
            TPTextTable currentTable = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Table) as TPTextTable;

            if (currentTable == null) return "请选中单元格";

            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;

            if (currentTable != null)
            {
                for (int k = 0; k < currentTable.AllRows.Count; k++)
                {
                    for (int e = 0; e < currentTable.AllRows[k].Cells.Count; e++)
                    {
                        if (currentTable.AllRows[k].Cells[e].CanAccess == true)
                        {
                            TPTextCell selectedCell = currentTable.AllRows[k].Cells[e];
                            #region 设置单元格的斜线
                            if (selectedCell != null)
                                //0:没有线 1:左上到右下的斜线 2:右上到左下的斜线 3:左上到右下的两条斜线
                                selectedCell.ItalicLineStyleInCell = (ItalicLineStyle)Enum.Parse(typeof(ItalicLineStyle), italicLineStyle.ToString());
                            #endregion
                        }
                    }
                }
            }

            this.RefreshSize();
            this.ContentChanged();
            this.EndContentChangeLog();
            this.EndUpdate();
            return string.Empty;
            #endregion
        }

        #endregion

        #region Add By wwj 2012-02-16 设置页眉和页脚的高度

        /// <summary>
        /// 设置页眉的高度
        /// </summary>
        /// <param name="height"></param>
        public int DocumentHeaderHeight
        {
            get
            {
                return intHeadHeight;
            }
            set
            {
                if (intHeadHeight != value)
                {
                    this.BeginUpdate();
                    this.BeginContentChangeLog();
                    myContent.AutoClearSelection = true;

                    intHeadHeight = value < 200 ? 200 : (value > 600 ? 600 : value);

                    this.RefreshSize();
                    this.ContentChanged();
                    this.EndContentChangeLog();
                    this.EndUpdate();
                }
            }
        }

        /// <summary>
        /// 设置页脚的高度
        /// </summary>
        /// <param name="height"></param>
        public int DocumentFooterHeight
        {
            get
            {
                return intFooterHeight;
            }
            set
            {
                if (intFooterHeight != value)
                {
                    this.BeginUpdate();
                    this.BeginContentChangeLog();
                    myContent.AutoClearSelection = true;

                    intFooterHeight = value < 20 ? 20 : (value > 200 ? 200 : value);

                    this.RefreshSize();
                    this.ContentChanged();
                    this.EndContentChangeLog();
                    this.EndUpdate();
                }
            }
        }

        #endregion

        #region Add By wwj 2012-02-21 重新计算并绘制界面元素

        /// <summary>
        /// 重新计算并绘制界面元素
        /// </summary>
        public void Refresh2()
        {
            this.BeginUpdate();
            this.BeginContentChangeLog();
            myContent.AutoClearSelection = true;

            this.RefreshSize();
            this.ContentChanged();
            this.EndContentChangeLog();
            this.EndUpdate();
        }

        #endregion

        /// <summary>
        /// 针对护理记录单的插入表格 Add By wwj 2012年5月
        /// </summary>
        public void TableInsertForNurseTable()
        {
            if (this.Info.DocumentModel == DocumentModel.Design || this.Info.DocumentModel == DocumentModel.Edit)
            {
                TPTextTable currentTable = myContent.GetParentByElement(myContent.CurrentElement, ZYTextConst.c_Table) as TPTextTable;
                if (currentTable == null)
                {
                    this.BeginUpdate();
                    if (myContent.HasSelected())
                        myContent.DeleteSeleciton();
                    else if (myOwnerControl.InsertMode == false)
                        myContent.DeleteCurrentElement();

                    myContent.AutoClearSelection = true;

                    TPTextTable table = myContent.GetFirstTable();
                    if (table != null)
                    {
                        myContent.InsertTable(table);
                    }


                    this.EndUpdate();
                }
            }
        }

        /// <summary>
        /// 设置单元格是否可以换行 Add By wwj 2012-06-06
        /// </summary>
        public void SetTableCellCanInsertEnter(bool canEnter)
        {
            this.Content.SetTableCellCanInsertEnter(canEnter);
        }
    }

}
