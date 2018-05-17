using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Wordbook;
using DrectSoft.Common;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Emr.QcManager
{

    public partial class QC_RecordByDoctor : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_app;

        /// <summary>
        /// <auth>Modify By Xlb</auth>
        /// <date>2013-06-18</date>
        /// </summary>
        /// <param name="app"></param>
        public QC_RecordByDoctor(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_app = app;
                Init();
                //Add by xlb 2013-06-18
                DS_Common.CancelMenu(panelControl1, new ContextMenuStrip());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Init()
        {
            try
            {
                lookUpWindowDepartment.SqlHelper = m_app.SqlHelper;
                DataTable Dept = m_app.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);
                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";
                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", 70);
                cols.Add("NAME", 80);
                SqlWordbook deptWordBook = new SqlWordbook("querydept", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                //lookUpEditorDepartment.CodeValue = "0000";


                this.lookUpWindowUser.SqlHelper = m_app.SqlHelper;
                string sql = "select ID,NAME,PY,WB from users";
                DataTable Name = m_app.SqlHelper.ExecuteDataTable(sql);
                Name.Columns["ID"].Caption = "医师工号";
                Name.Columns["NAME"].Caption = "医师姓名";
                Dictionary<string, int> cols1 = new Dictionary<string, int>();
                cols1.Add("ID", 70);
                cols1.Add("NAME", 80);
                SqlWordbook nameWordBook = new SqlWordbook("queryname", Name, "ID", "NAME", cols, "ID//NAME//PY//WB");
                this.lookUpEditorUser.SqlWordbook = nameWordBook;

                dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                dateEdit_end.Text = DateTime.Now.ToShortDateString();
                //this.lookUpEditorDepartment.SelectedText = "妇科";
                lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;     //王冀  2013  1  4  初始值设置为用户所在科室
                //this.lookUpEditorUser.SelectedText = "";
                this.lookUpEditorUser.CodeValue = m_app.User.DoctorId;           //王冀  2013  1  4  初始值设置为用户所在科室

                this.gridControlRecord.DataSource = null;
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
                string depid = lookUpEditorDepartment.CodeValue;
                string dateBegin = this.dateEdit_begin.Text;
                string dateEnd = this.dateEdit_end.DateTime.AddDays(1).ToString("yyyy-MM-dd");//.Text;

                string tablename = DataAccess.GetConfigValueByKey("AutoScoreMainpage");
                string[] mainpage = tablename.Split(',');
                if (this.dateEdit_begin.DateTime > this.dateEdit_end.DateTime)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("起始时间不能大于结束时间");
                    return;
                }
                string userid = this.lookUpEditorUser.CodeValue;
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_QueryByDoctor, userid, dateBegin, dateEnd, depid, mainpage[0], mainpage[3], mainpage[2]));
                this.gridControlRecord.DataSource = dt;
                this.labPatCount.Text = dt.Rows.Count.ToString();
                //if ((this.gridViewRecord.DataSource as DataTable) == null || (this.gridViewRecord.DataSource as DataTable).Rows.Count == 0)
                if (((DataView)this.gridViewRecord.DataSource).ToTable() == null || ((DataView)this.gridViewRecord.DataSource).ToTable().Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有符合条件的记录");
                    gridControlRecord.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                Init();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        public void  ResetControl()
        {
            dateEdit_begin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEdit_end.Text = DateTime.Now.ToShortDateString();
            this.gridControlRecord.DataSource = null;
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        private void gridViewRecord_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void QC_RecordByDoctor_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(20, this.Height - 22);
                this.labPatCount.Location = new Point(71, this.Height - 22);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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
