using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Core;
using DrectSoft.Wordbook;

namespace DrectSoft.Common.Library
{
    internal partial class DataSelectForm : CaptureCursorForm
    {
        #region readonly value defines
        /// <summary>
        /// 窗体的默认背景色。不能设为与System.Control一样的颜色
        /// </summary>
        private readonly Color ColorFormBack = Color.Silver;
        /// <summary>
        /// 窗体中子控件的默认背景色
        /// </summary>
        private readonly Color ColorChildBack = Color.WhiteSmoke;// .Gainsboro;
        /// <summary>
        /// 窗体中子控件的默认前景色
        /// </summary>
        private readonly Color ColorChildFore = SystemColors.ControlText;
        /// <summary>
        /// 阴影部分的背景色
        /// </summary>
        private readonly Color ColorShadowBack = SystemColors.ButtonShadow;
        /// <summary>
        /// 默认的阴影部分偏移量
        /// </summary>
        private readonly int DefaultShadowOffset = 5;
        /// <summary>
        /// 默认用来区分多选字符串的分隔符
        /// </summary>
        private readonly string[] MultiStringSeparators = new string[] { "、", ",", "，" };
        #endregion

        #region properties
        /// <summary>
        /// 是否显示字典列表的列标题。默认显示
        /// </summary>
        public bool ShowColumnTitle
        {
            get { return _showColumnTitle; }
            set { _showColumnTitle = value; }
        }
        private bool _showColumnTitle;

        /// <summary>
        /// 是否显示字典列表中的网格。默认显示
        /// </summary>
        public bool ShowGridline
        {
            get { return _showGridline; }
            set { _showGridline = value; }
        }
        private bool _showGridline;

        /// <summary>
        /// 是否习惯使用五笔代码
        /// </summary>
        public bool UseWB
        {
            get { return _useWB; }
            set { _useWB = value; }
        }
        private bool _useWB;

        /// <summary>
        /// 动态查询
        /// </summary>
        public bool IsDynamic
        {
            get { return _isDynamic; }
            set
            {
                _isDynamic = value;
                if (DesignMode)
                    return;
                SynchDynamicToControl();
            }
        }
        private bool _isDynamic;

        /// <summary>
        /// 查询匹配模式
        /// </summary>
        public ShowListMatchType MatchType
        {
            get { return _matchType; }
            set
            {
                ShowListMatchType tempType = _matchType;
                _matchType = value;
                if (DesignMode)
                    return;

                if (tempType != _matchType)
                {
                    if (Visible) // 窗口打开的情况下调整
                    {
                        SynchMatchTypeToControl();
                        SearchMatchRow(m_SearchText, _matchType, false);
                    }
                    else
                        m_NeedSynchMatchType = true;
                }
            }
        }
        private ShowListMatchType _matchType;

        /// <summary>
        /// ShowList窗口的显示模式
        /// </summary>
        public ShowListFormMode FormMode
        {
            get { return _formMode; }
            set
            {
                _formMode = value;
                if (value == ShowListFormMode.Full)
                    panelBottom.Height = m_DefBottomRegionHeight;
                else
                    panelBottom.Height = 0;
            }
        }
        private ShowListFormMode _formMode;

        ///// <summary>
        ///// 是否显示“确认”、“取消”按钮
        ///// </summary>
        //[
        //  Browsable(false),
        //  Category("ShowListConfig"),
        //  Description("是否显示“确认”、“取消”按钮"),
        //  ReadOnly(true)
        //]
        //public bool ShowFormButton
        //{
        //   get { return _showFormButton; }
        //   //set { _showFormButton = value; }
        //}
        //private bool _showFormButton;

        /// <summary>
        /// 当前ShowList窗口处理的字典类实例
        /// </summary>
        public BaseWordbook Wordbook
        {
            get { return _wordbook; }
            //set { _wordbook = value; }
        }
        private BaseWordbook _wordbook;

        /// <summary>
        /// 当前ShowList窗口处理的字典类类型
        /// </summary>
        public WordbookKind BookKind
        {
            get { return _bookKind; }
            //set { _wordbookKind = value; }
        }
        private WordbookKind _bookKind;

        /// <summary>
        /// ShowList窗口是否显示阴影
        /// </summary>
        public bool ShowShadow
        {
            get { return _showShadow; }
            set
            {
                _showShadow = value;
                if (_showShadow)
                    m_ShadowOffset = DefaultShadowOffset;
                else
                    m_ShadowOffset = 0;
            }
        }
        private bool _showShadow;

        /// <summary>
        /// 显示在编辑框中的值。多个名称时用“,”隔开
        /// </summary>
        public string DisplayValue
        {
            get { return _displayValue; }
            //set { _displayValue = value; }
        }
        private string _displayValue;

        /// <summary>
        /// 编辑框中的值对应的代码。多个代码时用“,”隔开
        /// </summary>
        public string CodeValue
        {
            get { return _codeValue; }
        }
        private string _codeValue;

        /// <summary>
        /// 编辑框中的值对应的代码。多个代码时用“,”隔开
        /// </summary>
        public string QueryValue
        {
            get { return _queryValue; }
        }
        private string _queryValue;

        /// <summary>
        /// 选中的数据记录行集合
        /// </summary>
        public List<DataRow> ResultRows
        {
            get
            {
                if (_resultRows == null)
                    _resultRows = new List<DataRow>();
                return _resultRows;
            }
        }
        private List<DataRow> _resultRows;

        /// <summary>
        /// 是否总是显示ShowList窗口(默认只有一条匹配记录时不显示窗口)
        /// </summary>
        public bool AlwaysShowWindow
        {
            get { return _alwaysShowWindow; }
            set { _alwaysShowWindow = value; }
        }
        private bool _alwaysShowWindow;

        /// <summary>
        /// 执行SQL语句的DataAccess。必须在使用前初始化此属性
        /// </summary>
        public IDataAccess SqlHelper
        {
            get { return _sqlHelper; }
            set { _sqlHelper = value; }
        }
        private IDataAccess _sqlHelper;

        /// <summary>
        /// 生成拼音、五笔缩写的对象。如果没有初始化，则将利用_sqlHelper创建新对象
        /// </summary>
        public GenerateShortCode GenShortCode
        {
            get
            {
                if (DesignMode)
                    return null;

                if (_genShortCode == null)
                {
                    if (_sqlHelper != null)
                        _genShortCode = new GenerateShortCode(_sqlHelper);
                    //else
                    //   throw new ArgumentNullException("生成拼音、五笔缩写的对象不存在");

                    return _genShortCode;
                }
                else
                    return _genShortCode;
            }
            set { _genShortCode = value; }
        }
        private GenerateShortCode _genShortCode;

        /// <summary>
        /// 允许多选
        /// </summary>
        public bool MultiSelect
        {
            get { return _multiSelect; }
            set
            {
                if (_multiSelect != value)
                {
                    _multiSelect = value;
                    // 根据是否多选设置界面

                    if (value)
                        panelMultiSelect.Visible = true;
                    else
                        panelMultiSelect.Visible = false;
                }
            }
        }
        private bool _multiSelect;

        /// <summary>
        /// 允许选择的最少记录数,最小为0
        /// </summary>
        public int MinCount
        {
            get { return _minCount; }
            set
            {
                if (value >= 0)
                    _minCount = value;
                else
                    _minCount = 0;
            }
        }
        private int _minCount;

        /// <summary>
        /// 允许选择的最大记录数
        /// </summary>
        public int MaxCount
        {
            get { return _maxCount; }
            set
            {
                if (value < MinCount)
                    _maxCount = MinCount;
                else
                    _maxCount = value;

                panelMultiSelect.Visible = (_maxCount > 1);
            }
        }
        private int _maxCount;

