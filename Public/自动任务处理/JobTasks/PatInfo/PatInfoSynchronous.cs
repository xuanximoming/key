using DrectSoft.Common.Eop;
using DrectSoft.Core;
using DrectSoft.Core.TimeLimitQC;
using DrectSoft.JobManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Text;

[assembly: Job("住院病人和床位数据同步", "从HIS读取病人和床位数据", "电子病历", true, typeof(PatInfoSynchronous))]
namespace DrectSoft.JobManager
{
    /// <summary>
    /// 对HIS与EMR中病人基本信息和床位数据进行同步处理
    /// </summary>
    public class PatInfoSynchronous : BaseJobAction, IDisposable
    {
        #region const
        private const string EMRPatTable = "INPATIENT";
        private const string EMRBedTable = "BED";
        private const string colFirstPageNo = "NOOFINPAT";
        private const string colHisFirstPageNo = "PATNOOFHIS";
        private const string colPatientState = "STATUS";
        private const string colBedDoctor = "Resident";
        private const string colTimeOfInWard = "InWardDate";
        private const string colInWard = "AdmitWard";
        private const string colInDept = "AdmitDept";
        private const string colInBed = "AdmitBed";
        private const string colTimeOfLeaveWard = "OutWardDate";
        private const string colFinalyWard = "OutHosWard";
        private const string colFinalyDept = "OutHosDept";
        private const string colFinalyBed = "OutBed";
        private const string colBedNo = "ID";
        private const string colBedState = "InBed";
        private const string colDeptCode = "DeptID";
        private const string colWardCode = "WardId";
        private const string colInsertDate = "crrq";

        #endregion

        #region new properties
        /// <summary>
        /// 有初始化动作
        /// </summary>
        public override bool HasInitializeAction { get { return true; } }
        /// <summary>
        /// 取得配置中要跳过的列
        /// </summary>
        private string m_UpdateColumns;
        #endregion

        #region private variables & properties
        /// <summary>
        /// 标记 Dispose 方法是否被调用过
        /// </summary>
        private bool m_disposed;

        private DataTable m_EmrPatientTable;
        private DataTable m_EmrBedTable;
        private IDataAccess m_EmrHelper;
        private IDataAccess m_HisHelper;

        private string MacAddress
        {
            get
            {
                if (String.IsNullOrEmpty(_macAddress))
                {
                    ManagementClass mcMAC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection mocMAC = mcMAC.GetInstances();
                    string macAdd = string.Empty;
                    foreach (ManagementObject m in mocMAC)
                    {
                        if ((bool)m["IPEnabled"])
                        {
                            macAdd = m["MacAddress"].ToString().Replace(":", "");
                            break;
                        }
                    }

                    _macAddress = macAdd;
                }
                return _macAddress;
            }
        }
        private string _macAddress;


        //private OrderInterfaceLogic m_OrderInterface;
        /// <summary>
        /// 同步病人时记录新入院的病人，以便发送质量监控的触发消息
        /// </summary>
        private Dictionary<string, DataRow> m_NewPatients;
        private Dictionary<string, DataRow> m_NewBeds; // 记录需要更新床位表中的首页序号和首页表中床号的床位
        private Qcsv m_Qcsv;
        //private ISynchApplication m_App; //by ukey 2016-08-19 屏蔽警告
        #endregion

        #region ctor & dispose
        /// <summary>
        /// 
        /// </summary>
        public PatInfoSynchronous()
        {
            try
            {

                m_EmrHelper = DataAccessFactory.DefaultDataAccess;
                m_HisHelper = DataAccessFactory.GetSqlDataAccess("HISDB");
                m_NewPatients = new Dictionary<string, DataRow>();
                m_NewBeds = new Dictionary<string, DataRow>();
                try
                {
                    m_Qcsv = new Qcsv();
                }
                catch
                {
                    m_Qcsv = null;
                }

                m_EmrPatientTable = m_EmrHelper.ExecuteDataTable("select * from " + EMRPatTable + " where 1=2");
                m_EmrHelper.ResetTableSchema(m_EmrPatientTable, EMRPatTable);

                m_EmrBedTable = m_EmrHelper.ExecuteDataTable("select * from " + EMRBedTable + " where 1=2");
                m_EmrHelper.ResetTableSchema(m_EmrBedTable, EMRBedTable);
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.m_disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    m_EmrBedTable.Dispose();
                    m_EmrPatientTable.Dispose();
                }
            }
            m_disposed = true;
        }

