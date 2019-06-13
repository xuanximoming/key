using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DrectSoft.Wordbook;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Xml;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class IemNewDiagInfoForm : DevBaseForm
    {
        private IEmrHost m_App;

        private Iem_Mainpage_Diagnosis m_IemDiagInfo = new Iem_Mainpage_Diagnosis();
        /// <summary>
        /// 手术信息
        /// </summary>
        public Iem_Mainpage_Diagnosis IemOperInfo
        {
            get
            {
                GetUI();
                return m_IemDiagInfo;
            }
            set
            {
                m_IemDiagInfo = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataTable DataOper
        {
            get
            {
                if (m_DataOper == null)
                    m_DataOper = new DataTable();

                GetDataOper();
                return m_DataOper;
            }
        }
        private DataTable m_DataOper;

        private string m_OPETYPE;//跳到此页面传来的操作类型标识  add ywk
        private string m_DIAGCODE;//传来的ICD编码
        private string m_STATUSID;//传来的入院病情
        private string m_AdmitInfo;//传来的子入院病情 
        public IemNewDiagInfoForm(IEmrHost app, string operatetype, string diagcode, string statusid,string admitinfo)
        {
            InitializeComponent();
            m_App = app;
            InitLookUpEditor();
            m_OPETYPE = operatetype;
            m_DIAGCODE = diagcode;
            m_STATUSID = statusid;
            m_AdmitInfo = admitinfo;
            BridFormValue(m_OPETYPE, m_DIAGCODE, m_STATUSID, m_AdmitInfo);

            IemMainPageManger IemM = new IemMainPageManger(m_App, m_App.CurrentPatientInfo);
            string cansee = IemM.GetConfigValueByKey("EmrInputConfig");
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(cansee);
            if (doc.GetElementsByTagName("IemPageContorlVisable")[0].InnerText == "0")//不可见
            {
                //labelControl4.Visible = false;
                //chkMandZ0.Visible = false;
                //chkMandZ1.Visible = false;
                //chkMandZ2.Visible = false;
                //chkMandZ3.Visible = false;
                //labelControl5.Visible = false;
                //chkRandC0.Visible = false;
                //chkRandC1.Visible = false;
                //chkRandC2.Visible = false;
                //chkRandC3.Visible = false;
                //labelControl6.Visible = false;
                //labelControl7.Visible = false;
                //labelControl8.Visible = false;
                //labelControl9.Visible = false;
                //chkSqAndSh0.Visible = false;
                //chkSqAndSh1.Visible = false;
                //chkSqAndSh2.Visible = false;
                //chkSqAndSh3.Visible = false;
                //chkLandB0.Visible = false;
                //chkLandB1.Visible = false;
                //chkLandB2.Visible = false;
                //chkLandB3.Visible = false;
                //chkRThree0.Visible = false;
                //chkRThree1.Visible = false;
                //chkRThree2.Visible = false;
                //chkRThree3.Visible = false;
                //chkFandB0.Visible = false;
                //chkFandB1.Visible = false;
                //chkFandB2.Visible = false;
                //chkFandB3.Visible = false;
                Point M = new Point(100, 100);
                Point M1 = new Point(280, 100);
                btnConfirm.Location = M;
                btnCancel.Location = M1;

            }
            if (doc.GetElementsByTagName("IemPageContorlVisable")[0].InnerText == "1")//可见
            {
                //labelControl4.Visible = true;
                //chkMandZ0.Visible = true;
                //chkMandZ1.Visible = true;
                //chkMandZ2.Visible = true;
                //chkMandZ3.Visible = true;
                //labelControl5.Visible = true;
                //chkRandC0.Visible = true;
                //chkRandC1.Visible = true;
                //chkRandC2.Visible = true;
                //chkRandC3.Visible = true;
                //labelControl6.Visible = true;
                //labelControl7.Visible = true;
                //labelControl8.Visible = true;
                //labelControl9.Visible = true;
                //chkSqAndSh0.Visible = true;
                //chkSqAndSh1.Visible = true;
                //chkSqAndSh2.Visible = true;
                //chkSqAndSh3.Visible = true;
                //chkLandB0.Visible = true;
                //chkLandB1.Visible = true;
                //chkLandB2.Visible = true;
                //chkLandB3.Visible = true;
                //chkRThree0.Visible = true;
                //chkRThree1.Visible = true;
                //chkRThree2.Visible = true;
                //chkRThree3.Visible = true;
                //chkFandB0.Visible = true;
                //chkFandB1.Visible = true;
                //chkFandB2.Visible = true;
                //chkFandB3.Visible = true;
            }
        }
        public string CanSeeContorl;
        /// <summary>
        /// 根据跳转来传来的状态值，控制此页面的显示情况
        /// add by ywk 
        /// </summary>
        /// <param name="operatetype"></param>
        private void BridFormValue(string operatetype, string diagcode, string statusid,string admitinfo)
        {
            if (operatetype == "add")
            {
                this.lueOutDiag.CodeValue = "";
                this.chkStatus1.Checked = false;
                this.chkStatus2.Checked = false;
                this.chkStatus3.Checked = false;
                this.chkStatus4.Checked = false;

                this.chkAdmitInfo1.Checked = false;
                this.chkAdmitInfo2.Checked = false;
                this.chkAdmitInfo3.Checked = false;
                this.chkAdmitInfo4.Checked = false;
            }
            if (operatetype == "edit")
            {
                if (!string.IsNullOrEmpty(diagcode))
                {
                    lueOutDiag.CodeValue = diagcode;
                }
                if (!string.IsNullOrEmpty(statusid))
                {
                    switch (statusid)//控制入院病情的选中情况
                    {
                        case "1":
                            chkStatus1.Checked = true;
                            break;
                        case "2":
                            chkStatus2.Checked = true;
                            break;
                        case "3":
                            chkStatus3.Checked = true;
                            break;
                        case "4":
                            chkStatus4.Checked = true;
                            break;
                        default:
                            break;
                    }
                    //如果上级页面传来的是有，就显示子项目的几个控件 add by ywk 2012年7月26日15:09:15
                    if (statusid == "1")
                    {
                        CanSeeContorl = "1";
                        //labelControl1.Visible = true;
                        //chkAdmitInfo1.Visible = true;
                        //chkAdmitInfo2.Visible = true;
                        //chkAdmitInfo3.Visible = true;
                        //chkAdmitInfo4.Visible = true;
                    }

                    if (!string.IsNullOrEmpty(admitinfo))
                    {
                        switch (admitinfo)
                        {
                            case"1":
                                chkAdmitInfo1.Checked = true;
                                break;
                            case "2":
                                chkAdmitInfo2.Checked = true;
                                break;
                            case "3":
                                chkAdmitInfo3.Checked = true;
                                break;
                            case"4":
                                chkAdmitInfo4.Checked = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
                //编辑状态进来。带过来几个诊断的符合情况
                #region 控制诊断符合情况的复选框
                IemMainPageManger IemM = new IemMainPageManger(m_App, m_App.CurrentPatientInfo);
                DataTable DTIemDiag = IemM.GetIemInfo().IemDiagInfo.OutDiagTable;
                DataTable NewDt = DTIemDiag.Clone();
                DataRow[] SpliteRows = DTIemDiag.Select("DIAGNOSIS_CODE='" + diagcode + "'");
                if (SpliteRows.Length > 0)
                {
                    for (int i = 0; i < SpliteRows.Length; i++)
                    {
                        NewDt.ImportRow(SpliteRows[i]);
                    }
                }
                if (NewDt.Rows.Count > 0)
                {
                    //if (NewDt.Rows[0]["AdmitInfo"].ToString() == "1")
                    //{
                    //    chkAdmitInfo1.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["AdmitInfo"].ToString() == "2")
                    //{
                    //    chkAdmitInfo2.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["AdmitInfo"].ToString() == "3")
                    //{
                    //    chkAdmitInfo3.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["AdmitInfo"].ToString() == "4")
                    //{
                    //    chkAdmitInfo4.Checked = true;
                    //}
                    //门诊和住院
                    //if (NewDt.Rows[0]["MENANDINHOP"].ToString() == "0")
                    //{
                    //    chkMandZ0.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["MENANDINHOP"].ToString() == "1")
                    //{
                    //    chkMandZ1.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["MENANDINHOP"].ToString() == "2")
                    //{
                    //    chkMandZ2.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["MENANDINHOP"].ToString() == "3")
                    //{
                    //    chkMandZ3.Checked = true;
                    //}
                    ////入院和出院
                    //if (NewDt.Rows[0]["INHOPANDOUTHOP"].ToString() == "0")
                    //{
                    //    chkRandC0.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["INHOPANDOUTHOP"].ToString() == "1")
                    //{
                    //    chkRandC1.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["INHOPANDOUTHOP"].ToString() == "2")
                    //{
                    //    chkRandC2.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["INHOPANDOUTHOP"].ToString() == "3")
                    //{
                    //    chkRandC3.Checked = true;
                    //}
                    ////术前和术后
                    //if (NewDt.Rows[0]["BEFOREOPEANDAFTEROPER"].ToString() == "0")
                    //{
                    //    chkSqAndSh0.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["BEFOREOPEANDAFTEROPER"].ToString() == "1")
                    //{
                    //    chkSqAndSh1.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["BEFOREOPEANDAFTEROPER"].ToString() == "2")
                    //{
                    //    chkSqAndSh2.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["BEFOREOPEANDAFTEROPER"].ToString() == "3")
                    //{
                    //    chkSqAndSh3.Checked = true;
                    //}
                    ////临床和病；理
                    //if (NewDt.Rows[0]["LINANDBINGLI"].ToString() == "0")
                    //{
                    //    chkLandB0.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["LINANDBINGLI"].ToString() == "1")
                    //{
                    //    chkLandB1.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["LINANDBINGLI"].ToString() == "2")
                    //{
                    //    chkLandB2.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["LINANDBINGLI"].ToString() == "3")
                    //{
                    //    chkLandB3.Checked = true;
                    //}
                    ////入院三日内
                    //if (NewDt.Rows[0]["INHOPTHREE"].ToString() == "0")
                    //{
                    //    chkRThree0.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["INHOPTHREE"].ToString() == "1")
                    //{
                    //    chkRThree1.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["INHOPTHREE"].ToString() == "2")
                    //{
                    //    chkRThree2.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["INHOPTHREE"].ToString() == "3")
                    //{
                    //    chkRThree3.Checked = true;
                    //}
                    ////放射和病理
                    //if (NewDt.Rows[0]["FANGANDBINGLI"].ToString() == "0")
                    //{
                    //    chkFandB0.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["FANGANDBINGLI"].ToString() == "1")
                    //{
                    //    chkFandB1.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["FANGANDBINGLI"].ToString() == "2")
                    //{
                    //    chkFandB2.Checked = true;
                    //}
                    //if (NewDt.Rows[0]["FANGANDBINGLI"].ToString() == "3")
                    //{
                    //    chkFandB3.Checked = true;
                    //}
                }
                #endregion
            }

        }
        private void IemNewDiagInfoForm_Load(object sender, EventArgs e)
        {

#if DEBUG
#else
            HideSbutton();
#endif
            //窗体加载子入院病情是不显示的 
            labelControl1.Visible = false;
            chkAdmitInfo1.Visible = false;
            chkAdmitInfo2.Visible = false;
            chkAdmitInfo3.Visible = false;
            chkAdmitInfo4.Visible = false;
            if (CanSeeContorl == "1")
            {
                labelControl1.Visible = true;
                chkAdmitInfo1.Visible = true;
                chkAdmitInfo2.Visible = true;
                chkAdmitInfo3.Visible = true;
                chkAdmitInfo4.Visible = true;
            }
        }


        private void InitLookUpEditor()
        {
            BindLueData(lueOutDiag, 12);
        }
        #region 绑定LUE
        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_App.SqlHelper;
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
            DataTable dataTable = AddTableColumn(m_App.SqlHelper.ExecuteDataTable("usp_GetLookUpEditorData", paramCollection, CommandType.StoredProcedure));
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

        private void GetUI()
        {

        }

        private void GetDataOper()
        {
            m_DataOper = new DataTable();
            #region
            if (!m_DataOper.Columns.Contains("Diagnosis_Code"))
                m_DataOper.Columns.Add("Diagnosis_Code");
            if (!m_DataOper.Columns.Contains("Diagnosis_Name"))
                m_DataOper.Columns.Add("Diagnosis_Name");
            if (!m_DataOper.Columns.Contains("Status_Id"))
                m_DataOper.Columns.Add("Status_Id");
            if (!m_DataOper.Columns.Contains("Status_Name"))
                m_DataOper.Columns.Add("Status_Name");

            if (!m_DataOper.Columns.Contains("MenAndInHop"))
                m_DataOper.Columns.Add("MenAndInHop");
            if (!m_DataOper.Columns.Contains("InHopAndOutHop"))
                m_DataOper.Columns.Add("InHopAndOutHop");
            if (!m_DataOper.Columns.Contains("BeforeOpeAndAfterOper"))
                m_DataOper.Columns.Add("BeforeOpeAndAfterOper");
            if (!m_DataOper.Columns.Contains("LinAndBingLi"))
                m_DataOper.Columns.Add("LinAndBingLi");
            if (!m_DataOper.Columns.Contains("InHopThree"))
                m_DataOper.Columns.Add("InHopThree");
            if (!m_DataOper.Columns.Contains("FangAndBingLi"))
                m_DataOper.Columns.Add("FangAndBingLi");

            if (!m_DataOper.Columns.Contains("MenAndInHopName"))
                m_DataOper.Columns.Add("MenAndInHopName");
            if (!m_DataOper.Columns.Contains("InHopAndOutHopName"))
                m_DataOper.Columns.Add("InHopAndOutHopName");
            if (!m_DataOper.Columns.Contains("BeforeOpeAndAfterOperName"))
                m_DataOper.Columns.Add("BeforeOpeAndAfterOperName");
            if (!m_DataOper.Columns.Contains("LinAndBingLiName"))
                m_DataOper.Columns.Add("LinAndBingLiName");
            if (!m_DataOper.Columns.Contains("InHopThreeName"))
                m_DataOper.Columns.Add("InHopThreeName");
            if (!m_DataOper.Columns.Contains("FangAndBingLiName"))
                m_DataOper.Columns.Add("FangAndBingLiName");

            if (!m_DataOper.Columns.Contains("AdmitInfo"))
                m_DataOper.Columns.Add("AdmitInfo");
            if (!m_DataOper.Columns.Contains("AdmitInfoName"))
                m_DataOper.Columns.Add("AdmitInfoName");
            #endregion
            FillUI();
            DataRow row = m_DataOper.NewRow();
            row["Diagnosis_Code"] = lueOutDiag.CodeValue;
            row["Diagnosis_Name"] = lueOutDiag.DisplayValue;

            //状态
            if (chkStatus1.Checked)
            {
                row["Status_Id"] = 1;
                row["Status_Name"] = chkStatus1.Tag.ToString();
            }
            else if (chkStatus2.Checked)
            {
                row["Status_Id"] = 2;
                row["Status_Name"] = chkStatus2.Tag.ToString();
            }
            else if (chkStatus3.Checked)
            {
                row["Status_Id"] = 3;
                row["Status_Name"] = chkStatus3.Tag.ToString();
            }
            else if (chkStatus4.Checked)
            {
                row["Status_Id"] = 4;
                row["Status_Name"] = chkStatus4.Tag.ToString();
            }

            //新增的（子）入院病情 add by  ywk 
            if (chkAdmitInfo1.Checked)//危
            {
                row["AdmitInfo"] = "1";
                row["AdmitInfoName"] = chkAdmitInfo1.Tag.ToString();
            }
            else if (chkAdmitInfo2.Checked)//重
            {
                row["AdmitInfo"] = "2";
                row["AdmitInfoName"] = chkAdmitInfo2.Tag.ToString();
            }
            else if (chkAdmitInfo3.Checked)//一般
            {
                row["AdmitInfo"] = "3";
                row["AdmitInfoName"] = chkAdmitInfo3.Tag.ToString();
            }
            else if (chkAdmitInfo4.Checked)//急 
            {
                row["AdmitInfo"] = "4";
                row["AdmitInfoName"] = chkAdmitInfo4.Tag.ToString();
            }

            //门诊和住院
            //if (chkMandZ0.Checked == true)
            //{
            //    row["MenAndInHop"] = "0";
            //    row["MenAndInHopName"] = chkMandZ0.Tag.ToString();
            //}
            //else if (chkMandZ1.Checked)
            //{
            //    row["MenAndInHop"] = "1";
            //    row["MenAndInHopName"] = chkMandZ1.Tag.ToString();
            //}
            //else if (chkMandZ2.Checked)
            //{
            //    row["MenAndInHop"] = "2";
            //    row["MenAndInHopName"] = chkMandZ2.Tag.ToString();
            //}
            //else if (chkMandZ3.Checked)
            //{
            //    row["MenAndInHop"] = "3";
            //    row["MenAndInHopName"] = chkMandZ3.Tag.ToString();
            //}
            ////入院和出院
            //if (chkRandC0.Checked)
            //{
            //    row["InHopAndOutHop"] = "0";
            //    row["InHopAndOutHopName"] = chkRandC0.Tag.ToString();
            //}
            //else if (chkRandC1.Checked)
            //{
            //    row["InHopAndOutHop"] = "1";
            //    row["InHopAndOutHopName"] = chkRandC1.Tag.ToString();
            //}
            //else if (chkRandC2.Checked)
            //{
            //    row["InHopAndOutHop"] = "2";
            //    row["InHopAndOutHopName"] = chkRandC2.Tag.ToString();
            //}
            //else if (chkRandC3.Checked)
            //{
            //    row["InHopAndOutHop"] = "3";
            //    row["InHopAndOutHopName"] = chkRandC3.Tag.ToString();
            //}

            ////术前和术后
            //if (chkSqAndSh0.Checked)
            //{
            //    row["BeforeOpeAndAfterOper"] = "0";
            //    row["BeforeOpeAndAfterOperName"] = chkSqAndSh0.Tag.ToString();
            //}
            //else if (chkSqAndSh1.Checked)
            //{
            //    row["BeforeOpeAndAfterOper"] = "1";
            //    row["BeforeOpeAndAfterOperName"] = chkSqAndSh1.Tag.ToString();
            //}
            //else if (chkSqAndSh2.Checked)
            //{
            //    row["BeforeOpeAndAfterOper"] = "2";
            //    row["BeforeOpeAndAfterOperName"] = chkSqAndSh2.Tag.ToString();
            //}
            //else if (chkSqAndSh3.Checked)
            //{
            //    row["BeforeOpeAndAfterOper"] = "3";
            //    row["BeforeOpeAndAfterOperName"] = chkSqAndSh3.Tag.ToString();
            //}
            ////临床和病理
            //if (chkLandB0.Checked)
            //{
            //    row["LinAndBingLi"] = "0";
            //    row["LinAndBingLiName"] = chkLandB0.Tag.ToString();
            //}
            //else if (chkLandB1.Checked)
            //{
            //    row["LinAndBingLi"] = "1";
            //    row["LinAndBingLiName"] = chkLandB1.Tag.ToString();
            //}
            //else if (chkLandB2.Checked)
            //{
            //    row["LinAndBingLi"] = "2";
            //    row["LinAndBingLiName"] = chkLandB2.Tag.ToString();
            //}
            //else if (chkLandB3.Checked)
            //{
            //    row["LinAndBingLi"] = "3";
            //    row["LinAndBingLiName"] = chkLandB3.Tag.ToString();
            //}
            ////入院三日内
            //if (chkRThree0.Checked)
            //{
            //    row["InHopThree"] = "0";
            //    row["InHopThreeName"] = chkRThree0.Tag.ToString();
            //}
            //else if (chkRThree1.Checked)
            //{
            //    row["InHopThree"] = "1";
            //    row["InHopThreeName"] = chkRThree1.Tag.ToString();
            //}
            //else if (chkRThree2.Checked)
            //{
            //    row["InHopThree"] = "2";
            //    row["InHopThreeName"] = chkRThree2.Tag.ToString();
            //}
            //else if (chkRThree3.Checked)
            //{
            //    row["InHopThree"] = "3";
            //    row["InHopThreeName"] = chkRThree3.Tag.ToString();
            //}
            ////放射和病理
            //if (chkFandB0.Checked)
            //{
            //    row["FangAndBingLi"] = "3";
            //    row["FangAndBingLiName"] = chkFandB0.Tag.ToString();
            //}
            //else if (chkFandB1.Checked)
            //{
            //    row["FangAndBingLi"] = "1";
            //    row["FangAndBingLiName"] = chkFandB1.Tag.ToString();
            //}
            //else if (chkFandB2.Checked)
            //{
            //    row["FangAndBingLi"] = "2";
            //    row["FangAndBingLiName"] = chkFandB2.Tag.ToString();
            //}
            //else if (chkFandB3.Checked)
            //{
            //    row["FangAndBingLi"] = "3";
            //    row["FangAndBingLiName"] = chkFandB3.Tag.ToString();
            //}


            m_DataOper.Rows.Add(row);
            //m_DataOper.AcceptChanges();

        }

        private void FillUI()
        {
            //if (m_IemDiagInfo == null || String.IsNullOrEmpty(m_IemDiagInfo.Diagnosis_Code))
            //    return;
            //lueOutDiag.CodeValue = m_IemDiagInfo.Diagnosis_Code;

            //if (m_IemDiagInfo.Status_Id == 1)
            //    chkStatus1.Checked = true;
            //if (m_IemDiagInfo.Status_Id == 2)
            //    chkStatus2.Checked = true;
            //if (m_IemDiagInfo.Status_Id == 3)
            //    chkStatus3.Checked = true;
            //if (m_IemDiagInfo.Status_Id == 4)
            //    chkStatus4.Checked = true;
            //if (m_IemDiagInfo.Status_Id == 5)
            //    chkStatus5.Checked = true;

        }

        /// <summary>
        /// 隐藏按钮
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
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.lueOutDiag.CodeValue))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                m_App.CustomMessageBox.MessageShow("请选择诊断编码");
            }

        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
        /// <summary>
        /// 宜昌中心人民医院需求
        /// add by ywk  2012年7月26日14:56:07
        /// 出院诊断中，入院病情，如果选择有，则显示增加子项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkStatus1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStatus1.Checked)//如果选中了有选项，就显示子项目
            {
                labelControl1.Visible = true;
                chkAdmitInfo1.Visible = true;
                chkAdmitInfo2.Visible = true;
                chkAdmitInfo3.Visible = true;
                chkAdmitInfo4.Visible = true;
            }
            else
            {
                labelControl1.Visible = false;
                chkAdmitInfo1.Visible = false;
                chkAdmitInfo2.Visible = false;
                chkAdmitInfo3.Visible = false;
                chkAdmitInfo4.Visible = false;
                //选择别的时，要把选”有“的时候的值清空
                chkAdmitInfo1.Checked = false;
                chkAdmitInfo2.Checked = false;
                chkAdmitInfo3.Checked = false;
                chkAdmitInfo4.Checked = false;
            }
        }

    }
}
