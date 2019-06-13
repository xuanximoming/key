using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using YidanSoft.Wordbook;
using YidanSoft.Common.Library;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class IemNewDiagInfoForm : DevExpress.XtraEditors.XtraForm
    {
        private IYidanEmrHost m_App;

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
        public IemNewDiagInfoForm(IYidanEmrHost app)
        {
            InitializeComponent();
            m_App = app;
            InitLookUpEditor();
        }
        private void IemNewDiagInfoForm_Load(object sender, EventArgs e)
        {

#if DEBUG
#else
            HideSbutton();
#endif
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
            else if (chkStatus5.Checked)
            {
                row["Status_Id"] = 5;
                row["Status_Name"] = chkStatus5.Tag.ToString();
            }
            m_DataOper.Rows.Add(row);
            //m_DataOper.AcceptChanges();

        }

        private void FillUI()
        {
            if (m_IemDiagInfo == null || String.IsNullOrEmpty(m_IemDiagInfo.Diagnosis_Code))
                return;
            lueOutDiag.CodeValue = m_IemDiagInfo.Diagnosis_Code;

            if (m_IemDiagInfo.Status_Id == 1)
                chkStatus1.Checked = true;
            if (m_IemDiagInfo.Status_Id == 2)
                chkStatus2.Checked = true;
            if (m_IemDiagInfo.Status_Id == 3)
                chkStatus3.Checked = true;
            if (m_IemDiagInfo.Status_Id == 4)
                chkStatus4.Checked = true;
            if (m_IemDiagInfo.Status_Id == 5)
                chkStatus5.Checked = true;

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
            if (!String.IsNullOrEmpty(this.lueOutDiag.CodeValue))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                m_App.CustomMessageBox.MessageShow("请选择手术编码");
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }

    }
}
