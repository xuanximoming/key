using DevExpress.XtraEditors;
using DrectSoft.Common.Library;
using DrectSoft.Core.DSReportManager;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace DrectSoft.Core.ReportManager.UCControl
{
    public partial class BirthDefectsControl : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 出生儿缺陷卡   add by jxh  2013-8-16
        /// </summary>
        IEmrHost m_Host;
        public ReportCardEntity1 m_ReportCardEntity;
        private SqlHelper m_SqlHelper;
        public string ID;//当前报告卡序号
        public string m_Noofinpat;//患者序号
        public string DiagICD10;  //诊断编号
        private string m_checkdata = string.Empty; //数据检验标识
        public DataTable m_dataTableDiagicd;//诊断数据源

        public BirthDefectsControl()
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

        public BirthDefectsControl(IEmrHost app)
        {
            try
            {
                m_Host = app;
                InitializeComponent();
                InitLookUpEditor();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 窗体加载事件
        /// add by jxh 2013年8月26日
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BirthDefects_Load(object sender, EventArgs e)
        {
            DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
            InitLookUpEditor();
        }

        private void InitLookUpEditor()
        {
            InitNation();
            InitDoctor();
            FillEdit();
        }
        /// <summary>
        /// 填表人初始值
        /// </summary>
        private void FillEdit()
        {
            try
            {
                this.lookUpEditorDoc.Text = m_Host.User.Name;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
            }

        }

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

        private void InitDoctor()
        {
            try
            {
                string sql_diag = "select distinct u.ID,u.NAME,u.PY,u.WB,u.grade from Users u where valid=1 ";
                DataTable diag = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql_diag);
                diag.Columns["ID"].Caption = "医生工号";
                diag.Columns["NAME"].Caption = "医生名称";
                Dictionary<string, int> colDiag = new Dictionary<string, int>();
                colDiag.Add("ID", 80);
                colDiag.Add("NAME", 160);

                LookUpWindow lookUpWindowInDiag = new LookUpWindow();
                this.lookUpEditorDoc.ListWindow = lookUpWindowInDiag;
                this.lookUpEditorAudit.ListWindow = lookUpWindowInDiag;
                SqlWordbook outWordBook = new SqlWordbook("querybook", diag, "ID", "NAME", colDiag, "ID//NAME//PY//WB");
                this.lookUpEditorDoc.SqlWordbook = outWordBook;
                this.lookUpEditorAudit.SqlWordbook = outWordBook;

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

        private void FillUI(ReportCardEntity1 _ReportCardEntity)
        {
            try
            {
                if (_ReportCardEntity == null)
                {
                    return;
                }
                else
                {
                    m_Noofinpat = _ReportCardEntity.ReportNoofinpat;
                    //报告卡序号
                    //ID = _ReportCardEntity.ReportId;
                    //报告卡省
                    txtREPORT_PROVINCE.Text = _ReportCardEntity.ReportProvince;
                    //报告卡市
                    txtREPORT_CITY.Text = _ReportCardEntity.ReportCity;
                    //报告卡城镇
                    txtREPORT_TOWN.Text = _ReportCardEntity.ReportTown;
                    //报告卡村
                    txtREPORT_VILLAGE.Text = _ReportCardEntity.ReportVillage;
                    //报告卡医院
                    txtREPORT_HOSPITAL.Text = _ReportCardEntity.ReportHospital;
                    //报告卡序号
                    // ReportNo
                    //产妇住院号
                    txtMOTHER_PATID.Text = _ReportCardEntity.MotherPatid;
                    //产妇姓名
                    txtMOTHER_NAME.Text = _ReportCardEntity.MotherName;
                    //产妇年龄
                    txtMOTHER_AGE.Text = _ReportCardEntity.MotherAge;
                    //民族
                    //txtNATIONAL.Text = _ReportCardEntity.National;
                    if (!string.IsNullOrEmpty(_ReportCardEntity.National))
                    {
                        this.lueNation.CodeValue = _ReportCardEntity.National;
                    }
                    //地址and邮编
                    txtADDRESS_POST.Text = _ReportCardEntity.AddressPost;
                    //孕次
                    txtPREGNANTNO.Text = _ReportCardEntity.Pregnantno;
                    //产次
                    txtPRODUCTIONNO.Text = _ReportCardEntity.Productionno;
                    //常住地
                    if (_ReportCardEntity.Localadd == "1")
                    {
                        checkLOCALADD1.Checked = true;
                    }
                    else if (_ReportCardEntity.Localadd == "2")
                    {
                        checkLOCALADD2.Checked = true;
                    }
                    //年人均收入
                    if (_ReportCardEntity.Percapitaincome == "1")
                    {
                        checkPERCAPITAINCOME1.Checked = true;
                    }
                    else if (_ReportCardEntity.Percapitaincome == "2")
                    {
                        checkPERCAPITAINCOME2.Checked = true;
                    }
                    else if (_ReportCardEntity.Percapitaincome == "3")
                    {
                        checkPERCAPITAINCOME3.Checked = true;
                    }
                    else if (_ReportCardEntity.Percapitaincome == "4")
                    {
                        checkPERCAPITAINCOME4.Checked = true;
                    }
                    else if (_ReportCardEntity.Percapitaincome == "5")
                    {
                        checkPERCAPITAINCOME5.Checked = true;
                    }
                    //文化程度
                    if (_ReportCardEntity.Educationlevel == "1")
                    {
                        checkEDUCATIONLEVEL1.Checked = true;
                    }
                    else if (_ReportCardEntity.Educationlevel == "2")
                    {
                        checkEDUCATIONLEVEL2.Checked = true;
                    }
                    else if (_ReportCardEntity.Educationlevel == "3")
                    {
                        checkEDUCATIONLEVEL3.Checked = true;
                    }
                    else if (_ReportCardEntity.Educationlevel == "4")
                    {
                        checkEDUCATIONLEVEL4.Checked = true;
                    }
                    else if (_ReportCardEntity.Educationlevel == "5")
                    {
                        checkEDUCATIONLEVEL5.Checked = true;
                    }
                    //孩子住院号
                    txtCHILD_PATID.Text = _ReportCardEntity.ChildPatid;
                    //孩子姓名
                    txtCHILD_NAME.Text = _ReportCardEntity.ChildName;
                    //是否本院出生
                    if (_ReportCardEntity.Isbornhere == "1")
                    {
                        checkISBORNHERE1.Checked = true;
                    }
                    else if (_ReportCardEntity.Isbornhere == "2")
                    {
                        checkISBORNHERE2.Checked = true;
                    }
                    //孩子性别
                    if (_ReportCardEntity.ChildSex == "1")
                    {
                        checkCHILD_SEX1.Checked = true;
                    }
                    else if (_ReportCardEntity.ChildSex == "2")
                    {
                        checkCHILD_SEX2.Checked = true;
                    }
                    else if (_ReportCardEntity.ChildSex == "3")
                    {
                        checkCHILD_SEX3.Checked = true;
                    }
                    //出生年
                    txtBORN_YEAR.Text = _ReportCardEntity.BornYear;
                    //出生月
                    txtBORN_MONTH.Text = _ReportCardEntity.BornMonth;
                    //出生日
                    txtBORN_DAY.Text = _ReportCardEntity.BornDay;
                    //胎龄
                    txtGESTATIONALAGE.Text = _ReportCardEntity.Gestationalage;
                    //体重
                    txtWEIGHT.Text = _ReportCardEntity.Weight;
                    //胎数
                    if (_ReportCardEntity.Births == "1")
                    {
                        checkBIRTHS1.Checked = true;
                    }
                    else if (_ReportCardEntity.Births == "2")
                    {
                        checkBIRTHS2.Checked = true;
                    }
                    else if (_ReportCardEntity.Births == "3")
                    {
                        checkBIRTHS3.Checked = true;
                    }
                    //是否同卵
                    if (_ReportCardEntity.Isidentical == "1")
                    {
                        checkISIDENTICAL1.Checked = true;
                    }
                    else if (_ReportCardEntity.Isidentical == "2")
                    {
                        checkISIDENTICAL2.Checked = true;
                    }
                    //转归
                    if (_ReportCardEntity.Outcome == "1")
                    {
                        checkOUTCOME1.Checked = true;
                    }
                    else if (_ReportCardEntity.Outcome == "2")
                    {
                        checkOUTCOME2.Checked = true;
                    }
                    else if (_ReportCardEntity.Outcome == "3")
                    {
                        checkOUTCOME3.Checked = true;
                    }
                    else if (_ReportCardEntity.Outcome == "4")
                    {
                        checkOUTCOME4.Checked = true;
                    }
                    else if (_ReportCardEntity.Outcome == "5")
                    {
                        checkOUTCOME5.Checked = true;
                    }
                    //是否引产
                    if (_ReportCardEntity.Inducedlabor == "1")
                    {
                        checkINDUCEDLABOR1.Checked = true;
                    }
                    else if (_ReportCardEntity.Inducedlabor == "2")
                    {
                        checkINDUCEDLABOR2.Checked = true;
                    }
                    //诊断依据
                    if (_ReportCardEntity.Diagnosticbasis != "")
                    {
                        checkDIAGNOSTICBASIS.Checked = true;
                    }
                    if (_ReportCardEntity.Diagnosticbasis1 != "")
                    {
                        checkDIAGNOSTICBASIS1.Checked = true;
                    }
                    if (_ReportCardEntity.Diagnosticbasis2 != "")
                    {
                        checkDIAGNOSTICBASIS2.Checked = true;
                    }
                    if (_ReportCardEntity.Diagnosticbasis3 != "")
                    {
                        checkDIAGNOSTICBASIS3.Checked = true;
                    }
                    //生化检验其他
                    txtDIAGNOSTICBASIS4.Text = _ReportCardEntity.Diagnosticbasis4;
                    if (_ReportCardEntity.Diagnosticbasis5 != "")
                    {
                        checkDIAGNOSTICBASIS5.Checked = true;
                    }
                    if (_ReportCardEntity.Diagnosticbasis6 != "")
                    {
                        checkDIAGNOSTICBASIS6.Checked = true;
                    }
                    //诊断依据其他内容
                    txtDIAGNOSTICBASIS7.Text = _ReportCardEntity.Diagnosticbasis7;
                    //出生缺陷诊断
                    if (_ReportCardEntity.DiagAnencephaly != "")
                    {
                        checkDIAG_ANENCEPHALY.Checked = true;
                    }
                    if (_ReportCardEntity.DiagSpina != "")
                    {
                        checkDIAG_SPINA.Checked = true;
                    }
                    if (_ReportCardEntity.DiagPengout != "")
                    {
                        checkDIAG_PENGOUT.Checked = true;
                    }
                    if (_ReportCardEntity.DiagHydrocephalus != "")
                    {
                        checkDIAG_HYDROCEPHALUS.Checked = true;
                    }
                    if (_ReportCardEntity.DiagCleftpalate != "")
                    {
                        checkDIAG_CLEFTPALATE.Checked = true;
                    }
                    if (_ReportCardEntity.DiagCleftlip != "")
                    {
                        checkDIAG_CLEFTLIP.Checked = true;
                    }
                    if (_ReportCardEntity.DiagCleftmerger != "")
                    {
                        checkDIAG_CLEFTMERGER.Checked = true;
                    }
                    if (_ReportCardEntity.DiagSmallears != "")
                    {
                        checkDIAG_SMALLEARS.Checked = true;
                    }
                    if (_ReportCardEntity.DiagOuterear != "")
                    {
                        checkDIAG_OUTEREAR.Checked = true;
                    }
                    if (_ReportCardEntity.DiagEsophageal != "")
                    {
                        checkDIAG_ESOPHAGEAL.Checked = true;
                    }
                    if (_ReportCardEntity.DiagRectum != "")
                    {
                        checkDIAG_RECTUM.Checked = true;
                    }
                    if (_ReportCardEntity.DiagHypospadias != "")
                    {
                        checkDIAG_HYPOSPADIAS.Checked = true;
                    }
                    if (_ReportCardEntity.DiagBladder != "")
                    {
                        checkDIAG_BLADDER.Checked = true;
                    }
                    if (_ReportCardEntity.DiagHorseshoefootleft != "")
                    {
                        checkDIAG_HORSESHOEFOOTLEFT.Checked = true;
                    }
                    if (_ReportCardEntity.DiagHorseshoefootright != "")
                    {
                        checkDIAG_HORSESHOEFOOTRIGHT.Checked = true;
                    }
                    if (_ReportCardEntity.DiagManypointleft != "")
                    {
                        checkDIAG_MANYPOINTLEFT.Checked = true;
                    }
                    if (_ReportCardEntity.DiagManypointright != "")
                    {
                        checkDIAG_MANYPOINTRIGHT.Checked = true;
                    }
                    if (_ReportCardEntity.DiagLimbslowerleft != "")
                    {
                        checkDIAG_LIMBSLOWERLEFT.Checked = true;
                    }
                    if (_ReportCardEntity.DiagLimbslowerright != "")
                    {
                        checkDIAG_LIMBSLOWERRIGHT.Checked = true;
                    }
                    if (_ReportCardEntity.DiagLimbsupperleft != "")
                    {
                        checkDIAG_LIMBSUPPERLEFT.Checked = true;
                    }
                    if (_ReportCardEntity.DiagLimbsupperright != "")
                    {
                        checkDIAG_LIMBSUPPERRIGHT.Checked = true;
                    }
                    if (_ReportCardEntity.DiagHernia != "")
                    {
                        checkDIAG_HERNIA.Checked = true;
                    }
                    if (_ReportCardEntity.DiagBulgingbelly != "")
                    {
                        checkDIAG_BULGINGBELLY.Checked = true;
                    }
                    if (_ReportCardEntity.DiagGastroschisis != "")
                    {
                        checkDIAG_GASTROSCHISIS.Checked = true;
                    }
                    if (_ReportCardEntity.DiagTwins != "")
                    {
                        checkDIAG_TWINS.Checked = true;
                    }
                    if (_ReportCardEntity.DiagTssyndrome != "")
                    {
                        checkDIAG_TSSYNDROME.Checked = true;
                    }
                    if (_ReportCardEntity.DiagHeartdisease != "")
                    {
                        checkDIAG_HEARTDISEASE.Checked = true;
                    }
                    if (_ReportCardEntity.DiagOther != "")
                    {
                        checkDIAG_OTHER.Checked = true;
                    }
                    //出生缺陷诊断其他内容
                    txtDIAG_OTHERCONTENT.Text = _ReportCardEntity.DiagOthercontent;
                    //发烧
                    if (_ReportCardEntity.Fever != "")
                    {
                        checkFEVER.Checked = true;
                    }
                    //病毒感染
                    if (_ReportCardEntity.Isvirusinfection != "")
                    {
                        checkISVIRUSINFECTION.Checked = true;
                    }
                    this.txtVIRUSINFECTION.Text = _ReportCardEntity.Virusinfection;
                    //糖尿病
                    if (_ReportCardEntity.Isdiabetes != "")
                    {
                        checkISDIABETES.Checked = true;
                    }
                    //患病其他
                    if (_ReportCardEntity.Isillother != "")
                    {
                        checkISILLOTHER.Checked = true;
                    }
                    txtILLOTHER.Text = _ReportCardEntity.Illother;
                    //磺胺类
                    if (_ReportCardEntity.Issulfa != "")
                    {
                        checkISSULFA.Checked = true;
                    }
                    txtSULFA.Text = _ReportCardEntity.Sulfa;
                    //抗生素
                    if (_ReportCardEntity.Isantibiotics != "")
                    {
                        checkISANTIBIOTICS.Checked = true;
                    }
                    this.txtANTIBIOTICS.Text = _ReportCardEntity.Antibiotics;
                    //避孕药
                    if (_ReportCardEntity.Isbirthcontrolpill != "")
                    {
                        checkISBIRTHCONTROLPILL.Checked = true;
                    }
                    this.txtBIRTHCONTROLPILL.Text = _ReportCardEntity.Birthcontrolpill;
                    //镇静药
                    if (_ReportCardEntity.Issedatives != "")
                    {
                        checkISSEDATIVES.Checked = true;
                    }
                    this.txtSEDATIVES.Text = _ReportCardEntity.Sedatives;
                    //服药其他
                    if (_ReportCardEntity.Ismedicineother != "")
                    {
                        checkISMEDICINEOTHER.Checked = true;
                    }
                    this.txtMEDICINEOTHER.Text = _ReportCardEntity.Medicineother;
                    //饮酒
                    if (_ReportCardEntity.Isdrinking != "")
                    {
                        checkISDRINKING.Checked = true;
                    }
                    this.txtDRINKING.Text = _ReportCardEntity.Drinking;
                    //农药
                    if (_ReportCardEntity.Ispesticide != "")
                    {
                        checkISPESTICIDE.Checked = true;
                    }
                    this.txtPESTICIDE.Text = _ReportCardEntity.Pesticide;
                    //射线
                    if (_ReportCardEntity.Isray != "")
                    {
                        checkISRAY.Checked = true;
                    }
                    this.txtRAY.Text = _ReportCardEntity.Ray;
                    //化学药剂
                    if (_ReportCardEntity.Ischemicalagents != "")
                    {
                        checkISCHEMICALAGENTS.Checked = true;
                    }
                    this.txtCHEMICALAGENTS.Text = _ReportCardEntity.Chemicalagents;
                    //其他有害因素
                    if (_ReportCardEntity.Isfactorother != "")
                    {
                        checkISFACTOROTHER.Checked = true;
                    }
                    this.txtFACTOROTHER.Text = _ReportCardEntity.Factorother;
                    //死胎例数
                    this.txtSTILLBIRTHNO.Text = _ReportCardEntity.Stillbirthno;
                    //自然流产例数
                    this.txtABORTIONNO.Text = _ReportCardEntity.Abortionno;
                    //缺陷儿例数
                    this.txtDEFECTSNO.Text = _ReportCardEntity.Defectsno;
                    //缺陷名
                    this.txtDEFECTSOF1.Text = _ReportCardEntity.Defectsof1;
                    this.txtDEFECTSOF2.Text = _ReportCardEntity.Defectsof2;
                    this.txtDEFECTSOF3.Text = _ReportCardEntity.Defectsof3;
                    //遗传缺陷名
                    this.txtYCDEFECTSOF1.Text = _ReportCardEntity.Ycdefectsof1;
                    this.txtYCDEFECTSOF2.Text = _ReportCardEntity.Ycdefectsof2;
                    this.txtYCDEFECTSOF3.Text = _ReportCardEntity.Ycdefectsof3;
                    //亲缘关系
                    this.txtKINSHIPDEFECTS1.Text = _ReportCardEntity.Kinshipdefects1;
                    this.txtKINSHIPDEFECTS2.Text = _ReportCardEntity.Kinshipdefects2;
                    this.txtKINSHIPDEFECTS3.Text = _ReportCardEntity.Kinshipdefects3;
                    //近亲
                    if (_ReportCardEntity.Cousinmarriage == "1")
                    {
                        checkCOUSINMARRIAGE1.Checked = true;
                    }
                    if (_ReportCardEntity.Cousinmarriage == "2")
                    {
                        checkCOUSINMARRIAGE2.Checked = true;
                    }
                    this.txtCOUSINMARRIAGEBETWEEN.Text = _ReportCardEntity.Cousinmarriagebetween;
                    //填表人
                    //this.txtPREPARER.Text = _ReportCardEntity.Preparer;
                    //this.lookUpEditorDoc.CodeValue = _ReportCardEntity.Preparer;
                    this.lookUpEditorDoc.Text = _ReportCardEntity.Preparer == "" ? m_Host.User.Name : _ReportCardEntity.Preparer;
                    //填表人职称
                    this.txtTHETITLE1.Text = _ReportCardEntity.Thetitle1;
                    //填表日期年
                    this.txtFILLDATEYEAR.Text = _ReportCardEntity.Filldateyear;
                    //填表日期月
                    this.txtFILLDATEMONTH.Text = _ReportCardEntity.Filldatemonth;
                    //填表日期日
                    this.txtFILLDATEDAY.Text = _ReportCardEntity.Filldateday;
                    //医院审表人
                    //this.txtHOSPITALREVIEW.Text = _ReportCardEntity.Hospitalreview;
                    this.lookUpEditorAudit.Text = _ReportCardEntity.Hospitalreview == "" ? "" : _ReportCardEntity.Hospitalreview;
                    //医院审表人职称
                    this.txtTHETITLE2.Text = _ReportCardEntity.Thetitle2;
                    //医院审表日期年
                    this.txtHOSPITALAUDITDATEYEAR.Text = _ReportCardEntity.Hospitalauditdateyear;
                    //医院审表日期月
                    this.txtHOSPITALAUDITDATEMONTH.Text = _ReportCardEntity.Hospitalauditdatemonth;
                    //医院审表日期日
                    this.txtHOSPITALAUDITDATEDAY.Text = _ReportCardEntity.Hospitalauditdateday;
                    //省级审表人
                    this.txtPROVINCIALVIEW.Text = _ReportCardEntity.Provincialview;
                    //省级审表人职称
                    this.txtTHETITLE3.Text = _ReportCardEntity.Thetitle3;
                    //省级审表日期年
                    this.txtPROVINCIALVIEWDATEYEAR.Text = _ReportCardEntity.Provincialviewdateyear;
                    //省级审表日期月
                    this.txtPROVINCIALVIEWDATEMONTH.Text = _ReportCardEntity.Provincialviewdatemonth;
                    //省级审表日期日                   
                    this.txtPROVINCIALVIEWDATEDAY.Text = _ReportCardEntity.Provincialviewdateday;
                    //产前
                    if (_ReportCardEntity.Prenatal == "1")
                    {
                        checkPRENATAL.Checked = true;
                    }
                    //产前周数
                    this.txtPRENATALNO.Text = _ReportCardEntity.Prenatalno;
                    //产后
                    if (_ReportCardEntity.Postpartum == "1")
                    {
                        checkPOSTPARTUM.Checked = true;
                    }
                    //并指左
                    if (_ReportCardEntity.Andtoshowleft == "1")
                    {
                        checkANDTOSHOWLEFT.Checked = true;
                    }
                    //并指右
                    if (_ReportCardEntity.Andtoshowright == "1")
                    {
                        checkANDTOSHOWRIGHT.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
            }
        }

        /// <summary>
        /// 数据检验
        /// </summary>
        public bool checkdata()
        {
            m_checkdata = "0";
            if (string.IsNullOrEmpty(this.txtREPORT_HOSPITAL.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入医院（保健院、所）名称");
                txtREPORT_HOSPITAL.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtMOTHER_PATID.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入产母住院号");
                txtMOTHER_PATID.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtMOTHER_NAME.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入产母姓名");
                txtMOTHER_NAME.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(lueNation.CodeValue))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择民族");
                lueNation.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtMOTHER_AGE.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入产母实足年龄");
                txtMOTHER_AGE.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtADDRESS_POST.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入通讯地址及邮编");
                txtADDRESS_POST.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtPREGNANTNO.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入孕次");
                txtPREGNANTNO.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtPRODUCTIONNO.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入产次");
                txtPRODUCTIONNO.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtCHILD_PATID.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入缺陷儿住院号");
                txtCHILD_PATID.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtCHILD_NAME.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入缺陷儿姓名");
                txtCHILD_NAME.Focus();
                return false;
            }
            if (!checkISBORNHERE1.Checked && !checkISBORNHERE2.Checked)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择是否本院产科生产");
                checkISBORNHERE1.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtBORN_YEAR.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入出生年");
                txtBORN_YEAR.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtBORN_MONTH.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入出生月");
                txtBORN_MONTH.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtBORN_DAY.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入出生日");
                txtBORN_DAY.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtGESTATIONALAGE.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入胎龄");
                txtGESTATIONALAGE.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtWEIGHT.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入体重");
                txtWEIGHT.Focus();
                return false;
            }
            if (!checkBIRTHS1.Checked && !checkBIRTHS2.Checked && !checkBIRTHS3.Checked)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择胎数");
                checkBIRTHS1.Focus();
                return false;
            }
            if (!checkCHILD_SEX1.Checked && !checkCHILD_SEX2.Checked && !checkCHILD_SEX3.Checked)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择缺陷儿性别");
                checkCHILD_SEX1.Focus();
                return false;
            }
            if (!checkOUTCOME1.Checked && !checkOUTCOME2.Checked && !checkOUTCOME3.Checked && !checkOUTCOME4.Checked && !checkOUTCOME5.Checked)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择转归情况");
                checkOUTCOME1.Focus();
                return false;
            }
            if (!checkINDUCEDLABOR1.Checked && !checkINDUCEDLABOR2.Checked)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择诊断为出生缺陷后是否治疗性引产");
                checkINDUCEDLABOR1.Focus();
                return false;
            }
            if (!checkDIAGNOSTICBASIS.Checked && !checkDIAGNOSTICBASIS1.Checked && !checkDIAGNOSTICBASIS2.Checked && !checkDIAGNOSTICBASIS3.Checked && !checkDIAGNOSTICBASIS5.Checked && !checkDIAGNOSTICBASIS6.Checked)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择诊断依据");
                checkDIAGNOSTICBASIS.Focus();
                checkDIAGNOSTICBASIS.Checked = true;
                return false;
            }

            foreach (Control control in this.Controls)
            {
                if (control is XtraScrollableControl)
                {
                    foreach (Control control1 in control.Controls)
                    {
                        if (control1 is Panel)
                        {
                            foreach (Control ctl in control1.Controls)
                            {
                                if (ctl is Panel)
                                {
                                    foreach (Control ctl1 in ctl.Controls)
                                    {
                                        if (ctl1.GetType() == typeof(CheckEdit))
                                        {
                                            if (((CheckEdit)ctl1).Checked == true)
                                            {
                                                m_checkdata = "1";
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (m_checkdata != "1")
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择出生缺陷诊断");
                checkDIAG_ANENCEPHALY.Focus();
                checkDIAG_ANENCEPHALY.Checked = true;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 报告卡提交方法
        /// add by jxh 2013年8月27日 
        /// </summary>
        /// <returns></returns>
        public bool Submit()
        {
            try
            {
                bool issuccess = false;
                if (m_ReportCardEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条出生缺陷上报记录或补录心出生缺陷报告信息");
                    issuccess = false;
                }
                if (checkdata())
                {
                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要提交该出生缺陷报告卡记录吗？", "提交出生缺陷报告卡", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                    {
                        if (SqlHelper == null)
                        {
                            SqlHelper = new SqlHelper(m_Host.SqlHelper);
                        }
                        string m_ReportId = m_ReportCardEntity.Id.ToString();//获得报告卡号
                        //提交操作判断状态是否改变 
                        ReportCardEntity1 m_CardEntity = new ReportCardEntity1();
                        m_CardEntity = SqlHelper.GetBirthDefectsReportCardEntity(m_ReportId);
                        if ("7" != m_CardEntity.State)//状态为1（已经保存的）才进行提交
                        {
                            m_CardEntity.State = "2";
                            m_SqlHelper.EditbirthdefectsReportCard(m_CardEntity);
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("提交成功");

                            issuccess = true;
                        }
                        else
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("此报告卡已经作废，不能提交");
                            issuccess = false;
                        }
                    }
                }
                else
                {
                    issuccess = false;
                }
                return issuccess;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("提交失败" + ex.Message);
                return false;
            }

        }

        //给实体赋值
        public ReportCardEntity1 GetEntityUI(ReportCardEntity1 _ReportCardEntity)
        {
            try
            {
                _ReportCardEntity.ReportId = DateTime.Now.Year.ToString() + "-" + GetNextId();
                //_ReportCardEntity.DiagCode = DiagICD10;
                _ReportCardEntity.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
                _ReportCardEntity.CreateDeptcode = m_Host.User.CurrentDeptId;
                _ReportCardEntity.CreateDeptname = m_Host.User.CurrentDeptName;
                _ReportCardEntity.CreateUsercode = m_Host.User.Id;
                _ReportCardEntity.CreateUsername = m_Host.User.Name;
                _ReportCardEntity.ReportNoofinpat = m_Noofinpat;
                _ReportCardEntity.State = "1";
                _ReportCardEntity.Vaild = "1";
                _ReportCardEntity.ReportProvince = this.txtREPORT_PROVINCE.Text;
                _ReportCardEntity.ReportCity = this.txtREPORT_CITY.Text;
                _ReportCardEntity.ReportTown = this.txtREPORT_TOWN.Text;
                _ReportCardEntity.ReportVillage = this.txtREPORT_VILLAGE.Text;
                _ReportCardEntity.ReportHospital = this.txtREPORT_HOSPITAL.Text;
                //_ReportCardEntity.ReportNo=
                _ReportCardEntity.MotherPatid = this.txtMOTHER_PATID.Text;
                _ReportCardEntity.MotherName = this.txtMOTHER_NAME.Text;
                _ReportCardEntity.MotherAge = this.txtMOTHER_AGE.Text;
                //_ReportCardEntity.National = this.txtNATIONAL.Text;
                _ReportCardEntity.National = this.lueNation.CodeValue.ToString().Trim();
                _ReportCardEntity.NationName = this.lueNation.Text.ToString().Trim();
                _ReportCardEntity.AddressPost = this.txtADDRESS_POST.Text;
                _ReportCardEntity.Pregnantno = this.txtPREGNANTNO.Text;
                _ReportCardEntity.Productionno = this.txtPRODUCTIONNO.Text;
                if (checkLOCALADD1.Checked)
                {
                    _ReportCardEntity.Localadd = "1";
                }
                else if (checkLOCALADD2.Checked)
                {
                    _ReportCardEntity.Localadd = "2";
                }
                if (checkPERCAPITAINCOME1.Checked)
                {
                    _ReportCardEntity.Percapitaincome = "1";
                }
                else if (checkPERCAPITAINCOME2.Checked)
                {
                    _ReportCardEntity.Percapitaincome = "2";
                }
                else if (checkPERCAPITAINCOME3.Checked)
                {
                    _ReportCardEntity.Percapitaincome = "3";
                }
                else if (checkPERCAPITAINCOME4.Checked)
                {
                    _ReportCardEntity.Percapitaincome = "4";
                }
                else if (checkPERCAPITAINCOME5.Checked)
                {
                    _ReportCardEntity.Percapitaincome = "5";
                }
                if (checkEDUCATIONLEVEL1.Checked)
                {
                    _ReportCardEntity.Educationlevel = "1";
                }
                else if (checkEDUCATIONLEVEL2.Checked)
                {
                    _ReportCardEntity.Educationlevel = "2";
                }
                else if (checkEDUCATIONLEVEL3.Checked)
                {
                    _ReportCardEntity.Educationlevel = "3";
                }
                else if (checkEDUCATIONLEVEL4.Checked)
                {
                    _ReportCardEntity.Educationlevel = "4";
                }
                else if (checkEDUCATIONLEVEL5.Checked)
                {
                    _ReportCardEntity.Educationlevel = "5";
                }
                _ReportCardEntity.ChildPatid = this.txtCHILD_PATID.Text;
                _ReportCardEntity.ChildName = this.txtCHILD_NAME.Text;
                if (checkCHILD_SEX1.Checked)
                {
                    _ReportCardEntity.ChildSex = "1";
                }
                else if (checkCHILD_SEX2.Checked)
                {
                    _ReportCardEntity.ChildSex = "2";
                }
                else if (checkCHILD_SEX3.Checked)
                {
                    _ReportCardEntity.ChildSex = "3";
                }
                if (checkISBORNHERE1.Checked)
                {
                    _ReportCardEntity.Isbornhere = "1";
                }
                else if (checkISBORNHERE2.Checked)
                {
                    _ReportCardEntity.Isbornhere = "2";
                }
                _ReportCardEntity.BornYear = txtBORN_YEAR.Text;
                _ReportCardEntity.BornMonth = this.txtBORN_MONTH.Text;
                _ReportCardEntity.BornDay = this.txtBORN_DAY.Text;
                _ReportCardEntity.Gestationalage = this.txtGESTATIONALAGE.Text;
                _ReportCardEntity.Weight = this.txtWEIGHT.Text;
                if (checkBIRTHS1.Checked)
                {
                    _ReportCardEntity.Births = "1";
                    _ReportCardEntity.Isidentical = "";
                }
                else if (checkBIRTHS2.Checked)
                {
                    _ReportCardEntity.Births = "2";
                    if (checkISIDENTICAL1.Checked)
                    {
                        _ReportCardEntity.Isidentical = "1";
                    }
                    else if (checkISIDENTICAL2.Checked)
                    {
                        _ReportCardEntity.Isidentical = "2";
                    }
                }
                else if (checkBIRTHS3.Checked)
                {
                    _ReportCardEntity.Births = "3";
                    if (checkISIDENTICAL1.Checked)
                    {
                        _ReportCardEntity.Isidentical = "1";
                    }
                    else if (checkISIDENTICAL2.Checked)
                    {
                        _ReportCardEntity.Isidentical = "2";
                    }
                }
                if (checkOUTCOME1.Checked)
                {
                    _ReportCardEntity.Outcome = "1";
                }
                else if (checkOUTCOME2.Checked)
                {
                    _ReportCardEntity.Outcome = "2";
                }
                else if (checkOUTCOME3.Checked)
                {
                    _ReportCardEntity.Outcome = "3";
                }
                else if (checkOUTCOME4.Checked)
                {
                    _ReportCardEntity.Outcome = "4";
                }
                else if (checkOUTCOME5.Checked)
                {
                    _ReportCardEntity.Outcome = "5";
                }
                else if (checkOUTCOME5.Checked)
                {
                    _ReportCardEntity.Outcome = "5";
                }
                if (checkINDUCEDLABOR1.Checked)  //是否引产
                {
                    _ReportCardEntity.Inducedlabor = "1";
                }
                else if (checkINDUCEDLABOR2.Checked)
                {
                    _ReportCardEntity.Inducedlabor = "2";
                }
                //诊断依据
                if (checkDIAGNOSTICBASIS.Checked)
                {
                    _ReportCardEntity.Diagnosticbasis = "1";
                }
                else
                {
                    _ReportCardEntity.Diagnosticbasis = "";
                }
                if (checkDIAGNOSTICBASIS1.Checked)
                {
                    _ReportCardEntity.Diagnosticbasis1 = "1";
                }
                else
                {
                    _ReportCardEntity.Diagnosticbasis1 = "";
                }
                if (checkDIAGNOSTICBASIS2.Checked)
                {
                    _ReportCardEntity.Diagnosticbasis2 = "1";
                }
                else
                {
                    _ReportCardEntity.Diagnosticbasis2 = "";
                }
                if (checkDIAGNOSTICBASIS3.Checked)
                {
                    _ReportCardEntity.Diagnosticbasis3 = "1";
                }
                else
                {
                    _ReportCardEntity.Diagnosticbasis3 = "";
                }
                if (checkDIAGNOSTICBASIS5.Checked)
                {
                    _ReportCardEntity.Diagnosticbasis5 = "1";
                }
                else
                {
                    _ReportCardEntity.Diagnosticbasis5 = "";
                }
                if (checkDIAGNOSTICBASIS6.Checked)
                {
                    _ReportCardEntity.Diagnosticbasis6 = "1";
                }
                else
                {
                    _ReportCardEntity.Diagnosticbasis6 = "";
                }
                _ReportCardEntity.Diagnosticbasis4 = this.txtDIAGNOSTICBASIS4.Text;
                _ReportCardEntity.Diagnosticbasis7 = this.txtDIAGNOSTICBASIS7.Text;
                //出生缺陷诊断
                if (checkDIAG_ANENCEPHALY.Checked)
                {
                    _ReportCardEntity.DiagAnencephaly = "1";
                }
                if (checkDIAG_SPINA.Checked)
                {
                    _ReportCardEntity.DiagSpina = "1";
                }
                if (checkDIAG_PENGOUT.Checked)
                {
                    _ReportCardEntity.DiagPengout = "1";
                }
                if (checkDIAG_HYDROCEPHALUS.Checked)
                {
                    _ReportCardEntity.DiagHydrocephalus = "1";
                }
                if (checkDIAG_CLEFTPALATE.Checked)
                {
                    _ReportCardEntity.DiagCleftpalate = "1";
                }
                if (checkDIAG_CLEFTLIP.Checked)
                {
                    _ReportCardEntity.DiagCleftlip = "1";
                }
                if (checkDIAG_CLEFTMERGER.Checked)
                {
                    _ReportCardEntity.DiagCleftmerger = "1";
                }
                if (checkDIAG_SMALLEARS.Checked)
                {
                    _ReportCardEntity.DiagSmallears = "1";
                }
                if (checkDIAG_OUTEREAR.Checked)
                {
                    _ReportCardEntity.DiagOuterear = "1";
                }
                if (checkDIAG_ESOPHAGEAL.Checked)
                {
                    _ReportCardEntity.DiagEsophageal = "1";
                }
                if (checkDIAG_RECTUM.Checked)
                {
                    _ReportCardEntity.DiagRectum = "1";
                }
                if (checkDIAG_HYPOSPADIAS.Checked)
                {
                    _ReportCardEntity.DiagHypospadias = "1";
                }
                if (checkDIAG_BLADDER.Checked)
                {
                    _ReportCardEntity.DiagBladder = "1";
                }
                if (checkDIAG_HORSESHOEFOOTLEFT.Checked)
                {
                    _ReportCardEntity.DiagHorseshoefootleft = "1";
                }
                if (checkDIAG_HORSESHOEFOOTRIGHT.Checked)
                {
                    _ReportCardEntity.DiagHorseshoefootright = "1";
                }
                if (checkDIAG_MANYPOINTLEFT.Checked)
                {
                    _ReportCardEntity.DiagManypointleft = "1";
                }
                if (checkDIAG_MANYPOINTRIGHT.Checked)
                {
                    _ReportCardEntity.DiagManypointright = "1";
                }
                if (checkDIAG_LIMBSLOWERLEFT.Checked)
                {
                    _ReportCardEntity.DiagLimbslowerleft = "1";
                }
                if (checkDIAG_LIMBSLOWERRIGHT.Checked)
                {
                    _ReportCardEntity.DiagLimbslowerright = "1";
                }
                if (checkDIAG_LIMBSUPPERLEFT.Checked)
                {
                    _ReportCardEntity.DiagLimbsupperleft = "1";
                }
                if (checkDIAG_LIMBSUPPERRIGHT.Checked)
                {
                    _ReportCardEntity.DiagLimbsupperright = "1";
                }
                if (checkDIAG_HERNIA.Checked)
                {
                    _ReportCardEntity.DiagHernia = "1";
                }
                if (checkDIAG_BULGINGBELLY.Checked)
                {
                    _ReportCardEntity.DiagBulgingbelly = "1";
                }
                if (checkDIAG_GASTROSCHISIS.Checked)
                {
                    _ReportCardEntity.DiagGastroschisis = "1";
                }
                if (checkDIAG_TWINS.Checked)
                {
                    _ReportCardEntity.DiagTwins = "1";
                }
                if (checkDIAG_TSSYNDROME.Checked)
                {
                    _ReportCardEntity.DiagTssyndrome = "1";
                }
                if (checkDIAG_HEARTDISEASE.Checked)
                {
                    _ReportCardEntity.DiagHeartdisease = "1";
                }
                if (checkDIAG_OTHER.Checked)
                {
                    _ReportCardEntity.DiagOther = "1";
                }
                //出生缺陷诊断其他内容
                _ReportCardEntity.DiagOthercontent = txtDIAG_OTHERCONTENT.Text;
                //发烧
                if (checkFEVER.Checked)
                {
                    _ReportCardEntity.Fever = "1";
                }
                //_ReportCardEntity.Feverno=    发烧度数

                //病毒感染
                if (checkISVIRUSINFECTION.Checked)
                {
                    _ReportCardEntity.Isvirusinfection = "1";
                }
                _ReportCardEntity.Virusinfection = this.txtVIRUSINFECTION.Text;
                //糖尿病
                if (checkISDIABETES.Checked)
                {
                    _ReportCardEntity.Isdiabetes = "1";
                }
                //患病其他
                if (checkISILLOTHER.Checked)
                {
                    _ReportCardEntity.Isillother = "1";
                }
                _ReportCardEntity.Illother = txtILLOTHER.Text;
                //磺胺类
                if (checkISSULFA.Checked)
                {
                    _ReportCardEntity.Issulfa = "1";
                }
                _ReportCardEntity.Sulfa = txtSULFA.Text;
                //抗生素
                if (checkISANTIBIOTICS.Checked)
                {
                    _ReportCardEntity.Isantibiotics = "1";
                }
                _ReportCardEntity.Antibiotics = this.txtANTIBIOTICS.Text;
                //避孕药
                if (checkISBIRTHCONTROLPILL.Checked)
                {
                    _ReportCardEntity.Isbirthcontrolpill = "1";
                }
                _ReportCardEntity.Birthcontrolpill = this.txtBIRTHCONTROLPILL.Text;
                //镇静药
                if (checkISSEDATIVES.Checked)
                {
                    _ReportCardEntity.Issedatives = "1";
                }
                _ReportCardEntity.Sedatives = this.txtSEDATIVES.Text;
                //服药其他
                if (checkISMEDICINEOTHER.Checked)
                {
                    _ReportCardEntity.Ismedicineother = "1";
                }
                _ReportCardEntity.Medicineother = this.txtMEDICINEOTHER.Text;
                //饮酒
                if (checkISDRINKING.Checked)
                {
                    _ReportCardEntity.Isdrinking = "1";
                }
                _ReportCardEntity.Drinking = this.txtDRINKING.Text;
                //农药
                if (checkISPESTICIDE.Checked)
                {
                    _ReportCardEntity.Ispesticide = "1";
                }
                _ReportCardEntity.Pesticide = this.txtPESTICIDE.Text;
                //射线
                if (checkISRAY.Checked)
                {
                    _ReportCardEntity.Isray = "1";
                }
                _ReportCardEntity.Ray = this.txtRAY.Text;
                //化学药剂
                if (checkISCHEMICALAGENTS.Checked)
                {
                    _ReportCardEntity.Ischemicalagents = "1";
                }
                _ReportCardEntity.Chemicalagents = this.txtCHEMICALAGENTS.Text;
                //其他有害因素
                if (checkISFACTOROTHER.Checked)
                {
                    _ReportCardEntity.Isfactorother = "1";
                }
                _ReportCardEntity.Factorother = this.txtFACTOROTHER.Text;
                //死胎例数
                _ReportCardEntity.Stillbirthno = this.txtSTILLBIRTHNO.Text;
                //自然流产例数
                _ReportCardEntity.Abortionno = this.txtABORTIONNO.Text;
                //缺陷儿例数
                _ReportCardEntity.Defectsno = this.txtDEFECTSNO.Text;
                //缺陷名
                _ReportCardEntity.Defectsof1 = this.txtDEFECTSOF1.Text;
                _ReportCardEntity.Defectsof2 = this.txtDEFECTSOF2.Text;
                _ReportCardEntity.Defectsof3 = this.txtDEFECTSOF3.Text;
                //遗传缺陷名
                _ReportCardEntity.Ycdefectsof1 = this.txtYCDEFECTSOF1.Text;
                _ReportCardEntity.Ycdefectsof2 = this.txtYCDEFECTSOF2.Text;
                _ReportCardEntity.Ycdefectsof3 = this.txtYCDEFECTSOF3.Text;
                //亲缘关系
                _ReportCardEntity.Kinshipdefects1 = this.txtKINSHIPDEFECTS1.Text;
                _ReportCardEntity.Kinshipdefects2 = this.txtKINSHIPDEFECTS2.Text;
                _ReportCardEntity.Kinshipdefects3 = this.txtKINSHIPDEFECTS3.Text;
                //近亲
                if (checkCOUSINMARRIAGE1.Checked)
                {
                    _ReportCardEntity.Cousinmarriage = "1";
                }
                if (checkCOUSINMARRIAGE2.Checked)
                {
                    _ReportCardEntity.Cousinmarriage = "2";
                }
                _ReportCardEntity.Cousinmarriagebetween = this.txtCOUSINMARRIAGEBETWEEN.Text;
                //填表人
                // _ReportCardEntity.Preparer = this.txtPREPARER.Text;
                _ReportCardEntity.Preparer = this.lookUpEditorDoc.CodeValue.ToString().Trim();
                _ReportCardEntity.Preparer = this.lookUpEditorDoc.Text.ToString().Trim();
                //填表人职称
                _ReportCardEntity.Thetitle1 = this.txtTHETITLE1.Text;
                //填表日期年
                _ReportCardEntity.Filldateyear = this.txtFILLDATEYEAR.Text;
                //填表日期月
                _ReportCardEntity.Filldatemonth = this.txtFILLDATEMONTH.Text;
                //填表日期日
                _ReportCardEntity.Filldateday = this.txtFILLDATEDAY.Text;
                //医院审表人
                // _ReportCardEntity.Hospitalreview = this.txtHOSPITALREVIEW.Text;
                _ReportCardEntity.Hospitalreview = this.lookUpEditorAudit.CodeValue.ToString().Trim();
                _ReportCardEntity.Hospitalreview = this.lookUpEditorAudit.Text.ToString().Trim();
                //医院审表人职称
                _ReportCardEntity.Thetitle2 = this.txtTHETITLE2.Text;
                //医院审表日期年
                _ReportCardEntity.Hospitalauditdateyear = this.txtHOSPITALAUDITDATEYEAR.Text;
                //医院审表日期月
                _ReportCardEntity.Hospitalauditdatemonth = this.txtHOSPITALAUDITDATEMONTH.Text;
                //医院审表日期日
                _ReportCardEntity.Hospitalauditdateday = this.txtHOSPITALAUDITDATEDAY.Text;
                //省级审表人
                _ReportCardEntity.Provincialview = this.txtPROVINCIALVIEW.Text;
                //省级审表人职称
                _ReportCardEntity.Thetitle3 = this.txtTHETITLE3.Text;
                //省级审表日期年
                _ReportCardEntity.Provincialviewdateyear = this.txtPROVINCIALVIEWDATEYEAR.Text;
                //省级审表日期月
                _ReportCardEntity.Provincialviewdatemonth = this.txtPROVINCIALVIEWDATEMONTH.Text;
                //省级审表日期日                   
                _ReportCardEntity.Provincialviewdateday = this.txtPROVINCIALVIEWDATEDAY.Text;
                //产前
                if (checkPRENATAL.Checked)
                {
                    _ReportCardEntity.Prenatal = "1";
                    _ReportCardEntity.Postpartum = "";
                }
                //产前周数
                _ReportCardEntity.Prenatalno = this.txtPRENATALNO.Text;
                //产后
                if (checkPOSTPARTUM.Checked)
                {
                    _ReportCardEntity.Postpartum = "1";
                    _ReportCardEntity.Prenatal = "";
                }
                //并指左
                if (checkANDTOSHOWLEFT.Checked)
                {
                    _ReportCardEntity.Andtoshowleft = "1";
                }
                //并指右
                if (checkANDTOSHOWRIGHT.Checked)
                {
                    _ReportCardEntity.Andtoshowright = "1";

                }


            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return null;
            }

            return _ReportCardEntity;
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

        /// <summary>
        /// 获取数据库记录下一个编码，用于生成卡片编号
        /// </summary>
        /// <returns></returns>
        public string GetNextId()
        {
            try
            {
                string nextid = string.Empty;
                DataTable dt = m_Host.SqlHelper.ExecuteDataTable("select count(*) as num from BIRTHDEFECTSCARD where VAILD='1' ");
                nextid = (Int32.Parse(dt.Rows[0]["num"].ToString() + 1)).ToString();
                return nextid;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return "100";
            }

        }

        //控制页面控件只读与否
        private void EnableState(bool isshow)
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    if (control.GetType() == typeof(XtraScrollableControl))
                    {
                        foreach (Control control1 in control.Controls)
                        {
                            if (control1 is TextEdit)
                            {
                                ((TextEdit)control1).Properties.ReadOnly = !isshow;
                            }
                            if (control1 is LookUpEditor)
                            {
                                ((LookUpEditor)control1).ReadOnly = !isshow;
                            }
                            if (control1 is Panel)
                            {
                                foreach (Control ct in control1.Controls)
                                {
                                    if (ct.GetType() == typeof(TextEdit))
                                    {
                                        ((TextEdit)ct).Properties.ReadOnly = !isshow;
                                    }
                                    if (ct.GetType() == typeof(LookUpEditor))
                                    {
                                        ((LookUpEditor)ct).ReadOnly = !isshow;
                                    }
                                    if (ct.GetType() == typeof(CheckEdit))
                                    {
                                        ((CheckEdit)ct).Properties.ReadOnly = !isshow;
                                    }
                                    if (ct is Panel)
                                    {
                                        foreach (Control ctl in ct.Controls)
                                        {
                                            if (ctl.GetType() == typeof(TextEdit))
                                            {
                                                ((TextEdit)ctl).Properties.ReadOnly = !isshow;
                                            }
                                            if (ctl.GetType() == typeof(LookUpEditor))
                                            {
                                                ((LookUpEditor)ctl).ReadOnly = !isshow;
                                            }
                                            if (ctl.GetType() == typeof(CheckEdit))
                                            {
                                                ((CheckEdit)ctl).Properties.ReadOnly = !isshow;
                                            }
                                        }
                                    }
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

        }

        public bool Save()
        {
            try
            {
                if (m_ReportCardEntity == null)//&& m_dataTableDiagicd == null
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条病人信息或补录缺陷儿报告信息");
                    return false;
                }

                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return false;
                }
                if (checkdata())
                {
                    string ErrorMsg;//用于在判断保存状态下，单据状态已经改变导致保存失败，而返回的消息
                    //保存数据
                    bool boo = PrivateSave(out ErrorMsg);
                    if (!boo)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ErrorMsg);
                    }
                    return boo;
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

        private string CheckItem()
        {
            //if (string.IsNullOrEmpty(this.txtBORN_MONTH.Text.Trim()))
            //{
            //    this.txtBORN_MONTH.Focus();
            //    return "请输入所属区县";
            //}
            return string.Empty; ;
        }

        public void LoadPage(string id, string type, string userRole)
        {
            try
            {
                if (id == null)
                    return;
                SqlHelper = new SqlHelper(m_Host.SqlHelper);
                if (type == "1")
                {
                    m_ReportCardEntity = SqlHelper.GetBirthDefectsReportCardEntity(id);
                }
                else if (type == "2")
                {
                    m_ReportCardEntity = SqlHelper.GetInpatientByNoofinpat1(id);
                    m_ReportCardEntity.CreateDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                    m_ReportCardEntity.ReportNoofinpat = id;
                    m_ReportCardEntity.CreateDeptcode = m_Host.User.CurrentDeptId;
                    m_ReportCardEntity.CreateUsercode = m_Host.User.Id;
                    m_ReportCardEntity.CreateUsername = m_Host.User.Name;
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

        private bool PrivateSave(out string errorinfo)
        {
            try
            {

                //新增保存
                if (m_ReportCardEntity.Id == "0")
                {
                    ReportCardEntity1 _ReportCardEntity = GetEntityUI(m_ReportCardEntity);
                    _ReportCardEntity.DiagCode = DiagICD10;
                    _ReportCardEntity.State = "1";
                    if (_ReportCardEntity == null)
                    {
                        errorinfo = "报告实体不能为空";
                        return false;
                    }




                    //_ReportCardEntity.DIAGICD10 = lookUpEditorDialogICD10.CodeValue;

                    if (SqlHelper == null)
                    {
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    }
                    try
                    {
                        int result = 1;
                        ID = m_SqlHelper.AddBirthDefectsReportCard(_ReportCardEntity);
                        if (!string.IsNullOrEmpty(ID))
                        {
                            //m_ReportCardEntity = m_SqlHelper.GetReportCardEntity(ReportID);
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
                    string m_ReportId = m_ReportCardEntity.Id.ToString();//获得报告卡号
                    string m_OldStateId = m_ReportCardEntity.State.ToString();//获得原来的状态字段
                    ReportCardEntity1 _ReportCardEntity = GetEntityUI(m_ReportCardEntity);
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
                        ReportCardEntity1 _myReportCardEntity = new ReportCardEntity1();
                        _myReportCardEntity = SqlHelper.GetBirthDefectsReportCardEntity(m_ReportId);
                        //m_SqlHelper.EditReportCard(_ReportCardEntity);

                        //int result = 1;
                        //errorinfo = result == 1 ? "修改成功" : "修改失败";
                        //return result == 1;

                        if (m_OldStateId == _myReportCardEntity.State)//原来状态和现在状态一致
                        {
                            m_SqlHelper.EditbirthdefectsReportCard(_ReportCardEntity);
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

        public void ClearPage()
        {
            try
            {
                foreach (Control control in this.Controls)
                {
                    if (control.GetType() == typeof(XtraScrollableControl))
                    {
                        foreach (Control control1 in control.Controls)
                        {
                            if (control1.GetType() == typeof(TextEdit))
                            {
                                ((TextEdit)control1).Text = "";
                            }
                            if (control1.GetType() == typeof(Panel))
                            {
                                foreach (Control ct in control1.Controls)
                                {
                                    if (ct.GetType() == typeof(TextEdit))
                                    {
                                        ((TextEdit)ct).Text = "";
                                    }
                                    if (ct.GetType() == typeof(CheckEdit))
                                    {
                                        ((CheckEdit)ct).Checked = false;
                                    }
                                    if (ct.GetType() == typeof(LookUpEditor))
                                    {
                                        ((LookUpEditor)ct).Text = "";
                                    }
                                    if (ct is Panel)
                                    {
                                        foreach (Control ctl in ct.Controls)
                                        {
                                            if (ctl.GetType() == typeof(TextEdit))
                                            {
                                                ((TextEdit)ctl).Text = "";
                                            }
                                            if (ctl.GetType() == typeof(CheckEdit))
                                            {
                                                ((CheckEdit)ctl).Checked = false;
                                            }
                                            if (ctl.GetType() == typeof(LookUpEditor))
                                            {
                                                ((LookUpEditor)ctl).Text = "";
                                            }
                                        }
                                    }
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
        }

        private void checkBIRTHS3_CheckedChanged(object sender, EventArgs e)
        {
            checkISIDENTICAL1.Enabled = true;
            checkISIDENTICAL2.Enabled = true;
        }

        private void checkBIRTHS1_CheckedChanged(object sender, EventArgs e)
        {
            checkISIDENTICAL1.Enabled = false;
            checkISIDENTICAL2.Enabled = false;
            checkISIDENTICAL1.Checked = false;
            checkISIDENTICAL2.Checked = false;
        }


        private void checkDIAGNOSTICBASIS3_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDIAGNOSTICBASIS4.Enabled)
            {
                txtDIAGNOSTICBASIS4.Enabled = false;
                txtDIAGNOSTICBASIS4.Text = string.Empty;
            }
            else
            {
                txtDIAGNOSTICBASIS4.Enabled = true;
            }
        }
        private void checkDIAGNOSTICBASIS6_CheckedChanged(object sender, EventArgs e)
        {
            if (txtDIAGNOSTICBASIS7.Enabled)
            {
                txtDIAGNOSTICBASIS7.Enabled = false;
                txtDIAGNOSTICBASIS7.Text = string.Empty;
            }
            else
            {
                txtDIAGNOSTICBASIS7.Enabled = true;
            }
        }

        private void checkPRENATAL_CheckedChanged(object sender, EventArgs e)
        {
            if (checkPRENATAL.Checked)
            {
                txtPRENATALNO.Enabled = true;
            }
            else
            {
                txtPRENATALNO.Enabled = false;
                txtPRENATALNO.Text = string.Empty;
            }
        }

        private void checkDIAG_OTHER_CheckedChanged(object sender, EventArgs e)
        {
            if (checkDIAG_OTHER.Checked)
            {
                txtDIAG_OTHERCONTENT.Enabled = true;
            }
            else
            {
                txtDIAG_OTHERCONTENT.Enabled = false;
                txtDIAG_OTHERCONTENT.Text = string.Empty;
            }
        }

        private void checkCOUSINMARRIAGE2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkCOUSINMARRIAGE2.Checked)
            {
                txtCOUSINMARRIAGEBETWEEN.Enabled = true;
            }
            else
            {
                txtCOUSINMARRIAGEBETWEEN.Enabled = false;
                txtCOUSINMARRIAGEBETWEEN.Text = string.Empty;
            }
        }


        private void checkEdit_CheckedChanged(object sender, EventArgs e)
        {
            string name = GetNextControlName(((Control)sender).Name);
            TextEdit textEdit = GetNextTextEdit(name);
            if (!textEdit.Enabled)
            {
                textEdit.Enabled = true;
            }
            else
            {
                textEdit.Enabled = false;
                textEdit.Text = string.Empty;
            }
        }

        public string GetNextControlName(string currentControlName)
        {
            string nextControlName = string.Empty;
            if (currentControlName.EndsWith("VIRUSINFECTION"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("ILLOTHER"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("SULFA"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("ANTIBIOTICS"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("BIRTHCONTROLPILL"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("SEDATIVES"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("MEDICINEOTHER"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("DRINKING"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("PESTICIDE"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("RAY"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("CHEMICALAGENTS"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            if (currentControlName.EndsWith("FACTOROTHER"))
            {
                nextControlName = "txt" + currentControlName.Substring("checkIS".Length);
            }
            return nextControlName;
        }

        private TextEdit GetNextTextEdit(string nextControlName)
        {
            foreach (Control control in this.Controls)
            {
                if (control.GetType() == typeof(XtraScrollableControl))
                {
                    foreach (Control control1 in control.Controls)
                    {
                        if (control1 is Panel)
                        {
                            foreach (Control ctl in control1.Controls)
                            {
                                if (ctl is TextEdit)
                                {
                                    if (ctl.Name == nextControlName)
                                    {
                                        return (TextEdit)ctl;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        public void GetDataDiagNosis(string noofinpat)
        {
            try
            {
                //if (m_dataTableDiagicd == null)
                {
                    m_dataTableDiagicd = m_SqlHelper.GetDiagWithBirthDefects(noofinpat);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return;
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
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条出生缺陷上报记录或补录出生缺陷报告信息");
                    return false;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要审核通过吗？", "审核通过", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    ReportCardEntity1 _ReportCardEntity = GetEntityUI(m_ReportCardEntity);
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
                        SqlHelper.EditbirthdefectsReportCard(_ReportCardEntity);
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
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条出生缺陷上报记录或补录出生缺陷报告信息");
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
                            m_ReportCardEntity.Cancelreason = unPassReason.PassReason;
                            ReportCardEntity1 _ReportCardEntity = GetEntityUI(m_ReportCardEntity);
                            if (_ReportCardEntity == null)
                                return false;
                            _ReportCardEntity.State = "5";

                            if (SqlHelper == null)
                                SqlHelper = new SqlHelper(m_Host.SqlHelper);
                            try
                            {
                                m_SqlHelper.EditbirthdefectsReportCard(_ReportCardEntity);
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

                if (m_ReportCardEntity.Id == "0")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该报告卡上报记录尚未保存或提交，不需要删除");
                    return false;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除吗？", "删除报告卡上报记录", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    ReportCardEntity1 _ReportCardEntity = GetEntityUI(m_ReportCardEntity);
                    if (_ReportCardEntity == null)
                        return false;
                    _ReportCardEntity.State = "7";

                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);

                    SqlHelper.EditbirthdefectsReportCard(_ReportCardEntity);
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

    }
}
