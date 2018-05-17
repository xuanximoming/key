using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DrectSoft.Core;
using DrectSoft.Wordbook;

namespace DrectSoft.Common.Library
{
    /// <summary>
    /// 处理代码显示、选择的组件
    /// </summary>
    [ToolboxBitmapAttribute(typeof(DrectSoft.Common.Library.LookUpWindow), "Images.ShowListWindow.ico")]
    public partial class LookUpWindow : Component, ISupportInitialize
    {
        #region properties
        /// <summary>
        /// 是否显示字典列表的列标题。默认显示
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("是否显示字典列表的列标题。默认显示"),
          DefaultValue(true)
        ]
        public bool ShowColumnTitle
        {
            get { return m_SelForm.ShowColumnTitle; }
            set { m_SelForm.ShowColumnTitle = value; }
        }

        /// <summary>
        /// 是否显示字典列表中的网格。默认显示
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("是否显示字典列表中的网格。默认显示"),
          DefaultValue(true)
        ]
        public bool ShowGridline
        {
            get { return m_SelForm.ShowGridline; }
            set { m_SelForm.ShowGridline = value; }
        }

        /// <summary>
        /// 是否习惯使用五笔代码
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("是否习惯使用五笔代码"),
          DefaultValue(false)
        ]
        public bool UseWB
        {
            get { return m_SelForm.UseWB; }
            set { m_SelForm.UseWB = value; }
        }

        /// <summary>
        /// 动态查询
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("动态查询"),
          DefaultValue(true)
        ]
        public bool IsDynamic
        {
            get { return m_SelForm.IsDynamic; }
            set { m_SelForm.IsDynamic = value; }
        }

        /// <summary>
        /// 查询匹配模式
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("查询匹配模式"),
          DefaultValue(ShowListMatchType.Begin)
        ]
        public ShowListMatchType MatchType
        {
            get { return m_SelForm.MatchType; }
            set { m_SelForm.MatchType = value; }
        }

        /// <summary>
        /// ShowList窗口的显示模式
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public ShowListFormMode FormMode
        {
            get { return m_SelForm.FormMode; }
        }

        /// <summary>
        /// 当前ShowList窗口处理的字典类实例
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public BaseWordbook Wordbook
        {
            get { return m_SelForm.Wordbook; }
        }

        /// <summary>
        /// 当前ShowList窗口处理的字典类类型
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public WordbookKind BookKind
        {
            get { return m_SelForm.BookKind; }
        }

        /// <summary>
        /// ShowList窗口是否显示阴影
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        public bool ShowShadow
        {
            get { return m_SelForm.ShowShadow; }
            set { m_SelForm.ShowShadow = value; }
        }

        /// <summary>
        /// 显示在编辑框中的值。多个名称时用“,”隔开
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public string DisplayValue
        {
            get { return m_SelForm.DisplayValue; }
        }

        /// <summary>
        /// 编辑框中的值对应的代码。多个代码时用“,”隔开
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public string CodeValue
        {
            get { return m_SelForm.CodeValue; }
        }

        /// <summary>
        /// 编辑框中的值对应的代码。多个代码时用“,”隔开
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public string QueryValue
        {
            get { return m_SelForm.QueryValue; }
        }

        /// <summary>
        /// 选中的数据记录行集合
        /// </summary>
        [
          Browsable(false),
          ReadOnly(true)
        ]
        public List<DataRow> ResultRows
        {
            get { return m_SelForm.ResultRows; }
        }

        /// <summary>
        /// 标记是否已经选择了值
        /// </summary>
        [Browsable(false)]
        public bool HadGetValue
        {
            get { return !String.IsNullOrEmpty(CodeValue); }
        }

        /// <summary>
        /// 是否总是显示ShowList窗口(默认只有一条匹配记录时不显示窗口)
        /// </summary>
        [Browsable(true), DefaultValue(false)]
        public bool AlwaysShowWindow
        {
            get { return m_SelForm.AlwaysShowWindow; }
            set { m_SelForm.AlwaysShowWindow = value; }
        }

        /// <summary>
        /// 执行SQL语句的DataAccess。必须在使用前初始化此属性
        /// </summary>
        [
          Browsable(false)
        ]
        public IDataAccess SqlHelper
        {
            get { return m_SelForm.SqlHelper; }
            set { m_SelForm.SqlHelper = value; }
        }

        /// <summary>
        /// 生成拼音、五笔缩写的对象。如果没有初始化，则将利用_sqlHelper创建新对象
        /// </summary>
        [Browsable(false)]
        public GenerateShortCode GenShortCode
        {
            get { return m_SelForm.GenShortCode; }
            set { m_SelForm.GenShortCode = value; }
        }

        /// <summary>
        /// 允许选择的最少记录数,最小为0
        /// </summary>
        [Browsable(false), DefaultValue(0)]
        public int MinCount
        {
            get { return m_SelForm.MinCount; }
            set { m_SelForm.MinCount = value; }
        }

        /// <summary>
        /// 允许选择的最大记录数
        /// </summary>
        [Browsable(false), DefaultValue(1)]
        public int MaxCount
        {
            get { return m_SelForm.MaxCount; }
            set { m_SelForm.MaxCount = value; }
        }

        /// <summary>
        /// 字体
        /// </summary>
        public Font Font
        {
            get { return m_SelForm.Font; }
            set { m_SelForm.Font = value; }
        }

        /// <summary>
        /// 获取或设置拥有此窗体的窗体
        /// </summary>
        [Browsable(false)]
        public Form Owner
        {
            get { return m_SelForm.Owner; }
            set { m_SelForm.Owner = value; }
        }
        #endregion

        #region private variables
        private DataSelectForm m_SelForm;
        #endregion

        #region ctors
        /// <summary>
        /// 
        /// </summary>
        public LookUpWindow()
        {
            if (!this.DesignMode)
            {
                m_SelForm = new DataSelectForm();
                InitializeComponent();
                Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public LookUpWindow(IContainer container)
            : this()
        {
            if (container == null)
                throw new ArgumentNullException();
            container.Add(this);
        }
        #endregion

        #region interface method
        /// <summary>
        /// 初始化ShowList控件时调用此函数
        /// </summary>
        /// <param name="wordbook">需要使用的字典类</param>
        /// <param name="kind">字典类类型</param>
        /// <param name="codeValue">初始化的代码值</param>
        public void CallLookUpWindow(BaseWordbook wordbook, WordbookKind kind, string codeValue)
        {
            CallLookUpWindow(wordbook, kind, codeValue
              , ShowListFormMode.Concision
              , new Point(0, 0)
              , new Size(100, 100)
              , new Rectangle(0, 0, 1000, 600)
              , ShowListCallType.Initialize);
        }

        /// <summary>
        /// 调用代码选择窗口。
        /// 初始化ShowList窗口的部分属性。这些属性因为与行为、显示效果等有关，所以需要统一设置。
        /// <param name="wordbook">默认的字典类</param>
        /// <param name="kind">字典类的类型</param>
        /// <param name="initText">查询条件初始值</param>
        /// <param name="formMode">ShowList窗口显示模式</param>
        /// <param name="initPosition">ShowListForm默认显示位置(屏幕坐标)</param>
        /// <param name="inputSize">ShowListForm输入框的尺寸</param>
        /// <param name="screenSize">调用ShowListForm的窗口所在屏幕的尺寸</param>
        /// </summary>
        public void CallLookUpWindow(BaseWordbook wordbook, WordbookKind kind, string initText, ShowListFormMode formMode, Point initPosition, Size inputSize, Rectangle screenSize)
        {
            CallLookUpWindow(wordbook, kind, initText, formMode, initPosition, inputSize, screenSize
              , ShowListCallType.Normal);
        }

        /// <summary>
        /// 调用代码选择窗口。
        /// 初始化ShowList窗口的部分属性。这些属性因为与行为、显示效果等有关，所以需要统一设置。
        /// <param name="wordbook">默认的字典类</param>
        /// <param name="kind">字典类的类型</param>
        /// <param name="initText">查询条件初始值</param>
        /// <param name="formMode">ShowList窗口显示模式</param>
        /// <param name="initPosition">ShowListForm默认显示位置(屏幕坐标)</param>
        /// <param name="inputSize">ShowListForm输入框的尺寸</param>
        /// <param name="screenSize">调用ShowListForm的窗口所在屏幕的尺寸</param>
        /// <param name="callType">调用模式</param>
        /// </summary>
        public void CallLookUpWindow(BaseWordbook wordbook, WordbookKind kind, string initText, ShowListFormMode formMode, Point initPosition, Size inputSize, Rectangle screenSize, ShowListCallType callType)
        {
            //m_SelForm.ClearTempData();

            //// 初始化时如果没有传入初始代码则直接退出
            //if ((callType == ShowListCallType.Initialize) && (String.IsNullOrEmpty(initText)))
            //   return;

            m_SelForm.CallShowListWindow(wordbook, kind, initText, formMode
               , initPosition, inputSize, screenSize, callType);
        }

        /// <summary>
        /// 检查指定的字典是否只包含一条记录。
        /// </summary>
        /// <param name="wordbook"></param>
        /// <param name="kind"></param>
        /// <returns>如果是的话则返回{代码,名称}对，否则返回空</returns>
        public string[] ValidateWordbookHasOneRecord(BaseWordbook wordbook, WordbookKind kind)
        {
            return m_SelForm.ValidateWordbookHasOneRecord(wordbook, kind);
        }
        #endregion

        #region ISupportInitialize 成员
        /// <summary>
        /// 
        /// </summary>
        public void BeginInit()
        { }

        /// <summary>
        /// 
        /// </summary>
        public void EndInit()
        { }

        #endregion
    }
}
