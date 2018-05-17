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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace DrectSoft.Core.RecordManage.UCControl
{
    public partial class UCSingInRecord : DevExpress.XtraEditors.XtraUserControl
    {
        public UCSingInRecord()
        {
            InitializeComponent();
        }

        //初始化科室
        private void InitDepartment()
        {
            try
            {
                lookUpWindowDepartment.SqlHelper = SqlUtil.App.SqlHelper;

                DataTable Dept = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);

                Dept.Columns["ID"].Caption = "科室编码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                lookUpEditorDepartment.CodeValue = "0000";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化病人姓名
        /// </summary>
        private void InitName()
        {
            try
            {
                this.lookUpWindowName.SqlHelper = SqlUtil.App.SqlHelper;

                DataTable Name = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                     new SqlParameter[] { new SqlParameter("@GetType", "4") }, CommandType.StoredProcedure);

                Name.Columns["ID"].Caption = "病人编号";
                Name.Columns["NAME"].Caption = "病人姓名";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook nameWordBook = new SqlWordbook("queryname", Name, "ID", "NAME", cols, "ID//NAME//PY//WB");
                this.lookUpEditorName.SqlWordbook = nameWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        private void UCSingInRecord_Load(object sender, EventArgs e)
        {
            try
            {
                lblSingInTip.Text = "签收人：" + SqlUtil.App.User.Name;

                //更新到期的借阅病历
                SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_UpDateDueApplyRecord",
                     new SqlParameter[] { new SqlParameter("@ApplyDoctor", SqlUtil.App.User.Id) }, CommandType.StoredProcedure);

                InitDepartment();
                InitName();

                Reset();
                this.ActiveControl = lookUpEditorName;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 查询事件
        /// edit by Yanqiao.Cai 2012-11-16
        /// 1、add try ... catch
        /// 2、加载性别图片
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
                //加载性别图片
                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListXB);

                DataTable table = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetSignInRecordNew"
                   , new SqlParameter[] 
                { 
                    new SqlParameter("@DateBegin", dateEditBegin.DateTime.Date.ToString("yyyy-MM-dd")), 
                    new SqlParameter("@DateEnd", string.IsNullOrEmpty(dateEnd.Text.Trim())? DateTime.Now.ToString("yyyy-MM-dd"):dateEnd.DateTime.Date.ToString("yyyy-MM-dd")),
                    new SqlParameter("@DateInBegin", dateEditInBegin.DateTime.Date.ToString("yyyy-MM-dd")), 
                    new SqlParameter("@DateInEnd", string.IsNullOrEmpty(dateEditInEnd.Text.Trim())?DateTime.Now.ToString("yyyy-MM-dd"):dateEditInEnd.DateTime.Date.ToString("yyyy-MM-dd")),
                    new SqlParameter("@PatientName", this.lookUpEditorName.Text.Trim()),
                    new SqlParameter("@OutHosDept", lookUpEditorDepartment.CodeValue.ToString())
                }
                   , CommandType.StoredProcedure);

                gridViewSingInRecord.SelectAll();
                gridViewSingInRecord.DeleteSelectedRows();
                gridControlSingInRecord.DataSource = table;

                lblTip.Text = "共" + table.Rows.Count.ToString() + "条记录";

                if (table.Rows.Count <= 0)
                    SqlUtil.App.CustomMessageBox.MessageShow("没有符合条件的记录");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 画面验证
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-26</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(lookUpEditorDepartment.CodeValue))
                {
                    lookUpEditorDepartment.Focus();
                    return "请选择出院科室";
                }
                else if (dateEditBegin.DateTime > dateEnd.DateTime)
                {
                    dateEditBegin.Focus();
                    return "申请开始日期不能大于申请结束日期";
                }
                else if (dateEditInBegin.DateTime > dateEditInEnd.DateTime)
                {
                    dateEditInBegin.Focus();
                    return "入院开始日期不能大于入院结束日期";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return string.Empty;
        }

        private void BrowserMedicalRecord(bool bolMessage)
        {
            try
            {
                #region 注释 by cyq 2012-12-06 修复权限问题 病案管理模块不可编辑病历
                //string noOfInpat; //住院号

                //int fouceRowIndex = gridViewSingInRecord.FocusedRowHandle;
                //if (fouceRowIndex < 0)
                //{
                //    if (bolMessage) SqlUtil.App.CustomMessageBox.MessageShow("请选择需要阅览的病历记录");
                //    return;
                //}
                //DataRow foucesRow = gridViewSingInRecord.GetDataRow(fouceRowIndex);
                //noOfInpat = foucesRow["noofinpat"].ToString();

                //SqlUtil.App.ChoosePatient(Convert.ToDecimal(noOfInpat));
                //SqlUtil.App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", "DrectSoft.Core.MainEmrPad.MainForm");
                ////HistoryRecordBrowser frmHistoryRecordBrowser = new HistoryRecordBrowser(PatID);
                ////frmHistoryRecordBrowser.ShowDialog();
                #endregion

                int fouceRowIndex = gridViewSingInRecord.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    if (bolMessage)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条病人记录");
                    }
                    return;
                }
                DataRow foucesRow = gridViewSingInRecord.GetDataRow(fouceRowIndex);

                string noOfFirstPage = foucesRow["NOOFINPAT"].ToString();
                EmrBrowser frm = new EmrBrowser(noOfFirstPage, SqlUtil.App);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 病历归还记录双击事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、双击小标题应无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewSingInRecord_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewSingInRecord.CalcHitInfo(gridControlSingInRecord.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                BrowserMedicalRecord(false);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 阅览病历事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                BrowserMedicalRecord(true);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void AuditApplayRecord(string AuditType)
        {
            try
            {
                int fouceRowIndex = gridViewSingInRecord.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    SqlUtil.App.CustomMessageBox.MessageShow("请选择需要签收的病历借阅申请记录");
                    return;
                }
                DataRow foucesRow = gridViewSingInRecord.GetDataRow(fouceRowIndex);

                if (SqlUtil.App.CustomMessageBox.MessageShow("您确定要签收该病历借阅申请记录吗？"
                    , CustomMessageBoxKind.QuestionYesNo) == DialogResult.No)
                    return;

                SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_AuditApplyRecord"
                  , new SqlParameter[] 
                { 
                    new SqlParameter("@AuditType", AuditType), 
                    new SqlParameter("@ManID",SqlUtil.App.User.Id),
                    new SqlParameter("@ID", foucesRow["ID"].ToString())
                }
                  , CommandType.StoredProcedure);

                SqlUtil.App.CustomMessageBox.MessageShow("签收成功");
                gridViewSingInRecord.DeleteRow(gridViewSingInRecord.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("签收失败，\n错误原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 签收事件
        /// </summary>
        /// <param name="AuditType"></param>
        private void btnSingIn_Click(object sender, EventArgs e)
        {
            try
            {
                AuditApplayRecord("5206");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-26</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewSingInRecord_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
                dateEditBegin.Text = DateTime.Now.AddDays(-7).ToShortDateString();
                dateEnd.Text = DateTime.Now.ToShortDateString();
                dateEditInBegin.Text = DateTime.Now.AddDays(-7).ToShortDateString();
                dateEditInEnd.Text = DateTime.Now.ToShortDateString();

                this.lookUpEditorName.CodeValue = string.Empty;
                this.lookUpEditorDepartment.CodeValue = "0000";
                this.lookUpEditorName.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 清空控件中的值
        /// <auth>张业兴</auth>
        /// <date>2012-12-17</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                lookUpEditorName.CodeValue = string.Empty;
                dateEditBegin.Text = string.Empty;
                dateEnd.Text = string.Empty;
                lookUpEditorDepartment.CodeValue = "0000";
                dateEditInBegin.Text = string.Empty;
                dateEditInEnd.Text = string.Empty;
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}
