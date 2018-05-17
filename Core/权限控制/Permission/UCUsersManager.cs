using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.Core.Permission
{
    public partial class UCUsersManager : DevExpress.XtraEditors.XtraUserControl
    {
        PermissionPublic m_PubOperator = new PermissionPublic();

        private IEmrHost m_App;

        public UCUsersManager(IEmrHost app)
        {
            try
            {
                InitializeComponent();
                m_App = app;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Modify by xlb 2013-04-07
        /// 屏蔽右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCUsersManager_Load(object sender, EventArgs e)
        {
            try
            {
                DrectSoft.DSSqlHelper.DS_SqlHelper.CreateSqlHelper();
                checkedListBoxControlJobs.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(checkedListBoxControlJobs_ItemCheck);
                checkedListBoxControlpower.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(checkedListBoxControlpower_ItemCheck);
                FillTreeListPermission();//窗口加载时显示权限控制信息
                FillTreeListUsers();//窗口加载时显示所有员工
                InitDeptInfo();//加载科室信息
                InitWardInfo();//加载病区信息
                InitJobsInfo();

                InitPower();
                //InitDeptInfo();
                InitMarital();
                InitCategory();
                InitJobTitle();
                InitGrade();

                InitUserInfo();
                //初始化性别
                InitLookSex();
                #region 屏蔽右键 Add by xlb  2013-04-07
                ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();
                DrectSoft.Common.DS_Common.CancelMenu(groupControlBasicInfo, contextMenuStrip1);
                #endregion
                this.ActiveControl = lookUpEditorusers;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        private void checkedListBoxControlpower_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {

                popupContainerEditpower.EditValue = string.Empty;
                foreach (DataRowView ch in checkedListBoxControlpower.CheckedItems)
                {
                    popupContainerEditpower.EditValue += ch["name"].ToString() + ",";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 初始化性别 
        /// add by ywk 2012年4月17日9:40:40
        /// </summary>
        private void InitLookSex()
        {
            try
            {
                string sql = string.Format(@"select  name,detailid as id from dictionary_detail where categoryid='3' and valid=1");
                DataTable dtSex = m_App.SqlHelper.ExecuteDataTable(sql);
                lookUpSex.Properties.DataSource = dtSex;
                lookUpSex.Properties.DisplayMember = "NAME";
                lookUpSex.Properties.ValueMember = "ID";
                lookUpSex.EditValue = "";
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 显示所有员工
        /// </summary>
        private void FillTreeListUsers()
        {
            try
            {
                MakeTreeOne(m_PubOperator.GetUsersInfo());
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }




        /// <summary>
        /// 显示员工信息
        /// 不使用 存在问题 绑定数据源时若存在相同名科室KeyFieldName（ID）冲突
        /// Modify by xlb
        /// </summary>
        /// <param name="dataTable"></param>
        private void InitUser(DataSet ds)
        {
            try
            {
                DataTable dtdept = ds.Tables[0];

                DataTable dtuser = ds.Tables[1];

                DataTable dtUsers = new DataTable();
                DataColumn dcOID = new DataColumn("KeyFieldName", Type.GetType("System.String"));
                dcOID.ReadOnly = true;
                DataColumn dcParentOID = new DataColumn("ParentFieldName", Type.GetType("System.String"));
                dcParentOID.ReadOnly = true;
                DataColumn dcNodeName = new DataColumn("NodeName", Type.GetType("System.String"));
                dcNodeName.ReadOnly = true;
                DataColumn dcNodeCode = new DataColumn("NodeCode", Type.GetType("System.String"));
                dcNodeCode.ReadOnly = true;
                //DataColumn dcOthers = new DataColumn("Others", Type.GetType("System.String"));
                dtUsers.Columns.Add(dcOID);
                dtUsers.Columns.Add(dcParentOID);
                dtUsers.Columns.Add(dcNodeName);
                dtUsers.Columns.Add(dcNodeCode);

                foreach (DataRow drdept in dtdept.Rows)
                {
                    DataRow newRow = dtUsers.NewRow();
                    newRow["KeyFieldName"] = drdept["NAME"].ToString().Trim();
                    newRow["ParentFieldName"] = DBNull.Value;
                    newRow["NodeName"] = drdept["NAME"].ToString().Trim();
                    newRow["NodeCode"] = DBNull.Value;
                    dtUsers.Rows.Add(newRow);

                    foreach (DataRow druser in dtuser.Rows)
                    {
                        if (drdept["ID"].ToString().Trim().Equals(druser["DEPTID"].ToString().Trim()))
                        {
                            DataRow newRowusers = dtUsers.NewRow();
                            newRowusers["KeyFieldName"] = druser["ID"].ToString().Trim();
                            newRowusers["ParentFieldName"] = drdept["NAME"].ToString().Trim();
                            newRowusers["NodeName"] = druser["NAME"].ToString().Trim();
                            newRowusers["NodeCode"] = druser["ID"].ToString().Trim();
                            dtUsers.Rows.Add(newRowusers);
                        }
                    }
                }


                treeListUsers.DataSource = dtUsers;
                treeListUsers.ParentFieldName = "ParentFieldName";
                treeListUsers.KeyFieldName = "KeyFieldName";
                try
                {
                    treeListUsers.PopulateColumns();
                    treeListUsers.BestFitColumns();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                foreach (DevExpress.XtraTreeList.Columns.TreeListColumn column in treeListUsers.Columns)
                {
                    column.OptionsColumn.AllowEdit = false;//列不可编辑
                    if (column.FieldName != "NodeName")
                        column.VisibleIndex = -1;//VisibleIndex=-1时隐藏列
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 加载父节点
        /// Add by xlb 203-04-07
        /// </summary>
        /// <param name="ds"></param>
        private void MakeTreeOne(DataSet ds)
        {
            try
            {
                DataTable dtDept = ds.Tables[0];
                TreeListNode ParentNode = null;
                foreach (DataRow row in dtDept.Rows)
                {
                    ParentNode = treeListUsers.AppendNode(new object[] { row["NAME"], row["ID"], DBNull.Value, row["NAME"] }, null);
                    ParentNode.Tag = row;
                    string deptId = row["ID"].ToString(); ;
                    DataTable dtUser = GetUsersByDept(deptId);
                    MakeTreeTwo(dtUser, ParentNode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过科室编号获取该科室所有用户
        /// Add by xlb 2013-04-07
        /// </summary>
        /// <param name="deptId"></param>
        private DataTable GetUsersByDept(string deptId)
        {
            try
            {
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable(@"select *　 from users 
                where deptid=@deptId and valid='1'", new SqlParameter[] { new SqlParameter("@deptId", deptId) }, CommandType.Text);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 加载二级节点
        /// Add by xlb 2013-04-07
        /// </summary>
        /// <param name="ds"></param>
        private void MakeTreeTwo(DataTable dt, TreeListNode node)
        {
            try
            {
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return;
                }
                TreeListNode nodes = null;
                foreach (DataRow row in dt.Rows)
                {
                    nodes = treeListUsers.AppendNode(new object[] { row["NAME"], row["ID"], row["ID"], row["NAME"] }, node);
                    nodes.Tag = row;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 显示员工信息
        /// </summary>
        /// <param name="dataTable"></param>
        private void InitUser(DataTable dataTable)
        {
            try
            {
                DataTable dtUsers = new DataTable();
                DataColumn dcOID = new DataColumn("KeyFieldName", Type.GetType("System.String"));
                dcOID.ReadOnly = true;
                DataColumn dcParentOID = new DataColumn("ParentFieldName", Type.GetType("System.String"));
                dcParentOID.ReadOnly = true;
                DataColumn dcNodeName = new DataColumn("NodeName", Type.GetType("System.String"));
                dcNodeName.ReadOnly = true;
                DataColumn dcNodeCode = new DataColumn("NodeCode", Type.GetType("System.String"));
                dcNodeCode.ReadOnly = true;
                //DataColumn dcOthers = new DataColumn("Others", Type.GetType("System.String"));
                dtUsers.Columns.Add(dcOID);
                dtUsers.Columns.Add(dcParentOID);
                dtUsers.Columns.Add(dcNodeName);
                dtUsers.Columns.Add(dcNodeCode);

                //首先将dataTable里所有不同人Deptname取出至LIST
                //Dictionary<string, string> dictionaryDept = new Dictionary<string, string>(); 
                List<String> listDept = new List<string>();
                foreach (DataRow dr in dataTable.Rows)
                {
                    String deptName = dr["Deptname"].ToString();
                    String deptId = dr["ID"].ToString();
                    if (!listDept.Contains(deptName))
                    {
                        listDept.Add(deptName);
                    }
                    //if (!dictionaryDept.ContainsKey(deptId))
                    //{
                    //    dictionaryDept.Add(deptId, deptName);
                    //} 
                }
                //将dataTable里Deptname与deptName一样人DATEROW取出
                foreach (string deptName in listDept)
                {
                    DataRow newRow = dtUsers.NewRow();
                    newRow["KeyFieldName"] = deptName;
                    newRow["ParentFieldName"] = DBNull.Value;
                    newRow["NodeName"] = deptName;
                    newRow["NodeCode"] = DBNull.Value;
                    dtUsers.Rows.Add(newRow);
                    //生成父节点
                    //将dataTable里Deptname与deptName一样人DATEROW取出
                    GetUsers(dtUsers, deptName, dataTable);
                }
                treeListUsers.DataSource = dtUsers;
                treeListUsers.ParentFieldName = "ParentFieldName";
                treeListUsers.KeyFieldName = "KeyFieldName";
                treeListUsers.ExpandAll();
                treeListUsers.PopulateColumns();
                treeListUsers.BestFitColumns();
                foreach (DevExpress.XtraTreeList.Columns.TreeListColumn column in treeListUsers.Columns)
                {
                    column.OptionsColumn.AllowEdit = false;//列不可编辑
                    if (column.FieldName != "NodeName")
                        column.VisibleIndex = -1;//VisibleIndex=-1时隐藏列
                }
            }
            catch (Exception)
            {

                throw;
            }


        }

        private void GetUsers(DataTable dtPermission, String strdeptName, DataTable dataTable)
        {
            try
            {
                DataTable dataTemp = dataTable.Clone();
                dataTemp.Clear();
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["Deptname"].ToString() == strdeptName)
                    {
                        dataTemp.ImportRow(row);
                        dataTemp.AcceptChanges();
                    }
                }
                foreach (DataRow row in dataTemp.Rows)
                {
                    //递归生成TREELIST DataRow newRow = dtPermission.NewRow();
                    DataRow newRow = dtPermission.NewRow();
                    newRow["KeyFieldName"] = row["ID"].ToString();
                    newRow["ParentFieldName"] = strdeptName;
                    newRow["NodeName"] = row["Name"].ToString();
                    newRow["NodeCode"] = row["ID"].ToString();
                    dtPermission.Rows.Add(newRow);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //ClearUI();
                IsEnable(false);
                return;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 选中一个员工节点，显示基本信息、授权信息和附属账户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListUsers_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                IsEnable(false);
                if (e.Node == null)
                {
                    return;
                }
                if (e.Node.Focused)
                {
                    if (e.Node.GetValue("NodeCode").ToString() == "")
                    {

                        ClearUI();
                        ///edit zyx 2012-12-20 清空页面信息
                        gridControl1.DataSource = null;
                        m_UserId = string.Empty;
                        FillTreeListPermission();
                        return;
                    }
                    textEditName.Text = e.Node.GetValue("NodeName").ToString();
                    textEditName.Tag = e.Node.GetValue("NodeCode").ToString();
                    m_UserId = e.Node.GetValue("NodeCode").ToString();
                    textEditID.Text = m_UserId;
                    gridControl1.BeginUpdate();
                    gridControl1.DataSource = null;
                    gridControl1.EndUpdate();

                    LoadTempUsers(m_UserId);

                    //填充员工基本信息 TODO
                    DataSet dsUsers = m_PubOperator.GetUsersInfo();
                    foreach (DataRow row in dsUsers.Tables[1].Rows)
                    {
                        if (row["ID"].ToString() == textEditID.Text.Trim())
                        {
                            lookUpEditorDept.CodeValue = row["DEPTID"].ToString();
                            lookUpEditorWard.CodeValue = row["WARDID"].ToString();
                            memoEditMemo.Text = row["MEMO"].ToString();

                            lookUpSex.EditValue = row["Sexy"].ToString() == "" ? "3" : row["Sexy"].ToString();


                            deBirth.Text = row["Birth"].ToString();
                            lookUpEditorMarital.CodeValue = row["Marital"].ToString();
                            txtIDNo.Text = row["IDNo"].ToString();
                            lookUpEditorCategory.CodeValue = row["Category"].ToString();
                            lookUpEditorJobTitle.CodeValue = row["JobTitle"].ToString();
                            txtRecipeID.Text = row["RecipeID"].ToString();
                            cmbRecipeMark.SelectedIndex = Convert.ToInt32(row["RecipeMark"].ToString() == "" ? "-1" : row["RecipeMark"].ToString());
                            cbPower.SelectedIndex = Convert.ToInt32(row["Power"].ToString() == "" ? "-1" : row["Power"].ToString());
                            cmbNarcosisMark.SelectedIndex = Convert.ToInt32(row["NarcosisMark"].ToString() == "" ? "-1" : row["NarcosisMark"].ToString());
                            lookUpEditorGrade.CodeValue = row["Grade"].ToString();
                            cmbStatus.SelectedIndex = Convert.ToInt32(row["Status"].ToString() == "" ? "-1" : row["Status"].ToString());
                            InitPower();
                            FillPower(row["DEPTORWARD"].ToString());
                            FillJobID(row["JOBID"].ToString());
                            FillJobTree(row["JOBID"].ToString());
                            break;
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
        /// 权限科室显示
        /// </summary>
        /// <param name="strPower">可切换科室代码</param>
        private void FillPower(string strPower)
        {
            try
            {
                popupContainerEditpower.EditValue = string.Empty;
                int k = 0;
                while (this.checkedListBoxControlpower.GetItem(k) != null)
                {
                    DataRowView ch = checkedListBoxControlpower.GetItem(k) as DataRowView;
                    checkedListBoxControlpower.SetItemChecked(k, false);
                    for (int i = 0; i < strPower.Split(',').Length; i++)
                    {
                        string str = strPower.Split(',')[i].ToString();

                        if (ch["code"].ToString() == str)
                        {
                            checkedListBoxControlpower.SetItemChecked(k, true);
                            break;
                        }
                    }
                    k++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 判定某个岗位对应的权限信息，若有某个权限就打钩显示出来
        /// </summary>
        /// <param name="strJobID"></param>
        private void FillJobID(string strJobID)
        {
            try
            {
                int k = 0;
                while (this.checkedListBoxControlJobs.GetItem(k) != null)
                {
                    DataRowView ch = checkedListBoxControlJobs.GetItem(k) as DataRowView;
                    checkedListBoxControlJobs.SetItemChecked(k, false);
                    for (int i = 0; i < strJobID.Split(',').Length; i++)
                    {
                        string str = strJobID.Split(',')[i].ToString();

                        if (ch["code"].ToString() == str)
                        {
                            checkedListBoxControlJobs.SetItemChecked(k, true);
                            break;
                        }
                    }
                    k++;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 权限控制节点打钩

        /// <summary>
        /// 权限控制节点打钩
        /// </summary>
        /// <param name="jobIds"></param>
        private void FillJobTree(string jobIds)
        {
            try
            {
                //to do 根据JOBID获取权限集合FILL 至 DATATABLE里
                ClearJobTree();
                string[] jobIDArray = jobIds.Split(',');
                foreach (string jobID in jobIDArray)
                {
                    if (jobID.Trim() != "")
                    {
                        DataTable dataTable = m_PubOperator.GetJobPermissionInfo(jobID).Tables[0];
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
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 清空CheckBox的打钩状态
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        void checkedListBoxControlJobs_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                popupContainerEditJob.EditValue = string.Empty;
                foreach (DataRowView ch in checkedListBoxControlJobs.CheckedItems)
                {
                    popupContainerEditJob.EditValue += ch["name"].ToString() + ",";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 控制节点CheckState

        /// <summary>
        /// 父节点的选中状态，判断子节点状态
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
                    //从根节点往下查询是否有打钩的子节点，如果有那么父节点的半选状态不变否则变为不选择状态
                    ValidParentNodeIsCanleSel(e.Node);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 递归子节点打钩
        /// </summary>
        /// <param name="node"></param>
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 递归子节点去掉打钩
        /// </summary>
        /// <param name="node"></param>
        private void ClearNode(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            try
            {
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode cnode in node.Nodes)
                {
                    if (cnode != null)
                    {
                        cnode.CheckState = CheckState.Unchecked;
                    }
                    if (cnode.HasChildren)
                    {
                        ClearNode(cnode);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 递归父节点不确定状态
        /// </summary>
        /// <param name="node"></param>
        private void ValidParentIsChecked(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            try
            {
                if (node.ParentNode != null)
                {
                    if (node.ParentNode.CheckState != CheckState.Checked)
                        node.ParentNode.CheckState = CheckState.Indeterminate;
                    ValidParentIsChecked(node.ParentNode);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 子节点父节点间状态的更新
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
                        //如果父节点的 状态为 半选 状态 这 更具父节点 判断子节点是否打钩
                        isSel = ValidIsHasCheckChildNode(node.ParentNode);
                        if (isSel == false)
                        {
                            //如果所有的 子节点 都没有 “选中”那么 父节点的状态 变为 非选中状态
                            node.ParentNode.CheckState = CheckState.Unchecked;
                        }
                    }
                    ValidParentNodeIsCanleSel(node.ParentNode);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 判断 子节点 是否 有 状态为 “选中”状态 
        /// true 表示有 false 表示没有
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ValidIsHasCheckChildNode(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            try
            {
                bool isCheck = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode cnode in node.Nodes)
                {
                    if (cnode != null)
                    {
                        if (cnode.CheckState == CheckState.Checked)
                        {
                            isCheck = true;
                            return isCheck;
                        }
                    }
                    if (cnode.HasChildren)
                    {
                        isCheck = ValidIsHasCheckChildNode(cnode);
                        if (isCheck == true)
                        {
                            return isCheck;
                        }
                    }
                }
                return isCheck;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        /// <summary>
        /// 删除员工信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                m_PubOperator.DelUser(textEditID.Text.Trim());
                ClearUI();
                FillTreeListUsers();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 清空UI上员工基本信息
        /// </summary>
        private void ClearUI()
        {
            try
            {
                textEditName.Text = "";
                textEditID.Text = "";
                lookUpEditorDept.Text = "";
                lookUpEditorWard.Text = "";
                memoEditMemo.Text = "";


                //cmbSexy.SelectedIndex = -1;
                lookUpSex.EditValue = "";

                deBirth.Text = "";
                lookUpEditorMarital.CodeValue = "";
                txtIDNo.Text = "";
                lookUpEditorCategory.CodeValue = "";
                lookUpEditorJobTitle.CodeValue = "";
                txtRecipeID.Text = "";
                cbPower.SelectedIndex = -1;
                cmbRecipeMark.SelectedIndex = -1;
                cmbNarcosisMark.SelectedIndex = -1;
                lookUpEditorGrade.CodeValue = "";
                cmbStatus.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 增加新员工，不需要功能

        /// <summary>
        /// 增加新员工
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ClearUI();
                m_PubOperator.InsertUser(textEditID.Text.Trim(), textEditName.Text.Trim());
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion


        /// <summary>
        /// 重新编辑员工基本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            try
            {
                //岗位权限
                string selectJobId = string.Empty;
                foreach (DataRowView item in checkedListBoxControlJobs.CheckedItems)
                {
                    selectJobId += item["code"].ToString() + ",";
                }

                //科室权限
                string selectDeptOrWard = string.Empty;
                foreach (DataRowView item in checkedListBoxControlpower.CheckedItems)
                {
                    selectDeptOrWard += item["code"].ToString() + ",";
                }
                if (selectDeptOrWard.Length > 0)
                {
                    selectDeptOrWard = selectDeptOrWard.Substring(0, selectDeptOrWard.Length - 1);
                }

                UsersEntity users = new UsersEntity();
                users.Id = textEditID.Text.Trim();
                users.Name = textEditName.Text.Trim();
                users.Deptid = lookUpEditorDept.CodeValue.ToString();
                users.Wardid = lookUpEditorWard.CodeValue.ToString();
                users.Jobid = selectJobId;
                users.DeptOrWard = selectDeptOrWard;

                users.Sexy = lookUpSex.EditValue.ToString().Trim();

                users.Birth = deBirth.Text.ToString();
                users.Marital = lookUpEditorMarital.CodeValue;
                if (CkeckIDCardss(txtIDNo.Text.Trim().ToString()))
                {
                    users.Idno = txtIDNo.Text.Trim();
                    users.Category = lookUpEditorCategory.CodeValue;
                    users.Jobtitle = lookUpEditorJobTitle.CodeValue;
                    users.Recipeid = txtRecipeID.Text.Trim();
                    users.Recipemark = cmbRecipeMark.SelectedIndex;
                    users.Powermark = cbPower.SelectedIndex;
                    users.Narcosismark = cmbNarcosisMark.SelectedIndex;
                    users.Grade = lookUpEditorGrade.CodeValue;
                    users.Status = cmbStatus.SelectedIndex;
                    users.Memo = memoEditMemo.Text.Trim();

                    string messages = m_PubOperator.UpdateUser(users);
                    m_App.CustomMessageBox.MessageShow(messages);
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请输入合法的身份证号");
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }


        }

        /// <summary>
        /// 编辑栏是否可用
        /// </summary>
        /// <param name="enable"></param>
        private void IsEnable(bool enable)
        {
            try
            {
                if (enable == true)
                {
                    // add by yxy  将病人名称，工号，所在科室，病区信息锁定，不能更改。该信息由HIS系统更新后同步
                    popupContainerEditJob.Enabled = true;
                    memoEditMemo.Enabled = true;
                    simpleButtonReset.Enabled = true;
                    //simpleButtonDelete.Enabled = true;
                    simpleButtonCancel.Enabled = true;

                    //cmbSexy.Enabled = true;
                    lookUpSex.Enabled = true;
                    deBirth.Enabled = true;
                    lookUpEditorMarital.Enabled = true;
                    txtIDNo.Enabled = true;
                    lookUpEditorCategory.Enabled = true;
                    lookUpEditorJobTitle.Enabled = true;
                    txtRecipeID.Enabled = true;
                    cmbRecipeMark.Enabled = true;
                    cbPower.Enabled = true;
                    if (cbPower.SelectedIndex == 1) popupContainerEditpower.Enabled = true;
                    cmbNarcosisMark.Enabled = true;
                    lookUpEditorGrade.Enabled = true;
                    cmbStatus.Enabled = true;
                }
                else
                {
                    popupContainerEditJob.Enabled = false;
                    popupContainerEditpower.Enabled = false;
                    memoEditMemo.Enabled = false;
                    simpleButtonReset.Enabled = false;
                    //simpleButtonDelete.Enabled = false;
                    simpleButtonCancel.Enabled = false;

                    //cmbSexy.Enabled = false;
                    lookUpSex.Enabled = false;

                    deBirth.Enabled = false;
                    lookUpEditorMarital.Enabled = false;
                    txtIDNo.Enabled = false;
                    lookUpEditorCategory.Enabled = false;
                    lookUpEditorJobTitle.Enabled = false;
                    txtRecipeID.Enabled = false;
                    cmbRecipeMark.Enabled = false;
                    cbPower.Enabled = false;
                    cmbNarcosisMark.Enabled = false;
                    lookUpEditorGrade.Enabled = false;
                    cmbStatus.Enabled = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 基本信息编辑区不可用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                string userID = textEditID.Text.Trim();
                if (string.IsNullOrEmpty(userID))
                {
                    m_App.CustomMessageBox.MessageShow("请选择员工！");
                    return;
                }
                IsEnable(true);
                this.ActiveControl = lookUpSex;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 附属账户相关属性和操作
        const string col_UserId = "UserID";
        const string col_UserName = "Name";
        const string col_StartDate = "startdate";
        const string col_EndDate = "enddate";
        const string str_QuerySql = "select b.UserID,b.StartDate,b.EndDate,a.Name from TempUsers b "
                                        + " left Join Users a on b.UserID=a.ID where b.MasterID='{0}'";

        const string str_DeleteUserSql = "delete from Users where ID='{0}'";
        const string str_DeleteMasterSql = "delete from TempUsers where UserID='{0}' and MasterID='{1}'";
        private string m_UserId = string.Empty;

        private DataTable TempUsers
        {
            get
            {
                if (tempUsers == null)
                {
                    InitTempUserSchema();
                }
                return tempUsers;

            }
        }
        private DataTable tempUsers;

        private void InitTempUserSchema()
        {
            try
            {
                DataColumn colid = new DataColumn(col_UserId);
                DataColumn colname = new DataColumn(col_UserName);
                DataColumn colstart = new DataColumn(col_StartDate);
                DataColumn colend = new DataColumn(col_EndDate);
                tempUsers = new DataTable();
                tempUsers.Columns.Add(colid);
                tempUsers.Columns.Add(colname);
                tempUsers.Columns.Add(colstart);
                tempUsers.Columns.Add(colend);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void LoadTempUsers(string userId)
        {
            try
            {
                if (userId == null || userId == string.Empty)
                {
                    return;
                }
                string s = string.Format(str_QuerySql, userId);
                DataTable sourceTable = m_App.SqlHelper.ExecuteDataTable(s);
                if (tempUsers != null)
                {
                    tempUsers.Rows.Clear();
                }
                foreach (DataRow row in sourceTable.Rows)
                {
                    DataRow newRow = TempUsers.NewRow();
                    newRow["UserID"] = row["UserID"];
                    newRow["Name"] = row["Name"];
                    newRow["StartDate"] = row["StartDate"];
                    newRow["EndDate"] = row["EndDate"];
                    TempUsers.Rows.Add(newRow);
                }
                gridControl1.DataSource = TempUsers;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private DataRow GetFocusedRow()
        {
            try
            {
                if (gridView1.FocusedRowHandle < 0) return null;
                DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                return row;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void repositoryItemDateEdit1_QueryCloseUp(object sender, CancelEventArgs e)
        {
            try
            {
                DataRow focuseRow = GetFocusedRow();
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(m_UserId))
                {
                    NewTempUser newForm = new NewTempUser(m_App, m_UserId);
                    if (newForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadTempUsers(m_UserId);
                    }
                }
                else
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请先选择一个主用户");
                }

            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow focuseRow = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                if (focuseRow == null)
                    return;
                if (m_App.CustomMessageBox.MessageShow("确认删除附属账户？", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                {
                    m_App.SqlHelper.ExecuteNoneQuery(string.Format(str_DeleteMasterSql, focuseRow["UserID"], m_UserId));
                    m_App.SqlHelper.ExecuteNoneQuery(string.Format(str_DeleteUserSql, focuseRow["UserID"]));
                    gridView1.DeleteRow(gridView1.FocusedRowHandle);
                }
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }


        }

        #endregion


        #region 初始化页面信息

        #region 填充权限控制节点
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
                //填充权限控制树基本信息
                treeListPermission.DataSource = dtPermission.DefaultView;//数据源
                treeListPermission.ParentFieldName = "ParentFieldName";//根节点
                treeListPermission.KeyFieldName = "KeyFieldName";//下级节点
                treeListPermission.Enabled = false;//控制树所有节点为不可选  add by jxh 2013-7-22
                treeListPermission.PopulateColumns();
                treeListPermission.BestFitColumns();
                treeListPermission.ExpandAll();
                treeListPermission.OptionsView.ShowCheckBoxes = true;
                foreach (DevExpress.XtraTreeList.Columns.TreeListColumn col in treeListPermission.Columns)
                {
                    col.OptionsColumn.AllowEdit = false;//列不可编辑
                    if (col.FieldName != "NodeName")
                        col.VisibleIndex = -1;//VisibleIndex=-1时隐藏列
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 循环遍历xml文件，填充节点
        /// </summary>
        /// <param name="xmlNode">xml父亲节点</param>
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
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        /// <summary>
        /// 初始化病区
        /// </summary>
        private void InitWardInfo()
        {
            try
            {
                lookUpWindowWard.SqlHelper = m_App.SqlHelper;
                DataTable ward = m_App.SqlHelper.ExecuteDataTable("usp_SelectWard", CommandType.StoredProcedure);
                ward.Columns["ID"].Caption = "代码";
                ward.Columns["NAME"].Caption = "病区名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", lookUpEditorWard.Width / 2);
                cols.Add("NAME", 160);

                SqlWordbook wardWordBook = new SqlWordbook("query", ward, "ID", "NAME", cols);
                lookUpEditorWard.SqlWordbook = wardWordBook;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 初始化用户切换科室权限
        /// </summary>
        private void InitPower()
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("code");
                dataTable.Columns.Add("name");
                DataTable dept = m_PubOperator.GetDeptOrWard(lookUpEditorCategory.CodeValue).Tables[0];
                foreach (DataRow row in dept.Rows)
                {
                    DataRow displayRow = dataTable.NewRow();
                    displayRow["code"] = row["Id"];
                    displayRow["name"] = row["Name"];
                    dataTable.Rows.Add(displayRow);
                    dataTable.AcceptChanges();
                }
                checkedListBoxControlpower.DisplayMember = "name";
                checkedListBoxControlpower.ValueMember = "code";
                checkedListBoxControlpower.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 设置工作岗位
        /// </summary>
        private void InitJobsInfo()
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("code");
                dataTable.Columns.Add("name");
                DataTable permissionTable = m_PubOperator.GetDepartInfo().Tables[0];
                foreach (DataRow row in permissionTable.Rows)
                {
                    DataRow displayRow = dataTable.NewRow();
                    displayRow["code"] = row["Id"];
                    displayRow["name"] = row["Title"];
                    dataTable.Rows.Add(displayRow);
                    dataTable.AcceptChanges();
                }
                checkedListBoxControlJobs.DisplayMember = "name";
                checkedListBoxControlJobs.ValueMember = "code";
                checkedListBoxControlJobs.DataSource = dataTable;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 初始化科室
        /// </summary>
        private void InitDeptInfo()
        {
            try
            {
                lookUpWindowDept.SqlHelper = m_App.SqlHelper;
                DataTable dept = m_App.SqlHelper.ExecuteDataTable("usp_SelectDepartment", CommandType.StoredProcedure);
                dept.Columns["ID"].Caption = "代码";
                dept.Columns["NAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", 65);
                cols.Add("NAME", 160);

                SqlWordbook deptWorkBook = new SqlWordbook("query", dept, "ID", "NAME", cols);
                lookUpEditorDept.SqlWordbook = deptWorkBook;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 初始化用户婚姻状况
        /// </summary>
        private void InitMarital()
        {
            try
            {
                string sql = string.Format(@"select a.detailid id,a.name from Dictionary_detail a where a.categoryid = '4'");

                lookUpWindowMarital.SqlHelper = m_App.SqlHelper;

                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "代码";
                Dept.Columns["NAME"].Caption = "类型";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 65);
                cols.Add("NAME", 160);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols);
                lookUpEditorMarital.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 绑定类别
        /// </summary>
        private void InitCategory()
        {
            try
            {
                string sql = string.Format(@"select * from categorydetail a where a.CategoryID = 4");

                lookUpWindowCategory.SqlHelper = m_App.SqlHelper;

                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "代码";
                Dept.Columns["NAME"].Caption = "类型";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 65);
                cols.Add("NAME", 160);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols);
                lookUpEditorCategory.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 初始化职称
        /// </summary>
        private void InitJobTitle()
        {
            try
            {
                string sql = string.Format(@"select a.detailid id,a.name from Dictionary_detail a where a.categoryid = '40'");

                lookUpWindowJobTitle.SqlHelper = m_App.SqlHelper;

                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "代码";
                Dept.Columns["NAME"].Caption = "类型";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 65);
                cols.Add("NAME", 160);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols);
                lookUpEditorJobTitle.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        /// <summary>
        /// 医生级别
        /// </summary>
        private void InitGrade()
        {
            try
            {
                string sql = string.Format(@"select * from categorydetail a where a.CategoryID = 20");

                lookUpWindowGrade.SqlHelper = m_App.SqlHelper;

                DataTable Dept = m_App.SqlHelper.ExecuteDataTable(sql);

                Dept.Columns["ID"].Caption = "代码";
                Dept.Columns["NAME"].Caption = "类型";

                Dictionary<string, int> cols = new Dictionary<string, int>();

                cols.Add("ID", 65);
                cols.Add("NAME", 160);

                SqlWordbook deptWordBook = new SqlWordbook("querybook", Dept, "ID", "NAME", cols);
                lookUpEditorGrade.SqlWordbook = deptWordBook;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 初始化查找用户信息
        /// </summary>
        private void InitUserInfo()
        {
            try
            {
                lookUpWindowusers.SqlHelper = m_App.SqlHelper;
                DataTable dept = m_App.SqlHelper.ExecuteDataTable(@"select a.id,a.name,a.py,a.wb,a.deptid,d.name deptname from users a  
                                                                    left join department d on a.deptid = d.id");
                dept.Columns["ID"].Caption = "工号";
                dept.Columns["NAME"].Caption = "姓名";
                dept.Columns["DEPTNAME"].Caption = "科室名称";

                Dictionary<string, int> cols = new Dictionary<string, int>();
                cols.Add("ID", 65);
                cols.Add("NAME", 100);
                cols.Add("DEPTNAME", 100);

                SqlWordbook deptWorkBook = new SqlWordbook("querybook", dept, "ID", "NAME", cols, "ID//NAME//DEPTNAME//PY//WB");
                lookUpEditorusers.SqlWordbook = deptWorkBook;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// 查询框值变化 定位用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorusers_CodeValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (lookUpEditorusers.CodeValue.Trim() == "")
                    return;
                TreeListNode leafnode = treeListUsers.FindNodeByKeyID(lookUpEditorusers.CodeValue);
                treeListUsers.FocusedNode = leafnode;
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        /// <summary>
        /// 清除密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonClearPassword_Click(object sender, EventArgs e)
        {
            try
            {
                string clearPassword = " update users set passwd = 'QK+S40FfCEQ=', regdate = '2005110717:30:35' where id = '{0}' ";
                string userID = textEditID.Text.Trim();
                if (!string.IsNullOrEmpty(userID))
                {
                    if (m_App.CustomMessageBox.MessageShow("是否清空密码！", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
                    {
                        m_App.SqlHelper.ExecuteNoneQuery(string.Format(clearPassword, userID));
                        m_App.CustomMessageBox.MessageShow("密码已清空！");
                    }
                }
                else
                {
                    m_App.CustomMessageBox.MessageShow("请选择员工！");
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 验证位身份证
        ///<auth>zyx</auth>
        ///<date>2012-12-14</date>
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns>是否真实身份证</returns>
        public static bool CheckIDCards(string Id)
        {
            try
            {
                int intLen = Id.Length;
                long n = 0;

                if (intLen == 18)
                {
                    if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
                    {
                        return false;//数字验证
                    }
                    string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                    if (address.IndexOf(Id.Remove(2)) == -1)
                    {
                        return false;//省份验证
                    }
                    string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                    DateTime time = new DateTime();
                    if (DateTime.TryParse(birth, out time) == false)
                    {
                        return false;//生日验证
                    }
                    string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                    string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                    char[] Ai = Id.Remove(17).ToCharArray();
                    int sum = 0;
                    for (int i = 0; i < 17; i++)
                    {
                        sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                    }
                    int y = -1;
                    Math.DivRem(sum, 11, out y);
                    if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
                    {
                        return false;//校验码验证
                    }
                    return true;//符合GB11643-1999标准
                }
                else if (intLen == 15)
                {
                    return CheckIDCards(Per15To18(Id));
                }
                else
                {
                    return false;//位数不对
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 15位身份证号 换算 18位身份号
        ///<auth>zyx</auth>
        ///<date>2012-12-14</date>
        /// </summary>
        /// <param name="perIDSrc">15位身份证号</param>
        /// <returns></returns>
        public static string Per15To18(string perIDSrc)
        {
            try
            {
                int iS = 0;
                //加权因子常数
                int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
                //校验码常数
                string LastCode = "10X98765432";
                //新身份证号
                string perIDNew;
                perIDNew = perIDSrc.Substring(0, 6);
                //填在第6位及第7位上填上‘1’，‘9’两个数字
                perIDNew += "19";
                perIDNew += perIDSrc.Substring(6, 9);

                //进行加权求和
                for (int i = 0; i < 17; i++)
                {
                    iS += int.Parse(perIDNew.Substring(i, 1)) * iW[i];
                }

                //取模运算，得到模值
                int iY = iS % 11;
                //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。
                perIDNew += LastCode.Substring(iY, 1);
                return perIDNew;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断是否为合法的身份证号
        /// <auth>张业兴</auth>
        /// <date>2012-12-18</date>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CkeckIDCardss(string id)
        {
            try
            {
                Tool tool = new Tool();
                if (string.IsNullOrEmpty(id))
                {
                    return true;
                }
                else
                {
                    if (tool.IsNumber(id))
                    {
                        return true;
                    }
                    else
                    {
                        string str = id.Substring(0, id.Length - 1);
                        if (tool.IsNumber(str))
                        {
                            if (id.Substring(id.Length - 1).Equals("x") || id.Substring(id.Length - 1).Equals("X"))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void cbPower_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cbPower.SelectedIndex == 1 && cbPower.Enabled == true)
            {
                popupContainerEditpower.Enabled = true;
            }
            else
            {
                popupContainerEditpower.Enabled = false;
            }
        }

    }
}
