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

            string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid  union select  '*' Id   , '全院' name ,'' py,'' wb  from dual");
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

        private void OutHospitalNoSubmit_Load(object sender, EventArgs e)
        {
            //初始科室下拉框
            InitDepartment();
            //初始化病人姓名下拉框 add by ywk 2012年10月9日 15:45:53 
            InitPatName();
            //初始化病历号下拉框
            IntiRecordID();
            dateEditBegin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEditEnd.Text = DateTime.Now.ToShortDateString();
            dateEditRYbegin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEditRYend.Text = DateTime.Now.ToShortDateString();
            this.lookUpEditorDepartment.Focus();
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
        /// <summary>
        /// 查询操作
        /// edit by ywk 2012年10月9日 15:36:09
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (dateEditRYbegin.DateTime.Date >dateEditRYend.DateTime.Date)
                {
                    MessageBox.Show("入院起始时间不能大于结束时间");
                    return;
                }
                if (dateEditBegin.DateTime.Date > dateEditEnd.DateTime.Date)
                {
                    MessageBox.Show("出院起始时间不能大于结束时间");
                    return;
                }
                BindDataSouce(this.lookUpEditorDepartment.CodeValue);
                //if ((this.gridView1.DataSource as DataTable) == null || (this.gridView1.DataSource as DataTable).Rows.Count == 0)
                if (((DataView)this.gridView1.DataSource).ToTable() == null || ((DataView)this.gridView1.DataSource).ToTable().Rows.Count == 0)
                {
                    MessageBox.Show("没有符合条件的记录");
                    gridControl1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
               MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 绑定数据源 
        /// edit by ywk 2012年10月9日 15:36:28 
        /// </summary>
        public void BindDataSouce(string deptid)
        {
            try
            {
                if (deptid == null || deptid == "")
                    deptid = "";
                //只做了2012版本的，没做中医版本的 add by ywk 2013年8月15日 19:58:03
                string tablename = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                string[] mainpage = tablename.Split(',');
                string patname = this.lookUpEditorName.Text.Trim();//病人姓名
                string recordid = this.lookUpEditorRecordID.CodeValue;//住院号
                string sexid = radioSex.SelectedIndex == 0 ? "" : radioSex.SelectedIndex.ToString();
                string RYtimebegin =dateEditRYbegin.DateTime.Date.ToString("yyyy-MM-dd");
                string RYtimeend =dateEditRYend.DateTime.Date.AddDays(1).ToString("yyyy-MM-dd");
                string timebegin = dateEditBegin.DateTime.Date.ToString("yyyy-MM-dd");
                string timeend = dateEditEnd.DateTime.Date.AddDays(1).ToString("yyyy-MM-dd");
                string patid = this.lookUpWindowRecordID.CodeValue;//原来的是  sql_QueryOutHospitalNOSubmit？？？原来的是错的吧 edit by ywk 
                string  LockState = GetLockStatusValue(this.cbxLockStatus.Text).ToString();
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(SQLUtil.sql_QueryOutHospitalNOLock,mainpage[0],mainpage[2], deptid, sexid, timebegin, timeend,DS_Common.FilterSpecialCharacter(patname), patid,RYtimebegin,RYtimeend,LockState));
                //SqlParameter p_result = new SqlParameter("@result", SqlDbType.Structured);   //注销 BY JXH  原因是调用存储过程无法使用wmsys.wm_concat（distinct XXX）函数，以致诊断无法串联上，所以SQL语句写程序里
                //p_result.Direction = ParameterDirection.Output;
                //SqlParameter[] sps =
                //{
                //new SqlParameter("@deptId",deptid),
                //new SqlParameter("@sex",sexid),
                //new SqlParameter("@dateBegin",timebegin),
                //new SqlParameter("@dateEnd",timeend),
                //new SqlParameter("@patName",patname),
                //new SqlParameter("@PatId",patid),
                //p_result
                //};
                //SqlHelper.DS_SqlHelper.CreateSqlHelper();
                //DataTable dt = SqlHelper.DS_SqlHelper.ExecuteDataTable("emrproc.usp_GetOutHosButNotLocks", sps, CommandType.StoredProcedure);
                gridControl1.DataSource = dt;
                labPatCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GetLockStatusValue(string describle)
        {
            string value = "0";
            switch (describle)
            {
                case "未完成":
                    value = "4700";
                    break;
                case "已归档":
                    value = "4701";
                    break;
                case "撤销归档":
                    value = "4702";
                    break;
                case "科室质控":
                    value = "4705";
                    break;
                case "已提交":
                    value = "4706";
                    break;
                case "已完成":
                    value = "4704";
                    break;
                case "补写提交":
                    value = "4707";
                    break;
                case "未归档":
                    value = "4707,4700,4702,4705,4706,4704";
                    break;
                case "全部":
                    value = "0";
                    break;
                //         '
                //when i.islock='4701'then '已归档'
                //when i.islock='4702' then '撤销归档'
                //  when i.islock='4704'then '已完成'
                //when i.islock='4705' then '科室质控'
                //   when i.islock='4706' then '已提交'
            }
            return value;
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

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            this.lookUpEditorDepartment.CodeValue = m_app.User.CurrentDeptId;
            this.lookUpEditorName.CodeValue = "";
            this.lookUpEditorRecordID.CodeValue = "";
            ResetControl();
        }

        public void ResetControl()
        {
            dateEditBegin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEditEnd.Text = DateTime.Now.ToShortDateString();
            dateEditRYbegin.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            dateEditRYend.Text = DateTime.Now.ToShortDateString();
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

            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
        }

        private void OutHospitalNoLock_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                //this.labPatCount.Location = new Point(74, this.Height - 92);
                //this.labelControlTotalPats.Location = new Point(23, this.Height - 92);
            }
            catch (Exception ex)
            {
               MyMessageBox.Show(1, ex);
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
