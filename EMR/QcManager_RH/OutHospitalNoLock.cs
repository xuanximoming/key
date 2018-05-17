using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Wordbook;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.QcManager
{
    public partial class OutHospitalNoLock : UserControl, IStartPlugIn
    {
        IEmrHost m_app;
        public OutHospitalNoLock(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;

        }

        public void RefreshData()
        {
            BindDataSouce(this.lookUpEditorDepartment.CodeValue);
        }

        //初始化科室
        private void InitDepartment()
        {

            lookUpWindowDepartment.SqlHelper = m_app.SqlHelper;

            //yxy 暂时加载TP科室
            //DataTable Dept = m_app.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
            //     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);

            string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");
            DataTable Dept = m_app.SqlHelper.ExecuteDataTable(sql);

            Dept.Columns["ID"].Caption = "科室代码";
            Dept.Columns["NAME"].Caption = "科室名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 65);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols,"ID//Name//PY//WB");
            lookUpEditorDepartment.SqlWordbook = deptWordBook;


        }

        private void OutHospitalNoSubmit_Load(object sender, EventArgs e)
        {
            InitDepartment();
        }

        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            throw new NotImplementedException();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {

            BindDataSouce(this.lookUpEditorDepartment.CodeValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BindDataSouce(string deptid)
        {
            if (deptid == null || deptid =="")
                deptid = "*";
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_QueryOutHospitalNOSubmit, deptid));
            gridControl1.DataSource = dt;

            labPatCount.Text = dt.Rows.Count.ToString();
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
