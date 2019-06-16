using System;
using System.Data;

namespace DrectSoft.Core.IEMMainPage
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

    #region 病案首页诊断基础信息板块
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
        /// 病案号 即his中的病人 住院号
        /// </summary>
        public string NOOFRECORD { get; set; }
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

        #region 出生地

        /// <summary>
        /// 出生地 省
        /// </summary>
        public string CSD_ProvinceID { get; set; }

        /// <summary>
        /// 出生地 市
        /// </summary>
        public string CSD_CityID { get; set; }

        /// <summary>
        /// 出生地 县
        /// </summary>
        public string CSD_DistrictID { get; set; }

        /// <summary>
        /// 出生地 省名称
        /// </summary>
        public string CSD_ProvinceName { get; set; }

        /// <summary>
        /// 出生地 市名称
        /// </summary>
        public string CSD_CityName { get; set; }

        /// <summary>
        /// 出生地 县名称
        /// </summary>
        public string CSD_DistrictName { get; set; }

        #endregion

        #region 现住址
        /// <summary>
        /// 现住址 省
        /// </summary>
        public string XZZ_ProvinceID { get; set; }

        /// <summary>
        /// 现住址 市
        /// </summary>
        public string XZZ_CityID { get; set; }

        /// <summary>
        /// 现住址 县
        /// </summary>
        public string XZZ_DistrictID { get; set; }

        /// <summary>
        /// 现住址 省名称
        /// </summary>
        public string XZZ_ProvinceName { get; set; }

        /// <summary>
        /// 现住址 市名称
        /// </summary>
        public string XZZ_CityName { get; set; }

        /// <summary>
        /// 现住址 县名称
        /// </summary>
        public string XZZ_DistrictName { get; set; }

        /// <summary>
        /// 现住址 电话
        /// </summary>
        public string XZZ_TEL
        {
            get
            {
                return _XZZ_TEL;
            }
            set
            {
                _XZZ_TEL = value;
            }
        }
        private string _XZZ_TEL;

        /// <summary>
        /// 现住址 邮编
        /// </summary>
        public string XZZ_Post
        {
            get
            {
                return _XZZ_Post;
            }
            set
            {
                _XZZ_Post = value;
            }
        }
        private string _XZZ_Post;

        #endregion

        #region 户口地址
        /// <summary>
        /// 户口地址 省
        /// </summary>
        public string HKDZ_ProvinceID { get; set; }

        /// <summary>
        /// 户口地址 市
        /// </summary>
        public string HKDZ_CityID { get; set; }

        /// <summary>
        /// 户口地址 县
        /// </summary>
        public string HKDZ_DistrictID { get; set; }

        /// <summary>
        /// 户口地址 省名称
        /// </summary>
        public string HKDZ_ProvinceName { get; set; }

        /// <summary>
        /// 户口地址 市名称
        /// </summary>
        public string HKDZ_CityName { get; set; }

        /// <summary>
        /// 户口地址 县名称
        /// </summary>
        public string HKDZ_DistrictName { get; set; }

        /// <summary>
        /// 户口所在地邮编
        /// </summary>
        public string HKDZ_Post
        {
            get
            {
                return _HKDZ_Post;
            }
            set
            {
                _HKDZ_Post = value;
            }
        }
        private string _HKDZ_Post;

        #endregion

        /// <summary>
        /// 籍贯 省名称
        /// </summary>
        public string JG_ProvinceName { get; set; }

        /// <summary>
        /// 籍贯 市名称
        /// </summary>
        public string JG_CityName { get; set; }


        /// <summary>
        /// 籍贯 省
        /// </summary>
        public string JG_ProvinceID { get; set; }

        /// <summary>
        /// 籍贯 市
        /// </summary>
        public string JG_CityID { get; set; }

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
        /// 死亡患者尸检 □ 1.是  2.否
        /// </summary>
        public string Autopsy_Flag { get; set; }


        #region 2012国家卫生部表中病案首页新增内容

        /// <summary>
        /// （年龄不足1周岁的） 年龄(月)
        /// </summary>
        public string MonthAge
        {
            get
            {
                return _MonthAge;
            }
            set
            {
                _MonthAge = value;
            }
        }
        private string _MonthAge;

        /// <summary>
        /// 新生儿出生体重(克)
        /// </summary>
        public string Weight
        {
            get
            {
                return _Weight;
            }
            set
            {
                _Weight = value;
            }
        }
        private string _Weight;

        /// <summary>
        /// 新生儿入院体重(克)
        /// </summary>
        public string InWeight
        {
            get
            {
                return _InWeight;
            }
            set
            {
                _InWeight = value;
            }
        }
        private string _InWeight;


        /// <summary>
        /// 入院途径:1.急诊  2.门诊  3.其他医疗机构转入  9.其他 
        /// </summary>
        public string InHosType
        {
            get
            {
                return _TypeHos;
            }
            set
            {
                _TypeHos = value;
            }
        }
        private string _TypeHos;

        /// <summary>
        /// 其他医疗机构转入
        /// </summary>
        public string TypeHos
        {
            get
            {
                return _InHosType;
            }
            set
            {
                _InHosType = value;
            }
        }
        private string _InHosType;

        /// <summary>
        /// 离院方式 □ 1.医嘱离院  2.医嘱转院 3.医嘱转社区卫生服务机构/乡镇卫生院 4.非医嘱离院5.死亡9.其他  OUTHOSTYPE 
        /// </summary>
        public string OutHosType
        {
            get
            {
                return _OutHosType;
            }
            set
            {
                _OutHosType = value;
            }
        }
        private string _OutHosType;

        /// <summary>
        /// 2.医嘱转院，拟接收医疗机构名称：RECEIVEHOSPITAL
        /// </summary>
        public string ReceiveHosPital
        {
            get
            {
                return _ReceiveHosPital;
            }
            set
            {
                _ReceiveHosPital = value;
            }
        }
        private string _ReceiveHosPital;

        /// <summary>
        /// 3.医嘱转社区卫生服务机构/乡镇卫生院，拟接收医疗机构名称：
        /// </summary>
        public string ReceiveHosPital2
        {
            get
            {
                return _ReceiveHosPital2;
            }
            set
            {
                _ReceiveHosPital2 = value;
            }
        }
        private string _ReceiveHosPital2;

        /// <summary>
        /// 是否有出院31天内再住院计划 □ 1.无  2.有AGAININHOSPITAL
        /// </summary>
        public string AgainInHospital
        {
            get
            {
                return _AgainInHospital;
            }
            set
            {
                _AgainInHospital = value;
            }
        }
        private string _AgainInHospital;


        /// <summary>
        /// 出院31天内再住院计划 目的:                                               
        /// </summary>
        public string AgainInHospitalReason
        {
            get
            {
                return _AgainInHospitalReason;
            }
            set
            {
                _AgainInHospitalReason = value;
            }
        }
        private string _AgainInHospitalReason;

        /// <summary>
        /// 颅脑损伤患者昏迷时间： 入院前    天     BEFOREHOSCOMADAY                               
        /// </summary>
        public string BeforeHosComaDay
        {
            get
            {
                return _BeforeHosComaDay;
            }
            set
            {
                _BeforeHosComaDay = value;
            }
        }
        private string _BeforeHosComaDay;

        /// <summary>
        /// 颅脑损伤患者昏迷时间： 入院前    小时                                     
        /// </summary>
        public string BeforeHosComaHour
        {
            get
            {
                return _BeforeHosComaHour;
            }
            set
            {
                _BeforeHosComaHour = value;
            }
        }
        private string _BeforeHosComaHour;

        /// <summary>
        /// 颅脑损伤患者昏迷时间： 入院前    分钟     BEFOREHOSCOMADAY                               
        /// </summary>
        public string BeforeHosComaMinute
        {
            get
            {
                return _BeforeHosComaMinute;
            }
            set
            {
                _BeforeHosComaMinute = value;
            }
        }
        private string _BeforeHosComaMinute;


        /// <summary>
        /// 颅脑损伤患者昏迷时间： 入院后   天     BEFOREHOSCOMADAY                               
        /// </summary>
        public string LaterHosComaDay
        {
            get
            {
                return _LaterHosComaDay;
            }
            set
            {
                _LaterHosComaDay = value;
            }
        }
        private string _LaterHosComaDay;

        /// <summary>
        /// 颅脑损伤患者昏迷时间： 入院后    小时                                     
        /// </summary>
        public string LaterHosComaHour
        {
            get
            {
                return _LaterHosComaHour;
            }
            set
            {
                _LaterHosComaHour = value;
            }
        }
        private string _LaterHosComaHour;

        /// <summary>
        /// 颅脑损伤患者昏迷时间： 入院后    分钟                               
        /// </summary>
        public string LaterHosComaMinute
        {
            get
            {
                return _LaterHosComaMinute;
            }
            set
            {
                _LaterHosComaMinute = value;
            }
        }
        private string _LaterHosComaMinute;

        /// <summary>
        /// 健康卡号                           
        /// </summary>
        public string CardNumber
        {
            get
            {
                return _CardNumber;
            }
            set
            {
                _CardNumber = value;
            }
        }
        private string _CardNumber;

        #endregion

        #region 新增的是否婴儿和母亲的首页序号标识
        /// <summary>
        /// 是否婴儿
        /// </summary>
        public string IsBaby { get; set; }
        /// <summary>
        /// 母亲的Noofinpat
        /// </summary>
        public string Mother { get; set; }
        /// <summary>
        /// 母亲的病历号码
        /// </summary>
        public string MotherPatOfHis { get; set; }
        #endregion

        #region 根据泗县中医院提出的需求修改

        /// <summary>
        /// 治疗类别 □ 1.中医（ 1.1 中医   1.2民族医）    2.中西医     3.西医
        /// </summary>
        public string CURE_TYPE
        {
            get
            {
                return this._CURE_TYPE;
            }
            set
            {
                _CURE_TYPE = value;
            }
        }
        private string _CURE_TYPE;

        /// <summary>
        /// 门（急）诊诊断（中医诊断） 名称
        /// </summary>
        public string MZZYZD_NAME
        {
            get
            {
                return this._MZZYZD_NAME;
            }
            set
            {
                _MZZYZD_NAME = value;
            }
        }
        private string _MZZYZD_NAME;

        /// <summary>
        /// 门（急）诊诊断（中医诊断） 编码
        /// </summary>
        public string MZZYZD_CODE
        {
            get
            {
                return this._MZZYZD_CODE;
            }
            set
            {
                _MZZYZD_CODE = value;
            }
        }
        private string _MZZYZD_CODE;

        /// <summary>
        /// 门（急）诊诊断（西医诊断） 名称
        /// </summary>
        public string MZXYZD_NAME
        {
            get
            {
                return this._MZXYZD_NAME;
            }
            set
            {
                _MZXYZD_NAME = value;
            }
        }
        private string _MZXYZD_NAME;

        /// <summary>
        /// 门（急）诊诊断（西医诊断） 编码
        /// </summary>
        public string MZXYZD_CODE
        {
            get
            {
                return this._MZXYZD_CODE;
            }
            set
            {
                _MZXYZD_CODE = value;
            }
        }
        private string _MZXYZD_CODE;


        /// <summary>
        /// 实施临床路径：□ 1. 中医  2. 西医  3 否 
        /// </summary>
        public string SSLCLJ
        {
            get
            {
                return this._SSLCLJ;
            }
            set
            {
                _SSLCLJ = value;
            }
        }
        private string _SSLCLJ;

        /// <summary>
        /// 使用医疗机构中药制剂：□ 1.是  2. 否 
        /// </summary>
        public string ZYZJ
        {
            get
            {
                return this._ZYZJ;
            }
            set
            {
                _ZYZJ = value;
            }
        }
        private string _ZYZJ;

        /// <summary>
        /// 使用中医诊疗设备：□  1.是 2. 否
        /// </summary>
        public string ZYZLSB
        {
            get
            {
                return this._ZYZLSB;
            }
            set
            {
                _ZYZLSB = value;
            }
        }
        private string _ZYZLSB;

        /// <summary>
        /// 使用中医诊疗技术：□ 1. 是  2. 否
        /// </summary>
        public string ZYZLJS
        {
            get
            {
                return this._ZYZLJS;
            }
            set
            {
                _ZYZLJS = value;
            }
        }
        private string _ZYZLJS;

        /// <summary>
        /// 辨证施护：□ 1.是  2. 否
        /// </summary>
        public string BZSH
        {
            get
            {
                return this._BZSH;
            }
            set
            {
                _BZSH = value;
            }
        }
        private string _BZSH;

        #endregion

    }
    #endregion

    #region 病案首页 诊断板块信息
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
        /// 入院诊断
        /// </summary>
        public string InDiagName { get; set; }

        /// <summary>
        /// 入院诊断
        /// </summary>
        public string InDiagID { get; set; }

        /// <summary>
        /// 出院诊断表  包括字段Diagnosis_Name   Status_Id  Diagnosis_Code
        /// Type     诊断类型 1、中医  2、西医
        /// TypeName 诊断类型名称
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
                    DataColumn dcType = new DataColumn("Type", Type.GetType("System.String"));
                    DataColumn dcTypeName = new DataColumn("TypeName", Type.GetType("System.String"));

                    _OutDiagTable.Columns.Add(dcDiagnosis_Name);
                    _OutDiagTable.Columns.Add(dcStatus_Id);
                    _OutDiagTable.Columns.Add(dcStatus_Name);
                    _OutDiagTable.Columns.Add(dcDiagnosis_Code);
                    _OutDiagTable.Columns.Add(dcDiagnosis_Type_Id);
                    _OutDiagTable.Columns.Add(dcOrder_Value);
                    _OutDiagTable.Columns.Add(dcType);
                    _OutDiagTable.Columns.Add(dcTypeName);

                }
                return _OutDiagTable;
            }
            set { _OutDiagTable = value; }
        }

        private DataTable _OutDiagTable;


        /// <summary>
        /// 病理诊断编号
        /// </summary>
        public string Pathology_Diagnosis_ID
        {
            get
            {
                return this._Pathology_Diagnosis_ID;
            }
            set
            {
                _Pathology_Diagnosis_ID = value;
            }
        }
        private string _Pathology_Diagnosis_ID;

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
        /// 损伤、中毒的外部因素 编号
        /// </summary>
        public string Hurt_Toxicosis_ElementID
        {
            get
            {
                return this._Hurt_Toxicosis_ElementID;
            }
            set
            {
                _Hurt_Toxicosis_ElementID = value;
            }
        }
        private string _Hurt_Toxicosis_ElementID;

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
        /// 药物过敏 □1.无 2.有
        /// </summary>
        public string Allergic_Flag
        {
            get
            {
                return _Allergic_Flag;
            }
            set
            {
                _Allergic_Flag = value;
            }
        }
        private string _Allergic_Flag;

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
        /// 血型
        /// </summary>
        public string BloodType { get; set; }

        /// <summary>
        /// Rh
        /// </summary>
        public string Rh { get; set; }

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
        /// 责任护士
        /// </summary>
        public string Duty_NurseName { get; set; }

        /// <summary>
        /// 责任护士
        /// </summary>
        public string Duty_NurseID { get; set; }

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

    #region 病案首页 手术信息板块
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

    #region 病案首页 费用信息板块
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
                if (_Total == null || _Total.Trim() == "")
                    _Total = "-";
                return this._Total;
            }
            set
            {
                _Total = value;
            }
        }
        private string _Total;

        /// <summary>
        /// 自付金额
        /// </summary>
        public string OwnFee
        {
            get
            {
                if (_OwnFee == null || _OwnFee.Trim() == "")
                    _OwnFee = "-";
                return _OwnFee;
            }
            set
            {
                _OwnFee = value;
            }
        }
        private string _OwnFee;

        /// <summary>
        /// 一般医疗服务费
        /// </summary>
        public string YBYLFY
        {
            get
            {
                if (_YBYLFY == null || _YBYLFY.Trim() == "")
                    _YBYLFY = "-";
                return _YBYLFY;
            }
            set
            {
                _YBYLFY = value;
            }
        }
        private string _YBYLFY;

        /// <summary>
        /// 中医辨证论治费：
        /// </summary>
        public string ZYBZLZF
        {
            get
            {
                if (_ZYBZLZF == null || _ZYBZLZF.Trim() == "")
                    _ZYBZLZF = "-";
                return _ZYBZLZF;
            }
            set
            {
                _ZYBZLZF = value;
            }
        }
        private string _ZYBZLZF;

        /// <summary>
        /// 中医辨证论治会诊费：
        /// </summary>
        public string ZYBZLZHZF
        {
            get
            {
                if (_ZYBZLZHZF == null || _ZYBZLZHZF.Trim() == "")
                    _ZYBZLZHZF = "-";
                return _ZYBZLZHZF;
            }
            set
            {
                _ZYBZLZHZF = value;
            }
        }
        private string _ZYBZLZHZF;

        /// <summary>
        /// 一般治疗操作费
        /// </summary>
        public string YBZLFY
        {
            get
            {
                if (_YBZLFY == null || _YBZLFY.Trim() == "")
                    _YBZLFY = "-";
                return _YBYLFY;
            }
            set
            {
                _YBZLFY = value;
            }
        }
        private string _YBZLFY;

        /// <summary>
        /// 护理费
        /// </summary>
        public string Care
        {
            get
            {
                if (_Care == null || _Care.Trim() == "")
                    _Care = "-";
                return _Care;
            }
            set
            {
                _Care = value;
            }
        }
        private string _Care;

        /// <summary>
        /// 综合类 其他费用
        /// </summary>
        public string ZHQTFY
        {
            get
            {
                if (_ZHQTFY == null || _ZHQTFY.Trim() == "")
                    _ZHQTFY = "-";
                return _ZHQTFY;
            }
            set
            {
                _ZHQTFY = value;
            }
        }
        private string _ZHQTFY;


        /// <summary>
        /// 诊断类 病理诊断费
        /// </summary>
        public string BLZDF
        {
            get
            {
                if (_BLZDF == null || _BLZDF.Trim() == "")
                    _BLZDF = "-";
                return _BLZDF;
            }
            set
            {
                _BLZDF = value;
            }
        }
        private string _BLZDF;

        /// <summary>
        /// 诊断类 实验室诊断费
        /// </summary>
        public string SYSZDF
        {
            get
            {
                if (_SYSZDF == null || _SYSZDF.Trim() == "")
                    _SYSZDF = "-";
                return _SYSZDF;
            }
            set
            {
                _SYSZDF = value;
            }
        }
        private string _SYSZDF;

        /// <summary>
        /// 诊断类 影像学诊断费
        /// </summary>
        public string YXXZDF
        {
            get
            {
                if (_YXXZDF == null || _YXXZDF.Trim() == "")
                    _YXXZDF = "-";
                return _YXXZDF;
            }
            set
            {
                _YXXZDF = value;
            }
        }
        private string _YXXZDF;

        /// <summary>
        /// 诊断类 临床诊断项目费
        /// </summary>
        public string LCZDF
        {
            get
            {
                if (_LCZDF == null || _LCZDF.Trim() == "")
                    _LCZDF = "-";
                return _LCZDF;
            }
            set
            {
                _LCZDF = value;
            }
        }
        private string _LCZDF;

        /// <summary>
        /// 治疗类 非手术治疗项目费
        /// </summary>
        public string FSSZLF
        {
            get
            {
                if (_FSSZLF == null || _FSSZLF.Trim() == "")
                    _FSSZLF = "-";
                return _FSSZLF;
            }
            set
            {
                _FSSZLF = value;
            }
        }
        private string _FSSZLF;

        /// <summary>
        /// 治疗类 临床物理治疗费
        /// </summary>
        public string LCWLZLF
        {
            get
            {
                if (_LCWLZLF == null || _LCWLZLF.Trim() == "")
                    _LCWLZLF = "-";
                return _LCWLZLF;
            }
            set
            {
                _LCWLZLF = value;
            }
        }
        private string _LCWLZLF;

        /// <summary>
        /// 治疗类 手术治疗费
        /// </summary>
        public string SSZLF
        {
            get
            {
                if (_SSZLF == null || _SSZLF.Trim() == "")
                    _SSZLF = "-";
                return _SSZLF;
            }
            set
            {
                _SSZLF = value;
            }
        }
        private string _SSZLF;

        /// <summary>
        /// 治疗类 麻醉费
        /// </summary>
        public string MZF
        {
            get
            {
                if (_MZF == null || _MZF.Trim() == "")
                    _MZF = "-";
                return _MZF;
            }
            set
            {
                _MZF = value;
            }
        }
        private string _MZF;

        /// <summary>
        /// 治疗类 手术费
        /// </summary>
        public string SSF
        {
            get
            {
                if (_SSF == null || _SSF.Trim() == "")
                    _SSF = "-";
                return _SSF;
            }
            set
            {
                _SSF = value;
            }
        }
        private string _SSF;

        /// <summary>
        /// 康复类 康复费
        /// </summary>
        public string KFF
        {
            get
            {
                if (_KFF == null || _KFF.Trim() == "")
                    _KFF = "-";
                return _KFF;
            }
            set
            {
                _KFF = value;
            }
        }
        private string _KFF;

        /// <summary>
        /// 中医类 中医诊断
        /// </summary>
        public string ZYZDF
        {
            get
            {
                if (_ZYZDF == null || _ZYZDF.Trim() == "")
                    _ZYZDF = "-";
                return _ZYZDF;
            }
            set
            {
                _ZYZDF = value;
            }
        }
        private string _ZYZDF;

        /// <summary>
        /// 中医类 中医治疗费
        /// </summary>
        public string ZYZLF
        {
            get
            {
                if (_ZYZLF == null || _ZYZLF.Trim() == "")
                    _ZYZLF = "-";
                return _ZYZLF;
            }
            set
            {
                _ZYZLF = value;
            }
        }
        private string _ZYZLF;

        /// <summary>
        /// 中医外治：
        /// </summary>
        public string ZYWZ
        {
            get
            {
                if (_ZYWZ == null || _ZYWZ.Trim() == "")
                    _ZYWZ = "-";
                return _ZYWZ;
            }
            set
            {
                _ZYWZ = value;
            }
        }
        private string _ZYWZ;

        /// <summary>
        /// 中医骨伤
        /// </summary>
        public string ZYGS
        {
            get
            {
                if (_ZYGS == null || _ZYGS.Trim() == "")
                    _ZYGS = "-";
                return _ZYGS;
            }
            set
            {
                _ZYGS = value;
            }
        }
        private string _ZYGS;

        /// <summary>
        /// 针刺与灸法
        /// </summary>
        public string ZCYJF
        {
            get
            {
                if (_ZCYJF == null || _ZCYJF.Trim() == "")
                    _ZCYJF = "-";
                return _ZCYJF;
            }
            set
            {
                _ZCYJF = value;
            }
        }
        private string _ZCYJF;

        /// <summary>
        /// 中医推拿治疗
        /// </summary>
        public string ZYTNZL
        {
            get
            {
                if (_ZYTNZL == null || _ZYTNZL.Trim() == "")
                    _ZYTNZL = "-";
                return _ZYTNZL;
            }
            set
            {
                _ZYTNZL = value;
            }
        }
        private string _ZYTNZL;

        /// <summary>
        /// 中医肛肠治疗
        /// </summary>
        public string ZYGCZL
        {
            get
            {
                if (_ZYGCZL == null || _ZYGCZL.Trim() == "")
                    _ZYGCZL = "-";
                return _ZYGCZL;
            }
            set
            {
                _ZYGCZL = value;
            }
        }
        private string _ZYGCZL;

        /// <summary>
        /// 中医特殊治疗
        /// </summary>
        public string ZYTSZL
        {
            get
            {
                if (_ZYTSZL == null || _ZYTSZL.Trim() == "")
                    _ZYTSZL = "-";
                return _ZYTSZL;
            }
            set
            {
                _ZYTSZL = value;
            }
        }
        private string _ZYTSZL;

        /// <summary>
        /// 中医其他
        /// </summary>
        public string ZYQT
        {
            get
            {
                if (_ZYQT == null || _ZYQT.Trim() == "")
                    _ZYQT = "-";
                return _ZYQT;
            }
            set
            {
                _ZYQT = value;
            }
        }
        private string _ZYQT;

        /// <summary>
        /// 中药特殊调配加工
        /// </summary>
        public string ZYTSTPJG
        {
            get
            {
                if (_ZYTSTPJG == null || _ZYTSTPJG.Trim() == "")
                    _ZYTSTPJG = "-";
                return _ZYTSTPJG;
            }
            set
            {
                _ZYTSTPJG = value;
            }
        }
        private string _ZYTSTPJG;

        /// <summary>
        /// 辨证施膳
        /// </summary>
        public string BZSS
        {
            get
            {
                if (_BZSS == null || _BZSS.Trim() == "")
                    _BZSS = "-";
                return _BZSS;
            }
            set
            {
                _BZSS = value;
            }
        }
        private string _BZSS;

        /// <summary>
        /// 西药类 西药费
        /// </summary>
        public string XYF
        {
            get
            {
                if (_XYF == null || _XYF.Trim() == "")
                    _XYF = "-";
                return _XYF;
            }
            set
            {
                _XYF = value;
            }
        }
        private string _XYF;

        /// <summary>
        /// 西药类 抗菌药物费用
        /// </summary>
        public string KJYWF
        {
            get
            {
                if (_KJYWF == null || _KJYWF.Trim() == "")
                    _KJYWF = "-";
                return _KJYWF;
            }
            set
            {
                _KJYWF = value;
            }
        }
        private string _KJYWF;

        /// <summary>
        /// 中药类 中成药费
        /// </summary>
        public string CPMedical
        {
            get
            {
                if (_CPMedical == null || _CPMedical.Trim() == "")
                    _CPMedical = "-";
                return _CPMedical;
            }
            set
            {
                _CPMedical = value;
            }
        }
        private string _CPMedical;

        /// <summary>
        /// 医疗机构中药制剂费
        /// </summary>
        public string YLJGZYZJF
        {
            get
            {
                if (_YLJGZYZJF == null || _YLJGZYZJF.Trim() == "")
                    _YLJGZYZJF = "-";
                return _YLJGZYZJF;
            }
            set
            {
                _YLJGZYZJF = value;
            }
        }
        private string _YLJGZYZJF;

        /// <summary>
        /// 中药类 中草药费
        /// </summary>
        public string CMedical
        {
            get
            {
                if (_CMedical == null || _CMedical.Trim() == "")
                    _CMedical = "-";
                return _CMedical;
            }
            set
            {
                _CMedical = value;
            }
        }
        private string _CMedical;

        /// <summary>
        /// 血液和血液制品类 血费
        /// </summary>
        public string BloodFee
        {
            get
            {
                if (_BloodFee == null || _BloodFee.Trim() == "")
                    _BloodFee = "-";
                return _BloodFee;
            }
            set
            {
                _BloodFee = value;
            }
        }
        private string _BloodFee;

        /// <summary>
        /// 血液和血液制品类 白蛋白类制品费
        /// </summary>
        public string XDBLZPF
        {
            get
            {
                if (_XDBLZPF == null || _XDBLZPF.Trim() == "")
                    _XDBLZPF = "-";
                return _XDBLZPF;
            }
            set
            {
                _XDBLZPF = value;
            }
        }
        private string _XDBLZPF;

        /// <summary>
        /// 血液和血液制品类 球蛋白类制品费
        /// </summary>
        public string QDBLZPF
        {
            get
            {
                if (_QDBLZPF == null || _QDBLZPF.Trim() == "")
                    _QDBLZPF = "-";
                return _QDBLZPF;
            }
            set
            {
                _QDBLZPF = value;
            }
        }
        private string _QDBLZPF;

        /// <summary>
        /// 血液和血液制品类 凝血因子类制品费
        /// </summary>
        public string NXYZLZPF
        {
            get
            {
                if (_NXYZLZPF == null || _NXYZLZPF.Trim() == "")
                    _NXYZLZPF = "-";
                return _NXYZLZPF;
            }
            set
            {
                _NXYZLZPF = value;
            }
        }
        private string _NXYZLZPF;

        /// <summary>
        /// 血液和血液制品类 细胞因子类制品费
        /// </summary>
        public string XBYZLZPF
        {
            get
            {
                if (_XBYZLZPF == null || _XBYZLZPF.Trim() == "")
                    _XBYZLZPF = "-";
                return _XBYZLZPF;
            }
            set
            {
                _XBYZLZPF = value;
            }
        }
        private string _XBYZLZPF;

        /// <summary>
        /// 耗材类 检查用一次性医用材料费
        /// </summary>
        public string JCYYCXCLF
        {
            get
            {
                if (_JCYYCXCLF == null || _JCYYCXCLF.Trim() == "")
                    _JCYYCXCLF = "-";
                return _JCYYCXCLF;
            }
            set
            {
                _JCYYCXCLF = value;
            }
        }
        private string _JCYYCXCLF;

        /// <summary>
        /// 耗材类 治疗用一次性医用材料费
        /// </summary>
        public string ZLYYCXCLF
        {
            get
            {
                if (_ZLYYCXCLF == null || _ZLYYCXCLF.Trim() == "")
                    _ZLYYCXCLF = "-";
                return _ZLYYCXCLF;
            }
            set
            {
                _ZLYYCXCLF = value;
            }
        }
        private string _ZLYYCXCLF;

        /// <summary>
        /// 耗材类 手术用一次性医用材料费
        /// </summary>
        public string SSYYCXCLF
        {
            get
            {
                if (_SSYYCXCLF == null || _SSYYCXCLF.Trim() == "")
                    _SSYYCXCLF = "-";
                return _SSYYCXCLF;
            }
            set
            {
                _SSYYCXCLF = value;
            }
        }
        private string _SSYYCXCLF;

        /// <summary>
        /// 其他类：（24）其他费
        /// </summary>
        public string OtherFee
        {
            get
            {
                if (_OtherFee == null || _OtherFee.Trim() == "")
                    _OtherFee = "-";
                return _OtherFee;
            }
            set
            {
                _OtherFee = value;
            }
        }
        private string _OtherFee;

    }
    #endregion

    #region 病案首页 产科产妇婴儿情况
    /// <summary>
    /// 打印病案首页 产科产妇婴儿情况
    /// </summary>
    public class Iem_MainPage_ObstetricsBaby
    {
        public DataTable OutBabyTable
        {
            get
            {
                if (_OutBabyTable == null)
                {
                    _OutBabyTable = new DataTable();
                    DataColumn TC = new DataColumn("TC", Type.GetType("System.String"));
                    DataColumn CC = new DataColumn("CC", Type.GetType("System.String"));
                    DataColumn TB = new DataColumn("TB", Type.GetType("System.String"));
                    DataColumn CFHYPLD = new DataColumn("CFHYPLD", Type.GetType("System.String"));
                    DataColumn MIDWIFERY = new DataColumn("MIDWIFERY", Type.GetType("System.String"));
                    DataColumn SEX = new DataColumn("SEX", Type.GetType("System.String"));

                    DataColumn APJ = new DataColumn("APJ", Type.GetType("System.String"));
                    DataColumn HEIGH = new DataColumn("HEIGH", Type.GetType("System.String"));
                    DataColumn WEIGHT = new DataColumn("WEIGHT", Type.GetType("System.String"));
                    DataColumn CCQK = new DataColumn("CCQK", Type.GetType("System.String"));
                    DataColumn BITHDAY = new DataColumn("BITHDAY", Type.GetType("System.String"));
                    DataColumn FMFS = new DataColumn("FMFS", Type.GetType("System.String"));

                    DataColumn CYQK = new DataColumn("CYQK", Type.GetType("System.String"));
                    DataColumn CREATE_USER = new DataColumn("CREATE_USER", Type.GetType("System.String"));
                    DataColumn CREATE_TIME = new DataColumn("CREATE_TIME", Type.GetType("System.String"));
                    DataColumn VALIDE = new DataColumn("VALIDE", Type.GetType("System.String"));
                    DataColumn IBSBABYID = new DataColumn("IBSBABYID", Type.GetType("System.String"));


                    _OutBabyTable.Columns.Add(TC);
                    _OutBabyTable.Columns.Add(CC);
                    _OutBabyTable.Columns.Add(TB);
                    _OutBabyTable.Columns.Add(CFHYPLD);
                    _OutBabyTable.Columns.Add(MIDWIFERY);
                    _OutBabyTable.Columns.Add(SEX);

                    _OutBabyTable.Columns.Add(APJ);
                    _OutBabyTable.Columns.Add(HEIGH);
                    _OutBabyTable.Columns.Add(WEIGHT);
                    _OutBabyTable.Columns.Add(CCQK);
                    _OutBabyTable.Columns.Add(BITHDAY);
                    _OutBabyTable.Columns.Add(FMFS);

                    _OutBabyTable.Columns.Add(CYQK);
                    _OutBabyTable.Columns.Add(CREATE_USER);
                    _OutBabyTable.Columns.Add(CREATE_TIME);
                    _OutBabyTable.Columns.Add(VALIDE);
                    _OutBabyTable.Columns.Add(IBSBABYID);
                }
                return _OutBabyTable;
            }
            set { _OutBabyTable = value; }
        }

        private DataTable _OutBabyTable;
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

        /// <summary>
        /// 编号
        /// </summary>
        public string IBSBABYID { get; set; }
    }
    #endregion
}
