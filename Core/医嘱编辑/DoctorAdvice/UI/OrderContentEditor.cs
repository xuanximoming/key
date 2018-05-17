using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DrectSoft.Common.Eop;
using Eop = DrectSoft.Common.Eop;
using DrectSoft.Common;
using System.Globalization;
using DrectSoft.Resources;
using DrectSoft.Wordbook;
using DevExpress.Utils;

namespace DrectSoft.Core.DoctorAdvice {
    /// <summary>
    /// 医嘱内容编辑器
    /// </summary>
    internal partial class OrderContentEditor : XtraUserControl {
        #region const
        /// <summary>
        /// 紧凑模式下的最小宽度
        /// </summary>
        private const int MinCompactWidth = 570;
        /// <summary>
        /// 正常模式下的最小宽度
        /// </summary>
        private const int MinNormalWidth = 770;
        #endregion

        #region properties
        /// <summary>
        /// 当前编辑的医嘱内容对象
        /// </summary>
        public OrderContent NewOrderContent {
            get {
                _tempContent = OrderContentFactory.CreateOrderContent(ContentKind, null);

                _tempContent.BeginInit();
                // 绑定创建输出内容的委托
                _tempContent.ProcessCreateOutputeInfo =
                   new OrderContent.GenerateOutputInfo(CustomDrawOperation.CreateOutputeInfo);

                // 需要根据医嘱内容的类型保存相应数据
                _tempContent.Item = Item;
                _tempContent.ItemFrequency = Frequency;
                _tempContent.ItemUsage = Usage;
                _tempContent.Amount = Amount;
                _tempContent.CurrentUnit = Unit;

                // 出院带药的附加信息
                if (OutDrugContent != null) {
                    OutDrugContent.ExecuteDays = ExecuteDays;
                    //OutDrugContent.TotalAmount = TotalAmount;
                    OutDrugContent.ReCalcTotalAmount();
                }
                else if (OperationContent != null) // 手术时间
            {
                    OperationContent.OperationTime = ExtraDatetime;
                    if (AnesthesiaOperation != null)
                        OperationContent.AnesthesiaOperation = AnesthesiaOperation;
                }
                else if (ShiftDeptContent != null) // 转往科室和病区
            {
                    ShiftDeptContent.ShiftDept = ShiftDept;
                    ShiftDeptContent.ShiftWard = ShiftWard;
                }
                else if (LeaveHospitalContent != null) // 出院时间
            {
                    LeaveHospitalContent.LeaveHospitalTime = ExtraDatetime;
                }
                else {
                    _tempContent.EntrustContent = EntrustContent;
                }
                _tempContent.EndInit();

                if (ckSelfProvide.Visible) {
                    if (ckSelfProvide.Checked)
                        _tempContent.Attributes |= OrderAttributeFlag.Provide4Oneself;
                    else
                        _tempContent.Attributes &= (~OrderAttributeFlag.Provide4Oneself);
                }
                return _tempContent;
            }
        }

        /// <summary>
        /// 出院带药执行天数或长期医嘱执行天数
        /// </summary>
        public int ExecuteDays {
            get {

                if (panelDays.Visible)
                    return Convert.ToInt32(edtDays.Value);
                else
                    return 0;
            }
            set {
                edtDays.Value = value;
            }
        }

        /// <summary>
        /// 医嘱开始时间
        /// </summary>
        public DateTime StartDateTime {
            get {
                return dateEditStart.DateTime.Date + timeEditStart.Time.TimeOfDay;
            }
            set {
                dateEditStart.DateTime = value;
                timeEditStart.Time = value;
            }
        }

        /// <summary>
        /// 合适的尺寸
        /// </summary>
        public Size SuitablySize {
            get {
                //int width;
                //int height;
                //if (m_CompactMode)
                //{
                //   //if (_useRadioCatalogInputStyle)
                //      width = panelItem.Width; // panelAmount.Width + panelUsage.Width + panelFrequency.Width
                //   //else
                //   //   width = panelCatalogInput.Width + panelItem.Width;
                //   height = panelTop.Height + panelClient.Height + panelClient2.Height + panelBottom.Height;
                //}
                //else
                //{
                //   width = panelClient.Width;
                //   height = panelTop.Height + panelClient.Height + panelBottom.Height;
                //}
                //return new Size(width, height);

                return new Size(SuitablyWidth, SuitablyHeight);
            }
        }

        /// <summary>
        /// 合适的宽度
        /// </summary>
        public int SuitablyWidth {
            get {
                if (m_CompactMode)
                    return MinCompactWidth; // panelCatalogInput.Width + panelItem.Width;
                else
                    return MinNormalWidth; // panelTop.Width;
            }
        }

        /// <summary>
        /// 合适的高度
        /// </summary>
        public int SuitablyHeight {
            get {
                if (m_CompactMode)
                    return panelTop.Height + panelClient.Height + panelClient2.Height + panelBottom.Height;
                else
                    return panelTop.Height + panelClient.Height + panelBottom.Height;
            }
        }

        /// <summary>
        /// 标记当前是在修改数据还是新增医嘱
        /// </summary>
        public bool IsModifyData {
            get { return _isModifyData; }
            set {
                _isModifyData = value;
                btnInsert.Enabled = !value;
                if (value)
                    FireStartEdit(new DataCommitArgs(DataCommitType.Modify));
                else
                    FireStartEdit(new DataCommitArgs(DataCommitType.Add));

                //SetShowFormImmediatelyProperty(value);
            }
        }
        private bool _isModifyData;

        /// <summary>
        /// 医嘱类别输入模式。Fasle: 使用LookUpEditor  True: 使用单选框模式
        /// </summary>
        public bool UseRadioCatalogInputStyle {
            get { return _useRadioCatalogInputStyle; }
            set {
                if (_useRadioCatalogInputStyle != value) {
                    _useRadioCatalogInputStyle = value;
                    ChangeCatalogInputStyle();
                    Size = SuitablySize;
                }
            }
        }
        private bool _useRadioCatalogInputStyle;
        #endregion

        #region private properties
        /// <summary>
        /// 出院带药医嘱内容对象(提供此属性，便于数据访问)
        /// </summary>
        private OutDruggeryContent OutDrugContent {
            get { return _tempContent as OutDruggeryContent; }
        }

        /// <summary>
        /// 手术医嘱内容对象(提供此属性，便于数据访问)
        /// </summary>
        private OperationOrderContent OperationContent {
            get { return _tempContent as OperationOrderContent; }
        }

