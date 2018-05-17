using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HelpForm
{
    public partial class DiagLibForm : DevExpress.XtraEditors.XtraForm
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
        DataTable m_FocusData;//声明的焦点所在处的数据
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

        private List<string> m_HaveBindNode = new List<string>();
        private void DiagLibForm_Load(object sender, EventArgs e)
        {
            m_SelectText = "";
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
            //InitTreeList();
        }
        /// <summary>
        /// 加载Treelist中的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="node"></param>
        private void LoadTreeList(string id, DataRow row, TreeListNode node)
        {

            //TreeListNode nd = treeListDiagRep.AppendNode(new object[] 
            //{ row["TITLE"], row["CONTENT"], row["PARENT_NODE"], row["NODE"], row["ID"] }, node);
            //nd.Tag = row;
            ////查找子节点
            //DataRow[] leafRows = m_MyTree.Select("PARENT_NODE='" + id + "'");
            //if (leafRows.Length > 0)
            //{
            //    foreach (DataRow leftrow in leafRows)
            //    {
            //        TreeListNode leafnd = treeListDiagRep.AppendNode(new object[]
            //        { leftrow["TITLE"], leftrow["CONTENT"], leftrow["PARENT_NODE"], leftrow["NODE"], leftrow["ID"] }, nd);
            //        leafnd.Tag = leftrow;
            //    }
            //}
            TreeListNode nd = treeListDiagRep.AppendNode(new object[] { row["TITLE"], row["CONTENT"], row["PARENT_NODE"] }, node);
            m_HaveBindNode.Add(row["NODE"].ToString());
            nd.Tag = row;
            //查找子节点
            DataRow[] leafRows = m_MyTree.Select("PARENT_NODE='" + id + "'");
            if (leafRows.Length > 0)
            {
                foreach (DataRow leftrow in leafRows)
                {
                    TreeListNode leafnd = treeListDiagRep.AppendNode(new object[] { leftrow["TITLE"], leftrow["CONTENT"], leftrow["NODE"], leftrow["ID"] }, nd);
                    leafnd.Tag = leftrow;
                    m_HaveBindNode.Add(leftrow["NODE"].ToString());
                }
            }
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

        private void lookUpEditType_EditValueChanged(object sender, EventArgs e)
        {
            m_DiagID = lookUpEditType.EditValue.ToString().Trim();
            //InitTreeList();
            DiagLibForm_Load(null, null);
        }

        private void treeListDiagRep_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
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


                m_Content = e.Node.GetValue("CONTENT").ToString();
                memoEditContent.Text = m_Content;
                memoEditContent.EditValue = m_Content;

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
                        btnAddParent.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增父节点
                        barButtonItemNew2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增子节点
                        barButtonItemModified2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改内容
                        barButtonItemDelet2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//删除
                    }
                    else
                    {
                        //判断本节点自身是否是父节点
                        //取得parentnode ,将他作为node再查上级node(查不出数据或者PARENT_NODE为0 则为父节点？)
                        //parentNode = treeListDiagRep.FocusedNode.GetValue("PARENT_NODE").ToString();
                        btnAddParent.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增父节点
                        barButtonItemNew2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增子节点
                        barButtonItemModified2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//修改内容
                        barButtonItemDelet2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//删除
                    }
                }
                popupMenu1.ShowPopup(treeListDiagRep.PointToScreen(new Point(e.X, e.Y)));
            }
        }
        /// <summary>
        /// 新增子分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemNew2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            TreeListNode focusNode = treeListDiagRep.FocusedNode;
            parentNode = ((DataRow)focusNode.Tag)["PARENT_NODE"].ToString();
            nodeId = ((DataRow)focusNode.Tag)["NODE"].ToString();
            nID = ((DataRow)focusNode.Tag)["ID"].ToString();

            //parentNode = treeListDiagRep.FocusedNode.GetValue("PARENT_NODE").ToString();
            //nodeId = treeListDiagRep.FocusedNode.GetValue("NODE").ToString();
            //nID = treeListDiagRep.FocusedNode.GetValue("ID").ToString();
            //TreeListNode focusNode = treeListDiagRep.FocusedNode;
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
                TreeListNode leafnode = treeListDiagRep.AppendNode(new object[] { m_returnTitle, m_returnContent, parentNode, nodeId, nID }, focusNode);//focusNode


                DataRow m_row = ReturnRow(m_returnTitle, m_returnContent, newChild.RPNode, newChild.RNode, nID);
                treeListDiagRep.FocusedNode = leafnode;
                leafnode.Tag = m_row;
                leafnode.SetValue("TITLE", m_returnTitle);
                memoEditContent.Text = m_returnContent;
            }
            //else
            //{
            //    newChild.ShowDialog();
            //}
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
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDelet2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //将立刻新增后的删除有错
            TreeListNode focusNode = treeListDiagRep.FocusedNode;
            parentNode = ((DataRow)focusNode.Tag)["PARENT_NODE"].ToString();
            nodeId = ((DataRow)focusNode.Tag)["NODE"].ToString();
            nID = ((DataRow)focusNode.Tag)["ID"].ToString();
            //parentNode = treeListDiagRep.FocusedNode.GetValue("PARENT_NODE").ToString();
            //nodeId = treeListDiagRep.FocusedNode.GetValue("NODE").ToString();
            //nID = treeListDiagRep.FocusedNode.GetValue("ID").ToString();

            m_App.SqlHelper.ExecuteNoneQuery(string.Format("update DEPTREP set valid='0' where id='{0}' and node='{1}' and parent_node='{2}'", nID, nodeId, parentNode));
            this.treeListDiagRep.DeleteNode(this.treeListDiagRep.FocusedNode);
        }
        /// <summary>
        /// 修改内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemModified2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode focusNode = treeListDiagRep.FocusedNode;
            if (this.treeListDiagRep.FocusedNode != null)
            {
                nodeTitle = treeListDiagRep.FocusedNode.GetValue("TITLE").ToString();
                nodeContent = treeListDiagRep.FocusedNode.GetValue("CONTENT").ToString();

                parentNode = ((DataRow)focusNode.Tag)["PARENT_NODE"].ToString();
                nodeId = ((DataRow)focusNode.Tag)["NODE"].ToString();
                nID = ((DataRow)focusNode.Tag)["ID"].ToString();

                string r_Content = string.Empty;//返回的内容
                string r_Title = string.Empty;//返回的内容
                ItemContent itemConte = new ItemContent(nodeTitle, nodeContent, nodeId, nID, parentNode, m_App);
                itemConte.StartPosition = FormStartPosition.CenterParent;//弹出窗体在父窗体中间

                if (itemConte.ShowDialog() == DialogResult.OK)
                {
                    DataTable focusDT = FocusedData(nodeId, parentNode, nID);
                    if (focusDT.Rows.Count > 0)
                    {
                        memoEditContent.Text = focusDT.Rows[0]["Content"].ToString();
                        focusNode.SetValue("TITLE", focusDT.Rows[0]["TITLE"].ToString());
                        //itemConte.SetTitleContent(out r_Title,out r_Content);
                        //DataRow m_row = ReturnRow(r_Title, r_Content, nodeId, parentNode, nID);
                        //focusNode.Tag = m_row;
                        //treeListDiagRep.FocusedNode = focusNode;
                    }
                }
            }
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
        /// 新增大分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddParent_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode focusNode = treeListDiagRep.FocusedNode;
            if (this.treeListDiagRep.FocusedNode != null)
            {
                string DiagID = lookUpEditType.EditValue.ToString();
                AddParentNode addParent = new AddParentNode(m_App, DiagID);
                addParent.StartPosition = FormStartPosition.CenterParent;//弹出窗体在父窗体中间
                if (addParent.ShowDialog() == DialogResult.OK)
                {
                    string m_returnTitle = string.Empty;//返回标题
                    string m_returnNode = string.Empty;//返回节点
                    string m_returnPNode = string.Empty;//返回父节点
                    addParent.SetTitleContent(out m_returnTitle);
                    //TreeListNode leafnd = treeListDiagRep.AppendNode(new object[] { leftrow["TITLE"], leftrow["CONTENT"], leftrow["PARENT_NODE"], leftrow["NODE"], leftrow["ID"] }, nd);
                    TreeListNode leafnode = treeListDiagRep.AppendNode(new object[] { m_returnTitle, "", addParent.RPNode, addParent.RNode, addParent.RNode }, null);

                    DataRow m_row = ReturnRow(m_returnTitle, "", addParent.RPNode, addParent.RNode, nID);
                    leafnode.Tag = m_row;
                    treeListDiagRep.FocusedNode = leafnode;

                }

            }
        }


    }


}