using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YidanSoft.FrameWork.BizBus;
//using YidanSoft.CommonServiceInf;
//using YidanSoft.Core.Model;
using System.Collections.ObjectModel;
//using YidanSoft.Core.ModelDalInfSql;
//using YidanSoft.Core.ModelDalInf;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using YidanSoft.Resources;
using YidanSoft.Wordbook;
using YidanSoft.Common.Library;
using YidanSoft.FrameWork;
using YidanSoft.FrameWork.WinForm;
using YidanSoft.FrameWork.WinForm.Plugin;
using System.Collections;
//using YidanSoft.Core.Widget;
//using YidanSoft.Common.Report;
using DevExpress.Utils;
 

namespace YidanSoft.Core.AnalysisService
{
    public partial class QuyeryConsole : Form, IStartPlugIn
    {
        #region vars
        private DataTable m_DeptData;
        private DataTable m_WardData;
        private DataTable m_EmbedData;
        private WaitDialogForm m_WaitDialog;
        private DataSet m_PatientData;
        private string m_statiskInfo = string.Empty;
        //private Dictionary<string, EmrEmbededMoleNode> m_EmbedNodes;
        #endregion

        #region Propers

        private IBizBus QueryBizBus
        {
            get
            {
                if (_queryBizbus == null)
                    _queryBizbus = YidanSoft.FrameWork.BizBus.BusFactory.GetBus();
                return _queryBizbus;
            }
        }
        private IBizBus _queryBizbus;

        private SQLManager _SQLManager;
        private SQLManager SQLManager
        {
            get
            {
                if (_SQLManager == null)
                    _SQLManager = new SQLManager(_app);
                return _SQLManager;
            }
        }

        //private IEMRQuery QueryService
        //{
        //    get
        //    {
        //        if (_queryService == null)
        //        {
        //            _queryService = QueryBizBus.BuildUp<IEMRQuery>();
        //        }
        //        return _queryService;
        //    }
        //}
        //private IEMRQuery _queryService;

        //private EmrEmbededMoleNodeDal EmbedDal
        //{
        //    get
        //    {
        //        if (_embedal == null)
        //            _embedal = EmrEmbededMoleNodeDal.Instance;
        //        return _embedal;
        //    }

        //}
        //private EmrEmbededMoleNodeDal _embedal;

        //private EmrQueryCondition _nowSearchMasterplate;
        //private EmrQueryCondition NowSearchMasterplate
        //{
        //    get
        //    {
        //        return _nowSearchMasterplate;
        //    }
        //    set
        //    {
        //        _nowSearchMasterplate = value;
        //        txtCondition.Text = " " + _nowSearchMasterplate.ToString().Trim(' ');
        //    }
        //}

        private IYidanEmrHost App
        {
            get
            {
                return _app;
            }
        }
        private IYidanEmrHost _app;

        private string ReportName
        {
            get
            {
                _reportName = App.User.Name + treeListSolution.FocusedNode["Name"].ToString();
                return _reportName;
            }
        }
        private string _reportName;


        #endregion

        #region ctor
        /// <summary>
        /// 构造函数
        /// </summary>
        public QuyeryConsole()
        {
            InitializeComponent();
        }

        #endregion

        #region common methods

        /// <summary>
        /// 初始化科室
        /// </summary>
        private void InitializeDepartment()
        {
            string sql = string.Format(@"select distinct ID, NAME,py,wb from department a ,dept2ward b where a.id = b.deptid ");

            if (m_DeptData == null)
                m_DeptData = _app.SqlHelper.ExecuteDataTable(sql);
            m_DeptData.Columns["ID"].Caption = "科室代码";
            m_DeptData.Columns["NAME"].Caption = "科室名称";
            Dictionary<String, int> columnwidthKs = new Dictionary<String, int>();
            columnwidthKs.Add("ID", 100);
            columnwidthKs.Add("NAME", 100);
            SqlWordbook sqlWordBookKs = new SqlWordbook("querydept", m_DeptData, "ID", "NAME", columnwidthKs, true);
            lookupDepartment.SqlWordbook = sqlWordBookKs;
        }

        /// <summary>
        ///病区数据
        /// </summary>
        private void InitializeWard()
        {
            string RetrieveWardSql = "select ID,Name,Py,Wb from Ward where Valid=1";
            if (m_WardData == null)
            {
                m_WardData = _app.SqlHelper.ExecuteDataTable(RetrieveWardSql);

                m_WardData.Columns["ID"].Caption = "病区代码";
                m_WardData.Columns["NAME"].Caption = "病区名称";
            }
            //绑定到下拉框
            Dictionary<String, int> columnwidthBq = new Dictionary<String, int>();
            columnwidthBq.Add("ID", 100);
            columnwidthBq.Add("NAME", 100);
            SqlWordbook sqlwordBq = new SqlWordbook("queryward", m_WardData, "ID", "NAME", columnwidthBq, true);
            lookupWard.SqlWordbook = sqlwordBq;
        }

