using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

namespace YidanSoft.Core.IEMMainPage
{
    public partial class InpatientMainPage : DevExpress.XtraEditors.XtraForm
    {
        #region members & propertys
        /// <summary>
        ///主程序
        /// </summary>
        private IYidanEmrHost m_app;
        private IYidanSoftLog m_Logger;

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

        public InpatientMainPage()
            : this(null)
        {
        }
        public InpatientMainPage(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            this.barManagerIme.EditorKeyDown += new KeyEventHandler(barManagerIme_EditorKeyDown);
            
        }

        private void InpatientMainPage_Load(object sender, EventArgs e)
        {
            try
            {
                InitLocation();
                this.btnSave.Visible = false;
                if (m_app.CurrentPatientInfo == null)
                    return;
                GetIemInfo();
                FillUI();
            }
            catch (Exception ex)
            {
                m_Logger.Error(ex);
            }
        }

        /// <summary>
        /// 初始化控件位置
        /// </summary>
        private void InitLocation()
        {
            Int32 pointX = (this.Parent.Width - this.ucIemBasInfo.Width) / 2;
            Int32 pointY = 40;
            this.barButton.FloatLocation = new Point(pointX, 40);
            this.ucIemBasInfo.Location = new Point(pointX, pointY + this.barButton.FloatSize.Height);
            this.ucIemDiagnose.Location = new Point(pointX, pointY + ucIemBasInfo.Height);
            this.ucIemOperInfo.Location = new Point(pointX, pointY + ucIemBasInfo.Height + ucIemDiagnose.Height);
            this.ucOthers.Location = new Point(pointX, pointY + ucIemBasInfo.Height + ucIemDiagnose.Height + ucIemOperInfo.Height);
            btnSave.Location = new Point(pointX + ucOthers.Width - btnSave.Width, ucOthers.Location.Y + ucOthers.Height);//+ btnSave.Height);
        }

        /// <summary>
        /// LOAD时获取病案首页信息
        /// </summary>
        private void GetIemInfo()
        {
            //首先去Iem_Mainpage_Basicinfo根据首页序号捞取资料，如果没有，则LOAD基本用户信息
            SqlParameter[] paraCollection = new SqlParameter[] { new SqlParameter("@NoOfInpat", m_app.CurrentPatientInfo.NoOfFirstPage) };
            DataSet dataSet = m_app.SqlHelper.ExecuteDataSet("usp_GetIemInfo", paraCollection, CommandType.StoredProcedure);
            IemInfo = new IemMainPageInfo();
            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                if (i == 0)
                    GetIemBasInfo(dataSet.Tables[i]);
                else if (i == 1)
                    GetItemDiagInfo(dataSet.Tables[i]);
                else if (i == 2)
                    GetItemOperInfo(dataSet.Tables[i]);
            }
            if (IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
            {
                IemInfo.IemBasicInfo.PatNoOfHis = m_app.CurrentPatientInfo.NoOfHisFirstPage;
                IemInfo.IemBasicInfo.NoOfInpat = m_app.CurrentPatientInfo.NoOfFirstPage;
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
                IemInfo.IemBasicInfo.PayID = row["PayID"].ToString();
                IemInfo.IemBasicInfo.SocialCare = row["SocialCare"].ToString();
                IemInfo.IemBasicInfo.Name = row["Name"].ToString();
                IemInfo.IemBasicInfo.SexID = row["SexID"].ToString();
                IemInfo.IemBasicInfo.Birth = row["Birth"].ToString();
                IemInfo.IemBasicInfo.Marital = row["Marital"].ToString();
                IemInfo.IemBasicInfo.JobID = row["JobID"].ToString();
                IemInfo.IemBasicInfo.ProvinceID = row["ProvinceID"].ToString();
                IemInfo.IemBasicInfo.CountyID = row["CountyID"].ToString();
                IemInfo.IemBasicInfo.NationID = row["NationID"].ToString();
                IemInfo.IemBasicInfo.NationalityID = row["NationalityID"].ToString();
                IemInfo.IemBasicInfo.IDNO = row["IDNO"].ToString();
                IemInfo.IemBasicInfo.Organization = row["Organization"].ToString();
                IemInfo.IemBasicInfo.OfficePlace = row["OfficePlace"].ToString();
                IemInfo.IemBasicInfo.OfficeTEL = row["OfficeTEL"].ToString();
                IemInfo.IemBasicInfo.OfficePost = row["OfficePost"].ToString();
                IemInfo.IemBasicInfo.NativeAddress = row["NativeAddress"].ToString();
                IemInfo.IemBasicInfo.NativeTEL = row["NativeTEL"].ToString();
                IemInfo.IemBasicInfo.NativePost = row["NativePost"].ToString();
                IemInfo.IemBasicInfo.ContactPerson = row["ContactPerson"].ToString();
                IemInfo.IemBasicInfo.Relationship = row["Relationship"].ToString();
                IemInfo.IemBasicInfo.ContactAddress = row["ContactAddress"].ToString();
                IemInfo.IemBasicInfo.ContactTEL = row["ContactTEL"].ToString();
                IemInfo.IemBasicInfo.AdmitDate = row["AdmitDate"].ToString();
                IemInfo.IemBasicInfo.AdmitDept = row["AdmitDept"].ToString();
                IemInfo.IemBasicInfo.AdmitWard = row["AdmitWard"].ToString();
                IemInfo.IemBasicInfo.Days_Before = Convertmy.ToDecimal(row["Days_Before"]);
                IemInfo.IemBasicInfo.Trans_Date = row["Trans_Date"].ToString();
                IemInfo.IemBasicInfo.Trans_AdmitDept = row["Trans_AdmitDept"].ToString();
                IemInfo.IemBasicInfo.Trans_AdmitWard = row["Trans_AdmitWard"].ToString();
                IemInfo.IemBasicInfo.Trans_AdmitDept_Again = row["Trans_AdmitDept_Again"].ToString();
                IemInfo.IemBasicInfo.OutWardDate = row["OutWardDate"].ToString();
                IemInfo.IemBasicInfo.OutHosDept = row["OutHosDept"].ToString();
                IemInfo.IemBasicInfo.OutHosWard = row["OutHosWard"].ToString();
                IemInfo.IemBasicInfo.Actual_Days = Convertmy.ToDecimal(row["Actual_Days"]);
                IemInfo.IemBasicInfo.Death_Time = row["Death_Time"].ToString();
                IemInfo.IemBasicInfo.Death_Reason = row["Death_Reason"].ToString();
                IemInfo.IemBasicInfo.AdmitInfo = row["AdmitInfo"].ToString();
                IemInfo.IemBasicInfo.In_Check_Date = row["In_Check_Date"].ToString();
                IemInfo.IemBasicInfo.Pathology_Diagnosis_Name = row["Pathology_Diagnosis_Name"].ToString();
                IemInfo.IemBasicInfo.Pathology_Observation_Sn = row["Pathology_Observation_Sn"].ToString();
                IemInfo.IemBasicInfo.Ashes_Diagnosis_Name = row["Ashes_Diagnosis_Name"].ToString();
                IemInfo.IemBasicInfo.Ashes_Anatomise_Sn = row["Ashes_Anatomise_Sn"].ToString();
                IemInfo.IemBasicInfo.Allergic_Drug = row["Allergic_Drug"].ToString();
                IemInfo.IemBasicInfo.Hbsag = Convertmy.ToDecimal(row["Hbsag"]);
                IemInfo.IemBasicInfo.Hcv_Ab = Convertmy.ToDecimal(row["Hcv_Ab"]);
                IemInfo.IemBasicInfo.Hiv_Ab = Convertmy.ToDecimal(row["Hiv_Ab"]);
                IemInfo.IemBasicInfo.Opd_Ipd_Id = Convertmy.ToDecimal(row["Opd_Ipd_Id"]);
                IemInfo.IemBasicInfo.In_Out_Inpatinet_Id = Convertmy.ToDecimal(row["In_Out_Inpatinet_Id"]);
                IemInfo.IemBasicInfo.Before_After_Or_Id = Convertmy.ToDecimal(row["Before_After_Or_Id"]);
                IemInfo.IemBasicInfo.Clinical_Pathology_Id = Convertmy.ToDecimal(row["Clinical_Pathology_Id"]);
                IemInfo.IemBasicInfo.Pacs_Pathology_Id = Convertmy.ToDecimal(row["Pacs_Pathology_Id"]);
                IemInfo.IemBasicInfo.Save_Times = Convertmy.ToDecimal(row["Save_Times"]);
                IemInfo.IemBasicInfo.Success_Times = Convertmy.ToDecimal(row["Success_Times"]);
                IemInfo.IemBasicInfo.Section_Director = row["Section_Director"].ToString();
                IemInfo.IemBasicInfo.Director = row["Director"].ToString();
                IemInfo.IemBasicInfo.Vs_Employee_Code = row["Vs_Employee_Code"].ToString();
                IemInfo.IemBasicInfo.Resident_Employee_Code = row["Resident_Employee_Code"].ToString();
                IemInfo.IemBasicInfo.Refresh_Employee_Code = row["Refresh_Employee_Code"].ToString();
                IemInfo.IemBasicInfo.Master_Interne = row["Master_Interne"].ToString();
                IemInfo.IemBasicInfo.Interne = row["Interne"].ToString();
                IemInfo.IemBasicInfo.Coding_User = row["Coding_User"].ToString();
                IemInfo.IemBasicInfo.Medical_Quality_Id = Convertmy.ToDecimal(row["Medical_Quality_Id"]);
                IemInfo.IemBasicInfo.Quality_Control_Doctor = row["Quality_Control_Doctor"].ToString();
                IemInfo.IemBasicInfo.Quality_Control_Nurse = row["Quality_Control_Nurse"].ToString();
                IemInfo.IemBasicInfo.Quality_Control_Date = row["Quality_Control_Date"].ToString();
                IemInfo.IemBasicInfo.Xay_Sn = row["Xay_Sn"].ToString();
                IemInfo.IemBasicInfo.Ct_Sn = row["Ct_Sn"].ToString();
                IemInfo.IemBasicInfo.Mri_Sn = row["Mri_Sn"].ToString();
                IemInfo.IemBasicInfo.Dsa_Sn = row["Dsa_Sn"].ToString();
                IemInfo.IemBasicInfo.Is_First_Case = Convertmy.ToDecimal(row["Is_First_Case"]);
                IemInfo.IemBasicInfo.Is_Following = Convertmy.ToDecimal(row["Is_Following"]);
                IemInfo.IemBasicInfo.Following_Ending_Date = row["Following_Ending_Date"].ToString();
                IemInfo.IemBasicInfo.Is_Teaching_Case = Convertmy.ToDecimal(row["Is_Teaching_Case"]);
                IemInfo.IemBasicInfo.Blood_Type_id = Convertmy.ToDecimal(row["Blood_Type_id"]);
                IemInfo.IemBasicInfo.Rh = Convertmy.ToDecimal(row["Rh"]);
                IemInfo.IemBasicInfo.Blood_Reaction_Id = Convertmy.ToDecimal(row["Blood_Reaction_Id"]);
                IemInfo.IemBasicInfo.Blood_Rbc = Convertmy.ToDecimal(row["Blood_Rbc"]);
                IemInfo.IemBasicInfo.Blood_Plt = Convertmy.ToDecimal(row["Blood_Plt"]);
                IemInfo.IemBasicInfo.Blood_Plasma = Convertmy.ToDecimal(row["Blood_Plasma"]);
                IemInfo.IemBasicInfo.Blood_Wb = Convertmy.ToDecimal(row["Blood_Wb"]);
                IemInfo.IemBasicInfo.Blood_Others = row["Blood_Others"].ToString();
                IemInfo.IemBasicInfo.Is_Completed = row["Is_Completed"].ToString();
                IemInfo.IemBasicInfo.completed_time = row["completed_time"].ToString();
                IemInfo.IemBasicInfo.Valide = Convertmy.ToDecimal(row["Valide"]);
                IemInfo.IemBasicInfo.Create_User = row["Create_User"].ToString();
                IemInfo.IemBasicInfo.Create_Time = row["Create_Time"].ToString();
                IemInfo.IemBasicInfo.Modified_User = row["Modified_User"].ToString();
                IemInfo.IemBasicInfo.Modified_Time = row["Modified_Time"].ToString();
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
            foreach (DataRow row in dataTable.Rows)
            {
                #region 赋值
                Iem_Mainpage_Diagnosis info = new Iem_Mainpage_Diagnosis();
                info.Iem_Mainpage_Diagnosis_NO = Convertmy.ToDecimal(row["Iem_Mainpage_Diagnosis_NO"]);
                info.Iem_Mainpage_NO = Convertmy.ToDecimal(row["Iem_Mainpage_NO"]);
                info.Diagnosis_Type_Id = Convertmy.ToDecimal(row["Diagnosis_Type_Id"]);
                info.Diagnosis_Code = row["Diagnosis_Code"].ToString();
                info.Diagnosis_Name = row["Diagnosis_Name"].ToString();
                info.Status_Id = Convertmy.ToDecimal(row["Status_Id"]);
                info.Order_Value = Convertmy.ToDecimal(row["Order_Value"]);
                info.Valide = Convertmy.ToDecimal(row["Valide"]);
                info.Create_User = row["Create_User"].ToString();
                info.Create_Time = row["Create_Time"].ToString();
                info.Cancel_User = row["Cancel_User"].ToString();
                info.Cancel_User = row["Cancel_User"].ToString();
                IemInfo.IemDiagInfo.Add(info);
                #endregion
            }
        }

        /// <summary>
        /// 手术
        /// </summary>
        /// <param name="dataTable"></param>
        private void GetItemOperInfo(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                #region 赋值
                Iem_MainPage_Operation info = new Iem_MainPage_Operation();
                info.Iem_MainPage_Operation_NO = Convertmy.ToDecimal(row["Iem_MainPage_Operation_NO"]);
                info.IEM_MainPage_NO = Convertmy.ToDecimal(row["IEM_MainPage_NO"]);
                info.Operation_Code = row["Operation_Code"].ToString();
                info.Operation_Date = row["Operation_Date"].ToString();
                info.Operation_Name = row["Operation_Name"].ToString();
                info.Execute_User1 = row["Execute_User1"].ToString();
                info.Execute_User2 = row["Execute_User2"].ToString();
                info.Execute_User3 = row["Execute_User3"].ToString();
                info.Anaesthesia_Type_Id = Convertmy.ToDecimal(row["Anaesthesia_Type_Id"]);
                info.Close_Level = Convertmy.ToDecimal(row["Close_Level"]);
                info.Anaesthesia_User = row["Anaesthesia_User"].ToString();
                info.Valide = Convertmy.ToDecimal(row["Valide"]);
                info.Create_User = row["Create_User"].ToString();
                info.Create_Time = row["Create_Time"].ToString();
                info.Cancel_User = row["Cancel_User"].ToString();
                info.Cancel_User = row["Cancel_User"].ToString();
                IemInfo.IemOperInfo.Add(info);
                #endregion
            }
        }
        #endregion

