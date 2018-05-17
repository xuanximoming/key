using System;
using System.Data;
using System.Diagnostics;
using DrectSoft.Core;
using DrectSoft.JobManager;

[assembly: Job("住院病人护理级别同步", "从HIS读取病人护理级别信息，并更新到首页表", "电子病历", true, typeof(PatientCareLevelSynch))]
namespace DrectSoft.JobManager
{
    /// <summary>
    /// 同步病人的护理级别
    /// </summary>
    public class PatientCareLevelSynch : BaseJobAction
    {
        #region const
        private const string s_SelectEmrCurrentPats = "select NoOfInpat, PatNoOfHis, AttendLevel from InPatient"
          + " where Status not in (1500, 1503, 1508, 1509)";
        private const string s_SelectPatsCareLevel =
           "select a.syxh hissyxh, b.ypdm hljb, b.ypmc hljbmc from ZY_BRSYK a, BQ_CQYZK b"
           + " where a.brzt not in (0, 3, 8, 9) and a.syxh = b.syxh and b.yzlb = 5 and b.yzzt = 2"
           + " order by a.syxh, b.xh desc";
        private const string s_ColHisFirstPageNo = "hissyxh";
        private const string s_ColCareLevelCode = "hljb";
        #endregion

        #region private variables
        private IDataAccess m_EmrHelper;
        private IDataAccess m_HisHelper;
        //private ISynchApplication m_App;//by ukey 2016-08-19 屏蔽警告
        private DataTable m_EmrPatTable;
        #endregion

        #region ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        public PatientCareLevelSynch()
        {
            try
            {
                m_EmrHelper = DataAccessFactory.DefaultDataAccess;
                m_HisHelper = DataAccessFactory.GetSqlDataAccess("HISDB");
                m_EmrPatTable = m_EmrHelper.ExecuteDataTable(s_SelectEmrCurrentPats + " and 1=2", false);
                m_EmrHelper.ResetTableSchema(m_EmrPatTable, "InPatient");
            }
            catch
            {
                throw;
            }
        }
        #endregion

        public override void Execute()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                // 读取当前在院病人
                m_EmrPatTable.Clear();
                DataTable table = m_EmrHelper.ExecuteDataTable(s_SelectEmrCurrentPats, false);
                m_EmrPatTable.Merge(table, false, MissingSchemaAction.Ignore);
                // 读取病人的护理医嘱信息
                DataTable careLevelTable = m_HisHelper.ExecuteDataTable(s_SelectPatsCareLevel, false);
                // 更新病人数据
                // (因为可能存在一个病人有多个护理医嘱的情况，所以用DataRow匹配更新的方式)
                foreach (DataRow row in careLevelTable.Rows)
                {
                    DataRow[] matchRows = m_EmrPatTable.Select("PatNoOfHis = " + row[s_ColHisFirstPageNo].ToString());
                    if ((matchRows != null) && (matchRows.Length > 0))
                    {
                        if (matchRows[0][s_ColCareLevelCode].ToString().Trim()
                           != row[s_ColCareLevelCode].ToString().Trim())
                            matchRows[0][s_ColCareLevelCode] = row[s_ColCareLevelCode];
                    }
                }
                int updatedCount = m_EmrHelper.UpdateTable(m_EmrPatTable, "Inpatient", false);
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, "护理级别", m_EmrPatTable.Rows.Count, updatedCount, DateTime.Now, true, string.Empty, TraceLevel.Info));
            }
            catch
            {
                throw;
            }
            finally
            {
                base.SynchState = SynchState.Stop;
            }
        }
    }
}
