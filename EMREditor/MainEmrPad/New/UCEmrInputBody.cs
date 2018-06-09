using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraNavBar;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.Core.CommonTableConfig;
using DrectSoft.Core.CommonTableConfig.CommonNoteUse;
using DrectSoft.Core.MainEmrPad.HelpForm;
using DrectSoft.Core.MainEmrPad.HistoryEMR;
using DrectSoft.Core.PersonTtemTemplet;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
namespace DrectSoft.Core.MainEmrPad.New
{
    public partial class UCEmrInputBody : DevExpress.XtraEditors.XtraUserControl
    {
        private IEmrHost m_app;
        public IEmrHost App
        {
            get { return m_app; }
            set { m_app = value; }
        }

        #region 基本控制
        /// <summary>
        /// 左侧菜单图片文件夹状态
        /// </summary>
        public FloderState floaderState = FloderState.Default;
        /// <summary>
        /// 编辑器是否可以编辑的开关
        /// </summary>
        private bool m_EditorEnableFlag = true;
        public bool EditorEnableFlag
        {
            get { return m_EditorEnableFlag; }
            set { m_EditorEnableFlag = value; }
        }
        /// <summary>
        /// 是否允许拖拽操作
        /// </summary>
        private bool m_IsAllowDrop = true;
        /// <summary>
        /// 弹出右键菜单开关
        /// </summary>
        public bool m_RightMouseButtonFlag = true;
        public bool RightMouseButtonFlag
        {
            get { return m_RightMouseButtonFlag; }
            set { m_RightMouseButtonFlag = value; }
        }
        #endregion

        #region 属性变量
        /// <summary>
        /// 当前病人
        /// </summary>
        private Inpatient m_CurrentInpatient;
        public Inpatient CurrentInpatient
        {
            get { return m_CurrentInpatient; }
            set { m_CurrentInpatient = value; }
        }
        /// <summary>
        /// 小模板列表
        /// </summary>
        private DataTable m_MyTreeFolders;
        private DataTable m_MyLeafs;

        private RecordDal m_RecordDal;
        public RecordDal RecordDal
        {
            get { return m_RecordDal; }
            set { m_RecordDal = value; }
        }
        private PatRecUtil m_PatUtil;
        public PatRecUtil PatUtil
        {
            get { return m_PatUtil; }
            set { m_PatUtil = value; }
        }
        /// <summary>
        /// 是否开启临床数据提取功能
        /// </summary>
        private string IsOpenLcdata = "0";
        private string IsOpenLisdata = "0";
        private string IsOpenpacsdata = "0";
        private string IsOpenorderdata = "0";
        /// <summary>
        /// 是否开启提交病历限制（必须存在首次病程才能提交病历0：不限制 1：限制）
        /// </summary>
        private string isCommitLimit = "0";
        /// <summary>
        /// 界面中调用的等待窗体
        /// </summary>
        private WaitDialogForm m_WaitDialog;
        public WaitDialogForm WaitDialog
        {
            get { return m_WaitDialog; }
            set { m_WaitDialog = value; }
        }

        /// <summary>
        /// 当前已经打开的病历模型集合
        /// </summary>
        public Dictionary<EmrModel, XtraTabPage> TempModelPages
        {
            get { return m_TempModelPages; }
        }
        Dictionary<EmrModel, XtraTabPage> m_TempModelPages;

        /// <summary>
        /// 当前已经打开的非病历类型的界面如首页、三测单等的集合
        /// </summary>
        public Dictionary<EmrModelContainer, XtraTabPage> TempContainerPages
        {
            get { return m_TempContainerPages; }
        }
        Dictionary<EmrModelContainer, XtraTabPage> m_TempContainerPages;

        /// <summary>
        /// 小模板列表
        /// </summary>
        private PersonItemManager m_PersonItem;
        /// <summary>
        /// 病历类别
        /// 注：加载右侧小模板列表使用
        /// </summary>
        public string MyContainerCode;

        /// <summary>
        /// 当前界面中编辑器对应的病历模型对应的树节点
        /// 【在SelectedPageChanged事件中计算出来】
        /// </summary>
        TreeListNode m_CurrentTreeListNode;

        /// <summary>
        /// 当前用户的信息
        /// </summary>
        Employee _doctorEmployye;
        Employee DoctorEmployee
        {
            get
            {
                if (_doctorEmployye == null)
                {
                    _doctorEmployye = new Employee(DS_Common.currentUser.Id);
                    _doctorEmployye.ReInitializeProperties();
                }
                _doctorEmployye.CurrentDept.Code = DS_Common.currentUser.CurrentDeptId;
                _doctorEmployye.CurrentWard.Code = DS_Common.currentUser.CurrentWardId;
                return _doctorEmployye;
            }
        }

        /// <summary>
        /// 当前界面中编辑器对应的病历模型
        /// 【在SelectedPageChanged事件中计算出来】
        /// </summary>
        public EmrModel CurrentModel
        {
            get { return m_CurrentModel; }
        }
        EmrModel m_CurrentModel;

        /// <summary>
        /// 当前选中的非病历类型的界面
        /// 如首页、三测单之类的内容
        /// 【在SelectedPageChanged事件中计算出来】
        /// </summary>
        public IEMREditor CurrentEmrEditor
        {
            get { return m_CurrentEmrEditor; }
        }
        IEMREditor m_CurrentEmrEditor;

        #region 增加是否在左侧树及右侧工具箱中显示LIS、PACS及医嘱信息
        //add by ywk 二〇一三年六月六日 10:56:04  
        private bool isshowlis;
        /// <summary>
        /// 控制是否显示LIS数据
        /// </summary>
        public bool IsShowLis
        {
            get { return isshowlis; }
            set { isshowlis = value; }
        }
        private bool isshowpacs;
        /// <summary>
        /// 控制是否显示PACS数据
        /// </summary>
        public bool IsShowPacs
        {
            get { return isshowpacs; }
            set { isshowpacs = value; }
        }
        private bool isshowdoc;
        /// <summary>
        /// 控制是否显示医嘱数据
        /// </summary>
        public bool IsShowDoc
        {
            get { return isshowdoc; }
            set { isshowdoc = value; }
        }
        private bool isshownur;
        /// <summary>
        /// 控制是否显示护理
        /// </summary>
        public bool IsShowNur
        {
            get { return isshownur; }
            set { isshownur = value; }
        }
        private bool isshowqc;
        /// <summary>
        /// 控制是否显示质控
        /// </summary>
        public bool IsShowQc
        {
            get { return isshowqc; }
            set { isshowqc = value; }
        }
        private bool isshowoup;
        /// <summary>
        /// 控制是否显示门诊病历
        /// </summary>
        public bool IsShowOup
        {
            get { return isshowoup; }
            set { isshowoup = value; }
        }
        private bool isshowrep;
        /// <summary>
        /// 控制是否显示传染病
        /// </summary>
        public bool IsShowRep
        {
            get { return isshowrep; }
            set { isshowrep = value; }
        }
        private bool isshowdig;
        /// <summary>
        /// 控制是否显示疾病危重评分
        /// </summary>
        public bool IsShowDig
        {
            get { return isshowdig; }
            set { isshowdig = value; }
        }
        private bool isshowtest;
        /// <summary>
        /// 控制是否显示三测单
        /// </summary>
        public bool IsShowTest
        {
            get { return isshowtest; }
            set { isshowtest = value; }
        }


        #endregion

        #region 控制右侧PANEL的显示与否
        private bool isshowdockpanel;
        /// <summary>
        /// 控制右侧DockPanel
        /// add by ywk 2013年6月13日 09:56:10
        /// </summary>
        public bool IsShowDockPanel
        {
            get { return isshowdockpanel; }
            set { isshowdockpanel = value; }
        }

        #endregion


        #endregion

        #region 公开控件
        /// <summary>
        /// CurrentForm
        /// </summary>
        public EditorForm CurrentForm
        {
            get
            {
                return CurrentInputTabPages.CurrentForm;
            }
        }