        ~PatInfoSynchronous()
        {
            Dispose(false);
        }
        #endregion

        #region private method
        /// <summary>
        /// 初始化Emr病人数据
        /// </summary>
        /// <param name="synchChanged"></param>
        /// <param name="condition"></param>
        private void InitEmrInpatientTable(bool synchChanged, string condition)
        {
            m_NewPatients.Clear();
            string sqlCommand = string.Empty;
            if (!synchChanged) // 初始化病人
            {
                sqlCommand =
                String.Format(CultureInfo.CurrentCulture
                             , "select * from {0} a where Status in ({1:D},{2:D},{3:D},{4:D},{5:D},{9:D}) or (Status in ({6:D}, {7:D}) and (OutWardDate > '{8}' or OutWardDate is null))"
                             , EMRPatTable
                             , InpatientState.InWard
                             , InpatientState.OutWard
                             , InpatientState.InICU
                             , InpatientState.InDeliveryRoom
                             , InpatientState.ShiftDept
                             , InpatientState.Balanced
                             , InpatientState.CancleBalanced
                             , DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd")
                             , InpatientState.New);
            }
            else // 同步信息有变化的病人
            {
                sqlCommand = String.Format(CultureInfo.CurrentCulture, "select * from {0} where 1=1", EMRPatTable);
                sqlCommand = sqlCommand + " and " + condition;
            }

            //if (!String.IsNullOrEmpty(condition))
            //    sqlCommand = sqlCommand + " and " + condition;

            m_EmrPatientTable.Clear();

            DataTable table = m_EmrHelper.ExecuteDataTable(sqlCommand, CommandType.Text);
            m_EmrPatientTable.Merge(table, false, MissingSchemaAction.Ignore);
        }

        private void InitEmrBedTable()
        {
            m_NewBeds.Clear();
            InitEmrBedTable("");
        }

        private void InitEmrBedTable(string condition)
        {
            string sqlCommand = "select * from " + EMRBedTable;

            if (!String.IsNullOrEmpty(condition))
                sqlCommand = sqlCommand + " where " + condition;

            m_EmrBedTable.Clear();
            //JobLogHelper.WriteSqlLog(Parent, sqlCommand);
            DataTable table = m_EmrHelper.ExecuteDataTable(sqlCommand, CommandType.Text);
            m_EmrBedTable.Merge(table, false, MissingSchemaAction.Ignore);
        }

        private void AddNewRecord(DataTable targetTable, DataRow sourceRow, bool isPatient)
        {
            DataRow newRow = targetTable.NewRow();
            InpatientState patState;
            BedState bedState;
            foreach (DataColumn column in sourceRow.Table.Columns)
            {
                if (column.ColumnName == colInsertDate)
                    continue;
                if (!targetTable.Columns.Contains(column.ColumnName.ToUpper()))
                    continue;
                newRow[column.ColumnName] = sourceRow[column.ColumnName.ToUpper()];
                if (isPatient)
                {
                    if ((column.ColumnName == colPatientState.ToUpper()) || (column.ColumnName == colPatientState.ToLower()))
                    {
                        patState = (InpatientState)Convert.ToInt32(newRow[colPatientState]);
                        if (patState == InpatientState.InWard)
                            m_NewPatients.Add(newRow[colHisFirstPageNo].ToString(), newRow);
                    }
                }
                else if ((column.ColumnName.Equals(colBedState.ToUpper())) || (column.ColumnName.Equals(colBedState.ToLower())))
                {
                    bedState = (BedState)Convert.ToInt32(newRow[colBedState]);
                    if ((bedState == BedState.Yes) || (bedState == BedState.Wrapped))
                    {
                        string key = GetNewBedKey(sourceRow);
                        if (!m_NewBeds.ContainsKey(key))
                            m_NewBeds.Add(key, newRow);
                    }
                }
            }
            targetTable.Rows.Add(newRow);
        }

