﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Wordbook;
using YiDanCommon.Ctrs.FORM;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class QC_Rescue_Info : DevBaseForm
    {
        IYidanEmrHost m_app;
        public QC_Rescue_Info(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;

            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();
        }

        private void QC_Doctor_Query_Load(object sender, EventArgs e)
        {
            try
            {
                InitDepartment();
                this.lookUpEditorDepartment.Focus();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }

        //初始化科室
        private void InitDepartment()
        {
            try
            {

                lookUpWindowDepartment.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");
                DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 65);
                cols.Add("NAME", 160);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;


            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
            }
        }

        private void BindData()
        {
            try
            {
                string dept_id = this.lookUpEditorDepartment.CodeValue.Trim();

                string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd");
                string end_time = this.dateEdit_end.DateTime.ToString("yyyy-MM-dd");

                DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(@"select ROW_NUMBER() OVER(ORDER BY a.outbed ASC) AS xh,
                                                                                    a.patid,
                                                                                    a.name,
                                                                                    (case
                                                                                        when a.SexID = 1 then
                                                                                        '男'
                                                                                        when a.SexID = 2 then
                                                                                        '女'
                                                                                        else
                                                                                        '未知'
                                                                                    end) as PATSEX,
                                                                                    a.agestr,
                                                                                    diag.name diag_name,
                                                                                    a.admitdate,
                                                                                    a.outhosdate,
                                                                                    a.address,
                                                                                    datediff('dd',
                                                                                            a.admitdate,
                                                                                            NVL(trim(a.outwarddate), TO_CHAR(SYSDATE, 'yyyy-mm-dd'))) INCOUNT,
                                                                                    '1' qjcs,
                                                                                    '1' CGCS,
                                                                                    '' lx,
                                                                                    '未结算' fee,
                                                                                    dept.name dept_name,
                                                                                    u.name doc_name
                                                                                from inpatient a
                                                                                left join diagnosis diag
                                                                                on a.AdmitDiagnosis = diag.markid
                                                                                left join department dept
                                                                                on a.outhosdept = dept.id
                                                                                left join doctor_assignpatient doc
                                                                                on doc.noofinpat = a.noofinpat
                                                                                left join users u
                                                                                on doc.id = u.id
                                                                where a.outhosdept = '{0}' or '{0}' is null and rownum <8", dept_id));
                gridControl1.DataSource = dt;
            }
            catch (Exception)
            {
                throw;
            }
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
                this.lookUpEditorDepartment.CodeValue = "";
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1,ex);
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

        private void yiDanDevButtonImportExcel_Click(object sender, EventArgs e)
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
    }
}