        /// <summary>
        /// 获取输入区域的宽度（包括阴影部分的宽度）
        /// </summary>
        private int InputRegionWidth
        {
            get { return panelTop.Width + m_ShadowOffset; }
        }

        /// <summary>
        /// 窗口有效区域的尺寸（含阴影部分）
        /// </summary>
        private Size ValiableRegionSize
        {
            get
            {
                int wMax = Math.Max(DataRegionWidth, InputRegionWidth);
                // 窗口尺寸要加上四周的阴影部分宽度
                return new Size(wMax + m_ShadowOffset * 2
                   , panelTop.Height + panelData.Height + panelBottom.Height + m_ShadowOffset * 2);
            }
        }

        //private EditorButton MenuButton
        //{
        //   get { return textInputor.Properties.Buttons[0]; }
        //}

        /// <summary>
        /// 数据选择区域的宽度（grid+多选结果框）,不含阴影
        /// </summary>
        private int DataRegionWidth
        {
            get
            {
                int regionWidth = SystemInformation.VerticalScrollBarWidth + 4; // 默认保留滚动条的宽度
                // 获得Grid的理想宽度(所有列宽度的集合)
                foreach (GridColumn col in gridViewbook.Columns)
                    regionWidth += col.Width;
                // 已支持多选，所以还要考虑加上多选结果区域的宽度
                if (_maxCount > 1)
                    regionWidth += panelMultiSelect.Width;

                int base2Right = m_ScreenSize.Width - m_InitPosition.X; //输入控件左边界到右边屏幕的距离
                int base2Left = m_InitPosition.X + panelTop.Width; // 输入控件右边界到左边屏幕的距离

                // 数据区域的宽度要满足以下限制
                if (regionWidth > Math.Max(base2Left, base2Right))
                    regionWidth = Math.Max(base2Left, base2Right);
                return regionWidth;
            }
        }
        #endregion

        #region 和控件布局、尺寸有关的私有变量
        /// <summary>
        /// ShowListForm 显示时的初始位置（屏幕坐标）
        /// </summary>
        private Point m_InitPosition;
        /// <summary>
        /// ShowListForm 输入框的尺寸
        /// </summary>
        private Size m_InputControlSize;
        /// <summary>
        /// 调用ShowListForm的窗口所在屏幕的尺寸
        /// </summary>
        private Rectangle m_ScreenSize;
        /// <summary>
        /// 标记窗口是否从左到右排列控件
        /// </summary>
        private bool m_Left2Right;
        /// <summary>
        /// 标记窗口是否从上到下排列控件
        /// </summary>
        private bool m_Top2Down;
        /// <summary>
        /// 阴影部分的偏移量
        /// </summary>
        private int m_ShadowOffset;
        /// <summary>
        /// 数据选择部分的默认高度（包括按钮区域的默认高度）
        /// </summary>
        private int m_DefDataRegionHeight;
        /// <summary>
        /// 按钮区域的默认高度
        /// </summary>
        private int m_DefBottomRegionHeight;
        #endregion

        #region //popupmenu
        //private BarDockControl barDockControlTop;
        //private BarDockControl barDockControlBottom;
        //private BarDockControl barDockControlLeft;
        //private BarDockControl barDockControlRight;
        //private BarCheckItem barItemIsDynamic;
        //private BarCheckItem barItemMatchFull;
        //private BarCheckItem barItemMatchBegin;
        //private BarCheckItem barItemMatchAny;
        //private BarManagerCategory[] barManagerCategory;
        //private PopupMenu popupSettingMenu;

        //private void InitializeBasicPopupmenuComponets()
        //{
        //   barItemIsDynamic = new BarCheckItem();
        //   barItemMatchFull = new BarCheckItem();
        //   barItemMatchBegin = new BarCheckItem();
        //   barItemMatchAny = new BarCheckItem();
        //   popupSettingMenu = new PopupMenu();
        //   barManagerCategory = new BarManagerCategory[] {
        //      new BarManagerCategory("查询方式", new Guid("4c9d2035-2e11-479e-a0c5-4fc93d06733d")),
        //      new BarManagerCategory("匹配方式", new Guid("07c5978d-2571-4c99-878e-6fd268d262d2"))};

        //   barItemIsDynamic.Caption = "动态";
        //   barItemIsDynamic.CategoryGuid = new Guid("4c9d2035-2e11-479e-a0c5-4fc93d06733d");
        //   barItemIsDynamic.Checked = true;
        //   barItemIsDynamic.Id = 0;
        //   barItemIsDynamic.Name = "barItemIsDynamic";
        //   barItemIsDynamic.CheckedChanged += new ItemClickEventHandler(barItemIsDynamic_CheckedChanged);

        //   barItemMatchFull.Caption = "完全匹配";
        //   barItemMatchFull.CategoryGuid = new Guid("07c5978d-2571-4c99-878e-6fd268d262d2");
        //   barItemMatchFull.GroupIndex = 1;
        //   barItemMatchFull.Id = 1;
        //   barItemMatchFull.Name = "barItemMatchFull";
        //   barItemMatchFull.CheckedChanged += new ItemClickEventHandler(barItemMatchFull_CheckedChanged);

        //   barItemMatchBegin.Caption = "前导相似";
        //   barItemMatchBegin.CategoryGuid = new Guid("07c5978d-2571-4c99-878e-6fd268d262d2");
        //   barItemMatchBegin.Checked = true;
        //   barItemMatchBegin.GroupIndex = 1;
        //   barItemMatchBegin.Id = 2;
        //   barItemMatchBegin.Name = "barItemMatchBegin";
        //   barItemMatchBegin.CheckedChanged += new ItemClickEventHandler(barItemMatchBegin_CheckedChanged);

        //   barItemMatchAny.Caption = "部分包含";
        //   barItemMatchAny.CategoryGuid = new System.Guid("07c5978d-2571-4c99-878e-6fd268d262d2");
        //   barItemMatchAny.GroupIndex = 1;
        //   barItemMatchAny.Id = 6;
        //   barItemMatchAny.Name = "barItemMatchAny";
        //   barItemMatchAny.CheckedChanged += new ItemClickEventHandler(barItemMatchAny_CheckedChanged);

        //   popupSettingMenu.Name = "popupSettingMenu";
        //}

        //private void InitializePopupMenuBeforCall(BarManager newBarManager)
        //{
        //   newBarManager.BeginInit();

        //   newBarManager.Categories.AddRange(barManagerCategory);
        //   newBarManager.Form = this;
        //   newBarManager.Items.AddRange(new BarItem[] {
        //      barItemIsDynamic,
        //      barItemMatchFull,
        //      barItemMatchBegin,
        //      barItemMatchAny});
        //   newBarManager.MaxItemId = 7;

        //   popupSettingMenu.BeginInit();
        //   popupSettingMenu.ClearLinks();
        //   popupSettingMenu.Manager = newBarManager;
        //   popupSettingMenu.LinksPersistInfo.AddRange(new LinkPersistInfo[] {
        //      new LinkPersistInfo(BarLinkUserDefines.PaintStyle, barItemIsDynamic, BarItemPaintStyle.CaptionGlyph),
        //      new LinkPersistInfo(barItemMatchFull, true),
        //      new LinkPersistInfo(barItemMatchBegin),
        //      new LinkPersistInfo(barItemMatchAny)});
        //   popupSettingMenu.EndInit();

        //   newBarManager.ForceLinkCreate();
        //   newBarManager.EndInit();
        //}
        #endregion

