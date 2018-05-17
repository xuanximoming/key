using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DrectSoft.Emr.QCTimeLimit.QCEnum;
using DrectSoft.DSSqlHelper;

namespace DrectSoft.Emr.QCTimeLimit.QCEntity
{
    /// <summary>
    /// 质量控制规则库
    /// QCRULE表对应的实体类
    /// </summary>
    public class QCRule
    {
        #region Property
        /// <summary>
        /// 规则代码
        /// </summary>
        public string RuleCode { get; set; }

        /// <summary>
        /// 规则条件代码 QCCondition
        /// </summary>
        public QCCondition Condition { get; set; }

        /// <summary>
        /// 规则描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Reminder { get; set; }

        /// <summary>
        /// 违规信息
        /// </summary>
        public string FoulMessage { get; set; }

        /// <summary>
        /// 延迟时间(以秒计算)
        /// </summary>
        public int DelayTime { get; set; }

        /// <summary>
        /// 规则时限(以秒计算)
        /// </summary>
        public int TimeLimit { get; set; }

        /// <summary>
        /// 操作方式  0:一次性  1: 触发一次执行一次 2:循环
        /// </summary>
        public OperationType MARK { get; set; }

        /// <summary>
        /// 循环处理时的次数(0=无穷) MARK==2时有值，否则为NULL
        /// </summary>
        public int CycleTimes { get; set; }

        /// <summary>
        /// 循环间隔	(单位:秒) MARK==2时有效，否则为NULL
        /// </summary>
        public int CycleInterval { get; set; }

        /// <summary>
        /// 医生级别  CategoryDetail.CategoryID = 20
        /// </summary>
        public DoctorGrade DoctorLevel { get; set; }

        /// <summary>
        /// 规则分类代码 RuleCategory
        /// </summary>
        public RuleCategory RuleCategory { get; set; }

        /// <summary>
        /// 为了方便前端在GridView中展示数据时分组方便，所以在这里增加了冗余的字段 这里 RuleCategoryName == RuleCategory.Description
        /// </summary>
        public string RuleCategoryName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 扣分
        /// </summary>
        public double Sorce { get; set; }

        /// <summary>
        /// 监控代码
        /// </summary>
        public string QCCode { get; set; }

        /// <summary>
        /// 有效字段
        /// </summary>
        public string Valid { get; set; }

        #endregion

        /// <summary>
        /// 获得所有记录，包括有效和无效的记录
        /// </summary>
        const string c_SqlQCRuleAll = "select * from QCRule";

        /// <summary>
        /// 获得所有有效的记录
        /// </summary>
        const string c_SqlQCRuleValid = "select * from QCRule where valid = '1'";

        /// <summary>
        /// 获取指定字段的记录
        /// </summary>
        const string c_SqlQCRule = "select RULECODE,CONDITIONCODE,DESCRIPTION,"
           + "REMINDER,FOULMESSAGE,DELAYTIME,TIMELIMIT,MARK,CYCLETIMES,CYCLEINTERVAL,"
           + "DOCOTORLEVEL,SORTCODE,MEMO,SCORE,QCCODE,valid from QCRule  order by sortcode ";//具体到列 by xlb 2013-01-06
        //通过条件代码获取使用是否该条件的实现规则
        const string c_sqlCountQcRule = " select count(1) from qcrule  where conditioncode=@conditionCode";
        /// <summary>
        /// 删除指定的记录
        /// </summary>
        const string c_sqlDelete = "delete from  QCRule  where rulecode=@rulecode ";
        //通过监控代码查询被使用该代码的规则条数的sql
        const string c_sqlCountByQcCode = " select count(1) from qcrule  where qccode=@qcCOde";

        #region 获得所有的病历质量控制规则库

