using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Emr.QCTimeLimit.QCEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Emr.QCTimeLimit.QCControlUse
{
    /// <summary>
    /// 时限条件维护界面
    /// by xlb 2013-01-03
    /// </summary>

    public partial class QCTimeCondition : DevBaseForm
    {
        string formCaption;//窗体名
        QCCondition _qcCondition;
        List<QCCondition> conditionList;//维护条件集合
        #region Enum xlb 2013-01-07
        private enum EditState
        {
            /// <summary>
            /// 无操作
            /// </summary>
            None = 0,
            /// <summary>
            /// 新增状态
            /// </summary>
            Add = 1,
            /// <summary>
            /// 编辑状态
            /// </summary>
            Edit = 2
        }
        #endregion
        EditState _editState = EditState.None;

        #region 构造 xlb 2013-01-08
        /// <summary>
        /// 构造方法
        /// </summary>
        public QCTimeCondition()
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

        /// <summary>
        /// 构造
        /// xlb 2013-01-06
        /// </summary>
        /// <param name="fCaption"></param>
        public QCTimeCondition(string fCaption)
        {
            try
            {
                formCaption = fCaption == null ? "" : fCaption.Substring(0, fCaption.IndexOf("("));
                InitializeComponent();
                SetEditState(false, false);
                this.ActiveControl = textEditCode;
                RegisterEvent();//事件
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 事件 xlb 2013-01-08
        /// <summary>
        /// 窗体加载事件
        /// by xlb 2013-01-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QCTimeControl_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = formCaption;
                InitDataConditon();
                textEditCode.Focus();
                textEditConditionCode.Enabled = false;//条件代码框不可编辑
                //禁掉右键菜单 xlb 2013-03-01
                DS_Common.CancelMenu(panelControlTop, contextMenuStrip1);
                DS_Common.CancelMenu(groupControl1, contextMenuStrip1);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 生成SQL语句事件
        /// xlb 2013-01-15
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToSql_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = textEditConditionTable.Text;
                string columnName = textEditConditionColumn.Text;
                string columnValue = textEditColumnValue.Text;
                string timeColumnName = textEditTimeColumn.Text;
                int times = TimesToSeconds(textEditTimeRange.Text);
                string timeRange = Convert.ToString(times / 60 / 60 / 24);
                if (timeColumnName != "" && timeRange != "" && timeRange != "0")
                {
                    textEditSql.Text = string.Format("select * from {0} where {1} {2} and {3}> sysdate-{4}", tableName, columnName, columnValue, timeColumnName, timeRange);
                }
                else
                {
                    textEditSql.Text = string.Format("select * from {0} where {1} {2}", tableName, columnName, columnValue);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 单击GridControl事件
        /// xlb 2013-01-09
        /// 应对focusedrowchanged失效时使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCondition_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewCondition.FocusedRowHandle < 0)
                {
                    return;
                }
                _qcCondition = gridViewCondition.GetRow(gridViewCondition.FocusedRowHandle) as QCCondition;
                DS_Common.ClearControl(groupControl1);
                ShowCondition(_qcCondition);
                SetEditState(false, false);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑事件
        /// by xlb 2013-01-07
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_qcCondition.Code))
                {
                    MessageBox.Show("请选择数据");
                    return;
                }
                SetEditState(true, true);
                _editState = EditState.Edit;
                textEditConditonDes.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// xlb 2013-01-07
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DS_Common.ClearControl(groupControl1);
                SetEditState(false, false);
                _qcCondition = new QCCondition();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 查询事件
        /// by xlb 2013-01-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// by xlb 2013-01-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                DS_Common.ClearControl(panelControlTop);
                InitDataConditon();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增时限条件事件
        /// by xlb 2013-01-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddConditon_Click(object sender, EventArgs e)
        {
            try
            {
                SetEditState(true, true);
                DS_Common.ClearControl(groupControl1);
                _qcCondition = new QCCondition();
                _editState = EditState.Add;
                this.textEditConditionCode.Text = (CalMaxQcConditionID() + 1).ToString();
                textEditConditonDes.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// xlb 2013-01-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCondition();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// by xlb 2013-01-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteCondition_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewCondition.FocusedRowHandle < 0)
                {
                    MessageBox.Show("没有数据");
                    return;
                }
                int rowHandel = gridViewCondition.FocusedRowHandle;
                _qcCondition = gridViewCondition.GetRow(rowHandel) as QCCondition;
                int count = QCRule.QcRuleCountByConditionCode(_qcCondition.Code);
                if (count > 0)
                {
                    MessageBox.Show("该条件已匹配了规则，无法删除");
                    return;
                }

                if (MyMessageBox.Show("您确定删除数据吗？", "提示", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                #region           ======注销 by xlb 2013-02-29=======
                //DataTable dtRuleTable = QCRule.GetAllQCRules();
                //if (dtRuleTable == null || dtRuleTable.Rows.Count <= 0)
                //{
                //    return;
                //}
                //foreach (DataRow dr in dtRuleTable.Rows)
                //{
                //    if (dr["CONDITIONCODE"].ToString() == _qcCondition.Code)
                //    {
                //        MessageBox.Show("该条件已匹配了规则，无法删除");
                //        return;
                //    }
                //}
                #endregion
                QCCondition.DeleteCondition(_qcCondition);
                MessageBox.Show("删除成功");
                InitDataConditon();
                DS_Common.ClearControl(groupControl1);
                gridViewCondition.MoveBy(rowHandel);//焦点定位到下一行
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        ///选择行变化触发的事件
        ///xlb 2013-01-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCondition_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle < 0)
                {
                    return;
                }
                _qcCondition = gridViewCondition.GetRow(e.FocusedRowHandle) as QCCondition;
                DS_Common.ClearControl(groupControl1);
                ShowCondition(_qcCondition);
                SetEditState(false, false);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 绘制序号列
        /// by xlb 2013-01-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewCondition_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
        /// 关闭弹出窗体事件
        /// xlb 2013-01-06
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
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 方法 xlb 2013-01-08

        /// <summary>
        /// 初始化时限条件集合数据
        /// xlb 2013-01-06
        /// </summary>
        private void InitDataConditon()
        {
            try
            {
                _qcCondition = new QCCondition();
                conditionList = QCCondition.getAllConditions(_qcCondition);
                gridControlCondition.DataSource = conditionList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 时限规则条件信息
        /// xlb 2013-01-06
        /// </summary>
        /// <param name="_qcCondition"></param>
        private void ShowCondition(QCCondition _qcCondition)
        {
            try
            {
                if (_qcCondition == null)
                {
                    throw new Exception("没有数据");
                }
                textEditConditionCode.Text = _qcCondition.Code;
                textEditConditonDes.Text = _qcCondition.Description;
                textEditConditionTable.Text = _qcCondition.TableName;
                textEditConditionColumn.Text = _qcCondition.ColumnName;
                textEditColumnValue.Text = _qcCondition.ColumnValue;
                textEditTimeColumn.Text = _qcCondition.TimeColumnName;
                int time = _qcCondition.TimeRange;
                TimeSpan times = new TimeSpan(0, 0, 0, time);
                textEditTimeRange.Text = DS_Common.TimeSpanToLocal(times);
                textEditPatColumn.Text = _qcCondition.PatNoColumnName;
                textEditMemo.Text = _qcCondition.Memo;
                if (_qcCondition.DBLink == "EMRDB")
                {
                    comboBoxEditDBLink.SelectedIndex = 0;
                }
                else
                {
                    comboBoxEditDBLink.SelectedIndex = 1;
                }

                string timeRange = Convert.ToString((int)_qcCondition.TimeRange / 60 / 60 / 24);
                if (timeRange != "" && timeRange != "" && timeRange != "0")
                {
                    textEditSql.Text = string.Format("select * from {0} where {1} {2} and {3}> sysdate-{4}",
                                                       _qcCondition.TableName,
                                                       _qcCondition.ColumnName,
                                                       _qcCondition.ColumnValue,
                                                       _qcCondition.TimeColumnName,
                                                       timeRange);
                }
                else
                {
                    textEditSql.Text = string.Format("select * from {0} where {1} {2}",
                                                      _qcCondition.TableName,
                                                      _qcCondition.ColumnName,
                                                      _qcCondition.ColumnValue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询方法
        /// by xlb 2013-01-06
        /// </summary>
        private void Search()
        {
            try
            {
                _qcCondition = new QCCondition();
                _qcCondition.Code = this.textEditCode.Text.Trim();
                _qcCondition.Description = this.textEditName.Text.Trim();
                List<QCCondition> getQcCondition = QCCondition.getAllConditions(_qcCondition);
                gridControlCondition.DataSource = getQcCondition;

                #region ---------------注销by xlb 2013-02-02--同时操作取不到最新数据集-------
                //List<QCCondition> qcConditionList = conditionList.FindAll(s =>
                //(s.Code.Contains(this.textEditCode.Text.Trim())
                //&& s.Description.Contains(textEditName.Text.Trim())))
                //as List<QCCondition>;
                //gridControlCondition.DataSource = qcConditionList;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存时限规则条件
        /// xlb 2013-01-07
        /// </summary>
        private void SaveCondition()
        {
            try
            {
                if (_qcCondition == null)
                {
                    return;
                }
                string message = "";
                bool result = ValidateInfo(ref message);
                if (!result)
                {
                    MessageBox.Show(message);
                    return;
                }
                _qcCondition.Code = this.textEditConditionCode.Text.Trim();
                _qcCondition.Description = this.textEditConditonDes.Text.Trim();
                _qcCondition.TableName = this.textEditConditionTable.Text.Trim();
                _qcCondition.ColumnName = this.textEditConditionColumn.Text.Trim();
                _qcCondition.ColumnValue = this.textEditColumnValue.Text.Trim();
                _qcCondition.TimeColumnName = this.textEditTimeColumn.Text.Trim();
                _qcCondition.TimeRange = TimesToSeconds(textEditTimeRange.Text.Trim());
                _qcCondition.PatNoColumnName = this.textEditPatColumn.Text.Trim();
                _qcCondition.Memo = textEditMemo.Text.Trim();
                _qcCondition.DBLink = comboBoxEditDBLink.SelectedItem.ToString();

                //质控条件集合
                List<QCCondition> qcConditionList = gridControlCondition.DataSource as List<QCCondition>;
                if (qcConditionList == null)
                {
                    qcConditionList = new List<QCCondition>();
                }

                if (_editState == EditState.Add)
                {
                    QCCondition.InsertCondition(_qcCondition);
                    MessageBox.Show("添加成功");
                    //集合中插入新增数据行 便于 定位焦点在新增行上
                    qcConditionList.Add(_qcCondition);
                }
                else
                {
                    QCCondition.UpdateCondition(_qcCondition);
                    MessageBox.Show("修改成功");
                }

                //绑定数据源
                gridControlCondition.DataSource = new List<QCCondition>(qcConditionList);
                //定位焦点在最后一行
                gridViewCondition.MoveBy(qcConditionList.Count - 1);
                DS_Common.ClearControl(groupControl1);
                SetEditState(false, false);
                _qcCondition = new QCCondition();
                gridViewCondition.FocusedRowHandle = (gridControlCondition.DataSource as List<QCCondition>).Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 产生时限规则条件最大编号
        /// by xlb 2013-01-07
        /// </summary>
        /// <returns></returns>
        private int CalMaxQcConditionID()
        {
            try
            {
                int maxid = 0;
                DataTable dtAllCondition = QCCondition.GetAllQCConditions();
                if (dtAllCondition == null || dtAllCondition.Rows.Count <= 0)
                {
                    return maxid;
                }
                foreach (DataRow dr in dtAllCondition.Rows)
                {
                    if (int.Parse(dr["CODE"].ToString()) > maxid)
                    {
                        maxid = int.Parse(dr["CODE"].ToString());
                    }
                }
                return maxid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置编辑区域是否可使用
        /// by xlb 2013-01-07
        /// </summary>
        /// <param name="isSave"></param>
        private void SetEditState(bool isEdit, bool isCan)
        {
            try
            {
                textEditConditonDes.Enabled = isEdit;
                textEditConditionTable.Enabled = isEdit;
                textEditConditionColumn.Enabled = isEdit;
                textEditColumnValue.Enabled = isEdit;
                textEditTimeColumn.Enabled = isEdit;
                textEditTimeRange.Enabled = isEdit;
                textEditPatColumn.Enabled = isEdit;
                comboBoxEditDBLink.Enabled = isEdit;
                textEditMemo.Enabled = isEdit;
                textEditSql.Enabled = isEdit;
                btnToSql.Enabled = isEdit;

                btnAddConditon.Enabled = !isCan;
                btnEdit.Enabled = !isCan;
                btnDeleteCondition.Enabled = !isCan;
                btnCancel.Enabled = isCan;
                btnSave.Enabled = isCan;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证控件是否空方法
        /// xlb 2013-01-07
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool ValidateInfo(ref string message)
        {
            try
            {
                if (string.IsNullOrEmpty(textEditConditonDes.Text))
                {
                    message = "条件名称不能为空";
                    textEditConditonDes.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textEditConditionTable.Text))
                {
                    message = "配置表名不能为空";
                    textEditConditionTable.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textEditConditionColumn.Text))
                {
                    message = "对应列名不能为空";
                    textEditConditionColumn.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textEditColumnValue.Text))
                {
                    message = "对应列值不能为空";
                    textEditColumnValue.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textEditTimeColumn.Text))
                {
                    message = "时间列不能为空";
                    textEditTimeColumn.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textEditTimeRange.Text))
                {

                    message = "时间范围不能为空";
                    textEditTimeRange.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textEditPatColumn.Text))
                {
                    message = "病序列名不能为空";
                    textEditPatColumn.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 建立事件方法
        /// xlb 2013-01-15
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                btnSearch.Click += new EventHandler(btnSearch_Click);
                btnReset.Click += new EventHandler(btnReset_Click);
                btnAddConditon.Click += new EventHandler(btnAddConditon_Click);
                btnSave.Click += new EventHandler(btnSave_Click);
                btnDeleteCondition.Click += new EventHandler(btnDeleteCondition_Click);
                btnClose.Click += new EventHandler(btnClose_Click);
                gridViewCondition.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewCondition_FocusedRowChanged);
                btnEdit.Click += new EventHandler(btnEdit_Click);
                btnCancel.Click += new EventHandler(btnCancel_Click);
                gridViewCondition.Click += new EventHandler(gridViewCondition_Click);
                btnToSql.Click += new EventHandler(btnToSql_Click);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 将格式时间转换成秒数
        /// xlb 2013-01-15
        /// </summary>
        /// <param name="timeRange"></param>
        /// <returns></returns>
        private int TimesToSeconds(string timeRange)
        {
            try
            {
                if (string.IsNullOrEmpty(timeRange))
                {
                    return 0;
                }
                int days = 0, hours = 0, mins = 0;
                string temp = timeRange.Trim();
                int index = temp.IndexOf("天");
                if (index > 0)
                {
                    days = int.Parse(temp.Substring(0, temp.IndexOf("天")));
                    temp = temp.Substring(index + 1, temp.Length - index - 1);
                }
                else
                {
                    days = 0;
                    temp = temp.Substring(index + 1, temp.Length - index - 1);
                }
                if (temp.IndexOf("时") > 0)
                {
                    hours = int.Parse(temp.Substring(0, temp.IndexOf("时")));
                    temp = temp.Substring(temp.IndexOf("时") + 1, temp.Length - temp.IndexOf("时") - 1);
                }
                else
                {
                    hours = 0;
                    temp = temp.Substring(temp.IndexOf("时") + 1, temp.Length - temp.IndexOf("时") - 1);
                }
                if (temp.IndexOf("分") > 0)
                {
                    mins = int.Parse(temp.Substring(0, temp.IndexOf("分")));
                }
                else
                {
                    mins = 0;
                }
                TimeSpan timeSpan = new TimeSpan(days, hours, mins, 0);
                int totalSeconds = (int)timeSpan.TotalSeconds;
                return totalSeconds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

    }
}