        #region private variables
        /// <summary>
        /// 用来缓存使用过的字典数据
        /// </summary>
        private DataSet m_CacheDataSet;
        /// <summary>
        /// 当前使用的字典的DataTable
        /// </summary>
        private DataTable m_DefaultTable;
        /// <summary>
        /// 当前DataTable的默认DataView
        /// </summary>
        private DataView m_DefaultView;
        private string m_DefaultRowFilter;
        /// <summary>
        /// 标记输入框中的文本改变后是否进行过查询。以决定回车等事件如何处理
        /// </summary>
        private bool m_HaveSearch;
        /// <summary>
        /// 用来在字典数据中进行查询的文本
        /// </summary>
        private string m_SearchText;

        /// <summary>
        /// 保存DataGridView中选中的行的序号（处理多选时需要）
        /// </summary>
        private int m_SelectedRowIndex;
        /// <summary>
        /// 需要在Grid显示时定位的行号
        /// </summary>
        private int m_LocateIndex;
        /// <summary>
        /// 标记是否处于初始化状态
        /// </summary>
        private bool m_Initializing;
        private bool m_ParentAutoClose;
        private bool m_NeedSynchMatchType; // 标记是否需要同步匹配模式到界面上
        #endregion

        #region ctor
        public DataSelectForm()
        {
            InitializeComponent();
            InitializeProperties();
            InitializePrivateVariable();
        }
        #endregion

        #region public methods
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
        public void CallShowListWindow(BaseWordbook wordbook, WordbookKind kind, string initText, ShowListFormMode formMode, Point initPosition, Size inputSize, Rectangle screenSize, ShowListCallType callType)
        {
            //StringBuilder timeLog = new StringBuilder();
            //timeLog.AppendLine(wordbook.Caption + " " + initText);
            //timeLog.AppendLine(String.Format("{0,20} {1}", "begin", DateTime.Now.ToString("mm:ss fff")));

            // 初始化前要清除上次的查询结果
            ClearLastData();
            m_Initializing = true;

            // 初始化时如果没有传入初始代码则直接退出
            if ((callType == ShowListCallType.Initialize) && (String.IsNullOrEmpty(initText)))
                return;

            if (wordbook == null)
                throw new ArgumentNullException("未初始化字典类");

            if (m_NeedSynchMatchType)
            {
                m_NeedSynchMatchType = false;
                SynchMatchTypeToControl();
            }

            _wordbook = wordbook;
            _bookKind = kind;

            // ShowList窗口的显示模式(使用List字典时，默认使用简洁窗口)
            if (_bookKind == WordbookKind.List)
                FormMode = ShowListFormMode.Concision;
            else if (_maxCount > 1)
                FormMode = ShowListFormMode.Full;
            else
                FormMode = formMode;

            // 首先检查属性配置是否正确
            string errMsgs = CheckFormProperties(true);
            if (errMsgs.Length > 0)
            {
                CancleSelectAction();
                throw new ArgumentException(errMsgs);
            }

            //timeLog.AppendLine(String.Format("{0,20} {1}", "begininitidata", DateTime.Now.ToString("mm:ss fff")));

            // 初始化字典数据
            InitializeWordbookData();

            //timeLog.AppendLine(String.Format("{0,20} {1}", "endinitdata", DateTime.Now.ToString("mm:ss fff")));

            m_InitPosition = initPosition;
            m_InputControlSize = inputSize;
            m_ScreenSize = screenSize;
            m_SearchText = initText.Trim();

            // 先初始化DataGridView（因为窗口的宽度与DataGrid中列的宽度有关）
            InitializeDataGridView();

            //timeLog.AppendLine(String.Format("{0,20} {1}", "dosearch", DateTime.Now.ToString("mm:ss fff")));

            if (_maxCount > 1)
                DoMultiSelectInitialize(m_SearchText, callType);
            else
                DoSingleSelectInitialize(m_SearchText, callType);

            //timeLog.AppendLine(String.Format("{0,20} {1}", "end", DateTime.Now.ToString("mm:ss fff")));
            //Trace.WriteLine(timeLog.ToString());
        }

        public string[] ValidateWordbookHasOneRecord(BaseWordbook wordbook, WordbookKind kind)
        {
            if (wordbook == null)
                return null;

            _wordbook = wordbook;
            _bookKind = kind;
            InitializeWordbookData();
            if (m_DefaultView.Count == 1)
                return new string[] { m_DefaultView[0][_wordbook.CodeField].ToString()
               , m_DefaultView[0][_wordbook.NameField].ToString()};
            else
                return null;
        }
        #endregion

        #region private common methods
        /// <summary>
        /// 初始化属性
        /// </summary>
        private void InitializeProperties()
        {
            _showColumnTitle = true;
            _showGridline = true;
            //_useWb = false;
            _isDynamic = true;
            panelSetting.Hide();
            _matchType = ShowListMatchType.Any;
            _formMode = ShowListFormMode.Full;
            ShowShadow = false;
            //_showFormButton = false;
            _bookKind = WordbookKind.Normal;

            _displayValue = "";
            _codeValue = "";
            _queryValue = "";
            _sqlHelper = null;
            _genShortCode = null;
            MinCount = 0;
            MaxCount = 1;

            BackColor = ColorFormBack;
            panelTop.BackColor = ColorChildBack;
            panelData.BackColor = ColorChildBack;
            panelTop.ForeColor = ColorChildFore;
            panelData.ForeColor = ColorChildFore;
            panelTopShadow.BackColor = ColorShadowBack;
            panelDataShadow.BackColor = ColorShadowBack;

            textInputor.ContextMenu = new ContextMenu();
            textInputor.MouseWheel += new MouseEventHandler(inputBox_MouseWheel);

            //InitializeBasicPopupmenuComponets();
        }

        /// <summary>
        /// 初始化私有变量
        /// </summary>
        private void InitializePrivateVariable()
        {
            m_CacheDataSet = new DataSet();
            m_CacheDataSet.Locale = CultureInfo.CurrentCulture;
            m_HaveSearch = false;
            m_SearchText = "";


            //m_MatchStringFormat = "{0} like '{1}%'";
            //m_MatchNumericFormat = "{0} = {1}";
            m_SelectedRowIndex = -1;
            m_Left2Right = true;
            m_Top2Down = true;
            m_DefDataRegionHeight = 238;
            m_DefBottomRegionHeight = 30;
            m_LocateIndex = -1;
            m_NeedSynchMatchType = false;
        }

        /// <summary>
        /// 检查窗口的属性设置是否正确
        /// </summary>
        /// <param name="fullCheck">是否进行完整检查标志</param>
        /// <returns>返回错误信息。为空表示没有错误</returns>
        private string CheckFormProperties(bool fullCheck)
        {
            StringBuilder msgs = new StringBuilder();

            if (_wordbook == null)
            {
                msgs.Append("未指定字典类\r\n");
            }
            if ((_wordbook is ListWordbook) && (_bookKind != WordbookKind.List))
            {
                msgs.Append("字典类的实际类型与类型属性不匹配\r\n");
            }
            if ((_wordbook is SqlWordbook) && (_bookKind != WordbookKind.Sql))
            {
                msgs.Append("字典类的实际类型与类型属性不匹配\r\n");
            }
            if (!fullCheck)
                return msgs.ToString();

            if ((_bookKind == WordbookKind.List) && (_formMode != ShowListFormMode.Concision))
            {
                msgs.Append("使用List型字典时没有应用简洁模式窗口");
            }

            return msgs.ToString();
        }

