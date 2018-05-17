using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Service;

namespace DrectSoft.Emr.QcManagerNew
{
    public partial class InWardPatRecInfo : UserControl
    {

        IEmrHost m_app;

        public InWardPatRecInfo(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
        }

        public void RefreshData()
        {
            LoadMasterInfo();
        }


        private void LoadMasterInfo()
        {
            gridControl_Master.DataSource = m_app.SqlHelper.ExecuteDataTable(SQLUtil.sql_QueryWardInfo);

        }

        private void LoadDetailInfo(string deptid)
        {
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_QueryWardDetailInfo, deptid));
            gridControl1.DataSource = dt;

            this.labDeptName.Text = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_QueryDeptNameByID, deptid)).Rows[0][0].ToString();

            this.labPatCount.Text = dt.Rows.Count.ToString();

            this.labNoRecord.Text = dt.Select("PATFILES = 0").Count().ToString();

        }

        private void ViewDetail()
        {
            if (gridViewMain.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
            if (foucesRow == null) return;

            if (foucesRow.IsNull("DEPTID")) return;
            string dept = foucesRow["DEPTID"].ToString();
            LoadDetailInfo(dept);

            xtraTabControl1.SelectedTabPage = xtraTabPage_Detail;

        }

        private void LoadPatView()
        {

            if (gridViewDetail.FocusedRowHandle < 0) return;
            DataRow foucesRow = gridViewDetail.GetDataRow(gridViewDetail.FocusedRowHandle);
            if (foucesRow == null) return;

            if (foucesRow.IsNull("NOOFINPAT")) return;

            m_app.ChoosePatient(Convert.ToDecimal(foucesRow["NOOFINPAT"].ToString()));
            if (m_app.CurrentPatientInfo != null)
                m_app.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
        }

        private void btn_ViewDetail_Click(object sender, EventArgs e)
        {
            ViewDetail();
        }

        private void gridViewMain_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gridViewMain.FocusedRowHandle == -1)
                {
                    return;
                }
                ViewDetail();
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_Return_Click(object sender, EventArgs e)
        {
            xtraTabControl1.SelectedTabPage = xtraTabPage_Master;
        }

        private void gridViewDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gridViewDetail.FocusedRowHandle == -1)
                {
                    return;
                }
                LoadPatView();
            }
            catch (Exception ex)
            {

            }

        }

        private void btn_ViewPatRec_Click(object sender, EventArgs e)
        {
            LoadPatView();
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "导出";
            saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                //gridViewCardList.ExportToXls(saveFileDialog.FileName);
                gridViewDetail.ExportToXls(saveFileDialog.FileName, true);

                m_app.CustomMessageBox.MessageShow("导出成功！");
            }
        }

        private void gridViewMain_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridViewDetail_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}
