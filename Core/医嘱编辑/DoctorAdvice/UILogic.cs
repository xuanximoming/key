using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Text;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Common.Eop;
using DrectSoft.Wordbook;
using System.Xml.Serialization;
using DrectSoft.FrameWork.WinForm.Plugin;
using Eop = DrectSoft.Common.Eop;
namespace DrectSoft.Core.DoctorAdvice
{
   /// <summary>
   /// 界面处理逻辑类。提供与界面显示、控件属性控制有关的算法、事件处理等。
   /// </summary>
   internal class UILogic
   {
      #region const & readonly
      /// <summary>
      /// 表示非正式行的行号
      /// </summary>
      public readonly int InvalidRowHandle;
      /// <summary>
      /// 表示处于新增状态的行的行号
      /// </summary>
      public readonly int NewItemRowHandle;
      #endregion

      #region static methods
      /// <summary>
      /// 统一格式化日期字符串的显示形式
      /// </summary>
      /// <param name="sourceDate"></param>
      /// <returns>默认格式的日期串"年-月-日"</returns>
      public static string ConvertToDateString(DateTime sourceDate)
      {
         return sourceDate.ToString(ConstFormat.ShortDateWithCentry, CultureInfo.CurrentCulture);
      }

      /// <summary>
      /// 统一格式化时间字符串的显示形式
      /// </summary>
      /// <param name="sourceTime"></param>
      /// <returns>默认格式的时间串"时:分"</returns>
      public static string ConvertToTimeString(TimeSpan sourceTime)
      {
         if (sourceTime == TimeSpan.MinValue)
            return "00:00";

         DateTime temp = new DateTime(sourceTime.Ticks);
         return temp.ToString(ConstFormat.ShortTime, CultureInfo.CurrentCulture);
      }
      #endregion

      #region public properties
      /// <summary>
      /// 用来选择医嘱内容类别的字典类实例
      /// </summary>
      public SqlWordbook CatalogWordbook
      {
         get
         {
            if (_catalogWordbook == null)
            {
               Dictionary<string, int> columnWidthes = new Dictionary<string, int>();
               columnWidthes.Add("Name", 150);
               _catalogWordbook =
                  new SqlWordbook(ConstSchemaNames.ContentCatalogTableName, m_CoreLogic.OrderContentCatalog
                  , ConstSchemaNames.ContentCatalogColId, ConstSchemaNames.ContentCatalogColName, columnWidthes, true);
            }
            _catalogWordbook.ExtraCondition = m_CoreLogic.GetOrderContentCatalogRowFilter(IsTempOrder);

            return _catalogWordbook;
         }
      }
      private SqlWordbook _catalogWordbook;

      /// <summary>
      /// 用来选择频次的字典类实例
      /// </summary>
      public OrderFrequencyBook FrequencyWordbook
      {
         get
         {
            if (_frequencyWordbook == null)
               _frequencyWordbook = new OrderFrequencyBook();

            if ((IsTempOrder) && (!m_CoreLogic.HasOutHospitalOrder)) // 如果是出院带药，则使用长期医嘱的频次
               _frequencyWordbook.ExtraCondition = String.Format(CultureInfo.CurrentCulture
                  , ConstSqlSentences.FormatFrequencyFilter
                  , OrderManagerKind.Normal, OrderManagerKind.ForTemp);
            else
               _frequencyWordbook.ExtraCondition = String.Format(CultureInfo.CurrentCulture
                  , ConstSqlSentences.FormatFrequencyFilter
                  , OrderManagerKind.Normal, OrderManagerKind.ForLong);
            return _frequencyWordbook;
         }
      }
      private OrderFrequencyBook _frequencyWordbook;

      /// <summary>
      /// 当前关联的医嘱对象表实例
      /// </summary>
      public OrderTable CurrentOrderTable
      {
         get { return m_CoreLogic.GetCurrentOrderTable(IsTempOrder); }
      }

      /// <summary>
      /// 标记当前处理的是临时还是长期医嘱
      /// </summary>
      public bool IsTempOrder
      {
         get { return _isTempOrder; }
         set
         {
            OrderState state = OrderStateFilter; // 切换长期临时时保持状态过滤不变
            _isTempOrder = value;

            OrderStateFilter = state;
            //ResetAllowNew();

            //FireContentBaseDataChanged(this, new EventArgs());
            FireAfterSwitchOrderTable(new EventArgs());
         }
      }
      private bool _isTempOrder;

      /// <summary>
      /// 当前查看的医嘱表的医嘱状态:全部、新增、有效
      /// </summary>
      public OrderState OrderStateFilter
      {
         get 
         {
            if (CurrentView.State == OrderState.Audited && IsTempOrder)
               return OrderState.Executed;
            else
               return CurrentView.State; 
         }
         set
         {
            if ((value == OrderState.Executed) && IsTempOrder)
               CurrentView.State = OrderState.Audited;
            else
               CurrentView.State = value;
            ResetAllowNew();
         }
      }

      /// <summary>
      /// 当前编辑的医嘱表对应的Grid设置信息
      /// </summary>
      public TypeGridBand[] CurrentBandSettings
      {
         get
         {
            if (IsTempOrder)
               return CustomDrawOperation.GridSetting.TempOrderSetting;
            else
               return CustomDrawOperation.GridSetting.LongOrderSetting;
         }
      }

      /// <summary>
      /// 当天医嘱最早允许开始时间(精确到分钟)
      /// </summary>
      public DateTime MinStartDateTime
      {
         get { return m_CoreLogic.MinStartDateTime; }
      }

      /// <summary>
      /// 当天医嘱默认开始时间（精确到分钟）
      /// </summary>
      public DateTime DefaultStartDateTime
      {
         get { return m_CoreLogic.GetDefaultStartDateTime(IsTempOrder); }
      }

      /// <summary>
      /// 默认的出院时间
      /// </summary>
      public DateTime DefaultLeaveHospitalTime
      {
         get { return m_CoreLogic.DefaultLeaveHospitalTime; }
      }

      /// <summary>
      /// 默认的停止医嘱时间
      /// </summary>
      public DateTime DefaultCeaseOrderTime
      {
         get
         {
            if (DateTime.Now.Hour > 12)
               return DateTime.Today.AddDays(1) + new TimeSpan(8, 0, 0);
            else
               return DateTime.Today + new TimeSpan(16, 0, 0);
         }
      }

      /// <summary>
      /// 标记当前医嘱表数据是否已被修改
      /// </summary>
      public bool HadChanged
      {
         get { return m_CoreLogic.HadChanged; }
      }

      /// <summary>
      /// 标记是否还有未发送的数据
      /// </summary>
      public bool HasNotSendData
      {
         get { return m_CoreLogic.HasNotSendData; }
      }

      /// <summary>
      /// 当前可用的医嘱分类
      /// </summary>
      public List<string> OrderCatalogs
      {
         get { return _orderCatalogs; }
      }
      private List<string> _orderCatalogs;

      /// <summary>
      /// 当前医嘱类型下可用的状态过滤
      /// </summary>
      public List<ImageComboBoxItem> StatusFilters
      {
         get { return _statusFilters; }
      }
      private List<ImageComboBoxItem> _statusFilters;

      /// <summary>
      /// 当前医嘱表的视图
      /// </summary>
      public OrderTableView CurrentView
      {
         get { return CurrentOrderTable.DefaultView; }
      }

      /// <summary>
      /// 当前医嘱表是否允许编辑
      /// </summary>
      public bool AllowEdit
      {
         get { return CurrentView.AllowEdit; }
      }

      /// <summary>
      /// 当前医嘱表是否允许添加新医嘱
      /// </summary>
      public bool AllowAddNew
      {
         get { return ((m_CallModel != EditorCallModel.Query) && (_allowAddNew || (m_CallModel == EditorCallModel.EditSuite))); }
      }
      private bool _allowAddNew;