        ///编辑器主页面
        public UCEmrInputTabPages CurrentInputTabPages
        {
            get
            {
                try
                {
                    if (panelControlEditor.Controls.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return panelControlEditor.Controls[0] as UCEmrInputTabPages;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        ///编辑器主页面选项卡容器
        public XtraTabControl xtraTabControlEmr
        {
            get
            {
                return CurrentInputTabPages.tabControlEmr;
            }
        }

        ///病案首页选项卡
        public XtraTabPage xtraTabPageIEMMainPage
        {
            get
            {
                return CurrentInputTabPages.tabPageIEMMainPage;
            }
        }

        ///父级UCEmrInput控件
        private UCEmrInput ucEmrInput
        {
            get
            {
                return (UCEmrInput)this.Parent.Parent;
            }
        }

        /// <summary>
        /// 预览区
        /// </summary>
        public UCEmrInputPreView ucEmrInputPreView
        {
            get
            {
                return (UCEmrInputPreView)m_TempDailyPreViewCollection.FirstOrDefault(p => p.Key == m_CurrentModel.DeptChangeID).Value;
            }
        }

        /// <summary>
        /// 当前界面中编辑器对应的病历文件夹模型
        /// 【在SelectedPageChanged事件中计算出来】
        /// </summary>
        public EmrModelContainer CurrentModelContainer
        {
            get { return m_CurrentModelContainer; }
            set { m_CurrentModelContainer = value; }
        }
        EmrModelContainer m_CurrentModelContainer;

        /// <summary>
        /// 右侧模块
        /// </summary>
        public DockPanel PanelContainerRight
        {
            get { return panelContainerRight; }
            set { panelContainerRight = value; }
        }

        /// <summary>
        /// TreeList当前病历树
        /// </summary>
        public TreeList CurrentTreeList
        {
            get
            {
                return treeListEmrNodeList;
            }
        }
        #endregion

        #region 构造器
        public UCEmrInputBody()
        {
            try
            {
                InitializeComponent();
                ///添加编辑器主页面
                AddEmrInputTabPages();
                ///初始化基本元素
                LoadBasicUCEmr();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public UCEmrInputBody(Inpatient inpatient, IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_CurrentInpatient = inpatient;
                m_app = app;
                ///设置权限状态
                floaderState = string.IsNullOrEmpty(m_app.FloderState) ? FloderState.Default : (FloderState)Enum.Parse(typeof(FloderState), m_app.FloderState);
                ///添加编辑器主页面
                AddEmrInputTabPages();
                ///初始化基本元素
                LoadBasicUCEmr();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public UCEmrInputBody(Inpatient inpatient, IEmrHost app, FloderState floadState)
        {
            try
            {
                InitializeComponent();
                m_CurrentInpatient = inpatient;
                m_app = app;
                floaderState = floadState;
                ///添加编辑器主页面
                AddEmrInputTabPages();
                ///初始化基本元素
                LoadBasicUCEmr();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 初始化窗体
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCEmrInputBody_Load(object sender, EventArgs e)
        {
            try
            {
                OnLoad();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 初始化主界面
        /// </summary>
        /// 注：包含右侧、右侧模块、病案首页
        public void OnLoad()
        {
            try
            {
                if (null == m_RecordDal)
                {
                    m_RecordDal = new RecordDal(m_app.SqlHelper);
                }

                //控制PACS\LIS栏位的显示add by ywk \二〇一三年六月六日 13:13:12
                IsShowPacs = DS_BaseService.IsShowThisMD("IsShowLISPACSDoc", "PACS");
                IsShowLis = DS_BaseService.IsShowThisMD("IsShowLISPACSDoc", "LIS");
                IsShowDoc = DS_BaseService.IsShowThisMD("IsShowLISPACSDoc", "Doc");
                IsShowNur = DS_BaseService.IsShowThisMD("IsShowLISPACSDoc", "Nur");
                IsShowOup = DS_BaseService.IsShowThisMD("IsShowLISPACSDoc", "Oup");
                IsShowDig = DS_BaseService.IsShowThisMD("IsShowLISPACSDoc", "Dig");
                IsShowQc = DS_BaseService.IsShowThisMD("IsShowLISPACSDoc", "Qc");
                IsShowRep = DS_BaseService.IsShowThisMD("IsShowLISPACSDoc", "Rep");
                IsShowTest = DS_BaseService.IsShowThisMD("IsShowLISPACSDoc", "Test");
                //控制右侧DockPanel add  by ywk 2013年6月13日 09:56:45
                IsShowDockPanel = DS_BaseService.IsShowThisMD("IsShowDockPanel", "MainEmrPad");


                ///初始化左侧模块和右侧模块
                InitLeftAndRightPart();
                ///加载病案首页
                LoadIEMMainPage();
                ///选中指定的病历
                SelectSpecifiedNode(treeListEmrNodeList.Nodes);
                if (IsShowDockPanel)//显示
                {
                    panelContainerRight.Visibility = DockVisibility.Visible;
                }
                else
                {
                    panelContainerRight.Visibility = DockVisibility.AutoHide;
                }
                //Add by xlb 2013-06-19
                treeListEmrNodeList.FocusedNode = SetDefaultFocusedNode(treeListEmrNodeList.Nodes);
                //FocusedNodeChanged(SetDefaultFocusedNode(treeListEmrNodeList.Nodes));
                navBarGroupPacs.Visible = IsShowPacs;
                navBarGroupLis.Visible = IsShowLis;
                navBarGroupDoctorAdive.Visible = IsShowDoc;
                IsOpenLcdata = GetConfigValueByKey("IsOpenlcData");//读取是否开启临床数据配置项
                IsOpenLisdata = GetConfigValueByKey("IsOpenLisdata");
                IsOpenpacsdata = GetConfigValueByKey("IsOpenpacsdata");
                IsOpenorderdata = GetConfigValueByKey("IsOpenorderdata");
                isCommitLimit = GetConfigValueByKey("isCommitLimit");//提交病历限制
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetConfigValueByKey(string key)
        {
            try
            {
                string sql1 = " select * from appcfg where configkey = '" + key + "'  ";
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql1, CommandType.Text);
                string config = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    config = dt.Rows[0]["value"].ToString();
                }
                return config;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 添加编辑器tab页
        /// </summary>
        private void AddEmrInputTabPages()
        {
            try
            {
                if (null != CurrentInputTabPages)
                {
                    this.panelControlEditor.Controls.Add(CurrentInputTabPages);
                    CurrentInputTabPages.Dock = DockStyle.Fill;
                }
                else
                {
                    UCEmrInputTabPages ucEmrInputTabPages = new UCEmrInputTabPages(m_CurrentInpatient, m_app);
                    ucEmrInputTabPages.Dock = DockStyle.Fill;
                    this.panelControlEditor.Controls.Add(ucEmrInputTabPages);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 选中默认的右侧工具栏
        /// </summary>
        private void SelectDefaultRightTool()
        {
            try
            {
                panelContainerRight.ActiveChild = dockPanelRight1;
                navBarControlTool.ActiveGroup = navBarGroupSymbol;
                navBarGroupSymbol.Expanded = true;
                ///默认隐藏医嘱数据
                navBarGroupDoctorAdive.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化左侧模块和右侧模块
        /// </summary>
        private void InitLeftAndRightPart()
        {
            try
            {
                if (m_CurrentInpatient == null)
                {
                    return;
                }
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在读取病人病历数据...");

                ///初始化病历树
                InitPatTree();

                ///选中默认的右侧工具栏
                SelectDefaultRightTool();
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载模板列表...");

                ///初始化小模板列表
                InitPersonTree(true, string.Empty);

                ///加载图片库
                LoadImageGalleryThread();

                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LoadImageGalleryThread()
        {
            Thread imageThread = new Thread(new ThreadStart(LoadImageGallery));
            imageThread.Start();
        }

        private void LoadImageGallery()
        {
            try
            {
                LoadImageInvoke(m_PatUtil.ImageGallery);
            }
            catch (Exception)
            {
            }
        }

        private void LoadImageInvoke(DataTable dtImage)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<DataTable>(LoadImageInvoke), dtImage);
                }
                else
                {
                    treeListPicture.BeginUnboundLoad();
                    foreach (DataRow row in dtImage.Rows)
                    {
                        TreeListNode node = treeListPicture.AppendNode(new object[] { row["名称"] }, null);
                        node.Tag = row;
                    }
                    treeListPicture.EndUnboundLoad();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 窗体Size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCEmrInputBody_Resize(object sender, EventArgs e)
        {
            try
            {
                panelContainerLeft.FloatSize = panelContainerLeft.Size;
                panelContainerRight.FloatSize = panelContainerRight.Size;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 初始化基本元素
        /// <summary>
        /// 加载窗体基本元素，初始化变量
        /// </summary>
        private void LoadBasicUCEmr()
        {
            try
            {
                xtraTabPageIEMMainPage.AccessibleName = ""; //为空则病案首页可以编辑，否则不显示编辑按钮
                MacroUtil.MacroSource = null;
                ExtraInit();
                InitDictionary();
                //InitFont();
                //InitDefaultFont();
                //HideWaitDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置NavBarControl皮肤(右侧模块)
        /// </summary>
        private void ExtraInit()
        {
            try
            {
                this.navBarControlTool.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Office 2010 Silver");
                this.navBarControlClinic.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Office 2010 Silver");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化用到的变量
        /// </summary>
        public void InitDictionary()
        {
            try
            {
                m_TempModelPages = new Dictionary<EmrModel, XtraTabPage>();
                m_TempContainerPages = new Dictionary<EmrModelContainer, XtraTabPage>();
                m_TempDailyPreViewCollection = new Dictionary<string, UCEmrInputPreView>();
                m_MyTreeFolders = null;
                m_MyLeafs = null;

                m_PersonItem = null;
                m_CurrentModel = null;
                m_CurrentModelContainer = null;
                m_CurrentTreeListNode = null;
                m_CurrentEmrEditor = null;
                m_RightMouseButtonFlag = true;
                m_EditorEnableFlag = true;
                m_IsAllowDrop = true;
                m_PatUtil = null;
                ///重置历史病历
                treeListHistory.Nodes.Clear();
                //m_IsNeedActiveEditArea = true;
                //m_IsLocateDaily = true;
                //m_NodeIEMMainPage = null;
                //m_CurrentSelectedEmrNode = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据节点重置按钮状态
        /// </summary>
        /// <param name="node"></param>
        public void ResetBarBtnState(TreeListNode node)
        {
            try
            {
                if (null == node || null == node.Tag)
                {
                    return;
                }
                if (node.Tag is EmrModelContainer)
                {
                    ucEmrInput.ResetEditModeAction(node.Tag as EmrModelContainer);
                }
                else if (node.Tag is EmrModelDeptContainer)
                {
                    ucEmrInput.ResetEditModeAction(node.Tag as EmrModelDeptContainer);
                }
                else if (node.Tag is EmrModel)
                {
                    ucEmrInput.ResetEditModeAction(node.Tag as EmrModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 病案首页
        /// <summary>
        /// 加载病案首页(先加载图片库)
        /// </summary>
        private void LoadIEMMainPage()
        {
            try
            {
                ///加载病案首页
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病案首页...");
                TreeListNode firstPageNode = GetIEMMainPageNode(treeListEmrNodeList.Nodes);
                if (null != firstPageNode && null != firstPageNode.Tag && firstPageNode.Tag is EmrModelContainer)
                {
                    EmrModelContainer container = (EmrModelContainer)firstPageNode.Tag;
                    if (null != container)
                    {
                        CreateCommonDocument(container);
                    }
                }
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取TreeList中“病案首页”节点
        /// </summary>
        /// <param name="node"></param>
        private TreeListNode GetIEMMainPageNode(TreeListNodes nodeList)
        {
            try
            {
                if (null == nodeList || nodeList.Count == 0)
                {
                    return null;
                }

                for (int i = 0; i < nodeList.Count; i++)
                {
                    TreeListNode node = nodeList[i];
                    if (node.Tag != null && node.Tag is EmrModelContainer)
                    {
                        EmrModelContainer container = node.Tag as EmrModelContainer;
                        if (container.ContainerCatalog == ContainerCatalog.BingAnShouYe)
                        {
                            return node;
                        }
                    }
                    if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        TreeListNode returnNode = GetIEMMainPageNode(node.Nodes);
                        if (null != returnNode)
                        {
                            return returnNode;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置默认焦点
        /// 区分医生护士
        /// <auth>XLB</auth>
        /// <date>2013-06-19</date>
        /// </summary>
        /// <param name="nodeLists"></param>
        private TreeListNode SetDefaultFocusedNode(TreeListNodes nodeLists)
        {
            try
            {
                if (nodeLists == null || nodeLists.Count <= 0)
                {
                    return null;
                }
                if (m_PatUtil == null)
                {
                    m_PatUtil = new PatRecUtil(m_app, m_CurrentInpatient);
                }
                string[] chooseNodeList = m_PatUtil.GetValueByConfigKey("EmrDefaultFocusedNode").Split(',');
                foreach (TreeListNode item in nodeLists)
                {
                    if (item.Tag != null && item.Tag is EmrModelContainer)
                    {
                        EmrModelContainer modelContainer = item.Tag as EmrModelContainer;
                        if (DoctorEmployee.Kind == EmployeeKind.Doctor)
                        {
                            if (modelContainer.ContainerCatalog == chooseNodeList[0])
                            {
                                item.Selected = true;
                                return item;
                            }
                        }
                        else if (DoctorEmployee.Kind == EmployeeKind.Nurse)
                        {
                            if (modelContainer.ContainerCatalog == chooseNodeList[1])
                            {
                                item.Selected = true;
                                return item;
                            }
                        }
                        else if (DoctorEmployee.Kind == EmployeeKind.Outdoctor)
                        {
                            if (modelContainer.ContainerCatalog == chooseNodeList[2])
                            {
                                item.Selected = true;
                                return item;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    if (item.Nodes != null && item.Nodes.Count > 0)
                    {
                        TreeListNode node = SetDefaultFocusedNode(item.Nodes);
                        if (node != null)
                        {
                            node.Selected = true;
                            return node;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建非病历类型界面
        /// 如首页、三测单之类的内容
        /// </summary>
        public void CreateCommonDocument(EmrModelContainer container)
        {
            ///读取容器里的包含第三方控件，使用反射使其实例化
            XtraTabPage newPad = null;
            try
            {
                if (string.IsNullOrEmpty(container.ModelEditor)) return;

                ///IEMREditor editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString() });
                IEMREditor editor;
                if (container.Args != "")
                {
                    editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString(), container.Args });
                }
                else
                {
                    editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString() });
                }

                if (container.ContainerCatalog == ContainerCatalog.SanCeDan)
                {
                    //xll 三测单权限控制 2013-05-24
                    if (this.floaderState == FloderState.Nurse
                        || (this.floaderState == FloderState.Default && DoctorEmployee.Kind == EmployeeKind.Nurse)
                        || (this.floaderState == FloderState.NoneAudit && DoctorEmployee.Kind == EmployeeKind.Nurse))
                    {

                        editor.ReadOnlyControl = false;
                    }
                    else
                    {
                        editor.ReadOnlyControl = true;
                    }
                }
                else
                {
                    editor.ReadOnlyControl = !m_EditorEnableFlag;//传递是否可以编辑字段
                }

                if (container.ContainerCatalog == ContainerCatalog.BingAnShouYe)//"病案首页"只需要加入现有的TabPage里
                {
                    newPad = xtraTabPageIEMMainPage;
                }
                else
                {
                    newPad = xtraTabControlEmr.TabPages.Add();
                }

                newPad.Text = editor.Title;
                editor.DesignUI.Dock = DockStyle.Fill;
                newPad.Controls.Add(editor.DesignUI);
                if (string.IsNullOrEmpty(newPad.AccessibleName) && (floaderState == FloderState.FirstPage || floaderState == FloderState.None || floaderState == FloderState.Nurse))
                {
                    newPad.AccessibleName = "不能编辑";
                }
                //加载控件至页面
                editor.Load(m_app);

                newPad.Tag = container;
                m_CurrentEmrEditor = editor;
                m_TempContainerPages.Add(container, newPad);
                xtraTabControlEmr.SelectedTabPage = newPad;

                if (m_CurrentTreeListNode != null)
                {
                    newPad.Text = m_CurrentTreeListNode.GetValue("colName").ToString();
                }
                MyContainerCode = container.ContainerCatalog;

                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                xtraTabControlEmr.TabPages.Remove(newPad);
                if (!(ex is System.ArgumentNullException))
                {
                    MyMessageBox.Show(1, ex);
                }
            }
        }
        #endregion

        #region 左侧病历树

        #region 加载树
        /// <summary>
        /// 初始化左侧的树 - 一级节点
        /// </summary>
        private void InitPatTree()
        {
            try
            {
                UnRegisterEvent();
                treeListEmrNodeList.ClearNodes();
                ///读取底层文件夹
                if (m_PatUtil == null)
                {
                    m_PatUtil = new PatRecUtil(m_app, m_CurrentInpatient);
                }
                ///判断配置是否为简版工作站
                string simpleDoc = DS_SqlService.GetConfigValueByKey("SimpleDoctorCentor");
                DataTable datatable;
                if (simpleDoc == "1")
                {
                    datatable = m_PatUtil.GetFolderInfoSimpleDoc("00");
                }
                else
                {
                    datatable = m_PatUtil.GetFolderInfo("00");
                }
                if (null == datatable || datatable.Rows.Count == 0)
                {
                    return;
                }
                //InsertIntoChangedDeptsNew();
                CheckForInsertChangedDept();

                //先在此处判断是否显示【检验资料】【检查资料】父节点add by ywk 二〇一三年六月六日 13:20:09 
                //简版医生站这样写会有问题 add by ywk 2013年7月17日 16:48:05
                DataRow[] finddr = datatable.Select(" CCODE in ('12','14','15','16','AN','AQ','AS')");
                if (finddr.Length >= 1)
                {
                    if (!IsShowNur)//不显示LIS节点
                    {
                        datatable.Rows.Remove(finddr[0]);
                    }
                    if (!IsShowLis)//不显示PACS节点
                    {
                        datatable.Rows.Remove(finddr[1]);
                    }
                    if (!IsShowPacs)
                    {
                        datatable.Rows.Remove(finddr[2]);
                    }
                    if (!IsShowQc)
                    {
                        datatable.Rows.Remove(finddr[3]);
                    }
                    if (!IsShowOup)
                    {
                        datatable.Rows.Remove(finddr[4]);
                    }
                    if (!IsShowRep)
                    {
                        datatable.Rows.Remove(finddr[5]);
                    }
                    if (!IsShowDig)
                    {
                        datatable.Rows.Remove(finddr[6]);
                    }

                }

                foreach (DataRow row in datatable.Rows)
                {
                    InitTree(row, null);
                }
                RegisterEvent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化左侧的树 - 子节点
        /// </summary>
        private void InitTree(DataRow row, TreeListNode node)
        {
            try
            {
                if (null == row)
                {
                    return;
                }

                TreeListNode patNode = treeListEmrNodeList.AppendNode(new object[] { row["CNAME"] }, node);
                EmrModelContainer container = new EmrModelContainer(row);
                patNode.Tag = container;
                if (container.ContainerCatalog == "AA")
                {///设置默认选中节点
                    patNode.Selected = true;
                }
                ///寻找子节点
                DataTable datatable = m_PatUtil.GetFolderInfo(row["CCODE"].ToString());

                if (row["CCODE"].ToString() == "11")//遍历到医生文档,判断医嘱是否显示 add by ywk二〇一三年六月六日 13:29:04
                {
                    DataRow[] finddr = datatable.Select(" CCODE ='29'");
                    if (finddr.Length > 0)
                    {
                        if (!IsShowDoc)//不显示
                        {
                            datatable.Rows.Remove(finddr[0]);
                        }
                    }
                }
                if (row["CCODE"].ToString() == "12" && IsShowNur)//遍历到护士文档,判断三测单是否显示 add by Ukey二〇一六年八月三十一日
                {
                    DataRow[] finddr = datatable.Select(" CCODE ='13'");
                    if (finddr.Length > 0)
                    {
                        if (!IsShowTest)//不显示
                        {
                            datatable.Rows.Remove(finddr[0]);
                        }
                    }
                }


                ///当前文件夹下存在其它文件夹
                if (null != datatable && datatable.Rows.Count > 0)
                {
                    foreach (DataRow nextRow in datatable.Rows)
                    {
                        InitTree(nextRow, patNode);
                    }
                    return;
                }

                if (container.EmrContainerType != ContainerType.None)
                {
                    if (container.ContainerCatalog == ContainerCatalog.BingChengJiLu)
                    {
                        DataTable changeDepts = DS_SqlService.GetInpatientChangeInfo((int)m_CurrentInpatient.NoOfFirstPage);
                        if (null != changeDepts && changeDepts.Rows.Count > 0)
                        {
                            foreach (DataRow deptRow in changeDepts.Rows)
                            {
                                ///科室文件夹
                                EmrModelDeptContainer deptContainer = new EmrModelDeptContainer(deptRow);
                                TreeListNode deptNode = treeListEmrNodeList.AppendNode(new object[] { deptContainer.Name }, patNode);
                                deptNode.Tag = deptContainer;
                                ///病程
                                DataTable leafs = m_PatUtil.GetPatRecInfoNew(container.ContainerCatalog, m_CurrentInpatient.NoOfFirstPage.ToString(), string.IsNullOrEmpty(deptRow["id"].ToString()) ? -1 : int.Parse(deptRow["id"].ToString()));
                                if (null != leafs && leafs.Rows.Count > 0)
                                {
                                    foreach (DataRow leaf in leafs.Rows)
                                    {
                                        EmrModel leafmodel = new EmrModel(leaf);
                                        TreeListNode leafNode = treeListEmrNodeList.AppendNode(new object[] { leafmodel.ModelName }, deptNode);
                                        leafNode.Tag = leafmodel;
                                    }
                                }
                                ///设置展开状态
                                if (deptContainer.DepartmentID == DS_Common.currentUser.CurrentDeptId && CheckIsDoctor())
                                {
                                    node.Expanded = true;
                                    patNode.Expanded = true;
                                    deptNode.Expanded = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        DataTable leafs = m_PatUtil.GetPatRecInfo(container.ContainerCatalog, m_CurrentInpatient.NoOfFirstPage.ToString());
                        if (null != leafs && leafs.Rows.Count > 0)
                        {
                            foreach (DataRow leaf in leafs.Rows)
                            {
                                EmrModel leafmodel = new EmrModel(leaf);
                                TreeListNode leafNode = treeListEmrNodeList.AppendNode(new object[] { leafmodel.ModelName }, patNode);
                                leafNode.Tag = leafmodel;
                            }
                        }
                    }
                }

                if (row["CCODE"].ToString() == "AP" || row["CCODE"].ToString() == "104")
                {///护理记录表格
                    InCommonNoteBiz inCommonNoteBiz = new InCommonNoteBiz(m_app);
                    ///该病人的所有生成单据
                    List<InCommonNoteEnmtity> InCommonNoteEnmtityList = inCommonNoteBiz.GetSimInCommonNote(m_CurrentInpatient.NoOfFirstPage.ToString());
                    foreach (InCommonNoteEnmtity inCommonNoteEnmtity in InCommonNoteEnmtityList)
                    {
                        string treeName = inCommonNoteEnmtity.InCommonNoteName + " " + DateUtil.getDateTime(inCommonNoteEnmtity.CreateDateTime, DateUtil.NORMAL_LONG);
                        TreeListNode leafNode = treeListEmrNodeList.AppendNode(new object[] { treeName }, patNode);
                        leafNode.Tag = inCommonNoteEnmtity;
                    }
                }
                //专门为诊疗时间轴加的功能 add by ywk 2013年7月23日 18:06:36
                if (row["CCODE"].ToString() == "87")//诊疗时间轴
                {
                    List<EmrModelContainer> listContainer = GetPatCureRecrod((int)m_CurrentInpatient.NoOfFirstPage, row);
                    foreach (EmrModelContainer emrModelContainer in listContainer)
                    {
                        TreeListNode leafNode = treeListEmrNodeList.AppendNode(new object[] { emrModelContainer.Name }, patNode);
                        leafNode.Tag = emrModelContainer;
                    }
                }


                else if (row["CCODE"].ToString() == ContainerCatalog.huizhenjilu)
                {///会诊记录
                    List<EmrModelContainer> listContainer = GetConsultRecrod((int)m_CurrentInpatient.NoOfFirstPage, row);
                    foreach (EmrModelContainer emrModelContainer in listContainer)
                    {
                        TreeListNode leafNode = treeListEmrNodeList.AppendNode(new object[] { emrModelContainer.Name }, patNode);
                        leafNode.Tag = emrModelContainer;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 设置左侧病历树的图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListEmrNodeList_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            try
            {
                TreeListNode node = e.Node;

                if (e.Node.Tag is EmrModelContainer)
                {
                    if (e.Node.Expanded)//文件夹打开
                    {
                        e.NodeImageIndex = 3;
                    }
                    else//文件夹未打开
                    {
                        e.NodeImageIndex = 2;
                    }
                    if (((EmrModelContainer)e.Node.Tag).ContainerCatalog == ContainerCatalog.huizhenjilu)
                    {
                        if (e.Node.ParentNode != null && e.Node.ParentNode.Tag != null && e.Node.ParentNode.Tag is EmrModelContainer)
                        {
                            if (((EmrModelContainer)e.Node.ParentNode.Tag).ContainerCatalog == ContainerCatalog.huizhenjilu)
                            {
                                e.NodeImageIndex = 0;
                            }
                        }
                    }
                }
                else if (e.Node.Tag is EmrModelDeptContainer)
                {
                    if (e.Node.Expanded)//文件夹打开
                    {
                        e.NodeImageIndex = 3;
                    }
                    else//文件夹未打开
                    {
                        e.NodeImageIndex = 2;
                    }
                }
                else if (e.Node.Tag is EmrModel)
                {
                    EmrModel model = (EmrModel)e.Node.Tag;
                    if (model.State == ExamineState.NotSubmit)//未提交
                    {
                        e.NodeImageIndex = 0;
                    }
                    else if (model.State == ExamineState.SubmitButNotExamine)//已提交，未审核
                    {
                        e.NodeImageIndex = 1;
                    }
                    else if (model.State == ExamineState.FirstExamine)//主治已经审核
                    {
                        e.NodeImageIndex = 14;
                    }
                    else if (model.State == ExamineState.SecondExamine)//主任已经审核
                    {
                        e.NodeImageIndex = 13;
                    }
                    else
                    {
                        e.NodeImageIndex = 1;
                    }
                }
                else if (e.Node.Tag is InCommonNoteEnmtity)
                {
                    e.NodeImageIndex = 0;
                }

                SetLockImage(e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设置节点图片状态
        /// </summary>
        public void SetStateImage()
        {
            try
            {
                TreeListNode node = this.treeListEmrNodeList.FocusedNode;
                if (node.Tag is EmrModelContainer)
                {
                    if (node.Expanded)
                    {
                        node.ImageIndex = 3;
                    }
                    else
                    {
                        node.ImageIndex = 2;
                    }
                }
                else if (node.Tag is EmrModel)
                {
                    EmrModel model = (EmrModel)node.Tag;
                    if (model.State == ExamineState.NotSubmit)//未提交
                    {
                        node.ImageIndex = 0;
                    }
                    else if (model.State == ExamineState.SubmitButNotExamine)//已提交，未审核
                    {
                        node.ImageIndex = 1;
                    }
                    else if (model.State == ExamineState.FirstExamine)//主治已经审核
                    {
                        node.ImageIndex = 14;
                    }
                    else if (model.State == ExamineState.SecondExamine)//主任已经审核
                    {
                        node.ImageIndex = 13;
                    }
                    else
                    {
                        node.ImageIndex = 1;
                    }
                }

                SetLockImage(node);
                treeListEmrNodeList.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetLockImage(DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            try
            {
                if (e.Node == null) return;

                if (floaderState == FloderState.Default || floaderState == FloderState.NoneAudit)
                {
                    if (DoctorEmployee.Kind == EmployeeKind.Nurse)//当前登录人是护士
                    {
                        if (!FindNurseDoc(e.Node))
                        {
                            SetLockFileAndFolderNode(e);
                        }
                    }
                    else if (CheckIsDoctor())//当前登录人是医生
                    {
                        if (FindNurseDoc(e.Node))
                        {
                            SetLockFileAndFolderNode(e);
                        }
                    }
                    else//其他人
                    {
                        SetLockFileAndFolderNode(e);
                    }
                }
                else if (floaderState == FloderState.FirstPage)
                {//病案管理 - 未归档的出院病人列表
                    if (!FindMediRecordFirstPage(e.Node))
                    {
                        SetLockFileAndFolderNode(e);
                    }
                }
                else if (floaderState == FloderState.None)
                {
                    SetLockFileAndFolderNode(e);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设置节点图片锁状态
        /// </summary>
        /// <param name="node"></param>
        private void SetLockImage(TreeListNode node)
        {
            try
            {
                if (node == null)
                {
                    return;
                }
                if (floaderState == FloderState.Default || floaderState == FloderState.NoneAudit)
                {///默认(只按医生护士区分权限)
                    if (DoctorEmployee.Kind == EmployeeKind.Nurse)//当前登录人是护士
                    {
                        if (!FindNurseDoc(node))
                        {
                            SetLockFileAndFolderNode(node);
                        }
                    }
                    else if (CheckIsDoctor())//当前登录人是医生
                    {
                        if (FindNurseDoc(node))
                        {
                            SetLockFileAndFolderNode(node);
                        }
                    }
                    else//其他人
                    {
                        SetLockFileAndFolderNode(node);
                    }
                }
                else if (floaderState == FloderState.Doctor)
                {///医生权限
                    if (CheckIsDoctor())//当前登录人是医生
                    {
                        if (FindNurseDoc(node))
                        {
                            SetLockFileAndFolderNode(node);
                        }
                    }
                }
                else if (floaderState == FloderState.Nurse)
                {///护士权限
                    if (DoctorEmployee.Kind == EmployeeKind.Nurse)//当前登录人是护士
                    {
                        if (!FindNurseDoc(node))
                        {
                            SetLockFileAndFolderNode(node);
                        }
                    }
                }
                else if (floaderState == FloderState.FirstPage)
                {///病案首页权限
                    if (!FindMediRecordFirstPage(node))
                    {
                        SetLockFileAndFolderNode(node);
                    }
                }
                else if (floaderState == FloderState.None)
                {///无权限
                    SetLockFileAndFolderNode(node);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置文件夹和文件锁
        /// </summary>
        /// <param name="e"></param>
        private void SetLockFileAndFolderNode(DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            try
            {
                if (e.Node == null)
                {
                    return;
                }
                e.NodeImageIndex = 12;//LockFile
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设置文件夹和文件锁
        /// </summary>
        /// <param name="e"></param>
        private void SetLockFileAndFolderNode(TreeListNode node)
        {
            try
            {
                if (node == null)
                {
                    return;
                }
                node.ImageIndex = 12;//LockFile
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断当前用户是否是医生
        /// </summary>
        /// <returns></returns>
        private bool CheckIsDoctor()
        {
            try
            {
                if (DoctorEmployee.Kind == EmployeeKind.Doctor || DoctorEmployee.Kind == EmployeeKind.Specialist)//当前登录人是医生
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 左侧菜单护士文档节点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-27</date>
        /// <param name="node"></param>
        /// </summary>
        private bool FindNurseDoc(TreeListNode node)
        {
            try
            {
                if (node == null)
                {
                    return false;
                }
                if (null != node.GetValue("colName") && node.GetValue("colName").ToString() == "护士文档")
                {
                    return true;
                }

                if (node.ParentNode != null)
                {
                    bool boo = FindNurseDoc(node.ParentNode);
                    if (boo)
                    {
                        return boo;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 左侧菜单病案首页节点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-27</date>
        /// <param name="node"></param>
        /// </summary>
        private bool FindMediRecordFirstPage(TreeListNode node)
        {
            try
            {
                if (node == null)
                {
                    return false;
                }

                if (null != node.GetValue("colName") && node.GetValue("colName").ToString() == "病案首页")
                {
                    return true;
                }
                else if (null != node.Tag && node.Tag is EmrModelContainer)
                {
                    EmrModelContainer container = node.Tag as EmrModelContainer;
                    if (null != container && container.ContainerCatalog == "AA")
                    {
                        return true;
                    }
                }
                if (node.ParentNode != null)
                {
                    bool boo = FindMediRecordFirstPage(node.ParentNode);
                    if (boo)
                    {
                        return boo;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 插入转科记录
        /**
        /// <summary>
        /// 插入转科记录
        /// 注：缺少转科记录的时候会先插入
        /// </summary>
        private void InsertIntoChangedDeptsNew()
        {
            try
            {
                ///根据病历或者科室和病区
                DataTable allDeptAndWards = YD_SqlService.GetDeptAndWardByRecords((int)m_CurrentInpatient.NoOfFirstPage);
                DataTable changeDepts = YD_SqlService.GetInpatientChangeInfo((int)m_CurrentInpatient.NoOfFirstPage);

                if (null == changeDepts || changeDepts.Rows.Count == 0 || !changeDepts.AsEnumerable().Any(p => p["newdeptid"].ToString() == YD_Common.currentUser.CurrentDeptId && p["newwardid"].ToString() == YD_Common.currentUser.CurrentWardId))
                {///不存在转科记录时
                    List<OracleParameter> list = new List<OracleParameter>();
                    OracleParameter param1 = new OracleParameter("noofinpat", OracleType.Int32);
                    param1.Value = (int)m_CurrentInpatient.NoOfFirstPage;
                    list.Add(param1);
                    OracleParameter param2 = new OracleParameter("newdeptid", OracleType.VarChar);
                    param2.Value = YD_Common.currentUser.CurrentDeptId;
                    list.Add(param2);
                    OracleParameter param3 = new OracleParameter("newwardid", OracleType.VarChar);
                    param3.Value = YD_Common.currentUser.CurrentWardId;
                    list.Add(param3);
                    OracleParameter param4 = new OracleParameter("createuser", OracleType.VarChar);
                    param4.Value = YD_Common.currentUser.Id;
                    list.Add(param4);
                    OracleParameter param5 = new OracleParameter("createtime", OracleType.VarChar);
                    param5.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    list.Add(param5);
                    DataTable theInpatient = YD_SqlService.GetInpatientByID((int)m_CurrentInpatient.NoOfFirstPage);
                    if (null != theInpatient && theInpatient.Rows.Count == 1)
                    {
                        OracleParameter param6 = new OracleParameter("newbedid", OracleType.VarChar);
                        param6.Value = null == theInpatient.Rows[0]["outbed"] ? string.Empty : theInpatient.Rows[0]["outbed"].ToString();
                        list.Add(param6);
                    }
                    OracleParameter param7 = new OracleParameter("changestyle", OracleType.Char);
                    param7.Value = (null == changeDepts || changeDepts.Rows.Count == 0) ? 0 : 1;
                    list.Add(param7);
                    OracleParameter param8 = new OracleParameter("valid", OracleType.Int32);
                    param8.Value = 1;
                    list.Add(param8);
                    YD_SqlService.InsertInpChangeInfo(list);
                }
                if (null != allDeptAndWards && allDeptAndWards.Rows.Count > 0)
                {///存在病程记录时
                    foreach (DataRow deptAndWard in allDeptAndWards.Rows)
                    {
                        if (deptAndWard["departcode"].ToString() == YD_Common.currentUser.CurrentDeptId && deptAndWard["wardcode"].ToString() == YD_Common.currentUser.CurrentWardId)
                        {
                            continue;
                        }
                        if (!changeDepts.AsEnumerable().Any(p => p["newdeptid"].ToString() == deptAndWard["departcode"].ToString() && p["newwardid"].ToString() == deptAndWard["wardcode"].ToString()))
                        {///转科记录中不存在时，插入转科记录
                            List<OracleParameter> list = new List<OracleParameter>();
                            OracleParameter param1 = new OracleParameter("noofinpat", OracleType.Int32);
                            param1.Value = (int)m_CurrentInpatient.NoOfFirstPage;
                            list.Add(param1);
                            OracleParameter param2 = new OracleParameter("newdeptid", OracleType.VarChar);
                            param2.Value = deptAndWard["departcode"].ToString();
                            list.Add(param2);
                            OracleParameter param3 = new OracleParameter("newwardid", OracleType.VarChar);
                            param3.Value = deptAndWard["wardcode"].ToString();
                            list.Add(param3);
                            OracleParameter param4 = new OracleParameter("createuser", OracleType.VarChar);
                            param4.Value = YD_Common.currentUser.Id;
                            list.Add(param4);
                            OracleParameter param5 = new OracleParameter("createtime", OracleType.VarChar);
                            param5.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            list.Add(param5);
                            if (deptAndWard["departcode"].ToString() == YD_Common.currentUser.CurrentDeptId)
                            {
                                DataTable theInpatient = YD_SqlService.GetInpatientByID((int)m_CurrentInpatient.NoOfFirstPage);
                                if (null != theInpatient && theInpatient.Rows.Count == 1)
                                {
                                    OracleParameter param6 = new OracleParameter("newbedid", OracleType.VarChar);
                                    param6.Value = null == theInpatient.Rows[0]["outbed"] ? string.Empty : theInpatient.Rows[0]["outbed"].ToString();
                                    list.Add(param6);
                                }
                            }
                            OracleParameter param7 = new OracleParameter("changestyle", OracleType.Char);
                            param7.Value = 1;
                            list.Add(param7);
                            OracleParameter param8 = new OracleParameter("valid", OracleType.Int32);
                            param8.Value = 1;
                            list.Add(param8);
                            YD_SqlService.InsertInpChangeInfo(list);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        **/

        /// <summary>
        /// 检查是否需要插入转科记录
        /// </summary>
        private void CheckForInsertChangedDept()
        {
            try
            {
                ///获取该病人的转科(病区/床)记录
                DataTable changeDepts = DS_SqlService.GetInpatientChangeInfo((int)m_CurrentInpatient.NoOfFirstPage);
                ///获取病人信息
                DataTable inpatientDt = DS_SqlService.GetInpatientByID((int)m_CurrentInpatient.NoOfFirstPage);
                if (null == inpatientDt || inpatientDt.Rows.Count == 0)
                {
                    return;
                }
                DataRow inpatient = inpatientDt.Rows[0];
                var lastChanged = changeDepts.AsEnumerable().OrderBy(p => DateTime.Parse(p["createtime"].ToString())).LastOrDefault();
                List<OracleParameter> paras = new List<OracleParameter>();
                if (null == changeDepts || changeDepts.Rows.Count == 0)
                {///不存在转科(病区/床)记录
                    paras = GetNewChangeDeptParams(inpatient, "0");
                }
                else if (inpatient["outhosdept"].ToString().Trim() != lastChanged["newdeptid"].ToString().Trim())
                {///转科
                    paras = GetNewChangeDeptParams(inpatient, "1");
                }
                else if (inpatient["outhosward"].ToString().Trim() != lastChanged["newwardid"].ToString().Trim())
                {///转病区
                    paras = GetNewChangeDeptParams(inpatient, "2");
                }
                //else if (inpatient["outbed"].ToString().Trim() != lastChanged["newbedid"].ToString().Trim())
                //{///转床
                //    paras = GetNewChangeDeptParams(inpatient, "3");
                //}
                if (null != paras && paras.Count() > 0)
                {
                    DS_SqlService.InsertInpChangeInfo(paras);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取转科记录参数
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-05-15</date>
        /// <param name="inpatient"></param>
        /// <param name="changeStyle">转科方式：0-入科；1-转科；2-转病区；3-转床</param>
        /// <returns></returns>
        public List<OracleParameter> GetNewChangeDeptParams(DataRow inpatient, string changeStyle)
        {
            try
            {
                List<OracleParameter> parameters = new List<OracleParameter>();
                if (null == inpatient)
                {
                    return parameters;
                }

                //noofinpat  
                OracleParameter param1 = new OracleParameter("noofinpat", OracleType.Int32);
                param1.Value = int.Parse(inpatient["noofinpat"].ToString());
                parameters.Add(param1);
                //newdeptid
                OracleParameter param2 = new OracleParameter("newdeptid", OracleType.VarChar);
                param2.Value = null == inpatient["outhosdept"] ? "" : inpatient["outhosdept"].ToString();
                parameters.Add(param2);
                //newwardid
                OracleParameter param3 = new OracleParameter("newwardid", OracleType.VarChar);
                param3.Value = null == inpatient["outhosward"] ? "" : inpatient["outhosward"].ToString();
                parameters.Add(param3);
                //newbedid
                OracleParameter param4 = new OracleParameter("newbedid", OracleType.VarChar);
                param4.Value = null == inpatient["outbed"] ? "" : inpatient["outbed"].ToString();
                parameters.Add(param4);
                //changestyle
                OracleParameter param5 = new OracleParameter("changestyle", OracleType.Char);
                param5.Value = changeStyle;
                parameters.Add(param5);
                //createuser 
                OracleParameter param6 = new OracleParameter("createuser", OracleType.VarChar);
                param6.Value = DS_Common.currentUser.Id;
                parameters.Add(param6);
                //createtime 
                OracleParameter param7 = new OracleParameter("createtime", OracleType.VarChar);
                param7.Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                parameters.Add(param7);
                //valid
                OracleParameter param8 = new OracleParameter("valid", OracleType.Int32);
                param8.Value = 1;
                parameters.Add(param8);
                //olddeptid
                OracleParameter param9 = new OracleParameter("olddeptid", OracleType.VarChar);
                param9.Value = string.Empty;
                parameters.Add(param9);
                //oldwardid
                OracleParameter param10 = new OracleParameter("oldwardid", OracleType.VarChar);
                param10.Value = string.Empty;
                parameters.Add(param10);
                //oldbedid
                OracleParameter param11 = new OracleParameter("oldbedid", OracleType.VarChar);
                param11.Value = string.Empty;
                parameters.Add(param11);
                //modifyuser 
                OracleParameter param12 = new OracleParameter("modifyuser", OracleType.VarChar);
                param12.Value = string.Empty;
                parameters.Add(param12);
                //modifytime 
                OracleParameter param13 = new OracleParameter("modifytime", OracleType.VarChar);
                param13.Value = string.Empty;
                parameters.Add(param13);

                return parameters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #endregion

        #region 工具提示（病历树）
        /// <summary>
        /// 病历树 - 焦点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListEmrNodeList_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hitInfo = treeListEmrNodeList.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.Node == null || hitInfo.Node.Level < 2)
                {
                    return;
                }
                if (hitInfo.Node.Tag is EmrModel)
                {
                    EmrModel model = (EmrModel)hitInfo.Node.Tag;
                    InitToolTip(null == model ? "" : model.ModelName, treeListEmrNodeList, treeListEmrNodeList.PointToScreen(e.Location));
                }
                else if (hitInfo.Node.Tag is EmrModelContainer)
                {
                    EmrModelContainer model = (EmrModelContainer)hitInfo.Node.Tag;
                    InitToolTip(null == model ? "" : model.Name, treeListEmrNodeList, treeListEmrNodeList.PointToScreen(e.Location));
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 工具提示方法
        /// </summary>
        /// <param name="displayName">提示信息</param>
        /// <param name="tipTreeList">树控件</param>
        /// <param name="point">位置</param>
        private void InitToolTip(string displayName, TreeList tipTreeList, Point point)
        {
            try
            {
                if (null != displayName && !string.IsNullOrEmpty(displayName.Trim()))
                {
                    toolTipControllerTreeList.SetToolTip(tipTreeList, displayName);
                    toolTipControllerTreeList.SetToolTipIconType(tipTreeList, DevExpress.Utils.ToolTipIconType.Exclamation);
                    toolTipControllerTreeList.ShowBeak = true;
                    toolTipControllerTreeList.ShowShadow = true;
                    toolTipControllerTreeList.Rounded = true;
                    toolTipControllerTreeList.ShowHint(displayName, point);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 右键事件 && 展开事件
        /// <summary>
        /// 节点展开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListEmrNodeList_BeforeExpand(object sender, BeforeExpandEventArgs e)
        {
            try
            {
                TreeListNode parentNode = e.Node.ParentNode;
                TreeListNodes nodes = null == parentNode ? treeListEmrNodeList.Nodes : parentNode.Nodes;

                if (nodes != null && nodes.Count > 0)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        TreeListNode node = nodes[i];
                        if (node != e.Node)
                        {
                            node.Expanded = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 右键事件(病历树)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListEmrNodeList_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right && m_RightMouseButtonFlag)//右键弹出菜单
                {
                    TreeListHitInfo hitInfo = treeListEmrNodeList.CalcHitInfo(e.Location);
                    if (hitInfo != null && hitInfo.Node != null)
                    {
                        treeListEmrNodeList.Focus();
                        treeListEmrNodeList.FocusedNode = hitInfo.Node;
                        TreeListNode node = hitInfo.Node;
                        if (node.Tag != null && (node.Tag is EmrModelContainer || node.Tag is EmrModelDeptContainer))//文件夹
                        {
                            SetEmrModelContainerButtonVisiable(node);
                        }
                        else if (node.Tag != null && node.Tag is EmrModel)//病历
                        {
                            SetEmrModelButtonVisiable(node);
                        }
                        else if (node.Tag != null && node.Tag is InCommonNoteEnmtity)  //护理通用记录单
                        {
                            SetEmrModelButtonVisiable(node);
                        }
                        SetAuditBtnState();
                        popupMenu_EmrNode.ShowPopup(treeListEmrNodeList.PointToScreen(new Point(e.X, e.Y)));
                    }
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {

                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 设置点击病历文件夹Button的显示情况
        /// </summary>
        /// <param name="node"></param>
        private void SetEmrModelContainerButtonVisiable(TreeListNode node)
        {
            try
            {
                if (null == node || null == node.Tag)
                {
                    return;
                }

                //保存，删除(病程)，提交，审核, 取消审核，删除(护理单) 不可见
                btnRight_SaveEmr.Visibility = BarItemVisibility.Never;
                btnRight_DeleteEmr.Visibility = BarItemVisibility.Never;
                btnRight_Submit.Visibility = BarItemVisibility.Never;
                btnRight_Audit.Visibility = BarItemVisibility.Never;
                btnRight_CancelAudit.Visibility = BarItemVisibility.Never;
                btnRight_DeleteDoc.Visibility = BarItemVisibility.Never;
                btnRight_Reback.Visibility = BarItemVisibility.Never;
                ///处理新增(病历或护理单)按钮显示状态
                btnRight_NewEmr.Visibility = BarItemVisibility.Never;
                btnRight_NewDoc.Visibility = BarItemVisibility.Never;
                if (CheckIsDoctor())///医生
                {
                    if (node.Tag is EmrModelContainer)
                    {///病历(包括住院志、病程、知情文件...)
                        EmrModelContainer container = node.Tag as EmrModelContainer;
                        //在模版工厂中可维护的传染病报告和医技申请应该也可新增
                        //add by ywk 二〇一三年六月十七日 13:58:55 
                        if (container.EmrContainerType == ContainerType.Docment || container.ContainerCatalog == "AQ" || container.ContainerCatalog == "47")
                        {
                            if (container.Name == "入院记录" && node.HasChildren == true)
                            {
                                //add by Ukey 2016-08-28当容器类型名称为入院记录，并且存在文档则跳过，默认为btnRight_NewEmr.Visibility = BarItemVisibility.Never;
                            }
                            else
                            {
                                btnRight_NewEmr.Visibility = BarItemVisibility.Always;
                            }
                        }
                    }
                    else if (node.Tag is EmrModelDeptContainer)
                    {///病程科室
                        btnRight_NewEmr.Visibility = BarItemVisibility.Always;
                    }
                }
                else if (DoctorEmployee.Kind == EmployeeKind.Nurse)
                {///护理记录表格
                    EmrModelContainer container = node.Tag as EmrModelContainer;
                    if (node.Tag is EmrModelContainer && container.EmrContainerType == ContainerType.NurseDocment)
                    {
                        if (container.ContainerCatalog == ContainerCatalog.HuLiJiLuBiaoGe)
                        {
                            btnRight_NewDoc.Visibility = BarItemVisibility.Always;
                        }
                        else
                        {
                            btnRight_NewEmr.Visibility = BarItemVisibility.Always;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置点击病历模板按钮显示状态
        /// </summary>
        /// <param name="node"></param>
        private void SetEmrModelButtonVisiable(TreeListNode node)
        {
            try
            {
                if (null == node || null == node.Tag)
                {
                    return;
                }

                SetAllRightBtnState(false);
                if (node.Tag is EmrModel)
                {
                    EmrModel model = (EmrModel)node.Tag;
                    if (CheckIsDoctor() && null != CurrentForm && null != CurrentForm.CurrentEditorControl && CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel == DocumentModel.Edit)
                    {///医生
                        btnRight_SaveEmr.Visibility = BarItemVisibility.Always;
                    }
                    else if (DoctorEmployee.Kind == EmployeeKind.Nurse && CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel == DocumentModel.Edit)
                    {///护士
                        btnRight_SaveEmr.Visibility = BarItemVisibility.Always;
                    }
                    IEmrModelPermision modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Delete, DoctorEmployee);
                    bool b = modelPermision.CanDo(model);
                    btnRight_DeleteEmr.Visibility = b ? BarItemVisibility.Always : BarItemVisibility.Never;
                    DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);



                    if ((model.State == ExamineState.SubmitButNotExamine && model.ExaminerXH == App.User.Id) || (model.State == ExamineState.FirstExamine && model.ExaminerXH == App.User.Id && grade == DoctorGrade.Attending) || (model.State == ExamineState.SecondExamine && model.ExaminerXH == App.User.Id && (grade == DoctorGrade.Chief || grade == DoctorGrade.AssociateChief)))
                    {
                        if (model.ModelCatalog == "AI" || model.ModelCatalog == "AJ" || model.ModelCatalog == "AK")
                        {
                            btnRight_Reback.Visibility = BarItemVisibility.Never;
                        }
                        else
                        {
                            btnRight_Reback.Visibility = BarItemVisibility.Always;
                        }
                    }
                    else
                    {
                        btnRight_Reback.Visibility = BarItemVisibility.Never;
                    }

                }
                else if (DoctorEmployee.Kind == EmployeeKind.Nurse && node.Tag is InCommonNoteEnmtity)
                {///护理记录表格权限不控制到人(只要无数据，任何人可以删除)
                    btnRight_DeleteDoc.Visibility = BarItemVisibility.Always;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置所有右键按钮显示状态
        /// </summary>
        /// <param name="b"></param>
        private void SetAllRightBtnState(bool boo)
        {
            try
            {
                btnRight_NewEmr.Visibility = boo ? BarItemVisibility.Always : BarItemVisibility.Never;//新增病历
                btnRight_DeleteEmr.Visibility = boo ? BarItemVisibility.Always : BarItemVisibility.Never;//删除病历
                btnRight_SaveEmr.Visibility = boo ? BarItemVisibility.Always : BarItemVisibility.Never;//保存病历

                btnRight_Submit.Visibility = boo ? BarItemVisibility.Always : BarItemVisibility.Never;//提交
                btnRight_Audit.Visibility = boo ? BarItemVisibility.Always : BarItemVisibility.Never;//审核
                btnRight_CancelAudit.Visibility = boo ? BarItemVisibility.Always : BarItemVisibility.Never;//取消审核

                btnRight_NewDoc.Visibility = boo ? BarItemVisibility.Always : BarItemVisibility.Never;//新增护理单
                btnRight_DeleteDoc.Visibility = boo ? BarItemVisibility.Always : BarItemVisibility.Never;//删除护理单
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置审核相关按钮显示状态(根据工具栏按钮状态调整)
        /// </summary>
        /// <param name="boo"></param>
        private void SetAuditBtnState()
        {
            try
            {
                this.btnRight_Submit.Visibility = ucEmrInput.btnItemSubmit.Enabled ? BarItemVisibility.Always : BarItemVisibility.Never;
                this.btnRight_Audit.Visibility = ucEmrInput.btnItemAudit.Enabled ? BarItemVisibility.Always : BarItemVisibility.Never;
                this.btnRight_CancelAudit.Visibility = ucEmrInput.btnItemCancelAudit.Enabled ? BarItemVisibility.Always : BarItemVisibility.Never;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 设置默认选中病历
        /// <summary>
        /// 设置默认选中病历
        /// </summary>
        /// <param name="nodes"></param>
        public void SelectSpecifiedNode(TreeListNodes nodes)
        {
            try
            {
                if (string.IsNullOrEmpty(m_app.CurrentSelectedEmrID) || !Tool.IsInt(m_app.CurrentSelectedEmrID.Trim()))
                {
                    return;
                }
                foreach (TreeListNode node in nodes)
                {
                    if (node.Nodes.Count > 0)
                    {
                        SelectSpecifiedNode(node.Nodes);
                    }
                    else if (null != node.Tag && node.Tag is EmrModel)
                    {
                        EmrModel model = node.Tag as EmrModel;
                        if (model != null)
                        {
                            if (model.InstanceId.ToString() == m_app.CurrentSelectedEmrID)
                            {
                                FocusedNodeChanged(node);
                                ExpandedSelectNodes(node);
                                m_app.CurrentSelectedEmrID = string.Empty;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 展开选中的病历的所有父节点
        /// </summary>
        private void ExpandedSelectNodes(TreeListNode node)
        {
            try
            {
                if (null == node || null == node.ParentNode)
                {
                    return;
                }
                node.ParentNode.Expanded = true;
                ExpandedSelectNodes(node.ParentNode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region FocusedNodeChanged - 左侧病历树
        /// <summary>
        /// 焦点切换事件(病历树)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListEmrNodeList_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                FocusedNodeChanged(e.Node);
                treeListEmrNodeList.MakeNodeVisible(e.Node);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 焦点切换方法(病历树)
        /// </summary>
        public void FocusedNodeChanged(TreeListNode node)
        {
            try
            {
                //YD_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病历...");
                if (node == null || node.Tag == null)
                {
                    return;
                }
                ucEmrInput.focusedNode = node;
                string oldCatelog = MyContainerCode;
                LoadModelFromTree(node);
                ///刷新大纲视图
                RefreashEmrView();
                if (oldCatelog != MyContainerCode)
                {
                    //根据科室类别刷新科室小模板
                    InitPersonTree(false, MyContainerCode);
                }
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                DS_Common.HideWaitDialog(m_WaitDialog);
                throw ex;
            }
        }

        /// <summary>
        /// 根据节点加载病历
        /// </summary>
        /// <param name="node"></param>
        private void LoadModelFromTree(TreeListNode node)
        {
            try
            {
                if (null == node)
                {
                    return;
                }

                string containercode = string.Empty;

                #region 容器类
                if (null != node.Tag && node.Tag is EmrModelContainer)//如果是容器类
                {
                    EmrModelContainer container = (EmrModelContainer)node.Tag;
                    if (node.HasChildren || container.Name == "会诊记录")
                    {
                        if (container.Name == "入院记录")
                        {
                            ucEmrInput.BGetHasChildren(node.HasChildren);
                        }
                        ucEmrInput.ResetEditModeAction(container);
                    }
                    else if (container.ContainerCatalog == "23")//23表示PACS影像
                    {
                        try
                        {
                            DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病历...");
                            PACSOutSide.PacsAll(m_CurrentInpatient);
                        }
                        catch (Exception ex)
                        {
                            MyMessageBox.Show(3, ex);
                        }
                    }
                    else
                    {
                        //展现非病历容器内容
                        if (container.EmrContainerType == ContainerType.None)
                        {
                            DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病历...");
                            if (m_TempContainerPages.ContainsKey(container))
                            {
                                //加载内容
                                xtraTabControlEmr.SelectedTabPage = m_TempContainerPages[container];
                                Control ctl = xtraTabControlEmr.SelectedTabPage.Controls.Count == 0 ? null : xtraTabControlEmr.SelectedTabPage.Controls[0];//zyx 2012-0-6
                                if (ctl != null && ctl.GetType().FullName.Equals("DrectSoft.Core.YDNurseDocument.MainNursingMeasure"))//如果是新的体温单界面
                                {
                                    (ctl as DrectSoft.Core.NurseDocument.MainNursingMeasure).simpleButtonRefresh_Click(null, null);
                                }
                            }
                            else
                            {
                                CreateCommonDocument(container);
                            }
                        }
                        ucEmrInput.ResetEditModeAction(container);
                    }
                    containercode = container.ContainerCatalog;
                }
                else if (null != node.Tag && node.Tag is EmrModelDeptContainer)//如果是容器类
                {
                    EmrModelDeptContainer container = (EmrModelDeptContainer)node.Tag;
                    ucEmrInput.ResetEditModeAction(container);
                    containercode = container.ContaineCatalog;
                }
                #endregion

                #region 模板类
                if (null != node && null != node.Tag && !node.HasChildren && (node.Tag is EmrModel))//如果是模板类，则说明是叶子节点
                {
                    DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病历...");
                    EmrModel theModel = (EmrModel)node.Tag;
                    //if (null == theModel || theModel.ModelContent == null || string.IsNullOrEmpty(theModel.ModelContent.OuterXml))
                    //{
                    //    theModel = m_RecordDal.LoadModelInstanceNew(theModel);
                    //}
                    if (null != theModel && m_TempModelPages.ContainsKey(theModel))
                    {
                        xtraTabControlEmr.SelectedTabPage = m_TempModelPages[theModel];
                    }
                    else
                    {
                        CreateNewDocument(theModel);
                    }

                    if (null != CurrentForm)
                    {
                        CurrentForm.ResetEditModeState(theModel);
                    }
                    ucEmrInput.ResetEditModeAction(theModel);

                    if (null != node.ParentNode && null != node.ParentNode.Tag)
                    {
                        if (node.ParentNode.Tag is EmrModelContainer)
                        {
                            containercode = ((EmrModelContainer)node.ParentNode.Tag).ContainerCatalog;
                        }
                        else if (node.ParentNode.Tag is EmrModelDeptContainer)
                        {
                            containercode = ((EmrModelDeptContainer)node.ParentNode.Tag).ContaineCatalog;
                        }
                    }
                }
                #endregion

                #region 护理记录表格 commonNote
                if (node.Tag is InCommonNoteEnmtity)
                {
                    DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载护理记录表格...");
                    InCommonNoteEnmtity inCommonNote = node.Tag as InCommonNoteEnmtity;
                    XtraTabPage tabpage = null;

                    foreach (XtraTabPage item in xtraTabControlEmr.TabPages)
                    {
                        if (item.Tag is InCommonNoteEnmtity && (item.Tag as InCommonNoteEnmtity).InCommonNoteFlow == inCommonNote.InCommonNoteFlow)
                        {
                            tabpage = item;
                            break;
                        }
                    }
                    if (tabpage == null)
                    {
                        CommonNoteBiz commonNoteBiz = new DrectSoft.Core.CommonTableConfig.CommonNoteBiz(m_app);
                        CommonNoteEntity commonNoteEntity = commonNoteBiz.GetDetailCommonNote(inCommonNote.CommonNoteFlow);
                        tabpage = new XtraTabPage();
                        tabpage.Text = inCommonNote.InCommonNoteName;
                        tabpage.Tag = inCommonNote;
                        bool canEdit = true;
                        if (this.floaderState == FloderState.Nurse
                            || (this.floaderState == FloderState.Default && DoctorEmployee.Kind == EmployeeKind.Nurse)
                             || (this.floaderState == FloderState.NoneAudit && DoctorEmployee.Kind == EmployeeKind.Nurse))
                        {
                            DataTable dt = DS_SqlService.GetInpatientByID((int)CurrentInpatient.NoOfFirstPage);
                            if (dt == null || dt.Rows.Count <= 0)
                            {
                                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("病人不存在");
                                return;
                            }
                            string deptID = dt.Rows[0]["outhosdept"].ToString();
                            if (deptID != inCommonNote.CurrDepartID)
                            {
                                canEdit = false;
                            }
                        }
                        else
                        {
                            canEdit = false;
                        }

                        UCInCommonNote ucInCommonNote = new UCInCommonNote(m_app, commonNoteEntity, inCommonNote, canEdit);
                        tabpage.Controls.Add(ucInCommonNote);
                        ucInCommonNote.Dock = DockStyle.Fill;
                        xtraTabControlEmr.TabPages.Add(tabpage);

                    }
                    xtraTabControlEmr.SelectedTabPage = tabpage;

                    //SetDailyEmrDoc(node);
                }
                #endregion

                MyContainerCode = containercode;

                //根据编辑器开关控制编辑器的编辑情况
                SetEditorNonEnable();

                //填充宏 当可以编辑病历时进行宏替换 Add By wwj 2012-05-14
                //if (barButtonSave.Enabled == true) FillModelMacro(CurrentForm);

                //将焦点置到编辑器中
                if (CurrentForm != null)
                {
                    //无修改则不提示保存
                    CurrentForm.CurrentEditorControl.EMRDoc.Modified = false;
                }

                #region 此处代表修改，先判断是否设置了留痕，如果设置了就直接可以开启了
                string config = DS_SqlService.GetConfigValueByKey("IsOpenOwnerSign");
                string isopenOwnerSign = string.IsNullOrEmpty(config) ? "0" : config;
                if (isopenOwnerSign == "1")
                {
                    if (CurrentForm != null)
                    {
                        this.CurrentForm.CurrentEditorControl.EMRDoc.SaveLogs.CurrentSaveLog.UserName = m_app.User.Name; //这里传入用户名
                        this.CurrentForm.CurrentEditorControl.EMRDoc.SaveLogs.CurrentSaveLog.Level = 1;         //这里传入用户级别
                        this.CurrentForm.CurrentEditorControl.EMRDoc.Content.UserLevel = 1;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建病历相关界面
        /// </summary>
        /// <param name="model"></param>
        public void CreateNewDocument(EmrModel model)
        {
            try
            {
                if (null == model)
                {
                    return;
                }
                //判断是否需要添加
                if (!m_TempModelPages.ContainsKey(model))
                {
                    //判断当前模型是否需要在新建tabpage页
                    XtraTabPage newPad = null;
                    //如果未实现则新建一个tabpage
                    if (newPad == null)
                    {
                        newPad = xtraTabControlEmr.TabPages.Add();
                        newPad.ShowCloseButton = DefaultBoolean.True;
                        EditorForm pad = new EditorForm(m_CurrentInpatient, m_app);
                        pad.DeptChangeID = model.DeptChangeID;
                        pad.Dock = DockStyle.Fill;
                        newPad.Controls.Add(pad);
                        if (model.ModelContent == null && model.InstanceId == -1)//不是新增就不用重进此方法
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.PreserveWhitespace = true;
                            string emrContent = m_PatUtil.GetTemplateContent(model.TempIdentity, model.PersonalTemplate);
                            if (!string.IsNullOrEmpty(emrContent))
                            {
                                emrContent = Util.ProcessEmrContent(emrContent);
                                doc.LoadXml(emrContent);
                                //`````zhangbin 7-29

                                //``````````
                                model.ModelContent = doc;
                            }

                        }

                        //if (model.DailyEmrModel && !model.FirstDailyEmrModel && model.InstanceId == -1)
                        //{///替换模板标题(病程时间) - 非首次病程在加载之前
                        //    pad.InsertDisplayTimeAndTitle(model.ModelContent, model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"), model.Description);
                        //}
                        m_RecordDal.LoadModelInstanceNew(model);
                        if (model.Valid == false)
                        {
                            string userNameAndID = DS_BaseService.GetUserNameAndID(model.CreatorXH);
                            MyMessageBox.Show("该病历已经被 " + userNameAndID + " 删除，您无权操作。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                            DeleteTabPageAndNode(model);
                            newPad.Controls.Remove(pad);
                            xtraTabControlEmr.TabPages.Remove(newPad);
                            return;
                        }
                        pad.LoadDocument(model.ModelContent.OuterXml);
                        if ((model.FirstDailyEmrModel && model.InstanceId == -1) || (model.DailyEmrModel && !model.FirstDailyEmrModel && model.InstanceId == -1))
                        {///替换模板标题(病程时间) - 首次病程在加载之后
                            bool result = pad.ReplaceFirstDailyTitle(model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"), model.Description);
                            if (!result)
                            {

                                pad.InsertDisplayTimeAndTitle(model.ModelContent, model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"), model.Description);
                                pad.LoadDocument(model.ModelContent.OuterXml);
                            }
                        }
                        if (model.InstanceId == -1)
                        {
                            pad.ReplaceModelMacro(model.ModelName);
                        }
                        pad.SetReplaceEvent(model.ModelName);
                        if (model.IsReadConfigPageSize)//是否需要读取配置中的页面设置
                        {
                            pad.SetDefaultPageSetting();
                        }
                        else
                        {
                            pad.SetActualPageSetting();
                        }

                        newPad.Tag = model;
                        m_CurrentModel = model;
                        newPad.Text = model.ModelName;//显示简短的文件名
                        m_TempModelPages.Add(model, newPad);
                        xtraTabControlEmr.SelectedTabPage = newPad;//触发SelectedPage事件
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新病历相关界面
        /// </summary>
        /// <param name="model"></param>
        /// <param name="thePad">需要更新的TabPage</param>
        public void UpdateDocument(XtraTabPage thePad, EmrModel model)
        {
            try
            {
                if (null == model)
                {
                    return;
                }
                //判断是否需要更新
                if (m_TempModelPages.Any(p => p.Value == thePad))
                {
                    thePad = m_TempModelPages.FirstOrDefault(p => p.Value == thePad).Value;
                    if (null == thePad)
                    {
                        return;
                    }
                    EditorForm pad = thePad.Controls[0] as EditorForm;
                    pad.DeptChangeID = model.DeptChangeID;
                    pad.Dock = DockStyle.Fill;

                    if (model.ModelContent == null)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.PreserveWhitespace = true;
                        string emrContent = m_PatUtil.GetTemplateContent(model.TempIdentity, model.PersonalTemplate);
                        emrContent = Util.ProcessEmrContent(emrContent);
                        doc.LoadXml(emrContent);
                        model.ModelContent = doc;
                    }
                    //if (model.DailyEmrModel && !model.FirstDailyEmrModel && model.InstanceId == -1)
                    //{///替换模板标题(病程时间) - 非首次病程在加载之前
                    //    pad.InsertDisplayTimeAndTitle(model.ModelContent, model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"), model.Description);
                    //}
                    m_RecordDal.LoadModelInstanceNew(model);
                    pad.LoadDocument(model.ModelContent.OuterXml);
                    if ((model.FirstDailyEmrModel && model.InstanceId == -1) || (model.DailyEmrModel && !model.FirstDailyEmrModel && model.InstanceId == -1))
                    {///替换模板标题(病程时间) - 首次病程在加载之后
                        bool result = pad.ReplaceFirstDailyTitle(model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"), model.Description);
                        if (!result)
                        {

                            pad.InsertDisplayTimeAndTitle(model.ModelContent, model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"), model.Description);
                            pad.LoadDocument(model.ModelContent.OuterXml);
                        }
                    }
                    //if (model.InstanceId == -1)
                    {
                        pad.ReplaceModelMacro(model.ModelName);
                    }
                    pad.SetReplaceEvent(model.ModelName);
                    if (model.IsReadConfigPageSize)//是否需要读取配置中的页面设置
                    {
                        pad.SetDefaultPageSetting();
                    }
                    else
                    {
                        pad.SetActualPageSetting();
                    }

                    thePad.Tag = model;
                    m_CurrentModel = model;
                    thePad.Text = model.ModelName;//显示简短的文件名
                    xtraTabControlEmr.SelectedTabPage = thePad;//触发SelectedPage事件
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据编辑器开关控制编辑器的编辑情况
        /// </summary>
        private void SetEditorNonEnable()
        {
            try
            {
                if (!m_EditorEnableFlag)
                {
                    //SetSetEmrDocNotEdit();
                    if (CurrentForm != null)
                    {
                        CurrentForm.CurrentEditorControl.EMRDoc.Info.DocumentModel = DocumentModel.Read;
                        SetDocmentReadOnlyMode();
                        CurrentForm.CurrentEditorControl.EMRDoc.Info.ShowParagraphFlag = false;
                        CurrentForm.CurrentEditorControl.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 定位节点(获取焦点)
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="changeID"></param>
        /// <param name="captionDateTime"></param>
        public void SetNodeFocused(TreeListNodes nodes, string changeID, string captionDateTime)
        {
            try
            {
                if (null == nodes || string.IsNullOrEmpty(captionDateTime))
                {
                    return;
                }
                foreach (TreeListNode node in nodes)
                {
                    if (null != node.Tag && node.Tag is EmrModel)
                    {
                        EmrModel modelevey = (EmrModel)node.Tag;
                        if (modelevey.DailyEmrModel && modelevey.DeptChangeID == changeID && modelevey.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == captionDateTime)
                        {
                            this.treeListEmrNodeList.FocusedNode = node;
                        }
                    }
                    else if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        SetNodeFocused(node.Nodes, changeID, captionDateTime);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SelectedPageChanged - Tab页切换
        /// <summary>
        /// TabPageControl中当前选中页改变
        /// </summary>
        /// <param name="page"></param>
        public void SelectedPageChanged(XtraTabPage page)
        {
            try
            {
                if (page == null)
                {
                    return;
                }
                //找到 当前病历EmrModel 或 非病历类型的界面IEmrEditor
                GetCurrentEmrModel(ref m_CurrentModel, ref m_CurrentEmrEditor, page);
                m_CurrentModelContainer = null;
                if (m_CurrentModel != null || m_CurrentEmrEditor != null)
                {
                    //通过非病历类型的界面IEmrEditor 找到 EmrModelContainer
                    GetEmrEditorContainer(m_CurrentEmrEditor);
                    //得到当前病历对应的树节点 m_CurrentTreeListNode
                    GetCurrentTreeListNode(null, m_CurrentModel, m_CurrentModelContainer);
                }

                if (m_CurrentModel == null && m_CurrentEmrEditor == null && m_CurrentModelContainer == null)
                {
                    if (treeListEmrNodeList.Nodes.Count > 0)
                    {
                        treeListEmrNodeList.FocusedNode = treeListEmrNodeList.Nodes[0];
                    }
                }
                else if (m_CurrentTreeListNode != null)
                {
                    UnRegisterEvent();
                    m_CurrentTreeListNode.Visible = true;
                    treeListEmrNodeList.FocusedNode = m_CurrentTreeListNode;
                    ucEmrInput.focusedNode = treeListEmrNodeList.FocusedNode;
                    treeListEmrNodeList.MakeNodeVisible(m_CurrentTreeListNode);
                    RegisterEvent();
                }

                if (m_CurrentModel != null)
                {
                    ucEmrInput.ResetEditModeAction(m_CurrentModel);
                }
                else if (m_CurrentModelContainer != null)
                {
                    ucEmrInput.ResetEditModeAction(m_CurrentModelContainer);
                }

                if (m_CurrentModel != null && m_CurrentModel.ModelCatalog == ContainerCatalog.BingChengJiLu)//Add by wwj 2013-04-27 增加针对病程的判断
                {
                    ///1、如果是编辑病历，且该科室文件夹下已保存节点数大于1时，显示预览区
                    TreeListNode focusedNode = treeListEmrNodeList.FocusedNode;
                    List<EmrModel> savedModelList = GetSavedEmrModelsInFloader(focusedNode);
                    List<DataRow> savedModelListDB = GetSavedEmrModelsInDB(focusedNode, (int)m_CurrentInpatient.NoOfFirstPage);
                    if ((null != m_CurrentModel && m_CurrentModel.InstanceId != -1
                        && null != focusedNode && null != focusedNode.ParentNode && focusedNode.ParentNode.Tag is EmrModelDeptContainer
                        && focusedNode.ParentNode.Nodes.Count > 1 && savedModelList.Count >= 1 && savedModelListDB.Count() >= 1)
                        || m_TempDailyPreViewCollection.ContainsKey(m_CurrentModel.DeptChangeID))
                    {
                        AddDailyEmrPreView(m_CurrentModel, xtraTabControlEmr.SelectedTabPage);
                    }
                    ///2、如果是新增病历，且需要显示病程预览区,并且且该科室文件夹下已保存节点数大于1时，显示预览区
                    else if ((m_CurrentModel != null && m_CurrentModel.InstanceId == -1 && m_CurrentModel.IsShowDailyEmrPreView
                        && focusedNode.ParentNode.Tag is EmrModelDeptContainer && (focusedNode.ParentNode.Nodes.Count >= 2 && savedModelList.Count >= 1 && savedModelListDB.Count() >= 1))
                        || m_TempDailyPreViewCollection.ContainsKey(m_CurrentModel.DeptChangeID))
                    {
                        string deptChangeID = m_CurrentModel.DeptChangeID;
                        string displayTime = m_CurrentModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                        AddDailyEmrPreView(m_CurrentModel, xtraTabControlEmr.SelectedTabPage);
                        if (null != m_TempDailyPreViewCollection && m_TempDailyPreViewCollection.Count() > 0)
                        {
                            UCEmrInputPreView preView = m_TempDailyPreViewCollection[deptChangeID];
                            String captionDatetime = preView.PreViewInner.CurrentEditorForm.SetCurrentElementNearest(displayTime);//新增病程时，预览区中的病历定位到与新增病程最近的那份病程
                            preView.PreViewInner.SelectNodeByCaptionDatetime(captionDatetime);
                        }
                    }
                }

                ///刷新大纲视图
                RefreashEmrView();

                //根据科室类别刷新科室小模板
                if (m_CurrentModel != null)
                {
                    MyContainerCode = m_CurrentModel.ModelCatalog;
                    InitPersonTree(false, m_CurrentModel.ModelCatalog);
                }

                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 得到当前EmrModel或EmrModelContainer对应的树节点m_CurrentTreeListNode
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="emrModel"></param>
        /// <param name="emrModelContainer"></param>
        public void GetCurrentTreeListNode(TreeListNodes nodes, EmrModel emrModel, EmrModelContainer emrModelContainer)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = treeListEmrNodeList.Nodes;
                    m_CurrentTreeListNode = null;
                }

                if (m_CurrentTreeListNode != null) return;

                foreach (TreeListNode subNode in nodes)
                {
                    if (subNode.Tag is EmrModel && (EmrModel)subNode.Tag == emrModel)
                    {
                        m_CurrentTreeListNode = subNode;
                    }
                    else if (subNode.Tag is EmrModelContainer && (EmrModelContainer)subNode.Tag == emrModelContainer)
                    {
                        m_CurrentTreeListNode = subNode;
                    }
                    GetCurrentTreeListNode(subNode.Nodes, emrModel, emrModelContainer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到处于激活状态的病历（EmrModel 或 IEMREditor）
        /// </summary>
        /// <param name="emrModel"></param>
        /// <param name="emrEditor">非病历类型的界面 如首页、三测单之类的内容</param>
        /// <param name="page"></param>
        public void GetCurrentEmrModel(ref EmrModel emrModel, ref IEMREditor emrEditor, XtraTabPage page)
        {
            try
            {
                emrModel = null;
                emrEditor = null;

                if (null != m_TempModelPages && m_TempModelPages.Count() > 0)
                {
                    //当前激活的TabPage中包含的EmrModel
                    foreach (KeyValuePair<EmrModel, XtraTabPage> item in m_TempModelPages)
                    {
                        if (item.Value == page)
                        {
                            emrModel = item.Key;
                        }
                    }
                }

                //当前激活的TabPage中包含的IEMREditor
                if (page.Controls.Count > 0)
                {
                    if (page.Controls[0] is IEMREditor)
                    {
                        emrEditor = page.Controls[0] as IEMREditor;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取当前m_TempContainerPages中emrEditor对应的EmrModelContainer
        /// </summary>
        /// <param name="emrEditor"></param>
        public void GetEmrEditorContainer(IEMREditor emrEditor)
        {
            try
            {
                if (emrEditor == null) return;

                if (null == m_TempContainerPages || m_TempContainerPages.Count == 0)
                {
                    return;
                }
                foreach (KeyValuePair<EmrModelContainer, XtraTabPage> item in m_TempContainerPages)
                {
                    if (null != item.Value && item.Value.Controls[0] == emrEditor)
                    {
                        m_CurrentModelContainer = item.Key;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 大纲视图
        private void panelContainerLeft_ActiveChildChanged(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            try
            {
                RefreashEmrView();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void RefreashEmrView()
        {
            try
            {
                if (panelContainerLeft.ActiveChild == dockPanelLeft2)
                {
                    treeListEmrView.Nodes.Clear();
                    CurrentInputTabPages.GetBrief(treeListEmrView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void treeListEmrView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode node = this.treeListEmrView.SelectedNode;
                CurrentInputTabPages.AfterSelected(node);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 关于会诊&&诊疗时间轴
        /// <summary>
        /// 关于诊疗时间轴 
        /// add by ywk 2013年7月23日 18:06:58 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private List<EmrModelContainer> GetPatCureRecrod(int noofinpat, DataRow mrow)
        {
            try
            {
                DataTable dt = DS_SqlService.GetPatCureRecrod(noofinpat);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return new List<EmrModelContainer>();
                }
                List<EmrModelContainer> list = new List<EmrModelContainer>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    mrow["CNAME"] = dt.Rows[i]["patname"].ToString();
                    EmrModelContainer container = new EmrModelContainer(mrow);
                    //container.Args = dt.Rows[i]["noofinpat"].ToString();
                    list.Add(container);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取会诊记录
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public List<EmrModelContainer> GetConsultRecrod(int noofinpat, DataRow dr)
        {
            try
            {
                DataTable dt = DS_SqlService.GetConsultRecrod(noofinpat);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return new List<EmrModelContainer>();
                }
                List<EmrModelContainer> list = new List<EmrModelContainer>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr["CNAME"] = dt.Rows[i]["cname"].ToString();
                    EmrModelContainer container = new EmrModelContainer(dr);
                    container.Args = dt.Rows[i]["consultapplysn"].ToString();
                    list.Add(container);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 右侧模块（工具箱、临床数据提取、小模板列表）
        #region 切换事件
        /// <summary>
        /// Dock标签页切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelContainerRight_ActiveChildChanged(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            try
            {
                if (panelContainerRight.ActiveChild == dockPanelRight1)//工具箱
                {
                    ///图库
                    navBarControlTool.ActiveGroup = this.navBarGroupPicture;
                    navBarGroupPicture.Expanded = true;
                    ///添加提示
                    ShowTipForNavBarGroup(navBarGroupPicture);
                }
                else if (panelContainerRight.ActiveChild == dockPanelRight2)///临床数据
                {
                    ///病历提取
                    navBarControlTool.ActiveGroup = this.navBarGroupCollect;
                    ///检验数据

                    //navBarGroupLis.Expanded = true;
                    /////初始化检验数据
                    // ActiveGroup(navBarGroupLis);

                    //xll 2013-08-15 默认选中病历提取功能 避免检验检查被屏蔽后还查找检验数据
                    navBarGroupCollect.Expanded = true;
                    ActiveGroup(navBarGroupCollect);
                    ShowTipForNavBarGroup(navBarGroupLis);
                }
                else if (panelContainerRight.ActiveChild == dockPanelRight3)
                {

                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "3")
                {
                    MyMessageBox.Show(3, ex);
                }
                else if (ex.Message == "4")
                {
                    MyMessageBox.Show(4, ex);
                }
                else
                {
                    MyMessageBox.Show(1, ex);
                }
            }
        }

        /// <summary>
        /// 工具箱 - 子模块切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarControlTool_ActiveGroupChanged(object sender, NavBarGroupEventArgs e)
        {
            try
            {
                ///增加NavBarGroup的提示
                ShowTipForNavBarGroup(e.Group);

                if (e.Group == navBarGroupPicture)
                {///图片库
                    if (treeListPicture.Nodes.Count < 1)
                    {
                        treeListPicture.BeginUnboundLoad();
                        foreach (DataRow row in m_PatUtil.ImageGallery.Rows)
                        {
                            TreeListNode node = treeListPicture.AppendNode(new object[] { row["名称"] }, null);
                            node.Tag = row;
                        }
                        treeListPicture.EndUnboundLoad();
                    }
                }
                else if (e.Group == navBarGroupZD)
                {///常用字典
                    gridControlZD.DataSource = m_PatUtil.ZDItemData;
                }
                else if (e.Group == navBarGroupCYC)
                {///常用词
                    if (treeListCYC.Nodes.Count < 1)
                    {
                        treeListCYC.BeginUnboundLoad();
                        foreach (DataRow row in m_PatUtil.Macro.Rows)
                        {
                            TreeListNode node = treeListCYC.AppendNode(new object[] { row["名称"] }, null);
                            node.Tag = row;
                        }
                        treeListCYC.EndUnboundLoad();
                    }
                }
                else if (e.Group == navBarGroupTZ)
                {///体征
                    if (treeListTZ.Nodes.Count < 1)
                    {
                        treeListTZ.BeginUnboundLoad();
                        foreach (DataRow row in m_PatUtil.TZItemTemplate.Rows)
                        {
                            TreeListNode node = treeListTZ.AppendNode(new object[] { row["名称"] }, null);
                            node.Tag = row;
                        }
                        treeListTZ.EndUnboundLoad();
                    }
                }
                else if (e.Group == navBarGroupSymbol)
                {///特殊字符
                    BindSymbol();
                }
                else if (e.Group == navBarGroupZZ)
                {///症状
                    if (treeListZZ.Nodes.Count < 1)
                    {
                        treeListZZ.BeginUnboundLoad();
                        foreach (DataRow row in m_PatUtil.ZZItemData.Rows)
                        {
                            TreeListNode node = treeListZZ.AppendNode(new object[] { row["名称"] }, null);
                            node.Tag = row;
                        }
                        treeListZZ.EndUnboundLoad();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 临床数据 - 子模块切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarControlClinic_ActiveGroupChanged(object sender, NavBarGroupEventArgs e)
        {
            try
            {
                //增加NavBarGroup的提示
                ShowTipForNavBarGroup(e.Group);

                ActiveGroup(e.Group);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 临床数据 - 子模块切换方法
        /// </summary>
        /// <param name="activeGroup"></param>
        private void ActiveGroup(NavBarGroup activeGroup)
        {
            try
            {
                if (activeGroup == navBarGroupCollect)
                {///病历提取
                    InitBLCollect();
                }
                else if (activeGroup == navBarGroupEmrInfo)
                {///病历信息
                    InitBLMessage();
                }
                else if (activeGroup == navBarGroupLis)
                {///检验数据
                    try
                    {
                        if (IsOpenLisdata.Trim() == "0")
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该检验数据提取功能未开启");
                            return;
                        }
                        if (IsShowLis)//只有显示LIS才会走下面的过程 add by ywk 2013年6月13日 15:58:37 上次控制LIS 显示与否漏掉了
                        {
                            if (m_PatUtil.LisReports != null)
                            {
                                ///存在该列则显示(出检日期)
                                gcReleaseDate.Visible = m_PatUtil.LisReports.Columns.Contains("RELEASEDATE");
                                ///设置数据集
                                gridControlLIS.DataSource = m_PatUtil.LisReports;
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("LIS错误" + ex.Message);
                    }
                }
                else if (activeGroup == navBarGroupPacs) //检查数据
                {
                    try
                    {
                        if (IsOpenpacsdata.Trim() == "0")
                        {
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该检查数据提取功能未开启");
                            return;
                        }
                        if (m_PatUtil.PacsReports != null)
                        {
                            gridControlPacs.DataSource = m_PatUtil.PacsReports;
                        }
                    }
                    catch (Exception)
                    {
                        throw new Exception("3");
                    }
                }
                else if (activeGroup == navBarGroupDoctorAdive)   //医嘱数据
                {
                    if (IsOpenorderdata.Trim() == "0")
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该医嘱数据提取功能未开启");
                        return;
                    }
                    if (IsShowDoc)
                    {
                        if (m_PatUtil.Orders != null)
                        {
                            gridControlDoctorAdive.DataSource = m_PatUtil.Orders;
                        }
                    }
                }
                else if (activeGroup == navBarGroupEmrHostory)
                {///历史病历信息
                    InitTreeListHistory(Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ===小模板列表===========================
        /// <summary>
        /// 科室小模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_DeptTemplete_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (m_PersonItem == null)
                {
                    m_PersonItem = new PersonItemManager(m_app);
                }
                m_PersonItem.ShowDialog();
                //InitPersonTree(true, string.Empty);
                //此处修改，科室小模板窗体关闭后，原来的右侧清空了，现在更改为加载进来时的科室模板列表，声明了属性记录窗体加载时的模板值。add by  ywk 2012年6月11日 15:58:45
                InitPersonTree(true, MyContainerCode);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 加载小模板列表
        /// </summary>
        /// <param name="refresh">是否刷新</param>
        /// <param name="catalog"></param>
        public void InitPersonTree(bool refresh, string catalog)
        {
            try
            {
                Thread personTreeThread = new Thread(new ParameterizedThreadStart(InitPersonTreeInner));
                personTreeThread.Start(new ParameterObject { Refresh = refresh, Catalog = catalog });
                //InitPersonTreeInner(new ParameterObject { Refresh = refresh, Catalog = catalog });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        class ParameterObject
        {
            public bool Refresh { get; set; }
            public string Catalog { get; set; }
        }

        private void InitPersonTreeInner(Object parameterObj)//(bool refresh, string catalog)
        {
            try
            {
                bool refresh = ((ParameterObject)parameterObj).Refresh;
                string catalog = ((ParameterObject)parameterObj).Catalog;
                if ((null == m_MyTreeFolders) || (refresh))
                {
                    m_MyTreeFolders = m_PatUtil.GetPersonTempleteFloder(DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.Id);
                    m_MyLeafs = m_PatUtil.GetPersonTemplete(DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.Id);
                }
                InitPersonTreeInvoke(catalog);
            }
            catch (Exception)
            {
            }
        }

        private void InitPersonTreeInvoke(string catalog)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<string>(InitPersonTreeInvoke), catalog);
                }
                else
                {
                    DataRow[] rows = m_MyTreeFolders.Select("(Previd='' or Previd is null)  and (container = '99' or container ='" + catalog + "')");
                    treeListPersonTemplate.BeginUnboundLoad();
                    treeListPersonTemplate.Nodes.Clear();
                    foreach (DataRow dr in rows)
                    {
                        LoadTree(dr["ID"].ToString(), dr, null, catalog);
                    }
                    treeListPersonTemplate.CollapseAll();
                    treeListPersonTemplate.EndUnboundLoad();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 小模板列表 - 加载树
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="node"></param>
        /// <param name="catalog"></param>
        private void LoadTree(string id, DataRow row, TreeListNode node, string catalog)
        {
            try
            {
                TreeListNode nd = treeListPersonTemplate.AppendNode(new object[] { id, row["NAME"], "Folder", row["ISPERSON"], row["CREATEUSERS"] }, node);
                nd.Tag = row;
                //先进行下非空验证edit by ywk 2013年7月22日 16:45:11
                if (m_MyLeafs == null)//如果为空
                {
                    m_MyLeafs = DS_SqlService.GetPersonTemplete(DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.Id);
                }

                //查找叶子节点
                DataRow[] leafRows = m_MyLeafs.Select("PARENTID='" + id + "' ");
                if (leafRows.Length > 0)
                {
                    foreach (DataRow leaf in leafRows)
                    {
                        TreeListNode leafnd = treeListPersonTemplate.AppendNode(new object[] { leaf["Code"], leaf["NAME"], "Leaf", leaf["ISPERSON"], leaf["CREATEUSERS"] }, nd);
                        TempletItem item = new TempletItem(leaf);
                        if (item.Content.Contains("||chr(10)||chr(13)||"))//存在换行，替代插入数据库中 edit by ywk
                        {
                            item.Content = item.Content.Replace("'||chr(10)||chr(13)||'", "\r\n");
                        }
                        item.CatalogName = row["NAME"].ToString();
                        leafnd.Tag = item;
                    }
                }

                DataRow[] rows = m_MyTreeFolders.Select("Previd='" + id + "' ");
                foreach (DataRow dr in rows)
                {
                    LoadTree(dr["ID"].ToString(), dr, nd, catalog);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 小模板列表 - 设置文件图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListPersonTemplate_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            try
            {
                TreeListNode node = e.Node;
                if (null != node.GetValue("NODETYPE") && node.GetValue("NODETYPE").ToString() == "Folder")
                {
                    if (node.Expanded)//文件夹打开
                    {
                        e.NodeImageIndex = 3;
                    }
                    else//文件夹未打开
                    {
                        e.NodeImageIndex = 2;
                    }
                }
                else
                {
                    e.NodeImageIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 小模板列表 - 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchTemplete_Click(object sender, EventArgs e)
        {
            try
            {
                SearchPersonTree(false, MyContainerCode, this.txt_SearchTemplete.Text);
                if (!string.IsNullOrEmpty(this.txt_SearchTemplete.Text))
                {
                    foreach (TreeListNode node in treeListPersonTemplate.Nodes)
                    {
                        node.ExpandAll();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 搜索模板树
        /// by cyq 2012-09-25
        /// </summary>
        /// <param name="refresh">是否从数据库刷新数据</param>
        /// <param name="catalog">模板‘病历类别’</param>
        /// <param name="keyword">搜索关键字</param>
        private void SearchPersonTree(bool refresh, string catalog, string keyword)
        {
            try
            {
                if ((null == m_MyTreeFolders) || (refresh))
                {
                    m_MyTreeFolders = DS_SqlService.GetPersonTempleteFloder(DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.Id);
                    m_MyLeafs = DS_SqlService.GetPersonTemplete(DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.Id);
                }
                //所有符合搜索条件的模板集合
                var serchedLeafs = m_MyLeafs.AsEnumerable().Where(p => p["Name"].ToString().Contains(keyword));
                IEnumerable<DataRow> allFolders = m_MyTreeFolders.AsEnumerable();
                //所有符合搜索条件的分类ID集合
                List<string> list = new List<string>();
                foreach (DataRow leaf in serchedLeafs)
                {
                    list = GetSearchParentIDs(list, allFolders, leaf, 0);
                }
                var allSearchedFolders = m_MyTreeFolders.AsEnumerable().Where(p => list.Contains(p["ID"].ToString()));
                var allSearchedFirstFolders = allSearchedFolders.Where(p => (null == p["Previd"] || p["Previd"].ToString() == "") && (p["container"].ToString() == "99" || p["container"].ToString() == catalog));
                treeListPersonTemplate.BeginUnboundLoad();
                treeListPersonTemplate.Nodes.Clear();
                foreach (DataRow dr in allSearchedFirstFolders)
                {
                    SearchLoadTree(dr, null, catalog, allSearchedFolders, serchedLeafs, keyword);
                }
                treeListPersonTemplate.CollapseAll();
                treeListPersonTemplate.EndUnboundLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据模板获取其对应的所有分类ID
        /// </summary>
        /// <param name="list">分类ID结果集</param>
        /// <param name="allFolders">所有分类集合</param>
        /// <param name="leaf">模板</param>
        /// <param name="childFlag">0-模板；1-分类</param>
        /// <returns></returns>
        private List<string> GetSearchParentIDs(List<string> list, IEnumerable<DataRow> allFolders, DataRow leaf, int childFlag)
        {
            try
            {
                var parentFolders = allFolders.Where(p => p["ID"].ToString() == leaf[childFlag == 0 ? "PARENTID" : "PREVID"].ToString());
                if (parentFolders.Count() > 0)
                {
                    var parentFolder = parentFolders.FirstOrDefault();
                    if (!list.Contains(parentFolder["ID"].ToString()))
                    {
                        list.Add(parentFolder["ID"].ToString());
                        if (null != parentFolder["PREVID"] && parentFolder["PREVID"].ToString() != "")
                        {
                            list = GetSearchParentIDs(list, allFolders, parentFolder, 1);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">分类</param>
        /// <param name="node">此分类的父节点</param>
        /// <param name="catalog">模板‘病历类别’</param>
        /// <param name="keyword">搜索关键字</param>
        private void SearchLoadTree(DataRow row, TreeListNode node, string catalog, IEnumerable<DataRow> allSearchedFolders, IEnumerable<DataRow> searchedLeafs, string keyword)
        {
            try
            {
                TreeListNode nd = treeListPersonTemplate.AppendNode(new object[] { row["ID"], row["NAME"], "Folder", row["ISPERSON"], row["CREATEUSERS"] }, node);
                nd.Tag = row;

                //查找叶子节点
                var leafRows = searchedLeafs.Where(p => p["PARENTID"].ToString() == row["ID"].ToString());
                if (null != leafRows && leafRows.Count() > 0)
                {
                    foreach (DataRow leaf in leafRows)
                    {
                        TreeListNode leafnd = treeListPersonTemplate.AppendNode(new object[] { leaf["Code"], leaf["NAME"], "Leaf", leaf["ISPERSON"], leaf["CREATEUSERS"] }, nd);
                        TempletItem item = new TempletItem(leaf);
                        item.CatalogName = row["NAME"].ToString();
                        leafnd.Tag = item;
                    }
                }

                var rows = allSearchedFolders.Where(p => p["Previd"].ToString() == row["ID"].ToString());
                foreach (DataRow dr in rows)
                {
                    SearchLoadTree(dr, nd, catalog, allSearchedFolders, searchedLeafs, keyword);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion===

        #region ===加载病历 Modify by xlb 2013-06-21====

        /// <summary>
        /// 加载病历提取信息
        /// </summary>
        public void InitBLCollect()
        {
            try
            {
                treeListEmrCollect.Nodes.Clear();
                if (CurrentForm == null)
                {
                    return;
                }
                DataTable dt = new DataTable();
                DataColumn dcName = new DataColumn("ID", Type.GetType("System.String"));
                DataColumn dcContent = new DataColumn("CONTENT", Type.GetType("System.String"));
                dt.Columns.Add(dcName);
                dt.Columns.Add(dcContent);

                DataTable blDt = m_PatUtil.GetDocCollectContent();
                if (null == blDt || blDt.Rows.Count == 0)
                {
                    return;
                }
                foreach (DataRow row in blDt.Rows)
                {
                    DataRow newrow = dt.NewRow();
                    newrow["ID"] = row["ID"];
                    newrow["CONTENT"] = row["CONTENT"];
                    if (newrow["CONTENT"].ToString().Length == 0)
                    {
                        continue;
                    }
                    dt.Rows.Add(newrow);
                    TreeListNode node = treeListEmrCollect.AppendNode(new object[] { row["CONTENT"] }, null);
                    node.Tag = newrow;
                }
                //注册鼠标事件 Add by xlb 2013-06-21
                //treeListEmrCollect.MouseUp += new MouseEventHandler(treeListEmrCollect_MouseUp);
                //取消右键删除事件避免重复注册
                barButtonItemDeleteEmrCollect.ItemClick -= new ItemClickEventHandler(barButtonItemDeleteEmrCollect_ItemClick);
                //注册右键删除事件
                barButtonItemDeleteEmrCollect.ItemClick += new ItemClickEventHandler(barButtonItemDeleteEmrCollect_ItemClick);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 病历提取右键弹出右键菜单事件
        /// <auth>XLB</auth>
        /// <date>2013-06-21</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListEmrCollect_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //默认隐藏右键
                barButtonItemDeleteEmrCollect.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //捕捉鼠标按下的键位
                if (e.Button == MouseButtons.Right)
                {
                    TreeListHitInfo hitInfo = treeListEmrCollect.CalcHitInfo(e.Location);
                    if (hitInfo != null && hitInfo.Node != null)
                    {
                        treeListEmrCollect.FocusedNode = hitInfo.Node;
                        TreeListNode node = hitInfo.Node;
                        if (node.Tag != null && node.Tag is DataRow)
                        {
                            //右键删除按钮可用
                            barButtonItemDeleteEmrCollect.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                        //弹出右键
                        popupMenuEmrCollect.ShowPopup(treeListEmrCollect.PointToScreen(new Point(e.X, e.Y)));
                    }
                }
                m_IsAllowDrop = false;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 右键删除事件
        /// Add by xlb 2013-06-21
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDeleteEmrCollect_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //获取当前焦点节点
                TreeListNode node = treeListEmrCollect.FocusedNode;
                DataRow dataRow = null;
                if (node != null && node.Tag != null && node.Tag is DataRow)
                {
                    dataRow = node.Tag as DataRow;
                }
                if (m_PatUtil == null)
                {
                    m_PatUtil = new PatRecUtil(m_app, m_CurrentInpatient);
                }
                string message = "";
                DialogResult dialogResult = MyMessageBox.Show("您确定删除吗？删除后不可恢复。", "提示", MyMessageBoxButtons.OkCancel, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon);
                if (dialogResult == DialogResult.Cancel)
                {
                    return;
                }
                //判断是否删除成功
                if (m_PatUtil.DeleteEmrCollect(dataRow, ref message))
                {
                    treeListEmrCollect.DeleteNode(node);
                    MessageBox.Show("删除成功");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        List<string> DiagNames = new List<string>();
        Dictionary<string, string> padFormDictionary = new Dictionary<string, string>();
        /// <summary>
        /// 加载病历信息
        /// 此处中心医院需求，要求加入【病历信息中添加“诊断”方便医生使用】
        /// add  by  ywk 2013年6月13日 11:06:14
        /// </summary>
        private void InitBLMessage()
        {
            //try
            //{

            //    treeListEmrInfo.Nodes.Clear();
            //    //PadForm padForm = GetSourcePadFormForReplaceItem("入院记录");
            //    if (CurrentForm == null) return;

            //    DataTable dt = new DataTable();
            //    DataColumn dcName = new DataColumn("NAME", Type.GetType("System.String"));
            //    DataColumn dcContent = new DataColumn("CONTENT", Type.GetType("System.String"));

            //    dt.Columns.Add(dcName);
            //    dt.Columns.Add(dcContent);

            //    //此处将各诊断数据先加进DataTable中 到PatDiag表里去找 
            //    //add by ywk 2013年6月13日 12:01:32
            //    DataTable dtPatdiagData = m_PatUtil.GetPatDiagData(CurrentInpatient.NoOfFirstPage.ToString());
            //    DiagNames.Clear();
            //    for (int i = 0; i < dtPatdiagData.Rows.Count; i++)
            //    {
            //        DiagNames.Add(dtPatdiagData.Rows[i]["名称"].ToString());

            //    }

            //    //绑定名称到节点，点击诊断时依旧弹出原诊断数据
            //    DataTable dtBLItemTemplate = m_PatUtil.BLItemTemplate;
            //    foreach (DataRow m_row in dtPatdiagData.Rows)
            //    {
            //        DataRow newrow = dtBLItemTemplate.NewRow();
            //        newrow["名称"] = m_row["名称"];
            //        dtBLItemTemplate.Rows.Add(newrow.ItemArray);

            //        for (int i = 1; i < dtBLItemTemplate.Rows.Count; i++)
            //        {
            //            //执行到每一行都和上一行进行比较，如果期望列的列值相同则删除当前行，同时游标退后一步 
            //            if (dtBLItemTemplate.Rows[i]["名称"].ToString() == dtBLItemTemplate.Rows[i - 1]["名称"].ToString())
            //            {
            //                dtBLItemTemplate.Rows.RemoveAt(i);
            //                i--;
            //            }
            //        }
            //    }

            //    foreach (DataRow row in dtBLItemTemplate.Rows)
            //    {
            //        DataRow newrow = dt.NewRow();
            //        newrow["NAME"] = row["名称"];
            //        newrow["CONTENT"] = CurrentForm.CurrentEditorControl.EMRDoc.GetReplaceText(row["名称"].ToString());
            //        if (newrow["CONTENT"].ToString().Length == 0)
            //        {
            //            continue;
            //        }
            //        dt.Rows.Add(newrow);
            //        TreeListNode node = treeListEmrInfo.AppendNode(new object[] { row["名称"] }, null);
            //        node.Tag = newrow;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            try
            {
                EditorForm pad = new EditorForm(m_CurrentInpatient, m_app);
                treeListEmrInfo.Nodes.Clear();
                //循环每个可替换元素，根据可替换元素的Name属性，查询并赋值Text属性
                //获得病历中所有可替换元素列表



                EditorFormDal m_PadFormDal = new EditorFormDal(m_app);
                //获得当前病历需要替换的项目需要从哪些病历中提取
                DataTable dtReplaceItem = m_PadFormDal.GetReplaceItemRight();

                //源病历的列表
                List<string> sourceEmrList = (from DataRow dr in dtReplaceItem.Rows
                                              select dr["source_emrname"].ToString()).Distinct().ToList();
                padFormDictionary.Clear();
                foreach (string sourceEmr in sourceEmrList)
                {
                    //PadForm padForm = GetSourcePadFormForReplaceItem(sourceEmr);
                    //padFormDictionary.Add(sourceEmr, padForm);

                    string emrContent = pad.GetSourceEditorFormForReplaceItem(sourceEmr);
                    if (emrContent != "")
                    {
                        padFormDictionary.Add(sourceEmr, emrContent);
                    }
                }

                // if (padFormDictionary.Count == 0) return;
                DataTable dt = new DataTable();
                DataColumn dcName = new DataColumn("NAME", Type.GetType("System.String"));
                DataColumn dcContent = new DataColumn("CONTENT", Type.GetType("System.String"));

                dt.Columns.Add(dcName);
                dt.Columns.Add(dcContent);
                DataTable dtPatdiagData = m_PatUtil.GetPatDiagData(m_CurrentInpatient.NoOfFirstPage.ToString());
                DiagNames.Clear();
                for (int i = 0; i < dtPatdiagData.Rows.Count; i++)
                {
                    DiagNames.Add(dtPatdiagData.Rows[i]["名称"].ToString());

                }

                //绑定名称到节点，点击诊断时依旧弹出原诊断数据
                DataTable dtBLItemTemplate = m_PatUtil.BLItemTemplate;
                foreach (DataRow m_row in dtPatdiagData.Rows)
                {
                    DataRow newrow = dtBLItemTemplate.NewRow();
                    newrow["名称"] = m_row["名称"];



                    for (int i = 1; i < dtBLItemTemplate.Rows.Count; i++)
                    {
                        //执行到每一行都和上一行进行比较，如果期望列的列值相同则删除当前行，同时游标退后一步 
                        if (dtBLItemTemplate.Rows[i]["名称"].ToString().Trim() == m_row["名称"].ToString().Trim())
                        {
                            dtBLItemTemplate.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    dtBLItemTemplate.Rows.Add(newrow.ItemArray);
                }

                foreach (DataRow row in dtBLItemTemplate.Rows)
                {
                    DataRow newrow = dt.NewRow();
                    newrow["NAME"] = row["名称"];
                    if (padFormDictionary.ContainsKey(row["source_emrname"].ToString()))
                    {
                        //  string replaceText = ZYEditorControl.GetReplaceTextByName(emrContent, source_itemname);
                        newrow["CONTENT"] = DrectSoft.Library.EmrEditor.Src.Gui.ZYEditorControl.GetReplaceTextByName(padFormDictionary[row["source_emrname"].ToString()], row["名称"].ToString().Trim());
                    }
                    else
                    {
                        newrow["CONTENT"] = newrow["NAME"];
                    }
                    if (newrow["CONTENT"].ToString().Trim().Length == 0)
                    {
                        newrow["CONTENT"] = newrow["NAME"];
                    }
                    dt.Rows.Add(newrow);
                    TreeListNode node = treeListEmrInfo.AppendNode(new object[] { row["名称"] }, null);
                    node.Tag = newrow;
                }



            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 绑定特殊字符 Add by wwj 2013-05-27 使用多线程的方式加快捞取速度
        private void BindSymbol()
        {
            Thread getSymbolThread = new Thread(new ThreadStart(BindSymbolInner));
            getSymbolThread.Start();
        }

        private void BindSymbolInner()
        {
            try
            {
                DataTable dataTableSymbol = m_PatUtil.Symbols;

                System.Drawing.Font font = new System.Drawing.Font("宋体", 10, FontStyle.Regular);
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                ImageList list = new ImageList();
                list.ImageSize = new Size(55, 25);
                foreach (DataRow dr in dataTableSymbol.Rows)
                {
                    string symbol = dr["名称"].ToString();
                    Bitmap bmp = new Bitmap(list.ImageSize.Width, list.ImageSize.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.DrawString(symbol, font, Brushes.Black, new RectangleF(0, 0, bmp.Width, bmp.Height), sf);
                    g.Dispose();
                    list.Images.Add(symbol, bmp);
                }
                BindSymbol(dataTableSymbol, list);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 绑定特殊字符
        /// </summary>
        /// <param name="dataTableSymbol"></param>
        private void BindSymbol(DataTable dataTableSymbol, ImageList list)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<DataTable, ImageList>(BindSymbol), dataTableSymbol, list);
                }
                else
                {
                    imageListBoxControlSymbol.Items.Clear();
                    imageListBoxControlSymbol.ImageList = list;

                    int i = 0;
                    foreach (DataRow dr in dataTableSymbol.Rows)
                    {
                        string symbol = dr["名称"].ToString();
                        imageListBoxControlSymbol.Items.Add(symbol, i);
                        i++;
                    }
                    imageListBoxControlSymbol.MultiColumn = true;
                    imageListBoxControlSymbol.HotTrackItems = true;
                    imageListBoxControlSymbol.HotTrackSelectMode = HotTrackSelectMode.SelectItemOnClick;
                    imageListBoxControlSymbol.HighlightedItemStyle = HighlightStyle.Skinned;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #endregion

        #region 增加NavBarGroup的提示【支持鼠标双击和拖拽】
        List<string> m_NavGroupMouseDragAndDoubleClick;///鼠标双击和拖拽
        List<string> m_NavGroupMouseDoubleClick;///鼠标双击
        /// <summary>
        /// 添加提示
        /// </summary>
        /// <param name="barGroup"></param>
        private void ShowTipForNavBarGroup(NavBarGroup barGroup)
        {
            try
            {
                if (null == m_NavGroupMouseDragAndDoubleClick || m_NavGroupMouseDragAndDoubleClick.Count() == 0)
                {
                    m_NavGroupMouseDragAndDoubleClick = new List<string>();
                    ///图库
                    m_NavGroupMouseDragAndDoubleClick.Add(navBarGroupPicture.Name);
                    ///常用词
                    m_NavGroupMouseDragAndDoubleClick.Add(navBarGroupCYC.Name);
                    ///体征
                    m_NavGroupMouseDragAndDoubleClick.Add(navBarGroupTZ.Name);
                    ///特殊字符
                    m_NavGroupMouseDragAndDoubleClick.Add(navBarGroupSymbol.Name);
                    ///症状
                    m_NavGroupMouseDragAndDoubleClick.Add(navBarGroupZZ.Name);
                    ///病历提取
                    m_NavGroupMouseDragAndDoubleClick.Add(navBarGroupCollect.Name);
                    ///病历信息
                    m_NavGroupMouseDragAndDoubleClick.Add(navBarGroupEmrInfo.Name);
                }
                if (null == m_NavGroupMouseDoubleClick || m_NavGroupMouseDoubleClick.Count() == 0)
                {
                    m_NavGroupMouseDoubleClick = new List<string>();
                    ///常用字典
                    m_NavGroupMouseDoubleClick.Add(navBarGroupZD.Name);
                    ///医嘱数据
                    m_NavGroupMouseDoubleClick.Add(navBarGroupDoctorAdive.Name);
                    ///检验数据
                    m_NavGroupMouseDoubleClick.Add(navBarGroupLis.Name);
                    ///检查数据
                    m_NavGroupMouseDoubleClick.Add(navBarGroupPacs.Name);
                    ///历史病历
                    m_NavGroupMouseDoubleClick.Add(navBarGroupEmrHostory.Name);
                }
                ///清除所有提示
                ClearTipForAllNavBarGroup();

                if (m_NavGroupMouseDragAndDoubleClick.Contains(barGroup.Name))
                {
                    barGroup.Caption += "【支持鼠标双击和拖拽】";
                }
                else if (m_NavGroupMouseDoubleClick.Contains(barGroup.Name))
                {
                    barGroup.Caption += "【支持鼠标双击】";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 清除所有提示信息(右侧模块)
        /// </summary>
        /// <param name="barGroup"></param>
        private void ClearTipForAllNavBarGroup()
        {
            try
            {
                ///工具箱
                ClearTipForNavBarGroup(navBarGroupPicture);
                ClearTipForNavBarGroup(navBarGroupZD);
                ClearTipForNavBarGroup(navBarGroupCYC);
                ClearTipForNavBarGroup(navBarGroupTZ);
                ClearTipForNavBarGroup(navBarGroupSymbol);
                ClearTipForNavBarGroup(navBarGroupZZ);
                ///临床数据
                ClearTipForNavBarGroup(navBarGroupDoctorAdive);
                ClearTipForNavBarGroup(navBarGroupLis);
                ClearTipForNavBarGroup(navBarGroupPacs);
                ClearTipForNavBarGroup(navBarGroupCollect);
                ClearTipForNavBarGroup(navBarGroupEmrInfo);
                ClearTipForNavBarGroup(navBarGroupEmrHostory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 清除提示信息
        /// </summary>
        /// <param name="barGroup"></param>
        private void ClearTipForNavBarGroup(NavBarGroup barGroup)
        {
            try
            {
                if (barGroup.Caption.Contains("【支持鼠标"))
                {
                    barGroup.Caption = barGroup.Caption.Substring(0, barGroup.Caption.IndexOf("【支持鼠标"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 鼠标拖拽
        #region TreeList - 拖动
        private void treeList_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    m_IsAllowDrop = true;
                }
                else
                {
                    m_IsAllowDrop = false;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void treeList_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && m_IsAllowDrop)
                {
                    TreeList list = (TreeList)sender;
                    DoDragDorpInfo(list);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void treeList_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                m_IsAllowDrop = false;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void DoDragDorpInfo(TreeList tree)
        {
            try
            {
                if (tree.FocusedNode == null)
                {
                    return;
                }
                KeyValuePair<string, object> data = new KeyValuePair<string, object>();
                if ((tree.FocusedNode != null) && (tree.FocusedNode.Tag != null))
                {

                    if (tree == treeListPersonTemplate)
                    {
                        TempletItem item = tree.FocusedNode.Tag as TempletItem;
                        if (item.Content.Contains("||chr(10)||chr(13)||"))//存在换行，替代插入数据库中 edit by ywk
                        {
                            item.Content = item.Content.Replace("'||chr(10)||chr(13)||'", "\r\n");
                        }
                        data = new KeyValuePair<string, object>(item.Content, ElementType.Text);
                    }
                    else
                    {
                        DataRow row = (DataRow)tree.FocusedNode.Tag;
                        if (tree.Name == treeListTZ.Name)
                        {///体征
                            byte[] tz = m_PatUtil.GetTZDetailInfo(row["名称"].ToString());
                            data = new KeyValuePair<string, object>("ZYTemplate", tz);
                        }
                        else if (tree.Name == treeListZZ.Name)
                        {///症状
                            byte[] tz = m_PatUtil.GetZZItemDetailInfo(row["名称"].ToString());
                            data = new KeyValuePair<string, object>("ZYTemplate", tz);
                        }
                        else if (tree.Name == treeListCYC.Name)
                        {///常用词
                            Macro mac = new Macro(row);
                            MacroUtil.FillMarcValue(m_CurrentInpatient.NoOfFirstPage.ToString(), mac, m_app.User.Id);
                            if (!string.IsNullOrEmpty(mac.MacroValue))
                            {
                                data = new KeyValuePair<string, object>(mac.MacroValue, ElementType.Text);
                            }
                        }
                        else if (tree.Name == treeListPicture.Name)
                        {///图库
                            byte[] img = m_PatUtil.GetImage(row["ID"].ToString());
                            data = new KeyValuePair<string, object>("ZYTextImage", img);
                        }
                        else if (tree.Name == treeListEmrCollect.Name)
                        {///提取病历
                            data = new KeyValuePair<string, object>(row["CONTENT"].ToString(), ElementType.Text);
                        }
                        else if (tree.Name == treeListEmrInfo.Name)
                        {///病历信息
                            ///病历信息需要特殊处理 ,因为涉及到了诊断信息 ywk 2013年6月13日 14:18:15
                            //data = new KeyValuePair<string, object>(row["CONTENT"].ToString(), ElementType.Text);
                            DiagFormLogic m_DiagFormLogic = new DiagFormLogic(App);
                            bool IsFindDiag = false;//是否找到了诊断
                            if (DiagNames.Count > 0)//如果是诊断就要弹出诊断窗体
                            {
                                for (int i = 0; i < DiagNames.Count; i++)
                                {
                                    if (DiagNames[i].ToString() == row["NAME"].ToString())
                                    {
                                        if (m_DiagFormLogic.ShowDialog(row["NAME"].ToString()))
                                        {

                                            //m_DiagFormLogic.GetDiag(CurrentForm);
                                            //m_DiagFormLogic.MDiagForm.GetDiag();
                                            data = new KeyValuePair<string, object>(m_DiagFormLogic.MDiagForm.GetDiag(), ElementType.Text);
                                            IsFindDiag = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                if (!IsFindDiag)//选择诊断后，不会影响到其他的病历信息拖拽add by ywk 2013年6月17日 15:50:04
                                {
                                    data = new KeyValuePair<string, object>(row["CONTENT"].ToString(), ElementType.Text);
                                }
                                //data = new KeyValuePair<string, object>(row["CONTENT"].ToString(), ElementType.Text);
                                //data = new KeyValuePair<string, object>(row["CONTENT"].ToString(), ElementType.Text);

                            }
                            else//否则还是显示各项的内容 
                            {
                                data = new KeyValuePair<string, object>(row["CONTENT"].ToString(), ElementType.Text);
                                //data = new KeyValuePair<string, object>(row["CONTENT"].ToString(), ElementType.Text);
                            }

                        }
                    }
                }
                if (!string.IsNullOrEmpty(data.Key))
                {
                    tree.DoDragDrop(data, DragDropEffects.All);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 特殊字符 - 拖动
        private void imageListBoxControlSymbol_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ImageListBoxControl list = (ImageListBoxControl)sender;
                if (e.Button == MouseButtons.Left)
                {
                    m_IsAllowDrop = true;
                }
                else
                {
                    m_IsAllowDrop = false;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void imageListBoxControlSymbol_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                ImageListBoxControl list = (ImageListBoxControl)sender;
                if (e.Button == System.Windows.Forms.MouseButtons.Left && m_IsAllowDrop)
                {
                    ListBoxItem lbi = list.SelectedItem as ListBoxItem;
                    list.HotTrackItems = false;
                    KeyValuePair<string, object> data = new KeyValuePair<string, object>(lbi.Value.ToString(), ElementType.Text);

                    if (!string.IsNullOrEmpty(data.Key))
                    {
                        list.DoDragDrop(data, DragDropEffects.All);
                    }
                }
                else
                {
                    list.HotTrackItems = true;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void imageListBoxControlSymbol_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                m_IsAllowDrop = false;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 小模板列表
        private void treeListPersonTemplate_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    m_IsAllowDrop = true;
                }
                else
                {
                    m_IsAllowDrop = false;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void treeListPersonTemplate_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && m_IsAllowDrop)
                {
                    TreeList list = (TreeList)sender;
                    if ((list.FocusedNode != null) && (list.FocusedNode.Tag is TempletItem))
                    {
                        DoDragDorpInfo(list);
                    }
                }
                else
                {
                    m_IsAllowDrop = false;
                }

                TreeListHitInfo hitInfo = treeListPersonTemplate.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.Node != null)
                {
                    if (hitInfo.Node.GetValue("NODETYPE").ToString() == "Leaf" && hitInfo.Node.Tag is TempletItem)
                    {
                        TempletItem item = (TempletItem)hitInfo.Node.Tag;
                        InitToolTip(null == item ? "" : item.Content, treeListPersonTemplate, treeListPersonTemplate.PointToScreen(e.Location));
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void treeListPersonTemplate_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                m_IsAllowDrop = false;
                //右侧小模板列表右键
                if (e.Button == MouseButtons.Right)
                {
                    btnRight_DeptTemplete.Visibility = BarItemVisibility.Always;
                    popupMenu_DeptTemplate.ShowPopup(treeListPersonTemplate.PointToScreen(new Point(e.X, e.Y)));
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion
        #endregion

        #region 鼠标双击事件
        /// <summary>
        /// 图库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListPicture_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right || null == CurrentForm || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }

                TreeListNode node = treeListPicture.FocusedNode;
                if (null == node || null == node.Tag)
                {
                    return;
                }
                DataRow row = (DataRow)node.Tag;
                if (null == row)
                {
                    return;
                }
                byte[] img = m_PatUtil.GetImage(row["ID"].ToString());
                CurrentForm.CurrentEditorControl.EMRDoc._InsertImage(img);
                CurrentForm.CurrentEditorControl.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 常用字典
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlZD_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right || null == CurrentForm || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                if (gridViewZD.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow row = gridViewZD.GetDataRow(gridViewZD.FocusedRowHandle);
                if (row == null)
                {
                    return;
                }
                DataTable sourceTable = m_PatUtil.GetZDDetailInfo(row["SNO"].ToString().TrimEnd('\0'));
                ChoiceForm choice = new ChoiceForm(m_app.SqlHelper, sourceTable);
                if (choice.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CurrentForm.CurrentEditorControl.EMRDoc._InserString(choice.CommitRow["D_NAME"].ToString());
                    CurrentForm.CurrentEditorControl.Focus();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 常用词
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListCYC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right || null == CurrentForm || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                TreeListNode node = treeListCYC.FocusedNode;
                if (null == node || null == node.Tag)
                {
                    return;
                }
                DataRow row = (DataRow)node.Tag;
                if (null == row)
                {
                    return;
                }
                Macro mac = new Macro(row);
                MacroUtil.FillMarcValue(m_CurrentInpatient.NoOfFirstPage.ToString(), mac, m_app.User.Id);
                if (null != mac && mac.MacroValue.Contains("【宏值不存在"))
                {
                    MyMessageBox.Show(mac.MacroValue, "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
                CurrentForm.CurrentEditorControl.EMRDoc._InserString(mac.MacroValue);
                CurrentForm.CurrentEditorControl.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 体征
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListTZ_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right || null == CurrentForm || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                TreeListNode node = treeListTZ.FocusedNode;
                if (null == node || null == node.Tag)
                {
                    return;
                }
                DataRow row = (DataRow)node.Tag;
                if (null == row)
                {
                    return;
                }
                byte[] tz = m_PatUtil.GetTZDetailInfo(row["名称"].ToString());
                KeyValuePair<string, object> data = new KeyValuePair<string, object>("ZYTemplate", tz);
                DragEventArgs args = new DragEventArgs(new DataObject(data), 0, 0, 0, DragDropEffects.None, DragDropEffects.None);
                CurrentForm.CurrentEditorControl.InsertElementByBytes(args);
                CurrentForm.CurrentEditorControl.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 特殊字符
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageListBoxControlSymbol_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right || null == CurrentForm || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                ImageListBoxControl list = (ImageListBoxControl)sender;
                if (null == list || list.ItemCount == 0)
                {
                    return;
                }
                ListBoxItem lbi = list.SelectedItem as ListBoxItem;
                if (null == lbi)
                {
                    return;
                }
                InsertContent(lbi.Value.ToString());
                CurrentForm.CurrentEditorControl.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 症状
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListZZ_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right || null == CurrentForm || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                TreeListNode node = treeListZZ.FocusedNode;
                if (null == node || null == node.Tag)
                {
                    return;
                }
                DataRow row = (DataRow)node.Tag;
                if (null == row)
                {
                    return;
                }
                byte[] zz = m_PatUtil.GetZZItemDetailInfo(row["名称"].ToString());
                KeyValuePair<string, object> data = new KeyValuePair<string, object>("ZYTemplate", zz);
                DragEventArgs args = new DragEventArgs(new DataObject(data), 0, 0, 0, DragDropEffects.None, DragDropEffects.None);
                CurrentForm.CurrentEditorControl.InsertElementByBytes(args);
                CurrentForm.CurrentEditorControl.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 病历提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListEmrCollect_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right || null == CurrentForm || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                InsertIntoEmr(treeListEmrCollect.FocusedNode, "CONTENT");
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 病历信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListEmrInfo_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right || null == CurrentForm || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                InsertIntoEmr(treeListEmrInfo.FocusedNode, "CONTENT");
                CurrentForm.CurrentEditorControl.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewRis_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string LISANDPACSTIP = DS_SqlService.GetConfigValueByKey("IsOpenLISandPACS");
                if (LISANDPACSTIP == "1")//销售演示
                {
                    PacsReport report = new PacsReport();
                    report.StartPosition = FormStartPosition.CenterScreen;
                    if (report.ShowDialog() == DialogResult.OK)
                    {
                        if (CurrentForm != null)
                        {
                            foreach (Image img in report.GetResultImageType())
                            {
                                MemoryStream ms = new MemoryStream();
                                byte[] imagedata = null;
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                                imagedata = ms.GetBuffer();
                                CurrentForm.CurrentEditorControl.EMRDoc._InsertImage(imagedata);
                            }

                            string resultText = report.GetResultStringType();
                            if (resultText != "")
                            {
                                CurrentForm.CurrentEditorControl.EMRDoc.Content.InsertString(resultText);

                                //换行
                                ZYTextElement ele = new ZYTextElement();
                                CurrentForm.CurrentEditorControl.EMRDoc.Content.InsertParagraph(ele);
                            }
                        }
                    }
                }
                else
                {
                    GridHitInfo hitInfo = gridViewRis.CalcHitInfo(gridControlPacs.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)));
                    if (hitInfo.RowHandle >= 0)
                    {
                        DataRowView drv = gridViewRis.GetRow(hitInfo.RowHandle) as DataRowView;
                        ReportPacs pacs = new ReportPacs(drv["检查项目"].ToString(), drv["检查时间"].ToString(), drv["检查状态"].ToString(),
                            drv["描述"].ToString(), drv["诊断"].ToString(), drv["建议"].ToString(),
                            drv["报告医生"].ToString(), drv["最终报告时间"].ToString());
                        pacs.StartPosition = FormStartPosition.CenterScreen;
                        if (pacs.ShowDialog() == DialogResult.OK)
                        {
                            if (!string.IsNullOrEmpty(pacs.CommitValue.ToString()))
                            {
                                if (CurrentForm != null)
                                {
                                    CurrentForm.CurrentEditorControl.EMRDoc._InserString(pacs.CommitValue.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 检验数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewLIS_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int index = gridViewLIS.FocusedRowHandle;
                if (index < 0) return;
                DataRow masterRow = gridViewLIS.GetDataRow(index);

                DataTable detail = m_PatUtil.GetReportDetail(masterRow);
                if (detail != null)
                {
                    using (ReportDetail detailForm = new ReportDetail(detail))
                    {
                        if (detailForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            if (!string.IsNullOrEmpty(detailForm.CommitValue))
                            {
                                if (CurrentForm != null)
                                {
                                    CurrentForm.CurrentEditorControl.EMRDoc._InserString(detailForm.CommitValue);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 医嘱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewDoctorAdive_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int index = gridViewDoctorAdive.FocusedRowHandle;
                if (index < 0) return;
                DataRow masterRow = gridViewDoctorAdive.GetDataRow(index);

                DataTable detail = m_PatUtil.GetPatOrders(masterRow);
                if (detail != null)
                {
                    using (OrdersDetail detailForm = new OrdersDetail(detail))
                    {
                        if (detailForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            if (!string.IsNullOrEmpty(detailForm.CommitValue))
                            {
                                if (CurrentForm != null)
                                {
                                    CurrentForm.CurrentEditorControl.EMRDoc._InserString(detailForm.CommitValue);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        private void InsertIntoEmr(TreeListNode node, string colnumName)
        {
            try
            {
                if (null == node || null == node.Tag)
                {
                    return;
                }
                DataRow row = (DataRow)node.Tag;
                if (null == row)
                {
                    return;
                }

                DiagFormLogic m_DiagFormLogic = new DiagFormLogic(App);
                if (DiagNames.Count > 0)//如果是诊断就要弹出诊断窗体
                {
                    for (int i = 0; i < DiagNames.Count; i++)
                    {
                        if (DiagNames[i].ToString() == row["NAME"].ToString())
                        {
                            if (m_DiagFormLogic.ShowDialog(row["NAME"].ToString()))
                            {
                                //data = new KeyValuePair<string, object>("","");
                                m_DiagFormLogic.GetDiag(CurrentForm);
                                //data = new KeyValuePair<string, object>(m_DiagFormLogic.GetDiag(), ElementType.Text);
                                //data = new KeyValuePair<string, object>("", "");
                                break;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    InsertContent(row[colnumName].ToString());

                }

                else
                {
                    InsertContent(row[colnumName].ToString());
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 编辑器插入内容
        /// </summary>
        /// <param name="content"></param>
        private void InsertContent(string content)
        {
            try
            {
                if (!CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                if (CurrentForm != null && !string.IsNullOrEmpty(content))
                {
                    CurrentForm.CurrentEditorControl.EMRDoc.Content.InsertString(content);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        #region 历史病历
        #region 历史病历 - 右侧工具栏
        /// <summary>
        /// 初始化病人历史记录树
        /// </summary>
        /// <param name="noofinpat"></param>
        private void InitTreeListHistory(int noofinpat)
        {
            try
            {
                if (treeListHistory.Nodes.Count == 0)
                {
                    //加载病人信息
                    LoadHistoryPat(noofinpat);
                    //加载病人对应的病历信息
                    LoadHistoryEmr();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载历史病人信息
        /// </summary>
        /// <param name="dt"></param>
        private void LoadHistoryPat(int noofinpat)
        {
            try
            {
                HistoryEMRBLL hisBLL = new HistoryEMRBLL(m_app, m_CurrentInpatient, m_RecordDal);
                DataTable dt = hisBLL.GetHistoryInpatient(noofinpat, "", "");
                foreach (DataRow dr in dt.Rows)
                {
                    string noofinpatHistory = dr["noofinpat"].ToString();
                    string nameHistory = dr["name"].ToString();
                    string admitdateHistory = dr["admitdate"].ToString();

                    TreeListNode node = treeListHistory.AppendNode(new object[] { "入院时间:" + admitdateHistory }, null);
                    node.Tag = new HistoryInpatient { NoOfInpat = noofinpatHistory, Name = nameHistory };
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadHistoryPat方法出错" + ex.Message);
            }
        }

        /// <summary>
        /// 加载病人的历史病历记录
        /// </summary>
        /// <param name="noofinpat"></param>
        private void LoadHistoryEmr()
        {
            try
            {
                foreach (TreeListNode node in treeListHistory.Nodes)
                {
                    HistoryInpatient pat = node.Tag as HistoryInpatient;
                    if (pat == null) continue;
                    DataTable datatable = m_RecordDal.GetHistoryInpatientFolder(Convert.ToInt32(pat.NoOfInpat));
                    foreach (DataRow dr in datatable.Rows)
                    {
                        //加载病历文件夹
                        TreeListNode subNode = treeListHistory.AppendNode(new object[] { dr["CNAME"].ToString() }, node);
                        EmrModelContainer container = new EmrModelContainer(dr);
                        subNode.Tag = container;

                        if (container.EmrContainerType != ContainerType.None)
                        {
                            //加载具体的病历
                            DataTable leafs = m_PatUtil.GetPatRecInfo(container.ContainerCatalog, pat.NoOfInpat);
                            {
                                foreach (DataRow leaf in leafs.Rows)
                                {
                                    EmrModel leafmodel = new EmrModel(leaf);
                                    TreeListNode leafNode = treeListHistory.AppendNode(new object[] { leafmodel.ModelName }, subNode);
                                    leafNode.Tag = leafmodel;
                                }
                            }
                        }
                    }
                }

                ReSetDailyEmrNodeInHistory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 历史病人节点
        /// </summary>
        public class HistoryInpatient
        {
            /// <summary>
            /// 病人首页序号
            /// </summary>
            public string NoOfInpat { get; set; }

            /// <summary>
            /// 病人名字
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// 历史病历双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListHistory_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    return;
                }
                TreeListHitInfo hitInfo = treeListHistory.CalcHitInfo(e.Location);
                if (hitInfo != null && hitInfo.Node != null)
                {
                    TreeListNode node = hitInfo.Node;
                    EmrModel model = node.Tag as EmrModel;
                    if (model != null)
                    {
                        //弹出病历内容
                        HistoryEmrFormNew printForm = new HistoryEmrFormNew(m_RecordDal, model, node, m_CurrentInpatient, m_app);
                        printForm.ShowDialog();
                        if (CurrentInputTabPages.CurrentForm != null)
                        {
                            CurrentInputTabPages.CurrentForm.Focus();
                        }

                        if (printForm.IsNeedInsertContent && CurrentInputTabPages.CurrentForm != null)
                        {
                            if (CanEdit())
                            {
                                CurrentInputTabPages.CurrentForm.CurrentEditorControl.EMRDoc._Paste();
                                Clipboard.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 历史病人图片绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListHistory_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            try
            {
                TreeListNode node = e.Node;
                if (e.Node.Tag is HistoryInpatient)
                {
                    e.NodeImageIndex = 15;
                }
                else if (e.Node.Tag is EmrModelContainer)
                {
                    e.NodeImageIndex = 2;
                }
                else if (e.Node.Tag is EmrModel)
                {
                    e.NodeImageIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 历史病历 - 导入历史病历
        /// <summary>
        /// 历史病历页面
        /// </summary>
        public void ShowHistoryEmrBatchInForm()
        {
            try
            {
                if (!CheckIsDoctor() && DoctorEmployee.Kind != EmployeeKind.Nurse)
                {
                    MyMessageBox.Show("对不起，您没有历史病历导入权限。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }

                int inCount = Convert.ToInt32(DS_SqlService.GetInHosCountNew((int)m_CurrentInpatient.NoOfFirstPage));
                if (inCount <= 1)
                {
                    MyMessageBox.Show("该病人无历史病历", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }

                ShowHistoryEmrBatchInFormInner();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 自动显示病历批量导入界面
        /// </summary>
        public void AutoShowHistoryEmrBatchInForm()
        {
            try
            {
                if (IsAutoShowHistoryEmrBatchInForm())
                {
                    ShowHistoryEmrBatchInFormInner();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 显示病历批量导入界面
        /// </summary>
        public void ShowHistoryEmrBatchInFormInner()
        {
            try
            {
                if (!(this.FindForm() is MainFormNew))
                {//Add by wwj 2013-04-25 解决外部调用时会自动弹出历史病历导入的问题
                    return;
                }
                if (CheckIsDoctor())
                {
                    string deptChangeID = string.Empty;
                    ChooseDeptForDailyEmrPrint chooseDept = new ChooseDeptForDailyEmrPrint("请选择病历需要导入的科室", m_CurrentInpatient.NoOfFirstPage.ToString(), true, GetAllEmrModelDeptContainer(treeListEmrNodeList.Nodes, null));

                    if (chooseDept.IsNeedShow)
                    {
                        chooseDept.ShowDialog();
                    }
                    if (chooseDept.DialogResult == DialogResult.Yes)
                    {
                        deptChangeID = chooseDept.DeptChangeID;
                    }
                    else
                    {
                        return;
                    }

                    HistoryEmrBatchInFormNew barchInForm = new HistoryEmrBatchInFormNew(m_app, m_CurrentInpatient, deptChangeID, Util.GetParentUserControl<UCEmrInput>(this));
                    barchInForm.StartPosition = FormStartPosition.CenterScreen;
                    barchInForm.ShowDialog();
                }
                else if (DoctorEmployee.Kind == EmployeeKind.Nurse)
                {
                    HistoryEmrBatchInFormNurse nurseForm = new HistoryEmrBatchInFormNurse(m_app, Util.GetParentUserControl<UCEmrInput>(this));
                    nurseForm.StartPosition = FormStartPosition.CenterScreen;
                    nurseForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断是否需要自动显示病历批量导入界面
        /// </summary>
        /// <returns></returns>
        public bool IsAutoShowHistoryEmrBatchInForm()
        {
            try
            {
                ///从配置中取得再加上是否是医生等判断
                string IsShowExportHistory = DS_SqlService.GetConfigValueByKey("IsShowExportHistory");
                ///类别：0-医生；1-护士；2-其它
                int type = CheckIsDoctor() ? 0 : (DoctorEmployee.Kind == EmployeeKind.Nurse ? 1 : 2);
                if (IsShowExportHistory != "1")
                {
                    return false;
                }
                //获取当前病人病历/护理文档数
                int recordCounts = DS_SqlService.GetRecordCounts((int)m_CurrentInpatient.NoOfFirstPage, type);
                if (recordCounts == 0)
                {//无病历/护理
                    DataTable inpHistorys = DS_SqlService.GetHistoryInpatients((int)m_CurrentInpatient.NoOfFirstPage, type, null, null);
                    if (null != inpHistorys && inpHistorys.Rows.Count > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 是否有历史病历导入权限
        /// 医生：导入病历相关
        /// 护士：导入护理相关
        /// </summary>
        /// <returns></returns>
        public bool CanImportHistoryEmr()
        {
            try
            {
                ///从配置中取得再加上是否是医生等判断
                string IsShowExportHistory = DS_SqlService.GetConfigValueByKey("IsShowExportHistory");
                if (IsShowExportHistory == "1")
                {
                    int type = CheckIsDoctor() ? 0 : (DoctorEmployee.Kind == EmployeeKind.Nurse ? 1 : 2);
                    DataTable inpHistorys = DS_SqlService.GetHistoryInpatients((int)m_CurrentInpatient.NoOfFirstPage, type, null, null);
                    if (null != inpHistorys && inpHistorys.Rows.Count > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 导入历史病历验证
        /// </summary>
        /// <returns></returns>
        public string CheckBeforeImport()
        {
            try
            {
                string IsShowExportHistory = DS_SqlService.GetConfigValueByKey("IsShowExportHistory");
                if (IsShowExportHistory != "1")
                {
                    return "对不起，历史病历导入功能尚未开放。";
                }
                if (!CheckIsDoctor() && DoctorEmployee.Kind != EmployeeKind.Nurse)
                {
                    return "对不起，您没有历史病历导入权限。";
                }
                int type = CheckIsDoctor() ? 0 : (DoctorEmployee.Kind == EmployeeKind.Nurse ? 1 : 2);
                DataTable inpHistorys = DS_SqlService.GetHistoryInpatients((int)m_CurrentInpatient.NoOfFirstPage, type, null, null);
                if (null == inpHistorys || inpHistorys.Rows.Count == 0)
                {
                    return "该病人没有历史病历";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #endregion

        #region 病历Model(菜单树)
        /// <summary>
        /// 获取页面所有病历
        /// 注：针对所有记录(包含住院志、病程记录......)
        /// </summary>
        /// <returns></returns>
        public List<EmrModel> GetAllRecordModels(TreeListNodes nodes, List<EmrModel> list)
        {
            try
            {
                if (null == nodes || nodes.Count == 0)
                {
                    return list;
                }
                if (null == list)
                {
                    list = new List<EmrModel>();
                }
                foreach (TreeListNode node in nodes)
                {
                    if (null != node.Tag && node.Tag is EmrModel)
                    {
                        EmrModel modelevey = (EmrModel)node.Tag;
                        list.Add(modelevey);
                    }
                    else if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        list = GetAllRecordModels(node.Nodes, list);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取页面所有病程记录
        /// 注：只针对病程记录
        /// </summary>
        /// <returns></returns>
        public List<EmrModel> GetAllEmrModels(TreeListNodes nodes, List<EmrModel> list)
        {
            try
            {
                if (null == nodes || nodes.Count == 0)
                {
                    return list;
                }
                if (null == list)
                {
                    list = new List<EmrModel>();
                }
                foreach (TreeListNode node in nodes)
                {
                    if (null != node.Tag && node.Tag is EmrModel)
                    {
                        EmrModel modelevey = (EmrModel)node.Tag;
                        if (modelevey.DailyEmrModel)
                        {
                            list.Add(modelevey);
                        }
                    }
                    else if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        list = GetAllEmrModels(node.Nodes, list);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取该病历节点所在科室病历文件夹下所有已保存的病历
        /// </summary>
        /// 注：只已保存针对病程记录(不包含新增状态的病历)
        /// <returns></returns>
        public List<EmrModel> GetSavedEmrModelsInFloader(TreeListNode node)
        {
            try
            {
                if (null == node)
                {
                    return new List<EmrModel>();
                }
                List<EmrModel> list = new List<EmrModel>();
                if (null != node.ParentNode && node.ParentNode.Tag is EmrModelDeptContainer)
                {
                    foreach (TreeListNode nd in node.ParentNode.Nodes)
                    {
                        if (null != nd.Tag && nd.Tag is EmrModel)
                        {
                            EmrModel modelevey = (EmrModel)nd.Tag;
                            if (modelevey.InstanceId != -1)
                            {
                                list.Add(modelevey);
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 从数据库中获取该病历节点所在科室病历文件夹下所有病历
        /// </summary>
        /// <returns></returns>
        public List<DataRow> GetSavedEmrModelsInDB(TreeListNode node, int noofinpat)
        {
            try
            {
                if (null == node)
                {
                    return new List<DataRow>();
                }
                if (null != node.ParentNode && node.ParentNode.Tag is EmrModelDeptContainer)
                {
                    EmrModelDeptContainer container = node.ParentNode.Tag as EmrModelDeptContainer;
                    DataTable leafs = m_PatUtil.GetPatRecInfoNew(container.ContaineCatalog, noofinpat.ToString(), string.IsNullOrEmpty(container.ChangeID) ? -1 : int.Parse(container.ChangeID));
                    return (null == leafs || leafs.Rows.Count == 0) ? new List<DataRow>() : leafs.AsEnumerable().ToList();
                }

                return new List<DataRow>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取页面所有病程记录(不包含当前选中的病程)
        /// 注：只针对病程记录
        /// </summary>
        /// <returns></returns>
        public List<EmrModel> GetAllEmrModelsNotThis(TreeListNodes nodes, TreeListNode focusedNode, List<EmrModel> list)
        {
            try
            {
                if (null == nodes || nodes.Count == 0)
                {
                    return list;
                }
                if (null == list)
                {
                    list = new List<EmrModel>();
                }
                foreach (TreeListNode node in nodes)
                {
                    if (null != node.Tag && node.Tag is EmrModel && node != focusedNode)
                    {
                        EmrModel modelevey = (EmrModel)node.Tag;
                        if (modelevey.DailyEmrModel)
                        {
                            list.Add(modelevey);
                        }
                    }
                    else if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        list = GetAllEmrModels(node.Nodes, list);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmrModelContainer GetCurrentModelContainer()
        {
            try
            {
                TreeListNode node = treeListEmrNodeList.FocusedNode;
                if (node != null && node.Tag != null && node.Tag is EmrModelContainer)
                {
                    EmrModelContainer emrModelContainer = node.Tag as EmrModelContainer;
                    return emrModelContainer;
                }
                else
                {
                    if (m_CurrentModelContainer != null)
                    {
                        return m_CurrentModelContainer;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到当前编辑器中显示病历对应的ID
        /// </summary>
        /// <returns></returns>
        public EmrModel GetCurrentModel()
        {
            try
            {
                TreeListNode node = treeListEmrNodeList.FocusedNode;
                if (node != null && node.Tag != null && node.Tag is EmrModel)
                {
                    EmrModel emrModel = node.Tag as EmrModel;
                    return emrModel;
                }
                else
                {
                    if (m_CurrentModel != null)
                    {
                        return m_CurrentModel;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据Model获取节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public TreeListNode GetNodeByEmrModel(TreeListNodes nodes, EmrModel model)
        {
            try
            {
                if (null == nodes || nodes.Count == 0)
                {
                    return null;
                }
                foreach (TreeListNode node in nodes)
                {
                    if (null != node.Tag && node.Tag is EmrModel)
                    {
                        EmrModel theModel = node.Tag as EmrModel;
                        if (theModel == model)
                        {
                            return node;
                        }
                    }
                    else if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        TreeListNode returnNode = GetNodeByEmrModel(node.Nodes, model);
                        if (null != returnNode)
                        {
                            return returnNode;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存时间 - 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_SaveEmr_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存文书录入中的病历
        /// </summary>
        public void Save()
        {
            try
            {
                GC.Collect();

                //检查是否男性且包含月经史
                if (!CheckMouthHistory())
                {
                    if (MyMessageBox.Show("该患者为男性，但病历内容含有月经史，您是否要继续保存？", "提示信息", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.No)
                    {
                        return;
                    }
                }

                if (m_CurrentModel != null)
                {
                    #region 保存前校验
                    if (m_CurrentModel.DailyEmrModel)
                    {
                        DataTable records = DS_SqlService.GetRecordTimesByNoofinpat(Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                        if (records.AsEnumerable().Any(p => p["captiondatetime"].ToString() == m_CurrentModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") && int.Parse(p["ID"].ToString()) != m_CurrentModel.InstanceId))
                        {
                            MyMessageBox.Show("数据库中已经存在时间为 " + m_CurrentModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " 的病历，请修改病历时间后再保存。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                            return;
                        }

                        List<EmrModel> notThisModels = GetAllEmrModelsNotThis(treeListEmrNodeList.Nodes, treeListEmrNodeList.FocusedNode, null);
                        if (m_CurrentModel.InstanceId == -1)
                        {///新增
                            if (m_CurrentModel.FirstDailyEmrModel)
                            {///首次病程
                                DataRow firstEmr = DS_SqlService.GetFirstDailyEmr(Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                                if (null != firstEmr)
                                {
                                    if (MyMessageBox.Show("该病人已存在首次病程，无法重复创建。您是否要刷新病程记录？刷新后您新增的首次病程将被直接关闭，不予保存。", "提示", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Yes)
                                    {
                                        RefreshBingChengJiLu();//RefreshEMRMainPad();
                                    }
                                    return;
                                }
                            }
                            else
                            {///非首次病程
                                if (!notThisModels.Any(p => p.FirstDailyEmrModel))
                                {///编辑器中不存在首次病程
                                    DataRow firstEmr = DS_SqlService.GetFirstDailyEmr(Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                                    if (null == firstEmr)
                                    {
                                        MyMessageBox.Show("该病人不存在首次病程，无法保存病历。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                                        return;
                                    }
                                    else
                                    {
                                        DateTime firstEmrTime = DateTime.Parse(firstEmr["captiondatetime"].ToString());
                                        if (m_CurrentModel.DisplayTime <= firstEmrTime)
                                        {
                                            MyMessageBox.Show("病程记录时间不能小于首次病程时间 " + firstEmr["captiondatetime"].ToString(), "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                                            return;
                                        }
                                        string userNameAndId = DS_BaseService.GetUserNameAndID(firstEmr["owner"].ToString().Trim());
                                        Save(m_CurrentModel);
                                        MyMessageBox.Show("保存成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                                        if (MyMessageBox.Show("该病人的首次病程已经被 " + userNameAndId + " 创建，您是否要刷新病程记录？刷新后您新增的首次病程将被直接关闭，不予保存。", "提示", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Yes)
                                        {
                                            m_app.CurrentSelectedEmrID = m_CurrentModel.InstanceId.ToString();
                                            RefreshBingChengJiLu(); //RefreshEMRMainPad();
                                        }
                                        return;
                                    }
                                }
                                else if (notThisModels.Any(p => p.FirstDailyEmrModel && p.InstanceId == -1))
                                {///编辑器中存在首次病程且状态为新增
                                    MyMessageBox.Show("请先保存首次病程。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                                    return;
                                }
                            }
                        }

                        int emrID = -1;
                        if (m_CurrentModel.FirstDailyEmrModel)
                        {
                            emrID = m_CurrentModel.InstanceId;
                        }
                        else
                        {
                            EmrModel firstEmr = notThisModels.FirstOrDefault(p => p.FirstDailyEmrModel);
                            if (null != firstEmr)
                            {
                                emrID = firstEmr.InstanceId;
                            }
                        }
                        DataTable dt = DS_SqlService.GetRecordByIDContainsDel(emrID);
                        if (null != dt && dt.Rows.Count == 1 && Convert.ToInt32(dt.Rows[0]["VALID"]) == 0)
                        {
                            string userNameAndID = DS_BaseService.GetUserNameAndID(dt.Rows[0]["owner"].ToString());
                            MyMessageBox.Show("该病人首次病程已经被 " + userNameAndID + " 删除，无法保存。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                            return;
                        }
                    }
                    #endregion

                    Save(m_CurrentModel);

                    if (null != CurrentForm && null != CurrentForm.CurrentEditorControl && null != CurrentForm.CurrentEditorControl.EMRDoc)
                    {
                        CurrentForm.CurrentEditorControl.EMRDoc.Modified = false;
                    }
                }
                else if (m_CurrentEmrEditor != null)
                {
                    DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在保存,请稍候...");
                    m_CurrentEmrEditor.Save();///针对非病历类型的界面 如首页、三测单等
                    DS_Common.HideWaitDialog(m_WaitDialog);
                }
                MyMessageBox.Show("保存成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="model"></param>
        public void Save(EmrModel model)
        {
            try
            {
                if (null == model)
                {
                    return;
                }

                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在保存,请稍候...");
                XmlDocument xmlEmrContent = new XmlDocument();
                xmlEmrContent.PreserveWhitespace = true;
                xmlEmrContent.LoadXml(CurrentForm.CurrentEditorControl.ToXMLString());
                model.ModelContent = xmlEmrContent;

                if (model.InstanceId == -1)
                {//新增
                    model.CreatorXH = DS_Common.currentUser.Id;
                    m_RecordDal.InsertModelInstanceNew(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }
                else
                {//修改
                    m_RecordDal.UpdateModelInstanceNew(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }

                RefreashEmrView(model, model.InstanceId == -1 ? EditState.Add : EditState.Edit);
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                DS_Common.HideWaitDialog(m_WaitDialog);
                throw ex;
            }
        }

        /// <summary>
        /// 保存时查看当前病历是否有月经史，只提示，如果用户还是保存，我们则让它保存吧
        /// </summary>
        /// <returns></returns>
        public bool CheckMouthHistory()
        {
            try
            {
                string mrecord_content = string.Empty;
                bool IsChecked = true;//是否验证通过  默认给通过  edit by ywk 2013年1月19日16:42:24
                XmlDocument xmlRecord = new XmlDocument();
                xmlRecord.PreserveWhitespace = true;
                xmlRecord.LoadXml(this.m_CurrentModel.ModelContent.InnerXml);
                XmlNode body = xmlRecord.SelectSingleNode("//body");
                //查找整个Body部分的标签元素有多少个
                int tipcount = body.SelectNodes("//roelement").Count;
                ArrayList arr_element = new ArrayList();
                for (int i = 0; i < tipcount; i++)
                {
                    arr_element.Add(body.SelectNodes("//roelement")[i].InnerText);//将此病历内容中的标签加入到链表
                }
                Dictionary<string, string> m_checkitem = new Dictionary<string, string>();
                m_checkitem.Add("月经史", "男性不能有月经史");
                string allstr = body.InnerText;

                List<string> tkeys = new List<string>(m_checkitem.Keys);

                if (null != m_app && null != m_app.CurrentPatientInfo && null != m_app.CurrentPatientInfo.PersonalInformation && null == m_app.CurrentPatientInfo.PersonalInformation.Sex)
                {
                    DataTable dt = DS_SqlService.GetInpatientByID((int)m_app.CurrentPatientInfo.NoOfFirstPage);
                    if (null == dt || dt.Rows.Count == 0)
                    {
                        throw new Exception("该病人数据异常，请联系管理员。");
                    }
                    m_app.CurrentPatientInfo.PersonalInformation.Sex = new CommonBaseCode();
                    m_app.CurrentPatientInfo.PersonalInformation.Sex.Code = dt.Rows[0]["sexid"].ToString();
                }
                if (m_app.CurrentPatientInfo.PersonalInformation.Sex.Code == "1")//是男性再进循环体
                {
                    for (int j = 0; j < tipcount; j++)
                    {
                        for (int k = 0; k < m_checkitem.Count; k++)
                        {
                            if (body.SelectNodes("//roelement")[j].InnerText.ToString().Trim().Replace("　", "").Replace(" ", "").StartsWith(tkeys[k].ToString()))
                            {
                                IsChecked = false;
                                return IsChecked;
                            }
                            else
                            {
                                IsChecked = true;
                            }
                        }
                    }
                }
                else
                {
                    IsChecked = true;
                }

                return IsChecked;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 另存为
        public void SaveAs()
        {
            try
            {
                string contentXml = ResetMacro(CurrentForm.CurrentEditorControl.ToXMLString());
                SaveAsForm saveAsForm = new SaveAsForm(m_CurrentModel, m_app, m_RecordDal, contentXml);
                saveAsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 新增

        #region 新增病历
        /// <summary>
        /// 新增事件 - 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_NewEmr_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                AddRecord();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增病历
        /// </summary>
        public void AddRecord()
        {
            try
            {
                TreeListNode node = treeListEmrNodeList.FocusedNode;
                if (node == null || node.Tag == null || node.Tag is EmrModel)
                {
                    return;
                }

                string oldcatelog = MyContainerCode;
                if (node.Tag is EmrModelContainer)
                {
                    EmrModelContainer container = (EmrModelContainer)node.Tag;
                    AddEmrModel(container, node);
                }
                else if (node.Tag is EmrModelDeptContainer)
                {
                    EmrModelDeptContainer container = (EmrModelDeptContainer)node.Tag;
                    AddEmrModel(container, node);
                }

                //add by cyq 2012-09-29 科室小模板数据刷新
                if (oldcatelog != MyContainerCode)
                {
                    InitPersonTree(false, MyContainerCode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 增加病历（焦点在非病程科室上新增：包含病程病程记录）
        /// </summary>
        /// <param name="container"></param>
        private void AddEmrModel(EmrModelContainer container, TreeListNode node)
        {
            try
            {
                if (null == container || null == node)
                {
                    return;
                }
                //添加病历相关文件
                if (container.EmrContainerType != ContainerType.None && !string.IsNullOrEmpty(container.ContainerCatalog))
                {
                    string catalog = container.ContainerCatalog;
                    string chooseDeptID = DS_Common.currentUser.CurrentDeptId;
                    if (catalog == ContainerCatalog.BingChengJiLu)
                    {///病程记录
                        ///是否为补写病历入口且为补写管理员
                        bool IsWriteAdmin = false;
                        if (floaderState == FloderState.NoneAudit)
                        {
                            List<string> jobList = DS_BaseService.GetRolesByUserID(DS_Common.currentUser.Id);
                            IsWriteAdmin = jobList.Contains("00") || jobList.Contains("111");
                        }

                        DataTable changInfoDt = DS_SqlService.GetEmrRightsOfChangeInfo(DS_Common.currentUser.Id, (int)m_CurrentInpatient.NoOfFirstPage);
                        if (!IsWriteAdmin && (null == changInfoDt || changInfoDt.Rows.Count == 0))
                        {///无科室权限
                            MyMessageBox.Show("对不起，您没有科室权限，请联系管理员。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                            return;
                        }
                        else if (changInfoDt.Rows.Count == 1)
                        {///只有一个科室权限
                            AddEmrAtBCJL(treeListEmrNodeList.FocusedNode, changInfoDt.Rows[0]["ID"].ToString());
                        }
                        else
                        {///有多个科室权限
                            ChooseDeptForDailyEmrPrint chooseDeptForm = new ChooseDeptForDailyEmrPrint(m_CurrentInpatient.NoOfFirstPage.ToString(), true, GetAllEmrModelDeptContainer(treeListEmrNodeList.Nodes, null), floaderState);
                            if (chooseDeptForm.ShowDialog() != DialogResult.Yes)
                            {
                                return;
                            }
                            chooseDeptID = chooseDeptForm.DeptID;
                            string changeID = chooseDeptForm.DeptChangeID;
                            AddEmrAtBCJL(treeListEmrNodeList.FocusedNode, changeID);
                        }
                    }
                    else
                    {///非病程记录
                        EmrModel model = GetTemplate(catalog, -1, chooseDeptID);
                        if (model != null)
                        {
                            model.Description = model.ModelName;
                            model.DefaultModelName = model.ModelName;
                            model.DisplayTime = DateTime.Now;
                            model.ModelName = model.ModelName + " " + model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " " + DoctorEmployee.Name;

                            if (AddNewPatRec(model))
                            {
                                container.AddModel(model);
                            }
                        }
                        else
                        {
                            if (CurrentForm != null && CurrentForm.Controls.Count > 0)
                            {
                                XtraTabPage page = CurrentInputTabPages.CurrentTabControl.SelectedTabPage;
                                SelectedPageChanged(page);
                            }
                        }
                        //add by cyq 2012-09-29 右侧科室小模板刷新
                        MyContainerCode = catalog;
                        ucEmrInput.ResetEditModeAction(model);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 增加病历（焦点在病程科室上新增）
        /// </summary>
        /// <param name="container"></param>
        private void AddEmrModel(EmrModelDeptContainer container, TreeListNode node)
        {
            try
            {
                if (null == container || null == node)
                {
                    return;
                }
                //添加病历相关文件
                if (container.EmrContainerType != ContainerType.None && !string.IsNullOrEmpty(container.ContaineCatalog))
                {
                    string infoStr = CheckDeptRights(container.DepartmentID, container.WardID);
                    if (!string.IsNullOrEmpty(infoStr))
                    {
                        MyMessageBox.Show(infoStr, "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                        return;
                    }
                    string catalog = container.ContaineCatalog;
                    EmrModel model = GetTemplate(catalog, string.IsNullOrEmpty(container.ChangeID.Trim()) ? -1 : int.Parse(container.ChangeID), container.DepartmentID);
                    if (model != null)
                    {
                        model.Description = model.ModelName;
                        model.DefaultModelName = model.ModelName;
                        model.ModelName = model.ModelName + " " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + DoctorEmployee.Name;

                        bool isOK = AddNewPatRec(model);
                        if (isOK)
                        {
                            container.AddModel(model);
                            ucEmrInput.ResetEditModeAction(model);
                        }
                    }
                    else
                    {
                        if (CurrentForm != null && CurrentForm.Controls.Count > 0)
                        {
                            XtraTabPage page = CurrentInputTabPages.CurrentTabControl.SelectedTabPage;
                            SelectedPageChanged(page);
                        }
                    }
                    //右侧科室小模板刷新
                    MyContainerCode = catalog;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 通过模板窗体得到病历模板
        /// </summary>
        /// <param name="catalog">病历类型</param>
        /// <param name="deptID">病历科室</param>
        /// <param name="changeID">转科记录ID</param>
        /// <returns></returns>
        private EmrModel GetTemplate(string catalog, int changeID, string deptID)
        {
            try
            {
                //病历模板列表
                DataTable sourceTable = m_PatUtil.GetTemplate(catalog, string.IsNullOrEmpty(deptID) ? DoctorEmployee.CurrentDept.Code : deptID);

                //获取首次病程m_FirstDailyEmrModel 、 病程列表m_AllDailyEmrModels
                GetDailyEmrModels(null);

                if (catalog == ContainerCatalog.BingChengJiLu)
                {
                    //如果已经存在医患沟通，则不能再次新增医患沟通
                    RemoveYiHuanGouTong(sourceTable);
                    //第一次添加为首次病程的模板，然后才能增加普通模板(只针对病程记录)
                    if (null != m_AllDailyEmrModels && m_AllDailyEmrModels.Count() > 0)
                    {
                        var theList = sourceTable.Select(" isfirstdaily <> '1' or isfirstdaily is null ");
                        sourceTable = (null == theList || theList.Count() == 0) ? new DataTable() : theList.CopyToDataTable();
                    }
                }

                //个人模板列表
                DataTable templatePersonTable = m_PatUtil.GetTemplatePerson(DS_Common.currentUser.Id, string.IsNullOrEmpty(deptID) ? DS_Common.currentUser.CurrentDeptId : deptID, catalog);
                //病历选择模板界面
                CatalogForm form = new CatalogForm(sourceTable, templatePersonTable, m_RecordDal, DoctorEmployee, m_app, catalog, m_CurrentInpatient);
                if (catalog == ContainerCatalog.BingChengJiLu)//病程
                {
                    if (m_AllDailyEmrModels.Count() == 0)
                    {//增加的病历是【首次病程】
                        form.SetFristDailyEmrFlag();
                    }
                    else
                    {//增加的病历是【除首次外的病程】
                        form.SetDailyEmrFlag();
                    }
                }

                if (form.ShowDialog() == DialogResult.OK)
                {
                    //病历创建人
                    form.CommitModel.CreatorXH = DS_Common.currentUser.Id;
                    if (catalog == ContainerCatalog.BingChengJiLu && changeID >= 0)//病程
                    {
                        DataTable changeDt = DS_SqlService.GetInpChangeInfoByID(changeID);
                        if (null != changeDt && changeDt.Rows.Count > 0)
                        {
                            form.CommitModel.DeptChangeID = changeID.ToString();
                            form.CommitModel.DepartCode = changeDt.Rows[0]["newdeptid"].ToString();
                            form.CommitModel.WardCode = changeDt.Rows[0]["newwardid"].ToString();
                        }
                    }
                    if (null == form.CommitModel.DeptChangeID || string.IsNullOrEmpty(form.CommitModel.DeptChangeID.Trim()) || catalog != ContainerCatalog.BingChengJiLu)
                    {
                        DataRow dtrowInpatient = m_PatUtil.GetInpatient(m_CurrentInpatient.NoOfFirstPage.ToString());
                        if (dtrowInpatient != null)//获取当前病人所在科室和病区
                        {
                            form.CommitModel.DepartCode = dtrowInpatient["OUTHOSDEPT"].ToString();
                            form.CommitModel.WardCode = dtrowInpatient["OUTHOSWARD"].ToString();
                        }
                    }
                    return form.CommitModel;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 病程记录文件夹下添加病程
        /// </summary>
        /// <param name="node"></param>
        /// <param name="changeID"></param>
        private void AddEmrAtBCJL(TreeListNode node, string changeID)
        {
            try
            {
                if (null == node || null == node.Tag || !(node.Tag is EmrModelContainer) || null == node.Nodes || node.Nodes.Count == 0 || string.IsNullOrEmpty(changeID))
                {
                    return;
                }
                EmrModelContainer container = node.Tag as EmrModelContainer;
                if (null == container || container.ContainerCatalog != ContainerCatalog.BingChengJiLu)
                {
                    return;
                }
                foreach (TreeListNode nd in node.Nodes)
                {
                    if (null != nd.Tag && nd.Tag is EmrModelDeptContainer && ((EmrModelDeptContainer)nd.Tag).ChangeID == changeID)
                    {
                        treeListEmrNodeList.FocusedNode = nd;
                        AddEmrModel(nd.Tag as EmrModelDeptContainer, nd);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 排除医患沟通
        /// </summary>
        /// <param name="dt"></param>
        private void RemoveYiHuanGouTong(DataTable dt)
        {
            try
            {
                bool isYihuangoutong = m_AllDailyEmrModels.Where(model =>
                {
                    if (model.IsYiHuanGouTong == "1")
                    {
                        return true;
                    }
                    return false;
                }).Count() > 0 ? true : false;

                if (isYihuangoutong)
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dt.Rows[i]["isyihuangoutong"].ToString() == "1")
                        {
                            dt.Rows.RemoveAt(i);
                        }
                    }
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 新增病历
        /// </summary>
        /// <param name="model"></param>
        /// <param name="startNew">true: 首次病程 false：非首次病程</param>
        /// <returns></returns>
        private bool AddNewPatRec(EmrModel model)
        {
            try
            {
                UnRegisterEvent();
                TreeListNode node = treeListEmrNodeList.AppendNode(new object[] { model.ModelName }, treeListEmrNodeList.FocusedNode);
                treeListEmrNodeList.FocusedNode.ExpandAll();
                node.Tag = model;

                //先不显示新增加的节点，因为在输入病程标题名称和时间的界面能取消病程
                node.Visible = false;

                if (model.DailyEmrModel)//设置病程相关信息
                {
                    bool isAdd = AddDailyEmrModel(model, node);
                    if (!isAdd)
                    {
                        treeListEmrNodeList.DeleteNode(node);
                        RegisterEvent();
                        return false;
                    }
                }
                //加载病历(编辑器)
                CreateNewDocument(model);
                //设置病历位置
                treeListEmrNodeList.SetNodeIndex(node, FindModelIndex(node));

                //病历增加结束后将节点显示出来
                node.Visible = true;

                treeListEmrNodeList.FocusedNode = node;
                if (xtraTabControlEmr.TabPages.Count == 1)
                {
                    SelectedPageChanged(xtraTabControlEmr.SelectedTabPage);
                }
                RegisterEvent();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增病程
        /// </summary>
        /// <param name="model">病程</param>
        /// <param name="node">新增的病程对应的树节点</param>
        /// <returns>true：创建成功 false：取消创建、创建失败 </returns>
        private bool AddDailyEmrModel(EmrModel model, TreeListNode node)
        {
            try
            {
                if (null == model || null == node)
                {
                    return false;
                }
                if (model.DailyEmrModel)//病程
                {
                    DateTime? firstDailyTime = null;
                    if (model.FirstDailyEmrModel)
                    {
                        if (model.InstanceId != -1)
                        {
                            firstDailyTime = model.DisplayTime;
                        }
                    }
                    else
                    {
                        EmrModel firstEmr = Util.GetFirstDailyEmrModel(treeListEmrNodeList);
                        if (null != firstEmr)
                        {
                            firstDailyTime = firstEmr.DisplayTime;
                        }
                    }
                    List<EmrModel> modelList = Util.GetAllEmrModels(treeListEmrNodeList.Nodes, new List<EmrModel>());
                    DailyTemplateForm daily = new DailyTemplateForm(m_app, model, firstDailyTime, modelList, this);
                    if (daily.ShowDialog() == DialogResult.OK)
                    {
                        model.DisplayTime = daily.CommitDateTime;
                        model.ModelName = model.Description + " " + model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " " + DoctorEmployee.Name;
                        model.Description = daily.CommitTitle;
                        //更加设置的时间动态改变树节点的名称
                        node.SetValue("colName", model.ModelName);

                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取界面中的首次病程记录EmrModel
        /// </summary>
        /// <returns></returns>
        private void GetDailyEmrModels(List<TreeListNode> parentNodes)
        {
            try
            {
                if (parentNodes == null)
                {
                    parentNodes = treeListEmrNodeList.Nodes.OfType<TreeListNode>().ToList();
                    m_FirstDailyEmrModel = null;
                    m_AllDailyEmrModels = new List<EmrModel>();
                }

                foreach (TreeListNode node in parentNodes)
                {
                    EmrModel emrModel = node.Tag as EmrModel;
                    if (emrModel != null)
                    {
                        if (emrModel.ModelCatalog == ContainerCatalog.BingChengJiLu)
                        {
                            m_AllDailyEmrModels.Add(emrModel);
                            //判断首次病程
                            if (emrModel.FirstDailyEmrModel)
                            {
                                m_FirstDailyEmrModel = emrModel;
                            }
                        }
                    }
                    if (node.Nodes.Count > 0)
                    {
                        GetDailyEmrModels(node.Nodes.OfType<TreeListNode>().ToList());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private EmrModel m_FirstDailyEmrModel;//首次病程的EmrModel
        private List<EmrModel> m_AllDailyEmrModels;//所有病程的集合EmrModel

        /// <summary>
        /// 检查病人当前科室与医生科室(包含权限科室)是否一致
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-24</date>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        private string CheckDeptRights(string deptID, string wardID)
        {
            try
            {
                if (floaderState == FloderState.NoneAudit)
                {///补写病历入口
                    List<string> jobList = DS_BaseService.GetRolesByUserID(DS_Common.currentUser.Id);
                    if (jobList.Contains("00") || jobList.Contains("111"))
                    {///补写管理员角色
                        return string.Empty;
                    }
                }

                List<string[]> list = DS_BaseService.GetDeptAndWardInRight(DS_Common.currentUser.Id);
                if (deptID != DS_Common.currentUser.CurrentDeptId)
                {
                    if (string.IsNullOrEmpty(deptID.Trim()))
                    {
                        return "该病人所属科室异常，请联系管理员。";
                    }
                    if (null == list || list.Count == 0 || !list.Any(p => p[0] == deptID))
                    {
                        string deptName = DS_BaseService.GetDeptNameByID(deptID);
                        return "对不起，您没有 " + deptName + "(" + deptID + ")" + " 的权限。";
                    }
                }
                if (wardID != DS_Common.currentUser.CurrentWardId)
                {
                    if (string.IsNullOrEmpty(wardID.Trim()))
                    {
                        return "该病人所属病区异常，请联系管理员。";
                    }
                    if (null == list || list.Count == 0 || !list.Any(p => p[1] == wardID))
                    {
                        string wardName = DS_BaseService.GetWardNameByID(wardID);
                        return "对不起，您没有 " + wardName + "(" + wardID + ")" + " 的权限。";
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有的病程科室List
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private List<EmrModelDeptContainer> GetAllEmrModelDeptContainer(TreeListNodes nodes, List<EmrModelDeptContainer> list)
        {
            try
            {
                if (null == nodes || nodes.Count == 0)
                {
                    return list;
                }
                if (null == list)
                {
                    list = new List<EmrModelDeptContainer>();
                }
                foreach (TreeListNode node in nodes)
                {
                    if (null != node.Tag && node.Tag is EmrModelDeptContainer)
                    {
                        list.Add(node.Tag as EmrModelDeptContainer);
                    }
                    else if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        list = GetAllEmrModelDeptContainer(node.Nodes, list);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 新增护理记录表格
        /// <summary>
        /// 新增事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_NewDoc_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CommonChooseForm commonChooseForm = new CommonChooseForm(m_app);
                DialogResult diaresult = commonChooseForm.ShowDialog();
                if (diaresult == DialogResult.OK)
                {
                    string message = "";
                    CommonNoteEntity commonNote = commonChooseForm.SelectCommonNoteEntity;
                    string InCommonNoteName = commonNote.CommonNoteName;
                    CommonNoteBiz commonNoteBiz = new DrectSoft.Core.CommonTableConfig.CommonNoteBiz(m_app);
                    commonNote = commonNoteBiz.GetDetailCommonNote(commonNote.CommonNoteFlow);
                    commonNote.CommonNoteName = InCommonNoteName;
                    TreeListNode patNode = treeListEmrNodeList.FocusedNode;

                    InCommonNoteEnmtity inCommonNote = InCommonNoteBiz.ConvertCommonToInCommon(commonNote);
                    InCommonNoteBiz icombiz = new DrectSoft.Core.CommonTableConfig.CommonNoteUse.InCommonNoteBiz(m_app);
                    DataTable inpatientDt = icombiz.GetInpatient(m_CurrentInpatient.NoOfFirstPage.ToString());
                    inCommonNote.CurrDepartID = inpatientDt.Rows[0]["OUTHOSDEPT"].ToString();
                    inCommonNote.CurrDepartName = inpatientDt.Rows[0]["DEPARTNAME"].ToString();
                    inCommonNote.CurrWardID = inpatientDt.Rows[0]["OUTHOSWARD"].ToString();
                    inCommonNote.CurrWardName = inpatientDt.Rows[0]["WARDNAME"].ToString();
                    inCommonNote.NoofInpatient = m_CurrentInpatient.NoOfFirstPage.ToString();
                    inCommonNote.InPatientName = inpatientDt.Rows[0]["NAME"].ToString();
                    bool saveResult = icombiz.SaveInCommomNoteAll(inCommonNote, ref message);
                    if (saveResult)
                    {
                        string treename = inCommonNote.InCommonNoteName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        TreeListNode leafNode = treeListEmrNodeList.AppendNode(new object[] { treename }, patNode);
                        leafNode.Tag = inCommonNote;
                        treeListEmrNodeList.FocusedNode = leafNode;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_DeleteDoc_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var treelistnode = treeListEmrNodeList.FocusedNode;
                InCommonNoteEnmtity inCommonNoteEnmtity = treelistnode.Tag as InCommonNoteEnmtity;
                if (inCommonNoteEnmtity == null) return;
                DialogResult diaresult = MyMessageBox.Show("您确定要删除该记录吗？", "删除记录", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon);
                if (diaresult == DialogResult.No)
                {
                    return;
                }
                InCommonNoteBiz inCommonNoteBiz = new InCommonNoteBiz(m_app);
                string message = "";
                //用来判断要删除的单据是否有记录，有则无法删除
                int count = inCommonNoteBiz.GetCommonItemCount(inCommonNoteEnmtity.InCommonNoteFlow);
                if (count != 0)
                {
                    MyMessageBox.Show("该记录正在被使用，无法删除。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
                bool Delresult = inCommonNoteBiz.DelInCommonNote(inCommonNoteEnmtity, ref message);
                if (Delresult)
                {
                    DeleteNodeNotTriggerEvent(treelistnode);
                    XtraTabPage xtraTabPageForDel = null;
                    foreach (XtraTabPage itemtab in xtraTabControlEmr.TabPages)
                    {
                        if (itemtab.Tag != null && itemtab.Tag is InCommonNoteEnmtity)
                        {
                            if ((itemtab.Tag as InCommonNoteEnmtity).InCommonNoteFlow == inCommonNoteEnmtity.InCommonNoteFlow)
                            {
                                xtraTabPageForDel = itemtab;
                                break;
                            }

                        }
                    }
                    if (xtraTabPageForDel != null)
                    {
                        xtraTabControlEmr.TabPages.Remove(xtraTabPageForDel);
                        MyMessageBox.Show("删除成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                    }


                }
                else
                {
                    MessageBox.Show(message);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #endregion

        #region 删除
        /// <summary>
        /// 删除事件 - 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_DeleteEmr_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                DeleteDocument();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        public void DeleteDocument()
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    if (!CanDelete())
                    {
                        return;
                    }
                    TreeListNode node = treeListEmrNodeList.FocusedNode;
                    if (node.Tag != null)
                    {
                        EmrModel emrModel = node.Tag as EmrModel;
                        if (emrModel != null)
                        {
                            string recordTypeName = "【病历:";
                            recordTypeName += emrModel.ModelName + "】";
                            if (MyMessageBox.Show("您确定要删除 " + recordTypeName + " 吗？", "提示", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Yes)
                            {
                                DeleteTemplate();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断病历是否可以删除
        /// </summary>
        private bool CanDelete()
        {
            try
            {
                TreeListNode node = treeListEmrNodeList.FocusedNode;
                if (node != null && node.Tag != null && node.Tag is EmrModel)
                {
                    EmrModel model = node.Tag as EmrModel;
                    if (model.CreatorXH != DS_Common.currentUser.Id)
                    {
                        string userStr = DS_BaseService.GetUserNameAndID(model.CreatorXH);
                        MyMessageBox.Show("该病历是 " + userStr + " 创建的，您没有权限删除。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                        return false;
                    }
                    if (model.FirstDailyEmrModel)
                    {
                        List<EmrModel> emrList = GetAllEmrModels(treeListEmrNodeList.Nodes, null);
                        if (null != emrList && emrList.Count() > 1)
                        {
                            MyMessageBox.Show("该首次病程下存在其它病历，不能删除。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                            return false;
                        }
                        else
                        {
                            DataTable dt = DS_SqlService.GetRecordTimesByNoofinpat((int)m_app.CurrentPatientInfo.NoOfFirstPage);
                            if (null != dt && dt.Rows.Count > 1)
                            {
                                var otherOwnerEmr = dt.AsEnumerable().FirstOrDefault(p => p["owner"].ToString() != DS_Common.currentUser.Id);
                                string memoName = string.Empty;
                                if (null != otherOwnerEmr)
                                {
                                    memoName = DS_BaseService.GetUserNameAndID(otherOwnerEmr["owner"].ToString());
                                }
                                else
                                {
                                    memoName = DS_Common.currentUser.Name + "(" + DS_Common.currentUser.Id + ")";
                                }
                                if (MyMessageBox.Show(memoName + " 已创建过病历，不能删除首次病程，您是否要刷新病程记录？", "提示", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon) == DialogResult.Yes)
                                {
                                    RefreshBingChengJiLu();
                                }
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeleteTemplate()
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    EmrModel emrModel = m_CurrentModel;

                    CurrentInputTabPages.UnRegisterEvent();///SelectedPageChanged事件
                    CurrentInputTabPages.SelectedPages.RemoveAll(page => { return page == xtraTabControlEmr.SelectedTabPage; });
                    DeleteTabPageAndNode(emrModel);
                    if (emrModel.InstanceId > 0)
                    {
                        RefreashEmrView(emrModel, EditState.Delete);
                        DS_SqlService.UpdateRecordValid(emrModel.InstanceId, false);
                    }
                    ///设置上一个焦点Tab页
                    if (CurrentInputTabPages.SelectedPages.Count > 0)
                    {
                        xtraTabControlEmr.SelectedTabPage = CurrentInputTabPages.SelectedPages[CurrentInputTabPages.SelectedPages.Count - 1];
                        SelectedPageChanged(xtraTabControlEmr.SelectedTabPage);
                    }
                    CurrentInputTabPages.RegisterEvent();///SelectedPageChanged事件
                    MyMessageBox.Show("删除成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将病历从TabControl和TreeList中删除
        /// </summary>
        /// <param name="emrModel"></param>
        private void DeleteTabPageAndNode(EmrModel emrModel)
        {
            try
            {
                TreeListNode node = FindEmrModelInNode(emrModel);
                DeleteNodeNotTriggerEvent(node);
                DeleteTablePage(emrModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到当前EmrModel所在的树节点
        /// </summary>
        /// <param name="emrModel"></param>
        private TreeListNode FindEmrModelInNode(EmrModel emrModel)
        {
            try
            {
                GetCurrentTreeListNode(null, emrModel);
                return m_Node;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private TreeListNode m_Node;
        private void GetCurrentTreeListNode(TreeListNodes nodes, EmrModel emrModel)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = treeListEmrNodeList.Nodes;
                }

                foreach (TreeListNode subNode in nodes)
                {
                    if (subNode.Tag is EmrModel && (EmrModel)subNode.Tag == emrModel)
                    {
                        m_Node = subNode;
                    }
                    GetCurrentTreeListNode(subNode.Nodes, emrModel);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DeleteTablePage(EmrModel emrModel)
        {
            try
            {
                if (null == emrModel || null == m_TempModelPages || m_TempModelPages.Count == 0 || !m_TempModelPages.ContainsKey(emrModel))
                {
                    return;
                }
                XtraTabPage page = m_TempModelPages[emrModel];
                if (page != null)
                {
                    m_TempModelPages.Remove(emrModel);
                    RemoveDailyPreViewCollection(emrModel, page);
                    TreeListNode theNode = GetNodeByEmrModel(treeListEmrNodeList.Nodes, emrModel);
                    if (null != theNode && null != theNode.Tag && theNode.Tag is EmrModel && (theNode.Tag as EmrModel).InstanceId == -1)
                    {
                        DeleteNodeNotTriggerEvent(theNode);
                    }
                    CurrentInputTabPages.CurrentTabControl.TabPages.Remove(page);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除树中的节点，不触发事件
        /// </summary>
        /// <param name="node"></param>
        private void DeleteNodeNotTriggerEvent(TreeListNode node)
        {
            try
            {
                UnRegisterEvent();
                treeListEmrNodeList.DeleteNode(node);
                RegisterEvent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 提交
        /// <summary>
        /// 提交事件 - 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_Submit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SubmitDocment();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void SubmitDocment()
        {
            try
            {
                if (null == m_CurrentModel)
                {
                    return;
                }
                if (!IsOpenThreeLevelAudit())
                {
                    MyMessageBox.Show("对不起，病历提交功能暂未开放。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }

                if (null != m_CurrentModel && m_CurrentModel.FirstDailyEmrModel && m_CurrentModel.InstanceId != -1)
                {
                    DataTable dt = DS_SqlService.GetRecordByIDContainsDel(m_CurrentModel.InstanceId);
                    if (null != dt && dt.Rows.Count == 1 && Convert.ToInt32(dt.Rows[0]["VALID"]) == 0)
                    {
                        string userNameAndID = DS_BaseService.GetUserNameAndID(dt.Rows[0]["owner"].ToString());
                        MyMessageBox.Show("该病人首次病程已经被 " + userNameAndID + " 删除，无法提交。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                        return;
                    }
                }

                DataRow firstEmr = DS_SqlService.GetFirstDailyEmr(Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                if (null == firstEmr && isCommitLimit == "1")
                {
                    MyMessageBox.Show("该病人不存在首次病程或首次病程已经被删除，无法提交病历。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }

                SubmitDocmentInner(m_CurrentModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SubmitDocmentInner(EmrModel model)
        {
            try
            {
                if (model == null)
                {
                    return;
                }
                if (MyMessageBox.Show("您确定要提交 " + model.ModelName + " 吗？", "提示", MyMessageBoxButtons.OkCancel, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Cancel)
                {
                    return;
                }
                if (string.IsNullOrEmpty(DoctorEmployee.Grade.Trim()))
                {
                    return;
                }

                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);
                switch (grade)
                {
                    case DoctorGrade.Resident://住院医师
                        model.State = ExamineState.SubmitButNotExamine;
                        break;
                    case DoctorGrade.Attending://主治医师
                        model.State = ExamineState.FirstExamine;
                        break;
                    case DoctorGrade.AssociateChief://副主任医师
                    case DoctorGrade.Chief://主任医师
                        model.State = ExamineState.SecondExamine;
                        break;
                    default:
                        return;
                }

                model.ExamineTime = DateTime.Now;
                model.ExaminerXH = DS_Common.currentUser.Id;
                model.Valid = true;
                model.IsModified = true;
                model.ModelContent = CurrentForm.CurrentEditorControl.BinaryToXml(CurrentForm.CurrentEditorControl.SaveBinary());
                Save(model);
                SetDocmentReadOnlyMode();
                SetStateImage();
                CurrentForm.CurrentEditorControl.ActiveEditArea = null;
                CurrentForm.CurrentEditorControl.Refresh();
                CurrentForm.CurrentEditorControl.EMRDoc.Modified = false;
                ucEmrInput.ResetEditModeAction(model);

                MyMessageBox.Show(!CurrentForm.CurrentEditorControl.CanEdit() ? "病历提交成功，不可再编辑。" : "病历提交成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
            }
            catch (Exception ex)
            {
                throw new Exception("提交病历时发生异常" + ex.Message);
            }
        }

        /// <summary>
        /// 是否开放三级检诊功能
        /// </summary>
        /// <returns></returns>
        public bool IsOpenThreeLevelAudit()
        {
            try
            {
                string emrConfig = DS_SqlService.GetConfigValueByKey("EmrSetting");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(emrConfig);
                XmlNodeList nodeList = doc.GetElementsByTagName("OpenThreeLevelAudit");
                if (nodeList.Count > 0)
                {
                    XmlElement ele = nodeList[0] as XmlElement;
                    if (ele.InnerText.Trim() == "0")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 审核
        /// <summary>
        /// 审核事件 - 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_Audit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                AuditDocment();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        public void AuditDocment()
        {
            try
            {
                if (null == m_CurrentModel)
                {
                    return;
                }

                if (!IsOpenThreeLevelAudit())
                {
                    MyMessageBox.Show("对不起，病历审核功能暂未开放。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }

                if (MyMessageBox.Show("您确定要审核 " + m_CurrentModel.ModelName + " 吗？", "提示", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.No)
                {
                    return;
                }

                if (string.IsNullOrEmpty(DoctorEmployee.Grade.Trim()))
                {
                    return;
                }
                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);

                switch (grade)
                {
                    case DoctorGrade.Attending://住院医师
                        m_CurrentModel.State = ExamineState.FirstExamine;
                        break;
                    case DoctorGrade.AssociateChief://副主任医师
                    case DoctorGrade.Chief://主任医师
                        m_CurrentModel.State = ExamineState.SecondExamine;
                        break;
                    case DoctorGrade.None:
                    case DoctorGrade.Resident:
                    default:
                        return;
                }


                m_CurrentModel.ExaminerXH = m_app.User.Id;
                //  m_CurrentModel.ExamineTime = DateTime.Now;
                if (!m_CurrentModel.IsModified)
                {
                    m_CurrentModel.IsModified = true;
                }
                m_CurrentModel.ModelContent = CurrentForm.CurrentEditorControl.BinaryToXml(CurrentForm.CurrentEditorControl.SaveBinary());
                Save(m_CurrentModel);

                //副主任医师和主任医师可以连续审核
                if (grade == DoctorGrade.AssociateChief || grade == DoctorGrade.Chief)
                {
                    MyMessageBox.Show("病历审核成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                }
                else
                {
                    SetDocmentReadOnlyMode();
                    MyMessageBox.Show("病历审核成功，不可再编辑。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                }

                SetStateImage();
                CurrentForm.CurrentEditorControl.ActiveEditArea = null;
                CurrentForm.CurrentEditorControl.Refresh();
                CurrentForm.CurrentEditorControl.EMRDoc.Modified = false;
                ucEmrInput.ResetEditModeAction(m_CurrentModel);
            }
            catch (Exception ex)
            {
                throw new Exception("审核病历时发生异常" + ex.Message + "");
            }
        }

        #endregion

        #region 取消审核
        /// <summary>
        /// 取消审核事件 - 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_CancelAudit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CancelAudit();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 申请开放事件 - 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_Reback_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Reback();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 病历审核
        /// </summary>
        public void CancelAudit()
        {
            try
            {
                if (null == m_CurrentModel)
                {
                    return;
                }
                if (!IsOpenThreeLevelAudit())
                {
                    MyMessageBox.Show("对不起，病历审核功能暂未开放。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
                if (MyMessageBox.Show("您确定要取消审核 " + m_CurrentModel.ModelName + " 吗？", "提示", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.No)
                {
                    return;
                }

                m_CurrentModel.State = ExamineState.NotSubmit;
                m_CurrentModel.ExaminerXH = m_app.User.Id;

                if (!m_CurrentModel.IsModified)
                {
                    m_CurrentModel.IsModified = true;
                }
                m_CurrentModel.ModelContent = CurrentForm.CurrentEditorControl.BinaryToXml(CurrentForm.CurrentEditorControl.SaveBinary());

                Save(m_CurrentModel);
                SetDocmentReadOnlyMode();

                SetStateImage();
                CurrentForm.CurrentEditorControl.ActiveEditArea = null;
                CurrentForm.CurrentEditorControl.Refresh();
                ucEmrInput.ResetEditModeAction(m_CurrentModel);
                MyMessageBox.Show("取消审核病历成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
            }
            catch (Exception ex)
            {
                throw new Exception("取消审核病历时发生异常" + ex.Message + "");
            }
        }
        /// <summary>
        /// 病历申请开放
        /// </summary>
        public void Reback()
        {
            try
            {
                if (null == m_CurrentModel)
                {
                    return;
                }
                if (!IsOpenThreeLevelAudit())
                {
                    MyMessageBox.Show("对不起，病历审核功能暂未开放。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);
                if ((m_CurrentModel.State == ExamineState.SubmitButNotExamine && m_CurrentModel.ExaminerXH == App.User.Id) || (m_CurrentModel.State == ExamineState.FirstExamine && m_CurrentModel.ExaminerXH == App.User.Id && grade == DoctorGrade.Attending) || (m_CurrentModel.State == ExamineState.SecondExamine && m_CurrentModel.ExaminerXH == App.User.Id && (grade == DoctorGrade.Chief || grade == DoctorGrade.AssociateChief)))
                {
                    m_CurrentModel.State = ExamineState.NotSubmit;
                    m_CurrentModel.ExaminerXH = m_app.User.Id;

                    if (m_CurrentModel.IsModified)
                    {
                        m_CurrentModel.IsModified = true;
                    }
                    m_CurrentModel.ModelContent = CurrentForm.CurrentEditorControl.BinaryToXml(CurrentForm.CurrentEditorControl.SaveBinary());

                    Save(m_CurrentModel);
                    SetDocmentReadOnlyMode();

                    SetStateImage();
                    CurrentForm.CurrentEditorControl.ActiveEditArea = null;
                    ucEmrInput.ResetEditModeAction(m_CurrentModel);
                    //CurrentForm.CurrentEditorControl.Refresh();
                    MyMessageBox.Show("申请开放成功，病程关闭后请重新打开！", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                    try//add by Ukey 2016-10-30 申请开放后关闭当前申请的文档
                    {
                        CurrentInputTabPages.SelectedPages.RemoveAll(page => { return page == CurrentInputTabPages.CurrentTabControl.SelectedTabPage; });

                        CurrentInputTabPages.UnRegisterEvent();
                        ClickTabPageCloseButton();

                        if (CurrentInputTabPages.SelectedPages.Count > 0)
                        {
                            CurrentInputTabPages.CurrentTabControl.SelectedTabPage = CurrentInputTabPages.SelectedPages[CurrentInputTabPages.SelectedPages.Count - 1];
                            CurrentInputTabPages.SelectedPages.Add(CurrentInputTabPages.CurrentTabControl.SelectedTabPage);
                            SelectedPageChanged(CurrentInputTabPages.CurrentTabControl.SelectedTabPage);
                        }

                        CurrentInputTabPages.RegisterEvent();
                    }
                    catch (Exception ex)
                    {
                        MyMessageBox.Show(1, ex.Message);
                    }
                }
                else
                {
                    MyMessageBox.Show("对不起，该病历不符合申请开放的条件。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("申请开放时发生异常" + ex.Message + "");
            }
        }

        #endregion

        #region 自检
        /// <summary>
        /// 检查一些必填元素是否已经填写
        /// </summary>
        public void CheckSelf()
        {
            try
            {
                if (null == m_CurrentModel)
                {
                    return;
                }
                //传进一段XML，先搜寻要验证的XML节点名称，然后取出紧跟其后的元素，比较两个元素之间的内容是否符合要求
                string mrecord_content = string.Empty;
                int findedcount = 0;//定义变量记录查找到指定标签的存在链表中的索引值
                string nexttip = string.Empty;//记录要查找的标签的下一个标签
                bool IsChecked = false;//是否验证通过
                m_CurrentModel.ModelContent = CurrentForm.CurrentEditorControl.BinaryToXml(CurrentForm.CurrentEditorControl.SaveBinary());
                if (null == m_CurrentModel.ModelContent || null == m_CurrentModel.ModelContent.OuterXml)
                {
                    MyMessageBox.Show("病历内容不能为空", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
                XmlDocument xmlRecord = new XmlDocument();
                xmlRecord.PreserveWhitespace = true;
                xmlRecord.LoadXml(m_CurrentModel.ModelContent.InnerXml);
                XmlNode body = xmlRecord.SelectSingleNode("//body");
                //查找整个Body部分的标签元素有多少个
                int tipcount = body.SelectNodes("//roelement").Count;
                ArrayList arr_element = new ArrayList();
                for (int i = 0; i < tipcount; i++)
                {
                    arr_element.Add(body.SelectNodes("//roelement")[i].InnerText);//将此病历内容中的标签加入到链表
                }

                //先取出系统参数配置中配置的那些要进行检查的项目要进行验证的限制长度 
                string config = DS_SqlService.GetConfigValueByKey("CheckDocByConfig");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(config);
                int confignum = doc.GetElementsByTagName("checkitem").Count;//取得系统参数配置中的要验证的项有几个

                Dictionary<string, string> m_checkitem = new Dictionary<string, string>();
                for (int i = 0; i < confignum; i++)
                {
                    m_checkitem.Add(doc.GetElementsByTagName("checkitem")[i].InnerText.Split(',')[0], doc.GetElementsByTagName("checkitem")[i].InnerText.Split(',')[1]);
                }
                string allstr = body.InnerText;

                List<string> tkeys = new List<string>(m_checkitem.Keys);
                for (int j = 0; j < tipcount; j++)
                {
                    for (int k = 0; k < m_checkitem.Count; k++)
                    {
                        if (body.SelectNodes("//roelement")[j].InnerText.ToString().Trim().Replace("　", "").Replace(" ", "").StartsWith(tkeys[k].ToString()))
                        {
                            findedcount = j;
                            nexttip = body.SelectNodes("//roelement")[j + 1].InnerText.ToString();//下一个元素名称
                            string startstr = tkeys[k].ToString();
                            int startop = allstr.Replace("　", "").Replace(" ", "").IndexOf(startstr.Replace("　", "").Replace(" ", ""), 0) + startstr.Length; //开始位置
                            int endop = allstr.Replace("　", "").Replace(" ", "").IndexOf(nexttip.Replace("　", "").Replace(" ", ""), startop); //结束位置

                            string my = allstr.Replace("　", "").Replace(" ", "").Substring(startop, endop - startop).Replace("：", ""); //取搜索的条数，用结束的位置-开始的位置,并返回 
                            int checklength = Int32.Parse(m_checkitem[tkeys[k]]);
                            if (my.Length < checklength)
                            {
                                MessageBox.Show(startstr + "内容应不得少于" + checklength.ToString() + "字");
                                IsChecked = false;
                                return;
                            }
                            else
                            {
                                IsChecked = true;
                            }
                        }
                    }
                }
                if (IsChecked)
                {
                    MyMessageBox.Show("所需验证元素均符合要求", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 关闭TabPage方法
        public void ClickTabPageCloseButton()
        {
            try
            {
                //todo 校验关闭的窗口是否需要保存
                if (CurrentInputTabPages.CurrentTabControl.SelectedTabPage.Tag is InCommonNoteEnmtity)
                {
                    CurrentInputTabPages.CurrentTabControl.TabPages.Remove(CurrentInputTabPages.CurrentTabControl.SelectedTabPage);
                }
                else
                {
                    if (CurrentInputTabPages.CurrentTabControl.SelectedTabPage.Tag is EmrModelContainer)
                    {
                        //todo 检验是否需要保存
                        m_TempContainerPages.Remove((EmrModelContainer)CurrentInputTabPages.CurrentTabControl.SelectedTabPage.Tag);
                        CurrentInputTabPages.CurrentTabControl.TabPages.Remove(CurrentInputTabPages.CurrentTabControl.SelectedTabPage);
                    }
                    else if (!CheckEditorIsSaved(m_CurrentModel))
                    {
                        //把病历的修改保存到临时变量中 EmrModel
                        CurrentForm.CurrentEditorControl.EMRDoc.ToXMLDocument(null == m_CurrentModel ? new XmlDocument() : m_CurrentModel.ModelContent);

                        string fileName = m_CurrentModel.ModelName;

                        if (MyMessageBox.Show(fileName + " 已经修改，您是否保存？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Yes)
                        {
                            Save();
                            DeleteTablePage(m_CurrentModel);
                        }
                        else
                        {
                            if (m_CurrentModel.InstanceId == -1)
                            {
                                UnRegisterEvent();
                                //删除TreeList中的对应的节点
                                DeleteTheNodeNotSaved(m_CurrentModel, treeListEmrNodeList.Nodes);
                                RegisterEvent();
                            }
                            DeleteTablePage(m_CurrentModel);
                        }
                    }
                    else
                    {
                        DeleteTablePage(m_CurrentModel);
                    }
                }
                ResetBarBtnState(treeListEmrNodeList.FocusedNode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 检查病历是否已经保存
        /// </summary>
        /// <returns></returns>
        public bool CheckEditorIsSaved(EmrModel emrModel)
        {
            try
            {
                if (emrModel == null)
                {
                    return false;
                }
                if (emrModel.InstanceId == -1)
                {
                    return false;
                }
                if (CurrentForm.CurrentEditorControl.EMRDoc.Modified)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 检查病历是否已经提交
        /// </summary>
        /// <returns></returns>
        public bool CheckEditorIsSubmit(EmrModel emrModel)
        {
            try
            {
                if (emrModel == null)
                {
                    return false;
                }
                if (emrModel.State == ExamineState.NotSubmit)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 删除左侧树中当前节点
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-17</date>
        /// <param name="model"></param>
        private bool DeleteTheNodeNotSaved(EmrModel model, TreeListNodes nodes)
        {
            try
            {
                if (null == model || null == nodes || nodes.Count == 0)
                {
                    return false;
                }

                foreach (TreeListNode node in nodes)
                {
                    if (node.Tag == model)
                    {
                        if (null != node.ParentNode && null != node.ParentNode.Nodes && node.ParentNode.Nodes.Count == 1)
                        {
                            node.ParentNode.Expanded = false;
                        }
                        nodes.Remove(node);
                        return true;
                    }
                    if (null != node && null != node.Nodes && node.Nodes.Count > 0)
                    {
                        if (DeleteTheNodeNotSaved(model, node.Nodes))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除所有Tab页
        /// </summary>
        private void RemoveAllTabPage()
        {
            try
            {
                int pageCount = xtraTabControlEmr.TabPages.Count;
                if (pageCount > 0)
                {
                    for (int i = pageCount - 1; i > 0; i--)
                    {
                        xtraTabControlEmr.TabPages.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除所有新增状态的Tab页
        /// </summary>
        private void RemoveAllNewTabPage()
        {
            try
            {
                int pageCount = xtraTabControlEmr.TabPages.Count;
                if (pageCount > 0)
                {
                    for (int i = pageCount - 1; i >= 0; i--)
                    {
                        XtraTabPage page = xtraTabControlEmr.TabPages[i];
                        xtraTabControlEmr.SelectedTabPage = page;
                        if (m_TempModelPages.Any(p => p.Value == page))
                        {
                            EmrModel model = m_TempModelPages.FirstOrDefault(p => p.Value == page).Key;
                            if (null != model && model.InstanceId == -1 && null != CurrentForm)
                            {
                                ClickTabPageCloseButton();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除新增状态的首次病程
        /// </summary>
        private void RemoveNewFirstEmrTabPage()
        {
            try
            {
                int pageCount = xtraTabControlEmr.TabPages.Count;
                if (pageCount > 0)
                {
                    for (int i = pageCount - 1; i >= 0; i--)
                    {
                        XtraTabPage page = xtraTabControlEmr.TabPages[i];
                        xtraTabControlEmr.SelectedTabPage = page;
                        if (m_TempModelPages.Any(p => p.Value == page && p.Key.FirstDailyEmrModel && p.Key.InstanceId == -1))
                        {
                            EmrModel model = m_TempModelPages.FirstOrDefault(p => p.Value == page && p.Key.FirstDailyEmrModel && p.Key.InstanceId == -1).Key;
                            if (null != model && model.InstanceId == -1 && null != CurrentForm)
                            {
                                DeleteTheNodeNotSaved(model, null == treeListEmrNodeList.FocusedNode.ParentNode ? treeListEmrNodeList.FocusedNode.Nodes : treeListEmrNodeList.FocusedNode.ParentNode.Nodes);
                                xtraTabControlEmr.TabPages.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 更改TreeListNode和TabPage的名字，针对在病程中修改病程名称和时间
        public void ReSetTreeListNodeAndTabPageName(string oldDateTime, string newDateTime, string oldTitle, string newTitle)
        {
            try
            {
                TreeListNode node = treeListEmrNodeList.FocusedNode;
                if (node != null && node.Tag is EmrModel)
                {
                    EmrModel model = node.Tag as EmrModel;
                    if (model.DailyEmrModel) //病程
                    {
                        if (!string.IsNullOrEmpty(newDateTime))
                        {
                            string title = model.ModelName.Split(' ')[0];
                            string date = model.ModelName.Split(' ')[1];
                            string time = model.ModelName.Split(' ')[2];
                            string creater = model.ModelName.Split(' ')[3];

                            model.ModelName = title + " " + (newDateTime == null ? date + " " + time : newDateTime) + " " + creater;
                            model.DisplayTime = newDateTime == null ? model.DisplayTime : Convert.ToDateTime(newDateTime);
                            node.SetValue("colName", model.ModelName);
                            xtraTabControlEmr.SelectedTabPage.Text = model.ModelName;
                        }
                        if (!string.IsNullOrEmpty(newTitle))
                        {
                            model.Description = newTitle;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重新设置该节点的顺序
        /// </summary>
        /// <param name="oldDateTime"></param>
        /// <param name="newDateTime"></param>
        /// <param name="node"></param>
        public void ReSortTreeNode(string oldDateTime, string newDateTime, TreeListNode node)
        {
            try
            {
                if (string.IsNullOrEmpty(newDateTime) || null == node)
                {
                    return;
                }

                var modelList = Util.GetAllEmrModels(treeListEmrNodeList.Nodes, null).OrderBy(q => q.DisplayTime);
                DateTime currentTime = DateTime.Parse(newDateTime);
                if (!string.IsNullOrEmpty(oldDateTime))
                {
                    DateTime oldTime = DateTime.Parse(oldDateTime);
                    DateTime minTime = currentTime > oldTime ? oldTime : currentTime;
                    DateTime maxTime = currentTime > oldTime ? currentTime : oldTime;
                    if (!modelList.Any(p => p.DisplayTime > minTime && p.DisplayTime < maxTime))
                    {///该节点时间并未越过其它节点
                        return;
                    }
                    else if (currentTime > oldTime)
                    {///将时间改大

                    }
                    else
                    {///将时间改小

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region 事件注册
        private void RegisterEvent()
        {
            try
            {
                treeListEmrNodeList.FocusedNodeChanged -= new FocusedNodeChangedEventHandler(treeListEmrNodeList_FocusedNodeChanged);
                treeListEmrNodeList.FocusedNodeChanged += new FocusedNodeChangedEventHandler(treeListEmrNodeList_FocusedNodeChanged);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UnRegisterEvent()
        {
            try
            {
                treeListEmrNodeList.FocusedNodeChanged -= new FocusedNodeChangedEventHandler(treeListEmrNodeList_FocusedNodeChanged);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 提取病历
        private DataTable InitDataCollect()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CONTENT");
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 表格相关
        /// <summary>
        /// 插入表格
        /// </summary>
        public void InsertTable()
        {
            try
            {
                if (null == m_CurrentModel || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                TableInsert tableInsertForm = new TableInsert();
                if (tableInsertForm.ShowDialog() == DialogResult.OK)
                {
                    CurrentForm.CurrentEditorControl.EMRDoc.TableInsert(tableInsertForm.Header,
                        tableInsertForm.Rows, tableInsertForm.Columns,
                        tableInsertForm.ColumnWidth);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入行
        /// </summary>
        public void InsertTableRow()
        {
            try
            {
                if (null == m_CurrentModel || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                RowsInsert rowsInsertForm = new RowsInsert();
                if (rowsInsertForm.ShowDialog() == DialogResult.OK)
                {
                    CurrentForm.CurrentEditorControl.EMRDoc.RowInsert(rowsInsertForm.RowNum, rowsInsertForm.IsBack);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入列
        /// </summary>
        public void InsertTableColumn()
        {
            try
            {
                if (null == m_CurrentModel || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                ColumnsInsert columnsInsertForm = new ColumnsInsert();
                if (columnsInsertForm.ShowDialog() == DialogResult.OK)
                {
                    CurrentForm.CurrentEditorControl.EMRDoc.ColumnInsert(columnsInsertForm.ColumnNum, columnsInsertForm.IsBack);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除表格
        /// </summary>
        public void DeleteTable()
        {
            try
            {
                if (null == m_CurrentModel || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                if (MyMessageBox.Show("您确定要删除光标所在的表格吗？", "提示", MyMessageBoxButtons.OkCancel, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.OK)
                {
                    CurrentForm.CurrentEditorControl.EMRDoc.TableDelete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        public void DeleteTableRow()
        {
            try
            {
                if (null == m_CurrentModel || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                if (MyMessageBox.Show("您确定要删除光标所在的行吗？", "提示", MyMessageBoxButtons.OkCancel, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.OK)
                {
                    CurrentForm.CurrentEditorControl.EMRDoc.TableDeleteRow();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除列
        /// </summary>
        public void DeleteTableColumn()
        {
            try
            {
                if (null == m_CurrentModel || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                if (MyMessageBox.Show("您确定要删除光标所在的列吗？", "提示", MyMessageBoxButtons.OkCancel, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.OK)
                {
                    CurrentForm.CurrentEditorControl.EMRDoc.TableDeleteCol();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 选中表格
        /// </summary>
        public void ChoiceTable()
        {
            try
            {
                if (null == m_CurrentModel || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                CurrentForm.CurrentEditorControl.EMRDoc.TableSelect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 表格属性
        /// </summary>
        public void SetTableProperty()
        {
            try
            {
                if (null == m_CurrentModel || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                TPTextTable table = CurrentForm.CurrentEditorControl.EMRDoc.GetCurrentTable();
                TPTextCell cell = CurrentForm.CurrentEditorControl.EMRDoc.GetCurrentCell();
                if (table != null && cell != null)
                {
                    TableProperty tablePropertyForm = new TableProperty(table, cell);
                    if (tablePropertyForm.ShowDialog() == DialogResult.OK)
                    {

                        CurrentForm.CurrentEditorControl.EMRDoc.ContentChanged();
                        CurrentForm.CurrentEditorControl.EMRDoc.Refresh();
                    }
                }
                else
                {
                    MyMessageBox.Show("光标必须在表格中才能更改表格属性", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 刷新文书录入
        /// <summary>
        /// 重新加载整个文书录入界面
        /// </summary>
        public void RefreshEMRMainPad()
        {
            try
            {
                m_app.ChoosePatient(m_CurrentInpatient.NoOfFirstPage);
                m_app.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询病历需要插入的索引位置
        /// </summary>
        /// 注：只针对病程记录
        /// <param name="node"></param>
        /// <returns></returns>
        private int FindModelIndex(TreeListNode node)
        {
            try
            {
                if (null == node || null == node.Tag || !(node.Tag is EmrModel) || null == node.TreeList)
                {
                    return -1;
                }
                int index = node.TreeList.GetNodeIndex(node);
                EmrModel model = node.Tag as EmrModel;
                if (null != node.PrevNode && (((EmrModel)node.PrevNode.Tag)).DisplayTime > model.DisplayTime)
                {///此节点应前移
                    int theNodeIndex = index;
                    for (int i = theNodeIndex - 1; i >= 0; i--)
                    {
                        TreeListNode nd = node.ParentNode.Nodes[i];
                        EmrModel theModel = (EmrModel)nd.Tag;
                        if (theModel.DisplayTime <= model.DisplayTime)
                        {
                            break;
                        }
                        if (null == nd.PrevNode)
                        {
                            index = node.TreeList.GetNodeIndex(nd);
                            break;
                        }
                        else
                        {
                            index = node.ParentNode.Nodes.IndexOf(nd);
                        }
                    }
                }
                else if (null != node.NextNode && (((EmrModel)node.NextNode.Tag)).DisplayTime < model.DisplayTime)
                {///此节点应后移
                    int theNodeIndex = index;
                    for (int j = theNodeIndex + 1; j < node.ParentNode.Nodes.Count; j++)
                    {
                        TreeListNode nd = node.ParentNode.Nodes[j];
                        if (null == nd.Tag || !(nd.Tag is EmrModel))
                        {
                            continue;
                        }
                        EmrModel theModel = (EmrModel)nd.Tag;
                        if (theModel.DisplayTime >= model.DisplayTime)
                        {
                            break;
                        }
                        if (null == nd.NextNode)
                        {
                            index = node.TreeList.GetNodeIndex(nd);
                            break;
                        }
                        else
                        {
                            index = node.TreeList.GetNodeIndex(nd);
                        }
                    }
                }
                node.SetValue("colName", model.ModelName);

                return index;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询病历科室需要插入的索引位置
        /// </summary>
        /// 注：只针对转科记录
        /// <param name="node"></param>
        /// <returns></returns>
        private int FindDeptContainerIndex(TreeListNode node)
        {
            try
            {
                if (null == node || null == node.Tag || !(node.Tag is EmrModelDeptContainer) || null == node.TreeList)
                {
                    return -1;
                }
                int index = node.TreeList.GetNodeIndex(node);
                EmrModelDeptContainer container = node.Tag as EmrModelDeptContainer;
                if (null != node.PrevNode && (((EmrModelDeptContainer)node.PrevNode.Tag)).CreateTime > container.CreateTime)
                {///此节点应前移
                    int theNodeIndex = index;
                    for (int i = theNodeIndex - 1; i >= 0; i--)
                    {
                        TreeListNode nd = node.ParentNode.Nodes[i];
                        EmrModelDeptContainer theContainer = (EmrModelDeptContainer)nd.Tag;
                        if (theContainer.CreateTime <= container.CreateTime)
                        {
                            break;
                        }
                        if (null == nd.PrevNode)
                        {
                            index = node.TreeList.GetNodeIndex(nd);
                            break;
                        }
                        else
                        {
                            index = node.ParentNode.Nodes.IndexOf(nd);
                        }
                    }
                }
                else if (null != node.NextNode && (((EmrModelDeptContainer)node.NextNode.Tag)).CreateTime < container.CreateTime)
                {///此节点应后移
                    int theNodeIndex = index;
                    for (int j = theNodeIndex + 1; j < node.ParentNode.Nodes.Count; j++)
                    {
                        TreeListNode nd = node.ParentNode.Nodes[j];
                        if (null == nd.Tag || !(nd.Tag is EmrModelDeptContainer))
                        {
                            continue;
                        }
                        EmrModelDeptContainer theContainer = (EmrModelDeptContainer)nd.Tag;
                        if (theContainer.CreateTime >= container.CreateTime)
                        {
                            break;
                        }
                        if (null == nd.NextNode)
                        {
                            index = node.TreeList.GetNodeIndex(nd);
                            break;
                        }
                        else
                        {
                            index = node.TreeList.GetNodeIndex(nd);
                        }
                    }
                }
                node.SetValue("colName", container.Name);

                return index;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置当前节点顺序
        /// </summary>
        /// 注：只针对病程记录
        /// <param name="node">当前节点</param>
        /// <param name="type">操作类型：0-EmrModel；1-EmrModelDeptContainer</param>
        public void SetTreeNodeIndex(TreeListNode node, int type)
        {
            try
            {
                if (null == node || null == node.Tag)
                {
                    return;
                }
                if (type == 0 && (!(node.Tag is EmrModel) || ((EmrModel)node.Tag).ModelCatalog != ContainerCatalog.BingChengJiLu))
                {
                    return;
                }
                if (type == 1 && (!(node.Tag is EmrModelDeptContainer) || ((EmrModelDeptContainer)node.Tag).ContaineCatalog != ContainerCatalog.BingChengJiLu))
                {
                    return;
                }
                int index = -1;
                if (type == 0)
                {
                    index = FindModelIndex(node);
                }
                else if (type == 1)
                {
                    index = FindDeptContainerIndex(node);
                }
                if (index != -1)
                {
                    treeListEmrNodeList.SetNodeIndex(node, index);
                }
                treeListEmrNodeList.MakeNodeVisible(node);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 刷新病程记录
        /// <summary>
        /// 获取病程记录文件夹节点
        /// </summary>
        /// <param name="nodes"></param>
        private TreeListNode GetBingChengJiLuNode(TreeListNodes nodes)
        {
            try
            {
                if (null == nodes || nodes.Count == 0)
                {
                    return null;
                }
                foreach (TreeListNode node in nodes)
                {
                    if (null != node && null != node.Tag && node.Tag is EmrModelContainer)
                    {
                        EmrModelContainer container = node.Tag as EmrModelContainer;
                        if (container.ContainerCatalog == ContainerCatalog.BingChengJiLu)
                        {
                            return node;
                        }
                    }
                    if (null != node && null != node.Nodes)
                    {
                        TreeListNode returnNode = GetBingChengJiLuNode(node.Nodes);
                        if (null != returnNode)
                        {
                            return returnNode;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刷新病程记录
        /// </summary>
        public void RefreshBingChengJiLu()
        {
            try
            {
                ///获取病程记录文件夹节点
                TreeListNode bingChengJiLuNode = GetBingChengJiLuNode(treeListEmrNodeList.Nodes);
                if (null == bingChengJiLuNode)
                {
                    return;
                }
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在刷新数据...");
                ///删除新增状态的首次病程
                RemoveNewFirstEmrTabPage();
                ///RemoveAllNewTabPage();
                ///获取转科/病区记录
                DataTable changeDepts = DS_SqlService.GetInpatientChangeInfo((int)m_CurrentInpatient.NoOfFirstPage);
                if (null == changeDepts || changeDepts.Rows.Count == 0)
                {
                    bingChengJiLuNode.Nodes.Clear();
                    return;
                }
                ///遍历转科/病区记录且更新科室文件夹节点
                foreach (DataRow row in changeDepts.Rows)
                {
                    bool isUpdate = false;
                    foreach (TreeListNode node in bingChengJiLuNode.Nodes)
                    {
                        if (null != node && null != node.Tag && node.Tag is EmrModelDeptContainer)
                        {
                            EmrModelDeptContainer container = node.Tag as EmrModelDeptContainer;
                            if (container.ChangeID == row["id"].ToString())
                            {///更新
                                AddOrUpdateContainerNode(node, bingChengJiLuNode, row, 1);
                                isUpdate = true;
                                break;
                            }
                        }
                    }
                    if (!isUpdate)
                    {///新增
                        AddOrUpdateContainerNode(null, bingChengJiLuNode, row, 0);
                        continue;
                    }
                }

                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                DS_Common.HideWaitDialog(m_WaitDialog);
                throw ex;
            }
        }

        /// <summary>
        /// 更新科室文件夹下的病程节点
        /// </summary>
        /// <param name="node">病程科室节点</param>
        /// <param name="noofinpat">病案号</param>
        private void RefreshModelNodes(TreeListNode node, int noofinpat)
        {
            try
            {
                if (null == node || null == node.Tag || !(node.Tag is EmrModelDeptContainer))
                {
                    return;
                }
                EmrModelDeptContainer container = node.Tag as EmrModelDeptContainer;
                ///获取该科室文件夹下所有病历(包含已删除)
                DataTable leafs = m_PatUtil.GetPatRecInfoContainsDel(container.ContaineCatalog, noofinpat.ToString(), string.IsNullOrEmpty(container.ChangeID) ? -1 : int.Parse(container.ChangeID));
                if (null == leafs || leafs.Rows.Count == 0)
                {
                    return;
                }
                foreach (DataRow leaf in leafs.Rows)
                {
                    bool isUpdate = false;
                    foreach (TreeListNode leafNode in node.Nodes)
                    {
                        if (null != leafNode && null != leafNode.Tag && leafNode.Tag is EmrModel)
                        {
                            EmrModel model = leafNode.Tag as EmrModel;
                            if (model.InstanceId != Convert.ToInt32(leaf["id"]))
                            {
                                continue;
                            }
                            if (Convert.ToInt32(leaf["valid"]) == 0)
                            {
                                node.Nodes.Remove(leafNode);
                                if (m_TempModelPages.Any(p => p.Key.InstanceId == model.InstanceId))
                                {///删除Tab页
                                    var modelAndPage = m_TempModelPages.FirstOrDefault(p => p.Key.InstanceId == model.InstanceId);
                                    xtraTabControlEmr.TabPages.Remove(modelAndPage.Value);
                                    CurrentInputTabPages.SelectedPages.Remove(modelAndPage.Value);
                                    m_TempModelPages.Remove(modelAndPage.Key);
                                }
                                isUpdate = true;
                                break;
                            }
                            else
                            {
                                AddOrUpdateModelNode(leafNode, node, leaf, 1);
                                isUpdate = true;
                                break;
                            }
                        }
                    }
                    if (!isUpdate && Convert.ToInt32(leaf["valid"]) != 0)
                    {
                        AddOrUpdateModelNode(null, node, leaf, 0);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加或更新科室节点
        /// </summary>
        /// <param name="node">需要操作的节点</param>
        /// <param name="parentNode">父节点</param>
        /// <param name="row">转科记录</param>
        /// <param name="type">操作类型：0-新增；1-更新</param>
        private void AddOrUpdateContainerNode(TreeListNode node, TreeListNode parentNode, DataRow row, int type)
        {
            try
            {
                EmrModelDeptContainer deptContainer = new EmrModelDeptContainer(row);
                if (type == 0)
                {
                    TreeListNode deptNode = treeListEmrNodeList.AppendNode(new object[] { deptContainer.Name }, parentNode);
                    deptNode.Tag = deptContainer;
                    SetTreeNodeIndex(deptNode, 1);
                    RefreshModelNodes(deptNode, (int)m_CurrentInpatient.NoOfFirstPage);
                }
                else if (type == 1)
                {
                    node.Tag = deptContainer;
                    node.SetValue("colName", deptContainer.Name);
                    SetTreeNodeIndex(node, 1);
                    RefreshModelNodes(node, (int)m_CurrentInpatient.NoOfFirstPage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加或更新病程节点
        /// </summary>
        /// <param name="node">需要操作的节点</param>
        /// <param name="parentNode">父节点</param>
        /// <param name="row">病程记录</param>
        /// <param name="type">操作类型：0-新增；1-更新</param>
        private void AddOrUpdateModelNode(TreeListNode node, TreeListNode parentNode, DataRow row, int type)
        {
            try
            {
                EmrModel model = new EmrModel(row);
                if (type == 0)
                {///添加
                    TreeListNode modelNode = treeListEmrNodeList.AppendNode(new object[] { model.ModelName }, parentNode);
                    modelNode.Tag = model;
                    SetTreeNodeIndex(modelNode, 0);
                }
                else if (type == 1)
                {///更新
                    if (null == node)
                    {
                        return;
                    }
                    ///1、更新节点
                    node.Tag = model;
                    node.SetValue("colName", model.ModelName);
                    if (m_TempModelPages.Any(p => p.Key.InstanceId == model.InstanceId))
                    {///2、更新Tab页
                        var modelAndPage = m_TempModelPages.FirstOrDefault(p => p.Key.InstanceId == model.InstanceId);
                        xtraTabControlEmr.TabPages.Remove(modelAndPage.Value);
                        CurrentInputTabPages.SelectedPages.Remove(modelAndPage.Value);
                        m_TempModelPages.Remove(modelAndPage.Key);
                        FocusedNodeChanged(node);
                        //CreateNewDocument(model);
                        //CurrentForm.CurrentEditorControl.EMRDoc.Modified = false;
                    }
                    SetTreeNodeIndex(node, 0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void treeListPersonTemplate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right || null == CurrentForm || !CurrentForm.CurrentEditorControl.CanEdit())
                {
                    return;
                }
                TreeList list = (TreeList)sender;
                if ((list.FocusedNode != null) && (list.FocusedNode.Tag is TempletItem))
                {


                    TreeListNode node = treeListPersonTemplate.FocusedNode;
                    if (null == node || null == node.Tag)
                    {
                        return;
                    }
                    object item = new object();
                    item = node.Tag;

                    if (null == item)
                    {
                        return;
                    }
                    CurrentForm.CurrentEditorControl.EMRDoc._InserString(((DrectSoft.Core.PersonTtemTemplet.TempletItem)(item)).Content);
                    CurrentForm.CurrentEditorControl.Focus();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}
