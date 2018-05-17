using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.FrameWork.WinForm;
using YidanSoft.Common.Controls;
using YidanSoft.Common.Object2Editor.Encoding;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Repository;

namespace YidanSoft.Core.Symbol
{

    internal enum EditState
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

    /// <summary>
    /// 特殊字符编辑器
    /// </summary>
    public partial class SymbolManager : Form, YidanSoft.FrameWork.IStartPlugIn
    {
        #region vars
        private IYidanEmrHost m_app;

        private IDataAccess sql_Helper;

        DataManager m_DataManager;

        DataTable dt_SymbolType;

        DataTable dt_Symbol;

        EditState m_SymbolTypeState;
        EditState m_SymbolState;

        //old_变量为你记录编号，方便在数据未保存的情况下点击grid后提示是否保存数据用
        string old_SymbolTypeID = "";
        string old_SymbolID = "";

        #endregion


        public SymbolManager()
        {
            InitializeComponent();
        }

        public SymbolManager(IYidanEmrHost app)
        {
            InitializeComponent();
            m_app = app;

        }



        #region Load
        private void SymbolManager_Load(object sender, EventArgs e)
        {
            m_DataManager = new DataManager(m_app, sql_Helper);

            //设置页面布局
            splitContainerControl2.SplitterPosition = SystemInformation.PrimaryMonitorSize.Width - 400;
            splitContainerControl3.SplitterPosition = SystemInformation.PrimaryMonitorSize.Width - 400;
            BindData();

            m_SymbolTypeState = EditState.View;
            m_SymbolState = EditState.View;
            SetButState();

        }



        #endregion

        #region method

        private void BindData()
        {
            dt_SymbolType = m_DataManager.GetSymbolType();
            BindSymbolType();
            dt_Symbol = m_DataManager.GetSymbolDetail(dt_SymbolType.Rows[0]["ID"].ToString());
            BindSymbol();
        }

        /// <summary>
        /// 绑定特殊字符类型信息，将编辑窗口数据置为空
        /// </summary>
        private void BindSymbolType()
        {
            if (dt_SymbolType == null || dt_SymbolType.Rows.Count == 0)
            {
                this.gridSymbolType.DataSource = null;
            }
            else
            {
                this.gridSymbolType.DataSource = dt_SymbolType;
                m_app.PublicMethod.ConvertGridDataSourceUpper(gridViewSymbolType);
            }
            this.txtTypeID.Text = "";
            this.txttypeName.Text = "";
            this.txtTypeMemo.Text = "";
        }

        /// <summary>
        /// 绑定特殊字符信息，将编辑窗口数据置为空
        /// </summary>
        private void BindSymbol()
        {
            if (dt_Symbol == null || dt_Symbol.Rows.Count == 0)
            {
                this.gridSymbolDetail.DataSource = null;
            }
            else
            {
                this.gridSymbolDetail.DataSource = dt_Symbol;
                m_app.PublicMethod.ConvertGridDataSourceUpper(gridViewSymbol);
            }

            this.txtID.Text = "";
            this.txtRTF.Text = "";
            this.txtLength.Text = "";
            this.txtMemo.Text = "";
        }

        /// <summary>
        /// 绑定符号维护面板
        /// </summary>
        /// <param name="dr"></param>
        private void BindSymbolDetail(DataRowView dr)
        {
            if (dr == null)
                return;
            RTFHelper rtfhelper = new RTFHelper();
            this.txtID.Text = dr["ID"].ToString();
            try
            {
                this.txtRTF.Rtf = rtfhelper.GetRTFByStr(dr["RTFstr"].ToString());
            }
            catch
            {
                this.txtRTF.Text = rtfhelper.GetStrBYRTF(dr["RTFstr"].ToString());
            }

            this.txtMemo.Text = dr["Memo"].ToString();
            this.txtLength.Text = dr["Length"].ToString();
        }

        /// <summary>
        /// 绑定类别维护面板
        /// </summary>
        /// <param name="dr"></param>
        private void BindSymbolTypeDetail(DataRowView dr)
        {
            if (dr == null)
                return;
            this.txtTypeID.Text = dr["ID"].ToString();
            this.txttypeName.Text = dr["Name"].ToString();
            this.txtTypeMemo.Text = dr["Memo"].ToString();
        }