        /// <summary>
        /// 初始传染病病种类别
        /// </summary>
        private void InitBzlb()
        {

            lookUpWindowDiag.SqlHelper = _app.SqlHelper;

            string sql = string.Format(@"select a.icd,a.name,a.py,a.wb from Diagnosis a;");
            DataTable Bzlb = _app.SqlHelper.ExecuteDataTable(sql);

            Bzlb.Columns["ICD"].Caption = "病种代码";
            Bzlb.Columns["NAME"].Caption = "病种名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("ICD", 80);
            cols.Add("NAME", 160);

            SqlWordbook deptWordBook = new SqlWordbook("querybook", Bzlb, "ICD", "NAME", cols, "ICD//Name//PY//WB");
            lookUpEditorDiag.SqlWordbook = deptWordBook;

        }

        private void QueryEmbedData()
        { 
            string EmbedData = "select a.ID,a.Name,a.Describe from Analysis_Embed a where  a.Valid=1";
            if (m_EmbedData == null)
            {
                m_EmbedData = _app.SqlHelper.ExecuteDataTable(EmbedData);
            }
        }


        /// <summary>
        ///  初始化图象List
        /// </summary>
        /// <param name="imageList"></param>
        private void InitializeImageList(ImageList imageList)
        {
            imageList.Images.Clear();
            imageList.TransparentColor = System.Drawing.Color.Magenta;
            imageList.Images.Add(GetBitmap(ResourceNames.FolderOpen));
            imageList.Images.Add(GetBitmap(ResourceNames.NewDocument));
            imageList.Images.Add(GetBitmap(ResourceNames.ModelDynamic));
            imageList.Images.Add(GetBitmap(ResourceNames.ModelEmbedded));
            imageList.Images.Add(GetBitmap(ResourceNames.ModelObject));
            imageList.Images.Add(GetBitmap(ResourceNames.ModelAtom));
            imageList.Images.SetKeyName(0, "Folder");
            imageList.Images.SetKeyName(1, "Document");
            imageList.Images.SetKeyName(2, "Dynamic");
            imageList.Images.SetKeyName(3, "Embeded");
            imageList.Images.SetKeyName(4, "Object");
            imageList.Images.SetKeyName(5, "Atom");
        }

        private Image GetBitmap(String iconname)
        {
            return ResourceManager.GetSmallIcon(iconname, IconType.Normal);
        }


        private void SetWaitDialogCaption(string caption)
        {
            if (m_WaitDialog != null)
            {
                if (!m_WaitDialog.Visible)
                    m_WaitDialog.Visible = true;
                m_WaitDialog.Caption = caption;
            }
        }

        private void HideWaitDialog()
        {
            if (m_WaitDialog != null)
                m_WaitDialog.Hide();
        }

        //private EmrEmbededMoleNode GetEmbedNode(string id)
        //{
        //    if (string.IsNullOrEmpty(id)) return null;
        //    return EmbedDal.GetEmbededMoleNode(id);

        //}


        private void InitData()
        {
            m_WaitDialog = new WaitDialogForm("正在加载", "请稍候！");
            SetWaitDialogCaption("正在初始化基础数据");
            //image
            InitializeImageList(this.imageList1);
            lookUpWindow.SqlHelper = DataAccessFactory.DefaultDataAccess;
            //m_EmbedNodes = new Dictionary<string, EmrEmbededMoleNode>();
            InitializeMyProject();
            //init wordbook
            InitializeDepartment();
            InitializeWard();
            //获取病历数据元
            QueryEmbedData();
            //构造模板容器树
            InitializeModelContainer();
            //构造查询数据元树
            InitEmbedTree();
            //
            InitializeControlProperty();
            InitBzlb();
            HideWaitDialog();

            

        }

        private void InitializeControlProperty()
        {
            cmbLogic.SelectedIndex = 0;
            dateEditStartDate.DateTime = DateTime.Today.AddMonths(-1).AddYears(-4);
            dateEditEndDate.DateTime = DateTime.Today;
        }

        /// <summary>
        /// 获取我的查询模板
        /// </summary>
        /// <param name="treeView"></param>
        private void InitializeMyProject()
        {
            treeListSolution.BeginUnboundLoad();
            treeListSolution.Nodes.Clear();
            string hit = App.User.Name + "查询统计案例";
            TreeListNode parentnode = treeListSolution.AppendNode(new object[] { hit }, null);

            DataTable dt = SQLManager.MDS_RetrieveMyTemplate(App.User.Id);
            foreach (DataRow dr in dt.Rows)
            {
                TreeListNode node = treeListSolution.AppendNode(new object[] { dr["Name"] }, parentnode);
                node.Tag = dr["SerialNum"];
                node.ImageIndex = 3;
            }
            treeListSolution.EndUnboundLoad();
            treeListSolution.ExpandAll();
        }

