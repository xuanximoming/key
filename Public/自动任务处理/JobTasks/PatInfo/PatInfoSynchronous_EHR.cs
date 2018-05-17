using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Management;
using System.IO;
using System.Configuration;
using YidanSoft.JobManager;
using YidanSoft.Core;
using YidanSoft.Core.TimeLimitQC;
using YidanSoft.Common.Eop;
using System.Data.SqlClient;

[assembly: Job("住院病人和床位数据同步", "从HIS读取病人和床位数据", "电子病历", true, typeof(PatInfoSynchronous))]
namespace YidanSoft.JobManager
{
    /// <summary>
    /// 对HIS与EMR中病人基本信息和床位数据进行同步处理
    /// </summary>
    public class PatInfoSynchronous_EHR : BaseJobAction, IDisposable
    {
        #region const
        private const string EMRPatTable = "CP_InPatient";
        private const string EMRBedTable = "CP_Bed";
        private const string colFirstPageNo = "syxh";
        private const string colHisFirstPageNo = "hissyxh";
        private const string colPatientState = "brzt";
        private const string colBedDoctor = "zyys";
        private const string colTimeOfInWard = "rqrq";
        private const string colInWard = "rybq";
        private const string colInDept = "ryks";
        private const string colInBed = "rycw";
        private const string colTimeOfLeaveWard = "cqrq";
        private const string colFinalyWard = "cybq";
        private const string colFinalyDept = "cyks";
        private const string colFinalyBed = "cycw";
        private const string colBedNo = "cwdm";
        private const string colBedState = "zcbz";
        private const string colDeptCode = "ksdm";
        private const string colWardCode = "bqdm";
        private const string colInsertDate = "crrq";


        #endregion

        #region new properties
        /// <summary>
        /// 有初始化动作
        /// </summary>
        public override bool HasInitializeAction { get { return true; } }
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


        /// <summary>
        /// 同步病人时记录新入院的病人，以便发送质量监控的触发消息
        /// </summary>
        private Dictionary<string, DataRow> m_NewPatients;
        private Dictionary<string, DataRow> m_NewBeds; // 记录需要更新床位表中的首页序号和首页表中床号的床位
        private Qcsv m_Qcsv;
        #endregion

        #region ctor & dispose
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public PatInfoSynchronous_EHR()
        {
            try
            {
                m_EmrHelper = DataAccessFactory.GetSqlDataAccess("EHRDB");
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

        ~PatInfoSynchronous_EHR()
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
                             , "select * from {0} a where brzt in ({1:D},{2:D},{3:D},{4:D},{5:D}) or (brzt in ({6:D}, {7:D}) and cqrq > '{8}')"
                             , EMRPatTable
                             , InpatientState.InWard
                             , InpatientState.OutWard
                             , InpatientState.InICU
                             , InpatientState.InDeliveryRoom
                             , InpatientState.ShiftDept
                             , InpatientState.Balanced
                             , InpatientState.CancleBalanced
                             , DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd")
                             , InpatientState.New);
            }
            else // 同步信息有变化的病人
            {
                sqlCommand = String.Format(CultureInfo.CurrentCulture, "select * from {0} where 1=1", EMRPatTable);
            }

            if (!String.IsNullOrEmpty(condition))
                sqlCommand = sqlCommand + " and " + condition;

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
                newRow[column.ColumnName] = sourceRow[column.ColumnName];
                if (isPatient)
                {
                    if (column.ColumnName == colPatientState)
                    {
                        patState = (InpatientState)Convert.ToInt32(newRow[colPatientState]);
                        if (patState == InpatientState.InWard)
                            m_NewPatients.Add(newRow[colHisFirstPageNo].ToString(), newRow);
                    }
                }
                else if (column.ColumnName.Equals(colBedState))
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

                if (sourceRow[col.ColumnName].ToString().Trim()
                       == targetRow[col.ColumnName].ToString().Trim())
                    continue;

