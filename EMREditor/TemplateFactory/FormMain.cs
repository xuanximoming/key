using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core;
using DrectSoft.Core.TemplatePerson;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Clipboard;
using DrectSoft.Library.EmrEditor.Src.Common;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Library.EmrEditor.Src.Gui;
using DrectSoft.Library.EmrEditor.Src.Print;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using XWriterDemo;

namespace DrectSoft.Emr.TemplateFactory
{
    /// <summary>
    /// edit by cyq 2013-03-06 add try...catch
    /// </summary>
    public partial class FormMain : DevBaseForm, IStartPlugIn
    {
        #region vars
        private bool IsPerson = false;
        private bool IsAdministrator = false;
        private ZYEditorControl pnlText;

        IEmrHost m_app;

        SQLUtil m_Util;
        SQLManger m_SqlManger;

        /// <summary>
        /// 当前模板实体
        /// </summary>
        Emrtemplet m_Emrtemplet;

        /// <summary>
        /// 当前子模板实体
        /// </summary>
        Emrtemplet_Item m_Emrtemplet_Item;

        /// <summary>
        /// 当前选中的模板数节点
        /// </summary>
        TreeListNode m_treelistnode;

        /// <summary>
        /// 当前选中的模板数节点
        /// </summary>
        TreeListNode m_treelistnode_item;

        /// <summary>
        /// 右键菜单添加新模板时候后记录下菜单中的信息
        /// </summary>
        Emrtemplet m_new_Emrtemplet;

        /// <summary>
        /// 右键菜单添加新模板时候后记录下菜单中的信息
        /// </summary>
        Emrtemplet_Item m_new_Emrtemplet_Item;

        /// <summary>
        /// 当前页眉文件
        /// </summary>
        EmrTempletHeader m_EmrTempletHeader;
        /// <summary>
        /// 新增页面文件
        /// </summary>
        EmrTempletHeader m_newEmrTempletHeader;

        /// <summary>
        /// 当前页脚文件
        /// </summary>
        EmrTemplet_Foot m_EmrTemplet_Foot;
        /// <summary>
        /// 新增页脚文件
        /// </summary>
        EmrTemplet_Foot m_newEmrTemplet_foot;


        /// <summary>
        /// 操作模板类型
        /// </summary>
        EmrTempletType m_EmrTempletType;

        /// <summary>
        /// 特殊字符集合
        /// </summary>
        public DataTable Symbols
        {
            get
            {
                if (_symbols == null)
                {
                    _symbols = m_app.SqlHelper.ExecuteDataTable("select  SYMBOL as 名称 from DICT_SYMBOL");
                    _symbols.TableName = "symbols";
                }
                return _symbols;
            }
        }
        private DataTable _symbols;

        /// <summary>
        /// 是否允许拖拽操作
        /// </summary>
        private bool m_IsAllowDrop = true;

        public static float s_LineSpace = 1.5f;
        public static Color s_ElementColor = Color.FromArgb(0x99, 0xCC, 0xFF);
        public static string s_ElementStyle = "背景色";

        private System.Windows.Forms.ContextMenu menuColor = new System.Windows.Forms.ContextMenu();

        private WaitDialogForm m_WaitDialog;

        #endregion