        private void InittreeListCurrentEmbed()
        {
            //treeListCurrentEmbed.Nodes.Clear();
            if (treeListCurrentEmbed.Nodes.Count > 0)
                return;
            treeListCurrentEmbed.BeginUnboundLoad();
            foreach (DataRow dr in SQLManager.TreeListEmbedTable.Rows)
            {
                TreeListNode treeobjNode = treeListCurrentEmbed.AppendNode(new object[] { dr["NAME"] }, null);
                //treeobjNode.Tag = obj;
                treeobjNode.ImageIndex = 4;
                treeobjNode.Expanded = true;
                foreach (DataRow drsun in SQLManager.TreeListEmbedSunTable.Rows)
                {
                    if (drsun["PERID"].ToString() == dr["ID"].ToString())
                    {
                        TreeListNode treeatomNode = treeListCurrentEmbed.AppendNode(new object[] { dr["NAME"] }, treeobjNode);
                        //treeatomNode.Tag = emratom;
                        treeatomNode.ImageIndex = 5;
                    }
                }
            }
            treeListCurrentEmbed.EndUnboundLoad();
        }

        private void InitEmbedTree()
        {
            if ((m_EmbedData == null) || (m_EmbedData.Rows.Count < 1))
                return;
            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            treeListEmbed.BeginUnboundLoad();

            foreach (DataRow row in m_EmbedData.Rows)
            {
                TreeListNode node = treeListEmbed.AppendNode(new object[] { row["Name"] }, null);
                node.HasChildren = true;
                node.Tag = row["ID"];
                node.ImageIndex = 3;

            }
            treeListEmbed.EndUnboundLoad();

            Cursor.Current = currentCursor;

        }

        /// <summary>
        /// 构造基础数据元
        /// </summary>
        /// <param name="node"></param>
        //private void InitEmbedNode(EmrEmbededMoleNode embedNode, TreeList treelist, TreeListNode parentNode)
        //{
        //    if (embedNode == null) return;

        //    foreach (EmrNode emrnode in embedNode.GetNodeListByNodeType(EmrNodeType.Object))
        //    {
        //        EmrObject obj = emrnode as EmrObject;
        //        TreeListNode treeobjNode = treelist.AppendNode(new object[] { obj.Name }, parentNode);
        //        treeobjNode.Tag = obj;
        //        treeobjNode.ImageIndex = 4;
        //        treeobjNode.Expanded = true;
        //        foreach (EmrAtomNode emratom in obj.GetAtomNodeList())
        //        {
        //            TreeListNode treeatomNode = treelist.AppendNode(new object[] { obj.Name }, treeobjNode);
        //            treeatomNode.Tag = emratom;
        //            treeatomNode.ImageIndex = 5;
        //        }
        //    }
        //}


        private void QueryInpatient()
        {
            StringBuilder baseSqlWhere = new StringBuilder(" 1=1");
            baseSqlWhere.Append(" and b.OutHosDate between '" + dateEditStartDate.DateTime.ToString("yyyy-MM-dd") + "' and '" + dateEditEndDate.DateTime.ToString("yyyy-MM-dd") + "'");

            if (!String.IsNullOrEmpty(lookupDepartment.CodeValue))
                baseSqlWhere.Append(" and b.AdmitDept='" + lookupDepartment.CodeValue + "'");

            if (!String.IsNullOrEmpty(lookupWard.CodeValue))
                baseSqlWhere.Append(" and b.AdmitWard='" + lookupWard.CodeValue + "'");

            //m_PatientData = SQLManager.QueryPatientRecord(baseSqlWhere.ToString(), NowSearchMasterplate.ToSqlString(), "");
            m_PatientData = SQLManager.QueryPatientRecord(baseSqlWhere.ToString(),"", "");
            gridControl1.DataSource = m_PatientData.Tables[0];

        }

        #endregion

        #region 统计相关方法和事件


        /// <summary>
        /// 获取统计所需要的数据
        /// </summary>
        private void WrapStatistInfo()
        {
            StringBuilder builder = new StringBuilder();

            foreach (DataRow dr in m_PatientData.Tables[0].Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                    continue;
                builder.Append("," + dr["NoOfInpat"].ToString());
            }

            if (builder.Length < 1) m_statiskInfo = string.Empty;
            else
            {
                m_statiskInfo = " a.NoOfInpat in (" + builder.ToString().Trim(',') + ")";
            }

        }

        /// <summary>
        /// 调出统计设计界面
        /// </summary>
        private void DoStastisk()
        {
            if ((m_PatientData == null) || (m_DeptData.Rows.Count < 1))
            {
                App.CustomMessageBox.MessageShow("无相关病历数据，请先指定查询条件,并执行查询！");

            }
            else
            {
                SetWaitDialogCaption("正在分析数据");
                WrapStatistInfo();
                //DataSet ds = QueryService.MRStatistik(m_statiskInfo);
                //ReportDesign desgin = new ReportDesign(ReportName, ds);
                HideWaitDialog();
                //desgin.Design();

            }

        }

        private void btnStatistik_Click(object sender, EventArgs e)
        {
            DoStastisk();
        }