        /// <summary>
        /// 转科医嘱内容对象(提供此属性，便于数据访问)
        /// </summary>
        private ShiftDeptOrderContent ShiftDeptContent {
            get { return _tempContent as ShiftDeptOrderContent; }
        }

        /// <summary>
        /// 出院医嘱内容对象(提供此属性，便于数据访问)
        /// </summary>
        private LeaveHospitalOrderContent LeaveHospitalContent {
            get { return _tempContent as LeaveHospitalOrderContent; }
        }

        /// <summary>
        /// 医嘱类别
        /// </summary>
        private OrderContentKind ContentKind {
            get {
                if (UseRadioCatalogInputStyle) {
                    if (ckEdtDruggery.Checked)
                        return OrderContentKind.Druggery;
                    else if (ckEdtNormalItem.Checked)
                        return OrderContentKind.ChargeItem;
                    else if (ckEdtGeneralItem.Checked)
                        return OrderContentKind.GeneralItem;
                    else if (ckEdtOperate.Checked)
                        return OrderContentKind.Operation;
                    else if (ckEdtText.Checked)
                        return OrderContentKind.TextNormal;
                    else if (ckEdtShiftDept.Checked)
                        return OrderContentKind.TextShiftDept;
                    else if (ckEdtLeaveHosp.Checked)
                        return OrderContentKind.TextLeaveHospital;
                    else if (ckEdtOutDruggery.Checked)
                        return OrderContentKind.OutDruggery;
                }
                else {
                    if (catalogInput.HadGetValue)
                        return (OrderContentKind)Convert.ToInt32(catalogInput.CodeValue);
                }

                return OrderContentKind.Druggery; //默认为药品，以便于后面按药品医嘱进行处理
            }
            set {
                if (catalogInput.SqlWordbook != null) {
                    catalogInput.CodeValue = value.ToString("D");
                    switch (value) {
                        case OrderContentKind.Druggery:
                            ckEdtDruggery.Checked = true;
                            break;
                        case OrderContentKind.ChargeItem:
                            ckEdtNormalItem.Checked = true;
                            break;
                        case OrderContentKind.GeneralItem:
                            ckEdtGeneralItem.Checked = true;
                            break;
                        case OrderContentKind.Operation:
                            ckEdtOperate.Checked = true;
                            break;
                        case OrderContentKind.TextNormal:
                            ckEdtText.Checked = true;
                            break;
                        case OrderContentKind.TextShiftDept:
                            ckEdtShiftDept.Checked = true;
                            break;
                        case OrderContentKind.TextLeaveHospital:
                            ckEdtLeaveHosp.Checked = true;
                            break;
                        case OrderContentKind.OutDruggery:
                            ckEdtOutDruggery.Checked = true;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 医嘱项目
        /// </summary>
        private ItemBase Item {
            get {
                if (((m_EnableFlags & EditorEnableFlag.CanChoiceItem) > 0)
                   && itemInput.HadGetValue && (itemInput.PersistentObjects.Count > 0))
                    return itemInput.PersistentObjects[0] as ItemBase;
                else
                    return null;
            }
        }

        /// <summary>
        /// 医嘱频次
        /// </summary>
        private OrderFrequency Frequency {
            get {
                // 临时医嘱中除出院带药，频次都会默认为ST
                if ((m_UILogic.IsTempOrder && (ContentKind != OrderContentKind.OutDruggery))
                      || ((m_EnableFlags & EditorEnableFlag.CanChoiceFrequency) > 0))
                    return frequencyInputor.Frequency;
                else
                    return null;
            }
        }

        /// <summary>
        /// 医嘱用法
        /// </summary>
        private OrderUsage Usage {
            get {
                if (((m_EnableFlags & EditorEnableFlag.CanChoiceUsage) > 0)
                   && usageInput.HadGetValue && (usageInput.PersistentObjects.Count > 0))
                    return usageInput.PersistentObjects[0] as OrderUsage;
                else
                    return null;
            }
        }

        /// <summary>
        /// 项目数量
        /// </summary>
        private decimal Amount {
            get {
                return Convert.ToDecimal(edtAmount.Value);
            }
        }

        /// <summary>
        /// 数量单位
        /// </summary>
        private ItemUnit Unit {
            get {
                if (selectUnit.SelectedItem != null)
                    return (ItemUnit)selectUnit.SelectedItem;
                else
                    return new ItemUnit();
            }
        }

        ///// <summary>
        ///// 出院带药的总数量
        ///// </summary>
        //private decimal TotalAmount
        //{
        //   get
        //   {
        //      return Amount * Frequency.ExecuteTimesPerDay * ExecuteDays;
        //   }
        //}

        /// <summary>
        /// 获取完整的附加时间信息（用作手术或出院时间）
        /// </summary>
        private DateTime ExtraDatetime {
            get {
                if (((m_EnableFlags & EditorEnableFlag.NeedOperationInfo) > 0)
                   || ((m_EnableFlags & EditorEnableFlag.NeedLeaveHospitalTime) > 0))
                    return dateEditExtra.DateTime.Date + timeEditExtra.Time.TimeOfDay;
                else
                    return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 转科科室
        /// </summary>
        private Eop.Department ShiftDept {
            get {
                if (((m_EnableFlags & EditorEnableFlag.NeedShiftDeptInfo) > 0)
                   && deptInput.HadGetValue && (deptInput.PersistentObjects.Count > 0))
                    return deptInput.PersistentObjects[0] as Eop.Department;
                else
                    return null;
            }
        }

        /// <summary>
        /// 转科病区
        /// </summary>
        private Eop.Ward ShiftWard {
            get {
                if (((m_EnableFlags & EditorEnableFlag.NeedShiftDeptInfo) > 0)
                   && wardInput.HadGetValue && (wardInput.PersistentObjects.Count > 0))
                    return wardInput.PersistentObjects[0] as Eop.Ward;
                else
                    return null;
            }
        }

        /// <summary>
        /// 麻醉方法
        /// </summary>
        private Eop.Anesthesia AnesthesiaOperation {
            get {
                if (((m_EnableFlags & EditorEnableFlag.NeedOperationInfo) > 0)
                   && anesthesiaInput.HadGetValue && (anesthesiaInput.PersistentObjects.Count > 0))
                    return anesthesiaInput.PersistentObjects[0] as Eop.Anesthesia;
                else
                    return null;
            }
        }

        /// <summary>
        /// 嘱托（对于手术、出院、转科无效）
        /// </summary>
        private string EntrustContent {
            get {
                if ((m_EnableFlags & EditorEnableFlag.CanInputEntrust) > 0)
                    return edtEntrustContent.Text;
                else
                    return "";
            }
        }
        #endregion

        #region private variables
        private UILogic m_UILogic;
        /// <summary>
        /// 编辑框是否可用标志
        /// </summary>
        private EditorEnableFlag m_EnableFlags;

        private IDataAccess m_SqlExecutor;

        /// <summary>
        /// 缓存由外部传入的医嘱内容对象
        /// </summary>
        private OrderContent _tempContent;

        private Druggery m_AllergicDrug;

        private SkinTestInputForm m_SkinTestForm;

        /// <summary>
        /// 保存唯一可供选择的频次，为空则表示有多个频次可供选择
        /// </summary>
        private string m_OnlyFrequency;

        /// <summary>
        /// 紧凑模式
        /// </summary>
        private bool m_CompactMode;
        #endregion

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="useCompactMode">是否使用紧凑模式</param>
        public OrderContentEditor(bool useCompactMode) {
            InitializeComponent();


            //btnNew.Image = ResourceManager.GetSmallIcon(ResourceNames.NewDocument, IconType.Normal);
            //btnOk.Image = ResourceManager.GetSmallIcon(ResourceNames.Ok, IconType.Normal);
            //btnInsert.Image = ResourceManager.GetSmallIcon(ResourceNames.AddToList, IconType.Normal);

            m_SkinTestForm = new SkinTestInputForm();
            m_SkinTestForm.HadMadeChoice += new EventHandler(DoAfterChoiceSkinTestResult);

            itemInput.EnterMoveNextControl = false; // 选定项目后再决定是否自动跳转到下一控件

            m_CompactMode = useCompactMode;
            InitializePanelsOfClient();

            btnCommit.Size = new Size(0, 0);
            Size = SuitablySize;
        }
        #endregion

        #region interface methods
        /// <summary>
        /// 初始化医嘱内容编辑器
        /// </summary>
        /// <param name="logicHandle">逻辑处理对象</param>
        /// <param name="sqlExecutor">数据访问对象</param>
        public void InitializeEditor(UILogic logicHandle, IDataAccess sqlExecutor) {
            if (logicHandle == null)
                throw new ArgumentNullException(String.Format(ConstMessages.ExceptionFormatNullObject, ConstNames.OrderUILogic));
            m_UILogic = logicHandle;
            m_SqlExecutor = sqlExecutor;
            showListWindow1.SqlHelper = sqlExecutor;

            this.usageInput.NormalWordbook = new DrectSoft.Wordbook.OrderUsageBook("ID//Py//Wb", 0, "记录状态//1", "", 0);
            this.wardInput.NormalWordbook = new DrectSoft.Wordbook.WardBook("ID//Py//Wb", 0, "记录状态//1", "", 0);
            this.deptInput.NormalWordbook = new DrectSoft.Wordbook.DepartmentBook("ID//Py//Wb", 0, "记录状态//1//科室类别//101", "", 0);
            this.anesthesiaInput.NormalWordbook = new DrectSoft.Wordbook.AnesthesiaBook("ID//Py//Wb//Name", 0, "记录状态//1", "", 0);

            deptInput.NormalWordbook.Parameters[ConstSchemaNames.DeptWardMapColExceptDept].Enabled = true;
            deptInput.NormalWordbook.Parameters[ConstSchemaNames.DeptWardMapColExceptDept].Value = logicHandle.DeptCode;

            wardInput.NormalWordbook.Parameters[ConstSchemaNames.DeptWardMapColExceptWard].Enabled = true;
            wardInput.NormalWordbook.Parameters[ConstSchemaNames.DeptWardMapColExceptWard].Value = logicHandle.WardCode;
        }

        /// <summary>
        /// 重设医嘱内容选择和默认频次选择的数据。
        /// 在对于的医嘱数据源发生变化，或需要处理特殊逻辑(比如出院带药)时调用
        /// </summary>
        public void ResetDataOfWordbook() {
            // 设置医嘱内容类别选择列表的字典类
            catalogInput.SqlWordbook = m_UILogic.CatalogWordbook;
            ResetCatalogChoiceGroup();

            // 因为医嘱频次与医嘱类型有关，所以由逻辑处理对象提供
            frequencyInputor.FrequencyBook = m_UILogic.FrequencyWordbook;
            if (m_UILogic.IsTempOrder) {
                string[] defFrequency = showListWindow1.ValidateWordbookHasOneRecord(frequencyInputor.FrequencyBook, WordbookKind.Normal);
                if (defFrequency != null)
                    m_OnlyFrequency = defFrequency[0];
                else
                    m_OnlyFrequency = "";
            }
            else
                m_OnlyFrequency = "";
        }

        /// <summary>
        /// 用传入的变量初始化医嘱内容编辑界面
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="content"></param>
        public void InitializeDefaultValue(DateTime startDateTime, OrderContent content) {
            if (startDateTime == DateTime.MinValue)
                StartDateTime = m_UILogic.DefaultStartDateTime;
            else
                StartDateTime = startDateTime;

            _tempContent = content;
            if (content == null) {
                if (catalogInput.HadGetValue) // 原先已有分类则保留
            {
                    string catalogCode = catalogInput.CodeValue.ToString();
                    ClearComponentsValue();
                    //catalogInput.CodeValue = catalogCode;
                    ContentKind = (OrderContentKind)Convert.ToInt32(catalogCode);
                    if (itemInput.CanFocus)
                        itemInput.Focus();
                }
                else {
                    //catalogInput.CodeValue = OrderContentKind.Druggery.ToString("D");
                    ContentKind = OrderContentKind.Druggery;
                    if (catalogInput.CanFocus)
                        catalogInput.Focus();
                }
            }
            else {
                // 用医嘱对象给相应属性赋值
                //catalogInput.CodeValue = content.OrderKind.ToString("D");
                ContentKind = content.OrderKind;

                if (content.Item != null)
                    itemInput.CodeValue = content.Item.KeyValue;
                else
                    itemInput.CodeValue = null;

                edtAmount.Value = content.Amount;
                if (content.CurrentUnit.Name != null)
                    selectUnit.SelectedIndex = selectUnit.Properties.Items.IndexOf(
                       content.CurrentUnit);
                else
                    selectUnit.SelectedIndex = -1;

                if (OutDrugContent != null) // 出院带药要输入天数 
                    edtDays.Value = OutDrugContent.ExecuteDays;
                else if (OperationContent != null) {
                    DateTime opDateTime = OperationContent.OperationTime;
                    dateEditExtra.DateTime = opDateTime.Date;
                    timeEditExtra.Time = opDateTime.Date;
                    anesthesiaInput.CodeValue = OperationContent.AnesthesiaOperation.Code;
                }
                else if (LeaveHospitalContent != null) {
                    DateTime leaveTime = LeaveHospitalContent.LeaveHospitalTime;
                    dateEditExtra.DateTime = leaveTime.Date;
                    timeEditExtra.Time = leaveTime.Date;
                }
                else if (ShiftDeptContent != null) // 转科要设置转往的科室、病区
            {
                    deptInput.CodeValue = ShiftDeptContent.ShiftDept.Code;
                    wardInput.CodeValue = ShiftDeptContent.ShiftWard.Code;
                }

                edtEntrustContent.Text = content.EntrustContent;
                // 用法、频次一定要在项目赋值后进行，以免被项目的默认设置冲掉
                if (content.ItemUsage != null)
                    usageInput.CodeValue = content.ItemUsage.Code;
                else
                    usageInput.CodeValue = "";

                frequencyInputor.Frequency = content.ItemFrequency;
                //_tempContent.ItemFrequency = content.ItemFrequency.Clone();

                ckSelfProvide.Checked = ((content.Attributes & OrderAttributeFlag.Provide4Oneself) > 0);

                itemInput.Focus();
            }
            IsModifyData = false;
            SetShowFormImmediatelyProperty(false);
        }

        /// <summary>
        /// 编辑指定医嘱的内容或以指定医嘱对象为基础编辑新增医嘱的内容
        /// </summary>
        /// <param name="startDateTime">开始时间</param>
        /// <param name="content">医嘱内容</param>
        public void EditContent(DateTime startDateTime, OrderContent content) {
            InitializeDefaultValue(startDateTime, content);
            IsModifyData = true;
            SetShowFormImmediatelyProperty(true);
        }

        /// <summary>
        /// 编辑指定医嘱的内容或以指定医嘱对象为基础编辑新增医嘱的内容
        /// </summary>
        /// <param name="order">医嘱对象实例</param>
        /// <param name="isTempOrder">ture:临时医嘱；false:长期医嘱</param>
        public void EditContent(Order order, bool isTempOrder) {
            InitializeDefaultValue(order.StartDateTime, order.Content);
            if (!isTempOrder) {
                LongOrder longOrder = order as LongOrder;
                if ((longOrder.CeaseInfo != null) && (longOrder.CeaseInfo.HadInitialized)) {
                    TimeSpan days = longOrder.CeaseInfo.ExecuteTime - longOrder.StartDateTime;
                    ExecuteDays = (int)days.TotalDays;
                }
            }
            IsModifyData = true;
            SetShowFormImmediatelyProperty(true);
        }

        /// <summary>
        /// 向医嘱内容编辑组件传入Form截获的功能键，以触发特定的事件
        /// </summary>
        /// <param name="e"></param>
        public void AcceptFunctionKeyPress(KeyEventArgs e) {
            if ((!e.Control) && (!e.Shift)) {
                if (e.KeyCode == Keys.F5)
                    btnOk_Click(this, new EventArgs());
                else if (e.KeyCode == Keys.F6) {
                    if (btnInsert.Enabled)
                        btnInsert_Click(this, new EventArgs());
                }
            }
        }

        //public void ResetControlFont(Font newFont)
        //{
        //   Font = newFont;
        //   styleController.Appearance.Font = newFont;
        //   styleController.AppearanceDisabled.Font = newFont;
        //   styleController.AppearanceDropDown.Font = newFont;
        //   styleController.AppearanceDropDownHeader.Font = newFont;
        //   styleController.AppearanceFocused.Font = newFont;
        //   groupControl1.Appearance.Font = newFont;
        //   groupControl1.AppearanceCaption.Font = newFont;
        //   groupControl2.Appearance.Font = newFont;
        //   groupControl2.AppearanceCaption.Font = newFont;
        //   catalogInput.Font = newFont;
        //   itemInput.Font = newFont;
        //   usageInput.Font = newFont;
        //   frequencyInput.Font = newFont;
        //   deptInput.Font = newFont;
        //   wardInput.Font = newFont;
        //   anesthesiaInput.Font = newFont;

        //   Width = panelItem.Width;
        //}
        #endregion

        #region custom event handler
        /// <summary>
        /// 数据编辑开始事件
        /// </summary>
        public event EventHandler<DataCommitArgs> StartEdit {
            add {
                onStartEdit = (EventHandler<DataCommitArgs>)Delegate.Combine(onStartEdit, value);
            }
            remove {
                onStartEdit = (EventHandler<DataCommitArgs>)Delegate.Remove(onStartEdit, value);
            }
        }
        private EventHandler<DataCommitArgs> onStartEdit;

        private void FireStartEdit(DataCommitArgs e) {
            if (onStartEdit != null)
                onStartEdit(this, e);
        }

        /// <summary>
        /// 数据编辑完成事件
        /// </summary>
        public event EventHandler<DataCommitArgs> EditFinished {
            add {
                onEditFinished = (EventHandler<DataCommitArgs>)Delegate.Combine(onEditFinished, value);
            }
            remove {
                onEditFinished = (EventHandler<DataCommitArgs>)Delegate.Remove(onEditFinished, value);
            }
        }
        private EventHandler<DataCommitArgs> onEditFinished;

        private void FireDataEditFinished(DataCommitArgs e) {
            FireSelectedItemChanged(false);
            if (onEditFinished != null)
                onEditFinished(this, e);
        }

        /// <summary>
        /// 选中的项目发生改变事件
        /// </summary>
        public event EventHandler<OrderItemArgs> SelectedItemChanged {
            add {
                onSelectedItemChanged = (EventHandler<OrderItemArgs>)Delegate.Combine(onSelectedItemChanged, value);
            }
            remove {
                onSelectedItemChanged = (EventHandler<OrderItemArgs>)Delegate.Remove(onSelectedItemChanged, value);
            }
        }
        private EventHandler<OrderItemArgs> onSelectedItemChanged;

        private void FireSelectedItemChanged(bool selectedItem) {
            if ((onSelectedItemChanged != null) && (Item != null)) {
                string usage;
                if (Usage != null)
                    usage = Usage.Name;
                else
                    usage = "";

                onSelectedItemChanged(this, new OrderItemArgs(selectedItem, Item.Kind, Item.KeyValue, Item.Name, Unit.Name, usage));
            }
        }
        #endregion

        #region private methods
        private void InitializePanelsOfClient() {
            // 调整Panel的属性
            panelTop.Height = panelAmount.Height;
            panelClient.Height = panelTop.Height;
            panelClient2.Height = panelClient.Height;

            panelAmount.Dock = DockStyle.Left;
            panelUsage.Dock = DockStyle.Left;
            panelFrequency.Dock = DockStyle.Left;
            panelDays.Dock = DockStyle.Left;
            panelExtraTime.Dock = DockStyle.Left;
            panelShiftDept.Dock = DockStyle.Left;
            panelOperation.Dock = DockStyle.Left;
            panelEntrust.Dock = DockStyle.Fill;

            //panelTop.Location = new Point(0, 0);
            //panelClient.Location = new Point(0, panelTop.Height);
            //panelClient2.Location = new Point(0, panelTop.Height * 2);

            panelCatalogChoice.Location = new Point(0, 0);
            panelCatalogChoice.Width = panelTop.Width - panelButton.Width;

            if (m_CompactMode) {
                panelStartTime.Visible = false;
                panelClient2.Controls.Add(panelEntrust);
                panelClient2.Controls.Add(panelDays);
                panelBottom.Height = panelCatalogChoice.Height + panelButton.Height;
                panelButton.Location = new Point(MinCompactWidth - panelButton.Width //panelCatalogInput.Width + panelItem.Width - panelButton.Width
                   , panelCatalogChoice.Height);
            }
            else {
                panelBottom.Height = panelButton.Height;
                panelButton.Location = new Point(panelCatalogChoice.Width, 0);
                panelClient2.Visible = false;
            }
        }

        /// <summary>
        /// 清空界面控件的值(除医嘱内容类别选择框)
        /// </summary>
        private void ClearComponentsValue() {
            itemInput.CodeValue = null;
            // --下面这段清除内容在itemInput的改变事件中会处理
            //edtPrice.Text = "";
            //edtSpecification.Text = "";
            //edtAmount.Value = 1;
            //selectUnit.Properties.Items.Clear();
            //usageInput.CodeValue = null;
            //frequencyInput.CodeValue = null;
            //edtDays.Value = 1;

            //edtTotal.Text = ""; // --总数量的内容会自动生成

            deptInput.CodeValue = "";
            wardInput.CodeValue = "";
            dateEditExtra.DateTime = DateTime.Now;
            timeEditExtra.Time = DateTime.Now;

            edtEntrustContent.Text = "";
            ckSelfProvide.Checked = false;
        }

        private void ChangeCatalogInputStyle() {
            panelCatalogChoice.Visible = UseRadioCatalogInputStyle;
            panelCatalogInput.Visible = !UseRadioCatalogInputStyle;

            // 需要手工处理项目框的宽度
            int plugWidth;

            if (UseRadioCatalogInputStyle)
                plugWidth = panelCatalogInput.Width;
            else
                plugWidth = -panelCatalogInput.Width;
            itemInput.Width += plugWidth;
            edtItemMemo.Location = new Point(edtItemMemo.Location.X + plugWidth, edtItemMemo.Location.Y);
        }

        private void ResetCatalogChoiceGroup() {
            DataTable catalogTable = catalogInput.SqlWordbook.BookData;
            catalogTable.DefaultView.RowFilter = catalogInput.SqlWordbook.ExtraCondition;

            foreach (Control ctrl in panelCatalogChoice.Controls)
                ctrl.Visible = false;

            OrderContentKind kind;
            foreach (DataRowView view in catalogTable.DefaultView) {
                kind = (OrderContentKind)Convert.ToInt32(view[ConstSchemaNames.ContentCatalogColId]);
                switch (kind) {
                    case OrderContentKind.Druggery:
                        ckEdtDruggery.Visible = true;
                        break;
                    case OrderContentKind.ChargeItem:
                        ckEdtNormalItem.Visible = true;
                        break;
                    case OrderContentKind.GeneralItem:
                        ckEdtGeneralItem.Visible = true;
                        break;
                    case OrderContentKind.Operation:
                        ckEdtOperate.Visible = true;
                        break;
                    case OrderContentKind.TextNormal:
                        ckEdtText.Visible = true;
                        break;
                    case OrderContentKind.TextShiftDept:
                        ckEdtShiftDept.Visible = true;
                        break;
                    case OrderContentKind.TextLeaveHospital:
                        ckEdtLeaveHosp.Visible = true;
                        break;
                    case OrderContentKind.OutDruggery:
                        ckEdtOutDruggery.Visible = true;
                        break;
                }
            }

            // 默认选中第一个类别
            (panelCatalogChoice.Controls[0] as CheckEdit).Checked = true;
        }

        private void SetVisiableAndEnable() {
            panelItem.Visible = ((m_EnableFlags & EditorEnableFlag.CanChoiceItem) > 0);
            panelAmount.Visible = ((m_EnableFlags & EditorEnableFlag.NeedInputAmount) > 0);
            panelUsage.Visible = ((m_EnableFlags & EditorEnableFlag.CanChoiceUsage) > 0);
            panelFrequency.Visible = ((m_EnableFlags & EditorEnableFlag.CanChoiceFrequency) > 0);
            if (panelFrequency.Visible)
                panelFrequency.Enabled = (String.IsNullOrEmpty(m_OnlyFrequency));

            panelDays.Visible = (((m_EnableFlags & EditorEnableFlag.CanSetExecuteDays) > 0)
               && ((!m_UILogic.IsTempOrder) || ((m_EnableFlags & EditorEnableFlag.NeedOutDruggeryInfo) > 0)));

            if ((m_EnableFlags & EditorEnableFlag.NeedOutDruggeryInfo) > 0) {
                panelTotal.Visible = true;

            }
            else {
                panelTotal.Visible = false;
            }

            panelExtraTime.Visible = false;
            if ((m_EnableFlags & EditorEnableFlag.NeedShiftDeptInfo) > 0) {
                panelShiftDept.Visible = true;
                labelExtraTime.Text = ConstNames.TimeOfShiftDept;
            }
            else
                panelShiftDept.Visible = false;
            if ((m_EnableFlags & EditorEnableFlag.NeedOperationInfo) > 0) {
                panelOperation.Visible = true;
                panelExtraTime.Visible = true;
                labelExtraTime.Text = ConstNames.TimeOfOperation;
            }
            else
                panelOperation.Visible = false;
            if ((m_EnableFlags & EditorEnableFlag.NeedLeaveHospitalTime) > 0) {
                panelExtraTime.Visible = true;
                labelExtraTime.Text = ConstNames.TimeOfOutHospital;
            }

            panelEntrust.Visible = ((m_EnableFlags & EditorEnableFlag.CanInputEntrust) > 0);

            ckSelfProvide.Visible = (((ContentKind == OrderContentKind.Druggery) || (ContentKind == OrderContentKind.OutDruggery))
               && (!m_CompactMode));
            ckSelfProvide.Checked = false;
        }

        private void SetShowFormImmediatelyProperty(bool isEdit) {
            //catalogInput.ShowFormImmediately = !isEdit;
            //itemInput.ShowFormImmediately = !isEdit;
            usageInput.ShowFormImmediately = !isEdit;
            frequencyInputor.ShowFormImmediately = !isEdit;
            deptInput.ShowFormImmediately = !isEdit;
            wardInput.ShowFormImmediately = !isEdit;
        }

        /// <summary>
        /// 医嘱内容类别改变后的处理函数
        /// </summary>
        private void SetValueAfterCatalogChanged() {
            // 类别改变后要完成以下事情：
            //    设置项目选择控件的字典类
            //    设置显示界面
            //    适当的设置控件默认值
            itemInput.NormalWordbook = m_UILogic.GetItemWordbook(ContentKind);
            m_EnableFlags = UILogic.GetContentEditorEnableStatus(ContentKind);

            SetVisiableAndEnable();

            ClearComponentsValue();
            if (ContentKind == OrderContentKind.TextLeaveHospital) {
                dateEditExtra.DateTime = m_UILogic.DefaultLeaveHospitalTime;
                timeEditExtra.Time = m_UILogic.DefaultLeaveHospitalTime;
            }
            // 输入文字医嘱时不一定要选项目
            if (ContentKind == OrderContentKind.TextNormal)
                itemInput.MinCount = 0;
            else
                itemInput.MinCount = 1;

            if (ContentKind == OrderContentKind.OutDruggery)
                edtDays.Value = 1;
            else
                edtDays.Value = 0; // 清空执行天数
        }

        /// <summary>
        /// 选中项目改变后，设置相关控件的值
        /// </summary>
        private void SetValueAfterItemChanged() {
            // 选中项目后，做以下工作：
            //    初始化规格、单价、单位列表(最小单位，如果是药品，则用规格和住院单位)
            //    药品、一般项目、临床项目保留上一次的频次；如果是药品，则保留用法
            //    对于药品，如果有默认用法/频次/数量/单位/天数，则赋值到相应控件(但不替换已有的频次、用法)
            //    对于药品要设置可选的用法范围

            // 必要的清理工作
            edtAmount.Value = 1; // 数量默认为1
            // edtDays.Value = 1; 执行天数和上一条保持一致
            selectUnit.Properties.Items.Clear();
            selectUnit2.Properties.Items.Clear();

            switch (ContentKind) {
                case OrderContentKind.Druggery:
                case OrderContentKind.OutDruggery:
                    break;
                case OrderContentKind.ChargeItem:
                case OrderContentKind.ClinicItem:
                    usageInput.CodeValue = null;
                    break;
                default:
                    usageInput.CodeValue = null;
                    frequencyInputor.Frequency = null;
                    break;
            }

            if (!String.IsNullOrEmpty(m_OnlyFrequency))
                frequencyInputor.SetFrequency(m_OnlyFrequency);
            else if ((m_UILogic.IsTempOrder) && (ContentKind != OrderContentKind.OutDruggery))
                frequencyInputor.SetFrequency(CoreBusinessLogic.BusinessLogic.TempOrderFrequencyCode);

            // 未选中项目，则清除单价和规格的显示内容
            if (Item == null) {
                edtItemMemo.Text = "";
                FireSelectedItemChanged(false);
            }
            else {
                // 下面是选中项目后的处理
                edtItemMemo.Text = Item.Price + " " + Item.Specification;

                // 添加单位列表
                // 如果是药品项目，还需进行其他处理
                Druggery drug = Item as Druggery;
                if (drug == null) {
                    selectUnit.Properties.Items.Add(Item.BaseUnit);
                    // 清除对用法的限制
                    usageInput.NormalWordbook.ExtraCondition = "";
                }
                else {
                    // 首先要取得药品的默认用法等信息（因为现在的模式是不自动从DB中获取未赋值的数据）
                    drug.InitializeDefaultUsageRange(m_SqlExecutor);

                    // 如果规格单位,或住院单位为空，则不将其加入到单位列表中
                    //modified by zhouhui  修改某些情况下没有默认单位的bug
                    if (!string.IsNullOrEmpty(drug.SpecUnit.Name))
                        selectUnit.Properties.Items.Add(drug.SpecUnit);

                    if (!string.IsNullOrEmpty(drug.WardUnit.Name)) {
                        selectUnit.Properties.Items.Add(drug.WardUnit);
                        selectUnit2.Properties.Items.Add(drug.WardUnit);
                        selectUnit2.SelectedIndex = 0;
                    }

                    // 根据剂型用法对应设置用法的选择范围
                    usageInput.NormalWordbook.ExtraCondition =
                       drug.DefaultUsageRangeCondition;

                    // 检查当前保留的用法是否在用法选择范围内（通过重新给代码属性赋值的方法实现）
                    usageInput.CodeValue = usageInput.CodeValue;

                    // 如果存在默认的用法、频次、数量等，且没有保留的用法、频次，则用默认值填充控件
                    // 现在默认取住院系统的默认设置，以后要根据当前使用的是门诊还是住院来定！！！
                    DruggeryOrderContent defaultContent = drug.GetDefaultUsageFrequency(
                       SystemApplyRange.InpatientDept, m_SqlExecutor);

                    if (defaultContent.Amount > 0) // 以默认数量来判断是否有默认的用法对应设置
               {
                        // 默认数量的单位是最小单位，要将其换算成当前单位的数量
                        edtAmount.Value = ((ItemUnit)selectUnit.Properties.Items[0]).Convert2CurrentUnit(
                           defaultContent.Amount);

                        // 设置默认用法
                        if (!usageInput.HadGetValue)
                            usageInput.CodeValue = defaultContent.ItemUsage.Code;
                        // 设置默认频次
                        if (!frequencyInputor.HadGetValue)
                            frequencyInputor.SetFrequency(defaultContent.ItemFrequency.Code);
                    }
                }
                selectUnit.SelectedIndex = 0; // 默认显示第一单位，药品的默认单位需要增加设置 ！！！
                FireSelectedItemChanged(true);
            }
        }

        ///// <summary>
        ///// 频次改变后，设置相关控件属性
        ///// </summary>
        //private void SetValueAfterFrequencyChanged()
        //{
        //   // 选择频次后，初始化用药时间选择框
        //   SetFrequencyDetail();
        //   // 出院带药时，如果数量/频次/天数有更新，则要刷新平均每天用量
        //   ResetOutDruggeryTotalAmout();
        //   // 出院带药时，如果数量/频次/总量有更新，则要刷新带药天数
        //   ResetOutDrugTotalAmount();
        //}

        /// <summary>
        /// 重设出院带药平均每天用量的显示
        /// </summary>
        private void ResetOutDruggeryTotalAmout() {
            if (ContentKind == OrderContentKind.OutDruggery) {
                Druggery drug = Item as Druggery;
                if (drug != null) {
                    // 算出平均每天要吃的量（相对于门诊单位）
                    decimal total = CalcTotalAmountOfAvgDay();
                    total = drug.PoliclinicUnit.Convert2CurrentUnit(Unit.Convert2BaseUnit(total));
                    int newTotal; // 对数值向上取整（是否要根据设置来定？？？）
                    newTotal = Decimal.ToInt32(total);
                    if (newTotal < total)
                        newTotal += 1;
                    edtTotal.Text = newTotal.ToString(CultureInfo.CurrentCulture) + Unit.Name;
                }
                else
                    edtTotal.Text = "";
            }
        }

        /// <summary>
        /// 计算出院带药平均每天用量(相对于当前选择的剂量单位)
        /// </summary>
        private decimal CalcTotalAmountOfAvgDay() {
            // 根据数量*频次计算得到（以后再加入草药贴数的处理！！！）
            return Amount * Frequency.ExecuteTimesPerDay;
        }

        private void CheckNeedSkinTestAfterSelectItem() {
            // 如果选择的是药品，则检查过敏信息
            //   是否是过敏药品
            //   如果连接HIS，则检查过敏记录中是否有相应记录，若无，则提示要做皮试，阳性则提示并取消输入，阴性则允许通过
            //   如果不连HIS，则提示输入过敏信息，只有选中阴性并确认才允许通过
            if (Item != null)
            //&& ((Item.Kind == ItemKind.WesternMedicine) || (Item.Kind == ItemKind.PatentMedicine) || (Item.Kind == ItemKind.HerbalMedicine)))
         {
                Druggery drug = Item as Druggery;
                if ((drug != null)
                   && ((drug.Attributes == DruggeryAttributeFlag.SkinTestLimited) || (drug.Attributes == DruggeryAttributeFlag.SkinTestUnlimited))) {
                    DataRow[] rows = m_UILogic.SkinTestResultTable.Select(ConstSchemaNames.SkinTestColSpecSerialNo + " = " + drug.SpecSerialNo);
                    if ((rows == null) || (rows.Length == 0) || (rows[0][ConstSchemaNames.SkinTestColFlag].ToString() == ConstNames.LightDemanding)) {
                        m_AllergicDrug = drug;
                        if (CoreBusinessLogic.BusinessLogic.ConnectToHis) {
                            m_SkinTestForm.ShowMessageOnly = true;
                            m_SkinTestForm.Message = ConstMessages.MsgNotFindSkinTestResult;
                        }
                        else if ((rows != null) && (rows.Length > 0) && (rows[0]["yxbz"].ToString() == ConstNames.LightDemanding)) {
                            m_SkinTestForm.ShowMessageOnly = true;
                            m_SkinTestForm.Message = ConstMessages.MsgSkinRestResultIsPlus;
                        }
                        else {
                            m_SkinTestForm.ShowMessageOnly = false;
                        }
                        // 在调出皮试结果输入控件前绑定事件；在使用结束后取消事件
                        itemInput.EnterMoveNextControl = false;
                        m_SkinTestForm.ShowDialog();
                        return;
                    }
                }
            }
            itemInput.EnterMoveNextControl = true;
        }

        private void DoAfterChoiceSkinTestResult(object sender, EventArgs e) {
            if (m_SkinTestForm.HadChoiceOne) {
                m_UILogic.SaveSkinTestResult(m_AllergicDrug.SpecSerialNo, m_AllergicDrug.Name, m_SkinTestForm.SkinTestResult);

            }
            if ((!m_SkinTestForm.HadChoiceOne)
               || (m_SkinTestForm.SkinTestResult == SkinTestResultKind.Plus)) {
                itemInput.CodeValue = null;
                itemInput.Focus();
                itemInput.EnterMoveNextControl = true;
            }
        }

        private decimal CalcTotalAmount() {
            //住院单位的总量
            decimal count = edtTotalAccount.Value;
            Druggery drug = Item as Druggery;
            if (drug == null)
                return 0;
            //住院单位系数 
            decimal quctiety = drug.SpecUnit.Quotiety;

            // 总量 /规格系数 
            return count / quctiety;
        }

        private void ResetOutDrugTotalAmount() {
            if (!panelTotal.Visible)
                return;

            //根据总量得到出院带药的天数
            decimal totalOfDay;
            //每天的用量
            totalOfDay = CalcTotalAmountOfAvgDay();

            //总量/每天用量
            decimal day = CalcTotalAmount() / totalOfDay;
            edtDays.Value = Convert.ToInt32(day);
        }
        #endregion

        #region events
        private void catalogInput_CodeValueChanged(object sender, EventArgs e) {
            SetValueAfterCatalogChanged();
        }

        private void itemInput_CodeValueChanged(object sender, EventArgs e) {
            SetValueAfterItemChanged();
            CheckNeedSkinTestAfterSelectItem();
        }

        private void usageInput_CodeValueChanged(object sender, EventArgs e) {
            //FireSelectedItemChanged(true);
        }

        private void edtAmount_EditValueChanged(object sender, EventArgs e) {
            // 出院带药时，如果数量/频次/天数有更新，则要刷新平均每天用量
            ResetOutDruggeryTotalAmout();
            // 出院带药时，如果数量/频次/总量有更新，则要刷新出院带药天数
            ResetOutDrugTotalAmount();
        }

        private void edtAmount_EditValueChanging(object sender, ChangingEventArgs e) {
            decimal newValue;

            //if ((e.NewValue.ToString() == "") || (e.NewValue.ToString() == "."))
            //   return;

            try {
                newValue = Convert.ToDecimal(e.NewValue);
                if ((newValue > edtAmount.Properties.MaxValue) || (newValue < edtAmount.Properties.MinValue))
                    edtAmount.ErrorText = String.Format(CultureInfo.CurrentCulture
                       , ConstMessages.FormatRangOfCount, edtAmount.Properties.MinValue, edtAmount.Properties.MaxValue);
                else
                    edtAmount.ErrorText = "";
            }
            catch { }
        }

        private void selectUnit_SelectedIndexChanged(object sender, EventArgs e) { }

        private void selectUnit_Enter(object sender, EventArgs e) {
            //selectUnit.ShowPopup();
        }

        private void selectUnit_CloseUp(object sender, CloseUpEventArgs e) {
            if (selectUnit.EnterMoveNextControl)
                SendKeys.Send("{ENTER}");
        }

        private void edtDays_EditValueChanged(object sender, EventArgs e) {
            // 出院带药时，如果数量/频次/天数有更新，则要刷新平均每天用量
            ResetOutDruggeryTotalAmout();
            // 出院带药时，如果数量/频次/总量有更新，则要刷新带药天数
            ResetOutDrugTotalAmount();
        }

        private void deptInput_CodeValueChanged(object sender, EventArgs e) {
            if (ShiftDept != null) {
                ShiftDept.InitializeCorrespondingWards(m_SqlExecutor);
                // 只显示科室对应的病区(要重设病区过滤条件)
                wardInput.CurrentWordbook.ExtraCondition = ShiftDept.CorrespondingWardsCondition;
                if (wardInput.HadGetValue)
                    wardInput.CodeValue = wardInput.CodeValue;
            }
            else {
                wardInput.CodeValue = "";
            }
        }

        private void wardInput_CodeValueChanged(object sender, EventArgs e) { }

        private void edtEntrustContent_EditValueChanged(object sender, EventArgs e) { }

        private void dateEditExtra_EditValueChanged(object sender, EventArgs e) {
        }

        private void timeEditExtra_EditValueChanged(object sender, EventArgs e) {
        }

        private void ckSelfProvide_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)13) {
                ProcessDialogKey(Keys.Tab);
                //SendKeys.Send("TAB");
                //e.Handled = true;
            }
        }

        private void ckTomorrowUsed_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)13) {
                ProcessDialogKey(Keys.Tab);
                //SendKeys.Send("TAB");
                //e.Handled = true;
            }
        }

