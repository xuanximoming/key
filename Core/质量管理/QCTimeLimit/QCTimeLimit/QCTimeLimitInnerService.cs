using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrectSoft.DSSqlHelper;
using System.Data.SqlClient;
using System.Data;
using DrectSoft.Emr.QCTimeLimit.QCEntity;
using System.Collections;
using System.Globalization;

//**********************************
//********* Create by wwj **********
//**********************************

namespace DrectSoft.Emr.QCTimeLimit
{
    /// <summary>
    /// 病历时限管理，在服务器端以内部服务的方式循环调用
    /// </summary>
    public class QCTimeLimitInnerService
    {
        /// <summary>
        /// 时限规则条件集合
        /// </summary>
        Dictionary<string, QCCondition> m_DictQCCondition;

        /// <summary>
        /// 时限规则分类集合
        /// </summary>
        Dictionary<string, RuleCategory> m_DictQCCategory;

        /// <summary>
        /// 时限规则集合，包括有效和无效的记录
        /// </summary>
        Dictionary<string, QCRule> m_DictQCRuleAll;

        /// <summary>
        /// 时限规则集合，有效的记录
        /// </summary>
        Dictionary<string, QCRule> m_DictQCRuleValid;

        /// <summary>
        /// 时限规则记录集合
        /// </summary>
        List<QCRecord> m_ListQCRecord;

