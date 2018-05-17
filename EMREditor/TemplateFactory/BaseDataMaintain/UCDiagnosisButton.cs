using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common;

namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    public partial class UCDiagnosisButton : DevExpress.XtraEditors.XtraUserControl
    {
        SqlHelp m_SqlHelp;
        EditState m_state = EditState.None;
        private IEmrHost m_app;

        public UCDiagnosisButton(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
        }

        //根据操作状态更新文本框|按钮状态
        private void FreshView()
        {
            switch (m_state)
            {
                case EditState.None:
                    this.textEditName.Enabled = false;
                    this.textEditName.Text = "";
                    this.simpleButtonAdd.Enabled = true;
                    this.simpleButtonEdit.Enabled = true;
                    this.simpleButtonDelete.Enabled = true;
                    this.simpleButtonSave.Enabled = false;
                    this.simpleButtonCancel.Enabled = false;
                    break;
                case EditState.Add:
                    this.textEditName.Enabled = true;
                    this.textEditName.Text = "";
                    this.simpleButtonAdd.Enabled = false;
                    this.simpleButtonEdit.Enabled = false;
                    this.simpleButtonDelete.Enabled = false;
                    this.simpleButtonSave.Enabled = true;
                    this.simpleButtonCancel.Enabled = true;
                    break;
                case EditState.Edit:
                    this.textEditName.Enabled = true;
                    this.simpleButtonAdd.Enabled = false;
                    this.simpleButtonEdit.Enabled = false;
                    this.simpleButtonDelete.Enabled = false;
                    this.simpleButtonSave.Enabled = true;
                    this.simpleButtonCancel.Enabled = true;
                    break;
                case EditState.View:
                    this.textEditName.Enabled = false;
                    this.simpleButtonAdd.Enabled = true;
                    this.simpleButtonEdit.Enabled = true;
                    this.simpleButtonDelete.Enabled = true;
                    this.simpleButtonSave.Enabled = false;
                    this.simpleButtonCancel.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、初始化焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCDiagnosisButton_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                m_SqlHelp = new SqlHelp(m_app);
                RefreshDiagButton();
            }
            this.FreshView();
            this.simpleButtonAdd.Focus();
        }

        private void RefreshDiagButton()
        {
            DataTable dt = m_SqlHelp.GetDiagButton();
            gridControlDiag.DataSource = dt;
        }

        private void gridViewDiag_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            FocusedRow();
        }

        private void FocusedRow()
        {
            if (gridViewDiag.FocusedRowHandle >= 0)
            {
                DataRow dr = gridViewDiag.GetFocusedDataRow();
                if (dr != null)
                {
                    textEditName.Text = dr["DIAGNAME"].ToString();
                }
            }
        }

        /// <summary>
        /// 新增事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、初始化焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                m_state = EditState.Add;
                this.FreshView();
                this.textEditName.Focus();
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
        /// 2、初始化焦点
        /// 3、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDiag.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dr = gridViewDiag.GetDataRow(gridViewDiag.FocusedRowHandle);
                if (null == dr || dr.ItemArray.Length <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                this.textEditName.Text = null == dr["DIAGNAME"] ? "" : dr["DIAGNAME"].ToString();

                m_state = EditState.Edit;
                this.FreshView();
                this.textEditName.Focus();
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
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDiag.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dr = gridViewDiag.GetDataRow(gridViewDiag.FocusedRowHandle);
                if (null == dr || dr.ItemArray.Length <= 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除按钮 " + dr["DIAGNAME"] + " 吗？", "删除按钮", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                string code = dr["CODE"].ToString();
                string diagname = dr["DIAGNAME"].ToString();
                m_SqlHelp.DeleteDiagButton(code, diagname);
                textEditName.Text = "";
                RefreshDiagButton();
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");

                m_state = EditState.None;
                this.FreshView();
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
        /// 2、初始化焦点
        /// 3、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.textEditName.Text.Trim()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("按钮名称不能为空");
                    this.textEditName.Focus();
                    return;
                }
                if (m_state == EditState.Add)
                {
                    string diagname = textEditName.Text.Trim();
                    if (!string.IsNullOrEmpty(diagname))
                    {
                        m_SqlHelp.SaveDiagButton(diagname);
                        RefreshDiagButton();
                        FocusedRowByDiagName(diagname);

                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("新增成功");
                    }
                }
                else if (m_state == EditState.Edit)
                {
                    DataRow dr = gridViewDiag.GetFocusedDataRow();
                    if (dr != null)
                    {
                        int code = int.Parse(dr["CODE"].ToString());
                        string newdiagname = textEditName.Text.Trim();
                        m_SqlHelp.EditDiagButton(code, newdiagname);

                        RefreshDiagButton();
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("修改成功");
                    }
                }
                m_state = EditState.None;
                this.FreshView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        //// <summary>
        /// 取消事件
        /// edit by Yanqiao.Cai 2012-11-07
        /// 1、add try ... catch
        /// 2、初始化焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                m_state = EditState.None;
                this.FreshView();
                this.simpleButtonAdd.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void FocusedRowByDiagName(string diagname)
        {
            for (int i = 0; i < gridViewDiag.RowCount; i++)
            {
                DataRowView drv = gridViewDiag.GetRow(i) as DataRowView;
                if (drv["diagname"].ToString() == diagname)
                {
                    gridViewDiag.FocusedRowHandle = i;
                }
            }
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-07</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDiag_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
    }
}