        private void FillUI()
        {
            this.ucIemBasInfo.FillUI(IemInfo, m_app);
            this.ucIemDiagnose.FillUI(IemInfo, m_app);
            this.ucIemOperInfo.FillUI(IemInfo, m_app);
            this.ucOthers.FillUI(IemInfo, m_app);
        }

        #region 保存



        private void barButtonItemSave_ItemClick(object sender, ItemClickEventArgs e)
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
                m_Logger.Error(ex);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
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
                m_Logger.Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetUI()
        {
            this.IemInfo = this.ucIemBasInfo.IemInfo;

            IemMainPageInfo infoDiag = this.ucIemDiagnose.IemInfo;
            IemMainPageInfo infoOper = this.ucIemOperInfo.IemInfo;
            IemMainPageInfo infoOther = this.ucOthers.IemInfo;



            //this.IemInfo.IemBasicInfo.AdmitInfo = infoDiag.IemBasicInfo.AdmitInfo;
            //this.IemInfo.IemBasicInfo.Pathology_Diagnosis_Name = infoDiag.IemBasicInfo.Pathology_Diagnosis_Name;
            //this.IemInfo.IemBasicInfo.Pathology_Observation_Sn = infoDiag.IemBasicInfo.Pathology_Observation_Sn;
            //this.IemInfo.IemBasicInfo.Ashes_Diagnosis_Name = infoDiag.IemBasicInfo.Ashes_Diagnosis_Name;
            this.IemInfo.IemBasicInfo.Ashes_Anatomise_Sn = infoDiag.IemBasicInfo.Ashes_Anatomise_Sn;
            this.IemInfo.IemBasicInfo.Allergic_Drug = infoDiag.IemBasicInfo.Allergic_Drug;
            this.IemInfo.IemBasicInfo.Hbsag = infoDiag.IemBasicInfo.Hbsag;
            this.IemInfo.IemBasicInfo.Hcv_Ab = infoDiag.IemBasicInfo.Hcv_Ab;
            this.IemInfo.IemBasicInfo.Hiv_Ab = infoDiag.IemBasicInfo.Hiv_Ab;
            this.IemInfo.IemBasicInfo.Opd_Ipd_Id = infoDiag.IemBasicInfo.Opd_Ipd_Id;
            this.IemInfo.IemBasicInfo.In_Out_Inpatinet_Id = infoDiag.IemBasicInfo.In_Out_Inpatinet_Id;
            this.IemInfo.IemBasicInfo.Before_After_Or_Id = infoDiag.IemBasicInfo.Before_After_Or_Id;
            this.IemInfo.IemBasicInfo.Clinical_Pathology_Id = infoDiag.IemBasicInfo.Clinical_Pathology_Id;
            this.IemInfo.IemBasicInfo.Pacs_Pathology_Id = infoDiag.IemBasicInfo.Pacs_Pathology_Id;
            this.IemInfo.IemBasicInfo.Save_Times = infoDiag.IemBasicInfo.Save_Times;
            this.IemInfo.IemBasicInfo.Success_Times = infoDiag.IemBasicInfo.Success_Times;
            this.IemInfo.IemBasicInfo.In_Check_Date = infoDiag.IemBasicInfo.In_Check_Date;
            //基本诊断
            this.IemInfo.IemDiagInfo = infoDiag.IemDiagInfo;
            //手术诊断
            if (infoOper.IemDiagInfo != null)
            {
                foreach (Iem_Mainpage_Diagnosis infoDia in infoOper.IemDiagInfo)
                {
                    this.IemInfo.IemDiagInfo.Add(infoDia);
                }
            }


            this.IemInfo.IemBasicInfo.Xay_Sn = infoOper.IemBasicInfo.Xay_Sn;
            this.IemInfo.IemBasicInfo.Ct_Sn = infoOper.IemBasicInfo.Ct_Sn;
            this.IemInfo.IemBasicInfo.Mri_Sn = infoOper.IemBasicInfo.Mri_Sn;
            this.IemInfo.IemBasicInfo.Dsa_Sn = infoOper.IemBasicInfo.Dsa_Sn;

            this.IemInfo.IemOperInfo = infoOper.IemOperInfo;

            this.IemInfo.IemBasicInfo.Is_First_Case = infoOther.IemBasicInfo.Is_First_Case;
            this.IemInfo.IemBasicInfo.Is_Following = infoOther.IemBasicInfo.Is_Following;
            this.IemInfo.IemBasicInfo.Is_Teaching_Case = infoOther.IemBasicInfo.Is_Teaching_Case;
            this.IemInfo.IemBasicInfo.Blood_Type_id = infoOther.IemBasicInfo.Blood_Type_id;
            this.IemInfo.IemBasicInfo.Blood_Type_id = infoOther.IemBasicInfo.Blood_Type_id;
            this.IemInfo.IemBasicInfo.Blood_Reaction_Id = infoOther.IemBasicInfo.Blood_Reaction_Id;
            this.IemInfo.IemBasicInfo.Blood_Rbc = infoOther.IemBasicInfo.Blood_Rbc;
            this.IemInfo.IemBasicInfo.Blood_Plt = infoOther.IemBasicInfo.Blood_Plt;
            this.IemInfo.IemBasicInfo.Blood_Plasma = infoOther.IemBasicInfo.Blood_Plasma;
            this.IemInfo.IemBasicInfo.Blood_Wb = infoOther.IemBasicInfo.Blood_Wb;
            this.IemInfo.IemBasicInfo.Blood_Others = infoOther.IemBasicInfo.Blood_Others;
            this.IemInfo.IemBasicInfo.Following_Ending_Date = infoOther.IemBasicInfo.Following_Ending_Date;
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

                InsertIemBasicInfo(this.IemInfo.IemBasicInfo, m_app.SqlHelper);

                foreach (Iem_Mainpage_Diagnosis item in this.IemInfo.IemDiagInfo)
                    InserIemDiagnoseInfo(item, m_app.SqlHelper);

                foreach (Iem_MainPage_Operation item in this.IemInfo.IemOperInfo)
                    InserIemOperInfo(item, m_app.SqlHelper);


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

        #region insert

        /// <summary>
        /// insert basic info
        /// </summary>
        /// <param name="info"></param>
        private void InsertIemBasicInfo(Iem_Mainpage_Basicinfo info, IDataAccess sqlHelper)
        {
            info.Create_User = m_app.User.DoctorId;
            #region
            SqlParameter paraPatNoOfHis = new SqlParameter("@PatNoOfHis", SqlDbType.Decimal);
            paraPatNoOfHis.Value = info.PatNoOfHis;
            SqlParameter paraNoOfInpat = new SqlParameter("@NoOfInpat", SqlDbType.Decimal);
            paraNoOfInpat.Value = info.NoOfInpat;
            SqlParameter paraPayID = new SqlParameter("@PayID", SqlDbType.VarChar, 4);
            paraPayID.Value = info.PayID;
            SqlParameter paraSocialCare = new SqlParameter("@SocialCare", SqlDbType.VarChar, 32);
            paraSocialCare.Value = info.SocialCare;
            SqlParameter paraInCount = new SqlParameter("@InCount", SqlDbType.Int);
            paraInCount.Value = info.InCount;
            SqlParameter paraName = new SqlParameter("@Name", SqlDbType.VarChar, 64);
            paraName.Value = info.Name;
            SqlParameter paraSexID = new SqlParameter("@SexID", SqlDbType.VarChar, 4);
            paraSexID.Value = info.SexID;
            SqlParameter paraBirth = new SqlParameter("@Birth", SqlDbType.VarChar, 10);
            paraBirth.Value = info.Birth;
            SqlParameter paraMarital = new SqlParameter("@Marital", SqlDbType.VarChar, 4);
            paraMarital.Value = info.Marital;
            SqlParameter paraJobID = new SqlParameter("@JobID", SqlDbType.VarChar, 4);
            paraJobID.Value = info.JobID;
            SqlParameter paraProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 10);
            paraProvinceID.Value = info.ProvinceID;
            SqlParameter paraCountyID = new SqlParameter("@CountyID", SqlDbType.VarChar, 10);
            paraCountyID.Value = info.CountyID;
            SqlParameter paraNationID = new SqlParameter("@NationID", SqlDbType.VarChar, 4);
            paraNationID.Value = info.NationID;
            SqlParameter paraNationalityID = new SqlParameter("@NationalityID", SqlDbType.VarChar, 4);
            paraNationalityID.Value = info.NationalityID;
            SqlParameter paraIDNO = new SqlParameter("@IDNO", SqlDbType.VarChar, 18);
            paraIDNO.Value = info.IDNO;
            SqlParameter paraOrganization = new SqlParameter("@Organization", SqlDbType.VarChar, 64);
            paraOrganization.Value = info.Organization;
            SqlParameter paraOfficePlace = new SqlParameter("@OfficePlace", SqlDbType.VarChar, 64);
            paraOfficePlace.Value = info.OfficePlace;
            SqlParameter paraOfficeTEL = new SqlParameter("@OfficeTEL", SqlDbType.VarChar, 16);
            paraOfficeTEL.Value = info.OfficeTEL;
            SqlParameter paraOfficePost = new SqlParameter("@OfficePost", SqlDbType.VarChar, 16);
            paraOfficePost.Value = info.OfficePost;
            SqlParameter paraNativeAddress = new SqlParameter("@NativeAddress", SqlDbType.VarChar, 64);
            paraNativeAddress.Value = info.NativeAddress;
            SqlParameter paraNativeTEL = new SqlParameter("@NativeTEL", SqlDbType.VarChar, 16);
            paraNativeTEL.Value = info.NativeTEL;
            SqlParameter paraNativePost = new SqlParameter("@NativePost", SqlDbType.VarChar, 16);
            paraNativePost.Value = info.NativePost;
            SqlParameter paraContactPerson = new SqlParameter("@ContactPerson", SqlDbType.VarChar, 32);
            paraContactPerson.Value = info.ContactPerson;
            SqlParameter paraRelationship = new SqlParameter("@Relationship", SqlDbType.VarChar, 4);
            paraRelationship.Value = info.Relationship;
            SqlParameter paraContactAddress = new SqlParameter("@ContactAddress", SqlDbType.VarChar, 255);
            paraContactAddress.Value = info.ContactAddress;
            SqlParameter paraContactTEL = new SqlParameter("@ContactTEL", SqlDbType.VarChar, 16);
            paraContactTEL.Value = info.ContactTEL;
            SqlParameter paraAdmitDate = new SqlParameter("@AdmitDate", SqlDbType.VarChar, 19);
            paraAdmitDate.Value = info.AdmitDate;
            SqlParameter paraAdmitDept = new SqlParameter("@AdmitDept", SqlDbType.VarChar, 12);
            paraAdmitDept.Value = info.AdmitDept;
            SqlParameter paraAdmitWard = new SqlParameter("@AdmitWard", SqlDbType.VarChar, 12);
            paraAdmitWard.Value = info.AdmitWard;
            SqlParameter paraDays_Before = new SqlParameter("@Days_Before", SqlDbType.Decimal);
            paraDays_Before.Value = info.Days_Before;
            SqlParameter paraTrans_Date = new SqlParameter("@Trans_Date", SqlDbType.VarChar, 19);
            paraTrans_Date.Value = info.Trans_Date;
            SqlParameter paraTrans_AdmitDept = new SqlParameter("@Trans_AdmitDept", SqlDbType.VarChar, 12);
            paraTrans_AdmitDept.Value = info.Trans_AdmitDept;
            SqlParameter paraTrans_AdmitWard = new SqlParameter("@Trans_AdmitWard", SqlDbType.VarChar, 12);
            paraTrans_AdmitWard.Value = info.Trans_AdmitWard;
            SqlParameter paraTrans_AdmitDept_Again = new SqlParameter("@Trans_AdmitDept_Again", SqlDbType.VarChar, 12);
            paraTrans_AdmitDept_Again.Value = info.Trans_AdmitDept_Again;
            SqlParameter paraOutWardDate = new SqlParameter("@OutWardDate", SqlDbType.VarChar, 19);
            paraOutWardDate.Value = info.OutWardDate;
            SqlParameter paraOutHosDept = new SqlParameter("@OutHosDept", SqlDbType.VarChar, 12);
            paraOutHosDept.Value = info.OutHosDept;
            SqlParameter paraOutHosWard = new SqlParameter("@OutHosWard", SqlDbType.VarChar, 12);
            paraOutHosWard.Value = info.OutHosWard;
            SqlParameter paraActual_Days = new SqlParameter("@Actual_Days", SqlDbType.Decimal);
            paraActual_Days.Value = info.Actual_Days;
            SqlParameter paraDeath_Time = new SqlParameter("@Death_Time", SqlDbType.VarChar, 19);
            paraDeath_Time.Value = info.Death_Time;
            SqlParameter paraDeath_Reason = new SqlParameter("@Death_Reason", SqlDbType.VarChar, 300);
            paraDeath_Reason.Value = info.Death_Reason;
            SqlParameter paraAdmitInfo = new SqlParameter("@AdmitInfo", SqlDbType.VarChar, 4);
            paraAdmitInfo.Value = info.AdmitInfo;
            SqlParameter paraIn_Check_Date = new SqlParameter("@In_Check_Date", SqlDbType.VarChar, 19);
            paraIn_Check_Date.Value = info.In_Check_Date;
            SqlParameter paraPathology_Diagnosis_Name = new SqlParameter("@Pathology_Diagnosis_Name", SqlDbType.VarChar, 300);
            paraPathology_Diagnosis_Name.Value = info.Pathology_Diagnosis_Name;
            SqlParameter paraPathology_Observation_Sn = new SqlParameter("@Pathology_Observation_Sn", SqlDbType.VarChar, 60);
            paraPathology_Observation_Sn.Value = info.Pathology_Observation_Sn;
            SqlParameter paraAshes_Diagnosis_Name = new SqlParameter("@Ashes_Diagnosis_Name", SqlDbType.VarChar, 300);
            paraAshes_Diagnosis_Name.Value = info.Ashes_Diagnosis_Name;
            SqlParameter paraAshes_Anatomise_Sn = new SqlParameter("@Ashes_Anatomise_Sn", SqlDbType.VarChar, 60);
            paraAshes_Anatomise_Sn.Value = info.Ashes_Anatomise_Sn;
            SqlParameter paraAllergic_Drug = new SqlParameter("@Allergic_Drug", SqlDbType.VarChar, 300);
            paraAllergic_Drug.Value = info.Allergic_Drug;
            SqlParameter paraHbsag = new SqlParameter("@Hbsag", SqlDbType.Decimal);
            paraHbsag.Value = info.Hbsag;
            SqlParameter paraHcv_Ab = new SqlParameter("@Hcv_Ab", SqlDbType.Decimal);
            paraHcv_Ab.Value = info.Hcv_Ab;
            SqlParameter paraHiv_Ab = new SqlParameter("@Hiv_Ab", SqlDbType.Decimal);
            paraHiv_Ab.Value = info.Hiv_Ab;
            SqlParameter paraOpd_Ipd_Id = new SqlParameter("@Opd_Ipd_Id", SqlDbType.Decimal);
            paraOpd_Ipd_Id.Value = info.Opd_Ipd_Id;
            SqlParameter paraIn_Out_Inpatinet_Id = new SqlParameter("@In_Out_Inpatinet_Id", SqlDbType.Decimal);
            paraIn_Out_Inpatinet_Id.Value = info.In_Out_Inpatinet_Id;
            SqlParameter paraBefore_After_Or_Id = new SqlParameter("@Before_After_Or_Id", SqlDbType.Decimal);
            paraBefore_After_Or_Id.Value = info.Before_After_Or_Id;
            SqlParameter paraClinical_Pathology_Id = new SqlParameter("@Clinical_Pathology_Id", SqlDbType.Decimal);
            paraClinical_Pathology_Id.Value = info.Clinical_Pathology_Id;
            SqlParameter paraPacs_Pathology_Id = new SqlParameter("@Pacs_Pathology_Id", SqlDbType.Decimal);
            paraPacs_Pathology_Id.Value = info.Pacs_Pathology_Id;
            SqlParameter paraSave_Times = new SqlParameter("@Save_Times", SqlDbType.Decimal);
            paraSave_Times.Value = info.Save_Times;
            SqlParameter paraSuccess_Times = new SqlParameter("@Success_Times", SqlDbType.Decimal);
            paraSuccess_Times.Value = info.Success_Times;
            SqlParameter paraSection_Director = new SqlParameter("@Section_Director", SqlDbType.VarChar, 20);
            paraSection_Director.Value = info.Section_Director;
            SqlParameter paraDirector = new SqlParameter("@Director", SqlDbType.VarChar, 20);
            paraDirector.Value = info.Director;
            SqlParameter paraVs_Employee_Code = new SqlParameter("@Vs_Employee_Code", SqlDbType.VarChar, 20);
            paraVs_Employee_Code.Value = info.Vs_Employee_Code;
            SqlParameter paraResident_Employee_Code = new SqlParameter("@Resident_Employee_Code", SqlDbType.VarChar, 20);
            paraResident_Employee_Code.Value = info.Resident_Employee_Code;
            SqlParameter paraRefresh_Employee_Code = new SqlParameter("@Refresh_Employee_Code", SqlDbType.VarChar, 20);
            paraRefresh_Employee_Code.Value = info.Refresh_Employee_Code;
            SqlParameter paraMaster_Interne = new SqlParameter("@Master_Interne", SqlDbType.VarChar, 20);
            paraMaster_Interne.Value = info.Master_Interne;
            SqlParameter paraInterne = new SqlParameter("@Interne", SqlDbType.VarChar, 20);
            paraInterne.Value = info.Interne;
            SqlParameter paraCoding_User = new SqlParameter("@Coding_User", SqlDbType.VarChar, 20);
            paraCoding_User.Value = info.Coding_User;
            SqlParameter paraMedical_Quality_Id = new SqlParameter("@Medical_Quality_Id", SqlDbType.Decimal);
            paraMedical_Quality_Id.Value = info.Medical_Quality_Id;
            SqlParameter paraQuality_Control_Doctor = new SqlParameter("@Quality_Control_Doctor", SqlDbType.VarChar, 20);
            paraQuality_Control_Doctor.Value = info.Quality_Control_Doctor;
            SqlParameter paraQuality_Control_Nurse = new SqlParameter("@Quality_Control_Nurse", SqlDbType.VarChar, 20);
            paraQuality_Control_Nurse.Value = info.Quality_Control_Nurse;
            SqlParameter paraQuality_Control_Date = new SqlParameter("@Quality_Control_Date", SqlDbType.VarChar, 19);
            paraQuality_Control_Date.Value = info.Quality_Control_Date;
            SqlParameter paraXay_Sn = new SqlParameter("@Xay_Sn", SqlDbType.VarChar, 300);
            paraXay_Sn.Value = info.Xay_Sn;
            SqlParameter paraCt_Sn = new SqlParameter("@Ct_Sn", SqlDbType.VarChar, 300);
            paraCt_Sn.Value = info.Ct_Sn;
            SqlParameter paraMri_Sn = new SqlParameter("@Mri_Sn", SqlDbType.VarChar, 300);
            paraMri_Sn.Value = info.Mri_Sn;
            SqlParameter paraDsa_Sn = new SqlParameter("@Dsa_Sn", SqlDbType.VarChar, 300);
            paraDsa_Sn.Value = info.Dsa_Sn;

            SqlParameter paraIs_First_Case = new SqlParameter("@Is_First_Case", SqlDbType.Decimal);
            paraIs_First_Case.Value = info.Is_First_Case;
            SqlParameter paraIs_Following = new SqlParameter("@Is_Following", SqlDbType.Decimal);
            paraIs_Following.Value = info.Is_Following;
            SqlParameter paraFollowing_Ending_Date = new SqlParameter("@Following_Ending_Date", SqlDbType.VarChar, 19);
            paraFollowing_Ending_Date.Value = info.Following_Ending_Date;
            SqlParameter paraIs_Teaching_Case = new SqlParameter("@Is_Teaching_Case", SqlDbType.Decimal);
            paraIs_Teaching_Case.Value = info.Is_Teaching_Case;
            SqlParameter paraBlood_Type_id = new SqlParameter("@Blood_Type_id", SqlDbType.Decimal);
            paraBlood_Type_id.Value = info.Blood_Type_id;
            SqlParameter paraRh = new SqlParameter("@Rh", SqlDbType.Decimal);
            paraRh.Value = info.Rh;
            SqlParameter paraBlood_Reaction_Id = new SqlParameter("@Blood_Reaction_Id", SqlDbType.Decimal);
            paraBlood_Reaction_Id.Value = info.Blood_Reaction_Id;
            SqlParameter paraBlood_Rbc = new SqlParameter("@Blood_Rbc", SqlDbType.Decimal);
            paraBlood_Rbc.Value = info.Blood_Rbc;
            SqlParameter paraBlood_Plt = new SqlParameter("@Blood_Plt", SqlDbType.Decimal);
            paraBlood_Plt.Value = info.Blood_Plt;
            SqlParameter paraBlood_Plasma = new SqlParameter("@Blood_Plasma", SqlDbType.Decimal);
            paraBlood_Plasma.Value = info.Blood_Plasma;
            SqlParameter paraBlood_Wb = new SqlParameter("@Blood_Wb", SqlDbType.Decimal);
            paraBlood_Wb.Value = info.Blood_Wb;
            SqlParameter paraBlood_Others = new SqlParameter("@Blood_Others", SqlDbType.VarChar, 60);
            paraBlood_Others.Value = info.Blood_Others;
            SqlParameter paraIs_Completed = new SqlParameter("@Is_Completed", SqlDbType.VarChar, 1);
            paraIs_Completed.Value = info.Is_Completed;
            SqlParameter paracompleted_time = new SqlParameter("@completed_time", SqlDbType.VarChar, 19);
            paracompleted_time.Value = info.completed_time;
            SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
            paraCreate_User.Value = info.Create_User;

            SqlParameter paraZymosis = new SqlParameter("@Zymosis", SqlDbType.VarChar,300);
            paraZymosis.Value = info.Zymosis;
            SqlParameter paraHurt_Toxicosis_Ele = new SqlParameter("@Hurt_Toxicosis_Ele", SqlDbType.VarChar, 300);
            paraHurt_Toxicosis_Ele.Value = info.Hurt_Toxicosis_Ele;

            SqlParameter[] paraColl = new SqlParameter[] { paraPatNoOfHis, paraNoOfInpat, paraPayID, paraSocialCare, paraInCount, paraName,
                paraSexID, paraBirth, paraMarital ,paraJobID,paraProvinceID,paraCountyID,paraNationID,paraNationalityID,paraIDNO,paraOrganization,paraOfficePlace,
            paraOfficeTEL,paraOfficePost,paraNativeAddress,paraNativeTEL,paraNativePost,paraContactPerson,paraRelationship,paraContactAddress,paraContactTEL,
            paraAdmitDate,paraAdmitDept,paraAdmitWard,paraDays_Before,paraTrans_Date,paraTrans_AdmitDept,paraTrans_AdmitWard,paraTrans_AdmitDept_Again,paraOutWardDate,
            paraOutHosDept,paraOutHosWard,paraActual_Days,paraDeath_Time,paraDeath_Reason,paraAdmitInfo,paraIn_Check_Date,paraPathology_Diagnosis_Name,paraPathology_Observation_Sn,
            paraAshes_Diagnosis_Name, paraAshes_Anatomise_Sn,paraAllergic_Drug,paraHbsag,paraHcv_Ab,paraHiv_Ab,paraOpd_Ipd_Id,paraIn_Out_Inpatinet_Id,paraBefore_After_Or_Id,
            paraClinical_Pathology_Id,paraPacs_Pathology_Id,paraSave_Times,paraSuccess_Times,paraSection_Director,paraDirector,paraVs_Employee_Code,paraResident_Employee_Code,
            paraRefresh_Employee_Code,paraMaster_Interne,paraInterne,paraCoding_User,paraMedical_Quality_Id,paraQuality_Control_Doctor,paraQuality_Control_Nurse,paraQuality_Control_Date,
            paraXay_Sn,paraCt_Sn,paraMri_Sn,paraDsa_Sn,paraIs_First_Case,paraIs_Following,paraFollowing_Ending_Date,paraIs_Teaching_Case,paraBlood_Type_id,paraRh,
            paraBlood_Reaction_Id,paraBlood_Rbc,paraBlood_Plt,paraBlood_Plasma,paraBlood_Wb,paraBlood_Others,paraIs_Completed,paracompleted_time,paraCreate_User,paraZymosis,paraHurt_Toxicosis_Ele};





            #endregion

            String strCmdText = InitSql(paraColl);
            this.IemInfo.IemBasicInfo.Iem_Mainpage_NO = Convertmy.ToDecimal(sqlHelper.ExecuteScalar(strCmdText));

            //this.IemInfo.IemBasicInfo.Iem_Mainpage_NO = Convertmy.ToDecimal(sqlHelper.ExecuteScalar("usp_Insert_Iem_Mainpage_Basic", paraColl, CommandType.StoredProcedure));
        }

