using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
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

namespace DrectSoft.Core.ReportManager
{
    public partial class ReportCardDialog : DevBaseForm
    {
        IEmrHost m_Host;
        public string ReportID;
        public string m_Noofinpat;
        public string m_diagicd10;
        private string m_iem_Mainpage_NO;
        public int m_upCount = 0;
        DataTable m_dataTableDiagicd;
        SqlHelper m_SqlHelper;
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
        public ReportCardEntity m_ReportCardEntity;
        public ReportCardDialog()
        {
            InitializeComponent();
        }
        public ReportCardDialog(IEmrHost app, string iem_no, DataTable tableDiagicd, string noofinpat)
        {

            try
            {
                InitializeComponent();
                m_Host = app;
                m_iem_Mainpage_NO = iem_no;
                m_dataTableDiagicd = tableDiagicd;
                if (m_dataTableDiagicd != null && m_dataTableDiagicd.Rows.Count > 0)
                {
                    m_diagicd10 = m_dataTableDiagicd.Rows[0]["icd"].ToString();
                }
                InitLookUpEditor();
                m_Noofinpat = noofinpat;
                InitDiagICD10(m_Noofinpat);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
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
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
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
                this.txtREPORT_ICD0.Text = "";
                this.txtREPORT_CLINICID.Text = "";
                this.txtREPORT_PATID.Text = "";
                this.txtREPORT_INDO.Text = "";
                this.txtREPORT_INPATNAME.Text = "";
                this.chkSex1.Checked = false;
                this.chkSex2.Checked = false;

                this.dateBIRTHDATE.Text = "";
                this.txtAge.Value = 0;
                this.lueNation.Text = "";
                this.txtCONTACTTEL.Text = "";
                this.lueMarital.Text = "";
                this.lueJob.Text = "";
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

                this.lookUpEditorDept.Text = "";
                this.lookUpEditorDoc.Text = "";
                this.dateREPORTDATE.Text = "";
                this.deteDEATHDATE.Text = "";
                this.txtDEATHREASON.Text = "";
                this.memRejest.Text = "";
                this.lookUpEditorOutDiag.Text = "";
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


            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
            }
        }

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

                    this.lookUpEditorDialogICD10.CodeValue = m_diagicd10;
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
                        this.txtAge.Value = MethodHelper.ComputeAge(Convert.ToDateTime(_ReportCardEntity.BirthDate));
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
                    this.lookUpEditorDoc.CodeValue = _ReportCardEntity.ReportDoctor;
                    this.dateREPORTDATE.Text = _ReportCardEntity.ReportDate;
                    this.deteDEATHDATE.Text = _ReportCardEntity.DeathDate;
                    this.lookDiagDept.CodeValue = _ReportCardEntity.ReportDiagfunit;
                    this.txtclinicalstages.Text = _ReportCardEntity.ClinicalStages;//临床分期

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


