using System;

namespace DrectSoft.Core.IEMMainPage
{

    /// <summary>
    /// 病案首页默认值数据表
    /// </summary>
    public class PaintDefaultInfo
    {
        #region declaration
        private int p_ID = 0;//标志列
        /// <summary>
        /// 标志列
        /// </summary>
        public int P_ID
        {
            get { return p_ID; }
            set { p_ID = value; }
        }
        private string p_TypeNum = String.Empty;//类型标识
        /// <summary>
        /// 类型标识
        /// </summary>
        public string P_TypeNum
        {
            get { return p_TypeNum; }
            set { p_TypeNum = value; }
        }

        private string p_TName = String.Empty;//节点名称
        /// <summary>
        /// 节点名称
        /// </summary>
        public string P_pTName
        {
            get { return p_TName; }
            set { p_TName = value; }
        }

        private string p_TValue = String.Empty;//节点值
        /// <summary>
        /// 节点值
        /// </summary>
        public string P_pTValue
        {
            get { return p_TValue; }
            set { p_TValue = value; }
        }



        private string provicename = string.Empty;
        /// <summary>
        /// 省名称
        /// </summary>
        public string Provicename
        {
            get { return provicename; }
            set { provicename = value; }
        }

        private string proviceid = string.Empty;
        /// <summary>
        /// 省编号
        /// </summary>
        public string ProviceID
        {
            get { return proviceid; }
            set { proviceid = value; }
        }


        private string cityname = string.Empty;
        /// <summary>
        /// 城市名称 
        /// </summary>
        public string Cityname
        {
            get { return cityname; }
            set { cityname = value; }
        }


        private string cityid = string.Empty;
        /// <summary>
        /// 城市编号
        /// </summary>
        public string CityID
        {
            get { return cityid; }
            set { cityid = value; }
        }
        private string dintname = string.Empty;
        /// <summary>
        /// 县名称
        /// </summary>
        public string Dintname
        {
            get { return dintname; }
            set { dintname = value; }
        }
        private string dintid = string.Empty;
        /// <summary>
        /// 县编号
        /// </summary>
        public string DintID
        {
            get { return dintid; }
            set { dintid = value; }
        }
        private string postid = string.Empty;
        /// <summary>
        /// 邮编
        /// </summary>
        public string PostID
        {
            get { return postid; }
            set { postid = value; }
        }
        private string turnindeptname = string.Empty;
        /// <summary>
        /// 转入科别名称
        /// </summary>
        public string TurnInDeptName
        {
            get { return turnindeptname; }
            set { turnindeptname = value; }
        }
        private string turnindeptid = string.Empty;
        /// <summary>
        /// 转入科别编号
        /// </summary>
        public string TurnInDeptID
        {
            get { return turnindeptid; }
            set { turnindeptid = value; }
        }
        #endregion declaration
    }

    #region 新增的一个表和Inpatient表结构一致 ywk
    /// <summary>
    /// 默认首页值的类 2012年5月16日 15:47:29
    /// </summary>
    public class Iem_Default_PaientInfo
    {
        /// <summary>
        /// 病案首页序号
        /// </summary>
        public string Iem_Mainpage_NO { get; set; }
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

        private string _MainDiagDate;
        /// <summary>
        /// 主要诊断确诊日期
        /// </summary>
        public string MainDiagDate { get { return _MainDiagDate; } set { _MainDiagDate = value; } }
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
        /// <summary>
        /// 入院途径
        /// </summary>
        public string AdmitWay { get; set; }
        /// <summary>
        /// 入院情况
        /// </summary>
        public string AdmitInfo { get; set; }

        /// <summary>
        /// 入院情况
        /// </summary>
        public string DeInHosCall { get; set; }
        /// <summary>
        /// 门诊诊断
        /// </summary>
        public string OutDiagID { get; set; }
        /// <summary>
        /// 科主任编号
        /// </summary>
        public string Section_DirectorID { get; set; }
        /// <summary>
        /// 科主任
        /// </summary>
        public string Section_DirectorName { get; set; }

        /// <summary>
        /// 主任（副主任）医师编号
        /// </summary>
        public string DirectorID { get; set; }
        /// <summary>
        /// 主任（副主任）医师
        /// </summary>
        public string DirectorName { get; set; }
        /// <summary>
        /// 主治医师（编号）
        /// </summary>
        public string Vs_EmployeeID { get; set; }
        /// <summary>
        /// 主治医师
        /// </summary>
        public string Vs_EmployeeName { get; set; }
        /// <summary>
        /// 住院医师(编号)
        /// </summary>
        public string Resident_EmployeeID { get; set; }
        /// <summary>
        /// 住院医师
        /// </summary>
        public string Resident_EmployeeName { get; set; }
        #endregion
        /// <summary>
        /// 是否婴儿
        /// </summary>
        public string IsBaby { get; set; }
        /// <summary>
        /// 母亲Noofinpat
        /// </summary>
        public string Mother { get; set; }
    }
    #endregion
}
