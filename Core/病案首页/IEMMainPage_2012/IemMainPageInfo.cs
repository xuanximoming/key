using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YidanSoft.Core.IEMMainPage
{
    public class IemMainPageInfo
    {
        private Iem_Mainpage_Basicinfo m_IemBasicInfo = new Iem_Mainpage_Basicinfo();
        private List<Iem_Mainpage_Diagnosis> m_IemDiagInfo = new List<Iem_Mainpage_Diagnosis>();
        private List<Iem_MainPage_Operation> m_IemOperInfo = new List<Iem_MainPage_Operation>();
        private Iem_MainPage_ObstetricsBaby m_IemObstetricsBaby = new Iem_MainPage_ObstetricsBaby();

        /// <summary>
        /// 病案首页基本信息
        /// </summary>
        public Iem_Mainpage_Basicinfo IemBasicInfo
        {
            get { return m_IemBasicInfo; }
            set { m_IemBasicInfo = value; }
        }

        /// <summary>
        /// 病案首页诊断信息
        /// </summary>
        public List<Iem_Mainpage_Diagnosis> IemDiagInfo
        {
            get { return m_IemDiagInfo; }
            set { m_IemDiagInfo = value; }
        }


        /// <summary>
        /// 病案首页手术信息
        /// </summary>
        public List<Iem_MainPage_Operation> IemOperInfo
        {
            get { return m_IemOperInfo; }
            set { m_IemOperInfo = value; }
        }

        /// <summary>
        /// 妇科婴儿产妇情况
        /// </summary>
        public Iem_MainPage_ObstetricsBaby IemObstetricsBaby
        {
            get { return m_IemObstetricsBaby; }
            set { m_IemObstetricsBaby = value; }
        }

        /// <summary>
        /// 病案首页出院诊断表
        /// </summary>
        public DataTable OutDiagTable
        {
            get
            {
                if (_OutDiagTable == null)
                {
                    _OutDiagTable = new DataTable();
                    DataColumn dcDiagnosis_Name = new DataColumn("Diagnosis_Name", Type.GetType("System.String"));
                    DataColumn dcStatus_Id = new DataColumn("Status_Id", Type.GetType("System.String"));
                    DataColumn dcStatus_Name = new DataColumn("Status_Name", Type.GetType("System.String"));
                    DataColumn dcDiagnosis_Code = new DataColumn("Diagnosis_Code", Type.GetType("System.String"));

                    _OutDiagTable.Columns.Add(dcDiagnosis_Name);
                    _OutDiagTable.Columns.Add(dcStatus_Id);
                    _OutDiagTable.Columns.Add(dcStatus_Name);
                    _OutDiagTable.Columns.Add(dcDiagnosis_Code);

                }
                return _OutDiagTable;
            }
            set { _OutDiagTable = value; }
        }

        private DataTable _OutDiagTable;

        /// <summary>
        /// 病案首页中手术信息
        /// </summary>
        public DataTable OperationTable { get; set; }
    }

    public class Iem_Mainpage_Basicinfo
    {
        /// <summary>
        /// 创建新的 Iem_Mainpage_Basicinfo 对象。
        /// </summary>
        /// <param name="iem_Mainpage_NO">Iem_Mainpage_NO 的初始值。</param>
        /// <param name="patNoOfHis">PatNoOfHis 的初始值。</param>
        /// <param name="noOfInpat">NoOfInpat 的初始值。</param>
        /// <param name="inCount">InCount 的初始值。</param>
        /// <param name="name">Name 的初始值。</param>
        /// <param name="sexID">SexID 的初始值。</param>
        /// <param name="birth">Birth 的初始值。</param>
        /// <param name="valide">Valide 的初始值。</param>
        /// <param name="create_User">Create_User 的初始值。</param>
        /// <param name="create_Time">Create_Time 的初始值。</param>
        public static Iem_Mainpage_Basicinfo CreateIem_Mainpage_Basicinfo(decimal iem_Mainpage_NO, string patNoOfHis, decimal noOfInpat, int inCount, string name, string sexID, string birth, decimal valide, string create_User, string create_Time)
        {
            Iem_Mainpage_Basicinfo iem_Mainpage_Basicinfo = new Iem_Mainpage_Basicinfo();
            iem_Mainpage_Basicinfo.Iem_Mainpage_NO = iem_Mainpage_NO;
            iem_Mainpage_Basicinfo.PatNoOfHis = patNoOfHis;
            iem_Mainpage_Basicinfo.NoOfInpat = noOfInpat;
            iem_Mainpage_Basicinfo.InCount = inCount;
            iem_Mainpage_Basicinfo.Name = name;
            iem_Mainpage_Basicinfo.SexID = sexID;
            iem_Mainpage_Basicinfo.Birth = birth;
            iem_Mainpage_Basicinfo.Valide = valide;
            iem_Mainpage_Basicinfo.Create_User = create_User;
            iem_Mainpage_Basicinfo.Create_Time = create_Time;
            return iem_Mainpage_Basicinfo;
        }

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

        public int InCount
        {
            get
            {
                return _InCount;
            }
            set
            {
                _InCount = value;
            }
        }
        private int _InCount;

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

        public string Birth
        {
            get
            {
                return _Birth;
            }
            set
            {
                _Birth = value;
            }
        }
        private string _Birth;

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

        public string CountyID
        {
            get
            {
                return _CountyID;
            }
            set
            {
                _CountyID = value;
            }
        }
        private string _CountyID;

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

        public string NationalityID
        {
            get
            {
                return _NationalityID;
            }
            set
            {
                _NationalityID = value;
            }
        }
        private string _NationalityID;

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

        public string AdmitDate
        {
            get
            {
                return _AdmitDate;
            }
            set
            {
                _AdmitDate = value;
            }
        }
        private string _AdmitDate;

        public string AdmitDept
        {
            get
            {
                return _AdmitDept;
            }
            set
            {
                _AdmitDept = value;
            }
        }
        private string _AdmitDept;

        public string AdmitWard
        {
            get
            {
                return _AdmitWard;
            }
            set
            {
                _AdmitWard = value;
            }
        }
        private string _AdmitWard;

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

        public string OutWardDate
        {
            get
            {
                return this._OutWardDate;
            }
            set
            {
                _OutWardDate = value;
            }
        }
        private string _OutWardDate;

        public string OutHosDept
        {
            get
            {
                return this._OutHosDept;
            }
            set
            {
                _OutHosDept = value;
            }
        }
        private string _OutHosDept;

        public string OutHosWard
        {
            get
            {
                return this._OutHosWard;
            }
            set
            {
                _OutHosWard = value;
            }
        }
        private string _OutHosWard;

        public global::System.Nullable<decimal> Actual_Days
        {
            get
            {
                return this._Actual_Days;
            }
            set
            {
                _Actual_Days = value;
            }
        }
        private global::System.Nullable<decimal> _Actual_Days;

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

        public global::System.Nullable<decimal> Hbsag
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
        private global::System.Nullable<decimal> _Hbsag;

        public global::System.Nullable<decimal> Hcv_Ab
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
        private global::System.Nullable<decimal> _Hcv_Ab;

        public global::System.Nullable<decimal> Hiv_Ab
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
        private global::System.Nullable<decimal> _Hiv_Ab;

        public global::System.Nullable<decimal> Opd_Ipd_Id
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
        private global::System.Nullable<decimal> _Opd_Ipd_Id;

        public global::System.Nullable<decimal> In_Out_Inpatinet_Id
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
        private global::System.Nullable<decimal> _In_Out_Inpatinet_Id;

        public global::System.Nullable<decimal> Before_After_Or_Id
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
        private global::System.Nullable<decimal> _Before_After_Or_Id;

        public global::System.Nullable<decimal> Clinical_Pathology_Id
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
        private global::System.Nullable<decimal> _Clinical_Pathology_Id;

        public global::System.Nullable<decimal> Pacs_Pathology_Id
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
        private global::System.Nullable<decimal> _Pacs_Pathology_Id;

        public global::System.Nullable<decimal> Save_Times
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
        private global::System.Nullable<decimal> _Save_Times;

        public global::System.Nullable<decimal> Success_Times
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
        private global::System.Nullable<decimal> _Success_Times;

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

        public global::System.Nullable<decimal> Medical_Quality_Id
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
        private global::System.Nullable<decimal> _Medical_Quality_Id;

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

        public string Quality_Control_Date
        {
            get
            {
                return this._Quality_Control_Date;
            }
            set
            {
                _Quality_Control_Date = value;
            }
        }
        private string _Quality_Control_Date;

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

        /// <summary>
        /// 医院传染病
        /// </summary>
        public string Zymosis { get; set; }

        /// <summary>
        /// 损伤中毒因素
        /// </summary>
        public string Hurt_Toxicosis_Ele { get; set; }

        /// <summary>
        /// 医院传染病状态
        /// </summary>
        public string ZymosisState { get; set; }
    }

    public class Iem_Mainpage_Diagnosis
    {
        /// <summary>
        /// 创建新的 Iem_Mainpage_Diagnosis 对象。
        /// </summary>
        /// <param name="iem_Mainpage_Diagnosis_NO">Iem_Mainpage_Diagnosis_NO 的初始值。</param>
        /// <param name="iem_Mainpage_NO">Iem_Mainpage_NO 的初始值。</param>
        /// <param name="diagnosis_Type_Id">Diagnosis_Type_Id 的初始值。</param>
        /// <param name="diagnosis_Name">Diagnosis_Name 的初始值。</param>
        /// <param name="order_Value">Order_Value 的初始值。</param>
        /// <param name="valide">Valide 的初始值。</param>
        /// <param name="create_User">Create_User 的初始值。</param>
        /// <param name="create_Time">Create_Time 的初始值。</param>
        public static Iem_Mainpage_Diagnosis CreateIem_Mainpage_Diagnosis(decimal iem_Mainpage_Diagnosis_NO, decimal iem_Mainpage_NO, decimal diagnosis_Type_Id, string diagnosis_Name, decimal order_Value, decimal valide, string create_User, string create_Time)
        {
            Iem_Mainpage_Diagnosis iem_Mainpage_Diagnosis = new Iem_Mainpage_Diagnosis();
            iem_Mainpage_Diagnosis.Iem_Mainpage_Diagnosis_NO = iem_Mainpage_Diagnosis_NO;
            iem_Mainpage_Diagnosis.Iem_Mainpage_NO = iem_Mainpage_NO;
            iem_Mainpage_Diagnosis.Diagnosis_Type_Id = diagnosis_Type_Id;
            iem_Mainpage_Diagnosis.Diagnosis_Name = diagnosis_Name;
            iem_Mainpage_Diagnosis.Order_Value = order_Value;
            iem_Mainpage_Diagnosis.Valide = valide;
            iem_Mainpage_Diagnosis.Create_User = create_User;
            iem_Mainpage_Diagnosis.Create_Time = create_Time;
            return iem_Mainpage_Diagnosis;
        }

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

        public decimal Diagnosis_Type_Id
        {
            get
            {
                return _Diagnosis_Type_Id;
            }
            set
            {
                _Diagnosis_Type_Id = value;
            }
        }
        private decimal _Diagnosis_Type_Id;

        public string Diagnosis_Code
        {
            get
            {
                return _Diagnosis_Code;
            }
            set
            {
                _Diagnosis_Code = value;
            }
        }
        private string _Diagnosis_Code;

        public string Diagnosis_Name
        {
            get
            {
                return _Diagnosis_Name;
            }
            set
            {
                _Diagnosis_Name = value;
            }
        }
        private string _Diagnosis_Name;

        public global::System.Nullable<decimal> Status_Id
        {
            get
            {
                return _Status_Id;
            }
            set
            {
                _Status_Id = value;
            }
        }
        private global::System.Nullable<decimal> _Status_Id;

        public decimal Order_Value
        {
            get
            {
                return _Order_Value;
            }
            set
            {
                _Order_Value = value;
            }
        }
        private decimal _Order_Value;

        public decimal Valide
        {
            get
            {
                return _Valide;
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
                return _Create_User;
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
                return _Create_Time;
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
                return _Cancel_User;
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
                return _Cancel_Time;
            }
            set
            {
                _Cancel_Time = value;
            }
        }
        private string _Cancel_Time;


    }

    public class Iem_MainPage_Operation
    {
        /// <summary>
        /// 创建新的 Iem_MainPage_Operation 对象。
        /// </summary>
        /// <param name="iem_MainPage_Operation_NO">Iem_MainPage_Operation_NO 的初始值。</param>
        /// <param name="iEM_MainPage_NO">IEM_MainPage_NO 的初始值。</param>
        /// <param name="operation_Code">Operation_Code 的初始值。</param>
        /// <param name="valide">Valide 的初始值。</param>
        /// <param name="create_User">Create_User 的初始值。</param>
        /// <param name="create_Time">Create_Time 的初始值。</param>
        public static Iem_MainPage_Operation CreateIem_MainPage_Operation(decimal iem_MainPage_Operation_NO, decimal iEM_MainPage_NO, string operation_Code, decimal valide, string create_User, string create_Time)
        {
            Iem_MainPage_Operation iem_MainPage_Operation = new Iem_MainPage_Operation();
            iem_MainPage_Operation.Iem_MainPage_Operation_NO = iem_MainPage_Operation_NO;
            iem_MainPage_Operation.IEM_MainPage_NO = iEM_MainPage_NO;
            iem_MainPage_Operation.Operation_Code = operation_Code;
            iem_MainPage_Operation.Valide = valide;
            iem_MainPage_Operation.Create_User = create_User;
            iem_MainPage_Operation.Create_Time = create_Time;
            return iem_MainPage_Operation;
        }

        public decimal Iem_MainPage_Operation_NO
        {
            get
            {
                return this._Iem_MainPage_Operation_NO;
            }
            set
            {
                _Iem_MainPage_Operation_NO = value;
            }
        }
        private decimal _Iem_MainPage_Operation_NO;

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

    /// <summary>
    /// 病案首页 产科产妇婴儿情况
    /// </summary>
    public class Iem_MainPage_ObstetricsBaby
    {
        public string IEM_MainPage_NO { get; set; }

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
        public string BithDay { get; set; }

        /// <summary>
        /// 分娩方式
        /// </summary>
        public string FMFS { get; set; }

        /// <summary>
        /// 出院情况
        /// </summary>
        public string CYQK { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Create_User { get; set; }
    }
}
