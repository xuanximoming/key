using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using MedicalRecordManage.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace MedicalRecordManage.UCControl
{
    /// <summary>
    /// 病历补写审核界面
    /// </summary>
    public partial class MedicalRecordWriteUpApprove : DevExpress.XtraEditors.XtraUserControl//, IEMREditor
    {
        //IEmrHost m_app;

        CGridCheckMarksSelection selection;
        internal CGridCheckMarksSelection Selection
        {
            get
            {
                return selection;
            }
        }

        public string m_sUser = "";

        //public MedicalRecordApprove(IEmrHost app)
        //{
        //    try
        //    {
        //        InitializeComponent();
        //        InitializeParameters();
        //        InitializeGrid();
        //        m_sUser = ComponentCommand.GetCurrentDoctor();
        //        m_app = app;
        //        ComponentCommand.InitializeDepartment(ref this.lookUpEditorDepartment, ref this.lookUpWindowDepartment);                
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public MedicalRecordWriteUpApprove()
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
        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (selection.SelectedCount > 0)
                {
                    ExecuteCommand(2, "审核通过");
                }
                else
                {
                    MessageBox.Show("请选择需要审核的补写记录 ", "信息提示");
                }
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        private void ExecuteCommand(int flag, string msg = "")
        {
            try
            {
                DialogResult dR = MyMessageBox.Show("您确认要" + msg + "？", "信息提示", MyMessageBoxButtons.OkCancel);
                if (dR == DialogResult.OK)
                {
                    for (int i = 0; i < selection.SelectedCount; i++)
                    {
                        DataRowView dataRow = (DataRowView)Selection.selection[i];
                        string id = dataRow["ID"].ToString();
                        if (string.IsNullOrEmpty(id))
                        {
                            return;
                        }
                        if (flag == 2)
                        {
                            WriteUpObject.Approve(id, m_sUser);
                        }
                        else if (flag == 3)
                        {
                            WriteUpObject.ApproveRef(id, m_sUser, memoReason.Text.Trim());
                        }
                    }
                    //Common.Ctrs.DLG.MessageBox.Show("审核成功！", "信息提示");
                    //刷新数据
                    selection.ClearSelection();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 审核不通过事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNotApproved_Click(object sender, EventArgs e)
        {
            try
            {
                if (selection.SelectedCount > 0)
                {
                    if (memoReason.Text.Trim() == null || memoReason.Text.Trim() == "")
                    {
                        MessageBox.Show("请输入审核不通过原因", "信息提示");
                        this.memoReason.Focus();
                    }
                    else
                    {
                        ExecuteCommand(3, "审核不通过");
                    }
                }
                else
                {
                    MessageBox.Show("请选择需要审核的补写记录 ", "信息提示");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        //s
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                //if (CheckDate())
                //{
                DevExpress.Utils.WaitDialogForm m_WaitDialog = new DevExpress.Utils.WaitDialogForm("正在查询数据...", "请稍等");
                LoadData();
                m_WaitDialog.Hide();
                this.txtDoctor.Focus();
                //}
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }

        private bool CheckDate()
        {
            try
            {
                if (this.dateStart.DateTime > this.dateEnd.DateTime)
                {
                    MessageBox.Show("申请起始日期不能大于结束日期", "信息提示");
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

        private void LoadData()
        {
            try
            {
                //add by zjy 2013-6-14

                string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                string sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_WriteUp_SQL + " and i.status = 1";
                if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
                {
                    sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_WriteUp_SQL_ZY + " and i.status = 1";
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
                    sql = sql + " and i.patid like '%'||@patid||'%' ";
                    SqlParameter param3 = new SqlParameter("@patid", SqlDbType.VarChar);
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

                DataTable dataTable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
                //压缩数据源生成新的数据源
                //压缩DateSet中的记录，将住院诊断信息合并
                ComponentCommand.ImpressDataSet(ref dataTable, "id", "cyzd");

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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtName.Text = "";
                this.txtNumber.Text = "";
                memoReason.Text = "";
                InitializeParameters();
                this.lookUpEditorDepartment.CodeValue = "0000";
                this.txtDoctor.Text = "";
                this.dbGrid.DataSource = null;
                this.txtDoctor.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void MedicalRecordApprove_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeParameters();
                InitializeGrid();
                m_sUser = ComponentCommand.GetCurrentDoctor();
                ComponentCommand.InitializeDepartment(ref this.lookUpEditorDepartment, ref this.lookUpWindowDepartment);
                DrectSoft.Common.DS_Common.CancelMenu(this.panelHead, contextMenuStrip1);
                DrectSoft.Common.DS_Common.CancelMenu(this.panelSubBottom, contextMenuStrip1);
                this.txtDoctor.Focus();
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
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

                selection = new CGridCheckMarksSelection(this.dbGridView);//把多选框绑定到你指定的grid
                selection.CheckMarkColumn.VisibleIndex = 0;//使多选框排第一列
                dbGridView.Columns[1].Width = 60;
                dbGridView.Columns[2].Width = 60;
                dbGridView.Columns[3].Width = 120;
                dbGridView.Columns[4].Width = 300;
                dbGridView.Columns[5].Width = 60;
                dbGridView.Columns[6].Width = 100;
                dbGridView.Columns[7].Width = 150;
                dbGridView.Columns[8].Width = 60;
                dbGridView.Columns[9].Width = 60;
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void dbGridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void MedicalRecordApprove_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                this.SelectNextControl(this.ActiveControl, true, true, true, false);
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
                MyMessageBox.Show(1, ex);
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
                MyMessageBox.Show(1, ex);
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
                MyMessageBox.Show(1, ex);
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
                MyMessageBox.Show(1, ex);
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
                MyMessageBox.Show(1, ex);
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
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memoReason_KeyPress(object sender, KeyPressEventArgs e)
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
                MyMessageBox.Show(1, ex);
            }
        }

        private void dbGrid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DataRow dataRow = dbGridView.GetDataRow(dbGridView.FocusedRowHandle);
                string noofinpat = dataRow["noofinpat"].ToString();
                MedicalRecordManage.UI.EmrBrowerDlg frm = new MedicalRecordManage.UI.EmrBrowerDlg(noofinpat, SqlUtil.App, FloderState.None);
                //frm.StartPosition = FormStartPosition.CenterScreen;
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {

                throw ex;
            }

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
        StringFormat s = new StringFormat();
        private void dbGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                s.Alignment = StringAlignment.Near;
                s.LineAlignment = StringAlignment.Center;
                if (e.CellValue == null)
                {
                    return;
                }
                DataRowView drv = dbGridView.GetRow(e.RowHandle) as DataRowView;
                //取得病人名字
                string patname = drv["name"].ToString().Trim();
                if (e.Column.FieldName == "NAME")
                {
                    if (patname.Contains("婴儿"))
                    {
                        Region oldRegion = e.Graphics.Clip;
                        e.Graphics.Clip = new Region(e.Bounds);

                        e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                        e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red,
                            new RectangleF(e.Bounds.Location, new SizeF(300, e.Bounds.Height)), s);

                        e.Graphics.Clip = oldRegion;
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }


    }
}