using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System.Data.SqlClient;
using System.Collections;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common;
using System.Drawing.Printing;
using DrectSoft.Core.Consultation;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Consultation.NEW;
using DrectSoft.DSSqlHelper;
using DrectSoft.Core.Consultation.NEW.Enum;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// 会诊明细统计界面
    /// </summary>
    public partial class UCRecord : DevExpress.XtraEditors.XtraUserControl
    {
        private IEmrHost m_app;

        public UCRecord()
        {
            InitializeComponent();
        }

        public UCRecord(IEmrHost app)
            : this()
        {
            try
            {
                m_app = app;
                BindLookUpEditorData();
                InitGridControl();
                InitDateEdit();
                DS_SqlHelper.CreateSqlHelper();
                #region 注销 By Wangji  2013 1 30
                //BindDataSource(Search());      edit by wangj 2013 1 30 加载时不查询
                //Search();
                //this.lookUpEditorApplyDept.Focus();
                //Application.AddMessageFilter(new MessageFilter());
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 注销

        //public class MessageFilter : IMessageFilter
        //{
        //    private readonly static IList StopControlTypes =
        //            new Type[] { typeof(TextEdit), typeof(LookUpEdit),typeof(TextBox) };
        //    private const int WM_KEYDOWN = 0x100;

        //    public MessageFilter()
        //    {
        //    }
        //    public bool PreFilterMessage(ref Message m)
        //    {
        //        if (m.Msg == WM_KEYDOWN && Control.ModifierKeys == Keys.None && m.WParam.ToInt32() == (int)Keys.Enter)
        //        {
        //            Control ctr = Control.FromHandle(m.HWnd);
        //            if (ctr == null)
        //            {
        //                ctr = Control.FromChildHandle(m.HWnd);
        //            }

        //            if (ctr != null && StopControlTypes.IndexOf(ctr.GetType()) >= 0)
        //            {
        //                SendKeys.Send("{Tab}");
        //                return true;
        //            }
        //        }

        //        return false;
        //    }
        //} 

        #endregion

        /// <summary>
        /// 初始化列表
        /// </summary>
        private void InitGridControl()
        {
            try
            {
                gridViewList.OptionsSelection.EnableAppearanceFocusedRow = true;
                gridViewList.OptionsSelection.EnableAppearanceFocusedCell = false; ;
                gridViewList.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
                gridViewList.DoubleClick += new EventHandler(gridViewList_DoubleClick);
                gridViewList.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gridViewList_CustomDrawCell);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化时间
        /// </summary>
        private void InitDateEdit()
        {
            try
            {
                dateEditConsultDateBegin.EditValue = System.DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
                dateEditConsultDateEnd.EditValue = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询方法
        /// 起始时间为空时仍减一天没问题？
        /// 解决起始日期为空报异常结束时间不输入检索不出数据问题
        /// Modify by xlb 2013-06-28
        /// </summary>
        /// <returns></returns>
        private DataTable Search()
        {
            try
            {
                DS_SqlHelper.CreateSqlHelper();
                SqlParameter p_result = new SqlParameter("@result", SqlDbType.Structured);
                p_result.Direction = ParameterDirection.Output;
                SqlParameter[] sqlparams = new SqlParameter[]
                {
                    new SqlParameter("@ApplyDeptid",this.lookUpEditorApplyDept.CodeValue),
                    new SqlParameter("@EmployeeDeptid",this.lookUpEditorEmployeeDept.CodeValue),
                    new SqlParameter("@DateTimeBegin",this.dateEditConsultDateBegin.DateTime.ToString("yyyy-MM-dd")),
                    new SqlParameter("@DateTimeEnd",this.dateEditConsultDateEnd.DateTime.ToString("yyyy-MM-dd")),
                    p_result
                };
                DataTable dt = DS_SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetAllConsultion", sqlparams, CommandType.StoredProcedure);
                return dt;

                //string consultTimeBegin = dateEditConsultDateBegin.Text;
                //string consultTimeEnd = dateEditConsultDateEnd.Text;
                //string consultType = "";
                //string urgency = lookUpEditorUrgency.CodeValue;
                //string name = textEditName.Text;
                //string patientSN = textEditPatientSN.Text;
                //string bedCode = lookUpEditorBed.CodeValue;

                //DataTable dt = Dal.DataAccess.GetConsultationData("23", consultTimeBegin, consultTimeEnd, consultType, urgency, name, patientSN, bedCode, m_app.User.CurrentDeptId);
                //gridControlList.DataSource = dt;

                //m_app.PublicMethod.ConvertGridDataSourceUpper(gridViewList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        /// <param name="dt"></param>
        private void BindDataSource(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)// 添加 dt 的判空  tj 2013-1-14
                {
                    this.gridControlList.DataSource = null;
                    this.labPatCount.Text = "0";
                    MessageBox.Show("没有符合条件的记录");
                    return;
                }
                if (this.checkBoxOutTime.Checked)
                {
                    dt = ToDataTable(dt.Select(string.Format("STATEID = '6730' and PLANCONSULTTIME<= '{0}' ", DateTime.Now.ToString("yyyy-MM-dd hh:mm")))); //, 
                    //dt = ToDataTable(dt.Select(string.Format("PLANCONSULTTIME<= '{0}' ", DateTime.Now.AddDays(-2))));
                }
                if (dt == null || dt.Rows.Count == 0) // 添加 dt 的判空  tj 2013-1-14
                {
                    this.gridControlList.DataSource = null;
                    this.labPatCount.Text = "0";
                    MessageBox.Show("没有符合条件的记录");
                    return;
                }
                this.gridControlList.DataSource = dt;
                m_app.PublicMethod.ConvertGridDataSourceUpper(gridViewList);
                this.labPatCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 注销
        //private void refreshGridView()
        //{
        //    string filter = "CONSULTSTATUS = '待会诊' and PLANCONSULTTIME<= '{0}'  ";
        //    DataTable dt = gridControlList.DataSource as DataTable;
        //    if (dt != null)
        //    {
        //        dt.DefaultView.RowFilter = string.Format(filter, DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
        //    }
        //}
        #endregion

        /// <summary>
        /// 数组DataRow[]转化成表DataTable
        /// 王冀 2013 1 7
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();
                foreach (DataRow row in rows)
                    tmp.ImportRow(row);
                return tmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化LookUpEdit控件
        /// </summary>
        private void BindLookUpEditorData()
        {
            try
            {
                DrectSoft.Common.Library.LookUpWindow luApplyDept = new DrectSoft.Common.Library.LookUpWindow();
                DrectSoft.Common.Library.LookUpWindow luEmplyeeDept = new DrectSoft.Common.Library.LookUpWindow();
                luApplyDept.SqlHelper = m_app.SqlHelper;
                luEmplyeeDept.SqlHelper = m_app.SqlHelper;
                lookUpEditorApplyDept.ListWindow = luApplyDept;
                lookUpEditorEmployeeDept.ListWindow = luEmplyeeDept;
                DataTable Dept = m_app.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
                     new SqlParameter[] { new SqlParameter("@GetType", "1") }, CommandType.StoredProcedure);

                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 70);
                cols.Add("NAME", 80);

                SqlWordbook deptWordBook = new SqlWordbook("querydept", Dept, "ID", "NAME", cols, "ID//NAME");
                lookUpEditorApplyDept.SqlWordbook = deptWordBook;
                lookUpEditorEmployeeDept.SqlWordbook = deptWordBook;
                lookUpEditorApplyDept.CodeValue = m_app.User.CurrentDeptId;        //王冀  2013  2  22  初始值设置为用户所在科室
                //lookUpEditorApplyDept.SelectedText = "妇科";
                //lookUpEditorEmployeeDept.SelectedText = "妇科";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 绑定当前病区的床位
        private void BindBed()
        {
            //lookUpEditorBed.Kind = WordbookKind.Sql;
            //lookUpEditorBed.ListWindow = lookUpWindowBed;
            //BindBedWordBook(GetConsultationData("7", m_app.User.CurrentWardId));
        }

        private void BindBedWordBook(DataTable dataTableData)
        {
            //for (int i = 0; i < dataTableData.Columns.Count; i++)
            //{
            //    if (dataTableData.Columns[i].ColumnName.ToLower().Trim() == "id")
            //    {
            //        dataTableData.Columns[i].Caption = "床位号";
            //    }
            //}

            //Dictionary<string, int> colWidths = new Dictionary<string, int>();
            //colWidths.Add("ID", 60);
            //SqlWordbook wordBook = new SqlWordbook("Bed", dataTableData, "ID", "ID", colWidths, "ID");
            //lookUpEditorBed.SqlWordbook = wordBook;
        }
        #endregion

        #region 已注销

        //private DataTable GetConsultationData(string typeID, string param)
        //{
        //    //if (Dal.DataAccess.App == null)
        //    //{
        //    //    Dal.DataAccess.App = m_app;
        //    //}
        //    //DataTable dataTableConsultationData = new DataTable();
        //    //dataTableConsultationData = Dal.DataAccess.GetConsultationData("", typeID, param);
        //    //return dataTableConsultationData;
        //}
        #endregion

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dateEditConsultDateBegin.DateTime > this.dateEditConsultDateEnd.DateTime)
                {
                    MessageBox.Show("起始时间不能大于结束时间");
                    return;
                }
                BindDataSource(Search());
                //if ((this.gridViewList.DataSource as DataTable) == null || (this.gridViewList.DataSource as DataTable).Rows.Count == 0)
                //if (((DataView)this.gridViewList.DataSource).ToTable() == null || ((DataView)this.gridViewList.DataSource).ToTable().Rows.Count == 0)
                //{
                //    Common.Ctrs.DLG.MessageBox.Show("没有符合条件的记录");
                //}
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// Add by xlb 2013-04-01
        /// 绘制单元格样式事件
        /// 已缴费记录前景色默认绿色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                DataRow dataRow = gridViewList.GetDataRow(e.RowHandle) as DataRow;
                if (dataRow == null)
                {
                    return;
                }
                if (dataRow["ISPAY"].ToString().Equals("1"))//已缴费
                {
                    e.Appearance.ForeColor = Color.LightSeaGreen;
                }
                else
                {
                    e.Appearance.ForeColor = Color.Red;//未缴费的记录前景色默认红色
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击查看会诊信息
        /// Modify by xlb 2013-04-01
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridViewList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                #region 已注销 by xlb 2013-04-01

                //if (gridViewList.FocusedRowHandle >= 0)
                //{
                //    DataRow dr = gridViewList.GetDataRow(gridViewList.FocusedRowHandle);
                //    if (dr != null)
                //    {
                //        string noOfFirstPage = dr["NoOfInpat"].ToString();
                //        string consultTypeID = dr["ConsultTypeID"].ToString();
                //        string consultApplySn = dr["ConsultApplySn"].ToString();

                //        //if (consultTypeID == Convert.ToString((int)ConsultType.One))
                //        //{
                //        //    FormRecordForOne formRecordForOne = new FormRecordForOne(noOfFirstPage, m_app, consultApplySn);
                //        //    formRecordForOne.StartPosition = FormStartPosition.CenterParent;
                //        //    formRecordForOne.ShowDialog();
                //        //}
                //        //else
                //        //{
                //        FormRecordForMultiply formRecrodForMultiply = new FormRecordForMultiply(noOfFirstPage, m_app, consultApplySn);
                //        formRecrodForMultiply.StartPosition = FormStartPosition.CenterParent;
                //        if (dr["APPLYUSER"].ToString() != m_app.User.Id)
                //            formRecrodForMultiply.ReadOnlyControl();
                //        formRecrodForMultiply.ShowDialog();
                //        //}
                //        //Search();
                //    }
                //}

                #endregion
                GridHitInfo gridHit = gridViewList.CalcHitInfo(gridControlList.PointToClient(Cursor.Position));
                if (gridHit.RowHandle < 0)
                {
                    return;
                }
                ShowConsultInfo(gridHit.RowHandle);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 导出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //gridControlList.ExportToXls(
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "导出";
                saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
                DialogResult dialogResult = saveFileDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    //gridViewCardList.ExportToXls(saveFileDialog.FileName);
                    gridControlList.ExportToXls(saveFileDialog.FileName, true);

                    m_app.CustomMessageBox.MessageShow("导出成功！");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 键盘事件 Enter同Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 快捷键Q触发查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Q && e.Control)
                {
                    this.simpleButtonSearch.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewList_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        /// <summary>
        /// 重置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                //lookUpEditorApplyDept.CodeValue = "0000";
                lookUpEditorApplyDept.CodeValue = m_app.User.CurrentDeptId;        //王冀  2013  2  25  初始值设置为用户所在科室
                lookUpEditorEmployeeDept.CodeValue = "";
                InitDateEdit();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void ResetControl()
        {
            InitDateEdit();
            this.gridControlList.DataSource = null;
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCRecord_Load(object sender, EventArgs e)
        {
            try
            {
                this.lookUpEditorApplyDept.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonPrint1_Click(object sender, EventArgs e)
        {
            try
            {
                m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体大小改变时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCRecord_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(25, this.Height - 30);
                this.labPatCount.Location = new Point(76, this.Height - 30);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 通过申请单号获取实际受邀记录信息
        /// Add by xlb 2013-03-19
        /// </summary>
        /// <param name="consultApplySN"></param>
        /// <returns></returns>
        private DataTable GetConsultationDepartmentBySN(string consultApplySN)
        {
            try
            {
                string sqlGetConsultationDepartment = "select * from consultrecorddepartment where consultapplysn = '{0}' and valid = '1'";
                return DS_SqlHelper.ExecuteDataTable(string.Format(sqlGetConsultationDepartment, consultApplySN), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据会诊信息状态打开相应的界面
        /// Add by xlb 2013-04-01
        /// </summary>
        /// <param name="rowHandel"></param>
        private void ShowConsultInfo(int rowHandel)
        {
            try
            {
                DataRow dataRow = gridViewList.GetDataRow(rowHandel) as DataRow;
                if (dataRow == null)
                {
                    return;
                }
                string noOfFirstPage = dataRow["NoOfInpat"].ToString();
                string consultTypeID = dataRow["ConsultTypeID"].ToString();
                string consultApplySn = dataRow["ConsultApplySn"].ToString();
                string stateID = dataRow["StateID"].ToString();
                FrmRecordConsult frmRecord = new FrmRecordConsult();
                DataTable dtReord = GetConsultationDepartmentBySN(consultApplySn);

                if (stateID == Convert.ToString((int)DrectSoft.Core.Consultation.NEW.Enum.ConsultStatus.WaitApprove))//待审核
                {
                    FrmConsultForReview frmApprove = new FrmConsultForReview(noOfFirstPage, m_app, consultApplySn, true/*只读*/);
                    if (frmApprove == null)
                    {
                        return;
                    }
                    frmApprove.StartPosition = FormStartPosition.CenterParent;
                    frmApprove.ShowDialog();
                }
                else if (stateID == Convert.ToString((int)DrectSoft.Core.Consultation.NEW.Enum.ConsultStatus.WaitConsultation))//待会诊
                {
                    if (dtReord.Rows.Count > 1)
                    {

                        frmRecord = new FrmRecordConsult(noOfFirstPage, consultApplySn, m_app, true, ConsultRecordForWrite.MultiEmployee);
                    }
                    else
                    {
                        frmRecord = new FrmRecordConsult(noOfFirstPage, consultApplySn, m_app, true);
                    }
                    if (frmRecord == null)
                    {
                        return;
                    }
                    frmRecord.StartPosition = FormStartPosition.CenterParent;
                    frmRecord.ShowDialog();
                }
                else if (stateID == Convert.ToString((int)DrectSoft.Core.Consultation.NEW.Enum.ConsultStatus.CancelConsultion))//已取消
                {
                    FormApplyForMultiply frmapply = new FormApplyForMultiply(noOfFirstPage, m_app, consultApplySn, true);
                    if (frmapply == null)
                    {
                        return;
                    }
                    frmapply.StartPosition = FormStartPosition.CenterParent;
                    frmapply.ShowDialog();
                }
                else if (stateID == Convert.ToString((int)DrectSoft.Core.Consultation.NEW.Enum.ConsultStatus.Reject))//否决 弹出申请页面
                {
                    FormApplyForMultiply frmapply = new FormApplyForMultiply(noOfFirstPage, m_app, consultApplySn, true);
                    if (frmapply == null)
                    {
                        return;
                    }
                    frmapply.StartPosition = FormStartPosition.CenterParent;
                    frmapply.ShowDialog();
                }
                else if (stateID == Convert.ToString((int)DrectSoft.Core.Consultation.NEW.Enum.ConsultStatus.RecordeSave) || stateID == Convert.ToString((int)DrectSoft.Core.Consultation.NEW.Enum.ConsultStatus.RecordeComplete))//完成，保存
                {
                    if (dtReord.Rows.Count > 1)
                    {

                        frmRecord = new FrmRecordConsult(noOfFirstPage, consultApplySn, m_app, true, ConsultRecordForWrite.MultiEmployee);
                    }
                    else
                    {
                        frmRecord = new FrmRecordConsult(noOfFirstPage, consultApplySn, m_app, true);
                    }
                    if (frmRecord == null)
                    {
                        return;
                    }
                    frmRecord.StartPosition = FormStartPosition.CenterParent;
                    frmRecord.ShowDialog();
                }
                else if (stateID == Convert.ToString((int)DrectSoft.Core.Consultation.NEW.Enum.ConsultStatus.ApplySave))//会诊申请保存 弹出申请页面不可编辑
                {
                    FormApplyForMultiply frmapply = new FormApplyForMultiply(noOfFirstPage, m_app, consultApplySn, true);
                    if (frmapply == null)
                    {
                        return;
                    }
                    frmapply.StartPosition = FormStartPosition.CenterParent;
                    frmapply.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //原来双击会弹出两个框，就是因为后台注册了一个事件，UI界面又搞了个事件啊  二次更改by ywk 2013年8月28日 14:20:36
        ///// <summary>
        ///// 列表双击事件
        ///// Modify by xlb 2013-04-01
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void gridViewList_DoubleClick_1(object sender, EventArgs e)
        //{

        //}

        /// <summary>
        /// 复选框选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxOutTime_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (checkBoxOutTime.Checked)
                //{
                BindDataSource(Search());
                //}
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}
