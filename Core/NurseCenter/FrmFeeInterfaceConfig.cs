using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.NurseCenter
{
    public partial class FrmFeeInterfaceConfig : DevBaseForm
    {
        #region Field && Property
        /// <summary>
        /// 当前操作类型 新增或修改
        /// </summary>
        OperationType m_CurrentOperationType = OperationType.None;

        IEmrHost m_App;
        #endregion

        #region .ctor
        private FrmFeeInterfaceConfig()
        {
            InitializeComponent();
        }

        public FrmFeeInterfaceConfig(IEmrHost app) : this()
        {
            m_App = app;
        }
        #endregion

        #region Load
        /// <summary>
        /// 初始化参数类型
        /// </summary>
        private void InitParameterType()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("NAME");

                DataRow dr = dt.NewRow();
                dr["ID"] = "VARCHAR";
                dr["NAME"] = "字符";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["ID"] = "DATETIME";
                dr["NAME"] = "日期";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["ID"] = "INT";
                dr["NAME"] = "整数";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["ID"] = "NUMBER";
                dr["NAME"] = "小数";
                dt.Rows.Add(dr);

                lookUpEditParameterType.Properties.DisplayMember = "NAME";
                lookUpEditParameterType.Properties.ValueMember = "ID";
                lookUpEditParameterType.Properties.DataSource = dt;
                lookUpEditParameterType.EditValue = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化参数方向
        /// </summary>
        private void InitParameterDirection()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("ID");
                dt.Columns.Add("NAME");

                DataRow dr = dt.NewRow();
                dr["ID"] = "IN";
                dr["NAME"] = "入参";
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr["ID"] = "OUT";
                dr["NAME"] = "出参";
                dt.Rows.Add(dr);

                lookUpEditParameterDirection.Properties.DisplayMember = "NAME";
                lookUpEditParameterDirection.Properties.ValueMember = "ID";
                lookUpEditParameterDirection.Properties.DataSource = dt;
                lookUpEditParameterDirection.EditValue = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化参数列表中的数据源
        /// </summary>
        private void InitGridControl()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PARAMETERNAME");
                dt.Columns.Add("PARAMETERMEMO");
                dt.Columns.Add("PARAMETERTYPE");
                dt.Columns.Add("PARAMETERTYPENAME");
                dt.Columns.Add("PARAMETERDIRECTION");
                dt.Columns.Add("PARAMETERDIRECTIONNAME");
                dt.Columns.Add("PARAMETERFIELD");
                dt.Columns.Add("GUID");
                dt.Columns.Add("ORDERID");
                dt.Columns.Add("MAINID");
                gridControlParameter.DataSource = dt;
                ((DataTable)gridControlParameter.DataSource).DefaultView.Sort = " orderid ";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化参数主表列表
        /// </summary>
        private void InitParameterMainList()
        {
            try
            {
                string sql = " select * from consultfeeinterfacemain where valid = '1' ";
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                lookUpEditID.Properties.DisplayMember = "ID";
                lookUpEditID.Properties.ValueMember = "ID";
                lookUpEditID.Properties.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    lookUpEditID.EditValue = dt.Rows[0]["ID"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitMain(DataRow dr)
        {
            try
            {
                textEditProcedureName.Text = dr["PROCEDURENAME"].ToString();
                textEditMemo.Text = dr["MEMO"].ToString();
                memoEditSQL.Text = dr["DATASOURCESQL"].ToString();
                checkBoxValid.Checked = dr["ISOPEN"].ToString() == "1" ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitDetail(string id)
        {
            try
            {
                string sql = @"select guid, orderid, parametername, parametermemo, 
                                      parametertype, decode(parametertype, 'VARCHAR', '字符', 'DATETIME', '日期', 'NUMBER', '小数', '字符') parametertypename, 
                                      parameterdirection, decode(parameterdirection, 'IN', '入参', 'OUT', '出参') parameterdirectionname, parameterfield 
                                 from consultfeeinterfacepara 
                                where valid = '1' and mainid = '" + id + "' order by orderid";
                DataTable dt = DS_SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                gridControlParameter.DataSource = null;
                gridControlParameter.DataSource = dt;
                ClearTextBoxContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmFeeInterfaceConfig_Load(object sender, EventArgs e)
        {
            try
            {
                InitGridControl();
                InitParameterType();
                InitParameterDirection();
                InitParameterMainList();
                ChangeBtnEnable(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region TextBox控制
        private void SetTextBoxEnable(bool isReadOnly)
        {
            try
            {
                textEditParameterName.Properties.ReadOnly = isReadOnly;
                textEditParameterMemo.Properties.ReadOnly = isReadOnly;
                lookUpEditParameterType.Properties.ReadOnly = isReadOnly;
                lookUpEditParameterDirection.Properties.ReadOnly = isReadOnly;
                memoEditField.Properties.ReadOnly = isReadOnly;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 清空TextBox中的内容
        /// </summary>
        private void ClearTextBoxContent()
        {
            try
            {
                textEditParameterName.Text = "";
                textEditParameterMemo.Text = "";
                lookUpEditParameterType.EditValue = "";
                lookUpEditParameterDirection.EditValue = "";
                memoEditField.Text = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 参数列表的按钮事件

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ClearTextBoxContent();
                SetTextBoxEnable(false);
                ChangeBtnEnable(false);
                m_CurrentOperationType = OperationType.Insert;
                textEditParameterName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetTextBoxEnable(true);
                ChangeBtnEnable(true);
                m_CurrentOperationType = OperationType.None;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = gridControlParameter.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("参数列表中没有数据，不能进行编辑操作！");
                    return;
                }
                else
                {
                    SetTextBoxEnable(false);
                    ChangeBtnEnable(false);
                    m_CurrentOperationType = OperationType.Edit;
                    textEditParameterName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = gridControlParameter.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("参数列表中没有数据，不能进行删除操作！");
                    return;
                }
                else
                {
                    if (MessageBox.Show("确定删除选中行？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        DataRowView drv = gridViewPara.GetRow(gridViewPara.FocusedRowHandle) as DataRowView;
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dt.Rows[i];
                            if (dr["PARAMETERNAME"].ToString() == drv["PARAMETERNAME"].ToString())
                            {
                                dt.Rows[i].Delete();
                                break;
                            }
                        }
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("已删除！");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChangeBtnEnable(bool isEnable)
        {
            try
            {
                btnAdd.Enabled = isEnable;
                btnEdit.Enabled = isEnable;
                btnDelete.Enabled = isEnable;
                btnSave.Enabled = !isEnable;
                btnCancel.Enabled = !isEnable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存参数设置前的验证
        /// </summary>
        private bool CheckBeforeSavePara()
        {
            try
            {
                if (textEditParameterName.Text.Trim() == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入参数名称！");
                    textEditParameterName.Focus();
                    return false;
                }
                else if (textEditParameterMemo.Text.Trim() == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入参数说明！");
                    textEditParameterMemo.Focus();
                    return false;
                }
                else if (lookUpEditParameterType.EditValue.ToString() == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择参数类型！");
                    lookUpEditParameterType.Focus();
                    return false;
                }
                else if (lookUpEditParameterDirection.EditValue.ToString() == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择参数方向！");
                    lookUpEditParameterDirection.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckBeforeSaveAll()
        {
            try
            {
                if (textEditProcedureName.Text.Trim() == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入存储过程名称！");
                    lookUpEditID.Focus();
                    return false;
                }
                else if (memoEditSQL.Text.Trim() == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("字段对应数据源SQL！");
                    memoEditSQL.Focus();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckBeforeSavePara())
                {
                    if (m_CurrentOperationType == OperationType.Edit)
                    {
                        DataRowView focusRow = gridViewPara.GetRow(gridViewPara.FocusedRowHandle) as DataRowView;
                        if (focusRow != null)
                        {
                            focusRow["PARAMETERNAME"] = textEditParameterName.Text.Trim();
                            focusRow["PARAMETERMEMO"] = textEditParameterMemo.Text.Trim();
                            focusRow["PARAMETERTYPE"] = lookUpEditParameterType.EditValue.ToString().Trim();
                            focusRow["PARAMETERTYPENAME"] = lookUpEditParameterType.Text.ToString().Trim();
                            focusRow["PARAMETERDIRECTION"] = lookUpEditParameterDirection.EditValue.ToString().Trim();
                            focusRow["PARAMETERDIRECTIONNAME"] = lookUpEditParameterDirection.Text.ToString().Trim();
                            focusRow["PARAMETERFIELD"] = memoEditField.Text.Trim();
                            gridViewPara.RefreshData();//修改后刷新GridView
                        }
                    }
                    else if (m_CurrentOperationType == OperationType.Insert)
                    {
                        DataTable dt = gridControlParameter.DataSource as DataTable;
                        DataRow dr = dt.NewRow();
                        dr["GUID"] = System.Guid.NewGuid().ToString();
                        dr["ORDERID"] = dt.Rows.Count + 1;
                        dr["PARAMETERNAME"] = textEditParameterName.Text.Trim();
                        dr["PARAMETERMEMO"] = textEditParameterMemo.Text.Trim();
                        dr["PARAMETERTYPE"] = lookUpEditParameterType.EditValue.ToString().Trim();
                        dr["PARAMETERTYPENAME"] = lookUpEditParameterType.Text.ToString().Trim();
                        dr["PARAMETERDIRECTION"] = lookUpEditParameterDirection.EditValue.ToString().Trim();
                        dr["PARAMETERDIRECTIONNAME"] = lookUpEditParameterDirection.Text.ToString().Trim();
                        dr["PARAMETERFIELD"] = memoEditField.Text.Trim();
                        dr["MAINID"] = lookUpEditID.EditValue.ToString();
                        dt.Rows.Add(dr);
                        m_CurrentOperationType = OperationType.Edit;//插入完成后状态改为编辑状态，方便修改刚刚插入的内容
                        gridViewPara.FocusedRowHandle = dt.Rows.Count - 1;//选中刚插入的数据
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 参数列表控制
        private void gridViewPara_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gridViewPara.FocusedRowHandle >= 0)
                {
                    DataRowView drv = gridViewPara.GetRow(gridViewPara.FocusedRowHandle) as DataRowView;
                    if (drv != null)
                    {
                        textEditParameterName.Text = drv["PARAMETERNAME"].ToString();
                        textEditParameterMemo.Text = drv["PARAMETERMEMO"].ToString();
                        lookUpEditParameterType.EditValue = drv["PARAMETERTYPE"].ToString();
                        lookUpEditParameterDirection.EditValue = drv["PARAMETERDIRECTION"].ToString();
                        memoEditField.Text = drv["PARAMETERFIELD"].ToString();
                        ChangeBtnEnable(true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 上下移动参数
        private void simpleButtonUP_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewPara.FocusedRowHandle > 0)
                {
                    DataRowView lastRow = gridViewPara.GetRow(gridViewPara.FocusedRowHandle - 1) as DataRowView;//上一行
                    DataRowView currentRow = gridViewPara.GetRow(gridViewPara.FocusedRowHandle) as DataRowView;//当前行
                    if (currentRow != null && lastRow != null)
                    {
                        DataTable dt = gridControlParameter.DataSource as DataTable;
                        if (dt != null)
                        {
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.ColumnName.ToUpper() != "ORDERID")
                                {
                                    string value = lastRow[dc.ColumnName].ToString();
                                    lastRow[dc.ColumnName] = currentRow[dc.ColumnName];
                                    currentRow[dc.ColumnName] = value;
                                }
                            }
                            gridViewPara.FocusedRowHandle--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void simpleButtonDown_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = gridControlParameter.DataSource as DataTable;
                if (dt != null)
                {
                    if (gridViewPara.FocusedRowHandle <= dt.Rows.Count - 1)
                    {
                        DataRowView currentRow = gridViewPara.GetRow(gridViewPara.FocusedRowHandle) as DataRowView;//当前行
                        DataRowView nextRow = gridViewPara.GetRow(gridViewPara.FocusedRowHandle + 1) as DataRowView;//下一行
                        if (currentRow != null && nextRow != null)
                        {
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.ColumnName.ToUpper() != "ORDERID")//除了OrderID
                                {
                                    string value = nextRow[dc.ColumnName].ToString();
                                    nextRow[dc.ColumnName] = currentRow[dc.ColumnName];
                                    currentRow[dc.ColumnName] = value;
                                }
                            }
                            gridViewPara.FocusedRowHandle++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 切换存储过程名称
        private void lookUpEditID_EditValueChanged(object sender, EventArgs e)
        {
            DataTable dt = lookUpEditID.Properties.DataSource as DataTable;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    if (dataRow["ID"].ToString() == lookUpEditID.EditValue.ToString())
                    {
                        InitMain(dataRow);
                        InitDetail(dataRow["ID"].ToString());
                        return;
                    }
                }
            }
        }
        #endregion

        #region 保存
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckBeforeSaveAll())
                {
                    //作废旧数据，并插入新数据
                    string id = lookUpEditID.EditValue.ToString();
                    string procedureName = textEditProcedureName.Text.Trim();
                    string procedureMemo = textEditMemo.Text.Trim();
                    string prodedureField = memoEditSQL.Text.Trim();
                    string isOpen = checkBoxValid.Checked ? "1" : "0";

                    #region 修改

                    #region 作废主表数据
                    DS_SqlHelper.ExecuteNonQuery(string.Format(
                        @"update consultfeeinterfacemain set valid = '0', modifyuser = '{0}', modifytime = sysdate where valid = '1' and id ='" + id + "' ",
                        m_App.User.Id), CommandType.Text);
                    #endregion

                    #region 新增主表数据
                    SqlParameter[] sp = new SqlParameter[6] { 
                        new SqlParameter("@name", SqlDbType.VarChar),
                        new SqlParameter("@memo", SqlDbType.VarChar),
                        new SqlParameter("@field", SqlDbType.VarChar),
                        new SqlParameter("@userid", SqlDbType.VarChar),
                        new SqlParameter("@id", SqlDbType.VarChar),
                        new SqlParameter("@isopen", SqlDbType.VarChar)
                    };
                    sp[0].Value = procedureName;
                    sp[1].Value = procedureMemo;
                    sp[2].Value = prodedureField;
                    sp[3].Value = m_App.User.Id;
                    sp[4].Value = id;
                    sp[5].Value = isOpen;
                    DS_SqlHelper.ExecuteNonQuery(@"insert into consultfeeinterfacemain(PROCEDURENAME, MEMO, DATASOURCESQL, VALID, MODIFYUSER, MODIFYTIME, ID, ISOPEN)
                                        values(@name, @memo, @field, '1', @userid, sysdate, @id, @isopen)", sp, CommandType.Text);
                    #endregion

                    #region 作废从表数据
                    //作废旧数据，并插入新参数
                    DS_SqlHelper.ExecuteNonQuery(string.Format(
                        @"update consultfeeinterfacepara set valid = '0', modifyuser = '{0}', modifytime = sysdate where valid = '1' and mainid = '" + id + "' ",
                        m_App.User.Id), CommandType.Text);
                    #endregion

                    #region 新增明细表数据
                    DataTable dt = gridControlParameter.DataSource as DataTable;
                    foreach (DataRow dr in dt.Rows)
                    {
                        sp = new SqlParameter[7] { 
                            new SqlParameter("@parametername", SqlDbType.VarChar),
                            new SqlParameter("@parametermemo", SqlDbType.VarChar),
                            new SqlParameter("@parametertype", SqlDbType.VarChar),
                            new SqlParameter("@parameterdirection", SqlDbType.VarChar),
                            new SqlParameter("@parameterfield", SqlDbType.VarChar),
                            new SqlParameter("@guid", SqlDbType.VarChar),
                            new SqlParameter("@orderid", SqlDbType.Int),
                        };
                        sp[0].Value = dr["PARAMETERNAME"].ToString();
                        sp[1].Value = dr["PARAMETERMEMO"].ToString();
                        sp[2].Value = dr["PARAMETERTYPE"].ToString();
                        sp[3].Value = dr["PARAMETERDIRECTION"].ToString();
                        sp[4].Value = dr["PARAMETERFIELD"].ToString();
                        sp[5].Value = dr["GUID"].ToString();
                        sp[6].Value = dr["ORDERID"].ToString();

                        DS_SqlHelper.ExecuteNonQuery(string.Format(
                            @"insert into consultfeeinterfacepara(PARAMETERNAME, PARAMETERMEMO, PARAMETERTYPE, PARAMETERDIRECTION, PARAMETERFIELD, VALID, MODIFYUSER, MODIFYTIME, GUID, ORDERID, MAINID)
                            values(@parametername, @parametermemo, @parametertype, @parameterdirection, @parameterfield, '1', '{0}', sysdate, @guid, @orderid, '" + id + "')", m_App.User.Id),
                                sp, CommandType.Text);
                    }

                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功！");
                    #endregion

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 退出
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    enum OperationType
    {
        None,
        Insert,
        Edit
    }
}