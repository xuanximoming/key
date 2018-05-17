using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common;
using DrectSoft.Core.RecordManage.PublicSet;
using System.Data.SqlClient;
using DrectSoft.Wordbook;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace DrectSoft.Core.RecordManage.UCControl
{
    public partial class UCApplyRecord : DevExpress.XtraEditors.XtraUserControl
    {
        public UCApplyRecord()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化科室
        /// </summary>
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

                cols.Add("ID", 80);
                cols.Add("NAME", 150);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                lookUpEditorDepartment.CodeValue = "0000";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 已弃用 by cyq 2012-12-06
        /// <summary>
        /// 初始化病人姓名
        /// </summary>
        //private void InitName()
        //{
        //    try
        //    {
        //        this.lookUpWindowName.SqlHelper = SqlUtil.App.SqlHelper;

        //        DataTable Name = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
        //             new SqlParameter[] { new SqlParameter("@GetType", "4") }, CommandType.StoredProcedure);

        //        Name.Columns["ID"].Caption = "病人编号";
        //        Name.Columns["NAME"].Caption = "病人姓名";

        //        Dictionary<string, int> cols = new Dictionary<string, int>();

        //        cols.Add("ID", 60);
        //        cols.Add("NAME", 90);

        //        SqlWordbook nameWordBook = new SqlWordbook("queryname", Name, "ID", "NAME", cols, "ID//NAME//PY//WB");
        //        this.txt_patiName.SqlWordbook = nameWordBook;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        #endregion

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-05
        /// 1、add try ... catch
        /// 2、封装初始化方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCApplyRecord_Load(object sender, EventArgs e)
        {
            try
            {
                lblAuditTip.Text = "审核人：" + SqlUtil.App.User.Name;

                InitDepartment();
                //InitName(); //edit by cyq 2012-12-06

                Reset();
                this.ActiveControl = txt_patiName;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 查询事件
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

                DataTable table = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetApplyRecordNew"
                   , new SqlParameter[] 
                { 
                    new SqlParameter("@DateBegin", dateBegin.DateTime.Date.ToString("yyyy-MM-dd")), 
                    new SqlParameter("@DateEnd", string.IsNullOrEmpty(dateEnd.Text.Trim())?DateTime.Now.ToString("yyyy-MM-dd"):dateEnd.DateTime.Date.ToString("yyyy-MM-dd")),
                    new SqlParameter("@PatientName", this.txt_patiName.Text.Trim()),
                    new SqlParameter("@DocName", this.txt_applyDoc.Text.Trim()),
                    new SqlParameter("@OutHosDept", lookUpEditorDepartment.CodeValue.ToString())
                }
                   , CommandType.StoredProcedure);

                //         DataTable table = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetApplyRecord"
                //, new SqlParameter[] 
                //             { 
                //                 new SqlParameter("@DateBegin", dateBegin.DateTime.Date.ToString("yyyy-MM-dd")), 
                //                 new SqlParameter("@DateEnd", dateEnd.DateTime.Date.ToString("yyyy-MM-dd")),
                //                 new SqlParameter("@OutHosDept", lookUpEditorDepartment.CodeValue.ToString())
                //             }
                //, CommandType.StoredProcedure);

                gridViewApplyRecord.SelectAll();
                gridViewApplyRecord.DeleteSelectedRows();
                gridControlApplyRecord.DataSource = table;

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
        /// 画面验证
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-26</date>
        /// </summary>
        /// <returns></returns>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(lookUpEditorDepartment.CodeValue))
                {
                    lookUpEditorDepartment.Focus();
                    return "请选择出院科室";
                }
                else if (dateBegin.DateTime.Date > dateEnd.DateTime.Date)
                {
                    dateBegin.Focus();
                    return "出院开始日期不能大于出院结束日期";
                }
                //else if (dateEditInBegin.DateTime.Date > dateEditInEnd.DateTime.Date)
                //{
                //    dateEditInBegin.Focus();
                //    return "入院开始日期不能大于入院结束日期";
                //}

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void BrowserMedicalRecord(bool bolMessage)
        {
            try
            {
                string noOfInpat; //住院号

                int fouceRowIndex = gridViewApplyRecord.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    if (bolMessage) SqlUtil.App.CustomMessageBox.MessageShow("请选择一条病人记录");
                    return;
                }
                DataRow foucesRow = gridViewApplyRecord.GetDataRow(fouceRowIndex);

                //edit by wyt 2012-11-09 新建病历显示窗口
                string noOfFirstPage = foucesRow["NOOFINPAT"].ToString();
                EmrBrowser frm = new EmrBrowser(noOfFirstPage, SqlUtil.App);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
                #region 取消加载插件方式显示病历 edit by wyt 2012-11-09
                //SqlUtil.App.ChoosePatient(Convert.ToDecimal(noOfInpat));
                //SqlUtil.App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", "DrectSoft.Core.MainEmrPad.MainForm");
                #endregion

                //HistoryRecordBrowser frmHistoryRecordBrowser = new HistoryRecordBrowser(PatID);
                //frmHistoryRecordBrowser.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 双击事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、双击小标题应无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewApplyRecord_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewApplyRecord.CalcHitInfo(gridControlApplyRecord.PointToClient(Cursor.Position));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AuditType">审批类别</param>
        /// <param name="flag">标识：0-审批；1-不予审批；2-作废</param>
        private void AuditApplayRecord(string AuditType,int flag)
        {
            string tipStr = flag == 0 ? "审批" : (flag == 1 ? "不予审批" : "作废");
            try
            {
                int fouceRowIndex = gridViewApplyRecord.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    SqlUtil.App.CustomMessageBox.MessageShow("请选择一条病历借阅申请记录");
                    return;
                }
                DataRow foucesRow = gridViewApplyRecord.GetDataRow(fouceRowIndex);

                if (SqlUtil.App.CustomMessageBox.MessageShow("您确定要" + tipStr + "该病历借阅申请记录吗？"
                    , CustomMessageBoxKind.QuestionYesNo) == DialogResult.No)
                {
                    return;
                }

                SqlUtil.App.SqlHelper.ExecuteNoneQuery("usp_AuditApplyRecord"
                  , new SqlParameter[] 
                { 
                    new SqlParameter("@AuditType", AuditType), 
                    new SqlParameter("@ManID",SqlUtil.App.User.Id),
                    new SqlParameter("@ID", foucesRow["ID"].ToString())
                }
                  , CommandType.StoredProcedure);

                SqlUtil.App.CustomMessageBox.MessageShow(tipStr + "成功");
                gridViewApplyRecord.DeleteRow(gridViewApplyRecord.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(tipStr+"失败，\n错误原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 审批事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAudit_Click(object sender, EventArgs e)
        {
            try
            {
                AuditApplayRecord("5202",0);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 不予审批事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNoAudit_Click(object sender, EventArgs e)
        {
            try
            {
                AuditApplayRecord("5203",1);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 作废事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                AuditApplayRecord("5204",2);
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
                dateBegin.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                dateEnd.Text = DateTime.Now.ToShortDateString();
                //dateEditInBegin.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                //dateEditInEnd.Text = DateTime.Now.ToShortDateString();
                this.txt_patiName.Text = string.Empty;
                this.txt_applyDoc.Text = string.Empty;
                this.lookUpEditorDepartment.CodeValue = "0000";
                this.txt_patiName.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-26</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewApplyRecord_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 列表事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-14</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewApplyRecord_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                DataRow dr = gridViewApplyRecord.GetDataRow(e.RowHandle) as DataRow;
                if (null == dr)
                {
                    return;
                }
                if (dr["STATUS"].ToString() == "5201")
                {//待审批
                    e.Appearance.ForeColor = Color.Black;
                }
                if (dr["STATUS"].ToString() == "5202")
                {//申请通过
                    e.Appearance.ForeColor = Color.YellowGreen;
                }
                else if (dr["STATUS"].ToString() == "5203")
                {//申请未通过
                    e.Appearance.ForeColor = Color.Red;
                }
                else if (dr["STATUS"].ToString() == "5204")
                {//申请作废
                    e.Appearance.ForeColor = Color.Orange;
                }
                else if (dr["STATUS"].ToString() == "5205")
                {//借阅到期
                    e.Appearance.ForeColor = Color.MediumAquamarine;
                }
                else if (dr["STATUS"].ToString() == "5206")
                {//已签收
                    e.Appearance.ForeColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 清空按钮
        /// <auth>张业兴</auth>
        /// <date>2012-12-17</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txt_patiName.Text = string.Empty;
                lookUpEditorDepartment.CodeValue = "0000";
                panelControl1.Text = string.Empty;
                dateBegin.Text = string.Empty;
                dateEnd.Text = string.Empty;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           
        }

        
    }
}
