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
    public partial class Print_UCIemBasInfo : UserControl
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
                return m_IemInfo;
            }
        }

        public Print_UCIemBasInfo(IemMainPageInfo info, IYidanEmrHost app)
        {
            InitializeComponent();
            //m_IemInfo = info;
            //m_App = app;

            //InitLookUpEditor();
        }

        private void Print_UCIemBasInfo_Load(object sender, EventArgs e)
        {
            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();

            //lueProvice.Focus();

            //HideSbutton();
            //FillUI();
        }

        #region private methods

        /// <summary>
        /// 初始化lookupeditor
        /// </summary>
        private void InitLookUpEditor()
        {
            InitLuePayId();
            //InitLueSex();
            //InitMarital();
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
            //BindLueData(lueProvice, 5);
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

            //lueAdmitDeptAgain.SqlWordbook = sqlWordBook;
            //lueAdmitDeptAgain.ListWindow = lupInfo;

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

            //lueTransAdmitWard.SqlWordbook = sqlWordBook1;
            //lueTransAdmitWard.ListWindow = lupInfo1;

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

        public void FillUI()
        {
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
 
            }
            else
            { 
                luePayId.CodeValue = info.IemBasicInfo.PayID;
                txtSocialCare.Text = info.IemBasicInfo.SocialCare;
                txtPatNoOfHis.Text = info.IemBasicInfo.PatNoOfHis.ToString();
                seInCount.Text = info.IemBasicInfo.InCount.ToString();
                txtName.Text = info.IemBasicInfo.Name;
                lueSex.Text = info.IemBasicInfo.SexID;
                if (!String.IsNullOrEmpty(info.IemBasicInfo.Birth))
                {
                    deBirth.Text = Convert.ToDateTime(info.IemBasicInfo.Birth).ToString("yyyy年MM月dd日");
                }
                txtAge.Text = app.CurrentPatientInfo.PersonalInformation.CurrentDisplayAge;
                lueMarital.Text = info.IemBasicInfo.Marital;
                lueJob.CodeValue = info.IemBasicInfo.JobID;
                //lueProvice.CodeValue = info.IemBasicInfo.ProvinceID;
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
                    txtAdmitWardDate.Text = Convert.ToDateTime(info.IemBasicInfo.AdmitDate).ToString("yyyy年MM月dd日 HH时");
                    //deAdmitDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.AdmitDate);
                    //teAdmitDate.Time = Convert.ToDateTime(info.IemBasicInfo.AdmitDate);
                }
                lueAdmitDept.CodeValue = info.IemBasicInfo.AdmitDept;
                lueAdmitWard.CodeValue = info.IemBasicInfo.AdmitWard;
                //seDaysBefore.Value = Convertmy.ToDecimal(info.IemBasicInfo.Days_Before);
                //if (!String.IsNullOrEmpty(info.IemBasicInfo.Trans_Date))
                //{
                //    deTransDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.Trans_Date);
                //    teTransDate.Time = Convert.ToDateTime(info.IemBasicInfo.Trans_Date);
                //}
                lueTransAdmitDept.CodeValue = info.IemBasicInfo.Trans_AdmitDept;
                //lueTransAdmitWard.CodeValue = info.IemBasicInfo.Trans_AdmitWard;
                //lueAdmitDeptAgain.CodeValue = info.IemBasicInfo.Trans_AdmitDept_Again;
                if (!String.IsNullOrEmpty(info.IemBasicInfo.OutWardDate))
                {
                    txtOutWardDate.Text = Convert.ToDateTime(info.IemBasicInfo.OutWardDate).ToString("yyyy年MM月dd日 HH时");
                    //deOutWardDate.DateTime = Convert.ToDateTime(info.IemBasicInfo.OutWardDate);
                    //teOutWardDate.Time = Convert.ToDateTime(info.IemBasicInfo.OutWardDate);
                }
                lueOutHosDept.CodeValue = info.IemBasicInfo.OutHosDept;
                lueOutHosWard.CodeValue = info.IemBasicInfo.OutHosWard;
                seActualDays.Text = info.IemBasicInfo.Actual_Days.ToString();
                 

            }
            #endregion
        }
 

        #endregion

        #region private events
        private void lueProvice_CodeValueChanged(object sender, EventArgs e)
        {
            //if (lueProvice.CodeValue != null)
            //{
            //    lueCounty.CodeValue = null;
            //    DataTable dataTable = m_DataTableCountry.Clone();
            //    foreach (DataRow row in m_DataTableCountry.Rows)
            //    {
            //        if (row["ParentID"].ToString() == lueProvice.CodeValue)
            //            dataTable.ImportRow(row);
            //    }
            //    //dataTable.AcceptChanges();
            //    BindLueCountryData(lueCounty, dataTable);

            //}
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
                if (control is TextEdit)
                {
                    if(control.Visible == true)
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
    }
}
