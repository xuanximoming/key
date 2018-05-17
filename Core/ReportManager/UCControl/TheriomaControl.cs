using DevExpress.XtraEditors;
using DrectSoft.Common.Library;
using DrectSoft.Core.DSReportManager;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.ReportManager.UCControl
{
    /// <summary>
    ///二次更改此类文件，增加try catch
    ///add by ywk 2013年8月9日 13:47:17
    /// </summary>
    public partial class TheriomaControl : DevExpress.XtraEditors.XtraUserControl
    {

        IEmrHost m_Host;
        public ReportCardEntity m_ReportCardEntity;
        private SqlHelper m_SqlHelper;
        public string ReportID;//当前报告卡序号
        public string m_Noofinpat;//患者序号
        public int m_upCount = 0;//标识当前 诊断数据源，索引位置
        public DataTable m_dataTableDiagicd;//诊断数据源

        public string DiagICD10
        {
            get { return m_diagicd10; }
            set
            {
                m_diagicd10 = value;
            }
        }
        private string m_diagicd10;//当前诊断编号

        SqlHelper SqlHelper
        {
            get
            {
                if (m_SqlHelper == null)
                    m_SqlHelper = new SqlHelper(m_Host.SqlHelper);
                return m_SqlHelper;
            }
            set { m_SqlHelper = value; }
        }


        public TheriomaControl()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {

                throw;
            }
        }



        public TheriomaControl(IEmrHost app)
        {
            try
            {
                m_Host = app;
                InitializeComponent();
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 返回当前病案的诊断信息数据源
        /// </summary>
        /// <param name="noofinpat"></param>
        public void GetDataDiagNosis(string noofinpat)
        {
            try
            {
                //if (m_dataTableDiagicd == null)
                {
                    m_dataTableDiagicd = m_SqlHelper.GetDiagnosis(noofinpat);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }

        public TheriomaControl(IEmrHost app, string Iem_NO)
        {
            try
            {
                m_Host = app;
                InitializeComponent();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }


        /// <summary>
        /// 加载方法，传入初始化值
        /// </summary>
        /// <param name="ID">疾病报告卡ID或者新增疾病报告卡时候传入病人首页序号</param>
        /// <param name="type">1、传入病报告卡序号  2、传入病人首页序号</param>
        /// <param name="userRole">1、申请人  2、审核人</param>
        public void LoadPage(string id, string type, string userRole)
        {
            try
            {
                if (id == null)
                    return;
                SqlHelper = new SqlHelper(m_Host.SqlHelper);
                if (type == "1")
                {
                    m_ReportCardEntity = SqlHelper.GetReportCardEntity(id);
                }
                else if (type == "2")
                {
                    m_ReportCardEntity = SqlHelper.GetInpatientByNoofinpat(id);
                    m_ReportCardEntity.ReportDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                    m_ReportCardEntity.Report_ICD10 = m_diagicd10;
                    m_ReportCardEntity.ReportInfunit = m_Host.User.CurrentDeptId;
                    m_ReportCardEntity.ReportDoctor = m_Host.User.Id;
                    m_ReportCardEntity.ReportDoctorName = m_Host.User.Name;
                }
                ClearPage();
                FillUI(m_ReportCardEntity);
                ReadOnlyControl(userRole);
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }
        /// <summary>
        /// 更具权限，确定当前页面状态
        /// </summary>
        /// <param name="userRole"></param>
        public void ReadOnlyControl(string userRole)
        {
            try
            {
                if (userRole == "1")
                {
                    EnableState(true);
                }
                else if (userRole == "2")
                {
                    EnableState(false);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }

        #region 页面控件状态
        private void EnableState(bool isshow)
        {
            try
            {
                this.txtREPORT_DISTRICTNAME.Properties.ReadOnly = !isshow;
                this.txtREPORT_No.Properties.ReadOnly = !isshow;
                this.txtREPORT_DIAGNOSIS.Properties.ReadOnly = !isshow;
                this.txtREPORT_ICD0.Properties.ReadOnly = !isshow;

                this.txtREPORT_CLINICID.Properties.ReadOnly = !isshow;
                this.txtREPORT_PATID.Properties.ReadOnly = !isshow;
                this.txtREPORT_INDO.Properties.ReadOnly = !isshow;
                this.txtREPORT_INPATNAME.Properties.ReadOnly = !isshow;
                chkSex1.Properties.ReadOnly = !isshow;
                chkSex2.Properties.ReadOnly = !isshow;

                this.txtAge.Properties.ReadOnly = !isshow;
                this.dateBIRTHDATE.Properties.ReadOnly = !isshow;
                this.lueNation.ReadOnly = !isshow;
                this.txtCONTACTTEL.Properties.ReadOnly = !isshow;
                this.lueMarital.ReadOnly = !isshow;
                this.lueJob.ReadOnly = !isshow;
                this.txtOFFICEADDRESS.Properties.ReadOnly = !isshow;

                this.txtORGDISTRICTNAME.Properties.ReadOnly = !isshow;

                this.txtORGTOWN.Properties.ReadOnly = !isshow;

                this.txtORGVILLAGENAME.Properties.ReadOnly = !isshow;
                /*现住地址*/

                this.txtXZZDISTRICT.Properties.ReadOnly = !isshow;

                this.txtXZZTOWN.Properties.ReadOnly = !isshow;

                this.txtXZZVILLIAGE.Properties.ReadOnly = !isshow;

                this.txtREPORT_DIAGNOSIS.Properties.ReadOnly = !isshow;
                this.txtPATHOLOGICALTYPE.Properties.ReadOnly = !isshow;
                this.txtPATHOLOGICALID.Properties.ReadOnly = !isshow;

                this.txtQZDIAGTIME_T.Properties.ReadOnly = !isshow;
                this.txtQZDIAGTIME_M.Properties.ReadOnly = !isshow;
                this.txtQZDIAGTIME_N.Properties.ReadOnly = !isshow;

                this.dateFIRSTDIADATE.Properties.ReadOnly = !isshow;

                //this.lookUpEditorDept.Properties.ReadOnly = !isshow;
                //this.lookUpEditorDoc.Properties.ReadOnly = !isshow;
                lookUpEditorDept.ReadOnly = !isshow;
                lookUpEditorDoc.ReadOnly = !isshow;
                lookDiagDept.ReadOnly = !isshow;

                this.dateREPORTDATE.Properties.ReadOnly = !isshow;
                this.deteDEATHDATE.Properties.ReadOnly = !isshow;
                this.txtDEATHREASON.Properties.ReadOnly = !isshow;
                this.memRejest.Properties.ReadOnly = !isshow;
                this.lookUpEditorOutDiag.ReadOnly = !isshow;
                this.dateREPORT_YDIAGNOSISDATA.Properties.ReadOnly = !isshow;
                this.ckeDiagnosis_0.Properties.ReadOnly = !isshow;
                this.ckeDiagnosis_1.Properties.ReadOnly = !isshow;
                this.ckeDiagnosis_2.Properties.ReadOnly = !isshow;
                this.ckeDiagnosis_3.Properties.ReadOnly = !isshow;
                this.ckeDiagnosis_4.Properties.ReadOnly = !isshow;
                this.ckeDiagnosis_5.Properties.ReadOnly = !isshow;
                this.ckeDiagnosis_6.Properties.ReadOnly = !isshow;
                this.ckeDiagnosis_7.Properties.ReadOnly = !isshow;
                this.ckeDiagnosis_8.Properties.ReadOnly = !isshow;
                this.ckeDiagnosis_9.Properties.ReadOnly = !isshow;
                //SetDateEditState(isshow, this.Controls);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }

        //private void SetDateEditState(bool isshow, ControlCollection ctls)
        //{
        //    foreach (Control ctl in ctls)
        //    {
        //        if (ctl.Controls.Count > 0)
        //        {
        //            SetDateEditState(isshow, ctl.Controls);
        //        }
        //        DateEdit dateEdit = ctl as DateEdit;
        //        if (dateEdit != null)
        //        {
        //            if (isshow)
        //            {
        //                dateEdit.Properties.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.SingleClick;
        //            }
        //            else
        //            {
        //                dateEdit.Properties.ShowDropDown = DevExpress.XtraEditors.Controls.ShowDropDown.Never;
        //            }
        //        }
        //    }
        //}
        #endregion

        /// <summary>
        /// 初始化lookupeditor
        /// </summary>
        private void InitLookUpEditor()
        {
            try
            {
                InitMarital();
                InitJob();
                InitNation();
                InitDepartment();
                InitDoctor("");
                InitOutDiag();
                InitDiagICD10();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 初始化数据
        /// <summary>
        /// 诊断
        /// </summary>
        private void InitOutDiag()
        {
            try
            {
                string sql_diag = "select icd,name,py,wb from diagnosis";
                DataTable diag = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql_diag);
                diag.Columns["ICD"].Caption = "编号";
                diag.Columns["NAME"].Caption = "诊断名称";
                Dictionary<string, int> colDiag = new Dictionary<string, int>();
                colDiag.Add("ICD", 80);
                colDiag.Add("NAME", 160);

                LookUpWindow lookUpWindowInDiag = new LookUpWindow();
                this.lookUpEditorOutDiag.ListWindow = lookUpWindowInDiag;
                SqlWordbook outWordBook = new SqlWordbook("outDiag", diag, "ICD", "NAME", colDiag, "ICD//NAME//PY//WB");
                this.lookUpEditorOutDiag.SqlWordbook = outWordBook;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }

        /// <summary>
        /// 初始化科室
        /// </summary>
        private void InitDepartment()
        {
            try
            {
                lookUpWindowDept.SqlHelper = m_Host.SqlHelper;
                string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");
                DataTable Dept = m_Host.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", 60);
                cols.Add("NAME", 150);
                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorDept.SqlWordbook = deptWordBook;

                lookDiagDept.SqlWordbook = deptWordBook;

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }


        }

        /// <summary>
        /// 初始化ICD编码 Add By wwj 2013-07-23
        /// </summary>
        public void InitDiagICD10() //(string noofinpat, bool isNew)
        {
            try
            {
                if (lookUpEditorDialogICD10.SqlWordbook == null)
                {
                    LookUpWindow lupInfo = new LookUpWindow();
                    lupInfo.SqlHelper = m_Host.SqlHelper;

                    //根据当前病人获取诊断编码和名称
                    string sql = string.Empty;

                    //                if (isNew)
                    //                {
                    //                    sql = @"select icd, name, tumorid, py, wb
                    //                                 from zymosis_diagnosis z 
                    //                                where z.icd in (
                    //                                    select imd.diagnosis_code from {0} imd, {1} imb
                    //                                     where imd.iem_mainpage_no = imb.iem_mainpage_no and imb.noofinpat = {2} and imb.valide = '1' and imd.valide = '1'
                    //                                   ) 
                    //                                  and (select count(1) from theriomareportcard zr 
                    //                                        where zr.diagicd10 = z.icd
                    //                                          and zr.report_noofinpat = {2}
                    //                                          and zr.diagicd10 = z.icd 
                    //                                          and zr.vaild=1
                    //                                  ) < z.upcount
                    //                                  and z.valid = '1'";
                    //                }
                    //                else
                    //                {
                    //                    sql = @"select icd, name, tumorid, py, wb
                    //                                 from zymosis_diagnosis z 
                    //                                where z.icd in (
                    //                                    select imd.diagnosis_code from {0} imd, {1} imb
                    //                                     where imd.iem_mainpage_no = imb.iem_mainpage_no and imb.noofinpat = {2} and imb.valide = '1' and imd.valide = '1'
                    //                                   )
                    //                                  and z.valid = '1'";
                    //                }

                    //                string valueStr = Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage").ToUpper();
                    //                if (valueStr.IndexOf("_SX") > -1)
                    //                {
                    //                    sql = string.Format(sql, "iem_mainpage_diagnosis_sx", "iem_mainpage_basicinfo_sx", noofinpat);
                    //                }
                    //                else
                    //                {
                    //                    sql = string.Format(sql, "iem_mainpage_diagnosis_2012", "iem_mainpage_basicinfo_2012", noofinpat);
                    //                }


                    sql = @"select icd, name, tumorid, py, wb from zymosis_diagnosis z where z.categoryid = '2' and z.valid = '1'";

                    DataTable dataTable = m_Host.SqlHelper.ExecuteDataTable(sql);
                    dataTable.Columns["ICD"].Caption = "ICD编码";
                    dataTable.Columns["NAME"].Caption = "编码名称";

                    Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
                    columnwidth.Add("ICD", 80);
                    columnwidth.Add("NAME", 120);

                    SqlWordbook sqlWordBook = new SqlWordbook("querybook", dataTable, "ICD", "NAME", columnwidth, "ICD//NAME//PY//WB");
                    lookUpEditorDialogICD10.SqlWordbook = sqlWordBook;
                    lookUpEditorDialogICD10.ListWindow = lupInfo;
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        private void InitMarital()
        {
            try
            {
                if (lueMarital.SqlWordbook == null)
                    BindLueData(lueMarital, 3);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }

        /// <summary>
        /// 职业代码
        /// </summary>
        private void InitJob()
        {
            try
            {
                if (lueJob.SqlWordbook == null)
                    BindLueData(lueJob, 4);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }

        /// <summary>
        /// 民族代码
        /// </summary>
        private void InitNation()
        {
            try
            {
                if (lueNation.SqlWordbook == null)
                    BindLueData(lueNation, 6);

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }
        }

        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            try
            {
                LookUpWindow lupInfo = new LookUpWindow();
                lupInfo.SqlHelper = m_Host.SqlHelper;
                DataTable dataTable = GetEditroData(queryType);

                Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
                columnwidth.Add("名称", lueInfo.Width);
                SqlWordbook sqlWordBook = new SqlWordbook("ID", dataTable, "ID", "Name", columnwidth, true);

                lueInfo.SqlWordbook = sqlWordBook;
                lueInfo.ListWindow = lupInfo;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }
        /// <summary>
        /// 获取lue的数据源
        /// </summary>
        /// <param name="queryType"></param>
        /// <returns></returns>
        private DataTable GetEditroData(Decimal queryType)
        {
            try
            {
                SqlParameter paraType = new SqlParameter("@QueryType", SqlDbType.Decimal);
                paraType.Value = queryType;
                SqlParameter[] paramCollection = new SqlParameter[] { paraType };
                DataTable dataTable = AddTableColumn(m_Host.SqlHelper.ExecuteDataTable("usp_GetLookUpEditorData", paramCollection, CommandType.StoredProcedure));
                return dataTable;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return null;
            }

        }

        /// <summary>
        /// 给lue的数据源，新增 名称 栏位
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private DataTable AddTableColumn(DataTable dataTable)
        {
            try
            {
                DataTable dataTableAdd = dataTable;
                if (!dataTableAdd.Columns.Contains("名称"))
                    dataTableAdd.Columns.Add("名称");
                foreach (DataRow row in dataTableAdd.Rows)
                    row["名称"] = row["Name"].ToString();
                return dataTableAdd;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return null;
            }

        }
        private void lookUpEditorDept_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                InitDoctor(this.lookUpEditorDept.CodeValue);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }

        /// <summary>
        /// 绑定医生下拉框
        /// </summary>
        private void InitDoctor(string deptid)
        {
            ///修改时间：2012-08-03
            ///修改人：cyq
            try
            {
                lookUpWindowDoc.SqlHelper = m_Host.SqlHelper;
                string sql = string.Empty;
                if (string.IsNullOrEmpty(deptid))
                {
                    sql = string.Format(@"select distinct u.ID,u.NAME,u.PY,u.WB,u.grade from Users u join categorydetail c on u.grade=c.id and c.categoryid='20' and  c.id in('2000','2001','2002','2003') ");
                }
                else
                {
                    sql = string.Format(@"select distinct u.ID,u.NAME,u.PY,u.WB,u.grade from Users u join categorydetail c on u.grade=c.id and c.categoryid='20' and  c.id in('2000','2001','2002','2003') and u.deptid = '{0}' ", deptid);
                }
                DataTable Bzlb = m_Host.SqlHelper.ExecuteDataTable(sql);

                Bzlb.Columns["ID"].Caption = "医生工号";
                Bzlb.Columns["NAME"].Caption = "医生名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 90);
                cols.Add("NAME", 120);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorDoc.SqlWordbook = deptWordBook;

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }


        }
        #endregion

        private void TheriomaControl_Load(object sender, EventArgs e)
        {
            try
            {
                InitLookUpEditor();
                //控制标题显示内容
                string cardtype = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("TheriomaCardTip");
                if (cardtype == "0")
                {
                    labelControl1.Text = "江西省居民肿瘤病例报告卡";
                }
                else
                {
                    labelControl1.Text = "恶性肿瘤病例报告卡";
                }
                ControlSetVisable(cardtype);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }
        }
        /// <summary>
        /// 控制中心医院和九江中医院的控件显示
        /// </summary>
        /// <param name="cardtype"></param>
        private void ControlSetVisable(string cardtype)
        {
            try
            {
                if (cardtype == "1")//中心的
                {
                    chksick.Visible = true;
                    chkdead.Visible = true;
                    label57.Visible = true;
                    label41.Visible = false;
                    memRejest.Visible = false;
                    label58.Visible = true;
                    lookDiagDept.Visible = true;
                    label59.Visible = true;
                    txtclinicalstages.Visible = true;
                    label60.Visible = true;


                }
                if (cardtype == "0")//九江
                {
                    chksick.Visible = false;
                    chkdead.Visible = false;
                    label57.Visible = false;

                    label41.Visible = true;
                    memRejest.Visible = true;

                    label58.Visible = false;
                    lookDiagDept.Visible = false;
                    label59.Visible = false;
                    txtclinicalstages.Visible = false;
                    label60.Visible = false;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }

        }


        /// <summary>
        /// 清空页面控件值
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-28
        /// 1、add try ... catch
        /// 2、附卡清空(如果存在附卡)
        public void ClearPage()
        {
            try
            {
                this.txtREPORT_No.Text = "";
                this.txtREPORT_DISTRICTNAME.Text = "";
                this.lookUpEditorDialogICD10.CodeValue = "";
                this.lookUpEditorDialogICD10.Text = "";
                this.txtREPORT_ICD0.Text = "";
                this.txtREPORT_CLINICID.Text = "";
                this.txtREPORT_PATID.Text = "";
                this.txtREPORT_INDO.Text = "";
                this.txtREPORT_INPATNAME.Text = "";
                this.chkSex1.Checked = false;
                this.chkSex2.Checked = false;

                this.dateBIRTHDATE.Text = "";
                this.txtAge.Value = 0;
                this.lueNation.CodeValue = "";
                this.txtCONTACTTEL.Text = "";
                this.lueMarital.CodeValue = "";
                this.lueJob.CodeValue = "";
                this.txtOFFICEADDRESS.Text = "";

                this.txtORGDISTRICTNAME.Text = "";
                //_ReportCardEntity.OrgTownId = "";
                this.txtORGTOWN.Text = "";
                //_ReportCardEntity.OrgVillage = "";
                this.txtORGVILLAGENAME.Text = "";
                /*现住地址*/
                //_ReportCardEntity.XZZProvinceId = "";
                //_ReportCardEntity.XZZProvince = "";
                //_ReportCardEntity.XZZCityId = "";
                //_ReportCardEntity.XZZCity = "";
                //_ReportCardEntity.XZZDistrictId = "";
                this.txtXZZDISTRICT.Text = "";
                //_ReportCardEntity.XZZTownId = "";
                this.txtXZZTOWN.Text = "";
                //_ReportCardEntity.XZZVillageId = "";
                this.txtXZZVILLIAGE.Text = "";

                this.txtREPORT_DIAGNOSIS.Text = "";
                this.txtPATHOLOGICALTYPE.Text = "";
                this.txtPATHOLOGICALID.Text = "";

                this.txtQZDIAGTIME_T.Text = "";
                this.txtQZDIAGTIME_M.Text = "";
                this.txtQZDIAGTIME_N.Text = "";

                this.dateFIRSTDIADATE.Text = "";

                this.lookUpEditorDept.CodeValue = "";
                this.lookDiagDept.CodeValue = "";
                this.lookUpEditorDoc.CodeValue = "";
                this.dateREPORTDATE.Text = "";
                this.deteDEATHDATE.Text = "";
                this.txtDEATHREASON.Text = "";
                this.memRejest.Text = "";
                this.lookUpEditorOutDiag.CodeValue = "";
                this.dateREPORT_YDIAGNOSISDATA.Text = "";

                this.ckeDiagnosis_0.Checked = false;
                this.ckeDiagnosis_1.Checked = false;
                this.ckeDiagnosis_2.Checked = false;
                this.ckeDiagnosis_3.Checked = false;
                this.ckeDiagnosis_4.Checked = false;
                this.ckeDiagnosis_5.Checked = false;
                this.ckeDiagnosis_6.Checked = false;
                this.ckeDiagnosis_7.Checked = false;
                this.ckeDiagnosis_8.Checked = false;
                this.ckeDiagnosis_9.Checked = false;
                //发病卡和死亡卡的控制 add by ywk 2013年8月5日 10:42:08
                this.chkdead.Checked = false;
                this.chksick.Checked = false;

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }
        }
        /// <summary>
        /// 初始化页面赋值
        /// </summary>
        /// <param name="_ReportCardEntity"></param>
        private void FillUI(ReportCardEntity _ReportCardEntity)
        {
            try
            {
                if (_ReportCardEntity == null)
                {
                    return;
                }
                else
                {
                    #region  绑定实体
                    m_Noofinpat = _ReportCardEntity.Report_Noofinpat;
                    this.txtREPORT_No.Text = _ReportCardEntity.Report_No;
                    this.txtREPORT_DISTRICTNAME.Text = _ReportCardEntity.Report_DistrictName;
                    this.lookUpEditorDialogICD10.CodeValue = _ReportCardEntity.Report_ICD10;
                    this.txtREPORT_ICD0.Text = _ReportCardEntity.Report_ICD0;

                    this.txtREPORT_CLINICID.Text = _ReportCardEntity.Report_ClinicId;
                    this.txtREPORT_PATID.Text = _ReportCardEntity.Report_PatId;
                    this.txtREPORT_INDO.Text = _ReportCardEntity.Report_INDO;
                    this.txtREPORT_INPATNAME.Text = _ReportCardEntity.Report_InpatName;
                    if (_ReportCardEntity.SexId == "1")
                        chkSex1.Checked = true;
                    else if (_ReportCardEntity.SexId == "2")
                        chkSex2.Checked = true;

                    this.dateBIRTHDATE.Text = _ReportCardEntity.BirthDate;
                    if (!string.IsNullOrEmpty(_ReportCardEntity.BirthDate))
                    {
                        int age = MethodHelper.ComputeAge(Convert.ToDateTime(_ReportCardEntity.BirthDate));
                        this.txtAge.Value = age;
                    }
                    else
                    {
                        this.txtAge.Value = 0;
                    }

                    this.lueNation.CodeValue = _ReportCardEntity.NationId;
                    //this.lueNation.Text = _ReportCardEntity.NationName;
                    this.txtCONTACTTEL.Text = _ReportCardEntity.ContactTel;
                    this.lueMarital.CodeValue = _ReportCardEntity.Martial;
                    this.lueJob.CodeValue = _ReportCardEntity.Occupation;
                    this.txtOFFICEADDRESS.Text = _ReportCardEntity.OfficeAddress;
                    /*户口所在地*/
                    //_ReportCardEntity.OrgProvinceId = "";
                    //_ReportCardEntity.OrgProvinceName = "";
                    //_ReportCardEntity.OrgCityId = "";
                    //_ReportCardEntity.OrgCityName = "";
                    //_ReportCardEntity.OrgDistrictId = "";
                    this.txtORGDISTRICTNAME.Text = _ReportCardEntity.OrgDistrictName;
                    //_ReportCardEntity.OrgTownId = "";
                    this.txtORGTOWN.Text = _ReportCardEntity.OrgTown;
                    //_ReportCardEntity.OrgVillage = "";
                    this.txtORGVILLAGENAME.Text = _ReportCardEntity.OrgVillageName;
                    /*现住地址*/
                    //_ReportCardEntity.XZZProvinceId = "";
                    //_ReportCardEntity.XZZProvince = "";
                    //_ReportCardEntity.XZZCityId = "";
                    //_ReportCardEntity.XZZCity = "";
                    //_ReportCardEntity.XZZDistrictId = "";
                    this.txtXZZDISTRICT.Text = _ReportCardEntity.XZZDistrict;
                    //_ReportCardEntity.XZZTownId = "";
                    this.txtXZZTOWN.Text = _ReportCardEntity.XZZTown;
                    //_ReportCardEntity.XZZVillageId = "";
                    this.txtXZZVILLIAGE.Text = _ReportCardEntity.XZZVillage;

                    this.txtREPORT_DIAGNOSIS.Text = _ReportCardEntity.Report_Diagnosis;
                    this.txtPATHOLOGICALTYPE.Text = _ReportCardEntity.PathologicalType;
                    this.txtPATHOLOGICALID.Text = _ReportCardEntity.PathologicalId;

                    this.txtQZDIAGTIME_T.Text = _ReportCardEntity.QZDigTime_T;
                    this.txtQZDIAGTIME_M.Text = _ReportCardEntity.QZDigTime_M;
                    this.txtQZDIAGTIME_N.Text = _ReportCardEntity.QZDiaTime_N;

                    this.dateFIRSTDIADATE.Text = _ReportCardEntity.FirstDiaDate;

                    this.lookUpEditorDept.CodeValue = _ReportCardEntity.ReportInfunit;

                    this.lookDiagDept.CodeValue = _ReportCardEntity.ReportDiagfunit;
                    this.txtclinicalstages.Text = _ReportCardEntity.ClinicalStages;//临床分期

                    this.lookUpEditorDoc.CodeValue = _ReportCardEntity.ReportDoctor;
                    this.dateREPORTDATE.Text = _ReportCardEntity.ReportDate;
                    this.deteDEATHDATE.Text = _ReportCardEntity.DeathDate;
                    this.txtDEATHREASON.Text = _ReportCardEntity.DeathReason;
                    this.memRejest.Text = _ReportCardEntity.ReJest;
                    this.lookUpEditorOutDiag.CodeValue = _ReportCardEntity.Report_YdiagNosis;
                    this.dateREPORT_YDIAGNOSISDATA.Text = _ReportCardEntity.Report_YdiagNosisData;
                    if (_ReportCardEntity.ReportCardType.Trim() == "0")//发病
                    {
                        this.chksick.Checked = true;
                    }
                    if (_ReportCardEntity.ReportCardType.Trim() == "1")//死亡
                    {
                        this.chkdead.Checked = true;
                    }

                    if (!String.IsNullOrEmpty(_ReportCardEntity.Report_DiagNosisBased))
                    {
                        string[] strArray = _ReportCardEntity.Report_DiagNosisBased.Split(new char[] { ',' });
                        foreach (string str in strArray)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                switch (str)
                                {
                                    case "1":
                                        this.ckeDiagnosis_1.Checked = true;
                                        break;
                                    case "2":
                                        this.ckeDiagnosis_2.Checked = true;
                                        break;
                                    case "3":
                                        this.ckeDiagnosis_3.Checked = true;
                                        break;
                                    case "4":
                                        this.ckeDiagnosis_4.Checked = true;
                                        break;
                                    case "5":
                                        this.ckeDiagnosis_5.Checked = true;
                                        break;
                                    case "6":
                                        this.ckeDiagnosis_6.Checked = true;
                                        break;
                                    case "7":
                                        this.ckeDiagnosis_7.Checked = true;
                                        break;
                                    case "8":
                                        this.ckeDiagnosis_8.Checked = true;
                                        break;
                                    case "9":
                                        this.ckeDiagnosis_9.Checked = true;
                                        break;
                                    case "0":
                                        this.ckeDiagnosis_0.Checked = true;
                                        break;
                                }
                            }
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }
                    #endregion


        }

        /// <summary>
        /// 给实体赋值
        /// </summary>
        /// <param name="_ReportCardEntity"></param>
        /// <returns></returns>
        public ReportCardEntity GetEntityUI(ReportCardEntity _ReportCardEntity)
        {
            try
            {

                //_ReportCardEntity.Report_Id = this.txtREPORT_No.Text;
                //_ReportCardEntity.Report_DistrictId = "";
                _ReportCardEntity.Report_DistrictName = this.txtREPORT_DISTRICTNAME.Text;
                _ReportCardEntity.Report_ICD10 = this.lookUpEditorDialogICD10.CodeValue;
                _ReportCardEntity.Report_ICD0 = this.txtREPORT_ICD0.Text;
                _ReportCardEntity.Report_ClinicId = this.txtREPORT_CLINICID.Text;
                _ReportCardEntity.Report_PatId = this.txtREPORT_PATID.Text;
                _ReportCardEntity.Report_INDO = this.txtREPORT_INDO.Text;
                _ReportCardEntity.Report_InpatName = this.txtREPORT_INPATNAME.Text;

                if (chkSex1.Checked)
                    _ReportCardEntity.SexId = "1";
                else if (chkSex2.Checked)
                    _ReportCardEntity.SexId = "2";


                //_ReportCardEntity.RealAge = this.txtAge.Value.ToString();


                _ReportCardEntity.BirthDate = this.dateBIRTHDATE.Text;

                if (!string.IsNullOrEmpty(_ReportCardEntity.BirthDate))
                {
                    _ReportCardEntity.RealAge = this.txtAge.Text;
                }
                else
                {
                    _ReportCardEntity.RealAge = "0";
                }

                _ReportCardEntity.NationId = this.lueNation.CodeValue;
                _ReportCardEntity.NationName = this.lueNation.Text;
                _ReportCardEntity.ContactTel = this.txtCONTACTTEL.Text;
                _ReportCardEntity.Martial = this.lueMarital.CodeValue;
                _ReportCardEntity.Occupation = this.lueJob.CodeValue;
                _ReportCardEntity.OfficeAddress = this.txtOFFICEADDRESS.Text;
                /*户口所在地*/
                //_ReportCardEntity.OrgProvinceId = "";
                //_ReportCardEntity.OrgCityId = "";
                //_ReportCardEntity.OrgDistrictId = "";
                //_ReportCardEntity.OrgTownId = "";
                //_ReportCardEntity.OrgVillage = "";

                //_ReportCardEntity.OrgProvinceName = "";
                //_ReportCardEntity.OrgCityName = "";
                _ReportCardEntity.OrgDistrictName = this.txtORGDISTRICTNAME.Text;
                _ReportCardEntity.OrgTown = this.txtORGTOWN.Text;
                _ReportCardEntity.OrgVillageName = this.txtORGVILLAGENAME.Text;
                /*现住地址*/
                //_ReportCardEntity.XZZProvinceId = "";
                //_ReportCardEntity.XZZCityId = "";
                //_ReportCardEntity.XZZDistrictId = "";
                //_ReportCardEntity.XZZTownId = "";
                //_ReportCardEntity.XZZVillageId = "";

                //_ReportCardEntity.XZZProvince = "";
                //_ReportCardEntity.XZZCity = "";
                _ReportCardEntity.XZZDistrict = this.txtXZZDISTRICT.Text;
                _ReportCardEntity.XZZTown = this.txtXZZTOWN.Text;
                _ReportCardEntity.XZZVillage = this.txtXZZVILLIAGE.Text;

                _ReportCardEntity.Report_Diagnosis = this.txtREPORT_DIAGNOSIS.Text;
                _ReportCardEntity.PathologicalType = this.txtPATHOLOGICALTYPE.Text;
                _ReportCardEntity.PathologicalId = this.txtPATHOLOGICALID.Text;

                _ReportCardEntity.QZDigTime_T = this.txtQZDIAGTIME_T.Text;
                _ReportCardEntity.QZDigTime_M = this.txtQZDIAGTIME_M.Text;
                _ReportCardEntity.QZDiaTime_N = this.txtQZDIAGTIME_N.Text;

                _ReportCardEntity.FirstDiaDate = this.dateFIRSTDIADATE.Text;

                _ReportCardEntity.ReportInfunit = this.lookUpEditorDept.CodeValue;
                _ReportCardEntity.ReportDiagfunit = this.lookDiagDept.CodeValue;
                _ReportCardEntity.ClinicalStages = this.txtclinicalstages.Text.Trim();//临床分期
                _ReportCardEntity.ReportDoctor = this.lookUpEditorDoc.CodeValue;
                _ReportCardEntity.ReportDate = this.dateREPORTDATE.Text;
                _ReportCardEntity.DeathDate = this.deteDEATHDATE.Text;
                _ReportCardEntity.DeathReason = this.txtDEATHREASON.Text;
                _ReportCardEntity.ReJest = this.memRejest.Text;
                _ReportCardEntity.Report_YdiagNosis = this.lookUpEditorOutDiag.CodeValue;
                _ReportCardEntity.Report_YdiagNosisData = this.dateREPORT_YDIAGNOSISDATA.Text;

                _ReportCardEntity.Report_Noofinpat = m_Noofinpat;
                _ReportCardEntity.Report_No = this.txtREPORT_No.Text;


                if (chkdead.Checked)
                {
                    _ReportCardEntity.ReportCardType = "1";//死亡
                }
                if (chksick.Checked)
                {
                    _ReportCardEntity.ReportCardType = "0";//发病
                }
                if (chksick.Checked && chkdead.Checked)//发病死亡都勾选，算死亡
                {
                    _ReportCardEntity.ReportCardType = "1";//死亡
                }

                #region 诊断依据
                string DiagNosisBased = string.Empty;
                if (ckeDiagnosis_1.Checked)
                {
                    DiagNosisBased += ",1";
                }
                if (ckeDiagnosis_2.Checked)
                {
                    DiagNosisBased += ",2";
                }
                if (ckeDiagnosis_3.Checked)
                {
                    DiagNosisBased += ",3";
                }
                if (ckeDiagnosis_4.Checked)
                {
                    DiagNosisBased += ",4";
                }
                if (ckeDiagnosis_5.Checked)
                {
                    DiagNosisBased += ",5";
                }
                if (ckeDiagnosis_6.Checked)
                {
                    DiagNosisBased += ",6";
                }
                if (ckeDiagnosis_7.Checked)
                {
                    DiagNosisBased += ",7";
                }
                if (ckeDiagnosis_8.Checked)
                {
                    DiagNosisBased += ",8";
                }
                if (ckeDiagnosis_9.Checked)
                {
                    DiagNosisBased += ",9";
                }
                if (ckeDiagnosis_0.Checked)
                {
                    DiagNosisBased += ",0";
                }

                if (!string.IsNullOrEmpty(DiagNosisBased))
                {
                    DiagNosisBased += ",";
                }

                _ReportCardEntity.Report_DiagNosisBased = DiagNosisBased;
                #endregion


            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return null;
            }

            return _ReportCardEntity;
        }
        /// <summary>
        /// 删除
        /// </summary>
        public bool Cancel()
        {
            try
            {
                if (m_ReportCardEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return false;
                }

                if (m_ReportCardEntity.Report_Id == "0")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该报告卡上报记录尚未保存或提交，不需要删除");
                    return false;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除吗？", "删除报告卡上报记录", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    ReportCardEntity _ReportCardEntity = GetEntityUI(m_ReportCardEntity);
                    if (_ReportCardEntity == null)
                        return false;
                    _ReportCardEntity.State = "7";

                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);

                    SqlHelper.EditReportCard(_ReportCardEntity);
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");
                    return true;
                }
                return false;
            }
            catch
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除失败");
                return false;
            }

        }

        /// <summary>
        /// 保存验证
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-13 add try ... catch
        public bool Save()
        {
            try
            {
                if (m_ReportCardEntity == null && m_dataTableDiagicd == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条病人信息或补录传染病报告信息");
                    return false;
                }

                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return false;
                }

                string ErrorMsg;//用于在判断保存状态下，单据状态已经改变导致保存失败，而返回的消息
                //保存数据
                bool boo = PrivateSave(out ErrorMsg);
                if (!boo)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ErrorMsg);
                }
                return boo;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 审核通过
        /// </summary>
        public bool Approv()
        {
            try
            {
                if (m_ReportCardEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return false;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要审核通过吗？", "审核通过", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    ReportCardEntity _ReportCardEntity = GetEntityUI(m_ReportCardEntity);
                    if (_ReportCardEntity == null)
                        return false;
                    _ReportCardEntity.State = "4";

                    _ReportCardEntity.AuditDate = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
                    _ReportCardEntity.AuditDeptcode = m_Host.User.CurrentDeptId;
                    _ReportCardEntity.AuditDeptname = m_Host.User.CurrentDeptName;
                    _ReportCardEntity.AuditUsercode = m_Host.User.Id;
                    _ReportCardEntity.AuditUsername = m_Host.User.Name;

                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    try
                    {
                        SqlHelper.EditReportCard(_ReportCardEntity);
                        //m_ReportCardEntity = null;
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("审核成功");
                        this.m_ReportCardEntity.State = "4";
                        return true;
                    }
                    catch
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("审核失败");
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return false;
            }
        }



        /// <summary>
        /// 退回，审核未通过
        /// </summary>
        public bool UnPassApprove()
        {
            try
            {
                if (m_ReportCardEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return false;
                }


                string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AuditUserCode");

                if (valueStr.ToLower().Contains("," + m_Host.User.Id + ","))
                {
                    //调用填写驳回原因窗口
                    UnPassReason unPassReason = new UnPassReason(m_Host);
                    if (unPassReason.ShowDialog() == DialogResult.OK)//点击确定按钮
                    {
                        //解决如果否决原因为空也可以进行否决操作BUG  eidt by ywk 2012年3月28日8:59:17
                        MemoEdit memoEditReason = unPassReason.Controls["memoEditReason"] as MemoEdit;
                        string rejectmemo = memoEditReason.Text.ToString();//取得否决原因
                        //判断是否填写了否决原因
                        if (!string.IsNullOrEmpty(rejectmemo))
                        {
                            m_ReportCardEntity.CANCELREASON = unPassReason.PassReason;
                            ReportCardEntity _ReportCardEntity = GetEntityUI(m_ReportCardEntity);
                            if (_ReportCardEntity == null)
                                return false;
                            _ReportCardEntity.State = "5";

                            if (SqlHelper == null)
                                SqlHelper = new SqlHelper(m_Host.SqlHelper);
                            try
                            {
                                m_SqlHelper.EditReportCard(_ReportCardEntity);
                                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("驳回成功");
                                m_ReportCardEntity.State = "5";
                                //m_ReportCardEntity = null;
                                return true;
                            }
                            catch
                            {
                                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("驳回失败");
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else
                    {
                        return false;
                    }


                    //return true;
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("对不起您没有审核权限");
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 保存时检查数据
        /// <summary>
        /// UI画面验证
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-24</date>
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-12 增加附卡验证(如果存在附卡)
        /// <returns></returns>
        private string CheckItem()
        {
            if (string.IsNullOrEmpty(this.txtREPORT_DISTRICTNAME.Text.Trim()))
            {
                this.txtREPORT_DISTRICTNAME.Focus();
                return "请输入所属区县";
            }
            else if (string.IsNullOrEmpty(this.txtREPORT_No.Text.Trim()))
            {
                this.txtREPORT_No.Focus();
                return "请输入报告卡编号";
            }
            else if (string.IsNullOrEmpty(this.txtREPORT_CLINICID.Text.Trim()))
            {
                this.txtREPORT_CLINICID.Focus();
                return "请输入门诊号";
            }
            else if (string.IsNullOrEmpty(this.txtREPORT_PATID.Text.Trim()))
            {
                this.txtREPORT_PATID.Focus();
                return "请输入住院号";
            }
            else if (string.IsNullOrEmpty(this.txtREPORT_INDO.Text.Trim()))
            {
                this.txtREPORT_INDO.Focus();
                return "请输入身份证号";
            }
            else if (string.IsNullOrEmpty(this.txtREPORT_INPATNAME.Text.Trim()))
            {
                this.txtREPORT_INPATNAME.Focus();
                return "请输入患者名称";
            }
            else if (chkSex1.Checked == false && chkSex2.Checked == false)
            {
                this.chkSex1.Focus();
                return "请输入性别";
            }
            else if (string.IsNullOrEmpty(this.dateBIRTHDATE.Text.Trim()))
            {
                this.dateBIRTHDATE.Focus();
                return "请输入出生年月";
            }
            else if (string.IsNullOrEmpty(this.lueNation.CodeValue.Trim())
                || string.IsNullOrEmpty(this.lueNation.Text.Trim()))
            {
                this.lueNation.Focus();
                return "请选择民族";
            }
            //else if (string.IsNullOrEmpty(this.txtCONTACTTEL.Text.Trim()))
            //{
            //    this.txtCONTACTTEL.Focus();
            //    return "请输入家庭电话";
            //}
            else if (string.IsNullOrEmpty(this.lueMarital.CodeValue.Trim())
                || string.IsNullOrEmpty(this.lueMarital.Text.Trim()))
            {
                this.lueMarital.Focus();
                return "请选择婚姻";
            }
            //else if (string.IsNullOrEmpty(this.lookUpEditorOutDiag.Text.Trim()))
            //{
            //    this.lookUpEditorOutDiag.Focus();
            //    return "请输入原诊断";
            //}
            //else if (string.IsNullOrEmpty(this.dateREPORT_YDIAGNOSISDATA.Text.Trim()))
            //{
            //    this.dateREPORT_YDIAGNOSISDATA.Focus();
            //    return "请输入原诊断日期";
            //}
            //else if (string.IsNullOrEmpty(this.lueJob.Text.Trim()))
            //{
            //    this.lueJob.Focus();
            //    return "请输入职业";
            //}
            //else if (string.IsNullOrEmpty(this.txtOFFICEADDRESS.Text.Trim()))
            //{
            //    this.txtOFFICEADDRESS.Focus();
            //    return "请输入工作单位";
            //}
            //else if (string.IsNullOrEmpty(this.txtORGDISTRICTNAME.Text.Trim()))
            //{
            //    this.txtORGDISTRICTNAME.Focus();
            //    return "请输入户口地址";
            //}
            //else if (string.IsNullOrEmpty(this.txtORGTOWN.Text.Trim()))
            //{
            //    this.txtORGTOWN.Focus();
            //    return "请输入户口区（县）";
            //}
            //else if (string.IsNullOrEmpty(this.txtORGVILLAGENAME.Text.Trim()))
            //{
            //    this.txtORGVILLAGENAME.Focus();
            //    return "请输入户口街道（乡镇）";
            //}
            //else if (string.IsNullOrEmpty(this.txtORGVILLAGENAME.Text.Trim()))
            //{
            //    this.txtORGVILLAGENAME.Focus();
            //    return "请输入户口居委会（村）";
            //}
            //else if (string.IsNullOrEmpty(this.txtXZZDISTRICT.Text.Trim()))
            //{
            //    this.txtXZZDISTRICT.Focus();
            //    return "请输入现住区（县）";
            //}
            //else if (string.IsNullOrEmpty(this.txtXZZTOWN.Text.Trim()))
            //{
            //    this.txtXZZTOWN.Focus();
            //    return "请输入现住街道（乡镇）";
            //}
            //else if (string.IsNullOrEmpty(this.txtXZZVILLIAGE.Text.Trim()))
            //{
            //    this.txtXZZVILLIAGE.Focus();
            //    return "请输入现住居委会（村）";
            //}
            //else if (string.IsNullOrEmpty(this.txtREPORT_DIAGNOSIS.Text.Trim()))
            //{
            //    this.txtREPORT_DIAGNOSIS.Focus();
            //    return "请输入诊断（部位）";
            //}
            //else if (string.IsNullOrEmpty(this.txtPATHOLOGICALTYPE.Text.Trim()))
            //{
            //    this.txtPATHOLOGICALTYPE.Focus();
            //    return "请输入病理学类型";
            //}
            //else if (string.IsNullOrEmpty(this.txtPATHOLOGICALID.Text.Trim()))
            //{
            //    this.txtPATHOLOGICALID.Focus();
            //    return "请输入病理号";
            //}
            else if (string.IsNullOrEmpty(this.lookUpEditorDept.CodeValue.Trim())
                || string.IsNullOrEmpty(this.lookUpEditorDept.Text.Trim()))
            {
                this.lookUpEditorDept.Focus();
                return "请选择报告单位";
            }
            else if (string.IsNullOrEmpty(this.lookUpEditorDoc.CodeValue.Trim())
                || string.IsNullOrEmpty(this.lookUpEditorDoc.Text.Trim()))
            {
                this.lookUpEditorDoc.Focus();
                return "请选择报告医师";
            }
            else if (string.IsNullOrEmpty(this.dateREPORTDATE.Text.Trim()))
            {
                this.dateREPORTDATE.Focus();
                return "请输入报告日期";
            }
            else if (this.ckeDiagnosis_1.Checked == false && this.ckeDiagnosis_2.Checked == false && this.ckeDiagnosis_3.Checked == false
                && this.ckeDiagnosis_4.Checked == false && this.ckeDiagnosis_5.Checked == false && this.ckeDiagnosis_6.Checked == false
                && this.ckeDiagnosis_7.Checked == false && this.ckeDiagnosis_8.Checked == false && this.ckeDiagnosis_9.Checked == false && this.ckeDiagnosis_0.Checked == false)
            {
                this.ckeDiagnosis_1.Focus();
                return "请选择诊断依据";
            }
            return string.Empty;
        }
        #endregion
        /// <summary>
        /// 新增errorinfo字段，用于存放返回的操作错误的信息 
        /// edit by ywk 2012年3月28日13:54:28
        /// </summary>
        /// edit by Yanqiao.Cai 2013-03-12
        /// 1、add try ... catch
        /// 2、插入附卡记录(如果存在附卡)
        /// <param name="errorinfo"></param>
        /// <returns></returns>
        private bool PrivateSave(out string errorinfo)
        {
            try
            {

                //新增保存
                if (m_ReportCardEntity.Report_Id == "0")
                {
                    ReportCardEntity _ReportCardEntity = GetEntityUI(m_ReportCardEntity);

                    if (_ReportCardEntity == null)
                    {
                        errorinfo = "报告实体不能为空";
                        return false;
                    }

                    _ReportCardEntity.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
                    _ReportCardEntity.CreateDeptcode = m_Host.User.CurrentDeptId;
                    _ReportCardEntity.CreateDeptname = m_Host.User.CurrentDeptName;
                    _ReportCardEntity.CreateUsercode = m_Host.User.Id;
                    _ReportCardEntity.CreateUsername = m_Host.User.Name;
                    _ReportCardEntity.State = "1";
                    _ReportCardEntity.Vaild = "1";

                    //if (m_dataTableDiagicd != null)
                    //{
                    //    m_diagicd10 = m_dataTableDiagicd.Rows[m_upCount]["diagnosis_code"].ToString();
                    //}
                    _ReportCardEntity.DIAGICD10 = lookUpEditorDialogICD10.CodeValue;

                    if (SqlHelper == null)
                    {
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    }
                    try
                    {
                        int result = 1;
                        ReportID = m_SqlHelper.AddReportCard(_ReportCardEntity);
                        if (!string.IsNullOrEmpty(ReportID))
                        {
                            m_ReportCardEntity = m_SqlHelper.GetReportCardEntity(ReportID);
                            errorinfo = result == 1 ? "新增成功" : "新增失败";
                            return result == 1;
                        }
                        else
                        {
                            errorinfo = "新增失败";
                            return false;
                        }

                    }
                    catch (Exception ex)
                    {
                        errorinfo = "新增失败 " + ex.Message;
                        return false;
                    }
                }
                else
                {
                    string m_ReportId = m_ReportCardEntity.Report_Id.ToString();//获得报告卡号
                    string m_OldStateId = m_ReportCardEntity.State.ToString();//获得原来的状态字段
                    ReportCardEntity _ReportCardEntity = GetEntityUI(m_ReportCardEntity);
                    if (_ReportCardEntity == null)
                    {
                        errorinfo = "报告实体不能为空";
                        return false;
                    }
                    if (SqlHelper == null)
                    {
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    }
                    try
                    {
                        ReportCardEntity _myReportCardEntity = new ReportCardEntity();
                        _myReportCardEntity = SqlHelper.GetReportCardEntity(m_ReportId);
                        //m_SqlHelper.EditReportCard(_ReportCardEntity);

                        //int result = 1;
                        //errorinfo = result == 1 ? "修改成功" : "修改失败";
                        //return result == 1;

                        if (m_OldStateId == _myReportCardEntity.State)//原来状态和现在状态一致
                        {
                            m_SqlHelper.EditReportCard(_ReportCardEntity);
                            int result = 1;
                            errorinfo = result == 1 ? "修改成功" : "修改失败";
                            return result == 1;
                        }
                        else//状态改变
                        {
                            errorinfo = "此报告卡状态已经改变";
                            return false;
                        }

                    }
                    catch (Exception ex)
                    {
                        errorinfo = "修改失败 " + ex.Message;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// 提交
        /// </summary>
        public bool Submit()
        {
            try
            {
                if (m_ReportCardEntity == null && m_dataTableDiagicd == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return false;
                }

                //add by cyq 2012-10-24
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return false;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要提交该肿瘤病上报记录吗？", "提交肿瘤病上报记录", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    //string ErrorMsg;//用于在判断提交状态下，单据状态已经改变导致保存失败，而返回的消息
                    //提交之前先保存数据到数据库中
                    // PrivateSave(out ErrorMsg);

                    //m_ReportCardEntity = SqlHelper.GetReportCardEntity(m_ReportCardEntity.Report_Id);
                    //m_ReportCardEntity.State = "2";
                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);

                    string m_ReportId = m_ReportCardEntity.Report_Id.ToString();//获得报告卡号

                    //提交操作判断状态是否改变 
                    ReportCardEntity _myReportCardEntity = new ReportCardEntity();
                    _myReportCardEntity = SqlHelper.GetReportCardEntity(m_ReportId);


                    if ("7" != _myReportCardEntity.State)//状态为1（已经保存的）才进行提交
                    {
                        _myReportCardEntity.State = "2";
                        m_SqlHelper.EditReportCard(_myReportCardEntity);
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("提交成功");
                        m_ReportCardEntity = _myReportCardEntity;
                        return true;
                    }
                    else
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("此报告卡已经作废，不能提交");
                        return false;
                    }
                    //if (!ErrorMsg.Contains("失败"))//保存动作已经成功
                    //   {   }
                    //   else
                    //   {
                    //       Common.Ctrs.DLG.MessageBox.Show("此报告卡状态已经改变");
                    //       return false;
                    //   }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("提交失败" + ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 提交修改
        /// </summary>
        public bool EditPass()
        {
            try
            {
                if (m_ReportCardEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条传染病上报记录或补录传染病报告信息");
                    return false;
                }

                ReportCardEntity _ReportCardEntity = GetEntityUI(m_ReportCardEntity);
                if (_ReportCardEntity == null)
                {
                    return false;
                }
                else
                {
                    if (_ReportCardEntity.State == "4")//已通过
                    {
                        string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AuditUserCode");

                        if (valueStr.ToLower().Contains("," + m_Host.User.Id + ","))
                        {
                            ReadOnlyControl("1");
                            return true;
                        }
                        else
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("对不起您没有修改权限");
                            return false;
                        }
                    }
                    else//未通过
                    {
                        if (_ReportCardEntity.CreateUsercode == m_Host.User.Id)
                        {
                            ReadOnlyControl("1");
                            return true;
                        }
                        else
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("对不起您没有修改权限");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return false;
            }


        }


        /// <summary>
        /// 设置焦点
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-13</date>
        public void SetFocusControl()
        {
            try
            {
                this.txtREPORT_DISTRICTNAME.Focus();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Color cr = colorHx16toRGB("#ACA899");
            ControlPaint.DrawBorder(e.Graphics,
                                this.panel1.ClientRectangle,
                               cr
                                ,
                                1,
                                ButtonBorderStyle.Solid,
                               cr,
                                1,
                                ButtonBorderStyle.Solid,
                               cr,
                                1,
                                ButtonBorderStyle.Solid,
                               cr,
                                1,
                                ButtonBorderStyle.Solid);

        }

        /// <summary>
        /// [颜色：16进制转成RGB]
        /// </summary>
        /// <param name="strColor">设置16进制颜色 [返回RGB]</param>
        /// <returns></returns>
        public static System.Drawing.Color colorHx16toRGB(string strHxColor)
        {
            try
            {

                return System.Drawing.Color.FromArgb(System.Int32.Parse(strHxColor.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier));

            }
            catch
            {//设为黑色
                return System.Drawing.Color.Black;
            }
        }

        private void chksick_Click(object sender, EventArgs e)
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
