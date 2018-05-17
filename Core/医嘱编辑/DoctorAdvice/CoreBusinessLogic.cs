using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using DrectSoft.Common.Eop;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using DrectSoft.Core.TimeLimitQC;
using DrectSoft.FrameWork.WinForm.Plugin;
using Eop = DrectSoft.Common.Eop;

namespace DrectSoft.Core.DoctorAdvice
{
    /// <summary>
    /// 处理有关医嘱的核心逻辑(存取数据、ORM转换、业务逻辑控制、数据校验等)
    /// 支持对病人医嘱的编辑和成套医嘱数据的编辑
    /// </summary>
    internal class CoreBusinessLogic
    {
        #region const & readonly
        //private const string QCValShiftDept = "O:3111"; // 转科医嘱
        //private const string QCValDanger = "O:3102Z";   // 病重医嘱
        //private const string QCValOperation = "O:3105"; // 手术医嘱
        //private const string QCValNormalPat = "P:1501N"; //正常状态病人
        #endregion

        #region static common methods
        /// <summary>
        /// 得到指定类型的医嘱在数据库中的表名
        /// </summary>
        /// <param name="isTemp">医嘱类型（长期、临时）</param>
        /// <returns>医嘱表名</returns>
        public static string GetOrderTableName(bool isTemp)
        {
            if (isTemp)
                return ConstSchemaNames.TempOrderTableName;
            else
                return ConstSchemaNames.LongOrderTableName;
        }

        /// <summary>
        /// 获得指定类型的医嘱表医中医嘱序号的字段名
        /// </summary>
        /// <param name="isTemp">医嘱类型（长期、临时）</param>
        /// <returns>医嘱序号的字段名</returns>
        public static string GetSerialNoField(bool isTemp)
        {
            if (isTemp)
                return ConstSchemaNames.OrderTempColSerialNo;
            else
                return ConstSchemaNames.OrderLongColSerialNo;
        }

