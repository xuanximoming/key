using DrectSoft.Common.Eop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DrectSoft.Core.DoctorAdvice
{
    /// <summary>
    /// 医嘱接口处理类。提供医嘱审核、数据保存等处理接口
    /// </summary>
    public class OrderInterfaceLogic
    {
        #region public properties
        /// <summary>
        /// 当前处理的病人。在调用处理函数前要给此属性赋值
        /// </summary>
        public Inpatient CurrentPatient
        {
            get { return _currentPatient; }
            set
            {
                _currentPatient = value;
                if (value != null)
                {
                    if (m_CoreLogic == null)
                        IniializeCoreLogic(value);
                    else
                        m_CoreLogic.CurrentPatient = value;
                }
            }
        }
        private Inpatient _currentPatient;

        /// <summary>
        /// 病人在HIS中的首页序号。可以直接给此属性赋值，达到切换病人的目的
        /// </summary>
        public string FirstPageNoOfHis
        {
            get
            {
                if (CurrentPatient != null)
                    return CurrentPatient.NoOfHisFirstPage;
                else
                    return "-1";
            }
            set
            {
                CurrentPatient = CreatePatientByFirstpageNoOfHis(value);
            }
        }

        /// <summary>
        /// 调用者的网卡地址
        /// </summary>
        public string MacAddress
        {
            get { return _macAddress; }
            set { _macAddress = value; }
        }
        private string _macAddress;
        #endregion

        #region private variables & properties
        private CoreBusinessLogic m_CoreLogic;
        private IDataAccess m_SqlExecutor;

        private bool HadInitialized
        {
            get { return (CurrentPatient != null); }
        }
        #endregion

        #region ctors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlExecutor"></param>
        public OrderInterfaceLogic(IDataAccess sqlExecutor, string macAddress)
        {
            if (sqlExecutor == null)
                throw new ArgumentNullException("数据访问组件为空");
            m_SqlExecutor = sqlExecutor;
            _macAddress = macAddress;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlExecutor"></param>
        /// <param name="inpatient"></param>
        public OrderInterfaceLogic(IDataAccess sqlExecutor, string macAddress, Inpatient inpatient)
            : this(sqlExecutor, macAddress)
        {
            if (inpatient == null)
                throw new ArgumentNullException("空病人");

            CurrentPatient = inpatient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlExecutor"></param>
        /// <param name="firstpageNoOfHis">病人在HIS中的首页序号</param>
        public OrderInterfaceLogic(IDataAccess sqlExecutor, string macAddress, string firstpageNoOfHis)
            : this(sqlExecutor, macAddress)
        {
            if (firstpageNoOfHis == "-1")
                throw new ArgumentNullException("空病人");

            FirstPageNoOfHis = firstpageNoOfHis;
        }
        #endregion

        #region private methods
        private static int CompareSerialNoOfOrder(Order x, Order y)
        {
            if (x == null)
            {
                if (y == null)
                    return 0;
                else
                    return -1;
            }
            else
            {
                if (y == null)
                    return 1;
                else
                    return x.SerialNo.CompareTo(y.SerialNo);
            }
        }

        private static Order[] GetOrdesBySerialNo(OrderTable table, decimal[] serialNums)
        {
            if ((serialNums == null) || (serialNums.Length == 0))
                return new Order[] { };

            List<Order> orders = new List<Order>();
            Order order;
            foreach (decimal serialNo in serialNums)
            {
                order = table.Orders[table.Orders.IndexOf(serialNo)];
                if (order != null)
                    orders.Add(order);
            }
            // 按照序号进行排序
            orders.Sort(CompareSerialNoOfOrder);
            Order[] result = new Order[orders.Count];
            orders.CopyTo(result);
            return result;
        }

        private Inpatient CreatePatientByFirstpageNoOfHis(string firstpageNoOfHis)
        {
            decimal firstpageNo = (decimal)m_SqlExecutor.ExecuteScalar(
               String.Format("select syxh from BL_BRSYK where hissyxh = {0}", firstpageNoOfHis));

            Inpatient inpatient = new Inpatient(firstpageNo);
            inpatient.NoOfHisFirstPage = firstpageNoOfHis;

            return inpatient;
        }

        private void IniializeCoreLogic(Inpatient patient)
        {
            if (patient != null)
            {
                m_CoreLogic = new CoreBusinessLogic(m_SqlExecutor, EditorCallModel.EditOrder);
                m_CoreLogic.CurrentPatient = patient;
            }
            else
                throw new ArgumentNullException("患者未赋值");
        }

        private string CombineSerialNo(DataTable dataTable, string serialNoField)
        {
            StringBuilder serialNoLine = new StringBuilder("0");
            foreach (DataRow row in dataTable.Rows)
                serialNoLine.Append("," + row[serialNoField].ToString());
            return serialNoLine.ToString();
        }

        private void SynchLongOrderExecResultToEmr(DataTable longOrderTable)
        {
            if ((longOrderTable != null) && (longOrderTable.Rows.Count > 0))
            {
                OrderTable currentTable = m_CoreLogic.GetCurrentOrderTable(false);
                if (currentTable.Orders.Count == 0)
                {
                    if (CoreBusinessLogic.BusinessLogic.UsedForAllPatient)
                        throw new ArgumentOutOfRangeException("传入的长期医嘱数据在电子病历系统中不存在");
                    else
                        return;
                }
                currentTable.OrderDataTable.Merge(longOrderTable);
                string changedSerialNo = CombineSerialNo(longOrderTable, "cqyzxh");
                DataRow[] changedRows = currentTable.OrderDataTable.Select("cqyzxh in (" + changedSerialNo + ")");
                Dictionary<decimal, DataRow> changedHerbSummaries = new Dictionary<decimal, DataRow>();

                foreach (DataRow row in changedRows)
                {
                    if (row["memo"].ToString().StartsWith(Order.HerbSummaryFlag))
                        changedHerbSummaries.Add(Convert.ToDecimal(row["memo"].ToString().Substring(Order.HerbSummaryFlag.Length - 1)), row);
                }
                // 同步关联的草药明细的状态
                if (changedHerbSummaries.Count > 0)
                {
                    decimal groupSerialNo;

                    foreach (DataRow row in currentTable.OrderDataTable.Rows)
                    {
                        groupSerialNo = Convert.ToDecimal(row["fzxh"]);
                        if (changedHerbSummaries.ContainsKey(groupSerialNo))
                        {
                            row["yzzt"] = changedHerbSummaries[groupSerialNo]["yzzt"];
                            row["zxczy"] = changedHerbSummaries[groupSerialNo]["zxczy"];
                            row["zxrq"] = changedHerbSummaries[groupSerialNo]["zxrq"];
                        }
                    }
                }

                DataTable changedTable = currentTable.OrderDataTable.GetChanges();
                m_SqlExecutor.UpdateTable(currentTable.OrderDataTable, CoreBusinessLogic.GetOrderTableName(currentTable.IsTempOrder), false);
                //currentTable.OrderDataTable.AcceptChanges();
                // 处理质量管理消息
                m_CoreLogic.HandleQcMessageAfterSynchExecute(changedTable, false);
            }
        }

        private void SynchTempOrderExecResultToEmr(DataTable tempOrderTable)
        {
            // 直接对数据集进行合并，然后保存到EMR数据库(这样会导致OrderTable中数据集内容和对象属性不一致)
            if ((tempOrderTable != null) && (tempOrderTable.Rows.Count > 0))
            {
                OrderTable currentTable = m_CoreLogic.GetCurrentOrderTable(true);
                if (currentTable.Orders.Count == 0)
                {
                    if (CoreBusinessLogic.BusinessLogic.UsedForAllPatient)
                        throw new ArgumentOutOfRangeException("传入的临时医嘱数据在电子病历系统中不存在");
                    else
                        return;
                }
                currentTable.OrderDataTable.Merge(tempOrderTable);

                string changedSerialNo = CombineSerialNo(tempOrderTable, "lsyzxh");
                DataRow[] changedRows = currentTable.OrderDataTable.Select("lsyzxh in (" + changedSerialNo + ")");

                Dictionary<decimal, DataRow> changedHerbSummaries = new Dictionary<decimal, DataRow>();
                StringBuilder reqSerialNos = new StringBuilder("0");
                foreach (DataRow row in changedRows)
                {
                    if ((row["sqdxh"].ToString() != "") && (Convert.ToDecimal(row["sqdxh"]) != 0))
                        reqSerialNos.Append("," + row["sqdxh"].ToString());
                    else if (row["memo"].ToString().StartsWith(Order.HerbSummaryFlag))
                        changedHerbSummaries.Add(Convert.ToDecimal(row["memo"].ToString().Substring(Order.HerbSummaryFlag.Length - 1)), row);
                }
                // 同步关联的草药明细的状态
                if (changedHerbSummaries.Count > 0)
                {
                    decimal groupSerialNo;
                    foreach (DataRow row in currentTable.OrderDataTable.Rows)
                    {
                        groupSerialNo = Convert.ToDecimal(row["fzxh"]);
                        if (changedHerbSummaries.ContainsKey(groupSerialNo))
                        {
                            row["yzzt"] = changedHerbSummaries[groupSerialNo]["yzzt"];
                            row["zxczy"] = changedHerbSummaries[groupSerialNo]["zxczy"];
                            row["zxrq"] = changedHerbSummaries[groupSerialNo]["zxrq"];
                        }
                    }
                }

                DataTable changedTable = currentTable.OrderDataTable.GetChanges();
                m_SqlExecutor.UpdateTable(currentTable.OrderDataTable, CoreBusinessLogic.GetOrderTableName(currentTable.IsTempOrder), false);
                //currentTable.OrderDataTable.AcceptChanges();
                // 将关联的申请单状态改成已执行 
                if (reqSerialNos.Length > 1)
                    m_SqlExecutor.ExecuteNoneQuery("update BL_SQDK set qrbz = 2 where xh in (" + reqSerialNos.ToString() + ")");

                // 处理质量管理消息
                m_CoreLogic.HandleQcMessageAfterSynchExecute(changedTable, true);
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// EMR医嘱审核接口,返回的异常中包含了审核不通过的原因。
        /// 传入的调用者工号和网卡地址在调用HIS接口是要用到，要避免冲突。
        /// </summary>
        /// <param name="executorCode">调用审核过程的操作员工号</param>
        /// <param name="macAddress">网卡地址</param>
        /// <param name="selectedLongs">要审核的长期医嘱序号集合</param>
        /// <param name="selectedTemps">要审核的临时医嘱序号集合</param>
        /// <param name="auditor">审核者工号</param>
        /// <param name="auditTime">审核时间</param>
        public void AuditOrder(string executorCode, string macAddress, decimal[] selectedLongs, decimal[] selectedTemps, string auditor, DateTime auditTime)
        {
            if (!CoreBusinessLogic.BusinessLogic.EnableEmrOrderModul)
                return;

            if (!HadInitialized)
                throw new ArgumentException(String.Format(ConstMessages.ExceptionFormatNullObject, ConstNames.Inpatient));

            // 首先做初步检查，然后执行审核的过程，最后保存更新后的数据（存到本地，并同步到HIS）
            try
            {
                OrderTable currentTable;
                Order[] selectedOrders = null;
                if (selectedTemps.Length > 0)
                {
                    currentTable = m_CoreLogic.GetCurrentOrderTable(true);
                    if (currentTable.Orders.Count == 0)
                    {
                        if (CoreBusinessLogic.BusinessLogic.UsedForAllPatient)
                            throw new ArgumentOutOfRangeException("传入的临时医嘱数据在电子病历系统中不存在");
                        else
                            return;
                    }
                    //currentTable.DefaultView.BeginInit();
                    selectedOrders = GetOrdesBySerialNo(currentTable, selectedTemps);
                    m_CoreLogic.AuditOrder(selectedOrders, auditor, auditTime, currentTable.IsTempOrder);
                    //currentTable.DefaultView.EndInit();
                }
                if (selectedLongs.Length > 0)
                {
                    currentTable = m_CoreLogic.GetCurrentOrderTable(false);
                    if (currentTable.Orders.Count == 0)
                    {
                        if (CoreBusinessLogic.BusinessLogic.UsedForAllPatient)
                            throw new ArgumentOutOfRangeException("传入的长期医嘱数据在电子病历系统中不存在");
                        else
                            return;
                    }
                    //currentTable.DefaultView.BeginInit();
                    m_CoreLogic.AuditOrder(GetOrdesBySerialNo(currentTable, selectedLongs), auditor, auditTime, currentTable.IsTempOrder);
                    //currentTable.DefaultView.EndInit();
                }
                // 保存数据
                m_CoreLogic.SaveAllChangedOrderData(executorCode, macAddress, true);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 同步医嘱执行结果接口(供HIS调用)
        /// 传入的数据集中包括
        ///       cqyzxh  utXh12      --长期医嘱序号 （或者 lsyzxh）
        /// 	zxczy   utMc16      --执行操作员(代码)
        /// 	zxrq    utDatetime  --执行日期
        ///       yzzt    utBz        --医嘱状态(EMR中定义的状态)
        /// </summary>
        /// <param name="longOrderTable">已执行的长期医嘱数据集</param>
        /// <param name="tempOrderTable">已执行的临时医嘱数据集</param>
        public void SynchExecuteResultToEmr(DataTable longOrderTable, DataTable tempOrderTable)
        {
            if (!CoreBusinessLogic.BusinessLogic.EnableEmrOrderModul)
                return;

            SynchTempOrderExecResultToEmr(tempOrderTable);

            SynchLongOrderExecResultToEmr(longOrderTable);
        }

        /// <summary>
        /// 保存申请单数据(过期)
        /// </summary>
        /// <param name="executorCode">申请单创建者代码</param>
        /// <param name="applySerialNo">申请单序号</param>
        /// <param name="executeDept">申请单执行科室</param>
        /// <param name="itemCodeArray">申请单包含的项目代码</param>
        /// <param name="starTime">申请单开始时间</param>
        /// <param name="operateTime">申请单创建时间</param>
        /// <param name="operateType">数据维护操作类型</param>
        [Obsolete(@"调用新方法SaveRequestFormData(string executorCode, decimal applySerialNo, RequestFormCategory applyCategory,  
         string executeDept, IList<RequestFormItem> itemArray, DateTime starTime, DateTime operateTime, RecordState operateType)")]
        public void SaveRequestFormData(string executorCode, decimal applySerialNo, string executeDept, string[] itemCodeArray, DateTime starTime, DateTime operateTime, RecordState operateType)
        {
            if (!CoreBusinessLogic.BusinessLogic.EnableEmrOrderModul)
                return;
            m_CoreLogic.SaveRequestFormData(executorCode, MacAddress, applySerialNo, executeDept, itemCodeArray, starTime, operateTime, operateType);
        }

        /// <summary>
        /// 保存申请单数据(支持临床项目)
        /// </summary>
        /// <param name="executorCode">申请单创建者代码</param>
        /// <param name="applySerialNo">申请单序号</param>
        /// <param name="executeDept">申请单执行科室</param>
        /// <param name="itemArray">申请单包含的项目代码</param>
        /// <param name="starTime">申请单开始时间</param>
        /// <param name="operateTime">申请单创建时间</param>
        /// <param name="operateType">数据维护操作类型</param>
        public void SaveRequestFormData(string executorCode, decimal applySerialNo, RequestFormCategory applyCategory,
           string executeDept, IList<RequestFormItem> itemArray, DateTime starTime, DateTime operateTime, RecordState operateType)
        {
            if (!CoreBusinessLogic.BusinessLogic.EnableEmrOrderModul)
                return;

            m_CoreLogic.SaveRequestFormData(executorCode, MacAddress, applySerialNo, applyCategory, executeDept, itemArray, starTime, operateTime, operateType);
        }

        public static decimal GetSend2HisApplySerialNo(decimal applySerialNo, OrderInterfaceLogic.RequestFormCategory applyCategory)
        {
            switch (applyCategory)
            {
                case OrderInterfaceLogic.RequestFormCategory.CL:
                    return -applySerialNo;
                default:
                    break;
            }

            return applySerialNo;
        }

        /// <summary>
        /// 获取指定医生为当前病人在指定时间之后新开的药品和治疗项目医嘱
        /// </summary>
        /// <param name="doctorCode">医生工号</param>
        /// <param name="startTime">指定的时间</param>
        /// <returns></returns>
        public DataTable GetNewOrder(string doctorCode, DateTime startTime)
        {
            return m_CoreLogic.GetNewOrder(doctorCode, startTime);
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
        /// 自动处理长期医嘱的状态，在病人状态由在病区变成转科或出区（以及撤销状态改变操作）时调用
        /// </summary>
        /// <param name="ceaeReason">停医嘱的原因</param>
        /// <param name="isRollback">标记是否是撤销病人状态改变操作时调用</param>
        public void AutoHandleLongOrderState(string executorCode, OrderCeaseReason ceaseReason, bool isRollback)
        {
            if (!CoreBusinessLogic.BusinessLogic.EnableEmrOrderModul)
                return;

            LongOrder ltemp;
            DateTime rollbackTime = DateTime.Now;
            foreach (Order order in m_CoreLogic.GetCurrentOrderTable(false).Orders)
            {
                if ((order.State == OrderState.Audited) || (order.State == OrderState.New))
                    break;
                ltemp = order as LongOrder;
                if ((ltemp.State == OrderState.Executed) && (!isRollback))
                {
                    // 将执行状态直接改成停止状态
                    ltemp.SetStateToCeased();
                }
                else if ((order.State == OrderState.Ceased) && (isRollback))
                {
                    // 如果停止原因和传入原因一致，且无停止时间或停止时间晚于当前日期，则将停止状态改成执行状态
                    ltemp.RollbackStateOfCeasedOrder(ceaseReason, rollbackTime);
                }
            }

            // 保存改变的数据(自动同步到HIS中)
            m_CoreLogic.SaveAllChangedOrderData(executorCode, MacAddress, true);
        }

        /// <summary>
        /// 刷新当前病人的医嘱数据
        /// </summary>
        public void RefreshPatientData()
        {
            CurrentPatient = CurrentPatient;
        }

        /// <summary>
        /// 重新同步未发送的医嘱到HIS中
        /// TODO：现在同步的处理还不是很严格。还需仔细研究如何保证EMR和HIS之间的数据一致性
        /// </summary>
        /// <param name="executorCode"></param>
        /// <param name="macAddress"></param>
        public void ResendSynchRecords(string executorCode, string macAddress)
        {
            if (!CoreBusinessLogic.BusinessLogic.EnableEmrOrderModul)
                return;
            m_CoreLogic.ResendSynchRecordsToHis(executorCode, macAddress);
        }

        /// <summary>
        /// 将指定的成套医嘱做为新医嘱插入
        /// </summary>
        /// <param name="suiteSerialNo"></param>
        public void InsertSuiteOrderAsNewOrder(string doctor, decimal suiteSerialNo)
        {
            if (suiteSerialNo <= 0)
                return;
            DataTable detailTable = m_SqlExecutor.ExecuteDataTable("select * from YY_CTYZMXK where ctyzxh = " + suiteSerialNo.ToString()
               , CommandType.Text);

            // 先处理临时医嘱
            DataRow[] selectedRows = detailTable.Select(ConstSchemaNames.SuiteDetailColAmount + " > 0 and yzbz in (2700, 2702)");
            InsertTempSuiteOrderIntoTable(doctor, selectedRows, true);
            selectedRows = detailTable.Select(ConstSchemaNames.SuiteDetailColAmount + " > 0 and yzbz in (2700, 2703)");
            InsertTempSuiteOrderIntoTable(doctor, selectedRows, false);
            m_CoreLogic.SaveAllChangedOrderData(doctor, MacAddress, true);

        }

        private void InsertTempSuiteOrderIntoTable(string doctor, DataRow[] selectedRows, bool isTempOrder)
        {
            if (selectedRows.Length == 0)
                return;

            object[,] selectedContents = new object[selectedRows.Length, 2];

            // 暂不处理出院带药bool needCalcTotalAmount = false ; 
            string checkMsg;
            DataRow row;
            OrderContent content;
            try
            {
                for (int index = 0; index < selectedRows.Length; index++)
                {
                    row = selectedRows[index];
                    selectedContents[index, 1] = (GroupPositionKind)Convert.ToInt32(row[ConstSchemaNames.SuiteDetailColGroupFlag]);
                    content = PersistentObjectFactory.CreateAndIntializeObject(
                       OrderContentFactory.GetOrderContentClassName(row[ConstSchemaNames.SuiteDetailColOrderCatalog]), row) as OrderContent;
                    content.ProcessCreateOutputeInfo = new OrderContent.GenerateOutputInfo(CustomDrawOperation.CreateOutputeInfo);

                    //checkMsg = content.CheckProperties();
                    //if (!String.IsNullOrEmpty(checkMsg))
                    //   m_MessageBox.MessageShow(checkMsg, CustomMessageBoxKind.InformationOk);

                    selectedContents[index, 0] = content;
                }
                //// TODO: 暂时跳过医嘱检查
                m_CoreLogic.CheckCanInsertOrder(null, isTempOrder, null);
                m_CoreLogic.InsertSuiteOrder(m_CoreLogic.GetCurrentOrderTable(isTempOrder), doctor, selectedContents, null);
            }
            catch { }
        }
        #endregion

        #region Class for RequestForm FeeItems Save

        /// <summary>
        /// 用以保存的申请单对应的收费项目信息
        /// </summary>
        public class RequestFormItem
        {
            string _itemCode;
            int _itemKind;
            private string _memo;
            private string _specimen;
            private string _specimenId;
            private int _jzbz;
            private string _execDept;

            /// <summary>
            /// 项目代码
            /// </summary>
            public string Code { get { return _itemCode; } }

            /// <summary>
            /// 项目类别
            /// </summary>
            public int Kind { get { return _itemKind; } }

            /// <summary>
            /// 申请单备注
            /// </summary>
            public string Memo { get { return _memo; } }

            /// <summary>
            /// 标本
            /// </summary>
            public string Specimen { get { return _specimen; } }

            /// <summary>
            /// 标本
            /// </summary>
            public string SpecimenId { get { return _specimenId; } }

            /// <summary>
            /// 加急标志
            /// </summary>
            public int Urgent { get { return _jzbz; } }

            /// <summary>
            /// 执行科室
            /// </summary>
            public string ExecDept { get { return _execDept; } }

            /// <summary>
            /// 构造一个申请单对应的收费项目
            /// </summary>
            /// <param name="code">项目代码</param>
            /// <param name="kind">0 收费小项目 1 临床项目</param>
            public RequestFormItem(string code, int kind)
            {
                _itemCode = code;
                _itemKind = kind;
            }

            public RequestFormItem(string code, int kind, string specimenId, string specimen, int urgent, string memo)
            {
                this._itemCode = code;
                this._itemKind = kind;
                this._specimenId = specimenId;
                this._specimen = specimen;
                this._memo = memo;
                this._jzbz = urgent;
            }

            public RequestFormItem(string code, int kind, string specimenId, string specimen, int urgent, string memo, string execDept)
            {
                this._itemCode = code;
                this._itemKind = kind;
                this._specimenId = specimenId;
                this._specimen = specimen;
                this._memo = memo;
                this._jzbz = urgent;
                this._execDept = execDept;
            }
        }

        public enum RequestFormCategory
        {
            /// <summary>
            /// 检验
            /// </summary>
            TF = 0,

            /// <summary>
            /// 检查
            /// </summary>
            CL = 1,
        }
        #endregion
    }
}
