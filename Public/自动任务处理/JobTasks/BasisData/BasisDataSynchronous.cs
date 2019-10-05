using DrectSoft.Core;
using DrectSoft.JobManager;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;

[assembly: Job("基础数据同步", "从HIS同步基础数据", "电子病历", true, typeof(BasisDataSynchronous))]
namespace DrectSoft.JobManager
{
    /// <summary>
    /// 基础数据同步任务
    /// </summary>
    public class BasisDataSynchronous : BaseJobAction
    {
        #region const String
        private const string UpdateMsgTable = "YY_JCSJGXK_MSG";
        private const string TableEmployee = "Users";
        private const string TableEmployeePwdField = "Passwd";
        private const string TableEmployeeSecretKeyField = "RegDate";
        private const string s_FieldPy = "Py";
        private const string s_FieldWb = "Wb";
        private const string DefaultPwd = "QK+S40FfCEQ=";
        private const string DefaultSecretKey = "2005110717:30:35";

        private const string s_CheckHasSynchMesg = "select top 1 * from YY_JCSJGXK_MSG";

        /// <summary>
        /// 基础数据同步设置的KEY
        /// </summary>
        private const string KeyBasisDataSynchSetting = "BasisDataSynchSetting";
        #endregion

        #region new properties
        /// <summary>
        /// 有自己的配置参数
        /// </summary>
        public override bool HasPrivateSettings { get { return true; } }

        /// <summary>
        /// 有初始化动作
        /// </summary>
        public override bool HasInitializeAction { get { return true; } }
        #endregion

        #region private variable
        private IDataAccess m_SqlHelperTarget;
        private IDataAccess m_SqlHelperSource;
        private DataSet m_TargetTables;
        private GenerateShortCode m_generateShortCode;
        private ISynchApplication m_Application;

        public BasisDataSynchSetting BasisSynchsetting
        {
            get
            {
                if (_basisSynchsetting == null)
                    InitializeSettings();
                return _basisSynchsetting;
            }
        }
        private BasisDataSynchSetting _basisSynchsetting;
        #endregion

        #region 构造
        public BasisDataSynchronous()
        {
            m_SqlHelperSource = DataAccessFactory.GetSqlDataAccess("HISDB");
            m_SqlHelperTarget = DataAccessFactory.DefaultDataAccess;
            m_TargetTables = new DataSet();
            m_TargetTables.Locale = CultureInfo.CurrentCulture;
        }

        public BasisDataSynchronous(ISynchApplication ISynchApplication)
        {
            m_SqlHelperSource = DataAccessFactory.GetSqlDataAccess("HISDB");
            m_SqlHelperTarget = DataAccessFactory.DefaultDataAccess;
            m_TargetTables = new DataSet();
            m_TargetTables.Locale = CultureInfo.CurrentCulture;
            m_Application = ISynchApplication;
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitializeSettings()
        {
            Stream stream = BasicSettings.GetConfig(KeyBasisDataSynchSetting);
            XmlSerializer serializer = new XmlSerializer(typeof(BasisDataSynchSetting));
            _basisSynchsetting = serializer.Deserialize(stream) as BasisDataSynchSetting;
        }
        #endregion

        #region public IJobAction 成员
        /// <summary>
        /// 执行初始化数据表的动作
        /// </summary>
        public override void ExecuteDataInitialize()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                InitializeSettings();
                InitAllTable();
            }
            catch
            { throw; }
            finally
            { base.SynchState = SynchState.Stop; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Execute()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                //SynchChangedBasisDatas();
                InitAllTable();
            }
            catch
            { throw; }
            finally
            { base.SynchState = SynchState.Stop; }
        }

        public override void RefreshPrivateSettings()
        {
            InitializeSettings();
        }
        #endregion

