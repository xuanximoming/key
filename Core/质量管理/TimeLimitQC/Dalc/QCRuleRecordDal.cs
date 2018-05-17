using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DrectSoft.Core.TimeLimitQC;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 时限纪录访问类
    /// </summary>
    public class QCRuleRecordDal
    {
        #region fields

        const string cstWhereUndo = " and a.FoulState in (1,3) ";
        const string cstSelectPatientRules =
            " select a.ID,a.NoOfInpat,a.NoOfRecord,a.RuleCode,a.ConditionCode,a.ConditionTime,a.Condition,a.ResultCode,a.ResultTime,a.Result,a.FoulState,a.Reminder "
            + ",a.FoulMessage,a.OperateTime,a.DocotorID,a.CRDocotorID,a.DocotorLevel,a.CycleTimes,a.Valid,a.Memo,b.Name,b.PatID,b.AdmitBed "
            + ",''UserName, d.TimeLimit,Users.Name AS CRDocotorName "
            + " from QCRecord a join InPatient b on a.NoOfInpat=b.NoOfInpat "
            + " left join QCRule d on a.RuleCode=d.RuleCode "
            + " left join Users c on a.DocotorID=c.ID "
            + " LEFT JOIN Users  ON a.CRDocotorID = Users.ID" //add by xjt,20100713
            + " where a.NoOfInpat=@NoOfInpat and a.Valid!=@Valid ";
        const string cstSelectDoctorRules =
            " select a.ID,a.NoOfInpat,a.NoOfRecord,a.RuleCode,a.ConditionCode,a.ConditionTime,a.Condition,a.ResultCode,a.ResultTime,a.Result,a.FoulState,a.Reminder "
            + ",a.FoulMessage,a.OperateTime,a.DocotorID,a.CRDocotorID,a.DocotorLevel,a.CycleTimes,a.Valid,a.Memo,b.Name,b.PatID,b.AdmitBed "
            + ", c.Name UserName"
            + ", d.TimeLimit "
            + " from QCRecord a left join InPatient b on a.NoOfInpat=b.NoOfInpat"
            + " left join QCRule d on a.RuleCode=d.RuleCode "
            + " left join Users c on c.ID= @DocotorID  "
            //+ " join Users c on c.ID= @DocotorID and b.OutBed = c.DeptId and b.OutHosWard = c.WardID and (a.DocotorLevel < 2000 or ((a.DocotorID = '' or a.DocotorID is null and c.Grade = a.DocotorLevel) or a.DocotorID = c.ID))"
            + " where a.NoOfInpat in(select NoOfInpat from doctor_assignpatient where ID=@DocotorID union select noofinpat from inpatient where inpatient.resident=@DocotorID) and a.Valid!=@Valid  "
            + " and a.FoulState in (1,3) and d.RuleCode is not null "//Add By wwj 2011-09-20
            + " order by  a.rulecode";
        const string cstInsertRuleRecord =
            " insert into QCRecord(ID,NoOfInpat,NoOfRecord,RuleCode,ConditionCode,ConditionTime,Condition,ResultCode,ResultTime "
            + ",Result,FoulState,Reminder,FoulMessage,OperateTime,DocotorID,CRDocotorID,DocotorLevel,CycleTimes,Valid,Memo) "
            + "values(@ID,@NoOfInpat, @NoOfRecord, @RuleCode, @ConditionCode, @ConditionTime, @Condition, @ResultCode, @ResultTime "
            + ",@Result, @FoulState, @Reminder, @FoulMessage, @OperateTime, @DocotorID,@CRDocotorID, @DocotorLevel, @CycleTimes, @Valid, @Memo)";
        const string cstUpdateRuleRecord =
            " update QCRecord set NoOfRecord=@NoOfRecord,ResultCode=@ResultCode, ResultTime=@ResultTime "
            + ",Result=@Result,FoulState=@FoulState,OperateTime=@OperateTime, CycleTimes=@CycleTimes,Valid=@Valid, Memo=@Memo "
            + " where ID=@ID ";
        const string cstCancelRuleRecord =
            " update QCRecord set ResultTime=@ResultTime, Valid=@Valid "
            + " where ID=@ID ";
        const string cstCancelRelateRuleRecord =
            " update QCRecord set Valid=0 "
            + " from QCRecord c "
            + " where exists(select 1 from QCRecord a, QCRule b "
            + "     where charindex(c.RuleCode, b.xggz)>0 and a.ID=@ID) "
            + " and c.FoulState in(1,3) ";
        const string cstSyncRelateRuleRecord =
            " update QCRecord set FoulState=@FoulState, ResultTime=@ResultTime "
            + " from QCRecord c "
            + " where exists(select 1 from QCRecord a, QCRule b "
            + "     where charindex(c.RuleCode, b.xggz)>0 and a.ID=@ID) "
            + " and c.FoulState in(1,3)";
        const string cstTriggerRelateRuleRecord =
            " update QCRecord set Valid=1 "
            + " from QCRecord c "
            + " where exists(select 1 from QCRecord a, QCRule b "
            + "     where charindex(c.RuleCode, b.xggz)>0 and a.ID=@ID) "
            + " and c.Valid=@Valid ";
        const string cstSelectUndoRuleRecordByDealType =
            "select a.ID,a.NoOfInpat,a.NoOfRecord,a.RuleCode,a.ConditionCode,a.ConditionTime,a.Condition,a.ResultCode,a.ResultTime,a.Result,a.FoulState,a.Reminder "
            + " ,a.FoulMessage,a.OperateTime,a.DocotorID,a.CRDocotorID,a.DocotorLevel,a.CycleTimes,a.Valid,a.Memo "
            + " from QCRecord a join QCRule d on a.RuleCode=d.RuleCode "
            + " where d.Mark=@Mark and a.Valid!=@Valid and a.FoulState in (1,3) ";
        const string cstEffectRuleRecord =
            " update QCRecord set FoulState=@FoulState "
            + " where ID=@ID ";
        const string cstQueryMaxID = "select max(ID) from QCRecord";

        const string param_xh = "ID";
        const string param_syxh = "NoOfInpat";
        const string param_blxh = "NoOfRecord";
        const string param_gzdm = "RuleCode";
        const string param_tjdm = "ConditionCode";
        const string param_tjsj = "ConditionTime";
        const string param_tjzt = "Condition";
        const string param_jgdm = "ResultCode";
        const string param_jgsj = "ResultTime";
        const string param_jgzt = "Result";
        const string param_wgzt = "FoulState";
        const string param_tsxx = "Reminder";
        const string param_wgxx = "FoulMessage";
        const string param_czsj = "OperateTime";
        const string param_zgdm = "DocotorID";
        const string param_cjzgdm = "CRDocotorID";
        const string param_zrjb = "DocotorLevel";
        const string param_xhjs = "CycleTimes";
        const string param_jlzt = "Valid";
        const string param_memo = "Memo";
        const string param_clbz = "Mark";
        const string col_xh = "ID";
        const string col_syxh = "NoOfInpat";
        const string col_blxh = "NoOfRecord";
        const string col_gzdm = "RuleCode";
        const string col_tjdm = "ConditionCode";
        const string col_tjsj = "ConditionTime";
        const string col_tjzt = "Condition";
        const string col_jgdm = "ResultCode";
        const string col_jgsj = "ResultTime";
        const string col_jgzt = "Result";
        const string col_wgzt = "FoulState";
        const string col_tsxx = "Reminder";
        const string col_wgxx = "FoulMessage";
        const string col_czsj = "OperateTime";
        const string col_zgdm = "DocotorID";
        const string col_cjzgdm = "CRDocotorID";
        const string col_zrjb = "DocotorLevel";
        const string col_xhjs = "CycleTimes";
        const string col_jlzt = "Valid";
        const string col_memo = "Memo";

        IDataAccess _sqlHelper;

        #endregion

        #region ctor

        /// <summary>
        /// ctor
        /// </summary>
        public QCRuleRecordDal()
        {
            _sqlHelper = DataAccessFactory.GetSqlDataAccess();
        }
        #endregion

        #region dalc procedures

        /// <summary>
        /// 数据行 -> 时限纪录
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public QCRuleRecord DataRow2QCRuleRecord(DataRow row)
        {
            decimal xh = decimal.Parse(row[col_xh].ToString());
            int patid = int.Parse(row[col_syxh].ToString());
            int eprid = int.Parse(row[col_blxh].ToString());
            QCRule rule = QCRule.SelectQCRule(row[col_gzdm].ToString());
            QCRuleRecord record = new QCRuleRecord(xh, patid, eprid, rule);
            record.CreateDoctor = row[col_cjzgdm].ToString();
            record.CreateTime = DateTime.Parse(row[col_czsj].ToString());
            record.DutyDoctor = row[col_zgdm].ToString();
            record.ConditionTime = DateTime.Parse(row[col_tjsj].ToString());
            record.ConditionState = (CompleteType)int.Parse(row[col_tjzt].ToString());
            record.ResultTime = DateTime.Parse(row[col_jgsj].ToString());
            record.ResultState = (CompleteType)int.Parse(row[col_jgzt].ToString());
            record.RuleState = (RuleRecordState)int.Parse(row[col_wgzt].ToString());
            if (row[col_xhjs] != DBNull.Value)
                record.LoopCount = int.Parse(row[col_xhjs].ToString());
            return record;
        }

        /// <summary>
        /// 取得病人已有的规则校验信息
        /// </summary>
        /// <param name="patid">病人Id</param>
        /// <returns>时限纪录数据集</returns>
        public DataSet GetPatientRulesDataSet(int patid)
        {
            SqlParameter paramSyxh = new SqlParameter(param_syxh, SqlDbType.Int);
            paramSyxh.Value = patid;
            SqlParameter paramJlzt = new SqlParameter(param_jlzt, SqlDbType.Int);
            paramJlzt.Value = (int)RecordState.Invalid; //注意这里的sql语句是Valid !=
            return _sqlHelper.ExecuteDataSet(cstSelectPatientRules,
                new SqlParameter[] { paramSyxh, paramJlzt });
        }

        /// <summary>
        /// 取得医生已有的规则校验信息
        /// </summary>
        /// <param name="doctorId">医生代码</param>
        /// <returns></returns>
        public DataSet GetDoctorRulesDataSet(string doctorId)
        {
            SqlParameter paramZgdm = new SqlParameter(param_zgdm, SqlDbType.VarChar, 6);
            paramZgdm.Value = doctorId;
            SqlParameter paramJlzt = new SqlParameter(param_jlzt, SqlDbType.Int);
            paramJlzt.Value = (int)RecordState.Invalid;    //注意这里的sql语句是Valid !=
            return _sqlHelper.ExecuteDataSet(cstSelectDoctorRules,
                new SqlParameter[] { paramZgdm, paramJlzt });
        }

        string GetDutyDoctorId(DoctorManagerPatient patient, DutyLevel dutylevel)
        {
            if (patient == null) return string.Empty;

            string doctor = string.Empty;
            switch (dutylevel)
            {
                case DutyLevel.All:
                    break;
                case DutyLevel.CWYS:
                    doctor = patient.Resident;
                    break;
                case DutyLevel.ZZYS:
                    doctor = patient.Attend;
                    break;
                case DutyLevel.ZRYS:
                    doctor = patient.Chief;
                    break;
                default:
                    break;
            }
            return doctor;
        }

        /// <summary>
        /// 插入符合条件后触发的时限规则
        /// </summary>
        /// <param name="patient">当前规则的责任医生代码</param>
        /// <param name="record"></param>
        public void InsertPatientRuleRecord(DoctorManagerPatient patient, QCRuleRecord record)
        {
            if (record == null) throw new ArgumentNullException("record");
            int retxh;
            //寻找最大值
            using (IDataReader reader = _sqlHelper.ExecuteReader(cstQueryMaxID))
            {
                reader.Read();
                if (reader.IsDBNull(0))
                    retxh = 0;
                else
                    retxh = Convert.ToInt32(reader.GetValue(0)) + 1;
            }
            record.Xh = retxh;
            SqlParameter[] insertparams = InitPatientRuleParams(patient, record);


            _sqlHelper.ExecuteNoneQuery(cstInsertRuleRecord, insertparams);
            record.Xh = decimal.Parse(retxh.ToString());
        }

        /// <summary>
        /// 更新时限规则的状态
        /// </summary>
        /// <param name="patient">当前规则的责任医生代码</param>
        /// <param name="record"></param>
        public void UpdatePatientRuleRecord(DoctorManagerPatient patient, QCRuleRecord record)
        {
            if (record == null) throw new ArgumentNullException("record");
            SqlParameter[] updateparams = InitPatientRuleParams(patient, record);
            _sqlHelper.ExecuteNoneQuery(cstUpdateRuleRecord, updateparams);
        }

        /// <summary>
        /// 取消时限规则
        /// </summary>
        /// <param name="record"></param>
        public void CancelPatientRuleRecord(QCRuleRecord record)
        {
            if (record == null) throw new ArgumentNullException("record");
            SqlParameter[] cancelparams = InitCancelRuleParams(record);
            _sqlHelper.ExecuteNoneQuery(cstCancelRuleRecord, cancelparams);
        }

        /// <summary>
        /// 取消相关规则记录
        /// </summary>
        /// <param name="record"></param>
        public void CancelRelateRuleRecord(QCRuleRecord record)
        {
            if (record == null) throw new ArgumentNullException("record");
            SqlParameter[] updateparams = InitRelateRecordParams(record);
            _sqlHelper.ExecuteNoneQuery(cstCancelRelateRuleRecord, updateparams);
        }

        /// <summary>
        /// 同步相关规则记录
        /// </summary>
        /// <param name="record"></param>
        public void SyncRelateRuleRecord(QCRuleRecord record)
        {
            if (record == null) throw new ArgumentNullException("record");
            SqlParameter[] updateparams = InitRelateRecordParams(record);
            _sqlHelper.ExecuteNoneQuery(cstSyncRelateRuleRecord, updateparams);
        }

        public void TriggerRelateRuleRecord(QCRuleRecord record)
        {
            if (record == null) throw new ArgumentNullException("record");
            SqlParameter[] triggerparams = InitTriggerRelateRuleParams(record);
            _sqlHelper.ExecuteNoneQuery(cstTriggerRelateRuleRecord, triggerparams);
        }

        SqlParameter[] InitCancelRuleParams(QCRuleRecord record)
        {
            SqlParameter paramXh = new SqlParameter(param_xh, SqlDbType.Decimal);
            SqlParameter paramJgsj = new SqlParameter(param_jgsj, SqlDbType.VarChar, 19);
            SqlParameter paramJlzt = new SqlParameter(param_jlzt, SqlDbType.Int);
            paramXh.Value = record.Xh;
            paramJgsj.Value = record.ResultTime.ToString("yyyy-MM-dd HH:mm:ss"); //record.ResultTime.ToString("yyyy-MM-dd") + " " + record.ResultTime.ToString("T");
            paramJlzt.Value = (int)record.RecordState;
            return new SqlParameter[] { 
                paramXh, paramJgsj, paramJlzt
                  };
        }

        SqlParameter[] InitPatientRuleParams(DoctorManagerPatient patient, QCRuleRecord record)
        {
            SqlParameter paramXh = new SqlParameter(param_xh, SqlDbType.Decimal);
            SqlParameter paramSyxh = new SqlParameter(param_syxh, SqlDbType.Int);
            SqlParameter paramBlxh = new SqlParameter(param_blxh, SqlDbType.Int);
            SqlParameter paramGzdm = new SqlParameter(param_gzdm, SqlDbType.VarChar, 64);
            SqlParameter paramTjdm = new SqlParameter(param_tjdm, SqlDbType.VarChar, 64);
            SqlParameter paramTjsj = new SqlParameter(param_tjsj, SqlDbType.VarChar, 19);
            SqlParameter paramTjzt = new SqlParameter(param_tjzt, SqlDbType.Int);
            SqlParameter paramJgdm = new SqlParameter(param_jgdm, SqlDbType.VarChar, 64);
            SqlParameter paramJgsj = new SqlParameter(param_jgsj, SqlDbType.VarChar, 19);
            SqlParameter paramJgzt = new SqlParameter(param_jgzt, SqlDbType.Int);
            SqlParameter paramWgzt = new SqlParameter(param_wgzt, SqlDbType.Int);
            SqlParameter paramTsxx = new SqlParameter(param_tsxx, SqlDbType.VarChar, 255);
            SqlParameter paramWgxx = new SqlParameter(param_wgxx, SqlDbType.VarChar, 255);
            SqlParameter paramCzsj = new SqlParameter(param_czsj, SqlDbType.VarChar, 19);
            SqlParameter paramZgdm = new SqlParameter(param_zgdm, SqlDbType.VarChar, 6);
            SqlParameter paramCjzgdm = new SqlParameter(param_cjzgdm, SqlDbType.VarChar, 6);
            SqlParameter paramZrjb = new SqlParameter(param_zrjb, SqlDbType.Int);
            SqlParameter paramXhjs = new SqlParameter(param_xhjs, SqlDbType.Int);
            SqlParameter paramJlzt = new SqlParameter(param_jlzt, SqlDbType.Int);
            SqlParameter paramMemo = new SqlParameter(param_memo, SqlDbType.VarChar, 64);

            paramXh.Value = record.Xh;
            paramSyxh.Value = record.PatId;
            paramBlxh.Value = record.EprId;
            paramGzdm.Value = record.Rule.Id;
            paramTjdm.Value = record.Rule.Condition.Id;
            paramTjsj.Value = record.ConditionTime.ToString("yyyy-MM-dd HH:mm:ss"); //record.ConditionTime.ToString("yyyy-MM-dd") + " " + record.ConditionTime.ToString("T");
            paramTjzt.Value = (int)record.ConditionState;
            if (record.Rule.Result != null)
                paramJgdm.Value = record.Rule.Result.Id;
            paramJgsj.Value = record.ResultTime.ToString("yyyy-MM-dd HH:mm:ss"); //record.ResultTime.ToString("yyyy-MM-dd") + " " + record.ResultTime.ToString("T");
            paramJgzt.Value = (int)record.ResultState;
            paramWgzt.Value = (int)record.RuleState;
            paramTsxx.Value = record.Rule.TipInfo;
            paramWgxx.Value = record.Rule.WarnInfo;
            paramCzsj.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("T");
            paramZgdm.Value = GetDutyDoctorId(patient, record.Rule.Dutylevel);
            paramCjzgdm.Value = record.CreateDoctor;
            paramZrjb.Value = (int)record.Rule.Dutylevel + ConstRes.cstDoctorLevelNo;
            paramXhjs.Value = record.LoopCount;
            paramJlzt.Value = (int)record.RecordState;
            paramMemo.Value = string.Empty;

            return new SqlParameter[] { paramXh,
                paramSyxh, paramBlxh, paramGzdm, paramTjdm, paramTjsj, paramTjzt, 
                paramJgdm, paramJgsj, paramJgzt, paramWgzt, paramTsxx, paramWgxx, 
                paramCzsj, paramZgdm, paramCjzgdm, paramZrjb, paramXhjs, paramJlzt, paramMemo
            };
        }

        SqlParameter[] InitRelateRecordParams(QCRuleRecord record)
        {
            SqlParameter paramXh = new SqlParameter(param_xh, SqlDbType.Decimal);
            SqlParameter paramWgzt = new SqlParameter(param_wgzt, SqlDbType.Int);
            SqlParameter paramJgsj = new SqlParameter(param_jgsj, SqlDbType.VarChar, 19);
            paramXh.Value = record.Xh;
            paramWgzt.Value = (int)record.RuleState;
            paramJgsj.Value = record.ResultTime.ToString("yyyy-MM-dd HH:mm:ss"); //record.ResultTime.ToString("yyyy-MM-dd") + " " + record.ResultTime.ToString("T");

            return new SqlParameter[] { paramXh, paramWgzt, paramJgsj };
        }

        SqlParameter[] InitTriggerRelateRuleParams(QCRuleRecord record)
        {
            SqlParameter paramXh = new SqlParameter(param_xh, SqlDbType.Decimal);
            SqlParameter paramJlzt = new SqlParameter(param_jlzt, SqlDbType.Int);
            paramXh.Value = record.Xh;
            paramJlzt.Value = (int)RecordState.ValidWait;

            return new SqlParameter[] { paramXh, paramJlzt };
        }

        SqlParameter[] InitEffectRuleParams(QCRuleRecord record)
        {
            SqlParameter paramXh = new SqlParameter(param_xh, SqlDbType.Decimal);
            paramXh.Value = record.Xh;
            SqlParameter paramWgzt = new SqlParameter(param_wgzt, SqlDbType.Int);
            paramWgzt.Value = (int)record.RuleState;
            return new SqlParameter[] { 
                paramXh, paramWgzt
                  };
        }

        /// <summary>
        /// 取得指定处理类型的时限纪录
        /// </summary>
        /// <param name="dealtype"></param>
        /// <returns></returns>
        public DataSet GetUndoRulesDataSetByDealType(RuleDealType dealtype)
        {
            SqlParameter paramClbz = new SqlParameter(param_clbz, SqlDbType.Int);
            paramClbz.Value = (int)dealtype;
            SqlParameter paramJlzt = new SqlParameter(param_jlzt, SqlDbType.Int);
            paramJlzt.Value = (int)RecordState.Invalid; //注意这里的sql语句是Valid !=
            return _sqlHelper.ExecuteDataSet(cstSelectUndoRuleRecordByDealType,
                new SqlParameter[] { paramClbz, paramJlzt });
        }

        /// <summary>
        /// 设置规则纪录完成状态
        /// </summary>
        /// <param name="rulerecord"></param>
        public void EffectRuleRecord(QCRuleRecord rulerecord)
        {
            SqlParameter[] effectparams = InitEffectRuleParams(rulerecord);
            _sqlHelper.ExecuteNoneQuery(cstEffectRuleRecord, effectparams);
        }

        /// <summary>
        /// 取得病人所有未处理的纪录
        /// </summary>
        /// <param name="patid"></param>
        /// <returns></returns>
        public DataSet GetPatientUndoRulesDataSet(int patid)
        {
            SqlParameter paramSyxh = new SqlParameter(param_syxh, SqlDbType.Int);
            paramSyxh.Value = patid;
            SqlParameter paramJlzt = new SqlParameter(param_jlzt, SqlDbType.Int);
            paramJlzt.Value = (int)RecordState.Invalid; //注意这里的sql语句是Valid !=
            return _sqlHelper.ExecuteDataSet(cstSelectPatientRules + cstWhereUndo,
                new SqlParameter[] { paramSyxh, paramJlzt });
        }
        #endregion
    }
}