        /// <summary>
        /// .ctor
        /// </summary>
        public QCTimeLimitInnerService()
        {
            try
            {
                DS_SqlHelper.CreateSqlHelperByDBNameFormDA("EMRDB");
             //   .CreateSqlHelperByDBNameFormDA("EMRDB");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            try
            {
                m_DictQCCondition = QCCondition.GetAllQCCondition();
                m_DictQCCategory = RuleCategory.GetAllRuleCategory();
                //获得所有QCRule，包括有效和无效
                m_DictQCRuleAll = QCRule.GetAllQCRule(m_DictQCCondition, m_DictQCCategory);
                m_ListQCRecord = QCRecord.GetAllQCRecord(m_DictQCRuleAll, m_DictQCCondition, m_DictQCCategory);
                //获得所有有效的QCRule
                m_DictQCRuleValid = QCRule.GetAllQCRuleValid(m_DictQCCondition, m_DictQCCategory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 总处理程序，病历时限控制的入口点
        /// </summary>
        public void MainProcess()
        {
            try
            {
                //【1】初始化数据
                InitData();

                //【2】处理数据
                Process1();
                Process2();
                Process3();
                //Process4();
            }
            catch (Exception ex)
            {
                //Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                throw ex;
            }
        }

        #region 处理1
        /// <summary>
        /// 生成提醒信息，插入初始化数据至QCRecord表
        /// 如果是循环时限，则只插入循环的第一条记录
        /// </summary>
        void Process1()
        {
            try
            {
                foreach (KeyValuePair<string, QCRule> pair in m_DictQCRuleValid)
                {
                    QCRule rule = pair.Value;
                    Process1QCRule(rule);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理每一条时限规则
        /// </summary>
        /// <param name="rule"></param>
        void Process1QCRule(QCRule rule)
        {
            try
            {
                QCCondition condition = rule.Condition;

                //通过QCCondition表中的配置获得相应表中的结果集, 存在病人流水号时，需要将其转换为首页序号
                DataTable dtConfigResult = QCCondition.GetConfigDataResult(condition);

                foreach (DataRow dr in dtConfigResult.Rows)
                {
                    //通过首页序号NOOFINPAT + RULECODE 在QCRecord表中获取对应的记录
                    string noofinpat = dr[condition.PatNoColumnName].ToString();
                    string rulecode = rule.RuleCode;
                    List<QCRecord> listQCRecord = (from QCRecord rec in m_ListQCRecord
                                                   where rec.NoOfInpat == noofinpat && rec.Rule.RuleCode == rulecode
                                                   select rec).ToList<QCRecord>();

                    //无记录则插入初始化的数据
                    if (listQCRecord.Count() == 0)
                    {
                        QCRecord.InsertInitData(noofinpat, rule, dr);
                        continue;
                    }

                    //通过时间比较是否存在相同的
                    DateTime timeValue = Convert.ToDateTime(dr[condition.TimeColumnName].ToString());
                    var qcRecords = from QCRecord record in listQCRecord
                                    where record.ConditionTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture)
                                          == timeValue.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture)
                                    select record;

                    //表示已经处理过，所以不对其再次处理
                    if (qcRecords.Count() > 0)
                    {
                        continue;
                    }

                    //针对不同的操作方式进行不同的处理,确保 listQCRecord.count() > 0
                    Process1QCRuleMark(rule, dr, listQCRecord);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 针对不同的操作方式进行不同的处理
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="drConfigDataResult"></param>
        /// <param name="listQCRecord"></param>
        void Process1QCRuleMark(QCRule rule, DataRow drConfigDataResult, List<QCRecord> listQCRecord)
        {
            try
            {
                string noofinpat = drConfigDataResult[rule.Condition.PatNoColumnName].ToString();

                //判断A记录的操作方式
                switch (rule.MARK)
                {
                    //一次性，只能触发一次
                    case QCEnum.OperationType.OnlyOne:

                        #region Add By wwj 2013-02-27
                        //修改事件触发时间后，【1】删除原先就的时限提醒记录，【2】再插入初始化数据到QCRecord表中
                        //例如：修改入院入区时间，则需要先删除原先入院记录的提醒信息，然后插入新的入院记录提醒信息
                        foreach (QCRecord qcRecord in listQCRecord)
                        {
                            QCRecord.CancelQCRecordData(qcRecord.ID);//【1】
                        }
                        QCRecord.InsertInitData(noofinpat, rule, drConfigDataResult);//【2】
                        #endregion

                        break;
                    //触发一次执行一次
                    case QCEnum.OperationType.EveryOne:
                        //插入初始化数据到QCRecord表中
                        QCRecord.InsertInitData(noofinpat, rule, drConfigDataResult);
                        break;
                    //循环触发
                    case QCEnum.OperationType.Circle:
                        Process1QCRuleCircle(rule, drConfigDataResult, listQCRecord);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理循环触发的时限规则
        /// 进行有限次循环时，如果有限次的循环未结束，则外部的触发将会不起作用
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="rule"></param>
        /// <param name="drConfigDataResult"></param>
        /// <param name="listQCRecord"></param>
        void Process1QCRuleCircle(QCRule rule, DataRow drConfigDataResult, List<QCRecord> listQCRecord)
        {
            try
            {
                string noofinpat = drConfigDataResult[rule.Condition.PatNoColumnName].ToString();

                if (rule.MARK == QCEnum.OperationType.Circle && rule.CycleTimes > 0)//有限次循环
                {
                    //获得离当前时间最接近的第一条循环记录
                    QCRecord record = (from QCRecord qcrecord in listQCRecord
                                       where qcrecord.CycleTimes == 1
                                       orderby qcrecord.RealConditionTime descending
                                       select qcrecord).FirstOrDefault();

                    //由于循环记录的第一条记录对应的QCRecord.FirstCycleRecordID为空，所有下面计算的时候要减去1
                    int count = (from QCRecord qcrecord in listQCRecord
                                 where qcrecord.FirstCycleRecordID == record.ID
                                 select qcrecord).Count();

                    if (count < rule.CycleTimes - 1)//上一次的有限次的循环未结束，则外部的触发将会不起作用
                    {
                        return;
                    }
                    else if (count == rule.CycleTimes - 1)//上一次的有限次的循环已经结束，则外部的触发开始起作用
                    {
                        //插入初始化数据到QCRecord表中
                        QCRecord.InsertInitData(noofinpat, rule, drConfigDataResult);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 处理2

        /// <summary>
        /// 判断时限的违规情况
        /// 如果针对的是循环时限，需要动态的判断，并插入初始数据到QCRecord表中
        /// </summary>
        void Process2()
        {
            try
            {
                //更新时限记录的条件状态为完成
                //QCRecord.REALCONDITIONTIME（实际条件时间）>= SYSDATE（当前时间）则 QCRecord.CONDITION=1，即条件已完成
                QCRecord.UpdateConditionComplete();

                //更新时限记录中的完成状态至未完成 当病历删除后导致对应的时限记录重新生效
                QCRecord.UpdateResultToUnComplete();

                //更新病历的违规状态，针对条件成立但是未完成的记录 即QCRecord.Condition == 1 AND QCRecord.Result == 0
                //QCRecord.REALCONDITIONTIME（实际条件时间）+ QCRecord.TimeLimit < SYSDATE（当前时间）则 QCRecord.FoulState=1，即违规
                QCRecord.UpdateFoulState();

                //处理循环时限规则
                Process2QCRuleCircle();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 处理循环时限规则，在适当的时机插入初始化数据至QCRecord表中
        /// </summary>
        void Process2QCRuleCircle()
        {
            try
            {
                //获取当前时间用于后面逻辑的比较
                DateTime currentDateTime = GetCurrentDateTime();

                //捞取QCRecord表中的循环规则时限的记录
                List<QCRecord> listQCRecord = QCRecord.GetAllQCRecordCircle(m_DictQCRuleAll, m_DictQCCondition, m_DictQCCategory);

                //获取拥有循环时限规则记录的所有病人列表
                List<string> listNoOfInpat = (from QCRecord record in listQCRecord
                                              where record.IsCycle == true
                                              select record.NoOfInpat).Distinct().ToList();

                //获取拥有循环时限规则记录的所有的RuleCode
                List<string> ListRuleCode = (from QCRecord record in listQCRecord
                                             where record.IsCycle == true
                                             select record.Rule.RuleCode).Distinct().ToList();

                foreach (string noofinpat in listNoOfInpat)
                {
                    foreach (string ruleCode in ListRuleCode)
                    {
                        //根据 病人首页号 + RuleCode 获得时限记录QCRecord
                        List<QCRecord> listQCRecordByPat = (from QCRecord record in listQCRecord
                                                            where record.NoOfInpat.Equals(noofinpat) && record.Rule.RuleCode.Equals(ruleCode) && record.IsCycle == true
                                                            orderby record.CycleTimes descending
                                                            select record).ToList<QCRecord>();

                        if (listQCRecordByPat.Count() == 0) continue;

                        //指定病人noofinpat、指定规则代码RuleCode 循环的最后一条记录QCRecord
                        QCRecord recLast = listQCRecordByPat.FirstOrDefault<QCRecord>();

                        listQCRecordByPat = (from QCRecord record in listQCRecord
                                             where record.NoOfInpat.Equals(noofinpat) && record.Rule.RuleCode.Equals(ruleCode) && record.IsCycle == true
                                             orderby record.CycleTimes ascending
                                             select record).ToList<QCRecord>();

                        //指定病人noofinpat、指定规则代码RuleCode 循环的第一条记录QCRecord
                        QCRecord recFirst = listQCRecordByPat.FirstOrDefault<QCRecord>();

                        if (recLast.Rule.CycleTimes == 0)//无限次循环
                        {
                            //条件发生时间 + 循环时间间隔 < 当前时间 && 循环未停止
                            if (recLast.RealConditionTime.AddSeconds(recLast.TimeLimit) < currentDateTime && !recLast.IsStopCycle)
                            {
                                //插入初始化的数据到QCRecord表中
                                QCRecord.InsertInitDataForCircle(recLast, recFirst);
                            }
                        }
                        else//有限次循环
                        {
                            //条件发生时间 + 循环时间间隔 < 当前时间 && 当前循环的次数小于循环需要的总次数 && 循环未停止
                            if (recLast.RealConditionTime.AddSeconds(recLast.TimeLimit) < currentDateTime
                                && recLast.CycleTimes < recLast.Rule.CycleTimes
                                && !recLast.IsStopCycle)
                            {
                                //插入初始化的数据到QCRecord表中
                                QCRecord.InsertInitDataForCircle(recLast, recFirst);
                            }
                            else if (recLast.CycleTimes == recLast.Rule.CycleTimes && !recLast.IsStopCycle)
                            {
                                //循环次数到达上限，停止循环，设置最后一条记录的QCRecord.IsStopCycle = 1
                                QCRecord.UpdateStopCycle(recLast.ID, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 处理3

        /// <summary>
        /// 更新QCRecord表中的数据，结束满足条件的时限
        /// </summary>
        void Process3()
        {
            try
            {
                //获得所有未完成的病历时限记录，为了系统的性能考虑，只抓取在院且出院时间在30天之内的记录
                List<QCRecord> listQCRecord = QCRecord.GetAllQCRecordUnComplete(m_DictQCRuleAll, m_DictQCCondition, m_DictQCCategory);

                //按用户、质量规则、条件时间来排序
                listQCRecord = (from QCRecord rec in listQCRecord
                                orderby rec.NoOfInpat, rec.Rule.QCCode, rec.RealConditionTime
                                select rec).ToList();

                //为了性能上的考虑，使用字典保存已经捞取过的数据，避免重复捞取数据
                Dictionary<string, DataTable> dictRecorddetailByPat = new Dictionary<string, DataTable>();

                foreach (QCRecord record in listQCRecord)
                {
                    string noofinpat = record.NoOfInpat;
                    string qccode = record.QCCode;

                    DataTable dtRecordDetailID = new DataTable();

                    #region 捞取符合条件的病人号RecordDetailID，用于结束对应的时限记录QCRecord
                    if (dictRecorddetailByPat.ContainsKey(noofinpat))
                    {
                        dtRecordDetailID = dictRecorddetailByPat[noofinpat];
                    }
                    else
                    {
                        dtRecordDetailID = GetRecordDetailIDByPatAndQCCode(noofinpat);
                        dictRecorddetailByPat.Add(noofinpat, dtRecordDetailID);
                    }

                    if (dtRecordDetailID.Rows.Count == 0) continue;

                    List<DataRow> listDataRow = (from DataRow dr in dtRecordDetailID.Rows
                                                 where dr["qc_code"].ToString().Equals(qccode)
                                                 orderby dr["createtime"].ToString()
                                                 select dr).Distinct().ToList();

                    if (listDataRow.Count == 0) continue;
                    #endregion

                    //更新时限记录的已完成
                    string recordDetailID = listDataRow[0]["id"].ToString();
                    //对于创建时间为空的时间处理 一般不会有空时间  xll 2013-05-21
                    DateTime recordDetailCreateTime = DateTime.Now;
                    if (!string.IsNullOrEmpty(listDataRow[0]["createtime"].ToString()))
                    {
                        recordDetailCreateTime = Convert.ToDateTime(listDataRow[0]["createtime"]);
                    }
                    DateTime realConditionTime = record.RealConditionTime;
                    string foulState = "0";//违规情况 默认不违规 0：不违规 1：违规
                    if ((recordDetailCreateTime - realConditionTime).Seconds >= record.TimeLimit)
                    {
                        foulState = "1";
                    }
                    QCRecord.UpdateResultComplete(record.ID, recordDetailID, foulState);

                    //删除已经在QCRecord表中匹配过的记录
                    dtRecordDetailID.Rows.Remove(listDataRow[0]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 处理4
        /// <summary>
        /// 处理被删除的病历对病历时限记录造成的影响
        /// </summary>
        void Process4()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 获得当前时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetCurrentDateTime()
        {
            try
            {
                return Convert.ToDateTime(DS_SqlHelper.ExecuteScalar("select sysdate from dual", CommandType.Text));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取病人对应病历的QCCode,且该病历未记录在时限记录中
        /// </summary>
        const string c_SqlGetRecordDetailID =
            @"select recorddetail.id, emrtemplet.qc_code, recorddetail.audittime createtime
                from recorddetail, emrtemplet
               where recorddetail.templateid = emrtemplet.templet_id
                 and recorddetail.noofinpat = @noofinpat
                 and recorddetail.valid = '1'
                 and not exists (select 1 from qcrecord where qcrecord.recorddetailid = recorddetail.id and recorddetail.valid = '1' and  qcrecord.valid=1)";

        /// <summary>
        /// 获得未记录在时限记录表中的病历ID
        /// 为了系统的性能考虑，只抓取在院且出院时间在30天只能的记录
        /// </summary>
        /// <returns></returns>
        public static DataTable GetRecordDetailIDByPatAndQCCode(string noofinpat)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] { 
                    new SqlParameter("@noofinpat", SqlDbType.NVarChar)
                };
                parms[0].Value = noofinpat;
                DataTable dt = DS_SqlHelper.ExecuteDataTable(c_SqlGetRecordDetailID, parms, CommandType.Text);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
