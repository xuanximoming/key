using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DrectSoft.Wordbook;
using DrectSoft.Core.RecordManage.PublicSet;
using DrectSoft.Common;

namespace DrectSoft.Core.RecordManage
{
    public partial class UCOperRecordLog : DevExpress.XtraEditors.XtraUserControl
    {
        public UCOperRecordLog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }

                DataTable table = SqlUtil.App.SqlHelper.ExecuteDataTable("emr_record_input.usp_getoperrecordlog"
                   , new SqlParameter[] 
                { 
                    new SqlParameter("@User_id", this.lookUpWindowUser.CodeValue), 
                    new SqlParameter("@Patient_id", this.lookUpWindowPatient.CodeValue),
                    new SqlParameter("@StartDate", dateEditBeginTime.DateTime.Date.ToString("yyyy-MM-dd")), 
                    new SqlParameter("@EndDate", dateEditEndTime.DateTime.Date.ToString("yyyy-MM-dd"))
                }
                   , CommandType.StoredProcedure);

                gridviewOperRecordLog.SelectAll();
                gridviewOperRecordLog.DeleteSelectedRows();
                gridControlOperRecordLog.DataSource = table;

                lblTip.Text = "共" + table.Rows.Count.ToString() + "条记录";

                if (table.Rows.Count <= 0)
                {
                    SqlUtil.App.CustomMessageBox.MessageShow("没有满足条件的记录");
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-05
        /// 1、add try ... catch
        /// 2、封装初始化方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCOperRecordLog_Load(object sender, EventArgs e)
        {
            try
            {
                InitDepartment();
                InitName();
                Reset();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 初始化科室
        /// </summary>
        private void InitDepartment()
        {
            try
            {
                lookUpWindowUserDept.SqlHelper = SqlUtil.App.SqlHelper;

                DataTable Dept = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);

                Dept.Columns["ID"].Caption = "科室编码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querydept", Dept, "ID", "NAME", cols,"ID//NAME//PY//WB");
                lookUpEditorUserDept.SqlWordbook = deptWordBook;
                lookUpEditorUserDept.CodeValue = "0000";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 初始化员工姓名和病人姓名
        /// </summary>
        private void InitName()
        {
            try
            {
                this.lookUpWindowUser.SqlHelper = SqlUtil.App.SqlHelper;
                this.lookUpWindowPatient.SqlHelper = SqlUtil.App.SqlHelper;

                string sqlusers = "select ID,NAME,PY,WB from users where";
                if (this.lookUpEditorUserDept.CodeValue.Trim() != "")
                {
                    string deptid = this.lookUpEditorUserDept.CodeValue == "0000" ? "" : this.lookUpEditorUserDept.CodeValue;
                    if (deptid != "")
                    {
                        sqlusers = "select ID,NAME,PY,WB from users where Deptid='" + deptid + "'";
                    }

                }
                DataTable users = SqlUtil.App.SqlHelper.ExecuteDataTable(sqlusers);
                users.Columns["ID"].Caption = "员工工号";
                users.Columns["NAME"].Caption = "员工姓名";

                DataTable patients = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
         new SqlParameter[] { new SqlParameter("@GetType", "4") }, CommandType.StoredProcedure);
                patients.Columns["ID"].Caption = "病人编号";
                patients.Columns["NAME"].Caption = "病人姓名";

                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook userWordBook = new SqlWordbook("queryuser", users, "ID", "NAME", cols, "ID//NAME//PY//WB");
                this.lookUpEditorUser.SqlWordbook = userWordBook;
                SqlWordbook patientWordBook = new SqlWordbook("querypatient", patients, "ID", "NAME", cols, "ID//NAME//PY//WB");
                this.lookUpEditorPatient.SqlWordbook = patientWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 画面验证
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-26</date>
        /// </summary>
        /// <returns></returns>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(lookUpEditorUser.CodeValue))
                {
                    lookUpEditorUser.Focus();
                    return "请选择员工姓名";
                }
                else if (dateEditBeginTime.DateTime.Date > dateEditEndTime.DateTime.Date)
                {
                    dateEditBeginTime.Focus();
                    return "操作开始日期不能大于操作结束日期";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return string.Empty;
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-26</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridviewOperRecordLog_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-05</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-05</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset()
        {
            try
            {
                dateEditBeginTime.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                dateEditEndTime.Text = DateTime.Now.ToShortDateString();
                this.lookUpEditorUserDept.CodeValue = "0000";
                this.lookUpEditorUser.CodeValue = string.Empty;
                this.lookUpEditorPatient.CodeValue = string.Empty;
                this.lookUpEditorUserDept.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
