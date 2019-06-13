using DevExpress.XtraEditors;
using DrectSoft.Common.Eop;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Convertmy = DrectSoft.Core.UtilsForExtension;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class UCIemDiagnoseEn : UserControl
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
        public UCIemDiagnoseEn(IEmrHost m_app)
        {
            InitializeComponent();
            m_App = m_app;
            m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            InitLookUpEditor();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCIemDiagnoseEn_Load(object sender, EventArgs e)
        {
            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();
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
            BindLueData(lueHurt_Toxicosis_Ele, 17);
            //BindLueData(lueZymosisName, 12);
        }

        /// <summary>
        /// 绑定下拉框
        /// </summary>
        private void BindEmployeeData()
        {
            //************所有操作人员*****************
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            DataTable dataTable = AddTableColumn(m_SqlHelper.ExecuteDataTable(" select ID, Name, Py, Memo from Users where  Valid = 1  and deptid='" + m_App.User.CurrentDeptId + "'  order by ID "));
            //GetEditroData(11);

            Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
            columnwidth.Add("名称", lueKszr.Width);
            SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

            //************护理操作人员sqlWordBook1*****************
            LookUpWindow lupInfo1 = new LookUpWindow();
            lupInfo1.SqlHelper = m_SqlHelper;
            DataTable dataTable1 = AddTableColumn(m_SqlHelper.ExecuteDataTable(" select ID, Name, Py, Memo from Users where  Valid = 1 and (category = '402'  ) and deptid='" + m_App.User.CurrentDeptId + "'  order by ID "));
            //GetEditroData(111);

            Dictionary<string, int> columnwidth1 = new Dictionary<String, Int32>();
            columnwidth1.Add("名称", lueKszr.Width);
            SqlWordbook sqlWordBook1 = new SqlWordbook("ID", dataTable1, "ID", "Name", columnwidth, true);

            //************非护理操作人员sqlWordBook2*****************
            LookUpWindow lupInfo2 = new LookUpWindow();
            lupInfo2.SqlHelper = m_SqlHelper;
            DataTable dataTable2 = AddTableColumn(m_SqlHelper.ExecuteDataTable(" select ID, Name, Py, Memo from Users where  Valid = 1 and (category = '400' or category = '401' or category = '403' ) and deptid='" + m_App.User.CurrentDeptId + "'  order by ID "));
            //  GetEditroData(112);

            Dictionary<string, int> columnwidth2 = new Dictionary<String, Int32>();
            columnwidth2.Add("名称", lueKszr.Width);
            SqlWordbook sqlWordBook2 = new SqlWordbook("ID", dataTable2, "ID", "Name", columnwidth2, true);

            lueKszr.SqlWordbook = sqlWordBook2;
            lueKszr.ListWindow = lupInfo2;

            lueZrys.SqlWordbook = sqlWordBook2;
            lueZrys.ListWindow = lupInfo2;

            lueZzys.SqlWordbook = sqlWordBook2;
            lueZzys.ListWindow = lupInfo2;

            lueZyys.SqlWordbook = sqlWordBook2;
            lueZyys.ListWindow = lupInfo2;

            lueDuty_Nurse.SqlWordbook = sqlWordBook1;
            lueDuty_Nurse.ListWindow = lupInfo1;

            luejxys.SqlWordbook = sqlWordBook2;
            luejxys.ListWindow = lupInfo2;

            lueSxys.SqlWordbook = sqlWordBook2;
            lueSxys.ListWindow = lupInfo2;

            lueBmy.SqlWordbook = sqlWordBook2;
            lueBmy.ListWindow = lupInfo2;

            lueZkys.SqlWordbook = sqlWordBook2;
            lueZkys.ListWindow = lupInfo2;

            lueZkhs.SqlWordbook = sqlWordBook1;
            lueZkhs.ListWindow = lupInfo1;

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
            DataTable dataTableOper = m_IemInfo.IemDiagInfo.OutDiagTable;
            this.gridControl1.DataSource = null;
            this.gridControl1.BeginUpdate();
            if (dataTableOper.Select("(Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8') and Type = '1'").Length != 0)
                this.gridControl1.DataSource = dataTableOper.Select("(Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8') and Type = '1'").CopyToDataTable();
            this.gridControl1.EndUpdate();

            //this.gridControl2.DataSource = null;
            //this.gridControl2.BeginUpdate();
            //if (dataTableOper.Select("(Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8') and Type = '2'").Length != 0)
            //    this.gridControl2.DataSource = dataTableOper.Select("(Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8') and Type = '2'").CopyToDataTable();
            //this.gridControl2.EndUpdate();
            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);
            //m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose2);

            lueHurt_Toxicosis_Ele.CodeValue = m_IemInfo.IemDiagInfo.Hurt_Toxicosis_ElementID;

            txtPathologyName.Text = m_IemInfo.IemDiagInfo.Pathology_Diagnosis_Name;
            txtPathologyID.Text = m_IemInfo.IemDiagInfo.Pathology_Diagnosis_ID;
            txtPathologySn.Text = m_IemInfo.IemDiagInfo.Pathology_Observation_Sn;


            txtAllergicDrug.Text = m_IemInfo.IemDiagInfo.Allergic_Drug;
            if (m_IemInfo.IemDiagInfo.Allergic_Flag == "1")
                chkAllergic1.Checked = true;//无过敏药物
            else if (m_IemInfo.IemDiagInfo.Allergic_Flag == "2")
                chkAllergic2.Checked = true;//有过敏药物

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


            //foreach (DataRow im in dataTableOper.Rows)
            //{
            //    if (im["Diagnosis_Type_Id"].ToString() == "13")
            //        this.lueOutDiag.CodeValue = im["Diagnosis_Code"].ToString() == "" ? "" : im["Diagnosis_Code"].ToString();
            //    //else if (im["Diagnosis_Type_Id"].ToString() == "2")
            //    //    this.lueInDiag.CodeValue = im["Diagnosis_Code"].ToString() == "" ? "" : im["Diagnosis_Code"].ToString();
            //}


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
            //}
            #endregion
        }

        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {

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
            {
                m_IemInfo.IemDiagInfo.Allergic_Flag = "";//为空
            }

            if (chkAutopsy1.Checked)
                m_IemInfo.IemBasicInfo.Autopsy_Flag = "1";
            else if (chkAutopsy2.Checked)
                m_IemInfo.IemBasicInfo.Autopsy_Flag = "2";
            else
            {
                m_IemInfo.IemBasicInfo.Autopsy_Flag = "";
            }

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
            {
                m_IemInfo.IemDiagInfo.BloodType = "";
            }

            if (chkRH1.Checked)
                m_IemInfo.IemDiagInfo.Rh = "1";
            else if (chkRH2.Checked)
                m_IemInfo.IemDiagInfo.Rh = "2";
            else if (chkRH3.Checked)
                m_IemInfo.IemDiagInfo.Rh = "3";
            else if (chkRH4.Checked)
                m_IemInfo.IemDiagInfo.Rh = "4";
            else
            {
                m_IemInfo.IemDiagInfo.Rh = "";
            }


            DataTable dt = m_IemInfo.IemDiagInfo.OutDiagTable;
            dt.Rows.Clear();
            //门(急)诊诊断
            //m_IemInfo.IemDiagInfo = new List<Iem_Mainpage_Diagnosis>();
            //if (!String.IsNullOrEmpty(this.lueOutDiag.CodeValue))
            //{
            //    DataRow imOut = dt.NewRow();
            //    //Iem_Mainpage_Diagnosis imOut = new Iem_Mainpage_Diagnosis();
            //    imOut["Diagnosis_Code"] = this.lueOutDiag.CodeValue;
            //    imOut["Diagnosis_Name"] = this.lueOutDiag.DisplayValue;
            //    imOut["Diagnosis_Type_Id"] = 13;
            //    //m_IemInfo.IemDiagInfo.Add(imOut);
            //    dt.Rows.Add(imOut);
            //    m_IemInfo.IemDiagInfo.OutDiagID = this.lueOutDiag.CodeValue;
            //    m_IemInfo.IemDiagInfo.OutDiagName = this.lueOutDiag.DisplayValue;
            //}
            #region 已修改  by xlb 2013-07-15 解决保存时表数据窜位置现象

            if (this.gridControl1.DataSource != null)
            {
                //出院诊断
                DataTable dataTable = this.gridControl1.DataSource as DataTable;
                //DataTable dataTable2 = this.gridControl2.DataSource as DataTable;
                DataTable dataTableNew = new DataTable();
                if (dataTable != null)
                {
                    //复制西医表结构
                    dataTableNew = dataTable.Clone();
                }
                //else if (dataTable2 != null)
                //{
                //    //复制中医表结构
                //    dataTableNew = dataTable2.Clone();
                //}
                if (dataTable != null)
                {
                    //中西医表都不为NULL则合并量表数据
                    dataTableNew.Merge(dataTable);
                    //dataTableNew.Merge(dataTable2);

                }
                //else if (dataTable == null)
                //{
                //    //否则复制不为NULL的表
                //    dataTableNew = dataTable2;
                //}
                //else if (dataTable2 == null)
                //{
                //    dataTableNew = dataTable;
                //}
                int XYindex = 0;//西医数据集排序码
                int ZYindex = 0;//中医数据集排序码
                for (int i = 0; i < dataTableNew.Rows.Count; i++)
                {
                    DataRow row = dataTableNew.Rows[i];

                    DataRow imOut = dt.NewRow();
                    imOut["Diagnosis_Code"] = row["Diagnosis_Code"].ToString();
                    imOut["Diagnosis_Name"] = row["Diagnosis_Name"].ToString();
                    if (row["Type"].ToString() == "1")//西医
                    {
                        //默认中西医诊断第一条为主要诊断
                        if (XYindex == 0)
                        {
                            imOut["Diagnosis_Type_Id"] = 7;//7表示主要诊断
                        }
                        else
                        {
                            imOut["Diagnosis_Type_Id"] = 8;//8表示其他诊断
                        }
                        XYindex++;
                    }
                    else if (row["Type"].ToString() == "2")//中医
                    {
                        //默认中西医诊断第一条为主要诊断
                        if (ZYindex == 0)
                        {
                            imOut["Diagnosis_Type_Id"] = 7;//7表示主要诊断
                        }
                        else
                        {
                            imOut["Diagnosis_Type_Id"] = 8;//8表示其他诊断
                        }
                        ZYindex++;
                    }
                    //if (i == 0)
                    //{
                    //    imOut["Diagnosis_Type_Id"] = 7;//7表示主要诊断
                    //}
                    //else
                    //{
                    //    imOut["Diagnosis_Type_Id"] = 8;//8表示其他诊断
                    //}
                    imOut["Status_Id"] = Convertmy.ToDecimal(row["Status_Id"]);
                    imOut["Status_Name"] = row["Status_Name"];
                    imOut["Order_Value"] = i + 1;
                    imOut["Type"] = row["Type"];
                    imOut["TypeName"] = row["TypeName"];
                    dt.Rows.Add(imOut);
                }
            }

            #endregion
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
            try
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
            catch (Exception ex)
            {
                throw ex;
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
                m_DiagInfoForm = new IemNewDiagInfoForm(m_App, "add", "", "", "", "");
                m_DiagInfoForm.ShowDialog();
                if (m_DiagInfoForm.DialogResult == DialogResult.OK)
                {
                    m_DiagInfoForm.IemOperInfo = null;
                    DataTable dataTable = m_DiagInfoForm.DataOper;

                    if (m_DiagInfoForm.DiagnosisType == "1")
                    {
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
                    else if (m_DiagInfoForm.DiagnosisType == "2")
                    {
                        DataTable dataTableOper = new DataTable();
                        //if (this.gridControl2.DataSource != null)
                        //    dataTableOper = this.gridControl2.DataSource as DataTable;
                        if (dataTableOper.Rows.Count == 0)
                            dataTableOper = dataTable.Clone();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            dataTableOper.ImportRow(row);
                        }
                        //this.gridControl2.BeginUpdate();
                        //this.gridControl2.DataSource = dataTableOper;
                        //m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose2);
                        //this.gridControl2.EndUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑西医出院诊断
        /// add by ywk 2012年6月4日 13:57:11
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditOutDiag_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDiagnose.FocusedRowHandle < 0)
                    return;
                DataRow dataRow = gridViewDiagnose.GetDataRow(gridViewDiagnose.FocusedRowHandle);
                if (dataRow == null)
                    return;
                string diagcode = dataRow["Diagnosis_Code"].ToString();//诊断的ICD编码
                string diagname = dataRow["Diagnosis_Name"].ToString();
                string statusid = dataRow["Status_Id"].ToString();//诊断结果（入院病情)
                string diagtype = "xiyi";
                m_DiagInfoForm = new IemNewDiagInfoForm(m_App, "edit", diagcode, diagname, statusid, diagtype);
                if (m_DiagInfoForm.ShowDialog() == DialogResult.OK)
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
                    //遍历选中行所在表的列若返回表中有该行则更新选中行的该列
                    foreach (DataColumn item in dataRow.Table.Columns)
                    {
                        DataRow rowOper = dataTable.Rows[0];
                        if (dataTable.Columns.Contains(item.ColumnName))
                        {
                            dataRow[item.ColumnName] = rowOper[item.ColumnName].ToString();
                        }
                    }
                    this.gridControl1.BeginUpdate();
                    this.gridControl1.DataSource = dataTableOper;
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);
                    this.gridControl1.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑中医出院诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnEditOutDiagCHn_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (gridViewDiagnose2.FocusedRowHandle < 0)
        //            return;
        //        DataRow dataRow = gridViewDiagnose2.GetDataRow(gridViewDiagnose2.FocusedRowHandle);
        //        if (dataRow == null)
        //            return;
        //        string diagcode = dataRow["Diagnosis_Code"].ToString();//诊断的ICD编码
        //        string diagname = dataRow["Diagnosis_Name"].ToString();
        //        string statusid = dataRow["Status_Id"].ToString();//诊断结果（入院病情)
        //        string diagtype = "zhongyi";
        //        m_DiagInfoForm = new IemNewDiagInfoForm(m_App, "edit", diagcode, diagname, statusid, diagtype);
        //        m_DiagInfoForm.ShowDialog();
        //        if (m_DiagInfoForm.DialogResult == DialogResult.OK)
        //        {
        //            m_DiagInfoForm.IemOperInfo = null;
        //            DataTable dataTable = m_DiagInfoForm.DataOper;
        //            DataTable dataTableOper = new DataTable();
        //            if (this.gridControl2.DataSource != null)
        //            {
        //                dataTableOper = this.gridControl2.DataSource as DataTable;
        //            }
        //            if (dataTableOper.Rows.Count == 0)
        //            {
        //                dataTableOper = dataTable.Clone();
        //            }
        //            //遍历选中行所在表的列若返回表中有该行则更新选中行的该列
        //            foreach (DataColumn item in dataRow.Table.Columns)
        //            {
        //                DataRow rowOper = dataTable.Rows[0];
        //                if (dataTable.Columns.Contains(item.ColumnName))
        //                {
        //                    dataRow[item.ColumnName] = rowOper[item.ColumnName].ToString();
        //                }
        //            }
        //            this.gridControl2.BeginUpdate();
        //            this.gridControl2.DataSource = dataTableOper;
        //            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose2);
        //            this.gridControl2.EndUpdate();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void UCIemDiagnoseEn_Paint(object sender, PaintEventArgs e)
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
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
            //((ShowUC)this.Parent).Close(true, m_IemInfo);
            //点击确认按钮就将数据更新到数据库
            CurrentInpatient = m_App.CurrentPatientInfo;
            CurrentInpatient.ReInitializeAllProperties();
            IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
            manger.SaveData(m_IemInfo);
            btn_Close_Click(sender, e);
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
            try
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
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void btn_down_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        //private void btn_up2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable dataTable = (DataTable)gridControl2.DataSource;
        //        int index = 0;
        //        if (gridViewDiagnose2.FocusedRowHandle < 1)
        //            return;
        //        else
        //        {
        //            DataTable dt = dataTable.Clone();
        //            for (int i = 0; i < dataTable.Rows.Count; i++)
        //            {
        //                if (i == gridViewDiagnose2.FocusedRowHandle - 1)
        //                {
        //                    dt.ImportRow(dataTable.Rows[i + 1]);
        //                }
        //                else if (i == gridViewDiagnose2.FocusedRowHandle)
        //                    dt.ImportRow(dataTable.Rows[i - 1]);
        //                else
        //                    dt.ImportRow(dataTable.Rows[i]);
        //            }
        //            index = gridViewDiagnose2.FocusedRowHandle - 1;

        //            this.gridControl2.BeginUpdate();
        //            this.gridControl2.DataSource = dt;
        //            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose2);
        //            this.gridControl2.EndUpdate();

        //            gridViewDiagnose2.FocusedRowHandle = index;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
        //    }
        //}

        //private void btn_down2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DataTable dataTable = (DataTable)gridControl2.DataSource;

        //        int index = 0;
        //        if (gridViewDiagnose2.FocusedRowHandle < 0)
        //            return;
        //        else if (gridViewDiagnose2.FocusedRowHandle == dataTable.Rows.Count - 1)
        //            return;
        //        else
        //        {
        //            DataTable dt = dataTable.Clone();
        //            for (int i = 0; i < dataTable.Rows.Count; i++)
        //            {
        //                if (i == gridViewDiagnose2.FocusedRowHandle + 1)
        //                {
        //                    dt.ImportRow(dataTable.Rows[i - 1]);
        //                }
        //                else if (i == gridViewDiagnose2.FocusedRowHandle)
        //                    dt.ImportRow(dataTable.Rows[i + 1]);
        //                else
        //                    dt.ImportRow(dataTable.Rows[i]);
        //            }

        //            index = gridViewDiagnose2.FocusedRowHandle + 1;
        //            this.gridControl2.BeginUpdate();
        //            this.gridControl2.DataSource = dt;
        //            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose2);
        //            this.gridControl2.EndUpdate();

        //            gridViewDiagnose2.FocusedRowHandle = index;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
        //    }

        //}

        //private void btn_del2_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (gridViewDiagnose2.FocusedRowHandle < 0)
        //            return;
        //        else
        //        {

        //            DataRow dataRow = gridViewDiagnose2.GetDataRow(gridViewDiagnose2.FocusedRowHandle);
        //            if (dataRow == null)
        //                return;

        //            DataTable dataTableOper = this.gridControl2.DataSource as DataTable;

        //            dataTableOper.Rows.Remove(dataRow);

        //            this.gridControl2.BeginUpdate();
        //            this.gridControl2.DataSource = dataTableOper;
        //            this.gridControl2.EndUpdate();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
        //    }
        //}
        /// <summary>
        /// 复选框选中后可右键取消选中
        /// add by ywk 2012年7月30日 08:43:05 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMedicalQuality3_Click(object sender, EventArgs e)
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

    }
}
