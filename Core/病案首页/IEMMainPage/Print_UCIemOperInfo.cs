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
    public partial class Print_UCIemOperInfo : UserControl
    {
        IDataAccess m_SqlHelper;
        IYidanSoftLog m_Logger;
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

        private IYidanEmrHost m_App;

        private DataTable m_DataTableDiag = null;
        public Print_UCIemOperInfo(IemMainPageInfo info, IYidanEmrHost app)
        {
            InitializeComponent();
            //m_IemInfo = info;
            //m_App = app;

            //m_SqlHelper = DataAccessFactory.DefaultDataAccess;
            //InitLookUpEditor();
        }

        private void Print_UCIemOperInfo_Load(object sender, EventArgs e)
        {
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
            GetUI();
        }



        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {


            //手术
            IemNewOperInfo m_OperInfoFrom = null ;
            DataTable dataTableOper = new DataTable();
            foreach (Iem_MainPage_Operation im in m_IemInfo.IemOperInfo)
            {
                if (m_OperInfoFrom == null)
                    m_OperInfoFrom = new IemNewOperInfo(m_App);
                m_OperInfoFrom.IemOperInfo = im;
                DataTable dataTable = m_OperInfoFrom.DataOper;
                if (dataTableOper.Rows.Count == 0)
                    dataTableOper = dataTable.Clone();
                foreach (DataRow row in dataTable.Rows)
                {
                    dataTableOper.ImportRow(row);
                }
                dataTableOper.AcceptChanges();

            }
            int i = 0;
            //循环加载4行手术信息，多余手术信息不加载
            foreach (DataRow dr in dataTableOper.Rows)
            {
                if (i == 0)
                {
                    labOperation_Code1.Text = dr["Operation_Code"].ToString();
                    labOperation_Date1.Text = dr["Operation_Date"].ToString();
                    labOperation_Name1.Text = dr["Operation_Name"].ToString();
                    labExecute_User1_Name1.Text = dr["Execute_User1_Name"].ToString();
                    labExecute_User2_Name1.Text = dr["Execute_User2_Name"].ToString();
                    labExecute_User3_Name1.Text = dr["Execute_User3_Name"].ToString();
                    labAnaesthesia_Type_Name1.Text = dr["Anaesthesia_Type_Name"].ToString();
                    labClose_Level_Name1.Text = dr["Close_Level_Name"].ToString();
                    labAnaesthesia_User_Name1.Text = dr["Anaesthesia_User_Name"].ToString();

                    labOperation_Code1.Visible = true;
                    labOperation_Date1.Visible = true;
                    labOperation_Name1.Visible = true;
                    labExecute_User1_Name1.Visible = true;
                    labExecute_User2_Name1.Visible = true;
                    labExecute_User3_Name1.Visible = true;
                    labAnaesthesia_Type_Name1.Visible = true;
                    labClose_Level_Name1.Visible = true;
                    labAnaesthesia_User_Name1.Visible = true;
                }
                else if (i == 1)
                {
                    labOperation_Code2.Text = dr["Operation_Code"].ToString();
                    labOperation_Date2.Text = dr["Operation_Date"].ToString();
                    labOperation_Name2.Text = dr["Operation_Name"].ToString();
                    labExecute_User1_Name2.Text = dr["Execute_User1_Name"].ToString();
                    labExecute_User2_Name2.Text = dr["Execute_User2_Name"].ToString();
                    labExecute_User3_Name2.Text = dr["Execute_User3_Name"].ToString();
                    labAnaesthesia_Type_Name2.Text = dr["Anaesthesia_Type_Name"].ToString();
                    labClose_Level_Name2.Text = dr["Close_Level_Name"].ToString();
                    labAnaesthesia_User_Name2.Text = dr["Anaesthesia_User_Name"].ToString();

                    labOperation_Code2.Visible = true;
                    labOperation_Date2.Visible = true;
                    labOperation_Name2.Visible = true;
                    labExecute_User1_Name2.Visible = true;
                    labExecute_User2_Name2.Visible = true;
                    labExecute_User3_Name2.Visible = true;
                    labAnaesthesia_Type_Name2.Visible = true;
                    labClose_Level_Name2.Visible = true;
                    labAnaesthesia_User_Name2.Visible = true;
                }
                else if (i == 2)
                {
                    labOperation_Code3.Text = dr["Operation_Code"].ToString();
                    labOperation_Date3.Text = dr["Operation_Date"].ToString();
                    labOperation_Name3.Text = dr["Operation_Name"].ToString();
                    labExecute_User1_Name3.Text = dr["Execute_User1_Name"].ToString();
                    labExecute_User2_Name3.Text = dr["Execute_User2_Name"].ToString();
                    labExecute_User3_Name3.Text = dr["Execute_User3_Name"].ToString();
                    labAnaesthesia_Type_Name3.Text = dr["Anaesthesia_Type_Name"].ToString();
                    labClose_Level_Name3.Text = dr["Close_Level_Name"].ToString();
                    labAnaesthesia_User_Name3.Text = dr["Anaesthesia_User_Name"].ToString();

                    labOperation_Code3.Visible = true;
                    labOperation_Date3.Visible = true;
                    labOperation_Name3.Visible = true;
                    labExecute_User1_Name3.Visible = true;
                    labExecute_User2_Name3.Visible = true;
                    labExecute_User3_Name3.Visible = true;
                    labAnaesthesia_Type_Name3.Visible = true;
                    labClose_Level_Name3.Visible = true;
                    labAnaesthesia_User_Name3.Visible = true;
                }
                else if (i == 3)
                {
                    labOperation_Code4.Text = dr["Operation_Code"].ToString();
                    labOperation_Date4.Text = dr["Operation_Date"].ToString();
                    labOperation_Name4.Text = dr["Operation_Name"].ToString();
                    labExecute_User1_Name4.Text = dr["Execute_User1_Name"].ToString();
                    labExecute_User2_Name4.Text = dr["Execute_User2_Name"].ToString();
                    labExecute_User3_Name4.Text = dr["Execute_User3_Name"].ToString();
                    labAnaesthesia_Type_Name4.Text = dr["Anaesthesia_Type_Name"].ToString();
                    labClose_Level_Name4.Text = dr["Close_Level_Name"].ToString();
                    labAnaesthesia_User_Name4.Text = dr["Anaesthesia_User_Name"].ToString();

                    labOperation_Code4.Visible = true;
                    labOperation_Date4.Visible = true;
                    labOperation_Name4.Visible = true;
                    labExecute_User1_Name4.Visible = true;
                    labExecute_User2_Name4.Visible = true;
                    labExecute_User3_Name4.Visible = true;
                    labAnaesthesia_Type_Name4.Visible = true;
                    labClose_Level_Name4.Visible = true;
                    labAnaesthesia_User_Name4.Visible = true;
                }
                else
                {
                    return;
                }

                i=i+1;
            }
            //foreach (Iem_MainPage_Operation oper in m_IemInfo.IemOperInfo )
            //{
            //    labOperation_Code1.Text = oper.Operation_Code;
            //        labOperation_Date1.Text = oper.Operation_Date;
            //        labOperation_Name1.Text = oper.Operation_Name;
            //            labExecute_User1_Name1.Text = oper.Execute_User1;
            //            labExecute_User2_Name1.Text = oper.Execute_User2;
            //                labExecute_User3_Name1.Text = oper.Execute_User3;
            //                labAnaesthesia_Type_Name1.Text = oper.Anaesthesia_Type_Id.;
            //                    labClose_Level_Name1.Text = oper.Close_Level.
            //                    labAnaesthesia_User_Name1
            //}

            //foreach (DataRow row in dataTable.Rows)
            //{

            //    Iem_MainPage_Operation imOut = new Iem_MainPage_Operation();

            //    imOut.Operation_Code = row["Operation_Code"].ToString();
            //    imOut.Operation_Name = row["Operation_Name"].ToString();
            //    imOut.Operation_Date = row["Operation_Date"].ToString();
            //    imOut.Execute_User1 = row["Execute_User1"].ToString();
            //    //imOut.Execute_User1_Name = row["Execute_User1_Name"];
            //    imOut.Execute_User2 = row["Execute_User2"].ToString();
            //    //imOut.Execute_User2_Name = row["Execute_User2_Name"];
            //    imOut.Execute_User3 = row["Execute_User3"].ToString();
            //    //imOut.Execute_User3_Name = row["Execute_User3_Name"];
            //    imOut.Anaesthesia_Type_Id = Convertmy.ToDecimal(row["Anaesthesia_Type_Id"]);
            //    //imOut.Anaesthesia_Type_Name = row["Anaesthesia_Type_Name"];
            //    imOut.Close_Level = Convertmy.ToDecimal(row["Close_Level"]);
            //    //imOut.Close_Level_Name = row["Close_Level_Name"];
            //    imOut.Anaesthesia_User = row["Anaesthesia_User"].ToString();
            //    //imOut.Anaesthesia_User_Name = row["Anaesthesia_User_Name"];
            //    m_IemInfo.IemOperInfo.Add(imOut);
            //}

            //}
            //if (this.gridControl2.DataSource != null)
            //{
            //    //术前诊断
            //    DataTable dataTable = this.gridControl2.DataSource as DataTable;
            //    foreach (DataRow row in dataTable.Rows)
            //    {

            //        Iem_Mainpage_Diagnosis imOut = new Iem_Mainpage_Diagnosis();
            //        imOut.Diagnosis_Code = row["Diagnosis_Type_Id"].ToString();
            //        imOut.Diagnosis_Name = row["Diagnosis_Type_Name"].ToString();
            //        imOut.Diagnosis_Type_Id = 9;
            //        m_IemInfo.IemDiagInfo.Add(imOut);
            //    }
            //}
            //if (this.gridControl3.DataSource != null)
            //{
            //    //术后诊断
            //    DataTable dataTable = this.gridControl3.DataSource as DataTable;
            //    foreach (DataRow row in dataTable.Rows)
            //    {

            //        Iem_Mainpage_Diagnosis imOut = new Iem_Mainpage_Diagnosis();
            //        imOut.Diagnosis_Code = row["Diagnosis_Type_Id"].ToString();
            //        imOut.Diagnosis_Name = row["Diagnosis_Type_Name"].ToString();
            //        imOut.Diagnosis_Type_Id = 10;
            //        m_IemInfo.IemDiagInfo.Add(imOut);
            //    }
            //}

            //m_IemInfo.IemBasicInfo.Xay_Sn = txtXRay.Text;
            //m_IemInfo.IemBasicInfo.Ct_Sn = txtCT.Text;
            //m_IemInfo.IemBasicInfo.Mri_Sn = txtMri.Text;
            //m_IemInfo.IemBasicInfo.Dsa_Sn = txtDsa.Text;
        }


        //#region private methods

        //private void BindLueData(LookUpEditor lueInfo, Decimal queryType)
        //{
        //    LookUpWindow lupInfo = new LookUpWindow();
        //    lupInfo.SqlHelper = m_SqlHelper;
        //    if (m_DataTableDiag == null)
        //        m_DataTableDiag = GetEditroData(queryType);

        //    Dictionary<string, int> columnwidth = new Dictionary<String, Int32>();
        //    columnwidth.Add("名称", lueInfo.Width);
        //    SqlWordbook sqlWordBook = new SqlWordbook("ID", m_DataTableDiag, "ID", "Name", columnwidth, true);

        //    lueInfo.SqlWordbook = sqlWordBook;
        //    lueInfo.ListWindow = lupInfo;
        //}

        ///// <summary>
        ///// 获取lue的数据源
        ///// </summary>
        ///// <param name="queryType"></param>
        ///// <returns></returns>
        //private DataTable GetEditroData(Decimal queryType)
        //{
        //    SqlParameter paraType = new SqlParameter("@QueryType", SqlDbType.Decimal);
        //    paraType.Value = queryType;
        //    SqlParameter[] paramCollection = new SqlParameter[] { paraType };
        //    DataTable dataTable = AddTableColumn(m_SqlHelper.ExecuteDataTable("usp_GetLookUpEditorData", paramCollection, CommandType.StoredProcedure));
        //    return dataTable;
        //}

        ///// <summary>
        ///// 给lue的数据源，新增 名称 栏位
        ///// </summary>
        ///// <param name="dataTable"></param>
        ///// <returns></returns>
        //private DataTable AddTableColumn(DataTable dataTable)
        //{
        //    DataTable dataTableAdd = dataTable;
        //    if (!dataTableAdd.Columns.Contains("名称"))
        //        dataTableAdd.Columns.Add("名称");
        //    foreach (DataRow row in dataTableAdd.Rows)
        //        row["名称"] = row["Name"].ToString();
        //    return dataTableAdd;
        //}

        ///// <summary>
        ///// 隐藏lue的S BUTTON
        ///// </summary>
        //private void HideSbutton()
        //{
        //    foreach (Control ctl in this.Controls)
        //    {
        //        if (ctl.GetType() == typeof(LookUpEditor))
        //            ((LookUpEditor)ctl).ShowSButton = false;
        //        else
        //        {
        //            foreach (Control ct in ctl.Controls)
        //            {
        //                if (ct.GetType() == typeof(LookUpEditor))
        //                    ((LookUpEditor)ct).ShowSButton = false;
        //            }
        //        }
        //    }
        //}
        //#endregion


        /// <summary>
        /// 术前诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void lueBefore_CodeValueChanged(object sender, EventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(lueBefore.CodeValue))
        //    {
        //        DataTable dataTable = this.gridControl2.DataSource as DataTable;
        //        if (dataTable == null)
        //        {
        //            dataTable = new DataTable();
        //            dataTable.Columns.Add("Diagnosis_Type_Id");
        //            dataTable.Columns.Add("Diagnosis_Type_Name");
        //        }
        //        DataRow row = dataTable.NewRow();
        //        row["Diagnosis_Type_Id"] = lueBefore.CodeValue;
        //        row["Diagnosis_Type_Name"] = lueBefore.DisplayValue;
        //        dataTable.Rows.Add(row);
        //        dataTable.AcceptChanges();
        //        gridControl2.BeginUpdate();
        //        this.gridControl2.DataSource = dataTable;
        //        gridControl2.EndUpdate();

        //    }
        //}

        /// <summary>
        /// 术后诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void lueAfter_CodeValueChanged(object sender, EventArgs e)
        //{
        //    if (!String.IsNullOrEmpty(lueAfter.CodeValue))
        //    {
        //        DataTable dataTable = this.gridControl3.DataSource as DataTable;
        //        if (dataTable == null)
        //        {
        //            dataTable = new DataTable();
        //            dataTable.Columns.Add("Diagnosis_Type_Id");
        //            dataTable.Columns.Add("Diagnosis_Type_Name");
        //        }
        //        DataRow row = dataTable.NewRow();
        //        row["Diagnosis_Type_Id"] = lueAfter.CodeValue;
        //        row["Diagnosis_Type_Name"] = lueAfter.DisplayValue;
        //        dataTable.Rows.Add(row);
        //        dataTable.AcceptChanges();
        //        gridControl3.BeginUpdate();
        //        this.gridControl3.DataSource = dataTable;
        //        gridControl3.EndUpdate();

        //    }

        //}

        //IemNewOperInfo m_OperInfoFrom = null;
        //private void btnAddDiagnose_Click(object sender, EventArgs e)
        //{
        //    if (m_OperInfoFrom == null)
        //        m_OperInfoFrom = new IemNewOperInfo(m_App);
        //    m_OperInfoFrom.ShowDialog();
        //    if (m_OperInfoFrom.DialogResult == DialogResult.OK)
        //    {
        //        m_OperInfoFrom.IemOperInfo = null;
        //        DataTable dataTable = m_OperInfoFrom.DataOper;


        //        DataTable dataTableOper = new DataTable();
        //        if (this.gridControl1.DataSource != null)
        //            dataTableOper = this.gridControl1.DataSource as DataTable;
        //        if (dataTableOper.Rows.Count == 0)
        //            dataTableOper = dataTable.Clone();
        //        foreach (DataRow row in dataTable.Rows)
        //        {
        //            dataTableOper.ImportRow(row);
        //        }
        //        gridControl1.BeginUpdate();
        //        this.gridControl1.DataSource = dataTableOper;
        //        gridControl1.EndUpdate();
        //    }
        //}

        //private void UCIemOperInfo_Paint(object sender, PaintEventArgs e)
        //{
        //    foreach (Control control in this.Controls)
        //    {
        //        if (control is TextEdit)
        //        {
        //            e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
        //                new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
        //        }
        //    }

        //    e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
        //    e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
        //}

        //private void btn_del_Operinfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (gridViewOper.FocusedRowHandle < 0)
        //        return;
        //    else
        //    {

        //        DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
        //        if (dataRow == null)
        //            return;

        //        DataTable dataTableOper = this.gridControl1.DataSource as DataTable;

        //        dataTableOper.Rows.Remove(dataRow);

        //        this.gridControl1.BeginUpdate();
        //        this.gridControl1.DataSource = dataTableOper;
        //        this.gridControl1.EndUpdate();

        //    }
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

        ///// <summary>
        ///// 删除术前诊断
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btn_del_operbefore_diag_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (gridViewBefore.FocusedRowHandle < 0)
        //        return;
        //    else
        //    {

        //        DataRow dataRow = gridViewBefore.GetDataRow(gridViewBefore.FocusedRowHandle);
        //        if (dataRow == null)
        //            return;

        //        DataTable dataTableOper = this.gridControl2.DataSource as DataTable;

        //        dataTableOper.Rows.Remove(dataRow);

        //        this.gridControl2.BeginUpdate();
        //        this.gridControl2.DataSource = dataTableOper;
        //        this.gridControl2.EndUpdate();

        //    }
        //}

        ///// <summary>
        ///// 术后
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void btn_operafter_diag_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (gridViewAfter.FocusedRowHandle < 0)
        //        return;
        //    else
        //    {

        //        DataRow dataRow = gridViewAfter.GetDataRow(gridViewAfter.FocusedRowHandle);
        //        if (dataRow == null)
        //            return;

        //        DataTable dataTableOper = this.gridControl3.DataSource as DataTable;

        //        dataTableOper.Rows.Remove(dataRow);

        //        this.gridControl3.BeginUpdate();
        //        this.gridControl3.DataSource = dataTableOper;
        //        this.gridControl3.EndUpdate();

        //    }
        //}

        //private void gridControl2_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (gridViewBefore.FocusedRowHandle < 0)
        //            return;
        //        else
        //        {
        //            DataRow dataRow = gridViewBefore.GetDataRow(gridViewBefore.FocusedRowHandle);
        //            this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
        //            this.btn_operafter_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //            this.btn_del_Operinfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //            this.btn_del_operbefore_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        //        }
        //    }
        //}

        //private void gridControl3_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (gridViewAfter.FocusedRowHandle < 0)
        //            return;
        //        else
        //        {
        //            DataRow dataRow = gridViewAfter.GetDataRow(gridViewAfter.FocusedRowHandle);
        //            this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
        //            this.btn_del_operbefore_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //            this.btn_del_Operinfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //            this.btn_operafter_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
        //        }
        //    }
        //}

    }
}
