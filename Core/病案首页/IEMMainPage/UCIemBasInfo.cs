using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using YidanSoft.Wordbook;
using YidanSoft.Common.Library;

using YidanSoft.FrameWork.WinForm.Plugin;

using Convertmy = YidanSoft.Core.UtilsForExtension;
using DevExpress.XtraEditors;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class UCIemBasInfo : UserControl
    {
        private IDataAccess m_SqlHelper;
        private IYidanEmrHost m_App;
        private IemMainPageInfo m_IemInfo;
        private DataHelper m_DataHelper = new DataHelper();

        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get
            {
                if (m_IemInfo == null)
                    m_IemInfo = new IemMainPageInfo();
                GetUI();
                return m_IemInfo;
            }
        }

        public UCIemBasInfo()
        {
            InitializeComponent();

            m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            InitLookUpEditor();
        }

        private void UCIemBasInfo_Load(object sender, EventArgs e)
        {
            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();

            lueProvice.Focus();
#if DEBUG
#else
            //HideSbutton();
#endif
        }

        #region private methods

        /// <summary>
        /// 初始化lookupeditor
        /// </summary>
        private void InitLookUpEditor()
        {
            InitLuePayId();
            InitLueSex();
            InitMarital();
            InitJob();
            InitProvice();
            InitCountry();
            InitNation();
            InitNationality();
            InitRelationship();
            InitDept();
            InitEmployee();
        }

        #region UI上lue的数据源赋值
        /// <summary>
        /// 医疗付款方式
        /// </summary>
        private void InitLuePayId()
        {
            BindLueData(luePayId, 1);
        }

        /// <summary>
        /// 病人性别
        /// </summary>
        private void InitLueSex()
        {
            BindLueData(lueSex, 2);
        }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        private void InitMarital()
        {
            BindLueData(lueMarital, 3);
        }

        /// <summary>
        /// 职业代码
        /// </summary>
        private void InitJob()
        {
            BindLueData(lueJob, 4);
        }

        /// <summary>
        /// 省市代码
        /// </summary>
        private void InitProvice()
        {
            BindLueData(lueProvice, 5);
        }

        ///// <summary>
        ///// 市代码
        ///// </summary>
        //private void InitProvAndCity()
        //{
        //    BindLueData(lueProvice, 4);
        //}


        /// <summary>
        /// 区县代码，在省市代码的CHANGEG事件里处理
        /// </summary>
        private void InitCountry()
        {
            BindLueCountryData(lueCounty, 13);
        }

        private DataTable m_DataTableCountry;
        /// <summary>
        /// 区县代码
        /// </summary>
        /// <param name="lueInfo"></param>
        /// <param name="queryType"></param>
        private void BindLueCountryData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            if (m_DataTableCountry == null)
                m_DataTableCountry = GetEditroData(queryType);

            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueInfo.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", m_DataTableCountry, "ID", "Name", columnwidth, true);

            lueInfo.SqlWordbook = sqlWordBook;
            lueInfo.ListWindow = lupInfo;
        }
        /// <summary>
        /// 区县代码
        /// </summary>
        /// <param name="lueInfo"></param>
        /// <param name="queryType"></param>
        private void BindLueCountryData(LookUpEditor lueInfo, DataTable dataTable)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;

            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueInfo.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

            lueInfo.SqlWordbook = sqlWordBook;
            lueInfo.ListWindow = lupInfo;
        }

        /// <summary>
        /// 民族代码
        /// </summary>
        private void InitNation()
        {
            BindLueData(lueNation, 6);
        }

        /// <summary>
        /// 国籍代码
        /// </summary>
        private void InitNationality()
        {
            BindLueData(lueNationality, 7);
        }

        /// <summary>
        /// 联系关系
        /// </summary>
        private void InitRelationship()
        {
            BindLueData(lueRelationship, 8);
        }


        private DataTable m_DataTableWard = null;
        /// <summary>
        /// 科室和病区
        /// </summary>
        private void InitDept()
        {
            BindDeptData();
            BindWardData();
        }

        /// <summary>
        /// 操作人员
        /// </summary>
        private void InitEmployee()
        {
            BindEmployeeData();
        }

        /// <summary>
        /// 所有科室
        /// </summary>
        private void BindDeptData()
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            DataTable dataTable = GetEditroData(9);
            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueAdmitDept.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

            lueAdmitDept.SqlWordbook = sqlWordBook;
            lueAdmitDept.ListWindow = lupInfo;

            lueTransAdmitDept.SqlWordbook = sqlWordBook;
            lueTransAdmitDept.ListWindow = lupInfo;

            lueAdmitDeptAgain.SqlWordbook = sqlWordBook;
            lueAdmitDeptAgain.ListWindow = lupInfo;

            lueOutHosDept.SqlWordbook = sqlWordBook;
            lueOutHosDept.ListWindow = lupInfo;

        }

        private void BindWardData()
        {
            LookUpWindow lupInfo1 = new LookUpWindow();
            lupInfo1.SqlHelper = m_SqlHelper;
            if (m_DataTableWard == null)
                m_DataTableWard = GetEditroData(10);
            Dictionary<string, int> columnwidth1 = new Dictionary<String, Int32>();
            columnwidth1.Add("名称", lueAdmitWard.Width);
            SqlWordbook sqlWordBook1 = new SqlWordbook("ID", m_DataTableWard, "ID", "Name", columnwidth1, true);

            lueAdmitWard.SqlWordbook = sqlWordBook1;
            lueAdmitWard.ListWindow = lupInfo1;

            lueTransAdmitWard.SqlWordbook = sqlWordBook1;
            lueTransAdmitWard.ListWindow = lupInfo1;

            lueOutHosWard.SqlWordbook = sqlWordBook1;
            lueOutHosWard.ListWindow = lupInfo1;
        }


        /// <summary>
        /// 所有操作人员
        /// </summary>
        private void BindEmployeeData()
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            DataTable dataTable = GetEditroData(11);

            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueKszr.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

            lueKszr.SqlWordbook = sqlWordBook;
            lueKszr.ListWindow = lupInfo;

            lueZrys.SqlWordbook = sqlWordBook;
            lueZrys.ListWindow = lupInfo;

            lueZzys.SqlWordbook = sqlWordBook;
            lueZzys.ListWindow = lupInfo;

            lueZyys.SqlWordbook = sqlWordBook;
            lueZyys.ListWindow = lupInfo;

            lueJxys.SqlWordbook = sqlWordBook;
            lueJxys.ListWindow = lupInfo;

            lueYjs.SqlWordbook = sqlWordBook;
            lueYjs.ListWindow = lupInfo;

            lueSxys.SqlWordbook = sqlWordBook;
            lueSxys.ListWindow = lupInfo;

            lueBmy.SqlWordbook = sqlWordBook;
            lueBmy.ListWindow = lupInfo;

            lueZkys.SqlWordbook = sqlWordBook;
            lueZkys.ListWindow = lupInfo;

            lueZkhs.SqlWordbook = sqlWordBook;
            lueZkhs.ListWindow = lupInfo;

        }



        #endregion

        #region 绑定LUE
        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            DataTable dataTable = GetEditroData(queryType);

            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueInfo.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

            lueInfo.SqlWordbook = sqlWordBook;
            lueInfo.ListWindow = lupInfo;
        }

        /// <summary>
        /// 获取lue的数据源
        /// </summary>
        /// <param name="queryType"></param>
        /// <returns></returns>
        private DataTable GetEditroData(Decimal queryType)
        {
            SqlParameter paraType = new SqlParameter("@QueryType", SqlDbType.Decimal);
            paraType.Value = queryType;
            SqlParameter[] paramCollection = new SqlParameter[] { paraType };
            DataTable dataTable = AddTableColumn(m_SqlHelper.ExecuteDataTable("usp_GetLookUpEditorData", paramCollection, CommandType.StoredProcedure));
            return dataTable;
        }

        /// <summary>
        /// 给lue的数据源，新增 名称 栏位
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private DataTable AddTableColumn(DataTable dataTable)
        {
            DataTable dataTableAdd = dataTable;
            if (!dataTableAdd.Columns.Contains("名称"))
                dataTableAdd.Columns.Add("名称");
            foreach (DataRow row in dataTableAdd.Rows)
                row["名称"] = row["Name"].ToString();
            return dataTableAdd;
        }
        #endregion

        public void FillUI(IemMainPageInfo info, IYidanEmrHost app)
        {
            m_App = app;
            m_IemInfo = info;

            InitForm();

            //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
            FillUIInner();
        }

        delegate void FillUIDelegate();
        private void FillUIInner()
        {
            #region
            IemMainPageInfo info = m_IemInfo;
            IYidanEmrHost app = m_App;
            if (info.IemBasicInfo.Iem_Mainpage_NO == 0)
            {
                //to do 病患基本信息
                btnBasInfo_Click(null, null);
            }
            else
            {
                btnBasInfo_Click(null, null);

                luePayId.CodeValue = info.IemBasicInfo.PayID;
                txtSocialCare.Text = info.IemBasicInfo.SocialCare;
                txtPatNoOfHis.Text = info.IemBasicInfo.PatNoOfHis.ToString();
                seInCount.Value = info.IemBasicInfo.InCount;
                txtName.Text = info.IemBasicInfo.Name;
                lueSex.CodeValue = info.IemBasicInfo.SexID;
                if (!String.IsNullOrEmpty(info.IemBasicInfo.Birth))
                {
                    deBirth.DateTime = Convert.ToDateTime(info.IemBasicInfo.Birth);
                }
                txtAge.Text = app.CurrentPatientInfo.PersonalInformation.CurrentDisplayAge;
                lueMarital.CodeValue = info.IemBasicInfo.Marital;
                lueJob.CodeValue = info.IemBasicInfo.JobID;
                lueProvice.CodeValue = info.IemBasicInfo.ProvinceID;
                lueCounty.CodeValue = info.IemBasicInfo.CountyID;
                lueNation.CodeValue = info.IemBasicInfo.NationID;
                lueNationality.CodeValue = info.IemBasicInfo.NationalityID;
                txtIDNO.Text = info.IemBasicInfo.IDNO;
                txtOfficePlace.Text = info.IemBasicInfo.OfficePlace;
                txtOfficeTEL.Text = info.IemBasicInfo.OfficeTEL;
                txtOfficePost.Text = info.IemBasicInfo.OfficePost;
                txtNativeAddress.Text = info.IemBasicInfo.NativeAddress;
                txtNativeTEL.Text = info.IemBasicInfo.NativeTEL;
                txtNativePost.Text = info.IemBasicInfo.NativePost;
                txtContactPerson.Text = info.IemBasicInfo.ContactPerson;
                lueRelationship.CodeValue = info.IemBasicInfo.Relationship;
                txtContactAddress.Text = info.IemBasicInfo.ContactAddress;
                txtContactTEL.Text = info.IemBasicInfo.ContactTEL;
                if (!String.IsNullOrEmpty(info.IemBasicInfo.AdmitDate))
                {
                    deAdmitDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.AdmitDate);
                    teAdmitDate.Time = Convert.ToDateTime(info.IemBasicInfo.AdmitDate);
                }
                lueAdmitDept.CodeValue = info.IemBasicInfo.AdmitDept;
                lueAdmitWard.CodeValue = info.IemBasicInfo.AdmitWard;
                seDaysBefore.Value = Convertmy.ToDecimal(info.IemBasicInfo.Days_Before);
                if (!String.IsNullOrEmpty(info.IemBasicInfo.Trans_Date))
                {
                    deTransDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.Trans_Date);
                    teTransDate.Time = Convert.ToDateTime(info.IemBasicInfo.Trans_Date);
                }
                lueTransAdmitDept.CodeValue = info.IemBasicInfo.Trans_AdmitDept;
                lueTransAdmitWard.CodeValue = info.IemBasicInfo.Trans_AdmitWard;
                lueAdmitDeptAgain.CodeValue = info.IemBasicInfo.Trans_AdmitDept_Again;
                if (!String.IsNullOrEmpty(info.IemBasicInfo.OutWardDate))
                {
                    deOutWardDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.OutWardDate);
                    teOutWardDate.Time = Convert.ToDateTime(info.IemBasicInfo.OutWardDate);
                }
                lueOutHosDept.CodeValue = info.IemBasicInfo.OutHosDept;
                lueOutHosWard.CodeValue = info.IemBasicInfo.OutHosWard;
                seActualDays.Value = Convertmy.ToDecimal(info.IemBasicInfo.Actual_Days);
                if (!String.IsNullOrEmpty(info.IemBasicInfo.Death_Time))
                {
                    deDeathTime.DateTime = Convert.ToDateTime(info.IemBasicInfo.Death_Time);
                    teDeathTime.Time = Convert.ToDateTime(info.IemBasicInfo.Death_Time);
                }
                txtDeathReason.Text = info.IemBasicInfo.Death_Reason;
                lueKszr.CodeValue = info.IemBasicInfo.Section_Director;
                lueZrys.CodeValue = info.IemBasicInfo.Director;
                lueZzys.CodeValue = info.IemBasicInfo.Vs_Employee_Code;
                lueZyys.CodeValue = info.IemBasicInfo.Resident_Employee_Code;
                lueJxys.CodeValue = info.IemBasicInfo.Refresh_Employee_Code;
                lueYjs.CodeValue = info.IemBasicInfo.Master_Interne;
                lueSxys.CodeValue = info.IemBasicInfo.Interne;
                lueBmy.CodeValue = info.IemBasicInfo.Coding_User;
                //病案质量
                if (Convertmy.ToDecimal(info.IemBasicInfo.Medical_Quality_Id) == 1)
                    chkMedicalQuality1.Checked = true;
                if (Convertmy.ToDecimal(info.IemBasicInfo.Medical_Quality_Id) == 2)
                    chkMedicalQuality2.Checked = true;
                if (Convertmy.ToDecimal(info.IemBasicInfo.Medical_Quality_Id) == 3)
                    chkMedicalQuality3.Checked = true;
                lueZkys.CodeValue = info.IemBasicInfo.Quality_Control_Doctor;
                lueZkhs.CodeValue = info.IemBasicInfo.Quality_Control_Nurse;
                if (!String.IsNullOrEmpty(info.IemBasicInfo.Quality_Control_Date))
                {
                    deZkDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.Quality_Control_Date);
                    teZkDate.Time = Convert.ToDateTime(info.IemBasicInfo.Quality_Control_Date);
                }

            }
            #endregion
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            m_IemInfo.IemBasicInfo.PayID = luePayId.CodeValue;
            m_IemInfo.IemBasicInfo.SocialCare = txtSocialCare.Text;

            m_IemInfo.IemBasicInfo.PatNoOfHis = txtPatNoOfHis.Text;
            m_IemInfo.IemBasicInfo.InCount = Convert.ToInt32(seInCount.Value.ToString());
            m_IemInfo.IemBasicInfo.Name = txtName.Text;
            m_IemInfo.IemBasicInfo.SexID = lueSex.CodeValue;
            m_IemInfo.IemBasicInfo.Birth = deBirth.DateTime.ToShortDateString();
            //m_IemInfo.IemBasicInfo.a =txtAge.Text  ;
            m_IemInfo.IemBasicInfo.Marital = lueMarital.CodeValue;
            m_IemInfo.IemBasicInfo.JobID = lueJob.CodeValue;
            m_IemInfo.IemBasicInfo.ProvinceID = lueProvice.CodeValue;
            m_IemInfo.IemBasicInfo.CountyID = lueCounty.CodeValue;
            m_IemInfo.IemBasicInfo.NationID = lueNation.CodeValue;
            m_IemInfo.IemBasicInfo.NationalityID = lueNationality.CodeValue;
            m_IemInfo.IemBasicInfo.IDNO = txtIDNO.Text;
            m_IemInfo.IemBasicInfo.OfficePlace = txtOfficePlace.Text;
            m_IemInfo.IemBasicInfo.OfficeTEL = txtOfficeTEL.Text;
            m_IemInfo.IemBasicInfo.OfficePost = txtOfficePost.Text;
            m_IemInfo.IemBasicInfo.NativeAddress = txtNativeAddress.Text;
            m_IemInfo.IemBasicInfo.NativeTEL = txtNativeTEL.Text;
            m_IemInfo.IemBasicInfo.NativePost = txtNativePost.Text;
            m_IemInfo.IemBasicInfo.ContactPerson = txtContactPerson.Text;
            m_IemInfo.IemBasicInfo.Relationship = lueRelationship.CodeValue;
            m_IemInfo.IemBasicInfo.ContactAddress = txtContactAddress.Text;
            m_IemInfo.IemBasicInfo.ContactTEL = txtContactTEL.Text;

            if (!(deAdmitDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.AdmitDate = deAdmitDate.DateTime.ToString("yyyy-MM-dd") + " " + teAdmitDate.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemBasicInfo.AdmitDate = ;
            m_IemInfo.IemBasicInfo.AdmitDept = lueAdmitDept.CodeValue;
            m_IemInfo.IemBasicInfo.AdmitWard = lueAdmitWard.CodeValue;
            m_IemInfo.IemBasicInfo.Days_Before = seDaysBefore.Value;
            if (!(deTransDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.Trans_Date = deTransDate.DateTime.ToString("yyyy-MM-dd") + " " + teTransDate.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemBasicInfo.Trans_Date = teTransDate.Time.ToShortTimeString();
            m_IemInfo.IemBasicInfo.Trans_AdmitDept = lueTransAdmitDept.CodeValue;
            m_IemInfo.IemBasicInfo.Trans_AdmitWard = lueTransAdmitWard.CodeValue;
            m_IemInfo.IemBasicInfo.Trans_AdmitDept_Again = lueAdmitDeptAgain.CodeValue;
            if (!(deOutWardDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.OutWardDate = deOutWardDate.DateTime.ToString("yyyy-MM-dd") + " " + teOutWardDate.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemBasicInfo.OutWardDate = teOutWardDate.Time.ToShortTimeString();
            m_IemInfo.IemBasicInfo.OutHosDept = lueOutHosDept.CodeValue;
            m_IemInfo.IemBasicInfo.OutHosWard = lueOutHosWard.CodeValue;
            m_IemInfo.IemBasicInfo.Actual_Days = seActualDays.Value;
            if (!(deDeathTime.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.Death_Time = deDeathTime.DateTime.ToString("yyyy-MM-dd") + " " + teDeathTime.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemBasicInfo.Death_Time = teDeathTime.Time.ToShortTimeString();
            m_IemInfo.IemBasicInfo.Death_Reason = txtDeathReason.Text;
            m_IemInfo.IemBasicInfo.Section_Director = lueKszr.CodeValue;
            m_IemInfo.IemBasicInfo.Director = lueZrys.CodeValue;
            m_IemInfo.IemBasicInfo.Vs_Employee_Code = lueZzys.CodeValue;
            m_IemInfo.IemBasicInfo.Resident_Employee_Code = lueZyys.CodeValue;
            m_IemInfo.IemBasicInfo.Refresh_Employee_Code = lueJxys.CodeValue;
            m_IemInfo.IemBasicInfo.Master_Interne = lueYjs.CodeValue;
            m_IemInfo.IemBasicInfo.Interne = lueSxys.CodeValue;
            m_IemInfo.IemBasicInfo.Coding_User = lueBmy.CodeValue;
            //病案质量
            if (chkMedicalQuality1.Checked == true)
                m_IemInfo.IemBasicInfo.Medical_Quality_Id = 1;
            if (chkMedicalQuality2.Checked == true)
                m_IemInfo.IemBasicInfo.Medical_Quality_Id = 2;
            if (chkMedicalQuality3.Checked == true)
                m_IemInfo.IemBasicInfo.Medical_Quality_Id = 3;

            m_IemInfo.IemBasicInfo.Quality_Control_Doctor = lueZkys.CodeValue;
            m_IemInfo.IemBasicInfo.Quality_Control_Nurse = lueZkhs.CodeValue;
            if (!(deZkDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.Quality_Control_Date = deZkDate.DateTime.ToString("yyyy-MM-dd") + " " + teZkDate.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemBasicInfo.Quality_Control_Date = teZkDate.Time.ToShortTimeString();
        }

        #endregion

        #region private events
        private void lueProvice_CodeValueChanged(object sender, EventArgs e)
        {
            if (lueProvice.CodeValue != null)
            {
                lueCounty.CodeValue = null;
                DataTable dataTable = m_DataTableCountry.Clone();
                foreach (DataRow row in m_DataTableCountry.Rows)
                {
                    if (row["ParentID"].ToString() == lueProvice.CodeValue)
                        dataTable.ImportRow(row);
                }
                //dataTable.AcceptChanges();
                BindLueCountryData(lueCounty, dataTable);

            }
        }
        #endregion

        private void HideSbutton()
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl.GetType() == typeof(LookUpEditor))
                    ((LookUpEditor)ctl).ShowSButton = false;
                else
                {
                    foreach (Control ct in ctl.Controls)
                    {
                        if (ct.GetType() == typeof(LookUpEditor))
                            ((LookUpEditor)ct).ShowSButton = false;
                    }
                }
            }
        }

        /// <summary>
        /// 导入基本资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBasInfo_Click(object sender, EventArgs e)
        {
            if (m_App == null)
                return;
            if (m_App.CurrentPatientInfo == null)
                return;
            try
            {
                luePayId.CodeValue = m_App.CurrentPatientInfo.PaymentKind.Code;
                txtPatNoOfHis.Text = m_App.CurrentPatientInfo.NoOfHisFirstPage.ToString();
                seInCount.Value = m_App.CurrentPatientInfo.TimesOfAdmission;
                if (m_App.CurrentPatientInfo.PersonalInformation != null)
                {
                    txtSocialCare.Text = m_App.CurrentPatientInfo.PersonalInformation.SocialInsuranceNo;
                    txtAge.Text = m_App.CurrentPatientInfo.PersonalInformation.DisplayAge;
                    txtName.Text = m_App.CurrentPatientInfo.PersonalInformation.PatientName;
                    if (m_App.CurrentPatientInfo.PersonalInformation.Sex != null)
                        lueSex.CodeValue = m_App.CurrentPatientInfo.PersonalInformation.Sex.Code;
                    if (m_App.CurrentPatientInfo.PersonalInformation.Birthday.CompareTo(DateTime.MinValue) != 0)
                        deBirth.DateTime = m_App.CurrentPatientInfo.PersonalInformation.Birthday;
                    //txtAge.Text = m_App.CurrentPatientInfo.PersonalInformation.CurrentDisplayAge;
                    if (m_App.CurrentPatientInfo.PersonalInformation.MarriageCondition != null)
                        lueMarital.CodeValue = m_App.CurrentPatientInfo.PersonalInformation.MarriageCondition.Code;

                    if (m_App.CurrentPatientInfo.PersonalInformation.DepartmentOfWork != null)
                    {
                        lueJob.CodeValue = m_App.CurrentPatientInfo.PersonalInformation.DepartmentOfWork.Occupation.Code;
                        txtOfficePlace.Text = m_App.CurrentPatientInfo.PersonalInformation.DepartmentOfWork.CompanyName + m_App.CurrentPatientInfo.PersonalInformation.DepartmentOfWork.CompanyAddress;
                    }
                    if (txtOfficePlace.Text.Trim() == "[]")
                    {
                        txtOfficePlace.Text = "";
                    }

                    if (m_App.CurrentPatientInfo.PersonalInformation.DomiciliaryInfo != null)
                    {
                        lueProvice.CodeValue = m_App.CurrentPatientInfo.PersonalInformation.DomiciliaryInfo.Province.Code;
                        lueCounty.CodeValue = m_App.CurrentPatientInfo.PersonalInformation.DomiciliaryInfo.City.Code;

                        lueNationality.CodeValue = m_App.CurrentPatientInfo.PersonalInformation.DomiciliaryInfo.Country.Code;
                        txtNativeAddress.Text = m_App.CurrentPatientInfo.PersonalInformation.DomiciliaryInfo.FullAddress;
                        txtNativeTEL.Text = m_App.CurrentPatientInfo.PersonalInformation.DomiciliaryInfo.PhoneNo;
                        txtNativePost.Text = m_App.CurrentPatientInfo.PersonalInformation.DomiciliaryInfo.Postalcode;
                    }
                    if (m_App.CurrentPatientInfo.PersonalInformation.Nation != null)
                        lueNation.CodeValue = m_App.CurrentPatientInfo.PersonalInformation.Nation.Code;

                    if (m_App.CurrentPatientInfo.PersonalInformation.LinkManInfo != null)
                    {
                        txtContactPerson.Text = m_App.CurrentPatientInfo.PersonalInformation.LinkManInfo.Name;
                        lueRelationship.CodeValue = m_App.CurrentPatientInfo.PersonalInformation.LinkManInfo.Relation.Code;
                        txtContactAddress.Text = m_App.CurrentPatientInfo.PersonalInformation.LinkManInfo.ContactAddress.FullAddress;
                        txtContactTEL.Text = m_App.CurrentPatientInfo.PersonalInformation.LinkManInfo.ContactAddress.PhoneNo;
                    }
                }
                if (m_App.CurrentPatientInfo.Recorder != null)
                    txtIDNO.Text = m_App.CurrentPatientInfo.Recorder.IdentityNo;


                if (m_App.CurrentPatientInfo.InfoOfAdmission != null)
                {
                    if (m_App.CurrentPatientInfo.InfoOfAdmission.AdmitInfo.StepOneDate.CompareTo(DateTime.MinValue) != 0)
                    {
                        deAdmitDate.DateTime = m_App.CurrentPatientInfo.InfoOfAdmission.AdmitInfo.StepOneDate;
                        teAdmitDate.Time = m_App.CurrentPatientInfo.InfoOfAdmission.AdmitInfo.StepOneDate;
                    }
                    lueAdmitDept.CodeValue = m_App.CurrentPatientInfo.InfoOfAdmission.AdmitInfo.CurrentDepartment.Code;
                    lueAdmitWard.CodeValue = m_App.CurrentPatientInfo.InfoOfAdmission.AdmitInfo.CurrentWard.Code;

                    lueZzys.CodeValue = m_App.CurrentPatientInfo.InfoOfAdmission.AttendingPhysician.Code;//主治
                    lueZyys.CodeValue = m_App.CurrentPatientInfo.InfoOfAdmission.Resident.Code;//住院
                    lueZrys.CodeValue = m_App.CurrentPatientInfo.InfoOfAdmission.Director.Code;//主任
                }
            }
            catch (Exception ex)
            {
                // to do nothing 
            }

        }

        /// <summary>
        /// 动态在空间下方画横线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCIemBasInfo_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextEdit)
                {
                    if (control.Visible == true)
                        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                            new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                }
            }

            e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
            e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(this.Width, 0));
            e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
        }

        private void InitForm()
        {
            //设置医院名称和位置
            if (m_App != null)
            {
                labelHospitalName.Text = m_DataHelper.GetHospitalName();
                labelHospitalName.Location = new Point((this.Width - TextRenderer.MeasureText(labelHospitalName.Text, labelHospitalName.Font).Width) / 2, labelHospitalName.Location.Y);
            }
        }
        #region 病案首页基本信息
        /// <summary>
        /// 病案首页基本信息
        /// </summary>
        /// <returns></returns>
        public Print_Iem_Mainpage_Basicinfo GetPrintBasicInfo()
        {
            Print_Iem_Mainpage_Basicinfo _Print_BasicInfo = new Print_Iem_Mainpage_Basicinfo();
            _Print_BasicInfo.HospitalName = labelHospitalName.Text;
            _Print_BasicInfo.PayName = luePayId.Text;
            _Print_BasicInfo.InCount = seInCount.Value.ToString();
            _Print_BasicInfo.PatNoOfHis = txtPatNoOfHis.Text;
            _Print_BasicInfo.Name = txtName.Text;

            _Print_BasicInfo.SexID = lueSex.CodeValue;
            _Print_BasicInfo.Birth = deBirth.DateTime.ToShortDateString();
            _Print_BasicInfo.Age = txtAge.Text;
            _Print_BasicInfo.Marital = lueMarital.CodeValue;
            _Print_BasicInfo.JobName = lueJob.Text;
            
            _Print_BasicInfo.CountyName = lueCounty.Text;
            _Print_BasicInfo.NationName = lueNation.Text;
            _Print_BasicInfo.NationalityName = lueNationality.Text;
            _Print_BasicInfo.IDNO = txtIDNO.Text;
            _Print_BasicInfo.OfficePlace = txtOfficePlace.Text;

            _Print_BasicInfo.OfficeTEL = txtOfficeTEL.Text;
            _Print_BasicInfo.OfficePost = txtOfficePost.Text;
            _Print_BasicInfo.NativeAddress = txtNativeAddress.Text;
            _Print_BasicInfo.NativeTEL = txtNativeTEL.Text;
            _Print_BasicInfo.NativePost = txtNativePost.Text;

            _Print_BasicInfo.ContactPerson = txtContactPerson.Text;
            _Print_BasicInfo.Relationship = lueRelationship.Text;
            _Print_BasicInfo.ContactAddress = txtContactAddress.Text;
            _Print_BasicInfo.ContactTEL = txtContactTEL.Text;
            _Print_BasicInfo.AdmitDate = deAdmitDate.DateTime.ToString("yyyy-MM-dd") + " " + teAdmitDate.Time.ToString("HH:mm:ss");
 
            _Print_BasicInfo.AdmitDeptName = lueAdmitDept.Text;
            _Print_BasicInfo.AdmitWardName = lueAdmitWard.Text;
            _Print_BasicInfo.Trans_AdmitDept = lueTransAdmitDept.Text;
            _Print_BasicInfo.OutWardDate = deOutWardDate.DateTime.ToString("yyyy-MM-dd") + " " + teOutWardDate.Time.ToString("HH:mm:ss");
            _Print_BasicInfo.OutHosDeptName = lueOutHosDept.Text;
            
            _Print_BasicInfo.OutHosWardName = lueOutHosWard.Text;
            _Print_BasicInfo.ActualDays = seActualDays.Value.ToString();

            return _Print_BasicInfo;
        }

        public Print_Iem_Mainpage_Diagnosis GetPrintDiagnosis(Print_Iem_Mainpage_Diagnosis _Print_Iem_Mainpage_Diagnosis)
        {

            _Print_Iem_Mainpage_Diagnosis.Section_Director = lueKszr.Text;
            _Print_Iem_Mainpage_Diagnosis.Director = lueZrys.Text;
            _Print_Iem_Mainpage_Diagnosis.Vs_Employee_Code = lueZzys.Text;
            _Print_Iem_Mainpage_Diagnosis.Resident_Employee_Code = lueZyys.Text;
            _Print_Iem_Mainpage_Diagnosis.Refresh_Employee_Code = lueJxys.Text;


            _Print_Iem_Mainpage_Diagnosis.Master_Interne = lueYjs.Text;
            _Print_Iem_Mainpage_Diagnosis.Interne = lueSxys.Text;
            _Print_Iem_Mainpage_Diagnosis.Coding_User = lueBmy.Text;
            _Print_Iem_Mainpage_Diagnosis.Medical_Quality_Id = m_IemInfo.IemBasicInfo.Medical_Quality_Id.ToString();
            _Print_Iem_Mainpage_Diagnosis.Quality_Control_Doctor = lueZkys.Text;

            _Print_Iem_Mainpage_Diagnosis.Quality_Control_Nurse = lueZkhs.Text;
            _Print_Iem_Mainpage_Diagnosis.Quality_Control_Date = m_IemInfo.IemBasicInfo.Quality_Control_Date;

            return _Print_Iem_Mainpage_Diagnosis;
        }

        #endregion

    }
}
