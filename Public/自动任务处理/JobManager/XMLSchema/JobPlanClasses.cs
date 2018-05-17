using System;
using System.Xml.Serialization;

namespace DrectSoft.JobManager
{
    /// <summary>
    /// 任务的执行计划
    /// </summary>
    [SerializableAttribute()]
    public class JobPlan
    {
        #region properties
        /// <summary>
        /// 任务类型（重复执行或只执行一次）
        /// </summary>
        public JobPlanType JobType
        {
            get { return _jobType; }
            set { _jobType = value; }
        }
        private JobPlanType _jobType;

        /// <summary>
        /// 执行一次的Datetime（任务类型为JustOnce时有效）
        /// </summary>
        [XmlIgnore()]
        public DateTime DateTimeOfExecOnce
        {
            get { return _dateTimeOfExecOnce; }
            set { _dateTimeOfExecOnce = value; }
        }
        private DateTime _dateTimeOfExecOnce;

        /// <summary>
        /// 格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        [XmlElementAttribute("DateTimeOfExecOnce")]
        public string XmlDateTimeOfExecOnce
        {
            get { return _dateTimeOfExecOnce.ToString("yyyy-MM-dd HH:mm:ss"); }
            set { _dateTimeOfExecOnce = Convert.ToDateTime(value); }
        }

        /// <summary>
        /// 任务执行频率
        /// </summary>
        public JobPlanFrequency Frequency
        {
            get { return _frequency; }
            set { _frequency = value; }
        }
        private JobPlanFrequency _frequency;

        /// <summary>
        /// 每天执行的频率设置
        /// </summary>
        public JobPlanFrequencyPerDay FrequencyPerDay
        {
            get { return _frequencyPerDay; }
            set { _frequencyPerDay = value; }
        }
        private JobPlanFrequencyPerDay _frequencyPerDay;

        /// <summary>
        /// 任务启用的时间范围
        /// </summary>
        public JobPlanDuration Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        private JobPlanDuration _duration;

        /// <summary>
        /// 需要立即执行
        /// </summary>
        [XmlIgnore()]
        public bool NeedRunNow
        {
            get { return CheckNeedRunNow(); }
        }

        /// <summary>
        /// 任务上次执行的时间（在动作执行时赋值，默认为最小值）
        /// </summary>
        [XmlIgnore()]
        public DateTime LastExecuteTime
        {
            get { return _lastExecuteTime; }
            set { _lastExecuteTime = value; }
        }
        private DateTime _lastExecuteTime = DateTime.MinValue;
        #endregion

        #region private methods
        private bool CheckNeedRunNow()
        {
            switch (JobType)
            {
                case JobPlanType.JustOnce:
                    return JobPlanFrequencyPerDay.CheckJustOnce(DateTimeOfExecOnce, DateTime.Now, LastExecuteTime);
                case JobPlanType.Repeat:
                    return CheckRepeat();
                default:
                    return false;
            }
        }

        /// <summary>
        /// 检查重复执行的情况
        /// </summary>
        /// <returns></returns>
        private bool CheckRepeat()
        {
            // 首先检查当前日期是否在计划的有效期内
            if (!Duration.CheckTodayIsInDuration())
                return false;
            // 其次检查当天是否需要执行
            if (!Frequency.CheckTodayIsInPlan(Duration.StartDate))
                return false;
            // 最后检查是否到了当天的执行时间点
            return FrequencyPerDay.CheckNowIsInPlan(LastExecuteTime);
        }
        #endregion
    }

    /// <summary>
    /// 任务执行频率
    /// </summary>
    [SerializableAttribute()]
    [XmlTypeAttribute(AnonymousType = true)]
    public class JobPlanFrequency
    {
        #region properties
        /// <summary>
        /// 执行间隔，单位由执行周期类型决定
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }
        private int _interval;

        /// <summary>
        /// 执行周期的单位，3：天；4：周
        /// </summary>
        public JobExecIntervalUnit IntervalUnit
        {
            get { return _intervalUnit; }
            set { _intervalUnit = value; }
        }
        private JobExecIntervalUnit _intervalUnit;

        /// <summary>
        /// 周中的天数组合，“0/1”组成的七位字符串，由星期日到星期六；1表示启用，0表示不启用；执行周期为周时才有效
        /// </summary>
        public string WeekDays
        {
            get { return _weekDays; }
            set { _weekDays = value; }
        }
        private string _weekDays;
        #endregion

