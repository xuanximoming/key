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
using System.Data.SqlClient;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Emr.QcManager
{
    public partial class OutHospitalNoSubmit : DevExpress.XtraEditors.XtraUserControl
    {
        IEmrHost m_app;
        public OutHospitalNoSubmit(IEmrHost app)
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

            cols.Add("ID", 70);
            cols.Add("NAME", 80);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorDepartment.SqlWordbook = deptWordBook;
            //lookUpEditorDepartment.SelectedText = "妇科";
            lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;        //王冀  2013  1  4  初始值设置为用户所在科室
        }
        /// <summary>
        /// 窗体加载事件
        /// add by ywk 2012年10月9日 16:41:20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutHospitalNoSubmit_Load(object sender, EventArgs e)
        {
            InitDepartment();
            //初始化病人姓名下拉框 add by ywk 2012年10月9日 15:45:53 
            InitPatName();
            //初始化病历号下拉框
            IntiRecordID();
            dateEditBegin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEditEnd.Text = DateTime.Now.ToShortDateString();

            //this.lookUpEditorDepartment.Focus();
        }

        /// <summary>
        /// 初始化病历号下拉框
        /// add by ywk 2012年10月9日 15:49:33 
        /// </summary>
        private void IntiRecordID()
        {
            this.lookUpWindowRecordID.SqlHelper = m_app.SqlHelper;

            DataTable RecordID = m_app.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                 new SqlParameter[] { new SqlParameter("@GetType", "5") }, CommandType.StoredProcedure);

            RecordID.Columns["ID"].Caption = "病历号";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 150);

            SqlWordbook recordIDWordBook = new SqlWordbook("queryrecordid", RecordID, "ID", "ID", cols);
            this.lookUpEditorRecordID.SqlWordbook = recordIDWordBook;
        }
        /// <summary>
        /// 初始化病人姓名下拉框
        /// add by ywk 2012年10月9日 15:46:10
        /// </summary>
        private void InitPatName()
        {
            this.lookUpWindowName.SqlHelper = m_app.SqlHelper;

            DataTable Name = m_app.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                 new SqlParameter[] { new SqlParameter("@GetType", "4") }, CommandType.StoredProcedure);

            Name.Columns["ID"].Caption = "代码";
            Name.Columns["NAME"].Caption = "病人姓名";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 70);
            cols.Add("NAME", 80);

            SqlWordbook nameWordBook = new SqlWordbook("queryname", Name, "ID", "NAME", cols);
            this.lookUpEditorName.SqlWordbook = nameWordBook;
        }
        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            throw new NotImplementedException();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (dateEditBegin.DateTime.Date > dateEditEnd.DateTime.Date)
                {
                    MessageBox.Show("起始时间不能大于结束时间");
                    return;
                }
                BindDataSouce(this.lookUpEditorDepartment.CodeValue);
                if (((DataView)this.gridViewNoSubmit.DataSource).ToTable() == null || ((DataView)this.gridViewNoSubmit.DataSource).ToTable().Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有符合条件的记录");
                    gridControl1.DataSource = null;//没有符合的就要把上次清空add by ywk 2013年8月23日 11:32:07
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        ///  绑定数据源 
        ///  add by ywk 2012年10月9日 16:42:54 
        /// </summary>
        public void BindDataSouce(string deptid)
        {
            if (deptid == null || deptid == "")
                deptid = "*";
            //只做了2012版本的，没做中医版本的 add by ywk 2013年8月15日 19:58:03
            string tablename = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
            string[] mainpage = tablename.Split(',');

            string patname = this.lookUpEditorName.Text.Trim();//病人姓名
            string patId = this.lookUpEditorRecordID.CodeValue;//住院号
            string sexid = radioSex.SelectedIndex == 0 ? "" : radioSex.SelectedIndex.ToString();
            string timebegin = dateEditBegin.DateTime.Date.ToString("yyyy-MM-dd");
            string timeend = dateEditEnd.DateTime.Date.AddDays(1).ToString("yyyy-MM-dd");
            //string patid = this.lookUpWindowRecordID.CodeValue;
            //DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_QueryOutHospitalNOSubmit, deptid));
            DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_QueryOutHospitalNOSubmit, mainpage[0], mainpage[2], deptid, sexid, timebegin, timeend, DS_Common.FilterSpecialCharacter(patname), DS_Common.FilterSpecialCharacter(patId)));
            gridControl1.DataSource = dt;
            this.labelControlCount.Text = dt.Rows.Count.ToString();
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            this.lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;   //当前科室ID    wangj 2013 2 26
            this.lookUpEditorName.CodeValue = "";
            this.lookUpEditorRecordID.CodeValue = "";
            ResetControl();
        }

        public void ResetControl()
        {
            dateEditBegin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEditEnd.Text = DateTime.Now.ToShortDateString();
           
            this.radioSex.SelectedIndex = 0;
            this.gridControl1.DataSource = null;
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
        }

        private void OutHospitalNoSubmit_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labPatCount.Location = new Point(20, this.Height - 23);
                this.labelControlCount.Location = new Point(71, this.Height - 23);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridViewNoSubmit_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void gridViewNoSubmit_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gridViewNoSubmit.FocusedRowHandle == -1)
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