                if (isPatient) // 更新病人
                {
                    // 跳过入院科室、入院病区、入院床位的更新
                    if ((col.ColumnName == colInDept) || (col.ColumnName == colInWard) || (col.ColumnName == colInBed))
                        continue;

                    // 在病人状态由在病区变成转科或出区（以及撤销状态改变操作）时要处理长期医嘱的状态
                    if (!initialized && (col.ColumnName == colPatientState))
                        DoAfterPatientStateChanged(sourceRow, targetRow);

                    targetRow[col.ColumnName] = sourceRow[col.ColumnName];
                }
                else // 更新床位
                {
                    targetRow[col.ColumnName] = sourceRow[col.ColumnName];
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
                  break;
               case InpatientState.OutWard:
                  // 出区
                  isRollback = false;
                  ceaseReason = OrderCeaseReason.LeaveHospital;
                  needHandle = true;
                  // 发送出院消息
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
      }


        private string MergePatientInfo(DataTable changedData, bool initialized)
        {
            m_NewPatients.Clear();
            DataRow[] matchRows;
            string maxInsertDate = "";

            foreach (DataRow hisRow in changedData.Rows)
            {
                // 第一条记录保存的就是最大的插入日期
                if (!initialized && String.IsNullOrEmpty(maxInsertDate))
                    maxInsertDate = hisRow[colInsertDate].ToString();

                matchRows = m_EmrPatientTable.Select(String.Format(CultureInfo.CurrentCulture
                   , "hissyxh = {0}", hisRow[colHisFirstPageNo]));
                switch (matchRows.Length)
                {
                    case 0:  // 插入
                        AddNewRecord(m_EmrPatientTable, hisRow, true);
                        break;
                    case 1:  // 更新
                        UpdateRecord(hisRow, matchRows[0], initialized, true);
                        break;
                    default:
                        //MessageBox.Show("EMR中病人数据有重复");
                        break;
                }
            }
            return maxInsertDate;
        }

