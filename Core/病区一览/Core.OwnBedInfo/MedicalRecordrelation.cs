using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using MedicalRecordManage.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MedicalRecordManage.UI
{
    /// <summary>
    /// 借阅申请界面
    /// </summary>
    public partial class MedicalRecordrelation : DevBaseForm//, IEMREditor
    {
        IEmrHost m_app;

        CGridCheckMarksSelection selection;
        internal CGridCheckMarksSelection Selection
        {
            get
            {
                return selection;
            }
        }
        //最大数量
        public int m_readMax = 0;
        //最大时间
        public int m_readMaxTime = 0;
        public int m_iCommandFlag = 0;
        /// <summary>
        /// add by jy.zhu 保存当前操作类型，1提交，0草稿
        /// </summary>
        public int m_iTabIndex = 0;
        public string m_sUser = "";
        //已勾选和未勾选的数据集
        private List<DataRow> checkedList = new List<DataRow>();

        private List<DataRow> notCheckedList = new List<DataRow>();


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
        private void InitializeParameters()
        {
            try
            {

                //初始化下拉框科室信息                
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 校验申请时间方法
        /// Modify by xlb 2013-03-28
        /// </summary>
        /// <returns></returns>

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Commit(1, "申请提交");

        }
        //
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckDate())
                {
                    DevExpress.Utils.WaitDialogForm m_WaitDialog = new DevExpress.Utils.WaitDialogForm("正在查询数据...", "请稍等");
                    LoadData();
                    m_WaitDialog.Hide();
                }
                //绑定控件
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// Modify by xlb 2013-05-29
        /// 解决同一天数据检索不出问题
        /// <auth> Modify by xlb</auth>
        /// <date>2013-05-29</date>
        /// </summary>
        private void LoadData()
        {
            try
            {


                string sql = "select * from inpatient where 1=1";



                List<DbParameter> sqlParams = new List<DbParameter>();
                if (this.txtName.Text.Trim() != null && this.txtName.Text.Trim() != "")
                {
                    sql = sql + " and i.name like @name ";
                    SqlParameter param1 = new SqlParameter("@name", SqlDbType.VarChar);
                    param1.Value = "%" + this.txtName.Text.Trim() + "%";
                    sqlParams.Add(param1);
                }
                if (this.txtNumber.Text.Trim() != null && this.txtNumber.Text.Trim() != "")
                {
                    sql = sql + " and i.patid like '%'||@patId||'%' ";
                    SqlParameter param2 = new SqlParameter("@patId", SqlDbType.VarChar);
                    param2.Value = this.txtNumber.Text.Trim();
                    sqlParams.Add(param2);
                }
                //if (this.lookUpEditorDiagnosis.Text.Trim() != null && this.lookUpEditorDiagnosis.Text.Trim() != "")
                //{
                //    sql = sql + " and i.cyzd like @cyzd ";
                //    SqlParameter param3 = new SqlParameter("@cyzd", SqlDbType.VarChar);
                //    param3.Value = "%" + this.lookUpEditorDiagnosis.Text.Trim() + "%";
                //    sqlParams.Add(param3);
                //}


                DataTable dataTable = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(sql, sqlParams, CommandType.Text);
                //压缩数据源生成新的数据源
                //压缩DateSet中的记录，将住院诊断信息合并

                string wheresql = "1=1 ";



                DataRow[] dr = dataTable.Select(wheresql);



                this.dbGrid.DataSource = ToDataTable(dr);

            }
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 校验时间方法
        /// </summary>
        /// <returns></returns>
        private bool CheckDate()
        {
            try
            {

                if (this.dateLeaveStart.DateTime.ToString().Trim() != "" && this.dateLeaveEnd.DateTime.ToString().Trim() != "")
                {
                    if (this.dateLeaveStart.DateTime > this.dateLeaveEnd.DateTime)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("入院起始日期不能大于结束日期", "信息提示");
                        this.dateLeaveStart.Focus();
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

        /// <summary>
        /// 重置方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {

                this.txtName.Text = "";
                this.txtNumber.Text = "";

                InitializeParameters();


                this.dbGrid.DataSource = null;
                this.txtName.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 关闭窗体事件
        /// Modify by xlb 2013-03-28
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MyMessageBox.Show("确定退出吗？", "提示",
                MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存草稿事件
        /// Modify by xlb 2013-03-28
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Commit(0, "申请保存为草稿");

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void Commit(int type, string info)
        {
            try
            {
                if (type == 1)
                {
                    this.m_iTabIndex = 2;
                }

                // this.m_iTabIndex = type;
                if (selection.SelectedCount > 0 && selection.SelectedCount <= this.m_readMax)
                {
                    string message = "";

                    List<string> nolist = new List<string>();
                    List<string> saveNolist = new List<string>();
                    BatchSave(type, ref saveNolist);
                    string msgmulti = "";
                    string msgsave = "";
                    /*
                    if (nolist.Count > 0)
                    {                           
                    for (int i = 0; i < nolist.Count; i++)
                    {
                        msgmulti = nolist[i].ToString() + " " + msgmulti;
                    }
                    msgmulti = msgmulti + "的病历已被申请, 不能重复申请!";
                    }
                    */

                    if (saveNolist.Count > 0)
                    {
                        for (int i = 0; i < saveNolist.Count; i++)
                        {
                            msgsave = saveNolist[i].ToString() + msgsave;
                        }
                        msgsave = msgsave + info + "操作成功";
                        //放到此处，如果没有选择任何选项，就没必要提示 edit by ywk 2013年3月26日10:33:39 
                        MessageBox.Show(msgmulti + msgsave, "信息提示");
                    }
                    else
                    {

                        MessageBox.Show("请选择要借阅的病历!");
                    }
                    m_iCommandFlag = 1;

                    selection.ClearSelection();
                    LoadData();

                    //else
                    //{
                    //    MessageBox.Show("请输入不超过" + this.m_readMaxTime + "天数", "信息提示");
                    //    this.txtTimes.Focus();
                    //}
                }
                else
                {
                    if (selection.SelectedCount == 0)
                    {
                        MessageBox.Show("请选择要借阅的病历!");
                    }
                    else
                    {
                        //Common.Ctrs.DLG.MessageBox.Show("请选择不超过" + m_readMax + "个病历申请借阅", "信息提示");
                        MessageBox.Show("可选择不超过" + m_readMax + "个病历申请借阅", "信息提示");
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }



        /// <summary>
        /// 存储函数
        /// Modify by xlb 2013-03-28
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="saveNolist">保存集合</param>
        private void BatchSave(int status/*状态*/, ref List<string> saveNolist)
        {
            try
            {

            }
            catch (Exception)
            {
                throw;
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

        /// <summary>
        /// 借阅申请界面加载事件
        /// Modify by xlb 2013-03-28
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MedicalRecordApply_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeImageList();
                selection = new CGridCheckMarksSelection(this.dbGridView);//把多选框绑定到你指定的grid
                selection.CheckMarkColumn.VisibleIndex = 0;//使多选框排第一列
                dbGridView.Columns[0].Width = 50;
                dbGridView.Columns[1].Width = 50;
                dbGridView.Columns[2].Width = 40;
                dbGridView.Columns[3].Width = 40;
                dbGridView.Columns[4].Width = 150;
                dbGridView.Columns[5].Width = 260;
                dbGridView.Columns[6].Width = 150;
                DrectSoft.Common.DS_Common.CancelMenu(this.panelHead, contextMenuStrip1);
                DrectSoft.Common.DS_Common.CancelMenu(this.panelSubBottom, contextMenuStrip1);
                //Add by xlb 2013-03-28

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void InitializeImageList()
        {
            try
            {
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MedicalRecordApply_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, false);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 控件键盘事件
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

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
                MyMessageBox.Show(1, ex);
            }
        }

        private void txtDiagnosis_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtTimes_KeyPress(object sender, KeyPressEventArgs e)
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

        private void lookUpEditorPurpose_KeyPress(object sender, KeyPressEventArgs e)
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

        #region 注销 by xlb 2013-03-28

        //private void lookUpEditorPurpose_EditValueChanged(object sender, EventArgs e)
        //{
        //    this.memoReason.Text = lookUpEditorPurpose.Text.Trim();
        //}
        #endregion

        private void dbGridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            /*
            try
            {
                DataRowView drv = dbGridView.GetRow(e.RowHandle) as DataRowView;
                if (e.Column.Caption == "选择")
                {
                    e.Column.AppearanceCell.BackColor = Color.Red;//.Image = "";
                }
            }
            catch (Exception ex)
            {

                Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
            */
        }

        /// <summary>
        /// checkEdit切换事件
        /// Add by xlb 2013-03-28
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdgPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //复选框选择借阅目的其他时开放借阅目的编辑框

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void dbGrid_DoubleClick(object sender, EventArgs e)
        {
            //int[] list = dbGridView.GetSelectedRows();
            //if (list.Length > 0)
            //{
            //    string status = dbGridView.GetRowCellValue(list[0], "CheckEdit").ToString();

            //}
        }

        private void txtTimes_KeyDown(object sender, KeyEventArgs e)
        {

        }

        //private void txtNumber_KeyPress_1(object sender, KeyPressEventArgs e)
        //{
        //    try
        //    {
        //        if ((int)e.KeyChar == 13)
        //        {
        //            SendKeys.Send("{Tab}");
        //            SendKeys.Flush();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyMessageBox.Show(1, ex);
        //    }
        //}
    }
}