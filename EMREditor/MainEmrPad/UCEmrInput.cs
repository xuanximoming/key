using DevExpress.Utils;
using DevExpress.XtraBars;
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
using DrectSoft.Core.IEMMainPage;
using DrectSoft.Core.MainEmrPad.HelpForm;
using DrectSoft.Core.MainEmrPad.HistoryEMR;
using DrectSoft.Core.MainEmrPad.ThreeLevelCheck;
using DrectSoft.Core.PersonTtemTemplet;
using DrectSoft.DSSqlHelper;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Common;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Library.EmrEditor.Src.Gui;
using DrectSoft.Library.EmrEditor.Src.Print;
using DrectSoft.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using XWriterDemo;

/*
 * 添加病历节点的时候需要注意按如下顺序，否则会导致最后一步SelectedPageChanged事件计算的不正确：
 * 1.在树中添加节点
 * 2.在节点的Tag中添加EmrModel
 * 3.设置界面的TabPage
 * 4.设置当前TabControl选中TabPage 【xtraTabControl1.SelectedTabPage = newPad】
 * 5.触发TabControl事件xtraTabControl1_SelectedPageChanged，计算出当前病历界面 “对应树中的节点”，“当前的EmrModel”，“当前的EmrModelContainer”
 */
namespace DrectSoft.Core.MainEmrPad
{
    public partial class UCEmrInput : DevExpress.XtraEditors.XtraUserControl //, IStartPlugIn
    {
        #region Field && Property                     edit by cyq 2013-01-23 添加 try...catch...
        PersonItemManager m_PersonItem;
        DataTable m_MyTreeFolders;
        DataTable m_MyLeafs;
        ItemCatalog m_ItemCatalog;
        ItemContentForm m_ItemContent;
        bool isSaved = true;//病程是否保存成功
        private FloderState floaderState = FloderState.Default; //add by cyq 2012-12-06 左侧菜单文件夹状态(病案管理专用)
        public List<string[]> m_RecordUpdateProcessList = new List<string[]>();

        private IEmrHost m_app;
        private IDataAccess sqlHelper;

        Inpatient m_CurrentInpatient;
        public Inpatient CurrentInpatient
        {
            get
            {
                return m_CurrentInpatient;
            }
            set
            {
                m_CurrentInpatient = value;
            }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        private Employee CurrentEmployee
        {
            get
            {
                if ((_currentEmployee == null) || (!_currentEmployee.Code.Equals(m_app.User.Id)))
                {
                    _currentEmployee = new DrectSoft.Common.Eop.Employee(m_app.User.Id);
                    _currentEmployee.ReInitializeProperties();
                }
                return _currentEmployee;
            }
        }
        private Employee _currentEmployee;

        /// <summary>
        /// 是否可以新增病历
        /// </summary>
        private bool CanAddNew
        {
            get
            {
                IEmrModelPermision modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Create, CurrentEmployee);
                return modelPermision.CanDo(null);
            }
        }
        private bool _canAddNew;

        PatRecUtil m_patUtil;

        RecordDal m_RecordDal;

        /// <summary>
        /// 当前界面中编辑器对应的病历模型
        /// 【在SelectedPageChanged事件中计算出来】
        /// </summary>
        EmrModel m_CurrentModel;

        /// <summary>
        /// 当前界面中编辑器对应的病历模型对应的树节点
        /// 【在SelectedPageChanged事件中计算出来】
        /// </summary>
        TreeListNode m_CurrentTreeListNode;

        /// <summary>
        /// 当前界面中编辑器对应的病历文件夹模型
        /// 【在SelectedPageChanged事件中计算出来】
        /// </summary>
        EmrModelContainer m_CurrentModelContainer;

        /// <summary>
        /// 当前选中的非病历类型的界面
        /// 如首页、三测单之类的内容
        /// 【在SelectedPageChanged事件中计算出来】
        /// </summary>
        IEMREditor m_CurrentEmrEditor;

        /// <summary>
        /// 当前已经打开的病历模型集合
        /// </summary>
        Dictionary<EmrModel, XtraTabPage> m_TempModelPages;

        /// <summary>
        /// 当前已经打开的非病历类型的界面如首页、三测单等的集合
        /// </summary>
        Dictionary<EmrModelContainer, XtraTabPage> m_TempContainerPages;


        /// <summary>
        /// 界面中调用的等待窗体
        /// </summary>
        private WaitDialogForm m_WaitDialog;

        /// <summary>
        /// 当前用户的信息
        /// </summary>
        Employee DoctorEmployee
        {
            get
            {
                if (_doctorEmployye == null)
                {
                    _doctorEmployye = new Employee(m_app.User.Id);
                    _doctorEmployye.ReInitializeProperties();
                }
                _doctorEmployye.CurrentDept.Code = m_app.User.CurrentDeptId;
                _doctorEmployye.CurrentWard.Code = m_app.User.CurrentWardId;
                return _doctorEmployye;
            }
        }
        Employee _doctorEmployye;

        /// <summary>
        /// 当前界面选中的编辑器
        /// </summary>
        PadForm CurrentForm
        {

            get
            {
                if (xtraTabControl1.SelectedTabPage != null)
                {
                    if (xtraTabControl1.SelectedTabPage.Controls.Count > 0)
                    {
                        return xtraTabControl1.SelectedTabPage.Controls[0] as PadForm;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 弹出右键菜单开关
        /// </summary>
        private bool m_RightMouseButtonFlag = true;
        public bool RightMouseButtonFlag
        {
            get
            {
                return m_RightMouseButtonFlag;
            }
        }

        /// <summary>
        /// 编辑器是否可以编辑的开关
        /// </summary>
        private bool m_EditorEnableFlag = true;

        /// <summary>
        /// 是否正在保存病历
        /// </summary> 
        private bool m_IsSaving = false;

        /// <summary>
        /// 是否允许拖拽操作
        /// </summary>
        private bool m_IsAllowDrop = true;

        /// <summary>
        /// 是否允许激活编辑区域，激活区域用于病程
        /// </summary>
        bool m_IsNeedActiveEditArea = true;
        public bool IsNeedActiveEditArea
        {
            get
            {
                return m_IsNeedActiveEditArea;
            }
            set
            {
                m_IsNeedActiveEditArea = value;
            }
        }

        /// <summary>
        /// 是否需要在编辑器中定位病程
        /// </summary>
        public bool IsLocateDaily
        {
            get
            {
                return m_IsLocateDaily;
            }
            set
            {
                m_IsLocateDaily = value;
            }
        }
        bool m_IsLocateDaily = true;

        //外部选中病历对应的树节点
        TreeListNode m_CurrentSelectedEmrNode;
        #endregion

        #region .ctor
        public UCEmrInput()
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("创建病历编辑组件...", "请稍等...");
                InitializeComponent();
                m_ItemCatalog = new ItemCatalog(m_app);
                m_ItemContent = new ItemContentForm(m_app);
                LoadUCEmrInput();

                //背景色注册进去   addby ywk 2012年12月12日15:34:18 
                System.EventHandler SetColorHandler = new EventHandler(SetDocumentColor);
                menuColor.MenuItems.Add(new ColorMenuItem(System.Drawing.Color.Transparent, "Transparent", SetColorHandler));
                this.menuColor.MenuItems.AddRange(ColorMenuItem.CreateStandItems(SetColorHandler));
                this.menuColor.MenuItems.Add(new MenuItem("-"));
                ColorMenuItem item = new ColorMenuItem(System.Drawing.Color.Black, "其它颜色...", SetColorHandler);
                item.DialogSelectColor = true;
                menuColor.MenuItems.Add(item);

                btnSetPageBackColor.Visibility = BarItemVisibility.Never;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-12-04
        /// 1、add try ... catch
        /// 2、方法封装
        /// </summary>
        public UCEmrInput(string tip)
        {
            try
            {
                m_WaitDialog = new WaitDialogForm(tip, "请稍等");
                InitializeComponent();
                LoadUCEmrInput();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// edit by Yanqiao.Cai 2012-12-04
        /// 1、add try ... catch
        /// 2、方法封装
        /// 注：此方法为病案室人员加载首页编辑权限使用
        ///     可编辑所有病人病案首页；不可编辑任何病历(未归档选项卡)
        /// </summary>
        public UCEmrInput(string tip, FloderState state)
        {
            try
            {
                //左侧文件夹状态
                floaderState = state;
                m_WaitDialog = new WaitDialogForm(tip, "请稍等");
                InitializeComponent();
                LoadUCEmrInput();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体加载方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-04</date>
        /// </summary>
        private void LoadUCEmrInput()
        {
            try
            {
                xtraTabPageIEMMainPage.AccessibleName = ""; //为空则病案首页可以编辑，否则不显示编辑按钮
                MacroUtil.MacroSource = null;
                ExtraInit();
                InitDictionary();
                InitFont();
                InitDefaultFont();
                HideWaitDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SetInnerVar(IEmrHost app, RecordDal recordDal)
        {
            try
            {
                m_app = app;
                sqlHelper = m_app.SqlHelper;
                m_RecordDal = recordDal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void RegisterEvent()
        {
            try
            {
                btnSavePopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnSavePopup_ItemClick);
                btnDeletePopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnDeletePopup_ItemClick);
                btnSubmitPopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnSubmitPopup_ItemClick);
                btnConfirmPopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnConfirmPopup_ItemClick);
                btnModulePopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnModulePopup_ItemClick);
                btnCancelAuditPopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItem_CancelAudit_ItemClick);
                treeList1.BeforeExpand += new BeforeExpandEventHandler(treeList1_BeforeExpand);
                this.FindForm().Shown += new EventHandler(UCEmrInput_Shown);
                treeListHistory.MouseDoubleClick += new MouseEventHandler(treeListHistory_MouseDoubleClick);
                treeListHistory.GetStateImage += new GetStateImageEventHandler(treeListHistory_GetStateImage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void UCEmrInput_Shown(object sender, EventArgs e)
        {
            try
            {
                //自动显示病历批量导入界面
                AutoShowHistoryEmrBatchInForm();
                this.FindForm().Shown -= new EventHandler(UCEmrInput_Shown);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void UCEmrInput_Activated(object sender, EventArgs e)
        {
            try
            {
                //自动显示病历批量导入界面
                AutoShowHistoryEmrBatchInForm();
                this.FindForm().Activated -= new EventHandler(UCEmrInput_Activated);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置字体下拉框
        /// </summary>
        private void InitFont()
        {
            try
            {
                repositoryItemComboBoxFont.Items.AddRange(FontCommon.FontList.ToArray());

                //添加字号
                foreach (string fontsizename in FontCommon.allFontSizeName)
                {
                    repositoryItemComboBox_SiZe.Items.Add(fontsizename);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置默认字体下拉框 add by wwj 2012-05-29
        /// </summary>
        private void InitDefaultFont()
        {
            try
            {
                repositoryItemComboBox2.Items.Clear();
                repositoryItemComboBox2.Items.AddRange(new string[] { "五号", "小五", "六号" });
                barEditItemDefaultFontSize.EditValue = ZYEditorControl.DefaultFontSize;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置NavBarControl皮肤
        /// </summary>
        private void ExtraInit()
        {
            try
            {
                this.navBarControlToolBox.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Office 2010 Silver");
                this.navBarControlClinical.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Office 2010 Silver");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化用到的变量
        /// </summary>
        private void InitDictionary()
        {
            try
            {
                m_TempModelPages = new Dictionary<EmrModel, XtraTabPage>();
                m_TempContainerPages = new Dictionary<EmrModelContainer, XtraTabPage>();
                m_PersonItem = null;
                m_MyTreeFolders = null;
                m_MyLeafs = null;
                m_CurrentModel = null;
                m_CurrentModelContainer = null;
                m_CurrentTreeListNode = null;
                m_CurrentEmrEditor = null;
                m_RightMouseButtonFlag = true;
                m_EditorEnableFlag = true;
                m_IsSaving = false;
                m_IsAllowDrop = true;
                m_patUtil = null;
                m_IsNeedActiveEditArea = true;
                m_IsLocateDaily = true;
                m_NodeIEMMainPage = null;
                m_CurrentSelectedEmrNode = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Load                          edit by cyq 2013-01-23 添加 try...catch...
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                //暂时将医嘱隐藏
                navBarGroup_DoctorAdive.Visible = false;
                //暂时将编辑和只读按钮隐藏
                btn_Edit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btnEmrReadOnly.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //if (m_app.User.Id == "00")
                //{//暂改为医生都可以看到
                barButtonItemChangeMacro.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //}
                //病历编辑器设置为默认值
                InitEmrDefaultSetting();
                //设置页面设置
                InitEmrPageSetting();
                //初始化病人信息，初始化树节点，初始化工具栏
                bool isOK = GetPatRec();
                if (!isOK) return;
                //绑定病人基本信息
                BindExtraInfo();
                SetBarStatus(false);
                SetBarVisible();

                //绑定个人模板
                //InitPersonTree(true, string.Empty);

                if (m_NodeIEMMainPage != null) treeList1.FocusedNode = m_NodeIEMMainPage;
                //选中指定的病历
                SelectSpecifiedNode();
                //绑定病历信息
                InitBLMessage();

                CollapseContainEmrModelNode();

                //读取配置，控制调用PACS影像按钮的可见性
                string cfgvalue = GetConfigValueByKey("EmrInputConfig");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cfgvalue);
                //textEdit35.Text = doc.GetElementsByTagName("引流量OR尿量")[0].InnerText.Trim();
                if (doc.GetElementsByTagName("BtnPacsImageVisable")[0].InnerText == "0")
                {
                    barButtonItem13.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    barButtonItem13.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
                //读取配置，控制出科检查按钮的可见性（2012年7月29日 09:37:37）add by ywk 
                if (doc.GetElementsByTagName("IsShowCheckBtn")[0].InnerText == "0")
                {
                    barButtonItem18.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    barButtonItem18.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                #region 配置历史病历导入的可见性 ywk 2012年12月7日10:45:33
                //配置是否显示历史病历导入的按钮 add by ywk 2012年12月7日10:42:22 
                //现历史病历导入有问题，容易造成病程记录左右节点不对应（病历丢失），且历史病历导入后，其实导入的病历
                //的创建人就应该是当前操作用户，应该可以编辑病程记录 
                ///edit by cyq 2013-02-21
                if (IsAutoShowHistoryEmrBatchInForm())
                {
                    btnHistoryEmrBatchIn.Enabled = true;
                }
                else
                {
                    btnHistoryEmrBatchIn.Enabled = false;
                }
                //string IsShowExportHistory = GetConfigValueByKey("IsShowExportHistory");
                //if (IsShowExportHistory == "0")
                //{
                //    btnHistoryEmrBatchIn.Enabled = false;
                //}
                //else
                //{
                //    btnHistoryEmrBatchIn.Enabled = true;
                //}
                #endregion

                RegisterEvent();

            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 根据登录人的身份判断控件的显示
        /// </summary>
        private void SetBarVisible()
        {
            try
            {
                if (DoctorEmployee.Grade.Trim() == "")
                {
                    barEditItem_FontSize.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barStaticItem22.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barEditItemDefaultFontSize.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);
                    if (grade == DoctorGrade.Nurse)
                    {
                        barEditItem_FontSize.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        barStaticItem22.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        barEditItemDefaultFontSize.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }
                    else
                    {
                        barEditItem_FontSize.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        barStaticItem22.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        barEditItemDefaultFontSize.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CollapseContainEmrModelNode()
        {
            try
            {
                treeList1.FocusedNodeChanged -= new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
                CollapseContainEmrModelNodeInner(treeList1.Nodes);
                treeList1.FocusedNodeChanged += new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CollapseContainEmrModelNodeInner(TreeListNodes nodes)
        {
            try
            {
                foreach (TreeListNode node in nodes)
                {
                    if (node.Nodes.Count > 0)
                    {
                        CollapseContainEmrModelNodeInner(node.Nodes);
                    }
                    else if (node.Tag is EmrModel)
                    {
                        if (m_CurrentSelectedEmrNode != null)
                        {
                            if (m_CurrentSelectedEmrNode.ParentNode == node.ParentNode)
                            {
                                continue;
                            }
                        }
                        node.ParentNode.Expanded = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 选中指定的病历
        /// </summary>
        private void SelectSpecifiedNode()
        {
            try
            {
                if (m_app.CurrentSelectedEmrID == "") return;
                SelectSpecifiedNodeInner(treeList1.Nodes);
                m_app.CurrentSelectedEmrID = "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SelectSpecifiedNodeInner(TreeListNodes nodes)
        {
            try
            {
                if (m_app.CurrentSelectedEmrID == "") return;
                foreach (TreeListNode node in nodes)
                {
                    if (node.Nodes.Count > 0)
                    {
                        SelectSpecifiedNodeInner(node.Nodes);
                    }
                    else
                    {
                        EmrModel model = node.Tag as EmrModel;
                        if (model != null)
                        {
                            if (model.InstanceId.ToString() == m_app.CurrentSelectedEmrID)
                            {
                                m_CurrentSelectedEmrNode = node;//记录外部选中病历对应树中的节点
                                FocusedNodeChanged(node);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 病历编辑器默认设置
        /// </summary>
        private void InitEmrDefaultSetting()
        {
            try
            {
                if (m_app.EmrDefaultSettings.LineSpace != "")
                {
                    PadForm.s_LineSpace = float.Parse(m_app.EmrDefaultSettings.LineSpace);
                }
                else
                {
                    PadForm.s_LineSpace = 1.5f;
                }
                PadForm.s_ElementColor = m_app.EmrDefaultSettings.EleColor;
                PadForm.s_ElementStyle = m_app.EmrDefaultSettings.EleStyle;
                //PadForm.s_LineHeight = Convert.ToInt32(m_app.EmrDefaultSettings.LineHeight);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 编辑器页面设置
        /// </summary>
        private void InitEmrPageSetting()
        {
            try
            {
                string xmldoc = BasicSettings.GetStringConfig("PageSetting");
                System.Xml.XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmldoc);
                XmlNode pagesetting = doc.ChildNodes[0].SelectSingleNode("pagesettings");
                if (pagesetting != null)
                {
                    XmlElement ele = (pagesetting as XmlElement).SelectSingleNode("page") as XmlElement;
                    XPageSettings ps = new XPageSettings();

                    // 暂时开放设置统一的纸张大小 Modified By wwj 2012-02-27
                    ps.PaperSize.Kind = (PaperKind)Enum.Parse(typeof(PaperKind), ele.GetAttribute("kind"));

                    if (ps.PaperSize.Kind == PaperKind.Custom)
                    {
                        ps.PaperSize.Height = int.Parse(ele.GetAttribute("height"));
                        ps.PaperSize.Width = int.Parse(ele.GetAttribute("width"));
                    }

                    ele = (pagesetting as XmlElement).SelectSingleNode("margins") as XmlElement;
                    ps.Margins.Left = int.Parse(ele.GetAttribute("left"));
                    ps.Margins.Right = int.Parse(ele.GetAttribute("right"));
                    ps.Margins.Top = int.Parse(ele.GetAttribute("top"));
                    ps.Margins.Bottom = int.Parse(ele.GetAttribute("bottom"));

                    ele = (pagesetting as XmlElement).SelectSingleNode("landscape") as XmlElement;
                    ps.Landscape = bool.Parse(ele.GetAttribute("value"));

                    //Add By wwj 2012-02-16 页眉和页脚高度设置
                    ele = (pagesetting as XmlElement).SelectSingleNode("header") as XmlElement;
                    ps.HeaderHeight = int.Parse(ele.GetAttribute("height"));
                    ele = (pagesetting as XmlElement).SelectSingleNode("footer") as XmlElement;
                    ps.FooterHeight = int.Parse(ele.GetAttribute("height"));

                    PadForm.m_DefaultPageSetting = ps;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化病人信息，初始化树节点，初始化工具栏
        /// </summary>
        private bool GetPatRec()
        {
            try
            {
                if (m_CurrentInpatient == null)
                {
                    //m_app.CustomMessageBox.MessageShow("请先选择病人！");
                    return false;
                }
                SetWaitDialogCaption("正在读取患者病历数据...");
                //读取基本信息
                BindPatBasic();
                //构造病历树
                InitPatTree();
                //初始化右侧工具栏
                InitTool();
                //选中默认的右侧工具栏
                SelectDefaultRightTool();
                HideWaitDialog();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 选中默认的右侧工具栏
        /// </summary>
        private void SelectDefaultRightTool()
        {
            try
            {
                this.panelContainer1.ActiveChild = this.dockPanel2;
                navBarGroup_Symb.Expanded = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 绑定病人基本信息
        /// </summary>
        private void BindPatBasic()
        {
            try
            {
                SetPatientInfo(Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化左侧的树
        /// </summary>
        private void InitPatTree()
        {
            try
            {
                treeList1.FocusedNodeChanged -= new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
                treeList1.ClearNodes();
                //读取底层文件夹
                if (m_patUtil == null)
                {
                    m_patUtil = new PatRecUtil(m_app, m_CurrentInpatient);
                }
                string simpleDoc = GetConfigValueByKey("SimpleDoctorCentor");
                DataTable datatable;
                if (simpleDoc == "1")
                {
                    datatable = m_patUtil.GetFolderInfoSimpleDoc("00");
                }
                else
                {
                    datatable = m_patUtil.GetFolderInfo("00");
                }

                foreach (DataRow row in datatable.Rows)
                {
                    InitTree(row, null);
                }
                treeList1.FocusedNodeChanged += new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
                ///刷新各首程的开始修改和结束修改时间
                SetRecordUpdateProcessList((int)m_CurrentInpatient.NoOfFirstPage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到配置信息  wyt 2012年8月27日
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        private void InitTree(DataRow row, TreeListNode node)
        {
            try
            {
                TreeListNode patNode = treeList1.AppendNode(new object[] { row["CNAME"] }, node);

                EmrModelContainer container = new EmrModelContainer(row);
                patNode.Tag = container;
                //add image todo

                //寻找子节点


                //如果当前节点是最底层文件夹，则新增其病历文件内容


                DataTable datatable = m_patUtil.GetFolderInfo(row["CCODE"].ToString());
                if (datatable.Rows.Count > 0)
                {

                    foreach (DataRow nextRow in datatable.Rows)
                    {
                        InitTree(nextRow, patNode);
                    }
                }
                else
                {

                    if (container.EmrContainerType != ContainerType.None)
                    {
                        //add by ywk  2012年11月22日11:22:34  病程折叠显示
                        DataTable leafs = m_patUtil.GetPatRecInfo(container.ContainerCatalog, m_CurrentInpatient.NoOfFirstPage.ToString());
                        {
                            foreach (DataRow leaf in leafs.Rows)
                            {
                                EmrModel leafmodel = new EmrModel(leaf);
                                TreeListNode leafNode = treeList1.AppendNode(new object[] { leafmodel.ModelName }, patNode);
                                leafNode.Tag = leafmodel;
                            }

                            //string createTimeYear = string.Empty;//用于记录病程的创建时间 年
                            //ArrayList YearGroup = new ArrayList();
                            //TreeListNode patNode1 = null;
                            //foreach (DataRow leaf in leafs.Rows)
                            //{
                            //    EmrModel leafmodel = new EmrModel(leaf);
                            //    if (leaf["SORTID"].ToString() == "AC")//如果是病程 
                            //    {
                            //        createTimeYear = leaf["CAPTIONDATETIME"].ToString().Substring(0, leaf["CAPTIONDATETIME"].ToString().IndexOf("-"));
                            //        if (!YearGroup.Contains(createTimeYear))
                            //        {
                            //            YearGroup.Add(createTimeYear);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        TreeListNode leafNode = treeList1.AppendNode(new object[] { leafmodel.ModelName }, patNode);
                            //        leafNode.Tag = leafmodel;
                            //    }
                            //}
                            //for (int i = 0; i < YearGroup.Count; i++)//先将年度分组显示为节点
                            //{
                            //    patNode1 = treeList1.AppendNode(new object[] { YearGroup[i].ToString() + "年度病程记录" }, patNode);
                            //    patNode1.StateImageIndex = 2;
                            //    patNode1.Tag = YearGroup[i].ToString();
                            //}
                            //for (int n = 0; n < patNode.Nodes.Count; n++)
                            //{

                            //    foreach (DataRow leaf in leafs.Rows)
                            //    {
                            //        EmrModel leafmodel = new EmrModel(leaf);
                            //        if (leaf["SORTID"].ToString() == "AC" && leaf["CAPTIONDATETIME"].ToString().Substring(0, leaf["CAPTIONDATETIME"].ToString().IndexOf("-"))
                            //            == patNode.Nodes[n].Tag.ToString())//如果是病程 
                            //        {

                            //            TreeListNode leafNode = treeList1.AppendNode(new object[] { leafmodel.ModelName }, patNode.Nodes[n]);
                            //            leafNode.Tag = leafmodel;
                            //        }
                            //    }
                            //}
                        }
                    }
                    //在护士记录下面添加记录表格文档
                    if (row["CCODE"].ToString() == "AP")
                    {
                        InCommonNoteBiz inCommonNoteBiz = new InCommonNoteBiz(m_app);
                        //该病人的所有生成单据
                        List<InCommonNoteEnmtity> InCommonNoteEnmtityList = inCommonNoteBiz.GetSimInCommonNote(CurrentInpatient.NoOfFirstPage.ToString());
                        foreach (InCommonNoteEnmtity inCommonNoteEnmtity in InCommonNoteEnmtityList)
                        {
                            string treeName = inCommonNoteEnmtity.InCommonNoteName + " " + DateUtil.getDateTime(inCommonNoteEnmtity.CreateDateTime, DateUtil.NORMAL_LONG);
                            TreeListNode leafNode = treeList1.AppendNode(new object[] { treeName }, patNode);
                            leafNode.Tag = inCommonNoteEnmtity;
                        }
                    }


                //加载会诊记录 Add By wwj 2011-11-01
                    else if (container.ContainerCatalog == ContainerCatalog.huizhenjilu)
                    {
                        List<EmrModelContainer> listContainer = m_RecordDal.GetConsultRecrod(row, m_CurrentInpatient.NoOfFirstPage.ToString());
                        foreach (EmrModelContainer emrModelContainer in listContainer)
                        {
                            TreeListNode leafNode = treeList1.AppendNode(new object[] { emrModelContainer.Name }, patNode);
                            leafNode.Tag = emrModelContainer;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void BindExtraInfo()
        {
            try
            {
                //加载图片库 add by zhouhui 配合拖拽工作
                treeListImage.BeginUnboundLoad();
                foreach (DataRow row in m_patUtil.ImageGallery.Rows)
                {
                    TreeListNode node = treeListImage.AppendNode(new object[] { row["名称"] }, null);
                    node.Tag = row;
                }
                treeListImage.EndUnboundLoad();
                //gridControlTool.MainView = gridViewMain;
                //navBarGroup_Image.ControlContainer.Controls.Add(gridControlTool);
                //gridControlTool.DataSource = m_patUtil.ImageGallery;
                //加载病案首页
                SetWaitDialogCaption("正在加载病案首页...");
                LoadIEMMainPage();
                HideWaitDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 右侧工具栏初始方法
        /// </summary>
        private void InitTool()
        {
            try
            {
                GetDataCollect();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region FocusedNodeChanged
        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                FocusedNodeChanged(e.Node);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        public void FocusedNodeChanged(TreeListNode node)
        {
            try
            {
                SetWaitDialogCaption("正在加载病历...");
                if ((node == null) || (node.Tag == null))
                {
                    return;
                }
                string oldCatelog = MyContainerCode;
                LoadModelFromTree(node);
                if (oldCatelog != MyContainerCode)
                {
                    //根据科室类别刷新科室小模板
                    InitPersonTree(false, MyContainerCode);
                }
                this.HideWaitDialog();
            }
            catch (Exception ex)
            {
                this.HideWaitDialog();
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 此页面加载右侧科室小模板给首次的科室小模板的值进行保存 add byywk 2012年6月11日 15:56:29
        /// </summary>
        public string MyContainerCode { get; set; }

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
                        ResetEditModeAction(container);
                        return;
                    }
                    else if (container.ContainerCatalog == "23")//23表示PACS影像
                    {
                        try
                        {
                            PACSOutSide.PacsAll(m_CurrentInpatient);
                        }
                        catch (Exception ex)
                        {
                            //xll 2013-2-21
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(3, ex);
                        }
                    }
                    else
                    {
                        //展现非病历容器内容
                        if (container.EmrContainerType == ContainerType.None)
                        {
                            if (m_TempContainerPages.ContainsKey(container))
                            {
                                //加载内容
                                xtraTabControl1.SelectedTabPage = m_TempContainerPages[container];
                                Control ctl = xtraTabControl1.SelectedTabPage.Controls.Count == 0 ? null : xtraTabControl1.SelectedTabPage.Controls[0];//zyx 2012-0-6
                                if (ctl != null && ctl.GetType().FullName.Equals("DrectSoft.Core.NurseDocument.MainNursingMeasure"))//如果是新的体温单界面
                                {
                                    //参数对象  
                                    object[] p = new object[2];
                                    //产生方法  
                                    MethodInfo m = ctl.GetType().GetMethod("simpleButtonRefresh_Click");
                                    //参数赋值。传入函数  
                                    p[0] = null;
                                    p[1] = null;
                                    //调用
                                    m.Invoke(ctl, p);
                                }
                            }
                            else
                            {
                                CreateCommonDocement(container);
                            }
                        }
                        ResetEditModeAction(container);
                    }

                    containercode = container.ContainerCatalog;
                }
                #endregion

                #region 模板类
                if (null != node && (!node.HasChildren) && (node.Tag is EmrModel))//如果是模板类，则说明是叶子节点
                {
                    m_CurrentModel = (EmrModel)node.Tag;
                    if (m_CurrentModel.ModelContent == null)
                    {
                        //为了提高初始化界面的速度，将病历内容的捞取延迟到打开病例的时候
                        m_CurrentModel = m_RecordDal.LoadModelInstance(m_CurrentModel.InstanceId);
                        node.Tag = m_CurrentModel;
                    }
                    //add by cyq 2013-02-05 修复同一用户多点登录，一个窗口修改节点时间，其它窗口节点未即时刷新的bug
                    if (node.GetValue("colName").ToString() != m_CurrentModel.ModelName)
                    {
                        node.SetValue("colName", m_CurrentModel.ModelName);
                    }

                    if (null != m_CurrentModel && m_TempModelPages.ContainsKey(m_CurrentModel)
                        && null != node.Tag && ((EmrModel)node.Tag).DailyEmrModel == false)//非病程
                    {
                        xtraTabControl1.SelectedTabPage = m_TempModelPages[m_CurrentModel];
                    }
                    else if (null != node && null != node.Tag && node.Tag is EmrModel && ((EmrModel)node.Tag).DailyEmrModel == true)//病程，由于病程的所有数据都保存在首页中，所以要打开首程
                    {
                        //（1）找到点击的病程对应的首程，并打开首次病程  该成打开当前病程的第一份病程
                        //EmrModel emrModelFirstDaily = node.ParentNode.FirstNode.Tag as EmrModel;
                        EmrModel emrModelFirstDaily = GetFirstDailyEmrModel(node.ParentNode.Nodes, node.Tag as EmrModel);
                        if (null == emrModelFirstDaily)
                        {
                            EmrModel thisModel = node.Tag as EmrModel;
                            foreach (TreeListNode tln in node.ParentNode.Nodes)
                            {
                                if (null != tln && tln.Tag is EmrModel)
                                {
                                    EmrModel emodel = tln.Tag as EmrModel;
                                    if (emodel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == thisModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") && emodel.InstanceId != thisModel.InstanceId)
                                    {
                                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("数据库中已存在时间为 " + thisModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " 的病历，您是否要刷新病程记录？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                                        {
                                            return;
                                        }
                                        ReFreshRecordData();
                                    }
                                }
                            }
                        }
                        if (null == emrModelFirstDaily)
                        {
                            return;
                        }
                        m_CurrentModel = emrModelFirstDaily;//保证点击病程后m_CurrentModel始终是第一个病程的EmrModel【即首程】

                        //（2）因为点击其他病程时首先打开的是首次病程，所以如果不是点击首次病程则不需要为首次病程设置编辑区域
                        bool isFocusFirstDailyNode = false;//点击的树节点是否是首次病程 true：首次病程 false：非首次病程
                        if (node.Tag != null && node.Tag is EmrModel && emrModelFirstDaily == node.Tag)
                        {
                            isFocusFirstDailyNode = true;
                        }
                        bool isNeedActiveEditArea = m_IsNeedActiveEditArea;
                        if (!isFocusFirstDailyNode)
                        {
                            m_IsNeedActiveEditArea = false;
                            m_IsLocateDaily = false;
                        }

                        //（3）加载并显示病历
                        if (null == m_CurrentModel || m_CurrentModel.ModelContent == null)
                        {
                            m_CurrentModel = m_RecordDal.LoadModelInstance(m_CurrentModel);
                        }

                        if (null != emrModelFirstDaily && m_TempModelPages.ContainsKey(emrModelFirstDaily))
                        {
                            xtraTabControl1.SelectedTabPage = m_TempModelPages[emrModelFirstDaily];
                        }
                        else
                        {
                            CreateNewDocument(m_CurrentModel);
                        }

                        //（4）将m_IsNeedActiveEditArea置回到（1）时的值
                        if (!isFocusFirstDailyNode)
                        {
                            m_IsNeedActiveEditArea = isNeedActiveEditArea;
                            m_IsLocateDaily = true;
                        }

                        //（5）设置回点击的病程病历
                        SetDailyEmrDoc(node);
                    }
                    else
                    {
                        //打开病历
                        CreateNewDocument(m_CurrentModel);
                    }

                    EmrModel model = (EmrModel)node.Tag;
                    CurrentForm.ResetEditModeState(model);
                    ResetEditModeAction(model);
                    if (model.DailyEmrModel)//针对病程的处理，对于选中的病程需要激活病程的编辑区域
                    {
                        ProcDailyEmrModel(node);
                    }
                    else
                    {
                        CheckIsAllowEdit(model);
                    }

                    if (node.ParentNode.Tag is EmrModelContainer)
                    {
                        containercode = ((EmrModelContainer)node.ParentNode.Tag).ContainerCatalog;
                    }

                    //this.InsertOperRecordLog(model, "0");//add by wyt
                }
                #endregion

                #region 护理记录表格 commonNote
                if (node.Tag is InCommonNoteEnmtity)
                {
                    InCommonNoteEnmtity inCommonNote = node.Tag as InCommonNoteEnmtity;
                    XtraTabPage tabpage = null;

                    foreach (XtraTabPage item in xtraTabControl1.TabPages)
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
                        if (DoctorEmployee.Kind == EmployeeKind.Nurse)
                        { canEdit = true; }
                        else
                        {
                            canEdit = false;
                        }
                        UCInCommonNote ucInCommonNote = new UCInCommonNote(m_app, commonNoteEntity, inCommonNote, canEdit);
                        tabpage.Controls.Add(ucInCommonNote);
                        ucInCommonNote.Dock = DockStyle.Fill;
                        xtraTabControl1.TabPages.Add(tabpage);

                    }
                    xtraTabControl1.SelectedTabPage = tabpage;

                    SetDailyEmrDoc(node);
                }
                #endregion

                MyContainerCode = containercode;

                //根据编辑器开关控制编辑器的编辑情况
                SetEditorNonEnable();

                //填充宏 当可以编辑病历时进行宏替换 Add By wwj 2012-05-14
                if (barButtonSave.Enabled == true) FillModelMacro(CurrentForm);

                //将焦点置到编辑器中
                if (CurrentForm != null)
                {
                    CurrentForm.GotFocusNotTriggerEvent();
                    //无修改则不提示保存 by cyq 2012-09-17 17:00 
                    CurrentForm.zyEditorControl1.EMRDoc.Modified = false;
                }
                ChangeHalf();

                #region 此处代表修改，先判断是否设置了留痕，如果设置了就直接可以开启了add by ywk 2012年12月25日14:15:10
                string isopenOwnerSign = DS_SqlService.GetConfigValueByKey("IsOpenOwnerSign") == "" ? "0" : DS_SqlService.GetConfigValueByKey("IsOpenOwnerSign");
                if (isopenOwnerSign == "1")
                {
                    //this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                    if (CurrentForm != null)
                    {
                        CurrentForm.GotFocusNotTriggerEvent();
                        this.CurrentForm.zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.UserName = m_app.User.Name; //这里传入用户名
                        this.CurrentForm.zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.Level = 1;         //这里传入用户级别
                        this.CurrentForm.zyEditorControl1.EMRDoc.Content.UserLevel = 1;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获得首次病程 
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        EmrModel GetFirstDailyEmrModel(TreeListNodes nodes)
        {
            try
            {
                foreach (TreeListNode node in nodes)
                {
                    if (node.Visible)
                    {
                        EmrModel emrModel = node.Tag as EmrModel;
                        if (emrModel.FirstDailyEmrModel)
                        {
                            return emrModel;
                        }
                    }
                }
                //if (nodes.Count > 0)
                //{
                //    EmrModel emrModel = nodes.FirstNode.Tag as EmrModel;
                //    if (emrModel.DailyEmrModel)
                //    {
                //        return emrModel;
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前科室中的第一份病程
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        EmrModel GetFirstDailyEmrModel(TreeListNodes nodes, EmrModel emrModelChild)
        {
            try
            {
                List<EmrModel> emrModelList = new List<EmrModel>();
                foreach (TreeListNode item in nodes)
                {
                    if (item.Tag is EmrModel && item.Visible == true)
                    {
                        emrModelList.Add(item.Tag as EmrModel);
                    }
                }
                if (emrModelList.Count == 0)
                {
                    emrModelChild.FirstDailyEmrModel = true;
                    return null;
                }
                DateTime dtChild = Convert.ToDateTime(emrModelChild.DisplayTime);
                if (emrModelList.Count == 1)
                {
                    if (emrModelList[0].DepartCode == emrModelChild.DepartCode)
                    {
                        return emrModelList[0];
                    }
                    else
                    {
                        emrModelChild.FirstDailyEmrModel = true;
                        return null;
                    }
                }
                DateTime dt0 = Convert.ToDateTime(emrModelList[0].DisplayTime);
                DateTime dtlength = Convert.ToDateTime(emrModelList[emrModelList.Count - 1].DisplayTime);


                for (int i = 0; i < emrModelList.Count - 1; i++)
                {
                    DateTime dtmin = Convert.ToDateTime(emrModelList[i].DisplayTime);
                    DateTime dtmax = Convert.ToDateTime(emrModelList[i + 1].DisplayTime);
                    if (dtChild <= dt0)  //新增时间小于第一个时间时  一般不太可能 
                    {
                        if (emrModelChild.DepartCode == emrModelList[0].DepartCode)
                        {
                            foreach (var item in emrModelList)
                            {
                                if (item.DepartCode == emrModelChild.DepartCode && item.FirstDailyEmrModel == true)
                                {
                                    return item;
                                }
                            }
                        }
                        else
                        {
                            emrModelChild.FirstDailyEmrModel = true;
                            return null;
                        }
                    }

                    if (dtChild >= dtmin && dtChild <= dtmax)  //新增时间在相邻2个时间之间
                    {
                        if (emrModelChild.DepartCode == emrModelList[i].DepartCode
                            && emrModelChild.DepartCode == emrModelList[i + 1].DepartCode)  //加入项目 与相邻2个项目科室相同
                        {
                            #region 已注释 by cyq 2013-01-22
                            /**
                            int firstindex = 0;  //同一科室的第一个
                            for (int j = i; j > 0; j--)
                            {//向上循环
                                if (emrModelList[j].DepartCode != emrModelList[j - 1].DepartCode && emrModelList[j].FirstDailyEmrModel == true)  //向上循环 获取相邻2个科室不同的序号
                                {
                                    firstindex = j;
                                    break;
                                }
                            }
                            for (int k = firstindex; k < emrModelList.Count; k++)
                            {//向下循环
                                if (emrModelList[k].DepartCode == emrModelChild.DepartCode && emrModelList[k].FirstDailyEmrModel == true)
                                {
                                    return emrModelList[k];
                                }
                            }
                            **/
                            #endregion

                            for (int j = i + 1; j >= 0; j--)
                            {//向上循环
                                if (emrModelList[j].DepartCode == emrModelChild.DepartCode
                                    && emrModelList[j].FirstDailyEmrModel == true)
                                {
                                    return emrModelList[j];
                                }
                            }
                            for (int j = i + 2; j < emrModelList.Count; j++)
                            {//向下循环
                                if (emrModelList[j].DepartCode == emrModelChild.DepartCode
                                    && emrModelList[j].FirstDailyEmrModel == true)
                                {
                                    return emrModelList[j];
                                }
                            }
                        }
                        else if (emrModelChild.DepartCode == emrModelList[i].DepartCode
                           && emrModelChild.DepartCode != emrModelList[i + 1].DepartCode)  //科室与上一个项目相同，与下一个项目不同
                        {
                            for (int j = i; j >= 0; j--)
                            {//向上循环
                                if (emrModelList[j].DepartCode == emrModelChild.DepartCode
                                    && emrModelList[j].FirstDailyEmrModel == true)
                                {
                                    return emrModelList[j];
                                }
                            }
                            for (int j = i + 1; j < emrModelList.Count; j++)
                            {//向下循环 add by cyq 2013-01-08
                                if (emrModelList[j].DepartCode == emrModelChild.DepartCode
                                    && emrModelList[j].FirstDailyEmrModel == true)
                                {
                                    return emrModelList[j];
                                }
                            }
                        }
                        else if (emrModelChild.DepartCode != emrModelList[i].DepartCode
                           && emrModelChild.DepartCode == emrModelList[i + 1].DepartCode)  //科室与上一个项目不同,与下一个项目相同
                        {
                            for (int j = i + 1; j < emrModelList.Count; j++)
                            {//向下循环
                                if (emrModelList[j].DepartCode == emrModelChild.DepartCode
                                    && emrModelList[j].FirstDailyEmrModel == true)
                                {
                                    return emrModelList[j];
                                }
                            }
                            for (int j = i; j >= 0; j--)
                            {//向上循环 add by cyq 2013-01-08
                                if (emrModelList[j].DepartCode == emrModelChild.DepartCode
                                    && emrModelList[j].FirstDailyEmrModel == true)
                                {
                                    return emrModelList[j];
                                }
                            }
                        }
                        else if (emrModelChild.DepartCode != emrModelList[i].DepartCode
                           && emrModelChild.DepartCode != emrModelList[i + 1].DepartCode
                           && emrModelList.Count() > i + 2 && emrModelChild.DepartCode == emrModelList[i + 2].DepartCode
                           && emrModelChild.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") == emrModelList[i + 2].DisplayTime.ToString("yyyy-MM-dd HH:mm:ss")
                            && emrModelList[i + 2].FirstDailyEmrModel)
                        {
                            return emrModelList[i + 2];
                        }
                        else
                        {
                            emrModelChild.FirstDailyEmrModel = true;
                            return null;
                        }
                    }

                    if (dtChild >= dtlength)  //时间大于最后一个
                    {
                        if (emrModelChild.DepartCode == emrModelList[emrModelList.Count - 1].DepartCode)
                        {
                            for (int j = emrModelList.Count - 1; j >= 0; j--)
                            {
                                if (emrModelChild.DepartCode == emrModelList[j].DepartCode
                                    && emrModelList[j].FirstDailyEmrModel == true)
                                {
                                    return emrModelList[j];
                                }
                            }
                        }
                        else
                        {
                            emrModelChild.FirstDailyEmrModel = true;
                            return null;
                        }

                    }
                }

                emrModelChild.FirstDailyEmrModel = true;
                return null;

                //List<EmrModel> emrModelListRight = emrModelList.FindAll(em => em.DepartCode == emrModelChild.DepartCode);  //查找大于gKe转科时间的记录
                //foreach (EmrModel itemright in emrModelListRight)
                //{
                //    if (itemright.FirstDailyEmrModel == true)
                //        return itemright;
                //}
                //emrModelChild.FirstDailyEmrModel = true;
                //return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 创建非病历类型界面
        /// 如首页、三测单之类的内容
        /// </summary>
        void CreateCommonDocement(EmrModelContainer container)
        {
            //读取容器里的包含第三方控件，使用反射使其实例化
            XtraTabPage newPad = null;
            try
            {
                if (string.IsNullOrEmpty(container.ModelEditor)) return;

                //IEMREditor editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString() });
                IEMREditor editor;
                if (container.Args != "")
                {
                    editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString(), container.Args });
                }
                else
                {
                    editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString() });
                }

                editor.ReadOnlyControl = !m_EditorEnableFlag;//传递是否可以编辑字段

                if (container.ContainerCatalog == ContainerCatalog.BingAnShouYe)//"病案首页"只需要加入现有的TabPage里
                {
                    newPad = xtraTabPageIEMMainPage;
                }
                else
                {
                    newPad = xtraTabControl1.TabPages.Add();
                }

                newPad.Text = editor.Title;
                editor.DesignUI.Dock = DockStyle.Fill;
                newPad.Controls.Add(editor.DesignUI);
                //加载控件至页面
                editor.Load(m_app);

                //newPad.Controls.Add();
                newPad.Tag = container;
                m_CurrentEmrEditor = editor;
                m_TempContainerPages.Add(container, newPad);
                xtraTabControl1.SelectedTabPage = newPad;

                if (m_CurrentTreeListNode != null)
                {
                    newPad.Text = m_CurrentTreeListNode.GetValue("colName").ToString();
                }

                this.HideWaitDialog();
            }
            catch (Exception ex)
            {
                xtraTabControl1.TabPages.Remove(newPad);
                if (ex is System.ArgumentNullException)
                { }
                else
                {
                    m_app.CustomMessageBox.MessageShow(ex.Message);
                }
            }
        }

        /// <summary>
        /// 创建非病历类型界面
        /// 如首页、三测单之类的内容
        /// 注：首页配置专用
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-11</date>
        /// </summary>
        //public  void CreateFirstPageDocement(EmrModelContainer container)
        //{
        //    XtraTabPage newPad = null;
        //    try
        //    {
        //        if (string.IsNullOrEmpty(container.ModelEditor))
        //        {
        //            return;
        //        }

        //        UCMainFormBase ucMainForm = new UCMainFormBase(m_app, m_CurrentInpatient.NoOfFirstPage.ToString());

        //        if (container.ContainerCatalog == ContainerCatalog.BingAnShouYe)//"病案首页"只需要加入现有的TabPage里
        //        {
        //            newPad = xtraTabPageIEMMainPage;
        //        }

        //        newPad.Text = container.Name;
        //        ucMainForm.Dock = DockStyle.Fill;
        //        newPad.Controls.Add(ucMainForm);

        //        //加载控件至页面
        //        ucMainForm.LoadForm(m_app);

        //        newPad.Tag = container;
        //        //m_CurrentEmrEditor = ucMainForm;
        //        m_TempContainerPages.Add(container, newPad);
        //        xtraTabControl1.SelectedTabPage = newPad;

        //        if (m_CurrentTreeListNode != null)
        //        {
        //            newPad.Text = m_CurrentTreeListNode.GetValue("colName").ToString();
        //        }

        //        this.HideWaitDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //        xtraTabControl1.TabPages.Remove(newPad);
        //        if (ex is System.ArgumentNullException)
        //        { }
        //        else
        //        {
        //            m_app.CustomMessageBox.MessageShow(ex.Message);
        //        }
        //    }
        //}

        /// <summary>
        /// 设置除病程外的病历可编辑情况
        /// edit by Yanqiao.Cai 2012-12-10
        /// 1、add try ... catch
        /// 2、设置可编辑状态
        /// </summary>
        /// <param name="model"></param>
        private void CheckIsAllowEdit(EmrModel model)
        {
            try
            {
                if (DoctorEmployee.Grade.Trim() != "")
                {
                    DoctorGrade grad = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);
                    if (model.State != ExamineState.NotSubmit && grad == DoctorGrade.Resident)//住院医且已提交
                    {
                        SetDocmentReadOnlyMode();
                    }
                }
                //病案管理 add by cyq 2012-12-10
                if (floaderState == FloderState.None || floaderState == FloderState.FirstPage)
                {
                    SetDocmentReadOnlyMode();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 对于选中的病程需要激活病程的编辑区域
        /// </summary>
        /// <param name="node"></param>
        private void ProcDailyEmrModel(TreeListNode node)
        {
            try
            {
                //SetDailyEmrAllowDelete(node);

                //当需要定位时，定位选中的病程整个病程记录中的位置
                if (m_IsLocateDaily)
                {
                    //定位病程
                    CurrentForm.LocateDailyEmrMode(node);
                }

                //设置病程对应的编辑区域
                SetDailyEmrEditArea(node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置病程病历 选中节点NotTriggerEvent 更改TabPage名称
        /// </summary>
        private void SetDailyEmrDoc(TreeListNode node)
        {
            try
            {
                //选中节点
                FocusNodeNotTriggerEvent(node);

                //更改TabPage名称
                ChangePageName(node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region SelectedPageChanged
        /// <summary>
        /// 通过SelectedPageChanged事件得到最新的m_CurrentModel, m_CurrentEmrEditor, m_CurrentModelContainer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            try
            {
                SelectedPageChanged(e.Page);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        public void SelectedPageChanged(XtraTabPage page)
        {
            try
            {
                if (page == null) return;

                //找到 当前病历EmrModel 或 非病历类型的界面IEmrEditor
                GetCurrentEmrModel(ref m_CurrentModel, ref m_CurrentEmrEditor, page);
                m_CurrentModelContainer = null;
                if (m_CurrentModel != null || m_CurrentEmrEditor != null)
                {
                    //通过非病历类型的界面IEmrEditor 找到 EmrModelContainer
                    GetEmrEditorContainer(m_CurrentEmrEditor);

                    //得到当前病历对应的树节点 m_CurrentTreeListNode
                    GetCurrentTreeListNode(null, m_CurrentModel, m_CurrentModelContainer);

                    ResetEditModeAction(m_CurrentModel);

                    //选中树节点 
                    SetFocusNode();
                }
                if (m_CurrentModelContainer != null)
                {
                    ResetEditModeAction(m_CurrentModelContainer);
                }
                if (m_CurrentModel == null && m_CurrentEmrEditor == null && m_CurrentModelContainer == null)
                {
                    if (treeList1.Nodes.Count > 0)
                    {
                        treeList1.FocusedNode = treeList1.Nodes[0];
                    }
                }
                //刷新科室小模板 edit by cyq (暂时不刷新)
                //TreeListNode focusedNode = treeList1.FocusedNode;
                //if (focusedNode.Tag is EmrModelContainer)
                //{
                //    InitPersonTree(false, ((EmrModelContainer)focusedNode.Tag).ContainerCatalog);
                //}
                //else if ((!focusedNode.HasChildren) && (focusedNode.Tag is EmrModel))//如果是模板类，则说明是叶子节点
                //{
                //    InitPersonTree(false, ((EmrModelContainer)focusedNode.ParentNode.Tag).ContainerCatalog);
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 得到处于激活状态的病历
        /// </summary>
        private void GetCurrentEmrModel(ref EmrModel emrModel,
            ref IEMREditor emrEditor,/*非病历类型的界面 如首页、三测单之类的内容*/
            XtraTabPage page)
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
                throw new Exception(ex.Message);
            }
        }

        private void GetEmrEditorContainer(IEMREditor emrEditor)
        {
            try
            {
                if (emrEditor == null) { return; }
                if (null == m_TempContainerPages || m_TempContainerPages.Count() == 0)
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

        private void GetCurrentTreeListNode(TreeListNodes nodes, EmrModel emrModel, EmrModelContainer emrModelContainer)
        {
            try
            {
                if (nodes == null)
                {
                    nodes = treeList1.Nodes;
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
        /// 设置病历对应的树节点【不触发FocusNodeChanged事件】
        /// </summary>
        private void SetFocusNode()
        {
            try
            {
                if (m_CurrentModel != null && m_CurrentModel.DailyEmrModel)//病程
                {
                    foreach (TreeListNode node in m_CurrentTreeListNode.ParentNode.Nodes)
                    {
                        //根据TabPage的名称来判断
                        //if (node.GetValue("colName").ToString() == m_TempModelPages[m_CurrentModel].Tooltip)
                        if (node.GetValue("colName").ToString() == m_TempModelPages[m_CurrentModel].Text)
                        {
                            //根据TabPage的名称来选中当前病程在整个病程TreeList中的位置
                            FocusNodeNotTriggerEvent(node);
                            ResetEditModeAction(node.Tag as EmrModel);

                            if (m_IsNeedActiveEditArea)
                            {
                                SetDailyEmrEditArea(node);
                            }
                            else
                            {
                                SetDocmentReadOnlyMode();
                            }
                        }
                    }
                }
                else
                {
                    FocusNodeNotTriggerEvent(m_CurrentTreeListNode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置病程编辑区【注意：在保存按钮可用的情况下才设置编辑区域】
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isCheck">是否需要判断</param>
        private void SetDailyEmrEditArea(TreeListNode node)
        {
            try
            {
                if (!m_IsNeedActiveEditArea) return;

                SetDocmentReadOnlyMode();
                CurrentForm.zyEditorControl1.ActiveEditArea = null;

                if (barButtonSave.Enabled)//当保存按钮可用时，表示此病历可以编辑，这时可以设置病历的编辑区域，否则不需要设置病历的编辑区域
                {
                    CurrentForm.SetDailyEmrEditArea(node);
                }
                else if (barButton_Audit.Enabled)//当审核按钮可用时表示当前病历正在审核，所以可以进行编辑
                {
                    CurrentForm.SetDailyEmrEditArea(node);
                }
                CurrentForm.zyEditorControl1.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Close TabPage
        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            try
            {
                XtraTabControlCloseButton();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(ex.Message);
            }
        }

        private void XtraTabControlCloseButton()
        {
            try
            {
                //todo 校验关闭的窗口是否需要保存
                if (xtraTabControl1.SelectedTabPage.Tag is InCommonNoteEnmtity)
                {
                    xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
                }
                else
                {
                    if (xtraTabControl1.SelectedTabPage.Tag is EmrModelContainer)
                    {
                        //todo 检验是否需要保存
                        m_TempContainerPages.Remove((EmrModelContainer)xtraTabControl1.SelectedTabPage.Tag);
                        xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
                    }
                    else if (CheckEditorIfSaved(m_CurrentModel)) //(null != m_CurrentModel && (m_CurrentModel.InstanceId == -1 || (m_CurrentModel.DailyEmrModel && modelList.Any(p => p.InstanceId == -1)) || CurrentForm.zyEditorControl1.EMRDoc.Modified))
                    {
                        //把病历的修改保存到临时变量中 EmrModel //edit by cyq 2013-01-07 null判断
                        CurrentForm.zyEditorControl1.EMRDoc.ToXMLDocument(null == m_CurrentModel ? new XmlDocument() : m_CurrentModel.ModelContent);

                        string fileName = m_CurrentModel.ModelName;
                        if (m_CurrentModel.DailyEmrModel)
                        {
                            fileName = "病程记录";
                        }

                        if (MyMessageBox.Show(fileName + " 已经修改，您是否要保存？", "提示信息", MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            SaveDocment(m_CurrentModel);
                            //xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
                            //m_TempModelPages.Remove(m_CurrentModel);
                            DeleteTablePage(m_CurrentModel);
                        }
                        else
                        {
                            //xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
                            //m_TempModelPages.Remove(m_CurrentModel);


                            //add by cyq 2013-02-02 修复关闭病程选项卡不保存时，左侧节点和选项卡中内容依然存在的bug
                            EmrModel toDeleteModel = m_CurrentModel;
                            if (toDeleteModel.ModelCatalog == "AC")
                            {//病程(删除所有新增病程节点)
                                DeleteNodesNotSaved(toDeleteModel);///1、删除未保存的新增节点
                                m_CurrentModel.ModelContent = null; ///2、将首程绑定内容清空(切换节点时会去数据库中读取)
                            }
                            else if (toDeleteModel.InstanceId == -1)
                            {//非病程(删除当前新增节点)
                                DeleteTheNodeNotSaved(toDeleteModel, treeList1.Nodes);
                            }
                            DeleteTablePage(toDeleteModel);
                        }
                    }
                    else
                    {
                        //xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
                        //m_TempModelPages.Remove(m_CurrentModel);
                        DeleteTablePage(m_CurrentModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查该病历/非病历是否已经保存
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-19</date>
        /// <returns></returns>
        private bool CheckEditorIfSaved(EmrModel emrModel)
        {
            try
            {
                List<EmrModel> modelList = new List<EmrModel>();
                if (null != emrModel && emrModel.DailyEmrModel)
                {//如果是病程，获取(编辑器上)该首程下所有病历节点
                    if (emrModel.FirstDailyEmrModel)
                    {
                        modelList = GetAllRecordModels(emrModel);
                    }
                    else
                    {
                        modelList = GetAllRecordModelNodes();
                    }
                }
                bool boo = null != emrModel && (emrModel.InstanceId == -1 || (emrModel.DailyEmrModel && modelList.Any(p => p.InstanceId == -1)) || CurrentForm.zyEditorControl1.EMRDoc.Modified);

                return boo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除左侧树病程记录节点中所有的新增节点
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-17</date>
        /// <param name="model"></param>
        private void DeleteNodesNotSaved(EmrModel model)
        {
            try
            {
                if (null == model)
                {
                    return;
                }
                TreeListNode bcNode = GetNodeByColName(treeList1.Nodes, "病程记录");
                if (null == bcNode || null == bcNode.Nodes || bcNode.Nodes.Count == 0)
                {
                    return;
                }

                //获取该首程model内容中的时间集合(解析xml)
                if (model.FirstDailyEmrModel)
                {
                    List<string> editorTimeList = DS_BaseService.GetEditEmrContentTimes(model.ModelContent);
                    if (null == editorTimeList || editorTimeList.Count() == 0)
                    {
                        return;
                    }
                    for (int i = bcNode.Nodes.Count - 1; i >= 0; i--)
                    {
                        if (null != bcNode.Nodes[i].Tag && bcNode.Nodes[i].Tag is EmrModel)
                        {
                            EmrModel modelevey = (EmrModel)bcNode.Nodes[i].Tag;
                            if (modelevey.InstanceId == -1 && editorTimeList.Contains(modelevey.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss")))
                            {
                                bcNode.Nodes.Remove(bcNode.Nodes[i]);
                            }
                        }
                    }
                }
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
                    if (node.GetValue("colName").ToString() == model.ModelName)
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
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据节点名称获取节点
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-02</date>
        /// <param name="nodes">节点集</param>
        /// <param name="colName">节点名称</param>
        /// <returns></returns>
        private TreeListNode GetNodeByColName(TreeListNodes nodes, string colName)
        {
            try
            {
                if (null == nodes || nodes.Count == 0 || string.IsNullOrEmpty(colName))
                {
                    return null;
                }
                foreach (TreeListNode node in nodes)
                {
                    if (node.GetValue("colName").ToString() == colName)
                    {
                        return node;
                    }
                    if (null != node && null != node.Nodes)
                    {
                        TreeListNode childNode = GetNodeByColName(node.Nodes, colName);
                        if (null != childNode && childNode.GetValue("colName").ToString() == colName)
                        {
                            return childNode;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 设置编辑器 DocumentModel

        /// <summary>
        /// 设置当前编辑器为只读模式
        /// </summary>
        private void SetDocmentReadOnlyMode()
        {
            try
            {
                if (CurrentForm == null) return;

                this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Read;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置当前编辑器为可编辑模式
        /// </summary>
        private void SetDomentEditMode()
        {
            try
            {
                if (CurrentForm == null) return;

                this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置当前编辑器为设计模式
        /// </summary>
        private void SetDocmentDesginMode()
        {
            try
            {
                if (CurrentForm == null) return;

                this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Design;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 不触发FocusedNodeChanged事件的方法 focusNode、deleteNode

        /// <summary>
        /// 设置获得焦点的树节点，不触发事件
        /// </summary>
        /// <param name="node"></param>
        private void FocusNodeNotTriggerEvent(TreeListNode node)
        {
            try
            {
                treeList1.FocusedNodeChanged -= new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
                node.Visible = true;
                treeList1.FocusedNode = node;
                treeList1.FocusedNodeChanged += new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                treeList1.FocusedNodeChanged -= new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
                treeList1.DeleteNode(node);
                treeList1.FocusedNodeChanged += new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 更改TabPage名称
        /// </summary>
        /// <param name="node"></param>
        private void ChangePageName(TreeListNode node)
        {
            try
            {
                XtraTabPage page = xtraTabControl1.SelectedTabPage;
                if (node.GetValue("colName") != null)
                {
                    page.Text = GetTabPageName(node.GetValue("colName").ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 病历文件增删改

        private void DeleteFromPad()
        {
            try
            {
                if (CurrentForm.zyEditorControl1 == null)
                {
                    MessageBox.Show("必须在编辑模式下才允许删除");
                    return;
                }

                if (CurrentForm.zyEditorControl1.ActiveEditArea != null)
                {
                    ZYTextElement topEle = CurrentForm.zyEditorControl1.ActiveEditArea.TopElement;
                    ZYTextElement endEle = CurrentForm.zyEditorControl1.ActiveEditArea.EndElement;

                    int start = CurrentForm.zyEditorControl1.EMRDoc.Content.IndexOf(topEle);
                    int end = 0;
                    if (CurrentForm.zyEditorControl1.ActiveEditArea.EndElement != null)
                    {
                        end = CurrentForm.zyEditorControl1.EMRDoc.Content.IndexOf(endEle);
                    }
                    else
                    {
                        end = CurrentForm.zyEditorControl1.EMRDoc.Content.Elements.Count - 1;
                    }
                    CurrentForm.zyEditorControl1.EMRDoc.Content.SetSelection(start, end - start + 1);
                    CurrentForm.zyEditorControl1.EMRDoc.Content.DeleteSeleciton(true);

                    CurrentForm.zyEditorControl1.ActiveEditArea = null;
                    CurrentForm.zyEditorControl1.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除节点后，如果焦点位于病程节点上，则病程中需要设置编辑区
        /// </summary>
        private void SetDailyEmrEditArea()
        {
            try
            {
                if (!m_IsNeedActiveEditArea) return;

                TreeListNode node = treeList1.FocusedNode;
                if (node != null)
                {
                    EmrModel model = node.Tag as EmrModel;
                    if (model != null)
                    {
                        ResetEditModeAction(model);
                        if (model.DailyEmrModel)
                        {
                            ProcDailyEmrModel(node);
                            ChangePageName(node);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                //treeList1.DeleteNode(m_CurrentTreeListNode);
                //DeleteNodeNotTriggerEvent(m_CurrentTreeListNode);
                DeleteTablePage(emrModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region
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
                    nodes = treeList1.Nodes;
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
        #endregion

        private void DeleteTablePage(EmrModel emrModel)
        {
            try
            {
                //edit by cyq 2013-01-24 修复null未判断
                //if (!m_TempModelPages.ContainsKey(emrModel))
                if (null == emrModel || null == m_TempModelPages || m_TempModelPages.Count == 0 || !m_TempModelPages.ContainsKey(emrModel))
                {
                    return;
                }
                XtraTabPage page = m_TempModelPages[emrModel];
                if (page != null)
                {
                    m_TempModelPages.Remove(emrModel);
                    xtraTabControl1.TabPages.Remove(page);
                    //触发PageSelectedChanged事件
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Print 相关
        private void JumpPrint()
        {
            try
            {
                if (CurrentForm == null) return;
                CurrentForm.zyEditorControl1.EMRDoc.EnableJumpPrint = !CurrentForm.zyEditorControl1.EMRDoc.EnableJumpPrint;
                CurrentForm.zyEditorControl1.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void PrientViewDocment()
        {
            try
            {
                if (CurrentForm == null) return;
                CurrentForm.zyEditorControl1.EMRDoc._PrintPreview();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void PrintDocment()
        {
            try
            {
                if (CurrentForm == null) return;
                CurrentForm.zyEditorControl1.EMRDoc._Print();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        //todo

        #region 病历数据读取操作

        private void GetFolderFiles(TreeListNode parentNode)
        {

            //安装文件夹读取病历内容
            //DataTable sourceTable = m_patUtil.GetFolderInfo();


        }



        #endregion

        #region 表格相关操作

        /// <summary>
        /// 插入表格
        /// add by ywk 
        /// </summary>
        private void InsertTable()
        {

            try
            {
                TableInsert tableInsertForm = new TableInsert();
                if (tableInsertForm.ShowDialog() == DialogResult.OK)
                {
                    //病程记录中插入表格 add by ywk  2012年12月6日10:07:12 反正可以修改，编辑器可以设置为编辑
                    //CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                    CurrentForm.zyEditorControl1.EMRDoc.TableInsert(tableInsertForm.Header,
                        tableInsertForm.Rows, tableInsertForm.Columns,
                        tableInsertForm.ColumnWidth);
                    //CurrentForm.zyEditorControl1.EMRDoc.TableInsertForNurseTable();
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
                return;
            }

        }

        private void InsertTableForNurseDocument()
        {
            try
            {
                if (CurrentForm != null && CurrentForm.zyEditorControl1 != null)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.TableInsertForNurseTable();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void InsertTableRow()
        {
            try
            {
                RowsInsert rowsInsertForm = new RowsInsert();
                if (rowsInsertForm.ShowDialog() == DialogResult.OK)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.RowInsert(rowsInsertForm.RowNum, rowsInsertForm.IsBack);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void InsertTableColumn()
        {
            try
            {
                ColumnsInsert columnsInsertForm = new ColumnsInsert();
                if (columnsInsertForm.ShowDialog() == DialogResult.OK)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.ColumnInsert(columnsInsertForm.ColumnNum, columnsInsertForm.IsBack);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DeleteTable()
        {
            try
            {
                if (m_app.CustomMessageBox.MessageShow("确定要删除光标所在的表格吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.TableDelete();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DeletTableRow()
        {
            try
            {
                if (m_app.CustomMessageBox.MessageShow("确定要删除光标所在的行吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.TableDeleteRow();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DeletTableColumn()
        {
            try
            {
                if (m_app.CustomMessageBox.MessageShow("确定要删除光标所在的列吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.TableDeleteCol();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ChoiceTable()
        {
            try
            {
                CurrentForm.zyEditorControl1.EMRDoc.TableSelect();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ChoiceTableRow()
        {
            try
            {
                CurrentForm.zyEditorControl1.EMRDoc.TableSelectRow();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ChoiceTableColumn()
        {
            try
            {
                CurrentForm.zyEditorControl1.EMRDoc.TableSelectCol();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void MergeTableCell()
        {
            try
            {
                if (CurrentForm.zyEditorControl1.EMRDoc.TableIsCanMerge() == 1)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.TableMergeCell();
                }
                else if (CurrentForm.zyEditorControl1.EMRDoc.TableIsCanMerge() == 2)
                {
                    MessageBox.Show("此次操作须得在表格中选中单元格才能生效", "不能合并", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else if (CurrentForm.zyEditorControl1.EMRDoc.TableIsCanMerge() == 3)
                {
                    MessageBox.Show("您并没有选中任何单元格", "不能合并", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else if (CurrentForm.zyEditorControl1.EMRDoc.TableIsCanMerge() == 4)
                {
                    MessageBox.Show("您只选中了一个单元格,无法再次合并", "不能合并", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else if (CurrentForm.zyEditorControl1.EMRDoc.TableIsCanMerge() == 5)
                {
                    MessageBox.Show("请检查您选择的单元格是否呈正规矩形", "不能合并", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ApartTableCell()
        {
            try
            {
                Dictionary<string, int> splitDic = CurrentForm.zyEditorControl1.EMRDoc.TableBeforeSplitCell();
                ApartCell apartCellForm = new ApartCell(splitDic["row"], splitDic["col"]);

                if (apartCellForm.ShowDialog() == DialogResult.OK)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.TableSplitCell(apartCellForm.intRow, apartCellForm.intColumn);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetTableProperty()
        {
            try
            {
                TPTextTable table = CurrentForm.zyEditorControl1.EMRDoc.GetCurrentTable();
                TPTextCell cell = CurrentForm.zyEditorControl1.EMRDoc.GetCurrentCell();
                if (table != null && cell != null)
                {
                    TableProperty tablePropertyForm = new TableProperty(table, cell);
                    if (tablePropertyForm.ShowDialog() == DialogResult.OK)
                    {

                        CurrentForm.zyEditorControl1.EMRDoc.ContentChanged();
                        CurrentForm.zyEditorControl1.EMRDoc.Refresh();
                        //CurrentForm.zyEditorControl1.EMRDoc.SetTableProperty(tablePropertyForm.Table);
                    }
                }
                else
                {
                    MessageBox.Show("光标必须在表格中才能更改表格属性", "不能打开表格属性", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetTableCellProperty()
        {
            //TPTextCell cell = this.pnlText.EMRDoc.GetCurrentCell();
            //if (cell != null)
            //{
            //    TableCellSetting cellSetting = new TableCellSetting(cell);
            //    cellSetting.StartPosition = FormStartPosition.CenterScreen;
            //    cellSetting.ShowDialog();
            //}
            //else
            //{
            //    MessageBox.Show("光标必须在表格中才能更改单元格的属性", "不能打开单元格属性", MyMessageBoxButtons.Ok, MessageBoxIcon.Warning);
            //}
        }

        #endregion

        #region  copy 相关
        private void Copy()
        {
            try
            {
                CurrentForm.zyEditorControl1.EMRDoc._Copy();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Paste()
        {
            try
            {
                CurrentForm.zyEditorControl1.EMRDoc._Paste();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Undo()
        {
            try
            {
                CurrentForm.zyEditorControl1.EMRDoc._Undo();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void Redo()
        {
            try
            {
                CurrentForm.zyEditorControl1.EMRDoc._Redo();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void BoldChar(bool reslut)
        {
            try
            {
                CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontBold(reslut);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region sub and sup

        void ReSetSupSub()
        {
            try
            {
                CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSup(false);
                CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSub(false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 上标
        /// </summary>
        private void SupChar(bool reslut)
        {
            try
            {
                ReSetSupSub();
                CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSup(reslut);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 下标
        /// </summary>
        private void SubChar(bool reslut)
        {
            try
            {
                ReSetSupSub();
                CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSub(reslut);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region public methods of WaitDialog
        public void SetWaitDialogCaption(string caption)
        {
            try
            {
                if (m_WaitDialog != null)
                {
                    if (!m_WaitDialog.Visible)
                    {
                        m_WaitDialog.Visible = true;
                    }
                    m_WaitDialog.Caption = caption;
                }
                else
                {
                    m_WaitDialog = new WaitDialogForm(caption, "请稍候...");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HideWaitDialog()
        {
            try
            {
                if (m_WaitDialog != null)
                    m_WaitDialog.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 创建病程相关界面
        /// </summary>
        /// <param name="model"></param>
        void CreateNewDocument(EmrModel model)
        {
            try
            {
                //add by cyq 2013-02-02 null 判断
                if (null == model)
                {
                    return;
                }
                //病程相关文件
                if (model.DailyEmrModel)
                {
                    //判断是否需要添加
                    if (!m_TempModelPages.ContainsKey(model))
                    {
                        //判断当前模型是否需要在新建tabpage页
                        XtraTabPage newPad = null;
                        //foreach (EmrModel md in m_TempModelPages.Keys)
                        //{
                        //    if (md.ModelCatalog.Equals(model.ModelCatalog))
                        //    {
                        //        newPad = m_TempModelPages[md];
                        //        break;
                        //    }

                        //}
                        //如果未实现则新建一个tabpage
                        if (newPad == null)
                        {
                            PadForm pad = new PadForm(m_patUtil, m_RecordDal);
                            pad.Dock = DockStyle.Fill;
                            InsertEmrModelContent(pad, model);
                            //由于宏元素中的数据可能与病人实际的信息有出入，所在加载病历的时候需要填充宏数据
                            //FillModelMacro(pad);  //Modified By wwj 2012-05-14
                            newPad = xtraTabControl1.TabPages.Add();
                            newPad.Text = GetTabPageName(model.ModelName); //model.ModelName;//显示简短的文件名
                            //newPad.Tooltip = model.ModelName;//文件全名
                            newPad.Controls.Add(pad);
                            newPad.Tag = model;
                            m_CurrentModel = model;
                            m_TempModelPages.Add(model, newPad);
                            xtraTabControl1.SelectedTabPage = newPad;//触发SelectedPage事件
                        }
                    }
                }
                else
                {

                    if (!m_TempModelPages.ContainsKey(model))
                    {

                        PadForm pad = new PadForm(m_patUtil, m_RecordDal);
                        pad.Dock = DockStyle.Fill;
                        pad.LoadDocment(model);
                        //由于宏元素中的数据可能与病人实际的信息有出入，所在加载病历的时候需要填充宏数据
                        //FillModelMacro(pad);  //Modified By wwj 2012-05-14
                        XtraTabPage newPad = xtraTabControl1.TabPages.Add();
                        newPad.Text = GetTabPageName(model.ModelName); //显示简短的文件名
                        //newPad.Tooltip = model.ModelName;//文件全名
                        newPad.Controls.Add(pad);
                        newPad.Tag = model;
                        m_CurrentModel = model;
                        m_TempModelPages.Add(model, newPad);
                        xtraTabControl1.SelectedTabPage = newPad;//触发SelectedPage事件
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 针对右侧工具栏的Event
        private void navBarControlToolBox_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            try
            {
                //Add by wwj 2012-06-25 按照老板的要求，增加NavBarGroup的提示
                ShowTipForNavBarGroup(e.Group);

                // modified by zhouhui 2011-08-15 配合修改右侧工具拖拽实现
                gridControlTool.MainView = gridViewMain;
                if (e.Group == navBarGroup_Image)//图片库
                {
                    if (treeListImage.Nodes.Count < 1)
                    {
                        treeListImage.BeginUnboundLoad();
                        foreach (DataRow row in m_patUtil.ImageGallery.Rows)
                        {
                            TreeListNode node = treeListImage.AppendNode(new object[] { row["名称"] }, null);
                            node.Tag = row;
                        }
                        treeListImage.EndUnboundLoad();
                    }

                    //navBarGroup_Image.ControlContainer.Controls.Add(gridControlTool);
                    //gridControlTool.DataSource = m_patUtil.ImageGallery;
                }

                else if (e.Group == navBarGroup_Zd)//字典库
                {
                    navBarGroup_Zd.ControlContainer.Controls.Add(gridControlTool);
                    gridControlTool.DataSource = m_patUtil.ZDItemData;

                }
                else if (e.Group == navBarGroup_Cy)//常用词
                {
                    //navBarGroup_Cy.ControlContainer.Controls.Add(gridControlTool);
                    //gridControlTool.DataSource = m_patUtil.Macro;
                    if (treeListCy.Nodes.Count < 1)
                    {
                        treeListCy.BeginUnboundLoad();
                        foreach (DataRow row in m_patUtil.Macro.Rows)
                        {
                            TreeListNode node = treeListCy.AppendNode(new object[] { row["名称"] }, null);
                            node.Tag = row;
                        }
                        treeListCy.EndUnboundLoad();
                    }

                }
                else if (e.Group == navBarGroup_TZ)//体征记录
                {
                    //navBarGroup_TZ.ControlContainer.Controls.Add(gridControlTool);
                    //gridControlTool.DataSource = m_patUtil.TZItemTemplate;

                    if (treeListTZ.Nodes.Count < 1)
                    {
                        treeListTZ.BeginUnboundLoad();
                        foreach (DataRow row in m_patUtil.TZItemTemplate.Rows)
                        {
                            TreeListNode node = treeListTZ.AppendNode(new object[] { row["名称"] }, null);
                            node.Tag = row;
                        }
                        treeListTZ.EndUnboundLoad();
                    }
                }
                else if (e.Group == navBarGroup_Symb)//特殊字符
                {
                    //navBarGroup_Symb.ControlContainer.Controls.Add(gridControlTool);
                    //gridControlTool.DataSource = m_patUtil.Symbols;

                    //if (treeListSymb.Nodes.Count < 1)
                    //{
                    //    treeListSymb.BeginUnboundLoad();
                    //    foreach (DataRow row in m_patUtil.Symbols.Rows)
                    //    {
                    //        TreeListNode node = treeListSymb.AppendNode(new object[] { row["名称"] }, null);
                    //        node.Tag = row;
                    //    }
                    //    treeListSymb.EndUnboundLoad();
                    //}

                    BindSymbol(m_patUtil.Symbols);
                }
                else if (e.Group == navBarGroup_ZZ)//症状
                {

                    if (treeListZZ.Nodes.Count < 1)
                    {
                        treeListZZ.BeginUnboundLoad();
                        foreach (DataRow row in m_patUtil.ZZItemData.Rows)
                        {
                            TreeListNode node = treeListZZ.AppendNode(new object[] { row["名称"] }, null);
                            node.Tag = row;
                        }
                        treeListZZ.EndUnboundLoad();
                    }
                    //navBarGroup_ZZ.ControlContainer.Controls.Add(gridControlTool);
                    //gridControlTool.DataSource = m_patUtil.ZZItemData;
                }

                //else if (e.Group == navBarGroup_DBL)//病历节点信息
                //{
                //    InitBLMessage();
                //}
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void BindSymbol(DataTable dataTableSymbol)
        {
            try
            {
                System.Drawing.Font font = new System.Drawing.Font("宋体", 10, FontStyle.Regular);
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

                imageListBoxControlSymbol.Items.Clear();

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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void gridViewMain_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentForm == null) return;
                if (!barButtonSave.Enabled) return;

                //GridView gv = sender as 

                if (gridViewMain.FocusedRowHandle < 0) return;

                DataRow row = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
                if (row == null) return;

                DataTable table = (DataTable)gridControlTool.DataSource;
                if (table.TableName.Equals("image") && CanEdit())
                {
                    //读取图片内容
                    byte[] img = m_patUtil.GetImage(row["ID"].ToString());

                    CurrentForm.zyEditorControl1.EMRDoc._InsertImage(img);

                }
                else if (table.TableName.Equals("zd") && CanEdit())
                {
                    DataTable sourceTable = m_patUtil.GetZDDetailInfo(row["SNO"].ToString().TrimEnd('\0'));
                    ChoiceForm choice = new ChoiceForm(m_app.SqlHelper, sourceTable);
                    if (choice.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        CurrentForm.zyEditorControl1.EMRDoc._InserString(choice.CommitRow["D_NAME"].ToString());
                    }

                }
                else if (table.TableName.Equals("Macro") && CanEdit())
                {
                    Macro mac = new Macro(row);

                    MacroUtil.FillMarcValue(m_CurrentInpatient.NoOfFirstPage.ToString(), mac, m_app.User.Id);
                    if (!string.IsNullOrEmpty(mac.MacroValue))
                        CurrentForm.zyEditorControl1.EMRDoc._InserString(mac.MacroValue);
                }
                else if (table.TableName.Equals("tz") && CanEdit())
                {
                    byte[] tz = m_patUtil.GetTZDetailInfo(row["名称"].ToString());
                    CurrentForm.zyEditorControl1.EMRDoc.InsertTemplateFile(tz);
                }
                else if (table.TableName.Equals("symbols") && CanEdit())
                {
                    if (!row.IsNull("名称"))
                    {
                        CurrentForm.zyEditorControl1.EMRDoc._InserString(row["名称"].ToString());
                    }

                }
                else if (table.TableName.Equals("zz") && CanEdit())
                {
                    byte[] zz = m_patUtil.GetZZItemDetailInfo(row["名称"].ToString());
                    CurrentForm.zyEditorControl1.EMRDoc.InsertTemplateFile(zz);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void navBarControlClinical_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            try
            {
                //Add by wwj 2012-06-25 按照老板的要求，增加NavBarGroup的提示
                ShowTipForNavBarGroup(e.Group);

                ActiveGroup(e.Group);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void ActiveGroup(NavBarGroup activeGroup)
        {
            try
            {
                if (activeGroup == navBarGroup_Bl)//病历提取 todo: 2011-07-14
                {
                    gridControlCollect.DataSource = m_patUtil.GetDocCollectContent();
                }

                else if (activeGroup == navBarGroup_Jy)//检验
                {

                    //gridControlTool.MainView = gridView_YJ;
                    //navBarGroup_Jy.ControlContainer.Controls.Add(gridControlTool);
                    try
                    {
                        if (m_patUtil.LisReports != null)
                        {
                            //徐亮亮 2012-12-17 存在该列则显示
                            gcReleaseDate.Visible = false;
                            if (m_patUtil.LisReports.Columns.Contains("RELEASEDATE"))
                            {
                                gcReleaseDate.Visible = true;
                            }
                            gridControlLIS.DataSource = m_patUtil.LisReports;
                        }
                    }
                    catch (Exception ex)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(4, ex);
                    }
                }
                else if (activeGroup == navBarGroup_Jc)//检查
                {

                    //gridControlTool.MainView = gridView_YJ;
                    //navBarGroup_Jc.ControlContainer.Controls.Add(gridControlTool);
                    try
                    {
                        if (m_patUtil.PacsReports != null)
                        {
                            gridControlPacs.DataSource = m_patUtil.PacsReports;
                        }
                    }
                    catch (Exception ex)
                    {

                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(3, ex);
                    }
                }
                else if (activeGroup == navBarGroup_DoctorAdive)//医嘱
                {

                    navBarGroup_DoctorAdive.ControlContainer.Controls.Add(gridControlTool);
                    gridControlTool.MainView = gridView_Yz;
                }
                else if (activeGroup == navBarGroup_history)//历史病历信息
                {
                    InitTreeListHistory(Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void gridView_YJ_DragObjectDrop(object sender, DevExpress.XtraGrid.Views.Base.DragObjectDropEventArgs e)
        {

        }

        private void gridViewLIS_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int index = gridViewLIS.FocusedRowHandle;
                if (index < 0) return;
                DataRow masterRow = gridViewLIS.GetDataRow(index);
                //string repNo = masterRow["ReportNo"].ToString();
                //string repType = masterRow["ReportType"].ToString();
                //string catalog = masterRow["RePortCatalog"].ToString();

                DataTable detail = m_patUtil.GetReportDetail(masterRow);
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
                                    CurrentForm.zyEditorControl1.EMRDoc._InserString(detailForm.CommitValue);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 双击查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewRis_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string LISANDPACSTIP = GetConfigValueByKey("IsOpenLISandPACS") == "" ? "" : GetConfigValueByKey("IsOpenLISandPACS");
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
                                CurrentForm.zyEditorControl1.EMRDoc._InsertImage(imagedata);
                            }

                            string resultText = report.GetResultStringType();
                            if (resultText != "")
                            {
                                CurrentForm.zyEditorControl1.EMRDoc.Content.InsertString(resultText);

                                //换行
                                ZYTextElement ele = new ZYTextElement();
                                CurrentForm.zyEditorControl1.EMRDoc.Content.InsertParagraph(ele);
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
                                    CurrentForm.zyEditorControl1.EMRDoc._InserString(pacs.CommitValue.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

            //*****************************销售演示数据*************************************
            //PacsReport report = new PacsReport();
            //report.StartPosition = FormStartPosition.CenterScreen;
            //if (report.ShowDialog() == DialogResult.OK)
            //{
            //    if (CurrentForm != null)
            //    {
            //        foreach (Image img in report.GetResultImageType())
            //        {
            //            MemoryStream ms = new MemoryStream();
            //            byte[] imagedata = null;
            //            img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            //            imagedata = ms.GetBuffer();
            //            CurrentForm.zyEditorControl1.EMRDoc._InsertImage(imagedata);
            //        }

            //        string resultText = report.GetResultStringType();
            //        if (resultText != "")
            //        {
            //            CurrentForm.zyEditorControl1.EMRDoc.Content.InsertString(resultText);

            //            //换行
            //            ZYTextElement ele = new ZYTextElement();
            //            CurrentForm.zyEditorControl1.EMRDoc.Content.InsertParagraph(ele);
            //        }
            //    }
            //}
            //*****************************************************************************

            //GridHitInfo hitInfo = gridViewRis.CalcHitInfo(gridControlPacs.PointToClient(new Point(Cursor.Position.X, Cursor.Position.Y)));
            //if (hitInfo.RowHandle >= 0)
            //{
            //    DataRowView drv = gridViewRis.GetRow(hitInfo.RowHandle) as DataRowView;
            //    ReportPacs pacs = new ReportPacs(drv["检查项目"].ToString(), drv["检查时间"].ToString(), drv["检查状态"].ToString(),
            //        drv["描述"].ToString(), drv["诊断"].ToString(), drv["建议"].ToString(),
            //        drv["报告医生"].ToString(), drv["最终报告时间"].ToString());
            //    pacs.StartPosition = FormStartPosition.CenterScreen;
            //    if (pacs.ShowDialog() == DialogResult.OK)
            //    {
            //        if (!string.IsNullOrEmpty(pacs.CommitValue.ToString()))
            //        {
            //            if (CurrentForm != null)
            //            {
            //                CurrentForm.zyEditorControl1.EMRDoc._InserString(pacs.CommitValue.ToString());
            //            }
            //        }
            //    }
            //}
        }
        #endregion

        #region 设置TreeList图片

        private void treeList1_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            try
            {
                TreeListNode node = e.Node;

                if (e.Node.Tag is EmrModelContainer)
                {
                    if (e.Node.Expanded)//文件夹打开
                    {
                        //e.NodeImageIndex = 1; 
                        e.NodeImageIndex = 3;
                    }
                    else//文件夹未打开
                    {
                        //e.NodeImageIndex = 0; 
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
                else if (e.Node.Tag is EmrModel)
                {
                    EmrModel model = (EmrModel)e.Node.Tag;
                    if (model.State == ExamineState.NotSubmit)//未提交
                    {
                        //e.NodeImageIndex = 3; 
                        e.NodeImageIndex = 0;
                    }
                    else if (model.State == ExamineState.SubmitButNotExamine)//已提交，未审核
                    {
                        //e.NodeImageIndex = 4; 
                        e.NodeImageIndex = 1;
                    }
                    else if (model.State == ExamineState.FirstExamine)//主治已经审核
                    {
                        //e.NodeImageIndex = 6; 
                        e.NodeImageIndex = 14;
                    }
                    else if (model.State == ExamineState.SecondExamine)//主任已经审核
                    {
                        //e.NodeImageIndex = 5; 
                        e.NodeImageIndex = 13;
                    }
                    else
                    {
                        //e.NodeImageIndex = 4; 
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

        //设置右侧树图片
        private void treeListPerson_GetStateImage(object sender, GetStateImageEventArgs e)
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

        private void SetLockImage(DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            try
            {
                if (e.Node == null) return;

                if (floaderState == FloderState.Default)
                {
                    if (DoctorEmployee.Kind == EmployeeKind.Nurse)//当前登录人是护士
                    {
                        bool isNurseNode = false;
                        FindNurseDoc(ref isNurseNode, e.Node);
                        if (!isNurseNode)
                        {
                            SetLockFileAndFolderNode(e);
                        }
                    }
                    else if (CheckIsDoctor())//当前登录人是医生
                    {
                        bool isNurseNode = false;
                        FindNurseDoc(ref isNurseNode, e.Node);
                        if (isNurseNode)
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
                    bool isFirstPage = false;
                    FindMediRecordFirstPage(ref isFirstPage, e.Node);
                    if (!isFirstPage)
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
        /// 判断当前用户是否是医生
        /// </summary>
        /// <returns></returns>
        private bool CheckIsDoctor()
        {
            try
            {
                if (DoctorEmployee.Kind == EmployeeKind.Doctor || DoctorEmployee.Kind == EmployeeKind.Specialist || DoctorEmployee.Kind == EmployeeKind.Outdoctor)//当前登录人是医生
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
        /// 设置文件夹和文件锁
        /// </summary>
        /// <param name="e"></param>
        private void SetLockFileAndFolderNode(DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            try
            {
                if (e.Node == null) return;

                if (e.Node.Tag != null && e.Node.Tag is EmrModel)
                {
                    //e.NodeImageIndex = 4; 
                    e.NodeImageIndex = 12;//LockFile
                }
                else
                {
                    //e.NodeImageIndex = 2; 
                    e.NodeImageIndex = 12;//LockFile
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 供外部调用
        public void GetStateImage()
        {
            try
            {
                TreeListNode node = this.treeList1.FocusedNode;
                if (node.Tag is EmrModelContainer)
                {
                    if (node.Expanded)
                    {
                        //node.ImageIndex = 1; 
                        node.ImageIndex = 3;
                    }
                    else
                    {
                        //node.ImageIndex = 0; 
                        node.ImageIndex = 2;
                    }
                }
                else if (node.Tag is EmrModel)
                {
                    EmrModel model = (EmrModel)node.Tag;
                    if (model.State == ExamineState.NotSubmit)//未提交
                    {
                        //node.ImageIndex = 3; 
                        node.ImageIndex = 0;
                    }
                    else if (model.State == ExamineState.SubmitButNotExamine)//已提交，未审核
                    {
                        //node.ImageIndex = 4; 
                        node.ImageIndex = 1;
                    }
                    else if (model.State == ExamineState.FirstExamine)//主治已经审核
                    {
                        //node.ImageIndex = 6; 
                        node.ImageIndex = 14;
                    }
                    else if (model.State == ExamineState.SecondExamine)//主任已经审核
                    {
                        //node.ImageIndex = 5; 
                        node.ImageIndex = 13;
                    }
                    else
                    {
                        //node.ImageIndex = 4; 
                        node.ImageIndex = 1;
                    }
                }

                SetLockImage(node);
                treeList1.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetLockImage(TreeListNode node)
        {
            try
            {
                if (node == null) return;

                if (DoctorEmployee.Kind == EmployeeKind.Nurse)//当前登录人是护士
                {
                    bool isNurseNode = false;
                    FindNurseDoc(ref isNurseNode, node);
                    if (!isNurseNode)
                    {
                        SetLockFileAndFolderNode(node);
                    }
                }
                else if (CheckIsDoctor())//当前登录人是医生
                {
                    bool isNurseNode = false;
                    FindNurseDoc(ref isNurseNode, node);
                    if (isNurseNode)
                    {
                        SetLockFileAndFolderNode(node);
                    }
                }
                else//其他人
                {
                    SetLockFileAndFolderNode(node);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置被锁定的文件夹和文件的图片
        /// </summary>
        /// <param name="e"></param>
        private void SetLockFileAndFolderNode(TreeListNode node)
        {
            try
            {
                if (node == null) return;

                if (node.Tag != null && node.Tag is EmrModel)
                {
                    //node.ImageIndex = 4; 
                    node.ImageIndex = 12;//LockFile
                }
                else
                {
                    //node.ImageIndex = 2; 
                    node.ImageIndex = 12;//LockFile
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        private void FindNurseDoc(ref bool flag, TreeListNode node)
        {
            try
            {
                if (node == null) return;

                if (node.GetValue("colName").ToString() == "护士文档")
                {
                    flag = true;
                    return;
                }

                if (node.ParentNode != null)
                {
                    FindNurseDoc(ref flag, node.ParentNode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 左侧菜单病案首页节点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-06</date>
        /// <param name="node"></param>
        /// </summary>
        private void FindMediRecordFirstPage(ref bool flag, TreeListNode node)
        {
            try
            {
                if (node == null)
                {
                    return;
                }

                if (node.GetValue("colName").ToString() == "病案首页")
                {
                    flag = true;
                    return;
                }
                if (node.ParentNode != null)
                {
                    FindMediRecordFirstPage(ref flag, node.ParentNode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 点击 TreeList

        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
            //if ((treeList1.FocusedNode == null) || (treeList1.FocusedNode.Tag == null)) return;

            //LoadModelFromTree(treeList1.FocusedNode);
        }

        private void treeList1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                btnDelCommonNote.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                if (e.Button == System.Windows.Forms.MouseButtons.Right && m_RightMouseButtonFlag)//右键弹出菜单
                {
                    TreeListHitInfo hitInfo = treeList1.CalcHitInfo(e.Location);
                    if (hitInfo != null && hitInfo.Node != null)
                    {
                        treeList1.Focus();
                        treeList1.FocusedNode = hitInfo.Node;
                        TreeListNode node = hitInfo.Node;
                        if (node.Tag != null && node.Tag is EmrModelContainer)//文件夹
                        {
                            SetEmrModelContainerButtonVisiable(node);
                        }
                        else if (node.Tag != null && node.Tag is EmrModel)//病历
                        {
                            SetEmrModelButtonVisiable(node);
                        }
                        else if (node.Tag != null && node.Tag is InCommonNoteEnmtity)  //护理通用记录单
                        {
                            //保存，提交，审核, 取消审核 新增 新增记录 删除按钮 不可见
                            btnSavePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            btnSubmitPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            btnConfirmPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            btnCancelAuditPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            btnModulePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            btnAddCommonRecord.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            btnDeletePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                            //删除通用单据按钮可见 徐亮亮 注 界面上2个删除一个是删除病历的 一个是删除通用单据的
                            if (DoctorEmployee.Kind == EmployeeKind.Nurse)//当前登录人是护士
                            {
                                btnDelCommonNote.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

                            }
                            else if (CheckIsDoctor())//当前登录人是医生
                            {
                                btnDelCommonNote.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            }
                            else//其他人
                            {
                                btnDelCommonNote.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            }


                        }
                        popupMenuInTreeList.ShowPopup(treeList1.PointToScreen(new Point(e.X, e.Y)));
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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
                //保存，删除，提交，审核, 取消审核 不可见
                btnSavePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btnDeletePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btnSubmitPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btnConfirmPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                btnCancelAuditPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                btnDelCommonNote.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                //模板 可见
                btnModulePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;


                SetPopupBarButton();

                //针对首页、三测单之类非病历类型的界面需要保存功能
                if (((EmrModelContainer)node.Tag).EmrContainerType == ContainerType.None
                    && ((EmrModelContainer)node.Tag).ContainerCatalog == ContainerCatalog.BingAnShouYe)
                {
                    btnSavePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
                //针对护理记录想象的配置
                if (((EmrModelContainer)node.Tag).ContainerCatalog == "AP")
                {
                    btnModulePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                    if (DoctorEmployee.Kind == EmployeeKind.Nurse)//当前登录人是护士
                    {
                        btnAddCommonRecord.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }
                    else if (CheckIsDoctor())//当前登录人是医生
                    {
                        btnAddCommonRecord.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    }
                    else//其他人
                    {
                        btnAddCommonRecord.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    }
                }
                else
                {
                    btnAddCommonRecord.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置点击病历模板Button的显示情况
        /// </summary>
        /// <param name="node"></param>
        private void SetEmrModelButtonVisiable(TreeListNode node)
        {
            try
            {
                //保存，删除，提交，审核, 取消审核 可见
                btnSavePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnDeletePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnSubmitPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnConfirmPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                btnCancelAuditPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //模板 不可见
                btnModulePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                SetPopupBarButton();
                //SetDailyEmrAllowDelete(node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置弹出BarButton
        /// </summary>
        private void SetPopupBarButton()
        {
            try
            {
                btnModulePopup.Enabled = btn_new.Enabled;
                if (!btnModulePopup.Enabled)
                {
                    btnModulePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    btnModulePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                btnDeletePopup.Enabled = barButton_Delete.Enabled;
                if (!barButton_Delete.Enabled)
                {
                    btnDeletePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    btnDeletePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                btnSavePopup.Enabled = barButtonSave.Enabled;
                if (!barButtonSave.Enabled)
                {
                    btnSavePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    btnSavePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                btnSubmitPopup.Enabled = barButton_Submit.Enabled;
                if (!barButton_Submit.Enabled)
                {
                    btnSubmitPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    btnSubmitPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                btnConfirmPopup.Enabled = barButton_Audit.Enabled;
                btnCancelAuditPopup.Enabled = barButtonItemCancelAudit.Enabled;

                if (!barButton_Audit.Enabled)
                {
                    btnConfirmPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    btnConfirmPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                if (!barButtonItemCancelAudit.Enabled)
                {
                    btnCancelAuditPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
                else
                {
                    btnCancelAuditPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region BarButton ItemClick

        #region print

        private void barButtonPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_Preview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SetEmrReadOnly2();
                PrientViewDocment();
                SelectedPageChanged(xtraTabControl1.SelectedTabPage);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_JumpPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SetEmrReadOnly2();
                JumpPrint();
                SelectedPageChanged(xtraTabControl1.SelectedTabPage);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_Print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SetEmrReadOnly2();
                PrintDocment();
                SelectedPageChanged(xtraTabControl1.SelectedTabPage);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Undo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    Undo();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Redo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    Redo();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 插入表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_InsertTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    InsertTable();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_InsertRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    InsertTableRow();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 插入列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_InsertCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    InsertTableColumn();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_DeletTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    DeleteTable();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_DeletCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    DeletTableColumn();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_DeletRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    DeletTableRow();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 选择表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barCheck_ChooseTable_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    ChoiceTable();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 单元格信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_CellInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    SetTableCellProperty();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 表格信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemTableProperty_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    SetTableProperty();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 只读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmrReadOnly_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SetEmrReadOnly();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void SetEmrReadOnly()
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    SetDocmentReadOnlyMode();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetEmrReadOnly2()
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    SetDocmentReadOnlyMode();
                    this.CurrentForm.zyEditorControl1.ActiveEditArea = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 申请编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SetEmrAllowEdit();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void SetEmrAllowEdit()
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                    CurrentForm.zyEditorControl1.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region  toolbar editor
        private void btnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.CurrentForm.zyEditorControl1.EMRDoc._Copy();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    this.CurrentForm.zyEditorControl1.EMRDoc._Paste();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_Cut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    this.CurrentForm.zyEditorControl1.EMRDoc._Cut();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_Undo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.CurrentForm.zyEditorControl1.EMRDoc._Undo();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_Redo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.CurrentForm.zyEditorControl1.EMRDoc._Redo();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barEditItem_Font_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    if (barEditItem_Font.EditValue == null) return;
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectioinFontName(this.barEditItem_Font.EditValue.ToString());
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barEditItem_FontSize_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (barEditItem_FontSize.EditValue == null) return;

                float size = FontCommon.GetFontSizeByName(barEditItem_FontSize.EditValue.ToString());

                if (CanEdit())
                {
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontSize(size);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        private void barEditItem_FontSize_EditValueChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (barEditItem_FontSize.EditValue == null) return;

                float size = FontCommon.GetFontSizeByName(barEditItem_FontSize.EditValue.ToString());

                if (CanEdit())
                {
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontSize(size);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_Bold_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    //确定按钮的状态
                    //this.barButtonItem_Bold.Down = this.barButtonItem_Bold.Down ? true : false;
                    ReSetSupSub();
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontBold(this.barButtonItem_Bold.Down);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_Italy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    //this.barButtonItem_Italy.Down = this.barButtonItem_Italy.Down ? true : false;
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontItalic(this.barButtonItem_Italy.Down);
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_Undline_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    //this.barButtonItem_Undline.Down = this.barButtonItem_Undline.Down ? true : false;
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionUnderLine(this.barButtonItem_Undline.Down);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItemBold_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    //this.barButtonItem_Undline.Down = this.barButtonItem_Undline.Down ? true : false;
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontBold(this.barButtonItemBold.Down);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_Sup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    //this.barButtonItem_Sup.Down = this.barButtonItem_Sup.Down ? true : false;
                    ReSetSupSub();
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSup(barButtonItem_Sup.Down);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_Sub_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    //this.barButtonItem_Sub.Down = this.barButtonItem_Sub.Down ? true : false;
                    ReSetSupSub();
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSub(barButtonItem_Sub.Down);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barEditItem_Font_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    if (barEditItem_Font.EditValue == null) return;
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectioinFontName(this.barEditItem_Font.EditValue.ToString());
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 大纲视图

        private void panelContainer2_ActiveChildChanged(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            try
            {
                if (panelContainer2.ActiveChild == dockPanelView)
                {
                    if (CurrentForm != null)
                    {
                        CurrentForm.zyEditorControl1.EMRDoc.GetBrief(treeView1);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode node = this.treeView1.SelectedNode;//this.treeView1.GetNodeAt(e.Location);
                if (node != null)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.Content.AutoClearSelection = true;
                    CurrentForm.zyEditorControl1.EMRDoc.Content.MoveSelectStart((node.Tag as ZYFixedText).FirstElement);
                    CurrentForm.zyEditorControl1.ScrollViewtopToCurrentElement();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 工具栏

        /// <summary>
        /// 初始化病历提取列表
        /// </summary>
        private void GetDataCollect()
        {
            try
            {
                gridControlCollect.DataSource = m_patUtil.GetDocCollectContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void gridControlCollect_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    GridHitInfo hitInfo = gridViewDataCollect.CalcHitInfo(e.Location);
                    if (hitInfo.RowHandle >= 0)
                    {
                        popupMenuDataCollect.ShowPopup(gridControlCollect.PointToScreen(e.Location));
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnInsertCollect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                InsertCollect();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void btnDeleteCollect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DeleteDataCollect();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void gridControlCollect_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                GridHitInfo hitInfo = gridViewDataCollect.CalcHitInfo(e.Location);
                if (hitInfo.RowHandle >= 0)
                {
                    InsertCollect();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void InsertCollect()
        {
            try
            {
                //if (CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel != DocumentModel.Edit) return;
                if (!barButtonSave.Enabled) return;

                DataRowView dr = gridViewDataCollect.GetRow(gridViewDataCollect.FocusedRowHandle) as DataRowView;
                if (dr != null)
                {
                    if (CurrentForm != null)
                    {
                        string collect = dr["CONTENT"].ToString();
                        CurrentForm.zyEditorControl1.EMRDoc.Content.InsertString(collect);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DeleteDataCollect()
        {
            try
            {
                DataRowView dr = gridViewDataCollect.GetRow(gridViewDataCollect.FocusedRowHandle) as DataRowView;
                m_patUtil.CancelDocCollectContent(dr["ID"].ToString());
                gridViewDataCollect.DeleteRow(gridViewDataCollect.FocusedRowHandle);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存
        /// edit by Yanqiao.Cai 2012-11-21
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                GC.Collect();
                if (m_CurrentModel != null)
                {
                    SaveDocment(m_CurrentModel);
                    this.HideWaitDialog();
                }
                else if (m_CurrentEmrEditor != null)
                {
                    SetWaitDialogCaption("正在保存,请稍等...");
                    // 针对非病历类型的界面 如首页、三测单等
                    m_CurrentEmrEditor.Save();
                    this.HideWaitDialog();
                }
            }
            catch (Exception ex)
            {
                this.HideWaitDialog();
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        #region 保存时查看当前病历是否有月经史，只提示，如果用户还是保存，我们则让它保存吧
        /// <summary>
        /// add by ywk 2012年11月20日16:27:46
        /// </summary>
        /// <returns></returns>
        public bool CheckMouthHistory()
        {
            try
            {
                string mrecord_content = string.Empty;
                bool IsChecked = true;//是否验证通过  默认给通过  edit by ywk 2013年1月19日16:42:24
                XmlDocument xmlRecord = new XmlDocument();
                xmlRecord.LoadXml(m_CurrentModel.ModelContent.InnerXml);
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

                if (m_app.CurrentPatientInfo.PersonalInformation.Sex.Code.ToString() == "1")//是男性再进循环体
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
                throw new Exception(ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSavePopup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                GC.Collect();
                if (m_CurrentModel != null)
                {
                    SetWaitDialogCaption("正在保存病历,请稍等...");
                    SaveDocment(m_CurrentModel);
                    this.HideWaitDialog();
                }
                else if (m_CurrentEmrEditor != null)
                {
                    SetWaitDialogCaption("正在保存,请稍等...");
                    // 针对非病历类型的界面 如首页、三测单等
                    m_CurrentEmrEditor.Save();
                    this.HideWaitDialog();
                }
            }
            catch (Exception ex)
            {
                this.HideWaitDialog();
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 保存，供外部调用
        /// </summary>
        public void SaveDocumentPublic()
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    SaveDocment(m_CurrentModel);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存方法
        /// edit by Yanqiao.Cai 2012-12-03
        /// </summary>
        private void SaveDocment(EmrModel model)
        {
            try
            {
                if (!bar2.Visible) return;//当Bar2不显示的时候不需要执行保存操作
                if (model == null) return;

                Save(model);
            }
            catch (Exception ex)
            {
                this.HideWaitDialog();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (null != CurrentForm && null != CurrentForm.zyEditorControl1 && null != CurrentForm.zyEditorControl1.EMRDoc)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.Modified = false;
                }
                //保存后重新加载病历信息节点树信息
                InitBLMessage();

                this.HideWaitDialog();
            }
            m_IsSaving = false;
        }

        /// <summary>
        /// 保存方法
        /// edit by Yanqiao.Cai 2012-11-22
        /// add try ... catch
        /// <param name="model"></param>
        private void Save(EmrModel model)
        {
            #region "处理事务前方法"
            /**
            #region 已注释
            //SqlConnection conn = new SqlConnection(YD_SqlHelper.ConnectionString);
            //conn.Open();
            //SqlTransaction tran = conn.BeginTransaction();
            //SqlCommand command = new SqlCommand();
            //command.Connection = conn;
            //command.Transaction = tran;
            #endregion
            try
            {
                if (model == null) return;

                #region 已注释
                //if (model.InstanceId == -1)
                //{
                //    this.InsertOperRecordLog(model, "1");//add by wyt 
                //}
                //else
                //{
                //    this.InsertOperRecordLog(model, "2");//add by wyt 
                //}
                //对应病程的病历做特殊处理
                #endregion
                if (model.DailyEmrModel)
                {
                    
                    //对病程的保存操作，实际上都是对首程的保存,除首程外的病程都没有内容
                    if (m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Tag is EmrModelContainer)
                    {
                        #region 已注释 edit by cyq 2012-11-22
                        //edit by cyq 2012-11-22
                        //foreach (TreeListNode node in m_CurrentTreeListNode.ParentNode.Nodes)
                        //{
                        //    EmrModel modelevey = (EmrModel)node.Tag;
                        //    if (modelevey.DepartCode == model.DepartCode)
                        //    {
                        //        SaveDocumentInner(modelevey);
                        //    }
                        //}
                        #endregion

                        //根据页面首程获取该首程下所有病历
                        List<EmrModel> list = GetAllRecordModels(model);
                        //获取数据库中所有病历(包含valid=0的)
                        DataTable allrecords = YD_SqlService.GetRecordsByNoofinpatContainDel(int.Parse(m_CurrentInpatient.NoOfFirstPage.ToString()));//edit by cyq 2013-01-11 处理事务
                        //页面传递过来的节点在数据库中已删除的集合
                        List<int> deleteIDList  = allrecords.Select(" valid = 0 ").Select(p => int.Parse(p["ID"].ToString())).ToList();
                        list = list.Where(p => !deleteIDList.Contains(p.InstanceId)).ToList();
                        //1、首程为编辑时时先保存病历节点
                        //2、首程为新增时时先保存首程节点
                        #region 已注释 by cyq 2013-01-16
                        //EmrModel firstEmr = null;
                        //if (list.Any(p => p.FirstDailyEmrModel))
                        //{
                        //    var firstEmrs = list.Where(p => p.FirstDailyEmrModel && p.DepartCode == model.DepartCode);
                        //    if (null != firstEmrs && firstEmrs.Count() == 1)
                        //    {
                        //        firstEmr = firstEmrs.FirstOrDefault();
                        //    }
                        //}
                        //if (null == firstEmr)
                        //{
                        //    firstEmr = model;
                        //}
                        #endregion
                        EmrModel firstEmr = model;
                        if (null == firstEmr || !firstEmr.FirstDailyEmrModel)
                        {
                            if (list.Any(p => p.FirstDailyEmrModel))
                            {
                                var firstEmrs = list.Where(p => p.FirstDailyEmrModel && p.DepartCode == model.DepartCode).OrderByDescending(q => q.DisplayTime);
                                if (null != firstEmrs && firstEmrs.Count() > 0)
                                {
                                    firstEmr = firstEmrs.FirstOrDefault();
                                }
                            }
                        }
                        
                        int firstEmr_ID = firstEmr.InstanceId;
                        if (firstEmr_ID == -1)
                        {//新增首程
                            if (ExistTheFirstRecord(firstEmr, m_CurrentInpatient.NoOfFirstPage.ToString())) //edit by cyq 2013-01-11 处理事务
                            {//首程已存在
                                //add by Yanqiao.Cai 2012-12-03 此变量改为全局变量
                                isSaved = false;
                                if (Common.Ctrs.DLG.MessageBox.Show("该病人已存在首程，不可重复创建。您是否要刷新病程记录？", "首程已存在", Common.Ctrs.DLG.MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }
                                //刷新病历树节点
                                RemoveAllPagesExceptMain();
                                InitPatTree();
                                //SetSelectNode("病程记录");
                                return;
                            }
                            SaveDocumentInner(firstEmr, list);
                        }
                        //保存节点
                        foreach (EmrModel em in list.Where(p => p.InstanceId != firstEmr.InstanceId && p.FirstDailyEmrModel == false))
                        {
                            SaveDocumentInner(em, list);
                        }
                        if (firstEmr_ID != -1)
                        {//编辑首程
                            SaveDocumentInner(firstEmr, list);
                        }
                    }
                    isSaved = true;
                }
                else
                {
                    SaveDocumentInner(model);
                }
                //YD_SqlHelper.CommitTransaction();
            }
            catch(SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            **/
            #endregion

            try
            {
                //检查病人保存病历时是否已转科 edit by cyq 2013-02-28
                string checkDept = CheckIfCurrentDept(int.Parse(m_CurrentInpatient.NoOfFirstPage.ToString()));
                if (!string.IsNullOrEmpty(checkDept))
                {
                    isSaved = false;
                    MessageBox.Show(checkDept);
                    return;
                }

                //检查是否男性且包含月经史 add by cyq 2013-03-01
                if (!CheckMouthHistory())
                {
                    if (MyMessageBox.Show(string.Format("该患者为男性，但病历内容含有月经史，您是否要继续保存？"), "提示信息", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.No)
                    {
                        isSaved = false;
                        return;
                    }
                }
                //SetWaitDialogCaption("正在保存，请稍等...");
                m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());

                //原保存对比的方法 2013-03-20 采用新判断(若加载病历到保存病历这个时间段首程未修改过，则直接保存)
                //SaveInTran(model);
                ///是否提示刷新病程
                bool isTipRefreash = false;
                if (model.DailyEmrModel)
                {
                    if (model.InstanceId == -1)
                    {///新增
                        if (ExistTheFirstRecord(model, (int)m_CurrentInpatient.NoOfFirstPage))
                        {///首程已存在
                            isSaved = false;
                            if (MyMessageBox.Show("该病人已存在首程，不可重复创建。您是否要刷新病程记录？", "提示信息", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.No)
                            {
                                return;
                            }
                            //刷新病历树节点
                            xtraTabControl1.TabPages.Clear();
                            InitPatTree();
                            return;
                        }
                        SaveForSpeedInTran(model);
                    }
                    else
                    {///修改
                        DataTable firstEmrDt = DS_SqlService.GetRecordByIDContainsDel(model.InstanceId);
                        if (null == firstEmrDt || firstEmrDt.Rows.Count == 0)
                        {
                            return;
                        }
                        DataRow firstEmrRow = firstEmrDt.Rows[0];
                        string[] oldFirstEmr = m_RecordUpdateProcessList.FirstOrDefault(p => int.Parse(p[0]) == int.Parse(firstEmrRow["ID"].ToString()));
                        if (null != oldFirstEmr && oldFirstEmr[1] == firstEmrRow["startupdateflag"].ToString() && oldFirstEmr[2] == firstEmrRow["endupdateflag"].ToString())
                        {///未修改过
                            if (int.Parse(firstEmrRow["valid"].ToString()) == 0)
                            {
                                MessageBox.Show("该首程已被删除，请联系管理员或刷新此页面重试。");
                                return;
                            }
                            SaveForSpeedInTran(model);
                        }
                        else
                        {///已修改过
                            if (model.DailyEmrModel)
                            {
                                isTipRefreash = true;
                            }
                            SaveInTran(model);
                        }
                    }
                }
                else
                {//保存非病历病程
                    SaveDocumentInner(model);
                    isSaved = true;
                }

                this.HideWaitDialog();
                if (isSaved)
                {
                    MessageBox.Show("保存成功");
                }
                if (isTipRefreash)
                {
                    ///刷新病历节点
                    if (MyMessageBox.Show("系统检测到其他人修改过此病程，建议刷新病程记录以提高保存效率。您是否要刷新病程记录？", "提示信息", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.No)
                    {
                        return;
                    }
                    ReFreshRecordData();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-14</date>
        /// <param name="model"></param>
        private void SaveInTran(EmrModel model)
        {
            #region 注释
            //SqlConnection conn = new SqlConnection(YD_SqlHelper.ConnectionString);
            //conn.Open();
            //SqlTransaction tran = conn.BeginTransaction();
            //SqlCommand command = new SqlCommand();
            //command.Connection = conn;
            //command.Transaction = tran;
            #endregion
            try
            {
                if (model == null)
                {
                    return;
                }

                if (model.DailyEmrModel)
                {
                    //对病程的保存操作，实际上都是对首程的保存,除首程外的病程都没有内容
                    if (m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Tag is EmrModelContainer)
                    {
                        //根据页面首程获取该首程下所有病历
                        List<EmrModel> list = GetAllRecordModels(model);//edit by cyq 2013-01-11 处理事务
                        if (null != list && list.Count() >= 20)
                        {
                            m_WaitDialog = new WaitDialogForm("该病程已被其它客户端修改过，系统将对比分析后再保存，可能耗时略长，请耐心等待。", "正在保存，请稍候...", new Size(570, 60));
                        }
                        else
                        {
                            DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在保存，请稍候...");
                        }

                        //SetWaitDialogCaption("正在保存，请稍等...");
                        //获取数据库中所有病历(包含valid=0的)
                        DataTable allrecords = DS_SqlService.GetRecordsByNoofinpatContainDel(int.Parse(m_CurrentInpatient.NoOfFirstPage.ToString()));
                        //页面传递过来的节点在数据库中已删除的集合
                        List<int> deleteIDList = allrecords.Select(" valid = 0 ").Select(p => int.Parse(p["ID"].ToString())).ToList();
                        list = list.Where(p => !deleteIDList.Contains(p.InstanceId)).ToList();
                        //1、首程为编辑时时先保存病历节点
                        //2、首程为新增时时先保存首程节点
                        EmrModel firstEmr = model;
                        if (null == firstEmr || !firstEmr.FirstDailyEmrModel)
                        {
                            if (list.Any(p => p.FirstDailyEmrModel))
                            {
                                var firstEmrs = list.Where(p => p.FirstDailyEmrModel && p.DepartCode == model.DepartCode).OrderByDescending(q => q.DisplayTime);
                                if (null != firstEmrs && firstEmrs.Count() > 0)
                                {
                                    firstEmr = firstEmrs.FirstOrDefault();
                                }
                            }
                        }
                        //判断首次病程是否已经被删除
                        if (model.InstanceId != -1 && deleteIDList.Contains(model.InstanceId))
                        {
                            string userStr = string.Empty;
                            DataRow firstRecordDR = allrecords.Select(" valid = 0 ").Where(p => int.Parse(p["id"].ToString()) == model.InstanceId).FirstOrDefault();
                            if (null != firstRecordDR)
                            {
                                string reateUserID = null == firstRecordDR["owner"] ? "" : firstRecordDR["owner"].ToString().Trim();
                                if (!string.IsNullOrEmpty(reateUserID))
                                {
                                    userStr = DS_BaseService.GetUserNameAndID(reateUserID);
                                }
                            }
                            throw new Exception("该首次病程已经被 " + userStr + " 删除，请联系 " + userStr + " 或者 管理员。");
                        }

                        #region 方法封装 by cyq 2013-01-15
                        /**
                        if (firstEmr_ID == -1)
                        {//新增首程
                            if (ExistTheFirstRecordInTran(firstEmr, m_CurrentInpatient.NoOfFirstPage.ToString()))
                            {//首程已存在
                                //add by Yanqiao.Cai 2012-12-03 此变量改为全局变量
                                isSaved = false;
                                if (Common.Ctrs.DLG.MessageBox.Show("该病人已存在首程，不可重复创建。您是否要刷新病程记录？", "首程已存在", Common.Ctrs.DLG.MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    YD_SqlHelper.CommitTransaction();
                                    return;
                                }
                                YD_SqlHelper.CommitTransaction();
                                //刷新病历树节点
                                RemoveAllPagesExceptMain();
                                InitPatTree();
                                //SetSelectNode("病程记录");
                                return;
                            }
                            else
                            {
                                SaveDocumentInnerInTran(firstEmr, list);
                            }
                        }
                        //保存节点
                        foreach (EmrModel em in list.Where(p => p.InstanceId != firstEmr.InstanceId && p.FirstDailyEmrModel == false))
                        {
                            SaveDocumentInnerInTran(em, list);
                        }
                        **/
                        #endregion
                        //保存所有节点(包含首程节点)
                        List<string[]> actionList = SaveNodesInTran(firstEmr, list, allrecords.Select(" 1=1 ").ToList());
                        #region 方法封装 by cyq 2013-01-15
                        //if (firstEmr_ID != -1)
                        //{//编辑首程
                        //    SaveDocumentInnerInTran(firstEmr, list);
                        //}
                        #endregion
                        //更新首程内容
                        SaveFirstDailyInTran(firstEmr, list, allrecords.Select(" 1=1 ").ToList(), actionList);

                        if (null != list && list.Count() >= 20)
                        {
                            if (null != m_WaitDialog)
                            {
                                m_WaitDialog.Hide();
                                m_WaitDialog = null;
                            }
                        }
                    }
                    isSaved = true;
                }
                else
                {//保存非病历病程
                    SaveDocumentInner(model);
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                isSaved = false;
                if (ex.Message != "return")
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// 保存方法 --- 提速，不做对比
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-20</date>
        /// <param name="model"></param>
        private void SaveForSpeedInTran(EmrModel model)
        {
            try
            {
                if (model == null)
                {
                    return;
                }
                SetWaitDialogCaption("正在保存，请稍等...");
                if (model.DailyEmrModel)
                {
                    if (null != m_CurrentTreeListNode && null != m_CurrentTreeListNode.ParentNode && m_CurrentTreeListNode.ParentNode.Tag is EmrModelContainer)
                    {
                        ///根据页面首程获取该首程下所有病历
                        List<EmrModel> list = GetAllRecordModels(model);
                        ///获取数据库中所有病历
                        var dbList = DS_SqlService.GetRecordsByNoofinpat((int)m_CurrentInpatient.NoOfFirstPage).Select(" 1=1 ");
                        ///a、更新首程开始修改标识
                        if (model.InstanceId >= 0)
                        {
                            DS_SqlService.UpdateRecordUpdateflag(model.InstanceId, 0);
                        }
                        ///b、更新首程(包含XML)及所有节点（事务）
                        DS_SqlHelper.CreateSqlHelper();
                        DS_SqlHelper.BeginTransaction();
                        foreach (EmrModel em in list)
                        {
                            if (em.FirstDailyEmrModel && model.FirstDailyEmrModel && em.InstanceId != model.InstanceId)
                            {
                                continue;
                            }
                            if (dbList.Any(p => p["captiondatetime"].ToString() == em.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") && int.Parse(p["ID"].ToString()) != em.InstanceId))
                            {
                                DS_SqlHelper.AppDbTransaction.Rollback();
                                throw new Exception("数据库中已存在时间为 " + em.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " 的病历，请选择其它的时间。");
                            }
                            List<OracleParameter> paramList = m_RecordDal.GetRecordParams(em, (int)m_CurrentInpatient.NoOfFirstPage, em.InstanceId == -1 ? EditState.Add : EditState.Edit);
                            if (em.FirstDailyEmrModel)
                            {
                                //content(param5)
                                OracleParameter param5 = new OracleParameter("content", OracleType.VarChar);
                                param5.Value = em.ModelContent.OuterXml;
                                paramList.Add(param5);
                            }
                            if (em.InstanceId == -1)
                            {
                                em.InstanceId = DS_SqlService.InsertRecordInTran(paramList);
                            }
                            else
                            {
                                DS_SqlService.UpdateRecordInTran(paramList);
                            }
                        }
                        DS_SqlHelper.CommitTransaction();
                        ///c、更新首程结束修改标识
                        DS_SqlService.UpdateRecordUpdateflag(model.InstanceId, 1);
                        ///d、刷新全局变量中此首程的开始修改和结束修改标识
                        SetRecordUpdateFlag(model.InstanceId);
                    }
                    isSaved = true;
                }
                else
                {///保存非病历病程
                    SaveDocumentInner(model);
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                isSaved = false;
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存所有病历 --- 导入历史病历专用
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-11</date>
        /// </summary>
        /// <param name="list">所有节点集合</param>
        public void SaveDocumentByModelList(List<EmrModel> emrList, RecordDal m_record, Inpatient m_Inpatient)
        {
            try
            {
                if (null != emrList && emrList.Count > 0)
                {

                    m_RecordDal = m_record;
                    m_CurrentInpatient = m_Inpatient;
                    //首次病程列表
                    var firstEmrList = emrList.Where(p => p.FirstDailyEmrModel).OrderBy(q => q.DisplayTime);
                    foreach (EmrModel model in firstEmrList)
                    {
                        var list = emrList;
                        if (firstEmrList.Any(p => p.DisplayTime > model.DisplayTime))
                        {
                            EmrModel nextFirstEmr = firstEmrList.Where(p => p.DisplayTime > model.DisplayTime).OrderBy(q => q.DisplayTime).FirstOrDefault();
                            if (null != nextFirstEmr)
                            {
                                list = list.Where(p => p.DisplayTime >= model.DisplayTime && p.DisplayTime < nextFirstEmr.DisplayTime && p.DepartCode == model.DepartCode).OrderBy(q => q.DisplayTime).ToList();
                            }
                        }
                        else
                        {
                            list = list.Where(p => p.DisplayTime >= model.DisplayTime && p.DepartCode == model.DepartCode).OrderBy(q => q.DisplayTime).ToList();
                        }
                        //1、首程为编辑时时先保存病历节点
                        //2、首程为新增时时先保存首程节点
                        int firstEmr_ID = model.InstanceId;
                        if (firstEmr_ID == -1)
                        {//新增首程
                            if (ExistTheFirstRecord(model, (int)m_CurrentInpatient.NoOfFirstPage))
                            {//首程已存在
                                //add by Yanqiao.Cai 2012-12-03 此变量改为全局变量
                                isSaved = false;
                                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人已存在首程，不可重复创建。您是否要刷新病程记录？", "首程已存在", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    return;
                                }
                                //刷新病历树节点
                                xtraTabControl1.TabPages.Clear();
                                InitPatTree();
                                return;
                            }
                            SaveDocumentInner(model, list);
                        }
                        //保存节点
                        foreach (EmrModel em in list.Where(p => p.InstanceId != model.InstanceId))
                        {
                            SaveDocumentInner(em, list);
                        }
                        if (firstEmr_ID != -1)
                        {//编辑首程
                            SaveDocumentInner(model, list);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SaveDocumentInner(EmrModel model)
        {
            try
            {
                SetWaitDialogCaption("正在保存，请稍等...");
                //add by cyq 2012-11-21 用于保存病历事件比较时确定是新增还是修改
                m_RecordDal.modelList = GetAllRecordModels(model);
                //新增
                if (model.InstanceId == -1)
                {
                    model.CreatorXH = m_app.User.Id;

                    //add by wyt
                    m_RecordDal.InsertModelInstance(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));

                }
                else//修改
                {
                    m_RecordDal.UpdateModelInstance(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存所有病历
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-14</date>
        /// </summary>
        /// <param name="model"></param>
        private void SaveDocumentInnerInTran(EmrModel model)
        {
            try
            {
                //add by cyq 2012-11-21 用于保存病历事件比较时确定是新增还是修改
                m_RecordDal.modelList = GetAllRecordModelsInTran(model);
                //新增
                if (model.InstanceId == -1)
                {
                    model.CreatorXH = m_app.User.Id;

                    //add by wyt
                    m_RecordDal.InsertModelInstanceInTran(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));

                }
                else//修改
                {
                    m_RecordDal.UpdateModelInstanceInTran(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存所有病历
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-11</date>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list">所有节点集合</param>
        private void SaveDocumentInner(EmrModel model, List<EmrModel> list)
        {
            try
            {
                //add by cyq 2012-11-21 用于保存病历事件比较时确定是新增还是修改
                m_RecordDal.modelList = list;
                //新增
                if (model.InstanceId == -1)
                {
                    model.CreatorXH = m_app.User.Id;

                    //add by wyt
                    m_RecordDal.InsertModelInstance(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));

                }
                else//修改
                {
                    m_RecordDal.UpdateModelInstance(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存所有病历
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-14</date>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list">所有节点集合</param>
        private void SaveDocumentInnerInTran(EmrModel model, List<EmrModel> list)
        {
            try
            {
                //add by cyq 2012-11-21 用于保存病历事件比较时确定是新增还是修改
                m_RecordDal.modelList = list;
                //新增
                if (model.InstanceId == -1)
                {
                    model.CreatorXH = m_app.User.Id;

                    //add by wyt
                    m_RecordDal.InsertModelInstanceInTran(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));

                }
                else//修改
                {
                    m_RecordDal.UpdateModelInstanceInTran(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存节点
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-15</date>
        /// <param name="firstEmr">首程</param>
        /// <param name="list">页面节点集合</param>
        /// <param name="allrecords">数据库所有节点集合(包含无效)</param>
        /// <returns>返回新增&编辑记录集</returns>
        private List<string[]> SaveNodesInTran(EmrModel firstEmr, List<EmrModel> list, List<DataRow> allrecords)
        {
            try
            {
                if (null == firstEmr)
                {
                    return new List<string[]>();
                }
                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.BeginTransaction();
                List<string[]> actionList = new List<string[]>();
                List<DataRow> dbList = allrecords.Where(p => int.Parse(p["valid"].ToString()) == 1).ToList();
                int firstEmr_ID = firstEmr.InstanceId;
                if (firstEmr_ID == -1)
                {//新增首程
                    if (ExistTheFirstRecordInTran(firstEmr, (int)m_CurrentInpatient.NoOfFirstPage))
                    {//首程已存在
                        //add by Yanqiao.Cai 2012-12-03 此变量改为全局变量
                        isSaved = false;
                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人已存在首程，不可重复创建。您是否要刷新病程记录？", "首程已存在", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            DS_SqlHelper.AppDbTransaction.Rollback();
                            throw new Exception("return");
                        }
                        DS_SqlHelper.AppDbTransaction.Rollback();
                        //刷新病历树节点
                        ReFreshRecordData();
                        isSaved = false;
                        throw new Exception("return");
                    }
                    else if (null != allrecords && allrecords.Count() > 0 && allrecords.Any(p => DateTime.Parse(p["captiondatetime"].ToString()) >= firstEmr.DisplayTime))
                    {//数据库中存在大于该新增首程的病历节点
                        string memo = string.Empty;
                        if (dbList.Any(p => p["captiondatetime"].ToString() == firstEmr.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss")))
                        {//数据库中存在该新增首程时间的病历节点
                            memo = "数据库中已存在时间为 " + firstEmr.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " 的病历，请选择其它的时间。";
                        }
                        else if (dbList.Any(p => DateTime.Parse(p["captiondatetime"].ToString()) > firstEmr.DisplayTime))
                        {
                            memo = "该首程后已存在其它科室病历，无法保存。";
                        }
                        else if (allrecords.Any(p => int.Parse(p["valid"].ToString()) == 0 && p["captiondatetime"].ToString() == firstEmr.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss")))
                        {
                            throw new Exception("数据库中已存在时间为 " + firstEmr.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " 的无效病历，请选择其它的时间。");
                        }
                        else if (allrecords.Any(p => DateTime.Parse(p["captiondatetime"].ToString()) > firstEmr.DisplayTime && p["departcode"].ToString().Trim() != firstEmr.DepartCode))
                        {
                            DataRow noValidRow = allrecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > firstEmr.DisplayTime && p["departcode"].ToString().Trim() != firstEmr.DepartCode).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                            if (null != noValidRow)
                            {
                                string memoStr = "该首程后存在其它科室的无效节点，无法保存，建议将首程时间改为 " + noValidRow["captiondatetime"].ToString() + " 以后。";
                                throw new Exception(memoStr);
                            }
                        }
                        if (!string.IsNullOrEmpty(memo))
                        {
                            memo += "您是否要刷新病程记录？";
                            if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(memo, "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                ReFreshRecordData();
                            }
                            throw new Exception("return");
                        }
                    }

                    SaveDocumentInnerInTran(firstEmr, list);
                    string[] nodeArray = new string[5];
                    nodeArray[0] = "add";
                    nodeArray[1] = firstEmr.InstanceId.ToString().Trim();
                    nodeArray[2] = firstEmr.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                    actionList.Add(nodeArray);
                }
                //保存节点
                var nodeList = null == list ? null : list.Where(p => p.InstanceId != firstEmr.InstanceId && p.FirstDailyEmrModel == false).OrderBy(q => q.DisplayTime);
                if (null != nodeList && nodeList.Count() > 0)
                {
                    //获取当前首程下一个首程
                    DataRow nextFirstDaily = null == dbList ? null : dbList.OrderBy(p => DateTime.Parse(p["captiondatetime"].ToString())).FirstOrDefault(q => int.Parse(q["firstdailyflag"].ToString()) == 1 && int.Parse(q["id"].ToString()) != firstEmr.InstanceId && DateTime.Parse(q["captiondatetime"].ToString()) > firstEmr.DisplayTime);
                    if (null != nextFirstDaily && null != nextFirstDaily["captiondatetime"])
                    {
                        //获取当前首程下最后一个病历
                        EmrModel lastRecord = nodeList.LastOrDefault();
                        if (null != lastRecord && lastRecord.DisplayTime >= DateTime.Parse(nextFirstDaily["captiondatetime"].ToString()))
                        {
                            if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该科室最后一个病历前已存在其他首程，无法保存。您是否要刷新病程记录？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                ReFreshRecordData();
                            }
                            throw new Exception("return");
                        }
                    }

                    foreach (EmrModel em in nodeList)
                    {
                        if (em.FirstDailyEmrModel && firstEmr.FirstDailyEmrModel && em.InstanceId != firstEmr.InstanceId)
                        {
                            continue;
                        }
                        if (null != dbList && dbList.Count > 0)
                        {
                            if (dbList.Any(p => p["captiondatetime"].ToString() == em.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") && int.Parse(p["ID"].ToString()) != em.InstanceId))
                            {
                                DS_SqlHelper.AppDbTransaction.Rollback();
                                throw new Exception("数据库中已存在时间为 " + em.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " 的病历，请选择其它的时间。");
                            }
                            //病历时间修改限制(修改前时间和修改后时间中间不能存在首程或其他科室病历)
                            string checkStr = CheckChangedRecordTime(em, allrecords);
                            if (!string.IsNullOrEmpty(checkStr))
                            {
                                DS_SqlHelper.AppDbTransaction.Rollback();
                                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(checkStr + "您是否要刷新病程记录？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    ReFreshRecordData();
                                }
                                throw new Exception("return");
                            }
                        }

                        int theID = em.InstanceId;
                        SaveDocumentInnerInTran(em, list);
                        if (theID == -1)
                        {//新增
                            string[] nArray = new string[5];
                            nArray[0] = "add";
                            nArray[1] = em.InstanceId.ToString().Trim();
                            nArray[2] = em.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                            actionList.Add(nArray);
                        }
                        else if (null != dbList && dbList.Count > 0 && dbList.Any(p => int.Parse(p["ID"].ToString().Trim()) == em.InstanceId && !p["captiondatetime"].ToString().Contains(em.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"))))
                        {//编辑
                            string[] ndArray = new string[5];
                            ndArray[0] = "edit";
                            ndArray[1] = em.InstanceId.ToString().Trim();
                            //当前时间
                            ndArray[2] = em.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss");
                            //原来时间(编辑之前)
                            DataRow theDBRow = dbList.FirstOrDefault(p => int.Parse(p["ID"].ToString().Trim()) == em.InstanceId);
                            ndArray[3] = null == theDBRow["captiondatetime"] ? em.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") : theDBRow["captiondatetime"].ToString();
                            ndArray[4] = null == theDBRow["name"] ? em.ModelName : theDBRow["name"].ToString();
                            actionList.Add(ndArray);
                        }
                    }
                }
                DS_SqlHelper.CommitTransaction();
                return actionList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 保存首程内容(针对更新)
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-15</date>
        /// <param name="firstEmr">首程</param>
        /// <param name="list">页面节点集合</param>
        /// <param name="allDBList">数据库所有节点集合(包含无效)</param>
        /// <param name="actionList">已变更节点集合</param>
        private void SaveFirstDailyInTran(EmrModel firstEmr, List<EmrModel> list, List<DataRow> allDBList, List<string[]> actionList)
        {
            try
            {
                if (null == firstEmr || null == list || list.Count == 0)
                {
                    return;
                }
                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.BeginTransaction();

                //add by cyq 2013-01-30 关于病历时间的限制
                if (null != allDBList && allDBList.Count > 0)
                {
                    if (allDBList.Any(p => p["captiondatetime"].ToString() == firstEmr.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") && int.Parse(p["ID"].ToString()) != firstEmr.InstanceId))
                    {
                        DS_SqlHelper.AppDbTransaction.Rollback();
                        throw new Exception("数据库中已存在时间为 " + firstEmr.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " 的病历，请选择其它的时间。");
                    }
                    //首程时间修改限制(修改前时间和修改后时间中间不能存在首程或其他科室病历-包含无效)
                    string checkStr = CheckChangedRecordTime(firstEmr, allDBList);
                    if (!string.IsNullOrEmpty(checkStr))
                    {
                        DS_SqlHelper.AppDbTransaction.Rollback();
                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(checkStr + "您是否要刷新病程记录？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            ReFreshRecordData();
                        }
                        throw new Exception("return");
                    }
                }

                if (firstEmr.InstanceId != -1)
                {//编辑首程
                    SaveDocumentInnerInTran(firstEmr, list);
                }
                DS_SqlHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                RollBackNodes(actionList);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 回滚节点
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-15</date>
        /// <param name="actionList">已变更节点集合</param>
        private void RollBackNodes(List<string[]> actionList)
        {
            try
            {
                if (null == actionList || actionList.Count == 0)
                {
                    return;
                }
                //回滚节点
                //1、回滚新增节点
                var newList = actionList.Where(p => null != p[0] && p[0].Trim() == "add");
                if (null != newList && newList.Count() > 0)
                {
                    string newIDs = string.Join(",", newList.Select(p => p[1]).ToArray());
                    DS_SqlService.DeleteRecordsByIDs(Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage), newIDs);
                }
                //2、回滚编辑节点
                var editList = actionList.Where(p => null != p[0] && p[0].Trim() == "edit");
                if (null != editList && editList.Count() > 0)
                {
                    foreach (string[] array in editList)
                    {
                        if (array.Length < 5 || array[2] == array[3])
                        {
                            continue;
                        }
                        List<OracleParameter> paramList = new List<OracleParameter>();
                        OracleParameter parm1 = new OracleParameter("id", OracleType.Int32);
                        parm1.Value = null == array[1] ? -1 : int.Parse(array[1]);
                        paramList.Add(parm1);
                        OracleParameter parm2 = new OracleParameter("captiondatetime", OracleType.VarChar);
                        parm2.Value = string.IsNullOrEmpty(array[3]) ? array[2] : array[3];
                        paramList.Add(parm2);
                        if (!string.IsNullOrEmpty(array[4]))
                        {
                            OracleParameter parm3 = new OracleParameter("name", OracleType.VarChar);
                            parm3.Value = array[4];
                            paramList.Add(parm3);
                        }
                        DS_SqlService.UpdateRecord(paramList);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 已弃用 by cyq 2012-01-05
        /// <summary>
        /// 取得所保存的首次病程对应的所有病历
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-21</date>
        /// </summary>
        //private List<EmrModel> GetAllRecordModels(EmrModel model)
        //{
        //    try
        //    {
        //        //获取该病人该科室下所有首次病程
        //        DataTable dt = YD_SqlService.GetFirstRecordsByDept((int)m_CurrentInpatient.NoOfFirstPage, model.DepartCode);
        //        List<EmrModel> list = new List<EmrModel>();
        //        if (m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Tag is EmrModelContainer)
        //        {
        //            foreach (TreeListNode node in m_CurrentTreeListNode.ParentNode.Nodes)
        //            {
        //                EmrModel modelevey = (EmrModel)node.Tag;
        //                if (modelevey.DepartCode == model.DepartCode)
        //                {
        //                    bool boo = dt.Select(" 1=1 ").Any(p => int.Parse(p["ID"].ToString()) != model.InstanceId);
        //                    if (!boo)
        //                    {
        //                        list.Add(modelevey);
        //                    }
        //                    else
        //                    {
        //                        DataRow drow = dt.Select(" captiondatetime > '" + model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ").OrderBy(p => p["captiondatetime"]).FirstOrDefault();
        //                        DateTime dtStart = model.DisplayTime;
        //                        DateTime dtEnd = null == drow ? DateTime.Now : DateTime.Parse(drow["captiondatetime"].ToString());
        //                        if (modelevey.DisplayTime >= dtStart && modelevey.DisplayTime <= dtEnd)
        //                        {
        //                            list.Add(modelevey);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        #endregion

        /// <summary>
        /// 取得所保存的首次病程对应的所有病历
        /// 注：页面传递节点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-21</date>
        /// </summary>
        private List<EmrModel> GetAllRecordModels(EmrModel model)
        {
            #region 已注释 by cyq 2013-01-22 09:08 修复A->B->A后删除B科室所有病历的BUG
            /**
            try
            {
                if (null == model)
                {
                    return new List<EmrModel>();
                }
                //获取该病人该科室下所有首次病程
                DataTable dt = YD_SqlService.GetFirstRecordsByDept((int)m_CurrentInpatient.NoOfFirstPage, model.DepartCode);
                //是否存在其他首程
                bool boo = dt.Select(" 1=1 ").Any(p => int.Parse(p["ID"].ToString()) != model.InstanceId);
                //同科室病历的时间范围
                string starttime = string.Empty;
                string endtime = string.Empty;
                if (boo)
                {
                    //查找上一个和下一个非本科室的病历
                    var preRecords = YD_SqlService.GetRecordsByTimeDiv((int)m_CurrentInpatient.NoOfFirstPage, null, null).Select(" departcode <> '" + model.DepartCode + "' ").OrderBy(p => p["captiondatetime"]);
                    if (null != preRecords && preRecords.Count() > 0)
                    {
                        DataRow preOtherRecord = preRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < model.DisplayTime).OrderByDescending(p => p["captiondatetime"]).FirstOrDefault();
                        DataRow nextOtherRecord = preRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > model.DisplayTime).OrderBy(p => p["captiondatetime"]).FirstOrDefault();
                        starttime = null == preOtherRecord ? string.Empty : preOtherRecord["captiondatetime"].ToString();
                        endtime = null == nextOtherRecord ? string.Empty : nextOtherRecord["captiondatetime"].ToString();
                    }
                }
                List<EmrModel> list = new List<EmrModel>();
                if (m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Tag is EmrModelContainer)
                {
                    foreach (TreeListNode node in m_CurrentTreeListNode.ParentNode.Nodes)
                    {
                        EmrModel modelevey = (EmrModel)node.Tag;
                        if (modelevey.DepartCode != model.DepartCode)
                        {
                            continue;
                        }
                        if (!boo)
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime) && modelevey.DisplayTime >= DateTime.Parse(starttime) && modelevey.DisplayTime <= DateTime.Parse(endtime))
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        else if (!string.IsNullOrEmpty(starttime) && string.IsNullOrEmpty(endtime) && modelevey.DisplayTime >= DateTime.Parse(starttime))
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        if (string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime) && modelevey.DisplayTime <= DateTime.Parse(endtime))
                        {
                            list.Add(modelevey);
                            continue;
                        }

                        DataRow drow = dt.Select(" captiondatetime > '" + model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ").OrderBy(p => p["captiondatetime"]).FirstOrDefault();
                        DataRow theLastRow = dt.Select(" 1=1 ").OrderByDescending(p => p["captiondatetime"]).FirstOrDefault();
                        DateTime dtLastRow = null == theLastRow ? DateTime.Now : DateTime.Parse(theLastRow["captiondatetime"].ToString());
                        DateTime dtStart = model.DisplayTime;

                        if (null == drow)
                        {
                            if (modelevey.DisplayTime >= dtLastRow)
                            {
                                list.Add(modelevey);
                            }
                        }
                        else
                        {
                            DateTime dtEnd = DateTime.Parse(drow["captiondatetime"].ToString());
                            if (modelevey.DisplayTime >= dtStart && modelevey.DisplayTime < dtEnd)
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
                throw new Exception(ex.Message);
            }
            **/
            #endregion

            try
            {
                if (null == model)
                {
                    return new List<EmrModel>();
                }
                DateTime? starttime = null;
                DateTime? endtime = null;
                //获取该病人所有病历(包含无效)
                DataTable allRecords = DS_SqlService.GetRecordsByNoofinpatContainDel((int)m_CurrentInpatient.NoOfFirstPage);
                if (null == allRecords && allRecords.Rows.Count == 0)
                {
                    return new List<EmrModel>();
                }
                //获取非本科室的所有病历(包含无效)
                var otherRecords = allRecords.Select(" departcode <> '" + model.DepartCode + "' ").OrderBy(p => DateTime.Parse(p["captiondatetime"].ToString()));
                //获取上一个非本科室的病历(包含无效)
                DataRow preOtherRecord = otherRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < model.DisplayTime).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                //获取下一个非本科室的病历(包含无效)
                DataRow nextOtherRecord = otherRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > model.DisplayTime).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                if (null != preOtherRecord)
                {
                    starttime = DateTime.Parse(preOtherRecord["captiondatetime"].ToString());
                }
                if (null != nextOtherRecord)
                {
                    endtime = DateTime.Parse(nextOtherRecord["captiondatetime"].ToString());
                }

                //获取数据库中所有本科室有效首程
                var thisFirstDailys = allRecords.Select(" firstdailyflag='1' and valid = 1 and departcode = '" + model.DepartCode + "' ");
                List<EmrModel> list = new List<EmrModel>();
                if (m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Tag is EmrModelContainer)
                {
                    foreach (TreeListNode node in m_CurrentTreeListNode.ParentNode.Nodes)
                    {
                        EmrModel modelevey = (EmrModel)node.Tag;
                        if (modelevey.DepartCode != model.DepartCode)
                        {
                            continue;
                        }
                        //获取数据库中所有有效首程(当前首程除外)
                        DataRow[] allFirstValidDailys = allRecords.Select(" firstdailyflag='1' and valid = 1 and ID <> " + model.InstanceId);
                        if (null == allFirstValidDailys || allFirstValidDailys.Count() == 0)
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        if (null != starttime && null != endtime && modelevey.DisplayTime >= starttime && modelevey.DisplayTime <= endtime)
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        else if (null != starttime && null == endtime && modelevey.DisplayTime >= starttime)
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        if (null == starttime && null != endtime && modelevey.DisplayTime <= endtime)
                        {
                            list.Add(modelevey);
                            continue;
                        }

                        //下一个本科室有效首次病程
                        DataRow nextThisFirstEmr = thisFirstDailys.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > model.DisplayTime).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                        //最后一个本科室有效首次病程
                        DataRow lastThisFirstEmr = thisFirstDailys.OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                        if (null == lastThisFirstEmr)
                        {
                            continue;
                        }
                        DateTime endEmrDatetime = DateTime.Parse(lastThisFirstEmr["captiondatetime"].ToString());

                        if (null == nextThisFirstEmr && modelevey.DisplayTime >= DateTime.Parse(lastThisFirstEmr["captiondatetime"].ToString()))
                        {
                            list.Add(modelevey);
                        }
                        else if (modelevey.DisplayTime >= model.DisplayTime && modelevey.DisplayTime < DateTime.Parse(nextThisFirstEmr["captiondatetime"].ToString()))
                        {
                            list.Add(modelevey);
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
        /// 取得所保存的首次病程对应的所有病历
        /// 注：页面传递节点
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-21</date>
        /// </summary>
        private List<EmrModel> GetAllRecordModelsInTran(EmrModel model)
        {
            #region 已注释 by cyq 2013-01-22 09:08 修复A->B->A后删除B科室所有病历的BUG
            /**
            try
            {
                if (null == model)
                {
                    return new List<EmrModel>();
                }
                //获取该病人该科室下所有首次病程
                DataTable dt = YD_SqlService.GetFirstRecordsByDeptInTran((int)m_CurrentInpatient.NoOfFirstPage, model.DepartCode);
                //是否存在其他首程
                bool boo = dt.Select(" 1=1 ").Any(p => int.Parse(p["ID"].ToString()) != model.InstanceId);
                //同科室病历的时间范围
                string starttime = string.Empty;
                string endtime = string.Empty;
                if (boo)
                {
                    //查找上一个和下一个非本科室的病历
                    var preRecords = YD_SqlService.GetRecordsByTimeDivInTran((int)m_CurrentInpatient.NoOfFirstPage, null, null).Select(" departcode <> '" + model.DepartCode + "' ").OrderBy(p => p["captiondatetime"]);
                    if (null != preRecords && preRecords.Count() > 0)
                    {
                        DataRow preOtherRecord = preRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < model.DisplayTime).OrderByDescending(p => p["captiondatetime"]).FirstOrDefault();
                        DataRow nextOtherRecord = preRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > model.DisplayTime).OrderBy(p => p["captiondatetime"]).FirstOrDefault();
                        starttime = null == preOtherRecord ? string.Empty : preOtherRecord["captiondatetime"].ToString();
                        endtime = null == nextOtherRecord ? string.Empty : nextOtherRecord["captiondatetime"].ToString();
                    }
                }
                List<EmrModel> list = new List<EmrModel>();
                if (m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Tag is EmrModelContainer)
                {
                    foreach (TreeListNode node in m_CurrentTreeListNode.ParentNode.Nodes)
                    {
                        EmrModel modelevey = (EmrModel)node.Tag;
                        if (modelevey.DepartCode != model.DepartCode)
                        {
                            continue;
                        }
                        if (!boo)
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        if (!string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime) && modelevey.DisplayTime >= DateTime.Parse(starttime) && modelevey.DisplayTime <= DateTime.Parse(endtime))
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        else if (!string.IsNullOrEmpty(starttime) && string.IsNullOrEmpty(endtime) && modelevey.DisplayTime >= DateTime.Parse(starttime))
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        if (string.IsNullOrEmpty(starttime) && !string.IsNullOrEmpty(endtime) && modelevey.DisplayTime <= DateTime.Parse(endtime))
                        {
                            list.Add(modelevey);
                            continue;
                        }

                        DataRow drow = dt.Select(" captiondatetime > '" + model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ").OrderBy(p => p["captiondatetime"]).FirstOrDefault();
                        DataRow theLastRow = dt.Select(" 1=1 ").OrderByDescending(p => p["captiondatetime"]).FirstOrDefault();
                        DateTime dtLastRow = null == theLastRow ? DateTime.Now : DateTime.Parse(theLastRow["captiondatetime"].ToString());
                        DateTime dtStart = model.DisplayTime;

                        if (null == drow)
                        {
                            if (modelevey.DisplayTime >= dtLastRow)
                            {
                                list.Add(modelevey);
                            }
                        }
                        else
                        {
                            DateTime dtEnd = DateTime.Parse(drow["captiondatetime"].ToString());
                            if (modelevey.DisplayTime >= dtStart && modelevey.DisplayTime < dtEnd)
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
                throw new Exception(ex.Message);
            }
            **/
            #endregion

            try
            {
                if (null == model)
                {
                    return new List<EmrModel>();
                }
                DateTime? starttime = null;
                DateTime? endtime = null;
                //获取该病人所有病历(包含无效)
                DataTable allRecords = DS_SqlService.GetRecordsByNoofinpatContainDelInTran((int)m_CurrentInpatient.NoOfFirstPage);
                if (null == allRecords && allRecords.Rows.Count == 0)
                {
                    return new List<EmrModel>();
                }
                //获取非本科室的所有病历(包含无效)
                var otherRecords = allRecords.Select(" departcode <> '" + model.DepartCode + "' ").OrderBy(p => DateTime.Parse(p["captiondatetime"].ToString()));
                //获取上一个非本科室的病历(包含无效)
                DataRow preOtherRecord = otherRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < model.DisplayTime).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                //获取下一个非本科室的病历(包含无效)
                DataRow nextOtherRecord = otherRecords.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > model.DisplayTime).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                if (null != preOtherRecord)
                {
                    starttime = DateTime.Parse(preOtherRecord["captiondatetime"].ToString());
                }
                if (null != nextOtherRecord)
                {
                    endtime = DateTime.Parse(nextOtherRecord["captiondatetime"].ToString());
                }

                //获取数据库中所有本科室有效首程
                var thisFirstDailys = allRecords.Select(" firstdailyflag='1' and valid = 1 and departcode = '" + model.DepartCode + "' ");
                List<EmrModel> list = new List<EmrModel>();
                if (m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Tag is EmrModelContainer)
                {
                    foreach (TreeListNode node in m_CurrentTreeListNode.ParentNode.Nodes)
                    {
                        EmrModel modelevey = (EmrModel)node.Tag;
                        if (modelevey.DepartCode != model.DepartCode)
                        {
                            continue;
                        }
                        //获取数据库中所有有效首程(当前首程除外)
                        DataRow[] allFirstValidDailys = allRecords.Select(" firstdailyflag='1' and valid = 1 and ID <> " + model.InstanceId);
                        if (null == allFirstValidDailys || allFirstValidDailys.Count() == 0)
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        if (null != starttime && null != endtime && modelevey.DisplayTime >= starttime && modelevey.DisplayTime <= endtime)
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        else if (null != starttime && null == endtime && modelevey.DisplayTime >= starttime)
                        {
                            list.Add(modelevey);
                            continue;
                        }
                        if (null == starttime && null != endtime && modelevey.DisplayTime <= endtime)
                        {
                            list.Add(modelevey);
                            continue;
                        }

                        //下一个本科室有效首次病程
                        DataRow nextThisFirstEmr = thisFirstDailys.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > model.DisplayTime).OrderBy(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                        //最后一个本科室有效首次病程
                        DataRow lastThisFirstEmr = thisFirstDailys.OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                        if (null == lastThisFirstEmr)
                        {
                            continue;
                        }
                        DateTime endEmrDatetime = DateTime.Parse(lastThisFirstEmr["captiondatetime"].ToString());

                        if (null == nextThisFirstEmr && modelevey.DisplayTime >= DateTime.Parse(lastThisFirstEmr["captiondatetime"].ToString()))
                        {
                            list.Add(modelevey);
                        }
                        else if (modelevey.DisplayTime >= model.DisplayTime && modelevey.DisplayTime < DateTime.Parse(nextThisFirstEmr["captiondatetime"].ToString()))
                        {
                            list.Add(modelevey);
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
        /// 获取页面所有病历节点
        /// 注：只针对病程记录
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-17</date>
        /// </summary>
        /// <returns></returns>
        private List<EmrModel> GetAllRecordModelNodes()
        {
            try
            {
                List<EmrModel> list = new List<EmrModel>();

                if (null != m_CurrentTreeListNode && null != m_CurrentTreeListNode.ParentNode && m_CurrentTreeListNode.ParentNode.Tag is EmrModelContainer)
                {
                    if (m_CurrentTreeListNode.ParentNode.GetValue("colName").ToString() == "病程记录")
                    {
                        foreach (TreeListNode node in m_CurrentTreeListNode.ParentNode.Nodes)
                        {
                            if (null != node.Tag && node.Tag is EmrModel)
                            {
                                EmrModel modelevey = (EmrModel)node.Tag;
                                list.Add(modelevey);
                            }
                        }
                    }
                }
                if (null != list && list.Count > 0)
                {
                    return list;
                }

                TreeListNodes treeNodes = null;
                if (null != treeList1.FocusedNode)
                {
                    if (treeList1.FocusedNode.GetValue("colName").ToString() == "病程记录")
                    {
                        treeNodes = treeList1.FocusedNode.Nodes;
                    }
                    else if (null != treeList1.FocusedNode.ParentNode && treeList1.FocusedNode.ParentNode.GetValue("colName").ToString() == "病程记录")
                    {
                        treeNodes = treeList1.FocusedNode.ParentNode.Nodes;
                    }
                    else if (null != treeList1.FocusedNode.Nodes && treeList1.FocusedNode.Nodes.Count > 0)
                    {
                        foreach (TreeListNode child in treeList1.FocusedNode.Nodes)
                        {
                            if (child.GetValue("colName").ToString() == "病程记录")
                            {
                                treeNodes = child.Nodes;
                                break;
                            }
                        }
                    }
                }
                else if (null != treeList1.Nodes && treeList1.Nodes.Count > 0)
                {
                    foreach (TreeListNode subNode in treeList1.Nodes)
                    {
                        if (subNode.GetValue("colName").ToString() == "病程记录")
                        {
                            treeNodes = subNode.Nodes;
                            break;
                        }
                    }
                }

                if (null != treeNodes && treeNodes.Count > 0)
                {
                    foreach (TreeListNode node in treeNodes)
                    {
                        if (null != node.Tag && node.Tag is EmrModel)
                        {
                            EmrModel modelevey = (EmrModel)node.Tag;
                            list.Add(modelevey);
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
        /// 获取页面所有病历节点
        /// 注：只针对所有记录(包含住院志、病程记录......)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-27</date>
        /// </summary>
        /// <returns></returns>
        private List<EmrModel> GetAllRecordModels(TreeListNodes nodes, List<EmrModel> list)
        {
            try
            {
                if (null == nodes || nodes.Count == 0 || null == list)
                {
                    return list;
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
                throw new Exception(ex.Message);
            }
        }

        #region 已弃用 by cyq 2013-01-05 不满足转科使用
        /// <summary>
        /// 判断首次病程是否已存在
        /// 即：数据库中是否已存在同一科室和病区的首程(该病人) --- 首程唯一性
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="index">首程ID</param>
        /// <param name="noofinpat">住院号</param>
        /// <returns></returns>
        //private bool ExistTheFirstRecord(string index, string noofinpat)
        //{
        //    try
        //    {
        //        bool boo = false;
        //        string sql1 = string.Format(@"select * from inpatient where noofinpat='{0}' ", noofinpat);
        //        DataTable dt1 = sqlHelper.ExecuteDataTable(sql1, CommandType.Text);
        //        if (null != dt1 && dt1.Rows.Count > 0)
        //        {
        //            string dept = null == dt1.Rows[0]["outhosdept"] ? "" : dt1.Rows[0]["outhosdept"].ToString();
        //            string sql2 = string.Format(@"select * from recorddetail where noofinpat='{0}' and valid='1' and sortid='AC' and firstdailyflag='1' ", noofinpat);
        //            if (!string.IsNullOrEmpty(dept))
        //            {
        //                sql2 += @" and departcode = '" + dept + "' ";
        //            }
        //            sql2 += @" and ID != " + (string.IsNullOrEmpty(index) ? -1 : int.Parse(index));
        //            sql2 += @" order by captiondatetime desc ";
        //            DataTable dt2 = sqlHelper.ExecuteDataTable(sql2, CommandType.Text);
        //            if (null != dt2 && dt2.Rows.Count > 0)
        //            {
        //                string captiondatetime = null == dt2.Rows[0]["captiondatetime"] ? "" : dt2.Rows[0]["captiondatetime"].ToString();
        //                DataTable records = YD_SqlService.GetRecordsByTimeDiv(string.IsNullOrEmpty(noofinpat) ? -1 : int.Parse(noofinpat), DateTime.Parse(captiondatetime), DateTime.Now);
        //                var recordRows = records.Select(" 1=1 ").Where(p => null != p["departcode"] && !string.IsNullOrEmpty(p["departcode"].ToString().Trim()));
        //                if (!string.IsNullOrEmpty(dept.Trim()) && !recordRows.Any(p => p["departcode"].ToString() != dept))
        //                {
        //                    boo = true;
        //                }
        //            }
        //        }

        //        return boo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
        #endregion

        /// <summary>
        /// 判断首次病程是否已存在
        /// 即：数据库中是否已存在同一科室和病区的首程(该病人) --- 首程唯一性
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="model">首程</param>
        /// <param name="noofinpat">住院号</param>
        /// <returns></returns>
        private bool ExistTheFirstRecord(EmrModel model, int noofinpat)
        {

            try
            {
                //查询该病人信息
                DataTable dt1 = DS_SqlService.GetInpatientByID(noofinpat, 2);
                if (null == dt1 || dt1.Rows.Count == 0)
                {
                    return false;
                }
                //病人所在科室
                string dept = null == dt1.Rows[0]["outhosdept"] ? "" : dt1.Rows[0]["outhosdept"].ToString();
                //获取该病人所有病历(包含无效)
                DataTable allRecords = DS_SqlService.GetRecordsByNoofinpatContainDel(noofinpat);
                //获取数据库中所有首程(包含无效,非当前首程) --- 针对A->B->A之后删除B所有病历的情况
                DataRow[] allFirstDailys = allRecords.Select(" firstdailyflag='1' and ID <> " + model.InstanceId);
                if (null != allFirstDailys && allFirstDailys.Count() > 0)
                {
                    DateTime captiondatetime = model.DisplayTime;
                    //获取其它科室的所有首程(包含无效)
                    var otherFirstEmrs = allFirstDailys.Where(p => p["departcode"].ToString() != model.DepartCode.Trim());
                    //获取本科室的所有有效首程
                    var thisFirstEmrs = allFirstDailys.Where(p => p["departcode"].ToString() == model.DepartCode.Trim() && int.Parse(p["valid"].ToString()) == 1);
                    //上一个本科室的有效首程
                    var preFirstThisEmr = thisFirstEmrs.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < captiondatetime).OrderByDescending(p => DateTime.Parse(p["captiondatetime"].ToString())).FirstOrDefault();
                    //下一个本科室的有效首程
                    var nextFirsThisEmr = thisFirstEmrs.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > captiondatetime).OrderBy(p => DateTime.Parse(p["captiondatetime"].ToString())).FirstOrDefault();
                    if (null != preFirstThisEmr && !otherFirstEmrs.Any(p => DateTime.Parse(p["captiondatetime"].ToString()) > DateTime.Parse(preFirstThisEmr["captiondatetime"].ToString()) && DateTime.Parse(p["captiondatetime"].ToString()) < captiondatetime))
                    {
                        return true;
                    }
                    else if (null != nextFirsThisEmr && !otherFirstEmrs.Any(p => DateTime.Parse(p["captiondatetime"].ToString()) < DateTime.Parse(nextFirsThisEmr["captiondatetime"].ToString()) && DateTime.Parse(p["captiondatetime"].ToString()) > captiondatetime))
                    {
                        return true;
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
        /// 判断首次病程是否已存在
        /// 即：数据库中是否已存在同一科室和病区的首程(该病人) --- 首程唯一性
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-11-20</date>
        /// <param name="model">首程</param>
        /// <param name="noofinpat">住院号</param>
        /// <returns></returns>
        private bool ExistTheFirstRecordInTran(EmrModel model, int noofinpat)
        {
            #region 已注释 by cyq 2013-01-22 08:58 修复A->b->A后删除B科室所有病历的bug
            /**
            try
            {
                if (null == model || string.IsNullOrEmpty(noofinpat))
                {
                    return false;
                }
                //查询该病人信息
                string sql1 = string.Format(@"select * from inpatient where noofinpat='{0}' ", noofinpat);
                DataTable dt1 = sqlHelper.ExecuteDataTableInTran(sql1, CommandType.Text);
                if (null == dt1 || dt1.Rows.Count == 0)
                {
                    return false;
                }
                //病人所在科室
                string dept = null == dt1.Rows[0]["outhosdept"] ? "" : dt1.Rows[0]["outhosdept"].ToString();
                //查询数据库中所有首程
                string sql2 = string.Format(@"select * from recorddetail where noofinpat='{0}' and valid='1' and sortid='AC' and firstdailyflag='1' ", noofinpat);
                sql2 += @" and ID != " + model.InstanceId;
                sql2 += @" order by captiondatetime ";
                DataTable dt2 = sqlHelper.ExecuteDataTableInTran(sql2, CommandType.Text);
                if (null != dt2 && dt2.Rows.Count > 0)
                {
                    DateTime captiondatetime = model.DisplayTime;
                    var preFirstEmr = dt2.Select(" captiondatetime < '" + captiondatetime.ToString("yyyy-MM-dd HH:mm:ss") + "' ").OrderByDescending(p => p["captiondatetime"]).FirstOrDefault();
                    var nextFirstEmr = dt2.Select(" captiondatetime > '" + captiondatetime.ToString("yyyy-MM-dd HH:mm:ss") + "' ").OrderBy(p => p["captiondatetime"]).FirstOrDefault();
                    if ((null != preFirstEmr && preFirstEmr["departcode"].ToString() == dept) || (null != nextFirstEmr && nextFirstEmr["departcode"].ToString() == dept))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            **/
            #endregion

            try
            {
                //查询该病人信息
                DataTable dt1 = DS_SqlService.GetInpatientByIDInTran(noofinpat);
                if (null == dt1 || dt1.Rows.Count == 0)
                {
                    return false;
                }
                //病人所在科室
                string dept = null == dt1.Rows[0]["outhosdept"] ? "" : dt1.Rows[0]["outhosdept"].ToString();
                //获取该病人所有病历(包含无效)
                DataTable allRecords = DS_SqlService.GetRecordsByNoofinpatContainDelInTran(noofinpat);
                //获取数据库中所有首程(包含无效,非当前首程) --- 针对A->B->A之后删除B所有病历的情况
                DataRow[] allFirstDailys = allRecords.Select(" firstdailyflag='1' and ID <> " + model.InstanceId);
                if (null != allFirstDailys && allFirstDailys.Count() > 0)
                {
                    DateTime captiondatetime = model.DisplayTime;
                    //获取其它科室的所有病历(包含无效)
                    var otherEmrs = allRecords.Select(" departcode <> '" + dept + "' ");
                    //获取其它科室的所有首程(包含无效)
                    //var otherFirstEmrs = allFirstDailys.Where(p => p["departcode"].ToString() != model.DepartCode.Trim());
                    //获取本科室的所有有效首程
                    var thisFirstEmrs = allFirstDailys.Where(p => p["departcode"].ToString() == model.DepartCode.Trim() && int.Parse(p["valid"].ToString()) == 1);
                    //上一个本科室的有效首程
                    var preFirstThisEmr = thisFirstEmrs.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < captiondatetime).OrderByDescending(p => DateTime.Parse(p["captiondatetime"].ToString())).FirstOrDefault();
                    //下一个本科室的有效首程
                    var nextFirsThisEmr = thisFirstEmrs.Where(p => DateTime.Parse(p["captiondatetime"].ToString()) > captiondatetime).OrderBy(p => DateTime.Parse(p["captiondatetime"].ToString())).FirstOrDefault();
                    if (null != preFirstThisEmr && !otherEmrs.Any(p => DateTime.Parse(p["captiondatetime"].ToString()) > DateTime.Parse(preFirstThisEmr["captiondatetime"].ToString()) && DateTime.Parse(p["captiondatetime"].ToString()) < captiondatetime))
                    {
                        return true;
                    }
                    else if (null != nextFirsThisEmr && !otherEmrs.Any(p => DateTime.Parse(p["captiondatetime"].ToString()) < DateTime.Parse(nextFirsThisEmr["captiondatetime"].ToString()) && DateTime.Parse(p["captiondatetime"].ToString()) > captiondatetime))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除事件
        /// </summary>
        /// /// edit by Yanqiao.Cai 2013-01-16
        /// 1、add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDeletePopup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DeleteDocumentPublic();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// /// edit by Yanqiao.Cai 2013-01-16
        /// 1、add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DeleteDocumentPublic();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除方法
        /// </summary>
        /// /// edit by Yanqiao.Cai 2013-01-16
        /// 1、add try ... catch
        public void DeleteDocumentPublic()
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    if (!CanDelete())
                    {
                        return;
                    }
                    //add by cyq 2013-01-16 精确提示
                    string recordTypeName = "病历";
                    TreeListNode node = this.treeList1.FocusedNode;
                    if (null != node && null != node.ParentNode && null != node.ParentNode.Tag && node.ParentNode.Tag is EmrModelContainer)
                    {
                        EmrModelContainer typeModel = node.ParentNode.Tag as EmrModelContainer;
                        recordTypeName = null == typeModel ? "病程记录" : typeModel.Name;
                    }

                    if (MyMessageBox.Show("您确定要删除该" + recordTypeName + "吗？", "提示信息", MyMessageBoxButtons.YesNo, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Yes)
                    {
                        DeleteTemplate();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 判断病历是否可以删除
        /// </summary>
        private bool CanDelete()
        {
            try
            {
                TreeListNode node = this.treeList1.FocusedNode;
                if (node != null && node.Tag != null)
                {
                    EmrModel model = node.Tag as EmrModel;
                    if (model != null)
                    {
                        if (model.CreatorXH != DS_Common.currentUser.Id)
                        {
                            string userStr = DS_BaseService.GetUserNameAndID(model.CreatorXH);

                            string recordTypeName = "病历";
                            if (null != node.ParentNode && null != node.ParentNode.Tag && node.ParentNode.Tag is EmrModelContainer)
                            {
                                EmrModelContainer typeModel = node.ParentNode.Tag as EmrModelContainer;
                                recordTypeName = null == typeModel ? "病程记录" : typeModel.Name;
                            }
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该" + recordTypeName + "为 " + userStr + " 创建，您没有权限删除。");
                            return false;
                        }

                        //edit by cyq 2013-01-16 修复转科之后首程(首程下有病历)可删除的bug
                        #region 已注释 edit by cyq 2013-01-16
                        //if (model.ModelCatalog == "AC"
                        //    && model.FirstDailyEmrModel
                        //    && node.ParentNode.Nodes[0] == node
                        //    && node.ParentNode.Nodes.Count > 1)  //默认第一份病程为首程 且只
                        //{
                        //    m_app.CustomMessageBox.MessageShow("首次病程不能删除", CustomMessageBoxKind.ErrorOk);
                        //    return false;
                        //}
                        //else
                        //{
                        //    return true;
                        //}
                        #endregion
                        if (model.ModelCatalog == "AC" && model.FirstDailyEmrModel && CheckExsitRecordInFirstDaily(model))
                        {
                            //该首程下存在病历
                            DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该首次病程下存在病程记录，不能删除。");
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void DeleteTemplate()
        {
            try
            {
                #region Delete
                //if ((treeList1.FocusedNode == null) || (treeList1.FocusedNode.Tag == null)) return;
                //if (treeList1.FocusedNode.Tag is EmrModel)
                //{
                //    EmrModel model = (EmrModel)treeList1.FocusedNode.Tag;
                //    if (model.InstanceId < 0)//新增节点直接删除
                //    {
                //        //编辑器内容切换至上一个节点内容
                //        //判断是否要切换
                //        if (model.DailyEmrModel)
                //        {
                //        }
                //    }
                //    {
                //        model.Valid = false;
                //        m_RecordDal.UpdateModelInstance(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                //        //判断是否需要切换
                //    }
                //    treeList1.DeleteNode(treeList1.FocusedNode);
                //}
                #endregion

                if (m_CurrentModel != null)
                {
                    EmrModel emrModel = m_CurrentModel;
                    if (m_CurrentModel.DailyEmrModel)
                    {
                        emrModel = treeList1.FocusedNode.Tag as EmrModel;
                        if (emrModel == null) return;
                    }

                    if (emrModel.DailyEmrModel)
                    {
                        CurrentForm.DeleteDailyEmr(emrModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        m_CurrentModel.ModelContent.PreserveWhitespace = true;
                        m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                    }

                    if (emrModel.InstanceId < 0)
                    {
                        //新增节点直接删除  先关闭TabPage，再删除树节点
                        DeleteTabPageAndNode(emrModel);
                    }
                    else
                    {
                        //创建事务
                        DS_SqlHelper.CreateSqlHelper();
                        DS_SqlHelper.BeginTransaction();

                        //节点直接删除  1.关闭TabPage，2.删除树节点【作废RecordDetail中对应的一条记录】
                        DeleteTabPageAndNode(emrModel);
                        //从数据库中逻辑删除节点
                        m_RecordDal.CancelEmrModelInTran(emrModel.InstanceId);

                        if (m_CurrentModel != null && m_CurrentModel.ModelCatalog == ContainerCatalog.BingChengJiLu)
                        {
                            m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                            CurrentForm.zyEditorControl1.EMRDoc.Modified = false;
                            //保存病历内容
                            SaveDocumentInnerInTran(m_CurrentModel);
                        }
                        DS_SqlHelper.CommitTransaction();
                    }

                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");
                    SetDailyEmrEditArea();
                    treeList1.Refresh();

                    //this.InsertOperRecordLog(emrModel, "3");//add by wyt
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查该首程下是否存在病历
        /// </summary>
        /// 注：1、从数据库中验证；2、从页面节点中验证(包含未保存的节点)
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-14</date>
        /// <param name="model">首次病程</param>
        /// <returns></returns>
        private bool CheckExsitRecordInFirstDaily(EmrModel model)
        {
            try
            {
                if (null == model || !model.FirstDailyEmrModel)
                {
                    return false;
                }
                ///1、判断数据库中是否存在
                string content = DS_SqlService.GetRecordContentsByID(model.InstanceId);
                if (!string.IsNullOrEmpty(content.Trim()))
                {
                    XmlDocument xmlRecord = new XmlDocument();
                    xmlRecord.PreserveWhitespace = true;
                    xmlRecord.LoadXml(content);
                    List<string> timeList = DS_BaseService.GetEditEmrContentTimes(xmlRecord).Where(p => p != model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss")).ToList();
                    if (null != timeList || timeList.Count() > 0)
                    {
                        string sqlTimeStr = DS_Common.CombineSQLStringByList(timeList);
                        if (!string.IsNullOrEmpty(sqlTimeStr))
                        {
                            DataTable dt = DS_SqlService.GetRecordByDateTimes((int)m_app.CurrentPatientInfo.NoOfFirstPage, sqlTimeStr);
                            if (null != dt && dt.Rows.Count > 0 && dt.Select(" 1=1 ").Any(p => int.Parse(p["ID"].ToString()) != model.InstanceId))
                            {
                                return true;
                            }
                        }
                    }
                }
                ///2、判断页面节点中是否存在(包含未保存的节点)
                List<EmrModel> list = new List<EmrModel>();
                if (null != m_CurrentTreeListNode && null != m_CurrentTreeListNode.ParentNode)
                {
                    foreach (TreeListNode node in m_CurrentTreeListNode.ParentNode.Nodes)
                    {
                        if (null != node && null != node.Tag && node.Tag is EmrModel)
                        {
                            EmrModel em = node.Tag as EmrModel;
                            if (em.DisplayTime >= model.DisplayTime)
                            {
                                list.Add(em);
                            }
                        }
                    }
                    //该节点下方所有节点(不包含该节点)
                    var nextNodes = list.Where(p => p.DisplayTime > model.DisplayTime).OrderBy(q => q.DisplayTime);
                    if (null == nextNodes || nextNodes.Count() == 0)
                    {
                        return false;
                    }
                    if (!nextNodes.Any(p => p.DepartCode != model.DepartCode))
                    {
                        return true;
                    }
                    EmrModel otherEmrModel = nextNodes.FirstOrDefault(p => p.DepartCode != model.DepartCode);
                    if (null != otherEmrModel)
                    {
                        if (nextNodes.Any(p => p.DisplayTime > model.DisplayTime && p.DisplayTime < otherEmrModel.DisplayTime && p.DepartCode == model.DepartCode))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 另存
        private void btn_SaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string contentXml = CurrentForm.zyEditorControl1.ToXMLString();
                SaveAsForm saveAsForm = new SaveAsForm(m_CurrentModel, m_app, m_RecordDal, contentXml);
                saveAsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 提交

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSubmitPopup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    SubmitDocment();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Submit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    SubmitDocment();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 供PadForm类调用提交功能
        /// </summary>
        public void SubmitDocumentPublic()
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    SubmitDocment();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SubmitDocment()
        {
            try
            {
                if (!IsOpenThreeLevelAudit())
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("病历提交功能暂未开放");
                    return;
                }

                if (treeList1.FocusedNode.Tag is EmrModel)
                {
                    EmrModel model = treeList1.FocusedNode.Tag as EmrModel;

                    if (model.DailyEmrModel)//病程提交
                    {
                        SubmitDocmentInner(model);
                    }
                    else//除病程以外的病历提交
                    {
                        SubmitDocmentInner(m_CurrentModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SubmitDocmentInner(EmrModel model)
        {
            if (model == null)
                return;
            try
            {
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("确定要提交 " + model.ModelName + " 吗？", "提示信息",
                    DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                if (DoctorEmployee.Grade.Trim() == "") return;

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

                ////自动签名 todo

                // 借用阅改时间属性来保存最近一次的修改时间
                model.ExamineTime = DateTime.Now;
                model.ExaminerXH = m_app.User.Id;
                model.Valid = true;
                model.IsModified = true;
                //model.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                Save(model);
                SetDocmentReadOnlyMode();

                CurrentForm.zyEditorControl1.ActiveEditArea = null;
                CurrentForm.zyEditorControl1.Refresh();
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("提交病历成功，病历不可再编辑。");
                ResetEditModeAction(model);
                GetStateImage();
                CurrentForm.zyEditorControl1.EMRDoc.Modified = false;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("提交病历时发生异常" + ex.Message + "");
            }
        }
        #endregion

        #region 新增

        /// <summary>
        /// 新增事件 - 模板
        /// </summary>
        /// edit by Yanqiao.Cai 2013-01-18
        /// 1、add try ... catch
        /// 2、方法封装
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnModulePopup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                AddRecord();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增事件 - 按钮
        /// </summary>
        /// edit by Yanqiao.Cai 2013-01-18
        /// 1、add try ... catch
        /// 2、方法封装
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_new_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                AddRecord();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增方法
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-18</date>
        /// </summary>
        private void AddRecord()
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
                if ((node == null) || (node.Tag == null) || (node.Tag is EmrModel))
                {
                    return;
                }

                //转科(非当前登录科室)后限制新增 add by cyq 2013-01-23
                string memo = CheckIfCurrentDept((int)m_app.CurrentPatientInfo.NoOfFirstPage);
                if (!string.IsNullOrEmpty(memo))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(memo);
                    return;
                }

                //检查页面首程记录数与数据库有效首程记录数是否一致
                if (node.GetValue("colName").ToString() == "病程记录" && !CheckFirstDailys((int)m_app.CurrentPatientInfo.NoOfFirstPage))
                {
                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人的病程记录已更新，请刷新数据后重试。您是否要刷新病程记录？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    ReFreshRecordData();
                    return;
                }

                string oldcatelog = MyContainerCode;
                AddEmrModel(node);
                //add by cyq 2012-09-29 科室小模板数据刷新
                if (oldcatelog != MyContainerCode)
                {
                    InitPersonTree(false, MyContainerCode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 增加病历
        /// </summary>
        /// <param name="node">TreeList.FocusedNode</param>
        private void AddEmrModel(TreeListNode node)
        {
            try
            {
                EmrModelContainer container = (EmrModelContainer)node.Tag;
                //添加病历相关文件
                if ((container.EmrContainerType != ContainerType.None) && (!string.IsNullOrEmpty(container.ContainerCatalog)))
                {
                    string catalog = container.ContainerCatalog;
                    EmrModel model = GetTemplate(catalog, node);
                    if (model != null)
                    {
                        model.Description = model.ModelName;
                        model.DefaultModelName = model.ModelName;
                        model.ModelName = model.ModelName + " " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + DoctorEmployee.Name;

                        //首次病程 startnew为true，否则为false
                        //bool startnew = (catalog.Equals("AB") || (!node.HasChildren));
                        bool startnew = catalog.Equals(ContainerCatalog.BingChengJiLu) && (!node.HasChildren);

                        bool isOK = AddNewPatRec(model, startnew);
                        if (isOK)
                        {
                            container.AddModel(model);
                            //m_CurrentModel
                        }
                    }
                    else
                    {
                        if (CurrentForm != null && CurrentForm.Controls.Count > 0)
                        {
                            XtraTabPage page = xtraTabControl1.SelectedTabPage;
                            SelectedPageChanged(page);
                        }
                    }
                    //add by cyq 2012-09-29 右侧科室小模板刷新
                    MyContainerCode = catalog;
                }

                #region 处理留痕(新增时是询问是否此次留痕，其他编辑时则是开始留痕操作 ) add by 杨伟康 2012年12月25日13:53:14
                //上面是将模板加载完毕
                //bool isopenThreeLevelAudit;
                //string isopenOwnerSign = string.Empty;//是否开放了个人书写留痕功能
                //isopenOwnerSign = YD_SqlService.GetConfigValueByKey("IsOpenOwnerSign");
                ////UCEmrInput UC1 = new UCEmrInput();
                //isopenThreeLevelAudit = IsOpenThreeLevelAudit();
                //if (isopenThreeLevelAudit == true && isopenOwnerSign == "1")
                //{
                //    Common.Ctrs.DLG.MessageBox.Show("对不起，三级检诊功能和设置个人修改痕迹不可同时开放");
                //    return;
                //}
                ////开放了修改个人痕迹显示的功能,走下面流程
                //if (isopenOwnerSign == "1")
                //{
                //    this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Read;
                //    this.CurrentForm.zyEditorControl1.EMRDoc.Info.LogicDelete = true;
                //    this.CurrentForm.zyEditorControl1.EMRDoc.Info.AutoLogicDelete = true;
                //    this.CurrentForm.zyEditorControl1.EMRDoc.Info.ShowMark = true;
                //    this.CurrentForm.zyEditorControl1.EMRDoc.Info.ShowAll = true;
                //    //zyEditorControl1.EMRDoc.Info.ShowMark = true;
                //    //zyEditorControl1.InsertMode = false;//记录修改痕迹
                //    this.CurrentForm.zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.UserName = m_app.User.Name; //这里传入用户名
                //    //todo 针对性设置修改痕迹


                //    if (MessageBox.Show("您确认此时并开启留痕吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                //    {
                //        this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                //        this.CurrentForm.zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.Level = 1;         //这里传入用户级别
                //        this.CurrentForm.zyEditorControl1.EMRDoc.Content.UserLevel = 1;                     //这里还是同样的用户级别
                //    }
                //    else
                //    {
                //        this.CurrentForm.zyEditorControl1.EMRDoc.SaveLogs.CurrentSaveLog.Level = 0;         //这里传入用户级别
                //        this.CurrentForm.zyEditorControl1.EMRDoc.Content.UserLevel = 0;                     //这里还是同样的用户级别
                //    }

                //}
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ////记录操作记录 --add by wyt 2012-08-20
        //private void InsertOperRecordLog(EmrModel model, string oper_id)
        //{
        //    SqlParameter[] sqlparams = new SqlParameter[]
        //                {
        //                    new SqlParameter("@USER_ID",SqlDbType.VarChar),
        //                    new SqlParameter("@USER_NAME",SqlDbType.VarChar),
        //                    new SqlParameter("@PATIENT_ID",SqlDbType.VarChar),
        //                    new SqlParameter("@PATIENT_NAME",SqlDbType.VarChar),
        //                    new SqlParameter("@DEPT_ID",SqlDbType.VarChar),
        //                    new SqlParameter("@RECORD_ID",SqlDbType.VarChar),
        //                    new SqlParameter("@RECORD_TYPE",SqlDbType.VarChar),
        //                    new SqlParameter("@RECORD_TITLE",SqlDbType.VarChar),
        //                    new SqlParameter("@OPER_ID",SqlDbType.VarChar),
        //                    new SqlParameter("@OPER_TIME",SqlDbType.VarChar),
        //                    new SqlParameter("@OPER_CONTENT",SqlDbType.Text)
        //                };
        //    sqlparams[0].Value = m_app.User.Id;
        //    sqlparams[1].Value = m_app.User.Name;
        //    sqlparams[2].Value = m_app.CurrentPatientInfo.NoOfFirstPage;
        //    sqlparams[3].Value = m_app.CurrentPatientInfo.Name;
        //    sqlparams[4].Value = m_app.User.CurrentDeptId;
        //    sqlparams[5].Value = model.InstanceId;
        //    sqlparams[6].Value = model.ModelCatalog;
        //    sqlparams[7].Value = model.ModelName;
        //    sqlparams[8].Value = oper_id;
        //    sqlparams[9].Value = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    sqlparams[10].Value = "";
        //    m_app.SqlHelper.ExecuteNoneQuery("EMR_RECORD_INPUT.usp_insertoperrecordlog", sqlparams, CommandType.StoredProcedure);
        //}

        private bool AddNewPatRec(EmrModel model, bool startNew)
        {
            try
            {
                TreeListNode node = treeList1.AppendNode(new object[] { model.ModelName }, treeList1.FocusedNode);
                treeList1.FocusedNode.ExpandAll();
                node.Tag = model;

                //先不显示新增加的节点，因为在输入病程标题名称和时间的界面能取消病程
                node.Visible = false;

                if (model.DailyEmrModel)//设置病程相关信息
                {
                    bool isAdd = AddDailyEmrModel(model, startNew/*true: 首次病程 false：非首次病程*/, node/*新增的病程节点*/);
                    if (!isAdd)
                    {
                        return false;
                    }
                }
                else //加载非病程类的文件
                {
                    CreateNewDocument(model);
                }

                int index = node.ParentNode.Nodes.IndexOf(node);
                FindInsertIndex(node, model, ref index);
                treeList1.SetNodeIndex(node, index);
                //病历增加结束后将节点显示出来
                node.Visible = true;
                //填充宏
                FillModelMacro(CurrentForm);
                //填充可替换项
                ReplaceModelMacro(model);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void FindInsertIndex(TreeListNode node, EmrModel model, ref int index)
        {
            try
            {
                if ((node.PrevNode != null) && (((EmrModel)node.PrevNode.Tag)).DisplayTime > model.DisplayTime)
                {
                    //find index
                    foreach (TreeListNode nd in node.ParentNode.Nodes)
                    {
                        //匹配时间
                        EmrModel prevModel = (EmrModel)nd.Tag;
                        if (prevModel.DisplayTime < model.DisplayTime)
                        {
                            if (nd.NextNode == null)
                            {
                                index = node.ParentNode.Nodes.IndexOf(nd) + 1;
                                break;
                            }
                            else
                            {
                                //配置后续节点时间
                                EmrModel nextModel = (EmrModel)nd.NextNode.Tag;
                                if (nextModel.DisplayTime > model.DisplayTime)
                                {
                                    index = node.ParentNode.Nodes.IndexOf(nd) + 1;
                                }
                            }
                        }
                    }

                    node.SetValue("colName", model.ModelName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 新增病程
        /// </summary>
        /// <param name="model">病程</param>
        /// <param name="startNew">第一个病程【首程】</param>
        /// <param name="node">新增的病程对应的树节点</param>
        /// <returns>true：创建成功 false：取消创建、创建失败 </returns>
        private bool AddDailyEmrModel(EmrModel model, bool startNew, TreeListNode node)
        {
            try
            {
                if (model.DailyEmrModel)//保证是病程
                {
                    //默认病程时间为系统当前时间
                    DateTime dt = DateTime.Now;

                    //设置病程的标题和时间等信息
                    if (!ProcessDailyTemplateForm(model, node))
                    {
                        //取消对病程的设置，则取消新病程的添加
                        DeleteNodeNotTriggerEvent(node);
                        if (CurrentForm != null && CurrentForm.Controls.Count > 0)
                        {
                            XtraTabPage page = xtraTabControl1.SelectedTabPage;
                            SelectedPageChanged(page);
                        }
                        return false;
                    }

                    if (model.FirstDailyEmrModel)//新科室的第一个病程
                    {
                        CreateNewDocument(model);
                    }
                    else//除首程外的其他病程
                    {
                        //点击“病程”文件夹时TabPage显示默认的病程【首程】
                        SetDefaultDailyEmrModelNode(node);

                        //将新增的病程插入到当前编辑器中
                        InsertEmrModelContent(CurrentForm, model);

                        //清空除首程以外的病程的内容，因为所有的病程数据都是保存在首次病程中，其他的病程是通过时间在首次病程中定位。
                        model.ModelContent = new XmlDocument();
                    }

                    //首程得到编辑器界面所有的内容
                    m_CurrentModel.ModelContent.PreserveWhitespace = true;
                    XmlDocument content = new XmlDocument();
                    CurrentForm.zyEditorControl1.EMRDoc.ToXMLDocument(content);
                    m_CurrentModel.ModelContent = content;

                    //选中新增的病程，并在TabPage的标题上显示病程的名字
                    SetDailyEmrDoc(node);

                    ResetEditModeAction(model);

                    //设置编辑区域
                    SetDailyEmrEditArea(node);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// 对新病程的设置  xll
        /// </summary>
        /// <param name="model"></param>
        /// <param name="node">新增的病程对应的树节点</param>
        /// <param name="patRecTime">病程时间</param>
        /// <returns>是否取消对病程的设置  true：病程设置成功  false：取消对病程设置，则取消这次病程的新增操作 </returns>
        private bool ProcessDailyTemplateForm(EmrModel model, TreeListNode node)
        {
            try
            {
                if (null == model)
                {
                    return false;
                }

                string titleName = model.IsShowFileName == "1" ? (string.IsNullOrEmpty(model.FileName.Trim()) ? model.Description.Trim() : model.FileName.Trim()) : "";

                EmrModel firstDailyEmrModelmy = GetFirstDailyEmrModel(node.ParentNode.Nodes);
                DateTime dt = DateTime.Now.AddYears(-200);  //初始时间设到很前
                if (firstDailyEmrModelmy != null && firstDailyEmrModelmy.FirstDailyEmrModel && firstDailyEmrModelmy.DisplayTime != DateTime.MinValue)//有首次病程
                {
                    dt = Convert.ToDateTime(firstDailyEmrModelmy.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"));
                }

                using (DailyTemplateForm daily = new DailyTemplateForm(dt, titleName, model.FirstDailyEmrModel, GetAllRecordModelNodes(), int.Parse(m_CurrentInpatient.NoOfFirstPage.ToString())))
                {
                    //edit by cyq 2013-01-08
                    //daily.SetAllDisplayDateTime(CurrentForm);
                    daily.SetAllDisplayDateTime(node);

                    if (daily.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        model.DisplayTime = daily.CommitDateTime;
                        model.ModelName = model.Description + " " + model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " " + DoctorEmployee.Name;
                        model.Description = daily.CommitTitle;

                        //Add By wwj 2012-02-14 更加设置的时间动态改变树节点的名称
                        node.SetValue("colName", model.ModelName);

                        //edit by cyq 2013-01-24 修复关于转科另起一页
                        //EmrModel firstDailyEmrModel = GetFirstDailyEmrModel(node.ParentNode.Nodes, model);
                        model.FirstDailyEmrModel = IfNewTabPage(node.ParentNode.Nodes, model);

                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 新增病历时是否新开一个tab页
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="nodes">页面树节点</param>
        /// <returns></returns>
        private bool IfNewTabPage(TreeListNodes nodes, EmrModel theModel)
        {
            try
            {
                List<EmrModel> emrModelList = new List<EmrModel>();
                if (null != nodes && nodes.Count > 0)
                {
                    foreach (TreeListNode item in nodes)
                    {
                        if (item.Tag is EmrModel && item.Visible == true)
                        {
                            emrModelList.Add(item.Tag as EmrModel);
                        }
                    }
                }
                if (emrModelList.Count == 0 || null == theModel)
                {
                    return true;
                }

                EmrModel preModel = emrModelList.Where(p => p.DisplayTime < theModel.DisplayTime).OrderByDescending(q => q.DisplayTime).FirstOrDefault();
                EmrModel nextModel = emrModelList.Where(p => p.DisplayTime > theModel.DisplayTime).OrderBy(q => q.DisplayTime).FirstOrDefault();
                if (null == preModel && null == nextModel)
                {
                    return true;
                }
                else if (null == preModel && null != nextModel)
                {//新增节点时间为第一位
                    if (theModel.DepartCode != nextModel.DepartCode)
                    {
                        return true;
                    }
                }
                else if (null != preModel && null == nextModel)
                {//新增节点时间为最后一位
                    if (theModel.DepartCode != preModel.DepartCode)
                    {
                        return true;
                    }
                    else
                    {
                        //获取所有的病历(包含无效)
                        DataTable dt = DS_SqlService.GetRecordsByNoofinpatContainDel((int)m_CurrentInpatient.NoOfFirstPage);
                        if (null != dt && dt.Rows.Count > 0)
                        {
                            EmrModel editPreRecord = emrModelList.Where(p => p.DisplayTime < theModel.DisplayTime).OrderByDescending(q => q.DisplayTime).FirstOrDefault();
                            var dbPreRecord = dt.Select(" 1=1 ").Where(p => DateTime.Parse(p["captiondatetime"].ToString()) < theModel.DisplayTime).OrderByDescending(q => DateTime.Parse(q["captiondatetime"].ToString())).FirstOrDefault();
                            string deptCode = editPreRecord.DepartCode;
                            if (null != dbPreRecord && DateTime.Parse(dbPreRecord["captiondatetime"].ToString()) > editPreRecord.DisplayTime)
                            {
                                deptCode = dbPreRecord["departcode"].ToString().Trim();
                            }
                            if (deptCode != theModel.DepartCode)
                            {
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    if (theModel.DepartCode != preModel.DepartCode && theModel.DepartCode != nextModel.DepartCode)
                    {
                        return true;
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
        /// 检查页面的(非新增)首程记录与数据库中的有效首程记录是否一致
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        private bool CheckFirstDailys(int noofinpat)
        {
            try
            {
                //获取数据库中该病人所有有效首程
                DataTable dt = DS_SqlService.GetFirstRecordsByDept(noofinpat, null);
                //检查页面首程记录数与数据库有效首程记录数是否一致
                List<EmrModel> modelList = GetAllRecordModelNodes();
                if (null != modelList && modelList.Count > 0)
                {
                    //获取页面所有非新增的首程
                    var firstEmrs = modelList.Where(p => p.FirstDailyEmrModel && p.InstanceId != -1);
                    if (null != firstEmrs && firstEmrs.Count() > 0)
                    {
                        if (firstEmrs.Count() != dt.Rows.Count)
                        {
                            return false;
                        }
                        else
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (!firstEmrs.Any(q => q.InstanceId == int.Parse(dr["id"].ToString())))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (null != dt && dt.Rows.Count > 0)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 点击“病程”文件夹时TabPage显示默认的病程【首程】
        /// </summary>
        /// <param name="node">非首次病程的病程节点</param>
        private void SetDefaultDailyEmrModelNode(TreeListNode node)
        {
            try
            {
                TreeListNode parentNode = node.ParentNode;
                if (parentNode.Nodes.Count > 0)//有子节点
                {
                    EmrModel firstModel = GetFirstDailyEmrModel(parentNode.Nodes, node.Tag as EmrModel);
                    if (firstModel != null && firstModel.DailyEmrModel == true)//子节点是病程
                    {
                        bool isNeedActiveEditAreaTemp = m_IsNeedActiveEditArea;
                        m_IsNeedActiveEditArea = false;
                        m_IsLocateDaily = false;

                        EmrModel emrModelFirst = GetFirstDailyEmrModel(parentNode.Nodes, node.Tag as EmrModel);  //科室第一病程
                        foreach (TreeListNode item in parentNode.Nodes)
                        {
                            if (item.Tag is EmrModel && item.Visible == true && emrModelFirst != null)
                            {
                                if ((item.Tag as EmrModel).InstanceId == emrModelFirst.InstanceId)
                                {

                                    treeList1.FocusedNode = item;//触发FocusNode事件 
                                }
                            }
                        }

                        m_IsNeedActiveEditArea = isNeedActiveEditAreaTemp;
                        m_IsLocateDaily = true;
                        FocusNodeNotTriggerEvent(parentNode);
                        CurrentForm.ResetEditModeState(m_CurrentModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 插入病历编辑器的内容
        /// </summary>
        /// <param name="pad"></param>
        /// <param name="model"></param>
        public void InsertEmrModelContent(PadForm pad, EmrModel model)
        {
            try
            {
                //if (model.FirstDailyEmrModel)
                //{
                //    pad.LoadDocment(model);
                //}
                if (model.DailyEmrModel)
                {
                    pad.InsertDailyInfo(model);
                }
                else
                {
                    pad.LoadDocment(model);
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
        /// <param name="catalog"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private EmrModel GetTemplate(string catalog, TreeListNode node)
        {
            try
            {
                //病历模板列表
                DataTable sourceTable = m_patUtil.GetTemplate(catalog, DoctorEmployee.CurrentDept.Code);
                //add by cyq 2013-01-19 第一次添加为首次病程模板，之后为普通模板(只针对病程记录)
                if (null != node && null != node.Nodes && node.Nodes.Count > 0 && catalog == ContainerCatalog.BingChengJiLu)
                {//首次病程
                    //var theList = sourceTable.Select(" isfirstdaily <> 1 ");//此处筛选还是有问题。如果isfirstdaily是空的话也捞取不到
                    //二次修改 ywk 2013年2月22日13:19:02 
                    var theList = sourceTable.Select(" isfirstdaily <> '1' or isfirstdaily is null ");

                    sourceTable = (null == theList || theList.Count() == 0) ? new DataTable() : theList.CopyToDataTable();
                }
                GetActualTemplate(sourceTable);
                //个人模板列表
                DataTable templatePersonTable = m_patUtil.GetTemplatePerson(m_app.User.Id, m_app.User.CurrentDeptId, catalog);
                //病历选择模板界面
                CatalogForm form = new CatalogForm(sourceTable, templatePersonTable, m_RecordDal, DoctorEmployee, m_app, catalog, m_CurrentInpatient);

                if (catalog == ContainerCatalog.BingChengJiLu)//病程
                {
                    if (node.Nodes.Count == 0)
                    {
                        //增加的病历是【首次病程】
                        form.SetFristDailyEmrFlag();
                    }
                    else
                    {
                        //增加的病历是【除首次外的病程】
                        form.SetDailyEmrFlag();
                    }
                }
                //todo 解决第一次插入病程的问题
                if (CurrentForm != null)
                    this.CurrentForm.RemoveGotFocusEvent();
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //if (catalog == ContainerCatalog.BingChengJiLu && node.Nodes.Count > 0)//除首次外的病程
                    //{
                    //    //排除已经指定了以新页开始和医患沟通的病程，因为这些病程已经是以新页开始的
                    //    if (form.CommitModel.IsNewPage == false && form.CommitModel.IsYiHuanGouTong == "1")
                    //    {
                    //        if (m_app.CustomMessageBox.MessageShow("是否换页显示？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    //        { }
                    //    }
                    //}

                    //病历创建人
                    form.CommitModel.CreatorXH = m_app.User.Id;
                    if (m_patUtil == null)
                        m_patUtil = new PatRecUtil(m_app, CurrentInpatient);
                    DataRow dtrowInpatient = m_patUtil.GetInpatient(CurrentInpatient.NoOfFirstPage.ToString());
                    if (dtrowInpatient != null)
                    {
                        form.CommitModel.DepartCode = dtrowInpatient["OUTHOSDEPT"].ToString();
                        form.CommitModel.WardCode = dtrowInpatient["OUTHOSWARD"].ToString();
                    }
                    if (node.Nodes.Count == 0)
                    {
                        //增加的病历是【首次病程】
                        form.CommitModel.RealFirstDaily = true;
                    }
                    return form.CommitModel;
                }
                //todo why?
                //this.CurrentForm.AddGotFocusEvent();
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 排除医患沟通
        /// </summary>
        /// <param name="dt"></param>
        private void GetActualTemplate(DataTable dt)
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
                if (node.Tag is EmrModelContainer)
                {
                    if (node.Nodes.Count > 0)
                    {
                        bool isYihuangoutong = false;
                        for (int i = 0; i < node.Nodes.Count; i++)
                        {
                            EmrModel model = node.Nodes[i].Tag as EmrModel;
                            if (model.IsYiHuanGouTong == "1")//有医患沟通
                            {
                                isYihuangoutong = true;
                                break;
                            }
                        }

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
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 审核

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnConfirmPopup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    AuditDocment();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Audit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    AuditDocment();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 供PadForm类调用审核功能
        /// </summary>
        public void AuditDocumentPublic()
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    AuditDocment();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AuditDocment()
        {
            try
            {
                if (!IsOpenThreeLevelAudit())
                {
                    m_app.CustomMessageBox.MessageShow("病历审核功能未开放");
                    return;
                }

                if (treeList1.FocusedNode.Tag is EmrModel)
                {
                    EmrModel model = treeList1.FocusedNode.Tag as EmrModel;

                    if (model.DailyEmrModel)//病程审核
                    {
                        AuditDocmentInner(model);
                    }
                    else//除病程以外的病历提交
                    {
                        AuditDocmentInner(m_CurrentModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void AuditDocmentInner(EmrModel currentModel)
        {
            try
            {
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("确定要审核 " + currentModel.ModelName + " 吗？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                if (DoctorEmployee.Grade.Trim() == "") return;

                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);

                switch (grade)
                {
                    case DoctorGrade.Attending://住院医师
                        currentModel.State = ExamineState.FirstExamine;
                        break;
                    case DoctorGrade.AssociateChief://副主任医师
                    case DoctorGrade.Chief://主任医师
                        currentModel.State = ExamineState.SecondExamine;
                        break;
                    case DoctorGrade.None:
                    case DoctorGrade.Resident:
                    default:
                        return;
                }


                currentModel.ExaminerXH = m_app.User.Id;
                currentModel.ExamineTime = DateTime.Now;
                // 直接给IsModified赋值，会导致使用“在此之后开始续打”功能时错误的设置打印状态,
                // 所以增加了判断语句（不知是否会引起其它BUG？）
                if (!currentModel.IsModified)
                    currentModel.IsModified = true;
                //currentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                Save(currentModel);

                //副主任医师和主任医师可以连续审核
                if (grade == DoctorGrade.AssociateChief || grade == DoctorGrade.Chief)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("审核病历成功");
                }
                else
                {
                    SetDocmentReadOnlyMode();
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("审核病历成功，病历不可再编辑。");
                }

                CurrentForm.zyEditorControl1.ActiveEditArea = null;
                CurrentForm.zyEditorControl1.Refresh();
                ResetEditModeAction(currentModel);
                GetStateImage();
                CurrentForm.zyEditorControl1.EMRDoc.Modified = false;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("审核病历时发生异常" + ex.Message + "");

            }
            //插入记录

        }
        #endregion

        #region 取消审核
        private void barButtonItemCancelAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    CancelAudit();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem_CancelAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    CancelAudit();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        public void CancelAuditPublic()
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    CancelAudit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CancelAudit()
        {
            try
            {
                if (treeList1.FocusedNode.Tag is EmrModel)
                {
                    EmrModel model = treeList1.FocusedNode.Tag as EmrModel;

                    if (model.DailyEmrModel)//取消病程审核
                    {
                        CancelAuditInner(model);
                    }
                    else//取消除病程以外的病历审核
                    {
                        CancelAuditInner(m_CurrentModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CancelAuditInner(EmrModel currentModel)
        {
            if (null == currentModel)
            {
                return;
            }
            if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("确定要取消审核 " + currentModel.ModelName + " 吗？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            try
            {
                currentModel.State = ExamineState.NotSubmit;
                currentModel.ExaminerXH = m_app.User.Id;
                currentModel.ExamineTime = DateTime.Now;

                if (!currentModel.IsModified)
                    currentModel.IsModified = true;

                //currentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());

                Save(currentModel);
                SetDocmentReadOnlyMode();

                CurrentForm.zyEditorControl1.ActiveEditArea = null;
                CurrentForm.zyEditorControl1.Refresh();
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("取消审核病历成功");
                ResetEditModeAction(currentModel);
                GetStateImage();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("取消审核病历时发生异常" + ex.Message + "");
            }
        }
        #endregion

        #region 自检

        /// <summary>
        /// 自检
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Check_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_CurrentModel != null)
                {
                    //CheckDocment();//以前的自检必点项
                    //add by ywk 2012年9月18日 13:15:33
                    CheckDocBYConfigItem();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 检查一些必填元素是否已经填写
        /// add by ywk 2012年9月18日 13:57:39
        /// </summary>
        private void CheckDocBYConfigItem()
        {
            try
            {
                //传进一段XML，先搜寻要验证的XML节点名称，然后取出紧跟其后的元素，比较两个元素之间的内容是否符合要求
                string mrecord_content = string.Empty;
                int findedcount = 0;//定义变量记录查找到指定标签的存在链表中的索引值
                string nexttip = string.Empty;//记录要查找的标签的下一个标签
                bool IsChecked = false;//是否验证通过
                m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                if (m_CurrentModel == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("病历内容不能为空");
                    return;
                }
                else
                {
                    XmlDocument xmlRecord = new XmlDocument();
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
                    string config = GetConfigValueByKey("CheckDocByConfig");
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
                                int startop = allstr.Replace("　", "").Replace(" ", "").IndexOf(startstr, 0) + startstr.Length; //开始位置
                                int endop = allstr.Replace("　", "").Replace(" ", "").IndexOf(nexttip, startop); //结束位置
                                string my = allstr.Replace("　", "").Replace(" ", "").Substring(startop, endop - startop).Replace("：", ""); //取搜索的条数，用结束的位置-开始的位置,并返回 
                                int checklength = Int32.Parse(m_checkitem[tkeys[k]]);
                                if (my.Length < checklength)
                                {
                                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(startstr + "内容应不得少于" + checklength.ToString() + "字");
                                    IsChecked = false;
                                    return;
                                }
                                else
                                {
                                    IsChecked = true;
                                    //break;
                                    //return;
                                }
                                //m_app.CustomMessageBox.MessageShow(startstr+"后面的内容:"+my);
                            }
                        }
                    }
                    if (IsChecked)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("所需验证元素均符合要求");
                    }
                    //string allstr = body.InnerText;
                    //string startstr = "主　诉：";
                    //int startop = allstr.IndexOf("主　诉：", 0) + startstr.Length; //开始位置
                    //int endop = allstr.IndexOf("现病史", startop); //结束位置
                    //string my=allstr.Substring(startop, endop - startop); //取搜索的条数，用结束的位置-开始的位置,并返回 
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 必填项检查
        /// </summary>
        private void CheckDocment()
        {
            try
            {
                DocumentModel model = this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel;

                this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Test;
                this.CurrentForm.zyEditorControl1.CheckMustClickElement();
                this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 病历提取
        /// <summary>
        /// 病历提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Collect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string errorMessage = "请选择要提取的病历内容";
                string correctMessage = "病历提取成功";
                if (CurrentForm == null)
                {
                    m_app.CustomMessageBox.MessageShow(errorMessage, DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    return;
                }

                string collectContent = CurrentForm.zyEditorControl1.EMRDoc.Content.GetSelectedText();
                if (collectContent != null && collectContent.Trim() != "")
                {
                    m_patUtil.InsertDocCollectContent(collectContent);
                    AddDocCollect(collectContent);
                    m_app.CustomMessageBox.MessageShow(correctMessage, DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow(errorMessage, DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void AddDocCollect(string content)
        {
            try
            {
                DataTable dt;
                if (gridControlCollect.DataSource == null)
                {
                    dt = InitDataCollect();
                    gridControlCollect.DataSource = dt;
                }
                else
                {
                    dt = gridControlCollect.DataSource as DataTable;
                }
                DataRow dr = dt.NewRow();
                dr["CONTENT"] = content;
                dt.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 根据权限、病历状态、登录人决定BarButton和PopupButton的显示

        /// <summary>
        /// 针对病历或容器BtnButton的可用情况
        /// </summary>
        /// <param name="b"></param>
        private void SetBarStatusModelOrContainer(bool b)
        {
            try
            {
                btn_SaveAs.Enabled = b;//另存
                btn_Collect.Enabled = b;//抽取
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void SetBarStatus(bool enable)
        {
            try
            {
                btn_new.Enabled = enable;//新增
                barButton_Delete.Enabled = enable;//删除
                barButton_Submit.Enabled = enable;//提交
                barButton_Audit.Enabled = enable;//审核
                barButtonItemCancelAudit.Enabled = enable;//取消审核 todo wwj 2011-09-06
                barButtonSave.Enabled = enable;//保存 add by cyq 2013-01-17
                btn_SaveAs.Enabled = enable;//另存
                //btnHistoryEmrBatchIn.Enabled = enable;//历史病历批量导入 wwj 2012-04-26
                CanEditEmrDoc(enable);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 处于审核状态时BarButton可以使用的情况
        /// </summary>
        /// <param name="b"></param>
        private void AuditStateBarButton(bool b)
        {
            try
            {
                barButton_Audit.Enabled = b;
                barButtonItemCancelAudit.Enabled = b;//取消审核 todo wwj 2011-09-06

                //可以审核就可以保存、编辑、只读等
                CanEditEmrDoc(b);

                //处于审核状态的病历不能删除、不能提交
                barButton_Delete.Enabled = false;
                barButton_Submit.Enabled = false;

                //不能审核时需要把编辑器置为只读
                if (!b)
                {
                    SetDocmentReadOnlyMode();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 可以编辑就可以用的BarButton【不包括提交和删除】
        /// </summary>
        /// <param name="b"></param>
        private void CanEditEmrDoc(bool b)
        {
            try
            {
                btnCopy.Enabled = b;//复制
                btnPaste.Enabled = b;//粘贴
                barButtonItem_Cut.Enabled = b;//剪切
                barButtonItem_Undo.Enabled = b;//撤销
                barButtonItem_Redo.Enabled = b;//重复
                barEditItem_Font.Enabled = b;//字体
                barEditItem_FontSize.Enabled = b;//字体大小
                barEditItemDefaultFontSize.Enabled = b;//默认字体大小
                barButtonItem_Bold.Enabled = b;//加粗
                barButtonItem_Italy.Enabled = b;//斜体
                barButtonItem_Undline.Enabled = b;//下划线
                barButtonItemBold.Enabled = b;//加粗
                barButtonItem_Sup.Enabled = b;//下标
                barButtonItem_Sub.Enabled = b;//上标
                barSubItem1.Enabled = b;//表格
                btn_Edit.Enabled = b;//编辑
                btnEmrReadOnly.Enabled = b;//只读
                barButtonSave.Enabled = b;//保存
                barButton_Check.Enabled = b;//自检
                barButton_Delete.Enabled = b;//删除 Add by wwj 2011-09-06
                barButtonItemQuanZi.Enabled = b;//圈字 Add by wwj 2012-05-31

                barButtonFontColor.Enabled = b;//字体颜色 add by ywk 2012年11月19日9:22:23
                btnSetPageBackColor.Enabled = b;//设置背景颜色 add by ywk 2012年12月13日9:55:52
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重设编辑模式下的Action状态
        /// </summary>
        private void ResetEditModeAction(EmrModel model)
        {
            try
            {
                if (model == null) return;

                SetBarStatus(false);//重置所有的BarButton
                SetBarStatusModelOrContainer(true);

                IEmrModelPermision modelPermision;
                bool b;

                #region 根据权限
                //DrectSoft.Common.Eop.Employee empl = new DrectSoft.Common.Eop.Employee(m_app.User.Id);
                //empl.ReInitializeProperties();
                modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Audit, CurrentEmployee);
                b = modelPermision.CanDo(model);
                if (b) // 文件已提交，处于审核模式，且有审核权限
                {
                    if (model.State == ExamineState.FirstExamine //主治医已经检查完成
                        && DoctorEmployee.Grade.Trim() != ""
                        && (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade) == DoctorGrade.Attending)//登录人是主治医
                    {
                        //主治医已经审核过，不能再修改
                        AuditStateBarButton(!b);
                    }
                    else
                    {
                        //主任医可以再次审核
                        AuditStateBarButton(b);
                    }
                }
                else // 非审核模式下，针对新增文件的操作才可用
                {
                    if (model.State == ExamineState.NotSubmit)
                    {
                        // 保存——具有编辑权限
                        modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Edit, CurrentEmployee);
                        b = modelPermision.CanDo(model);
                        barButtonSave.Enabled = b;

                        if (!barButtonSave.Enabled)//保存按钮不可用
                        {
                            //不可编辑
                            CanEditEmrDoc(false);
                            SetDocmentReadOnlyMode();
                        }
                        else
                        {
                            //可以编辑
                            CanEditEmrDoc(true);
                            SetDomentEditMode();
                        }

                        // 提交——新增状态，未归档，本人创建
                        modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Submit, CurrentEmployee);
                        b = modelPermision.CanDo(model);
                        barButton_Submit.Enabled = b;

                        // 删除——新增状态，未归档，本人创建
                        modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Delete, CurrentEmployee);
                        b = modelPermision.CanDo(model);
                        barButton_Delete.Enabled = b;
                    }
                    else
                    {
                        barButtonSave.Enabled = false;
                        //不可编辑
                        CanEditEmrDoc(false);
                        SetDocmentReadOnlyMode();
                        barButton_Submit.Enabled = false;
                        barButton_Delete.Enabled = false;
                    }
                }
                #endregion

                #region 根据病历类别

                if (model.DailyEmrModel)//对应病程不开放另存功能
                {
                    btn_SaveAs.Enabled = false;
                }

                #endregion

                //病案管理 add by cyq 2012-12-10
                if (floaderState == FloderState.None || floaderState == FloderState.FirstPage)
                {
                    CanEditEmrDoc(false);
                    SetDocmentReadOnlyMode();
                }

                ControlTreeListNodeByUser();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重设编辑模式下的Action状态
        /// </summary>
        /// <param name="container"></param>
        private void ResetEditModeAction(EmrModelContainer container)
        {
            try
            {
                if (container == null) return;

                SetBarStatusModelOrContainer(false);
                //医生文档节点不可新增操作 add by ywk 2012年12月19日10:20:37 
                if (container.EmrContainerType == ContainerType.None || container.Name == "医生文档")
                {
                    //非病历类型的界面 如：首页、三测单没有删除和添加功能
                    btn_new.Enabled = false;
                    CanEditEmrDoc(false);
                    //提交
                    btnSubmitPopup.Enabled = false;
                    barButton_Submit.Enabled = false;
                    //审核
                    barButton_Audit.Enabled = false;
                    btnConfirmPopup.Enabled = false;
                    //取消审核 todo wwj 2011-09-06
                    barButtonItemCancelAudit.Enabled = false;
                    btnCancelAuditPopup.Enabled = false;
                    //edit by cyq 2013-01-17
                    //病案首页默认显示“保存”
                    //if (container.ContainerCatalog == ContainerCatalog.BingAnShouYe)
                    //{
                    //    barButtonSave.Enabled = true;
                    //    btnSavePopup.Enabled = true;
                    //}
                    //else if (container.ContainerCatalog == ContainerCatalog.huizhenjilu)
                    //{
                    //    barButtonSave.Enabled = true;
                    //    btnSavePopup.Enabled = true;
                    //}
                }
                else
                {
                    btn_new.Enabled = CanAddNew;//add by zhouhui 2012-5-18 新增的权限也要判断职工的医师级别
                    CanEditEmrDoc(false);
                    barButton_Submit.Enabled = false;
                    barButton_Audit.Enabled = false;
                    barButtonItemCancelAudit.Enabled = false;//取消审核 todo wwj 2011-09-06
                }

                ControlTreeListNodeByUser();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("ResetEditModeAction方法出错" + ex.Message);
            }

        }

        #endregion

        #region 对登录人员进行控管，护士只能修改“护士文档”，医生能修改除“护士文档”以外所以的文档
        /// <summary>
        /// 对登录人员进行控管，护士只能修改“护士文档”，医生能修改除“护士文档”以外所以的文档
        /// </summary>
        private void ControlTreeListNodeByUser()
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode; ;

                if (node == null) return;

                if (DoctorEmployee.Kind == EmployeeKind.Nurse)//当前登录人是护士
                {
                    bool isNurseNode = false;
                    SetBarButtonStatusNurse(node, ref isNurseNode);
                    if (!isNurseNode)
                    {
                        SetSetEmrDocNotEdit();
                    }
                }
                else if (CheckIsDoctor())//当前登录人是医生
                {
                    bool isDoctorNode = true;
                    SetBarButtonStatusDoctor(node, ref isDoctorNode);
                    if (!isDoctorNode)
                    {
                        SetSetEmrDocNotEdit();
                    }
                }
                else//其他人
                {
                    SetSetEmrDocNotEdit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 不可编辑
        /// </summary>
        private void SetSetEmrDocNotEdit()
        {
            try
            {
                CanEditEmrDoc(false);
                SetDocmentReadOnlyMode();
                btn_new.Enabled = false;
                barButtonSave.Enabled = false;
                barButton_Submit.Enabled = false;
                barButton_Delete.Enabled = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 判断树节点是否是护士文档的节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isNurseNode"></param>
        private void SetBarButtonStatusNurse(TreeListNode node, ref bool isNurseNode)
        {
            try
            {
                if (node.ParentNode == null)
                {
                    return;
                }
                else
                {
                    if (node.ParentNode.GetValue("colName").ToString() == "护士文档")
                    {
                        isNurseNode = true;
                        return;
                    }
                }
                SetBarButtonStatusNurse(node.ParentNode, ref isNurseNode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 判断树节点是否是除护士文档以外的节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isDoctorNode"></param>
        private void SetBarButtonStatusDoctor(TreeListNode node, ref bool isDoctorNode)
        {
            try
            {
                if (node.ParentNode == null)
                {
                    return;
                }
                else
                {
                    if (node.ParentNode.GetValue("colName").ToString() == "护士文档")
                    {
                        isDoctorNode = false;
                        return;
                    }
                }
                SetBarButtonStatusDoctor(node.ParentNode, ref isDoctorNode);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                    SetSetEmrDocNotEdit();
                    if (CurrentForm != null)
                    {
                        //CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Read;
                        SetDocmentReadOnlyMode();
                        CurrentForm.zyEditorControl1.EMRDoc.Info.ShowParagraphFlag = false;
                        CurrentForm.zyEditorControl1.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 导出
        private void barButtonExportFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ExportFile();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 导出病历文件
        /// </summary>
        private void ExportFile()
        {
            try
            {
                if (CurrentForm == null) return;
                using (System.Windows.Forms.SaveFileDialog dlg = new SaveFileDialog())
                {
                    string filename = CurrentForm.zyEditorControl1.Text;
                    dlg.Filter = "电子病历文件|*.b";
                    dlg.FileName = filename;

                    if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        byte[] data = CurrentForm.zyEditorControl1.SaveBinary();
                        System.IO.FileStream fs = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.Create);
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void barButton_ExportXml_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ExprotXML();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 导出XML文件
        /// </summary>
        private void ExprotXML()
        {
            try
            {
                if (CurrentForm == null) return;
                using (System.Windows.Forms.SaveFileDialog dlg = new SaveFileDialog())
                {
                    string filename = CurrentForm.zyEditorControl1.Text;
                    dlg.Filter = "XML文件|*.xml";
                    if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        CurrentForm.zyEditorControl1.EMRDoc.ToXMLFile(dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 查找
        private void barButtonFind_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                find();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 查找
        /// </summary>
        public void find()
        {
            try
            {
                if (CurrentForm == null) return;
                if (m_Searchfrm != null)
                {
                    m_Searchfrm.Close();
                    m_Searchfrm.Dispose();
                }
                m_Searchfrm = new SearchFrm(m_app, CurrentForm.zyEditorControl1);
                m_Searchfrm.Show(this);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        SearchFrm m_Searchfrm;
        #endregion

        #region TreeList 提示 edit by cyq 2012-09-26
        private void InitToolTip(string displayName, TreeList tipTreeList, Point point)
        {
            try
            {
                if (displayName.Trim() != "")
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
                throw new Exception(ex.Message);
            }
        }

        private void treeList1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hitInfo = treeList1.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.Node == null || hitInfo.Node.Level < 2)
                {
                    return;
                }
                if (hitInfo.Node.Tag is EmrModel)
                {
                    EmrModel model = (EmrModel)hitInfo.Node.Tag;
                    InitToolTip(null == model ? "" : model.ModelName, treeList1, treeList1.PointToScreen(e.Location));
                }
                else if (hitInfo.Node.Tag is EmrModelContainer)
                {
                    EmrModelContainer model = (EmrModelContainer)hitInfo.Node.Tag;
                    InitToolTip(null == model ? "" : model.Name, treeList1, treeList1.PointToScreen(e.Location));
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 得到Button的状态,供外部调用

        public bool GetSaveButtonEnable()
        {
            try
            {
                return barButtonSave.Enabled;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GetDeleteButtonEnable()
        {
            try
            {
                return barButton_Delete.Enabled;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GetSubmitButtonEnable()
        {
            try
            {
                return barButton_Submit.Enabled;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GetAuditButtonEnable()
        {
            try
            {
                return barButton_Audit.Enabled;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GetCancelAuditButtonEnable()
        {
            try
            {
                return barButtonItemCancelAudit.Enabled;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool GetLocateButtonEnable()
        {
            try
            {
                if (m_CurrentModel.ModelCatalog == ContainerCatalog.BingChengJiLu)//针对病程开启定位功能
                {
                    if (m_CurrentTreeListNode != null && m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Nodes.Count > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public TreeListNodes GetACTreeListNodes()
        {
            try
            {
                if (m_CurrentModel.ModelCatalog == ContainerCatalog.BingChengJiLu && m_CurrentTreeListNode != null
                    && m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Nodes.Count > 0)
                {
                    return m_CurrentTreeListNode.ParentNode.Nodes;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 转换TabPage的名字
        private string GetTabPageName(string modelName)
        {
            try
            {
                return modelName;

                //string name = string.Empty;
                //string[] names = modelName.Split(' ');
                //if (names.Length >= 4)
                //{
                //    for (int i = 0; i < names.Length - 3; i++)
                //    {
                //        name += names[i];
                //    }
                //}
                //return name;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 更改TreeListNode和TabPage的名字，针对在病程中修改病程名称和时间
        public void ReSetTreeListNodeAndTabPageName(string oldDateTime, string oldTitle, string newDateTime, string newTitle)
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
                if (node != null && node.Tag is EmrModel)
                {
                    EmrModel model = node.Tag as EmrModel;
                    if (model.DailyEmrModel) //病程
                    {
                        if (newDateTime == null && newTitle == null) return;

                        string title = model.ModelName.Split(' ')[0];
                        string date = model.ModelName.Split(' ')[1];
                        string time = model.ModelName.Split(' ')[2];
                        string creater = model.ModelName.Split(' ')[3];

                        //model.ModelName = (newTitle == null ? title : newTitle) + " " + (newDateTime == null ? date + " " + time : newDateTime) + " " + creater;
                        model.ModelName = title + " " + (newDateTime == null ? date + " " + time : newDateTime) + " " + creater;
                        model.DisplayTime = newDateTime == null ? model.DisplayTime : Convert.ToDateTime(newDateTime);
                        model.Description = model.ModelName;
                        node.SetValue("colName", model.ModelName);
                        xtraTabControl1.SelectedTabPage.Text = model.ModelName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 得到TreeList中具有焦點的Node
        public TreeListNode GetFocusNode()
        {
            try
            {
                return treeList1.FocusedNode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 展開子節點的同時收縮同級的節點，保證在同級中只有一個節點處於展開的狀態，暫時不用
        private void treeList1_BeforeExpand(object sender, BeforeExpandEventArgs e)
        {
            try
            {
                TreeListNode parentNode = e.Node.ParentNode;
                if (parentNode != null && parentNode.Nodes.Count > 0)
                {
                    for (int i = 0; i < parentNode.Nodes.Count; i++)
                    {
                        TreeListNode node = parentNode.Nodes[i];
                        if (node != e.Node)
                        {
                            node.Expanded = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 右侧工具栏拖拽实现

        #region 已作废
        private void treeList_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    m_IsAllowDrop = true;
                else
                    m_IsAllowDrop = false;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void treeList_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && m_IsAllowDrop)
                {
                    TreeList list = (TreeList)sender;
                    DoDragDorpInfo(list);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 正在使用中

        private void imageListBoxControlSymbol_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ImageListBoxControl list = (ImageListBoxControl)sender;
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        private void DoDragDorpInfo(TreeList tree)
        {
            try
            {
                //add by yxy 在工具栏拖拽的时候需要判断病历是否可编辑
                //if (this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel == DocumentModel.Read && )
                //    return;
                if (tree.FocusedNode == null) return;
                KeyValuePair<string, object> data = new KeyValuePair<string, object>();
                if ((tree.FocusedNode != null) && (tree.FocusedNode.Tag != null))
                {

                    if (tree.Name == treeListPerson.Name)
                    {

                        TempletItem item = tree.FocusedNode.Tag as TempletItem;

                        data = new KeyValuePair<string, object>(item.Content, ElementType.Text);
                    }
                    else
                    {

                        DataRow row = (DataRow)tree.FocusedNode.Tag;

                        if (tree.Name == treeListSymb.Name)
                        {
                            data = new KeyValuePair<string, object>(row["名称"].ToString(), ElementType.Text);
                        }
                        else if (tree.Name == treeListTZ.Name)
                        {
                            byte[] tz = m_patUtil.GetTZDetailInfo(row["名称"].ToString());
                            data = new KeyValuePair<string, object>("ZYTemplate", tz);

                        }
                        else if (tree.Name == treeListZZ.Name)
                        {
                            byte[] tz = m_patUtil.GetZZItemDetailInfo(row["名称"].ToString());
                            data = new KeyValuePair<string, object>("ZYTemplate", tz);
                        }

                        else if (tree.Name == treeListCy.Name)
                        {
                            Macro mac = new Macro(row);

                            MacroUtil.FillMarcValue(m_CurrentInpatient.NoOfFirstPage.ToString(), mac, m_app.User.Id);
                            if (!string.IsNullOrEmpty(mac.MacroValue))
                                data = new KeyValuePair<string, object>(mac.MacroValue, ElementType.Text);
                        }
                        else if (tree.Name == treeListImage.Name)
                        {
                            byte[] img = m_patUtil.GetImage(row["ID"].ToString());

                            data = new KeyValuePair<string, object>("ZYTextImage", img);
                        }
                        //yxy
                        else if (tree.Name == treeListBL.Name)
                        {
                            data = new KeyValuePair<string, object>(row["CONTENT"].ToString(), ElementType.Text);
                        }
                    }

                }
                if (!string.IsNullOrEmpty(data.Key))
                    tree.DoDragDrop(data, DragDropEffects.All);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 科室小模板相关实现 "关于具体功能协调问题(待需求确定)"
        const string sql_queryTree = "select * from emrtemplet_item_person_catalog where (deptid='{0}' and isperson='0') or (isperson='1' and createusers='{1}')   and deptid='{0}' or createusers='{1}'";
        const string sql_queryLeaf = "select * from emrtemplet_item_person a where (a.deptid='{0}' and isperson=0) or (a.isperson = 1 and a.createusers = '{1}')   and deptid='{0}' or createusers='{1}'";
        #region "暂停"
        //        const string sql_insertCatalog = @"insert into emrtemplet_item_person_catalog
        //                                                  (ID, NAME, Previd, VALID, Memo, DEPTID, CONTAINER,isperson,createusers)
        //                                                VALUES
        //                                                  ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}','{7}','{8}')";
        #endregion
        //科室小模板
        private void barButtonItemPerson_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_PersonItem == null)
                    m_PersonItem = new PersonItemManager(m_app);
                m_PersonItem.ShowDialog();
                //InitPersonTree(true, string.Empty);
                //此处修改，科室小模板窗体关闭后，原来的右侧清空了，现在更改为加载进来时的科室模板列表，声明了属性记录窗体加载时的模板值。add by  ywk 2012年6月11日 15:58:45
                InitPersonTree(true, MyContainerCode);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 加载小模板列表
        /// </summary>
        /// <param name="refresh"></param>
        /// <param name="catalog"></param>
        private void InitPersonTree(bool refresh, string catalog)
        {
            try
            {
                if ((m_MyTreeFolders == null) || (refresh))
                {
                    m_MyTreeFolders = m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryTree, m_app.User.CurrentDeptId, m_app.User.Id));
                    m_MyLeafs = m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryLeaf, m_app.User.CurrentDeptId, m_app.User.Id));
                }
                DataRow[] rows = m_MyTreeFolders.Select("(Previd='' or Previd is null)  and (container = '99' or container ='" + catalog + "')");
                treeListPerson.BeginUnboundLoad();
                treeListPerson.Nodes.Clear();
                foreach (DataRow dr in rows)
                {
                    LoadTree(dr["ID"].ToString(), dr, null, catalog);
                }
                treeListPerson.CollapseAll();
                treeListPerson.EndUnboundLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void LoadTree(string id, DataRow row, TreeListNode node, string catalog)
        {
            try
            {
                TreeListNode nd = treeListPerson.AppendNode(new object[] { id, row["NAME"], "Folder", row["ISPERSON"], row["CREATEUSERS"] }, node);
                nd.Tag = row;

                //查找叶子节点
                DataRow[] leafRows = m_MyLeafs.Select("PARENTID='" + id + "' ");
                if (leafRows.Length > 0)
                {
                    foreach (DataRow leaf in leafRows)
                    {
                        TreeListNode leafnd = treeListPerson.AppendNode(new object[] { leaf["Code"], leaf["NAME"], "Leaf", leaf["ISPERSON"], leaf["CREATEUSERS"] }, nd);
                        TempletItem item = new TempletItem(leaf);
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
                if ((m_MyTreeFolders == null) || (refresh))
                {
                    m_MyTreeFolders = m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryTree, m_app.User.CurrentDeptId, m_app.User.Id));
                    m_MyLeafs = m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryLeaf, m_app.User.CurrentDeptId, m_app.User.Id));
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
                treeListPerson.BeginUnboundLoad();
                treeListPerson.Nodes.Clear();
                foreach (DataRow dr in allSearchedFirstFolders)
                {
                    SearchLoadTree(dr, null, catalog, allSearchedFolders, serchedLeafs, keyword);
                }
                treeListPerson.CollapseAll();
                treeListPerson.EndUnboundLoad();
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
                TreeListNode nd = treeListPerson.AppendNode(new object[] { row["ID"], row["NAME"], "Folder", row["ISPERSON"], row["CREATEUSERS"] }, node);
                nd.Tag = row;

                //查找叶子节点
                var leafRows = searchedLeafs.Where(p => p["PARENTID"].ToString() == row["ID"].ToString());
                if (null != leafRows && leafRows.Count() > 0)
                {
                    foreach (DataRow leaf in leafRows)
                    {
                        TreeListNode leafnd = treeListPerson.AppendNode(new object[] { leaf["Code"], leaf["NAME"], "Leaf", leaf["ISPERSON"], leaf["CREATEUSERS"] }, nd);
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
        /// 搜索事件
        /// by cyq 2012-09-25
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                SearchPersonTree(false, MyContainerCode, this.txt_search.Text);
                if (!string.IsNullOrEmpty(this.txt_search.Text))
                {
                    foreach (TreeListNode node in treeListPerson.Nodes)
                    {
                        node.ExpandAll();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void treeListPerson_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    m_IsAllowDrop = true;
                else
                    m_IsAllowDrop = false;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void treeListPerson_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && m_IsAllowDrop)
                {
                    TreeList list = (TreeList)sender;
                    if ((list.FocusedNode != null) && (list.FocusedNode.Tag is TempletItem))
                        DoDragDorpInfo(list);
                }
                else
                {
                    m_IsAllowDrop = false;
                }

                //工具提示 add by cyq 2012-09-26
                TreeListHitInfo hitInfo = treeListPerson.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.Node != null)
                {
                    if (hitInfo.Node.GetValue("NODETYPE").ToString() == "Leaf" && hitInfo.Node.Tag is TempletItem)
                    {
                        TempletItem item = (TempletItem)hitInfo.Node.Tag;
                        InitToolTip(null == item ? "" : item.Content, treeListPerson, treeListPerson.PointToScreen(e.Location));
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        private void treeListPerson_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                m_IsAllowDrop = false;
                //by cyq 2012-09-03
                //右侧小模板列表右键
                if (e.Button == MouseButtons.Right)
                {
                    barButtonDeptTemplate.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    popupMenuDeptTemplate.ShowPopup(treeListPerson.PointToScreen(new Point(e.X, e.Y)));
                }

                #region "暂时停用"
                //by cyq 2012-09-03
                //右侧小模板列表右键
                //if (e.Button == MouseButtons.Right)
                //{
                //    TreeListHitInfo hitInfo = treeListPerson.CalcHitInfo(e.Location);
                //    treeListPerson.Focus();
                //    if (hitInfo != null && hitInfo.Node != null      &&1!=1)
                //    {
                //        treeListPerson.FocusedNode = hitInfo.Node;
                //        TreeListNode node = hitInfo.Node;
                //        barButtonDeptTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //        if (node.HasChildren)//父节点为空,本身是父节点
                //        {
                //            barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增大分类
                //            barButtonItemNewChild.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增子分类
                //            barButtonItemNewTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增模板
                //            barButtonItemDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//删除
                //            barButtonItemModifyCate.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//修改分类
                //            barButtonItemModifyTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改模板
                //            //此节点是分类节点，但是他的子节点下是有分类节点
                //            if (treeListPerson.FocusedNode.Nodes[0].GetValue("NODETYPE").ToString().Equals("Folder"))
                //            {
                //                barButtonItemNewTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改模板
                //            }
                //            //此节点有子节点又有父节点，本节点又是分类节点。不显示大分类
                //            if (node.ParentNode != null)
                //            {
                //                barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增大分类
                //            }
                //        }
                //        else//子节点
                //        {
                //            barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增大分类
                //            barButtonItemNewChild.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增子分类
                //            barButtonItemNewTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增模板
                //            barButtonItemDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//删除
                //            barButtonItemModifyCate.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//修改分类
                //            barButtonItemModifyTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//修改模板
                //            ////节点为分类节点，但是还没有添加子模板的情况  edit by ywk 2012年6月11日 09:27:22（分类节点）
                //            if (treeListPerson.FocusedNode.GetValue("NODETYPE").ToString().Equals("Folder"))
                //            {
                //                barButtonItemModifyTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改模板
                //                barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增大分类
                //                //barButtonItemNewItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增模板
                //            }
                //            //此节点点击的是具体的模板，不显示分类相关菜单(模板节点)
                //            if (treeListPerson.FocusedNode.GetValue("NODETYPE").ToString().Equals("Leaf"))
                //            {
                //                barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增大分类
                //                barButtonItemNewChild.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增子分类
                //                barButtonItemModifyCate.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改分类
                //                barButtonItemNewTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增模板
                //            }
                //        }
                //    }
                //    else
                //    {
                //        barButtonDeptTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                //        barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //        barButtonItemNewChild.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //        barButtonItemNewTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //        barButtonItemDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //        barButtonItemModifyCate.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //        barButtonItemModifyTemplete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                //    }
                //    popupMenuDeptTemplete.ShowPopup(treeListPerson.PointToScreen(new Point(e.X, e.Y)));
                //}
                #endregion
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void treeListPerson_AfterExpand(object sender, NodeEventArgs e)
        {
            //if (e.Node.HasChildren)
            //    e.Node.ExpandAll();
        }

        #region "暂时停用"
        //新增大分类
        private void barButtonItemNewRoot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ////新增大分类的时候清空上次的值  ywk 
            //m_ItemCatalog = new ItemCatalog(m_app);
            //m_ItemCatalog.BigOrSmallItem = "big";
            //if (m_ItemCatalog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    try
            //    {
            //        string id = Guid.NewGuid().ToString();
            //        string createuser = m_app.User.Id;
            //        sqlHelper.ExecuteNoneQuery(string.Format(sql_insertCatalog, id, m_ItemCatalog.ITEMNAME, string.Empty, 1, m_ItemCatalog.Memo,
            //            m_app.User.CurrentDeptId, m_ItemCatalog.ContainerCode, m_ItemCatalog.IsPerson, createuser));
            //        TreeListNode leafnode = treeListPerson.AppendNode(new object[] { id, m_ItemCatalog.ITEMNAME, "Folder", m_ItemCatalog.IsPerson, m_ItemCatalog.CreateUser }, null);
            //        DataTable dt = new DataTable();
            //        dt.Columns.Add("ISPERSON");
            //        dt.Columns.Add("CREATEUSERS");
            //        dt.Columns.Add("ITEMNAME");
            //        DataRow drow = dt.NewRow();
            //        drow["ISPERSON"] = m_ItemCatalog.IsPerson;
            //        drow["CREATEUSERS"] = m_ItemCatalog.CreateUser;
            //        drow["ITEMNAME"] = m_ItemCatalog.ITEMNAME;
            //        leafnode.Tag = drow;
            //        treeListPerson.FocusedNode = leafnode;
            //    }
            //    catch (Exception EX)
            //    {
            //        m_app.CustomMessageBox.MessageShow(EX.Message);
            //    }
            //}
        }
        //新增子分类
        private void barButtonItemNewChild_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ////新增子分类的时候清空上次的值  ywk 
            //m_ItemCatalog = new ItemCatalog(m_app);
            //if ((treeListPerson.Nodes.Count > 0) && (treeListPerson.FocusedNode == null))
            //{
            //    m_app.CustomMessageBox.MessageShow("请选择父节点");
            //    return;
            //}

            //string parentID = string.Empty;
            //DataRow row = (DataRow)(treeListPerson.FocusedNode.Tag);
            //if (this.treeListPerson.FocusedNode != null)
            //{
            //    parentID = treeListPerson.FocusedNode.GetValue("ID").ToString();
            //    m_ItemCatalog.IsPerson = row["ISPERSON"].ToString();
            //    m_ItemCatalog.CreateUser = row["CREATEUSERS"].ToString();
            //    m_ItemCatalog.BigOrSmallItem = "small";
            //}

            //if (m_ItemCatalog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    try
            //    {
            //        string id = Guid.NewGuid().ToString();
            //        string createuser = m_app.User.Id;
            //        sqlHelper.ExecuteNoneQuery(string.Format(sql_insertCatalog, id, m_ItemCatalog.ITEMNAME, parentID, 1, m_ItemCatalog.Memo,
            //                    m_app.User.CurrentDeptId, m_ItemCatalog.ContainerCode, m_ItemCatalog.IsPerson, createuser));
            //        TreeListNode leafnode = treeListPerson.AppendNode(new object[] { id, m_ItemCatalog.ITEMNAME, "Folder", m_ItemCatalog.IsPerson, m_ItemCatalog.CreateUser }, treeListPerson.FocusedNode);
            //        DataTable dt = new DataTable();
            //        dt.Columns.Add("ISPERSON");
            //        dt.Columns.Add("CREATEUSERS");
            //        dt.Columns.Add("ITEMNAME");
            //        DataRow drow = dt.NewRow();
            //        drow["ISPERSON"] = m_ItemCatalog.IsPerson;
            //        drow["CREATEUSERS"] = m_ItemCatalog.CreateUser;
            //        drow["ITEMNAME"] = m_ItemCatalog.ITEMNAME;
            //        leafnode.Tag = drow;

            //        treeListPerson.FocusedNode = leafnode;
            //    }
            //    catch (Exception EX)
            //    {
            //        m_app.CustomMessageBox.MessageShow(EX.Message);
            //    }

            //}
        }
        //新增模板
        private void barButtonItemNewTemplete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //            if (treeListPerson.FocusedNode == null) return;
            //            string type = treeListPerson.FocusedNode.GetValue("NODETYPE").ToString();
            //            if (type.Equals("Leaf"))
            //            {
            //                m_app.CustomMessageBox.MessageShow("请选择分类节点添加子模板");
            //                return;
            //            }
            //            if ((treeListPerson.FocusedNode.HasChildren) && (treeListPerson.FocusedNode.Nodes[0].GetValue("NODETYPE").ToString().Equals("Folder")))
            //            {
            //                m_app.CustomMessageBox.MessageShow("请选择最底层分类节点添加子模板");
            //                return;
            //            }

            //            DataRow drow = (DataRow)(treeListPerson.FocusedNode.Tag);
            //            //m_ItemContent.MyItem.CatalogName = "";
            //            //新增时，清空上次操作里的值 add ywk 
            //            m_ItemContent.MyItem.Content = "";
            //            m_ItemContent.MyItem.ItemName = "";

            //            m_ItemContent.MyItem.Code = Guid.NewGuid().ToString();
            //            m_ItemContent.MyItem.ParentID = treeListPerson.FocusedNode.GetValue("ID").ToString();
            //            m_ItemContent.MyItem.CatalogName = drow["ITEMNAME"].ToString();
            //            m_ItemContent.MyItem.IsPerson = drow["ISPERSON"].ToString();
            //            //新增的时候把当前登录人赋给创建人 add by ywk 2012年6月5日 10:28:10 
            //            m_ItemContent.MyItem.CreateUser = m_app.User.Id;
            //            //m_ItemContent.MyItem.CreateUser = treeListPerson.FocusedNode.GetValue("CREATEUSER").ToString();

            //            if (m_ItemContent.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //            {
            //                try
            //                {
            //                    //string createuser = "";
            //                    //if (m_ItemContent.MyItem.IsPerson == "1")
            //                    string createuser = m_app.User.Id;// 科室小模板和个人小模板都加上创建人

            //                    // 输入的科室小模板的模板内容
            //                    string content = m_ItemContent.MyItem.Content;
            //                    if (m_ItemContent.MyItem.Content.Contains("\r\n"))//存在换行，替代插入数据库中 edit by ywk
            //                    {
            //                        content = m_ItemContent.MyItem.Content.Replace("\r\n", "'||chr(10)||chr(13)||'");
            //                    }
            //                    const string sql_InsertItem = @"insert into emrtemplet_item_person(CODE,NAME,deptid,item_content,deptshare,parentid,isperson,createusers)
            //                                            values('{0}','{1}','{2}','{3}','{4}','{5}', '{6}','{7}')";
            //                    m_app.SqlHelper.ExecuteNoneQuery(string.Format(sql_InsertItem, m_ItemContent.MyItem.Code, m_ItemContent.MyItem.ItemName,
            //                        m_app.User.CurrentDeptId, content/*m_ItemContent.MyItem.Content*/, 1, m_ItemContent.MyItem.ParentID, m_ItemContent.MyItem.IsPerson, createuser));

            //                    TreeListNode nd = treeListPerson.AppendNode(new object[] { m_ItemContent.MyItem.Code, m_ItemContent.MyItem.ItemName, "Leaf", m_ItemContent.MyItem.IsPerson, m_ItemContent.MyItem.CreateUser }, treeListPerson.FocusedNode);
            //                    nd.Tag = m_ItemContent.MyItem;
            //                    //treeListPerson.FocusedNode.ExpandAll();

            //                    treeListPerson.FocusedNode = nd;

            //                }
            //                catch (Exception ex)
            //                {
            //                    m_app.CustomMessageBox.MessageShow(ex.Message);
            //                }
            //            }
        }
        //修改分类
        private void barButtonItemModifyCate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //            //修改分类语句
            //            const string sql_updateItem = @"update emrtemplet_item_person_catalog 
            //                                                set name = '{1}',
            //                                                    memo = '{2}',
            //                                                    container = '{3}',
            //                                                    isperson  = '{4}',
            //                                                    createusers = '{5}'
            //                                                where ID = '{0}'";
            //            //更新掉分类下面的模板IsPerson字段 add by ywk 
            //            const string sql_updatechildItem = @"update emrtemplet_item_person set isperson='{0}'
            //                                             where parentid='{1}' ";

            //            const string sql_queryTreeByID = "select * from emrtemplet_item_person_catalog where ID = '{0}'";
            //            if ((treeListPerson.Nodes.Count > 0) && (treeListPerson.FocusedNode == null))
            //            {
            //                m_app.CustomMessageBox.MessageShow("请选择节点！");
            //                return;
            //            }

            //            string nodeID = string.Empty;
            //            if (this.treeListPerson.FocusedNode != null)
            //            {
            //                nodeID = treeListPerson.FocusedNode.GetValue("ID").ToString();
            //            }

            //            DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryTreeByID, nodeID));
            //            if (dt.Rows.Count == 0)
            //            {
            //                m_app.CustomMessageBox.MessageShow("请选择分类节点！");
            //                return;
            //            }
            //            m_ItemCatalog.ITEMNAME = dt.Rows[0]["Name"].ToString();
            //            m_ItemCatalog.Memo = dt.Rows[0]["memo"].ToString();
            //            m_ItemCatalog.ContainerCode = dt.Rows[0]["container"].ToString();
            //            m_ItemCatalog.IsPerson = dt.Rows[0]["isperson"].ToString();
            //            m_ItemCatalog.CreateUser = dt.Rows[0]["createusers"].ToString();
            //            if (m_ItemCatalog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //            {
            //                try
            //                {
            //                    string createuser = m_ItemCatalog.CreateUser;
            //                    if (m_ItemCatalog.IsPerson == "1")
            //                        createuser = m_app.User.Id;
            //                    //更新了emrtemplet_item_person_catalog
            //                    sqlHelper.ExecuteNoneQuery(string.Format(sql_updateItem, nodeID, m_ItemCatalog.ITEMNAME, m_ItemCatalog.Memo,
            //                                                                m_ItemCatalog.ContainerCode, m_ItemCatalog.IsPerson, createuser));
            //                    //再更新emrtemplet_item_person表
            //                    sqlHelper.ExecuteNoneQuery(string.Format(sql_updatechildItem, m_ItemCatalog.IsPerson, nodeID));

            //                    InitPersonTree(true, MyContainerCode);

            //                    TreeListNode leafnode = treeListPerson.FindNodeByKeyID(nodeID);
            //                    treeListPerson.FocusedNode = leafnode;
            //                }
            //                catch (Exception EX)
            //                {
            //                    m_app.CustomMessageBox.MessageShow(EX.Message);
            //                }

            //            }
        }
        //修改模板
        private void barButtonItemModifyTemplete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //            if (treeListPerson.FocusedNode == null) return;
            //            TreeListNode focusNode = treeListPerson.FocusedNode;
            //            if ((treeListPerson.FocusedNode.HasChildren) && (treeListPerson.FocusedNode.Nodes[0].GetValue("NODETYPE").ToString().Equals("Folder")))
            //            {
            //                m_app.CustomMessageBox.MessageShow("请选择最底层分类节点编辑子模板");
            //                return;
            //            }
            //            //新增时先记录点击的节点以便修改后还是定位在此处
            //            string nodeID = string.Empty;
            //            if (this.treeListPerson.FocusedNode != null)
            //            {
            //                nodeID = treeListPerson.FocusedNode.GetValue("ID").ToString();
            //            }
            //            TempletItem item = (TempletItem)treeList1.FocusedNode.Tag;
            //            m_ItemContent.MyItem.Content = item.Content.Replace("\n\r", "\r\n");
            //            m_ItemContent.MyItem.Code = item.Code;//模板CODE
            //            m_ItemContent.MyItem.ItemName = item.ItemName;//模板名称
            //            m_ItemContent.MyItem.CatalogName = item.CatalogName;//分类名称
            //            m_ItemContent.MyItem.ParentID = item.ParentID;//父节点
            //            m_ItemContent.MyItem.IsPerson = item.IsPerson;//区分科室和个人小模板

            //            //修改和删除只能控制到是创建人进行操作 add by ywk 
            //            m_ItemContent.MyItem.CreateUser = item.CreateUser;
            //            if (m_ItemContent.MyItem.CreateUser != m_app.User.Id)
            //            {
            //                m_app.CustomMessageBox.MessageShow("此模板只有创建者可以进行修改");
            //                return;
            //            }

            //            if (m_ItemContent.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //            {
            //                string content = string.Empty;
            //                if (m_ItemContent.MyItem.Content.Contains("\r\n"))//存在换行，替代插入数据库中 edit by ywk
            //                {
            //                    content = m_ItemContent.MyItem.Content.Replace("\r\n", "'||chr(10)||chr(13)||'");
            //                }
            //                else
            //                {
            //                    content = m_ItemContent.MyItem.Content;
            //                }
            //                string updatesql = string.Format(@" update emrtemplet_item_person set item_content='{0}',name='{1}' 
            //                    where code='{2}' ", content, m_ItemContent.MyItem.ItemName, item.Code);
            //                m_app.SqlHelper.ExecuteNoneQuery(updatesql, CommandType.Text);

            //                InitPersonTree(true, MyContainerCode);
            //                TreeListNode leafnode = treeListPerson.FindNodeByKeyID(nodeID);
            //                treeListPerson.FocusedNode = leafnode;
            //            }
        }
        //删除
        private void barButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (treeListPerson.FocusedNode == null) return;

            //DataRow drow = (DataRow)(treeListPerson.FocusedNode.Tag);
            //string id = treeListPerson.FocusedNode.GetValue("ID").ToString();
            //string type = treeListPerson.FocusedNode.GetValue("NODETYPE").ToString();
            //string createuser = drow["CREATEUSERS"].ToString();
            //if (!string.IsNullOrEmpty(id))
            //{
            //    //string createuser=string.Empty;//创建者
            //    if (createuser == "")
            //    {
            //        string sql = string.Format(@"select createusers from emrtemplet_item_person_catalog where id='{0}'", id);
            //        DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            //        if (dt.Rows.Count > 0)
            //        {
            //            createuser = dt.Rows[0]["CREATEUSERS"].ToString();
            //        }
            //    }


            //    try
            //    {
            //        //修改和删除只能控制到是创建人进行操作 add by ywk 
            //        //m_ItemContent.MyItem.CreateUser = createuser;
            //        if (createuser != m_app.User.Id)
            //        {
            //            m_app.CustomMessageBox.MessageShow("此模板只有创建者可以删除！");
            //            return;
            //        }

            //        if (type.Equals("Leaf"))
            //            sqlHelper.ExecuteNoneQuery(string.Format("delete from emrtemplet_item_person where Code='{0}'", id));
            //        else
            //            sqlHelper.ExecuteNoneQuery(string.Format("delete from emrtemplet_item_person_catalog where id='{0}'", id));
            //        this.treeListPerson.DeleteNode(this.treeListPerson.FocusedNode);
            //    }
            //    catch (Exception ex)
            //    {
            //        m_app.CustomMessageBox.MessageShow(ex.Message);
            //    }
            //}
        }
        #endregion
        #endregion

        #region 打印 add by wwj 2011-08-24
        private void barButtonItemEmrPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //if (m_CurrentModel == null) return;
                //if (CurrentForm == null) return;

                ////add by yxy 在打印之前先提示保存  病历不保存病历内容会出现丢失现象
                //if (CurrentForm.zyEditorControl1.EMRDoc.Modified && barButtonSave.Enabled == true)
                //{
                //    m_app.CustomMessageBox.MessageShow(m_CurrentModel.ModelName + " 已经修改，请保存后再打印。");
                //    return;
                //}

                if (m_CurrentModel != null)
                {
                    //add by yxy 在打印之前先提示保存  病历不保存病历内容会出现丢失现象
                    if (CheckEditorIfSaved(m_CurrentModel))//if (CurrentForm.zyEditorControl1.EMRDoc.Modified && barButtonSave.Enabled == true) //edit by cyq 2013-02-19
                    {
                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病历已修改，请保存后再打印，您是否要保存？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            SaveDocment(m_CurrentModel);
                        }
                        else
                        {
                            return;
                        }
                        //m_app.CustomMessageBox.MessageShow(m_CurrentModel.ModelName + " 已经修改，请保存后再打印。");
                        //return;
                    }

                    PrintForm printForm = new PrintForm(m_patUtil, m_CurrentModel, CurrentForm.zyEditorControl1.CurrentPage.PageIndex, m_CurrentInpatient.NoOfFirstPage.ToString());
                    printForm.ShowDialog();
                    //string deptChangeID = string.Empty;
                    //if (m_CurrentModel.ModelCatalog == "AC")
                    //{
                    //    ChooseDeptForDailyEmrPrint chooseDept = new ChooseDeptForDailyEmrPrint(m_CurrentInpatient.NoOfFirstPage.ToString());
                    //    if (chooseDept.IsNeedShow)
                    //    {
                    //        chooseDept.ShowDialog();
                    //    }
                    //    if (chooseDept.DialogResult == DialogResult.Yes)
                    //    {
                    //        deptChangeID = chooseDept.DeptChangeID;
                    //    }
                    //    else
                    //    {
                    //        return;
                    //    }
                    //}
                    //New.PrintForm printForm = new New.PrintForm(m_CurrentModel, m_CurrentInpatient, m_app, deptChangeID);
                    //printForm.ShowDialog();

                }
                else if (m_CurrentEmrEditor != null)
                {
                    //SetWaitDialogCaption("正在加载打印预览,请稍等");
                    // 针对非病历类型的界面 如首页、三测单等
                    m_CurrentEmrEditor.Print();
                }

                //PrintForm printForm = new PrintForm(m_patUtil, m_CurrentModel, CurrentForm.zyEditorControl1.CurrentPage.PageIndex);
                //printForm.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 切换病人，可以供外部调用

        public void PatientChanging()
        {
            try
            {
                CloseAllTabPages();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PatientChanged(Inpatient inpatient)
        {
            try
            {
                m_CurrentInpatient = inpatient;
                m_CurrentInpatient.ReInitializeAllProperties();
                InitDictionary();
                xtraTabControl1.SelectedPageChanged -= new TabPageChangedEventHandler(xtraTabControl1_SelectedPageChanged);
                xtraTabControl1.CloseButtonClick -= new EventHandler(xtraTabControl1_CloseButtonClick);
                RemoveTabPageExcludFirst();
                xtraTabControl1.SelectedPageChanged += new TabPageChangedEventHandler(xtraTabControl1_SelectedPageChanged);
                xtraTabControl1.CloseButtonClick += new EventHandler(xtraTabControl1_CloseButtonClick);

                bool isOK = GetPatRec();
                if (!isOK) return;
                BindExtraInfo();
                SetBarStatus(false);
                SetBarVisible();

                if (m_NodeIEMMainPage != null) treeList1.FocusedNode = m_NodeIEMMainPage;
                //清空宏元素 Add By wwj 2012-05-14
                MacroUtil.MacroSource = null;
                //选中指定的病历
                SelectSpecifiedNode();
                //绑定个人模板
                InitPersonTree(true, string.Empty);
                //绑定病历信息
                InitBLMessage();
                CollapseContainEmrModelNode();
                ResetGroupControl();

                //显示历史病历导入界面
                if (IsAutoShowHistoryEmrBatchInForm())
                {
                    btnHistoryEmrBatchIn.Enabled = true;
                }
                else
                {
                    btnHistoryEmrBatchIn.Enabled = false;
                }
                this.FindForm().Activated += new EventHandler(UCEmrInput_Activated);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重新捞取右侧工具栏中的数据
        /// </summary>
        private void ResetGroupControl()
        {
            try
            {
                if (navBarControlClinical.ActiveGroup != null)
                {
                    treeListHistory.Nodes.Clear();
                    ActiveGroup(navBarControlClinical.ActiveGroup);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void PatientChangedByIEmrHost(decimal noofinpat)
        {
            try
            {
                CloseAllTabPages();
                PatientChanged(new Inpatient(noofinpat));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CloseAllTabPages()
        {
            try
            {
                for (int i = xtraTabControl1.TabPages.Count - 1; i >= 0; i--)
                {
                    if (i >= xtraTabControl1.TabPages.Count)
                    {
                        continue;
                    }
                    XtraTabPage page = xtraTabControl1.TabPages[i];
                    xtraTabControl1.SelectedTabPage = page;

                    if (CurrentForm != null)
                    {
                        XtraTabControlCloseButton();
                    }
                    else if (page.Controls.Count > 0 && page.Controls[0] is IEMREditor)
                    {
                        //IEMREditor emrEditor = page.Controls[0] as IEMREditor;
                        //emrEditor.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*
        /// <summary>
        /// 关闭所有页面的时候调用
        /// </summary>
        private void CloseAllTabPage()
        {
            try
            {
                bool isShowMessageBox = false;
                foreach (KeyValuePair<EmrModel, XtraTabPage> item in m_TempModelPages)
                {
                    XtraTabPage tabPage = item.Value as XtraTabPage;
                    if (tabPage != null)
                    {
                        if (tabPage.Controls.Count > 0 && tabPage.Controls[0] is PadForm)
                        {
                            PadForm padForm = tabPage.Controls[0] as PadForm;
                            if (padForm.zyEditorControl1.EMRDoc.Modified)
                            {
                                if (!isShowMessageBox)
                                {
                                    if (m_app.CustomMessageBox.MessageShow("有未保存的文件，确定要保存吗？",
                                        DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) != System.Windows.Forms.DialogResult.Yes) return;

                                    isShowMessageBox = true;
                                }

                                EmrModel emrModel = item.Key;
                                if (emrModel != null)
                                {
                                    ////【1】切换页面，计算当前页面的m_Current...
                                    //SelectedPageChanged(tabPage, false);
                                    ////【2】保存
                                    //if (m_CurrentModel != null)
                                    //{
                                    //    m_CurrentModel.ModelContent = padForm.zyEditorControl1.BinaryToXml(padForm.zyEditorControl1.SaveBinary());
                                    //    Save(m_CurrentModel);
                                    //}

                                    emrModel.ModelContent = padForm.zyEditorControl1.BinaryToXml(padForm.zyEditorControl1.SaveBinary());

                                    if (!emrModel.DailyEmrModel)
                                    {
                                        SaveDocumentInner(emrModel);
                                    }
                                    else
                                    {
                                        TreeListNode treeListNode = m_CurrentTreeListNode.Clone() as TreeListNode;
                                        GetTreeListNodeByEmrModel(null, emrModel, ref treeListNode);
                                        if (treeListNode != null)
                                        {
                                            foreach (TreeListNode node in treeListNode.ParentNode.Nodes)
                                            {
                                                SaveDocumentInner((EmrModel)node.Tag);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (isShowMessageBox)
                {
                    m_app.CustomMessageBox.MessageShow("保存成功", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                }
            }
            catch(Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("保存病历出错", DrectSoft.Core.CustomMessageBoxKind.ErrorYes);
            }
        }

        private void GetTreeListNodeByEmrModel(TreeListNodes nodes, EmrModel emrModel, ref TreeListNode node)
        {
            if (nodes == null)
            {
                nodes = treeList1.Nodes;
            }

            foreach (TreeListNode subNode in nodes)
            {
                if (subNode.Tag is EmrModel && (EmrModel)subNode.Tag == emrModel)
                {
                    node = subNode;
                }
                GetCurrentTreeListNode(subNode.Nodes, emrModel);
            }
        }
    */

        private void RemoveTabPageExcludFirst()
        {
            try
            {
                int pageCount = xtraTabControl1.TabPages.Count;
                if (pageCount > 0)
                {
                    if (pageCount > 1)
                    {
                        for (int i = pageCount - 1; i > 0; i--)
                        {
                            xtraTabControl1.TabPages.RemoveAt(i);
                        }
                    }
                    xtraTabControl1.TabPages[0].Controls.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region 隐藏工具栏，供外部调用, 如：质控部分对病历的调用
        public void HideBar()
        {
            try
            {
                //bar1.Visible = false;
                bar2.Visible = false;
                bar3.Visible = false;
                barBtnPrint.Visibility = BarItemVisibility.Always;
                panelContainer1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                m_RightMouseButtonFlag = false;
                m_EditorEnableFlag = false;
                m_IsNeedActiveEditArea = false;
                xtraTabPageIEMMainPage.AccessibleName = "不能编辑";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 三级检诊人员设置 add by wwj 2011-09-06
        private void barButtonItemThreeLevelCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                UserSettingForm userSettingForm = new UserSettingForm(this.m_app);
                userSettingForm.SetDepartmentNonEnableFlag();
                userSettingForm.StartPosition = FormStartPosition.CenterScreen;
                userSettingForm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 绑定大病历中的信息到工具栏中
        /// <summary>
        /// 加载住院志中可替换的节点信息 add by yxy 2011-10-11
        /// </summary>
        private void InitBLMessage()
        {
            try
            {
                //获得病历中所有可替换元素列表
                //ArrayList al = new ArrayList();
                //ZYTextDocument doc = CurrentForm.zyEditorControl1.EMRDoc;
                //doc.GetAllSpecElement(al, doc.RootDocumentElement, ElementType.Replace, null);

                //循环每个可替换元素，根据可替换元素的Name属性，查询并赋值线Text属性
                //if (al.Count < 0) return;
                //PadForm padForm = GetMainRecord();
                treeListBL.Nodes.Clear();
                PadForm padForm = GetSourcePadFormForReplaceItem("首次病程");
                if (padForm == null) return;

                DataTable dt = new DataTable();
                DataColumn dcName = new DataColumn("NAME", Type.GetType("System.String"));
                DataColumn dcContent = new DataColumn("CONTENT", Type.GetType("System.String"));

                dt.Columns.Add(dcName);
                dt.Columns.Add(dcContent);

                DataRow newrow = dt.NewRow();


                foreach (DataRow row in m_patUtil.BLItemTemplate.Rows)
                {
                    newrow = dt.NewRow();
                    newrow["NAME"] = row["名称"];
                    newrow["CONTENT"] = padForm.zyEditorControl1.EMRDoc.GetReplaceText(row["名称"].ToString());
                    if (newrow["CONTENT"].ToString().Length == 0)
                        continue;
                    dt.Rows.Add(newrow);
                    TreeListNode node = treeListBL.AppendNode(new object[] { row["名称"] }, null);
                    node.Tag = newrow;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 判断光标当前所在位置是否可以进行编辑操作
        /// </summary>
        /// <returns></returns>
        private bool CanEdit()
        {
            try
            {
                if (CurrentForm == null) return false;

                return CurrentForm.zyEditorControl1.CanEdit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region 添加病案首页
        /// <summary>
        /// 保存界面中TreeList中的“病案首页”节点
        /// </summary>
        TreeListNode m_NodeIEMMainPage;

        /// <summary>
        /// 加载病案首页
        /// </summary>
        private void LoadIEMMainPage()
        {
            try
            {
                m_NodeIEMMainPage = null;
                GetIEMMainPageNode(null);
                if (m_NodeIEMMainPage != null)
                {
                    EmrModelContainer container = (EmrModelContainer)m_NodeIEMMainPage.Tag;
                    if (container != null)
                    {
                        CreateCommonDocement(container);
                        //CreateFirstPageDocement(container);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获取TreeList中“病案首页”节点
        /// </summary>
        /// <param name="node"></param>
        private void GetIEMMainPageNode(TreeListNode node)
        {
            try
            {
                TreeListNodes nodes;
                if (node == null)
                {
                    nodes = treeList1.Nodes;
                }
                else
                {
                    nodes = node.Nodes;
                }

                for (int i = 0; i < nodes.Count; i++)
                {
                    TreeListNode subNode = nodes[i];
                    if (subNode.Tag != null && subNode.Tag is EmrModelContainer)
                    {
                        EmrModelContainer container = subNode.Tag as EmrModelContainer;

                        //找到“病案首页”对应的树节点 
                        //【程序集的名称 + 继承了IEMREditor接口的界面， 例如：DrectSoft.Core.IEMMainPage.UCMainPage, DrectSoft.Core.IEMMainPage】
                        if (container.ContainerCatalog == ContainerCatalog.BingAnShouYe)
                        {
                            m_NodeIEMMainPage = subNode;
                            break;
                        }
                    }
                    GetIEMMainPageNode(subNode);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region ***************未使用的方法*************************
        /// <summary>
        /// 将现有文件插入到当前编辑器中，未使用
        /// </summary>
        private void InsertImage()
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog dlgFile = new System.Windows.Forms.OpenFileDialog())
                {
                    dlgFile.Filter = "图像文件 (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|所有文件 (*.*)|*.*";
                    dlgFile.Title = "插入图片";

                    dlgFile.ShowReadOnly = false;
                    dlgFile.ShowHelp = false;
                    dlgFile.CheckFileExists = true;
                    dlgFile.ReadOnlyChecked = false;
                    if (dlgFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {

                        System.Drawing.Image img = System.Drawing.Image.FromFile(dlgFile.FileName);

                        MemoryStream ms = new MemoryStream();
                        byte[] imagedata = null;
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                        imagedata = ms.GetBuffer();

                        CurrentForm.zyEditorControl1.EMRDoc._InsertImage(imagedata);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 全部保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void ExportDocment(string caption)
        {
            try
            {
                using (System.Windows.Forms.SaveFileDialog dlg = new SaveFileDialog())
                {
                    string filename = caption;
                    //if (filename.Substring(filename.IndexOf('.') + 1) == "t")
                    //{
                    //    dlg.Filter = "页眉模板|*.t";
                    //}
                    //else //(filename.Substring(filename.IndexOf('.') + 1) == "xml")
                    //{
                    //    dlg.Filter = "电子病历文件|*.b";
                    //}
                    dlg.Filter = "XML文件|*.xml";
                    if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        CurrentForm.zyEditorControl1.EMRDoc.ToXMLFile(dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void BindOtherInfo()
        {
            try
            {
                gridControlTool.DataSource = m_patUtil.Symbols;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 诊疗计划
        private void barButtonItemDiagnosis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DiagLibForm libForm = new DiagLibForm(m_app, "诊疗计划", "2");
                libForm.ShowDialog();
                if (CurrentForm != null && libForm.GetDiag() != "")
                {
                    if (CanEdit())
                    {
                        CurrentForm.zyEditorControl1.EMRDoc._InserString(libForm.GetDiag());
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        /// <summary>
        /// 得到当前编辑器中显示病历对应的ID
        /// </summary>
        /// <returns></returns>
        public EmrModel GetCurrentModel()
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
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
                throw new Exception(ex.Message);
            }
        }

        public EmrModelContainer GetCurrentModelContainer()
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
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

        #region 【暂时不用，在点击删除的按钮后判断】
        /// <summary>
        /// 设置针对病程的删除按钮的使用情况
        /// </summary>
        /// <param name="node"></param>
        private void SetDailyEmrAllowDelete(TreeListNode node)
        {
            try
            {
                //如果没有权限删除则直接退出
                if (barButton_Delete.Enabled == false) return;

                //如果要删除的是病程，需要等到其他病程全部被删除，才能删除首次病程
                if (node.Tag is EmrModel && ((EmrModel)node.Tag).DailyEmrModel)
                {
                    EmrModel model = (EmrModel)node.Tag;
                    if (model.FirstDailyEmrModel == true && node.ParentNode.Nodes.Count == 1)
                    {
                        //点击首程，且病程中只有首程，则删除按钮可用
                        btnDeletePopup.Enabled = true;
                        barButton_Delete.Enabled = true;
                    }
                    else if (model.FirstDailyEmrModel == true && node.ParentNode.Nodes.Count > 1)
                    {
                        //点击首程，且病程中除了首程还有其他病程，则删除按钮不可用
                        btnDeletePopup.Enabled = false;
                        barButton_Delete.Enabled = false;
                    }
                    else if (model.FirstDailyEmrModel == false && node.ParentNode.Nodes.Count > 1)
                    {
                        //点击除首程外的其他病程，则删除按钮可用
                        btnDeletePopup.Enabled = true;
                        barButton_Delete.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 右侧工具栏病人历史病历

        /// <summary>
        /// 点击入院次数，右侧工具栏显示历次住院记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_InCount_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.panelContainer1.ActiveChild = this.dockPanel3;
                navBarGroup_history.Expanded = true;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        #region 历史病例批量导入
        private void btnHistoryEmrBatchIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string errorStr = CheckBeforeInport();
                if (!string.IsNullOrEmpty(errorStr))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(errorStr);
                    return;
                }
                ShowHistoryEmrBatchInForm();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void ShowHistoryEmrBatchInForm()
        {
            try
            {
                if (!bar2.Visible) return;

                if (!CheckIsDoctor())
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("只有医生开放此权限");
                    return;
                }

                int inCount = Convert.ToInt32(m_RecordDal.GetHistoryInCount((int)m_CurrentInpatient.NoOfFirstPage));
                if (inCount <= 1)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人无历史病历");
                    return;
                }
                else
                {
                    inCount = m_RecordDal.GetEmrCountByNoofinpat(m_CurrentInpatient.NoOfFirstPage.ToString());
                    if (inCount > 0)
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("该病人已经存在病历，无法导入。");
                        return;
                    }
                    List<EmrModel> mList = GetAllRecordModels(treeList1.Nodes, new List<EmrModel>());
                    if (null != mList && mList.Count() > 0 && mList.Any(p => p.InstanceId == -1))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您有新增的病历尚未保存，请删除后再导入。");
                        return;
                    }
                    ShowHistoryEmrBatchInFormInner();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 显示病历批量导入界面
        /// </summary>
        private void ShowHistoryEmrBatchInFormInner()
        {
            try
            {
                HistoryEmrBatchInForm barchInForm = new HistoryEmrBatchInForm(m_app, m_RecordDal, m_CurrentInpatient, m_patUtil);
                barchInForm.StartPosition = FormStartPosition.CenterScreen;
                barchInForm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 自动显示病历批量导入界面
        /// </summary>
        private void AutoShowHistoryEmrBatchInForm()
        {
            try
            {
                if (!bar2.Visible) return;

                if (IsAutoShowHistoryEmrBatchInForm())
                {
                    ShowHistoryEmrBatchInFormInner();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 判断是否需要自动显示病历批量导入界面
        /// </summary>
        /// <returns></returns>
        private bool IsAutoShowHistoryEmrBatchInForm()
        {
            try
            {
                #region 已注释
                //if (CheckIsDoctor())
                //{
                //    int inCount = Convert.ToInt32(m_RecordDal.GetHistoryInCount((int)m_CurrentInpatient.NoOfFirstPage));
                //    if (inCount > 1)
                //    {
                //        inCount = m_RecordDal.GetEmrCountByNoofinpat(m_CurrentInpatient.NoOfFirstPage.ToString());
                //        if (inCount == 0)
                //        {
                //            return true;
                //        }
                //    }
                //}
                //return false;
                //原来是根据是否为医生判断是否自动显示病历导入界面
                #endregion

                //现在更改为从配置中取得再加上是否是医生等判断，在原有判断加一层判断 add by ywk 2012年12月7日10:47:36 
                string IsShowExportHistory = GetConfigValueByKey("IsShowExportHistory");
                if (IsShowExportHistory == "1" && CheckIsDoctor())
                {
                    //获取当前病人病历数(不包含'AI','AJ','AK')
                    int recordCounts = m_RecordDal.GetEmrCountByNoofinpat(m_CurrentInpatient.NoOfFirstPage.ToString());
                    if (recordCounts == 0)
                    {//无病历
                        DataTable inpHistorys = GetInpHistorys((int)m_CurrentInpatient.NoOfFirstPage, "", "");
                        if (null != inpHistorys && inpHistorys.Rows.Count > 0)
                        {
                            return true;
                        }
                    }

                    #region 原历史病历判断方法 已注释 by cyq 2013-03-01
                    //获取该病人的住院次数
                    //int inCount = Convert.ToInt32(m_RecordDal.GetHistoryInCount((int)m_CurrentInpatient.NoOfFirstPage));
                    //if (inCount > 1)
                    //{
                    //    inCount = m_RecordDal.GetEmrCountByNoofinpat(m_CurrentInpatient.NoOfFirstPage.ToString());
                    //    if (inCount == 0 && IsShowExportHistory == "1")
                    //    {
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        return false;
                    //    }
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                    #endregion
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 导入历史病历验证
        /// </summary>
        /// <returns></returns>
        private string CheckBeforeInport()
        {
            try
            {
                string IsShowExportHistory = GetConfigValueByKey("IsShowExportHistory");
                if (IsShowExportHistory != "1")
                {
                    return "对不起，历史病历导入功能尚未开放。";
                }
                if (!CheckIsDoctor())
                {
                    return "对不起，历史病历导入功能只对医生开放。";
                }
                //获取当前病人病历数(不包含'AI','AJ','AK')
                int recordCounts = m_RecordDal.GetEmrCountByNoofinpat(m_CurrentInpatient.NoOfFirstPage.ToString());
                if (recordCounts > 0)
                {
                    return "该病人已存在病历，无法导入历史病历。";
                }
                DataTable inpHistorys = GetInpHistorys((int)m_CurrentInpatient.NoOfFirstPage, "", "");
                if (null == inpHistorys || inpHistorys.Rows.Count == 0)
                {
                    return "该病人没有历史病历";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取该病人历史记录(不包含无病历的入院记录)
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <returns></returns>
        public DataTable GetInpHistorys(int noofinpat, string dateBegin, string dateEnd)
        {
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[]
                {
                    new SqlParameter("NOOFINPAT", SqlDbType.Int),
                    new SqlParameter("admitbegindate",SqlDbType.VarChar),
                    new SqlParameter("admitenddate",SqlDbType.VarChar)
                };
                sqlParams[0].Value = noofinpat;
                sqlParams[1].Value = dateBegin;
                sqlParams[2].Value = dateEnd;
                return m_app.SqlHelper.ExecuteDataTable("emr_record_input.usp_getHistoryInpatient", sqlParams, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 复用元素替换 Add By wwj 2012-05-14
        private void barButtonItemReplaceItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!CheckIsDoctor())
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("此权限只对医生开放");
                    return;
                }
                ReplaceItemForAllEmrRecord();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void ReplaceItemForAllEmrRecord()
        {
            try
            {
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("是否重新替换所有复用元素？", "替换复用元素", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    RefreshEMRMainPad();

                    DataTable dt = m_RecordDal.GetEmrRecordByNoofinpat(m_CurrentInpatient.NoOfFirstPage.ToString());

                    m_TreeAllNode = new List<TreeListNode>();
                    GetTreeAllNodeForReplaceItem(treeList1.Nodes);

                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (TreeListNode node in m_TreeAllNode)
                        {
                            EmrModel model = node.Tag as EmrModel;
                            if (model != null)
                            {
                                if (dr["id"].ToString() == model.InstanceId.ToString())
                                {
                                    treeList1.FocusedNode = node;
                                    ReplaceModelMacro(model);
                                }
                            }
                        }
                    }

                    RefreshEMRMainPad();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 重新加载整个文书录入界面
        /// </summary>
        private void RefreshEMRMainPad()
        {
            try
            {
                m_app.ChoosePatient(m_CurrentInpatient.NoOfFirstPage);
                m_app.LoadPlugIn("DrectSoft.Core.MainEmrPad.dll", DS_BaseService.GetUCEmrInputPath());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 病历树的所有节点
        /// </summary>
        private List<TreeListNode> m_TreeAllNode = new List<TreeListNode>();

        /// <summary>
        /// 得到树中所有节点用于复用元素
        /// </summary>
        /// <param name="nodes"></param>
        private void GetTreeAllNodeForReplaceItem(TreeListNodes nodes)
        {
            try
            {
                foreach (TreeListNode node in nodes)
                {
                    if (node.Nodes.Count > 0)
                    {
                        GetTreeAllNodeForReplaceItem(node.Nodes);
                    }
                    else
                    {
                        if ((!node.HasChildren) && (node.Tag is EmrModel))//如果是模板类，则说明是叶子节点
                        {
                            m_TreeAllNode.Add(node);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 切换全局字体默认大小
        private void barEditItemDefaultFontSize_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                ZYEditorControl.DefaultFontSize = barEditItemDefaultFontSize.EditValue.ToString();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 复制第一页的护理表格，并插入到光标的当前位置【由于编辑器显示和打印的Bug暂不开放使用】
        private void barButtonItemAddNurseTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("确定要插入第一个表格吗？", "插入表格", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.InsertTableForNurseDocument();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 圈字
        private void barButtonItemQuanZi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CanEdit())
                {
                    this.CurrentForm.zyEditorControl1.EMRDoc.SetCircleFont(this.barButtonItemQuanZi.Down);
                    this.CurrentForm.zyEditorControl1.EMRDoc.Refresh();
                    this.CurrentForm.zyEditorControl1.Refresh();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 更改当前病历中宏元素的数据【现在只开放修改“科室”“病区”的宏元素】
        private void barButtonItemChangeMacro_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //if (CurrentForm != null && m_CurrentModel != null)
                //{
                //    ChangeMacroFrom changeMacro = new ChangeMacroFrom(m_app, CurrentForm, m_CurrentInpatient);
                //    changeMacro.StartPosition = FormStartPosition.CenterScreen;
                //    if (changeMacro.ShowDialog() == DialogResult.Yes)
                //    {
                //        if (Common.Ctrs.DLG.MessageBox.Show("确定要保存到数据库中吗？", "提示信息", Common.Ctrs.DLG.MessageBoxButtons.YesNo) == DialogResult.Yes)
                //        {
                //            SaveDocment(m_CurrentModel);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 点击DockPanel之后更改对应的名称
        private void panelContainer1_ActiveChildChanged(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            try
            {
                if (panelContainer1.ActiveChild == dockPanel2)
                {
                    this.navBarControlToolBox.ActiveGroup = this.navBarGroup_Image;
                    navBarGroup_Image.Expanded = true;
                    ShowTipForNavBarGroup(navBarGroup_Image);
                }
                else if (panelContainer1.ActiveChild == dockPanel3)
                {
                    this.navBarControlClinical.ActiveGroup = this.navBarGroup_Bl;
                    //navBarGroup_Bl.Expanded = true;
                    // ShowTipForNavBarGroup(navBarGroup_Bl);
                    //zyx 2012-01-05 临床数据提取所在的TAB页签默认选中
                    navBarGroup_Jy.Expanded = true;
                    ShowTipForNavBarGroup(navBarGroup_Jy);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 出科按钮的逻辑操作
        private bool CheckRight = true;//出科检查是否检查通过
        IemMainPageManger m_IemMainPage;
        /// <summary>
        /// 病案首页病患信息
        /// </summary>
        public DrectSoft.Core.IEMMainPage.IemMainPageInfo IemInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 出科检查按钮
        /// add by ywk 2012年7月27日 10:18:32
        /// 此处调用一些检查方法，如果验证通过就把病人的flag值更新为1，否则为0，以供HIS，取出flag，决定是否取我们的视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //GOMyCheck();
        }

        public void GOMyCheck()
        {
            try
            {
                string showcontent = string.Empty;
                showcontent = GetReturnContent();
                //string showcontent = string.Empty;
                //CheckInpatient(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), out showcontent);
                if (CheckRight)
                {
                    //仁和医院和中心医院病历验证通过代码相反
                    if (GetConfigValueByKey("HosCode") == "1")
                    {
                        UpdateFlag(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), "1");
                    }
                    else if (GetConfigValueByKey("HosCode") == "0")
                    {
                        UpdateFlag(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), "0");
                    }
                    //CheckTipContent checkT = new CheckTipContent(m_app, "验证通过！", this);
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("验证通过");
                }
                else
                {
                    if (showcontent == "")
                    {
                        if (GetConfigValueByKey("HosCode") == "1")
                        {
                            UpdateFlag(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), "1");
                        }
                        else if (GetConfigValueByKey("HosCode") == "0")
                        {
                            UpdateFlag(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), "0");
                        }
                        //CheckTipContent checkT = new CheckTipContent(m_app, "验证通过！", this);
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("验证通过");
                    }
                    else
                    {
                        if (GetConfigValueByKey("HosCode") == "1")
                        {
                            UpdateFlag(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), "0");
                        }
                        else if (GetConfigValueByKey("HosCode") == "0")
                        {
                            UpdateFlag(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), "1");
                        }

                        //if (m_CheckT == null)
                        //{
                        m_CheckT = new CheckTipContent(m_app, showcontent, this);
                        m_CheckT.TopMost = true;
                        //m_CheckT.StartPosition = FormStartPosition.WindowsDefaultBounds;
                        //Point m_point=new Point (
                        int workWidth = Screen.PrimaryScreen.WorkingArea.Width;
                        int thisWidth = m_CheckT.Width;
                        int m_Width = workWidth - thisWidth;
                        m_CheckT.Location = new Point(m_Width, 100);
                        m_CheckT.StartPosition = FormStartPosition.Manual;
                        m_CheckT.Show();
                    }
                    //}
                    //else
                    //{
                    //    m_CheckT.RefreshContent(showcontent);
                    //    m_CheckT.Show();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GetReturnContent()
        {
            try
            {
                string showcontent = string.Empty;
                CheckInpatient(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), out showcontent);
                return showcontent;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        CheckTipContent m_CheckT;
        /// <summary>
        /// 验证通过将此病人的flag更为0.供HIS使用 
        /// add by ywk 2012年7月27日 14:09:20
        /// </summary>
        /// <param name="p"></param>
        private void UpdateFlag(string noofinpat, string flag)
        {
            try
            {
                string upsql = string.Format(@"update iem_mainpage_basicinfo_2012 set PatFlag='{0}' where noofinpat='{1}' and valide='1' ", flag, noofinpat);
                m_app.SqlHelper.ExecuteNoneQuery(upsql, CommandType.Text);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
                return;
            }
        }

        ///// <summary>
        ///// 传入病人的首页序号 判断各个情况
        ///// add by ywk 
        ///// </summary>
        ///// <param name="noofinpat"></param>
        //public void CheckOutDept(string noofinpat)
        //{
        //    string TipMessage = string.Empty;//要返回的提示信息
        //    CheckInpatient(noofinpat, out TipMessage);
        //}
        /// <summary>
        ///  传入病人的首页序号 判断各个情况
        /// </summary>
        /// <param name="noofinpat"></param>
        /// <param name="TipMessage"></param>
        private void CheckInpatient(string noofinpat, out string TipMessage)
        {
            try
            {
                m_IemMainPage = new IemMainPageManger(m_app, m_app.CurrentPatientInfo);
                TipMessage = string.Empty;
                string ReturnMessage = string.Empty;//要返回的提示信息

                bool CheckInDate = false;
                bool CheckOutDate = false;
                //DataTable m_Dt = m_app.PatientInfos.Tables[0];//此处数据要重新构建病人表， m_app.PatientInfos.Tables[0】取的是在院人员  
                DataTable m_Dt = GetInpatient();//此处数据要重新构建病人表， m_app.PatientInfos.Tables[0】取的是在院人员 

                DataTable InpatientDt = m_Dt.Clone();//存储病人信息的表数据
                DataRow[] filterrow = m_Dt.Select("noofinpat='" + m_app.CurrentPatientInfo.NoOfFirstPage + "'");
                if (filterrow.Length > 0)
                {
                    for (int i = 0; i < filterrow.Length; i++)
                    {
                        InpatientDt.ImportRow(filterrow[i]);
                    }
                }
                //0221:  messagebox("系统提示","入院日期不能大于出院日期！")
                string AdmitHosDate1 = InpatientDt.Rows[0]["ADMITDATE"].ToString();//入院时间
                string OutHosDate1 = InpatientDt.Rows[0]["OUTHOSDATE"].ToString();//出院时间
                if (string.IsNullOrEmpty(AdmitHosDate1))//入院日期为空
                {
                    ReturnMessage += "入院日期不能为空\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                    //return;
                }
                else
                {
                    CheckInDate = true;
                }
                if (string.IsNullOrEmpty(OutHosDate1))//出院日期为空
                {
                    ReturnMessage += "出院日期不能为空\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                    //return;
                    //CheckOutDate = true;
                }
                else
                {
                    CheckOutDate = true;
                }

                if (CheckInDate && CheckOutDate)
                {
                    DateTime AdmitHosDate = DateTime.Parse(AdmitHosDate1);
                    DateTime OutHosDate = DateTime.Parse(OutHosDate1);
                    if (AdmitHosDate > OutHosDate)//入院日期大于出院日期！
                    {
                        ReturnMessage += "入院日期不能大于出院日期\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                        //return;
                    }
                }

                //messagebox("提示","此患者并非在院患者")
                //string statusid = InpatientDt.Rows[0]["STATUS"].ToString();
                //if (statusid == "1502" || statusid == "1503")
                //{
                //    ReturnMessage += "此患者并非在院患者！\n\r";
                //    CheckRight = false;
                //    TipMessage = ReturnMessage;
                //}

                //0227:  messagebox("系统提示","确诊日期应该在入院日期与出院日期之间！")
                //0233:  messagebox("提示","住院天数不能为负数！")
                string m_totaldays = InpatientDt.Rows[0]["TOTALDAYS"].ToString();
                int I_TotalDay = Int32.Parse(m_totaldays == "" ? "0" : m_totaldays);
                if (string.IsNullOrEmpty(m_totaldays))//住院天数为空
                {
                    ReturnMessage += "住院天数不能为空\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                    //return;
                }
                if (I_TotalDay < 0)
                {
                    ReturnMessage += "住院天数不能为负数\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                    //return;
                }
                if (I_TotalDay == 0)
                {
                    ReturnMessage += "住院天数不能为0\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;

                }
                //0240:  messagebox("系统提示","出生日期不应该大于入院日期!")
                DateTime m_DtBirthDay = DateTime.Parse(InpatientDt.Rows[0]["BIRTH"].ToString());
                if (!string.IsNullOrEmpty(AdmitHosDate1))
                {
                    DateTime AdmitHosDate = DateTime.Parse(AdmitHosDate1);
                    if (m_DtBirthDay > AdmitHosDate)//出生日期大于入院日期
                    {
                        ReturnMessage += "出生日期不能大于入院日期\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                        //return;
                    }
                }
                //0247:  messagebox("提示","请在出院卡片首页中输入主要诊断")
                IemInfo = m_IemMainPage.GetIemInfo();
                if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count < 0)
                {
                    ReturnMessage += "出院卡片首页中主要诊断不能为空\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                    //return;
                }
                //MessageBox('提示','年龄只能在0和100之间！')
                int m_Age = Int32.Parse(InpatientDt.Rows[0]["AGE"].ToString());
                if (m_Age < 0 || m_Age > 100)
                {
                    ReturnMessage += "年龄只能是0至100之间的整数\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                    //return;
                }
                //MessageBox('提示','诊断符合情况只能输入0到3之间的数字！')
                int m_MenAndInHop = Int32.Parse(IemInfo.IemBasicInfo.MenAndInHop.ToString() == "" ? "10" : IemInfo.IemBasicInfo.MenAndInHop.ToString());
                int m_InHopAndOutHop = Int32.Parse(IemInfo.IemBasicInfo.InHopAndOutHop.ToString() == "" ? "10" : IemInfo.IemBasicInfo.InHopAndOutHop.ToString());
                int m_BeforeOpeAndAfterOper = Int32.Parse(IemInfo.IemBasicInfo.BeforeOpeAndAfterOper.ToString() == "" ? "10" : IemInfo.IemBasicInfo.BeforeOpeAndAfterOper.ToString());
                int m_LinAndBingLi = Int32.Parse(IemInfo.IemBasicInfo.LinAndBingLi.ToString() == "" ? "10" : IemInfo.IemBasicInfo.LinAndBingLi.ToString());
                int m_InHopThree = Int32.Parse(IemInfo.IemBasicInfo.InHopThree.ToString() == "" ? "10" : IemInfo.IemBasicInfo.InHopThree.ToString());
                int m_FangAndBingLi = Int32.Parse(IemInfo.IemBasicInfo.FangAndBingLi.ToString() == "" ? "10" : IemInfo.IemBasicInfo.FangAndBingLi.ToString());
                if (m_MenAndInHop < 0 || m_MenAndInHop > 4 || m_InHopAndOutHop < 0 || m_InHopAndOutHop > 4 || m_BeforeOpeAndAfterOper < 0 || m_BeforeOpeAndAfterOper > 4 || m_LinAndBingLi < 0 || m_LinAndBingLi > 4 || m_InHopThree < 0 || m_InHopThree > 4 || m_FangAndBingLi < 0 || m_FangAndBingLi > 4)
                {
                    ReturnMessage += "诊断符合情况只能输入0到4之间的数字\n\r";//新版首页（1、符合 2、不符合 3、不肯定 4、未做）
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                    //return;
                }
                if (m_MenAndInHop == 10 || m_InHopAndOutHop == 10 || m_BeforeOpeAndAfterOper == 10 || m_LinAndBingLi == 10 || m_InHopThree == 10 || m_FangAndBingLi == 10)
                {
                    ReturnMessage += "诊断符合情况不能为空\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                //MessageBox('提示','诊断符合情况有输入[误诊]的选项,是否继续?',Question!,YesNo! ,1) 
                ArrayList DiagInfo = new ArrayList();
                DiagInfo.Add(IemInfo.IemBasicInfo.MenAndInHop.ToString());
                DiagInfo.Add(IemInfo.IemBasicInfo.InHopAndOutHop.ToString());
                DiagInfo.Add(IemInfo.IemBasicInfo.BeforeOpeAndAfterOper.ToString());
                DiagInfo.Add(IemInfo.IemBasicInfo.LinAndBingLi.ToString());
                DiagInfo.Add(IemInfo.IemBasicInfo.InHopThree.ToString());
                DiagInfo.Add(IemInfo.IemBasicInfo.FangAndBingLi.ToString());
                if (DiagInfo.Contains("2"))//存在误诊的情况//当前新版首页不存在误诊这个选项 add by ywk 2013年2月22日10:27:41  不符合就是误诊
                {
                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(string.Format("诊断符合情况有输入[不符合]的选项，您是否要继续？"), "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                    {
                        CheckRight = true;
                    }
                    else
                    {
                        CheckRight = false;
                    }
                }
                string cansee = DS_SqlService.GetConfigValueByKey("EmrInputConfig");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(cansee);
                string pageContorlVisable = doc.GetElementsByTagName("IemPageContorlVisable")[0].InnerText;
                if (CheckRight)//上面判断有误诊选择了继续
                {
                    if (ReturnMessage.Length > 10)
                    {
                        CheckRight = false;
                    }
                    //MessageBox('提示','诊断符合情况中[门诊与住院]、[入院与出院]、[入院三日内]不能填[未做]')  
                    if (DiagInfo[0].ToString() == "4" || DiagInfo[1].ToString() == "4" || DiagInfo[4].ToString() == "4")
                    {
                        ReturnMessage += "诊断符合情况中[门诊与住院]、[入院与出院]、[入院三日内]不能填[未做]\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                        //return;
                    }
                    // MessageBox('提示','术前术后诊断必须与手术信息相对应！如果选择确诊必须填手术信息，如果未作则不能有手术信息！') 
                    if (DiagInfo[2].ToString() == "1" && IemInfo.IemOperInfo.Operation_Table.Rows.Count == 0)//是确诊，手术信息为空的话 
                    {
                        ReturnMessage += "术前和术后手术信息为确诊，但无手术信息。\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                        //return;
                    }
                    if (DiagInfo[2].ToString() == "0" && IemInfo.IemOperInfo.Operation_Table.Rows.Count > 0)//是未作，手术信息有数据的话 
                    {
                        ReturnMessage += "术前和术后手术信息为未作，但有手术信息。\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                        //return;
                    }
                    // 0376:  Messagebox("提示","请给出诊断的转归代码")
                    //messagebox("提示","此患者并非在院患者")
                    //            --如果有手术
                    //0066:   Gf_msg('手术日期不能为空!',211)
                    DataTable DtOper = IemInfo.IemOperInfo.Operation_Table;
                    if (DtOper.Rows.Count > 0)
                    {
                        for (int i = 0; i < DtOper.Rows.Count; i++)
                        {
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Operation_Date"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但手术日期不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                                //return;
                            }
                            //0076:   Gf_msg('是否择期手术不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCHOOSEDATE"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否择期手术不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                    //return;
                                }
                            }
                            //    0081:   Gf_msg('是否无菌手术不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCLEAROPE"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否无菌手术不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                    //return;
                                }
                            }
                            //0086:   Gf_msg('麻醉方式不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Anaesthesia_Type_Id"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但麻醉方式不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                                //return;
                            }
                            //  0091:   Gf_msg('手术医师不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Execute_User1"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但手术医师不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                                //return;
                            }
                            //    0096:   Gf_msg('切口愈合方式不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Close_Level"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但切口愈合方式不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                                //return;
                            }
                            //0101:   Gf_msg('是否感染不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISGANRAN"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否感染不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                    //return;
                                }
                            }
                        }
                    }
                    else
                    {
                        ReturnMessage += "无手术信息。\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                }
                else//没选择误诊也要接着判断手术的信息
                {
                    DataTable DtOper = IemInfo.IemOperInfo.Operation_Table;
                    if (DtOper.Rows.Count > 0)
                    {
                        for (int i = 0; i < DtOper.Rows.Count; i++)
                        {
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Operation_Date"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但手术日期不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                                //return;
                            }
                            //0076:   Gf_msg('是否择期手术不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCHOOSEDATE"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否择期手术不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                    //return;
                                }
                            }
                            //    0081:   Gf_msg('是否无菌手术不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCLEAROPE"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否无菌手术不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                    //return;
                                }
                            }
                            //0086:   Gf_msg('麻醉方式不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Anaesthesia_Type_Id"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但麻醉方式不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                                //return;
                            }
                            //  0091:   Gf_msg('手术医师不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Execute_User1"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但手术医师不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                                //return;
                            }
                            //    0096:   Gf_msg('切口愈合方式不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Close_Level"].ToString() == "")
                            {
                                ReturnMessage += "有手术信息，但切口愈合方式不能为空。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                                //return;
                            }
                            //0101:   Gf_msg('是否感染不能为空！',211)
                            if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISGANRAN"].ToString() == "")
                            {
                                if (pageContorlVisable == "1")
                                {
                                    ReturnMessage += "有手术信息，但是否感染不能为空。\n\r";
                                    CheckRight = false;
                                    TipMessage = ReturnMessage;
                                    //return;
                                }
                            }
                        }
                    }
                    else
                    {
                        ReturnMessage += "无手术信息。\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                }

                //如果主诊断是关于生孩子的诊断，则需要判断以下内容
                //*********注：（diagnosis_type_id=7是表示主诊断，8是标识其他诊断 ）******* 
                //  如果主诊断icd码第一位是S或T开头，2，3位编码小于90的必须输外部损伤原因
                //    0034:  MessageBox('提示','你录入的婴儿数量与维护的不符，请点击多胎~r~n按钮维护婴儿记录！')
                string M_DiagICD = string.Empty;//用于存放主诊断的ICD编码

                if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count > 0)
                {
                    DataRow[] drrow = IemInfo.IemDiagInfo.OutDiagTable.Select(" diagnosis_type_id=7 ");
                    if (drrow.Length > 0)
                    {
                        M_DiagICD = drrow[0]["Diagnosis_Code"].ToString();
                    }
                    if (M_DiagICD.StartsWith("S") || M_DiagICD.StartsWith("T"))
                    {
                        int SencondThirdDiag = Int32.Parse(M_DiagICD.Substring(1, 2));
                        if (SencondThirdDiag < 90)
                        {
                            if (string.IsNullOrEmpty(IemInfo.IemDiagInfo.Hurt_Toxicosis_ElementID))
                            {
                                ReturnMessage += "请输入外部损伤原因。\n\r";
                                CheckRight = false;
                                TipMessage = ReturnMessage;
                            }
                        }
                    }

                    //if (IemInfo.IemDiagInfo.)
                    //{

                    //}
                }

                ArrayList BabyDiagArr = new ArrayList();//存储关于生孩子的诊断信息
                DataTable dtDiagBaby = GetBabyDiagInfo();
                bool ISAboutBaby = false;//此变量标识诊断是否是生孩子的诊断
                if (!string.IsNullOrEmpty(M_DiagICD))//表明主诊断有值
                {
                    for (int i = 0; i < dtDiagBaby.Rows.Count; i++)
                    {
                        if (M_DiagICD.ToString() == dtDiagBaby.Rows[i]["icd"].ToString())
                        {
                            ISAboutBaby = true;
                            break;
                        }
                        else
                        {
                            ISAboutBaby = false;
                            //return;
                        }
                    }
                }

                if (ISAboutBaby)
                {
                    //    0049:   messagebox("提示","出院日期大于当前日期10天以上，不能录入！")
                    if (!string.IsNullOrEmpty(OutHosDate1)) //出院时间
                    {
                        DateTime OutHosDate = DateTime.Parse(OutHosDate1);
                        DateTime NowDate = DateTime.Now;
                        TimeSpan timeSpan = OutHosDate - NowDate;//两个时间相减 
                        if (timeSpan.Days > 10)
                        {
                            ReturnMessage += "出院日期大于当前日期10天以上，不能录入。\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                        }
                    }
                    //    0063:   messagebox("注意","该患者住院天数超过50天！")
                    if (I_TotalDay > 50)
                    {
                        ReturnMessage += "该患者住院天数超过50天。\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    //    0041:   messagebox('提示','请输入损伤外部原因!')
                    if (string.IsNullOrEmpty(IemInfo.IemDiagInfo.Hurt_Toxicosis_ElementID))
                    {
                        ReturnMessage += "损伤外部原因不能为空\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }

                    //  0029:  messagebox("提示","请输入婴儿出生日期")
                    if (string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.BithDay))
                    {
                        ReturnMessage += "请选择婴儿出生日期\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    //    0047:   MessageBox('提示','请在出院卡片Ⅱ中录入完整的妇婴信息！')
                    if (string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.APJ) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.BithDayPrint) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.CC) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.CCQK) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.CFHYPLD) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.FMFS) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.Heigh))
                    {
                        ReturnMessage += "请在出院卡片Ⅱ中录入完整的妇婴信息\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    //    0122:   messagebox("提示","请给出诊断的转归代码")
                    if (string.IsNullOrEmpty(IemInfo.IemBasicInfo.ZG_FLAG))
                    {
                        ReturnMessage += "请给出诊断的转归代码\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    //    0011:  messagebox("提示","该患者没有诊断")
                    if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count == 0)
                    {
                        ReturnMessage += "该患者没有诊断\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                    //    0039:   messagebox("提示","请选择诊断类别")
                    if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(IemInfo.IemDiagInfo.OutDiagTable.Rows[0]["diagnosis_type_id"].ToString()))
                        {
                            ReturnMessage += "请选择诊断类别\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                        }
                    }
                }
                //    0073:   MessageBox('提示','转归代码为死亡时，其所有的疾病情况~r~n应均为死亡！')
                //if (IemInfo.IemBasicInfo.ZG_FLAG=="4"&& IemInfo.IemBasicInfo.)
                //{

                //}
                TipMessage = ReturnMessage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// /获得有关生孩子的诊断
        /// 现在的有关生孩子的诊断都是存放在数据库zx_diagsaboutbaby表中
        /// </summary>
        /// <returns></returns>
        private DataTable GetBabyDiagInfo()
        {
            try
            {
                string searchip = string.Format("select * from zx_diagsaboutbaby where valid='1' ");
                return m_app.SqlHelper.ExecuteDataTable(searchip, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 获得所有病人
        /// </summary>
        /// <returns></returns>
        private DataTable GetInpatient()
        {
            try
            {
                string searchip = string.Format("select * from inpatient");
                return m_app.SqlHelper.ExecuteDataTable(searchip, CommandType.Text);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        /// <summary>
        /// 新增的调用PACS图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string HospitalNo = m_CurrentInpatient.RecordNoOfHospital;//住院号
            //int nPatientType = 2;//患者类型（1.门诊号 2.住院号）
            //int LookType = 1;//类型（1.图像 2.报告）

            //if (CheckPackIsExist())
            //{

            //    //string jointpath = Application.StartupPath + @"\joint.dll";//获取程序所在文件夹
            //    //if (!File.Exists(jointpath))//不存在此接口DLL就不执行 
            //    //{
            //    //    m_app.CustomMessageBox.MessageShow("调用失败，缺少程序集joint.dll");
            //    //    return;
            //    //}
            //    //string connectpath = Application.StartupPath + @"\Connection.dll";//获取程序所在文件夹
            //    //if (!File.Exists(connectpath))//不存在此接口DLL就不执行 
            //    //{
            //    //    m_app.CustomMessageBox.MessageShow("调用失败，缺少程序集Connection.dll");
            //    //    return;
            //    //}
            //    //string pacspath = Application.StartupPath + @"\PACSID.dll";//获取程序所在文件夹
            //    //if (!File.Exists(pacspath))//不存在此接口DLL就不执行 
            //    //{
            //    //    m_app.CustomMessageBox.MessageShow("调用失败，缺少程序集PACSID.dll");
            //    //    return;
            //    //}

            //    try
            //    {
            //        //if (PacsInitialize() == 1)//启动
            //        {
            //            //MessageBox.Show("启动成功");
            //            //if (PacsViewByPatientInfo(LookType, HospitalNo, nPatientType))
            //            if (PacsView(nPatientType, m_CurrentInpatient.RecordNoOfHospital, LookType) == 1)
            //            {
            //                //MessageBox.Show("调用成功");
            //                //PacsRelease();//
            //            }
            //            else
            //            {
            //                MessageBox.Show("调用失败");
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        try
            //        {
            //            PacsClose();
            //            PacsStart();
            //        }
            //        catch (Exception ex1)
            //        { }

            //        MessageBox.Show(ex.Message);
            //    }
            //    finally
            //    {
            //    }
            //}
        }
        /// <summary>
        /// 判断调用PACS的DLL是否存在 
        /// </summary>
        private bool CheckPackIsExist()
        {
            try
            {
                string jointpath = Application.StartupPath + @"\joint.dll";//获取程序所在文件夹
                if (!File.Exists(jointpath))//不存在此接口DLL就不执行 
                {
                    //m_app.CustomMessageBox.MessageShow("调用失败，缺少程序集joint.dll");
                    return false;
                }
                string connectpath = Application.StartupPath + @"\Connection.dll";//获取程序所在文件夹
                if (!File.Exists(connectpath))//不存在此接口DLL就不执行 
                {
                    //m_app.CustomMessageBox.MessageShow("调用失败，缺少程序集Connection.dll");
                    return false;
                }
                string pacspath = Application.StartupPath + @"\PACSID.dll";//获取程序所在文件夹
                if (!File.Exists(pacspath))//不存在此接口DLL就不执行 
                {
                    //m_app.CustomMessageBox.MessageShow("调用失败，缺少程序集PACSID.dll");
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
        /// 导出PDF文档
        /// add by ywk 2012年8月17日 16:40:48
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnToPDF_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (CurrentForm == null) return;
            //using (System.Windows.Forms.SaveFileDialog dlg = new SaveFileDialog())
            //{
            //    string filename = CurrentForm.zyEditorControl1.Text;
            //    dlg.Filter = "JPG文件|*.jpg";
            //    if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            //    {

            //        #region 注释PDF
            //        //Document document = new Document();
            //        ////MemoryStream ms = new MemoryStream();
            //        ////m_CurrentModel.ModelContent.Save(ms);
            //        ////保存到MemoryStream
            //        ////CurrentForm.zyEditorControl1.EMRDoc.ToXMLFile(dlg.FileName);
            //        ////PdfWriter.GetInstance(document, ms);
            //        //PdfWriter.GetInstance(document, new FileStream(dlg.FileName.ToString(), FileMode.Create));

            //        //document.Open();
            //        //BaseFont bfChinese = BaseFont.CreateFont("C://WINDOWS//Fonts//simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//根据本地设置字体样式
            //        //BaseColor baseColor = new BaseColor(0, 0, 0);//设置颜色RGB
            //        //iTextSharp.text.Font fontChinese = new iTextSharp.text.Font(bfChinese, 12, iTextSharp.text.Font.NORMAL, baseColor);//设置字体的样式
            //        //Paragraph paragraph = new Paragraph("我是一行字", fontChinese);
            //        //paragraph.Alignment = Element.ALIGN_CENTER;
            //        //document.Add(paragraph);//输出一行字
            //        //Paragraph par = new Paragraph(" ", fontChinese);//隔离字于表之间的距离
            //        //document.Add(par);
            //        //int[] Widths = { 100, 200, 300, 400 };//设置每行的宽度
            //        //PdfPTable table = new PdfPTable(4);
            //        //table.SetWidths(Widths);
            //        //PdfPCell cell1 = new PdfPCell(new Phrase("号码1", fontChinese));
            //        //cell1.BackgroundColor = new BaseColor(255, 0, 0);
            //        //cell1.MinimumHeight = 20f;//设置一行高度
            //        //PdfPCell cell2 = new PdfPCell(new Phrase("号码2", fontChinese));
            //        //cell2.BackgroundColor = new BaseColor(255, 0, 0);
            //        //PdfPCell cell3 = new PdfPCell(new Phrase("号码3", fontChinese));
            //        //cell3.BackgroundColor = new BaseColor(255, 0, 0);
            //        //PdfPCell cell4 = new PdfPCell(new Phrase("号码4", fontChinese));
            //        //cell4.BackgroundColor = new BaseColor(255, 0, 0);
            //        //PdfPCell[] cells = { cell1, cell2, cell3, cell4 };
            //        //PdfPRow row = new PdfPRow(cells);//将列加入行中
            //        //table.Rows.Add(row);//插入一行数据
            //        ////for (int i = 0; i < PDFTable.Rows.Count; i++)
            //        ////{
            //        ////    for (int j = 0; j < PDFTable.Columns.Count; j++)
            //        ////    {
            //        //table.AddCell(new Phrase("as", fontChinese));
            //        ////    }
            //        ////}
            //        ////DataTable dt = new DataTable();
            //        ////DataTable datatable = m_patUtil.GetFolderInfo("00");
            //        ////document.Add(datatable);
            //        //document.Add(table);
            //        //MessageBox.Show("导出成功");
            //        //document.Close();
            //        #endregion
            //        #region 导出jpg(注释)
            //        Bitmap objBitmap = null;
            //        Graphics g = null;
            //        Font stringFont = new Font("宋体", 12, FontStyle.Bold);
            //        StringFormat stringFormat = new StringFormat();
            //        stringFormat.FormatFlags = StringFormatFlags.NoWrap;
            //        objBitmap = new Bitmap(1, 1);
            //        g = Graphics.FromImage(objBitmap);
            //        SizeF stringSize = g.MeasureString(CurrentForm.zyEditorControl1.EMRDoc.Content.GetText(), stringFont);
            //        //int nWidth = (int)stringSize.Width;


            //        //int nHeight = (int)stringSize.Height;
            //        int nWidth = (int)CurrentForm.zyEditorControl1.Width;
            //        int nHeight = (int)CurrentForm.zyEditorControl1.Height;
            //        g.Dispose();
            //        objBitmap.Dispose();
            //        objBitmap = new Bitmap(nWidth, nHeight);
            //        g = Graphics.FromImage(objBitmap);
            //        g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, nWidth, nHeight));//m_CurrentModel.ModelContent.InnerXml.ToString()
            //        g.TextRenderingHint = TextRenderingHint.AntiAlias;
            //        g.DrawString(CurrentForm.zyEditorControl1.EMRDoc.Content.GetText(), stringFont, new SolidBrush(Color.Black), new PointF(0, 0), stringFormat);
            //        objBitmap.Save(new FileStream(dlg.FileName.ToString(), FileMode.Create), ImageFormat.Jpeg);
            //        objBitmap.Dispose();
            //        #endregion

            //    }
            //}
        }
        /// <summary>
        /// 调用PACS图像
        /// add by ywk 2012年8月21日 10:26:36
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCallPacs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string valuestr = GetConfigValueByKey("PACSRevision");
                if (valuestr == "yczxyy")  //宜昌中心医院
                {
                    PacsZhongXinHos();
                }
                else
                {
                    PACSOutSide.PacsAll(m_CurrentInpatient);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(3, ex);
            }
        }

        /// <summary>
        /// 中心医院调用pacs方法
        /// </summary>
        private void PacsZhongXinHos()
        {
            string HospitalNo = m_CurrentInpatient.RecordNoOfHospital;//住院号
            int nPatientType = 2;//患者类型（1.门诊号 2.住院号）
            int LookType = 1;//类型（1.图像 2.报告）

            if (CheckPackIsExist())
            {

                //string jointpath = Application.StartupPath + @"\joint.dll";//获取程序所在文件夹
                //if (!File.Exists(jointpath))//不存在此接口DLL就不执行 
                //{
                //    m_app.CustomMessageBox.MessageShow("调用失败，缺少程序集joint.dll");
                //    return;
                //}
                //string connectpath = Application.StartupPath + @"\Connection.dll";//获取程序所在文件夹
                //if (!File.Exists(connectpath))//不存在此接口DLL就不执行 
                //{
                //    m_app.CustomMessageBox.MessageShow("调用失败，缺少程序集Connection.dll");
                //    return;
                //}
                //string pacspath = Application.StartupPath + @"\PACSID.dll";//获取程序所在文件夹
                //if (!File.Exists(pacspath))//不存在此接口DLL就不执行 
                //{
                //    m_app.CustomMessageBox.MessageShow("调用失败，缺少程序集PACSID.dll");
                //    return;
                //}

                try
                {
                    //if (PacsInitialize() == 1)//启动
                    {
                        //MessageBox.Show("启动成功");
                        //if (PacsViewByPatientInfo(LookType, HospitalNo, nPatientType))
                        //if (PacsView(nPatientType, m_CurrentInpatient.RecordNoOfHospital, LookType) == 1)
                        //{
                        //MessageBox.Show("调用成功");
                        //PacsRelease();//
                        // }
                        // else
                        //{
                        MessageBox.Show("调用失败");
                        //}
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        //PacsClose();
                        //PacsStart();
                    }
                    catch (Exception ex1)
                    { }

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// 出科检查
        /// add by ywk 2012年8月21日 10:26:44 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckOutDep_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                GOMyCheck();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #region "科室小模板"
        private void barButtonDeptTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        private void btnCancelAuditPopup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }


        //添加通用单据模板
        private void btnAddCommonRecord_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                    TreeListNode patNode = treeList1.FocusedNode;

                    InCommonNoteEnmtity inCommonNote = InCommonNoteBiz.ConvertCommonToInCommon(commonNote);
                    InCommonNoteBiz icombiz = new DrectSoft.Core.CommonTableConfig.CommonNoteUse.InCommonNoteBiz(m_app);
                    DataTable inpatientDt = icombiz.GetInpatient(CurrentInpatient.NoOfFirstPage.ToString());
                    inCommonNote.CurrDepartID = inpatientDt.Rows[0]["OUTHOSDEPT"].ToString();
                    inCommonNote.CurrDepartName = inpatientDt.Rows[0]["DEPARTNAME"].ToString();
                    inCommonNote.CurrWardID = inpatientDt.Rows[0]["OUTHOSWARD"].ToString();
                    inCommonNote.CurrWardName = inpatientDt.Rows[0]["WARDNAME"].ToString();
                    inCommonNote.NoofInpatient = CurrentInpatient.NoOfFirstPage.ToString();
                    inCommonNote.InPatientName = inpatientDt.Rows[0]["NAME"].ToString();
                    bool saveResult = icombiz.SaveInCommomNoteAll(inCommonNote, ref message);
                    if (saveResult)
                    {
                        string treename = inCommonNote.InCommonNoteName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        TreeListNode leafNode = treeList1.AppendNode(new object[] { treename }, patNode);
                        leafNode.Tag = inCommonNote;
                        treeList1.FocusedNode = leafNode;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除通用模板
        /// edit by xlb 2013-02-17
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelCommonNote_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                var treelistnode = treeList1.FocusedNode;
                InCommonNoteEnmtity inCommonNoteEnmtity = treelistnode.Tag as InCommonNoteEnmtity;
                if (inCommonNoteEnmtity == null) return;
                DialogResult diaresult = MyMessageBox.Show("您确定要删除该记录吗？", "删除记录", MyMessageBoxButtons.YesNo);
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
                    //throw new Exception("已被使用无法删除");
                    MessageBox.Show("已被使用无法删除");
                    return;
                }
                bool Delresult = inCommonNoteBiz.DelInCommonNote(inCommonNoteEnmtity, ref message);
                if (Delresult)
                {
                    treeList1.FocusedNodeChanged -= new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
                    treeList1.DeleteNode(treelistnode);
                    treeList1.FocusedNodeChanged += new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
                    XtraTabPage xtraTabPageForDel = null;
                    foreach (XtraTabPage itemtab in xtraTabControl1.TabPages)
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
                        xtraTabControl1.TabPages.Remove(xtraTabPageForDel);
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");
                    }


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
        //文书录入的存储背景色的菜单
        private System.Windows.Forms.ContextMenu menuColor = new System.Windows.Forms.ContextMenu();
        /// <summary>
        /// /字体颜色
        /// add by ywk 2012年11月19日9:18:33
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonFontColor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (ColorDialog cdia = new ColorDialog())
                {
                    if (cdia.ShowDialog() == DialogResult.OK)
                    {
                        this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionColor(cdia.Color);
                        //SetDocumentColor(sender, e);
                        //ColorMenuItem item = sender as ColorMenuItem;

                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 设置文书录入界面的背景色
        /// add by ywk 2012年12月12日15:42:49 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDocumentColor(object sender, System.EventArgs e)
        {
            try
            {
                ColorMenuItem item = sender as ColorMenuItem;
                if (item != null)
                {
                    this.CurrentForm.zyEditorControl1.PageBackColor = item.Color;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 设置背景色
        /// add by ywk 2012年12月12日16:00:44
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetPageBackColor_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (ColorDialog cdia = new ColorDialog())
                {
                    if (cdia.ShowDialog() == DialogResult.OK)
                    {

                        this.CurrentForm.zyEditorControl1.PageBackColor = cdia.Color;
                        this.CurrentForm.zyEditorControl1.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 刷新病历节点数据（左侧节点）
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-19</date>
        private void ReFreshRecordData()
        {
            try
            {
                RemoveTabPageExcludFirst();
                RefreshEMRMainPad();
                SetSelectNode(treeList1.Nodes, "病程记录");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置选中节点且展开
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-08</date>
        /// <param name="nodeName"></param>
        private bool SetSelectNode(TreeListNodes nodes, string nodeName)
        {
            try
            {
                if (string.IsNullOrEmpty(nodeName) || null == nodes || nodes.Count == 0)
                {
                    return false;
                }
                bool boo = false;
                foreach (TreeListNode node in nodes)
                {
                    if (node.GetValue("colName").ToString() == nodeName)
                    {
                        node.Selected = true;
                        node.ExpandAll();
                        boo = true;
                        break;
                    }
                    boo = SetSelectNode(node.Nodes, nodeName);
                    if (boo)
                    {
                        break;
                    }
                }
                return boo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查病人当前科室与医生科室(包含权限科室)是否一致
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-24</date>
        /// <param name="noofinpat">首页序号</param>
        /// <returns></returns>
        private string CheckIfCurrentDept(int noofinpat)
        {
            try
            {
                DataTable inpDt = DS_SqlService.GetInpatientByID(noofinpat, 2);
                if (null == inpDt || inpDt.Rows.Count == 0)
                {
                    return "该病人不存在，请刷新数据重试。";
                }
                string config = DS_SqlService.GetConfigValueByKey("WardAndDeptRelationship");
                if (config == "2")
                {///科室包含病区
                    string dept = null == inpDt.Rows[0]["outhosdept"] ? "" : inpDt.Rows[0]["outhosdept"].ToString().Trim();
                    if (dept != DS_Common.currentUser.CurrentDeptId)
                    {//该病人已转科
                        if (string.IsNullOrEmpty(dept.Trim()))
                        {
                            return "该病人所属科室异常，请联系管理员。";
                        }
                        string deptName = DS_BaseService.GetDeptNameByID(dept);
                        List<string[]> list = DS_BaseService.GetDeptAndWardInRight(DS_Common.currentUser.Id);
                        if (null != list && list.Count > 0 && list.Any(p => p[0] == dept))
                        {//转科后科室在医生权限范围内
                            return "该病人已转至 " + deptName + "(" + dept + ")" + "，请切换科室。";
                        }
                        else
                        {
                            return "该病人已转至 " + deptName + "(" + dept + ")" + "，您无权操作。";
                        }
                    }
                }
                else if (config == "1")
                {///病区包含科室
                    string wardCode = null == inpDt.Rows[0]["outhosward"] ? "" : inpDt.Rows[0]["outhosward"].ToString().Trim();
                    if (wardCode != DS_Common.currentUser.CurrentWardId)
                    {
                        if (string.IsNullOrEmpty(wardCode.Trim()))
                        {
                            return "该病人所属病区异常，请联系管理员。";
                        }
                        string wardName = DS_BaseService.GetWardNameByID(wardCode);
                        List<string[]> list = DS_BaseService.GetDeptAndWardInRight(DS_Common.currentUser.Id);
                        if (null != list && list.Count > 0 && list.Any(p => p[1] == wardCode))
                        {//转病区后病区在医生权限范围内
                            return "该病人已转至 " + wardName + "(" + wardCode + ")" + "，请切换病区。";
                        }
                        else
                        {
                            return "该病人已转至 " + wardName + "(" + wardCode + ")" + "，您无权操作。";
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 病历时间修改限制
        /// </summary>
        /// 1、修改前时间和修改后时间中间不能存在首程或其他科室病历
        /// 2、首程修改前时间和修改后时间中间不能存在其它病历
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-01-30</date>
        /// <param name="em">病历</param>
        /// <param name="dbList">数据库中有效节点集合</param>
        /// <returns></returns>
        private string CheckChangedRecordTime(EmrModel em, List<DataRow> allDBList)
        {
            try
            {
                if (null == em || null == allDBList || allDBList.Count() == 0)
                {
                    return string.Empty;
                }
                var dbList = allDBList.Where(p => int.Parse(p["valid"].ToString()) == 1);
                if (dbList.Any(p => int.Parse(p["ID"].ToString()) == em.InstanceId && p["captiondatetime"].ToString() != em.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss")))
                {//修改病历时间
                    DataRow dbOldRow = dbList.FirstOrDefault(p => int.Parse(p["ID"].ToString()) == em.InstanceId);
                    if (null != dbOldRow && null != dbOldRow["captiondatetime"])
                    {
                        DateTime oldTime = DateTime.Parse(dbOldRow["captiondatetime"].ToString());
                        DateTime smallerTime = oldTime < em.DisplayTime ? oldTime : em.DisplayTime;
                        DateTime biggerTime = oldTime < em.DisplayTime ? em.DisplayTime : oldTime;
                        if (dbList.Any(p => DateTime.Parse(p["captiondatetime"].ToString()) > smallerTime && DateTime.Parse(p["captiondatetime"].ToString()) < biggerTime && (p["departcode"].ToString().Trim() != em.DepartCode || p["firstdailyflag"].ToString().Trim() == "1")))
                        {
                            return "数据库中病程记录已更新，请刷新后重试。";
                        }

                        //add by cyq 2013-02-01 限制首程时间(避免首程时间修改至病历之后-并发操作)
                        if (em.DailyEmrModel && em.FirstDailyEmrModel && dbList.Any(p => int.Parse(p["ID"].ToString()) != em.InstanceId && DateTime.Parse(p["captiondatetime"].ToString()) >= smallerTime && DateTime.Parse(p["captiondatetime"].ToString()) <= biggerTime))
                        {//首程(编辑)
                            return "数据库中病程记录已更新，请刷新后重试。";
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化各首程的开始修改和结束修改标识
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-20</date>
        /// <param name="noofinpat"></param>
        private void SetRecordUpdateProcessList(int noofinpat)
        {
            try
            {
                DataTable dt = DS_SqlService.GetFirstRecordsForSpeed(noofinpat);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return;
                }
                m_RecordUpdateProcessList = new List<string[]>();
                foreach (DataRow drow in dt.Rows)
                {
                    string[] array = new string[3];
                    array[0] = null == drow["ID"] ? string.Empty : drow["ID"].ToString().Trim();
                    array[1] = null == drow["startupdateflag"] ? string.Empty : drow["startupdateflag"].ToString();
                    array[2] = null == drow["endupdateflag"] ? string.Empty : drow["endupdateflag"].ToString();
                    m_RecordUpdateProcessList.Add(array);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 初始化各首程的开始修改和结束修改标识
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-03-20</date>
        /// <param name="id"></param>
        private void SetRecordUpdateFlag(int id)
        {
            try
            {
                DataTable dt = DS_SqlService.GetRecordByIDContainsDel(id);
                if (null == dt || dt.Rows.Count == 0)
                {
                    return;
                }
                DataRow theFirstEmr = dt.Rows[0];
                if (null != m_RecordUpdateProcessList)
                {
                    string[] array = m_RecordUpdateProcessList.FirstOrDefault(q => int.Parse(q[0]) == int.Parse(theFirstEmr["ID"].ToString()));
                    if (null != array)
                    {
                        array[0] = null == theFirstEmr["ID"] ? string.Empty : theFirstEmr["ID"].ToString().Trim();
                        array[1] = null == theFirstEmr["startupdateflag"] ? string.Empty : theFirstEmr["startupdateflag"].ToString();
                        array[2] = null == theFirstEmr["endupdateflag"] ? string.Empty : theFirstEmr["endupdateflag"].ToString();
                    }
                    else
                    {
                        array = new string[3];
                        array[0] = null == theFirstEmr["ID"] ? string.Empty : theFirstEmr["ID"].ToString().Trim();
                        array[1] = null == theFirstEmr["startupdateflag"] ? string.Empty : theFirstEmr["startupdateflag"].ToString();
                        array[2] = null == theFirstEmr["endupdateflag"] ? string.Empty : theFirstEmr["endupdateflag"].ToString();
                        m_RecordUpdateProcessList.Add(array);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void dockPanel2_Click(object sender, EventArgs e)
        {

        }
    }
}



//catch (DocumentException de)
//{
//    //Response.Write(de.ToString());
//}