        #region public methods
        /// <summary>
        /// 检查当天是否在执行计划内
        /// </summary>
        /// <param name="planStartDate">计划开始日期</param>
        /// <returns></returns>
        public bool CheckTodayIsInPlan(DateTime planStartDate)
        {
            if (DateTime.Now.Date < planStartDate.Date)
                return false;
            //

            if (IntervalUnit == JobExecIntervalUnit.Day)
                return CheckTodayIsInCirclePlan(planStartDate);
            else if (IntervalUnit == JobExecIntervalUnit.Week)
                return CheckTodayIsInWeekDays(planStartDate);
            else
                return false;
        }
        #endregion

        #region private methods
        private bool CheckTodayIsInCirclePlan(DateTime planStartDate)
        {
            TimeSpan diff = DateTime.Now.Date - planStartDate;
            return ((diff.TotalDays % Interval) == 0);
        }

        private bool CheckTodayIsInWeekDays(DateTime planStartDate)
        {
            if ((String.IsNullOrEmpty(WeekDays)) || (WeekDays.Length != 7))
                return false;

            TimeSpan diff = DateTime.Now.Date - planStartDate;
            if (((diff.TotalDays / 7) % Interval) == 0)
            {
                int weekDay = (int)DateTime.Now.DayOfWeek;
                return (WeekDays[weekDay - 1] == '1');
            }
            else
                return false;
        }
        #endregion
    }

    /// <summary>
    /// 每天执行时的频率设置
    /// </summary>
    [SerializableAttribute()]
    [XmlTypeAttribute(AnonymousType = true)]
    public class JobPlanFrequencyPerDay
    {
        /// <summary>
        /// 时间冗余量（秒），在判断是否到了预定执行时间时需要增加冗余量
        /// </summary>
        private const int s_RedundancyTime = 600;

        #region properties
        /// <summary>
        /// 重复执行，为否时每天只执行一次
        /// </summary>
        public bool Repeatly
        {
            get { return _repeatly; }
            set { _repeatly = value; }
        }
        private bool _repeatly;

        /// <summary>
        /// 执行一次的时间点
        /// </summary>
        [XmlIgnore()]
        public TimeSpan TimeOfExecOnce
        {
            get { return _timeOfExecOnce; }
            set { _timeOfExecOnce = value; }
        }
        private TimeSpan _timeOfExecOnce;

        /// <summary>
        /// 格式:HH:mm:ss
        /// </summary>
        [XmlElementAttribute("TimeOfExecOnce")]
        public string XmlTimeOfExecOnce
        {
            get { return ((DateTime)(DateTime.Now.Date + _timeOfExecOnce)).ToString("HH:mm:ss"); }
            set { _timeOfExecOnce = Convert.ToDateTime(value).TimeOfDay; }
        }

        /// <summary>
        /// 重复执行的时间间隔
        /// </summary>
        //[CLSCompliant(false)]
        public uint Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }
        private uint _interval;

        /// <summary>
        /// 重复执行的时间间隔单位
        /// </summary>
        public JobExecIntervalUnit IntervalUnit
        {
            get { return _intervalUnit; }
            set { _intervalUnit = value; }
        }
        private JobExecIntervalUnit _intervalUnit;

        /// <summary>
        /// 以秒计的时间间隔
        /// </summary>
        [XmlIgnore()]
        //[CLSCompliant(false)]
        public uint IntervalInSeconds
        {
            get
            {
                if (IntervalUnit == JobExecIntervalUnit.Minute)
                    return Interval * 60;
                else if (IntervalUnit == JobExecIntervalUnit.Hour)
                    return Interval * 3600;
                else
                    return 0;
            }
        }

        /// <summary>
        /// 重复执行开始的时间,
        /// </summary>
        [XmlIgnore()]
        public TimeSpan StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        private TimeSpan _startTime;

        /// <remark/>
        [XmlElementAttribute("StarTime")]
        public string XmlStarTime
        {
            get { return ((DateTime)(DateTime.Now.Date + _startTime)).ToString("HH:mm:ss"); }
            set { _startTime = Convert.ToDateTime(value).TimeOfDay; }
        }

        /// <summary>
        /// 重复执行结束的时间,格式:HH:mm:ss
        /// </summary>
        [XmlIgnore()]
        public TimeSpan EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
        private TimeSpan _endTime;

        /// <summary>
        /// 格式:HH:mm:ss
        /// </summary>
        [XmlElementAttribute("EndTime")]
        public string XmlEndTime
        {
            get
            {
                return ((DateTime)(DateTime.Now.Date + _endTime)).ToString("HH:mm:ss");
            }
            set { _endTime = Convert.ToDateTime(value).TimeOfDay; }
        }
        #endregion

        #region public methods
        public bool CheckNowIsInPlan(DateTime lastExecuteTime)
        {
            if (Repeatly)
                return CheckTimeIsInCirclePlan(lastExecuteTime);
            else
                return CheckJustOnce(DateTime.Now.Date + TimeOfExecOnce, DateTime.Now, lastExecuteTime);
        }

        /// <summary>
        /// 检查只执行一次的情况
        /// </summary>
        /// <param name="plannedTime">预定的计划执行时间点</param>
        /// <param name="executeTime">要检查的执行时间点</param>
        /// <param name="lastExecuteTime">上次执行时间</param>
        /// <returns></returns>
        public static bool CheckJustOnce(DateTime plannedTime, DateTime executeTime, DateTime lastExecuteTime)
        {
            if (executeTime < plannedTime) // 还没有到预定的执行时间点
                return false;
            else if (executeTime == plannedTime) // 刚好是预定的执行时间点
                return true;
            else
            {
                TimeSpan diff = executeTime - plannedTime;
                // 程序执行时有可能因为运算速度问题，没有在指定的那一秒进来运算，
                // 所以还应加一些冗余量，以便错过执行点后还能再补做
                //    如果上次执行时间在计划时间之前，说明任务没有按计划执行，需要补做
                if ((diff.TotalSeconds < s_RedundancyTime) && (lastExecuteTime < plannedTime))
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region private methods
        private bool CheckTimeIsInCirclePlan(DateTime lastExecuteTime)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;

            // 首先检查当前时间点是否在运行时间范围内
            if ((now.TotalSeconds < StartTime.TotalSeconds) || (now.TotalSeconds > EndTime.TotalSeconds))
                return false;

            TimeSpan diff;
            // 检查是否是预定的执行时间点
            diff = now - StartTime;
            if ((diff.TotalSeconds % IntervalInSeconds) == 0)
                return true;
            else if (DateTime.Today > lastExecuteTime.Date) // 上次执行时间在今天之前
                return true;
            else
            {
                // 如果上次执行时间与当前时间差超过时间间隔，则允许执行
                diff = now - lastExecuteTime.TimeOfDay;
                if (diff.TotalSeconds >= IntervalInSeconds)
                    return true;
                else
                    return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// 持续时间（任务启用的时间范围）
    /// </summary>
    [SerializableAttribute()]
    [XmlTypeAttribute(AnonymousType = true)]
    public class JobPlanDuration
    {
        #region properties
        /// <summary>
        /// 任务开始执行的日期
        /// </summary>
        [XmlIgnore()]
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        private DateTime _startDate;

        /// <summary>
        /// 格式:yyyy-MM-dd
        /// </summary>
        [XmlElementAttribute("StartDate")]
        public string XmlStartDate
        {
            get { return _startDate.ToString("yyyy-MM-dd"); }
            set { _startDate = Convert.ToDateTime(value); }
        }

        /// <summary>
        /// 任务停止执行的日期
        /// </summary>
        [XmlIgnore()]
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
        private DateTime _endDate;

        /// <summary>
        /// 格式:yyyy-MM-dd
        /// </summary>
        [XmlElementAttribute("EndDate")]
        public string XmlEndDate
        {
            get { return _endDate.ToString("yyyy-MM-dd"); }
            set { _endDate = Convert.ToDateTime(value); }
        }

        /// <summary>
        /// 是否有结束日期
        /// </summary>
        public bool HasEndDate
        {
            get { return _hasEndDate; }
            set { _hasEndDate = value; }
        }
        private bool _hasEndDate;
        #endregion

        #region public methods
        /// <summary>
        /// 检查当天是否在日期范围设置内(仅比较日期部分)
        /// </summary>
        /// <returns></returns>
        public bool CheckTodayIsInDuration()
        {
            DateTime date = DateTime.Now;
            if (date.Date < StartDate.Date)
                return false;
            if (HasEndDate && (date.Date > EndDate.Date))
                return false;
            return true;
        }
        #endregion
    }
}