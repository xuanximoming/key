using System;
using System.Data;


namespace DrectSoft.Common.Eop
{
    /// <summary>
    /// 病人个人基本信息
    /// </summary>
    public class PatientBasicInfo : EPBaseObject
    {
        #region properties
        private decimal _NoOfFirstPage;
        /// <summary>
        /// 母亲首页序号
        /// </summary>
        public decimal NoOfFirstPage
        {
            get { return _NoOfFirstPage; }
            set { _NoOfFirstPage = value; }
        }

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string PatientName
        {
            get { return _patientName; }
            set
            {
                _patientName = value;
                // 需要重新生成拼音、五笔代码◎◎◎
            }
        }
        private string _patientName;

        /// <summary>
        /// 病人性别
        /// </summary>
        public CommonBaseCode Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        private CommonBaseCode _sex;

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday
        {
            get
            {
                return _birthday;
            }
            set { _birthday = value; }
        }
        internal string InternalBirthday
        {
            get { return _birthday.ToString("yyyy-MM-dd"); }
            set { _birthday = Convert.ToDateTime(value); }
        }
        private DateTime _birthday;

        private DateTime _inHosDate = DateTime.Now;
        public DateTime InHosDate
        {
            get
            {
                return _inHosDate;
            }
            set { _inHosDate = value; }
        }

        /// <summary>
        /// 病人当前年龄,单位：年
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public int CurrentAge
        {
            get
            {
                TimeSpan diff = (DateTime.Now.Date - Birthday);
                return diff.Days / 365;
            }
        }

        /// <summary>
        /// 病人当前年龄（显示用）
        /// </summary>
        [System.Xml.Serialization.XmlIgnore()]
        public string CurrentDisplayAge
        {
            get { return CalcDisplayAge(Birthday, _inHosDate); }
        }

        /// <summary>
        /// 显示年龄(入院时年龄，根据实际情况显示的年龄,如XXX年,XX月XX天,XX天)
        /// </summary>
        public string DisplayAge
        {
            get { return _displayAge; }
            internal set { _displayAge = value; }
        }
        private string _displayAge;

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentificationNo
        {
            get { return _identificationNo; }
            set { _identificationNo = value; }
        }
        private string _identificationNo;

        /// <summary>
        /// 社保卡号
        /// </summary>
        public string SocialInsuranceNo
        {
            get { return _socialInsuranceNo; }
            set { _socialInsuranceNo = value; }
        }
        private string _socialInsuranceNo;

        /// <summary>
        /// 保险卡号
        /// </summary>
        public string InsuranceNo
        {
            get { return _insuranceNo; }
            set { _insuranceNo = value; }
        }
        private string _insuranceNo;

        /// <summary>
        /// 其它卡号
        /// </summary>
        public string OtherCardNo
        {
            get { return _otherCardNo; }
            set { _otherCardNo = value; }
        }
        private string _otherCardNo;


        private int _babySequence;
        /// <summary>
        /// 婴儿序号(从1开始，0表示不是婴儿)
        /// </summary>
        public int BabySequence
        {
            get { return _babySequence; }
            set { _babySequence = value; }
        }

        private decimal _motherFirstPageNo;
        /// <summary>
        /// 母亲首页序号
        /// </summary>
        public decimal MotherFirstPageNo
        {
            get { return _motherFirstPageNo; }
            set { _motherFirstPageNo = value; }
        }

        /// <summary>
        /// 文化程度 
        /// </summary>
        public CommonBaseCode EducationLevel
        {
            get { return _educationLevel; }
            set { _educationLevel = value; }
        }
        private CommonBaseCode _educationLevel;

        /// <summary>
        /// 婚姻状况 
        /// </summary>
        public CommonBaseCode MarriageCondition
        {
            get { return _marriageCondition; }
            set { _marriageCondition = value; }
        }
        private CommonBaseCode _marriageCondition;

        /// <summary>
        /// 民族代码 
        /// </summary>
        public CommonBaseCode Nation
        {
            get { return _nation; }
            set { _nation = value; }
        }
        private CommonBaseCode _nation;

        /// <summary>
        /// 出生地
        /// </summary>
        public Address Homeplace
        {
            get { return _homeplace; }
            set { _homeplace = value; }
        }
        private Address _homeplace;

