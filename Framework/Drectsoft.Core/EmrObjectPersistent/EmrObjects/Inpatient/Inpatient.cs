using System;
using System.Data;


namespace DrectSoft.Common.Eop
{
    /// <summary>
    /// ������ҳ����(���˶����ORM)
    /// </summary>
    public class Inpatient : EPBaseObject
    {

        #region properties
        /// <summary>
        /// ������Ҫ����
        /// </summary>
        public new string Code
        {
            get { return RecordNoOfHospital; }
        }

        /// <summary>
        /// ���ز�������
        /// </summary>
        public new string Name
        {
            get { return PersonalInformation.PatientName; }
        }


        /// <summary>
        /// ��ҳ���
        /// </summary>
        public decimal NoOfFirstPage
        {
            get { return _noOfFirstPage; }
            set { _noOfFirstPage = value; }
        }
        private decimal _noOfFirstPage;
        /// <summary>
        /// HIS��ҳ���
        /// </summary>
        public string NoOfHisFirstPage
        {
            get { return _noOfHisFirstPage; }
            set { _noOfHisFirstPage = value; }
        }
        private string _noOfHisFirstPage;

        /// <summary>
        /// �������
        /// </summary>
        public string RecordNoOfClinic
        {
            get { return _recordNoOfClinic; }
            set { _recordNoOfClinic = value; }
        }
        private string _recordNoOfClinic;

        /// <summary>
        /// ��������
        /// </summary>
        public string RecordNoOfMedical
        {
            get { return _recordNoOfMedical; }
            set { _recordNoOfMedical = value; }
        }
        private string _recordNoOfMedical;

        /// <summary>
        /// סԺ����
        /// </summary>
        public string RecordNoOfHospital
        {
            get { return _recordNoOfHospital; }
            set { _recordNoOfHospital = value; }
        }
        private string _recordNoOfHospital;

        /// <summary>
        /// �ڼ�����Ժ(>=1)
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
        /// ��ʷ������
        /// </summary>
        public string HistoryProvider
        {
            get { return _historyProvider; }
            set { _historyProvider = value; }
        }
        private string _historyProvider;

        /// <summary>
        /// ����״̬
        /// </summary>
        public InpatientState State
        {
            get { return _state; }
            set { _state = value; }
        }
        private InpatientState _state;

        /// <summary>
        /// ����ǰһ״̬,ʱ���������õ�
        /// </summary>
        public InpatientState PreState
        {
            get { return _preState; }
            set { _preState = value; }
        }
        private InpatientState _preState;

        /// <summary>
        /// Σ�ؼ���
        /// </summary>
        public CommonBaseCode PatientCondition
        {
            get { return _patientCondition; }
            set { _patientCondition = value; }
        }
        private CommonBaseCode _patientCondition;

        /// <summary>
        /// Ӥ����־
        /// </summary>
        public bool IsBaby
        {
            get { return _isBaby; }
            set { _isBaby = value; }
        }
        private bool _isBaby;


        /// <summary>
        /// ҽ������
        /// </summary>
        public MedicareType Medicare
        {
            get { return _medicare; }
            set { _medicare = value; }
        }
        private MedicareType _medicare;

        /// <summary>
        /// ҽ������
        /// </summary>
        public double MedicareQuota
        {
            get { return _medicareQuota; }
            set { _medicareQuota = value; }
        }
        private double _medicareQuota;

        /// <summary>
        /// ��������(ҽ�Ƹ��ʽ)
        /// </summary>
        public CommonBaseCode PaymentKind
        {
            get { return _pamentKind; }
            set { _pamentKind = value; }
        }
        private CommonBaseCode _pamentKind;

        /// <summary>
        /// ������Դ�����ء���أ� 
        /// </summary>
        public CommonBaseCode PatientSource
        {
            get { return _patientSource; }
            set { _patientSource = value; }
        }
        private CommonBaseCode _patientSource;

        /// <summary>
        /// �������ͣ����ˡ�ְҵ����
        /// </summary>
        public CommonBaseCode PatientKind
        {
            get { return _patientKind; }
            set { _patientKind = value; }
        }
        private CommonBaseCode _patientKind;

        /// <summary>
        /// ƾ֤���ʹ���
        /// </summary>
        public string VouchersCode
        {
            get { return _vouchersCode; }
            set { _vouchersCode = value; }
        }
        private string _vouchersCode;

        /// <summary>
        /// ƾ֤��������
        /// </summary>
        public string VouchersName
        {
            get { return _vouchersName; }
            set { _vouchersName = value; }
        }
        private string _vouchersName;

        /// <summary>
        /// ƾ֤��
        /// </summary>
        public string VouchersNo
        {
            get { return _vouchersNo; }
            set { _vouchersNo = value; }
        }
        private string _vouchersNo;

        /// <summary>
        /// ��¼Ա
        /// </summary>
        public Employee Recorder
        {
            get { return _recorder; }
            set { _recorder = value; }
        }
        private Employee _recorder;

        /// <summary>
        /// ��������
        /// </summary>
        public DateTime ModifyDate
        {
            get { return _modifyDate; }
            set { _modifyDate = value; }
        }
        private DateTime _modifyDate;

        /// <summary>
        /// ���˻�����Ϣ
        /// </summary>
        public PatientBasicInfo PersonalInformation
        {
            get
            {
                return _personalInformation;
            }
            set
            {
                _personalInformation = value;
            }
        }
        private PatientBasicInfo _personalInformation;

        /// <summary>
        /// סԺ��Ϣ
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
        /// ��ʵ����ƥ��ġ���ȡDB�����ݵ�SQL���
        /// </summary>
        public override string InitializeSentence
        {
            get { return GetQuerySentenceFromXml("SelectInpatient"); }
        }

        /// <summary>
        /// ��DataTable�а�����ֵ���˼�¼������
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
        /// �ô����ʼ��ʵ��
        /// </summary>
        /// <param name="firstPageNo"></param>
        public Inpatient(decimal firstPageNo)
            : base(firstPageNo.ToString())
        {
            NoOfFirstPage = firstPageNo;
            InitBaseInfo();
        }

        /// <summary>
        /// ��DataRow��ʼ��ʵ��
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
        /// ����HIS����ҳ��Ŵ������˵ľ�̬����
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
                DataTable table = PersistentObjectFactory.SqlExecutor.ExecuteDataTable(sqlStr, CommandType.Text);
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
        /// ��ʼ�����е����ԣ������������͵������Լ�������
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
        /// ���˱�Ƚϴ�Ĭ�ϵĳ�ʼ����ʽ�Ὣ������Ϣ��ȫ�������ٹ��ˣ������������������⡣
        /// ������ֻȡ�������˵����ݣ��Ҳ������棡����
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
        /// �Ƚϲ��˶���
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>�Ƿ����</returns>
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
        /// ���Hash��
        /// </summary>
        /// <returns>Hash��</returns>
        public override int GetHashCode()
        {
            return NoOfFirstPage.GetHashCode();
        }
        #endregion
    }
}