                    #endregion
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
            }

        }

        /// <summary>
        /// 给实体赋值
        /// </summary>
        /// <param name="_ReportCardEntity"></param>
        /// <returns></returns>
        private ReportCardEntity GetEntityUI(ReportCardEntity _ReportCardEntity)
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
                    //int age = DateTime.Now.Year - Convert.ToDateTime(_ReportCardEntity.BirthDate).Year + 1;
                    //_ReportCardEntity.RealAge = age.ToString();
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

                _ReportCardEntity.Report_Noofinpat = m_Noofinpat;
                _ReportCardEntity.Report_No = this.txtREPORT_No.Text;


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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return null;
            }

            return _ReportCardEntity;
        }

        private void ReportCardForm_Load(object sender, EventArgs e)
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
            }
        }
        public void CheckedSick()
        {
            chksick.Checked = true;
        }
        /// <summary>
        /// 控制中心医院和九江中医院的控件显示
        /// </summary>
        /// <param name="cardtype"></param>
        private void ControlSetVisable(string cardtype)
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
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
            }
        }

        private void InitOutDiag()
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

        /// <summary>
        /// 初始化ICD编码 Add By wwj 2013-07-23
        /// </summary>
        public void InitDiagICD10(string noofinpat)
        {
            try
            {
                if (lookUpEditorDialogICD10.SqlWordbook == null)
                {
                    LookUpWindow lupInfo = new LookUpWindow();
                    lupInfo.SqlHelper = m_Host.SqlHelper;

                    //                //根据当前病人获取诊断编码和名称
                    //                string valueStr = Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage").ToUpper();
                    //                string sql = string.Empty;

                    //                sql = @"select icd, name, tumorid, py, wb
                    //                                from zymosis_diagnosis z 
                    //                            where z.icd in (
                    //                                select imd.diagnosis_code from {0} imd, {1} imb
                    //                                    where imd.iem_mainpage_no = imb.iem_mainpage_no and imb.noofinpat = {2} and imb.valide = '1' and imd.valide = '1'
                    //                                ) 
                    //                                and (select count(1) from theriomareportcard zr 
                    //                                    where zr.diagicd10 = z.icd
                    //                                        and zr.report_noofinpat = {2}
                    //                                        and zr.diagicd10 = z.icd 
                    //                                        and zr.vaild=1
                    //                                ) < z.upcount
                    //                                and z.valid = '1'";

                    //                if (valueStr.IndexOf("_SX") > -1)
                    //                {
                    //                    sql = string.Format(sql, "iem_mainpage_diagnosis_sx", "iem_mainpage_basicinfo_sx", noofinpat);
                    //                }
                    //                else
                    //                {
                    //                    sql = string.Format(sql, "iem_mainpage_diagnosis_2012", "iem_mainpage_basicinfo_2012", noofinpat);
                    //                }

                    string sql = @"select icd, name, tumorid, py, wb from zymosis_diagnosis z where z.categoryid = '2' and z.valid = '1'";

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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
            }

        }

        #region 初始化数据

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

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
            }


        }
        #endregion


        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Submit())
                {
                    m_upCount++;
                    if (m_upCount < m_dataTableDiagicd.Rows.Count)
                    {
                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人出院诊断还存在符合肿瘤病上报条件，是否立即填报？", "肿瘤病上报", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            //this.Close();

                        }
                        else
                        {
                            m_diagicd10 = m_dataTableDiagicd.Rows[m_upCount]["icd"].ToString();
                            m_ReportCardEntity = null;
                            LoadPage(m_Noofinpat, "2", "1");
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
            }

        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Save())
                {
                    m_upCount++;
                    if (m_upCount < m_dataTableDiagicd.Rows.Count)
                    {
                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人出院诊断还存在符合肿瘤病上报条件，是否立即填报？", "肿瘤病上报", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            //this.Close();

                        }
                        else
                        {
                            m_diagicd10 = m_dataTableDiagicd.Rows[m_upCount]["icd"].ToString();
                            m_ReportCardEntity = null;
                            LoadPage(m_Noofinpat, "2", "1");
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
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
                if (m_ReportCardEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条病人信息或补录肿瘤病报告信息");
                    return false;
                }

                //add by cyq 2012-10-24
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return false;
                }

                string ErrorMsg;//用于在判断保存状态下，单据状态已经改变导致保存失败，而返回的消息
                //保存数据
                bool boo = PrivateSave(out ErrorMsg);
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ErrorMsg);
                return boo;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return false;
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
                this.dateBIRTHDATE.Focus();
                return "请选择名族";
            }
            //else if (string.IsNullOrEmpty(this.txtCONTACTTEL.Text))
            //{
            //    this.dateBIRTHDATE.Focus();
            //    return "请输入家庭电话";
            //}
            else if (string.IsNullOrEmpty(this.lueMarital.CodeValue.Trim())
                || string.IsNullOrEmpty(this.lueMarital.Text.Trim()))
            {
                this.lueMarital.Focus();
                return "请选择婚姻";
            }
            //else if (string.IsNullOrEmpty(this.lookUpEditorOutDiag.CodeValue))
            //{
            //    this.lookUpEditorOutDiag.Focus();
            //    return "请输入原诊断";
            //}
            //else if (string.IsNullOrEmpty(this.dateREPORT_YDIAGNOSISDATA.Text))
            //{
            //    this.dateREPORT_YDIAGNOSISDATA.Focus();
            //    return "请输入原诊断日期";
            //}
            //else if (string.IsNullOrEmpty(this.lueJob.CodeValue))
            //{
            //    this.lueJob.Focus();
            //    return "请输入职业";
            //}
            //else if (string.IsNullOrEmpty(this.txtOFFICEADDRESS.Text))
            //{
            //    this.txtOFFICEADDRESS.Focus();
            //    return "请输入工作单位";
            //}
            //else if (string.IsNullOrEmpty(this.txtORGDISTRICTNAME.Text))
            //{
            //    this.txtORGDISTRICTNAME.Focus();
            //    return "请输入户口地址";
            //}
            //else if (string.IsNullOrEmpty(this.txtORGTOWN.Text))
            //{
            //    this.txtORGTOWN.Focus();
            //    return "请输入户口区（县）";
            //}
            //else if (string.IsNullOrEmpty(this.txtORGVILLAGENAME.Text))
            //{
            //    this.txtORGVILLAGENAME.Focus();
            //    return "请输入户口街道（乡镇）";
            //}
            //else if (string.IsNullOrEmpty(this.txtORGVILLAGENAME.Text))
            //{
            //    this.txtORGVILLAGENAME.Focus();
            //    return "请输入户口居委会（村）";
            //}
            //else if (string.IsNullOrEmpty(this.txtXZZDISTRICT.Text))
            //{
            //    this.txtXZZDISTRICT.Focus();
            //    return "请输入现住区（县）";
            //}
            //else if (string.IsNullOrEmpty(this.txtXZZTOWN.Text))
            //{
            //    this.txtXZZTOWN.Focus();
            //    return "请输入现住街道（乡镇）";
            //}
            //else if (string.IsNullOrEmpty(this.txtXZZVILLIAGE.Text))
            //{
            //    this.txtXZZVILLIAGE.Focus();
            //    return "请输入现住居委会（村）";
            //}
            //else if (string.IsNullOrEmpty(this.txtREPORT_DIAGNOSIS.Text))
            //{
            //    this.txtREPORT_DIAGNOSIS.Focus();
            //    return "请输入诊断（部位）";
            //}
            //else if (string.IsNullOrEmpty(this.txtPATHOLOGICALTYPE.Text))
            //{
            //    this.txtPATHOLOGICALTYPE.Focus();
            //    return "请输入病理学类型";
            //}
            //else if (string.IsNullOrEmpty(this.txtPATHOLOGICALID.Text))
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

                    _ReportCardEntity.DIAGICD10 = m_diagicd10;

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
                    catch
                    {

                        errorinfo = "新增失败";
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
                        errorinfo = "报告卡实体不能为空";
                        return false;
                    }

                    _ReportCardEntity.State = "1";

                    if (SqlHelper == null)
                    {
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    }
                    try
                    {
                        ReportCardEntity _myReportCardEntity = new ReportCardEntity();
                        _myReportCardEntity = SqlHelper.GetReportCardEntity(m_ReportId);

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
                    catch
                    {
                        errorinfo = "修改失败";
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
                if (m_ReportCardEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条肿瘤病上报记录或补录肿瘤病报告信息");
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
                    string ErrorMsg;//用于在判断提交状态下，单据状态已经改变导致保存失败，而返回的消息
                    //提交之前先保存数据到数据库中
                    PrivateSave(out ErrorMsg);

                    m_ReportCardEntity = SqlHelper.GetReportCardEntity(ReportID);
                    m_ReportCardEntity.State = "2";
                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);

                    string m_ReportId = m_ReportCardEntity.Report_Id.ToString();//获得报告卡号

                    //提交操作判断状态是否改变 
                    ReportCardEntity _myReportCardEntity = new ReportCardEntity();
                    _myReportCardEntity = SqlHelper.GetReportCardEntity(m_ReportId);

                    if (!ErrorMsg.Contains("失败"))//保存动作已经成功
                    {
                        if ("1" == _myReportCardEntity.State)//状态为1（已经保存的）才进行提交
                        {
                            m_SqlHelper.EditReportCard(m_ReportCardEntity);
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("提交成功");
                            m_ReportCardEntity = null;
                            return true;
                        }
                        else
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("此报告卡状态已经改变");
                            return false;
                        }
                    }
                    else
                    {

                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("此报告卡状态已经改变");
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("提交失败" + ex.Message);
                return false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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