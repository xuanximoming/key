using CommonTableConfig;
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
    /// <summary>
    /// 单据维护界面
    /// </summary>
    public partial class CommonNoteConfig : DevBaseForm, IStartPlugIn
    {
        private IEmrHost m_app;
        CommonNoteBiz commonNoteBiz;
        CommonNoteInfo uCCommonNoteInfo;

        public CommonNoteConfig()
        {
            InitializeComponent();
        }

        public CommonNoteConfig(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                CommonTabHelper.DataElementListAll = null;
                m_app = app;
                InitData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //初始化信息
        private void InitData()
        {
            try
            {
                if (commonNoteBiz == null)
                {
                    commonNoteBiz = new CommonNoteBiz(m_app);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost application)
        {
            try
            {

                if (application == null)
                {
                    throw new ArgumentNullException("application");
                }
                CommonNoteConfig commonNoteConfig = new CommonNoteConfig(application);
                PlugIn plg = new PlugIn(this.GetType().ToString(), commonNoteConfig);
                return plg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                CommonNoteEntity commonNoteEntity = new CommonTableConfig.CommonNoteEntity();
                AddTabPage(commonNoteEntity);
            }
            catch (Exception ex)
            {
                //m_app.CustomMessageBox.MessageShow(ex.Message);
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        //添加通用单标签页 包括新增和修改
        private void AddTabPage(CommonNoteEntity commonNoteEntity)
        {
            try
            {
                uCCommonNoteInfo = new CommonTableConfig.CommonNoteInfo(commonNoteEntity, m_app);
                uCCommonNoteInfo.btnSave.Click += new EventHandler(btnSave_Click);
                uCCommonNoteInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //保存
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                string message = "";
                bool validateResult = uCCommonNoteInfo.ValidateCommonNote();
                if (validateResult == false)
                {
                    return;
                }
                CommonNoteEntity commonNoteEntityNew = uCCommonNoteInfo.SaveCommonNote();
                bool saveResult = commonNoteBiz.SaveCommonNoteAll(commonNoteEntityNew, ref message);
                CommonNoteCountEntity commonNoteCountEntity = uCCommonNoteInfo.SaveCommonCount();
                commonNoteCountEntity.CommonNoteFlow = commonNoteEntityNew.CommonNoteFlow;
                CommonNoteBiz.AddOrModCommonNoteEntity(commonNoteCountEntity);
                if (saveResult)
                {
                    m_app.CustomMessageBox.MessageShow("保存成功");


                    //从数据库中取对象 为了确保对象数据的完整
                    commonNoteEntityNew = commonNoteBiz.GetDetailCommonNote(commonNoteEntityNew.CommonNoteFlow);
                    Search();
                    //RefreshTabpage(commonNoteEntityNew);
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow(message);
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                CommonNoteEntity commonNoteEntity = gvDataCommon.GetFocusedRow() as CommonNoteEntity;
                if (commonNoteEntity == null)
                {
                    return;
                    //Common.Ctrs.DLG.MessageBox.Show("没有选中要删除的行");
                    //throw new Exception("没有选中要删除的行");
                }
                DialogResult diaResult = m_app.CustomMessageBox.MessageShow("确定要删除吗？", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo);
                if (diaResult == DialogResult.No)
                {
                    return;
                }
                string message = "";
                bool result = commonNoteBiz.DelCommonNote(commonNoteEntity, ref message);
                if (result)
                {
                    m_app.CustomMessageBox.MessageShow("删除成功！");
                    Search();
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow(message);
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        private void Search()
        {
            try
            {
                string commonNoteName = "";
                if (txtCommonNoteName.Text.Trim() != "")
                {
                    commonNoteName = txtCommonNoteName.Text.Trim();
                }
                List<CommonNoteEntity> commonNoteEntityList = commonNoteBiz.GetSimpleCommonNote(commonNoteName);
                for (int i = 0; i < commonNoteEntityList.Count; i++)
                {
                    commonNoteEntityList[i].CreateDateTime = DateUtil.getDateTime(commonNoteEntityList[i].CreateDateTime, DateUtil.NORMAL_LONG);
                }
                gcDataCommon.DataSource = commonNoteEntityList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加数据库存在的tab
        /// </summary>
        /// <param name="commonNoteEntity"></param>
        private void RefreshTabpage(CommonNoteEntity commonNoteEntity)
        {
            AddTabPage(commonNoteEntity);
        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            //try
            //{
            //    xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
            //}
            //catch (Exception ex)
            //{
            //    m_app.CustomMessageBox.MessageShow(ex.Message);
            //}
        }

        /// <summary>
        /// 编辑事件
        /// xlb
        /// 2013-01-16
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                CommonNoteEntity commonNoteEntity = gvDataCommon.GetFocusedRow() as CommonNoteEntity;
                if (commonNoteEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有选中要编辑的项目");
                    return;
                    //throw new Exception("没有选中要编辑的项目");
                }
                RefreshTabpage(commonNoteEntity);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 焦点改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDataCommon_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
        }


        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            //if (xtraTabControl1.SelectedTabPage == tabpagePrint && tabpagePrint.Controls.Count == 0)
            //{
            //    LoadPrint loadPrint = new LoadPrint(m_app);
            //    loadPrint.Dock = DockStyle.Fill;
            //    loadPrint.TopLevel = false;
            //    loadPrint.Show();
            //    loadPrint.FormBorderStyle = FormBorderStyle.None;
            //    tabpagePrint.Controls.Add(loadPrint);

            //}
        }

        /// <summary>
        /// 加序号列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDataCommon_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
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

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                CommonNoteEntity commonNoteEntity = gvDataCommon.GetFocusedRow() as CommonNoteEntity;
                if (commonNoteEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有选中记录");
                    return;
                }
                CommonNoteEntity commonNoteNew = new CommonNoteEntity();
                commonNoteNew = commonNoteBiz.GetDetailCommonNote(commonNoteEntity.CommonNoteFlow);
                commonNoteNew.CommonNoteFlow = "";
                commonNoteNew.CreateDateTime = "";
                commonNoteNew.CreateDoctorID = "";
                commonNoteNew.CreateDoctorName = "";
                if (commonNoteNew.CommonNote_TabList != null)
                {
                    foreach (var tabitem in commonNoteNew.CommonNote_TabList)
                    {
                        tabitem.CommonNoteFlow = "";
                        tabitem.CommonNote_Tab_Flow = "";
                        tabitem.CreateDateTime = "";
                        tabitem.CreateDoctorID = "";
                        tabitem.CreateDoctorName = "";
                        foreach (var item in tabitem.CommonNote_ItemList)
                        {
                            item.CommonNoteFlow = "";
                            item.CommonNote_Tab_Flow = "";
                            item.CommonNote_Item_Flow = "";
                            item.CreateDateTime = "";
                            item.CreateDoctorID = "";
                            item.CreateDoctorName = "";
                        }
                    }
                }
                string message = "";
                bool saveResult = commonNoteBiz.SaveCommonNoteAll(commonNoteNew, ref message);
                if (saveResult)
                {
                    Search();
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("保存成功");
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(message);
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvDataCommon_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                CommonNoteEntity commonNoteEntity = gvDataCommon.GetFocusedRow() as CommonNoteEntity;
                if (commonNoteEntity == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("没有选中要编辑的项目");
                    return;
                    //throw new Exception("没有选中要编辑的项目");
                }
                RefreshTabpage(commonNoteEntity);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 模板维护事件
        /// by xlb 2012-12-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModelWeiHu_Click(object sender, EventArgs e)
        {
            try
            {
                CommonNote_ModelWeiHu uCCommonNote_Model = new CommonNote_ModelWeiHu(m_app);
                if (uCCommonNote_Model == null)
                {
                    return;
                }
                //打开的窗体在维护界面上方
                uCCommonNote_Model.TopMost = true;
                uCCommonNote_Model.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}