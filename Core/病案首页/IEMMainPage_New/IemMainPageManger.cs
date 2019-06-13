using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Common.Eop;

namespace YidanSoft.Core.IEMMainPage
{
    class IemMainPageManger
    {
        private IYidanEmrHost m_app;
        private Inpatient CurrentInpatient;

        DataHelper m_DataHelper = new DataHelper();
        public IemMainPageManger(IYidanEmrHost app,Inpatient _CurrentInpatient)
        {

            m_app = app;
            CurrentInpatient = _CurrentInpatient;
        }
 

        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get;
            set;
        }

        #region 根据首页序号得到病案首页的信息，并给界面赋值
        /// <summary>
        /// LOAD时获取病案首页信息
        /// </summary>
        public IemMainPageInfo GetIemInfo()
        {
            //首先去Iem_Mainpage_Basicinfo根据首页序号捞取资料，如果没有，则LOAD基本用户信息
            SqlParameter[] paraCollection = new SqlParameter[] { new SqlParameter("@NoOfInpat", CurrentInpatient.NoOfFirstPage) };
            DataSet dataSet = m_app.SqlHelper.ExecuteDataSet("IEM_MAIN_PAGE.usp_getieminfo_new", paraCollection, CommandType.StoredProcedure);
            IemInfo = new IemMainPageInfo();
            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                if (i == 0)
                    GetIemBasInfo(dataSet.Tables[i]);
                else if (i == 1)
                    GetItemDiagInfo(dataSet.Tables[i]);
                else if (i == 2)
                    //GetItemOperInfo(dataSet.Tables[i]);
                    IemInfo.IemOperInfo.Operation_Table = dataSet.Tables[i];
                else if (i == 3)
                    GetItemObsBaby(dataSet.Tables[i]);
            }
            if (IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
            {
                IemInfo.IemBasicInfo.PatNoOfHis = CurrentInpatient.NoOfHisFirstPage;
                IemInfo.IemBasicInfo.NoOfInpat = CurrentInpatient.NoOfFirstPage.ToString();
            }

            return IemInfo;
        }

