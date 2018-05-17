using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DrectSoft.Common.Ctrs.FORM;
using System.Data.OracleClient;
using DrectSoft.Service;
using DrectSoft.DSSqlHelper;
using DrectSoft.Common;

namespace DrectSoft.Core.PersonTtemTemplet
{
    public partial class PersonItemManager : DevBaseForm, IStartPlugIn
    {
        #region 属性及SQL语句
        IDataAccess sqlHelper;

        IEmrHost m_app;

        const string sql_insertCatalog = @"insert into emrtemplet_item_person_catalog
                                                  (ID, NAME, Previd, VALID, Memo, DEPTID, CONTAINER,isperson,createusers)
                                                VALUES
                                                  ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}','{7}','{8}')";

        const string sql_InsertItem = @"insert into emrtemplet_item_person(CODE,NAME,deptid,item_content,deptshare,parentid,isperson,createusers)
                                            values('{0}','{1}','{2}','{3}','{4}','{5}', '{6}','{7}')";

        const string sql_deletCatalog = "delete from emrtemplet_item_person_catalog where id='{0}'";

        //const string sql_queryTree = "select * from emrtemplet_item_person_catalog a where a.deptid='{0}' and (a.isperson = 0 or (a.isperson = 1 and a.createusers = '{1}'))";
        //处理个人小模板创建人转科，模板要看到自己创建的
        const string sql_queryTree = "select * from emrtemplet_item_person_catalog where (deptid='{0}' and isperson='0') or (isperson='1' and createusers='{1}')  and deptid='{0}' or createusers='{1}'";

        //const string sql_queryLeaf = "select * from emrtemplet_item_person a where a.deptid='{0}' and (a.isperson = 0 or (a.isperson = 1 and a.createusers = '{1}'))";
        const string sql_queryLeaf = "select * from emrtemplet_item_person a where (a.deptid='{0}' and isperson=0) or (a.isperson = 1 and a.createusers = '{1}')   and deptid='{0}' or createusers='{1}'";

        const string sql_deletLeaf = "delete from emrtemplet_item_person where Code='{0}'";
        //修改分类语句
        const string sql_updateItem = @"update emrtemplet_item_person_catalog 
                                                set name = '{1}',
                                                    memo = '{2}',
                                                    container = '{3}',
                                                    isperson  = '{4}'
                                                where ID = '{0}'";
        //更新掉分类下面的模板IsPerson字段 add by ywk 
        const string sql_updatechildItem = @"update emrtemplet_item_person set isperson='{0}'
                                             where parentid='{1}' ";

        const string sql_queryTreeByID = "select * from emrtemplet_item_person_catalog where ID = '{0}'";


        ItemCatalog m_ItemCatalog;
        ItemContentForm m_ItemContent;

        DataTable m_MyTreeFolders;
        DataTable m_MyLeafs;
        #endregion

        public PersonItemManager()
        {

        }

        public PersonItemManager(IEmrHost app)
        {
            InitializeComponent();
            m_app = app;
            m_ItemCatalog = new ItemCatalog(app);
            m_ItemContent = new ItemContentForm(app);
            sqlHelper = m_app.SqlHelper;
        }

        /// <summary>
        /// 加载数据到树控件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="node"></param>
        private void LoadTree(string id, DataRow row, TreeListNode node)
        {
            if (m_MyLeafs == null)
                m_MyLeafs = sqlHelper.ExecuteDataTable(string.Format(sql_queryLeaf, m_app.User.CurrentDeptId, m_app.User.Id));

            TreeListNode nd = treeList1.AppendNode(new object[] { id, row["NAME"], "Folder", row["ISPERSON"], row["CREATEUSERS"], row["MEMO"], row["DEPTID"], row["CONTAINER"], row["PREVID"] }, node);
            nd.Tag = row;

            //查找叶子节点
            DataRow[] leafRows = m_MyLeafs.Select("PARENTID='" + id + "'");
            if (leafRows.Length > 0)
            {
                foreach (DataRow leaf in leafRows)
                {
                    TreeListNode leafnd = treeList1.AppendNode(new object[] { leaf["Code"], leaf["NAME"], "Leaf", leaf["ISPERSON"], leaf["CREATEUSERS"], leaf["ITEM_CONTENT"], leaf["DEPTID"], leaf["PARENTID"] }, nd);
                    TempletItem item = new TempletItem(leaf);
                    item.CatalogName = row["NAME"].ToString();
                    leafnd.Tag = item;
                }
            }

            DataRow[] rows = m_MyTreeFolders.Select("Previd='" + id + "' ");

            foreach (DataRow dr in rows)
            {
                LoadTree(dr["ID"].ToString(), dr, nd);
            }
        }

        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {

            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;
            sqlHelper = host.SqlHelper;
            return plg;

        }

        /// <summary>
        /// 删除操作(右键)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDelet2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DeleteItem();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonItemManager_Load(object sender, EventArgs e)
        {
            //yxy  每次加载页面时候刷新数据源
            m_MyTreeFolders = null;
            m_MyLeafs = null;
            if (m_MyTreeFolders == null)
                m_MyTreeFolders = sqlHelper.ExecuteDataTable(string.Format(sql_queryTree, m_app.User.CurrentDeptId, m_app.User.Id));
            DataRow[] rows = m_MyTreeFolders.Select("Previd='' or Previd is null ");
            treeList1.ClearNodes();
            foreach (DataRow dr in rows)
            {
                LoadTree(dr["ID"].ToString(), dr, null);
            }
        }