      /// <summary>
      /// 存放皮试结果的表
      /// </summary>
      public DataTable SkinTestResultTable
      {
         get
         {
            if (_skinTestResultTable == null)
               GetSkinTestResultData();

            return _skinTestResultTable;
         }
      }
      private DataTable _skinTestResultTable;

      /// <summary>
      /// 统一的消息输出框
      /// </summary>
      public ICustomMessageBox CustomMessageBox
      {
         get { return _customMessageBox; }
      }
      private ICustomMessageBox _customMessageBox;

      /// <summary>
      /// 当前处理的病人
      /// </summary>
      public Inpatient CurrentPatient
      {
         get { return m_CoreLogic.CurrentPatient; }
         set
         {
            FireProcessStarting(ConstMessages.HintReadOrderData);
            m_CoreLogic.CurrentPatient = value;
            _skinTestResultTable = null;
            DoAfterPatientChanged();
         }
      }

      public SuiteOrderHandle SuiteHelper
      {
         get { return m_CoreLogic.SuiteHelper; }
      }

      /// <summary>
      /// 当前科室代码
      /// </summary>
      public string DeptCode
      {
         get { return _deptCode; }
      }
      private string _deptCode;
      /// <summary>
      /// 当前病区代码
      /// </summary>
      public string WardCode
      {
         get { return _wardCode; }
      }
      private string _wardCode;

      /// <summary>
      /// 当前病人的医嘱中是否包含有效的“出院医嘱”（没有被取消的）
      /// </summary>
      public bool HasOutHospitalOrder
      {
         get { return m_CoreLogic.HasOutHospitalOrder; }
      }
      #endregion

      #region private variables
      private CoreBusinessLogic m_CoreLogic;
      /// <summary>
      /// 保存项目选择字典类的Hash表，主键是医嘱内容类别编号
      /// </summary>
      private Dictionary<OrderContentKind, BaseWordbook> m_ItemWordbook;
      /// <summary>
      /// 当前医生工号
      /// </summary>
      private string m_DoctorCode;
      /// <summary>
      /// 本机的网卡地址
      /// </summary>
      private string m_MacAddress;
      private RecipeRuleChecker m_RepRuleChecker;
      private EditorCallModel m_CallModel;
      #endregion

      #region ctors
      /// <summary>
      /// 创建逻辑处理实例
      /// </summary>
      /// <param name="app">框架对象，用来初始化变量</param>
      /// <param name="invalidRowHandle">表示非正式行的行号</param>
      /// <param name="newItemRowHandle">表示处于新增状态的行号</param>
      /// <param name="callModel">调用模式(编辑医嘱、编辑成套)</param>
      public UILogic(IEmrHost app, int invalidRowHandle, int newItemRowHandle, EditorCallModel callModel)
      {
         //if (callModel != EditorCallModel.EditSuite)
         //   m_CoreLogic = new CoreBusinessLogic(app.SqlHelper);
         //else
         m_CoreLogic = new CoreBusinessLogic(app, callModel);

         m_CoreLogic.OutHospitalOrderChanged += new EventHandler(DoAfterOutHospitalOrderChanged);
         m_CoreLogic.PatientOrderDataChanged += new EventHandler(DoAfterPatientOrderDataChanged);

         InvalidRowHandle = invalidRowHandle;
         NewItemRowHandle = newItemRowHandle;

         m_DoctorCode = app.User.DoctorId;
         _deptCode = app.User.CurrentDeptId;
         _wardCode = app.User.CurrentWardId;
         m_MacAddress = app.MacAddress;
         _customMessageBox = app.CustomMessageBox;
         m_CallModel = callModel;

         _orderCatalogs = new List<string>();
         _orderCatalogs.Add(ConstNames.LongOrder);
         _orderCatalogs.Add(ConstNames.TempOrder);

         _statusFilters = new List<ImageComboBoxItem>();
         _statusFilters.Add(new ImageComboBoxItem(ConstNames.OrderStateAll, OrderState.All, -1));
         _statusFilters.Add(new ImageComboBoxItem(ConstNames.OrderStateNew, OrderState.New, -1));
         _statusFilters.Add(new ImageComboBoxItem(ConstNames.OrderStateAudited, OrderState.Audited, 11));
         _statusFilters.Add(new ImageComboBoxItem(ConstNames.OrderStateExecuted, OrderState.Executed, 13));
         _statusFilters.Add(new ImageComboBoxItem(ConstNames.OrderStateCeased, OrderState.Ceased, 7));
         _statusFilters.Add(new ImageComboBoxItem(ConstNames.OrderStateCancelled, OrderState.Cancellation, 6));

         CreateWordbookDictionary();

         if (CoreBusinessLogic.BusinessLogic.EnableOrderRules)
         {
            try
            {
               object techTitle = app.SqlHelper.ExecuteScalar(String.Format(ConstSqlSentences.FormatSelectEmployee, m_DoctorCode));
               //if (techTitle == null)
               //   m_RepRuleChecker = new RecipeRuleChecker(ExchangeInfoHelper.ExchangeInfoServer, _deptCode, "");
               //else
               //   m_RepRuleChecker = new RecipeRuleChecker(ExchangeInfoHelper.ExchangeInfoServer, _deptCode, techTitle.ToString());
               if (techTitle == null)
                  m_RepRuleChecker = new RecipeRuleChecker(app.SqlHelper, _deptCode, "");
               else
                  m_RepRuleChecker = new RecipeRuleChecker(app.SqlHelper, _deptCode, techTitle.ToString());
            }
            catch
            {
               CoreBusinessLogic.BusinessLogic.EnableOrderRules = false;
               //throw new CallRemotingException(ConstMessages.MsgCanntGetRecipeRuleData);
            }
         }
      }
      #endregion

      #region custom event handler
      /// <summary>
      /// 切换当前处理的医嘱表事件(切换已完成)
      /// </summary>
      public event EventHandler AfterSwitchOrderTable
      {
         add
         {
            onAfterSwitchOrderTable = (EventHandler)Delegate.Combine(onAfterSwitchOrderTable, value);
         }
         remove
         {
            onAfterSwitchOrderTable = (EventHandler)Delegate.Remove(onAfterSwitchOrderTable, value);
         }
      }
      private EventHandler onAfterSwitchOrderTable;

      /// <summary>
      /// 当前处理的数据源发生改变后触发
      /// </summary>
      /// <param name="e"></param>
      /// <param name="actualIndex"></param>
      protected void FireAfterSwitchOrderTable(EventArgs e)
      {
         if (onAfterSwitchOrderTable != null)
            onAfterSwitchOrderTable(this, e);
      }

      /// <summary>
      /// 当前编辑的医嘱表其新增标志改变事件。
      /// 编辑器可以在此事件中处理是否显示新行、是否需要重设医嘱内容编辑器的基础数据
      /// </summary>
      public event EventHandler AllowNewChanged
      {
         add
         {
            onAllowNewChanged = (EventHandler)Delegate.Combine(onAllowNewChanged, value);
         }
         remove
         {
            onAllowNewChanged = (EventHandler)Delegate.Remove(onAllowNewChanged, value);
         }
      }
      private EventHandler onAllowNewChanged;

      protected void FireAllowNewChanged(object sender, EventArgs e)
      {
         if (onAllowNewChanged != null)
            onAllowNewChanged(sender, e);
      }

      /// <summary>
      /// 编辑医嘱内容所需的基础数据被改变事件。
      /// 编辑器器可以在此事件中重设医嘱内容编辑器的基础数据
      /// </summary>
      public event EventHandler ContentBaseDataChanged
      {
         add
         {
            onContentBaseDataChanged = (EventHandler)Delegate.Combine(onContentBaseDataChanged, value);
         }
         remove
         {
            onContentBaseDataChanged = (EventHandler)Delegate.Remove(onContentBaseDataChanged, value);
         }
      }
      private EventHandler onContentBaseDataChanged;