        /// <summary>
        /// 初始化DataGridView
        /// </summary>
        private void InitializeDataGridView()
        {
            gridControl1.BeginUpdate();
            gridViewbook.BeginUpdate();
            // Grid是否显示Line属性
            gridViewbook.OptionsView.ShowVertLines = _showGridline;
            gridViewbook.OptionsView.ShowVertLines = _showGridline;

            // 设置数据源
            gridControl1.DataSource = null;
            // 创建列（如果两次的列一样则跳过）
            GridColumn[] newCols = _wordbook.GenerateDevGridColumnCollection();
            bool containsAll = (gridViewbook.Columns.Count == newCols.Length);
            if (containsAll)
            {
                foreach (GridColumn col in newCols)
                {
                    if (!gridViewbook.Columns.Contains(col))
                    {
                        containsAll = false;
                        break;
                    }
                }
            }
            if (!containsAll)
            {
                gridViewbook.Columns.Clear();
                ////gridViewbook.Columns.AddRange(_wordbook.GenerateDevGridColumnCollection());

            }

            gridControl1.DataSource = m_DefaultTable;
            //// 直接定位到指定行
            //if (m_LocateIndex > -1)
            //{
            //   gridViewbook.FocusedRowHandle = m_LocateIndex;
            //   // 设置聚焦的行后GridView没有自动滚（原因不明！！！），所以用下面的方法将聚焦的行显示到屏幕上
            //   gridViewbook.TopRowIndex = m_LocateIndex - 3;
            //   m_LocateIndex = -1;
            //}
            gridViewbook.EndUpdate();
            gridControl1.EndUpdate();
        }
        #endregion

        #region private methods of handle select
        private void DoMultiSelectInitialize(string searchText, ShowListCallType callType)
        {
            // 如果是多选
            //    先分解传入的字符串，匹配到对应记录，添加到List中
            //    若是初始化ShowListBox时调用则不显示Form，否则强制显示Form
            string[] texts = SplitSearchText(searchText);
            foreach (string t in texts)
            {
                if (String.IsNullOrEmpty(t))
                    continue;

                if (SearchMatchRow(t, ShowListMatchType.Full, (callType == ShowListCallType.Initialize), true) > 0)
                {
                    if (m_LocateIndex >= 0)
                        DoAfterRowHadBeenSelected(); // 多选时的处理是不同的
                }
            }
            if (callType == ShowListCallType.Initialize)
                CommitSelectAction();
            else
            {
                textInputor.Text = "";
                //inputBox_TextChanged(this, new EventArgs());
                m_Initializing = false;
                gridViewbook.MoveFirst();
                if (gridViewbook.FocusedRowHandle >= 0)
                    m_SelectedRowIndex = gridViewbook.FocusedRowHandle;
                else
                    m_SelectedRowIndex = -1;
                //if (Visible)
                //   Visible = false;
                //try
                {
                    ShowDialog();
                }
                //catch
                //{
                //   Show();
                //}
            }
        }

