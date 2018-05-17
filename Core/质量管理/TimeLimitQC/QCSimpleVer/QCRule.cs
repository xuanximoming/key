using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Xml;
using DrectSoft.Core.TimeLimitQC;
using System.ComponentModel;
using System.Linq;
namespace DrectSoft.Core.TimeLimitQC
{
    #region EnumDutyLevel

    /// <summary>
    /// 责任医生级别
    /// </summary>
    public enum DutyLevel
    {
        /// <summary>
        /// 全部级别
        /// </summary>
        All = 0,
        /// <summary>
        /// 主任医生
        /// </summary>
        ZRYS = 1,
        /// <summary>
        /// 主治医生
        /// </summary>
        ZZYS = 2,
        /// <summary>
        /// 床位医生
        /// </summary>
        CWYS = 3,
        /// <summary>
        /// 副主任医生
        /// </summary>
        FZRYS = 4
    }

    #endregion

    #region EnumRelationRuleDealType

    /// <summary>
    /// 相关规则处理方式
    /// </summary>
    public enum RelateRuleDealType
    {
        /// <summary>
        /// 不处理
        /// </summary>
        [Description("不处理")]
        None = 0,

        /// <summary>
        /// 同效规则
        /// </summary>
        [Description("同效规则")]
        SyncRule = 1,

        /// <summary>
        /// 取消规则
        /// </summary>
        [Description("取消规则")]
        CancelRule = 2,

        /// <summary>
        /// 触发规则
        /// </summary>
        [Description("触发规则")]
        GenRule = 3,
    }
    #endregion

    #region Enum RuleDealType

    /// <summary>
    /// 规则处理方式
    /// </summary>
    public enum RuleDealType
    {
        /// <summary>
        /// 一次性
        /// </summary>
        [Description("一次性")]
        Once = 0,

        /// <summary>
        /// 循环生效
        /// </summary>
        [Description("循环生效")]
        Loop = 1,

        /// <summary>
        /// 触发后生效(规则条件成立时记录状态=ValidWait)
        /// </summary>
        [Description("触发后生效")]
        NeedTrigger = 2,

        /// <summary>
        /// 内部触发用(规则条件成立时记录状态=ValidNonVisible)
        /// </summary>
        [Description("内部触发用")]
        InnerForTrigger = 3,
    }
    #endregion

    #region QCRule

    /// <summary>
    /// 规则
    /// </summary>
    public class QCRule
    {
        #region fields

        QCCondition _condition;
        QCResult _result;
        TimeSpan _timelimit;
        string _tipInfo = string.Empty;
        string _warnInfo = string.Empty;
        string _id;
        string _name;
        DutyLevel _dutylevel = DutyLevel.All;
        IList<QCRule> _relateRules = new List<QCRule>();
        RelateRuleDealType _relateDealType = RelateRuleDealType.None;
        RuleDealType _dealType = RuleDealType.Once;
        int _looptimes = 0;
        TimeSpan _looptimeinterval;
        bool _isNew;
        bool _invalid;
        static IList<QCRule> _allRules = new List<QCRule>();
        static QCRuleDal dal = new QCRuleDal();
        QCRuleGroup _group;
        string _preRelateIds = string.Empty;

        #endregion

        #region properties

        /// <summary>
        /// 提示信息
        /// </summary>
        [Browsable(false)]
        public string TipInfo
        {
            get { return _tipInfo; }
            set { _tipInfo = value; }
        }

        /// <summary>
        /// 违规信息
        /// </summary>
        [Browsable(false)]
        public string WarnInfo
        {
            get { return _warnInfo; }
            set { _warnInfo = value; }
        }