        #region 获取病案首页信息
        /// <summary>
        /// 基本信息
        /// </summary>
        /// <param name="dataTable"></param>
        private void GetIemBasInfo(DataTable dataTable)
        {

            foreach (DataRow row in dataTable.Rows)
            {
                #region 赋值
                IemInfo.IemBasicInfo.Iem_Mainpage_NO = row["Iem_Mainpage_NO"].ToString();
                IemInfo.IemBasicInfo.PatNoOfHis = row["PatNoOfHis"].ToString();
                IemInfo.IemBasicInfo.NoOfInpat = row["NoOfInpat"].ToString();
                IemInfo.IemBasicInfo.InCount = row["InCount"].ToString();
                IemInfo.IemBasicInfo.SocialCare = row["socialcare"].ToString();

                IemInfo.IemBasicInfo.PayID = row["PayID"].ToString();
                IemInfo.IemBasicInfo.PayName = row["PayName"].ToString();
                IemInfo.IemBasicInfo.Name = row["Name"].ToString();
                IemInfo.IemBasicInfo.SexID = row["SexID"].ToString();
                IemInfo.IemBasicInfo.Birth = row["Birth"].ToString();

                IemInfo.IemBasicInfo.Marital = row["Marital"].ToString();
                IemInfo.IemBasicInfo.JobID = row["JobID"].ToString();
                IemInfo.IemBasicInfo.JobName = row["JobName"].ToString();
                IemInfo.IemBasicInfo.ProvinceID = row["ProvinceID"].ToString();
                IemInfo.IemBasicInfo.ProvinceName = row["ProvinceName"].ToString();

                IemInfo.IemBasicInfo.CountyID = row["CountyID"].ToString();
                IemInfo.IemBasicInfo.CountyName = row["CountyName"].ToString();
                IemInfo.IemBasicInfo.NationID = row["NationID"].ToString();
                IemInfo.IemBasicInfo.NationName = row["NationName"].ToString();
                IemInfo.IemBasicInfo.NationalityID = row["NationalityID"].ToString();

                IemInfo.IemBasicInfo.NationalityName = row["NationalityName"].ToString();
                IemInfo.IemBasicInfo.IDNO = row["IDNO"].ToString();
                IemInfo.IemBasicInfo.Organization = row["Organization"].ToString();
                IemInfo.IemBasicInfo.OfficePlace = row["OfficePlace"].ToString();
                IemInfo.IemBasicInfo.OfficeTEL = row["OfficeTEL"].ToString();

                IemInfo.IemBasicInfo.OfficePost = row["OfficePost"].ToString();
                IemInfo.IemBasicInfo.NativeAddress = row["NativeAddress"].ToString();
                IemInfo.IemBasicInfo.NativeTEL = row["NativeTEL"].ToString();
                IemInfo.IemBasicInfo.NativePost = row["NativePost"].ToString();
                IemInfo.IemBasicInfo.ContactPerson = row["ContactPerson"].ToString();

                IemInfo.IemBasicInfo.RelationshipID = row["relationship"].ToString();
                IemInfo.IemBasicInfo.RelationshipName = row["RelationshipName"].ToString();
                IemInfo.IemBasicInfo.ContactAddress = row["ContactAddress"].ToString();
                IemInfo.IemBasicInfo.ContactTEL = row["ContactTEL"].ToString();
                IemInfo.IemBasicInfo.AdmitDate = row["AdmitDate"].ToString();

                IemInfo.IemBasicInfo.AdmitDeptID = row["AdmitDept"].ToString();
                IemInfo.IemBasicInfo.AdmitDeptName = row["AdmitDeptName"].ToString();
                IemInfo.IemBasicInfo.AdmitWardID = row["AdmitWard"].ToString();
                IemInfo.IemBasicInfo.AdmitWardName = row["AdmitWardName"].ToString();
                IemInfo.IemBasicInfo.Trans_AdmitDeptID = row["Trans_AdmitDept"].ToString();

                IemInfo.IemBasicInfo.Trans_AdmitDeptName = row["Trans_AdmitDeptName"].ToString();
                IemInfo.IemBasicInfo.OutWardDate = row["OutWardDate"].ToString();
                IemInfo.IemBasicInfo.OutHosDeptID = row["OutHosDept"].ToString();
                IemInfo.IemBasicInfo.OutHosDeptName = row["outhosdeptName"].ToString();
                IemInfo.IemBasicInfo.OutHosWardID = row["OutHosWard"].ToString();

                IemInfo.IemBasicInfo.OutHosWardName = row["OutHosWardName"].ToString();
                IemInfo.IemBasicInfo.ActualDays = row["Actual_Days"].ToString();
                IemInfo.IemBasicInfo.Days_Before = row["Days_Before"].ToString();
                IemInfo.IemBasicInfo.Trans_Date = row["Trans_Date"].ToString();
                IemInfo.IemBasicInfo.Trans_AdmitDept_Again = row["Trans_AdmitDept_Again"].ToString();

                IemInfo.IemBasicInfo.Trans_AdmitWard = row["Trans_AdmitWard"].ToString();
                IemInfo.IemBasicInfo.Death_Time = row["death_time"].ToString();
                IemInfo.IemBasicInfo.Death_Reason = row["Death_Reason"].ToString();

                IemInfo.IemBasicInfo.Age = CurrentInpatient.PersonalInformation.CurrentDisplayAge;
                IemInfo.IemBasicInfo.Ashes_Anatomise_Sn = row["ashes_anatomise_sn"].ToString();
                IemInfo.IemBasicInfo.Ashes_Diagnosis_Name = row["ashes_diagnosis_name"].ToString();
                IemInfo.IemBasicInfo.HospitalName = m_DataHelper.GetHospitalName();

                ///////诊断实体中
                IemInfo.IemDiagInfo.AdmitInfo = row["AdmitInfo"].ToString();
                IemInfo.IemDiagInfo.In_Check_Date = row["In_Check_Date"].ToString();
                IemInfo.IemDiagInfo.ZymosisID = row["zymosis"].ToString();
                IemInfo.IemDiagInfo.ZymosisName = row["zymosisName"].ToString();
                IemInfo.IemDiagInfo.ZymosisState = row["ZymosisState"].ToString();

                IemInfo.IemDiagInfo.Pathology_Diagnosis_Name = row["pathology_diagnosis_name"].ToString();
                IemInfo.IemDiagInfo.Pathology_Observation_Sn = row["pathology_observation_sn"].ToString();
                IemInfo.IemDiagInfo.Hurt_Toxicosis_Element = row["hurt_toxicosis_ele"].ToString();
                IemInfo.IemDiagInfo.Allergic_Drug = row["allergic_drug"].ToString();
                IemInfo.IemDiagInfo.Hbsag = row["hbsag"].ToString();

                IemInfo.IemDiagInfo.Hcv_Ab = row["hcv_ab"].ToString();
                IemInfo.IemDiagInfo.Hiv_Ab = row["hiv_ab"].ToString();
                IemInfo.IemDiagInfo.Opd_Ipd_Id = row["opd_ipd_id"].ToString();
                IemInfo.IemDiagInfo.In_Out_Inpatinet_Id = row["in_out_inpatinet_id"].ToString();
                IemInfo.IemDiagInfo.Before_After_Or_Id = row["before_after_or_id"].ToString();

                IemInfo.IemDiagInfo.Clinical_Pathology_Id = row["clinical_pathology_id"].ToString();
                IemInfo.IemDiagInfo.Pacs_Pathology_Id = row["pacs_pathology_id"].ToString();
                IemInfo.IemDiagInfo.Save_Times = row["save_times"].ToString();
                IemInfo.IemDiagInfo.Success_Times = row["success_times"].ToString();
                IemInfo.IemDiagInfo.Section_DirectorID = row["section_director"].ToString();

                IemInfo.IemDiagInfo.Section_DirectorName = row["section_directorName"].ToString();
                IemInfo.IemDiagInfo.DirectorID = row["director"].ToString();
                IemInfo.IemDiagInfo.DirectorName = row["directorName"].ToString();
                IemInfo.IemDiagInfo.Vs_EmployeeID = row["vs_employeeID"].ToString();
                IemInfo.IemDiagInfo.Vs_EmployeeName = row["vs_employeeName"].ToString();

                IemInfo.IemDiagInfo.Resident_EmployeeID = row["resident_employeeID"].ToString();
                IemInfo.IemDiagInfo.Resident_EmployeeName = row["resident_employeeName"].ToString();
                IemInfo.IemDiagInfo.Refresh_EmployeeID = row["refresh_employeeID"].ToString();
                IemInfo.IemDiagInfo.Refresh_EmployeeName = row["refresh_employeeName"].ToString();
                IemInfo.IemDiagInfo.Master_InterneID = row["master_interne"].ToString();

                IemInfo.IemDiagInfo.Master_InterneName = row["master_interneName"].ToString();
                IemInfo.IemDiagInfo.InterneID = row["interne"].ToString();
                IemInfo.IemDiagInfo.InterneName = row["interneName"].ToString();
                IemInfo.IemDiagInfo.Coding_UserID = row["coding_user"].ToString();
                IemInfo.IemDiagInfo.Coding_UserName = row["coding_userName"].ToString();

                IemInfo.IemDiagInfo.Medical_Quality_Id = row["medical_quality_id"].ToString();
                IemInfo.IemDiagInfo.Quality_Control_DoctorID = row["quality_control_doctor"].ToString();
                IemInfo.IemDiagInfo.Quality_Control_DoctorName = row["quality_control_doctorName"].ToString();
                IemInfo.IemDiagInfo.Quality_Control_NurseID = row["quality_control_nurse"].ToString();
                IemInfo.IemDiagInfo.Quality_Control_NurseName = row["quality_control_nurseName"].ToString();

                IemInfo.IemDiagInfo.Quality_Control_Date = row["quality_control_date"].ToString();

                ///////费用模块
                IemInfo.IemFeeInfo.Ashes_Check = row["Ashes_Check"].ToString();
                IemInfo.IemFeeInfo.IsFirstCase = row["is_first_case"].ToString();
                IemInfo.IemFeeInfo.IsFollowing = row["is_following"].ToString();
                IemInfo.IemFeeInfo.IsTeachingCase = row["is_teaching_case"].ToString();
                IemInfo.IemFeeInfo.Following_Ending_Date = row["following_ending_date"].ToString();

                IemInfo.IemFeeInfo.BloodType = row["blood_type_id"].ToString();
                IemInfo.IemFeeInfo.Rh = row["Rh"].ToString();
                IemInfo.IemFeeInfo.BloodReaction = row["blood_reaction_id"].ToString();
                IemInfo.IemFeeInfo.Rbc = row["blood_rbc"].ToString();
                IemInfo.IemFeeInfo.Plt = row["blood_plt"].ToString();

                IemInfo.IemFeeInfo.Plasma = row["Blood_Plasma"].ToString();
                IemInfo.IemFeeInfo.Wb = row["blood_wb"].ToString();
                IemInfo.IemFeeInfo.Others = row["blood_others"].ToString();

                ////费用信息暂时假数据
                IemInfo.IemFeeInfo.Total = "15000";
                IemInfo.IemFeeInfo.Bed = "100";
                IemInfo.IemFeeInfo.Care = "200";
                IemInfo.IemFeeInfo.WMedical = "300";
                IemInfo.IemFeeInfo.CPMedical = "400";
                IemInfo.IemFeeInfo.CMedical = "500";
                IemInfo.IemFeeInfo.Radiate = "600";
                IemInfo.IemFeeInfo.Assay = "700";
                IemInfo.IemFeeInfo.Ox = "800";
                IemInfo.IemFeeInfo.Blood = "900";
                IemInfo.IemFeeInfo.Mecical = "1000";
                IemInfo.IemFeeInfo.Operation = "1100";
                IemInfo.IemFeeInfo.Accouche = "1200";
                IemInfo.IemFeeInfo.Ris = "1300";
                IemInfo.IemFeeInfo.Anaesthesia = "1400";
                IemInfo.IemFeeInfo.Baby = "1500";
                IemInfo.IemFeeInfo.FollwBed = "1600";
                IemInfo.IemFeeInfo.Others1 = "1700";
                IemInfo.IemFeeInfo.Others2 = "1800";
                IemInfo.IemFeeInfo.Others3 = "1900";
                //IemInfo.IemFeeInfo = GetIemFeeInfo(IemInfo);



                ////基础信息模块
                IemInfo.IemBasicInfo.Is_Completed = row["Is_Completed"].ToString();
                IemInfo.IemBasicInfo.completed_time = row["completed_time"].ToString();
                IemInfo.IemBasicInfo.Valide = row["Valide"].ToString();
                IemInfo.IemBasicInfo.Create_User = row["Create_User"].ToString();
                IemInfo.IemBasicInfo.Create_Time = row["Create_Time"].ToString();
                IemInfo.IemBasicInfo.Modified_User = row["Modified_User"].ToString();
                IemInfo.IemBasicInfo.Modified_Time = row["Modified_Time"].ToString();

                IemInfo.IemBasicInfo.Xay_Sn = row["Xay_Sn"].ToString();
                IemInfo.IemBasicInfo.Ct_Sn = row["Ct_Sn"].ToString();
                IemInfo.IemBasicInfo.Mri_Sn = row["Mri_Sn"].ToString();
                IemInfo.IemBasicInfo.Dsa_Sn = row["Dsa_Sn"].ToString();

                #endregion
                break;
            }

            //给实体赋空值
            if (dataTable.Rows.Count == 0)
            {
                #region 赋空值
                IemInfo.IemBasicInfo.Iem_Mainpage_NO = "";
                IemInfo.IemBasicInfo.PatNoOfHis = "";
                IemInfo.IemBasicInfo.NoOfInpat = "";
                IemInfo.IemBasicInfo.InCount = "";
                IemInfo.IemBasicInfo.SocialCare = "";

                IemInfo.IemBasicInfo.PayID = "";
                IemInfo.IemBasicInfo.PayName = "";
                IemInfo.IemBasicInfo.Name = "";
                IemInfo.IemBasicInfo.SexID = "";
                IemInfo.IemBasicInfo.Birth = "";

                IemInfo.IemBasicInfo.Marital = "";
                IemInfo.IemBasicInfo.JobID = "";
                IemInfo.IemBasicInfo.JobName = "";
                IemInfo.IemBasicInfo.ProvinceID = "";
                IemInfo.IemBasicInfo.ProvinceName = "";

                IemInfo.IemBasicInfo.CountyID = "";
                IemInfo.IemBasicInfo.CountyName = "";
                IemInfo.IemBasicInfo.NationID = "";
                IemInfo.IemBasicInfo.NationName = "";
                IemInfo.IemBasicInfo.NationalityID = "";

                IemInfo.IemBasicInfo.NationalityName = "";
                IemInfo.IemBasicInfo.IDNO = "";
                IemInfo.IemBasicInfo.Organization = "";
                IemInfo.IemBasicInfo.OfficePlace = "";
                IemInfo.IemBasicInfo.OfficeTEL = "";

                IemInfo.IemBasicInfo.OfficePost = "";
                IemInfo.IemBasicInfo.NativeAddress = "";
                IemInfo.IemBasicInfo.NativeTEL = "";
                IemInfo.IemBasicInfo.NativePost = "";
                IemInfo.IemBasicInfo.ContactPerson = "";

                IemInfo.IemBasicInfo.RelationshipID = "";
                IemInfo.IemBasicInfo.RelationshipName = "";
                IemInfo.IemBasicInfo.ContactAddress = "";
                IemInfo.IemBasicInfo.ContactTEL = "";
                IemInfo.IemBasicInfo.AdmitDate = "";

                IemInfo.IemBasicInfo.AdmitDeptID = "";
                IemInfo.IemBasicInfo.AdmitDeptName = "";
                IemInfo.IemBasicInfo.AdmitWardID = "";
                IemInfo.IemBasicInfo.AdmitWardName = "";
                IemInfo.IemBasicInfo.Trans_AdmitDeptID = "";

                IemInfo.IemBasicInfo.Trans_AdmitDeptName = "";
                IemInfo.IemBasicInfo.OutWardDate = "";
                IemInfo.IemBasicInfo.OutHosDeptID = "";
                IemInfo.IemBasicInfo.OutHosDeptName = "";
                IemInfo.IemBasicInfo.OutHosWardID = "";

                IemInfo.IemBasicInfo.OutHosWardName = "";
                IemInfo.IemBasicInfo.ActualDays = "";
                IemInfo.IemBasicInfo.Days_Before = "";
                IemInfo.IemBasicInfo.Trans_Date = "";
                IemInfo.IemBasicInfo.Trans_AdmitDept_Again = "";

                IemInfo.IemBasicInfo.Trans_AdmitWard = "";
                IemInfo.IemBasicInfo.Death_Time = "";
                IemInfo.IemBasicInfo.Death_Reason  = "";

                IemInfo.IemBasicInfo.Age = "";
                IemInfo.IemBasicInfo.Ashes_Anatomise_Sn = "";
                IemInfo.IemBasicInfo.Ashes_Diagnosis_Name = "";
                IemInfo.IemBasicInfo.HospitalName = "";

                ///////诊断实体中
                IemInfo.IemDiagInfo.AdmitInfo = "";
                IemInfo.IemDiagInfo.In_Check_Date = "";
                IemInfo.IemDiagInfo.ZymosisID = "";
                IemInfo.IemDiagInfo.ZymosisName = "";
                IemInfo.IemDiagInfo.ZymosisState = "";

                IemInfo.IemDiagInfo.Pathology_Diagnosis_Name = "";
                IemInfo.IemDiagInfo.Pathology_Observation_Sn = "";
                IemInfo.IemDiagInfo.Hurt_Toxicosis_Element = "";
                IemInfo.IemDiagInfo.Allergic_Drug = "";
                IemInfo.IemDiagInfo.Hbsag = "";

                IemInfo.IemDiagInfo.Hcv_Ab = "";
                IemInfo.IemDiagInfo.Hiv_Ab = "";
                IemInfo.IemDiagInfo.Opd_Ipd_Id = "";
                IemInfo.IemDiagInfo.In_Out_Inpatinet_Id = "";
                IemInfo.IemDiagInfo.Before_After_Or_Id = "";

                IemInfo.IemDiagInfo.Clinical_Pathology_Id = "";
                IemInfo.IemDiagInfo.Pacs_Pathology_Id = "";
                IemInfo.IemDiagInfo.Save_Times = "";
                IemInfo.IemDiagInfo.Success_Times = "";
                IemInfo.IemDiagInfo.Section_DirectorID = "";

                IemInfo.IemDiagInfo.Section_DirectorName = "";
                IemInfo.IemDiagInfo.DirectorID = "";
                IemInfo.IemDiagInfo.DirectorName = "";
                IemInfo.IemDiagInfo.Vs_EmployeeID = "";
                IemInfo.IemDiagInfo.Vs_EmployeeName = "";

                IemInfo.IemDiagInfo.Resident_EmployeeID = "";
                IemInfo.IemDiagInfo.Resident_EmployeeName = "";
                IemInfo.IemDiagInfo.Refresh_EmployeeID = "";
                IemInfo.IemDiagInfo.Refresh_EmployeeName = "";
                IemInfo.IemDiagInfo.Master_InterneID = "";

                IemInfo.IemDiagInfo.Master_InterneName = "";
                IemInfo.IemDiagInfo.InterneID = "";
                IemInfo.IemDiagInfo.InterneName = "";
                IemInfo.IemDiagInfo.Coding_UserID = "";
                IemInfo.IemDiagInfo.Coding_UserName = "";

                IemInfo.IemDiagInfo.Medical_Quality_Id = "";
                IemInfo.IemDiagInfo.Quality_Control_DoctorID = "";
                IemInfo.IemDiagInfo.Quality_Control_DoctorName = "";
                IemInfo.IemDiagInfo.Quality_Control_NurseID = "";
                IemInfo.IemDiagInfo.Quality_Control_NurseName = "";

                IemInfo.IemDiagInfo.Quality_Control_Date = "";

                ///////费用模块
                IemInfo.IemFeeInfo.Ashes_Check = "";
                IemInfo.IemFeeInfo.IsFirstCase = "";
                IemInfo.IemFeeInfo.IsFollowing = "";
                IemInfo.IemFeeInfo.IsTeachingCase = "";
                IemInfo.IemFeeInfo.Following_Ending_Date = "";

                IemInfo.IemFeeInfo.BloodType = "";
                IemInfo.IemFeeInfo.Rh = "";
                IemInfo.IemFeeInfo.BloodReaction = "";
                IemInfo.IemFeeInfo.Rbc = "";
                IemInfo.IemFeeInfo.Plt = "";

                IemInfo.IemFeeInfo.Plasma = "";
                IemInfo.IemFeeInfo.Wb = "";
                IemInfo.IemFeeInfo.Others = "";

                ////费用信息暂时假数据
                IemInfo.IemFeeInfo.Total = "15000";
                IemInfo.IemFeeInfo.Bed = "100";
                IemInfo.IemFeeInfo.Care = "200";
                IemInfo.IemFeeInfo.WMedical = "300";
                IemInfo.IemFeeInfo.CPMedical = "400";
                IemInfo.IemFeeInfo.CMedical = "500";
                IemInfo.IemFeeInfo.Radiate = "600";
                IemInfo.IemFeeInfo.Assay = "700";
                IemInfo.IemFeeInfo.Ox = "800";
                IemInfo.IemFeeInfo.Blood = "900";
                IemInfo.IemFeeInfo.Mecical = "1000";
                IemInfo.IemFeeInfo.Operation = "1100";
                IemInfo.IemFeeInfo.Accouche = "1200";
                IemInfo.IemFeeInfo.Ris = "1300";
                IemInfo.IemFeeInfo.Anaesthesia = "1400";
                IemInfo.IemFeeInfo.Baby = "1500";
                IemInfo.IemFeeInfo.FollwBed = "1600";
                IemInfo.IemFeeInfo.Others1 = "1700";
                IemInfo.IemFeeInfo.Others2 = "1800";
                IemInfo.IemFeeInfo.Others3 = "1900";
                //IemInfo.IemFeeInfo = GetIemFeeInfo(IemInfo);



                ////基础信息模块
                IemInfo.IemBasicInfo.Is_Completed = "";
                IemInfo.IemBasicInfo.completed_time = "";
                IemInfo.IemBasicInfo.Valide = "";
                IemInfo.IemBasicInfo.Create_User = "";
                IemInfo.IemBasicInfo.Create_Time = "";
                IemInfo.IemBasicInfo.Modified_User = "";
                IemInfo.IemBasicInfo.Modified_Time = "";

                IemInfo.IemBasicInfo.Xay_Sn = "";
                IemInfo.IemBasicInfo.Ct_Sn = "";
                IemInfo.IemBasicInfo.Mri_Sn = "";
                IemInfo.IemBasicInfo.Dsa_Sn = "";


                #region 赋基础值

                IemInfo.IemBasicInfo.PatNoOfHis = CurrentInpatient.NoOfHisFirstPage.ToString();
                IemInfo.IemBasicInfo.InCount = CurrentInpatient.TimesOfAdmission.ToString();

                IemInfo.IemBasicInfo.PayID = CurrentInpatient.PaymentKind.Code.ToString();
                IemInfo.IemBasicInfo.PayName = CurrentInpatient.PaymentKind.Name.ToString();

                IemInfo.IemBasicInfo.Age = CurrentInpatient.PersonalInformation.CurrentDisplayAge;
                IemInfo.IemBasicInfo.HospitalName = m_DataHelper.GetHospitalName();
                if (CurrentInpatient.PersonalInformation != null)
                {
                    IemInfo.IemBasicInfo.SocialCare = CurrentInpatient.PersonalInformation.SocialInsuranceNo;
                    IemInfo.IemBasicInfo.Age = CurrentInpatient.PersonalInformation.DisplayAge;
                    IemInfo.IemBasicInfo.Name = CurrentInpatient.PersonalInformation.PatientName;

                    if (CurrentInpatient.PersonalInformation.Sex != null)
                        IemInfo.IemBasicInfo.SexID = CurrentInpatient.PersonalInformation.Sex.Code;
                    if (CurrentInpatient.PersonalInformation.Birthday.CompareTo(DateTime.MinValue) != 0)
                    {
                        IemInfo.IemBasicInfo.Birth = CurrentInpatient.PersonalInformation.Birthday.ToString("yyyy-MM-dd");
                    }
                    if (CurrentInpatient.PersonalInformation.MarriageCondition != null)
                        IemInfo.IemBasicInfo.Marital = CurrentInpatient.PersonalInformation.MarriageCondition.Code;


                    if (CurrentInpatient.PersonalInformation.DepartmentOfWork != null)
                    {
                        IemInfo.IemBasicInfo.JobID = CurrentInpatient.PersonalInformation.DepartmentOfWork.Occupation.Code;
                        IemInfo.IemBasicInfo.JobName = CurrentInpatient.PersonalInformation.DepartmentOfWork.Occupation.Name;
                        IemInfo.IemBasicInfo.OfficePlace = CurrentInpatient.PersonalInformation.DepartmentOfWork.CompanyName + CurrentInpatient.PersonalInformation.DepartmentOfWork.CompanyAddress;
                    }
                    if (IemInfo.IemBasicInfo.OfficePlace.Trim() == "[]")
                    {
                        IemInfo.IemBasicInfo.OfficePlace = "";
                    }

                    if (CurrentInpatient.PersonalInformation.DomiciliaryInfo != null)
                    {
                        IemInfo.IemBasicInfo.ProvinceID = CurrentInpatient.PersonalInformation.DomiciliaryInfo.Province.Code;
                        IemInfo.IemBasicInfo.ProvinceName = CurrentInpatient.PersonalInformation.DomiciliaryInfo.Province.Name;
                        IemInfo.IemBasicInfo.CountyID = CurrentInpatient.PersonalInformation.DomiciliaryInfo.City.Code;
                        IemInfo.IemBasicInfo.CountyID = CurrentInpatient.PersonalInformation.DomiciliaryInfo.City.Name;

                        IemInfo.IemBasicInfo.NationalityID = CurrentInpatient.PersonalInformation.DomiciliaryInfo.Country.Code;
                        IemInfo.IemBasicInfo.NationalityName = CurrentInpatient.PersonalInformation.DomiciliaryInfo.Country.Name;
                        IemInfo.IemBasicInfo.NativeAddress = CurrentInpatient.PersonalInformation.DomiciliaryInfo.FullAddress;
                        IemInfo.IemBasicInfo.NativeTEL = CurrentInpatient.PersonalInformation.DomiciliaryInfo.PhoneNo;
                        IemInfo.IemBasicInfo.NativePost = CurrentInpatient.PersonalInformation.DomiciliaryInfo.Postalcode;
 
                    }
                    if (CurrentInpatient.PersonalInformation.Nation != null)
                    {
                        IemInfo.IemBasicInfo.NationID = CurrentInpatient.PersonalInformation.Nation.Code;
                        IemInfo.IemBasicInfo.NationName = CurrentInpatient.PersonalInformation.Nation.Name;
                    }

                    if (CurrentInpatient.PersonalInformation.LinkManInfo != null)
                    {
                        IemInfo.IemBasicInfo.ContactPerson = CurrentInpatient.PersonalInformation.LinkManInfo.Name;
                        IemInfo.IemBasicInfo.RelationshipID = CurrentInpatient.PersonalInformation.LinkManInfo.Relation.Code;
                        IemInfo.IemBasicInfo.RelationshipName = CurrentInpatient.PersonalInformation.LinkManInfo.Relation.Name;
                        IemInfo.IemBasicInfo.ContactAddress = CurrentInpatient.PersonalInformation.LinkManInfo.ContactAddress.FullAddress;
                        IemInfo.IemBasicInfo.ContactTEL = CurrentInpatient.PersonalInformation.LinkManInfo.ContactAddress.PhoneNo;
                    }
                }
                if (CurrentInpatient.Recorder != null)
                    IemInfo.IemBasicInfo.IDNO = CurrentInpatient.Recorder.IdentityNo;

                if (CurrentInpatient.InfoOfAdmission != null)
                {
                    if (CurrentInpatient.InfoOfAdmission.AdmitInfo.StepOneDate.CompareTo(DateTime.MinValue) != 0)
                    {
                        //deTransDate.DateTime.ToString("yyyy-MM-dd") + " " + teTransDate.Time.ToString("HH:mm:ss");
                        IemInfo.IemBasicInfo.AdmitDate = CurrentInpatient.InfoOfAdmission.AdmitInfo.StepOneDate.ToString("yyyy-MM-dd") + " " + CurrentInpatient.InfoOfAdmission.AdmitInfo.StepOneDate.ToString("HH:mm:ss");
                         
                        //teAdmitDate.Time = CurrentInpatient.InfoOfAdmission.AdmitInfo.StepOneDate;
                    }

                    if (CurrentInpatient.InfoOfAdmission.DischargeInfo.StepOneDate.CompareTo(DateTime.MinValue) != 0)
                    {
                        //deTransDate.DateTime.ToString("yyyy-MM-dd") + " " + teTransDate.Time.ToString("HH:mm:ss");
                        IemInfo.IemBasicInfo.OutWardDate = CurrentInpatient.InfoOfAdmission.DischargeInfo.StepOneDate.ToString("yyyy-MM-dd") + " " + CurrentInpatient.InfoOfAdmission.DischargeInfo.StepOneDate.ToString("HH:mm:ss");
                         
                        //teAdmitDate.Time = CurrentInpatient.InfoOfAdmission.AdmitInfo.StepOneDate;
                    }
                    IemInfo.IemBasicInfo.AdmitDeptID = CurrentInpatient.InfoOfAdmission.AdmitInfo.CurrentDepartment.Code;
                    IemInfo.IemBasicInfo.AdmitDeptName = CurrentInpatient.InfoOfAdmission.AdmitInfo.CurrentDepartment.Name; 

                    IemInfo.IemBasicInfo.AdmitWardID = CurrentInpatient.InfoOfAdmission.AdmitInfo.CurrentWard.Code;
                    IemInfo.IemBasicInfo.AdmitWardName = CurrentInpatient.InfoOfAdmission.AdmitInfo.CurrentWard.Name;

                    IemInfo.IemDiagInfo.Vs_EmployeeID = CurrentInpatient.InfoOfAdmission.AttendingPhysician.Code;
                    IemInfo.IemDiagInfo.Vs_EmployeeName = CurrentInpatient.InfoOfAdmission.AttendingPhysician.Name;

                    IemInfo.IemDiagInfo.Resident_EmployeeID = CurrentInpatient.InfoOfAdmission.Resident.Code;
                    IemInfo.IemDiagInfo.Resident_EmployeeName = CurrentInpatient.InfoOfAdmission.Resident.Name;

                    IemInfo.IemDiagInfo.Section_DirectorID = CurrentInpatient.InfoOfAdmission.Director.Code;
                    IemInfo.IemDiagInfo.Section_DirectorName = CurrentInpatient.InfoOfAdmission.Director.Name;
                }

                #endregion


                #endregion
            }
        }