        /// <summary>
        /// 新增大分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemNewRoot_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //新增大分类的时候清空上次的值  ywk 
            m_ItemCatalog = new ItemCatalog(m_app);
            m_ItemCatalog.BigOrSmallItem = "big";
            m_ItemCatalog.CreateUser = m_app.User.Id;
            m_ItemCatalog.CanChangeFlag = true;
            m_ItemCatalog.SetContainerCodeEditFlag(true);
            m_ItemCatalog.SetIsPersonEditFlag(true);
            if (m_ItemCatalog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string id = Guid.NewGuid().ToString();
                    //string createuser = "";
                    //if (m_ItemCatalog.IsPerson == "1")无论新增的是科室小模板还是个人小模板，都往数据库插入创建人
                    string createuser = m_app.User.Id;
                    sqlHelper.ExecuteNoneQuery(string.Format(sql_insertCatalog, id, DS_Common.FilterSpecialCharacter(m_ItemCatalog.ITEMNAME), string.Empty, 1, DS_Common.FilterSpecialCharacter(m_ItemCatalog.Memo),
                        m_app.User.CurrentDeptId, m_ItemCatalog.ContainerCode, m_ItemCatalog.IsPerson, DS_Common.FilterSpecialCharacter(createuser)));
                    TreeListNode leafnode = treeList1.AppendNode(new object[] { id, m_ItemCatalog.ITEMNAME, "Folder", m_ItemCatalog.IsPerson, m_ItemCatalog.CreateUser, m_ItemCatalog.Memo, m_ItemCatalog.ContainerCode }, null);
                    DataTable dt = sqlHelper.ExecuteDataTable("select * from emrtemplet_item_person_catalog where ID='" + id + "'");
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        leafnode.Tag = dt.Rows[0];
                    }
                    //if (treeList1.FocusedNode != null)
                    //    treeList1.FocusedNode.ExpandAll();
                    m_app.CustomMessageBox.MessageShow("新增成功");
                    treeList1.FocusedNode = leafnode;
                }
                catch (Exception EX)
                {
                    m_app.CustomMessageBox.MessageShow(EX.Message);
                }
            }
        }
        /// <summary>
        /// 新增子分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemNew2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //新增子分类的时候清空上次的值  ywk 
            m_ItemCatalog = new ItemCatalog(m_app);
            if (treeList1.FocusedNode == null)
            {
                m_app.CustomMessageBox.MessageShow("请选择父节点");
                return;
            }
            if (!treeList1.FocusedNode.GetValue("NODETYPE").ToString().Equals("Folder"))
            {
                m_app.CustomMessageBox.MessageShow("请选择分类节点");
                return;
            }
            string parentID = treeList1.FocusedNode.GetValue("ID").ToString();
            string isPerson = treeList1.FocusedNode.GetValue("ISPERSON").ToString();
            //分类权限控制
            string msg = GetMsgIfNotCreateUser(parentID, null == treeList1.FocusedNode.GetValue("createusers") ? "" : treeList1.FocusedNode.GetValue("createusers").ToString(), 0, 2, int.Parse(isPerson));
            if (!string.IsNullOrEmpty(msg))
            {
                m_app.CustomMessageBox.MessageShow(msg);
                return;
            }
            m_ItemCatalog.IsPerson = isPerson;
            m_ItemCatalog.CreateUser = m_app.User.Id; //treeList1.FocusedNode.GetValue("CREATEUSER").ToString();
            m_ItemCatalog.BigOrSmallItem = "small";
            string containerCode = null == treeList1.FocusedNode.GetValue("CONTAINER") ? "" : treeList1.FocusedNode.GetValue("CONTAINER").ToString();
            if (string.IsNullOrEmpty(containerCode))
            {
                DataRow drow = (DataRow)treeList1.FocusedNode.Tag;
                containerCode = null == drow ? "" : (null == drow["CONTAINER"] ? "" : drow["CONTAINER"].ToString());
                if (string.IsNullOrEmpty(containerCode))
                {
                    DataTable dt = m_app.SqlHelper.ExecuteDataTable(" select * from emrtemplet_item_person_catalog where id='" + parentID + "'");
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        containerCode = null == dt.Rows[0]["CONTAINER"] ? "" : dt.Rows[0]["CONTAINER"].ToString();
                    }
                }
            }
            m_ItemCatalog.ContainerCode = containerCode;
            m_ItemCatalog.SetContainerCodeEditFlag(false);
            m_ItemCatalog.SetIsPersonEditFlag(false);

            if (m_ItemCatalog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string id = Guid.NewGuid().ToString();
                    //string createuser = "";
                    //if (m_ItemCatalog.IsPerson == "1")
                    //    createuser = m_app.User.Id;
                    //无论科室小模板还是个人小模板都插入创建人
                    string createuser = m_app.User.Id;
                    sqlHelper.ExecuteNoneQuery(string.Format(sql_insertCatalog, id, DS_Common.FilterSpecialCharacter(m_ItemCatalog.ITEMNAME), parentID, 1, DS_Common.FilterSpecialCharacter(m_ItemCatalog.Memo),
                                m_app.User.CurrentDeptId, m_ItemCatalog.ContainerCode, m_ItemCatalog.IsPerson, DS_Common.FilterSpecialCharacter(createuser)));
                    TreeListNode leafnode = treeList1.AppendNode(new object[] { id, m_ItemCatalog.ITEMNAME, "Folder", m_ItemCatalog.IsPerson, m_ItemCatalog.CreateUser, m_ItemCatalog.Memo, m_ItemCatalog.ContainerCode }, treeList1.FocusedNode);
                    DataTable dt = sqlHelper.ExecuteDataTable("select * from emrtemplet_item_person_catalog where ID='" + id + "'");
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        leafnode.Tag = dt.Rows[0];
                    }
                    m_app.CustomMessageBox.MessageShow("新增成功");
                    //if (treeList1.FocusedNode != null)
                    //    treeList1.FocusedNode.ExpandAll();
                    treeList1.FocusedNode = leafnode;
                }
                catch (Exception EX)
                {
                    m_app.CustomMessageBox.MessageShow(EX.Message);
                }

            }
        }

        /// <summary>
        /// 新增小模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemNewItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (treeList1.FocusedNode == null)
            {
                return;
            }

            try
            {
                string type = treeList1.FocusedNode.GetValue("NODETYPE").ToString();
                string createUser = string.Empty;
                string isPerson = string.Empty;
                string parentID = string.Empty;
                if (type.Equals("Folder"))
                {
                    parentID = treeList1.FocusedNode.GetValue("ID").ToString();
                    isPerson = treeList1.FocusedNode.GetValue("ISPERSON").ToString();
                    createUser = null == treeList1.FocusedNode.GetValue("createusers") ? "" : treeList1.FocusedNode.GetValue("createusers").ToString();
                }
                else
                {
                    DataTable focusDt = m_app.SqlHelper.ExecuteDataTable("select * from emrtemplet_item_person where code='" + treeList1.FocusedNode.GetValue("ID").ToString() + "'");
                    if (null != focusDt && focusDt.Rows.Count > 0)
                    {
                        parentID = null == focusDt.Rows[0]["parentid"] ? "" : focusDt.Rows[0]["parentid"].ToString();
                        createUser = null == treeList1.FocusedNode.ParentNode.GetValue("createusers") ? "" : treeList1.FocusedNode.ParentNode.GetValue("createusers").ToString();
                        isPerson = null == treeList1.FocusedNode.ParentNode.GetValue("ISPERSON") ? "" : treeList1.FocusedNode.ParentNode.GetValue("ISPERSON").ToString();
                    }
                }
                //分类权限控制
                string msg = GetMsgIfNotCreateUser(parentID, createUser, 0, 2, int.Parse(isPerson));
                if (!string.IsNullOrEmpty(msg))
                {
                    m_app.CustomMessageBox.MessageShow(msg);
                    return;
                }
                //宜昌中心医院要求大分类下可以直接插模板 edit by ywk  2012年11月30日11:45:27
                //if (treeList1.FocusedNode.Level == 0)
                //{
                //    m_app.CustomMessageBox.MessageShow("大分类下不可以新增模板，请选择子分类");
                //    return;
                //}

                //if (type.Equals("Leaf"))
                //{
                //    m_app.CustomMessageBox.MessageShow("请选择分类节点");
                //    return;
                //}

                //新增时，清空上次操作里的值 add ywk 
                m_ItemContent.MyItem.Content = "";
                m_ItemContent.MyItem.ItemName = "";
                m_ItemContent.MyItem.Code = Guid.NewGuid().ToString();
                m_ItemContent.MyItem.ParentID = parentID;
                m_ItemContent.MyItem.CatalogName = type.Equals("Folder") ? treeList1.FocusedNode.GetValue("ITEMNAME").ToString() : treeList1.FocusedNode.ParentNode.GetValue("ITEMNAME").ToString();
                m_ItemContent.MyItem.IsPerson = isPerson;
                //新增的时候把当前登录人赋给创建人 add by ywk 2012年6月5日 10:28:10 
                m_ItemContent.MyItem.CreateUser = m_app.User.Id;

                if (m_ItemContent.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 输入的科室小模板的模板内容
                    string content = m_ItemContent.MyItem.Content;
                    if (m_ItemContent.MyItem.Content.Contains("\r\n"))//存在换行，替代插入数据库中 edit by ywk
                    {
                        content = m_ItemContent.MyItem.Content.Replace("\r\n", "'||chr(10)||chr(13)||'");
                    }

                    List<OracleParameter> paraList = new List<OracleParameter>();
                    OracleParameter param1 = new OracleParameter("code", OracleType.VarChar);
                    OracleParameter param2 = new OracleParameter("name", OracleType.VarChar);
                    OracleParameter param3 = new OracleParameter("item_content", OracleType.Clob);
                    OracleParameter param4 = new OracleParameter("deptshare", OracleType.Int32);
                    OracleParameter param5 = new OracleParameter("parentid", OracleType.VarChar);
                    OracleParameter param6 = new OracleParameter("deptid", OracleType.VarChar);
                    OracleParameter param7 = new OracleParameter("isperson", OracleType.VarChar);
                    OracleParameter param8 = new OracleParameter("createusers", OracleType.VarChar);
                    param1.Value = m_ItemContent.MyItem.Code;
                    param2.Value = m_ItemContent.MyItem.ItemName;
                    param3.Value = content;
                    param4.Value = 1;
                    param5.Value = m_ItemContent.MyItem.ParentID;
                    param6.Value = m_app.User.CurrentDeptId;
                    param7.Value = m_ItemContent.MyItem.IsPerson;
                    param8.Value = m_ItemContent.MyItem.CreateUser;
                    paraList.Add(param1);
                    paraList.Add(param2);
                    paraList.Add(param3);
                    paraList.Add(param4);
                    paraList.Add(param5);
                    paraList.Add(param6);
                    paraList.Add(param7);
                    paraList.Add(param8);
                    DS_SqlService.InsertTemplete(paraList);

                    TreeListNode nd = treeList1.AppendNode(new object[] { m_ItemContent.MyItem.Code, m_ItemContent.MyItem.ItemName, "Leaf", m_ItemContent.MyItem.IsPerson, m_ItemContent.MyItem.CreateUser, m_ItemContent.MyItem.Content, m_ItemContent.MyItem.DeptID, m_ItemContent.MyItem.ParentID }, type.Equals("Folder") ? treeList1.FocusedNode : treeList1.FocusedNode.ParentNode);
                    nd.Tag = m_ItemContent.MyItem;
                    //treeList1.FocusedNode.ExpandAll();
                    m_app.CustomMessageBox.MessageShow("新增成功");
                    treeList1.FocusedNode = nd;
                }
            }
            catch (Exception ex)
            {
                m_app.CustomMessageBox.MessageShow(ex.Message);
            }
        }
        /// <summary>
        /// add by  ywk 2012年6月1日 14:54:43
        ///  修改模板的功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditTempl_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (treeList1.FocusedNode == null)
            {
                return;
            }
            TreeListNode focusNode = treeList1.FocusedNode;
            if (!treeList1.FocusedNode.GetValue("NODETYPE").ToString().Equals("Leaf"))
            {
                m_app.CustomMessageBox.MessageShow("请选择模板");
                return;
            }
            //新增时先记录点击的节点以便修改后还是定位在此处
            string nodeID = string.Empty;
            if (this.treeList1.FocusedNode != null)
            {
                nodeID = treeList1.FocusedNode.GetValue("ID").ToString();
            }

            TempletItem item = (TempletItem)treeList1.FocusedNode.Tag;
            //原显示是有问题的add by 杨伟康
            if (item.Content.Contains("\n\r"))
            {
                m_ItemContent.MyItem.Content = item.Content.Replace("\n\r", "\r\n");
            }
            else if (item.Content.Contains("'||chr(10)||chr(13)||'"))
            {
                m_ItemContent.MyItem.Content = item.Content.Replace("'||chr(10)||chr(13)||'", "\r\n");
            }
            else
            {
                m_ItemContent.MyItem.Content = item.Content;
            }
            //m_ItemContent.MyItem.Content = item.Content.Replace("\n\r", "\r\n");


            m_ItemContent.MyItem.Code = item.Code;//模板CODE
            m_ItemContent.MyItem.ItemName = item.ItemName;//模板名称
            m_ItemContent.MyItem.CatalogName = item.CatalogName;//分类名称
            m_ItemContent.MyItem.ParentID = item.ParentID;//父节点
            m_ItemContent.MyItem.IsPerson = item.IsPerson;//区分科室和个人小模板
            m_ItemContent.MyItem.CreateUser = item.CreateUser;

            //修改和删除只能控制到是创建人进行操作 add by ywk  edit by cyq 2012-09-27
            //if (m_ItemContent.MyItem.CreateUser != m_app.User.Id)
            //{
            //    m_app.CustomMessageBox.MessageShow("此模板只有创建者可以进行修改");
            //    return;
            //}
                //是否可以修改所有人创建的模板（0不可以 1;K）
            string CanEditPersonTempleteAll = AppConfigReader.GetAppConfig("CanEditPersonTempleteAll").Config;
            if (CanEditPersonTempleteAll.Trim() == "0")
            {
                string msg = GetMsgIfNotCreateUser(nodeID, item.CreateUser, 1, 0, int.Parse(item.IsPerson));
                if (!string.IsNullOrEmpty(msg))
                {
                    m_app.CustomMessageBox.MessageShow(msg);
                    return;
                }
            }
            if (m_ItemContent.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string content = string.Empty;
                if (m_ItemContent.MyItem.Content.Contains("\r\n"))//存在换行，替代插入数据库中 edit by ywk
                {
                    content = m_ItemContent.MyItem.Content.Replace("\r\n", "'||chr(10)||chr(13)||'");
                }
                else
                {
                    content = m_ItemContent.MyItem.Content;
                }
                string updatesql = @" update emrtemplet_item_person set item_content=@content,name=@name where code=@code ";
                OracleParameter[] paraList = new OracleParameter[]
                {
                    new OracleParameter("content",OracleType.Clob),
                    new OracleParameter("name",OracleType.VarChar),
                    new OracleParameter("code",OracleType.VarChar)
                };
                paraList[0].Value = content;
                paraList[1].Value = m_ItemContent.MyItem.ItemName;
                paraList[2].Value = item.Code;

                DS_SqlHelper.CreateSqlHelper();
                DS_SqlHelper.ExecuteNonQuery(updatesql, paraList, CommandType.Text);

                PersonItemManager_Load(null, null);
                TreeListNode leafnode = treeList1.FindNodeByKeyID(nodeID);
                m_app.CustomMessageBox.MessageShow("修改成功");
                treeList1.FocusedNode = leafnode;
            }
        }
        /// <summary>
        /// 点击绑定模板的树控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }

            string type = e.Node.GetValue("NODETYPE").ToString();
            if ((type.Equals("Leaf")) && (e.Node.Tag is TempletItem))//此处只是在点击子模板的时候右侧显示相应的内容
            {
                TempletItem item = (TempletItem)e.Node.Tag;
                itemDisplayContent1.ItemName = item.ItemName;
                //显示时，换行也要体现出来
                //edit by ywk 2012年6月1日 14:37:11

                if (item.Content.Contains("\n\r"))
                {
                    itemDisplayContent1.Content = item.Content.Replace("\n\r", "\r\n");
                }
                else if (item.Content.Contains("'||chr(10)||chr(13)||'"))
                {
                    itemDisplayContent1.Content = item.Content.Replace("'||chr(10)||chr(13)||'", "\r\n");
                }
                else
                {
                    itemDisplayContent1.Content = item.Content;
                }
                //itemDisplayContent1.Content = item.Content.Replace("\n\r", "\r\n");
                //itemDisplayContent1.Content = item.Content.Replace("'||chr(10)||chr(13)||'", "\r\n");
                itemDisplayContent1.CatalogName = item.CatalogName;
                itemDisplayContent1.ISPserson = item.IsPerson;
                itemDisplayContent1.CreateUser = item.CreateUser;
                itemDisplayContent1.Code = item.Code;
                //itemDisplayContent1.ContainerCode = item.Container;
                itemDisplayContent1.InitFocus();
            }
            if (type.Equals("Folder"))//当点击的是文件夹节点，右侧清空
            {
                //点击文件夹节点，勾选其属于个人模板还是科室模板 add by ywk 2012年6月28日 14:30:23
                TreeListNode focusNode = treeList1.FocusedNode;
                string IsPerson = string.Empty;//区分科室小模板和个人小模板
                string CatalogName = string.Empty;//分类名称 
                string CreateUSer = string.Empty;//创建人
                if (focusNode != null)
                {
                    IsPerson = focusNode.GetValue("ISPERSON").ToString();
                    CatalogName = focusNode.GetValue("ITEMNAME").ToString();//分类名称 
                    CreateUSer = focusNode.GetValue("CREATEUSER").ToString();//创建人
                }
                itemDisplayContent1.Code = string.Empty;
                itemDisplayContent1.ItemName = "";
                itemDisplayContent1.CatalogName = CatalogName;
                itemDisplayContent1.CreateUser = CreateUSer;
                itemDisplayContent1.Content = "";
                itemDisplayContent1.ISPserson = IsPerson;
                //itemDisplayContent1.ISPserson = item.IsPerson;
            }
            //设置当前位置 add by cyq 2012-10-08
            string currentLocation = GetCurrentLocation();
            this.lbl_currentLocation.Text = currentLocation;
            this.lbl_currentLocation.ToolTip = currentLocation;
        }

        /// <summary>
        /// 双击编辑 by cyq 2012-09-27
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList1_DoubleClick(object sender, EventArgs e)
        {
            btn_edit_ItemClick(null, null);
        }

        private void barManager1_QueryShowPopupMenu(object sender, DevExpress.XtraBars.QueryShowPopupMenuEventArgs e)
        {
            if (e.Control != treeList1)
                e.Cancel = true;
        }

        private void treeList1_AfterExpand(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            if (e.Node.HasChildren)
                e.Node.ExpandAll();
        }

        /// <summary>
        /// 修改分类操作
        /// 当修改分类时，其分类下的具体模板也改掉  （分类为科室小模板，分类下模板也要更新掉）
        /// edit by ywk 2012年6月28日 13:29:46
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemModified2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (treeList1.FocusedNode == null)
            {
                m_app.CustomMessageBox.MessageShow("请选择节点！");
                return;
            }
            string nodeID = treeList1.FocusedNode.GetValue("ID").ToString();
            string type = treeList1.FocusedNode.GetValue("NODETYPE").ToString();
            string createUser = null == treeList1.FocusedNode.GetValue("createusers") ? "" : treeList1.FocusedNode.GetValue("createusers").ToString();
            if (!type.Equals("Folder"))
            {
                m_app.CustomMessageBox.MessageShow("请选择分类节点！");
                return;
            }
            DataRow drow = (DataRow)treeList1.FocusedNode.Tag;
            string isPerson = null == drow["isperson"] ? "" : drow["isperson"].ToString();
            //只有创建者可以修改 by cyq2012-09-27
            string msg = GetMsgIfNotCreateUser(nodeID, createUser, 0, 0, int.Parse(isPerson));
            if (!string.IsNullOrEmpty(msg))
            {
                m_app.CustomMessageBox.MessageShow(msg);
                return;
            }
            string containerCode = null == drow["container"] ? "" : drow["container"].ToString();

            m_ItemCatalog.ITEMNAME = null == drow["Name"] ? "" : drow["Name"].ToString();
            m_ItemCatalog.Memo = null == drow["memo"] ? "" : drow["memo"].ToString();
            m_ItemCatalog.ContainerCode = containerCode;
            m_ItemCatalog.IsPerson = isPerson;
            m_ItemCatalog.CreateUser = null == drow["createusers"] ? "" : drow["createusers"].ToString();
            //小分类的适应文件夹和模板类型不可修改
            m_ItemCatalog.SetContainerCodeEditFlag(treeList1.FocusedNode.Level == 0 && !treeList1.FocusedNode.HasChildren);
            m_ItemCatalog.SetIsPersonEditFlag(treeList1.FocusedNode.Level == 0 && !treeList1.FocusedNode.HasChildren);
            //m_ItemCatalog.CanChangeFlag = !(treeList1.FocusedNode.Level == 0 && treeList1.FocusedNode.HasChildren);
            if (m_ItemCatalog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    if (treeList1.FocusedNode.HasChildren && !m_ItemCatalog.ContainerCode.Equals(containerCode))
                    {
                        m_app.CustomMessageBox.MessageShow("该分类包含子集，不可修改适应病历夹");
                        m_ItemCatalog.ContainerCode = containerCode;
                        return;
                    }
                    if (treeList1.FocusedNode.HasChildren && !m_ItemCatalog.IsPerson.Equals(isPerson))
                    {
                        m_app.CustomMessageBox.MessageShow("该分类包含子集，不可修改模板类型");
                        m_ItemCatalog.IsPerson = isPerson;
                        return;
                    }
                    //更新了emrtemplet_item_person_catalog
                    sqlHelper.ExecuteNoneQuery(string.Format(sql_updateItem, nodeID, m_ItemCatalog.ITEMNAME, m_ItemCatalog.Memo,
                                                                m_ItemCatalog.ContainerCode, m_ItemCatalog.IsPerson));
                    PersonItemManager_Load(null, null);
                    m_app.CustomMessageBox.MessageShow("修改成功");
                    TreeListNode leafnode = treeList1.FindNodeByKeyID(nodeID);
                    treeList1.FocusedNode = leafnode;
                }
                catch (Exception EX)
                {
                    m_app.CustomMessageBox.MessageShow(EX.Message);
                }

            }
        }

        /// <summary>
        /// 删除操作    by cyq 2012-09-27
        /// 按钮删除(true)&右键删除(false)
        /// <param name="flag">是否需要删除确认提示:1-需要；0-不需要</param>
        /// </summary>
        private void DeleteItem()
        {
            if (treeList1.FocusedNode == null)
            {
                return;
            }

            string id = treeList1.FocusedNode.GetValue("ID").ToString();
            string type = treeList1.FocusedNode.GetValue("NODETYPE").ToString();
            string isPerson = string.Empty;
            if (type == "Folder")
            {
                DataRow drow = (DataRow)treeList1.FocusedNode.Tag;
                isPerson = null == drow["isPerson"] ? "" : drow["isPerson"].ToString();
            }
            else
            {
                TempletItem item = (TempletItem)treeList1.FocusedNode.Tag;
                isPerson = item.IsPerson;
            }
            string createuser = treeList1.FocusedNode.GetValue("CREATEUSER").ToString();
            string userName = string.Empty;
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    string msg = GetMsgIfNotCreateUser(id, createuser, type == "Folder" ? 0 : 1, 1, int.Parse(isPerson));
                    if (!string.IsNullOrEmpty(msg))
                    {
                        m_app.CustomMessageBox.MessageShow(msg);
                        return;
                    }
                    if (treeList1.FocusedNode.HasChildren && !IfHasDeleteRights(treeList1.FocusedNode))
                    {
                        m_app.CustomMessageBox.MessageShow("该分类包含其他人创建的子分类或模板，您无权删除。");
                        return;
                    }
                    //注：此处考虑为先考虑逻辑上是否满足删除条件，再提示是否确认删除
                    string sureStr = treeList1.FocusedNode.HasChildren ? "您确定要删除所选则分类及其所有子项吗？该操作不可恢复" : "您确定要删除吗？该操作不可恢复";
                    if (!(m_app.CustomMessageBox.MessageShow(sureStr, CustomMessageBoxKind.QuestionYesNo) == System.Windows.Forms.DialogResult.Yes))
                    {
                        return;
                    }

                    //if (type.Equals("Leaf"))
                    //{
                    //    sqlHelper.ExecuteNoneQuery(string.Format(sql_deletLeaf, id));
                    //}
                    //else
                    //{
                    //    sqlHelper.ExecuteNoneQuery(string.Format(sql_deletCatalog, id));
                    //}
                    DeleteNodeCascadeAll(treeList1.FocusedNode);
                    this.treeList1.DeleteNode(this.treeList1.FocusedNode);
                    m_app.CustomMessageBox.MessageShow("删除成功");
                }
                catch (Exception ex)
                {
                    m_app.CustomMessageBox.MessageShow(ex.Message);
                }
            }
        }

        /// <summary>
        /// 是否有删除该分类节点的权限
        /// </summary>
        /// <returns></returns>
        private bool IfHasDeleteRights(TreeListNode tagetNode)
        {
            bool boo = true;
            string currentUserID = m_app.User.Id;
            if (tagetNode.Nodes.Count > 0)
            {
                foreach (TreeListNode node in tagetNode.Nodes)
                {
                    if (!currentUserID.Equals(node.GetValue("CREATEUSER")))
                    {
                        boo = false;
                        break;
                    }
                    if (node.Nodes.Count > 0)
                    {
                        boo = IfHasDeleteRights(node);
                        if (!boo)
                        {
                            break;
                        }
                    }
                }
            }
            return boo;
        }

        /// <summary>
        /// 删除某节点及其所有子节点
        /// </summary>
        /// <param name="tagetNode"></param>
        private void DeleteNodeCascadeAll(TreeListNode tagetNode)
        {
            if (tagetNode.HasChildren)
            {
                foreach (TreeListNode node in tagetNode.Nodes)
                {
                    DeleteNodeCascadeAll(node);
                }
            }

            string id = tagetNode.GetValue("ID").ToString();
            string type = tagetNode.GetValue("NODETYPE").ToString();
            if (type.Equals("Folder"))
            {
                sqlHelper.ExecuteNoneQuery(string.Format(sql_deletCatalog, id));
            }
            else
            {
                sqlHelper.ExecuteNoneQuery(string.Format(sql_deletLeaf, id));
            }
        }

        /// <summary>
        /// 如果当前操作的节点不是创建人，获取提示信息
        /// </summary>
        /// <param name="itemID">当前所操作的节点ID</param>
        /// <param name="createUserID">创建人ID</param>
        /// <param name="type">节点类型：0-分类；1-模板</param>
        /// <param name="actionType">操作类型：0-修改；1-删除；2-新增</param>
        /// <param name="isPerson">模板类型：0-科室小模板；1-个人小模板</param>
        /// <returns></returns>
        private string GetMsgIfNotCreateUser(string itemID, string createUserID, int nodeType, int actionType, int isPerson)
        {
            string msg = string.Empty;
            string userName = string.Empty;
            if (string.IsNullOrEmpty(createUserID))
            {
                string sql = string.Empty;
                if (nodeType == 0)
                {
                    sql = @"select a.createusers,u.Name UserName from emrtemplet_item_person_catalog a join users u on a.createusers=u.id and a.id='" + itemID + "'";
                }
                else
                {
                    sql = @"select a.createusers,u.Name as UserName from emrtemplet_item_person a join users u on a.createusers=u.id and a.code='" + itemID + "'";
                }
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    createUserID = null == dt.Rows[0]["createusers"] ? "" : dt.Rows[0]["createusers"].ToString();
                    userName = null == dt.Rows[0]["UserName"] ? "" : dt.Rows[0]["UserName"].ToString();
                }
            }
            else
            {
                string sql = "select Name as UserName from users where id='" + createUserID + "'";
                DataTable dt = m_app.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt.Rows.Count > 0)
                {
                    userName = null == dt.Rows[0]["UserName"] ? "" : dt.Rows[0]["UserName"].ToString();
                }
            }

            if (createUserID != m_app.User.Id)
            {
                if (isPerson == 0 && actionType == 2)
                {
                    return msg;
                }
                string nodeTypeName = nodeType == 0 ? "分类" : "模板";
                string actionName = actionType == 0 ? "修改" : (actionType == 1 ? "删除" : "新增");
                if (string.IsNullOrEmpty(createUserID))
                {
                    msg = "该" + nodeTypeName + "为系统默认" + nodeTypeName + "，如果要" + actionName + "，请联系管理员";
                }
                else
                {
                    msg = "该" + nodeTypeName + "为 " + userName + "(" + createUserID + ") 创建，您无权" + actionName;
                }
            }

            return msg;
        }

        /// <summary>
        /// 获取当前位置(树节点路径)
        /// </summary>
        /// <returns></returns>
        private string GetCurrentLocation()
        {
            string type = treeList1.FocusedNode.GetValue("NODETYPE").ToString();
            string currentLocation = string.Empty;
            if (type.Equals("Leaf"))
            {
                currentLocation += " -> " + treeList1.FocusedNode.GetValue("ITEMNAME").ToString();
            }
            string folderLocation = GetFolderLocation(string.Empty, type.Equals("Leaf") ? treeList1.FocusedNode.ParentNode.GetValue("ID").ToString() : treeList1.FocusedNode.GetValue("ID").ToString());
            return "当前位置：" + folderLocation + currentLocation;
        }

        /// <summary>
        /// 获取分类节点位置
        /// </summary>
        /// <param name="folderLocation">位置信息</param>
        /// <param name="itemID">分类节点ID</param>
        /// <returns></returns>
        private string GetFolderLocation(string folderLocation, string itemID)
        {
            DataTable dt = m_app.SqlHelper.ExecuteDataTable("select ID,NAME,PREVID from emrtemplet_item_person_catalog where ID = '" + itemID + "'", CommandType.Text);
            if (null != dt && dt.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(folderLocation))
                {
                    folderLocation = dt.Rows[0]["NAME"].ToString();
                }
                else
                {
                    folderLocation = dt.Rows[0]["NAME"].ToString() + " -> " + folderLocation;
                }
                if (null != dt.Rows[0]["PREVID"] && dt.Rows[0]["PREVID"].ToString() != "")
                {
                    folderLocation = GetFolderLocation(folderLocation, dt.Rows[0]["PREVID"].ToString());
                }
            }
            return folderLocation;
        }

        #region "工具栏按钮事件 add by cyq 2012-09-26"
        /// <summary>
        /// 编辑(分类和模板)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_edit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode focusedNode = this.treeList1.FocusedNode;
            if (null == focusedNode)
            {
                m_app.CustomMessageBox.MessageShow("请选择要编辑的对象");
                return;
            }
            if (focusedNode.GetValue("NODETYPE").ToString() == "Folder")
            {//编辑分类
                barButtonItemModified2_ItemClick(null, null);
            }
            else
            {//编辑模板
                btnEditTempl_ItemClick(null, null);
                //EditTemplate(focusedNode);//设置右侧信息显示的地方可直接编辑
            }
        }

        #endregion

        /// <summary>
        /// MouseUP事件控制菜单显示
        /// add by 杨伟康 2012年6月1日 15:59:52
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeListHitInfo hitInfo = treeList1.CalcHitInfo(e.Location);
                if (hitInfo != null && hitInfo.Node != null)
                {
                    treeList1.Focus();
                    treeList1.FocusedNode = hitInfo.Node;
                    TreeListNode node = hitInfo.Node;
                    string type = treeList1.FocusedNode.GetValue("NODETYPE").ToString();

                    if (type == "Folder")
                    {
                        barButtonItemNew2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增子分类
                        barButtonItemModified2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//修改子分类
                        btnEditTempl.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改模板
                        barButtonItemDelet2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//删除
                        if (node.Level == 0)
                        {
                            barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增大分类
                            barButtonItemNewItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增模板  宜昌中心医院要求大分类下可以直接新增模板 add by ywk 2012年11月30日11:46:13
                        }
                        else
                        {
                            barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                            barButtonItemNewItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        }
                    }
                    else
                    {
                        barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        barButtonItemNew2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        barButtonItemNewItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        barButtonItemModified2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                        btnEditTempl.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                        barButtonItemDelet2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    }

                    #region by cyq 2012-09-28
                    //if (node.HasChildren)//父节点为空,本身是父节点
                    //{
                    //    barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增大分类
                    //    barButtonItemNew2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增子分类
                    //    barButtonItemNewItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增模板
                    //    barButtonItemDelet2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//删除
                    //    barButtonItemModified2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//修改分类
                    //    btnEditTempl.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改模板
                    //    //此节点是分类节点，但是他的子节点下是有分类节点
                    //    if (treeList1.FocusedNode.Nodes[0].GetValue("NODETYPE").ToString().Equals("Folder"))
                    //    {
                    //        barButtonItemNewItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改模板
                    //    }
                    //    //此节点有子节点又有父节点，本节点又是分类节点。不显示大分类
                    //    if (node.ParentNode != null)
                    //    {
                    //        barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增大分类
                    //    }

                    //}
                    //else//子节点
                    //{
                    //    //一开始一个都没有
                    //    //if (node.ParentNode!=null)
                    //    //{

                    //    //}
                    //    barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增大分类
                    //    barButtonItemNew2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增子分类
                    //    barButtonItemNewItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//新增模板
                    //    barButtonItemDelet2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//删除
                    //    barButtonItemModified2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//修改分类
                    //    btnEditTempl.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//修改模板
                    //    ////节点为分类节点，但是还没有添加子模板的情况  edit by ywk 2012年6月11日 09:27:22（分类节点）
                    //    if (treeList1.FocusedNode.GetValue("NODETYPE").ToString().Equals("Folder"))
                    //    {
                    //        btnEditTempl.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改模板
                    //        barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增大分类
                    //        //barButtonItemNewItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增模板
                    //    }
                    //    //此节点点击的是具体的模板，不显示分类相关菜单(模板节点)
                    //    if (treeList1.FocusedNode.GetValue("NODETYPE").ToString().Equals("Leaf"))
                    //    {
                    //        barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增大分类
                    //        barButtonItemNew2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增子分类
                    //        barButtonItemModified2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//修改分类
                    //        barButtonItemNewItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//新增模板
                    //        //edit by cyq 2012-09-27 设置模板可右键编辑
                    //        btnEditTempl.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;//修改模板
                    //    }

                    //}
                    #endregion
                }
                else
                {
                    //by cyq 2012-09-25
                    barButtonItemNewRoot.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barButtonItemNew2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barButtonItemNewItem.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barButtonItemDelet2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barButtonItemModified2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    btnEditTempl.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
        }

        /// <summary>
        /// 设置菜单图片
        /// add by cyq 2012-09-25 11:15
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList1_GetStateImage(object sender, GetStateImageEventArgs e)
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


    }

    public class TempletItem
    {
        public string Content { get; set; }

        public string Code { get; set; }

        public string ParentID { get; set; }

        public string ItemName { get; set; }

        public string CatalogName { get; set; }

        public string Container { get; set; }

        public string DeptID { get; set; }

        public string IsPerson { get; set; }

        public string CreateUser { get; set; }

        public TempletItem()
        {

        }

        public TempletItem(DataRow row)
        {
            Content = row["ITEM_CONTENT"].ToString();
            Code = row["Code"].ToString();
            ItemName = row["Name"].ToString();
            ParentID = row["PARENTID"].ToString();
            CatalogName = string.Empty;
            //Container = row["Container"].ToString();
            DeptID = row["DEPTID"].ToString();

            IsPerson = row["ISPERSON"].ToString();
            CreateUser = row["CREATEUSERS"].ToString();
        }
    }
}