      private void FireContentBaseDataChanged(object sender, EventArgs e)
      {
         if (onContentBaseDataChanged != null)
            onContentBaseDataChanged(sender, e);

         ResetAllowNew();
      }

      /// <summary>
      /// 进行每步逻辑处理前的事件，可以显示当前执行的步骤信息
      /// </summary>
      public event EventHandler<ProcessHintArgs> ProcessStarting
      {
         add { onProcessStarting = (EventHandler<ProcessHintArgs>)Delegate.Combine(onProcessStarting, value); }
         remove { onProcessStarting = (EventHandler<ProcessHintArgs>)Delegate.Combine(onProcessStarting, value); }
      }
      private EventHandler<ProcessHintArgs> onProcessStarting;

      private void FireProcessStarting(string processHint)
      {
         if (onProcessStarting != null)
         {
            if (processHint == null)
               processHint = "";
            onProcessStarting(this, new ProcessHintArgs(processHint));
         }
      }
      #endregion

      #region event handle
      private void DoAfterOutHospitalOrderChanged(object sender, EventArgs e)
      {
         FireContentBaseDataChanged(sender, e);
      }

      protected void DoAfterPatientOrderDataChanged(object sender, EventArgs e)
      {
         ResetAllowNew();
      }
      #endregion

      #region private methods
      /// <summary>
      /// 创建项目选择字典类的Hash表实例
      /// </summary>
      private void CreateWordbookDictionary()
      {
         m_ItemWordbook = new Dictionary<OrderContentKind, BaseWordbook>();
         BaseWordbook wordbook;

         // 药品
         wordbook = new DruggeryBook();
         m_ItemWordbook.Add(OrderContentKind.Druggery, wordbook);

         // 收费项目、常规项目等都使用同一种字典类，只是要通过参数来显示特定内容
         // 普通项目
         wordbook = new ChargeItemBook();
         wordbook.ExtraCondition = String.Format(CultureInfo.CurrentCulture
                  , ConstSqlSentences.FormatChargeItemFilterNormal
            , ItemKind.Cure
            , ItemKind.Transfusion
            , ItemKind.Examination
            , ItemKind.Assay
            , ItemKind.Infusion
            , ItemKind.Meterial
            , ItemKind.Diagnosis
            , ItemKind.Other
            , ItemKind.Sugar);
         m_ItemWordbook.Add(OrderContentKind.ChargeItem, wordbook);

         // 常规项目
         wordbook = new ChargeItemBook();
         wordbook.ExtraCondition = ConstSqlSentences.ChargeItemFilterGeneral;
         m_ItemWordbook.Add(OrderContentKind.GeneralItem, wordbook);

         // 临床项目(未实现！！！)

         // 出院带药
         wordbook = new DruggeryBook();
         m_ItemWordbook.Add(OrderContentKind.OutDruggery, wordbook);

         // 手术
         wordbook = new ChargeItemBook();
         wordbook.ExtraCondition = String.Format(CultureInfo.CurrentCulture
                  , ConstSqlSentences.FormatChargeItemFilterOperation, ItemKind.Operation);
         m_ItemWordbook.Add(OrderContentKind.Operation, wordbook);

         // 文字医嘱
         wordbook = new ChargeItemBook();
         wordbook.ExtraCondition = ConstSqlSentences.ChargeItemFilterText;
         m_ItemWordbook.Add(OrderContentKind.TextNormal, wordbook);
         // 转科医嘱、术后医嘱、出院医嘱暂时不需要对应项目！！！         
      }