        private void btnCommit_Enter(object sender, EventArgs e) {
            btnOk.Focus();
            //FireDataEditFinished(new EventArgs());
        }

        private void btnNew_Click(object sender, EventArgs e) {
            IsModifyData = false;
            InitializeDefaultValue(DateTime.MinValue, null);

        }

        private void btnOk_Click(object sender, EventArgs e) {
            try
            {
                if (IsModifyData)
                {
                    FireDataEditFinished(new DataCommitArgs(DataCommitType.Modify));
                }
                else
                {
                    FireDataEditFinished(new DataCommitArgs(DataCommitType.Add));
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e) {
            FireDataEditFinished(new DataCommitArgs(DataCommitType.Insert));
        }

        private void OrderContentEditor_Enter(object sender, EventArgs e) {
            if (IsModifyData)
                FireStartEdit(new DataCommitArgs(DataCommitType.Modify));
            else
                FireStartEdit(new DataCommitArgs(DataCommitType.Add));
        }

        private void ckEdtDruggery_CheckedChanged(object sender, EventArgs e) {
            try
            {
                if (ckEdtDruggery.Checked)
                {
                    ContentKind = OrderContentKind.Druggery;
                    this.itemInput.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void ckEdtNormalItem_CheckedChanged(object sender, EventArgs e) {
            try
            {
                if (ckEdtNormalItem.Checked)
                {
                    ContentKind = OrderContentKind.ChargeItem;
                    this.itemInput.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void ckEdtGeneralItem_CheckedChanged(object sender, EventArgs e) {
            try
            {
                if (ckEdtGeneralItem.Checked)
                {
                    ContentKind = OrderContentKind.GeneralItem;
                    this.itemInput.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void ckEdtOperate_CheckedChanged(object sender, EventArgs e) { 
            try
            {
                if (ckEdtOperate.Checked)
                {
                    ContentKind = OrderContentKind.Operation;
                    this.itemInput.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void ckEdtText_CheckedChanged(object sender, EventArgs e) {
            try
            {
                if (ckEdtText.Checked)
                {
                    ContentKind = OrderContentKind.TextNormal;
                    this.itemInput.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void ckEdtShiftDept_CheckedChanged(object sender, EventArgs e) {
            try
            {
                if (ckEdtShiftDept.Checked)
                {
                    ContentKind = OrderContentKind.TextShiftDept;
                    this.itemInput.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void ckEdtLeaveHosp_CheckedChanged(object sender, EventArgs e) {   
            try
            {
                if (ckEdtLeaveHosp.Checked)
                {
                    ContentKind = OrderContentKind.TextLeaveHospital;
                    this.itemInput.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void ckEdtOutDruggery_CheckedChanged(object sender, EventArgs e) {   
            try
            {
                if (ckEdtOutDruggery.Checked)
                {
                    ContentKind = OrderContentKind.OutDruggery;
                    this.itemInput.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void ckSupToTemp_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)13) {
                ProcessDialogKey(Keys.Tab);
                //SendKeys.Send("TAB");
                //e.Handled = true;
            }
        }

        void edtTotalAccount_ValueChanged(object sender, System.EventArgs e) {
            ResetOutDrugTotalAmount();
        }

        private void selectUnit2_CloseUp(object sender, EventArgs e) {
            if (selectUnit.EnterMoveNextControl)
                SendKeys.Send("{ENTER}");
        }

        /// <summary>
        /// 复选框回车事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-25</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        #endregion

    }
}
