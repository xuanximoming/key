using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DrectSoft.Core.TimeLimitQC;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 时限规则数据访问类
    /// </summary>
    public class QCRuleDal
    {
        #region fields

        IDataAccess _sqlHelper;

        const string SQL_SelectAllRules =
            " select RuleCode,ConditionCode,ResultCode,Description,Reminder,FoulMessage,TimeLimit,RelatedRule,RelatedMark,Mark,"
            + " CycleTimes, CycleInterval, DocotorLevel,SortCode,Valid,Memo "
            + " from QCRule "
            + " where Valid=1";
        const string SQL_SelectAllRulesIncludeInvalid =
            " select RuleCode,ConditionCode,ResultCode,Description,Reminder,FoulMessage,TimeLimit,RelatedRule,RelatedMark,Mark, "
            + " CycleTimes, CycleInterval, DocotorLevel,SortCode,Valid,Memo "
            + " from QCRule ";
        const string SQL_SelectRuleById =
            " select RuleCode,ConditionCode,ResultCode,Description,Reminder,FoulMessage,TimeLimit,RelatedRule,RelatedMark,Mark, "
            + " CycleTimes, CycleInterval, DocotorLevel,SortCode,Valid,Memo "
            + " from QCRule "
            + " where RuleCode=@RuleCode and Valid=1 ";
        const string SQL_InsertRule =
            " insert into QCRule (RuleCode,ConditionCode,ResultCode,Description,Reminder,FoulMessage,TimeLimit,RelatedRule,RelatedMark,Mark,CycleTimes, CycleInterval,DocotorLevel,SortCode,Valid,Memo) "
            + " values(@RuleCode,@ConditionCode,@ResultCode,@Description,@Reminder,@FoulMessage,@TimeLimit,@RelatedRule,@RelatedMark,@Mark,@CycleTimes,@CycleInterval,@DocotorLevel,@SortCode,@Valid,@Memo) ";
        const string SQL_DeleteRule =
            " delete from QCRule where RuleCode=@RuleCode ";
        const string SQL_UpdateRule =
            " update QCRule set ConditionCode=@ConditionCode,ResultCode=@ResultCode,Description=@Description "
            + " ,Reminder=@Reminder,FoulMessage=@FoulMessage,TimeLimit=@TimeLimit, RelatedRule=@RelatedRule "
            + " ,RelatedMark=@RelatedMark,Mark=@Mark, CycleTimes=@CycleTimes, CycleInterval=@CycleInterval,DocotorLevel=@DocotorLevel,SortCode=@SortCode,Valid=@Valid,Memo=@Memo"
            + " where RuleCode=@RuleCode ";
        const string tab_zlkzjgk = "QCRule";
        const string col_RuleCode = "RuleCode";
        const string col_ConditionCode = "ConditionCode";
        const string col_ResultCode = "ResultCode";
        const string col_Description = "Description";
        const string col_Reminder = "Reminder";
        const string col_FoulMessage = "FoulMessage";
        const string col_TimeLimit = "TimeLimit";
        const string col_RelatedRule = "RelatedRule";
        const string col_RelatedMark = "RelatedMark";
        const string col_Mark = "Mark";
        const string col_xhcs = "CycleTimes";
        const string col_xhjg = "CycleInterval";
        const string col_ysjb = "DocotorLevel";
        const string col_fldm = "SortCode";
        const string col_Valid = "Valid";
        const string col_Memo = "Memo";
        const string param_RuleCode = "RuleCode";
        const string param_ConditionCode = "ConditionCode";
        const string param_ResultCode = "ResultCode";
        const string param_Description = "Description";
        const string param_Reminder = "Reminder";
        const string param_FoulMessage = "FoulMessage";
        const string param_TimeLimit = "TimeLimit";
        const string param_RelatedRule = "RelatedRule";
        const string param_RelatedMark = "RelatedMark";
        const string param_Mark = "Mark";
        const string param_xhcs = "CycleTimes";
        const string param_xhjg = "CycleInterval";
        const string param_ysjb = "DocotorLevel";
        const string param_fldm = "SortCode";
        const string param_Valid = "Valid";
        const string param_Memo = "Memo";

        #endregion

        #region ctor

        /// <summary>
        /// 构造
        /// </summary>
        public QCRuleDal()
        {
            _sqlHelper = DataAccessFactory.GetSqlDataAccess();
        }

        #endregion

        #region dalc procedure

        /// <summary>
        /// 取得时限结果数据集
        /// </summary>
        /// <returns></returns>
        public DataSet GetRulesDataSet()
        {
            return _sqlHelper.ExecuteDataSet(SQL_SelectAllRulesIncludeInvalid);
        }

        /// <summary>
        /// 取得时限结果集合
        /// </summary>
        /// <returns></returns>
        public IList<QCRule> GetRulesList()
        {
            IList<QCRule> resultList = new List<QCRule>();
            DataSet dsResults = GetRulesDataSet();
            if (dsResults != null && dsResults.Tables.Count > 0)
            {
                DataTable dtResults = dsResults.Tables[0];
                for (int i = 0; i < dtResults.Rows.Count; i++)
                    resultList.Add(DataRow2QCRule(dtResults.Rows[i]));
            }
            return resultList;
        }

        /// <summary>
        /// 取得时限结果集合2
        /// </summary>
        /// <returns></returns>
        public IList<QCRule> GetRulesList(IList<QCCondition> allconditions, IList<QCResult> allresults)
        {
            IList<QCRule> resultList = new List<QCRule>();
            DataSet dsResults = GetRulesDataSet();
            if (dsResults != null && dsResults.Tables.Count > 0)
            {
                DataTable dtResults = dsResults.Tables[0];
                for (int i = 0; i < dtResults.Rows.Count; i++)
                    resultList.Add(DataRow2QCRule(dtResults.Rows[i], allconditions, allresults, false));
            }
            //加载相关规则
            foreach (QCRule qcr in resultList)
            {
                if (!string.IsNullOrEmpty(qcr.PreRelateRuleIds))
                {
                    foreach (QCRule qcrrelate in resultList)
                    { 
                        if (qcr.PreRelateRuleIds.Contains(qcrrelate.Id+","))
                            qcr.AddRelateRule(qcrrelate.Clone());
                    }
                }
            }
            return resultList;
        }

        void GetRelateRules(QCRule rule, string relateIds)
        {
            GetRelateRules(rule, relateIds, null, null);
        }

        void GetRelateRules(QCRule rule, string relateIds, IList<QCCondition> allconditions, IList<QCResult> allresults)
        {
            rule.ClearRelateRules();
            string[] ruleIds = relateIds.Split(',');
            for (int i = 0; i < ruleIds.Length; i++)
            {
                string ruleId = ruleIds[i].Trim();
                if (string.IsNullOrEmpty(ruleId)) continue;
                QCRule relaterule = null;
                if (allconditions == null || allresults == null)
                    relaterule = GetQCRuleById(ruleId);
                else
                    relaterule = GetQCRuleById(ruleId, allconditions, allresults);
                if (relaterule != null) rule.AddRelateRule(relaterule);
            }
        }

        /// <summary>
        /// 数据行 -> 时限
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public QCRule DataRow2QCRule(DataRow row)
        {
            QCRule rule = DataRow2QCRuleBase(row);

            QCConditionDal qccd = new QCConditionDal();
            rule.Condition = qccd.GetConditionById(row[col_ConditionCode].ToString());
            QCResultDal qcrd = new QCResultDal();
            rule.Result = qcrd.GetResultById(row[col_ResultCode].ToString());
            if (row[col_fldm] != DBNull.Value)
            {
                QCRuleGroupDal qcrgd = new QCRuleGroupDal();
                rule.Group = qcrgd.GetRuleGroupById(row[col_fldm].ToString());
            }

            string relaterules = row[col_RelatedRule].ToString();
            if (!string.IsNullOrEmpty(relaterules)) GetRelateRules(rule, relaterules);

            return rule;
        }

        /// <summary>
        /// 数据行 -> 时限
        /// </summary>
        /// <param name="row">数据</param>
        /// <param name="allconditions">所有规则条件</param>
        /// <param name="allresults">所有规则结果</param>
        /// <param name="loadRelated">是否加载相关规则</param>
        /// <returns></returns>
        public QCRule DataRow2QCRule(DataRow row, IList<QCCondition> allconditions, IList<QCResult> allresults, bool loadRelated)
        {
            QCRule rule = DataRow2QCRuleBase(row);

            string conditionId = row[col_ConditionCode].ToString();
            string resultId = row[col_ResultCode].ToString();
            string groupId = string.Empty;
            if (row[col_fldm] != DBNull.Value)
                groupId = row[col_fldm].ToString();
            QCCondition qcc = QCCondition.SelectQCCondition(conditionId);
            QCResult qcr = QCResult.SelectQCResult(resultId);
            QCRuleGroup qcrg = QCRuleGroup.SelectQCRuleGroup(groupId);

            rule.Condition = qcc;
            rule.Result = qcr;
            rule.Group = qcrg;

            string relaterules = row[col_RelatedRule].ToString();
            if (!string.IsNullOrEmpty(relaterules))
                if (loadRelated) GetRelateRules(rule, relaterules, allconditions, allresults);
                else rule.PreRelateRuleIds = relaterules;

            return rule;
        }

        QCRule DataRow2QCRuleBase(DataRow row)
        {
            if (row == null) throw new ArgumentNullException("row");

            string ruleId = row[col_RuleCode].ToString();
            string ruleDescription = row[col_Description].ToString();
            DutyLevel dutyLevel = DutyLevel.All;
            if (row[col_ysjb] != DBNull.Value) dutyLevel = (DutyLevel)(int.Parse(row[col_ysjb].ToString()) - ConstRes.cstDoctorLevelNo);

            QCRule qcr = new QCRule(ruleId, ruleDescription, dutyLevel);
            qcr.TipInfo = row[col_Reminder].ToString();
            qcr.WarnInfo = row[col_FoulMessage].ToString();
            qcr.Timelimit = new TimeSpan(0, 0, Convert.ToInt32(double.Parse(row[col_TimeLimit].ToString())));
            if (row[col_RelatedMark] != DBNull.Value)
                qcr.RelateDealType = (RelateRuleDealType)int.Parse(row[col_RelatedMark].ToString());
            if (row[col_Mark] != DBNull.Value)
                qcr.DealType = (RuleDealType)int.Parse(row[col_Mark].ToString());
            if (qcr.DealType == RuleDealType.Loop)
            {
                qcr.LoopTimes = int.Parse(row[col_xhcs].ToString());
                qcr.LoopTimeInterVal = new TimeSpan(0, 0, Convert.ToInt32(double.Parse(row[col_xhjg].ToString())));
            }
            if (int.Parse(row[col_Valid].ToString()) == 1)
                qcr.Invalid = false;
            else
                qcr.Invalid = true;

            return qcr;
        }

        /// <summary>
        /// 取得指定的时限规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public QCRule GetQCRuleById(string ruleId)
        {
            DataRow drRule = GetQCRuleDataById(ruleId);
            return DataRow2QCRule(drRule);
        }

        /// <summary>
        /// 取得指定的时限规则,给定条件\结果集合
        /// </summary>
        /// <param name="ruleId"></param>
        /// <param name="conditions"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public QCRule GetQCRuleById(string ruleId, IList<QCCondition> conditions, IList<QCResult> results)
        {
            DataRow drRule = GetQCRuleDataById(ruleId);
            return DataRow2QCRule(drRule, conditions, results, true);
        }

        DataRow GetQCRuleDataById(string ruleId)
        { 
            SqlParameter paramRuleId = new SqlParameter(param_RuleCode, SqlDbType.VarChar, 64);
            paramRuleId.Value = ruleId;
            DataTable dtRule = _sqlHelper.ExecuteDataTable(SQL_SelectRuleById
                , new SqlParameter[] { paramRuleId });
            if (dtRule == null || dtRule.Rows.Count == 0) return null;
            DataRow drRule = dtRule.Rows[0];
            return drRule;
        }

        /// <summary>
        /// 保存时限规则条件到数据库
        /// </summary>
        /// <param name="rule"></param>
        public void SaveRule(QCRule rule)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            SqlParameter[] sqlparams = InitQCRuleParams(rule);
            if (rule.IsNew)
                _sqlHelper.ExecuteNoneQuery(SQL_InsertRule, sqlparams);
            else
                _sqlHelper.ExecuteNoneQuery(SQL_UpdateRule, sqlparams);
        }

        SqlParameter[] InitQCRuleParams(QCRule rule)
        {
            SqlParameter paramGzdm = new SqlParameter(param_RuleCode, SqlDbType.VarChar, 64);
            SqlParameter paramTjdm = new SqlParameter(param_ConditionCode, SqlDbType.VarChar, 64);
            SqlParameter paramJgdm = new SqlParameter(param_ResultCode, SqlDbType.VarChar, 64);
            SqlParameter paramGzms = new SqlParameter(param_Description, SqlDbType.VarChar, 64);
            SqlParameter paramTsxx = new SqlParameter(param_Reminder, SqlDbType.VarChar, 255);
            SqlParameter paramWgxx = new SqlParameter(param_FoulMessage, SqlDbType.VarChar, 255);
            SqlParameter paramGzsx = new SqlParameter(param_TimeLimit, SqlDbType.Float);
            SqlParameter paramXggz = new SqlParameter(param_RelatedRule, SqlDbType.VarChar, 1024);
            SqlParameter paramXgcl = new SqlParameter(param_RelatedMark, SqlDbType.Int);
            SqlParameter paramClbz = new SqlParameter(param_Mark, SqlDbType.Int);
            SqlParameter paramXhcs = new SqlParameter(param_xhcs, SqlDbType.Int);
            SqlParameter paramXhjg = new SqlParameter(param_xhjg, SqlDbType.Float);
            SqlParameter paramYsjb = new SqlParameter(param_ysjb, SqlDbType.Int);
            SqlParameter paramFldm = new SqlParameter(param_fldm, SqlDbType.VarChar, 64);
            SqlParameter paramYxjl = new SqlParameter(param_Valid, SqlDbType.Int);
            SqlParameter paramMemo = new SqlParameter(param_Memo, SqlDbType.VarChar, 64);

            paramGzdm.Value = rule.Id;
            paramTjdm.Value = rule.Condition.Id;
            paramJgdm.Value = rule.Result.Id;
            paramGzms.Value = rule.Name;
            paramTsxx.Value = rule.TipInfo;
            paramWgxx.Value = rule.WarnInfo;
            paramGzsx.Value = rule.Timelimit.TotalSeconds;
            paramXggz.Value = rule.RelateRuleIds;
            paramXgcl.Value = (int)rule.RelateDealType;
            paramClbz.Value = (int)rule.DealType;
            paramXhcs.Value = rule.LoopTimes;
            paramXhjg.Value = rule.LoopTimeInterVal.TotalSeconds;
            paramYsjb.Value = (int)rule.Dutylevel + ConstRes.cstDoctorLevelNo;
            if (rule.Group != null) paramFldm.Value = rule.Group.Id;
            paramYxjl.Value = rule.Invalid?0:1;
            paramMemo.Value = string.Empty;

            return new SqlParameter[]{
                paramGzdm ,paramTjdm ,paramJgdm ,paramGzms ,paramTsxx ,
                paramWgxx ,paramGzsx ,paramXggz ,paramXgcl ,paramClbz ,
                paramXhcs, paramXhjg,
                paramYsjb ,paramFldm, paramYxjl ,paramMemo
            };
        }

        /// <summary>
        /// 数据库删除规则
        /// </summary>
        /// <param name="rule"></param>
        public void DeleteQCRule(QCRule rule)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            SqlParameter paramGzdm = new SqlParameter(param_RuleCode, SqlDbType.VarChar, 64);
            paramGzdm.Value = rule.Id;
            _sqlHelper.ExecuteNoneQuery(SQL_DeleteRule,
                new SqlParameter[] { paramGzdm });
        }
        #endregion
    }
}