        private void UpdateRecord(DataRow sourceRow, DataRow targetRow, bool initialized, bool isPatient)
        {
            BedState bedState;

            foreach (DataColumn col in sourceRow.Table.Columns)
            {
                if (col.ColumnName == colInsertDate)
                    continue;
                // 更新时跳过syxh字段（因为此字段从HIS返回的是默认值，只在插入新病人和新床位时作为默认值提供）
                // 若不跳过此字段，则在某些情况下会错误的更新数据，比如：
                //    在HIS中将病人从A床换到B床，马上再换回来（在同步床位数据的一个周期内）。执行第一次
                //    换床动作时，会将这两张床号插入同步消息表，第二次换床动作则不会插入消息表(因为已经
                //    记录过这两张床状态要更新)。同步数据时，由于床位信息前后是一样的，所以其它字段值都
                //    不会更新，也不就会将A床记下来等待重新更新syxh字段。但是从HIS获得的变更数据集中syxh为-1，
                //    从而将正确的syxh值更新成-1了。
                if (col.ColumnName.Equals(colFirstPageNo))
                    continue;
                if (!targetRow.Table.Columns.Contains(col.ColumnName.ToUpper()))
                    continue;
                if (sourceRow[col.ColumnName].ToString().Trim()
                       == targetRow[col.ColumnName.ToUpper()].ToString().Trim())
                    continue;

                //*************************add by ywk **************************************
                //取得系统配置中的，在数据同步时的要更新配置的列（配置的列在电子病历表中不进行更新）
                string[] updatecolumns = m_UpdateColumns.Split(',');
                bool isContinue = false;
                foreach (string item in updatecolumns)
                {
                    try
                    {
                        if (item.Trim().ToLower() == col.ColumnName.Trim().ToLower())
                        {
                            if (targetRow.Table.Columns.Contains(item.Trim()))
                            {
                                if (!string.IsNullOrEmpty(targetRow[item.Trim()].ToString()))//有值就不更新
                                {
                                    isContinue = true;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                if (isContinue) continue;
                //**************************************************************************

                if (isPatient) // 更新病人
                {
                    // 跳过入院科室、入院病区、入院床位的更新
                    //if ((col.ColumnName == colInDept.ToUpper()) || (col.ColumnName == colInWard.ToUpper()) || (col.ColumnName == colInBed.ToUpper()))
                    //可暂时去除跳过入院科室、入院病区，入院床位不跳过，中心医院存在大量入院床位为空的
                    //add by ywk 2013年4月26日16:12:05
                    if ((col.ColumnName == colInDept.ToUpper()) || (col.ColumnName == colInWard.ToUpper()))
                        continue;

                    // 在病人状态由在病区变成转科或出区（以及撤销状态改变操作）时要处理长期医嘱的状态
                    if (!initialized && (col.ColumnName == colPatientState.ToUpper()))
                        DoAfterPatientStateChanged(sourceRow, targetRow);

                    targetRow[col.ColumnName] = sourceRow[col.ColumnName.ToUpper()];
                }
                else // 更新床位
                {
                    targetRow[col.ColumnName] = sourceRow[col.ColumnName.ToUpper()];
                    bedState = BedState.None;
                    if (col.ColumnName.Equals(colBedState))
                    {
                        bedState = (BedState)Convert.ToInt32(sourceRow[colBedState]);
                    }
                    // 如果记录是由空床变成占床或包床或所在的病人改变或了，则需记录下来，以便处理床位表关联的EMR首页序号和EMR首页库的床号
                    if ((bedState == BedState.Yes) || (bedState == BedState.Wrapped) || col.ColumnName.Equals(colHisFirstPageNo))
                    {
                        string key = GetNewBedKey(sourceRow);
                        if (!m_NewBeds.ContainsKey(key))
                            m_NewBeds.Add(key, sourceRow);
                    }
                }
            }
        }

        #region 根据系统配置的KEY取得具体的值（appcfg）ywk
        /// <summary>
        /// 得到配置信息（泗县修改 2012年5月9日15:29:40  ywk）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigValueByKey(string key)
        {
            string sql1 = " select * from appcfg where configkey = '" + key + "'; ";
            DataTable dt = m_EmrHelper.ExecuteDataTable(sql1, CommandType.Text);
            string config = string.Empty;
            if (dt.Rows.Count > 0)
            {
                config = dt.Rows[0]["value"].ToString();
            }
            return config;
        }
        #endregion


        private string GetNewBedKey(DataRow sourceRow)
        {
            return sourceRow[colWardCode].ToString() + "," + sourceRow[colBedNo].ToString();
        }

        private void DoAfterPatientStateChanged(DataRow sourceRow, DataRow targetRow)
        {
            InpatientState oldState = (InpatientState)Enum.Parse(typeof(InpatientState)
               , targetRow[colPatientState].ToString());
            InpatientState newState = (InpatientState)Enum.Parse(typeof(InpatientState)
               , sourceRow[colPatientState].ToString());

            bool isRollback = false;
            OrderCeaseReason ceaseReason = OrderCeaseReason.None;
            bool needHandle = false; // 标记是否需要处理长期医嘱的状态

            if (oldState == InpatientState.InWard)
            {
                switch (newState)
                {
                    case InpatientState.ShiftDept:
                        // 转科
                        isRollback = false;
                        ceaseReason = OrderCeaseReason.ShiftDept;
                        needHandle = true;
                        // 发送转科消息
                        SendQCMessage(targetRow, targetRow[colBedDoctor].ToString(), oldState
                           , Convert.ToDateTime(sourceRow[colTimeOfLeaveWard]));
                        break;
                    case InpatientState.OutWard:
                        // 出区
                        isRollback = false;
                        ceaseReason = OrderCeaseReason.LeaveHospital;
                        needHandle = true;
                        // 发送出院消息
                        SendQCMessage(targetRow, targetRow[colBedDoctor].ToString(), oldState
                           , Convert.ToDateTime(sourceRow[colTimeOfLeaveWard]));
                        break;
                }
            }
            else if (newState == InpatientState.InWard)
            {
                switch (oldState)
                {
                    case InpatientState.ShiftDept:
                    case InpatientState.InICU:
                    case InpatientState.InDeliveryRoom:
                        if ((targetRow[colFinalyWard].ToString() == sourceRow[colFinalyWard].ToString())
                           && (targetRow[colFinalyDept].ToString() == sourceRow[colFinalyDept].ToString()))
                        {
                            // 撤销转科
                            isRollback = true;
                            ceaseReason = OrderCeaseReason.ShiftDept;
                            needHandle = true;
                        }
                        else
                        {
                            // 如果是转科，则发送转入消息
                            SendQCMessage(targetRow, targetRow[colBedDoctor].ToString(), oldState, DateTime.Now);
                        }
                        break;
                    case InpatientState.OutWard:
                    case InpatientState.CancleBalanced:
                        // 出区召回
                        isRollback = true;
                        ceaseReason = OrderCeaseReason.LeaveHospital;
                        needHandle = true;
                        break;
                }
            }
            else if (newState == InpatientState.Balanced)
            {
                if (oldState != InpatientState.Balanced) // 发送出院消息
                    SendQCMessage(targetRow, targetRow[colBedDoctor].ToString(), InpatientState.OutWard, DateTime.Parse(sourceRow["OutWardDate"].ToString()));
            }
            if (needHandle)
                AutoHandleLongOrderState(Convert.ToDecimal(targetRow[colHisFirstPageNo]), "00", ceaseReason, isRollback);
        }

        private void AutoHandleLongOrderState(decimal firstpageNoOfHis, string executorCode, OrderCeaseReason ceaseReason, bool isRollback)
        {
            //try
            //{
            //    if (m_OrderInterface == null)
            //        m_OrderInterface = new OrderInterfaceLogic(m_EmrHelper, MacAddress, firstpageNoOfHis);
            //    else
            //        m_OrderInterface.FirstPageNoOfHis = firstpageNoOfHis;
            //    m_OrderInterface.AutoHandleLongOrderState(executorCode, ceaseReason, isRollback);
            //}
            //catch (Exception err)
            //{
            //    m_App.WriteLog(new JobExecuteInfoArgs(Parent, "医嘱", err));
            //}
        }

        private string MergePatientInfo(DataTable changedData, bool initialized)
        {
            m_NewPatients.Clear();
            DataRow[] matchRows;
            DataRow[] matchRows2;
            string maxInsertDate = "";
            try
            {
                foreach (DataRow hisRow in changedData.Rows)
                {
                    // 第一条记录保存的就是最大的插入日期
                    if (!initialized && String.IsNullOrEmpty(maxInsertDate))
                        maxInsertDate = hisRow[colInsertDate].ToString();

                    matchRows = m_EmrPatientTable.Select(String.Format(" PATNOOFHIS = '{0}' ", hisRow[colHisFirstPageNo].ToString().Trim()), " PATNOOFHIS asc ");
                    switch (matchRows.Length)
                    {
                        case 0:  // 插入
                            AddNewRecord(m_EmrPatientTable, hisRow, true);
                            break;
                        case 1:  // 更新
                            matchRows2 = m_EmrPatientTable.Select(String.Format(" PATNOOFHIS = '{0}' and status='1503' ", hisRow[colHisFirstPageNo].ToString().Trim()), " PATNOOFHIS asc ");
                            if (matchRows2.Length == 0)
                            {
                                UpdateRecord(hisRow, matchRows[0], initialized, true);
                            }
                            break;
                        default:
                            //MessageBox.Show("EMR中病人数据有重复");
                            break;
                    }
                }
                //xll 2013-03-07
                ChangeBabyStatus();


            }
            catch (Exception ex)
            {
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, "病人同步", ex));

            }
            return maxInsertDate;
        }


        /// <summary>
        /// xll 2013-03-07 同步时 将婴儿的状态改成和母亲状态一样
        /// </summary>
        private void ChangeBabyStatus()
        {
            try
            {
                string sqlUpdateBaby = "EMR_CommonNote.usp_ChangeBabyStatus";
                m_EmrHelper.ExecuteNoneQuery(sqlUpdateBaby, CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                //有异常就抛掉 不管 避免出错
            }
        }

        private string MergeBedInfo(DataTable changedData, bool initialized)
        {
            m_NewBeds.Clear();
            DataRow[] matchRows;
            string maxInsertDate = "";

            try
            {
                foreach (DataRow hisRow in changedData.Rows)
                {
                    // 第一条记录保存的就是最大的插入日期
                    if (!initialized && String.IsNullOrEmpty(maxInsertDate))
                        maxInsertDate = hisRow[colInsertDate].ToString();

                    matchRows = m_EmrBedTable.Select(String.Format(CultureInfo.CurrentCulture
                       , "ID = '{0}' and WardID = '{1}' and RoomID = '{2}'", hisRow[colBedNo].ToString().Trim(), hisRow["WardID"].ToString().Trim(), hisRow["RoomID"].ToString().Trim()));
                    switch (matchRows.Length)
                    {
                        case 0:  // 插入
                            AddNewRecord(m_EmrBedTable, hisRow, false);
                            break;
                        case 1:  // 更新
                            UpdateRecord(hisRow, matchRows[0], true, false);
                            break;
                        default:
                            //MessageBox.Show("EMR中病人数据有重复");
                            break;
                    }
                }
                return maxInsertDate;
            }
            catch (Exception ex)
            {
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, "病人同步", ex));
            }
            return string.Empty;
        }

        private int SaveEmrTableData(DataTable sourceTable, string tableName)
        {
            int updatedCount = 0;
            if (sourceTable != null)
            {
                //DataTable changedData = sourceTable.GetChanges();
                //if ((changedData != null) && (changedData.Rows.Count > 0))
                try
                {
                    updatedCount = m_EmrHelper.UpdateTable(sourceTable, tableName, true);
                }
                catch (Exception ex)
                {
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, "病人同步", ex));
                }

                // 单独处理新入院病人(因为需要获得EMR中的syxh)
                if (m_NewPatients.Count > 0)
                {
                    StringBuilder condition = new StringBuilder("'-" + 1 + "'");
                    foreach (KeyValuePair<string, DataRow> para in m_NewPatients)
                    {
                        condition.Append(',');
                        condition.Append("'" + para.Key + "'");
                    }
                    //DrectSoft.JobManager.JobLogHelper.WriteSqlLog(Parent, "select syxh, hissyxh from " + EMRPatTable + " where hissyxh in (" + condition.ToString() + ")");
                    DataTable newPats = m_EmrHelper.ExecuteDataTable("select NoOfInpat, PatNoOfHis from " + EMRPatTable + " where PatNoOfHis in (" + condition.ToString() + ")", CommandType.Text);
                    string key;
                    foreach (DataRow row in newPats.Rows)
                    {
                        key = row[colHisFirstPageNo].ToString();
                        m_NewPatients[key][colFirstPageNo] = row[colFirstPageNo];
                        SendQCMessage(m_NewPatients[key], m_NewPatients[key][colBedDoctor].ToString()
                           , InpatientState.New, Convert.ToDateTime(m_NewPatients[key][colTimeOfInWard]));
                    }
                    m_NewPatients.Clear();
                }
            }
            return updatedCount;
        }