        #region ctor
        /// <summary>
        /// ctor
        /// </summary>
        public FormMain()
        {
            try
            {
                InitializeComponent();
                m_WaitDialog = new WaitDialogForm("正在创建用户界面...", "请稍侯");
            }
            catch (Exception ex)
            {
                HideWaitDialog();
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                //右侧的模板列表加载时显示不全，先将此处背景色更改(都设为fill了？？？)edit by ywk 2012年4月16日9:25:02
                treeList_Template.BackColor = Color.White;
                Init();
                label1.Visible = false;
                pictureBox1.Visible = false;
                if (m_app.User.Id != "00")//add by Ukey 2016-08-21
                {
                    barSubItem7.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barButtonItem14.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barButtonItemBaseData.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                HideWaitDialog();
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        //声明委托(异步加载数据)
        delegate void InitTemplet();

        #endregion

        #region 初始化基本信息
        private void SetWaitDialogCaption(string caption)
        {
            try
            {
                if (m_WaitDialog != null)
                {
                    if (!m_WaitDialog.Visible)
                        m_WaitDialog.Visible = true;
                    m_WaitDialog.Caption = caption;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void HideWaitDialog()
        {
            try
            {
                if (m_WaitDialog != null)
                {
                    m_WaitDialog.Hide();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string m_deptId = "";
        private string m_ccode = "";
        private DataTable m_dtDept;
        private DataTable m_dtTem;
        private DataTable m_dtTemNode;
        private DataTable templeteTable;

        //显示科室、节点下拉查询框
        private void ShowQuery()
        {
            try
            {
                this.lookUpWindowDeptTem.SqlHelper = m_app.SqlHelper;
                this.lookUpWindowTemNode.SqlHelper = m_app.SqlHelper;
                this.lookUpWindowTem.SqlHelper = m_app.SqlHelper;
                GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);

                this.m_dtDept = m_Util.EmrDepartment;
                m_dtDept.Columns["DEPT_ID"].Caption = "科室编码";
                m_dtDept.Columns["DEPT_NAME"].Caption = "科室名称";
                Dictionary<string, int> colWidth = new Dictionary<string, int>();
                colWidth.Add("DEPT_ID", 60);
                colWidth.Add("DEPT_NAME", 100);
                DataTable depts = m_dtDept.Copy();
                shortCode.AutoAddShortCode(depts, "DEPT_NAME");
                SqlWordbook deptWordBook = new SqlWordbook("Depts", depts, "DEPT_ID", "DEPT_NAME", colWidth, "DEPT_ID//DEPT_NAME//PY//WB");
                this.lookUpEditorDeptTem.SqlWordbook = deptWordBook;

                m_dtTemNode = m_Util.ModelContainers;
                m_dtTemNode.Columns["CCODE"].Caption = "代码";
                m_dtTemNode.Columns["CNAME"].Caption = "节点名称";
                Dictionary<string, int> colWidth2 = new Dictionary<string, int>();
                colWidth2.Add("CCODE", 40);
                colWidth2.Add("CNAME", 80);
                DataTable nodes = m_dtTemNode.Copy();
                shortCode.AutoAddShortCode(nodes, "CNAME");
                SqlWordbook temNodeWordBook = new SqlWordbook("DeptChilds", nodes, "CCODE", "CNAME", colWidth2, "CCODE//CNAME//PY//WB");
                this.lookUpEditorTemNode.SqlWordbook = temNodeWordBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Init()
        {
            try
            {
                //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;//解决第三方控件异步报错的问题
                //模板加载完毕前先禁用add by ywk 2013年4月2日16:41:56 
                treeList_Template.Enabled = false;


                //SetWaitDialogCaption("正在加载模板数据");
                m_Util = new SQLUtil(m_app);
                m_SqlManger = new SQLManger(m_app);
                InitBasic();
                InitFont();
                InitItemTree();
                treeList_Template.VertScrollVisibility = ScrollVisibility.Auto;
                InitEmrDefaultSetting();
                InitMacro();
                InitBarButtonVisible();
                SetWaitDialogCaption("正在绘画界面...");

                InitBasicTree();
                //InitSubItemTree();
                this.ShowQuery();

                //异步加载还原 add by ywk  2013年2月20日15:12:40
                (new InitTemplet(InitTemplateTree)).BeginInvoke(null, null);//异步处理模板列表
                SetWaitDialogCaption("正在加载模板内容...");
                //InitTemplateTree();
                InitMyTemplateTree();
                SetWaitDialogCaption("正在加载子模板内容...");
                InitItemTemplateTree();

                HideWaitDialog();//---------
                pnlText.EMRDoc.Modified = false;
                //注册快捷键
                pnlText.EMRDoc.OnSaveEMR += new SaveEMRHandler(EMRDoc_OnSaveEMRTemp);
                pnlText.EMRDoc.OnShowFindForm += new ShowFindFormHandler(EMRDoc_OnShowFindForm);
                SetWaitDialogCaption("正在加载特殊字符...");
                BindSymbol(Symbols);
                DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = false;//解决第三方控件异步报错的问题
                //加载右侧模板列表，给予提示信息 edit by ywk 2012年4月16日10:00:13

                //设置模板数据集
                templeteTable = m_Util.SimpleModelList.DefaultView.ToTable();
                HideWaitDialog();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 控制某些按钮是否显示
        /// </summary>
        private void InitBarButtonVisible()
        {
            try
            {
                bool isHiddenGlobalAreaEmrSet = true;
                string[] roles = BasicSettings.GetStringConfig("EditEmrGlobalAreaSetRole").Split(',');
                string[] userRoles = m_SqlManger.GetUserRoleID(m_app.User.Id).Split(',');
                foreach (string role in roles)
                {
                    if (userRoles.Contains(role))
                    {
                        isHiddenGlobalAreaEmrSet = false;
                        break;
                    }
                }
                if (isHiddenGlobalAreaEmrSet)
                {
                    btn_Default.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btn_HeaderFooterSetting.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MakeTree(DataRow row, TreeListNode parentNode)
        {
            try
            {
                //读取科
                DataRow[] rows = null;
                string mr_name = "";
                //03节点
                TreeListNode node = null;//
                node = treeList_Template.AppendNode(new object[] { row["DEPT_NAME"].ToString(), row["DEPT_ID"].ToString() }, parentNode);
                node.Tag = new EmrNodeObj(row, EmrNodeType.Container);
                if (!row["DEPT_ID"].ToString().Equals("*"))
                {   //03子节点
                    rows = m_Util.EmrDepartment.Select("DEPT_ID <>'" + row["DEPT_ID"].ToString() + "' and DEPT_ID like '" + row["DEPT_ID"].ToString() + "%'");
                    foreach (DataRow myRow in rows)
                    {
                        //todo 
                        MakeTree(myRow, node);
                    }
                    if (rows.Length > 0) return;
                }
                //读取创建子集文件夹 注释掉试试
                foreach (DataRow catalogRow in m_Util.ModelContainers.Rows)
                {
                    TreeListNode catalogNode = treeList_Template.AppendNode(new object[] { catalogRow["CNAME"].ToString(), catalogRow["CCODE"].ToString() + row["DEPT_ID"] }, node);
                    catalogNode.Tag = new EmrNodeObj(catalogRow, EmrNodeType.Container);

                    //读取模板
                    m_Util.ModelList.DefaultView.RowFilter = "DEPT_ID='" + row["DEPT_ID"] + "' and MR_CLASS='" + catalogRow["CCODE"] + "'" + " and Valid=1 ";

                    DataTable table = m_Util.ModelList.DefaultView.ToTable();
                    //设置叶子节点
                    foreach (DataRow leafRow in table.Rows)
                    {
                        //if (leafRow["todept"].Equals("0"))
                        //    mr_name = "☋" + leafRow["mr_name"].ToString();
                        //else
                        mr_name = leafRow["mr_name"].ToString();
                        TreeListNode leafNode = treeList_Template.AppendNode(new object[] { mr_name, leafRow["Templet_ID"].ToString() }, catalogNode);
                        leafNode.Tag = new EmrNodeObj(leafRow, EmrNodeType.Leaf);
                    }

                }
                //BulidChildNode(row, node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void MakeMyTree(DataRow row, TreeListNode parentNode)
        {
            try
            {
                //读取科
                DataRow[] rows = null;
                string mr_name = "";
                //03节点
                TreeListNode node = null;//

                node = treeListMy.AppendNode(new object[] { row["DEPT_NAME"].ToString(), row["DEPT_ID"].ToString() }, parentNode);
                node.Tag = new EmrNodeObj(row, EmrNodeType.Container);
                if (!row["DEPT_ID"].ToString().Equals("*"))
                {   //03子节点
                    rows = m_Util.EmrDepartment.Select("DEPT_ID <>'" + row["DEPT_ID"].ToString() + "' and DEPT_ID like '" + row["DEPT_ID"].ToString() + "%'");
                    foreach (DataRow myRow in rows)
                    {
                        //todo 
                        MakeMyTree(myRow, node);
                    }
                    if (rows.Length > 0) return;
                }
                //读取创建子集文件夹 注释掉试试
                foreach (DataRow catalogRow in m_Util.ModelContainers.Rows)
                {
                    TreeListNode catalogNode = treeListMy.AppendNode(new object[] { catalogRow["CNAME"].ToString(), catalogRow["CCODE"].ToString() + row["DEPT_ID"] }, node);
                    catalogNode.Tag = new EmrNodeObj(catalogRow, EmrNodeType.Container);

                    //读取模板
                    m_Util.MyModelList.DefaultView.RowFilter = "DEPT_ID='" + row["DEPT_ID"] + "' and MR_CLASS='" + catalogRow["CCODE"] + "'" + " and Valid=1 ";

                    DataTable table = m_Util.MyModelList.DefaultView.ToTable();
                    //设置叶子节点
                    foreach (DataRow leafRow in table.Rows)
                    {
                        //if (leafRow["todept"].Equals("0"))
                        //    mr_name = "☋" + leafRow["mr_name"].ToString();
                        //else
                        mr_name = leafRow["mr_name"].ToString();
                        TreeListNode leafNode = treeListMy.AppendNode(new object[] { mr_name, leafRow["Templet_ID"].ToString() }, catalogNode);
                        leafNode.Tag = new EmrNodeObj(leafRow, EmrNodeType.Leaf);
                    }

                }
                //BulidChildNode(row, node);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void BulidChildNode(DataRow row, TreeListNode node)
        {
            try
            {
                string mr_name = "";
                foreach (DataRow catalogRow in m_Util.ModelContainers.Rows)
                {
                    TreeListNode catalogNode = treeList_Template.AppendNode(new object[] { catalogRow["CNAME"].ToString(), catalogRow["CCODE"].ToString() + row["DEPT_ID"] }, node);
                    catalogNode.Tag = new EmrNodeObj(catalogRow, EmrNodeType.Container);

                    //读取模板
                    m_Util.ModelList.DefaultView.RowFilter = "DEPT_ID='" + row["DEPT_ID"] + "' and MR_CLASS='" + catalogRow["CCODE"] + "'" + " and Valid=1 ";

                    DataTable table = m_Util.ModelList.DefaultView.ToTable();
                    //设置叶子节点
                    foreach (DataRow leafRow in table.Rows)
                    {
                        //if (leafRow["todept"].Equals("0"))
                        //    mr_name = "☋" + leafRow["mr_name"].ToString();
                        //else
                        mr_name = leafRow["mr_name"].ToString();
                        TreeListNode leafNode = treeList_Template.AppendNode(new object[] { mr_name, leafRow["Templet_ID"].ToString() }, catalogNode);
                        leafNode.Tag = new EmrNodeObj(leafRow, EmrNodeType.Leaf);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void MakeTree2(DataRow row, TreeListNode parentNode)
        {
            try
            {
                //读取科
                DataRow[] rows = null;
                //03节点
                TreeListNode node = null;//
                node = treeList_Items.AppendNode(new object[] { row["DEPT_NAME"].ToString(), row["DEPT_ID"].ToString() }, parentNode);
                node.Tag = new EmrNodeObj(row, EmrNodeType.Container);

                if (!row["DEPT_ID"].ToString().Equals("*"))
                {   //03子节点
                    rows = m_Util.EmrDepartment.Select("DEPT_ID <>'" + row["DEPT_ID"].ToString() + "' and DEPT_ID like '" + row["DEPT_ID"].ToString() + "%'");
                    foreach (DataRow myRow in rows)
                    {
                        //todo 
                        MakeTree2(myRow, node);
                    }
                    if (rows.Length > 0) return;

                    //读取创建子集文件夹
                    foreach (string key in m_Util.ItemCatalog.Keys)
                    {
                        TreeListNode catalogNode = treeList_Items.AppendNode(new object[] { key, m_Util.ItemCatalog[key] + row["DEPT_ID"].ToString() }, node);
                        catalogNode.Tag = new EmrNodeObj(m_Util.ItemCatalog[key], EmrNodeType.Container);
                        //读取子模板

                        m_Util.ItemModelList.DefaultView.RowFilter = "DEPT_ID='" + row["DEPT_ID"] + "' and MR_CLASS='" + m_Util.ItemCatalog[key] + "'";

                        DataTable table = m_Util.ItemModelList.DefaultView.ToTable();
                        //设置叶子节点
                        foreach (DataRow leafRow in table.Rows)
                        {
                            TreeListNode leafNode = treeList_Items.AppendNode(new object[] { leafRow["mr_name"].ToString() }, catalogNode);
                            leafNode.Tag = new EmrNodeObj(leafRow, EmrNodeType.Leaf);
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
        /// 病历模板列表
        /// </summary>
        private void InitTemplateTree()
        {
            // try
            // {
            //SetWaitDialogCaption("正在加载模板内容...");

            //this.marqueeProgressBarControl1.Text = "正在加载模板列表......";
            this.marqueeProgressBarControl1.Properties.ShowTitle = true;
            this.marqueeProgressBarControl1.ToolTipTitle = "正在加载模板列表......";
            this.marqueeProgressBarControl1.Properties.MarqueeAnimationSpeed = 60;
            //this.marqueeProgressBarControl1.Properties
            this.marqueeProgressBarControl1.Properties.Stopped = false;


            treeList_Template.BeginUnboundLoad();
            treeList_Template.ClearNodes();
            //捞起通用模板 代码需要优化
            DataRow[] rws = m_Util.EmrDepartment.Select("Len(DEPT_ID)=1");
            if (rws.Length > 0)
            {
                DataRow row = rws[0];
                TreeListNode node = null;//
                node = treeList_Template.AppendNode(new object[] { row["DEPT_NAME"].ToString(), row["DEPT_ID"].ToString() }, null);
                node.Tag = new EmrNodeObj(row, EmrNodeType.Container);

                //读取创建子集文件夹
                //注释掉
                foreach (DataRow catalogRow in m_Util.ModelContainers.Rows)
                {
                    TreeListNode catalogNode = treeList_Template.AppendNode(new object[] { catalogRow["CNAME"].ToString(), catalogRow["CCODE"].ToString() + row["DEPT_ID"] }, node);
                    catalogNode.Tag = new EmrNodeObj(catalogRow, EmrNodeType.Container);

                    //读取模板
                    m_Util.ModelList.DefaultView.RowFilter = "DEPT_ID='" + row["DEPT_ID"] + "' and MR_CLASS='" + catalogRow["CCODE"] + "'" + " and Valid=1 ";

                    DataTable table = m_Util.ModelList.DefaultView.ToTable();
                    //设置叶子节点
                    foreach (DataRow leafRow in table.Rows)
                    {
                        TreeListNode leafNode = treeList_Template.AppendNode(new object[] { leafRow["mr_name"].ToString(), leafRow["Templet_ID"].ToString() }, catalogNode);

                        leafNode.Tag = new EmrNodeObj(leafRow, EmrNodeType.Leaf);
                    }
                }
            }

            //edit by cyq 2013-02-19
            DataRow[] rows = FilterRepeatDept(m_Util.EmrDepartment).Select("Len(DEPT_ID)>1");
            //DataRow[] rows;
            //if (IsAdministrator)
            //{
            //    rows = m_Util.EmrDepartment.Select("Len(DEPT_ID)=2");
            //}
            //else
            //    rows = m_Util.EmrDepartment.Select("Len(DEPT_ID)>1");

            if (null != rows && rows.Count() > 0)
            {
                foreach (DataRow row in rows)
                {
                    MakeTree(row, null);
                }
            }
            treeList_Template.EndUnboundLoad();
            //加载完毕再打开可编辑操作 add by ywk 2013年4月2日16:42:19 
            treeList_Template.Enabled = true;
            this.marqueeProgressBarControl1.Properties.Stopped = true;
            this.marqueeProgressBarControl1.Visible = false;
            HideWaitDialog();
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }

        /// <summary>
        /// 子模板列表
        /// </summary>
        private void InitItemTemplateTree()
        {
            try
            {
                treeList_Items.BeginUnboundLoad();
                treeList_Items.ClearNodes();
                //捞起通用模板
                DataRow[] rws = m_Util.EmrDepartment.Select("Len(DEPT_ID)=1");
                if (rws.Length > 0)
                {
                    TreeListNode node = null;//
                    DataRow row = rws[0];
                    node = treeList_Items.AppendNode(new object[] { row["DEPT_NAME"].ToString(), row["DEPT_ID"].ToString() }, null);
                    node.Tag = new EmrNodeObj(rws[0], EmrNodeType.Container);

                    //读取创建子集文件夹
                    foreach (string key in m_Util.ItemCatalog.Keys)
                    {
                        TreeListNode catalogNode = treeList_Items.AppendNode(new object[] { key, m_Util.ItemCatalog[key] + row["DEPT_ID"].ToString() }, node);
                        catalogNode.Tag = new EmrNodeObj(m_Util.ItemCatalog[key], EmrNodeType.Container);
                        //读取子模板

                        m_Util.ItemModelList.DefaultView.RowFilter = "DEPT_ID='" + row["DEPT_ID"] + "' and MR_CLASS='" + m_Util.ItemCatalog[key] + "'";

                        DataTable table = m_Util.ItemModelList.DefaultView.ToTable();
                        //设置叶子节点
                        foreach (DataRow leafRow in table.Rows)
                        {
                            TreeListNode leafNode = treeList_Items.AppendNode(new object[] { leafRow["mr_name"].ToString() }, catalogNode);
                            leafNode.Tag = new EmrNodeObj(leafRow, EmrNodeType.Leaf);
                        }
                    }

                }

                //edit by cyq 2013-02-19
                DataRow[] rows = FilterRepeatDept(m_Util.EmrDepartment).Select("Len(DEPT_ID)>1");
                //DataRow[] rows;
                //if (IsAdministrator)
                //{
                //    rows = m_Util.EmrDepartment.Select("Len(DEPT_ID)=2");
                //}
                //else
                //    rows = m_Util.EmrDepartment.Select("Len(DEPT_ID)>1");

                if (null != rows && rows.Count() > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        MakeTree2(row, null);
                        //TreeListNode node = treeList_Items.AppendNode(new object[] { row["DEPT_NAME"].ToString() }, null);
                        //node.Tag = new EmrNodeObj(row, EmrNodeType.Container);
                        ////读取创建子集文件夹
                        //foreach (string key in m_Util.ItemCatalog.Keys)
                        //{
                        //    TreeListNode catalogNode = treeList_Items.AppendNode(new object[] { key }, node);
                        //    catalogNode.Tag = new EmrNodeObj(m_Util.ItemCatalog[key], EmrNodeType.Container);
                        //    //读取子模板

                        //    m_Util.ItemModelList.DefaultView.RowFilter = "DEPT_ID='" + row["DEPT_ID"] + "' and MR_CLASS='" + m_Util.ItemCatalog[key] + "'";

                        //    DataTable table = m_Util.ItemModelList.DefaultView.ToTable();
                        //    //设置叶子节点
                        //    foreach (DataRow leafRow in table.Rows)
                        //    {
                        //        TreeListNode leafNode = treeList_Template.AppendNode(new object[] { leafRow["mr_name"].ToString() }, node);
                        //        leafNode.Tag = new EmrNodeObj(leafRow, EmrNodeType.Leaf);
                        //    }
                    }
                }
                treeList_Items.EndUnboundLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void InitMyTemplateTree()
        {
            try
            {
                treeListMy.BeginUnboundLoad();
                treeListMy.ClearNodes();
                //捞起通用模板 代码需要优化
                DataRow[] rws = m_Util.EmrDepartment.Select("Len(DEPT_ID)=1");
                if (rws.Length > 0)
                {
                    DataRow row = rws[0];
                    TreeListNode node = null;//
                    node = treeListMy.AppendNode(new object[] { row["DEPT_NAME"].ToString(), row["DEPT_ID"].ToString() }, null);
                    node.Tag = new EmrNodeObj(row, EmrNodeType.Container);

                    //读取创建子集文件夹
                    //注释掉
                    foreach (DataRow catalogRow in m_Util.ModelContainers.Rows)
                    {
                        TreeListNode catalogNode = treeListMy.AppendNode(new object[] { catalogRow["CNAME"].ToString(), catalogRow["CCODE"].ToString() + row["DEPT_ID"] }, node);
                        catalogNode.Tag = new EmrNodeObj(catalogRow, EmrNodeType.Container);

                        //读取模板
                        m_Util.MyModelList.DefaultView.RowFilter = "DEPT_ID='" + row["DEPT_ID"] + "' and MR_CLASS='" + catalogRow["CCODE"] + "'" + " and Valid=1 ";

                        DataTable table = m_Util.MyModelList.DefaultView.ToTable();
                        //设置叶子节点
                        foreach (DataRow leafRow in table.Rows)
                        {
                            TreeListNode leafNode = treeListMy.AppendNode(new object[] { leafRow["mr_name"].ToString(), leafRow["Templet_ID"].ToString() }, catalogNode);
                            leafNode.Tag = new EmrNodeObj(leafRow, EmrNodeType.Leaf);
                        }
                    }
                }

                //edit by cyq 2013-02-19
                DataRow[] rows = FilterRepeatDept(m_Util.EmrDepartment).Select("Len(DEPT_ID)>1");
                //DataRow[] rows;
                //if (IsAdministrator)
                //{
                //    rows = m_Util.EmrDepartment.Select("Len(DEPT_ID)=2");
                //}
                //else
                //    rows = m_Util.EmrDepartment.Select("Len(DEPT_ID)>1");

                if (null != rows && rows.Count() > 0)
                {
                    foreach (DataRow row in rows)
                    {
                        MakeMyTree(row, null);
                    }
                }
                treeListMy.EndUnboundLoad();

                HideWaitDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 子模板列表
        /// </summary>
        private void InitSubTemplateTree()
        {
        }

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
                throw ex;
            }
        }

        private void InitBasic()
        {
            try
            {
                this.navBarControl1.View = new DevExpress.XtraNavBar.ViewInfo.StandardSkinNavigationPaneViewInfoRegistrator("Office 2010 Silver");
                EditorPadForm padForm = new EditorPadForm(m_app);
                padForm.PropertyObjChanged += new EventHandler<EventOjbArgs>(padForm_PropertyObjChanged);
                pnlText = padForm.zyEditorControl1;
                padForm.Dock = DockStyle.Fill;
                this.Controls.Add(padForm);

                padForm.propertyGrid = this.propertyGridControl1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void padForm_Click(object sender, EventOjbArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 去掉重复显示
        /// 即：有大科室无(对应的)小科室，有小科室则无(对应的)大科室。
        /// </summary>
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2013-02-19</date>
        /// <param name="dt"></param>
        /// <returns></returns>
        private DataTable FilterRepeatDept(DataTable dt)
        {
            try
            {
                //xll 2013-04-26
                DataTable dttem = dt.Copy();
                dttem.Clear();
                //获取数据集中的大科室ID
                List<string> bigDeptIDs = dt.Select("Len(DEPT_ID)=2").Select(p => p["DEPT_ID"].ToString()).ToList();
                var rows = dt.Select(" 1=1 ").AsEnumerable();
                if (null != bigDeptIDs && bigDeptIDs.Count() > 0)
                {
                    foreach (string str in bigDeptIDs)
                    {
                        var repIDs = rows.Where(p => p["DEPT_ID"].ToString().Contains(str) && p["DEPT_ID"].ToString().Trim().Length > 2).Select(p => p["DEPT_ID"].ToString()).ToList();
                        rows = rows.Where(p => !repIDs.Contains(p["DEPT_ID"].ToString()));
                    }
                }
                return (null == rows || rows.Count() == 0) ? dttem : rows.CopyToDataTable();
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
                    s_LineSpace = float.Parse(m_app.EmrDefaultSettings.LineSpace);
                }
                else
                {
                    s_LineSpace = 1.5f;
                }
                s_ElementColor = m_app.EmrDefaultSettings.EleColor;
                s_ElementStyle = m_app.EmrDefaultSettings.EleStyle;


                //行间距
                pnlText.SetLineSpace(s_LineSpace);

                ZYEditorControl.ElementBackColor = s_ElementColor;
                ZYEditorControl.ElementStyle = s_ElementStyle;

                //PadForm.s_LineHeight = Convert.ToInt32(m_app.EmrDefaultSettings.LineHeight);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化特殊符号
        /// </summary>
        /// <param name="dataTableSymbol"></param>
        private void BindSymbol(DataTable dataTableSymbol)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private bool bolSetForeColor = true;
        /// <summary>
        /// set color event handles
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">argument</param>
        private void SetDocumentColor(object sender, System.EventArgs e)
        {
            try
            {
                ColorMenuItem item = sender as ColorMenuItem;
                if (item != null)
                {
                    if (bolSetForeColor)
                    {
                        this.pnlText.EMRDoc.SetSelectionColor(item.Color);

                    }
                    else
                    {
                        this.pnlText.PageBackColor = item.Color;
                        this.pnlText.Invalidate();
                        //MessageBox.Show(item.Color + "设置文档背景色,没有合适的方法");
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        #region 表格相关操作


        private void InsertTable()
        {
            try
            {
                TableInsert tableInsertForm = new TableInsert();
                if (tableInsertForm.ShowDialog() == DialogResult.OK)
                {
                    pnlText.EMRDoc.TableInsert(tableInsertForm.Header,
                        tableInsertForm.Rows, tableInsertForm.Columns,
                        tableInsertForm.ColumnWidth);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertTableRow()
        {
            try
            {
                RowsInsert rowsInsertForm = new RowsInsert();
                if (rowsInsertForm.ShowDialog() == DialogResult.OK)
                {
                    pnlText.EMRDoc.RowInsert(rowsInsertForm.RowNum, rowsInsertForm.IsBack);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertTableColumn()
        {
            try
            {
                ColumnsInsert columnsInsertForm = new ColumnsInsert();
                if (columnsInsertForm.ShowDialog() == DialogResult.OK)
                {
                    pnlText.EMRDoc.ColumnInsert(columnsInsertForm.ColumnNum, columnsInsertForm.IsBack);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeleteTable()
        {
            try
            {
                pnlText.EMRDoc.TableDelete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeletTableRow()
        {
            try
            {
                pnlText.EMRDoc.TableDeleteRow();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DeletTableColumn()
        {
            try
            {
                pnlText.EMRDoc.TableDeleteCol();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChoiceTable()
        {
            try
            {
                pnlText.EMRDoc.TableSelect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChoiceTableRow()
        {
            try
            {
                pnlText.EMRDoc.TableSelectRow();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ChoiceTableColumn()
        {
            try
            {
                pnlText.EMRDoc.TableSelectCol();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MergeTableCell()
        {
            try
            {
                if (pnlText.EMRDoc.TableIsCanMerge() == 1)
                {
                    pnlText.EMRDoc.TableMergeCell();
                }
                else if (pnlText.EMRDoc.TableIsCanMerge() == 2)
                {
                    MessageBox.Show("此次操作须得在表格中选中单元格才能生效", "不能合并", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else if (pnlText.EMRDoc.TableIsCanMerge() == 3)
                {
                    MessageBox.Show("您并没有选中任何单元格", "不能合并", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else if (pnlText.EMRDoc.TableIsCanMerge() == 4)
                {
                    MessageBox.Show("您只选中了一个单元格,无法再次合并", "不能合并", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else if (pnlText.EMRDoc.TableIsCanMerge() == 5)
                {
                    MessageBox.Show("请检查您选择的单元格是否呈正规矩形", "不能合并", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ApartTableCell()
        {
            try
            {
                Dictionary<string, int> splitDic = pnlText.EMRDoc.TableBeforeSplitCell();
                ApartCell apartCellForm = new ApartCell(splitDic["row"], splitDic["col"]);

                if (apartCellForm.ShowDialog() == DialogResult.OK)
                {
                    pnlText.EMRDoc.TableSplitCell(apartCellForm.intRow, apartCellForm.intColumn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetTableProperty()
        {
            try
            {
                TPTextTable table = pnlText.EMRDoc.GetCurrentTable();
                TPTextCell cell = pnlText.EMRDoc.GetCurrentCell();
                if (table != null && cell != null)
                {
                    TableProperty tablePropertyForm = new TableProperty(table, cell);
                    if (tablePropertyForm.ShowDialog() == DialogResult.OK)
                    {

                        pnlText.EMRDoc.ContentChanged();
                        pnlText.EMRDoc.Refresh();
                        //pnlText.EMRDoc.SetTableProperty(tablePropertyForm.Table);
                    }
                }
                else
                {
                    MessageBox.Show("光标必须在表格中才能更改表格属性", "不能打开表格属性", MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 操作病历模板相关


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
                        pnlText.EMRDoc.ToXMLFile(dlg.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void OpenTemplate()
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.CheckFileExists = true;
                    //dlg.Filter = "xml文件|*.xml|模板文件|*.t";
                    if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        FileInfo fi = new FileInfo(dlg.FileName);
                        if (fi.Extension == ".t")
                        {

                            this.pnlText.EMRDoc.ShowHeader = false;
                            this.pnlText.EMRDoc.ShowFooter = false;
                            this.pnlText.EMRDoc.HeadString = "";
                            this.pnlText.EMRDoc.FooterString = "";
                        }
                        else
                        {

                            this.pnlText.EMRDoc.ShowHeader = true;
                            this.pnlText.EMRDoc.ShowFooter = true;
                        }

                        if (this.pnlText.LoadXMLFile(dlg.FileName))
                        {
                            this.Text = fi.Name;
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
        /// 在加载新数据之前保存数据
        /// </summary>
        private void LoadBeforeSave()
        {
            try
            {
                this.pnlText.EMRDoc.ShowFooter = true;
                this.pnlText.EMRDoc.ShowHeader = true;

                //在切换病历文件节点(Node.tag =EmrNodeObj 且type=Leaf)的时候，则判断当前模板是否已经改变是则提示保存否则放弃
                //if(pnlText.EMRDoc.Modified)
                //m_app.CustomMessageBox.MessageShow()？
                if (pnlText.EMRDoc.Modified)
                {
                    //提示保存
                    if (m_app.CustomMessageBox.MessageShow(string.Format("您有数据未保存，是否保存？"), CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        if (m_EmrTempletType == EmrTempletType.Templet)
                            UpdateTemplet();
                        else if (m_EmrTempletType == EmrTempletType.Templet_Item)
                            UpdateTemplet_Item();
                        else if (m_EmrTempletType == EmrTempletType.Templet_Header)
                            UpdateTemplet_Header();
                        else if (m_EmrTempletType == EmrTempletType.Templet_Foot)
                            UpdateTemplet_Foot();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载模板
        /// </summary>
        /// <param name="templetId"></param>
        private void LoadTemplet(Emrtemplet emrtemplet)
        {
            try
            {
                LoadBeforeSave();

                //加载数据
                m_Emrtemplet = emrtemplet;

                //新增模板添加空XML
                if (m_Emrtemplet.XML_DOC_NEW.Length == 0)
                {
                    this.pnlText.EMRDoc.ClearContent();
                    //return;
                }
                else
                {
                    XmlDocument content = new XmlDocument();
                    content.PreserveWhitespace = true;
                    content.LoadXml(m_Emrtemplet.XML_DOC_NEW);
                    this.pnlText.EMRDoc.ClearContent();
                    pnlText.LoadXML(content);
                }
                this.pnlText.EMRDoc.ShowHeader = true;
                this.pnlText.EMRDoc.ShowFooter = true;
                //设置页眉头
                pnlText.SetTitle(m_app.CurrentHospitalInfo.Name);
                //读取子模板
                InitSubItemTree(emrtemplet.DeptId);
                //初始化EMR
                //InitEmrDefaultSet();
                pnlText.EMRDoc.Modified = false;

                ////行间距
                //pnlText.SetLineSpace(2.2f);

                SetHeaderFooterHeight();

                pnlText.EMRDoc.Modified = false;

                SetTempletAuditState(emrtemplet);

                //追加判断 如果审核功能关闭 则 提交审核按钮变灰
                string[] config = m_SqlManger.GetTempletAuditConfig();
                if (config.Length != 3)
                    return;
                //如果配置表中审核功能 关闭 则不进行审核  保存后直接为审核后状态 直接可以使用
                if (config[0] == "0")
                {
                    //edit by cyq 2013-02-27
                    SetAuditBtnState(false);
                    //btn_commit.Enabled = false;
                    //btn_Audit.Enabled = false;
                    //btn_CancelSubmit.Enabled = false;
                    //btn_NoAudit.Enabled = false;
                }
                //add by Yanqiao.Cai 2013-03-11
                this.Text = "模型维护工厂" + " --- " + m_Emrtemplet.MrName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 权限相关控制

        /// <summary>
        /// 根据当前登录人权限 已经选择的模版科室 判断是否能够修改当前病历
        /// </summary>
        /// <param name="emrtemplet"></param>
        private bool SetDocumentModel(string deptid)
        {
            try
            {
                string[] config = m_SqlManger.GetTempletAuditConfig();
                if (config.Length != 3)
                    return false;
                else
                {
                    if (config[0] == "0")
                    {
                        return false;
                    }
                    else
                    {
                        DataTable dt = m_SqlManger.GetJobsByUserID(m_app.User.Id);
                        string jobid = dt.Rows[0][0].ToString();

                        //判断如果登陆人有管理员权限 则全部可以修改
                        if (CheckJobInConfig(jobid, config[1]))
                        {
                            return true;
                        }
                        //如果有模板管理，主任医生权限 则可以修改当前科室模版
                        else //if (jobid.IndexOf("77") > -1 || jobid.IndexOf("11") > -1)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (deptid == dr["deptid"].ToString())
                                {
                                    return true;
                                }
                            }
                            return false;
                        }
                    }
                    //else
                    //{ 
                    //    return false;
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据保存人的权限 判断新增模版状态   主任与管理员直接为提交状态 
        /// </summary>
        /// <param name="emrtemplet"></param>
        private void GetTempletState(Emrtemplet emrtemplet)
        {
            try
            {
                if (null == emrtemplet)
                {
                    SetAuditBtnState(false);
                    return;
                }
                //读取配置表中信息 判断
                string[] config = m_SqlManger.GetTempletAuditConfig();
                if (config.Length != 3)
                    return;
                else
                {
                    //如果配置表中审核功能 关闭 则不进行审核  保存后直接为审核后状态 直接可以使用
                    if (config[0] == "0")
                    {
                        emrtemplet.State = "2";
                        emrtemplet.Auditor = m_app.User.Id;
                        return;
                    }
                    else
                    {
                        DataTable dt = m_SqlManger.GetJobsByUserID(m_app.User.Id);
                        string jobid = dt.Rows[0][0].ToString();

                        //判断如果登陆人有管理员权限 则保存病历为审核完成状态
                        if (CheckJobInConfig(jobid, config[1]))
                        {
                            emrtemplet.State = "2";
                            emrtemplet.Auditor = m_app.User.Id;
                        }
                        //如果有模板管理，主任医生权限 则保存病历为审核完成状态
                        //else if (jobid.IndexOf("77") > -1 || jobid.IndexOf("11") > -1)
                        else if (CheckJobInConfig(jobid, config[2]))
                        {
                            emrtemplet.State = "2";
                            emrtemplet.Auditor = m_app.User.Id;
                        }
                        else
                        {
                            emrtemplet.State = "0";
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
        /// 判断角色是否在配置的角色中
        /// </summary>
        /// <param name="jobid"></param>
        /// <param name="configjob"></param>
        /// <returns></returns>
        private bool CheckJobInConfig(string jobid, string configjob)
        {
            try
            {
                string[] configjobs = configjob.Split(',');

                foreach (string cng in configjobs)
                {
                    if (jobid.IndexOf(cng) > -1)
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
        /// 判断当前病历是否可以审核
        /// </summary>
        /// <param name="emrtemplet"></param>
        private void SetTempletAuditState(Emrtemplet emrtemplet)
        {
            try
            {
                if (null == emrtemplet)
                {
                    SetAuditBtnState(false);
                    return;
                }
                string[] config = m_SqlManger.GetTempletAuditConfig();
                if (config.Length != 3)
                    return;
                else
                {
                    //开关关了则直接有审核权限
                    if (config[0] == "0")
                    {
                        //DataTable dt = m_SqlManger.GetJobsByUserID(m_app.User.Id);
                        //string jobid = dt.Rows[0][0].ToString();

                        ////判断如果登陆人有管理员权限 则全部可以修改
                        //if (CheckJobInConfig(jobid, config[1]))
                        //{
                        //    CanEditEmrDoc(true);
                        //}
                        ////如果有模板管理，主任医生权限 则可以修改当前科室模版
                        //else //if (jobid.IndexOf("77") > -1 || jobid.IndexOf("11") > -1)
                        //{
                        //    foreach (DataRow dr in dt.Rows)
                        //    {
                        //        if (emrtemplet.DeptId == dr["deptid"].ToString())
                        //        {
                        CanEditEmrDoc(true);
                        //edit by cyq 2013-02-27
                        SetAuditBtnState(false);
                        //btn_commit.Enabled = false;
                        //btn_Audit.Enabled = false;
                        //btn_CancelSubmit.Enabled = false;
                        return;
                        //        }
                        //    }
                        //    CanEditEmrDoc(false);
                        //} 
                        //btn_commit.Enabled = false;
                        //btn_Audit.Enabled = false;
                        //btn_CancelSubmit.Enabled = false;
                    }
                    else
                    {
                        DataTable dt = m_SqlManger.GetJobsByUserID(m_app.User.Id);
                        string jobid = dt.Rows[0][0].ToString();

                        //判断如果登陆人有管理员权限 则全部可以修改
                        if (CheckJobInConfig(jobid, config[1]))
                        {
                            if (emrtemplet.State == "0")
                            {
                                if (emrtemplet.CreatorId == m_app.User.Id)
                                {
                                    CanEditEmrDoc(true);
                                }
                                else
                                {
                                    CanEditEmrDoc(false);
                                }
                            }
                            else if (emrtemplet.State == "1")
                                CanEditEmrDoc(true);
                            else if (emrtemplet.State == "2")
                                CanEditEmrDoc(true);
                            else if (emrtemplet.State == "3")
                            {
                                if (emrtemplet.CreatorId == m_app.User.Id)
                                {
                                    CanEditEmrDoc(true);
                                    btn_CancelSubmit.Enabled = false;
                                }
                                else
                                {
                                    CanEditEmrDoc(false);
                                }
                            }
                            else
                            {
                                CanEditEmrDoc(true);
                            }

                            btn_commit.Enabled = false;

                            return;
                        }
                        //如果有模板管理，主任医生权限 则可以修改当前科室模版
                        else if (CheckJobInConfig(jobid, config[2]))
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (emrtemplet.DeptId == dr["deptid"].ToString())
                                {
                                    if (emrtemplet.State == "0")
                                    {
                                        if (emrtemplet.CreatorId == m_app.User.Id)
                                        {
                                            CanEditEmrDoc(true);
                                        }
                                        else
                                        {
                                            CanEditEmrDoc(false);
                                        }
                                    }
                                    else if (emrtemplet.State == "1")
                                    {
                                        CanEditEmrDoc(true);
                                        btn_commit.Enabled = false;
                                    }
                                    //已审核状态可以修改病历 但是不能审核 反审核病历
                                    else if (emrtemplet.State == "2")
                                    {
                                        CanEditEmrDoc(true);
                                    }
                                    else if (emrtemplet.State == "3")
                                    {
                                        if (emrtemplet.CreatorId == m_app.User.Id)
                                        {
                                            CanEditEmrDoc(true);
                                            btn_CancelSubmit.Enabled = false;
                                        }
                                        else
                                        {
                                            CanEditEmrDoc(false);
                                        }
                                    }
                                    else
                                    {
                                        CanEditEmrDoc(true);
                                    }
                                    btn_commit.Enabled = false;
                                    return;
                                }
                            }
                            CanEditEmrDoc(false);

                        }
                        else   //如果是住院医生填写  只能提交和修改自己录入的模版
                        {
                            if (emrtemplet.State == "0")
                            {
                                if (emrtemplet.CreatorId == m_app.User.Id)
                                {
                                    CanEditEmrDoc(true);
                                    btn_commit.Enabled = true;


                                }
                                else
                                {
                                    CanEditEmrDoc(false);
                                    btn_commit.Enabled = false;
                                }
                            }
                            else if (emrtemplet.State == "1")
                            {
                                CanEditEmrDoc(false);
                            }
                            else if (emrtemplet.State == "2")
                                CanEditEmrDoc(false);
                            else if (emrtemplet.State == "3")
                            {
                                if (emrtemplet.CreatorId == m_app.User.Id)
                                {
                                    CanEditEmrDoc(true);
                                    btn_commit.Enabled = true;
                                }
                                else
                                {
                                    CanEditEmrDoc(false);
                                    btn_commit.Enabled = false;
                                }
                            }
                            else
                            {
                                CanEditEmrDoc(true);
                            }


                            btn_Audit.Enabled = false;
                            btn_CancelSubmit.Enabled = false;
                            return;
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
        /// 设置提交、审核按钮显示状态
        /// </summary>
        /// <param name="boo"></param>
        private void SetAuditBtnState(bool boo)
        {
            try
            {
                //提交
                this.btn_commit.Enabled = boo;
                //审核
                this.btn_Audit.Enabled = boo;
                //审核不通过
                this.btn_CancelSubmit.Enabled = boo;
                //待审核模板(按钮)
                this.btn_NoAudit.Enabled = boo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary> 
        /// 设置页眉和页脚高度 Add by wwj 2012-02-16
        /// </summary>
        private void SetHeaderFooterHeight()
        {
            try
            {
                string config = m_Util.GetConfigValueByKey("PageSetting");
                System.Xml.XmlDocument doc = new XmlDocument();
                doc.LoadXml(config);
                XmlNode pagesetting = doc.ChildNodes[0].SelectSingleNode("pagesettings");
                XmlElement ele = (pagesetting as XmlElement).SelectSingleNode("header") as XmlElement;
                pnlText.EMRDoc.DocumentHeaderHeight = Convert.ToInt32(ele.GetAttribute("height"));
                ele = (pagesetting as XmlElement).SelectSingleNode("footer") as XmlElement;
                pnlText.EMRDoc.DocumentFooterHeight = Convert.ToInt32(ele.GetAttribute("height"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载页脚
        /// </summary>
        /// <param name="templetId"></param>
        private void LoadTemplet_Foot(EmrTemplet_Foot emrtemplet_Foot)
        {
            try
            {
                LoadBeforeSave();
                //加载数据
                m_EmrTemplet_Foot = emrtemplet_Foot;

                //新增模板添加空XML
                if (emrtemplet_Foot.Content.Length == 0)
                {
                    this.pnlText.EMRDoc.ClearContent();
                    return;
                }
                XmlDocument content = new XmlDocument();
                content.LoadXml(emrtemplet_Foot.Content);
                this.pnlText.EMRDoc.ClearContent();
                pnlText.LoadXML(content);

                this.pnlText.EMRDoc.ShowHeader = false;
                this.pnlText.EMRDoc.ShowFooter = false;

                m_EmrTempletType = EmrTempletType.Templet_Foot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载页眉
        /// </summary>
        /// <param name="templetId"></param>
        private void LoadTemplet_Header(EmrTempletHeader emrTempletHeader)
        {
            try
            {
                LoadBeforeSave();
                //加载数据
                m_EmrTempletHeader = emrTempletHeader;

                //新增模板添加空XML
                if (emrTempletHeader.Content.Length == 0)
                {
                    this.pnlText.EMRDoc.ClearContent();
                    return;
                }
                XmlDocument content = new XmlDocument();
                content.PreserveWhitespace = true;
                content.LoadXml(emrTempletHeader.Content);
                this.pnlText.EMRDoc.ClearContent();
                pnlText.LoadXML(content);

                this.pnlText.EMRDoc.ShowHeader = false;
                this.pnlText.EMRDoc.ShowFooter = false;

                m_EmrTempletType = EmrTempletType.Templet_Header;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 加载子模板
        /// </summary>
        /// <param name="templetId"></param>
        private void LoadTemplet_Item(Emrtemplet_Item emrtemplet_item)
        {
            try
            {
                LoadBeforeSave();
                //加载数据
                m_Emrtemplet_Item = emrtemplet_item;

                //新增模板添加空XML
                if (m_Emrtemplet_Item.ITEM_DOC_NEW.Length == 0)
                {
                    this.pnlText.EMRDoc.ClearContent();
                    return;
                }
                XmlDocument content = new XmlDocument();
                content.LoadXml(m_Emrtemplet_Item.ITEM_DOC_NEW);
                this.pnlText.EMRDoc.ClearContent();
                pnlText.LoadXML(content);
                this.pnlText.EMRDoc.ShowHeader = true;
                this.pnlText.EMRDoc.ShowFooter = true;

                InitSubItemTree(emrtemplet_item.DeptId);
                //读取子模板
                InitSubItemTree(emrtemplet_item.DeptId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        ///yxy 根据新增模板基本信息条用条件新模板页面（树节点添加时候保存基本信息）toolbar按钮添加模板信息为空
        /// edit by Yanqiao.Cai 2012-11-06 add try ... catch
        /// </summary>
        /// <param name="emrtemplet"></param>
        private void AddNewTemplet(Emrtemplet emrtemplet)
        {
            try
            {
                //弹出模板标题窗口，参考天鹏界面
                //改变构造函数，设置页面设置复选框显示情况
                AddNewTemplate addnewtemplate = new AddNewTemplate(emrtemplet, m_app, IsShowPageConfig);
                addnewtemplate.Text = "病例模板属性";

                //此处取得是不是有星号 addd by ywk 2012年4月15日13:07:59
                bool HasStar = true;
                HasStar = addnewtemplate.ContainStar;

                DataRow obj = (DataRow)((EmrNodeObj)m_treelistnode.Tag).Obj;
                if (barButtonItem10.Caption == "修改")
                {
                    addnewtemplate.InitValue(obj);
                }
                else//add by cyq 2013-02-19
                {//新增
                    if (obj.Table.Columns.Contains("CCODE"))
                    {
                        string mr_class = null == obj["CCODE"] ? "" : obj["CCODE"].ToString();
                        string dept_id = string.Empty;
                        if (null != m_treelistnode && null != m_treelistnode.ParentNode && null != m_treelistnode.ParentNode.Tag)
                        {
                            DataRow parentRow = (DataRow)((EmrNodeObj)m_treelistnode.ParentNode.Tag).Obj;
                            dept_id = (null == parentRow || null == parentRow["DEPT_ID"]) ? "" : parentRow["DEPT_ID"].ToString();
                        }
                        addnewtemplate.SetDefaultValue(mr_class, dept_id);
                    }
                    else
                    {
                        addnewtemplate.SetFirstFocusControl();
                    }
                }
                addnewtemplate.ShowDialog();
                //如果不是点击确认按钮的则不记录新的模板信息
                if (!addnewtemplate.m_IsOK)
                {
                    m_new_Emrtemplet = new Emrtemplet();
                }
                else
                {
                    m_new_Emrtemplet = addnewtemplate.m_emrtemplet;

                    if (barButtonItem10.Caption != "修改")
                    {//新增
                        LoadTemplet(m_new_Emrtemplet);
                        m_EmrTempletType = EmrTempletType.Templet;
                    }
                    else//进行修改操作
                    {
                        //DataRow obj = (DataRow)((EmrNodeObj)m_treelistnode.Tag).Obj;
                        addnewtemplate.GetDataRow(obj);
                        string m_rname = obj["mr_name"].ToString().Trim('★');//节点名称 

                        //edit by ywk 2012年4月15日13:32:46 mr_name的星号不能被插入数据库                     
                        if (m_new_Emrtemplet.ShowStar)
                        {
                            m_treelistnode.SetValue("NAME", "★" + m_rname);
                            obj["mr_name"] = "★" + m_rname;
                        }
                        else
                        {
                            m_treelistnode.SetValue("NAME", m_rname);
                        }
                        m_SqlManger.UpdateEmrTemplateAttribute(m_new_Emrtemplet);
                        //add by cyq 2013-03-11 更新模板对象
                        m_Emrtemplet = m_new_Emrtemplet;
                        LoadTemplet(m_SqlManger.GetTemplet(m_Emrtemplet.TempletId));
                    }
                    //add by cyq 2013-02-19 重置对象
                    m_new_Emrtemplet = null;
                }

                barButtonItem10.Caption = "新增模板";
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        ///yxy 根据新增模板基本信息条用条件新模板页面（树节点添加时候保存基本信息）toolbar按钮添加模板信息为空
        /// </summary>
        /// <param name="emrtemplet"></param>
        private void AddNewTemplet_Item(Emrtemplet_Item emrtemplet_item)
        {
            try
            {
                //弹出模板标题窗口，参考天鹏界面
                AddNewTemplate_Item addnewtemplate_Item = new AddNewTemplate_Item(emrtemplet_item, m_app);


                if (barButtonItem13.Caption == "修改")
                {
                    DataRow obj = (DataRow)((EmrNodeObj)m_treelistnode_item.Tag).Obj;
                    addnewtemplate_Item.InitValue(obj);
                }
                else//add by cyq 2013-02-26
                {//新增
                    EmrNodeObj eno = (EmrNodeObj)m_treelistnode_item.Tag;
                    string mr_class = eno.Obj.ToString();
                    string dept_id = string.Empty;
                    if (null != m_treelistnode_item && null != m_treelistnode_item.ParentNode && null != m_treelistnode_item.ParentNode.Tag)
                    {
                        //DataRow parentRow = (DataRow)((EmrNodeObj)m_treelistnode_item.ParentNode.Tag).Obj;

                        //此处强转会存在问题的 add by ywk 2013年4月2日16:12:03 应该使用as DataRow 永远不会抛出异常,即如果转换不成功,会返回null
                        DataRow parentRow = ((EmrNodeObj)m_treelistnode_item.ParentNode.Tag).Obj as DataRow;
                        dept_id = (null == parentRow || null == parentRow["DEPT_ID"]) ? "" : parentRow["DEPT_ID"].ToString();
                    }
                    addnewtemplate_Item.SetDefaultValue(mr_class, dept_id);
                }

                addnewtemplate_Item.ShowDialog();
                //如果不是点击确认按钮的则不记录新的模板信息
                if (!addnewtemplate_Item.m_IsOK)
                    m_new_Emrtemplet_Item = new Emrtemplet_Item();
                else
                {
                    m_new_Emrtemplet_Item = addnewtemplate_Item.m_emrtemplet_item;

                    if (barButtonItem13.Caption != "修改")
                    {//新增
                        LoadTemplet_Item(m_new_Emrtemplet_Item);
                        m_EmrTempletType = EmrTempletType.Templet_Item;
                    }
                    else
                    {//修改
                        DataRow obj = (DataRow)((EmrNodeObj)m_treelistnode_item.Tag).Obj;
                        addnewtemplate_Item.GetDataRow(obj);
                        m_treelistnode_item.SetValue("NAME", obj["mr_name"].ToString());
                        m_SqlManger.UpdateEmrTemplateItemAttribute(m_new_Emrtemplet_Item.MrCode, m_new_Emrtemplet_Item.MrName);
                    }
                    //add by cyq 2013-02-19 重置对象
                    m_new_Emrtemplet_Item = null;
                }
                barButtonItem13.Caption = "新增子模板";
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 保存模板信息
        /// </summary>
        private void UpdateTemplet()
        {
            try
            {
                //保存相关内容
                if (m_Emrtemplet == null)
                    return;

                XmlDocument content = new XmlDocument();

                pnlText.EMRDoc.ToXMLDocument(content);
                m_Emrtemplet.XML_DOC_NEW = content.OuterXml;
                m_Emrtemplet.LastTime = DateTime.Now.ToShortDateString() + ' ' + DateTime.Now.ToLongTimeString();
                string editType = "";
                //无templetID代表为新增模板
                if (m_Emrtemplet.TempletId == "")
                {
                    editType = "1";
                    m_Emrtemplet.CreatorId = m_app.User.Id;
                    //m_Emrtemplet.CreatorId =  m_app.User.Id;
                }
                else
                {
                    editType = "2";
                }
                //判断状态
                GetTempletState(m_Emrtemplet);

                string templetid = m_SqlManger.SaveTemplet(m_Emrtemplet, editType);
                m_SqlManger.SaveTemplateContent(templetid, m_Emrtemplet, editType);//保存病历部分单独提取出来进行处理 Add by wwj 2012-06-20

                pnlText.EMRDoc.Modified = false;
                m_app.CustomMessageBox.MessageShow("保存成功");

                if (editType == "1")
                {
                    DataRow dt_row = m_SqlManger.GetTemplet_Table(templetid).Rows[0];

                    TreeListNode PatNode = treeList_Template.FindNodeByFieldValue("KeyID", dt_row["Mr_Class"].ToString() + dt_row["Dept_Id"].ToString());
                    TreeListNode leafNode = treeList_Template.AppendNode(new object[] { dt_row["mr_name"].ToString(), dt_row["Templet_ID"].ToString() }, PatNode);
                    leafNode.Tag = new EmrNodeObj(dt_row, EmrNodeType.Leaf);

                    treeList_Template.FocusedNode = leafNode;

                    LoadTemplet(m_SqlManger.GetTemplet(templetid));
                }
                else
                {
                    //如果是修改刷新模版
                    RefreshTreeListNode(m_Emrtemplet.TempletId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdatePersonTemplet()
        {
            try
            {
                //保存相关内容
                if (m_Emrtemplet == null)
                    return;

                XmlDocument content = new XmlDocument();

                pnlText.EMRDoc.ToXMLDocument(content);
                m_Emrtemplet.XML_DOC_NEW = content.OuterXml;
                m_Emrtemplet.LastTime = DateTime.Now.ToShortDateString() + ' ' + DateTime.Now.ToLongTimeString();

                SaveMyTemplate(m_Emrtemplet, "2");

                pnlText.EMRDoc.Modified = false;
                m_app.CustomMessageBox.MessageShow("保存成功");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存子模板信息
        /// </summary>
        private void UpdateTemplet_Item()
        {
            try
            {
                //保存相关内容
                if (m_Emrtemplet_Item == null)
                    return;

                XmlDocument content = new XmlDocument();

                pnlText.EMRDoc.ToXMLDocument(content);
                m_Emrtemplet_Item.ITEM_DOC_NEW = content.OuterXml;
                m_Emrtemplet_Item.LastTime = DateTime.Now.ToShortDateString() + ' ' + DateTime.Now.ToLongTimeString();
                string editType = "";
                //无templetID代表为新增模板
                if (m_Emrtemplet_Item.MrCode == "")
                {
                    editType = "1";
                    m_Emrtemplet_Item.CreatorId = m_app.User.Id;
                    //m_Emrtemplet.CreatorId =  m_app.User.Id;
                }
                else
                {
                    editType = "2";
                }
                string mr_code = m_SqlManger.SaveTemplet_Item(m_Emrtemplet_Item, editType);

                pnlText.EMRDoc.Modified = false;
                m_app.CustomMessageBox.MessageShow("保存成功");
                //添加节点
                if (editType == "1")
                {
                    DataRow dt_row = m_SqlManger.GetTemplet_Item_Table(mr_code).Rows[0];

                    TreeListNode PatNode = treeList_Items.FindNodeByFieldValue("KeyID", dt_row["mr_class"].ToString() + dt_row["dept_id"].ToString());
                    TreeListNode leafNode = treeList_Items.AppendNode(new object[] { dt_row["mr_name"].ToString() }, PatNode);
                    leafNode.Tag = new EmrNodeObj(dt_row, EmrNodeType.Leaf);

                    treeList_Items.FocusedNode = leafNode;
                }
                LoadTemplet_Item(m_SqlManger.GetTemplet_Item(mr_code));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存模板页眉信息
        /// </summary>
        private void UpdateTemplet_Header()
        {
            //保存相关内容
            if (m_EmrTempletHeader == null)
                m_EmrTempletHeader = new EmrTempletHeader();

            XmlDocument content = new XmlDocument();
            pnlText.EMRDoc.ToXMLDocument(content);
            m_EmrTempletHeader.Content = content.OuterXml;

            AddNewTemplet_Header addtemplet_header = new AddNewTemplet_Header(m_app);
            addtemplet_header.m_EmrTempletHeader = m_EmrTempletHeader;
            addtemplet_header.m_EmrTempletType = EmrTempletType.Templet_Header;
            addtemplet_header.ShowDialog();
            if (addtemplet_header.IsSave)
            {
                pnlText.EMRDoc.Modified = false;
                m_EmrTempletHeader = addtemplet_header.m_EmrTempletHeader;

                this.pnlText.EMRDoc.ShowHeader = true;
                this.pnlText.EMRDoc.ShowFooter = true;

                this.pnlText.EMRDoc.ClearContent();
                m_EmrTempletType = EmrTempletType.None;
            }

        }

        /// <summary>
        /// 保存模板页脚信息
        /// </summary>
        private void UpdateTemplet_Foot()
        {
            try
            {
                //保存相关内容
                if (m_EmrTemplet_Foot == null)
                    m_EmrTemplet_Foot = new EmrTemplet_Foot();

                XmlDocument content = new XmlDocument();
                pnlText.EMRDoc.ToXMLDocument(content);
                m_EmrTemplet_Foot.Content = content.OuterXml;

                AddNewTemplet_Header addtemplet_header = new AddNewTemplet_Header(m_app);
                addtemplet_header.m_EmrTemplet_Foot = m_EmrTemplet_Foot;
                addtemplet_header.m_EmrTempletType = EmrTempletType.Templet_Foot;
                addtemplet_header.ShowDialog();
                if (addtemplet_header.IsSave)
                {
                    pnlText.EMRDoc.Modified = false;
                    m_EmrTemplet_Foot = addtemplet_header.m_EmrTemplet_Foot;

                    this.pnlText.EMRDoc.ShowHeader = true;
                    this.pnlText.EMRDoc.ShowFooter = true;

                    this.pnlText.EMRDoc.ClearContent();
                    m_EmrTempletType = EmrTempletType.None;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 删除模板
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// 2、优化提示
        private void DeleteTemplet()
        {
            try
            {
                if (m_treelistnode == null)
                {
                    return;
                }
                if (m_Emrtemplet == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您尚未加载模板，无法删除。");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(string.Format("您确定要删除模板 " + m_Emrtemplet.MrName + " 吗？"), "删除模板", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(m_Emrtemplet.TempletId))
                    {//新增
                        m_Emrtemplet = null;
                        this.pnlText.EMRDoc.ClearContent();
                        pnlText.EMRDoc.Modified = false;
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");
                    }
                    //进行删除操作
                    else if (m_SqlManger.DelTemplet(m_Emrtemplet, IsPerson))
                    {
                        //删除对应节点
                        if (IsPerson)
                        {
                            this.treeListMy.Nodes.Remove(m_treelistnode);
                        }
                        else
                        {
                            this.treeList_Template.Nodes.Remove(m_treelistnode);
                        }
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");

                        m_Emrtemplet = null;
                        this.pnlText.EMRDoc.ClearContent();
                    }
                    this.Text = "模型维护工厂";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除子模板
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// 2、优化提示
        private void DeleteTemplet_Item()
        {
            try
            {
                if (m_treelistnode_item == null)
                {
                    return;
                }
                if (m_Emrtemplet_Item == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您尚未加载子模板，无法删除。");
                    return;
                }
                if (m_app.CustomMessageBox.MessageShow(string.Format("您确定要删除子模板 " + m_Emrtemplet_Item.MrName + " 吗？"), CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                {
                    if (m_SqlManger.DelTemplet_Item(m_Emrtemplet_Item))
                    {
                        //删除对应节点
                        this.treeList_Items.Nodes.Remove(m_treelistnode_item);
                        m_app.CustomMessageBox.MessageShow("删除成功");

                        m_Emrtemplet_Item = null;
                        this.pnlText.EMRDoc.ClearContent();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除模板页眉文件
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// 2、优化提示
        private void DeleteTemplet_Header()
        {
            try
            {
                if (m_EmrTempletHeader == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您尚未加载页眉文件，无法删除。");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(string.Format("您确定要删除页眉文件 " + m_EmrTempletHeader.Name + " 吗？"), "删除页眉", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    if (m_SqlManger.DelTempletHeader(m_EmrTempletHeader))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");

                        m_EmrTempletHeader = null;
                        this.pnlText.EMRDoc.ClearContent();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除页脚文件
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// 2、优化提示
        private void DeleteTemplet_Foot()
        {
            try
            {
                if (m_newEmrTemplet_foot == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您尚未加载页脚文件，无法删除。");
                    return;
                }
                if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(string.Format("您确定要删除页脚文件 " + m_newEmrTemplet_foot.Name + " 吗？"), "删除页脚", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.OkCancel) == DialogResult.OK)
                {
                    if (m_SqlManger.DelTempletFoot(m_newEmrTemplet_foot))
                    {
                        DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("删除成功");

                        m_newEmrTemplet_foot = null;
                        this.pnlText.EMRDoc.ClearContent();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region toolbox
        private ImageList imageList;
        private ImageList headerImageList;
        void InitializeImageLists()
        {
            try
            {
                imageList = new ImageList();
                imageList.ImageSize = new Size(16, 16);
                Bitmap icons = //(Bitmap)rmListImages.GetObject("DrectSoft.Emr.TemplateFactory.ListIcons");
                IconImages.ListIcons;
                icons.MakeTransparent(Color.FromArgb(192, 192, 192));
                imageList.Images.AddStrip(icons);

                headerImageList = new ImageList();
                headerImageList.ImageSize = new Size(16, 16);
                icons = //(Bitmap)rmListImages.GetObject("HeaderIcons");
                    IconImages.HeaderIcons;
                icons.MakeTransparent(Color.FromArgb(0, 255, 255));
                headerImageList.Images.AddStrip(icons);

                #region 已注释
                //this.imageListBoxControl1.ImageList = imageList;
                //outlookLargeIcons = new ImageList();
                //outlookLargeIcons.ImageSize = new Size(32, 32);
                //icons = //(Bitmap)rmListImages.GetObject("OutlookLargeIcons");
                //    IconImages.OutlookLargeIcons;
                //icons.MakeTransparent(Color.FromArgb(255, 0, 255));
                //outlookLargeIcons.Images.AddStrip(icons);

                //outlookSmallIcons = new ImageList();
                //outlookSmallIcons.ImageSize = new Size(16, 16);
                //icons = //(Bitmap)rmListImages.GetObject("OutlookSmallIcons");
                //    IconImages.OutlookSmallIcons;
                //icons.MakeTransparent(Color.FromArgb(255, 0, 255));
                //outlookSmallIcons.Images.AddStrip(icons);

                //myShortcutsLargeIcons = new ImageList();
                //myShortcutsLargeIcons.ColorDepth = ColorDepth.Depth32Bit;
                //myShortcutsLargeIcons.ImageSize = new Size(48, 48);
                //icons = //(Bitmap)rmListImages.GetObject("MyShortcutsLargeIcons");
                //    IconImages.MyShortcutsLargeIcons;
                //icons.MakeTransparent(Color.FromArgb(255, 0, 255));
                //myShortcutsLargeIcons.Images.AddStrip(icons);

                //myShortcutsSmallIcons = new ImageList();
                //myShortcutsSmallIcons.ImageSize = new Size(16, 16);
                //icons = //(Bitmap)rmListImages.GetObject("MyShortcutsSmallIcons");
                //    IconImages.MyShortcutsSmallIcons;
                //icons.MakeTransparent(Color.FromArgb(255, 0, 255));
                //myShortcutsSmallIcons.Images.AddStrip(icons);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 复用项目
        /// </summary>
        private void InitItemTree()
        {
            try
            {
                DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem item;
                TreeListNode node;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("单选", 0, ElementType.SingleElement);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("多选", 1, ElementType.MultiElement);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("有无选", 2, ElementType.HaveNotElement);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("复选框", 1, ElementType.CheckBox);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("标签", 3, ElementType.FixedText);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("只读文本", 3, ElementType.Text);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("定位符", 3, ElementType.Flag);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("按钮", 3, ElementType.Button);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("录入提示", 3, ElementType.PromptText);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("格式化数字", 4, ElementType.FormatNumber);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;


                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("格式化字符串", 5, ElementType.FormatString);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("格式化日期时间", 5, ElementType.FormatDatetime);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("月经史公式", 6, ElementType.MensesFormula);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;
                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("水平线", 2, ElementType.HorizontalLine);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;
                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("分页符", 2, ElementType.PageEnd);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;

                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("字典", 0, ElementType.LookUpEditor);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;


                //新增牙齿检查，将它作为元素，然后可以做进子模板中去  add by ywk 2012年11月27日16:00:56 
                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("牙齿检查", 6, ElementType.ToothCheck);
                node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;


                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("斜线", 0, ElementType.Vlie);
                //node = treeList_Basic.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 基本项目
        /// </summary>
        private void InitBasicTree()
        {
            try
            {
                DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem item;
                TreeListNode node;

                //获得当前病历需要替换的项目需要从哪些病历中提取
                DataTable dtReplaceItem = m_SqlManger.GetReplaceItem();
                //List<string> sourceEmrList = m_RecordDal.GetSourceEMRByDestEmrName(model.ModelName, dtReplaceItem);
                foreach (DataRow dr in dtReplaceItem.Rows)
                {
                    item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem(dr["NAME"].ToString(), 0, ElementType.Replace);
                    node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                    node.Tag = item;
                }

                #region 已注释
                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("主诉", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("症状", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("体征", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("初步诊断", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("入院诊断", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("最后诊断", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("病历特点", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("病历摘要", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("辅助检查", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("专科情况", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("现病史", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("既往史", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("手术外伤史", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;
                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("输血史", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;
                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("过敏史", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;
                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("个人史", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;
                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("月经史", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;
                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("家族史", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("体格检查", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;
                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("专科情况(体检)", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;

                //item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("门诊及院外重要辅助检查结果", 0, ElementType.Replace);
                //node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                //node.Tag = item;
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitWordBooks()
        {
            try
            {
                DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem item;
                TreeListNode node;
                item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem("用户", 0, ElementType.LookUpEditor);
                node = treeList_Conform.AppendNode(new object[] { item.Text }, null);
                node.Tag = item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void InitSubItemTree(string deptid)
        {
            try
            {
                if ((treeList_Sub.Tag == null) || (!treeList_Sub.Tag.ToString().Equals(deptid)))
                {
                    treeList_Sub.Tag = deptid;
                    DataTable dt = m_Util.Get_Templet_Item(deptid);
                    treeList_Sub.ClearNodes();
                    DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem item;
                    TreeListNode node;
                    foreach (DataRow dr in dt.Rows)
                    {
                        item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem(dr["MR_NAME"].ToString(), 0, ElementType.Template);
                        node = treeList_Sub.AppendNode(new object[] { item.Text }, null);
                        node.Tag = item;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitMacro()
        {
            try
            {
                Dictionary<string, Macro> list = MacroUtil.MacrosList;

                DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem item;

                TreeListNode node;
                treeList_Pat.BeginUnboundLoad();
                foreach (Macro mac in list.Values)
                {
                    item = new DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem(mac.Name, 0, ElementType.Macro);

                    node = treeList_Pat.AppendNode(new object[] { item.Text }, null);
                    node.Tag = item;
                }
                treeList_Pat.EndUnboundLoad();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void DoDragDorpInfo(TreeList tree)
        {
            try
            {
                if (tree.FocusedNode == null) return;

                if (tree.Name == treeListImage.Name)//插入图片
                {
                    DataRow row = (DataRow)tree.FocusedNode.Tag;

                    byte[] img = m_SqlManger.GetImage(row["ID"].ToString());

                    KeyValuePair<string, object> data = new KeyValuePair<string, object>("ZYTextImage", img);
                    tree.DoDragDrop(data, DragDropEffects.All);
                }
                else if ((tree.FocusedNode != null) && (tree.FocusedNode.Tag != null))
                {


                    DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem item = (DrectSoft.Library.EditorUtility.WinControls.OutlookBarItem)tree.FocusedNode.Tag;

                    KeyValuePair<string, object> data = new KeyValuePair<string, object>(item.Text, item.Tag);
                    tree.DoDragDrop(data, DragDropEffects.All);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void treeList_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hitInfo = ((TreeList)sender).CalcHitInfo(e.Location);
                ((TreeList)sender).FocusedNode = hitInfo.Node;

                TreeList tree = (TreeList)sender;
                DoDragDorpInfo(tree);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void treeList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TreeList tree = (TreeList)sender;
                DoDragDorpInfo(tree);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        #endregion

        #region IStartPlugIn 成员

        public IPlugIn Run(IEmrHost host)
        {
            try
            {
                IPlugIn plg = new PlugIn(this.GetType().ToString(), this);

                m_app = host;
                IsAdministrator = m_app.User.GWCodes.Split(',').Contains("66");
                return plg;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region other

        void padForm_PropertyObjChanged(object sender, EventOjbArgs e)
        {
            try
            {
                this.propertyGridControl1.SelectedObject = e.PropertyObj;
                propertyGridControl1.RetrieveFields();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void propertyGridControl1_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            try
            {
                pnlText.EMRDoc.RefreshSize();
                //mainFrm.editorfrm.pnlText.EMRDoc.RefreshLine();
                pnlText.EMRDoc.ContentChanged();
                //mainFrm.editorfrm.pnlText.EMRDoc.EndUpdate();
                pnlText.Refresh();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }


        #endregion

        #region toolbar事件

        private void btn_Default_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ConfigFrm frm = new ConfigFrm();
                if (frm.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                {
                    // SetEmrDefaultSet(frm.LineSpace);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Merge_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.pnlText.EMRDoc.TableIsCanMerge() == 1)
                {
                    this.pnlText.EMRDoc.TableMergeCell();
                }
                else if (this.pnlText.EMRDoc.TableIsCanMerge() == 2)
                {

                    m_app.CustomMessageBox.MessageShow("此次操作须得在表格中选中单元格才能生效");
                }
                else if (this.pnlText.EMRDoc.TableIsCanMerge() == 3)
                {
                    m_app.CustomMessageBox.MessageShow("您并没有选中任何单元格");
                }
                else if (this.pnlText.EMRDoc.TableIsCanMerge() == 4)
                {
                    m_app.CustomMessageBox.MessageShow("您只选中了一个单元格,无法再次合并");
                }
                else if (this.pnlText.EMRDoc.TableIsCanMerge() == 5)
                {
                    m_app.CustomMessageBox.MessageShow("请检查您选择的单元格是否呈正规矩形");
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// Modify by xlb 2013-04-07
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Split_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Dictionary<string, int> splitDic = this.pnlText.EMRDoc.TableBeforeSplitCell();
                //Add by xlb 2013-04-07
                if (splitDic == null)
                {
                    MessageBox.Show("此操作必须在表格中才能生效");
                    return;
                }
                ApartCell apartCellForm = new ApartCell(splitDic["row"], splitDic["col"]);

                if (apartCellForm.ShowDialog() == DialogResult.OK)
                {
                    this.pnlText.EMRDoc.TableSplitCell(apartCellForm.intRow, apartCellForm.intColumn);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        #region 单元格斜线
        /// <summary>
        /// 单元格斜线（左上->右下）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LeftTop2RightBottom_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string str = this.pnlText.EMRDoc.SetItalicLineInCell(1);
                if (str != string.Empty)
                {
                    m_app.CustomMessageBox.MessageShow(str);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 单元格斜线（右上->左上）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RightTop2LeftBottom_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string str = this.pnlText.EMRDoc.SetItalicLineInCell(2);
                if (str != string.Empty)
                {
                    m_app.CustomMessageBox.MessageShow(str);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 取消单元格斜线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ItalicNoLine_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string str = this.pnlText.EMRDoc.SetItalicLineInCell(0);
                if (str != string.Empty)
                {
                    m_app.CustomMessageBox.MessageShow(str);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 单元格斜线（左上->右下）(两条线)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LeftTop2RightBottom2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string str = this.pnlText.EMRDoc.SetItalicLineInCell(3);
                if (str != string.Empty)
                {
                    m_app.CustomMessageBox.MessageShow(str);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 单元格斜线（左上(两条线)->右下）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LeftTop2RightBottom4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string str = this.pnlText.EMRDoc.SetItalicLineInCell(4);
                if (str != string.Empty)
                {
                    m_app.CustomMessageBox.MessageShow(str);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        #endregion

        #region 单元格属性设置
        private void btn_CellBorderWidth_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TPTextCell cell = this.pnlText.EMRDoc.GetCurrentCell();
                if (cell != null)
                {
                    TableCellSetting cellSetting = new TableCellSetting(cell, null);
                    cellSetting.StartPosition = FormStartPosition.CenterScreen;
                    cellSetting.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        #endregion

        #region 单元格中控制
        private void barButtonItemCanEnter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.pnlText.EMRDoc.SetTableCellCanInsertEnter(true);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void barButtonItemCanNotEnter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.pnlText.EMRDoc.SetTableCellCanInsertEnter(false);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        #endregion

        #region 页眉页脚设置
        private void btn_HeaderFooterSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                HeaderFooterSetting set = new HeaderFooterSetting(this.pnlText.EMRDoc, m_Util);
                set.StartPosition = FormStartPosition.CenterScreen;
                set.ShowDialog();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }
        #endregion

        void ReSetSupSub()
        {
            try
            {
                this.pnlText.EMRDoc.SetSelectionSup(false);
                this.pnlText.EMRDoc.SetSelectionSub(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Sump_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.btn_Sumb.Down = this.btn_Sumb.Down ? true : false;
                this.btn_Sump.Down = this.btn_Sump.Down ? true : false;
                ReSetSupSub();
                this.pnlText.EMRDoc.SetSelectionSup(btn_Sump.Down);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Sumb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.btn_Sump.Down = this.btn_Sump.Down ? true : false;
                this.btn_Sumb.Down = this.btn_Sumb.Down ? true : false;
                ReSetSupSub();
                this.pnlText.EMRDoc.SetSelectionSub(btn_Sumb.Down);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Undline_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.btn_Undline.Down = this.btn_Undline.Down ? true : false;
                this.pnlText.EMRDoc.SetSelectionUnderLine(this.btn_Undline.Down);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Italy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.btn_Italy.Down = this.btn_Italy.Down ? true : false;
                this.pnlText.EMRDoc.SetSelectionFontItalic(this.btn_Italy.Down);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Bold_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ReSetSupSub();
                //确定按钮的状态
                this.btn_Bold.Down = this.btn_Bold.Down ? true : false;
                this.pnlText.EMRDoc.SetSelectionFontBold(this.btn_Bold.Down);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Redo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.pnlText != null)
                {
                    this.pnlText.EMRDoc._Redo();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Undo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (this.pnlText != null)
                {
                    this.pnlText.EMRDoc._Undo();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 查找替换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Find_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SearchFrm searchfrm = new SearchFrm(this.pnlText);
                searchfrm.Show(this);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Paste_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (null == EmrClipboard.Data && string.IsNullOrEmpty(EmrClipboard.PatientID))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("剪切板无内容");
                    return;
                }
                this.pnlText.EMRDoc._Paste();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Cut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.pnlText.EMRDoc._Cut();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Copy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.pnlText.EMRDoc._Copy();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_Open_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog dlg = new OpenFileDialog())
                {
                    dlg.CheckFileExists = true;
                    //dlg.Filter = "xml文件|*.xml|模板文件|*.t";
                    if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        FileInfo fi = new FileInfo(dlg.FileName);
                        if (fi.Extension == ".t")
                        {

                            this.pnlText.EMRDoc.ShowHeader = false;
                            this.pnlText.EMRDoc.ShowFooter = false;
                            this.pnlText.EMRDoc.HeadString = "";
                            this.pnlText.EMRDoc.FooterString = "";
                        }
                        else
                        {

                            this.pnlText.EMRDoc.ShowHeader = true;
                            this.pnlText.EMRDoc.ShowFooter = true;
                        }

                        System.IO.FileStream fs = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.Open);
                        byte[] data = new byte[fs.Length];
                        fs.Read(data, 0, (int)fs.Length);
                        fs.Close();
                        fs.Dispose();

                        if (this.pnlText.LoadBinary(data))
                        {
                            this.Text = "模型维护工厂" + " --- " + fi.Name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 新增病例模板事件
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddNewDoc_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                AddNewTemplet(null);//新增病历模板
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增页眉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddHeader_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadBeforeSave();
                this.pnlText.EMRDoc.ClearContent();

                //隐藏页眉页脚
                //this.视图ToolStripMenuItem.Enabled = false;
                this.pnlText.EMRDoc.ShowHeader = false;
                this.pnlText.EMRDoc.ShowFooter = false;
                this.pnlText.EMRDoc.HeadString = "";
                this.pnlText.EMRDoc.FooterString = "";

                m_EmrTempletHeader = new EmrTempletHeader();

                m_EmrTempletType = EmrTempletType.Templet_Header;
                //this.editorfrm.TabText = "新建页眉模板.t";
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_InsertTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TableInsert tableInsertForm = new TableInsert();
                if (tableInsertForm.ShowDialog() == DialogResult.OK)
                {
                    this.pnlText.EMRDoc.TableInsert(tableInsertForm.Header,
                        tableInsertForm.Rows, tableInsertForm.Columns,
                        tableInsertForm.ColumnWidth);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_InsertRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                RowsInsert rowsInsertForm = new RowsInsert();
                if (rowsInsertForm.ShowDialog() == DialogResult.OK)
                {
                    this.pnlText.EMRDoc.RowInsert(rowsInsertForm.RowNum, rowsInsertForm.IsBack);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_InsertCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ColumnsInsert columnsInsertForm = new ColumnsInsert();
                if (columnsInsertForm.ShowDialog() == DialogResult.OK)
                {
                    this.pnlText.EMRDoc.ColumnInsert(columnsInsertForm.ColumnNum, columnsInsertForm.IsBack);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_DelteTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.pnlText.EMRDoc.TableDelete();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_DeleteRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.pnlText.EMRDoc.TableDeleteRow();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_DeleteCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.pnlText.EMRDoc.TableDeleteCol();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_TableProperty_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TPTextTable table = this.pnlText.EMRDoc.GetCurrentTable();
                TPTextCell cell = this.pnlText.EMRDoc.GetCurrentCell();
                if (table != null && cell != null)
                {
                    TableProperty tablePropertyForm = new TableProperty(table, cell);
                    if (tablePropertyForm.ShowDialog() == DialogResult.OK)
                    {

                        this.pnlText.EMRDoc.ContentChanged();
                        this.pnlText.EMRDoc.Refresh();
                        //this.pnlText.EMRDoc.SetTableProperty(tablePropertyForm.Table);
                    }
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("光标必须在表格中才能更改表格属性");
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_InsertImage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                        //this.pnlText.EMRDoc._InsertImage(dlgFile.FileName);
                        Image img = Image.FromFile(dlgFile.FileName);

                        MemoryStream ms = new MemoryStream();
                        byte[] imagedata = null;
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                        imagedata = ms.GetBuffer();

                        this.pnlText.EMRDoc._InsertImage(imagedata);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// 2、优化提示
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_EmrTempletType == EmrTempletType.Templet)
                {//模板
                    if (IsPerson)
                    {
                        UpdatePersonTemplet();
                    }
                    else
                    {
                        UpdateTemplet();
                    }
                    RefreshTreeListNode(m_Emrtemplet.TempletId);
                }
                else if (m_EmrTempletType == EmrTempletType.Templet_Item)
                {//子模板
                    UpdateTemplet_Item();
                }
                //yxy
                else if (m_EmrTempletType == EmrTempletType.Templet_Header)
                {//页眉
                    UpdateTemplet_Header();
                }
                else if (m_EmrTempletType == EmrTempletType.Templet_Foot)
                {//页脚
                    UpdateTemplet_Foot();
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您尚未加载任何内容，无法保存。");
                    return;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_SaveAS_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (System.Windows.Forms.SaveFileDialog dlg = new SaveFileDialog())
                {
                    string filename = this.pnlText.Text;
                    if (filename.Substring(filename.IndexOf('.') + 1) == "t")
                    {
                        dlg.Filter = "页眉模板|*.t";
                    }
                    else //(filename.Substring(filename.IndexOf('.') + 1) == "xml")
                    {
                        dlg.Filter = "电子病历文件|*.b";
                    }

                    if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        byte[] data = this.pnlText.SaveBinary();
                        System.IO.FileStream fs = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.Create);
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 模板标签页点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelContainer1_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeList_Template.Focused && null != m_Emrtemplet)
                {//模板列表
                    SetTempletAuditState(m_Emrtemplet);
                }
                else
                {
                    SetAuditBtnState(false);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 删除模板信息
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-26
        /// 1、add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_EmrTempletType == EmrTempletType.Templet)
                {
                    DeleteTemplet();
                    SetTempletAuditState(m_Emrtemplet);
                }
                else if (m_EmrTempletType == EmrTempletType.Templet_Item)
                {
                    DeleteTemplet_Item();
                }
                else if (m_EmrTempletType == EmrTempletType.Templet_Header)
                {
                    DeleteTemplet_Header();
                }
                else if (m_EmrTempletType == EmrTempletType.Templet_Foot)
                {
                    DeleteTemplet_Foot();
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您尚未加载模板，无法删除。");
                    return;
                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }


        private void btn_left_DownChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!this.btn_left.Down)//取消左对齐
                {
                    this.pnlText.EMRDoc.SetAlign(ParagraphAlignConst.Left);
                    return;
                }
                this.pnlText.EMRDoc.SetAlign(ParagraphAlignConst.Left);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_center_DownChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!this.btn_center.Down)//取消居中对齐
                {
                    this.pnlText.EMRDoc.SetAlign(ParagraphAlignConst.Left);
                    return;
                }
                this.pnlText.EMRDoc.SetAlign(ParagraphAlignConst.Center);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_right_DownChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!this.btn_right.Down)//取消右对齐
                {
                    this.pnlText.EMRDoc.SetAlign(ParagraphAlignConst.Left);
                    return;
                }
                this.pnlText.EMRDoc.SetAlign(ParagraphAlignConst.Right);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_disperse_DownChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (!this.btn_disperse.Down)//取消分散对齐
                {
                    this.pnlText.EMRDoc.SetAlign(ParagraphAlignConst.Left);
                    return;
                }
                this.pnlText.EMRDoc.SetAlign(ParagraphAlignConst.Justify);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }


        /// <summary>
        /// 调用批量删除模板日志信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ReplaceSaveLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                RePlaceTempletSaveLog m_RePlaceTempletSaveLog = new RePlaceTempletSaveLog(m_app);
                m_RePlaceTempletSaveLog.Text = "批量删除模板日志信息";
                m_RePlaceTempletSaveLog.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 基础数据维护 Add wwj 2011-11-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemBaseData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                BaseDataMaintain.BaseDataMaintain baseDataMaintain = new BaseDataMaintain.BaseDataMaintain(m_app);
                baseDataMaintain.StartPosition = FormStartPosition.CenterScreen;
                baseDataMaintain.ShowDialog();
                //编辑操作后进行刷新数据 add by ywk  2012年3月30日16:13:07 
                if (baseDataMaintain.IsClose)
                {
                    //treeList_Conform.Refresh();
                    treeList_Conform.Nodes.Clear();
                    InitBasicTree();//重新刷新复用项目
                    //重新刷新基本信息 
                    treeList_Pat.Nodes.Clear();
                    InitMacro();
                    //重新刷新特殊符号
                    DataTable mysym = m_app.SqlHelper.ExecuteDataTable("select  SYMBOL as 名称 from DICT_SYMBOL");
                    imageListBoxControlSymbol.Items.Clear();
                    BindSymbol(mysym);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_FontName_EditValueChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (btn_FontName.EditValue == null) return;
                this.pnlText.EMRDoc.SetSelectioinFontName(this.btn_FontName.EditValue.ToString());
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        private void btn_FontName_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (btn_FontName.EditValue == null) return;
                this.pnlText.EMRDoc.SetSelectioinFontName(this.btn_FontName.EditValue.ToString());
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_FontSize_EditValueChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (btn_FontSize.EditValue == null) return;

                float size = FontCommon.GetFontSizeByName(btn_FontSize.EditValue.ToString());
                this.pnlText.EMRDoc.SetSelectionFontSize(size);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        private void btn_FontSize_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (btn_FontSize.EditValue == null) return;

                float size = FontCommon.GetFontSizeByName(btn_FontSize.EditValue.ToString());
                this.pnlText.EMRDoc.SetSelectionFontSize(size);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }


        /// <summary>
        /// 调用页脚管理页眉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Templet_FootManger m_Templet_FootManger = new Templet_FootManger(m_app);
                if (m_Templet_FootManger.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //加载页脚到页眉
                    if (!m_Templet_FootManger.IsEdit)
                    {
                        m_EmrTemplet_Foot = m_Templet_FootManger.m_emrtemplet_foot;

                        XmlDocument content = new XmlDocument();
                        content.PreserveWhitespace = true;
                        content.LoadXml(m_EmrTemplet_Foot.Content);
                        //this.pnlText.EMRDoc.ClearContent();
                        pnlText.EMRDoc.ShowHeader = true;
                        pnlText.EMRDoc.ShowFooter = true;
                        this.pnlText.EMRDoc.FootDocument = content;
                    }
                    //修改页脚
                    else if (m_Templet_FootManger.IsEdit == true)
                    {
                        m_EmrTemplet_Foot = m_Templet_FootManger.m_emrtemplet_foot;

                        LoadTemplet_Foot(m_EmrTemplet_Foot);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 调用批量替换页眉页脚功能
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-22 add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Replace_Title_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                RePlaceTempletTitle m_RePlaceTempletTitle = new RePlaceTempletTitle(m_app);
                m_RePlaceTempletTitle.ShowDialog();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FontColor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (ColorDialog cdia = new ColorDialog())
                {
                    if (cdia.ShowDialog() == DialogResult.OK)
                    {
                        this.pnlText.EMRDoc.SetSelectionColor(cdia.Color);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        private void btn_ClearClipboard_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                EmrClipboard.Data = null;
                EmrClipboard.PatientID = null;
                Clipboard.SetData(System.Windows.Forms.DataFormats.UnicodeText, null);
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("清空成功");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        #endregion

        #region tree
        //更改科室下拉框数据事件
        private void lookUpEditorDeptTem_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_deptId = this.lookUpEditorDeptTem.CodeValue;
                m_ccode = this.lookUpEditorTemNode.CodeValue;
                this.lookUpEditorTem.Text = "";
                if (!string.IsNullOrEmpty(m_deptId))
                {
                    //m_Util.ModelList.DefaultView.RowFilter = "DEPT_ID='" + m_deptId + "' and MR_CLASS='" + m_ccode + "'" + " and Valid=1 ";
                    //m_dtTem = m_Util.ModelList.DefaultView.ToTable();

                    //edit by cyq 2013-02-20
                    var rows = templeteTable.Select("DEPT_ID='" + m_deptId + "' " + " and Valid=1 " + " and MR_CLASS='" + m_ccode + "'");
                    m_dtTem = (null == rows || rows.Count() == 0) ? templeteTable.Clone() : rows.CopyToDataTable();
                    //m_Util.SimpleModelList.DefaultView.RowFilter = "DEPT_ID='" + m_deptId + "' " + " and Valid=1 " + " and MR_CLASS='" + m_ccode + "'";
                    //m_dtTem = m_Util.SimpleModelList.DefaultView.ToTable();
                    if (m_dtTem.Rows.Count == 0)
                    {
                        this.lookUpEditorTem.CodeValue = string.Empty;
                        this.lookUpEditorTem.Enabled = false;
                        //return;
                    }
                    else
                    {
                        this.lookUpEditorTem.Enabled = true;
                    }
                    m_dtTem.Columns["TEMPLETE_ID"].Caption = "编号";
                    m_dtTem.Columns["ORDERNAME"].Caption = "模板名称";

                    Dictionary<string, int> colWidth = new Dictionary<string, int>();
                    colWidth.Add("TEMPLETE_ID", 60);
                    colWidth.Add("ORDERNAME", 140);
                    DataTable templeteData = m_dtTem.Copy();
                    GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
                    shortCode.AutoAddShortCode(templeteData, "ORDERNAME");
                    SqlWordbook temWordBook = new SqlWordbook("Templetes", templeteData, "TEMPLETE_ID", "ORDERNAME", colWidth, "TEMPLETE_ID//ORDERNAME//PY//WB");

                    this.lookUpEditorTem.SqlWordbook = temWordBook;
                }
                else
                {
                    this.lookUpEditorTemNode.CodeValue = string.Empty;
                    this.lookUpEditorTem.CodeValue = string.Empty;
                    this.lookUpEditorTem.Enabled = false;
                }
                SetFocusedNode(m_deptId, m_ccode, this.lookUpEditorTem.CodeValue, treeList_Template.Nodes);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        //更改子节点下拉框数据事件
        private void lookUpEditorTemNode_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {

                m_deptId = this.lookUpEditorDeptTem.CodeValue;
                m_ccode = this.lookUpEditorTemNode.CodeValue;
                this.lookUpEditorTem.Text = "";
                if (!string.IsNullOrEmpty(m_deptId) && !string.IsNullOrEmpty(m_ccode))
                {
                    //m_Util.ModelList.DefaultView.RowFilter = "DEPT_ID='" + m_deptId + "' and MR_CLASS='" + m_ccode + "'" + " and Valid=1 ";
                    //m_dtTem = m_Util.ModelList.DefaultView.ToTable();
                    //DataView dv = new DataView();
                    //dv.Table = m_Util.SimpleModelList;
                    //dv.RowFilter = "DEPT_ID='" + m_deptId + "' and MR_CLASS='" + m_ccode + "'" + " and Valid=1 ";
                    //m_dtTem = dv.ToTable();

                    //edit by cyq 2013-02-20
                    var rows = templeteTable.Select("DEPT_ID='" + m_deptId + "' and MR_CLASS='" + m_ccode + "'" + " and Valid=1 ");
                    m_dtTem = (null == rows || rows.Count() == 0) ? templeteTable.Clone() : rows.CopyToDataTable();
                    //m_Util.SimpleModelList.DefaultView.RowFilter = "DEPT_ID='" + m_deptId + "' and MR_CLASS='" + m_ccode + "'" + " and Valid=1 ";
                    //m_dtTem = m_Util.SimpleModelList.DefaultView.ToTable();
                    if (m_dtTem.Rows.Count == 0)
                    {
                        this.lookUpEditorTem.Enabled = false;
                        //return;
                    }
                    else
                    {
                        this.lookUpEditorTem.Enabled = true;
                    }
                    m_dtTem.Columns["TEMPLETE_ID"].Caption = "编号";
                    m_dtTem.Columns["ORDERNAME"].Caption = "模板名称";

                    Dictionary<string, int> colWidth = new Dictionary<string, int>();
                    colWidth.Add("TEMPLETE_ID", 60);
                    colWidth.Add("ORDERNAME", 140);
                    DataTable templeteData = m_dtTem.Copy();
                    GenerateShortCode shortCode = new GenerateShortCode(m_app.SqlHelper);
                    shortCode.AutoAddShortCode(templeteData, "ORDERNAME");
                    SqlWordbook temWordBook = new SqlWordbook("Templetes", templeteData, "TEMPLETE_ID", "ORDERNAME", colWidth, "TEMPLETE_ID//ORDERNAME//PY//WB");

                    this.lookUpEditorTem.SqlWordbook = temWordBook;
                }
                else
                {
                    this.lookUpEditorTem.CodeValue = string.Empty;
                    this.lookUpEditorTem.Enabled = false;
                }
                SetFocusedNode(m_deptId, m_ccode, this.lookUpEditorTem.CodeValue, treeList_Template.Nodes);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        //更改模板下拉框数据事件
        private void lookUpEditorTem_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_deptId = this.lookUpEditorDeptTem.CodeValue;
                m_ccode = this.lookUpEditorTemNode.CodeValue;
                string templet_id = this.lookUpEditorTem.CodeValue;

                if (string.IsNullOrEmpty(templet_id))
                {
                    return;
                }
                //add by cyq 2013-02-20
                SetFocusedNode(m_deptId, m_ccode, templet_id, treeList_Template.Nodes);
                string stateid = "";
                if (null != treeList_Template.FocusedNode && null != treeList_Template.FocusedNode.Tag)
                {
                    DataRow drow = (DataRow)((EmrNodeObj)treeList_Template.FocusedNode.Tag).Obj;
                    stateid = null == drow["State"] ? "" : drow["State"].ToString(); //读取模板节点数据中的模板状态
                }

                #region 方法封装 edit by cyq 2013-02-20
                //foreach (TreeListNode node in treeList_Template.Nodes)
                //{
                //    DataRow obj = (DataRow)((EmrNodeObj)node.Tag).Obj;
                //    if (obj[0].ToString() == m_deptId)
                //    {
                //        foreach (TreeListNode node2 in node.Nodes)
                //        {
                //            DataRow obj2 = (DataRow)((EmrNodeObj)node2.Tag).Obj;
                //            {
                //                if (obj2[0].ToString() == m_ccode)
                //                {
                //                    foreach (TreeListNode node3 in node2.Nodes)
                //                    {
                //                        DataRow obj3 = (DataRow)((EmrNodeObj)node3.Tag).Obj;
                //                        {
                //                            if (obj3[0].ToString() == templet_id)
                //                            {
                //                                treeList_Template.FocusedNode = node3;
                //                                stateid = obj3["State"].ToString(); //读取模板节点数据中的模板状态
                //                                break;
                //                            }
                //                        }
                //                    }
                //                    break;
                //                }
                //            }

                //        }
                //        break;
                //    }
                //}
                #endregion

                LoadTemplet(m_SqlManger.GetTemplet(templet_id));
                m_EmrTempletType = EmrTempletType.Templet;
                label1.Visible = true;
                pictureBox1.Visible = true;
                if (stateid == "0")//保存未提交
                {
                    pictureBox1.Image = imageCollection_Tree.Images[0];
                    label1.Text = "【模板状态：保存未提交】";
                }
                if (stateid == "1")//提交
                {
                    pictureBox1.Image = imageCollection_Tree.Images[6];
                    label1.Text = "【模板状态：提交】";
                }
                if (stateid == "2")//审核通过
                {
                    pictureBox1.Image = imageCollection_Tree.Images[5];
                    label1.Text = "【模板状态：审核通过】";
                }
                if (stateid == "3")//审核未通过
                {
                    pictureBox1.Image = imageCollection_Tree.Images[4];
                    label1.Text = "【模板状态：审核未通过】";
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        private void treeList_Items_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            try
            {
                if ((e.Node == null) || (e.Node.Tag == null)) return;
                EmrNodeObj obj = (EmrNodeObj)e.Node.Tag;

                if (obj.Type == EmrNodeType.Container)
                {
                    if (e.Node.Expanded)
                        e.NodeImageIndex = 3;
                    else
                        e.NodeImageIndex = 2;
                }
                else if (obj.Type == EmrNodeType.Leaf)
                    e.NodeImageIndex = 0;

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void treeList_Template_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            try
            {
                if ((e.Node == null) || (e.Node.Tag == null)) return;
                EmrNodeObj obj = (EmrNodeObj)e.Node.Tag;

                if (obj.Type == EmrNodeType.Container)
                {
                    if (e.Node.Expanded)
                        e.NodeImageIndex = 3;
                    else
                        e.NodeImageIndex = 2;
                }
                //else if (obj.Type == EmrNodeType.Leaf)
                //    e.NodeImageIndex = 0;
                else if (obj.Type == EmrNodeType.Leaf)
                {
                    DataRow o = (DataRow)((EmrNodeObj)e.Node.Tag).Obj;
                    if (o["State"].ToString() == "0")
                        e.NodeImageIndex = 0;
                    else if (o["State"].ToString() == "1")
                        e.NodeImageIndex = 6;
                    else if (o["State"].ToString() == "2")
                        e.NodeImageIndex = 5;
                    else if (o["State"].ToString() == "3")
                        e.NodeImageIndex = 4;

                    //if (o["todept"].ToString() != "0")
                    //    e.Node. = 7;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 设置选中的节点
        /// </summary>
        /// <param name="type">类型：0-科室；1-节点(模板类型，如住院志)；2-模板</param>
        /// <param name="id">科室ID/节点(模板类型)编码/模板ID</param>
        /// <param name="nodes">节点集</param>
        private bool SetFocusedNode(string deptID, string mrClass, string templeteID, TreeListNodes nodes)
        {
            try
            {
                if (null == nodes || nodes.Count == 0)
                {
                    return false;
                }
                CollapseAllNodes(nodes);
                if (!string.IsNullOrEmpty(deptID) && !string.IsNullOrEmpty(mrClass) && !string.IsNullOrEmpty(templeteID))
                {
                    if (SetFocusedNodeSingle(deptID, nodes))
                    {
                        bool boo = SetFocusedNodeSingle(mrClass, null == treeList_Template.FocusedNode ? null : treeList_Template.FocusedNode.Nodes);
                        if (boo)
                        {
                            SetFocusedNodeSingle(templeteID, null == treeList_Template.FocusedNode ? null : treeList_Template.FocusedNode.Nodes);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(deptID) && !string.IsNullOrEmpty(mrClass))
                {
                    if (SetFocusedNodeSingle(deptID, nodes))
                    {
                        SetFocusedNodeSingle(mrClass, null == treeList_Template.FocusedNode ? null : treeList_Template.FocusedNode.Nodes);
                    }
                }
                else if (!string.IsNullOrEmpty(deptID))
                {
                    SetFocusedNodeSingle(deptID, nodes);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置节点焦点（ID唯一）
        /// </summary>
        /// <param name="id">科室ID/节点(模板类型)编码/模板ID</param>
        /// <param name="nodes">节点集</param>
        private bool SetFocusedNodeSingle(string id, TreeListNodes nodes)
        {
            try
            {
                if (null == nodes || nodes.Count == 0 || string.IsNullOrEmpty(id))
                {
                    return false;
                }
                foreach (TreeListNode node in nodes)
                {
                    if (null == node || null == node.Tag || !(node.Tag is EmrNodeObj))
                    {
                        continue;
                    }
                    DataRow row = (DataRow)((EmrNodeObj)node.Tag).Obj;

                    if (null != row[0] && row[0].ToString() == id)
                    {
                        treeList_Template.FocusedNode = node;
                        return true;
                    }
                    if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        bool boo = SetFocusedNodeSingle(id, node.Nodes);
                        if (boo)
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
        /// 收缩全部节点
        /// </summary>
        /// <param name="nodes"></param>
        private void CollapseAllNodes(TreeListNodes nodes)
        {
            try
            {
                if (null == nodes || nodes.Count == 0)
                {
                    return;
                }
                foreach (TreeListNode node in nodes)
                {
                    node.Expanded = false;
                    if (null != node.Nodes && node.Nodes.Count > 0)
                    {
                        CollapseAllNodes(node.Nodes);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region tree 事件

        /// <summary>
        /// 选择模板列表中模板节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Template_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (e.Node == null)
                {
                    return;
                }
                if (e.Node.Focused)
                {
                    m_treelistnode = e.Node;
                    //if (e.Node.Tag == null)
                    //    return;
                    //if (((EmrNodeObj)e.Node.Tag).Type != EmrNodeType.Leaf)
                    //{
                    //    DataRow obj = (DataRow)((EmrNodeObj)e.Node.Tag).Obj;
                    //    string mr_class = obj.ItemArray[0].ToString();
                    //    //选择是科室下子节点时记录新建EmrTemplet相关值
                    //    if (obj.ItemArray.Count() == 4)
                    //    {
                    //        m_new_Emrtemplet = new Emrtemplet(m_app);
                    //        m_new_Emrtemplet.MrClass = obj.ItemArray[0].ToString();

                    //        //获取科室节点，取科室信息
                    //        obj = (DataRow)((EmrNodeObj)e.Node.ParentNode.Tag).Obj;
                    //        m_new_Emrtemplet.DeptId = obj.ItemArray[0].ToString();
                    //    }

                    //}
                    //else
                    //{
                    //    DataRow obj = (DataRow)((EmrNodeObj)e.Node.Tag).Obj;
                    //    string templet_id = obj.ItemArray[0].ToString();
                    //    LoadTemplet(m_SqlManger.GetTemplet(templet_id));
                    //    m_EmrTempletType = EmrTempletType.Templet;
                    //    //记录下选中的叶子节点
                    //    m_treelistnode = e.Node;
                    //}
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        public bool IsShowPageConfig = true;//声明控制页面设置选择项的显示变量，默认赋给true
        private void treeList_Template_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hit = treeList_Template.CalcHitInfo(new Point(e.X, e.Y));
                if (e.Button == MouseButtons.Right)
                {
                    if (hit.Node != null)
                    {
                        if (hit.Node.Tag == null)
                            return;
                        //对病程记录的右键操作将页面设置的选择框进行隐藏 edit by ywk 2012年3月31日14:12:39 
                        DataRow mrows = (DataRow)((EmrNodeObj)hit.Node.Tag).Obj;
                        string typename = "";//获得鼠标选择的（点的文件夹）节点名称 
                        string childtype = "";
                        if (mrows.ItemArray.Length > 4)//选的文件夹下子节点
                        {
                            childtype = mrows[7].ToString();//点的文件夹下的子节点
                            if (childtype == "AC")//判断所属分类的标志
                            {
                                IsShowPageConfig = true;
                            }
                            else
                            {
                                IsShowPageConfig = true;
                            }
                        }
                        else//点的文件夹
                        {
                            typename = mrows[1].ToString();
                            if (typename == "病程记录")//判断所点文件夹的名称 
                            {
                                IsShowPageConfig = true;//不显示 
                            }
                            else
                            {
                                IsShowPageConfig = true;
                            }
                        }

                        //判断该登录人是否有权限操作这个科室模版
                        if (CheckDocmentEditLimit(hit.Node))
                        {
                            if (((EmrNodeObj)hit.Node.Tag).Type == EmrNodeType.Leaf)
                            {
                                //DataRow obj = (DataRow)((EmrNodeObj)hit.Node.Tag).Obj;
                                treeList_Template.FocusedNode = hit.Node;
                                treeList_Template.Focus();
                                barButtonItem10.Caption = "修改";
                                popupMenuTemplet.ShowPopup(treeList_Template.PointToScreen(new Point(e.X, e.Y)));
                                //this.btn
                            }
                            else
                            {
                                DataRow obj = (DataRow)((EmrNodeObj)hit.Node.Tag).Obj;
                                string mr_class = obj.ItemArray[0].ToString();
                                //选择是科室下子节点时记录新建EmrTemplet相关值
                                if (obj.ItemArray.Count() == 4)
                                {
                                    treeList_Template.FocusedNode = hit.Node;
                                    treeList_Template.Focus();
                                    barButtonItem10.Caption = "新建模板";
                                    popupMenuTemplet.ShowPopup(treeList_Template.PointToScreen(new Point(e.X, e.Y)));
                                }
                            }
                        }
                    }
                    barButtonItemSetMy.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//隐藏设为我的模板add ywk
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 添加工具提示 --- 模板列表
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Template_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hitInfo = treeList_Template.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.Node != null)
                {
                    if (hitInfo.Node.Level >= 2 && hitInfo.Node.Tag is EmrNodeObj)
                    {
                        DataRow item = (DataRow)((EmrNodeObj)hitInfo.Node.Tag).Obj;
                        if (null == item)
                        {
                            return;
                        }
                        DS_Common.InitToolTip(toolTipControllerTreeList, treeList_Template, treeList_Template.PointToScreen(e.Location), null == item[1] ? "" : item[1].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 添加工具提示 --- 小模板列表
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-12</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Items_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hitInfo = treeList_Items.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.Node != null)
                {
                    if (hitInfo.Node.Level >= 2 && hitInfo.Node.Tag is EmrNodeObj)
                    {
                        EmrNodeObj emrObj = (EmrNodeObj)hitInfo.Node.Tag;
                        if (emrObj.Obj is DataRow)
                        {
                            DataRow item = (DataRow)emrObj.Obj;
                            if (null == item)
                            {
                                return;
                            }
                            DS_Common.InitToolTip(toolTipControllerTreeList, treeList_Items, treeList_Items.PointToScreen(e.Location), null == item["mr_name"] ? "" : item["mr_name"].ToString());
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
        /// 根据右键模块树结构中选中的接点 判断当前登录人是否可以编辑该模块信息 或添加新模版
        /// </summary>
        /// <param name="treenode"></param>
        /// <returns></returns>
        private bool CheckDocmentEditLimit(TreeListNode treenode)
        {
            try
            {
                string deptid;
                if (((EmrNodeObj)treenode.Tag).Type == EmrNodeType.Leaf)
                {
                    deptid = ((DataRow)((EmrNodeObj)treenode.ParentNode.ParentNode.Tag).Obj).ItemArray[0].ToString();
                }
                else
                {
                    DataRow obj = (DataRow)((EmrNodeObj)treenode.Tag).Obj;
                    if (obj.ItemArray.Count() == 4)
                    {
                        obj = (DataRow)((EmrNodeObj)treenode.ParentNode.Tag).Obj;
                    }
                    deptid = obj.ItemArray[0].ToString();
                    //选择是科室下子节点时记录新建EmrTemplet相关值
                }
                return SetDocumentModel(deptid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void treeList_Items_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (e.Node == null)
                {
                    return;
                }
                if (e.Node.Focused)
                {
                    m_treelistnode_item = e.Node;
                    //if (e.Node.Tag == null)
                    //    return;
                    //if (((EmrNodeObj)e.Node.Tag).Type != EmrNodeType.Leaf)
                    //{
                    //    m_new_Emrtemplet_Item = new Emrtemplet_Item(m_app);

                    //    if (((EmrNodeObj)e.Node.Tag).Obj.GetType().Name == "DataRow")
                    //    {
                    //        DataRow obj = (DataRow)((EmrNodeObj)e.Node.Tag).Obj;

                    //        m_new_Emrtemplet_Item.DeptId = obj.ItemArray[0].ToString();
                    //    }
                    //    //如果选中的是叶子节点上一节点则记录下信息
                    //    else
                    //    {
                    //        m_new_Emrtemplet_Item.MrClass = ((EmrNodeObj)e.Node.Tag).Obj.ToString();
                    //        DataRow obj = (DataRow)((EmrNodeObj)e.Node.ParentNode.Tag).Obj;
                    //        m_new_Emrtemplet_Item.DeptId = obj.ItemArray[0].ToString();
                    //    }
                    //}
                    //else
                    //{

                    //    DataRow obj = (DataRow)((EmrNodeObj)e.Node.Tag).Obj;
                    //    string mr_Code = obj.ItemArray[1].ToString();
                    //    m_Emrtemplet_Item = m_SqlManger.GetTemplet_Item(mr_Code);
                    //    //LoadTemplet_Item(m_SqlManger.GetTemplet_Item(mr_Code));
                    //    m_EmrTempletType = EmrTempletType.Templet_Item;
                    //    ////记录下选中的叶子节点
                    //    m_treelistnode_item = e.Node;
                    //}
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 字模板模块右击弹出新增字模板按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Items_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hit = treeList_Items.CalcHitInfo(new Point(e.X, e.Y));

                if (e.Button == MouseButtons.Right)
                {
                    if (hit.Node != null)
                    {
                        if (hit.Node.Tag == null)
                            return;
                        if (((EmrNodeObj)hit.Node.Tag).Type == EmrNodeType.Leaf)
                        {
                            treeList_Items.FocusedNode = hit.Node;
                            treeList_Items.Focus();
                            barButtonItem13.Caption = "修改";
                            popupMenuItem.ShowPopup(treeList_Items.PointToScreen(new Point(e.X, e.Y)));
                        }
                        else
                        {
                            //点击节点为叶子节点上一节点时弹出右键菜单
                            if (((EmrNodeObj)hit.Node.Tag).Obj.GetType().Name == "String")
                            {
                                treeList_Items.FocusedNode = hit.Node;
                                treeList_Items.Focus();
                                barButtonItem13.Caption = "新建子模板";
                                popupMenuItem.ShowPopup(treeList_Items.PointToScreen(new Point(e.X, e.Y)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 双击模板树加载模板
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-25
        /// 1、add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Template_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TreeList list = (TreeList)sender;

                m_treelistnode = list.FocusedNode;
                TreeListNode node = list.FocusedNode;
                if (list.Name == this.treeListMy.Name)
                    IsPerson = true;
                else
                    IsPerson = false;
                if (node == null)
                    return;
                if (node.Tag == null)
                    return;
                if (((EmrNodeObj)node.Tag).Type != EmrNodeType.Leaf)
                {
                    DataRow obj = (DataRow)((EmrNodeObj)node.Tag).Obj;
                    string mr_class = obj.ItemArray[0].ToString();
                    //选择是科室下子节点时记录新建EmrTemplet相关值
                    if (obj.ItemArray.Count() == 4)
                    {
                        m_new_Emrtemplet = new Emrtemplet();
                        m_new_Emrtemplet.MrClass = obj.ItemArray[0].ToString();

                        //获取科室节点，取科室信息
                        obj = (DataRow)((EmrNodeObj)m_treelistnode.ParentNode.Tag).Obj;
                        m_new_Emrtemplet.DeptId = obj.ItemArray[0].ToString();
                    }
                    label1.Visible = false;
                    pictureBox1.Visible = false;
                }
                else
                {
                    DataRow obj = (DataRow)((EmrNodeObj)node.Tag).Obj;
                    string templet_id = obj.ItemArray[0].ToString();
                    if (IsPerson)
                    {
                        LoadTemplet(m_SqlManger.GetMyTemplet(templet_id));
                    }
                    else
                    {
                        LoadTemplet(m_SqlManger.GetTemplet(templet_id));
                    }
                    m_EmrTempletType = EmrTempletType.Templet;
                    //add by cyq 2013-03-11 设置选项卡名称
                    this.Text = "模型维护工厂" + " --- " + m_Emrtemplet.MrName;
                    //记录下选中的叶子节点
                    //m_treelistnode = e.Node;
                    string stateid = m_Emrtemplet.State;//obj["State"].ToString(); //edit by cyq 2013-02-25 模板状态即时刷新
                    label1.Visible = true;
                    pictureBox1.Visible = true;
                    if (stateid == "0")//保存未提交
                    {
                        pictureBox1.Image = imageCollection_Tree.Images[0];
                        label1.Text = "【模板状态：保存未提交】";
                    }
                    if (stateid == "1")//提交
                    {
                        pictureBox1.Image = imageCollection_Tree.Images[6];
                        label1.Text = "【模板状态：待审核】";//"【模板状态：提交】"; //edit by cyq 2013-02-25
                    }
                    if (stateid == "2")//审核通过
                    {
                        pictureBox1.Image = imageCollection_Tree.Images[5];
                        label1.Text = "【模板状态：审核通过】";
                    }
                    if (stateid == "3")//审核未通过
                    {
                        pictureBox1.Image = imageCollection_Tree.Images[4];
                        label1.Text = "【模板状态：审核未通过】";
                    }
                    if (obj["State"].ToString() != stateid)
                    {
                        RefreshTreeListNode(m_Emrtemplet.TempletId);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 双击子模板树加子载模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Items_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (m_treelistnode_item == null)
                    return;
                if (m_treelistnode_item.Tag == null)
                    return;
                if (((EmrNodeObj)m_treelistnode_item.Tag).Type != EmrNodeType.Leaf)
                {
                    m_new_Emrtemplet_Item = new Emrtemplet_Item();

                    if (((EmrNodeObj)m_treelistnode_item.Tag).Obj.GetType().Name == "DataRow")
                    {
                        DataRow obj = (DataRow)((EmrNodeObj)m_treelistnode_item.Tag).Obj;

                        m_new_Emrtemplet_Item.DeptId = obj.ItemArray[0].ToString();
                    }
                    //如果选中的是叶子节点上一节点则记录下信息
                    else
                    {
                        m_new_Emrtemplet_Item.MrClass = ((EmrNodeObj)m_treelistnode_item.Tag).Obj.ToString();
                        DataRow obj = (DataRow)((EmrNodeObj)m_treelistnode_item.ParentNode.Tag).Obj;
                        m_new_Emrtemplet_Item.DeptId = obj.ItemArray[0].ToString();
                    }
                }
                else
                {

                    DataRow obj = (DataRow)((EmrNodeObj)m_treelistnode_item.Tag).Obj;
                    string mr_Code = obj.ItemArray[1].ToString();
                    //m_Emrtemplet_Item = m_SqlManger.GetTemplet_Item(mr_Code);
                    LoadTemplet_Item(m_SqlManger.GetTemplet_Item(mr_Code));
                    m_EmrTempletType = EmrTempletType.Templet_Item;
                    ////记录下选中的叶子节点
                    //m_treelistnode_item = m_treelistnode_item;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        #endregion

        #region 菜单按钮事件
        /// <summary>
        /// 调用新建模板页窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                AddNewTemplet(m_new_Emrtemplet);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// toolbar按钮新增子模板
        /// edit by Yanqiao.Cai 2012-11-06
        /// add try ... catch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //树节点添加模板，记录子模板基本值
                AddNewTemplet_Item(m_new_Emrtemplet_Item);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                AddNewTemplet_Item(m_new_Emrtemplet_Item);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 打开模板页眉，加载页眉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnOpenTempletHeader_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ShowTempletHeader m_showTempletHeader = new ShowTempletHeader(m_app);
                m_showTempletHeader.GetTempletId(m_Emrtemplet.TempletId);
                if (m_showTempletHeader.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!m_showTempletHeader.IsEdit)
                    {
                        m_EmrTempletHeader = m_showTempletHeader.m_emrtempletheader;

                        XmlDocument content = new XmlDocument();
                        content.PreserveWhitespace = true;
                        //content.LoadXml(m_EmrTempletHeader.Content.Replace(imgcontent, ""));
                        //content.LoadXml(m_EmrTempletHeader.Content.Replace(content.SelectSingleNode("//body/p [@align=0]/img").InnerText,""));
                        content.LoadXml(m_EmrTempletHeader.Content);
                        //string imgcontent = content.SelectSingleNode("//body/p [@align=0]/img").InnerText;


                        pnlText.EMRDoc.ShowHeader = true;
                        pnlText.EMRDoc.ShowFooter = true;




                        this.pnlText.EMRDoc.HeadDocument = content;


                        //string imgcontent = content.SelectSingleNode("//body/p [@align=0]/img").InnerText;
                        //byte[] bytBuf = Convert.FromBase64String(imgcontent);
                        //System.IO.MemoryStream myStream = new System.IO.MemoryStream(bytBuf);
                        //System.Drawing.Image myImg = System.Drawing.Image.FromStream(myStream);

                        ////XmlDocument newcontent =
                        //XmlDocument content1 = new XmlDocument();
                        ////content.PreserveWhitespace = true;
                        //content1.LoadXml( m_showTempletHeader.m_emrtempletheader.Content);
                        ////this.pnlText.EMRDoc.ClearContent();
                        //pnlText.EMRDoc.ShowHeader = true;
                        //pnlText.EMRDoc.ShowFooter = true;
                        ////pnlText.LoadXML(m_showTempletHeader.m_emrtempletheader.Content);
                        //pnlText.Document.HeadDocument = content1;




                    }
                    //修改页眉
                    else if (m_showTempletHeader.IsEdit == true)
                    {
                        m_EmrTempletHeader = m_showTempletHeader.m_emrtempletheader;

                        LoadTemplet_Header(m_EmrTempletHeader);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 设置页眉子模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SetSubTitle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (PageTitleForm titleForm = new PageTitleForm())
                {
                    if (titleForm.SubTitle == null)
                    {
                        m_app.CustomMessageBox.MessageShow("此页眉无子模板元素！");
                        return;
                    }
                    titleForm.SubTitle = pnlText.GetSubTitle();
                    if (titleForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        pnlText.SetSubTitle(titleForm.SubTitle);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_PageSetting_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // 2018-10-14 修改，添加修改编辑器页面设置
                XPageSettings PageSettings = new XPageSettings();
                bool result = pnlText.EMRDoc._PageSetting(ref PageSettings);
                if (!result)
                {
                    Margins margins = new System.Drawing.Printing.Margins();
                    PaperSize pagersize = new System.Drawing.Printing.PaperSize();
                    margins = PageSettings.Margins;
                    pagersize.Width = PageSettings.PaperSize.Width;
                    pagersize.Height = PageSettings.PaperSize.Height;
                    m_Util.SetPageSetting(PageSettings.PaperSize.Kind, pagersize, margins);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void btn_RowHeight_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (e.Item.Tag == null) return;
                int rowheight = (int)e.Item.Tag;
                this.pnlText.EMRDoc.EnableFixedLineHeigh = true;
                this.pnlText.EMRDoc.FixedLineHeigh = rowheight;
                this.pnlText.EMRDoc.ContentChanged();
                this.pnlText.EMRDoc.EndUpdate();
                //SetEmrDefaultSet();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 新增页脚文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add_Templet_Foot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                LoadBeforeSave();
                this.pnlText.EMRDoc.ClearContent();

                //隐藏页眉页脚
                //this.视图ToolStripMenuItem.Enabled = false;
                this.pnlText.EMRDoc.ShowHeader = false;
                this.pnlText.EMRDoc.ShowFooter = false;
                this.pnlText.EMRDoc.HeadString = "";
                this.pnlText.EMRDoc.FooterString = "";

                m_EmrTemplet_Foot = new EmrTemplet_Foot();

                m_EmrTempletType = EmrTempletType.Templet_Foot;
                //this.editorfrm.TabText = "新建页眉模板.t";
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 加载默认设置
        /// </summary>
        /// <param name="lineSpace"></param>
        private void SetEmrDefaultSet(string lineSpace)
        {
            try
            {
                m_app.EmrDefaultSettings.EleStyle = ZYEditorControl.ElementStyle;
                m_app.EmrDefaultSettings.EleColor = ZYEditorControl.ElementBackColor;
                //m_app.EmrDefaultSettings.LineHeight = pnlText.EMRDoc.FixedLineHeigh.ToString();
                m_app.EmrDefaultSettings.LineSpace = lineSpace;
                m_SqlManger.SetEmrDefault(CreateEmrDefaultSetXML());
                if (lineSpace != "")
                {
                    this.pnlText.SetLineSpace(float.Parse(lineSpace));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string CreateEmrDefaultSetXML()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<root>" +
                    "<fontname>宋体</fontname>" +
                    "<fontsize>小四</fontsize>" +
                    "<linespace>" + m_app.EmrDefaultSettings.LineSpace + "</linespace>" +
                    "<elestyle>" + ZYEditorControl.ElementStyle + "</elestyle>" +
                    "<elecolor>" + ColorTranslator.ToHtml(ZYEditorControl.ElementBackColor) + "</elecolor>" +
                    //"<lineheight>" + pnlText.EMRDoc.FixedLineHeigh + "</lineheight>" +
                    "</root>";
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitEmrDefaultSet()
        {
            try
            {
                //pnlText.SetLineSpace(float.Parse(m_app.EmrDefaultSettings.LineSpace));
                ZYEditorControl.ElementBackColor = m_app.EmrDefaultSettings.EleColor;
                ZYEditorControl.ElementStyle = m_app.EmrDefaultSettings.EleStyle;
                pnlText.EMRDoc.EnableFixedLineHeigh = true;
                pnlText.EMRDoc.FixedLineHeigh = Convert.ToInt32(m_app.EmrDefaultSettings.LineHeight);
                pnlText.EMRDoc.ContentChanged();
                pnlText.EMRDoc.EndUpdate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Ctrl+s保存事件
        /// </summary>
        void EMRDoc_OnSaveEMRTemp()
        {
            try
            {
                btn_Save_ItemClick(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查找替换
        /// </summary>
        void EMRDoc_OnShowFindForm()
        {
            try
            {
                btn_Find_ItemClick(null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 设置编辑器 DocumentModel

        /// <summary>
        /// 可以编辑就可以用的BarButton【不包括提交和删除】
        /// </summary>
        /// <param name="b"></param>
        private void CanEditEmrDoc(bool b)
        {
            try
            {
                btn_Delete.Enabled = b; //删除
                btn_Save.Enabled = b;   //保存
                btn_Cut.Enabled = b;    //剪切
                btn_Paste.Enabled = b;  //复制
                btn_Redo.Enabled = b;   //撤销
                btn_FontName.Enabled = b;   //字体
                btn_FontSize.Enabled = b;   //字号
                btn_Bold.Enabled = b;   //粗体
                btn_Italy.Enabled = b;  //斜体
                btn_Undline.Enabled = b;    //下划线
                btn_Sump.Enabled = b;   //上标
                btn_Sumb.Enabled = b;   //下标
                btn_FontColor.Enabled = b;  //字体颜色
                btn_InsertImage.Enabled = b;    //背景色
                barSubItem4.Enabled = b;    //表格编辑功能菜单
                barSubItem7.Enabled = b;    //选项中功能不可用
                btn_left.Enabled = b;   //左对齐
                btn_right.Enabled = b;  //右对齐
                btn_center.Enabled = b; //居中对齐
                btn_disperse.Enabled = b;   //分散对齐

                btn_Save2.Enabled = b;   //保存
                btn_Cut2.Enabled = b;    //剪切
                btn_Paste2.Enabled = b;  //复制
                btn_Redo2.Enabled = b;   //撤销
                btn_Redo2.Enabled = b;   //撤销

                btn_commit.Enabled = b; //提交
                btn_Audit.Enabled = b;  //审核
                btn_CancelSubmit.Enabled = b;  //撤销审核

                if (b)
                    SetDocmentDesginMode();
                else
                    SetDocmentReadOnlyMode();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置当前编辑器为只读模式
        /// </summary>
        private void SetDocmentReadOnlyMode()
        {
            try
            {
                this.pnlText.EMRDoc.Info.DocumentModel = DocumentModel.Read;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置当前编辑器为设计模式
        /// </summary>
        private void SetDocmentDesginMode()
        {
            try
            {
                this.pnlText.EMRDoc.Info.DocumentModel = DocumentModel.Design;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 审核功能模块事件

        /// <summary>
        /// 调用待审核窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NoAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (CheckAudit())
                {
                    NoAudit noaudit = new NoAudit(m_app);
                    if (noaudit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string templetid = noaudit.TempletID;
                        LoadTemplet(m_SqlManger.GetTemplet(templetid));
                        TreeListNode PatNode = treeList_Template.FindNodeByFieldValue("KeyID", templetid);
                        treeList_Template.FocusedNode = PatNode;
                        m_EmrTempletType = EmrTempletType.Templet;
                    }
                    //add ywk
                    if (noaudit.IsClose)
                    {
                        foreach (string item in noaudit.TempleteID)
                        {
                            RefreshTreeListNode(item);
                        }
                    }
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您没有审核权限");
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 判断当前登录人有无审核权限
        /// </summary>
        /// <returns></returns>
        private bool CheckAudit()
        {
            try
            {
                string[] config = m_SqlManger.GetTempletAuditConfig();
                if (config.Length != 3)
                    return false;
                else
                {
                    DataTable dt = m_SqlManger.GetJobsByUserID(m_app.User.Id);
                    string jobid = dt.Rows[0][0].ToString();

                    //判断如果登陆人有管理员权限 则全部可以修改
                    if (CheckJobInConfig(jobid, config[1]))
                    {
                        return true;
                    }
                    //如果有模板管理，主任医生权限 则可以修改当前科室模版
                    else if (CheckJobInConfig(jobid, config[2]))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 提交模版，将当前模版提交到上级医生处
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-25
        /// 1、add try ... catch
        /// 2、添加确认信息
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_commit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_EmrTempletType == EmrTempletType.Templet)
                {
                    //保存相关内容
                    if (m_Emrtemplet == null)
                    {
                        return;
                    }
                    if (pnlText.EMRDoc.Modified)
                    {
                        //提示保存 如果不保存 不给提交
                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您有数据尚未保存，无法提交。是否要保存？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                        if (m_EmrTempletType == EmrTempletType.Templet)
                        {
                            UpdateTemplet();
                        }
                    }

                    //提交模板确认
                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要提交模板 " + m_Emrtemplet.MrName + " 吗？", "提交模板", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    string mess = m_SqlManger.AuditTemplet(m_Emrtemplet.TempletId, m_app.User.Id, "1");
                    if (mess == "审核成功")
                    {
                        m_app.CustomMessageBox.MessageShow("提交成功");

                        RefreshTreeListNode(m_Emrtemplet.TempletId);
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow(mess);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-25
        /// 1、add try ... catch
        /// 2、添加确认信息
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Audit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_EmrTempletType == EmrTempletType.Templet)
                {

                    if (m_Emrtemplet == null)
                        return;

                    if (pnlText.EMRDoc.Modified)
                    {
                        //提示保存 如果不保存 不给审核
                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您有数据尚未保存，不能审核。是否要保存？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                        if (m_EmrTempletType == EmrTempletType.Templet)
                        {
                            UpdateTemplet();
                        }
                    }

                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要审核通过模板 " + m_Emrtemplet.MrName + " 吗？", "审核模板", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    string mess = m_SqlManger.AuditTemplet(m_Emrtemplet.TempletId, m_app.User.Id, "2");
                    if (mess == "审核成功")
                    {
                        m_app.CustomMessageBox.MessageShow("审核成功");

                        RefreshTreeListNode(m_Emrtemplet.TempletId);
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow(mess);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 取消审核状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CancelSubmit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (m_EmrTempletType == EmrTempletType.Templet)
                {
                    //保存相关内容
                    if (m_Emrtemplet == null)
                        return;

                    if (pnlText.EMRDoc.Modified)
                    {
                        //提示保存 如果不保存 不给审核
                        if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您有数据尚未保存，不能取消审核。是否要保存？", "提示信息", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                        if (m_EmrTempletType == EmrTempletType.Templet)
                        {
                            UpdateTemplet();
                        }
                    }

                    if (DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("您确定要取消审核模板 " + m_Emrtemplet.MrName + " 吗？", "取消审核模板", DrectSoft.Common.Ctrs.DLG.MyMessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                    string mess = m_SqlManger.AuditTemplet(m_Emrtemplet.TempletId, m_app.User.Id, "3");
                    if (mess == "审核成功")
                    {
                        m_app.CustomMessageBox.MessageShow("取消审核成功");

                        RefreshTreeListNode(m_Emrtemplet.TempletId);
                    }
                    else
                    {
                        m_app.CustomMessageBox.MessageShow(mess);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 刷新模版列表中树接点的图标  重新加载模版信息
        /// </summary>
        /// edit by Yanqiao.Cai 2013-02-25
        /// 1、add try ... catch
        /// <param name="templetid"></param>
        private void RefreshTreeListNode(string templetid)
        {
            try
            {
                LoadTemplet(m_SqlManger.GetTemplet(templetid));
                TreeListNode PatNode = treeList_Template.FindNodeByFieldValue("KeyID", templetid);
                if (PatNode == null)
                    return;
                DataRow dt_row = m_SqlManger.GetTemplet_Table(templetid).Rows[0];
                PatNode.Tag = new EmrNodeObj(dt_row, EmrNodeType.Leaf);

                treeList_Template.FocusedNode = PatNode;
                treeList_Template.Refresh();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 特殊符号事件

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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
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
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        #endregion

        private void navBarControl1_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {
            try
            {
                if (e.Group == navBarGroup_Image)//特殊字符
                {
                    if (treeListImage.Nodes.Count < 1)
                    {
                        treeListImage.BeginUnboundLoad();
                        foreach (DataRow row in m_SqlManger.ImageGallery.Rows)
                        {
                            TreeListNode node = treeListImage.AppendNode(new object[] { row["名称"] }, null);
                            node.Tag = row;
                        }
                        treeListImage.EndUnboundLoad();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void barButtonItemSetMy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if ((treeList_Template.FocusedNode == null) || (treeList_Template.FocusedNode.Tag == null)) return;

                EmrNodeObj obj = (EmrNodeObj)treeList_Template.FocusedNode.Tag;

                if (obj.Type == EmrNodeType.Leaf)
                {
                    SetMyTemplate((DataRow)obj.Obj);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void SetMyTemplate(DataRow row)
        {
            try
            {
                //save content
                DataRow[] rows = m_Util.MyModelList.Select("templet_id ='" + row["templet_id"].ToString() + "' and creator_id='" + m_app.User.Id + "'");
                if (rows.Length > 0)
                {
                    m_app.CustomMessageBox.MessageShow("您已经拥有该病历模板");
                    return;
                }

                XmlDocument content = new XmlDocument();
                pnlText.EMRDoc.ToXMLDocument(content);

                SqlParameter paramtype = new SqlParameter("EditType", SqlDbType.VarChar);
                paramtype.Value = "1";
                SqlParameter paramid = new SqlParameter("CODE", SqlDbType.VarChar);
                paramid.Value = row["templet_id"].ToString();
                SqlParameter paramname = new SqlParameter("MR_NAME", SqlDbType.VarChar);
                string mrName = row["mr_name"].ToString();
                mrName = mrName.Replace('★', ' ');
                paramname.Value = mrName.Trim();
                SqlParameter paramContent = new SqlParameter("EMRTEMLETCONTENT", SqlDbType.VarChar);
                paramContent.Value = RecordDal.ZipEmrXml(content.OuterXml);
                SqlParameter paramclass = new SqlParameter("MR_CLASS", SqlDbType.VarChar);
                paramclass.Value = row["Mr_Class"].ToString();
                SqlParameter paramCreater = new SqlParameter("CREATEUSER", SqlDbType.VarChar);
                paramCreater.Value = m_app.User.DoctorId;


                m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditMyEmrTemplet", new SqlParameter[] { paramtype, paramid, paramCreater, paramname, paramclass, paramContent }, CommandType.StoredProcedure);
                m_app.CustomMessageBox.MessageShow("个人模板保存成功");
                SetWaitDialogCaption("重新加载个人模板列表");

                m_Util.ReloadMyTemplate();
                InitMyTemplateTree();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveMyTemplate(Emrtemplet template, string editorType)
        {
            try
            {
                SqlParameter paramtype = new SqlParameter("EditType", SqlDbType.VarChar);
                paramtype.Value = editorType;
                SqlParameter paramid = new SqlParameter("CODE", SqlDbType.VarChar);
                paramid.Value = template.TempletId;
                SqlParameter paramname = new SqlParameter("MR_NAME", SqlDbType.VarChar);
                paramname.Value = template.MrName;
                SqlParameter paramContent = new SqlParameter("EMRTEMLETCONTENT", SqlDbType.VarChar);
                paramContent.Value = template.ZipXML_DOC_NEW;
                SqlParameter paramCreater = new SqlParameter("CREATEUSER", SqlDbType.VarChar);
                paramCreater.Value = m_app.User.DoctorId;
                m_app.SqlHelper.ExecuteDataSet("EmrTempletFactory.usp_EditMyEmrTemplet", new SqlParameter[] { paramtype, paramid, paramCreater, paramname, paramContent }, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 新增的图片维护功能。跳转到图片维护模块
        /// add by ywk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnOperImage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
        /// <summary>
        /// 新增的图片维护功能。跳转到图片维护模块
        /// add by ywk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                m_app.LoadPlugIn("DrectSoft.Core.ImageManager.dll", "DrectSoft.Core.ImageManager.ImageManager");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void panelContainer1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                this.treeList_Template.Location = new Point(0, 58);
                this.treeList_Template.Height = this.panelContainer1.Height - 113;
                this.treeList_Items.Location = new Point(0, 58);
                this.treeList_Items.Height = this.panelContainer1.Height - 113;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 数据元维护功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCommonNote_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                m_app.LoadPlugIn("DrectSoft.Core.CommonTableConfig.dll", "DrectSoft.Core.CommonTableConfig.CommonNoteConfig");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }
        private void btnDateElement_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                m_app.LoadPlugIn("DrectSoft.Core.CommonTableConfig.dll", "DrectSoft.Core.CommonTableConfig.DataElementConfig");
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void bnt_templetpagke_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TemplatePagek TemplatePagek = new TemplatePagek(m_app);
                if (TemplatePagek.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                {

                }

            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex.Message);
            }
        }

        private void barButtonItemPerson_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TemplatePersonForm TemplatePersonForm = new TemplatePersonForm(m_app);
            TemplatePersonForm.ShowDialog();
        }


    }

    /// <summary>
    /// 节点值
    /// </summary>
    public struct EmrNodeObj
    {
        public object Obj
        {
            get
            {
                return _obj;
            }
        }
        private object _obj;

        public EmrNodeType Type
        {
            get
            {
                return _type;
            }
        }
        private EmrNodeType _type;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="type"></param>
        public EmrNodeObj(object row, EmrNodeType type)
        {
            _obj = row;
            _type = type;

        }
    }

    /// <summary>
    /// 节点类别
    /// </summary>
    public enum EmrNodeType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Container = 1,
        /// <summary>
        /// 叶子节点
        /// </summary>
        Leaf = 2,

    }

    /// <summary>
    /// 操作模板类型
    /// </summary>
    public enum EmrTempletType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 模板
        /// </summary>
        Templet = 1,
        /// <summary>
        /// 子模板
        /// </summary>
        Templet_Item = 2,

        /// <summary>
        /// 页眉文件
        /// </summary>
        Templet_Header = 4,

        /// <summary>
        /// 页脚文件
        /// </summary>
        Templet_Foot = 8

    }
}
