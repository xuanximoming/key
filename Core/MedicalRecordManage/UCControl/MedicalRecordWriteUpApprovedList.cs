using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MedicalRecordManage.Object;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;

using System.Data.Common;
using System.Data.SqlClient;
using DrectSoft.Common;
using DrectSoft.Service;
using MedicalRecordManage.UI;


namespace MedicalRecordManage.UCControl
{
    /// <summary>
    /// 病历借阅记录查询界面
    /// </summary>
    public partial class MedicalRecordWriteUpApprovedList : DevExpress.XtraEditors.XtraUserControl//, IEMREditor
    {
        //IEmrHost m_app;

        //public MedicalRecordApprovedList(IEmrHost app)
        //{
        //    InitializeComponent();
        //    InitializeParameters();
        //    InitializeGrid();
        //    m_app = app;
        //    ComponentCommand.InitializeDepartment(ref this.lookUpEditorDepartment, ref this.lookUpWindowDepartment);            
        //}

        public MedicalRecordWriteUpApprovedList()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception)
            {

                throw;
            }

        }
        private void InitializeParameters()
        {
            try
            {
                this.dateStart.DateTime = System.DateTime.Today.AddMonths(-6);
                this.dateEnd.DateTime = System.DateTime.Today;
                //初始化下拉框科室信息

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void InitializeGrid()
        {
            try
            {
                for (int i = 0; i < dbGridView.Columns.Count; i++)
                {
                    //dbGridView.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    dbGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                //if (CheckDate())
                //{
                    DevExpress.Utils.WaitDialogForm m_WaitDialog = new DevExpress.Utils.WaitDialogForm("正在查询数据...", "请稍等");
                    LoadData();
                    m_WaitDialog.Hide();
                //}
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        private bool CheckDate()
        {
            try
            {
                if (this.dateStart.DateTime > this.dateEnd.DateTime)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("申请起始日期不能大于结束日期", "信息提示");
                    this.dateStart.Focus();
                    return false;
                }
                else
                    return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 查询加载数据方法
        /// Modify by xlb 2013-05-30
        /// </summary>
        private void LoadData()
        {
            try
            {
                //add by 2013-6-14   xll 
                string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                string sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_WriteUp_SQL;
                if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
                {
                    sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_WriteUp_SQL_ZY;
                }

                List<DbParameter> sqlParams = new List<DbParameter>();
                if (this.txtDoctor.Text.Trim() != null && this.txtDoctor.Text.Trim() != "")
                {
                    sql = sql + " and i.applyname like @doc ";
                    SqlParameter param1 = new SqlParameter("@doc", SqlDbType.VarChar);
                    param1.Value = "%" + this.txtDoctor.Text.Trim() + "%";
                    sqlParams.Add(param1);
                }
                if (this.txtName.Text.Trim() != null && this.txtName.Text.Trim() != "")
                {
                    sql = sql + " and i.name like @name ";
                    SqlParameter param2 = new SqlParameter("@name", SqlDbType.VarChar);
                    param2.Value = "%" + this.txtName.Text.Trim() + "%";
                    sqlParams.Add(param2);
                }
                if (this.txtNumber.Text.Trim() != null && this.txtNumber.Text.Trim() != "")
                {
                    sql = sql + " and i.patid like '%'||@patId||'%' ";
                    SqlParameter param3 = new SqlParameter("@patId", SqlDbType.VarChar);
                    param3.Value = this.txtNumber.Text.Trim();
                    sqlParams.Add(param3);
                }

                if (this.lookUpEditorDepartment.Text != null && this.lookUpEditorDepartment.Text.Trim() != "")
                {
                    if (lookUpEditorDepartment.CodeValue != "0000")
                    {
                        sql = sql + " and i.outhosdept = @dept";
                        SqlParameter param4 = new SqlParameter("@dept", SqlDbType.VarChar);
                        param4.Value = lookUpEditorDepartment.CodeValue;
                        sqlParams.Add(param4);
                    }
                }

                if (this.dateStart.Text.Trim() != null && this.dateStart.Text.Trim() != "")
                {
                    string ds = this.dateStart.DateTime.ToString("yyyy-MM-dd 00:00:00");
                    sql = sql + " and  to_char(i.applydate,'yyyy-mm-dd hh24:mi:ss') >= @ds";
                    SqlParameter param5 = new SqlParameter("@ds", SqlDbType.VarChar);
                    param5.Value = ds;
                    sqlParams.Add(param5);
                }
                if (this.dateEnd.Text.Trim() != null && this.dateEnd.Text.Trim() != "")
                {
                    string de = this.dateEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                    sql = sql + " and  to_char(i.applydate,'yyyy-mm-dd hh24:mi:ss') <= @de";
                    SqlParameter param6 = new SqlParameter("@de", SqlDbType.VarChar);
                    param6.Value = de;
                    sqlParams.Add(param6);
                }
                if (this.cbxStatus.Text != "")
                {
                    sql = sql + " and  i.status = @st";
                    SqlParameter param7 = new SqlParameter("@st", SqlDbType.VarChar);
                    param7.Value = ComponentCommand.GetStatusValue(this.cbxStatus.Text);
                    sqlParams.Add(param7);
                }
                DataTable dataTable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
                //压缩数据源生成新的数据源
                //压缩DateSet中的记录，将住院诊断信息合并
                ComponentCommand.ImpressDataSet(ref dataTable, "id", "cyzd");
                //
                ComponentCommand.InitializeStatusInfo(ref dataTable);
                string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    ResultName = SqlUtil.GetPatsBabyContent(SqlUtil.App, dataTable.Rows[i]["noofinpat"].ToString());
                    dataTable.Rows[i]["Name"] = ResultName;
                    if (dataTable.Rows[i]["isbaby"].ToString() == "1")
                    {
                        dataTable.Rows[i]["PATID"] = SqlUtil.GetPatsBabyMother(dataTable.Rows[i]["mother"].ToString());
                    }
                }
                this.dbGrid.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtName.Text = "";
                this.txtNumber.Text = "";
                InitializeParameters();
                this.lookUpEditorDepartment.CodeValue = "0000";
                cbxStatus.Text = "";
                this.txtDoctor.Text = "";
                this.dbGrid.DataSource = null;
                this.txtDoctor.Focus();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void MedicalRecordApprovedList_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeParameters();
                InitializeGrid();
                DrectSoft.Common.DS_Common.CancelMenu(this.panelHead, contextMenuStrip1);
                ComponentCommand.InitializeDepartment(ref this.lookUpEditorDepartment, ref this.lookUpWindowDepartment);
                this.txtDoctor.Focus();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void dbGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void MedicalRecordApprovedList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                this.SelectNextControl(this.ActiveControl, true, true, true, false);
        }
        //
        private void dbGrid_Click(object sender, EventArgs e)
        {

        }

        private void dbGridView_RowLoaded(object sender, DevExpress.XtraGrid.Views.Base.RowEventArgs e)
        {
            //DataRowView drv = dbGridView.GetRow(e.RowHandle) as DataRowView;
        }

        private void dbGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                DataRowView drv = dbGridView.GetRow(e.RowHandle) as DataRowView;
                if (drv != null)
                {
                    string status = drv["STATUSDES"].ToString();

                    switch (status)
                    {
                        case "草稿":
                            e.Appearance.ForeColor = ComponentCommand._CAO_GAO_COLOR;
                            break;
                        case "待审核":
                            e.Appearance.ForeColor = ComponentCommand._DAI_SHEN_HE_COLOR;
                            break;
                        case "审核通过":
                            e.Appearance.ForeColor = ComponentCommand._SHEN_HE_TONG_GUO_COLOR;
                            break;
                        case "审核不通过":
                            e.Appearance.ForeColor = ComponentCommand._SHEN_HE_BU_TONG_GUO_COLOR;
                            break;
                        case "撤销":
                            e.Appearance.ForeColor = ComponentCommand._CHE_XIAO_COLOR;
                            break;
                        case "归还":
                            e.Appearance.ForeColor = ComponentCommand._GUI_HUAN_COLOR;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void txtDoctor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
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

        private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
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

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
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

        private void cbxStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
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

        private void lookUpEditorDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
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

        private void dateStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
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

        private void dateEnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
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

        private void dbGrid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //查询界面就是查询，不应再可编辑了b
                //DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hitInfo = dbGridView.CalcHitInfo(dbGrid.PointToClient(Cursor.Position));
                //if (hitInfo.RowHandle < 0)
                //{
                //    return;
                //}
                //if (dbGridView.FocusedRowHandle < 0) { return; }
                ////add by 2013-7-4 zjy注释
                ////LoadPatRecordEditor1("grd1");
                ////int[] list = dbGridView.GetSelectedRows();
                ////if (list.Length > 0)
                ////{
                ////    string noofinpat = dbGridView.GetRowCellValue(list[0], "NOOFINPAT").ToString();
                ////    LoadEmrContent(noofinpat);
                ////}

                //DataRow dataRow = dbGridView.GetDataRow(dbGridView.FocusedRowHandle);
                //string noofinpat = dataRow["noofinpat"].ToString();
                //if (SqlUtil.HasBaby(noofinpat))
                //{
                //    ChoosePatOrBaby choosepat = new ChoosePatOrBaby(SqlUtil.App, noofinpat);
                //    choosepat.StartPosition = FormStartPosition.CenterParent;
                //    if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //    {
                //        SqlUtil.App.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));
                //        SqlUtil.App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", YD_BaseService.GetUCEmrInputPath());
                //    }
                //}
                //else
                //{
                //    SqlUtil.App.ChoosePatient(Convert.ToDecimal(noofinpat));
                //    SqlUtil.App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", YD_BaseService.GetUCEmrInputPath());
                //}

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void LoadEmrContent(string noofinpat)
        {
            try
            {
                MedicalRecordManage.UI.EmrBrowerDlg frm = new MedicalRecordManage.UI.EmrBrowerDlg(noofinpat, SqlUtil.App, FloderState.None);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnMedical_Click(object sender, EventArgs e)
        {
            try
            {
                if (dbGridView.DataSource != null)
                {
                    int[] list = dbGridView.GetSelectedRows();
                    if (list.Length > 0)
                    {
                        string noofinpat = dbGridView.GetRowCellValue(list[0], "NOOFINPAT").ToString();
                        LoadEmrContent(noofinpat);
                    }
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}