        private void SetButState()
        {
            if (m_SymbolTypeState == EditState.View || m_SymbolState == EditState.View)
            {
                //类别
                this.btnTypeADD.Enabled = true;
                this.BtnTypeEdit.Enabled = true;
                this.btnTypeDel.Enabled = true;

                this.btnTypeSave.Enabled = false;
                this.BtnTypeClear.Enabled = false;

                this.txtTypeID.Enabled = false;
                this.txttypeName.Enabled = false;
                this.txtTypeMemo.Enabled = false;

                //明细
                this.btnSymbolEdit.Enabled = true;
                this.btnSymbolADD.Enabled = true;
                this.btnSymbolDel.Enabled = true;

                this.btnSymbolSave.Enabled = false;
                this.btnSymbolClear.Enabled = false;

                this.txtID.Enabled = false;
                this.txtRTF.Enabled = false;
                this.txtMemo.Enabled = false;
                this.txtLength.Enabled = false;
            }

            if (m_SymbolTypeState == EditState.Edit|| m_SymbolTypeState == EditState.Add)
            {
                this.txtTypeID.Enabled = false;
                this.txtTypeMemo.Enabled = true;
                this.txttypeName.Enabled = true;

                this.btnTypeADD.Enabled = false;
                this.BtnTypeEdit.Enabled = false;
                this.btnTypeDel.Enabled = false;
                this.btnTypeSave.Enabled = true;
                this.BtnTypeClear.Enabled = true;
            }


            if (m_SymbolState == EditState.Add || m_SymbolState == EditState.Edit)
            {
                this.txtID.Enabled = false;
                this.txtRTF.Enabled = true;
                this.txtLength.Enabled = true;
                this.txtMemo.Enabled = true;


                this.btnSymbolADD.Enabled = false;
                this.btnSymbolEdit.Enabled = false;
                this.btnSymbolDel.Enabled = false;
                this.btnSymbolSave.Enabled = true;
                this.btnSymbolClear.Enabled = true;
            }

        }


        #endregion

        #region  event

