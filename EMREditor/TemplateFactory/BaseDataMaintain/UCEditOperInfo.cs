using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork;
using DrectSoft.Wordbook;
using DrectSoft.Common;
using DrectSoft.Service;
using DrectSoft.Core;
using DrectSoft.SysTableEdit;

namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    /// <summary>
    /// 手术信息维护界面
    /// </summary>
    public partial class UCEditOperInfo : DevExpress.XtraEditors.XtraUserControl, IStartPlugIn
    {
        #region 属性及其他字段
        public UCEditOperInfo()
        {
            InitializeComponent();
        }
        private IEmrHost m_app;
        SQLManger m_SqlManager;
        /// <summary>
        /// 当前页面状态
        /// </summary>
        EditState m_EditState = EditState.None;
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCEditOperInfo(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_SqlManager = new SQLManger(m_app);
        }
        DataTable m_OperTable;

        #endregion

        #region 方法相关
        /// <summary>
        /// 刷新页面数据
        /// </summary>
        private void RefreshData()
        {
            BindGrid();
            m_EditState = EditState.View;
            BtnState();
            ClearPageValue();
        }
        /// <summary>
        /// 绑定Grid列表的值 
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-22
        /// 1、add try ... catch
        private void BindGrid()
        {
            try
            {
                DataTable dt = m_SqlManager.GetOperInfo_Table("");
                this.gridControl1.DataSource = dt;
                if (!string.IsNullOrEmpty(this.txt_seach.Text))
                {
                    FilterDataSource();
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 初始化手术类别下拉框
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        private void InitSslb()
        {
            try
            {
                lookUpWindowsslb.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select * from categorydetail a where a.categoryid = 8 ");
                DataTable Sslb = m_app.SqlHelper.ExecuteDataTable(sql);

                Sslb.Columns["ID"].Caption = "类别代码";
                Sslb.Columns["NAME"].Caption = "类别名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 90);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Sslb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorsslb.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 清空页面控件的值
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        private void ClearPageValue()
        {
            try
            {
                this.txtMapID.Text = "";
                this.txtID.Text = "";
                this.txtName.Text = "";
                this.txtPy.Text = "";
                this.txtWb.Text = "";
                cmbValid.SelectedIndex = -1;
                this.txtMemo.Text = "";
                this.lookUpEditorsslb.CodeValue = "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重置方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
        /// </summary>
        private void Reset()
        {
            try
            {
                this.txtMapID.Text = "";
                this.txtName.Text = "";
                this.txtPy.Text = "";
                this.txtWb.Text = "";
                cmbValid.SelectedIndex = -1;
                this.txtMemo.Text = "";
                this.lookUpEditorsslb.CodeValue = "";
                if (m_EditState == EditState.Add)
                {
                    this.txtID.Text = "";
                    this.txtID.Focus();
                }
                else
                {
                    this.txtMapID.Focus();
                }
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
            if (m_EditState == EditState.View || m_EditState == EditState.None)
            {
                this.btnADD.Enabled = true;
                this.btnDel.Enabled = true;
                this.BtnEdit.Enabled = true;

                this.btnSave.Enabled = false;
                this.btn_reset.Enabled = false;
                this.BtnClear.Enabled = false;

                this.txtMapID.Enabled = false;
                this.txtID.Enabled = false;
                this.txtName.Enabled = false;

                //cmbValid.Enabled = false;//edit by cyq 2013-02-22
                this.txtMemo.Enabled = false;
                this.lookUpEditorsslb.Enabled = false;
            }
            else if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
            {
                this.btnADD.Enabled = false;
                this.btnDel.Enabled = false;
                this.BtnEdit.Enabled = false;
                this.btnSave.Enabled = true;
                this.btn_reset.Enabled = true;
                this.BtnClear.Enabled = true;

                if (m_EditState == EditState.Add)
                    this.txtID.Enabled = true;
                else
                    this.txtID.Enabled = false;
                this.txtMapID.Enabled = true;

                this.txtName.Enabled = true;
                //cmbValid.Enabled = true;//edit by cyq 2013-02-22
                this.txtMemo.Enabled = true;
                this.lookUpEditorsslb.Enabled = true;
            }
            this.txtPy.Enabled = false;
            this.txtWb.Enabled = false;
        }
        /// <summary>
        /// 将gridview中对应行值加载到实体中
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private OperInfoEntity SetEntityByDataRow(DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            OperInfoEntity operInfoEntity = new OperInfoEntity();
            operInfoEntity.Id = dr["Id"].ToString();
            operInfoEntity.Mapid = dr["Mapid"].ToString();
            operInfoEntity.Name = dr["Name"].ToString();
            operInfoEntity.Py = dr["Py"].ToString();
            operInfoEntity.Wb = dr["Wb"].ToString();
            operInfoEntity.Valid = Convert.ToInt32(dr["Valid"].ToString() == "" ? "0" : dr["Valid"].ToString());
            operInfoEntity.Memo = dr["Memo"].ToString();
            operInfoEntity.Sslb = dr["Sslb"].ToString();
            return operInfoEntity;
        }
        /// <summary>
        /// 将手术信息实体中值加载到页面
        /// </summary>
        /// <param name="operInfoEntity"></param>
        private void SetPageValue(OperInfoEntity operInfoEntity)
        {
            if (operInfoEntity == null || operInfoEntity.Id == "")
                return;

            this.txtID.Text = operInfoEntity.Id.Trim();
            this.txtMapID.Text = operInfoEntity.Mapid.Trim();
            this.txtName.Text = operInfoEntity.Name.Trim();
            this.txtPy.Text = operInfoEntity.Py.Trim();
            this.txtWb.Text = operInfoEntity.Wb.Trim();
            this.cmbValid.SelectedIndex = operInfoEntity.Valid;
            this.txtMemo.Text = operInfoEntity.Memo.Trim();
            this.lookUpEditorsslb.CodeValue = operInfoEntity.Sslb.ToString().Trim();
        }
        
        /// <summary>
        /// 验证页面数据是否可以保存
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool IsSave()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtID.Text.Trim()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("手术代码不能为空");
                    this.txtID.Focus();
                    return false;
                }
                else if (m_EditState == EditState.Add && !m_SqlManager.CheckOperID(this.txtID.Text.Trim()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该手术代码已存在，请重新输入。");
                    this.txtID.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("手术名称不能为空");
                    this.txtName.Focus();
                    return false;
                }
                //else if (string.IsNullOrEmpty(this.lookUpEditorsslb.CodeValue.Trim()))
                //{
                //    Common.Ctrs.DLG.MessageBox.Show("请选择手术类别");
                //    this.lookUpEditorsslb.Focus();
                //    return false;
                //}
                //else if (null == this.cmbValid.EditValue || this.cmbValid.EditValue.ToString() == "")
                //{
                //    Common.Ctrs.DLG.MessageBox.Show("请选择是否有效");
                //    cmbValid.Focus();
                //    return false;
                //}

                return true;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                return false;
            }
        }
        
        /// <summary>
        /// 绑定下拉框的数据源
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitOperData()
        {
            try
            {
                lookUpWindoOpe.SqlHelper = m_app.SqlHelper;

                if (m_OperTable == null)
                    m_OperTable = m_SqlManager.GetOperInfo_Table("");

                m_OperTable.Columns["ID"].Caption = "ID";
                m_OperTable.Columns["NAME"].Caption = "手术名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 80);
                cols.Add("NAME", 170);

                SqlWordbook OperWordBook = new SqlWordbook("querybook", m_OperTable, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorDiag.SqlWordbook = OperWordBook;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 将页面控件的值加入到实体
        /// </summary>
        /// <returns></returns>
        private OperInfoEntity SetEntityByPageValue()
        {
            OperInfoEntity operInfoEntity = new OperInfoEntity();
            operInfoEntity.Id = this.txtID.Text.Trim();
            operInfoEntity.Mapid = this.txtMapID.Text.Trim();
            operInfoEntity.Name = this.txtName.Text.Trim();
            operInfoEntity.Py = this.txtPy.Text.Trim();
            operInfoEntity.Wb = this.txtWb.Text.Trim();

            operInfoEntity.Valid = 1; //this.cmbValid.SelectedIndex;
            operInfoEntity.Memo = this.txtMemo.Text.Trim();
            operInfoEntity.Sslb = lookUpEditorsslb.CodeValue.Trim();

            return operInfoEntity;
        }
        /// <summary>
        /// 保存手术信息
        /// </summary>
        /// <param name="operInfoEntity"></param>
        /// <returns></returns>
        private bool SaveOperInfo(OperInfoEntity operInfoEntity)
        {
            try
            {
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";
                m_SqlManager.SaveOperInfo(operInfoEntity, edittype);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 得到拼音和五笔
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private void SetPYWB(string name)
        {
            try
            {
                GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
                string[] code = shortCode.GenerateStringShortCode(name);

                this.txtPy.Text = code[0].ToString();
                this.txtWb.Text = code[1].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 事件相关
        public IPlugIn Run(IEmrHost host)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCEditOperInfo_Load(object sender, EventArgs e)
        {
            try
            {
                InitSslb();
                //edit by cyq 2013-02-22 弃用下拉框检索
                //InitOperData();
                RefreshData();
                this.lookUpEditorDiag.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// 3、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnADD_Click(object sender, EventArgs e)
        {
            try
            {
                m_EditState = EditState.Add;
                ClearPageValue();
                BtnState();
                this.txtID.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 删除事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (null == dr || dr.ItemArray.Length <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除手术记录 " + dr["NAME"] + " 吗？该操作不可恢复。", "删除手术记录", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                if (m_SqlManager.DelOperInfo(dr["ID"].ToString()))
                {
                    m_app.CustomMessageBox.MessageShow("删除成功");
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 编辑事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// 3、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (foucesRow == null || foucesRow.IsNull("ID"))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }

                SetPageValue(SetEntityByDataRow(foucesRow));

                m_EditState = EditState.Edit;
                BtnState();
                this.txtMapID.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsSave())
                {
                    return;
                }
                if (SaveOperInfo(SetEntityByPageValue()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功");
                    //if (m_EditState == EditState.Add)
                    //{
                    //    m_app.CustomMessageBox.MessageShow("新增手术信息成功");
                    //}
                    //else
                    //{
                    //    m_app.CustomMessageBox.MessageShow("修改手术信息成功");
                    //}
                    RefreshData();
                    ClearPageValue();
                }
                else
                {
                    //if (m_EditState == EditState.Add)
                    //{
                    //    m_app.CustomMessageBox.MessageShow("新增手术信息失败");
                    //}
                    //else
                    //{
                    //    m_app.CustomMessageBox.MessageShow("修改手术信息失败");
                    //}
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存失败");
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重置事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
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
        /// 清空事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// 3、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPageValue();
                m_EditState = EditState.View;
                BtnState();
                this.txtID.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 点击列表带出详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
                return;
            DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("ID"))
                return;

            SetPageValue(SetEntityByDataRow(foucesRow));
            m_EditState = EditState.View;

            BtnState();
        }
        /// <summary>
        /// 检索事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorDiag_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lookUpEditorDiag.CodeValue))
                {
                    DataTable dt = m_OperTable.Clone();
                    foreach (DataRow dr in m_OperTable.Select(string.Format("ID='{0}'", lookUpEditorDiag.CodeValue)))
                    {
                        dt.Rows.Add(dr.ItemArray);
                    }
                    gridControl1.DataSource = dt;

                    gridControl1_Click(null, null);
                }
                else
                {
                    gridControl1.DataSource = m_OperTable;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 检索事件
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-22</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_seach_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                FilterDataSource();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 拼音五笔自动生成
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-22</date>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SetPYWB(this.txtName.Text);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 检索方法
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-22</date>
        private void FilterDataSource()
        {
            try
            {
                DataTable dt = (DataTable)gridControl1.DataSource;
                if (null == dt || dt.Rows.Count == 0)
                {
                    return;
                }
                string searchStr = DS_Common.FilterSpecialCharacter(this.txt_seach.Text);
                string filterStr = string.Format(" id like '%{0}%' or mapid like '%{0}%' or name like '%{0}%' or py like '%{0}%' or wb like '%{0}%' or sslbname like '%{0}%' or validName like '%{0}%' or memo like '%{0}%' ", searchStr);
                dt.DefaultView.RowFilter = filterStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
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

        private void btnSetBieMing_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = gridView1.GetFocusedDataRow();
                if (dr == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("未选中记录");
                    return;
                }
                DiagnosisOtherNaem diagnosisOtherNaem = new DiagnosisOtherNaem(dr["ID"].ToString(), "3");
                diagnosisOtherNaem.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }
}
