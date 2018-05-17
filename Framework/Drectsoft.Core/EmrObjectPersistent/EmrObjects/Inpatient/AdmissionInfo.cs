using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using DrectSoft.Common.Eop;



namespace DrectSoft.Common.Eop
{
    /// <summary>
    /// 住院信息
    /// </summary>
    public class AdmissionInfo : EPBaseObject
    {
        #region properties
        /// <summary>
        /// 入院基本信息
        /// </summary>
        public AdmissionBasicInfo AdmitInfo
        {
            get { return _admitInfo; }
            set { _admitInfo = value; }
        }
        private AdmissionBasicInfo _admitInfo;

        /// <summary>
        /// 出院基本信息
        /// </summary>
        public AdmissionBasicInfo DischargeInfo
        {
            get { return _dischargeInfo; }
            set { _dischargeInfo = value; }
        }
        private AdmissionBasicInfo _dischargeInfo;

        /// <summary>
        /// 入院途径（门诊、转院等）
        /// </summary>
        public CommonBaseCode AdmissionKind
        {
            get { return _admissionKind; }
            set { _admissionKind = value; }
        }
        private CommonBaseCode _admissionKind;

        /// <summary>
        /// 入院情况(入院时病情)
        /// </summary>
        public CommonBaseCode AdmitStatus
        {
            get { return _admitStatus; }
            set { _admitStatus = value; }
        }
        private CommonBaseCode _admitStatus;

        /// <summary>
        /// 住院天数
        /// </summary>
        public decimal LengthOfStay
        {
            get
            {
                return _lengthOfStay;
            }
            set { _lengthOfStay = value; }
        }
        private decimal _lengthOfStay;

        /// <summary>
        /// X线信息
        /// </summary>
        public string XRecordNo
        {
            get { return _xRecordNo; }
            set { _xRecordNo = value; }
        }
        private string _xRecordNo;

        /// <summary>
        /// 出院方式(治愈、好转等)
        /// </summary>
        public CommonBaseCode DischargeStatus
        {
            get { return _dischargeStatus; }
            set { _dischargeStatus = value; }
        }
        private CommonBaseCode _dischargeStatus;

        /// <summary>
        /// 门诊诊断
        /// </summary>
        public ICDDiagnosis DiagnosisOfClinic
        {
            get { return _diagnosisOfClinic; }
            set { _diagnosisOfClinic = value; }
        }
        private ICDDiagnosis _diagnosisOfClinic;

        /// <summary>
        /// 门诊医生
        /// </summary>
        public Employee ClinicDoctor
        {
            get { return _clinicDoctor; }
            set { _clinicDoctor = value; }
        }
        private Employee _clinicDoctor;

        /// <summary>
        /// 门诊诊断(中医)
        /// </summary>
        public ChineseDiagnosis ChineseDiagnosisOfClinic
        {
            get { return _chineseDiagnosisOfClinic; }
            set { _chineseDiagnosisOfClinic = value; }
        }
        private ChineseDiagnosis _chineseDiagnosisOfClinic;

        /// <summary>
        /// 门诊症候(中医)
        /// </summary>
        public ChineseDiagnosis ChineseDiagnosisOfClinic2
        {
            get { return _chineseDiagnosisOfClinic2; }
            set { _chineseDiagnosisOfClinic2 = value; }
        }
        private ChineseDiagnosis _chineseDiagnosisOfClinic2;

        /// <summary>
        /// 发病节气(中医使用)
        /// </summary>
        public string SolarTerm
        {
            get { return _solarTerm; }
            set { _solarTerm = value; }
        }
        private string _solarTerm;

        /// <summary>
        /// 住院医生
        /// </summary>
        public Employee Resident
        {
            get { return _resident; }
            set { _resident = value; }
        }
        private Employee _resident;

        /// <summary>
        /// 主治医生
        /// </summary>
        public Employee AttendingPhysician
        {
            get { return _attendingPhysician; }
            set { _attendingPhysician = value; }
        }
        private Employee _attendingPhysician;

        /// <summary>
        /// 主任医师代码
        /// </summary>
        public Employee Director
        {
            get { return _director; }
            set { _director = value; }
        }
        private Employee _director;

        /// <summary>
        /// 护理级别
        /// </summary>
        public ChargeItem CareLevel
        {
            get { return _careLevel; }
            set { _careLevel = value; }
        }
        private ChargeItem _careLevel;

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
        #endregion

        #region ctors
        /// <summary>
        /// 初始化空实例
        /// </summary>
        public AdmissionInfo()
            : base()
        { }

