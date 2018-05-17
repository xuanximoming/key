using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Resources;
using DrectSoft.Wordbook;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DrectSoft.Core.DoctorAdvice
{
    /// <summary>
    /// 医嘱编辑控件
    /// </summary>
    public partial class AdviceEditor : XtraUserControl, ISupportInitialize
    {
        #region public properties
        /// <summary>
        /// 标记医嘱数据是否改变过
        /// </summary>
        [Browsable(false)]
        public bool HadChanged
        {
            get
            {
                if (m_UILogic == null)
                    return false;
                return m_UILogic.HadChanged;
            }
        }

        /// <summary>
        /// 标记当前操作的是何种类型的医嘱
        /// </summary>
        [Browsable(false)]
        public bool IsTempOrder
        {
            get { return m_UILogic.IsTempOrder; }
            set
            {
                m_UILogic.IsTempOrder = value;
                barItemLongOrder.Checked = !value;
                barItemTempOrder.Checked = value;
            }
        }

        /// <summary>
        /// 当前查看的医嘱状态(目前提供全部、有效、新增三种选择)
        /// 临时医嘱已审核的看作有效，长期医嘱已执行的看作有效
        /// </summary>
        [Browsable(false)]
        public OrderState OrderState
        {
            get { return m_UILogic.OrderStateFilter; }
            set
            {
                m_UILogic.OrderStateFilter = value;
                barItemStateAll.Checked = (value == OrderState.All);
                barItemStateAvailably.Checked = (value == OrderState.Executed);
                barItemStateNew.Checked = (value == OrderState.New);
                advGridView.MoveLast();
            }
        }

        /// <summary>
        /// 是否允许添加新医嘱
        /// </summary>
        public bool AllowAddNew
        {
            get
            {
                if (m_UILogic != null)
                    return m_UILogic.AllowAddNew;
                else
                    return false;
            }
        }

        /// <summary>
        /// 包含适用于当前显示的医嘱类别的频次的字典类
        /// </summary>
        [CLSCompliantAttribute(false)]
        public OrderFrequencyBook FrequencyWordbook
        {
            get { return m_UILogic.FrequencyWordbook; }
        }

        /// <summary>
        /// 当前病人的医嘱中是否包含有效的“出院医嘱”（没有被取消的）
        /// </summary>
        public bool HasOutHospitalOrder
        {
            get { return m_UILogic.HasOutHospitalOrder; }
        }

        /// <summary>
        /// 成套医嘱逻辑处理对象
        /// </summary>
        public SuiteOrderHandle SuiteHelper
        {
            get
            {
                if (m_UILogic == null)
                    InitializeUILogicObject();
                return m_UILogic.SuiteHelper;
            }
        }
        #endregion

        #region private variables
        private UILogic m_UILogic;
        private OrderContentEditor contentEditor;
        /// <summary>
        /// 美康合理用药组件
        /// </summary>
        //private PassComponent m_MedicomPass;
        private DateTimeInputForm m_TimeInputForm;
        private WaitDialogForm m_WaitDialog;

        private int m_FocusedRowHandle;
        private IEmrHost m_App;

        /// <summary>
        /// 标记是否按下了ESC键(用来处理按ESC取消当前编辑数据的情况)
        /// </summary>
        private bool m_HadPressESC;
        /// <summary>
        /// 标记数据校验产生了错误（和HasPressESC组合使用）
        /// </summary>
        private bool m_HadError;
        /// <summary>
        /// 标记被修改的数据内容
        /// </summary>
        private UpdateContentFlag m_UpdateFlag;
        private bool m_FocusInGrid;
        private EditorCallModel m_CallModel; // 是否被编辑界面调用
        #endregion

        #region private properties
        private Inpatient CurrentPatient
        {
            get
            {
                if (m_UILogic == null)
                    return null;

                return m_UILogic.CurrentPatient;
            }
            set
            {
                if (value == null)
                {
                    ClearSurface();

                    orderToolBar.Visible = false;
                    statusBar.Visible = false;
                }
                else if ((CurrentPatient == null) || (CurrentPatient.NoOfFirstPage != value.NoOfFirstPage))
                {
                    InitializePatientData(value);
                }
            }
        }

        private void InitializePatientData(Inpatient value)
        {
            try
            {
                ClearSurface();

                if (m_UILogic == null)
                    InitializeUILogicObject();

                bool isFirstPat = (m_UILogic.CurrentPatient == null);
                m_UILogic.CurrentPatient = value;

                InitializeToolbar(m_CallModel);

                //IsTempOrder = false;
                //OrderState = OrderState.All;
                if (isFirstPat)
                {
                    // 首次显示时，编辑模式下默认显示新医嘱，查询模式默认显示有效医嘱
                    if (m_CallModel == EditorCallModel.EditOrder)
                        OrderState = OrderState.New;
                    else if (m_CallModel == EditorCallModel.Query)
                        OrderState = OrderState.Executed;
                    else
                        OrderState = OrderState.All;
                }
                else
                    OrderState = OrderState;
                IsTempOrder = IsTempOrder;

                //barItemOrderCatalog.EditValue = selectOrderCatalog.Items[0];
                //barItemFilterStatus.EditValue = OrderState.All;

                // 初始化合理用药组件中的病人信息
                /*
                if (CoreBusinessLogic.BusinessLogic.UseMedicomPlug)
                    SetMedicomPassPatient();
                 */

                // 主动刷新工具栏的按钮状态
                ResetToolBarItemState();

                InitializeSkinTestResult();

                Enabled = true;
                orderToolBar.Visible = true;
                statusBar.Visible = (m_CallModel != EditorCallModel.EditSuite);
            }
            finally
            {
                HideWaitDialog();
            }
        }

        /// <summary>
        /// 完整的新开始日期和时间（精确到分钟）
        /// </summary>
        private DateTime NewStartDateTime
        {
            get
            {
                if (OrderTemp.StartDateTime == DateTime.MinValue)
                    return m_UILogic.DefaultStartDateTime;

                if (OrderTemp.StartDateTime.TimeOfDay == TimeSpan.Zero)
                    return OrderTemp.StartDateTime.Date + m_UILogic.DefaultStartDateTime.TimeOfDay;
                else
                    return OrderTemp.StartDateTime.Date + new TimeSpan(
                       OrderTemp.StartDateTime.Hour, OrderTemp.StartDateTime.Minute, 0);
                //if ((m_TempStartDate == DateTime.MinValue)
                //   && (m_TempStartTime == TimeSpan.MinValue))
                //   return m_UILogic.DefaultStartDateTime;

                //if (m_TempStartTime == TimeSpan.MinValue)
                //   return m_TempStartDate.Date + m_UILogic.DefaultStartDateTime.TimeOfDay;
                //else
                //   return m_TempStartDate.Date
                //      + new TimeSpan(m_TempStartTime.Hours, m_TempStartTime.Minutes, 0);
            }
        }

        /// <summary>
        /// 当前Focused的行对应的医嘱对象
        /// </summary>
        private Order FocusedOrder
        {
            get
            {
                if (m_FocusedRowHandle >= 0)
                    return m_UILogic.CurrentView[m_FocusedRowHandle].OrderCache;
                else
                    return null;
            }
        }

        /// <summary>
        /// 返回当前编辑的医嘱类别使用的临时变量
        /// </summary>
        private Order OrderTemp
        {
            get
            {
                if (m_UILogic.IsTempOrder)
                    return _tempOrder;
                else
                    return _longOrder;
            }
        }
        private LongOrder _longOrder;
        private TempOrder _tempOrder;

        /// <summary>
        /// 医嘱剪贴板(赋值、粘贴医嘱时会用到医嘱内容和分组数据)
        /// </summary>
        private Collection<Order> OrderClipboard
        {
            get
            {
                if (_orderClipboard == null)
                    _orderClipboard = new Collection<Order>();
                return _orderClipboard;
            }
        }
        private Collection<Order> _orderClipboard;

        private OrderSuiteEditForm SuiteEditForm
        {
            get
            {
                if (_suiteEditForm == null)
                {
                    _suiteEditForm = new OrderSuiteEditForm();
                    _suiteEditForm.InitializeProperty(m_App.SqlHelper, m_App.CustomMessageBox);
                }
                return _suiteEditForm;
            }
        }
        private OrderSuiteEditForm _suiteEditForm;

        /// <summary>
        /// 对工具栏中部分有快捷键的按钮做控制，以保证只在对Grid中数据进行操作时才起效，避免影响到其它框中的编辑操作
        /// </summary>
        private bool EnableShortCut
        {
            get { return _enableShortCut; }
            set
            {
                _enableShortCut = value;
                ResetToolBarItemState();
                //BindOrUnbindItemShortcut(value);
                //if (!value)
                //   advGridView.ClearSelection();
            }
        }
        private bool _enableShortCut;


        /*
        private List<MediIntfDrugInfo> MedicomDrugInfos
        {
            get
            {
                if (_medicomDrugInfos == null)
                    _medicomDrugInfos = new List<MediIntfDrugInfo>();
                return _medicomDrugInfos;
            }
        }
        private List<MediIntfDrugInfo> _medicomDrugInfos;
        */
        private bool IsInEditing
        {
            get
            {
                if (m_CallModel == EditorCallModel.EditOrder)
                    return (CurrentPatient != null);
                else if (m_CallModel == EditorCallModel.EditSuite)
                    return ((SuiteHelper != null) && (SuiteHelper.CurrentSuiteNo > 0));
                else
                    return true;
            }
        }
        #endregion

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="callModel">指定调用模式</param>
        public AdviceEditor(IEmrHost app, EditorCallModel callModel)
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("创建组件……", "正在执行操作，请稍等。");
                InitializeComponent();

                m_CallModel = callModel;
                CustomInitialize(app);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region public call methods
        /// <summary>
        /// 编辑或查询指定病人的医嘱数据
        /// </summary>
        /// <param name="inpatient"></param>
        public void CallShowPatientOrder(Inpatient inpatient)
        {
            if (m_CallModel == EditorCallModel.EditSuite)
                throw new ApplicationException(ConstMessages.ExceptionNotOrderEdit);

            Cursor originalCursor = Cursor;
            Cursor = Cursors.WaitCursor;

            CurrentPatient = inpatient;

            Cursor = originalCursor;
            HideWaitDialog();
        }

        /// <summary>
        /// 编辑成套医嘱数据
        /// </summary>
        /// <param name="suiteSerialNo">要编辑的成套医嘱序号</param>
        public void CallEditSuiteOrder(decimal suiteSerialNo)
        {
            if (m_CallModel != EditorCallModel.EditSuite)
                throw new ApplicationException(ConstMessages.ExceptionNotOrderSuitEdit);

            Cursor originalCursor = Cursor;
            Cursor = Cursors.WaitCursor;

            SuiteHelper.CurrentSuiteNo = suiteSerialNo;

            Cursor = originalCursor;
            HideWaitDialog();
        }
        #endregion

        #region public methods
        /// <summary>
        /// 父窗口关闭时调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnParentFormClosing(object sender, FormClosingEventArgs e)
        {
            m_FocusInGrid = false;
            e.Cancel = !CheckCanExitOrSwitch(null);
        }

        /// <summary>
        /// 在退出前或切换病人时检查允许切换
        /// </summary>
        /// <param name="newPatient">切换后的新病人,退出时传null</param>
        /// <returns></returns>
        public bool CheckCanExitOrSwitch(Inpatient newPatient)
        {
            if ((IsDisposed) || (m_UILogic == null) || (CurrentPatient == null) || (m_CallModel == EditorCallModel.Query)
               || ((newPatient != null) && (CurrentPatient.NoOfFirstPage == newPatient.NoOfFirstPage)))
                return true;

            DialogResult result;
            if (HadChanged)
            {
                result = m_App.CustomMessageBox.MessageShow(ConstMessages.MsgPromptingSaveData
                   , CustomMessageBoxKind.QuestionYesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        DoSaveData();
                        if (!HadChanged)
                            return false;
                        break;
                    case DialogResult.No:
                        return true;
                    default:
                        return false;
                }
            }
            if ((m_CallModel == EditorCallModel.EditOrder) && (m_UILogic.HasNotSendData))
            {
                result = m_App.CustomMessageBox.MessageShow(ConstMessages.MsgPromptingSendData
                   , CustomMessageBoxKind.QuestionYesNoCancel);
                switch (result)
                {
                    case DialogResult.Yes:
                        DoSendChangedDataToHIS();
                        if (!m_UILogic.HasNotSendData)
                            return false;
                        break;
                    case DialogResult.No:
                        return true;
                    default:
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 插入成套医嘱
        /// </summary>
        /// <param name="selectedContents">包含医嘱内容和分组情况的数组</param>
        public void InsertSuiteOrder(object[,] selectedContents)
        {
            try
            {
                m_UILogic.CheckAllowInsertOrder(null);

                // 成套默认为添加
                m_UILogic.InsertSuiteOrder(selectedContents, null);
                MoveFocusedRowByHand(false);
            }
            catch (Exception e)
            {
                ShowErrorMessage(e.Message, CustomMessageBoxKind.ErrorOk);
            }
        }

        /// <summary>
        /// 向医嘱内容编辑组件传入Form截获的功能键，以触发特定的事件
        /// </summary>
        /// <param name="e"></param>
        public void AcceptFunctionKeyPress(KeyEventArgs e)
        {
            if ((contentEditor.Visible) && (m_UILogic.AllowEdit))
                contentEditor.AcceptFunctionKeyPress(e);
        }
        #endregion

        #region custom event handler
        /// <summary>
        /// 添加新的手术医嘱事件
        /// </summary>
        public event EventHandler OperationOrderAdded
        {
            add { onOperationOrderAdded = (EventHandler)Delegate.Combine(onOperationOrderAdded, value); }
            remove { onOperationOrderAdded = (EventHandler)Delegate.Remove(onOperationOrderAdded, value); }
        }
        private EventHandler onOperationOrderAdded;

        public event EventHandler AfterSwitchOrderTable
        {
            add { onAfterSwitchOrderTable = (EventHandler)Delegate.Combine(onAfterSwitchOrderTable, value); }
            remove { onAfterSwitchOrderTable = (EventHandler)Delegate.Remove(onAfterSwitchOrderTable, value); }
        }
        private EventHandler onAfterSwitchOrderTable;

        private void FireAfterSwitchOrderTable(object sender, EventArgs e)
        {
            if (onAfterSwitchOrderTable != null)
                onAfterSwitchOrderTable(this, new EventArgs());
        }

        /// <summary>
        /// 医嘱打印委托
        /// </summary>
        /// <param name="patient">要打印医嘱的病人</param>
        /// <returns></returns>
        public delegate void PrintOrder(Inpatient patient);

        /// <summary>
        /// 处理输出信息创建
        /// </summary>
        public PrintOrder PrintCurrentPatientOrder
        {
            get { return _printCurrentPatientOrder; }
            set
            {
                _printCurrentPatientOrder = value;
                barItemPrint.Enabled = (value != null);
            }
        }
        private PrintOrder _printCurrentPatientOrder;
        #endregion

        #region private methods of initialize data
        private void CustomInitialize(IEmrHost app)
        {
            if (app == null)
                throw new ArgumentNullException();

            m_App = app;
            _longOrder = new LongOrder(); // 临时医嘱对象不需要重复创建
            _tempOrder = new TempOrder();

            // 绑定控件的事件
            BindingEvents2Controls();

            BindUniformImageToControl();

            repItemDateEdit.EditValueChangedFiringMode = EditValueChangedFiringMode.Buffered;
            repItemTimeEdit.EditValueChangedFiringMode = EditValueChangedFiringMode.Buffered;

            // 日期控件不要设NullDate，就可以让当前日期成为默认日期
            //itemDateEdit.NullDate = m_UILogic.DefaultStartDateTime.Date;

            //m_MedicomPass = null;
            CurrentPatient = null;
            CreateOrderEditControl();

            if (m_CallModel == EditorCallModel.EditSuite) // 维护成套时只需要初始化一次Grid样式
                ResetGridStyle();

            HideWaitDialog();
        }

        private void ClearSurface()
        {
            if (advGridView.SelectedRowsCount > 0)
                advGridView.ClearSelection();
            //MedicomDrugInfos.Clear();
            Enabled = false;
            EnableShortCut = false;
        }

        private void InitializeUILogicObject()
        {
            if (m_UILogic == null)
            {
                SetWaitDialogCaption(ConstMessages.HintInitData);
                bool enabledOrderRules = CoreBusinessLogic.BusinessLogic.EnableOrderRules;
                //try
                //{
                //创建逻辑处理对象实例
                m_UILogic = new UILogic(m_App, GridControl.InvalidRowHandle, GridControl.NewItemRowHandle, m_CallModel);
                //}
                //catch (CallRemotingException err)
                //{
                //   HideWaitDialog();
                //   m_App.CustomMessageBox.MessageShow(err.Message, CustomMessageBoxKind.ErrorYes);
                //}
                //catch
                //{
                //   throw;
                //}
                if (enabledOrderRules != CoreBusinessLogic.BusinessLogic.EnableOrderRules)
                {
                    HideWaitDialog();
                    m_App.CustomMessageBox.MessageShow(ConstMessages.MsgCanntGetRecipeRuleData, CustomMessageBoxKind.ErrorYes);
                }
                m_UILogic.AfterSwitchOrderTable += new EventHandler(DoAfterSwitchOrderTable);
                m_UILogic.AllowNewChanged += new EventHandler(AfterCurrentTableAllowNewChanged);
                m_UILogic.ContentBaseDataChanged += new EventHandler(AfterContentBaseDataChanged);
                m_UILogic.ProcessStarting += new EventHandler<ProcessHintArgs>(DoUILogicProcessStaring);
                if (m_CallModel == EditorCallModel.EditSuite)
                {
                    // 必须在创建UILogic后绑定事件（以保证先执行CoreLogic中绑定的事件）
                    SuiteHelper.AfterSwitchSuite += new EventHandler(DoAfterSwitchSuite);
                }

                contentEditor.InitializeEditor(m_UILogic, m_App.SqlHelper);
                HideWaitDialog();
            }
        }

        private void SetUnboundColumnFieldName()
        {
            gridColStartDate.FieldName = OrderView.UNStartDate;
            gridColStartTime.FieldName = OrderView.UNStartTime;
            gridColContent.FieldName = OrderView.UNContent;
            gridColCeaseDate.FieldName = OrderView.UNCeaseDate;
            gridColCeaseTime.FieldName = OrderView.UNCeaseTime;
        }

        private void BindingEvents2Controls()
        {
            gridCtrl.DataSourceChanged += new EventHandler(GridDataSourceChanged);
            gridCtrl.MouseDown += new MouseEventHandler(DoAfterGridCtrlMouseDown);
            gridCtrl.KeyPress += new KeyPressEventHandler(GridKeyPress);
            advGridView.CustomUnboundColumnData += new CustomColumnDataEventHandler(GridCustomUnboundColumnData);
            advGridView.ValidateRow += new ValidateRowEventHandler(GridValidateRow);
            advGridView.InitNewRow += new InitNewRowEventHandler(GridInitNewRow);
            advGridView.FocusedColumnChanged += new FocusedColumnChangedEventHandler(GridFocusedColumnChanged);
            advGridView.FocusedRowChanged += new FocusedRowChangedEventHandler(GridFocusedRowChanged);
            advGridView.InvalidRowException += new InvalidRowExceptionEventHandler(SetInvalidRowExceptionMode);
            advGridView.SelectionChanged += new SelectionChangedEventHandler(SelectionChanged);
            advGridView.GotFocus += new EventHandler(GridViewGotFocus);
            advGridView.LostFocus += new EventHandler(GridViewLostFocus);
            advGridView.CustomDrawCell += new RowCellCustomDrawEventHandler(CustomDrawOrderGridCell);
            advGridView.DoubleClick += new EventHandler(GridRowDoubleClick);

            repItemContentEdit.QueryPopUp += new CancelEventHandler(GridContentEditPopup);
            repItemContentEdit.CloseUp += new CloseUpEventHandler(GridContentEditClose);

            repItemDateEdit.EditValueChanging += new ChangingEventHandler(StartDateEditValueChanging);
            repItemTimeEdit.EditValueChanging += new ChangingEventHandler(StartTimeEditValueChanging);

            barItemOrderCatalog.EditValueChanged += new EventHandler(ItemFilterOrderCatalogClick);
            barItemFilterStatus.EditValueChanged += new EventHandler(ItemFilterOrderStateClick);

            barItemSave.ItemClick += new ItemClickEventHandler(ItemSaveClick);
            barItemDelete.ItemClick += new ItemClickEventHandler(ItemDeleteNewOrderClick);
            barItemSetGroup.ItemClick += new ItemClickEventHandler(ItemSetGroupClick);
            barItemCancelGroup.ItemClick += new ItemClickEventHandler(ItemCancelGroupClick);
            barItemCancel.ItemClick += new ItemClickEventHandler(ItemCancelClick);
            barItemCease.ItemClick += new ItemClickEventHandler(ItemCeaseClick);
            barItemAudit.ItemClick += new ItemClickEventHandler(ItemAuditClick);
            barItemUp.ItemClick += new ItemClickEventHandler(ItemMoveNewOrderUpClick);
            barItemDown.ItemClick += new ItemClickEventHandler(ItemMoveNewOrderDownClick);
            barItemCheckOrder.ItemClick += new ItemClickEventHandler(DoCheckOrders);
            barItemDrugManual.ItemClick += new ItemClickEventHandler(DoShowPlugDrugInfoMenu);
            barItemSubmit.ItemClick += new ItemClickEventHandler(ItemSubmitClick);
            barItemLongOrder.CheckedChanged += new ItemClickEventHandler(ItemLongOrderClick);
            barItemTempOrder.CheckedChanged += new ItemClickEventHandler(ItemTempOrderClick);
            barItemStateAll.CheckedChanged += new ItemClickEventHandler(ItemStateAllClick);
            barItemStateNew.CheckedChanged += new ItemClickEventHandler(ItemStateNewClick);
            barItemStateAvailably.CheckedChanged += new ItemClickEventHandler(ItemStateAvailablyClick);
            barItemCut.ItemClick += new ItemClickEventHandler(ItemCutClick);
            barItemCopy.ItemClick += new ItemClickEventHandler(ItemCopyClick);
            barItemPaste.ItemClick += new ItemClickEventHandler(ItemPasteClick);
            barItemRefresh.ItemClick += new ItemClickEventHandler(ItemRefreshClick);
            barItemDrugInfo.ItemClick += new ItemClickEventHandler(DoShowDrugInfo);
            barItemPrint.ItemClick += new ItemClickEventHandler(ItemPrintClick);
            barItemExpandHerbDetail.ItemClick += new ItemClickEventHandler(ItemExpandHerbDetailClick);
            barItemExpandAllHerb.ItemClick += new ItemClickEventHandler(ItemExpandAllHerbClick);
            barItemCollapseHerbDetail.ItemClick += new ItemClickEventHandler(ItemCollapseHerbDetailClick);
            barItemCollapseAllHerb.ItemClick += new ItemClickEventHandler(ItemCollapseAllHerbClick);
            barItemAutoGroup.ItemClick += new ItemClickEventHandler(ItemAutoGroupClick);

            this.Leave += new EventHandler(LeaveAdviceEditor);
        }

        private void InitializeSkinTestResult()
        {
            SetWaitDialogCaption(ConstMessages.HintInitSkinTestResult);
            gridAllergic.DataSource = m_UILogic.SkinTestResultTable;
        }
        #endregion

        #region private methods of UI process
        private void CreateOrderEditControl()
        {
            if (m_CallModel != EditorCallModel.EditSuite)
                contentEditor = new OrderContentEditor(false);
            else
                contentEditor = new OrderContentEditor(true);
            contentEditor.UseRadioCatalogInputStyle = CoreBusinessLogic.BusinessLogic.UseRadioCatalogInputStyle;

            MinimumSize = new Size(contentEditor.SuitablyWidth, 400);
            Size = MinimumSize;
            panelContentEditor.Height = contentEditor.SuitablyHeight;
            panelContentEditor.Controls.Add(contentEditor);

            contentEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            contentEditor.Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            contentEditor.Location = new Point(0, 0);
            contentEditor.Margin = new Padding(0);
            contentEditor.Name = "contentEditor";
            //contentEditor.Size = new Size(734, 102);
            contentEditor.StartEdit += new EventHandler<DataCommitArgs>(BeforeContentEditStart);
            contentEditor.EditFinished += new EventHandler<DataCommitArgs>(AfterContentEditFinished);
            contentEditor.SelectedItemChanged += new EventHandler<OrderItemArgs>(DoAfterContentEditorSelectedItemChanged);
        }

        private void InitializeToolbar(EditorCallModel callModel)
        {
            // 禁用退出和审核按钮
            barItemExit.Visibility = BarItemVisibility.Never;
            barItemAudit.Visibility = BarItemVisibility.Never;
            barItemPrint.Enabled = (PrintCurrentPatientOrder != null);

            if (callModel == EditorCallModel.EditOrder)
            {
                if ((CoreBusinessLogic.BusinessLogic.ConnectToHis)
                   && (!CoreBusinessLogic.BusinessLogic.AutoSyncData))
                    barItemSubmit.Visibility = BarItemVisibility.Always;
                else
                    barItemSubmit.Visibility = BarItemVisibility.Never;

                // 绑定合理用药
                /*
                if ((CoreBusinessLogic.BusinessLogic.UseMedicomPlug)
                   && InitializeMedicomPass())
                {
                    barItemCheckOrder.Visibility = BarItemVisibility.Always;
                    barItemDrugManual.Visibility = BarItemVisibility.Always;
                }
                else
                {
                    //m_MedicomPass = null;
                    barItemCheckOrder.Visibility = BarItemVisibility.Never;
                    barItemDrugManual.Visibility = BarItemVisibility.Never;
                }
                 */
            }
            else if (callModel == EditorCallModel.Query) // 查询模式下要禁用编辑按钮
            {
                barItemSave.Visibility = BarItemVisibility.Never;
                barItemSubmit.Visibility = BarItemVisibility.Never;
                barItemCut.Visibility = BarItemVisibility.Never;
                barItemCopy.Visibility = BarItemVisibility.Never;
                barItemPaste.Visibility = BarItemVisibility.Never;
                barItemDelete.Visibility = BarItemVisibility.Never;
                barItemUp.Visibility = BarItemVisibility.Never;
                barItemDown.Visibility = BarItemVisibility.Never;
                barItemCancel.Visibility = BarItemVisibility.Never;
                barItemCease.Visibility = BarItemVisibility.Never;
                barItemSetGroup.Visibility = BarItemVisibility.Never;
                barItemAutoGroup.Visibility = BarItemVisibility.Never;
                barItemCancelGroup.Visibility = BarItemVisibility.Never;
                barItemCheckOrder.Visibility = BarItemVisibility.Never;
                barItemDrugManual.Visibility = BarItemVisibility.Never;
            }
            else if (callModel == EditorCallModel.EditSuite) // 
            {
                barItemPrint.Visibility = BarItemVisibility.Never;
                barItemRefresh.Visibility = BarItemVisibility.Never;
                barItemSubmit.Visibility = BarItemVisibility.Never;
                barItemCancel.Visibility = BarItemVisibility.Never;
                barItemCease.Visibility = BarItemVisibility.Never;
                barItemCheckOrder.Visibility = BarItemVisibility.Never;
                barItemDrugManual.Visibility = BarItemVisibility.Never;
                barItemStateAll.Visibility = BarItemVisibility.Never;
                barItemStateAvailably.Visibility = BarItemVisibility.Never;
                barItemStateNew.Visibility = BarItemVisibility.Never;
                barItemSkinTestInfo.Visibility = BarItemVisibility.Never;
                barItemExpandHerbDetail.Visibility = BarItemVisibility.Never;
                barItemCollapseHerbDetail.Visibility = BarItemVisibility.Never;
                barItemExpandAllHerb.Visibility = BarItemVisibility.Never;
                barItemCollapseAllHerb.Visibility = BarItemVisibility.Never;
            }
        }

        private void BindUniformImageToControl()
        {
            InitializeImageList();

            barItemSave.ImageIndex = 0;
            barItemSubmit.ImageIndex = 2;
            barItemPrint.ImageIndex = 4;
            barItemCut.ImageIndex = 6;
            barItemCopy.ImageIndex = 8;
            barItemPaste.ImageIndex = 10;
            barItemDelete.ImageIndex = 12;
            barItemUp.ImageIndex = 14;
            barItemDown.ImageIndex = 16;
            barItemCancel.ImageIndex = 18;
            barItemCease.ImageIndex = 20;
            barItemSetGroup.ImageIndex = 22;
            barItemCancelGroup.ImageIndex = 24;
            barItemCheckOrder.ImageIndex = 26;
            barItemDrugManual.ImageIndex = 28;
            barItemExit.ImageIndex = 30;
            barItemRefresh.ImageIndex = 32;
            barItemAutoGroup.ImageIndex = 34;

            barItemSave.ImageIndexDisabled = 1;
            barItemSubmit.ImageIndexDisabled = 3;
            barItemPrint.ImageIndexDisabled = 5;
            barItemCut.ImageIndexDisabled = 7;
            barItemCopy.ImageIndexDisabled = 9;
            barItemPaste.ImageIndexDisabled = 11;
            barItemDelete.ImageIndexDisabled = 13;
            barItemUp.ImageIndexDisabled = 15;
            barItemDown.ImageIndexDisabled = 17;
            barItemCancel.ImageIndexDisabled = 19;
            barItemCease.ImageIndexDisabled = 21;
            barItemSetGroup.ImageIndexDisabled = 23;
            barItemCancelGroup.ImageIndexDisabled = 25;
            barItemCheckOrder.ImageIndexDisabled = 27;
            barItemDrugManual.ImageIndexDisabled = 29;
            barItemExit.ImageIndexDisabled = 31;
            barItemRefresh.ImageIndexDisabled = 33;
            barItemAutoGroup.ImageIndexDisabled = 35;

            barItemLegendNew.Glyph = GetStaticItemGlyph(OrderState.New, true, barItemLegendNew.Caption);
            barItemLegendAudit.Glyph = GetStaticItemGlyph(OrderState.Audited, true, barItemLegendAudit.Caption);
            barItemLegendCancel.Glyph = GetStaticItemGlyph(OrderState.Cancellation, true, barItemLegendCancel.Caption);
            barItemLegendExecuted.Glyph = GetStaticItemGlyph(OrderState.Executed, true, barItemLegendExecuted.Caption);
            barItemLegendCeased.Glyph = GetStaticItemGlyph(OrderState.Ceased, true, barItemLegendCeased.Caption);
            barItemLegendNotSynch.Glyph = GetStaticItemGlyph(OrderState.All, false, barItemLegendNotSynch.Caption);
        }

        private Image GetStaticItemGlyph(OrderState orderState, bool hadSynched, string staticText)
        {
            return CustomDrawOperation.CreateColorLegend(
               CustomDrawOperation.GetBackColorByState(orderState, false, hadSynched)
               , CustomDrawOperation.GetForeColorByState(orderState, false, hadSynched)
               , staticText);
        }

        private void InitializeImageList()
        {
            imageListSmall.ColorDepth = ColorDepth.Depth24Bit;
            imageListSmall.ImageSize = new Size(16, 16);
            imageListSmall.TransparentColor = Color.Magenta;
            imageListSmall.Images.AddRange(new Image[] {
              ResourceManager.GetSmallIcon(ResourceNames.Save, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Save, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.TransferData, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.TransferData, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.Print, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Print, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.Cut, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Cut, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.Copy, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Copy, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.Paste, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Paste, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.Delete, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Delete, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.ArrowUp, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.ArrowUp, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.ArrowDown, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.ArrowDown, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.Cancel, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Cancel, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.Stop, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Stop, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.SetGroup, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.SetGroup, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.CancelGroup, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.CancelGroup, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.SpellCheck, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.SpellCheck, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.Help, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Help, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.Exit, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Exit, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.Refresh, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.Refresh, IconType.Disable)
            , ResourceManager.GetSmallIcon(ResourceNames.AutoGroup, IconType.Normal)
            , ResourceManager.GetSmallIcon(ResourceNames.AutoGroup, IconType.Disable)
         });
        }

        private void ResetToolBarItemState()
        {
            if (m_CallModel != EditorCallModel.Query)
            {
                EditProcessFlag flags = 0;
                try
                {
                    if (m_UILogic != null)
                        flags = m_UILogic.GetBarItemStatus(advGridView.GetSelectedRows());
                }
                catch
                { }
                barItemDelete.Enabled = (((flags & EditProcessFlag.Delete) > 0) && EnableShortCut);
                barItemCancel.Enabled = ((flags & EditProcessFlag.Cancel) > 0);
                barItemCease.Enabled = ((flags & EditProcessFlag.Cease) > 0);
                barItemUp.Enabled = ((flags & EditProcessFlag.MoveUp) > 0);
                barItemDown.Enabled = ((flags & EditProcessFlag.MoveDown) > 0);
                barItemSetGroup.Enabled = ((flags & EditProcessFlag.SetGroup) > 0);
                barItemAutoGroup.Enabled = AllowAddNew;
                barItemCancelGroup.Enabled = ((flags & EditProcessFlag.CancelGroup) > 0);
                if ((m_UILogic != null) && (m_FocusedRowHandle == m_UILogic.NewItemRowHandle))
                    barItemSave.Enabled = m_UILogic.HadChanged;
                else
                    barItemSave.Enabled = ((flags & EditProcessFlag.Save) > 0);

                barItemSubmit.Enabled = ((m_UILogic != null) && m_UILogic.HasNotSendData);

                barItemAudit.Enabled = ((flags & EditProcessFlag.Audit) > 0);

                barItemCut.Enabled = (((flags & EditProcessFlag.Cut) > 0) && EnableShortCut);
                barItemCopy.Enabled = (((flags & EditProcessFlag.Copy) > 0) && EnableShortCut);
                // 粘贴--在粘贴的时候再判断是否可执行操作（不能粘贴时要给出提示）(已开出院医嘱，则禁止粘贴)
                barItemPaste.Enabled = ((OrderClipboard.Count > 0) && EnableShortCut && (!HasOutHospitalOrder));

                barItemCheckOrder.Enabled = ((m_UILogic != null) && AllowAddNew /*&& (m_MedicomPass != null)*/);
                barItemDrugInfo.Enabled = (/*(m_MedicomPass != null) && */(advGridView.SelectedRowsCount == 1)
                   && (OrderTemp.Content != null)
                   && ((OrderTemp.Content.OrderKind == OrderContentKind.Druggery) || (OrderTemp.Content.OrderKind == OrderContentKind.OutDruggery)));

                // 控制草药明细展开收缩相关菜单
                barItemExpandHerbDetail.Enabled = ((flags & EditProcessFlag.IsHerbSummary) > 0);
                barItemCollapseHerbDetail.Enabled = ((flags & EditProcessFlag.IsHerbDetail) > 0);
            }
        }

        private void ResetGridStyle()
        {
            //-- advGridView.BeginUpdate(); // 由外部方法控制
            ResetControlFont();

            advGridView.RowHeight = CustomDrawOperation.GridSetting.RowHeight;

            ResetGridViewBandAndColumns();

            //advGridView.EndDataUpdate();
        }

        private void ResetGridViewBandAndColumns()
        {
            // 数据源改变时重设Grid中允许显示的列及其尺寸
            GridBand[] bands;
            BandedGridColumn col;

            advGridView.Bands.Clear();
            if (m_CallModel != EditorCallModel.EditSuite)
            {
                TypeGridBand[] bandSetting = m_UILogic.CurrentBandSettings;
                bands = new GridBand[bandSetting.Length];
                for (int index = 0; index < bands.Length; index++)
                {
                    switch (bandSetting[index].BandName)
                    {
                        case OrderGridBandName.bandBeginInfo:
                            bands[index] = bandBeginInfo;
                            break;
                        case OrderGridBandName.bandAuditInfo:
                            bands[index] = bandAuditInfo;
                            break;
                        case OrderGridBandName.bandExecuteInfo:
                            bands[index] = bandExecuteInfo;
                            break;
                        case OrderGridBandName.bandCeaseInfo:
                            bands[index] = bandCeaseInfo;
                            break;
                        default:
                            throw new IndexOutOfRangeException(String.Format(ConstMessages.ExceptionFormatNotFindBand,
                               bandSetting[index].BandName));
                    }

                    bands[index].Columns.Clear();
                    foreach (string colName in bandSetting[index].ColumnNames)
                    {
                        col = advGridView.Columns[colName];
                        if (col == null)
                            throw new IndexOutOfRangeException(String.Format(ConstMessages.ExceptionFormatNotFindColumn,
                               colName));
                        col.Visible = true;
                        col.Width = CustomDrawOperation.GridSetting.GetColumnWidth(colName);
                        col.Caption = CustomDrawOperation.GridSetting.GetColumnCaption(colName);
                        bands[index].Columns.Add(col);
                    }
                }

                // 直接添加合理用药检查结果列
                if ((m_CallModel == EditorCallModel.EditOrder) && CoreBusinessLogic.BusinessLogic.UseMedicomPlug)
                    bands[0].Columns.Insert(0, gridColCheckResult);
            }
            else
            {
                // 维护成套医嘱时只需要医嘱内容列
                bands = new GridBand[] { bandBeginInfo };
                bandBeginInfo.Columns.Clear();
                bandBeginInfo.Columns.Add(gridColContent);
            }

            advGridView.Bands.AddRange(bands);
            advGridView.OptionsView.ShowBands = CustomDrawOperation.GridSetting.ShowBand;
        }

        private void ResetControlFont()
        {
            gridCtrl.Font = CustomDrawOperation.GridSetting.GridFont.Font;

            foreach (AppearanceObject ap in advGridView.Appearance)
                ap.Font = gridCtrl.Font;

            //contentEditor.ResetControlFont(gridCtrl.Font);
        }

        /// <summary>
        /// 根据当前选中的行和列判断Grid是否可编辑
        /// </summary>
        /// <param name="columnName">选中的列名</param>
        /// <param name="rowHandle">选中的行号</param>
        private void SetGridEditable(string columnName, int rowHandle)
        {
            advGridView.OptionsBehavior.Editable = ((m_UILogic != null)
                       && m_UILogic.JudgeCellCanEdit(columnName, rowHandle));

            if (advGridView.OptionsBehavior.Editable)
            {
                advGridView.FocusRectStyle = DrawFocusRectStyle.CellFocus;
                advGridView.OptionsSelection.EnableAppearanceFocusedCell = true;
                advGridView.OptionsSelection.EnableAppearanceFocusedRow = true;
            }
            else
            {
                advGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
                advGridView.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            }
        }

        private void ChangeVisibleOfEditRegion(bool visible)
        {
            if (panelContentEditor.Visible != visible)
            {
                panelContentEditor.Visible = visible;
                barItemEditRegion.Checked = visible;
                if (visible)
                {
                    //btnAdd.Focus();
                    PrepareEditNewOrder();
                }
            }
        }

        private void ShowOperationHint(string hintMessage)
        {
            barItemHint.Caption = "提示：" + hintMessage;
        }

        /// <summary>
        /// 手工移动Focused 的行
        /// </summary>
        /// <param name="toLast">True：Focused最后一行 False：Focused下一行</param>
        private void MoveFocusedRowByHand(bool toLast)
        {
            if (toLast)
            {
                advGridView.MoveLast();
                advGridView.TopRowIndex = advGridView.FocusedRowHandle;
            }
            else
                advGridView.MoveNext();
        }
        #endregion

        #region private methods of WaitDialog
        private void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }
        }

        private void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
        #endregion

        #region private methods of normal
        /// <summary>
        /// 用指定行给临时变量赋值
        /// </summary>
        /// <param name="rowHandle"></param>
        private void SetTempVarialbesFromRow(int rowHandle)
        {
            if (m_UILogic.CheckRowHandleIsValidate(rowHandle))
            {
                OrderTemp.StartDateTime = m_UILogic.CurrentView[rowHandle].OrderCache.StartDateTime;
                OrderTemp.Content = PersistentObjectFactory.CloneEopBaseObject(
                   m_UILogic.CurrentView[rowHandle].OrderCache.Content) as OrderContent;
            }
            else
            {
                OrderTemp.StartDateTime = DateTime.MinValue;
                OrderTemp.Content = null;
            }
        }

        /// <summary>
        /// 为Unbound列获取指定行、指定列的值
        /// </summary>
        /// <param name="rowIndex">指定的行号</param>
        /// <param name="fieldName">指定的列名</param>
        /// <returns>值</returns>
        private object GetCustomColumnData(int rowIndex, string fieldName)
        {
            //// 现在不在Grid中直接添加新行，所以可以直接从数据集中取数据
            ////return m_UILogic.CurrentOrderTable.GetCustomColumnData(rowIndex, fieldName);

            // 如果是对当前编辑行进行取值，则在通过校验以前使用临时变量中的值给列赋值；
            // 在按ESC取消对当前行编辑时，使用原始记录的值，并且初始化临时变量
            bool indexValidated = m_UILogic.CheckRowHandleIsValidate(rowIndex);
            if ((!indexValidated) || (rowIndex == m_FocusedRowHandle))
            {
                bool useOriginal = (m_HadError && m_HadPressESC && (indexValidated)); // 标记是否使用原始数据
                switch (fieldName)
                {
                    case OrderView.UNStartDate:
                        if (useOriginal) // 在这里给临时变量赋值而不是调用统一的赋值方法，其原因是避免多次调用赋值方法
                            OrderTemp.StartDateTime = m_UILogic.CurrentView[rowIndex].OrderCache.StartDateTime;
                        else if (OrderTemp.StartDateTime.Date == DateTime.MinValue)
                            OrderTemp.StartDateTime = m_UILogic.DefaultStartDateTime;
                        return UILogic.ConvertToDateString(OrderTemp.StartDateTime);
                    case OrderView.UNStartTime:
                        if (useOriginal)
                            OrderTemp.StartDateTime = m_UILogic.CurrentView[rowIndex].OrderCache.StartDateTime;
                        else if (OrderTemp.StartDateTime.TimeOfDay == TimeSpan.Zero)
                            OrderTemp.StartDateTime = m_UILogic.DefaultStartDateTime;
                        return UILogic.ConvertToTimeString(OrderTemp.StartDateTime.TimeOfDay);
                }
            }

            return m_UILogic.CurrentOrderTable.GetCustomColumnData(rowIndex, fieldName);
        }

        /// <summary>
        /// 以统一的方式显示错误消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="errorKind">错误类型</param>
        private void ShowErrorMessage(string message, CustomMessageBoxKind errorKind)
        {
            m_UILogic.CustomMessageBox.MessageShow(message, errorKind);
        }

        /// <summary>
        /// 检查指定的行号是否被选中
        /// </summary>
        /// <param name="rowHandle"></param>
        /// <returns></returns>
        private bool CheckRowIsInSelection(int rowHandle)
        {
            if (advGridView.SelectedRowsCount == 0)
                return false;

            foreach (int handle in advGridView.GetSelectedRows())
                if (handle == rowHandle)
                    return true;

            return false;
        }

        private void PrepareEditNewOrder()
        {
            contentEditor.InitializeDefaultValue(DateTime.MinValue, null);
        }

        private int GetAimPositionOfNewOrder(DataCommitType commitType)
        {
            switch (commitType)
            {
                case DataCommitType.Add:
                    return advGridView.RowCount;
                case DataCommitType.Modify:
                    return m_FocusedRowHandle;
                case DataCommitType.Insert:
                    if (m_FocusedRowHandle < 0)
                        return 0;
                    else
                        return m_FocusedRowHandle + 1;
                default:
                    return -1;
            }
        }

        private object[,] ConvertOrderToObjectArray(Collection<Order> orders)
        {
            object[,] result = new object[orders.Count, 2];
            for (int index = 0; index < orders.Count; index++)
            {
                result[index, 0] = orders[index].Content;
                result[index, 1] = orders[index].GroupPosFlag;
            }
            return result;
        }

        #endregion

        #region private methods of operation process
        private void CommitOrderEdit(DataCommitType commitType)
        {
            try
            {
                UpdateContentFlag updateFlag = UpdateContentFlag.StartDate
                   | UpdateContentFlag.Content;
                OrderTemp.StartDateTime = contentEditor.StartDateTime;
                OrderTemp.Content = contentEditor.NewOrderContent;
                if (!IsTempOrder) // 长期医嘱要设置停止时间
                {
                    DateTime ceaseDate = OrderTemp.StartDateTime.AddDays(contentEditor.ExecuteDays);

                    if (contentEditor.ExecuteDays > 0)
                    {
                        _longOrder.CeaseOrder(m_App.User.DoctorId, ceaseDate, OrderCeaseReason.Natural);
                        updateFlag |= UpdateContentFlag.CeaseDate;
                    }
                    else if ((_longOrder.CeaseInfo != null) && (_longOrder.CeaseInfo.HadInitialized))
                    {
                        // 清空停止日期
                        _longOrder.CancelInfo.SetPropertyValue(null, DateTime.MinValue);
                        updateFlag |= UpdateContentFlag.CeaseDate;
                    }
                }
                // 获得新医嘱要插入的位置或要修改的医嘱所在的位置
                int rowIndex = GetAimPositionOfNewOrder(commitType);

                if (rowIndex != -1)
                {
                    try
                    {
                        m_UILogic.CheckOrderValueBeforeSet(rowIndex, OrderTemp, updateFlag);
                        if (m_CallModel == EditorCallModel.EditOrder)
                            m_UILogic.CheckRecipeRule(OrderTemp.Content);
                    }
                    catch (DataCheckException err)
                    {
                        if (err.WarnningLevel == 0)
                        {
                            if (m_App.CustomMessageBox.MessageShow(err.Message + "\r\n" + "是否继续？", CustomMessageBoxKind.QuestionYesNo)
                               == DialogResult.No)
                                return;
                        }
                        else
                            throw err;
                    }
                    // 更新对应的数据行
                    if (commitType == DataCommitType.Modify)
                        m_UILogic.SetNewOrderElementValue(rowIndex, OrderTemp, updateFlag);
                    else
                    {
                        // 创建新医嘱，插入医嘱表
                        Order newOrder = m_UILogic.CurrentOrderTable.NewOrder();
                        m_UILogic.InitNewOrderValue(newOrder, OrderTemp);
                        m_UILogic.InsertOrder(rowIndex, newOrder);
                        //主动刷新一下医嘱内容
                        newOrder.Content.ResetContentOutputText();
                        // modified by zhouhui 先刷新下gridctrl数据源，再定位新行
                        gridCtrl.RefreshDataSource();
                        advGridView.ClearSelection();
                        advGridView.FocusedRowHandle = rowIndex;
                    }

                    // 重设工具条按钮的状态
                    ResetToolBarItemState();

                    PrepareEditNewOrder();
                    return;
                }
                else
                {
                    throw new DataCheckException(ConstMessages.ExceptionCantInsertOrder, ConstMessages.ExceptionTitleOrder);
                }
            }
            catch (Exception err)
            {
                m_App.CustomMessageBox.MessageShow(err.Message, CustomMessageBoxKind.ErrorOk);
            }

        }

        private void DoSaveData()
        {
            if (m_CallModel == EditorCallModel.EditOrder)
            {
                try
                {
                    ProcessSaveData();
                    HideWaitDialog();
                    ResetToolBarItemState();
                }
                catch (ArgumentNullException err)
                {
                    HideWaitDialog();
                    ShowErrorMessage(err.Message, CustomMessageBoxKind.ErrorOk);
                }
                catch (DataCheckException err)
                {
                    HideWaitDialog();
                    ShowErrorMessage(err.Message, CustomMessageBoxKind.ErrorOk);
                    // 定位到出错的行◎◎◎

                }
                catch
                {
                    HideWaitDialog();
                    ShowErrorMessage(ConstMessages.FailedSaveData, CustomMessageBoxKind.ErrorOk);
                }
            }
            else
            {
                try
                {
                    m_UILogic.SaveCurrentSuiteDetailData();
                }
                catch
                {
                    ShowErrorMessage(ConstMessages.FailedSaveSuiteDetail, CustomMessageBoxKind.ErrorOk);
                }
            }
        }

        private bool ProcessSaveData()
        {
            string caption;
            try
            {
                /* if (m_MedicomPass != null)
                 {
                     CheckNewOrderUseMedicom();
                     if (MedicomDrugInfos.Count > 0)
                     {
                         foreach (MediIntfDrugInfo drugInfo in MedicomDrugInfos)
                         {
                             switch (drugInfo.Warn)
                             {
                                 case PassWarnType.Lower:
                                 case PassWarnType.Higher:
                                 case PassWarnType.Normal:
                                 case PassWarnType.Critical:
                                     if (drugInfo.OrderType == "1")
                                         caption = ConstNames.TempOrder;
                                     else
                                         caption = ConstNames.LongOrder;

                                     if (m_App.CustomMessageBox.MessageShow(String.Format(ConstMessages.FormatMedicomCheckNotPass, caption)
                                        , CustomMessageBoxKind.QuestionYesNo) == DialogResult.No)
                                         throw new DataCheckException(ConstMessages.MsgSaveDataAfterModified, caption);
                                     else
                                         break;
                             }
                         }
                     }
                 }
                 */
                m_UILogic.SaveOrderTableData(false);
                HideWaitDialog();
                //SetToolBarItemStatus();
                return true;
            }
            catch (DataCheckException err)
            {
                HideWaitDialog();
                if (err.WarnningLevel == 0)
                {
                    if (Convert.ToBoolean(err.DataName))
                        caption = ConstNames.TempOrder;
                    else
                        caption = ConstNames.LongOrder;
                    if (m_App.CustomMessageBox.MessageShow(String.Format(ConstMessages.FormatOrderSaveWarning, caption, err.Message), CustomMessageBoxKind.QuestionYesNo)
                       == DialogResult.No)
                        return false;
                    else
                    {
                        m_UILogic.SaveOrderTableData(true);
                        //SetToolBarItemStatus();
                        return true;
                    }
                }
                else
                    throw err;
            }
            catch
            {
                HideWaitDialog();
                throw;
            }
        }

        private void DoSendChangedDataToHIS()
        {
            try
            {
                if (ProcessSaveData())
                {
                    m_UILogic.ManualSynchDataToHIS();
                    HideWaitDialog();
                    ResetToolBarItemState();
                    ShowErrorMessage(ConstMessages.MsgSuccessSendData, CustomMessageBoxKind.InformationOk);
                }
            }
            catch (ArgumentNullException err)
            {
                HideWaitDialog();
                ShowErrorMessage(err.Message, CustomMessageBoxKind.ErrorOk);
            }
            catch (DataCheckException err)
            {
                HideWaitDialog();
                ShowErrorMessage(err.Message, CustomMessageBoxKind.ErrorOk);
            }
            catch
            {
                HideWaitDialog();
                ShowErrorMessage(ConstMessages.FailedSendDataToHis, CustomMessageBoxKind.ErrorOk);
            }
        }
        #endregion

        #region event handle
        private void DoAfterSwitchOrderTable(object sender, EventArgs e)
        {
            if (IsInEditing && (gridCtrl.MainView != null))
            {
                contentEditor.ResetDataOfWordbook();
                PrepareEditNewOrder();
                gridCtrl.DataSource = null;
                gridCtrl.DataSource = m_UILogic.CurrentOrderTable;
                //此处这样处理是为强制让当前的grid获取焦点
                //以触发gridviewgotfoucs事件
                contentEditor.Focus();
                gridCtrl.Focus();
                m_FocusInGrid = true;

                if (m_FocusedRowHandle < 0)
                    ShowOperationHint(m_UILogic.GetHintOfEditOperation(-1));
                // 定位到最后一行
                advGridView.MoveLast();

                FireAfterSwitchOrderTable(sender, e);
            }
        }

        /// <summary>
        /// 医嘱内容的对应的基础数据发生变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AfterContentBaseDataChanged(object sender, EventArgs e)
        {
            // 重设医嘱内容编辑器的基础数据
            contentEditor.ResetDataOfWordbook();
        }

        /// <summary>
        /// 当前编辑的医嘱表是否允许新增标志发生变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AfterCurrentTableAllowNewChanged(object sender, EventArgs e)
        {
            ChangeVisibleOfEditRegion(AllowAddNew);
            FireAfterSwitchOrderTable(this, new EventArgs());
        }

        private void AfterContentEditFinished(object sender, DataCommitArgs e)
        {
            CommitOrderEdit(e.CommitType);
            e.Handled = true;
        }

        private void BeforeContentEditStart(object sender, DataCommitArgs e)
        {
            switch (e.CommitType)
            {
                case DataCommitType.Add:
                    ShowOperationHint(ConstMessages.OpHintAddNewOrder);
                    break;
                case DataCommitType.Modify:
                    ShowOperationHint(ConstMessages.OpHintModifyData);
                    break;
                default:
                    ShowOperationHint("");
                    break;
            }
            e.Handled = true;
            EnableShortCut = false;
        }

        private void DoUILogicProcessStaring(object sender, ProcessHintArgs e)
        {
            SetWaitDialogCaption(e.ProcessHint);
        }

        private void LeaveAdviceEditor(object sender, EventArgs e)
        {
            EnableShortCut = false;
        }

        private void DoAfterSwitchSuite(object sender, EventArgs e)
        {
            if (SuiteHelper.CurrentSuiteNo <= 0)
            {
                Enabled = false;
                EnableShortCut = false;
                orderToolBar.Visible = false;
                statusBar.Visible = false;
                ChangeVisibleOfEditRegion(false);
            }
            else
            {
                Enabled = true;
                orderToolBar.Visible = true;
                statusBar.Visible = (m_CallModel == EditorCallModel.EditOrder);

                IsTempOrder = IsTempOrder;

                InitializeToolbar(m_CallModel);
                barItemLongOrder.Checked = true;

                // 主动刷新工具栏的按钮状态
                ResetToolBarItemState();

                ChangeVisibleOfEditRegion(true);

                HideWaitDialog();
            }
        }
        #endregion

        #region event handle of order grid
        private void GridDataSourceChanged(object sender, EventArgs e)
        {
            if (gridCtrl.DataSource != null)
            {
                // 在进行下面处理前一定要锁定控件，否则会使得添加新行时不能正常刷新
                advGridView.BeginUpdate();

                SetUnboundColumnFieldName();// 设置GridView中Unbound列对应的字段名
                MoveFocusedRowByHand(true);// 将焦点置于新行或最后一条医嘱
                m_FocusedRowHandle = advGridView.FocusedRowHandle;// 预先初始化部分临时变量
                // 切换数据源时，如果长期和临时的记录数一样多，则不会触发FocusedRow改变事件，
                // 也就不能给临时变量赋值，所以在这里强制调用临时变量赋值方法
                SetTempVarialbesFromRow(m_FocusedRowHandle);
                m_UpdateFlag = 0;
                if (m_CallModel != EditorCallModel.EditSuite)
                    ResetGridStyle();// 设置Grid的样式

                advGridView.EndUpdate();

                ChangeVisibleOfEditRegion(AllowAddNew); // 切换编辑区域的Visible状态
            }
        }

        private void GridInitNewRow(object sender, InitNewRowEventArgs e)
        {
            //// 初始化新增医嘱
            //Order newOrder = (advGridView.GetRow(e.RowHandle) as OrderView).OrderCache;

            //m_UILogic.InitNewOrderValue(newOrder, NewStartDateTime, OrderTemp.Content);

            //// 同步开始时间变量
            //OrderTemp.StartDateTime = newOrder.StartDateTime;

            //m_UpdateFlag = UpdateContentFlag.StartDate | UpdateContentFlag.Content;
        }

        private void GridCustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            // 注意：Unbound列的FieldName不能和数据集中的字段名重复。
            // 目前获取Unbound列的值时是通过OrderView对象来处理的，
            // 所以Unbound列的FieldName也不是任意指定的，要符合OrderView中的定义。
            if (e.IsGetData)
            {
                if (e.Column == gridColCheckResult)
                    e.Value = GetDrugWarnPicture(e.ListSourceRowIndex);
                else
                    e.Value = GetCustomColumnData(e.ListSourceRowIndex, e.Column.FieldName);
            }
        }

        private void GridFocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            // 根据聚焦的单元格，判断Grid是否可编辑
            SetGridEditable(e.FocusedColumn.FieldName, advGridView.FocusedRowHandle);
            gridCtrl.Refresh();
        }

        private void GridFocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            // 根据聚焦的单元格，判断Grid是否可编辑
            SetGridEditable(advGridView.FocusedColumn.FieldName, e.FocusedRowHandle);

            //// 因为对新行的内容第一次编辑完成后会触发两次行改变事件，
            //// 为了不清除临时变量,这种情况下需要跳过后面的处理
            //if (((e.FocusedRowHandle == m_UILogic.InvalidRowHandle)
            //      && (e.PrevFocusedRowHandle == m_UILogic.NewItemRowHandle))
            //   || ((e.PrevFocusedRowHandle == m_UILogic.InvalidRowHandle)
            //      && (e.FocusedRowHandle == m_UILogic.NewItemRowHandle)))
            //   return;

            // 初始化临时变量
            m_FocusedRowHandle = e.FocusedRowHandle;
            m_UpdateFlag = 0;

            // 已存在的医嘱记录，则将可编辑的值保存到临时变量，否则清空临时变量（保留时间数据）
            // （进行InitNewRow前会触发两次FocusedRowChanged事件）
            SetTempVarialbesFromRow(e.FocusedRowHandle);
            if (panelContentEditor.Visible && contentEditor.IsModifyData)
                PrepareEditNewOrder();

            //// 删除医嘱集合中开始时间、医嘱内容为空的记录，目的是防止出现多条没有输入完整的新医嘱
            //if ((e.PrevFocusedRowHandle > 0) && (e.PrevFocusedRowHandle != e.FocusedRowHandle))
            //{
            //   advGridView.FocusedRowChanged -= new FocusedRowChangedEventHandler(DoFocusedRowChanged);
            //   advGridView.BeginInit();
            //   m_UILogic.DeleteAbnormalNewOrders();
            //   advGridView.EndInit();
            //   advGridView.FocusedRowChanged += new FocusedRowChangedEventHandler(DoFocusedRowChanged);
            //}

            ShowOperationHint(m_UILogic.GetHintOfEditOperation(m_FocusedRowHandle));
            CloseDrugInfoForm();
            // 重设工具条按钮的状态
            ResetToolBarItemState();
        }

        private void GridValidateRow(object sender, ValidateRowEventArgs e)
        {
            // 对数据进行集中校验
            try
            {
                m_UILogic.CheckOrderValueBeforeSet(e.RowHandle, OrderTemp, m_UpdateFlag);
                e.Valid = true;
                //// (现在只允许在Grid中编辑开始时间，时间修改后直接更新Row，所以不需要再去更新数据)
                // 更新对应的数据行
                m_UILogic.SetNewOrderElementValue(e.RowHandle, OrderTemp, m_UpdateFlag);
            }
            catch (DataCheckException err)
            {
                if (m_FocusInGrid)
                {
                    // 要清空上次的提示信息
                    if (advGridView.HasColumnErrors)
                        advGridView.ClearColumnErrors();

                    advGridView.SetColumnError(advGridView.Columns[err.DataName], err.Message);
                }
                else
                {
                    advGridView.DeleteRow(e.RowHandle);
                }
                e.Valid = false;
            }
            m_HadError = !e.Valid;
            m_HadPressESC = false;
        }

        private void GridRowDoubleClick(object sender, EventArgs e)
        {
            if ((m_UILogic.AllowEdit) && (FocusedOrder != null) && (FocusedOrder.State == OrderState.New)
               && AllowAddNew) // 申请单需要调用申请单编辑,而且申请单医嘱不能直接删除,
            // 所以copy了下面的条件语句并去掉了对DeleteItem的Enabled判断
            {
                TempOrder tmp = FocusedOrder as TempOrder;
                //if (tmp != null && tmp.ApplySerialNo != 0)
                //{
                //   NetWorkStudio.Logic.RequestOrder.CallWebWindowHelper callWebWindowHelper = new NetWorkStudio.Logic.RequestOrder.CallWebWindowHelper();
                //   if (tmp.ApplySerialNo < 0) //检查
                //      // NetWorkStudio.Logic.RequestOrderInterface.CallWebWindow.ShowCheckListEditFrm((-tmp.ApplySerialNo).ToString(), m_App);
                //      callWebWindowHelper.ShowCheckListEditFrm((-tmp.ApplySerialNo).ToString(), m_App);
                //   else
                //      //NetWorkStudio.Logic.RequestOrderInterface.CallWebWindow.ShowInspectionEditFrm(tmp.ApplySerialNo.ToString(), m_App);
                //      callWebWindowHelper.ShowInspectionEditFrm(tmp.ApplySerialNo.ToString(), m_App);
                //   return;
                //}
            }

            if ((m_UILogic.AllowEdit) && (FocusedOrder != null) && (FocusedOrder.State == OrderState.New)
               && AllowAddNew && barItemDelete.Enabled) // 能删除表示能编辑
            {
                ChangeVisibleOfEditRegion(true);

                SetTempVarialbesFromRow(m_FocusedRowHandle);
                contentEditor.EditContent(OrderTemp, IsTempOrder);
                //contentEditor.EditContent(FocusedOrder.StartDateTime, FocusedOrder.Content);
                //if ((!IsTempOrder) && (_longOrder.CeaseInfo != null) && (_longOrder.CeaseInfo.HadInitialized))
                //{
                //   TimeSpan days = _longOrder.CeaseInfo.ExecuteTime - _longOrder.StartDateTime;
                //   contentEditor.ExecuteDays = (int)days.TotalDays;
                //}
            }
        }

        private void GridKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
                m_HadPressESC = true;
        }

        private void SetInvalidRowExceptionMode(object sender, InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = ExceptionMode.NoAction;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 重设工具条按钮的状态
            ResetToolBarItemState();
        }

        private void GridContentEditPopup(object sender, CancelEventArgs e)
        {
            //// 在弹出编辑界面前进行适当的初始化
            //if ((advGridView.FocusedRowHandle == m_UILogic.NewItemRowHandle)
            //   || (advGridView.FocusedRowHandle == m_UILogic.InvalidRowHandle))
            //{
            //   if (OrderTemp.Content == null) // 新增医嘱的类别和上一条医嘱保持一致
            //   {
            //      if (advGridView.RowCount == m_UILogic.CurrentView.Count)
            //         OrderTemp.Content = m_UILogic.CreateDefaultContent(advGridView.FocusedRowHandle);
            //      else // FocusedRowHandle为新行时，有可能没有在DataSource中添加行，所以要特殊处理
            //         OrderTemp.Content = m_UILogic.CreateDefaultContent(-1);
            //   }
            //   contentEditor.EditContent(OrderTemp.Content, false);
            //}
            //else
            //{
            //   contentEditor.EditContent(OrderTemp.Content, true);
            //}
        }

        private void GridContentEditClose(object sender, CloseUpEventArgs e)
        {
            //m_FocusInGrid = true;
            //OrderTemp.Content = contentEditor.NewOrderContent;
            //e.Value = contentEditor.NewOrderContent.ToString();
            //m_UpdateFlag |= UpdateContentFlag.Content;

            //// 编辑新行时，如果医嘱内容已经有内容，则自动完成添加
            //if (((m_FocusedRowHandle == m_UILogic.InvalidRowHandle)
            //   || (m_FocusedRowHandle == m_UILogic.NewItemRowHandle))
            //   && (OrderTemp.Content != null))
            //{
            //   SendKeys.Send("{DOWN}");
            //   SendKeys.Send("{RIGHT}");
            //   SendKeys.Send("{LEFT}");
            //}
        }

        private void StartTimeEditValueChanging(object sender, ChangingEventArgs e)
        {
            if (e.NewValue.ToString().Length > 0)
            {
                OrderTemp.StartDateTime = OrderTemp.StartDateTime.Date
                   + Convert.ToDateTime(e.NewValue, CultureInfo.CurrentCulture).TimeOfDay;
                //FocusedOrder.StartDateTime = FocusedOrder.StartDateTime.Date
                //   + Convert.ToDateTime(e.NewValue, CultureInfo.CurrentCulture).TimeOfDay;
                m_UpdateFlag |= UpdateContentFlag.StartDate;

            }
        }

        private void StartDateEditValueChanging(object sender, ChangingEventArgs e)
        {
            if (e.NewValue.ToString().Length > 0)
            {
                OrderTemp.StartDateTime = Convert.ToDateTime(e.NewValue, CultureInfo.CurrentCulture).Date
                   + OrderTemp.StartDateTime.TimeOfDay;
                //FocusedOrder.StartDateTime = Convert.ToDateTime(e.NewValue, CultureInfo.CurrentCulture).Date
                //   + FocusedOrder.StartDateTime.TimeOfDay;
                m_UpdateFlag |= UpdateContentFlag.StartDate;
            }
        }

        private void GridViewGotFocus(object sender, EventArgs e)
        {
            m_FocusInGrid = true;
            EnableShortCut = true;
        }

        private void GridViewLostFocus(object sender, EventArgs e)
        {
            m_FocusInGrid = false;
        }
        #endregion

        #region event handle & methods by customdraw
        private void CustomDrawOrderGridCell(object sender, RowCellCustomDrawEventArgs e)
        {
            // 新行、但不是聚焦的行，则表示是新行，还没有数据
            if ((e.RowHandle == m_UILogic.InvalidRowHandle)
               || ((e.RowHandle == m_UILogic.NewItemRowHandle)
               && (e.RowHandle != m_FocusedRowHandle)))
                return;

            switch (e.Column.FieldName)
            {
                case OrderView.UNContent:// 对医嘱内容进行重画
                    DoCustomDrawOrderGridContent(e);
                    break;
                default:
                    if (!CustomDrawOperation.DrawSetting.ShowRepeatInfo)
                        DoCustomDrawOrderGridRepeatRowInfo(e);
                    break;
            }
        }

        private void DoCustomDrawOrderGridRepeatRowInfo(RowCellCustomDrawEventArgs e)
        {
            if ((e.RowHandle > 0) && (e.RowHandle != advGridView.FocusedRowHandle))
            {
                OrderView preView = advGridView.GetRow(e.RowHandle - 1) as OrderView;
                OrderView curView = advGridView.GetRow(e.RowHandle) as OrderView;

                // 可以处理以下内容：
                // 开始日期、创建者、审核日期和时间、审核者、执行日期和时间、执行者、
                // 停止日期和时间、停止者、停止审核者

                switch (e.Column.FieldName)
                {
                    case OrderView.UNStartDate:
                        if ((curView.Creator == preView.Creator) && (curView.StartDate == preView.StartDate))
                            e.DisplayText = CustomDrawOperation.DrawSetting.ReplaceOfRepeatInfo;
                        break;
                    case OrderView.UNStartTime:
                        if ((curView.Creator == preView.Creator) && (curView.StartDate == preView.StartDate) && (curView.StartTime == preView.StartTime))
                            e.DisplayText = CustomDrawOperation.DrawSetting.ReplaceOfRepeatInfo;
                        break;
                    case OrderView.ColCreator:
                        if ((curView.Creator == preView.Creator) && (curView.StartDate == preView.StartDate))
                            e.DisplayText = CustomDrawOperation.DrawSetting.ReplaceOfRepeatInfo;
                        break;
                    case OrderView.UNCeaseDate:
                        if ((!String.IsNullOrEmpty(curView.Ceasor)) && (curView.Ceasor == preView.Ceasor)
                           && (curView.CeaseDate == preView.CeaseDate))
                            e.DisplayText = CustomDrawOperation.DrawSetting.ReplaceOfRepeatInfo;
                        break;
                    case OrderView.UNCeaseTime:
                        if ((!String.IsNullOrEmpty(curView.Ceasor)) && (curView.Ceasor == preView.Ceasor)
                           && (curView.CeaseDate == preView.CeaseDate) && (curView.CeaseTime == preView.CeaseTime))
                            e.DisplayText = CustomDrawOperation.DrawSetting.ReplaceOfRepeatInfo;
                        break;
                    case OrderView.ColCeasor:
                        if ((!String.IsNullOrEmpty(curView.Ceasor)) && (curView.Ceasor == preView.Ceasor)
                           && (curView.CeaseDate == preView.CeaseDate))
                            e.DisplayText = CustomDrawOperation.DrawSetting.ReplaceOfRepeatInfo;
                        break;
                }
            }

        }

        private void DoCustomDrawOrderGridContent(RowCellCustomDrawEventArgs e)
        {
            SolidBrush foreBrushNormal, foreBrushCancel, foreBrushGroup, backBrush;
            //// 1 画医嘱内容数据
            ////    分3种情况：1)未选中 2)被选中，但不处于Focused列 3)被选中，且处于Focused列
            //bool isFocused = ((e.RowHandle == advGridView.FocusedRowHandle)
            //   && (e.Column == advGridView.FocusedColumn)); // 是否是聚焦的单元格

            //// 设置合适的前景色(处于选中时还要判断是否处于可编辑的状态)
            //if (CheckRowIsInSelection(e.RowHandle)
            //   && (((!advGridView.Editable) && (isFocused))
            //         || (!isFocused)))
            //{
            //   foreBrushNormal = new SolidBrush(CustomDrawOperation.DrawSetting.ForeColor.ForeColor);
            //   foreBrushCancel = new SolidBrush(CustomDrawOperation.DrawSetting.CancelledColor.ForeColor);
            //   foreBrushGroup = new SolidBrush(CustomDrawOperation.DrawSetting.GroupFlagColor.ForeColor);
            //}
            //else
            //{
            //   foreBrushNormal = new SolidBrush(CustomDrawOperation.DrawSetting.ForeColor.BackColor);
            //   foreBrushCancel = new SolidBrush(CustomDrawOperation.DrawSetting.CancelledColor.BackColor);
            //   foreBrushGroup = new SolidBrush(CustomDrawOperation.DrawSetting.GroupFlagColor.BackColor);
            //}
            //// Focused的时候要改变默认的背景色
            //if (advGridView.Editable && isFocused)
            //{
            //   backBrush = new SolidBrush(CustomDrawOperation.DrawSetting.BackColor.BackColor);
            //   e.Graphics.FillRectangle(backBrush, e.Bounds);
            //}

            //// 如果是当前编辑的行，则用临时变量的值
            //OrderContent content;
            //if (e.RowHandle == m_FocusedRowHandle)
            //   content = OrderTemp.Content;
            //else
            //   content = m_UILogic.CurrentView[e.RowHandle].OrderCache.Content;

            // 现在不论是否选中或Focused，都使用固定的颜色
            OrderView currentRow = m_UILogic.CurrentView[e.RowHandle];
            OrderState state = currentRow.State;
            if (state == OrderState.Cancellation)
                state = OrderState.Audited;
            foreBrushNormal = new SolidBrush(CustomDrawOperation.GetForeColorByState(state, false, currentRow.HadSynch));
            foreBrushCancel = new SolidBrush(CustomDrawOperation.GetForeColorByState(OrderState.Cancellation, false, currentRow.HadSynch));
            foreBrushGroup = new SolidBrush(CustomDrawOperation.DrawSetting.GroupFlagColor.BackColor);

            backBrush = new SolidBrush(CustomDrawOperation.GetBackColorByState(state, false, currentRow.HadSynch));
            e.Graphics.FillRectangle(backBrush, e.Bounds);

            // 现在不在数据行直接编辑数据，所以直接使用数据集中的数据
            OrderContent content = currentRow.OrderCache.Content;
            if (content == null)
                return;

            foreach (OutputInfoStruct output in content.Outputs)
            {
                if (output.OutputType == OrderOutputTextType.NormalText)
                    e.Appearance.DrawString(e.Cache, output.Text
                       , CellBoundsToGrid(e.Bounds, output.Bounds)
                       , output.Font, foreBrushNormal, new StringFormat());
                else if (output.OutputType == OrderOutputTextType.CancelInfo)
                    e.Appearance.DrawString(e.Cache, output.Text
                       , CellBoundsToGrid(e.Bounds, output.Bounds)
                       , output.Font, foreBrushCancel, new StringFormat());
                else
                    e.Graphics.FillRectangle(foreBrushGroup
                       , CellBoundsToGrid(e.Bounds, output.Bounds));
            }
            e.Handled = true;
        }

        private static Rectangle CellBoundsToGrid(Rectangle bounds, Rectangle outputBounds)
        {
            return new Rectangle(bounds.X + outputBounds.X
               , bounds.Y + outputBounds.Y
               , outputBounds.Width
               , outputBounds.Height);
        }

        private void CustomAllergicGridCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if ((e.RowHandle != GridControl.InvalidRowHandle)
               && (e.RowHandle != GridControl.NewItemRowHandle))
            {
                DataRow row = gridViewAllergic.GetDataRow(e.RowHandle);
                if ((row != null) && (row[ConstSchemaNames.SkinTestColFlag].ToString() == ConstNames.LightDemanding))
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
        }
        #endregion

        #region event handle of barItems
        private void ItemDeleteNewOrderClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            try
            {
                int[] rowHandles = advGridView.GetSelectedRows();
                if (rowHandles == null)
                    return;
                advGridView.BeginDataUpdate();
                m_UILogic.DeleteNewOrder(rowHandles);
                advGridView.EndDataUpdate();
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void ItemSetGroupClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            // 首先检查是否可以成组，然后再设置为组
            try
            {
                int[] rowHandles = advGridView.GetSelectedRows();
                if ((rowHandles == null) || (rowHandles.Length == 1))
                    return;
                m_UILogic.SetOrderGroup(rowHandles);
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void ItemAutoGroupClick(object sender, ItemClickEventArgs e)
        {
            // 处于编辑状态
            if (AllowAddNew)
            {
                advGridView.ClearSelection();
                int[] rowHandles = m_UILogic.AutoSetNewOrderGrouped();
                if ((rowHandles != null) && (rowHandles.Length > 0))
                    advGridView.SelectRange(rowHandles[0], rowHandles[rowHandles.Length - 1]);
                else
                    MoveFocusedRowByHand(true);
            }
        }

        private void ItemAuditClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            try
            {
                int[] rowHandles = advGridView.GetSelectedRows();
                if (rowHandles == null)
                    return;
                m_UILogic.AuditOrder(rowHandles);
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void ItemMoveNewOrderUpClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            try
            {
                int[] rowHandles = advGridView.GetSelectedRows();
                if (rowHandles == null)
                    return;

                int acturalSteps = m_UILogic.MoveNewOrderUp(rowHandles);
                // 重新选中被移动的记录（整体倒退 acturalSteps 步）
                advGridView.BeginSelection();
                advGridView.ClearSelection();

                advGridView.SelectRange(rowHandles[0] - acturalSteps, rowHandles[0] + rowHandles.Length - 1 - acturalSteps);

                advGridView.FocusedRowHandle = advGridView.GetPrevVisibleRow(advGridView.FocusedRowHandle);
                advGridView.EndSelection();
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void ItemMoveNewOrderDownClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            try
            {
                int[] rowHandles = advGridView.GetSelectedRows();
                if (rowHandles == null)
                    return;

                int acturalSteps = m_UILogic.MoveNewOrderDown(rowHandles);
                // 重新选中移动的记录（整体前进 acturalSteps 步）
                advGridView.BeginSelection();
                advGridView.ClearSelection();

                advGridView.SelectRange(rowHandles[0] + acturalSteps, rowHandles[0] + rowHandles.Length - 1 + acturalSteps);

                advGridView.FocusedRowHandle = advGridView.GetNextVisibleRow(advGridView.FocusedRowHandle);
                advGridView.EndSelection();
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void ItemCancelGroupClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            try
            {
                int[] rowHandles = advGridView.GetSelectedRows();
                if (rowHandles == null)
                    return;
                m_UILogic.CancelOrderGroup(rowHandles);
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void ItemCeaseClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            if (m_TimeInputForm == null)
            {
                m_TimeInputForm = new DateTimeInputForm();
                m_TimeInputForm.App = m_App;
            }
            if (m_TimeInputForm.InputDateTime < DateTime.Now)
                m_TimeInputForm.InputDateTime = m_UILogic.DefaultCeaseOrderTime;
            if (m_TimeInputForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int[] rowHandles = advGridView.GetSelectedRows();
                    if (rowHandles == null)
                        return;
                    m_UILogic.SetLongOrderCeaseInfo(rowHandles, m_TimeInputForm.InputDateTime);
                }
                catch (Exception err)
                {
                    ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
                }
            }
        }

        private void ItemCancelClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            try
            {
                int[] rowHandles = advGridView.GetSelectedRows();
                if (rowHandles == null)
                    return;
                m_UILogic.CancelOrder(rowHandles);
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void ItemFilterOrderCatalogClick(object sender, EventArgs e)
        {
            // 改变Grid关联的医嘱数据集
            if (barItemFilterStatus.EditValue == null)
                m_UILogic.SwitchOrderTable((barItemOrderCatalog.EditValue.ToString() == ConstNames.TempOrder)
                   , OrderState.All);
            else
                m_UILogic.SwitchOrderTable((barItemOrderCatalog.EditValue.ToString() == ConstNames.TempOrder)
                   , (OrderState)barItemFilterStatus.EditValue);
        }

        private void ItemFilterOrderStateClick(object sender, EventArgs e)
        {
            // 同步OrderTableView中的状态
            m_UILogic.FilterOrderByState((OrderState)barItemFilterStatus.EditValue);
        }

        private void ItemSaveClick(object sender, ItemClickEventArgs e)
        {
            DoSaveData();
        }

        private void ItemSubmitClick(object sender, ItemClickEventArgs e)
        {
            DoSendChangedDataToHIS();
        }

        private void ItemStateAvailablyClick(object sender, ItemClickEventArgs e)
        {
            if (OrderState != OrderState.Executed)
                OrderState = OrderState.Executed;
        }

        private void ItemStateNewClick(object sender, ItemClickEventArgs e)
        {
            if (OrderState != OrderState.New)
                OrderState = OrderState.New;
        }

        private void ItemStateAllClick(object sender, ItemClickEventArgs e)
        {
            if (OrderState != OrderState.All)
                OrderState = OrderState.All;
        }

        private void ItemTempOrderClick(object sender, ItemClickEventArgs e)
        {
            if (!IsTempOrder)
                IsTempOrder = true;
        }

        private void ItemLongOrderClick(object sender, ItemClickEventArgs e)
        {
            if (IsTempOrder)
                IsTempOrder = false;
        }

        private void ItemCutClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            // 将选中医嘱从医嘱表中移出，保存到缓存中
            try
            {
                OrderClipboard.Clear();
                int[] rowHandles = advGridView.GetSelectedRows();
                if (rowHandles == null)
                    return;
                Order[] orders = m_UILogic.CutOrdersFromList(rowHandles);
                if (orders != null)
                {
                    foreach (Order order in orders)
                        OrderClipboard.Add(order);
                    barItemPaste.Enabled = (OrderClipboard.Count > 0);
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void ItemCopyClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            // 克隆选中的医嘱，保存到缓存中
            try
            {
                OrderClipboard.Clear();
                int[] rowHandles = advGridView.GetSelectedRows();
                if (rowHandles == null)
                    return;
                Order[] orders = m_UILogic.CopyOrdersFromList(rowHandles);
                if (orders != null)
                {
                    foreach (Order order in orders)
                        OrderClipboard.Add(order);
                    barItemPaste.Enabled = (OrderClipboard.Count > 0);
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void ItemPasteClick(object sender, ItemClickEventArgs e)
        {
            //if (!EnableShortCut)
            //   return;
            if (OrderClipboard.Count == 0)
                return;

            try
            {
                int[] rowHandles = advGridView.GetSelectedRows();
                m_UILogic.CheckAllowInsertOrder(rowHandles);

                // 调出修改界面，完成插入工作（插入到当前行的下一行）
                if (SuiteEditForm.CallOrderSuiteEditForm(true, ConvertOrderToObjectArray(OrderClipboard)
                   , m_UILogic.FrequencyWordbook) == DialogResult.OK)
                {
                    object[,] contents = SuiteEditForm.SelectedContents;
                    Order focusedOrder;
                    if ((rowHandles != null) && (rowHandles.Length > 0))
                        focusedOrder = m_UILogic.CurrentOrderTable.Orders[rowHandles[rowHandles.Length - 1]];
                    else
                        focusedOrder = null;

                    m_UILogic.InsertSuiteOrder(contents, focusedOrder);
                    MoveFocusedRowByHand(false);
                }
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void ItemRefreshClick(object sender, ItemClickEventArgs e)
        {
            //CurrentPatient = null; //先清空病人再调初始化
            //CallShowPatientOrder(CurrentPatient);
            InitializePatientData(CurrentPatient);
        }

        private void ItemPrintClick(object sender, ItemClickEventArgs e)
        {
            if (PrintCurrentPatientOrder != null)
                PrintCurrentPatientOrder(CurrentPatient);
        }

        private void ItemExpandHerbDetailClick(object sender, ItemClickEventArgs e)
        {
            m_UILogic.CurrentView.ExpandHerbDetail(m_FocusedRowHandle);
        }

        private void ItemExpandAllHerbClick(object sender, ItemClickEventArgs e)
        {
            m_UILogic.CurrentView.ExpandAllHerbDetail();
        }

        private void ItemCollapseHerbDetailClick(object sender, ItemClickEventArgs e)
        {
            m_UILogic.CurrentView.CollapseHerbDetail(m_FocusedRowHandle);
        }

        private void ItemCollapseAllHerbClick(object sender, ItemClickEventArgs e)
        {
            m_UILogic.CurrentView.CollapseAllHerbDetail();
        }
        #endregion

        #region private methods of medicompass
        /*
        private bool InitializeMedicomPass()
        {
            if (m_MedicomPass != null)
                return true;

            SetWaitDialogCaption(ConstMessages.HintInitMedicom);
            try
            {
                if (m_MedicomPass == null)
                    m_MedicomPass = new PassComponent();

                return m_MedicomPass.InitializePassIntf(m_App.User.CurrentDeptId
                   , m_App.User.CurrentDeptName
                   , m_App.User.DoctorId
                   , m_App.User.DoctorName);
            }
            catch
            {
                HideWaitDialog();
                m_App.CustomMessageBox.MessageShow(ConstMessages.FailedInitMedicom, CustomMessageBoxKind.ErrorOk);
                return false;
            }
        }
        
        private void SetMedicomPassPatient()
        {
            if (m_MedicomPass != null)
                m_MedicomPass.PassCheckHelper.PassSetPatient(ConvertPatientToStruct(CurrentPatient));
        }
        
        private static MediIntfPatientInfo ConvertPatientToStruct(Inpatient patient)
        {
            MediIntfPatientInfo result = new MediIntfPatientInfo();
            result.PatientID = patient.NoOfFirstPage.ToString();
            result.Birthday = patient.PersonalInformation.Birthday.ToString(ConstFormat.FullDate, CultureInfo.CurrentCulture);
            result.DeptName = patient.InfoOfAdmission.DischargeInfo.CurrentDepartment.Name;
            //result.Doctor = patient.InHos.

            return result;
        }
        */
        private void DoAfterGridCtrlMouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hitInfo = advGridView.CalcHitInfo(e.X, e.Y);
            if (hitInfo.InRowCell && (hitInfo.Column == gridColCheckResult))
            {
                /*
                if ((MedicomDrugInfos.Count > 0)
                   && (advGridView.GetRowCellValue(hitInfo.RowHandle, hitInfo.Column) != null))
                {
                    string serialNo;
                    if (IsTempOrder)
                        serialNo = m_UILogic.CurrentView[hitInfo.RowHandle].SerialNo.ToString();
                    else
                        serialNo = (-m_UILogic.CurrentView[hitInfo.RowHandle].SerialNo).ToString();

                   
                    foreach (MediIntfDrugInfo drugInfo in MedicomDrugInfos)
                    {
                        if (drugInfo.OrderUniqueCode == serialNo)
                            m_MedicomPass.PassCheckHelper.PassSetWarnDrug(serialNo);
                    }
                    
                }
                */
            }
        }

        private void DoShowPlugDrugInfoMenu(object sender, ItemClickEventArgs e)
        {
            /* 
            if ((FocusedOrder.Content != null)
               && ((FocusedOrder.Content.OrderKind == OrderContentKind.Druggery)
                  || (FocusedOrder.Content.OrderKind == OrderContentKind.OutDruggery)))
            {
                
                if ((FocusedOrder.Content.Item != null) && (FocusedOrder.Content.Item.KeyInitialized))
                    m_MedicomPass.PassCheckHelper.CurrentDrugIndex = FocusedOrder.Content.Item.KeyValue;
                else
                    m_MedicomPass.PassCheckHelper.CurrentDrugIndex = "";
            }
            //如果没有选中药品，则禁用某些菜单项！！！

            m_MedicomPass.PassContextMenu.Show(Cursor.Position);
             */
        }

        private void DoCheckOrders(object sender, ItemClickEventArgs e)
        {
            try
            {
                CheckNewOrderUseMedicom();
            }
            catch (Exception err)
            {
                ShowErrorMessage(err.Message, CustomMessageBoxKind.WarningOk);
            }
        }

        private void CheckNewOrderUseMedicom()
        {
            CreateMedicomDrugInfoData();
            /*
            if (MedicomDrugInfos.Count > 0)
            {
                if (!m_MedicomPass.PassCheckHelper.PassSetRecipeInfos(MedicomDrugInfos))
                    throw new ArgumentNullException(ConstMessages.ExceptionFailedSendDataToMedicom);
                m_MedicomPass.PassCheckHelper.DoPassCheck(PassCheckType.HospSubmitAuto);
                advGridView.BeginUpdate(); // 通过刷新的方式改变检查结果的图片
                advGridView.EndUpdate();
            }
             */
        }

        private void DoShowDrugInfo(object sender, ItemClickEventArgs e)
        {
            /*
            if ((m_FocusedRowHandle > 0) && (m_MedicomPass != null))
            {
                OrderContent content = m_UILogic.CurrentView[m_FocusedRowHandle].OrderCache.Content;
                if ((content.OrderKind == OrderContentKind.Druggery) || (content.OrderKind == OrderContentKind.OutDruggery))
                {
                    // 传入药品信息
                    // 设置浮动窗口位置
                    // 显示信息
                    Rectangle cellRange = CustomDrawOperation.GetGridCellRect(advGridView, m_FocusedRowHandle, gridColContent);
                    m_MedicomPass.PassCheckHelper.PassQueryDrugInfo(content.Item.KeyValue, content.Item.Name
                       , content.CurrentUnit.Name, content.ItemUsage.Name
                       , new Point(cellRange.X + cellRange.Width, cellRange.Y)
                       , new Point(cellRange.X + cellRange.Width + 200, cellRange.Y + 150));
                }
            }
             */
        }

        private void DoAfterContentEditorSelectedItemChanged(object sender, OrderItemArgs e)
        {
            /*
            if (m_MedicomPass != null)
            {
                if (e.HadData)
                {
                    switch (e.Kind)
                    {
                        case ItemKind.WesternMedicine:
                        case ItemKind.PatentMedicine:
                        case ItemKind.HerbalMedicine:
                            Rectangle client = ClientRectangle;
                            Point rightBottom = new Point(client.Width, client.Height - 30 - panelContentEditor.Height);
                            Point leftTop = new Point(rightBottom.X - 200, rightBottom.Y - 150);
                            m_MedicomPass.PassCheckHelper.PassQueryDrugInfo(e.ItemCode, e.ItemName, e.DoseUnit, e.Usage
                               , PointToScreen(leftTop), PointToScreen(rightBottom));
                            //, new Point(500, 300), new Point(700, 450));
                            break;
                        default:
                            CloseDrugInfoForm();
                            break;
                    }
                }
                else
                    CloseDrugInfoForm();
            }
             */
        }

        private Image GetDrugWarnPicture(int rowHandle)
        {
            /*
            if ((MedicomDrugInfos.Count > 0) && (rowHandle >= 0))
            {
                decimal serialNo = m_UILogic.CurrentView[rowHandle].SerialNo;
                if (!IsTempOrder)
                    serialNo = -serialNo;

                foreach (MediIntfDrugInfo drugInfo in MedicomDrugInfos)
                {
                    if (drugInfo.OrderUniqueCode == serialNo.ToString())
                        return m_MedicomPass.PassCheckHelper.GetWarnBmp(drugInfo.Warn);
                }
            }
             */

            return null;

            //// 切换数据集后都要调用此方法
            //if (MedicomDrugInfos.Count > 0)
            //{
            //   decimal serialNo;
            //   int rowHandle;
            //   foreach (MediIntfDrugInfo drugInfo in MedicomDrugInfos)
            //   {
            //      if ((drugInfo.OrderType == "1") == IsTempOrder)
            //      {
            //         serialNo = Convert.ToDecimal(drugInfo.OrderUniqueCode);
            //         if (serialNo < 0)
            //            serialNo = -serialNo;
            //         rowHandle = m_UILogic.CurrentView.IndexOf(serialNo);                  
            //         if (rowHandle > 0)
            //            advGridView.SetRowCellValue(rowHandle, gridColCheckResult
            //               , m_MedicomPass.PassCheckHelper.GetWarnBmp(drugInfo.Warn));
            //      }
            //   }
            //}
        }

        private void CloseDrugInfoForm()
        {
            /*
            if ((m_MedicomPass != null) && (m_MedicomPass.PassCheckHelper.CurrentDrugIndex != null))
                m_MedicomPass.PassCheckHelper.DoCommand(402);
             */
        }

        private void CreateMedicomDrugInfoData()
        {
            //MedicomDrugInfos.Clear();
            Order temp;
            // 插入新临时医嘱
            OrderTable table = m_UILogic.GetCurrentOrderTable(true);
            for (int index = table.Orders.Count - 1; index >= 0; index--)
            {
                temp = table.Orders[index];
                if (temp.State != OrderState.New)
                    break;
                /*
                if ((temp.Content.OrderKind == OrderContentKind.Druggery)
                   || (temp.Content.OrderKind == OrderContentKind.OutDruggery))
                    MedicomDrugInfos.Insert(0, ConvertTempOrderToMediIntfDrugInfo(temp));
                 */
            }
            // 插入新长期医嘱
            table = m_UILogic.GetCurrentOrderTable(false);
            for (int index = table.Orders.Count - 1; index >= 0; index--)
            {
                temp = table.Orders[index];
                if (temp.State != OrderState.New)
                    break;
                /*
                if (temp.Content.OrderKind == OrderContentKind.Druggery)
                    MedicomDrugInfos.Insert(0, ConvertLongOrderToMediIntfDrugInfo((temp as LongOrder)));
                 */
            }
        }
        /*
        private MediIntfDrugInfo ConvertOrderToMediIntfDrugInfo(Order temp, bool isTemp)
        {
            MediIntfDrugInfo drugInfo = new MediIntfDrugInfo();
            if (isTemp)
                drugInfo.OrderUniqueCode = temp.SerialNo.ToString();//医嘱唯一码（必须传值）(长期、临时不能重复)
            else
                drugInfo.OrderUniqueCode = (-temp.SerialNo).ToString(); // 用负数表示长期医嘱的序号
            drugInfo.DrugCode = temp.Content.Item.KeyValue; //药品编码 （必须传值） 
            drugInfo.DrugName = temp.Content.Item.Name; //药品名称 （必须传值）
            drugInfo.SingleDose = temp.Content.Amount.ToString(); //每次用量 （必须传值）
            drugInfo.DoseUnit = temp.Content.CurrentUnit.Name; // 剂量单位 （必须传值）
            drugInfo.Frequency = temp.Content.ItemFrequency.ConvertToMedicomFrequency(); //用药频率(次/天)（必须传值） 
            drugInfo.StartDate = temp.StartDateTime.ToString(ConstFormat.FullDate); //用药开始日期，格式：yyyy-mm-dd （必须传值） 
            drugInfo.RouteName = temp.Content.ItemUsage.Name; // 给药途径中文名称 （必须传值） 
            drugInfo.GroupTag = temp.GroupSerialNo.ToString(); // 成组医嘱标志 （必须传值）
            drugInfo.OrderDoctorId = temp.CreateInfo.Executor.Code;// 下嘱医生ID/下嘱医生姓名 （必须传值）
            drugInfo.OrderDoctorName = temp.CreateInfo.Executor.Name;
            return drugInfo;
        }

        private MediIntfDrugInfo ConvertTempOrderToMediIntfDrugInfo(Order order)
        {
            MediIntfDrugInfo drugInfo = ConvertOrderToMediIntfDrugInfo(order, true);
            drugInfo.OrderType = "1";// 是否为临时医嘱 1-是临时医嘱 0或空 长期医嘱 （必须传值）
            return drugInfo;
        }

        private MediIntfDrugInfo ConvertLongOrderToMediIntfDrugInfo(LongOrder longOrder)
        {
            MediIntfDrugInfo drugInfo = ConvertOrderToMediIntfDrugInfo(longOrder, false);
            if ((longOrder.CeaseInfo != null) && (longOrder.CeaseInfo.HadInitialized))
                drugInfo.EndDate = longOrder.CeaseInfo.ExecuteTime.ToString(ConstFormat.FullDate); //用药结束日期，格式：yyyy-mm-dd （可以不传值），默认值为当天 
            drugInfo.OrderType = "0";// 是否为临时医嘱 1-是临时医嘱 0或空 长期医嘱 （必须传值）
            return drugInfo;
        }
        */
        #endregion

        #region ISupportInitialize Members

        public void BeginInit()
        { }

        public void EndInit()
        { }

        #endregion

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-22</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advGridView_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}