        /// <summary>
        /// insert diagnose info
        /// </summary>
        private void InserIemDiagnoseInfo(Iem_Mainpage_Diagnosis info, IDataAccess sqlHelper)
        {
            //info.Create_User = m_app.User.DoctorId;
            //info.Iem_Mainpage_NO = this.IemInfo.IemBasicInfo.Iem_Mainpage_NO.ToString();
            //SqlParameter paraIem_Mainpage_NO = new SqlParameter("@Iem_Mainpage_NO", SqlDbType.Decimal);
            //paraIem_Mainpage_NO.Value = info.Iem_Mainpage_NO;
            //SqlParameter paraDiagnosis_Type_Id = new SqlParameter("@Diagnosis_Type_Id", SqlDbType.Decimal);
            //paraDiagnosis_Type_Id.Value = info.Diagnosis_Type_Id;
            //SqlParameter paraDiagnosis_Code = new SqlParameter("@Diagnosis_Code", SqlDbType.VarChar, 60);
            //paraDiagnosis_Code.Value = info.Diagnosis_Code;
            //SqlParameter paraDiagnosis_Name = new SqlParameter("@Diagnosis_Name", SqlDbType.VarChar, 300);
            //paraDiagnosis_Name.Value = info.Diagnosis_Name;
            //SqlParameter paraStatus_Id = new SqlParameter("@Status_Id", SqlDbType.Decimal);
            //paraStatus_Id.Value = info.Status_Id;
            //SqlParameter paraOrder_Value = new SqlParameter("@Order_Value", SqlDbType.Decimal);
            //paraOrder_Value.Value = info.Order_Value;
            //SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
            //paraCreate_User.Value = info.Create_User;

            SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraDiagnosis_Type_Id, paraDiagnosis_Code, paraDiagnosis_Name, paraStatus_Id, paraOrder_Value, paraCreate_User };

