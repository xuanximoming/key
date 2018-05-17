using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.OwnBedInfo
{
    /// <summary>
    /// 病人表实体类
    /// </summary>
    #region 病案首页诊断基础信息板块
    /// <summary>
    /// 
    /// </summary>
    public class PatientEntity
    {
        /// <summary>
        /// 病案首页序号
        /// </summary>
        public string Iem_Mainpage_NO { get; set; }
        /// <summary>
        /// 病人首页序号
        /// </summary>
        public int NoOfInpat { get; set; }
        private string _PatNoOfHis;
        /// <summary>
        /// His首页序号
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
        private string _Noofclinic;
        /// <summary>
        /// 门诊号码
        /// </summary>
        public string NoOfcClinic
        {
            get { return _Noofclinic; }
            set { _Noofclinic = value; }
        }
        private string _Noofrecord;
        /// <summary>
        /// 病案号码
        /// </summary>
        public string NoOfRecord
        {
            get { return _Noofrecord; }
            set { _Noofrecord = value; }
        }
        private string _patid;
        /// <summary>
        /// 住院号码
        /// </summary>
        public string PatID
        {
            get { return _patid; }
            set { _patid = value; }
        }
        private string Innerpix;
        /// <summary>
        /// 外部索引值
        /// </summary>
        public string INNERPIX
        {
            get { return Innerpix; }
            set { Innerpix = value; }
        }
        private string _outpix;
        /// <summary>
        /// 外部索引值
        /// </summary>
        public string OUTPIX
        {
            get { return _outpix; }
            set { _outpix = value; }
        }
        private string _Name;
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
        private string _py;
        /// <summary>
        /// 拼音
        /// </summary>
        public string PY
        {
            get { return _py; }
            set { _py = value; }
        }
        private string _wb;
        /// <summary>
        /// 五笔
        /// </summary>
        public string WB
        {
            get { return _wb; }
            set { _wb = value; }
        }
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
        /// <summary>
        /// 病人来源(Dictionary_detail,CategoryID= '2')
        /// </summary>
        public string ORIGIN { get; set; }
        private int _InCount;
        /// <summary>
        /// 入院次数
        /// </summary>
        public int InCount
        {
            get { return _InCount; }
            set { _InCount = value; }
        }
        private string _SexID;
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
        private int _Age;
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age
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
        /// <summary>
        /// 显示的年龄
        /// </summary>
        public string AgeStr { get; set; }
        private string _IDNO;
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
        private string _Marital;
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
        /// <summary>
        /// 职业
        /// </summary>
        public string JobID { get; set; }
        private string _JobName;
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
        /// 民族ID
        /// </summary>
        public string NationID { get; set; }
        private string _NationName;
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
        /// <summary>
        /// 国籍ID
        /// </summary>
        public string NationalityID { get; set; }
        private string _NationalityName;
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
        /// <summary>
        /// 籍贯 省
        /// </summary>
        public string JG_ProvinceID { get; set; }
        /// <summary>
        /// 籍贯 市
        /// </summary>
        public string JG_CityID { get; set; }
        
        /// <summary>
        /// 工作单位
        /// </summary>
        public string Organization { get; set; }
        private string _OfficePlace;
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
        private string _OfficeTEL;
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
        private string _OfficePost;
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
        /// 户口邮编
        /// </summary>
        public string NATIVEPOST
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
        /// <summary>
        /// 户口电话
        /// </summary>
        public string NATIVETEL { get; set; }
        /// <summary>
        /// 户口地址
        /// </summary>
        public string NATIVEADDRESS { get; set; }
        /// <summary>
        /// 当前地址
        /// </summary>
        public string ADDRESS { get; set; }
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
        /// 联系人单位 
        /// </summary>
        public string CONTACTOFFICE { get; set; }
        /// <summary>
        /// 联系人邮编
        /// </summary>
        public string CONTACTPOST { get; set; }
        /// <summary>
        /// 病史陈述者
        /// </summary>
        public string OFFERER { get; set; }
        /// <summary>
        /// 社保卡号
        /// </summary>
        public string SocialCare { get; set; }
        /// <summary>
        /// 保险卡号
        /// </summary>
        public string INSURANCE { get; set; }
        /// <summary>
        /// 其他卡号
        /// </summary>
        public string CARDNO { get; set; }
        /// <summary>
        /// 入院情况 
        /// </summary>
        public string ADMITINFO { get; set; }
        /// <summary>
        /// 入院科室
        /// </summary>
        public string AdmitDeptID { get; set; }
        /// <summary>
        /// 入院病区
        /// </summary>
        public string AdmitWardID { get; set; }
        /// <summary>
        /// 入院床位
        /// </summary>
        public string ADMITBED { get; set; }
        private string _AdmitDate;
        /// <summary>
        /// 入院时间
        /// </summary>
        public string AdmitDate { get { return _AdmitDate; } set { _AdmitDate = value; } }
        /// <summary>
        /// 入区日期
        /// </summary>
        public string INWARDDATE { get; set; }
        /// <summary>
        /// 入院诊断
        /// </summary>
        public string ADMITDIAGNOSIS { get; set; }
        private string _OutWardDate;
        /// <summary>
        /// 出区时间
        /// </summary>
        public string OutWardDate { get { return _OutWardDate; } set { _OutWardDate = value; } }
        /// <summary>
        /// 出院科室
        /// </summary>
        public string OutHosDeptID { get; set; }
        /// <summary>
        /// 出院病区
        /// </summary>
        public string OutHosWardID { get; set; }
        /// <summary>
        /// 出院床位
        /// </summary>
        public string OutBed { get; set; }
        /// <summary>
        /// 出院日期
        /// </summary>
        public string OUTHOSDATE { get; set; }
        /// <summary>
        /// 出院诊断
        /// </summary>
        public string OUTDIAGNOSIS { get; set; }
        /// <summary>
        /// 住院天数
        /// </summary>
        public int TOTALDAYS { get; set; }
        /// <summary>
        /// 门诊诊断
        /// </summary>
        public string CLINICDIAGNOSIS { get; set; }
        /// <summary>
        /// 发病节气
        /// </summary>
        public string SOLARTERMS { get; set; }
        /// <summary>
        /// 入院途径(Dictionary_detail,CategoryID= '6')
        /// </summary>
        public string ADMITWAY { get; set; }
        /// <summary>
        /// /出院方式(Dictionary_detail,CategoryID= '15')	
        /// </summary>
        public string OUTWAY { get; set; }
        /// <summary>
        /// /门诊医生(Users.ID)
        /// </summary>
        public string CLINICDOCTOR { get; set; }
        /// <summary>
        /// 住院医师代码(Users.ID)
        /// </summary>
        public string RESIDENT { get; set; }
        /// <summary>
        /// 主治医师代码(Users.ID)
        /// </summary>
        public string ATTEND { get; set; }
        /// <summary>
        /// 主任医师代码(Users.ID)
        /// </summary>
        public string CHIEF { get; set; }
        /// <summary>
        /// 文化程度(Dictionary_detail,CategoryID= '25')
        /// </summary>
        public string EDU { get; set; }
        /// <summary>
        /// (受)教育年限(单位:年)
        /// </summary>
        public int EDUC { get; set; }
        /// <summary>
        /// 宗教信仰
        /// </summary>
        public string RELIGION { get; set; }
        /// <summary>
        /// 病人状态(CategoryDetail.ID,CategoryID = 15)
        /// </summary>
        public int STATUS { get; set; }
        /// <summary>
        /// 危重级别(Dictionary_detail,CategoryID= '53')
        /// </summary>
        public string CRITICALLEVEL { get; set; }
        /// <summary>
        /// 护理级别(ChargingMinItem.sfxmdm, xmlb = 2409)
        /// </summary>
        public string ATTENDLEVEL { get; set; }
        /// <summary>
        /// 重点病人(CategoryDetail.ID,CategoryID = 0)
        /// </summary>
        public int EMPHASIS { get; set; }
        /// <summary>
        /// 婴儿序号(从1开始，0表示不是婴儿)
        /// </summary>
        public int ISBABY { get; set; }
        /// <summary>
        /// 母亲首页序号(InPatient.NoOfInpat, yexh = 0)
        /// </summary>
        public int MOTHER { get; set; }
        /// <summary>
        /// 医保代码
        /// </summary>
        public string MEDICAREID { get; set; }
        /// <summary>
        /// 医保定额
        /// </summary>
        public int MEDICAREQUOTA { get; set; }
        /// <summary>
        /// 凭证类型(代码)
        /// </summary>
        public string VOUCHERSCODE { get; set; }
        /// <summary>
        /// 病人类型(Dictionary_detail,CategoryID= '45')
        /// </summary>
        public string STYLE { get; set; }
        /// <summary>
        /// 操作员(Users.ID)
        /// </summary>
        public string OPERATOR { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string MEMO { get; set; }
        /// <summary>
        /// 临床路径中状态
        /// </summary>
        public int CPSTATUS { get; set; }
        /// <summary>
        /// 出去床位
        /// </summary>
        public int OUTWARDBED { get; set; }
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
        /*******************************************************/
    #endregion
    }
}
