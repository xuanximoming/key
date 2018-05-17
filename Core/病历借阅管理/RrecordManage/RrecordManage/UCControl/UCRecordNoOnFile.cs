using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common;
using DrectSoft.Wordbook;
using DrectSoft.Core.RecordManage.PublicSet;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.Service;

namespace DrectSoft.Core.RecordManage.UCControl
{
    public partial class UCRecordNoOnFile : DevExpress.XtraEditors.XtraUserControl
    {
        public UCRecordNoOnFile()
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

                Dept.Columns["ID"].Caption = "科室代码";
                Dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols, "ID//NAME//PY//WB");
                lookUpEditorDepartment.SqlWordbook = deptWordBook;
                lookUpEditorDepartment.CodeValue = DS_Common.currentUser.CurrentDeptId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region edit by cyq 2012-12-04 已弃用
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

        /// <summary>
        /// 初始化住院号
        /// </summary>
        //private void IntiRecordID()
        //{
        //    try
        //    {
        //        this.lookUpWindowRecordID.SqlHelper = SqlUtil.App.SqlHelper;

        //        DataTable RecordID = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetMedicalRrecordViewFrm",
        //             new SqlParameter[] { new SqlParameter("@GetType", "5") }, CommandType.StoredProcedure);

        //        RecordID.Columns["ID"].Caption = "住院号";

        //        Dictionary<string, int> cols = new Dictionary<string, int>();

        //        cols.Add("ID", 120);

        //        SqlWordbook recordIDWordBook = new SqlWordbook("queryrecordid", RecordID, "ID", "ID", cols, "ID");
        //        this.txt_recordID.SqlWordbook = recordIDWordBook;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        #endregion