        private string[] SplitSearchText(string searchText)
        {
            if (String.IsNullOrEmpty(searchText))
                return new string[] { };

            if (searchText.Contains("，"))
                searchText = searchText.Replace("，", ","); // 全角的逗号要替换成半角的

            string[] separators = MultiStringSeparators;
            return searchText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        private void DoSingleSelectInitialize(string searchText, ShowListCallType callType)
        {
            // 如果是单选
            //    初始化时调用，则查找匹配记录，不显示Form
            //    强制显示Form时，只定位记录，不做其它处理
            //    其它情况下，过滤数据，如果只有一条匹配，则不显示Form
            if (callType == ShowListCallType.Initialize)
            {
                // 单选的初始化过程：直接匹配记录，最后判断是否有结果
                if (SearchMatchRow(searchText, ShowListMatchType.Full, true) > 0)
                    DoAfterRowHadBeenSelected();
            }
            else
            {
                int rowCount;
                if (callType == ShowListCallType.Normal)
                    rowCount = SearchMatchRow(searchText, MatchType, false);
                else// 强制显示窗口时，只查找、不过滤数据
                    rowCount = SearchMatchRow(searchText, MatchType, false, true);

                if ((!AlwaysShowWindow) && (rowCount == 1))
                {
                    DoAfterRowHadBeenSelected();
                }
                else
                {
                    textInputor.TextChanged -= new EventHandler(inputBox_TextChanged);
                    //if (rowCount > 0)
                    //   textInputor.Text = "";
                    //else
                    textInputor.Text = searchText.Trim();
                    textInputor.TextChanged += new EventHandler(inputBox_TextChanged);
                    m_Initializing = false;
                    //if (Visible)
                    //   Visible = false;
                    //try
                    {
                        ShowDialog();
                    }
                    //catch
                    //{
                    //   Show();
                    //}
                }
            }
        }

        /// <summary>
        /// 清除上次查询结果
        /// </summary>
        private void ClearLastData()
        {
            m_HaveSearch = false;
            //m_SelectedRowIndexes.Clear();
            if (_resultRows == null)
                _resultRows = new List<DataRow>();
            else
                _resultRows.Clear();
            _codeValue = "";
            _displayValue = "";
            _queryValue = "";
            m_SearchText = "";
            lbSelectedRecords.Items.Clear();
        }

        /// <summary>
        /// 初始化现在要使用的字典数据
        /// </summary>
        private void InitializeWordbookData()
        {
            // --首先在数据缓存中检查, 没有的话再从数据库读取
            // 数据缓存现在移到框架中，由框架决定如何取数据
            bool cached = (_wordbook.CacheTime != -1); // 根据字典的缓存时间设置决定是否使用缓存数据
            m_DefaultRowFilter = _wordbook.GenerateFilterExpression();

            switch (_bookKind)
            {
                case WordbookKind.Normal:
                    if (cached)
                    {
                        m_DefaultTable = _sqlHelper.ExecuteDataTable(_wordbook.QuerySentence
                           , m_DefaultRowFilter);
                        m_DefaultRowFilter = "";
                    }
                    else
                    {
                        m_DefaultTable = _sqlHelper.ExecuteDataTable(_wordbook.QuerySentence
                           , cached, CommandType.Text);
                    }
                    break;
                case WordbookKind.List:
                    // 对于List类型的字典，不通过框架取数据，仍然使用自己的缓存
                    if (m_CacheDataSet.Tables.Contains(_wordbook.WordbookName))
                        m_DefaultTable = m_CacheDataSet.Tables[_wordbook.WordbookName];
                    else
                        m_DefaultTable = ConvertStringList2DataTable(_wordbook.WordbookName
                                                 , (_wordbook as ListWordbook).Items);
                    break;
                case WordbookKind.Sql:
                    SqlWordbook sqlBook = _wordbook as SqlWordbook;
                    sqlBook.EnsureBookData(SqlHelper, GenShortCode);
                    m_DefaultTable = sqlBook.BookData;
                    //// 判断是否需要使用Sql语句创建数据集
                    //if (sqlBook.UseSqlStatement)
                    //   m_DefaultTable = _sqlHelper.ExecuteDataTable(_wordbook.QuerySentence
                    //      , cached, CommandType.Text);
                    //else
                    //   m_DefaultTable = ResetSqlbookData(sqlBook);
                    break;
                default:
                    throw new ArgumentException("未处理的字典类型");
            }

            m_DefaultView = m_DefaultTable.DefaultView;
            m_DefaultView.RowFilter = m_DefaultRowFilter;// _wordbook.GenerateFilterExpression(); // 取字典的默认过虑条件
        }

        /// <summary>
        /// 根据传入的StringList生成DataTable。自动添加序号、拼音、五笔列
        /// </summary>
        /// <param name="tableName">指定的DataTable名称</param>
        /// <param name="source">用来生成DataTable的StringList</param>
        /// <returns></returns>
        private DataTable ConvertStringList2DataTable(string tableName, Collection<string> source)
        {
            DataTable newTable = new DataTable(tableName);
            newTable.Locale = CultureInfo.CurrentCulture;
            newTable.Columns.AddRange(new DataColumn[] { 
              new DataColumn("xh", Type.GetType("System.Int32"))
            , new DataColumn("name", Type.GetType("System.String"))
            , new DataColumn("py", Type.GetType("System.String"))
            , new DataColumn("wb", Type.GetType("System.String")) });

            DataRow newRow;
            string[] shortCode;
            for (int i = 0; i < source.Count; i++)
            {
                newRow = newTable.NewRow();
                newRow["xh"] = i;
                newRow["name"] = source[i];

                shortCode = GenShortCode.GenerateStringShortCode(source[i]);
                newRow["py"] = shortCode[0];
                newRow["wb"] = shortCode[1];
                newTable.Rows.Add(newRow);
            }

            return newTable;
        }

        /// <summary>
        /// 根据当前的查询文本查找数据（即过滤数据）
        /// </summary>
        /// <param name="searchText">查询数据的条件值</param>
        /// <param name="matchType">数据匹配模式</param>
        /// <param name="useCodeFieldOnly">是否直接使用默认的代码列进行匹配</param>
        /// <returns>返回匹配的记录行数</returns>
        private int SearchMatchRow(string searchText, ShowListMatchType matchType, bool useCodeFieldOnly)
        {
            int rowCount = SearchMatchRow(searchText, matchType, useCodeFieldOnly, false);
            gridViewbook.MoveFirst(); // 
            return rowCount;
        }

        /// <summary>
        /// 根据当前的查询文本查找数据（即过滤数据）
        /// </summary>
        /// <param name="searchText">查询数据的条件值</param>
        /// <param name="matchType">数据匹配模式</param>
        /// <param name="useCodeFieldOnly">是否直接使用默认的代码列进行匹配</param>
        /// <param name="findOnly">是否只查找数据，不过滤</param>
        /// <returns>返回匹配的记录行数</returns>
        private int SearchMatchRow(string searchText, ShowListMatchType matchType, bool useCodeFieldOnly, bool findOnly)
        {
            m_HaveSearch = true;
            m_LocateIndex = -1;

            // 首先根据输入内容的性质设置首选的查询列名
            StringType inputType = CommonOperation.GetStringType(searchText);
            if (inputType == StringType.Numeric)
            {
                StringBuilder newString = new StringBuilder();
                for (int index = 0; index < searchText.Length; index++)
                {
                    if (searchText[index] >= 65296)
                        newString.Append((char)(searchText[index] - 65248));
                    else
                        newString.Append(searchText[index]);
                }
                searchText = newString.ToString();
            }

            string firstField;

            if (useCodeFieldOnly)
                firstField = _wordbook.CodeField;
            else
                firstField = GetFirstMatchField(inputType);

            if (findOnly)
                m_DefaultView.RowFilter = m_DefaultRowFilter;

            // 如果在首选列中没有找到，再依次从其它列中进行查找。
            int i = -1;
            string field; // 保存当前查询列名的变量
            //string defaultExpression = _wordbook.GenerateFilterExpression(); // 字典类默认的过滤条件表达式
            string expression; // 保存根据查询内容和查询列生成的查询条件表达式
            do
            {
                i++;

                if (i == 0)
                    field = firstField;
                else
                {
                    field = _wordbook.CurrentMatchFields[i - 1];
                    if (field == firstField)
                        continue;
                }

                if (findOnly)
                {
                    //m_DefaultView.RowFilter = defaultExpression;
                    // 初始内容为空，则不需要查找
                    if (searchText.Length == 0)
                        break;
                    // 如果字段的类型是数值型，则查询文本的类型也必须是数值型，否则不能Find
                    if ((m_DefaultTable.Columns[field].DataType != typeof(string))
                       && (inputType != StringType.Numeric))
                        break;

                    m_DefaultView.Sort = field;
                    m_LocateIndex = m_DefaultView.Find(searchText);
                    if (m_LocateIndex >= 0)
                        break;
                }
                else
                {
                    if (inputType == StringType.Empty)
                        expression = "";
                    else if (m_DefaultTable.Columns[field].DataType == typeof(string))
                        expression = String.Format(CultureInfo.CurrentCulture
                           , GetMatchStringFormatByMatchType(matchType), field
                           , CommonOperation.TransferCondition(searchText, GetFilterOperatorByMatchType(matchType), true));
                    else if (inputType == StringType.Numeric)
                        expression = String.Format(CultureInfo.CurrentCulture
                           , GetMatchNumericFormatByMatchType(matchType), field, searchText);
                    else
                        expression = " 1 = 2 ";

                    if (m_DefaultRowFilter.Length == 0)
                        m_DefaultView.RowFilter = expression;
                    else if (expression.Length == 0)
                        m_DefaultView.RowFilter = m_DefaultRowFilter;
                    else
                        m_DefaultView.RowFilter = m_DefaultRowFilter + " AND " + expression;

                    // 找到记录则退出（如果是只使用代码列进行查找，则不论是否找到记录都退出）
                    if ((m_DefaultView.Count > 0) || (useCodeFieldOnly))
                        break;
                }
            }
            while (i < _wordbook.CurrentMatchFields.Count);

            // 每次过滤数据后初始化选中的行集合
            if (findOnly)
            {
                m_SelectedRowIndex = -1;

                // 只定位且能定位到记录时才将该记录保存到已选行集合中，否则保存第一行（如果存在的话）
                if (m_LocateIndex >= 0)
                    m_SelectedRowIndex = m_LocateIndex;
                else if (m_DefaultView.Count > 0)
                    m_SelectedRowIndex = 0;
            }

            return m_DefaultView.Count;
        }

        private string GetMatchNumericFormatByMatchType(ShowListMatchType matchType)
        {
            switch (matchType)
            {
                case ShowListMatchType.Begin:
                    return "{0} >= {1}";
                case ShowListMatchType.Full:
                    return "{0} = {1}";
                default:
                    return "{0} >= {1}";
            }
        }

        private string GetMatchStringFormatByMatchType(ShowListMatchType matchType)
        {
            switch (matchType)
            {
                case ShowListMatchType.Begin:
                    return "{0} like '{1}%'";
                case ShowListMatchType.Full:
                    return "{0} = '{1}'";
                default:
                    return "{0} like '%{1}%'";
            }
        }

        /// <summary>
        /// 取得当前过滤使用的操作符
        /// </summary>
        private CompareOperator GetFilterOperatorByMatchType(ShowListMatchType matchType)
        {
            if (matchType == ShowListMatchType.Full)
                return CompareOperator.Equal;
            else
                return CompareOperator.Like;
        }

        /// <summary>
        /// 根据字符串类型，获取首选的查询字段名
        /// </summary>
        /// <param name="inputType">字符串类型</param>
        /// <returns></returns>
        private string GetFirstMatchField(StringType inputType)
        {
            string field = "";
            switch (inputType)
            {
                case StringType.EnglishChar: // 全英文字母的字符串默认与py/wb字段匹配
                    if (_wordbook.CurrentMatchFields.Contains("py") && (!_useWB))
                        field = "py";
                    else if (_wordbook.CurrentMatchFields.Contains("wb") && _useWB)
                        field = "wb";
                    break;
                case StringType.Char: // ASCII字符
                case StringType.Other: // 其它字符，则默认与名称字段匹配
                    //if (_wordbook.CurrentFilterFields.Contains("name"))
                    field = _wordbook.NameField;
                    break;
                case StringType.Empty: // 空串时，使用默认的第一个字段
                    field = _wordbook.CurrentMatchFields[0];
                    break;
                // 数字/带数字的字符串都看作是代码字段，因无特殊标记，则默认取非py/wb/name的第一个字段
            }
            if (field.Length == 0)
            {
                foreach (string colName in _wordbook.CurrentMatchFields)
                {
                    if ((colName == "py") || (colName == "wb") || (colName == "name"))
                        continue;
                    field = colName;
                    break;
                }
            }
            if (String.IsNullOrEmpty(field))
                field = _wordbook.CurrentMatchFields[0];
            return field;
        }

        /// <summary>
        /// 在选中记录后进行数据处理工作
        /// </summary>
        private void SaveSelectedRow()
        {
            if ((m_DefaultView.Count == 0) || (ResultRows.Count == MaxCount))
                return;

            // 保存选中的行.数据集中只有一条匹配记录时直接处理数据集
            DataRow selectedRow;
            if (m_DefaultView.Count == 1)
            {
                selectedRow = m_DefaultView[0].Row;
            }
            else
            {
                // 如果对Grid进行了排序，则此时的RowHandle不是实际的数据行号，所以要通过GetDataRow方法来得到实际的数据行
                // 但是窗口显示前，因为会进行初始化工作，此时虽然处理了数据集，但是还没有更新Grid的数据源，所以不能用GetDataRow方法
                // 基于以上原因，增加了是否在初始化标志
                if (m_Initializing)
                    selectedRow = m_DefaultView[m_SelectedRowIndex].Row;
                else
                    selectedRow = gridViewbook.GetDataRow(m_SelectedRowIndex);
            }

            // 检查选中的记录是否已经存在
            foreach (DataRow row in ResultRows)
            {
                if (row[Wordbook.QueryCodeField].ToString() == selectedRow[Wordbook.QueryCodeField].ToString())
                    return;
            }

            ResultRows.Add(CloneDataRow(selectedRow));

            if (_maxCount > 1)
                lbSelectedRecords.Items.Add(ResultRows[ResultRows.Count - 1][Wordbook.NameField]);
        }

        /// <summary>
        /// 用传入的DataRow复制一条记录
        /// </summary>
        /// <param name="sourceRow"></param>
        /// <returns></returns>
        private static DataRow CloneDataRow(DataRow sourceRow)
        {
            DataRow cloneRow = sourceRow.Table.NewRow();
            for (int i = 0; i < sourceRow.Table.Columns.Count; i++)
                cloneRow[i] = sourceRow[i];
            return cloneRow;
        }

        /// <summary>
        /// 完成数据选择操作
        /// </summary>
        private void CommitSelectAction()
        {
            if (ResultRows.Count > 0)
            {
                string queryCodeField;
                if (String.IsNullOrEmpty(Wordbook.QueryCodeField))
                    queryCodeField = Wordbook.CodeField;
                else
                    queryCodeField = Wordbook.QueryCodeField;

                // 处理DisplayValue和CodeValue
                StringBuilder tempCode = new StringBuilder(ResultRows[0][Wordbook.CodeField].ToString().TrimEnd());
                StringBuilder tempDisplay = new StringBuilder(ResultRows[0][Wordbook.NameField].ToString().TrimEnd());
                StringBuilder tempQueryCode = new StringBuilder(ResultRows[0][queryCodeField].ToString().TrimEnd());
                for (int i = 1; i < ResultRows.Count; i++)
                {
                    tempCode.Append(MultiStringSeparators[1]);
                    tempCode.Append(ResultRows[i][Wordbook.CodeField].ToString().TrimEnd());
                    tempDisplay.Append(MultiStringSeparators[0]);
                    tempDisplay.Append(ResultRows[i][Wordbook.NameField].ToString().TrimEnd());
                    tempQueryCode.Append(MultiStringSeparators[1]);
                    tempQueryCode.Append(ResultRows[i][queryCodeField].ToString().TrimEnd());
                }
                _codeValue = tempCode.ToString();
                _displayValue = tempDisplay.ToString();
                _queryValue = tempQueryCode.ToString();

                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 取消数据选择动作
        /// </summary>
        private void CancleSelectAction()
        {
            ClearLastData();
            DialogResult = DialogResult.Cancel;
            //Close();
        }

        /// <summary>
        /// 执行选中行后的操作
        /// </summary>
        private void DoAfterRowHadBeenSelected()
        {
            if (m_SelectedRowIndex >= 0)
            {
                // 在多选模式下，增加在同一条记录上按回车时即退出的处理
                // 通过比较结果集记录数的改变情况来判断是否是在同一条记录上按的回车
                int preResultCount = ResultRows.Count;

                SaveSelectedRow();

                if ((_maxCount == 1) // 如果是单选，则可以完成提交
                   || (preResultCount == ResultRows.Count))
                    CommitSelectAction();
                else
                    textInputor.SelectAll();
            }
        }
        #endregion

        #region private UI methods
        /// <summary>
        /// 设置ShowListForm的显示样式
        /// </summary>
        private void SetFormStyle()
        {
            SuspendLayout();
            Opacity = 1;

            gridControl1.SuspendLayout();

            // 首先要确定Form及其控件的位置和尺寸
            SetFormAndControlsSize();
            SetFormAndControlsPosition();

            // 设置界面风格（简洁、完整）
            // 简洁窗口（输入框、Grid、扩展按钮）
            // 完整窗口（增加显示查询设置、确认、取消按钮）         
            gridControl1.ResumeLayout(false);
            gridControl1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

            // 窗口尺寸由小到大时，没有触发重绘事件，所以在此强制重绘
            mainForm_Paint(this, new PaintEventArgs(Graphics.FromHwnd(Handle), ClientRectangle));
        }

        ///// <summary>
        ///// 设置BottomPanel的高度(用改变高度的方式达到改变Panel是否可视的效果)
        ///// </summary>
        ///// <param name="visible">是否可视</param>
        //private void SetBottomPanelHeight(bool visible)
        //{
        //   if (visible)
        //      panelBottom.Height = m_DefBottomRegionHeight;
        //   else
        //      panelBottom.Height = 0;
        //}

        /// <summary>
        /// 设置窗口中控件的尺寸
        /// </summary>
        private void SetFormAndControlsSize()
        {
            // 首先将输入框的尺寸与调用控件进行同步
            textInputor.Size = m_InputControlSize;

            panelTop.Size = new Size(m_InputControlSize.Width, m_InputControlSize.Height);

            // 多选时根据名称字段的宽度设置多选列表的宽度
            if (_maxCount > 1)
            {
                int listWidth = 150;
                if (gridViewbook.Columns[Wordbook.NameField] != null)
                    listWidth = gridViewbook.Columns[Wordbook.NameField].Width;
                panelMultiSelect.Width = listWidth + panelSelRecord.Width
                   + SystemInformation.VerticalScrollBarWidth;
            }

            // 判断窗口布局类型，并获得Grid的理想宽度
            JudgeFormLayoutType();

            // 调整窗口及数据部分的尺寸
            panelData.Size = new Size(DataRegionWidth, m_DefDataRegionHeight);
            Size = ValiableRegionSize;
        }

        private void SetFormAndControlsPosition()
        {
            // 如果初始位置的坐标是(-1, -1)， 则改成居中显示
            if (m_InitPosition == new Point(-1, -1))
                m_InitPosition = new Point((m_ScreenSize.Width - Width) / 2
                   , (m_ScreenSize.Height - Height) / 2);

            // 计算窗口、数据输入部分、数据选择部分的坐标
            int pFX; // Form的X坐标
            int pFY; // Form的Y坐标
            int pTX; // 输入框所在Panel的X坐标
            int pTY; // 输入框所在Panel的Y坐标
            int pDX; // Grid所在Panel的的X坐标
            int pDY; // Grid所在Panel的的Y坐标

            // 设置窗口布局
            if (m_Left2Right)
            {
                pFX = m_InitPosition.X - m_ShadowOffset;
                pTX = m_ShadowOffset;
                pDX = m_ShadowOffset;
                panelSetting.Dock = DockStyle.Right;
            }
            else
            {
                pFX = m_InitPosition.X + m_InputControlSize.Width + m_ShadowOffset - Width;
                pTX = Width - panelTop.Width - m_ShadowOffset;
                pDX = Width - panelData.Width - m_ShadowOffset;
                panelSetting.Dock = DockStyle.Left;
            }

            if (m_Top2Down)
            {
                pFY = m_InitPosition.Y - m_ShadowOffset;
                pTY = m_ShadowOffset;
                pDY = m_ShadowOffset + panelTop.Height;
            }
            else
            {
                pFY = m_InitPosition.Y + panelTop.Height + m_ShadowOffset - Height;
                pTY = m_ShadowOffset + panelData.Height;
                pDY = m_ShadowOffset;
            }

            // 多选时根据左右布局，改变多选列表的位置
            if (_maxCount > 1)
                SetMultiSelectPanelPosition(m_Left2Right);

            Location = new Point(pFX, pFY);
            panelTop.Location = new Point(pTX, pTY);
            panelData.Location = new Point(pDX, pDY);
        }

        private void SetMultiSelectPanelPosition(bool left2Right)
        {
            if (left2Right)
            {
                panelMultiSelect.Dock = DockStyle.Right;
                panelSelRecord.Dock = DockStyle.Left;
                btnSelectAll.Text = ">>";
                btnSelectOne.Text = ">";
                btnDeleteAll.Text = "<<";
                btnDeleteOne.Text = "<";
            }
            else
            {
                panelMultiSelect.Dock = DockStyle.Left;
                panelSelRecord.Dock = DockStyle.Right;
                btnSelectAll.Text = "<<";
                btnSelectOne.Text = "<";
                btnDeleteAll.Text = ">>";
                btnDeleteOne.Text = ">";
            }
        }

        /// <summary>
        /// 判断窗口布局类型，并获得Grid的理想宽度
        /// </summary>
        private void JudgeFormLayoutType()
        {
            // 1、判断窗口是从上到下布局还是从下到上布局
            if ((m_ScreenSize.Height - m_InitPosition.Y) >= ValiableRegionSize.Height)
                m_Top2Down = true;
            else
                m_Top2Down = false;

            int base2Right = m_ScreenSize.Width - m_InitPosition.X;

            // 2、判断窗口是从左到右显示还是从右到左显示
            m_Left2Right = (base2Right >= Math.Max(DataRegionWidth, InputRegionWidth));
        }

        private void SynchDynamicToControl()
        {
            ckEditDynamic.Checked = IsDynamic;
            //barItemIsDynamic.Checked = _isDynamic;
        }

        private void SynchMatchTypeToControl()
        {
            switch (_matchType)
            {
                case ShowListMatchType.Begin:
                    if (!ckEditBegin.Checked)
                        ckEditBegin.Checked = true;
                    ckEditAny.Checked = false;
                    ckEditFull.Checked = false;
                    //if (!barItemMatchBegin.Checked)
                    //   barItemMatchBegin.Checked = true;
                    //barItemMatchFull.Checked = false;
                    //barItemMatchAny.Checked = false;
                    break;
                case ShowListMatchType.Full:
                    if (!ckEditFull.Checked)
                        ckEditFull.Checked = true;
                    ckEditAny.Checked = false;
                    ckEditBegin.Checked = false;
                    //if (!barItemMatchFull.Checked)
                    //   barItemMatchFull.Checked = true;
                    //barItemMatchBegin.Checked = false;
                    //barItemMatchAny.Checked = false;
                    break;
                default:
                    if (!ckEditAny.Checked)
                        ckEditAny.Checked = true;
                    ckEditBegin.Checked = false;
                    ckEditFull.Checked = false;
                    //if (!barItemMatchAny.Checked)
                    //   barItemMatchAny.Checked = true;
                    //barItemMatchFull.Checked = false;
                    //barItemMatchBegin.Checked = false;
                    break;
            }
        }

        private void ResetControlFont(Font font)
        {
            Font = font;
            //styleController1.Appearance.Font = font;
            //styleController1.AppearanceDisabled.Font = font;
            //styleController1.AppearanceDropDown.Font = font;
            //styleController1.AppearanceDropDownHeader.Font = font;
            //styleController1.AppearanceFocused.Font = font;

            textInputor.Font = font;
            btnOk.Font = font;
            btnCancel.Font = font;
            btnSelectAll.Font = font;
            btnSelectOne.Font = font;
            btnDeleteAll.Font = font;
            btnDeleteOne.Font = font;
            //lbSelectedRecords.Font = font;

            foreach (AppearanceObject ap in gridViewbook.Appearance)
                ap.Font = font;
        }

        private void ChangeFormOpacity(bool toTransparency)
        {
            if (toTransparency)
                Opacity = 0.2;
            else
                Opacity = 1;
        }
        #endregion

        #region events
        private void mainForm_Load(object sender, EventArgs e)
        {
            ImeMode = ImeMode.Off; // 强制关闭输入法
            // 根据属性设置窗口样式
            SetFormStyle();
        }

        private void mainForm_Shown(object sender, EventArgs e)
        {
            m_ParentAutoClose = false;
            if (Owner != null)
            {
                CaptureCursorForm cursorForm = Owner as CaptureCursorForm;
                if ((cursorForm != null) && (cursorForm.TimerActived))
                {
                    cursorForm.TimerActived = false;
                    m_ParentAutoClose = true;
                }
            }

            // 直接定位到指定行
            if (m_LocateIndex >= 0)
            {
                gridViewbook.FocusedRowHandle = m_LocateIndex;
                // 设置聚焦的行后GridView没有自动滚（原因不明！！！），所以用下面的方法将聚焦的行显示到屏幕上
                gridViewbook.TopRowIndex = m_LocateIndex - 3;
                m_LocateIndex = -1;
            }

            if (textInputor.CanFocus)
                textInputor.Focus();
            else
                throw new ApplicationException("不能定位输入控件");
            TimerActived = true;
        }

        private void mainForm_Paint(object sender, PaintEventArgs e)
        {
            // 显示不规则窗口
            Rectangle topRectangle = new Rectangle(panelTopShadow.Location, panelTopShadow.Size);
            Rectangle dataRectangle = new Rectangle(panelDataShadow.Location, panelDataShadow.Size);

            Region newRegion = new Region(topRectangle);
            newRegion.Union(dataRectangle);
            Region = newRegion;
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            TimerActived = false;
            //textInputor.Text = "";

            if (Owner != null)
            {
                CaptureCursorForm cursorForm = Owner as CaptureCursorForm;
                if ((cursorForm != null) && m_ParentAutoClose)
                    cursorForm.TimerActived = true;
            }
            m_ParentAutoClose = false;
        }

        private void mainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 17) // Control 键
                ChangeFormOpacity(true);
        }

        private void mainForm_KeyUp(object sender, KeyEventArgs e)
        {
            //if (!e.Control)
            ChangeFormOpacity(false);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_maxCount > 1)
                CommitSelectAction();
            else
                DoAfterRowHadBeenSelected();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            CancleSelectAction();
        }

