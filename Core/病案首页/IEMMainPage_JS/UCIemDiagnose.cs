using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Library;
using DrectSoft.Wordbook;
using System.Data.SqlClient;

using DrectSoft.FrameWork.WinForm.Plugin;

using Convertmy = DrectSoft.Core.UtilsForExtension;
using DevExpress.XtraEditors;
using DrectSoft.Common.Eop;
using System.Xml;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class UCIemDiagnose : UserControl
    {
        IDataAccess m_SqlHelper;
        IDrectSoftLog m_Logger;

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

        private IEmrHost m_App;

        private DataTable m_DataTableDiag = null;
        public UCIemDiagnose()
        {
            InitializeComponent();

            m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            InitLookUpEditor();
        }
        private string CanSEEControl = string.Empty;
        private void UCIemDiagnose_Load(object sender, EventArgs e)
        {
            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();
            #if DEBUG
            #else
            //HideSbutton();
            #endif
            IemMainPageManger IemM = new IemMainPageManger(m_App, m_App.CurrentPatientInfo);
            string cansee = IemM.GetConfigValueByKey("EmrInputConfig");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(cansee);
            if (doc.GetElementsByTagName("IemPageContorlVisable")[0].InnerText == "0")//不可见
            {
                CanSEEControl = "0";
            }
            if (doc.GetElementsByTagName("IemPageContorlVisable")[0].InnerText == "1")//可见
            {
                CanSEEControl = "1";
            }

            //固定编码员 add by cyq 2012-11-23
            string encoder = GetFixedEncoder();
            if (!string.IsNullOrEmpty(encoder.Trim()))
            {
                lueBmy.CodeValue = encoder;
                lueBmy.Enabled = false;
            }
            else
            {
                lueBmy.Enabled = true;
            }
        }

        #region 初始化下拉框
        private void InitLookUpEditor()
        {
            InitLueDiagnose();
            BindEmployeeData();
        }

        private void InitLueDiagnose()
        {
            //BindLueData(lueInDiag, 12);
            BindLueData(lueOutDiag, 12);
            BindLueData(lueHurt_Toxicosis_Ele, 17);
            //BindLueData(lueZymosisName, 12);
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

            lueDuty_Nurse.SqlWordbook = sqlWordBook;
            lueDuty_Nurse.ListWindow = lupInfo;

            luejxys.SqlWordbook = sqlWordBook;
            luejxys.ListWindow = lupInfo;

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

        public void FillUI(IemMainPageInfo info, IEmrHost app)
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
            //DataTable dataTableOper = m_IemInfo.IemDiagInfo.OutDiagTable;//这种取值，进行编辑后再进入娶不到值
            IemMainPageManger IemM = new IemMainPageManger(m_App, m_App.CurrentPatientInfo);
            DataTable dataTableOper = IemM.GetIemInfo().IemDiagInfo.OutDiagTable;

            this.gridControl1.DataSource = null;
            this.gridControl1.BeginUpdate();
            if (dataTableOper.Select("Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8'").Length != 0)
                this.gridControl1.DataSource = dataTableOper.Select("Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8'").CopyToDataTable();
            this.gridControl1.EndUpdate();

            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);

            lueHurt_Toxicosis_Ele.CodeValue = m_IemInfo.IemDiagInfo.Hurt_Toxicosis_ElementID;

            txtPathologyName.Text = m_IemInfo.IemDiagInfo.Pathology_Diagnosis_Name;
            txtPathologyID.Text = m_IemInfo.IemDiagInfo.Pathology_Diagnosis_ID;
            txtPathologySn.Text = m_IemInfo.IemDiagInfo.Pathology_Observation_Sn;


            txtAllergicDrug.Text = m_IemInfo.IemDiagInfo.Allergic_Drug;
            if (m_IemInfo.IemDiagInfo.Allergic_Flag == "1")
                chkAllergic1.Checked = true;
            else if (m_IemInfo.IemDiagInfo.Allergic_Flag == "2")
                chkAllergic2.Checked = true;

            if (m_IemInfo.IemBasicInfo.Autopsy_Flag == "1")
                chkAutopsy1.Checked = true;
            else if (m_IemInfo.IemBasicInfo.Autopsy_Flag == "2")
                chkAutopsy2.Checked = true;

            if (m_IemInfo.IemDiagInfo.BloodType == "1")
                chkBlood1.Checked = true;
            else if (m_IemInfo.IemDiagInfo.BloodType == "2")
                chkBlood2.Checked = true;
            else if (m_IemInfo.IemDiagInfo.BloodType == "3")
                chkBlood3.Checked = true;
            else if (m_IemInfo.IemDiagInfo.BloodType == "4")
                chkBlood4.Checked = true;
            else if (m_IemInfo.IemDiagInfo.BloodType == "5")
                chkBlood5.Checked = true;
            else if (m_IemInfo.IemDiagInfo.BloodType == "6")
                chkBlood6.Checked = true;


            if (m_IemInfo.IemDiagInfo.Rh == "1")
                chkRH1.Checked = true;
            else if (m_IemInfo.IemDiagInfo.Rh == "2")
                chkRH2.Checked = true;
            else if (m_IemInfo.IemDiagInfo.Rh == "3")
                chkRH3.Checked = true;
            else if (m_IemInfo.IemDiagInfo.Rh == "4")
                chkRH4.Checked = true;


            foreach (DataRow im in dataTableOper.Rows)
            {
                if (im["Diagnosis_Type_Id"].ToString() == "13")
                    this.lueOutDiag.CodeValue = im["Diagnosis_Code"].ToString() == "" ? "" : im["Diagnosis_Code"].ToString();
                //else if (im["Diagnosis_Type_Id"].ToString() == "2")
                //    this.lueInDiag.CodeValue = im["Diagnosis_Code"].ToString() == "" ? "" : im["Diagnosis_Code"].ToString();
            }
            //如果dataTableOper为0，将门诊诊断的值赋给下拉框add by ywk 2012年6月15日 13:32:01 
            if (dataTableOper.Rows.Count == 0 && !string.IsNullOrEmpty(m_IemInfo.IemDiagInfo.OutDiagID))
            {
                this.lueOutDiag.CodeValue = m_IemInfo.IemDiagInfo.OutDiagID;
            }

            lueKszr.CodeValue = m_IemInfo.IemDiagInfo.Section_DirectorID;
            lueZrys.CodeValue = m_IemInfo.IemDiagInfo.DirectorID;
            lueZzys.CodeValue = m_IemInfo.IemDiagInfo.Vs_EmployeeID;
            lueZyys.CodeValue = m_IemInfo.IemDiagInfo.Resident_EmployeeID;
            lueDuty_Nurse.CodeValue = m_IemInfo.IemDiagInfo.Duty_NurseID;
            luejxys.CodeValue = m_IemInfo.IemDiagInfo.Refresh_EmployeeID;
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

            #endregion
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            m_IemInfo.IemDiagInfo = new Iem_Mainpage_Diagnosis();

            m_IemInfo.IemDiagInfo.Hurt_Toxicosis_ElementID = lueHurt_Toxicosis_Ele.CodeValue;
            m_IemInfo.IemDiagInfo.Hurt_Toxicosis_Element = lueHurt_Toxicosis_Ele.Text;

            m_IemInfo.IemDiagInfo.Pathology_Diagnosis_Name = txtPathologyName.Text;
            m_IemInfo.IemDiagInfo.Pathology_Diagnosis_ID = txtPathologyID.Text;
            m_IemInfo.IemDiagInfo.Pathology_Observation_Sn = txtPathologySn.Text;


            m_IemInfo.IemDiagInfo.Allergic_Drug = txtAllergicDrug.Text;
            if (chkAllergic1.Checked)
                m_IemInfo.IemDiagInfo.Allergic_Flag = "1";
            else if (chkAllergic2.Checked)
                m_IemInfo.IemDiagInfo.Allergic_Flag = "2";
            else
                m_IemInfo.IemDiagInfo.Allergic_Flag = "";


            if (chkAutopsy1.Checked)
                m_IemInfo.IemBasicInfo.Autopsy_Flag = "1";
            else if (chkAutopsy2.Checked)
                m_IemInfo.IemBasicInfo.Autopsy_Flag = "2";
            else
                m_IemInfo.IemBasicInfo.Autopsy_Flag = "";

            if (chkBlood1.Checked)
                m_IemInfo.IemDiagInfo.BloodType = "1";
            else if (chkBlood2.Checked)
                m_IemInfo.IemDiagInfo.BloodType = "2";
            else if (chkBlood3.Checked)
                m_IemInfo.IemDiagInfo.BloodType = "3";
            else if (chkBlood4.Checked)
                m_IemInfo.IemDiagInfo.BloodType = "4";
            else if (chkBlood5.Checked)
                m_IemInfo.IemDiagInfo.BloodType = "5";
            else if (chkBlood6.Checked)
                m_IemInfo.IemDiagInfo.BloodType = "6";
            else
                m_IemInfo.IemDiagInfo.BloodType = "";


            if (chkRH1.Checked)
                m_IemInfo.IemDiagInfo.Rh = "1";
            else if (chkRH2.Checked)
                m_IemInfo.IemDiagInfo.Rh = "2";
            else if (chkRH3.Checked)
                m_IemInfo.IemDiagInfo.Rh = "3";
            else if (chkRH4.Checked)
                m_IemInfo.IemDiagInfo.Rh = "4";
            else
                m_IemInfo.IemDiagInfo.Rh = "";


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

                    imOut["MenAndInHop"] = row["MenAndInHop"];
                    imOut["InHopAndOutHop"] = row["InHopAndOutHop"];
                    imOut["BeforeOpeAndAfterOper"] = row["BeforeOpeAndAfterOper"];
                    imOut["LinAndBingLi"] = row["LinAndBingLi"];
                    imOut["InHopThree"] = row["InHopThree"];
                    imOut["FangAndBingLi"] = row["FangAndBingLi"];

                    imOut["AdmitInfo"] = row["AdmitInfo"];

                    dt.Rows.Add(imOut);
                }
            }

            m_IemInfo.IemDiagInfo.Section_DirectorID = lueKszr.CodeValue;
            m_IemInfo.IemDiagInfo.Section_DirectorName = lueKszr.Text;
            m_IemInfo.IemDiagInfo.DirectorID = lueZrys.CodeValue;
            m_IemInfo.IemDiagInfo.DirectorName = lueZrys.Text;
            m_IemInfo.IemDiagInfo.Vs_EmployeeID = lueZzys.CodeValue;
            m_IemInfo.IemDiagInfo.Vs_EmployeeName = lueZzys.Text;
            m_IemInfo.IemDiagInfo.Resident_EmployeeID = lueZyys.CodeValue;
            m_IemInfo.IemDiagInfo.Resident_EmployeeName = lueZyys.Text;
            m_IemInfo.IemDiagInfo.Duty_NurseID = lueDuty_Nurse.CodeValue;
            m_IemInfo.IemDiagInfo.Duty_NurseName = lueDuty_Nurse.Text;
            m_IemInfo.IemDiagInfo.Refresh_EmployeeID = luejxys.CodeValue;
            m_IemInfo.IemDiagInfo.Refresh_EmployeeName = luejxys.Text;
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

            //新增的几个诊断符合情况add by ywk 2012年6月26日13:31:49
            //门诊和住院
            //if (chkMandZ0.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.MenAndInHop = "0";
            //}
            //if (chkMandZ1.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.MenAndInHop = "1";
            //}
            //if (chkMandZ2.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.MenAndInHop = "2";
            //}
            //if (chkMandZ3.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.MenAndInHop = "3";
            //}
            ////入院和出院
            //if (chkRandC0.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.InHopAndOutHop = "0";
            //}
            //if (chkRandC1.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.InHopAndOutHop = "1";
            //}
            //if (chkRandC2.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.InHopAndOutHop = "2";
            //}
            //if (chkRandC3.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.InHopAndOutHop = "3";
            //}
            ////术前和术后
            //if (chkSqAndSh0.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.BeforeOpeAndAfterOper = "0";
            //}
            //if (chkSqAndSh1.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.BeforeOpeAndAfterOper = "1";
            //}
            //if (chkSqAndSh2.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.BeforeOpeAndAfterOper = "2";
            //}
            //if (chkSqAndSh3.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.BeforeOpeAndAfterOper = "3";
            //}
            ////临床和病理
            //if (chkLandB0.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.LinAndBingLi = "0";
            //}
            //if (chkLandB1.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.LinAndBingLi = "1";
            //}
            //if (chkLandB2.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.LinAndBingLi = "2";
            //}
            //if (chkLandB3.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.LinAndBingLi = "3";
            //}
            ////入院三日内
            //if (chkRThree0.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.InHopThree = "0";
            //}
            //if (chkRThree1.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.InHopThree = "1";
            //}
            //if (chkRThree2.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.InHopThree = "2";
            //}
            //if (chkRThree3.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.InHopThree = "3";
            //}
            ////放射和病理
            //if (chkFandB0.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.FangAndBingLi = "0";
            //}
            //if (chkFandB1.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.FangAndBingLi = "1";
            //}
            //if (chkFandB2.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.FangAndBingLi = "2";
            //}
            //if (chkFandB3.Checked == true)
            //{
            //    m_IemInfo.IemDiagInfo.FangAndBingLi = "3";
            //}



            m_IemInfo.IemDiagInfo.Quality_Control_DoctorID = lueZkys.CodeValue;
            m_IemInfo.IemDiagInfo.Quality_Control_DoctorName = lueZkys.Text;
            m_IemInfo.IemDiagInfo.Quality_Control_NurseID = lueZkhs.CodeValue;
            m_IemInfo.IemDiagInfo.Quality_Control_NurseName = lueZkhs.Text;
            if (!(deZkDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                m_IemInfo.IemDiagInfo.Quality_Control_Date = deZkDate.DateTime.ToString("yyyy-MM-dd");//+ " " + teZkDate.Time.ToString("HH:mm:ss");

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
        /// <summary>
        /// 新增出院诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewOutDiag_Click(object sender, EventArgs e)
        {
            //if (m_DiagInfoForm == null)
                m_DiagInfoForm = new IemNewDiagInfoForm(m_App, "add", "", "","");
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
        /// <summary>
        /// 新加的编辑出院诊断
        /// add by ywk 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditOutDiag_Click(object sender, EventArgs e)
        {
            if (gridViewDiagnose.FocusedRowHandle < 0)
                return;
            DataRow dataRow = gridViewDiagnose.GetDataRow(gridViewDiagnose.FocusedRowHandle);
            if (dataRow == null)
                return;
            string diagcode = dataRow["Diagnosis_Code"].ToString();//诊断的ICD编码
            string statusid = dataRow["Status_Id"].ToString();//诊断结果（入院病情)
            string admitinfo = dataRow["AdmitInfo"].ToString();//子入院病情 
            m_DiagInfoForm = new IemNewDiagInfoForm(m_App, "edit", diagcode, statusid, admitinfo);
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
                    dataTableOper.Rows.Remove(dataRow);//由编辑页面返回后，要将原来编辑的此行移除，因为编辑时，会加上这个行 
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

        private Inpatient CurrentInpatient;//add by ywk 
        private void btn_OK_Click(object sender, EventArgs e)
        {
            GetUI();
            ((ShowUC)this.Parent).Close(true, m_IemInfo);
            //点击确认按钮就把数据更新到数据库中
            CurrentInpatient = m_App.CurrentPatientInfo;
            CurrentInpatient.ReInitializeAllProperties();
            IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
            manger.SaveData(m_IemInfo);


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
        /// <summary>
        /// 根据配置控制列的可见性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDiagnose_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

        }

        private void gridViewDiagnose_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {

        }
        /// <summary>
        /// 根据配置控制列的可见性
        /// add by ywk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Load(object sender, EventArgs e)
        {
            if (CanSEEControl == "0")
            {
                gridColumn6.Visible = false;
                gridColumn7.Visible = false;
                gridColumn8.Visible = false;
                gridColumn9.Visible = false;
                gridColumn10.Visible = false;
                gridColumn11.Visible = false;
            }
            if (CanSEEControl == "1")
            {
                //gridColumn6.Visible = true;
                //gridColumn7.Visible = true;
                //gridColumn8.Visible = true;
                //gridColumn9.Visible = true;
                //gridColumn10.Visible = true;
                //gridColumn11.Visible = true;
            }
        }
        /// <summary>
        /// 更改，选中后可消除选择
        /// add by ywk 2012年7月23日 08:54:53
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAllergic2_CheckedChanged(object sender, EventArgs e)
        {
            CheckEdit chkEdit = GetCheckEdit(((Control)sender).Name);
            if (chkEdit.Checked)
            {
                chkEdit.Checked = false;
            }
        }
        /// <summary>
        /// 根据名称返回控件
        /// </summary>
        /// <param name="ControlName"></param>
        /// <returns></returns>
        private CheckEdit GetCheckEdit(string ControlName)
        {
            foreach (Control control in this.Controls)
            {
                if (control.Name == ControlName)
                {
                    return (CheckEdit)control;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取固定编码员工号(配置)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-23</date>
        /// </summary>
        public string GetFixedEncoder()
        {
            try
            {
                IemMainPageManger IemM = new IemMainPageManger(m_App, m_App.CurrentPatientInfo);
                string encoder = string.Empty;
                string encoderConfig = IemM.GetConfigValueByKey("FixedEncoder");
                if (!string.IsNullOrEmpty(encoderConfig.Trim()))
                {
                    if (encoderConfig.Contains(","))
                    {
                        string[] str = encoderConfig.Split(',');
                        encoder = str[0];
                    }
                    else
                    {
                        encoder = encoderConfig;
                    }
                    //员工工号不满6位，则不足6位
                    if (!string.IsNullOrEmpty(encoder.Trim()) && encoder.Length < 6)
                    {
                        int length = encoder.Length;
                        for (int i = 0; i < 6 - length; i++)
                        {
                            encoder = "0" + encoder;
                        }
                    }
                }
                return encoder;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
