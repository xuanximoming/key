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

namespace DrectSoft.SysTableEdit
{
    public partial class UCEditSurgery : DevExpress.XtraEditors.XtraUserControl, IStartPlugIn
    {
        private IEmrHost m_app;

        SysTableManger m_SysTableManger;

        /// <summary>
        /// 当前页面状态
        /// </summary>
        EditState m_EditState = EditState.None;

        public UCEditSurgery(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_SysTableManger = new SysTableManger(m_app);
        }

        private void UCEditSurgery_Load(object sender, EventArgs e)
        {
            InitSslb();
            InitBzlb();
            RefreshData();
        }



        #region 初始化页面下拉框
        /// <summary>
        /// 初始手术类别
        /// </summary>
        private void InitSslb()
        {

            lookUpWindowsslb.SqlHelper = m_app.SqlHelper;

            string sql = string.Format(@"select * from categorydetail a where a.categoryid = 8 ");
            DataTable Sslb = m_app.SqlHelper.ExecuteDataTable(sql);

            Sslb.Columns["ID"].Caption = "手术类别代码";
            Sslb.Columns["NAME"].Caption = "手术类别名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 80);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Sslb, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorsslb.SqlWordbook = deptWordBook;

        }

        /// <summary>
        /// 初始病种类别
        /// </summary>
        private void InitBzlb()
        {

            lookUpWindowbzlb.SqlHelper = m_app.SqlHelper;

            string sql = string.Format(@"select * from diseasecfg a where a.category = 701 ");
            DataTable Bzlb = m_app.SqlHelper.ExecuteDataTable(sql);

            Bzlb.Columns["ID"].Caption = "手术病种代码";
            Bzlb.Columns["NAME"].Caption = "手术病种名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 80);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorbzlb.SqlWordbook = deptWordBook;

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
            DataTable dt = m_SysTableManger.GetSurgery_Table("");
            this.gridControl1.DataSource = dt;
        }

        /// <summary>
        /// 将gridview中对应行值加载到实体中
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Surgery SetEntityByDataRow(DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            Surgery surgery = new Surgery();

            surgery.Id = dr["Id"].ToString();
            surgery.Mapid = dr["Mapid"].ToString();
            surgery.Standardcode = dr["Standardcode"].ToString();
            surgery.Name = dr["Name"].ToString();
            surgery.Py = dr["Py"].ToString();

            surgery.Wb = dr["Wb"].ToString();
            surgery.Valid = Convert.ToInt32(dr["Valid"].ToString());
            surgery.Memo = dr["Memo"].ToString();
            surgery.Bzlb = dr["Bzlb"].ToString();
            surgery.Sslb = Convert.ToInt32(dr["Sslb"].ToString());

            return surgery;
        }

        /// <summary>
        /// 将手术实体中值加载到页面
        /// </summary>
        /// <param name="Surgery"></param>
        private void SetPageValue(Surgery surgery)
        {
            if (surgery == null || surgery.Id == "")
                return;

            this.txtID.Text = surgery.Id.Trim();
            this.txtMapID.Text = surgery.Mapid.Trim();
            this.txtName.Text = surgery.Name.Trim();
            this.txtPy.Text = surgery.Py.Trim();
            this.txtWb.Text = surgery.Wb.Trim();

            this.cmbValid.SelectedIndex = surgery.Valid;
            this.txtMemo.Text = surgery.Memo.Trim();
            this.txtStandardCode.Text = surgery.Standardcode.Trim();
            this.lookUpEditorbzlb.CodeValue = surgery.Bzlb.ToString().Trim();
            this.lookUpEditorsslb.CodeValue = surgery.Sslb.ToString().Trim();

        }

        /// <summary>
        /// 将页面值加入到手术实体中
        /// </summary>
        /// <returns></returns>
        private Surgery SetEntityByPageValue()
        {
            Surgery surgery = new Surgery();


            surgery.Id = this.txtID.Text.Trim();
            surgery.Mapid = this.txtMapID.Text.Trim();
            surgery.Standardcode = this.txtStandardCode.Text.Trim();
            surgery.Name = this.txtName.Text.Trim();
            surgery.Py = this.txtPy.Text.Trim();
            surgery.Wb = this.txtWb.Text.Trim();

            surgery.Valid = this.cmbValid.SelectedIndex;
            surgery.Memo = this.txtMemo.Text.Trim();
            surgery.Bzlb = this.lookUpEditorbzlb.CodeValue.Trim();
            surgery.Sslb = Convert.ToInt32(lookUpEditorsslb.CodeValue.Trim());

            return surgery;
        }

