using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common;

namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    public partial class UCMacro : DevExpress.XtraEditors.XtraUserControl
    {
        SqlHelp m_SqlHelp;
        EditState m_state = EditState.None;
        List<string> m_ListMacro = new List<string>();
        public IEmrHost Host
        {
            get;
            set;
        }

        public UCMacro()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 根据操作状态更新文本框|按钮状态
        /// edit by Yanqiao.Cai 2012-11-16
        /// add try ... catch
        /// </summary>
        private void FreshView()
        {
            try
            {
                switch (m_state)
                {
                    case EditState.None:
                        this.textEditName.Enabled = false;
                        this.textEditTable.Enabled = false;
                        this.textEditExample.Enabled = false;
                        this.textEditColumn.Enabled = false;
                        this.memoEditSql.Enabled = false;
                        this.textEditName.Text = "";
                        this.textEditTable.Text = "";
                        this.textEditExample.Text = "";
                        this.textEditColumn.Text = "";
                        this.memoEditSql.Text = "";
                        this.simpleButtonAdd.Enabled = true;
                        this.simpleButtonModify.Enabled = true;
                        this.simpleButtonDelete.Enabled = true;
                        this.simpleButtonSave.Enabled = false;
                        this.btn_reset.Enabled = false;
                        this.simpleButtonCancel.Enabled = false;
                        break;
                    case EditState.Add:
                        this.textEditName.Enabled = true;
                        this.textEditTable.Enabled = true;
                        this.textEditExample.Enabled = true;
                        this.textEditColumn.Enabled = true;
                        this.memoEditSql.Enabled = true;
                        this.textEditName.Text = "";
                        this.textEditTable.Text = "";
                        this.textEditExample.Text = "";
                        this.textEditColumn.Text = "";
                        this.memoEditSql.Text = "";
                        this.simpleButtonAdd.Enabled = false;
                        this.simpleButtonModify.Enabled = false;
                        this.simpleButtonDelete.Enabled = false;
                        this.simpleButtonSave.Enabled = true;
                        this.btn_reset.Enabled = true;
                        this.simpleButtonCancel.Enabled = true;
                        break;
                    case EditState.Edit:
                        this.textEditName.Enabled = true;
                        this.textEditTable.Enabled = true;
                        this.textEditExample.Enabled = true;
                        this.textEditColumn.Enabled = true;
                        this.memoEditSql.Enabled = true;
                        this.simpleButtonAdd.Enabled = false;
                        this.simpleButtonModify.Enabled = false;
                        this.simpleButtonDelete.Enabled = false;
                        this.simpleButtonSave.Enabled = true;
                        this.btn_reset.Enabled = true;
                        this.simpleButtonCancel.Enabled = true;
                        break;
                    case EditState.View:
                        this.textEditName.Enabled = false;
                        this.textEditTable.Enabled = false;
                        this.textEditExample.Enabled = false;
                        this.textEditColumn.Enabled = false;
                        this.memoEditSql.Enabled = false;
                        this.simpleButtonAdd.Enabled = true;
                        this.simpleButtonModify.Enabled = true;
                        this.simpleButtonDelete.Enabled = true;
                        this.simpleButtonSave.Enabled = false;
                        this.btn_reset.Enabled = false;
                        this.simpleButtonCancel.Enabled = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UCMacro_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                m_SqlHelp = new SqlHelp(Host);
                RefreshMacro();
            }
            this.FreshView();
            this.simpleButtonAdd.Focus();
        }

        /// <summary>
        /// 刷新数据事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        private void RefreshMacro()
        {
            try
            {
                DataTable dataSource = m_SqlHelp.GetMacro();
                m_ListMacro.Clear();
                foreach (DataRow dr in dataSource.Rows)
                {
                    m_ListMacro.Add(dr["D_NAME"].ToString());
                }
                gridControlMacro.DataSource = dataSource;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridViewMacro_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private Macro GetFormMacro()
        {
            Macro macro = new Macro();
            macro.Name = textEditName.Text;
            macro.Example = textEditExample.Text;
            macro.Column = textEditColumn.Text;
            macro.Table = textEditTable.Text;
            macro.Sql = memoEditSql.Text.Replace("'", "''");
            return macro;
        }

        /// <summary>
        /// 新增事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonNew_Click(object sender, EventArgs e)
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
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewMacro.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dr = gridViewMacro.GetDataRow(gridViewMacro.FocusedRowHandle);
                if (null == dr)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                SetTextValue(dr);

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
        /// 设置编辑项文本值
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        private void SetTextValue(DataRow dr)
        {
            try
            {
                if (null != dr && dr.ItemArray.Length >= 5)
                {
                    this.textEditName.Text = null == dr[0] ? "" : dr[0].ToString();
                    this.textEditExample.Text = null == dr[1] ? "" : dr[1].ToString();
                    this.textEditColumn.Text = null == dr[2] ? "" : dr[2].ToString();
                    this.textEditTable.Text = null == dr[3] ? "" : dr[3].ToString();
                    this.memoEditSql.Text = null == dr[4] ? "" : dr[4].ToString();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewMacro.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                DataRow dr = gridViewMacro.GetDataRow(gridViewMacro.FocusedRowHandle);
                if (null == dr)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除常用词 " + dr[0] + " 吗？", "删除常用词", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                Macro macro = GetFormMacro();
                m_SqlHelp.DeleteMacro(macro);
                RefreshMacro();
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
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_state == EditState.Add)
                {
                    if (string.IsNullOrEmpty(textEditName.Text.Trim()))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("名称不能为空");
                        textEditName.Focus();
                        return;
                    }
                    Macro macro = GetFormMacro();
                    if (m_ListMacro.Contains(macro.Name))
                    {
                        m_SqlHelp.UpdateMacro(macro);
                    }
                    else
                    {
                        m_SqlHelp.InsertMacro(macro);
                    }
                    RefreshMacro();
                }
                else if (m_state == EditState.Edit)
                {
                    if (string.IsNullOrEmpty(textEditName.Text.Trim()))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("名称不能为空");
                        textEditName.Focus();
                        return;
                    }
                    Macro macro = GetFormMacro();
                    m_SqlHelp.UpdateMacro(macro);
                    RefreshMacro();
                }
                m_state = EditState.None;
                this.FreshView();
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
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                m_state = EditState.None;
                this.FreshView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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
                if (m_state == EditState.Add || m_state == EditState.Edit)
                {
                    this.textEditName.Text = string.Empty;
                    this.textEditExample.Text = string.Empty;
                    this.textEditColumn.Text = string.Empty;
                    this.textEditTable.Text = string.Empty;
                    this.memoEditSql.Text = string.Empty;
                    this.textEditName.Focus();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void gridViewMacro_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gridViewMacro.FocusedRowHandle >= 0)
            {
                DataRowView drv = gridViewMacro.GetRow(gridViewMacro.FocusedRowHandle) as DataRowView;
                if (drv != null)
                {
                    Macro macro = new Macro(drv);
                    textEditName.Text = macro.Name;
                    textEditExample.Text = macro.Example;
                    textEditColumn.Text = macro.Column;
                    textEditTable.Text = macro.Table;
                    memoEditSql.Text = macro.Sql;
                }
            }
            m_state = EditState.View;
            this.FreshView();
        }

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMacro_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
