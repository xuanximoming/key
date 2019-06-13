using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YidanSoft.Core.IEMMainPage
{
    public class Print_IemMainPageInfo
    {
        private Print_Iem_Mainpage_Basicinfo m_IemBasicInfo = new Print_Iem_Mainpage_Basicinfo();
        private Print_Iem_Mainpage_Diagnosis m_IemDiagInfo = new Print_Iem_Mainpage_Diagnosis();
        private Print_Iem_MainPage_Operation m_IemOperInfo = new Print_Iem_MainPage_Operation();
        private Print_Iem_MainPage_Fee m_IemFeeInfo = new Print_Iem_MainPage_Fee();
        private Print_Iem_MainPage_ObstetricsBaby m_IemObstetricsBaby = new Print_Iem_MainPage_ObstetricsBaby();

        /// <summary>
        /// 病案首页基本信息
        /// </summary>
        public Print_Iem_Mainpage_Basicinfo IemBasicInfo
        {
            get { return m_IemBasicInfo; }
            set { m_IemBasicInfo = value; }
        }

        /// <summary>
        /// 病案首页诊断信息
        /// </summary>
        public Print_Iem_Mainpage_Diagnosis IemDiagInfo
        {
            get { return m_IemDiagInfo; }
            set { m_IemDiagInfo = value; }
        }


        /// <summary>
        /// 病案首页手术信息
        /// </summary>
        public Print_Iem_MainPage_Operation IemOperInfo
        {
            get { return m_IemOperInfo; }
            set { m_IemOperInfo = value; }
        }

        /// <summary>
        /// 病案首页费用信息
        /// </summary>
        public Print_Iem_MainPage_Fee IemFeeInfo
        {
            get { return m_IemFeeInfo; }
            set { m_IemFeeInfo = value; }
        }

        /// <summary>
        /// 妇科婴儿产妇情况
        /// </summary>
        public Print_Iem_MainPage_ObstetricsBaby IemObstetricsBaby
        {
            get { return m_IemObstetricsBaby; }
            set { m_IemObstetricsBaby = value; }
        }

    }

    #region 打印病案首页诊断基础信息板块
    /// <summary>
    /// 打印病案首页诊断基础信息板块
    /// </summary>
    public class Print_Iem_Mainpage_Basicinfo
    {

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        private string _PayName;
        /// <summary>
        /// 医疗付款方式
        /// </summary>
        public string PayName
        {
            get { return _PayName; }
            set { _PayName = value; }
        }


        private string _InCount;
        /// <summary>
        /// 医疗付款方式
        /// </summary>
        public string InCount
        {
            get { return _InCount; }
            set { _InCount = value; }
        }

        /// <summary>
        /// 病案号
        /// </summary>
        public string PatNoOfHis
        {
            get
            {
                return _PatNoOfHis;
            }
            set
            {
                _PatNoOfHis = value;
            }
        }
        private string _PatNoOfHis;

        /// <summary>
        /// 患者姓名
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        private string _Name;

        /// <summary>
        /// 性别编号
        /// </summary>
        public string SexID
        {
            get
            {
                return _SexID;
            }
            set
            {
                _SexID = value;
            }
        }
        private string _SexID;

        /// <summary>
        /// 出生
        /// </summary>
        public string Birth
        {
            get
            {
                if (_Birth == "")
                    return "";
                else
                    return Convert.ToDateTime(_Birth).ToString("yyyy年MM月dd日");
            }
            set
            {
                _Birth = value;
            }
        }
        private string _Birth;

        /// <summary>
        /// 年龄
        /// </summary>
        public string Age
        {
            get
            {
                return _Age;
            }
            set
            {
                _Age = value;
            }
        }
        private string _Age;

        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string Marital
        {
            get
            {
                return _Marital;
            }
            set
            {
                _Marital = value;
            }
        }
        private string _Marital;

        /// <summary>
        /// 职业
        /// </summary>
        public string JobName
        {
            get
            {
                return _JobName;
            }
            set
            {
                _JobName = value;
            }
        }
        private string _JobName;

        /// <summary>
        /// 出生地
        /// </summary>
        public string CountyName
        {
            get
            {
                return _CountyName;
            }
            set
            {
                _CountyName = value;
            }
        }
        private string _CountyName;

        /// <summary>
        /// 民族
        /// </summary>
        public string NationName
        {
            get
            {
                return _NationName;
            }
            set
            {
                _NationName = value;
            }
        }
        private string _NationName;

        /// <summary>
        /// 国籍
        /// </summary>
        public string NationalityName
        {
            get
            {
                return _NationalityName;
            }
            set
            {
                _NationalityName = value;
            }
        }
        private string _NationalityName;

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDNO
        {
            get
            {
                return _IDNO;
            }
            set
            {
                _IDNO = value;
            }
        }
        private string _IDNO;

        /// <summary>
        /// 工作单位及地址
        /// </summary>
        public string OfficePlace
        {
            get
            {
                return _OfficePlace;
            }
            set
            {
                _OfficePlace = value;
            }
        }
        private string _OfficePlace;

        /// <summary>
        /// 工作单位电话
        /// </summary>
        public string OfficeTEL
        {
            get
            {
                return _OfficeTEL;
            }
            set
            {
                _OfficeTEL = value;
            }
        }
        private string _OfficeTEL;

        /// <summary>
        /// 工作单位邮编
        /// </summary>
        public string OfficePost
        {
            get
            {
                return _OfficePost;
            }
            set
            {
                _OfficePost = value;
            }
        }
        private string _OfficePost;

        /// <summary>
        /// 户口地址
        /// </summary>
        public string NativeAddress
        {
            get
            {
                return _NativeAddress;
            }
            set
            {
                _NativeAddress = value;
            }
        }
        private string _NativeAddress;

        /// <summary>
        /// 户口电话
        /// </summary>
        public string NativeTEL
        {
            get
            {
                return _NativeTEL;
            }
            set
            {
                _NativeTEL = value;
            }
        }
        private string _NativeTEL;

        /// <summary>
        /// 户口所在地邮编
        /// </summary>
        public string NativePost
        {
            get
            {
                return _NativePost;
            }
            set
            {
                _NativePost = value;
            }
        }
        private string _NativePost;

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string ContactPerson
        {
            get
            {
                return _ContactPerson;
            }
            set
            {
                _ContactPerson = value;
            }
        }
        private string _ContactPerson;

        /// <summary>
        /// 与联系人关系
        /// </summary>
        public string Relationship
        {
            get
            {
                return _Relationship;
            }
            set
            {
                _Relationship = value;
            }
        }
        private string _Relationship;

        /// <summary>
        /// 联系人地址
        /// </summary>
        public string ContactAddress
        {
            get
            {
                return _ContactAddress;
            }
            set
            {
                _ContactAddress = value;
            }
        }
        private string _ContactAddress;

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactTEL
        {
            get
            {
                return _ContactTEL;
            }
            set
            {
                _ContactTEL = value;
            }
        }
        private string _ContactTEL;

        /// <summary>
        /// 入院时间
        /// </summary>
        public string AdmitDate
        {
            get
            {
                if (_AdmitDate == "")
                    return "";
                else
                    return Convert.ToDateTime(_AdmitDate).ToString("yyyy年MM月dd日 HH时");
            }
            set
            {
                _AdmitDate = value;
            }
        }
        private string _AdmitDate;

        /// <summary>
        /// 入院科室
        /// </summary>
        public string AdmitDeptName
        {
            get
            {
                return _AdmitDeptName;
            }
            set
            {
                _AdmitDeptName = value;
            }
        }
        private string _AdmitDeptName;

        /// <summary>
        /// 入院病区
        /// </summary>
        public string AdmitWardName
        {
            get
            {
                return _AdmitWardName;
            }
            set
            {
                _AdmitWardName = value;
            }
        }
        private string _AdmitWardName;

        /// <summary>
        /// 转院科别
        /// </summary>
        public string Trans_AdmitDept
        {
            get
            {
                return _Trans_AdmitDept;
            }
            set
            {
                _Trans_AdmitDept = value;
            }
        }
        private string _Trans_AdmitDept;

        /// <summary>
        /// 出院时间
        /// </summary>
        public string OutWardDate
        {
            get
            {
                if (_OutWardDate == "")
                    return "";
                else
                    return Convert.ToDateTime(_OutWardDate).ToString("yyyy年MM月dd日 HH时");
            }
            set
            {
                _OutWardDate = value;
            }
        }
        private string _OutWardDate;

        /// <summary>
        /// 出院科室
        /// </summary>
        public string OutHosDeptName
        {
            get
            {
                return this._OutHosDeptName;
            }
            set
            {
                _OutHosDeptName = value;
            }
        }
        private string _OutHosDeptName;

        /// <summary>
        /// 出院病区
        /// </summary>
        public string OutHosWardName
        {
            get
            {
                return this._OutHosWardName;
            }
            set
            {
                _OutHosWardName = value;
            }
        }
        private string _OutHosWardName;

        /// <summary>
        /// 实际住院天数
        /// </summary>
        public string ActualDays
        {
            get
            {
                return this._ActualDays;
            }
            set
            {
                _ActualDays = value;
            }
        }
        private string _ActualDays;

        /******************************************************************************************************************************************/





        #region

        public decimal Print_Iem_Mainpage_NO
        {
            get
            {
                return _Print_Iem_Mainpage_NO;
            }
            set
            {
                _Print_Iem_Mainpage_NO = value;
            }
        }
        private decimal _Print_Iem_Mainpage_NO;

        public decimal NoOfInpat
        {
            get
            {
                return _NoOfInpat;
            }
            set
            {
                _NoOfInpat = value;
            }
        }
        private decimal _NoOfInpat;

        public string PayID
        {
            get
            {
                return _PayID;
            }
            set
            {
                _PayID = value;
            }
        }
        private string _PayID;

        public string SocialCare
        {
            get
            {
                return _SocialCare;
            }
            set
            {
                _SocialCare = value;
            }
        }
        private string _SocialCare;


        public string JobID
        {
            get
            {
                return _JobID;
            }
            set
            {
                _JobID = value;
            }
        }
        private string _JobID;

        public string ProvinceID
        {
            get
            {
                return _ProvinceID;
            }
            set
            {
                _ProvinceID = value;
            }
        }
        private string _ProvinceID;

        public string NationID
        {
            get
            {
                return _NationID;
            }
            set
            {
                _NationID = value;
            }
        }
        private string _NationID;


        public string Organization
        {
            get
            {
                return _Organization;
            }
            set
            {
                _Organization = value;
            }
        }
        private string _Organization;

        public global::System.Nullable<decimal> Days_Before
        {
            get
            {
                return _Days_Before;
            }
            set
            {
                _Days_Before = value;
            }
        }
        private global::System.Nullable<decimal> _Days_Before;
        public string Trans_Date
        {
            get
            {
                return _Trans_Date;
            }
            set
            {
                _Trans_Date = value;
            }
        }
        private string _Trans_Date;

        public string Trans_AdmitWard
        {
            get
            {
                return _Trans_AdmitWard;
            }
            set
            {
                _Trans_AdmitWard = value;
            }
        }
        private string _Trans_AdmitWard;

        public string Trans_AdmitDept_Again
        {
            get
            {
                return _Trans_AdmitDept_Again;
            }
            set
            {
                _Trans_AdmitDept_Again = value;
            }
        }
        private string _Trans_AdmitDept_Again;

        public string Death_Time
        {
            get
            {
                return this._Death_Time;
            }
            set
            {
                _Death_Time = value;
            }
        }
        private string _Death_Time;

        public string Death_Reason
        {
            get
            {
                return this._Death_Reason;
            }
            set
            {
                _Death_Reason = value;
            }
        }
        private string _Death_Reason;

        public string AdmitInfo
        {
            get
            {
                return this._AdmitInfo;
            }
            set
            {
                _AdmitInfo = value;
            }
        }
        private string _AdmitInfo;

        public string In_Check_Date
        {
            get
            {
                return this._In_Check_Date;
            }
            set
            {
                _In_Check_Date = value;
            }
        }
        private string _In_Check_Date;


        private string _Pathology_Diagnosis_Name;

        public string Pathology_Observation_Sn
        {
            get
            {
                return this._Pathology_Observation_Sn;
            }
            set
            {
                _Pathology_Observation_Sn = value;
            }
        }
        private string _Pathology_Observation_Sn;

        public string Ashes_Diagnosis_Name
        {
            get
            {
                return this._Ashes_Diagnosis_Name;
            }
            set
            {
                _Ashes_Diagnosis_Name = value;
            }
        }
        private string _Ashes_Diagnosis_Name;

        public string Ashes_Anatomise_Sn
        {
            get
            {
                return this._Ashes_Anatomise_Sn;
            }
            set
            {
                _Ashes_Anatomise_Sn = value;
            }
        }
        private string _Ashes_Anatomise_Sn;

        public string Xay_Sn
        {
            get
            {
                return this._Xay_Sn;
            }
            set
            {
                _Xay_Sn = value;
            }
        }
        private string _Xay_Sn;

        public string Ct_Sn
        {
            get
            {
                return this._Ct_Sn;
            }
            set
            {
                _Ct_Sn = value;
            }
        }
        private string _Ct_Sn;

        public string Mri_Sn
        {
            get
            {
                return this._Mri_Sn;
            }
            set
            {
                _Mri_Sn = value;
            }
        }
        private string _Mri_Sn;

        public string Dsa_Sn
        {
            get
            {
                return this._Dsa_Sn;
            }
            set
            {
                _Dsa_Sn = value;
            }
        }
        private string _Dsa_Sn;

        public global::System.Nullable<decimal> Is_First_Case
        {
            get
            {
                return this._Is_First_Case;
            }
            set
            {
                _Is_First_Case = value;
            }
        }
        private global::System.Nullable<decimal> _Is_First_Case;

        public global::System.Nullable<decimal> Is_Following
        {
            get
            {
                return this._Is_Following;
            }
            set
            {
                _Is_Following = value;
            }
        }
        private global::System.Nullable<decimal> _Is_Following;

        public string Following_Ending_Date
        {
            get
            {
                return this._Following_Ending_Date;
            }
            set
            {
                _Following_Ending_Date = value;
            }
        }
        private string _Following_Ending_Date;

        public global::System.Nullable<decimal> Is_Teaching_Case
        {
            get
            {
                return this._Is_Teaching_Case;
            }
            set
            {
                _Is_Teaching_Case = value;
            }
        }
        private global::System.Nullable<decimal> _Is_Teaching_Case;

        public global::System.Nullable<decimal> Blood_Type_id
        {
            get
            {
                return this._Blood_Type_id;
            }
            set
            {
                _Blood_Type_id = value;
            }
        }
        private global::System.Nullable<decimal> _Blood_Type_id;

        public global::System.Nullable<decimal> Rh
        {
            get
            {
                return this._Rh;
            }
            set
            {
                _Rh = value;
            }
        }
        private global::System.Nullable<decimal> _Rh;

        public global::System.Nullable<decimal> Blood_Reaction_Id
        {
            get
            {
                return this._Blood_Reaction_Id;
            }
            set
            {
                _Blood_Reaction_Id = value;
            }
        }
        private global::System.Nullable<decimal> _Blood_Reaction_Id;

        public global::System.Nullable<decimal> Blood_Rbc
        {
            get
            {
                return this._Blood_Rbc;
            }
            set
            {
                _Blood_Rbc = value;
            }
        }
        private global::System.Nullable<decimal> _Blood_Rbc;

        public global::System.Nullable<decimal> Blood_Plt
        {
            get
            {
                return this._Blood_Plt;
            }
            set
            {
                _Blood_Plt = value;
            }
        }
        private global::System.Nullable<decimal> _Blood_Plt;

        public global::System.Nullable<decimal> Blood_Plasma
        {
            get
            {
                return this._Blood_Plasma;
            }
            set
            {
                _Blood_Plasma = value;
            }
        }
        private global::System.Nullable<decimal> _Blood_Plasma;

        public global::System.Nullable<decimal> Blood_Wb
        {
            get
            {
                return this._Blood_Wb;
            }
            set
            {
                _Blood_Wb = value;
            }
        }
        private global::System.Nullable<decimal> _Blood_Wb;

        public string Blood_Others
        {
            get
            {
                return this._Blood_Others;
            }
            set
            {
                _Blood_Others = value;
            }
        }
        private string _Blood_Others;

        public string Is_Completed
        {
            get
            {
                return this._Is_Completed;
            }
            set
            {
                _Is_Completed = value;
            }
        }
        private string _Is_Completed;

        public string completed_time
        {
            get
            {
                return this._completed_time;
            }
            set
            {
                _completed_time = value;
            }
        }
        private string _completed_time;

        public decimal Valide
        {
            get
            {
                return this._Valide;
            }
            set
            {
                _Valide = value;
            }
        }
        private decimal _Valide;

        public string Create_User
        {
            get
            {
                return this._Create_User;
            }
            set
            {
                _Create_User = value;
            }
        }
        private string _Create_User;

        public string Create_Time
        {
            get
            {
                return this._Create_Time;
            }
            set
            {
                _Create_Time = value;
            }
        }
        private string _Create_Time;

        public string Modified_User
        {
            get
            {
                return this._Modified_User;
            }
            set
            {
                _Modified_User = value;
            }
        }
        private string _Modified_User;

        public string Modified_Time
        {
            get
            {
                return this._Modified_Time;
            }
            set
            {
                _Modified_Time = value;
            }
        }
        private string _Modified_Time;

        #endregion

    }
    #endregion

    #region 打印病案首页 诊断板块信息
    /// <summary>
    /// 打印病案首页 诊断板块信息
    /// </summary>
    public class Print_Iem_Mainpage_Diagnosis
    {
        /// <summary>
        /// 门急诊诊断
        /// </summary>
        public string OutDiag { get; set; }

        /// <summary>
        /// 入院状态：危急一般
        /// </summary>
        public string AdmitInfo { get; set; }

        /// <summary>
        /// 入院诊断
        /// </summary>
        public string InDiag { get; set; }

        /// <summary>
        /// 入院确诊时间
        /// </summary>
        public string In_Check_Date
        {
            get
            {
                if (_In_Check_Date == "")
                    return "";
                else
                    return Convert.ToDateTime(_In_Check_Date).ToString("yyyy年MM月dd日");
            }
            set { _In_Check_Date = value; }
        }
        private string _In_Check_Date;

        /// <summary>
        /// 出院诊断表  包括字段Diagnosis_Name   Status  Diagnosis_Code
        /// </summary>
        public DataTable OutDiagTable { get; set; }

        /// <summary>
        /// 医院传染病名称
        /// </summary>
        public string ZymosisName { get; set; }

        /// <summary>
        /// 医院传染病ICD-10
        /// </summary>
        public string ZymosisCode { get; set; }

        /// <summary>
        /// 医院传染病状态
        /// </summary>
        public string ZymosisState { get; set; }

        /// <summary>
        /// 病历诊断名称
        /// </summary>
        public string Pathology_Diagnosis_Name
        {
            get
            {
                return this._Pathology_Diagnosis_Name;
            }
            set
            {
                _Pathology_Diagnosis_Name = value;
            }
        }
        private string _Pathology_Diagnosis_Name;


        /// <summary>
        /// 损伤、中毒的外部因素：
        /// </summary>
        public string Hurt_Toxicosis_Element
        {
            get
            {
                return this._Hurt_Toxicosis_Element;
            }
            set
            {
                _Hurt_Toxicosis_Element = value;
            }
        }
        private string _Hurt_Toxicosis_Element;

        /// <summary>
        /// 药物过敏
        /// </summary>
        public string Allergic_Drug
        {
            get
            {
                return this._Allergic_Drug;
            }
            set
            {
                _Allergic_Drug = value;
            }
        }
        private string _Allergic_Drug;

        /// <summary>
        /// Hbsag
        /// </summary>
        public string Hbsag
        {
            get
            {
                return this._Hbsag;
            }
            set
            {
                _Hbsag = value;
            }
        }
        private string _Hbsag;

        /// <summary>
        /// Hcv_Ab
        /// </summary>
        public string Hcv_Ab
        {
            get
            {
                return this._Hcv_Ab;
            }
            set
            {
                _Hcv_Ab = value;
            }
        }
        private string _Hcv_Ab;

        /// <summary>
        /// Hiv_Ab
        /// </summary>
        public string Hiv_Ab
        {
            get
            {
                return this._Hiv_Ab;
            }
            set
            {
                _Hiv_Ab = value;
            }
        }
        private string _Hiv_Ab;


        /// <summary>
        /// 诊断符合情况  门诊与出院
        /// </summary>
        public string Opd_Ipd_Id
        {
            get
            {
                return this._Opd_Ipd_Id;
            }
            set
            {
                _Opd_Ipd_Id = value;
            }
        }
        private string _Opd_Ipd_Id;

        /// <summary>
        /// 入院与出院
        /// </summary>
        public string In_Out_Inpatinet_Id
        {
            get
            {
                return this._In_Out_Inpatinet_Id;
            }
            set
            {
                _In_Out_Inpatinet_Id = value;
            }
        }
        private string _In_Out_Inpatinet_Id;

        /// <summary>
        /// 术前与术后
        /// </summary>
        public string Before_After_Or_Id
        {
            get
            {
                return this._Before_After_Or_Id;
            }
            set
            {
                _Before_After_Or_Id = value;
            }
        }
        private string _Before_After_Or_Id;

        /// <summary>
        /// 临床与病理
        /// </summary>
        public string Clinical_Pathology_Id
        {
            get
            {
                return this._Clinical_Pathology_Id;
            }
            set
            {
                _Clinical_Pathology_Id = value;
            }
        }
        private string _Clinical_Pathology_Id;

        /// <summary>
        /// 放射与病理
        /// </summary>
        public string Pacs_Pathology_Id
        {
            get
            {
                return this._Pacs_Pathology_Id;
            }
            set
            {
                _Pacs_Pathology_Id = value;
            }
        }
        private string _Pacs_Pathology_Id;

        /// <summary>
        /// 抢救次数
        /// </summary>
        public string Save_Times
        {
            get
            {
                return this._Save_Times;
            }
            set
            {
                _Save_Times = value;
            }
        }
        private string _Save_Times;

        /// <summary>
        /// 成功次数
        /// </summary>
        public string Success_Times
        {
            get
            {
                return this._Success_Times;
            }
            set
            {
                _Success_Times = value;
            }
        }
        private string _Success_Times;

        /// <summary>
        /// 科主任
        /// </summary>
        public string Section_Director
        {
            get
            {
                return this._Section_Director;
            }
            set
            {
                _Section_Director = value;
            }
        }
        private string _Section_Director;

        /// <summary>
        /// 主（副主）任医师
        /// </summary>
        public string Director
        {
            get
            {
                return this._Director;
            }
            set
            {
                _Director = value;
            }
        }
        private string _Director;

        /// <summary>
        /// 主治医师
        /// </summary>
        public string Vs_Employee_Code
        {
            get
            {
                return this._Vs_Employee_Code;
            }
            set
            {
                _Vs_Employee_Code = value;
            }
        }
        private string _Vs_Employee_Code;

        /// <summary>
        /// 住院医师
        /// </summary>
        public string Resident_Employee_Code
        {
            get
            {
                return this._Resident_Employee_Code;
            }
            set
            {
                _Resident_Employee_Code = value;
            }
        }
        private string _Resident_Employee_Code;

        /// <summary>
        /// 进修医师
        /// </summary>
        public string Refresh_Employee_Code
        {
            get
            {
                return this._Refresh_Employee_Code;
            }
            set
            {
                _Refresh_Employee_Code = value;
            }
        }
        private string _Refresh_Employee_Code;

        /// <summary>
        /// 研究生实习医师
        /// </summary>
        public string Master_Interne
        {
            get
            {
                return this._Master_Interne;
            }
            set
            {
                _Master_Interne = value;
            }
        }
        private string _Master_Interne;

        /// <summary>
        /// 实习医师
        /// </summary>
        public string Interne
        {
            get
            {
                return this._Interne;
            }
            set
            {
                _Interne = value;
            }
        }
        private string _Interne;

        /// <summary>
        /// 编码员
        /// </summary>
        public string Coding_User
        {
            get
            {
                return this._Coding_User;
            }
            set
            {
                _Coding_User = value;
            }
        }
        private string _Coding_User;

        /// <summary>
        /// 病案质量
        /// </summary>
        public string Medical_Quality_Id
        {
            get
            {
                return this._Medical_Quality_Id;
            }
            set
            {
                _Medical_Quality_Id = value;
            }
        }
        private string _Medical_Quality_Id;

        /// <summary>
        /// 质控医师
        /// </summary>
        public string Quality_Control_Doctor
        {
            get
            {
                return this._Quality_Control_Doctor;
            }
            set
            {
                _Quality_Control_Doctor = value;
            }
        }
        private string _Quality_Control_Doctor;

        /// <summary>
        /// 质控护士
        /// </summary>
        public string Quality_Control_Nurse
        {
            get
            {
                return this._Quality_Control_Nurse;
            }
            set
            {
                _Quality_Control_Nurse = value;
            }
        }
        private string _Quality_Control_Nurse;

        /// <summary>
        /// 日期：
        /// </summary>
        public string Quality_Control_Date
        {
            get
            {
                if (_Quality_Control_Date == "")
                    return "";
                else
                    return Convert.ToDateTime(_Quality_Control_Date).ToString("yyyy年MM月dd日");
            }
            set
            {
                _Quality_Control_Date = value;
            }
        }
        private string _Quality_Control_Date;

        public decimal Iem_Mainpage_Diagnosis_NO
        {
            get
            {
                return _Iem_Mainpage_Diagnosis_NO;
            }
            set
            {
                _Iem_Mainpage_Diagnosis_NO = value;
            }
        }
        private decimal _Iem_Mainpage_Diagnosis_NO;

        public decimal Iem_Mainpage_NO
        {
            get
            {
                return _Iem_Mainpage_NO;
            }
            set
            {
                _Iem_Mainpage_NO = value;
            }
        }
        private decimal _Iem_Mainpage_NO;


    }
    #endregion

    #region 打印病案首页 手术信息板块
    /// <summary>
    /// 打印病案首页 手术信息板块
    /// </summary>
    public class Print_Iem_MainPage_Operation
    {

        public decimal IEM_MainPage_NO
        {
            get
            {
                return this._IEM_MainPage_NO;
            }
            set
            {
                _IEM_MainPage_NO = value;
            }
        }
        private decimal _IEM_MainPage_NO;

        /// <summary>
        /// 病人手术信息
        /// 手术操作码  	Operation_Code  手术操作日期	Operation_Date  手术操作名称	Operation_Name  术者		Execute_User1_Name  术者ID		Execute_User1
        ///I助		Execute_User2_Name  I助ID		Execute_User2   II助		Execute_User3_Name  II助ID		Execute_User3   麻醉方式	Anaesthesia_Type_Name
        ///麻醉方式ID	Anaesthesia_Type_Id     切口愈合等级	Close_Level_Name    切口愈合等级ID	Close_Level
        ///麻醉医师	Anaesthesia_User_Name   麻醉医师ID	Anaesthesia_User
        /// </summary>
        public DataTable Operation_Table
        {
            get { return _Operation_Table; }
            set { _Operation_Table = value; }
        }
        private DataTable _Operation_Table;

        public string Operation_Code
        {
            get
            {
                return this._Operation_Code;
            }
            set
            {
                _Operation_Code = value;
            }
        }
        private string _Operation_Code;

        public string Operation_Date
        {
            get
            {
                return this._Operation_Date;
            }
            set
            {
                _Operation_Date = value;
            }
        }
        private string _Operation_Date;

        public string Operation_Name
        {
            get
            {
                return this._Operation_Name;
            }
            set
            {
                _Operation_Name = value;
            }
        }
        private string _Operation_Name;

        public string Execute_User1
        {
            get
            {
                return this._Execute_User1;
            }
            set
            {
                _Execute_User1 = value;
            }
        }
        private string _Execute_User1;

        public string Execute_User2
        {
            get
            {
                return this._Execute_User2;
            }
            set
            {
                _Execute_User2 = value;
            }
        }
        private string _Execute_User2;

        public string Execute_User3
        {
            get
            {
                return this._Execute_User3;
            }
            set
            {
                _Execute_User3 = value;
            }
        }
        private string _Execute_User3;

        public global::System.Nullable<decimal> Anaesthesia_Type_Id
        {
            get
            {
                return this._Anaesthesia_Type_Id;
            }
            set
            {
                _Anaesthesia_Type_Id = value;
            }
        }
        private global::System.Nullable<decimal> _Anaesthesia_Type_Id;

        public global::System.Nullable<decimal> Close_Level
        {
            get
            {
                return this._Close_Level;
            }
            set
            {
                _Close_Level = value;
            }
        }
        private global::System.Nullable<decimal> _Close_Level;

        public string Anaesthesia_User
        {
            get
            {
                return this._Anaesthesia_User;
            }
            set
            {
                _Anaesthesia_User = value;
            }
        }
        private string _Anaesthesia_User;

        public decimal Valide
        {
            get
            {
                return this._Valide;
            }
            set
            {
                _Valide = value;
            }
        }
        private decimal _Valide;

        public string Create_User
        {
            get
            {
                return this._Create_User;
            }
            set
            {
                _Create_User = value;
            }
        }
        private string _Create_User;

        public string Create_Time
        {
            get
            {
                return this._Create_Time;
            }
            set
            {
                _Create_Time = value;
            }
        }
        private string _Create_Time;

        public string Cancel_User
        {
            get
            {
                return this._Cancel_User;
            }
            set
            {
                _Cancel_User = value;
            }
        }
        private string _Cancel_User;

        public string Cancel_Time
        {
            get
            {
                return this._Cancel_Time;
            }
            set
            {
                _Cancel_Time = value;
            }
        }
        private string _Cancel_Time;

    }
    #endregion

    #region 打印病案首页 费用信息板块
    /// <summary>
    /// 打印病案首页 费用信息板块
    /// </summary>
    public class Print_Iem_MainPage_Fee
    {
        public decimal IEM_MainPage_NO
        {
            get
            {
                return this._IEM_MainPage_NO;
            }
            set
            {
                _IEM_MainPage_NO = value;
            }
        }
        private decimal _IEM_MainPage_NO;

        /// <summary>
        /// 总费用
        /// </summary>
        public string Total { get; set; }

        /// <summary>
        /// 床费
        /// </summary>
        public string Bed { get; set; }

        /// <summary>
        /// 护理费用
        /// </summary>
        public string Care { get; set; }

        /// <summary>
        /// 西药
        /// </summary>
        public string WMedical { get; set; }

        /// <summary>
        ///中成药
        /// </summary>
        public string CPMedical { get; set; }

        /// <summary>
        /// 中草药
        /// </summary>
        public string CMedical { get; set; }

        /// <summary>
        /// 放射
        /// </summary>
        public string Radiate { get; set; }

        /// <summary>
        /// 化验
        /// </summary>
        public string Assay { get; set; }

        /// <summary>
        /// 输氧
        /// </summary>
        public string Ox { get; set; }

        /// <summary>
        /// 输血
        /// </summary>
        public string Blood { get; set; }

        /// <summary>
        /// 诊疗
        /// </summary>
        public string Mecical { get; set; }

        /// <summary>
        /// 手术
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// 接生
        /// </summary>
        public string Accouche { get; set; }

        /// <summary>
        /// 检验
        /// </summary>
        public string Ris { get; set; }

        /// <summary>
        /// 麻醉费
        /// </summary>
        public string Anaesthesia { get; set; }

        /// <summary>
        /// 婴儿费
        /// </summary>
        public string Baby { get; set; }

        /// <summary>
        /// 陪床费
        /// </summary>
        public string FollwBed { get; set; }

        /// <summary>
        /// 其他1
        /// </summary>
        public string Others1 { get; set; }

        /// <summary>
        /// 其他2
        /// </summary>
        public string Others2 { get; set; }

        /// <summary>
        /// 其他3
        /// </summary>
        public string Others3 { get; set; }

        /// <summary>
        /// 尸检
        /// </summary>
        public string Ashes_Check { get; set; }

        /// <summary>
        /// 手术、治疗、检查、诊断为本院第一例
        /// </summary>
        public string IsFirstCase { get; set; }

        /// <summary>
        /// 随诊
        /// </summary>
        public string IsFollowing { get; set; }

        /// <summary>
        /// 随诊时间 日
        /// </summary>
        public string IsFollowingDay { get; set; }

        /// <summary>
        /// 随诊时间 月
        /// </summary>
        public string IsFollowingMon { get; set; }

        /// <summary>
        /// 随诊时间 年
        /// </summary>
        public string IsFollowingYear { get; set; }

        /// <summary>
        /// 示教病例
        /// </summary>
        public string IsTeachingCase { get; set; }

        /// <summary>
        /// 血型
        /// </summary>
        public string BloodType { get; set; }

        /// <summary>
        /// Rh
        /// </summary>
        public string Rh { get; set; }

        /// <summary>
        /// 输血反应
        /// </summary>
        public string BloodReaction { get; set; }

        /// <summary>
        /// 红细胞
        /// </summary>
        public string Rbc { get; set; }

        /// <summary>
        /// 血小板
        /// </summary>
        public string Plt { get; set; }

        /// <summary>
        /// 血浆
        /// </summary>
        public string Plasma { get; set; }

        /// <summary>
        /// 全血
        /// </summary>
        public string Wb { get; set; }

        /// <summary>
        /// 输血其他
        /// </summary>
        public string Others { get; set; }

    }
    #endregion

    #region 打印病案首页 产科产妇婴儿情况
    /// <summary>
    /// 打印病案首页 产科产妇婴儿情况
    /// </summary>
    public class Print_Iem_MainPage_ObstetricsBaby
    {
        public string IEM_MainPage_NO
        {
            get
            {
                return this._IEM_MainPage_NO;
            }
            set
            {
                _IEM_MainPage_NO = value;
            }
        }
        private string _IEM_MainPage_NO;

        /// <summary>
        /// 
        /// </summary>
        public string IEM_MainPage_ObstetricsBabyID { get; set; }

        /// <summary>
        /// 胎次
        /// </summary>
        public string TC { get; set; }

        /// <summary>
        /// 产次
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        /// 胎别
        /// </summary>
        public string TB { get; set; }

        /// <summary>
        /// 产妇会阴破裂度
        /// </summary>
        public string CFHYPLD { get; set; }

        /// <summary>
        /// 接产者
        /// </summary>
        public string Midwifery { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 阿帕加评加
        /// </summary>
        public string APJ { get; set; }

        /// <summary>
        /// 身长
        /// </summary>
        public string Heigh { get; set; }

        /// <summary>
        /// 体重
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 产出情况
        /// </summary>
        public string CCQK { get; set; }

        /// <summary>
        /// 出生时间
        /// </summary>
        public string BithDay
        {
            get
            {
                if (v_BithDay == "")
                    return "";
                else
                    return Convert.ToDateTime(v_BithDay).ToString("yyyy年MM月dd日 HH时");
            }
            set { v_BithDay = value; }
        }

        private string v_BithDay;

        /// <summary>
        /// 分娩方式
        /// </summary>
        public string FMFS { get; set; }

        /// <summary>
        /// 出院情况
        /// </summary>
        public string CYQK { get; set; }
    }
    #endregion
}
