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
            //if (info.IemBasicInfo.Iem_Mainpage_NO == "")
            //{
            //    //to do 病患基本信息
            //    btnBasInfo_Click(null, null);
            //}
            //else
            //{
                //btnBasInfo_Click(null, null);

                luePayId.CodeValue = info.IemBasicInfo.PayID;
                txtSocialCare.Text = info.IemBasicInfo.SocialCare;
                txtPatNoOfHis.Text = info.IemBasicInfo.PatNoOfHis.ToString();
                seInCount.Value = info.IemBasicInfo.InCount ==""?0:Convert.ToInt32(info.IemBasicInfo.InCount);
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

                lueRelationship.CodeValue = info.IemBasicInfo.RelationshipID;
                txtContactAddress.Text = info.IemBasicInfo.ContactAddress;
                txtContactTEL.Text = info.IemBasicInfo.ContactTEL;
                if (!String.IsNullOrEmpty(info.IemBasicInfo.AdmitDate))
                {
                    deAdmitDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.AdmitDate);
                    teAdmitDate.Time = Convert.ToDateTime(info.IemBasicInfo.AdmitDate);
                }

                lueAdmitDept.CodeValue = info.IemBasicInfo.AdmitDeptID;
                lueAdmitWard.CodeValue = info.IemBasicInfo.AdmitWardID;
                seDaysBefore.Value = Convertmy.ToDecimal(info.IemBasicInfo.Days_Before);
                if (!String.IsNullOrEmpty(info.IemBasicInfo.Trans_Date))
                {
                    deTransDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.Trans_Date);
                    teTransDate.Time = Convert.ToDateTime(info.IemBasicInfo.Trans_Date);
                }
                lueTransAdmitDept.CodeValue = info.IemBasicInfo.Trans_AdmitDeptID;
                //lueTransAdmitWard.CodeValue = info.IemBasicInfo.Trans_AdmitWardID;
                lueAdmitDeptAgain.CodeValue = info.IemBasicInfo.Trans_AdmitDept_Again;
                if (!String.IsNullOrEmpty(info.IemBasicInfo.OutWardDate))
                {
                    deOutWardDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.OutWardDate);
                    teOutWardDate.Time = Convert.ToDateTime(info.IemBasicInfo.OutWardDate);
                }
                lueOutHosDept.CodeValue = info.IemBasicInfo.OutHosDeptID;
                lueOutHosWard.CodeValue = info.IemBasicInfo.OutHosWardID;
                seActualDays.Value = Convertmy.ToDecimal(info.IemBasicInfo.ActualDays);
                if (!String.IsNullOrEmpty(info.IemBasicInfo.Death_Time))
                {
                    deDeathTime.DateTime = Convert.ToDateTime(info.IemBasicInfo.Death_Time);
                    teDeathTime.Time = Convert.ToDateTime(info.IemBasicInfo.Death_Time);
                }
                txtDeathReason.Text = info.IemBasicInfo.Death_Reason;
                

            //}
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
            m_IemInfo.IemBasicInfo.InCount = seInCount.Value.ToString();
            m_IemInfo.IemBasicInfo.Name = txtName.Text;
            m_IemInfo.IemBasicInfo.SexID = lueSex.CodeValue;
            m_IemInfo.IemBasicInfo.Birth = deBirth.DateTime.ToShortDateString();
            //m_IemInfo.IemBasicInfo.a =txtAge.Text  ;
            m_IemInfo.IemBasicInfo.Marital = lueMarital.CodeValue;
            m_IemInfo.IemBasicInfo.JobID = lueJob.CodeValue;
            m_IemInfo.IemBasicInfo.JobName = lueJob.Text;
            m_IemInfo.IemBasicInfo.ProvinceID = lueProvice.CodeValue;
            m_IemInfo.IemBasicInfo.ProvinceName = lueProvice.Text;
            m_IemInfo.IemBasicInfo.CountyID = lueCounty.CodeValue;
            m_IemInfo.IemBasicInfo.CountyName = lueCounty.Text;
            m_IemInfo.IemBasicInfo.NationID = lueNation.CodeValue;
            m_IemInfo.IemBasicInfo.NationName = lueNation.Text;
            m_IemInfo.IemBasicInfo.NationalityID = lueNationality.CodeValue;
            m_IemInfo.IemBasicInfo.NationalityName = lueNationality.Text;
            m_IemInfo.IemBasicInfo.IDNO = txtIDNO.Text;
            m_IemInfo.IemBasicInfo.OfficePlace = txtOfficePlace.Text;
            m_IemInfo.IemBasicInfo.OfficeTEL = txtOfficeTEL.Text;
            m_IemInfo.IemBasicInfo.OfficePost = txtOfficePost.Text;
            m_IemInfo.IemBasicInfo.NativeAddress = txtNativeAddress.Text;
            m_IemInfo.IemBasicInfo.NativeTEL = txtNativeTEL.Text;
            m_IemInfo.IemBasicInfo.NativePost = txtNativePost.Text;
            m_IemInfo.IemBasicInfo.ContactPerson = txtContactPerson.Text;
            m_IemInfo.IemBasicInfo.RelationshipID = lueRelationship.CodeValue;
            m_IemInfo.IemBasicInfo.RelationshipName = lueRelationship.Text;
            m_IemInfo.IemBasicInfo.ContactAddress = txtContactAddress.Text;
            m_IemInfo.IemBasicInfo.ContactTEL = txtContactTEL.Text;

            if (!(deAdmitDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.AdmitDate = deAdmitDate.DateTime.ToString("yyyy-MM-dd") + " " + teAdmitDate.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemBasicInfo.AdmitDate = ;
            m_IemInfo.IemBasicInfo.AdmitDeptID = lueAdmitDept.CodeValue;
            m_IemInfo.IemBasicInfo.AdmitDeptName = lueAdmitDept.Text;
            m_IemInfo.IemBasicInfo.AdmitWardID = lueAdmitWard.CodeValue;
            m_IemInfo.IemBasicInfo.AdmitWardName = lueAdmitWard.Text;
            m_IemInfo.IemBasicInfo.Days_Before = seDaysBefore.Value.ToString();
            if (!(deTransDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.Trans_Date = deTransDate.DateTime.ToString("yyyy-MM-dd") + " " + teTransDate.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemBasicInfo.Trans_Date = teTransDate.Time.ToShortTimeString();
            m_IemInfo.IemBasicInfo.Trans_AdmitDeptID = lueTransAdmitDept.CodeValue;
            m_IemInfo.IemBasicInfo.Trans_AdmitDeptName = lueTransAdmitDept.Text;
            m_IemInfo.IemBasicInfo.Trans_AdmitWard = lueTransAdmitWard.CodeValue;
            m_IemInfo.IemBasicInfo.Trans_AdmitDept_Again = lueAdmitDeptAgain.CodeValue;
            if (!(deOutWardDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.OutWardDate = deOutWardDate.DateTime.ToString("yyyy-MM-dd") + " " + teOutWardDate.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemBasicInfo.OutWardDate = teOutWardDate.Time.ToShortTimeString();
            m_IemInfo.IemBasicInfo.OutHosDeptID = lueOutHosDept.CodeValue;
            m_IemInfo.IemBasicInfo.OutHosDeptName = lueOutHosDept.Text;
            m_IemInfo.IemBasicInfo.OutHosWardID = lueOutHosWard.CodeValue;
            m_IemInfo.IemBasicInfo.OutHosWardName = lueOutHosWard.Text;
            m_IemInfo.IemBasicInfo.ActualDays = seActualDays.Value.ToString();
            if (!(deDeathTime.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemBasicInfo.Death_Time = deDeathTime.DateTime.ToString("yyyy-MM-dd") + " " + teDeathTime.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemBasicInfo.Death_Time = teDeathTime.Time.ToShortTimeString();
            m_IemInfo.IemBasicInfo.Death_Reason = txtDeathReason.Text;

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
        /// 动态在空间下方画横线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCIemBasInfo_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is LabelControl)
                {
                    //if (control.Visible == true)
                    //{
                        //e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                        //    new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                        control.Visible = false;
                        e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);
                        
                    //}
                }
                if (control is TextEdit)
                {
                    if (control.Visible == true)
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                            new Point(control.Width + control.Location.X, control.Height + control.Location.Y));


                    }
                }
            }

            //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
            //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(this.Width, 0));
            //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
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
        public Iem_Mainpage_Basicinfo GetPrintBasicInfo()
        {
            Iem_Mainpage_Basicinfo _BasicInfo = new Iem_Mainpage_Basicinfo();
            //_BasicInfo.HospitalName = labelHospitalName.Text;
            //_BasicInfo.PayName = luePayId.Text;
            //_BasicInfo.InCount = seInCount.Value.ToString();
            //_BasicInfo.PatNoOfHis = txtPatNoOfHis.Text;
            //_BasicInfo.Name = txtName.Text;

            //_BasicInfo.SexID = lueSex.CodeValue;
            //_BasicInfo.Birth = deBirth.DateTime.ToShortDateString();
            //_BasicInfo.Age = txtAge.Text;
            //_BasicInfo.Marital = lueMarital.CodeValue;
            //_BasicInfo.JobName = lueJob.Text;
            
            //_BasicInfo.CountyName = lueCounty.Text;
            //_BasicInfo.NationName = lueNation.Text;
            //_BasicInfo.NationalityName = lueNationality.Text;
            //_BasicInfo.IDNO = txtIDNO.Text;
            //_BasicInfo.OfficePlace = txtOfficePlace.Text;

            //_BasicInfo.OfficeTEL = txtOfficeTEL.Text;
            //_BasicInfo.OfficePost = txtOfficePost.Text;
            //_BasicInfo.NativeAddress = txtNativeAddress.Text;
            //_BasicInfo.NativeTEL = txtNativeTEL.Text;
            //_BasicInfo.NativePost = txtNativePost.Text;

            //_BasicInfo.ContactPerson = txtContactPerson.Text;
            //_BasicInfo.Relationship = lueRelationship.Text;
            //_BasicInfo.ContactAddress = txtContactAddress.Text;
            //_BasicInfo.ContactTEL = txtContactTEL.Text;
            //_BasicInfo.AdmitDate = deAdmitDate.DateTime.ToString("yyyy-MM-dd") + " " + teAdmitDate.Time.ToString("HH:mm:ss");
 
            //_BasicInfo.AdmitDeptName = lueAdmitDept.Text;
            //_BasicInfo.AdmitWardName = lueAdmitWard.Text;
            //_BasicInfo.Trans_AdmitDept = lueTransAdmitDept.Text;
            //_BasicInfo.OutWardDate = deOutWardDate.DateTime.ToString("yyyy-MM-dd") + " " + teOutWardDate.Time.ToString("HH:mm:ss");
            //_BasicInfo.OutHosDeptName = lueOutHosDept.Text;
            
            //_BasicInfo.OutHosWardName = lueOutHosWard.Text;
            //_BasicInfo.ActualDays = seActualDays.Value.ToString();

            return _BasicInfo;
        }

        public Iem_Mainpage_Diagnosis GetPrintDiagnosis(Iem_Mainpage_Diagnosis _Iem_Mainpage_Diagnosis)
        {

            //_Iem_Mainpage_Diagnosis.Section_Director = lueKszr.Text;
            //_Iem_Mainpage_Diagnosis.Director = lueZrys.Text;
            //_Iem_Mainpage_Diagnosis.Vs_Employee_Code = lueZzys.Text;
            //_Iem_Mainpage_Diagnosis.Resident_Employee_Code = lueZyys.Text;
            //_Iem_Mainpage_Diagnosis.Refresh_Employee_Code = lueJxys.Text;


            //_Iem_Mainpage_Diagnosis.Master_Interne = lueYjs.Text;
            //_Iem_Mainpage_Diagnosis.Interne = lueSxys.Text;
            //_Iem_Mainpage_Diagnosis.Coding_User = lueBmy.Text;
            //_Iem_Mainpage_Diagnosis.Medical_Quality_Id = m_IemInfo.IemBasicInfo.Medical_Quality_Id.ToString();
            //_Iem_Mainpage_Diagnosis.Quality_Control_Doctor = lueZkys.Text;

            //_Iem_Mainpage_Diagnosis.Quality_Control_Nurse = lueZkhs.Text;
            //_Iem_Mainpage_Diagnosis.Quality_Control_Date = m_IemInfo.IemBasicInfo.Quality_Control_Date;

            return _Iem_Mainpage_Diagnosis;
        }

        #endregion

        private void btn_OK_Click(object sender, EventArgs e)
        {
            GetUI();
            ((ShowUC)this.Parent).Close(true,m_IemInfo);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
        }

    }
}
