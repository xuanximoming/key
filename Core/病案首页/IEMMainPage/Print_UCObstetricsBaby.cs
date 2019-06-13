using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YidanSoft.Common.Library;
using YidanSoft.Wordbook;
using System.Data.SqlClient;
//

using Convertmy = YidanSoft.Core.UtilsForExtension;
using YidanSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraEditors;

namespace YidanSoft.Core.IEMMainPage
{
    public partial class Print_UCObstetricsBaby : UserControl
    {
        IDataAccess m_SqlHelper;
        IYidanSoftLog m_Logger;
        private IemMainPageInfo m_IemInfo;
        /// <summary>
        /// 病案首页产科产妇婴儿信息
        /// </summary>
        public IemMainPageInfo IemInfo
        {
            get
            {
                m_IemInfo = new IemMainPageInfo();
                return m_IemInfo;
            }
        }
    
        private IYidanEmrHost m_App;

        private DataTable m_DataTableDiag = null;
        public Print_UCObstetricsBaby(IemMainPageInfo info, IYidanEmrHost app)
        {
            InitializeComponent();
            //m_IemInfo = info;
            //m_App = app;

            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();
        }

        private void Print_UCIemOperInfo_Load(object sender, EventArgs e)
        {
            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();

            //FillUI();
        }

        private void InitLookUpEditor()
        {
            InitLueDiagnose();
        }

        private void InitLueDiagnose()
        {
            //BindLueData(lueBefore, 12);
            //BindLueData(lueAfter, 12);
        }

        public void FillUI()
        {
            //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
            FillUIInner();
        }
        
        delegate void FillUIDelegate();
        private void FillUIInner()
        {
            #region
            //if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == 0)
            //{
            //    //to do 病患基本信息
            //}
            //else
            //{
            //    //诊断，通过VALUECHANGED时间往GRID里加
            //    gridControl2.BeginUpdate();
            //    gridControl3.BeginUpdate();

            //    gridControl2.DataSource = null;
            //    gridControl3.DataSource = null;
            //    foreach (Iem_Mainpage_Diagnosis im in m_IemInfo.IemDiagInfo)
            //    {
            //        //术前诊断
            //        if (im.Diagnosis_Type_Id == 9)
            //            this.lueBefore.CodeValue = im.Diagnosis_Code;
            //        //术后诊断
            //        else if (im.Diagnosis_Type_Id == 10)
            //            this.lueAfter.CodeValue = im.Diagnosis_Code;
            //    }

            //    //手术
            //    DataTable dataTableOper = new DataTable();
            //    foreach (Iem_MainPage_Operation im in m_IemInfo.IemOperInfo)
            //    {
            //        if (m_OperInfoFrom == null)
            //            m_OperInfoFrom = new IemNewOperInfo(m_App);
            //        m_OperInfoFrom.IemOperInfo = im;
            //        DataTable dataTable = m_OperInfoFrom.DataOper;
            //        if (dataTableOper.Rows.Count == 0)
            //            dataTableOper = dataTable.Clone();
            //        foreach (DataRow row in dataTable.Rows)
            //        {
            //            dataTableOper.ImportRow(row);
            //        }
            //        dataTableOper.AcceptChanges();

            //    }
                //this.gridControl1.DataSource = dataTableOper;


                //txtXRay.Text = m_IemInfo.IemBasicInfo.Xay_Sn;
                //txtCT.Text = m_IemInfo.IemBasicInfo.Ct_Sn;
                //txtMri.Text = m_IemInfo.IemBasicInfo.Mri_Sn;
                //txtDsa.Text = m_IemInfo.IemBasicInfo.Dsa_Sn;

                //gridControl2.EndUpdate();
                //gridControl3.EndUpdate();
            //}
            #endregion
        }




        #region private methods

        private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        {
            LookUpWindow lupInfo = new LookUpWindow();
            lupInfo.SqlHelper = m_SqlHelper;
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
            DataTable dataTable = AddTableColumn(m_SqlHelper.ExecuteDataTable("usp_GetLookUpEditorData", paramCollection, CommandType.StoredProcedure));
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

        /// <summary>
        /// 隐藏lue的S BUTTON
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
        #endregion

 }
}
