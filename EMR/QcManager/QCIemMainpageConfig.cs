using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.QcManager
{
    /// <summary>
    /// add by wyt
    /// </summary>
    public partial class QCIemMainpageConfig : DevBaseForm
    {
        IEmrHost m_app;
        OperType m_opertype;
        /// <summary>
        /// 实体变量  用来储存实体的值 王冀 2012 12 19
        /// </summary>
        IemMainpageQC qc = new IemMainpageQC();

        public QCIemMainpageConfig(IEmrHost app)
        {
            m_app = app;
            DataAccess.App = app;
            m_opertype = OperType.QUERY;
            InitializeComponent();
        }

        private void gViewConfigPoint_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DrectSoft.Common.DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.ToString());
            }
        }

        /// <summary>
        /// 窗体加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QCIemMainpageConfig_Load(object sender, EventArgs e)
        {
            try
            {

                DataTable dtMainpageQC = DataAccess.OperIemMainPageQC(qc, OperType.QUERY);
                this.gridControlConfig.DataSource = dtMainpageQC;
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// 根据状态重设按钮及编辑区的可操作与否 edit by 王冀 2012 12 19
        /// </summary>
        private void FreshOperView()
        {
            try
            {
                switch (m_opertype)
                {
                    case OperType.QUERY:
                        //foreach (Control c in panelControl.Controls)
                        //{
                        //    c.Enabled = false;
                        //    if (c.GetType() != typeof(LabelControl))
                        //    {
                        //        c.Text = "";
                        //    }
                        //}

                        this.groupBox1.Enabled = false;
                        this.chkHaveCondition.Checked = false;
                        foreach (Control c in groupBox1.Controls)
                        {
                            if (c.GetType() != typeof(LabelControl) && c.GetType() != typeof(RadioButton) && c.GetType() != typeof(CheckBox))
                            {
                                c.Text = "";
                            }
                        }
                        this.groupBox2.Enabled = false;
                        foreach (Control c in groupBox2.Controls)
                        {
                            if (c.GetType() != typeof(LabelControl) && c.GetType() != typeof(RadioButton))
                            {
                                c.Text = "";
                            }
                        }
                        panelControlButton.Enabled = true;
                        foreach (Control c in panelControlButton.Controls)
                        {
                            if (c.Name == "btnADD" || c.Name == "btnEdit" || c.Name == "btnDel")
                            {
                                c.Enabled = true;
                            }
                            else
                            {
                                c.Enabled = false;
                            }
                        }
                        break;
                    case OperType.ADD:
                        //foreach (Control c in panelControl.Controls)
                        //{
                        //    c.Enabled = true;
                        //    if (c.GetType() != typeof(LabelControl))
                        //    {
                        //        c.Text = "";
                        //    }
                        //}
                        this.groupBox1.Enabled = true;
                        this.chkHaveCondition.Checked = false;
                        foreach (Control c in groupBox1.Controls)
                        {
                            if (c.GetType() != typeof(LabelControl) && c.GetType() != typeof(RadioButton) && c.GetType() != typeof(CheckBox))
                            {
                                c.Text = "";
                            }
                        }
                        this.groupBox2.Enabled = true;
                        foreach (Control c in groupBox2.Controls)
                        {
                            if (c.GetType() != typeof(LabelControl) && c.GetType() != typeof(RadioButton))
                            {
                                c.Text = "";
                            }
                        }
                        foreach (Control c in panelControlButton.Controls)
                        {
                            if (c.Name == "btnADD" || c.Name == "btnEdit" || c.Name == "btnDel")
                            {
                                c.Enabled = false;
                            }
                            else
                            {
                                c.Enabled = true;
                            }
                        }
                        break;
                    case OperType.EDIT:
                        //foreach (Control c in panelControl.Controls)
                        //{
                        //    c.Enabled = true;
                        //}
                        this.groupBox1.Enabled = true;
                        this.groupBox2.Enabled = true;
                        foreach (Control c in panelControlButton.Controls)
                        {
                            if (c.Name == "btnADD" || c.Name == "btnEdit" || c.Name == "btnDel")
                            {
                                c.Enabled = false;
                            }
                            else
                            {
                                c.Enabled = true;
                            }
                        }
                        break;
                    case OperType.DEL:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 将编辑区的值传给实体  edit by 王冀 2012 12 19
        /// </summary>
        /// <returns></returns>
        private IemMainpageQC GetQCFromView()
        {
            try
            {
                if (comboBoxEditTableType.SelectedIndex == -1)
                {
                    m_app.CustomMessageBox.MessageShow("表类型不能为空");
                    this.comboBoxEditTableType.Focus();
                    return null;
                }
                if (textEditFields.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("校验字段不能为空");
                    this.textEditFields.Focus();
                    return null;
                }
                if (rbValues.Checked && this.textEditValues.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("校验值不能为空");
                    this.textEditValues.Focus();
                    return null;
                }
                if (chkHaveCondition.Checked)
                {
                    if (comboBoxEditConditionTable.SelectedIndex == -1)
                    {
                        m_app.CustomMessageBox.MessageShow("条件表类型不能为空");
                        this.comboBoxEditConditionTable.Focus();
                        return null;
                    }
                    if (textEditConditionFields.Text.Trim() == "")
                    {
                        m_app.CustomMessageBox.MessageShow("条件校验字段不能为空");
                        this.textEditConditionFields.Focus();
                        return null;
                    }
                    if (rbConditionValues.Checked && this.textEditConditionValues.Text.Trim() == "")
                    {
                        m_app.CustomMessageBox.MessageShow("条件值不能为空");
                        this.textEditConditionValues.Focus();
                        return null;
                    }
                }
                if (textEditReductScore.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("扣分值不能为空");
                    this.textEditReductScore.Focus();
                    return null;
                }
                if (textEditReductReason.Text.Trim() == "")
                {
                    m_app.CustomMessageBox.MessageShow("扣分原因不能为空");
                    this.textEditReductReason.Focus();
                    return null;
                }
                if (this.cmbValid.SelectedIndex == -1)
                {
                    m_app.CustomMessageBox.MessageShow("有效性不能为空");
                    this.cmbValid.Focus();
                    return null;
                }
                IemMainpageQC qc = new IemMainpageQC();
                qc.ID = textEditID.Text.Trim();
                qc.TableType = comboBoxEditTableType.SelectedIndex.ToString();
                qc.Fields = textEditFields.Text.Trim();
                if (rbNotNull.Checked)
                {
                    qc.FieldsValue = "不为空";
                }
                else
                {
                    qc.FieldsValue = textEditValues.Text.Trim();
                }
                if (chkHaveCondition.Checked)
                {
                    qc.ConditionTableType = comboBoxEditTableType.SelectedIndex.ToString();
                    qc.ConditionFields = textEditConditionFields.Text.Trim();
                    if (rbConditionNotNull.Checked)
                    {
                        qc.ConditionValues = "不为空";
                    }
                    else
                    {
                        qc.ConditionValues = textEditConditionValues.Text.Trim();
                    }
                }
                try
                {
                    qc.ReductScore = float.Parse(textEditReductScore.Text.Trim());
                }
                catch (Exception)
                {
                    m_app.CustomMessageBox.MessageShow("分数为无效数字");
                    this.textEditReductScore.Focus();
                    return null;
                }

                qc.ReductReason = textEditReductReason.Text.Trim();
                qc.Valide = cmbValid.SelectedIndex;
                return qc;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        /// <summary>
        /// 将选中行的值赋给下面的编辑区  edit by 王冀 2012 12 19
        /// </summary>
        private void LoadQCView()
        {
            try
            {
                DataTable dt = this.gridControlConfig.DataSource as DataTable;
                int index = this.gViewConfig.FocusedRowHandle;
                if (index >= 0 && index < dt.Rows.Count)
                {
                    DataRow dr = dt.Rows[index];
                    this.textEditID.Text = dr["ID"].ToString();
                    switch (dr["TABLETYPE"].ToString())
                    {
                        case "基本信息表":
                            this.comboBoxEditTableType.SelectedIndex = 0;
                            break;
                        case "诊断表":
                            this.comboBoxEditTableType.SelectedIndex = 1;
                            break;
                        case "手术表":
                            this.comboBoxEditTableType.SelectedIndex = 2;
                            break;
                        case "婴儿表":
                            this.comboBoxEditTableType.SelectedIndex = 3;
                            break;
                        case "病人表":
                            this.comboBoxEditTableType.SelectedIndex = 4;
                            break;
                        default:
                            this.comboBoxEditTableType.SelectedIndex = -1;
                            break;
                    }
                    this.textEditFields.Text = dr["FIELDS"].ToString();
                    string fieldsvalue = dr["FIELDSVALUE"].ToString();
                    if (fieldsvalue == "不为空" || fieldsvalue == "")
                    {
                        rbNotNull.Checked = true;
                        this.textEditValues.Text = "";
                    }
                    else
                    {
                        rbValues.Checked = true;
                        this.textEditValues.Text = fieldsvalue;
                    }
                    switch (dr["CONDITIONTABLETYPE"].ToString())
                    {
                        case "基本信息表":
                            this.comboBoxEditConditionTable.SelectedIndex = 0;
                            break;
                        case "诊断表":
                            this.comboBoxEditConditionTable.SelectedIndex = 1;
                            break;
                        case "手术表":
                            this.comboBoxEditConditionTable.SelectedIndex = 2;
                            break;
                        case "婴儿表":
                            this.comboBoxEditConditionTable.SelectedIndex = 3;
                            break;
                        case "病人表":
                            this.comboBoxEditConditionTable.SelectedIndex = 4;
                            break;
                        default:
                            this.comboBoxEditConditionTable.SelectedIndex = -1;
                            break;
                    }
                    this.textEditConditionFields.Text = dr["CONDITIONFIELDS"].ToString();
                    string conditionValues = dr["CONDITIONFIELDSVALUE"].ToString();
                    if (conditionValues == "不为空" || conditionValues == "")
                    {
                        rbConditionNotNull.Checked = true;
                        this.textEditConditionValues.Text = "";
                    }
                    else
                    {
                        rbConditionValues.Checked = true;
                        this.textEditConditionValues.Text = conditionValues;
                    }
                    if (this.comboBoxEditConditionTable.SelectedIndex != -1)
                    {
                        this.chkHaveCondition.Checked = true;
                    }
                    else
                    {
                        this.chkHaveCondition.Checked = false;
                    }
                    this.textEditReductScore.Text = dr["REDUCTSCORE"].ToString();
                    this.textEditReductReason.Text = dr["REDUCTREASON"].ToString();
                    switch (dr["VALIDE"].ToString())
                    {
                        case "否":
                            this.cmbValid.SelectedIndex = 0;
                            break;
                        case "是":
                            this.cmbValid.SelectedIndex = 1;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 添加 edit by 王冀 2012 12 19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnADD_Click(object sender, EventArgs e)
        {
            try
            {
                m_opertype = OperType.ADD;
                this.FreshOperView();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// 编辑  edit by 王冀 2012 12 19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                m_opertype = OperType.EDIT;
                this.FreshOperView();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
        /// <summary>
        /// 删除 edit by 王冀 2012 12 19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.OK == m_app.CustomMessageBox.MessageShow("确定要删除标准？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
                {
                    if (this.textEditID.Text == "")
                    {
                        m_app.CustomMessageBox.MessageShow("请选择一条删除标准");
                        return;
                    }
                    m_opertype = OperType.DEL;
                    this.FreshOperView();
                    IemMainpageQC qc = this.GetQCFromView();
                    if (qc == null)
                    {
                        return;
                    }
                    DataAccess.OperIemMainPageQC(qc, m_opertype);
                    //DataTable dtMainpageQC = DataAccess.OperIemMainPageQC(qc, OperType.QUERY);
                    //this.gridControlConfig.DataSource = dtMainpageQC;
                    m_app.CustomMessageBox.MessageShow("删除成功");
                    m_opertype = OperType.QUERY;
                    this.FreshOperView();
                    refreshData();
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// 保存 edit by 王冀 2012 12 19
        /// Modify by xlb 2013-06-18
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_opertype == OperType.ADD)
                {
                    qc = this.GetQCFromView();
                    if (qc == null)
                    {
                        return;
                    }
                    qc.ID = Guid.NewGuid().ToString().Substring(16, 16);
                    DataAccess.OperIemMainPageQC(qc, m_opertype);
                    m_app.CustomMessageBox.MessageShow("新增成功");
                    m_opertype = OperType.QUERY;
                    this.FreshOperView();
                    refreshData();

                }
                else if (m_opertype == OperType.EDIT)
                {
                    int rh = gViewConfig.FocusedRowHandle;
                    qc = this.GetQCFromView();
                    if (qc == null)
                    {
                        return;
                    }
                    DataAccess.OperIemMainPageQC(qc, m_opertype);
                    m_app.CustomMessageBox.MessageShow("修改成功");
                    m_opertype = OperType.QUERY;
                    this.FreshOperView();
                    refreshData();
                    gViewConfig.FocusedRowHandle = rh;
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }


        /// <summary>
        /// 取消  edit by 王冀 2012 12 19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                m_opertype = OperType.QUERY;
                this.FreshOperView();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void textEditReductScore_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar < 48 && e.KeyChar > 57 && e.KeyChar != '-' && e.KeyChar != '-')
                {
                    e.Handled = true;    //true表示把这次按键给取消掉
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }


        private void rbNotNull_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbNotNull.Checked)
                {
                    this.textEditValues.Enabled = false;
                }
                else
                {
                    this.textEditValues.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void rbValues_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbNotNull.Checked)
                {
                    this.textEditValues.Enabled = false;
                }
                else
                {
                    this.textEditValues.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void rbConditionNotNull_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbConditionNotNull.Checked)
                {
                    this.textEditConditionValues.Enabled = false;
                }
                else
                {
                    this.textEditConditionValues.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void rbConditionValues_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbConditionNotNull.Checked)
                {
                    this.textEditConditionValues.Enabled = false;
                }
                else
                {
                    this.textEditConditionValues.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
        /// <summary>
        /// 行选中改变时发生
        /// add by 王冀 2012 12 19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gViewConfig_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (m_opertype == OperType.ADD || m_opertype == OperType.EDIT)
                {
                    if (DialogResult.OK != m_app.CustomMessageBox.MessageShow("是否保存记录？", DrectSoft.Core.CustomMessageBoxKind.QuestionOkCancel))
                    {
                        this.LoadQCView();
                        btnCancel_Click(sender, e);
                        return;
                    }
                    btnSave_Click(sender, e);
                    return;
                }
                this.LoadQCView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 刷新数据
        /// add by 王冀 2012 12 19
        /// </summary>
        private void refreshData()
        {
            try
            {
                DataTable dtMainpageQC = DataAccess.OperIemMainPageQC(qc, OperType.QUERY);
                this.gridControlConfig.DataSource = dtMainpageQC;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}