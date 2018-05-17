using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.Emr.QCTimeLimit.QCEnum;
using System.Data;
using System.Data.SqlClient;
using DrectSoft.DSSqlHelper;
using System.Globalization;

namespace DrectSoft.Emr.QCTimeLimit.QCEntity
{
    /// <summary>
    /// 病历质量控制记录库
    /// QCRecord表对应的实体类
    /// </summary>
    public class QCRecord
    {
        #region Property
        /// <summary>
        /// 序号
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 首页序号 Inpatient.NoofInpat
        /// </summary>
        public string NoOfInpat { get; set; }

        /// <summary>
        /// 病历号 Inpatient.Noofrecord
        /// </summary>
        public string NoOfRecord { get; set; }

        /// <summary>
        /// 规则代码 QCRule.RULECODE
        /// </summary>
        public QCRule Rule { get; set; }

        /// <summary>
        /// 条件代码(规则的前提条件)  QCCondition.Code
        /// </summary>
        public QCCondition Condition { get; set; }

        /// <summary>
        /// 条件时间 即触发的时间
        /// </summary>
        public DateTime ConditionTime { get; set; }

        /// <summary>
        /// 条件时间(条件成立的时间)  触发的时间 + 延迟时间
        /// </summary>
        public DateTime RealConditionTime { get; set; }

        /// <summary>
        /// 条件状态(是否完成)  
        /// 对于条件状态未完成的记录在系统的界面中不予显示
        /// </summary>
        public bool IsConditionFinish { get; set; }

        /// <summary>
        /// 结果时间(结果成立的时间)
        /// </summary>
        public DateTime ResultTime { get; set; }

        /// <summary>
        /// 结果状态(是否完成) 
        /// </summary>
        public bool IsResultFinish { get; set; }

