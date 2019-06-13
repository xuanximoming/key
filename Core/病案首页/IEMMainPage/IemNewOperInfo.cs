using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Common.Library;

using Convertmy = YidanSoft.Core.UtilsForExtension;
using YidanSoft.Wordbook;
using System.Data.SqlClient;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class IemNewOperInfo : DevExpress.XtraEditors.XtraForm
    {

        private IYidanEmrHost m_App;

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

        public IemNewOperInfo(IYidanEmrHost app)
        {
            InitializeComponent();
            m_App = app;
            InitLookUpEditor();
        }

        private void IemNewOperInfo_Load(object sender, EventArgs e)
        {
#if DEBUG
#else
            HideSbutton();
#endif
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
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.lueOperCode.CodeValue))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                m_App.CustomMessageBox.MessageShow("请选择手术编码");
            }
        }

        private DataTable m_DataTableDiag;
        private void InitLookUpEditor()
        {

            BindLueData(lueOperCode, 12);
            BindLueOperData(lueExecute1, 11);
            BindLueOperData(lueExecute2, 11);
            BindLueOperData(lueExecute3, 11);
            BindLueOperData(lueAnaesthesiaUser, 11);
            BindLueData(lueCloseLevel, 15);
            BindLueData(lueAnaesthesiaType, 14);
        }

        private void GetUI()
        {

            m_IemOperInfo.Operation_Code = lueOperCode.CodeValue;
            if (deOperDate.DateTime.CompareTo(DateTime.MinValue) != 0)
                m_IemOperInfo.Operation_Date = deOperDate.DateTime.ToShortDateString() + "" + teOperDate.Time.ToShortTimeString();
            m_IemOperInfo.Operation_Name = lueOperCode.DisplayValue;
            m_IemOperInfo.Execute_User1 = lueExecute1.CodeValue;
            m_IemOperInfo.Execute_User2 = lueExecute2.CodeValue;
            m_IemOperInfo.Execute_User3 = lueExecute3.CodeValue;
            m_IemOperInfo.Anaesthesia_Type_Id = Convertmy.ToDecimal(lueAnaesthesiaType.CodeValue);
            m_IemOperInfo.Close_Level = Convertmy.ToDecimal(lueCloseLevel.CodeValue);
            m_IemOperInfo.Anaesthesia_User = lueAnaesthesiaUser.CodeValue;
        }

        private void GetDataOper()
        {
            m_DataOper = new DataTable();
            #region
            if (!m_DataOper.Columns.Contains("Operation_Code"))
                m_DataOper.Columns.Add("Operation_Code");
            if (!m_DataOper.Columns.Contains("Operation_Date"))
                m_DataOper.Columns.Add("Operation_Date");
            if (!m_DataOper.Columns.Contains("Operation_Name"))
                m_DataOper.Columns.Add("Operation_Name");

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
            #endregion
            FillUI();
            DataRow row = m_DataOper.NewRow();
            row["Operation_Code"] = lueOperCode.CodeValue;
            row["Operation_Name"] = lueOperCode.DisplayValue;
            if (deOperDate.DateTime.CompareTo(DateTime.MinValue) != 0)
                row["Operation_Date"] = deOperDate.DateTime.ToShortDateString() + " " + teOperDate.Time.ToShortTimeString();
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
            row["Anaesthesia_User"] = lueAnaesthesiaUser.CodeValue;
            row["Anaesthesia_User_Name"] = lueAnaesthesiaUser.DisplayValue;
            m_DataOper.Rows.Add(row);
            //m_DataOper.AcceptChanges();

        }

        private void FillUI()
        {
            if (m_IemOperInfo == null || String.IsNullOrEmpty(m_IemOperInfo.Operation_Code))
                return;
            lueOperCode.CodeValue = m_IemOperInfo.Operation_Code;
            if (!String.IsNullOrEmpty(m_IemOperInfo.Operation_Date))
            {
                deOperDate.DateTime = Convert.ToDateTime(m_IemOperInfo.Operation_Date);
                teOperDate.Time = Convert.ToDateTime(m_IemOperInfo.Operation_Date);
            }
            //lueOperCode.DisplayValue = m_IemOperInfo.Operation_Name;
            lueExecute1.CodeValue = m_IemOperInfo.Execute_User1;
            lueExecute2.CodeValue = m_IemOperInfo.Execute_User2;
            lueExecute3.CodeValue = m_IemOperInfo.Execute_User3;
            if (m_IemOperInfo.Anaesthesia_Type_Id != null)
                lueAnaesthesiaType.CodeValue = m_IemOperInfo.Anaesthesia_Type_Id.ToString();
            if (m_IemOperInfo.Close_Level != null)
                lueCloseLevel.CodeValue = m_IemOperInfo.Close_Level.ToString();
            lueAnaesthesiaUser.CodeValue = m_IemOperInfo.Anaesthesia_User;
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
        private void BindLueOperData(LookUpEditor lueInfo, Decimal queryType)
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

    }
}