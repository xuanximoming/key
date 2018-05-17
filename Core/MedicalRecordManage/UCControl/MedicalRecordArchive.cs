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
using DrectSoft.Common;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.FrameWork;

using MedicalRecordManage.Object;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Service;

namespace MedicalRecordManage.UCControl
{
    /// <summary>
    /// 已归档病历查询界面
    /// </summary>
    public partial class MedicalRecordArchive : DevExpress.XtraEditors.XtraUserControl
    {
        public MedicalRecordArchive()
        {
            InitializeComponent();
        }

        #region 初始化
        private void InitForm()
        {
            try
            {
                //初始化科室
                InitDepartment();

                //初始化诊断名称
                InitDiag();

                //初始化手术名称
                InitSurgery();

                //初始化住院医师
                InitPhysician();

                Reset();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 初始化科室
        /// <summary>
        /// 初始化科室
        /// </summary>
        private void InitDepartment()
        {
            try
            {
                ComponentCommand.InitializeDepartment(ref this.lookUpEditorDepartment, ref this.lookUpWindowDepartment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 初始诊断名称
        private void InitDiag()
        {
            try
            {
                ComponentCommand.InitializeDiagnosis(ref this.lookUpEditorInDiag, ref this.lookUpWindowInDiag);
                ComponentCommand.InitializeDiagnosis(ref this.lookUpEditorOutDiag, ref this.lookUpWindowOutDiag);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 初始化住院医师
        /// </summary>
        private void InitPhysician()
        {
            try
            {
                this.lookUpWindowPhysician.SqlHelper = SqlUtil.App.SqlHelper;
                string sql = "select ID,NAME,PY,WB from users";
                DataTable Name = SqlUtil.App.SqlHelper.ExecuteDataTable(sql);
                Name.Columns["ID"].Caption = "医师工号";
                Name.Columns["NAME"].Caption = "医师姓名";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 80);
                cols.Add("NAME", 100);

                SqlWordbook nameWordBook = new SqlWordbook("queryname", Name, "ID", "NAME", cols, "ID//NAME//PY//WB");
                this.lookUpEditorPhysician.SqlWordbook = nameWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        #region 初始手术名称
        private void InitSurgery()
        {
            try
            {
                lookUpWindowSurgery.SqlHelper = SqlUtil.App.SqlHelper;

                DataTable Dept = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetRecordManageFrm",
                     new SqlParameter[] { new SqlParameter("@FrmType", "2") }, CommandType.StoredProcedure);

                Dept.Columns["ID"].Caption = "手术代码";
                Dept.Columns["NAME"].Caption = "手术名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 120);

                SqlWordbook operWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorSurgery.SqlWordbook = operWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-05
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCRecordOnFile_Load(object sender, EventArgs e)
        {
            try
            {
                InitForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 双击病人事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、双击小标题应无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewRecordOnFile_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewRecordOnFile.CalcHitInfo(gridControlRecordOnFile.PointToClient(Cursor.Position));
                if (hitInfo.RowHandle < 0)
                {
                    return;
                }
                BrowserMedicalRecord(false);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
                DevExpress.Utils.WaitDialogForm m_WaitDialog = new DevExpress.Utils.WaitDialogForm("正在查询数据...", "请稍等");                
                string errorStr = CheckItem();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    m_WaitDialog.Hide();
                    MessageBox.Show(errorStr);
                    return;
                }

                SqlParameter[] sqlParam = new SqlParameter[] 
                { new SqlParameter("@DateBegin",SqlDbType.VarChar ),
                  new SqlParameter("@DateEnd",SqlDbType.VarChar ),
                  new SqlParameter("@PatID",SqlDbType.VarChar ),
                  new SqlParameter("@Name", SqlDbType.VarChar ),
                  new SqlParameter("@SexID", SqlDbType.VarChar ),
                  new SqlParameter("@AgeBegin",SqlDbType.VarChar ),
                  new SqlParameter("@AgeEnd", SqlDbType.VarChar ),
                  new SqlParameter("@OutHosDept",SqlDbType.VarChar ),
                  new SqlParameter("@InDiag", SqlDbType.VarChar ),
                  new SqlParameter("@OutDiag", SqlDbType.VarChar ),
                  new SqlParameter("@SurgeryID", SqlDbType.VarChar ),
                  new SqlParameter("@Physician", SqlDbType.VarChar )
                };

                sqlParam[0].Value = dateEditBegin.DateTime.Date.ToString("yyyy-MM-dd");
                sqlParam[1].Value = string.IsNullOrEmpty(this.dateEditEnd.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : this.dateEditEnd.DateTime.ToString("yyyy-MM-dd");
                sqlParam[2].Value = txtPatID.Text.Trim();
                sqlParam[3].Value = txtName.Text.Trim();
                sqlParam[4].Value = radioSex.SelectedIndex == 0 ? "" : radioSex.SelectedIndex.ToString();
                sqlParam[5].Value = txtAgeBegin.Text.Trim();
                sqlParam[6].Value = txtAgeEnd.Text.Trim();
                sqlParam[7].Value = lookUpEditorDepartment.CodeValue;
                sqlParam[8].Value = lookUpEditorInDiag.CodeValue;
                sqlParam[9].Value = lookUpEditorOutDiag.CodeValue;
                sqlParam[10].Value = lookUpEditorSurgery.CodeValue;
                sqlParam[11].Value = this.lookUpEditorPhysician.CodeValue.Trim();

                DataTable table = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetRecordOnFile", sqlParam, CommandType.StoredProcedure);
                //加载性别图片
                DS_Common.InitializeImage_XB(repositoryItemImageXB, imageListXB);

                gridViewRecordOnFile.SelectAll();
                gridViewRecordOnFile.DeleteSelectedRows();
                gridControlRecordOnFile.DataSource = table;

                m_WaitDialog.Hide();
                lblTip.Text = "共" + table.Rows.Count.ToString() + "条记录";

                if (table.Rows.Count <= 0)
                {
                    SqlUtil.App.CustomMessageBox.MessageShow("没有满足条件的记录");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BrowserMedicalRecord(bool bolMessage)
        {
            try
            {

                int fouceRowIndex = gridViewRecordOnFile.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    if (bolMessage) SqlUtil.App.CustomMessageBox.MessageShow("请选择一条病人记录");
                    return;
                }
                DataRow foucesRow = gridViewRecordOnFile.GetDataRow(fouceRowIndex);
                

                //edit by wyt 2012-11-09 新建病历显示窗口
                string noOfFirstPage = foucesRow["NOOFINPAT"].ToString();
                MedicalRecordManage.UI.EmrBrowerDlg frm = new MedicalRecordManage.UI.EmrBrowerDlg(noOfFirstPage, SqlUtil.App, FloderState.None);
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog();
                #region 取消加载插件方式显示病历 edit by wyt 2012-11-09
                //SqlUtil.App.ChoosePatient(Convert.ToDecimal(noOfInpat));
                //SqlUtil.App.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", "DrectSoft.Core.MainEmrPad.MainForm");
                #endregion

                //this.LoadEmrContent(noOfFirstPage);
                //if (m_UCEmrInput != null)
                //{
                //    EmrBrowser browser = new EmrBrowser();
                //    browser.Controls.Add(m_UCEmrInput);
                //    m_UCEmrInput.Dock = DockStyle.Fill;
                //    browser.ShowDialog();
                //}

                //HistoryRecordBrowser frmHistoryRecordBrowser = new HistoryRecordBrowser(PatID);
                //frmHistoryRecordBrowser.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        public bool IsNumeric(string value)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
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
                if (dateEditBegin.DateTime.Date > dateEditEnd.DateTime.Date)
                {
                    dateEditBegin.Focus();
                    return "出院开始日期不能大于出院结束日期";
                }
                else if (string.IsNullOrEmpty(lookUpEditorDepartment.CodeValue))
                {
                    lookUpEditorDepartment.Focus();
                    return "请选择出院科室";
                }
                else if (!IsNumeric(this.txtAgeBegin.Text.Trim()))
                {
                    this.txtAgeBegin.Focus();
                    this.txtAgeBegin.SelectAll();
                    return "年龄只能是数字";
                }
                else if (!IsNumeric(this.txtAgeEnd.Text.Trim()))
                {
                    this.txtAgeEnd.Focus();
                    this.txtAgeEnd.SelectAll();
                    return "年龄只能是数字";
                }
                else if (!string.IsNullOrEmpty(this.txtAgeBegin.Text.Trim()) && !string.IsNullOrEmpty(this.txtAgeEnd.Text.Trim()) && double.Parse(this.txtAgeBegin.Text.Trim()) > double.Parse(this.txtAgeEnd.Text.Trim()))
                {
                    this.txtAgeBegin.Focus();
                    this.txtAgeBegin.SelectAll();
                    return "开始年龄不能大于结束年龄";
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
        private void gridViewRecordOnFile_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
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
                dateEditBegin.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                dateEditEnd.Text = DateTime.Now.ToShortDateString();

                this.lookUpEditorDepartment.CodeValue = "0000";
                this.lookUpEditorInDiag.CodeValue = string.Empty;
                this.lookUpEditorOutDiag.CodeValue = string.Empty;
                this.lookUpEditorPhysician.CodeValue = string.Empty;
                this.lookUpEditorSurgery.CodeValue = string.Empty;
                this.radioSex.SelectedIndex = 0;
                this.txtName.Text = string.Empty;
                this.txtAgeBegin.Text = string.Empty;
                this.txtAgeEnd.Text = string.Empty;
                this.txtPatID.Text = string.Empty;
                this.txtName.Focus();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 清空和重置重复 XLB 2013-05-29
 
        //private void DevButtonClear1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        dateEditBegin.Text = string.Empty;
        //        dateEditEnd.Text = string.Empty;
        //        txtName.Text = string.Empty;
        //        txtAgeBegin.Text = string.Empty;
        //        txtAgeEnd.Text = string.Empty;
        //        radioSex.SelectedIndex = 0;
        //        lookUpEditorInDiag.CodeValue = string.Empty;
        //        txtPatID.Text = string.Empty;
        //        lookUpEditorPhysician.CodeValue = string.Empty;
        //        lookUpEditorOutDiag.CodeValue = string.Empty;
        //        lookUpEditorDepartment.CodeValue = "0000";
        //        lookUpEditorSurgery.CodeValue = string.Empty;
        //    }
        //    catch(Exception ex)
        //    {
        //        Common.Ctrs.DLG.MessageBox.Show(ex.Message);
        //    }

        //}

        #endregion

        private void gridControlRecordOnFile_DoubleClick(object sender, EventArgs e)
        {
            //
        }
    }
}