      private void GetSkinTestResultData()
      {
         try
         {
            _skinTestResultTable = m_CoreLogic.GetSkinTestResultData();
         }
         catch
         {
            _skinTestResultTable = null;
         }
         if (_skinTestResultTable == null)
         {
            _skinTestResultTable = new DataTable();
            _skinTestResultTable.Locale = CultureInfo.CurrentCulture;
            DataColumn col;
            col = new DataColumn(ConstSchemaNames.SkinTestColDruggeryName, typeof(string));
            _skinTestResultTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.SkinTestColBeginDate, typeof(string));
            _skinTestResultTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.SkinTestColEndDate, typeof(string));
            _skinTestResultTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.SkinTestColFlag, typeof(string));
            _skinTestResultTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.SkinTestColSpecSerialNo, typeof(string));
            _skinTestResultTable.Columns.Add(col);
         }
      }

      private void DoAfterPatientChanged()
      {
         if ((CoreBusinessLogic.BusinessLogic.EnableOrderRules) && (CurrentPatient != null) && (m_RepRuleChecker != null))
         {
            m_RepRuleChecker.MedCareCode = CurrentPatient.Medicare.Code;
            m_RepRuleChecker.WarrantCode = CurrentPatient.Medicare.VoucherCode;// 凭证类型
            if ((CurrentPatient.InfoOfAdmission.AdmitInfo.Diagnosis != null) && (CurrentPatient.InfoOfAdmission.AdmitInfo.Diagnosis.Code != null))
               m_RepRuleChecker.DiagnoseCode = CurrentPatient.InfoOfAdmission.AdmitInfo.Diagnosis.Code; // 入院诊断
            else
               m_RepRuleChecker.DiagnoseCode = "";
         }
         //// 触发数据源改变事件
         //FireAfterSwitchOrderTable(new EventArgs());
      }

      /// <summary>
      /// 在指定的医嘱对象表中通过行号获取对应的医嘱对象
      /// </summary>
      /// <param name="currentView">在视图中查找的医嘱对象</param>
      /// <param name="rowHandle">指定的行号</param>
      /// <returns>对应的医嘱对象</returns>
      private Order GetOrderByRowHandle(OrderTableView currentView, int rowHandle)
      {
         if (!CheckRowHandleIsValidate(rowHandle, true))
            return null;

         if (rowHandle == NewItemRowHandle)
         {
            if (currentView.Count > 0)
               rowHandle = currentView.Count - 1;
            else
               return null;
         }

         if (rowHandle > (currentView.Count - 1))
            return null;

         Order mapOrder = currentView[rowHandle].OrderCache;
         if (mapOrder == null)
            throw new IndexOutOfRangeException(ConstMessages.ExceptionOrderIndexNotFind);
         else
            return mapOrder;
      }

      /// <summary>
      /// 检查选中的行集是否有效。无效时产生异常
      /// </summary>
      /// <param name="selectedHandles"></param>
      private void CheckSelectedRange(int[] selectedHandles)
      {
         if ((selectedHandles == null) || (selectedHandles.Length == 0))
            throw new ArgumentException(String.Format(ConstMessages.ExceptionFormatNoValue, "选择范围"));

         int startHandle = selectedHandles[0];
         int endHandle = selectedHandles[selectedHandles.Length - 1];

         // 应该在当前显示的记录范围内
         if ((startHandle < 0)
            || (endHandle > CurrentView.Count))
            throw new DataCheckException(ConstMessages.CheckOrderSelectionRange, "选中行");
      }

      private Order[] GetSlectedOrders(int[] selectedHandles)
      {
         CheckSelectedRange(selectedHandles);

         Collection<Order> selectedOrders = new Collection<Order>();
         foreach (int rowHandle in selectedHandles)
         {
            if (CheckRowHandleIsValidate(rowHandle))
               selectedOrders.Add(GetOrderByRowHandle(CurrentView, rowHandle));
         }

         Order[] result = new Order[selectedOrders.Count];
         selectedOrders.CopyTo(result, 0);
         return result;
      }

      /// <summary>
      /// 计算对选中的行集可以执行的医嘱操作
      /// </summary>
      /// <param name="rowHandles"></param>
      /// <returns></returns>
      private EditProcessFlag CalcEditProcessFlag(int[] selectedHandles)
      {
         Order[] selectedOrders;
         try
         {
            selectedOrders = GetSlectedOrders(selectedHandles);
         }
         catch
         {
            return 0;
         }

         EditProcessFlag result = m_CoreLogic.CalcEditProcessFlag(CurrentOrderTable, selectedOrders);

         // 保存--做过修改，则需要保存数据
         if (HadChanged)
            result |= EditProcessFlag.Save;

         return result;
      }

      /// <summary>
      /// 检查选中的记录是否可以向下移动
      /// </summary>
      /// <param name="selectedHandles">选中的记录行号</param>
      /// <returns>不可以时返回原因</returns>
      private string CheckCanBeMoveDown(int[] selectedHandles)
      {
         CheckSelectedRange(selectedHandles);

         EditProcessFlag flag = CalcEditProcessFlag(selectedHandles);
         if ((flag & EditProcessFlag.MoveDown) > 0)
            return "";

         return CoreBusinessLogic.FormatProcessErrorMessage(ConstNames.OpMoveDown, ConstMessages.ConditionMoveDown);
      }

      /// <summary>
      /// 检查选中的记录是否可以向上移动
      /// </summary>
      /// <param name="selectedHandles">选中的记录行号</param>
      /// <returns>不可以时返回原因</returns>
      private string CheckCanBeMoveUp(int[] selectedHandles)
      {
         CheckSelectedRange(selectedHandles);

         EditProcessFlag flag = CalcEditProcessFlag(selectedHandles);
         if ((flag & EditProcessFlag.MoveUp) > 0)
            return "";

         return CoreBusinessLogic.FormatProcessErrorMessage(ConstNames.OpMoveUp, ConstMessages.ConditionMoveUp);
      }

      /// <summary>
      /// 由界面更新了医嘱的开始时间时，根据规则同步其它医嘱的开始时间
      /// </summary>
      /// <param name="rowHandle">当前更新的医嘱序号</param>
      /// <param name="startDateTime">更新后的医嘱时间</param>
      private void SynchStartDateAfterOrderValueChanged(int rowHandle, DateTime startDateTime)
      {
         Order aimOrder = GetOrderByRowHandle(CurrentView, rowHandle);
         Order temp;
         // 如果医嘱已分组，则同步同组医嘱的时间
         // 如果开始时间大于后一条医嘱的开始时间，则同时改变后续医嘱的时间，直到所有记录的开始时间都符合规则
         for (int index = CurrentOrderTable.Orders.Count - 1; index >= 0; index--)
         {
            temp = CurrentOrderTable.Orders[index];
            // 走到该条一组所在分组的前面时直接退出
            // (因为在调用此方法前已经有逻辑检查，保证本条的开始时间不会小于前面医嘱的时间)
            if (temp.GroupSerialNo < aimOrder.GroupSerialNo)
               break;
            if (temp.StartDateTime < startDateTime)
               temp.StartDateTime = startDateTime;
         }
      }

      /// <summary>
      /// 如果指定医嘱的用法、频次与同组的其它医嘱不一致，则根据指定医嘱的信息进行同步
      /// </summary>
      /// <param name="aimOrder"></param>
      private void SynchPublicInfoOfGroupedOrders(Order aimOrder)
      {
         Collection<Order> orderList = CurrentOrderTable.GetOtherOrdersOfSameGroup(aimOrder);
         CurrentView.BeginInit();
         foreach (Order temp in orderList)
         {
            //if (temp.Content.ItemUsage.Equals(aimOrder.Content.ItemUsage)
            //   && (temp.Content.ItemFrequency.Equals(aimOrder.Content.ItemFrequency)))
            //   break; // 同组的医嘱只要有一条与指定医嘱用法、频次相同，则说明其它医嘱也相同
            temp.Content.ItemFrequency = aimOrder.Content.ItemFrequency.Clone();
            temp.Content.ItemUsage = aimOrder.Content.ItemUsage.Clone();
         }
         CurrentView.EndInit();
      }

      private bool JudgeCellOfOriginalRowCanEdit(string columnName, int rowHandle)
      {
         // 对老数据进行编辑
         Order temp = GetOrderByRowHandle(CurrentView, rowHandle);

         // 长期医嘱中的“转科医嘱”不能被编辑
         if (!IsTempOrder && (temp.Content != null)
            && (temp.Content.OrderKind == OrderContentKind.TextShiftDept))
            return false;

         // 临时医嘱中的“申请单医嘱”不能被编辑
         if (IsTempOrder)
         {
            if (((TempOrder)temp).ApplySerialNo != 0)
               return false;
         }

         // 当前医嘱只读
         if (temp.ReadOnly)
            return false;

         // 现在只放开新医嘱的开始日期和时间的修改
         switch (columnName)
         {
            case OrderView.UNStartDate:
            case OrderView.UNStartTime:
               return (temp.State == OrderState.New);
            default:
               return false;
         }

         //// 1、开始日期、开始时间、医嘱内容
         ////       只有在当前医嘱状态是新增时可以编辑
         //// 2、停止日期、停止时间
         ////       只有在当前医嘱是长期医嘱
         ////       且可以停止（药品、项目等医嘱）
         ////       非取消状态
         ////       停止后还未被审核可以编辑
         //// 3、其它列都不可以直接编辑
         //switch (columnName)
         //{
         //   case OrderView.UNStartDate:
         //   case OrderView.UNStartTime:
         //   //case OrderView.UNContent:
         //   //   if (temp.State == OrderState.New)
         //   //      return true;
         //   //   else
         //   //      return false;
         //   //case OrderView.UNCeaseDate:
         //   //case OrderView.UNCeaseTime:
         //   //   LongOrder longOrder = temp as LongOrder;
         //   //   if ((longOrder != null)
         //   //      && (longOrder.Content.CanCeased)
         //   //      && ((longOrder.State == OrderState.New) 
         //   //         || (longOrder.State == OrderState.Executed)))
         //   //      return true;
         //   //   else
         //   //      return false;
         //   default:
         //      return false;
         //}
      }

      private static bool JudgeCellOfNewRowCanEdit(string columnName)
      {
         switch (columnName)
         {
            case OrderView.UNStartDate:
            case OrderView.UNStartTime:
               //case OrderView.UNContent:
               //case OrderView.UNCeaseDate:
               //case OrderView.UNCeaseTime:
               return true;
            default:
               return false;
         }
      }

      private void ResetAllowNew()
      {
         bool allowNew;
         if (IsTempOrder)
            allowNew = m_CoreLogic.AllowAddTemp;
         else
            allowNew = m_CoreLogic.AllowAddLong;
         if (_allowAddNew != allowNew)
         {
            _allowAddNew = allowNew;
            FireAllowNewChanged(this, new EventArgs());
         }
      }

      private int GetCountOfBrothers(int startPosition, bool downwards)
      {
         int result = 0;
         int borderIndex;
         int moveStep;
         if (downwards) // 向下查找
         {
            borderIndex = CurrentView.Count - 1;
            moveStep = 1;
         }
         else  // 向上查找
         {
            borderIndex = 0;
            moveStep = -1;
         }
         bool needCheckLinkToApply = (CurrentView.Table.IsTempOrder
            && (CurrentView[startPosition].ApplySerialNo != 0));
         int nextIndex = startPosition;
         // 分在一组的或关联同一个申请单序号的医嘱都算一组
         while (nextIndex != borderIndex)
         {
            nextIndex = nextIndex + moveStep;
            if (CurrentView[nextIndex].GroupSerialNo != CurrentView[startPosition].GroupSerialNo)
            {
               if (needCheckLinkToApply)
               {
                  if (CurrentView[nextIndex].ApplySerialNo == CurrentView[startPosition].ApplySerialNo)
                     result++;
                  else
                     return result;
               }
               else
                  return result;
            }
            else
            {
               needCheckLinkToApply = false; // 医嘱不可能即分组又关联申请单
               result++;
            }
         }
         return result;
      }

      private void DoSaveOrderData(bool isTempOrder, bool skipWarnning)
      {
         OrderTable currentTable = GetCurrentOrderTable(isTempOrder);
         if (currentTable.HadChanged)
         {
            Order[] changedOrders = currentTable.GetNewAndChangedOrders();
            if ((changedOrders != null) && (changedOrders.Length > 0))
            {
               FireProcessStarting(GetProcessHint(ConstNames.OpCheck, isTempOrder));

               try
               {
                  m_CoreLogic.CheckOrderData(currentTable, changedOrders, true, skipWarnning);
               }
               catch (DataCheckException err)
               {
                  if ((err.WarnningLevel == 1) || (!skipWarnning))
                     throw err;
               }

               FireProcessStarting(GetProcessHint(ConstNames.OpSave, isTempOrder));
               currentTable.DefaultView.BeginInit();
               m_CoreLogic.SaveChangedOrderDataInEditor(currentTable, changedOrders, m_DoctorCode, m_MacAddress);
               currentTable.DefaultView.EndInit();
            }
         }
      }

      private string GetProcessHint(string opName, bool isTempOrder)
      {
         if (isTempOrder)
            return String.Format(CultureInfo.CurrentCulture, "{0} {1} 数据……", opName, ConstNames.TempOrder);
         else
            return String.Format(CultureInfo.CurrentCulture, "{0} {1} 数据……", opName, ConstNames.LongOrder);
      }
      #endregion

      #region public methods
      /// <summary>
      /// 检查指定的行号是否是有效行
      /// </summary>
      /// <param name="rowHandle"></param>
      /// <returns>true：是  false：否</returns>
      public bool CheckRowHandleIsValidate(int rowHandle)
      {
         return CheckRowHandleIsValidate(rowHandle, false);
      }

      /// <summary>
      /// 检查指定的行号是否是有效行
      /// </summary>
      /// <param name="rowHandle"></param>
      /// <param name="includeNew">是否可以包含新行</param>
      /// <returns></returns>
      public bool CheckRowHandleIsValidate(int rowHandle, bool includeNew)
      {
         if (rowHandle == InvalidRowHandle)
            return false;
         else if (rowHandle == NewItemRowHandle)
            return includeNew;
         else if (rowHandle < 0)
            return false;
         else
            return true;
      }

      /// <summary>
      /// 判断由行号和列名指定的单元格是否可进入编辑状态
      /// </summary>
      /// <param name="columnName">指定的列名</param>
      /// <param name="rowHandle">指定的行号</param>
      /// <returns></returns>
      public bool JudgeCellCanEdit(string columnName, int rowHandle)
      {
         // 医嘱表只读、非正式行号
         if ((m_CoreLogic.CurrentPatient == null)
            || (!CurrentView.AllowEdit) || (rowHandle == InvalidRowHandle))
            return false;

         // 新行
         if (rowHandle == NewItemRowHandle)
            return JudgeCellOfNewRowCanEdit(columnName);
         else
            return JudgeCellOfOriginalRowCanEdit(columnName, rowHandle);
      }

      /// <summary>
      /// 根据医嘱内容的类别编号，返回对应的字典类实例
      /// </summary>
      /// <param name="catalogNo"></param>
      /// <returns>无对应字典类实例则返回NULL</returns>
      public BaseWordbook GetItemWordbook(OrderContentKind orderCatalog)
      {
         if (m_ItemWordbook.ContainsKey(orderCatalog))
         {
            BaseWordbook wordbook = m_ItemWordbook[orderCatalog];
            // 如果不允许在长期医嘱中输入草药，则需要过滤药品字典
            if (orderCatalog == OrderContentKind.Druggery)
            {
               if ((!IsTempOrder) && (!CoreBusinessLogic.BusinessLogic.AllowLongHerbOrder))
                  wordbook.ExtraCondition = String.Format(ConstSqlSentences.FormatDruggeryKindFilter, (int)ItemKind.HerbalMedicine);
               else
                  wordbook.ExtraCondition = "";
            }

            return wordbook;
         }
         else
            return null;
      }

      /// <summary>
      /// 创建默认的医嘱内容实例(以前一条医嘱为准，编辑新增医嘱的内容时调用)
      /// </summary>
      /// <param name="currentRowHandle">当前Grid中Focused的行号</param>
      /// <returns></returns>
      public OrderContent CreateDefaultContent(int currentRowHandle)
      {
         int preRowHandle;
         if (CheckRowHandleIsValidate(currentRowHandle))
            preRowHandle = currentRowHandle - 1;
         else if (currentRowHandle == NewItemRowHandle)//(CurrentView.Count > 0)
            preRowHandle = CurrentView.Count - 2; // 此时新行是List的最后一条记录
         else
            preRowHandle = CurrentView.Count - 1; // 非正常行

         Order preOrder = GetOrderByRowHandle(CurrentView, preRowHandle);
         OrderContent defaultContent;

         if ((preOrder != null) && (preOrder.State == OrderState.New))// 新增医嘱的类别和上一条医嘱保持一致
            defaultContent = OrderContentFactory.CreateOrderContent(
               preOrder.Content.OrderKind, null);
         else // 默认为药品医嘱
            defaultContent = OrderContentFactory.CreateOrderContent(
               OrderContentKind.Druggery, null);
         return defaultContent;
      }

      /// <summary>
      /// Grid中插入新行后，初始化对应的新增医嘱（新增状态，还未初始化）的属性
      /// </summary>
      /// <param name="newOrder">新增医嘱对应的实例</param>
      /// <param name="tempOrder">保存初始化数据的医嘱实例</param>
      public void InitNewOrderValue(Order newOrder, Order tempOrder)
      {
         if (newOrder == null)
            throw new ArgumentNullException(ConstMessages.ExceptionNullOrderObject);

         newOrder.BeginInit();

         newOrder.PatientId = m_CoreLogic.CurrentPatientNo;

         // 新增医嘱都默认为不分组，另外分组序号在编辑时没有作用，保存到数据库时会更新
         // 完成该行的数据编辑后再根据是否自动分组进行处理！！！

         newOrder.OriginalWard = new Eop.Ward(_wardCode);
         newOrder.OriginalDepartment = new Eop.Department(_deptCode);

         if (newOrder.CreateInfo == null)
            newOrder.CreateInfo = new OrderOperateInfo(m_DoctorCode, DateTime.Now);
         else
            newOrder.CreateInfo.SetPropertyValue(m_DoctorCode, DateTime.Now);

         // 如果开始时间未赋值，则使用默认时间
         if (tempOrder.StartDateTime.Date == DateTime.MinValue.Date)
            newOrder.StartDateTime = DefaultStartDateTime;
         else if (tempOrder.StartDateTime.TimeOfDay == TimeSpan.Zero)
            newOrder.StartDateTime = tempOrder.StartDateTime.Date + DefaultStartDateTime.TimeOfDay;
         else
            newOrder.StartDateTime = tempOrder.StartDateTime;

         if (tempOrder.Content != null)
            newOrder.Content = tempOrder.Content;

         if ((newOrder as LongOrder) != null)
         {
            OrderOperateInfo ceaseInfo = (tempOrder as LongOrder).CeaseInfo;
            if (ceaseInfo != null)
               (newOrder as LongOrder).CeaseOrder(ceaseInfo.Executor.Code, ceaseInfo.ExecuteTime, OrderCeaseReason.Natural);
         }

         newOrder.EndInit();
      }

      /// <summary>
      /// 在将编辑框中的数据赋值给医嘱对象时验证医嘱开始时间和医嘱内容是否正确
      /// </summary>
      /// <param name="targetIndex">当前处理的医嘱行号(相对于当前视图的行号)(如果是新增或插入医嘱，则行号可能不在List的索引内)</param>
      /// <param name="orderTemp">保存带校验值的医嘱临时变量</param>
      /// <param name="updateFalg">标记哪些数据已被更新</param>
      public void CheckOrderValueBeforeSet(int targetIndex, Order orderTemp, UpdateContentFlag updateFalg)
      {
         int trueIndex; // 视图中的行号转成Table中的行号
         if (targetIndex < 0)
            trueIndex = targetIndex;
         else
         {
            Order temp = GetOrderByRowHandle(CurrentView, targetIndex);
            if (temp != null)
               trueIndex = CurrentOrderTable.Orders.IndexOf(temp.SerialNo);
            else
               trueIndex = -1;
         }
         m_CoreLogic.CheckOrderValueBeforeSet(CurrentOrderTable
            , orderTemp
            , trueIndex
            , updateFalg);
      }

      /// <summary>
      /// 设置医嘱对象的开始时间和医嘱内容
      /// </summary>
      /// <param name="rowHandle">需要处理的医嘱行号</param>
      /// <param name="orderTemp">保存新值的医嘱临时变量</param>
      /// <param name="updateFlag">标记哪些数据已被更新</param>
      public void SetNewOrderElementValue(int rowHandle, Order orderTemp, UpdateContentFlag updateFlag)
      {
         if (orderTemp == null)
            throw new ArgumentNullException(String.Format(ConstMessages.ExceptionFormatNullObject, "医嘱临时变量"));
         // 设置前是否需要再做一次数据检查？？？
         Order aimOrder = GetOrderByRowHandle(CurrentView, rowHandle);
         if (aimOrder == null)
            throw new IndexOutOfRangeException(ConstMessages.ExceptionOrderIndexNotFind);

         if ((updateFlag & UpdateContentFlag.StartDate) > 0)
         {
            aimOrder.StartDateTime = orderTemp.StartDateTime;
            SynchStartDateAfterOrderValueChanged(rowHandle, orderTemp.StartDateTime);
         }
         if ((updateFlag & UpdateContentFlag.Content) > 0)
         {
            aimOrder.Content = PersistentObjectFactory.CloneEopBaseObject(orderTemp.Content) as OrderContent;
            // 如果医嘱已分组，则同步同组医嘱的用法、频次
            if (aimOrder.GroupPosFlag != GroupPositionKind.SingleOrder)
               SynchPublicInfoOfGroupedOrders(aimOrder);
         }
         if ((updateFlag & UpdateContentFlag.CeaseDate) > 0)
         {
            OrderOperateInfo ceaseInfo = (orderTemp as LongOrder).CeaseInfo;
            (aimOrder as LongOrder).CeaseOrder(ceaseInfo.Executor.Code, ceaseInfo.ExecuteTime, OrderCeaseReason.Natural);
         }
         if (updateFlag != 0)
         {
            // 对数据做修改后要更新创建者信息
            aimOrder.CreateInfo.Executor.Code = m_DoctorCode;
         }
      }

      /// <summary>
      /// 向当前医嘱表的指定位置插入医嘱
      /// </summary>
      /// <param name="rowHandle"></param>
      /// <param name="order"></param>
      public void InsertOrder(int rowHandle, Order order)
      {
         // 插入前进行检查
         Order focusedOrder = null;
         if ((rowHandle >= 0) && (rowHandle < CurrentView.Count))
            focusedOrder = CurrentView[rowHandle].OrderCache;

         m_CoreLogic.CheckCanInsertOrder(focusedOrder, IsTempOrder, new Order[] { order });

         CurrentOrderTable.InsertOrderAt(order, CurrentView.GetOriginalIndex(rowHandle));
      }

      /// <summary>
      /// 删除医嘱集合中不正常状态的医嘱。目前是删除开始时间或医嘱内容为空的新医嘱
      /// </summary>
      public void DeleteAbnormalNewOrders()
      {
         Order temp;
         for (int index = CurrentOrderTable.Orders.Count - 1; index >= 0; index--)
         {
            temp = CurrentOrderTable.Orders[index];
            if (temp.State == OrderState.New)
            {
               if ((temp.Content == null) || (temp.Content.ToString().Length == 0))
                  CurrentOrderTable.RemoveOrderAt(index);
            }
         }
      }

      /// <summary>
      /// 删除指定的新医嘱
      /// </summary>
      /// <param name="selectedHandles"></param>
      public void DeleteNewOrder(int[] selectedHandles)
      {
         Order[] selectedOrders = GetSlectedOrders(selectedHandles);

         m_CoreLogic.DeleteNewOrder(selectedOrders, IsTempOrder);
      }

      /// <summary>
      /// 将指定范围的医嘱设为一组
      /// </summary>
      /// <param name="selectedHandles">当前选中的医嘱行号</param>
      public void SetOrderGroup(int[] selectedHandles)
      {
         Order[] selectedOrders = GetSlectedOrders(selectedHandles);

         m_CoreLogic.SetOrderGroup(selectedOrders, IsTempOrder);
      }

      /// <summary>
      /// 自动对新医嘱进行分组
      /// </summary>
      /// <returns>被分组的记录的行号</returns>
      public int[] AutoSetNewOrderGrouped()
      {
         if (AllowAddNew)
         {
            return m_CoreLogic.AutoSetNewOrderGrouped(IsTempOrder);
         }
         return null;
      }

      /// <summary>
      /// 取消分组信息
      /// </summary>
      /// <param name="selectedHandles">当前选中的医嘱行号</param>
      public void CancelOrderGroup(int[] selectedHandles)
      {
         Order[] selectedOrders = GetSlectedOrders(selectedHandles);

         m_CoreLogic.CancelOrderGroup(selectedOrders, IsTempOrder);
      }

      /// <summary>
      /// 审核指定范围内的医嘱
      /// </summary>
      /// <param name="selectedHandles">当前选中的医嘱行号</param>
      public void AuditOrder(int[] selectedHandles)
      {
         Order[] selectedOrders;
         selectedOrders = GetSlectedOrders(selectedHandles);
         if (selectedHandles.Length != selectedHandles.Length)
            throw new ArgumentException(ConstMessages.CheckSelectedRangWithDataRow);

         m_CoreLogic.AuditOrder(selectedOrders, m_DoctorCode, DateTime.Now, IsTempOrder);
      }

      /// <summary>
      /// 根据当前选中记录的情况设置工具栏按钮的状态
      /// </summary>
      /// <param name="rowHandles">选中的记录行号(应与当前视图一致)</param>
      /// <returns>相应按钮的状态集合</returns>
      public EditProcessFlag GetBarItemStatus(int[] rowHandles)
      {
         if ((!AllowEdit) || (rowHandles == null) || (rowHandles.Length == 0))
         {
            if (HadChanged)
               return EditProcessFlag.Save;
            else
               return 0;
         }
         return CalcEditProcessFlag(rowHandles);
      }

      /// <summary>
      /// 根据指定的医嘱类型，获取对应的医嘱内容各编辑框是否可用标志
      /// </summary>
      /// <param name="orderKind">医嘱类型</param>
      /// <returns></returns>
      public static EditorEnableFlag GetContentEditorEnableStatus(OrderContentKind orderKind)
      {
         EditorEnableFlag result = 0;
         // 除三种特殊的文字医嘱外都需要选项目
         if (!((orderKind == OrderContentKind.TextAfterOperation)
            || (orderKind == OrderContentKind.TextShiftDept)
            || (orderKind == OrderContentKind.TextLeaveHospital)))
            result |= EditorEnableFlag.CanChoiceItem;
         // 药品和普通项目需要输入数量，可以设置执行天数(长期或出院带药时有效)
         if ((orderKind == OrderContentKind.Druggery)
            || (orderKind == OrderContentKind.ChargeItem)
            || (orderKind == OrderContentKind.OutDruggery))
         {
            result |= EditorEnableFlag.NeedInputAmount | EditorEnableFlag.CanSetExecuteDays;
         }
         // 只有药品需要输入用法
         if ((orderKind == OrderContentKind.Druggery)
            || (orderKind == OrderContentKind.OutDruggery))
            result |= EditorEnableFlag.CanChoiceUsage;
         // 只有药品和普通项目需要输入频次
         if ((orderKind == OrderContentKind.Druggery)
            || (orderKind == OrderContentKind.ChargeItem)
            || (orderKind == OrderContentKind.OutDruggery))
            result |= EditorEnableFlag.CanChoiceFrequency;
         // 出院带药的附加信息
         if (orderKind == OrderContentKind.OutDruggery)
            result |= EditorEnableFlag.NeedOutDruggeryInfo;
         // 转科医嘱的附加信息
         if (orderKind == OrderContentKind.TextShiftDept)
            result |= EditorEnableFlag.NeedShiftDeptInfo;
         // 手术医嘱和出院医嘱的附加信息
         if (orderKind == OrderContentKind.Operation)
            result |= EditorEnableFlag.NeedOperationInfo;
         if (orderKind == OrderContentKind.TextLeaveHospital)
            result |= EditorEnableFlag.NeedLeaveHospitalTime;
         // 转科、出院、术后暂时不能输入嘱托(因为要用嘱托字段存其它数据)
         if ((orderKind != OrderContentKind.TextLeaveHospital)
            && (orderKind != OrderContentKind.TextShiftDept)
            && (orderKind != OrderContentKind.TextAfterOperation))
            result |= EditorEnableFlag.CanInputEntrust;
         return result;
      }

      /// <summary>
      /// 根据传入的医嘱表分类名和医嘱状态，切换当前编辑的医嘱数据
      /// </summary>
      /// <param name="catalog">医嘱类别</param>
      /// <param name="status">医嘱状态</param>
      public void SwitchOrderTable(bool isTempOrder, OrderState state)
      {
         IsTempOrder = isTempOrder;
         FilterOrderByState(state); // 通过调用此方法来触发事件
         //FireAfterSwitchOrderTable(new EventArgs());
      }

      /// <summary>
      /// 根据传入的医嘱状态过滤当前编辑的医嘱表
      /// </summary>
      /// <param name="status"></param>
      public void FilterOrderByState(OrderState state)
      {
         CurrentView.State = state;
         //ResetAllowNew();
      }

      /// <summary>
      /// 将选中的新医嘱下移一格
      /// </summary>
      /// <param name="selectedHandles">当前选中的医嘱行号</param>
      /// <returns>实际下移的格数</returns>
      public int MoveNewOrderDown(int[] selectedHandles)
      {
         // 首先检查是否可以下移
         string errMsg = CheckCanBeMoveDown(selectedHandles);
         if ((errMsg == null) || (errMsg.Length == 0))
         {
            CurrentView.BeginInit();

            // 通过将选中记录的下一条医嘱上移n位实现下移的效果
            // (如果下一条医嘱属于分组或关联了申请单，则同组的要一起上移)
            int nextIndex = selectedHandles[selectedHandles.Length - 1] + 1;
            int newIndex = nextIndex - selectedHandles.Length;
            int brothersCount = GetCountOfBrothers(nextIndex, true);
            for (int increment = 0; increment <= brothersCount; increment++)
            {
               CurrentView.Move(nextIndex + increment, newIndex + increment);
               // 移动医嘱后自动调整时间（移上来的医嘱的开始时间不能大于下一条）
               if (CurrentView[nextIndex + increment].OrderCache.StartDateTime > CurrentView[newIndex + increment + 1].OrderCache.StartDateTime)
                  CurrentView[nextIndex + increment].OrderCache.StartDateTime = CurrentView[newIndex + increment + 1].OrderCache.StartDateTime;
            }
            CurrentView.EndInit();
            return brothersCount + 1;
         }
         else
            throw new DataCheckException(errMsg, "选中行");
      }

      /// <summary>
      /// 将选中的新医嘱上移一格
      /// </summary>
      /// <param name="selectedHandles">当前选中的医嘱行号</param>
      /// <return>实际上移的格数</return>
      public int MoveNewOrderUp(int[] selectedHandles)
      {
         // 首先检查是否可以上移
         string errMsg = CheckCanBeMoveUp(selectedHandles);
         if ((errMsg == null) || (errMsg.Length == 0))
         {
            CurrentView.BeginInit();

            // 通过将选中记录的前一条医嘱下移n位实现上移的效果
            // (如果下一条医嘱属于分组或关联了申请单，则同组的要一起上移)
            int nextIndex = selectedHandles[0] - 1;
            int newIndex = nextIndex + selectedHandles.Length;
            int brothersCount = GetCountOfBrothers(nextIndex, false);
            for (int increment = 0; increment <= brothersCount; increment++)
            {
               CurrentView.Move(nextIndex - increment, newIndex - increment);
               // 移动医嘱后自动调整时间（移下来的医嘱的开始时间不能小于上一条）
               if (CurrentView[newIndex - increment].OrderCache.StartDateTime < CurrentView[newIndex - increment - 1].OrderCache.StartDateTime)
                  CurrentView[newIndex - increment].OrderCache.StartDateTime = CurrentView[newIndex - increment - 1].OrderCache.StartDateTime;
            }
            CurrentView.EndInit();
            return brothersCount + 1;
         }
         else
            throw new DataCheckException(errMsg, "选中行");
      }

      /// <summary>
      /// 取消已审核医嘱
      /// </summary>
      /// <param name="rowHandles"></param>
      public void CancelOrder(int[] selectedHandles)
      {
         Order[] selectedOrders = GetSlectedOrders(selectedHandles);

         CurrentView.BeginInit();
         m_CoreLogic.CancelOrder(selectedOrders, m_DoctorCode, DateTime.Now, IsTempOrder);
         CurrentView.EndInit();
      }

      /// <summary>
      /// 设置长期医嘱的停止日期
      /// </summary>
      /// <param name="selectedHandles"></param>
      public void SetLongOrderCeaseInfo(int[] selectedHandles, DateTime ceaseTime)
      {
         Order[] selectedOrders = GetSlectedOrders(selectedHandles);

         CurrentView.BeginInit();
         m_CoreLogic.SetLongOrderCeaseInfo(selectedOrders, m_DoctorCode, ceaseTime);
         CurrentView.EndInit();
      }

      /// <summary>
      /// 保存皮试信息
      /// </summary>
      /// <param name="specSerialNo"></param>
      /// <param name="druggeryName"></param>
      /// <param name="skinTestResultKind"></param>
      public void SaveSkinTestResult(decimal specSerialNo, string druggeryName, SkinTestResultKind skinTestResultKind)
      {
         DataTable newResult =
            m_CoreLogic.SaveSkinTestResult(m_DoctorCode, specSerialNo, druggeryName, skinTestResultKind);
         _skinTestResultTable.Merge(newResult);
      }

      /// <summary>
      /// 获取指定医生为当前病人在指定时间之后新开的医嘱
      /// </summary>
      /// <param name="doctorCode">医生工号</param>
      /// <param name="startTime">指定的时间</param>
      /// <param name="needDrug">获取药品或治疗项目医嘱标志，true：药品</param>
      /// <returns></returns>
      public DataTable GetNewOrder(string doctorCode, DateTime startTime, bool needDrug)
      {
         return m_CoreLogic.GetNewOrder(doctorCode, startTime, needDrug);
      }

      /// <summary>
      /// 将最近做过修改的医嘱保存到数据库中
      /// </summary>
      /// <param name="skipWarnning">是否跳过警告信息</param>
      public void SaveOrderTableData(bool skipWarnning)
      {
         if (!HadChanged)
            return;

         try
         {
            DoSaveOrderData(IsTempOrder, skipWarnning); // 先保存当前浏览的数据

            DoSaveOrderData(!IsTempOrder, skipWarnning);
         }
         catch
         {
            throw;
         }
      }

      /// <summary>
      /// 手工同步指定的类型的医嘱表数据到HIS中（在调用前要手工执行数据保存方法）
      /// </summary>
      public void ManualSynchDataToHIS()
      {
         FireProcessStarting(GetProcessHint(ConstNames.OpSend, IsTempOrder));
         OrderTable currentTable = m_CoreLogic.GetCurrentOrderTable(IsTempOrder);
         m_CoreLogic.SynchDataToHIS(currentTable, m_DoctorCode, m_MacAddress);

         FireProcessStarting(GetProcessHint(ConstNames.OpSend, !IsTempOrder));
         currentTable = m_CoreLogic.GetCurrentOrderTable(!IsTempOrder);
         m_CoreLogic.SynchDataToHIS(currentTable, m_DoctorCode, m_MacAddress);
      }

      public string GetHintOfEditOperation(int focusedRowHandle)
      {
         // 病人已出院，则不能编辑医嘱
         if (!AllowEdit)
            return "病人已出区或已出院，不能编辑医嘱";

         Order focusedOrder = GetOrderByRowHandle(CurrentView, focusedRowHandle);
         OrderState orderstate = OrderState.None;
         bool link2Request = false;
         if (focusedOrder != null)
         {
            orderstate = focusedOrder.State;
            TempOrder temp = focusedOrder as TempOrder;
            if (temp != null)
               link2Request = (temp.ApplySerialNo != 0);
         }

         if (m_CoreLogic.HasOutHospitalOrder)
            return "已开出院医嘱，在取消前只能编辑出院带药医嘱";
         else if (m_CoreLogic.HasShiftDeptOrder)
            return "已开转科医嘱，在取消前不能修改医嘱";

         switch (orderstate)
         {
            case OrderState.Ceased:
               return "该医嘱已停止，不能修改";
            case OrderState.Cancellation:
               return "该医嘱已取消，不能修改";
            case OrderState.Executed:
               return "该医嘱已执行";
            case OrderState.Audited:
               if (link2Request)
                  return "此医嘱和申请单关联，如要修改或删除，请在申请单模块进行";
               else
                  return "该医嘱已审核(可以取消)";
            case OrderState.New:
               if (link2Request)
                  return "此医嘱和申请单关联，如要修改或删除，请在申请单模块进行";
               else
                  return "双击该条医嘱可以在医嘱编辑区域进行修改";
            default:
               return "";
         }
      }

      /// <summary>
      /// 从医嘱列表中剪切选中的医嘱
      /// </summary>
      /// <param name="selectedHandles"></param>
      /// <returns>剪切出来的医嘱对象</returns>
      public Order[] CutOrdersFromList(int[] selectedHandles)
      {
         Order[] selectedOrders = GetSlectedOrders(selectedHandles);

         // 将选中医嘱从医嘱表中移出
         m_CoreLogic.DeleteNewOrder(selectedOrders, IsTempOrder);
         return selectedOrders;
      }

      /// <summary>
      /// 从医嘱列表中复制选中的医嘱
      /// </summary>
      /// <param name="selectedHandles"></param>
      /// <returns></returns>
      public Order[] CopyOrdersFromList(int[] selectedHandles)
      {
         Order[] selectedOrders = GetSlectedOrders(selectedHandles);

         Order[] cloneOrders;
         cloneOrders = new Order[selectedOrders.Length];
         for (int index = 0; index < selectedOrders.Length; index++)
         {
            if (IsTempOrder)
               cloneOrders[index] = PersistentObjectFactory.CloneEopBaseObject(selectedOrders[index]) as TempOrder;
            else
               cloneOrders[index] = PersistentObjectFactory.CloneEopBaseObject(selectedOrders[index]) as LongOrder;
         }

         return cloneOrders;
      }

      /// <summary>
      /// 检查是否允许向当前医嘱Table的当前位置插入医嘱
      /// </summary>
      /// <returns>true:允许 false:不允许, 在异常中保存不能插入的原因</returns>
      public void CheckAllowInsertOrder(int[] selectedHandles)
      {
         if ((selectedHandles != null) && (selectedHandles.Length > 1))
            throw new ArgumentException("选中多条医嘱的情况下不能插入医嘱");

         Order focusedOrder = null;
         if ((selectedHandles != null) && (selectedHandles.Length == 1))
            focusedOrder = CurrentOrderTable.Orders[selectedHandles[0]];

         m_CoreLogic.CheckCanInsertOrder(focusedOrder, IsTempOrder, null);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="contents"></param>
      public void InsertSuiteOrder(object[,] contents, Order focusedOrder)
      {
         CurrentView.BeginInit();
         int insertedNum = m_CoreLogic.InsertSuiteOrder(CurrentOrderTable, m_DoctorCode, contents, focusedOrder);
         int remainNum = contents.GetUpperBound(0) - insertedNum + 1;
         if (remainNum != 0)
            CustomMessageBox.MessageShow("有" + remainNum.ToString() + "条记录因为不满足校验规则没有插入", CustomMessageBoxKind.InformationOk);
         CurrentView.EndInit();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="currentOrderTable"></param>
      /// <param name="contents"></param>
      /// <param name="fousedOrder"></param>
      public void InsertSuiteOrder(OrderTable currentOrderTable, object[,] contents, Order fousedOrder)
      {
         int insertedNum = m_CoreLogic.InsertSuiteOrder(currentOrderTable, m_DoctorCode, contents, fousedOrder);
         int remainNum = contents.GetUpperBound(0) - insertedNum + 1;
         if (remainNum != 0)
            CustomMessageBox.MessageShow("有" + remainNum.ToString() + "条记录因为不满足校验规则没有插入", CustomMessageBoxKind.InformationOk);
      }

      /// <summary>
      /// 根据标志返回对应医嘱表
      /// </summary>
      /// <param name="isTempOrder"></param>
      /// <returns></returns>
      public OrderTable GetCurrentOrderTable(bool isTempOrder)
      {
         return m_CoreLogic.GetCurrentOrderTable(isTempOrder);
      }

      /// <summary>
      /// 检查当前的医嘱内容中的项目是否满足处方规则
      /// </summary>
      /// <param name="content">医嘱内容对象</param>
      public void CheckRecipeRule(OrderContent content)
      {
         if ((content == null) || (content.Item == null) || (!content.Item.KeyInitialized))
            return;

         if (CoreBusinessLogic.BusinessLogic.EnableOrderRules && (m_RepRuleChecker != null))
         {
            switch (content.OrderKind)
            {
               case OrderContentKind.Druggery:
               case OrderContentKind.OutDruggery:
                  m_RepRuleChecker.CheckDruggery((content.Item as Druggery), content.Amount, content.CurrentUnit);
                  break;
               case OrderContentKind.ChargeItem:
                  m_RepRuleChecker.CheckItem((content.Item as ChargeItem), content.Amount, content.CurrentUnit);
                  break;
            }
         }
         if (CoreBusinessLogic.BusinessLogic.EnableItemAgeWarning && (content.OrderKind == OrderContentKind.ChargeItem))
         {
            //modified by zhouhui 此处的报警项目可能为空
            if (CoreBusinessLogic.BusinessLogic.WaringItem == null)
               return;

            int pos = CoreBusinessLogic.BusinessLogic.WaringItem.IndexOf(content.Item.Code);
            if ((pos >= 0)
               && ((CoreBusinessLogic.BusinessLogic.WaringItem.Length <= (pos + content.Item.Code.Length))
                  || (CoreBusinessLogic.BusinessLogic.WaringItem[pos + content.Item.Code.Length] == ',')))
            {
               if (CurrentPatient.PersonalInformation.CurrentAge <= CoreBusinessLogic.BusinessLogic.MaxWarningAge)
                  throw new DataCheckException(String.Format(ConstMessages.FormatOrderSaveWarning, CoreBusinessLogic.BusinessLogic.MaxWarningAge), ConstNames.ContentChargeItem, 1);
            }
         }
      }

      /// <summary>
      /// 保存当前编辑的成套的明细数据
      /// </summary>
      public void SaveCurrentSuiteDetailData()
      {
         m_CoreLogic.SaveCurrentSuiteDetailData();
      }
      #endregion
   }
}