        /// <summary>
        /// 保存手术值
        /// </summary>
        /// <param name="diag"></param>
        /// <returns></returns>
        private bool SaveSurgery(Surgery surg)
        {
            try
            {
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";
                m_SysTableManger.SaveSurgery(surg, edittype);
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
            this.txtStandardCode.Text = "";
            this.txtName.Text = "";
            this.txtPy.Text = "";

            this.txtWb.Text = "";
            this.lookUpEditorbzlb.CodeValue = "";
            cmbValid.SelectedIndex = -1;
            this.txtMemo.Text = "";
            this.lookUpEditorsslb.CodeValue = "";
        }

        /// <summary>
        /// 控制页面控件状态
        /// </summary>
        private void BtnState()
        {
            if (m_EditState == EditState.View)
            {
                this.btnADD.Enabled = true;
                this.btnDel.Enabled = true;
                this.BtnEdit.Enabled = true;

                this.btnSave.Enabled = false;
                this.BtnClear.Enabled = false;

                this.txtMapID.Enabled = false;
                this.txtID.Enabled = false;
                this.txtStandardCode.Enabled = false;
                this.txtName.Enabled = false;
                this.txtPy.Enabled = false;

                this.txtWb.Enabled = false;
                this.lookUpEditorbzlb.Enabled = false;
                cmbValid.Enabled = false;
                this.txtMemo.Enabled = false;
                this.lookUpEditorsslb.Enabled = false;
            }
            else if (m_EditState == EditState.Add || m_EditState == EditState.Edit)
            {
                this.btnADD.Enabled = false;
                this.btnDel.Enabled = false;
                this.BtnEdit.Enabled = false;

                this.btnSave.Enabled = true;
                this.BtnClear.Enabled = true;

                if (m_EditState == EditState.Add)
                    this.txtID.Enabled = true;
                else
                    this.txtID.Enabled = false;
                this.txtMapID.Enabled = true;
                this.txtStandardCode.Enabled = true;
                this.txtName.Enabled = true;
                this.txtPy.Enabled = true;

                this.txtWb.Enabled = true;
                this.lookUpEditorbzlb.Enabled = true;
                cmbValid.Enabled = true;
                this.txtMemo.Enabled = true;
                this.lookUpEditorsslb.Enabled = true;
            }

        }

        /// <summary>
        /// 验证页面数据是否可以保存
        /// </summary>
        /// <returns></returns>
        private bool IsSave()
        {
            if (this.txtID.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请输入手术代码！");
                return false;
            }

            if (this.txtName.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请输入手术名称！");
                return false;
            }

            if (this.lookUpEditorsslb.CodeValue.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请输入手术类别！");
                return false;
            }

            if (m_EditState == EditState.Add)
                if (!m_SysTableManger.CheckSurgeryID(this.txtID.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("输入入手术代码已在数据库中存在！");
                    return false;
                }

            return true;
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
        private void btnTypeADD_Click(object sender, EventArgs e)
        {
            m_EditState = EditState.Add;
            ClearPageValue();
            BtnState();

        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeDel_Click(object sender, EventArgs e)
        {
            string id = this.txtID.Text.Trim();
            if (id == "")
            {
                m_app.CustomMessageBox.MessageShow("请选择要删除的记录！");
                return;
            }
            if (m_SysTableManger.DelSurgery(id))
            {
                m_app.CustomMessageBox.MessageShow("删除成功！");
                RefreshData();
            }
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTypeEdit_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
                return;
            DataRow foucesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (foucesRow == null)
                return;

            if (foucesRow.IsNull("ID"))
                return;

            SetPageValue(SetEntityByDataRow(foucesRow));

            m_EditState = EditState.Edit;
            BtnState();
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeSave_Click(object sender, EventArgs e)
        {
            if (!IsSave())
                return;
            if (SaveSurgery(SetEntityByPageValue()))
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增手术成功！");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改手术成功！");
                }
                RefreshData();

            }
            else
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增手术失败！");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改手术失败！");
                }
            }
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTypeClear_Click(object sender, EventArgs e)
        {
            ClearPageValue();
            m_EditState = EditState.View;
            BtnState();
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
    }
}
