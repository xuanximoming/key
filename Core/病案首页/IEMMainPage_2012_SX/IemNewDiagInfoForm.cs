using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Library;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrectSoft.Core.IEMMainPage
{
    public partial class IemNewDiagInfoForm : DevBaseForm
    {
        private IEmrHost m_App;

        private IemNewDiagInfo m_DataSearch;
        private Iem_Mainpage_Diagnosis m_IemDiagInfo = new Iem_Mainpage_Diagnosis();

        /// <summary>
        /// 诊断信息
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
        /// 诊断类型 1、西医  2、中医
        /// </summary>

        public string DiagnosisType;

        /// <summary>
        /// 西医诊断库
        /// </summary>
        private DataTable Diagnosis
        {
            get
            {
                if (m_Diagnosis == null)
                    m_Diagnosis = GetEditroData(12);
                return m_Diagnosis;
            }
            set
            {
                m_Diagnosis = value;
            }
        }
        private DataTable m_Diagnosis;

        /// <summary>
        /// 中医诊断库
        /// </summary>
        private DataTable DiagnosisOfChinese
        {
            get
            {
                if (m_DiagnosisOfChinese == null)
                    m_DiagnosisOfChinese = GetEditroData(19);
                return m_DiagnosisOfChinese;
            }
            set
            {
                m_DiagnosisOfChinese = value;
            }
        }
        private DataTable m_DiagnosisOfChinese;

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
        private string m_DIAGNAME;
        private string m_STATUSID;//传来的入院病情
        private string m_DIAGTYPE;//传来的中西医的标志
        private string m_ORIEN;//传来的诊断方位
        public IemNewDiagInfoForm()
        {
            InitializeComponent();
        }
        public IemNewDiagInfoForm(IEmrHost app, string operatetype, string diagcode, string dianame, string status, string orien, string diagtype)
            : this()
        {
            try
            {
                m_App = app;
                InitLookUpEditor();
                m_OPETYPE = operatetype;
                m_DIAGCODE = diagcode;
                m_STATUSID = status;
                m_DIAGTYPE = diagtype;
                m_DIAGNAME = dianame;
                m_ORIEN = orien;
                bwj1.Text = dianame;
                BridFormValue(m_OPETYPE, m_DIAGCODE, m_DIAGNAME, m_STATUSID, m_DIAGTYPE, m_ORIEN);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 根据传来的值控制显示 
        /// </summary>
        /// <param name="m_OPETYPE"></param>
        /// <param name="m_DIAGCODE"></param>
        /// <param name="m_STATUSID"></param>
        /// <param name="m_DIAGTYPE"></param>
        private void BridFormValue(string m_OPETYPE, string m_DIAGCODE, string m_DIAGNAME, string m_STATUSID, string m_DIAGTYPE, string m_ORIEN)
        {
            try
            {
                if (m_OPETYPE == "add")
                {
                }
                if (m_OPETYPE == "edit")
                {
                    if (m_DIAGTYPE == "xiyi")
                    {
                        chkDiagType1.Checked = true;
                    }
                    if (m_DIAGTYPE == "zhongyi")
                    {
                        chkDiagType2.Checked = true;
                    }
                    if (!string.IsNullOrEmpty(m_STATUSID))
                    {
                        switch (m_STATUSID)//控制入院病情的选中情况
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
                    }
                    if (!string.IsNullOrEmpty(m_ORIEN))
                    {
                        switch (m_ORIEN)//控制入院病情的选中情况
                        {
                            case "1":
                                chkOrien1.Checked = true;
                                break;
                            case "2":
                                chkOrien2.Checked = true;
                                break;
                            case "3":
                                chkOrien3.Checked = true;
                                break;
                            case "4":
                                chkOrien4.Checked = true;
                                break;
                            default:
                                break;
                        }
                    }

                    this.bwj1.DiaCode = m_DIAGCODE;
                    this.bwj1.DiaValue = m_DIAGNAME;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string DiagType = string.Empty;//诊断类型
        private string GoType = string.Empty;//表明大类别的Type
        private string inputText = string.Empty;//获取文本里面的内容

        private void obj_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_OPETYPE == "DIAG")
                {

                    if (chkDiagType1.Checked)//西医
                    {
                        DiagType = "XIYI";
                        inputText = bwj1.Text.ToString().Trim();
                        GoType = "OUTHOSDIAG";
                        IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, dtXY, GoType, DiagType, inputText);
                        //inputText = string.Empty;
                        if (diagInfo.GetFormResult())
                        {
                            diagInfo.ShowDialog();
                            if (diagInfo.IsClosed)
                            {
                                bwj1.Text = diagInfo.inText;
                                bwj1.DiaCode = diagInfo.inCode;
                                bwj1.DiaValue = diagInfo.inText;
                            }
                        }
                        else
                        {
                            bwj1.DiaCode = diagInfo.inCode;
                            bwj1.DiaValue = diagInfo.inText;
                            bwj1.Multiline = false;
                        }
                    }
                    if (chkDiagType2.Checked)//中医
                    {
                        DiagType = "ZHONGYI";
                        inputText = bwj1.Text.ToString().Trim();
                        GoType = "OUTHOSDIAG";
                        IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, dtZY, GoType, DiagType, inputText);
                        //inputText = string.Empty;
                        if (diagInfo.GetFormResult())
                        {
                            diagInfo.ShowDialog();
                            if (diagInfo.IsClosed)
                            {
                                bwj1.Text = diagInfo.inText;
                                bwj1.DiaCode = diagInfo.inCode;
                                bwj1.DiaValue = diagInfo.inText;
                            }
                        }
                        else
                        {
                            bwj1.DiaCode = diagInfo.inCode;
                            bwj1.DiaValue = diagInfo.inText;
                            bwj1.Multiline = false;
                        }
                    }
                    //GoType = "OUTHOSDIAG";
                    //IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, dtZY, GoType, DiagType, inputText); 
                    ////inputText = string.Empty;
                    //if (diagInfo.GetFormResult())
                    //{
                    //    diagInfo.ShowDialog();
                    //    if (diagInfo.IsClosed)
                    //    {
                    //        bwj1.Text = diagInfo.inText;
                    //        bwj1.DiaCode = diagInfo.inCode;
                    //        bwj1.DiaValue = diagInfo.inText;
                    //    }
                    //}
                    //else
                    //{
                    //    bwj1.DiaCode = diagInfo.inCode;
                    //    bwj1.DiaValue = diagInfo.inText;
                    //    bwj1.Multiline = false;
                    //}
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }



        private void IemNewDiagInfoForm_Load(object sender, EventArgs e)
        {
            GetFormLoadData();
            // this.bwj1.obj.Click += new EventHandler(obj_Click);
#if DEBUG
#else
            HideSbutton();
#endif
            bwj1.Focus();
        }

        private DataTable dtZY = new DataTable();
        private DataTable dtXY = new DataTable();
        public void GetFormLoadData()
        {
            try
            {
                string SqlAllDiagChinese = @"select id icd, name, py, wb from diagnosisofchinese where valid='1' union 
select icdid icd, name, py, wb from diagnosischiothername where valid='1'";

                dtZY = m_App.SqlHelper.ExecuteDataTable(SqlAllDiagChinese, CommandType.Text);

                string SqlAllDiag = @"select py, wb, name, icd from diagnosis where valid='1' union 
select py, wb, name, icdid from diagnosisothername where valid='1'";
                dtXY = m_App.SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void getMZResult()
        {
            try
            {
                if (chkDiagType2.Checked)//中医
                {
                    DiagnosisType = "2";
                    if (!string.IsNullOrEmpty(bwj1.Text.Trim()) == true)
                    {
                        //GetFormLoadData("ZHONGYI");
                        string filter = string.Empty;

                        string NameFilter = " NAME= '{0}'";
                        filter += string.Format(NameFilter, bwj1.Text.Trim());
                        dtZY.DefaultView.RowFilter = filter;

                        int dataResult = dtZY.DefaultView.ToTable().Rows.Count;

                        if (dataResult == 0)
                        {
                            bwj1.DiaValue = bwj1.Text.Trim();
                            bwj1.DiaCode = "";
                        }

                    }
                }
                if (chkDiagType1.Checked)//西医
                {
                    DiagnosisType = "1";
                    if (!string.IsNullOrEmpty(bwj1.Text.Trim()) == true)
                    {
                        //GetFormLoadData("XIYI");
                        string filter = string.Empty;

                        string NameFilter = " NAME= '{0}'";
                        filter += string.Format(NameFilter, bwj1.Text.Trim());
                        dtXY.DefaultView.RowFilter = filter;

                        int dataResult = dtXY.DefaultView.ToTable().Rows.Count;

                        if (dataResult == 0)
                        {
                            bwj1.DiaValue = bwj1.Text.Trim();
                            bwj1.DiaCode = "";
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
        /// 判断是中医还是西医 绑定数据源
        /// </summary>
        private void InitLookUpEditor()
        {
            try
            {
                if (chkDiagType1.Checked)
                {
                    BindLueData(lueOutDiag, Diagnosis);
                    DiagnosisType = "1";
                }
                else if (chkDiagType2.Checked)
                {
                    BindLueData(lueOutDiag, DiagnosisOfChinese);
                    DiagnosisType = "2";
                }
                lueOutDiag.CodeValue = "";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #region 绑定LUE
        private void BindLueData(LookUpEditor lueInfo, DataTable table)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_App.SqlHelper;
            DataTable dataTable = table;

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
            if (!m_DataOper.Columns.Contains("Orien_Id"))
                m_DataOper.Columns.Add("Orien_Id");
            if (!m_DataOper.Columns.Contains("Orien_Name"))
                m_DataOper.Columns.Add("Orien_Name");
            if (!m_DataOper.Columns.Contains("Type"))
                m_DataOper.Columns.Add("Type");
            if (!m_DataOper.Columns.Contains("TypeName"))
                m_DataOper.Columns.Add("TypeName");
            #endregion
            DataRow row = m_DataOper.NewRow();
            row["Diagnosis_Code"] = bwj1.DiaCode; //lueOutDiag.CodeValue;
            row["Diagnosis_Name"] = bwj1.DiaValue;//lueOutDiag.DisplayValue;

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

            //方位
            if (chkOrien1.Checked)
            {
                row["Orien_Id"] = 1;
                row["Orien_Name"] = chkOrien1.Tag.ToString();
            }
            else if (chkOrien2.Checked)
            {
                row["Orien_Id"] = 2;
                row["Orien_Name"] = chkOrien2.Tag.ToString();
            }
            else if (chkOrien3.Checked)
            {
                row["Orien_Id"] = 3;
                row["Orien_Name"] = chkOrien3.Tag.ToString();
            }
            else if (chkOrien4.Checked)
            {
                row["Orien_Id"] = 4;
                row["Orien_Name"] = chkOrien4.Tag.ToString();
            }

            if (chkDiagType1.Checked)
            {
                row["Type"] = "1";
                row["TypeName"] = "西医诊断";
            }
            else if (chkDiagType2.Checked)
            {
                row["Type"] = "2";
                row["TypeName"] = "中医诊断";
            }

            m_DataOper.Rows.Add(row);
            //m_DataOper.AcceptChanges();

        }

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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                getMZResult();

                if (String.IsNullOrEmpty(this.bwj1.Text))
                {
                    m_App.CustomMessageBox.MessageShow("请选择诊断信息");
                    return;

                }
                if (!String.IsNullOrEmpty(this.bwj1.DiaCode))
                {
                    bwj1.Text = "";
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.bwj1.DiaValue = bwj1.Text.Trim();
                    this.bwj1.DiaCode = string.Empty;
                    this.DialogResult = DialogResult.OK;
                    return;
                    //m_App.CustomMessageBox.MessageShow("请选择手术编码");
                }
                //if (string.IsNullOrEmpty(bwj1.Text.Trim()))
                //{
                //    m_App.CustomMessageBox.MessageShow("请输入住院诊断信息");
                //}
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void chkDiagType1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                InitLookUpEditor();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void bwj1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                Button btn = new Button();
                string oldstr = bwj1.Text.Trim();
                //if (bwj1.Text.Trim() == null)
                //{
                //    IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, GoType, DiagType, inputText);
                //    if(diagInfo.
                //    //inputText = string.Empty;
                //}
                if (bwj1.Text.Trim() != null && e.KeyChar == 13)
                {
                    if (chkDiagType1.Checked)//西医
                    {
                        DiagType = "XIYI";
                        inputText = bwj1.Text.Trim();
                        GoType = "OUTHOSDIAG";
                        IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, dtXY, GoType, DiagType, inputText);
                        //inputText = string.Empty;
                        if (diagInfo.GetFormResult())
                        {
                            diagInfo.ShowDialog();
                            if (diagInfo.IsClosed)
                            {

                                bwj1.Text = diagInfo.inText;

                                //bwj1.Enabled = true;

                                bwj1.DiaCode = diagInfo.inCode;
                                bwj1.DiaValue = diagInfo.inText;
                                bwj1.Focus();
                                bwj1.Enabled = true;

                                //bwj1.Focused = true;
                            }
                        }
                        else
                        {
                            bwj1.DiaCode = diagInfo.inCode;
                            bwj1.DiaValue = diagInfo.inText;
                            //bwj1.Multiline = false;
                        }
                    }
                    if (chkDiagType2.Checked)//中医
                    {
                        DiagType = "ZHONGYI";
                        inputText = bwj1.Text.Trim();
                        GoType = "OUTHOSDIAG";
                        IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, dtZY, GoType, DiagType, inputText);
                        //inputText = string.Empty;
                        if (diagInfo.GetFormResult())
                        {
                            diagInfo.ShowDialog();
                            if (diagInfo.IsClosed)
                            {

                                bwj1.Text = diagInfo.inText;

                                //bwj1.Enabled = true;

                                bwj1.DiaCode = diagInfo.inCode;
                                bwj1.DiaValue = diagInfo.inText;
                                bwj1.Focus();
                                bwj1.Enabled = true;

                                //bwj1.Focused = true;
                            }
                        }
                        else
                        {
                            bwj1.DiaCode = diagInfo.inCode;
                            bwj1.DiaValue = diagInfo.inText;
                            //bwj1.Multiline = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        public void diagInfo_FormClosed(object sender, EventArgs e)
        {
            try
            {
                IemNewDiagInfo diagInfo = (IemNewDiagInfo)sender;
                if (diagInfo.IsClosed)
                {
                    bwj1.Text = diagInfo.inText;
                    //bwj1.DiaCode = string.Empty;

                    bwj1.DiaCode = diagInfo.inCode;
                    bwj1.DiaValue = diagInfo.inText;
                    bwj1.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
    }
}