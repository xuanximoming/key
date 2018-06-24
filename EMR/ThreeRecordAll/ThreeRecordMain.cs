using DevExpress.XtraBars;
using DevExpress.XtraTab;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.CommonTableConfig;
using DrectSoft.Core.CommonTableConfig.JLDZT;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DrectSoft.EMR.ThreeRecordAll
{
    /// <summary>	
    /// <title>功能描述</title>
    /// <auth>xuliangliang</auth>
    /// <date></date>
    /// </summary>
    public partial class ThreeRecordMain : DevBaseForm, IStartPlugIn
    {
        IEmrHost m_app;

        private static string SCDZT = "SCDZT"; //三测单整体
        private static string SCDPL = "SCDPL"; //三测单批量
        List<CommonNoteEntity> commonNoteEntityListPL;  //可以批量录入的护理单
        public ThreeRecordMain()
        {
            InitializeComponent();
        }

        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost application)
        {
            try
            {
                if (application == null)
                    throw new ArgumentNullException("application");
                m_app = application;
                PlugIn plg = new PlugIn(this.GetType().ToString(), this);
                return plg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 护理数据录入界面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreeRecordMain_Load(object sender, EventArgs e)
        {
            //对于批量录入功能的显示处理
            try
            {
                SetPLVisible();
                SetNurseJLD();
                SetNurseJLDPL();

                if (btnSCDOPL.Visibility == BarItemVisibility.Always)
                {
                    btnSCDPL_ItemClick(null, null);
                }
                else if (btnSCDSin.Visibility == BarItemVisibility.Always)
                {
                    btnSCDSing_ItemClick(null, null);
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 初始化三测单快速录入是否可用
        /// </summary>
        private void SetPLVisible()
        {
            try
            {
                string strValue = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("ShowMultiRecordsInput");
                if (strValue == "0")
                {
                    btnSCDOPL.Visibility = BarItemVisibility.Never;
                }
                else
                {
                    btnSCDOPL.Visibility = BarItemVisibility.Always;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// edit by xlb 2013-01-28
        /// 改用新的记录单整体录入
        /// 隐藏该功能
        /// xll 2013-06-04 修改该 如果没有单据 就不启用该功能
        /// </summary>
        private void SetNurseJLD()
        {
            try
            {
                CommonNoteBiz commonNoteBiz = new CommonNoteBiz(m_app);
                List<CommonNoteEntity> commonNoteEntityList = commonNoteBiz.GetCommonNoteByDeptWard(m_app.User.CurrentWardId, "02");

                if (commonNoteEntityList == null || commonNoteEntityList.Count == 0)
                {
                    btnHLDSing.Visibility = BarItemVisibility.Never;
                }
                else
                {
                    btnHLDSing.Visibility = BarItemVisibility.Always;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 记录单快速录入
        /// </summary>
        private void SetNurseJLDPL()
        {
            try
            {
                CommonNoteBiz commonNoteBiz = new CommonNoteBiz(m_app);
                List<CommonNoteEntity> commonNoteEntityList = commonNoteBiz.GetCommonNoteByDeptWard(m_app.User.CurrentWardId, "02");
                if (commonNoteEntityList == null) return;
                //只显示批量的
                commonNoteEntityListPL = commonNoteEntityList.FindAll(a => a.UsingFlag == "1") as List<CommonNoteEntity>;
                if (commonNoteEntityListPL == null || commonNoteEntityListPL.Count == 0)
                {
                    btnHLDPL.Visibility = BarItemVisibility.Never;
                }
                else
                {
                    btnHLDPL.Visibility = BarItemVisibility.Always;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        /// <summary>
        /// 新的记录单整体录入
        /// add xlb 2013-01-28
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJLDSing_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                XtraTabPage xtraTabPage = HasTabPage("PL记录单整体录入");
                if (xtraTabPage == null)
                {
                    xtraTabPage = new XtraTabPage();
                    xtraTabPage.Name = "PL记录单整体录入";
                    xtraTabPage.Text = "护理单单人录入";
                    NurseJLDForm nurseJLDForm = new ThreeRecordAll.NurseJLDForm(m_app);
                    nurseJLDForm.TopLevel = false;
                    nurseJLDForm.FormBorderStyle = FormBorderStyle.None;
                    nurseJLDForm.Dock = DockStyle.Fill;
                    nurseJLDForm.Show();
                    xtraTabPage.Controls.Add(nurseJLDForm);
                    tabControlSCD.TabPages.Add(xtraTabPage);
                }
                tabControlSCD.SelectedTabPage = xtraTabPage;

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }



        /// <summary>
        /// 护理单批量录入 对于单个单据 暂不用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void barButtonItem_ItemClickKS(object sender, ItemClickEventArgs e)
        {
            try
            {
                BarButtonItem barButtonItem = (e.Item as BarButtonItem);
                if (barButtonItem == null
                    || barButtonItem.Tag == null
                    || !(barButtonItem.Tag is CommonNoteEntity))
                {
                    return;
                }
                CommonNoteEntity commonNoteEntity = barButtonItem.Tag as CommonNoteEntity;
                //commonNoteEntity = OpenHLDPL(commonNoteEntity);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 打开选中单据的批量录入
        /// </summary>
        /// <param name="commonNoteEntity"></param>
        private void OpenHLDPL(CommonNoteEntity commonNoteEntity)
        {
            try
            {
                if (commonNoteEntity == null)
                {
                    return;
                }
                XtraTabPage xtraTabPage = HasTabPage("KS" + commonNoteEntity.CommonNoteFlow);
                if (xtraTabPage == null)
                {
                    xtraTabPage = new XtraTabPage();
                    xtraTabPage.Name = "KS" + commonNoteEntity.CommonNoteFlow;
                    xtraTabPage.Text = commonNoteEntity.CommonNoteName + "多人录入";
                    if (commonNoteEntity.CommonNote_TabList == null)
                    {
                        CommonNoteBiz commonNoteBiz = new CommonNoteBiz(m_app);
                        commonNoteEntity = commonNoteBiz.GetDetailCommonNote(commonNoteEntity.CommonNoteFlow);
                    }
                    DayListForm dayListForm = new DayListForm(commonNoteEntity, m_app);
                    dayListForm.TopLevel = false;
                    dayListForm.FormBorderStyle = FormBorderStyle.None;
                    dayListForm.Dock = DockStyle.Fill;
                    dayListForm.Show();
                    xtraTabPage.Controls.Add(dayListForm);
                    tabControlSCD.TabPages.Add(xtraTabPage);
                }
                tabControlSCD.SelectedTabPage = xtraTabPage;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        /// <summary>
        /// 记录单整体录入单项点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void barButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                BarButtonItem barButtonItem = (e.Item as BarButtonItem);
                if (barButtonItem == null
                    || barButtonItem.Tag == null
                    || !(barButtonItem.Tag is CommonNoteEntity)) { return; }
                CommonNoteEntity commonNoteEntity = barButtonItem.Tag as CommonNoteEntity;
                XtraTabPage xtraTabPage = HasTabPage("PL" + commonNoteEntity.CommonNoteFlow);
                if (xtraTabPage == null)
                {
                    xtraTabPage = new XtraTabPage();
                    xtraTabPage.Name = "PL" + commonNoteEntity.CommonNoteFlow;
                    xtraTabPage.Text = commonNoteEntity.CommonNoteName + "多人录入";
                    NurseJLDForm nurseJLDForm = new ThreeRecordAll.NurseJLDForm(m_app, commonNoteEntity);
                    nurseJLDForm.TopLevel = false;
                    nurseJLDForm.FormBorderStyle = FormBorderStyle.None;
                    nurseJLDForm.Dock = DockStyle.Fill;
                    nurseJLDForm.Show();
                    xtraTabPage.Controls.Add(nurseJLDForm);
                    tabControlSCD.TabPages.Add(xtraTabPage);
                }
                tabControlSCD.SelectedTabPage = xtraTabPage;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
            }
        }

        private void tabControlSCD_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            //if (e.Page.Controls.Count == 0)
            //{
            //    MultiInputVitalSign multiInputVitalSignFrm = new MultiInputVitalSign();
            //    multiInputVitalSignFrm.Dock = DockStyle.Fill;
            //    multiInputVitalSignFrm.FormBorderStyle = FormBorderStyle.None;
            //    multiInputVitalSignFrm.Show();
            //    e.Page.Controls.Add(multiInputVitalSignFrm);
            //}
        }

        /// <summary>
        /// 三测单单人录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSCDSing_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                XtraTabPage xtraTabPage = HasTabPage(SCDZT);
                if (xtraTabPage == null)
                {
                    xtraTabPage = new XtraTabPage();
                    xtraTabPage.Name = SCDZT;
                    xtraTabPage.Text = "三测单单人录入";
                    ThreeRecordMainZT threeRecordMainZT = new ThreeRecordMainZT(m_app);
                    threeRecordMainZT.TopLevel = false;
                    threeRecordMainZT.FormBorderStyle = FormBorderStyle.None;
                    threeRecordMainZT.Dock = DockStyle.Fill;
                    threeRecordMainZT.Show();
                    xtraTabPage.Controls.Add(threeRecordMainZT);
                    tabControlSCD.TabPages.Add(xtraTabPage);
                }
                tabControlSCD.SelectedTabPage = xtraTabPage;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// 三测单批量录入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSCDPL_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                XtraTabPage xtraTabPage = HasTabPage(SCDPL);
                if (xtraTabPage == null)
                {
                    xtraTabPage = new XtraTabPage();
                    xtraTabPage.Name = SCDPL;
                    xtraTabPage.Text = "三测单多人录入";
                    MultiInputVitalSign multiInputVitalSignFrm = new MultiInputVitalSign();
                    multiInputVitalSignFrm.Dock = DockStyle.Fill;
                    multiInputVitalSignFrm.FormBorderStyle = FormBorderStyle.None;
                    multiInputVitalSignFrm.Show();
                    xtraTabPage.Controls.Add(multiInputVitalSignFrm);
                    tabControlSCD.TabPages.Add(xtraTabPage);
                }
                tabControlSCD.SelectedTabPage = xtraTabPage;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.StackTrace);
                throw;
            }
        }

        private void tabControlSCD_CloseButtonClick(object sender, EventArgs e)
        {
            if (tabControlSCD.SelectedTabPage == null)
            {
                return;
            }
            tabControlSCD.TabPages.Remove(tabControlSCD.SelectedTabPage);
        }

        private XtraTabPage HasTabPage(string tabPageName)
        {

            try
            {
                if (tabControlSCD.TabPages == null) return null;
                foreach (XtraTabPage item in tabControlSCD.TabPages)
                {
                    if (item.Name == tabPageName)
                    {
                        return item;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void btnHLDPL_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {

                ChooseHLDPLForm chooseHLDPLForm = new ChooseHLDPLForm(commonNoteEntityListPL);
                DialogResult dresult = chooseHLDPLForm.ShowDialog();
                if (dresult == DialogResult.OK)
                {
                    OpenHLDPL(chooseHLDPLForm.SelectCommonNoteEntity);
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barLargeButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要退出吗？", "退出", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.Cancel)
                {
                    return;
                }
                //退出
                this.Close();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }




    }
}