        private static void DeserializerDoctorAdviceSetting()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DoctorAdviceSetting));
            //FileStream fileStream = new FileStream(fileName, FileMode.Open);
            _customSetting = (DoctorAdviceSetting)serializer.Deserialize(BasicSettings.GetConfig("DoctorAdviceSetting"));
            //fileStream.Close();
            _customSetting.CalcOtherVariables();
        }

        /// <summary>
        /// 将传入的医嘱处理操作错误信息进行格式化
        /// </summary>
        /// <param name="processName">操作名称</param>
        /// <param name="messages">错误信息</param>
        /// <returns></returns>
        public static string FormatProcessErrorMessage(string processName, string messages)
        {
            return String.Format(CultureInfo.CurrentCulture
               , ConstMessages.FormatOpError, processName, messages);
        }
        #endregion

        #region public properties
        /// <summary>
        /// 有关医嘱的自定义设置
        /// </summary>
        public static DoctorAdviceSetting CustomSetting
        {
            get
            {
                if (_customSetting == null)
                    DeserializerDoctorAdviceSetting();
                return _customSetting;
            }
        }
        private static DoctorAdviceSetting _customSetting;

        /// <summary>
        /// 业务逻辑设置
        /// </summary>
        public static BusinessLogicSetting BusinessLogic
        {
            get { return CustomSetting.BusinessLogic; }
        }

        /// <summary>
        /// 医嘱内容类别数据表
        /// </summary>
        public DataTable OrderContentCatalog
        {
            get
            {
                if (_orderContentCatalog == null)
                    CreateOrderContentCatalogTable();
                return _orderContentCatalog;
            }
        }
        private DataTable _orderContentCatalog;

        /// <summary>
        /// 当天医嘱最早允许开始时间(以入院时间为准)
        /// </summary>
        public DateTime MinStartDateTime
        {
            get
            {
                if (CurrentPatient != null)
                    return CurrentPatient.InfoOfAdmission.AdmitInfo.StepOneDate;
                else
                    return DateTime.Now;
            }
        }

        /// <summary>
        /// 新医嘱的开始时间在当前时间之前多长时间就进行提示。以小时为单位
        /// </summary>
        public DateTime WarnStartDateTime
        {
            get
            {
                return DateTime.Now.AddHours(-BusinessLogic.StartDateWarningHours);
            }
        }

        /// <summary>
        /// 默认的出院时间
        /// </summary>
        public DateTime DefaultLeaveHospitalTime
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
        /// 当前处理的病人
        /// </summary>
        public Inpatient CurrentPatient
        {
            get { return _currentPatient; }
            set
            {
                //if (_currentPatient != value)
                {
                    if ((value == null) || ((value.NoOfHisFirstPage == "-1") && (value.NoOfFirstPage == 0)))
                        _currentPatient = null;
                    else
                        _currentPatient = value;
                    DoAfterSwitchPatient();
                }
            }
        }
        private Inpatient _currentPatient;

        /// <summary>
        /// 当前处理的病人的首页序号（初始化新医嘱对象数据时需要）
        /// 提供此属性是为了兼容医嘱和成套医嘱的处理
        /// </summary>
        public decimal CurrentPatientNo
        {
            get
            {
                if (m_ProcessModel != EditorCallModel.EditSuite)
                {
                    if (CurrentPatient != null)
                        return CurrentPatient.NoOfFirstPage;
                    else
                        return -1;
                }
                else
                {
                    return 1;
                }
            }
        }

        /// <summary>
        /// 当前病人的医嘱中是否包含有效的“出院医嘱”（没有被取消的）
        /// </summary>
        public bool HasOutHospitalOrder
        {
            get
            {
                return (m_TempTable != null) && m_TempTable.HasOutHospitalOrder;
            }
        }

        /// <summary>
        /// 当前病人的医嘱中是否包含有效的“转科医嘱”(新的、还未执行，可能已审核)
        /// </summary>
        public bool HasShiftDeptOrder
        {
            get
            {
                return (m_TempTable != null) && m_TempTable.HasShiftDeptOrder;
            }
        }

        /// <summary>
        /// 是否允许添加新长期医嘱
        /// </summary>
        public bool AllowAddLong
        {
            get
            {
                // 不允许添加新长期医嘱的情况:
                //    当前医嘱表只读
                //    临时医嘱中包含出院医嘱，不能添加长期医嘱
                //    包含有效的转科医嘱(在医嘱编辑的逻辑中要保证有效的转科医嘱只可能出现在病人转科前)
                return (m_LongTable != null) && (!HasOutHospitalOrder) && (!HasShiftDeptOrder)
                   && (m_LongTable.DefaultView.AllowEdit) && (m_LongTable.DefaultView.AllowNew);
            }
        }

        /// <summary>
        /// 是否允许添加临时医嘱
        /// </summary>
        public bool AllowAddTemp
        {
            get
            {
                // 不允许添加新临时医嘱的情况:
                //    当前医嘱表只读
                //    包含有效的转科医嘱(在医嘱编辑的逻辑中要保证有效的转科医嘱只可能出现在病人转科前)
                return (m_TempTable != null) && (!HasShiftDeptOrder)
                   && (m_TempTable.DefaultView.AllowEdit) && (m_TempTable.DefaultView.AllowNew);
            }
        }

        /// <summary>
        /// 标记医嘱表数据是否发生改变
        /// </summary>
        public bool HadChanged
        {
            get
            {
                return (m_LongTable != null) && (m_TempTable != null)
                   && (m_LongTable.HadChanged || m_TempTable.HadChanged);
            }
        }

        /// <summary>
        /// 标记是否还有未发送的数据
        /// </summary>
        public bool HasNotSendData
        {
            get
            {
                return (m_LongTable != null) && (m_TempTable != null)
                   && (m_LongTable.HasNotSendData || m_TempTable.HasNotSendData);
            }
        }

        /// <summary>
        /// 成套医嘱逻辑处理对象
        /// </summary>
        public SuiteOrderHandle SuiteHelper
        {
            get
            {
                return _suiteHelper;
            }
            set
            {
                _suiteHelper = value;
                DoAfterSwitchSuite(this, new EventArgs());
            }
        }
        private SuiteOrderHandle _suiteHelper;
        #endregion

        #region private variables
        private OrderTable m_LongTable;
        private OrderTable m_TempTable;
        private IDataAccess m_SqlExecutor;
        private DataTable m_OutputTable;
        /// <summary>
        /// 标明是处理患者医嘱还是成套医嘱数据
        /// </summary>
        private EditorCallModel m_ProcessModel;
        private Qcsv m_Qcsv;
        //private LogSendRecord m_SynchLogHelper;
        #endregion

        #region private properties
        private DataTable TableOfSynchTempOrder
        {
            get
            {
                if (_tableOfSynchTempOrder == null)
                {
                    _tableOfSynchTempOrder = m_SqlExecutor.ExecuteDataTable(String.Format(CultureInfo.CurrentCulture
                       , ConstSqlSentences.FormatSelectOrderSchema, GetOrderTableName(true)));
                    m_SqlExecutor.ResetTableSchema(_tableOfSynchTempOrder, GetOrderTableName(true));
                }
                return _tableOfSynchTempOrder;
            }
        }
        private DataTable _tableOfSynchTempOrder;

        private DataTable TableOfSynchLongOrder
        {
            get
            {
                if (_tableOfSynchLongOrder == null)
                {
                    _tableOfSynchLongOrder = m_SqlExecutor.ExecuteDataTable(String.Format(CultureInfo.CurrentCulture
                       , ConstSqlSentences.FormatSelectOrderSchema, GetOrderTableName(false)));
                    m_SqlExecutor.ResetTableSchema(_tableOfSynchLongOrder, GetOrderTableName(false));
                }
                return _tableOfSynchLongOrder;
            }
        }
        private DataTable _tableOfSynchLongOrder;

        //private IExchangeInfoServer m_InfoServer {
        //    get {
        //        if (_infoServer == null)
        //            _infoServer = ExchangeInfoHelper.ExchangeInfoServer;
        //        return _infoServer;
        //    }
        //}
        //private IExchangeInfoServer _infoServer;
        #endregion

        #region ctors
        /// <summary>
        /// 创建医嘱逻辑处理对象
        /// </summary>
        /// <param name="app"></param>
        /// <param name="callModel">调用模式</param>
        public CoreBusinessLogic(IEmrHost app, EditorCallModel callModel)
            : this(app.SqlHelper)
        {
            PersistentObjectFactory.SqlExecutor = app.SqlHelper;
            m_SqlExecutor = app.SqlHelper;
            m_Qcsv = new Qcsv();

            if (callModel == EditorCallModel.EditSuite)
            {
                _suiteHelper = new SuiteOrderHandle(app, false);
                _suiteHelper.AfterSwitchSuite += new EventHandler(DoAfterSwitchSuite);
            }
            m_ProcessModel = callModel;
        }

        /// <summary>
        /// 创建查询模式的逻辑处理
        /// </summary>
        /// <param name="sqlHelper"></param>
        public CoreBusinessLogic(IDataAccess sqlHelper)
        {
            PersistentObjectFactory.SqlExecutor = sqlHelper;
            m_SqlExecutor = sqlHelper;
            m_ProcessModel = EditorCallModel.Query;
        }

        /// <summary>
        /// 临时添加
        /// </summary>
        /// <param name="sqlHelper"></param>
        /// <param name="callModel"></param>
        public CoreBusinessLogic(IDataAccess sqlHelper, EditorCallModel callModel)
            : this(sqlHelper)
        {
            m_ProcessModel = EditorCallModel.EditOrder;
        }
        #endregion

        #region custom event handler
        /// <summary>
        /// 出院医嘱改变事件(增加或被删除、取消)。
        /// </summary>
        public event EventHandler OutHospitalOrderChanged
        {
            add
            {
                onOutHospitalOrderChanged = (EventHandler)Delegate.Combine(onOutHospitalOrderChanged, value);
            }
            remove
            {
                onOutHospitalOrderChanged = (EventHandler)Delegate.Remove(onOutHospitalOrderChanged, value);
            }
        }
        private EventHandler onOutHospitalOrderChanged;

        private void FireOutHospitalOrderChanged(object sender, EventArgs e)
        {
            if (onOutHospitalOrderChanged != null)
                onOutHospitalOrderChanged(sender, e);
        }

        /// <summary>
        /// 病人医嘱数据发生改变事件。
        /// </summary>
        public event EventHandler PatientOrderDataChanged
        {
            add
            {
                onPatientOrderDataChanged = (EventHandler)Delegate.Combine(onPatientOrderDataChanged, value);
            }
            remove
            {
                onPatientOrderDataChanged = (EventHandler)Delegate.Remove(onPatientOrderDataChanged, value);
            }
        }
        private EventHandler onPatientOrderDataChanged;

        private void FirePatientOrderDataChanged(EventArgs e)
        {
            if (onPatientOrderDataChanged != null)
                onPatientOrderDataChanged(this, e);
        }
        #endregion

        #region event hadle
        private void DoAfterOutHospitalOrderChanged(object sender, EventArgs e)
        {
            FireOutHospitalOrderChanged(sender, e);
        }

        private void DoAfterSwitchSuite(object sender, EventArgs e)
        {
            //m_ProcessModel = EditorCallModel.EditSuite;

            if (m_TempTable != null)
            {
                m_TempTable.OutHospitalOrderChanged -= new EventHandler(DoAfterOutHospitalOrderChanged);
                m_TempTable.DefaultView.ListChanged -= new ListChangedEventHandler(DoOrderViewListChanged);
            }
            if (m_LongTable != null)
            {
                m_LongTable.DefaultView.ListChanged -= new ListChangedEventHandler(DoOrderViewListChanged);
            }

            if (SuiteHelper.CurrentSuiteNo <= 0)
            {
                m_TempTable = null;
                m_LongTable = null;
                return;
            }

            CreateTempOrderTable(SuiteHelper.TempOrderTable);
            CreateLongOrderTable(SuiteHelper.LongOrderTable);

            m_TempTable.DefaultView.AllowEdit = true;
            m_LongTable.DefaultView.AllowEdit = true;
        }
        #endregion

        #region private common methods
        private void CreateOrderContentCatalogTable()
        {
            if (_orderContentCatalog != null)
                return;

            _orderContentCatalog = new DataTable();
            _orderContentCatalog.Locale = CultureInfo.CurrentCulture;
            // 创建表结构
            _orderContentCatalog.Columns.AddRange(new DataColumn[] {
              new DataColumn(ConstSchemaNames.ContentCatalogColId, typeof(int))
            , new DataColumn(ConstSchemaNames.ContentCatalogColName, typeof(string))
            , new DataColumn(ConstSchemaNames.ContentCatalogColFlag, typeof(int))
            });
            _orderContentCatalog.Columns[ConstSchemaNames.ContentCatalogColId].Caption = ConstNames.ContentCatalogId; // 对应OrderContentKind中的值
            _orderContentCatalog.Columns[ConstSchemaNames.ContentCatalogColName].Caption = ConstNames.ContentCatalogName;
            _orderContentCatalog.Columns[ConstSchemaNames.ContentCatalogColFlag].Caption = ConstNames.ContentCatalogFlag; // 医嘱管理标志，说明分类适用的医嘱类别，对应OrderManagerKind中的值
            _orderContentCatalog.PrimaryKey = new DataColumn[] { _orderContentCatalog.Columns[ConstSchemaNames.ContentCatalogColId] };

            // 加入数据
            InsertOrderContentCatalogRow(OrderContentKind.Druggery, ConstNames.ContentDruggery, OrderManagerKind.Normal);
            InsertOrderContentCatalogRow(OrderContentKind.ChargeItem, ConstNames.ContentChargeItem, OrderManagerKind.Normal);
            InsertOrderContentCatalogRow(OrderContentKind.GeneralItem, ConstNames.ContentGeneralItem, OrderManagerKind.ForLong);
            InsertOrderContentCatalogRow(OrderContentKind.ClinicItem, ConstNames.ContentClinicItem, OrderManagerKind.Normal);
            InsertOrderContentCatalogRow(OrderContentKind.OutDruggery, ConstNames.ContentOutDruggery, OrderManagerKind.ForTemp);
            InsertOrderContentCatalogRow(OrderContentKind.Operation, ConstNames.ContentOperation, OrderManagerKind.ForTemp);
            InsertOrderContentCatalogRow(OrderContentKind.TextNormal, ConstNames.ContentTextNormal, OrderManagerKind.Normal);
            InsertOrderContentCatalogRow(OrderContentKind.TextShiftDept, ConstNames.ContentTextShiftDept, OrderManagerKind.ForTemp);
            //InsertOrderContentCatalogRow(OrderContentKind.TextAfterOperation, "术后医嘱", OrderManagerKind.ForLong);
            InsertOrderContentCatalogRow(OrderContentKind.TextLeaveHospital, ConstNames.ContentTextLeaveHospital, OrderManagerKind.ForTemp);

            //_orderContentCatalog.DefaultView.Sort = ConstSchemaNames.ContentCatalogColName;
        }

        private void InsertOrderContentCatalogRow(OrderContentKind contentKind, string name, OrderManagerKind managerKind)
        {
            DataRow row = _orderContentCatalog.NewRow();

            row[ConstSchemaNames.ContentCatalogColId] = (int)contentKind;
            row[ConstSchemaNames.ContentCatalogColName] = name;
            row[ConstSchemaNames.ContentCatalogColFlag] = (int)managerKind;

            _orderContentCatalog.Rows.Add(row);
        }

        private void DoAfterSwitchPatient()
        {
            if (m_TempTable != null)
            {
                m_TempTable.OutHospitalOrderChanged -= new EventHandler(DoAfterOutHospitalOrderChanged);
                m_TempTable.DefaultView.ListChanged -= new ListChangedEventHandler(DoOrderViewListChanged);
            }
            if (m_LongTable != null)
            {
                m_LongTable.DefaultView.ListChanged -= new ListChangedEventHandler(DoOrderViewListChanged);
            }

            if (CurrentPatient == null)
            {
                m_TempTable = null;
                m_LongTable = null;
                return;
            }

            CreateTempOrderTable(QueryOrderData(true));
            CreateLongOrderTable(QueryOrderData(false));

            // 如果病人在病区才允许编辑
            bool allowEdit;
            switch (CurrentPatient.State)
            {
                case InpatientState.InWard:
                case InpatientState.InICU:
                case InpatientState.InDeliveryRoom:
                    allowEdit = true;
                    break;
                default:
                    allowEdit = false;
                    break;
            }

            m_TempTable.DefaultView.AllowEdit = allowEdit && (m_ProcessModel != EditorCallModel.Query);
            m_LongTable.DefaultView.AllowEdit = allowEdit && (m_ProcessModel != EditorCallModel.Query);

            // 检查已执行的长期医嘱是否已到停止时间，如果已到，则将其状态改为停止
            AutoHandleExecutingLongOrder();

            //m_SynchLogHelper.CurrentPatient = CurrentPatient;
        }

        private void CreateTempOrderTable(DataTable orderData)
        {
            m_TempTable = new OrderTable(orderData, true, m_SqlExecutor);
            m_TempTable.OutHospitalOrderChanged += new EventHandler(DoAfterOutHospitalOrderChanged);
            m_TempTable.DefaultView.ListChanged += new ListChangedEventHandler(DoOrderViewListChanged);
        }

        private void CreateLongOrderTable(DataTable orderData)
        {
            m_LongTable = new OrderTable(orderData, false, m_SqlExecutor);
            m_LongTable.DefaultView.ListChanged += new ListChangedEventHandler(DoOrderViewListChanged);
        }

        /// <summary>
        /// 查询指定类别的医嘱数据
        /// </summary>
        /// <param name="isTemp">医嘱种类标记（长期、临时）</param>
        /// <returns>医嘱数据表</returns>
        private DataTable QueryOrderData(bool isTemp)
        {
            DataTable table = null;
            if (BusinessLogic.EnableEmrOrderModul) // 启用EMR医嘱功能时，读EMR自己的医嘱表
            {

                string tableName = GetOrderTableName(isTemp);

                table = m_SqlExecutor.ExecuteDataTable(
                   String.Format(CultureInfo.CurrentCulture
                      , ConstSqlSentences.FormatSelectOrderData, tableName, _currentPatient.NoOfFirstPage, GetSerialNoField(isTemp))
                      , CommandType.Text);
                //m_SqlExecutor.ResetTableSchema(table, tableName);
            }
            else // 通过接口读取HIS的医嘱数据
            {
                //TODO 读取his医嘱数据集
                //string[,] parameters = new string[3, 6];

                //parameters[0, 0] = "queryType";
                //parameters[1, 0] = "int";
                //parameters[2, 0] = "1"; // 查单个病人
                //parameters[0, 1] = "hisFirstPageNo";
                //parameters[1, 1] = "string";
                //parameters[2, 1] = CurrentPatient.NoOfHisFirstPage.ToString();
                //parameters[0, 2] = "wardCode";
                //parameters[1, 2] = "string";
                //parameters[2, 2] = "";
                //parameters[0, 3] = "deptCode";
                //parameters[1, 3] = "string";
                //parameters[2, 3] = "";
                //parameters[0, 4] = "orderType";
                //parameters[1, 4] = "int";
                //parameters[2, 4] = isTemp ? "2" : "1";
                //parameters[0, 5] = "orderState";
                //parameters[1, 5] = "int";
                //parameters[2, 5] = "0"; // 所有医嘱

                //string sExio;
                //sExio = m_InfoServer.BuildExchangeInfoString(ExchangeInfoOrderConst.MsgGetHisOrder
                //   , ExchangeInfoOrderConst.EmrSystemName
                //   , ExchangeInfoOrderConst.HisSystemName, parameters);

                //string outMsg;
                //if (m_InfoServer.AddSyncExchangeInfo(sExio, ExchangeInfoOrderConst.DefaultEncoding, out outMsg) != ResponseFlag.Complete)
                //    throw new ApplicationException(outMsg);
                //// 转换数据集
                //DataSet resultDS = new DataSet();
                //if (!String.IsNullOrEmpty(outMsg)) {
                //    // 数据集转换成byte数组
                //    MemoryStream source = new MemoryStream();
                //    byte[] data = ExchangeInfoOrderConst.DefaultEncoding.GetBytes(outMsg);
                //    source.Write(data, 0, data.Length);
                //    source.Seek(0, SeekOrigin.Begin);
                //    resultDS.ReadXml(source);
                //}
                //else
                //    throw new ApplicationException(ConstMessages.ExceptionCallRemoting);
                //// 首页序号更新成EMR自己的
                //table = resultDS.Tables[0];
                //// 返回的数据集里只有HIS首页序号，要替换成EMR自己的首页序号
                //table.Columns.Remove("hissyxh");
                //DataColumn col = new DataColumn("syxh", typeof(decimal));
                //col.DefaultValue = CurrentPatient.NoOfFirstPage;
                ////foreach (DataRow row in table.Rows)
                ////   row["syxh"] = CurrentPatient.NoOfFirstPage;
                //table.AcceptChanges();
            }

            return table;
        }

        private void GenerateOutputTable()
        {
            m_OutputTable = new DataTable();
            m_OutputTable.Locale = CultureInfo.CurrentCulture;
            DataColumn col;
            col = new DataColumn(ConstSchemaNames.OrderOutputColProductSerialNo, Type.GetType("System.String"));
            col.Caption = ConstNames.OrderOutputProductSerialNo;
            m_OutputTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.OrderOutputColDruggeryName, Type.GetType("System.String"));
            col.Caption = ConstNames.OrderOutputDruggeryName;
            m_OutputTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.OrderOutputColAmount, Type.GetType("System.Double"));
            col.Caption = ConstNames.OrderOutputAmount;
            m_OutputTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.OrderOutputColUnit, Type.GetType("System.String"));
            col.Caption = ConstNames.OrderOutputUnit;
            m_OutputTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.OrderOutputColUsageCode, Type.GetType("System.String"));
            col.Caption = ConstNames.OrderOutputUsageCode;
            m_OutputTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.OrderOutputColUsageName, Type.GetType("System.String"));
            col.Caption = ConstNames.OrderOutputUsageName;
            m_OutputTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.OrderOutputColFrequencyCode, Type.GetType("System.String"));
            col.Caption = ConstNames.OrderOutputFrequencyCode;
            m_OutputTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.OrderOutputColFrequencyName, Type.GetType("System.String"));
            col.Caption = ConstNames.OrderOutputFrequencyName;
            m_OutputTable.Columns.Add(col);
            col = new DataColumn(ConstSchemaNames.OrderOutputColDruggeryCode, Type.GetType("System.String"));
            col.Caption = ConstNames.OrderOutPutDruggeryCode;
            m_OutputTable.Columns.Add(col);

        }

        /// <summary>
        /// 医嘱对象List改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoOrderViewListChanged(object sender, ListChangedEventArgs e)
        {
            // “出院医嘱”、“转科医嘱”的添加或删除、取消，会影响到是否允许添加新医嘱
            //    为优化处理，需要额外的判断，以减少事件触发的次数
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                case ListChangedType.ItemAdded:
                case ListChangedType.ItemChanged:
                case ListChangedType.ItemDeleted:
                    FirePatientOrderDataChanged(new EventArgs());
                    break;
            }
        }

        private static void SetNewOrderTogether(OrderTable table)
        {
            Order temp;
            // 将新医嘱归到一起，放在已审核医嘱的后面
            bool hasMeetAudit = false;
            int lastAuditIndex = -1;
            List<int> newList = new List<int>();
            for (int rowHandle = table.Orders.Count - 1; rowHandle >= 0; rowHandle--)
            {
                temp = table.Orders[rowHandle];
                if ((temp.State != OrderState.Audited) && (temp.State != OrderState.New))
                    break;
                // 倒序查找，第一次遇到已审核记录时记下其位置
                // 如果在找到已审核记录后又发现新记录，则记下其位置
                if ((temp.State == OrderState.Audited) && (lastAuditIndex == -1))
                {
                    hasMeetAudit = true;
                    lastAuditIndex = rowHandle;
                }
                else if ((temp.State == OrderState.New) && hasMeetAudit)
                {
                    newList.Add(rowHandle);
                }
            }
            if ((newList.Count > 0) && (lastAuditIndex > -1))
            {
                // 倒序处理最后一条审核医嘱之前的新医嘱，将其按原先的顺序插入到所有审核医嘱的后面
                for (int index = newList.Count - 1; index >= 0; index--)
                {
                    table.MoveOrder(newList[index], lastAuditIndex + 1);
                    lastAuditIndex -= 1; // 移动后，最后一条审核医嘱的位置将前移一格
                }
            }
        }
        #endregion

        #region private methods of checking data
        private void CheckOrderStartDatetime(OrderTable currentTable, int targetIndex, DateTime startDateTime)
        {
            // 检查开始日期（必填）
            //    不能早于当天某个时间点(新增状态的医嘱)
            //    不能早于上一条医嘱的开始时间
            if (startDateTime.Date == DateTime.MinValue.Date)
                throw new DataCheckException(ConstMessages.CheckStartDateNull, OrderView.UNStartDate);
            else if (startDateTime < MinStartDateTime)
                throw new DataCheckException(String.Format(CultureInfo.CurrentCulture, ConstMessages.FormatStartDateMustAfter, MinStartDateTime)
                   , OrderView.UNStartDate);
            else
            {
                int preIndex = targetIndex - 1;
                bool isFirstNew;
                Order preOrder = null;
                if ((preIndex < 0) || (preIndex >= currentTable.Orders.Count))
                    isFirstNew = true;
                else
                {
                    preOrder = currentTable.Orders[preIndex];
                    isFirstNew = (preOrder.State != OrderState.New);
                }

                if (!isFirstNew)
                {
                    DateTime preStartTime;
                    // 如果前一条医嘱是组开始或组中间，说明它和当前处理的医嘱属于同一个分组，则取分组前的那条医嘱的开始时间
                    if ((preOrder.GroupPosFlag != GroupPositionKind.SingleOrder)
                       && (preOrder.GroupPosFlag != GroupPositionKind.GroupEnd))
                    {
                        preStartTime = MinStartDateTime; // 本组之前可能没有新医嘱
                        for (int handle = preIndex - 1; handle >= 0; handle--)
                        {
                            if (currentTable.Orders[handle].State == OrderState.New)
                            {
                                if (currentTable.Orders[handle].GroupSerialNo != preOrder.GroupSerialNo)
                                {
                                    preStartTime = currentTable.Orders[handle].StartDateTime;
                                    break;
                                }
                            }
                            else
                                break;
                        }
                    }
                    else
                        preStartTime = preOrder.StartDateTime;

                    if (startDateTime < preStartTime)
                        throw new DataCheckException(ConstMessages.CheckStartDateBeforPreRow, OrderView.UNStartDate);
                }
                if (startDateTime < WarnStartDateTime)
                    throw new DataCheckException(String.Format(CultureInfo.CurrentCulture, ConstMessages.FormatStartDateMustBefore, WarnStartDateTime)
                       , currentTable.IsTempOrder.ToString(), 0);
            }
        }

        private void CheckCeaseTimeOfLongOrder(OrderTable currentTable, Order orderTemp)
        {
            LongOrder longOrder = orderTemp as LongOrder;
            if ((!currentTable.IsTempOrder) && (longOrder != null))
            {
                if (longOrder.CeaseInfo.ExecuteTime != DateTime.MinValue)
                {
                    // 检查停止日期（只对长期医嘱有效，可以为空）
                    //    不能早于当前时间
                    //    不能早于医嘱开始时间 
                    if ((longOrder.CeaseInfo.ExecuteTime < DateTime.Now)
                       || (longOrder.CeaseInfo.ExecuteTime <= longOrder.StartDateTime))
                        throw new DataCheckException(ConstMessages.CheckCeaseDateBeforeStartDate, OrderView.UNCeaseDate);
                }
            }
        }

        private void CheckOrderContentData(OrderTable currentTable, OrderContent content, int targetIndex, GroupPositionKind currentGroupKind)
        {
            if (content == null)
                throw new DataCheckException(ConstMessages.ExceptionNullOrderObject, OrderView.UNContent);

            //    医嘱内容的类型是否允许在当前医嘱类别中出现
            if (HasOutHospitalOrder && (content.OrderKind != OrderContentKind.OutDruggery)
               && (content.OrderKind != OrderContentKind.TextLeaveHospital))
                throw new DataCheckException(ConstMessages.CheckOnlyAllowDruggery, OrderView.UNContent);

            if (!HasOutHospitalOrder) // 出院医嘱比较特殊，前后允许输入的内容有很大不同，所以不作下面的检查
            {
                OrderContentCatalog.DefaultView.RowFilter = GetOrderContentCatalogRowFilter(currentTable.IsTempOrder);
                OrderContentCatalog.DefaultView.Sort = ConstSchemaNames.ContentCatalogColId;
                int locateIndex = OrderContentCatalog.DefaultView.Find((int)content.OrderKind);
                if (locateIndex < 0)
                    throw new DataCheckException(ConstMessages.CheckCatalogNotSupport, OrderView.UNContent);
            }

            // 检查数据是否有问题
            string contentErr = content.CheckProperties();
            if (contentErr.Length > 0)
                throw new DataCheckException(contentErr, OrderView.UNContent);

            // 检查是否输入重复的药品或项目(只在新医嘱和已审核、正在执行的医嘱中检查，跳过分组的医嘱，目前不对临时医嘱做检查)
            if (currentTable.IsTempOrder)
                return;

            //add by zhouhui 成套医嘱的药品或项目不检查重复
            if ((currentGroupKind != GroupPositionKind.None) && (currentGroupKind != GroupPositionKind.SingleOrder))
                return;

            bool isGeneral = ((content.OrderKind == OrderContentKind.GeneralItem) && (!currentTable.IsTempOrder));
            //bool isOutDruggery = (content.OrderKind == OrderContentKind.OutDruggery);
            switch (content.OrderKind)
            {
                case OrderContentKind.Druggery:
                case OrderContentKind.ChargeItem:
                case OrderContentKind.GeneralItem:
                case OrderContentKind.Operation:
                case OrderContentKind.OutDruggery:
                    Order order;
                    for (int index = currentTable.Orders.Count - 1; index >= 0; index--)
                    {
                        if (index == targetIndex)
                            continue;
                        order = currentTable.Orders[index];
                        if (order.Content.OrderKind != content.OrderKind)
                        {
                            if ((m_ProcessModel == EditorCallModel.EditSuite)
                               && ((content.OrderKind == OrderContentKind.OutDruggery) || (order.Content.OrderKind == OrderContentKind.OutDruggery)))
                                throw new DataCheckException(ConstMessages.CheckNotAllowMixCatalogInSuite, OrderView.UNContent);
                            else
                                continue;
                        }
                        if (order.GroupPosFlag != GroupPositionKind.SingleOrder)
                            continue;
                        if ((order.Content.Item.Kind == content.Item.Kind)
                           && (order.Content.Item.KeyValue == content.Item.KeyValue))
                        {
                            switch (order.State)
                            {
                                case OrderState.New:
                                case OrderState.Audited:
                                    throw new DataCheckException(ConstMessages.CheckItemRepeatedInNew, OrderView.UNContent);
                                case OrderState.Executed:
                                    if ((!currentTable.IsTempOrder) && (!isGeneral))
                                        throw new DataCheckException(ConstMessages.CheckItemRepeatedInExecuting, OrderView.UNContent);
                                    break;
                            }
                        }
                    }
                    break;
            }
            //// 常规医嘱检查
            //if ((content.OrderKind == OrderContentKind.GeneralItem) && (!currentTable.IsTempOrder))
            //{
            //   LongOrder order;
            //   for (int index = currentTable.Orders.Count - 1; index >= 0; index--)
            //   {
            //      if (index == targetIndex)
            //         continue;
            //      order = currentTable.Orders[index] as LongOrder;
            //      if ((order.State == OrderState.Executed) || (order.State == OrderState.Ceased))
            //         break;

            //      if ((order.Content.OrderKind == OrderContentKind.GeneralItem)
            //         && (order.Content.Item.Kind == content.Item.Kind))
            //      {
            //         throw new DataCheckException("不能重复添加相同类型的常规医嘱", OrderView.UNContent);
            //      }
            //   }
            //}
        }

        private string CheckCanBeDeleted(OrderTable table, Order[] selectedOrders)
        {
            EditProcessFlag flag = CalcEditProcessFlag(table, selectedOrders);
            if ((flag & EditProcessFlag.Delete) > 0)
                return "";

            StringBuilder msgs = new StringBuilder();
            msgs.AppendLine(ConstMessages.CheckAllIsNew);
            msgs.AppendLine(ConstMessages.CheckDelAllOutOrder);

            return FormatProcessErrorMessage(ConstNames.OpDelete, msgs.ToString());
        }

        private string CheckCanSetGroup(OrderTable table, Order[] selectedOrders)
        {
            if (selectedOrders.Length == 1)
                throw new DataCheckException(ConstMessages.CheckOnlyOneRowInGroup, ConstMessages.ExceptionTitleOrder);

            EditProcessFlag flag = CalcEditProcessFlag(table, selectedOrders);
            if ((flag & EditProcessFlag.SetGroup) > 0)
                return "";

            StringBuilder msgs = new StringBuilder();
            msgs.AppendLine(ConstMessages.CheckNewInGroup);
            msgs.AppendLine(ConstMessages.CheckSerialInGroup);

            return FormatProcessErrorMessage(ConstNames.OpSetGroup, msgs.ToString());
        }

        private void AutoHandleExecutingLongOrder()
        {
            foreach (LongOrder temp in m_LongTable.Orders)
            {
                // 将已到停止时间的医嘱改成已停止状态
                if ((temp.State == OrderState.Executed) && (temp.CeaseInfo != null) && (temp.CeaseInfo.HadInitialized))
                {
                    //if (temp.CeaseInfo.ExecuteTime > DateTime.Now)
                    //此处逻辑错误 modified by zhouhui
                    if (temp.CeaseInfo.ExecuteTime < DateTime.Now)
                        temp.SetStateToCeased();
                }
            }
        }

        private static string CheckCanCancelGroup(Order[] selectedOrders)
        {
            // 只检查是否都是新增医嘱
            if (CheckStateIsSame(selectedOrders) == OrderState.New)
                return "";

            return FormatProcessErrorMessage(ConstNames.OpCancelGroup, ConstMessages.CheckCancelGroup);
        }

        private string CheckCanBeAudited(OrderTable currentTable, Order[] selectedOrders)
        {
            EditProcessFlag flag = CalcEditProcessFlag(currentTable, selectedOrders);
            if ((flag & EditProcessFlag.Audit) > 0)
                return "";

            StringBuilder msgs = new StringBuilder();
            msgs.AppendLine(ConstMessages.CheckAllIsNew);
            msgs.AppendLine(ConstMessages.CheckAudit);

            return FormatProcessErrorMessage(ConstNames.OpAudit, msgs.ToString());
        }

        private string CheckCanBeCancelled(OrderTable currentTable, Order[] selectedOrders)
        {
            EditProcessFlag flag = CalcEditProcessFlag(currentTable, selectedOrders);
            if ((flag & EditProcessFlag.Cancel) > 0)
                return "";

            StringBuilder msgs = new StringBuilder();
            msgs.AppendLine(ConstMessages.CheckAllIsAudited);
            msgs.AppendLine(ConstMessages.CheckCancel);

            return FormatProcessErrorMessage(ConstNames.OpCancelGroup, msgs.ToString());
        }

        private string CheckCanSetCeaseTime(OrderTable currentTable, Order[] selectedOrders, DateTime ceaseTime)
        {
            StringBuilder msgs = new StringBuilder();
            EditProcessFlag flag = CalcEditProcessFlag(currentTable, selectedOrders);
            if ((flag & EditProcessFlag.Cease) > 0)
            {
                if (ceaseTime < DateTime.Now)
                    msgs.AppendLine(ConstMessages.CheckCeaseDateBeforeNow);
                else
                {
                    foreach (Order order in selectedOrders)
                        if (order.StartDateTime >= ceaseTime)
                        {
                            msgs.AppendLine(ConstMessages.CheckCeaseDateBeforeStartDate);
                            break;
                        }
                    return "";
                }
            }
            else
                msgs.AppendLine(ConstMessages.CheckCeaseTimeIsNull);

            return FormatProcessErrorMessage(ConstNames.OpCease, msgs.ToString());
        }

        private static void CheckCanInsertOrdersToGroup(OrderTable currentTable, Order focusedOrder, Order[] insertOrders)
        {
            if ((focusedOrder.GroupPosFlag == GroupPositionKind.GroupMiddle) || (focusedOrder.GroupPosFlag == GroupPositionKind.GroupEnd))
            {
                // 只允许使用直接添加的方式向组中插入新医嘱，粘贴等情况下都不允许
                if ((insertOrders == null) || (insertOrders.Length == 0))
                    throw new DataCheckException(ConstMessages.CheckInsertRowInGroup, ConstMessages.ExceptionTitleOrderTable);
                if ((!CheckCommonPropertiesIsSame(new Order[] { focusedOrder, insertOrders[0] }))
                   || (!CheckCommonPropertiesIsSame(insertOrders)))
                    throw new DataCheckException(ConstMessages.CheckPropertyInGroup, ConstMessages.ExceptionTitleOrderTable);
                AttributeOfSelectedFlag flag = GetAttributeOfSelectedOrder(currentTable, insertOrders);
                if ((flag & AttributeOfSelectedFlag.AllIsHerbDruggery) > 0)
                {
                    if (focusedOrder.Content.Item.Kind != ItemKind.HerbalMedicine)
                        throw new DataCheckException(ConstMessages.CheckNotAllowInsertFerbInGroup, ConstMessages.ExceptionTitleOrderTable);
                }
                else if ((flag & AttributeOfSelectedFlag.AllIsOtherDruggery) > 0)
                {
                    if (focusedOrder.Content.Item.Kind == ItemKind.HerbalMedicine)
                        throw new DataCheckException(ConstMessages.CheckOnlyAllowInsertFerbInGroup, ConstMessages.ExceptionTitleOrderTable);
                }
                else
                    throw new DataCheckException(ConstMessages.CheckOnlyAllowDruggeryInGroup, ConstMessages.ExceptionTitleOrderTable);
            }
        }
        #endregion

        #region private methods of process logic
        private static void DoDeleteNewOrder(OrderTable table, Order[] selectedOrders)
        {
            bool inGroup = false;
            foreach (Order order in selectedOrders)
            {
                if (order == null)
                    throw new IndexOutOfRangeException(ConstMessages.ExceptionOrderNotFind);

                if (order.GroupPosFlag != GroupPositionKind.SingleOrder)
                    inGroup = true;

                table.RemoveOrder(order);
            }

            // 如果删除了已分组的医嘱，则重新设置分组
            if (inGroup)
                DoReformingGroup(table);
        }

        private static void DoSetGroup(OrderTable currentTable, Order[] selectedOrders)
        {
            Order order;
            decimal groupSerialNo = -1;
            int endIndex = selectedOrders.Length - 1;
            for (int index = 0; index <= endIndex; index++)
            {
                order = selectedOrders[index];
                if (order == null)
                    throw new IndexOutOfRangeException(ConstMessages.ExceptionOrderNotFind);
                if (index == 0)
                {
                    order.GroupPosFlag = GroupPositionKind.GroupStart;
                    groupSerialNo = order.SerialNo;
                }
                else if (index == endIndex)
                    order.GroupPosFlag = GroupPositionKind.GroupEnd;
                else
                    order.GroupPosFlag = GroupPositionKind.GroupMiddle;
                // 分组序号和本组的第一条保持一致
                order.GroupSerialNo = groupSerialNo;
            }
            // 如果是对草药进行分组，则要自动插入一条明细数据
            if (selectedOrders[endIndex].Content.Item.Kind == ItemKind.HerbalMedicine)
                HandleHerbSummary(currentTable, selectedOrders[endIndex], true);
        }

        private static void DoCancelGroup(OrderTable table, Order[] selectedOrders)
        {
            // 如果选中记录属于分组，则将其改为单条记录，最后重新组织分组
            foreach (Order order in selectedOrders)
            {
                if (order == null)
                    throw new IndexOutOfRangeException(ConstMessages.ExceptionOrderNotFind);
                if (order.GroupPosFlag != GroupPositionKind.SingleOrder)
                {
                    order.GroupPosFlag = GroupPositionKind.SingleOrder;
                    order.GroupSerialNo = order.SerialNo;
                }
            }
            DoReformingGroup(table);
        }

        private static void DoReformingGroup(OrderTable table)
        {
            // 因为删除等操作，可能破坏了原有的分组情况，此时尽可能按照原有分组情况进行重新分组
            //OrderTableView currentView = table.DefaultView;

            //// 这种操作只会发生在新医嘱的处理中
            //if ((currentView.State != OrderState.All)
            //   && (currentView.State != OrderState.New))
            //   return;

            // 如果组头或组尾的记录被删除，则根据情况重新设置头、尾
            // 按照分组标记重新更新分组序号
            // 需要处理草药汇总信息：
            //    删除记录或取消分组时可能会破坏草药明细和汇总信息的对应关系
            //    遇到草药汇总信息要判断和前一条记录是否匹配
            //    重新成组时，要根据情况插入汇总信息
            int endHandle = table.Orders.Count - 1;// currentView.Count - 1;
            decimal serialOld = 0;
            decimal serialNew = 0;
            Collection<Order> needDelSummaries = new Collection<Order>(); // 等待删除的草药汇总信息
            Collection<Order> needInsertedSummaries = new Collection<Order>(); // 等待插入草药汇总信息的分组记录最后一条记录
            TextOrderContent textContent;
            Order temp;
            Order next;
            for (int index = 0; index < table.Orders.Count; index++)
            {
                temp = table.Orders[index];//temp = currentView[index].OrderCache;
                if (temp.State != OrderState.New)
                    continue;

                // 不是新增医嘱，或没有加入组，则不用处理
                if ((temp.State != OrderState.New)
                   || (temp.GroupPosFlag == GroupPositionKind.SingleOrder))
                    continue;

                // 先检查汇总信息
                textContent = temp.Content as TextOrderContent;
                if ((textContent != null) && (textContent.IsSummaryOfHerbDetail))
                {
                    if ((index == 0)
                       || (table.Orders[index - 1].GroupSerialNo != textContent.GroupSerialNoOfLinkedHerbs)
                       || (table.Orders[index - 1].GroupPosFlag == GroupPositionKind.SingleOrder)
                       || (table.Orders[index - 1].Content.Item == null)
                       || (table.Orders[index - 1].Content.Item.Kind != ItemKind.HerbalMedicine))
                        needDelSummaries.Add(temp);
                    continue; // 
                }

                // 重新分组时，需要根据前后记录的情况进行处理
                if (index < endHandle)
                    next = table.Orders[index + 1]; // currentView[index + 1].OrderCache;
                else
                    next = null;

                // 以下处理如果遇到分组结束，则判断是否需要插入汇总信息
                if (temp.GroupPosFlag == GroupPositionKind.GroupStart)
                {
                    // 本条是头，检查后面是否有同组的记录，没有则改为单条
                    if ((next == null) || (next.GroupSerialNo != temp.GroupSerialNo))
                    {
                        temp.GroupPosFlag = GroupPositionKind.SingleOrder;
                        serialOld = -1;
                    }
                    else
                    {
                        serialOld = temp.GroupSerialNo;
                    }
                    serialNew = serialOld;
                }
                else if (temp.GroupPosFlag == GroupPositionKind.GroupEnd)
                {
                    // 本条是尾，检查前面是否有同组的记录，没有则改为单条
                    if (temp.GroupSerialNo != serialOld) // 与前一条分组序号不一致，表示是不同组
                    {
                        temp.GroupPosFlag = GroupPositionKind.SingleOrder;
                        serialOld = -1;
                        serialNew = temp.SerialNo;
                    }
                    else if (temp.GroupSerialNo != serialNew) // 分组序号与新分组序号不一致，表示要改变分组
                    {
                        needInsertedSummaries.Add(temp);
                    }
                    // 重设结束记录的分组序号是必须的(因为本组可能只剩下自己，或者组头被删掉，由中间的记录作为组头)
                    temp.GroupSerialNo = serialNew;
                }
                else if (temp.GroupPosFlag == GroupPositionKind.GroupMiddle)
                {
                    // 本条是中间，检查前后是否有同组的记录
                    // ，前面没有则改为头，后面没有则改为尾，都没有则改为单条
                    if (temp.GroupSerialNo != serialOld) // 前面没有同组的记录
                    {
                        temp.GroupPosFlag = GroupPositionKind.GroupStart;
                        serialOld = temp.GroupSerialNo;
                        serialNew = temp.SerialNo;
                    }
                    temp.GroupSerialNo = serialNew;

                    if ((next == null) || (next.GroupSerialNo != serialOld)) // 后面没有同组的
                    {
                        if (temp.GroupPosFlag == GroupPositionKind.GroupStart)
                            temp.GroupPosFlag = GroupPositionKind.SingleOrder;
                        else
                        {
                            temp.GroupPosFlag = GroupPositionKind.GroupEnd;
                            needInsertedSummaries.Add(temp);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 处理草药汇总信息
        /// </summary>
        /// <param name="currentTable"></param>
        /// <param name="herbDetail">关联的医嘱明细记录(必须传分组的最后一条记录)</param>
        /// <param name="addSummary">true:添加汇总, false:删除汇总</param>
        private static void HandleHerbSummary(OrderTable currentTable, Order herbDetail, bool addSummary)
        {
            int index = currentTable.Orders.IndexOf(herbDetail.SerialNo);
            if (addSummary)
            {
                Order newSummary = currentTable.NewOrder();
                newSummary.BeginInit();

                newSummary.PatientId = herbDetail.PatientId;

                newSummary.OriginalWard = new Eop.Ward(herbDetail.OriginalWard.Code);
                newSummary.OriginalDepartment = new Eop.Department(herbDetail.OriginalDepartment.Code);

                if (newSummary.CreateInfo == null)
                    newSummary.CreateInfo = new OrderOperateInfo();
                newSummary.CreateInfo.SetPropertyValue(herbDetail.CreateInfo.Executor.Code, herbDetail.CreateInfo.ExecuteTime);

                newSummary.StartDateTime = herbDetail.StartDateTime;
                newSummary.Content = new TextOrderContent();
                // 
                newSummary.Content.EntrustContent = String.Format(Order.HerbSummaryFormat, 1, herbDetail.Content.ItemUsage, herbDetail.Content.ItemFrequency);
                newSummary.Memo = Order.HerbSummaryFlag + herbDetail.GroupSerialNo.ToString();
                newSummary.EndInit();
                // 将汇总记录插在最后一条明细后面
                currentTable.InsertOrderAt(newSummary, index + 1);
                //add by zhouhui 分组信息要像新增的普通医嘱一样，需要重新处理其输出信息
                newSummary.Content.ProcessCreateOutputeInfo = new OrderContent.GenerateOutputInfo(CustomDrawOperation.CreateOutputeInfo);
            }
            else
            {
                TextOrderContent textContent = currentTable.Orders[index + 1].Content as TextOrderContent;
                if ((textContent != null) && (textContent.IsSummaryOfHerbDetail))
                    currentTable.RemoveOrderAt(index + 1);
            }
        }

        private void DoAutoCeaseLongOrder(string ceasor, DateTime ceaseTime, OrderCeaseReason ceaseReason)
        {
            if (ceaseTime == DateTime.MinValue)
                throw new DataCheckException(String.Format(ConstMessages.ExceptionFormatNoValue, ConstNames.TimeOfCease), OrderView.UNCeaseDate);

            foreach (LongOrder order in m_LongTable.Orders)
            {
                if ((order.State == OrderState.Audited) || (order.State == OrderState.Executed))
                {
                    // --只在未赋过停止时间或当前停止时间大于自动停的时间的情况下更新
                    // --(对于已有停止信息的医嘱不应该修改停止时间，应该在医嘱执行的时候直接修改状态！！！)
                    // 如果已有停止时间，且停止时间晚于转科执行时间，则在现在的医嘱处理模式下不能将这些医嘱停到转科时间
                    // 所以对这些医嘱的停止时间也要修改（会造成数据库中的停止时间和已打印的时间不一致）
                    if ((order.CeaseInfo == null) || (!order.CeaseInfo.HadInitialized)
                       || (order.CeaseInfo.ExecuteTime > ceaseTime))
                    {
                        if ((order.CeaseInfo != null) && (order.CeaseInfo.HadInitialized))
                            order.Memo = order.CeaseInfo.ToString(); // 将原始的停止信息保存在Memo中
                        order.CeaseOrder(ceasor, ceaseTime, ceaseReason);
                    }
                }
            }
        }

        private void DoCeaseGeneralOrder(ItemKind generalKind, string ceasor, DateTime ceaseTime)
        {
            LongOrder order;
            for (int index = m_LongTable.Orders.Count - 1; index >= 0; index--)
            {
                order = m_LongTable.Orders[index] as LongOrder;
                if ((order.State == OrderState.Executed)
                   && (order.Content.OrderKind == OrderContentKind.GeneralItem)
                   && (order.Content.Item.Kind == generalKind))
                {
                    order.CeaseOrder(ceasor, ceaseTime, OrderCeaseReason.NewGeneral);
                    break; // 相同类型的常规医嘱，最多只会有一条处于已执行状态
                }
            }
        }

        private void DoAuditTempOrder(OrderTable currentTable, Order[] selectedTemps, string auditor, DateTime auditTime)
        {
            TempOrder temp;
            TextOrderContent textContent;
            Collection<decimal> linkHerbDetails = new Collection<decimal>(); // 关联的草药明细的分组序号
            foreach (Order order in selectedTemps)
            {
                temp = order as TempOrder;
                if (temp == null)
                    throw new IndexOutOfRangeException(ConstMessages.ExceptionOrderNotFind);
                if (order.State != OrderState.New) // 只能审核状态为新增的医嘱
                    continue;

                switch (order.Content.OrderKind)
                {
                    // 如果是文字医嘱，则直接将状态置为已执行
                    case OrderContentKind.TextNormal:
                    case OrderContentKind.TextShiftDept:
                    case OrderContentKind.TextLeaveHospital:
                    case OrderContentKind.TextAfterOperation:
                        temp.ExecuteOrder(auditor, auditTime);
                        break;
                    default:
                        // 设置医嘱的审核时间、审核人，并更新状态
                        temp.AuditOrder(auditor, auditTime);
                        break;
                }
                // 如果是草药汇总信息，则同时审核相关的明细记录(目前HIS的医嘱中不保存草药明细)
                textContent = temp.Content as TextOrderContent;
                if ((textContent != null) && (textContent.IsSummaryOfHerbDetail))
                    linkHerbDetails.Add(textContent.GroupSerialNoOfLinkedHerbs);
            }

            DoAutoAuditHerbDetailOrders(currentTable, linkHerbDetails, auditor, auditTime);
        }

        private void DoAuditLongOrder(OrderTable currentTable, Order[] selectedLongs, string auditor, DateTime auditTime)
        {
            // 1 更新相关字段
            DateTime autoCeaseTime = DateTime.MinValue;
            OrderCeaseReason ceaseReason = OrderCeaseReason.None;

            LongOrder temp;
            Order tendOrder = null; // 护理级别
            Order dangerOrder = null; // 危重级别
            TextOrderContent textContent;
            Collection<decimal> linkHerbDetails = new Collection<decimal>(); // 关联的草药明细的分组序号
            foreach (Order order in selectedLongs)
            {
                temp = order as LongOrder;
                if (temp == null)
                    throw new IndexOutOfRangeException(ConstMessages.ExceptionOrderNotFind);

                // 设置医嘱的审核时间、审核人，并更新状态
                if (order.State == OrderState.New) // 新增
                    temp.AuditOrder(auditor, auditTime);
                else if (order.State == OrderState.Executed) // 已执行状态下审核应该是审核停止信息
                    temp.AuditCeaseOrder(auditor, auditTime);
                else
                    continue;

                // 停相应的常规医嘱
                if (temp.Content.OrderKind == OrderContentKind.GeneralItem)
                {
                    DoCeaseGeneralOrder(temp.Content.Item.Kind, auditor, temp.StartDateTime);
                    if (temp.Content.Item.Kind == ItemKind.DangerLevel)
                        dangerOrder = temp;
                    else if (temp.Content.Item.Kind == ItemKind.Care)
                        tendOrder = temp;
                }
                // 术后、产后要停有效的长期医嘱
                else if (temp.Content.OrderKind == OrderContentKind.TextAfterOperation)
                {
                    // 长期医嘱停到术后的开始时间
                    autoCeaseTime = temp.StartDateTime;
                    ceaseReason = OrderCeaseReason.AfterOperation;
                }
                else
                {
                    // 如果是草药汇总信息，则同时审核相关的明细记录(目前HIS的医嘱中不保存草药明细)
                    textContent = temp.Content as TextOrderContent;
                    if ((textContent != null) && (textContent.IsSummaryOfHerbDetail))
                        linkHerbDetails.Add(textContent.GroupSerialNoOfLinkedHerbs);
                }
            }
            // 2 停长期医嘱
            // 术后、产后要停有效的长期医嘱(已审核的医嘱，也要)
            // 检查医嘱状态是否可设为已停止
            // 检查已审核的医嘱的停止时间是否超过出院或转科的时间
            if (autoCeaseTime > DateTime.MinValue)
                DoAutoCeaseLongOrder(auditor, autoCeaseTime, ceaseReason);

            DoAutoAuditHerbDetailOrders(currentTable, linkHerbDetails, auditor, auditTime);

            // 4 执行额外的自动处理任务
            if ((dangerOrder != null) || (tendOrder != null))
                SetCurrentPatientDangerAndTendLevel(dangerOrder, tendOrder);
        }

        private void DoAutoAuditHerbDetailOrders(OrderTable currentTable, Collection<decimal> linkHerbDetails, string auditor, DateTime auditTime)
        {
            if (linkHerbDetails.Count > 0)
            {
                for (int index = currentTable.Orders.Count - 1; index >= 0; index--)
                {
                    if ((currentTable.Orders[index].State == OrderState.Executed)
                       || (currentTable.Orders[index].State == OrderState.Ceased))
                        break;
                    if (currentTable.Orders[index].State != OrderState.New)
                        continue;

                    if (linkHerbDetails.Contains(currentTable.Orders[index].GroupSerialNo))
                        currentTable.Orders[index].AuditOrder(auditor, auditTime);
                }
            }
        }

        private void SetCurrentPatientDangerAndTendLevel(Order dangerOrder, Order tendOrder)
        {
            string updateCmd = null;
            ItemBase item;
            if (dangerOrder != null) // 危重级别改变时要发送相应消息
            {

                item = dangerOrder.Content.Item;
                updateCmd = ConstSchemaNames.InpatientColDangerLevel + " = '" + item.Memo.Trim() + "'"; // 备注中存放的是对应的危重级别代码

                if (CurrentPatient.PatientCondition == null)
                    CurrentPatient.ReInitializeProperties();
                CurrentPatient.PatientCondition.Code = item.Memo.Trim();
                CurrentPatient.PatientCondition.Name = item.Name;
            }

            if (tendOrder != null)
            {
                item = tendOrder.Content.Item;
                if (String.IsNullOrEmpty(updateCmd))
                    updateCmd = ConstSchemaNames.InpatientColCareLevel + " = '" + item.Code.Trim() + "'";
                else
                    updateCmd += " ," + ConstSchemaNames.InpatientColCareLevel + " = '" + item.Code.Trim() + "'";

                if (CurrentPatient.PatientCondition == null)
                    CurrentPatient.ReInitializeProperties();
                CurrentPatient.InfoOfAdmission.CareLevel.Code = item.Code;
                CurrentPatient.InfoOfAdmission.CareLevel.Name = item.Name;
            }

            if (!String.IsNullOrEmpty(updateCmd))
            {
                // 先更新EMR中病人的危重和护理级别
                m_SqlExecutor.ExecuteNoneQuery(String.Format(CultureInfo.CurrentCulture
                   , ConstSqlSentences.FormatUpdateInpatient, updateCmd, CurrentPatient.NoOfFirstPage));

                // TODO再更新HIS中病人的危重和护理级别
                //DataTable changedPatient = m_SqlExecutor.ExecuteDataTable(
                //   String.Format(CultureInfo.CurrentCulture, ConstSqlSentences.FormatSelectInpatient, CurrentPatient.NoOfFirstPage));
                //MemoryStream source = new MemoryStream();
                //changedPatient.WriteXml(source, XmlWriteMode.WriteSchema);
                //source.Seek(0, SeekOrigin.Begin);
                //byte[] data = new byte[source.Length];
                //source.Read(data, 0, (int)source.Length);

                //string[,] parameters = new string[3, 1];
                //parameters[0, 0] = "changeData";
                //parameters[1, 0] = "System.Byte[]";
                //parameters[2, 0] = Convert.ToBase64String(data);

                //string sExio = m_InfoServer.BuildExchangeInfoString(ExchangeInfoOrderConst.MsgUpdatePatient, ExchangeInfoOrderConst.EmrSystemName
                //   , ExchangeInfoOrderConst.HisSystemName,
                //    parameters);

                //string outMsg;
                //if (m_InfoServer.AddSyncExchangeInfo(sExio, out outMsg) != ResponseFlag.Complete)
                //    throw new ApplicationException(outMsg);
            }
        }

        private void DoCancelOrder(OrderTable currentTable, Order[] selectedOrders, string cancellor, DateTime cancelTime)
        {
            if (cancelTime == DateTime.MinValue)
                throw new DataCheckException(String.Format(ConstMessages.ExceptionFormatNoValue, ConstNames.TimeOfCancel), ConstNames.TimeOfCancel);

            TextOrderContent textContent;
            Collection<decimal> linkHerbDetails = new Collection<decimal>(); // 关联的草药明细的分组序号

            foreach (Order order in selectedOrders)
            {
                if (order.State == OrderState.Audited)
                    order.CancelOrder(cancellor, cancelTime);

                // 如果是草药汇总信息，则同时审核相关的明细记录(目前HIS的医嘱中不保存草药明细)
                textContent = order.Content as TextOrderContent;
                if ((textContent != null) && (textContent.IsSummaryOfHerbDetail))
                    linkHerbDetails.Add(textContent.GroupSerialNoOfLinkedHerbs);
            }

            if (linkHerbDetails.Count > 0)
            {
                for (int index = currentTable.Orders.Count - 1; index >= 0; index--)
                {
                    if (currentTable.Orders[index].State != OrderState.Audited)
                        continue;
                    else if (linkHerbDetails.Contains(currentTable.Orders[index].GroupSerialNo))
                        currentTable.Orders[index].CancelOrder(cancellor, cancelTime);
                }
            }
        }

        private void DoSetLongOrderCeaseInfo(Order[] selectedOrders, string ceasor, DateTime ceaseTime)
        {
            LongOrder longOrder;
            foreach (Order order in selectedOrders)
            {
                longOrder = order as LongOrder;
                //modified by zhouhui 如果当前的长嘱没有审核之前是允许其修改停止时间的
                //TODO:?待处理因允许修改停止时间而带来的医嘱打印问题
                //if ((longOrder != null) && ((longOrder.CeaseInfo == null) || (!longOrder.CeaseInfo.HadInitialized)))
                if ((longOrder != null) && (longOrder.State != OrderState.Ceased))
                {
                    if (ceaseTime < longOrder.StartDateTime)
                        continue;
                    longOrder.CeaseOrder(ceasor, ceaseTime, OrderCeaseReason.Natural);
                    // 同组的一起停止
                    if (longOrder.GroupPosFlag != GroupPositionKind.SingleOrder)
                    {
                        Collection<Order> groupOrders = m_LongTable.GetOtherOrdersOfSameGroup(order);
                        foreach (Order gpOrder in groupOrders)
                        {
                            longOrder = gpOrder as LongOrder;
                            if ((longOrder != null) && ((longOrder.CeaseInfo == null) || (!longOrder.CeaseInfo.HadInitialized)))
                                longOrder.CeaseOrder(ceasor, ceaseTime, OrderCeaseReason.Natural);
                        }
                    }
                }
            }
        }

        private void DoSaveChangedData(OrderTable currentTable, Order[] changedOrders, string executorCode, string macAddress, bool callByEditor, bool forceSend, bool autoDeleteNewOrder)
        {
            //return DoSaveChangedData(currentTable, changedOrders, executorCode, macAddress, callByEditor, forceSend, autoDeleteNewOrder, false, null, DateTime.MinValue);

            if ((changedOrders == null) || (changedOrders.Length == 0))
                return;

            if (callByEditor) // 在医嘱编辑器中调用时才更新创建信息
            {
                DateTime createTime = DateTime.Now;
                foreach (Order order in changedOrders)
                {
                    if ((order.State == OrderState.New) && (order.EditState != OrderEditState.Deleted))
                        order.CreateInfo.ExecuteTime = createTime;
                }
            }
            DataTable changedTable = currentTable.SyncObjectData2Table(changedOrders, autoDeleteNewOrder);

            DataTable currentChangedTable;
            try
            {
                currentChangedTable = DoSaveDataToEmr(currentTable, changedTable, autoDeleteNewOrder);
            }
            catch (Exception e)
            {
                currentTable.OrderDataTable.RejectChanges(); // 取消对DataTable所做的修改
                throw e;
            }

            // 本地保存成功后再设置内存中数据的改变标志
            currentTable.AcceptChanges();

            if (autoDeleteNewOrder) // 只在主动删除新医嘱时强制刷新新医嘱的医嘱序号，否则会有问题(因为不知道如何对应新增医嘱的序号)
                SynchSerialNoOfNewOrder(currentTable, currentChangedTable);

            if (forceSend || ((CoreBusinessLogic.BusinessLogic.AutoSyncData) && (currentChangedTable != null)))
            {
                SendOrderDataToHis(currentTable, currentChangedTable, executorCode, macAddress);
            }
        }

        //private DataTable DoSaveChangedData(OrderTable currentTable, Order[] changedOrders, string executorCode, string macAddress, bool callByEditor, bool forceSend, bool autoDeleteNewOrder, bool saveSendLog, string fireCode, DateTime fireTime)
        //{
        //   if ((changedOrders == null) || (changedOrders.Length == 0))
        //      return;

        //   if (callByEditor) // 在医嘱编辑器中调用时才更新创建信息
        //   {
        //      DateTime createTime = DateTime.Now;
        //      foreach (Order order in changedOrders)
        //      {
        //         if ((order.State == OrderState.New) && (order.EditState != OrderEditState.Deleted))
        //            order.CreateInfo.ExecuteTime = createTime;
        //      }
        //   }
        //   DataTable changedTable = currentTable.SyncObjectData2Table(changedOrders, autoDeleteNewOrder);

        //   DataTable currentChangedTable;
        //   try
        //   {
        //      currentChangedTable = DoSaveDataToEmr(currentTable, changedTable, autoDeleteNewOrder);
        //   }
        //   catch (Exception e)
        //   {
        //      currentTable.OrderDataTable.RejectChanges(); // 取消对DataTable所做的修改
        //      throw e;
        //   }

        //   // 本地保存成功后再设置内存中数据的改变标志
        //   currentTable.AcceptChanges();

        //   if (autoDeleteNewOrder) // 只在主动删除新医嘱时强制刷新新医嘱的医嘱序号，否则会有问题(因为不知道如何对应新增医嘱的序号)
        //      SynchSerialNoOfNewOrder(currentTable, currentChangedTable);

        //   if (forceSend || ((CoreBusinessLogic.BusinessLogic.AutoSyncData) && (currentChangedTable != null)))
        //   {
        //      //在发送到HIS前记录日志
        //      if (saveSendLog)
        //         m_SynchLogHelper.SaveRecord(currentChangedTable, currentTable.IsTempOrder, fireCode, fireTime);

        //      SendOrderDataToHis(currentTable, currentChangedTable, executorCode, macAddress);
        //   }
        //   return currentChangedTable;
        //}

        //yxy
        private DataTable DoSaveDataToEmr(OrderTable currentTable, DataTable changedTable, bool autoDeleteNewOrder)
        {
            if ((changedTable != null) && (changedTable.Rows.Count > 0))
            {
                string tableName = GetOrderTableName(currentTable.IsTempOrder);

                // 如果需要主动删除数据库表中新增状态的医嘱(一般是编辑器调用保存方法才会发生)，则做特殊处理
                if (autoDeleteNewOrder)
                {
                    // 保存前主动删除数据库中新增状态的医嘱
                    //    因为保存后并没有同步医嘱序号，所以再次保存时用内存中的序号去删除数据会产生错误
                    //    另外，在编辑器中编辑新医嘱时，可能会改变原始的顺序，所以要通过重新插入的方式保证新医嘱的相对顺序不会出错

                    // 在做删除操作前，先检查被删除的新医嘱在数据库中的状态
                    //    如果存在且状态为新增，则可以删除再插入
                    //    如果存在且状态不为新增，则不能删除、插入，不能保存，必须刷新数据
                    //    如果有序号不在指定范围内的新增医嘱，说明可能开过申请单，则不能执行删除操作，必须刷新数据
                    OrderState state;
                    string serialNoField = GetSerialNoField(currentTable.IsTempOrder);

                    DataViewRowState oldRowState = changedTable.DefaultView.RowStateFilter;
                    // 组合已删除的医嘱的序号，作为查询条件
                    changedTable.DefaultView.RowStateFilter = DataViewRowState.Deleted;
                    StringBuilder deletedSerials = new StringBuilder("0, ");
                    DataRowView rowView;
                    for (int index = changedTable.DefaultView.Count - 1; index >= 0; index--)
                    {
                        rowView = changedTable.DefaultView[index];
                        state = (OrderState)Convert.ToInt32(rowView[ConstSchemaNames.OrderColState]);
                        if (state == OrderState.New)
                        {
                            deletedSerials.Append(rowView[serialNoField] + ",");
                            changedTable.Rows.Remove(rowView.Row);
                        }
                    }
                    deletedSerials.Append(" 0");

                    //object num = m_SqlExecutor.ExecuteScalar(String.Format(CultureInfo.CurrentCulture
                    //      , ConstSqlSentences.FormatSelectChangedOrderData
                    //      , tableName, CurrentPatient.NoOfFirstPage, OrderState.New, serialNoField, deletedSerials.ToString()));
                    //if (num != null) {
                    //    if (Convert.ToInt32(num) > 0)
                    //        throw new DataCheckException(ConstMessages.CheckOrderStateBeforeSave, ConstMessages.ExceptionTitleOrder);
                    //}

                    changedTable.DefaultView.RowStateFilter = oldRowState;
                    m_SqlExecutor.ExecuteNoneQuery(String.Format(CultureInfo.CurrentCulture
                          , ConstSqlSentences.FormatDeleteNewOrder
                          , tableName, CurrentPatient.NoOfFirstPage, OrderState.New));
                }

                //// 保存前先调用HIS的医嘱检查
                //if (CoreBusinessLogic.BusinessLogic.ConnectToHis)
                //{
                //   // 数据集转换成byte数组
                //   MemoryStream source = new MemoryStream();
                //   changedTable.WriteXml(source, XmlWriteMode.WriteSchema);
                //   source.Seek(0, SeekOrigin.Begin);
                //   byte[] data = new byte[source.Length];
                //   source.Read(data, 0, (int)source.Length);

                //   CallDoctorAdviceService(ExchangeInfoOrderConst.MsgCheckData, data, executorCode, macAddress, currentTable.IsTempOrder);

                //}
                //yxy
                //m_SqlExecutor.UpdateTable(changedTable, tableName, false);
                DoUpdateOrder(changedTable, tableName, false);


                // 手工调用更新分组序号的存储过程
                SqlParameter[] paras = new SqlParameter[]{
               new SqlParameter(ConstSchemaNames.ProcParaFirstpageNo, SqlDbType.Decimal)
               , new SqlParameter(ConstSchemaNames.ProcParaOrderKind, SqlDbType.Int)
               , new SqlParameter(ConstSchemaNames.ProcParaOnlyNew, SqlDbType.Int)
            };
                paras[0].Value = CurrentPatient.NoOfFirstPage;
                if (currentTable.IsTempOrder)
                    paras[1].Value = 0;
                else
                    paras[1].Value = 1;
                paras[2].Value = 1;
                m_SqlExecutor.ExecuteNoneQuery(ConstSchemaNames.ProcUpdateSerialNo, paras, CommandType.StoredProcedure);

                return changedTable;
            }
            else
                return null;
        }

        /// <summary>
        /// 保存修改过的医嘱到Oracle数据库中
        /// </summary>
        /// <param name="changerTable"></param>
        /// <param name="tableName"></param>
        /// <param name="needUpdateSchema"></param>
        private void DoUpdateOrder(DataTable changerTable, string tableName, bool needUpdateSchema)
        {
            if (changerTable == null || changerTable.Rows.Count == 0)
                return;
            if (tableName == "Long_Order")
            {
                DoUpdateLong_Order(changerTable);
            }
            else if (tableName == "Temp_Order")
            {
                DoUpdateTemp_Order(changerTable);
            }
        }

        #region 将医嘱信息保存到数据库中
        /// <summary>
        /// 根据传入的长期医嘱表将长期医嘱信息保存到数据库中
        /// </summary>
        /// <param name="changerTable"></param>
        private void DoUpdateLong_Order(DataTable changerTable)
        {
            if (changerTable == null || changerTable.Rows.Count == 0)
                return;
            string editType;
            foreach (DataRow dr in changerTable.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    editType = "1";

                    SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@LONGID",SqlDbType.Int),
                    new SqlParameter("@NOOFINPAT",SqlDbType.Int),
                    new SqlParameter("@GROUPID",SqlDbType.Int),
                    new SqlParameter("@GROUPFLAG",SqlDbType.Int),
                    new SqlParameter("@WARDID",SqlDbType.VarChar),

                    new SqlParameter("@DEPTID",SqlDbType.VarChar),
                    new SqlParameter("@TYPEDOCTOR",SqlDbType.VarChar),
                    new SqlParameter("@TYPEDATE",SqlDbType.VarChar),
                    new SqlParameter("@AUDITOR",SqlDbType.VarChar),
                    new SqlParameter("@DATEOFAUDIT",SqlDbType.VarChar),

                    new SqlParameter("@EXECUTOR",SqlDbType.VarChar),
                    new SqlParameter("@EXECUTEDATE",SqlDbType.VarChar),
                    new SqlParameter("@CANCELDOCTOR",SqlDbType.VarChar),
                    new SqlParameter("@CANCELDATE",SqlDbType.VarChar),
                    new SqlParameter("@CEASEDCOCTOR",SqlDbType.VarChar),

                    new SqlParameter("@CEASEDATE",SqlDbType.VarChar),
                    new SqlParameter("@CEASENURSE",SqlDbType.VarChar),
                    new SqlParameter("@CEASEADUDITDATE",SqlDbType.VarChar),
                    new SqlParameter("@STARTDATE",SqlDbType.VarChar),
                    new SqlParameter("@TOMORROW",SqlDbType.Int),

                    new SqlParameter("@PRODUCTNO",SqlDbType.Int),
                    new SqlParameter("@NORMNO",SqlDbType.Int),
                    new SqlParameter("@MEDICINENO",SqlDbType.Int),
                    new SqlParameter("@DRUGNO",SqlDbType.VarChar),
                    new SqlParameter("@DRUGNAME",SqlDbType.VarChar),

                    new SqlParameter("@DRUGNORM",SqlDbType.VarChar),
                    new SqlParameter("@ITEMTYPE",SqlDbType.Int),
                    new SqlParameter("@MINUNIT",SqlDbType.VarChar),
                    new SqlParameter("@DRUGDOSE",SqlDbType.Int),
                    new SqlParameter("@DOSEUNIT",SqlDbType.VarChar),

                    new SqlParameter("@UNITRATE",SqlDbType.Int),
                    new SqlParameter("@UNITTYPE",SqlDbType.Int),
                    new SqlParameter("@DRUGUSE",SqlDbType.VarChar),
                    new SqlParameter("@BATCHNO",SqlDbType.VarChar),
                    new SqlParameter("@EXECUTECOUNT",SqlDbType.Int),

                    new SqlParameter("@EXECUTECYCLE",SqlDbType.Int),
                    new SqlParameter("@CYCLEUNIT",SqlDbType.Int),
                    new SqlParameter("@DATEOFWEEK",SqlDbType.VarChar),
                    new SqlParameter("@INNEREXECUTETIME",SqlDbType.VarChar),
                    new SqlParameter("@EXECUTEDEPT",SqlDbType.VarChar),

                    new SqlParameter("@ENTRUST",SqlDbType.VarChar),
                    new SqlParameter("@ORDERTYPE",SqlDbType.Int),
                    new SqlParameter("@ORDERSTATUS",SqlDbType.Int),
                    new SqlParameter("@SPECIALMARK",SqlDbType.Int),
                    new SqlParameter("@CEASEREASON",SqlDbType.Int),

                    new SqlParameter("@CURGERYID",SqlDbType.Int),
                    new SqlParameter("@CONTENT",SqlDbType.VarChar),
                    new SqlParameter("@SYNCH",SqlDbType.Int),
                    new SqlParameter("@MEMO",SqlDbType.VarChar),
                    new SqlParameter("@DJFL",SqlDbType.VarChar)
 
                };

                    sqlParam[0].Value = editType;
                    for (int i = 0; i < changerTable.Columns.Count; i++)
                    {
                        sqlParam[i + 1].Value = dr[i];
                    }

                    try
                    {
                        m_SqlExecutor.ExecuteDataSet("usp_EditEmrLONG_ORDER", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
                    }
                    catch (Exception e)
                    {
                        ;
                    }
                }
            }
        }

        private void DoUpdateTemp_Order(DataTable changerTable)
        {
            if (changerTable == null || changerTable.Rows.Count == 0)
                return;
            string editType;
            foreach (DataRow dr in changerTable.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    editType = "1";

                    SqlParameter[] sqlParam = new SqlParameter[] 
                {
                    new SqlParameter("@EditType",SqlDbType.VarChar),
                    new SqlParameter("@TEMPID",SqlDbType.Int),
                    new SqlParameter("@NOOFINPAT",SqlDbType.Int),
                    new SqlParameter("@GROUPID",SqlDbType.Int),
                    new SqlParameter("@GROUPFLAG",SqlDbType.Int),
                    new SqlParameter("@WARDID",SqlDbType.VarChar),

                    new SqlParameter("@DEPTID",SqlDbType.VarChar),
                    new SqlParameter("@TYPEDOCTOR",SqlDbType.VarChar),
                    new SqlParameter("@TYPEDATE",SqlDbType.VarChar),
                    new SqlParameter("@AUDITOR",SqlDbType.VarChar),
                    new SqlParameter("@DATEOFAUDIT",SqlDbType.VarChar),

                    new SqlParameter("@EXECUTOR",SqlDbType.VarChar),
                    new SqlParameter("@EXECUTEDATE",SqlDbType.VarChar),
                    new SqlParameter("@CANCELDOCTOR",SqlDbType.VarChar),
                    new SqlParameter("@CANCELDATE",SqlDbType.VarChar),
                    new SqlParameter("@STARTDATE",SqlDbType.VarChar),

                    new SqlParameter("@PRODUCTNO",SqlDbType.Int),
                    new SqlParameter("@NORMNO",SqlDbType.Int),
                    new SqlParameter("@MEDICINENO",SqlDbType.Int),
                    new SqlParameter("@DRUGNO",SqlDbType.VarChar),
                    new SqlParameter("@DRUGNAME",SqlDbType.VarChar),

                    new SqlParameter("@DRUGNORM",SqlDbType.VarChar),
                    new SqlParameter("@ITEMTYPE",SqlDbType.Int),
                    new SqlParameter("@MINUNIT",SqlDbType.VarChar),
                    new SqlParameter("@DRUGDOSE",SqlDbType.Int),
                    new SqlParameter("@DOSEUNIT",SqlDbType.VarChar),

                    new SqlParameter("@UNITRATE",SqlDbType.Int),
                    new SqlParameter("@UNITTYPE",SqlDbType.Int),
                    new SqlParameter("@DRUGUSE",SqlDbType.VarChar),
                    new SqlParameter("@BATCHNO",SqlDbType.VarChar),
                    new SqlParameter("@EXECUTECOUNT",SqlDbType.Int),

                    new SqlParameter("@EXECUTECYCLE",SqlDbType.Int),
                    new SqlParameter("@CYCLEUNIT",SqlDbType.Int),
                    new SqlParameter("@DATEOFWEEK",SqlDbType.VarChar),
                    new SqlParameter("@INNEREXECUTETIME",SqlDbType.VarChar),
                    new SqlParameter("@ZXTS",SqlDbType.Int),

                    new SqlParameter("@TOTALDOSE",SqlDbType.Int),
                    new SqlParameter("@ENTRUST",SqlDbType.VarChar),
                    new SqlParameter("@ORDERTYPE",SqlDbType.Int),
                    new SqlParameter("@ORDERSTATUS",SqlDbType.Int),
                    new SqlParameter("@SPECIALMARK",SqlDbType.Int),

                    new SqlParameter("@CEASEID",SqlDbType.Int),
                    new SqlParameter("@CEASEDATE",SqlDbType.VarChar),
                    new SqlParameter("@CONTENT",SqlDbType.VarChar),
                    new SqlParameter("@SYNCH",SqlDbType.Int),
                    new SqlParameter("@MEMO",SqlDbType.VarChar),

                    new SqlParameter("@FORMTYPE",SqlDbType.VarChar)
 
                };

                    sqlParam[0].Value = editType;
                    for (int i = 0; i < changerTable.Columns.Count; i++)
                    {
                        sqlParam[i + 1].Value = dr[i];
                    }

                    try
                    {
                        m_SqlExecutor.ExecuteDataSet("usp_EditEmrTEMP_ORDER", sqlParam, CommandType.StoredProcedure).Tables[0].Rows[0][0].ToString();
                    }
                    catch (Exception e)
                    {
                        ;
                    }
                }
            }
        }
        #endregion


        private void DoManualSynchDataToHis(OrderTable currentTable, string executorCode, string macAddress)
        {
            // 取出同步标志为0的数据，保存到HIS
            DataTable changedData = m_SqlExecutor.ExecuteDataTable(String.Format(CultureInfo.CurrentCulture
                     , ConstSqlSentences.FormatSelectNotSynchedOrderData
                     , GetOrderTableName(currentTable.IsTempOrder)
                     , CurrentPatient.NoOfFirstPage
                     , GetSerialNoField(currentTable.IsTempOrder)));
            // 不判断变化的记录数，因为可能没有改过医嘱，只是删除了新医嘱，此时从EMR得到的变更数据集是空的
            //if (changedData.Rows.Count > 0)
            //{
            DataTable table;
            if (currentTable.IsTempOrder)
                table = TableOfSynchTempOrder;
            else
                table = TableOfSynchLongOrder;
            table.Clear();
            table.Merge(changedData);
            SendOrderDataToHis(currentTable, table, executorCode, macAddress);
            //}
        }

        private void SendOrderDataToHis(OrderTable currentTable, DataTable changedTable, string executorCode, string macAddress)
        {
            if (CoreBusinessLogic.BusinessLogic.ConnectToHis)
            {
                // 数据集转换成byte数组
                MemoryStream source = new MemoryStream();
                changedTable.WriteXml(source, XmlWriteMode.WriteSchema);
                source.Seek(0, SeekOrigin.Begin);
                byte[] data = new byte[source.Length];
                source.Read(data, 0, (int)source.Length);

                // 3 调用接口检查数据
                CallDoctorAdviceService(ExchangeInfoOrderConst.MsgCheckData, data, executorCode, macAddress, currentTable.IsTempOrder);

                // 4 调用接口同步数据
                CallDoctorAdviceService(ExchangeInfoOrderConst.MsgSaveData, data, executorCode, macAddress, currentTable.IsTempOrder);

                // 同步成功，则更新同步标志
                UpdateSynchFlagToTrue(currentTable, changedTable);

                currentTable.AcceptChanges();
            }
        }

        private void SynchSerialNoOfNewOrder(OrderTable currentTable, DataTable changedTable)
        {
            string oldRowFilter = changedTable.DefaultView.RowFilter;

            changedTable.DefaultView.RowFilter = String.Format("{1} = {0:D}", OrderState.New, ConstSchemaNames.OrderColState);// .RowStateFilter = DataViewRowState.CurrentRows;
            if (changedTable.DefaultView.Count == 0)
                return;

            // 保存后同步新医嘱的序号和分组序号
            string serialNoField = GetSerialNoField(currentTable.IsTempOrder);
            IDataReader serialNoReader = m_SqlExecutor.ExecuteReader(
               String.Format(CultureInfo.CurrentCulture
                  , ConstSqlSentences.FormatSelectSerialNosOfNewSynchedOrder
                  , serialNoField, GetOrderTableName(currentTable.IsTempOrder)
                  , CurrentPatient.NoOfFirstPage, OrderState.New));

            List<decimal> trueSerialNos = new List<decimal>();
            List<decimal> trueGroupNos = new List<decimal>();
            while (serialNoReader.Read())
            {
                trueSerialNos.Add(Convert.ToDecimal(serialNoReader[serialNoField]));
                trueGroupNos.Add(Convert.ToDecimal(serialNoReader[ConstSchemaNames.OrderColGroupSerialNo]));
            }
            serialNoReader.Close();

            if (trueSerialNos.Count != changedTable.DefaultView.Count)
                throw new DataCheckException(ConstMessages.CheckNumberOfSynchedOrder, "");

            // OrderTable、数据表和changedTable都要更新（倒序更新）
            DataRow[] newRows = currentTable.OrderDataTable.Select(
               String.Format(CultureInfo.CurrentCulture, "{1} = {0:D}", OrderState.New, ConstSchemaNames.OrderColState));
            int changedIndex = changedTable.Rows.Count - 1;
            decimal oldSerialNo;
            decimal newSerialNo;
            decimal newGroupNo;
            Order temp;
            for (int index = trueSerialNos.Count - 1; index >= 0; index--)
            {
                oldSerialNo = Convert.ToDecimal(changedTable.DefaultView[index][serialNoField]);
                newSerialNo = trueSerialNos[index];
                newGroupNo = trueGroupNos[index];

                temp = currentTable.Orders[currentTable.Orders.IndexOf(oldSerialNo)];
                temp.SerialNo = newSerialNo;
                temp.GroupSerialNo = newGroupNo;
                if (currentTable.Orders.MaxSerialNo < temp.SerialNo)
                    currentTable.Orders.MaxSerialNo = temp.SerialNo;

                changedTable.DefaultView[index][serialNoField] = newSerialNo;
                changedTable.DefaultView[index][ConstSchemaNames.OrderColGroupSerialNo] = newGroupNo;

                newRows[index][serialNoField] = newSerialNo;
                newRows[index][ConstSchemaNames.OrderColGroupSerialNo] = newGroupNo;
            }
            changedTable.DefaultView.RowFilter = oldRowFilter;
            currentTable.AcceptChanges();
        }

        private void CallDoctorAdviceService(string messageName, byte[] data, string executorCode, string macAddress, bool isTempOrder)
        {
            string[,] parameters = new string[3, 6];

            //parameters[0, 0] = "wkdz";
            //parameters[1, 0] = "string";
            //parameters[2, 0] = macAddress;
            //parameters[0, 1] = "syxh";
            //parameters[1, 1] = "string";
            //parameters[2, 1] = CurrentPatient.NoOfHisFirstPage.ToString();
            //parameters[0, 2] = "xmlcqdata";
            //parameters[1, 2] = "base64string";
            //parameters[2, 2] = Convert.ToBase64String(data);
            //parameters[0, 3] = "iscqls";
            //parameters[1, 3] = "bool";
            //parameters[2, 3] = isTempOrder.ToString();
            //parameters[0, 4] = "encodingName";
            //parameters[1, 4] = "string";
            //parameters[2, 4] = ExchangeInfoOrderConst.EncodingName;
            //parameters[0, 5] = "lyjzsj";
            //parameters[1, 5] = "string";
            //parameters[2, 5] = string.Format("{0:D2}", CoreBusinessLogic.BusinessLogic.BlockingTimeOfTakeDrug);


            //string sExio;
            //sExio = m_InfoServer.BuildExchangeInfoString(messageName, ExchangeInfoOrderConst.EmrSystemName
            //   , ExchangeInfoOrderConst.HisSystemName, parameters);

            //string outMsg;
            //if (m_InfoServer.AddSyncExchangeInfo(sExio, ExchangeInfoOrderConst.DefaultEncoding, out outMsg) != ResponseFlag.Complete)
            //    throw new ApplicationException(outMsg);
            //// 通过判断返回数据集，检查操作是否成功
            //if (String.IsNullOrEmpty(outMsg))
            //    throw new DataCheckException(ConstMessages.ExceptionCallRemoting, ConstMessages.ExceptionTitleOrderTable);
            //else if (outMsg[0] != 'T') {
            //    if (outMsg[0] == 'F')
            //        throw new DataCheckException(outMsg.Substring(1), ConstMessages.ExceptionTitleOrderTable);
            //    else
            //        throw new DataCheckException(outMsg, ConstMessages.ExceptionTitleOrderTable);
            //}
        }

        private void UpdateSynchFlagToTrue(OrderTable currentTable, DataTable changedTable)
        {
            if (changedTable == null)// || (changedTable.Rows.Count == 0))
                return;

            changedTable.AcceptChanges();
            foreach (DataRow row in changedTable.Rows)
            {
                if (row[ConstSchemaNames.OrderColSynchFlag].ToString() == "0")
                    row[ConstSchemaNames.OrderColSynchFlag] = 1;

                // wxg , 2009-2-20, 6院修改, 如果是申请单医嘱将直接设置标志为已执行（其他医院需要再考虑）
                //if (!string.IsNullOrEmpty(row[ConstSchemaNames.OrderRequestOrderNo].ToString())
                //    && (decimal.Parse(row[ConstSchemaNames.OrderRequestOrderNo].ToString()) > 0)) {
                //    row[ConstSchemaNames.OrderColState] = "3202";
                //}
            }

            m_SqlExecutor.UpdateTable(changedTable, GetOrderTableName(currentTable.IsTempOrder), false);
            currentTable.DefaultView.BeginInit();
            currentTable.AcceptDataSended();
            currentTable.DefaultView.EndInit();
        }

        private void AddOrderDataIntoOutputTable(string doctorCode, OrderTable orderTable, bool needDrug, DateTime startTime)
        {
            OrderContentKind contentKind;
            if (needDrug)
                contentKind = OrderContentKind.Druggery;
            else
                contentKind = OrderContentKind.ChargeItem;

            DataRow row;
            // 取创建日期在当天之后的、已审核的药品或治疗项目医嘱(不包括出院带药)
            // TODO: 因为现在ModelBuild功能的限制，还不能方便的将所有的类型的医嘱转成模型数据
            foreach (Order order in orderTable.Orders)
            {
                if (((order.State == OrderState.Audited) || (order.State == OrderState.New))
                    && (order.CreateInfo.ExecuteTime >= startTime)
                    //&& (order.CreateInfo.Executor.Code.Trim() == doctorCode)
                    && (order.Content.OrderKind == contentKind))
                {
                    row = m_OutputTable.NewRow();
                    if (needDrug)
                        row[ConstSchemaNames.OrderOutputColProductSerialNo] = order.Content.Item.KeyValue;
                    else
                    {
                        // add by zhouhui 待确认
                        //如果是项目医嘱，需要将其项目代码放在字段ypdm中，以便大型项目的检查校验
                        row[ConstSchemaNames.OrderOutputColProductSerialNo] = 0;
                        row[ConstSchemaNames.OrderOutputColDruggeryCode] = order.Content.Item.KeyValue;
                    }
                    row[ConstSchemaNames.OrderOutputColDruggeryName] = order.Content.Item.Name;
                    row[ConstSchemaNames.OrderOutputColAmount] = order.Content.Amount;
                    row[ConstSchemaNames.OrderOutputColUnit] = order.Content.CurrentUnit.Name;
                    if ((order.Content.ItemUsage != null) && order.Content.ItemUsage.KeyInitialized)
                    {
                        row[ConstSchemaNames.OrderOutputColUsageCode] = order.Content.ItemUsage.Code;
                        row[ConstSchemaNames.OrderOutputColUsageName] = order.Content.ItemUsage.Name;
                    }
                    else
                    {
                        row[ConstSchemaNames.OrderOutputColUsageCode] = "";
                        row[ConstSchemaNames.OrderOutputColUsageName] = "";
                    }
                    row[ConstSchemaNames.OrderOutputColFrequencyCode] = order.Content.ItemFrequency.Code;
                    row[ConstSchemaNames.OrderOutputColFrequencyName] = order.Content.ItemFrequency.Name;
                    m_OutputTable.Rows.Add(row);
                }
            }
        }

        private void CancelOrderLinkedToRequest(string executorCode, decimal applySerialNo, DateTime operateTime)
        {
            TempOrder order;
            for (int index = m_TempTable.Orders.Count - 1; index >= 0; index--)
            {
                order = m_TempTable.Orders[index] as TempOrder;
                if ((order.State == OrderState.Executed) || (order.State == OrderState.Ceased))
                    break;

                if ((order.State == OrderState.Audited) && (order.ApplySerialNo == applySerialNo))
                    order.CancelOrder(executorCode, operateTime);
            }
        }

        private void DeleteOrderLinkedToRequest(decimal applySerialNo)
        {
            TempOrder order;
            for (int index = m_TempTable.Orders.Count - 1; index >= 0; index--)
            {
                if (m_TempTable.Orders[index].State != OrderState.New)
                    break;
                order = m_TempTable.Orders[index] as TempOrder;
                if ((order.State == OrderState.New) && (order.ApplySerialNo == applySerialNo))
                    order.Delete();
            }
        }

        [Obsolete("不再使用（原因在于无法处理临床项目）")]
        private void AddRequestItemIntoTempOrderTable(string executorCode, decimal applySerialNo, string executeDept, string[] itemCodeArray, DateTime starTime, DateTime operateTime)
        {
            TempOrder order;
            ChargeItemOrderContent content;

            foreach (string itemCode in itemCodeArray)
            {
                order = m_TempTable.NewOrder() as TempOrder;
                order.BeginInit();

                order.PatientId = Convert.ToDecimal(CurrentPatient.NoOfFirstPage, CultureInfo.CurrentCulture);
                order.OriginalDepartment = new Eop.Department(CurrentPatient.InfoOfAdmission.DischargeInfo.CurrentDepartment.Code);
                order.OriginalWard = new Eop.Ward(CurrentPatient.InfoOfAdmission.DischargeInfo.CurrentWard.Code);

                order.ApplySerialNo = applySerialNo;
                order.ExecuteDept = new Eop.Department(executeDept);
                order.CreateInfo = new OrderOperateInfo(executorCode, operateTime);
                order.StartDateTime = starTime;

                content = new ChargeItemOrderContent();
                content.BeginInit();
                content.Item = new ChargeItem(itemCode);
                content.Item.ReInitializeProperties();
                content.Amount = 1;
                content.CurrentUnit = content.Item.BaseUnit;
                content.ItemFrequency = new OrderFrequency(BusinessLogic.TempOrderFrequencyCode);
                content.EndInit();
                order.Content = content;

                order.EndInit();
                m_TempTable.AddOrder(order);
            }
        }

        private void AddRequestItemIntoTempOrderTable(string executorCode, decimal applySerialNo, string executeDept,
           IList<OrderInterfaceLogic.RequestFormItem> itemArray, DateTime starTime, DateTime operateTime)
        {
            TempOrder order;
            ChargeItemOrderContent content;
            ItemBase orderItem;

            foreach (OrderInterfaceLogic.RequestFormItem item in itemArray)
            {
                order = m_TempTable.NewOrder() as TempOrder;
                order.BeginInit();

                order.PatientId = Convert.ToDecimal(CurrentPatient.NoOfFirstPage, CultureInfo.CurrentCulture);
                order.OriginalDepartment = new Eop.Department(CurrentPatient.InfoOfAdmission.DischargeInfo.CurrentDepartment.Code);
                order.OriginalWard = new Eop.Ward(CurrentPatient.InfoOfAdmission.DischargeInfo.CurrentWard.Code);

                order.ApplySerialNo = applySerialNo;
                //order.ExecuteDept = new Department(executeDept);
                order.ExecuteDept = new Eop.Department(string.IsNullOrEmpty(item.ExecDept) ? executeDept : item.ExecDept);
                order.CreateInfo = new OrderOperateInfo(executorCode, operateTime);
                order.StartDateTime = starTime;
                order.Memo = string.Format("{0}`{1}`{2}`{3}", item.SpecimenId, item.Specimen, item.Memo, item.Urgent.ToString());

                // item.Kind  ( 0 收费 1 临床 )，未使用枚举
                switch (item.Kind)
                {
                    case 0:
                        content = new ChargeItemOrderContent();
                        orderItem = new ChargeItem();
                        break;
                    case 1:
                        content = new ClinicItemOrderContent();
                        orderItem = new ClinicItem();
                        break;
                    default:
                        throw new NotSupportedException("超出支持的项目类别( 0 收费 1 临床 )");
                }

                content.BeginInit();
                content.Item = orderItem;
                orderItem.Code = item.Code;
                content.Item.ReInitializeProperties();
                content.Amount = 1;
                content.CurrentUnit = content.Item.BaseUnit;
                content.ItemFrequency = new OrderFrequency(BusinessLogic.TempOrderFrequencyCode);
                content.EndInit();
                order.Content = content;

                order.EndInit();
                m_TempTable.AddOrder(order);
            }
        }

        private void SendQcMessage(string docCode, QCConditionType conditionType, object conditionObj, DateTime conditionTime)
        {
            // TODO: 现在时限规则设置还有问题，现在是通过读取对象的属性来判断条件，但是由于病人对象和医嘱对象中都只保留最新状态，而有一些条件是需要判断前后状态的变化
            m_Qcsv.AddRuleRecord(Convert.ToInt32(CurrentPatient.NoOfFirstPage), -1, docCode, conditionType, conditionObj, conditionTime);
        }
        #endregion

        #region private methods about edit flag calc
        private static bool CheckIsSpecialTextOrder(Order order)
        {
            bool isHerbSummary;
            return CheckIsSpecialTextOrder(order, out isHerbSummary);
        }

        private static bool CheckIsSpecialTextOrder(Order order, out bool isHerbSummary)
        {
            isHerbSummary = false;

            if ((order == null) || (order.Content == null))
                return false;

            switch (order.Content.OrderKind)
            {
                case OrderContentKind.TextAfterOperation:
                case OrderContentKind.TextShiftDept:
                case OrderContentKind.TextLeaveHospital:
                    return true;
                case OrderContentKind.TextNormal:
                    isHerbSummary = ((order.Content as TextOrderContent).IsSummaryOfHerbDetail);
                    return isHerbSummary;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 获取指定医嘱相对于所有新医嘱的位置（所有的新医嘱应该是连续的一组）
        /// 在判断选中的行是否可以上下移动时需要用到
        /// </summary>
        /// <param name="currentTable">医嘱所在的医嘱对象表</param>
        /// <param name="order">指定的医嘱</param>
        /// <returns>借用GroupPositionKind来表示</returns>
        private static GroupPositionKind GetOrderPosInNewGroup(OrderTable currentTable, Order order)
        {
            // None：不属于新医嘱
            // Single：只有一条新医嘱
            // GroupStart：第一条新医嘱
            // GroupMiddle：新医嘱的中间记录
            // GroupEnd：最后一条新医嘱

            if (order == null)
                throw new ArgumentException(ConstMessages.ExceptionOrderIndexNotFind);

            // 不是新增状态或特殊的文字医嘱，可以认为是在新医嘱之前
            if (order.State != OrderState.New)
                return GroupPositionKind.None;

            if (CheckIsSpecialTextOrder(order))
                return GroupPositionKind.None;

            // "术后医嘱"、"转科医嘱"、"出院医嘱"不能被移动，所以要排除在新医嘱之外
            // 在下面的处理中按照简化的思路处理：
            //    当前医嘱的前一条是特殊医嘱、空、非新增医嘱，本条就看作新医嘱的开始
            //    当前医嘱的后一条是特殊医嘱、空，本条就看作新医嘱的结束
            // 在添加、删除医嘱的地方保证：
            //    "术后医嘱"、"转科医嘱"前不会出现新增状态的医嘱
            //    "出院医嘱"后面只可能有出院带药医嘱
            int index = currentTable.Orders.IndexOf(order.SerialNo);
            Order pre = null;
            Order next = null;
            bool isStart = true;
            bool isEnd = true;

            if (index > 0)
            {
                pre = currentTable.Orders[index - 1];
                if ((!CheckIsSpecialTextOrder(pre)) && (pre.State == OrderState.New))
                    isStart = false;
            }
            if (index < (currentTable.Orders.Count - 1))
            {
                next = currentTable.Orders[index + 1];
                if (!CheckIsSpecialTextOrder(next))
                    isEnd = false;
            }

            if (isStart && isEnd)
                return GroupPositionKind.SingleOrder;
            if (isStart)
                return GroupPositionKind.GroupStart;
            if (isEnd)
                return GroupPositionKind.GroupEnd;

            return GroupPositionKind.GroupMiddle;
        }

        private static bool CheckIsBeforeSecondNewOrder(OrderTable currentTable, Order order)
        {
            if (order == null)
                return false;

            // 借用GroupPositionKind来表示相对位置
            GroupPositionKind pos = GetOrderPosInNewGroup(currentTable, order);

            return ((pos == GroupPositionKind.None) || (pos == GroupPositionKind.SingleOrder)
               || (pos == GroupPositionKind.GroupStart));
        }

        private static bool CheckIsLastNewOrder(OrderTable currentTable, Order order)
        {
            if (order == null)
                return false;

            GroupPositionKind pos = GetOrderPosInNewGroup(currentTable, order);

            return ((pos == GroupPositionKind.SingleOrder) || (pos == GroupPositionKind.GroupEnd));
        }

        private static OrderState CheckStateIsSame(Order[] selectedOrders)
        {
            OrderState state = OrderState.All; // 医嘱状态，初始为All，None表示状态不一致

            foreach (Order order in selectedOrders)
            {
                if (order == null)
                    return OrderState.None;
                if (state != order.State)
                {
                    if (state == OrderState.All)
                        state = order.State;
                    else
                        return OrderState.None;
                }
            }
            return state;
        }

        private static int CalcCountOfOutDruggery(OrderTable tempTable)
        {
            if ((tempTable == null) || (!tempTable.IsTempOrder))
                return 0;

            int totalCountOfOutDruggery = 0; // 临时医嘱中出院带药医嘱的数量
            for (int index = tempTable.Orders.Count - 1; index >= 0; index--)
            {
                if (tempTable.Orders[index].Content.OrderKind != OrderContentKind.OutDruggery)
                    break;
                totalCountOfOutDruggery++;
            }
            return totalCountOfOutDruggery;
        }

        private static decimal CheckHasPieceOfGroup(Order order, decimal lastGroupNo)
        {
            // -9999 表示错误
            // -2 表示处理第一条记录
            // -1 表示分组正常结束
            // 其它情况返回分组序号
            decimal errorNum = -9999;

            if (order == null)
                return errorNum;

            // 遇到组头或单条的记录，表示开始新的分组，后面的记录分组序号应该和它一致
            // ，直到遇到组尾的记录，将分组序号重设为-1
            switch (order.GroupPosFlag)
            {
                case GroupPositionKind.GroupStart:
                    if ((lastGroupNo == -2) || (lastGroupNo == -1))
                        return order.GroupSerialNo;
                    else
                        return errorNum;
                case GroupPositionKind.GroupMiddle:
                    if (order.GroupSerialNo != lastGroupNo)
                        return errorNum;
                    return order.GroupSerialNo;
                case GroupPositionKind.GroupEnd:
                    if (order.GroupSerialNo != lastGroupNo)
                        return errorNum;
                    return -1; // 表示分组结束 
                default:
                    if ((lastGroupNo == -2) || (lastGroupNo == -1))
                        return -1;
                    else
                        return errorNum;
            }
        }

        private static bool CheckCommonPropertiesIsSame(Order[] selectedOrders)
        {
            DateTime startTime = DateTime.Now;
            DateTime ceaseTime = DateTime.MinValue;
            string usageCode = String.Empty;
            OrderFrequency frequency = null;
            LongOrder longOrder;
            // 检查开始时间、用法、频次、停止时间等信息是否一致，以便判断这些医嘱是否属于同一批
            foreach (Order order in selectedOrders)
            {
                if (order != null)
                {
                    longOrder = order as LongOrder;
                    if (String.IsNullOrEmpty(usageCode)) //第一条
                    {
                        startTime = order.StartDateTime;

                        if ((order.Content.ItemUsage != null)
                           && (order.Content.ItemUsage.KeyInitialized))
                            usageCode = order.Content.ItemUsage.Code;
                        else
                            usageCode = "";

                        if (order.Content.ItemFrequency.KeyInitialized)
                            frequency = order.Content.ItemFrequency;
                        else
                            frequency = null;

                        if ((longOrder != null) && (longOrder.CeaseInfo != null) && (longOrder.CeaseInfo.HadInitialized))
                            ceaseTime = longOrder.CeaseInfo.ExecuteTime;
                    }
                    else
                    {
                        if (order.StartDateTime != startTime)
                            return false;

                        if ((order.Content.ItemUsage == null)
                           || (!order.Content.ItemUsage.KeyInitialized)
                           || (order.Content.ItemUsage.Code != usageCode))
                            return false;

                        if ((!order.Content.ItemFrequency.KeyInitialized)
                           || (!order.Content.ItemFrequency.Equals(frequency)))
                            return false;

                        if (longOrder != null)
                        {
                            if ((longOrder.CeaseInfo != null) && (longOrder.CeaseInfo.HadInitialized))
                            {
                                if ((ceaseTime == DateTime.MinValue) || (ceaseTime != longOrder.CeaseInfo.ExecuteTime))
                                    return false;
                            }
                            else
                            {
                                if (ceaseTime > DateTime.MinValue)
                                    return false;
                            }
                        }
                    }
                }
                else
                    return false;
            }
            return true;
        }

        private static AttributeOfSelectedFlag GetAttributeOfSelectedOrder(OrderTable table, Order[] selectedOrders)
        {
            AttributeOfSelectedFlag result = AttributeOfSelectedFlag.NumIsSerial
               | AttributeOfSelectedFlag.InSameGroup;

            if (CheckIsBeforeSecondNewOrder(table, selectedOrders[0]))
                result |= AttributeOfSelectedFlag.HasFirstNew;

            if (CheckIsLastNewOrder(table, selectedOrders[selectedOrders.Length - 1]))
                result |= AttributeOfSelectedFlag.HasLastNew;

            bool checkSerial = true;
            bool checkSpecial = true;
            bool checkLeaveHospital = table.IsTempOrder;
            bool checkOutDruggery = false;
            int countOfOutDruggery = 0;
            ItemKind itemKind = ItemKind.None;
            bool sameItemKind = true;
            bool isHerbSummary;

            int orderIndex = -1;
            decimal lastGroupSerialNo = -2; // -1表示没有遇到已分组的记录
            bool checkCeaseInfo = (!table.IsTempOrder);
            bool checkLinkToApply = table.IsTempOrder;
            LongOrder longOrder;
            foreach (Order order in selectedOrders)
            {
                if (order != null)
                {
                    // 检查是否含有特殊状态的医嘱
                    result |= GetStateAttributeOfSelected(order);
                    // 检查医嘱序号是否连续
                    if (checkSerial)
                    {
                        if (orderIndex == -1)
                            orderIndex = table.Orders.IndexOf(order.SerialNo);
                        else if (order.SerialNo != table.Orders[orderIndex].SerialNo)
                        {
                            result &= (~AttributeOfSelectedFlag.NumIsSerial);
                            checkSerial = false;
                        }
                        orderIndex++;
                    }
                    // 是否是特殊医嘱
                    if (checkSpecial && CheckIsSpecialTextOrder(order, out isHerbSummary))
                    {
                        result |= AttributeOfSelectedFlag.HasSpecial;
                        if (isHerbSummary)
                            result |= AttributeOfSelectedFlag.HasHerbSummary;
                        checkCeaseInfo = !isHerbSummary;
                    }
                    // 检查出院医嘱
                    if (checkLeaveHospital && (order.Content != null))
                    {
                        if (order.Content.OrderKind == OrderContentKind.TextLeaveHospital)
                        {
                            checkLeaveHospital = false;
                            checkOutDruggery = true;
                            result |= AttributeOfSelectedFlag.HasLeaveHospital;
                        }
                    }
                    // 检查医嘱内容的类型
                    if ((order.Content != null) && (order.Content.Item != null))
                    {
                        if (order.Content.OrderKind == OrderContentKind.OutDruggery)
                            countOfOutDruggery++;
                        if (sameItemKind && (itemKind != order.Content.Item.Kind))
                        {
                            if (itemKind == ItemKind.None)
                                itemKind = order.Content.Item.Kind;
                            else if ((itemKind != ItemKind.HerbalMedicine)
                               && ((itemKind == ItemKind.WesternMedicine) || (itemKind == ItemKind.PatentMedicine))
                               && ((order.Content.OrderKind == OrderContentKind.Druggery) || (order.Content.OrderKind == OrderContentKind.OutDruggery)))
                                itemKind = ItemKind.WesternMedicine; // 用西药来代替西药和成药
                            else
                                sameItemKind = false;
                        }
                    }
                    else
                        sameItemKind = false;

                    // 对分组进行检查
                    if ((lastGroupSerialNo != -2) && (order.GroupSerialNo != lastGroupSerialNo)) // 分组有改变
                        result &= (~AttributeOfSelectedFlag.InSameGroup);
                    if ((result & AttributeOfSelectedFlag.HasPieceOfGroup) == 0)
                        result |= GetGroupAttributeOfSelected(order, ref lastGroupSerialNo);

                    // 检查停止信息
                    if (checkCeaseInfo)
                    {
                        longOrder = order as LongOrder;
                        if ((longOrder != null) && (longOrder.CeaseInfo != null) && longOrder.CeaseInfo.HadInitialized)
                        {
                            result |= AttributeOfSelectedFlag.HasCeaseInfo;
                            checkCeaseInfo = false;
                        }
                    }
                    if (checkLinkToApply)
                    {
                        if ((order as TempOrder).ApplySerialNo != 0)
                        {
                            result |= AttributeOfSelectedFlag.HasLinkToApply;
                            checkLinkToApply = false;
                        }
                    }
                }
                else
                {
                    result &= (~AttributeOfSelectedFlag.NumIsSerial);
                    checkSerial = false;
                    sameItemKind = false;
                }
            }
            // 如果最终的分组序号不是-1，则表示最后一个分组的记录没有被选全
            if (lastGroupSerialNo != -1)
                result |= AttributeOfSelectedFlag.HasPieceOfGroup;
            // 检查出院带药是否全部选中
            if ((!checkOutDruggery) || (CalcCountOfOutDruggery(table) == countOfOutDruggery))
                result |= AttributeOfSelectedFlag.SelectedAllOutDurg;
            // 判断是否全部是药品内容
            if (sameItemKind)
            {
                if (itemKind == ItemKind.HerbalMedicine)
                    result |= AttributeOfSelectedFlag.AllIsHerbDruggery;
                else if (itemKind == ItemKind.WesternMedicine)
                    result |= AttributeOfSelectedFlag.AllIsOtherDruggery;
            }
            return result;
        }

        private static AttributeOfSelectedFlag GetGroupAttributeOfSelected(Order order, ref decimal lastGroupSerialNo)
        {
            AttributeOfSelectedFlag result = 0;

            if (order.GroupPosFlag != GroupPositionKind.SingleOrder)
                result |= AttributeOfSelectedFlag.HasGrouped;

            lastGroupSerialNo = CheckHasPieceOfGroup(order, lastGroupSerialNo);
            if (lastGroupSerialNo == -9999)
                result |= AttributeOfSelectedFlag.HasPieceOfGroup;

            return result;
        }

        private static AttributeOfSelectedFlag GetStateAttributeOfSelected(Order order)
        {
            switch (order.State)
            {
                case OrderState.Cancellation:
                    return AttributeOfSelectedFlag.HasCancelled;
                case OrderState.Ceased:
                    return AttributeOfSelectedFlag.HasCeased;
                default:
                    return 0;
            }
        }

        private static EditProcessFlag CalcEditFlagAboutAllowNew(Order[] selectedOrders, OrderState state, bool noSpecialOrder, AttributeOfSelectedFlag flag)
        {
            bool isSerial = ((flag & AttributeOfSelectedFlag.NumIsSerial) > 0); // 行号是否连续
            bool noGrouped = ((flag & AttributeOfSelectedFlag.HasGrouped) == 0); // 是否有包含在分组中的
            bool notFirstNew = ((flag & AttributeOfSelectedFlag.HasFirstNew) == 0); // 是否有处于所有新增记录中的第一条的，在第一条新增之前也认为true
            bool notLastNew = ((flag & AttributeOfSelectedFlag.HasLastNew) == 0); // 是否有处于所有新增记录中的最后一条的
            bool allIsDrug = (((flag & AttributeOfSelectedFlag.AllIsHerbDruggery) > 0)
               || ((flag & AttributeOfSelectedFlag.AllIsOtherDruggery) > 0)); // 标记是否都是药品

            EditProcessFlag result = 0;
            if (state == OrderState.New) // 下面的都具有公共条件：全部是新增
            {
                if (isSerial && noGrouped && noSpecialOrder)
                {
                    // 上移--全部是新增，记录是连续的，首条在第一条新增状态的记录之后，没有处于分组中的，无特殊医嘱
                    if (notFirstNew)
                        result |= EditProcessFlag.MoveUp;
                    // 下移--全部是新增，记录是连续的，末尾在最后一条新增状态的记录之前，没有处于分组中的，无特殊医嘱
                    if (notLastNew)
                        result |= EditProcessFlag.MoveDown;
                    // 成组--全部是新增，记录是连续的，有多条，没有已分组的，无特殊医嘱,基本属性都一致，全部是药品
                    if ((selectedOrders.Length > 1) && allIsDrug && (CheckCommonPropertiesIsSame(selectedOrders)))
                        result |= EditProcessFlag.SetGroup;
                }
                // 取消分组--全部是新增，有分组
                if (!noGrouped)
                    result |= EditProcessFlag.CancelGroup;
            }
            if ((selectedOrders.Length == 1) && allIsDrug && noGrouped)
            {
                // 组开始--单条新增记录，不在组中，不是最后一个新增记录
                if (notLastNew)
                    result |= EditProcessFlag.GroupStart;
                // 组结束--单条新增记录，不在组中，不是第一个新增记录
                if (notFirstNew)
                    result |= EditProcessFlag.GroupEnd;
            }
            return result;
        }
        #endregion

        #region public methods
        /// <summary>
        /// 根据医嘱类型获取用来过滤医嘱类别数据表的条件
        /// </summary>
        /// <param name="isTempOrder"></param>
        /// <returns></returns>
        public string GetOrderContentCatalogRowFilter(bool isTempOrder)
        {
            if (m_ProcessModel == EditorCallModel.EditOrder)
            {
                // 按照当前维护的医嘱类型，设置可选的医嘱内容
                if (isTempOrder)
                {
                    if (HasOutHospitalOrder) // 如果已录入出院医嘱，则只能输入出院带药
                        return String.Format(CultureInfo.CurrentCulture
                           , "Flag in ({0:D}, {1:D}) and ID = {2:D}"
                           , OrderManagerKind.Normal, OrderManagerKind.ForTemp, OrderContentKind.OutDruggery);
                    else
                        return String.Format(CultureInfo.CurrentCulture
                           , "Flag in ({0:D}, {1:D}) and ID <> {2:D}"
                           , OrderManagerKind.Normal, OrderManagerKind.ForTemp, OrderContentKind.OutDruggery);
                }
                else
                    return String.Format(CultureInfo.CurrentCulture
                          , "Flag in ({0:D}, {1:D})"
                          , OrderManagerKind.Normal, OrderManagerKind.ForLong);
            }
            else if (m_ProcessModel == EditorCallModel.EditSuite)
            {
                if (isTempOrder)
                    return String.Format(CultureInfo.CurrentCulture
                          , "ID in ({0:D}, {1:D}, {2:D}, {3:D})"
                          , OrderContentKind.Druggery, OrderContentKind.ChargeItem, OrderContentKind.OutDruggery, OrderContentKind.TextNormal);
                else
                    return String.Format(CultureInfo.CurrentCulture
                          , "ID in ({0:D}, {1:D}, {2:D}, {3:D})"
                          , OrderContentKind.Druggery, OrderContentKind.ChargeItem, OrderContentKind.GeneralItem, OrderContentKind.TextNormal);
            }
            else
                return "1=2";
        }

        /// <summary>
        /// 根据标志返回对应医嘱表
        /// </summary>
        /// <param name="isTempOrder"></param>
        /// <returns></returns>
        public OrderTable GetCurrentOrderTable(bool isTempOrder)
        {
            if (isTempOrder)
                return m_TempTable;
            else
                return m_LongTable;
        }

        /// <summary>
        /// 当天医嘱默认开始时间（精确到分钟）
        /// </summary>
        public DateTime GetDefaultStartDateTime(bool isTempOrder)
        {
            OrderTable currentTable;
            if (isTempOrder)
                currentTable = m_TempTable;
            else
                currentTable = m_LongTable;

            // 开始时间默认值的优先级：
            //      高：上一条新增医嘱的开始时间（如果在最小开始时间之后）
            //      中：设置中约定的时间（警告时间）
            //      低：当前时间（默认值）
            DateTime last = DateTime.MinValue;
            for (int index = currentTable.Orders.Count - 1; index >= 0; index--)
            {
                last = currentTable.Orders[index].StartDateTime;
                if (last > DateTime.MinValue) // 最后一个医嘱对象可能是新加入的，还没有初始化开始日期
                    break;
            }

            if (last >= WarnStartDateTime)
                return last;
            else
            {
                // 长期医嘱从第二天八点开始，临时医嘱从当前时间开始
                if (isTempOrder)
                {
                    // 默认为当前日期，分钟以半小时为单位（45～15分钟为“00”，15～45分钟为“30”）
                    int hour = DateTime.Now.Hour;
                    int minute = DateTime.Now.Minute;
                    if (minute <= 30)
                        minute = 30;
                    else
                    {
                        hour += 1;
                        minute = 0;
                    }
                    return DateTime.Today + new TimeSpan(hour, minute, 0);
                }
                else
                {
                    return DateTime.Now.AddDays(1).Date + new TimeSpan(8, 0, 0);
                }
            }
        }

        /// <summary>
        /// 计算对选中的医嘱可以执行的医嘱操作
        /// </summary>
        /// <param name="table"></param>
        /// <param name="selectedOrders"></param>
        /// <returns></returns>
        public EditProcessFlag CalcEditProcessFlag(OrderTable table, Order[] selectedOrders)
        {
            if ((table == null) || (selectedOrders == null) || (selectedOrders.Length == 0))
                return 0;

            Type orderType;
            bool allowNew;
            if (table.IsTempOrder)
            {
                orderType = typeof(TempOrder);
                allowNew = AllowAddTemp;
            }
            else
            {
                orderType = typeof(LongOrder);
                allowNew = AllowAddLong;
            }

            foreach (Order order in selectedOrders)
                if (order.GetType() != orderType)
                    return 0;

            EditProcessFlag result = 0;
            OrderState state = CheckStateIsSame(selectedOrders); // 状态是否一致（一致时是何种状态），None表示状态不一致
            AttributeOfSelectedFlag flag = GetAttributeOfSelectedOrder(table, selectedOrders);

            bool noCancelled = ((flag & AttributeOfSelectedFlag.HasCancelled) == 0); // 是否包含已取消医嘱
            bool noCeased = ((flag & AttributeOfSelectedFlag.HasCeased) == 0); // 是否包含已停止医嘱
            bool noCeaseInfo = ((flag & AttributeOfSelectedFlag.HasCeaseInfo) == 0); // 是否包含有停止信息的医嘱
            bool noSpecialOrder = ((flag & AttributeOfSelectedFlag.HasSpecial) == 0); // 是否包含特殊医嘱(术后、转科、出院等),影响到医嘱是否能移位
            bool noLeaveHospital = ((flag & AttributeOfSelectedFlag.HasLeaveHospital) == 0); // 是否包含出院医嘱
            bool allOutDurgSelected = ((flag & AttributeOfSelectedFlag.SelectedAllOutDurg) > 0); // 出院带药医嘱是否全部选中了
            bool noPieceOfGroup = ((flag & AttributeOfSelectedFlag.HasPieceOfGroup) == 0); // 同组的记录是否被全部选中
            bool linkToApply = ((flag & AttributeOfSelectedFlag.HasLinkToApply) > 0); // 标记是否关联申请单
            bool hasHerbSummary = ((flag & AttributeOfSelectedFlag.HasHerbSummary) > 0); // 标记是否有草药汇总信息

            // 以下是和是否允许新增有关的
            if (allowNew)
            {
                if (linkToApply)
                    noSpecialOrder = false;
                result |= CalcEditFlagAboutAllowNew(selectedOrders, state, noSpecialOrder, flag);
            }
            // 删除--全部是新增, 删除“出院医嘱”时同时删除了所有“出院带药”医嘱，不包含关联申请单的医嘱， 不包含草药汇总信息
            if ((state == OrderState.New) && (noLeaveHospital || allOutDurgSelected) && (!linkToApply) && (!hasHerbSummary))
                result |= EditProcessFlag.Delete;
            // 取消--全部是审核，不包含关联申请单的医嘱
            if ((state == OrderState.Audited) && (!linkToApply))// && noShiftDeptOrder)
                result |= EditProcessFlag.Cancel;
            // 停止--长期的，且没有包含已停止、已取消的记录、没有设过停止时间的
            // modified by zhouhui 停止--长期的，且没有包含已停止、已取消的记录
            //TODO: 目前长嘱的停止时间是可以修改的，要处理相关的事情
            //if ((!table.IsTempOrder) && noCancelled && noCeased && noCeaseInfo)
            if ((!table.IsTempOrder) && noCancelled && noCeased)
                result |= EditProcessFlag.Cease;
            // 审核--全部是新增,同组的医嘱不能分开审核//，不包含关联申请单的医嘱
            if ((state == OrderState.New) && noPieceOfGroup)// && (!linkToApply))
                result |= EditProcessFlag.Audit;
            // 执行--全部是已审核状态(执行按钮只是临时使用！！！)
            if (state == OrderState.Audited)
                result |= EditProcessFlag.Execute;
            //// 多选--选中的是单条
            //if (selectedHandles.Length == 1)
            //   result |= EditProcessFlag.StartMultiSelect;

            // 剪切--全部是新医嘱，成组医嘱被一起选中，不包含特殊医嘱
            if (allowNew && (state == OrderState.New) && noPieceOfGroup && noSpecialOrder)
                result |= EditProcessFlag.Cut;
            // 复制--不包含特殊医嘱
            if (allowNew && noSpecialOrder)
                result |= EditProcessFlag.Copy;

            // 是草药汇总信息--单条，文字医嘱，关联草药明细
            if ((selectedOrders.Length == 1) && (hasHerbSummary))
                result |= EditProcessFlag.IsHerbSummary;

            // 是草药明细信息--单条，已分组，是草药项目
            if ((selectedOrders.Length == 1) && (selectedOrders[0].GroupPosFlag != GroupPositionKind.SingleOrder)
               && (selectedOrders[0].Content.Item != null) && (selectedOrders[0].Content.Item.Kind == ItemKind.HerbalMedicine))
                result |= EditProcessFlag.IsHerbDetail;

            return result;
        }

        /// <summary>
        /// 删除指定的新医嘱
        /// </summary>
        /// <param name="selectedHandles"></param>
        public void DeleteNewOrder(Order[] selectedOrders, bool isTempOrder)
        {
            OrderTable currentTable = GetCurrentOrderTable(isTempOrder);

            string errMsg;
            try
            {
                errMsg = CheckCanBeDeleted(currentTable, selectedOrders);
            }
            catch
            {
                throw;
            }
            if (String.IsNullOrEmpty(errMsg))
            {
                currentTable.DefaultView.BeginInit();

                DoDeleteNewOrder(currentTable, selectedOrders);

                currentTable.DefaultView.EndInit();
            }
            else
                throw new DataCheckException(errMsg, ConstMessages.ExceptionTitleOrder);
        }

        /// <summary>
        /// 将指定范围的医嘱设为一组
        /// </summary>
        /// <param name="selectedHandles">当前选中的医嘱行号</param>
        public void SetOrderGroup(Order[] selectedOrders, bool isTempOrder)
        {
            OrderTable currentTable = GetCurrentOrderTable(isTempOrder);

            string errMsg;
            try
            {
                errMsg = CheckCanSetGroup(currentTable, selectedOrders);
            }
            catch
            {
                throw;
            }
            if (String.IsNullOrEmpty(errMsg))
            {
                currentTable.DefaultView.BeginInit();

                DoSetGroup(currentTable, selectedOrders);

                currentTable.DefaultView.EndInit();
            }
            else
                throw new DataCheckException(errMsg, ConstMessages.ExceptionTitleOrder);
        }

        /// <summary>
        /// 自动对新医嘱进行分组
        /// </summary>
        /// <returns>被分组的记录的行号</returns>
        public int[] AutoSetNewOrderGrouped(bool isTempOrder)
        {
            OrderTable currentTable = GetCurrentOrderTable(isTempOrder);
            OrderTableView defaultView = currentTable.DefaultView;
            // 从最后一条新增医嘱开始向前检查，找到可以成组的最大范围
            // 将选中范围内的记录设为一组
            int index = defaultView.Count - 1; // index最终指向找到的倒数第一个不能加入分组的记录
            for (; index >= 0; index--)
            {
                // 都是新增状态、未分组
                if ((defaultView[index].State != OrderState.New)
                   || (defaultView[index].GroupPosFlag != GroupPositionKind.SingleOrder))
                    break;
                if ((defaultView[index].OrderCache.Content.OrderKind != OrderContentKind.Druggery)
                   && (defaultView[index].OrderCache.Content.OrderKind != OrderContentKind.OutDruggery))
                    break;
                if (index == (defaultView.Count - 1))
                    continue;
                // 两两检查是否可以成组，得到最终的index位置
                try
                {
                    if (!String.IsNullOrEmpty(CheckCanSetGroup(currentTable, new Order[] { defaultView[index].OrderCache, defaultView[index + 1].OrderCache })))
                        break;
                }
                catch
                {
                    break;
                }
            }

            index++;
            int maxGroupCount = defaultView.Count - index;

            // 选中指定记录，设为组 
            //if (index < (defaultView.Count - 1))
            if (maxGroupCount > 1)
            {
                Order[] selectedOrders = new Order[maxGroupCount];
                int[] rowHandles = new int[maxGroupCount];
                for (int index2 = 0; index2 < maxGroupCount; index2++)
                {
                    selectedOrders[index2] = defaultView[index + index2].OrderCache;
                    rowHandles[index2] = index + index2;
                }
                SetOrderGroup(selectedOrders, isTempOrder);
                return rowHandles;
            }
            else
                return null;
        }

        /// <summary>
        /// 取消选定医嘱的分组
        /// </summary>
        /// <param name="selectedOrders">当前选中的医嘱</param>
        /// <param name="isTempOrder"></param>
        public void CancelOrderGroup(Order[] selectedOrders, bool isTempOrder)
        {
            OrderTable currentTable = GetCurrentOrderTable(isTempOrder);

            string errMsg;
            try
            {
                errMsg = CheckCanCancelGroup(selectedOrders);
            }
            catch
            {
                throw;
            }
            if (String.IsNullOrEmpty(errMsg))
            {
                currentTable.DefaultView.BeginInit();

                DoCancelGroup(currentTable, selectedOrders);

                currentTable.DefaultView.EndInit();
            }
            else
                throw new DataCheckException(errMsg, ConstMessages.ExceptionTitleOrder);
        }

        /// <summary>
        /// 审核选定的医嘱
        /// </summary>
        /// <param name="selectedOrders">当前选定的医嘱</param>
        /// <param name="auditor">审核者代码</param>
        /// <param name="auditTime">审核时间</param>
        /// <param name="isTempOrder">标记是否是临时医嘱</param>
        public void AuditOrder(Order[] selectedOrders, string auditor, DateTime auditTime, bool isTempOrder)
        {
            OrderTable currentTable = GetCurrentOrderTable(isTempOrder);

            string errMsg;
            try
            {
                errMsg = CheckCanBeAudited(currentTable, selectedOrders);
            }
            catch
            {
                throw;
            }
            if (String.IsNullOrEmpty(errMsg))
            {
                currentTable.DefaultView.BeginInit();

                if (isTempOrder)
                    DoAuditTempOrder(currentTable, selectedOrders, auditor, auditTime);
                else
                    DoAuditLongOrder(currentTable, selectedOrders, auditor, auditTime);

                currentTable.DefaultView.EndInit();
            }
            else
                throw new DataCheckException(errMsg, ConstMessages.ExceptionTitleOrder);
        }

        /// <summary>
        /// 取消选定的已审核医嘱
        /// </summary>
        /// <param name="selectedOrders">当前选定的医嘱</param>
        /// <param name="auditor">审核者代码</param>
        /// <param name="auditTime">审核时间</param>
        public void CancelOrder(Order[] selectedOrders, string cancellor, DateTime cancelTime, bool isTempOrder)
        {
            OrderTable currentTable = GetCurrentOrderTable(isTempOrder);

            string errMsg;
            try
            {
                errMsg = CheckCanBeCancelled(currentTable, selectedOrders);
            }
            catch
            {
                throw;
            }
            if (String.IsNullOrEmpty(errMsg))
            {
                //currentTable.DefaultView.BeginInit();

                DoCancelOrder(currentTable, selectedOrders, cancellor, cancelTime);

                //currentTable.DefaultView.EndInit();
            }
            else
                throw new DataCheckException(errMsg, ConstMessages.ExceptionTitleOrder);
        }

        /// <summary>
        /// 设置长期医嘱的停止时间
        /// </summary>
        /// <param name="selectedOrders"></param>
        /// <param name="m_DoctorCode"></param>
        /// <param name="ceaseTime"></param>
        public void SetLongOrderCeaseInfo(Order[] selectedOrders, string ceasor, DateTime ceaseTime)
        {
            string errMsg;
            try
            {
                errMsg = CheckCanSetCeaseTime(m_LongTable, selectedOrders, ceaseTime);
            }
            catch
            {
                throw;
            }
            if (String.IsNullOrEmpty(errMsg))
            {
                //m_LongTable.DefaultView.BeginInit();

                DoSetLongOrderCeaseInfo(selectedOrders, ceasor, ceaseTime);

                //m_LongTable.DefaultView.EndInit();
            }
            else
                throw new DataCheckException(errMsg, ConstMessages.ExceptionTitleOrder);
        }

        /// <summary>
        /// 检查传入的医嘱数据是否符合要求
        /// </summary>
        /// <param name="currentTable">在指定的医嘱表中检查</param>
        /// <param name="orders">要检查的医嘱集合</param>
        /// <param name="callByEditor">是否是编辑器调用(不在编辑器调用则不检查开始时间)</param>
        public void CheckOrderData(OrderTable currentTable, Order[] orders, bool callByEditor, bool skipWarnning)
        {
            if ((orders == null) || (orders.Length == 0))
                return;

            UpdateContentFlag updateFlag;
            if (callByEditor)
                updateFlag = UpdateContentFlag.StartDate | UpdateContentFlag.Content;
            else
                updateFlag = UpdateContentFlag.Content;

            Order order;
            for (int index = 0; index < orders.Length; index++)
            {
                order = orders[index];
                if (order.EditState == OrderEditState.Deleted)
                    continue;
                if (order.State == OrderState.Ceased) // 停止状态的可以跳过时间、内容检查
                    continue;
                try
                {
                    CheckOrderValueBeforeSet(currentTable, order
                       , currentTable.Orders.IndexOf(order.SerialNo), updateFlag);
                }
                catch (DataCheckException e)
                {
                    if ((e.WarnningLevel == 1) || (!skipWarnning))
                    {
                        e.RowIndex = index;
                        e.OrderSerialNo = order.SerialNo;
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定医嘱表中最近做过修改的医嘱保存到数据库中(在调用之前要手工调用数据检查方法)
        /// 主要供编辑器调用
        /// </summary>
        /// <param name="changedOrders"></param>
        /// <param name="currentTable"></param>
        /// <param name="executorCode">执行保存操作的职工代码</param>
        /// <param name="macAddress">调用端的网卡地址</param>
        /// <param name="callByEditor">标记是否是在医嘱编辑器中调用</param>
        public void SaveChangedOrderDataInEditor(OrderTable currentTable, Order[] changedOrders, string executorCode, string macAddress)
        {
            if ((String.IsNullOrEmpty(executorCode)) || (String.IsNullOrEmpty(macAddress)))
                throw new ArgumentNullException();

            DoSaveChangedData(currentTable, changedOrders, executorCode, macAddress, true, false, true);
        }

        /// <summary>
        /// 将最近做过修改的医嘱保存到数据库中
        /// 主要供其它处理逻辑中直接调用
        /// </summary>
        /// <param name="executorCode">执行保存操作的职工代码</param>
        /// <param name="macAddress">调用端的网卡地址</param>
        /// <param name="forceSend">强制发送</param>
        public void SaveAllChangedOrderData(string executorCode, string macAddress, bool forceSend)
        {
            //SaveAllChangedOrderData(executorCode, macAddress, forceSend, false, null, DateTime.MinValue);

            if ((String.IsNullOrEmpty(executorCode)) || (String.IsNullOrEmpty(macAddress)))
                throw new ArgumentNullException();

            Order[] changedOrders = m_TempTable.GetChangedOrders();

            if ((changedOrders != null) && (changedOrders.Length > 0))
            {
                CheckOrderData(m_TempTable, changedOrders, false, true);

                DoSaveChangedData(m_TempTable, changedOrders, executorCode, macAddress, false, forceSend, false);
            }

            changedOrders = m_LongTable.GetChangedOrders();
            if ((changedOrders != null) && (changedOrders.Length > 0))
            {
                CheckOrderData(m_LongTable, changedOrders, false, true);

                DoSaveChangedData(m_LongTable, changedOrders, executorCode, macAddress, false, forceSend, false);
            }
        }

        ///// <summary>
        ///// 将最近做过修改的医嘱保存到数据库中
        ///// 主要供其它处理逻辑中直接调用
        ///// </summary>
        ///// <param name="executorCode">执行保存操作的职工代码</param>
        ///// <param name="macAddress">调用端的网卡地址</param>
        ///// <param name="forceSend">强制发送</param>
        ///// <param name="saveSendLog">是否保存有变更的医嘱数据到记录表中</param>
        ///// <param name="fireCode">触发保存动作的职工代码，和执行保存操作的不一定是同一个人</param>
        ///// <param name="fireTime">触发保存动作的时间</param>
        //public void SaveAllChangedOrderData(string executorCode, string macAddress, bool forceSend, bool saveSendLog, string fireCode, DateTime fireTime)
        //{
        //   if ((String.IsNullOrEmpty(executorCode)) || (String.IsNullOrEmpty(macAddress)))
        //      throw new ArgumentNullException();
        //   if (saveSendLog && (String.IsNullOrEmpty(fireCode) || (fireTime == DateTime.MinValue)))
        //      throw new ArgumentNullException("触发保存动作的职工代码");

        //   Order[] changedOrders = m_TempTable.GetChangedOrders();

        //   if ((changedOrders != null) && (changedOrders.Length > 0))
        //   {
        //      CheckOrderData(m_TempTable, changedOrders, false, true);

        //      DoSaveChangedData(m_TempTable, changedOrders, executorCode, macAddress, false, forceSend, false, saveSendLog, fireCode, fireTime);
        //   }

        //   changedOrders = m_LongTable.GetChangedOrders();
        //   if ((changedOrders != null) && (changedOrders.Length > 0))
        //   {
        //      CheckOrderData(m_LongTable, changedOrders, false, true);

        //      DoSaveChangedData(m_LongTable, changedOrders, executorCode, macAddress, false, forceSend, false, saveSendLog, fireCode, fireTime);
        //   }
        //}

        /// <summary>
        /// 同步病人的长期、临时数据到HIS中（在调用前要手工执行数据保存方法）
        /// </summary>
        /// <param name="currentTable"></param>
        /// <param name="executorCode"></param>
        /// <param name="macAddress"></param>
        /// <param name="callByEditor"></param>
        public void SynchDataToHIS(OrderTable currentTable, string executorCode, string macAddress)
        {
            if ((String.IsNullOrEmpty(executorCode)) || (String.IsNullOrEmpty(macAddress)))
                throw new ArgumentNullException();

            DoManualSynchDataToHis(currentTable, executorCode, macAddress);
        }

        /// <summary>
        /// 验证医嘱开始时间和医嘱内容是否正确
        /// </summary>
        /// <param name="currentTable">指定的医嘱表</param>
        /// <param name="orderTemp">保存带校验值的医嘱临时变量</param>
        /// <param name="targetIndex">当前处理的医嘱行号(相对于Table的行号)(如果是新增或插入医嘱，则行号可能不在List的索引内)</param>
        /// <param name="updateFalg">标记哪些数据已被更新</param>
        /// <returns>出错的列名和错误信息列表，便于Grid设置提示信息</returns>
        public void CheckOrderValueBeforeSet(OrderTable currentTable, Order orderTemp, int targetIndex, UpdateContentFlag updateFalg)
        {
            if (orderTemp == null)
                throw new ArgumentNullException(String.Format(ConstMessages.ExceptionFormatNoValue, ConstNames.TempOrder));

            if (m_ProcessModel == EditorCallModel.EditOrder) // 只有编辑医嘱时才关心时间的逻辑
            {
                if (((updateFalg & UpdateContentFlag.StartDate) > 0)
                   && (orderTemp.State == OrderState.New)) // 检查开始日期
                {
                    CheckOrderStartDatetime(currentTable, targetIndex, orderTemp.StartDateTime);
                    //如果是长期医嘱，则在修改完开始时间后需要校验其停止时间
                    if (!currentTable.IsTempOrder)
                    {
                        LongOrder orderLong = (LongOrder)orderTemp;
                        if ((orderLong != null) && (orderLong.CeaseInfo != null))
                            CheckCeaseTimeOfLongOrder(currentTable, orderLong);
                    }
                }
                if ((updateFalg & UpdateContentFlag.CeaseDate) > 0) // 检查长期医嘱的停止时间
                    CheckCeaseTimeOfLongOrder(currentTable, orderTemp);
            }

            if ((updateFalg & UpdateContentFlag.Content) > 0) // 检查医嘱内容
                CheckOrderContentData(currentTable, orderTemp.Content, targetIndex, GroupPositionKind.None);

            // 检查整体逻辑
        }

        /// <summary>
        /// 获取指定医生为当前病人在指定时间之后新开的药品和治疗项目医嘱
        /// </summary>
        /// <param name="doctorCode">医生工号</param>
        /// <param name="startTime">指定的时间</param>
        /// <returns></returns>
        public DataTable GetNewOrder(string doctorCode, DateTime startTime)
        {
            if (String.IsNullOrEmpty(doctorCode))
                throw new ArgumentNullException(String.Format(ConstMessages.ExceptionFormatNoValue, ConstNames.DoctorId));

            if (m_OutputTable == null)
                GenerateOutputTable();
            else
                m_OutputTable.Clear();

            AddOrderDataIntoOutputTable(doctorCode, m_LongTable, true, startTime);
            AddOrderDataIntoOutputTable(doctorCode, m_TempTable, true, startTime);

            AddOrderDataIntoOutputTable(doctorCode, m_LongTable, false, startTime);
            AddOrderDataIntoOutputTable(doctorCode, m_TempTable, false, startTime);

            return m_OutputTable;
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
            if (String.IsNullOrEmpty(doctorCode))
                throw new ArgumentNullException(String.Format(ConstMessages.ExceptionFormatNoValue, ConstNames.DoctorId));

            if (m_OutputTable == null)
                GenerateOutputTable();
            else
                m_OutputTable.Clear();

            AddOrderDataIntoOutputTable(doctorCode, m_LongTable, needDrug, startTime);
            AddOrderDataIntoOutputTable(doctorCode, m_TempTable, needDrug, startTime);

            return m_OutputTable;
        }

        /// <summary>
        /// 获取皮试信息DataTable
        /// </summary>
        /// <returns></returns>
        public DataTable GetSkinTestResultData()
        {
            string commandText = String.Format(CultureInfo.CurrentCulture
               , ConstSqlSentences.FormatSelectSkinTestResult
               , CurrentPatient.NoOfFirstPage);
            return m_SqlExecutor.ExecuteDataTable(commandText);
        }

        /// <summary>
        /// 保存皮试信息
        /// </summary>
        /// <param name="specSerialNo"></param>
        /// <param name="druggeryName"></param>
        /// <param name="skinTestResultKind"></param>
        public DataTable SaveSkinTestResult(string doctorCode, decimal specSerialNo, string druggeryName, SkinTestResultKind skinTestResultKind)
        {
            int plusFlag;
            if (skinTestResultKind == SkinTestResultKind.Plus)
                plusFlag = 1;
            else
                plusFlag = 0;

            string endDate;
            if (skinTestResultKind == SkinTestResultKind.MinusTreeDay)
                endDate = DateTime.Now.Date.AddDays(3).ToString(ConstFormat.FullDate);
            else
                endDate = "";

            string insertCommand = String.Format(CultureInfo.CurrentCulture
               , ConstSqlSentences.FormatInsertSkinTestResult
               , CurrentPatient.NoOfFirstPage, specSerialNo, DateTime.Now.ToString(ConstFormat.FullDate)
               , endDate, plusFlag, doctorCode, DateTime.Now.ToString(ConstFormat.FullDateTime));
            m_SqlExecutor.ExecuteNoneQuery(insertCommand);
            return GetSkinTestResultData();
        }

        /// <summary>
        /// 检查是否可以向当前选中医嘱的后面插入医嘱。如果当前选择医嘱为空，表示添加新医嘱
        /// </summary>
        /// <param name="focusedOrder">当前选中的医嘱</param>
        /// <param name="isTempOrder">指明向那个医嘱表插入数据</param>
        /// <param name="insertOrders">要插入的医嘱</param>
        public void CheckCanInsertOrder(Order focusedOrder, bool isTempOrder, Order[] insertOrders)
        {
            // 检查当前位置是否可以插入新医嘱，提示不能插入的原因
            //    是否允许编辑病人医嘱
            //    是否有转科(限制医嘱添加操作)、出院医嘱(限制医嘱添加、修改操作，可以增加出院带药)
            //    当前医嘱类别是否允许新增医嘱
            //    当前选择的医嘱状态是否允许添加医嘱
            //    当前选中行的下一行是否可以插入医嘱（最多选中一行）
            //       下一行应该是新医嘱(或为空)
            //       或未选中医嘱
            //    要检查插入的位置是否在分组中，如果是的话，还要检查能否加入分组；能加入的话，则在插入后重新分组，否则提示
            OrderTable currentTable = GetCurrentOrderTable(isTempOrder);
            if (!currentTable.DefaultView.AllowEdit)
                throw new DataCheckException(ConstMessages.CheckEditableOfOrderCatalog, ConstMessages.ExceptionTitleOrderTable);
            if (!currentTable.DefaultView.AllowNew)
                throw new DataCheckException(ConstMessages.CheckCanInsertOrderAtSpecialState, ConstMessages.ExceptionTitleOrderTable);
            if (HasShiftDeptOrder)
                throw new DataCheckException(ConstMessages.CheckCantAddNewAfterHasShiftDeptOrder, ConstMessages.ExceptionTitleOrderTable);
            if ((HasOutHospitalOrder) && (!isTempOrder))
                throw new DataCheckException(ConstMessages.CheckOnlyAllowDruggeryAfterHasOutHospitalOrder, ConstMessages.ExceptionTitleOrderTable);
            if (focusedOrder != null)
            {
                int index = currentTable.Orders.IndexOf(focusedOrder.SerialNo);
                if ((index >= 0) && ((index + 1) < currentTable.Orders.Count))
                {
                    if (currentTable.Orders[index + 1].State != OrderState.New)
                        throw new DataCheckException(ConstMessages.CheckCanInsertOrderAfterCurrent, ConstMessages.ExceptionTitleOrderTable);
                    if (isTempOrder && HasOutHospitalOrder)
                    {
                        int indexOfOutHospital = currentTable.Orders.Count - 1;
                        for (int i = indexOfOutHospital; i >= 0; i--)
                        {
                            if (currentTable.Orders[i].Content.OrderKind == OrderContentKind.TextLeaveHospital)
                            {
                                indexOfOutHospital = i;
                                break;
                            }
                        }
                        if (index < indexOfOutHospital)
                            throw new DataCheckException(ConstMessages.CheckCanInsertOrderBeforeOutHospitalOrder, ConstMessages.ExceptionTitleOrderTable);
                    }
                }
                // 检查是否在分组中
                CheckCanInsertOrdersToGroup(currentTable, focusedOrder, insertOrders);
            }
        }

        /// <summary>
        /// 插入成套或要粘贴的医嘱
        /// </summary>
        /// <param name="currentTable"></param>
        /// <param name="docCode"></param>
        /// <param name="contents"></param>
        /// <param name="focusedOrder"></param>
        /// <returns>实际插入的记录数</returns>
        public int InsertSuiteOrder(OrderTable currentTable, string docCode, object[,] contents, Order focusedOrder)
        {
            if ((contents == null) || (contents.GetUpperBound(0) < 0))
                return 0;

            Order order;
            decimal patientId = CurrentPatient.NoOfFirstPage;
            DateTime startTime = GetDefaultStartDateTime(currentTable.IsTempOrder);
            DateTime operateTime = DateTime.Now;
            OrderContent content;
            //bool skipOrder = false; // 标记是否要跳过同组的医嘱(组)
            int insertPos;

            if (focusedOrder == null)
                insertPos = currentTable.Orders.Count;
            else
                insertPos = currentTable.Orders.IndexOf(focusedOrder.SerialNo) + 1;

            //currentTable.DefaultView.BeginInit();
            int insertedNum = 0;

            for (int index = 0; index <= contents.GetUpperBound(0); index++)
            {
                content = contents[index, 0] as OrderContent;
                // 先检查医嘱内容，不通过，则跳到下一条继续插入
                try
                {
                    CheckOrderContentData(currentTable, content, insertPos, (GroupPositionKind)contents[index, 1]);
                }
                catch
                {
                    continue;
                }

                order = currentTable.NewOrder();
                order.BeginInit();

                order.PatientId = patientId;
                order.OriginalDepartment = new Eop.Department(CurrentPatient.InfoOfAdmission.DischargeInfo.CurrentDepartment.Code);
                order.OriginalWard = new Eop.Ward(CurrentPatient.InfoOfAdmission.DischargeInfo.CurrentWard.Code);

                order.CreateInfo = new OrderOperateInfo(docCode, operateTime);
                order.StartDateTime = startTime;
                order.GroupPosFlag = (GroupPositionKind)Convert.ToInt32(contents[index, 1]);

                // 处理分组（没有处理一组医嘱中部分通过的情况！！！）
                if ((order.GroupPosFlag == GroupPositionKind.SingleOrder)
                   || (order.GroupPosFlag == GroupPositionKind.GroupStart))
                    order.GroupSerialNo = order.SerialNo;
                else
                    order.GroupSerialNo = currentTable.Orders[currentTable.Orders.Count - 1].GroupSerialNo; // 与上一条一致

                order.Content = content;

                order.EndInit();
                currentTable.InsertOrderAt(order, insertPos);

                insertPos++;
                insertedNum++;
            }

            return insertedNum;

            //currentTable.DefaultView.EndInit();
        }

        /// <summary>
        /// 保存申请单数据
        /// </summary>
        /// <param name="executorCode">申请单创建者代码</param>
        /// <param name="macAddress">调用者的网卡地址</param>
        /// <param name="applySerialNo">申请单序号</param>
        /// <param name="executeDept">申请单执行科室</param>
        /// <param name="itemCodeArray">申请单包含的项目代码</param>
        /// <param name="starTime">申请单开始时间</param>
        /// <param name="operateTime">申请单创建时间</param>
        /// <param name="operateType">数据维护操作类型</param>
        [Obsolete(@"请调用SaveRequestFormData(string executorCode, string macAddress, decimal applySerialNo, OrderInterfaceLogic.RequestFormCategory applyCategory,
         string executeDept, IList<OrderInterfaceLogic.RequestFormItem> itemArray, DateTime starTime, DateTime operateTime, RecordState operateType)")]
        public void SaveRequestFormData(string executorCode, string macAddress, decimal applySerialNo, string executeDept, string[] itemCodeArray, DateTime starTime, DateTime operateTime, RecordState operateType)
        {
            CheckCanInsertOrder(null, true, null);

            // 添加新申请单时，根据项目插入新临时医嘱，并保存关联的申请单号
            // 修改申请单时，清除原先插入的新临时医嘱，根据申请单重新插入临时医嘱
            // 删除申请单时，清除关联的新临时医嘱
            // 取消申请单时，取消关联的临时医嘱
            m_TempTable.DefaultView.BeginInit();
            if ((operateType == RecordState.Deleted) || (operateType == RecordState.Modified))
                DeleteOrderLinkedToRequest(applySerialNo);

            if ((operateType == RecordState.Added) || (operateType == RecordState.Modified))
                AddRequestItemIntoTempOrderTable(executorCode, applySerialNo, executeDept, itemCodeArray, starTime, operateTime);

            if (operateType == RecordState.Cancelled)
                CancelOrderLinkedToRequest(executorCode, applySerialNo, operateTime);
            m_TempTable.DefaultView.EndInit();

            // 保存临时医嘱数据
            // 检查数据是否满足约束
            Order[] changedOrders = m_TempTable.GetNewAndChangedRequestOrder();
            if ((changedOrders == null) || (changedOrders.Length == 0))
                return;
            CheckOrderData(m_TempTable, changedOrders, false, true);
            // 先存到本地，不能自动发送
            DoSaveChangedData(m_TempTable, changedOrders, executorCode, macAddress, false, false, false);
        }

        /// <summary>
        /// 保存申请单数据
        /// </summary>
        /// <param name="executorCode">申请单创建者代码</param>
        /// <param name="macAddress">调用者的网卡地址</param>
        /// <param name="applySerialNo">申请单序号(区分检验和检查申请单，采用+-号的方式，检验+，检查-)</param>
        /// <param name="applyCategory">申请单类别</param>
        /// <param name="executeDept">申请单执行科室</param>
        /// <param name="itemArray">申请单包含的项目代码</param>
        /// <param name="starTime">申请单开始时间</param>
        /// <param name="operateTime">申请单创建时间</param>
        /// <param name="operateType">数据维护操作类型</param>
        public void SaveRequestFormData(string executorCode, string macAddress, decimal applySerialNo, OrderInterfaceLogic.RequestFormCategory applyCategory,
           string executeDept, IList<OrderInterfaceLogic.RequestFormItem> itemArray, DateTime starTime, DateTime operateTime, RecordState operateType)
        {
            CheckCanInsertOrder(null, true, null);

            // 添加新申请单时，根据项目插入新临时医嘱，并保存关联的申请单号
            // 修改申请单时，清除原先插入的新临时医嘱，根据申请单重新插入临时医嘱
            // 删除申请单时，清除关联的新临时医嘱
            // 取消申请单时，取消关联的临时医嘱
            m_TempTable.DefaultView.BeginInit();

            applySerialNo = OrderInterfaceLogic.GetSend2HisApplySerialNo(applySerialNo, applyCategory);

            if ((operateType == RecordState.Deleted) || (operateType == RecordState.Modified))
                DeleteOrderLinkedToRequest(applySerialNo);

            if ((operateType == RecordState.Added) || (operateType == RecordState.Modified))
                AddRequestItemIntoTempOrderTable(executorCode, applySerialNo, executeDept, itemArray, starTime, operateTime);

            if (operateType == RecordState.Cancelled)
                CancelOrderLinkedToRequest(executorCode, applySerialNo, operateTime);
            m_TempTable.DefaultView.EndInit();

            // 保存临时医嘱数据
            // 检查数据是否满足约束
            Order[] changedOrders = m_TempTable.GetNewAndChangedRequestOrder();
            if ((changedOrders == null) || (changedOrders.Length == 0))
                return;
            CheckOrderData(m_TempTable, changedOrders, false, true);
            // 先存到本地，不能自动发送
            // DoSaveChangedData(m_TempTable, changedOrders, executorCode, macAddress, false, false, false);
            // wxg -- 2009-02-13 自动发送试试, 6院先开 autoDeleteNewOrder = true
            DoSaveChangedData(m_TempTable, changedOrders, executorCode, macAddress, false, true, true);
        }

        /// <summary>
        /// 保存当前编辑的成套的明细数据
        /// </summary>
        public void SaveCurrentSuiteDetailData()
        {
            // 将对象改变同步到Table 中
            try
            {
                Order[] changedOrders = m_TempTable.GetNewAndChangedOrders();
                m_TempTable.SyncObjectData2Table(changedOrders, true);

                changedOrders = m_LongTable.GetNewAndChangedOrders();
                m_LongTable.SyncObjectData2Table(changedOrders, true);

                // 调用 SuiteHelper 的 SaveDetail 方法
                SuiteHelper.SaveSuiteDetailData();

                m_TempTable.AcceptChanges();
                m_LongTable.AcceptChanges();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 重新同步未发送的医嘱到HIS中,供接口方法调用
        /// </summary>
        /// <param name="executorCode"></param>
        /// <param name="macAddress"></param>
        public void ResendSynchRecordsToHis(string executorCode, string macAddress)
        {
            OrderTable currentTable = GetCurrentOrderTable(true);
            SynchDataToHIS(currentTable, executorCode, macAddress);

            currentTable = GetCurrentOrderTable(false);
            SynchDataToHIS(currentTable, executorCode, macAddress);
        }

        /// <summary>
        /// 在同步执行结果后处理时限消息
        /// </summary>
        /// <param name="changedData">已执行或已停止的医嘱</param>
        /// <param name="isTemp">标记是否是临时医嘱</param>
        internal void HandleQcMessageAfterSynchExecute(DataTable changedData, bool isTemp)
        {
            // 停止危重医嘱后，要重新处理时限消息
            // 执行危重、转科、手术医嘱后，处理时限消息
            OrderTable table = new OrderTable(changedData, isTemp, m_SqlExecutor);
            OperationOrderContent opContent;

            if (isTemp)
            {
                foreach (Order order in table.Orders)
                {
                    switch (order.Content.OrderKind)
                    {
                        case OrderContentKind.TextShiftDept:
                            SendQcMessage(order.CreateInfo.Executor.Code, QCConditionType.AdviceChange, order, order.StartDateTime);
                            break;
                        case OrderContentKind.Operation:
                            opContent = order.Content as OperationOrderContent;
                            if (opContent != null)
                                SendQcMessage(order.CreateInfo.Executor.Code, QCConditionType.AdviceChange, order, opContent.OperationTime);
                            break;
                    }
                }
            }
            else
            {
                LongOrder longOrder;
                foreach (Order order in table.Orders)
                {
                    if ((order.Content.Item.Kind == ItemKind.DangerLevel)
                       && (order.Content.Item.Memo.Trim().Equals("1"))) // 1表示危重
                    {
                        if (order.State == OrderState.Executed)
                            SendQcMessage(order.CreateInfo.Executor.Code, QCConditionType.AdviceChange, order, order.StartDateTime);
                        else if (order.State == OrderState.Ceased)
                        {
                            longOrder = order as LongOrder;
                            SendQcMessage(longOrder.CeaseInfo.Executor.Code, QCConditionType.PatStateChange, CurrentPatient, longOrder.CeaseInfo.ExecuteTime);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
