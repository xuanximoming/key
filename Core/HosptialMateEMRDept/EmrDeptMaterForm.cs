using DevExpress.XtraBars;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.HosptialMateEMRDept
{
    public partial class EmrDeptMaterForm : Form, IStartPlugIn
    {
        #region vars
        IEmrHost m_app;
        DataTable m_HosptialDept;
        DataTable m_EmrDepts;
        string m_currentHisDeptid;
        #endregion

        #region proper
        public DataTable EmrDept2Hosptial
        {

            get
            {

                return _emrDept2Hosptial;
            }

        }
        private DataTable _emrDept2Hosptial;

        public DataTable EmrMyTemplet
        {
            get
            {
                return _emrMyTemplet;
            }
        }
        private DataTable _emrMyTemplet;


        /// <summary>
        /// 模板文件夹
        /// </summary>
        public DataTable ModelContainers
        {
            get
            {
                if (_modelContainers == null)
                {

                    _modelContainers = m_app.SqlHelper.ExecuteDataTable("select CCODE,CNAME,CTYPE,OPEN_FLAG from DICT_CATALOG where CTYPE = '2' and (UTYPE = '3' or UTYPE = '1') and CNAME<>'子女病程记录'");

                }
                return _modelContainers;
            }
        }
        private DataTable _modelContainers;

        /// <summary>
        /// 模板列表
        /// </summary>
        public DataTable ModelList
        {
            get
            {
                if (_modelList == null)
                {
                    //_modelList = m_app.SqlHelper.ExecuteDataTable("select templet_id, file_name, path, dept_id, creator_id, create_datetime, last_time, permission, mr_class, mr_code, mr_name, mr_attr, qc_code, content_code, visibled, new_page_flag, sno, change_topic_flag, parent_id, parent_modify_datetime, supper_id, file_flag, write_times,hospital_code from EMR_TEMPLET_INDEX where   file_flag >= 2 order by CREATE_DATETIME");
                    //yxy 加载模板值改掉               
                    string sql = @"  select templet_id TEMPLETID,
                                            file_name,
                                            dept_id,
                                            creator_id,
                                            create_datetime,
                                            last_time,
                                            permission,
                                            mr_class,
                                            mr_code,
                                            mr_name,
                                            mr_attr,
                                            qc_code,
                                            new_page_flag,
                                            file_flag,
                                            write_times,
                                            hospital_code
                                       from emrtemplet
                                        where valid='1'
                                     /* where file_flag >= 2 */
                                      order by CREATE_DATETIME";//筛掉已经在模板工厂里删除的
                    _modelList = m_app.SqlHelper.ExecuteDataTable(sql);
                }
                return _modelList;
            }
            set
            {
                _modelList = value;
            }
        }
        private DataTable _modelList;

        #endregion


        public EmrDeptMaterForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化科室列表
        /// </summary>
        private void InitDeptID()
        {
            lookUpWindowDept.SqlHelper = m_app.SqlHelper;
            DataTable dept = m_HosptialDept.Copy();
            dept.Columns["ID"].Caption = "科室编号";
            dept.Columns["NAME"].Caption = "科室名称";

            Dictionary<string, int> cols = new Dictionary<string, int>();
            cols.Add("ID", 65);
            cols.Add("NAME", 100);

            SqlWordbook deptWorkBook = new SqlWordbook("querybook", dept, "ID", "NAME", cols, "ID//NAME//");
            lookUpEditorDept.SqlWordbook = deptWorkBook;
        }

        #region 模板操作相关


        private void InitDeptTemplets()
        {
            listView1.Clear();
            foreach (DataRow row in _emrMyTemplet.Rows)
            {
                ListViewItem item = new ListViewItem(row["MR_NAME"].ToString());
                item.Tag = row;
                listView1.Items.Add(item);
            }

        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="hissid"></param>
        private void Add2MyTemplet(string id, string name, string hissid)
        {
            //ListViewItem item
            //check exits
            DataRow[] rows = EmrMyTemplet.Select("TEMPLETID='" + id + "'");
            if (rows.Length > 0) return;
            DataRow newRow = EmrMyTemplet.NewRow();
            newRow["TEMPLETID"] = id;
            newRow["MR_NAME"] = name;
            newRow["HIS_DEPT_ID"] = hissid;
            EmrMyTemplet.Rows.Add(newRow);
            ListViewItem item = new ListViewItem(name);

            item.Tag = newRow;
            listView1.Items.Add(item);
        }

        /// <summary>
        /// 按照节点添加模板
        /// </summary>
        /// <param name="node"></param>
        private void Add2MyTemplet(TreeListNode node)
        {
            if ((node == null) || (node.Tag == null)) return;

            if (node.HasChildren)
            {
                foreach (TreeListNode nd in node.Nodes)
                {
                    if (node.Tag == null) continue;
                    DataRow row = (DataRow)nd.Tag;
                    Add2MyTemplet(row["TEMPLETID"].ToString(), row["MR_NAME"].ToString(), m_currentHisDeptid);
                    nd.Visible = false;
                }
            }
            else
            {
                if (node.Tag is DataRow)
                {
                    DataRow row = (DataRow)node.Tag;
                    Add2MyTemplet(row["TEMPLETID"].ToString(), row["MR_NAME"].ToString(), m_currentHisDeptid);
                    node.Visible = false;
                }
            }

        }



        /// <summary>
        /// 将模板从已配模板中移除 "<"
        /// </summary>
        /// <param name="item"></param>
        private void RemoveFromMyTemplet(ListViewItem item)
        {
            //声明配对模板记录已经删除的数据 2012-4-16 14:05:05
            if (DeleteTemplTable.Columns.Count == 0)
            {
                DataColumn ColTempletID = new DataColumn();
                ColTempletID.ColumnName = "TEMPLETID";
                DeleteTemplTable.Columns.Add(ColTempletID);
                DataColumn ColHis_DeptID = new DataColumn();
                ColHis_DeptID.ColumnName = "HIS_DEPT_ID";
                DeleteTemplTable.Columns.Add(ColHis_DeptID);
                DataColumn ColMrName = new DataColumn();
                ColMrName.ColumnName = "MR_NAME";
                DeleteTemplTable.Columns.Add(ColMrName);
            }

            DataRow row = (DataRow)item.Tag;
            TreeListNode treenode = treeList_Template.FindNodeByFieldValue("ID", row["TEMPLETID"].ToString());
            if (treenode != null)
            {
                treenode.Visible = true;
                treenode.Selected = true;
            }

            //将删除的行存进新表 edit by ywk 2012年4月16日13:56:18
            if (DeleteTemplTable.Columns.Count == 2)//先删一个保存，再删除
            {
                DataColumn ColMrName = new DataColumn();
                ColMrName.ColumnName = "MR_NAME";
                DeleteTemplTable.Columns.Add(ColMrName);
            }
            DeleteTemplTable.Rows.Add(row.ItemArray);

            //treeList_Template.FindNodeByFieldValue("KeyID", dt_row["Mr_Class"].ToString() + dt_row["Dept_Id"].ToString())


            //foreach (DataRow dr in EmrMyTemplet.Rows)
            //{
            //    if (dr["TEMPLETID"] == row["TEMPLETID"])
            //    {
            //        EmrMyTemplet.Rows.Remove(dr);
            //        break;
            //    }
            //}

            row.Delete();
            //row.Table.AcceptChanges();
            listView1.Items.Remove(item);
        }


        private void MakeTempleteTree(string depid)
        {

            treeList_Template.BeginUnboundLoad();
            treeList_Template.ClearNodes();

            foreach (DataRow catalogRow in ModelContainers.Rows)
            {

                TreeListNode catalogNode = treeList_Template.AppendNode(new object[] { catalogRow["CNAME"].ToString(), catalogRow["CCODE"].ToString() }, null);
                catalogNode.Tag = "catalog";

                //读取模板
                ModelList.DefaultView.RowFilter = "DEPT_ID='" + depid + "' and MR_CLASS='" + catalogRow["CCODE"] + "'";

                DataTable table = ModelList.DefaultView.ToTable();
                //设置叶子节点
                foreach (DataRow leafRow in table.Rows)
                {
                    TreeListNode leafNode = treeList_Template.AppendNode(new object[] { leafRow["mr_name"].ToString(), leafRow["TEMPLETID"] }, catalogNode);
                    leafNode.Tag = leafRow;

                    foreach (DataRow row in _emrMyTemplet.Rows)
                    {
                        //row存在删除状态，访问会出错！！！
                        if (!row.RowState.ToString().Equals("Deleted"))
                        {
                            if (row["TEMPLETID"].Equals(leafRow["TEMPLETID"]))
                            {
                                leafNode.Visible = false;
                            }
                        }
                    }
                }
            }
            treeList_Template.EndUnboundLoad();

        }


        /// <summary>
        /// /待配模板>已配模板
        /// edit by ywk 2012年9月6日 14:25:19
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add3_Click(object sender, EventArgs e)
        {
            if (treeList_Template.FocusedNode == null)
            {
                m_app.CustomMessageBox.MessageShow("请先选择要操作的模板");
                return;
            }
            if (string.IsNullOrEmpty(m_currentHisDeptid))
            {
                m_app.CustomMessageBox.MessageShow("请先选择HIS科室");
                return;
            }
            //修改未选中具体模板，报错的BUG add by ywk 2012年9月6日 14:33:11
            if (treeList_Template.FocusedNode.Tag.GetType().FullName.ToString() != "System.Data.DataRow")
            {
                m_app.CustomMessageBox.MessageShow("请选择具体的模板进行匹配");
                return;
            }
            //System.Data.DataRow

            DataRow dr = (DataRow)treeList_Template.FocusedNode.Tag;
            Add2MyTemplet(treeList_Template.FocusedNode);

            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Text == dr["MR_NAME"].ToString())
                {
                    listView1.Items[i].Selected = true;
                    listView1.Focus();
                    break;
                }
            }
        }
        /// <summary>
        /// /待配模板>>已配模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddAll3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_currentHisDeptid))
            {
                m_app.CustomMessageBox.MessageShow("请先选择HIS科室");
                return;
            }

            foreach (TreeListNode nd in treeList_Template.Nodes)
                Add2MyTemplet(nd);
        }
        /// <summary>
        /// /已配模板 -> 待配模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Remove3_Click(object sender, EventArgs e)
        {
            if (listView1.FocusedItem == null)
            {
                m_app.CustomMessageBox.MessageShow("请先选择要操作的模板");
                return;
            }
            RemoveFromMyTemplet(listView1.FocusedItem);
        }
        /// <summary>
        /// 已配模板->>待配模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RemoveAll3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                RemoveFromMyTemplet(item);
            }
        }
        /// <summary>
        /// 配对科室向电子病历科室(方向<<)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RemoveAll1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_currentHisDeptid))
            {
                m_app.CustomMessageBox.MessageShow("请先选择HIS科室");
                return;
            }
            //foreach (TreeListNode nd in treeList_Dept.Nodes)
            ////foreach (TreeListNode nd in treeList_MyDepts.Nodes)
            // RemoveFromMyDepts(nd);
            //更改为for循环 2012年2月24日17:38:16
            for (int i = treeList_MyDepts.Nodes.Count - 1; i >= 0; i--)
            {
                RemoveFromMyDepts(treeList_MyDepts.Nodes[i]);
            }
        }

        //DataRow DeletedRow = new DataRow();

        /// <summary>
        /// 移除配对科室列表
        /// </summary>
        /// <param name="node"></param>
        private void RemoveFromMyDepts(TreeListNode node)
        {
            //声明配对科室记录已经删除的数据  edit by ywk 2012年4月16日11:23:42 
            if (DeletedTable.Columns.Count == 0)
            {
                DataColumn ColEMRDeptID = new DataColumn();
                ColEMRDeptID.ColumnName = "EMR_DEPT_ID";
                DeletedTable.Columns.Add(ColEMRDeptID);
                DataColumn ColDeptName = new DataColumn();
                ColDeptName.ColumnName = "DEPT_NAME";
                DeletedTable.Columns.Add(ColDeptName);
                DataColumn ColHISDeptID = new DataColumn();
                ColHISDeptID.ColumnName = "HIS_DEPT_ID";
                DeletedTable.Columns.Add(ColHISDeptID);
            }


            // DataRow row = (DataRow)node.Tag;
            //DataRow[] rows = EmrDept2Hosptial.Select("EMR_DEPT_ID='" + row["EMR_DEPT_ID"] + "'");
            //if (rows.Length > 0)
            //DeletedTable = EmrDept2Hosptial.Clone();此处克隆。删除多个不可累加，所以声明了全局变量的DT edit by ywk

            DataRow row = (DataRow)node.Tag;

            if (DeletedTable.Columns.Count == 2)//消除先删一个保存，再接着删除，不存在已删除列的BUG
            {
                DeletedTable.Columns.Clear();//清空数据
                DeletedTable.Rows.Clear();
                DataColumn ColEMRDeptID = new DataColumn();
                ColEMRDeptID.ColumnName = "EMR_DEPT_ID";
                DeletedTable.Columns.Add(ColEMRDeptID);
                DataColumn ColDeptName = new DataColumn();
                ColDeptName.ColumnName = "DEPT_NAME";
                DeletedTable.Columns.Add(ColDeptName);
                DataColumn ColHISDeptID = new DataColumn();
                ColHISDeptID.ColumnName = "HIS_DEPT_ID";
                DeletedTable.Columns.Add(ColHISDeptID);

            }
            DeletedTable.Rows.Add(row.ItemArray);//将删除的行存进新表

            row.Delete();
            treeList_MyDepts.Nodes.Remove(node);
            // treeList_MyDepts.Nodes.Remove(node);
            //EmrDept2Hosptial.Rows.Remove(row);
        }
        #endregion


        #region 操作 科室树
        private void Add2MyDepts(string emrid, string emrName, string hisid)
        {
            //added by ywk 2012年3月8日14:39:14
            string sql = string.Format(@" select * from emrdept2his where emr_dept_id
                ='{0}' and his_dept_id='{1}' ", emrid, hisid);
            DataTable Mydt = m_app.SqlHelper.ExecuteDataTable(sql);//获得datatable
            if (Mydt.Rows.Count > 0)
            {
                //已存在的对应关系，不重复加入
                //m_app.CustomMessageBox.MessageShow("对不起，该对应关系已经存在！");
                return;
            }

            DataRow[] rows = EmrDept2Hosptial.Select("EMR_DEPT_ID='" + emrid + "'");
            if (rows.Length > 0) return;

            treeList_MyDepts.BeginUnboundLoad();

            DataRow newRow = EmrDept2Hosptial.NewRow();
            newRow["EMR_DEPT_ID"] = emrid;
            newRow["dept_name"] = emrName;
            newRow["HIS_DEPT_ID"] = hisid;
            EmrDept2Hosptial.Rows.Add(newRow);

            TreeListNode node = treeList_MyDepts.AppendNode(new object[] { newRow["dept_name"] }, null);
            node.Tag = newRow;

            treeList_MyDepts.EndUnboundLoad();
        }

        private void Add2MyDepts(TreeListNode node)
        {

            if ((node == null) || (node.Tag == null)) return;

            if (node.HasChildren)
            {
                foreach (TreeListNode nd in node.Nodes)
                {
                    if (node.Tag == null) continue;
                    DataRow row = (DataRow)nd.Tag;
                    Add2MyDepts(row["DEPT_ID"].ToString(), row["DEPT_NAME"].ToString(), m_currentHisDeptid);
                }
            }
            else
            {
                DataRow row = (DataRow)node.Tag;
                Add2MyDepts(row["DEPT_ID"].ToString(), row["DEPT_NAME"].ToString(), m_currentHisDeptid);
            }
        }



        /// <summary>
        /// 读取his与科室对应信息
        /// </summary>
        /// <param name="deptid"></param>
        private void GetHIS2EMRDEPTS(string deptid)
        {
            //去除重复项
            _emrDept2Hosptial = m_app.SqlHelper.ExecuteDataTable(string.Format("select  distinct   a.EMR_DEPT_ID,b.dept_name,a.HIS_DEPT_ID FROM EMRDEPT2HIS a,EMRDEPT b WHERE a.EMR_DEPT_ID=b.dept_id and HIS_DEPT_ID='{0}'", deptid));
            m_app.SqlHelper.ResetTableSchema(EmrDept2Hosptial, "EMRDEPT2HIS");
            InitChoosedDepts();
            _emrMyTemplet = m_app.SqlHelper.ExecuteDataTable(string.Format("select  distinct  a.TEMPLETID,a.HIS_DEPT_ID,b.MR_NAME from TEMPLET2HISDEPT a,emrtemplet b  where a.TEMPLETID=B.TEMPLET_ID and a.His_Dept_Id='{0}' and b.valid='1' ", deptid));//有效(且去重复)
            m_app.SqlHelper.ResetTableSchema(EmrMyTemplet, "TEMPLET2HISDEPT");
            InitDeptTemplets();
        }

        private void InitChoosedDepts()
        {
            if (EmrDept2Hosptial == null) return;

            treeList_MyDepts.BeginUnboundLoad();
            treeList_MyDepts.ClearNodes();
            foreach (DataRow row in EmrDept2Hosptial.Rows)
            {
                TreeListNode node = treeList_MyDepts.AppendNode(new object[] { row["dept_name"] }, null);
                node.Tag = row;
            }

            treeList_MyDepts.EndUnboundLoad();

        }

        private void InitDept()
        {
            //读取临床科室
            m_HosptialDept = m_app.SqlHelper.ExecuteDataTable("select distinct a.ID,a.NAME from DEPARTMENT  a ,Dept2ward b where a.ID=b.deptid");
            InitDeptID();
            gridControl1.DataSource = m_HosptialDept;

            //读取emr科室
            m_EmrDepts = m_app.SqlHelper.ExecuteDataTable("select DEPT_ID,DEPT_NAME from EMRDEPT");

            InitEmrDeptTree();

            //默认选中当前用户所在科室
            lookUpEditorDept.CodeValue = m_app.User.CurrentDeptId;
        }

        private void InitEmrDeptTree()
        {
            treeList_Dept.BeginUnboundLoad();
            treeList_Dept.ClearNodes();
            DataRow[] rows = m_EmrDepts.Select("Len(DEPT_ID)<3");

            foreach (DataRow row in rows)
            {
                MakeTree(row, null);
            }
            treeList_Dept.EndUnboundLoad();
        }

        private void MakeTree(DataRow row, TreeListNode parentNode)
        {
            TreeListNode node = null;//
            node = treeList_Dept.AppendNode(new object[] { row["DEPT_NAME"].ToString(), row["DEPT_ID"].ToString() }, parentNode);
            node.Tag = row;
            DataRow[] rows;
            if (!row["DEPT_ID"].ToString().Equals("*"))
            {   //03子节点
                rows = m_EmrDepts.Select("DEPT_ID <>'" + row["DEPT_ID"].ToString() + "' and DEPT_ID like '" + row["DEPT_ID"].ToString() + "%'");
                foreach (DataRow myRow in rows)
                {
                    //todo 
                    MakeTree(myRow, node);
                }

            }

        }
        /// <summary>
        /// ">按钮" 事件(电子病历科室>配对科室)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Add1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_currentHisDeptid))
            {
                m_app.CustomMessageBox.MessageShow("请先选择HIS科室");
                return;
            }
            Add2MyDepts(treeList_Dept.FocusedNode);

        }

        private void btn_AddAll1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_currentHisDeptid))
            {
                m_app.CustomMessageBox.MessageShow("请先选择HIS科室");
                return;
            }
            foreach (TreeListNode nd in treeList_Dept.Nodes)
                Add2MyDepts(nd);

        }
        /// <summary>
        /// 将配对科室移除 "<"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Remove1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(m_currentHisDeptid))
            {
                m_app.CustomMessageBox.MessageShow("请先选择HIS科室");
                return;
            }

            //都移除后，不可再次移除 edit by 2012年4月17日8:46:29
            if (treeList_MyDepts.Nodes.Count == 0)
            {
                m_app.CustomMessageBox.MessageShow("请先选择HIS科室");
                return;
            }
            RemoveFromMyDepts(treeList_MyDepts.FocusedNode);
        }



        //private void treeList_MyDepts_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        //{
        //    if ((e.Node == null) || (e.Node.Tag == null)) return;
        //    //捞取对应的模板列表
        //    DataRow row = (DataRow)e.Node.Tag;
        //    MakeTempleteTree(row["EMR_DEPT_ID"].ToString());

        //}

        private void treeList_MyDepts_Click(object sender, EventArgs e)
        {
            TreeListNode node = treeList_MyDepts.FocusedNode;
            if ((node == null) || (node.Tag == null)) return;

            //捞取对应的模板列表
            DataRow row = (DataRow)node.Tag;
            MakeTempleteTree(row["EMR_DEPT_ID"].ToString());
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost host)
        {
            m_app = host;
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            return plg;

        }

        //新增的表，用于存放删除的配对科室的信息数据 
        //edit by ywk
        DataTable DeletedTable = new DataTable();
        //新增的表，用于存放删除的配对模板的信息数据
        //edit by ywk 
        DataTable DeleteTemplTable = new DataTable();

        private void EmrDeptMaterForm_Load(object sender, EventArgs e)
        {
            InitDept();

            if (GetConfigValueByKey("AllMatchButtonShowFlag") == "0")
            {
                btn_AddAll1.Hide();
                btn_AddAll3.Hide();
                btn_RemoveAll1.Hide();
                btn_RemoveAll3.Hide();
            }
            else
            {
                btn_AddAll1.Show();
                btn_AddAll3.Show();
                btn_RemoveAll1.Show();
                btn_RemoveAll3.Show();
            }

            this.lookUpEditorDept.Focus();

            #region 原来的声明保存删除数据
            //////声明配对科室记录已经删除的数据  edit by ywk 2012年4月16日11:23:42 
            //DataColumn ColEMRDeptID = new DataColumn();
            //ColEMRDeptID.ColumnName = "EMR_DEPT_ID";
            //DeletedTable.Columns.Add(ColEMRDeptID);
            //DataColumn ColDeptName = new DataColumn();
            //ColDeptName.ColumnName = "DEPT_NAME";
            //DeletedTable.Columns.Add(ColDeptName);
            //DataColumn ColHISDeptID = new DataColumn();
            //ColHISDeptID.ColumnName = "HIS_DEPT_ID";
            //DeletedTable.Columns.Add(ColHISDeptID);
            //DeletedTable.PrimaryKey = new DataColumn[] { ColEMRDeptID, ColHISDeptID };

            ////声明配对模板记录已经删除的数据 2012-4-16 14:05:05
            //DataColumn ColTempletID = new DataColumn();
            //ColTempletID.ColumnName = "TEMPLETID";
            //DeleteTemplTable.Columns.Add(ColTempletID);
            //DataColumn ColHis_DeptID = new DataColumn();
            //ColHis_DeptID.ColumnName = "HIS_DEPT_ID";
            //DeleteTemplTable.Columns.Add(ColHis_DeptID);
            //DataColumn ColMrName = new DataColumn();
            //ColMrName.ColumnName = "MR_NAME";
            //DeleteTemplTable.Columns.Add(ColMrName);
            #endregion
        }


        /// <summary>
        /// 保存操作
        /// </summary>
        void SaveMyDepts()
        {
            try
            {
                //保存科室信息
                DataTable updateTable = EmrDept2Hosptial.Copy();
                //消除不关闭窗体，多次保存此处删除列的错误edit by ywk  2012年4月16日
                if (updateTable.Columns.Contains("DEPT_NAME"))
                {
                    updateTable.Columns.Remove("DEPT_NAME");
                    //updateTable.AcceptChanges();
                }
                if (DeletedTable.Columns.Contains("DEPT_NAME"))
                {
                    DeletedTable.Columns.Remove("DEPT_NAME");
                    //DeletedTable.AcceptChanges();
                }
                m_app.SqlHelper.UpdateTable(updateTable, "EMRDEPT2HIS", true, DeletedTable);
                //保存模板信息
                DataTable updateTemplet = EmrMyTemplet.Copy();
                if (updateTemplet.Columns.Contains("MR_NAME"))
                {
                    updateTemplet.Columns.Remove("MR_NAME");
                }
                if (DeleteTemplTable.Columns.Contains("MR_NAME"))
                {
                    DeleteTemplTable.Columns.Remove("MR_NAME");//新增的记录删除数据的表
                }
                m_app.SqlHelper.UpdateTable(updateTemplet, "TEMPLET2HISDEPT", true, DeleteTemplTable);
                m_app.CustomMessageBox.MessageShow("保存成功");

            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            SaveMyDepts();

        }

        private void btn_Cancle_Click(object sender, EventArgs e)
        {

        }

        private void layoutView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0) return;
            DataRow dataRow = layoutView1.GetDataRow(e.FocusedRowHandle);
            if (dataRow == null) return;

            //check
            if ((EmrDept2Hosptial != null) && (EmrDept2Hosptial.GetChanges() != null))
            {

                if (m_app.CustomMessageBox.MessageShow("数据已修改过，是否需要保存？", CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.OK)
                {
                    SaveMyDepts();
                    return;
                }
            }

            m_currentHisDeptid = dataRow["ID"].ToString();
            GetHIS2EMRDEPTS(m_currentHisDeptid);
            labelControlTip.Text = "科室：" + dataRow["NAME"].ToString().Trim();
            labelControlTip.Visible = true;
            OperDeptID = m_currentHisDeptid;
        }
        private string OperDeptID = string.Empty;
        /// <summary>
        /// 选择科室事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEditorDept_CodeValueChanged(object sender, EventArgs e)
        {
            if (lookUpEditorDept.CodeValue == null || lookUpEditorDept.CodeValue == "") return;

            string deptid = lookUpEditorDept.CodeValue;
            string deptname = lookUpEditorDept.DisplayValue;

            GetDeptFocused(deptid);
            labelControlTip.Text = "科室：" + deptname;

            OperDeptID = deptid;
            this.lookUpEditorDept.CodeValueChanged -= new System.EventHandler(this.lookUpEditorDept_CodeValueChanged);
            lookUpEditorDept.CodeValue = deptid;
            this.lookUpEditorDept.CodeValueChanged += new System.EventHandler(this.lookUpEditorDept_CodeValueChanged);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                DataRow dr = item.Tag as DataRow;
                if (dr != null)
                {
                    string templateID = dr["TEMPLETID"].ToString();
                    DataTable dt = m_app.SqlHelper.ExecuteDataTable(string.Format(m_GetTemplateBelongDept, templateID));
                    gridControlBelongToDept.DataSource = dt;
                }
            }
        }

        /// <summary>
        /// layoutView1选中科室
        /// </summary>
        /// <param name="deptid">科室id</param>
        private void GetDeptFocused(string deptid)
        {
            DataTable sourcetable = (DataTable)gridControl1.DataSource;
            DataRow[] rows = sourcetable.Select("ID='" + deptid + "'");

            if (rows.Length < 1)
            {
                return;
            }
            int index = this.layoutView1.GetRowHandle(sourcetable.Rows.IndexOf(rows[0]));
            layoutView1.Focus();
            this.layoutView1.FocusedRowHandle = index;
            labelControlTip.Visible = true;
        }

        private string m_GetTemplateBelongDept = @" select distinct  emrtemplet.mr_name, department.id, department.name,emrtemplet.templet_id TEMPID  from templet2hisdept, department, emrtemplet
                                                    where templet2hisdept.his_dept_id = department.id and emrtemplet.templet_id = templet2hisdept.templetid
                                                    and templet2hisdept.templetid = '{0}' 
                                                    and department.valid = '1' and emrtemplet.valid = '1' ";


        private string m_DeleteTemplate = @"update emrtemplet set valid='0' where  emrtemplet.templet_id = '{0}' ";
        private string m_Delete2HisDept = "delete from templet2hisdept where templetid='{0}' and his_dept_id='{1}'";
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (listView1.FocusedItem == null)
            {
                return;
            }
            else
            {
                if (listView1.SelectedItems.Count > 0)
                {
                    ListViewItem item = listView1.SelectedItems[0];
                    DataRow dr = item.Tag as DataRow;
                    if (dr != null)
                    {
                        string templateID = dr["TEMPLETID"].ToString();
                        if (templateID != "")
                        {
                            //m_app.CustomMessageBox.MessageShow(templateID);
                            //m_app.SqlHelper.ExecuteNoneQuery(string.Format(m_DeleteTemplate, templateID));

                            if (m_app.CustomMessageBox.MessageShow(string.Format("是否确认删除？"), CustomMessageBoxKind.QuestionOkCancel) == DialogResult.OK)
                            {
                                m_app.SqlHelper.ExecuteNoneQuery(string.Format(m_Delete2HisDept, templateID, OperDeptID));

                                DataTable _MyTemplet = m_app.SqlHelper.ExecuteDataTable(string.Format("select  distinct  a.TEMPLETID,a.HIS_DEPT_ID,b.MR_NAME from TEMPLET2HISDEPT a,emrtemplet b  where a.TEMPLETID=B.TEMPLET_ID and a.His_Dept_Id='{0}' and b.valid='1' ", OperDeptID));

                                //直接删除节点，数据后台删除
                                listView1.Items.Remove(item);
                                //listView1.Clear();
                                //foreach (DataRow row in _MyTemplet.Rows)
                                //{
                                //    ListViewItem item1 = new ListViewItem(row["MR_NAME"].ToString());
                                //    item1.Tag = row;
                                //    listView1.Items.Add(item1);
                                //}
                                this.gridControlBelongToDept.DataSource = null;
                            }
                        }
                    }

                    ////声明配对模板记录已经删除的数据 2012-4-16 14:05:05
                    //if (DeleteTemplTable.Columns.Count == 0)
                    //{
                    //    DataColumn ColTempletID = new DataColumn();
                    //    ColTempletID.ColumnName = "TEMPLETID";
                    //    DeleteTemplTable.Columns.Add(ColTempletID);
                    //    DataColumn ColHis_DeptID = new DataColumn();
                    //    ColHis_DeptID.ColumnName = "HIS_DEPT_ID";
                    //    DeleteTemplTable.Columns.Add(ColHis_DeptID);
                    //    DataColumn ColMrName = new DataColumn();
                    //    ColMrName.ColumnName = "MR_NAME";
                    //    DeleteTemplTable.Columns.Add(ColMrName);
                    //}

                    //DataRow row = (DataRow)item.Tag;
                    //TreeListNode treenode = treeList_Template.FindNodeByFieldValue("ID", row["TEMPLETID"].ToString());
                    //if (treenode != null)
                    //{
                    //    treenode.Visible = true;
                    //    treenode.Selected = true;
                    //}

                    ////将删除的行存进新表 edit by ywk 2012年4月16日13:56:18
                    //if (DeleteTemplTable.Columns.Count == 2)//先删一个保存，再删除
                    //{
                    //    DataColumn ColMrName = new DataColumn();
                    //    ColMrName.ColumnName = "MR_NAME";
                    //    DeleteTemplTable.Columns.Add(ColMrName);
                    //}
                    //DeleteTemplTable.Rows.Add(row.ItemArray);

                    //row.Delete();
                    //listView1.Items.Remove(item);
                }
                else
                {
                    m_app.CustomMessageBox.MessageShow("请选中要删除的模板");
                }
            }
        }
        /// <summary>
        /// 右键弹出删除菜单
        /// add by ywk 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.popupMenu1.ShowPopup(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                ListViewHitTestInfo hitInfo = listView1.HitTest(Control.MousePosition.X, Control.MousePosition.Y);
                if (hitInfo.Item != null && hitInfo.SubItem != null)
                {
                    listView1.Focus();
                    listView1.FocusedItem = hitInfo.Item;

                    this.barButtonItem2.Visibility = BarItemVisibility.Always;

                }
            }
        }

        /// <summary>
        /// 得到配置信息  ywk 2012年5月21日 11:29:14
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
    }
}