        private void inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                // 判断按回车是否可以直接退出
                if (m_HaveSearch || IsDynamic)
                    //CommitSelectAction();
                    DoAfterRowHadBeenSelected();
                else if (!IsDynamic)
                    SearchMatchRow(m_SearchText, MatchType, false);
            }
        }

        private void inputBox_TextChanged(object sender, EventArgs e)
        {
            m_HaveSearch = false;
            m_SearchText = textInputor.Text.Trim();

            // 动态查询时，每次文本改变都进行查询
            if (IsDynamic)
                SearchMatchRow(m_SearchText, MatchType, false);
        }

        private void inputBox_Leave(object sender, EventArgs e)
        {
            if (!gridControl1.Focused || (m_DefaultView.Count <= 1))
                textInputor.Focus();
        }

        private void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if ((_maxCount > 1) && e.Control && (e.KeyCode == Keys.Enter))
                CommitSelectAction(); // 多选时按Ctrl + Enter可以退出

            // 按上下键、翻页键，则要发送到Grid中
            switch (e.KeyCode)
            {
                case Keys.Down:
                    gridViewbook.MoveNext();
                    break;
                case Keys.Up:
                    gridViewbook.MovePrev();
                    break;
                case Keys.PageDown:
                    gridViewbook.MoveNextPage();
                    break;
                case Keys.PageUp:
                    gridViewbook.MovePrevPage();
                    break;
                //e.Handled = SendKey2DataGridView(e.KeyCode);               
                default:
                    e.Handled = false;
                    break;
            }
        }

        private void inputBox_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            //if (e.Button == MenuButton)
            //{
            //   // 每次手工创建菜单组件（因为BarManager在窗口关闭时会自动释放）
            //   BarManager newBarManager = new BarManager();
            //   InitializePopupMenuBeforCall(newBarManager);

            //   popupSettingMenu.ShowPopup(textInputor.PointToScreen(new Point(textInputor.Width - 150, textInputor.Height)));
            //}
        }

        private void inputBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                gridViewbook.MoveBy(-3);
            else
                gridViewbook.MoveBy(3);
        }

        private void panelBottom_SizeChanged(object sender, EventArgs e)
        {
            // 重新计算按钮的位置
            if (panelBottom.Width >= 250)
            {
                btnOk.Location = new Point(panelBottom.Width / 2 - 125, 3);
                btnCancel.Location = new Point(panelBottom.Width / 2 + 50, 3);
            }
            else
            {
                btnOk.Location = new Point(panelBottom.Width / 2 - 75, 3);
                btnCancel.Location = new Point(panelBottom.Width / 2 + 1, 3);
            }
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            // 双击记录时表示直接退出
            if (gridViewbook.FocusedRowHandle >= 0)
                //CommitSelectAction();
                DoAfterRowHadBeenSelected();
            textInputor.Focus();
        }

        private void gridViewbook_SelectionChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (gridViewbook.FocusedRowHandle == -1)
                m_SelectedRowIndex = -1;
            else
            {
                m_SelectedRowIndex = e.FocusedRowHandle;
            }
            // 焦点置回输入框
            textInputor.Focus();
        }

        private void gridViewbook_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            textInputor.Focus();
        }

        private void panelTop_SizeChanged(object sender, EventArgs e)
        {
            panelTopShadow.Size = panelTop.Size
               + new Size(m_ShadowOffset * 2, m_ShadowOffset * 2);
        }

        private void panelData_SizeChanged(object sender, EventArgs e)
        {
            panelDataShadow.Size = panelData.Size
               + new Size(m_ShadowOffset * 2, m_ShadowOffset * 2);
        }

        private void panelTop_LocationChanged(object sender, EventArgs e)
        {
            // 设置阴影部分坐标
            Size baseOffset = new Size(m_ShadowOffset, m_ShadowOffset);
            panelTopShadow.Location = panelTop.Location - baseOffset;
        }

        private void panelData_LocationChanged(object sender, EventArgs e)
        {
            // 设置阴影部分坐标
            Size baseOffset = new Size(m_ShadowOffset, m_ShadowOffset);
            panelDataShadow.Location = panelData.Location - baseOffset;
        }

        #region //removed
        //private void inputBox_MouseUp(object sender, MouseEventArgs e)
        //{
        //   if ((e.Button & MouseButtons.Right) != 0
        //      && textInputor.ClientRectangle.Contains(e.X, e.Y))
        //   {
        //      // 每次手工创建菜单组件（因为BarManager在窗口关闭时会自动释放）
        //      BarManager newBarManager = new BarManager();
        //      InitializePopupMenuBeforCall(newBarManager);

        //      popupSettingMenu.ShowPopup(Control.MousePosition);
        //   }
        //}

        //private void barItemIsDynamic_CheckedChanged(object sender, ItemClickEventArgs e)
        //{
        //   IsDynamic = barItemIsDynamic.Checked;
        //}

        //private void barItemMatchBegin_CheckedChanged(object sender, ItemClickEventArgs e)
        //{
        //   if (barItemMatchBegin.Checked)
        //      MatchType = ShowListMatchType.Begin;
        //}

        //private void barItemMatchFull_CheckedChanged(object sender, ItemClickEventArgs e)
        //{
        //   if (barItemMatchFull.Checked)
        //      MatchType = ShowListMatchType.Full;
        //}

        //private void barItemMatchAny_CheckedChanged(object sender, ItemClickEventArgs e)
        //{
        //   if (barItemMatchAny.Checked)
        //      MatchType = ShowListMatchType.Any;
        //}
        #endregion

        private void ckEditBegin_CheckedChanged(object sender, EventArgs e)
        {
            if (ckEditBegin.Checked)
                MatchType = ShowListMatchType.Begin;
        }

        private void ckEditAny_CheckedChanged(object sender, EventArgs e)
        {
            if (ckEditAny.Checked)
                MatchType = ShowListMatchType.Any;
        }

        private void ckEditFull_CheckedChanged(object sender, EventArgs e)
        {
            if (ckEditFull.Checked)
                MatchType = ShowListMatchType.Full;
        }

        private void ckEditDynamic_CheckedChanged(object sender, EventArgs e)
        {
            IsDynamic = ckEditDynamic.Checked;
        }

        private void btnSelectOne_Click(object sender, EventArgs e)
        {
            DoAfterRowHadBeenSelected();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int rowHandle = 0; rowHandle < gridViewbook.RowCount; rowHandle++)
            {
                m_SelectedRowIndex = rowHandle;
                DoAfterRowHadBeenSelected();
            }
        }

        private void btnDeleteOne_Click(object sender, EventArgs e)
        {
            if (lbSelectedRecords.SelectedIndex >= 0)
            {
                ResultRows.RemoveAt(lbSelectedRecords.SelectedIndex);
                lbSelectedRecords.Items.RemoveAt(lbSelectedRecords.SelectedIndex);
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            ClearLastData();
        }

        private void DataSelectForm_FontChanged(object sender, EventArgs e)
        {
            ResetControlFont(Font);
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (lbSelectedRecords.SelectedIndex <= 0)
                return;

            int pos = lbSelectedRecords.SelectedIndex;
            // 先移动result中的记录
            DataRow selectedRow = ResultRows[pos];
            ResultRows.RemoveAt(pos);
            ResultRows.Insert(pos - 1, selectedRow);
            // 再移动控件中的记录
            object selectedObj = lbSelectedRecords.Items[pos];
            lbSelectedRecords.Items.RemoveAt(pos);
            lbSelectedRecords.Items.Insert(pos - 1, selectedObj);

            lbSelectedRecords.SelectedIndex = pos - 1;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (lbSelectedRecords.SelectedIndex < 0)
                return;
            if (lbSelectedRecords.SelectedIndex == (lbSelectedRecords.ItemCount - 1))
                return;

            int pos = lbSelectedRecords.SelectedIndex;
            // 先移动result中的记录
            DataRow selectedRow = ResultRows[pos];
            ResultRows.RemoveAt(pos);
            ResultRows.Insert(pos + 1, selectedRow);
            // 再移动控件中的记录
            object selectedObj = lbSelectedRecords.Items[pos];
            lbSelectedRecords.Items.RemoveAt(pos);
            lbSelectedRecords.Items.Insert(pos + 1, selectedObj);

            lbSelectedRecords.SelectedIndex = pos + 1;
        }
        #endregion
    }
}

