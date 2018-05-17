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
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.KnowledgeBase
{
    public partial class UCMedicineDirect : DevExpress.XtraEditors.XtraUserControl
    {

        private WaitDialogForm m_WaitDialog;
        IEmrHost m_app;
        SqlManger m_SqlManger;
        DataTable Doseform = new DataTable();
        string sqlGetDrug = @"select * from MedicineDirect md where length(doseform)<>0 and upper(md.directtitle2) like '%{0}%' or upper(md.pinyin) like '%{0}%'";
        TreeList ntree = new TreeList();
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

        public UCMedicineDirect(IEmrHost _app)
        {
            try
            {
                InitializeComponent();
                m_app = _app;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 页面加载
        /// edit by 王冀 2012-10-30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCMedicineDirect_Load(object sender, EventArgs e)
        {
            try
            {
                m_WaitDialog = new WaitDialogForm("创建用户界面……", "请稍等。");
                m_SqlManger = new SqlManger(m_app);
                MakeTree();
                this.ActiveControl = txtDirectTitle;
                txtFind.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #region 已注释 YD_Common存在方法 by xlb 2013-01-04
        ///// <summary>
        ///// 初始化等待框
        ///// edit by 王冀 2012-10-30
        ///// </summary>
        ///// <param name="caption"></param>
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

        //private void HideWaitDialog()
        //{
        //    if (m_WaitDialog != null)
        //        m_WaitDialog.Hide();
        //}
        #endregion

        /// <summary>
        /// 构建树控件
        /// 王冀 2012-10-29 修改 加图标
        /// </summary>
        private void MakeTree()
        {
            try
            {
                DS_Common.SetWaitDialogCaption(m_WaitDialog,"正在构建树控件！");
                //DataTable Doseform;
                //DataTable DirectTitle2;
                //已绑定节点 清空 王冀 2012-10-30
                //DataTable drug;
                m_HaveBindNode.Clear();
                treeList_Medicine.ClearNodes();
                Doseform = m_SqlManger.GetMedicaineDirectTreeOne();

                //第一级
                foreach (DataRow dr in Doseform.Rows)
                {
                    TreeListNode parentNode = null;//
                    parentNode = treeList_Medicine.AppendNode(new object[] { dr["Doseform"].ToString(), "Folder", dr["Doseform"].ToString() }, null);
                    m_HaveBindNode.Add(dr["Doseform"].ToString());
                    DataTable DirectTitle2 = m_SqlManger.GetMedicaineDirectTreeTwo(dr["Doseform"].ToString());

                    //第二级
                    foreach (DataRow secdr in DirectTitle2.Rows)
                    {
                        TreeListNode node = null;
                        node = treeList_Medicine.AppendNode(new object[] { secdr["DirectTitle"].ToString(), "Leaf", secdr["ID"].ToString() }, parentNode);
                        //m_LeafNode.Add(secdr["id"].ToString());
                        node.Tag = secdr["id"].ToString();
                    }
                }
                ntree = treeList_Medicine;
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

        }
        /// <summary>
        /// 选中节点改变事件
        /// edit by 王冀 2012-10-30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Medicine_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            //xll 2012-10-29
            try
            {
                if (e.Node == null)
                {
                    txtDirectTitle.Text = txtDirectTitle2.Text = txtDoseform.Text = txtDirectContent.Text = "";
                    return;
                }
                if (e.Node.Tag == null || e.Node.Tag.ToString() == "")
                {
                    txtDirectTitle.Text = txtDirectTitle2.Text = txtDoseform.Text = txtDirectContent.Text = "";
                    return;
                }
                DataTable dt = m_SqlManger.GetMedicaineDirectByID(e.Node.Tag.ToString());
                txtDirectTitle.Text = dt.Rows[0]["DirectTitle"].ToString();
                txtDirectTitle2.Text = dt.Rows[0]["DirectTitle2"].ToString();
                txtDoseform.Text = dt.Rows[0]["Doseform"].ToString();
                txtDirectContent.Text = dt.Rows[0]["DirectContent"].ToString();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }


        /// <summary>
        /// 设置菜单图片
        /// 王冀 2012 10 29
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
        /// 王冀 2012 10 29
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Medicine_AfterExpand(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            if (e.Node.HasChildren)
                e.Node.ExpandAll();
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
        /// 王冀2012-10-29
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                DS_Common.SetWaitDialogCaption(m_WaitDialog,"正在构建树控件！");
                m_HaveBindNode.Clear();
                treeListFind.ClearNodes();
                string key = this.txtFind.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
                if (CheckKey(key))
                {
                    this.txtFind.Focus(); return;
                }

                if (key == null || key.Trim() == string.Empty)
                {
                    this.treeListFind.Visible = false;
                    this.treeList_Medicine.Visible = true;
                }
                else
                {
                    this.treeList_Medicine.Visible = false;
                    this.treeListFind.Visible = true;
                    dtMyLeafNode = m_SqlManger.GetMedicaineByKey(sqlGetDrug, key);
                    if (dtMyLeafNode.Rows.Count == 0)
                    {
                        MessageBox.Show("没有查询到结果，请重新输入！");
                        return;
                    }
                    else
                    {
                        LoadTreeList(dtMyLeafNode);
                    }
                }
                DS_Common.HideWaitDialog(m_WaitDialog);
                txtFind.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 加载Treelist中的数据
        /// 王冀 2012 10 30
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
                    if (!m_HaveBindNode.Contains(dr["DOSEFORM"].ToString()))
                    {
                        TreeListNode nd = this.treeListFind.AppendNode(new object[] { dr["Doseform"].ToString(), "Folder", dr["Doseform"].ToString() }, null);
                        m_HaveBindNode.Add(dr["Doseform"].ToString());
                        DataRow[] leafRows = table.Select("DOSEFORM = '" + dr["Doseform"].ToString() + "'");
                        foreach (DataRow leftrow in leafRows)
                        {
                            TreeListNode leafnd = treeListFind.AppendNode(new object[] { leftrow["DirectTitle"].ToString(), "Leaf", leftrow["ID"].ToString() }, nd);
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
    }
}