        #region private Methods
        /// <summary>
        /// 同步有变化的基础数据表
        /// </summary>
        private void SynchChangedBasisDatas()
        {
            try
            {
                //取更新消息表
                DataTable updatedMsgTable = GetMessageTableData();
                if (updatedMsgTable == null)
                    return;

                Collection<string> sourceTables = new Collection<string>();
                for (int i = 0; i < updatedMsgTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(updatedMsgTable.Rows[i]["gxbz"]) == 1)
                        continue;
                    sourceTables.Add(updatedMsgTable.Rows[i]["tablename"].ToString().Trim());
                }

                // 根据源表，重新组织要执行的语句
                if (sourceTables.Count > 0)
                {
                    OpenSynchSentence(sourceTables);

                    DoExecuteSynch();

                    // 更新源中的同步记录
                    string updateMsgString = String.Format("update YY_JCSJGXK_MSG set gxbz = 1, gxrq = '{0}' where gxbz = 0"
                       , DateTime.Now.ToString("yyyyMMddHH:mm:ss"));
                    m_SqlHelperSource.ExecuteNoneQuery(updateMsgString, CommandType.Text);
                }
            }
            catch
            {
                base.SynchState = SynchState.Stop;
                throw;
            }
        }

        /// <summary>
        /// 如果消息表里没有同步记录，则一次性初始化所有基础数据，否则仍只同步有更新的内容
        /// </summary>
        private void InitAllTable()
        {
            if (_basisSynchsetting == null) InitializeSettings();
            OpenSynchSentence(null);
            DoExecuteSynch();

        }

        private void OpenSynchSentence(Collection<string> sourceTables)
        {
            foreach (TableMapping setting in BasisSynchsetting.TableMappings)
            {
                if (!setting.Enabled)
                    continue;
                setting.NeedRunNow = false;
                foreach (TableMappingDataSource source in setting.DataSources)
                {
                    if (!setting.NeedRunNow && ((sourceTables == null) || sourceTables.Contains(source.SourceTable)))
                        setting.NeedRunNow = true;
                }
            }
        }

        private void ResetPyWbValue(DataTable table, string nameField)
        {
            if ((table == null) || (table.Rows.Count <= 0) || string.IsNullOrEmpty(nameField) || (!table.Columns.Contains(nameField)))
                return;
            if (m_generateShortCode == null)
                m_generateShortCode = new GenerateShortCode(m_SqlHelperTarget);

            int pyMaxLen = 0;
            if (table.Columns.Contains(s_FieldPy))
                pyMaxLen = table.Columns[s_FieldPy].MaxLength;
            int wbMaxLen = 0;
            if (table.Columns.Contains(s_FieldWb))
                wbMaxLen = table.Columns[s_FieldWb].MaxLength;

            //NameAbbreviation na;
            foreach (DataRow row in table.Rows)
            {
                if ((row.RowState == DataRowState.Added) || (row.RowState == DataRowState.Modified))
                {
                    if (((pyMaxLen > 0) && String.IsNullOrEmpty(row[s_FieldWb].ToString()))
                       || ((wbMaxLen > 0) && String.IsNullOrEmpty(row[s_FieldPy].ToString())))
                    {
                        string[] codes = m_generateShortCode.GenerateStringShortCode(row[nameField].ToString());

                        if ((codes != null) && (codes.Length == 2))
                        {
                            row[s_FieldPy] = codes[0];
                            row[s_FieldWb] = codes[1];
                        }
                    }
                }
            }
        }

        private void DoExecuteSynch()
        {
            int changedCount = 0;
            int updatedCount = 0;
            //DataTable changedTable;

            // 首先执行数据同步的处理
            foreach (TableMapping setting in BasisSynchsetting.TableMappings)
            {
                try
                {
                    if (!setting.NeedRunNow)
                        continue;

                    changedCount = 0;
                    updatedCount = 0;
                    // 如果有预执行语句要处理，则先处理,然后清空对此表的缓存
                    if (!String.IsNullOrEmpty(setting.PreHandleSentence))
                    {
                        if (m_TargetTables.Tables.Contains(setting.TargetTable))
                        {
                            m_TargetTables.Tables.Remove(setting.TargetTable);
                            m_SqlHelperTarget.ExecuteNoneQuery(setting.PreHandleSentence);
                        }
                    }

                    // 依次执行同步语句
                    foreach (TableMappingDataSource source in setting.DataSources)
                    {
                        if (source.Enabled)
                        {
                            // 如果有预执行语句要处理，则先处理,然后清空对此表的缓存
                            if (!String.IsNullOrEmpty(source.FilteCondition))
                            {
                                if (m_TargetTables.Tables.Contains(setting.TargetTable))
                                {
                                    RemoveFromTargetTable(setting.TargetTable, source);
                                }
                            }
                            changedCount += MergeSourceAndTargetTable(setting.TargetTable, source.SelectSentence);
                        }
                    }
                    changedCount += MergeSourceAndTargetTable(setting.TargetTable, setting.SelectSentence);

                    // 重设py,wb字段的值
                    if (!string.IsNullOrEmpty(setting.NameField))
                        ResetPyWbValue(m_TargetTables.Tables[setting.TargetTable], setting.NameField);

                    // 保存数据，记Log
                    if (m_TargetTables.Tables[setting.TargetTable] != null)
                    {
                        updatedCount = m_SqlHelperTarget.UpdateTable(m_TargetTables.Tables[setting.TargetTable]
                           , setting.TargetTable, false);
                    }
                    if (!String.IsNullOrEmpty(setting.OtherSentence))
                        m_SqlHelperTarget.ExecuteNoneQuery(setting.OtherSentence, CommandType.Text);
                }
                catch (Exception e)
                {
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(this.Parent, e.Message));
                    base.SynchState = SynchState.Stop;
                }
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, setting.TargetTable, m_TargetTables.Tables[setting.TargetTable].Rows.Count, updatedCount, DateTime.Now, true, string.Empty, TraceLevel.Info));
            }
        }

        private DataTable GetMessageTableData()
        {
            string selectString = string.Format(CultureInfo.CurrentCulture, "select * from {0} where gxbz=0", UpdateMsgTable);
            return m_SqlHelperSource.ExecuteDataTable(selectString, CommandType.Text);
        }

        private int MergeSourceAndTargetTable(string targetTable, string selectSentence)
        {
            if (!String.IsNullOrEmpty(selectSentence))
            {
                if (m_TargetTables.Tables[targetTable] != null)
                {
                    m_TargetTables.Tables.Remove(targetTable);
                }
                InitializeTargetTableDataAndSchema(targetTable);

                DataTable tempTable = m_SqlHelperSource.ExecuteDataTable(selectSentence, CommandType.Text);

                // 手工处理合并过程
                MergeDataTable(m_TargetTables.Tables[targetTable], tempTable);

                return tempTable.Rows.Count;
            }
            return 0;
        }

        private void MergeDataTable(DataTable targetTable, DataTable sourceTable)
        {
            // 以主键进行匹配，存在则更新，不存在则添加
            // 假设主键的列不会超过四个
            string key1 = "";
            string key2 = "";
            string key3 = "";
            string key4 = "";
            // 首先生成匹配条件的格式化串
            DataColumn[] keys = targetTable.PrimaryKey;
            if ((keys == null) || (keys.Length == 0))
                throw new ArgumentNullException(targetTable.TableName, "主键为空");
            StringBuilder condition = new StringBuilder("1=1");
            for (int index = 0; index < keys.Length; index++)
            {
                if (!sourceTable.Columns.Contains(keys[index].ColumnName))
                    throw new ArgumentNullException(targetTable.TableName, "源表数据集中不存在需要的主键列" + keys[index].ColumnName);
                if (keys[index].DataType == typeof(string))
                    condition.AppendFormat(" and {0} = '{{{1}}}'", keys[index].ColumnName, index);
                else
                    condition.AppendFormat(" and {0} = {{{1}}}", keys[index].ColumnName, index);
            }
            if (keys.Length == 1)
            {
                key1 = keys[0].ColumnName;
                key2 = keys[0].ColumnName;
                key3 = keys[0].ColumnName;
                key4 = keys[0].ColumnName;
            }
            else if (keys.Length == 2)
            {
                key1 = keys[0].ColumnName;
                key2 = keys[1].ColumnName;
                key3 = keys[1].ColumnName;
                key4 = keys[1].ColumnName;
            }
            else if (keys.Length == 3)
            {
                key1 = keys[0].ColumnName;
                key2 = keys[1].ColumnName;
                key3 = keys[2].ColumnName;
                key4 = keys[2].ColumnName;
            }
            else
            {
                key1 = keys[0].ColumnName;
                key2 = keys[1].ColumnName;
                key3 = keys[2].ColumnName;
                key4 = keys[3].ColumnName;
            }
            DataRow[] matchRows;
            DataRow newRow;
            string conditionFormat = condition.ToString();
            foreach (DataRow row in sourceTable.Rows)
            {
                matchRows = targetTable.Select(String.Format(CultureInfo.CurrentCulture
                   , conditionFormat, row[key1], row[key2], row[key3], row[key4]));
                try
                {
                    switch (matchRows.Length)
                    {
                        case 0:  // 插入
                            newRow = targetTable.NewRow();
                            foreach (DataColumn column in sourceTable.Columns)
                            {
                                newRow[column.ColumnName] = row[column.ColumnName];
                            }
                            targetTable.Rows.Add(newRow);
                            break;
                        case 1:  // 更新
                            foreach (DataColumn col in sourceTable.Columns)
                            {
                                if (row[col.ColumnName].ToString().Trim()
                                       == matchRows[0][col.ColumnName].ToString().Trim())
                                    continue;
                                //为排除因为同步原因使users表的jobid改成上个状态，这边先加个限制add by ywk  2012年1月10日10:30:12 
                                //if (targetTable.TableName == "Users" && !string.IsNullOrEmpty(matchRows[0]["jobid"].ToString()))//如果目标表里的jobid不为空,而且为用户表
                                if (targetTable.TableName == "Users")//如果目标表里的jobid不为空,而且为用户表
                                {
                                    //先行让科室病区变化掉 add by ywk  2013年4月2日9:25:28 

                                    if (!string.IsNullOrEmpty(matchRows[0]["deptid"].ToString()) && !string.IsNullOrEmpty(matchRows[0]["wardid"].ToString()))
                                    {
                                        //if (col.ColumnName == "jobid")
                                        if (!string.IsNullOrEmpty(matchRows[0]["jobid"].ToString()))
                                        {

                                        }
                                        else
                                        {
                                            matchRows[0][col.ColumnName] = row[col.ColumnName];//赋值 
                                        }
                                    }
                                    //如果首次EMR中无病区，HIS后期设置病区，应该也要赋值,应该用或者关系add by ywk 2013年5月6日15:01:09  
                                    else if (string.IsNullOrEmpty(matchRows[0]["deptid"].ToString()) || string.IsNullOrEmpty(matchRows[0]["wardid"].ToString()))
                                    {
                                        //if (col.ColumnName == "jobid")
                                        if (!string.IsNullOrEmpty(matchRows[0]["jobid"].ToString()))
                                        {

                                        }
                                        else
                                        {
                                            matchRows[0][col.ColumnName] = row[col.ColumnName];//赋值 
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(matchRows[0]["jobid"].ToString()))//JOBID不变化
                                    {

                                    }
                                    //matchRows[0]["jobid"] = row[col.ColumnName];//什么都不做
                                }
                                //同步的用户表，而且jobid为空，（为保证其他字段正常同步过来修改）add by ywk 2013年4月1日15:40:31 
                                //else if ((targetTable.TableName == "Users" && string.IsNullOrEmpty(matchRows[0]["jobid"].ToString())))
                                //{
                                //    matchRows[0][col.ColumnName] = row[col.ColumnName];
                                //}
                                else
                                {
                                    matchRows[0][col.ColumnName] = row[col.ColumnName];
                                }
                            }
                            break;
                        default:
                            //MessageBox.Show("数据有重复");
                            break;
                    }
                }
                catch (Exception err)
                {
                    m_Application.WriteLog(new JobExecuteInfoArgs(Parent, sourceTable.TableName, err));
                }
            }
        }

        private void InitializeTargetTableDataAndSchema(string targetTableName)
        {
            //查找指定表的索引号并利用索引号操作
            if (string.IsNullOrEmpty(targetTableName))
                return;

            string querySql = String.Format(CultureInfo.CurrentCulture, "select * from {0}", targetTableName);
            DataTable table = m_SqlHelperTarget.ExecuteDataTable(querySql, CommandType.Text);
            table.TableName = targetTableName;
            m_SqlHelperTarget.ResetTableSchema(table, targetTableName);

            if (targetTableName == TableEmployee)
            {
                table.Columns[TableEmployeePwdField].DefaultValue = DefaultPwd;
                table.Columns[TableEmployeeSecretKeyField].DefaultValue = DefaultSecretKey;
            }

            m_TargetTables.Tables.Add(table.Copy());
        }

        private void RemoveFromTargetTable(string targetTableName, TableMappingDataSource source)
        {
            m_TargetTables.Tables[targetTableName].DefaultView.RowFilter = source.FilteCondition;

            for (int i = 0; i < m_TargetTables.Tables[targetTableName].DefaultView.Count; i++)
            {
                DataRow row = m_TargetTables.Tables[targetTableName].DefaultView[i].Row;
                row.Delete();
            }
        }
        #endregion

    }
}