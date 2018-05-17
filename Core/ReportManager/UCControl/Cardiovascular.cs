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
    /// <summary>
    ///心脑血管病发病报告卡界面
    ///add by ywk 2013年8月14日 13:12:37
    /// </summary>
    public partial class Cardiovascular : DevExpress.XtraEditors.XtraUserControl
    {
        #region 字段及属性
        IEmrHost m_Host;
        public CardiovasularEntity m_CardiovasularEntity;
        private SqlHelper m_SqlHelper;
        public string m_Noofinpat;//患者序号
        public string ID;//当前报告卡序号
        public DataTable m_DTCularDiagicd;//诊断数据源
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

        public string DiagICD
        {
            get { return m_diagicd; }
            set
            {
                m_diagicd = value;
            }
        }
        private string m_diagicd;//当前诊断编号
        #endregion

        #region 构造函数
        public Cardiovascular()
        {
            InitializeComponent();
        }

        public Cardiovascular(IEmrHost app)
        {
            InitializeComponent();
            m_Host = app;
        }
        #endregion

        #region 事件
        /// <summary>
        /// 窗体加载事件
        /// add by ywk 2013年8月20日 13:24:54
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cardiovascular_Load(object sender, EventArgs e)
        {
            DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
            InitLookUpEditor();
        }


        #endregion

        #region 方法
        /// <summary>
        /// 报告卡提交方法
        /// add by ywk 2013年8月26日 15:11:31
        /// </summary>
        /// <returns></returns>
        public bool Submit()
        {
            try
            {
                bool issuccess = false;
                if (m_CardiovasularEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条心脑血管病卡上报记录或补录心脑血管病报告信息");
                    issuccess = false;
                }
                if (CheckMustInput())
                {
                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要提交该心脑血管病卡记录吗？", "提交心脑血管病卡", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                    {
                        if (SqlHelper == null)
                        {
                            SqlHelper = new SqlHelper(m_Host.SqlHelper);
                        }
                        string m_ReportId = m_CardiovasularEntity.ReportID.ToString();//获得报告卡号
                        //提交操作判断状态是否改变 
                        CardiovasularEntity m_CardEntity = new CardiovasularEntity();
                        m_CardEntity = SqlHelper.GetCardiovasularEntity(m_ReportId);
                        if ("7" != m_CardEntity.STATE)//状态为1（已经保存的）才进行提交
                        {
                            m_CardEntity.STATE = "2";
                            m_SqlHelper.EditcardiovascularCard(m_CardEntity);
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
        /// <summary>
        /// 验证必填项
        /// 除“工作单位”、、“死亡日期”、“死亡原因”及最后一行的“ICD10编码”外，其余限制为必填项目。
        /// </summary>
        /// <returns></returns>
        private bool CheckMustInput()
        {
            if (string.IsNullOrEmpty(this.txtNOOFCLINIC.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写门诊号");
                txtNOOFCLINIC.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtPatid.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写住院号");
                txtPatid.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtname.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写姓名");
                txtname.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtIDNO.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写身份证号");
                txtIDNO.Focus();
                return false;
            }

            if (!chksexM.Checked && !chksexW.Checked)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择性别");
                chksexM.Focus();
                chksexM.Checked = true;
                return false;
            }
            if (string.IsNullOrEmpty(this.dateBIRTHDATE.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写出生日期");
                dateBIRTHDATE.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.lueNation.CodeValue))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写民族");
                lueNation.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.lueJob.CodeValue))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写职业");
                lueJob.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtCONTACTTEL.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写联系电话");
                txtCONTACTTEL.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtHKPROVICE.Text.ToString().Trim()) &&
                string.IsNullOrEmpty(this.txtHKCITY.Text.ToString().Trim()) &&
                string.IsNullOrEmpty(this.txtHKSTREET.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写户籍地址");
                txtHKPROVICE.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtHKADDRESSID.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写户籍地址编码");
                txtHKADDRESSID.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtXZZPROVICE.Text.ToString().Trim()) &&
                string.IsNullOrEmpty(this.txtXZZCITY.Text.ToString().Trim()) &&
                string.IsNullOrEmpty(this.txtXZZSTREET.Text.ToString().Trim()) &&
                string.IsNullOrEmpty(this.txtXZZCOMMITTEES.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写现住地址");
                txtXZZPROVICE.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.txtXZZADDRESSID.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写现住地址编码");
                txtXZZADDRESSID.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.lookUpEditorDialogICD10.CodeValue))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择ICD编码");
                lookUpEditorDialogICD10.Focus();
                return false;
            }
            if (this.chkLCZZ.Checked == false && this.chkXDT.Checked == false && this.chkXGZY.Checked == false
                && this.chkCT.Checked == false && this.chkCGZ.Checked == false && this.chkTGJC.Checked == false
                && this.chkCSJC.Checked == false && this.chkSYSJC.Checked == false && this.chkSWBFB.Checked == false)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择诊断依据");
                chkLCZZ.Focus();
                chkLCZZ.Checked = true;
                return false;
            }
            if (string.IsNullOrEmpty(this.dateDIAGNOSEDATE.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择确诊日期");
                dateDIAGNOSEDATE.Focus();
                return false;
            }
            if (!chkIsNoSick.Checked && !chkIsSick.Checked)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择是否首次发病");
                chkIsNoSick.Focus();
                chkIsNoSick.Checked = true;
                return false;
            }
            if (!chkSHosp.Checked && !chkCityHosp.Checked && !chkXJHosp.Checked && !chkXZJHosp.Checked && !chkQTHosp.Checked && !chkBXHosp.Checked)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择确诊单位");
                chkSHosp.Focus();
                chkSHosp.Checked = true;
                return false;
            }
            if (!chkoutflagZY.Checked && !chkoutflagHZ.Checked && !chkoutflagWY.Checked && !chkoutflagSW.Checked && !chkoutflagQT.Checked)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择转归情况");
                chkoutflagZY.Focus();
                chkoutflagZY.Checked = true;
                return false;
            }
            if (string.IsNullOrEmpty(txtREPORTDEPT.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请填写报告单位");
                txtREPORTDEPT.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(lookUpEditorDoc.CodeValue))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择报告医师");
                lookUpEditorDoc.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.dateREPORTDATE.Text.ToString().Trim()))
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择报告日期");
                dateREPORTDATE.Focus();
                return false;
            }
            return true;   //Edit jxh  原来是false？
        }

        public void Tesr()
        {
            MessageBox.Show(this.txtCONTACTTEL.Text);
        }
        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitLookUpEditor()
        {
            InitJob();
            InitNation();

            InitDoctor("");
            InitDiag();
        }
        /// <summary>
        /// 诊断下拉框
        /// </summary>
        private void InitDiag()
        {
            string sql_diag = "select icd,name,py,wb from diagnosis";
            DataTable diag = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql_diag);
            diag.Columns["ICD"].Caption = "编号";
            diag.Columns["NAME"].Caption = "诊断名称";
            Dictionary<string, int> colDiag = new Dictionary<string, int>();
            colDiag.Add("ICD", 80);
            colDiag.Add("NAME", 160);

            LookUpWindow lookUpWindowInDiag = new LookUpWindow();
            this.lookUpEditorDialogICD10.ListWindow = lookUpWindowInDiag;
            SqlWordbook outWordBook = new SqlWordbook("outDiag", diag, "ICD", "NAME", colDiag, "ICD//NAME//PY//WB");
            this.lookUpEditorDialogICD10.SqlWordbook = outWordBook;
        }

        /// <summary>
        /// 绑定医生下拉框
        /// </summary>
        private void InitDoctor(string deptid)
        {

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
        /// 获取数据库记录下一个编码，用于生成卡片编号
        /// </summary>
        /// <returns></returns>
        public string GetNextId()
        {
            try
            {
                string nextid = string.Empty;
                DataTable dt = m_Host.SqlHelper.ExecuteDataTable("select count(*)  as id  from cardiovascularcard where VAILD='1' ");
                //nextid = (dt.Rows.Count + 1).ToString();
                nextid = (Int32.Parse(dt.Rows[0]["id"].ToString() + 1)).ToString();
                return nextid;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return "100";
            }

        }
        /// <summary>
        /// 根据实体填充窗体控件值
        /// add by ywk 2013年8月20日 13:33:06
        /// </summary>
        /// <param name="m_CardiovasularEntity"></param>
        public void FillUIBYEntity(CardiovasularEntity m_CardiovasularEntity)
        {
            try
            {
                //InitJob();
                //InitNation();

                //InitDoctor("");
                //InitDiag();

                m_Noofinpat = m_CardiovasularEntity.NOOFINPAT;
                this.txtNOOFCLINIC.Text = m_CardiovasularEntity.NOOFCLINIC;
                this.txtPatid.Text = m_CardiovasularEntity.PATID;
                this.txtname.Text = m_CardiovasularEntity.NAME;
                this.txtIDNO.Text = m_CardiovasularEntity.IDNO;
                if (m_CardiovasularEntity.SEXID == "1")
                {
                    chksexM.Checked = true;
                }
                if (m_CardiovasularEntity.SEXID == "2")
                {
                    chksexW.Checked = true;
                }
                this.dateBIRTHDATE.Text = m_CardiovasularEntity.BIRTH;
                this.txtAge.Text = m_CardiovasularEntity.AGE;
                if (!string.IsNullOrEmpty(m_CardiovasularEntity.NationId))
                {
                    this.lueNation.CodeValue = m_CardiovasularEntity.NationId;
                }
                if (!string.IsNullOrEmpty(m_CardiovasularEntity.JOBID))
                {
                    this.lueJob.CodeValue = m_CardiovasularEntity.JOBID;
                }



                this.txtOFFICEADDRESS.Text = m_CardiovasularEntity.OFFICEPLACE;
                this.txtCONTACTTEL.Text = m_CardiovasularEntity.CONTACTTEL;
                this.txtHKPROVICE.Text = m_CardiovasularEntity.HKPROVICE;
                this.txtHKCITY.Text = m_CardiovasularEntity.HKCITY;
                this.txtHKSTREET.Text = m_CardiovasularEntity.HKSTREET;
                this.txtHKADDRESSID.Text = m_CardiovasularEntity.HKADDRESSID;
                this.txtXZZPROVICE.Text = m_CardiovasularEntity.XZZPROVICE;
                this.txtXZZCITY.Text = m_CardiovasularEntity.XZZCITY;
                this.txtXZZSTREET.Text = m_CardiovasularEntity.XZZSTREET;
                this.txtXZZCOMMITTEES.Text = m_CardiovasularEntity.XZZCOMMITTEES;
                this.txtXZZPARM.Text = m_CardiovasularEntity.XZZPARM;
                this.txtXZZADDRESSID.Text = m_CardiovasularEntity.XZZADDRESSID;
                if (!string.IsNullOrEmpty(m_CardiovasularEntity.ICD))
                {
                    this.lookUpEditorDialogICD10.CodeValue = m_CardiovasularEntity.ICD;
                }

                if (m_CardiovasularEntity.DIAGZWMXQCX == "1")
                {
                    chkDIAGZWMXQCX.Checked = true;
                }
                else
                {
                    chkDIAGZWMXQCX.Checked = false;
                }
                if (m_CardiovasularEntity.DIAGNCX == "1")
                {
                    chkDIAGNCX.Checked = true;
                }
                else
                {
                    chkDIAGNCX.Checked = false;
                }
                if (m_CardiovasularEntity.DIAGNGS == "1")
                {
                    chkDIAGNGS.Checked = true;
                }
                else
                {
                    chkDIAGNGS.Checked = false;
                }
                if (m_CardiovasularEntity.DIAGWFLNZZ == "1")
                {
                    chkDIAGWFLNZZ.Checked = true;
                }
                else
                {
                    chkDIAGWFLNZZ.Checked = false;
                }
                if (m_CardiovasularEntity.DIAGJXXJGS == "1")
                {
                    chkDIAGJXXJGS.Checked = true;
                }
                else
                {
                    chkDIAGJXXJGS.Checked = false;
                }
                if (m_CardiovasularEntity.DIAGXXCS == "1")
                {
                    chkDIAGXXCS.Checked = true;
                }
                else
                {
                    chkDIAGXXCS.Checked = false;
                }
                if (!String.IsNullOrEmpty(m_CardiovasularEntity.DIAGNOSISBASED))
                {
                    string[] strArray = m_CardiovasularEntity.DIAGNOSISBASED.Split(new char[] { ',' });
                    foreach (string str in strArray)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            switch (str)
                            {
                                case "1":
                                    this.chkLCZZ.Checked = true;
                                    this.chkXDT.Checked = false;
                                    this.chkXGZY.Checked = false;
                                    this.chkCT.Checked = false;
                                    this.chkCGZ.Checked = false;
                                    this.chkTGJC.Checked = false;
                                    this.chkCSJC.Checked = false;
                                    this.chkSYSJC.Checked = false;
                                    this.chkSWBFB.Checked = false;
                                    break;
                                case "2":
                                    this.chkXDT.Checked = true;
                                    this.chkLCZZ.Checked = false;
                                    this.chkXGZY.Checked = false;
                                    this.chkCT.Checked = false;
                                    this.chkCGZ.Checked = false;
                                    this.chkTGJC.Checked = false;
                                    this.chkCSJC.Checked = false;
                                    this.chkSYSJC.Checked = false;
                                    this.chkSWBFB.Checked = false;
                                    break;
                                case "3":
                                    this.chkXGZY.Checked = true;
                                    this.chkLCZZ.Checked = false;
                                    this.chkXDT.Checked = false;
                                    this.chkCT.Checked = false;
                                    this.chkCGZ.Checked = false;
                                    this.chkTGJC.Checked = false;
                                    this.chkCSJC.Checked = false;
                                    this.chkSYSJC.Checked = false;
                                    this.chkSWBFB.Checked = false;
                                    break;
                                case "4":
                                    this.chkCT.Checked = true;
                                    this.chkLCZZ.Checked = false;
                                    this.chkXDT.Checked = false;
                                    this.chkXGZY.Checked = false;
                                    this.chkCGZ.Checked = false;
                                    this.chkTGJC.Checked = false;
                                    this.chkCSJC.Checked = false;
                                    this.chkSYSJC.Checked = false;
                                    this.chkSWBFB.Checked = false;
                                    break;
                                case "5":
                                    this.chkCGZ.Checked = true;
                                    this.chkLCZZ.Checked = false;
                                    this.chkXDT.Checked = false;
                                    this.chkXGZY.Checked = false;
                                    this.chkCT.Checked = false;
                                    this.chkTGJC.Checked = false;
                                    this.chkCSJC.Checked = false;
                                    this.chkSYSJC.Checked = false;
                                    this.chkSWBFB.Checked = false;
                                    break;
                                case "6":
                                    this.chkTGJC.Checked = true;
                                    this.chkLCZZ.Checked = false;
                                    this.chkXDT.Checked = false;
                                    this.chkXGZY.Checked = false;
                                    this.chkCT.Checked = false;
                                    this.chkCGZ.Checked = false;
                                    this.chkCSJC.Checked = false;
                                    this.chkSYSJC.Checked = false;
                                    this.chkSWBFB.Checked = false;
                                    break;
                                case "7":
                                    this.chkCSJC.Checked = true;
                                    this.chkLCZZ.Checked = false;
                                    this.chkXDT.Checked = false;
                                    this.chkXGZY.Checked = false;
                                    this.chkCT.Checked = false;
                                    this.chkCGZ.Checked = false;
                                    this.chkTGJC.Checked = false;
                                    this.chkSYSJC.Checked = false;
                                    this.chkSWBFB.Checked = false;
                                    break;
                                case "8":
                                    this.chkSYSJC.Checked = true;
                                    this.chkLCZZ.Checked = false;
                                    this.chkXDT.Checked = false;
                                    this.chkXGZY.Checked = false;
                                    this.chkCT.Checked = false;
                                    this.chkCGZ.Checked = false;
                                    this.chkTGJC.Checked = false;
                                    this.chkCSJC.Checked = false;
                                    this.chkSWBFB.Checked = false;
                                    break;
                                case "9":
                                    this.chkSWBFB.Checked = true;
                                    this.chkLCZZ.Checked = false;
                                    this.chkXDT.Checked = false;
                                    this.chkXGZY.Checked = false;
                                    this.chkCT.Checked = false;
                                    this.chkCGZ.Checked = false;
                                    this.chkTGJC.Checked = false;
                                    this.chkCSJC.Checked = false;
                                    this.chkSYSJC.Checked = false;
                                    break;
                            }
                        }
                    }
                }
                this.dateDIAGNOSEDATE.Text = m_CardiovasularEntity.DIAGNOSEDATE;
                if (m_CardiovasularEntity.ISFIRSTSICK == "1")
                {
                    chkIsSick.Checked = true;
                }
                else
                {
                    chkIsNoSick.Checked = true;
                }
                if (m_CardiovasularEntity.DIAGHOSPITAL == "1")
                {
                    chkSHosp.Checked = true;
                }
                else
                {
                    chkSHosp.Checked = false;
                }
                if (m_CardiovasularEntity.DIAGHOSPITAL == "2")
                {
                    chkCityHosp.Checked = true;
                }
                else
                {
                    chkCityHosp.Checked = false;
                }
                if (m_CardiovasularEntity.DIAGHOSPITAL == "3")
                {
                    chkXJHosp.Checked = true;
                }
                else
                {
                    chkXJHosp.Checked = false;
                }
                if (m_CardiovasularEntity.DIAGHOSPITAL == "4")
                {
                    chkXZJHosp.Checked = true;
                }
                else
                {
                    chkXZJHosp.Checked = false;
                }
                if (m_CardiovasularEntity.DIAGHOSPITAL == "5")
                {
                    chkQTHosp.Checked = true;
                }
                else
                {
                    chkQTHosp.Checked = false;
                }
                if (m_CardiovasularEntity.DIAGHOSPITAL == "9")
                {
                    chkBXHosp.Checked = true;
                }
                else
                {
                    chkBXHosp.Checked = false;
                }
                if (m_CardiovasularEntity.OUTFLAG == "1")
                {
                    chkoutflagZY.Checked = true;
                }
                else
                {
                    chkoutflagZY.Checked = false;
                }
                if (m_CardiovasularEntity.OUTFLAG == "2")
                {
                    chkoutflagHZ.Checked = true;
                }
                else
                {
                    chkoutflagHZ.Checked = false;
                }
                if (m_CardiovasularEntity.OUTFLAG == "3")
                {
                    chkoutflagWY.Checked = true;
                }
                else
                {
                    chkoutflagWY.Checked = false;
                }
                if (m_CardiovasularEntity.OUTFLAG == "4")
                {
                    chkoutflagSW.Checked = true;
                }
                else
                {
                    chkoutflagSW.Checked = false;
                }
                if (m_CardiovasularEntity.OUTFLAG == "5")
                {
                    chkoutflagQT.Checked = true;
                }
                else
                {
                    chkoutflagQT.Checked = false;
                }
                this.deteDEATHDATE.Text = m_CardiovasularEntity.DIEDATE;
                this.txtREPORTDEPT.Text = m_CardiovasularEntity.REPORTDEPT;
                this.lookUpEditorDoc.CodeValue = m_CardiovasularEntity.REPORTUSERCODE == "" ? m_Host.User.Id : m_CardiovasularEntity.REPORTUSERCODE;
                this.dateREPORTDATE.Text = m_CardiovasularEntity.REPORTDATE;

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("FillUIBYEntity出错信息为" + ex.Message);
            }

        }
        /// <summary>
        /// 根据窗体值给实体赋值
        /// add by ywk 2013年8月20日 11:49:45
        /// </summary>
        /// <returns></returns>
        public CardiovasularEntity GetEntityUI()
        {
            InitJob();
            InitNation();

            InitDoctor("");
            InitDiag();
            CardiovasularEntity cardentity = new CardiovasularEntity();
            cardentity.AGE = this.txtAge.Text.ToString().Trim();
            cardentity.BIRTH = this.dateBIRTHDATE.Text.ToString().Trim();
            cardentity.CONTACTTEL = this.txtCONTACTTEL.Text.ToString().Trim();
            cardentity.NOOFINPAT = m_Noofinpat;
            cardentity.CREATE_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
            cardentity.CREATE_DEPTCODE = m_Host.User.CurrentDeptId;
            cardentity.CREATE_DEPTNAME = m_Host.User.CurrentDeptName;
            cardentity.CREATE_USERCODE = m_Host.User.Id;
            cardentity.CREATE_USERNAME = m_Host.User.Name;
            if (chkSHosp.Checked)   // edit  jxh   原来是chkbshosp
            {
                cardentity.DIAGHOSPITAL = "1";
            }
            if (chkCityHosp.Checked)
            {
                cardentity.DIAGHOSPITAL = "2";
            }
            if (chkXJHosp.Checked)
            {
                cardentity.DIAGHOSPITAL = "3";
            }
            if (chkXZJHosp.Checked)
            {
                cardentity.DIAGHOSPITAL = "4";
            }
            if (chkQTHosp.Checked)
            {
                cardentity.DIAGHOSPITAL = "5";
            }
            if (chkBXHosp.Checked)
            {
                cardentity.DIAGHOSPITAL = "9";
            }
            if (chkDIAGJXXJGS.Checked)
            {
                cardentity.DIAGJXXJGS = "1";
            }
            if (chkDIAGNCX.Checked)
            {
                cardentity.DIAGNCX = "1";
            }
            if (chkDIAGNGS.Checked)
            {
                cardentity.DIAGNGS = "1";
            }

            cardentity.DIAGNOSEDATE = this.dateDIAGNOSEDATE.Text.ToString().Trim();
            #region 诊断依据可多选，特殊处理下
            string DiagNosisBased = string.Empty;
            if (chkLCZZ.Checked)
            {
                DiagNosisBased += ",1";
            }
            if (chkXDT.Checked)
            {
                DiagNosisBased += ",2";
            }
            if (chkXGZY.Checked)
            {
                DiagNosisBased += ",3";
            }
            if (chkCT.Checked)
            {
                DiagNosisBased += ",4";
            }
            if (chkCGZ.Checked)
            {
                DiagNosisBased += ",5";
            }
            if (chkTGJC.Checked)
            {
                DiagNosisBased += ",6";
            }
            if (chkCSJC.Checked)
            {
                DiagNosisBased += ",7";
            }
            if (chkSYSJC.Checked)
            {
                DiagNosisBased += ",8";
            }
            if (chkSWBFB.Checked)
            {
                DiagNosisBased += ",9";
            }

            if (!string.IsNullOrEmpty(DiagNosisBased))
            {
                DiagNosisBased += ",";
            }
            cardentity.DIAGNOSISBASED = DiagNosisBased;
            #endregion

            if (chkDIAGWFLNZZ.Checked)
            {
                cardentity.DIAGWFLNZZ = "1";
            }
            if (chkDIAGXXCS.Checked)
            {
                cardentity.DIAGXXCS = "1";
            }
            if (chkDIAGZWMXQCX.Checked)
            {
                cardentity.DIAGZWMXQCX = "1";
            }
            cardentity.DIEDATE = this.deteDEATHDATE.Text.ToString().Trim();
            cardentity.HKADDRESSID = this.txtHKADDRESSID.Text.ToString().Trim();
            cardentity.HKCITY = this.txtHKCITY.Text.ToString().Trim();
            cardentity.HKPROVICE = this.txtHKPROVICE.Text.ToString().Trim();
            cardentity.HKSTREET = this.txtHKSTREET.Text.ToString().Trim();
            cardentity.ICD = this.lookUpEditorDialogICD10.CodeValue.ToString().Trim();
            cardentity.IDNO = this.txtIDNO.Text.ToString().Trim();
            if (chkIsSick.Checked)
            {
                cardentity.ISFIRSTSICK = "1";
            }
            if (chkIsNoSick.Checked)
            {
                cardentity.ISFIRSTSICK = "0";
            }
            cardentity.JOBID = this.lueJob.CodeValue.ToString().Trim();
            cardentity.JOBNAME = this.lueJob.Text.ToString().Trim();
            cardentity.NAME = this.txtname.Text.ToString().Trim();
            cardentity.NationId = this.lueNation.CodeValue.ToString().Trim();
            cardentity.NationName = this.lueNation.Text.ToString().Trim();
            cardentity.NOOFCLINIC = this.txtNOOFCLINIC.Text.ToString().Trim();
            cardentity.NOOFINPAT = m_Noofinpat;
            //cardentity.NOOFINPAT = m_Host.CurrentPatientInfo.NoOfFirstPage.ToString();
            cardentity.OFFICEPLACE = this.txtOFFICEADDRESS.Text.ToString().Trim();
            if (chkoutflagZY.Checked)
            {
                cardentity.OUTFLAG = "1";
            }
            if (chkoutflagHZ.Checked)
            {
                cardentity.OUTFLAG = "2";
            }
            if (chkoutflagWY.Checked)
            {
                cardentity.OUTFLAG = "3";
            }
            if (chkoutflagSW.Checked)
            {
                cardentity.OUTFLAG = "4";
            }
            if (chkoutflagQT.Checked)
            {
                cardentity.OUTFLAG = "5";
            }

            cardentity.PATID = this.txtPatid.Text.ToString().Trim();
            cardentity.REPORTDATE = this.dateREPORTDATE.Text.ToString().Trim();
            cardentity.REPORTDEPT = this.txtREPORTDEPT.Text.ToString().Trim();

            cardentity.ReportNo = DateTime.Now.Year.ToString() + "-" + GetNextId();
            cardentity.REPORTUSERCODE = this.lookUpEditorDoc.CodeValue.ToString().Trim();
            cardentity.REPORTUSERNAME = this.lookUpEditorDoc.Text.ToString().Trim();
            if (chksexM.Checked)
            {
                cardentity.SEXID = "1";
                cardentity.SEXNAME = "男";
            }
            if (chksexW.Checked)
            {
                cardentity.SEXID = "2";
                cardentity.SEXNAME = "女";
            }
            cardentity.STATE = "1";
            //cardentity.STATE=
            cardentity.VAILD = "1";
            cardentity.XZZADDRESSID = this.txtXZZADDRESSID.Text.ToString().Trim();
            cardentity.XZZCITY = this.txtXZZCITY.Text.ToString().Trim();
            cardentity.XZZCOMMITTEES = this.txtXZZCOMMITTEES.Text.ToString().Trim();
            cardentity.XZZPARM = this.txtXZZPARM.Text.ToString().Trim();
            cardentity.XZZPROVICE = this.txtXZZPROVICE.Text.ToString().Trim();
            cardentity.XZZSTREET = this.txtXZZSTREET.Text.ToString().Trim();
            return cardentity;
        }
        /// <summary>
        /// 控制界面各栏位控件的可操作性
        /// add by ywk 2013年8月20日18:39:09
        /// </summary>
        /// <param name="isshow"></param>
        public void EnableState(bool canedit)
        {
            //this.txtNOOFCLINIC.Enabled = canedit;
            //this.txtPatid.Enabled = canedit;
            //this.txtname.Enabled = canedit;
            //this.txtIDNO.Enabled = canedit;
            //this.chksexM.Enabled = canedit;
            //this.chksexW.Enabled = canedit;
            //this.dateBIRTHDATE.Enabled = canedit;
            //this.txtAge.Enabled = canedit;
            //this.
            foreach (Control item in this.Controls)     //edit  by  jxh  2013-9-7
            {
                if (item is XtraScrollableControl)
                {
                    foreach (Control control in item.Controls)
                    {
                        if (control is Panel)
                        {
                            foreach (Control ctl in control.Controls)
                            {
                                if (ctl is TextEdit)
                                {
                                    ctl.Enabled = canedit;
                                }
                                if (ctl is CheckEdit)
                                {
                                    ctl.Enabled = canedit;
                                }
                                if (ctl is LookUpEditor)
                                {
                                    ctl.Enabled = canedit;
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 加载方法，传入初始化值
        /// </summary>
        /// <param name="ID">报告卡ID或者新增报告卡时候传入病人首页序号</param>
        /// <param name="type">1、报告卡序号  2、传入病人首页序号</param>
        /// <param name="userRole">1、申请人  2、审核人</param>
        public void LoadPage(string id, string type, string userRole)
        {
            if (type == "1")
            {
                m_CardiovasularEntity = SqlHelper.GetCardiovasularEntity(id);
                //FillUIBYEntity(m_CardiovasularEntity);
            }
            else if (type == "2")
            {
                m_CardiovasularEntity = SqlHelper.GetCardInpatientByNoofinpat(id);
                m_CardiovasularEntity.ICD = DiagICD;       //edit  by   jxh  页面显示用
                m_CardiovasularEntity.REPORTUSERCODE = m_Host.User.Id;
                m_CardiovasularEntity.REPORTDATE = DateTime.Now.ToString();
                InitLookUpEditor();
            }
            ClearPageControl();
            FillUIBYEntity(m_CardiovasularEntity);
            ReadOnlyControl(userRole);
        }
        /// <summary>
        /// 根据用户权限控制按钮可编辑性
        /// add by ywk 2013年8月26日 09:11:54
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
        /// 获得患者的诊断数据
        /// </summary>
        /// <param name="noofinpat"></param>
        public void GetDataDiagNosis(string noofinpat)
        {
            try
            {
                m_DTCularDiagicd = m_SqlHelper.GetDiagWithVascular(noofinpat);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 清空界面控件的值(应用递归实现)
        /// </summary>
        public void ClearPageControl()            //edit  by  jxh   2013-9-7
        {
            foreach (Control control in this.Controls)
            {
                if (control is XtraScrollableControl)
                {
                    foreach (Control item in control.Controls)
                    {
                        if (item is Panel)
                        {
                            foreach (Control fouritem in item.Controls)
                            {
                                if (fouritem is TextEdit)
                                {
                                    fouritem.Text = "";
                                }
                                if (fouritem is CheckEdit)
                                {
                                    (fouritem as CheckEdit).Checked = false;
                                }
                                if (fouritem is DateEdit)
                                {
                                    fouritem.Text = "";
                                }
                                if (fouritem is LookUpEdit)
                                {
                                    (fouritem as LookUpEdit).EditValue = "";
                                }
                            }

                        }

                    }
                }
            }
        }

        //public void DoClear(Panel m_panel)
        //{
        //    foreach (Control item in m_panel.Controls)
        //    {
        //        if (item is Panel)
        //        {
        //            DoClear(m_panel);
        //        }
        //        else if (item is TextEdit)
        //        {
        //            item.Text = "";
        //        }
        //        else if (item is CheckEdit)
        //        {
        //            (item as CheckEdit).Checked = false;
        //        }
        //        else if (item is DateEdit)
        //        {
        //            item.Text = "";
        //        }
        //        else if (item is LookUpEdit)
        //        {
        //            (item as LookUpEdit).EditValue = "";
        //        }
        //    }
        //}


        /// <summary>
        /// 保存方法
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            try
            {
                if (m_CardiovasularEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条病人信息或补录心脑血管病发病报告卡信息");
                    return false;
                }
                string m_ReportId = m_CardiovasularEntity.ReportID.ToString();
                //新增操作
                if (string.IsNullOrEmpty(m_ReportId))
                {
                    CardiovasularEntity m_cardEntity = GetEntityUI();
                    if (m_cardEntity == null)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("报告卡实体为空");
                        return false;
                    }
                    m_cardEntity.CREATE_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
                    m_cardEntity.CREATE_DEPTCODE = m_Host.User.CurrentDeptId;
                    m_cardEntity.CREATE_DEPTNAME = m_Host.User.CurrentDeptName;
                    m_cardEntity.CREATE_USERCODE = m_Host.User.Id;
                    m_cardEntity.CREATE_USERNAME = m_Host.User.Name;
                    m_cardEntity.STATE = "1";
                    m_cardEntity.VAILD = "1";
                    if (SqlHelper == null)
                    {
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    }
                    if (CheckMustInput())
                    {
                        ID = m_SqlHelper.SaveCardiovacular(m_cardEntity);
                        if (!string.IsNullOrEmpty(ID))
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("新增报告卡成功");
                            return true;
                        }
                        else
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("新增失败");
                            return false;
                        }

                    }
                    else
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请核实所有必填项是否填写完整");
                        return false;
                    }


                }
                else
                {
                    string m_ReportID = m_CardiovasularEntity.ReportID.ToString();
                    string m_OldStateId = m_CardiovasularEntity.STATE.ToString();//获得原来的状态字段
                    CardiovasularEntity _CardiovasularEntity = GetEntityUI();  //add jxh
                    if (SqlHelper == null)
                    {
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    }

                    if (CheckMustInput())
                    {
                        CardiovasularEntity m_cardEntityT = new CardiovasularEntity();

                        m_cardEntityT = SqlHelper.GetCardiovasularEntity(m_ReportID);
                        if (m_OldStateId == m_cardEntityT.STATE)
                        {
                            _CardiovasularEntity.ReportID = m_ReportID;
                            m_SqlHelper.EditcardiovascularCard(_CardiovasularEntity);
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("报告卡信息更新成功");
                            return true;
                        }
                    }
                    else
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请核实所有必填项是否填写完整");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息" + ex.Message);
                return false;
            }
        }

        #endregion

        /// <summary>
        /// 审核通过
        /// </summary>
        public bool Approv()
        {
            try
            {
                if (m_CardiovasularEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条心脑血管病上报记录或补录心脑血管病报告信息");
                    return false;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要审核通过吗？", "审核通过", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    CardiovasularEntity _CardiovasularEntity = GetEntityUI();
                    if (_CardiovasularEntity == null)
                        return false;
                    _CardiovasularEntity.STATE = "4";

                    _CardiovasularEntity.AUDIT_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:m:ss");
                    _CardiovasularEntity.AUDIT_DEPTCODE = m_Host.User.CurrentDeptId;
                    _CardiovasularEntity.AUDIT_DEPTNAME = m_Host.User.CurrentDeptName;
                    _CardiovasularEntity.AUDIT_USERCODE = m_Host.User.Id;
                    _CardiovasularEntity.AUDIT_USERNAME = m_Host.User.Name;

                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);
                    try
                    {
                        _CardiovasularEntity.ReportID = m_CardiovasularEntity.ReportID;
                        SqlHelper.EditcardiovascularCard(_CardiovasularEntity);
                        //m_ReportCardEntity = null;
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("审核成功");
                        this.m_CardiovasularEntity.STATE = "4";
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
                if (m_CardiovasularEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条心脑血管病上报记录或补录心脑血管病报告信息");
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
                            //m_CardiovasularEntity.CANCELREASON = unPassReason.PassReason;
                            CardiovasularEntity _ReportCardEntity = GetEntityUI();
                            if (_ReportCardEntity == null)
                                return false;
                            _ReportCardEntity.STATE = "5";

                            if (SqlHelper == null)
                                SqlHelper = new SqlHelper(m_Host.SqlHelper);
                            try
                            {
                                _ReportCardEntity.CANCELREASON = unPassReason.PassReason;
                                _ReportCardEntity.ReportID = m_CardiovasularEntity.ReportID;
                                m_SqlHelper.EditcardiovascularCard(_ReportCardEntity);
                                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("驳回成功");
                                m_CardiovasularEntity.STATE = "5";
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
                if (m_CardiovasularEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return false;
                }

                if (m_CardiovasularEntity.ReportID == "0")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该报告卡上报记录尚未保存或提交，不需要删除");
                    return false;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除吗？", "删除报告卡上报记录", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    CardiovasularEntity _ReportCardEntity = GetEntityUI();
                    if (_ReportCardEntity == null)
                        return false;
                    _ReportCardEntity.STATE = "7";
                    _ReportCardEntity.ReportID = m_CardiovasularEntity.ReportID;
                    if (SqlHelper == null)
                        SqlHelper = new SqlHelper(m_Host.SqlHelper);

                    SqlHelper.EditcardiovascularCard(_ReportCardEntity);
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

        private void dateBIRTHDATE_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //if (m_CardiovasularEntity == null)
                //{
                //    Common.Ctrs.DLG.MessageBox.Show("请选择或新增一条记录");
                //    return;
                //}

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("出错信息为" + ex.Message);
                return;
            }
        }

        private void dateBIRTHDATE_Leave(object sender, EventArgs e)
        {
            if (dateBIRTHDATE.Text != "")
            {
                DateTime age = DateTime.Parse(dateBIRTHDATE.Text);
                int nl = DateTime.Now.Year - age.Year;
                m_CardiovasularEntity.AGE = nl.ToString();
                txtAge.Text = m_CardiovasularEntity.AGE;
            }
        }

    }
}