        private void SendQCMessage(DataRow patientRow, string docId, InpatientState preState, DateTime inTime)
        {
            if (m_Qcsv == null)
                return;

            try
            {
                int firstPageNo = Convert.ToInt32(patientRow[colFirstPageNo]);
                Inpatient inpatient = new Inpatient(firstPageNo);
                inpatient.ReInitializeProperties();
                inpatient.PreState = preState;
                // m_Qcsv.AddRuleRecord(firstPageNo, -1, docId, QCConditionType.PatStateChange, inpatient, inTime);
            }
            catch (Exception err)
            {

                JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, "时限", err));
            }
        }

        private string CreateFilterCondition(DataTable sourceTable, string filterColumn)
        {
            StringBuilder condition = new StringBuilder("" + filterColumn + " in (");
            foreach (DataRow row in sourceTable.Rows)
                condition.Append(row[filterColumn].ToString() + ", ");
            condition.Append("0)");
            return condition.ToString();
        }

        /// <summary>
        /// 初始化病人和床位数据
        /// </summary>
        /// <param name="patientTable"></param>
        /// <param name="bedTable"></param>
        private void InitPatientAndBedData(DataTable patientTable, DataTable bedTable)
        {
            int updatedCount;
            if ((patientTable != null) && (patientTable.Rows.Count > 0))
            {
                try
                {
                    // 取所有EMR的病人数据
                    InitEmrInpatientTable(false, CreateFilterCondition(patientTable, colHisFirstPageNo));
                    // 合并，保存
                    MergePatientInfo(patientTable, true);
                    updatedCount = SaveEmrTableData(m_EmrPatientTable, EMRPatTable);
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, EMRPatTable, patientTable.Rows.Count, updatedCount, DateTime.Now, true, string.Empty, TraceLevel.Info));
                }
                catch (Exception err)
                {
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, EMRPatTable, err));
                    return;
                }
            }

            if ((bedTable != null) && (bedTable.Rows.Count > 0))
            {
                try
                {
                    // 取所有EMR的床位数据
                    InitEmrBedTable();
                    // 合并，保存
                    MergeBedInfo(bedTable, true);
                    updatedCount = SaveEmrTableData(m_EmrBedTable, EMRBedTable);
                    // 执行 syxh 的更新处理
                    //JobLogHelper.WriteSqlLog(Parent, "update " + EMRBedTable + " set syxh = b.syxh from " + EMRBedTable + " a, " + EMRPatTable + " b where a.hissyxh = b.hissyxh");
                    m_EmrHelper.ExecuteNoneQuery("    update bed a  set NoOfInpat = (select b.NoOfInpat from InPatient b where a.PatNoOfHis = b.PatNoOfHis) where exists (select 1 from InPatient b where a.PatNoOfHis = b.PatNoOfHis)");
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, EMRBedTable, bedTable.Rows.Count, updatedCount, DateTime.Now, true, string.Empty, TraceLevel.Info));
                }
                catch (Exception err)
                {
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, EMRBedTable, err));
                    return;
                }
            }
        }

        private void SynchChangedPatientAndBedData(DataTable patientTable, DataTable bedTable)
        {
            int updatedCount;
            // 增加记录病人和床位更新记录的插入日期（由数据返回，以HIS的数据库时间为准）。
            // 同步程序在更新记录时只能更新最大插入日期之前的记录，否则有可能因为读数据
            // 到执行完成之间时间差的问题，将执行期间记录的数据也置为已更新
            string maxInsertDate;

            if ((patientTable != null) && (patientTable.Rows.Count > 0))
            {
                try
                {
                    // 取指定的EMR的病人数据
                    InitEmrInpatientTable(true, "1=1");
                    //InitEmrInpatientTable(true, CreateFilterCondition(patientTable, colHisFirstPageNo));

                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, "Inpatient:" + m_EmrPatientTable.Rows.Count.ToString(), TraceLevel.Info));
                    // 合并，保存
                    maxInsertDate = MergePatientInfo(patientTable, false);
                    updatedCount = SaveEmrTableData(m_EmrPatientTable, EMRPatTable);
                    // 操作成功后再更新HIS库中的标志
                    m_HisHelper.ExecuteNoneQuery(String.Format("update ZY_BRSYK_MSG set gxbz = 1 , gxrq = '{0}' where gxbz = 0 and crrq <= '{1}'"
                       , DateTime.Now.ToString("yyyyMMddHH:mm:ss"), maxInsertDate)
                       , CommandType.Text);
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, EMRPatTable, patientTable.Rows.Count, updatedCount, DateTime.Now, true, string.Empty, TraceLevel.Info));
                }
                catch (Exception err)
                {
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, EMRPatTable, err));
                    return;
                }
            }

            if ((bedTable != null) && (bedTable.Rows.Count > 0))
            {
                try
                {
                    StringBuilder condition2 = new StringBuilder("ID in (");
                    foreach (DataRow row in bedTable.Rows)
                        condition2.Append("'" + row[colBedNo].ToString().Trim() + "', ");
                    condition2.Append("'')");

                    // 取指定的EMR的床位数据
                    InitEmrBedTable(condition2.ToString());
                    // 合并，保存
                    maxInsertDate = MergeBedInfo(bedTable, false);
                    updatedCount = SaveEmrTableData(m_EmrBedTable, EMRBedTable);

                    int totalUpdateSyxh = 0;
                    int totalUpdateCwdm = 0;
                    object objRowcount;
                    if (m_NewBeds.Count > 0)
                    {
                        foreach (KeyValuePair<string, DataRow> para in m_NewBeds)
                        {
                            // 更新床位表中关联的EMR首页序号
                            objRowcount = m_EmrHelper.ExecuteScalar(String.Format("update {0} a set NoOfInpat = (select b.NoOfInpat from {1} b where a.PatNoOfHis = b.PatNoOfHis)"
                                        + " where exists(select 1 from InPatient b where a.PatNoOfHis = b.PatNoOfHis and a.WardId = '{3}' and a.ID = '{2}' and a.InBed <> 1300)"
                               , EMRBedTable, EMRPatTable, para.Value[colBedNo], para.Value[colWardCode], para.Value[colDeptCode]));
                            if (objRowcount != null)
                                totalUpdateSyxh += Convert.ToInt32(objRowcount);
                            // 更新首页库中的当前床位
                            //JobLogHelper.WriteSqlLog(Parent, String.Format("update {0} set cycw = b.cwdm from {0} a, {1} b where a.hissyxh = b.hissyxh and b.bqdm = '{3}' and b.ksdm = '{4}' and b.cwdm = '{2}' and b.zcbz = 1301"
                            //   , EMRPatTable, EMRBedTable, para.Key, para.Value[colWardCode], para.Value[colDeptCode]));
                            objRowcount = m_EmrHelper.ExecuteScalar(String.Format("update {0} set OutBed a = (select b.ID from {1} b where a.PatNoOfHis = b.PatNoOfHis )"
                                       + "where exists(select 1 from {1} b where a.PatNoOfHis = b.PatNoOfHis and  b.WardId = '{3}' and b.ID = '{2}' and b.InBed = 1301)"
                               , EMRPatTable, EMRBedTable, para.Value[colBedNo], para.Value[colWardCode], para.Value[colDeptCode]));
                            if (objRowcount != null)
                                totalUpdateCwdm += Convert.ToInt32(objRowcount);
                        }
                    }

                    // 操作成功后再更新HIS库中的标志
                    m_HisHelper.ExecuteNoneQuery(String.Format("update ZY_BCDMK_MSG set gxbz = 1 , gxrq = '{0}' where gxbz = 0 and crrq <= '{1}'"
                       , DateTime.Now.ToString("yyyyMMddHH:mm:ss"), maxInsertDate)
                       , CommandType.Text);
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, EMRBedTable, bedTable.Rows.Count, updatedCount, DateTime.Now, true, string.Empty, TraceLevel.Info));
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, EMRBedTable, totalUpdateSyxh, totalUpdateCwdm, DateTime.Now, true, "CWDMK.syxh/BRSYK.cycw", TraceLevel.Info));
                }
                catch (Exception err)
                {
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, EMRBedTable, err));
                    return;
                }
            }
        }

        private DataSet GetPatientAndBedTable(bool isInit)
        {
            try
            {
                string commandText = string.Empty;
                DataSet ds = new DataSet();
                string sqlInpatient = string.Empty;
                //IDataAccess sqlDataAccess = DataAccessFactory.GetSqlDataAccess("HISDB");

                //通过配置同步上线的科室病人
                string sqlConfigSyncInpatientDept = " select value from appcfg where configkey = 'SyncInpatientDept' ";
                string depts = m_EmrHelper.ExecuteScalar(sqlConfigSyncInpatientDept, CommandType.Text).ToString();//同步属于科室的病人
                string sqlDept = string.Empty;
                for (int i = 0; i < depts.Split(',').Length; i++)
                {
                    sqlDept += "'" + depts.Split(',')[i] + "',";
                }
                sqlDept = sqlDept.TrimEnd(',');

                if (isInit)
                {
                    //读取所有在院患者 modified by zhouhui
                    //add by yxy 目前情况无法导入转科到指定科室的病人  如原先心内科的病人转科到眼科，但是入院科室为心内科无法读取。
                    //为何要抓取一年前的呢，反正都是病人， 在电子病历中可以区分出院在院的，如果HIS中状态为出院，可是出院时间为空，这个就娶不到值了 edit by ywk  2012年11月20日14:42:24
                    sqlInpatient = " select  * from ZC_INPATIENT where  STATUS IN(1500,1501,1503,1502); ";
                    DataTable inpatient = m_HisHelper.ExecuteDataTable(string.Format(sqlInpatient, sqlDept), CommandType.Text);
                    inpatient.TableName = "inpatient";
                    ds.Tables.Add(inpatient.Copy());
                    //读取所有床位
                    DataTable bed = m_HisHelper.ExecuteDataTable(" select * from ZC_BEDS ");
                    bed.TableName = "bed";
                    ds.Tables.Add(bed.Copy());
                }
                else
                {
                    //读取当日所有患者(在院患者、四日内出院患者)
                    //add by yxy 目前情况无法导入转科到指定科室的病人  如原先心内科的病人转科到眼科，但是入院科室为心内科无法读取。

                    sqlInpatient = @" select * from ZC_INPATIENT where STATUS IN(1500,1501) 
                                             union 
                                             select *from  ZC_INPATIENT where 
                                                       (STATUS IN(1502,1503) and (outwarddate between '{0}' and '{1}'))";
                    //xll 2012-11-21 修改查找病人出院天数10天内的自动同步
                    DataTable inpatient = this.m_HisHelper.ExecuteDataTable(string.Format(sqlInpatient, DateTime.Now.AddDays(-10.0).ToString("yyyy-MM-dd"),
                        DateTime.Now.AddDays(1.0).ToString("yyyy-MM-dd"), sqlDept), CommandType.Text);
                    inpatient.TableName = "inpatient";
                    ds.Tables.Add(inpatient.Copy());
                    DataTable bed = m_HisHelper.ExecuteDataTable("select * from ZC_BEDS");
                    bed.TableName = "bed";
                    ds.Tables.Add(bed.Copy());
                    //读取所有床位
                    //commandText = "usp_Emr_GetChangedInpatients";
                    //return sqlDataAccess.ExecuteDataSet(commandText, CommandType.StoredProcedure);
                }

                return ds;
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ExecuteCore(bool isInit)
        {
            base.SynchState = SynchState.Busy;
            try
            {
                m_UpdateColumns = GetConfigValueByKey("UpdateCloumns");
                DataSet resultDS = GetPatientAndBedTable(isInit);
                InitPatientAndBedData(resultDS.Tables[0], resultDS.Tables[1]);
                //if (isInit)
                //    InitPatientAndBedData(resultDS.Tables[0], resultDS.Tables[1]);
                //else
                //    SynchChangedPatientAndBedData(resultDS.Tables[0], resultDS.Tables[1]);
            }
            catch
            { throw; }
            finally
            { base.SynchState = SynchState.Stop; }
        }
        #endregion



        #region public IJobAction 成员
        public override void ExecuteDataInitialize()
        {
            ExecuteCore(true);
        }

        public override void Execute()
        {
            try
            {
                ExecuteCore(false);
            }
            catch (Exception ex)
            {
                //todo
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(this.Parent, ex.Message));
            }
        }
        #endregion
    }
}