        /// <summary>
        /// 点击类型grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewSymbolType_Click(object sender, EventArgs e)
        {


            if (gridViewSymbolType.FocusedRowHandle < -1)
                return;
            int typeRowID = gridViewSymbolType.FocusedRowHandle;
            //确认是否有信息未保存
            if (m_SymbolTypeState != EditState.View || m_SymbolState != EditState.View)
            {
                if (m_app.CustomMessageBox.MessageShow(string.Format("您有数据未保存，是否保存？"), CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                {
                    if (m_SymbolState != EditState.View)
                    {
                        btnSymbolSave_Click(null, null);
                    }
                    if (m_SymbolTypeState != EditState.View)
                    {
                        btnTypeSave_Click(null, null);
                    }
                }
                else
                {
                    m_SymbolTypeState = EditState.View;
                    m_SymbolState = EditState.View;

                    SetButState();
                }
            }

            string SymbolTypeID = dt_SymbolType.Rows[typeRowID]["ID"].ToString();

            DataRowView dr = (DataRowView)gridViewSymbolType.GetRow(typeRowID);
            if (dr != null)
            {
                BindSymbolTypeDetail(dr);

                dt_Symbol = m_DataManager.GetSymbolDetail(SymbolTypeID);
                BindSymbol();

                old_SymbolTypeID = SymbolTypeID;
                gridViewSymbolType.FocusedRowHandle = typeRowID;
            }

        }

        /// <summary>
        /// 点击特殊字符grid事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewSymbol_Click(object sender, EventArgs e)
        {
            if (gridViewSymbol.FocusedRowHandle < -1)
                return;
            int RowID = gridViewSymbol.FocusedRowHandle;
            //确认是否有信息未保存
            if (m_SymbolState != EditState.View)
            {
                if (m_app.CustomMessageBox.MessageShow(string.Format("您有数据未保存，是否保存？"), CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                {
                    if (m_SymbolState != EditState.View)
                    {
                        btnSymbolSave_Click(null, null);
                    }
                }
                else
                {
                    m_SymbolState = EditState.View;

                    SetButState();
                }
            }


            DataRowView dr = (DataRowView)gridViewSymbol.GetRow(RowID);
            if (dr != null)
            {
                BindSymbolDetail(dr);

                old_SymbolID = dr["ID"].ToString();
                gridViewSymbol.FocusedRowHandle = RowID;
            }

        }

        /// <summary>
        /// 特别字母类型修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTypeEdit_Click(object sender, EventArgs e)
        {
            if (this.txtTypeID.Text.ToString().Trim().Length == 0)
                gridViewSymbolType_Click(null, null);
            if (this.txtTypeID.Text.ToString().Trim().Length == 0)
            {
                m_app.CustomMessageBox.MessageShow("请选择需要修改的类别！");
                return;
            }
            m_SymbolTypeState = EditState.Edit;

            SetButState();
        }

        /// <summary>
        /// 特殊符号类别新增按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeADD_Click(object sender, EventArgs e)
        {
            m_SymbolTypeState = EditState.Add;
            this.txtTypeID.Text = "";
            this.txtTypeMemo.Text = "";
            this.txttypeName.Text = "";

            SetButState();

        }

        /// <summary>
        ///删除特殊符号类型按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeDel_Click(object sender, EventArgs e)
        {
            if (gridViewSymbolType.FocusedRowHandle < -1)
                return;
            DataRowView dr = (DataRowView)gridViewSymbolType.GetRow(gridViewSymbolType.FocusedRowHandle);
            if (dr != null)
            {
                string CategoryID = dr["ID"].ToString();
                if (m_DataManager.DelSymbolType(CategoryID))
                {
                    BindData();
                    m_SymbolTypeState = EditState.View;
                    m_SymbolState = EditState.View;
                    SetButState();
                }
            }
        }


        /// <summary>
        /// 特别字母类型取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTypeClear_Click(object sender, EventArgs e)
        {

            if (gridViewSymbolType.FocusedRowHandle < -1)
                return;
            int typeRowID = gridViewSymbolType.FocusedRowHandle;

            DataRowView dr = (DataRowView)gridViewSymbolType.GetRow(typeRowID);
            if (dr != null)
            {
                BindSymbolTypeDetail(dr);

            }
            m_SymbolTypeState = EditState.View;
            SetButState();

        }

        /// <summary>
        /// 特别字母类型保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTypeSave_Click(object sender, EventArgs e)
        {
            if (m_SymbolTypeState == EditState.Add)
            {
                string SymbolTypeName = this.txttypeName.Text.ToString().Trim();
                string SymbolTypeMemo = this.txtTypeMemo.Text.ToString().Trim();
                /**** add by dxj 2011/6/22 ****/
                if (SymbolTypeName == String.Empty)
                {
                    m_app.CustomMessageBox.MessageShow("类别名称不能为空！");
                    return;
                }
                /**********/
                if (m_DataManager.AddSymbolType(SymbolTypeName, SymbolTypeMemo))
                {
                    BindData();
                    m_SymbolTypeState = EditState.View;
                    m_SymbolState = EditState.View;
                    SetButState();
                }
            }
            else if (m_SymbolTypeState == EditState.Edit)
            {
                string SymbolTypeID = this.txtTypeID.Text.ToString().Trim();
                string SymbolTypeName = this.txttypeName.Text.ToString().Trim();
                string SymbolTypeMemo = this.txtTypeMemo.Text.ToString().Trim();

                if (m_DataManager.EditSymbolType(SymbolTypeID, SymbolTypeName, SymbolTypeMemo))
                {
                    BindData();
                    m_SymbolTypeState = EditState.View;
                    m_SymbolState = EditState.View;
                    SetButState();
                }
            }
        }

        /// <summary>
        /// 特别字母修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSymbolEdit_Click(object sender, EventArgs e)
        {
            if (this.txtID.Text.ToString().Trim().Length == 0)
                gridViewSymbol_Click(null, null);
            if (this.txtID.Text.ToString().Trim().Length == 0)
            {
                m_app.CustomMessageBox.MessageShow("请选择需要修改的特殊字符！");
                return;
            }
            m_SymbolState = EditState.Edit;

            SetButState();
        }

        /// <summary>
        /// 特别字母新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSymbolADD_Click(object sender, EventArgs e)
        {
            m_SymbolState = EditState.Add;
            DataRowView dr = (DataRowView)gridViewSymbolType.GetRow(gridViewSymbolType.FocusedRowHandle);
            string CategoryID = dr["ID"].ToString();
            this.txtID.Text = m_DataManager.GetNewSymbolID(CategoryID);
            this.txtRTF.Text = "";
            this.txtLength.Text = "";
            this.txtMemo.Text = "";

            SetButState();

        }


        /// <summary>
        /// 特别字母保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSymbolSave_Click(object sender, EventArgs e)
        {
            if (this.txtRTF.Text.ToString().Trim().Length <= 0)
            {
                m_app.CustomMessageBox.MessageShow("特殊字符为空！");
                return;
            }
            string ID = this.txtID.Text.ToString().Trim();
            string RTF = RtfEncoding.GetRtfCoreContent(txtRTF.Rtf).Replace(RtfEncoding.RtfNewLine, "");

            string CategoryID = "";
            //判断是否由点击grid后跳转过来
            if (sender != null)
            {
                DataRowView dr = (DataRowView)gridViewSymbolType.GetRow(gridViewSymbolType.FocusedRowHandle);
                CategoryID = dr["ID"].ToString();
            }
            else
            {
                CategoryID = old_SymbolTypeID;
            }
            string Length = this.txtLength.Text.ToString().Trim();
            string Memo = this.txtMemo.Text.ToString().Trim();

            if (m_SymbolState ==  EditState.Add)
            {
                if (m_DataManager.ADDSymbols(ID, RTF, CategoryID, Length, Memo))
                {

                    int typeRowID = gridViewSymbolType.FocusedRowHandle;
                    if (typeRowID < -1)
                        return;
                    dt_Symbol = m_DataManager.GetSymbolDetail(dt_SymbolType.Rows[typeRowID]["ID"].ToString());
                    BindSymbol();
                    m_SymbolState = EditState.View;
                    SetButState();

                }
            }
            else if (m_SymbolState ==  EditState.Edit)
            {
                string SymbolTypeID = this.txtTypeID.Text.ToString().Trim();
                string SymbolTypeName = this.txttypeName.Text.ToString().Trim();
                string SymbolTypeMemo = this.txtTypeMemo.Text.ToString().Trim();

                if (m_DataManager.EditSymbols(ID, RTF, CategoryID, Length, Memo))
                {
                    int typeRowID = gridViewSymbolType.FocusedRowHandle;
                    if (typeRowID < -1)
                        return;

                    dt_Symbol = m_DataManager.GetSymbolDetail(dt_SymbolType.Rows[typeRowID]["ID"].ToString());
                    BindSymbol();
                    m_SymbolState = EditState.View;
                    SetButState();
                }
            }

        }

        /// <summary>
        /// 特别字母取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSymbolClear_Click(object sender, EventArgs e)
        {
            int RowID = gridViewSymbol.FocusedRowHandle;

            if (RowID < -1)
                return;

            DataRowView dr = (DataRowView)gridViewSymbol.GetRow(RowID);
            if (dr != null)
            {
                BindSymbolDetail(dr);

            }
            m_SymbolState = EditState.View;
            SetButState();
        }


        /// <summary>
        /// 删除特殊符号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSymbolDel_Click(object sender, EventArgs e)
        {
            if (gridViewSymbol.FocusedRowHandle < -1)
                return;
            DataRowView dr = (DataRowView)gridViewSymbol.GetRow(gridViewSymbol.FocusedRowHandle);
            if (dr != null)
            {
                string ID = dr["ID"].ToString();
                if (m_DataManager.DelSymbols(ID))
                {
                    int typeRowID = gridViewSymbolType.FocusedRowHandle;
                    if (typeRowID < -1)
                        return;
                    dt_Symbol = m_DataManager.GetSymbolDetail(dt_SymbolType.Rows[typeRowID]["ID"].ToString());
                    BindSymbol();
                    m_SymbolState = EditState.View;
                    SetButState();
                }
            }
        }


        /// <summary>
        /// 点击gridViewSymbolType前记录下之前选中的行对应的ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewSymbolType_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (gridViewSymbolType.FocusedRowHandle < -1)
                return;
            DataRowView dr = (DataRowView)gridViewSymbolType.GetRow(gridViewSymbolType.FocusedRowHandle);
            if (dr == null)
                return;
            old_SymbolTypeID = dr["ID"].ToString();
        }

        /// <summary>
        ///  点击gridViewSymbol前记录下之前选中的行对应的ID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewSymbol_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            if (gridViewSymbol.FocusedRowHandle < -1)
                return;
            DataRowView dr = (DataRowView)gridViewSymbol.GetRow(gridViewSymbol.FocusedRowHandle);

            if (dr == null)
                return;
            old_SymbolID = dr["ID"].ToString();
        }


        private void gridViewSymbol_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.RowHandle <= -1)
                return;
            if (e.Column.FieldName != "RTF")
                return;
            RTFHelper rtfhelper = new RTFHelper();
            DataRow dr = gridViewSymbol.GetDataRow(e.RowHandle);

            string rtf = rtfhelper.GetRTFByStr(dr["RTF"].ToString());

            Image img = EmrSymbolEngine.PrintRTFImage(rtf, e.Bounds.Size);
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            if (img != null)
                e.Graphics.DrawImage(img, e.Bounds.Location);
            e.Handled = true;


        }

        /// <summary>
        /// 下标标按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSub_Click(object sender, EventArgs e)
        {
            if (this.txtRTF.SelectionLength == 0)
                return;
            int startindex = txtRTF.SelectionStart;
            int length = txtRTF.SelectionLength;
            for (int i = startindex; i < startindex + length; i++)
            {
                txtRTF.Select(i, 1);
                RtfPrintNativeMethods.SetSelectionSubscript(txtRTF, true);
            }
        }

        /// <summary>
        /// 上标按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSuper_Click(object sender, EventArgs e)
        {
            if (txtRTF.SelectionLength == 0)
                return;
            int startindex = txtRTF.SelectionStart;
            int length = txtRTF.SelectionLength;
            for (int i = startindex; i < startindex + length; i++)
            {
                txtRTF.Select(i, 1);
                RtfPrintNativeMethods.SetSelectionSuperscript(txtRTF, true);
            }
        }

        #endregion

        #region IStartPlugIn 成员

        public YidanSoft.FrameWork.IPlugIn Run(IYidanEmrHost host)
        {
            if (host == null)
                throw new ArgumentNullException("application");
            m_app = host;
            sql_Helper = m_app.SqlHelper;

            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            return plg;

        }

        #endregion



    }
}
