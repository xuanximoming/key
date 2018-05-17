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
using DrectSoft.Common;

namespace DrectSoft.SysTableEdit
{
    public partial class UCEditDiagnosisOfChinese : DevExpress.XtraEditors.XtraUserControl, IStartPlugIn
    {
        private IEmrHost m_app;

        SysTableManger m_SysTableManger;

        /// <summary>
        /// 当前页面状态
        /// </summary>
        EditState m_EditState = EditState.None;

        public UCEditDiagnosisOfChinese(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_SysTableManger = new SysTableManger(m_app);
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、初始化焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCEditDiagnosis_Load(object sender, EventArgs e)
        {
            try
            {
                InitBzlb();
                //新增初始化中医诊断检索框数据
                InitLookDiag();
                RefreshData();
                this.lookUpEditorDiag.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        DataTable m_DiagTable;

        /// <summary>
        /// 新增初始化中医诊断检索框数据
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、优化下拉列表
        /// </summary>
        private void InitLookDiag()
        {
            try
            {
                lookUpWindowDiag.SqlHelper = m_app.SqlHelper;
                if (m_DiagTable == null)
                    m_DiagTable = m_SysTableManger.GetDiagnosisOfChinese_Table("");
                m_DiagTable.Columns["ID"].Caption = "诊断代码";
                m_DiagTable.Columns["NAME"].Caption = "诊断名称";
                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", 80);
                cols.Add("NAME", 170);
                SqlWordbook deptWordBook = new SqlWordbook("querybook", m_DiagTable, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorDiag.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #region 初始化页面下拉框值

        /// <summary>
        /// 初始病种类别
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、优化下拉列表
        /// </summary>
        private void InitBzlb()
        {
            try
            {
                lookUpWindowCategory.SqlHelper = m_app.SqlHelper;

                string sql = string.Format(@"select * from categorydetail a where a.categoryid = 7 ");
                DataTable Bzlb = m_app.SqlHelper.ExecuteDataTable(sql);

                Bzlb.Columns["ID"].Caption = "病种代码";
                Bzlb.Columns["NAME"].Caption = "病种名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 60);
                cols.Add("NAME", 120);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
                lookUpEditorCategory.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 方法

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
        /// 绑定grid值
        /// </summary>
        private void BindGrid()
        {
            DataTable dt = m_SysTableManger.GetDiagnosisOfChinese_Table("");
            this.gridControl1.DataSource = dt;
        }

        /// <summary>
        /// 将gridview中对应行值加载到实体中
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private DiagnosisOfChinese SetEntityByDataRow(DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            DiagnosisOfChinese diagnosis = new DiagnosisOfChinese();

            diagnosis.Id = dr["Id"].ToString();
            diagnosis.Mapid = dr["Mapid"].ToString();
            diagnosis.Name = dr["Name"].ToString();
            diagnosis.Py = dr["Py"].ToString();
            diagnosis.Wb = dr["Wb"].ToString();

            diagnosis.Valid = Convert.ToInt32(dr["Valid"].ToString());
            diagnosis.Memo = dr["Memo"].ToString();
            diagnosis.Memo1 = dr["Memo1"].ToString();
            diagnosis.Category = dr["Category"].ToString();

            return diagnosis;
        }

        /// <summary>
        /// 将诊断实体中值加载到页面
        /// </summary>
        /// <param name="diagnosis"></param>
        private void SetPageValue(DiagnosisOfChinese diagnosis)
        {
            if (diagnosis == null || diagnosis.Id == "")
                return;

            this.txtID.Text = diagnosis.Id.Trim();
            this.txtMapID.Text = diagnosis.Mapid.Trim();
            this.txtName.Text = diagnosis.Name.Trim();
            this.txtPy.Text = diagnosis.Py.Trim();
            this.txtWb.Text = diagnosis.Wb.Trim();

            this.cmbValid.SelectedIndex = diagnosis.Valid;
            this.txtMemo.Text = diagnosis.Memo.Trim();
            this.txtMemo1.Text = diagnosis.Memo1.Trim();
            this.lookUpEditorCategory.CodeValue = diagnosis.Category.Trim();

        }

        /// <summary>
        /// 将页面值加入到诊断实体中
        /// </summary>
        /// <returns></returns>
        private DiagnosisOfChinese SetEntityByPageValue()
        {
            DiagnosisOfChinese diagnosis = new DiagnosisOfChinese();


            diagnosis.Id = this.txtID.Text.Trim();
            diagnosis.Mapid = this.txtMapID.Text.Trim();
            diagnosis.Name = this.txtName.Text.Trim();
            diagnosis.Py = this.txtPy.Text.Trim();
            diagnosis.Wb = this.txtWb.Text.Trim();

            diagnosis.Valid = this.cmbValid.SelectedIndex;
            diagnosis.Memo = this.txtMemo.Text.Trim();
            diagnosis.Memo1 = this.txtMemo1.Text.Trim();
            diagnosis.Category = this.lookUpEditorCategory.CodeValue.Trim();

            return diagnosis;
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
                if (null != dr && dr.ItemArray.Length >= 5)
                {
                    this.txtID.Text = null == dr["ID"] ? "" : dr["ID"].ToString();
                    this.txtMapID.Text = null == dr["MAPID"] ? "" : dr["MAPID"].ToString();
                    this.txtMemo1.Text = null == dr["MEMO1"] ? "" : dr["MEMO1"].ToString();
                    this.txtName.Text = null == dr["NAME"] ? "" : dr["NAME"].ToString();
                    this.txtPy.Text = null == dr["PY"] ? "" : dr["PY"].ToString();
                    this.txtWb.Text = null == dr["WB"] ? "" : dr["WB"].ToString();
                    this.lookUpEditorCategory.CodeValue = null == dr["CATEGORYID"] ? "" : dr["CATEGORYID"].ToString();
                    this.cmbValid.SelectedIndex = (null == dr["VALID"] || dr["VALID"].ToString().Trim() == "") ? -1 : int.Parse(dr["VALID"].ToString());
                    this.txtMemo.Text = null == dr["MEMO"] ? "" : dr["MEMO"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存诊断值
        /// </summary>
        /// <param name="diag"></param>
        /// <returns></returns>
        private bool SaveDiagnosis(DiagnosisOfChinese diag)
        {
            try
            {
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";
                m_SysTableManger.SaveDiagnosisOfChinese(diag, edittype);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        /// <summary>
        /// 清空页面空间值
        /// </summary>
        private void ClearPageValue()
        {
            this.txtMapID.Text = "";
            this.txtID.Text = "";
            this.txtMemo1.Text = "";
            this.txtName.Text = "";
            this.txtPy.Text = "";

            this.txtWb.Text = "";
            this.lookUpEditorCategory.CodeValue = "";
            cmbValid.SelectedIndex = -1;
            this.txtMemo.Text = "";
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
                this.txtID.Text = "";
                this.txtMemo1.Text = "";
                this.txtName.Text = "";
                this.txtPy.Text = "";

                this.txtWb.Text = "";
                this.lookUpEditorCategory.CodeValue = "";
                cmbValid.SelectedIndex = -1;
                this.txtMemo.Text = "";

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
        /// edit by Yanqiao.Cai 2012-11-07
        /// add try ... catch
        /// </summary>
        private void BtnState()
        {
            try
            {
                if (m_EditState == EditState.View)
                {
                    this.btnADD.Enabled = true;
                    this.btnDel.Enabled = true;
                    this.BtnEdit.Enabled = true;

                    this.btnSave.Enabled = false;
                    this.btn_reset.Enabled = false;
                    this.BtnClear.Enabled = false;

                    this.txtMapID.Enabled = false;
                    this.txtID.Enabled = false;
                    this.txtMemo1.Enabled = false;
                    this.txtName.Enabled = false;
                    this.txtPy.Enabled = false;

                    this.txtWb.Enabled = false;
                    this.lookUpEditorCategory.Enabled = false;
                    cmbValid.Enabled = false;
                    this.txtMemo.Enabled = false;
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
                    this.txtMemo1.Enabled = true;
                    this.txtName.Enabled = true;
                    this.txtPy.Enabled = true;

                    this.txtWb.Enabled = true;
                    this.lookUpEditorCategory.Enabled = true;
                    cmbValid.Enabled = true;
                    this.txtMemo.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 验证页面数据是否可以保存
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、优化提示
        /// 3、添加焦点
        /// </summary>
        private bool IsSave()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtID.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("诊断代码不能为空");
                    this.txtID.Focus();
                    return false;
                }
                else if (m_EditState == EditState.Add && !m_SysTableManger.CheckDiagnosisOfChineseID(this.txtID.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("该诊断代码已存在，请重新输入。");
                    this.txtID.Focus();
                    return false;
                }
                //else if (string.IsNullOrEmpty(this.txtMapID.Text.Trim()))
                //{
                //    m_app.CustomMessageBox.MessageShow("映射代码不能为空");
                //    this.txtMapID.Focus();
                //    return false;
                //}
                else if (string.IsNullOrEmpty(this.txtName.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("诊断名称不能为空");
                    this.txtName.Focus();
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

        #endregion

        public IPlugIn Run(FrameWork.WinForm.Plugin.IEmrHost host)
        {
            throw new NotImplementedException();
        }

        #region 事件
        /// <summary>
        /// 新增事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeADD_Click(object sender, EventArgs e)
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
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeDel_Click(object sender, EventArgs e)
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
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除诊断 " + dr["Name"] + " 吗？", "删除诊断", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                if (m_SysTableManger.DelDiagnosisOfChinese(dr["ID"].ToString()))
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
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、添加焦点
        /// 3、逻辑优化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTypeEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (null == dr || dr.IsNull("ID") || dr.ItemArray.Length <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                
                SetPageValue(SetEntityByDataRow(dr));
                SetPageValueByDataRow(dr);

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
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、优化提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsSave())
                {
                    return;
                }
                if (SaveDiagnosis(SetEntityByPageValue()))
                {
                    if (m_EditState == EditState.Add)
                    {
                        m_app.CustomMessageBox.MessageShow("新增诊断成功");
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow("修改诊断成功");
                    }
                    RefreshData();

                }
                else
                {
                    if (m_EditState == EditState.Add)
                    {
                        m_app.CustomMessageBox.MessageShow("新增诊断失败");
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow("修改诊断失败");
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTypeClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPageValue();
                m_EditState = EditState.View;
                BtnState();
                this.btnADD.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 
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

        #endregion
        /// <summary>
        /// 新增的中医诊断加入检索功能
        /// ywk 2012年5月30日 11:42:16
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorDiag_CodeValueChanged(object sender, EventArgs e)
        {
            if (lookUpEditorDiag.CodeValue != "")
            {
                DataTable dt = m_DiagTable.Clone();
                foreach (DataRow dr in m_DiagTable.Select(string.Format("ID='{0}'", lookUpEditorDiag.CodeValue)))
                {
                    dt.Rows.Add(dr.ItemArray);
                }
                gridControl1.DataSource = dt;

                gridControl1_Click(null, null);
            }
            else
            {
                gridControl1.DataSource = m_DiagTable;
            }
        }

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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnWeiHu_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = gridView1.GetFocusedDataRow();
                if (dr == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("未选中记录");
                    return;
                }
                DiagnosisOtherNaem diagnosisOtherNaem = new DiagnosisOtherNaem(dr["ID"].ToString(), "2");
                diagnosisOtherNaem.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        
    }
}
