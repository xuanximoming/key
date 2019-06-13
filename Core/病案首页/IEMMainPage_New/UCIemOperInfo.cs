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
    public partial class UCIemOperInfo : UserControl
    {
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
        public UCIemOperInfo()
        {
            InitializeComponent();

        }

        private void UCIemOperInfo_Load(object sender, EventArgs e)
        {
        }
 
 
        public void FillUI(IemMainPageInfo info, IYidanEmrHost app)
        {
            m_App = app;
            m_IemInfo = info;

            //(new FillUIDelegate(FillUIInner)).BeginInvoke(null, null);
            FillUIInner();
        }
        
        delegate void FillUIDelegate();
        private void FillUIInner()
        {
            #region
            if (m_IemInfo.IemBasicInfo.Iem_Mainpage_NO == "")
            {
                //to do 病患基本信息
            }
            else
            {

                //手术
                //DataTable dataTableOper = new DataTable();
                //foreach (Iem_MainPage_Operation im in m_IemInfo.IemOperInfo)
                //{
                //    if (m_OperInfoFrom == null)
                //        m_OperInfoFrom = new IemNewOperInfo(m_App);
                //    m_OperInfoFrom.IemOperInfo = im;
                //    DataTable dataTable = m_OperInfoFrom.DataOper;
                //    if (dataTableOper.Rows.Count == 0)
                //        dataTableOper = dataTable.Clone();
                //    foreach (DataRow row in dataTable.Rows)
                //    {
                //        dataTableOper.ImportRow(row);
                //    }
                //    dataTableOper.AcceptChanges();
                //}
                //this.gridControl1.DataSource = dataTableOper;
                this.gridControl1.DataSource = null;
                this.gridControl1.DataSource = m_IemInfo.IemOperInfo.Operation_Table;

                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);

                //txtXRay.Text = m_IemInfo.IemBasicInfo.Xay_Sn;
                //txtCT.Text = m_IemInfo.IemBasicInfo.Ct_Sn;
                //txtMri.Text = m_IemInfo.IemBasicInfo.Mri_Sn;
                //txtDsa.Text = m_IemInfo.IemBasicInfo.Dsa_Sn;

                //gridControl2.EndUpdate();
                //gridControl3.EndUpdate();
            }
            #endregion
        }


        /// <summary>
        /// GET UI
        /// </summary>
        private void GetUI()
        {
            if (this.gridControl1.DataSource != null)
            {
                //手术
                
                DataTable dtOperation = m_IemInfo.IemOperInfo.Operation_Table.Clone() ;
                dtOperation.Rows.Clear();

                DataTable dataTable = this.gridControl1.DataSource as DataTable;
                foreach (DataRow row in dataTable.Rows)
                {
                    DataRow imOut = dtOperation.NewRow();
                    //Iem_MainPage_Operation imOut = new Iem_MainPage_Operation();

                    imOut["Operation_Code"] = row["Operation_Code"].ToString();
                    imOut["Operation_Name"] = row["Operation_Name"].ToString();
                    imOut["Operation_Date"] = row["Operation_Date"].ToString();
                    imOut["Execute_User1"] = row["Execute_User1"].ToString();
                    imOut["Execute_User1_Name"] = row["Execute_User1_Name"];
                    imOut["Execute_User2"] = row["Execute_User2"].ToString();
                    imOut["Execute_User2_Name"] = row["Execute_User2_Name"].ToString();
                    imOut["Execute_User3"] = row["Execute_User3"].ToString();
                    imOut["Execute_User3_Name"] = row["Execute_User3_Name"].ToString();
                    imOut["Anaesthesia_Type_Id"] = row["Anaesthesia_Type_Id"].ToString();
                    imOut["Anaesthesia_Type_Name"] = row["Anaesthesia_Type_Name"].ToString();
                    imOut["Close_Level"] = row["Close_Level"].ToString();
                    imOut["Close_Level_Name"] = row["Close_Level_Name"].ToString();
                    imOut["Anaesthesia_User"] = row["Anaesthesia_User"].ToString();
                    imOut["Anaesthesia_User_Name"] = row["Anaesthesia_User_Name"].ToString();
                    dtOperation.Rows.Add(imOut);
                }

                m_IemInfo.IemOperInfo.Operation_Table = dtOperation;

            }
         
        }
 
        IemNewOperInfo m_OperInfoFrom = null;
        private void btnAddDiagnose_Click(object sender, EventArgs e)
        {
            if (m_OperInfoFrom == null)
                m_OperInfoFrom = new IemNewOperInfo(m_App);
            m_OperInfoFrom.ShowDialog();
            if (m_OperInfoFrom.DialogResult == DialogResult.OK)
            {
                m_OperInfoFrom.IemOperInfo = null;
                DataTable dataTable = m_OperInfoFrom.DataOper;


                DataTable dataTableOper = new DataTable();
                if (this.gridControl1.DataSource != null)
                    dataTableOper = this.gridControl1.DataSource as DataTable;
                if (dataTableOper.Rows.Count == 0)
                    dataTableOper = dataTable.Clone();
                foreach (DataRow row in dataTable.Rows)
                {
                    dataTableOper.ImportRow(row);
                }
                gridControl1.BeginUpdate();
                this.gridControl1.DataSource = dataTableOper;
                
                gridControl1.EndUpdate();
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
            }
        }

        private void UCIemOperInfo_Paint(object sender, PaintEventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is LabelControl)
                {
                    control.Visible = false;
                    e.Graphics.DrawString(control.Text, control.Font, Brushes.Black, control.Location);

                }

                if (control is TextEdit)
                {
                    e.Graphics.DrawLine(Pens.Black, new Point(control.Location.X, control.Location.Y + control.Height),
                        new Point(control.Width + control.Location.X, control.Height + control.Location.Y));
                }
            }

            //e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(0, this.Height));
            //e.Graphics.DrawLine(Pens.Black, new Point(this.Width - 1, 0), new Point(this.Width - 1, this.Height));
        }

        private void btn_del_Operinfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridViewOper.FocusedRowHandle < 0)
                return;
            else
            {

                DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                if (dataRow == null)
                    return;

                DataTable dataTableOper = this.gridControl1.DataSource as DataTable;

                dataTableOper.Rows.Remove(dataRow);

                this.gridControl1.BeginUpdate();
                this.gridControl1.DataSource = dataTableOper;
                m_App.PublicMethod.ConvertGridDataSourceUpper(gridViewOper);
                this.gridControl1.EndUpdate();

            }
        }

        private void barManager1_QueryShowPopupMenu(object sender, DevExpress.XtraBars.QueryShowPopupMenuEventArgs e)
        {
            if (e.Control == this.gridControl1)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void gridControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (gridViewOper.FocusedRowHandle < 0)
                    return;
                else
                {
                    DataRow dataRow = gridViewOper.GetDataRow(gridViewOper.FocusedRowHandle);
                    this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    this.btn_operafter_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    this.btn_del_operbefore_diag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    this.btn_del_Operinfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
            }
        }
 

        private void btn_OK_Click(object sender, EventArgs e)
        {
            GetUI();
            ((ShowUC)this.Parent).Close(true, m_IemInfo);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            ((ShowUC)this.Parent).Close(false, null);
        }
    }
}