        /// <summary>
        /// 用DataRow初始化实例
        /// </summary>
        /// <param name="sourceRow"></param>
        public AdmissionInfo(DataRow sourceRow)
            : base(sourceRow)
        { }
        #endregion

        #region public methods
        /// <summary>
        /// 初始化所有的属性，包括引用类型的属性自己的属性
        /// </summary>
        public override void ReInitializeAllProperties()
        {
            try
            {
                if (AdmitInfo != null)
                    AdmitInfo.ReInitializeAllProperties();
                if (DischargeInfo != null)
                    DischargeInfo.ReInitializeAllProperties();
                if (AdmissionKind != null)
                    AdmissionKind.ReInitializeAllProperties();
                if (AdmitStatus != null)
                    AdmitStatus.ReInitializeAllProperties();
                if (DischargeStatus != null)
                    DischargeStatus.ReInitializeAllProperties();
                if (DiagnosisOfClinic != null)
                    DiagnosisOfClinic.ReInitializeAllProperties();
                if (ClinicDoctor != null)
                    ClinicDoctor.ReInitializeAllProperties();
                if (ChineseDiagnosisOfClinic != null)
                    ChineseDiagnosisOfClinic.ReInitializeAllProperties();
                if (ChineseDiagnosisOfClinic2 != null)
                    ChineseDiagnosisOfClinic2.ReInitializeAllProperties();
                if (Resident != null)
                    Resident.ReInitializeAllProperties();
                if (AttendingPhysician != null)
                    AttendingPhysician.ReInitializeAllProperties();
                if (Director != null)
                    Director.ReInitializeAllProperties();
                if (CareLevel != null)
                    CareLevel.ReInitializeAllProperties();
            }
            catch (Exception)
            {

            }
        }
        #endregion


        internal void EnsureLengthOfStay(string noOfHisFirstPage)
        {
            //以病人出区计算病人住院天数
            if ((DischargeInfo == null) || (DischargeInfo.StepOneDate == DateTime.MinValue))
                _lengthOfStay = -1;
            else
            {
                //出区日期-入区日期
                TimeSpan span = DischargeInfo.StepOneDate - AdmitInfo.StepTwoDate;
                _lengthOfStay = span.Days;
            }
            //调用HIS 函数计算病人住院天数
            //如此调用是否合理？！
            // string sql = "select dbo.fun_zyb_calzyts (" + noOfHisFirstPage + ",1,getdate())";
            //LengthOfStay = Convert.ToDecimal(hisDataAccess.ExecuteScalar(sql));

        }

        //因为有时外部也调用生日，所以就会发生生日日期不对，还是分开处理
        internal void EnsureLengthOfStay(decimal NoOfHisFirstPage, PatientBasicInfo patientBasic)
        {
            //以病人出区计算病人住院天数
            if ((DischargeInfo == null) || (DischargeInfo.StepOneDate == DateTime.MinValue))
                _lengthOfStay = -1;
            else
            {
                //出区日期-入区日期
                TimeSpan span = DischargeInfo.StepOneDate - AdmitInfo.StepTwoDate;
                _lengthOfStay = span.Days;
            }

            //         select dbo.fun_zyb_calzyts ('26',1,getdate())

            //select b.birth from ZY_BRSYK a
            //inner join ZY_BABYSYK b on (a.syxh=b.yesyxh)
            //where a.yebz=1 and b.syxh=26
            //调用HIS 函数计算病人住院天数
            //如此调用是否合理？！
            //string sql = "select dbo.fun_zyb_calzyts (" + NoOfHisFirstPage + ",1,getdate())";
            //if (patientBasic.BabySequence == 1)
            //{
            //    sql = sql + "select dbo.ufnConvertDateString(b.birth,'TD') from ZY_BRSYK a inner join ZY_BABYSYK b on (a.syxh=b.yesyxh)" +
            //          "where a.yebz=1 and a.syxh=" + NoOfHisFirstPage + "";
            //}
            //DataSet dsHisBasicInfo = hisDataAccess.ExecuteDataSet(sql);
            //LengthOfStay = Convert.ToDecimal(dsHisBasicInfo.Tables[0].Rows[0][0].ToString());
            //if (patientBasic.BabySequence == 1 && dsHisBasicInfo.Tables[1] != null && dsHisBasicInfo.Tables[1].Rows.Count > 0)
            //    patientBasic.Birthday = Convert.ToDateTime(dsHisBasicInfo.Tables[1].Rows[0][0].ToString());

        }
    }
}
