using DevExpress.XtraEditors;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Emr.QCTimeLimit.QCEntity;
using DrectSoft.DSSqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DrectSoft.Emr.QCTimeLimit.QCControlUse
{
    /// <summary>
    /// 监控代码维护界面
    /// 项令波 2013-01-08
    /// </summary>
    public partial class QCEmrItem : DevBaseForm
    {
        string formName;//窗体名称
        EmrQcItem emrQcItem;

        /// <summary>
        /// 构造方法
        /// </summary>
        public QCEmrItem()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QCEmrItem(string formCaption)
        {
            try
            {
                InitializeComponent();
                formName = formCaption.Substring(0, formCaption.IndexOf("("));
                InitDataEMrQcItem();
                SetEditState(false, true);
                DS_SqlHelper.CreateSqlHelper();

                #region 事件
                btnSearch.Click += new EventHandler(btnSearch_Click);
                btnReset.Click += new EventHandler(btnReset_Click);
                btnAddEmrQcItem.Click += new EventHandler(btnAddEmrQcItem_Click);
                btnEdit.Click += new EventHandler(btnEdit_Click);
                btnDeleteEmrQcItem.Click += new EventHandler(btnDeleteEmrQcItem_Click);
                btnSave.Click += new EventHandler(btnSave_Click);
                btnCancel.Click += new EventHandler(btnCancel_Click);
                btnClose.Click += new EventHandler(btnClose_Click);
                gridViewEmrQcItem.DoubleClick += new EventHandler(gridViewEmrQcItem_DoubleClick);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 事件 xlb 2013-01-08

        /// <summary>
        /// 查询事件
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(Object sender, EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击事件
        /// 应对FocusedRowChanged事件没触发
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewEmrQcItem_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gridViewEmrQcItem.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择数据");
                    return;
                }
                emrQcItem = gridViewEmrQcItem.GetRow(gridViewEmrQcItem.FocusedRowHandle) as EmrQcItem;
                SetEditState(false, true);
                ShowEmrQcItem(emrQcItem);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ClearControl(panelControlTop);
                InitDataEMrQcItem();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增事件
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddEmrQcItem_Click(object sender, EventArgs e)
        {

            try
            {
                SetEditState(true, false);
                ClearControl(groupControlNotton);
                textEditCode.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑事件
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewEmrQcItem.FocusedRowHandle < 0 || emrQcItem.I_Code == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选中数据");
                    return;
                }
                SetEditState(true, false);
                textEditCode.Focus();
                textEditICode.Enabled = false;
                int rowHandel = gridViewEmrQcItem.FocusedRowHandle;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteEmrQcItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewEmrQcItem.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有数据");
                    return;
                }
                //获得需删除行的行号
                int rowHandel = gridViewEmrQcItem.FocusedRowHandle;
                int maxRow = (gridViewEmrQcItem.DataSource as List<EmrQcItem>).Count;
                emrQcItem = gridViewEmrQcItem.GetRow(rowHandel) as EmrQcItem;
                int count = QCRule.QcRuleByQcCode(emrQcItem.I_Code);
                if (count > 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该监控代码被使用，无法删除");
                    return;
                }
                DialogResult dialogDelete = DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定删除此条数据吗？", "提示", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel);
                if (dialogDelete == DialogResult.Cancel)
                {
                    return;
                }
                EmrQcItem.DeleteEmrQcItem(emrQcItem);
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");
                InitDataEMrQcItem();
                ClearControl(groupControlNotton);
                SetEditState(false, true);
                if (rowHandel >= 0 && rowHandel < maxRow - 1)
                {
                    //焦点定位到删除行的下行 如果是最后一行
                    gridViewEmrQcItem.MoveBy(rowHandel);
                }
                else if (rowHandel == maxRow - 1)
                {
                    //如果删除最后一行则焦点定位到上一条数据
                    gridViewEmrQcItem.MoveBy(rowHandel - 1);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveEmrQcItem();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetEditState(false, true);
                ClearControl(groupControlNotton);
                emrQcItem = new EmrQcItem();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 选中行变化时触发的事件
        /// 项令波 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewEmrQcItem_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle < 0)
                {
                    return;
                }
                emrQcItem = gridViewEmrQcItem.GetRow(e.FocusedRowHandle) as EmrQcItem;
                SetEditState(false, true);
                ShowEmrQcItem(emrQcItem);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭事件
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 序号列
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewEmrQcItem_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 窗体加载事件
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QCTimeRecord_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = formName;
                textEditCodeDesc.Focus();
                gridControlEmrQcItem.TabStop = false;//屏蔽掉tab切换焦点定位列表上
                //禁掉右键菜单
                DS_Common.CancelMenu(panelControlTop, contextMenuStrip1);
                DS_Common.CancelMenu(groupControlNotton, contextMenuStrip1);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 方法 xlb 2013-01-08

        /// <summary>
        /// 初始化病历质量控制数据
        /// xlb 2013-01-08
        /// </summary>
        private void InitDataEMrQcItem()
        {
            try
            {
                emrQcItem = new EmrQcItem();
                List<EmrQcItem> emrQcItemList = EmrQcItem.GetEmrQCItem(emrQcItem);
                gridControlEmrQcItem.DataSource = emrQcItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询方法
        /// xlb 2013-01-08
        /// </summary>
        private void Search()
        {
            try
            {
                emrQcItem = new EmrQcItem();
                emrQcItem.I_Name = textEditCodeDesc.Text.Trim();
                emrQcItem.I_Code = txtCode.Text.Trim();
                List<EmrQcItem> emrQcItemList = EmrQcItem.GetEmrQCItem(emrQcItem);
                gridControlEmrQcItem.DataSource = emrQcItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 显示选中行信息
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="emrQcItem"></param>
        private void ShowEmrQcItem(EmrQcItem emrQcItem)
        {
            try
            {
                textEditCodeName.Text = emrQcItem.Name;
                textEditCode.Text = emrQcItem.Code;
                textEditICode.Text = emrQcItem.I_Code;
                textEditICodeName.Text = emrQcItem.I_Name;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置文本编辑区域可编辑性和按钮的可用性
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="isEdit"></param>
        /// <param name="isCan"></param>
        private void SetEditState(bool isEdit, bool isCan)
        {
            try
            {
                textEditCodeName.Enabled = isEdit;
                textEditCode.Enabled = isEdit;
                textEditICode.Enabled = isEdit;
                textEditICodeName.Enabled = isEdit;

                btnAddEmrQcItem.Enabled = isCan;
                btnEdit.Enabled = isCan;
                btnDeleteEmrQcItem.Enabled = isCan;

                btnSave.Enabled = !isCan;
                btnCancel.Enabled = !isCan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 已注销 xlb 2013-01-15
        ///// <summary>
        ///// 清除文本区域内容
        /////  xlb 2013-01-08
        ///// </summary>
        //private void ClearEditText()
        //{
        //    try
        //    {
        //        textEditCodeName.Text = string.Empty;
        //        textEditCode.Text = string.Empty;
        //        textEditICode.Text = string.Empty;
        //        textEditICodeName.Text = string.Empty;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        /// <summary>
        /// 清除指定区域组件内容
        /// xlb 2013-01-15
        /// </summary>
        /// <param name="control"></param>
        private void ClearControl(Control control)
        {
            try
            {
                foreach (Control ctrl in control.Controls)
                {
                    Type type = ctrl.GetType();
                    if (type == typeof(TextEdit))
                    {
                        TextEdit txtEdit = ctrl as TextEdit;
                        if (txtEdit != null)
                        {
                            txtEdit.Text = string.Empty;
                        }
                    }
                    else if (type == typeof(ComboBoxEdit))
                    {
                        ComboBoxEdit cmbEdit = ctrl as ComboBoxEdit;
                        if (cmbEdit != null)
                        {
                            cmbEdit.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存方法
        /// xlb 2013-01-08
        /// </summary>
        private void SaveEmrQcItem()
        {
            try
            {
                string message = "";
                if (!ValidateEdit(ref message))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(message);
                    return;
                }
                emrQcItem = new EmrQcItem();
                emrQcItem.Name = textEditCodeName.Text;
                emrQcItem.Code = textEditCode.Text;
                emrQcItem.I_Code = textEditICode.Text.Trim();
                emrQcItem.I_Name = textEditICodeName.Text;
                List<EmrQcItem> emrQcItemList = gridControlEmrQcItem.DataSource as List<EmrQcItem>;
                if (emrQcItemList == null)
                {
                    emrQcItemList = new List<EmrQcItem>();
                }

                if (textEditICode.Enabled)
                {
                    SqlParameter[] sps = { new SqlParameter("@ICODE", textEditICode.Text.Trim()) };
                    DataTable dt = DS_SqlHelper.ExecuteDataTable("select count(1) from emrqcitem where I_CODE=@ICODE ", sps, CommandType.Text);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        int count = int.Parse(dt.Rows[0][0].ToString());
                        if (count > 0)
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该监控代码已存在");
                            textEditICode.Focus();
                            return;
                        }
                    }
                    EmrQcItem.InsertToEmrQcItem(emrQcItem);
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("新增成功");
                    emrQcItemList.Add(emrQcItem);
                }
                else
                {
                    EmrQcItem.UpdateToEmrQcItem(emrQcItem);
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("修改成功");
                }
                gridControlEmrQcItem.DataSource = new List<EmrQcItem>(emrQcItemList);
                gridViewEmrQcItem.MoveBy(emrQcItemList.Count - 1);

                SetEditState(false, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 校验方法
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool ValidateEdit(ref string message)
        {
            try
            {
                if (string.IsNullOrEmpty(textEditCode.Text))
                {
                    message = "分类代码不能为空";
                    textEditCode.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textEditCodeName.Text))
                {
                    message = "分类名称不能为空";
                    textEditCodeName.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textEditICode.Text))
                {
                    message = "监控代码不能为空";
                    textEditICode.Focus();
                    return false;

                }
                else if (string.IsNullOrEmpty(textEditICodeName.Text))
                {
                    message = "监控名称不能为空";
                    textEditICodeName.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}