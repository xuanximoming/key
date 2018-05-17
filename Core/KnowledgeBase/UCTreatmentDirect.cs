using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.Utils;
using DevExpress.XtraTreeList;
using System.Data.Odbc;
using System.Data.OracleClient;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.KnowledgeBase
{
    public partial class UCTreatmentDirect : DevExpress.XtraEditors.XtraUserControl
    {

        private WaitDialogForm m_WaitDialog;
        IEmrHost m_app;
        SqlManger m_SqlManger;


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
        /// 查询到的结果叶节点
        /// </summary>
        DataTable dtMyLeafNode = new DataTable();

        List<TreeListNode> TreeListNodelist = new List<TreeListNode>();

        public UCTreatmentDirect(IEmrHost _app)
        {
            InitializeComponent();
            m_app = _app;
        }
        /// <summary>
        /// 页面加载
        /// 王冀 2012-10-30 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCMedicineDirect_Load(object sender, EventArgs e)
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("创建用户界面……", "请稍等。");
                m_SqlManger = new SqlManger(m_app);
                this.cbClass.SelectedIndex = 0;
                MakeTree();
                txtFind.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 已注释 by xlb 2013-01-04 YD_Common存在该方法
        /// <summary>
        /// 初始化等待框
        /// 王冀 2012-10-30 
        /// </summary>
        /// <param name="caption"></param>
        //private void SetWaitDialogCaption(string caption)
        //{
        //    try
        //    {
        //        if (m_WaitDialog != null)
        //        {
        //            if (!m_WaitDialog.Visible)
        //                m_WaitDialog.Visible = true;
        //            m_WaitDialog.Caption = caption;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
        //    }
        //}
        /// <summary>
        /// 等待框收起
        /// 王冀 2012-10-30 
        /// </summary>
        //private void HideWaitDialog()
        //{
        //    try
        //    {
        //        if (m_WaitDialog != null)
        //            m_WaitDialog.Hide();
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
        //    }
        //}
        #endregion

        /// <summary>
        /// 构建树控件
        /// 王冀 2012-10-30 
        /// </summary>
        private void MakeTree()
        {
            try
            {
                DS_Common.SetWaitDialogCaption(m_WaitDialog,"正在构建树控件！");
                DataTable Class;
                DataTable Treatment;
                m_HaveBindNode.Clear();
                treeList_Medicine.ClearNodes();
                Class = m_SqlManger.GetTreatmentTreeOne(this.cbClass.SelectedIndex.ToString());

                //第一级
                foreach (DataRow dr in Class.Rows)
                {
                    TreeListNode parentNode = null;//
                    parentNode = treeList_Medicine.AppendNode(new object[] { dr["unitmname"].ToString(), "Folder", dr["unitmid"].ToString() }, null);
                    m_HaveBindNode.Add(dr["unitmname"].ToString());
                    Treatment = m_SqlManger.GetTreatmentTreeTwo(dr["unitmid"].ToString());

                    //第二级
                    foreach (DataRow secdr in Treatment.Rows)
                    {
                        TreeListNode node = null;
                        node = treeList_Medicine.AppendNode(new object[] { secdr["jibingmingcheng"].ToString(), "Leaf", secdr["ID"].ToString() }, parentNode);
                        //m_LeafNode.Add(secdr["id"].ToString());
                        node.Tag = secdr["id"].ToString();
                    }
                }
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 选中节点改变事件
        /// 王冀 2012-10-30 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Medicine_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (e.Node == null)
                {
                    this.txtDirectContent.Text = "";
                    return;
                }
                if (e.Node.Tag == null || e.Node.Tag.ToString() == "")
                {
                    this.txtDirectContent.Text = "";
                    return;
                }
                DataTable dt = m_SqlManger.GetTreatmentByID(e.Node.Tag.ToString());
                this.txtDirectContent.Text = System.Text.Encoding.GetEncoding("gb2312").GetString(dt.Rows[0]["content"] as byte[]);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }



        /// <summary>
        /// 设置菜单图片
        /// 王冀 2012 10 30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Medicine_GetStateImage(object sender, GetStateImageEventArgs e)
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
        /// <summary>
        /// 节点直接展开
        /// 王冀 2012 10 30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Medicine_AfterExpand(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            try
            {
                if (e.Node.HasChildren)
                    e.Node.ExpandAll();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        private bool CheckKey(string key)
        {
            if (key.Length > 16)
            {
                MessageBox.Show("关键字长度不能超过16个英文字符或者8个中文字符!");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 搜索功能
        /// 王冀2012-10-30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                string key = this.txtFind.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
                if (CheckKey(key))
                {
                    this.txtFind.Focus(); return;
                }
                m_HaveBindNode.Clear();
                treeList_Medicine.ClearNodes();

                if (key == null || key.Trim() == string.Empty)
                {
                    dtMyLeafNode = m_SqlManger.GetTreatmentByKey(key, this.cbClass.SelectedIndex.ToString());
                }
                else
                { dtMyLeafNode = m_SqlManger.GetTreatmentByKey(key, this.cbClass.SelectedIndex.ToString()); }

                LoadTreeList(dtMyLeafNode);
                txtFind.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 加载Treelist中的数据
        /// 王冀 2012 10 31
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <param name="node"></param>
        private void LoadTreeList(DataTable table)
        {
            try
            {
                foreach (DataRow dr in table.Rows)
                {
                    if (!m_HaveBindNode.Contains(dr["unitmname"].ToString()))
                    {
                        TreeListNode nd = this.treeList_Medicine.AppendNode(new object[] { dr["unitmname"].ToString(), "Folder", dr["unitmname"].ToString() }, null);
                        m_HaveBindNode.Add(dr["unitmname"].ToString());
                        DataRow[] leafRows = table.Select("unitmname='" + dr["unitmname"].ToString() + "'");
                        foreach (DataRow leftrow in leafRows)
                        {
                            TreeListNode leafnd = treeList_Medicine.AppendNode(new object[] { leftrow["jibingmingcheng"].ToString(), "Leaf", leftrow["ID"].ToString() }, nd);
                            m_HaveBindNode.Add(leftrow["id"].ToString());
                            leafnd.Tag = leftrow["id"].ToString();
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
        /// 搜索分类下拉框选项改变事件
        /// 王冀 2012 10 31
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MakeTree();
                txtFind.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

    }
}
