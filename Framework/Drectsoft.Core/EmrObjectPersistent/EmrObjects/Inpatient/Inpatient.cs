using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using DrectSoft.Common.Eop;
using System.Data;
using System.Data.SqlClient;


namespace DrectSoft.Common.Eop
{
    /// <summary>
    /// 病人首页库类(病人对象的ORM)
    /// </summary>
    public class Inpatient : EPBaseObject
    {

        #region properties
        /// <summary>
        /// 返回主要号码
        /// </summary>
        public new string Code
        {
            get { return RecordNoOfHospital; }
        }

        /// <summary>
        /// 返回病人姓名
        /// </summary>
        public new string Name
        {
            get { return PersonalInformation.PatientName; }
        }

        /// <summary>
        /// 首页序号
        /// </summary>
        public decimal NoOfFirstPage
        {
            get { return _noOfFirstPage; }
            set { _noOfFirstPage = value; }
        }
        private decimal _noOfFirstPage;

        /// <summary>
        /// HIS首页序号
        /// </summary>
        public string NoOfHisFirstPage
        {
            get { return _noOfHisFirstPage; }
            set { _noOfHisFirstPage = value; }
        }
        private string _noOfHisFirstPage;

        /// <summary>
        /// 门诊号码
        /// </summary>
        public string RecordNoOfClinic
        {
            get { return _recordNoOfClinic; }
            set { _recordNoOfClinic = value; }
        }
        private string _recordNoOfClinic;

        /// <summary>
        /// 病案号码
        /// </summary>
        public string RecordNoOfMedical
        {
            get { return _recordNoOfMedical; }
            set { _recordNoOfMedical = value; }
        }
        private string _recordNoOfMedical;

        /// <summary>
        /// 住院号码
        /// </summary>
        public string RecordNoOfHospital
        {
            get { return _recordNoOfHospital; }
            set { _recordNoOfHospital = value; }
        }
        private string _recordNoOfHospital;

        /// <summary>
        /// 第几次入院(>=1)
        /// </summary>
        public int TimesOfAdmission
        {
            get { return _timesOfAdmission; }
            set
            {
                if (value < 1)
                    _timesOfAdmission = 1;
                else
                    _timesOfAdmission = value;
            }
        }
        private int _timesOfAdmission;

        /// <summary>
        /// 病史陈述者
        /// </summary>
        public string HistoryProvider
        {
            get { return _historyProvider; }
            set { _historyProvider = value; }
        }
        private string _historyProvider;

        /// <summary>
        /// 病人状态
        /// </summary>
        public InpatientState State
        {
            get { return _state; }
            set { _state = value; }
        }
        private InpatientState _state;

        /// <summary>
        /// 病人前一状态,时限质量会用到
        /// </summary>
        public InpatientState PreState
        {
            get { return _preState; }
            set { _preState = value; }
        }
        private InpatientState _preState;

        /// <summary>
        /// 危重级别
        /// </summary>
        public CommonBaseCode PatientCondition
        {
            get { return _patientCondition; }
            set { _patientCondition = value; }
        }
        private CommonBaseCode _patientCondition;

        /// <summary>
        /// 婴儿标志
        /// </summary>
        public bool IsBaby
        {
            get { return _isBaby; }
            set { _isBaby = value; }
        }
        private bool _isBaby;


        /// <summary>
        /// 医保代码
        /// </summary>
        public MedicareType Medicare
        {
            get { return _medicare; }
            set { _medicare = value; }
        }
        private MedicareType _medicare;

        /// <summary>
        /// 医保定额
        /// </summary>
        public double MedicareQuota
        {
            get { return _medicareQuota; }
            set { _medicareQuota = value; }
        }
        private double _medicareQuota;

        /// <summary>
        /// 病人性质(医疗付款方式)
        /// </summary>
        public CommonBaseCode PaymentKind
        {
            get { return _pamentKind; }
            set { _pamentKind = value; }
        }
        private CommonBaseCode _pamentKind;

        /// <summary>
        /// 病人来源（本地、外地） 
        /// </summary>
        public CommonBaseCode PatientSource
        {
            get { return _patientSource; }
            set { _patientSource = value; }
        }
        private CommonBaseCode _patientSource;

        /// <summary>
        /// 病人类型（工伤、职业病）
        /// </summary>
        public CommonBaseCode PatientKind
        {
            get { return _patientKind; }
            set { _patientKind = value; }
        }
        private CommonBaseCode _patientKind;

        /// <summary>
        /// 凭证类型代码
        /// </summary>
        public string VouchersCode
        {
            get { return _vouchersCode; }
            set { _vouchersCode = value; }
        }
        private string _vouchersCode;

        /// <summary>
        /// 凭证类型名称
        /// </summary>
        public string VouchersName
        {
            get { return _vouchersName; }
            set { _vouchersName = value; }
        }
        private string _vouchersName;

        /// <summary>
        /// 凭证号
        /// </summary>
        public string VouchersNo
        {
            get { return _vouchersNo; }
            set { _vouchersNo = value; }
        }
        private string _vouchersNo;

        /// <summary>
        /// 记录员
        /// </summary>
        public Employee Recorder
        {
            get { return _recorder; }
            set { _recorder = value; }
        }
        private Employee _recorder;

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime ModifyDate
        {
            get { return _modifyDate; }
            set { _modifyDate = value; }
        }
        private DateTime _modifyDate;

        /// <summary>
        /// 病人基本信息
        /// </summary>
        public PatientBasicInfo PersonalInformation
        {
            get
            {
                return _personalInformation;
            }
            set { _personalInformation = value; }
        }
        private PatientBasicInfo _personalInformation;

