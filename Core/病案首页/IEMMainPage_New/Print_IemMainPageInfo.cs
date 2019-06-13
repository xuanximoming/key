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
        private Iem_Mainpage_Diagnosis m_IemDiagInfo = new Iem_Mainpage_Diagnosis();
        private Iem_MainPage_Operation m_IemOperInfo = new Iem_MainPage_Operation();
        private Iem_MainPage_Fee m_IemFeeInfo = new Iem_MainPage_Fee();
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
        public Iem_Mainpage_Diagnosis IemDiagInfo
        {
            get { return m_IemDiagInfo; }
            set { m_IemDiagInfo = value; }
        }


        /// <summary>
        /// 病案首页手术信息
        /// </summary>
        public Iem_MainPage_Operation IemOperInfo
        {
            get { return m_IemOperInfo; }
            set { m_IemOperInfo = value; }
        }

        /// <summary>
        /// 病案首页费用信息
        /// </summary>
        public Iem_MainPage_Fee IemFeeInfo
        {
            get { return m_IemFeeInfo; }
            set { m_IemFeeInfo = value; }
        }

        /// <summary>
        /// 妇科婴儿产妇情况
        /// </summary>
        public Iem_MainPage_ObstetricsBaby IemObstetricsBaby
        {
            get { return m_IemObstetricsBaby; }
            set { m_IemObstetricsBaby = value; }
        }

    }

    #region 打印病案首页诊断基础信息板块
    /// <summary>
    /// 打印病案首页诊断基础信息板块
    /// </summary>
    public class Iem_Mainpage_Basicinfo
    {
        /// <summary>
        /// 病案首页序号
        /// </summary>
        public string Iem_Mainpage_NO { get; set; }

        /// <summary>
        /// 病人首页序号
        /// </summary>
        public string NoOfInpat { get; set; }

        /// <summary>
        /// 社保卡号
        /// </summary>
        public string SocialCare { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 医疗付款方式ID
        /// </summary>
        public string PayID { get; set; }

        private string _PayName;
        /// <summary>
        /// 医疗付款方式Name
        /// </summary>
        public string PayName
        {
            get { return _PayName; }
            set { _PayName = value; }
        }

        private string _InCount;
        /// <summary>
        /// 入院次数
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
        /// 出生--打印时候显示
        /// </summary>
        public string BirthPrint
        {
            get
            {
                if (_Birth == "")
                    return "";
                else
                    return Convert.ToDateTime(_Birth).ToString("yyyy年MM月dd日");
            }
        }
        private string _Birth;

        /// <summary>
        /// 出生
        /// </summary>
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
        /// 职业
        /// </summary>
        public string JobID { get; set; }

        /// <summary>
        /// 出生地市县
        /// </summary>
        public string ProvinceID { get; set; }

        /// <summary>
        /// 出生地市县
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 出生地地区
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
        /// 出生地地区ID
        /// </summary>
        public string CountyID { get; set; }

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
        /// 民族ID
        /// </summary>
        public string NationID { get; set; }

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
        /// 国籍ID
        /// </summary>
        public string NationalityID { get; set; }

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
        /// 工作单位
        /// </summary>
        public string Organization { get; set; }

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
        public string RelationshipName { get; set; }

        /// <summary>
        /// 与联系人关系ID
        /// </summary>
        public string RelationshipID { get; set; }

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
        /// 入院时间----打印时候显示
        /// </summary>
        public string AdmitDatePrint
        {
            get
            {
                if (_AdmitDate == "")
                    return "";
                else
                    return Convert.ToDateTime(_AdmitDate).ToString("yyyy年MM月dd日 HH时");
            }
        }
        private string _AdmitDate;

        /// <summary>
        /// 入院时间
        /// </summary>
        public string AdmitDate { get { return _AdmitDate; } set { _AdmitDate = value; } }

        /// <summary>
        /// 入院科室Name
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
        /// 入院科室
        /// </summary>
        public string AdmitDeptID { get; set; }

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
        /// 入院病区
        /// </summary>
        public string AdmitWardID { get; set; }

        /// <summary>
        /// 转院科别
        /// </summary>
        public string Trans_AdmitDeptName
        {
            get
            {
                return _Trans_AdmitDeptName;
            }
            set
            {
                _Trans_AdmitDeptName = value;
            }
        }
        private string _Trans_AdmitDeptName;

        /// <summary>
        /// 转院科别
        /// </summary>
        public string Trans_AdmitDeptID { get; set; }

        /// <summary>
        /// 出院时间
        /// </summary>
        public string OutWardDatePrint
        {
            get
            {
                if (_OutWardDate == "")
                    return "";
                else
                    return Convert.ToDateTime(_OutWardDate).ToString("yyyy年MM月dd日 HH时");
            }
        }
        private string _OutWardDate;

        /// <summary>
        /// 出院时间
        /// </summary>
        public string OutWardDate { get { return _OutWardDate; } set { _OutWardDate = value; } }

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
        /// 出院科室
        /// </summary>
        public string OutHosDeptID { get; set; }

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
        /// 出院病区
        /// </summary>
        public string OutHosWardID { get; set; }

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


        /// <summary>
        /// 术前住院天数
        /// </summary>
        public string Days_Before { get; set; }

        /// <summary>
        /// 转入病区时间
        /// </summary>
        public string Trans_Date { get; set; }

        /// <summary>
        /// 再转科室
        /// </summary>
        public string Trans_AdmitDept_Again { get; set; }

        /// <summary>
        /// -再转科室(Department.ID)     
        /// </summary>
        public string Trans_AdmitWard { get; set; }

        /// <summary>
        /// 死亡时间
        /// </summary>
        public string Death_Time { get; set; }

        /// <summary>
        /// 死亡原因
        /// </summary>
        public string Death_Reason { get; set; }

        /// <summary>
        /// --完成否 y/n  
        /// </summary>
        public string Is_Completed { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public string completed_time { get; set; }

        /// <summary>
        /// 作废否 1/0
        /// </summary>
        public string Valide { get; set; }

        /// <summary>
        /// 创建此记录者
        /// </summary>
        public string Create_User { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string Create_Time { get; set; }


        /// <summary>
        /// 修改此记录者
        /// </summary>
        public string Modified_User { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string Modified_Time { get; set; }

        /// <summary>
        /// x线检查号
        /// </summary>
        public string Xay_Sn { get; set; }

        /// <summary>
        /// CT检查号
        /// </summary>
        public string Ct_Sn { get; set; }

        /// <summary>
        /// mri检查号  
        /// </summary>
        public string Mri_Sn { get; set; }

        /// <summary>
        /// Dsa检查号
        /// </summary>
        public string Dsa_Sn { get; set; }

        /// <summary>
        /// 尸检主要诊断名称
        /// </summary>
        public string Ashes_Diagnosis_Name { get; set; }

        /// <summary>
        /// 尸体解剖号
        /// </summary>
        public string Ashes_Anatomise_Sn { get; set; }

    }
    #endregion

    #region 打印病案首页 诊断板块信息
    /// <summary>
    /// 打印病案首页 诊断板块信息
    /// </summary>
    public class Iem_Mainpage_Diagnosis
    {
        /// <summary>
        /// 门急诊诊断
        /// </summary>
        public string OutDiagID { get; set; }

        /// <summary>
        /// 门急诊诊断
        /// </summary>
        public string OutDiagName { get; set; }

        /// <summary>
        /// 入院状态：危急一般
        /// </summary>
        public string AdmitInfo { get; set; }

        /// <summary>
        /// 入院诊断
        /// </summary>
        public string InDiagName { get; set; }

        /// <summary>
        /// 入院诊断
        /// </summary>
        public string InDiagID { get; set; }

        /// <summary>
        /// 入院确诊时间--打印显示
        /// </summary>
        public string In_Check_DatePrint
        {
            get
            {
                if (_In_Check_Date == "")
                    return "";
                else
                    return Convert.ToDateTime(_In_Check_Date).ToString("yyyy年MM月dd日");
            }
        }
        private string _In_Check_Date;

        /// <summary>
        /// 入院确诊时间
        /// </summary>
        public string In_Check_Date { get { return _In_Check_Date; } set { _In_Check_Date = value; } }

        /// <summary>
        /// 出院诊断表  包括字段Diagnosis_Name   Status_Id  Diagnosis_Code
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
                    DataColumn dcDiagnosis_Type_Id = new DataColumn("Diagnosis_Type_Id", Type.GetType("System.String"));
                    DataColumn dcOrder_Value = new DataColumn("Order_Value", Type.GetType("System.String"));

                    _OutDiagTable.Columns.Add(dcDiagnosis_Name);
                    _OutDiagTable.Columns.Add(dcStatus_Id);
                    _OutDiagTable.Columns.Add(dcStatus_Name);
                    _OutDiagTable.Columns.Add(dcDiagnosis_Code);
                    _OutDiagTable.Columns.Add(dcDiagnosis_Type_Id);
                    _OutDiagTable.Columns.Add(dcOrder_Value);

                }
                return _OutDiagTable;
            }
            set { _OutDiagTable = value; }
        }

        private DataTable _OutDiagTable;

        /// <summary>
        /// 医院传染病名称
        /// </summary>
        public string ZymosisName { get; set; }

        /// <summary>
        /// 医院传染病ICD-10
        /// </summary>
        public string ZymosisID { get; set; }

        /// <summary>
        /// 医院传染病状态
        /// </summary>
        public string ZymosisState { get; set; }

        /// <summary>
        /// 病理诊断名称
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
        /// 病理诊断SN
        /// </summary>
        public string Pathology_Observation_Sn { get; set; }


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
        public string Section_DirectorName { get; set; }


        /// <summary>
        /// 科主任
        /// </summary>
        public string Section_DirectorID { get; set; }

        /// <summary>
        /// 主（副主）任医师
        /// </summary>
        public string DirectorName { get; set; }

        /// <summary>
        /// 主（副主）任医师
        /// </summary>
        public string DirectorID { get; set; }

        /// <summary>
        /// 主治医师
        /// </summary>
        public string Vs_EmployeeID { get; set; }

        /// <summary>
        /// 主治医师
        /// </summary>
        public string Vs_EmployeeName { get; set; }

        /// <summary>
        /// 住院医师
        /// </summary>
        public string Resident_EmployeeID { get; set; }

        /// <summary>
        /// 住院医师
        /// </summary>
        public string Resident_EmployeeName { get; set; }

        /// <summary>
        /// 进修医师
        /// </summary>
        public string Refresh_EmployeeID { get; set; }

        /// <summary>
        /// 进修医师
        /// </summary>
        public string Refresh_EmployeeName { get; set; }

        /// <summary>
        /// 研究生实习医师
        /// </summary>
        public string Master_InterneName { get; set; }

        /// <summary>
        /// 研究生实习医师
        /// </summary>
        public string Master_InterneID { get; set; }

        /// <summary>
        /// 实习医师
        /// </summary>
        public string InterneName { get; set; }

        /// <summary>
        /// 实习医师
        /// </summary>
        public string InterneID { get; set; }

        /// <summary>
        /// 编码员
        /// </summary>
        public string Coding_UserName { get; set; }

        /// <summary>
        /// 编码员
        /// </summary>
        public string Coding_UserID { get; set; }

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
        public string Quality_Control_DoctorID { get; set; }

        /// <summary>
        /// 质控医师
        /// </summary>
        public string Quality_Control_DoctorName { get; set; }

        /// <summary>
        /// 质控护士
        /// </summary>
        public string Quality_Control_NurseID { get; set; }

        /// <summary>
        /// 质控护士
        /// </summary>
        public string Quality_Control_NurseName { get; set; }

        /// <summary>
        /// 日期：
        /// </summary>
        public string Quality_Control_DatePrint
        {
            get
            {
                if (_Quality_Control_Date == "")
                    return "";
                else
                    return Convert.ToDateTime(_Quality_Control_Date).ToString("yyyy年MM月dd日");
            }
        }
        private string _Quality_Control_Date;

        /// <summary>
        /// 日期：
        /// </summary>
        public string Quality_Control_Date { get { return _Quality_Control_Date; } set { _Quality_Control_Date = value; } }

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
    public class Iem_MainPage_Operation
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


    }
    #endregion

    #region 打印病案首页 费用信息板块
    /// <summary>
    /// 打印病案首页 费用信息板块
    /// </summary>
    public class Iem_MainPage_Fee
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
        public string Total
        {
            get
            {
                return this._Total;
            }
            set
            {
                _Total = value;
            }
        }
        private string _Total ="-";

        /// <summary>
        /// 床费
        /// </summary>
        public string Bed 
        {
            get
            {
                return this._Bed;
            }
            set
            {
                _Bed = value;
            }
        }
        private string _Bed = "-";

        /// <summary>
        /// 护理费用
        /// </summary>
        public string Care
        {
            get
            {
                return this._Care;
            }
            set
            {
                _Care = value;
            }
        }
        private string _Care = "-";

        /// <summary>
        /// 西药
        /// </summary>
        public string WMedical
        {
            get
            {
                return this._WMedical;
            }
            set
            {
                _WMedical = value;
            }
        }
        private string _WMedical = "-";

        /// <summary>
        ///中成药
        /// </summary>
        public string CPMedical
        {
            get
            {
                return this._CPMedical;
            }
            set
            {
                _CPMedical = value;
            }
        }
        private string _CPMedical = "-";

        /// <summary>
        /// 中草药
        /// </summary>
        public string CMedical
        {
            get
            {
                return this._CMedical;
            }
            set
            {
                _CMedical = value;
            }
        }
        private string _CMedical = "-";

        /// <summary>
        /// 放射
        /// </summary>
        public string Radiate
        {
            get
            {
                return this._Radiate;
            }
            set
            {
                _Radiate = value;
            }
        }
        private string _Radiate = "-";

        /// <summary>
        /// 化验
        /// </summary>
        public string Assay
        {
            get
            {
                return this._Assay;
            }
            set
            {
                _Assay = value;
            }
        }
        private string _Assay = "-";

        /// <summary>
        /// 输氧
        /// </summary>
        public string Ox
        {
            get
            {
                return this._Ox;
            }
            set
            {
                _Ox = value;
            }
        }
        private string _Ox = "-";

        /// <summary>
        /// 输血
        /// </summary>
        public string Blood
        {
            get
            {
                return this._Blood;
            }
            set
            {
                _Blood = value;
            }
        }
        private string _Blood = "-";

        /// <summary>
        /// 诊疗
        /// </summary>
        public string Mecical
        {
            get
            {
                return this._Mecical;
            }
            set
            {
                _Mecical = value;
            }
        }
        private string _Mecical = "-";

        /// <summary>
        /// 手术
        /// </summary>
        public string Operation
        {
            get
            {
                return this._Operation;
            }
            set
            {
                _Operation = value;
            }
        }
        private string _Operation = "-";

        /// <summary>
        /// 接生
        /// </summary>
        public string Accouche
        {
            get
            {
                return this._Accouche;
            }
            set
            {
                _Accouche = value;
            }
        }
        private string _Accouche = "-";

        /// <summary>
        /// 检验
        /// </summary>
        public string Ris
        {
            get
            {
                return this._Ris;
            }
            set
            {
                _Ris = value;
            }
        }
        private string _Ris = "-";

        /// <summary>
        /// 麻醉费
        /// </summary>
        public string Anaesthesia
        {
            get
            {
                return this._Anaesthesia;
            }
            set
            {
                _Anaesthesia = value;
            }
        }
        private string _Anaesthesia = "-";

        /// <summary>
        /// 婴儿费
        /// </summary>
        public string Baby
        {
            get
            {
                return this._Baby;
            }
            set
            {
                _Baby = value;
            }
        }
        private string _Baby = "-";

        /// <summary>
        /// 陪床费
        /// </summary>
        public string FollwBed
        {
            get
            {
                return this._FollwBed;
            }
            set
            {
                _FollwBed = value;
            }
        }
        private string _FollwBed = "-";

        /// <summary>
        /// 其他1
        /// </summary>
        public string Others1
        {
            get
            {
                return this._Others1;
            }
            set
            {
                _Others1 = value;
            }
        }
        private string _Others1 = "-";

        /// <summary>
        /// 其他2
        /// </summary>
        public string Others2
        {
            get
            {
                return this._Others2;
            }
            set
            {
                _Others2 = value;
            }
        }
        private string _Others2 = "-";

        /// <summary>
        /// 其他3
        /// </summary>
        public string Others3
        {
            get
            {
                return this._Others3;
            }
            set
            {
                _Others3 = value;
            }
        }
        private string _Others3 = "-";

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
        public string IsFollowingDay 
        { 
            get 
            {
                if (_Following_Ending_Date == "")
                    return "";
                string[] s= _Following_Ending_Date.Split('-');
                return s[0]; 
            } 
        }

        /// <summary>
        /// 随诊时间 月
        /// </summary>
        public string IsFollowingMon
        {
            get
            {
                if (_Following_Ending_Date == "")
                    return "";
                string[] s = _Following_Ending_Date.Split('-');
                return s[1];
            }
        }

        /// <summary>
        /// 随诊时间 年
        /// </summary>
        public string IsFollowingYear
        {
            get
            {
                if (_Following_Ending_Date == "")
                    return "";
                string[] s = _Following_Ending_Date.Split('-');
                return s[2];
            }
        }

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

        /// <summary>
        /// 随诊期限   格式：天-月-年
        /// </summary>
        public string Following_Ending_Date { get { return _Following_Ending_Date; } set { _Following_Ending_Date = value; } }
        private string _Following_Ending_Date;

    }
    #endregion

    #region 打印病案首页 产科产妇婴儿情况
    /// <summary>
    /// 打印病案首页 产科产妇婴儿情况
    /// </summary>
    public class Iem_MainPage_ObstetricsBaby
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
        public string BithDayPrint
        {
            get
            {
                if (v_BithDay == "")
                    return "";
                else
                    return Convert.ToDateTime(v_BithDay).ToString("yyyy年MM月dd日 HH时");
            }
        }

        private string v_BithDay;

        /// <summary>
        /// 出生时间
        /// </summary>
        public string BithDay { get { return v_BithDay; } set { v_BithDay = value; } }

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
    #endregion
}
