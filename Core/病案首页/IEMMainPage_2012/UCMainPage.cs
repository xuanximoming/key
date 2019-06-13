using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

using YidanSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;

using YidanSoft.FrameWork.BizBus;

using Convertmy = YidanSoft.Core.UtilsForExtension;
using YidanSoft.Common.Library;
using YidanSoft.Wordbook;
using DevExpress.XtraBars;
using YidanSoft.FrameWork.WinForm;
using YidanSoft.FrameWork;
using System.Diagnostics;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class UCMainPage : DevExpress.XtraEditors.XtraUserControl, IEMREditor//,IStartPlugIn
    {
        #region fields & propertys
        private UCOthers ucOthers;
        private UCIemOperInfo ucIemOperInfo;
        private UCIemDiagnose ucIemDiagnose;
        private UCIemBasInfo ucIemBasInfo;
        private UCObstetricsBaby ucIemBabytInfo;

        private IYidanEmrHost m_app;
        private IYidanSoftLog m_Logger;
        private IBizBus m_BizBus;
        private IemMainPageInfo m_IemInfo = new IemMainPageInfo();

        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get { return m_IemInfo; }
            set { m_IemInfo = value; }
        }
        #endregion

        public UCMainPage()
            : this(null)
        {
        }
        public UCMainPage(IYidanEmrHost app)
        {

            InitializeComponent();
            m_app = app;
            //this.Load += new EventHandler(UCMainPage_Load);
            simpleButtonSave.Click += new EventHandler(simpleButtonSave_Click);

            //m_app.ChoosePatient(150);//切换病人
        }

        private void InitUserControl()
        {
            AddUserControl();
            this.ucIemBasInfo.BackColor = Color.White;
            this.ucIemDiagnose.BackColor = Color.White;
            this.ucIemOperInfo.BackColor = Color.White;
            this.ucOthers.BackColor = Color.White;

            if (ucIemBabytInfo == null)
                this.Height = ucOthers.Location.Y + ucOthers.Height + 20;
            else
            {
                this.ucIemBabytInfo.BackColor = Color.White;
                this.Height = ucIemBabytInfo.Location.Y + ucIemBabytInfo.Height + 20;
            }
            InitLocation();
        }

        void simpleButtonSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        /// <summary>
        /// 在界面中增加用户控件
        /// </summary>
        private void AddUserControl()
        {
            ucOthers = new UCOthers();
            ucIemOperInfo = new UCIemOperInfo();
            ucIemDiagnose = new UCIemDiagnose();
            ucIemBasInfo = new UCIemBasInfo();

            this.Controls.Add(ucOthers);
            this.Controls.Add(ucIemOperInfo);
            this.Controls.Add(ucIemDiagnose);
            this.Controls.Add(ucIemBasInfo);

            if (m_app.User.CurrentDeptId == "2401")
            {
                ucIemBabytInfo = new UCObstetricsBaby();
                this.Controls.Add(ucIemBabytInfo);
            }
        }

        void UCMainPage_Load(object sender, EventArgs e)
        {
            DateTime oTimeBegin = DateTime.Now;
            //InitLocation();

            if (m_app.CurrentPatientInfo == null)
                return;

            //delegateLoad deleteLoad = new delegateLoad(LoadForm);
            //deleteLoad.Invoke();
            LoadForm();

            TimeSpan oTime = DateTime.Now.Subtract(oTimeBegin);
            m_app.CustomMessageBox.MessageShow("UCMainPage_Load" + oTime.Milliseconds + "毫秒");
        }

        private void LoadForm()
        {
            //m_BizBus = BusFactory.GetBus();
            //m_Logger = m_BizBus.BuildUp<IYidanSoftLog>(new string[] { "病案首页" });
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(141)))), ((int)(((byte)(189)))));
            GetIemInfo();
            FillUI();
        }

        delegate void delegateLoad();

        /// <summary>
        /// 初始化控件位置
        /// </summary>
        private void InitLocation()
        {
            Int32 pointX = (this.Width - this.ucIemBasInfo.Width) / 2;
            Int32 pointY = 20;
            this.ucIemBasInfo.Location = new Point(pointX, pointY);
            this.ucIemDiagnose.Location = new Point(pointX, pointY + ucIemBasInfo.Height);
            this.ucIemOperInfo.Location = new Point(pointX, pointY + ucIemBasInfo.Height + ucIemDiagnose.Height);
            this.ucOthers.Location = new Point(pointX, pointY + ucIemBasInfo.Height + ucIemDiagnose.Height + ucIemOperInfo.Height);
            if (ucIemBabytInfo != null)
            {
                this.ucIemBabytInfo.Location = new Point(pointX, pointY + ucIemBasInfo.Height + ucIemDiagnose.Height + ucIemOperInfo.Height + ucOthers.Height);
            }
        }

        #region 根据首页序号得到病案首页的信息，并给界面赋值
        /// <summary>
        /// LOAD时获取病案首页信息
        /// </summary>
        private void GetIemInfo()
        {
            //首先去Iem_Mainpage_Basicinfo根据首页序号捞取资料，如果没有，则LOAD基本用户信息
            SqlParameter[] paraCollection = new SqlParameter[] { new SqlParameter("@NoOfInpat", m_app.CurrentPatientInfo.NoOfFirstPage) };
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
                IemInfo.IemBasicInfo.PatNoOfHis = m_app.CurrentPatientInfo.NoOfHisFirstPage;
                IemInfo.IemBasicInfo.NoOfInpat = m_app.CurrentPatientInfo.NoOfFirstPage.ToString();
            }


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
        }

        /// <summary>
        /// 诊断
        /// </summary>
        /// <param name="dataTable"></param>
        private void GetItemDiagInfo(DataTable dataTable)
        {
            DataTable dt = m_IemInfo.IemDiagInfo.OutDiagTable;
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

                #endregion
            }

            m_IemInfo.IemDiagInfo.OutDiagTable = dt;
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

        private void FillUI()
        {
            //DateTime oTimeBegin = DateTime.Now;
            this.ucIemBasInfo.FillUI(IemInfo, m_app);
            this.ucIemDiagnose.FillUI(IemInfo, m_app);
            this.ucIemOperInfo.FillUI(IemInfo, m_app);
            this.ucOthers.FillUI(IemInfo, m_app);

            if (ucIemBabytInfo != null)
                this.ucIemBabytInfo.FillUI(IemInfo, m_app);

            //TimeSpan oTime = DateTime.Now.Subtract(oTimeBegin);
            ////m_app.CustomMessageBox.MessageShow("Load" + oTime.Milliseconds + "毫秒");
            //MessageBox.Show(oTime.Milliseconds + "毫秒");
        }

        #endregion

        #region 保存

        public void SaveData()
        {
            try
            {
                if (m_app.CurrentPatientInfo == null)
                    return;
                GetUI();
                if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
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
        /// 将得到的数据显示在用户控件中
        /// </summary>
        private void GetUI()
        {
            //this.IemInfo = this.ucIemBasInfo.IemInfo;

            //IemMainPageInfo infoDiag = this.ucIemDiagnose.IemInfo;
            //IemMainPageInfo infoOper = this.ucIemOperInfo.IemInfo;
            //IemMainPageInfo infoOther = this.ucOthers.IemInfo;
            //IemMainPageInfo infoObstetircsBaby = this.ucIemBabytInfo.IemInfo;


            //this.IemInfo.IemBasicInfo.AdmitInfo = infoDiag.IemBasicInfo.AdmitInfo;
            //this.IemInfo.IemBasicInfo.Pathology_Diagnosis_Name = infoDiag.IemBasicInfo.Pathology_Diagnosis_Name;
            //this.IemInfo.IemBasicInfo.Pathology_Observation_Sn = infoDiag.IemBasicInfo.Pathology_Observation_Sn;
            //this.IemInfo.IemBasicInfo.Ashes_Diagnosis_Name = infoDiag.IemBasicInfo.Ashes_Diagnosis_Name;
            //this.IemInfo.IemBasicInfo.Ashes_Anatomise_Sn = infoDiag.IemBasicInfo.Ashes_Anatomise_Sn;
            //this.IemInfo.IemBasicInfo.Allergic_Drug = infoDiag.IemBasicInfo.Allergic_Drug;
            //this.IemInfo.IemBasicInfo.Hbsag = infoDiag.IemBasicInfo.Hbsag;
            //this.IemInfo.IemBasicInfo.Hcv_Ab = infoDiag.IemBasicInfo.Hcv_Ab;
            //this.IemInfo.IemBasicInfo.Hiv_Ab = infoDiag.IemBasicInfo.Hiv_Ab;
            //this.IemInfo.IemBasicInfo.Opd_Ipd_Id = infoDiag.IemBasicInfo.Opd_Ipd_Id;
            //this.IemInfo.IemBasicInfo.In_Out_Inpatinet_Id = infoDiag.IemBasicInfo.In_Out_Inpatinet_Id;
            //this.IemInfo.IemBasicInfo.Before_After_Or_Id = infoDiag.IemBasicInfo.Before_After_Or_Id;
            //this.IemInfo.IemBasicInfo.Clinical_Pathology_Id = infoDiag.IemBasicInfo.Clinical_Pathology_Id;
            //this.IemInfo.IemBasicInfo.Pacs_Pathology_Id = infoDiag.IemBasicInfo.Pacs_Pathology_Id;
            //this.IemInfo.IemBasicInfo.Save_Times = infoDiag.IemBasicInfo.Save_Times;
            //this.IemInfo.IemBasicInfo.Success_Times = infoDiag.IemBasicInfo.Success_Times;
            //this.IemInfo.IemBasicInfo.In_Check_Date = infoDiag.IemBasicInfo.In_Check_Date;
            ////基本诊断
            //this.IemInfo.IemDiagInfo = infoDiag.IemDiagInfo;
            ////手术诊断
            //if (infoOper.IemDiagInfo != null)
            //{
            //    foreach (Iem_Mainpage_Diagnosis infoDia in infoOper.IemDiagInfo)
            //    {
            //        this.IemInfo.IemDiagInfo.Add(infoDia);
            //    }
            //}


            //this.IemInfo.IemBasicInfo.Xay_Sn = infoOper.IemBasicInfo.Xay_Sn;
            //this.IemInfo.IemBasicInfo.Ct_Sn = infoOper.IemBasicInfo.Ct_Sn;
            //this.IemInfo.IemBasicInfo.Mri_Sn = infoOper.IemBasicInfo.Mri_Sn;
            //this.IemInfo.IemBasicInfo.Dsa_Sn = infoOper.IemBasicInfo.Dsa_Sn;

            //this.IemInfo.IemOperInfo = infoOper.IemOperInfo;

            //this.IemInfo.IemBasicInfo.Is_First_Case = infoOther.IemBasicInfo.Is_First_Case;
            //this.IemInfo.IemBasicInfo.Is_Following = infoOther.IemBasicInfo.Is_Following;
            //this.IemInfo.IemBasicInfo.Is_Teaching_Case = infoOther.IemBasicInfo.Is_Teaching_Case;
            //this.IemInfo.IemBasicInfo.Blood_Type_id = infoOther.IemBasicInfo.Blood_Type_id;
            //this.IemInfo.IemBasicInfo.Blood_Type_id = infoOther.IemBasicInfo.Blood_Type_id;
            //this.IemInfo.IemBasicInfo.Blood_Reaction_Id = infoOther.IemBasicInfo.Blood_Reaction_Id;
            //this.IemInfo.IemBasicInfo.Blood_Rbc = infoOther.IemBasicInfo.Blood_Rbc;
            //this.IemInfo.IemBasicInfo.Blood_Plt = infoOther.IemBasicInfo.Blood_Plt;
            //this.IemInfo.IemBasicInfo.Blood_Plasma = infoOther.IemBasicInfo.Blood_Plasma;
            //this.IemInfo.IemBasicInfo.Blood_Wb = infoOther.IemBasicInfo.Blood_Wb;
            //this.IemInfo.IemBasicInfo.Blood_Others = infoOther.IemBasicInfo.Blood_Others;
            //this.IemInfo.IemBasicInfo.Following_Ending_Date = infoOther.IemBasicInfo.Following_Ending_Date;

            //this.IemInfo.IemBasicInfo.Zymosis = infoDiag.IemBasicInfo.Zymosis;
            //this.IemInfo.IemBasicInfo.Hurt_Toxicosis_Ele = infoDiag.IemBasicInfo.Hurt_Toxicosis_Ele;
            //this.IemInfo.IemBasicInfo.ZymosisState = infoDiag.IemBasicInfo.ZymosisState;

            //this.IemInfo.IemObstetricsBaby = infoObstetircsBaby.IemObstetricsBaby;
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
                FillUI();
            }
            catch (Exception ex)
            {
                m_app.SqlHelper.RollbackTransaction();
            }

        }

        //#region insert

        ///// <summary>
        ///// 根据首页实体保存首页数据
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="sqlHelper"></param>
        //private void InsertIemBasicInfo(IemMainPageInfo info, IDataAccess sqlHelper)
        //{
        //    info.IemBasicInfo.Create_User = m_app.User.DoctorId;

        //    #region
        //    SqlParameter paraPatNoOfHis = new SqlParameter("@PatNoOfHis", SqlDbType.VarChar, 14);
        //    paraPatNoOfHis.Value = info.IemBasicInfo.PatNoOfHis;
        //    SqlParameter paraNoOfInpat = new SqlParameter("@NoOfInpat", SqlDbType.Decimal);
        //    paraNoOfInpat.Value = info.IemBasicInfo.NoOfInpat;
        //    SqlParameter paraPayID = new SqlParameter("@PayID", SqlDbType.VarChar, 4);
        //    paraPayID.Value = info.IemBasicInfo.PayID;
        //    SqlParameter paraSocialCare = new SqlParameter("@SocialCare", SqlDbType.VarChar, 32);
        //    paraSocialCare.Value = info.IemBasicInfo.SocialCare;
        //    SqlParameter paraInCount = new SqlParameter("@InCount", SqlDbType.Int);
        //    paraInCount.Value = info.IemBasicInfo.InCount;
        //    SqlParameter paraName = new SqlParameter("@Name", SqlDbType.VarChar, 64);
        //    paraName.Value = info.IemBasicInfo.Name;
        //    SqlParameter paraSexID = new SqlParameter("@SexID", SqlDbType.VarChar, 4);
        //    paraSexID.Value = info.IemBasicInfo.SexID;
        //    SqlParameter paraBirth = new SqlParameter("@Birth", SqlDbType.VarChar, 10);
        //    paraBirth.Value = info.IemBasicInfo.Birth;
        //    SqlParameter paraMarital = new SqlParameter("@Marital", SqlDbType.VarChar, 4);
        //    paraMarital.Value = info.IemBasicInfo.Marital;
        //    SqlParameter paraJobID = new SqlParameter("@JobID", SqlDbType.VarChar, 4);
        //    paraJobID.Value = info.IemBasicInfo.JobID;
        //    SqlParameter paraProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 10);
        //    paraProvinceID.Value = info.IemBasicInfo.ProvinceID;
        //    SqlParameter paraCountyID = new SqlParameter("@CountyID", SqlDbType.VarChar, 10);
        //    paraCountyID.Value = info.IemBasicInfo.CountyID;
        //    SqlParameter paraNationID = new SqlParameter("@NationID", SqlDbType.VarChar, 4);
        //    paraNationID.Value = info.IemBasicInfo.NationID;
        //    SqlParameter paraNationalityID = new SqlParameter("@NationalityID", SqlDbType.VarChar, 4);
        //    paraNationalityID.Value = info.IemBasicInfo.NationalityID;
        //    SqlParameter paraIDNO = new SqlParameter("@IDNO", SqlDbType.VarChar, 18);
        //    paraIDNO.Value = info.IemBasicInfo.IDNO;
        //    SqlParameter paraOrganization = new SqlParameter("@Organization", SqlDbType.VarChar, 64);
        //    paraOrganization.Value = info.IemBasicInfo.Organization;
        //    SqlParameter paraOfficePlace = new SqlParameter("@OfficePlace", SqlDbType.VarChar, 64);
        //    paraOfficePlace.Value = info.IemBasicInfo.OfficePlace;
        //    SqlParameter paraOfficeTEL = new SqlParameter("@OfficeTEL", SqlDbType.VarChar, 16);
        //    paraOfficeTEL.Value = info.IemBasicInfo.OfficeTEL;
        //    SqlParameter paraOfficePost = new SqlParameter("@OfficePost", SqlDbType.VarChar, 16);
        //    paraOfficePost.Value = info.IemBasicInfo.OfficePost;
        //    SqlParameter paraNativeAddress = new SqlParameter("@NativeAddress", SqlDbType.VarChar, 64);
        //    paraNativeAddress.Value = info.IemBasicInfo.NativeAddress;
        //    SqlParameter paraNativeTEL = new SqlParameter("@NativeTEL", SqlDbType.VarChar, 16);
        //    paraNativeTEL.Value = info.IemBasicInfo.NativeTEL;
        //    SqlParameter paraNativePost = new SqlParameter("@NativePost", SqlDbType.VarChar, 16);
        //    paraNativePost.Value = info.IemBasicInfo.NativePost;
        //    SqlParameter paraContactPerson = new SqlParameter("@ContactPerson", SqlDbType.VarChar, 32);
        //    paraContactPerson.Value = info.IemBasicInfo.ContactPerson;
        //    SqlParameter paraRelationship = new SqlParameter("@Relationship", SqlDbType.VarChar, 4);
        //    paraRelationship.Value = info.IemBasicInfo.RelationshipID;
        //    SqlParameter paraContactAddress = new SqlParameter("@ContactAddress", SqlDbType.VarChar, 255);
        //    paraContactAddress.Value = info.IemBasicInfo.ContactAddress;
        //    SqlParameter paraContactTEL = new SqlParameter("@ContactTEL", SqlDbType.VarChar, 16);
        //    paraContactTEL.Value = info.IemBasicInfo.ContactTEL;
        //    SqlParameter paraAdmitDate = new SqlParameter("@AdmitDate", SqlDbType.VarChar, 19);
        //    paraAdmitDate.Value = info.IemBasicInfo.AdmitDate;
        //    SqlParameter paraAdmitDept = new SqlParameter("@AdmitDept", SqlDbType.VarChar, 12);
        //    paraAdmitDept.Value = info.IemBasicInfo.AdmitDeptID;
        //    SqlParameter paraAdmitWard = new SqlParameter("@AdmitWard", SqlDbType.VarChar, 12);
        //    paraAdmitWard.Value = info.IemBasicInfo.AdmitWardID;
        //    SqlParameter paraDays_Before = new SqlParameter("@Days_Before", SqlDbType.Decimal);
        //    paraDays_Before.Value = info.IemBasicInfo.Days_Before;
        //    SqlParameter paraTrans_Date = new SqlParameter("@Trans_Date", SqlDbType.VarChar, 19);
        //    paraTrans_Date.Value = info.IemBasicInfo.Trans_Date;
        //    SqlParameter paraTrans_AdmitDept = new SqlParameter("@Trans_AdmitDept", SqlDbType.VarChar, 12);
        //    paraTrans_AdmitDept.Value = info.IemBasicInfo.Trans_AdmitDeptID;
        //    SqlParameter paraTrans_AdmitWard = new SqlParameter("@Trans_AdmitWard", SqlDbType.VarChar, 12);
        //    paraTrans_AdmitWard.Value = info.IemBasicInfo.Trans_AdmitWard;
        //    SqlParameter paraTrans_AdmitDept_Again = new SqlParameter("@Trans_AdmitDept_Again", SqlDbType.VarChar, 12);
        //    paraTrans_AdmitDept_Again.Value = info.IemBasicInfo.Trans_AdmitDept_Again;
        //    SqlParameter paraOutWardDate = new SqlParameter("@OutWardDate", SqlDbType.VarChar, 19);
        //    paraOutWardDate.Value = info.IemBasicInfo.OutWardDate;
        //    SqlParameter paraOutHosDept = new SqlParameter("@OutHosDept", SqlDbType.VarChar, 12);
        //    paraOutHosDept.Value = info.IemBasicInfo.OutHosDeptID;
        //    SqlParameter paraOutHosWard = new SqlParameter("@OutHosWard", SqlDbType.VarChar, 12);
        //    paraOutHosWard.Value = info.IemBasicInfo.OutHosWardID;
        //    SqlParameter paraActual_Days = new SqlParameter("@Actual_Days", SqlDbType.Decimal);
        //    paraActual_Days.Value = info.IemBasicInfo.ActualDays;
        //    SqlParameter paraDeath_Time = new SqlParameter("@Death_Time", SqlDbType.VarChar, 19);
        //    paraDeath_Time.Value = info.IemBasicInfo.Death_Time;
        //    SqlParameter paraDeath_Reason = new SqlParameter("@Death_Reason", SqlDbType.VarChar, 300);
        //    paraDeath_Reason.Value = info.IemBasicInfo.Death_Reason;

        //    SqlParameter paraIs_Completed = new SqlParameter("@Is_Completed", SqlDbType.VarChar, 1);
        //    paraIs_Completed.Value = info.IemBasicInfo.Is_Completed;
        //    SqlParameter paracompleted_time = new SqlParameter("@completed_time", SqlDbType.VarChar, 19);
        //    paracompleted_time.Value = info.IemBasicInfo.completed_time;
        //    SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
        //    paraCreate_User.Value = info.IemBasicInfo.Create_User;

        //    SqlParameter paraXay_Sn = new SqlParameter("@Xay_Sn", SqlDbType.VarChar, 300);
        //    paraXay_Sn.Value = info.IemBasicInfo.Xay_Sn;
        //    SqlParameter paraCt_Sn = new SqlParameter("@Ct_Sn", SqlDbType.VarChar, 300);
        //    paraCt_Sn.Value = info.IemBasicInfo.Ct_Sn;
        //    SqlParameter paraMri_Sn = new SqlParameter("@Mri_Sn", SqlDbType.VarChar, 300);
        //    paraMri_Sn.Value = info.IemBasicInfo.Mri_Sn;
        //    SqlParameter paraDsa_Sn = new SqlParameter("@Dsa_Sn", SqlDbType.VarChar, 300);
        //    paraDsa_Sn.Value = info.IemBasicInfo.Dsa_Sn;

        //    SqlParameter paraAshes_Diagnosis_Name = new SqlParameter("@Ashes_Diagnosis_Name", SqlDbType.VarChar, 300);
        //    paraAshes_Diagnosis_Name.Value = info.IemBasicInfo.Ashes_Diagnosis_Name;

        //    SqlParameter paraAshes_Anatomise_Sn = new SqlParameter("@Ashes_Anatomise_Sn", SqlDbType.VarChar, 60);
        //    paraAshes_Anatomise_Sn.Value = info.IemBasicInfo.Ashes_Anatomise_Sn;

        //    //////诊断实体中数据
        //    SqlParameter paraAdmitInfo = new SqlParameter("@AdmitInfo", SqlDbType.VarChar, 4);
        //    paraAdmitInfo.Value = info.IemDiagInfo.AdmitInfo;
        //    SqlParameter paraIn_Check_Date = new SqlParameter("@In_Check_Date", SqlDbType.VarChar, 19);
        //    paraIn_Check_Date.Value = info.IemDiagInfo.In_Check_Date;
        //    SqlParameter paraPathology_Diagnosis_Name = new SqlParameter("@Pathology_Diagnosis_Name", SqlDbType.VarChar, 300);
        //    paraPathology_Diagnosis_Name.Value = info.IemDiagInfo.Pathology_Diagnosis_Name;
        //    SqlParameter paraPathology_Observation_Sn = new SqlParameter("@Pathology_Observation_Sn", SqlDbType.VarChar, 60);
        //    paraPathology_Observation_Sn.Value = info.IemDiagInfo.Pathology_Observation_Sn;
        //    SqlParameter paraAllergic_Drug = new SqlParameter("@Allergic_Drug", SqlDbType.VarChar, 300);
        //    paraAllergic_Drug.Value = info.IemDiagInfo.Allergic_Drug;
        //    SqlParameter paraHbsag = new SqlParameter("@Hbsag", SqlDbType.Decimal);
        //    paraHbsag.Value = info.IemDiagInfo.Hbsag;
        //    SqlParameter paraHcv_Ab = new SqlParameter("@Hcv_Ab", SqlDbType.Decimal);
        //    paraHcv_Ab.Value = info.IemDiagInfo.Hcv_Ab;
        //    SqlParameter paraHiv_Ab = new SqlParameter("@Hiv_Ab", SqlDbType.Decimal);
        //    paraHiv_Ab.Value = info.IemDiagInfo.Hiv_Ab;
        //    SqlParameter paraOpd_Ipd_Id = new SqlParameter("@Opd_Ipd_Id", SqlDbType.Decimal);
        //    paraOpd_Ipd_Id.Value = info.IemDiagInfo.Opd_Ipd_Id;
        //    SqlParameter paraIn_Out_Inpatinet_Id = new SqlParameter("@In_Out_Inpatinet_Id", SqlDbType.Decimal);
        //    paraIn_Out_Inpatinet_Id.Value = info.IemDiagInfo.In_Out_Inpatinet_Id;
        //    SqlParameter paraBefore_After_Or_Id = new SqlParameter("@Before_After_Or_Id", SqlDbType.Decimal);
        //    paraBefore_After_Or_Id.Value = info.IemDiagInfo.Before_After_Or_Id;
        //    SqlParameter paraClinical_Pathology_Id = new SqlParameter("@Clinical_Pathology_Id", SqlDbType.Decimal);
        //    paraClinical_Pathology_Id.Value = info.IemDiagInfo.Clinical_Pathology_Id;
        //    SqlParameter paraPacs_Pathology_Id = new SqlParameter("@Pacs_Pathology_Id", SqlDbType.Decimal);
        //    paraPacs_Pathology_Id.Value = info.IemDiagInfo.Pacs_Pathology_Id;
        //    SqlParameter paraSave_Times = new SqlParameter("@Save_Times", SqlDbType.Decimal);
        //    paraSave_Times.Value = info.IemDiagInfo.Save_Times;
        //    SqlParameter paraSuccess_Times = new SqlParameter("@Success_Times", SqlDbType.Decimal);
        //    paraSuccess_Times.Value = info.IemDiagInfo.Success_Times;
        //    SqlParameter paraSection_Director = new SqlParameter("@Section_Director", SqlDbType.VarChar, 20);
        //    paraSection_Director.Value = info.IemDiagInfo.Section_DirectorID;
        //    SqlParameter paraDirector = new SqlParameter("@Director", SqlDbType.VarChar, 20);
        //    paraDirector.Value = info.IemDiagInfo.DirectorID;
        //    SqlParameter paraVs_Employee_Code = new SqlParameter("@Vs_Employee_Code", SqlDbType.VarChar, 20);
        //    paraVs_Employee_Code.Value = info.IemDiagInfo.Vs_EmployeeID;
        //    SqlParameter paraResident_Employee_Code = new SqlParameter("@Resident_Employee_Code", SqlDbType.VarChar, 20);
        //    paraResident_Employee_Code.Value = info.IemDiagInfo.Resident_EmployeeID;
        //    SqlParameter paraRefresh_Employee_Code = new SqlParameter("@Refresh_Employee_Code", SqlDbType.VarChar, 20);
        //    paraRefresh_Employee_Code.Value = info.IemDiagInfo.Refresh_EmployeeID;
        //    SqlParameter paraMaster_Interne = new SqlParameter("@Master_Interne", SqlDbType.VarChar, 20);
        //    paraMaster_Interne.Value = info.IemDiagInfo.Master_InterneID;
        //    SqlParameter paraInterne = new SqlParameter("@Interne", SqlDbType.VarChar, 20);
        //    paraInterne.Value = info.IemDiagInfo.InterneID;
        //    SqlParameter paraCoding_User = new SqlParameter("@Coding_User", SqlDbType.VarChar, 20);
        //    paraCoding_User.Value = info.IemDiagInfo.Coding_UserID;
        //    SqlParameter paraMedical_Quality_Id = new SqlParameter("@Medical_Quality_Id", SqlDbType.Decimal);
        //    paraMedical_Quality_Id.Value = info.IemDiagInfo.Medical_Quality_Id;
        //    SqlParameter paraQuality_Control_Doctor = new SqlParameter("@Quality_Control_Doctor", SqlDbType.VarChar, 20);
        //    paraQuality_Control_Doctor.Value = info.IemDiagInfo.Quality_Control_DoctorID;
        //    SqlParameter paraQuality_Control_Nurse = new SqlParameter("@Quality_Control_Nurse", SqlDbType.VarChar, 20);
        //    paraQuality_Control_Nurse.Value = info.IemDiagInfo.Quality_Control_NurseID;
        //    SqlParameter paraQuality_Control_Date = new SqlParameter("@Quality_Control_Date", SqlDbType.VarChar, 19);
        //    paraQuality_Control_Date.Value = info.IemDiagInfo.Quality_Control_Date;

        //    SqlParameter paraZymosis = new SqlParameter("@Zymosis", SqlDbType.VarChar, 300);
        //    paraZymosis.Value = info.IemDiagInfo.ZymosisID;
        //    SqlParameter paraHurt_Toxicosis_Ele = new SqlParameter("@Hurt_Toxicosis_Ele", SqlDbType.VarChar, 300);
        //    paraHurt_Toxicosis_Ele.Value = info.IemDiagInfo.Hurt_Toxicosis_Element;
        //    SqlParameter paraZymosisState = new SqlParameter("@ZymosisState", SqlDbType.VarChar, 300);
        //    paraZymosisState.Value = info.IemDiagInfo.ZymosisState;


        //    //////费用实体中取数据
        //    SqlParameter paraIs_First_Case = new SqlParameter("@Is_First_Case", SqlDbType.Decimal);
        //    paraIs_First_Case.Value = info.IemFeeInfo.IsFirstCase;
        //    SqlParameter paraIs_Following = new SqlParameter("@Is_Following", SqlDbType.Decimal);
        //    paraIs_Following.Value = info.IemFeeInfo.IsFollowing;
        //    SqlParameter paraFollowing_Ending_Date = new SqlParameter("@Following_Ending_Date", SqlDbType.VarChar, 19);
        //    paraFollowing_Ending_Date.Value = info.IemFeeInfo.Following_Ending_Date;
        //    SqlParameter paraIs_Teaching_Case = new SqlParameter("@Is_Teaching_Case", SqlDbType.Decimal);
        //    paraIs_Teaching_Case.Value = info.IemFeeInfo.IsTeachingCase;
        //    SqlParameter paraBlood_Type_id = new SqlParameter("@Blood_Type_id", SqlDbType.Decimal);
        //    paraBlood_Type_id.Value = info.IemFeeInfo.BloodType;
        //    SqlParameter paraRh = new SqlParameter("@Rh", SqlDbType.Decimal);
        //    paraRh.Value = info.IemFeeInfo.Rh;
        //    SqlParameter paraBlood_Reaction_Id = new SqlParameter("@Blood_Reaction_Id", SqlDbType.Decimal);
        //    paraBlood_Reaction_Id.Value = info.IemFeeInfo.BloodReaction;
        //    SqlParameter paraBlood_Rbc = new SqlParameter("@Blood_Rbc", SqlDbType.Decimal);
        //    paraBlood_Rbc.Value = info.IemFeeInfo.Rbc;
        //    SqlParameter paraBlood_Plt = new SqlParameter("@Blood_Plt", SqlDbType.Decimal);
        //    paraBlood_Plt.Value = info.IemFeeInfo.Plt;
        //    SqlParameter paraBlood_Plasma = new SqlParameter("@Blood_Plasma", SqlDbType.Decimal);
        //    paraBlood_Plasma.Value = info.IemFeeInfo.Plasma;
        //    SqlParameter paraBlood_Wb = new SqlParameter("@Blood_Wb", SqlDbType.Decimal);
        //    paraBlood_Wb.Value = info.IemFeeInfo.Wb;
        //    SqlParameter paraBlood_Others = new SqlParameter("@Blood_Others", SqlDbType.VarChar, 60);
        //    paraBlood_Others.Value = info.IemFeeInfo.Others;

        //    SqlParameter[] paraColl = new SqlParameter[] { paraPatNoOfHis, paraNoOfInpat, paraPayID, paraSocialCare, paraInCount, paraName,
        //        paraSexID, paraBirth, paraMarital ,paraJobID,paraProvinceID,paraCountyID,paraNationID,paraNationalityID,paraIDNO,paraOrganization,paraOfficePlace,
        //    paraOfficeTEL,paraOfficePost,paraNativeAddress,paraNativeTEL,paraNativePost,paraContactPerson,paraRelationship,paraContactAddress,paraContactTEL,
        //    paraAdmitDate,paraAdmitDept,paraAdmitWard,paraDays_Before,paraTrans_Date,paraTrans_AdmitDept,paraTrans_AdmitWard,paraTrans_AdmitDept_Again,paraOutWardDate,
        //    paraOutHosDept,paraOutHosWard,paraActual_Days,paraDeath_Time,paraDeath_Reason,paraAdmitInfo,paraIn_Check_Date,paraPathology_Diagnosis_Name,paraPathology_Observation_Sn,
        //    paraAshes_Diagnosis_Name, paraAshes_Anatomise_Sn,paraAllergic_Drug,paraHbsag,paraHcv_Ab,paraHiv_Ab,paraOpd_Ipd_Id,paraIn_Out_Inpatinet_Id,paraBefore_After_Or_Id,
        //    paraClinical_Pathology_Id,paraPacs_Pathology_Id,paraSave_Times,paraSuccess_Times,paraSection_Director,paraDirector,paraVs_Employee_Code,paraResident_Employee_Code,
        //    paraRefresh_Employee_Code,paraMaster_Interne,paraInterne,paraCoding_User,paraMedical_Quality_Id,paraQuality_Control_Doctor,paraQuality_Control_Nurse,paraQuality_Control_Date,
        //    paraXay_Sn,paraCt_Sn,paraMri_Sn,paraDsa_Sn,paraIs_First_Case,paraIs_Following,paraFollowing_Ending_Date,paraIs_Teaching_Case,paraBlood_Type_id,paraRh,
        //    paraBlood_Reaction_Id,paraBlood_Rbc,paraBlood_Plt,paraBlood_Plasma,paraBlood_Wb,paraBlood_Others,paraIs_Completed,paracompleted_time,paraCreate_User,paraZymosis,
        //    paraHurt_Toxicosis_Ele,paraZymosisState};

        //    #endregion

        //    string no = sqlHelper.ExecuteDataTable("IEM_MAIN_PAGE.usp_insertiembasicinfo", paraColl, CommandType.StoredProcedure).Rows[0][0].ToString();
        //    this.IemInfo.IemBasicInfo.Iem_Mainpage_NO = no;

        //}

        ///// <summary>
        ///// insert diagnose info
        ///// </summary>
        //private void InserIemDiagnoseInfo(DataRow info, IDataAccess sqlHelper)
        //{
            
        //    //info.Create_User = m_app.User.DoctorId;
        //    //info.Iem_Mainpage_NO = this.IemInfo.IemBasicInfo.Iem_Mainpage_NO.ToString();
        //    SqlParameter paraIem_Mainpage_NO = new SqlParameter("@Iem_Mainpage_NO", SqlDbType.Decimal);
        //    paraIem_Mainpage_NO.Value = m_IemInfo.IemBasicInfo.Iem_Mainpage_NO;
        //    SqlParameter paraDiagnosis_Type_Id = new SqlParameter("@Diagnosis_Type_Id", SqlDbType.Decimal);
        //    paraDiagnosis_Type_Id.Value = info["Diagnosis_Type_Id"];
        //    SqlParameter paraDiagnosis_Code = new SqlParameter("@Diagnosis_Code", SqlDbType.VarChar, 60);
        //    paraDiagnosis_Code.Value = info["Diagnosis_Code"];
        //    SqlParameter paraDiagnosis_Name = new SqlParameter("@Diagnosis_Name", SqlDbType.VarChar, 300);
        //    paraDiagnosis_Name.Value = info["Diagnosis_Name"];
        //    SqlParameter paraStatus_Id = new SqlParameter("@Status_Id", SqlDbType.Decimal);
        //    paraStatus_Id.Value = info["Status_Id"];
        //    SqlParameter paraOrder_Value = new SqlParameter("@Order_Value", SqlDbType.Decimal);
        //    paraOrder_Value.Value = info["Order_Value"];
        //    SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
        //    paraCreate_User.Value = m_app.User.DoctorId;

        //    SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraDiagnosis_Type_Id, paraDiagnosis_Code, paraDiagnosis_Name, paraStatus_Id, paraOrder_Value, paraCreate_User };

        //    sqlHelper.ExecuteNoneQuery("IEM_MAIN_PAGE.usp_Insert_Iem_Mainpage_Diag", paraColl, CommandType.StoredProcedure);
        //}

        ///// <summary>
        ///// insert oper info
        ///// </summary>
        //private void InserIemOperInfo(DataRow info, IDataAccess sqlHelper)
        //{
        //    //info.Create_User = ;
        //    //info.IEM_MainPage_NO = ;
        //    SqlParameter paraIem_Mainpage_NO = new SqlParameter("@IEM_MainPage_NO", SqlDbType.Decimal);
        //    paraIem_Mainpage_NO.Value = this.IemInfo.IemBasicInfo.Iem_Mainpage_NO;
        //    SqlParameter paraOperation_Code = new SqlParameter("@Operation_Code", SqlDbType.VarChar, 60);
        //    paraOperation_Code.Value = info["Operation_Code"];
        //    SqlParameter paraOperation_Date = new SqlParameter("@Operation_Date", SqlDbType.VarChar, 19);
        //    paraOperation_Date.Value = info["Operation_Date"];
        //    SqlParameter paraOperation_Name = new SqlParameter("@Operation_Name", SqlDbType.VarChar, 300);
        //    paraOperation_Name.Value = info["Operation_Name"];
        //    SqlParameter paraExecute_User1 = new SqlParameter("@Execute_User1", SqlDbType.VarChar, 20);
        //    paraExecute_User1.Value = info["Execute_User1"];
        //    SqlParameter paraExecute_User2 = new SqlParameter("@Execute_User2", SqlDbType.VarChar, 20);
        //    paraExecute_User2.Value = info["Execute_User2"];
        //    SqlParameter paraExecute_User3 = new SqlParameter("@Execute_User3", SqlDbType.VarChar, 20);
        //    paraExecute_User3.Value = info["Execute_User3"];
        //    SqlParameter paraAnaesthesia_Type_Id = new SqlParameter("@Anaesthesia_Type_Id", SqlDbType.Decimal);
        //    paraAnaesthesia_Type_Id.Value = info["Anaesthesia_Type_Id"];
        //    SqlParameter paraClose_Level = new SqlParameter("@Close_Level", SqlDbType.Decimal);
        //    paraClose_Level.Value = info["Close_Level"];
        //    SqlParameter paraAnaesthesia_User = new SqlParameter("@Anaesthesia_User", SqlDbType.VarChar, 20);
        //    paraAnaesthesia_User.Value = info["Anaesthesia_User"];
        //    SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
        //    paraCreate_User.Value = m_app.User.DoctorId;

        //    SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraOperation_Code, paraOperation_Date, paraOperation_Name, paraExecute_User1, paraExecute_User2, paraExecute_User3,
        //        paraAnaesthesia_Type_Id,paraClose_Level,paraAnaesthesia_User,paraCreate_User };

        //    sqlHelper.ExecuteNoneQuery("IEM_MAIN_PAGE.usp_Insert_Iem_MainPage_Oper", paraColl, CommandType.StoredProcedure);

        //}

        ///// <summary>
        ///// 保存孕妇情况
        ///// </summary>
        ///// <param name="baby"></param>
        ///// <param name="sqlHelper"></param>
        //private void InsertIemObstetricsBaby(Iem_MainPage_ObstetricsBaby baby, IDataAccess sqlHelper)
        //{
        //    baby.Create_User = m_app.User.DoctorId;
        //    baby.IEM_MainPage_NO = this.IemInfo.IemBasicInfo.Iem_Mainpage_NO.ToString();
        //    SqlParameter paraIem_Mainpage_NO = new SqlParameter("@iem_mainpage_no", SqlDbType.Decimal);
        //    paraIem_Mainpage_NO.Value = baby.IEM_MainPage_NO;
        //    SqlParameter paraTC = new SqlParameter("@TC", SqlDbType.VarChar, 1);
        //    paraTC.Value = baby.TC;
        //    SqlParameter paraCC = new SqlParameter("@CC", SqlDbType.VarChar, 1);
        //    paraCC.Value = baby.CC;
        //    SqlParameter paraTB = new SqlParameter("@TB", SqlDbType.VarChar, 1);
        //    paraTB.Value = baby.TB;
        //    SqlParameter paraCFHYPLD = new SqlParameter("@CFHYPLD", SqlDbType.VarChar, 1);
        //    paraCFHYPLD.Value = baby.CFHYPLD;
        //    SqlParameter paraMIDWIFERY = new SqlParameter("@MIDWIFERY", SqlDbType.VarChar, 20);
        //    paraMIDWIFERY.Value = baby.Midwifery;
        //    SqlParameter paraSex = new SqlParameter("@Sex", SqlDbType.VarChar, 1);
        //    paraSex.Value = baby.Sex;
        //    SqlParameter paraAPJ = new SqlParameter("@APJ", SqlDbType.VarChar, 10);
        //    paraAPJ.Value = baby.APJ;
        //    SqlParameter paraHeigh = new SqlParameter("@Heigh", SqlDbType.VarChar, 10);
        //    paraHeigh.Value = baby.Heigh;
        //    SqlParameter paraWeight = new SqlParameter("@Weight", SqlDbType.VarChar, 10);
        //    paraWeight.Value = baby.Weight;
        //    SqlParameter paraCCQK = new SqlParameter("@CCQK", SqlDbType.VarChar, 1);
        //    paraCCQK.Value = baby.CCQK;
        //    SqlParameter paraBITHDAY = new SqlParameter("@BITHDAY", SqlDbType.VarChar, 1);
        //    paraBITHDAY.Value = baby.BithDay;
        //    SqlParameter paraFMFS = new SqlParameter("@FMFS", SqlDbType.VarChar, 1);
        //    paraFMFS.Value = baby.FMFS;
        //    SqlParameter paraCYQK = new SqlParameter("@CYQK", SqlDbType.VarChar, 1);
        //    paraCYQK.Value = baby.CYQK;
        //    SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
        //    paraCreate_User.Value = baby.Create_User;

        //    SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO,paraTC, paraCC,paraTB,paraCFHYPLD,paraMIDWIFERY, paraSex,paraAPJ,
        //                paraHeigh,paraWeight,paraCCQK,paraBITHDAY,paraFMFS,paraCYQK,paraCreate_User };

        //    sqlHelper.ExecuteNoneQuery("IEM_MAIN_PAGE.usp_insert_iem_main_ObsBaby", paraColl, CommandType.StoredProcedure);

        //}



        //#endregion

        //#region update
        ///// <summary>
        ///// 更新首页信息
        ///// </summary>
        ///// <param name="info"></param>
        //private void UpdateItemInfo()
        //{
        //    try
        //    {
        //        m_app.SqlHelper.BeginTransaction();

        //        UpdateIemBasicInfo(this.IemInfo.IemBasicInfo, m_app.SqlHelper);

        //        // 先把之前的诊断，都给CANCLE
        //        UpdateIemDiagnoseInfo(this.IemInfo, m_app.SqlHelper);
        //        foreach (DataRow item in this.IemInfo.IemDiagInfo.OutDiagTable.Rows)
        //            InserIemDiagnoseInfo(item, m_app.SqlHelper);

        //        // 先把之前的手术，都给CANCLE
        //        UpdateIemOperInfo(this.IemInfo, m_app.SqlHelper);
        //        foreach (DataRow item in this.IemInfo.IemOperInfo.Operation_Table.Rows)
        //            InserIemOperInfo(item, m_app.SqlHelper);


        //        //修改产妇婴儿系想你
        //        DeleteIemObstetricsBaby(IemInfo.IemObstetricsBaby, m_app.SqlHelper);
        //        InsertIemObstetricsBaby(IemInfo.IemObstetricsBaby, m_app.SqlHelper);

        //        m_app.SqlHelper.CommitTransaction();

        //        m_app.CustomMessageBox.MessageShow("更新成功");
        //        GetIemInfo();
        //        FillUI();
        //    }
        //    catch (Exception ex)
        //    {
        //        m_app.SqlHelper.RollbackTransaction();
        //    }
        //}

        //private void UpdateIemBasicInfo(IemMainPageInfo info, IDataAccess sqlHelper)
        //{
        //    info.IemBasicInfo.Create_User = m_app.User.DoctorId;

        //    #region
        //    SqlParameter paraPatNoOfHis = new SqlParameter("@PatNoOfHis", SqlDbType.VarChar, 14);
        //    paraPatNoOfHis.Value = info.IemBasicInfo.PatNoOfHis;
        //    SqlParameter paraNoOfInpat = new SqlParameter("@NoOfInpat", SqlDbType.Decimal);
        //    paraNoOfInpat.Value = info.IemBasicInfo.NoOfInpat;
        //    SqlParameter paraPayID = new SqlParameter("@PayID", SqlDbType.VarChar, 4);
        //    paraPayID.Value = info.IemBasicInfo.PayID;
        //    SqlParameter paraSocialCare = new SqlParameter("@SocialCare", SqlDbType.VarChar, 32);
        //    paraSocialCare.Value = info.IemBasicInfo.SocialCare;
        //    SqlParameter paraInCount = new SqlParameter("@InCount", SqlDbType.Int);
        //    paraInCount.Value = info.IemBasicInfo.InCount;
        //    SqlParameter paraName = new SqlParameter("@Name", SqlDbType.VarChar, 64);
        //    paraName.Value = info.IemBasicInfo.Name;
        //    SqlParameter paraSexID = new SqlParameter("@SexID", SqlDbType.VarChar, 4);
        //    paraSexID.Value = info.IemBasicInfo.SexID;
        //    SqlParameter paraBirth = new SqlParameter("@Birth", SqlDbType.VarChar, 10);
        //    paraBirth.Value = info.IemBasicInfo.Birth;
        //    SqlParameter paraMarital = new SqlParameter("@Marital", SqlDbType.VarChar, 4);
        //    paraMarital.Value = info.IemBasicInfo.Marital;
        //    SqlParameter paraJobID = new SqlParameter("@JobID", SqlDbType.VarChar, 4);
        //    paraJobID.Value = info.IemBasicInfo.JobID;
        //    SqlParameter paraProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 10);
        //    paraProvinceID.Value = info.IemBasicInfo.ProvinceID;
        //    SqlParameter paraCountyID = new SqlParameter("@CountyID", SqlDbType.VarChar, 10);
        //    paraCountyID.Value = info.IemBasicInfo.CountyID;
        //    SqlParameter paraNationID = new SqlParameter("@NationID", SqlDbType.VarChar, 4);
        //    paraNationID.Value = info.IemBasicInfo.NationID;
        //    SqlParameter paraNationalityID = new SqlParameter("@NationalityID", SqlDbType.VarChar, 4);
        //    paraNationalityID.Value = info.IemBasicInfo.NationalityID;
        //    SqlParameter paraIDNO = new SqlParameter("@IDNO", SqlDbType.VarChar, 18);
        //    paraIDNO.Value = info.IemBasicInfo.IDNO;
        //    SqlParameter paraOrganization = new SqlParameter("@Organization", SqlDbType.VarChar, 64);
        //    paraOrganization.Value = info.IemBasicInfo.Organization;
        //    SqlParameter paraOfficePlace = new SqlParameter("@OfficePlace", SqlDbType.VarChar, 64);
        //    paraOfficePlace.Value = info.IemBasicInfo.OfficePlace;
        //    SqlParameter paraOfficeTEL = new SqlParameter("@OfficeTEL", SqlDbType.VarChar, 16);
        //    paraOfficeTEL.Value = info.IemBasicInfo.OfficeTEL;
        //    SqlParameter paraOfficePost = new SqlParameter("@OfficePost", SqlDbType.VarChar, 16);
        //    paraOfficePost.Value = info.IemBasicInfo.OfficePost;
        //    SqlParameter paraNativeAddress = new SqlParameter("@NativeAddress", SqlDbType.VarChar, 64);
        //    paraNativeAddress.Value = info.IemBasicInfo.NativeAddress;
        //    SqlParameter paraNativeTEL = new SqlParameter("@NativeTEL", SqlDbType.VarChar, 16);
        //    paraNativeTEL.Value = info.IemBasicInfo.NativeTEL;
        //    SqlParameter paraNativePost = new SqlParameter("@NativePost", SqlDbType.VarChar, 16);
        //    paraNativePost.Value = info.IemBasicInfo.NativePost;
        //    SqlParameter paraContactPerson = new SqlParameter("@ContactPerson", SqlDbType.VarChar, 32);
        //    paraContactPerson.Value = info.IemBasicInfo.ContactPerson;
        //    SqlParameter paraRelationship = new SqlParameter("@Relationship", SqlDbType.VarChar, 4);
        //    paraRelationship.Value = info.IemBasicInfo.RelationshipID;
        //    SqlParameter paraContactAddress = new SqlParameter("@ContactAddress", SqlDbType.VarChar, 255);
        //    paraContactAddress.Value = info.IemBasicInfo.ContactAddress;
        //    SqlParameter paraContactTEL = new SqlParameter("@ContactTEL", SqlDbType.VarChar, 16);
        //    paraContactTEL.Value = info.IemBasicInfo.ContactTEL;
        //    SqlParameter paraAdmitDate = new SqlParameter("@AdmitDate", SqlDbType.VarChar, 19);
        //    paraAdmitDate.Value = info.IemBasicInfo.AdmitDate;
        //    SqlParameter paraAdmitDept = new SqlParameter("@AdmitDept", SqlDbType.VarChar, 12);
        //    paraAdmitDept.Value = info.IemBasicInfo.AdmitDeptID;
        //    SqlParameter paraAdmitWard = new SqlParameter("@AdmitWard", SqlDbType.VarChar, 12);
        //    paraAdmitWard.Value = info.IemBasicInfo.AdmitWardID;
        //    SqlParameter paraDays_Before = new SqlParameter("@Days_Before", SqlDbType.Decimal);
        //    paraDays_Before.Value = info.IemBasicInfo.Days_Before;
        //    SqlParameter paraTrans_Date = new SqlParameter("@Trans_Date", SqlDbType.VarChar, 19);
        //    paraTrans_Date.Value = info.IemBasicInfo.Trans_Date;
        //    SqlParameter paraTrans_AdmitDept = new SqlParameter("@Trans_AdmitDept", SqlDbType.VarChar, 12);
        //    paraTrans_AdmitDept.Value = info.IemBasicInfo.Trans_AdmitDeptID;
        //    SqlParameter paraTrans_AdmitWard = new SqlParameter("@Trans_AdmitWard", SqlDbType.VarChar, 12);
        //    paraTrans_AdmitWard.Value = info.IemBasicInfo.Trans_AdmitWard;
        //    SqlParameter paraTrans_AdmitDept_Again = new SqlParameter("@Trans_AdmitDept_Again", SqlDbType.VarChar, 12);
        //    paraTrans_AdmitDept_Again.Value = info.IemBasicInfo.Trans_AdmitDept_Again;
        //    SqlParameter paraOutWardDate = new SqlParameter("@OutWardDate", SqlDbType.VarChar, 19);
        //    paraOutWardDate.Value = info.IemBasicInfo.OutWardDate;
        //    SqlParameter paraOutHosDept = new SqlParameter("@OutHosDept", SqlDbType.VarChar, 12);
        //    paraOutHosDept.Value = info.IemBasicInfo.OutHosDeptID;
        //    SqlParameter paraOutHosWard = new SqlParameter("@OutHosWard", SqlDbType.VarChar, 12);
        //    paraOutHosWard.Value = info.IemBasicInfo.OutHosWardID;
        //    SqlParameter paraActual_Days = new SqlParameter("@Actual_Days", SqlDbType.Decimal);
        //    paraActual_Days.Value = info.IemBasicInfo.ActualDays;
        //    SqlParameter paraDeath_Time = new SqlParameter("@Death_Time", SqlDbType.VarChar, 19);
        //    paraDeath_Time.Value = info.IemBasicInfo.Death_Time;
        //    SqlParameter paraDeath_Reason = new SqlParameter("@Death_Reason", SqlDbType.VarChar, 300);
        //    paraDeath_Reason.Value = info.IemBasicInfo.Death_Reason;

        //    SqlParameter paraIs_Completed = new SqlParameter("@Is_Completed", SqlDbType.VarChar, 1);
        //    paraIs_Completed.Value = info.IemBasicInfo.Is_Completed;
        //    SqlParameter paracompleted_time = new SqlParameter("@completed_time", SqlDbType.VarChar, 19);
        //    paracompleted_time.Value = info.IemBasicInfo.completed_time;
 

        //    SqlParameter paraXay_Sn = new SqlParameter("@Xay_Sn", SqlDbType.VarChar, 300);
        //    paraXay_Sn.Value = info.IemBasicInfo.Xay_Sn;
        //    SqlParameter paraCt_Sn = new SqlParameter("@Ct_Sn", SqlDbType.VarChar, 300);
        //    paraCt_Sn.Value = info.IemBasicInfo.Ct_Sn;
        //    SqlParameter paraMri_Sn = new SqlParameter("@Mri_Sn", SqlDbType.VarChar, 300);
        //    paraMri_Sn.Value = info.IemBasicInfo.Mri_Sn;
        //    SqlParameter paraDsa_Sn = new SqlParameter("@Dsa_Sn", SqlDbType.VarChar, 300);
        //    paraDsa_Sn.Value = info.IemBasicInfo.Dsa_Sn;

        //    SqlParameter paraAshes_Diagnosis_Name = new SqlParameter("@Ashes_Diagnosis_Name", SqlDbType.VarChar, 300);
        //    paraAshes_Diagnosis_Name.Value = info.IemBasicInfo.Ashes_Diagnosis_Name;

        //    SqlParameter paraAshes_Anatomise_Sn = new SqlParameter("@Ashes_Anatomise_Sn", SqlDbType.VarChar, 60);
        //    paraAshes_Anatomise_Sn.Value = info.IemBasicInfo.Ashes_Anatomise_Sn;

        //    //////诊断实体中数据
        //    SqlParameter paraAdmitInfo = new SqlParameter("@AdmitInfo", SqlDbType.VarChar, 4);
        //    paraAdmitInfo.Value = info.IemDiagInfo.AdmitInfo;
        //    SqlParameter paraIn_Check_Date = new SqlParameter("@In_Check_Date", SqlDbType.VarChar, 19);
        //    paraIn_Check_Date.Value = info.IemDiagInfo.In_Check_Date;
        //    SqlParameter paraPathology_Diagnosis_Name = new SqlParameter("@Pathology_Diagnosis_Name", SqlDbType.VarChar, 300);
        //    paraPathology_Diagnosis_Name.Value = info.IemDiagInfo.Pathology_Diagnosis_Name;
        //    SqlParameter paraPathology_Observation_Sn = new SqlParameter("@Pathology_Observation_Sn", SqlDbType.VarChar, 60);
        //    paraPathology_Observation_Sn.Value = info.IemDiagInfo.Pathology_Observation_Sn;
        //    SqlParameter paraAllergic_Drug = new SqlParameter("@Allergic_Drug", SqlDbType.VarChar, 300);
        //    paraAllergic_Drug.Value = info.IemDiagInfo.Allergic_Drug;
        //    SqlParameter paraHbsag = new SqlParameter("@Hbsag", SqlDbType.Decimal);
        //    paraHbsag.Value = info.IemDiagInfo.Hbsag;
        //    SqlParameter paraHcv_Ab = new SqlParameter("@Hcv_Ab", SqlDbType.Decimal);
        //    paraHcv_Ab.Value = info.IemDiagInfo.Hcv_Ab;
        //    SqlParameter paraHiv_Ab = new SqlParameter("@Hiv_Ab", SqlDbType.Decimal);
        //    paraHiv_Ab.Value = info.IemDiagInfo.Hiv_Ab;
        //    SqlParameter paraOpd_Ipd_Id = new SqlParameter("@Opd_Ipd_Id", SqlDbType.Decimal);
        //    paraOpd_Ipd_Id.Value = info.IemDiagInfo.Opd_Ipd_Id;
        //    SqlParameter paraIn_Out_Inpatinet_Id = new SqlParameter("@In_Out_Inpatinet_Id", SqlDbType.Decimal);
        //    paraIn_Out_Inpatinet_Id.Value = info.IemDiagInfo.In_Out_Inpatinet_Id;
        //    SqlParameter paraBefore_After_Or_Id = new SqlParameter("@Before_After_Or_Id", SqlDbType.Decimal);
        //    paraBefore_After_Or_Id.Value = info.IemDiagInfo.Before_After_Or_Id;
        //    SqlParameter paraClinical_Pathology_Id = new SqlParameter("@Clinical_Pathology_Id", SqlDbType.Decimal);
        //    paraClinical_Pathology_Id.Value = info.IemDiagInfo.Clinical_Pathology_Id;
        //    SqlParameter paraPacs_Pathology_Id = new SqlParameter("@Pacs_Pathology_Id", SqlDbType.Decimal);
        //    paraPacs_Pathology_Id.Value = info.IemDiagInfo.Pacs_Pathology_Id;
        //    SqlParameter paraSave_Times = new SqlParameter("@Save_Times", SqlDbType.Decimal);
        //    paraSave_Times.Value = info.IemDiagInfo.Save_Times;
        //    SqlParameter paraSuccess_Times = new SqlParameter("@Success_Times", SqlDbType.Decimal);
        //    paraSuccess_Times.Value = info.IemDiagInfo.Success_Times;
        //    SqlParameter paraSection_Director = new SqlParameter("@Section_Director", SqlDbType.VarChar, 20);
        //    paraSection_Director.Value = info.IemDiagInfo.Section_DirectorID;
        //    SqlParameter paraDirector = new SqlParameter("@Director", SqlDbType.VarChar, 20);
        //    paraDirector.Value = info.IemDiagInfo.DirectorID;
        //    SqlParameter paraVs_Employee_Code = new SqlParameter("@Vs_Employee_Code", SqlDbType.VarChar, 20);
        //    paraVs_Employee_Code.Value = info.IemDiagInfo.Vs_EmployeeID;
        //    SqlParameter paraResident_Employee_Code = new SqlParameter("@Resident_Employee_Code", SqlDbType.VarChar, 20);
        //    paraResident_Employee_Code.Value = info.IemDiagInfo.Resident_EmployeeID;
        //    SqlParameter paraRefresh_Employee_Code = new SqlParameter("@Refresh_Employee_Code", SqlDbType.VarChar, 20);
        //    paraRefresh_Employee_Code.Value = info.IemDiagInfo.Refresh_EmployeeID;
        //    SqlParameter paraMaster_Interne = new SqlParameter("@Master_Interne", SqlDbType.VarChar, 20);
        //    paraMaster_Interne.Value = info.IemDiagInfo.Master_InterneID;
        //    SqlParameter paraInterne = new SqlParameter("@Interne", SqlDbType.VarChar, 20);
        //    paraInterne.Value = info.IemDiagInfo.InterneID;
        //    SqlParameter paraCoding_User = new SqlParameter("@Coding_User", SqlDbType.VarChar, 20);
        //    paraCoding_User.Value = info.IemDiagInfo.Coding_UserID;
        //    SqlParameter paraMedical_Quality_Id = new SqlParameter("@Medical_Quality_Id", SqlDbType.Decimal);
        //    paraMedical_Quality_Id.Value = info.IemDiagInfo.Medical_Quality_Id;
        //    SqlParameter paraQuality_Control_Doctor = new SqlParameter("@Quality_Control_Doctor", SqlDbType.VarChar, 20);
        //    paraQuality_Control_Doctor.Value = info.IemDiagInfo.Quality_Control_DoctorID;
        //    SqlParameter paraQuality_Control_Nurse = new SqlParameter("@Quality_Control_Nurse", SqlDbType.VarChar, 20);
        //    paraQuality_Control_Nurse.Value = info.IemDiagInfo.Quality_Control_NurseID;
        //    SqlParameter paraQuality_Control_Date = new SqlParameter("@Quality_Control_Date", SqlDbType.VarChar, 19);
        //    paraQuality_Control_Date.Value = info.IemDiagInfo.Quality_Control_Date;

        //    SqlParameter paraZymosis = new SqlParameter("@Zymosis", SqlDbType.VarChar, 300);
        //    paraZymosis.Value = info.IemDiagInfo.ZymosisID;
        //    SqlParameter paraHurt_Toxicosis_Ele = new SqlParameter("@Hurt_Toxicosis_Ele", SqlDbType.VarChar, 300);
        //    paraHurt_Toxicosis_Ele.Value = info.IemDiagInfo.Hurt_Toxicosis_Element;
        //    SqlParameter paraZymosisState = new SqlParameter("@ZymosisState", SqlDbType.VarChar, 300);
        //    paraZymosisState.Value = info.IemDiagInfo.ZymosisState;


        //    //////费用实体中取数据
        //    SqlParameter paraIs_First_Case = new SqlParameter("@Is_First_Case", SqlDbType.Decimal);
        //    paraIs_First_Case.Value = info.IemFeeInfo.IsFirstCase;
        //    SqlParameter paraIs_Following = new SqlParameter("@Is_Following", SqlDbType.Decimal);
        //    paraIs_Following.Value = info.IemFeeInfo.IsFollowing;
        //    SqlParameter paraFollowing_Ending_Date = new SqlParameter("@Following_Ending_Date", SqlDbType.VarChar, 19);
        //    paraFollowing_Ending_Date.Value = info.IemFeeInfo.Following_Ending_Date;
        //    SqlParameter paraIs_Teaching_Case = new SqlParameter("@Is_Teaching_Case", SqlDbType.Decimal);
        //    paraIs_Teaching_Case.Value = info.IemFeeInfo.IsTeachingCase;
        //    SqlParameter paraBlood_Type_id = new SqlParameter("@Blood_Type_id", SqlDbType.Decimal);
        //    paraBlood_Type_id.Value = info.IemFeeInfo.BloodType;
        //    SqlParameter paraRh = new SqlParameter("@Rh", SqlDbType.Decimal);
        //    paraRh.Value = info.IemFeeInfo.Rh;
        //    SqlParameter paraBlood_Reaction_Id = new SqlParameter("@Blood_Reaction_Id", SqlDbType.Decimal);
        //    paraBlood_Reaction_Id.Value = info.IemFeeInfo.BloodReaction;
        //    SqlParameter paraBlood_Rbc = new SqlParameter("@Blood_Rbc", SqlDbType.Decimal);
        //    paraBlood_Rbc.Value = info.IemFeeInfo.Rbc;
        //    SqlParameter paraBlood_Plt = new SqlParameter("@Blood_Plt", SqlDbType.Decimal);
        //    paraBlood_Plt.Value = info.IemFeeInfo.Plt;
        //    SqlParameter paraBlood_Plasma = new SqlParameter("@Blood_Plasma", SqlDbType.Decimal);
        //    paraBlood_Plasma.Value = info.IemFeeInfo.Plasma;
        //    SqlParameter paraBlood_Wb = new SqlParameter("@Blood_Wb", SqlDbType.Decimal);
        //    paraBlood_Wb.Value = info.IemFeeInfo.Wb;
        //    SqlParameter paraBlood_Others = new SqlParameter("@Blood_Others", SqlDbType.VarChar, 60);
        //    paraBlood_Others.Value = info.IemFeeInfo.Others;

        //    SqlParameter[] paraColl = new SqlParameter[] { paraPatNoOfHis, paraNoOfInpat, paraPayID, paraSocialCare, paraInCount, paraName,
        //        paraSexID, paraBirth, paraMarital ,paraJobID,paraProvinceID,paraCountyID,paraNationID,paraNationalityID,paraIDNO,paraOrganization,paraOfficePlace,
        //    paraOfficeTEL,paraOfficePost,paraNativeAddress,paraNativeTEL,paraNativePost,paraContactPerson,paraRelationship,paraContactAddress,paraContactTEL,
        //    paraAdmitDate,paraAdmitDept,paraAdmitWard,paraDays_Before,paraTrans_Date,paraTrans_AdmitDept,paraTrans_AdmitWard,paraTrans_AdmitDept_Again,paraOutWardDate,
        //    paraOutHosDept,paraOutHosWard,paraActual_Days,paraDeath_Time,paraDeath_Reason,paraAdmitInfo,paraIn_Check_Date,paraPathology_Diagnosis_Name,paraPathology_Observation_Sn,
        //    paraAshes_Diagnosis_Name, paraAshes_Anatomise_Sn,paraAllergic_Drug,paraHbsag,paraHcv_Ab,paraHiv_Ab,paraOpd_Ipd_Id,paraIn_Out_Inpatinet_Id,paraBefore_After_Or_Id,
        //    paraClinical_Pathology_Id,paraPacs_Pathology_Id,paraSave_Times,paraSuccess_Times,paraSection_Director,paraDirector,paraVs_Employee_Code,paraResident_Employee_Code,
        //    paraRefresh_Employee_Code,paraMaster_Interne,paraInterne,paraCoding_User,paraMedical_Quality_Id,paraQuality_Control_Doctor,paraQuality_Control_Nurse,paraQuality_Control_Date,
        //    paraXay_Sn,paraCt_Sn,paraMri_Sn,paraDsa_Sn,paraIs_First_Case,paraIs_Following,paraFollowing_Ending_Date,paraIs_Teaching_Case,paraBlood_Type_id,paraRh,
        //    paraBlood_Reaction_Id,paraBlood_Rbc,paraBlood_Plt,paraBlood_Plasma,paraBlood_Wb,paraBlood_Others,paraIs_Completed,paracompleted_time,paraCreate_User,paraZymosis,
        //    paraHurt_Toxicosis_Ele,paraZymosisState};

        //    #endregion

        //    string no = sqlHelper.ExecuteDataTable("IEM_MAIN_PAGE.usp_Updateiembasicinfo", paraColl, CommandType.StoredProcedure).Rows[0][0].ToString();
        //    this.IemInfo.IemBasicInfo.Iem_Mainpage_NO = no;
        //}

        ///// <summary>
        ///// 取消之前的诊断信息
        ///// </summary>
        //private void UpdateIemDiagnoseInfo(IemMainPageInfo info, IDataAccess sqlHelper)
        //{
        //    info.IemBasicInfo.Create_User = m_app.User.DoctorId;
        //    SqlParameter paraIem_Mainpage_NO = new SqlParameter("@Iem_Mainpage_NO", SqlDbType.Decimal);
        //    paraIem_Mainpage_NO.Value = info.IemBasicInfo.Iem_Mainpage_NO;
        //    SqlParameter paraCreate_User = new SqlParameter("@Cancel_User", SqlDbType.VarChar, 10);
        //    paraCreate_User.Value = info.IemBasicInfo.Create_User;

        //    SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraCreate_User };

        //    sqlHelper.ExecuteNoneQuery("usp_Update_Iem_Mainpage_Diag", paraColl, CommandType.StoredProcedure);
        //}

        ///// <summary>
        ///// 取消之前的手术信息
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="sqlHelper"></param>
        //private void UpdateIemOperInfo(IemMainPageInfo info, IDataAccess sqlHelper)
        //{
        //    info.IemBasicInfo.Create_User = m_app.User.DoctorId;
        //    SqlParameter paraIem_Mainpage_NO = new SqlParameter("@IEM_MainPage_NO", SqlDbType.Decimal);
        //    paraIem_Mainpage_NO.Value = info.IemBasicInfo.Iem_Mainpage_NO;
        //    SqlParameter paraCreate_User = new SqlParameter("@Cancel_User", SqlDbType.VarChar, 10);
        //    paraCreate_User.Value = info.IemBasicInfo.Create_User;

        //    SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraCreate_User };

        //    sqlHelper.ExecuteNoneQuery("usp_Update_Iem_MainPage_Oper", paraColl, CommandType.StoredProcedure);

        //}

        ///// <summary>
        ///// 取消之前的产妇婴儿信息
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="sqlHelper"></param>
        //private void DeleteIemObstetricsBaby(Iem_MainPage_ObstetricsBaby info, IDataAccess sqlHelper)
        //{
        //    string sql = string.Format("delete IEM_MAINPAGE_OBSTETRICSBABY where IEM_MAINPAGE_NO = '{0}'", m_IemInfo.IemBasicInfo.Iem_Mainpage_NO);

        //    sqlHelper.ExecuteNoneQuery(sql, CommandType.Text);

        //}


        //private String InitUpdateSql(SqlParameter[] paraColl)
        //{
        //    foreach (SqlParameter para in paraColl)
        //    {
        //        if (para.SqlDbType != SqlDbType.VarChar)
        //        {
        //            if (para.Value == null)
        //                para.Value = System.Data.SqlTypes.SqlString.Null;
        //        }
        //        else
        //        {
        //            if (para.Value == null)
        //                para.Value = String.Empty;
        //        }
        //    }
        //    StringBuilder str = new StringBuilder();
        //    str.Append("update  Iem_Mainpage_Basicinfo");
        //    str.Append(" set     PatNoOfHis = '" + paraColl[0].Value + "' ,");
        //    str.Append(" NoOfInpat =" + paraColl[1].Value + ",");
        //    str.Append(" PayID =  '" + paraColl[2].Value + "',");
        //    str.Append(" SocialCare ='" + paraColl[3].Value + "',");
        //    str.Append(" InCount =  " + paraColl[4].Value + ",");
        //    str.Append(" Name ='" + paraColl[5].Value + "',");
        //    str.Append(" SexID = '" + paraColl[6].Value + "',");
        //    str.Append(" Birth ='" + paraColl[7].Value + "',");
        //    str.Append(" Marital ='" + paraColl[8].Value + "',");
        //    str.Append(" JobID = '" + paraColl[9].Value + "',");
        //    str.Append(" ProvinceID ='" + paraColl[10].Value + "',");
        //    str.Append(" CountyID ='" + paraColl[11].Value + "',");
        //    str.Append(" NationID ='" + paraColl[12].Value + "' ,");
        //    str.Append(" NationalityID = '" + paraColl[13].Value + "',");
        //    str.Append(" IDNO = '" + paraColl[14].Value + "',");
        //    str.Append(" Organization ='" + paraColl[15].Value + "' ,");
        //    str.Append(" OfficePlace = '" + paraColl[16].Value + "',");
        //    str.Append(" OfficeTEL =  '" + paraColl[17].Value + "',");
        //    str.Append(" OfficePost = '" + paraColl[18].Value + "',");
        //    str.Append(" NativeAddress = '" + paraColl[19].Value + "',");
        //    str.Append(" NativeTEL = '" + paraColl[20].Value + "',");
        //    str.Append(" NativePost = '" + paraColl[21].Value + "',");
        //    str.Append(" ContactPerson = '" + paraColl[22].Value + "',");
        //    str.Append(" Relationship = '" + paraColl[23].Value + "',");
        //    str.Append(" ContactAddress = '" + paraColl[24].Value + "',");
        //    str.Append(" ContactTEL =  '" + paraColl[25].Value + "',");
        //    str.Append(" AdmitDate =  '" + paraColl[26].Value + "',");
        //    str.Append(" AdmitDept =  '" + paraColl[27].Value + "',");
        //    str.Append(" AdmitWard = '" + paraColl[28].Value + "',");
        //    str.Append(" Days_Before = '" + paraColl[29].Value + "',");
        //    str.Append(" Trans_Date = '" + paraColl[30].Value + "',");
        //    str.Append(" Trans_AdmitDept = '" + paraColl[31].Value + "',");
        //    str.Append(" Trans_AdmitWard = '" + paraColl[32].Value + "',");
        //    str.Append(" Trans_AdmitDept_Again = '" + paraColl[33].Value + "',");
        //    str.Append(" OutWardDate = '" + paraColl[34].Value + "', ");
        //    str.Append(" OutHosDept = '" + paraColl[35].Value + "',");
        //    str.Append(" OutHosWard = '" + paraColl[36].Value + "',");
        //    str.Append(" Actual_Days = " + paraColl[37].Value + ",");
        //    str.Append(" Death_Time = '" + paraColl[38].Value + "',");
        //    str.Append(" Death_Reason = '" + paraColl[39].Value + "',");
        //    str.Append(" AdmitInfo = '" + paraColl[40].Value + "',");
        //    str.Append(" In_Check_Date = '" + paraColl[41].Value + "',");
        //    str.Append(" Pathology_Diagnosis_Name = '" + paraColl[42].Value + "',");
        //    str.Append(" Pathology_Observation_Sn = '" + paraColl[43].Value + "' ,");
        //    str.Append(" Ashes_Diagnosis_Name = '" + paraColl[44].Value + "',");
        //    str.Append(" Ashes_Anatomise_Sn = '" + paraColl[45].Value + "',");
        //    str.Append(" Allergic_Drug = '" + paraColl[46].Value + "',");
        //    str.Append(" Hbsag = " + paraColl[47].Value + " ,");
        //    str.Append(" Hcv_Ab = " + paraColl[48].Value + " ,");
        //    str.Append(" Hiv_Ab = " + paraColl[49].Value + " ,");
        //    str.Append(" Opd_Ipd_Id = " + paraColl[50].Value + " ,");
        //    str.Append(" In_Out_Inpatinet_Id = " + paraColl[51].Value + " ,");
        //    str.Append(" Before_After_Or_Id = " + paraColl[52].Value + " ,");
        //    str.Append(" Clinical_Pathology_Id = " + paraColl[53].Value + " ,");
        //    str.Append(" Pacs_Pathology_Id = " + paraColl[54].Value + " ,");
        //    str.Append(" Save_Times = " + paraColl[55].Value + " ,");
        //    str.Append(" Success_Times = " + paraColl[56].Value + " ,");
        //    str.Append(" Section_Director = '" + paraColl[57].Value + "',");
        //    str.Append(" Director = '" + paraColl[58].Value + "',");
        //    str.Append(" Vs_Employee_Code = '" + paraColl[59].Value + "',");
        //    str.Append(" Resident_Employee_Code = '" + paraColl[60].Value + "',");
        //    str.Append(" Refresh_Employee_Code = '" + paraColl[61].Value + "',");
        //    str.Append(" Master_Interne = '" + paraColl[62].Value + "',");
        //    str.Append(" Interne = '" + paraColl[63].Value + "',");
        //    str.Append(" Coding_User = '" + paraColl[64].Value + " ',");
        //    str.Append(" Medical_Quality_Id = " + paraColl[65].Value + " ,");
        //    str.Append(" Quality_Control_Doctor = '" + paraColl[66].Value + "' ,");
        //    str.Append(" Quality_Control_Nurse = '" + paraColl[67].Value + "' ,");
        //    str.Append(" Quality_Control_Date = '" + paraColl[68].Value + "' ,");
        //    str.Append(" Xay_Sn = '" + paraColl[69].Value + "' ,");
        //    str.Append(" Ct_Sn = '" + paraColl[70].Value + "' ,");
        //    str.Append(" Mri_Sn = '" + paraColl[71].Value + "' ,");
        //    str.Append(" Dsa_Sn = '" + paraColl[72].Value + "' ,");
        //    str.Append(" Is_First_Case =" + paraColl[73].Value + " ,");
        //    str.Append(" Is_Following = " + paraColl[74].Value + " ,");
        //    str.Append(" Following_Ending_Date = '" + paraColl[75].Value + "' ,");
        //    str.Append(" Is_Teaching_Case = " + paraColl[76].Value + " ,");
        //    str.Append(" Blood_Type_id = " + paraColl[77].Value + " ,");
        //    str.Append(" Rh = " + paraColl[78].Value + " ,");
        //    str.Append(" Blood_Reaction_Id = " + paraColl[79].Value + " ,");
        //    str.Append(" Blood_Rbc = " + paraColl[80].Value + " ,");
        //    str.Append(" Blood_Plt = " + paraColl[81].Value + " ,");
        //    str.Append(" Blood_Plasma = " + paraColl[82].Value + " ,");
        //    str.Append(" Blood_Wb = " + paraColl[83].Value + " ,");
        //    str.Append(" Blood_Others = '" + paraColl[84].Value + "' ,");
        //    str.Append(" Is_Completed = '" + paraColl[85].Value + "',");
        //    str.Append(" completed_time = '" + paraColl[86].Value + "' ,");
        //    str.Append(" Modified_User = '" + paraColl[87].Value + "' ,");
        //    str.Append(" Modified_Time = to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),");
        //    str.Append(" Zymosis = '" + paraColl[89].Value + "' ,");
        //    str.Append(" Hurt_Toxicosis_Ele = '" + paraColl[90].Value + "',");
        //    str.Append(" ZymosisState = '" + paraColl[91].Value + "'");
        //    str.Append(" where   Iem_Mainpage_NO = " + paraColl[88].Value + "");
        //    str.Append(" and Valide = 1 ;");

        //    return str.ToString();
        //}
        //#endregion
        #endregion

        #region 初始化病人信息
        /// <summary>
        /// 初始化病人信息
        /// </summary>
        /// <param name="firstPageNo"></param>
        public void InitPatientInfo()
        {
            GetIemInfo();
            FillUI();
        }
        #endregion

        #region IEMREditor 成员
        public Control DesignUI
        {
            get { return this; }
        }

        public new void Load(IYidanEmrHost app)
        {
            m_app = app;
            InitUserControl();
            if (m_app.CurrentPatientInfo == null)
                return;

            LoadForm();
            //delegateLoad deleteLoad = new delegateLoad(LoadForm);
            //deleteLoad.BeginInvoke(null, null);
        }

        public void Save()
        {
            SaveData();
        }

        public string Title
        {
            get { return "病案首页"; }
        }
        #endregion

        private void UCMainPage_SizeChanged(object sender, EventArgs e)
        {
            ReSetUCLocaton();
        }

        /// <summary>
        /// 改变UserControl的位置
        /// </summary>
        private void ReSetUCLocaton()
        {
            Int32 pointX = (this.Width - this.ucIemBasInfo.Width) / 2;
            Int32 pointY = ucIemBasInfo.Location.Y;
            this.ucIemBasInfo.Location = new Point(pointX, pointY);
            this.ucIemDiagnose.Location = new Point(pointX, pointY + ucIemBasInfo.Height);
            this.ucIemOperInfo.Location = new Point(pointX, pointY + ucIemBasInfo.Height + ucIemDiagnose.Height);
            this.ucOthers.Location = new Point(pointX, pointY + ucIemBasInfo.Height + ucIemDiagnose.Height + ucIemOperInfo.Height);

            if (ucIemBabytInfo != null)
                this.ucIemBabytInfo.Location = new Point(pointX, pointY + ucIemBasInfo.Height + ucIemDiagnose.Height + ucIemOperInfo.Height + ucOthers.Height);
        }


        public void Print()
        {
            PrintForm printForm = new PrintForm(GetPrintDataSource());
            printForm.WindowState = FormWindowState.Maximized;
            printForm.ShowDialog();
        }


        #region 为打印准备数据源
        /// <summary>
        /// 病案首页信息
        /// </summary>
        /// <returns></returns>
        private IemMainPageInfo GetPrintDataSource()
        {
            IemMainPageInfo _IemMainPageInfo = new IemMainPageInfo();

            _IemMainPageInfo.IemBasicInfo = ucIemBasInfo.GetPrintBasicInfo();
            _IemMainPageInfo.IemDiagInfo = ucIemBasInfo.GetPrintDiagnosis(ucIemDiagnose.GetPrintDiagnosis());
            _IemMainPageInfo.IemOperInfo = ucIemOperInfo.GetPrintOperation();
            _IemMainPageInfo.IemFeeInfo = ucOthers.GetPrintFee();
            if (ucIemBabytInfo != null)
            {
                _IemMainPageInfo.IemObstetricsBaby = ucIemBabytInfo.GetPrintObsBaby();
            }
            else
            {
                _IemMainPageInfo.IemObstetricsBaby = null;
            }

            return _IemMainPageInfo;
        }


        #endregion

    }
}
