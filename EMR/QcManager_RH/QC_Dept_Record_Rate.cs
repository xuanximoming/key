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

namespace DrectSoft.Emr.QcManager
{
    public partial class QC_Dept_Record_Rate : DevExpress.XtraEditors.XtraForm
    {
        IEmrHost m_app;
        public QC_Dept_Record_Rate(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        private void QC_Doctor_Query_Load(object sender, EventArgs e)
        {
            InitDepartment();
        }

        //初始化科室
        private void InitDepartment()
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

        private void btn_query_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {

            string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd");
            string end_time = this.dateEdit_end.DateTime.ToString("yyyy-MM-dd");

            DataTable dt = m_app.SqlHelper.ExecuteDataTable("select ROW_NUMBER() OVER(ORDER BY a.id) AS xh, a.qc_rate || '%' qc_rate,a.* from qc_doc_record_rate a;");
            gridControl1.DataSource = dt;

 


        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "导出Excel";
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
    }
}