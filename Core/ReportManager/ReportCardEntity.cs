using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ReportManager
{
    /// <summary>
    /// <date>2013-07-09</date>
    /// 报告卡对应实体
    /// <auth>XLB</auth>
    /// </summary>
    public class ReportCardEntity
    {
        /// <summary>
        ///  患者离开医院方式
        /// </summary>
        private string report_OuthosType = String.Empty;

        /// <summary>
        ///  患者序号
        /// </summary>
        private string report_Noofinpat = String.Empty;
        /// <summary>
        ///  报告卡序号
        /// </summary>
        private string report_Id = "0";
        /// <summary>
        ///  报告卡编码
        /// </summary>
        private string report_No = String.Empty;
        /// <summary>
        /// 报告卡区(县)编码
        /// </summary>
        private string report_districtId = String.Empty;

        /// <summary>
        /// 报告卡区(县)名称
        /// </summary>
        private string report_districtName = String.Empty;

        /// <summary>
        /// 报告卡ICD-10编码
        /// </summary>
        private string report_ICD10 = String.Empty;

        /// <summary>
        /// 报告卡ICD-0编码
        /// </summary>
        private string report_ICD0 = String.Empty;

        /// <summary>
        /// 报告卡门诊号
        /// </summary>
        private string report_ClinicId = String.Empty;

        /// <summary>
        /// 报告卡住院号
        /// </summary>
        private string report_PatId = String.Empty;

        /// <summary>
        /// 报告卡身份证号
        /// </summary>
        private string report_INDO = String.Empty;

        /// <summary>
        /// 报告卡病人姓名
        /// </summary>
        private string report_InpatName = String.Empty;

        /// <summary>
        /// 性别
        /// </summary>
        private string sexId = String.Empty;

        /// <summary>
        /// 实足年龄
        /// </summary>
        private string realAge = String.Empty;

        /// <summary>
        /// 出生日期
        /// </summary>
        private string birthDate = String.Empty;

        /// <summary>
        /// 民族编码
        /// </summary>
        private string nationId = String.Empty;

        /// <summary>
        /// 名族名称
        /// </summary>
        private string nationName = String.Empty;

        /// <summary>
        /// 家庭联系电话
        /// </summary>
        private string contactTel = String.Empty;

        /// <summary>
        /// 婚姻状况
        /// </summary>
        private string martial = String.Empty;

        /// <summary>
        /// 病患职业
        /// </summary>
        private string occuPation = String.Empty;

        /// <summary>
        /// 工作单位地址
        /// </summary>
        private string officeAddress = String.Empty;

        /// <summary>
        /// 户口地址省份对应编码
        /// </summary>
        private string orgProvinceId = String.Empty;

        /// <summary>
        /// 户口地址所在市编码
        /// </summary>
        private string orgCityId = String.Empty;

        /// <summary>
        /// 户口所在地区县编码
        /// </summary>
        private string orgDistrictId = String.Empty;

        /// <summary>
        /// 户口所在地镇(街道)编码
        /// </summary>
        private string orgTownId = String.Empty;

        /// <summary>
        /// 户口所在地村编码
        /// </summary>
        private string orgVillage = String.Empty;

        /// <summary>
        /// 户口所在地省份全称
        /// </summary>
        private string orgProvinceName = String.Empty;

        /// <summary>
        /// 户口所在地区县编码
        /// </summary>
        private string orgDistrictName = String.Empty;

        /// <summary>
        /// 户口所在地市全称
        /// </summary>
        private string orgCityName = String.Empty;

        /// <summary>
        /// 户口所在地镇(街道)全称
        /// </summary>
        private string orgTown = String.Empty;

        /// <summary>
        /// 户口所在地村全称
        /// </summary>
        private string orgVillageName = String.Empty;

        /// <summary>
        /// 现住址省份编码
        /// </summary>
        private string xZZProvinceId = String.Empty;

        /// <summary>
        /// 现住址市编码
        /// </summary>
        private string xZZCityId = String.Empty;

        /// <summary>
        /// 现住址区县编码
        /// </summary>
        private string xZZDistrictId = String.Empty;

        /// <summary>
        /// 现住址镇(街道)编码
        /// </summary>
        private string xZZTownId = String.Empty;

        /// <summary>
        /// 现住址所在村编码
        /// </summary>
        private string xZZVillageId = String.Empty;

        /// <summary>
        /// 现住址省份全称
        /// </summary>
        private string xZZProvince = String.Empty;

        /// <summary>
        /// 现住址市全称
        /// </summary>
        private string xZZCity = String.Empty;

        /// <summary>
        /// 现住址所在区县全称
        /// </summary>
        private string xZZDistrict = String.Empty;

        /// <summary>
        /// 现住址镇(街道)全称
        /// </summary>
        private string xZZTown = String.Empty;

        /// <summary>
        /// 现住址村全称
        /// </summary>
        private string xZZVillage = String.Empty;

        /// <summary>
        /// 诊断
        /// </summary>
        private string report_Diagnosis = String.Empty;

        /// <summary>
        /// 病理类型
        /// </summary>
        private string pathologicalType = String.Empty;

        /// <summary>
        /// 病理诊断病理号
        /// </summary>
        private string pathologicalId = String.Empty;

        /// <summary>
        /// 确诊日期_T期
        /// </summary>
        private string qZDiagTime_T = String.Empty;

        /// <summary>
        /// 确诊日期_N期
        /// </summary>
        private string qZDiagTime_N = String.Empty;

        /// <summary>
        /// 确诊日期_M期
        /// </summary>
        private string qZDiagTime_M = String.Empty;

        /// <summary>
        /// 首次确诊日期
        /// </summary>
        private string firstDiaDate = String.Empty;

        /// <summary>
        /// 报告单位
        /// </summary>
        private string reportinfunit = String.Empty;

        /// <summary>
        /// 报告医生
        /// </summary>
        private string reportDoctor = String.Empty;

        /// <summary>
        /// 报告日期
        /// </summary>
        private string reportDate = String.Empty;

        /// <summary>
        /// 死亡日期
        /// </summary>
        private string deathDate = String.Empty;

        /// <summary>
        /// 死亡原因
        /// </summary>
        private string deathReason = String.Empty;

        /// <summary>
        /// 病历摘要
        /// </summary>
        private string reJest = String.Empty;


        /// <summary>
        /// 诊断单位
        /// </summary>
        private string reportdiagfunit = String.Empty;

        public string ReportDiagfunit
        {
            get { return reportdiagfunit; }
            set { reportdiagfunit = value; }
        }
        /// <summary>
        /// 诊断单位
        /// </summary>
        private string reportdiagfunitname = String.Empty;

        public string ReportDiagfunitName
        {
            get { return reportdiagfunitname; }
            set { reportdiagfunitname = value; }
        }
        /// <summary>
        /// 临床分期
        /// </summary>
        private string clinicalstages = String.Empty;

        public string ClinicalStages
        {
            get { return clinicalstages; }
            set { clinicalstages = value; }
        }

        /// <summary>
        /// 报告卡类型
        /// </summary>
        private string reportCardType = String.Empty;




        private string m_State = String.Empty;
        private string m_CreateDate = String.Empty;
        private string m_CreateUsercode = String.Empty;
        private string m_CreateUsername = String.Empty;

        private string m_CreateDeptcode = String.Empty;
        private string m_CreateDeptname = String.Empty;
        private string m_ModifyDate = String.Empty;
        private string m_ModifyUsercode = String.Empty;
        private string m_ModifyUsername = String.Empty;

        private string m_ModifyDeptcode = String.Empty;
        private string m_ModifyDeptname = String.Empty;
        private string m_AuditDate = String.Empty;
        private string m_AuditUsercode = String.Empty;
        private string m_AuditUsername = String.Empty;

        private string m_AuditDeptcode = String.Empty;
        private string m_AuditDeptname = String.Empty;
        private string m_Vaild = String.Empty;
        //report_OuthosType
        public string Report_OuthosType
        {
            get { return report_OuthosType; }
            set { report_OuthosType = value; }
        }
        /// <summary>
        /// 报告卡序号
        /// </summary>
        public string Report_Id
        {
            get { return report_Id; }
            set { report_Id = value; }
        }

        /// <summary>
        /// 报告卡序号
        /// </summary>
        public string Report_Noofinpat
        {
            get { return report_Noofinpat; }
            set { report_Noofinpat = value; }
        }

        /// <summary>
        /// 报告卡编码
        /// </summary>
        public string Report_No
        {
            get { return report_No; }
            set { report_No = value; }
        }

        /// <summary>
        /// 报告卡区县编码
        /// </summary>
        public string Report_DistrictId
        {
            get { return report_districtId; }
            set { report_districtId = value; }
        }

        /// <summary>
        /// 报告卡区县名称
        /// </summary>
        public string Report_DistrictName
        {
            get
            {
                return report_districtName;
            }
            set
            {
                report_districtName = value;
            }
        }

        /// <summary>
        /// 报告卡ICD-10编码
        /// </summary>
        public string Report_ICD10
        {
            get
            {
                return report_ICD10;
            }
            set
            {
                report_ICD10 = value;
            }
        }

        /// <summary>
        /// 报告卡ICD-0编码
        /// </summary>
        public string Report_ICD0
        {
            get
            {
                return report_ICD0;
            }
            set
            {
                report_ICD0 = value;
            }
        }

        /// <summary>
        /// 报告卡门诊号
        /// </summary>
        public string Report_ClinicId
        {
            get
            {
                return report_ClinicId;
            }
            set
            {
                report_ClinicId = value;
            }
        }

        /// <summary>
        /// 报告卡住院号
        /// </summary>
        public string Report_PatId
        {
            get
            {
                return report_PatId;
            }
            set
            {
                report_PatId = value;
            }
        }

        /// <summary>
        /// 报告卡身份证号
        /// </summary>
        public string Report_INDO
        {
            get
            {
                return report_INDO;
            }
            set
            {
                report_INDO = value;
            }
        }

        /// <summary>
        /// 报告卡病人姓名
        /// </summary>
        public string Report_InpatName
        {
            get
            {
                return report_InpatName;
            }
            set
            {
                report_InpatName = value;
            }
        }

        /// <summary>
        /// 性别
        /// </summary>
        public string SexId
        {
            get
            {
                return sexId;
            }
            set
            {
                sexId = value;
            }
        }

        /// <summary>
        /// 实足年龄
        /// </summary>
        public string RealAge
        {
            get
            {
                return realAge;
            }
            set
            {
                realAge = value;
            }
        }

        /// <summary>
        /// 出生日期
        /// </summary>
        public string BirthDate
        {
            get
            {
                return birthDate;
            }
            set
            {
                birthDate = value;
            }
        }

        private string _birthDateYear;

        /// <summary>
        /// 出生日期之年份
        /// </summary>
        public string BirthDateYear
        {
            get
            {
                _birthDateYear = string.IsNullOrEmpty(birthDate) ? "" : Convert.ToDateTime(birthDate).ToString("yyyy");
                return _birthDateYear;
            }
        }

        private string _birthDateMonth;

        /// <summary>
        /// 出生日期之月份
        /// </summary>
        public string BirthDateMonth
        {
            get
            {
                _birthDateMonth = string.IsNullOrEmpty(birthDate) ? "" : Convert.ToDateTime(birthDate).ToString("MM");
                return _birthDateMonth;
            }
        }

        private string _birthDateDay;

        /// <summary>
        /// 出生日期之天
        /// </summary>
        public string BirthDateDay
        {
            get
            {
                _birthDateDay = string.IsNullOrEmpty(birthDate) ? "" : Convert.ToDateTime(birthDate).ToString("dd");
                return _birthDateDay;
            }
        }

        /// <summary>
        /// 民族编码
        /// </summary>
        public string NationId
        {
            get
            {
                return nationId;
            }
            set
            {
                nationId = value;
            }
        }

        /// <summary>
        /// 民族全称
        /// </summary>
        public string NationName
        {
            get
            {
                return nationName;
            }
            set
            {
                nationName = value;
            }
        }

        /// <summary>
        /// 家庭电话
        /// </summary>
        public string ContactTel
        {
            get
            {
                return contactTel;
            }
            set
            {
                contactTel = value;
            }
        }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string Martial
        {
            get
            {
                return martial;
            }
            set
            {
                martial = value;
            }
        }

        /// <summary>
        /// 患者职业
        /// </summary>
        public string Occupation
        {
            get
            {
                return occuPation;
            }
            set
            {
                occuPation = value;
            }
        }

        /// <summary>
        /// 工作单位地址
        /// </summary>
        public string OfficeAddress
        {
            get
            {
                return officeAddress;
            }
            set
            {
                officeAddress = value;
            }
        }

        /// <summary>
        /// 户口所在地省份编码
        /// </summary>
        public string OrgProvinceId
        {
            get
            {
                return orgProvinceId;
            }
            set
            {
                orgProvinceId = value;
            }
        }

        /// <summary>
        /// 户口所在地市编码
        /// </summary>
        public string OrgCityId
        {
            get
            {
                return orgCityId;
            }
            set
            {
                orgCityId = value;
            }
        }

        /// <summary>
        /// 户口所在地区县编码
        /// </summary>
        public string OrgDistrictId
        {
            get
            {
                return orgDistrictId;
            }
            set
            {
                orgDistrictId = value;
            }
        }

        /// <summary>
        /// 户口所在地镇(街道)编码
        /// </summary>
        public string OrgTownId
        {
            get
            {
                return orgTownId;
            }
            set
            {
                orgTownId = value;
            }
        }

        /// <summary>
        /// 户口所在地存地址
        /// </summary>
        public string OrgVillage
        {
            get
            {
                return orgVillage;
            }
            set
            {
                orgVillage = value;
            }
        }
        //存放户口地址拼接内容 add by ywk 
        private string hkaddress;

        public string HkAddress
        {
            get { return OrgProvinceName + OrgCityName + OrgDistrictName + OrgTown + OrgVillageName; }
            set { hkaddress = value; }
        }


        //存放现住址拼接内容
        private string xzzaddress;

        public string XzzAddress
        {
            get { return XZZProvince + XZZCity + XZZDistrict + XZZTown + XZZVillage; }
            set { xzzaddress = value; }
        }
        /// <summary>
        /// 户口所在地省份全称
        /// </summary>
        public string OrgProvinceName
        {
            get
            {
                return orgProvinceName;
            }
            set
            {
                orgProvinceName = value;
            }
        }

        /// <summary>
        /// 户口所在地市全称
        /// </summary>
        public string OrgCityName
        {
            get
            {
                return orgCityName;
            }
            set
            {
                orgCityName = value;
            }
        }

        /// <summary>
        /// 户口所在地区县全称
        /// </summary>
        public string OrgDistrictName
        {
            get
            {
                return orgDistrictName;
            }
            set
            {
                orgDistrictName = value;
            }
        }

        /// <summary>
        /// 户口所在地镇(街道)全称
        /// </summary>
        public string OrgTown
        {
            get
            {
                return orgTown;
            }
            set
            {
                orgTown = value;
            }
        }

        /// <summary>
        /// 户口所在地村全称
        /// </summary>
        public string OrgVillageName
        {
            get
            {
                return orgVillageName;
            }
            set
            {
                orgVillageName = value;
            }
        }

        /// <summary>
        /// 现住址省份编码
        /// </summary>
        public string XZZProvinceId
        {
            get
            {
                return xZZProvinceId;
            }
            set
            {
                xZZProvinceId = value;
            }
        }

        /// <summary>
        /// 现住址市编码
        /// </summary>
        public string XZZCityId
        {
            get
            {
                return xZZCityId;
            }
            set
            {
                xZZCityId = value;
            }
        }

        /// <summary>
        /// 现住址区县编码
        /// </summary>
        public string XZZDistrictId
        {
            get
            {
                return xZZDistrictId;
            }
            set
            {
                xZZDistrictId = value;
            }
        }

        /// <summary>
        /// 现住址镇(街道)编码
        /// </summary>
        public string XZZTownId
        {
            get
            {
                return xZZTownId;
            }
            set
            {
                xZZTownId = value;
            }
        }

        /// <summary>
        /// 现住址所在村编码
        /// </summary>
        public string XZZVillageId
        {
            get
            {
                return xZZVillageId;
            }
            set
            {
                xZZVillageId = value;
            }
        }

        /// <summary>
        ///现住址所在地省份全称 
        /// </summary>
        public string XZZProvince
        {
            get
            {
                return xZZProvince;
            }
            set
            {
                xZZProvince = value;
            }
        }

        /// <summary>
        /// 现住址所在地市全称
        /// </summary>
        public string XZZCity
        {
            get
            {
                return xZZCity;
            }
            set
            {
                xZZCity = value;
            }
        }

        /// <summary>
        /// 现住址所在地区县全称
        /// </summary>
        public string XZZDistrict
        {
            get
            {
                return xZZDistrict;
            }
            set
            {
                xZZDistrict = value;
            }
        }

        /// <summary>
        /// 现住址所在地镇(街道)全称
        /// </summary>
        public string XZZTown
        {
            get
            {
                return xZZTown;
            }
            set
            {
                xZZTown = value;
            }
        }

        /// <summary>
        /// 现住址所在地村全称
        /// </summary>
        public string XZZVillage
        {
            get
            {
                return xZZVillage;
            }
            set
            {
                xZZVillage = value;
            }
        }

        /// <summary>
        /// 诊断
        /// </summary>
        public string Report_Diagnosis
        {
            get
            {
                return report_Diagnosis;
            }
            set
            {
                report_Diagnosis = value;
            }
        }

        /// <summary>
        /// 病理类型
        /// </summary>
        public string PathologicalType
        {
            get
            {
                return pathologicalType;
            }
            set
            {
                pathologicalType = value;
            }
        }

        /// <summary>
        /// 病理诊断编码
        /// </summary>
        public string PathologicalId
        {
            get
            {
                return pathologicalId;
            }
            set
            {
                pathologicalId = value;
            }
        }

        /// <summary>
        /// 确诊时期_T期
        /// </summary>
        public string QZDigTime_T
        {
            get
            {
                return qZDiagTime_T;
            }
            set
            {
                qZDiagTime_T = value;
            }
        }

        /// <summary>
        /// 确诊时期_N期
        /// </summary>
        public string QZDiaTime_N
        {
            get
            {
                return qZDiagTime_N;
            }
            set
            {
                qZDiagTime_N = value;
            }
        }

        /// <summary>
        /// 确诊时期_M期
        /// </summary>
        public string QZDigTime_M
        {
            get
            {
                return qZDiagTime_M;
            }
            set
            {
                qZDiagTime_M = value;
            }
        }

        /// <summary>
        /// 首次确诊日期
        /// </summary>
        public string FirstDiaDate
        {
            get
            {
                return firstDiaDate;
            }
            set
            {
                firstDiaDate = value;
            }
        }
        private string _diagyear;
        public string DiagYear
        {
            get
            {
                _diagyear = string.IsNullOrEmpty(firstDiaDate) ? "" : Convert.ToDateTime(firstDiaDate).ToString("yyyy");
                return _diagyear;
            }
        }
        private string _diagmonth;
        public string DiagMonth
        {
            get
            {
                _diagmonth = string.IsNullOrEmpty(firstDiaDate) ? "" : Convert.ToDateTime(firstDiaDate).ToString("MM");
                return _diagmonth;
            }
        }
        private string _diagday;
        public string DiagDay
        {
            get
            {
                _diagday = string.IsNullOrEmpty(firstDiaDate) ? "" : Convert.ToDateTime(firstDiaDate).ToString("dd");
                return _diagday;
            }
        }



        /// <summary>
        /// 报告单位
        /// </summary>
        public string ReportInfunit
        {
            get
            {
                return reportinfunit;
            }
            set
            {
                reportinfunit = value;
            }
        }

        /// <summary>
        /// 报告医生
        /// </summary>
        public string ReportDoctor
        {
            get
            {
                return reportDoctor;
            }
            set
            {
                reportDoctor = value;
            }
        }

        /// <summary>
        /// 报告时间
        /// </summary>
        public string ReportDate
        {
            get
            {
                return reportDate;
            }
            set
            {
                reportDate = value;
            }
        }

        private string _reportDateTear;

        /// <summary>
        /// 报告日期之年份
        /// </summary>
        public string ReportDateYear
        {
            get
            {
                _reportDateTear = string.IsNullOrEmpty(reportDate) ? "" : Convert.ToDateTime(reportDate).ToString("yyyy");
                return _reportDateTear;
            }
        }

        private string _reportDateMonth;

        /// <summary>
        /// 报告日期之月份
        /// </summary>
        public string ReportDateMonth
        {
            get
            {
                _reportDateMonth = string.IsNullOrEmpty(reportDate) ? "" : Convert.ToDateTime(reportDate).ToString("MM");
                return _reportDateMonth;
            }
        }

        private string _reportDateDay;

        /// <summary>
        /// 报告日期之天
        /// </summary>
        public string ReportDateDay
        {
            get
            {
                _reportDateDay = string.IsNullOrEmpty(reportDate) ? "" : Convert.ToDateTime(reportDate).ToString("dd");
                return _reportDateDay;
            }
        }

        /// <summary>
        /// 死亡时间
        /// </summary>
        public string DeathDate
        {
            get
            {
                return deathDate;
            }
            set
            {
                deathDate = value;
            }
        }

        private string _deathDateYear;

        /// <summary>
        /// 死亡时间之年份
        /// </summary>
        public string DeathDateYear
        {
            get
            {
                _deathDateYear = string.IsNullOrEmpty(deathDate) ? "" : Convert.ToDateTime(deathDate).ToString("yyyy");
                return _deathDateYear;
            }
        }

        private string _deathDateMonth;

        /// <summary>
        /// 死亡时间之月份
        /// </summary>
        public string DeathDateMonth
        {
            get
            {
                _deathDateMonth = string.IsNullOrEmpty(deathDate) ?
                "" : Convert.ToDateTime(deathDate).ToString("MM");
                return _deathDateMonth;
            }
        }

        private string _deathDateDay;

        /// <summary>
        /// 死亡时间之天
        /// </summary>
        public string DeathDateDay
        {
            get
            {
                _deathDateDay = string.IsNullOrEmpty(deathDate) ?
                "" : Convert.ToDateTime(deathDate).ToString("dd");
                return _deathDateDay;
            }
        }

        /// <summary>
        /// 根本死因
        /// </summary>
        public string DeathReason
        {
            get
            {
                return deathReason;
            }
            set
            {
                deathReason = value;
            }
        }

        /// <summary>
        /// 病历摘要
        /// </summary>
        public string ReJest
        {
            get
            {
                return reJest;
            }
            set
            {
                reJest = value;
            }
        }



        /// <summary>
        /// 报告卡类型
        /// </summary>
        public string ReportCardType
        {
            get
            {
                return reportCardType;
            }
            set
            {
                reportCardType = value;
            }
        }
        /// <summary>
        /// 原诊断
        /// </summary>
        public string report_YdiagNosis;
        /// <summary>
        /// 原诊断
        /// </summary>
        public string Report_YdiagNosis
        {
            get
            {
                return report_YdiagNosis;
            }
            set
            {
                report_YdiagNosis = value;
            }
        }

        /// <summary>
        /// 原诊断日期
        /// </summary>
        public string report_YdiagNosisData;

        /// <summary>
        /// 原诊断日期
        /// </summary>
        public string Report_YdiagNosisData
        {
            get
            {
                return report_YdiagNosisData;
            }
            set
            {
                report_YdiagNosisData = value;
            }
        }

        /// <summary>
        /// 诊断依据
        /// </summary>
        public string report_DiagNosisBased;

        /// <summary>
        /// 诊断依据
        /// </summary>
        public string Report_DiagNosisBased
        {
            get
            {
                return report_DiagNosisBased;
            }
            set
            {
                report_DiagNosisBased = value;
            }
        }

        private string _martialText;

        /// <summary>
        /// 婚姻状况对应文本
        /// </summary>
        public string MartialText
        {
            get
            {
                return _martialText;
            }
            set
            {
                _martialText = value;
            }
        }

        private string _reportInfunitName;

        private string _occuName;

        /// <summary>
        /// 职业全称
        /// </summary>
        public string OccuName
        {
            get
            {
                return _occuName;
            }
            set
            {
                _occuName = value;
            }
        }

        /// <summary>
        /// 报告单位全称
        /// </summary>
        public string ReportInfunitName
        {
            get
            {
                return _reportInfunitName;
            }
            set
            {
                _reportInfunitName = value;
            }
        }

        private string _reportDoctorName;

        /// <summary>
        /// 报告医师全称
        /// </summary>
        public string ReportDoctorName
        {
            get
            {
                return _reportDoctorName;
            }
            set
            {
                _reportDoctorName = value;
            }
        }

        private string c0;

        /// <summary>
        /// 死亡补发病
        /// </summary>
        public string C0
        {
            get
            {
                string[] array = report_DiagNosisBased.Split(',');
                c0 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "0":
                            c0 = "√";
                            break;
                    }
                }
                return c0;
            }

        }

        //增加发病卡和死亡卡的标识属性 add by ywk 2013年8月6日 18:28:09
        private string c10;//发病卡

        public string C10
        {
            get
            {
                if (ReportCardType == "0")//发病 
                {
                    c10 = "√";
                }
                return c10;
            }

        }

        private string c11;// 死亡卡

        public string C11
        {
            get
            {
                if (ReportCardType == "1")//死亡
                {
                    c11 = "√";
                }
                return c11;
            }

        }


        private string c1;

        /// <summary>
        /// 临床
        /// </summary>
        public string C1
        {
            get
            {
                string[] array = report_DiagNosisBased.Split(',');
                c1 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "1":
                            c1 = "√";
                            break;
                    }
                }
                return c1;
            }
        }

        private string c2;

        /// <summary>
        /// X线、CT、超声、内窥镜
        /// </summary>
        public string C2
        {
            get
            {
                string[] array = report_DiagNosisBased.Split(',');
                c2 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "2":
                            c2 = "√";
                            break;
                    }
                }
                return c2;
            }
        }

        private string c3;

        /// <summary>
        /// 手术、尸检(无病理)
        /// </summary>
        public string C3
        {
            get
            {
                string[] array = report_DiagNosisBased.Split(',');
                c3 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "3":
                            c3 = "√";
                            break;
                    }
                }
                return c3;
            }
        }

        private string c4;

        /// <summary>
        /// 生化、免疫
        /// </summary>
        public string C4
        {
            get
            {
                string[] array = report_DiagNosisBased.Split(',');
                c4 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "4":
                            c4 = "√";
                            break;
                    }
                }
                return c4;
            }
        }

        private string c5;

        /// <summary>
        /// 细胞学、血片
        /// </summary>
        public string C5
        {
            get
            {
                string[] array = report_DiagNosisBased.Split(',');
                c5 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "5":
                            c5 = "√";
                            break;
                    }
                }
                return c5;
            }
        }

        private string c6;

        /// <summary>
        /// 病理(继发)
        /// </summary>
        public string C6
        {
            get
            {
                string[] array = report_DiagNosisBased.Split(',');
                c6 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "6":
                            c6 = "√";
                            break;
                    }
                }
                return c6;
            }
        }

        private string c7;

        /// <summary>
        /// 病理(原发)
        /// </summary>
        public string C7
        {
            get
            {
                string[] array = report_DiagNosisBased.Split(',');
                c7 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "7":
                            c7 = "√";
                            break;
                    }
                }
                return c7;
            }
        }

        private string c8;

        /// <summary>
        /// 尸检(有病理)
        /// </summary>
        public string C8
        {
            get
            {
                string[] array = report_DiagNosisBased.Split(',');
                c8 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "8":
                            c8 = "√";
                            break;
                    }
                }
                return c8;
            }
        }

        private string c9;

        /// <summary>
        /// 不详
        /// </summary>
        public string C9
        {
            get
            {
                string[] array = report_DiagNosisBased.Split(',');
                c9 = string.Empty;
                foreach (var arr in array)
                {
                    if (string.IsNullOrEmpty(arr))
                    {
                        continue;
                    }
                    switch (arr)
                    {
                        case "9":
                            c9 = "√";
                            break;
                    }
                }
                return c9;
            }
        }

        /// <summary>
        /// 报告状态【 1、新增保存 2、提交 3、撤回 4、审核通过 5、审核未通过撤回 6、上报	7、作废】
        /// </summary>
        public string State
        {
            get
            {
                return m_State;
            }
            set
            {
                m_State = value;
            }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate
        {
            get
            {
                return m_CreateDate;
            }
            set
            {
                m_CreateDate = value;
            }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUsercode
        {
            get
            {
                return m_CreateUsercode;
            }
            set
            {
                m_CreateUsercode = value;
            }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUsername
        {
            get
            {
                return m_CreateUsername;
            }
            set
            {
                m_CreateUsername = value;
            }
        }

        /// <summary>
        /// 创建人科室
        /// </summary>
        public string CreateDeptcode
        {
            get
            {
                return m_CreateDeptcode;
            }
            set
            {
                m_CreateDeptcode = value;
            }
        }

        /// <summary>
        /// 创建人科室
        /// </summary>
        public string CreateDeptname
        {
            get
            {
                return m_CreateDeptname;
            }
            set
            {
                m_CreateDeptname = value;
            }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public string ModifyDate
        {
            get
            {
                return m_ModifyDate;
            }
            set
            {
                m_ModifyDate = value;
            }
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyUsercode
        {
            get
            {
                return m_ModifyUsercode;
            }
            set
            {
                m_ModifyUsercode = value;
            }
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyUsername
        {
            get
            {
                return m_ModifyUsername;
            }
            set
            {
                m_ModifyUsername = value;
            }
        }

        /// <summary>
        /// 修改人科室
        /// </summary>
        public string ModifyDeptcode
        {
            get
            {
                return m_ModifyDeptcode;
            }
            set
            {
                m_ModifyDeptcode = value;
            }
        }

        /// <summary>
        /// 修改人科室
        /// </summary>
        public string ModifyDeptname
        {
            get
            {
                return m_ModifyDeptname;
            }
            set
            {
                m_ModifyDeptname = value;
            }
        }

        /// <summary>
        /// 审核时间
        /// </summary>
        public string AuditDate
        {
            get
            {
                return m_AuditDate;
            }
            set
            {
                m_AuditDate = value;
            }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUsercode
        {
            get
            {
                return m_AuditUsercode;
            }
            set
            {
                m_AuditUsercode = value;
            }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditUsername
        {
            get
            {
                return m_AuditUsername;
            }
            set
            {
                m_AuditUsername = value;
            }
        }

        /// <summary>
        /// 审核人科室
        /// </summary>
        public string AuditDeptcode
        {
            get
            {
                return m_AuditDeptcode;
            }
            set
            {
                m_AuditDeptcode = value;
            }
        }

        /// <summary>
        /// 审核人科室
        /// </summary>
        public string AuditDeptname
        {
            get
            {
                return m_AuditDeptname;
            }
            set
            {
                m_AuditDeptname = value;
            }
        }

        /// <summary>
        /// 状态是否有效  1、有效   0、无效
        /// </summary>
        public string Vaild
        {
            get
            {
                return m_Vaild;
            }
            set
            {
                m_Vaild = value;
            }
        }

        private string _DIAGICD10 = string.Empty;
        /// <summary>
        /// 传染病病种(对应传染病诊断库)
        /// </summary>
        public string DIAGICD10
        {
            get
            {
                return _DIAGICD10;
            }
            set
            {
                _DIAGICD10 = value;
            }
        }

        private string _CANCELREASON = string.Empty;
        /// <summary>
        /// 否决原因
        /// </summary>
        public string CANCELREASON
        {
            get
            {
                return _CANCELREASON;
            }
            set
            {
                _CANCELREASON = value;
            }
        }      

    }
}