        /// <summary>
        /// 工作单位
        /// </summary>
        public WorkDepartment DepartmentOfWork
        {
            get { return _departmentOfWork; }
            set { _departmentOfWork = value; }
        }
        private WorkDepartment _departmentOfWork;

        /// <summary>
        /// 户口有关信息
        /// </summary>
        public Address DomiciliaryInfo
        {
            get { return _domiciliaryInfo; }
            set { _domiciliaryInfo = value; }
        }
        private Address _domiciliaryInfo;

        /// <summary>
        /// 籍贯
        /// </summary>
        public Address NativePlace
        {
            get { return _nativePlace; }
            set { _nativePlace = value; }
        }
        private Address _nativePlace;

        /// <summary>
        /// 联系信息
        /// </summary>
        public LinkMan LinkManInfo
        {
            get { return _linkManInfo; }
            set { _linkManInfo = value; }
        }
        private LinkMan _linkManInfo;

        /// <summary>
        /// 受教育年限
        /// 以年为单位
        /// </summary>
        public decimal YearsOfEducation
        {
            get { return _yearOfEducation; }
            set { _yearOfEducation = value; }
        }
        private decimal _yearOfEducation;

        /// <summary>
        /// 宗教信仰
        /// </summary>
        public string Faith
        {
            get { return _faith; }
            set { _faith = value; }
        }
        private string _faith;

        /// <summary>
        /// 目前住址
        /// </summary>
        public string CurrentAddress
        {
            get { return _currentAddress; }
            set { _currentAddress = value; }
        }
        private string _currentAddress;

        #endregion

        /// <summary>
        /// 与实体类匹配的、读取DB中数据的SQL语句
        /// </summary>
        public override string InitializeSentence
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 在DataTable中按主键值过滤记录的条件
        /// </summary>
        public override string FilterCondition
        {
            get { throw new NotImplementedException(); }
        }

        #region ctors
        /// <summary>
        /// 
        /// </summary>
        public PatientBasicInfo()
            : base()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceRow"></param>
        public PatientBasicInfo(DataRow sourceRow)
            : base(sourceRow)
        { }

        #endregion

        /// <summary>
        /// 初始化所有的属性，包括引用类型的属性自己的属性
        /// </summary>
        public override void ReInitializeAllProperties()
        {
            if (Sex != null)
                Sex.ReInitializeAllProperties();
            if (EducationLevel != null)
                EducationLevel.ReInitializeAllProperties();
            if (MarriageCondition != null)
                MarriageCondition.ReInitializeAllProperties();
            if (Nation != null)
                Nation.ReInitializeAllProperties();
            if (Homeplace != null)
                Homeplace.ReInitializeAllProperties();
            if (DepartmentOfWork != null)
                DepartmentOfWork.ReInitializeAllProperties();
            if (DomiciliaryInfo != null)
                DomiciliaryInfo.ReInitializeAllProperties();
            if (NativePlace != null)
                NativePlace.ReInitializeAllProperties();
            if (LinkManInfo != null)
                LinkManInfo.ReInitializeAllProperties();
        }