        private void btnSaveCondition_Click(object sender, EventArgs e)
        {
            FormSaveQueryModel saveModel = new FormSaveQueryModel( App);
            if (saveModel.ShowDialog() == DialogResult.OK)
            {
                //InitializeMyProject();
            }
        }


        private void barButtonItemStatic_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DoStastisk();
        }

        #endregion

        #region 构造条件

        //private EmrQueryCondition LoadConditions(String queryId)
        //{
        //    DataTable dt = QueryService.MDS_GetTemplateByID(queryId).Tables[0];

        //    if (dt == null || dt.Rows.Count == 0)
        //        return null;

        //    EmrQueryCondition condition = null;
        //    Hashtable subconds = new Hashtable();
        //    Collection<Hashtable> hts = new Collection<Hashtable>();
        //    EmrNodeType ent = EmrNodeType.None;
        //    string fullPath = string.Empty;
        //    string fullPathName = string.Empty;
        //    decimal lastXh = decimal.Zero;
        //    bool lastIsTable = false;

        //    String lastCompare = "";
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        String compare = dr["Compare"].ToString();
        //        String logic = dr["Logic"].ToString();
        //        if (logic != "")
        //        {
        //            // 保存上次的原子条件或表格条件
        //            if (hts.Count > 0)
        //            {
        //                EmrNodeQueryResult enqr = new EmrNodeQueryResult(ent, hts, fullPath, fullPathName);
        //                EmrQueryCondition emrQueryCondition = new EmrQueryCondition(enqr, lastCompare);
        //                subconds.Add(lastXh, emrQueryCondition);
        //                hts.Clear();
        //            }
        //            if (dr["LeftSubxh"] == DBNull.Value || dr["RightSubxh"] == DBNull.Value)
        //                throw new ApplicationException("查询模板数据有误!");
        //            decimal leftxh = decimal.Parse(dr["LeftSubxh"].ToString());
        //            decimal rightxh = decimal.Parse(dr["RightSubxh"].ToString());
        //            EmrQueryCondition condA = null;
        //            EmrQueryCondition condB = null;
        //            if (subconds.ContainsKey(leftxh)) condA = subconds[leftxh] as EmrQueryCondition;
        //            if (subconds.ContainsKey(rightxh)) condB = subconds[rightxh] as EmrQueryCondition;
        //            condition = EmrQueryCondition.CombineCondition(condA, condB, logic);
        //            subconds.Add(dr["SerialNum"], condition);
        //        }
        //        else if (compare == "")
        //        {
        //            EmrQueryCondition eqcnull = new EmrQueryCondition();
        //            subconds.Add(dr["SerialNum"], eqcnull);
        //        }
        //        else
        //        {
        //            // 非表格清空查询条件集合
        //            if (!(lastIsTable && (dr["LineCode"] != DBNull.Value))) hts.Clear();
        //            lastIsTable = dr["LineCode"] != DBNull.Value;
        //            Hashtable ht = new Hashtable();
        //            if (dr["TemplateID"] != DBNull.Value)
        //            {
        //                ht.Add(EmrNodeType.Model, dr["TemplateID"].ToString());
        //                ent = EmrNodeType.Model;
        //            }
        //            if (dr["StructID"] != DBNull.Value)
        //            {
        //                ht.Add(EmrNodeType.DynamicMoleNode, dr["StructID"].ToString());
        //                ent = EmrNodeType.DynamicMoleNode;
        //            }
        //            if (dr["EmbedId"] != DBNull.Value)
        //            {
        //                ht.Add(EmrNodeType.EmbededMoleNode, dr["EmbedId"].ToString());
        //                ent = EmrNodeType.EmbededMoleNode;
        //            }
        //            if (lastIsTable)
        //            {
        //                if (dr["ObjectID"] != DBNull.Value)
        //                {
        //                    ht.Add(EmrNodeType.Table, dr["ObjectID"].ToString());
        //                    ent = EmrNodeType.Table;
        //                }
        //                if (dr["LineCode"] != DBNull.Value)
        //                {
        //                    ht.Add(EmrNodeType.Row, dr["LineCode"].ToString());
        //                    ent = EmrNodeType.Row;
        //                }
        //                if (dr["AtomID"] != DBNull.Value)
        //                {
        //                    ht.Add(EmrNodeType.Cell, dr["AtomID"].ToString());
        //                    ent = EmrNodeType.Cell;
        //                }
        //                ent = EmrNodeType.Table;
        //            }
        //            else
        //            {
        //                if (dr["ObjectID"] != DBNull.Value)
        //                {
        //                    ht.Add(EmrNodeType.Object, dr["ObjectID"].ToString());
        //                    ent = EmrNodeType.Object;
        //                }
        //                if (dr["AtomID"] != DBNull.Value)
        //                {
        //                    ht.Add(EmrNodeType.AtomNode, dr["AtomID"].ToString());
        //                    ent = EmrNodeType.AtomNode;
        //                }
        //            }
        //            if (dr["Qryvalue"] != DBNull.Value)
        //                ht.Add(EmrNodeQueryResult.CstQueryValue, dr["Qryvalue"].ToString());
        //            if (dr["Dspvalue"] != DBNull.Value)
        //                ht.Add(EmrNodeQueryResult.CstDisplayValue, dr["Dspvalue"].ToString());
        //            if (dr["Unit"] != DBNull.Value) ht.Add(EmrNodeQueryResult.CstDisplayUnit, dr["Unit"].ToString());
        //            if (dr["ValueType"] != DBNull.Value) ht.Add(EmrNodeQueryResult.CstValueType, dr["ValueType"]);
        //            if (dr["FullPath"] != DBNull.Value) fullPath = dr["FullPath"].ToString();
        //            if (dr["FullpathName"] != DBNull.Value) fullPathName = dr["FullpathName"].ToString();
        //            lastXh = decimal.Parse(dr["SerialNum"].ToString());
        //            lastCompare = compare;
        //            hts.Add(ht);
        //        }
        //    }

