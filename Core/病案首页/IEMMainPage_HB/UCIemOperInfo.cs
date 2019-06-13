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
using DrectSoft.Common;
using Convertmy = DrectSoft.Core.UtilsForExtension;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraEditors;
using DrectSoft.Common.Eop;
using System.Xml;
using DrectSoft.Service;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class UCIemOperInfo : UserControl
    {
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
        public bool editFlag = false;  //add by cyq 2012-12-06 病案室人员编辑首页(状态改为归档)
        //private DataTable m_DataTableDiag = null;
        public UCIemOperInfo()
        {
            InitializeComponent();
            InitDiagSuit();
        }
        private string CanSEEControl = string.Empty;
        private void UCIemOperInfo_Load(object sender, EventArgs e)
        {
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

        }

        public void FillUI(IemMainPageInfo info, IEmrHost app)
        {
            try
            {
                m_App = app;
                m_IemInfo = info;
                chx_qj1.CheckedChanged += new EventHandler(chx_qj1_CheckedChanged);
                //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
                FillUIInner();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 抢救复选框选择事件
        /// xlb 2013-01-22
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chx_qj1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chx_qj1.Checked)
                {
                    txt_qjCount.Text = string.Empty;
                    txt_qjSucceedCount.Text = string.Empty;

                    txt_qjCount.Enabled = false;
                    txt_qjSucceedCount.Enabled = false;
                }
                else
                {
                    txt_qjCount.Enabled = true;
                    txt_qjSucceedCount.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        delegate void FillUIDelegate();
        private void FillUIInner()
        {
            try
            {
                #region
                if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
                {
                    //to do 病患基本信息
                    //病案首页中无数据，关于颅内昏迷的几个时间默认显示为---  edit  by ywk 2012年5月30日 10:19:12
                    txtBeforeHosComaDay.Text = m_IemInfo.IemBasicInfo.BeforeHosComaDay;
                    txtBeforeHosComaHour.Text = m_IemInfo.IemBasicInfo.BeforeHosComaHour;
                    txtBeforeHosComaMinute.Text = m_IemInfo.IemBasicInfo.BeforeHosComaMinute;

                    txtLaterHosComaDay.Text = m_IemInfo.IemBasicInfo.LaterHosComaDay;
                    txtLaterHosComaHour.Text = m_IemInfo.IemBasicInfo.LaterHosComaHour;
                    txtLaterHosComaMinute.Text = m_IemInfo.IemBasicInfo.LaterHosComaMinute;
                }
                else
                {
                    #region 已注释
                    //手术
                    //DataTable dataTableOper = new DataTable();
                    //foreach (Iem_MainPage_Operation im in m_IemInfo.IemOperInfo)
                    //{
                    //    if (m_OperInfoFrom == null)
                    //        m_OperInfoFrom = new IemNewOperInfo(m_App);
                    //    m_OperInfoFrom.IemOperInfo = im;
                    //    DataTable dataTable = m_OperInfoFrom.DataOper;
                    //    if (dataTableOper.Rows.Count == 0)
                    //        dataTableOper = dataTable.Clone();
                    //    foreach (DataRow row in dataTable.Rows)
                    //    {
                    //        dataTableOper.ImportRow(row);
                    //    }
                    //    dataTableOper.AcceptChanges();
                    //}
                    //this.gridControl1.DataSource = dataTableOper;
                    #endregion

                    #region 已注释 by cyq 2012-12-24 去掉手术列表
                    //this.gridControl1.DataSource = null;

                    //this.gridControl1.DataSource = m_IemInfo.IemOperInfo.Operation_Table;

                    //m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);

                    //txtXRay.Text = m_IemInfo.IemBasicInfo.Xay_Sn;
                    //txtCT.Text = m_IemInfo.IemBasicInfo.Ct_Sn;
                    //txtMri.Text = m_IemInfo.IemBasicInfo.Mri_Sn;
                    //txtDsa.Text = m_IemInfo.IemBasicInfo.Dsa_Sn;

                    //gridControl2.EndUpdate();
                    //gridControl3.EndUpdate();
                    #endregion

                    #region 新增项目信息
                    //离院方式
                    if (m_IemInfo.IemBasicInfo.OutHosType == "1")
                        chkOutHosType1.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.OutHosType == "2")
                        chkOutHosType2.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.OutHosType == "3")
                        chkOutHosType3.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.OutHosType == "4")
                        chkOutHosType4.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.OutHosType == "5")
                        chkOutHosType5.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.OutHosType == "9")
                        chkOutHosType9.Checked = true;

                    txtReceiveHosPital.Text = m_IemInfo.IemBasicInfo.ReceiveHosPital;
                    txtReceiveHosPital2.Text = m_IemInfo.IemBasicInfo.ReceiveHosPital2;
                    //是否有出院再入院计划
                    if (m_IemInfo.IemBasicInfo.AgainInHospital == "1")
                        chkAgainInHospital1.Checked = true;
                    else if (m_IemInfo.IemBasicInfo.AgainInHospital == "2")
                        chkAgainInHospital2.Checked = true;

                    txtAgainInHospitalReason.Text = m_IemInfo.IemBasicInfo.AgainInHospitalReason;
                    //脑颅损伤患者昏迷时间 - 入院前
                    txtBeforeHosComaDay.Text = m_IemInfo.IemBasicInfo.BeforeHosComaDay;
                    txtBeforeHosComaHour.Text = m_IemInfo.IemBasicInfo.BeforeHosComaHour;
                    txtBeforeHosComaMinute.Text = m_IemInfo.IemBasicInfo.BeforeHosComaMinute;
                    //脑颅损伤患者昏迷时间 - 入院后
                    txtLaterHosComaDay.Text = m_IemInfo.IemBasicInfo.LaterHosComaDay;
                    txtLaterHosComaHour.Text = m_IemInfo.IemBasicInfo.LaterHosComaHour;
                    txtLaterHosComaMinute.Text = m_IemInfo.IemBasicInfo.LaterHosComaMinute;

                    //抢救
                    txt_qjCount.Text = m_IemInfo.IemOthers.Emergency_times;
                    txt_qjSucceedCount.Text = m_IemInfo.IemOthers.Emergency_Successful_times;
                    //add by ywk 2013年2月26日8:50:19  
                    if (string.IsNullOrEmpty(txt_qjCount.Text) && string.IsNullOrEmpty(txt_qjSucceedCount.Text))//如果抢救次数和成功次数都为空，就选择无抢救
                    {
                        chx_qj1.Checked = true;
                    }
                    //都有值，就选择有
                    if (!string.IsNullOrEmpty(txt_qjCount.Text) && !string.IsNullOrEmpty(txt_qjSucceedCount.Text))
                    {
                        chx_qj2.Checked = true;
                    }
                    //诊断符合情况
                    lue_diagSuit1.EditValue = m_IemInfo.IemBasicInfo.MenAndInHop;
                    lue_diagSuit2.EditValue = m_IemInfo.IemBasicInfo.InHopAndOutHop;
                    lue_diagSuit3.EditValue = m_IemInfo.IemBasicInfo.BeforeOpeAndAfterOper;
                    lue_diagSuit4.EditValue = m_IemInfo.IemBasicInfo.LinAndBingLi;
                    lue_diagSuit5.EditValue = m_IemInfo.IemBasicInfo.FangAndBingLi;
                    lue_diagSuit6.EditValue = m_IemInfo.IemBasicInfo.InHopThree;

                    //if (m_IemInfo.IemBasicInfo.ZG_FLAG == "1")
                    //    chkzg_flag1.Checked = true;
                    //else if (m_IemInfo.IemBasicInfo.ZG_FLAG == "2")
                    //    chkzg_flag2.Checked = true;
                    //else if (m_IemInfo.IemBasicInfo.ZG_FLAG == "3")
                    //    chkzg_flag3.Checked = true;
                    //else if (m_IemInfo.IemBasicInfo.ZG_FLAG == "4")
                    //    chkzg_flag4.Checked = true;
                    //else if (m_IemInfo.IemBasicInfo.ZG_FLAG == "5")
                    //    chkzg_flag5.Checked = true;

                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化诊断符合情况
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-23</date>
        private void InitDiagSuit()
        {
            try
            {
                DataTable dt = GetDiagSuitTypes();

                lue_diagSuit1.Properties.DataSource = dt;
                lue_diagSuit1.Properties.DisplayMember = "NAME";
                lue_diagSuit1.Properties.ValueMember = "ID";
                lue_diagSuit1.Properties.ShowHeader = false;
                lue_diagSuit1.Properties.ShowFooter = false;

                lue_diagSuit2.Properties.DataSource = dt;
                lue_diagSuit2.Properties.DisplayMember = "NAME";
                lue_diagSuit2.Properties.ValueMember = "ID";
                lue_diagSuit2.Properties.ShowHeader = false;
                lue_diagSuit2.Properties.ShowFooter = false;

                lue_diagSuit3.Properties.DataSource = dt;
                lue_diagSuit3.Properties.DisplayMember = "NAME";
                lue_diagSuit3.Properties.ValueMember = "ID";
                lue_diagSuit3.Properties.ShowHeader = false;
                lue_diagSuit3.Properties.ShowFooter = false;

                lue_diagSuit4.Properties.DataSource = dt;
                lue_diagSuit4.Properties.DisplayMember = "NAME";
                lue_diagSuit4.Properties.ValueMember = "ID";
                lue_diagSuit4.Properties.ShowHeader = false;
                lue_diagSuit4.Properties.ShowFooter = false;

                lue_diagSuit5.Properties.DataSource = dt;
                lue_diagSuit5.Properties.DisplayMember = "NAME";
                lue_diagSuit5.Properties.ValueMember = "ID";
                lue_diagSuit5.Properties.ShowHeader = false;
                lue_diagSuit5.Properties.ShowFooter = false;

                lue_diagSuit6.Properties.DataSource = dt;
                lue_diagSuit6.Properties.DisplayMember = "NAME";
                lue_diagSuit6.Properties.ValueMember = "ID";
                lue_diagSuit6.Properties.ShowHeader = false;
                lue_diagSuit6.Properties.ShowFooter = false;

                LookUpColumnInfoCollection coll1 = lue_diagSuit1.Properties.Columns;
                LookUpColumnInfoCollection coll2 = lue_diagSuit2.Properties.Columns;
                LookUpColumnInfoCollection coll3 = lue_diagSuit3.Properties.Columns;
                LookUpColumnInfoCollection coll4 = lue_diagSuit4.Properties.Columns;
                LookUpColumnInfoCollection coll5 = lue_diagSuit5.Properties.Columns;
                LookUpColumnInfoCollection coll6 = lue_diagSuit6.Properties.Columns;
                coll1.Add(new LookUpColumnInfo("ID".ToUpper()));
                coll1.Add(new LookUpColumnInfo("NAME".ToUpper()));
                coll2.Add(new LookUpColumnInfo("ID".ToUpper()));
                coll2.Add(new LookUpColumnInfo("NAME".ToUpper()));
                coll3.Add(new LookUpColumnInfo("ID".ToUpper()));
                coll3.Add(new LookUpColumnInfo("NAME".ToUpper()));
                coll4.Add(new LookUpColumnInfo("ID".ToUpper()));
                coll4.Add(new LookUpColumnInfo("NAME".ToUpper()));
                coll5.Add(new LookUpColumnInfo("ID".ToUpper()));
                coll5.Add(new LookUpColumnInfo("NAME".ToUpper()));
                coll6.Add(new LookUpColumnInfo("ID".ToUpper()));
                coll6.Add(new LookUpColumnInfo("NAME".ToUpper()));

                lue_diagSuit1.Properties.Columns["NAME"].Visible = true;
                lue_diagSuit1.Properties.Columns["ID"].Visible = false;
                lue_diagSuit2.Properties.Columns["NAME"].Visible = true;
                lue_diagSuit2.Properties.Columns["ID"].Visible = false;
                lue_diagSuit3.Properties.Columns["NAME"].Visible = true;
                lue_diagSuit3.Properties.Columns["ID"].Visible = false;
                lue_diagSuit4.Properties.Columns["NAME"].Visible = true;
                lue_diagSuit4.Properties.Columns["ID"].Visible = false;
                lue_diagSuit5.Properties.Columns["NAME"].Visible = true;
                lue_diagSuit5.Properties.Columns["ID"].Visible = false;
                lue_diagSuit6.Properties.Columns["NAME"].Visible = true;
                lue_diagSuit6.Properties.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 诊断符合情况下拉框数据集
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-23</date>
        /// <returns></returns>
        private DataTable GetDiagSuitTypes()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("NAME");

                DataRow row1 = dt.NewRow();
                row1["ID"] = "";
                row1["NAME"] = "";
                dt.Rows.Add(row1);

                DataRow row2 = dt.NewRow();
                row2["ID"] = "1";
                row2["NAME"] = "符合";
                dt.Rows.Add(row2);

                DataRow row3 = dt.NewRow();
                row3["ID"] = "2";
                row3["NAME"] = "不符合";
                dt.Rows.Add(row3);

                DataRow row4 = dt.NewRow();
                row4["ID"] = "3";
                row4["NAME"] = "不肯定";
                dt.Rows.Add(row4);

                DataRow row5 = dt.NewRow();
                row5["ID"] = "4";
                row5["NAME"] = "未做";
                dt.Rows.Add(row5);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// GET UI
        /// edit by xlb 2013-01-22
        /// </summary>
        private void GetUI()
        {
            try
            {
                #region 注释手术 by cyq 2012-12-24
                //if (this.gridControl1.DataSource != null)
                //{
                //手术

                //DataTable dtOperation = m_IemInfo.IemOperInfo.Operation_Table.Clone();
                //dtOperation.Rows.Clear();

                //DataTable dataTable = this.gridControl1.DataSource as DataTable;
                //foreach (DataRow row in dataTable.Rows)
                //{
                //    DataRow imOut = dtOperation.NewRow();
                //    //Iem_MainPage_Operation imOut = new Iem_MainPage_Operation();

                //    imOut["Operation_Code"] = row["Operation_Code"].ToString();
                //    imOut["Operation_Name"] = row["Operation_Name"].ToString();
                //    imOut["Operation_Date"] = row["Operation_Date"].ToString();

                //    imOut["operation_level"] = row["operation_level"].ToString();
                //    imOut["operation_level_Name"] = row["operation_level_Name"].ToString();

                //    imOut["Execute_User1"] = row["Execute_User1"].ToString();
                //    imOut["Execute_User1_Name"] = row["Execute_User1_Name"];
                //    imOut["Execute_User2"] = row["Execute_User2"].ToString();
                //    imOut["Execute_User2_Name"] = row["Execute_User2_Name"].ToString();
                //    imOut["Execute_User3"] = row["Execute_User3"].ToString();
                //    imOut["Execute_User3_Name"] = row["Execute_User3_Name"].ToString();
                //    imOut["Anaesthesia_Type_Id"] = row["Anaesthesia_Type_Id"].ToString();
                //    imOut["Anaesthesia_Type_Name"] = row["Anaesthesia_Type_Name"].ToString();
                //    imOut["Close_Level"] = row["Close_Level"].ToString();
                //    imOut["Close_Level_Name"] = row["Close_Level_Name"].ToString();
                //    imOut["Anaesthesia_User"] = row["Anaesthesia_User"].ToString();
                //    imOut["Anaesthesia_User_Name"] = row["Anaesthesia_User_Name"].ToString();

                //    imOut["IsChooseDate"] = row["IsChooseDate"].ToString();
                //    imOut["IsClearOpe"] = row["IsClearOpe"].ToString();
                //    imOut["IsGanRan"] = row["IsGanRan"].ToString();
                //    imOut["IsChooseDateName"] = row["IsChooseDateName"].ToString();
                //    imOut["IsClearOpeName"] = row["IsClearOpeName"].ToString();
                //    imOut["IsGanRanName"] = row["IsGanRanName"].ToString();
                //    //麻醉分级和手术并发症 add by cyq 2012-10-17
                //    imOut["Anesthesia_Level"] = row["Anesthesia_Level"].ToString();
                //    imOut["OperComplication_Code"] = row["OperComplication_Code"].ToString();
                //    dtOperation.Rows.Add(imOut);
                //}

                //m_IemInfo.IemOperInfo.Operation_Table = dtOperation;

                //}
                #endregion

                #region 新增项目信息
                //离院方式
                if (chkOutHosType1.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "1";
                else if (chkOutHosType2.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "2";
                else if (chkOutHosType3.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "3";
                else if (chkOutHosType4.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "4";
                else if (chkOutHosType5.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "5";
                else if (chkOutHosType9.Checked)
                    m_IemInfo.IemBasicInfo.OutHosType = "9";
                else
                    m_IemInfo.IemBasicInfo.OutHosType = "";

                m_IemInfo.IemBasicInfo.ReceiveHosPital = txtReceiveHosPital.Text;
                m_IemInfo.IemBasicInfo.ReceiveHosPital2 = txtReceiveHosPital2.Text;
                //是否有出院31天内再入院计划
                if (chkAgainInHospital1.Checked)
                    m_IemInfo.IemBasicInfo.AgainInHospital = "1";
                else if (chkAgainInHospital2.Checked)
                    m_IemInfo.IemBasicInfo.AgainInHospital = "2";
                else
                    m_IemInfo.IemBasicInfo.AgainInHospital = "";

                m_IemInfo.IemBasicInfo.AgainInHospitalReason = txtAgainInHospitalReason.Text;
                //颅脑损伤患者昏迷时间 - 入院前
                m_IemInfo.IemBasicInfo.BeforeHosComaDay = txtBeforeHosComaDay.Text;
                m_IemInfo.IemBasicInfo.BeforeHosComaHour = txtBeforeHosComaHour.Text;
                m_IemInfo.IemBasicInfo.BeforeHosComaMinute = txtBeforeHosComaMinute.Text;
                //颅脑损伤患者昏迷时间 - 入院后
                m_IemInfo.IemBasicInfo.LaterHosComaDay = txtLaterHosComaDay.Text;
                m_IemInfo.IemBasicInfo.LaterHosComaHour = txtLaterHosComaHour.Text;
                m_IemInfo.IemBasicInfo.LaterHosComaMinute = txtLaterHosComaMinute.Text;

                //抢救
                m_IemInfo.IemOthers.Emergency_times = txt_qjCount.Text;
                m_IemInfo.IemOthers.Emergency_Successful_times = txt_qjSucceedCount.Text;

                //诊断符合情况
                m_IemInfo.IemBasicInfo.MenAndInHop = null == lue_diagSuit1.EditValue ? "" : lue_diagSuit1.EditValue.ToString();
                m_IemInfo.IemBasicInfo.InHopAndOutHop = null == lue_diagSuit2.EditValue ? "" : lue_diagSuit2.EditValue.ToString();
                m_IemInfo.IemBasicInfo.BeforeOpeAndAfterOper = null == lue_diagSuit3.EditValue ? "" : lue_diagSuit3.EditValue.ToString();
                m_IemInfo.IemBasicInfo.LinAndBingLi = null == lue_diagSuit4.EditValue ? "" : lue_diagSuit4.EditValue.ToString();
                m_IemInfo.IemBasicInfo.FangAndBingLi = null == lue_diagSuit5.EditValue ? "" : lue_diagSuit5.EditValue.ToString();
                m_IemInfo.IemBasicInfo.InHopThree = null == lue_diagSuit6.EditValue ? "" : lue_diagSuit6.EditValue.ToString();

                //if (chkzg_flag1.Checked)
                //    m_IemInfo.IemBasicInfo.ZG_FLAG = "1";
                //else if (chkzg_flag2.Checked)
                //    m_IemInfo.IemBasicInfo.ZG_FLAG = "2";
                //else if (chkzg_flag3.Checked)
                //    m_IemInfo.IemBasicInfo.ZG_FLAG = "3";
                //else if (chkzg_flag4.Checked)
                //    m_IemInfo.IemBasicInfo.ZG_FLAG = "4";
                //else if (chkzg_flag5.Checked)
                //    m_IemInfo.IemBasicInfo.ZG_FLAG = "5";
                //else
                //    m_IemInfo.IemBasicInfo.ZG_FLAG = "";

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        IemNewOperInfo m_OperInfoFrom = null;
        //private void btnAddOperation_Click(object sender, EventArgs e)
        //{
        //    m_OperInfoFrom = new IemNewOperInfo(m_App, "new", null);
        //    m_OperInfoFrom.ShowDialog();
        //    if (m_OperInfoFrom.DialogResult == DialogResult.OK)
        //    {
        //        m_OperInfoFrom.IemOperInfo = null;
        //        DataTable dataTable = m_OperInfoFrom.DataOper;

        //        DataTable dataTableOper = new DataTable();
        //        if (this.gridControl1.DataSource != null)
        //        {
        //            dataTableOper = this.gridControl1.DataSource as DataTable;
        //        }
        //        if (dataTableOper.Rows.Count == 0)
        //        {
        //            dataTableOper = dataTable.Clone();
        //        }
        //        foreach (DataRow row in dataTable.Rows)
        //        {
        //            dataTableOper.ImportRow(row);
        //        }
        //        gridControl1.BeginUpdate();
        //        this.gridControl1.DataSource = dataTableOper;

        //        gridControl1.EndUpdate();
        //        m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
        //    }
        //}
        //private void btnEditOperation_Click(object sender, EventArgs e)
        //{

        //    if (gridViewOper.FocusedRowHandle < 0)
        //    {
        //        Common.Ctrs.DLG.MessageBox.Show("请选中一条记录");
        //        return;
        //    }
        //    DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
        //    if (dataRow == null)
        //    {
        //        Common.Ctrs.DLG.MessageBox.Show("请选中一条记录");
        //        return;
        //    }
        //    DataTable dataTableOper = this.gridControl1.DataSource as DataTable;
        //    DataTable dataTable = new DataTable();
        //    dataTable = dataTableOper.Clone();
        //    dataTable.ImportRow(dataRow);

        //    m_OperInfoFrom = new IemNewOperInfo(m_App, "edit", dataTable);

        //    m_OperInfoFrom.ShowDialog();
        //    if (m_OperInfoFrom.DialogResult == DialogResult.OK)
        //    {
        //        m_OperInfoFrom.IemOperInfo = null;

        //        dataTableOper.Rows.Remove(dataRow);
        //        foreach (DataRow row in m_OperInfoFrom.DataOper.Rows)
        //        {
        //            dataTableOper.ImportRow(row);
        //        }
        //        gridControl1.BeginUpdate();
        //        this.gridControl1.DataSource = dataTableOper;

        //        gridControl1.EndUpdate();
        //        m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
        //    }
        //}

        private void UCIemOperInfo_Paint(object sender, PaintEventArgs e)
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
        /// 右键删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_del_Operinfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //DeleteItem();
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            //DeleteItem();
        }

        /// <summary>
        /// 删除
        /// </summary>
        //private void DeleteItem()
        //{
        //    if (gridViewOper.FocusedRowHandle < 0)
        //    {
        //        Common.Ctrs.DLG.MessageBox.Show("请选中一条记录");
        //        return;
        //    }
        //    DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
        //    if (dataRow == null)
        //    {
        //        Common.Ctrs.DLG.MessageBox.Show("请选中一条记录");
        //        return;
        //    }
        //    if (Common.Ctrs.DLG.MessageBox.Show("您确定要删除该项手术记录吗？确定后生效，该操作不可恢复。", "删除手术记录", Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
        //    {
        //        return;
        //    }

        //    DataTable dataTableOper = this.gridControl1.DataSource as DataTable;

        //    dataTableOper.Rows.Remove(dataRow);

        //    this.gridControl1.BeginUpdate();
        //    this.gridControl1.DataSource = dataTableOper;
        //    m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
        //    this.gridControl1.EndUpdate();
        //}

        //private void barManager1_QueryShowPopupMenu(object sender, DevExpress.XtraBars.QueryShowPopupMenuEventArgs e)
        //{
        //    if (e.Control == this.gridControl1)
        //    {
        //        e.Cancel = false;
        //    }
        //    else
        //    {
        //        e.Cancel = true;
        //    }
        //}

        //private void gridControl1_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (gridViewOper.FocusedRowHandle < 0)
        //            return;
        //        else
        //        {
        //            DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
        //            this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
        //            this.btn_operafter_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //            this.btn_del_operbefore_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //            this.btn_del_Operinfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        //        }
        //    }
        //}

        private Inpatient CurrentInpatient;//add by ywk 
        /// <summary>
        /// edit by xlb 2013-01-23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                bool isSuit = Validate(ref message);
                if (!isSuit)
                {
                    //throw new Exception(message);//在这抛异常，岂不是抛出了系统级，应抛出信息为message
                    //edit by ywk 2013年2月26日8:42:25 
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(message);
                    return;
                }
                //设置当前病人(修复m_App病人丢失问题)
                if (null == m_App || null == m_App.CurrentPatientInfo || m_App.CurrentPatientInfo.NoOfFirstPage.ToString() != m_IemInfo.IemBasicInfo.NoOfInpat)
                {
                    CurrentInpatient = DS_SqlService.GetPatientInfo(m_IemInfo.IemBasicInfo.NoOfInpat);
                }
                else
                {
                    CurrentInpatient = m_App.CurrentPatientInfo;
                }

                GetUI();
                //edit by 2012-12-20 张业兴 关闭弹出框只关闭提示框
                //((ShowUC)this.Parent).Close(true, m_IemInfo);
                //点击确认按钮就将数据更新到数据库
                //CurrentInpatient = m_App.CurrentPatientInfo;
                if (null != CurrentInpatient)
                {
                    CurrentInpatient.ReInitializeAllProperties();
                }
                IemMainPageManger manger = new IemMainPageManger(m_App, CurrentInpatient);
                manger.SaveData(m_IemInfo);

                //add by cyq 2012-12-05 病案室人员编辑后状态改为已归档
                if (editFlag)
                {
                    DS_BaseService.SetRecordsRebacked(int.Parse(CurrentInpatient.NoOfFirstPage.ToString().Trim()));
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 校验方法
        /// xlb 2013-01-23
        /// </summary>
        /// <param name="message"></param>
        private bool Validate(ref string message)
        {
            try
            {
                //edit by ywk 2013年2月26日8:44:08  
                bool ischecked = true;//先定义参数
                //if (!chx_qj1.Checked && !chx_qj2.Checked)
                //{
                //    message = "请选择有无抢救";
                //    return false;
                //}
                //else
                //{
                if (!chx_qj1.Checked)
                {
                    if (string.IsNullOrEmpty(txt_qjCount.Text))
                    {
                        txt_qjCount.Focus();
                        message = "请输入抢救次数";
                        ischecked = false;
                    }
                    else if (string.IsNullOrEmpty(txt_qjSucceedCount.Text))
                    {
                        txt_qjSucceedCount.Focus();
                        message = "请输入成功次数";
                        ischecked = false;
                    }
                }
                else
                {
                    txt_qjCount.Text = string.Empty;
                    txt_qjSucceedCount.Text = string.Empty;

                    txt_qjCount.Enabled = false;
                    txt_qjSucceedCount.Enabled = false;
                    ischecked = true;
                }
                //}
                return ischecked;
                //return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
        }
        /// <summary>
        /// 控制宜昌提出的手术的几个列的显示性（根据配置）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void gridControl1_Load(object sender, EventArgs e)
        //{
        //    if (CanSEEControl == "0")
        //    {
        //        gridColumn12.Visible = false;
        //        gridColumn13.Visible = false;
        //        gridColumn20.Visible = false;
        //    }
        //    if (CanSEEControl == "1")
        //    {
        //        gridColumn12.Visible = true;
        //        gridColumn13.Visible = true;
        //        gridColumn20.Visible = true;
        //    }
        //}

        /// <summary>
        /// 更改，选中后可消除选择
        /// add by ywk 2012年7月23日 08:54:53
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_CheckedChanged(object sender, EventArgs e)
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
        /// 复选框按键事件
        /// 注：回车即勾选/不勾选
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-29</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chb_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                DS_Common.cbx_KeyPress(sender);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}