            sqlHelper.ExecuteNoneQuery("usp_Insert_Iem_Mainpage_Diag", paraColl, CommandType.StoredProcedure);
        }

        /// <summary>
        /// insert oper info
        /// </summary>
        private void InserIemOperInfo(Iem_MainPage_Operation info, IDataAccess sqlHelper)
        {
            info.Create_User = m_app.User.DoctorId;
            info.IEM_MainPage_NO = this.IemInfo.IemBasicInfo.Iem_Mainpage_NO.ToString();
            SqlParameter paraIem_Mainpage_NO = new SqlParameter("@IEM_MainPage_NO", SqlDbType.Decimal);
            paraIem_Mainpage_NO.Value = info.IEM_MainPage_NO;
            SqlParameter paraOperation_Code = new SqlParameter("@Operation_Code", SqlDbType.VarChar, 60);
            paraOperation_Code.Value = info.Operation_Code;
            SqlParameter paraOperation_Date = new SqlParameter("@Operation_Date", SqlDbType.VarChar, 19);
            paraOperation_Date.Value = info.Operation_Date;
            SqlParameter paraOperation_Name = new SqlParameter("@Operation_Name", SqlDbType.VarChar, 300);
            paraOperation_Name.Value = info.Operation_Name;
            SqlParameter paraExecute_User1 = new SqlParameter("@Execute_User1", SqlDbType.VarChar, 20);
            paraExecute_User1.Value = info.Execute_User1;
            SqlParameter paraExecute_User2 = new SqlParameter("@Execute_User2", SqlDbType.VarChar, 20);
            paraExecute_User2.Value = info.Execute_User2;
            SqlParameter paraExecute_User3 = new SqlParameter("@Execute_User3", SqlDbType.VarChar, 20);
            paraExecute_User3.Value = info.Execute_User3;
            SqlParameter paraAnaesthesia_Type_Id = new SqlParameter("@Anaesthesia_Type_Id", SqlDbType.Decimal);
            paraAnaesthesia_Type_Id.Value = info.Anaesthesia_Type_Id;
            SqlParameter paraClose_Level = new SqlParameter("@Close_Level", SqlDbType.Decimal);
            paraClose_Level.Value = info.Close_Level;
            SqlParameter paraAnaesthesia_User = new SqlParameter("@Anaesthesia_User", SqlDbType.VarChar, 20);
            paraAnaesthesia_User.Value = info.Anaesthesia_User;
            SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
            paraCreate_User.Value = info.Create_User;

            SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraOperation_Code, paraOperation_Date, paraOperation_Name, paraExecute_User1, paraExecute_User2, paraExecute_User3,
                paraAnaesthesia_Type_Id,paraClose_Level,paraAnaesthesia_User,paraCreate_User };