        //    return condition;
        //}

        //private void ShowConditionToListBox(ListBox listBox, EmrQueryCondition cond)
        //{
        //    if (cond == null || EmrQueryCondition.IsNullCondition(cond))
        //        return;
        //    if (cond.CondA != null || cond.CondB != null)
        //    {
        //        ShowConditionToListBox(listBox, cond.CondA);
        //        ShowConditionToListBox(listBox, cond.CondB);
        //    }
        //    else
        //    {
        //        listBox.Items.Add(cond);
        //    }
        //}

        private void btnClearCondition_Click(object sender, EventArgs e)
        {
            listBoxCond.Items.Clear();
            //NowSearchMasterplate = new EmrQueryCondition();
        }

        private void btnAddCondition_Click(object sender, EventArgs e)
        {
            //EmrQueryCondition eqc = listBoxCond.SelectedItem as EmrQueryCondition;
            //if (treeListCurrentEmbed.FocusedNode != null && treeListCurrentEmbed.FocusedNode.Tag != null && treeListCurrentEmbed.FocusedNode.Tag is EmrAtomNode)
            //{
            //    EmrNodeQueryResult qryNode = new EmrNodeQueryResult(treeListCurrentEmbed.FocusedNode.Tag as EmrNode);
            //    EmrQueryCondition qryCond = new EmrQueryCondition(qryNode, cmbCondition.SelectedItem.ToString());
            //    NowSearchMasterplate = EmrQueryCondition.CombineCondition(NowSearchMasterplate, qryCond, cmbLogic.SelectedItem.ToString());
            //    listBoxCond.Items.Add(qryCond);
            //}
            //else
            //{
            //    App.CustomMessageBox.MessageShow("请选择原子节点");
            //}


            if (treeListCurrentEmbed.FocusedNode != null && treeListCurrentEmbed.FocusedNode.ParentNode != null)
            {
                string qrycond = cmbLogic.SelectedItem.ToString() + "主要诊断(西医诊断)/"+treeListCurrentEmbed.FocusedNode.ParentNode.GetValue(0).ToString() +"/"+
                                treeListCurrentEmbed.FocusedNode.GetValue(0).ToString() +" "+ cmbCondition.SelectedItem.ToString()+" " + lookUpEditorDiag.DisplayValue;
                listBoxCond.Items.Add(qrycond);
            }
            else
            {
                App.CustomMessageBox.MessageShow("请选择原子节点");
            }
        }

        private void btnClearCondition_Click_1(object sender, EventArgs e)
        {
            listBoxCond.Items.Clear();
            //NowSearchMasterplate = new EmrQueryCondition();
            treeListCurrentEmbed.FocusedNode = null;
        }

        private void btnDeleteCondition_Click(object sender, EventArgs e)
        {
            if (listBoxCond.SelectedItem == null)
                return;

            //EmrQueryCondition subCond = listBoxCond.SelectedItem as EmrQueryCondition;
            //if (NowSearchMasterplate != null && subCond != null)
            //    NowSearchMasterplate = EmrQueryCondition.RemoveCondition(NowSearchMasterplate, subCond);

            listBoxCond.Items.Remove(listBoxCond.SelectedItem);
        }

