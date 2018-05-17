using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DrectSoft.Common.Eop;
using DrectSoft.Wordbook;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace DrectSoft.Common.Library
{
    /// <summary>
    /// 数据选择组件
    /// </summary>
    [DefaultEvent("CodeValueChanged"), ToolboxBitmapAttribute(typeof(DrectSoft.Common.Library.LookUpEditor), "Images.ShowListBox.ico")]
    public partial class LookUpEditor : TextEdit, ISupportInitialize
    {
        #region 改写的 ButtonEdit 属性
        /// <summary>
        /// 
        /// </summary>
        protected bool AllowButtonPress
        {
            get { return !base.GetValidationCanceled(); }
        }

        /// <summary>
        /// <para>Gets the class name of the current editor.
        /// </para>
        /// </summary>
        /// <value>The string identifying the class name of the current editor.
        /// </value>
        [Browsable(false)]
        public override string EditorTypeName
        {
            get { return "ButtonEdit"; }
        }

        /// <summary>
        /// <para>Gets a value indicating whether a container needs to set focus to the editor when it works as an inplace control.
        /// </para>
        /// </summary>
        /// <value><b>true</b>, if a container needs to set focus to the editor; otherwise, <b>false</b>.
        /// </value>
        [Description("Gets a value indicating whether a container needs to set focus to the editor when it works as an inplace control."), Browsable(false)]
        public override bool IsNeedFocus
        {
            get { return (Properties.TextEditStyle == TextEditStyles.Standard); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal new ButtonEditViewInfo ViewInfo
        {
            get { return (base.ViewInfo as ButtonEditViewInfo); }
        }

        /// <summary>
        /// 代码中不能使用此属性，否则后果自负
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new object EditValue
        {
            get { return base.EditValue; }
            set { base.EditValue = value; }
        }

        /// <summary>
        /// 代码中不能使用此属性，否则后果自负
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new RepositoryItemButtonEdit Properties
        {
            get { return (base.Properties as RepositoryItemButtonEdit); }
        }
        #endregion

        #region properties
        /// <summary>
        /// 未选择记录时是否保留编辑框的现在的数据
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("在ShowList窗口未选择记录时是否保留编辑框的现在的数据。默认不保留"),
          DefaultValue(false)
        ]
        public bool KeepOriginalData
        {
            get { return _keepOriginalData; }
            set { _keepOriginalData = value; }
        }
        private bool _keepOriginalData;

        /// <summary>
        /// ReadOnly
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("输入框设为只读"),
          DefaultValue(false)
        ]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                Properties.ReadOnly = _readOnly;
                ShowListButton.Enabled = !_readOnly;
            }
        }
        private bool _readOnly;

        /// <summary>
        /// 显示“S”按钮
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("编辑框后面是否显示表示ShowList的“S”按钮。默认显示"),
          DefaultValue(false)
        ]
        public bool ShowSButton
        {
            get { return _showSButton; }
            set
            {
                _showSButton = value;
                ShowListButton.Visible = value;
            }
        }
        private bool _showSButton;

        /// <summary>
        /// 设置或获取边框样式
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("设置或获取边框样式"),
          DefaultValue(BorderStyle.FixedSingle)
        ]
        public new BorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set
            {
                if (value != _borderStyle)
                {
                    _borderStyle = value;

                    switch (_borderStyle)
                    {
                        case BorderStyle.None:
                            base.BorderStyle = BorderStyles.NoBorder;
                            Properties.ButtonsStyle = BorderStyles.Flat;
                            break;
                        case BorderStyle.Fixed3D:
                            base.BorderStyle = BorderStyles.Style3D;
                            Properties.ButtonsStyle = BorderStyles.Style3D;
                            break;
                        case BorderStyle.FixedSingle:
                            base.BorderStyle = BorderStyles.Simple;
                            Properties.ButtonsStyle = BorderStyles.Simple;
                            break;
                    }
                    //this.LookAndFeel.Style = LookAndFeelStyle.Skin;
                }
            }
        }
        private BorderStyle _borderStyle;

        /// <summary>
        /// 按回车是否自动跳转到下一控件
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("按回车是否自动跳转到下一控件"),
          DefaultValue(false)
        ]
        public new bool EnterMoveNextControl
        {
            get { return _enterMoveNextControl; }
            set { _enterMoveNextControl = value; }
        }
        private bool _enterMoveNextControl;

        /// <summary>
        /// 自动弹出ShowList窗口
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("进入编辑框后是否立即显示代码选择窗口。默认不显示"),
          DefaultValue(false)
        ]
        public bool ShowFormImmediately
        {
            get { return _showFormImmediately; }
            set { _showFormImmediately = value; }
        }
        private bool _showFormImmediately;

        /// <summary>
        /// ShowList窗口的显示模式
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("ShowList窗口的显示模式。默认简洁显示"),
          DefaultValue(ShowListFormMode.Concision)
        ]
        public ShowListFormMode FormMode
        {
            get { return _formMode; }
            set { _formMode = value; }
        }
        private ShowListFormMode _formMode;

        /// <summary>
        /// 使用的代码字典类型
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("使用何种类型的代码字典。普通的、通过SQL语句创建的或由StringList创建的"),
          DefaultValue(WordbookKind.Normal),
        ]
        public WordbookKind Kind
        {
            get { return _kind; }
            set
            {
                _kind = value;
                if (this.DesignMode)
                    return;

                if (_kind == WordbookKind.List)
                    CreateListWordbook();
            }
        }
        private WordbookKind _kind;

        /// <summary>
        /// 预定义字典类
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("选择并设置该编辑框使用的预定义字典类"),
          DefaultValue(null),
          Editor(typeof(NormalWordbookEditor), typeof(UITypeEditor))
        ]
        public BaseWordbook NormalWordbook
        {
            get { return _normalWordbook; }
            set { _normalWordbook = value; }
        }
        private BaseWordbook _normalWordbook;

        /// <summary>
        /// 默认的StringList字典名称
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("用StringList动态创建的字典名字，要保证在本程序集内唯一"),
          DefaultValue(null)
        ]
        public string ListWordbookName
        {
            get { return _listWordbookName; }
            set
            {
                _listWordbookName = value;
                if (this.DesignMode)
                    return;

                CreateListWordbook();
            }
        }
        private string _listWordbookName;

        /// <summary>
        /// 作为字典使用的StringList
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("用来动态创建字典的StringList"),
          Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design", typeof(UITypeEditor)),
          MergableProperty(false),
          DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
          Localizable(true)
        ]
        public Collection<string> ListWordbookValues
        {
            get
            {
                if (_listWordbookValues == null)
                    _listWordbookValues = new Collection<string>();
                if (_listWordbook != null)
                {
                    _listWordbookValues.Clear();
                    foreach (string code in _listWordbook.Items)
                        _listWordbookValues.Add(code);
                }
                return _listWordbookValues;
            }
        }
        private Collection<string> _listWordbookValues;

        /// <summary>
        /// 使用StringList创建的字典类
        /// </summary>
        [Browsable(false), DefaultValue(null)]
        public ListWordbook ListWordbook
        {
            get { return _listWordbook; }
            set
            {
                _listWordbook = value;
                if (value == null)
                    _listWordbookName = "";
                else
                    _listWordbookName = _listWordbook.WordbookName;
            }
        }
        private ListWordbook _listWordbook;

        /// <summary>
        /// 使用SQL语句动态创建的字典类
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("使用SQL语句动态创建的字典类"),
          DefaultValue(null),
          Editor(typeof(SqlWordbookEditor), typeof(UITypeEditor))
        ]
        public SqlWordbook SqlWordbook
        {
            get { return _sqlWordbook; }
            set { _sqlWordbook = value; }
        }
        private SqlWordbook _sqlWordbook;

        /// <summary>
        /// 对应的ShowListWindow
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("显示、选择数据的ShowList窗口")
        ]
        public LookUpWindow ListWindow
        {
            get { return _listWindow; }
            set
            {
                _listWindow = value;
                //SyncListWindowProperties();
            }
        }
        private LookUpWindow _listWindow;

        /// <summary>
        /// 获得当前使用的字典类实例
        /// </summary>
        [
          Browsable(false),
          Category("ShowListConfig"),
          Description("当前使用的字典类实例"),
          ReadOnly(true)
        ]
        public BaseWordbook CurrentWordbook
        {
            get
            {
                BaseWordbook wordbook;
                switch (_kind)
                {
                    case WordbookKind.Normal:
                        wordbook = _normalWordbook;
                        break;
                    case WordbookKind.List:
                        wordbook = _listWordbook;
                        break;
                    default:
                        wordbook = _sqlWordbook;
                        break;
                }
                return wordbook;
            }
        }

        /// <summary>
        /// 显示值
        /// </summary>
        [
          Browsable(false),
          Category("ShowListConfig"),
          Description("显示在编辑框中的值。多个名称时用“、”隔开"),
          DefaultValue(""),
          ReadOnly(true)
        ]
        public string DisplayValue
        {
            get
            {
                if (_displayValue == null)
                    _displayValue = "";
                return _displayValue;
            }
        }
        private string _displayValue;

        /// <summary>
        /// 作为代码保存的值
        /// </summary>
        [
          Browsable(false),
          Category("ShowListConfig"),
          Description("编辑框中的值对应的代码。多个代码时用“,”或“、”隔开"),
          DefaultValue("")
        ]
        public string CodeValue
        {
            get
            {
                if (_codeValue == null)
                    _codeValue = "";
                return _codeValue;
            }
            set
            {
                // 直接赋值，可以实现初始化控件数据的效果
                ResetPropertiesValue();
                _codeValue = value;
                SelectRecord(_codeValue, ShowListCallType.Initialize);
            }
        }
        private string _codeValue;

        /// <summary>
        /// 编辑框中的值对应的代码。多个代码时用“,”隔开
        /// </summary>
        [
          Browsable(false),
          Category("ShowListConfig"),
          Description("编辑框中的值对应的查询代码。多个代码时用“,”或“、”隔开"),
          ReadOnly(true)
        ]
        public string QueryValue
        {
            get
            {
                if (_queryValue == null)
                    _queryValue = "";
                return _queryValue;
            }
        }
        private string _queryValue;

        /// <summary>
        /// 标记输入框是否已经选择了值/是否已初始化
        /// </summary>
        [Browsable(false)]
        public bool HadGetValue
        {
            get { return !String.IsNullOrEmpty(CodeValue); }
        }

        /// <summary>
        /// 选中的记录对应的DataRow
        /// </summary>
        [
          Browsable(false),
          Category("ShowListConfig"),
          Description("选中的数据记录行集合"),
          ReadOnly(true)
        ]
        public Collection<DataRow> ResultRows
        {
            get
            {
                if (_resultRows == null)
                    _resultRows = new Collection<DataRow>();
                return _resultRows;
            }
        }
        private Collection<DataRow> _resultRows;

        /// <summary>
        /// 根据当前选中的字典记录生成其对应的对象实例.
        /// 如果对象类中有对数据库的操作,要记得给对象的SqlHelper属性赋值
        /// </summary>
        [Browsable(false)]
        public Collection<EPBaseObject> PersistentObjects
        {
            get
            {
                if (_persistentObjects == null)
                {
                    _persistentObjects = new Collection<EPBaseObject>();

                    if ((CurrentWordbook != null) && (_kind == WordbookKind.Normal))
                    {
                        foreach (DataRow row in ResultRows)
                        {
                            CurrentWordbook.CurrentRow = row;
                            _persistentObjects.Add(CurrentWordbook.PersistentObject);
                        }
                    }
                }
                return _persistentObjects;
            }
        }
        private Collection<EPBaseObject> _persistentObjects;

        /// <summary>
        /// 允许选择的最少记录数(最小为0),会影响到未输入内容时按回车是否弹出选择窗口
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("允许选择的最少记录数,最小为0"),
          DefaultValue(0)
        ]
        public int MinCount
        {
            get { return _minCount; }
            set
            {
                if (value >= 0)
                    _minCount = value;
                else
                    _minCount = 0;
                //SyncListWindowProperties();
            }
        }
        private int _minCount;

        /// <summary>
        /// 允许选择的最大记录数
        /// </summary>
        [
          Category("ShowListConfig"),
          Description("允许选择的最大记录数，不能小于MinCount"),
          DefaultValue(1)
        ]
        public int MaxCount
        {
            get { return _maxCount; }
            set
            {
                if (value < MinCount)
                    _maxCount = MinCount;
                else
                    _maxCount = value;
                //SyncListWindowProperties();
            }
        }
        private int _maxCount;
        #endregion


        #region private variables & properties
        private EditorButton ShowListButton
        {
            get
            {
                if (Properties.Buttons.Count == 0)
                    Properties.Buttons.Add(new EditorButton());
                return Properties.Buttons[0];
            }
        }

        /// <summary>
        /// 控件所属的Form
        /// </summary>
        private Form m_OwnerForm;
        #endregion

        #region properties's ShouldSerialize & Reset Method
        private bool ShouldSerializeWordbook()
        {
            return (_normalWordbook != null);
        }
        private void ResetWordbook()
        {
            NormalWordbook = null;
        }
        #endregion

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        public LookUpEditor()
        {
            try
            {
                //Add By wwj 2012-06-07 修改下来标示的符号
                ShowListButton.Caption = "▼";
                ShowListButton.Appearance.Font = new Font("宋体", 6F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));

                ShowListButton.Kind = ButtonPredefines.Glyph;
                ShowListButton.Visible = false;
                Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));

                _borderStyle = BorderStyle.FixedSingle;
                _showSButton = false;
                //_showFormImmediately = true;
                _formMode = ShowListFormMode.Concision;
                _kind = WordbookKind.Normal;

                _minCount = 0;
                _maxCount = 1;

                ResetPropertiesValue();

                if (!DesignMode)
                    BoundEventToControl();


                this.ContextMenu = new ContextMenu();//bwj 20121022
            }
            catch (Exception xe)
            {
                throw xe;
            }
        }
        #endregion

        #region ISupportInitialize Members
        /// <summary>
        /// 用信号通知对象初始化即将开始。
        /// </summary>
        public void BeginInit()
        { }

        /// <summary>
        /// 用信号通知对象初始化已完成。
        /// </summary>
        public void EndInit()
        {
            if (!this.DesignMode)
                CreateListWordbook();
        }

        #endregion

        #region private methods
        private void BoundEventToControl()
        {
            Properties.Appearance.Changed += new EventHandler(BoxAppearanceChanged);
            //FontChanged += new EventHandler(InputEditorFontChanged);
            KeyPress += new KeyPressEventHandler(InputEditorKeyPress);
            KeyDown += new KeyEventHandler(InputEditorKeyDown);
            Enter += new EventHandler(InputEditorEnter);
            Leave += new EventHandler(InputEditorLeave);

        }

        /// <summary>
        /// 用StringList创建List字典
        /// </summary>
        private void CreateListWordbook()
        {
            if ((_kind == WordbookKind.List)
               && (_listWordbookName != null) && (_listWordbookName.Length > 0)
               && (_listWordbookValues != null))
            {
                _listWordbook = new ListWordbook(_listWordbookName, _listWordbookValues);
            }
        }

        private void SyncListWindowProperties()
        {
            if (_listWindow != null)
            {
                _listWindow.MinCount = MinCount;
                _listWindow.MaxCount = MaxCount;
                _listWindow.Font = Font;
            }
        }

        /// <summary>
        /// 执行选择代码的过程
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="callType">调用ShowListWindow方式--</param>
        private void SelectRecord(string searchText, ShowListCallType callType)
        {
            LookUpWindow _listWindow_back = new LookUpWindow();
            _listWindow_back = _listWindow;
            if (_listWindow == null)
            {
                MessageBox.Show("未设置代码选择窗口", "严重错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SyncListWindowProperties();

            // 调用ShowListWindow的执行函数
            // 暂未处理使用动态SQL语句的情况！！！
            if (callType == ShowListCallType.Initialize)
                _listWindow.CallLookUpWindow(CurrentWordbook
                   , Kind
                   , searchText);
            else
            {
                //if (m_OwnerForm == null)
                m_OwnerForm = FindForm();
                // 设置ShowList窗口的查询匹配模式
                ShowListMatchTypeSetting setting = ShowListMatchTypeRecorder.ReadDefaultSetting(CurrentWordbook.WordbookName);
                _listWindow.MatchType = setting.MatchType;
                _listWindow.IsDynamic = setting.IsDynamic;

                bool oldShowImmediately = ShowFormImmediately;
                ShowFormImmediately = false; // 强制关闭此属性
                _listWindow.Owner = m_OwnerForm;
                _listWindow.CallLookUpWindow(CurrentWordbook
                   , Kind
                   , searchText
                   , FormMode
                   , this.Parent.PointToScreen(this.Location)
                   , Size
                   , Screen.GetBounds(this)
                   , callType);
                if (_listWindow.CodeValue.Trim() == "" && _listWindow.DisplayValue == "")
                {
                    _listWindow = _listWindow_back;
                }
                _listWindow.Owner = null;

                ShowFormImmediately = oldShowImmediately;
                // 保存ShowList窗口的查询匹配模式
                ShowListMatchTypeRecorder.WriteSetting(CurrentWordbook.WordbookName, _listWindow.MatchType, _listWindow.IsDynamic);
            }

            // 保存选择结果         
            if (_listWindow.HadGetValue)
            {
                string tempCodeValue = _codeValue;
                string tempDisplayValue = _displayValue;

                _codeValue = _listWindow.CodeValue;
                _displayValue = _listWindow.DisplayValue;
                _queryValue = _listWindow.QueryValue;

                if (_resultRows == null)
                    _resultRows = new Collection<DataRow>();
                else
                    _resultRows.Clear();

                foreach (DataRow row in _listWindow.ResultRows)
                    _resultRows.Add(row);

                if (_persistentObjects != null)
                    _persistentObjects.Clear();

                _persistentObjects = null;

                // 触发CodeValue变化事件
                if ((_codeValue != tempCodeValue) || (_displayValue != tempDisplayValue))
                    OnCodeValueChanged(new EventArgs());
            }
            else
            {
                // 不保留原始数据或初始化控件时，都清空相关属性
                if ((!KeepOriginalData) || (callType == ShowListCallType.Initialize))
                    CleanupPropertiesValue();
            }
            Text = _displayValue;
            SelectAll();
        }

        /// <summary>
        /// 清除输入框中的数据
        /// </summary>
        private void CleanupPropertiesValue()
        {
            string tempCodeValue = _codeValue;
            string tempDisplayValue = _displayValue;

            ResetPropertiesValue();

            // 触发CodeValue变化事件
            if ((_codeValue != tempCodeValue) || (_displayValue != tempDisplayValue))
                OnCodeValueChanged(new EventArgs());
        }

        /// <summary>
        /// 重设ShowListBox中当前保留的代码等数据
        /// </summary>
        private void ResetPropertiesValue()
        {
            _codeValue = "";
            _displayValue = "";
            _queryValue = "";
            if (_resultRows != null)
                _resultRows.Clear();

            if (CurrentWordbook != null)
                CurrentWordbook.CurrentRow = null;

            if (_persistentObjects != null)
            {
                _persistentObjects.Clear();
            }
        }

        private void DoShowWindowDirectly(string searchText)
        {
            bool oldSet = ListWindow.AlwaysShowWindow;
            ListWindow.AlwaysShowWindow = true;

            SelectRecord(searchText, ShowListCallType.ShowDirectly);

            ListWindow.AlwaysShowWindow = oldSet;
        }
        #endregion

        #region custom eventhandler
        /// <summary>
        /// 选择值变化事件
        /// </summary>
        public event EventHandler CodeValueChanged
        {
            add { onCodeValueChanged = (EventHandler)Delegate.Combine(onCodeValueChanged, value); }
            remove { onCodeValueChanged = (EventHandler)Delegate.Remove(onCodeValueChanged, value); }
        }
        private event EventHandler onCodeValueChanged;

        /// <summary>
        /// 选择的数据已改变事件
        /// </summary>
        /// <param name="e"></param>
        private void OnCodeValueChanged(EventArgs e)
        {
            if (onCodeValueChanged != null)
                onCodeValueChanged(this, e);
        }
        #endregion

        #region control events
        private void BoxAppearanceChanged(object sender, EventArgs e)
        {
            //SyncListWindowProperties();
        }

        private void InputEditorKeyPress(object sender, KeyPressEventArgs e)
        {
            // 按Tab键时，如果输入框中的内容改变过，则清空当前保留的代码等数据，否则不变化
            if ((e.KeyChar == (char)9) && (Text.Trim() != DisplayValue))
            {
                CleanupPropertiesValue();
            }
            // 按回车时，如果输入框中的内容改变过，则根据情况来处理，否则不变化
            else if ((e.KeyChar == (char)13) || (e.KeyChar == (char)10))
            {
                string searchText = Text.Trim();
                if ((searchText != DisplayValue) || (DisplayValue.Length == 0))
                {
                    // 不为空或必填时，直接显示ShowListWindow， 否则清空当前保留的代码等数据
                    if ((Text.Trim().Length > 0) || (MinCount > 0))
                        SelectRecord(searchText, ShowListCallType.Normal);
                    else
                        CleanupPropertiesValue();
                }

                // 根据设置决定是否自动跳转到下一控件
                if (EnterMoveNextControl)
                {
                    ProcessDialogKey(Keys.Tab);
                    e.Handled = true;
                }
            }

        }

        private void InputEditorKeyDown(object sender, KeyEventArgs e)
        {
            // 按Down键时，直接弹出选择窗口
            if (!e.Alt && !e.Control && !e.Shift && (e.KeyCode == Keys.Down))
            {
                DoShowWindowDirectly(Text.Trim());
                e.Handled = true;
            }
        }

        private void InputEditorLeave(object sender, EventArgs e)
        {
            // 失去焦点时如果输入框中的内容改变过，则清空当前保留的代码等数据，否则不变化
            if (Text.Trim() != DisplayValue)
            {
                CleanupPropertiesValue();
            }
        }

        private void InputEditorEnter(object sender, EventArgs e)
        {
            // 默认全选
            SelectAll();
            // 进入控件时判断是否要直接显示ShowListWindow
            if ((ShowFormImmediately) && (!ReadOnly)
                && CodeValue.Length != 0//bwj 20121011加 目的解决为默认焦点项时不展示
                )
            {
                DoShowWindowDirectly(CodeValue);
                //if (HadGetValue && EnterMoveNextControl && (_listWindow.HadGetValue || (!KeepOriginalData)))
                //{
                //   // 如果是自动跳转到下一控件，则在选择窗口关闭后，发送一个回车
                //   SendKeys.Send("{ENTER}");
                //}
            }
        }

        private void ShowListButtonClick(object sender, EventArgs e)
        {
            DoShowWindowDirectly(Text.Trim());
        }
        #endregion

        #region 改写的 ButtonEdit 方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        protected virtual void NotifyButtonStateChanged(EditorButton button)
        {
            base.AccessibilityNotifyClients(AccessibleEvents.StateChange, button.Index + 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonInfo"></param>
        protected internal virtual void OnClickButton(EditorButtonObjectInfoArgs buttonInfo)
        {
            ShowListButtonClick(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((!e.Handled && EditorContainsFocus) && base.Enabled)
            {
                for (int num1 = 0; num1 < Properties.Buttons.Count; num1++)
                {
                    EditorButton button1 = Properties.Buttons[num1];
                    EditorButtonObjectInfoArgs args1 = ViewInfo.ButtonInfoByButton(button1);
                    if ((((args1 != null) && button1.Visible) && (button1.Enabled && button1.Shortcut.IsExist)) && (button1.Shortcut.Key == e.KeyData))
                    {
                        e.Handled = true;
                        OnClickButton(args1);
                    }
                }
            }
            base.OnKeyDown(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (base.Parent != null)
            {
                base.UpdateViewInfoState();
                if (e.Button == MouseButtons.Left)
                {
                    EditHitInfo info1 = ViewInfo.CalcHitInfo(new Point(e.X, e.Y));
                    if (((info1.HitTest == EditHitTest.Button) && AllowButtonPress) && ViewInfo.CanPress(info1))
                    {
                        ViewInfo.PressedInfo = info1;
                        RefreshVisualLayout();
                        OnPressButton(info1.HitObject as EditorButtonObjectInfoArgs);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            base.UpdateViewInfoState();
            if (e.Button == MouseButtons.Left)
            {
                EditHitInfo info1 = ViewInfo.CalcHitInfo(new Point(e.X, e.Y));
                EditHitInfo info2 = ViewInfo.PressedInfo;
                ClearHotPressed();
                if ((info2 != null) && (info2.HitTest == EditHitTest.Button))
                {
                    NotifyButtonStateChanged((info2.HitObject as EditorButtonObjectInfoArgs).Button);
                }
                if ((AllowButtonPress && !info1.IsEmpty) && (ViewInfo.CompareHitInfo(info1, info2) && (info1.HitTest == EditHitTest.Button)))
                {
                    OnClickButton(info1.HitObject as EditorButtonObjectInfoArgs);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buttonInfo"></param>
        protected virtual void OnPressButton(EditorButtonObjectInfoArgs buttonInfo)
        {
            //Properties.RaiseButtonPressed(new ButtonPressedEventArgs(buttonInfo.Button));
            NotifyButtonStateChanged(buttonInfo.Button);
        }
        #endregion

        #region 开关 用于控制是否需要重绘  默认false Add by  xlb 2013-04-24

        /// <summary>
        /// 是否需要绘制底部线条默认false
        /// </summary>
        private bool _isNeedPaint = false;
        /// <summary>
        /// IsNeedPaint
        /// </summary>
        [Category("扩展属性"), Description("是否需要绘制底部线条"), DefaultValue(false)]

        public bool IsNeedPaint
        {
            get
            {
                return _isNeedPaint;
            }
            set
            {
                _isNeedPaint = value;
            }
        }

        /// <summary>
        /// 重写重绘事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                base.OnPaint(e);
                if (IsNeedPaint)
                {
                    e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(0, this.Height - 2), new Point(this.Width, this.Height - 2));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }
}
