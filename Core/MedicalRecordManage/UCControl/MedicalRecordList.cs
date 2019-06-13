using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Service;
using DrectSoft.Wordbook;
using MedicalRecordManage.Object;
using MedicalRecordManage.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MedicalRecordManage.UCControl
{
    public partial class MedicalRecordList : DevExpress.XtraEditors.XtraUserControl//, IEMREditor
    {
        //IEmrHost m_app;

        #region 相关方法
        public MedicalRecordList()
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
                this.dateAdmitStart.DateTime = System.DateTime.Today.AddMonths(-2);
                this.dateAdmitEnd.DateTime = System.DateTime.Today;
                this.dateLeaveStart.DateTime = System.DateTime.Today.AddMonths(-2);
                this.dateLeaveEnd.DateTime = System.DateTime.Today;

            }
            catch (Exception)
            {

                throw;
            }
            //初始化下拉框
        }

        private void InitializeButtonStatus()
        {
            try
            {
                int lockstatus = ComponentCommand.GetLockStatusValue(this.cbxLockStatus.Text);

                if (lockstatus == 4700)
                {//未完成
                    this.btn_reback.Enabled = false;
                    this.btnCancel.Enabled = false;
                    this.simpleButton_BackCommit.Enabled = false;
                }
                else if (lockstatus == 4701)
                {//已归档
                    this.btn_reback.Enabled = false;
                    this.btnCancel.Enabled = false;
                    this.simpleButton_BackCommit.Enabled = false;
                }
                else if (lockstatus == 4702)
                {//撤销归档
                    this.btn_reback.Enabled = true;
                    this.btnCancel.Enabled = false;
                    this.simpleButton_BackCommit.Enabled = false;
                }
                else if (lockstatus == 4704)
                {//已完成
                    this.btn_reback.Enabled = false;
                    this.btnCancel.Enabled = false;
                    this.simpleButton_BackCommit.Enabled = false;
                }
                else if (lockstatus == 4706)
                {//已提交
                    this.btn_reback.Enabled = true;
                    this.btnCancel.Enabled = false;
                    this.simpleButton_BackCommit.Enabled = true;
                }
                else if (lockstatus == 4707)
                {//已提交
                    this.btn_reback.Enabled = true;
                    this.btnCancel.Enabled = false;
                    this.simpleButton_BackCommit.Enabled = true;
                }
                else if (lockstatus == 4705)
                {//科室质控
                    this.btn_reback.Enabled = false;
                    this.btnCancel.Enabled = false;
                    this.simpleButton_BackCommit.Enabled = false;
                }
                else
                {
                    this.btn_reback.Enabled = true;
                    this.btnCancel.Enabled = false;
                    this.simpleButton_BackCommit.Enabled = false;
                }
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
                    dbGridView.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                }
                //selection = new CGridCheckMarksSelection(this.dbGridView);//把多选框绑定到你指定的grid
                //selection.CheckMarkColumn.VisibleIndex = 0;//使多选框排第一列
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 校验时间
        /// </summary>
        /// <returns></returns>
        private bool CheckDate()
        {
            try
            {
                if (this.dateAdmitStart.DateTime > this.dateAdmitEnd.DateTime)
                {
                    MessageBox.Show("入院起始日期不能大于结束日期", "信息提示");
                    this.dateAdmitStart.Focus();
                    return false;
                }
                else if (this.dateLeaveStart.DateTime > this.dateLeaveEnd.DateTime)
                {
                    MessageBox.Show("出院起始日期不能大于结束日期", "信息提示");
                    this.dateLeaveStart.Focus();
                    return false;
                }
                if (rdbtnOutPat.Checked)
                {
                    if (this.dateAdmitStart.DateTime > this.dateLeaveStart.DateTime)
                    {
                        MessageBox.Show("入院起始日期不能大于出院开始日期", "信息提示");
                        this.dateAdmitStart.Focus();
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadEmrContent(string noofinpat)
        {
            try
            {
                MedicalRecordManage.UI.EmrBrowerDlg frm = new MedicalRecordManage.UI.EmrBrowerDlg(noofinpat, SqlUtil.App, FloderState.None);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Modify by xlb 2013-05-29 同一天数据检索不出的问题
        /// </summary>
        /// 增加参数进行查询
        private void LoadData(string serachtype)
        {
            try
            {
                string valueStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("AutoScoreMainpage");
                if (serachtype == "inhos")
                {
                    GoSerachInHosPatInfo(valueStr);
                }
                if (serachtype == "outhos")
                {
                    GoSerachOutHosPatInfo(valueStr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// /在院病人进行查询
        /// </summary>
        /// <param name="valueStr"></param>
        private void GoSerachInHosPatInfo(string valueStr)
        {
            try
            {
                string sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_InHOSINP_LONG;
                if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
                {
                    sql = DaoCommand._SELECT_MEDICAL_RECORD_InHOSINP_LONG_ZY;
                }
                List<DbParameter> sqlParams = new List<DbParameter>();
                if (this.txtName.Text.Trim() != null && this.txtName.Text.Trim() != "")
                {
                    sql = sql + " and i.name like '%'||@name||'%' ";
                    SqlParameter param1 = new SqlParameter("@name", SqlDbType.VarChar);
                    param1.Value = this.txtName.Text.Trim();
                    sqlParams.Add(param1);
                }
                if (this.txtNumber.Text.Trim() != null && this.txtNumber.Text.Trim() != "")
                {
                    sql = sql + " and i.patid like '%'||@patId||'%'";
                    SqlParameter param2 = new SqlParameter("@patId", SqlDbType.VarChar);
                    param2.Value = this.txtNumber.Text.Trim();
                    sqlParams.Add(param2);
                }

                if (this.lookUpEditorDepartment.Text != null && this.lookUpEditorDepartment.Text.Trim() != "")
                {
                    if (lookUpEditorDepartment.CodeValue != "0000")
                    {
                        sql = sql + " and i.outhosdept = @code ";
                        SqlParameter param5 = new SqlParameter("@code", SqlDbType.VarChar);
                        param5.Value = lookUpEditorDepartment.CodeValue;
                        sqlParams.Add(param5);
                    }
                }

                //add by ck 2013-8-26
                if (this.lookUpEditorDoctor.Text != null && this.lookUpEditorDoctor.Text.Trim() != "")
                {
                    sql = sql + " and i.resident = @inpatientID";
                    SqlParameter param4 = new SqlParameter("@inpatientID", SqlDbType.VarChar);
                    param4.Value = lookUpEditorDoctor.CodeValue;
                    sqlParams.Add(param4);
                }//主治医生

                if (this.dateAdmitStart.Text.Trim() != null && this.dateAdmitStart.Text.Trim() != "")
                {
                    string ds = this.dateAdmitStart.DateTime.ToString("yyyy-MM-dd 00:00:00");//开始时间默认当天的0点
                    sql = sql + " and i.admitdate >= @ds ";
                    SqlParameter param6 = new SqlParameter("@ds", SqlDbType.VarChar);
                    param6.Value = ds;
                    sqlParams.Add(param6);
                }
                if (this.dateAdmitEnd.Text.Trim() != null && this.dateAdmitEnd.Text.Trim() != "")
                {
                    string de = this.dateAdmitEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");//结束时间默认当天23:59:59
                    sql = sql + " and i.admitdate <= @de ";
                    SqlParameter param7 = new SqlParameter("@de", SqlDbType.VarChar);
                    param7.Value = de;
                    sqlParams.Add(param7);
                }

                if (this.cbxLockStatus.Text.Trim() != null && this.cbxLockStatus.Text.Trim() != "")
                {
                    int lockstatus = ComponentCommand.GetLockStatusValue(this.cbxLockStatus.Text);

                    if (lockstatus == 4700)
                    {
                        sql = sql + " and (i.islock = @lockstatus  OR  i.islock is null)";
                    }
                    else
                    {
                        sql = sql + " and nvl(i.islock,4700) = @lockstatus";
                    }
                    SqlParameter param10 = new SqlParameter("@lockstatus", SqlDbType.VarChar);

                    param10.Value = lockstatus;

                    sqlParams.Add(param10);
                }
                DataTable dataTable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
                //压缩数据源生成新的数据源
                //压缩DateSet中的记录，将住院诊断信息合并
                ComponentCommand.ImpressDataSet(ref dataTable, "noofinpat", "cyzd", "operation_name");
                //add by zjy 2013-6-17
                string wheresql = "1=1 ";
                if (this.lookUpEditorDiagnosis.Text.Trim() != null && this.lookUpEditorDiagnosis.Text.Trim() != "")
                {
                    wheresql = wheresql + " and cyzd like '%" + this.lookUpEditorDiagnosis.Text.Trim() + "%'";
                }
                if (this.lookUpEditorOperation.Text.Trim() != null && this.lookUpEditorOperation.Text.Trim() != "")
                {
                    wheresql = wheresql + " and operation_name like '%" + this.lookUpEditorOperation.Text.Trim().Trim() + "%'";
                }

                DataRow[] dr = dataTable.Select(wheresql);
                if (dr != null && dr.Length > 0)
                {
                    DataTable tableRecord = ToDataTable(dr);
                    string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
                    for (int i = 0; i < tableRecord.Rows.Count; i++)
                    {
                        ResultName = SqlUtil.GetPatsBabyContent(SqlUtil.App, tableRecord.Rows[i]["noofinpat"].ToString());
                        tableRecord.Rows[i]["Name"] = ResultName;
                        if (tableRecord.Rows[i]["isbaby"].ToString() == "1")
                        {
                            tableRecord.Rows[i]["PATID"] = SqlUtil.GetPatsBabyMother(dataTable.Rows[i]["mother"].ToString());
                        }
                    }
                    //绑定控件
                    this.dbGrid.DataSource = tableRecord;
                    lblTip.Text = "共" + tableRecord.Rows.Count.ToString() + "条";
                }
                else
                {
                    this.dbGrid.DataSource = null;
                    lblTip.Text = "共0条";
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 出院病人查询方法
        /// </summary>
        /// <param name="valueStr"></param>
        private void GoSerachOutHosPatInfo(string valueStr)
        {
            try
            {
                string sql = MedicalRecordManage.Object.DaoCommand._SELECT_MEDICAL_RECORD_INPATIENT_LONG;
                if (valueStr.ToLower().Contains("iem_mainpage_diagnosis_sx"))
                {
                    sql = DaoCommand._SELECT_MEDICAL_RECORD_INPATIENT_LONG_ZY;
                }
                List<DbParameter> sqlParams = new List<DbParameter>();
                if (this.txtName.Text.Trim() != null && this.txtName.Text.Trim() != "")
                {
                    sql = sql + " and i.name like '%'||@name||'%' ";
                    SqlParameter param1 = new SqlParameter("@name", SqlDbType.VarChar);
                    param1.Value = this.txtName.Text.Trim();
                    sqlParams.Add(param1);
                }
                if (this.txtNumber.Text.Trim() != null && this.txtNumber.Text.Trim() != "")
                {
                    sql = sql + " and i.patid like '%'||@patId||'%'";
                    SqlParameter param2 = new SqlParameter("@patId", SqlDbType.VarChar);
                    param2.Value = this.txtNumber.Text.Trim();
                    sqlParams.Add(param2);
                }

                if (this.lookUpEditorDepartment.Text != null && this.lookUpEditorDepartment.Text.Trim() != "")
                {
                    if (lookUpEditorDepartment.CodeValue != "0000")
                    {
                        sql = sql + " and i.outhosdept = @code ";
                        SqlParameter param5 = new SqlParameter("@code", SqlDbType.VarChar);
                        param5.Value = lookUpEditorDepartment.CodeValue;
                        sqlParams.Add(param5);
                    }
                }

                //add by ck 2013-8-26
                if (this.lookUpEditorDoctor.Text != null && this.lookUpEditorDoctor.Text.Trim() != "")
                {
                    sql = sql + " and i.resident = @inpatientID";
                    SqlParameter param4 = new SqlParameter("@inpatientID", SqlDbType.VarChar);
                    param4.Value = lookUpEditorDoctor.CodeValue;
                    sqlParams.Add(param4);
                }//主治医生

                if (this.dateAdmitStart.Text.Trim() != null && this.dateAdmitStart.Text.Trim() != "")
                {
                    string ds = this.dateAdmitStart.DateTime.ToString("yyyy-MM-dd 00:00:00");//开始时间默认当天的0点
                    sql = sql + " and i.admitdate >= @ds ";
                    SqlParameter param6 = new SqlParameter("@ds", SqlDbType.VarChar);
                    param6.Value = ds;
                    sqlParams.Add(param6);
                }
                if (this.dateAdmitEnd.Text.Trim() != null && this.dateAdmitEnd.Text.Trim() != "")
                {
                    string de = this.dateAdmitEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");//结束时间默认当天23:59:59
                    sql = sql + " and i.admitdate <= @de ";
                    SqlParameter param7 = new SqlParameter("@de", SqlDbType.VarChar);
                    param7.Value = de;
                    sqlParams.Add(param7);
                }
                if (this.dateLeaveStart.Text.Trim() != null && this.dateLeaveStart.Text.Trim() != "")
                {
                    string ds = this.dateLeaveStart.DateTime.ToString("yyyy-MM-dd 00:00:00");
                    sql = sql + " and i.outhosdate >= @dss";
                    SqlParameter param8 = new SqlParameter("@dss", SqlDbType.VarChar);
                    param8.Value = ds;
                    sqlParams.Add(param8);
                }
                if (this.dateLeaveEnd.Text.Trim() != null && this.dateLeaveEnd.Text.Trim() != "")
                {
                    string de = this.dateLeaveEnd.DateTime.ToString("yyyy-MM-dd 23:59:59");
                    sql = sql + " and i.outhosdate <= @dee";
                    SqlParameter param9 = new SqlParameter("@dee", SqlDbType.VarChar);
                    param9.Value = de;
                    sqlParams.Add(param9);
                }
                if (this.cbxLockStatus.Text.Trim() != null && this.cbxLockStatus.Text.Trim() != "")
                {
                    int lockstatus = ComponentCommand.GetLockStatusValue(this.cbxLockStatus.Text);

                    if (lockstatus == 4700)
                    {
                        sql = sql + " and (i.islock = @lockstatus  OR  i.islock is null)";
                    }
                    else
                    {
                        sql = sql + " and (i.islock = @lockstatus or @lockstatus =0) ";
                    }
                    SqlParameter param10 = new SqlParameter("@lockstatus", SqlDbType.VarChar);

                    param10.Value = lockstatus;

                    sqlParams.Add(param10);
                }
                DataTable dataTable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
                //压缩数据源生成新的数据源
                //压缩DateSet中的记录，将住院诊断信息合并
                ComponentCommand.ImpressDataSet(ref dataTable, "noofinpat", "cyzd", "operation_name");
                //add by zjy 2013-6-17
                string wheresql = "1=1 ";
                if (this.lookUpEditorDiagnosis.Text.Trim() != null && this.lookUpEditorDiagnosis.Text.Trim() != "")
                {
                    wheresql = wheresql + " and cyzd like '%" + this.lookUpEditorDiagnosis.Text.Trim() + "%'";
                }
                if (this.lookUpEditorOperation.Text.Trim() != null && this.lookUpEditorOperation.Text.Trim() != "")
                {
                    wheresql = wheresql + " and operation_name like '%" + this.lookUpEditorOperation.Text.Trim().Trim() + "%'";
                }

                DataRow[] dr = dataTable.Select(wheresql);
                if (dr != null && dr.Length > 0)
                {
                    DataTable tableRecord = ToDataTable(dr);
                    string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
                    for (int i = 0; i < tableRecord.Rows.Count; i++)
                    {
                        ResultName = SqlUtil.GetPatsBabyContent(SqlUtil.App, tableRecord.Rows[i]["noofinpat"].ToString());
                        tableRecord.Rows[i]["Name"] = ResultName;
                        if (tableRecord.Rows[i]["isbaby"].ToString() == "1")
                        {
                            tableRecord.Rows[i]["PATID"] = SqlUtil.GetPatsBabyMother(dataTable.Rows[i]["mother"].ToString());
                        }
                    }
                    //绑定控件
                    this.dbGrid.DataSource = tableRecord;
                    lblTip.Text = "共" + tableRecord.Rows.Count.ToString() + "条";
                }
                else
                {
                    this.dbGrid.DataSource = null;
                    lblTip.Text = "共0条";
                }
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }

        }
        /// <summary>
        /// DataRow[]转换DataTable
        /// </summary>
        /// add by zjy 2013-6-17
        /// <param name="rows"></param>
        /// <returns></returns>
        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();  // 复制DataRow的表结构
                foreach (DataRow row in rows)
                    tmp.Rows.Add(row.ItemArray);  // 将DataRow添加到DataTable中
                return tmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 相关事件

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeParameters();
                this.txtName.Text = "";
                this.txtNumber.Text = "";
                this.lookUpEditorDepartment.CodeValue = "0000";
                //this.lookUpEditorDepartment.Text = "";
                this.lookUpEditorDoctor.Text = "";
                this.lookUpEditorDiagnosis.Text = "";
                this.lookUpEditorOperation.Text = "";
                this.dbGrid.DataSource = null;
                this.txtName.Focus();
            }
            catch (Exception)
            {

                throw;
            }
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
                if (CheckDate())
                {
                    DevExpress.Utils.WaitDialogForm m_WaitDialog = new DevExpress.Utils.WaitDialogForm("正在查询数据...", "请稍等");
                    InitializeButtonStatus();
                    string serachtype = string.Empty;
                    if (rdbtnInPat.Checked)
                    {
                        serachtype = "inhos";
                    }
                    if (rdbtnOutPat.Checked)
                    {
                        serachtype = "outhos";
                    }
                    LoadData(serachtype);


                    m_WaitDialog.Hide();
                }
            }
            catch (Exception ex)
            {

                MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MedicalRecordList_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeParameters();
                InitializeGrid();
                DrectSoft.Common.DS_Common.CancelMenu(this.panelHead, contextMenuStrip1);
                ComponentCommand.InitializeDepartment(ref this.lookUpEditorDepartment, ref this.lookUpWindowDepartment);
                ComponentCommand.InitializeDiagnosis(ref this.lookUpEditorDiagnosis, ref this.lookUpWindowDoctor);
                ComponentCommand.InitializeOperation(ref this.lookUpEditorOperation, ref this.lookUpWindowOperation);
                //this.lookUpWindowLock
                //  ComponentCommand.InitializeDoctor(ref this.lookUpEditorDoctor, ref this.lookUpWindowDoctor);
                //add by ck 2013-8-26
                this.txtName.Focus();
                this.rdbtnOutPat.Checked = true;//默认勾中出院的
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region  =============处理键盘事件 Enter同Tab=========================

        /// <summary>
        /// 键盘Enter事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void MedicalRecordList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.SelectNextControl(this.ActiveControl, true, true, true, false);
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

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateAdmitStart_KeyPress(object sender, KeyPressEventArgs e)
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
        private void dateAdmitEnd_KeyPress(object sender, KeyPressEventArgs e)
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
        private void dateLeaveStart_KeyPress(object sender, KeyPressEventArgs e)
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
        private void dateLeaveEnd_KeyPress(object sender, KeyPressEventArgs e)
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

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorDiagnosis_KeyPress(object sender, KeyPressEventArgs e)
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
        /// Enter同Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorOperation_KeyPress(object sender, KeyPressEventArgs e)
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

        #endregion

        /// <summary>
        /// 列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                //if (dbGridView.FocusedRowHandle < 0)
                //{
                //    return;
                //}
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
                //        SqlUtil.App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                //        //LoadEmrContent(noofinpat);
                //    }
                //}
                //else
                //{
                //    SqlUtil.App.ChoosePatient(Convert.ToDecimal(noofinpat));
                //    //LoadEmrContent(noofinpat);
                //    SqlUtil.App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
        //归档
        private void btn_reback_Click(object sender, EventArgs e)
        {
            try
            {
                int fouceRowIndex = dbGridView.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    dbGrid.Focus();
                    SqlUtil.App.CustomMessageBox.MessageShow("请选择一条病人记录");
                    return;
                }
                DataRow foucesRow = dbGridView.GetDataRow(fouceRowIndex);
                if (null == foucesRow || null == foucesRow["NOOFINPAT"])
                {
                    return;
                }
                int noofinpat = int.Parse(foucesRow["NOOFINPAT"].ToString().Trim());
                string statusname = foucesRow["GDZT"].ToString().Trim();
                if (!(statusname == "已提交" || statusname == "补写提交" || statusname == "撤销归档"))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该状态病历不能直接归档");
                    return;
                }
                #region 注释验证是否有病例
                //DataTable dt = DS_SqlService.GetRecordsByNoofinpat(noofinpat);
                //if (null == dt || dt.Rows.Count == 0)
                //{
                //    Common.Ctrs.DLG.MessageBox.Show(foucesRow["NAME"] + " 没有病历，无法归档。");
                //    return;
                //}
                #endregion
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要归档 " + foucesRow["NAME"] + " 的病历吗？", "归档病历", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                // DS_BaseService.CheckRecordRebacked(noofinpat.ToString())
                if (foucesRow["islock"].ToString() != "4701")
                {
                    int num = DS_SqlService.SetRecordsRebacked(noofinpat.ToString());
                    if (num > 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("归档成功");
                        dbGridView.DeleteRow(dbGridView.FocusedRowHandle);
                        return;
                    }
                }
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人已归档。");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                int fouceRowIndex = dbGridView.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    dbGrid.Focus();
                    SqlUtil.App.CustomMessageBox.MessageShow("请选择一条病人记录");
                    return;
                }
                DataRow foucesRow = dbGridView.GetDataRow(fouceRowIndex);
                if (null == foucesRow || null == foucesRow["NOOFINPAT"])
                {
                    return;
                }
                int noofinpat = int.Parse(foucesRow["NOOFINPAT"].ToString().Trim());
                string statusname = foucesRow["GDZT"].ToString().Trim();
                if (!(statusname == "已归档"))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该状态病历不能撤销归档");
                    return;
                }
                //DataTable dt = DS_SqlService.GetRecordsByNoofinpat(noofinpat);
                //if (null == dt || dt.Rows.Count == 0)
                //{
                //    Common.Ctrs.DLG.MessageBox.Show(foucesRow["NAME"] + " 没有病历，无法撤销归档。");
                //    return;
                //}

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要撤销 " + foucesRow["NAME"] + " 的病历归档吗？", "撤销归档病历", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                //DS_BaseService.CheckRecordRebacked(noofinpat.ToString())
                if (foucesRow["islock"].ToString() == "4701")
                {
                    int num = DS_SqlService.SetRecordsCancel(noofinpat.ToString());
                    if (num > 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("撤销归档成功");
                        dbGridView.DeleteRow(dbGridView.FocusedRowHandle);
                        return;
                    }
                }
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人未归档无法撤销。");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 通过选择病人状态控制相关控件的可见性
        /// add by ywk 2013年9月5日 18:54:57
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbtnInPat_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.dbGrid.DataSource = null;
                if (rdbtnInPat.Checked)
                {
                    lblIslock.Visible = false;
                    cbxLockStatus.EditValue = "未完成";
                    cbxLockStatus.Visible = false;
                    labelControl5.Visible = false;
                    dateLeaveStart.Visible = false;
                    dateLeaveEnd.Visible = false;
                    labelControl9.Visible = false;
                    labelControl6.Visible = false;
                    lookUpEditorDiagnosis.Visible = false;
                    btn_reback.Visible = false;
                    btnCancel.Visible = false;
                    dbGridView.Columns[5].Visible = false;
                    dbGridView.Columns[6].Visible = false;
                    dbGridView.Columns[9].Visible = false;
                    dbGridView.Columns[10].Visible = false;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 勾选了出院病人 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdbtnOutPat_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.dbGrid.DataSource = null;
                if (rdbtnOutPat.Checked)
                {
                    lblIslock.Visible = true;
                    cbxLockStatus.EditValue = "已提交";
                    cbxLockStatus.Visible = true;
                    labelControl5.Visible = true;
                    dateLeaveStart.Visible = true;
                    dateLeaveEnd.Visible = true;
                    labelControl9.Visible = true;
                    labelControl6.Visible = true;
                    lookUpEditorDiagnosis.Visible = true;
                    btn_reback.Visible = true;
                    btnCancel.Visible = false;
                    dbGridView.Columns[5].Visible = true;
                    dbGridView.Columns[6].Visible = true;
                    dbGridView.Columns[9].Visible = true;
                    dbGridView.Columns[10].Visible = true;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void simpleButton_BackCommit_Click(object sender, EventArgs e)
        {
            try
            {
                int fouceRowIndex = dbGridView.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    dbGrid.Focus();
                    SqlUtil.App.CustomMessageBox.MessageShow("请选择一条病人记录");
                    return;
                }
                DataRow foucesRow = dbGridView.GetDataRow(fouceRowIndex);
                if (null == foucesRow || null == foucesRow["NOOFINPAT"])
                {
                    return;
                }
                int noofinpat = int.Parse(foucesRow["NOOFINPAT"].ToString().Trim());

                //DataTable dt = DS_SqlService.GetRecordsByNoofinpat(noofinpat);
                //if (null == dt || dt.Rows.Count == 0)
                //{
                //    Common.Ctrs.DLG.MessageBox.Show(foucesRow["NAME"] + " 没有病历，无法撤销归档。");
                //    return;
                //}

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要撤销 " + foucesRow["NAME"] + " 的病历提交吗？", "撤销提交病历", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                //DS_BaseService.CheckRecordRebacked(noofinpat.ToString())
                if (foucesRow["islock"].ToString() == "4707" || foucesRow["islock"].ToString() == "4706")
                {
                    int num = DS_SqlService.SetRecordsCancelCommit(noofinpat.ToString());
                    if (num > 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("撤销提交成功");
                        dbGridView.DeleteRow(dbGridView.FocusedRowHandle);
                        return;
                    }
                }
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人未提交无法撤销。");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void lookUpEditorDepartment_CodeValueChanged(object sender, EventArgs e)
        {
            InitDoc(lookUpEditorDepartment.CodeValue);
        }

        /// <summary>
        /// 初始化医生列表
        /// </summary>
        private void InitDoc(string deptCode)
        {
            //  lookUpWindowDoctor.SqlHelper = SqlHelper.DS_SqlHelper;

            DataTable docs = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable("select ID,NAME,PY,WB from users where valid != 0  and (deptid='" + deptCode + "') order by name ");

            docs.Columns["ID"].Caption = "医生代码";
            docs.Columns["NAME"].Caption = "医生名称";
            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("ID", 60);
            cols.Add("NAME", 90);

            SqlWordbook docWorkBook = new SqlWordbook("querybook", docs, "ID", "NAME", cols, "ID//NAME//PY//WB");
            lookUpEditorDoctor.SqlWordbook = docWorkBook;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            PatientStatus ps = new PatientStatus();
            ps.ShowDialog();
        }



    }
}

