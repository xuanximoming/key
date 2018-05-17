using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Wordbook;
using System.Data.SqlClient;
using System.Collections;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class UCRecord : DevExpress.XtraEditors.XtraUserControl
    {
        private IYidanEmrHost m_app;

        public UCRecord()
        {
            InitializeComponent();
        }

        public UCRecord(IYidanEmrHost app)
            : this()
        {
            m_app = app;
            BindLookUpEditorData();
            InitGridControl();
            InitDateEdit();
            //BindDataSource(Search());      edit by wangj 2013 1 30 加载时不查询
            //Search();
            //this.lookUpEditorApplyDept.Focus();
            //Application.AddMessageFilter(new MessageFilter());
        }
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

        private void InitGridControl()
        {
            try
            {
                gridViewList.OptionsSelection.EnableAppearanceFocusedRow = true;
                gridViewList.OptionsSelection.EnableAppearanceFocusedCell = false; ;
                gridViewList.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
                gridViewList.DoubleClick += new EventHandler(gridViewList_DoubleClick);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitDateEdit()
        {
            try
            {
                dateEditConsultDateBegin.EditValue = System.DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
                dateEditConsultDateEnd.EditValue = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable Search()
        {
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[]
                {
                    new SqlParameter(@"ApplyDeptid",this.lookUpEditorApplyDept.CodeValue),
                    new SqlParameter(@"EmployeeDeptid",this.lookUpEditorEmployeeDept.CodeValue),
                    new SqlParameter(@"DateTimeBegin",this.dateEditConsultDateBegin.DateTime.AddDays(-1).ToString("yyyy-MM-dd")),
                    new SqlParameter(@"DateTimeEnd",this.dateEditConsultDateEnd.DateTime.AddDays(1).ToString("yyyy-MM-dd"))
                };
                DataTable dt = m_app.SqlHelper.ExecuteDataTable("EMR_CONSULTATION.usp_GetAllConsultion", sqlparams, CommandType.StoredProcedure);
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
            catch (Exception)
            {
                throw;
            }
        }

        private void BindDataSource(DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0)// 添加 dt 的判空  tj 2013-1-14
                {
                    this.gridControlList.DataSource = null;
                    this.labPatCount.Text = "0";
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("没有符合条件的记录");
                    return;
                }
                if (this.checkBoxOutTime.Checked)
                {
                    dt = ToDataTable(dt.Select(string.Format("CONSULTSTATUS = '待会诊' and PLANCONSULTTIME<= '{0}' ", DateTime.Now.ToString("yyyy-MM-dd hh:mm")))); //, 
                    //dt = ToDataTable(dt.Select(string.Format("PLANCONSULTTIME<= '{0}' ", DateTime.Now.AddDays(-2))));
                }
                if (dt == null || dt.Rows.Count == 0) // 添加 dt 的判空  tj 2013-1-14
                {
                    this.gridControlList.DataSource = null;
                    this.labPatCount.Text = "0";
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("没有符合条件的记录");
                    return;
                }
                this.gridControlList.DataSource = dt;
                this.labPatCount.Text = dt.Rows.Count.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //private void refreshGridView()
        //{
        //    string filter = "CONSULTSTATUS = '待会诊' and PLANCONSULTTIME<= '{0}'  ";
        //    DataTable dt = gridControlList.DataSource as DataTable;
        //    if (dt != null)
        //    {
        //        dt.DefaultView.RowFilter = string.Format(filter, DateTime.Now.ToString("yyyy-MM-dd hh:mm"));
        //    }
        //}

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
            catch (Exception)
            {
                throw;
            }
        }

        private void BindLookUpEditorData()
        {
            try
            {
                YidanSoft.Common.Library.LookUpWindow luApplyDept = new YidanSoft.Common.Library.LookUpWindow();
                YidanSoft.Common.Library.LookUpWindow luEmplyeeDept = new YidanSoft.Common.Library.LookUpWindow();
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
            catch (Exception)
            {
                throw;
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

        void simpleButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dateEditConsultDateBegin.DateTime > this.dateEditConsultDateEnd.DateTime)
                {
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("起始时间不能大于结束时间");
                    return;
                }
                BindDataSource(Search());
                //if ((this.gridViewList.DataSource as DataTable) == null || (this.gridViewList.DataSource as DataTable).Rows.Count == 0)
                //if (((DataView)this.gridViewList.DataSource).ToTable() == null || ((DataView)this.gridViewList.DataSource).ToTable().Rows.Count == 0)
                //{
                //    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("没有符合条件的记录");
                //}
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击查看会诊信息 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridViewList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridViewList.FocusedRowHandle >= 0)
                {
                    DataRow dr = gridViewList.GetDataRow(gridViewList.FocusedRowHandle);
                    if (dr != null)
                    {
                        string noOfFirstPage = dr["NoOfInpat"].ToString();
                        string consultTypeID = dr["ConsultTypeID"].ToString();
                        string consultApplySn = dr["ConsultApplySn"].ToString();

                        //if (consultTypeID == Convert.ToString((int)ConsultType.One))
                        //{
                        //    FormRecordForOne formRecordForOne = new FormRecordForOne(noOfFirstPage, m_app, consultApplySn);
                        //    formRecordForOne.StartPosition = FormStartPosition.CenterParent;
                        //    formRecordForOne.ShowDialog();
                        //}
                        //else
                        //{
                        FormRecordForMultiply formRecrodForMultiply = new FormRecordForMultiply(noOfFirstPage, m_app, consultApplySn);
                        formRecrodForMultiply.StartPosition = FormStartPosition.CenterParent;
                        if (dr["APPLYUSER"].ToString() != m_app.User.Id)
                            formRecrodForMultiply.ReadOnlyControl();
                        formRecrodForMultiply.ShowDialog();
                        //}
                        //Search();
                    }
                }
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

        private void gridViewList_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                try
                {
                    YiDanCommon.YD_Common.AutoIndex(e);
                }
                catch (Exception ex)
                {
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
                }
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

        private void UCRecord_Load(object sender, EventArgs e)
        {
            try
            {
                this.lookUpEditorApplyDept.Focus();
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

        private void yiDanDevButtonPrint1_Click(object sender, EventArgs e)
        {
            m_app.CustomMessageBox.MessageShow("打印功能暂未上线！");
        }

        private void UCRecord_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.labelControlTotalPats.Location = new Point(25, this.Height - 30);
                this.labPatCount.Location = new Point(76, this.Height - 30);
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

        private void gridViewList_DoubleClick_1(object sender, EventArgs e)
        {
            try
            {
                if (this.gridViewList.FocusedRowHandle == -1)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }

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
                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }
    }
}