        /// <summary>
        /// 违规状态(是否违规) 
        /// </summary>
        public bool IsFoul { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Reminder { get; set; }

        /// <summary>
        /// 违规信息
        /// </summary>
        public string FoulMessage { get; set; }

        /// <summary>
        /// 操作时间，即记录创建的时间
        /// </summary>
        public DateTime OperateTime { get; set; }

        /// <summary>
        /// 责任医生级别QCRule.DOCOTORLEVEL
        /// </summary>
        public DoctorGrade DoctorLevel { get; set; }

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool IsCycle { get; set; }

        /// <summary>
        /// 循环规则的第一条记录ID  QCRECORD.ID，IsCycle == true时生效
        /// </summary>
        public string FirstCycleRecordID { get; set; }

        /// <summary>
        /// 循环计数(用于限定次数的时限规则)
        /// </summary>
        public int CycleTimes { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 规则时限(以秒计算)
        /// </summary>
        public int TimeLimit { get; set; }

        /// <summary>
        /// 监控代码 用于与病历进行匹配
        /// </summary>
        public string QCCode { get; set; }

        /// <summary>
        /// 病历编号  Recorddetail.ID
        /// </summary>
        public string RecordDetailID { get; set; }

        /// <summary>
        /// 扣分
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// 针对循环时限规则，即ISCYCLE == 1，表示下一次是否需要插入初始化数据到QCRecord表中，即是否停止循环
        /// ISCYCLENEXT == 1， 表示下一次不需要插入数据，即循环停止
        /// </summary>
        public bool IsStopCycle { get; set; }
        #endregion

        #region SQL-statement
        /// <summary>
        /// 获取指定病人和时限规则的记录SQL
        /// </summary>
        const string c_SqlQCRecord = "select * from qcrecord where noofinpat = @noofinpat and rulecode = @rulecode and valid = '1'";

        /// <summary>
        /// 获取所有有效的病历时限记录 这里为了性能上的考虑，只抓取在院和出院30天内的数据
        /// </summary>
        //const string c_SqlQCRecordAll = "select * from qcrecord where valid = '1'";
        const string c_SqlQCRecordAll =
            @"select * from qcrecord 
               where qcrecord.valid = '1'
                     and (
                             exists (select 1 from inpatient where inpatient.noofinpat = qcrecord.noofinpat and inpatient.status not in ('1502', '1503'))
                             or
                             exists (select 1 from inpatient where inpatient.noofinpat = qcrecord.noofinpat and inpatient.status in ('1502', '1503')
                                        and to_date(inpatient.outhosdate, 'YYYY-MM-DD HH24:MI:SS') + 30 > sysdate)
                          )";

        /// <summary>
        /// 插入初始化数据的SQL
        /// </summary>
        const string c_SqlInsertInitData =
            @"INSERT INTO qcrecord(ID, noofinpat, noofrecord, rulecode, conditioncode, conditiontime, realconditiontime, 
                                   condition, resulttime, result, foulstate, reminder, foulmessage, docotorlevel, cycletimes, 
                                   valid, memo, iscycle, firstcyclerecordid, timelimit, qccode, score, operatetime)
              SELECT seq_qcrecord_id.nextval, @noofinpat, (select patnoofhis from inpatient where noofinpat = @noofinpat),
                     @rulecode, @conditioncode, @conditiontime, @realconditiontime, 
                     @condition, '', '0', '0', @reminder, @foulmessage, @docotorlevel, @cycletimes,
                     '1', @memo, @iscycle, @firstcyclerecordid, @timelimit, @qccode, @score, TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS')
                FROM DUAL ";

        /// <summary>
        /// 更新时限记录的条件状态为完成  针对条件未完成，且当前时间 <= QCRecord.RealConditionTime
        /// </summary>
        const string c_SqlUpdateCondition =
            @"update qcrecord set condition = '1' 
               where condition = '0' and to_date(realconditiontime, 'YYYY-MM-DD HH24:MI:SS') <= sysdate and valid = '1'";

        /// <summary>
        /// 更新时限记录是否停止循环时限
        /// </summary>
        const string c_SqlUpdateIsStopCycle = @"update qcrecord set isstopcycle = @isstopcycle where id = @id";

        /// <summary>
        /// 更新时限记录中的完成状态至未完成
        /// </summary>
        const string c_SqlUpdateResultToUnComplete =
            @"update qcrecord q 
                 set q.recorddetailid = '', q.result = 0, q.resulttime = ''
               where q.recorddetailid is not null and q.result = 1 
                 and not exists (select 1 from recorddetail r where r.id = q.recorddetailid and r.valid = '1')
                 and (exists (select 1 from inpatient where inpatient.noofinpat = q.noofinpat and inpatient.status not in ('1502', '1503'))
                      or
                      exists (select 1 from inpatient where inpatient.noofinpat = q.noofinpat and inpatient.status in ('1502', '1503') and to_date(inpatient.outhosdate, 'YYYY-MM-DD HH24:MI:SS') + 30 > sysdate))
             ";

        /// <summary>
        /// 更新时限记录的已完成 --TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS')
        /// </summary>
        const string c_SqlUpdateResultComplete =
            @"update qcrecord q
                 set q.recorddetailid = @recorddetail, q.result = 1, q.foulstate = @foulstate, q.resulttime = (select r.audittime from recorddetail r where r.id = @recorddetail and r.valid = '1')
               where q.id = @id";

        /// <summary>
        /// 更新病历的违规状态
        /// </summary>
        const string c_SqlUpdateFoulState =
            @"update qcrecord 
                 set foulstate = 1
               where recorddetailid is null and condition = '1' and result = 0
                 and to_date(realconditiontime, 'YYYY-MM-DD HH24:MI:SS') + qcrecord.timelimit/60/60/24  < sysdate 
                 and valid = '1'";

        /// <summary>
        /// 获得循环时限规则的记录，针对未出院的病人捞取数据 且 对应的时限规则未生效
        /// </summary>
        const string c_SqlGetCircleRecord =
            @"select q.* from qcrecord q
               where q.valid = '1' and q.condition = '1' and q.iscycle = '1'
                 and exists (select 1 from qcrule r where r.rulecode = q.rulecode and r.valid = 1)
                 and exists (select 1 from inpatient i where i.noofinpat = q.noofinpat and i.status not in ('1502', '1503'))";

        /// <summary>
        /// 获得所有未完成的病历时限记录  病人在院 or（病人出院 and 出院时间在30天内）
        /// </summary>
        const string c_SqlGetUnCompleteRecord =
            @"select * from qcrecord 
               where qcrecord.valid = '1' and qcrecord.result = '0'
                 and (
                        exists (select 1 from inpatient where inpatient.noofinpat = qcrecord.noofinpat and inpatient.status not in ('1502', '1503'))
                        or
                        exists (select 1 from inpatient where inpatient.noofinpat = qcrecord.noofinpat and inpatient.status in ('1502', '1503')
                                and to_date(inpatient.outhosdate, 'YYYY-MM-DD HH24:MI:SS') + 30 > sysdate)
                     )";

        /// <summary>
        /// 作废指定的时限记录提醒
        /// </summary>
        const string c_SqlCancelRecordByID = @" update qcrecord set valid = '0' where id = '{0}' and valid = '1' ";
        #endregion

        #region 得到所有的QCRecord记录 、 通过 noofinpat + ruleCode 在QCRecord表中获取相应的记录、 获得循环时限规则记录

        /// <summary>
        /// 得到所有的QCRecord记录 DataTable -> List
        /// </summary>
        /// <param name="dictRule"></param>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        public static List<QCRecord> GetAllQCRecord(
            Dictionary<string, QCRule> dictRule,
            Dictionary<string, QCCondition> dictCondition,
            Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                DataTable dataTableRecord = DS_SqlHelper.ExecuteDataTable(c_SqlQCRecordAll, CommandType.Text);
                return GetAllQCRecord(dataTableRecord, dictRule, dictCondition, dictCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得指定病人的所有病历质量控制结果记录库 DataTable -> List
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="ruleCode"></param>
        /// <param name="dictRule"></param>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        public static List<QCRecord> GetAllQCRecord(string noofinpat, string ruleCode,
            Dictionary<string, QCRule> dictRule,
            Dictionary<string, QCCondition> dictCondition,
            Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                DataTable dataTableRecord = GetQCRecords(noofinpat, ruleCode);
                return GetAllQCRecord(dataTableRecord, dictRule, dictCondition, dictCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得循环时限规则记录 DataTable -> List
        /// </summary>
        /// <param name="dictRule"></param>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        public static List<QCRecord> GetAllQCRecordCircle(
            Dictionary<string, QCRule> dictRule,
            Dictionary<string, QCCondition> dictCondition,
            Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                DataTable dataTableRecord = DS_SqlHelper.ExecuteDataTable(c_SqlGetCircleRecord, CommandType.Text);
                return GetAllQCRecord(dataTableRecord, dictRule, dictCondition, dictCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得所有未完成的病历时限记录 DataTable -> List
        /// </summary>
        /// <param name="dictRule"></param>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        public static List<QCRecord> GetAllQCRecordUnComplete(
            Dictionary<string, QCRule> dictRule,
            Dictionary<string, QCCondition> dictCondition,
            Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                DataTable dataTableRecord = DS_SqlHelper.ExecuteDataTable(c_SqlGetUnCompleteRecord, CommandType.Text);
                return GetAllQCRecord(dataTableRecord, dictRule, dictCondition, dictCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得所有的病历质量控制结果记录库 DataTable -> List
        /// </summary>
        /// <param name="dataTableRecord"></param>
        /// <param name="dictRule"></param>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        public static List<QCRecord> GetAllQCRecord(DataTable dataTableRecord,
            Dictionary<string, QCRule> dictRule,
            Dictionary<string, QCCondition> dictCondition,
            Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                List<QCRecord> recordList = new List<QCRecord>();
                foreach (DataRow dr in dataTableRecord.Rows)
                {
                    recordList.Add(ConvertToQCRecordFromDataRow(dr, dictRule, dictCondition, dictCategory));
                }
                return recordList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从数据行对象转换为质量规则对象，即 DataRow -> QCRule
        /// </summary>
        /// <param name="dataRowRule"></param>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        static QCRecord ConvertToQCRecordFromDataRow(DataRow dataRowRecord,
            Dictionary<string, QCRule> dictRule,
            Dictionary<string, QCCondition> dictCondition,
            Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                int result;
                QCRecord record = new QCRecord();

                #region QCRecord实例赋值
                record.ID = dataRowRecord["ID"].ToString();
                record.NoOfInpat = dataRowRecord["NOOFINPAT"].ToString();
                record.NoOfRecord = dataRowRecord["NOOFRECORD"].ToString();
                record.Rule = dictRule[dataRowRecord["RULECODE"].ToString()];
                record.Condition = dictCondition[dataRowRecord["CONDITIONCODE"].ToString()];
                record.ConditionTime = Convert.ToDateTime(dataRowRecord["CONDITIONTIME"].ToString());
                record.RealConditionTime = Convert.ToDateTime(dataRowRecord["REALCONDITIONTIME"].ToString());
                record.IsConditionFinish = dataRowRecord["CONDITION"].ToString() == "0" ? false : true;
                if (dataRowRecord["RESULTTIME"].ToString() != "")
                {
                    record.ResultTime = Convert.ToDateTime(dataRowRecord["RESULTTIME"].ToString());
                }
                record.IsResultFinish = dataRowRecord["RESULT"].ToString() == "0" ? false : true;
                record.IsFoul = dataRowRecord["FOULSTATE"].ToString() == "0" ? false : true;
                record.Reminder = dataRowRecord["REMINDER"].ToString();
                record.FoulMessage = dataRowRecord["FOULMESSAGE"].ToString();
                record.OperateTime = Convert.ToDateTime(dataRowRecord["OPERATETIME"].ToString());
                record.DoctorLevel = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), dataRowRecord["DOCOTORLEVEL"].ToString());
                record.IsCycle = dataRowRecord["ISCYCLE"].ToString() == "0" ? false : true;
                record.FirstCycleRecordID = dataRowRecord["FIRSTCYCLERECORDID"].ToString();
                record.CycleTimes = 0;
                if (int.TryParse(dataRowRecord["CYCLETIMES"].ToString(), out result))
                {
                    record.CycleTimes = result;
                }
                record.Memo = dataRowRecord["MEMO"].ToString();
                record.TimeLimit = 0;
                if (int.TryParse(dataRowRecord["TIMELIMIT"].ToString(), out result))
                {
                    record.TimeLimit = result;
                }
                record.QCCode = dataRowRecord["QCCODE"].ToString();
                record.RecordDetailID = dataRowRecord["RECORDDETAILID"].ToString();
                record.Score = 0;
                if (int.TryParse(dataRowRecord["SCORE"].ToString(), out result))
                {
                    record.Score = result;
                }
                record.IsStopCycle = false;
                if (dataRowRecord["ISSTOPCYCLE"].ToString() == "1")
                {
                    record.IsStopCycle = true;
                }
                #endregion

                return record;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取病历时限记录
        /// </summary>
        /// <param name="noofinpat">首页序号</param>
        /// <param name="ruleCode">时限规则代码</param>
        /// <returns></returns>
        static DataTable GetQCRecords(string noofinpat, string ruleCode)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("@noofinpat", SqlDbType.NVarChar),
                    new SqlParameter("@rulecode", SqlDbType.NVarChar)
                };
                parms[0].Value = noofinpat;
                parms[1].Value = ruleCode;
                return DS_SqlHelper.ExecuteDataTable(c_SqlQCRecord, parms, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 作废指定的时限记录提醒
        public static void CancelQCRecordData(string qcRecordID)
        {
            try
            {
                DS_SqlHelper.ExecuteNonQuery(string.Format(c_SqlCancelRecordByID, qcRecordID), CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 在时限规则触发时插入初始化数据
        /// <summary>
        /// 插入初始化数据，即外部触发时应该往QCRecord表中插入的一条时限记录信息
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="rule"></param>
        /// <param name="ConfigDataResult"></param>
        public static void InsertInitData(string noofinpat, QCRule rule, DataRow drConfigDataResult)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("@noofinpat", SqlDbType.NVarChar),
                    new SqlParameter("@rulecode", SqlDbType.NVarChar),
                    new SqlParameter("@conditioncode", SqlDbType.NVarChar),
                    new SqlParameter("@conditiontime", SqlDbType.NVarChar),
                    new SqlParameter("@realconditiontime", SqlDbType.NVarChar),
                    new SqlParameter("@condition", SqlDbType.Int),
                    new SqlParameter("@reminder", SqlDbType.NVarChar),
                    new SqlParameter("@foulmessage", SqlDbType.NVarChar),
                    new SqlParameter("@docotorlevel", SqlDbType.Int),
                    new SqlParameter("@cycletimes", SqlDbType.Int),
                    new SqlParameter("@memo", SqlDbType.NVarChar),
                    new SqlParameter("@iscycle", SqlDbType.Int),
                    new SqlParameter("@firstcyclerecordid", SqlDbType.NVarChar),
                    new SqlParameter("@timelimit", SqlDbType.Int),
                    new SqlParameter("@qccode", SqlDbType.NVarChar),
                    new SqlParameter("@score", SqlDbType.Decimal)
                };
                parms[0].Value = noofinpat;
                parms[1].Value = rule.RuleCode;
                parms[2].Value = rule.Condition.Code;
                parms[3].Value = (Convert.ToDateTime(drConfigDataResult[rule.Condition.TimeColumnName].ToString())).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                DateTime RealConditionTime = Convert.ToDateTime(parms[3].Value).AddSeconds((int)rule.DelayTime);
                parms[4].Value = RealConditionTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                parms[5].Value = (int)rule.DelayTime == 0 ? 1 : 0;
                parms[6].Value = rule.Reminder;
                parms[7].Value = rule.FoulMessage;
                parms[8].Value = (int)rule.DoctorLevel;
                parms[9].Value = rule.MARK == OperationType.Circle ? "1" : "0";
                parms[10].Value = rule.Memo;
                parms[11].Value = rule.MARK == OperationType.Circle ? "1" : "0";
                parms[12].Value = "";
                parms[13].Value = rule.TimeLimit.ToString();
                parms[14].Value = rule.QCCode;
                parms[15].Value = rule.Sorce.ToString();

                DS_SqlHelper.ExecuteNonQuery(c_SqlInsertInitData, parms, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入初始化数据，只针对循环时限规则
        /// </summary>
        /// <param name="record"></param>
        public static void InsertInitDataForCircle(QCRecord recordLast, QCRecord recordFirst)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("@noofinpat", SqlDbType.NVarChar),
                    new SqlParameter("@rulecode", SqlDbType.NVarChar),
                    new SqlParameter("@conditioncode", SqlDbType.NVarChar),
                    new SqlParameter("@conditiontime", SqlDbType.NVarChar),
                    new SqlParameter("@realconditiontime", SqlDbType.NVarChar),
                    new SqlParameter("@condition", SqlDbType.Int),
                    new SqlParameter("@reminder", SqlDbType.NVarChar),
                    new SqlParameter("@foulmessage", SqlDbType.NVarChar),
                    new SqlParameter("@docotorlevel", SqlDbType.Int),
                    new SqlParameter("@cycletimes", SqlDbType.Int),
                    new SqlParameter("@memo", SqlDbType.NVarChar),
                    new SqlParameter("@iscycle", SqlDbType.Int),
                    new SqlParameter("@firstcyclerecordid", SqlDbType.NVarChar),
                    new SqlParameter("@timelimit", SqlDbType.Int),
                    new SqlParameter("@qccode", SqlDbType.NVarChar),
                    new SqlParameter("@score", SqlDbType.Decimal)
                };
                parms[0].Value = recordLast.NoOfInpat;
                parms[1].Value = recordLast.Rule.RuleCode;
                parms[2].Value = recordLast.Condition.Code;
                parms[3].Value = recordLast.RealConditionTime.AddSeconds(recordLast.TimeLimit).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                parms[4].Value = recordLast.RealConditionTime.AddSeconds(recordLast.TimeLimit).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
                parms[5].Value = 1;//由于循环中的规则没有延迟时间，所以插入即生效
                parms[6].Value = recordLast.Reminder;
                parms[7].Value = recordLast.FoulMessage;
                parms[8].Value = (int)recordLast.DoctorLevel;
                parms[9].Value = ++recordLast.CycleTimes;//当前是循环的次数
                parms[10].Value = recordLast.Memo;
                parms[11].Value = recordLast.Rule.MARK == OperationType.Circle ? "1" : "0";
                parms[12].Value = recordFirst.ID;
                parms[13].Value = recordLast.TimeLimit;
                parms[14].Value = recordLast.QCCode;
                parms[15].Value = recordLast.Score;

                DS_SqlHelper.ExecuteNonQuery(c_SqlInsertInitData, parms, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 更新QCRecord表中的内容

        /// <summary>
        /// 更新时限记录的条件状态为完成
        /// </summary>
        public static void UpdateConditionComplete()
        {
            try
            {
                DS_SqlHelper.ExecuteNonQuery(c_SqlUpdateCondition, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新时限记录中的完成状态至未完成
        /// </summary>
        public static void UpdateResultToUnComplete()
        {
            try
            {
                DS_SqlHelper.ExecuteNonQuery(c_SqlUpdateResultToUnComplete, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新病历的违规状态
        /// </summary>
        public static void UpdateFoulState()
        {
            try
            {
                DS_SqlHelper.ExecuteNonQuery(c_SqlUpdateFoulState, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新时限记录的已完成
        /// </summary>
        /// <param name="id"></param>
        public static void UpdateResultComplete(string id, string recordDetailID, string foulState)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.NVarChar),
                    new SqlParameter("@recorddetail", SqlDbType.NVarChar),
                    new SqlParameter("@foulstate", SqlDbType.NVarChar)
                };
                parms[0].Value = id;
                parms[1].Value = recordDetailID;
                parms[2].Value = foulState;
                DS_SqlHelper.ExecuteNonQuery(c_SqlUpdateResultComplete, parms, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新时限记录是否停止循环时限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isStopCycle"></param>
        public static void UpdateStopCycle(string id, bool isStopCycle)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("@id", SqlDbType.NVarChar),
                    new SqlParameter("@isstopcycle", SqlDbType.NVarChar)
                };
                parms[0].Value = id;
                parms[1].Value = isStopCycle ? "1" : "";
                DS_SqlHelper.ExecuteNonQuery(c_SqlUpdateIsStopCycle, parms, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除质控记录
        /// xlb 2013-01-11
        /// </summary>
        /// <param name="ruleCode"></param>
        public static void DeleteQcRecird(string ruleCode)
        {
            try
            {
                string sqlDeleteQcRecord = "delete QcRecord where rulecode=@ruleCode";
                SqlParameter[] sps = { new SqlParameter("@ruleCode", ruleCode) };
                DS_SqlHelper.ExecuteNonQuery(sqlDeleteQcRecord, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有质控记录
        /// xlb 13-01-11
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllQcRecord(string ruleCode)
        {
            try
            {
                string sqlAllQcRecord = "select count(*) from qcrecord where rulecode=@ruleCode";
                SqlParameter[] sps = { new SqlParameter("@ruleCode", ruleCode == null ? "" : ruleCode) };
                return DS_SqlHelper.ExecuteDataTable(sqlAllQcRecord, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
    }
}