        ///// <summary>
        ///// 获得所有的病历质量控制规则库 DataTable -> Dictionary<string, QCRule>
        ///// </summary>
        ///// <param name="dictCondition"></param>
        ///// <param name="dictCategory"></param>
        ///// <returns></returns>
        //public static Dictionary<string, QCRule> GetAllQCRule(Dictionary<string, QCCondition> dictCondition, Dictionary<string, RuleCategory> dictCategory)
        //{
        //    try
        //    {
        //        DataTable dataTableRules = GetAllQCRules();
        //        return GetAllQCRule(dataTableRules, dictCondition, dictCategory);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 获得所有的病历质量控制规则库 DataTable -> Dictionary<string, QCRule>
        /// </summary>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        public static Dictionary<string, QCRule> GetAllQCRule(Dictionary<string, QCCondition> dictCondition, Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                DataTable dataTableRules = GetAllQCRules2();
                return GetAllQCRule(dataTableRules, dictCondition, dictCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得所有有效的病历质量控制规则库 DataTable -> Dictionary<string, QCRule>
        /// </summary>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        public static Dictionary<string, QCRule> GetAllQCRuleValid(Dictionary<string, QCCondition> dictCondition, Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                DataTable dataTableRules = GetAllQCRulesValid();
                return GetAllQCRule(dataTableRules, dictCondition, dictCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得病历质量控制规则库 DataTable -> Dictionary<string, QCRule>
        /// </summary>
        /// <param name="dataTableRules"></param>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        public static Dictionary<string, QCRule> GetAllQCRule(DataTable dataTableRules, Dictionary<string, QCCondition> dictCondition, Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                Dictionary<string, QCRule> ruleDictionary = new Dictionary<string, QCRule>();
                foreach (DataRow dr in dataTableRules.Rows)
                {
                    QCRule rule = ConvertToQCRuleFromDataRow(dr, dictCondition, dictCategory);
                    ruleDictionary.Add(rule.RuleCode, rule);
                }
                return ruleDictionary;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得时限规则集合
        /// by xlb 2013-01-05
        /// </summary>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        public static IList<QCRule> GetRuleList(Dictionary<string, QCCondition> dictCondition, Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                IList<QCRule> ruleList = new List<QCRule>();
                DataTable dtRuleList = GetAllQCRules();
                foreach (DataRow dr in dtRuleList.Rows)
                {
                    QCRule rule = ConvertToQCRuleFromDataRow(dr, dictCondition, dictCategory);
                    ruleList.Add(rule);
                }
                return ruleList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从数据行对象转换为质量规则对象，即 DataRow -> QCRule
        /// Modify by xlb 2013-03-25 扣分项可带小数 转换为整型引起错误
        /// </summary>
        /// <param name="dataRowRule"></param>
        /// <param name="dictCondition"></param>
        /// <param name="dictCategory"></param>
        /// <returns></returns>
        static QCRule ConvertToQCRuleFromDataRow(DataRow dataRowRule, Dictionary<string, QCCondition> dictCondition, Dictionary<string, RuleCategory> dictCategory)
        {
            try
            {
                int result;
                QCRule rule = new QCRule();

                #region QCRule实例赋值
                rule.RuleCode = dataRowRule["RULECODE"].ToString();
                rule.Condition = dictCondition[dataRowRule["CONDITIONCODE"].ToString()];

                rule.Description = dataRowRule["DESCRIPTION"].ToString();
                rule.Reminder = dataRowRule["REMINDER"].ToString();
                rule.FoulMessage = dataRowRule["FOULMESSAGE"].ToString();
                rule.DelayTime = 0;
                if (int.TryParse(dataRowRule["DELAYTIME"].ToString(), out result))
                {
                    rule.DelayTime = result;
                }
                rule.TimeLimit = 0;
                if (int.TryParse(dataRowRule["TIMELIMIT"].ToString(), out result))
                {
                    rule.TimeLimit = result;
                }
                rule.MARK = (OperationType)Enum.Parse(typeof(OperationType), dataRowRule["MARK"].ToString());
                rule.CycleTimes = 0;
                if (int.TryParse(dataRowRule["CYCLETIMES"].ToString(), out result))
                {
                    rule.CycleTimes = result;
                }
                rule.CycleInterval = 0;
                if (int.TryParse(dataRowRule["CYCLEINTERVAL"].ToString(), out result))
                {
                    rule.CycleInterval = result;
                }
                rule.DoctorLevel = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), dataRowRule["DOCOTORLEVEL"].ToString());

                string sortCode = dataRowRule["SORTCODE"].ToString().Trim();
                if (sortCode != "" && dictCategory.ContainsKey(sortCode))
                {
                    rule.RuleCategory = dictCategory[sortCode];
                    rule.RuleCategoryName = sortCode + " " + rule.RuleCategory.Description;
                }

                rule.Memo = dataRowRule["MEMO"].ToString();
                rule.Sorce = 0;
                double Qcscore = 0;
                if (Double.TryParse(dataRowRule["SCORE"].ToString(), out Qcscore))
                {
                    rule.Sorce = Qcscore;
                }
                rule.QCCode = dataRowRule["QCCODE"].ToString();
                rule.Valid = dataRowRule["VALID"].ToString();
                #endregion

                return rule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有时限规则
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllQCRules()
        {
            try
            {
                DataTable dtQCRule = DS_SqlHelper.ExecuteDataTable(c_SqlQCRule, CommandType.Text);
                return dtQCRule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有时限规则，包括有效和无效的记录
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllQCRules2()
        {
            try
            {
                DataTable dtQCRule = DS_SqlHelper.ExecuteDataTable(c_SqlQCRuleAll, CommandType.Text);
                return dtQCRule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有有效的时限规则
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllQCRulesValid()
        {
            try
            {
                DataTable dtQCRule = DS_SqlHelper.ExecuteDataTable(c_SqlQCRuleValid, CommandType.Text);
                return dtQCRule;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除时限规则
        /// xlb 2013-01-06
        /// </summary>
        /// <param name="rule"></param>
        public static void DeleteQcRule(QCRule rule)
        {
            try
            {
                if (rule == null)
                {
                    return;
                }
                SqlParameter[] sps = { new SqlParameter("@rulecode", rule.RuleCode == null ? "" : rule.RuleCode) };
                DS_SqlHelper.ExecuteNonQuery(c_sqlDelete, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除时限规则
        /// xlb 2013-01-09
        /// </summary>
        /// <param name="ruleCode"></param>
        public static void DeleteQcRule(string ruleCode)
        {
            try
            {
                if (ruleCode == null)
                {
                    return;
                }
                SqlParameter[] sps = { new SqlParameter("@rulecode", ruleCode) };
                DS_SqlHelper.ExecuteNonQuery(c_sqlDelete, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过条件代码获取是否该条件被规则使用
        /// </summary>
        /// <returns></returns>
        public static int QcRuleCountByConditionCode(string conditionCode)
        {
            try
            {
                SqlParameter[] sps = { new SqlParameter("@conditioncode", conditionCode) };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(c_sqlCountQcRule, sps, CommandType.Text);
                int count = int.Parse(dt.Rows[0][0].ToString());
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过监控代码获取是否被使用
        /// Add xlb 
        /// </summary>
        /// <param name="QcCode">监控代码</param>
        /// <returns>返回使用条数</returns>
        public static int QcRuleByQcCode(string QcCode)
        {
            try
            {
                SqlParameter[] sps = { new SqlParameter("@qcCode", QcCode) };
                DataTable dt = DS_SqlHelper.ExecuteDataTable(c_sqlCountByQcCode, sps, CommandType.Text);
                int count = int.Parse(dt.Rows[0][0].ToString());
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增时限规则
        /// by xlb 2013-01-06
        /// </summary>
        /// <param name="qcRule"></param>
        public static void InsertQcRule(QCRule qcRule, QCCondition qcCondition, RuleCategory ruleCategory)
        {
            try
            {
                string c_SqlInsert = "insert into QCRule(RULECODE,CONDITIONCODE,DESCRIPTION,"
                                   + "REMINDER,FOULMESSAGE,DELAYTIME,TIMELIMIT,MARK,CYCLETIMES,CYCLEINTERVAL,"
                                   + "DOCOTORLEVEL,SORTCODE,MEMO,SCORE,QCCODE,valid) values(seq_qcrulecode.nextval,@CONDITIONCODE,@DESCRIPTION,"
                                   + "@REMINDER,@FOULMESSAGE,@DELAYTIME,@TIMELIMIT,@MARK,@CYCLETIMES,@CYCLEINTERVAL,@DOCOTORLEVEL,"
                                   + "@SORTCODE,@MEMO,@SCORE,@QCCODE,@valid)";
                SqlParameter[] sps ={
                               new SqlParameter("@CONDITIONCODE",qcCondition.Code),
                               new SqlParameter("@DESCRIPTION",qcRule.Description),
                               new SqlParameter("@REMINDER",qcRule.Reminder),
                               new SqlParameter("@FOULMESSAGE",qcRule.FoulMessage),
                               new SqlParameter("@TIMELIMIT",qcRule.TimeLimit),
                               new SqlParameter("@MARK",qcRule.MARK),
                               new SqlParameter("@CYCLETIMES",qcRule.CycleTimes),
                               new SqlParameter("@CYCLEINTERVAL",qcRule.CycleInterval),
                               new SqlParameter("@DOCOTORLEVEL",(int)qcRule.DoctorLevel),
                               new SqlParameter("@SORTCODE",ruleCategory.Code),
                               new SqlParameter("@valid",qcRule.Valid),
                               new SqlParameter("@MEMO",qcRule.Valid),
                               new SqlParameter("@DELAYTIME",qcRule.DelayTime),
                               new SqlParameter("@SCORE",qcRule.Sorce),
                               new SqlParameter("@QCCODE",qcRule.QCCode),
                            
                                };
                DS_SqlHelper.ExecuteNonQuery(c_SqlInsert, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 修改时限规则信息
        /// by xlb 2013-01-07
        /// </summary>
        /// <param name="qcRule"></param>
        public static void UpdateQcRule(QCRule qcRule, QCCondition qcCondition, RuleCategory ruleCategory)
        {
            try
            {
                string c_sqlUpdate = @"update QCRule set conditioncode=@conditioncode,description=@description,reminder=@reminder,"
                                    + "foulmessage=@foculmessage,timelimit=@timelimit,mark=@mark,cycletimes=@cycletimes,"
                                    + "cycleinterval=@cycleinterval,"
                                    + "docotorlevel=@docotorlevel,sortcode=@sortcode,valid=@valid,memo=@memo,"
                                    + "delaytime=@delaytime,score=@score,qccode=@qccode where rulecode=@rulecode";

                SqlParameter[] sps ={
                               new SqlParameter("@conditioncode",qcCondition.Code),
                               new SqlParameter("@description",qcRule.Description),
                               new SqlParameter("@reminder",qcRule.Reminder),
                               new SqlParameter("@foculmessage",qcRule.FoulMessage),
                               new SqlParameter("@timelimit",qcRule.TimeLimit),
                               new SqlParameter("@mark",(int)qcRule.MARK),
                               new SqlParameter("@cycletimes",qcRule.CycleTimes),
                               new SqlParameter("@cycleinterval",qcRule.CycleInterval),
                               new SqlParameter("@docotorlevel",(int)qcRule.DoctorLevel),
                               new SqlParameter("@sortcode",ruleCategory.Code),
                               new SqlParameter("@valid",qcRule.Valid),
                               new SqlParameter("@memo",qcRule.Memo),
                               new SqlParameter("@delaytime",qcRule.DelayTime),
                               new SqlParameter("@score",qcRule.Sorce),
                               new SqlParameter("@qccode",qcRule.QCCode),
                               new SqlParameter("@rulecode",qcRule.RuleCode)
                                };
                DS_SqlHelper.ExecuteNonQuery(c_sqlUpdate, sps, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


    }
}
