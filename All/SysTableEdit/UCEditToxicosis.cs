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

namespace DrectSoft.SysTableEdit
{
    public partial class UCEditToxicosis : DevExpress.XtraEditors.XtraUserControl, IStartPlugIn
    {
        private IEmrHost m_app;

        SysTableManger m_SysTableManger;

        /// <summary>
        /// 当前页面状态
        /// </summary>
        EditState m_EditState = EditState.None;

        public UCEditToxicosis(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_SysTableManger = new SysTableManger(m_app);
        }

        private void UCEditToxicosis_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

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
            DataTable dt = m_SysTableManger.GetToxicosis_Table("");
            this.gridControl1.DataSource = dt;
        }

        /// <summary>
        /// 将gridview中对应行值加载到实体中
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Toxicosis SetEntityByDataRow(DataRow dr)
        {
            if (dr == null)
            {
                return null;
            }
            Toxicosis toxicosis = new Toxicosis();

            toxicosis.Id = dr["Id"].ToString();
            toxicosis.Mapid = dr["Mapid"].ToString();
            toxicosis.Standardcode = dr["Standardcode"].ToString();
            toxicosis.Name = dr["Name"].ToString();
            toxicosis.Py = dr["Py"].ToString();

            toxicosis.Wb = dr["Wb"].ToString();
            toxicosis.Valid = Convert.ToInt32(dr["Valid"].ToString());
            toxicosis.Memo = dr["Memo"].ToString();


            return toxicosis;
        }

        /// <summary>
        /// 将损伤中毒库实体中值加载到页面
        /// </summary>
        /// <param name="Toxicosis"></param>
        private void SetPageValue(Toxicosis Toxicosis)
        {
            if (Toxicosis == null || Toxicosis.Id == "")
                return;

            this.txtID.Text = Toxicosis.Id.Trim();
            this.txtMapID.Text = Toxicosis.Mapid.Trim();
            this.txtName.Text = Toxicosis.Name.Trim();
            this.txtPy.Text = Toxicosis.Py.Trim();
            this.txtWb.Text = Toxicosis.Wb.Trim();

            this.cmbValid.SelectedIndex = Toxicosis.Valid;
            this.txtMemo.Text = Toxicosis.Memo.Trim();
            this.txtStandardCode.Text = Toxicosis.Standardcode.Trim();


        }

        /// <summary>
        /// 将页面值加入到损伤中毒库实体中
        /// </summary>
        /// <returns></returns>
        private Toxicosis SetEntityByPageValue()
        {
            Toxicosis Toxicosis = new Toxicosis();


            Toxicosis.Id = this.txtID.Text.Trim();
            Toxicosis.Mapid = this.txtMapID.Text.Trim();
            Toxicosis.Standardcode = this.txtStandardCode.Text.Trim();
            Toxicosis.Name = this.txtName.Text.Trim();
            Toxicosis.Py = this.txtPy.Text.Trim();
            Toxicosis.Wb = this.txtWb.Text.Trim();

            Toxicosis.Valid = this.cmbValid.SelectedIndex;
            Toxicosis.Memo = this.txtMemo.Text.Trim();


            return Toxicosis;
        }

        /// <summary>
        /// 保存损伤中毒库值
        /// </summary>
        /// <param name="diag"></param>
        /// <returns></returns>
        private bool SaveToxicosis(Toxicosis toxi)
        {
            try
            {
                string edittype = "";
                if (m_EditState == EditState.Add)
                    edittype = "1";
                else
                    edittype = "2";
                m_SysTableManger.SaveToxicosis(toxi, edittype);
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

                this.txtMapID.Enabled = false;
                this.txtID.Enabled = false;
                this.txtStandardCode.Enabled = false;
                this.txtName.Enabled = false;
                this.txtPy.Enabled = false;

                this.txtWb.Enabled = false;
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
                this.txtStandardCode.Enabled = true;
                this.txtName.Enabled = true;
                this.txtPy.Enabled = true;

                this.txtWb.Enabled = true;
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
                m_app.CustomMessageBox.MessageShow("请输入损伤中毒代码！");
                return false;
            }

            if (this.txtName.Text.Trim() == "")
            {
                m_app.CustomMessageBox.MessageShow("请输入损伤中毒名称！");
                return false;
            }

            if (m_EditState == EditState.Add)
                if (!m_SysTableManger.CheckToxicosisID(this.txtID.Text.Trim()))
                {
                    m_app.CustomMessageBox.MessageShow("输入损伤中毒代码已在数据库中存在！");
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
            if (m_SysTableManger.DelToxicosis(id))
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
            if (SaveToxicosis(SetEntityByPageValue()))
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增损伤中毒库成功！");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改损伤中毒库成功！");
                }
                RefreshData();

            }
            else
            {
                if (m_EditState == EditState.Add)
                {
                    m_app.CustomMessageBox.MessageShow("新增损伤中毒库失败！");
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("修改损伤中毒库失败！");
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
