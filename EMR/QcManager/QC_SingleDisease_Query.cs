using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.QcManager
{
    public partial class QC_SingleDisease_Query : DevBaseForm
    {
        IEmrHost m_app;
        public QC_SingleDisease_Query(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;

            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();
        }

        private void QC_Doctor_Query_Load(object sender, EventArgs e)
        {
            this.dateEdit_begin.Focus();
        }


        private void btn_query_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            try
            {
                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd");
                string end_time = this.dateEdit_end.DateTime.ToString("yyyy-MM-dd");


                DataTable dt = m_app.SqlHelper.ExecuteDataTable("select a.id xh,a.* from qc_SingleDisease a ");
                gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
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

        private void DevButtonImportExcel_Click(object sender, EventArgs e)
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}