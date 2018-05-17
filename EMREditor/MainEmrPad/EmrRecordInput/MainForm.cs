using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YidanSoft.Library.EmrEditor.Src.Document;
using YidanSoft.Library.EmrEditor.Src.Gui;
using System.Data.OracleClient;
using DevExpress.XtraTreeList.Nodes;
using System.Xml.Serialization;
using System.IO;
using YidanSoft.FrameWork;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.FrameWork.WinForm;
using System.Xml;
using System.Collections;
using YidanSoft.Common.Eop;
using DevExpress.XtraTab;
using DevExpress.Utils;
using System.Reflection;
using YindanSoft.Emr.Util;
using DevExpress.XtraTreeList;
using YidanSoft.Library.EmrEditor.Src.Common;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using YidanSoft.Library.EmrEditor.Src.Print;
using System.Drawing.Printing;
using YidanSoft.Core;
using Yidansoft.Core.PersonTtemTemplet;

/*
 * 添加病历节点的时候需要注意按如下顺序，否则会导致最后一步SelectedPageChanged事件计算的不正确：
 * 1.在树中添加节点
 * 2.在节点的Tag中添加EmrModel
 * 3.设置界面的TabPage
 * 4.设置当前TabControl选中TabPage 【xtraTabControl1.SelectedTabPage = newPad】
 * 5.触发TabControl事件xtraTabControl1_SelectedPageChanged，计算出当前病历界面 “对应树中的节点”，“当前的EmrModel”，“当前的EmrModelContainer”
 */
namespace Yidansoft.Core.MainEmrPad.EmrRecordInput
{
    public partial class MainForm : Form, IStartPlugIn
    {
        #region Field && Property
        PersonItemManager m_PersonItem;
        DataTable m_MyTreeFolders;
        DataTable m_MyLeafs;
        /// <summary>
        /// 用于判断病案首页在TreeList中的节点
        /// </summary>
        const string c_IEMMainPageDLL = "YidanSoft.Core.IEMMainPage.UCMainPage,YidanSoft.Core.IEMMainPage";

        private IYidanEmrHost m_app;

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

        #endregion

        #region .ctor
        public MainForm()
        {
            m_WaitDialog = new WaitDialogForm("创建病历编辑组件……", "请稍等。");
            InitializeComponent();
            RegisterEvent();

            ExtraInit();
            InitDictionary();
            InitFont();
            HideWaitDialog();
        }

