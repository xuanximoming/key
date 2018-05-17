using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.SysTableEdit.DataEntity;
using DrectSoft.Wordbook;
using DrectSoft.Core;
using DrectSoft.Common.Ctrs.FORM;
using System.Data.SqlClient;
using DrectSoft.Common;

///<title>并发症维护</title>
///<auth>Yanqiao.Cai</auth>
///<date>2011-10-15</date>
namespace DrectSoft.SysTableEdit
{
    public partial class UCEditComplications : DevExpress.XtraEditors.XtraUserControl, IStartPlugIn
    {
        private IEmrHost m_app;

        /// <summary>
        /// 当前页面状态
        /// </summary>
        EditState m_EditState = EditState.None;

        public UCEditComplications(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        private void UCEditComplications_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshData();
                BtnState();
                this.txt_search.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #region 方法

        /// <summary>
        /// 刷新页面数据
        /// </summary>
        private void RefreshData()
        {
            try
            {
                BindGrid();
                m_EditState = EditState.View;
                BtnState();
                Reset();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 绑定grid值
        /// </summary>
        private void BindGrid()
        {
            try
            {
                string sqlStr = @" select ID,icd_code as Code,Name,PY,WB,SortIndex,Memo,case when Valid=1 then '是' else '否' end as Valid from Complications where 1=1 ";
                string searchStr = this.txt_search.Text.Trim();
                if (!string.IsNullOrEmpty(searchStr))
                {
                    sqlStr += @" and icd_code like '%" + searchStr + "%' or Name like '%" + searchStr + "%' or PY like '%" + searchStr + "%' or wb like '%" + searchStr + "%' or Memo like '%" + searchStr + "%' ";
                    if (Tools.IsInt(searchStr))
                    {
                        sqlStr += @" or sortindex = " + searchStr + " ";
                    }
                    if (searchStr == "是" || searchStr == "否")
                    {
                        sqlStr += @" or valid = " + (searchStr == "是" ? 1 : 0) + " ";
                    }
                }
                sqlStr += " order by SortIndex ";
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sqlStr, CommandType.Text);
                if (null == dt || dt.Rows.Count == 0)
                {
                    dt = new DataTable();
                }
                this.gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将gridview中对应行值加载到实体中
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private void SetPageValueByDataRow(DataRow dr)
        {
            try
            {
                if (null == dr)
                {
                    return;
                }

                this.txt_ICDCode.Text = null == dr["Code"] ? "" : dr["Code"].ToString();
                this.txt_name.Text = null == dr["Name"] ? "" : dr["Name"].ToString();
                this.txt_sortIndex.Text = null == dr["SortIndex"] ? "" : dr["SortIndex"].ToString();
                this.txt_py.Text = null == dr["PY"] ? "" : dr["PY"].ToString();
                this.txt_wb.Text = null == dr["WB"] ? "" : dr["WB"].ToString();
                this.cmb_valid.EditValue = null == dr["VALID"] ? "" : dr["VALID"].ToString();
                this.txt_memo.Text = null == dr["Memo"] ? "" : dr["Memo"].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 清空/重置 页面控件值
        /// </summary>
        private void Reset()
        {
            try
            {
                if (m_EditState == EditState.Add)
                {
                    this.txt_ICDCode.Text = "";
                }
                this.txt_name.Text = "";
                this.txt_sortIndex.Text = "";
                this.txt_py.Text = "";
                this.txt_wb.Text = "";
                this.cmb_valid.EditValue = "";
                this.txt_memo.Text = "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 控制页面控件状态
        /// </summary>
        private void BtnState()
        {
            try
            {
                if (m_EditState == EditState.View)
                {
                    //按钮
                    this.btn_add.Enabled = true;
                    this.btn_edit.Enabled = true;
                    this.btn_delete.Enabled = true;
                    this.btn_save.Enabled = false;
                    this.btn_reset.Enabled = false;
                    this.btn_cancel.Enabled = false;

                    //输入框
                    this.txt_ICDCode.Enabled = false;
                    this.txt_name.Enabled = false;
                    this.txt_sortIndex.Enabled = false;
                    this.cmb_valid.Enabled = false;
                    this.txt_memo.Enabled = false;
                }
                else if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
                {
                    this.btn_add.Enabled = false;
                    this.btn_edit.Enabled = false;
                    this.btn_delete.Enabled = false;
                    this.btn_save.Enabled = true;
                    this.btn_reset.Enabled = true;
                    this.btn_cancel.Enabled = true;

                    if (m_EditState == EditState.Add)
                    {
                        this.txt_ICDCode.Enabled = true;
                    }
                    else
                    {
                        this.txt_ICDCode.Enabled = false;
                    }
                    this.txt_name.Enabled = true;
                    this.txt_sortIndex.Enabled = true;
                    this.cmb_valid.Enabled = true;
                    this.txt_memo.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 验证页面数据是否可以保存
        /// </summary>
        /// <returns></returns>
        private string CheckItem()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txt_ICDCode.Text.Trim()))
                {
                    this.txt_ICDCode.Focus();
                    return "ICD编码不能为空";
                }
                else if (!Tools.IsDigitalOrLetter(this.txt_ICDCode.Text.Trim()))
                {
                    this.txt_ICDCode.Text = "";
                    this.txt_ICDCode.Focus();
                    return "ICD编码只能由英文字母和数字组成";
                }
                else if (string.IsNullOrEmpty(this.txt_name.Text.Trim()))
                {
                    this.txt_name.Focus();
                    return "并发症名称不能为空";
                }
                else if (!string.IsNullOrEmpty(txt_sortIndex.Text.Trim()) && !Tools.IsNumeric(txt_sortIndex.Text.Trim()))
                {
                    this.txt_sortIndex.Text = "";
                    this.txt_sortIndex.Focus();
                    return "排序号只能是数字";
                }
                else if (null == this.cmb_valid.EditValue || this.cmb_valid.EditValue.ToString() == "")
                {
                    this.cmb_valid.Focus();
                    return "请选择是否有效";
                }

                return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到拼音和五笔
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private void GetPYWB(string name)
        {
            try
            {
                GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
                string[] code = shortCode.GenerateStringShortCode(name);

                this.txt_py.Text = code[0].ToString();
                this.txt_wb.Text = code[1].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取排序号
        /// </summary>
        /// <returns></returns>
        private int GetSortIndex()
        {
            try
            {
                int sortIndex = 1;
                if (string.IsNullOrEmpty(txt_sortIndex.Text.Trim()))
                {
                    string sqlIndex = " select max(sortindex) from Complications ";
                    DataTable indexDT = m_app.SqlHelper.ExecuteDataTable(sqlIndex, CommandType.Text);
                    if (null != indexDT && indexDT.Rows.Count > 0 && null != indexDT.Rows[0][0] && indexDT.Rows[0][0].ToString() != "")
                    {
                        sortIndex = int.Parse(indexDT.Rows[0][0].ToString()) + 1;
                    }
                }
                else
                {
                    sortIndex = int.Parse(txt_sortIndex.Text.Trim());
                }
                return sortIndex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 数据库中是否已存在该并发症ICD编码
        /// </summary>
        /// <param name="icdCode"></param>
        /// <returns></returns>
        private bool CheckCodeExist(string icdCode)
        {
            try
            {
                bool boo = false;
                if (!string.IsNullOrEmpty(icdCode))
                {
                    string sqlStr = "select icd_code from Complications where icd_code = @code ";
                    SqlParameter code = new SqlParameter("@code",SqlDbType.VarChar,20);
                    code.Value = icdCode;
                    DataTable dt = m_app.SqlHelper.ExecuteDataTable(sqlStr, new SqlParameter[] { code });
                    boo = null != dt && dt.Rows.Count > 0;
                }
                return boo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public IPlugIn Run(FrameWork.WinForm.Plugin.IEmrHost host)
        {
            throw new NotImplementedException();
        }
       
        #region 事件
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                m_EditState = EditState.Add;
                Reset();
                BtnState();
                this.txt_ICDCode.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (foucesRow == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                SetPageValueByDataRow(foucesRow);

                m_EditState = EditState.Edit;
                BtnState();
                this.txt_name.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (foucesRow == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除并发症 " + foucesRow["NAME"] + " 吗？", "删除并发症", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                string deleteSQL = @" delete from Complications where id = '" + foucesRow["ID"] + "'";
                m_app.SqlHelper.ExecuteNoneQuery(deleteSQL, CommandType.Text);
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");
                RefreshData();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                string error = CheckItem();
                if (!string.IsNullOrEmpty(error))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(error);
                    return;
                }
                if (m_EditState == EditState.Add && CheckCodeExist(txt_ICDCode.Text.Trim()))
                {
                    txt_ICDCode.Text = "";
                    txt_ICDCode.Focus();
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该ICD编码已存在，请重新输入。");
                    return;
                }

                this.txt_search.EditValueChanged -= new EventHandler(txt_search_EditValueChanged);
                this.txt_search.Text = "";
                this.txt_search.EditValueChanged += new EventHandler(txt_search_EditValueChanged);

                SqlParameter code = new SqlParameter("@code", SqlDbType.VarChar, 20);
                code.Value = txt_ICDCode.Text.Trim();
                SqlParameter name = new SqlParameter("@name", SqlDbType.VarChar, 50);
                name.Value = txt_name.Text.Trim();
                SqlParameter py = new SqlParameter("@py", SqlDbType.VarChar, 8);
                py.Value = txt_py.Text.Trim();
                SqlParameter wb = new SqlParameter("@wb", SqlDbType.VarChar, 8);
                wb.Value = txt_wb.Text.Trim();
                //序号不填写则默认排序至最后
                SqlParameter sortIndex = new SqlParameter("@sortIndex", SqlDbType.Int, 2);
                sortIndex.Value = GetSortIndex();
                SqlParameter memo = new SqlParameter("@memo", SqlDbType.VarChar, 300);
                memo.Value = txt_wb.Text.Trim();
                SqlParameter valid = new SqlParameter("@valid", SqlDbType.Int, 2);
                valid.Value = (null == cmb_valid.EditValue || cmb_valid.EditValue.ToString() == "" || cmb_valid.EditValue.ToString() == "是") ? 1 : 0;

                SqlParameter[] sqlParams = new SqlParameter[] {
                    code,name,py,wb,sortIndex,memo,valid
                };

                if (m_EditState == EditState.Add)
                {
                    string createSQL = "insert into Complications(ID,icd_code,Name,PY,WB,SortIndex,Memo,Valid) values ('" + Guid.NewGuid().ToString() + "',@code,@name,@py,@wb,@sortIndex,@memo,@valid)";
                    m_app.SqlHelper.ExecuteNoneQuery(createSQL, sqlParams);
                    m_app.CustomMessageBox.MessageShow("新增成功");
                }
                else
                {
                    DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    string updateSQL = " update Complications set Name=@name,PY=@py,wb=@wb,sortindex=@sortIndex,Memo=@memo,valid=@valid where id='" + foucesRow["ID"] + "'";
                    m_app.SqlHelper.ExecuteNoneQuery(updateSQL, sqlParams);
                    m_app.CustomMessageBox.MessageShow("修改成功");
                }
                RefreshData();
                this.btn_add.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_reset_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
                this.txt_ICDCode.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            try
            {
                Reset();
                m_EditState = EditState.View;
                BtnState();
                this.btn_add.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 拼音五笔自动生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_name_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GetPYWB(this.txt_name.Text);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 数据集点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (foucesRow == null)
                {
                    return;
                }
                SetPageValueByDataRow(foucesRow);
                m_EditState = EditState.View;
                BtnState();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_search_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)gridControl1.DataSource;
                if (dt != null)
                {
                    string searchStr = DS_Common.FilterSpecialCharacter(this.txt_search.Text);
                    string filterStr = @" Code like '%" + searchStr + "%' or Name like '%" + searchStr + "%' or PY like '%" + searchStr + "%' or WB like '%" + searchStr + "%'"
                                + " or Valid like '%" + searchStr + "%' or Memo like '%" + searchStr + "%' ";
                    if (!string.IsNullOrEmpty(searchStr) && Tools.IsInt(searchStr))
                    {
                        filterStr += @" or SortIndex = " + int.Parse(searchStr) + " ";
                    }
                    dt.DefaultView.RowFilter = filterStr;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        #endregion

    }
}
