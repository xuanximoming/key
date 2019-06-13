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
    public partial class Print_UCIemDiagnose : UserControl
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
                return m_IemInfo;
            }
        }

        private IYidanEmrHost m_App;

        private DataTable m_DataTableDiag = null;
        public Print_UCIemDiagnose(IemMainPageInfo info, IYidanEmrHost app)
        {
            InitializeComponent();
            //m_IemInfo = info;
            //m_App = app;

            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;

        }

        private void Print_UCIemDiagnose_Load(object sender, EventArgs e)
        {
            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();
            //BindEmployeeData();
            //HideSbutton();

            //FillUI();

        }

        private void InitLookUpEditor()
        {
            InitLueDiagnose();
        }

        private void InitLueDiagnose()
        {
            BindLueData(lueInDiag, 12);
            BindLueData(lueOutDiag, 12);
        }

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
        public void FillUI()
        {
            //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
            FillUIInner();
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

        delegate void FillUIDelegate();
        private void FillUIInner()
        {
            IemMainPageInfo info = m_IemInfo;
            if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == 0)
            {
                //to do 病患基本信息
            }
            else
            {
                #region 将出院诊断信息加载到页面中
                DataTable dataTableOper = new DataTable();

                foreach (Iem_Mainpage_Diagnosis im in m_IemInfo.IemDiagInfo)
                {
                    if (m_DiagInfoForm == null)
                        m_DiagInfoForm = new IemNewDiagInfoForm(m_App);
                    if (im.Diagnosis_Type_Id == 7 || im.Diagnosis_Type_Id == 8)
                    {
                        m_DiagInfoForm.IemOperInfo = im;
                        DataTable dataTable = m_DiagInfoForm.DataOper;
                        if (dataTableOper.Rows.Count == 0)
                            dataTableOper = dataTable.Clone();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            dataTableOper.ImportRow(row);
                        }
                        //dataTableOper.AcceptChanges();
                    }

                }
                int i = 0;
                foreach (DataRow dr in dataTableOper.Rows)
                {
                    if (i == 0)
                    {
                        labDiagnosis_Name1.Text = dr["Diagnosis_Name"].ToString();
                        if (Convert.ToInt32(dr["Status_Id"].ToString()) == 1)
                            chkStatus_1_1.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 2)
                            chkStatus_2_1.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 3)
                            chkStatus_3_1.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 4)
                            chkStatus_4_1.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 5)
                            chkStatus_5_1.Visible = true;
                        labDiagnosis_Code1.Text = dr["Diagnosis_Code"].ToString();

                        labDiagnosis_Name1.Visible = true;
                        labDiagnosis_Code1.Visible = true;
                    }
                    else if (i == 1)
                    {
                        labDiagnosis_Name2.Text = dr["Diagnosis_Name"].ToString();
                        if (Convert.ToInt32(dr["Status_Id"].ToString()) == 1)
                            chkStatus_1_2.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 2)
                            chkStatus_2_2.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 3)
                            chkStatus_3_2.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 4)
                            chkStatus_4_2.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 5)
                            chkStatus_5_2.Visible = true;
                        labDiagnosis_Code2.Text = dr["Diagnosis_Code"].ToString();

                        labDiagnosis_Name2.Visible = true;
                        labDiagnosis_Code2.Visible = true;
                    }
                    else if (i == 2)
                    {
                        labDiagnosis_Name3.Text = dr["Diagnosis_Name"].ToString();
                        if (Convert.ToInt32(dr["Status_Id"].ToString()) == 1)
                            chkStatus_1_3.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 2)
                            chkStatus_2_3.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 3)
                            chkStatus_3_3.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 4)
                            chkStatus_4_3.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 5)
                            chkStatus_5_3.Visible = true;
                        labDiagnosis_Code3.Text = dr["Diagnosis_Code"].ToString();

                        labDiagnosis_Name3.Visible = true;
                        labDiagnosis_Code3.Visible = true;
                    }
                    else if (i == 3)
                    {
                        labDiagnosis_Name4.Text = dr["Diagnosis_Name"].ToString();
                        if (Convert.ToInt32(dr["Status_Id"].ToString()) == 1)
                            chkStatus_1_4.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 2)
                            chkStatus_2_4.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 3)
                            chkStatus_3_4.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 4)
                            chkStatus_4_4.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 5)
                            chkStatus_5_4.Visible = true;
                        labDiagnosis_Code4.Text = dr["Diagnosis_Code"].ToString();

                        labDiagnosis_Name4.Visible = true;
                        labDiagnosis_Code4.Visible = true;
                    }
                    else if (i == 4)
                    {
                        labDiagnosis_Name5.Text = dr["Diagnosis_Name"].ToString();
                        if (Convert.ToInt32(dr["Status_Id"].ToString()) == 1)
                            chkStatus_1_5.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 2)
                            chkStatus_2_5.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 3)
                            chkStatus_3_5.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 4)
                            chkStatus_4_5.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 5)
                            chkStatus_5_5.Visible = true;
                        labDiagnosis_Code5.Text = dr["Diagnosis_Code"].ToString();

                        labDiagnosis_Name5.Visible = true;
                        labDiagnosis_Code5.Visible = true;
                    }
                    else if (i == 5)
                    {
                        labDiagnosis_Name6.Text = dr["Diagnosis_Name"].ToString();
                        if (Convert.ToInt32(dr["Status_Id"].ToString()) == 1)
                            chkStatus_1_6.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 2)
                            chkStatus_2_6.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 3)
                            chkStatus_3_6.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 4)
                            chkStatus_4_6.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 5)
                            chkStatus_5_6.Visible = true;
                        labDiagnosis_Code6.Text = dr["Diagnosis_Code"].ToString();

                        labDiagnosis_Name6.Visible = true;
                        labDiagnosis_Code6.Visible = true;
                    }
                    else if (i == 6)
                    {
                        labDiagnosis_Name7.Text = dr["Diagnosis_Name"].ToString();
                        if (Convert.ToInt32(dr["Status_Id"].ToString()) == 1)
                            chkStatus_1_7.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 2)
                            chkStatus_2_7.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 3)
                            chkStatus_3_7.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 4)
                            chkStatus_4_7.Visible = true;
                        else if (Convert.ToInt32(dr["Status_Id"].ToString()) == 5)
                            chkStatus_5_7.Visible = true;
                        labDiagnosis_Code7.Text = dr["Diagnosis_Code"].ToString();

                        labDiagnosis_Name7.Visible = true;
                        labDiagnosis_Code7.Visible = true;
                    }
                }
                #endregion

                #region 相关人员加载到页面

                lueKszr.CodeValue = info.IemBasicInfo.Section_Director;
                lueZrys.CodeValue = info.IemBasicInfo.Director;
                lueZzys.CodeValue = info.IemBasicInfo.Vs_Employee_Code;
                lueZyys.CodeValue = info.IemBasicInfo.Resident_Employee_Code;
                lueJxys.CodeValue = info.IemBasicInfo.Refresh_Employee_Code;
                lueYjs.CodeValue = info.IemBasicInfo.Master_Interne;
                lueSxys.CodeValue = info.IemBasicInfo.Interne;
                lueBmy.CodeValue = info.IemBasicInfo.Coding_User;
                #endregion

                #region 其他信息
                labMedical_Quality.Text = info.IemBasicInfo.Medical_Quality_Id.ToString();
 
                //this.gridControl1.BeginUpdate();
                //this.gridControl1.DataSource = dataTableOper;
                //this.gridControl1.EndUpdate();

                //入院状态
                //if (m_IemInfo.IemBasicInfo.AdmitInfo == "1")
                //    chkAdmitInfo1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.AdmitInfo == "2")
                //    chkAdmitInfo2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.AdmitInfo == "3")
                //    chkAdmitInfo3.Checked = true;
                labAdmitInfo.Text = m_IemInfo.IemBasicInfo.AdmitInfo;

                txtPathologyName.Text = m_IemInfo.IemBasicInfo.Pathology_Diagnosis_Name;
                //txtPathologyObservationSn.Text = m_IemInfo.IemBasicInfo.Pathology_Observation_Sn;

                //txtAshesDiagnosisName.Text = m_IemInfo.IemBasicInfo.Ashes_Diagnosis_Name;
                //txtAshesAnatomiseSn.Text = m_IemInfo.IemBasicInfo.Ashes_Anatomise_Sn;

                //labelControl1.Text = m_IemInfo.IemBasicInfo.Allergic_Drug;

                chkHBsAg.Text = m_IemInfo.IemBasicInfo.Hbsag.ToString();
                //if (m_IemInfo.IemBasicInfo.Hbsag == 0)
                //    chkHBsAg1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Hbsag == 1)
                //    chkHBsAg2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Hbsag == 2)
                //    chkHBsAg3.Checked = true;

                chkHCV.Text = m_IemInfo.IemBasicInfo.Hcv_Ab.ToString();
                //if (m_IemInfo.IemBasicInfo.Hcv_Ab == 0)
                //    chkHCV1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Hcv_Ab == 1)
                //    chkHCV2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Hcv_Ab == 2)
                //    chkHCV3.Checked = true;


                chkHIV.Text = m_IemInfo.IemBasicInfo.Hiv_Ab.ToString();
                //if (m_IemInfo.IemBasicInfo.Hiv_Ab == 0)
                //    chkHIV1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Hiv_Ab == 1)
                //    chkHIV2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Hiv_Ab == 2)
                //    chkHIV3.Checked = true;


                chkOpdIpd.Text = m_IemInfo.IemBasicInfo.Opd_Ipd_Id.ToString();

                //if (m_IemInfo.IemBasicInfo.Opd_Ipd_Id == 0)
                //    chkOpdIpd1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Opd_Ipd_Id == 1)
                //    chkOpdIpd2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Opd_Ipd_Id == 2)
                //    chkOpdIpd3.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Opd_Ipd_Id == 3)
                //    chkOpdIpd4.Checked = true;


                chkInOut.Text = m_IemInfo.IemBasicInfo.In_Out_Inpatinet_Id.ToString();

                //if (m_IemInfo.IemBasicInfo.In_Out_Inpatinet_Id == 0)
                //    chkInOut1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.In_Out_Inpatinet_Id == 1)
                //    chkInOut2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.In_Out_Inpatinet_Id == 2)
                //    chkInOut3.Checked = true;
                //if (m_IemInfo.IemBasicInfo.In_Out_Inpatinet_Id == 3)
                //    chkInOut4.Checked = true;

                chkBeforeAfter.Text = m_IemInfo.IemBasicInfo.Before_After_Or_Id.ToString();
                //if (m_IemInfo.IemBasicInfo.Before_After_Or_Id == 0)
                //    chkBeforeAfter1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Before_After_Or_Id == 1)
                //    chkBeforeAfter2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Before_After_Or_Id == 2)
                //    chkBeforeAfter3.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Before_After_Or_Id == 3)
                //    chkBeforeAfter4.Checked = true;

                chkClinical.Text = m_IemInfo.IemBasicInfo.Clinical_Pathology_Id.ToString();
                //if (m_IemInfo.IemBasicInfo.Clinical_Pathology_Id == 0)
                //    chkClinical1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Clinical_Pathology_Id == 1)
                //    chkClinical2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Clinical_Pathology_Id == 2)
                //    chkClinical3.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Clinical_Pathology_Id == 3)
                //    chkClinical4.Checked = true;

                chkPacsPathology.Text = m_IemInfo.IemBasicInfo.Pacs_Pathology_Id.ToString();
                //if (m_IemInfo.IemBasicInfo.Pacs_Pathology_Id == 0)
                //    chkPacsPathology1.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Pacs_Pathology_Id == 1)
                //    chkPacsPathology2.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Pacs_Pathology_Id == 2)
                //    chkPacsPathology3.Checked = true;
                //if (m_IemInfo.IemBasicInfo.Pacs_Pathology_Id == 3)
                //    chkPacsPathology4.Checked = true;

                seSaveTimes.Text = m_IemInfo.IemBasicInfo.Save_Times.ToString();
                seSuccessTimes.Text = m_IemInfo.IemBasicInfo.Success_Times.ToString();


                //入院后确诊日期
                if (!String.IsNullOrEmpty(m_IemInfo.IemBasicInfo.In_Check_Date))
                {
                    txtIn_Check_Date.Text = Convert.ToDateTime(m_IemInfo.IemBasicInfo.In_Check_Date).ToString("yyyy年MM月dd日");
                    //deInCheckDate.DateTime = Convert.ToDateTime(m_IemInfo.IemBasicInfo.In_Check_Date);
                    //teInCheckDate.Time = Convert.ToDateTime(m_IemInfo.IemBasicInfo.In_Check_Date);
                }

                foreach (Iem_Mainpage_Diagnosis im in m_IemInfo.IemDiagInfo)
                {
                    if (im.Diagnosis_Type_Id == 13)
                        this.lueOutDiag.CodeValue = im.Diagnosis_Code;
                    else if (im.Diagnosis_Type_Id == 2)
                        this.lueInDiag.CodeValue = im.Diagnosis_Code;
                }
                #endregion
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

        private void UCIemDiagnose_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextEdit)
                {
                    e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                        new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                }
            }

            e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
            e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
        }

    }
}