        /// <summary>
        /// 规则代码
        /// </summary>
        [DisplayName("代码")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 规则描述
        /// </summary>
        [DisplayName("描述")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///  时限
        /// </summary>
        [Browsable(false)]
        public TimeSpan Timelimit
        {
            get { return _timelimit; }
            set { _timelimit = value; }
        }

        /// <summary>
        /// 规则条件
        /// </summary>
        [Browsable(false)]
        public QCCondition Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        /// <summary>
        /// 规则结果
        /// </summary>
        [Browsable(false)]
        public QCResult Result
        {
            get { return _result; }
            set { _result = value; }
        }

        /// <summary>
        /// 医生级别
        /// </summary>
        [Browsable(false)]
        public DutyLevel Dutylevel
        {
            get { return _dutylevel; }
            set { _dutylevel = value; }
        }

        /// <summary>
        /// 相关规则
        /// </summary>
        [Browsable(false)]
        public ReadOnlyCollection<QCRule> RelateRules
        {
            get
            {
                return new ReadOnlyCollection<QCRule>(_relateRules);
            }
        }

        /// <summary>
        /// 相关规则Id
        /// </summary>
        [Browsable(false)]
        public string RelateRuleIds
        {
            get
            {
                string idlist = string.Empty;
                if (_relateRules != null)
                    for (int i = 0; i < _relateRules.Count; i++)
                        idlist = idlist + _relateRules[i].Id + ",";
                return idlist;
            }
        }

        /// <summary>
        /// 预先保存相关规则Id,加载所有规则后再次加载相关规则
        /// </summary>
        internal string PreRelateRuleIds
        {
            get { return _preRelateIds; }
            set { _preRelateIds = value; }
        }

        /// <summary>
        /// 相关规则处理方式
        /// </summary>
        [Browsable(false)]
        public RelateRuleDealType RelateDealType
        {
            get { return _relateDealType; }
            set { _relateDealType = value; }
        }

        /// <summary>
        /// 规则处理方式
        /// </summary>
        [Browsable(false)]
        public RuleDealType DealType
        {
            get { return _dealType; }
            set { _dealType = value; }
        }

        /// <summary>
        /// 规则循环处理时的次数
        /// </summary>
        [Browsable(false)]
        public int LoopTimes
        {
            get { return _looptimes; }
            set { _looptimes = value; }
        }

        /// <summary>
        /// 循环规则时间间隔
        /// </summary>
        [Browsable(false)]
        public TimeSpan LoopTimeInterVal
        {
            get { return _looptimeinterval; }
            set { _looptimeinterval = value; }
        }

        /// <summary>
        /// 新规则
        /// </summary>
        [Browsable(false)]
        public bool IsNew
        {
            get { return _isNew; }
        }

        /// <summary>
        /// 无效
        /// </summary>
        [Browsable(false)]
        public bool Invalid
        {
            get { return _invalid; }
            set { _invalid = value; }
        }

        /// <summary>
        /// 规则分类
        /// </summary>
        [DisplayName("分类")]
        public QCRuleGroup Group
        {
            get { return _group; }
            set { _group = value; }
        }

        #endregion

        #region ctor

        /// <summary>
        /// 构造一个空的新规则
        /// </summary>
        public QCRule()
            : this(string.Empty, string.Empty, DutyLevel.All)
        {
            _isNew = true;
        }

        /// <summary>
        /// 构造
        /// </summary>
        public QCRule(string id, string name, DutyLevel dutylevel)
        {
            _id = id;
            _name = name;
            _dutylevel = dutylevel;
        }
        #endregion

        /// <summary>
        /// 得到所有的规则
        /// </summary>
        /// <param name="conditions">时限条件集合</param>
        /// <param name="results">时限结果集合</param>
        /// <returns></returns>
        public static IList<QCRule> GetAllRules(IList<QCCondition> conditions, IList<QCResult> results)
        {
            //if (_allRules == null || _allRules.Count == 0)
            {
                _allRules = dal.GetRulesList(conditions, results);
            }
            return _allRules;
        }

        /// <summary>
        /// 得到指定代码的时限规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static QCRule SelectQCRule(string ruleId)
        {
            foreach (QCRule qcr in _allRules)
            {
                if (qcr._id == ruleId) return qcr;
            }
            return null;
        }

        /// <summary>
        /// 取得指定条件的时限规则
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IList<QCRule> GetRulesByCondition(QCCondition condition)
        {
            IList<QCRule> ret = new List<QCRule>();
            foreach (QCRule qcr in _allRules)
            {
                if (qcr._condition.Id == condition.Id)
                    ret.Add(qcr.Clone());
            }
            return ret;
        }

        /// <summary>
        /// 取得指定结果的时限规则
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static List<QCRule> GetRulesByResult(QCResult result)
        {
            List<QCRule> ret = new List<QCRule>();


            foreach (QCRule qcr in _allRules)
            {
                if (qcr.Result == null) continue;
                if (qcr.Result.Id == result.Id)
                    ret.Add(qcr.Clone());
            }
            return ret;
        }

        /// <summary>
        /// 字符型 -> TimeSpan型
        /// </summary>
        /// <param name="timelimit"></param>
        /// <returns></returns>
        public static TimeSpan LimitString2TimeSpan(string timelimit)
        {
            if (string.IsNullOrEmpty(timelimit)) return TimeSpan.Zero;

            int days = 0, hours = 0, minutes = 0, seconds = 0;

            int i;

            string temp = timelimit;

            i = timelimit.IndexOf("d");
            if (i >= 0)
            {
                if (i > 0) days = int.Parse(temp.Substring(0, i));
                temp = temp.Substring(i + 1, temp.Length - i - 1);
            }
            i = temp.IndexOf("h");
            if (i >= 0)
            {
                if (i > 0) hours = int.Parse(temp.Substring(0, i));
                temp = temp.Substring(i + 1, temp.Length - i - 1);
            }
            i = temp.IndexOf("n");
            if (i >= 0)
            {
                if (i > 0) minutes = int.Parse(temp.Substring(0, i));
                temp = temp.Substring(i + 1, temp.Length - i - 1);
            }
            i = temp.IndexOf("s");
            if (i >= 0)
            {
                if (i > 0) seconds = int.Parse(temp.Substring(0, i));
                temp = temp.Substring(i + 1, temp.Length - i - 1);
            }

            return new TimeSpan(days, hours, minutes, seconds);
        }

        /// <summary>
        /// TimeSpan型 -> 字符型
        /// </summary>
        /// <param name="timelimit"></param>
        /// <returns></returns>
        public static string TimeSpan2LimitString(TimeSpan timelimit)
        {
            int days = 0, hours = 0, minutes = 0, seconds = 0;

            days = timelimit.Days;
            hours = timelimit.Hours;
            minutes = timelimit.Minutes;
            seconds = timelimit.Seconds;

            string ts = string.Empty;
            if (days != 0)
            {
                ts += days.ToString() + "d";
            }
            else
                ts += "d";
            if (hours != 0)
            {
                ts += hours.ToString() + "h";
            }
            else
                ts += "h";
            if (minutes != 0)
            {
                ts += minutes.ToString() + "n";
            }
            else
                ts += "n";
            if (seconds != 0)
            {
                ts += seconds.ToString() + "s";
            }
            else
                ts += "s";

            return ts;
        }

        /// <summary>
        /// 判断条件Id是否已经存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IdIsExisted(string id)
        {
            if (_allRules == null) return false;

            foreach (QCRule qcr in _allRules)
            {
                if (qcr.Id == id) return true;
            }
            return false;
        }

        /// <summary>
        /// 保存时限规则条件
        /// </summary>
        /// <param name="rule"></param>
        public static void SaveQCRule(QCRule rule)
        {
            dal.SaveRule(rule);
        }

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="rule"></param>
        public static void DeleteQCRule(QCRule rule)
        {
            dal.DeleteQCRule(rule);
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public QCRule Clone()
        {
            QCRule qcrc = new QCRule(_id, _name, _dutylevel);
            qcrc.Timelimit = _timelimit;
            qcrc.TipInfo = _tipInfo;
            qcrc.WarnInfo = _warnInfo;
            qcrc.RelateDealType = _relateDealType;
            qcrc.DealType = _dealType;
            qcrc.LoopTimes = _looptimes;
            qcrc.LoopTimeInterVal = _looptimeinterval;
            qcrc.Invalid = _invalid;
            if (_condition != null) qcrc.Condition = _condition.Clone();
            if (_result != null) qcrc.Result = _result.Clone();
            if (_group != null) qcrc.Group = _group.Clone();
            return qcrc;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this._name;
        }

        #region relate rule operator

        /// <summary>
        /// 清空相关规则集合
        /// </summary>
        public void ClearRelateRules()
        {
            _relateRules.Clear();
        }

        /// <summary>
        /// 增加相关规则
        /// </summary>
        /// <returns></returns>
        public bool AddRelateRule(QCRule rule)
        {
            if (!CheckRelateNoLoop(rule)) return false;
            if (!CheckRelateNoContain(rule)) return false;
            this._relateRules.Add(rule.Clone());
            return true;
        }

        bool CheckRelateNoLoop(QCRule rule)
        {
            if (rule.Id == this.Id) return false;
            if (rule.RelateRules != null)
            {
                foreach (QCRule subrule in rule.RelateRules)
                {
                    if (!CheckRelateNoLoop(subrule)) return false;
                }
            }
            return true;
        }

        bool CheckRelateNoContain(QCRule rule)
        {
            foreach (QCRule subrule in _relateRules)
            {
                if (subrule.Id == rule.Id) return false;
            }
            return true;
        }
        #endregion
    }

    #endregion

}