        private void btnEditCondition_Click(object sender, EventArgs e)
        {
            if (listBoxCond.SelectedItem == null)
                return;

            //EmrQueryCondition eqc = listBoxCond.SelectedItem as EmrQueryCondition;
            //if (treeListCurrentEmbed.FocusedNode != null && treeListCurrentEmbed.FocusedNode.Tag != null && treeListCurrentEmbed.FocusedNode.Tag is EmrAtomNode)
            //{

            //    TreeListNode selectNode = treeListCurrentEmbed.FocusedNode;
            //    EmrNodeQueryResult qryNode = new EmrNodeQueryResult(selectNode.Tag as EmrNode);
            //    eqc.EmrQueryNode = qryNode;
            //    eqc.Compare = cmbCondition.SelectedItem.ToString();
            //    if (eqc.CondP != null)
            //        eqc.CondP.Logic = cmbLogic.SelectedItem.ToString();
            //    listBoxCond.Items[listBoxCond.SelectedIndex] = eqc;
            //}
            //else
            //{
            //    App.CustomMessageBox.MessageShow("请选择原子节点");
            //}

            string eqc = listBoxCond.SelectedItem.ToString();
            if (treeListCurrentEmbed.FocusedNode != null )
            {

                TreeListNode selectNode = treeListCurrentEmbed.FocusedNode;
                //EmrNodeQueryResult qryNode = new EmrNodeQueryResult(selectNode.Tag as EmrNode);
                //eqc.EmrQueryNode = qryNode;
                //eqc.Compare = cmbCondition.SelectedItem.ToString();
                //if (eqc.CondP != null)
                //    eqc.CondP.Logic = cmbLogic.SelectedItem.ToString();
                string neweqc = "";
                if(eqc.LastIndexOfAny("=,>,<,>=,<=,!=".ToCharArray())>0)
                neweqc = eqc.Substring(0, eqc.LastIndexOfAny("=,>,<,>=,<=,!=".ToCharArray()));
                if(neweqc.LastIndexOfAny("=,>,<,>=,<=,!=".ToCharArray())>0)
                neweqc = eqc.Substring(0, neweqc.LastIndexOfAny("=,>,<,>=,<=,!=".ToCharArray()));
                neweqc += cmbCondition.SelectedItem.ToString() + " " + lookUpEditorDiag.DisplayValue;

                listBoxCond.Items[listBoxCond.SelectedIndex] = neweqc;
            }
            else
            {
                App.CustomMessageBox.MessageShow("请选择原子节点");
            }
        }


        private void listBoxCond_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (listBoxCond.SelectedItem != null)
            //{
            //    EmrQueryCondition SelectCondition = listBoxCond.SelectedItem as EmrQueryCondition;
            //    if (SelectCondition.EmrQueryNode != null && SelectCondition.EmrQueryNode.NodeType == EmrNodeType.AtomNode)
            //    {
            //        //FullPath	"/*/235372ac-6455-4ba2-8728-c2c2a9628bc0/40ba47b2-acd3-47cb-acb6-68cd40e95df6/620ab5a7-82a5-412d-ab12-26fa5204b86d"	string
            //        String[] NodeIds = SelectCondition.EmrQueryNode.FullPath.Split('/');
            //        EmrEmbededMoleNode SelectModelEmbed = QueryEmbededModel(NodeIds[2]);
            //        if (treeListCurrentEmbed.Nodes.Count == 0 || (treeListCurrentEmbed.Nodes.Count > 0 && SelectModelEmbed != (EmrEmbededMoleNode)treeListCurrentEmbed.Tag))
            //        {

            //            InitEmbedNode(SelectModelEmbed, treeListCurrentEmbed, null);
            //            treeListCurrentEmbed.Tag = SelectModelEmbed;
            //        }

            //    }

            //}

