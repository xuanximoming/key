using DevExpress.XtraBars;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.Permission
{
    public partial class UCJobsManager : DevExpress.XtraEditors.XtraUserControl
    {
        PermissionPublic m_PubOperator = new PermissionPublic();

        private IEmrHost m_App;

        public UCJobsManager(IEmrHost app)
        {
            InitializeComponent();
            m_App = app;
        }

        /// <summary>
        /// Modify by xlb 2013-04-07
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCJobManager_Load(object sender, EventArgs e)
        {
            try
            {
                FillTreeListJobsAll();//窗口加载显示岗位信息
                FillTreeListPermission();//窗口加载显示权限节点信息
                //ClearUI();
                this.treeListJobsAll.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListJobsAll_FocusedNodeChanged);
                #region Add by xlb 2013-04-07  屏蔽右键菜单
                ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
                DS_Common.CancelMenu(groupControlBasicInfo, contextMenuStrip1);
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 填充岗位信息
        /// </summary>
        private void FillTreeListJobsAll()
        {
            try
            {
                treeListJobsAll.DataSource = m_PubOperator.GetDepartInfo().Tables[0].DefaultView;
                treeListJobsAll.FocusedNode = null;
                //TODO:若有一列别选中，右侧上方显示选中列岗位的基本信息，下方显示选中岗位拥有的权限

                m_App.PublicMethod.ConvertTreeListDataSourceUpper(treeListJobsAll);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 权限控制填充控制节点

        /// <summary>
        /// 权限控制填充控制节点
        /// </summary>
        private void FillTreeListPermission()
        {
            try
            {
                DataTable dtPermission = new DataTable();
                DataColumn dcOID = new DataColumn("KeyFieldName", Type.GetType("System.String"));
                dcOID.ReadOnly = true;
                DataColumn dcParentOID = new DataColumn("ParentFieldName", Type.GetType("System.String"));
                dcParentOID.ReadOnly = true;
                DataColumn dcNodeName = new DataColumn("NodeName", Type.GetType("System.String"));
                dcNodeName.ReadOnly = true;
                DataColumn dcNodeCode = new DataColumn("NodeCode", Type.GetType("System.String"));
                dcNodeCode.ReadOnly = true;
                //DataColumn dcOthers = new DataColumn("Others", Type.GetType("System.String"));
                dtPermission.Columns.Add(dcOID);
                dtPermission.Columns.Add(dcParentOID);
                dtPermission.Columns.Add(dcNodeName);
                dtPermission.Columns.Add(dcNodeCode);
                //dtPermission.Columns.Add(dcOthers);

                XmlDocument xmlDoc = new XmlDocument();
                string path = Application.StartupPath + "\\file.menu";//获取file.menu路径
                xmlDoc.Load(path);//读取file.menu

                XmlElement xmlElement = xmlDoc.DocumentElement;
                XmlNodeList xmlNodeList = xmlElement.ChildNodes;

                foreach (XmlNode xmlNode in xmlNodeList)
                {
                    string nodeName = xmlNode.Attributes["caption"].Value;
                    DataRow newRow = dtPermission.NewRow();
                    newRow["KeyFieldName"] = nodeName;
                    newRow["ParentFieldName"] = DBNull.Value;
                    newRow["NodeName"] = nodeName;
                    newRow["NodeCode"] = DBNull.Value;
                    //newRow["Others"] = nodeName;

                    dtPermission.Rows.Add(newRow);
                    dtPermission.AcceptChanges();
                    if (xmlNode.HasChildNodes)
                    {
                        GetNodes(xmlNode, dtPermission);
                    }
                }
                treeListPermission.DataSource = dtPermission.DefaultView;
                treeListPermission.ParentFieldName = "ParentFieldName";
                treeListPermission.KeyFieldName = "KeyFieldName";
                treeListPermission.PopulateColumns();
                treeListPermission.BestFitColumns();
                treeListPermission.ExpandAll();
                treeListPermission.OptionsView.ShowCheckBoxes = true;
                foreach (DevExpress.XtraTreeList.Columns.TreeListColumn column in treeListPermission.Columns)
                {
                    column.OptionsColumn.AllowEdit = false;
                    if (column.FieldName != "NodeName")
                        column.VisibleIndex = -1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 循环遍历xml文件，填充节点
        /// </summary>
        /// <param name="xmlNode">xml节点</param>
        /// <param name="dataTable">获得新行的表</param>
        public void GetNodes(XmlNode xmlNode, DataTable dataTable)
        {
            try
            {
                if (!xmlNode.HasChildNodes)
                {
                    return;
                }
                foreach (XmlElement nodes in xmlNode.ChildNodes)
                {
                    string nodesValue = nodes.Attributes["class"].Value + "_" + nodes.Attributes["library"].Value;
                    string nodesName = nodes.Attributes["caption"].Value;
                    DataRow row = dataTable.NewRow();
                    row["KeyFieldName"] = nodesName;
                    row["ParentFieldName"] = xmlNode.Attributes["caption"].Value;
                    row["NodeName"] = nodesName;
                    row["NodeCode"] = nodesValue;
                    //row["Others"] = nodesName;

                    dataTable.Rows.Add(row);
                    dataTable.AcceptChanges();
                    GetNodes(nodes, dataTable);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        Boolean IsHaveNodeFocus = false;

        /// <summary>
        /// 选中一个岗位节点，显示岗位基本信息和岗位的授权信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListJobsAll_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                IsEnable(false);//编辑栏不可用
                if (e.Node == null)
                {
                    return;
                }
                if (e.Node.Focused)
                {
                    //填充岗位基本信息
                    DataSet dsJobs = m_PubOperator.GetDepartInfo();

                    //将datatable字段转换为大写
                    m_App.PublicMethod.ConvertDataSourceUpper(dsJobs.Tables[0]);

                    foreach (DataRow row in dsJobs.Tables[0].Rows)
                    {
                        if (row["ID"].ToString() == e.Node.GetValue("ID").ToString())
                        {
                            textEditJob.Text = row["TITLE"].ToString();
                            textEditJobID.Text = row["ID"].ToString();
                            memoEditMemo.Text = row["MEMO"].ToString();
                        }
                    }

                    //勾选权限控制信息
                    if (textEditJobID.Text != "")
                    {
                        FillJobTree(textEditJobID.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 对岗位信息进行编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {

            try
            {
                //string messages = m_PubOperator.UpdateJob(textEditJob.Text.Trim(), textEditJobID.Text.Trim(), memoEditMemo.Text.Trim());
                //MessageBox.Show(messages);
                ClearUI();
                IsEnable(true);
                m_IsEdit = false;
                textEditJob.Focus();
                //simpleButtonEdit.Enabled = false;//点击新增后，修改按钮不可用edit by ywk 2012年9月18日 11:17:43
                //simpleButtonDelete.Enabled = false;//wangj  2013 3 5
                return;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private Boolean m_IsEdit = false;
        /// <summary>
        /// 新增岗位信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Isenable)
                {
                    return;
                }
                if (textEditJob.Text.Trim() == string.Empty || textEditJobID.Text.Trim() == string.Empty)
                {
                    m_App.CustomMessageBox.MessageShow("请输入岗位名称和代码！");
                    return;
                }
                if (m_IsEdit == false)
                {
                    if (textEditJobID.Text != string.Empty && textEditJob.Text != String.Empty)
                    {
                        string messages = m_PubOperator.InsertJob(textEditJobID.Text.Trim(), textEditJob.Text.Trim(), memoEditMemo.Text.Trim());

                        m_App.CustomMessageBox.MessageShow(messages);
                        this.textEditJobID.Focus();//提示错误后焦点对上 edit by ywk 2012年9月18日 11:15:35
                    }
                    else
                    {
                        m_App.CustomMessageBox.MessageShow("请输入岗位名称和代码！");
                    }
                    FillTreeListJobsAll();
                }
                else
                {
                    string messages = m_PubOperator.UpdateJob(textEditJob.Text.Trim(), textEditJobID.Text.Trim(), memoEditMemo.Text.Trim());
                    m_App.CustomMessageBox.MessageShow(messages);
                }
                IsEnable(false);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        #region 控制节点CheckState

        /// <summary>
        /// 节点打钩状态的切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListPermission_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            try
            {
                if (e.Node == null)
                {
                    return;
                }
                System.Data.DataRowView rov = treeListPermission.GetDataRecordByNode(e.Node) as System.Data.DataRowView;
                if (e.Node.CheckState == CheckState.Indeterminate)
                {
                    e.Node.CheckState = CheckState.Checked;
                }
                if (e.Node.CheckState == CheckState.Checked)
                {
                    if (e.Node.HasChildren)
                    {
                        CheckNode(e.Node);
                    }
                    ValidParentIsChecked(e.Node);
                }
                else if (e.Node.CheckState == CheckState.Unchecked)
                {
                    if (e.Node.HasChildren)
                    {
                        ClearNode(e.Node);
                    }
                    if (e.Node.ParentNode != null)
                        e.Node.ParentNode.CheckState = CheckState.Indeterminate;
                    //从根节点往下 查询 是否有打钩的子节点，如果有那么 父节点的 半选状态不变否则变为 不选择状态
                    ValidParentNodeIsCanleSel(e.Node);
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void CheckNode(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            try
            {
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode colNode in node.Nodes)
                {
                    if (colNode != null)
                    {
                        colNode.CheckState = CheckState.Checked;
                    }
                    if (colNode.HasChildren)
                    {
                        CheckNode(colNode);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 递归调用自己设置CheckBox状态
        /// </summary>
        /// <param name="node"></param>
        private void ClearNode(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            try
            {
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode colNode in node.Nodes)
                {
                    if (colNode != null)
                    {
                        colNode.CheckState = CheckState.Unchecked;
                    }
                    if (colNode.HasChildren)
                    {
                        ClearNode(colNode);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据子节点CheckBox是否全选状态更改父节点不确定状态
        /// </summary>
        /// <param name="node"></param>
        private void ValidParentIsChecked(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            try
            {
                if (node.ParentNode != null)
                {
                    if (node.ParentNode.CheckState != CheckState.Checked)
                    {
                        node.ParentNode.CheckState = CheckState.Indeterminate;
                    }
                    ValidParentIsChecked(node.ParentNode);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据子节点CheckBox是否全选状态更改父节点非选中状态
        /// </summary>
        /// <param name="node"></param>
        private void ValidParentNodeIsCanleSel(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            try
            {
                bool isSel = false;
                if (node.ParentNode != null)
                {
                    if (node.ParentNode.CheckState == CheckState.Indeterminate)
                    {
                        //如果父节点的状态为半选状态，根据父节点判断子节点是否打钩
                        isSel = ValidIsHasCheckChildNode(node.ParentNode);
                        if (isSel == false)
                        {
                            //如果所有的子节点都没有选中，那么父节点的状态变为非选中状态
                            node.ParentNode.CheckState = CheckState.Unchecked;
                        }
                    }
                    ValidParentNodeIsCanleSel(node.ParentNode);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 判断 子节点 是否 有 状态为 “选中”状态 
        /// true 表示有 false 表示为 没有
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ValidIsHasCheckChildNode(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            try
            {
                bool isCheck = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode colNode in node.Nodes)
                {
                    if (colNode != null)
                    {
                        if (colNode.CheckState == CheckState.Checked)
                        {
                            isCheck = true;
                            return isCheck;
                        }
                    }
                    if (colNode.HasChildren)
                    {
                        isCheck = ValidIsHasCheckChildNode(colNode);
                        if (isCheck == true)
                        {
                            return isCheck;
                        }
                    }
                }
                return isCheck;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 权限控制节点打钩
        /// </summary>
        /// <param name="jobID"></param>
        private void FillJobTree(string jobId)
        {
            try
            {
                //根据JOBID获取权限集合FILL 至 DATATABLE里
                ClearJobTree();
                DataTable dataTable = m_PubOperator.GetJobPermissionInfo(jobId).Tables[0];
                foreach (TreeListNode node in this.treeListPermission.Nodes)
                {
                    foreach (TreeListNode subNode in node.Nodes)
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            if (subNode.GetValue("NodeName").ToString().Trim() == row["Modulename"].ToString().Trim())
                                subNode.Checked = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 清空所有CheckBox选中状态
        /// </summary>
        private void ClearJobTree()
        {
            try
            {
                foreach (TreeListNode node in this.treeListPermission.Nodes)
                {
                    node.Checked = false;
                    foreach (TreeListNode subNode in node.Nodes)
                    {
                        subNode.Checked = false;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 删除岗位信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string jobID = textEditJobID.Text.Trim();
                if (string.IsNullOrEmpty(jobID))
                {
                    m_App.CustomMessageBox.MessageShow("请选择岗位！");
                    return;
                }
                else
                {
                    if (m_App.CustomMessageBox.MessageShow("确认要删除岗位" + textEditJobID.Text.Trim() + "？", CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                    {
                        m_PubOperator.DelDepart(textEditJobID.Text.Trim());
                        FillTreeListJobsAll();
                        ClearUI();
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 保存授权信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                m_App.SqlHelper.BeginTransaction();
                string jobId = this.textEditJobID.Text.Trim();
                m_PubOperator.DeleteJobPermission(jobId, m_App.SqlHelper);

                if (jobId != "")
                {
                    foreach (TreeListNode node in this.treeListPermission.Nodes)
                    {
                        foreach (TreeListNode subNode in node.Nodes)
                        {
                            string nodeName = subNode.GetValue("NodeName").ToString();
                            string nodeCode = subNode.GetValue("NodeCode").ToString();
                            //事务实现，3个参数循环插入数据库勾选权限
                            if (subNode.Checked)
                                m_PubOperator.SetPermission(jobId, nodeName, nodeCode, m_App.SqlHelper);
                            //m_App.SqlHelper

                        }
                    }
                    m_App.CustomMessageBox.MessageShow("权限授权成功！");
                    m_App.SqlHelper.CommitTransaction();
                }
                else//如果不选择岗位，就直接授权，则返回edit by ywk 2012年9月18日 11:12:47
                {
                    m_App.CustomMessageBox.MessageShow("请先选择岗位信息！");
                    return;
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 基本信息是否可编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ClearUI();
                IsEnable(false);

                return;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 清空UI信息
        /// </summary>
        private void ClearUI()
        {
            try
            {
                textEditJob.Text = "";
                textEditJobID.Text = "";
                memoEditMemo.Text = "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        Boolean Isenable = false;

        /// <summary>
        /// 编辑栏是否可用
        /// </summary>
        /// <param name="enable"></param>
        private void IsEnable(bool enable)
        {
            try
            {
                Isenable = enable;
                if (enable == true)
                {
                    textEditJob.Enabled = true;
                    textEditJobID.Enabled = true;
                    memoEditMemo.Enabled = true;
                    simpleButtonSave.Enabled = true;
                    //simpleButtonDelete.Enabled = true;
                    simpleButtonOK.Enabled = true;
                    simpleButtonCancel.Enabled = true;
                    simpleButtonEdit.Enabled = false;
                    simpleButtonDelete.Enabled = false;// wangj 2013 3 5
                }
                else
                {
                    textEditJob.Enabled = false;
                    textEditJobID.Enabled = false;
                    memoEditMemo.Enabled = false;
                    simpleButtonSave.Enabled = false;
                    //simpleButtonDelete.Enabled = false;
                    simpleButtonOK.Enabled = false;
                    simpleButtonCancel.Enabled = false;
                    simpleButtonEdit.Enabled = true;
                    simpleButtonDelete.Enabled = true;// wangj 2013 3 5
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void simpleButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (textEditJobID.Text.Trim() == "")
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选中要编辑的岗位");
                    return;
                }
                IsEnable(true);
                textEditJobID.Enabled = false;
                m_IsEdit = true;
                //string messages = m_PubOperator.UpdateJob(textEditJob.Text.Trim(), textEditJobID.Text.Trim(), memoEditMemo.Text.Trim());
                //m_App.CustomMessageBox.MessageShow(messages);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void barButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ClearUI();
                IsEnable(true);
                m_IsEdit = false;
                textEditJob.Focus();
                return;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void barButtonItemDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                m_PubOperator.DelDepart(textEditJobID.Text.Trim());
                ClearUI();
                FillTreeListJobsAll();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        private void barButtonItemEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                IsEnable(true);
                textEditJobID.Enabled = false;
                m_IsEdit = true;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 右击弹出菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListJobsAll_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hit = treeListJobsAll.CalcHitInfo(new Point(e.X, e.Y));
                barButtonItemAdd.Visibility = BarItemVisibility.Always;
                barButtonItemDelete.Visibility = BarItemVisibility.Always;
                barButtonItemEdit.Visibility = BarItemVisibility.Always;
                if (e.Button == MouseButtons.Right)
                {
                    if (hit.Node != null)
                    {
                        barButtonItemAdd.Visibility = BarItemVisibility.Always;
                        barButtonItemDelete.Visibility = BarItemVisibility.Always;
                        barButtonItemEdit.Visibility = BarItemVisibility.Always;
                        treeListJobsAll.FocusedNode = hit.Node;
                        treeListJobsAll.Focus();
                        popupMenu1.ShowPopup(treeListJobsAll.PointToScreen(new Point(e.X, e.Y)));
                    }
                    else
                    {
                        barButtonItemAdd.Visibility = BarItemVisibility.Never;
                        barButtonItemDelete.Visibility = BarItemVisibility.Never;
                        barButtonItemEdit.Visibility = BarItemVisibility.Never;
                    }
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
        }
    }
}
