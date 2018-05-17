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
    /// /单病种分析
    /// add by ywk 
    /// </summary>
    public partial class UcSingleDisease : DevExpress.XtraEditors.XtraUserControl
    {
        public UcSingleDisease()
        {
            InitializeComponent();
        }
        IYidanEmrHost m_app;
        public UcSingleDisease(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;

            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            BindData();
        }
        private void BindData()
        {
            string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd");
            string end_time = this.dateEdit_end.DateTime.ToString("yyyy-MM-dd");


            DataTable dt = m_app.SqlHelper.ExecuteDataTable("select a.id xh,a.* from qc_SingleDisease a ");
            gridControl1.DataSource = dt;

            this.labPatCount.Text = dt.Rows.Count.ToString();
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
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

        private void UcSingleDisease_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(16, this.Height - 22);
                this.labPatCount.Location = new Point(67, this.Height - 22);
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
