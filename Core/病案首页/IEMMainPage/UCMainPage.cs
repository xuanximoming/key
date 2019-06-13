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
            DataSet dataSet = m_app.SqlHelper.ExecuteDataSet("usp_GetIemInfo", paraCollection, CommandType.StoredProcedure);
            IemInfo = new IemMainPageInfo();
            for (int i = 0; i < dataSet.Tables.Count; i++)
            {
                if (i == 0)
                    GetIemBasInfo(dataSet.Tables[i]);
                else if (i == 1)
                    GetItemDiagInfo(dataSet.Tables[i]);
                else if (i == 2)
                    //GetItemOperInfo(dataSet.Tables[i]);
                    IemInfo.OperationTable = dataSet.Tables[i];
                else if (i == 3)
                    GetItemObsBaby(dataSet.Tables[i]);
            }
            if (IemInfo.IemBasicInfo.Iem_Mainpage_NO == 0)
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
                IemInfo.IemBasicInfo.Iem_Mainpage_NO = Convertmy.ToDecimal(row["Iem_Mainpage_NO"]);

                IemInfo.IemBasicInfo.PatNoOfHis = row["PatNoOfHis"].ToString();
                IemInfo.IemBasicInfo.NoOfInpat = Convertmy.ToDecimal(row["NoOfInpat"]);
                IemInfo.IemBasicInfo.InCount = Convertmy.ToInt32(row["InCount"]);
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
                IemInfo.IemBasicInfo.Zymosis = row["Zymosis"].ToString();
                IemInfo.IemBasicInfo.Hurt_Toxicosis_Ele = row["Hurt_Toxicosis_Ele"].ToString();

                IemInfo.IemBasicInfo.ZymosisState = row["ZymosisState"].ToString();
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
            Iem_Mainpage_Diagnosis info = new Iem_Mainpage_Diagnosis();
            DataTable dt = m_IemInfo.OutDiagTable ;
            foreach (DataRow row in dataTable.Rows)
            {
                #region 赋值
                if (row["Diagnosis_Type_Id"].ToString() == "7" || row["Diagnosis_Type_Id"].ToString() == "8")
                {
                    DataRow dr = dt.NewRow();
                    dr["Diagnosis_Name"] = row["Diagnosis_Name"].ToString();
                    dr["Status_Id"] = row["Status_Id"].ToString();
                    //dr["Status_Name"] = row["Status_Name"].ToString();
                    switch (row["Status_Id"].ToString())
                    { 
                        case "1" :
                            dr["Status_Name"] = "治愈";
                            break;
                        case "2":
                            dr["Status_Name"] = "好转";
                            break;
                        case "3":
                            dr["Status_Name"] = "未愈";
                            break;
                        case "4":
                            dr["Status_Name"] = "死亡";
                            break;
                        case "5":
                            dr["Status_Name"] = "其他";
                            break;
                    }
                    dr["Diagnosis_Code"] = row["Diagnosis_Code"].ToString();
                    dt.Rows.Add(dr);
                } 
                else
                {

                    info = new Iem_Mainpage_Diagnosis();
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
                }
                #endregion
            }

            m_IemInfo.OutDiagTable = dt;
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

        private void GetItemObsBaby(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                #region 赋值
                IemInfo.IemObstetricsBaby.IEM_MainPage_NO = row["Iem_Mainpage_NO"].ToString();

                IemInfo.IemObstetricsBaby.TC = row["TC"].ToString();
                IemInfo.IemObstetricsBaby.TB = row["TB"].ToString();
                IemInfo.IemObstetricsBaby.CC =  row["CC"].ToString();
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
                if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == 0)
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
            this.IemInfo = this.ucIemBasInfo.IemInfo;

            IemMainPageInfo infoDiag = this.ucIemDiagnose.IemInfo;
            IemMainPageInfo infoOper = this.ucIemOperInfo.IemInfo;
            IemMainPageInfo infoOther = this.ucOthers.IemInfo;
            IemMainPageInfo infoObstetircsBaby = this.ucIemBabytInfo.IemInfo;


            this.IemInfo.IemBasicInfo.AdmitInfo = infoDiag.IemBasicInfo.AdmitInfo;
            this.IemInfo.IemBasicInfo.Pathology_Diagnosis_Name = infoDiag.IemBasicInfo.Pathology_Diagnosis_Name;
            this.IemInfo.IemBasicInfo.Pathology_Observation_Sn = infoDiag.IemBasicInfo.Pathology_Observation_Sn;
            this.IemInfo.IemBasicInfo.Ashes_Diagnosis_Name = infoDiag.IemBasicInfo.Ashes_Diagnosis_Name;
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
            
            this.IemInfo.IemBasicInfo.Zymosis = infoDiag.IemBasicInfo.Zymosis;
            this.IemInfo.IemBasicInfo.Hurt_Toxicosis_Ele = infoDiag.IemBasicInfo.Hurt_Toxicosis_Ele;
            this.IemInfo.IemBasicInfo.ZymosisState = infoDiag.IemBasicInfo.ZymosisState;

            this.IemInfo.IemObstetricsBaby = infoObstetircsBaby.IemObstetricsBaby;
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

        #region insert

        /// <summary>
        /// insert basic info
        /// </summary>
        /// <param name="info"></param>
        private void InsertIemBasicInfo(Iem_Mainpage_Basicinfo info, IDataAccess sqlHelper)
        {
            info.Create_User = m_app.User.DoctorId;

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
            SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
            paraCreate_User.Value = info.Create_User;

            SqlParameter paraZymosis = new SqlParameter("@Zymosis", SqlDbType.VarChar, 300);
            paraZymosis.Value = info.Zymosis;
            SqlParameter paraHurt_Toxicosis_Ele = new SqlParameter("@Hurt_Toxicosis_Ele", SqlDbType.VarChar, 300);
            paraHurt_Toxicosis_Ele.Value = info.Hurt_Toxicosis_Ele;
            SqlParameter paraZymosisState = new SqlParameter("@ZymosisState", SqlDbType.VarChar, 300);
            paraZymosisState.Value = info.ZymosisState;

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
            this.IemInfo.IemBasicInfo.Iem_Mainpage_NO = Convertmy.ToDecimal(no);

            //this.IemInfo.IemBasicInfo.Iem_Mainpage_NO = Convertmy.ToDecimal(sqlHelper.ExecuteScalar("usp_Insert_Iem_Mainpage_Basic", paraColl, CommandType.StoredProcedure));
        }

        /// <summary>
        /// insert diagnose info
        /// </summary>
        private void InserIemDiagnoseInfo(Iem_Mainpage_Diagnosis info, IDataAccess sqlHelper)
        {
            info.Create_User = m_app.User.DoctorId;
            info.Iem_Mainpage_NO = this.IemInfo.IemBasicInfo.Iem_Mainpage_NO;
            SqlParameter paraIem_Mainpage_NO = new SqlParameter("@Iem_Mainpage_NO", SqlDbType.Decimal);
            paraIem_Mainpage_NO.Value = info.Iem_Mainpage_NO;
            SqlParameter paraDiagnosis_Type_Id = new SqlParameter("@Diagnosis_Type_Id", SqlDbType.Decimal);
            paraDiagnosis_Type_Id.Value = info.Diagnosis_Type_Id;
            SqlParameter paraDiagnosis_Code = new SqlParameter("@Diagnosis_Code", SqlDbType.VarChar, 60);
            paraDiagnosis_Code.Value = info.Diagnosis_Code;
            SqlParameter paraDiagnosis_Name = new SqlParameter("@Diagnosis_Name", SqlDbType.VarChar, 300);
            paraDiagnosis_Name.Value = info.Diagnosis_Name;
            SqlParameter paraStatus_Id = new SqlParameter("@Status_Id", SqlDbType.Decimal);
            paraStatus_Id.Value = info.Status_Id;
            SqlParameter paraOrder_Value = new SqlParameter("@Order_Value", SqlDbType.Decimal);
            paraOrder_Value.Value = info.Order_Value;
            SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
            paraCreate_User.Value = info.Create_User;

            SqlParameter[] paraColl = new SqlParameter[] { paraIem_Mainpage_NO, paraDiagnosis_Type_Id, paraDiagnosis_Code, paraDiagnosis_Name, paraStatus_Id, paraOrder_Value, paraCreate_User };

            sqlHelper.ExecuteNoneQuery("IEM_MAIN_PAGE.usp_Insert_Iem_Mainpage_Diag", paraColl, CommandType.StoredProcedure);
        }

        /// <summary>
        /// insert oper info
        /// </summary>
        private void InserIemOperInfo(Iem_MainPage_Operation info, IDataAccess sqlHelper)
        {
            info.Create_User = m_app.User.DoctorId;
            info.IEM_MainPage_NO = this.IemInfo.IemBasicInfo.Iem_Mainpage_NO;
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
            SqlParameter paraBITHDAY = new SqlParameter("@BITHDAY",SqlDbType.VarChar, 1);
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

        /*
        private String InitSql(SqlParameter[] paraColl)
        {
            StringBuilder str = new StringBuilder();
            str.Append(" insert  into Iem_Mainpage_Basicinfo(Iem_Mainpage_NO, PatNoOfHis ,NoOfInpat ,PayID ,SocialCare , InCount ,Name ,SexID ,Birth ,Marital ,JobID ,ProvinceID ,");
            str.Append(" CountyID ,NationID ,NationalityID ,IDNO ,Organization ,OfficePlace ,OfficeTEL ,OfficePost , NativeAddress , NativeTEL ,NativePost ,ContactPerson ,");
            str.Append(" Relationship ,ContactAddress , ContactTEL , AdmitDate , AdmitDept ,AdmitWard , Days_Before ,Trans_Date ,Trans_AdmitDept , Trans_AdmitWard ,");
            str.Append(" Trans_AdmitDept_Again ,OutWardDate , OutHosDept ,OutHosWard ,Actual_Days ,Death_Time ,Death_Reason ,AdmitInfo , In_Check_Date ,Pathology_Diagnosis_Name ,Pathology_Observation_Sn ,");
            str.Append(" Ashes_Diagnosis_Name , Ashes_Anatomise_Sn ,  Allergic_Drug ,  Hbsag ,  Hcv_Ab ,Hiv_Ab ,Opd_Ipd_Id , In_Out_Inpatinet_Id , Before_After_Or_Id , Clinical_Pathology_Id , Pacs_Pathology_Id ,");
            str.Append(" Save_Times ,Success_Times ,Section_Director , Director ,Vs_Employee_Code ,Resident_Employee_Code ,Refresh_Employee_Code ,Master_Interne , Interne , Coding_User , Medical_Quality_Id ,Quality_Control_Doctor ,");
            str.Append(" Quality_Control_Nurse ,Quality_Control_Date , Xay_Sn ,Ct_Sn , Mri_Sn ,Dsa_Sn , Is_First_Case , Is_Following ,Following_Ending_Date ,Is_Teaching_Case ,Blood_Type_id ,");
            str.Append(" Rh ,Blood_Reaction_Id ,Blood_Rbc ,Blood_Plt , Blood_Plasma , Blood_Wb ,Blood_Others , Is_Completed ,completed_time , Create_User , Valide, Create_Time) ");
            str.Append(" values(");
            str.Append(" SEQ_IEM_MAINPAGE_BASICINFO_ID.NEXTVAL , ");

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

            str.Append(",1,to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss')); ");
            str.Append(" select SEQ_IEM_MAINPAGE_BASICINFO_ID.CURRVAL from dual;");

            return str.ToString();
        }
        */

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


                //修改产妇婴儿系想你
                DeleteIemObstetricsBaby(IemInfo.IemObstetricsBaby, m_app.SqlHelper);
                InsertIemObstetricsBaby(IemInfo.IemObstetricsBaby, m_app.SqlHelper);

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
            paraZymosis.Value = info.Zymosis;
            SqlParameter paraHurt_Toxicosis_Ele = new SqlParameter("@Hurt_Toxicosis_Ele", SqlDbType.VarChar,300);
            paraHurt_Toxicosis_Ele.Value = info.Hurt_Toxicosis_Ele;
            SqlParameter paraZymosisState = new SqlParameter("@ZymosisState", SqlDbType.VarChar, 300);
            paraZymosisState.Value = info.ZymosisState;

            SqlParameter[] paraColl = new SqlParameter[] { paraPatNoOfHis, paraNoOfInpat, paraPayID, paraSocialCare, paraInCount, paraName,
                paraSexID, paraBirth, paraMarital ,paraJobID,paraProvinceID,paraCountyID,paraNationID,paraNationalityID,paraIDNO,paraOrganization,paraOfficePlace,
            paraOfficeTEL,paraOfficePost,paraNativeAddress,paraNativeTEL,paraNativePost,paraContactPerson,paraRelationship,paraContactAddress,paraContactTEL,
            paraAdmitDate,paraAdmitDept,paraAdmitWard,paraDays_Before,paraTrans_Date,paraTrans_AdmitDept,paraTrans_AdmitWard,paraTrans_AdmitDept_Again,paraOutWardDate,
            paraOutHosDept,paraOutHosWard,paraActual_Days,paraDeath_Time,paraDeath_Reason,paraAdmitInfo,paraIn_Check_Date,paraPathology_Diagnosis_Name,paraPathology_Observation_Sn,
            paraAshes_Diagnosis_Name, paraAshes_Anatomise_Sn,paraAllergic_Drug,paraHbsag,paraHcv_Ab,paraHiv_Ab,paraOpd_Ipd_Id,paraIn_Out_Inpatinet_Id,paraBefore_After_Or_Id,
            paraClinical_Pathology_Id,paraPacs_Pathology_Id,paraSave_Times,paraSuccess_Times,paraSection_Director,paraDirector,paraVs_Employee_Code,paraResident_Employee_Code,
            paraRefresh_Employee_Code,paraMaster_Interne,paraInterne,paraCoding_User,paraMedical_Quality_Id,paraQuality_Control_Doctor,paraQuality_Control_Nurse,paraQuality_Control_Date,
            paraXay_Sn,paraCt_Sn,paraMri_Sn,paraDsa_Sn,paraIs_First_Case,paraIs_Following,paraFollowing_Ending_Date,paraIs_Teaching_Case,paraBlood_Type_id,paraRh,
            paraBlood_Reaction_Id,paraBlood_Rbc,paraBlood_Plt,paraBlood_Plasma,paraBlood_Wb,paraBlood_Others,paraIs_Completed,paracompleted_time,paraModified_User,paraIem_Mainpage_NO,
            paraZymosis,paraHurt_Toxicosis_Ele,paraZymosisState};





            #endregion

            String strCmdText = InitUpdateSql(paraColl);
            sqlHelper.ExecuteNoneQuery(strCmdText, CommandType.Text);
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
            string sql = string.Format("delete IEM_MAINPAGE_OBSTETRICSBABY where IEM_MAINPAGE_NO = '{0}'", m_IemInfo.IemBasicInfo.Iem_Mainpage_NO);

            sqlHelper.ExecuteNoneQuery(sql, CommandType.Text);

        }
        

        private String InitUpdateSql(SqlParameter[] paraColl)
        {
            foreach (SqlParameter para in paraColl)
            {
                if (para.SqlDbType != SqlDbType.VarChar)
                {
                    if (para.Value == null)
                        para.Value = System.Data.SqlTypes.SqlString.Null;
                }
                else
                {
                    if (para.Value == null)
                        para.Value = String.Empty;
                }
            }
            StringBuilder str = new StringBuilder();
            str.Append("update  Iem_Mainpage_Basicinfo");
            str.Append(" set     PatNoOfHis = '" + paraColl[0].Value + "' ,");
            str.Append(" NoOfInpat =" + paraColl[1].Value + ",");
            str.Append(" PayID =  '" + paraColl[2].Value + "',");
            str.Append(" SocialCare ='" + paraColl[3].Value + "',");
            str.Append(" InCount =  " + paraColl[4].Value + ",");
            str.Append(" Name ='" + paraColl[5].Value + "',");
            str.Append(" SexID = '" + paraColl[6].Value + "',");
            str.Append(" Birth ='" + paraColl[7].Value + "',");
            str.Append(" Marital ='" + paraColl[8].Value + "',");
            str.Append(" JobID = '" + paraColl[9].Value + "',");
            str.Append(" ProvinceID ='" + paraColl[10].Value + "',");
            str.Append(" CountyID ='" + paraColl[11].Value + "',");
            str.Append(" NationID ='" + paraColl[12].Value + "' ,");
            str.Append(" NationalityID = '" + paraColl[13].Value + "',");
            str.Append(" IDNO = '" + paraColl[14].Value + "',");
            str.Append(" Organization ='" + paraColl[15].Value + "' ,");
            str.Append(" OfficePlace = '" + paraColl[16].Value + "',");
            str.Append(" OfficeTEL =  '" + paraColl[17].Value + "',");
            str.Append(" OfficePost = '" + paraColl[18].Value + "',");
            str.Append(" NativeAddress = '" + paraColl[19].Value + "',");
            str.Append(" NativeTEL = '" + paraColl[20].Value + "',");
            str.Append(" NativePost = '" + paraColl[21].Value + "',");
            str.Append(" ContactPerson = '" + paraColl[22].Value + "',");
            str.Append(" Relationship = '" + paraColl[23].Value + "',");
            str.Append(" ContactAddress = '" + paraColl[24].Value + "',");
            str.Append(" ContactTEL =  '" + paraColl[25].Value + "',");
            str.Append(" AdmitDate =  '" + paraColl[26].Value + "',");
            str.Append(" AdmitDept =  '" + paraColl[27].Value + "',");
            str.Append(" AdmitWard = '" + paraColl[28].Value + "',");
            str.Append(" Days_Before = '" + paraColl[29].Value + "',");
            str.Append(" Trans_Date = '" + paraColl[30].Value + "',");
            str.Append(" Trans_AdmitDept = '" + paraColl[31].Value + "',");
            str.Append(" Trans_AdmitWard = '" + paraColl[32].Value + "',");
            str.Append(" Trans_AdmitDept_Again = '" + paraColl[33].Value + "',");
            str.Append(" OutWardDate = '" + paraColl[34].Value + "', ");
            str.Append(" OutHosDept = '" + paraColl[35].Value + "',");
            str.Append(" OutHosWard = '" + paraColl[36].Value + "',");
            str.Append(" Actual_Days = " + paraColl[37].Value + ",");
            str.Append(" Death_Time = '" + paraColl[38].Value + "',");
            str.Append(" Death_Reason = '" + paraColl[39].Value + "',");
            str.Append(" AdmitInfo = '" + paraColl[40].Value + "',");
            str.Append(" In_Check_Date = '" + paraColl[41].Value + "',");
            str.Append(" Pathology_Diagnosis_Name = '" + paraColl[42].Value + "',");
            str.Append(" Pathology_Observation_Sn = '" + paraColl[43].Value + "' ,");
            str.Append(" Ashes_Diagnosis_Name = '" + paraColl[44].Value + "',");
            str.Append(" Ashes_Anatomise_Sn = '" + paraColl[45].Value + "',");
            str.Append(" Allergic_Drug = '" + paraColl[46].Value + "',");
            str.Append(" Hbsag = " + paraColl[47].Value + " ,");
            str.Append(" Hcv_Ab = " + paraColl[48].Value + " ,");
            str.Append(" Hiv_Ab = " + paraColl[49].Value + " ,");
            str.Append(" Opd_Ipd_Id = " + paraColl[50].Value + " ,");
            str.Append(" In_Out_Inpatinet_Id = " + paraColl[51].Value + " ,");
            str.Append(" Before_After_Or_Id = " + paraColl[52].Value + " ,");
            str.Append(" Clinical_Pathology_Id = " + paraColl[53].Value + " ,");
            str.Append(" Pacs_Pathology_Id = " + paraColl[54].Value + " ,");
            str.Append(" Save_Times = " + paraColl[55].Value + " ,");
            str.Append(" Success_Times = " + paraColl[56].Value + " ,");
            str.Append(" Section_Director = '" + paraColl[57].Value + "',");
            str.Append(" Director = '" + paraColl[58].Value + "',");
            str.Append(" Vs_Employee_Code = '" + paraColl[59].Value + "',");
            str.Append(" Resident_Employee_Code = '" + paraColl[60].Value + "',");
            str.Append(" Refresh_Employee_Code = '" + paraColl[61].Value + "',");
            str.Append(" Master_Interne = '" + paraColl[62].Value + "',");
            str.Append(" Interne = '" + paraColl[63].Value + "',");
            str.Append(" Coding_User = '" + paraColl[64].Value + " ',");
            str.Append(" Medical_Quality_Id = " + paraColl[65].Value + " ,");
            str.Append(" Quality_Control_Doctor = '" + paraColl[66].Value + "' ,");
            str.Append(" Quality_Control_Nurse = '" + paraColl[67].Value + "' ,");
            str.Append(" Quality_Control_Date = '" + paraColl[68].Value + "' ,");
            str.Append(" Xay_Sn = '" + paraColl[69].Value + "' ,");
            str.Append(" Ct_Sn = '" + paraColl[70].Value + "' ,");
            str.Append(" Mri_Sn = '" + paraColl[71].Value + "' ,");
            str.Append(" Dsa_Sn = '" + paraColl[72].Value + "' ,");
            str.Append(" Is_First_Case =" + paraColl[73].Value + " ,");
            str.Append(" Is_Following = " + paraColl[74].Value + " ,");
            str.Append(" Following_Ending_Date = '" + paraColl[75].Value + "' ,");
            str.Append(" Is_Teaching_Case = " + paraColl[76].Value + " ,");
            str.Append(" Blood_Type_id = " + paraColl[77].Value + " ,");
            str.Append(" Rh = " + paraColl[78].Value + " ,");
            str.Append(" Blood_Reaction_Id = " + paraColl[79].Value + " ,");
            str.Append(" Blood_Rbc = " + paraColl[80].Value + " ,");
            str.Append(" Blood_Plt = " + paraColl[81].Value + " ,");
            str.Append(" Blood_Plasma = " + paraColl[82].Value + " ,");
            str.Append(" Blood_Wb = " + paraColl[83].Value + " ,");
            str.Append(" Blood_Others = '" + paraColl[84].Value + "' ,");
            str.Append(" Is_Completed = '" + paraColl[85].Value + "',");
            str.Append(" completed_time = '" + paraColl[86].Value + "' ,");
            str.Append(" Modified_User = '" + paraColl[87].Value + "' ,");
            str.Append(" Modified_Time = to_char(sysdate, 'yyyy-mm-dd hh24:mi:ss'),");
            str.Append(" Zymosis = '" + paraColl[89].Value + "' ,");
            str.Append(" Hurt_Toxicosis_Ele = '" + paraColl[90].Value + "',");
            str.Append(" ZymosisState = '" + paraColl[91].Value + "'");
            str.Append(" where   Iem_Mainpage_NO = " + paraColl[88].Value + "");
            str.Append(" and Valide = 1 ;");

            return str.ToString();
        }
        #endregion
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

            if(ucIemBabytInfo!=null)
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
        private Print_IemMainPageInfo GetPrintDataSource()
        {
            Print_IemMainPageInfo _Print_IemMainPageInfo = new Print_IemMainPageInfo();

            _Print_IemMainPageInfo.IemBasicInfo = ucIemBasInfo.GetPrintBasicInfo();
            _Print_IemMainPageInfo.IemDiagInfo = ucIemBasInfo.GetPrintDiagnosis(ucIemDiagnose.GetPrintDiagnosis());
            _Print_IemMainPageInfo.IemOperInfo = ucIemOperInfo.GetPrintOperation();
            _Print_IemMainPageInfo.IemFeeInfo = ucOthers.GetPrintFee();
            if (ucIemBabytInfo != null)
            {
                _Print_IemMainPageInfo.IemObstetricsBaby = ucIemBabytInfo.GetPrintObsBaby();
            }
            else
            {
                _Print_IemMainPageInfo.IemObstetricsBaby = null;
            }

            return _Print_IemMainPageInfo;
        }

 
        #endregion
 
    }
}