        private string MergeBedInfo(DataTable changedData, bool initialized)
        {
            m_NewBeds.Clear();
            DataRow[] matchRows;
            string maxInsertDate = "";

            foreach (DataRow hisRow in changedData.Rows)
            {
                // 第一条记录保存的就是最大的插入日期
                if (!initialized && String.IsNullOrEmpty(maxInsertDate))
                    maxInsertDate = hisRow[colInsertDate].ToString();

                matchRows = m_EmrBedTable.Select(String.Format(CultureInfo.CurrentCulture
                   , "cwdm = '{0}' and bqdm = '{1}'", hisRow[colBedNo], hisRow["bqdm"]));
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

        private int SaveEmrTableData(DataTable sourceTable, string tableName)
        {
            int updatedCount = 0;
            if (sourceTable != null)
            {
                //DataTable changedData = sourceTable.GetChanges();
                //if ((changedData != null) && (changedData.Rows.Count > 0))
                updatedCount = m_EmrHelper.UpdateTable(sourceTable, tableName, true);

                // 单独处理新入院病人(因为需要获得EMR中的syxh)
                if (m_NewPatients.Count > 0)
                {
                    StringBuilder condition = new StringBuilder("-1");
                    foreach (KeyValuePair<string, DataRow> para in m_NewPatients)
                    {
                        condition.Append(',');
                        condition.Append(para.Key);
                    }
                    //Winning.Job.JobLogHelper.WriteSqlLog(Parent, "select syxh, hissyxh from " + EMRPatTable + " where hissyxh in (" + condition.ToString() + ")");
                    DataTable newPats = m_EmrHelper.ExecuteDataTable("select syxh, hissyxh from " + EMRPatTable + " where hissyxh in (" + condition.ToString() + ")", CommandType.Text);
                    string key;
                    foreach (DataRow row in newPats.Rows)
                    {
                        key = row[colHisFirstPageNo].ToString();
                        m_NewPatients[key][colFirstPageNo] = row[colFirstPageNo];
                    }
                    m_NewPatients.Clear();
                }
            }
            return updatedCount;
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
                    
                }
                catch (Exception err)
                {
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
                    //m_EmrHelper.ExecuteNoneQuery("update " + EMRBedTable + " set syxh = b.syxh from " + EMRBedTable + " a, " + EMRPatTable + " b where a.hissyxh = b.hissyxh");
                }
                catch (Exception err)
                {
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
                    InitEmrInpatientTable(true, CreateFilterCondition(patientTable, colHisFirstPageNo));
                    // 合并，保存
                    maxInsertDate = MergePatientInfo(patientTable, false);
                    updatedCount = SaveEmrTableData(m_EmrPatientTable, EMRPatTable);
                    // 操作成功后再更新HIS库中的标志
                    //JobLogHelper.WriteSqlLog(Parent,String.Format("update ZY_BRSYK_MSG set gxbz = 1 , gxrq = '{0}' where gxbz = 0"
                    //   , DateTime.Now.ToString("yyyyMMddHH:mm:ss")));
                    //m_HisHelper.ExecuteNoneQuery(String.Format("update ZY_BRSYK_MSG set gxbz = 1 , gxrq = '{0}' where gxbz = 0 and crrq <= '{1}'"
                    //   , DateTime.Now.ToString("yyyyMMddHH:mm:ss"), maxInsertDate)
                    //   , CommandType.Text);
                }
                catch (Exception err)
                {
                    return;
                }
            }

            if ((bedTable != null) && (bedTable.Rows.Count > 0))
            {
                try
                {
                    StringBuilder condition2 = new StringBuilder("cwdm in (");
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
                            objRowcount = m_EmrHelper.ExecuteScalar(String.Format("update {0} set syxh = b.syxh from {0} a, {1} b where a.hissyxh = b.hissyxh and a.bqdm = '{3}' and a.cwdm = '{2}' and a.zcbz <> 1300"
                               + "\r\n"
                               + "select @@rowcount"
                               , EMRBedTable, EMRPatTable, para.Value[colBedNo], para.Value[colWardCode], para.Value[colDeptCode]));
                            if (objRowcount != null)
                                totalUpdateSyxh += Convert.ToInt32(objRowcount);
                            // 更新首页库中的当前床位
                            //JobLogHelper.WriteSqlLog(Parent, String.Format("update {0} set cycw = b.cwdm from {0} a, {1} b where a.hissyxh = b.hissyxh and b.bqdm = '{3}' and b.ksdm = '{4}' and b.cwdm = '{2}' and b.zcbz = 1301"
                            //   , EMRPatTable, EMRBedTable, para.Key, para.Value[colWardCode], para.Value[colDeptCode]));
                            objRowcount = m_EmrHelper.ExecuteScalar(String.Format("update {0} set cycw = b.cwdm from {0} a, {1} b where a.hissyxh = b.hissyxh and b.bqdm = '{3}' and b.cwdm = '{2}' and b.zcbz = 1301"
                               + "\r\n"
                               + "select @@rowcount"
                               , EMRPatTable, EMRBedTable, para.Value[colBedNo], para.Value[colWardCode], para.Value[colDeptCode]));
                            if (objRowcount != null)
                                totalUpdateCwdm += Convert.ToInt32(objRowcount);
                        }
                    }

                    // 操作成功后再更新HIS库中的标志
                    //m_HisHelper.ExecuteNoneQuery(String.Format("update ZY_BCDMK_MSG set gxbz = 1 , gxrq = '{0}' where gxbz = 0 and crrq <= '{1}'"
                    //   , DateTime.Now.ToString("yyyyMMddHH:mm:ss"), maxInsertDate)
                    //   , CommandType.Text);
                }
                catch (Exception err)
                {
                    return;
                }
            }
        }

        private DataSet GetPatientAndBedTable(bool isInit)
        {
            try
            {
                string commandText = string.Empty;
                IDataAccess sqlDataAccess = DataAccessFactory.GetSqlDataAccess("HISDB");

                if (isInit)
                {
                    commandText = "usp_EmrDTS_GetChangedInpatients_bak";
                    SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("jsrq", SqlDbType.VarChar, 8) };
                    parameters[0].Value = DateTime.Now.ToString("yyyyMMdd");
                    return sqlDataAccess.ExecuteDataSet(commandText, parameters, CommandType.StoredProcedure);
                }
                else
                {
                    commandText = "usp_EmrDTS_GetChangedInpatients_bak";
                    return sqlDataAccess.ExecuteDataSet(commandText, CommandType.StoredProcedure);
                }
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
                DataSet resultDS = GetPatientAndBedTable(isInit);
                if (isInit)
                    InitPatientAndBedData(resultDS.Tables[0], resultDS.Tables[1]);
                else
                    SynchChangedPatientAndBedData(resultDS.Tables[0], resultDS.Tables[1]);
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
            ExecuteCore(false);
        }
        #endregion
    }
}