            if (listBoxCond.SelectedItem != null)
            {
                InittreeListCurrentEmbed();
            }
        }

        #endregion

        #region 初始化模板容器
        private void InitializeModelContainer()
        {
            treeListRanage.BeginUnboundLoad();
            //foreach (EmrModelContainer emrModelContainer in QueryContainer())
            //{
            //    TreeListNode node = treeListRanage.AppendNode(new object[] { emrModelContainer.Name, }, null, CheckState.Checked);
            //    node.Tag = emrModelContainer;
            //    node.Checked = true;
            //    node.ImageIndex = 0;
            //}
            //treeListRanage.ExpandAll();
            //treeListRanage.EndUnboundLoad();

            foreach (DataRow dr in SQLManager.ItemCatalogTable.Rows)
            {
                TreeListNode node = treeListRanage.AppendNode(new object[] { dr["NAME"], }, null, CheckState.Checked);
                //node.Tag = dr;
                node.Checked = true;
                node.ImageIndex = 0;
            }
            treeListRanage.ExpandAll();
            treeListRanage.EndUnboundLoad();
        }

        //private Collection<EmrModelContainer> QueryContainer()
        //{
        //    Collection<EmrModelContainer> containers = new Collection<EmrModelContainer>();

        //    IEmrModelContainerDal containerDal = EmrModelContainerDal.Instance;
        //    DataTable ds = containerDal.GetMixNodeSet();
        //    if (ds != null)
        //    {
        //        foreach (DataRow dr in ds.Rows)
        //        {
        //            EmrModelContainer container = containerDal.DataRow2EmrModelContainer(dr);
        //            containers.Add(container);
        //        }
        //    }
        //    return containers;
        //}

        //private EmrEmbededMoleNode QueryEmbededModel(string id)
        //{
        //    EmrEmbededMoleNode embededModel = GetEmbededNode(id);
        //    UnloadAllTextObject(embededModel.Nodes);
        //    return embededModel;
        //}

        //private EmrEmbededMoleNode GetEmbededNode(string id)
        //{

        //    EmrEmbededMoleNode embededModel = null;

        //    if (m_EmbedNodes.Keys.Contains(id))
        //        embededModel = m_EmbedNodes[id];
        //    else
        //    {
        //        embededModel = EmbedDal.GetEmbededMoleNode(id);
        //        m_EmbedNodes.Add(id, embededModel);
        //    }
        //    return embededModel;


        //}

        //private void UnloadAllTextObject(EmrNodeCollection nodes)
        //{
        //    for (int i = nodes.Count; i > 0; i--)
        //    {
        //        EmrNode node = nodes[i - 1];
        //        if (node.NodeType == EmrNodeType.Text)
        //            nodes.Remove(node);
        //        else
        //        {
        //            EmrPackage ep = node as EmrPackage;
        //            if (ep != null)
        //                UnloadAllTextObject(ep.Nodes);
        //        }
        //    }
        //}


        ////根据选择的原子结点不同，动态设置数据输入控件和可用逻辑条件
        //private void ShowAtomNodeWidget(TreeListNode eNode)
        //{

        //    panelControlValueContrls.Controls.Clear();

        //    cmbCondition.Properties.Items.Clear();
        //    cmbCondition.Text = "";

        //    if (eNode.Tag == null)
        //        return;

        //    if ((eNode.Tag as EmrNode).NodeType == EmrNodeType.AtomNode)
        //    {
        //        EmrAtomNode ean = (eNode.Tag as EmrAtomNode);
        //        //图形结点暂不支持查询
        //        if (ean.AtomType != EmrAtomType.Graphic)
        //        {
        //            AbstractNodeWidget valueInputControl = WidgetFactory.CreateNodeWidget(ean);
        //            valueInputControl.Name = "ValueInputControl";
        //            //valueInputControl.Top = btnEditCondition.Top;
        //            valueInputControl.Top = 5;/************ Modified by dxj 2011/6/24 *****************/
        //            valueInputControl.Left = cmbCondition.Left;
        //            valueInputControl.Width = cmbCondition.Width;
        //            valueInputControl.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
        //            //设置当前结点显示的控件
        //            panelControlValueContrls.Controls.Add(valueInputControl);

        //            #region 根据类型动态设置可选择条件
        //            switch (ean.AtomType)
        //            {
        //                case EmrAtomType.String:
        //                case EmrAtomType.Integer:
        //                case EmrAtomType.Double:
        //                case EmrAtomType.Time:
        //                case EmrAtomType.Area:
        //                case EmrAtomType.Volume:
        //                case EmrAtomType.Length:
        //                    cmbCondition.Properties.Items.Add("=");
        //                    cmbCondition.Properties.Items.Add(">");
        //                    cmbCondition.Properties.Items.Add("<");
        //                    cmbCondition.Properties.Items.Add(">=");
        //                    cmbCondition.Properties.Items.Add("<=");
        //                    cmbCondition.Properties.Items.Add("!=");
        //                    break;
        //                default:
        //                    break;
        //            }
        //            if (cmbCondition.Properties.Items.Count > 0)
        //                cmbCondition.SelectedIndex = 0;
        //            #endregion
        //        }
        //    }
        //}

        //#endregion

        //#region IStartPlugIn 成员

        //public IPlugIn Run(YidanSoft.FrameWork.WinForm.Plugin.IYidanEmrHost host)
        //{
        //    _app = host;
        //    PlugIn plg = new PlugIn(this.GetType().ToString(), this);

        //    return plg;
        //}

        //#endregion

        //#region 模型树相关

        //private void treeListEmbed_BeforeExpand(object sender, BeforeExpandEventArgs e)
        //{
        //    if (e.Node.Nodes.Count < 1)
        //    {
        //        if (e.Node.Tag == null) return;
        //        string guid = e.Node.Tag.ToString();
        //        EmrEmbededMoleNode embedNode = GetEmbededNode(guid);
        //        InitEmbedNode(embedNode, treeListEmbed, e.Node);
        //    }

        //}

        //private void treeListEmbed_GetSelectImage(object sender, GetSelectImageEventArgs e)
        //{
        //    if (e.Node.ParentNode == null)
        //        e.NodeImageIndex = 3;
        //    else if (e.Node.Tag is EmrObject)
        //    {
        //        e.NodeImageIndex = 4;
        //    }
        //    else
        //        e.NodeImageIndex = 5;
        //}

        private void treeListSolution_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if ((e.Node == null) || (e.Node.Tag == null)) return;

            string guid = e.Node.Tag.ToString();
            if (string.IsNullOrEmpty(guid)) return;
            listBoxCond.Items.Clear();
            listBoxCond.Items.Add("主要诊断(西医诊断)/主要诊断/主要诊断 = " + e.Node.GetValue(0).ToString());
            //NowSearchMasterplate = LoadConditions(guid);
            //ShowConditionToListBox(listBoxCond, NowSearchMasterplate);
        }

        private void treeListEmbed_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            TreeListNode modelEmbeTreeNode = e.Node;

            if (e.Node.RootNode.Tag != null)
            {
                treeListCurrentEmbed.BeginUnboundLoad();
                treeListCurrentEmbed.ClearNodes();
                string guid = e.Node.RootNode.Tag.ToString();
                //EmrEmbededMoleNode embed = GetEmbededNode(guid);
                //InitEmbedNode(embed, treeListCurrentEmbed, null);
                //treeListCurrentEmbed.Tag = embed;

                
            }


        }


        //private void treeListCurrentEmbed_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        //{
        //    if (e.Node==null)
        //    {
        //        return;
        //    }
        //    ShowAtomNodeWidget(e.Node);
        //    if (listBoxCond.SelectedItem != null)
        //    {
        //        EmrQueryCondition SelectCondition = listBoxCond.SelectedItem as EmrQueryCondition;
        //        if (SelectCondition.EmrQueryNode != null && SelectCondition.EmrQueryNode.NodeType == EmrNodeType.AtomNode)
        //        {
        //            //TODO
        //            cmbCondition.Text = SelectCondition.Compare;
        //            cmbLogic.Text = SelectCondition.CondP != null ? SelectCondition.CondP.Logic : SelectCondition.Logic;

        //            //???? 低层提供的控件，不知道怎么赋值
        //            //Control[] controls = pnlConditionControl.Controls.Find("ValueInputControl", false);
        //            //if (controls.Length > 0)
        //            //    controls[0].Text = SelectCondition.EmrQueryNode.DisplayValue;
        //        }
        //    }
        //}

        //private void treeListSolution_GetSelectImage(object sender, GetSelectImageEventArgs e)
        //{
        //    if (e.Node.ParentNode == null)
        //        e.NodeImageIndex = 0;
        //    else
        //        e.NodeImageIndex = 1;
        //}

        //#endregion

        //#region common events
        private void QuyeryConsole_Load(object sender, EventArgs e)
        {
            InitData();
            //this.Activate();
        }

        //private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{

        //}

        //private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (NowSearchMasterplate==null)
        //    {
        //        return;
        //    }//add by dxj 2011/6/24
        //    SetWaitDialogCaption("正在查询数据");
        //    QueryInpatient();
        //    HideWaitDialog();
        //}

        //private void barButtonItemDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (gridView1.FocusedRowHandle < 0) return;
        //    DataRow fourcesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
        //    if (fourcesRow == null) return;
        //    fourcesRow.Delete();
        //}

        //private void barButtonItemExportWord_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    saveFileDialog1.Filter = "Text Files | *.txt";
        //    saveFileDialog1.DefaultExt = "txt";
        //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //        gridControl1.ExportToText(saveFileDialog1.FileName);
        //}

        //private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    saveFileDialog1.Filter = "html Files | *.html";
        //    saveFileDialog1.DefaultExt = "txt";
        //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //        gridControl1.ExportToHtml(saveFileDialog1.FileName);

        //}

        //private void barButtonItemExportExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    saveFileDialog1.Filter = "Excel Files | *.xls";
        //    saveFileDialog1.DefaultExt = "Excel";
        //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        //        gridControl1.ExportToXls(saveFileDialog1.FileName);

        //}

        //private void barButtonItemRead_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    if (gridView1.FocusedRowHandle < 0) return;
        //    DataRow fourcesRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
        //    if (fourcesRow == null) return;

        //    EprBrower brower = new EprBrower(fourcesRow["NoOfInpat"].ToString());
        //    brower.ShowDialog();

        //}

        //private void barButtonNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    listBoxCond.Items.Clear();
        //    NowSearchMasterplate = new EmrQueryCondition();
        //    treeListCurrentEmbed.FocusedNode = null;
        //}
        #endregion




        public IPlugIn Run(IYidanEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            _app = host;
            //QueryService = host.SqlHelper;
            return plg;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (NowSearchMasterplate == null)
            //{
            //    return;
            //}//add by dxj 2011/6/24
            SetWaitDialogCaption("正在查询数据");
            QueryInpatient();
            HideWaitDialog();
        }

        private void barButtonNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            listBoxCond.Items.Clear();
            //NowSearchMasterplate = new EmrQueryCondition();
            //treeListCurrentEmbed.FocusedNode = null;
        }

        private void treeListCurrentEmbed_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
             if (e.Node==null)
                 return;

             this.lookUpEditorDiag.Visible = true;

        }

        private void listBoxCond_ValueMemberChanged(object sender, EventArgs e)
        {
            StringBuilder text = new StringBuilder();
 
            foreach(ListBox.ObjectCollection obj in listBoxCond.Items )
            {
            text.Append(obj.ToString());
            }

            txtCondition.Text = text.ToString();
        }

        private void listBoxCond_ControlAdded(object sender, ControlEventArgs e)
        {
            StringBuilder text = new StringBuilder();

            foreach (ListBox.ObjectCollection obj in listBoxCond.Items)
            {
                text.Append(obj.ToString());
            }

            txtCondition.Text = text.ToString();
        }


    }
}
