using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    /// <summary>
    /// 复用项目维护界面
    /// </summary>
    public partial class UCEditModelFactory : DevExpress.XtraEditors.XtraUserControl, IStartPlugIn
    {

        private IEmrHost m_app;

        //SysTableManger m_SysTableManger;
        SqlHelp m_SysTableManger;



        /// <summary>
        /// 当前页面状态
        /// </summary>
        EditState m_EditState = EditState.None;

        public UCEditModelFactory(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            //m_SysTableManger = new SysTableManger(m_app);
            m_SysTableManger = new SqlHelp(m_app);
        }

        #region 事件
        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCEditModelFactory_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshData();
                this.btnADD.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 点击列表信息，将数据加载到下面控件里
        /// MOdify by xlb 2013-04-03 为了和其他界面统一按钮状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_Click(object sender, EventArgs e)
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
            if (foucesRow.IsNull("id"))
            {
                return;
            }

            SetPageValue(SetEntityByDataRow(foucesRow));
            m_EditState = EditState.View;

            BtnState();
        }

        /// <summary>
        /// 新增事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、添加焦点
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
                //设置焦点
                this.txtId.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// edit by Yanqiao.Cai 2012-11-07
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
                    MessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (null == dr || dr.ItemArray.Length <= 0)
                {
                    MessageBox.Show("请选择一条记录");
                    return;
                }
                if (MyMessageBox.Show("您确定要删除该复用项目吗？", "删除复用项目", MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                // edit by Yanqiao.Cai 2012-11-07
                //string modelid = this.txtId.Text.Trim();
                //if (modelid == "")
                //{
                //    m_app.CustomMessageBox.MessageShow("请选择要删除的记录");
                //    return;
                //}
                if (m_SysTableManger.DelEmrItem(dr["ID"].ToString()))
                {
                    m_app.CustomMessageBox.MessageShow("删除成功");
                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、添加焦点
        /// 3、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    MessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (null == dr || dr.IsNull("id") || dr.ItemArray.Length <= 0)
                {
                    MessageBox.Show("请选择一条记录");
                    return;
                }

                SetPageValue(SetEntityByDataRow(dr));
                SetPageValueByDataRow(dr);
                m_EditState = EditState.Edit;
                BtnState();
                //设置焦点
                this.txtId.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// add try ... catch
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
                if (SaveEmrItem(SetEntityByPage()))
                {
                    if (m_EditState == EditState.Add)
                    {
                        m_app.CustomMessageBox.MessageShow("新增成功");
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow("修改成功");
                    }
                    RefreshData();
                    //this.gridControl1.DataSource = m_SysTableManger.GetModelFactory(""); ;
                }
                else
                {
                    if (m_EditState == EditState.Add)
                    {
                        m_app.CustomMessageBox.MessageShow("新增失败");
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow("修改失败");
                    }
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
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、添加焦点
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
                //设置焦点
                this.btnADD.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 方法
        /// <summary>
        /// 实现接口部分 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost host)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 进行保存操作
        /// </summary>
        /// <param name="emrReplaceItem"></param>
        /// <returns></returns>
        private bool SaveEmrItem(EmrReplaceItem emrItem)
        {
            try
            {
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";
                m_SysTableManger.SaveEmrItem(emrItem, edittype);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 画面验证
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、提示优化
        /// 3、逻辑优化
        /// </summary>
        private bool IsSave()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtId.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("记录编号不能为空");
                    this.txtId.Focus();
                    return false;
                }
                else if (m_EditState == EditState.Add && !m_SysTableManger.CheckEmrItemID(this.txtId.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("该记录编号已存在，请重新输入。");
                    this.txtId.Focus();
                    return false;
                }
                //if (this.txtDestEmrName.Text.Trim() == "")
                //{
                //    m_app.CustomMessageBox.MessageShow("请输目标入院记录！");
                //    return false;
                //}
                else if (string.IsNullOrEmpty(this.txtSourceEmrName.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("源病历不能为空");
                    this.txtSourceEmrName.Focus();
                    return false;
                }
                else if (string.IsNullOrEmpty(this.txtDestItemName.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("目标病历元素不能为空");
                    return false;
                }
                else if (string.IsNullOrEmpty(this.txtSourceItemName.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("源病历元素不能为空");
                    this.txtSourceItemName.Focus();
                    return false;
                }
                else if (this.cmbValid.SelectedIndex == -1)
                {
                    m_app.CustomMessageBox.MessageShow("请选择是否有效");
                    this.cmbValid.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 清空页面控件的值
        /// </summary>
        private void ClearPageValue()
        {
            this.txtId.Text = "";
            this.txtDestEmrName.Text = "";
            this.txtDestItemName.Text = "";
            this.txtSourceEmrName.Text = "";
            this.txtSourceItemName.Text = "";
            cmbValid.SelectedIndex = -1;
        }

        /// <summary>
        /// 重置方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-16</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset()
        {
            try
            {
                this.txtDestEmrName.Text = "";
                this.txtDestItemName.Text = "";
                this.txtSourceEmrName.Text = "";
                this.txtSourceItemName.Text = "";
                cmbValid.SelectedIndex = -1;

                if (m_EditState == EditState.Add)
                {
                    this.txtId.Text = "";
                    this.txtId.Focus();
                }
                else
                {
                    this.txtDestEmrName.Focus();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            BindData();
            m_EditState = EditState.View;
            BtnState();
            ClearPageValue();
        }
        /// <summary>
        /// 绑定Grid数据
        /// </summary>
        private void BindData()
        {
            //if (m_DiagTable == null)
            DataTable m_DiagTable = new DataTable();
            m_DiagTable = m_SysTableManger.GetModelFactory("");
            this.gridControl1.DataSource = m_DiagTable;
        }

        /// <summary>
        /// /通过判断不同类型操作控件按钮状态
        /// Modify by xlb 2013-04-03
        /// </summary>
        private void BtnState()
        {
            //查看详细状态
            if (m_EditState == EditState.View)
            {
                this.btnADD.Enabled = true;
                this.btnDel.Enabled = true;
                this.BtnEdit.Enabled = true;

                this.btnSave.Enabled = false;
                this.btn_reset.Enabled = false;
                this.BtnClear.Enabled = false;

                this.txtId.Enabled = false;
                this.txtDestEmrName.Enabled = false;
                this.txtDestItemName.Enabled = false;
                this.txtSourceEmrName.Enabled = false;
                this.txtSourceItemName.Enabled = false;
                cmbValid.Enabled = false;
            }
            else if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
            {
                this.btnADD.Enabled = false;//新增或编辑状态新增和编辑按钮应置灰
                this.btnDel.Enabled = false;
                this.BtnEdit.Enabled = false;

                this.btnSave.Enabled = true;
                this.btn_reset.Enabled = true;
                this.BtnClear.Enabled = true;

                if (m_EditState == EditState.Add)
                {
                    this.txtId.Enabled = true;
                }
                else
                {
                    this.txtId.Enabled = false;
                }
                this.txtDestEmrName.Enabled = true;
                this.txtDestItemName.Enabled = true;
                this.txtSourceEmrName.Enabled = true;
                this.txtSourceItemName.Enabled = true;
                //this.btnADD.Enabled = true;
                //this.btnDel.Enabled = true;
                cmbValid.Enabled = true;
            }

        }
        /// <summary>
        /// 将实体值赋给页面元素
        /// </summary>
        /// <param name="emrReplaceItem"></param>
        private void SetPageValue(EmrReplaceItem emrReplaceItem)
        {
            if (emrReplaceItem == null || emrReplaceItem.Id == "")
                return;
            txtId.Text = emrReplaceItem.Id.Trim();
            txtDestEmrName.Text = emrReplaceItem.DestEmrName.Trim();
            txtDestItemName.Text = emrReplaceItem.DestItemName.Trim();
            txtSourceEmrName.Text = emrReplaceItem.SourceEmrName.Trim();
            txtSourceItemName.Text = emrReplaceItem.SourceItemName.Trim();
            cmbValid.SelectedIndex = emrReplaceItem.Valid;
        }

        /// <summary>
        /// 设置编辑区域值
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-07</date>
        /// <param name="dr"></param>
        /// </summary>
        private void SetPageValueByDataRow(DataRow dr)
        {
            try
            {
                if (null != dr && dr.ItemArray.Length >= 6)
                {
                    this.txtId.Text = null == dr["ID"] ? "" : dr["ID"].ToString();
                    this.txtDestEmrName.Text = null == dr["Dest_EmrName"] ? "" : dr["Dest_EmrName"].ToString();
                    this.txtSourceEmrName.Text = null == dr["Source_EmrName"] ? "" : dr["Source_EmrName"].ToString();
                    this.txtDestItemName.Text = null == dr["Dest_ItemName"] ? "" : dr["Dest_ItemName"].ToString();
                    this.txtSourceItemName.Text = null == dr["Source_ItemName"] ? "" : dr["Source_ItemName"].ToString();
                    this.cmbValid.SelectedIndex = (null == dr["VALID"] || dr["VALID"].ToString().Trim() == "") ? -1 : int.Parse(dr["VALID"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 将grid中的值赋值给实体
        /// </summary>
        /// <param name="foucesRow"></param>
        /// <returns></returns>
        private EmrReplaceItem SetEntityByDataRow(DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            EmrReplaceItem emrReplaceItem = new EmrReplaceItem();

            emrReplaceItem.Id = dr["id"].ToString();
            emrReplaceItem.DestEmrName = dr["Dest_EmrName"].ToString();
            emrReplaceItem.DestItemName = dr["Dest_ItemName"].ToString();
            emrReplaceItem.SourceItemName = dr["Source_ItemName"].ToString();
            emrReplaceItem.SourceEmrName = dr["Source_EmrName"].ToString();
            emrReplaceItem.Valid = Convert.ToInt32(dr["Valid"].ToString());

            return emrReplaceItem;
        }
        /// <summary>
        /// 将页面值加到实体里
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private EmrReplaceItem SetEntityByPage()
        {
            EmrReplaceItem emrReplaceItem = new EmrReplaceItem();

            emrReplaceItem.Id = txtId.Text.Trim();
            emrReplaceItem.DestEmrName = txtDestEmrName.Text.Trim();
            emrReplaceItem.DestItemName = txtDestItemName.Text.Trim();
            emrReplaceItem.SourceItemName = txtSourceItemName.Text.Trim();
            emrReplaceItem.SourceEmrName = txtSourceEmrName.Text.Trim();
            emrReplaceItem.Valid = cmbValid.SelectedIndex;

            return emrReplaceItem;

        }
        #endregion

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-07</date>
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
                MyMessageBox.Show(1, ex);
            }
        }
    }

    public enum EditState
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 1,
        /// <summary>
        /// 新增
        /// </summary>
        Add = 2,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 4,
        /// <summary>
        /// 视图
        /// </summary>
        View = 8
    }
}
