using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using YidanSoft.FrameWork.WinForm;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Wordbook;
using YidanSoft.Common.Library;
using YidanSoft.Resources;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class QC_DiagOperRecord : DevExpress.XtraEditors.XtraUserControl
    {
        IYidanEmrHost m_app;
        public QC_DiagOperRecord(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            InitSqlWorkBook();
            //m_app.CustomMessageBox.MessageShow("ceshi");
        }

        private void InitSqlWorkBook()
        {
            try
            {
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
                LookUpWindow lookUpWindowInDiag = new LookUpWindow();
                LookUpWindow lookUpWindowOutDiag = new LookUpWindow();
                LookUpWindow lookUpWindowOper = new LookUpWindow();
                lookUpWindowInDiag.SqlHelper = m_app.SqlHelper;
                lookUpWindowOutDiag.SqlHelper = m_app.SqlHelper;
                lookUpWindowOper.SqlHelper = m_app.SqlHelper;
                this.lookUpEditorInDiag.ListWindow = lookUpWindowInDiag;
                this.lookUpEditorOutDiag.ListWindow = lookUpWindowInDiag;
                this.lookUpEditorOper.ListWindow = lookUpWindowOper;

                string sql_diag = "select icd,name,py,wb from diagnosis";
                DataTable diag = m_app.SqlHelper.ExecuteDataTable(sql_diag);
                diag.Columns["ICD"].Caption = "编号";
                diag.Columns["NAME"].Caption = "诊断名称";

                string sql_oper = "select id,name,py,wb from operation";
                DataTable oper = m_app.SqlHelper.ExecuteDataTable(sql_oper);
                oper.Columns["ID"].Caption = "编号";
                oper.Columns["NAME"].Caption = "手术名称";

                Dictionary<string, int> colDiag = new Dictionary<string, int>();
                colDiag.Add("ICD", 80);
                colDiag.Add("NAME", 160);

                Dictionary<string, int> colOper = new Dictionary<string, int>();
                colOper.Add("ID", 80);
                colOper.Add("NAME", 160);

                SqlWordbook inWordBook = new SqlWordbook("inDiag", diag, "ICD", "NAME", colDiag, "ICD//NAME//PY//WB");
                this.lookUpEditorInDiag.SqlWordbook = inWordBook;
                SqlWordbook outWordBook = new SqlWordbook("outDiag", diag, "ICD", "NAME", colDiag, "ICD//NAME//PY//WB");
                this.lookUpEditorOutDiag.SqlWordbook = outWordBook;
                SqlWordbook operWordBook = new SqlWordbook("oper", oper, "ID", "NAME", colOper, "ID//NAME//PY//WB");
                this.lookUpEditorOper.SqlWordbook = operWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            try
            {
                string inDiag = this.lookUpEditorInDiag.CodeValue;
                string outDiag = this.lookUpEditorOutDiag.CodeValue;
                string oper = this.lookUpEditorOper.CodeValue;
                if (this.dateEdit_begin.DateTime > this.dateEdit_end.DateTime)
                {
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("起始时间不能大于结束时间");
                    return;
                }
                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                string end_time = this.dateEdit_end.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";//.AddDays(1)
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_QueryDiagOperRecord, inDiag, outDiag, oper, begin_time, end_time));
                if (dt == null || dt.Rows.Count == 0)
                {
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("没有符合条件的记录");
                    return;
                }
                this.gridControlRecord.DataSource = dt;
                this.labPatCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                this.lookUpEditorInDiag.CodeValue = "";
                this.lookUpEditorOutDiag.CodeValue = "";
                this.lookUpEditorOper.CodeValue = "";
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
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
                    gridControlRecord.ExportToXls(saveFileDialog.FileName, true);

                    m_app.CustomMessageBox.MessageShow("导出成功！");
                }
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }


        private void gridViewRecord_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                YiDanCommon.YD_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
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

        private void QC_DiagOperRecord_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(20, this.Height - 21);
                this.labPatCount.Location = new Point(71, this.Height - 21);
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

        private void gridViewRecord_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gridViewRecord.FocusedRowHandle == -1)
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
