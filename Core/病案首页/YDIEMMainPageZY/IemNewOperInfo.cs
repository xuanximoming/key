using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Library;
using DrectSoft.Common.Ctrs.DLG;
using Convertmy = DrectSoft.Core.UtilsForExtension;
using DrectSoft.Wordbook;
using System.Data.SqlClient;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common;

namespace DrectSoft.Core.IEMMainPageZY                       
{
    /// <summary>
    /// 新增或编辑手术记录界面
    /// </summary>
    public partial class IemNewOperInfo :DevBaseForm
    {

        private IEmrHost m_App;

        private Iem_MainPage_Operation m_IemOperInfo = new Iem_MainPage_Operation();

        /// <summary>
        /// 手术信息
        /// </summary>
        public Iem_MainPage_Operation IemOperInfo
        {
            get
            {
                GetUI();
                return m_IemOperInfo;
            }
            set
            {
                m_IemOperInfo = value;
            }
        }

        private string MZDiagType = string.Empty;//诊断类型
        private string GoType = string.Empty;//表明大类别的Type
        private string inputText = string.Empty;//获取文本里面的内容
        private string valueStr = DrectSoft.Service.YD_SqlService.GetConfigValueByKey("MainPageDiagnosisType");//取配置值

        private DataTable dtXY = new DataTable();
        private DataTable dtDoc = new DataTable();

        /// <summary>
        /// 
        /// </summary>
        public DataTable DataOper
        {
            get
            {
                if (m_DataOper == null)
                {
                    m_DataOper = new DataTable();
                }
                GetDataOper();
                return m_DataOper;
            }
            set
            {
                m_DataOper = new DataTable();
                m_DataOper = value.Clone();
                foreach (DataRow Row in value.Rows)
                {
                    m_DataOper.ImportRow(Row);
                }
            }
        }

        private DataTable m_DataOper;

        #region 手术信息界面方法

