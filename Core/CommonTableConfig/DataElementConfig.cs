using DevExpress.XtraTab;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DrectSoft.Core.CommonTableConfig
{
    public partial class DataElementConfig : DevBaseForm, IStartPlugIn
    {

        private IEmrHost m_app;
        DataElemntBiz dataElemntBiz;
        DataElementInfo uCDataElementInfo;
        public DataElementConfig()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="app"></param>
        public DataElementConfig(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_app = app;
                InitData();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //初始化基础数据
        private void InitData()
        {
            try
            {
                List<BaseDictory> baseClassList = CommonTabHelper.GetAllDataElemnetClass();
                baseClassList.Insert(0, new BaseDictory());
                cboClass.DataSource = baseClassList;
                cboClass.DisplayMember = "Name";
                if (dataElemntBiz == null)
                    dataElemntBiz = new DataElemntBiz(m_app);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost application)
        {
            try
            {
                if (application == null)
                    throw new ArgumentNullException("application");

                DataElementConfig imageManager = new DataElementConfig(application);
                PlugIn plg = new PlugIn(this.GetType().ToString(), imageManager);
                return plg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //根据条件查找数据元
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchDateElement();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        //数据元搜索
        private void SearchDateElement()
        {
            try
            {
                DataElementEntity dataElementEntitySearch = new CommonTableConfig.DataElementEntity();
                dataElementEntitySearch.ElementId = txtElementId.Text.Trim();
                dataElementEntitySearch.ElementName = txtElementName.Text.Trim();
                dataElementEntitySearch.ElementPYM = txtPYM.Text.Trim();
                BaseDictory baseDictory = cboClass.SelectedItem as BaseDictory;
                if (baseDictory == null || baseDictory.Id == null)
                    dataElementEntitySearch.ElementClass = "";
                else
                    dataElementEntitySearch.ElementClass = baseDictory.Id;
                List<DataElementEntity> dataElementEntityList = dataElemntBiz.GetDataElement(dataElementEntitySearch);
                gcdDataElement.DataSource = dataElementEntityList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddTabControl(new DataElementEntity());
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        //打开一个数据元详情标签 包括修改和新增
        private void AddTabControl(DataElementEntity dataElementEntity)
        {
            try
            {
                XtraTabPage tabpage = new XtraTabPage();
                uCDataElementInfo = new DataElementInfo(dataElementEntity, m_app);
                uCDataElementInfo.btnSave.Click += new EventHandler(btnSave_Click);
                tabpage.Tag = dataElementEntity;
                if (string.IsNullOrEmpty(dataElementEntity.ElementFlow))
                {
                    uCDataElementInfo.Text = "新增数据元";
                }
                else
                {
                    uCDataElementInfo.Text = "编辑" + dataElementEntity.ElementName;
                }
                uCDataElementInfo.ShowDialog();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //保存按钮触发  保存信息
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (uCDataElementInfo == null) return;
                bool result = uCDataElementInfo.SaveDataElement();
                if (result)
                {
                    uCDataElementInfo.Text = uCDataElementInfo.m_dataElementEntity.ElementName;
                    SearchDateElement();
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        //删除选中的信息
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                DataElementEntity dataElementEntity = gvDataElement.GetFocusedRow() as DataElementEntity;
                if (dataElementEntity == null) return;

                DialogResult dialogResult = m_app.CustomMessageBox.MessageShow("确定要删除吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string message = "";
                    bool result = dataElemntBiz.DelDataElement(dataElementEntity, ref message);
                    if (result)
                    {
                        m_app.CustomMessageBox.MessageShow("删除成功！");
                        SearchDateElement();
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow(message);
                    }
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtElementId.Text = "";
                txtElementName.Text = "";
                txtPYM.Text = "";
                cboClass.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 数据元项双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcdDataElement_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                EditDataElement();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// 编辑项目
        /// </summary>
        private void EditDataElement()
        {
            try
            {
                DataElementEntity dataElementEntity = gvDataElement.GetFocusedRow() as DataElementEntity;
                int editindex = gvDataElement.FocusedRowHandle;
                if (dataElementEntity == null)
                {
                    //m_app.CustomMessageBox.MessageShow("未选中要编辑的项目");
                    //return;
                    throw new Exception("未选中要编辑的项目");
                }
                DataElementEntity dataElementEntityClone = dataElementEntity.Clone() as DataElementEntity;
                dataElemntBiz.ConvertDataElemnetClass(dataElementEntityClone, 0);
                dataElemntBiz.ConvertIsDataElementOne(dataElementEntityClone, 0);
                if (dataElementEntityClone == null) return;
                AddTabControl(dataElementEntityClone);
                gvDataElement.MoveBy(editindex);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                EditDataElement();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// add by xlb 2012-12-29
        /// 序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDataElement_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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