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
using DrectSoft.Core;
using DrectSoft.Common;

namespace DrectSoft.Emr.TemplateFactory.BaseDataMaintain
{
    public partial class UCDictionary : DevExpress.XtraEditors.XtraUserControl
    {
        SqlHelp m_SqlHelp;
        EditState m_state = EditState.None;
        EditState m_stateDetail = EditState.None;

        public IEmrHost Host
        {
            get;
            set;
        }

        public UCDictionary()
        {
            InitializeComponent();
        }

        //根据操作状态更新文本框|按钮状态
        private void FreshView()
        {
            switch (m_state)
            {
                case EditState.None:
                    this.textEditDictionaryName.Enabled = false;
                    this.textEditDictionaryName.Text = "";
                    this.simpleButtonDictionaryAdd.Enabled = true;
                    this.simpleButtonModify.Enabled = true;
                    this.simpleButtonDictionaryDelete.Enabled = true;
                    this.simpleButtonDictionarySave.Enabled = false;
                    this.simpleButtonDictionaryCancel.Enabled = false;
                    break;
                case EditState.Add:
                    this.textEditDictionaryName.Enabled = true;
                    this.textEditDictionaryName.Text = "";
                    this.simpleButtonDictionaryAdd.Enabled = false;
                    this.simpleButtonModify.Enabled = false;
                    this.simpleButtonDictionaryDelete.Enabled = false;
                    this.simpleButtonDictionarySave.Enabled = true;
                    this.simpleButtonDictionaryCancel.Enabled = true;
                    break;
                case EditState.Edit:
                    this.textEditDictionaryName.Enabled = true;
                    this.simpleButtonDictionaryAdd.Enabled = false;
                    this.simpleButtonModify.Enabled = false;
                    this.simpleButtonDictionaryDelete.Enabled = false;
                    this.simpleButtonDictionarySave.Enabled = true;
                    this.simpleButtonDictionaryCancel.Enabled = true;
                    break;
                case EditState.View:
                    this.textEditDictionaryName.Enabled = false;
                    this.simpleButtonDictionaryAdd.Enabled = true;
                    this.simpleButtonModify.Enabled = true;
                    this.simpleButtonDictionaryDelete.Enabled = true;
                    this.simpleButtonDictionarySave.Enabled = false;
                    this.simpleButtonDictionaryCancel.Enabled = false;
                    break;
            }
            switch (m_stateDetail)
            {
                case EditState.None:
                    this.textEditDictionaryDetailCode.Enabled = false;
                    this.textEditDictionaryDetailName.Enabled = false;
                    this.textEditDictionaryDetailCode.Text = "";
                    this.textEditDictionaryDetailName.Text = "";
                    this.simpleButtonAddDetail.Enabled = true;
                    this.simpleButtonModifyDetail.Enabled = true;
                    this.simpleButtonDeleteDetail.Enabled = true;
                    this.simpleButtonSaveDetail.Enabled = false;
                    this.simpleButtonCancelDetail.Enabled = false;
                    break;
                case EditState.Add:
                    this.textEditDictionaryDetailCode.Enabled = true;
                    this.textEditDictionaryDetailName.Enabled = true;
                    this.textEditDictionaryDetailCode.Text = "";
                    this.textEditDictionaryDetailName.Text = "";
                    this.simpleButtonAddDetail.Enabled = false;
                    this.simpleButtonModifyDetail.Enabled = false;
                    this.simpleButtonDeleteDetail.Enabled = false;
                    this.simpleButtonSaveDetail.Enabled = true;
                    this.simpleButtonCancelDetail.Enabled = true;
                    break;
                case EditState.Edit:
                    this.textEditDictionaryDetailCode.Enabled = true;
                    this.textEditDictionaryDetailName.Enabled = true;
                    this.simpleButtonAddDetail.Enabled = false;
                    this.simpleButtonModifyDetail.Enabled = false;
                    this.simpleButtonDeleteDetail.Enabled = false;
                    this.simpleButtonSaveDetail.Enabled = true;
                    this.simpleButtonCancelDetail.Enabled = true;
                    break;
                case EditState.View:
                    this.textEditDictionaryDetailCode.Enabled = false;
                    this.textEditDictionaryDetailName.Enabled = false;
                    this.simpleButtonAddDetail.Enabled = true;
                    this.simpleButtonModifyDetail.Enabled = true;
                    this.simpleButtonDeleteDetail.Enabled = true;
                    this.simpleButtonSaveDetail.Enabled = false;
                    this.simpleButtonCancelDetail.Enabled = false;
                    break;
            }
        }
        private void UCDictionary_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                m_SqlHelp = new SqlHelp(Host);
                RefreshDictionary();
            }
            this.FreshView();
        }

        private void gridControlDictionary_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo gridHitInfo = gridViewDictionary.CalcHitInfo(e.Location);
            if (gridHitInfo.RowHandle >= 0)
            {
                DataRowView drv = gridViewDictionary.GetRow(gridHitInfo.RowHandle) as DataRowView;
                if (drv != null)
                {
                    Dictionary dict = new Dictionary(drv);
                    textEditDictionaryName.Text = dict.Name;
                }
            }
        }

        /// <summary>
        /// 新增字典分类事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// 3、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDictionaryAdd_Click(object sender, EventArgs e)
        {
            try
            {
                m_state = EditState.Add;
                this.FreshView();
                this.textEditDictionaryName.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑字典分类事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// 3、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonModify_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDictionary.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条字典分类记录");
                    return;
                }
                DataRow dr = gridViewDictionary.GetDataRow(gridViewDictionary.FocusedRowHandle);
                if (null == dr)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                this.textEditDictionaryName.Text = null == dr["NAME"] ? "" : dr["NAME"].ToString();

                m_state = EditState.Edit;
                this.FreshView();
                this.textEditDictionaryName.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除字典分类事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDictionaryDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDictionary.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条字典分类记录");
                    return;
                }
                DataRow dr = gridViewDictionary.GetDataRow(gridViewDictionary.FocusedRowHandle);
                if (null == dr || null == dr["NAME"])
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条字典分类记录");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除字典分类 " + dr["NAME"] + " 吗？", "删除字典分类", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                Dictionary dict = GetFormDictionary();
                m_SqlHelp.DeleteDictionary(dict);
                RefreshDictionary();

                m_state = EditState.None;
                this.FreshView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存字典分类事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDictionarySave_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_state == EditState.Add)
                {
                    if (string.IsNullOrEmpty(textEditDictionaryName.Text.Trim()))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("分类名称不能为空");
                        textEditDictionaryName.Focus();
                        return;
                    }
                    Dictionary dict = GetFormDictionary();
                    DataRowView drv = gridViewDictionary.GetRow(gridViewDictionary.FocusedRowHandle) as DataRowView;
                    dict.Name = textEditDictionaryName.Text.Trim();
                    m_SqlHelp.InsertDictionary(dict);
                    RefreshDictionary();
                }
                else if (m_state == EditState.Edit)
                {
                    if (gridViewDictionary.FocusedRowHandle < 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条字典分类记录");
                        return;
                    }
                    if (string.IsNullOrEmpty(textEditDictionaryName.Text.Trim()))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("分类名称不能为空");
                        textEditDictionaryName.Focus();
                        return;
                    }
                    Dictionary dict = GetFormDictionary();
                    DataRowView drv = gridViewDictionary.GetRow(gridViewDictionary.FocusedRowHandle) as DataRowView;
                    dict.Name = textEditDictionaryName.Text.Trim();
                    m_SqlHelp.UpdateDictionary(dict);
                    RefreshDictionary();
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
        /// 取消字典分类事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDictionaryCancel_Click(object sender, EventArgs e)
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
        /// 刷新字典分类事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshDictionary()
        {
            try
            {
                DataTable dataSource = m_SqlHelp.GetDictionary();
                gridControlDictionary.DataSource = dataSource;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private Dictionary GetFormDictionary()
        {
            Dictionary dict = new Dictionary();
            dict.Name = textEditDictionaryName.Text;

            DataRowView drv = gridViewDictionary.GetRow(gridViewDictionary.FocusedRowHandle) as DataRowView;
            dict.ID = drv[Dictionary.s_ID].ToString().Trim();

            return dict;
        }

        private DictionaryDetail GetFormDictionaryDetail()
        {
            DictionaryDetail dictDetail = new DictionaryDetail();
            dictDetail.Name = textEditDictionaryDetailName.Text;
            dictDetail.CODE = textEditDictionaryDetailCode.Text;

            if (gridViewDictionaryDetail.FocusedRowHandle >= 0)
            {
                DataRowView drv = gridViewDictionaryDetail.GetRow(gridViewDictionaryDetail.FocusedRowHandle) as DataRowView;
                dictDetail.ID = drv[DictionaryDetail.s_ID].ToString().Trim();
                dictDetail.DictionaryID = drv[DictionaryDetail.s_DictionaryID].ToString().Trim();
            }
            return dictDetail;
        }

        private void gridViewDictionary_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            RefreshDictionaryDetail();
            m_state = EditState.View;
            this.FreshView();
        }

        private void gridViewDictionaryDetail_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gridViewDictionaryDetailFocusedRowChanged();
        }

        private void gridViewDictionaryDetailFocusedRowChanged()
        {
            if (gridViewDictionaryDetail.FocusedRowHandle >= 0)
            {
                DataRowView drv = gridViewDictionaryDetail.GetRow(gridViewDictionaryDetail.FocusedRowHandle) as DataRowView;
                if (drv != null)
                {
                    DictionaryDetail dict = new DictionaryDetail(drv);
                    textEditDictionaryDetailName.Text = dict.Name;
                    textEditDictionaryDetailCode.Text = dict.CODE;
                }
            }
            m_stateDetail = EditState.View;
            this.FreshView();
        }

        /// <summary>
        /// 新增字典明细事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonAddDetail_Click(object sender, EventArgs e)
        {
            try
            {
                m_stateDetail = EditState.Add;
                this.FreshView();
                this.textEditDictionaryDetailName.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑字典明细事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// 3、添加焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonModifyDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDictionaryDetail.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条字典记录");
                    return;
                }
                DataRow dr = gridViewDictionaryDetail.GetDataRow(gridViewDictionaryDetail.FocusedRowHandle);
                if (null == dr)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条记录");
                    return;
                }
                this.textEditDictionaryDetailName.Text = null == dr["NAME"] ? "" : dr["NAME"].ToString();
                this.textEditDictionaryDetailCode.Text = null == dr["CODE"] ? "" : dr["CODE"].ToString();

                m_stateDetail = EditState.Edit;
                this.FreshView();
                this.textEditDictionaryDetailName.Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除字典明细事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDeleteDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewDictionary.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条字典分类记录");
                    return;
                }
                else if (gridViewDictionaryDetail.FocusedRowHandle < 0)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条字典记录");
                    return;
                }
                DataRow dr = gridViewDictionaryDetail.GetDataRow(gridViewDictionaryDetail.FocusedRowHandle);
                if (null == dr || null == dr["NAME"])
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条字典记录");
                    return;
                }

                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要删除字典 " + dr["NAME"] + " 吗？", "删除字典", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }

                DictionaryDetail dictDetail = GetFormDictionaryDetail();
                m_SqlHelp.DeleteDictionaryDetail(dictDetail);
                RefreshDictionaryDetail();

                m_stateDetail = EditState.None;
                this.FreshView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存字典明细事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// 1、add try ... catch
        /// 2、添加提示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSaveDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_stateDetail == EditState.Add)
                {
                    if (gridViewDictionary.FocusedRowHandle < 0)
                    {
                        Host.CustomMessageBox.MessageShow("请选择一条分类记录", Core.CustomMessageBoxKind.InformationOk);
                        return;
                    }
                    else if (textEditDictionaryDetailName.Text.Trim() == "")
                    {
                        Host.CustomMessageBox.MessageShow("分类明细的名称不能为空", Core.CustomMessageBoxKind.InformationOk);
                        textEditDictionaryDetailName.Focus();
                        return;
                    }

                    DictionaryDetail dictDetail = GetFormDictionaryDetail();
                    dictDetail.Name = textEditDictionaryDetailName.Text.Trim();
                    dictDetail.CODE = textEditDictionaryDetailCode.Text.Trim();
                    Dictionary dict = GetFormDictionary();
                    dictDetail.DictionaryID = dict.ID;

                    m_SqlHelp.InsertDictionaryDetail(dictDetail);
                    RefreshDictionaryDetail();
                }
                else if (m_stateDetail == EditState.Edit)
                {
                    if (gridViewDictionary.FocusedRowHandle < 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条分类记录");
                        return;
                    }
                    else if (gridViewDictionaryDetail.FocusedRowHandle < 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条字典记录");
                        return;
                    }
                    else if (string.IsNullOrEmpty(textEditDictionaryDetailName.Text.Trim()))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("字典名称不能为空");
                        textEditDictionaryDetailName.Focus();
                        return;
                    }

                    DictionaryDetail dictDetail = GetFormDictionaryDetail();
                    dictDetail.Name = textEditDictionaryDetailName.Text.Trim();
                    dictDetail.CODE = textEditDictionaryDetailCode.Text.Trim();
                    Dictionary dict = GetFormDictionary();
                    dictDetail.DictionaryID = dict.ID;

                    m_SqlHelp.UpdateDictionaryDetail(dictDetail);
                    RefreshDictionaryDetail();
                }
                m_stateDetail = EditState.None;
                this.FreshView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消字典明细事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonCancelDetail_Click(object sender, EventArgs e)
        {
            try
            {
                m_stateDetail = EditState.None;
                this.FreshView();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 字典刷新事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshDictionaryDetail()
        {
            try
            {
                DataRowView drv = gridViewDictionary.GetRow(gridViewDictionary.FocusedRowHandle) as DataRowView;
                if (drv != null)
                {
                    Dictionary dict = new Dictionary(drv);
                    DataTable dataSource = m_SqlHelp.GetDictionaryDetail(dict.ID);
                    gridControlDictionaryDetail.DataSource = dataSource;
                    gridViewDictionaryDetailFocusedRowChanged();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdatePYWB()
        {
            GetPYWB();
        }

        /// <summary>
        /// 得到拼音和五笔
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private void GetPYWB()
        {
            //IDataAccess m_DataAccess = DataAccessFactory.GetSqlDataAccess();
            //GenerateShortCode shortCode = new GenerateShortCode(m_DataAccess);

            //DataTable dt = m_DataAccess.ExecuteDataTable("select * from diagnosis");
            //foreach (DataRow dr in dt.Rows)
            //{
            //    string id = dr["icd"].ToString();
            //    string name = dr["name"].ToString();
            //    string[] code = shortCode.GenerateStringShortCode(name);

            //    string py = code[0]; //PY
            //    string wb = code[1]; //WB

            //    m_DataAccess.ExecuteNoneQuery(string.Format("update diagnosis set py ='{0}', wb = '{1}' where icd = '{2}'", py, wb, id));
            //}
        }

        /// <summary>
        /// 序号 --- 字典
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDictionaryDetail_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        /// <summary>
        /// 序号 --- 分类
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-06</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDictionary_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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