        /// <summary>
        /// 住院信息
        /// </summary>
        public AdmissionInfo InfoOfAdmission
        {
            get
            {
                if (_infoOfAdmission != null)

                    _infoOfAdmission.EnsureLengthOfStay(NoOfHisFirstPage);
                return _infoOfAdmission;
            }
            set { _infoOfAdmission = value; }
        }
        private AdmissionInfo _infoOfAdmission;

        
        /// <summary>
        /// 与实体类匹配的、读取DB中数据的SQL语句
        /// </summary>
        public override string InitializeSentence
        {
            get { return GetQuerySentenceFromXml("SelectInpatient"); }
        }

        /// <summary>
        /// 在DataTable中按主键值过滤记录的条件
        /// </summary>
        public override string FilterCondition
        {
            get { return "NoOfInpat = " + NoOfFirstPage; }
        }

        private string QuerySentence
        {
            get
            {
                if (InitializeSentence.ToLower().Contains("where"))
                    return InitializeSentence + " and " + FilterCondition;
                else
                    return InitializeSentence + " where " + FilterCondition;
            }
        }
        #endregion

        #region ctors
        /// <summary>
        /// 
        /// </summary>
        public Inpatient()
            : base()
        { }

        /// <summary>
        /// 用代码初始化实例
        /// </summary>
        /// <param name="firstPageNo"></param>
        public Inpatient(decimal firstPageNo)
            : base(firstPageNo.ToString())
        {
            NoOfFirstPage = firstPageNo;
            InitBaseInfo();
        }

        /// <summary>
        /// 用DataRow初始化实例
        /// </summary>
        /// <param name="sourceRow"></param>
        public Inpatient(DataRow sourceRow)
            : base(sourceRow)
        { }

        private void InitBaseInfo()
        {
            if (null != _personalInformation)
            {
                _personalInformation.NoOfFirstPage = NoOfFirstPage;
                DataRow row = GetInpatient(NoOfFirstPage);
                if (null != row)
                {
                    _personalInformation.InHosDate = string.IsNullOrEmpty(row["admitdate"].ToString().Trim()) ? DateTime.Now : DateTime.Parse(row["admitdate"].ToString());
                }
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return NoOfFirstPage.ToString();
        }

        /// <summary>
        /// 根据HIS的首页序号创建病人的静态方法
        /// </summary>
        /// <param name="noOfHisFirstPage"></param>
        /// <returns></returns>
        public static Inpatient CreateInpatientByHisSerialNo(decimal noOfHisFirstPage)
        {
            if (PersistentObjectFactory.SqlExecutor != null)
            {
                string cmd = GetQuerySentenceFromXml("SelectInpatient");
                if (cmd.ToLower().Contains("where"))
                    cmd += " and PatNoOfHis = " + noOfHisFirstPage.ToString();
                else
                    cmd += " where  PatNoOfHis = " + noOfHisFirstPage.ToString();

                DataTable table = PersistentObjectFactory.SqlExecutor.ExecuteDataTable(
                   cmd, false, CommandType.Text);
                if ((table != null) && (table.Rows.Count > 0))
                    return new Inpatient(table.Rows[0]);
            }
            return null;
        }

        public DataRow GetInpatient(decimal noOfHisFirstPage)
        {
            try
            {
                string sqlStr = " select * from inpatient where noofinpat = " + noOfHisFirstPage;
                DataTable table = PersistentObjectFactory.SqlExecutor.ExecuteDataTable(sqlStr,CommandType.Text);
                if (table != null && table.Rows.Count > 0)
                {
                    return table.Rows[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化所有的属性，包括引用类型的属性自己的属性
        /// </summary>
        public override void ReInitializeAllProperties()
        {
            ReInitializeProperties();
            if (PatientCondition != null)
                PatientCondition.ReInitializeAllProperties();
            if (Medicare != null)
                Medicare.ReInitializeAllProperties();
            if (PaymentKind != null)
                PaymentKind.ReInitializeAllProperties();
            if (PatientSource != null)
                PatientSource.ReInitializeAllProperties();
            if (PatientKind != null)
                PatientKind.ReInitializeAllProperties();
            if (Recorder != null)
                Recorder.ReInitializeAllProperties();
            if (PersonalInformation != null)
            {
                PersonalInformation.ReInitializeAllProperties();
                InitBaseInfo();
            }
            if (InfoOfAdmission != null)
                InfoOfAdmission.ReInitializeAllProperties();
        }

        /// <summary>
        /// 病人表比较大，默认的初始化方式会将病人信息先全部加载再过滤，这样做会有性能问题。
        /// 在这里只取单个病人的数据，且不做缓存！！！
        /// </summary>
        public override void ReInitializeProperties()
        {
            if (PersistentObjectFactory.SqlExecutor != null)
            {
                DataTable table = PersistentObjectFactory.SqlExecutor.ExecuteDataTable(
                   QuerySentence, false, CommandType.Text);
                if ((table != null) && (table.Rows.Count > 0))
                    Initialize(table.Rows[0]);
            }
        }

        /// <summary>
        /// 比较病人对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Inpatient newPat = obj as Inpatient;
            if (newPat == null)
                return false;

            return (NoOfFirstPage == newPat.NoOfFirstPage);
        }

        /// <summary>
        /// 获得Hash码
        /// </summary>
        /// <returns>Hash码</returns>
        public override int GetHashCode()
        {
            return NoOfFirstPage.GetHashCode();
        }
        #endregion
    }
}