        /// <summary>
        /// 手术信息窗口
        /// </summary>
        /// <param name="app">应用程序对象接口</param>
        /// <param name="operateType">操作类型（"edit","new"）</param>
        /// <param name="dtOper">"edit"初始数据</param>
        public IemNewOperInfo(IEmrHost app, string operateType, DataTable dtOper)
        {
            try
            {
                InitializeComponent();
                InitControl();   //add by ixh
                m_App = app;
                InitLookUpEditor();
                if (operateType == "edit")
                {
                    this.m_DataOper = dtOper;
                    this.FreshDataByDataOper();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetFocus()
        {
            this.deOperDate.Focus();
        }

        //add by ixh 判断显示哪个控件
        private void InitControl()
        {
            try
            {
                if (valueStr == "0")
                {
                    lueOperCode.Visible = true; 
                    lueOperCode.Visible = true;
                    lueOper.Visible = false;
                    lueOper.Visible = false;
                }
                else
                {
                    lueOperCode.Visible = false;
                    lueOperCode.Visible = false;
                    lueOper.Visible = true;
                    lueOper.Visible = true;
                    lueOper.Left = 87;
                    lueOper.Width = 156;                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetFormLoadData()
        {
            try
            {
                string SqlAllDiag = @"select py, wb, name, ID icd from operation where valid='1'";
                dtXY = m_App.SqlHelper.ExecuteDataTable(SqlAllDiag, CommandType.Text);
                string SqlAllDoctor = @"SELECT py,wb, NAME,ID icd FROM users WHERE valid = 1 ORDER BY icd";
                dtDoc = m_App.SqlHelper.ExecuteDataTable(SqlAllDoctor, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 已注释
        /// </summary>
        private void FillUI()
        {
            //if (m_IemOperInfo == null || String.IsNullOrEmpty(m_IemOperInfo.Operation_Code))
            //    return;
            //lueOperCode.CodeValue = m_IemOperInfo.Operation_Code;
            //if (!String.IsNullOrEmpty(m_IemOperInfo.Operation_Date))
            //{
            //    deOperDate.DateTime = Convert.ToDateTime(m_IemOperInfo.Operation_Date);
            //    teOperDate.Time = Convert.ToDateTime(m_IemOperInfo.Operation_Date);
            //}
            ////lueOperCode.DisplayValue = m_IemOperInfo.Operation_Name;
            //lueExecute1.CodeValue = m_IemOperInfo.Execute_User1;
            //lueExecute2.CodeValue = m_IemOperInfo.Execute_User2;
            //lueExecute3.CodeValue = m_IemOperInfo.Execute_User3;
            //if (m_IemOperInfo.Anaesthesia_Type_Id != null)
            //    lueAnaesthesiaType.CodeValue = m_IemOperInfo.Anaesthesia_Type_Id.ToString();
            //if (m_IemOperInfo.Close_Level != null)
            //    lueCloseLevel.CodeValue = m_IemOperInfo.Close_Level.ToString();
            //lueAnaesthesiaUser.CodeValue = m_IemOperInfo.Anaesthesia_User;
        }

        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }
        private void BindLueOperData(LookUpEditor lueInfo, Decimal queryType)
        {
            try
            {
                LookUpWindow lupInfo = new LookUpWindow();
                lupInfo.SqlHelper = m_App.SqlHelper;
                if (m_DataTableDiag == null)
                    m_DataTableDiag = GetEditroData(queryType);

                Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
                columnwidth.Add("名称", lueInfo.Width);
                SqlWordbook sqlWordBook = new SqlWordbook("ID", m_DataTableDiag, "ID", "Name", columnwidth, true);

                lueInfo.SqlWordbook = sqlWordBook;
                lueInfo.ListWindow = lupInfo;
            }
            catch (Exception)
            {
                throw;
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
                DataTable dataTable = AddTableColumn(m_App.SqlHelper.ExecuteDataTable("usp_GetLookUpEditorData", paramCollection, CommandType.StoredProcedure));
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
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
            catch (Exception)
            {
                throw;
            }
        }

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
        /// 在打开页面时，跟页面数据的匹配，得到匹配的数据结果
        /// </summary>
        private void getOPERResult()
        {
            try
            {
                if (!string.IsNullOrEmpty(lueOperCode.Text.Trim()) == true)
                {
                    //GetFormLoadData("XIYI");
                    string filter = string.Empty;

                    string NameFilter = " NAME= '{0}'";
                    filter += string.Format(NameFilter, lueOperCode.Text.Trim());
                    dtXY.DefaultView.RowFilter = filter;

                    int dataResult = dtXY.DefaultView.ToTable().Rows.Count;

                    if (dataResult > 0)
                    {
                        lueOperCode.DiaValue = lueOperCode.Text.Trim();
                        lueOperCode.DiaCode = dtXY.DefaultView.ToTable().Rows[0]["icd"].ToString();
                        //lueMZXYZD_CODE.DiaCode = dtXY.DefaultView.ToTable().Rows[0][3].ToString();    //dtZY.row["icd"].ToString();

                    }
                    if (dataResult == 0)
                    {
                        lueOperCode.DiaValue = lueOperCode.Text.Trim();
                        lueOperCode.DiaCode = "";
                    }

                }
                if (!string.IsNullOrEmpty(lueAnaesthesiaUser.Text.Trim()) == true)
                {
                    string filter = string.Empty;
                    string NameFilter = "NAME='{0}'";
                    filter += string.Format(NameFilter, lueAnaesthesiaUser.Text.Trim());
                    dtDoc.DefaultView.RowFilter = filter;

                    int dataResult = dtDoc.DefaultView.ToTable().Rows.Count;
                    if (dataResult > 0)
                    {
                        lueAnaesthesiaUser.DiaValue = lueAnaesthesiaUser.Text.Trim();
                        lueAnaesthesiaUser.DiaCode = dtDoc.DefaultView.ToTable().Rows[0]["icd"].ToString();
                    }
                    if (dataResult == 0)
                    {
                        lueAnaesthesiaUser.DiaValue = lueAnaesthesiaUser.Text.Trim();
                        lueAnaesthesiaUser.DiaCode = "";
                    }
                }
                if (string.IsNullOrEmpty(lueAnaesthesiaUser.Text.Trim()) == true)
                {
                    string filter = string.Empty;
                    string NameFilter = "NAME='{0}'";
                    filter += string.Format(NameFilter, lueAnaesthesiaUser.Text.Trim());
                    dtDoc.DefaultView.RowFilter = filter;

                    int dataResult = dtDoc.DefaultView.ToTable().Rows.Count;
                    if (dataResult > 0)
                    {
                        lueAnaesthesiaUser.DiaValue = lueAnaesthesiaUser.Text.Trim();
                        lueAnaesthesiaUser.DiaCode = dtDoc.DefaultView.ToTable().Rows[0]["icd"].ToString();
                    }
                    if (dataResult == 0)
                    {
                        lueAnaesthesiaUser.DiaValue = lueAnaesthesiaUser.Text.Trim();
                        lueAnaesthesiaUser.DiaCode = "";
                    }
                }
                if (string.IsNullOrEmpty(lueOperCode.Text.Trim()) == true)
                {
                    string filter = string.Empty;
                    string NameFilter = "NAME='{0}'";
                    filter += string.Format(NameFilter, lueOperCode.Text.Trim());
                    dtDoc.DefaultView.RowFilter = filter;

                    int dataResult = dtDoc.DefaultView.ToTable().Rows.Count;
                    if (dataResult > 0)
                    {
                        lueOperCode.DiaValue = lueOperCode.Text.Trim();
                        lueOperCode.DiaCode = dtDoc.DefaultView.ToTable().Rows[0]["icd"].ToString();
                    }
                    if (dataResult == 0)
                    {
                        lueOperCode.DiaValue = lueOperCode.Text.Trim();
                        lueOperCode.DiaCode = "";
                    }
                }
            }
            catch (Exception )
            {
                throw;
            }
        }

        private DataTable m_DataTableDiag;

        /// <summary>
        /// 初始化控件  
        /// </summary>
        private void InitLookUpEditor()
        {
            try
            {
                //BindLueData(lueOperCode1, 20);                           王冀  2013 1 12
                BindLueOperData(lueExecute1, 11);
                BindLueOperData(lueExecute2, 11);
                BindLueOperData(lueExecute3, 11);
                BindLueOperData(lueAnaesthesiaUser1, 11);               
                BindLueData(lueCloseLevel, 15);
                BindLueData(lueAnaesthesiaType, 14);
                BindLueData(lueOperlevel, 18);                
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void BindOper()
        {
            LookUpWindow lupInfo1 = new LookUpWindow();
            lupInfo1.SqlHelper = m_App.SqlHelper;           
            DataTable Dept = dtXY;
            Dept.Columns["NAME"].Caption = "名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("ICD", 80);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ICD", "NAME", cols, "ICD//NAME//PY//WB");
            lueOper.SqlWordbook = deptWordBook;

            lueOper.ListWindow = lupInfo1;
        }

        /// <summary>
        /// 已注释
        /// </summary>
        private void GetUI()
        {

            //m_IemOperInfo.Operation_Code = lueOperCode.CodeValue;
            //if (deOperDate.DateTime.CompareTo(DateTime.MinValue) != 0)
            //    m_IemOperInfo.Operation_Date = deOperDate.DateTime.ToShortDateString() + "" + teOperDate.Time.ToShortTimeString();
            //m_IemOperInfo.Operation_Name = lueOperCode.DisplayValue;
            //m_IemOperInfo.Execute_User1 = lueExecute1.CodeValue;
            //m_IemOperInfo.Execute_User2 = lueExecute2.CodeValue;
            //m_IemOperInfo.Execute_User3 = lueExecute3.CodeValue;
            //m_IemOperInfo.Anaesthesia_Type_Id = Convertmy.ToDecimal(lueAnaesthesiaType.CodeValue);
            //m_IemOperInfo.Close_Level = Convertmy.ToDecimal(lueCloseLevel.CodeValue);
            //m_IemOperInfo.Anaesthesia_User = lueAnaesthesiaUser.CodeValue;
        }

        /// <summary>
        /// 将窗口当前操作数据显示到View   edit by wangj  2013 1 12
        /// </summary>
        private void FreshDataByDataOper()
        {
            try
            {
                if (this.m_DataOper.Rows.Count == 0)
                {
                    return;
                }
                DataRow row = m_DataOper.Rows[0];
                //lueOperCode1.CodeValue = row["Operation_Code"].ToString();
                if (valueStr == "0")
                {
                    lueOperCode.DiaCode = row["Operation_Code"].ToString();
                    lueOperCode.Text = row["Operation_Name"].ToString();
                    lueOperCode.DiaValue = row["Operation_Name"].ToString();
                }
                else
                {                    
                    lueOper.Text = row["Operation_Name"].ToString();
                    //lueOper.CodeValue = row["Operation_Name"].ToString();
                }
                    if (row["Operation_Date"].ToString() != "")
                {
                    deOperDate.DateTime = DateTime.Parse(DateTime.Parse(row["Operation_Date"].ToString()).ToShortDateString());
                    teOperDate.Time = DateTime.Parse(DateTime.Parse(row["Operation_Date"].ToString()).ToShortTimeString());
                }
                lueOperlevel.CodeValue = row["operation_level"].ToString();
                lueExecute1.CodeValue = row["Execute_User1"].ToString();
                lueExecute2.CodeValue = row["Execute_User2"].ToString();
                lueExecute3.CodeValue = row["Execute_User3"].ToString();
                lueAnaesthesiaType.CodeValue = row["Anaesthesia_Type_Id"].ToString();
                lueCloseLevel.CodeValue = row["Close_Level"].ToString();
                lueAnaesthesiaUser1.CodeValue = row["Anaesthesia_User"].ToString();
                //lueAnaesthesiaUser.DiaCode = row["Anaesthesia_User"].ToString();
                //lueAnaesthesiaUser.DiaValue = row["Anaesthesia_User_Name"].ToString();
                //lueAnaesthesiaUser.Text = lueAnaesthesiaUser.DiaValue;
                int seconds;
                if (!int.TryParse(row["OperInTimes"].ToString(), out seconds))
                {
                    seconds = 0;
                }
                TimeSpan times = new TimeSpan(0, 0, 0, seconds);
                txtOperInTime.Text = DS_Common.TimeSpanToLocal(times);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetDataOper()
        {
            try
            {
                m_DataOper = new DataTable();
                #region
                if (!m_DataOper.Columns.Contains("Operation_Code"))
                    m_DataOper.Columns.Add("Operation_Code");
                if (!m_DataOper.Columns.Contains("Operation_Date"))
                    m_DataOper.Columns.Add("Operation_Date");
                if (!m_DataOper.Columns.Contains("Operation_Name"))
                    m_DataOper.Columns.Add("Operation_Name");

                if (!m_DataOper.Columns.Contains("operation_level"))
                    m_DataOper.Columns.Add("operation_level");
                if (!m_DataOper.Columns.Contains("operation_level_Name"))
                    m_DataOper.Columns.Add("operation_level_Name");

                if (!m_DataOper.Columns.Contains("Execute_User1"))
                    m_DataOper.Columns.Add("Execute_User1");
                if (!m_DataOper.Columns.Contains("Execute_User1_Name"))
                    m_DataOper.Columns.Add("Execute_User1_Name");

                if (!m_DataOper.Columns.Contains("Execute_User2"))
                    m_DataOper.Columns.Add("Execute_User2");
                if (!m_DataOper.Columns.Contains("Execute_User2_Name"))
                    m_DataOper.Columns.Add("Execute_User2_Name");

                if (!m_DataOper.Columns.Contains("Execute_User3"))
                    m_DataOper.Columns.Add("Execute_User3");
                if (!m_DataOper.Columns.Contains("Execute_User3_Name"))
                    m_DataOper.Columns.Add("Execute_User3_Name");

                if (!m_DataOper.Columns.Contains("Anaesthesia_Type_Id"))
                    m_DataOper.Columns.Add("Anaesthesia_Type_Id");
                if (!m_DataOper.Columns.Contains("Anaesthesia_Type_Name"))
                    m_DataOper.Columns.Add("Anaesthesia_Type_Name");

                if (!m_DataOper.Columns.Contains("Close_Level"))
                    m_DataOper.Columns.Add("Close_Level");
                if (!m_DataOper.Columns.Contains("Close_Level_Name"))
                    m_DataOper.Columns.Add("Close_Level_Name");

                if (!m_DataOper.Columns.Contains("Anaesthesia_User"))
                    m_DataOper.Columns.Add("Anaesthesia_User");
                if (!m_DataOper.Columns.Contains("Anaesthesia_User_Name"))
                    m_DataOper.Columns.Add("Anaesthesia_User_Name");
                if (!m_DataOper.Columns.Contains("OperInTimes"))
                {
                    m_DataOper.Columns.Add("OperInTimes");
                }
                #endregion
                FillUI();
                DataRow row = m_DataOper.NewRow();
                //if (lueOperCode.DiaCode == "非编码手术")
                //{
                //    row["Operation_Code"] = string.Empty; ;
                //}
                //else
                //{
                if (valueStr == "0")
                {
                    row["Operation_Code"] = lueOperCode.DiaCode;//1.CodeValue;
                    //}
                    row["Operation_Name"] = lueOperCode.DiaValue;//1.DisplayValue;
                }
                else
                {
                    row["Operation_Code"] = lueOper.CodeValue;//1.CodeValue;                   
                    row["Operation_Name"] = lueOper.DisplayValue;//1.DisplayValue;
                }
                if (deOperDate.DateTime.CompareTo(DateTime.MinValue) != 0)
                    row["Operation_Date"] = deOperDate.DateTime.ToShortDateString() + " " + teOperDate.Time.ToShortTimeString();

                row["operation_level"] = lueOperlevel.CodeValue;
                row["operation_level_Name"] = lueOperlevel.DisplayValue;

                row["Execute_User1"] = lueExecute1.CodeValue;
                row["Execute_User1_Name"] = lueExecute1.DisplayValue;
                row["Execute_User2"] = lueExecute2.CodeValue;
                row["Execute_User2_Name"] = lueExecute2.DisplayValue;
                row["Execute_User3"] = lueExecute3.CodeValue;
                row["Execute_User3_Name"] = lueExecute3.DisplayValue;
                row["Anaesthesia_Type_Id"] = lueAnaesthesiaType.CodeValue;
                row["Anaesthesia_Type_Name"] = lueAnaesthesiaType.DisplayValue;
                row["Close_Level"] = lueCloseLevel.CodeValue;
                row["Close_Level_Name"] = lueCloseLevel.DisplayValue;
                row["Anaesthesia_User"] = lueAnaesthesiaUser1.CodeValue;//.CodeValue;
                row["Anaesthesia_User_Name"] = lueAnaesthesiaUser1.DisplayValue;//.DisplayValue;
                row["OperInTimes"] =TimesToSeconds(txtOperInTime.Text).ToString();
                m_DataOper.Rows.Add(row);
                //m_DataOper.AcceptChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 时间转换成秒
        /// xlb 2013-01-09
        /// </summary>
        private int TimesToSeconds(string timeRange)
        {
            try
            {
                if (string.IsNullOrEmpty(timeRange))
                {
                    return 0;
                }
                int days = 0, hours = 0, mins = 0;
                string temp = timeRange.Trim();
                int index = temp.IndexOf("天");
                if (index > 0)
                {
                    days = int.Parse(temp.Substring(0, temp.IndexOf("天")));
                    temp = temp.Substring(index + 1, temp.Length - index - 1);
                }
                else
                {
                    days = 0;
                    temp = temp.Substring(index + 1, temp.Length - index - 1);
                }
                if (temp.IndexOf("时") > 0)
                {
                    hours = int.Parse(temp.Substring(0, temp.IndexOf("时")));
                    temp = temp.Substring(temp.IndexOf("时") + 1, temp.Length - temp.IndexOf("时") - 1);
                }
                else
                {
                    hours = 0;
                    temp = temp.Substring(temp.IndexOf("时") + 1, temp.Length - temp.IndexOf("时") - 1);
                }
                if (temp.IndexOf("分") > 0)
                {
                    mins = int.Parse(temp.Substring(0, temp.IndexOf("分")));
                }
                else
                {
                    mins = 0;
                }
                TimeSpan timeSpan = new TimeSpan(days, hours, mins, 0);
                int totalSeconds = (int)timeSpan.TotalSeconds;
                return totalSeconds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region 手术信息界面事件

        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
               MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                getOPERResult();
                //手术信息验证 add by ywk 
                if (valueStr == "0")
                {
                    if (String.IsNullOrEmpty(this.lueOperCode.Text))
                    {
                        m_App.CustomMessageBox.MessageShow("请选择手术信息");
                        return;
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(this.lueOper.Text))
                    {
                        m_App.CustomMessageBox.MessageShow("请选择手术信息");
                        return;
                    }
                }
                if (!String.IsNullOrEmpty(this.lueOperCode.DiaCode))
                {
                    lueOperCode.Text = "";
                }
                else
                {
                    this.lueOperCode.DiaValue = lueOperCode.Text.Trim();

                    this.lueOperCode.DiaCode = "非编码手术";
                }
                if (!String.IsNullOrEmpty(this.lueAnaesthesiaUser.DiaCode))
                {
                    lueAnaesthesiaUser.Text = "";
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.lueAnaesthesiaUser.DiaValue = lueAnaesthesiaUser.Text.Trim();
                    this.lueAnaesthesiaUser.DiaCode = lueAnaesthesiaUser.Text.Trim();
                    this.DialogResult = DialogResult.OK;
                }

                //if (!String.IsNullOrEmpty(this.lueOperCode.DiaCode))
                //{
                //    this.DialogResult = DialogResult.OK;
                //}
                //else
                //{
                //    m_App.CustomMessageBox.MessageShow("请选择手术编码");
                //}
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IemNewOperInfo_Load(object sender, EventArgs e)
        {
            try
            {
                GetFormLoadData();
                BindOper();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 键盘事件
        /// Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueOperCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                
                if (e.KeyChar == 13)//lueOperCode.Text.Trim() != null &&
                {
                    GoType = "operate";
                    MZDiagType = "operate";
                    inputText = lueOperCode.Text.Trim();

                    IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, dtXY, GoType, MZDiagType, inputText);
                    if (diagInfo.GetFormResult())
                    {
                        diagInfo.ShowDialog();
                        if (diagInfo.IsClosed)
                        {
                            lueOperCode.Text = diagInfo.inText;
                            lueOperCode.DiaCode = diagInfo.inCode;
                            lueOperCode.DiaValue = diagInfo.inText;
                        }
                    }
                    else
                    {
                        lueOperCode.DiaCode = diagInfo.inCode;
                        lueOperCode.DiaValue = diagInfo.inText;
                        lueOperCode.Multiline = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueAnaesthesiaUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)//lueOperCode.Text.Trim() != null &&
                {
                    GoType = "operate";
                    MZDiagType = "anaesthetist";
                    inputText = lueAnaesthesiaUser.Text.Trim();

                    IemNewDiagInfo diagInfo = new IemNewDiagInfo(m_App, dtDoc, GoType, MZDiagType, inputText);
                    if (diagInfo.GetFormResult())
                    {
                        diagInfo.ShowDialog();
                        if (diagInfo.IsClosed)
                        {
                            lueAnaesthesiaUser.Text = diagInfo.inText;
                            lueAnaesthesiaUser.DiaCode = diagInfo.inCode;
                            lueAnaesthesiaUser.DiaValue = diagInfo.inText;
                        }
                    }
                    else
                    {
                        lueAnaesthesiaUser.DiaCode = diagInfo.inCode;
                        lueAnaesthesiaUser.DiaValue = diagInfo.inText;
                        lueAnaesthesiaUser.Multiline = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}