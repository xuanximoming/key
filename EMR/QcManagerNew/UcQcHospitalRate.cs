using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YindanSoft.Emr.QcManagerNew
{
    /// <summary>
    /// 全院质控率
    /// add by ywk 2012年10月23日 09:50:02 
    /// </summary>
    public partial class UcQcHospitalRate : DevExpress.XtraEditors.XtraUserControl
    {
        IYidanEmrHost m_app;
        public UcQcHospitalRate(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }
        public UcQcHospitalRate()
        {
            InitializeComponent();
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
                gridControl1.ExportToXls(saveFileDialog.FileName, true);

                m_app.CustomMessageBox.MessageShow("导出成功！");
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd");
            string end_time = this.dateEdit_end.DateTime.ToString("yyyy-MM-dd");

            DataTable dt = m_app.SqlHelper.ExecuteDataTable(@"select 
                                                                    ROW_NUMBER() OVER(ORDER BY a.id) AS xh,
                                                                    a.qc_rate || '%' qc_rate,a.* from qc_hospital_rate a;");
            gridControl1.DataSource = dt;

        }

        private void UcQcHospitalRate_Load(object sender, EventArgs e)
        {
            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();
        }

        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                YiDanCommon.YD_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gridView1.FocusedRowHandle == -1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
