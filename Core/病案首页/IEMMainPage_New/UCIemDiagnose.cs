using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YidanSoft.Common.Library;
using YidanSoft.Wordbook;
using System.Data.SqlClient;

using YidanSoft.FrameWork.WinForm.Plugin;

using Convertmy = YidanSoft.Core.UtilsForExtension;
using DevExpress.XtraEditors;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class UCIemDiagnose : UserControl
    {
        IDataAccess m_SqlHelper;
        IYidanSoftLog m_Logger;

        private IemMainPageInfo m_IemInfo;
        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get
            {
                m_IemInfo = new IemMainPageInfo();
                GetUI();
                return m_IemInfo;
            }
        }

        private IYidanEmrHost m_App;

        private DataTable m_DataTableDiag = null;
        public UCIemDiagnose()
        {
            InitializeComponent();

            m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            InitLookUpEditor();
        }

        private void UCIemDiagnose_Load(object sender, EventArgs e)
        {
            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();
#if DEBUG
#else
            //HideSbutton();
#endif
        }

        #region 初始化下拉框
        private void InitLookUpEditor()
        {
            InitLueDiagnose();
            BindEmployeeData();
        }

        private void InitLueDiagnose()
        {
            BindLueData(lueInDiag, 12);
            BindLueData(lueOutDiag, 12);
            BindLueData(lueZymosisName, 12);
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

        #region private methods

        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            if (m_DataTableDiag == null)
                m_DataTableDiag = GetEditroData(queryType);

            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueInfo.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", m_DataTableDiag, "ID", "Name", columnwidth, true);

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

        private IemNewDiagInfoForm m_DiagInfoForm = null;

        public void FillUI(IemMainPageInfo info, IYidanEmrHost app)
        {
            m_App = app;
            m_IemInfo = info;

            //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
            FillUIInner();
        }

        delegate void FillUIDelegate();
        private void FillUIInner()
        {
            #region
            //if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
            //{
            //    //to do 病患基本信息
            //}
            //else
            //{
            //出院诊断
            //DataTable dataTableOper = new DataTable();
            //foreach (Iem_Mainpage_Diagnosis im in m_IemInfo.IemDiagInfo)
            //{
            //    if (m_DiagInfoForm == null)
            //        m_DiagInfoForm = new IemNewDiagInfoForm(m_App);
            //    if (im.Diagnosis_Type_Id == 7 || im.Diagnosis_Type_Id == 8)
            //    {
            //        m_DiagInfoForm.IemOperInfo = im;
            //        DataTable dataTable = m_DiagInfoForm.DataOper;
            //        if (dataTableOper.Rows.Count == 0)
            //            dataTableOper = dataTable.Clone();
            //        foreach (DataRow row in dataTable.Rows)
            //        {
            //            dataTableOper.ImportRow(row);
            //        }
            //        //dataTableOper.AcceptChanges();
            //    }

            //}
            DataTable dataTableOper = m_IemInfo.IemDiagInfo.OutDiagTable;
            this.gridControl1.DataSource = null;
            this.gridControl1.BeginUpdate();
            if (dataTableOper.Select("Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8'").Length != 0)
                this.gridControl1.DataSource = dataTableOper.Select("Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8'").CopyToDataTable();
            this.gridControl1.EndUpdate();

            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);

            //入院状态
            if (m_IemInfo.IemDiagInfo.AdmitInfo == "1")
                chkAdmitInfo1.Checked = true;
            if (m_IemInfo.IemDiagInfo.AdmitInfo == "2")
                chkAdmitInfo2.Checked = true;
            if (m_IemInfo.IemDiagInfo.AdmitInfo == "3")
                chkAdmitInfo3.Checked = true;
            txtPathologyName.Text = m_IemInfo.IemDiagInfo.Pathology_Diagnosis_Name;
            //txtPathologyObservationSn.Text = m_IemInfo.IemDiagInfo.Pathology_Observation_Sn;

            //txtAshesDiagnosisName.Text = m_IemInfo.IemDiagInfo.Ashes_Diagnosis_Name;
            //txtAshesAnatomiseSn.Text = m_IemInfo.IemDiagInfo.Ashes_Anatomise_Sn;

            txtAllergicDrug.Text = m_IemInfo.IemDiagInfo.Allergic_Drug;

            if (m_IemInfo.IemDiagInfo.Hbsag == "0")
                chkHBsAg1.Checked = true;
            if (m_IemInfo.IemDiagInfo.Hbsag == "1")
                chkHBsAg2.Checked = true;
            if (m_IemInfo.IemDiagInfo.Hbsag == "2")
                chkHBsAg3.Checked = true;

            if (m_IemInfo.IemDiagInfo.Hcv_Ab == "0")
                chkHCV1.Checked = true;
            if (m_IemInfo.IemDiagInfo.Hcv_Ab == "1")
                chkHCV2.Checked = true;
            if (m_IemInfo.IemDiagInfo.Hcv_Ab == "2")
                chkHCV3.Checked = true;



            if (m_IemInfo.IemDiagInfo.Hiv_Ab == "0")
                chkHIV1.Checked = true;
            if (m_IemInfo.IemDiagInfo.Hiv_Ab == "1")
                chkHIV2.Checked = true;
            if (m_IemInfo.IemDiagInfo.Hiv_Ab == "2")
                chkHIV3.Checked = true;



            if (m_IemInfo.IemDiagInfo.Opd_Ipd_Id == "0")
                chkOpdIpd1.Checked = true;
            if (m_IemInfo.IemDiagInfo.Opd_Ipd_Id == "1")
                chkOpdIpd2.Checked = true;
            if (m_IemInfo.IemDiagInfo.Opd_Ipd_Id == "2")
                chkOpdIpd3.Checked = true;
            if (m_IemInfo.IemDiagInfo.Opd_Ipd_Id == "3")
                chkOpdIpd4.Checked = true;




            if (m_IemInfo.IemDiagInfo.In_Out_Inpatinet_Id == "0")
                chkInOut1.Checked = true;
            if (m_IemInfo.IemDiagInfo.In_Out_Inpatinet_Id == "1")
                chkInOut2.Checked = true;
            if (m_IemInfo.IemDiagInfo.In_Out_Inpatinet_Id == "2")
                chkInOut3.Checked = true;
            if (m_IemInfo.IemDiagInfo.In_Out_Inpatinet_Id == "3")
                chkInOut4.Checked = true;


            if (m_IemInfo.IemDiagInfo.Before_After_Or_Id == "0")
                chkBeforeAfter1.Checked = true;
            if (m_IemInfo.IemDiagInfo.Before_After_Or_Id == "1")
                chkBeforeAfter2.Checked = true;
            if (m_IemInfo.IemDiagInfo.Before_After_Or_Id == "2")
                chkBeforeAfter3.Checked = true;
            if (m_IemInfo.IemDiagInfo.Before_After_Or_Id == "3")
                chkBeforeAfter4.Checked = true;


            if (m_IemInfo.IemDiagInfo.Clinical_Pathology_Id == "0")
                chkClinical1.Checked = true;
            if (m_IemInfo.IemDiagInfo.Clinical_Pathology_Id == "1")
                chkClinical2.Checked = true;
            if (m_IemInfo.IemDiagInfo.Clinical_Pathology_Id == "2")
                chkClinical3.Checked = true;
            if (m_IemInfo.IemDiagInfo.Clinical_Pathology_Id == "3")
                chkClinical4.Checked = true;


            if (m_IemInfo.IemDiagInfo.Pacs_Pathology_Id == "0")
                chkPacsPathology1.Checked = true;
            if (m_IemInfo.IemDiagInfo.Pacs_Pathology_Id == "1")
                chkPacsPathology2.Checked = true;
            if (m_IemInfo.IemDiagInfo.Pacs_Pathology_Id == "2")
                chkPacsPathology3.Checked = true;
            if (m_IemInfo.IemDiagInfo.Pacs_Pathology_Id == "3")
                chkPacsPathology4.Checked = true;

            seSaveTimes.Value = Convertmy.ToDecimal(m_IemInfo.IemDiagInfo.Save_Times);
            seSuccessTimes.Value = Convertmy.ToDecimal(m_IemInfo.IemDiagInfo.Success_Times);


            //入院后确诊日期
            if (!String.IsNullOrEmpty(m_IemInfo.IemDiagInfo.In_Check_Date))
            {
                deInCheckDate.DateTime = Convert.ToDateTime(m_IemInfo.IemDiagInfo.In_Check_Date);
                teInCheckDate.Time = Convert.ToDateTime(m_IemInfo.IemDiagInfo.In_Check_Date);
            }

            foreach (DataRow im in dataTableOper.Rows)
            {
                if (im["Diagnosis_Type_Id"].ToString() == "13")
                    this.lueOutDiag.CodeValue = im["Diagnosis_Code"].ToString() == "" ? "" : im["Diagnosis_Code"].ToString();
                else if (im["Diagnosis_Type_Id"].ToString() == "2")
                    this.lueInDiag.CodeValue = im["Diagnosis_Code"].ToString() == "" ? "" : im["Diagnosis_Code"].ToString();
            }

            lueZymosisName.CodeValue = m_IemInfo.IemDiagInfo.ZymosisID;
            txtHurt_Toxicosis_Ele.Text = m_IemInfo.IemDiagInfo.Hurt_Toxicosis_Element;
            if (m_IemInfo.IemDiagInfo.ZymosisState != "")
                cmbZymosisState.SelectedIndex = Convert.ToInt32(m_IemInfo.IemDiagInfo.ZymosisState) - 1;


            lueKszr.CodeValue = m_IemInfo.IemDiagInfo.Section_DirectorID;
            lueZrys.CodeValue = m_IemInfo.IemDiagInfo.DirectorID;
            lueZzys.CodeValue = m_IemInfo.IemDiagInfo.Vs_EmployeeID;
            lueZyys.CodeValue = m_IemInfo.IemDiagInfo.Resident_EmployeeID;
            lueJxys.CodeValue = m_IemInfo.IemDiagInfo.Refresh_EmployeeID;
            lueYjs.CodeValue = m_IemInfo.IemDiagInfo.Master_InterneID;
            lueSxys.CodeValue = m_IemInfo.IemDiagInfo.InterneID;
            lueBmy.CodeValue = m_IemInfo.IemDiagInfo.Coding_UserID;
            //病案质量
            if (Convertmy.ToDecimal(m_IemInfo.IemDiagInfo.Medical_Quality_Id) == 1)
                chkMedicalQuality1.Checked = true;
            if (Convertmy.ToDecimal(m_IemInfo.IemDiagInfo.Medical_Quality_Id) == 2)
                chkMedicalQuality2.Checked = true;
            if (Convertmy.ToDecimal(m_IemInfo.IemDiagInfo.Medical_Quality_Id) == 3)
                chkMedicalQuality3.Checked = true;
            lueZkys.CodeValue = m_IemInfo.IemDiagInfo.Quality_Control_DoctorID;
            lueZkhs.CodeValue = m_IemInfo.IemDiagInfo.Quality_Control_NurseID;
            if (!String.IsNullOrEmpty(m_IemInfo.IemDiagInfo.Quality_Control_Date))
            {
                deZkDate.DateTime = Convert.ToDateTime(m_IemInfo.IemDiagInfo.Quality_Control_Date);
                //teZkDate.Time = Convert.ToDateTime(m_IemInfo.IemDiagInfo.Quality_Control_Date);
            }
            //}
            #endregion
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            //入院状态
            if (chkAdmitInfo1.Checked == true)
                m_IemInfo.IemDiagInfo.AdmitInfo = "1";
            else if (chkAdmitInfo2.Checked == true)
                m_IemInfo.IemDiagInfo.AdmitInfo = "2";
            else if (chkAdmitInfo3.Checked == true)
                m_IemInfo.IemDiagInfo.AdmitInfo = "3";
            m_IemInfo.IemDiagInfo.Pathology_Diagnosis_Name = txtPathologyName.Text;
            //m_IemInfo.IemDiagInfo.Pathology_Observation_Sn = txtPathologyObservationSn.Text;

            //m_IemInfo.IemDiagInfo.Ashes_Diagnosis_Name = txtAshesDiagnosisName.Text;
            //m_IemInfo.IemDiagInfo.Ashes_Anatomise_Sn = txtAshesAnatomiseSn.Text;

            m_IemInfo.IemDiagInfo.Allergic_Drug = txtAllergicDrug.Text;

            if (chkHBsAg1.Checked == true)
                m_IemInfo.IemDiagInfo.Hbsag = "0";
            if (chkHBsAg2.Checked == true)
                m_IemInfo.IemDiagInfo.Hbsag = "1";
            if (chkHBsAg3.Checked == true)
                m_IemInfo.IemDiagInfo.Hbsag = "2";

            if (chkHCV1.Checked == true)
                m_IemInfo.IemDiagInfo.Hcv_Ab = "0";
            if (chkHCV2.Checked == true)
                m_IemInfo.IemDiagInfo.Hcv_Ab = "1";
            if (chkHCV3.Checked == true)
                m_IemInfo.IemDiagInfo.Hcv_Ab = "2";



            if (chkHIV1.Checked == true)
                m_IemInfo.IemDiagInfo.Hiv_Ab = "0";
            if (chkHIV2.Checked == true)
                m_IemInfo.IemDiagInfo.Hiv_Ab = "1";
            if (chkHIV3.Checked == true)
                m_IemInfo.IemDiagInfo.Hiv_Ab = "2";



            if (chkOpdIpd1.Checked == true)
                m_IemInfo.IemDiagInfo.Opd_Ipd_Id = "0";
            if (chkOpdIpd2.Checked == true)
                m_IemInfo.IemDiagInfo.Opd_Ipd_Id = "1";
            if (chkOpdIpd3.Checked == true)
                m_IemInfo.IemDiagInfo.Opd_Ipd_Id = "2";
            if (chkOpdIpd4.Checked == true)
                m_IemInfo.IemDiagInfo.Opd_Ipd_Id = "3";




            if (chkInOut1.Checked == true)
                m_IemInfo.IemDiagInfo.In_Out_Inpatinet_Id = "0";
            if (chkInOut2.Checked == true)
                m_IemInfo.IemDiagInfo.In_Out_Inpatinet_Id = "1";
            if (chkInOut3.Checked == true)
                m_IemInfo.IemDiagInfo.In_Out_Inpatinet_Id = "2";
            if (chkInOut4.Checked == true)
                m_IemInfo.IemDiagInfo.In_Out_Inpatinet_Id = "3";


            if (chkBeforeAfter1.Checked == true)
                m_IemInfo.IemDiagInfo.Before_After_Or_Id = "0";
            if (chkBeforeAfter2.Checked == true)
                m_IemInfo.IemDiagInfo.Before_After_Or_Id = "1";
            if (chkBeforeAfter3.Checked == true)
                m_IemInfo.IemDiagInfo.Before_After_Or_Id = "2";
            if (chkBeforeAfter4.Checked == true)
                m_IemInfo.IemDiagInfo.Before_After_Or_Id = "3";


            if (chkClinical1.Checked == true)
                m_IemInfo.IemDiagInfo.Clinical_Pathology_Id = "0";
            if (chkClinical2.Checked == true)
                m_IemInfo.IemDiagInfo.Clinical_Pathology_Id = "1";
            if (chkClinical3.Checked == true)
                m_IemInfo.IemDiagInfo.Clinical_Pathology_Id = "2";
            if (chkClinical4.Checked == true)
                m_IemInfo.IemDiagInfo.Clinical_Pathology_Id = "3";


            if (chkPacsPathology1.Checked == true)
                m_IemInfo.IemDiagInfo.Pacs_Pathology_Id = "0";
            if (chkPacsPathology2.Checked == true)
                m_IemInfo.IemDiagInfo.Pacs_Pathology_Id = "1";
            if (chkPacsPathology3.Checked == true)
                m_IemInfo.IemDiagInfo.Pacs_Pathology_Id = "2";
            if (chkPacsPathology4.Checked == true)
                m_IemInfo.IemDiagInfo.Pacs_Pathology_Id = "3";

            m_IemInfo.IemDiagInfo.Save_Times = seSaveTimes.Value.ToString();
            m_IemInfo.IemDiagInfo.Success_Times = seSuccessTimes.Value.ToString();


            //入院后确诊日期
            if (!(deInCheckDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemDiagInfo.In_Check_Date = deInCheckDate.DateTime.ToString("yyyy-MM-dd") + " " + teInCheckDate.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemDiagInfo.In_Check_Date = 

            DataTable dt = m_IemInfo.IemDiagInfo.OutDiagTable;
            dt.Rows.Clear();
            //门(急)诊诊断
            //m_IemInfo.IemDiagInfo = new List<Iem_Mainpage_Diagnosis>();
            if (!String.IsNullOrEmpty(this.lueOutDiag.CodeValue))
            {
                DataRow imOut = dt.NewRow();
                //Iem_Mainpage_Diagnosis imOut = new Iem_Mainpage_Diagnosis();
                imOut["Diagnosis_Code"] = this.lueOutDiag.CodeValue;
                imOut["Diagnosis_Name"] = this.lueOutDiag.DisplayValue;
                imOut["Diagnosis_Type_Id"] = 13;
                //m_IemInfo.IemDiagInfo.Add(imOut);
                dt.Rows.Add(imOut);
                m_IemInfo.IemDiagInfo.OutDiagID = this.lueOutDiag.CodeValue;
                m_IemInfo.IemDiagInfo.OutDiagName = this.lueOutDiag.DisplayValue;
            }
            //入院诊断
            if (!String.IsNullOrEmpty(this.lueInDiag.CodeValue))
            {
                DataRow imOut = dt.NewRow();
                //Iem_Mainpage_Diagnosis imOut = new Iem_Mainpage_Diagnosis();
                imOut["Diagnosis_Code"] = this.lueInDiag.CodeValue;
                imOut["Diagnosis_Name"] = this.lueInDiag.DisplayValue;
                imOut["Diagnosis_Type_Id"] = 2;
                //m_IemInfo.IemDiagInfo.Add(imOut);
                dt.Rows.Add(imOut);

                m_IemInfo.IemDiagInfo.InDiagID = this.lueInDiag.CodeValue;
                m_IemInfo.IemDiagInfo.InDiagName = this.lueInDiag.DisplayValue;
            }
            if (this.gridControl1.DataSource != null)
            {
                //出院诊断
                DataTable dataTable = this.gridControl1.DataSource as DataTable;
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];

                    DataRow imOut = dt.NewRow();
                    imOut["Diagnosis_Code"] = row["Diagnosis_Code"].ToString();
                    imOut["Diagnosis_Name"] = row["Diagnosis_Name"].ToString();
                    if (i == 0)
                        imOut["Diagnosis_Type_Id"] = 7;
                    else
                        imOut["Diagnosis_Type_Id"] = 8;
                    imOut["Status_Id"] = Convertmy.ToDecimal(row["Status_Id"]);
                    imOut["Status_Name"] = row["Status_Name"];
                    imOut["Order_Value"] = i + 1;
                    dt.Rows.Add(imOut);
                }
            }

            m_IemInfo.IemDiagInfo.ZymosisID = lueZymosisName.CodeValue;
            m_IemInfo.IemDiagInfo.ZymosisName = lueZymosisName.Text;
            m_IemInfo.IemDiagInfo.Hurt_Toxicosis_Element = txtHurt_Toxicosis_Ele.Text;

            if (cmbZymosisState.SelectedIndex > -1)
            {
                m_IemInfo.IemDiagInfo.ZymosisState = (cmbZymosisState.SelectedIndex + 1).ToString();
            }
            m_IemInfo.IemDiagInfo.Section_DirectorID = lueKszr.CodeValue;
            m_IemInfo.IemDiagInfo.Section_DirectorName = lueKszr.Text;
            m_IemInfo.IemDiagInfo.DirectorID = lueZrys.CodeValue;
            m_IemInfo.IemDiagInfo.DirectorName = lueZrys.Text;
            m_IemInfo.IemDiagInfo.Vs_EmployeeID = lueZzys.CodeValue;
            m_IemInfo.IemDiagInfo.Vs_EmployeeName = lueZzys.Text;
            m_IemInfo.IemDiagInfo.Resident_EmployeeID = lueZyys.CodeValue;
            m_IemInfo.IemDiagInfo.Resident_EmployeeName = lueZyys.Text;
            m_IemInfo.IemDiagInfo.Refresh_EmployeeID = lueJxys.CodeValue;
            m_IemInfo.IemDiagInfo.Refresh_EmployeeName = lueJxys.Text;
            m_IemInfo.IemDiagInfo.Master_InterneID = lueYjs.CodeValue;
            m_IemInfo.IemDiagInfo.Master_InterneName = lueYjs.Text;
            m_IemInfo.IemDiagInfo.InterneID = lueSxys.CodeValue;
            m_IemInfo.IemDiagInfo.InterneName = lueSxys.Text;
            m_IemInfo.IemDiagInfo.Coding_UserID = lueBmy.CodeValue;
            m_IemInfo.IemDiagInfo.Coding_UserName = lueBmy.Text;
            //病案质量
            if (chkMedicalQuality1.Checked == true)
                m_IemInfo.IemDiagInfo.Medical_Quality_Id = "1";
            if (chkMedicalQuality2.Checked == true)
                m_IemInfo.IemDiagInfo.Medical_Quality_Id = "2";
            if (chkMedicalQuality3.Checked == true)
                m_IemInfo.IemDiagInfo.Medical_Quality_Id = "3";

            m_IemInfo.IemDiagInfo.Quality_Control_DoctorID = lueZkys.CodeValue;
            m_IemInfo.IemDiagInfo.Quality_Control_DoctorName = lueZkys.Text;
            m_IemInfo.IemDiagInfo.Quality_Control_NurseID = lueZkhs.CodeValue;
            m_IemInfo.IemDiagInfo.Quality_Control_NurseName = lueZkhs.Text;
            if (!(deZkDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemDiagInfo.Quality_Control_Date = deZkDate.DateTime.ToString("yyyy-MM-dd");//+ " " + teZkDate.Time.ToString("HH:mm:ss");
            //m_IemInfo.IemBasicInfo.Quality_Control_Date = teZkDate.Time.ToShortTimeString();
        }


        /// <summary>
        /// 隐藏lue的S BUTTON
        /// </summary>
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

        private void btnNewOutDiag_Click(object sender, EventArgs e)
        {
            if (m_DiagInfoForm == null)
                m_DiagInfoForm = new IemNewDiagInfoForm(m_App);
            m_DiagInfoForm.ShowDialog();
            if (m_DiagInfoForm.DialogResult == DialogResult.OK)
            {
                m_DiagInfoForm.IemOperInfo = null;
                DataTable dataTable = m_DiagInfoForm.DataOper;


                DataTable dataTableOper = new DataTable();
                if (this.gridControl1.DataSource != null)
                    dataTableOper = this.gridControl1.DataSource as DataTable;
                if (dataTableOper.Rows.Count == 0)
                    dataTableOper = dataTable.Clone();
                foreach (DataRow row in dataTable.Rows)
                {
                    dataTableOper.ImportRow(row);
                }
                this.gridControl1.BeginUpdate();
                this.gridControl1.DataSource = dataTableOper;
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);
                this.gridControl1.EndUpdate();
            }
        }

        private void UCIemDiagnose_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is LabelControl)
                {
                    control.Visible = false;
                    e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

                }
                if (control is TextEdit)
                {
                    e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                        new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                }
            }

            //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
            //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
        }

        /// <summary>
        /// 删除诊断中数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_del_diag_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewDiagnose.FocusedRowHandle < 0)
                return;
            else
            {

                DataRow dataRow = gridViewDiagnose.GetDataRow(gridViewDiagnose.FocusedRowHandle);
                if (dataRow == null)
                    return;

                DataTable dataTableOper = this.gridControl1.DataSource as DataTable;

                dataTableOper.Rows.Remove(dataRow);

                this.gridControl1.BeginUpdate();
                this.gridControl1.DataSource = dataTableOper;
                this.gridControl1.EndUpdate();

            }

        }

        private void barManager1_QueryShowPopupMenu(object sender, DevExpress.XtraBars.QueryShowPopupMenuEventArgs e)
        {
            if (e.Control == this.gridControl1)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void gridControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (gridViewDiagnose.FocusedRowHandle < 0)
                    return;
                else
                {
                    DataRow dataRow = gridViewDiagnose.GetDataRow(gridViewDiagnose.FocusedRowHandle);
                    this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                }
            }

        }


        private void btn_OK_Click(object sender, EventArgs e)
        {
            GetUI();
            ((ShowUC)this.Parent).Close(true, m_IemInfo);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            btn_del_diag_ItemClick(null, null);
        }

        private void btn_up_Click(object sender, EventArgs e)
        {

            DataTable dataTable = (DataTable)gridControl1.DataSource;
            int index = 0;
            if (gridViewDiagnose.FocusedRowHandle < 1)
                return;
            else
            {
                DataTable dt = dataTable.Clone();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (i == gridViewDiagnose.FocusedRowHandle - 1)
                    {
                        dt.ImportRow(dataTable.Rows[i + 1]);
                    }
                    else if (i == gridViewDiagnose.FocusedRowHandle)
                        dt.ImportRow(dataTable.Rows[i - 1]);
                    else
                        dt.ImportRow(dataTable.Rows[i]);
                }
                index = gridViewDiagnose.FocusedRowHandle - 1;

                this.gridControl1.BeginUpdate();
                this.gridControl1.DataSource = dt;
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);
                this.gridControl1.EndUpdate();

                gridViewDiagnose.FocusedRowHandle = index;
            }



        }


        private void btn_down_Click(object sender, EventArgs e)
        {
            DataTable dataTable = (DataTable)gridControl1.DataSource;

            int index = 0;
            if (gridViewDiagnose.FocusedRowHandle < 0)
                return;
            else if (gridViewDiagnose.FocusedRowHandle == dataTable.Rows.Count - 1)
                return;
            else
            {
                DataTable dt = dataTable.Clone();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (i == gridViewDiagnose.FocusedRowHandle + 1)
                    {
                        dt.ImportRow(dataTable.Rows[i - 1]);
                    }
                    else if (i == gridViewDiagnose.FocusedRowHandle)
                        dt.ImportRow(dataTable.Rows[i + 1]);
                    else
                        dt.ImportRow(dataTable.Rows[i]);
                }

                index = gridViewDiagnose.FocusedRowHandle + 1;
                this.gridControl1.BeginUpdate();
                this.gridControl1.DataSource = dt;
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);
                this.gridControl1.EndUpdate();

                gridViewDiagnose.FocusedRowHandle = index;
            }
        }
    }
}