        /// <summary>
        /// 提取病人费用信息
        /// </summary>
        /// <param name="_IemInfo"></param>
        /// <returns></returns>
        public Iem_MainPage_Fee GetIemFeeInfo(IemMainPageInfo _IemInfo)
        {
            //add by yxy
            return new Iem_MainPage_Fee();

            if (_IemInfo == null || _IemInfo.IemBasicInfo == null)
                return new Iem_MainPage_Fee();
            IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess("HISDB");

            if (sqlHelper == null)
            {
                m_app.CustomMessageBox.MessageShow("无法连接到HIS！", CustomMessageBoxKind.ErrorOk);
                return new Iem_MainPage_Fee();
            }
            //to do  yxy 提取HIS数据库中病人费用信息

            string sql = string.Format(@"SELECT     CONVERT(varchar(12), PatCode) AS PatID,FeeCode,
                                             CONVERT(varchar(12), FeeName) AS FeeName, CONVERT(float, SUM(Amount)) AS amount
                                            FROM  root.InnerRecipeCount WITH (nolock)
                                            where PatCode = '{0}'
                                            GROUP BY PatCode, FeeName,FeeCode", m_app.CurrentPatientInfo.NoOfHisFirstPage);//m_App.CurrentPatientInfo.NoOfHisFirstPage);
            //SqlParameter[] paraColl = new SqlParameter[] { new SqlParameter("@syxh", m_App.CurrentPatientInfo.NoOfHisFirstPage ) };
            //DataTable dataTable = sqlHelper.ExecuteDataTable("usp_bq_fymxcx", paraColl, CommandType.StoredProcedure);

            DataTable dataTable = sqlHelper.ExecuteDataTable(sql, CommandType.Text);
            //to do 赋值
            //to do 赋值
            Double totalFee = 0;
            Double OtherFee = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                totalFee = totalFee + Convert.ToDouble(row["amount"].ToString());
                OtherFee = OtherFee + Convert.ToDouble(row["amount"].ToString());

                //床费
                if (row["FeeName"].ToString().Trim() == "床位费")
                {
                    _IemInfo.IemFeeInfo.Bed = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //护理费
                else if (row["FeeName"].ToString().Trim() == "护理费")
                {
                    _IemInfo.IemFeeInfo.Care = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //西药费
                else if (row["FeeName"].ToString().Trim() == "西药费")
                {
                    _IemInfo.IemFeeInfo.WMedical = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }

                //中成药费
                else if (row["FeeName"].ToString().Trim() == "中成药费")
                {
                    _IemInfo.IemFeeInfo.CPMedical = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //中草药费
                else if (row["FeeName"].ToString().Trim() == "草药费")
                {
                    _IemInfo.IemFeeInfo.CMedical = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //放射费
                else if (row["FeeName"].ToString().Trim() == "放射费")
                {
                    _IemInfo.IemFeeInfo.Radiate = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //检验
                else if (row["FeeName"].ToString().Trim() == "其它")
                {
                    _IemInfo.IemFeeInfo.Assay = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //输氧费
                else if (row["FeeName"].ToString().Trim() == "输氧费")
                {
                    _IemInfo.IemFeeInfo.Ox = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }

                //输血费
                else if (row["FeeName"].ToString().Trim() == "输血费")
                {
                    _IemInfo.IemFeeInfo.Blood = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //诊查费
                else if (row["FeeName"].ToString().Trim() == "诊查费")
                {
                    _IemInfo.IemFeeInfo.Mecical = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //手术费
                else if (row["FeeName"].ToString().Trim() == "手术费")
                {
                    _IemInfo.IemFeeInfo.Operation = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //接生费
                else if (row["FeeName"].ToString().Trim() == "接生费")
                {
                    _IemInfo.IemFeeInfo.Accouche = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //检查费
                else if (row["FeeName"].ToString().Trim() == "检查费")
                {
                    _IemInfo.IemFeeInfo.Ris = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }

                //麻醉费
                else if (row["FeeName"].ToString().Trim() == "麻醉费")
                {
                    _IemInfo.IemFeeInfo.Anaesthesia = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //治疗费
                else if (row["FeeName"].ToString().Trim() == "治疗费")
                {
                    _IemInfo.IemFeeInfo.Others2 = row["amount"].ToString();
                    OtherFee = OtherFee - Convert.ToDouble(row["amount"].ToString());
                }
                //婴儿费   陪床费  药占比  检验费 未匹配


            }

            _IemInfo.IemFeeInfo.Total = totalFee.ToString();

            _IemInfo.IemFeeInfo.Others3 = OtherFee.ToString();

            return _IemInfo.IemFeeInfo;
        }

        /// <summary>
        /// 诊断
        /// </summary>
        /// <param name="dataTable"></param>
        private void GetItemDiagInfo(DataTable dataTable)
        {
            DataTable dt = IemInfo.IemDiagInfo.OutDiagTable;
            dt.Rows.Clear();
            foreach (DataRow row in dataTable.Rows)
            {
                #region 赋值

                DataRow dr = dt.NewRow();
                dr["Diagnosis_Name"] = row["Diagnosis_Name"].ToString();
                dr["Status_Id"] = row["Status_Id"].ToString();
                dr["Status_Name"] = row["Status_Name"].ToString();
                dr["Diagnosis_Type_Id"] = row["Diagnosis_Type_Id"].ToString();
                dr["Diagnosis_Code"] = row["Diagnosis_Code"].ToString();
                dt.Rows.Add(dr);


                if (dr["Diagnosis_Type_Id"].ToString() == "13")
                {
                    IemInfo.IemDiagInfo.OutDiagID = row["Diagnosis_Code"].ToString();
                    IemInfo.IemDiagInfo.OutDiagName = row["Diagnosis_Name"].ToString();
                }
                else if (dr["Diagnosis_Type_Id"].ToString() == "2")
                {
                    IemInfo.IemDiagInfo.InDiagID = row["Diagnosis_Code"].ToString();
                    IemInfo.IemDiagInfo.InDiagName = row["Diagnosis_Name"].ToString();
                }

                #endregion
            }

            IemInfo.IemDiagInfo.OutDiagTable = dt;
        }


        private void GetItemObsBaby(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                #region 赋值
                IemInfo.IemObstetricsBaby.IEM_MainPage_NO = row["Iem_Mainpage_NO"].ToString();

                IemInfo.IemObstetricsBaby.TC = row["TC"].ToString();
                IemInfo.IemObstetricsBaby.TB = row["TB"].ToString();
                IemInfo.IemObstetricsBaby.CC = row["CC"].ToString();
                IemInfo.IemObstetricsBaby.CFHYPLD = row["CFHYPLD"].ToString();
                IemInfo.IemObstetricsBaby.Midwifery = row["Midwifery"].ToString();
                IemInfo.IemObstetricsBaby.Sex = row["Sex"].ToString();

                IemInfo.IemObstetricsBaby.APJ = row["APJ"].ToString();
                IemInfo.IemObstetricsBaby.Heigh = row["Heigh"].ToString();
                IemInfo.IemObstetricsBaby.Weight = row["Weight"].ToString();
                IemInfo.IemObstetricsBaby.BithDay = row["BithDay"].ToString();
                IemInfo.IemObstetricsBaby.CCQK = row["CCQK"].ToString();

                IemInfo.IemObstetricsBaby.CYQK = row["CYQK"].ToString();
                IemInfo.IemObstetricsBaby.FMFS = row["FMFS"].ToString();
                IemInfo.IemObstetricsBaby.IEM_MainPage_ObstetricsBabyID = row["IEM_MAINPAGE_OBSBABYID"].ToString();



                #endregion
                break;
            }
        }
        #endregion

     

        #endregion

        #region 保存

        public void SaveData(IemMainPageInfo m_iemInfo)
        {
            try
            {
                if (CurrentInpatient == null)
                    return;
                IemInfo = m_iemInfo;
                if (IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
                    InsertIemInfo();
                else
                    UpdateItemInfo();

            }
            catch (Exception ex)
            {
                //m_Logger.Error(ex);
            }
        }
 
        /// <summary>
        /// 保存首页信息
        /// </summary>
        /// <param name="info"></param>
        private void InsertIemInfo()
        {

            try
            {
                m_app.SqlHelper.BeginTransaction();

                InsertIemBasicInfo(this.IemInfo, m_app.SqlHelper);

                foreach (DataRow item in this.IemInfo.IemDiagInfo.OutDiagTable.Rows)
                    InserIemDiagnoseInfo(item, m_app.SqlHelper);

                foreach (DataRow item in this.IemInfo.IemOperInfo.Operation_Table.Rows)
                    InserIemOperInfo(item, m_app.SqlHelper);

                //插入产妇婴儿情况
                InsertIemObstetricsBaby(this.IemInfo.IemObstetricsBaby, m_app.SqlHelper);

                m_app.SqlHelper.CommitTransaction();

                m_app.CustomMessageBox.MessageShow("保存成功");
                GetIemInfo();
            }
            catch (Exception ex)
            {
                m_app.SqlHelper.RollbackTransaction();
            }

        }

        #region insert

        /// <summary>
        /// 根据首页实体保存首页数据
        /// </summary>
        /// <param name="info"></param>
        /// <param name="sqlHelper"></param>
        private void InsertIemBasicInfo(IemMainPageInfo info, IDataAccess sqlHelper)
        {
            info.IemBasicInfo.Create_User = m_app.User.DoctorId;

            #region
            SqlParameter paraPatNoOfHis = new SqlParameter("@PatNoOfHis", SqlDbType.VarChar, 14);
            paraPatNoOfHis.Value = info.IemBasicInfo.PatNoOfHis;
            SqlParameter paraNoOfInpat = new SqlParameter("@NoOfInpat", SqlDbType.VarChar,9);
            paraNoOfInpat.Value = info.IemBasicInfo.NoOfInpat;
            SqlParameter paraPayID = new SqlParameter("@PayID", SqlDbType.VarChar, 4);
            paraPayID.Value = info.IemBasicInfo.PayID;
            SqlParameter paraSocialCare = new SqlParameter("@SocialCare", SqlDbType.VarChar, 32);
            paraSocialCare.Value = info.IemBasicInfo.SocialCare;
            SqlParameter paraInCount = new SqlParameter("@InCount", SqlDbType.VarChar,4);
            paraInCount.Value = info.IemBasicInfo.InCount;
            SqlParameter paraName = new SqlParameter("@Name", SqlDbType.VarChar, 64);
            paraName.Value = info.IemBasicInfo.Name;
            SqlParameter paraSexID = new SqlParameter("@SexID", SqlDbType.VarChar, 4);
            paraSexID.Value = info.IemBasicInfo.SexID;
            SqlParameter paraBirth = new SqlParameter("@Birth", SqlDbType.VarChar, 10);
            paraBirth.Value = info.IemBasicInfo.Birth;
            SqlParameter paraMarital = new SqlParameter("@Marital", SqlDbType.VarChar, 4);
            paraMarital.Value = info.IemBasicInfo.Marital;
            SqlParameter paraJobID = new SqlParameter("@JobID", SqlDbType.VarChar, 4);
            paraJobID.Value = info.IemBasicInfo.JobID;
            SqlParameter paraProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 10);
            paraProvinceID.Value = info.IemBasicInfo.ProvinceID;
            SqlParameter paraCountyID = new SqlParameter("@CountyID", SqlDbType.VarChar, 10);
            paraCountyID.Value = info.IemBasicInfo.CountyID;
            SqlParameter paraNationID = new SqlParameter("@NationID", SqlDbType.VarChar, 4);
            paraNationID.Value = info.IemBasicInfo.NationID;
            SqlParameter paraNationalityID = new SqlParameter("@NationalityID", SqlDbType.VarChar, 4);
            paraNationalityID.Value = info.IemBasicInfo.NationalityID;
            SqlParameter paraIDNO = new SqlParameter("@IDNO", SqlDbType.VarChar, 18);
            paraIDNO.Value = info.IemBasicInfo.IDNO;
            SqlParameter paraOrganization = new SqlParameter("@Organization", SqlDbType.VarChar, 64);
            paraOrganization.Value = info.IemBasicInfo.Organization;
            SqlParameter paraOfficePlace = new SqlParameter("@OfficePlace", SqlDbType.VarChar, 64);
            paraOfficePlace.Value = info.IemBasicInfo.OfficePlace;
            SqlParameter paraOfficeTEL = new SqlParameter("@OfficeTEL", SqlDbType.VarChar, 16);
            paraOfficeTEL.Value = info.IemBasicInfo.OfficeTEL;
            SqlParameter paraOfficePost = new SqlParameter("@OfficePost", SqlDbType.VarChar, 16);
            paraOfficePost.Value = info.IemBasicInfo.OfficePost;
            SqlParameter paraNativeAddress = new SqlParameter("@NativeAddress", SqlDbType.VarChar, 64);
            paraNativeAddress.Value = info.IemBasicInfo.NativeAddress;
            SqlParameter paraNativeTEL = new SqlParameter("@NativeTEL", SqlDbType.VarChar, 16);
            paraNativeTEL.Value = info.IemBasicInfo.NativeTEL;
            SqlParameter paraNativePost = new SqlParameter("@NativePost", SqlDbType.VarChar, 16);
            paraNativePost.Value = info.IemBasicInfo.NativePost;
            SqlParameter paraContactPerson = new SqlParameter("@ContactPerson", SqlDbType.VarChar, 32);
            paraContactPerson.Value = info.IemBasicInfo.ContactPerson;
            SqlParameter paraRelationship = new SqlParameter("@Relationship", SqlDbType.VarChar, 4);
            paraRelationship.Value = info.IemBasicInfo.RelationshipID;
            SqlParameter paraContactAddress = new SqlParameter("@ContactAddress", SqlDbType.VarChar, 255);
            paraContactAddress.Value = info.IemBasicInfo.ContactAddress;
            SqlParameter paraContactTEL = new SqlParameter("@ContactTEL", SqlDbType.VarChar, 16);
            paraContactTEL.Value = info.IemBasicInfo.ContactTEL;
            SqlParameter paraAdmitDate = new SqlParameter("@AdmitDate", SqlDbType.VarChar, 19);
            paraAdmitDate.Value = info.IemBasicInfo.AdmitDate;
            SqlParameter paraAdmitDept = new SqlParameter("@AdmitDept", SqlDbType.VarChar, 12);
            paraAdmitDept.Value = info.IemBasicInfo.AdmitDeptID;
            SqlParameter paraAdmitWard = new SqlParameter("@AdmitWard", SqlDbType.VarChar, 12);
            paraAdmitWard.Value = info.IemBasicInfo.AdmitWardID;
            SqlParameter paraDays_Before = new SqlParameter("@Days_Before", SqlDbType.VarChar,4);
            paraDays_Before.Value = info.IemBasicInfo.Days_Before;
            SqlParameter paraTrans_Date = new SqlParameter("@Trans_Date", SqlDbType.VarChar, 19);
            paraTrans_Date.Value = info.IemBasicInfo.Trans_Date;
            SqlParameter paraTrans_AdmitDept = new SqlParameter("@Trans_AdmitDept", SqlDbType.VarChar, 12);
            paraTrans_AdmitDept.Value = info.IemBasicInfo.Trans_AdmitDeptID;
            SqlParameter paraTrans_AdmitWard = new SqlParameter("@Trans_AdmitWard", SqlDbType.VarChar, 12);
            paraTrans_AdmitWard.Value = info.IemBasicInfo.Trans_AdmitWard;
            SqlParameter paraTrans_AdmitDept_Again = new SqlParameter("@Trans_AdmitDept_Again", SqlDbType.VarChar, 12);
            paraTrans_AdmitDept_Again.Value = info.IemBasicInfo.Trans_AdmitDept_Again;
            SqlParameter paraOutWardDate = new SqlParameter("@OutWardDate", SqlDbType.VarChar, 19);
            paraOutWardDate.Value = info.IemBasicInfo.OutWardDate;
            SqlParameter paraOutHosDept = new SqlParameter("@OutHosDept", SqlDbType.VarChar, 12);
            paraOutHosDept.Value = info.IemBasicInfo.OutHosDeptID;
            SqlParameter paraOutHosWard = new SqlParameter("@OutHosWard", SqlDbType.VarChar, 12);
            paraOutHosWard.Value = info.IemBasicInfo.OutHosWardID;
            SqlParameter paraActual_Days = new SqlParameter("@Actual_Days", SqlDbType.VarChar,4);
            paraActual_Days.Value = info.IemBasicInfo.ActualDays;
            SqlParameter paraDeath_Time = new SqlParameter("@Death_Time", SqlDbType.VarChar, 19);
            paraDeath_Time.Value = info.IemBasicInfo.Death_Time;
            SqlParameter paraDeath_Reason = new SqlParameter("@Death_Reason", SqlDbType.VarChar, 300);
            paraDeath_Reason.Value = info.IemBasicInfo.Death_Reason;

            SqlParameter paraIs_Completed = new SqlParameter("@Is_Completed", SqlDbType.VarChar, 1);
            paraIs_Completed.Value = info.IemBasicInfo.Is_Completed;
            SqlParameter paracompleted_time = new SqlParameter("@completed_time", SqlDbType.VarChar, 19);
            paracompleted_time.Value = info.IemBasicInfo.completed_time;
            SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
            paraCreate_User.Value = info.IemBasicInfo.Create_User;

            SqlParameter paraXay_Sn = new SqlParameter("@Xay_Sn", SqlDbType.VarChar, 300);
            paraXay_Sn.Value = info.IemBasicInfo.Xay_Sn;
            SqlParameter paraCt_Sn = new SqlParameter("@Ct_Sn", SqlDbType.VarChar, 300);
            paraCt_Sn.Value = info.IemBasicInfo.Ct_Sn;
            SqlParameter paraMri_Sn = new SqlParameter("@Mri_Sn", SqlDbType.VarChar, 300);
            paraMri_Sn.Value = info.IemBasicInfo.Mri_Sn;
            SqlParameter paraDsa_Sn = new SqlParameter("@Dsa_Sn", SqlDbType.VarChar, 300);
            paraDsa_Sn.Value = info.IemBasicInfo.Dsa_Sn;

            SqlParameter paraAshes_Diagnosis_Name = new SqlParameter("@Ashes_Diagnosis_Name", SqlDbType.VarChar, 300);
            paraAshes_Diagnosis_Name.Value = info.IemBasicInfo.Ashes_Diagnosis_Name;

            SqlParameter paraAshes_Anatomise_Sn = new SqlParameter("@Ashes_Anatomise_Sn", SqlDbType.VarChar, 60);
            paraAshes_Anatomise_Sn.Value = info.IemBasicInfo.Ashes_Anatomise_Sn;

            //////诊断实体中数据
            SqlParameter paraAdmitInfo = new SqlParameter("@AdmitInfo", SqlDbType.VarChar, 4);
            paraAdmitInfo.Value = info.IemDiagInfo.AdmitInfo;
            SqlParameter paraIn_Check_Date = new SqlParameter("@In_Check_Date", SqlDbType.VarChar, 19);
            paraIn_Check_Date.Value = info.IemDiagInfo.In_Check_Date;
            SqlParameter paraPathology_Diagnosis_Name = new SqlParameter("@Pathology_Diagnosis_Name", SqlDbType.VarChar, 300);
            paraPathology_Diagnosis_Name.Value = info.IemDiagInfo.Pathology_Diagnosis_Name;
            SqlParameter paraPathology_Observation_Sn = new SqlParameter("@Pathology_Observation_Sn", SqlDbType.VarChar, 60);
            paraPathology_Observation_Sn.Value = info.IemDiagInfo.Pathology_Observation_Sn;
            SqlParameter paraAllergic_Drug = new SqlParameter("@Allergic_Drug", SqlDbType.VarChar, 300);
            paraAllergic_Drug.Value = info.IemDiagInfo.Allergic_Drug;
            SqlParameter paraHbsag = new SqlParameter("@Hbsag", SqlDbType.VarChar,1);
            paraHbsag.Value = info.IemDiagInfo.Hbsag;
            SqlParameter paraHcv_Ab = new SqlParameter("@Hcv_Ab", SqlDbType.VarChar, 1);
            paraHcv_Ab.Value = info.IemDiagInfo.Hcv_Ab;
            SqlParameter paraHiv_Ab = new SqlParameter("@Hiv_Ab", SqlDbType.VarChar, 1);
            paraHiv_Ab.Value = info.IemDiagInfo.Hiv_Ab;
            SqlParameter paraOpd_Ipd_Id = new SqlParameter("@Opd_Ipd_Id", SqlDbType.VarChar, 1);
            paraOpd_Ipd_Id.Value = info.IemDiagInfo.Opd_Ipd_Id;
            SqlParameter paraIn_Out_Inpatinet_Id = new SqlParameter("@In_Out_Inpatinet_Id", SqlDbType.VarChar, 1);
            paraIn_Out_Inpatinet_Id.Value = info.IemDiagInfo.In_Out_Inpatinet_Id;
            SqlParameter paraBefore_After_Or_Id = new SqlParameter("@Before_After_Or_Id", SqlDbType.VarChar, 1);
            paraBefore_After_Or_Id.Value = info.IemDiagInfo.Before_After_Or_Id;
            SqlParameter paraClinical_Pathology_Id = new SqlParameter("@Clinical_Pathology_Id", SqlDbType.VarChar, 1);
            paraClinical_Pathology_Id.Value = info.IemDiagInfo.Clinical_Pathology_Id;
            SqlParameter paraPacs_Pathology_Id = new SqlParameter("@Pacs_Pathology_Id", SqlDbType.VarChar, 1);
            paraPacs_Pathology_Id.Value = info.IemDiagInfo.Pacs_Pathology_Id;
            SqlParameter paraSave_Times = new SqlParameter("@Save_Times", SqlDbType.VarChar, 4);
            paraSave_Times.Value = info.IemDiagInfo.Save_Times;
            SqlParameter paraSuccess_Times = new SqlParameter("@Success_Times", SqlDbType.VarChar, 4);
            paraSuccess_Times.Value = info.IemDiagInfo.Success_Times;
            SqlParameter paraSection_Director = new SqlParameter("@Section_Director", SqlDbType.VarChar, 20);
            paraSection_Director.Value = info.IemDiagInfo.Section_DirectorID;
            SqlParameter paraDirector = new SqlParameter("@Director", SqlDbType.VarChar, 20);
            paraDirector.Value = info.IemDiagInfo.DirectorID;
            SqlParameter paraVs_Employee_Code = new SqlParameter("@Vs_Employee_Code", SqlDbType.VarChar, 20);
            paraVs_Employee_Code.Value = info.IemDiagInfo.Vs_EmployeeID;
            SqlParameter paraResident_Employee_Code = new SqlParameter("@Resident_Employee_Code", SqlDbType.VarChar, 20);
            paraResident_Employee_Code.Value = info.IemDiagInfo.Resident_EmployeeID;
            SqlParameter paraRefresh_Employee_Code = new SqlParameter("@Refresh_Employee_Code", SqlDbType.VarChar, 20);
            paraRefresh_Employee_Code.Value = info.IemDiagInfo.Refresh_EmployeeID;
            SqlParameter paraMaster_Interne = new SqlParameter("@Master_Interne", SqlDbType.VarChar, 20);
            paraMaster_Interne.Value = info.IemDiagInfo.Master_InterneID;
            SqlParameter paraInterne = new SqlParameter("@Interne", SqlDbType.VarChar, 20);
            paraInterne.Value = info.IemDiagInfo.InterneID;
            SqlParameter paraCoding_User = new SqlParameter("@Coding_User", SqlDbType.VarChar, 20);
            paraCoding_User.Value = info.IemDiagInfo.Coding_UserID;
            SqlParameter paraMedical_Quality_Id = new SqlParameter("@Medical_Quality_Id", SqlDbType.VarChar,1);
            paraMedical_Quality_Id.Value = info.IemDiagInfo.Medical_Quality_Id;
            SqlParameter paraQuality_Control_Doctor = new SqlParameter("@Quality_Control_Doctor", SqlDbType.VarChar, 20);
            paraQuality_Control_Doctor.Value = info.IemDiagInfo.Quality_Control_DoctorID;
            SqlParameter paraQuality_Control_Nurse = new SqlParameter("@Quality_Control_Nurse", SqlDbType.VarChar, 20);
            paraQuality_Control_Nurse.Value = info.IemDiagInfo.Quality_Control_NurseID;
            SqlParameter paraQuality_Control_Date = new SqlParameter("@Quality_Control_Date", SqlDbType.VarChar, 19);
            paraQuality_Control_Date.Value = info.IemDiagInfo.Quality_Control_Date;

            SqlParameter paraZymosis = new SqlParameter("@Zymosis", SqlDbType.VarChar, 300);
            paraZymosis.Value = info.IemDiagInfo.ZymosisID;
            SqlParameter paraHurt_Toxicosis_Ele = new SqlParameter("@Hurt_Toxicosis_Ele", SqlDbType.VarChar, 300);
            paraHurt_Toxicosis_Ele.Value = info.IemDiagInfo.Hurt_Toxicosis_Element;
            SqlParameter paraZymosisState = new SqlParameter("@ZymosisState", SqlDbType.VarChar, 300);
            paraZymosisState.Value = info.IemDiagInfo.ZymosisState;


            //////费用实体中取数据
            SqlParameter paraIs_First_Case = new SqlParameter("@Is_First_Case", SqlDbType.VarChar, 1);
            paraIs_First_Case.Value = info.IemFeeInfo.IsFirstCase;
            SqlParameter paraIs_Following = new SqlParameter("@Is_Following", SqlDbType.VarChar, 1);
            paraIs_Following.Value = info.IemFeeInfo.IsFollowing;
            SqlParameter paraFollowing_Ending_Date = new SqlParameter("@Following_Ending_Date", SqlDbType.VarChar, 19);
            paraFollowing_Ending_Date.Value = info.IemFeeInfo.Following_Ending_Date;
            SqlParameter paraIs_Teaching_Case = new SqlParameter("@Is_Teaching_Case", SqlDbType.VarChar, 1);
            paraIs_Teaching_Case.Value = info.IemFeeInfo.IsTeachingCase;
            SqlParameter paraBlood_Type_id = new SqlParameter("@Blood_Type_id", SqlDbType.VarChar, 3);
            paraBlood_Type_id.Value = info.IemFeeInfo.BloodType;
            SqlParameter paraRh = new SqlParameter("@Rh", SqlDbType.VarChar, 4);
            paraRh.Value = info.IemFeeInfo.Rh;
            SqlParameter paraBlood_Reaction_Id = new SqlParameter("@Blood_Reaction_Id", SqlDbType.VarChar, 4);
            paraBlood_Reaction_Id.Value = info.IemFeeInfo.BloodReaction;
            SqlParameter paraBlood_Rbc = new SqlParameter("@Blood_Rbc", SqlDbType.VarChar, 4);
            paraBlood_Rbc.Value = info.IemFeeInfo.Rbc;
            SqlParameter paraBlood_Plt = new SqlParameter("@Blood_Plt", SqlDbType.VarChar, 4);
            paraBlood_Plt.Value = info.IemFeeInfo.Plt;
            SqlParameter paraBlood_Plasma = new SqlParameter("@Blood_Plasma", SqlDbType.VarChar, 4);
            paraBlood_Plasma.Value = info.IemFeeInfo.Plasma;
            SqlParameter paraBlood_Wb = new SqlParameter("@Blood_Wb", SqlDbType.VarChar, 4);
            paraBlood_Wb.Value = info.IemFeeInfo.Wb;
            SqlParameter paraBlood_Others = new SqlParameter("@Blood_Others", SqlDbType.VarChar, 60);
            paraBlood_Others.Value = info.IemFeeInfo.Others;

            SqlParameter[] paraColl = new SqlParameter[] { paraPatNoOfHis, paraNoOfInpat, paraPayID, paraSocialCare, paraInCount, paraName,
                paraSexID, paraBirth, paraMarital ,paraJobID,paraProvinceID,paraCountyID,paraNationID,paraNationalityID,paraIDNO,paraOrganization,paraOfficePlace,
            paraOfficeTEL,paraOfficePost,paraNativeAddress,paraNativeTEL,paraNativePost,paraContactPerson,paraRelationship,paraContactAddress,paraContactTEL,
            paraAdmitDate,paraAdmitDept,paraAdmitWard,paraDays_Before,paraTrans_Date,paraTrans_AdmitDept,paraTrans_AdmitWard,paraTrans_AdmitDept_Again,paraOutWardDate,
            paraOutHosDept,paraOutHosWard,paraActual_Days,paraDeath_Time,paraDeath_Reason,paraAdmitInfo,paraIn_Check_Date,paraPathology_Diagnosis_Name,paraPathology_Observation_Sn,
            paraAshes_Diagnosis_Name, paraAshes_Anatomise_Sn,paraAllergic_Drug,paraHbsag,paraHcv_Ab,paraHiv_Ab,paraOpd_Ipd_Id,paraIn_Out_Inpatinet_Id,paraBefore_After_Or_Id,
            paraClinical_Pathology_Id,paraPacs_Pathology_Id,paraSave_Times,paraSuccess_Times,paraSection_Director,paraDirector,paraVs_Employee_Code,paraResident_Employee_Code,
            paraRefresh_Employee_Code,paraMaster_Interne,paraInterne,paraCoding_User,paraMedical_Quality_Id,paraQuality_Control_Doctor,paraQuality_Control_Nurse,paraQuality_Control_Date,
            paraXay_Sn,paraCt_Sn,paraMri_Sn,paraDsa_Sn,paraIs_First_Case,paraIs_Following,paraFollowing_Ending_Date,paraIs_Teaching_Case,paraBlood_Type_id,paraRh,
            paraBlood_Reaction_Id,paraBlood_Rbc,paraBlood_Plt,paraBlood_Plasma,paraBlood_Wb,paraBlood_Others,paraIs_Completed,paracompleted_time,paraCreate_User,paraZymosis,
            paraHurt_Toxicosis_Ele,paraZymosisState};

            #endregion

            string no = sqlHelper.ExecuteDataTable("IEM_MAIN_PAGE.usp_insertiembasicinfo", paraColl, CommandType.StoredProcedure).Rows[0][0].ToString();
            this.IemInfo.IemBasicInfo.Iem_Mainpage_NO = no;

        }

        /// <summary>
        /// insert diagnose info
        /// </summary>
        private void InserIemDiagnoseInfo(DataRow info, IDataAccess sqlHelper)
        {

            SqlParameter paraIem_Mainpage_NO = new SqlParameter("@Iem_Mainpage_NO", SqlDbType.VarChar,12);
            paraIem_Mainpage_NO.Value = IemInfo.IemBasicInfo.Iem_Mainpage_NO;
            SqlParameter paraDiagnosis_Type_Id = new SqlParameter("@Diagnosis_Type_Id", SqlDbType.VarChar,12);
            paraDiagnosis_Type_Id.Value = info["Diagnosis_Type_Id"];
            SqlParameter paraDiagnosis_Code = new SqlParameter("@Diagnosis_Code", SqlDbType.VarChar, 60);
            paraDiagnosis_Code.Value = info["Diagnosis_Code"];
            SqlParameter paraDiagnosis_Name = new SqlParameter("@Diagnosis_Name", SqlDbType.VarChar, 300);
            paraDiagnosis_Name.Value = info["Diagnosis_Name"];
            SqlParameter paraStatus_Id = new SqlParameter("@Status_Id", SqlDbType.VarChar,12);
            paraStatus_Id.Value = info["Status_Id"];
            SqlParameter paraOrder_Value = new SqlParameter("@Order_Value", SqlDbType.VarChar,3);
            paraOrder_Value.Value = info["Order_Value"].ToString() == "" ? "0" : info["Order_Value"].ToString();
            SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
            paraCreate_User.Value = m_app.User.DoctorId;

            SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraDiagnosis_Type_Id, paraDiagnosis_Code, paraDiagnosis_Name, paraStatus_Id, paraOrder_Value, paraCreate_User };

            sqlHelper.ExecuteNoneQuery("IEM_MAIN_PAGE.usp_Insert_Iem_Mainpage_Diag", paraColl, CommandType.StoredProcedure);
        }

        /// <summary>
        /// insert oper info
        /// </summary>
        private void InserIemOperInfo(DataRow info, IDataAccess sqlHelper)
        {
            //info.Create_User = ;
            //info.IEM_MainPage_NO = ;
            SqlParameter paraIem_Mainpage_NO = new SqlParameter("@IEM_MainPage_NO", SqlDbType.Decimal);
            paraIem_Mainpage_NO.Value = this.IemInfo.IemBasicInfo.Iem_Mainpage_NO;
            SqlParameter paraOperation_Code = new SqlParameter("@Operation_Code", SqlDbType.VarChar, 60);
            paraOperation_Code.Value = info["Operation_Code"];
            SqlParameter paraOperation_Date = new SqlParameter("@Operation_Date", SqlDbType.VarChar, 19);
            paraOperation_Date.Value = info["Operation_Date"];
            SqlParameter paraOperation_Name = new SqlParameter("@Operation_Name", SqlDbType.VarChar, 300);
            paraOperation_Name.Value = info["Operation_Name"];
            SqlParameter paraExecute_User1 = new SqlParameter("@Execute_User1", SqlDbType.VarChar, 20);
            paraExecute_User1.Value = info["Execute_User1"];
            SqlParameter paraExecute_User2 = new SqlParameter("@Execute_User2", SqlDbType.VarChar, 20);
            paraExecute_User2.Value = info["Execute_User2"];
            SqlParameter paraExecute_User3 = new SqlParameter("@Execute_User3", SqlDbType.VarChar, 20);
            paraExecute_User3.Value = info["Execute_User3"];
            SqlParameter paraAnaesthesia_Type_Id = new SqlParameter("@Anaesthesia_Type_Id", SqlDbType.VarChar,3);
            paraAnaesthesia_Type_Id.Value = info["Anaesthesia_Type_Id"];
            SqlParameter paraClose_Level = new SqlParameter("@Close_Level", SqlDbType.VarChar,3);
            paraClose_Level.Value = info["Close_Level"];
            SqlParameter paraAnaesthesia_User = new SqlParameter("@Anaesthesia_User", SqlDbType.VarChar, 20);
            paraAnaesthesia_User.Value = info["Anaesthesia_User"];
            SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
            paraCreate_User.Value = m_app.User.DoctorId;

            SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraOperation_Code, paraOperation_Date, paraOperation_Name, paraExecute_User1, paraExecute_User2, paraExecute_User3,
                paraAnaesthesia_Type_Id,paraClose_Level,paraAnaesthesia_User,paraCreate_User };

            sqlHelper.ExecuteNoneQuery("IEM_MAIN_PAGE.usp_Insert_Iem_MainPage_Oper", paraColl, CommandType.StoredProcedure);

        }

        /// <summary>
        /// 保存孕妇情况
        /// </summary>
        /// <param name="baby"></param>
        /// <param name="sqlHelper"></param>
        private void InsertIemObstetricsBaby(Iem_MainPage_ObstetricsBaby baby, IDataAccess sqlHelper)
        {
            baby.Create_User = m_app.User.DoctorId;
            baby.IEM_MainPage_NO = this.IemInfo.IemBasicInfo.Iem_Mainpage_NO.ToString();
            SqlParameter paraIem_Mainpage_NO = new SqlParameter("@iem_mainpage_no", SqlDbType.Decimal);
            paraIem_Mainpage_NO.Value = baby.IEM_MainPage_NO;
            SqlParameter paraTC = new SqlParameter("@TC", SqlDbType.VarChar, 1);
            paraTC.Value = baby.TC;
            SqlParameter paraCC = new SqlParameter("@CC", SqlDbType.VarChar, 1);
            paraCC.Value = baby.CC;
            SqlParameter paraTB = new SqlParameter("@TB", SqlDbType.VarChar, 1);
            paraTB.Value = baby.TB;
            SqlParameter paraCFHYPLD = new SqlParameter("@CFHYPLD", SqlDbType.VarChar, 1);
            paraCFHYPLD.Value = baby.CFHYPLD;
            SqlParameter paraMIDWIFERY = new SqlParameter("@MIDWIFERY", SqlDbType.VarChar, 20);
            paraMIDWIFERY.Value = baby.Midwifery;
            SqlParameter paraSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
            paraSex.Value = baby.Sex;
            SqlParameter paraAPJ = new SqlParameter("@APJ", SqlDbType.VarChar, 10);
            paraAPJ.Value = baby.APJ;
            SqlParameter paraHeigh = new SqlParameter("@Heigh", SqlDbType.VarChar, 10);
            paraHeigh.Value = baby.Heigh;
            SqlParameter paraWeight = new SqlParameter("@Weight", SqlDbType.VarChar, 10);
            paraWeight.Value = baby.Weight;
            SqlParameter paraCCQK = new SqlParameter("@CCQK", SqlDbType.VarChar, 1);
            paraCCQK.Value = baby.CCQK;
            SqlParameter paraBITHDAY = new SqlParameter("@BITHDAY", SqlDbType.VarChar, 1);
            paraBITHDAY.Value = baby.BithDay;
            SqlParameter paraFMFS = new SqlParameter("@FMFS", SqlDbType.VarChar, 1);
            paraFMFS.Value = baby.FMFS;
            SqlParameter paraCYQK = new SqlParameter("@CYQK", SqlDbType.VarChar, 1);
            paraCYQK.Value = baby.CYQK;
            SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
            paraCreate_User.Value = baby.Create_User;

            SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO,paraTC, paraCC,paraTB,paraCFHYPLD,paraMIDWIFERY, paraSex,paraAPJ,
                        paraHeigh,paraWeight,paraCCQK,paraBITHDAY,paraFMFS,paraCYQK,paraCreate_User };

            sqlHelper.ExecuteNoneQuery("IEM_MAIN_PAGE.usp_insert_iem_main_ObsBaby", paraColl, CommandType.StoredProcedure);

        }



        #endregion

        #region update
        /// <summary>
        /// 更新首页信息
        /// </summary>
        /// <param name="info"></param>
        private void UpdateItemInfo()
        {
            try
            {
                m_app.SqlHelper.BeginTransaction();

                UpdateIemBasicInfo(this.IemInfo, m_app.SqlHelper);

                // 先把之前的诊断，都给CANCLE
                UpdateIemDiagnoseInfo(this.IemInfo, m_app.SqlHelper);
                foreach (DataRow item in this.IemInfo.IemDiagInfo.OutDiagTable.Rows)
                    InserIemDiagnoseInfo(item, m_app.SqlHelper);

                // 先把之前的手术，都给CANCLE
                UpdateIemOperInfo(this.IemInfo, m_app.SqlHelper);
                foreach (DataRow item in this.IemInfo.IemOperInfo.Operation_Table.Rows)
                    InserIemOperInfo(item, m_app.SqlHelper);


                //修改产妇婴儿系想你
                DeleteIemObstetricsBaby(IemInfo.IemObstetricsBaby, m_app.SqlHelper);
                InsertIemObstetricsBaby(IemInfo.IemObstetricsBaby, m_app.SqlHelper);

                m_app.SqlHelper.CommitTransaction();

                m_app.CustomMessageBox.MessageShow("更新成功");
                GetIemInfo();
            }
            catch (Exception ex)
            {
                m_app.SqlHelper.RollbackTransaction();
            }
        }

        private void UpdateIemBasicInfo(IemMainPageInfo info, IDataAccess sqlHelper)
        {
            info.IemBasicInfo.Create_User = m_app.User.DoctorId;
            #region
            SqlParameter paraiem_mainpage_no = new SqlParameter("@iem_mainpage_no", SqlDbType.VarChar, 12);
            paraiem_mainpage_no.Value = IemInfo.IemBasicInfo.Iem_Mainpage_NO;
            SqlParameter paraPatNoOfHis = new SqlParameter("@PatNoOfHis", SqlDbType.VarChar, 14);
            paraPatNoOfHis.Value = info.IemBasicInfo.PatNoOfHis;
            SqlParameter paraNoOfInpat = new SqlParameter("@NoOfInpat", SqlDbType.VarChar, 9);
            paraNoOfInpat.Value = info.IemBasicInfo.NoOfInpat;
            SqlParameter paraPayID = new SqlParameter("@PayID", SqlDbType.VarChar, 4);
            paraPayID.Value = info.IemBasicInfo.PayID;
            SqlParameter paraSocialCare = new SqlParameter("@SocialCare", SqlDbType.VarChar, 32);
            paraSocialCare.Value = info.IemBasicInfo.SocialCare;
            
            SqlParameter paraInCount = new SqlParameter("@InCount", SqlDbType.VarChar, 4);
            paraInCount.Value = info.IemBasicInfo.InCount;
            SqlParameter paraName = new SqlParameter("@Name", SqlDbType.VarChar, 64);
            paraName.Value = info.IemBasicInfo.Name;
            SqlParameter paraSexID = new SqlParameter("@SexID", SqlDbType.VarChar, 4);
            paraSexID.Value = info.IemBasicInfo.SexID;
            SqlParameter paraBirth = new SqlParameter("@Birth", SqlDbType.VarChar, 10);
            paraBirth.Value = info.IemBasicInfo.Birth;
            SqlParameter paraMarital = new SqlParameter("@Marital", SqlDbType.VarChar, 4);
            paraMarital.Value = info.IemBasicInfo.Marital;
            
            SqlParameter paraJobID = new SqlParameter("@JobID", SqlDbType.VarChar, 4);
            paraJobID.Value = info.IemBasicInfo.JobID;
            SqlParameter paraProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 10);
            paraProvinceID.Value = info.IemBasicInfo.ProvinceID;
            SqlParameter paraCountyID = new SqlParameter("@CountyID", SqlDbType.VarChar, 10);
            paraCountyID.Value = info.IemBasicInfo.CountyID;
            SqlParameter paraNationID = new SqlParameter("@NationID", SqlDbType.VarChar, 4);
            paraNationID.Value = info.IemBasicInfo.NationID;
            SqlParameter paraNationalityID = new SqlParameter("@NationalityID", SqlDbType.VarChar, 4);
            paraNationalityID.Value = info.IemBasicInfo.NationalityID;
            
            SqlParameter paraIDNO = new SqlParameter("@IDNO", SqlDbType.VarChar, 18);
            paraIDNO.Value = info.IemBasicInfo.IDNO;
            SqlParameter paraOrganization = new SqlParameter("@Organization", SqlDbType.VarChar, 64);
            paraOrganization.Value = info.IemBasicInfo.Organization;
            SqlParameter paraOfficePlace = new SqlParameter("@OfficePlace", SqlDbType.VarChar, 64);
            paraOfficePlace.Value = info.IemBasicInfo.OfficePlace;
            SqlParameter paraOfficeTEL = new SqlParameter("@OfficeTEL", SqlDbType.VarChar, 16);
            paraOfficeTEL.Value = info.IemBasicInfo.OfficeTEL;
            SqlParameter paraOfficePost = new SqlParameter("@OfficePost", SqlDbType.VarChar, 16);
            paraOfficePost.Value = info.IemBasicInfo.OfficePost;
            
            SqlParameter paraNativeAddress = new SqlParameter("@NativeAddress", SqlDbType.VarChar, 64);
            paraNativeAddress.Value = info.IemBasicInfo.NativeAddress;
            SqlParameter paraNativeTEL = new SqlParameter("@NativeTEL", SqlDbType.VarChar, 16);
            paraNativeTEL.Value = info.IemBasicInfo.NativeTEL;
            SqlParameter paraNativePost = new SqlParameter("@NativePost", SqlDbType.VarChar, 16);
            paraNativePost.Value = info.IemBasicInfo.NativePost;
            SqlParameter paraContactPerson = new SqlParameter("@ContactPerson", SqlDbType.VarChar, 32);
            paraContactPerson.Value = info.IemBasicInfo.ContactPerson;
            SqlParameter paraRelationship = new SqlParameter("@Relationship", SqlDbType.VarChar, 4);
            paraRelationship.Value = info.IemBasicInfo.RelationshipID;
            
            SqlParameter paraContactAddress = new SqlParameter("@ContactAddress", SqlDbType.VarChar, 255);
            paraContactAddress.Value = info.IemBasicInfo.ContactAddress;
            SqlParameter paraContactTEL = new SqlParameter("@ContactTEL", SqlDbType.VarChar, 16);
            paraContactTEL.Value = info.IemBasicInfo.ContactTEL;
            SqlParameter paraAdmitDate = new SqlParameter("@AdmitDate", SqlDbType.VarChar, 19);
            paraAdmitDate.Value = info.IemBasicInfo.AdmitDate;
            SqlParameter paraAdmitDept = new SqlParameter("@AdmitDept", SqlDbType.VarChar, 12);
            paraAdmitDept.Value = info.IemBasicInfo.AdmitDeptID;
            SqlParameter paraAdmitWard = new SqlParameter("@AdmitWard", SqlDbType.VarChar, 12);
            paraAdmitWard.Value = info.IemBasicInfo.AdmitWardID;
            
            SqlParameter paraDays_Before = new SqlParameter("@Days_Before", SqlDbType.VarChar, 4);
            paraDays_Before.Value = info.IemBasicInfo.Days_Before;
            SqlParameter paraTrans_Date = new SqlParameter("@Trans_Date", SqlDbType.VarChar, 19);
            paraTrans_Date.Value = info.IemBasicInfo.Trans_Date;
            SqlParameter paraTrans_AdmitDept = new SqlParameter("@Trans_AdmitDept", SqlDbType.VarChar, 12);
            paraTrans_AdmitDept.Value = info.IemBasicInfo.Trans_AdmitDeptID;
            SqlParameter paraTrans_AdmitWard = new SqlParameter("@Trans_AdmitWard", SqlDbType.VarChar, 12);
            paraTrans_AdmitWard.Value = info.IemBasicInfo.Trans_AdmitWard;
            SqlParameter paraTrans_AdmitDept_Again = new SqlParameter("@Trans_AdmitDept_Again", SqlDbType.VarChar, 12);
            paraTrans_AdmitDept_Again.Value = info.IemBasicInfo.Trans_AdmitDept_Again;
            
            SqlParameter paraOutWardDate = new SqlParameter("@OutWardDate", SqlDbType.VarChar, 19);
            paraOutWardDate.Value = info.IemBasicInfo.OutWardDate;
            SqlParameter paraOutHosDept = new SqlParameter("@OutHosDept", SqlDbType.VarChar, 12);
            paraOutHosDept.Value = info.IemBasicInfo.OutHosDeptID;
            SqlParameter paraOutHosWard = new SqlParameter("@OutHosWard", SqlDbType.VarChar, 12);
            paraOutHosWard.Value = info.IemBasicInfo.OutHosWardID;
            SqlParameter paraActual_Days = new SqlParameter("@Actual_Days", SqlDbType.VarChar, 4);
            paraActual_Days.Value = info.IemBasicInfo.ActualDays;
            SqlParameter paraDeath_Time = new SqlParameter("@Death_Time", SqlDbType.VarChar, 19);
            paraDeath_Time.Value = info.IemBasicInfo.Death_Time;
            SqlParameter paraDeath_Reason = new SqlParameter("@Death_Reason", SqlDbType.VarChar, 300);
            paraDeath_Reason.Value = info.IemBasicInfo.Death_Reason;

            SqlParameter paraIs_Completed = new SqlParameter("@Is_Completed", SqlDbType.VarChar, 1);
            paraIs_Completed.Value = info.IemBasicInfo.Is_Completed;
            SqlParameter paracompleted_time = new SqlParameter("@completed_time", SqlDbType.VarChar, 19);
            paracompleted_time.Value = info.IemBasicInfo.completed_time;
            //SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
            //paraCreate_User.Value = info.IemBasicInfo.Create_User;

            SqlParameter paraXay_Sn = new SqlParameter("@Xay_Sn", SqlDbType.VarChar, 300);
            paraXay_Sn.Value = info.IemBasicInfo.Xay_Sn;
            SqlParameter paraCt_Sn = new SqlParameter("@Ct_Sn", SqlDbType.VarChar, 300);
            paraCt_Sn.Value = info.IemBasicInfo.Ct_Sn;
            SqlParameter paraMri_Sn = new SqlParameter("@Mri_Sn", SqlDbType.VarChar, 300);
            paraMri_Sn.Value = info.IemBasicInfo.Mri_Sn;
            SqlParameter paraDsa_Sn = new SqlParameter("@Dsa_Sn", SqlDbType.VarChar, 300);
            paraDsa_Sn.Value = info.IemBasicInfo.Dsa_Sn;

            SqlParameter paraAshes_Diagnosis_Name = new SqlParameter("@Ashes_Diagnosis_Name", SqlDbType.VarChar, 300);
            paraAshes_Diagnosis_Name.Value = info.IemBasicInfo.Ashes_Diagnosis_Name;

            SqlParameter paraAshes_Anatomise_Sn = new SqlParameter("@Ashes_Anatomise_Sn", SqlDbType.VarChar, 60);
            paraAshes_Anatomise_Sn.Value = info.IemBasicInfo.Ashes_Anatomise_Sn;

            //////诊断实体中数据
            SqlParameter paraAdmitInfo = new SqlParameter("@AdmitInfo", SqlDbType.VarChar, 4);
            paraAdmitInfo.Value = info.IemDiagInfo.AdmitInfo;
            SqlParameter paraIn_Check_Date = new SqlParameter("@In_Check_Date", SqlDbType.VarChar, 19);
            paraIn_Check_Date.Value = info.IemDiagInfo.In_Check_Date;
            SqlParameter paraPathology_Diagnosis_Name = new SqlParameter("@Pathology_Diagnosis_Name", SqlDbType.VarChar, 300);
            paraPathology_Diagnosis_Name.Value = info.IemDiagInfo.Pathology_Diagnosis_Name;
            SqlParameter paraPathology_Observation_Sn = new SqlParameter("@Pathology_Observation_Sn", SqlDbType.VarChar, 60);
            paraPathology_Observation_Sn.Value = info.IemDiagInfo.Pathology_Observation_Sn;
            SqlParameter paraAllergic_Drug = new SqlParameter("@Allergic_Drug", SqlDbType.VarChar, 300);
            paraAllergic_Drug.Value = info.IemDiagInfo.Allergic_Drug;
            SqlParameter paraHbsag = new SqlParameter("@Hbsag", SqlDbType.VarChar, 1);
            paraHbsag.Value = info.IemDiagInfo.Hbsag;
            SqlParameter paraHcv_Ab = new SqlParameter("@Hcv_Ab", SqlDbType.VarChar, 1);
            paraHcv_Ab.Value = info.IemDiagInfo.Hcv_Ab;
            SqlParameter paraHiv_Ab = new SqlParameter("@Hiv_Ab", SqlDbType.VarChar, 1);
            paraHiv_Ab.Value = info.IemDiagInfo.Hiv_Ab;
            SqlParameter paraOpd_Ipd_Id = new SqlParameter("@Opd_Ipd_Id", SqlDbType.VarChar, 1);
            paraOpd_Ipd_Id.Value = info.IemDiagInfo.Opd_Ipd_Id;
            SqlParameter paraIn_Out_Inpatinet_Id = new SqlParameter("@In_Out_Inpatinet_Id", SqlDbType.VarChar, 1);
            paraIn_Out_Inpatinet_Id.Value = info.IemDiagInfo.In_Out_Inpatinet_Id;
            SqlParameter paraBefore_After_Or_Id = new SqlParameter("@Before_After_Or_Id", SqlDbType.VarChar, 1);
            paraBefore_After_Or_Id.Value = info.IemDiagInfo.Before_After_Or_Id;
            SqlParameter paraClinical_Pathology_Id = new SqlParameter("@Clinical_Pathology_Id", SqlDbType.VarChar, 1);
            paraClinical_Pathology_Id.Value = info.IemDiagInfo.Clinical_Pathology_Id;
            SqlParameter paraPacs_Pathology_Id = new SqlParameter("@Pacs_Pathology_Id", SqlDbType.VarChar, 1);
            paraPacs_Pathology_Id.Value = info.IemDiagInfo.Pacs_Pathology_Id;
            SqlParameter paraSave_Times = new SqlParameter("@Save_Times", SqlDbType.VarChar, 4);
            paraSave_Times.Value = info.IemDiagInfo.Save_Times;
            SqlParameter paraSuccess_Times = new SqlParameter("@Success_Times", SqlDbType.VarChar, 4);
            paraSuccess_Times.Value = info.IemDiagInfo.Success_Times;
            SqlParameter paraSection_Director = new SqlParameter("@Section_Director", SqlDbType.VarChar, 20);
            paraSection_Director.Value = info.IemDiagInfo.Section_DirectorID;
            SqlParameter paraDirector = new SqlParameter("@Director", SqlDbType.VarChar, 20);
            paraDirector.Value = info.IemDiagInfo.DirectorID;
            SqlParameter paraVs_Employee_Code = new SqlParameter("@Vs_Employee_Code", SqlDbType.VarChar, 20);
            paraVs_Employee_Code.Value = info.IemDiagInfo.Vs_EmployeeID;
            SqlParameter paraResident_Employee_Code = new SqlParameter("@Resident_Employee_Code", SqlDbType.VarChar, 20);
            paraResident_Employee_Code.Value = info.IemDiagInfo.Resident_EmployeeID;
            SqlParameter paraRefresh_Employee_Code = new SqlParameter("@Refresh_Employee_Code", SqlDbType.VarChar, 20);
            paraRefresh_Employee_Code.Value = info.IemDiagInfo.Refresh_EmployeeID;
            SqlParameter paraMaster_Interne = new SqlParameter("@Master_Interne", SqlDbType.VarChar, 20);
            paraMaster_Interne.Value = info.IemDiagInfo.Master_InterneID;
            SqlParameter paraInterne = new SqlParameter("@Interne", SqlDbType.VarChar, 20);
            paraInterne.Value = info.IemDiagInfo.InterneID;
            SqlParameter paraCoding_User = new SqlParameter("@Coding_User", SqlDbType.VarChar, 20);
            paraCoding_User.Value = info.IemDiagInfo.Coding_UserID;
            SqlParameter paraMedical_Quality_Id = new SqlParameter("@Medical_Quality_Id", SqlDbType.VarChar,1);
            paraMedical_Quality_Id.Value = info.IemDiagInfo.Medical_Quality_Id;
            SqlParameter paraQuality_Control_Doctor = new SqlParameter("@Quality_Control_Doctor", SqlDbType.VarChar, 20);
            paraQuality_Control_Doctor.Value = info.IemDiagInfo.Quality_Control_DoctorID;
            SqlParameter paraQuality_Control_Nurse = new SqlParameter("@Quality_Control_Nurse", SqlDbType.VarChar, 20);
            paraQuality_Control_Nurse.Value = info.IemDiagInfo.Quality_Control_NurseID;
            SqlParameter paraQuality_Control_Date = new SqlParameter("@Quality_Control_Date", SqlDbType.VarChar, 19);
            paraQuality_Control_Date.Value = info.IemDiagInfo.Quality_Control_Date;

            //////费用实体中取数据
            SqlParameter paraIs_First_Case = new SqlParameter("@Is_First_Case", SqlDbType.VarChar, 1);
            paraIs_First_Case.Value = info.IemFeeInfo.IsFirstCase;
            SqlParameter paraIs_Following = new SqlParameter("@Is_Following", SqlDbType.VarChar, 1);
            paraIs_Following.Value = info.IemFeeInfo.IsFollowing;
            SqlParameter paraFollowing_Ending_Date = new SqlParameter("@Following_Ending_Date", SqlDbType.VarChar, 19);
            paraFollowing_Ending_Date.Value = info.IemFeeInfo.Following_Ending_Date;
            SqlParameter paraIs_Teaching_Case = new SqlParameter("@Is_Teaching_Case", SqlDbType.VarChar, 1);
            paraIs_Teaching_Case.Value = info.IemFeeInfo.IsTeachingCase;
            SqlParameter paraBlood_Type_id = new SqlParameter("@Blood_Type_id", SqlDbType.VarChar, 3);
            paraBlood_Type_id.Value = info.IemFeeInfo.BloodType;
            SqlParameter paraRh = new SqlParameter("@Rh", SqlDbType.VarChar, 4);
            paraRh.Value = info.IemFeeInfo.Rh;
            SqlParameter paraBlood_Reaction_Id = new SqlParameter("@Blood_Reaction_Id", SqlDbType.VarChar, 4);
            paraBlood_Reaction_Id.Value = info.IemFeeInfo.BloodReaction;
            SqlParameter paraBlood_Rbc = new SqlParameter("@Blood_Rbc", SqlDbType.VarChar, 4);
            paraBlood_Rbc.Value = info.IemFeeInfo.Rbc;
            SqlParameter paraBlood_Plt = new SqlParameter("@Blood_Plt", SqlDbType.VarChar, 4);
            paraBlood_Plt.Value = info.IemFeeInfo.Plt;
            SqlParameter paraBlood_Plasma = new SqlParameter("@Blood_Plasma", SqlDbType.VarChar, 4);
            paraBlood_Plasma.Value = info.IemFeeInfo.Plasma;
            SqlParameter paraBlood_Wb = new SqlParameter("@Blood_Wb", SqlDbType.VarChar, 4);
            paraBlood_Wb.Value = info.IemFeeInfo.Wb;
            SqlParameter paraBlood_Others = new SqlParameter("@Blood_Others", SqlDbType.VarChar, 60);
            paraBlood_Others.Value = info.IemFeeInfo.Others;

            SqlParameter paraZymosis = new SqlParameter("@Zymosis", SqlDbType.VarChar, 300);
            paraZymosis.Value = info.IemDiagInfo.ZymosisID;
            SqlParameter paraHurt_Toxicosis_Ele = new SqlParameter("@Hurt_Toxicosis_Ele", SqlDbType.VarChar, 300);
            paraHurt_Toxicosis_Ele.Value = info.IemDiagInfo.Hurt_Toxicosis_Element;
            SqlParameter paraZymosisState = new SqlParameter("@ZymosisState", SqlDbType.VarChar, 300);
            paraZymosisState.Value = info.IemDiagInfo.ZymosisState;

            SqlParameter[] paraColl = new SqlParameter[] {paraiem_mainpage_no, paraPatNoOfHis, paraNoOfInpat, paraPayID, paraSocialCare, paraInCount, paraName,
                paraSexID, paraBirth, paraMarital ,paraJobID,paraProvinceID,paraCountyID,paraNationID,paraNationalityID,paraIDNO,paraOrganization,paraOfficePlace,
            paraOfficeTEL,paraOfficePost,paraNativeAddress,paraNativeTEL,paraNativePost,paraContactPerson,paraRelationship,paraContactAddress,paraContactTEL,
            paraAdmitDate,paraAdmitDept,paraAdmitWard,paraDays_Before,paraTrans_Date,paraTrans_AdmitDept,paraTrans_AdmitWard,paraTrans_AdmitDept_Again,paraOutWardDate,
            paraOutHosDept,paraOutHosWard,paraActual_Days,paraDeath_Time,paraDeath_Reason,paraAdmitInfo,paraIn_Check_Date,paraPathology_Diagnosis_Name,paraPathology_Observation_Sn,
            paraAshes_Diagnosis_Name, paraAshes_Anatomise_Sn,paraAllergic_Drug,paraHbsag,paraHcv_Ab,paraHiv_Ab,paraOpd_Ipd_Id,paraIn_Out_Inpatinet_Id,paraBefore_After_Or_Id,
            paraClinical_Pathology_Id,paraPacs_Pathology_Id,paraSave_Times,paraSuccess_Times,paraSection_Director,paraDirector,paraVs_Employee_Code,paraResident_Employee_Code,
            paraRefresh_Employee_Code,paraMaster_Interne,paraInterne,paraCoding_User,paraMedical_Quality_Id,paraQuality_Control_Doctor,paraQuality_Control_Nurse,paraQuality_Control_Date,
            paraXay_Sn,paraCt_Sn,paraMri_Sn,paraDsa_Sn,paraIs_First_Case,paraIs_Following,paraFollowing_Ending_Date,paraIs_Teaching_Case,paraBlood_Type_id,paraRh,
            paraBlood_Reaction_Id,paraBlood_Rbc,paraBlood_Plt,paraBlood_Plasma,paraBlood_Wb,paraBlood_Others,paraIs_Completed,paracompleted_time,paraZymosis,
            paraHurt_Toxicosis_Ele,paraZymosisState};

            #endregion

            string no = sqlHelper.ExecuteDataTable("IEM_MAIN_PAGE.usp_Upateiembasicinfo", paraColl, CommandType.StoredProcedure).Rows[0][0].ToString();
            this.IemInfo.IemBasicInfo.Iem_Mainpage_NO = no;
        }

        /// <summary>
        /// 取消之前的诊断信息
        /// </summary>
        private void UpdateIemDiagnoseInfo(IemMainPageInfo info, IDataAccess sqlHelper)
        {
            info.IemBasicInfo.Create_User = m_app.User.DoctorId;
            SqlParameter paraIem_Mainpage_NO = new SqlParameter("@Iem_Mainpage_NO", SqlDbType.Decimal);
            paraIem_Mainpage_NO.Value = info.IemBasicInfo.Iem_Mainpage_NO;
            SqlParameter paraCreate_User = new SqlParameter("@Cancel_User", SqlDbType.VarChar, 10);
            paraCreate_User.Value = info.IemBasicInfo.Create_User;

            SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraCreate_User };

            sqlHelper.ExecuteNoneQuery("usp_Update_Iem_Mainpage_Diag", paraColl, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 取消之前的手术信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="sqlHelper"></param>
        private void UpdateIemOperInfo(IemMainPageInfo info, IDataAccess sqlHelper)
        {
            info.IemBasicInfo.Create_User = m_app.User.DoctorId;
            SqlParameter paraIem_Mainpage_NO = new SqlParameter("@IEM_MainPage_NO", SqlDbType.Decimal);
            paraIem_Mainpage_NO.Value = info.IemBasicInfo.Iem_Mainpage_NO;
            SqlParameter paraCreate_User = new SqlParameter("@Cancel_User", SqlDbType.VarChar, 10);
            paraCreate_User.Value = info.IemBasicInfo.Create_User;

            SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraCreate_User };

            sqlHelper.ExecuteNoneQuery("usp_Update_Iem_MainPage_Oper", paraColl, CommandType.StoredProcedure);

        }

        /// <summary>
        /// 取消之前的产妇婴儿信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="sqlHelper"></param>
        private void DeleteIemObstetricsBaby(Iem_MainPage_ObstetricsBaby info, IDataAccess sqlHelper)
        {
            string sql = string.Format("delete IEM_MAINPAGE_OBSTETRICSBABY where IEM_MAINPAGE_NO = '{0}'", IemInfo.IemBasicInfo.Iem_Mainpage_NO);

            sqlHelper.ExecuteNoneQuery(sql, CommandType.Text);

        }

        #endregion
        #endregion
    }
}
