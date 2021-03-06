using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

using System.Globalization;
using log4net;

namespace DrectSoft.Common.Eop
{
    /// <summary>
    /// 职工类
    /// TODO: 最好是再继承出具体的医生类、护士类等子类
    /// </summary>
    public class Employee : EPBaseObject
    {
        #region properties
        /// <summary>
        /// 性别
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
            get { return _birthday; }
            set { _birthday = value; }
        }
        private DateTime _birthday;

        /// <summary>
        /// 婚姻状况
        /// </summary>
        public CommonBaseCode MaritalState
        {
            get { return _maritalState; }
            set { _maritalState = value; }
        }
        private CommonBaseCode _maritalState;

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityNo
        {
            get { return _identityNo; }
            set
            {
                // TODO: 此处需要添加身份证验证代码

                _identityNo = value;
            }
        }
        private string _identityNo;

        /// <summary>
        /// 所属科室
        /// </summary>
        public Department CurrentDept
        {
            get { return _currentDept; }
            set { _currentDept = value; }
        }
        private Department _currentDept;

        /// <summary>
        /// 所属病区
        /// </summary>
        public Ward CurrentWard
        {
            get { return _currentWard; }
            set { _currentWard = value; }
        }
        private Ward _currentWard;

        /// <summary>
        /// 职工类别
        /// </summary>
        public EmployeeKind Kind
        {
            get { return _kind; }
            set { _kind = value; }
        }
        private EmployeeKind _kind;

        /// <summary>
        /// 职称代码
        /// </summary>
        public CommonBaseCode TechnicalTitle
        {
            get { return _technicalTitle; }
            set { _technicalTitle = value; }
        }
        private CommonBaseCode _technicalTitle;

        /// <summary>
        /// 处方章号
        /// </summary>
        public string RecipeSealNo
        {
            get { return _recipeSealNo; }
            set { _recipeSealNo = value; }
        }
        private string _recipeSealNo;

        /// <summary>
        /// 是否有处方权
        /// </summary>
        public bool HasRecipeRight
        {
            get { return _hasRecipeRight; }
            set { _hasRecipeRight = value; }
        }
        private bool _hasRecipeRight;

        /// <summary>
        /// 是否有麻醉处方权
        /// </summary>
        public bool HasNarcosisRight
        {
            get { return _hasNarcosisRight; }
            set { _hasNarcosisRight = value; }
        }
        private bool _hasNarcosisRight;

        /// <summary>
        /// 分组编码(对医生进行分组管理时需要)
        /// </summary>
        public string GroupCode
        {
            get { return _groupCode; }
            set { _groupCode = value; }
        }
        private string _groupCode;

        /// <summary>
        /// 医生级别
        /// </summary>
        public string Grade
        {
            get { return _grade; }
            set { _grade = value; }
        }
        private string _grade;

        /// <summary>
        /// 医生级别数字（便于比较医生级别高低，从低到高，由1开始, -1表示无级别）
        /// </summary>
        public int DoctorGradeNumber
        {
            get
            {
                if (string.IsNullOrEmpty(Grade)) return -1;
                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), Grade);
                switch (grade)
                {
                    case DoctorGrade.Resident:
                        return 0;
                    case DoctorGrade.Attending:
                        return 1;
                    case DoctorGrade.AssociateChief:
                        return 2;
                    case DoctorGrade.Chief:
                        return 2;
                    case DoctorGrade.Nurse:
                        return 0;
                    default:
                        return -1;
                }
            }
        }

        /// <summary>
        /// 与实体类匹配的、读取DB中数据的SQL语句
        /// </summary>
        public override string InitializeSentence
        {
            get { return GetQuerySentenceFromXml("SelectEmployee"); }
        }

        /// <summary>
        /// 在DataTable中按主键值过滤记录的条件
        /// </summary>
        public override string FilterCondition
        {
            get { return FormatFilterString("UserID", Code); }
        }
        #endregion

        #region ctors
        /// <summary>
        /// 
        /// </summary>
        public Employee()
            : base()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        public Employee(string code)
            : base(code)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        public Employee(string code, string name)
            : base(code, name)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceRow"></param>
        public Employee(DataRow sourceRow)
            : base(sourceRow)
        { }

        #endregion

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(Name))
                ReInitializeProperties();

            if (String.IsNullOrEmpty(Name))
                return String.Format(CultureInfo.CurrentCulture, "[{0}]", Code);
            else
                return String.Format(CultureInfo.CurrentCulture, "{0}", Name);
        }

        /// <summary>
        /// 初始化所有的属性，包括引用类型的属性自己的属性
        /// </summary>
        public override void ReInitializeAllProperties()
        {
            ReInitializeProperties();
            if (Sex != null)
                Sex.ReInitializeAllProperties();
            if (MaritalState != null)
                MaritalState.ReInitializeAllProperties();
            if (CurrentDept != null)
                CurrentDept.ReInitializeAllProperties();
            if (CurrentWard != null)
                CurrentWard.ReInitializeAllProperties();
            if (TechnicalTitle != null)
                TechnicalTitle.ReInitializeAllProperties();
        }
        #endregion
    }
}
