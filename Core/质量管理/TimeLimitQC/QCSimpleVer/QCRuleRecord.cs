using System;
using System.Collections.Generic;
using System.Text;
using DrectSoft.Core.TimeLimitQC;

namespace DrectSoft.Core.TimeLimitQC
{
    /// <summary>
    /// 病人时限纪录
    /// </summary>
    public class QCRuleRecord
    {
        #region fields

        decimal _xh;
        int _patId;
        int _eprId;
        DateTime _conditionTime = DateTime.MinValue;
        CompleteType _conditionState;
        DateTime _resultTime = DateTime.MinValue;
        CompleteType _resultState;
        DateTime _createTime;
        string _createDoctor;
        string _dutyDoctor;
        RuleRecordState _ruleState;
        QCRule _rule;
        int _loopCount = 1;
        RecordState _recordState = RecordState.Valid;

        #endregion

        #region properties

        /// <summary>
        /// 纪录序号
        /// </summary>
        public decimal Xh
        {
            get { return _xh; }
            internal set { _xh = value; }
        }

        /// <summary>
        /// 病历Id
        /// </summary>
        public int EprId
        {
            get { return _eprId; }
            set { _eprId = value; }
        }

        /// <summary>
        /// 病人Id
        /// </summary>
        public int PatId
        {
            get { return _patId; }
            set { _patId = value; }
        }

        /// <summary>
        /// 条件发生时间
        /// </summary>
        public DateTime ConditionTime
        {
            get { return _conditionTime; }
            set { _conditionTime = value; }
        }

        /// <summary>
        /// 结果发生时间
        /// </summary>
        public DateTime ResultTime
        {
            get { return _resultTime; }
            set { _resultTime = value; }
        }

        /// <summary>
        /// 条件完成状态
        /// </summary>
        public CompleteType ConditionState
        {
            get { return _conditionState; }
            set { _conditionState = value; }
        }

        /// <summary>
        /// 结果完成状态
        /// </summary>
        public CompleteType ResultState
        {
            get { return _resultState; }
            set { _resultState = value; }
        }

        /// <summary>
        /// 规则纪录创建时间
        /// </summary>
        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 创建纪录的医生代码
        /// </summary>
        public string CreateDoctor
        {
            get { return _createDoctor; }
            set { _createDoctor = value; }
        }

        /// <summary>
        /// 责任医生代码
        /// </summary>
        public string DutyDoctor
        {
            get { return _dutyDoctor; }
            set { _dutyDoctor = value; }
        }

        /// <summary>
        /// 规则纪录状态
        /// </summary>
        public RuleRecordState RuleState
        {
            get { return _ruleState; }
            set { _ruleState = value; }
        }

        /// <summary>
        /// 纪录相对的规则
        /// </summary>
        public QCRule Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }

        /// <summary>
        /// 循环计数
        /// </summary>
        public int LoopCount
        {
            get { return _loopCount; }
            set { _loopCount = value; }
        }

        /// <summary>
        /// 数据记录状态
        /// </summary>
        public RecordState RecordState
        {
            get { return _recordState; }
            set { _recordState = value; }
        }
        #endregion

        #region ctor

        /// <summary>
        /// 构造新的记录
        /// </summary>
        public QCRuleRecord(int patid, int eprid, QCRule rule)
        {
            _patId = patid;
            _eprId = eprid;
            _rule = rule;
            _xh = -1;
        }

        /// <summary>
        /// 构造已有记录
        /// </summary>
        /// <param name="xh"></param>
        /// <param name="patid"></param>
        /// <param name="eprid"></param>
        /// <param name="rule"></param>
        public QCRuleRecord(decimal xh, int patid, int eprid, QCRule rule)
        {
            _patId = patid;
            _eprId = eprid;
            _rule = rule;
            _xh = xh;
        }
        #endregion
    }
}