        /// <summary>
        /// 初始化诊断
        /// </summary>
        private void InitDiag()
        {
            try
            {
                DataTable disease = new DataTable();
                disease.Columns.Add("ICD");
                disease.Columns.Add("NAME");
                disease.Columns.Add("PY");
                disease.Columns.Add("WB");
                DataTable diagnosis = SqlUtil.App.SqlHelper.ExecuteDataTable("select py, wb, name, icd from diagnosis  where valid='1' union select py, wb, name, icdid from diagnosisothername where valid='1'");
                foreach (DataRow row in diagnosis.Rows)
                {
                    DataRow displayRow = disease.NewRow();
                    displayRow["ICD"] = row["ICD"];
                    displayRow["NAME"] = row["NAME"];
                    displayRow["PY"] = row["PY"];
                    displayRow["WB"] = row["WB"];
                    disease.Rows.Add(displayRow);
                }

                this.lookUpWindowInDiag.SqlHelper = SqlUtil.App.SqlHelper;
                this.lookUpWindowOutDiag.SqlHelper = SqlUtil.App.SqlHelper;
                disease.Columns["ICD"].Caption = "诊断编码";
                disease.Columns["NAME"].Caption = "诊断名称";
                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ICD", 60);
                cols.Add("NAME", 120);

                SqlWordbook diagWordBook = new SqlWordbook("queryDiag", disease, "ICD", "NAME", cols, "ICD//NAME//PY//WB");
                this.lookUpEditorInDiag.SqlWordbook = diagWordBook;
                this.lookUpEditorOutDiag.SqlWordbook = diagWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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

                cols.Add("ID", 60);
                cols.Add("NAME", 60);

                SqlWordbook nameWordBook = new SqlWordbook("queryname", Name, "ID", "NAME", cols, "ID//NAME//PY//WB");
                this.lookUpEditorPhysician.SqlWordbook = nameWordBook;
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
        private void UCRecordNoOnFile_Load(object sender, EventArgs e)
        {
            try
            {
                //edit by cyq 2012-12-04
                //IntiRecordID();
                //InitName();
                InitDepartment();
                InitDiag();
                InitPhysician();
                //获取病历状态
                SqlUtil.GetDictionarydetail(lookUpRecordStatus, "1", "47", "usp_GetRecordManageFrm");

                //add by cyq 2012-12-23 病案室人员显示归档按钮
                this.btn_reback.Visible = DS_BaseService.CheckIfQuatityControlPerson(DS_Common.currentUser.Id);

                Reset();
                this.ActiveControl = txt_recordID;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        public void InitializeSqlUtil(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost app)
        {
            if (SqlUtil.App == null)
            {
                SqlUtil.App = app;
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

                DataTable table = SqlUtil.App.SqlHelper.ExecuteDataTable("usp_GetRecordNoOnFileWyt"
                    , new SqlParameter[] 
                { 
                    new SqlParameter("@DateOutHosBegin", dateEditBegin.DateTime.Date.ToString("yyyy-MM-dd")), 
                    new SqlParameter("@DateOutHosEnd", string.IsNullOrEmpty(this.dateEditEnd.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : this.dateEditEnd.DateTime.ToString("yyyy-MM-dd")),
                    new SqlParameter("@DateInHosBegin", "1900-01-01"), //dateEditInBegin.DateTime.Date.ToString("yyyy-MM-dd")
                    new SqlParameter("@DateInHosEnd",DateTime.Now.ToString("yyyy-MM-dd")),//string.IsNullOrEmpty(this.dateEditInEnd.Text) ? DateTime.Now.ToString("yyyy-MM-dd") : this.dateEditInEnd.DateTime.ToString("yyyy-MM-dd")
                    new SqlParameter("@DeptID", lookUpEditorDepartment.CodeValue.ToString()),
                    new SqlParameter("@InDiag", lookUpEditorInDiag.CodeValue.ToString()),
                    new SqlParameter("@OutDiag", lookUpEditorOutDiag.CodeValue.ToString()),
                    new SqlParameter("@Status", lookUpRecordStatus.EditValue.ToString()) ,
                    new SqlParameter("@Patientname", this.txt_patiName.Text.Trim()),
                    new SqlParameter("@Recordid", this.txt_recordID.Text.Trim()),
                    new SqlParameter("@Physician", this.lookUpEditorPhysician.CodeValue.ToString()) 
                }
                    , CommandType.StoredProcedure);
                

                gridviewRecordNoOnFile.SelectAll();
                gridviewRecordNoOnFile.DeleteSelectedRows();
                gridControlRecordNoOnFile.DataSource = table;

                lblTip.Text = "共" + table.Rows.Count.ToString() + "份";

                if (table.Rows.Count <= 0)
                {
                    SqlUtil.App.CustomMessageBox.MessageShow("没有满足条件的记录");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 加载文书录入页面
        /// edit by Yanqiao.Cai 2012-11-23
        /// </summary>
        /// <param name="bolMessage"></param>
        private void BrowserMedicalRecord(bool bolMessage)
        {
            try
            {
                int fouceRowIndex = gridviewRecordNoOnFile.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    if (bolMessage) 
                    {
                        gridControlRecordNoOnFile.Focus();
                        SqlUtil.App.CustomMessageBox.MessageShow("请选择一条病人记录"); 
                    }
                    return;
                }
                DataRow foucesRow = gridviewRecordNoOnFile.GetDataRow(fouceRowIndex);

                //edit by wyt 2012-11-09 新建病历显示窗口
                string noOfFirstPage = foucesRow["NOOFINPAT"].ToString();
                //设置文书录入左侧菜单图标状态
                FloderState floaderState = FloderState.None;
                if (DS_BaseService.CheckIfQuatityControlPerson(DS_Common.currentUser.Id))
                {
                    floaderState = FloderState.FirstPage;
                }
                EmrBrowser frm = new EmrBrowser(noOfFirstPage, SqlUtil.App, floaderState);
                frm.StartPosition = FormStartPosition.CenterParent;
                //添加窗体关闭事件 add by cyq 2012-12-06
                frm.FormClosed += new FormClosedEventHandler(EmrBrowser_FormClosed);
                frm.ShowDialog();
                //移除窗体关闭事件 add by cyq 2012-12-06
                frm.FormClosed -= new FormClosedEventHandler(EmrBrowser_FormClosed);

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
        /// 弹出窗体关闭事件
        /// 1、若当前所编辑病历记录已归档，则移除该记录
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmrBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (gridviewRecordNoOnFile.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow foucesRow = gridviewRecordNoOnFile.GetDataRow(gridviewRecordNoOnFile.FocusedRowHandle);
                if (null == foucesRow || null == foucesRow["NOOFINPAT"])
                {
                    return;
                }
                if (!DS_BaseService.CheckRecordRebacked(foucesRow["NOOFINPAT"].ToString()))
                {//若当前所编辑病历记录已归档，则移除该记录
                    gridviewRecordNoOnFile.DeleteRow(gridviewRecordNoOnFile.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击病人事件
        /// edit by Yanqiao.Cai 2012-11-12
        /// 1、add try ... catch
        /// 2、双击小标题应无操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridviewRecordNoOnFile_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridviewRecordNoOnFile.CalcHitInfo(gridControlRecordNoOnFile.PointToClient(Cursor.Position));
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
        /// 画面检查
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-26</date>
        /// </summary>
        /// <returns></returns>
        private string CheckItem()
        {
            try
            {
                //if (dateEditInBegin.DateTime > dateEditInEnd.DateTime)
                //{
                //    dateEditInBegin.Focus();
                //    return "入院开始日期不能大于入院结束日期";
                //}
                //else 
                if (lookUpRecordStatus.EditValue == null)
                {
                    lookUpRecordStatus.Focus();
                    return "请选择病历状态";
                }
                else if (string.IsNullOrEmpty(lookUpEditorDepartment.CodeValue))
                {
                    lookUpEditorDepartment.Focus();
                    return "请选择出院科室";
                }
                else if (dateEditBegin.DateTime > dateEditEnd.DateTime)
                {
                    dateEditBegin.Focus();
                    return "出院开始日期不能大于出院结束日期";
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
        private void gridviewRecordNoOnFile_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
                dateEditBegin.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                dateEditEnd.Text = DateTime.Now.ToShortDateString();
                //dateEditInBegin.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                //dateEditInEnd.Text = DateTime.Now.ToShortDateString();

                this.txt_patiName.Text = string.Empty;
                this.txt_recordID.Text = string.Empty;
                //edit by cyq 2012-12-04
                //this.lookUpEditorDepartment.CodeValue = "0000";
                this.lookUpEditorDepartment.CodeValue = DS_Common.currentUser.CurrentDeptId;
                this.lookUpRecordStatus.EditValue = "4700";
                this.lookUpEditorPhysician.CodeValue = string.Empty;
                this.lookUpEditorInDiag.CodeValue = string.Empty;
                this.lookUpEditorOutDiag.CodeValue = string.Empty;
                this.txt_recordID.Focus();
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
                txt_patiName.Text = string.Empty;
                txt_recordID.Text = string.Empty;
                //dateEditInBegin.Text = string.Empty;
                //dateEditInEnd.Text = string.Empty;
                lookUpEditorInDiag.Text = string.Empty;
                lookUpEditorPhysician.Text = string.Empty;
                lookUpRecordStatus.SelectedText = "未归档";
                lookUpEditorDepartment.CodeValue = "0000";
                dateEditBegin.Text = string.Empty;
                dateEditEnd.Text = string.Empty;
                panelControl2.Text = string.Empty;

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 病历归档事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-05</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reback_Click(object sender, EventArgs e)
        {
            try
            {
                int fouceRowIndex = gridviewRecordNoOnFile.FocusedRowHandle;
                if (fouceRowIndex < 0)
                {
                    gridControlRecordNoOnFile.Focus();
                    SqlUtil.App.CustomMessageBox.MessageShow("请选择一条病人记录");
                    return;
                }
                DataRow foucesRow = gridviewRecordNoOnFile.GetDataRow(fouceRowIndex);
                if (null == foucesRow || null == foucesRow["NOOFINPAT"])
                {
                    return;
                }
                int noofinpat = int.Parse(foucesRow["NOOFINPAT"].ToString().Trim());

                DataTable dt = DS_SqlService.GetRecordsByNoofinpat(noofinpat);
                if (null == dt || dt.Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(foucesRow["NAME"] + " 没有病历，无法归档。");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要归档 " + foucesRow["NAME"] + " 的病历吗？", "归档病历", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                if (DS_BaseService.CheckRecordRebacked(noofinpat.ToString()))
                {
                    int num = DS_SqlService.SetRecordsRebacked(noofinpat.ToString());
                    if (num > 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("归档成功");
                        gridviewRecordNoOnFile.DeleteRow(gridviewRecordNoOnFile.FocusedRowHandle);
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
    }
}