            sqlHelper.ExecuteNoneQuery("usp_Insert_Iem_MainPage_Oper", paraColl, CommandType.StoredProcedure);

        }


        private String InitSql(SqlParameter[] paraColl)
        {
            StringBuilder str = new StringBuilder();
            str.Append("insert  into dbo.Iem_Mainpage_Basicinfo( PatNoOfHis ,NoOfInpat ,PayID ,SocialCare , InCount ,Name ,SexID ,Birth ,Marital ,JobID ,ProvinceID ,");
            str.Append(" CountyID ,NationID ,NationalityID ,IDNO ,Organization ,OfficePlace ,OfficeTEL ,OfficePost , NativeAddress , NativeTEL ,NativePost ,ContactPerson ,");
            str.Append(" Relationship ,ContactAddress , ContactTEL , AdmitDate , AdmitDept ,AdmitWard , Days_Before ,Trans_Date ,Trans_AdmitDept , Trans_AdmitWard ,");
            str.Append(" Trans_AdmitDept_Again ,OutWardDate , OutHosDept ,OutHosWard ,Actual_Days ,Death_Time ,Death_Reason ,AdmitInfo , In_Check_Date ,Pathology_Diagnosis_Name ,Pathology_Observation_Sn ,");
            str.Append(" Ashes_Diagnosis_Name , Ashes_Anatomise_Sn ,  Allergic_Drug ,  Hbsag ,  Hcv_Ab ,Hiv_Ab ,Opd_Ipd_Id , In_Out_Inpatinet_Id , Before_After_Or_Id , Clinical_Pathology_Id , Pacs_Pathology_Id ,");
            str.Append("  Save_Times ,Success_Times ,Section_Director , Director ,Vs_Employee_Code ,Resident_Employee_Code ,Refresh_Employee_Code ,Master_Interne , Interne , Coding_User , Medical_Quality_Id ,Quality_Control_Doctor ,");
            str.Append(" Quality_Control_Nurse ,Quality_Control_Date , Xay_Sn ,Ct_Sn , Mri_Sn ,Dsa_Sn , Is_First_Case , Is_Following ,Following_Ending_Date ,Is_Teaching_Case ,Blood_Type_id ,");
            str.Append("Rh ,Blood_Reaction_Id ,Blood_Rbc ,Blood_Plt , Blood_Plasma , Blood_Wb ,Blood_Others , Is_Completed ,completed_time  ,Create_User ,Valide,Create_Time,Zymosis,Hurt_Toxicosis_Ele) ");
            str.Append(" values(");
            for (int i = 0; i < paraColl.Length; i++)
            {
                SqlParameter para = paraColl[i];
                if (i != 0)
                    str.Append(",");
                if (para.SqlDbType == SqlDbType.VarChar)
                    str.Append("'" + para.Value + "'");
                else
                    if (para.Value == null)
                        str.Append(System.Data.SqlTypes.SqlString.Null);
                    else
                        str.Append(para.Value);

            }
            str.Append(",1,convert(varchar(19),getdate(),120))");
            str.Append("   select @@identity");
            return str.ToString();
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

                UpdateIemBasicInfo(this.IemInfo.IemBasicInfo, m_app.SqlHelper);

                // 先把之前的诊断，都给CANCLE
                UpdateIemDiagnoseInfo(this.IemInfo, m_app.SqlHelper);
                foreach (Iem_Mainpage_Diagnosis item in this.IemInfo.IemDiagInfo)
                    InserIemDiagnoseInfo(item, m_app.SqlHelper);

                // 先把之前的手术，都给CANCLE
                UpdateIemOperInfo(this.IemInfo, m_app.SqlHelper);
                foreach (Iem_MainPage_Operation item in this.IemInfo.IemOperInfo)
                    InserIemOperInfo(item, m_app.SqlHelper);


                m_app.SqlHelper.CommitTransaction();

                m_app.CustomMessageBox.MessageShow("更新成功");
                GetIemInfo();
                FillUI();
            }
            catch (Exception ex)
            {
                m_app.SqlHelper.RollbackTransaction();
            }
        }

        private void UpdateIemBasicInfo(Iem_Mainpage_Basicinfo info, IDataAccess sqlHelper)
        {
            info.Create_User = m_app.User.DoctorId;
            info.Modified_User = m_app.User.DoctorId;
            #region
            SqlParameter paraPatNoOfHis = new SqlParameter("@PatNoOfHis", SqlDbType.VarChar,14);
            paraPatNoOfHis.Value = info.PatNoOfHis;
            SqlParameter paraNoOfInpat = new SqlParameter("@NoOfInpat", SqlDbType.Decimal);
            paraNoOfInpat.Value = info.NoOfInpat;
            SqlParameter paraPayID = new SqlParameter("@PayID", SqlDbType.VarChar, 4);
            paraPayID.Value = info.PayID;
            SqlParameter paraSocialCare = new SqlParameter("@SocialCare", SqlDbType.VarChar, 32);
            paraSocialCare.Value = info.SocialCare;
            SqlParameter paraInCount = new SqlParameter("@InCount", SqlDbType.Int);
            paraInCount.Value = info.InCount;
            SqlParameter paraName = new SqlParameter("@Name", SqlDbType.VarChar, 64);
            paraName.Value = info.Name;
            SqlParameter paraSexID = new SqlParameter("@SexID", SqlDbType.VarChar, 4);
            paraSexID.Value = info.SexID;
            SqlParameter paraBirth = new SqlParameter("@Birth", SqlDbType.VarChar, 10);
            paraBirth.Value = info.Birth;
            SqlParameter paraMarital = new SqlParameter("@Marital", SqlDbType.VarChar, 4);
            paraMarital.Value = info.Marital;
            SqlParameter paraJobID = new SqlParameter("@JobID", SqlDbType.VarChar, 4);
            paraJobID.Value = info.JobID;
            SqlParameter paraProvinceID = new SqlParameter("@ProvinceID", SqlDbType.VarChar, 10);
            paraProvinceID.Value = info.ProvinceID;
            SqlParameter paraCountyID = new SqlParameter("@CountyID", SqlDbType.VarChar, 10);
            paraCountyID.Value = info.CountyID;
            SqlParameter paraNationID = new SqlParameter("@NationID", SqlDbType.VarChar, 4);
            paraNationID.Value = info.NationID;
            SqlParameter paraNationalityID = new SqlParameter("@NationalityID", SqlDbType.VarChar, 4);
            paraNationalityID.Value = info.NationalityID;
            SqlParameter paraIDNO = new SqlParameter("@IDNO", SqlDbType.VarChar, 18);
            paraIDNO.Value = info.IDNO;
            SqlParameter paraOrganization = new SqlParameter("@Organization", SqlDbType.VarChar, 64);
            paraOrganization.Value = info.Organization;
            SqlParameter paraOfficePlace = new SqlParameter("@OfficePlace", SqlDbType.VarChar, 64);
            paraOfficePlace.Value = info.OfficePlace;
            SqlParameter paraOfficeTEL = new SqlParameter("@OfficeTEL", SqlDbType.VarChar, 16);
            paraOfficeTEL.Value = info.OfficeTEL;
            SqlParameter paraOfficePost = new SqlParameter("@OfficePost", SqlDbType.VarChar, 16);
            paraOfficePost.Value = info.OfficePost;
            SqlParameter paraNativeAddress = new SqlParameter("@NativeAddress", SqlDbType.VarChar, 64);
            paraNativeAddress.Value = info.NativeAddress;
            SqlParameter paraNativeTEL = new SqlParameter("@NativeTEL", SqlDbType.VarChar, 16);
            paraNativeTEL.Value = info.NativeTEL;
            SqlParameter paraNativePost = new SqlParameter("@NativePost", SqlDbType.VarChar, 16);
            paraNativePost.Value = info.NativePost;
            SqlParameter paraContactPerson = new SqlParameter("@ContactPerson", SqlDbType.VarChar, 32);
            paraContactPerson.Value = info.ContactPerson;
            SqlParameter paraRelationship = new SqlParameter("@Relationship", SqlDbType.VarChar, 4);
            paraRelationship.Value = info.Relationship;
            SqlParameter paraContactAddress = new SqlParameter("@ContactAddress", SqlDbType.VarChar, 255);
            paraContactAddress.Value = info.ContactAddress;
            SqlParameter paraContactTEL = new SqlParameter("@ContactTEL", SqlDbType.VarChar, 16);
            paraContactTEL.Value = info.ContactTEL;
            SqlParameter paraAdmitDate = new SqlParameter("@AdmitDate", SqlDbType.VarChar, 19);
            paraAdmitDate.Value = info.AdmitDate;
            SqlParameter paraAdmitDept = new SqlParameter("@AdmitDept", SqlDbType.VarChar, 12);
            paraAdmitDept.Value = info.AdmitDept;
            SqlParameter paraAdmitWard = new SqlParameter("@AdmitWard", SqlDbType.VarChar, 12);
            paraAdmitWard.Value = info.AdmitWard;
            SqlParameter paraDays_Before = new SqlParameter("@Days_Before", SqlDbType.Decimal);
            paraDays_Before.Value = info.Days_Before;
            SqlParameter paraTrans_Date = new SqlParameter("@Trans_Date", SqlDbType.VarChar, 19);
            paraTrans_Date.Value = info.Trans_Date;
            SqlParameter paraTrans_AdmitDept = new SqlParameter("@Trans_AdmitDept", SqlDbType.VarChar, 12);
            paraTrans_AdmitDept.Value = info.Trans_AdmitDept;
            SqlParameter paraTrans_AdmitWard = new SqlParameter("@Trans_AdmitWard", SqlDbType.VarChar, 12);
            paraTrans_AdmitWard.Value = info.Trans_AdmitWard;
            SqlParameter paraTrans_AdmitDept_Again = new SqlParameter("@Trans_AdmitDept_Again", SqlDbType.VarChar, 12);
            paraTrans_AdmitDept_Again.Value = info.Trans_AdmitDept_Again;
            SqlParameter paraOutWardDate = new SqlParameter("@OutWardDate", SqlDbType.VarChar, 19);
            paraOutWardDate.Value = info.OutWardDate;
            SqlParameter paraOutHosDept = new SqlParameter("@OutHosDept", SqlDbType.VarChar, 12);
            paraOutHosDept.Value = info.OutHosDept;
            SqlParameter paraOutHosWard = new SqlParameter("@OutHosWard", SqlDbType.VarChar, 12);
            paraOutHosWard.Value = info.OutHosWard;
            SqlParameter paraActual_Days = new SqlParameter("@Actual_Days", SqlDbType.Decimal);
            paraActual_Days.Value = info.Actual_Days;
            SqlParameter paraDeath_Time = new SqlParameter("@Death_Time", SqlDbType.VarChar, 19);
            paraDeath_Time.Value = info.Death_Time;
            SqlParameter paraDeath_Reason = new SqlParameter("@Death_Reason", SqlDbType.VarChar, 300);
            paraDeath_Reason.Value = info.Death_Reason;
            SqlParameter paraAdmitInfo = new SqlParameter("@AdmitInfo", SqlDbType.VarChar, 4);
            paraAdmitInfo.Value = info.AdmitInfo;
            SqlParameter paraIn_Check_Date = new SqlParameter("@In_Check_Date", SqlDbType.VarChar, 19);
            paraIn_Check_Date.Value = info.In_Check_Date;
            SqlParameter paraPathology_Diagnosis_Name = new SqlParameter("@Pathology_Diagnosis_Name", SqlDbType.VarChar, 300);
            paraPathology_Diagnosis_Name.Value = info.Pathology_Diagnosis_Name;
            SqlParameter paraPathology_Observation_Sn = new SqlParameter("@Pathology_Observation_Sn", SqlDbType.VarChar, 60);
            paraPathology_Observation_Sn.Value = info.Pathology_Observation_Sn;
            SqlParameter paraAshes_Diagnosis_Name = new SqlParameter("@Ashes_Diagnosis_Name", SqlDbType.VarChar, 300);
            paraAshes_Diagnosis_Name.Value = info.Ashes_Diagnosis_Name;
            SqlParameter paraAshes_Anatomise_Sn = new SqlParameter("@Ashes_Anatomise_Sn", SqlDbType.VarChar, 60);
            paraAshes_Anatomise_Sn.Value = info.Ashes_Anatomise_Sn;
            SqlParameter paraAllergic_Drug = new SqlParameter("@Allergic_Drug", SqlDbType.VarChar, 300);
            paraAllergic_Drug.Value = info.Allergic_Drug;
            SqlParameter paraHbsag = new SqlParameter("@Hbsag", SqlDbType.Decimal);
            paraHbsag.Value = info.Hbsag;
            SqlParameter paraHcv_Ab = new SqlParameter("@Hcv_Ab", SqlDbType.Decimal);
            paraHcv_Ab.Value = info.Hcv_Ab;
            SqlParameter paraHiv_Ab = new SqlParameter("@Hiv_Ab", SqlDbType.Decimal);
            paraHiv_Ab.Value = info.Hiv_Ab;
            SqlParameter paraOpd_Ipd_Id = new SqlParameter("@Opd_Ipd_Id", SqlDbType.Decimal);
            paraOpd_Ipd_Id.Value = info.Opd_Ipd_Id;
            SqlParameter paraIn_Out_Inpatinet_Id = new SqlParameter("@In_Out_Inpatinet_Id", SqlDbType.Decimal);
            paraIn_Out_Inpatinet_Id.Value = info.In_Out_Inpatinet_Id;
            SqlParameter paraBefore_After_Or_Id = new SqlParameter("@Before_After_Or_Id", SqlDbType.Decimal);
            paraBefore_After_Or_Id.Value = info.Before_After_Or_Id;
            SqlParameter paraClinical_Pathology_Id = new SqlParameter("@Clinical_Pathology_Id", SqlDbType.Decimal);
            paraClinical_Pathology_Id.Value = info.Clinical_Pathology_Id;
            SqlParameter paraPacs_Pathology_Id = new SqlParameter("@Pacs_Pathology_Id", SqlDbType.Decimal);
            paraPacs_Pathology_Id.Value = info.Pacs_Pathology_Id;
            SqlParameter paraSave_Times = new SqlParameter("@Save_Times", SqlDbType.Decimal);
            paraSave_Times.Value = info.Save_Times;
            SqlParameter paraSuccess_Times = new SqlParameter("@Success_Times", SqlDbType.Decimal);
            paraSuccess_Times.Value = info.Success_Times;
            SqlParameter paraSection_Director = new SqlParameter("@Section_Director", SqlDbType.VarChar, 20);
            paraSection_Director.Value = info.Section_Director;
            SqlParameter paraDirector = new SqlParameter("@Director", SqlDbType.VarChar, 20);
            paraDirector.Value = info.Director;
            SqlParameter paraVs_Employee_Code = new SqlParameter("@Vs_Employee_Code", SqlDbType.VarChar, 20);
            paraVs_Employee_Code.Value = info.Vs_Employee_Code;
            SqlParameter paraResident_Employee_Code = new SqlParameter("@Resident_Employee_Code", SqlDbType.VarChar, 20);
            paraResident_Employee_Code.Value = info.Resident_Employee_Code;
            SqlParameter paraRefresh_Employee_Code = new SqlParameter("@Refresh_Employee_Code", SqlDbType.VarChar, 20);
            paraRefresh_Employee_Code.Value = info.Refresh_Employee_Code;
            SqlParameter paraMaster_Interne = new SqlParameter("@Master_Interne", SqlDbType.VarChar, 20);
            paraMaster_Interne.Value = info.Master_Interne;
            SqlParameter paraInterne = new SqlParameter("@Interne", SqlDbType.VarChar, 20);
            paraInterne.Value = info.Interne;
            SqlParameter paraCoding_User = new SqlParameter("@Coding_User", SqlDbType.VarChar, 20);
            paraCoding_User.Value = info.Coding_User;
            SqlParameter paraMedical_Quality_Id = new SqlParameter("@Medical_Quality_Id", SqlDbType.Decimal);
            paraMedical_Quality_Id.Value = info.Medical_Quality_Id;
            SqlParameter paraQuality_Control_Doctor = new SqlParameter("@Quality_Control_Doctor", SqlDbType.VarChar, 20);
            paraQuality_Control_Doctor.Value = info.Quality_Control_Doctor;
            SqlParameter paraQuality_Control_Nurse = new SqlParameter("@Quality_Control_Nurse", SqlDbType.VarChar, 20);
            paraQuality_Control_Nurse.Value = info.Quality_Control_Nurse;
            SqlParameter paraQuality_Control_Date = new SqlParameter("@Quality_Control_Date", SqlDbType.VarChar, 19);
            paraQuality_Control_Date.Value = info.Quality_Control_Date;
            SqlParameter paraXay_Sn = new SqlParameter("@Xay_Sn", SqlDbType.VarChar, 300);
            paraXay_Sn.Value = info.Xay_Sn;
            SqlParameter paraCt_Sn = new SqlParameter("@Ct_Sn", SqlDbType.VarChar, 300);
            paraCt_Sn.Value = info.Ct_Sn;
            SqlParameter paraMri_Sn = new SqlParameter("@Mri_Sn", SqlDbType.VarChar, 300);
            paraMri_Sn.Value = info.Mri_Sn;
            SqlParameter paraDsa_Sn = new SqlParameter("@Dsa_Sn", SqlDbType.VarChar, 300);
            paraDsa_Sn.Value = info.Dsa_Sn;

            SqlParameter paraIs_First_Case = new SqlParameter("@Is_First_Case", SqlDbType.Decimal);
            paraIs_First_Case.Value = info.Is_First_Case;
            SqlParameter paraIs_Following = new SqlParameter("@Is_Following", SqlDbType.Decimal);
            paraIs_Following.Value = info.Is_Following;
            SqlParameter paraFollowing_Ending_Date = new SqlParameter("@Following_Ending_Date", SqlDbType.VarChar, 19);
            paraFollowing_Ending_Date.Value = info.Following_Ending_Date;
            SqlParameter paraIs_Teaching_Case = new SqlParameter("@Is_Teaching_Case", SqlDbType.Decimal);
            paraIs_Teaching_Case.Value = info.Is_Teaching_Case;
            SqlParameter paraBlood_Type_id = new SqlParameter("@Blood_Type_id", SqlDbType.Decimal);
            paraBlood_Type_id.Value = info.Blood_Type_id;
            SqlParameter paraRh = new SqlParameter("@Rh", SqlDbType.Decimal);
            paraRh.Value = info.Rh;
            SqlParameter paraBlood_Reaction_Id = new SqlParameter("@Blood_Reaction_Id", SqlDbType.Decimal);
            paraBlood_Reaction_Id.Value = info.Blood_Reaction_Id;
            SqlParameter paraBlood_Rbc = new SqlParameter("@Blood_Rbc", SqlDbType.Decimal);
            paraBlood_Rbc.Value = info.Blood_Rbc;
            SqlParameter paraBlood_Plt = new SqlParameter("@Blood_Plt", SqlDbType.Decimal);
            paraBlood_Plt.Value = info.Blood_Plt;
            SqlParameter paraBlood_Plasma = new SqlParameter("@Blood_Plasma", SqlDbType.Decimal);
            paraBlood_Plasma.Value = info.Blood_Plasma;
            SqlParameter paraBlood_Wb = new SqlParameter("@Blood_Wb", SqlDbType.Decimal);
            paraBlood_Wb.Value = info.Blood_Wb;
            SqlParameter paraBlood_Others = new SqlParameter("@Blood_Others", SqlDbType.VarChar, 60);
            paraBlood_Others.Value = info.Blood_Others;
            SqlParameter paraIs_Completed = new SqlParameter("@Is_Completed", SqlDbType.VarChar, 1);
            paraIs_Completed.Value = info.Is_Completed;
            SqlParameter paracompleted_time = new SqlParameter("@completed_time", SqlDbType.VarChar, 19);
            paracompleted_time.Value = info.completed_time;
            SqlParameter paraModified_User = new SqlParameter("@Modified_User", SqlDbType.VarChar, 10);
            paraModified_User.Value = info.Modified_User;
            SqlParameter paraIem_Mainpage_NO = new SqlParameter("@Iem_Mainpage_NO", SqlDbType.Decimal);
            paraIem_Mainpage_NO.Value = info.Iem_Mainpage_NO;
            SqlParameter paraZymosis = new SqlParameter("@Zymosis", SqlDbType.VarChar, 300);
            paraModified_User.Value = info.Zymosis;
            SqlParameter paraHurt_Toxicosis_Ele = new SqlParameter("@Hurt_Toxicosis_Ele", SqlDbType.VarChar,300);
            paraIem_Mainpage_NO.Value = info.Hurt_Toxicosis_Ele;

            SqlParameter[] paraColl = new SqlParameter[] { paraPatNoOfHis, paraNoOfInpat, paraPayID, paraSocialCare, paraInCount, paraName,
                paraSexID, paraBirth, paraMarital ,paraJobID,paraProvinceID,paraCountyID,paraNationID,paraNationalityID,paraIDNO,paraOrganization,paraOfficePlace,
            paraOfficeTEL,paraOfficePost,paraNativeAddress,paraNativeTEL,paraNativePost,paraContactPerson,paraRelationship,paraContactAddress,paraContactTEL,
            paraAdmitDate,paraAdmitDept,paraAdmitWard,paraDays_Before,paraTrans_Date,paraTrans_AdmitDept,paraTrans_AdmitWard,paraTrans_AdmitDept_Again,paraOutWardDate,
            paraOutHosDept,paraOutHosWard,paraActual_Days,paraDeath_Time,paraDeath_Reason,paraAdmitInfo,paraIn_Check_Date,paraPathology_Diagnosis_Name,paraPathology_Observation_Sn,
            paraAshes_Diagnosis_Name, paraAshes_Anatomise_Sn,paraAllergic_Drug,paraHbsag,paraHcv_Ab,paraHiv_Ab,paraOpd_Ipd_Id,paraIn_Out_Inpatinet_Id,paraBefore_After_Or_Id,
            paraClinical_Pathology_Id,paraPacs_Pathology_Id,paraSave_Times,paraSuccess_Times,paraSection_Director,paraDirector,paraVs_Employee_Code,paraResident_Employee_Code,
            paraRefresh_Employee_Code,paraMaster_Interne,paraInterne,paraCoding_User,paraMedical_Quality_Id,paraQuality_Control_Doctor,paraQuality_Control_Nurse,paraQuality_Control_Date,
            paraXay_Sn,paraCt_Sn,paraMri_Sn,paraDsa_Sn,paraIs_First_Case,paraIs_Following,paraFollowing_Ending_Date,paraIs_Teaching_Case,paraBlood_Type_id,paraRh,
            paraBlood_Reaction_Id,paraBlood_Rbc,paraBlood_Plt,paraBlood_Plasma,paraBlood_Wb,paraBlood_Others,paraIs_Completed,paracompleted_time,paraModified_User,paraIem_Mainpage_NO,paraZymosis,paraHurt_Toxicosis_Ele};





            #endregion

            String strCmdText = InitUpdateSql(paraColl);
            sqlHelper.ExecuteNoneQuery(strCmdText);
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
 
        #endregion
        #endregion


        #region 病患选择,暂时保留
        void barManagerIme_EditorKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BaseEdit editor = barManagerIme.ActiveEditor;
                if (editor == null)
                    return;
                BarEditItem barEdit = barManagerIme.ActiveEditItemLink.Item;

                decimal firstPageNo;
                Point position = editor.PointToScreen(new Point(0, 0));
                firstPageNo = ShowListSelectPatient(editor.Text, position);
                if (firstPageNo > 0)
                {
                    m_app.ChoosePatient(firstPageNo);
                    //this.barEditItemChoose.EditValue = m_app.CurrentPatientInfo.RecordNoOfHospital;
                    this.barStaticItemName.Caption = m_app.CurrentPatientInfo.PersonalInformation.PatientName;
                    GetIemInfo();
                    FillUI();
                }
            }
        }

        private LookUpWindow m_LookUpWindow;
        private SqlWordbook PatientWordbook
        {
            get
            {
                if (_patientWordBook == null)
                    CreatePatBook();
                return _patientWordBook;
            }

        }
        private SqlWordbook _patientWordBook;

        public DataSet PatientInfos
        {
            get
            {
                if (_patientInfos == null)
                {
                    _patientInfos = m_app.PatientInfos;
                }
                return _patientInfos;
            }
        }

        private DataSet _patientInfos;


        /// <summary>
        /// 选择病人
        /// </summary>
        /// <param name="initialvalue"></param>
        /// <param name="position"></param>
        /// <returns>选中的病人的首页序号</returns>
        private decimal ShowListSelectPatient(string initialvalue, Point position)
        {
            if (m_LookUpWindow == null)
            {
                m_LookUpWindow = new LookUpWindow();
                m_LookUpWindow.SqlHelper = DataAccessFactory.DefaultDataAccess;
            }

            m_LookUpWindow.CallLookUpWindow(PatientWordbook, WordbookKind.Sql, initialvalue, ShowListFormMode.Concision
                , position, new Size(130, 23), Screen.FromControl(this).Bounds);
            if (m_LookUpWindow.HadGetValue)
                return Convert.ToDecimal(m_LookUpWindow.ResultRows[0]["NoOfInpat"]);
            else
                return -1;
        }

        private void CreatePatBook()
        {
            Dictionary<string, int> colWidths = new Dictionary<string, int>();

            colWidths.Add("BedID", 70);
            colWidths.Add("PatName", 100);
            colWidths.Add("SexName", 50);
            colWidths.Add("AgeStr", 50);
            colWidths.Add("PatID", 130);
            colWidths.Add("AdmitDate", 130);
            /******Modified By dxj 2011/6/21******/
            DataTable dtBed = PatientInfos.Tables["床位信息"].Copy();
            dtBed.Columns["BedID"].Caption = "床位";
            dtBed.Columns["PatName"].Caption = "患者姓名";
            dtBed.Columns["SexName"].Caption = "性 别";
            dtBed.Columns["AgeStr"].Caption = "年龄";
            dtBed.Columns["PatID"].Caption = "住院号";
            dtBed.Columns["AdmitDate"].Caption = "入院日期";
            /*****************/
            _patientWordBook = new SqlWordbook("Inpatients", dtBed, "NoOfInpat", "PatName"
               , colWidths, "BedID//Py//PatID");
            _patientWordBook.ExtraCondition = "InBed = 1301";
        }
        #endregion

    }
}