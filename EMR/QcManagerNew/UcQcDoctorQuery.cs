using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Wordbook;

namespace YindanSoft.Emr.QcManagerNew
{
    /// <summary>
    /// 医师医疗质量统计
    /// </summary>
    public partial class UcQcDoctorQuery : DevExpress.XtraEditors.XtraUserControl
    {
        IYidanEmrHost m_app;
        public UcQcDoctorQuery(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;

            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();
        }
        public UcQcDoctorQuery()
        {
            InitializeComponent();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
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

        private void btn_query_Click(object sender, EventArgs e)
        {
            BindData();
        }
        private void BindData()
        {
            string deptid = this.lookUpEditorDepartment.CodeValue;
            string begin_time = this.dateEdit_begin.DateTime.ToString("yyyy-MM-dd");
            string end_time = this.dateEdit_end.DateTime.ToString("yyyy-MM-dd");

            if (deptid == null || deptid == "")
                deptid = "*";
            DataTable dt = m_app.SqlHelper.ExecuteDataTable("select a.id xh,a.* from qc_doctor a ");
            gridControl1.DataSource = dt;
            this.labPatCount.Text = dt.Rows.Count.ToString();



        }
        private void UcQcDoctorQuery_Load(object sender, EventArgs e)
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

            cols.Add("ID", 70);
            cols.Add("NAME", 80);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorDepartment.SqlWordbook = deptWordBook;

            //lookUpEditorDepartment.SelectedText = "妇科";
            lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;        //王冀  2013  1  4  初始值设置为用户所在科室
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

        private void UcQcDoctorQuery_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(21, this.Height - 23);
                this.labPatCount.Location = new Point(72, this.Height - 23);
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
