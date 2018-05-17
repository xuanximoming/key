using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraNavBar;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common.Eop;
using DrectSoft.Core.EMR_NursingDocument.EMRInput.HelpForm;
using DrectSoft.Core.EMR_NursingDocument.EMRInput.HistoryEMR;
using DrectSoft.Core.EMR_NursingDocument.EMRInput.Table.ThreeLevelCheck;
using DrectSoft.Core.EMR_NursingDocument.PublicSet;
using DrectSoft.Core.EMR_NursingDocument.UserContorls;
using DrectSoft.Core.IEMMainPage;
using DrectSoft.Core.OwnBedInfo;
using DrectSoft.Core.PersonTtemTemplet;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Common;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Library.EmrEditor.Src.Gui;
using DrectSoft.Library.EmrEditor.Src.Print;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

/*
 * 添加病历节点的时候需要注意按如下顺序，否则会导致最后一步SelectedPageChanged事件计算的不正确：
 * 1.在树中添加节点
 * 2.在节点的Tag中添加EmrModel
 * 3.设置界面的TabPage
 * 4.设置当前TabControl选中TabPage 【xtraTabControl1.SelectedTabPage = newPad】
 * 5.触发TabControl事件xtraTabControl1_SelectedPageChanged，计算出当前病历界面 “对应树中的节点”，“当前的EmrModel”，“当前的EmrModelContainer”
 */
namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class UCEmrInput : DevExpress.XtraEditors.XtraUserControl //, IStartPlugIn
    {
        #region Field && Property
        PersonItemManager m_PersonItem;
        DataTable m_MyTreeFolders;
        DataTable m_MyLeafs;
        private IEmrHost m_app;
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
            m_WaitDialog = new WaitDialogForm("创建病历编辑组件……", "请稍等。");
            InitializeComponent();
            xtraTabPageIEMMainPage.AccessibleName = ""; //为空则病案首页可以编辑，否则不显示编辑按钮
            MacroUtil.MacroSource = null;
            ExtraInit();
            InitDictionary();
            InitFont();
            InitDefaultFont();
            HideWaitDialog();

        }



        #region 绑定病人列表
        /// <summary>
        /// 绑定病人列表
        /// add by ywk 2012年8月7日 14:58:24 
        /// 此处的病人要选中上个窗体选择的病人
        /// </summary>
        private void BindPatInfos()
        {
            DataTable dtSource = GetPatientDataInner(3, string.Empty, "N");
            string filter = string.Format(@"  yebz='0' ");
            //gridMain.DataSource = m_DataManager.TableWard;
            if (dtSource != null)
            {
                dtSource.DefaultView.RowFilter = filter;
            }
            //处理含有婴儿的情况  ywk
            DataTable newDt = dtSource.DefaultView.ToTable();
            string ResultName = string.Empty;//声明最终要在列表显示的姓名的内容
            for (int i = 0; i < newDt.Rows.Count; i++)
            {
                ResultName = DataManager.GetPatsBabyContent(m_app, newDt.Rows[i]["noofinpat"].ToString());
                newDt.Rows[i]["PatName"] = ResultName;
            }
            this.gridControl1.DataSource = newDt;

            for (int i = 0; i < this.gridView1.RowCount; i++)
            {
                DataRow row = this.gridView1.GetDataRow(i);
                if (row["noofinpat"].ToString() == m_app.CurrentPatientInfo.NoOfFirstPage.ToString())
                {
                    this.gridView1.FocusedRowHandle = i;
                    break;
                }
            }
        }

        private DataTable GetPatientDataInner(int queryType, string strNurs, string strBed)
        {
            DataTable dataTablePatientData = new DataTable();
            string wardAndDeptRelationship = BasicSettings.GetStringConfig("WardAndDeptRelationship");
            if (wardAndDeptRelationship == "1")//1：一个病区包含多个科室  2：一个科室包含多个病区
            {
                dataTablePatientData = GetPatientInfoData(m_app.User.CurrentDeptId, m_app.User.CurrentWardId);
                //col_ksmc.Visible = true;
            }
            else
            {
                //读取主框架的患者信息
                dataTablePatientData = m_app.PatientInfos.Tables[1]; //GetPatientData(queryType, strNurs, strBed);
                //col_ksmc.Visible = false;
            }

            //List<string> listAttendLevel = GetAttendLevel();

            //for (int i = dataTablePatientData.Rows.Count - 1; i >= 0; i--)
            //{
            //    if (!listAttendLevel.Contains(dataTablePatientData.Rows[i]["AttendLevel"].ToString())
            //        && dataTablePatientData.Rows[i]["PatID"].ToString().Trim() != "")
            //    {
            //        dataTablePatientData.Rows.RemoveAt(i);
            //    }
            //}

            //lbl_Total.Text = dataTablePatientData.Rows.Count + " 张床位";

            return dataTablePatientData;
        }

        private DataTable GetPatientInfoData(string dept, string ward)
        {
            SqlParameter[] SqlParams = new SqlParameter[] { 
                  new SqlParameter("Deptids", SqlDbType.VarChar, 8),
                  new SqlParameter("Wardid", SqlDbType.VarChar, 8)
               };
            SqlParams[0].Value = dept; // 科室代码
            SqlParams[1].Value = ward; // 病区代码
            DataTable table1 = m_app.SqlHelper.ExecuteDataTable("usp_QueryInwardPatients2", SqlParams, CommandType.StoredProcedure);
            table1.TableName = "床位信息";
            table1.Columns["BedID"].Caption = "床位";
            table1.Columns["PatName"].Caption = "患者姓名";
            table1.Columns["Sex"].Caption = "性别";
            table1.Columns["AgeStr"].Caption = "年龄";
            table1.Columns["PatID"].Caption = "住院号";
            table1.Columns["AdmitDate"].Caption = "入院日期";
            return table1;
        }
        #endregion


        public void SetInnerVar(IEmrHost app, RecordDal recordDal)
        {
            m_app = app;
            m_RecordDal = recordDal;
        }

        private void RegisterEvent()
        {
            btnSavePopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnSavePopup_ItemClick);
            btnDeletePopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnDeletePopup_ItemClick);
            btnSubmitPopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnSubmitPopup_ItemClick);
            btnConfirmPopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnConfirmPopup_ItemClick);
            btnModulePopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(btnModulePopup_ItemClick);
            btnCancelAuditPopup.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(barButtonItem_CancelAudit_ItemClick);
            treeList1.BeforeExpand += new BeforeExpandEventHandler(treeList1_BeforeExpand);
            this.FindForm().Shown += new EventHandler(UCEmrInput_Shown);
        }

        void UCEmrInput_Shown(object sender, EventArgs e)
        {
            //自动显示病历批量导入界面
            AutoShowHistoryEmrBatchInForm();
            this.FindForm().Shown -= new EventHandler(UCEmrInput_Shown);
        }

        void UCEmrInput_Activated(object sender, EventArgs e)
        {
            //自动显示病历批量导入界面
            AutoShowHistoryEmrBatchInForm();
            this.FindForm().Activated -= new EventHandler(UCEmrInput_Activated);
        }

        /// <summary>
        /// 设置字体下拉框
        /// </summary>
        private void InitFont()
        {
            repositoryItemComboBoxFont.Items.AddRange(FontCommon.FontList.ToArray());

            //添加字号
            foreach (string fontsizename in FontCommon.allFontSizeName)
            {
                repositoryItemComboBox_SiZe.Items.Add(fontsizename);
            }
        }

        /// <summary>
        /// 设置默认字体下拉框 add by wwj 2012-05-29
        /// </summary>
        private void InitDefaultFont()
        {
            repositoryItemComboBox2.Items.Clear();
            repositoryItemComboBox2.Items.AddRange(new string[] { "五号", "小五", "六号" });
            barEditItemDefaultFontSize.EditValue = ZYEditorControl.DefaultFontSize;
        }

        /// <summary>
        /// 设置NavBarControl皮肤
        /// </summary>
        private void ExtraInit()
        {
            this.navBarControlToolBox.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Office 2010 Silver");
            this.navBarControlClinical.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Office 2010 Silver");
        }

        /// <summary>
        /// 初始化用到的变量
        /// </summary>
        private void InitDictionary()
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
        #endregion

        #region Load
        private void Form1_Load(object sender, EventArgs e)
        {
            BindPatInfos();
            PacsClose();
            PacsStart();

            //暂时将医嘱隐藏
            navBarGroup_DoctorAdive.Visible = false;
            //暂时将编辑和只读按钮隐藏
            btn_Edit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnEmrReadOnly.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            if (m_app.User.Id == "00")
            {
                barButtonItemChangeMacro.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
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

            RegisterEvent();
        }
        /// <summary>
        /// 得到配置信息  ywk 2012年6月27日 14:37:37
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfigValueByKey(string key)
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
        /// <summary>
        /// 根据登录人的身份判断控件的显示
        /// </summary>
        private void SetBarVisible()
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

        private void CollapseContainEmrModelNode()
        {
            treeList1.FocusedNodeChanged -= new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
            CollapseContainEmrModelNodeInner(treeList1.Nodes);
            treeList1.FocusedNodeChanged += new FocusedNodeChangedEventHandler(treeList1_FocusedNodeChanged);
        }

        private void CollapseContainEmrModelNodeInner(TreeListNodes nodes)
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

        /// <summary>
        /// 选中指定的病历
        /// </summary>
        private void SelectSpecifiedNode()
        {
            if (m_app.CurrentSelectedEmrID == "") return;
            SelectSpecifiedNodeInner(treeList1.Nodes);
            m_app.CurrentSelectedEmrID = "";
        }

        private void SelectSpecifiedNodeInner(TreeListNodes nodes)
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

        /// <summary>
        /// 初始化病人信息，初始化树节点，初始化工具栏
        /// </summary>
        private bool GetPatRec()
        {
            if (m_CurrentInpatient == null)
            {
                //m_app.CustomMessageBox.MessageShow("请先选择病人！");
                return false;
            }
            SetWaitDialogCaption("读取患者病历数据");
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

        /// <summary>
        /// 选中默认的右侧工具栏
        /// </summary>
        private void SelectDefaultRightTool()
        {
            this.panelContainer1.ActiveChild = this.dockPanel2;
            navBarGroup_Symb.Expanded = true;
        }

        /// <summary>
        /// 绑定病人基本信息
        /// </summary>
        private void BindPatBasic()
        {
            SetPatientInfo(Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
        }

        /// <summary>
        /// 初始化左侧的树
        /// </summary>
        private void InitPatTree()
        {
            treeList1.ClearNodes();
            //读取底层文件夹
            if (m_patUtil == null)
            {
                m_patUtil = new PatRecUtil(m_app, m_CurrentInpatient);
            }

            DataTable datatable = m_patUtil.GetFolderInfoHS("12");

            foreach (DataRow row in datatable.Rows)
            {
                InitTree(row, null);
            }
        }

        private void InitTreeByNurseRecord(DataRow row, TreeListNode node)
        {
            TreeListNode nursNode = treeList1.AppendNode(new object[] { row["CNAME"] }, node);
            nursNode.Tag = row;
        }


        private void InitTree(DataRow row, TreeListNode node)
        {


            TreeListNode patNode = treeList1.AppendNode(new object[] { row["CNAME"] }, node);
            if (row["ccode"].ToString() == "12")
                patNode.Expanded = true;

            EmrModelContainer container = new EmrModelContainer(row);
            patNode.Tag = container;
            //add image todo

            //寻找子节点


            //如果当前节点是最底层文件夹，则新增其病历文件内容

            if (row["CCODE"].ToString() == "AI") //护理记录文件夹
            {
                DataTable dtNurseRecord = GetNursDocMenusByParentID("1"); //获取功能性护理记录 如手术护理记录单
                foreach (DataRow item in dtNurseRecord.Rows)
                {
                    InitTreeByNurseRecord(item, patNode);
                }
            }

            DataTable datatable = m_patUtil.GetFolderInfo(row["CCODE"].ToString());
            if (datatable.Rows.Count > 0)
            {

                foreach (DataRow nextRow in datatable.Rows)
                {
                    InitTree(nextRow, patNode);
                }
                //patNode.ExpandAll();
            }
            else
            {

                if (container.EmrContainerType != ContainerType.None)
                {
                    //todo
                    DataTable leafs = m_patUtil.GetPatRecInfo(container.ContainerCatalog, m_CurrentInpatient.NoOfFirstPage.ToString());
                    {
                        foreach (DataRow leaf in leafs.Rows)
                        {
                            EmrModel leafmodel = new EmrModel(leaf);
                            TreeListNode leafNode = treeList1.AppendNode(new object[] { leafmodel.ModelName }, patNode);
                            leafNode.Tag = leafmodel;

                        }
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


        /// <summary>
        /// 获取护理文档菜单数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetNursDocMenusByParentID(string parentID)
        {
            string sqlStr = " select ID,CNAME,MNAME,PARENTID,SORTINDEX from dict_nursingdocuments_catalog where 1=1 ";
            if (!string.IsNullOrEmpty(parentID))
            {
                sqlStr += String.Format(" and PARENTID = {0} ", parentID);
            }
            else
            {
                sqlStr += " and PARENTID is null ";
            }
            sqlStr += " order by SORTINDEX ";
            return m_app.SqlHelper.ExecuteDataTable(sqlStr);
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
            SetWaitDialogCaption("正在加载病案首页");
            LoadIEMMainPage();
            HideWaitDialog();
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
            FocusedNodeChanged(e.Node);
        }

        public void FocusedNodeChanged(TreeListNode node)
        {
            SetWaitDialogCaption("正在加载病历，请稍等");
            if ((node == null) || (node.Tag == null)) return;
            LoadModelFromTree(node);
            this.HideWaitDialog();
        }
        /// <summary>
        /// 此页面加载右侧科室小模板给首次的科室小模板的值进行保存 add byywk 2012年6月11日 15:56:29
        /// </summary>
        public string MyContainerCode { get; set; }

        private void LoadModelFromTree(TreeListNode node)
        {
            string containercode = string.Empty;

            #region 容器类
            if (node.Tag is EmrModelContainer)//如果是容器类
            {
                EmrModelContainer container = (EmrModelContainer)node.Tag;
                if (node.HasChildren || container.Name == "会诊记录")
                {
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
                    //为了提高初始化界面的速度，将病历内容的捞取延迟到打开病例的时候
                    m_CurrentModel = m_RecordDal.LoadModelInstance(m_CurrentModel.InstanceId);
                    node.Tag = m_CurrentModel;
                }

                if (m_TempModelPages.ContainsKey(m_CurrentModel)
                    && ((EmrModel)node.Tag).DailyEmrModel == false)//非病程
                {
                    xtraTabControl1.SelectedTabPage = m_TempModelPages[m_CurrentModel];
                }
                else if (((EmrModel)node.Tag).DailyEmrModel == true)//病程，由于病程的所有数据都保存在首页中，所以要打开首程
                {
                    //（1）找到点击的病程对应的首程，并打开首次病程
                    //EmrModel emrModelFirstDaily = node.ParentNode.FirstNode.Tag as EmrModel;
                    EmrModel emrModelFirstDaily = GetFirstDailyEmrModel(node.ParentNode.Nodes);
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
            }
            #endregion

            #region 记录单功能性类
            if (node.Tag is DataRow)
            {
                DataRow dr = (DataRow)node.Tag;
                if (null != dr && null != dr["PARENTID"] && dr["PARENTID"].ToString() != "")
                {
                    InitAllListView(node);
                }
            }
            #endregion

            MyContainerCode = containercode;
            //根据科室类别刷新科室小模板
            InitPersonTree(false, containercode);

            //根据编辑器开关控制编辑器的编辑情况
            SetEditorNonEnable();

            //填充宏 当可以编辑病历时进行宏替换 Add By wwj 2012-05-14
            if (barButtonSave.Enabled == true) FillModelMacro(CurrentForm);

            //将焦点置到编辑器中
            if (CurrentForm != null)
            {
                CurrentForm.GotFocusNotTriggerEvent();
            }
            ChangeHalf();
        }


        /// <summary>
        /// 初始化 护理记录
        /// </summary>
        private void InitAllListView(TreeListNode node)
        {
            DataRow dr = (DataRow)node.Tag;
            string modelID = null == dr ? "0" : (null == dr["ID"] ? "0" : dr["ID"].ToString());
            bool hasTab = false;
            foreach (XtraTabPage item in xtraTabControl1.TabPages)
            {
                if (item != null && item.Tag != null && item.Tag is DataRow)
                {
                    DataRow drSelect = item.Tag as DataRow;
                    if (drSelect.Equals(dr))
                    {
                        xtraTabControl1.SelectedTabPage = item;
                        hasTab = true;
                        break;
                    }
                }
            }

            if (!hasTab)
            {
                MethodSet.App = m_app;
                MethodSet.CurrentInPatient = m_CurrentInpatient;
                UCNusreRecordMain m_UCNusreRecordMain = new UCNusreRecordMain();
                m_UCNusreRecordMain.Dock = DockStyle.Fill;
                XtraTabPage newPad = new XtraTabPage();
                newPad.Controls.Add(m_UCNusreRecordMain);
                newPad.Text = dr["CNAME"].ToString();
                newPad.Tag = dr;
                xtraTabControl1.TabPages.Add(newPad);
                xtraTabControl1.SelectedTabPage = newPad;
                m_UCNusreRecordMain.InitData(dr["ID"].ToString());
                m_UCNusreRecordMain.SetOperNurseRecord();
            }
        }

        /// <summary>
        /// 获得首次病程
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        EmrModel GetFirstDailyEmrModel(TreeListNodes nodes)
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

        /// <summary>
        /// 创建非病历类型界面
        /// 如首页、三测单之类的内容
        /// </summary>
        void CreateCommonDocement(EmrModelContainer container)
        {
            //读取容器里的包含第三方控件，使用反射使其实例化
            XtraTabPage newPad = new XtraTabPage();
            try
            {
                if (string.IsNullOrEmpty(container.ModelEditor)) return;

                //IEMREditor editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString() });
                IEMREditor editor;

                if (container.ContainerCatalog == "13") //三测单
                {
                    editor = new MainNursingMeasure(m_app);
                    editor.ReadOnlyControl = !m_EditorEnableFlag;//传递是否可以编辑字段s
                }
                else
                {
                    if (container.Args == null || container.Args.Trim() == "")
                    {
                        editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString() });
                    }
                    else
                    {
                        editor = (IEMREditor)Activator.CreateInstance(Type.GetType(container.ModelEditor), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString(), container.Args });
                    }
                    // editor=(IEMREditor)Activator.CreateInstance(Type.GetType("DrectSoft.Core.IEMMainPage.UCMainForm,DrectSoft.Core.IEMMainPage_2012"), new object[] { m_CurrentInpatient.NoOfFirstPage.ToString(), "EMRCTL.UCMRFirstPageContainer" };
                    //editor=null;
                }



                if (container.ContainerCatalog == ContainerCatalog.BingAnShouYe)//"病案首页"只需要加入现有的TabPage里
                {
                    newPad = xtraTabPageIEMMainPage;
                    editor.ReadOnlyControl = true;
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
        /// 设置除病程外的病历可编辑情况
        /// </summary>
        /// <param name="model"></param>
        private void CheckIsAllowEdit(EmrModel model)
        {
            if (DoctorEmployee.Grade.Trim() != "")
            {
                DoctorGrade grad = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), DoctorEmployee.Grade);
                if (model.State != ExamineState.NotSubmit && grad == DoctorGrade.Resident)//住院医且已提交
                {
                    SetDocmentReadOnlyMode();
                }
            }
        }

        /// <summary>
        /// 对于选中的病程需要激活病程的编辑区域
        /// </summary>
        /// <param name="node"></param>
        private void ProcDailyEmrModel(TreeListNode node)
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

        /// <summary>
        /// 设置病程病历 选中节点NotTriggerEvent 更改TabPage名称
        /// </summary>
        private void SetDailyEmrDoc(TreeListNode node)
        {
            //选中节点
            FocusNodeNotTriggerEvent(node);

            //更改TabPage名称
            ChangePageName(node);
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
            SelectedPageChanged(e.Page);
        }

        public void SelectedPageChanged(XtraTabPage page)
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
            if (emrEditor == null) return;
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
            else if (xtraTabControl1.SelectedTabPage.Tag is DataRow)
            {
                xtraTabControl1.TabPages.Remove(xtraTabControl1.SelectedTabPage);
                //m_UCNusreRecordMain = null;
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

                if (m_app.CustomMessageBox.MessageShow(fileName + " 已经修改，是否保存", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
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
            if (node == null) return;
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
            if (emrModel == null) return;
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

        private void InsertTableForNurseDocument()
        {
            if (CurrentForm != null && CurrentForm.zyEditorControl1 != null)
            {
                CurrentForm.zyEditorControl1.EMRDoc.TableInsertForNurseTable();
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
            if (m_app.CustomMessageBox.MessageShow("确定要删除光标所在的表格吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                CurrentForm.zyEditorControl1.EMRDoc.TableDelete();
            }
        }

        private void DeletTableRow()
        {
            if (m_app.CustomMessageBox.MessageShow("确定要删除光标所在的行吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                CurrentForm.zyEditorControl1.EMRDoc.TableDeleteRow();
            }
        }

        private void DeletTableColumn()
        {
            if (m_app.CustomMessageBox.MessageShow("确定要删除光标所在的列吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                CurrentForm.zyEditorControl1.EMRDoc.TableDeleteCol();
            }
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

        #region 针对右侧工具栏的Event
        private void navBarControlToolBox_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
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

        private void BindSymbol(DataTable dataTableSymbol)
        {
            Font font = new System.Drawing.Font("宋体", 10, FontStyle.Regular);
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

        private void gridViewMain_DoubleClick(object sender, EventArgs e)
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

        private void navBarControlClinical_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            //Add by wwj 2012-06-25 按照老板的要求，增加NavBarGroup的提示
            ShowTipForNavBarGroup(e.Group);

            ActiveGroup(e.Group);
        }

        private void ActiveGroup(NavBarGroup activeGroup)
        {
            if (activeGroup == navBarGroup_Bl)//病历提取 todo: 2011-07-14
            {
                gridControlCollect.DataSource = m_patUtil.GetDocCollectContent();
            }

            else if (activeGroup == navBarGroup_Jy)//检验
            {
                //gridControlTool.MainView = gridView_YJ;
                //navBarGroup_Jy.ControlContainer.Controls.Add(gridControlTool);
                if (m_patUtil.LisReports != null)
                {
                    gridControlLIS.DataSource = m_patUtil.LisReports;
                }
            }
            else if (activeGroup == navBarGroup_Jc)//检查
            {
                //gridControlTool.MainView = gridView_YJ;
                //navBarGroup_Jc.ControlContainer.Controls.Add(gridControlTool);
                if (m_patUtil.PacsReports != null)
                {
                    gridControlPacs.DataSource = m_patUtil.PacsReports;
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

        private void gridViewRis_DoubleClick(object sender, EventArgs e)
        {
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
        #endregion

        #region 设置TreeList图片

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
            else if (e.Node.Tag is DataRow)
            {
                e.NodeImageIndex = 0;
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

        /// <summary>
        /// 判断当前用户是否是医生
        /// </summary>
        /// <returns></returns>
        private bool CheckIsDoctor()
        {
            if (DoctorEmployee.Kind == EmployeeKind.Doctor || DoctorEmployee.Kind == EmployeeKind.Specialist)//当前登录人是医生
            {
                return true;
            }
            return false;
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
            treeList1.Refresh();
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
            //if (DoctorEmployee.Kind == EmployeeKind.Nurse)
            //{
            //    flag = true;
            //    return;
            //}
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

        #region 点击 TreeList

        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
            //if ((treeList1.FocusedNode == null) || (treeList1.FocusedNode.Tag == null)) return;

            //LoadModelFromTree(treeList1.FocusedNode);
        }

        private void treeList1_MouseUp(object sender, MouseEventArgs e)
        {
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
                    if (!(node.Tag is DataRow))
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
            //保存，删除，提交，审核, 取消审核 不可见
            btnSavePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnDeletePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnSubmitPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnConfirmPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btnCancelAuditPopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //模板 可见
            btnModulePopup.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;

            SetPopupBarButton();

            //针对首页、三测单之类非病历类型的界面需要保存功能
            if (((EmrModelContainer)node.Tag).EmrContainerType == ContainerType.None
                && ((EmrModelContainer)node.Tag).ContainerCatalog == ContainerCatalog.BingAnShouYe)
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
            SelectedPageChanged(xtraTabControl1.SelectedTabPage);
        }

        private void barButtonItem_JumpPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetEmrReadOnly2();
            JumpPrint();
            SelectedPageChanged(xtraTabControl1.SelectedTabPage);
        }

        private void barButtonItem_Print_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetEmrReadOnly2();
            PrintDocment();
            SelectedPageChanged(xtraTabControl1.SelectedTabPage);
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
                SetTableCellProperty();
            }
        }

        /// <summary>
        /// 表格信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemTableProperty_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        #region  toolbar editor
        private void btnCopy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.CurrentForm.zyEditorControl1.EMRDoc._Copy();
        }

        private void btnPaste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CanEdit())
            {
                this.CurrentForm.zyEditorControl1.EMRDoc._Paste();
            }
        }

        private void barButtonItem_Cut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CanEdit())
            {
                this.CurrentForm.zyEditorControl1.EMRDoc._Cut();
            }
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
            if (CanEdit())
            {
                if (barEditItem_Font.EditValue == null) return;
                this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectioinFontName(this.barEditItem_Font.EditValue.ToString());
            }
        }

        private void barEditItem_FontSize_EditValueChanged(object sender, EventArgs e)
        {
            if (barEditItem_FontSize.EditValue == null) return;

            float size = FontCommon.GetFontSizeByName(barEditItem_FontSize.EditValue.ToString());

            if (CanEdit())
            {
                this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontSize(size);
            }
        }
        private void barEditItem_FontSize_EditValueChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barEditItem_FontSize.EditValue == null) return;

            float size = FontCommon.GetFontSizeByName(barEditItem_FontSize.EditValue.ToString());

            if (CanEdit())
            {
                this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontSize(size);
            }
        }

        private void barButtonItem_Bold_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CanEdit())
            {
                //确定按钮的状态
                //this.barButtonItem_Bold.Down = this.barButtonItem_Bold.Down ? true : false;
                ReSetSupSub();
                this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontBold(this.barButtonItem_Bold.Down);
            }
        }

        private void barButtonItem_Italy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CanEdit())
            {
                //this.barButtonItem_Italy.Down = this.barButtonItem_Italy.Down ? true : false;
                this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontItalic(this.barButtonItem_Italy.Down);
            }
        }

        private void barButtonItem_Undline_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CanEdit())
            {
                //this.barButtonItem_Undline.Down = this.barButtonItem_Undline.Down ? true : false;
                this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionUnderLine(this.barButtonItem_Undline.Down);
            }
        }

        private void barButtonItemBold_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CanEdit())
            {
                //this.barButtonItem_Undline.Down = this.barButtonItem_Undline.Down ? true : false;
                this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionFontBold(this.barButtonItemBold.Down);
            }
        }

        private void barButtonItem_Sup_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CanEdit())
            {
                //this.barButtonItem_Sup.Down = this.barButtonItem_Sup.Down ? true : false;
                ReSetSupSub();
                this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSup(barButtonItem_Sup.Down);
            }
        }

        private void barButtonItem_Sub_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CanEdit())
            {
                //this.barButtonItem_Sub.Down = this.barButtonItem_Sub.Down ? true : false;
                ReSetSupSub();
                this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectionSub(barButtonItem_Sub.Down);
            }
        }

        private void barEditItem_Font_EditValueChanged(object sender, EventArgs e)
        {
            if (CanEdit())
            {
                if (barEditItem_Font.EditValue == null) return;
                this.CurrentForm.zyEditorControl1.EMRDoc.SetSelectioinFontName(this.barEditItem_Font.EditValue.ToString());
            }
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
                //SetWaitDialogCaption("正在保存病历,请稍等");
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
            if (!bar2.Visible) return;//当Bar2不显示的时候不需要执行保存操作
            if (model == null) return;
            if (m_IsSaving) return;
            m_IsSaving = true;
            try
            {
                SetWaitDialogCaption("正在保存病历,请稍等");
                m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                Save(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CurrentForm.zyEditorControl1.EMRDoc.Modified = false;
                HideWaitDialog();
                m_app.CustomMessageBox.MessageShow("病历保存成功！");
                //保存后重新加载病历信息节点树信息
                InitBLMessage();
            }
            m_IsSaving = false;
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
                    //xll 解决住院志保存丢失
                    m_RecordDal.InsertModelInstanceNew(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }
                else//修改
                {
                    m_RecordDal.UpdateModelInstance(model, Convert.ToInt32(m_CurrentInpatient.NoOfFirstPage));
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            DeleteDocumentPublic();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButton_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteDocumentPublic();
        }

        public void DeleteDocumentPublic()
        {
            if (m_CurrentModel != null)
            {
                if (!CanDelete())
                {
                    return;
                }
                if (m_app.CustomMessageBox.MessageShow("是否删除当前病历?", CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    DeleteTemplate();
                }
            }
        }

        /// <summary>
        /// 判断病历是否可以删除
        /// </summary>
        private bool CanDelete()
        {
            TreeListNode node = this.treeList1.FocusedNode;
            if (node != null && node.Tag != null)
            {
                EmrModel model = node.Tag as EmrModel;
                if (model != null)
                {
                    if (model.ModelCatalog == "AC" && model.FirstDailyEmrModel && node.ParentNode.Nodes.Count > 1)
                    {
                        m_app.CustomMessageBox.MessageShow("首次病程不能删除?", CustomMessageBoxKind.ErrorOk);
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
                    //节点直接删除  1.关闭TabPage，2.删除树节点【作废RecordDetail中对应的一条记录】
                    DeleteTabPageAndNode(emrModel);
                    m_RecordDal.CancelEmrModel(emrModel.InstanceId);

                    if (m_CurrentModel != null && m_CurrentModel.ModelCatalog == ContainerCatalog.BingChengJiLu)
                    {
                        m_CurrentModel.ModelContent = CurrentForm.zyEditorControl1.BinaryToXml(CurrentForm.zyEditorControl1.SaveBinary());
                        CurrentForm.zyEditorControl1.EMRDoc.Modified = false;
                        SaveDocumentInner(m_CurrentModel);
                    }
                }

                m_app.CustomMessageBox.MessageShow("删除成功!", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                SetDailyEmrEditArea();
                treeList1.Refresh();
            }
        }

        #endregion

        #region 另存
        private void btn_SaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string contentXml = CurrentForm.zyEditorControl1.ToXMLString();
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

        /// <summary>
        /// 供PadForm类调用提交功能
        /// </summary>
        public void SubmitDocumentPublic()
        {
            if (m_CurrentModel != null)
            {
                SubmitDocment();
            }
        }

        private void SubmitDocment()
        {
            if (!IsOpenThreeLevelAudit())
            {
                m_app.CustomMessageBox.MessageShow("病历提交功能暂未开放！");
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

        private void SubmitDocmentInner(EmrModel model)
        {
            if (model == null)
                return;
            try
            {
                if (m_app.CustomMessageBox.MessageShow("确定要提交 " + model.ModelName + " 吗？",
                    CustomMessageBoxKind.QuestionYesNo) == DialogResult.No)
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
                m_app.CustomMessageBox.MessageShow("提交病历成功！病历不可再编辑");
                ResetEditModeAction(model);
                GetStateImage();
                CurrentForm.zyEditorControl1.EMRDoc.Modified = false;
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("提交病历时发生异常！" + ex.Message + "");
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
                    model.DefaultModelName = model.ModelName;
                    model.ModelName = model.ModelName + " " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + DoctorEmployee.Name;

                    //首次病程 startnew为true，否则为false
                    //bool startnew = (catalog.Equals("AB") || (!node.HasChildren));
                    bool startnew = catalog.Equals(ContainerCatalog.BingChengJiLu) && (!node.HasChildren);

                    bool isOK = AddNewPatRec(model, startnew);
                    if (isOK)
                    {
                        container.AddModel(model);
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
            }
        }

        private bool AddNewPatRec(EmrModel model, bool startNew)
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

        private void FindInsertIndex(TreeListNode node, EmrModel model, ref int index)
        {


            if ((node.PrevNode != null) && node.PrevNode.Tag is EmrModel && (((EmrModel)node.PrevNode.Tag)).DisplayTime > model.DisplayTime)
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

                if (startNew)//新增首程
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

        /// 对新病程的设置
        /// </summary>
        /// <param name="model"></param>
        /// <param name="node">新增的病程对应的树节点</param>
        /// <param name="patRecTime">病程时间</param>
        /// <returns>是否取消对病程的设置  true：病程设置成功  false：取消对病程设置，则取消这次病程的新增操作</returns>
        private bool ProcessDailyTemplateForm(EmrModel model, TreeListNode node)
        {
            string titleName = model.IsShowFileName == "1" ? (string.IsNullOrEmpty(model.FileName.Trim()) ? model.Description.Trim() : model.FileName.Trim()) : "";

            EmrModel firstDailyEmrModel = GetFirstDailyEmrModel(node.ParentNode.Nodes);
            DateTime dt = DateTime.Now;
            bool isFirstDailyEmr = true;//新增的病历是否是首次病程
            if (firstDailyEmrModel != null && firstDailyEmrModel.FirstDailyEmrModel && firstDailyEmrModel.DisplayTime != DateTime.MinValue)//有首次病程
            {
                dt = Convert.ToDateTime(firstDailyEmrModel.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss"));
                isFirstDailyEmr = false;
            }

            using (DailyTemplateForm daily = new DailyTemplateForm(dt, titleName, isFirstDailyEmr))
            {
                daily.SetAllDisplayDateTime(CurrentForm);
                if (daily.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    model.DisplayTime = daily.CommitDateTime;
                    model.ModelName = model.Description + " " + model.DisplayTime.ToString("yyyy-MM-dd HH:mm:ss") + " " + DoctorEmployee.Name;
                    model.Description = daily.CommitTitle;

                    //Add By wwj 2012-02-14 更加设置的时间动态改变树节点的名称
                    node.SetValue("colName", model.ModelName);

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 点击“病程”文件夹时TabPage显示默认的病程【首程】
        /// </summary>
        /// <param name="node">非首次病程的病程节点</param>
        private void SetDefaultDailyEmrModelNode(TreeListNode node)
        {
            TreeListNode parentNode = node.ParentNode;
            if (parentNode.Nodes.Count > 0)//有子节点
            {
                EmrModel firstModel = GetFirstDailyEmrModel(parentNode.Nodes);
                if (firstModel != null && firstModel.DailyEmrModel == true)//子节点是病程
                {
                    bool isNeedActiveEditAreaTemp = m_IsNeedActiveEditArea;
                    m_IsNeedActiveEditArea = false;
                    m_IsLocateDaily = false;

                    treeList1.FocusedNode = parentNode.Nodes[0];//触发FocusNode事件

                    m_IsNeedActiveEditArea = isNeedActiveEditAreaTemp;
                    m_IsLocateDaily = true;
                    FocusNodeNotTriggerEvent(parentNode);
                    CurrentForm.ResetEditModeState(m_CurrentModel);
                }
            }
        }

        /// <summary>
        /// 插入病历编辑器的内容
        /// </summary>
        /// <param name="pad"></param>
        /// <param name="model"></param>
        public void InsertEmrModelContent(PadForm pad, EmrModel model)
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
                return form.CommitModel;
            }
            //todo why?
            //this.CurrentForm.AddGotFocusEvent();
            return null;
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
                        if (model == null) continue;
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

        /// <summary>
        /// 供PadForm类调用审核功能
        /// </summary>
        public void AuditDocumentPublic()
        {
            if (m_CurrentModel != null)
            {
                AuditDocment();
            }
        }

        private void AuditDocment()
        {
            if (!IsOpenThreeLevelAudit())
            {
                m_app.CustomMessageBox.MessageShow("病历审核功能未开放！");
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

        private void AuditDocmentInner(EmrModel currentModel)
        {
            try
            {
                if (m_app.CustomMessageBox.MessageShow("确定要审核 " + currentModel.ModelName + " 吗？", CustomMessageBoxKind.ErrorYesNo) == DialogResult.No)
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
                    m_app.CustomMessageBox.MessageShow("审核病历成功！");
                }
                else
                {
                    SetDocmentReadOnlyMode();
                    m_app.CustomMessageBox.MessageShow("审核病历成功！病历不可再编辑");
                }

                CurrentForm.zyEditorControl1.ActiveEditArea = null;
                CurrentForm.zyEditorControl1.Refresh();
                ResetEditModeAction(currentModel);
                GetStateImage();
                CurrentForm.zyEditorControl1.EMRDoc.Modified = false;
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("审核病历时发生异常！" + ex.Message + "");

            }
            //插入记录

        }
        #endregion

        #region 取消审核
        private void barButtonItemCancelAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                CancelAudit();
            }
        }

        private void barButtonItem_CancelAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_CurrentModel != null)
            {
                CancelAudit();
            }
        }

        public void CancelAuditPublic()
        {
            if (m_CurrentModel != null)
            {
                CancelAudit();
            }
        }

        public void CancelAudit()
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

        public void CancelAuditInner(EmrModel currentModel)
        {
            if (m_app.CustomMessageBox.MessageShow("确定要取消审核 " + currentModel.ModelName + " 吗？", CustomMessageBoxKind.ErrorYesNo) == DialogResult.No)
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
                m_app.CustomMessageBox.MessageShow("取消审核病历成功！");
                ResetEditModeAction(currentModel);
                GetStateImage();
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow("取消审核病历时发生异常！" + ex.Message + "");
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

        #region 病历提取
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
            barButtonItemCancelAudit.Enabled = enable;//取消审核 todo wwj 2011-09-06
            btn_SaveAs.Enabled = enable;//另存
            //btnHistoryEmrBatchIn.Enabled = enable;//历史病历批量导入 wwj 2012-04-26
            CanEditEmrDoc(enable);
        }

        /// <summary>
        /// 处于审核状态时BarButton可以使用的情况
        /// </summary>
        /// <param name="b"></param>
        private void AuditStateBarButton(bool b)
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
                //提交
                btnSubmitPopup.Enabled = false;
                barButton_Submit.Enabled = false;
                //审核
                barButton_Audit.Enabled = false;
                btnConfirmPopup.Enabled = false;
                //取消审核 todo wwj 2011-09-06
                barButtonItemCancelAudit.Enabled = false;
                btnCancelAuditPopup.Enabled = false;
                //病案首页默认显示“保存”
                if (container.ContainerCatalog == ContainerCatalog.BingAnShouYe)
                {
                    barButtonSave.Enabled = true;
                    btnSavePopup.Enabled = true;
                }
                else if (container.ContainerCatalog == ContainerCatalog.huizhenjilu)
                {
                    barButtonSave.Enabled = true;
                    btnSavePopup.Enabled = true;
                }
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

        #endregion

        #region 对登录人员进行控管，护士只能修改“护士文档”，医生能修改除“护士文档”以外所以的文档
        /// <summary>
        /// 对登录人员进行控管，护士只能修改“护士文档”，医生能修改除“护士文档”以外所以的文档
        /// </summary>
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

        /// <summary>
        /// 判断树节点是否是护士文档的节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isNurseNode"></param>
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

        /// <summary>
        /// 判断树节点是否是除护士文档以外的节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isDoctorNode"></param>
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

        /// <summary>
        /// 根据编辑器开关控制编辑器的编辑情况
        /// </summary>
        private void SetEditorNonEnable()
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
        public void find()
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
        SearchFrm m_Searchfrm;
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

        public bool GetCancelAuditButtonEnable()
        {
            return barButtonItemCancelAudit.Enabled;
        }

        public bool GetLocateButtonEnable()
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

        public TreeListNodes GetACTreeListNodes()
        {
            if (m_CurrentModel.ModelCatalog == ContainerCatalog.BingChengJiLu && m_CurrentTreeListNode != null
                && m_CurrentTreeListNode.ParentNode != null && m_CurrentTreeListNode.ParentNode.Nodes.Count > 0)
            {
                return m_CurrentTreeListNode.ParentNode.Nodes;
            }
            return null;
        }
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

                    //model.ModelName = (newTitle == null ? title : newTitle) + " " + (newDateTime == null ? date + " " + time : newDateTime) + " " + creater;
                    model.ModelName = title + " " + (newDateTime == null ? date + " " + time : newDateTime) + " " + creater;
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
        #endregion

        #region 右侧工具栏拖拽实现

        #region 已作废
        private void treeList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                m_IsAllowDrop = true;
            else
                m_IsAllowDrop = false;
        }

        private void treeList_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && m_IsAllowDrop)
            {
                TreeList list = (TreeList)sender;
                DoDragDorpInfo(list);
            }
        }

        private void treeList_MouseUp(object sender, MouseEventArgs e)
        {
            m_IsAllowDrop = false;
        }
        #endregion

        #region 正在使用中

        private void imageListBoxControlSymbol_MouseDown(object sender, MouseEventArgs e)
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

        private void imageListBoxControlSymbol_MouseMove(object sender, MouseEventArgs e)
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

        private void imageListBoxControlSymbol_MouseUp(object sender, MouseEventArgs e)
        {
            m_IsAllowDrop = false;
        }
        #endregion

        private void DoDragDorpInfo(TreeList tree)
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
        #endregion

        #region 科室小模板相关实现
        //捞取科室和个人小模板的数据。下面再筛选出通用的（99）
        //const string sql_queryTree = "select * from emrtemplet_item_person_catalog where deptid='{0}' and (isperson = 0 or (isperson = 1 and createusers = '{1}'))";
        //当创建个人小模板的人转科后，这边就取不到了，edit by ywk  2012年7月2日 10:49:43
        const string sql_queryTree = "select * from emrtemplet_item_person_catalog where (deptid='{0}' and isperson='0') or (isperson='1' and createusers='{1}')   and deptid='{0}' or createusers='{1}'";
        //add by yxy 添加个人小模板
        //const string sql_queryLeaf = "select * from emrtemplet_item_person where deptid='{0}' and (isperson = 0 or (isperson = 1 and createusers = '{1}'))";
        const string sql_queryLeaf = "select * from emrtemplet_item_person a where (a.deptid='{0}' and isperson=0) or (a.isperson = 1 and a.createusers = '{1}')   and deptid='{0}' or createusers='{1}'";


        private void barButtonItemPerson_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_PersonItem == null)
                m_PersonItem = new PersonItemManager(m_app);
            m_PersonItem.ShowDialog();
            //InitPersonTree(true, string.Empty);
            //此处修改，科室小模板窗体关闭后，原来的右侧清空了，现在更改为加载进来时的科室模板列表，声明了属性记录窗体加载时的模板值。add by  ywk 2012年6月11日 15:58:45
            InitPersonTree(true, MyContainerCode);

        }
        /// <summary>
        /// 加载小模板列表
        /// </summary>
        /// <param name="refresh"></param>
        /// <param name="catalog"></param>
        private void InitPersonTree(bool refresh, string catalog)
        {
            //if (m_CurrentEmrEditor != null) return;
            if ((m_MyTreeFolders == null) || (refresh))
            {
                m_MyTreeFolders = m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryTree, m_app.User.CurrentDeptId, m_app.User.Id));
                m_MyLeafs = m_app.SqlHelper.ExecuteDataTable(string.Format(sql_queryLeaf, m_app.User.CurrentDeptId, m_app.User.Id));
            }
            // add yxy 根据妇科提出需求，现在需要科室小模板中添加通用类型小模板，及每个文件类型都能使用的小模板
            DataRow[] rows = m_MyTreeFolders.Select("(Previd='' or Previd is null)  and (container = '99' or container ='" + catalog + "')");
            //DataRow[] rows = m_MyTreeFolders.Select("(Previd='' or Previd is null)  and container ='" + catalog + "'");
            treeListPerson.BeginUnboundLoad();
            treeListPerson.Nodes.Clear();
            foreach (DataRow dr in rows)
            {
                LoadTree(dr["ID"].ToString(), dr, null, catalog);
            }
            treeListPerson.CollapseAll();
            treeListPerson.EndUnboundLoad();

            //treeListPerson.CollapseAll();
            //treeListPerson.ExpandAll();

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
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                m_IsAllowDrop = true;
            else
                m_IsAllowDrop = false;
        }

        private void treeListPerson_MouseMove(object sender, MouseEventArgs e)
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
        }

        private void treeListPerson_MouseUp(object sender, MouseEventArgs e)
        {
            m_IsAllowDrop = false;
        }

        private void treeListPerson_AfterExpand(object sender, NodeEventArgs e)
        {
            //if (e.Node.HasChildren)
            //    e.Node.ExpandAll();
        }

        #endregion

        #region 打印 add by wwj 2011-08-24
        private void barButtonItemEmrPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (m_CurrentModel == null) return;
            //if (CurrentForm == null) return;

            ////add by yxy 在打印之前先提示保存  病历不保存病历内容会出现丢失现象
            //if (CurrentForm.zyEditorControl1.EMRDoc.Modified && barButtonSave.Enabled == true)
            //{
            //    m_app.CustomMessageBox.MessageShow(m_CurrentModel.ModelName + " 已经修改，请保存后再打印！");
            //    return;
            //}

            if (m_CurrentModel != null)
            {

                //add by yxy 在打印之前先提示保存  病历不保存病历内容会出现丢失现象
                if (CurrentForm.zyEditorControl1.EMRDoc.Modified && barButtonSave.Enabled == true)
                {
                    if (m_app.CustomMessageBox.MessageShow(" 已经修改，需要保存后再打印，是否保存", DrectSoft.Core.CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        SaveDocment(m_CurrentModel);
                    }
                    else
                        return;
                    //m_app.CustomMessageBox.MessageShow(m_CurrentModel.ModelName + " 已经修改，请保存后再打印！");
                    //return;
                }


                PrintForm printForm = new PrintForm(m_patUtil, m_CurrentModel, CurrentForm.zyEditorControl1.CurrentPage.PageIndex, m_CurrentInpatient.NoOfFirstPage.ToString());
                printForm.ShowDialog();

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
        #endregion

        #region 切换病人，可以供外部调用

        public void PatientChanging()
        {
            CloseAllTabPages();
        }

        public void PatientChanged(Inpatient inpatient)
        {
            PacsClose();
            PacsStart();

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
            this.FindForm().Activated += new EventHandler(UCEmrInput_Activated);
        }

        /// <summary>
        /// 重新捞取右侧工具栏中的数据
        /// </summary>
        private void ResetGroupControl()
        {
            if (navBarControlClinical.ActiveGroup != null)
            {
                treeListHistory.Nodes.Clear();
                ActiveGroup(navBarControlClinical.ActiveGroup);
            }
        }

        public void PatientChangedByIEmrHost(decimal noofinpat)
        {
            CloseAllTabPages();
            PatientChanged(new Inpatient(noofinpat));
        }


        public void CloseAllTabPages()
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

        #region 隐藏工具栏，供外部调用, 如：质控部分对病历的调用
        public void HideBar()
        {
            //bar1.Visible = false;
            bar2.Visible = false;
            bar3.Visible = false;
            panelContainer1.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            m_RightMouseButtonFlag = false;
            m_EditorEnableFlag = false;
            m_IsNeedActiveEditArea = false;
            xtraTabPageIEMMainPage.AccessibleName = "不能编辑";
        }
        #endregion

        #region 三级检诊人员设置 add by wwj 2011-09-06
        private void barButtonItemThreeLevelCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UserSettingForm userSettingForm = new UserSettingForm(this.m_app);
            userSettingForm.SetDepartmentNonEnableFlag();
            userSettingForm.StartPosition = FormStartPosition.CenterScreen;
            userSettingForm.ShowDialog();
        }
        #endregion

        #region 绑定大病历中的信息到工具栏中
        /// <summary>
        /// 加载住院志中可替换的节点信息 add by yxy 2011-10-11
        /// </summary>
        private void InitBLMessage()
        {
            //获得病历中所有可替换元素列表
            //ArrayList al = new ArrayList();
            //ZYTextDocument doc = CurrentForm.zyEditorControl1.EMRDoc;
            //doc.GetAllSpecElement(al, doc.RootDocumentElement, ElementType.Replace, null);

            //循环每个可替换元素，根据可替换元素的Name属性，查询并赋值线Text属性
            //if (al.Count < 0) return;
            //PadForm padForm = GetMainRecord();
            treeListBL.Nodes.Clear();
            PadForm padForm = GetSourcePadFormForReplaceItem("入院记录");
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
        #endregion

        /// <summary>
        /// 判断光标当前所在位置是否可以进行编辑操作
        /// </summary>
        /// <returns></returns>
        private bool CanEdit()
        {
            if (CurrentForm == null) return false;

            return CurrentForm.zyEditorControl1.CanEdit();
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
            //m_NodeIEMMainPage = null;

            //GetIEMMainPageNode(null);
            // if (m_NodeIEMMainPage != null)
            //{
            // EmrModelContainer container = (EmrModelContainer)m_NodeIEMMainPage.Tag;
            if (m_patUtil == null)
            {
                m_patUtil = new PatRecUtil(m_app, m_CurrentInpatient);
            }

            DataTable datatable = m_patUtil.GetFolderInfoHS("AA");
            if (datatable.Rows.Count <= 0) return;
            EmrModelContainer container = new EmrModelContainer(datatable.Rows[0]);
            if (container != null)
            {
                CreateCommonDocement(container);
            }

            // }
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
        #endregion

        #region ***************未使用的方法*************************
        /// <summary>
        /// 将现有文件插入到当前编辑器中，未使用
        /// </summary>
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

        private void BindOtherInfo()
        {
            gridControlTool.DataSource = m_patUtil.Symbols;
        }
        #endregion

        #region 诊疗计划
        private void barButtonItemDiagnosis_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
        #endregion

        /// <summary>
        /// 得到当前编辑器中显示病历对应的ID
        /// </summary>
        /// <returns></returns>
        public EmrModel GetCurrentModel()
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

        public EmrModelContainer GetCurrentModelContainer()
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

        #region 【暂时不用，在点击删除的按钮后判断】
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

        #region 右侧工具栏病人历史病历

        /// <summary>
        /// 点击入院次数，右侧工具栏显示历次住院记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem_InCount_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.panelContainer1.ActiveChild = this.dockPanel3;
            navBarGroup_history.Expanded = true;
        }

        #endregion

        #region 历史病例批量导入
        private void btnHistoryEmrBatchIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowHistoryEmrBatchInForm();
        }

        private void ShowHistoryEmrBatchInForm()
        {
            if (!bar2.Visible) return;

            if (!CheckIsDoctor())
            {
                m_app.CustomMessageBox.MessageShow("只有医生开放此权限!", CustomMessageBoxKind.InformationOk);
                return;
            }

            int inCount = Convert.ToInt32(m_RecordDal.GetHistoryInCount((int)m_CurrentInpatient.NoOfFirstPage));
            if (inCount <= 1)
            {
                m_app.CustomMessageBox.MessageShow("此病人无历史病历!", CustomMessageBoxKind.InformationOk);
            }
            else
            {
                inCount = m_RecordDal.GetEmrCountByNoofinpat(m_CurrentInpatient.NoOfFirstPage.ToString());
                if (inCount > 0)
                {
                    m_app.CustomMessageBox.MessageShow("此病人已经存在病历，无法导入!", CustomMessageBoxKind.InformationOk);
                }
                else
                {
                    ShowHistoryEmrBatchInFormInner();
                }
            }
        }

        /// <summary>
        /// 显示病历批量导入界面
        /// </summary>
        private void ShowHistoryEmrBatchInFormInner()
        {
            HistoryEmrBatchInForm barchInForm = new HistoryEmrBatchInForm(m_app, m_RecordDal, m_CurrentInpatient, m_patUtil);
            barchInForm.StartPosition = FormStartPosition.CenterScreen;
            barchInForm.ShowDialog();
        }

        /// <summary>
        /// 自动显示病历批量导入界面
        /// </summary>
        private void AutoShowHistoryEmrBatchInForm()
        {
            if (!bar2.Visible) return;

            if (IsAutoShowHistoryEmrBatchInForm())
            {
                ShowHistoryEmrBatchInFormInner();
            }
        }

        /// <summary>
        /// 判断是否需要自动显示病历批量导入界面
        /// </summary>
        /// <returns></returns>
        private bool IsAutoShowHistoryEmrBatchInForm()
        {
            if (CheckIsDoctor())
            {
                int inCount = Convert.ToInt32(m_RecordDal.GetHistoryInCount((int)m_CurrentInpatient.NoOfFirstPage));
                if (inCount > 1)
                {
                    inCount = m_RecordDal.GetEmrCountByNoofinpat(m_CurrentInpatient.NoOfFirstPage.ToString());
                    if (inCount == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 复用元素替换 Add By wwj 2012-05-14
        private void barButtonItemReplaceItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!CheckIsDoctor())
            {
                m_app.CustomMessageBox.MessageShow("只有医生开放此权限!", CustomMessageBoxKind.InformationOk);
                return;
            }
            ReplaceItemForAllEmrRecord();
        }

        private void ReplaceItemForAllEmrRecord()
        {
            if (m_app.CustomMessageBox.MessageShow("是否重新替换所有复用元素？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
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

        /// <summary>
        /// 重新加载整个文书录入界面
        /// </summary>
        private void RefreshEMRMainPad()
        {
            m_app.ChoosePatient(m_CurrentInpatient.NoOfFirstPage);
            m_app.LoadPlugIn("DrectSoft.Core.EMR_NursingDocument.EMRInput.Table.dll", "DrectSoft.Core.EMR_NursingDocument.EMRInput.Table.MainForm");
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
        #endregion

        #region 切换全局字体默认大小
        private void barEditItemDefaultFontSize_EditValueChanged(object sender, EventArgs e)
        {
            ZYEditorControl.DefaultFontSize = barEditItemDefaultFontSize.EditValue.ToString();
        }
        #endregion

        #region 复制第一页的护理表格，并插入到光标的当前位置【由于编辑器显示和打印的Bug暂不开放使用】
        private void barButtonItemAddNurseTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (m_app.CustomMessageBox.MessageShow("确定要插入第一个表格吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                this.InsertTableForNurseDocument();
            }
        }
        #endregion

        #region 圈字
        private void barButtonItemQuanZi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CanEdit())
            {
                this.CurrentForm.zyEditorControl1.EMRDoc.SetCircleFont(this.barButtonItemQuanZi.Down);
                this.CurrentForm.zyEditorControl1.EMRDoc.Refresh();
                this.CurrentForm.zyEditorControl1.Refresh();
            }
        }
        #endregion

        #region 更改当前病历中宏元素的数据【现在只开放修改“科室”“病区”的宏元素】
        private void barButtonItemChangeMacro_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CurrentForm != null && m_CurrentModel != null)
            {
                ChangeMacroFrom changeMacro = new ChangeMacroFrom(m_app, CurrentForm, m_CurrentInpatient);
                changeMacro.StartPosition = FormStartPosition.CenterScreen;
                if (changeMacro.ShowDialog() == DialogResult.Yes)
                {
                    if (m_app.CustomMessageBox.MessageShow("确定要保存到数据库中吗？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        SaveDocment(m_CurrentModel);
                    }
                }
            }
        }
        #endregion

        #region 点击DockPanel之后更改对应的名称
        private void panelContainer1_ActiveChildChanged(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
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
                navBarGroup_Bl.Expanded = true;
                ShowTipForNavBarGroup(navBarGroup_Bl);
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

            GOMyCheck();
        }

        public void GOMyCheck()
        {
            string showcontent = string.Empty;
            showcontent = GetReturnContent();
            //string showcontent = string.Empty;
            //CheckInpatient(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), out showcontent);
            if (CheckRight)
            {
                UpdateFlag(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), "1");
                //CheckTipContent checkT = new CheckTipContent(m_app, "验证通过！", this);
                m_app.CustomMessageBox.MessageShow("验证通过！");
            }
            else
            {
                UpdateFlag(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), "0");

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
                //}
                //else
                //{
                //    m_CheckT.RefreshContent(showcontent);
                //    m_CheckT.Show();
                //}
            }
        }

        public string GetReturnContent()
        {
            string showcontent = string.Empty;
            CheckInpatient(m_app.CurrentPatientInfo.NoOfFirstPage.ToString(), out showcontent);
            return showcontent;
        }

        CheckTipContent m_CheckT;
        /// <summary>
        /// 验证通过将此病人的flag更为0.供HIS使用 
        /// add by ywk 2012年7月27日 14:09:20
        /// </summary>
        /// <param name="p"></param>
        private void UpdateFlag(string noofinpat, string flag)
        {
            string upsql = string.Format(@"update iem_mainpage_basicinfo_2012 set PatFlag='{0}' where noofinpat='{1}' and valide='1' ", flag, noofinpat);
            try
            {
                m_app.SqlHelper.ExecuteNoneQuery(upsql, CommandType.Text);
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
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
                ReturnMessage += "入院日期为空！\n\r";
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
                ReturnMessage += "出院日期为空！\n\r";
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
                    ReturnMessage += "入院日期不能大于出院日期！\n\r";
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
                ReturnMessage += "住院天数为空！\n\r";
                CheckRight = false;
                TipMessage = ReturnMessage;
                //return;
            }
            if (I_TotalDay < 0)
            {
                ReturnMessage += "住院天数不能为负数！\n\r";
                CheckRight = false;
                TipMessage = ReturnMessage;
                //return;
            }
            if (I_TotalDay == 0)
            {
                ReturnMessage += "住院天数不能为0！\n\r";
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
                    ReturnMessage += "出生日期不应该大于入院日期！\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                    //return;
                }
            }
            //0247:  messagebox("提示","请在出院卡片首页中输入主要诊断")
            IemInfo = m_IemMainPage.GetIemInfo();
            if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count < 0)
            {
                ReturnMessage += "请在出院卡片首页中输入主要诊断！\n\r";
                CheckRight = false;
                TipMessage = ReturnMessage;
                //return;
            }
            //MessageBox('提示','年龄只能在0和100之间！')
            int m_Age = Int32.Parse(InpatientDt.Rows[0]["AGE"].ToString());
            if (m_Age < 0 || m_Age > 100)
            {
                ReturnMessage += "年龄只能在0和100之间！\n\r";
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
            if (m_MenAndInHop < 0 || m_MenAndInHop > 3 || m_InHopAndOutHop < 0 || m_InHopAndOutHop > 3 || m_BeforeOpeAndAfterOper < 0 || m_BeforeOpeAndAfterOper > 3 || m_LinAndBingLi < 0 || m_LinAndBingLi > 3 || m_InHopThree < 0 || m_InHopThree > 3 || m_FangAndBingLi < 0 || m_FangAndBingLi > 3)
            {
                ReturnMessage += "诊断符合情况只能输入0到3之间的数字！\n\r";
                CheckRight = false;
                TipMessage = ReturnMessage;
                //return;
            }
            if (m_MenAndInHop == 10 || m_InHopAndOutHop == 10 || m_BeforeOpeAndAfterOper == 10 || m_LinAndBingLi == 10 || m_InHopThree == 10 || m_FangAndBingLi == 10)
            {
                ReturnMessage += "诊断符合情况有空值！\n\r";
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
            if (DiagInfo.Contains("2"))//存在误诊的情况
            {
                if (m_app.CustomMessageBox.MessageShow(string.Format("诊断符合情况有输入[误诊]的选项,是否继续？"), CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                {
                    CheckRight = true;
                }
                else
                {
                    CheckRight = false;
                }
            }
            if (CheckRight)//上面判断有误诊选择了继续
            {
                if (ReturnMessage.Length > 10)
                {
                    CheckRight = false;
                }
                //MessageBox('提示','诊断符合情况中[门诊与住院]、[入院与出院]、[入院三日内]不能填[未做]')  
                if (DiagInfo[0].ToString() == "0" || DiagInfo[1].ToString() == "0" || DiagInfo[4].ToString() == "0")
                {
                    ReturnMessage += "诊断符合情况中[门诊与住院]、[入院与出院]、[入院三日内]不能填[未做]！\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                    //return;
                }
                // MessageBox('提示','术前术后诊断必须与手术信息相对应！如果选择确诊必须填手术信息，如果未作则不能有手术信息！') 
                if (DiagInfo[2].ToString() == "1" && IemInfo.IemOperInfo.Operation_Table.Rows.Count == 0)//是确诊，手术信息为空的话 
                {
                    ReturnMessage += "术前和术后手术信息为确诊，但无手术信息！\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                    //return;
                }
                if (DiagInfo[2].ToString() == "0" && IemInfo.IemOperInfo.Operation_Table.Rows.Count > 0)//是未作，手术信息有数据的话 
                {
                    ReturnMessage += "术前和术后手术信息为未作，但有手术信息！\n\r";
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
                            ReturnMessage += "有手术信息，但手术日期不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //0076:   Gf_msg('是否择期手术不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCHOOSEDATE"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但是否择期手术不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //    0081:   Gf_msg('是否无菌手术不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCLEAROPE"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但是否无菌手术不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //0086:   Gf_msg('麻醉方式不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Anaesthesia_Type_Id"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但麻醉方式不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //  0091:   Gf_msg('手术医师不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Execute_User1"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但手术医师不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //    0096:   Gf_msg('切口愈合方式不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Close_Level"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但切口愈合方式不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //0101:   Gf_msg('是否感染不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISGANRAN"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但是否感染不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                    }
                }
                else
                {
                    ReturnMessage += "无手术信息！\n\r";
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
                            ReturnMessage += "有手术信息，但手术日期不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //0076:   Gf_msg('是否择期手术不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCHOOSEDATE"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但是否择期手术不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //    0081:   Gf_msg('是否无菌手术不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISCLEAROPE"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但是否无菌手术不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //0086:   Gf_msg('麻醉方式不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Anaesthesia_Type_Id"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但麻醉方式不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //  0091:   Gf_msg('手术医师不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Execute_User1"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但手术医师不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //    0096:   Gf_msg('切口愈合方式不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["Close_Level"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但切口愈合方式不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                        //0101:   Gf_msg('是否感染不能为空！',211)
                        if (DtOper.Rows[i]["Operation_Code"].ToString() != "" && DtOper.Rows[i]["ISGANRAN"].ToString() == "")
                        {
                            ReturnMessage += "有手术信息，但是否感染不能为空！\n\r";
                            CheckRight = false;
                            TipMessage = ReturnMessage;
                            //return;
                        }
                    }
                }
                else
                {
                    ReturnMessage += "无手术信息！\n\r";
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
                            ReturnMessage += "请输入外部损伤原因！\n\r";
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
                        ReturnMessage += "出院日期大于当前日期10天以上，不能录入！\n\r";
                        CheckRight = false;
                        TipMessage = ReturnMessage;
                    }
                }
                //    0063:   messagebox("注意","该患者住院天数超过50天！")
                if (I_TotalDay > 50)
                {
                    ReturnMessage += "该患者住院天数超过50天！\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                //    0041:   messagebox('提示','请输入损伤外部原因!')
                if (string.IsNullOrEmpty(IemInfo.IemDiagInfo.Hurt_Toxicosis_ElementID))
                {
                    ReturnMessage += "请输入损伤外部原因！\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }

                //  0029:  messagebox("提示","请输入婴儿出生日期")
                if (string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.BithDay))
                {
                    ReturnMessage += "请输入婴儿出生日期！\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                //    0047:   MessageBox('提示','请在出院卡片Ⅱ中录入完整的妇婴信息！')
                if (string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.APJ) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.BithDayPrint) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.CC) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.CCQK) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.CFHYPLD) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.FMFS) || string.IsNullOrEmpty(IemInfo.IemObstetricsBaby.Heigh))
                {
                    ReturnMessage += "请在出院卡片Ⅱ中录入完整的妇婴信息！\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                //    0122:   messagebox("提示","请给出诊断的转归代码")
                if (string.IsNullOrEmpty(IemInfo.IemBasicInfo.ZG_FLAG))
                {
                    ReturnMessage += "请给出诊断的转归代码！\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                //    0011:  messagebox("提示","该患者没有诊断")
                if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count == 0)
                {
                    ReturnMessage += "该患者没有诊断！\n\r";
                    CheckRight = false;
                    TipMessage = ReturnMessage;
                }
                //    0039:   messagebox("提示","请选择诊断类别")
                if (IemInfo.IemDiagInfo.OutDiagTable.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(IemInfo.IemDiagInfo.OutDiagTable.Rows[0]["diagnosis_type_id"].ToString()))
                    {
                        ReturnMessage += "请选择诊断类别！\n\r";
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
        /// <summary>
        /// /获得有关生孩子的诊断
        /// 现在的有关生孩子的诊断都是存放在数据库zx_diagsaboutbaby表中
        /// </summary>
        /// <returns></returns>
        private DataTable GetBabyDiagInfo()
        {
            string searchip = string.Format("select * from zx_diagsaboutbaby where valid='1' ");
            return m_app.SqlHelper.ExecuteDataTable(searchip, CommandType.Text);

        }
        /// <summary>
        /// 获得所有病人
        /// </summary>
        /// <returns></returns>
        private DataTable GetInpatient()
        {
            string searchip = string.Format("select * from inpatient");
            return m_app.SqlHelper.ExecuteDataTable(searchip, CommandType.Text);
        }

        #endregion

        #region 调用PACS图像
        [DllImport("joint.dll")]
        public static extern int PacsView(int nPatientType, string lpszID, int nImageType); //Pacs调阅3.1版

        [DllImport("joint.dll")]
        public static extern bool PacsViewByPatientInfo(int nType, string str, int nPatientType);
        [DllImport("joint.dll")]
        public static extern int PacsInitialize();
        [DllImport("joint.dll")]
        public static extern void PacsRelease();

        /// <summary>
        /// 关闭Pacs调用
        /// </summary>
        public void PacsClose()
        {
            try
            {
                if (CheckPackIsExist())
                {
                    PacsRelease();//关闭Pacs调用
                }

            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 启动Pacs调用
        /// </summary>
        public void PacsStart()
        {
            try
            {
                if (CheckPackIsExist())
                {
                    PacsInitialize();
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 新增的调用PACS图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                        if (PacsView(nPatientType, m_CurrentInpatient.RecordNoOfHospital, LookType) == 1)
                        {
                            //MessageBox.Show("调用成功");
                            //PacsRelease();//
                        }
                        else
                        {
                            MessageBox.Show("调用失败");
                        }
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        PacsClose();
                        PacsStart();
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
        /// 判断调用PACS的DLL是否存在 
        /// </summary>
        private bool CheckPackIsExist()
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
        #endregion

        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_EditValueChanged(object sender, EventArgs e)
        {
            refreshGridView();
        }

        /// <summary>
        /// 刷新病人列表控件
        /// </summary>
        private void refreshGridView()
        {
            string filter = string.Format(" PATID like '%{0}%' or PatName like '%{0}%' or BedID like '%{0}%' ", txtSearch.Text.Trim());
            //DataView dv = gridMain.DataSource as DataView;
            DataTable dt = gridControl1.DataSource as DataTable;
            if (dt != null)
            {
                dt.DefaultView.RowFilter = filter;
            }
        }

        /// <summary>
        /// 内部的病人列表双击换人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DataRow dataRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string noofinpat = dataRow["NoOfInpat"].ToString();

            if (DataManager.HasBaby(noofinpat))
            {
                ChoosePatOrBaby choosepat = new ChoosePatOrBaby(m_app, noofinpat);
                choosepat.StartPosition = FormStartPosition.CenterParent;
                if (choosepat.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    m_app.ChoosePatient(decimal.Parse(choosepat.NOOfINPAT));

                }
            }
            else
            {

                m_app.ChoosePatient(Convert.ToDecimal(noofinpat));
            }

            m_RecordDal = new RecordDal(m_app.SqlHelper);
            CurrentInpatient = m_app.CurrentPatientInfo;
            PatientChanging();
            PatientChanged(m_app.CurrentPatientInfo);
        }

        StringFormat sf = new StringFormat();
        /// <summary>
        /// 设置单元格颜色 婴儿
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {


            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            if (e.CellValue == null) return;
            DataRowView drv = gridView1.GetRow(e.RowHandle) as DataRowView;
            //取得病人名字
            string patname = drv["PatName"].ToString().Trim();

            if (e.Column == ColHZXM)
            {
                if (patname.Contains("婴儿"))
                {
                    e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));
                    e.Graphics.DrawString(patname, e.Appearance.Font, Brushes.Red, e.Bounds, new StringFormat());
                    e.Handled = true;
                }
            }
        }
    }
}