        private void RegisterEvent()
        {
            btnSavePopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnSavePopup_ItemClick);
            btnDeletePopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnDeletePopup_ItemClick);
            btnSubmitPopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnSubmitPopup_ItemClick);
            btnConfirmPopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnConfirmPopup_ItemClick);
            btnModulePopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnModulePopup_ItemClick);
            FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }

        private void InitFont()
        {
            repositoryItemComboBoxFont.Items.AddRange(FontCommon.FontList.ToArray());
            //this.listBox1.Items.AddRange(FontCommon.FontList.ToArray());

            //添加字号
            foreach (string fontsizename in FontCommon.allFontSizeName)
            {
                repositoryItemComboBox_SiZe.Items.Add(fontsizename);
            }
        }

        private void ExtraInit()
        {
            this.navBarControlToolBox.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Office 2010 Silver");
            this.navBarControlClinical.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Office 2010 Silver");
        }

        private void InitDictionary()
        {
            m_TempModelPages = new Dictionary<EmrModel, XtraTabPage>();
            m_TempContainerPages = new Dictionary<EmrModelContainer, XtraTabPage>();
        }
        #endregion

        #region Load
        private void Form1_Load(object sender, EventArgs e)
        {
            //暂时将医嘱隐藏
            navBarGroup_DoctorAdive.Visible = false;
            //暂时将编辑和只读按钮隐藏
            btn_Edit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnEmrReadOnly.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //病历编辑器设置为默认值
            InitEmrDefaultSetting();
            //设置页面设置
            InitEmrPageSetting();
            //初始化病人信息，初始化树节点，初始化工具栏
            GetPatRec();
            //绑定病人基本信息
            BindExtraInfo();
            SetBarStatus(false);
            //绑定个人模板
            InitPersonTree(true, string.Empty);
        }

        /// <summary>
        /// 病历编辑器默认设置
        /// </summary>
        private void InitEmrDefaultSetting()
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

        /// <summary>
        /// 编辑器页面设置
        /// </summary>
        private void InitEmrPageSetting()
        {
            string xmldoc = BasicSettings.GetStringConfig("PageSetting");
            System.Xml.XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmldoc);
            XmlNode pagesetting = doc.ChildNodes[0].SelectSingleNode("pagesettings");
            if (pagesetting != null)
            {
                XmlElement ele = (pagesetting as XmlElement).SelectSingleNode("page") as XmlElement;
                XPageSettings ps = new XPageSettings();
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
                PadForm.m_PageSetting = ps;
            }
        }

        /// <summary>
        /// 初始化病人信息，初始化树节点，初始化工具栏
        /// </summary>
        private void GetPatRec()
        {
            if (m_app.CurrentPatientInfo == null)
            {
                m_app.CustomMessageBox.MessageShow("请先选择病人！");
                return;
            }
            SetWaitDialogCaption("读取患者基本信息");
            //读取基本信息
            BindPatBasic();
            //构造病历树
            SetWaitDialogCaption("读取患者病历数据");
            InitPatTree();
            //读取病人病历内容
            GetPatRec_Files();
            //初始化右侧工具栏
            InitTool();
            HideWaitDialog();
        }

        /// <summary>
        /// 绑定病人基本信息
        /// </summary>
        private void BindPatBasic()
        {
            m_app.CurrentPatientInfo.ReInitializeAllProperties();
            barStatic_Age.Caption = m_app.CurrentPatientInfo.PersonalInformation.DisplayAge;
            barStatic_Bed.Caption = m_app.CurrentPatientInfo.InfoOfAdmission.AdmitInfo.BedNo + "床";
            barStatic_Blh.Caption = m_app.CurrentPatientInfo.RecordNoOfHospital;
            barStatic_Dept.Caption = m_app.CurrentPatientInfo.InfoOfAdmission.AdmitInfo.CurrentDepartment.Name + ":"
                + m_app.CurrentPatientInfo.InfoOfAdmission.AdmitInfo.CurrentWard.Name; ;
            barStatic_InWardDate.Caption = m_app.CurrentPatientInfo.InfoOfAdmission.AdmitInfo.StepOneDate.ToString("yyyy-MM-dd HH");
            barStaticPatName.Caption = m_app.CurrentPatientInfo.Name;
            barStaticSex.Caption = m_app.CurrentPatientInfo.PersonalInformation.Sex.Name;

        }

        /// <summary>
        /// 初始化左侧的树
        /// </summary>
        private void InitPatTree()
        {
            treeList1.ClearNodes();
            //读取底层文件夹
            if (m_patUtil == null) m_patUtil = new PatRecUtil(m_app);
            DataTable datatable = m_patUtil.GetFolderInfo("00");
            foreach (DataRow row in datatable.Rows)
            {
                InitTree(row, null);
            }
        }

        private void InitTree(DataRow row, TreeListNode node)
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
                patNode.ExpandAll();
            }
            else
            {

                if (container.EmrContainerType != ContainerType.None)
                {
                    //todo
                    DataTable leafs = m_patUtil.GetPatRecInfo(container.ContainerCatalog, m_app.CurrentPatientInfo.NoOfFirstPage.ToString());
                    {
                        foreach (DataRow leaf in leafs.Rows)
                        {
                            EmrModel leafmodel = new EmrModel(leaf);
                            TreeListNode leafNode = treeList1.AppendNode(new object[] { leafmodel.ModelName }, patNode);
                            leafNode.Tag = leafmodel;

                        }

                    }

                }

            }
        }

        private void BindExtraInfo()
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
            //SetWaitDialogCaption("正在加载患者病案数据");
            //LoadIEMMainPage();
            HideWaitDialog();
        }

        private void GetPatRec_Files()
        {
            //读取级别=2的文件夹所有病历，并加载

        }

        /// <summary>
        /// 右侧工具栏初始方法
        /// </summary>
        private void InitTool()
        {
            GetDataCollect();
        }
        #endregion

        #region FocusedNodeChanged
        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            SetWaitDialogCaption("正在加载病历，请稍等");
            if ((e.Node == null) || (e.Node.Tag == null)) return;
            LoadModelFromTree(e.Node);
            this.HideWaitDialog();
        }


        private void LoadModelFromTree(TreeListNode node)
        {
            string containercode = string.Empty;
            #region 容器类
            if (node.Tag is EmrModelContainer)//如果是容器类
            {
                EmrModelContainer container = (EmrModelContainer)node.Tag;
                if (node.HasChildren)
                {
                    //针对病程的处理：点击“病程”文件夹时TabPage显示默认的病程【首程】
                    SetDefaultDailyEmrModelNode(node);
                    SetDailyEmrAllowDelete(node);
                    ResetEditModeAction(container);
                    return;
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
            if ((!node.HasChildren) && (node.Tag is EmrModel))//如果是模板类，则说明是叶子节点
            {
                m_CurrentModel = (EmrModel)node.Tag;
                if (m_CurrentModel.ModelContent == null)
                {
                    m_CurrentModel = m_RecordDal.LoadModelInstance(m_CurrentModel.InstanceId);

                    node.Tag = m_CurrentModel;
                }


                if (m_TempModelPages.ContainsKey(m_CurrentModel)
                    && ((EmrModel)node.Tag).DailyEmrModel == false)//非病程
                {
                    xtraTabControl1.SelectedTabPage = m_TempModelPages[m_CurrentModel];
                }
                else if (((EmrModel)node.Tag).DailyEmrModel == true)//病程
                {
                    //点击的病程对应的首程
                    EmrModel emrModelFirstDaily = emrModelFirstDaily = node.ParentNode.FirstNode.Tag as EmrModel;
                    m_CurrentModel = emrModelFirstDaily;//保证点击病程后m_CurrentModel始终是第一个病程的EmrModel【即首程】
                    if (m_CurrentModel.ModelContent == null)
                    {
                        m_CurrentModel = m_RecordDal.LoadModelInstance(m_CurrentModel);
                    }

                    if (m_TempModelPages.ContainsKey(emrModelFirstDaily))
                    {
                        xtraTabControl1.SelectedTabPage = m_TempModelPages[emrModelFirstDaily];
                    }
                    else
                    {
                        CreateNewDocument(m_CurrentModel);
                    }
                    //设置指定的病程病历
                    SetDailyEmrDoc(node);
                }
                else
                {
                    CreateNewDocument(m_CurrentModel);
                }

                //ResetEditModeAction(m_CurrentModel); wwj 2011-07-12

                EmrModel model = (EmrModel)node.Tag;
                CurrentForm.ResetEditModeState(model);
                ResetEditModeAction(model);
                if (model.DailyEmrModel)//针对病程的处理
                {
                    ProcDailyEmrModel(node);
                }
                else
                {
                    CheckIsAllowEdit(model);
                }

                //
                if (node.ParentNode.Tag is EmrModelContainer)
                {

                    containercode = ((EmrModelContainer)node.ParentNode.Tag).ContainerCatalog;
                }
            }
            #endregion

            //刷新科室小模板
            InitPersonTree(false, containercode);

        }

        /// <summary>
        /// 设置除病程外的病历可编辑情况
        /// </summary>
        /// <param name="model"></param>
        private void CheckIsAllowEdit(EmrModel model)
        {
            DoctorGrade grad = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);
            if (model.State != ExamineState.NotSubmit && grad == DoctorGrade.Resident)//住院医且已提交
            {
                SetDocmentReadOnlyMode();
            }
        }

        private void ProcDailyEmrModel(TreeListNode node)
        {
            SetDailyEmrAllowDelete(node);

            //定位选中的病程整个病程记录中的位置
            CurrentForm.LocateDailyEmrMode(node);

            //设置病程对应的编辑区域
            SetDailyEmrEditArea(node);
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
            SelectedPageChanged(e.Page, true);
        }

        bool m_IsNeedActiveEditArea = true;
        public void SelectedPageChanged(XtraTabPage page, bool isNeedActiveEditArea)
        {
            m_IsNeedActiveEditArea = isNeedActiveEditArea;//被外部调用则不需要激活病程的EditArea

            //找到 当前病历EmrModel 或 非病历类型的界面IEmrEditor
            GetCurrentEmrModel(ref m_CurrentModel, ref m_CurrentEmrEditor, page);

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
        }

        /// <summary>
        /// 得到处于激活状态的病历
        /// </summary>
        private void GetCurrentEmrModel(ref EmrModel emrModel,
            ref IEMREditor emrEditor,/*非病历类型的界面 如首页、三测单之类的内容*/
            XtraTabPage page)
        {
            emrModel = null;
            emrEditor = null;

            //当前激活的TabPage中包含的EmrModel
            foreach (KeyValuePair<EmrModel, XtraTabPage> item in m_TempModelPages)
            {
                if (item.Value == page)
                {
                    emrModel = item.Key;
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

        private void GetEmrEditorContainer(IEMREditor emrEditor)
        {
            m_CurrentModelContainer = null;
            foreach (KeyValuePair<EmrModelContainer, XtraTabPage> item in m_TempContainerPages)
            {
                if (item.Value.Controls[0] == emrEditor)
                {
                    m_CurrentModelContainer = item.Key;
                }
            }
        }

        private void GetCurrentTreeListNode(TreeListNodes nodes, EmrModel emrModel, EmrModelContainer emrModelContainer)
        {
            if (nodes == null)
            {
                nodes = treeList1.Nodes;
            }

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

        /// <summary>
        /// 设置病历对应的树节点【不触发FocusNodeChanged事件】
        /// </summary>
        private void SetFocusNode()
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
                            m_IsNeedActiveEditArea = true;
                        }
                    }
                }
            }
            else
            {
                FocusNodeNotTriggerEvent(m_CurrentTreeListNode);
            }
        }

        /// <summary>
        /// 设置病程编辑区【注意：在保存按钮可用的情况下才设置编辑区域】
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isCheck">是否需要判断</param>
        private void SetDailyEmrEditArea(TreeListNode node)
        {
            CurrentForm.zyEditorControl1.ActiveEditArea = null;
            CurrentForm.zyEditorControl1.Refresh();
            SetDocmentReadOnlyMode();

            if (barButtonSave.Enabled)//当保存按钮可用时，表示此病历可以编辑，这时可以设置病历的编辑区域，否则不需要设置病历的编辑区域
            {
                CurrentForm.SetDailyEmrEditArea(node);
            }
            else if (barButton_Audit.Enabled)//当审核按钮可用时表示当前病历正在审核，所以可以进行编辑
            {
                CurrentForm.SetDailyEmrEditArea(node);
            }
        }
        #endregion

        #region Close TabPage
        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            XtraTabControlCloseButton();
        }

        private void XtraTabControlCloseButton()
        {
            //todo 校验关闭的窗口是否需要保存
            if (xtraTabControl1.SelectedTabPage.Tag is EmrModelContainer)
            {
                //todo 检验是否需要保存
                m_TempContainerPages.Remove((EmrModelContainer)xtraTabControl1.SelectedTabPage.Tag);
                xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
            }
            else if (CurrentForm.zyEditorControl1.EMRDoc.Modified && barButtonSave.Enabled == true)
            {
                //把病历的修改保存到临时变量中 EmrModel
                CurrentForm.zyEditorControl1.EMRDoc.ToXMLDocument(m_CurrentModel.ModelContent);

                string fileName = m_CurrentModel.ModelName;
                if (m_CurrentModel.DailyEmrModel)
                {
                    fileName = "病程记录";
                }

                if (m_app.CustomMessageBox.MessageShow(fileName + " 已经修改，是否保存", YidanSoft.Core.CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
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
                    DeleteTablePage(m_CurrentModel);
                }
            }
            else
            {
                //xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
                //m_TempModelPages.Remove(m_CurrentModel);
                DeleteTablePage(m_CurrentModel);
            }
        }
        #endregion

        #region 设置编辑器 DocumentModel

        /// <summary>
        /// 设置当前编辑器为只读模式
        /// </summary>
        private void SetDocmentReadOnlyMode()
        {
            if (CurrentForm == null) return;

            this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Read;
        }

        /// <summary>
        /// 设置当前编辑器为可编辑模式
        /// </summary>
        private void SetDomentEditMode()
        {
            if (CurrentForm == null) return;

            this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
        }

        /// <summary>
        /// 设置当前编辑器为设计模式
        /// </summary>
        private void SetDocmentDesginMode()
        {
            if (CurrentForm == null) return;

            this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Design;
        }

        #endregion

        #region 不触发FocusedNodeChanged事件的方法 focusNode、deleteNode

        /// <summary>
        /// 设置获得焦点的树节点，不触发事件
        /// </summary>
        /// <param name="node"></param>
        private void FocusNodeNotTriggerEvent(TreeListNode node)
        {
            treeList1.FocusedNodeChanged -= new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
            node.Visible = true;
            treeList1.FocusedNode = node;
            treeList1.FocusedNodeChanged += new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
        }

        /// <summary>
        /// 删除树中的节点，不触发事件
        /// </summary>
        /// <param name="node"></param>
        private void DeleteNodeNotTriggerEvent(TreeListNode node)
        {
            treeList1.FocusedNodeChanged -= new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
            treeList1.DeleteNode(node);
            treeList1.FocusedNodeChanged += new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
        }
        #endregion

        /// <summary>
        /// 点击“病程”文件夹时TabPage显示默认的病程【首程】
        /// </summary>
        /// <param name="node"></param>
        private void SetDefaultDailyEmrModelNode(TreeListNode node)
        {
            if (node.Nodes.Count > 0)//有子节点
            {
                if (node.Nodes[0].Tag is EmrModel && ((EmrModel)node.Nodes[0].Tag).DailyEmrModel == true)//子节点是病程
                {
                    treeList1.FocusedNode = node.Nodes[0];
                    FocusNodeNotTriggerEvent(node);

                    CurrentForm.ResetEditModeState(m_CurrentModel);
                }
            }
        }

        /// <summary>
        /// 设置病程病历 选中节点NotTriggerEvent 更改TabPage名称
        /// </summary>
        private void SetDailyEmrDoc(TreeListNode node)
        {
            //选中节点
            FocusNodeNotTriggerEvent(node);

            //更改TabPage名称
            ChangePageName(node);

            //EmrModel model = node.Tag as EmrModel;
        }

        /// <summary>
        /// 更改TabPage名称
        /// </summary>
        /// <param name="node"></param>
        private void ChangePageName(TreeListNode node)
        {
            XtraTabPage page = xtraTabControl1.SelectedTabPage;
            if (node.GetValue("colName") != null)
            {
                page.Text = GetTabPageName(node.GetValue("colName").ToString());
            }
        }

        #region 病历文件增删改

        private void FillModelMacro()
        {
            //根据病历的不同状态，调用程序在此处初始化宏的值。
            //替换标题中的宏
            XmlDocument headerdoc = new XmlDocument();

            headerdoc.LoadXml(CurrentForm.zyEditorControl1.EMRDoc.HeadString);
            XmlNodeList nodes = headerdoc.SelectNodes("header/p/macro");
            foreach (XmlNode node in nodes)
            {
                node.InnerText = GetDataByNameForMacro(node.Attributes["name"].Value);
            }
            CurrentForm.zyEditorControl1.EMRDoc.HeadString = headerdoc.OuterXml;


            //替换文档中的宏
            //获得所有宏元素列表
            ArrayList al = new ArrayList();
            ZYTextDocument doc = CurrentForm.zyEditorControl1.EMRDoc;
            doc.GetAllSpecElement(al, doc.RootDocumentElement, ElementType.Macro, null);

            //循环每个宏元素，根据宏元素的Name属性，查询并赋值线Text属性
            foreach (ZYMacro m in al)
            {
                m.Text = GetDataByNameForMacro(m.Name);
            }
            doc.RefreshSize();
            doc.ContentChanged();
            doc.OwnerControl.Refresh();
            doc.UpdateCaret();
        }

        /// <summary>
        /// 寻常入院记录
        /// </summary>
        private PadForm GetMainRecord()
        {
            //写死 找到入院记录
            foreach (EmrModel model in m_TempModelPages.Keys)
            {
                if (model.ModelCatalog.Equals("AB"))
                {
                    return (PadForm)m_TempModelPages[model].Controls[0];
                }
            }

            return null;

        }

        private void ReplaceModelMacro()
        {
            //获得病历中所有可替换元素列表
            ArrayList al = new ArrayList();
            ZYTextDocument doc = CurrentForm.zyEditorControl1.EMRDoc;
            doc.GetAllSpecElement(al, doc.RootDocumentElement, ElementType.Replace, null);

            //循环每个可替换元素，根据可替换元素的Name属性，查询并赋值线Text属性
            if (al.Count < 0) return;
            PadForm padForm = GetMainRecord();
            if (padForm == null) return;
            foreach (ZYReplace m in al)
            {
                //寻找入院记录内容
                m.Text = padForm.zyEditorControl1.EMRDoc.GetReplaceText(m.Name);
            }
            doc.RefreshSize();
            doc.ContentChanged();
            doc.OwnerControl.Refresh();
            doc.UpdateCaret();
        }

        string GetDataByNameForMacro(string name)
        {
            //此处应该写具体的实现方法
            return MacroUtil.FillMarcValue(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), name);
            //return "宏" + name + "的数据";
        }

        string GetDataByNameForReplace(string name)
        {
            //此处应该写具体的实现方法
            return "可替换项" + name + "的数据";
        }

        private void DeleteFromPad()
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

        /// <summary>
        /// 删除节点后，如果焦点位于病程节点上，则病程中需要设置编辑区
        /// </summary>
        private void SetDailyEmrEditArea()
        {
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

        /// <summary>
        /// 将病历从TabControl和TreeList中删除
        /// </summary>
        /// <param name="emrModel"></param>
        private void DeleteTabPageAndNode(EmrModel emrModel)
        {
            TreeListNode node = FindEmrModelInNode(emrModel);
            DeleteNodeNotTriggerEvent(node);
            //treeList1.DeleteNode(m_CurrentTreeListNode);
            //DeleteNodeNotTriggerEvent(m_CurrentTreeListNode);
            DeleteTablePage(emrModel);
        }

        #region
        /// <summary>
        /// 得到当前EmrModel所在的树节点
        /// </summary>
        /// <param name="emrModel"></param>
        private TreeListNode FindEmrModelInNode(EmrModel emrModel)
        {
            GetCurrentTreeListNode(null, emrModel);
            return m_Node;
        }

        private TreeListNode m_Node;
        private void GetCurrentTreeListNode(TreeListNodes nodes, EmrModel emrModel)
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
        #endregion

        private void DeleteTablePage(EmrModel emrModel)
        {
            if (!m_TempModelPages.ContainsKey(emrModel))
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

        #endregion

        #region Print 相关
        private void JumpPrint()
        {
            if (CurrentForm == null) return;
            CurrentForm.zyEditorControl1.EMRDoc.EnableJumpPrint = !CurrentForm.zyEditorControl1.EMRDoc.EnableJumpPrint;
            CurrentForm.zyEditorControl1.Refresh();
        }

        private void PrientViewDocment()
        {
            if (CurrentForm == null) return;
            CurrentForm.zyEditorControl1.EMRDoc._PrintPreview();
        }

        private void PrintDocment()
        {
            if (CurrentForm == null) return;
            CurrentForm.zyEditorControl1.EMRDoc._Print();
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

        /// <summary>
        /// 读取某个病历文的具体内容，并加载至编辑器
        /// </summary>
        /// <param name="fileIndex"></param>
        private void GetFileInfo(string fileIndex)
        {
            //读取用户的权限，分别模型显示当前病历内容

            //读取病程的时候需要特殊处理


        }


        #region 表格相关操作


        private void InsertTable()
        {
            TableInsert tableInsertForm = new TableInsert();
            if (tableInsertForm.ShowDialog() == DialogResult.OK)
            {
                CurrentForm.zyEditorControl1.EMRDoc.TableInsert(tableInsertForm.Header,
                    tableInsertForm.Rows, tableInsertForm.Columns,
                    tableInsertForm.ColumnWidth);
            }
        }

        private void InsertTableRow()
        {
            RowsInsert rowsInsertForm = new RowsInsert();
            if (rowsInsertForm.ShowDialog() == DialogResult.OK)
            {
                CurrentForm.zyEditorControl1.EMRDoc.RowInsert(rowsInsertForm.RowNum, rowsInsertForm.IsBack);
            }
        }

        private void InsertTableColumn()
        {
            ColumnsInsert columnsInsertForm = new ColumnsInsert();
            if (columnsInsertForm.ShowDialog() == DialogResult.OK)
            {
                CurrentForm.zyEditorControl1.EMRDoc.ColumnInsert(columnsInsertForm.ColumnNum, columnsInsertForm.IsBack);
            }
        }


        private void DeleteTable()
        {
            CurrentForm.zyEditorControl1.EMRDoc.TableDelete();
        }

        private void DeletTableRow()
        {
            CurrentForm.zyEditorControl1.EMRDoc.TableDeleteRow();
        }

        private void DeletTableColumn()
        {
            CurrentForm.zyEditorControl1.EMRDoc.TableDeleteCol();
        }

        private void ChoiceTable()
        {
            CurrentForm.zyEditorControl1.EMRDoc.TableSelect();
        }

        private void ChoiceTableRow()
        {
            CurrentForm.zyEditorControl1.EMRDoc.TableSelectRow();
        }

        private void ChoiceTableColumn()
        {
            CurrentForm.zyEditorControl1.EMRDoc.TableSelectCol();
        }

        private void MergeTableCell()
        {
            if (CurrentForm.zyEditorControl1.EMRDoc.TableIsCanMerge() == 1)
            {
                CurrentForm.zyEditorControl1.EMRDoc.TableMergeCell();
            }
            else if (CurrentForm.zyEditorControl1.EMRDoc.TableIsCanMerge() == 2)
            {
                MessageBox.Show("此次操作须得在表格中选中单元格才能生效", "不能合并", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (CurrentForm.zyEditorControl1.EMRDoc.TableIsCanMerge() == 3)
            {
                MessageBox.Show("您并没有选中任何单元格", "不能合并", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (CurrentForm.zyEditorControl1.EMRDoc.TableIsCanMerge() == 4)
            {
                MessageBox.Show("您只选中了一个单元格,无法再次合并", "不能合并", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (CurrentForm.zyEditorControl1.EMRDoc.TableIsCanMerge() == 5)
            {
                MessageBox.Show("请检查您选择的单元格是否呈正规矩形", "不能合并", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ApartTableCell()
        {

            Dictionary<string, int> splitDic = CurrentForm.zyEditorControl1.EMRDoc.TableBeforeSplitCell();
            ApartCell apartCellForm = new ApartCell(splitDic["row"], splitDic["col"]);

            if (apartCellForm.ShowDialog() == DialogResult.OK)
            {
                CurrentForm.zyEditorControl1.EMRDoc.TableSplitCell(apartCellForm.intRow, apartCellForm.intColumn);
            }
        }

        private void SetTableProperty()
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
                MessageBox.Show("光标必须在表格中才能更改表格属性", "不能打开表格属性", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion
        private void ExportDocment(string caption)
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

        #region  copy 相关
        private void Copy()
        {
            CurrentForm.zyEditorControl1.EMRDoc._Copy();
        }

        private void Paste()
        {
            CurrentForm.zyEditorControl1.EMRDoc._Paste();
        }

        private void Undo()
        {
            CurrentForm.zyEditorControl1.EMRDoc._Undo();
        }
        private void Redo()
        {
            CurrentForm.zyEditorControl1.EMRDoc._Redo();

        }

        private void BoldChar(bool reslut)
        {
            CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontBold(reslut);

        }

        #endregion

        #region sub and sup

        void ReSetSupSub()
        {
            CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSup(false);
            CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSub(false);
        }

        /// <summary>
        /// 上标
        /// </summary>
        private void SupChar(bool reslut)
        {
            ReSetSupSub();
            CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSup(reslut);
        }

        /// <summary>
        /// 下标
        /// </summary>
        private void SubChar(bool reslut)
        {
            ReSetSupSub();
            CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSub(reslut);
        }
        #endregion

        private void InsertImage()
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

                    Image img = Image.FromFile(dlgFile.FileName);

                    MemoryStream ms = new MemoryStream();
                    byte[] imagedata = null;
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                    imagedata = ms.GetBuffer();

                    CurrentForm.zyEditorControl1.EMRDoc._InsertImage(imagedata);
                }
            }
        }

        #region public methods of WaitDialog
        public void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }

        }

        public void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }
        #endregion

        /// <summary>
        /// 对新病程的设置
        /// </summary>
        /// <param name="model"></param>
        /// <param name="patRecTime"></param>
        /// <param name="isInsertPadForm">是否要将病历的内容插入到病历编辑器中</param>
        /// <returns></returns>
        private bool ProcessDailyTemplateForm(EmrModel model, TreeListNode node, out DateTime patRecTime)
        {
            patRecTime = DateTime.Now;
            string titleName = model.IsShowFileName == "1" ? model.FileName : "";
            using (DailyTemplateForm daily = new DailyTemplateForm(DateTime.Now, titleName/*model.Description*/))
            {
                daily.SetAllDisplayDateTime(node);
                if (daily.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //m_IsAddDailyEmrModel = true;
                    model.DisplayTime = daily.CommitDateTime;
                    model.Description = daily.CommitTitle;
                    return true;
                }
                return false;
                //else
                //{
                //    m_IsAddDailyEmrModel = false;
                //}
            }
        }

        /// <summary>
        /// 创建病程相关界面
        /// </summary>
        /// <param name="model"></param>
        void CreateNewDocument(EmrModel model)
        {
            //病程相关文件
            if (model.DailyEmrModel)
            {
                //判断是否需要添加
                if (!m_TempModelPages.ContainsKey(model))
                {
                    //判断当前模型是否需要在新建tabpage页
                    XtraTabPage newPad = null;
                    foreach (EmrModel md in m_TempModelPages.Keys)
                    {
                        if (md.ModelCatalog.Equals(model.ModelCatalog))
                        {
                            newPad = m_TempModelPages[md];
                            break;
                        }

                    }
                    //如果未实现则新建一个tabpage
                    if (newPad == null)
                    {
                        PadForm pad = new PadForm(m_patUtil, m_RecordDal);
                        pad.Dock = DockStyle.Fill;
                        InsertEmrModelContent(pad, model);
                        newPad = xtraTabControl1.TabPages.Add();
                        newPad.Text = GetTabPageName(model.ModelName); //model.ModelName;//显示简短的文件名
                        //newPad.Tooltip = model.ModelName;//文件全名
                        newPad.Controls.Add(pad);
                        newPad.Tag = model;
                        m_CurrentModel = model;
                        m_TempModelPages.Add(model, newPad);
                        xtraTabControl1.SelectedTabPage = newPad;
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
                    XtraTabPage newPad = xtraTabControl1.TabPages.Add();
                    newPad.Text = GetTabPageName(model.ModelName); //显示简短的文件名
                    //newPad.Tooltip = model.ModelName;//文件全名
                    newPad.Controls.Add(pad);
                    newPad.Tag = model;
                    m_CurrentModel = model;
                    m_TempModelPages.Add(model, newPad);
                    xtraTabControl1.SelectedTabPage = newPad;
                }
            }

        }

        /// <summary>
        /// 插入病历编辑器的内容
        /// </summary>
        /// <param name="pad"></param>
        /// <param name="model"></param>
        private void InsertEmrModelContent(PadForm pad, EmrModel model)
        {
            if (model.DailyEmrModel)
            {
                pad.InsertDailyInfo(model);
            }
            else
            {
                pad.LoadDocment(model);
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

                IEMREditor editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor));

                bool isLoadMainPage = false;
                if (container.ModelEditor == c_IEMMainPageDLL)//"病案首页"只需要加入现有的TabPage里
                {
                    isLoadMainPage = true;
                    newPad = xtraTabPageIEMMainPage;
                }
                else
                {
                    isLoadMainPage = false;
                    newPad = xtraTabControl1.TabPages.Add();
                }

                if (isLoadMainPage)
                {
                    SetWaitDialogCaption("正在加载患者病案数据");
                }

                //加载控件至页面
                editor.Load(m_app);

                newPad.Text = editor.Title;
                editor.DesignUI.Dock = DockStyle.Fill;
                newPad.Controls.Add(editor.DesignUI);
                //newPad.Controls.Add();
                newPad.Tag = container;
                m_CurrentEmrEditor = editor;
                m_TempContainerPages.Add(container, newPad);
                xtraTabControl1.SelectedTabPage = newPad;

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


        ///// <summary>
        ///// 增加新病程
        ///// </summary>
        //private void AddNewDailyContent(EmrModel model, out DateTime patRecTime)
        //{
        //    patRecTime = DateTime.Now;
        //    using (DailyTemplateForm daily = new DailyTemplateForm(DateTime.Now, model.ModelName))
        //    {

        //        if (daily.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            m_IsAddDailyEmrModel = true;
        //            model.DisplayTime = daily.CommitDateTime;
        //            model.Description = daily.CommitTitle;
        //            CurrentForm.InsertDailyInfo(model);
        //        }
        //        else
        //        {
        //            m_IsAddDailyEmrModel = false;
        //        }

        //    }
        //}

        private void BindOtherInfo()
        {
            gridControlTool.DataSource = m_patUtil.Symbols;
        }

        private void AddNewPatRec(EmrModel model, bool startNew)
        {
            TreeListNode node = treeList1.AppendNode(new object[] { model.ModelName }, treeList1.FocusedNode);
            treeList1.FocusedNode.ExpandAll();
            node.Tag = model;
            node.Visible = false;

            //设置病程相关信息
            if (model.DailyEmrModel)
            {
                bool isAdd = AddDailyEmrModel(model, startNew, node);
                if (!isAdd)
                {
                    return;
                }

            }//加载非病程类文件
            else
            {
                CreateNewDocument(model);
            }

            //填充宏
            FillModelMacro();
            ReplaceModelMacro();
            node.Visible = true;
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
            if (model.DailyEmrModel)//保证是病程
            {
                DateTime dt = DateTime.Now;

                //设置病程的标题和时间等信息
                if (!ProcessDailyTemplateForm(model, node, out dt))
                {
                    //取消对病程的设置，则取消新病程的添加
                    DeleteNodeNotTriggerEvent(node);
                    return false;
                }

                if (startNew)//新增首程
                {
                    CreateNewDocument(model);
                }
                else//除首程外的其他病程
                {
                    //将新增的病程插入到当前编辑器中
                    InsertEmrModelContent(CurrentForm, model);
                    //清空除首程以外的病程
                    model.ModelContent = new XmlDocument();
                }

                //首程得到编辑器界面所有的内容
                m_CurrentModel.ModelContent.PreserveWhitespace = false;
                //m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());

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

        void treeList1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //TreeListNode node = treeList1.FocusedNode;
            //if ((node == null) || (node.Tag == null) || (node.Tag is EmrModel)) return;


            //EmrModelContainer container = (EmrModelContainer)node.Tag;

            ////添加病历相关文件
            //if ((container.EmrContainerType != ContainerType.None) && (!string.IsNullOrEmpty(container.ContainerCatalog)))
            //{

            //    string catalog = container.ContainerCatalog;
            //    EmrModel model = GetTemplate(catalog);
            //    if (model != null)
            //    {
            //        //bool daily = !catalog.Equals("AB");
            //        bool startnew = (catalog.Equals("AB") || (!node.HasChildren));
            //        AddNewPatRec(model, startnew);
            //        container.AddModel(model);
            //    }

            //}

            ////展现非病历容器内容
            //if ((container.EmrContainerType == ContainerType.None))
            //{
            //    if (m_TempContainerPages.ContainsKey(container))
            //    {
            //        //加载内容
            //        xtraTabControl1.SelectedTabPage = m_TempContainerPages[container];

            //    }
            //    else
            //    {
            //        CreateCommonDocement(container);
            //    }
            //}
        }

        #region IStartPlugIn 成员

        public IPlugIn Run(YidanSoft.FrameWork.WinForm.Plugin.IYidanEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            plg.PatientChanged += new PatientChangedHandler(plg_PatientChanged);
            m_app = host;
            m_RecordDal = new RecordDal(m_app.SqlHelper);
            return plg;
        }

        void plg_PatientChanged(object Sender, PatientArgs arg)
        {
            //CloseAllTabPage();

            CloseAllTabPages();

            InitDictionary();
            xtraTabControl1.SelectedPageChanged -= new TabPageChangedEventHandler(xtraTabControl1_SelectedPageChanged);
            xtraTabControl1.CloseButtonClick -= new EventHandler(xtraTabControl1_CloseButtonClick);
            RemoveTabPageExcludFirst();
            xtraTabControl1.SelectedPageChanged += new TabPageChangedEventHandler(xtraTabControl1_SelectedPageChanged);
            xtraTabControl1.CloseButtonClick += new EventHandler(xtraTabControl1_CloseButtonClick);

            GetPatRec();
            BindExtraInfo();
            SetBarStatus(false);
        }

        private void RemoveTabPageExcludFirst()
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

        #endregion

        #region 针对右侧工具栏的Event
        private void navBarControlToolBox_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
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
                if (treeListSymb.Nodes.Count < 1)
                {
                    treeListSymb.BeginUnboundLoad();
                    foreach (DataRow row in m_patUtil.Symbols.Rows)
                    {
                        TreeListNode node = treeListSymb.AppendNode(new object[] { row["名称"] }, null);
                        node.Tag = row;
                    }
                    treeListSymb.EndUnboundLoad();
                }
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



        }

        private void gridViewMain_DoubleClick(object sender, EventArgs e)
        {
            if (!barButtonSave.Enabled) return;

            if (gridViewMain.FocusedRowHandle < 0) return;

            DataRow row = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
            if (row == null) return;

            DataTable table = (DataTable)gridControlTool.DataSource;
            if (table.TableName.Equals("image"))
            {
                //读取图片内容
                byte[] img = m_patUtil.GetImage(row["ID"].ToString());

                CurrentForm.zyEditorControl1.EMRDoc._InsertImage(img);

            }
            else if (table.TableName.Equals("zd"))
            {
                DataTable sourceTable = m_patUtil.GetZDDetailInfo(row["D_NAME"].ToString());
                ChoiceForm choice = new ChoiceForm(m_app.SqlHelper, sourceTable);
                if (choice.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CurrentForm.zyEditorControl1.EMRDoc._InserString(choice.CommitRow["D_NAME"].ToString());
                }

            }
            else if (table.TableName.Equals("Macro"))
            {
                Macro mac = new Macro(row);

                MacroUtil.FillMarcValue(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), mac);
                if (!string.IsNullOrEmpty(mac.MacroValue))
                    CurrentForm.zyEditorControl1.EMRDoc._InserString(mac.MacroValue);
            }
            else if (table.TableName.Equals("tz"))
            {
                byte[] tz = m_patUtil.GetTZDetailInfo(row["名称"].ToString());
                CurrentForm.zyEditorControl1.EMRDoc.InsertTemplateFile(tz);
            }
            else if (table.TableName.Equals("symbols"))
            {
                if (!row.IsNull("名称"))
                {
                    CurrentForm.zyEditorControl1.EMRDoc._InserString(row["名称"].ToString());
                }

            }
            else if (table.TableName.Equals("zz"))
            {
                byte[] zz = m_patUtil.GetZZItemDetailInfo(row["名称"].ToString());
                CurrentForm.zyEditorControl1.EMRDoc.InsertTemplateFile(zz);
            }


        }

        private void navBarControlClinical_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {

            if (e.Group == navBarGroup_Bl)//病历提取 todo: 2011-07-14
            {
                gridControlCollect.DataSource = m_patUtil.GetDocCollectContent();
            }

            else if (e.Group == navBarGroup_Jy)//医技
            {
                //gridControlTool.MainView = gridView_YJ;
                //navBarGroup_Jy.ControlContainer.Controls.Add(gridControlTool);
                if (m_patUtil.Reports != null)
                {
                    m_patUtil.Reports.DefaultView.RowFilter = "RePortCatalog='LIS'";
                    gridControlLIS.DataSource = m_patUtil.Reports;
                }
            }
            else if (e.Group == navBarGroup_Jc)
            {
                gridControlTool.MainView = gridView_YJ;
                navBarGroup_Jc.ControlContainer.Controls.Add(gridControlTool);
                //if (m_patUtil.Reports != null)
                //{
                //    m_patUtil.Reports.DefaultView.RowFilter = "RePortCatalog='RIS'";
                //    gridControlTool.DataSource = m_patUtil.Reports;
                //}
            }
            else if (e.Group == navBarGroup_DoctorAdive)//医嘱
            {
                navBarGroup_DoctorAdive.ControlContainer.Controls.Add(gridControlTool);
                gridControlTool.MainView = gridView_Yz;
            }
        }

        private void gridView_YJ_DragObjectDrop(object sender, DevExpress.XtraGrid.Views.Base.DragObjectDropEventArgs e)
        {

        }

        private void gridViewLIS_DoubleClick(object sender, EventArgs e)
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
        #endregion

        #region 设置TreeList图片
        private void treeList1_StateChanged(object sender, EventArgs e)
        {

        }

        private void treeList1_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
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

            SetLockImage(e);
        }

        private void SetLockImage(DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            if (e.Node == null) return;

            if (DoctorEmployee.Kind == EmployeeKind.Nurse)//当前登录人是护士
            {
                bool isNurseNode = false;
                FindNurseDoc(ref isNurseNode, e.Node);
                if (!isNurseNode)
                {
                    SetLockFileAndFolderNode(e);
                }
            }
            else if (DoctorEmployee.Kind == EmployeeKind.Doctor || DoctorEmployee.Kind == EmployeeKind.Specialist)//当前登录人是医生
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

        private void SetLockFileAndFolderNode(DevExpress.XtraTreeList.GetStateImageEventArgs e)
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

        #region 供外部调用
        public void GetStateImage()
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
        }

        private void SetLockImage(TreeListNode node)
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
            else if (DoctorEmployee.Kind == EmployeeKind.Doctor || DoctorEmployee.Kind == EmployeeKind.Specialist)//当前登录人是医生
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

        /// <summary>
        /// 设置被锁定的文件夹和文件的图片
        /// </summary>
        /// <param name="e"></param>
        private void SetLockFileAndFolderNode(TreeListNode node)
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
        #endregion

        private void FindNurseDoc(ref bool flag, TreeListNode node)
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

        #endregion

        #region 添加病案首页
        /*
        /// <summary>
        /// 保存界面中TreeList中的“病案首页”节点
        /// </summary>
        TreeListNode m_NodeIEMMainPage;

        /// <summary>
        /// 加载病案首页
        /// </summary>
        private void LoadIEMMainPage()
        {
            m_NodeIEMMainPage = null;
            GetIEMMainPageNode(null);
            if (m_NodeIEMMainPage != null)
            {
                EmrModelContainer container = (EmrModelContainer)m_NodeIEMMainPage.Tag;
                if (container != null)
                {
                    CreateCommonDocement(container);
                }
            }
        }

        /// <summary>
        /// 获取TreeList中“病案首页”节点
        /// </summary>
        /// <param name="node"></param>
        private void GetIEMMainPageNode(TreeListNode node)
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
                    //【程序集的名称 + 继承了IEMREditor接口的界面， 例如：YidanSoft.Core.IEMMainPage.UCMainPage, YidanSoft.Core.IEMMainPage】
                    if (container.ModelEditor == IEMMainPageDLL)
                    {
                        m_NodeIEMMainPage = subNode;
                        break;
                    }
                }
                GetIEMMainPageNode(subNode);
            }
        }
        */
        #endregion

        #region 点击 TreeList
        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
            //if ((treeList1.FocusedNode == null) || (treeList1.FocusedNode.Tag == null)) return;

            //LoadModelFromTree(treeList1.FocusedNode);
        }

        private void treeList1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)//右键弹出菜单
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
                    popupMenuInTreeList.ShowPopup(treeList1.PointToScreen(new Point(e.X, e.Y)));
                }
            }
        }

        /// <summary>
        /// 设置点击病历文件夹Button的显示情况
        /// </summary>
        /// <param name="node"></param>
        private void SetEmrModelContainerButtonVisiable(TreeListNode node)
        {
            //保存，删除，提交，审核 不可见
            btnSavePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnDeletePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnSubmitPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnConfirmPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //模板 可见
            btnModulePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            SetPopupBarButton();

            //针对首页、三测单之类非病历类型的界面需要保存功能
            if (((EmrModelContainer)node.Tag).EmrContainerType == ContainerType.None
                && ((EmrModelContainer)node.Tag).ModelEditor == c_IEMMainPageDLL)
            {
                btnSavePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
        }

        /// <summary>
        /// 设置点击病历模板Button的显示情况
        /// </summary>
        /// <param name="node"></param>
        private void SetEmrModelButtonVisiable(TreeListNode node)
        {
            //保存，删除，提交，审核 可见
            btnSavePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            btnDeletePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            btnSubmitPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            btnConfirmPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            //模板 不可见
            btnModulePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            SetPopupBarButton();
            SetDailyEmrAllowDelete(node);
        }

        /// <summary>
        /// 设置弹出BarButton
        /// </summary>
        private void SetPopupBarButton()
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
            if (!barButton_Audit.Enabled)
            {
                btnConfirmPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                btnConfirmPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
        }

        /// <summary>
        /// 设置针对病程的删除按钮的使用情况
        /// </summary>
        /// <param name="node"></param>
        private void SetDailyEmrAllowDelete(TreeListNode node)
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
        #endregion

        #region BarButton ItemClick

        #region print

        private void barButtonPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (treeList1.FocusedNode == null || (treeList1.FocusedNode.Tag == null))
                return;

            DataRow row = (DataRow)treeList1.Tag;
            //判断是否需要需要
            bool reslut = true;

            if (reslut)
            {
                //病程打印
            }
            else
            {
                //打印
            }
        }

        private void barButtonItem_Preview_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetEmrReadOnly2();
            PrientViewDocment();
            SelectedPageChanged(xtraTabControl1.SelectedTabPage, true);
        }

        private void barButtonItem_JumpPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetEmrReadOnly2();
            JumpPrint();
            SelectedPageChanged(xtraTabControl1.SelectedTabPage, true);
        }

        private void barButtonItem_Print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetEmrReadOnly2();
            PrintDocment();
            SelectedPageChanged(xtraTabControl1.SelectedTabPage, true);
        }

        #endregion

        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Undo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                Undo();
            }
        }

        /// <summary>
        /// 重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Redo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                Redo();
            }
        }

        /// <summary>
        /// 插入表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_InsertTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                InsertTable();
            }
        }

        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_InsertRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                InsertTableRow();
            }
        }

        /// <summary>
        /// 插入列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_InsertCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                InsertTableColumn();
            }
        }

        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_DeletTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                DeleteTable();
            }
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_DeletCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                DeletTableColumn();
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_DeletRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                DeletTableRow();
            }
        }

        /// <summary>
        /// 选择表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barCheck_ChooseTable_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                ChoiceTable();
            }
        }

        /// <summary>
        /// 单元格信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_CellInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                SetTableProperty();
            }
        }

        /// <summary>
        /// 只读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmrReadOnly_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetEmrReadOnly();
        }

        private void SetEmrReadOnly()
        {
            if (m_CurrentModel != null)
            {
                SetDocmentReadOnlyMode();
            }
        }

        private void SetEmrReadOnly2()
        {
            if (m_CurrentModel != null)
            {
                SetDocmentReadOnlyMode();
                this.CurrentForm.zyEditorControl1.ActiveEditArea = null;
            }
        }

        /// <summary>
        /// 申请编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetEmrAllowEdit();
        }

        private void SetEmrAllowEdit()
        {
            if (m_CurrentModel != null)
            {
                CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Edit;
                CurrentForm.zyEditorControl1.Refresh();
            }
        }
        #endregion

        #region Popup ItemClick

        /// <summary>
        /// 全部保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void SaveAll()
        {
            //foreach (TabPage page in xtraTabControl1.TabPages)
            //{
            //    if (page.Controls.Count > 0)
            //    {
            //        PadForm form = page.Controls[0] as PadForm;
            //        if (form != null)
            //        {
            //            m_TempModelPages.get
            //        }
            //    }
            //}
        }
        #endregion

        #region  toolbar editor



        private void btnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.CurrentForm.zyEditorControl1.EMRDoc._Copy();
        }

        private void btnPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.CurrentForm.zyEditorControl1.EMRDoc._Paste();
        }

        private void barButtonItem_Cut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.CurrentForm.zyEditorControl1.EMRDoc._Cut();
        }

        private void barButtonItem_Undo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.CurrentForm.zyEditorControl1.EMRDoc._Undo();
        }

        private void barButtonItem_Redo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.CurrentForm.zyEditorControl1.EMRDoc._Redo();
        }

        private void barEditItem_Font_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barEditItem_FontSize_EditValueChanged(object sender, EventArgs e)
        {
            if (barEditItem_FontSize.EditValue == null) return;

            float size = FontCommon.GetFontSizeByName(barEditItem_FontSize.EditValue.ToString());
            this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontSize(size);
        }

        private void barButtonItem_Bold_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {


            //确定按钮的状态
            this.barButtonItem_Bold.Down = this.barButtonItem_Bold.Down ? true : false;
            ReSetSupSub();
            this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontBold(this.barButtonItem_Bold.Down);
        }

        private void barButtonItem_Italy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            this.barButtonItem_Italy.Down = this.barButtonItem_Italy.Down ? true : false;
            this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontItalic(this.barButtonItem_Italy.Down);
        }

        private void barButtonItem_Undline_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_Undline.Down = this.barButtonItem_Undline.Down ? true : false; ;
            this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionUnderLine(this.barButtonItem_Undline.Down);
        }

        private void barButtonItem_Sup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_Sup.Down = this.barButtonItem_Sup.Down ? true : false;
            ReSetSupSub();
            this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSup(barButtonItem_Sup.Down);

        }

        private void barButtonItem_Sub_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.barButtonItem_Sub.Down = this.barButtonItem_Sub.Down ? true : false; ;
            ReSetSupSub();
            this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSub(barButtonItem_Sub.Down);
        }

        private void barEditItem_Font_EditValueChanged(object sender, EventArgs e)
        {
            if (barEditItem_Font.EditValue == null) return;
            this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectioinFontName(this.barEditItem_Font.EditValue.ToString());
        }
        #endregion

        #region 大纲视图

        private void panelContainer2_ActiveChildChanged(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            if (panelContainer2.ActiveChild == dockPanelView)
            {
                if (CurrentForm != null)
                {
                    CurrentForm.zyEditorControl1.EMRDoc.GetBrief(treeView1);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = this.treeView1.SelectedNode;//this.treeView1.GetNodeAt(e.Location);
            if (node != null)
            {
                CurrentForm.zyEditorControl1.EMRDoc.Content.AutoClearSelection = true;
                CurrentForm.zyEditorControl1.EMRDoc.Content.MoveSelectStart((node.Tag as ZYFixedText).FirstElement);
                CurrentForm.zyEditorControl1.ScrollViewtopToCurrentElement();
            }
        }
        #endregion

        #region 工具栏

        /// <summary>
        /// 初始化病历提取列表
        /// </summary>
        private void GetDataCollect()
        {
            gridControlCollect.DataSource = m_patUtil.GetDocCollectContent();
        }

        private void gridControlCollect_MouseUp(object sender, MouseEventArgs e)
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

        private void btnInsertCollect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InsertCollect();
        }

        private void btnDeleteCollect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteDataCollect();
        }

        private void gridControlCollect_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridHitInfo hitInfo = gridViewDataCollect.CalcHitInfo(e.Location);
            if (hitInfo.RowHandle >= 0)
            {
                InsertCollect();
            }
        }

        private void InsertCollect()
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

        private void DeleteDataCollect()
        {
            DataRowView dr = gridViewDataCollect.GetRow(gridViewDataCollect.FocusedRowHandle) as DataRowView;
            m_patUtil.CancelDocCollectContent(dr["ID"].ToString());
            gridViewDataCollect.DeleteRow(gridViewDataCollect.FocusedRowHandle);
        }

        #endregion

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                SetWaitDialogCaption("正在保存病历,请稍等");
                SaveDocment(m_CurrentModel);
                this.HideWaitDialog();
            }
            else if (m_CurrentEmrEditor != null)
            {
                SetWaitDialogCaption("正在保存,请稍等");
                // 针对非病历类型的界面 如首页、三测单等
                m_CurrentEmrEditor.Save();
                this.HideWaitDialog();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSavePopup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                SetWaitDialogCaption("正在保存病历,请稍等");
                SaveDocment(m_CurrentModel);
                this.HideWaitDialog();
            }
            else if (m_CurrentEmrEditor != null)
            {
                SetWaitDialogCaption("正在保存,请稍等");
                // 针对非病历类型的界面 如首页、三测单等
                m_CurrentEmrEditor.Save();
                this.HideWaitDialog();
            }
        }

        /// <summary>
        /// 保存，供外部调用
        /// </summary>
        public void SaveDocumentPublic()
        {
            if (m_CurrentModel != null)
            {
                SaveDocment(m_CurrentModel);
            }
        }

        private void SaveDocment(EmrModel model)
        {
            if (model == null) return;
            m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
            Save(model);
            CurrentForm.zyEditorControl1.EMRDoc.Modified = false;
            m_app.CustomMessageBox.MessageShow("病历保存成功！");
        }

        private void Save(EmrModel model)
        {
            if (model == null) return;

            //对应病程的病历做特殊处理
            if (model.DailyEmrModel)
            {
                //对病程的保存操作，实际上都是对首程的保存,除首程外的病程都没有内容
                if (m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Tag is EmrModelContainer)
                {
                    foreach (TreeListNode node in m_CurrentTreeListNode.ParentNode.Nodes)
                    {
                        model = (EmrModel)node.Tag;
                        SaveDocumentInner(model);
                    }
                }
            }
            else
            {
                SaveDocumentInner(model);
            }
        }

        private void SaveDocumentInner(EmrModel model)
        {
            try
            {
                //新增
                if (model.InstanceId == -1)
                {
                    model.CreatorXH = m_app.User.Id;
                    m_RecordDal.InsertModelInstance(model, Convert.ToInt32(m_app.CurrentPatientInfo.NoOfFirstPage));

                    //log


                }
                else//修改
                {
                    //
                    m_RecordDal.UpdateModelInstance(model, Convert.ToInt32(m_app.CurrentPatientInfo.NoOfFirstPage));
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("病历保存时产生异常！" + ex.Message + "");
            }
        }
        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDeletePopup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                if (m_app.CustomMessageBox.MessageShow("是否删除当前病历?", CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    DeleteTemplate();
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                if (m_app.CustomMessageBox.MessageShow("是否删除当前病历?", CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    DeleteTemplate();
                }
            }
        }

        public void DeleteDocumentPublic()
        {
            if (m_CurrentModel != null)
            {
                if (m_app.CustomMessageBox.MessageShow("是否删除当前病历?", CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    DeleteTemplate();
                }
            }
        }


        private void DeleteTemplate()
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
            //        m_RecordDal.UpdateModelInstance(model, Convert.ToInt32(m_app.CurrentPatientInfo.NoOfFirstPage));
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
                    m_CurrentModel.ModelContent.PreserveWhitespace = false;
                    m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                }

                if (emrModel.InstanceId < 0)
                {
                    //新增节点直接删除  先关闭TabPage，再删除树节点
                    DeleteTabPageAndNode(emrModel);
                }
                else
                {
                    //节点直接删除  1.关闭TabPage，2.删除树节点【作废RecordDetail中对应的一条记录】
                    DeleteTabPageAndNode(emrModel);
                    m_RecordDal.CancelEmrModel(emrModel.InstanceId);
                }

                m_app.CustomMessageBox.MessageShow("删除成功!", YidanSoft.Core.CustomMessageBoxKind.InformationOk);
                SetDailyEmrEditArea();
                treeList1.Refresh();
            }
        }

        #endregion

        #region 另存
        private void btn_SaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string contentXml = CurrentForm.zyEditorControl1.ToXMLString();
            if (m_CurrentModel.DailyEmrModel)
            {
                //contentXml = CurrentForm.zyEditorControl1.ActiveEditArea.
            }
            SaveAsForm saveAsForm = new SaveAsForm(m_CurrentModel, m_app, m_RecordDal, contentXml);
            saveAsForm.ShowDialog();
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
            if (m_CurrentModel != null)
            {
                SubmitDocment();
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Submit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                SubmitDocment();
            }
        }

        public void SubmitDocumentPublic()
        {
            if (m_CurrentModel != null)
            {
                SubmitDocment();
            }
        }

        private void SubmitDocment()
        {
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

        private void SubmitDocmentInner(EmrModel model)
        {
            if (model == null)
                return;
            try
            {
                bool isSubmitInOrder = true;
                //todo 需要安装顺序提交
                // 不按顺序提交时仅提醒
                //foreach (EmrModel model in DH.SelectedContainer.Models)
                //{
                //    if ((model.State == ExamineState.NotSubmit) && (model.Index < currentModel.Index))
                //    {
                //        isSubmitInOrder = false;
                //        break;
                //    }
                //}
                string msg;
                if (isSubmitInOrder)
                    msg = "确定要提交“{0}”吗？";
                else
                    msg = "确定要提交“{0}”吗？" + String.Format("（请按时间顺序依次提交{0}）", model.ModelName);

                // 实际上并没有审核病历，所以不能记录审核人，但是仍设置病历状态为相应值，以便于程序做逻辑控制

                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);

                switch (grade)
                {

                    case DoctorGrade.Resident:
                        model.State = ExamineState.SubmitButNotExamine;
                        break;
                    case DoctorGrade.Attending:
                        model.State = ExamineState.FirstExamine;
                        break;
                    case DoctorGrade.AssociateChief:
                    case DoctorGrade.Chief:
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
                model.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                Save(model);
                //TraceLog.InsertOperationLog(DH.CurrentPatInfo.NoOfFirstPage, DH.SelectedModel.InstanceId, DH.SelectedModel.Name, OperationType.Submit, doctorId);
                SetDocmentReadOnlyMode();

                CurrentForm.zyEditorControl1.ActiveEditArea = null;
                CurrentForm.zyEditorControl1.Refresh();
                m_app.CustomMessageBox.MessageShow("提交病历成功！病历不可再编辑");
                ResetEditModeAction(model);
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("提交病历时产生异常！" + ex.Message + "");
            }
        }
        #endregion

        #region 新增

        /// <summary>
        /// 模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnModulePopup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode node = treeList1.FocusedNode;
            if ((node == null) || (node.Tag == null) || (node.Tag is EmrModel)) return;

            AddEmrModel(node);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_new_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode node = treeList1.FocusedNode;
            if ((node == null) || (node.Tag == null) || (node.Tag is EmrModel)) return;

            AddEmrModel(node);
        }

        /// <summary>
        /// 增加病历
        /// </summary>
        /// <param name="node">TreeList.FocusedNode</param>
        private void AddEmrModel(TreeListNode node)
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
                    model.ModelName = model.ModelName + " " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + DoctorEmployee.Name;

                    bool startnew = (catalog.Equals("AB") || (!node.HasChildren));
                    AddNewPatRec(model, startnew);
                    container.AddModel(model);
                }
            }
        }

        /// <summary>
        /// 排除医患沟通
        /// </summary>
        /// <param name="dt"></param>
        private void GetActualTemplate(DataTable dt)
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

        /// <summary>
        /// 通过模板窗体得到病历模板
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private EmrModel GetTemplate(string catalog, TreeListNode node)
        {
            //病历模板列表
            DataTable sourceTable = m_patUtil.GetTemplate(catalog, DoctorEmployee.CurrentDept.Code);
            GetActualTemplate(sourceTable);
            //个人模板列表
            DataTable templatePersonTable = m_patUtil.GetTemplatePerson(catalog, m_app.User.Id);
            //病历选择模板界面
            CatalogForm form = new CatalogForm(sourceTable, templatePersonTable, m_RecordDal, DoctorEmployee);

            if (catalog == "AC")//病程
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
                //病历创建人
                form.CommitModel.CreatorXH = m_app.User.Id;
                //todo ? why ?
                //this.CurrentForm.AddGotFocusEvent();
                return form.CommitModel;
            }
            //todo why?
            //this.CurrentForm.AddGotFocusEvent();
            return null;
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
            if (m_CurrentModel != null)
            {
                AuditDocment();
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Audit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                AuditDocment();
            }
        }

        public void AuditDocumentPublic()
        {
            if (m_CurrentModel != null)
            {
                AuditDocment();
            }
        }

        private void AuditDocment()
        {
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

        private void AuditDocmentInner(EmrModel currentModel)
        {
            try
            {
                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);

                switch (grade)
                {
                    case DoctorGrade.Attending:
                        currentModel.State = ExamineState.FirstExamine;
                        break;
                    case DoctorGrade.AssociateChief:
                    case DoctorGrade.Chief:
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
                currentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                Save(currentModel);
                SetDocmentReadOnlyMode();

                CurrentForm.zyEditorControl1.ActiveEditArea = null;
                CurrentForm.zyEditorControl1.Refresh();
                m_app.CustomMessageBox.MessageShow("审核病历成功！病历不可再编辑");
                ResetEditModeAction(currentModel);
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("审核病历时产生异常！" + ex.Message + "");

            }
            //插入记录

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
            if (m_CurrentModel != null)
            {
                CheckDocment();
            }
        }

        /// <summary>
        /// 必填项检查
        /// </summary>
        private void CheckDocment()
        {
            DocumentModel model = this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel;

            this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = DocumentModel.Test;
            this.CurrentForm.zyEditorControl1.CheckMustClickElement();
            this.CurrentForm.zyEditorControl1.EMRDoc.Info.DocumentModel = model;
        }

        #endregion

        #region 病历提交
        /// <summary>
        /// 病历提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Collect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string errorMessage = "请选择要提取的病历内容!";
            string correctMessage = "病历提取成功!";
            if (CurrentForm == null)
            {
                m_app.CustomMessageBox.MessageShow(errorMessage, YidanSoft.Core.CustomMessageBoxKind.InformationOk);
                return;
            }

            string collectContent = CurrentForm.zyEditorControl1.EMRDoc.Content.GetSelectedText();
            if (collectContent != null && collectContent.Trim() != "")
            {
                m_patUtil.InsertDocCollectContent(collectContent);
                AddDocCollect(collectContent);
                m_app.CustomMessageBox.MessageShow(correctMessage, YidanSoft.Core.CustomMessageBoxKind.InformationOk);
            }
            else
            {
                m_app.CustomMessageBox.MessageShow(errorMessage, YidanSoft.Core.CustomMessageBoxKind.InformationOk);
            }
        }

        private void AddDocCollect(string content)
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

        private DataTable InitDataCollect()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CONTENT");
            return dt;
        }
        #endregion

        #region 根据权限、病历状态、登录人决定BarButton和PopupButton的显示

        /// <summary>
        /// 针对病历或容器BtnButton的可用情况
        /// </summary>
        /// <param name="b"></param>
        private void SetBarStatusModelOrContainer(bool b)
        {
            btn_SaveAs.Enabled = b;//另存
            btn_Collect.Enabled = b;//抽取
        }

        private void SetBarStatus(bool enable)
        {
            btn_new.Enabled = enable;//新增
            barButton_Delete.Enabled = enable;//删除
            barButton_Submit.Enabled = enable;//提交
            barButton_Audit.Enabled = enable;//审核
            btn_SaveAs.Enabled = enable;//另存
            CanEditEmrDoc(enable);
        }

        /// <summary>
        /// 处于审核状态时BarButton可以使用的情况
        /// </summary>
        /// <param name="b"></param>
        private void AuditStateBarButton(bool b)
        {
            barButton_Audit.Enabled = b;

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

        /// <summary>
        /// 可以编辑就可以用的BarButton【不包括提交和删除】
        /// </summary>
        /// <param name="b"></param>
        private void CanEditEmrDoc(bool b)
        {
            btnCopy.Enabled = b;//复制
            btnPaste.Enabled = b;//粘贴
            barButtonItem_Cut.Enabled = b;//剪切
            barButtonItem_Undo.Enabled = b;//撤销
            barButtonItem_Redo.Enabled = b;//重复
            barEditItem_Font.Enabled = b;//字体
            barEditItem_FontSize.Enabled = b;//字体大小
            barButtonItem_Bold.Enabled = b;//加粗
            barButtonItem_Italy.Enabled = b;//斜体
            barButtonItem_Undline.Enabled = b;//下划线
            barButtonItem_Sup.Enabled = b;//下标
            barButtonItem_Sub.Enabled = b;//上标
            barSubItem1.Enabled = b;//表格
            btn_Edit.Enabled = b;//编辑
            btnEmrReadOnly.Enabled = b;//只读
            barButtonSave.Enabled = b;//保存
            barButton_Check.Enabled = b;//自检
            barButton_Delete.Enabled = b;//删除
        }

        /// <summary>
        /// 重设编辑模式下的Action状态
        /// </summary>
        private void ResetEditModeAction(EmrModel model)
        {
            if (model == null) return;

            SetBarStatus(false);//重置所有的BarButton
            SetBarStatusModelOrContainer(true);

            IEmrModelPermision modelPermision;
            bool b;

            #region 根据权限
            YidanSoft.Common.Eop.Employee empl = new YidanSoft.Common.Eop.Employee(m_app.User.Id);
            empl.ReInitializeProperties();
            modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Audit, empl);
            b = modelPermision.CanDo(model);
            if (b) // 文件已提交，处于审核模式，且有审核权限
            {
                if (model.State == ExamineState.FirstExamine //主治医已经检查完成
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
                    modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Edit, empl);
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
                    modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Submit, empl);
                    b = modelPermision.CanDo(model);
                    barButton_Submit.Enabled = b;

                    // 删除——新增状态，未归档，本人创建
                    modelPermision = ModelPermisionFactroy.Create(ModelPermisionType.Delete, empl);
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

            ControlTreeListNodeByUser();
        }

        /// <summary>
        /// 重设编辑模式下的Action状态
        /// </summary>
        /// <param name="container"></param>
        private void ResetEditModeAction(EmrModelContainer container)
        {
            if (container == null) return;

            SetBarStatusModelOrContainer(false);

            if (container.EmrContainerType == ContainerType.None)
            {
                //非病历类型的界面 如：首页、三测单没有删除和添加功能
                btn_new.Enabled = false;
                CanEditEmrDoc(false);

                btnSubmitPopup.Enabled = false;
                barButton_Submit.Enabled = false;
                barButton_Audit.Enabled = false;
                btnConfirmPopup.Enabled = false;
                barButton_Submit.Enabled = false;
                barButton_Audit.Enabled = false;

                //病案首页默认显示“保存”
                if (container.ModelEditor == c_IEMMainPageDLL)
                {
                    barButtonSave.Enabled = true;
                    btnSavePopup.Enabled = true;
                }
            }
            else
            {
                btn_new.Enabled = true;
                CanEditEmrDoc(false);
                barButton_Submit.Enabled = false;
                barButton_Audit.Enabled = false;
            }

            ControlTreeListNodeByUser();
        }

        private void ControlTreeListNodeByUser()
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
            else if (DoctorEmployee.Kind == EmployeeKind.Doctor || DoctorEmployee.Kind == EmployeeKind.Specialist)//当前登录人是医生
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

        /// <summary>
        /// 不可编辑
        /// </summary>
        private void SetSetEmrDocNotEdit()
        {
            CanEditEmrDoc(false);
            SetDocmentReadOnlyMode();
            btn_new.Enabled = false;
            barButtonSave.Enabled = false;
            barButton_Submit.Enabled = false;
            barButton_Delete.Enabled = false;
        }

        private void SetBarButtonStatusNurse(TreeListNode node, ref bool isNurseNode)
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

        private void SetBarButtonStatusDoctor(TreeListNode node, ref bool isDoctorNode)
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
        #endregion

        #region 导出
        private void barButtonExportFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExportFile();
        }

        /// <summary>
        /// 导出病历文件
        /// </summary>
        private void ExportFile()
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

        private void barButton_ExportXml_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExprotXML();
        }

        /// <summary>
        /// 导出XML文件
        /// </summary>
        private void ExprotXML()
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
        #endregion

        #region 查找
        private void barButtonFind_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            find();
        }

        /// <summary>
        /// 查找
        /// </summary>
        private void find()
        {
            if (CurrentForm == null) return;
            SearchFrm searchfrm = new SearchFrm(CurrentForm.zyEditorControl1);
            searchfrm.Show(this);
        }
        #endregion

        #region TreeList 提示
        private void InitToolTip(string displayName)
        {
            if (displayName.Trim() != "")
            {
                toolTipControllerTreeList.SetToolTip(treeList1, displayName);
            }
        }

        private void treeList1_MouseMove(object sender, MouseEventArgs e)
        {
            TreeListHitInfo hitInfo = treeList1.CalcHitInfo(treeList1.PointToScreen(e.Location));
            if (hitInfo.Node == null) return;

            InitToolTip(hitInfo.Node.GetValue("colName").ToString());
        }
        #endregion

        #region 得到Button的状态,供外部调用

        public bool GetSaveButtonEnable()
        {
            return barButtonSave.Enabled;
        }

        public bool GetDeleteButtonEnable()
        {
            return barButton_Delete.Enabled;
        }

        public bool GetSubmitButtonEnable()
        {
            return barButton_Submit.Enabled;
        }

        public bool GetAuditButtonEnable()
        {
            return barButton_Audit.Enabled;
        }

        #endregion

        #region

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //CloseAllTabPage();
            CloseAllTabPages();
        }

        private void CloseAllTabPages()
        {
            for (int i = xtraTabControl1.TabPages.Count - 1; i >= 0; i--)
            {
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
                                        YidanSoft.Core.CustomMessageBoxKind.QuestionYesNo) != System.Windows.Forms.DialogResult.Yes) return;

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
                    m_app.CustomMessageBox.MessageShow("保存成功", YidanSoft.Core.CustomMessageBoxKind.InformationOk);
                }
            }
            catch(Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("保存病历出错", YidanSoft.Core.CustomMessageBoxKind.ErrorYes);
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
        #endregion

        #region 转换TabPage的名字
        private string GetTabPageName(string modelName)
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
        #endregion

        #region 更改TreeListNode和TabPage的名字，针对在病程中修改病程名称和时间
        public void ReSetTreeListNodeAndTabPageName(string oldDateTime, string oldTitle, string newDateTime, string newTitle)
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

                    model.ModelName = (newTitle == null ? title : newTitle) + " " + (newDateTime == null ? date + " " + time : newDateTime) + " " + creater;
                    model.DisplayTime = newDateTime == null ? model.DisplayTime : Convert.ToDateTime(newDateTime);
                    model.Description = model.ModelName;
                    node.SetValue("colName", model.ModelName);
                    xtraTabControl1.SelectedTabPage.Text = model.ModelName;
                }
            }
        }
        #endregion

        #region 得到TreeList中具有焦點的Node
        public TreeListNode GetFocusNode()
        {
            return treeList1.FocusedNode;
        }
        #endregion

        #region 展開子節點的同時收縮同級的節點，保證在同級中只有一個節點處於展開的狀態，暫時不用
        private void treeList1_BeforeExpand(object sender, BeforeExpandEventArgs e)
        {
            //    TreeListNode parentNode = e.Node.ParentNode;
            //    if (parentNode != null && parentNode.Nodes.Count > 0)
            //    {
            //        for (int i = 0; i < parentNode.Nodes.Count; i++)
            //        {
            //            TreeListNode node = parentNode.Nodes[i];
            //            if (node != e.Node)
            //            {
            //                node.Expanded = false;
            //            }
            //        }
            //    }
        }
        #endregion

        #region 右侧工具栏拖拽实现

        private void DoDragDorpInfo(TreeList tree)
        {
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

                        MacroUtil.FillMarcValue(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), mac);
                        if (!string.IsNullOrEmpty(mac.MacroValue))
                            data = new KeyValuePair<string, object>(mac.MacroValue, ElementType.Text);
                    }
                    else if (tree.Name == treeListImage.Name)
                    {
                        byte[] img = m_patUtil.GetImage(row["ID"].ToString());

                        data = new KeyValuePair<string, object>("ZYTextImage", img);
                    }
                }

            }
            if (!string.IsNullOrEmpty(data.Key))
                tree.DoDragDrop(data, DragDropEffects.All);


        }

        private void treeList_MouseDown(object sender, MouseEventArgs e)
        {
            TreeList list = (TreeList)sender;
            DoDragDorpInfo(list);

        }
        #endregion

        #region 科室小模板相关实现
        const string sql_queryTree = "select * from emrtemplet_item_person_catalog where deptid='{0}'";
        const string sql_queryLeaf = "select * from emrtemplet_item_person where deptid='{0}'";
        private void barButtonItemPerson_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_PersonItem == null)
                m_PersonItem = new PersonItemManager(m_app);
            m_PersonItem.ShowDialog();
            InitPersonTree(true, string.Empty);

        }

        private void InitPersonTree(bool refresh, string catalog)
        {
            if ((m_MyTreeFolders == null) || (refresh))
            {
                m_MyTreeFolders = m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryTree, m_app.User.CurrentDeptId));
                m_MyLeafs = m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryLeaf, m_app.User.CurrentDeptId));
            }
            DataRow[] rows = m_MyTreeFolders.Select("(Previd='' or Previd is null)  and container ='" + catalog + "'");
            treeListPerson.BeginUnboundLoad();
            treeListPerson.Nodes.Clear();
            foreach (DataRow dr in rows)
            {
                LoadTree(dr["ID"].ToString(), dr, null, catalog);
            }
            treeListPerson.EndUnboundLoad();
            treeListPerson.ExpandAll();
        }

        private void LoadTree(string id, DataRow row, TreeListNode node, string catalog)
        {



            TreeListNode nd = treeListPerson.AppendNode(new object[] { id, row["NAME"], "Folder" }, node);
            nd.Tag = row;

            //查找叶子节点
            DataRow[] leafRows = m_MyLeafs.Select("PARENTID='" + id + "' ");
            if (leafRows.Length > 0)
            {
                foreach (DataRow leaf in leafRows)
                {
                    TreeListNode leafnd = treeListPerson.AppendNode(new object[] { leaf["Code"], leaf["NAME"], "Leaf" }, nd);
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

        private void treeListPerson_MouseDown(object sender, MouseEventArgs e)
        {
            TreeList list = (TreeList)sender;
            if ((list.FocusedNode != null) && (list.FocusedNode.Tag is TempletItem))
                DoDragDorpInfo(list);
        }
        private void treeListPerson_AfterExpand(object sender, NodeEventArgs e)
        {
            if (e.Node.HasChildren)
                e.Node.ExpandAll();
        }

        #endregion

        #region 打印 add by wwj 2011-08-24
        private void barButtonItemEmrPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel == null) return;
            if (CurrentForm == null) return;

            PrintForm printForm = new PrintForm(m_patUtil, m_CurrentModel, CurrentForm.zyEditorControl1.CurrentPage.PageIndex);
            printForm.ShowDialog();
        }
        #endregion
    }
}
