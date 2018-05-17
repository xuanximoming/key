using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrectSoft.Core
{
    public class Iem_Mainpage_Basicinfo
    {
        #region declaration
        private string m_PathologyDiagnosisName = String.Empty;
        private string m_PathologyObservationSn = String.Empty;
        private string m_AshesDiagnosisName = String.Empty;
        private string m_AshesAnatomiseSn = String.Empty;
        private string m_AllergicDrug = String.Empty;
        private int m_Hbsag = 0;
        private int m_HcvAb = 0;
        private int m_HivAb = 0;
        private int m_OpdIpdId = 0;
        private int m_InOutInpatinetId = 0;
        private int m_BeforeAfterOrId = 0;
        private int m_ClinicalPathologyId = 0;
        private int m_PacsPathologyId = 0;
        private int m_SaveTimes = 0;
        private int m_SuccessTimes = 0;
        private string m_SectionDirector = String.Empty;
        private string m_Director = String.Empty;
        private string m_VsEmployeeCode = String.Empty;
        private string m_ResidentEmployeeCode = String.Empty;
        private string m_RefreshEmployeeCode = String.Empty;
        private string m_MasterInterne = String.Empty;
        private string m_Interne = String.Empty;
        private string m_CodingUser = String.Empty;
        private int m_MedicalQualityId = 0;
        private string m_QualityControlDoctor = String.Empty;
        private string m_QualityControlNurse = String.Empty;
        private string m_QualityControlDate = String.Empty;
        private string m_XaySn = String.Empty;
        private string m_CtSn = String.Empty;
        private string m_MriSn = String.Empty;
        private string m_DsaSn = String.Empty;
        private int m_IsFirstCase = 0;
        private int m_IsFollowing = 0;
        private string m_FollowingEndingDate = String.Empty;
        private int m_IsTeachingCase = 0;
        private int m_BloodTypeId = 0;
        private int m_Rh = 0;
        private int m_BloodReactionId = 0;
        private int m_BloodRbc = 0;
        private int m_BloodPlt = 0;
        private int m_BloodPlasma = 0;
        private int m_BloodWb = 0;
        private string m_BloodOthers = String.Empty;
        private string m_IsCompleted = String.Empty;
        private string m_CompletedTime = String.Empty;
        private int m_Valide = 0;
        private string m_CreateUser = String.Empty;
        private string m_CreateTime = String.Empty;
        private string m_ModifiedUser = String.Empty;
        private string m_ModifiedTime = String.Empty;
        private int m_IemMainpageNo = 0;
        private int m_Patnoofhis = 0;
        private int m_Noofinpat = 0;
        private string m_Payid = String.Empty;
        private string m_Socialcare = String.Empty;
        private int m_Incount = 0;
        private string m_Name = String.Empty;
        private string m_Sexid = String.Empty;
        private string m_Marital = String.Empty;
        private string m_Jobid = String.Empty;
        private string m_Provinceid = String.Empty;
        private string m_Countyid = String.Empty;
        private string m_Nationid = String.Empty;
        private string m_Nationalityid = String.Empty;
        private string m_Idno = String.Empty;
        private string m_Organization = String.Empty;
        private string m_Officeplace = String.Empty;
        private string m_Officetel = String.Empty;
        private string m_Officepost = String.Empty;
        private string m_Nativeaddress = String.Empty;
        private string m_Nativetel = String.Empty;
        private string m_Nativepost = String.Empty;
        private string m_Contactperson = String.Empty;
        private string m_Relationship = String.Empty;
        private string m_Contactaddress = String.Empty;
        private string m_Contacttel = String.Empty;
        private string m_Admitdate = String.Empty;
        private string m_Admitdept = String.Empty;
        private string m_Admitward = String.Empty;
        private int m_DaysBefore = 0;
        private string m_TransDate = String.Empty;
        private string m_TransAdmitdept = String.Empty;
        private string m_TransAdmitward = String.Empty;
        private string m_TransAdmitdeptAgain = String.Empty;
        private string m_Outwarddate = String.Empty;
        private string m_Outhosdept = String.Empty;
        private string m_Outhosward = String.Empty;
        private int m_ActualDays = 0;
        private string m_DeathTime = String.Empty;
        private string m_DeathReason = String.Empty;
        private string m_Admitinfo = String.Empty;
        private string m_InCheckDate = String.Empty;
        #endregion declaration

        public Iem_Mainpage_Basicinfo()
        {
        }

        #region Properties
        public string PathologyDiagnosisName
        {
            get
            {
                return m_PathologyDiagnosisName;
            }
            set
            {
                m_PathologyDiagnosisName = value;
            }
        }
        public string PathologyObservationSn
        {
            get
            {
                return m_PathologyObservationSn;
            }
            set
            {
                m_PathologyObservationSn = value;
            }
        }
        public string AshesDiagnosisName
        {
            get
            {
                return m_AshesDiagnosisName;
            }
            set
            {
                m_AshesDiagnosisName = value;
            }
        }
        public string AshesAnatomiseSn
        {
            get
            {
                return m_AshesAnatomiseSn;
            }
            set
            {
                m_AshesAnatomiseSn = value;
            }
        }
        public string AllergicDrug
        {
            get
            {
                return m_AllergicDrug;
            }
            set
            {
                m_AllergicDrug = value;
            }
        }
        public int Hbsag
        {
            get
            {
                return m_Hbsag;
            }
            set
            {
                m_Hbsag = value;
            }
        }
        public int HcvAb
        {
            get
            {
                return m_HcvAb;
            }
            set
            {
                m_HcvAb = value;
            }
        }
        public int HivAb
        {
            get
            {
                return m_HivAb;
            }
            set
            {
                m_HivAb = value;
            }
        }
        public int OpdIpdId
        {
            get
            {
                return m_OpdIpdId;
            }
            set
            {
                m_OpdIpdId = value;
            }
        }
        public int InOutInpatinetId
        {
            get
            {
                return m_InOutInpatinetId;
            }
            set
            {
                m_InOutInpatinetId = value;
            }
        }
        public int BeforeAfterOrId
        {
            get
            {
                return m_BeforeAfterOrId;
            }
            set
            {
                m_BeforeAfterOrId = value;
            }
        }
        public int ClinicalPathologyId
        {
            get
            {
                return m_ClinicalPathologyId;
            }
            set
            {
                m_ClinicalPathologyId = value;
            }
        }
        public int PacsPathologyId
        {
            get
            {
                return m_PacsPathologyId;
            }
            set
            {
                m_PacsPathologyId = value;
            }
        }
        public int SaveTimes
        {
            get
            {
                return m_SaveTimes;
            }
            set
            {
                m_SaveTimes = value;
            }
        }
        public int SuccessTimes
        {
            get
            {
                return m_SuccessTimes;
            }
            set
            {
                m_SuccessTimes = value;
            }
        }
        public string SectionDirector
        {
            get
            {
                return m_SectionDirector;
            }
            set
            {
                m_SectionDirector = value;
            }
        }
        public string Director
        {
            get
            {
                return m_Director;
            }
            set
            {
                m_Director = value;
            }
        }
        public string VsEmployeeCode
        {
            get
            {
                return m_VsEmployeeCode;
            }
            set
            {
                m_VsEmployeeCode = value;
            }
        }
        public string ResidentEmployeeCode
        {
            get
            {
                return m_ResidentEmployeeCode;
            }
            set
            {
                m_ResidentEmployeeCode = value;
            }
        }
        public string RefreshEmployeeCode
        {
            get
            {
                return m_RefreshEmployeeCode;
            }
            set
            {
                m_RefreshEmployeeCode = value;
            }
        }
        public string MasterInterne
        {
            get
            {
                return m_MasterInterne;
            }
            set
            {
                m_MasterInterne = value;
            }
        }
        public string Interne
        {
            get
            {
                return m_Interne;
            }
            set
            {
                m_Interne = value;
            }
        }
        public string CodingUser
        {
            get
            {
                return m_CodingUser;
            }
            set
            {
                m_CodingUser = value;
            }
        }
        public int MedicalQualityId
        {
            get
            {
                return m_MedicalQualityId;
            }
            set
            {
                m_MedicalQualityId = value;
            }
        }
        public string QualityControlDoctor
        {
            get
            {
                return m_QualityControlDoctor;
            }
            set
            {
                m_QualityControlDoctor = value;
            }
        }
        public string QualityControlNurse
        {
            get
            {
                return m_QualityControlNurse;
            }
            set
            {
                m_QualityControlNurse = value;
            }
        }
        public string QualityControlDate
        {
            get
            {
                return m_QualityControlDate;
            }
            set
            {
                m_QualityControlDate = value;
            }
        }
        public string XaySn
        {
            get
            {
                return m_XaySn;
            }
            set
            {
                m_XaySn = value;
            }
        }
        public string CtSn
        {
            get
            {
                return m_CtSn;
            }
            set
            {
                m_CtSn = value;
            }
        }
        public string MriSn
        {
            get
            {
                return m_MriSn;
            }
            set
            {
                m_MriSn = value;
            }
        }
        public string DsaSn
        {
            get
            {
                return m_DsaSn;
            }
            set
            {
                m_DsaSn = value;
            }
        }
        public int IsFirstCase
        {
            get
            {
                return m_IsFirstCase;
            }
            set
            {
                m_IsFirstCase = value;
            }
        }
        public int IsFollowing
        {
            get
            {
                return m_IsFollowing;
            }
            set
            {
                m_IsFollowing = value;
            }
        }
        public string FollowingEndingDate
        {
            get
            {
                return m_FollowingEndingDate;
            }
            set
            {
                m_FollowingEndingDate = value;
            }
        }
        public int IsTeachingCase
        {
            get
            {
                return m_IsTeachingCase;
            }
            set
            {
                m_IsTeachingCase = value;
            }
        }
        public int BloodTypeId
        {
            get
            {
                return m_BloodTypeId;
            }
            set
            {
                m_BloodTypeId = value;
            }
        }
        public int Rh
        {
            get
            {
                return m_Rh;
            }
            set
            {
                m_Rh = value;
            }
        }
        public int BloodReactionId
        {
            get
            {
                return m_BloodReactionId;
            }
            set
            {
                m_BloodReactionId = value;
            }
        }
        public int BloodRbc
        {
            get
            {
                return m_BloodRbc;
            }
            set
            {
                m_BloodRbc = value;
            }
        }
        public int BloodPlt
        {
            get
            {
                return m_BloodPlt;
            }
            set
            {
                m_BloodPlt = value;
            }
        }
        public int BloodPlasma
        {
            get
            {
                return m_BloodPlasma;
            }
            set
            {
                m_BloodPlasma = value;
            }
        }
        public int BloodWb
        {
            get
            {
                return m_BloodWb;
            }
            set
            {
                m_BloodWb = value;
            }
        }
        public string BloodOthers
        {
            get
            {
                return m_BloodOthers;
            }
            set
            {
                m_BloodOthers = value;
            }
        }
        public string IsCompleted
        {
            get
            {
                return m_IsCompleted;
            }
            set
            {
                m_IsCompleted = value;
            }
        }
        public string CompletedTime
        {
            get
            {
                return m_CompletedTime;
            }
            set
            {
                m_CompletedTime = value;
            }
        }
        public int Valide
        {
            get
            {
                return m_Valide;
            }
            set
            {
                m_Valide = value;
            }
        }
        public string CreateUser
        {
            get
            {
                return m_CreateUser;
            }
            set
            {
                m_CreateUser = value;
            }
        }
        public string CreateTime
        {
            get
            {
                return m_CreateTime;
            }
            set
            {
                m_CreateTime = value;
            }
        }
        public string ModifiedUser
        {
            get
            {
                return m_ModifiedUser;
            }
            set
            {
                m_ModifiedUser = value;
            }
        }
        public string ModifiedTime
        {
            get
            {
                return m_ModifiedTime;
            }
            set
            {
                m_ModifiedTime = value;
            }
        }
        public int IemMainpageNo
        {
            get
            {
                return m_IemMainpageNo;
            }
            set
            {
                m_IemMainpageNo = value;
            }
        }
        public int Patnoofhis
        {
            get
            {
                return m_Patnoofhis;
            }
            set
            {
                m_Patnoofhis = value;
            }
        }
        public int Noofinpat
        {
            get
            {
                return m_Noofinpat;
            }
            set
            {
                m_Noofinpat = value;
            }
        }
        public string Payid
        {
            get
            {
                return m_Payid;
            }
            set
            {
                m_Payid = value;
            }
        }
        public string Socialcare
        {
            get
            {
                return m_Socialcare;
            }
            set
            {
                m_Socialcare = value;
            }
        }
        public int Incount
        {
            get
            {
                return m_Incount;
            }
            set
            {
                m_Incount = value;
            }
        }
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
        public string Sexid
        {
            get
            {
                return m_Sexid;
            }
            set
            {
                m_Sexid = value;
            }
        }
        public string Marital
        {
            get
            {
                return m_Marital;
            }
            set
            {
                m_Marital = value;
            }
        }
        public string Jobid
        {
            get
            {
                return m_Jobid;
            }
            set
            {
                m_Jobid = value;
            }
        }
        public string Provinceid
        {
            get
            {
                return m_Provinceid;
            }
            set
            {
                m_Provinceid = value;
            }
        }
        public string Countyid
        {
            get
            {
                return m_Countyid;
            }
            set
            {
                m_Countyid = value;
            }
        }
        public string Nationid
        {
            get
            {
                return m_Nationid;
            }
            set
            {
                m_Nationid = value;
            }
        }
        public string Nationalityid
        {
            get
            {
                return m_Nationalityid;
            }
            set
            {
                m_Nationalityid = value;
            }
        }
        public string Idno
        {
            get
            {
                return m_Idno;
            }
            set
            {
                m_Idno = value;
            }
        }
        public string Organization
        {
            get
            {
                return m_Organization;
            }
            set
            {
                m_Organization = value;
            }
        }
        public string Officeplace
        {
            get
            {
                return m_Officeplace;
            }
            set
            {
                m_Officeplace = value;
            }
        }
        public string Officetel
        {
            get
            {
                return m_Officetel;
            }
            set
            {
                m_Officetel = value;
            }
        }
        public string Officepost
        {
            get
            {
                return m_Officepost;
            }
            set
            {
                m_Officepost = value;
            }
        }
        public string Nativeaddress
        {
            get
            {
                return m_Nativeaddress;
            }
            set
            {
                m_Nativeaddress = value;
            }
        }
        public string Nativetel
        {
            get
            {
                return m_Nativetel;
            }
            set
            {
                m_Nativetel = value;
            }
        }
        public string Nativepost
        {
            get
            {
                return m_Nativepost;
            }
            set
            {
                m_Nativepost = value;
            }
        }
        public string Contactperson
        {
            get
            {
                return m_Contactperson;
            }
            set
            {
                m_Contactperson = value;
            }
        }
        public string Relationship
        {
            get
            {
                return m_Relationship;
            }
            set
            {
                m_Relationship = value;
            }
        }
        public string Contactaddress
        {
            get
            {
                return m_Contactaddress;
            }
            set
            {
                m_Contactaddress = value;
            }
        }
        public string Contacttel
        {
            get
            {
                return m_Contacttel;
            }
            set
            {
                m_Contacttel = value;
            }
        }
        public string Admitdate
        {
            get
            {
                return m_Admitdate;
            }
            set
            {
                m_Admitdate = value;
            }
        }
        public string Admitdept
        {
            get
            {
                return m_Admitdept;
            }
            set
            {
                m_Admitdept = value;
            }
        }
        public string Admitward
        {
            get
            {
                return m_Admitward;
            }
            set
            {
                m_Admitward = value;
            }
        }
        public int DaysBefore
        {
            get
            {
                return m_DaysBefore;
            }
            set
            {
                m_DaysBefore = value;
            }
        }
        public string TransDate
        {
            get
            {
                return m_TransDate;
            }
            set
            {
                m_TransDate = value;
            }
        }
        public string TransAdmitdept
        {
            get
            {
                return m_TransAdmitdept;
            }
            set
            {
                m_TransAdmitdept = value;
            }
        }
        public string TransAdmitward
        {
            get
            {
                return m_TransAdmitward;
            }
            set
            {
                m_TransAdmitward = value;
            }
        }
        public string TransAdmitdeptAgain
        {
            get
            {
                return m_TransAdmitdeptAgain;
            }
            set
            {
                m_TransAdmitdeptAgain = value;
            }
        }
        public string Outwarddate
        {
            get
            {
                return m_Outwarddate;
            }
            set
            {
                m_Outwarddate = value;
            }
        }
        public string Outhosdept
        {
            get
            {
                return m_Outhosdept;
            }
            set
            {
                m_Outhosdept = value;
            }
        }
        public string Outhosward
        {
            get
            {
                return m_Outhosward;
            }
            set
            {
                m_Outhosward = value;
            }
        }
        public int ActualDays
        {
            get
            {
                return m_ActualDays;
            }
            set
            {
                m_ActualDays = value;
            }
        }
        public string DeathTime
        {
            get
            {
                return m_DeathTime;
            }
            set
            {
                m_DeathTime = value;
            }
        }
        public string DeathReason
        {
            get
            {
                return m_DeathReason;
            }
            set
            {
                m_DeathReason = value;
            }
        }
        public string Admitinfo
        {
            get
            {
                return m_Admitinfo;
            }
            set
            {
                m_Admitinfo = value;
            }
        }
        public string InCheckDate
        {
            get
            {
                return m_InCheckDate;
            }
            set
            {
                m_InCheckDate = value;
            }
        }
        #endregion Properties
    }
}