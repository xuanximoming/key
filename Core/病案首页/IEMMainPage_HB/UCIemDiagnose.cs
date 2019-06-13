using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Common.Library;
using DrectSoft.Core.ReportManager;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Convertmy = DrectSoft.Core.UtilsForExtension;
#pragma warning disable 0618

namespace DrectSoft.Core.IEMMainPage
{
    public partial class UCIemDiagnose : UserControl
    {
        IDataAccess m_SqlHelper;
        IDrectSoftLog m_Logger;
        public bool editFlag = false;  //add by cyq 2012-12-06 病案室人员编辑首页(状态改为归档)
        private string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("MorphologyIsShow");  // add by jxh 
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
        //诊断下拉框数据集
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
            try
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

                if (valueStr != "0")  //add by jxh 修改这两列状态为显示
                {
                    this.gridColumn41.Visible = false;
                    this.gridColumn42.Visible = false;
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

                ///1、手术列表显示：并发症、麻醉分级、是否择期手术、是否无菌手术、是否感染
                if (CanSEEControl == "1")
                {
                    gridViewOper.Columns["OPERCOMPLICATION_CODE"].Visible = true;
                    gridViewOper.Columns["ANESTHESIA_LEVEL"].Visible = true;
                    gridViewOper.Columns["ISCHOOSEDATENAME"].Visible = true;
                    gridViewOper.Columns["ISCLEAROPENAME"].Visible = true;
                    gridViewOper.Columns["ISGANRANNAME"].Visible = true;
                }
                else
                {
                    gridViewOper.Columns["OPERCOMPLICATION_CODE"].Visible = false;
                    gridViewOper.Columns["ANESTHESIA_LEVEL"].Visible = false;
                    gridViewOper.Columns["ISCHOOSEDATENAME"].Visible = false;
                    gridViewOper.Columns["ISCLEAROPENAME"].Visible = false;
                    gridViewOper.Columns["ISGANRANNAME"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        /// <summary>
        /// 二次修改捞取诊断
        /// add by ywk 2013年3月19日10:19:49  
        /// </summary>
        /// <param name="lueInfo"></param>
        /// <param name="queryType"></param>
        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            //更改捞取诊断的数据源 add by ywk 2013年3月19日10:18:52 
            //if (m_DataTableDiag == null)
            //{
            //string getdiagtype = "1";
            string getdiagtype = DS_SqlService.GetConfigValueByKey("GetDiagnosisType") == "" ? "0" : DS_SqlService.GetConfigValueByKey("GetDiagnosisType");
            if (getdiagtype == "0")//EMR
            {
                m_DataTableDiag = GetEditroData(queryType);
            }
            if (getdiagtype == "1")//HIS 
            {
                try
                {
                    using (OracleConnection conn = new OracleConnection(DataAccessFactory.GetSqlDataAccess("HISDB").GetDbConnection().ConnectionString))
                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        m_DataTableDiag = new DataTable();
                        if (queryType.ToString() == "17")//代表损伤
                        {
                            m_DataTableDiag = GetEditroData(queryType);
                        }
                        else
                        {
                            OracleCommand cmd = conn.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = " SELECT  MarkId as  ID,  NAME, py , WB, memo,  icd  FROM yd_diagnosis ";
                            OracleDataAdapter myoadapt = new OracleDataAdapter(cmd.CommandText, conn);
                            myoadapt.Fill(m_DataTableDiag);
                        }
                        //MessageBox.Show("诊断取的HIS");
                    }
                }
                catch (Exception ex)
                {
                    m_DataTableDiag = GetEditroData(queryType);
                    //MessageBox.Show("出异常，诊断取自EMR" + ex.Message);
                }
            }

            //m_DataTableDiag = GetEditroData(queryType);
            //}
            //损伤中毒因素要改造下 add by ywk 二〇一三年五月二十九日 10:28:39 
            if (queryType.ToString() == "17")//代表损伤
            {
                m_DataTableDiag.Columns["ID"].Caption = "编码";
                m_DataTableDiag.Columns["NAME"].Caption = "名称";
                Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
                columnwidth.Add("ID", 90);
                columnwidth.Add("NAME", 210);
                SqlWordbook sqlWordBook = new SqlWordbook("ID", m_DataTableDiag, "ID", "NAME", columnwidth, "ID//NAME//PY");

                lueInfo.SqlWordbook = sqlWordBook;
                lueInfo.ListWindow = lupInfo;
            }
            else
            {
                m_DataTableDiag.Columns["ID"].Caption = "诊断编码";
                m_DataTableDiag.Columns["NAME"].Caption = "诊断名称";
                Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
                columnwidth.Add("ID", 90);
                columnwidth.Add("NAME", 210);
                SqlWordbook sqlWordBook = new SqlWordbook("ID", m_DataTableDiag, "ID", "NAME", columnwidth, "ID//NAME//PY//WB");

                lueInfo.SqlWordbook = sqlWordBook;
                lueInfo.ListWindow = lupInfo;
            }
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
            FillUIInnerOper();
        }

        delegate void FillUIDelegate();
        private void FillUIInner()
        {
            try
            {
                #region 已注释
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
                #endregion

                #region
                //设置当前病人(修复m_App病人丢失问题)
                Inpatient currentPatientInfo = null;
                if (null == m_App || null == m_App.CurrentPatientInfo)
                {
                    currentPatientInfo = DS_SqlService.GetPatientInfo(m_IemInfo.IemBasicInfo.NoOfInpat);
                }
                else
                {
                    currentPatientInfo = m_App.CurrentPatientInfo;
                }

                IemMainPageManger IemM = new IemMainPageManger(m_App, null == m_App.CurrentPatientInfo ? currentPatientInfo : m_App.CurrentPatientInfo);
                DataTable dataTableOper = IemM.GetIemInfo().IemDiagInfo.OutDiagTable;

                this.gridControl1.DataSource = null;
                this.gridControl1.BeginUpdate();
                if (dataTableOper.Select("Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8'").Length != 0)
                {
                    this.gridControl1.DataSource = dataTableOper.Select("Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8'").CopyToDataTable();
                }
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void FillUIInnerOper()
        {
            try
            {
                if (null != m_IemInfo && null != m_IemInfo.IemBasicInfo && !string.IsNullOrEmpty(m_IemInfo.IemBasicInfo.Iem_Mainpage_NO))
                {
                    this.gridControl2.BeginUpdate();
                    this.gridControl2.DataSource = m_IemInfo.IemOperInfo.Operation_Table;
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
                    this.gridControl2.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            try
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

                        imOut["MorphoLogyIcd"] = row["MorphoLogyIcd"].ToString();
                        imOut["MorphoLogyName"] = row["MorphoLogyName"].ToString();

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

                        //add by cyq 2012-12-25
                        imOut["iem_mainpage_diagnosis_no"] = row["iem_mainpage_diagnosis_no"];
                        imOut["iem_mainpage_no"] = row["iem_mainpage_no"];
                        imOut["Status_Id_Out"] = row["Status_Id_Out"];
                        imOut["Status_Name_Out"] = row["Status_Name_Out"];

                        dt.Rows.Add(imOut);
                    }
                }

                m_IemInfo.IemDiagInfo.OutDiagTable = dt;

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

                #region 已注释
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
                #endregion

                m_IemInfo.IemDiagInfo.Quality_Control_DoctorID = lueZkys.CodeValue;
                m_IemInfo.IemDiagInfo.Quality_Control_DoctorName = lueZkys.Text;
                m_IemInfo.IemDiagInfo.Quality_Control_NurseID = lueZkhs.CodeValue;
                m_IemInfo.IemDiagInfo.Quality_Control_NurseName = lueZkhs.Text;
                if (!(deZkDate.DateTime.CompareTo(DateTime.MinValue) == 0))
                {
                    m_IemInfo.IemDiagInfo.Quality_Control_Date = deZkDate.DateTime.ToString("yyyy-MM-dd");//+ " " + teZkDate.Time.ToString("HH:mm:ss");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// GET Operation UI
        /// </summary>
        private void GetOperationUI()
        {
            try
            {
                if (this.gridControl2.DataSource != null && (gridControl2.DataSource as DataTable).Rows.Count > 0)
                {
                    //手术

                    DataTable dtOperation = m_IemInfo.IemOperInfo.Operation_Table.Clone();
                    dtOperation.Rows.Clear();

                    DataTable dataTable = this.gridControl2.DataSource as DataTable;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        DataRow imOut = dtOperation.NewRow();

                        imOut["Operation_Code"] = row["Operation_Code"].ToString();
                        imOut["Operation_Name"] = row["Operation_Name"].ToString();
                        imOut["Operation_Date"] = row["Operation_Date"].ToString();

                        imOut["operation_level"] = row["operation_level"].ToString();
                        imOut["operation_level_Name"] = row["operation_level_Name"].ToString();

                        imOut["Execute_User1"] = row["Execute_User1"].ToString();
                        imOut["Execute_User1_Name"] = row["Execute_User1_Name"];
                        imOut["Execute_User2"] = row["Execute_User2"].ToString();
                        imOut["Execute_User2_Name"] = row["Execute_User2_Name"].ToString();
                        imOut["Execute_User3"] = row["Execute_User3"].ToString();
                        imOut["Execute_User3_Name"] = row["Execute_User3_Name"].ToString();
                        imOut["Anaesthesia_Type_Id"] = row["Anaesthesia_Type_Id"].ToString();
                        imOut["Anaesthesia_Type_Name"] = row["Anaesthesia_Type_Name"].ToString();
                        imOut["Close_Level"] = row["Close_Level"].ToString();
                        imOut["Close_Level_Name"] = row["Close_Level_Name"].ToString();
                        imOut["Anaesthesia_User"] = row["Anaesthesia_User"].ToString();
                        imOut["Anaesthesia_User_Name"] = row["Anaesthesia_User_Name"].ToString();

                        imOut["IsChooseDate"] = row["IsChooseDate"].ToString();
                        imOut["IsClearOpe"] = row["IsClearOpe"].ToString();
                        imOut["IsGanRan"] = row["IsGanRan"].ToString();
                        imOut["IsChooseDateName"] = row["IsChooseDateName"].ToString();
                        imOut["IsClearOpeName"] = row["IsClearOpeName"].ToString();
                        imOut["IsGanRanName"] = row["IsGanRanName"].ToString();
                        //麻醉分级和手术并发症 add by cyq 2012-10-17
                        imOut["Anesthesia_Level"] = row["Anesthesia_Level"].ToString();
                        imOut["OperComplication_Code"] = row["OperComplication_Code"].ToString();
                        dtOperation.Rows.Add(imOut);
                    }

                    m_IemInfo.IemOperInfo.Operation_Table = dtOperation;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
            try
            {
                //if (m_DiagInfoForm == null)
                m_DiagInfoForm = new IemNewDiagInfoForm(m_App, "add", "", "", "");
                m_DiagInfoForm.ShowDialog();
                if (m_DiagInfoForm.DialogResult == DialogResult.OK)
                {
                    m_DiagInfoForm.IemOperInfo = null;
                    DataTable dataTable = m_DiagInfoForm.DataOper;

                    DataTable dataTableOper = new DataTable();
                    if (this.gridControl1.DataSource != null)
                    {
                        dataTableOper = this.gridControl1.DataSource as DataTable;
                    }
                    if (dataTableOper.Rows.Count == 0)
                    {
                        dataTableOper = dataTable.Clone();
                    }
                    DataRow row = dataTable.Rows[0];
                    if (CheckIfExistTheRow(dataTableOper, row))
                    {
                        MessageBox.Show("诊断列表中已存在相同的记录，已自动合并。");
                        return;
                    }
                    DataRow newRow = dataTableOper.NewRow();
                    foreach (DataColumn item in dataTableOper.Columns)
                    {
                        if (dataTable.Columns.Contains(item.ColumnName))
                        {
                            newRow[item.ColumnName] = row[item.ColumnName].ToString();
                        }
                    }
                    dataTableOper.Rows.Add(newRow);
                    this.gridControl1.BeginUpdate();
                    this.gridControl1.DataSource = dataTableOper;
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);
                    this.gridControl1.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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
            try
            {
                if (gridViewDiagnose.FocusedRowHandle < 0)
                {
                    return;
                }
                int rowHandel = gridViewDiagnose.FocusedRowHandle;
                DataRow dataRow = gridViewDiagnose.GetDataRow(gridViewDiagnose.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }
                int diagnosisNo = (null != dataRow["iem_mainpage_diagnosis_no"] && !string.IsNullOrEmpty(dataRow["iem_mainpage_diagnosis_no"].ToString())) ? int.Parse(dataRow["iem_mainpage_diagnosis_no"].ToString().Trim()) : -1;
                int mainpageNo = (null != dataRow["iem_mainpage_no"] && !string.IsNullOrEmpty(dataRow["iem_mainpage_no"].ToString())) ? int.Parse(dataRow["iem_mainpage_no"].ToString().Trim()) : -1;
                string diagcode = dataRow["Diagnosis_Code"].ToString();//诊断的ICD编码
                string statusid = dataRow["Status_Id"].ToString();//诊断结果（入院病情)
                string admitinfo = dataRow["AdmitInfo"].ToString();//子入院病情 
                string morphoicd = dataRow["MorphoLogyIcd"].ToString();//形态学诊断编码 add by jxh
                string statusIDOut = dataRow["Status_Id_Out"].ToString();//出院情况 
                //m_DiagInfoForm = new IemNewDiagInfoForm(m_App, "edit", diagcode, statusid, admitinfo);
                m_DiagInfoForm = new IemNewDiagInfoForm(m_App, "edit", diagcode, statusid, admitinfo, morphoicd, statusIDOut, diagnosisNo, mainpageNo);
                m_DiagInfoForm.ShowDialog();

                if (m_DiagInfoForm.DialogResult == DialogResult.OK)
                {
                    m_DiagInfoForm.IemOperInfo = null;
                    DataTable dataTable = m_DiagInfoForm.DataOper;
                    DataTable dataTableOper = new DataTable();
                    if (this.gridControl1.DataSource != null)
                    {
                        dataTableOper = this.gridControl1.DataSource as DataTable;
                    }
                    if (dataTableOper.Rows.Count == 0)
                    {
                        dataTableOper = dataTable.Clone();
                    }
                    DataRow rows = dataTable.Rows[0];

                    if (CheckIfExistTheRow(dataTableOper, rows))
                    {
                        MessageBox.Show("诊断列表中存在相同的记录，已自动合并。");
                        return;
                    }
                    foreach (DataColumn item in dataTableOper.Columns)
                    {
                        if (dataTable.Columns.Contains(item.ColumnName))
                        {
                            dataRow[item.ColumnName] = rows[item.ColumnName].ToString();
                        }
                    }
                    gridControl1.DataSource = dataTableOper;
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);
                }

                gridViewDiagnose.MoveBy(rowHandel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 列表中是否存在相同的记录
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool CheckIfExistTheRow(DataTable dt, DataRow row)
        {
            try
            {
                bool boo = false;
                DataRow[] thisList = dt.Select(" 1=1 ");
                if (null != dt && dt.Rows.Count > 0)
                {
                    foreach (DataRow drow in dt.Rows)
                    {
                        if ((null != drow["diagnosis_code"] && null != row["diagnosis_code"] && drow["diagnosis_code"].ToString() == row["diagnosis_code"].ToString())
                            && (null != drow["Status_Id"] && null != row["Status_Id"] && (drow["Status_Id"].ToString() == "0" ? "" : drow["Status_Id"].ToString()) == row["Status_Id"].ToString())
                            && (null != drow["AdmitInfo"] && null != row["AdmitInfo"] && (drow["AdmitInfo"].ToString() == "0" ? "" : drow["AdmitInfo"].ToString()) == row["AdmitInfo"].ToString())
                            && (null != drow["Status_Id_Out"] && null != row["Status_Id_Out"] && (drow["Status_Id_Out"].ToString() == "0" ? "" : drow["Status_Id_Out"].ToString()) == row["Status_Id_Out"].ToString())
                            && (null != drow["iem_mainpage_diagnosis_no"] && null != row["iem_mainpage_diagnosis_no"] && ((drow["iem_mainpage_diagnosis_no"].ToString() != row["iem_mainpage_diagnosis_no"].ToString()) || (drow["iem_mainpage_diagnosis_no"].ToString() == "0" && row["iem_mainpage_diagnosis_no"].ToString() == "0"))))
                        {
                            boo = true;
                            break;
                        }
                    }
                }
                return boo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
            try
            {
                if (gridViewDiagnose.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选中一条诊断记录");
                    return;
                }

                DataRow dataRow = gridViewDiagnose.GetDataRow(gridViewDiagnose.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }
                DataTable dataTableOper = this.gridControl1.DataSource as DataTable;

                dataTableOper.Rows.Remove(dataRow);

                this.gridControl1.BeginUpdate();
                this.gridControl1.DataSource = dataTableOper;
                this.gridControl1.EndUpdate();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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

        /// <summary>
        /// 右键
        /// </summary>
        /// edit by Yanqiao.Cai 2012-12-20
        /// 1、add try ... catch
        /// 2、右键标题无操作
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    GridHitInfo hitInfo = gridViewDiagnose.CalcHitInfo(gridControl1.PointToClient(Cursor.Position));
                    if (hitInfo.RowHandle < 0)
                    {
                        return;
                    }

                    if (gridViewDiagnose.FocusedRowHandle < 0)
                    {
                        return;
                    }
                    else
                    {
                        DataRow dataRow = gridViewDiagnose.GetDataRow(gridViewDiagnose.FocusedRowHandle);
                        this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private Inpatient CurrentInpatient;//add by ywk 

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                //设置当前病人(修复m_App病人丢失问题) add by cyq 2012-12-06
                if (null == m_App || null == m_App.CurrentPatientInfo || m_App.CurrentPatientInfo.NoOfFirstPage.ToString() != m_IemInfo.IemBasicInfo.NoOfInpat)
                {
                    if (string.IsNullOrEmpty(m_IemInfo.IemBasicInfo.NoOfInpat))
                    {
                        CurrentInpatient = m_App.CurrentPatientInfo;
                    }
                    else
                    {
                        CurrentInpatient = DS_SqlService.GetPatientInfo(m_IemInfo.IemBasicInfo.NoOfInpat);
                    }
                }
                else
                {
                    CurrentInpatient = m_App.CurrentPatientInfo;
                }



                GetUI();
                //获取手术信息 add by cyq 2012-12-21
                GetOperationUI();

                //edit by 2012-12-20 张业兴 关闭弹出框只关闭提示框
                //((ShowUC)this.Parent).Close(true, m_IemInfo);  
                //点击确认按钮就把数据更新到数据库中
                //CurrentInpatient = m_App.CurrentPatientInfo;
                if (null != CurrentInpatient)
                {
                    CurrentInpatient.ReInitializeAllProperties();
                }
                IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
                //m_IemInfo = manger.GetIemInfo();
                manger.SaveData(m_IemInfo);
                //add by cyq 2012-12-05 病案室人员编辑后状态改为已归档
                if (editFlag)
                {
                    DS_BaseService.SetRecordsRebacked(int.Parse(CurrentInpatient.NoOfFirstPage.ToString().Trim()));
                }
                //add by ywk 2013年8月15日 19:41:17
                string ischeckreport = DS_SqlService.GetConfigValueByKey("MorphologyIsShow");//根据形态学控制是否判断填写报告卡
                if (ischeckreport == "0")
                {
                    GetReportVialde();
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 进行判断医生所填的出院诊断是否在报告卡病种维护中
        /// add by ywk 2013年8月15日 17:09:53
        /// </summary>
        public void GetReportVialde()
        {
            try
            {
                IemMainPageManger m_IemMainPage = new IemMainPageManger(m_App, CurrentInpatient);
                bool iscontinue = false;//防止一个病人符合多个报告卡的诊断范围 add by ywk 2013年8月15日 17:11:00

                if (m_IemMainPage != null)
                {
                    IemMainPageInfo IemInfo = m_IemMainPage.GetIemInfo();
                    if (IemInfo != null && IemInfo.IemBasicInfo != null)
                    {
                        //验证逻辑： 出院诊断列表中，存在属于传染病诊断的列表，且已经申报的次数小于该诊断设置的最大申报次数
                        string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                        //add by ywk 2013年8月15日 17:17:45
                        //1、先查出一共多少个报告卡(如果卡没有维护相应的ICD编码就不必要判断了)
                        string searchcardnum = string.Format("select * from reportcategory where valid=1 ");
                        DataTable dtcardnum = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(searchcardnum, CommandType.Text);
                        Dictionary<string, bool> m_dic = new Dictionary<string, bool>();
                        string tablename = string.Empty;//存放表名
                        DataTable dtcarddiags = null;
                        bool hasdiagcode = false;//是否有配置诊断编码组合
                        //2、查出各个报告卡是否维护了病种诊断编码
                        for (int i = 0; i < dtcardnum.Rows.Count; i++)
                        {
                            tablename = dtcardnum.Rows[i]["TABLENAME"].ToString().Trim();
                            dtcarddiags = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(string.Format("select * from zymosis_diagnosis where CATEGORYID='{0}'", dtcardnum.Rows[i]["ID"].ToString().Trim()));
                            if (dtcarddiags.Rows.Count > 0)
                            {
                                hasdiagcode = true;
                            }
                            else
                            {
                                hasdiagcode = false;
                            }

                            m_dic.Add(tablename, hasdiagcode);

                        }
                        //3、根据诊断编码和reportcategory维护的对应卡信息表名去判断然后弹出各个报告卡填写框
                        foreach (var item in m_dic.Keys)
                        {
                            string sqlText = string.Empty;
                            switch (item)
                            {
                                case "theriomareportcard":
                                    if (m_dic[item])
                                    {
                                        sqlText =
                        @"select ie.iem_mainpage_no,z.icd,z.upcount,z.name from iem_mainpage_diagnosis_2012 ie    
                                left join zymosis_diagnosis z on ie.diagnosis_code=z.icd and z.valid = 1  and ie.DIAGNOSIS_TYPE_ID<>13
                                left join iem_mainpage_basicinfo_2012 imb on imb.iem_mainpage_no=ie.iem_mainpage_no and imb.valide = 1
                               where z.CATEGORYID = 2 and z.valid=1 and ie.valide=1 and ie.iem_mainpage_no = @iem_mainpage_no  
                                and exists
                                (
                                    select 1 from zymosis_diagnosis z 
                                        where z.icd = ie.diagnosis_code and z.categoryid = 2 and z.valid = 1
                                        and (
                                                select count(1) from theriomareportcard zr 
                                                    where zr.report_icd10 = z.icd and zr.report_noofinpat = imb.noofinpat 
                                                    and zr.vaild = 1 and zr.state != '7'
                                            ) < z.upcount
                                )
                               group by ie.iem_mainpage_no,z.icd,z.upcount,z.name    
                              having count(z.icd)>0 ";
                                        GoTheriomareportTip(sqlText, IemInfo.IemBasicInfo);
                                    }

                                    break;
                                case "CARDIOVASCULARCARD":
                                    if (m_dic[item])
                                    {
                                        sqlText = @"select ie.iem_mainpage_no,z.icd,z.upcount,z.name from iem_mainpage_diagnosis_2012 ie    
                                left join zymosis_diagnosis z on ie.diagnosis_code=z.icd and z.valid = 1  and ie.DIAGNOSIS_TYPE_ID<>13
                                left join iem_mainpage_basicinfo_2012 imb on imb.iem_mainpage_no=ie.iem_mainpage_no and imb.valide = 1
                               where z.CATEGORYID = 4 and z.valid=1 and ie.valide=1 and ie.iem_mainpage_no = @iem_mainpage_no  
                                and exists
                                (
                                    select 1 from zymosis_diagnosis z 
                                        where z.icd = ie.diagnosis_code and z.categoryid = 4 and z.valid = 1
                                        and (
                                                select count(1) from cardiovascularcard zr 
                                                    where zr.icd = z.icd and zr.noofinpat = imb.noofinpat 
                                                    and zr.vaild = 1 and zr.state != '7'
                                            ) < z.upcount
                                )
                               group by ie.iem_mainpage_no,z.icd,z.upcount,z.name    
                              having count(z.icd)>0 ";
                                        GetCardiovasular(sqlText, IemInfo.IemBasicInfo);
                                    }

                                    break;
                                case "birthdefectreportcard":
                                    if (m_dic[item])
                                    {
                                        sqlText = @"select ie.iem_mainpage_no, z.icd, z.upcount, z.name
                                      from iem_mainpage_diagnosis_2012 ie
                                      left join zymosis_diagnosis z on ie.diagnosis_code = z.icd
                                                                   and z.valid = 1  and ie.DIAGNOSIS_TYPE_ID<>13
                                      left join iem_mainpage_basicinfo_2012 imb on imb.iem_mainpage_no =
                                                                                   ie.iem_mainpage_no
                                                                               and imb.valide = 1
                                     where z.CATEGORYID = 3
                                       and z.valid = 1
                                       and ie.valide = 1
                                       and ie.iem_mainpage_no = @iem_mainpage_no
                                       and exists (select 1
                                              from zymosis_diagnosis z
                                             where z.icd = ie.diagnosis_code  and z.categoryid = 3
                                               and z.valid = 1  and (select count(1)
                                                      from birthdefectscard zr
                                                     where zr.diag_code = z.icd
                                                       and zr.report_noofinpat = imb.noofinpat
                                                       and zr.vaild = 1
                                                       and zr.state != '7') < z.upcount)
                                     group by ie.iem_mainpage_no, z.icd, z.upcount, z.name
                                    having count(z.icd) > 0";
                                        GetBirthdefects(sqlText, IemInfo.IemBasicInfo);
                                    }

                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }


        }
        /// <summary>
        /// 验证是否需要填写出生缺陷报告卡
        /// add by ywk 2013年8月15日 22:56:46
        /// </summary>
        /// <param name="sqlText"></param>
        private void GetBirthdefects(string sqlText, Iem_Mainpage_Basicinfo Iem_Mainpagebasic)
        {
            try
            {
                DataTable reportcard = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(string.Format("select OUTHOSDEPT,ADMITDATE,BIRTH from INPATIENT where NOOFINPAT='{0}'", Iem_Mainpagebasic.NoOfInpat));
                if (reportcard.Rows.Count > 0)
                {
                    DateTime birthday = Convert.ToDateTime(DateTime.Parse(reportcard.Rows[0]["BIRTH"].ToString()).ToString("yyyy-MM-dd"));
                    DateTime inhosday = Convert.ToDateTime(DateTime.Parse(reportcard.Rows[0]["ADMITDATE"].ToString()).ToString("yyyy-MM-dd"));
                    TimeSpan days = inhosday - birthday;
                    int indays = days.Days;

                    string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("IsChildDept");
                    string[] strarray = valueStr.Split(',');
                    foreach (string str in strarray)
                    {
                        if (str == reportcard.Rows[0]["OUTHOSDEPT"].ToString() && indays <= 42)
                        {
                            DataTable table = m_App.SqlHelper.ExecuteDataTable(sqlText, new SqlParameter[] { new SqlParameter("@iem_mainpage_no", Iem_Mainpagebasic.Iem_Mainpage_NO) }, CommandType.Text);
                            if (table != null && table.Rows.Count > 0)
                            {
                                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该婴儿出院诊断符合出生缺陷报告卡上报条件，是否立即填报？", "出生缺陷上报", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    BirthDefectsDialog Birthcardvadiag = new BirthDefectsDialog(m_App, table, Iem_Mainpagebasic.NoOfInpat);
                                    Birthcardvadiag.icd10 = table.Rows[0]["icd"].ToString();
                                    Birthcardvadiag.LoadBirthPage(CurrentInpatient.NoOfFirstPage.ToString(), "2", "1");
                                    Birthcardvadiag.StartPosition = FormStartPosition.CenterParent;
                                    Birthcardvadiag.ShowDialog();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 验证是否需要填写脑卒中报告卡
        /// add by ywk 2013年8月15日 22:56:46
        /// </summary>
        /// <param name="sqlText"></param>
        private void GetCardiovasular(string sqlText, Iem_Mainpage_Basicinfo Iem_Mainpagebasic)
        {
            try
            {
                DataTable table = m_App.SqlHelper.ExecuteDataTable(sqlText, new SqlParameter[] { new SqlParameter("@iem_mainpage_no", Iem_Mainpagebasic.Iem_Mainpage_NO) }, CommandType.Text);
                if (table != null && table.Rows.Count > 0)
                {
                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人出院诊断符合心脑血管病病例报告卡上报条件，是否立即填报？", "心脑血管病病上报", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //ReportCardDialog reportOperateDialog = new ReportCardDialog(m_App, Iem_Mainpagebasic.Iem_Mainpage_NO, table, Iem_ Mainpagebasic.NoOfInpat);
                        //reportOperateDialog.m_diagicd10 = table.Rows[0]["icd"].ToString();
                        //reportOperateDialog.LoadPage(CurrentInpatient.NoOfFirstPage.ToString(), "2", "1");
                        //reportOperateDialog.CheckedSick();
                        //reportOperateDialog.ShowDialog();
                        CardiovascularDialog cardvadiag = new CardiovascularDialog(m_App, Iem_Mainpagebasic.NoOfInpat);
                        cardvadiag.icd10 = table.Rows[0]["icd"].ToString();
                        cardvadiag.LoadChildPage(CurrentInpatient.NoOfFirstPage.ToString(), "2", "1");
                        cardvadiag.StartPosition = FormStartPosition.CenterParent;
                        cardvadiag.ShowDialog();
                    }
                }


            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 根据传进的SQL弹出填写报告卡的界面
        /// add by ywk 2013年8月15日 22:38:30
        /// </summary>
        /// <param name="sql"></param>
        private void GoTheriomareportTip(string sql, Iem_Mainpage_Basicinfo Iem_Mainpagebasic)
        {
            try
            {
                DataTable table = m_App.SqlHelper.ExecuteDataTable(sql, new SqlParameter[] { new SqlParameter("@iem_mainpage_no", Iem_Mainpagebasic.Iem_Mainpage_NO) }, CommandType.Text);
                if (table != null && table.Rows.Count > 0)
                {
                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人出院诊断符合肿瘤病上报条件，是否立即填报？", "肿瘤病上报", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ReportCardDialog reportOperateDialog = new ReportCardDialog(m_App, Iem_Mainpagebasic.Iem_Mainpage_NO, table, Iem_Mainpagebasic.NoOfInpat);
                        reportOperateDialog.m_diagicd10 = table.Rows[0]["icd"].ToString();
                        reportOperateDialog.LoadPage(CurrentInpatient.NoOfFirstPage.ToString(), "2", "1");
                        reportOperateDialog.CheckedSick();
                        reportOperateDialog.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }

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

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-20</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDiagnose_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增手术
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_addOperation_Click(object sender, EventArgs e)
        {
            try
            {
                IemNewOperInfo m_OperInfoFrom = new IemNewOperInfo(m_App, "new", null);
                m_OperInfoFrom.ShowDialog();
                if (m_OperInfoFrom.DialogResult == DialogResult.OK)
                {
                    m_OperInfoFrom.IemOperInfo = null;
                    DataTable dataTable = m_OperInfoFrom.DataOper;

                    DataTable dataTableOper = new DataTable();
                    if (this.gridControl2.DataSource != null)
                    {
                        dataTableOper = this.gridControl2.DataSource as DataTable;
                    }
                    if (dataTableOper.Rows.Count == 0)
                    {
                        dataTableOper = dataTable.Clone();
                    }
                    DataRow newRow = dataTableOper.NewRow();
                    DataRow rows = dataTable.Rows[0];
                    foreach (DataColumn item in dataTableOper.Columns)
                    {
                        if (dataTable.Columns.Contains(item.ColumnName))
                        {
                            newRow[item.ColumnName] = rows[item.ColumnName].ToString();
                        }
                    }
                    dataTableOper.Rows.Add(newRow);
                    gridControl2.BeginUpdate();
                    this.gridControl2.DataSource = dataTableOper;

                    gridControl2.EndUpdate();
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑手术
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_editOperation_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewOper.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选中一条记录");
                    return;
                }
                DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                if (dataRow == null)
                {
                    MessageBox.Show("请选中一条记录");
                    return;
                }
                DataTable dataTableOper = this.gridControl2.DataSource as DataTable;
                DataTable dataTable = new DataTable();
                dataTable = dataTableOper.Clone();
                dataTable.ImportRow(dataRow);

                IemNewOperInfo m_OperInfoFrom = new IemNewOperInfo(m_App, "edit", dataTable);
                if (m_OperInfoFrom.ShowDialog() == DialogResult.OK)
                {
                    m_OperInfoFrom.IemOperInfo = null;
                    DataRow rows = m_OperInfoFrom.DataOper.Rows[0];
                    foreach (DataColumn item in dataRow.Table.Columns)
                    {
                        if (m_OperInfoFrom.DataOper.Columns.Contains(item.ColumnName))
                        {
                            dataRow[item.ColumnName] = rows[item.ColumnName].ToString();
                        }
                    }
                    gridControl2.BeginUpdate();
                    this.gridControl2.DataSource = dataTableOper;

                    gridControl2.EndUpdate();
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除手术
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_deleteOperation_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewOper.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                if (dataRow == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }

                DataTable dataTableOper = this.gridControl2.DataSource as DataTable;
                dataTableOper.Rows.Remove(dataRow);

                this.gridControl2.BeginUpdate();
                this.gridControl2.DataSource = dataTableOper;
                this.gridControl2.EndUpdate();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除手术 --- 右键
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_deleteOperation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                btn_deleteOperation_Click(null, null);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 右键 --- 手术列表
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-21</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl2_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    GridHitInfo hitInfo = gridViewOper.CalcHitInfo(gridControl2.PointToClient(Cursor.Position));
                    if (hitInfo.RowHandle < 0)
                    {
                        return;
                    }
                    if (gridViewOper.FocusedRowHandle < 0)
                    {
                        return;
                    }
                    else
                    {
                        DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                        this.popupMenu2.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// Enter事件 --- 获取焦点选中内容
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-23</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Enter(object sender, EventArgs e)
        {
            try
            {
                DS_Common.txt_Enter(sender);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-20</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewOper_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