        /// <summary>
        /// 刷新病人年龄数据
        /// </summary>
        /// <param name="inWardTime">入区时间</param>
        public void RefreshAge(DateTime inWardTime)
        {
            DisplayAge = CalcDisplayAge(Birthday, inWardTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="birthday"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static string CalcDisplayAge(DateTime birthday, DateTime endDate)
        {
            string displayAge;
            int accAge;
            CalculateAge(birthday, endDate, out displayAge, out accAge);

            return displayAge;
        }

        /// <summary>
        /// TODO按照病案年龄处理逻辑计算病人截止到指定日期的年龄（显示用）//edit by wyt 2012-11-1
        /// </summary>
        /// <param name="birthday">病人出生日期</param>
        /// <param name="endDate">截止日期</param>
        /// <param name="displayAge">显示年龄</param>
        /// <param name="accurateAge">精确的年龄值</param>
        public static void CalculateAge(DateTime birthday, DateTime endDate, out string displayAge, out int accurateAge)
        {
            #region 屏蔽前台的运算逻辑，改成调用存储过程
            accurateAge = 0;
            displayAge = string.Empty;
            if (birthday > endDate)
                return;

            // 精确的年龄 = 年数 + 月数 + 天数 构成

            TimeSpan ts = endDate - birthday;
            //long P_BirthDay = DateAndTime.DateDiff(DateInterval.Year,
            //dateTimePicker1.Value, DateTime.Now,
            //FirstDayOfWeek.Sunday, FirstWeekOfYear.Jan1); //计算年龄
            //MessageBox.Show(P_BirthDay.ToString());


            //TimeSpan dateDiff;
            int yearPart;
            int monthPart;
            int dayPart;
            int hourPart;
            int minPart;
            //int decMonth = 0;
            //int decYear = 0;

            // 比较两个日期天数部分（该月的第几天）。看是否足月。
            dayPart = endDate.Day - birthday.Day;
            if (dayPart < 0)
            {
                endDate = endDate.AddMonths(-1);
                dayPart += DateTime.DaysInMonth(endDate.Year, endDate.Month);
                //decMonth = 1; // 不足月，月数要扣除1
            }
            // 计算月数
            monthPart = endDate.Month - birthday.Month;
            if (monthPart < 0)
            {
                monthPart += 12;
                endDate = endDate.AddYears(-1);
            }
            // 计算年数
            yearPart = endDate.Year - birthday.Year;
            // 格式化年龄输出
            if (yearPart >= 14)//14岁及以上可以输出岁数
            {
                displayAge = yearPart.ToString() + "岁";
            }
            else if (yearPart >= 1)//14岁以下
            {
                if (monthPart >= 1) // 14岁以下1个月以上可以输出月数
                {
                    displayAge = yearPart.ToString() + "岁"/* + monthPart.ToString() + "个月"*/;
                }
                else
                {
                    displayAge = yearPart.ToString() + "岁";
                }
            }
            else if (monthPart > 0)  // 1个月以上
            {
                displayAge = monthPart.ToString() + "个月";
            }
            else if (dayPart > 0)  // 1个月以下可以输出天数
            {
                displayAge = dayPart.ToString() + "天";
            }
            else if (dayPart == 0)  // update by Ukey 2016-11-13 17:24 1天以下输出小时和分钟数                         
            {
                hourPart = endDate.Hour - birthday.Hour;
                minPart = endDate.Minute - birthday.Minute;
                if (hourPart > 0)
                {
                    if (minPart < 0 && hourPart > 1)
                    {
                        displayAge = (hourPart - 1).ToString() + "小时" + (minPart + 60).ToString() + "分";
                    }
                    else if (minPart < 0 && hourPart == 1)
                    {
                        displayAge = (minPart + 60).ToString() + "分";
                    }
                    else if (minPart == 0)
                    {
                        displayAge = hourPart.ToString() + "小时";
                    }
                    else
                    {
                        displayAge = hourPart.ToString() + "小时" + minPart.ToString() + "分";
                    }
                }
                else
                {
                    displayAge = minPart.ToString() + "分";
                }
            }
            #endregion
            #region 已注释
            //accurateAge = 0;
            //displayAge = string.Empty;
            //if (birthday > endDate)
            //    return;

            //// 精确的年龄 = 年数 + 月数 + 天数 构成

            //TimeSpan dateDiff;
            //int yearPart;
            //int monthPart;
            //int dayPart;
            //int decMonth = 0;
            //int decYear = 0;

            //// 比较两个日期天数部分（该月的第几天）。看是否足月。
            //if (endDate.Day >= birthday.Day)
            //    dayPart = endDate.Day - birthday.Day;
            //else
            //{
            //    decMonth = 1; // 不足月，月数要扣除1
            //    dateDiff = (birthday.AddMonths(1) - birthday);
            //    dayPart = endDate.Day + dateDiff.Days - birthday.Day - 1;
            //}

            //// 比较两个日期的月份部分。看是否足年。
            //if ((endDate.Month - decMonth) >= birthday.Month)
            //    monthPart = endDate.Month - birthday.Month - decMonth;
            //else
            //{
            //    decYear = 1; // 不足年，年数要扣除1
            //    monthPart = endDate.Month + 12 - birthday.Month - decMonth;
            //}

            //// 年数
            //yearPart = endDate.Year - birthday.Year - decYear;
            //if (yearPart >= 14) // 14岁及以上的情况
            //{
            //    // 对2岁上的若不是周岁，则加一，以符合日常习惯
            //    if ((monthPart == 0) && (dayPart == 0))
            //    {
            //        displayAge = String.Format("{0}岁", yearPart);
            //        accurateAge = yearPart;
            //    }
            //    else
            //    {
            //        //accurateAge = yearPart + 1;
            //        accurateAge = yearPart;//保存实际年龄（泗县修改 2012年5月9日12:16:37 ywk）
            //        displayAge = String.Format("{0}岁", accurateAge);

            //    }
            //}
            //else if (yearPart >= 1) // 2岁及以上的情况
            //{
            //    // 对2岁上的若不是周岁，则加一，以符合日常习惯
            //    if (monthPart == 0)
            //    {
            //        displayAge = String.Format("{0}岁", yearPart);
            //        accurateAge = yearPart;
            //    }
            //    else
            //    {
            //        displayAge = String.Format("{0}岁{1}个月", yearPart, monthPart);
            //        //accurateAge = yearPart + 1;
            //        accurateAge = yearPart ;//保存实际年龄（泗县修改 2012年5月9日12:16:37 ywk

            //    }
            //}
            //else if (monthPart >= 1)
            //{
            //    if (dayPart == 0)
            //    {
            //        accurateAge = monthPart;
            //        displayAge = String.Format("{0}个月", monthPart);
            //    }
            //    else
            //    {
            //        accurateAge = monthPart;
            //        displayAge = String.Format("{0}个月{1}天", monthPart, dayPart);
            //    }
            //}
            //else 
            //{
            //    if (dayPart > 0)
            //    {
            //        displayAge = String.Format("{0}天", dayPart);
            //    }
            //    else if (dayPart == 0)
            //    {
            //        displayAge = "1天";
            //    }
            //}

            //    if (PersistentObjectFactory.SqlExecutor != null)
            //    {
            //         proc usp_Emr_CalcAge
            //             @csrq	utDatetime
            //            ,@dqrq	utDatetime
            //            ,@sjnl	int = null output
            //            ,@xsnl	utMc16 = null output
            //        SqlParameter[] paras = new SqlParameter[] { 
            //           new SqlParameter("csrq", SqlDbType.VarChar, 19, ParameterDirection.Input, false
            //              , 0, 0, null, DataRowVersion.Default, birthday.ToString("yyyy-MM-dd HH:mm:ss"))
            //           , new SqlParameter("dqrq", SqlDbType.VarChar, 19, ParameterDirection.Input, false
            //              , 0, 0, null, DataRowVersion.Default, endDate.ToString("yyyy-MM-dd HH:mm:ss"))
            //           , new SqlParameter("sjnl", SqlDbType.Int, 4, ParameterDirection.Output, true
            //              , 10, 0, null, DataRowVersion.Default, null)
            //           , new SqlParameter("xsnl", SqlDbType.VarChar, 16, ParameterDirection.Output, true
            //              , 0, 0, null, DataRowVersion.Default, null)
            //        };
            //        if ((paras[2].Value != null) && (paras[3].Value != null))
            //        {
            //           accurateAge = Convert.ToInt32(paras[2].Value);
            //           displayAge = paras[3].Value.ToString();
            //           return;
            //        }

            //        string cmd = "declare @sjnl	int , @xsnl	utMc16"
            //           + " exec usp_Emr_CalcAge '" + birthday.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "', @sjnl output, @xsnl output"
            //           + " select @sjnl, @xsnl";

            //        DataTable table = PersistentObjectFactory.SqlExecutor.ExecuteDataTable(cmd, false, CommandType.Text);

            //        accurateAge = Convert.ToInt32(table.Rows[0][0]);
            //        displayAge = table.Rows[0][1].ToString();
            //        return;
            //    }

            //    accurateAge = (endDate.Year - birthday.Year) * 10000;
            //    displayAge = Convert.ToString(endDate.Year - birthday.Year + 1);
            #endregion
        }

    }
}
