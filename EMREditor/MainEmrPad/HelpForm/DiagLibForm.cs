using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    public partial class DiagLibForm : DevBaseForm
    {
        #region SQL

        private const string SqlDeptRep = " SELECT * FROM DEPTREP WHERE id = {0} AND parent_node <> 0  and  valid ='1' ORDER BY NODE ";//增加标识有效判断 ywk
        private const string SqlDeptRepClass = " SELECT * FROM deptrepclass WHERE valid = 'Y' ";
        /*private const string SqlDeptRep = 
            " SELECT ID, NODE, PARENT_NODE, TITLE, CONTENT FROM DEPTREP WHERE id = {0} AND parent_node <> 0 and parent_node = 1 " +
            " UNION ALL " + 
            " SELECT ID, NODE, PARENT_NODE, TITLE, CONTENT FROM DEPTREP WHERE id = {0} AND parent_node in " +
                " (SELECT node FROM DEPTREP WHERE id = {0} AND parent_node <> 0 and parent_node = 1) ";*/

        #endregion

        #region FIELD && PROPERTY
        private IEmrHost m_App;

        /// <summary>
        /// 诊断名称
        /// </summary>
        private string m_DiagName = string.Empty;

        private string m_DiagID = string.Empty;

        private string m_Content = string.Empty;

        private string m_SelectText = string.Empty;

        private bool m_IsReLoad = true;

        DataTable m_MyTree;//声明的DataTable用于绑定Treelist add by  ywk 
        DataTable m_MyTreeNode;//声明的DataTable用于绑定加载时全部的TreelistNode add by  王冀 2012 10 23
        DataTable m_FocusData;//声明的焦点所在处的数据 
        /// <summary>
        /// 已绑定节点
        /// </summary>
        private List<string> m_HaveBindNode = new List<string>();
        /// <summary>
        /// 已绑定树节点
        /// </summary>
        private List<string> m_TreeNode = new List<string>();
        /// <summary>
        /// 已绑定叶节点
        /// </summary>
        private List<string> m_LeafNode = new List<string>();
        /// <summary>
        /// 已绑定树节点
        /// </summary>
        DataTable dtMyTreeNode = new DataTable();
        /// <summary>
        /// 已绑定叶节点
        /// </summary>
        DataTable dtMyLeafNode = new DataTable();
        #endregion

        #region 关于Treelist里的节点声明
        private string nodeId = string.Empty;//点击的节点ID
        private string nodeContent = string.Empty;//点击节点的内容
        private string nodeTitle = string.Empty;//点击节点的标题
        private string nID = string.Empty;//表中的ID
        private string parentNode = string.Empty;//点击节点的父节点
        #endregion

        #region .ctor
        public DiagLibForm()
        {
            InitializeComponent();

        }

        public DiagLibForm(IEmrHost app, string name, string id)
            : this()
        {
            m_App = app;
            m_DiagName = name;
            m_DiagID = id;
            this.Text = m_DiagName;
        }
        #endregion

        #region 方法和事件


        public void SetFormName(string name, string id)
        {
            if (m_DiagID != id && id != "")
            {
                m_DiagName = name;
                m_DiagID = id;
                m_IsReLoad = true;
            }
            else
            {
                m_IsReLoad = false;
            }
            this.Text = m_DiagName;
        }

        List<TreeListNode> TreeListNodelist = new List<TreeListNode>();


        private void DiagLibForm_Load(object sender, EventArgs e)
        {
            m_SelectText = "";
            memoEditContent.Text = string.Empty;
            InitLookUpEditor();
            //InitTreeList();
            //每次加载先清除数据 
            m_MyTree = null;
            if (m_MyTree == null)
            {
                m_MyTree = m_App.SqlHelper.ExecuteDataTable(string.Format(SqlDeptRep, m_DiagID), CommandType.Text);
            }
            treeListDiagRep.ClearNodes();
            //DataRow[] rows = m_MyTree.Select("ID ='1' ");//所有父节点
            m_HaveBindNode.Clear();
            foreach (DataRow dr in m_MyTree.Rows)
            {
                if (!m_HaveBindNode.Contains(dr["NODE"].ToString()))
                {
                    LoadTreeList(dr["NODE"].ToString(), dr, null);
                    //LoadTreeList(dr["PARENT_NODE"].ToString(), dr, null);
                }
            }
            m_HaveBindNode.Clear();
            this.txtQuery.Focus();
            //InitTreeList();
        }
        /// <summary>
        /// 加载Treelist中的数据
        /// 王冀 2012 10 23
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="node"></param>
        private void LoadTreeList(string id, DataRow row, TreeListNode node)
        {
            //
            TreeListNode nd = treeListDiagRep.AppendNode(new object[] { row["TITLE"], "Folder", row["CONTENT"], row["PARENT_NODE"] }, node);
            m_HaveBindNode.Add(row["NODE"].ToString());
            nd.Tag = row;
            //查找子节点
            DataRow[] leafRows = m_MyTree.Select("PARENT_NODE='" + id + "'");
            if (leafRows.Length > 0)
            {
                foreach (DataRow leftrow in leafRows)
                {
                    TreeListNode leafnd = treeListDiagRep.AppendNode(new object[] { leftrow["TITLE"], "Leaf", leftrow["CONTENT"], leftrow["NODE"], leftrow["ID"] }, nd);
                    leafnd.Tag = leftrow;
                    m_HaveBindNode.Add(leftrow["NODE"].ToString());
                    m_LeafNode.Add(leftrow["NODE"].ToString());
                    TreeListNodelist.Add(leafnd);
                }
            }

        }

        private bool CheckKey(string key)
        {
            if (key.Length > 16)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("关键字长度不能超过16个英文字符或者8个中文字符!");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 关键字查找
        /// 王冀 2012 10 23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnQurey_Click(object sender, EventArgs e)
        {
            try
            {
                string key = this.txtQuery.Text.Trim();
                string sql;
                if (key == null || key.Trim() == string.Empty)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入关键字!");
                    this.txtQuery.Focus(); return;
                }
                if (CheckKey(key))
                {
                    this.txtQuery.Focus(); return;
                }
                //过滤特殊字符串 add by ywk 2013年4月26日11:25:03 
                string keyStr = DS_Common.FilterSpecialCharacter(key);

                sql = string.Format(@"select * from DEPTREP where  id = {0} AND valid ='1' and title like '%{1}%' ORDER BY NODE ", m_DiagID, keyStr);

                m_MyTreeNode = null;
                m_MyTreeNode = m_App.SqlHelper.ExecuteDataTable(sql, CommandType.Text);

                treeListDiagRep.ClearNodes();
                m_HaveBindNode.Clear();
                foreach (DataRow dr in m_MyTreeNode.Rows)
                {
                    if (!m_HaveBindNode.Contains(dr["NODE"].ToString()))
                    {
                        if (m_LeafNode.Contains(dr["node"].ToString()))
                        {
                            LoadLeafList(dr["NODE"].ToString(), dr, null);
                        }
                        else
                        {
                            LoadTreeList(dr["NODE"].ToString(), dr, null);
                        }
                    }
                }
                m_HaveBindNode.Clear();
                txtQuery.Text = "";
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 向上加载Treelist中的数据
        /// 王冀 2012 10 23
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="node"></param>
        private void LoadLeafList(string id, DataRow row, TreeListNode node)
        {
            TreeListNode TreeListNodeRight = null;
            foreach (TreeListNode item in TreeListNodelist)
            {
                DataRow dataRow = item.Tag as DataRow;
                if (dataRow["NODE"].ToString() == id && dataRow["id"].ToString() == row["id"].ToString())
                {
                    TreeListNodeRight = item;//找到参数所在的节点
                    break;
                }
            }
            //向上找到根节点
            TreeListNode rnd = TreeListNodeRight.RootNode;
            DataRow dr = (DataRow)rnd.Tag;
            TreeListNode nd = treeListDiagRep.AppendNode(new object[] { dr["TITLE"], "Folder", dr["CONTENT"], dr["PARENT_NODE"] }, node);
            m_HaveBindNode.Add(dr["NODE"].ToString());
            nd.Tag = dr;
            //向下查找子节点
            DataRow[] leafRows = m_MyTree.Select("PARENT_NODE='" + dr["NODE"].ToString() + "'");
            if (leafRows.Length > 0)
            {
                foreach (DataRow leftrow in leafRows)
                {
                    TreeListNode leafnd = treeListDiagRep.AppendNode(new object[] { leftrow["TITLE"], "Leaf", leftrow["CONTENT"], leftrow["NODE"], leftrow["ID"] }, nd);
                    leafnd.Tag = leftrow;
                    m_HaveBindNode.Add(leftrow["NODE"].ToString());
                    m_LeafNode.Add(leftrow["NODE"].ToString());
                }
            }
        }


        /// <summary>
        /// 设置菜单图片
        /// 王冀 2012 10 23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListDiagRep_GetStateImage(object sender, GetStateImageEventArgs e)
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
        /// <summary>
        /// 
        /// 王冀 2012 10 23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListDiagRep_AfterExpand(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            if (e.Node.HasChildren)
                e.Node.ExpandAll();
        }

        private void InitTreeList()
        {
            if (m_IsReLoad)
            {
                //DataTable dt = m_App.SqlHelper.ExecuteDataTable(string.Format(SqlDeptRep, m_DiagID), CommandType.Text);
                //treeListDiagRep.BeginUnboundLoad();
                //treeListDiagRep.DataSource = dt;
                //treeListDiagRep.EndUnboundLoad();
                //treeListDiagRep.CollapseAll();

            }
        }

        private void InitLookUpEditor()
        {
            DataTable dt = m_App.SqlHelper.ExecuteDataTable(SqlDeptRepClass, CommandType.Text);
            lookUpEditType.Properties.DataSource = dt;
            lookUpEditType.Properties.ValueMember = "ID";
            lookUpEditType.Properties.DisplayMember = "NAME";
            lookUpEditType.EditValue = m_DiagID;
        }

        /// <summary>
        /// 诊疗计划根目录选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                m_DiagID = lookUpEditType.EditValue.ToString().Trim();
                //提示框标题this.text改变   wj, m_DiagName不确定2012.10.22
                this.Text = m_DiagName = lookUpEditType.Text;
                //InitTreeList();

                m_TreeNode.Clear();
                m_LeafNode.Clear();
                DiagLibForm_Load(null, null);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 选中节点改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListDiagRep_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            try
            {
                // 新增完成后再次点击所取得的值不正确 
                if (e.Node != null)
                {

                    //DataTable focusDT = FocusedData(nodeId, parentNode, nID);
                    //if (focusDT.Rows.Count > 0)
                    //{
                    //    memoEditContent.Text = focusDT.Rows[0]["Content"].ToString();
                    //}
                    //memoEditContent.Text = focusDT.Rows[0]["Content"].ToString();
                    //m_Content = e.Node.GetValue("CONTENT").ToString();
                    //if (e.Node.GetValue("PARENT_NODE") != null &&
                    //     e.Node.GetValue("NODE") != null &&
                    //     e.Node.GetValue("ID") != null)
                    //{
                    //    parentNode = e.Node.GetValue("PARENT_NODE").ToString();
                    //    nodeId = e.Node.GetValue("NODE").ToString();
                    //    nID = e.Node.GetValue("ID").ToString();

                    //}

                    if (null != e.Node && null != e.Node.Tag && e.Node.Tag is DataRow)
                    {
                        DataRow row = (DataRow)e.Node.Tag;
                        if (null == row)
                        {
                            return;
                        }
                        m_Content = row["CONTENT"].ToString();
                        memoEditContent.Text = m_Content;
                        memoEditContent.EditValue = m_Content;
                    }

                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        private void simpleButtonExit_Click(object sender, EventArgs e)
        {
            m_SelectText = "";
            this.Close();
        }

        private void simpleButtonInsert1_Click(object sender, EventArgs e)
        {
            m_SelectText = memoEditContent.Text;
            this.Close();
        }

        private void simpleButtonInsert2_Click(object sender, EventArgs e)
        {
            m_SelectText = memoEditContent.Text;
            this.Close();
        }

        /// <summary>
        /// 得到选中的诊断信息
        /// </summary>
        /// <returns></returns>
        public string GetDiag()
        {
            return m_SelectText;
        }
        #endregion

        private void barManager1_QueryShowPopupMenu(object sender, DevExpress.XtraBars.QueryShowPopupMenuEventArgs e)
        {
            if (e.Control != treeListDiagRep)
                e.Cancel = true;

        }

        /// <summary>
        /// 右键弹出菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListDiagRep_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeListHitInfo hitInfo = treeListDiagRep.CalcHitInfo(e.Location);
                if (hitInfo != null && hitInfo.Node != null)
                {
                    treeListDiagRep.Focus();
                    treeListDiagRep.FocusedNode = hitInfo.Node;
                    //focusNode = hitInfo.Node;

                    TreeListNode node = hitInfo.Node;
                    //判断父节点是否为空
                    //if (node.HasChildren)//有子节点，点击的是父节点
                    if (node.ParentNode == null)//父节点为空,本身是父节点
                    {
                        btnRight_AddCategory.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增父节点
                        btnRight_ModifyCategory.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        btnRight_DeleteCategory.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        btnRight_AddDiag.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增子节点
                        btnRight_ModifyDiag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改内容
                        btnRight_DeleteDiag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//删除
                    }
                    else
                    {
                        //判断本节点自身是否是父节点
                        btnRight_AddCategory.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增父节点
                        btnRight_ModifyCategory.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        btnRight_DeleteCategory.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        btnRight_AddDiag.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增子节点
                        btnRight_ModifyDiag.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//修改内容
                        btnRight_DeleteDiag.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//删除
                    }
                }
                popupMenu1.ShowPopup(treeListDiagRep.PointToScreen(new Point(e.X, e.Y)));
            }
        }

        /// <summary>
        /// 返回DataRow
        /// </summary>
        /// <param name="m_returnTitle"></param>
        /// <param name="m_returnContent"></param>
        /// <param name="parentNode"></param>
        /// <param name="nodeId"></param>
        /// <param name="nID"></param>
        /// <returns></returns>
        private DataRow ReturnRow(string m_returnTitle, string m_returnContent, string parentNode, string nodeId, string nID)
        {
            if (m_MyTree == null)
            {
                m_MyTree = m_App.SqlHelper.ExecuteDataTable(string.Format(SqlDeptRep, m_DiagID), CommandType.Text);
            }
            DataTable Dt = m_MyTree.Clone();
            //if (Dt.Rows.Count>0)
            //{
            DataRow m_row = Dt.NewRow();
            m_row["TITLE"] = m_returnTitle;
            m_row["CONTENT"] = m_returnContent;
            m_row["NODE"] = nodeId;
            m_row["ID"] = nID;
            m_row["PARENT_NODE"] = parentNode;
            //}
            return m_row;

        }

        private void btnAddParentNode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        #region  处理FocusNode
        public DataTable FocusedData(string nodeid, string parentNode, string nID)
        {
            string sql = string.Format(@"select content,title from DEPTREP where node='{0}' and parent_node='{1}' and 
                                    id='{2}' and valid='1'", nodeId, parentNode, nID);
            m_FocusData = m_App.SqlHelper.ExecuteDataTable(sql);

            return m_FocusData;
        }
        #endregion

        /// <summary>
        /// 清空
        /// 王冀 2012 10 23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.txtQuery.Text = "";
        }
        /// <summary>
        /// 重置
        /// 王冀 2012 10 23
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonReset1_Click(object sender, EventArgs e)
        {
            try
            {
                DiagLibForm_Load(sender, e);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 左侧菜单右键事件
        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_AddCategory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TreeListNode focusNode = treeListDiagRep.FocusedNode;
                if (this.treeListDiagRep.FocusedNode != null)
                {
                    if (null == focusNode || null == focusNode.Tag)
                    {
                        return;
                    }
                    DataRow row = (DataRow)focusNode.Tag;
                    string DiagID = lookUpEditType.EditValue.ToString();
                    AddParentNode addParent = new AddParentNode(m_App, EditState.Add);
                    addParent.InitNode(DiagID, Convert.ToInt32(row["node"]), Convert.ToInt32(row["parent_node"]), row["title"].ToString());
                    addParent.StartPosition = FormStartPosition.CenterParent;//弹出窗体在父窗体中间
                    if (addParent.ShowDialog() == DialogResult.OK)
                    {
                        string m_returnTitle = string.Empty;//返回标题
                        string m_returnNode = string.Empty;//返回节点
                        string m_returnPNode = string.Empty;//返回父节点
                        addParent.SetTitleContent(out m_returnTitle);
                        TreeListNode leafnode = treeListDiagRep.AppendNode(new object[] { m_returnTitle, "Folder", addParent.RPNode, addParent.RNode, addParent.RNode }, null);
                        leafnode.ImageIndex = 2;
                        DataRow m_row = ReturnRow(m_returnTitle, "", addParent.RPNode, addParent.RNode, nID);
                        leafnode.Tag = m_row;
                        treeListDiagRep.FocusedNode = leafnode;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_ModifyCategory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TreeListNode focusNode = treeListDiagRep.FocusedNode;
                if (this.treeListDiagRep.FocusedNode != null)
                {
                    if (null == focusNode || null == focusNode.Tag)
                    {
                        return;
                    }
                    DataRow row = (DataRow)focusNode.Tag;
                    AddParentNode addParent = new AddParentNode(m_App, EditState.Edit);
                    addParent.InitNode(row["id"].ToString(), Convert.ToInt32(row["node"]), Convert.ToInt32(row["parent_node"]), row["title"].ToString());
                    addParent.StartPosition = FormStartPosition.CenterParent;//弹出窗体在父窗体中间
                    if (addParent.ShowDialog() == DialogResult.OK)
                    {
                        row["title"] = addParent.Title;
                        focusNode.SetValue(0, addParent.Title);
                        focusNode.Tag = row;
                        treeListDiagRep.FocusedNode = focusNode;

                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_DeleteCategory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TreeListNode focusNode = treeListDiagRep.FocusedNode;
                if (null == focusNode || null == focusNode.Tag)
                {
                    return;
                }
                if (null != focusNode.Nodes && focusNode.Nodes.Count > 0)
                {

                    MyMessageBox.Show("该节点下存在子节点，无法删除。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
                DataRow row = (DataRow)focusNode.Tag;
                if (MyMessageBox.Show("您确定要删除分类 " + row["title"].ToString() + " 么？", "提示", MyMessageBoxButtons.OkCancel, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Cancel)
                {
                    return;
                }
                PatRecUtil prUtil = new PatRecUtil(m_App, m_App.CurrentPatientInfo);
                prUtil.DeleteParentNode(row["id"].ToString(), Convert.ToInt32(row["node"]), Convert.ToInt32(row["parent_node"]));
                this.treeListDiagRep.DeleteNode(focusNode);
                MyMessageBox.Show("删除成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 新增诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_AddDiag_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TreeListNode focusNode = treeListDiagRep.FocusedNode;
                parentNode = ((DataRow)focusNode.Tag)["PARENT_NODE"].ToString();
                nodeId = ((DataRow)focusNode.Tag)["NODE"].ToString();
                nID = ((DataRow)focusNode.Tag)["ID"].ToString();

                nodeContent = treeListDiagRep.FocusedNode.GetValue("CONTENT").ToString();
                nodeTitle = treeListDiagRep.FocusedNode.GetValue("TITLE").ToString();
                string DiagID = lookUpEditType.EditValue.ToString();

                string m_returnTitle = string.Empty;//返回标题
                string m_returnContent = string.Empty;//返回内容

                NewChildNode newChild = new NewChildNode(nodeTitle, nodeContent, nodeId, nID, parentNode, m_App, DiagID);
                newChild.StartPosition = FormStartPosition.CenterParent;//弹出窗体在父窗体中间
                if (newChild.ShowDialog() == DialogResult.OK)
                {
                    newChild.SetTitleContent(out m_returnTitle, out m_returnContent);
                    TreeListNode leafnode = treeListDiagRep.AppendNode(new object[] { m_returnTitle, "Leaf", m_returnContent, parentNode, nodeId, nID }, focusNode);//focusNode

                    DataRow m_row = ReturnRow(m_returnTitle, m_returnContent, newChild.RPNode, newChild.RNode, nID);
                    treeListDiagRep.FocusedNode = leafnode;
                    leafnode.Tag = m_row;
                    leafnode.SetValue("TITLE", m_returnTitle);
                    memoEditContent.Text = m_returnContent;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 修改诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_ModifyDiag_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TreeListNode focusNode = treeListDiagRep.FocusedNode;
                if (focusNode != null && null != focusNode.Tag)
                {
                    DataRow row = (DataRow)focusNode.Tag;
                    nodeTitle = row["TITLE"].ToString();
                    nodeContent = row["CONTENT"].ToString();

                    parentNode = row["PARENT_NODE"].ToString();
                    nodeId = row["NODE"].ToString();
                    nID = row["ID"].ToString();

                    string r_Content = string.Empty;//返回的内容
                    string r_Title = string.Empty;//返回的内容
                    ItemContent itemConte = new ItemContent(nodeTitle, nodeContent, nodeId, nID, parentNode, m_App);
                    itemConte.SetTitleContent();
                    itemConte.StartPosition = FormStartPosition.CenterParent;//弹出窗体在父窗体中间

                    if (itemConte.ShowDialog() == DialogResult.OK)
                    {
                        DataTable focusDT = FocusedData(nodeId, parentNode, nID);
                        if (focusDT.Rows.Count > 0)
                        {
                            memoEditContent.Text = focusDT.Rows[0]["Content"].ToString();
                            focusNode.SetValue("TITLE", focusDT.Rows[0]["TITLE"].ToString());
                        }
                        row["TITLE"] = itemConte.NodeTitle;
                        row["CONTENT"] = itemConte.NodeContent;
                        focusNode.Tag = row;
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 删除诊断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRight_DeleteDiag_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //将立刻新增后的删除有错
                TreeListNode focusNode = treeListDiagRep.FocusedNode;
                if (null == focusNode || null == focusNode.Tag)
                {
                    return;
                }
                DataRow row = (DataRow)focusNode.Tag;
                parentNode = row["PARENT_NODE"].ToString();
                nodeId = row["NODE"].ToString();
                nID = row["ID"].ToString();
                if (MyMessageBox.Show("您确定要删除分类 " + row["title"].ToString() + " 么？", "提示", MyMessageBoxButtons.OkCancel, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.QuestionIcon) == DialogResult.Cancel)
                {
                    return;
                }
                m_App.SqlHelper.ExecuteNoneQuery(string.Format("update DEPTREP set valid='0' where id='{0}' and node='{1}' and parent_node='{2}'", nID, nodeId, parentNode));
                this.treeListDiagRep.DeleteNode(this.treeListDiagRep.FocusedNode);
                MyMessageBox.Show("删除成功", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.InformationIcon);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

    }


}