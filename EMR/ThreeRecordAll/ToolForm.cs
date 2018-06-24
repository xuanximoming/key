using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Core.PersonTtemTemplet;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Library.EmrEditor.Src.Document;
using DrectSoft.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DrectSoft.EMR.ThreeRecordAll
{
    public partial class ToolForm : DevBaseForm
    {
        bool m_IsAllowDrop = true;
        DataTable dataTableSymbol = null;
        /// <summary>
        /// 小模板列表
        /// </summary>
        private PersonItemManager m_PersonItem;

        /// <summary>
        /// 小模板列表
        /// </summary>
        private DataTable m_MyTreeFolders;
        private DataTable m_MyLeafs;
        IEmrHost m_app;
        public ToolForm(IEmrHost app)
        {
            m_app = app;
            InitializeComponent();
            BindSymbolInner();
        }



        #region 绑定特殊字符 xll


        /// <summary>
        /// 初始化特殊字符
        /// </summary>
        private void InitDataSymbol()
        {
            dataTableSymbol = DrectSoft.DSSqlHelper.DS_SqlHelper.ExecuteDataTable("select  SYMBOL as 名称 from DICT_SYMBOL", CommandType.Text);
        }

        /// <summary>
        /// 获取特殊字符 绑定
        /// </summary>
        private void BindSymbolInner()
        {
            try
            {
                if (dataTableSymbol == null)
                {
                    InitDataSymbol();
                }
                System.Drawing.Font font = new System.Drawing.Font("宋体", 10, FontStyle.Regular);
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;

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
                BindSymbol(dataTableSymbol, list);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 绑定特殊字符
        /// </summary>
        /// <param name="dataTableSymbol"></param>
        private void BindSymbol(DataTable dataTableSymbol, ImageList list)
        {
            try
            {

                imageListBoxControlSymbol.Items.Clear();
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

        #region 特殊字符 - 拖动
        private void imageListBoxControlSymbol_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                ImageListBoxControl list = (ImageListBoxControl)sender;
                if (e.Button == MouseButtons.Left)
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
                MyMessageBox.Show(1, ex);
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
                MyMessageBox.Show(1, ex);
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
                MyMessageBox.Show(1, ex);
            }
        }
        #endregion

        #region 小模板列表拖拽
        private void treeListPersonTemplate_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
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
                MyMessageBox.Show(1, ex);
            }
        }

        private void treeListPersonTemplate_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left && m_IsAllowDrop)
                {
                    TreeList list = (TreeList)sender;
                    if ((list.FocusedNode != null) && (list.FocusedNode.Tag is TempletItem))
                    {
                        DoDragDorpInfo(list);
                    }
                }
                else
                {
                    m_IsAllowDrop = false;
                }

                TreeListHitInfo hitInfo = treeListPersonTemplate.CalcHitInfo(new Point(e.X, e.Y));
                if (hitInfo.Node != null)
                {
                    if (hitInfo.Node.GetValue("NODETYPE").ToString() == "Leaf" && hitInfo.Node.Tag is TempletItem)
                    {
                        TempletItem item = (TempletItem)hitInfo.Node.Tag;
                        InitToolTip(null == item ? "" : item.Content, treeListPersonTemplate, treeListPersonTemplate.PointToScreen(e.Location));
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void treeListPersonTemplate_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                m_IsAllowDrop = false;
                //右侧小模板列表右键
                if (e.Button == MouseButtons.Right)
                {
                    btnRight_DeptTemplete.Visibility = BarItemVisibility.Always;
                    popupMenu_DeptTemplate.ShowPopup(treeListPersonTemplate.PointToScreen(new Point(e.X, e.Y)));
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 工具提示方法
        /// </summary>
        /// <param name="displayName">提示信息</param>
        /// <param name="tipTreeList">树控件</param>
        /// <param name="point">位置</param>
        private void InitToolTip(string displayName, TreeList tipTreeList, Point point)
        {
            try
            {
                if (null != displayName && !string.IsNullOrEmpty(displayName.Trim()))
                {
                    toolTipControllerTreeList.SetToolTip(tipTreeList, displayName);
                    toolTipControllerTreeList.SetToolTipIconType(tipTreeList, DevExpress.Utils.ToolTipIconType.Exclamation);
                    toolTipControllerTreeList.ShowBeak = true;
                    toolTipControllerTreeList.ShowShadow = true;
                    toolTipControllerTreeList.Rounded = true;
                    toolTipControllerTreeList.ShowHint(displayName, point);
                }
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
                if (tree.FocusedNode == null)
                {
                    return;
                }
                KeyValuePair<string, object> data = new KeyValuePair<string, object>();
                if ((tree.FocusedNode != null) && (tree.FocusedNode.Tag != null))
                {

                    if (tree == treeListPersonTemplate)
                    {
                        TempletItem item = tree.FocusedNode.Tag as TempletItem;
                        if (item.Content.Contains("||chr(10)||chr(13)||"))//存在换行，替代插入数据库中 edit by ywk
                        {
                            item.Content = item.Content.Replace("'||chr(10)||chr(13)||'", "\r\n");
                        }
                        data = new KeyValuePair<string, object>(item.Content, ElementType.Text);
                    }
                }
                if (!string.IsNullOrEmpty(data.Key))
                {
                    tree.DoDragDrop(data, DragDropEffects.All);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void btnRight_DeptTemplete_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (m_PersonItem == null)
                {
                    m_PersonItem = new PersonItemManager(m_app);
                }
                m_PersonItem.ShowDialog();
                //InitPersonTree(true, string.Empty);
                //此处修改，科室小模板窗体关闭后，原来的右侧清空了，现在更改为加载进来时的科室模板列表，声明了属性记录窗体加载时的模板值。
                InitPersonTreeInner(true, "AP");
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }



        class ParameterObject
        {
            public bool Refresh { get; set; }
            public string Catalog { get; set; }
        }

        /// <summary>
        /// xll 初始化界面
        /// </summary>
        /// <param name="refresh"></param>
        /// <param name="catalog"></param>
        private void InitPersonTreeInner(bool refresh, string catalog)//(bool refresh, string catalog)
        {
            try
            {
                if ((null == m_MyTreeFolders) || (refresh))
                {
                    m_MyTreeFolders = DS_SqlService.GetPersonTempleteFloder(DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.Id);
                    m_MyLeafs = DS_SqlService.GetPersonTemplete(DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.Id);
                }
                InitPersonTreeInvoke(catalog);
            }
            catch (Exception)
            {
            }
        }

        private void InitPersonTreeInvoke(string catalog)
        {
            try
            {

                DataRow[] rows = m_MyTreeFolders.Select("(Previd='' or Previd is null)  and (container = '99' or container ='" + catalog + "')");
                treeListPersonTemplate.BeginUnboundLoad();
                treeListPersonTemplate.Nodes.Clear();
                foreach (DataRow dr in rows)
                {
                    LoadTree(dr["ID"].ToString(), dr, null, catalog);
                }
                treeListPersonTemplate.CollapseAll();
                treeListPersonTemplate.EndUnboundLoad();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 小模板列表 - 加载树
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="node"></param>
        /// <param name="catalog"></param>
        private void LoadTree(string id, DataRow row, TreeListNode node, string catalog)
        {

            try
            {
                TreeListNode nd = treeListPersonTemplate.AppendNode(new object[] { id, row["NAME"], "Folder", row["ISPERSON"], row["CREATEUSERS"] }, node);
                nd.Tag = row;

                //查找叶子节点
                DataRow[] leafRows = m_MyLeafs.Select("PARENTID='" + id + "' ");
                if (leafRows.Length > 0)
                {
                    foreach (DataRow leaf in leafRows)
                    {
                        TreeListNode leafnd = treeListPersonTemplate.AppendNode(new object[] { leaf["Code"], leaf["NAME"], "Leaf", leaf["ISPERSON"], leaf["CREATEUSERS"] }, nd);
                        TempletItem item = new TempletItem(leaf);
                        if (item.Content.Contains("||chr(10)||chr(13)||"))//存在换行，替代插入数据库中 edit by ywk
                        {
                            item.Content = item.Content.Replace("'||chr(10)||chr(13)||'", "\r\n");
                        }
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 小模板列表 - 设置文件图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeListPersonTemplate_GetStateImage(object sender, GetStateImageEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void btn_SearchTemplete_Click(object sender, EventArgs e)
        {
            try
            {
                SearchPersonTree(false, "AP", this.txt_SearchTemplete.Text);
                if (!string.IsNullOrEmpty(this.txt_SearchTemplete.Text))
                {
                    foreach (TreeListNode node in treeListPersonTemplate.Nodes)
                    {
                        node.ExpandAll();
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 搜索模板树
        /// by cyq 2012-09-25
        /// </summary>
        /// <param name="refresh">是否从数据库刷新数据</param>
        /// <param name="catalog">模板‘病历类别’</param>
        /// <param name="keyword">搜索关键字</param>
        private void SearchPersonTree(bool refresh, string catalog, string keyword)
        {
            try
            {
                if ((null == m_MyTreeFolders) || (refresh))
                {
                    m_MyTreeFolders = DS_SqlService.GetPersonTempleteFloder(DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.Id);
                    m_MyLeafs = DS_SqlService.GetPersonTemplete(DS_Common.currentUser.CurrentDeptId, DS_Common.currentUser.Id);
                }
                //所有符合搜索条件的模板集合
                var serchedLeafs = m_MyLeafs.AsEnumerable().Where(p => p["Name"].ToString().Contains(keyword));
                IEnumerable<DataRow> allFolders = m_MyTreeFolders.AsEnumerable();
                //所有符合搜索条件的分类ID集合
                List<string> list = new List<string>();
                foreach (DataRow leaf in serchedLeafs)
                {
                    list = GetSearchParentIDs(list, allFolders, leaf, 0);
                }
                var allSearchedFolders = m_MyTreeFolders.AsEnumerable().Where(p => list.Contains(p["ID"].ToString()));
                var allSearchedFirstFolders = allSearchedFolders.Where(p => (null == p["Previd"] || p["Previd"].ToString() == "") && (p["container"].ToString() == "99" || p["container"].ToString() == catalog));
                treeListPersonTemplate.BeginUnboundLoad();
                treeListPersonTemplate.Nodes.Clear();
                foreach (DataRow dr in allSearchedFirstFolders)
                {
                    SearchLoadTree(dr, null, catalog, allSearchedFolders, serchedLeafs, keyword);
                }
                treeListPersonTemplate.CollapseAll();
                treeListPersonTemplate.EndUnboundLoad();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 根据模板获取其对应的所有分类ID
        /// </summary>
        /// <param name="list">分类ID结果集</param>
        /// <param name="allFolders">所有分类集合</param>
        /// <param name="leaf">模板</param>
        /// <param name="childFlag">0-模板；1-分类</param>
        /// <returns></returns>
        private List<string> GetSearchParentIDs(List<string> list, IEnumerable<DataRow> allFolders, DataRow leaf, int childFlag)
        {
            try
            {
                var parentFolders = allFolders.Where(p => p["ID"].ToString() == leaf[childFlag == 0 ? "PARENTID" : "PREVID"].ToString());
                if (parentFolders.Count() > 0)
                {
                    var parentFolder = parentFolders.FirstOrDefault();
                    if (!list.Contains(parentFolder["ID"].ToString()))
                    {
                        list.Add(parentFolder["ID"].ToString());
                        if (null != parentFolder["PREVID"] && parentFolder["PREVID"].ToString() != "")
                        {
                            list = GetSearchParentIDs(list, allFolders, parentFolder, 1);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">分类</param>
        /// <param name="node">此分类的父节点</param>
        /// <param name="catalog">模板‘病历类别’</param>
        /// <param name="keyword">搜索关键字</param>
        private void SearchLoadTree(DataRow row, TreeListNode node, string catalog, IEnumerable<DataRow> allSearchedFolders, IEnumerable<DataRow> searchedLeafs, string keyword)
        {
            try
            {
                TreeListNode nd = treeListPersonTemplate.AppendNode(new object[] { row["ID"], row["NAME"], "Folder", row["ISPERSON"], row["CREATEUSERS"] }, node);
                nd.Tag = row;

                //查找叶子节点
                var leafRows = searchedLeafs.Where(p => p["PARENTID"].ToString() == row["ID"].ToString());
                if (null != leafRows && leafRows.Count() > 0)
                {
                    foreach (DataRow leaf in leafRows)
                    {
                        TreeListNode leafnd = treeListPersonTemplate.AppendNode(new object[] { leaf["Code"], leaf["NAME"], "Leaf", leaf["ISPERSON"], leaf["CREATEUSERS"] }, nd);
                        TempletItem item = new TempletItem(leaf);
                        item.CatalogName = row["NAME"].ToString();
                        leafnd.Tag = item;
                    }
                }

                var rows = allSearchedFolders.Where(p => p["Previd"].ToString() == row["ID"].ToString());
                foreach (DataRow dr in rows)
                {
                    SearchLoadTree(dr, nd, catalog, allSearchedFolders, searchedLeafs, keyword);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void navBarControl1_ActiveGroupChanged(object sender, DevExpress.XtraNavBar.NavBarGroupEventArgs e)
        {

            try
            {
                if (e.Group == navBarGroupTSZF)
                {
                    BindSymbolInner();
                }
                else if (e.Group == navBarGroupKSMB)
                {
                    if (m_MyTreeFolders == null)
                    {
                        InitPersonTreeInner(false, "AP");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
