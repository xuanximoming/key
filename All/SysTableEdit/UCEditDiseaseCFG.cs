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
    public partial class UCEditDiseaseCFG : DevExpress.XtraEditors.XtraUserControl, IStartPlugIn
    {
        private IEmrHost m_app;

        SysTableManger m_SysTableManger;

        /// <summary>
        /// 当前页面状态
        /// </summary>
        EditState m_EditState = EditState.None;

        public UCEditDiseaseCFG(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_SysTableManger = new SysTableManger(m_app);
        }

        private void UCEditDiagnosis_Load(object sender, EventArgs e)
        {
            InitBzlb();
            RefreshData();
        }



        #region 初始化页面下拉框

        /// <summary>
        /// 初始病种类别
        /// </summary>
        private void InitBzlb()
        {

            lookUpWindowCategory.SqlHelper = m_app.SqlHelper;

            string sql = string.Format(@"select * from categorydetail a where a.categoryid = 7 ");
            DataTable Bzlb = m_app.SqlHelper.ExecuteDataTable(sql);

            Bzlb.Columns["ID"].Caption = "病种代码";
            Bzlb.Columns["NAME"].Caption = "病种名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ID", 80);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ID", "NAME", cols, "ID//Name//PY//WB");
            lookUpEditorCategory.SqlWordbook = deptWordBook;

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
            DataTable dt = m_SysTableManger.GetDiseaseCFG_Table("");
            this.gridControl1.DataSource = dt;
        }

        /// <summary>
        /// 将gridview中对应行值加载到实体中
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private DiseaseCFG SetEntityByDataRow(DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            DiseaseCFG diseaseCFG = new DiseaseCFG();

            diseaseCFG.Id = dr["Id"].ToString();
            diseaseCFG.Mapid = dr["Mapid"].ToString();
            diseaseCFG.Name = dr["Name"].ToString();
            diseaseCFG.Py = dr["Py"].ToString();
            diseaseCFG.Wb = dr["Wb"].ToString();

            diseaseCFG.Diseaseid = dr["Diseaseid"].ToString();
            diseaseCFG.Surgeryid = dr["Surgeryid"].ToString();
            diseaseCFG.Category = dr["Category"].ToString();
            diseaseCFG.Mark = dr["Mark"].ToString();

            diseaseCFG.Parentid = dr["Parentid"].ToString();
            diseaseCFG.Valid = Convert.ToInt32(dr["Valid"].ToString());
            diseaseCFG.Memo = dr["Memo"].ToString();

            return diseaseCFG;
        }

        /// <summary>
        /// 将病种实体中值加载到页面
        /// </summary>
        /// <param name="diagnosis"></param>
        private void SetPageValue(DiseaseCFG diseaseCFG)
        {
            if (diseaseCFG == null || diseaseCFG.Id == "")
                return;

            this.txtID.Text = diseaseCFG.Id.Trim();
            this.txtMapID.Text = diseaseCFG.Mapid.Trim();
            this.txtName.Text = diseaseCFG.Name.Trim();
            this.txtPy.Text = diseaseCFG.Py.Trim();
            this.txtWb.Text = diseaseCFG.Wb.Trim();

            this.txtDiseaseID.Text = diseaseCFG.Diseaseid.Trim();
            this.txtSurgeryID.Text = diseaseCFG.Surgeryid.Trim();
            this.lookUpEditorCategory.CodeValue = diseaseCFG.Category.Trim();
            this.txtMark.Text = diseaseCFG.Mark.Trim();

            this.txtParentID.Text = diseaseCFG.Parentid.Trim();
            cmbValid.SelectedIndex = diseaseCFG.Valid;
            this.txtMemo.Text = diseaseCFG.Memo.Trim();
        }

        /// <summary>
        /// 将页面值加入到病种实体中
        /// </summary>
        /// <returns></returns>
        private DiseaseCFG SetEntityByPageValue()
        {
            DiseaseCFG diseaseCFG = new DiseaseCFG();


            diseaseCFG.Id = this.txtID.Text.Trim();
            diseaseCFG.Mapid = this.txtMapID.Text.Trim();
            diseaseCFG.Name = this.txtName.Text.Trim();
            diseaseCFG.Py = this.txtPy.Text.Trim();
            diseaseCFG.Wb = this.txtWb.Text.Trim();

            diseaseCFG.Diseaseid = this.txtDiseaseID.Text.Trim();
            diseaseCFG.Surgeryid = this.txtSurgeryID.Text.Trim();
            diseaseCFG.Category = this.lookUpEditorCategory.CodeValue.Trim();
            diseaseCFG.Mark = this.txtMark.Text.Trim();

            diseaseCFG.Parentid = this.txtParentID.Text.Trim();
            diseaseCFG.Valid = cmbValid.SelectedIndex;
            diseaseCFG.Memo = this.txtMemo.Text.Trim();

            return diseaseCFG;
        }

        /// <summary>
        /// 保存病种值
        /// </summary>
        /// <param name="diag"></param>
        /// <returns></returns>
        private bool SaveDiseaseCFG(DiseaseCFG diseaseCFG)
        {
            try
            {
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";
                m_SysTableManger.SaveDiseaseCFG(diseaseCFG, edittype);
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
            this.txtID.Text =  "";
            this.txtMapID.Text =  "";
            this.txtName.Text =  "";
            this.txtPy.Text =  "";
            this.txtWb.Text =  "";

            this.txtDiseaseID.Text =  "";
            this.txtSurgeryID.Text =  "";
            this.lookUpEditorCategory.CodeValue = "";
            this.txtMark.Text =  "";

            this.txtParentID.Text =  "";
            cmbValid.SelectedIndex = -1;
            this.txtMemo.Text = "";
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

                this.txtID.Enabled = false;
                this.txtMapID.Enabled = false;
                this.txtName.Enabled = false;
                this.txtPy.Enabled = false;
                this.txtWb.Enabled = false;

                this.txtDiseaseID.Enabled = false;
                this.txtSurgeryID.Enabled = false;
                this.lookUpEditorCategory.Enabled = false;
                this.txtMark.Enabled = false;

                this.txtParentID.Enabled = false;
                cmbValid.Enabled = false;
                this.txtMemo.Enabled = false;
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
                this.txtName.Enabled = true;
                this.txtPy.Enabled = true;
                this.txtWb.Enabled = true;

                this.txtDiseaseID.Enabled = true;
                this.txtSurgeryID.Enabled = true;
                this.lookUpEditorCategory.Enabled = true;
                this.txtMark.Enabled = true;
                this.txtParentID.Enabled = true;

                cmbValid.Enabled = true;
                this.txtMemo.Enabled = true;
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
                m_app.CustomMessageBox.MessageShow("请输入病种代码！");
                return false;
            }
 
            if (this.txtName.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请输入病种名称！");
                return false;
            }
            if (this.cmbValid.SelectedIndex == -1)
            {
                m_app.CustomMessageBox.MessageShow("请选择是否有效！");
                return false;
            }
            if (m_EditState == EditState.Add)
            if (!m_SysTableManger.CheckDiseaseCFGID(this.txtID.Text.Trim()))
            {
                m_app.CustomMessageBox.MessageShow("输入病种代码已在数据库中存在！");
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
            if (m_SysTableManger.DelDiseaseCFG(id))
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
            if (SaveDiseaseCFG(SetEntityByPageValue()))
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增成功！");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改成功！");
                }
                RefreshData();

            }
            else
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增失败！");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改失败！");
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
