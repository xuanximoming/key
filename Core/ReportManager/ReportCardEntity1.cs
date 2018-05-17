using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core.ReportManager
{
    /// <summary>
    /// <date>2013-08-19</date>
    /// 报告卡对应实体
    /// <auth>jxh</auth>
    /// </summary>
    public class ReportCardEntity1
    {

        #region declaration
        private string m_Id = "0";
        private string m_ReportNoofinpat = String.Empty;
        private string m_ReportId = String.Empty;
        private string m_DiagCode = String.Empty;
        private string m_String3 = String.Empty;
        private string m_String4 = String.Empty;
        private string m_String5 = String.Empty;
        private string m_ReportProvince = String.Empty;
        private string m_ReportCity = String.Empty;
        private string m_ReportTown = String.Empty;
        private string m_ReportVillage = String.Empty;
        private string m_ReportHospital = String.Empty;
        private string m_ReportNo = String.Empty;
        private string m_MotherPatid = String.Empty;
        private string m_MotherName = String.Empty;
        private string m_MotherAge = String.Empty;
        private string m_National = String.Empty;
        private string m_AddressPost = String.Empty;
        private string m_Pregnantno = String.Empty;
        private string m_Productionno = String.Empty;
        private string m_Localadd = String.Empty;
        private string m_Percapitaincome = String.Empty;
        private string m_Educationlevel = String.Empty;
        private string m_ChildPatid = String.Empty;
        private string m_ChildName = String.Empty;
        private string m_Isbornhere = String.Empty;
        private string m_ChildSex = String.Empty;
        private string m_BornYear = String.Empty;
        private string m_BornMonth = String.Empty;
        private string m_BornDay = String.Empty;
        private string m_Gestationalage = String.Empty;
        private string m_Weight = String.Empty;
        private string m_Births = String.Empty;
        private string m_Isidentical = String.Empty;
        private string m_Outcome = String.Empty;
        private string m_Inducedlabor = String.Empty;
        private string m_Diagnosticbasis = String.Empty;
        private string m_Diagnosticbasis1 = String.Empty;
        private string m_Diagnosticbasis2 = String.Empty;
        private string m_Diagnosticbasis3 = String.Empty;
        private string m_Diagnosticbasis4 = String.Empty;
        private string m_Diagnosticbasis5 = String.Empty;
        private string m_Diagnosticbasis6 = String.Empty;
        private string m_Diagnosticbasis7 = String.Empty;
        private string m_DiagAnencephaly = String.Empty;
        private string m_DiagSpina = String.Empty;
        private string m_DiagPengout = String.Empty;
        private string m_DiagHydrocephalus = String.Empty;
        private string m_DiagCleftpalate = String.Empty;
        private string m_DiagCleftlip = String.Empty;
        private string m_DiagCleftmerger = String.Empty;
        private string m_DiagSmallears = String.Empty;
        private string m_DiagOuterear = String.Empty;
        private string m_DiagEsophageal = String.Empty;
        private string m_DiagRectum = String.Empty;
        private string m_DiagHypospadias = String.Empty;
        private string m_DiagBladder = String.Empty;
        private string m_DiagHorseshoefootleft = String.Empty;
        private string m_DiagHorseshoefootright = String.Empty;
        private string m_DiagManypointleft = String.Empty;
        private string m_DiagManypointright = String.Empty;
        private string m_DiagLimbsupperleft = String.Empty;
        private string m_DiagLimbsupperright = String.Empty;
        private string m_DiagLimbslowerleft = String.Empty;
        private string m_DiagLimbslowerright = String.Empty;
        private string m_DiagHernia = String.Empty;
        private string m_DiagBulgingbelly = String.Empty;
        private string m_DiagGastroschisis = String.Empty;
        private string m_DiagTwins = String.Empty;
        private string m_DiagTssyndrome = String.Empty;
        private string m_DiagHeartdisease = String.Empty;
        private string m_DiagOther = String.Empty;
        private string m_DiagOthercontent = String.Empty;
        private string m_Fever = String.Empty;
        private string m_Virusinfection = String.Empty;
        private string m_Illother = String.Empty;
        private string m_Sulfa = String.Empty;
        private string m_Antibiotics = String.Empty;
        private string m_Birthcontrolpill = String.Empty;
        private string m_Sedatives = String.Empty;
        private string m_Medicineother = String.Empty;
        private string m_Drinking = String.Empty;
        private string m_Pesticide = String.Empty;
        private string m_Ray = String.Empty;
        private string m_Chemicalagents = String.Empty;
        private string m_Factorother = String.Empty;
        private string m_Stillbirthno = String.Empty;
        private string m_Abortionno = String.Empty;
        private string m_Defectsno = String.Empty;
        private string m_Defectsof1 = String.Empty;
        private string m_Defectsof2 = String.Empty;
        private string m_Defectsof3 = String.Empty;
        private string m_Ycdefectsof1 = String.Empty;
        private string m_Ycdefectsof2 = String.Empty;
        private string m_Ycdefectsof3 = String.Empty;
        private string m_Kinshipdefects1 = String.Empty;
        private string m_Kinshipdefects2 = String.Empty;
        private string m_Kinshipdefects3 = String.Empty;
        private string m_Cousinmarriage = String.Empty;
        private string m_Cousinmarriagebetween = String.Empty;
        private string m_Preparer = String.Empty;
        private string m_Thetitle1 = String.Empty;
        private string m_Filldateyear = String.Empty;
        private string m_Filldatemonth = String.Empty;
        private string m_Filldateday = String.Empty;
        private string m_Hospitalreview = String.Empty;
        private string m_Thetitle2 = String.Empty;
        private string m_Hospitalauditdateyear = String.Empty;
        private string m_Hospitalauditdatemonth = String.Empty;
        private string m_Hospitalauditdateday = String.Empty;
        private string m_Provincialview = String.Empty;
        private string m_Thetitle3 = String.Empty;
        private string m_Provincialviewdateyear = String.Empty;
        private string m_Provincialviewdatemonth = String.Empty;
        private string m_Provincialviewdateday = String.Empty;
        private string m_Feverno = String.Empty;
        private string m_Isvirusinfection = String.Empty;
        private string m_Isdiabetes = String.Empty;
        private string m_Isillother = String.Empty;
        private string m_Issulfa = String.Empty;
        private string m_Isantibiotics = String.Empty;
        private string m_Isbirthcontrolpill = String.Empty;
        private string m_Issedatives = String.Empty;
        private string m_Ismedicineother = String.Empty;
        private string m_Isdrinking = String.Empty;
        private string m_Ispesticide = String.Empty;
        private string m_Isray = String.Empty;
        private string m_Ischemicalagents = String.Empty;
        private string m_Isfactorother = String.Empty;
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
        private string m_Cancelreason = String.Empty;
        private string m_Prenatal = String.Empty;
        private string m_Prenatalno = String.Empty;
        private string m_Postpartum = String.Empty;
        private string m_Andtoshowleft = String.Empty;
        private string m_Andtoshowright = String.Empty;
        #endregion declaration



        #region Properties
        ///<summary>
        ///序号
        ///</summary>
        public string Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                m_Id = value;
            }
        }
        ///<summary>
        ///病案首页编号
        ///</summary>
        public string ReportNoofinpat
        {
            get
            {
                return m_ReportNoofinpat;
            }
            set
            {
                m_ReportNoofinpat = value;
            }
        }
        ///<summary>
        ///报告卡序号
        ///</summary>
        public string ReportId
        {
            get
            {
                return m_ReportId;
            }
            set
            {
                m_ReportId = value;
            }
        }
        ///<summary>
        ///病种编号
        ///</summary>
        public string DiagCode
        {
            get
            {
                return m_DiagCode;
            }
            set
            {
                m_DiagCode = value;
            }
        }
        ///<summary>
        ///预留4
        ///</summary>
        public string String3
        {
            get
            {
                return m_String3;
            }
            set
            {
                m_String3 = value;
            }
        }
        ///<summary>
        ///预留5
        ///</summary>
        public string String4
        {
            get
            {
                return m_String4;
            }
            set
            {
                m_String4 = value;
            }
        }
        
        ///<summary>
        ///预留6
        ///</summary>
        public string String5
        {
            get
            {
                return m_String5;
            }
            set
            {
                m_String5 = value;
            }
        }
        ///<summary>
        ///报告卡省份
        ///</summary>
        public string ReportProvince
        {
            get
            {
                return m_ReportProvince;
            }
            set
            {
                m_ReportProvince = value;
            }
        }
        ///<summary>
        ///报告卡市（县）
        ///</summary>
        public string ReportCity
        {
            get
            {
                return m_ReportCity;
            }
            set
            {
                m_ReportCity = value;
            }
        }
        ///<summary>
        ///报告卡乡镇
        ///</summary>
        public string ReportTown
        {
            get
            {
                return m_ReportTown;
            }
            set
            {
                m_ReportTown = value;
            }
        }
        ///<summary>
        ///报告卡村
        ///</summary>
        public string ReportVillage
        {
            get
            {
                return m_ReportVillage;
            }
            set
            {
                m_ReportVillage = value;
            }
        }
        ///<summary>
        ///报告卡医院
        ///</summary>
        public string ReportHospital
        {
            get
            {
                return m_ReportHospital;
            }
            set
            {
                m_ReportHospital = value;
            }
        }
        ///<summary>
        ///右上方不明框框
        ///</summary>
        public string ReportNo
        {
            get
            {
                return m_ReportNo;
            }
            set
            {
                m_ReportNo = value;
            }
        }
        ///<summary>
        ///产妇住院号
        ///</summary>
        public string MotherPatid
        {
            get
            {
                return m_MotherPatid;
            }
            set
            {
                m_MotherPatid = value;
            }
        }
        ///<summary>
        ///姓名
        ///</summary>
        public string MotherName
        {
            get
            {
                return m_MotherName;
            }
            set
            {
                m_MotherName = value;
            }
        }
        ///<summary>
        ///年龄
        ///</summary>
        public string MotherAge
        {
            get
            {
                return m_MotherAge;
            }
            set
            {
                m_MotherAge = value;
            }
        }
        ///<summary>
        ///民族
        ///</summary>
        public string National
        {
            get
            {
                return m_National;
            }
            set
            {
                m_National = value;
            }
        }
        /// <summary>
        /// 民族名称
        /// </summary>
        private string m_nationName = String.Empty;
        public string NationName
        {
            get
            {
                return m_nationName;
            }
            set
            {
                m_nationName = value;
            }
        }
        ///<summary>
        ///地址and邮编
        ///</summary>
        public string AddressPost
        {
            get
            {
                return m_AddressPost;
            }
            set
            {
                m_AddressPost = value;
            }
        }
        ///<summary>
        ///孕次
        ///</summary>
        public string Pregnantno
        {
            get
            {
                return m_Pregnantno;
            }
            set
            {
                m_Pregnantno = value;
            }
        }
        ///<summary>
        ///产次
        ///</summary>
        public string Productionno
        {
            get
            {
                return m_Productionno;
            }
            set
            {
                m_Productionno = value;
            }
        }
        ///<summary>
        ///常住地
        ///</summary>
        public string Localadd
        {
            get
            {
                return m_Localadd;
            }
            set
            {
                m_Localadd = value;
            }
        }
        ///<summary>
        ///年人均收入
        ///</summary>
        public string Percapitaincome
        {
            get
            {
                return m_Percapitaincome;
            }
            set
            {
                m_Percapitaincome = value;
            }
        }
        ///<summary>
        ///文化程度
        ///</summary>
        public string Educationlevel
        {
            get
            {
                return m_Educationlevel;
            }
            set
            {
                m_Educationlevel = value;
            }
        }
        ///<summary>
        ///孩子住院号
        ///</summary>
        public string ChildPatid
        {
            get
            {
                return m_ChildPatid;
            }
            set
            {
                m_ChildPatid = value;
            }
        }
        ///<summary>
        ///孩子姓名
        ///</summary>
        public string ChildName
        {
            get
            {
                return m_ChildName;
            }
            set
            {
                m_ChildName = value;
            }
        }
        ///<summary>
        ///是否本院出生
        ///</summary>
        public string Isbornhere
        {
            get
            {
                return m_Isbornhere;
            }
            set
            {
                m_Isbornhere = value;
            }
        }
        ///<summary>
        ///孩子性别
        ///</summary>
        public string ChildSex
        {
            get
            {
                return m_ChildSex;
            }
            set
            {
                m_ChildSex = value;
            }
        }
        ///<summary>
        ///出生年
        ///</summary>
        public string BornYear
        {
            get
            {
                return m_BornYear;
            }
            set
            {
                m_BornYear = value;
            }
        }
        ///<summary>
        ///出生月
        ///</summary>
        public string BornMonth
        {
            get
            {
                return m_BornMonth;
            }
            set
            {
                m_BornMonth = value;
            }
        }
        ///<summary>
        ///出生日
        ///</summary>
        public string BornDay
        {
            get
            {
                return m_BornDay;
            }
            set
            {
                m_BornDay = value;
            }
        }
        ///<summary>
        ///胎龄
        ///</summary>
        public string Gestationalage
        {
            get
            {
                return m_Gestationalage;
            }
            set
            {
                m_Gestationalage = value;
            }
        }
        ///<summary>
        ///体重
        ///</summary>
        public string Weight
        {
            get
            {
                return m_Weight;
            }
            set
            {
                m_Weight = value;
            }
        }
        ///<summary>
        ///胎数
        ///</summary>
        public string Births
        {
            get
            {
                return m_Births;
            }
            set
            {
                m_Births = value;
            }
        }
        ///<summary>
        ///是否同卵
        ///</summary>
        public string Isidentical
        {
            get
            {
                return m_Isidentical;
            }
            set
            {
                m_Isidentical = value;
            }
        }
        ///<summary>
        ///转归
        ///</summary>
        public string Outcome
        {
            get
            {
                return m_Outcome;
            }
            set
            {
                m_Outcome = value;
            }
        }
        ///<summary>
        ///是否引产
        ///</summary>
        public string Inducedlabor
        {
            get
            {
                return m_Inducedlabor;
            }
            set
            {
                m_Inducedlabor = value;
            }
        }
        ///<summary>
        ///诊断依据——临床
        ///</summary>
        public string Diagnosticbasis
        {
            get
            {
                return m_Diagnosticbasis;
            }
            set
            {
                m_Diagnosticbasis = value;
            }
        }
        ///<summary>
        ///诊断依据——超声波
        ///</summary>
        public string Diagnosticbasis1
        {
            get
            {
                return m_Diagnosticbasis1;
            }
            set
            {
                m_Diagnosticbasis1 = value;
            }
        }
        ///<summary>
        ///诊断依据——尸解
        ///</summary>
        public string Diagnosticbasis2
        {
            get
            {
                return m_Diagnosticbasis2;
            }
            set
            {
                m_Diagnosticbasis2 = value;
            }
        }
        ///<summary>
        ///诊断依据——生化检查
        ///</summary>
        public string Diagnosticbasis3
        {
            get
            {
                return m_Diagnosticbasis3;
            }
            set
            {
                m_Diagnosticbasis3 = value;
            }
        }
        ///<summary>
        ///诊断依据——生化检查——其他
        ///</summary>
        public string Diagnosticbasis4
        {
            get
            {
                return m_Diagnosticbasis4;
            }
            set
            {
                m_Diagnosticbasis4 = value;
            }
        }
        ///<summary>
        ///诊断依据——染色体
        ///</summary>
        public string Diagnosticbasis5
        {
            get
            {
                return m_Diagnosticbasis5;
            }
            set
            {
                m_Diagnosticbasis5 = value;
            }
        }
        ///<summary>
        ///诊断依据——其他
        ///</summary>
        public string Diagnosticbasis6
        {
            get
            {
                return m_Diagnosticbasis6;
            }
            set
            {
                m_Diagnosticbasis6 = value;
            }
        }
        ///<summary>
        ///诊断依据——其他——内容
        ///</summary>
        public string Diagnosticbasis7
        {
            get
            {
                return m_Diagnosticbasis7;
            }
            set
            {
                m_Diagnosticbasis7 = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——无脑畸形
        ///</summary>
        public string DiagAnencephaly
        {
            get
            {
                return m_DiagAnencephaly;
            }
            set
            {
                m_DiagAnencephaly = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——脊柱裂
        ///</summary>
        public string DiagSpina
        {
            get
            {
                return m_DiagSpina;
            }
            set
            {
                m_DiagSpina = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——脑彭出
        ///</summary>
        public string DiagPengout
        {
            get
            {
                return m_DiagPengout;
            }
            set
            {
                m_DiagPengout = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——先天性脑积水
        ///</summary>
        public string DiagHydrocephalus
        {
            get
            {
                return m_DiagHydrocephalus;
            }
            set
            {
                m_DiagHydrocephalus = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——腭裂
        ///</summary>
        public string DiagCleftpalate
        {
            get
            {
                return m_DiagCleftpalate;
            }
            set
            {
                m_DiagCleftpalate = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——唇裂
        ///</summary>
        public string DiagCleftlip
        {
            get
            {
                return m_DiagCleftlip;
            }
            set
            {
                m_DiagCleftlip = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——唇裂合并腭裂
        ///</summary>
        public string DiagCleftmerger
        {
            get
            {
                return m_DiagCleftmerger;
            }
            set
            {
                m_DiagCleftmerger = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——小耳（包括无耳）
        ///</summary>
        public string DiagSmallears
        {
            get
            {
                return m_DiagSmallears;
            }
            set
            {
                m_DiagSmallears = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——外耳其它畸形（小耳、无耳除外）
        ///</summary>
        public string DiagOuterear
        {
            get
            {
                return m_DiagOuterear;
            }
            set
            {
                m_DiagOuterear = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——食道闭锁或狭窄
        ///</summary>
        public string DiagEsophageal
        {
            get
            {
                return m_DiagEsophageal;
            }
            set
            {
                m_DiagEsophageal = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——直肠肛门闭锁或狭窄（包括无肛）
        ///</summary>
        public string DiagRectum
        {
            get
            {
                return m_DiagRectum;
            }
            set
            {
                m_DiagRectum = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——尿道下裂
        ///</summary>
        public string DiagHypospadias
        {
            get
            {
                return m_DiagHypospadias;
            }
            set
            {
                m_DiagHypospadias = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——膀胱外翻
        ///</summary>
        public string DiagBladder
        {
            get
            {
                return m_DiagBladder;
            }
            set
            {
                m_DiagBladder = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——马蹄内翻足_左 
        ///</summary>
        public string DiagHorseshoefootleft
        {
            get
            {
                return m_DiagHorseshoefootleft;
            }
            set
            {
                m_DiagHorseshoefootleft = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——马蹄内翻足_右
        ///</summary>
        public string DiagHorseshoefootright
        {
            get
            {
                return m_DiagHorseshoefootright;
            }
            set
            {
                m_DiagHorseshoefootright = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——多指（趾）_左
        ///</summary>
        public string DiagManypointleft
        {
            get
            {
                return m_DiagManypointleft;
            }
            set
            {
                m_DiagManypointleft = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——多指（趾）_右
        ///</summary>
        public string DiagManypointright
        {
            get
            {
                return m_DiagManypointright;
            }
            set
            {
                m_DiagManypointright = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——肢体短缩_上肢 _左
        ///</summary>
        public string DiagLimbsupperleft
        {
            get
            {
                return m_DiagLimbsupperleft;
            }
            set
            {
                m_DiagLimbsupperleft = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——肢体短缩_上肢 _右
        ///</summary>
        public string DiagLimbsupperright
        {
            get
            {
                return m_DiagLimbsupperright;
            }
            set
            {
                m_DiagLimbsupperright = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——肢体短缩_下肢 _左
        ///</summary>
        public string DiagLimbslowerleft
        {
            get
            {
                return m_DiagLimbslowerleft;
            }
            set
            {
                m_DiagLimbslowerleft = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——肢体短缩_下肢 _右
        ///</summary>
        public string DiagLimbslowerright
        {
            get
            {
                return m_DiagLimbslowerright;
            }
            set
            {
                m_DiagLimbslowerright = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——先天性膈疝
        ///</summary>
        public string DiagHernia
        {
            get
            {
                return m_DiagHernia;
            }
            set
            {
                m_DiagHernia = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——脐膨出
        ///</summary>
        public string DiagBulgingbelly
        {
            get
            {
                return m_DiagBulgingbelly;
            }
            set
            {
                m_DiagBulgingbelly = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——腹裂
        ///</summary>
        public string DiagGastroschisis
        {
            get
            {
                return m_DiagGastroschisis;
            }
            set
            {
                m_DiagGastroschisis = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——联体双胎
        ///</summary>
        public string DiagTwins
        {
            get
            {
                return m_DiagTwins;
            }
            set
            {
                m_DiagTwins = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——唐氏综合征（21-三体综合征）
        ///</summary>
        public string DiagTssyndrome
        {
            get
            {
                return m_DiagTssyndrome;
            }
            set
            {
                m_DiagTssyndrome = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——先天性心脏病（类型）
        ///</summary>
        public string DiagHeartdisease
        {
            get
            {
                return m_DiagHeartdisease;
            }
            set
            {
                m_DiagHeartdisease = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——其他（写明病名或详细描述）
        ///</summary>
        public string DiagOther
        {
            get
            {
                return m_DiagOther;
            }
            set
            {
                m_DiagOther = value;
            }
        }
        ///<summary>
        ///出生缺陷诊断——其他内容
        ///</summary>
        public string DiagOthercontent
        {
            get
            {
                return m_DiagOthercontent;
            }
            set
            {
                m_DiagOthercontent = value;
            }
        }
        ///<summary>
        ///发烧（＞38℃）
        ///</summary>
        public string Fever
        {
            get
            {
                return m_Fever;
            }
            set
            {
                m_Fever = value;
            }
        }
        ///<summary>
        ///病毒感染
        ///</summary>
        public string Virusinfection
        {
            get
            {
                return m_Virusinfection;
            }
            set
            {
                m_Virusinfection = value;
            }
        }
        ///<summary>
        ///患病其他
        ///</summary>
        public string Illother
        {
            get
            {
                return m_Illother;
            }
            set
            {
                m_Illother = value;
            }
        }
        ///<summary>
        ///磺胺类
        ///</summary>
        public string Sulfa
        {
            get
            {
                return m_Sulfa;
            }
            set
            {
                m_Sulfa = value;
            }
        }
        ///<summary>
        ///抗生素
        ///</summary>
        public string Antibiotics
        {
            get
            {
                return m_Antibiotics;
            }
            set
            {
                m_Antibiotics = value;
            }
        }
        ///<summary>
        ///避孕药
        ///</summary>
        public string Birthcontrolpill
        {
            get
            {
                return m_Birthcontrolpill;
            }
            set
            {
                m_Birthcontrolpill = value;
            }
        }
        ///<summary>
        ///镇静药
        ///</summary>
        public string Sedatives
        {
            get
            {
                return m_Sedatives;
            }
            set
            {
                m_Sedatives = value;
            }
        }
        ///<summary>
        ///服药其他
        ///</summary>
        public string Medicineother
        {
            get
            {
                return m_Medicineother;
            }
            set
            {
                m_Medicineother = value;
            }
        }
        ///<summary>
        ///饮酒
        ///</summary>
        public string Drinking
        {
            get
            {
                return m_Drinking;
            }
            set
            {
                m_Drinking = value;
            }
        }
        ///<summary>
        ///农药
        ///</summary>
        public string Pesticide
        {
            get
            {
                return m_Pesticide;
            }
            set
            {
                m_Pesticide = value;
            }
        }
        ///<summary>
        ///射线
        ///</summary>
        public string Ray
        {
            get
            {
                return m_Ray;
            }
            set
            {
                m_Ray = value;
            }
        }
        ///<summary>
        ///化学制剂
        ///</summary>
        public string Chemicalagents
        {
            get
            {
                return m_Chemicalagents;
            }
            set
            {
                m_Chemicalagents = value;
            }
        }
        ///<summary>
        ///其他有害因素
        ///</summary>
        public string Factorother
        {
            get
            {
                return m_Factorother;
            }
            set
            {
                m_Factorother = value;
            }
        }
        ///<summary>
        ///死胎例数
        ///</summary>
        public string Stillbirthno
        {
            get
            {
                return m_Stillbirthno;
            }
            set
            {
                m_Stillbirthno = value;
            }
        }
        ///<summary>
        ///自然流产例数
        ///</summary>
        public string Abortionno
        {
            get
            {
                return m_Abortionno;
            }
            set
            {
                m_Abortionno = value;
            }
        }
        ///<summary>
        ///缺陷儿例数
        ///</summary>
        public string Defectsno
        {
            get
            {
                return m_Defectsno;
            }
            set
            {
                m_Defectsno = value;
            }
        }
        ///<summary>
        ///缺陷名1
        ///</summary>
        public string Defectsof1
        {
            get
            {
                return m_Defectsof1;
            }
            set
            {
                m_Defectsof1 = value;
            }
        }
        ///<summary>
        ///缺陷名2
        ///</summary>
        public string Defectsof2
        {
            get
            {
                return m_Defectsof2;
            }
            set
            {
                m_Defectsof2 = value;
            }
        }
        ///<summary>
        ///缺陷名3
        ///</summary>
        public string Defectsof3
        {
            get
            {
                return m_Defectsof3;
            }
            set
            {
                m_Defectsof3 = value;
            }
        }
        ///<summary>
        ///遗传缺陷名1
        ///</summary>
        public string Ycdefectsof1
        {
            get
            {
                return m_Ycdefectsof1;
            }
            set
            {
                m_Ycdefectsof1 = value;
            }
        }
        ///<summary>
        ///遗传缺陷名2
        ///</summary>
        public string Ycdefectsof2
        {
            get
            {
                return m_Ycdefectsof2;
            }
            set
            {
                m_Ycdefectsof2 = value;
            }
        }
        ///<summary>
        ///遗传缺陷名3
        ///</summary>
        public string Ycdefectsof3
        {
            get
            {
                return m_Ycdefectsof3;
            }
            set
            {
                m_Ycdefectsof3 = value;
            }
        }
        ///<summary>
        ///与缺陷儿亲缘关系1
        ///</summary>
        public string Kinshipdefects1
        {
            get
            {
                return m_Kinshipdefects1;
            }
            set
            {
                m_Kinshipdefects1 = value;
            }
        }
        ///<summary>
        ///与缺陷儿亲缘关系2
        ///</summary>
        public string Kinshipdefects2
        {
            get
            {
                return m_Kinshipdefects2;
            }
            set
            {
                m_Kinshipdefects2 = value;
            }
        }
        ///<summary>
        ///与缺陷儿亲缘关系3
        ///</summary>
        public string Kinshipdefects3
        {
            get
            {
                return m_Kinshipdefects3;
            }
            set
            {
                m_Kinshipdefects3 = value;
            }
        }
        ///<summary>
        ///近亲婚配史
        ///</summary>
        public string Cousinmarriage
        {
            get
            {
                return m_Cousinmarriage;
            }
            set
            {
                m_Cousinmarriage = value;
            }
        }
        ///<summary>
        ///近亲婚配史关系
        ///</summary>
        public string Cousinmarriagebetween
        {
            get
            {
                return m_Cousinmarriagebetween;
            }
            set
            {
                m_Cousinmarriagebetween = value;
            }
        }
        ///<summary>
        ///填表人
        ///</summary>
        public string Preparer
        {
            get
            {
                return m_Preparer;
            }
            set
            {
                m_Preparer = value;
            }
        }
        ///<summary>
        ///填表人职称
        ///</summary>
        public string Thetitle1
        {
            get
            {
                return m_Thetitle1;
            }
            set
            {
                m_Thetitle1 = value;
            }
        }
        ///<summary>
        ///填表日期年
        ///</summary>
        public string Filldateyear
        {
            get
            {
                return m_Filldateyear;
            }
            set
            {
                m_Filldateyear = value;
            }
        }
        ///<summary>
        ///填表日期月
        ///</summary>
        public string Filldatemonth
        {
            get
            {
                return m_Filldatemonth;
            }
            set
            {
                m_Filldatemonth = value;
            }
        }
        ///<summary>
        ///填表日期日
        ///</summary>
        public string Filldateday
        {
            get
            {
                return m_Filldateday;
            }
            set
            {
                m_Filldateday = value;
            }
        }
        ///<summary>
        ///医院审表人
        ///</summary>
        public string Hospitalreview
        {
            get
            {
                return m_Hospitalreview;
            }
            set
            {
                m_Hospitalreview = value;
            }
        }
        ///<summary>
        ///医院审表人职称
        ///</summary>
        public string Thetitle2
        {
            get
            {
                return m_Thetitle2;
            }
            set
            {
                m_Thetitle2 = value;
            }
        }
        ///<summary>
        ///医院审表日期年
        ///</summary>
        public string Hospitalauditdateyear
        {
            get
            {
                return m_Hospitalauditdateyear;
            }
            set
            {
                m_Hospitalauditdateyear = value;
            }
        }
        ///<summary>
        ///医院审表日期月
        ///</summary>
        public string Hospitalauditdatemonth
        {
            get
            {
                return m_Hospitalauditdatemonth;
            }
            set
            {
                m_Hospitalauditdatemonth = value;
            }
        }
        ///<summary>
        ///医院审表日期日
        ///</summary>
        public string Hospitalauditdateday
        {
            get
            {
                return m_Hospitalauditdateday;
            }
            set
            {
                m_Hospitalauditdateday = value;
            }
        }
        ///<summary>
        ///省级审表人
        ///</summary>
        public string Provincialview
        {
            get
            {
                return m_Provincialview;
            }
            set
            {
                m_Provincialview = value;
            }
        }
        ///<summary>
        ///省级审表人职称
        ///</summary>
        public string Thetitle3
        {
            get
            {
                return m_Thetitle3;
            }
            set
            {
                m_Thetitle3 = value;
            }
        }
        ///<summary>
        ///省级审表日期年
        ///</summary>
        public string Provincialviewdateyear
        {
            get
            {
                return m_Provincialviewdateyear;
            }
            set
            {
                m_Provincialviewdateyear = value;
            }
        }
        ///<summary>
        ///省级审表日期月
        ///</summary>
        public string Provincialviewdatemonth
        {
            get
            {
                return m_Provincialviewdatemonth;
            }
            set
            {
                m_Provincialviewdatemonth = value;
            }
        }
        ///<summary>
        ///省级审表日期日
        ///</summary>
        public string Provincialviewdateday
        {
            get
            {
                return m_Provincialviewdateday;
            }
            set
            {
                m_Provincialviewdateday = value;
            }
        }
        ///<summary>
        ///发烧度数
        ///</summary>
        public string Feverno
        {
            get
            {
                return m_Feverno;
            }
            set
            {
                m_Feverno = value;
            }
        }
        ///<summary>
        ///是否病毒感染
        ///</summary>
        public string Isvirusinfection
        {
            get
            {
                return m_Isvirusinfection;
            }
            set
            {
                m_Isvirusinfection = value;
            }
        }
        ///<summary>
        ///是否糖尿病
        ///</summary>
        public string Isdiabetes
        {
            get
            {
                return m_Isdiabetes;
            }
            set
            {
                m_Isdiabetes = value;
            }
        }
        ///<summary>
        ///是否患病其他
        ///</summary>
        public string Isillother
        {
            get
            {
                return m_Isillother;
            }
            set
            {
                m_Isillother = value;
            }
        }
        ///<summary>
        ///是否磺胺类
        ///</summary>
        public string Issulfa
        {
            get
            {
                return m_Issulfa;
            }
            set
            {
                m_Issulfa = value;
            }
        }
        ///<summary>
        ///是否抗生素
        ///</summary>
        public string Isantibiotics
        {
            get
            {
                return m_Isantibiotics;
            }
            set
            {
                m_Isantibiotics = value;
            }
        }
        ///<summary>
        ///是否避孕药
        ///</summary>
        public string Isbirthcontrolpill
        {
            get
            {
                return m_Isbirthcontrolpill;
            }
            set
            {
                m_Isbirthcontrolpill = value;
            }
        }
        ///<summary>
        ///是否镇静药
        ///</summary>
        public string Issedatives
        {
            get
            {
                return m_Issedatives;
            }
            set
            {
                m_Issedatives = value;
            }
        }
        ///<summary>
        ///是否服药其他
        ///</summary>
        public string Ismedicineother
        {
            get
            {
                return m_Ismedicineother;
            }
            set
            {
                m_Ismedicineother = value;
            }
        }
        ///<summary>
        ///是否饮酒
        ///</summary>
        public string Isdrinking
        {
            get
            {
                return m_Isdrinking;
            }
            set
            {
                m_Isdrinking = value;
            }
        }
        ///<summary>
        ///是否农药
        ///</summary>
        public string Ispesticide
        {
            get
            {
                return m_Ispesticide;
            }
            set
            {
                m_Ispesticide = value;
            }
        }
        ///<summary>
        ///是否射线
        ///</summary>
        public string Isray
        {
            get
            {
                return m_Isray;
            }
            set
            {
                m_Isray = value;
            }
        }
        ///<summary>
        ///是否化学制剂
        ///</summary>
        public string Ischemicalagents
        {
            get
            {
                return m_Ischemicalagents;
            }
            set
            {
                m_Ischemicalagents = value;
            }
        }
        ///<summary>
        ///是否其他有害因素
        ///</summary>
        public string Isfactorother
        {
            get
            {
                return m_Isfactorother;
            }
            set
            {
                m_Isfactorother = value;
            }
        }
        ///<summary>
        ///""""报告状态【 1、新增保存 2、提交 3、撤回 4、?to open this dialog next """
        ///</summary>
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
        ///<summary>
        ///创建时间
        ///</summary>
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
        ///<summary>
        ///创建人
        ///</summary>
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
        ///<summary>
        ///创建人
        ///</summary>
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
        ///<summary>
        ///创建人科室
        ///</summary>
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
        ///<summary>
        ///创建人科室
        ///</summary>
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
        ///<summary>
        ///修改时间
        ///</summary>
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
        ///<summary>
        ///修改人
        ///</summary>
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
        ///<summary>
        ///修改人
        ///</summary>
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
        ///<summary>
        ///修改人科室
        ///</summary>
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
        ///<summary>
        ///修改人科室
        ///</summary>
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
        ///<summary>
        ///审核时间
        ///</summary>
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
        ///<summary>
        ///审核人
        ///</summary>
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
        ///<summary>
        ///审核人
        ///</summary>
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
        ///<summary>
        ///审核人科室
        ///</summary>
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
        ///<summary>
        ///审核人科室
        ///</summary>
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
        ///<summary>
        ///状态是否有效  1、有效   0、无效
        ///</summary>
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
        ///<summary>
        ///否决原因
        ///</summary>
        public string Cancelreason
        {
            get
            {
                return m_Cancelreason;
            }
            set
            {
                m_Cancelreason = value;
            }
        }
        ///<summary>
        ///产前
        ///</summary>
        public string Prenatal
        {
            get
            {
                return m_Prenatal;
            }
            set
            {
                m_Prenatal = value;
            }
        }
        ///<summary>
        ///产前周数
        ///</summary>
        public string Prenatalno
        {
            get
            {
                return m_Prenatalno;
            }
            set
            {
                m_Prenatalno = value;
            }
        }
        ///<summary>
        ///产后
        ///</summary>
        public string Postpartum
        {
            get
            {
                return m_Postpartum;
            }
            set
            {
                m_Postpartum = value;
            }
        }
        ///<summary>
        ///并指左
        ///</summary>
        public string Andtoshowleft
        {
            get
            {
                return m_Andtoshowleft;
            }
            set
            {
                m_Andtoshowleft = value;
            }
        }
        ///<summary>
        ///并指右
        ///</summary>
        public string Andtoshowright
        {
            get
            {
                return m_Andtoshowright;
            }
            set
            {
                m_Andtoshowright = value;
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
                c1 = string.Empty;
                if(Diagnosticbasis!="")              
                {                    
                     c1 = "√";                  
                }
                else
                {
                    c1 = " ";
                }
                return c1;
            }
        }
        private string c2;

        /// <summary>
        /// 超声波
        /// </summary>
        public string C2
        {
            get
            {
                c2 = string.Empty;
                if (Diagnosticbasis1 != "")
                {
                    c2 = "√";
                }
                else
                {
                    c2 = " ";
                }
                return c2;
            }
        }
        private string c3;

        /// <summary>
        /// 尸检
        /// </summary>
        public string C3
        {
            get
            {
                c3 = string.Empty;
                if (Diagnosticbasis2 != "")
                {
                    c3 = "√";
                }
                else
                {
                    c3 = " ";
                }
                return c3;
            }
        }
        private string c4;

        /// <summary>
        /// 生化检验
        /// </summary>
        public string C4
        {
            get
            {
                c4 = string.Empty;
                if (Diagnosticbasis3 != "")
                {
                    c4 = "√";
                }
                else
                {
                    c4 = " ";
                }
                return c4;
            }
        }
        private string c5;

        /// <summary>
        /// 染色体
        /// </summary>
        public string C5
        {
            get
            {
                c5 = string.Empty;
                if (Diagnosticbasis5 != "")
                {
                    c5 = "√";
                }
                else
                {
                    c5 = "  ";
                }
                return c5;
            }
        }
        private string c6;

        /// <summary>
        /// 其他
        /// </summary>
        public string C6
        {
            get
            {
                c6 = string.Empty;
                if (Diagnosticbasis6 != "")
                {
                    c6 = "√";
                }
                else
                {
                    c6 = " ";
                }
                return c6;
            }
        }
        private string a1;

        /// <summary>
        /// 产前
        /// </summary>
        public string A1
        {
            get
            {
                a1 = string.Empty;
                if (Prenatal != "")
                {
                    a1 = "√";
                }
                else
                {
                    a1 = " ";
                }
                return a1;
            }
        }
        private string a2;

        /// <summary>
        /// 产后
        /// </summary>
        public string A2
        {
            get
            {
                a2 = string.Empty;
                if (Postpartum != "")
                {
                    a2 = "√";
                }
                else
                {
                    a2 = " ";
                }
                return a2;
            }
        }

        private string d1;

        /// <summary>
        /// 无脑畸形
        /// </summary>
        public string D1
        {
            get
            {
                d1 = string.Empty;
                if (DiagAnencephaly != "")
                {
                    d1 = "√";
                }
                else
                {
                    d1 = " ";
                }
                return d1;
            }
        }
        private string d2;

        /// <summary>
        /// 脊柱裂
        /// </summary>
        public string D2
        {
            get
            {
                d2 = string.Empty;
                if (DiagSpina != "")
                {
                    d2 = "√";
                }
                else
                {
                    d2 = " ";
                }
                return d2;
            }
        }
        private string d3;

        /// <summary>
        /// 脑彭出
        /// </summary>
        public string D3
        {
            get
            {
                d3 = string.Empty;
                if (DiagPengout != "")
                {
                    d3 = "√";
                }
                else
                {
                    d3 = " ";
                }
                return d3;
            }
        }
        private string d4;

        /// <summary>
        /// 先天性脑积水
        /// </summary>
        public string D4
        {
            get
            {
                d4 = string.Empty;
                if (DiagHydrocephalus != "")
                {
                    d4 = "√";
                }
                else
                {
                    d4 = " ";
                }
                return d4;
            }
        }
        private string d5;

        /// <summary>
        /// 腭裂
        /// </summary>
        public string D5
        {
            get
            {
                d5 = string.Empty;
                if (DiagCleftpalate != "")
                {
                    d5 = "√";
                }
                else
                {
                    d5 = " ";
                }
                return d5;
            }
        }
        private string d6;

        /// <summary>
        /// 唇裂
        /// </summary>
        public string D6
        {
            get
            {
                d6 = string.Empty;
                if (DiagCleftlip != "")
                {
                    d6 = "√";
                }
                else
                {
                    d6 = " ";
                }
                return d6;
            }
        }
        private string d7;

        /// <summary>
        /// 唇裂合并腭裂
        /// </summary>
        public string D7
        {
            get
            {
                d7 = string.Empty;
                if (DiagCleftmerger != "")
                {
                    d7 = "√";
                }
                else
                {
                    d7 = " ";
                }
                return d7;
            }
        }
        private string d8;

        /// <summary>
        /// 小耳（包括无耳）
        /// </summary>
        public string D8
        {
            get
            {
                d8 = string.Empty;
                if (DiagSmallears != "")
                {
                    d8 = "√";
                }
                else
                {
                    d8 = " ";
                }
                return d8;
            }
        }
        private string d9;

        /// <summary>
        /// 外耳其它畸形（小耳、无耳除外）
        /// </summary>
        public string D9
        {
            get
            {
                d9 = string.Empty;
                if (DiagOuterear != "")
                {
                    d9 = "√";
                }
                else
                {
                    d9 = " ";
                }
                return d9;
            }
        }
        private string d10;

        /// <summary>
        /// 食道闭锁或狭窄
        /// </summary>
        public string D10
        {
            get
            {
                d10 = string.Empty;
                if (DiagEsophageal != "")
                {
                    d10 = "√";
                }
                else
                {
                    d10 = " ";
                }
                return d10;
            }
        }
        private string d11;

        /// <summary>
        /// 直肠肛门闭锁或狭窄（包括无肛）
        /// </summary>
        public string D11
        {
            get
            {
                d11 = string.Empty;
                if (DiagRectum != "")
                {
                    d11 = "√";
                }
                else
                {
                    d11 = " ";
                }
                return d11;
            }
        }
        private string d12;

        /// <summary>
        /// 尿道下裂
        /// </summary>
        public string D12
        {
            get
            {
                d12 = string.Empty;
                if (DiagHypospadias != "")
                {
                    d12 = "√";
                }
                else
                {
                    d12 = " ";
                }
                return d12;
            }
        }
        private string d13;

        /// <summary>
        /// 膀胱外翻
        /// </summary>
        public string D13
        {
            get
            {
                d13 = string.Empty;
                if (DiagBladder != "")
                {
                    d13 = "√";
                }
                else
                {
                    d13 = " ";
                }
                return d13;
            }
        }
        private string d14;

        /// <summary>
        /// 马蹄内翻足_左 
        /// </summary>
        public string D14
        {
            get
            {
                d14 = string.Empty;
                if (DiagHorseshoefootleft != "")
                {
                    d14 = "√";
                }
                else
                {
                    d14 = " ";
                }
                return d14;
            }
        }
        private string d15;

        /// <summary>
        /// 马蹄内翻足_右
        /// </summary>
        public string D15
        {
            get
            {
                d15 = string.Empty;
                if (DiagHorseshoefootright != "")
                {
                    d15 = "√";
                }
                else
                {
                    d15 = " ";
                }
                return d15;
            }
        }
        private string d16;

        /// <summary>
        /// 多指（趾）_左
        /// </summary>
        public string D16
        {
            get
            {
                d16 = string.Empty;
                if (DiagManypointleft != "")
                {
                    d16 = "√";
                }
                else
                {
                    d16 = " ";
                }
                return d16;
            }
        }
        private string d17;

        /// <summary>
        /// 多指（趾）_右
        /// </summary>
        public string D17
        {
            get
            {
                d17 = string.Empty;
                if (DiagManypointright != "")
                {
                    d17 = "√";
                }
                else
                {
                    d17 = " ";
                }
                return d17;
            }
        }
        private string d18;

        /// <summary>
        /// 肢体短缩_上肢 _左
        /// </summary>
        public string D18
        {
            get
            {
                d18 = string.Empty;
                if (DiagLimbsupperleft != "")
                {
                    d18 = "√";
                }
                else
                {
                    d18 = " ";
                }
                return d18;
            }
        }
        private string d19;

        /// <summary>
        /// 肢体短缩_上肢 _右
        /// </summary>
        public string D19
        {
            get
            {
                d19 = string.Empty;
                if (DiagLimbsupperright != "")
                {
                    d19 = "√";
                }
                else
                {
                    d19 = " ";
                }
                return d19;
            }
        }
        private string d20;

        /// <summary>
        /// 肢体短缩_下肢 _左
        /// </summary>
        public string D20
        {
            get
            {
                d20 = string.Empty;
                if (DiagLimbslowerleft != "")
                {
                    d20 = "√";
                }
                else
                {
                    d20 = " ";
                }
                return d20;
            }
        }
        private string d21;

        /// <summary>
        /// 肢体短缩_下肢 _右
        /// </summary>
        public string D21
        {
            get
            {
                d21 = string.Empty;
                if (DiagLimbslowerright != "")
                {
                    d21 = "√";
                }
                else
                {
                    d21 = " ";
                }
                return d21;
            }
        }
        private string d22;

        /// <summary>
        /// 先天性膈疝
        /// </summary>
        public string D22
        {
            get
            {
                d22 = string.Empty;
                if (DiagHernia != "")
                {
                    d22 = "√";
                }
                else
                {
                    d22 = " ";
                }
                return d22;
            }
        }
        private string d23;

        /// <summary>
        /// 脐膨出
        /// </summary>
        public string D23
        {
            get
            {
                d23 = string.Empty;
                if (DiagBulgingbelly != "")
                {
                    d23 = "√";
                }
                else
                {
                    d23 = " ";
                }
                return d23;
            }
        }
        private string d24;

        /// <summary>
        /// 腹裂
        /// </summary>
        public string D24
        {
            get
            {
                d24 = string.Empty;
                if (DiagGastroschisis != "")
                {
                    d24 = "√";
                }
                else
                {
                    d24 = " ";
                }
                return d24;
            }
        }
        private string d25;

        /// <summary>
        /// 联体双胎
        /// </summary>
        public string D25
        {
            get
            {
                d25 = string.Empty;
                if (DiagTwins != "")
                {
                    d25 = "√";
                }
                else
                {
                    d25 = " ";
                }
                return d25;
            }
        }
        private string d26;

        /// <summary>
        /// 唐氏综合征
        /// </summary>
        public string D26
        {
            get
            {
                d26 = string.Empty;
                if (DiagTssyndrome != "")
                {
                    d26 = "√";
                }
                else
                {
                    d26 = " ";
                }
                return d26;
            }
        }
        private string d27;

        /// <summary>
        /// 先天性心脏病
        /// </summary>
        public string D27
        {
            get
            {
                d27 = string.Empty;
                if (DiagHeartdisease != "")
                {
                    d27 = "√";
                }
                else
                {
                    d27 = " ";
                }
                return d27;
            }
        }
        private string d28;

        /// <summary>
        /// 其他（写明病名或详细描述）
        /// </summary>
        public string D28
        {
            get
            {
                d28 = string.Empty;
                if (DiagOther != "")
                {
                    d28 = "√";
                }
                else
                {
                    d28 = " ";
                }
                return d28;
            }
        }
        private string d29;

        /// <summary>
        /// 并指左
        /// </summary>
        public string D29
        {
            get
            {
                d29 = string.Empty;
                if (Andtoshowleft != "")
                {
                    d29 = "√";
                }
                else
                {
                    d29 = " ";
                }
                return d29;
            }
        }
        private string d30;

        /// <summary>
        /// 并指左
        /// </summary>
        public string D30
        {
            get
            {
                d30 = string.Empty;
                if (Andtoshowright != "")
                {
                    d30 = "√";
                }
                else
                {
                    d30 = " ";
                }
                return d30;
            }
        }

        private string b1;

        /// <summary>
        /// 发烧
        /// </summary>
        public string B1
        {
            get
            {
                b1 = string.Empty;
                if (Fever != "")
                {
                    b1 = "√";
                }
                else
                {
                    d30 = " ";
                }
                return b1;
            }
        }
        private string b2;

        /// <summary>
        /// 糖尿病
        /// </summary>
        public string B2
        {
            get
            {
                b2 = string.Empty;
                if (Isdiabetes != "")
                {
                    b2 = "√";
                }
                else
                {
                    d30 = " ";
                }
                return b2;
            }
        }
        private string b3;

        /// <summary>
        /// 是否本院出生
        /// </summary>
        public string B3
        {
            get
            {
                b3 = string.Empty;
                if (Isbornhere != "")
                {
                    b3 =Isbornhere;
                }
                else
                {
                    b3 = " ";
                }
                return b3;
            }
        }
        private string b4;

        /// <summary>
        /// 胎数
        /// </summary>
        public string B4
        {
            get
            {
                b4 = string.Empty;
                if (Births != "")
                {
                    b4 = Births;
                }
                else
                {
                    b4 = " ";
                }
                return b4;
            }
        }
        private string b5;

        /// <summary>
        /// 是否同卵
        /// </summary>
        public string B5
        {
            get
            {
                b5 = string.Empty;
                if (Isidentical != "")
                {
                    b5 = Isidentical;
                }
                else
                {
                    b5 = " ";
                }
                return b5;
            }
        }
        private string b6;

        /// <summary>
        /// 糖尿病
        /// </summary>
        public string B6
        {
            get
            {
                b6 = string.Empty;
                if (Isdiabetes != "")
                {
                    b6 = "√";
                }
                else
                {
                    b6 = " ";
                }
                return b6;
            }
        }
        private string b7;

        /// <summary>
        /// 糖尿病
        /// </summary>
        public string B7
        {
            get
            {
                b7 = string.Empty;
                if (Isdiabetes != "")
                {
                    b7 = "√";
                }
                else
                {
                    b7 = " ";
                }
                return b7;
            }
        }
        private string b8;

        /// <summary>
        /// 糖尿病
        /// </summary>
        public string B8
        {
            get
            {
                b8 = string.Empty;
                if (Isdiabetes != "")
                {
                    b8 = "√";
                }
                else
                {
                    b8 = " ";
                }
                return b8;
            }
        }
        private string b9;

        /// <summary>
        /// 糖尿病
        /// </summary>
        public string B9
        {
            get
            {
                b9 = string.Empty;
                if (Isdiabetes != "")
                {
                    b9 = "√";
                }
                else
                {
                    b9 = " ";
                }
                return b9;
            }
        }
        private string b10;

        /// <summary>
        /// 糖尿病
        /// </summary>
        public string B10
        {
            get
            {
                b10 = string.Empty;
                if (Isdiabetes != "")
                {
                    b10 = "√";
                }
                else
                {
                    b10 = " ";
                }
                return b10;
            }
        }

        #endregion Properties
    }
}
