using DevExpress.XtraEditors;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.DSSqlHelper;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Convertmy = DrectSoft.Core.UtilsForExtension;

namespace DrectSoft.Core.IEMMainPageZY
{
    /// <summary>
    /// 诊断编辑界面
    /// </summary>
    public partial class UCIemDiagnose : UserControl
    {
        IDataAccess m_SqlHelper;
        IDrectSoftLog m_Logger;
        private IEmrHost m_App;

        private DataTable m_DataTableDiag = null;

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

        public UCIemDiagnose()
        {
            InitializeComponent();

            m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            InitLookUpEditor();
        }

        private void UCIemDiagnose_Load(object sender, EventArgs e)
        {
            try
            {
                //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
                //InitLookUpEditor();

                //HideSbutton();
                this.ActiveControl = btnNewOutDiag;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 初始化下拉框

        private void InitLookUpEditor()
        {
            try
            {
                InitLueDiagnose();
                BindEmployeeData();
                InitDialogData(lookUpEditPathologyName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitLueDiagnose()
        {
            //BindLueData(lueInDiag, 12); 
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

        /// <summary>
        /// 初始化控件数据源
        /// <auth>XLB</auth>
        /// <date>2013-05-23</date>>
        /// </summary>
        /// <param name="lookUpEdit"></param>
        private void InitDialogData(LookUpEditor lookUpEdit)
        {
            try
            {
                LookUpWindow lookUpWindow = new LookUpWindow();
                lookUpEdit.ListWindow = lookUpWindow;
                lookUpEdit.Kind = WordbookKind.Sql;
                DataTable dt = GetDialogData();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (dt.Columns[i].ColumnName.ToUpper().Trim() == "ICD")
                    {
                        dt.Columns[i].Caption = "编码";
                    }
                    else if (dt.Columns[i].ColumnName.ToUpper().Trim() == "NAME")
                    {
                        dt.Columns[i].Caption = "名称";
                    }
                }

                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                dictionary.Add("ICD", 85);
                dictionary.Add("NAME", 130);
                SqlWordbook sqlWordBook = new SqlWordbook("Dialog", dt, "ICD", "NAME", dictionary, "ICD//NAME//PY//WB");
                lookUpEdit.SqlWordbook = sqlWordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 抓取病历诊断编码
        /// <auth>XLB</auth>
        /// <adte>2013-05-27</adte>
        /// </summary>
        private DataTable GetDialogData()
        {
            try
            {
                string sql = "select ICD,NAME,PY,WB from  diagnosis_xt_bj where valid='1'";
                DataTable dtDialog = YD_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                return dtDialog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region private methods

        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
            //if (m_DataTableDiag == null)
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

            this.gridControl2.DataSource = null;
            this.gridControl2.BeginUpdate();
            if (dataTableOper.Select("(Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8') and Type = '2'").Length != 0)
                this.gridControl2.DataSource = dataTableOper.Select("(Diagnosis_Type_Id = '7' or Diagnosis_Type_Id = '8') and Type = '2'").CopyToDataTable();
            this.gridControl2.EndUpdate();
            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);
            m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose2);

            lueHurt_Toxicosis_Ele.CodeValue = m_IemInfo.IemDiagInfo.Hurt_Toxicosis_ElementID;

            lookUpEditPathologyName.Text = m_IemInfo.IemDiagInfo.Pathology_Diagnosis_Name;
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
            if (m_IemInfo.IemDiagInfo.IemInpatOutHosStatus == "0")
            {
                chkOutZY.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInpatOutHosStatus == "1")
            {
                chkOutHZ.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInpatOutHosStatus == "2")
            {
                chkOutWY.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInpatOutHosStatus == "3")
            {
                chkOutDead.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInpatOutHosStatus == "4")
            {
                chkOutOthers.Checked = true;
            }
            if (m_IemInfo.IemDiagInfo.IemDieaseYNZZ == "1")
            {
                chkYNZZS.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemDieaseYNZZ == "0")
            {
                chkYNZZF.Checked = true;
            }
            if (m_IemInfo.IemDiagInfo.IemMZYOutHos == "0")
            {
                chkMZYCY0.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemMZYOutHos == "1")
            {
                chkMZYCY1.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemMZYOutHos == "2")
            {
                chkMZYCY2.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemMZYOutHos == "3")
            {
                chkMZYCY3.Checked = true;
            }
            if (m_IemInfo.IemDiagInfo.IemInHosYOutHos == "0")
            {
                chkRYYCY0.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInHosYOutHos == "1")
            {
                chkRYYCY1.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInHosYOutHos == "2")
            {
                chkRYYCY2.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInHosYOutHos == "3")
            {
                chkRYYCY3.Checked = true;
            }
            if (m_IemInfo.IemDiagInfo.IemLCYBL == "0")
            {
                chkLCYBL0.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemLCYBL == "1")
            {
                chkLCYBL1.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInHosYOutHos == "2")
            {
                chkLCYBL2.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInHosYOutHos == "3")
            {
                chkLCYBL3.Checked = true;
            }
            if (m_IemInfo.IemDiagInfo.IemFSYBL == "0")
            {
                chkFSYBL0.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemFSYBL == "1")
            {
                chkFSYBL1.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemFSYBL == "2")
            {
                chkFSYBL2.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemFSYBL == "3")
            {
                ChkFSYBL3.Checked = true;
            }
            //抢救次数
            txtQJCS.Text = m_IemInfo.IemDiagInfo.IemQJCS;
            //成功次数
            txtSuccessCS.Text = m_IemInfo.IemDiagInfo.IemSuccessTimes;
            if (m_IemInfo.IemDiagInfo.IemInpatLY == "1")
            {
                chkInpLY1.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInpatLY == "2")
            {
                chkInpLY2.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInpatLY == "3")
            {
                chkInpLY3.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInpatLY == "4")
            {
                chkInpLY4.Checked = true;
            }
            else if (m_IemInfo.IemDiagInfo.IemInpatLY == "5")
            {
                chkInpLY5.Checked = true;
            }
        }

        /// <summary>
        /// GET UI
        /// Modify by xlb 2013-07-09 
        /// 解决保存界面数据集篡位以及主要诊断诊断类型用了其他诊断的类型
        /// </summary>
        private void GetUI()
        {

            m_IemInfo.IemDiagInfo.Hurt_Toxicosis_ElementID = lueHurt_Toxicosis_Ele.CodeValue;
            m_IemInfo.IemDiagInfo.Hurt_Toxicosis_Element = lueHurt_Toxicosis_Ele.Text;

            m_IemInfo.IemDiagInfo.Pathology_Diagnosis_Name = lookUpEditPathologyName.Text;
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

            if (this.gridControl1.DataSource != null || this.gridControl2.DataSource != null)
            {
                //出院诊断
                DataTable dataTable = this.gridControl1.DataSource as DataTable;
                DataTable dataTable2 = this.gridControl2.DataSource as DataTable;
                DataTable dataTableNew = new DataTable();
                if (dataTable != null)
                {
                    //复制西医表结构
                    dataTableNew = dataTable.Clone();
                }
                else if (dataTable2 != null)
                {
                    //复制中医表结构
                    dataTableNew = dataTable2.Clone();
                }
                if (dataTable != null && dataTable2 != null)
                {
                    //中西医表都不为NULL则合并量表数据
                    dataTableNew.Merge(dataTable);
                    dataTableNew.Merge(dataTable2);

                }
                else if (dataTable == null)
                {
                    //否则复制不为NULL的表
                    dataTableNew = dataTable2;
                }
                else if (dataTable2 == null)
                {
                    dataTableNew = dataTable;
                }
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
            {
                m_IemInfo.IemDiagInfo.Quality_Control_Date = deZkDate.DateTime.ToString("yyyy-MM-dd");//+ " " + teZkDate.Time.ToString("HH:mm:ss");
                m_IemInfo.IemDiagInfo.Quality_Control_DateYear = deZkDate.DateTime.ToString("yyyy");
                m_IemInfo.IemDiagInfo.Quality_Control_DateMonth = deZkDate.DateTime.ToString("MM");
                m_IemInfo.IemDiagInfo.Quality_Control_DateDay = deZkDate.DateTime.ToString("dd");
            }
            if (chkOutZY.Checked)
            {
                //治愈
                m_IemInfo.IemDiagInfo.IemInpatOutHosStatus = "0";
            }
            else if (chkOutHZ.Checked)
            {
                //好转
                m_IemInfo.IemDiagInfo.IemInpatOutHosStatus = "1";
            }
            else if (chkOutWY.Checked)
            {
                //未愈
                m_IemInfo.IemDiagInfo.IemInpatOutHosStatus = "2";
            }
            else if (chkOutDead.Checked)
            {
                //死亡
                m_IemInfo.IemDiagInfo.IemInpatOutHosStatus = "3";
            }
            else if (chkOutOthers.Checked)
            {
                //其他
                m_IemInfo.IemDiagInfo.IemInpatOutHosStatus = "4";
            }
            //疾病是否疑难杂症1是0不是
            m_IemInfo.IemDiagInfo.IemDieaseYNZZ = chkYNZZS.Checked ? "1" : "0";
            if (chkMZYCY0.Checked)
            {
                //门诊与出院未作
                m_IemInfo.IemDiagInfo.IemMZYOutHos = "0";
            }
            else if (chkMZYCY1.Checked)
            {
                //门诊与出院符合
                m_IemInfo.IemDiagInfo.IemMZYOutHos = "1";
            }
            else if (chkMZYCY2.Checked)
            {
                //门诊与出院不符合
                m_IemInfo.IemDiagInfo.IemMZYOutHos = "2";
            }
            else if (chkMZYCY3.Checked)
            {
                //门诊与出院不肯定
                m_IemInfo.IemDiagInfo.IemMZYOutHos = "3";
            }
            if (chkRYYCY0.Checked)
            {
                //入院与出院未作
                m_IemInfo.IemDiagInfo.IemInHosYOutHos = "0";
            }
            else if (chkRYYCY1.Checked)
            {
                //入院与出院符合
                m_IemInfo.IemDiagInfo.IemInHosYOutHos = "1";
            }
            else if (chkRYYCY2.Checked)
            {
                //入院与出院不符合
                m_IemInfo.IemDiagInfo.IemInHosYOutHos = "2";
            }
            else if (chkRYYCY3.Checked)
            {
                //入院与出院不肯定
                m_IemInfo.IemDiagInfo.IemInHosYOutHos = "3";
            }
            if (chkLCYBL0.Checked)
            {
                //临床与病理未作
                m_IemInfo.IemDiagInfo.IemLCYBL = "0";
            }
            else if (chkLCYBL1.Checked)
            {
                //临床与病理符合
                m_IemInfo.IemDiagInfo.IemLCYBL = "1";
            }
            else if (chkLCYBL2.Checked)
            {
                //临床与病理不符合
                m_IemInfo.IemDiagInfo.IemLCYBL = "2";
            }
            else if (chkLCYBL3.Checked)
            {
                //临床与病理不肯定
                m_IemInfo.IemDiagInfo.IemLCYBL = "3";
            }
            if (chkFSYBL0.Checked)
            {
                //反射与病理未作
                m_IemInfo.IemDiagInfo.IemFSYBL = "0";
            }
            else if (chkFSYBL1.Checked)
            {
                //反射与病理符合
                m_IemInfo.IemDiagInfo.IemFSYBL = "1";
            }
            else if (chkFSYBL2.Checked)
            {
                //放射与病理不符合
                m_IemInfo.IemDiagInfo.IemFSYBL = "2";
            }
            else if (ChkFSYBL3.Checked)
            {
                //放射与病理不肯定
                m_IemInfo.IemDiagInfo.IemFSYBL = "3";
            }
            if (chkInpLY1.Checked)
            {
                //病人来源本市区、区县
                m_IemInfo.IemDiagInfo.IemInpatLY = "1";
            }
            else if (chkInpLY2.Checked)
            {
                //病人来源本省其他县市
                m_IemInfo.IemDiagInfo.IemInpatLY = "2";
            }
            else if (chkInpLY3.Checked)
            {
                //病人来源外省
                m_IemInfo.IemDiagInfo.IemInpatLY = "3";
            }
            else if (chkInpLY4.Checked)
            {
                //病人来源港澳台地区
                m_IemInfo.IemDiagInfo.IemInpatLY = "4";
            }
            else if (chkInpLY5.Checked)
            {
                //病人来源国外
                m_IemInfo.IemDiagInfo.IemInpatLY = "5";
            }
            //抢救次数
            m_IemInfo.IemDiagInfo.IemQJCS = txtQJCS.Text;
            //成功次数
            m_IemInfo.IemDiagInfo.IemSuccessTimes = txtSuccessCS.Text;
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
                        {
                            dataTableOper = this.gridControl1.DataSource as DataTable;
                        }
                        if (dataTableOper.Rows.Count == 0)
                        {
                            dataTableOper = dataTable.Clone();
                        }
                        DataRow newRow = dataTableOper.NewRow();
                        foreach (DataColumn item in dataTableOper.Columns)
                        {
                            DataRow row = dataTable.Rows[0];
                            if (dataTable.Columns.Contains(item.ColumnName))
                            {
                                newRow[item.ColumnName] = row[item.ColumnName].ToString();
                            }
                        }
                        dataTableOper.Rows.Add(newRow);
                        //foreach (DataRow row in dataTable.Rows)
                        //{
                        //    dataTableOper.ImportRow(row);
                        //}
                        this.gridControl1.BeginUpdate();
                        this.gridControl1.DataSource = dataTableOper;
                        m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);
                        this.gridControl1.EndUpdate();
                    }
                    else if (m_DiagInfoForm.DiagnosisType == "2")
                    {
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
                        foreach (DataColumn item in dataTableOper.Columns)
                        {
                            DataRow row = dataTable.Rows[0];
                            if (dataTable.Columns.Contains(item.ColumnName))
                            {
                                newRow[item.ColumnName] = row[item.ColumnName].ToString();
                            }
                        }
                        dataTableOper.Rows.Add(newRow);
                        //foreach (DataRow row in dataTable.Rows)
                        //{
                        //    dataTableOper.ImportRow(row);
                        //}
                        this.gridControl2.BeginUpdate();
                        this.gridControl2.DataSource = dataTableOper;
                        m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose2);
                        this.gridControl2.EndUpdate();
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
                {
                    return;
                }
                DataRow dataRow = gridViewDiagnose.GetDataRow(gridViewDiagnose.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }
                string diagcode = dataRow["Diagnosis_Code"].ToString();//诊断的ICD编码
                string diagname = dataRow["Diagnosis_Name"].ToString();
                string statusid = dataRow["Status_Id"].ToString();//诊断结果（入院病情)
                string diagtype = "xiyi";
                m_DiagInfoForm = new IemNewDiagInfoForm(m_App, "edit", diagcode, diagname, statusid, diagtype);
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
                    foreach (DataColumn item in dataRow.Table.Columns)
                    {
                        DataRow row = dataTable.Rows[0];
                        if (dataTable.Columns.Contains(item.ColumnName))
                        {
                            dataRow[item.ColumnName] = row[item.ColumnName].ToString();
                        }
                    }
                    //foreach (DataRow row in dataTable.Rows)
                    //{
                    //    dataTableOper.Rows.Remove(dataRow);
                    //    dataTableOper.ImportRow(row);
                    //}
                    this.gridControl1.BeginUpdate();
                    this.gridControl1.DataSource = dataTableOper;
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose);
                    this.gridControl1.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 编辑中医出院诊断
        /// 解决位置错乱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditOutDiagCHn_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDiagnose2.FocusedRowHandle < 0)
                    return;
                DataRow dataRow = gridViewDiagnose2.GetDataRow(gridViewDiagnose2.FocusedRowHandle);
                if (dataRow == null)
                    return;
                string diagcode = dataRow["Diagnosis_Code"].ToString();//诊断的ICD编码
                string diagname = dataRow["Diagnosis_Name"].ToString();
                string statusid = dataRow["Status_Id"].ToString();//诊断结果（入院病情)
                string diagtype = "zhongyi";
                m_DiagInfoForm = new IemNewDiagInfoForm(m_App, "edit", diagcode, diagname, statusid, diagtype);
                m_DiagInfoForm.ShowDialog();
                if (m_DiagInfoForm.DialogResult == DialogResult.OK)
                {
                    m_DiagInfoForm.IemOperInfo = null;
                    DataTable dataTable = m_DiagInfoForm.DataOper;
                    DataTable dataTableOper = new DataTable();
                    if (this.gridControl2.DataSource != null)
                    {
                        dataTableOper = this.gridControl2.DataSource as DataTable;
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
                    //foreach (DataRow row in dataTable.Rows)
                    //{
                    //    dataTableOper.Rows.Remove(dataRow);
                    //    dataTableOper.ImportRow(row);
                    //}
                    this.gridControl2.BeginUpdate();
                    this.gridControl2.DataSource = dataTableOper;
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose2);
                    this.gridControl2.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        #region 注销 已重写控件解决重绘带来的闪屏等问题
        //private void UCIemDiagnose_Paint(object sender, PaintEventArgs e)
        //{
        //    foreach (Control control in this.Controls)
        //    {
        //        if (control is LabelControl)
        //        {
        //            control.Visible = false;
        //            e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

        //        }
        //        if (control is TextEdit)
        //        {
        //            e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
        //                new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
        //        }
        //    }

        //    //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
        //    //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
        //}
        #endregion

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
                    return;
                }
                DataRow dataRow = gridViewDiagnose.GetDataRow(gridViewDiagnose.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }
                if (MyMessageBox.Show("您确定删除吗？", "提示", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
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
                MyMessageBox.Show(ex.Message);
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

        /// <summary>
        /// <auth>Modify by xlb</auth>
        /// <date>2013-05-27</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            GetUI();
            //((ShowUC)this.Parent).Close(true, m_IemInfo);
            //点击确认按钮就将数据更新到数据库
            CurrentInpatient = m_App.CurrentPatientInfo;
            CurrentInpatient.ReInitializeAllProperties();
            IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
            manger.SaveData(m_IemInfo);
            GetReportVialde();

        }

        public void GetReportVialde()
        {
            try
            {

                IemMainPageManger m_IemMainPage = new IemMainPageManger(m_App, CurrentInpatient);
                if (m_IemMainPage != null)
                {
                    IemMainPageInfo IemInfo = m_IemMainPage.GetIemInfo();
                    if (IemInfo != null && IemInfo.IemBasicInfo != null)
                    {
                        //验证逻辑： 出院诊断列表中，存在属于传染病诊断的列表，且已经申报的次数小于该诊断设置的最大申报次数
                        string valueStr = DrectSoft.Service.YD_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                        string sqlText =
                            //                            @"select ie.iem_mainpage_no,z.icd,z.upcount,z.name from iem_mainpage_diagnosis_sx ie    
                            //                                left join zymosis_diagnosis z on ie.diagnosis_code=z.icd   
                            //                                left join iem_mainpage_basicinfo_sx imb on imb.iem_mainpage_no=ie.iem_mainpage_no  
                            //                               where z.CATEGORYID = 2 and z.valid=1 and ie.valide=1 and ie.iem_mainpage_no = @iem_mainpage_no  
                            //                                and (
                            //                                    select count(1) FROM THERIOMAREPORTCARD zr 
                            //                                     where zr.DIAGICD10=z.icd and zr.REPORT_NOOFINPAT=imb.noofinpat 
                            //                                       and zr.vaild=1 and zr.state != '7'
                            //                                    ) < z.upcount  
                            //                               group by ie.iem_mainpage_no,z.icd,z.upcount,z.name    
                            //                              having count(z.icd)>0 ";

                            @"select ie.iem_mainpage_no,z.icd,z.upcount,z.name from iem_mainpage_diagnosis_sx ie    
                                left join zymosis_diagnosis z on ie.diagnosis_code=z.icd and z.valid = 1
                                left join iem_mainpage_basicinfo_sx imb on imb.iem_mainpage_no=ie.iem_mainpage_no and imb.valide = 1
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

                        DataTable table = m_App.SqlHelper.ExecuteDataTable(sqlText, new SqlParameter[] { new SqlParameter("@iem_mainpage_no", IemInfo.IemBasicInfo.Iem_Mainpage_NO) }, CommandType.Text);
                        if (table != null && table.Rows.Count > 0)
                        {
                            if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人出院诊断符合肿瘤病上报条件，是否立即填报？", "肿瘤病上报", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                /* to zhouhui
                                ReportCardDialog reportOperateDialog = new ReportCardDialog(m_App, IemInfo.IemBasicInfo.Iem_Mainpage_NO, table, IemInfo.IemBasicInfo.NoOfInpat);
                                reportOperateDialog.m_diagicd10 = table.Rows[0]["icd"].ToString();
                                reportOperateDialog.LoadPage(CurrentInpatient.NoOfFirstPage.ToString(), "2", "1");
                                reportOperateDialog.ShowDialog();
                                 * **/
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }


        }


        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (this.gridViewDiagnose.RowCount > 0)
            {
                if (MyMessageBox.Show("您确定要删除吗？", "提示", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                btn_del_diag_ItemClick(null, null);
            }
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

        private void btn_up2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = (DataTable)gridControl2.DataSource;
                int index = 0;
                if (gridViewDiagnose2.FocusedRowHandle < 1)
                    return;
                else
                {
                    DataTable dt = dataTable.Clone();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (i == gridViewDiagnose2.FocusedRowHandle - 1)
                        {
                            dt.ImportRow(dataTable.Rows[i + 1]);
                        }
                        else if (i == gridViewDiagnose2.FocusedRowHandle)
                            dt.ImportRow(dataTable.Rows[i - 1]);
                        else
                            dt.ImportRow(dataTable.Rows[i]);
                    }
                    index = gridViewDiagnose2.FocusedRowHandle - 1;

                    this.gridControl2.BeginUpdate();
                    this.gridControl2.DataSource = dt;
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose2);
                    this.gridControl2.EndUpdate();

                    gridViewDiagnose2.FocusedRowHandle = index;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void btn_down2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dataTable = (DataTable)gridControl2.DataSource;

                int index = 0;
                if (gridViewDiagnose2.FocusedRowHandle < 0)
                    return;
                else if (gridViewDiagnose2.FocusedRowHandle == dataTable.Rows.Count - 1)
                    return;
                else
                {
                    DataTable dt = dataTable.Clone();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (i == gridViewDiagnose2.FocusedRowHandle + 1)
                        {
                            dt.ImportRow(dataTable.Rows[i - 1]);
                        }
                        else if (i == gridViewDiagnose2.FocusedRowHandle)
                            dt.ImportRow(dataTable.Rows[i + 1]);
                        else
                            dt.ImportRow(dataTable.Rows[i]);
                    }

                    index = gridViewDiagnose2.FocusedRowHandle + 1;
                    this.gridControl2.BeginUpdate();
                    this.gridControl2.DataSource = dt;
                    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewDiagnose2);
                    this.gridControl2.EndUpdate();

                    gridViewDiagnose2.FocusedRowHandle = index;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }

        }

        private void btn_del2_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDiagnose2.FocusedRowHandle < 0)
                {
                    return;
                }


                DataRow dataRow = gridViewDiagnose2.GetDataRow(gridViewDiagnose2.FocusedRowHandle);
                if (dataRow == null)
                {
                    return;
                }
                if (MyMessageBox.Show("您确定要删除吗？", "提示", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 复选框选中后可右键取消选中
        /// add by ywk 2012年7月30日 08:43:05 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMedicalQuality3_Click(object sender, EventArgs e)
        {
            try
            {
                //CheckEdit chkEdit = GetCheckEdit(((Control)sender).Name);
                //if (chkEdit.Checked)
                //{
                //    chkEdit.Checked = false;
                //}
                if (sender is CheckEdit)
                {
                    CheckEdit chkEdit = (CheckEdit)sender;
                    chkEdit.Checked = !chkEdit.Checked;
                }
                else if (sender is CheckBox)
                {
                    CheckBox chkBox = (CheckBox)sender;
                    chkBox.Checked = !chkBox.Checked;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
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
        /// 病理诊断选择后联动显示其对应编码
        /// <auth>xlb</auth>>
        /// <date>2013-05-27</date>>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditPathologyName_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtPathologyID.Text = lookUpEditPathologyName.CodeValue;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 复选框回车事件改变勾选状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAllergic1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    DS_Common.cbx_KeyPress(sender);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void labelControl49_Click(object sender, EventArgs e)
        {

        }

        private void lueBmy_CodeValueChanged(object sender, EventArgs e)
        {

        }
    }
}
