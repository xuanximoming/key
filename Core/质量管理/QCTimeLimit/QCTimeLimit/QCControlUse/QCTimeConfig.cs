using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.DSSqlHelper;
using DrectSoft.Emr.QCTimeLimit.QCEntity;
using DrectSoft.Emr.QCTimeLimit.QCEnum;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Emr.QCTimeLimit.QCControlUse
{
    /// <summary>
    /// 病历时限维护界面
    /// by xlb 2013-01-05
    /// </summary>
    public partial class QCTimeConfig : DevBaseForm, IStartPlugIn
    {

        QCRule _currentRule;

        private enum EditSate
        {
            None = 0,
            Add = 1,
            Edit = 2
        }
        EditSate editstate = EditSate.None;

        /// <summary>
        /// 构造方法
        ///   xlb
        /// 2013-01-11
        /// </summary>
        public QCTimeConfig()
        {
            try
            {
                InitializeComponent();
                DS_SqlHelper.CreateSqlHelper();
                InitDataEmrQcItem();
                InitDataRule();
                InitLookUpQcCondition();
                InitRuleCategoryName();
                InitLookUpEditDocLevel();
                SetRuleEditState(false, false);
                RegisterEvent();
                textBoxRuleId.Enabled = false;
                //禁掉第三方控件右键菜单
                DS_Common.CancelMenu(groupControlCondtion, contextMenuStrip1);
                DS_Common.CancelMenu(groupControlRule, contextMenuStrip1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 事件

        /// <summary>
        /// 维护条件单击事件
        /// by xlb 2013-01-05
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConditionEdit_Click(object sender, EventArgs e)
        {
            try
            {
                QCTimeCondition qcTimeControl = new QCTimeCondition(this.buttonConditionEdit.Text);
                if (qcTimeControl == null)
                {
                    return;
                }
                qcTimeControl.ShowDialog();
                InitLookUpQcCondition();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 维护记录事件
        /// xlb 2013-01-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmrQcItem_Click(object sender, EventArgs e)
        {
            try
            {
                QCEmrItem qcTimeRecord = new QCEmrItem(this.btnEmrQcItem.Text);
                if (qcTimeRecord == null)
                {
                    return;
                }
                qcTimeRecord.ShowDialog();
                //关闭监控代码窗体时刷新监控代码下拉框
                InitDataEmrQcItem();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 禁掉第三方控件右键菜单的方法

        ///// <summary>
        ///// 屏蔽第三方控件右键菜单
        ///// Add xlb 2013-02-29
        ///// </summary>
        ///// <param name="control"></param>
        //private void CancelMenu(Control control,ContextMenuStrip contextMenuStrip)
        //{
        //    try
        //    {
        //        foreach (Control ctrl in control.Controls)
        //        {
        //            Type type = ctrl.GetType();
        //            if (type == typeof(LabelControl))
        //            {
        //                continue;
        //            }
        //            else if (type == typeof(TextEdit))
        //            {
        //                TextEdit textEdit = ctrl as TextEdit;
        //                if (textEdit != null)
        //                {
        //                    textEdit.Properties.ContextMenuStrip = contextMenuStrip;
        //                }
        //            }
        //            else if (type == typeof(MemoEdit))
        //            {
        //                MemoEdit memoEdit = ctrl as MemoEdit;
        //                if (memoEdit != null)
        //                {
        //                    memoEdit.Properties.ContextMenuStrip = contextMenuStrip;
        //                }
        //            }
        //            else if (type == typeof(SpinEdit))
        //            {
        //                SpinEdit spinEdit = ctrl as SpinEdit;
        //                if (spinEdit != null)
        //                {
        //                    spinEdit.Properties.ContextMenuStrip = contextMenuStrip;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        /// <summary>
        /// 选择行变化时出发的事件
        /// by xlb 2013-01-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewRule_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (e.FocusedRowHandle < 0)
                {
                    return;
                }
                _currentRule = gridViewRule.GetRow(e.FocusedRowHandle) as QCRule;
                DS_Common.ClearControl(groupControlRule);
                ShowRule(_currentRule);
                SetRuleEditState(false, false);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 单击列表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewRule.FocusedRowHandle < 0)
                {
                    return;
                }
                _currentRule = gridViewRule.GetRow(gridViewRule.FocusedRowHandle) as QCRule;
                if (_currentRule == null)
                {
                    return;
                }
                DS_Common.ClearControl(groupControlRule);
                DS_Common.ClearControl(groupControlCondtion);
                ShowRule(_currentRule);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// by xlb 2013-01-05
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentRule == null || _currentRule.RuleCode == null)
                {
                    MessageBox.Show("请选择要删除的数据");
                    return;
                }
                DataTable dtQcRecord = QCRecord.GetAllQcRecord(_currentRule.RuleCode);
                int count = int.Parse(dtQcRecord.Rows[0][0].ToString());
                if (count > 0)
                {
                    MessageBox.Show("该规则已被质控记录使用，无法删除");
                    return;
                }
                if (MyMessageBox.Show("您确定删除吗？", "提示", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                QCRule.DeleteQcRule(_currentRule);
                MessageBox.Show("删除成功");
                InitDataRule();
                DS_Common.ClearControl(groupControlRule);
                DS_Common.ClearControl(groupControlCondtion);
                lookUpEditDoctorLevel.EditValue = null;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增规则事件
        /// xlb 2013-01-05
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNewRule_Click(object sender, EventArgs e)
        {
            try
            {
                DS_Common.ClearControl(groupControlRule);
                DS_Common.ClearControl(groupControlCondtion);
                SetRuleEditState(true, true);
                editstate = EditSate.Add;
                _currentRule = new QCRule();
                lookUpEditDoctorLevel.EditValue = null;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 刷新事件
        /// xlb 2013-04-05
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLoadRule_Click(object sender, EventArgs e)
        {
            try
            {
                InitDataRule();
                InitRuleCategoryName();
                DS_Common.ClearControl(groupControlRule);
                DS_Common.ClearControl(groupControlCondtion);
                SetRuleEditState(false, false);
                lookUpEditDoctorLevel.EditValue = null;
                _currentRule = new QCRule();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 已注销 xlb 2013-01-25
        ///// <summary>
        ///// 双击事件
        ///// by xlb 2013-01-05
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void gridViewRule_DoubleClick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (gridViewRule.FocusedRowHandle < 0)
        //        {
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MyMessageBox.Show(1, ex);
        //    }
        //}
        #endregion

        /// <summary>
        /// 编辑事件
        /// by xlb 2013-01-05
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEditRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentRule == null || string.IsNullOrEmpty(_currentRule.RuleCode))
                {
                    MessageBox.Show("请选中一条数据");
                    return;
                }
                SetRuleEditState(true, true);
                editstate = EditSate.Edit;
                textBoxRuleName.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// xlb 203-01-15
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetRuleEditState(false, false);
                _currentRule = new QCRule();
                DS_Common.ClearControl(groupControlRule);
                DS_Common.ClearControl(groupControlCondtion);
                lookUpEditDoctorLevel.EditValue = null;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// xlb 2013-01-07
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveRule_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCurrentRule();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 绘制样式事件
        /// xlb 2013-01-10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewRule_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                QCRule ruleQc = gridViewRule.GetRow(e.RowHandle) as QCRule;
                if (ruleQc.Valid == "0")
                {
                    //e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
                    e.Appearance.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 操作方式下拉框选择行改变触发事件
        /// xlb 2013-01-06
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxOpMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxOpMode.SelectedIndex == 0 || comboBoxOpMode.SelectedIndex == 1)
                {
                    textEditLooptimes.Enabled = false;
                    textEditLoopinterval.Enabled = false;
                    textEditLooptimes.Text = "0";
                    textEditLoopinterval.Text = "0";
                }
                else
                {
                    textEditLooptimes.Enabled = true;
                    textEditLoopinterval.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 病历时限条件代码选择改变时引发的事件
        /// xlb 2013-01-15
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditConditionName_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string code = lookUpEditConditionName.EditValue.ToString();
                SqlParameter[] sps = { new SqlParameter("@code", code) };
                DataTable dt = DS_SqlHelper.ExecuteDataTable("select * from qccondition where code=@code", sps, CommandType.Text);
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return;
                }
                textEditTableName.Text = dt.Rows[0]["TABLENAME"] == null ? "" : dt.Rows[0]["TABLENAME"].ToString();
                textEditColumn.Text = dt.Rows[0]["COLUMNNAME"] == null ? "" : dt.Rows[0]["COLUMNNAME"].ToString();
                textEditColumnValue.Text = dt.Rows[0]["COLUMNVALUE"] == null ? "" : dt.Rows[0]["COLUMNVALUE"].ToString();
                textEditPatColumn.Text = dt.Rows[0]["PATNOCOLUMNNAME"] == null ? "" : dt.Rows[0]["PATNOCOLUMNNAME"].ToString();
                textEditTimeColumn.Text = dt.Rows[0]["TIMECOLUMNNAME"] == null ? "" : dt.Rows[0]["TIMECOLUMNNAME"].ToString();
                if (dt.Rows[0]["TIMERANGE"].ToString() != null && dt.Rows[0]["TIMERANGE"].ToString() != "")
                {
                    int times = Convert.ToInt32(dt.Rows[0]["TIMERANGE"].ToString());
                    TimeSpan timeSpan = new TimeSpan(0, 0, 0, times);
                    textEditTimeRange.Text = DS_Common.TimeSpanToLocal(timeSpan);
                }

                textEditMemo.Text = dt.Rows[0]["MEMO"].ToString();
                if (dt.Rows[0]["TIMECOLUMNNAME"].ToString() != "" && dt.Rows[0]["TIMERANGE"].ToString() != "" && dt.Rows[0]["TIMERANGE"].ToString() != "0")
                {
                    textEditSql.Text = string.Format("select * from {0} where {1} {2} and {3}>sysdate-{4}",
                                                      textEditTableName.Text,
                                                      textEditColumn.Text,
                                                      textEditColumnValue.Text,
                                                      textEditTimeColumn.Text,
                                                      Convert.ToInt32(dt.Rows[0]["TIMERANGE"]) / 60 / 60 / 24);
                }
                else
                {
                    textEditSql.Text = string.Format("select * from {0} where {1} {2}",
                                                      textEditTableName.Text,
                                                      textEditColumn.Text,
                                                      textEditColumnValue.Text);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 限制SpinEdit只能输入非负数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinEditDelayTime_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (e.NewValue != null && e.NewValue.ToString().StartsWith("-"))
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 方法 by xlb 2013-01-05

        /// <summary>
        /// 设置界面控件只读性方法
        /// by xlb 2013-01-05 
        /// </summary>
        /// <param name="isEdit"></param>
        /// <param name="isNew"></param>
        private void SetRuleEditState(bool isEdit, bool isNew)
        {
            try
            {
                groupControlCondtion.Enabled = isEdit;
                groupControlRule.Enabled = isEdit;
                buttonDelRule.Enabled = !isNew;
                buttonEditRule.Enabled = !isNew;
                buttonNewRule.Enabled = !isNew;
                buttonSaveRule.Enabled = isNew;
                btnCancel.Enabled = isNew;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 显示时限详细信息方法
        /// by xlb 2013-01-05
        /// </summary>
        /// <param name="qcRule"></param>
        private void ShowRule(QCRule qcRule)
        {
            try
            {
                if (qcRule == null)
                {
                    return;
                }
                lookUpEditConditionName.EditValue = qcRule.Condition.Code;
                textEditTableName.Text = qcRule.Condition.TableName;
                textEditColumn.Text = qcRule.Condition.ColumnName;
                textEditColumnValue.Text = qcRule.Condition.ColumnValue;
                textEditTimeColumn.Text = qcRule.Condition.TimeColumnName;
                int times = qcRule.Condition.TimeRange;
                TimeSpan timeSpan = new TimeSpan(0, 0, 0, times);
                textEditTimeRange.Text = DS_Common.TimeSpanToLocal(timeSpan);
                textEditPatColumn.Text = qcRule.Condition.PatNoColumnName;
                textEditMemo.Text = qcRule.Memo;
                if (_currentRule.Condition.TableName != "" && _currentRule.Condition.ColumnName != "" && _currentRule.Condition.ColumnValue != "" && _currentRule.Condition.TimeRange > 0)
                {
                    textEditSql.Text = string.Format("select * from {0} where {1} {2} and {3}>sysdate-{4}",
                                                      _currentRule.Condition.TableName,
                                                      _currentRule.Condition.ColumnName,
                                                      _currentRule.Condition.ColumnValue,
                                                      _currentRule.Condition.TimeColumnName,
                                                      _currentRule.Condition.TimeRange / 60 / 60 / 24);
                }
                else
                {
                    textEditSql.Text = string.Format("select * from {0} where {1} {2}",
                                                      _currentRule.Condition.TableName,
                                                      _currentRule.Condition.ColumnName,
                                                      _currentRule.Condition.ColumnValue);
                }

                textBoxRuleId.Text = qcRule.RuleCode;
                textBoxRuleName.Text = qcRule.Description;
                int timesLimit = qcRule.TimeLimit;
                TimeSpan timeSpans = new TimeSpan(0, 0, 0, timesLimit);
                textBoxRuleTime.Text = DS_Common.TimeSpanToLocal(timeSpans);
                textBoxTipInfo.Text = qcRule.Reminder;
                textBoxWarningInfo.Text = qcRule.FoulMessage;
                textEditLooptimes.Text = qcRule.CycleTimes.ToString();
                TimeSpan timeSpanCycle = new TimeSpan(0, 0, 0, qcRule.CycleInterval);
                textEditLoopinterval.Text = DS_Common.TimeSpanToLocal(timeSpanCycle);
                spinEditScore.Text = qcRule.Sorce.ToString();

                lookUpEditQcCode.EditValue = qcRule.QCCode;
                textEditRuleMemo.Text = qcRule.Memo;
                lookUpEditRuleGroup.EditValue = qcRule.RuleCategory.Code;
                lookUpEditDoctorLevel.EditValue = (Decimal)qcRule.DoctorLevel;
                spinEditDelayTime.Text = qcRule.DelayTime.ToString();
                if (qcRule.Valid == "1")
                {
                    checkEditValid.Checked = true;
                }
                else
                {
                    checkEditValid.Checked = false;
                }
                if (qcRule.MARK == OperationType.OnlyOne)
                {
                    comboBoxOpMode.SelectedIndex = 0;
                }
                else if (qcRule.MARK == OperationType.EveryOne)
                {
                    comboBoxOpMode.SelectedIndex = 1;
                }
                else if (qcRule.MARK == OperationType.Circle)
                {
                    comboBoxOpMode.SelectedIndex = 2;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 生成DevLookUpEdit的显示下拉列表
        /// by xlb 2013-01-06
        /// </summary>
        /// <param name="lookupEdit"></param>
        /// <param name="table"></param>
        private void AddLookupColumnInfo(LookUpEdit lookupEdit, KeyValuePair<string, string>[] fields)
        {
            try
            {
                lookupEdit.Properties.Columns.Clear();
                for (int i = 0; i < fields.Length; i++)
                {
                    KeyValuePair<string, string> field = fields[i];
                    LookUpColumnInfo luci = new LookUpColumnInfo(field.Key, field.Value, 100);
                    lookupEdit.Properties.Columns.Add(luci);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化LookUpEdit控件
        /// by xlb 2013-01-06
        /// </summary>
        private void InitRuleCategoryName()
        {
            try
            {
                DataTable dtRuleCategoryName = RuleCategory.GetAllRuleCategorys();
                lookUpEditRuleGroup.Properties.DataSource = dtRuleCategoryName;
                AddLookupColumnInfo(lookUpEditRuleGroup, new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("CODE", RuleCategory.cstFieldCaptionId), 
                new KeyValuePair<string,string>("DESCRIPTION", RuleCategory.cstFieldCaptionDescript)
            });
                lookUpEditRuleGroup.Properties.DisplayMember = "DESCRIPTION";
                lookUpEditRuleGroup.Properties.ValueMember = "CODE";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 已注销 xlb 2013-01-15
        ///// <summary>
        ///// 遍历清除文本框内容
        ///// 2013-01-10
        ///// </summary>
        ///// <param name="gc"></param>
        //private void ClearText(GroupControl gc)
        //{
        //    try
        //    {
        //        foreach (Control ctl in gc.Controls)
        //        {
        //            Type type = ctl.GetType();
        //            if (type == typeof(TextEdit))
        //            {
        //                TextEdit textEdit = ctl as TextEdit;
        //                if (textEdit != null)
        //                {
        //                    textEdit.Text = string.Empty;
        //                }
        //            }
        //            else if (type == typeof(ComboBoxEdit))
        //            {
        //                ComboBoxEdit cmbEdit = ctl as ComboBoxEdit;
        //                if (cmbEdit != null)
        //                {
        //                    cmbEdit.SelectedIndex = -1;
        //                }
        //            }
        //            else if (type == typeof(LookUpEdit))
        //            {
        //                LookUpEdit lookUpEdit = ctl as LookUpEdit;
        //                if (lookUpEdit != null)
        //                {
        //                    lookUpEdit.EditValue = string.Empty;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        /// <summary>
        /// 保存方法
        /// xlb 2013-01-06
        /// </summary>
        private void SaveCurrentRule()
        {
            try
            {
                string message = "";
                bool result = Validate(ref message);
                if (!result)
                {
                    MessageBox.Show(message);
                    return;
                }
                QCRule qcRule = new QCRule();
                RuleCategory ruleCategory = new RuleCategory();
                QCCondition qcCondition = new QCCondition();
                qcRule.RuleCode = textBoxRuleId.Text.Trim();
                qcCondition.Code = lookUpEditConditionName.EditValue.ToString();
                qcRule.TimeLimit = TimesToSeconds(textBoxRuleTime.Text);
                qcRule.Description = textBoxRuleName.Text.Trim();

                qcRule.DoctorLevel = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), lookUpEditDoctorLevel.EditValue.ToString());
                qcRule.Reminder = textBoxTipInfo.Text.Trim();
                qcRule.FoulMessage = textBoxWarningInfo.Text.Trim();
                if (comboBoxOpMode.SelectedIndex >= 0)
                {
                    qcRule.MARK = (OperationType)comboBoxOpMode.SelectedIndex;
                }
                else
                {
                    qcRule.MARK = OperationType.OnlyOne;
                }
                qcRule.CycleTimes = int.Parse(textEditLooptimes.Text == "" ? "0" : textEditLooptimes.Text.Trim());
                qcRule.CycleInterval = TimesToSeconds(textEditLoopinterval.Text == "" ? "0" : textEditLoopinterval.Text.Trim());
                qcRule.DelayTime = int.Parse(spinEditDelayTime.Value.ToString());
                qcRule.Sorce = double.Parse(spinEditScore.Text == "" ? "0" : spinEditScore.Text.Trim());
                qcRule.QCCode = lookUpEditQcCode.EditValue.ToString();
                qcRule.Memo = textEditRuleMemo.Text.Trim();
                ruleCategory.Code = lookUpEditRuleGroup.EditValue.ToString();
                qcRule.Valid = checkEditValid.Checked ? "1" : "0";
                if (editstate == EditSate.Edit)
                {
                    QCRule.UpdateQcRule(qcRule, qcCondition, ruleCategory);
                    MessageBox.Show("修改成功");
                }
                else if (editstate == EditSate.Add)
                {
                    QCRule.InsertQcRule(qcRule, qcCondition, ruleCategory);
                    MessageBox.Show("添加成功");
                }
                InitDataRule();
                SetRuleEditState(false, false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化责任医生下拉框
        /// xlb 2013-01-08
        /// </summary>
        private void InitLookUpEditDocLevel()
        {
            try
            {
                DataTable dtDoctorLevel = DS_SqlHelper.ExecuteDataTable("select * from categorydetail where categoryid='20'", CommandType.Text);
                lookUpEditDoctorLevel.Properties.DataSource = dtDoctorLevel;
                AddLookupColumnInfo(lookUpEditDoctorLevel, new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("ID", "代码"), 
                new KeyValuePair<string,string>("NAME","名称")
            });
                lookUpEditDoctorLevel.Properties.DisplayMember = "NAME";
                lookUpEditDoctorLevel.Properties.ValueMember = "ID";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化病历时限条件下拉框
        /// xlb 2013-01-15
        /// </summary>
        private void InitLookUpQcCondition()
        {
            try
            {
                DataTable dt = DS_SqlHelper.ExecuteDataTable("select code,description from qccondition where valid='1'", CommandType.Text);
                lookUpEditConditionName.Properties.DataSource = dt;
                AddLookupColumnInfo(lookUpEditConditionName, new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("CODE", "代码"), 
                new KeyValuePair<string,string>("DESCRIPTION","描述")
            });
                lookUpEditConditionName.Properties.DisplayMember = "DESCRIPTION";
                lookUpEditConditionName.Properties.ValueMember = "CODE";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 时间转换成秒
        /// xlb 2013-01-09
        /// </summary>
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

        /// <summary>
        /// 初始化时限规则数据集
        /// by xlb 2013-01-05
        /// </summary>
        private void InitDataRule()
        {
            try
            {
                Dictionary<string, QCCondition> dictQCCondition = QCCondition.GetAllQCCondition();
                Dictionary<string, RuleCategory> dictRuleCategory = RuleCategory.GetAllRuleCategory();
                IList<QCRule> listRule = QCRule.GetRuleList(dictQCCondition, dictRuleCategory);

                gridControlRule.DataSource = listRule;
                gridViewRule.ExpandAllGroups();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        ///初始化监控代码下拉框
        ///xlb 2013-01-14
        /// </summary>
        private void InitDataEmrQcItem()
        {
            try
            {
                DataTable dt = DS_SqlHelper.ExecuteDataTable("Select i_code,i_name from emrqcitem ", CommandType.Text);

                lookUpEditQcCode.Properties.DataSource = dt;
                AddLookupColumnInfo(lookUpEditQcCode, new KeyValuePair<string, string>[]{
                new KeyValuePair<string,string>("I_CODE", "代码"), 
                new KeyValuePair<string,string>("I_NAME","名称")
            });
                lookUpEditQcCode.Properties.DisplayMember = "I_NAME";
                lookUpEditQcCode.Properties.ValueMember = "I_CODE";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 校验方法
        /// xlb 2013-01-10
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool Validate(ref string message)
        {
            try
            {
                if (string.IsNullOrEmpty(textBoxRuleName.Text))
                {
                    message = "规则描述不能为空";
                    textBoxRuleName.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(lookUpEditDoctorLevel.Text.ToString()))
                {
                    message = "责任医生不能为空";
                    lookUpEditDoctorLevel.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textBoxRuleTime.Text))
                {
                    message = "时限限制不能为空";
                    textBoxRuleTime.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textBoxTipInfo.Text))
                {
                    message = "提示信息不能为空";
                    textBoxTipInfo.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(textBoxWarningInfo.Text))
                {
                    message = "警告信息不能为空";
                    textBoxWarningInfo.Focus();
                    return false;
                }
                else if (comboBoxOpMode.SelectedIndex < 0)
                {
                    message = "请选择操作方式";
                    comboBoxOpMode.Focus();
                    return false;
                }
                else if (comboBoxOpMode.SelectedItem.ToString().Equals("循环触发")
                    && string.IsNullOrEmpty(textEditLooptimes.Text.ToString()))
                {
                    message = "请输入循环次数";
                    textEditLooptimes.Focus();
                    return false;
                }
                else if (comboBoxOpMode.SelectedItem.ToString().Equals("循环触发") &&
                    string.IsNullOrEmpty(textEditLoopinterval.Text.ToString()))
                {
                    message = "请输入循环间隔时间";
                    textEditLoopinterval.Focus();
                    return false;
                }
                else if (comboBoxOpMode.SelectedItem.ToString().Equals("循环触发") &&
                    TimesToSeconds(textEditLoopinterval.Text) == 0)
                {
                    message = "循环间隔时间不能为0";
                    textEditLoopinterval.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(spinEditScore.Text))
                {
                    message = "请输入扣分";
                    spinEditScore.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(lookUpEditQcCode.Text))
                {
                    message = "请选择监控代码";
                    lookUpEditQcCode.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(lookUpEditRuleGroup.EditValue.ToString()))
                {
                    message = "请选择规则分组";
                    lookUpEditRuleGroup.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(lookUpEditConditionName.Text))
                {
                    message = "请选择条件代码";
                    lookUpEditConditionName.Focus();
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
        /// 建立事件
        /// </summary>
        private void RegisterEvent()
        {
            try
            {
                buttonConditionEdit.Click += new EventHandler(buttonConditionEdit_Click);
                //gridViewRule.DoubleClick += new EventHandler(gridViewRule_DoubleClick);
                gridViewRule.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(gridViewRule_CustomDrawCell);
                buttonLoadRule.Click += new EventHandler(buttonLoadRule_Click);
                buttonNewRule.Click += new EventHandler(buttonNewRule_Click);
                buttonDelRule.Click += new EventHandler(buttonDelRule_Click);
                buttonEditRule.Click += new EventHandler(buttonEditRule_Click);
                gridViewRule.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(gridViewRule_FocusedRowChanged);
                gridViewRule.Click += new EventHandler(gridViewRule_Click);
                buttonSaveRule.Click += new EventHandler(buttonSaveRule_Click);
                btnEmrQcItem.Click += new EventHandler(btnEmrQcItem_Click);
                btnCancel.Click += new EventHandler(btnCancel_Click);
                lookUpEditConditionName.EditValueChanged += new EventHandler(lookUpEditConditionName_EditValueChanged);
                spinEditDelayTime.EditValueChanging += new ChangingEventHandler(spinEditDelayTime_EditValueChanging);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region IStartup Members

        public virtual IPlugIn Run(IEmrHost application)
        {

            //app = application;
            return new PlugIn(this.GetType().ToString(), this);
        }

        #endregion

        private void buttonSaveRule_Click_1(object sender, EventArgs e)
        {

        }

        #endregion
    }
}