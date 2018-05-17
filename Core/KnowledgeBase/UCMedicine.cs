using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DrectSoft.Core.KnowledgeBase
{
    public partial class UCMedicine : DevExpress.XtraEditors.XtraUserControl
    {


        IEmrHost m_app;

        SqlManger m_SqlManger;

        DataTable m_dataTable;

        private List<string> m_HaveBindNode = new List<string>();

        public UCMedicine(IEmrHost _app)
        {
            InitializeComponent();

            m_app = _app;
        }

        /// <summary>
        /// 创建树
        /// edit 王冀 2012-10-30
        /// </summary>
        private void MakeTree()
        {
            try
            {
                DataTable rootDrug;
                //DataTable secordDrug;
                //DataTable drug;
                m_HaveBindNode.Clear();//已绑定节点 清空 王冀 2012-10-30
                treeList_Medicine.ClearNodes();
                rootDrug = m_SqlManger.GetMedicineTreeOne();
                MakeTreeone(rootDrug);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }

            #region 已注释 王冀 2012-10-30
            ////第一级
            //foreach (DataRow dr in rootDrug.Rows)
            //{

            //    TreeListNode parentNode = null;//
            //    parentNode = treeList_Medicine.AppendNode(new object[] { dr["categorytwo"].ToString(), "Folder", dr["categorytwo"].ToString() }, null);
            //    m_HaveBindNode.Add(dr["categorytwo"].ToString());
            //    secordDrug = m_SqlManger.GetMedicaineTreeSec(dr["categorytwo"].ToString());

            //    MakeTreetwo(secordDrug, parentNode);
            //    ////第二级
            //    //foreach (DataRow secdr in secordDrug.Rows)
            //    //{
            //    //    TreeListNode node = null;//
            //    //    node = treeList_Medicine.AppendNode(new object[] { secdr["categorythree"].ToString(), "Folder", secdr["categorythree"].ToString() }, parentNode);
            //    //    m_HaveBindNode.Add(secdr["categorythree"].ToString());
            //    //    drug = m_SqlManger.GetMedicaineByThreeName(secdr["categorythree"].ToString());

            //    //    foreach (DataRow threedr in drug.Rows)
            //    //    {
            //    //        TreeListNode threenode = null;//
            //    //        threenode = treeList_Medicine.AppendNode(new object[] { threedr["Name"].ToString(), "Leaf", threedr["ID"].ToString() }, node);
            //    //        m_HaveBindNode.Add(threedr["ID"].ToString());
            //    //        threenode.Tag = threedr["id"].ToString();
            //    //    }
            //    //}

            //}
            #endregion
        }
        /// <summary>
        /// 正常加载树三级节点
        /// 王冀 2012 10 31
        /// </summary>
        /// <param name="drug"></param>
        /// <param name="node"></param>
        private void MakeTreethree(DataTable drug, TreeListNode node)
        {
            try
            {
                foreach (DataRow threedr in drug.Rows)
                {
                    TreeListNode threenode = null;//
                    if (!m_HaveBindNode.Contains(threedr["ID"].ToString()))
                    {
                        threenode = treeList_Medicine.AppendNode(new object[] { threedr["Name"].ToString(), "Leaf", threedr["ID"].ToString() }, node);
                        m_HaveBindNode.Add(threedr["ID"].ToString());
                        threenode.Tag = threedr["id"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 正常加载树二级节点
        /// 王冀 2012 10 31
        /// </summary>
        /// <param name="secordDrug"></param>
        /// <param name="parentNode"></param>
        private void MakeTreetwo(DataTable secordDrug, TreeListNode parentNode)
        {
            try
            {
                foreach (DataRow secdr in secordDrug.Rows)
                {
                    TreeListNode node = null;//
                    if (!m_HaveBindNode.Contains(secdr["categorythree"].ToString() + "2" + secdr["categorytwo"].ToString()))
                    {
                        node = treeList_Medicine.AppendNode(new object[] { secdr["categorythree"].ToString(), "Folder", secdr["categorythree"].ToString() }, parentNode);
                        m_HaveBindNode.Add(secdr["categorythree"].ToString() + "2" + secdr["categorytwo"].ToString());
                        DataTable drug = m_SqlManger.GetMedicaineByThreeName(secdr["categorythree"].ToString());
                        MakeTreethree(drug, node);
                    }

                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 根据查询到的二级节点加载树二级节点
        /// 王冀 2012 10 31
        /// </summary>
        /// <param name="secordDrug"></param>
        /// <param name="parentNode"></param>
        private void MakeTreetwo(DataRow[] secordDrug, TreeListNode parentNode)
        {
            try
            {
                foreach (DataRow secdr in secordDrug)
                {
                    TreeListNode node = null;//
                    if (!m_HaveBindNode.Contains(secdr["categorythree"].ToString() + "2" + secdr["categorytwo"].ToString()))
                    {
                        node = treeList_Medicine.AppendNode(new object[] { secdr["categorythree"].ToString(), "Folder", secdr["categorythree"].ToString() }, parentNode);
                        m_HaveBindNode.Add(secdr["categorythree"].ToString() + "2" + secdr["categorytwo"].ToString());
                        DataTable drug = m_SqlManger.GetMedicaineByThreeName(secdr["categorythree"].ToString());
                        MakeTreethree(drug, node);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 根据三级节点加载二级节点
        /// 王冀 2012 10 31
        /// </summary>
        /// <param name="secordDrug"></param>
        /// <param name="parentNode"></param>
        private void MakeThreeTwoTwo(DataTable secordDrug, TreeListNode parentNode)
        {
            try
            {
                foreach (DataRow secdr in secordDrug.Rows)
                {
                    TreeListNode node = null;//
                    if (!m_HaveBindNode.Contains(secdr["categorythree"].ToString() + "2" + secdr["categorytwo"].ToString()))
                    {
                        node = treeList_Medicine.AppendNode(new object[] { secdr["categorythree"].ToString(), "Folder", secdr["categorythree"].ToString() }, parentNode);
                        m_HaveBindNode.Add(secdr["categorythree"].ToString() + "2" + secdr["categorytwo"].ToString());
                        //DataTable drug = m_SqlManger.GetMedicaineByThreeName(secdr["categorythree"].ToString());
                        DataRow[] dataRows = secordDrug.Select("categorythree='" + secdr["categorythree"].ToString() + "'");
                        MakeTreethree(ToDataTable(dataRows), node);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 正常加载树一级节点
        /// 王冀 2012 10 31
        /// </summary>
        /// <param name="rootDrug"></param>
        private void MakeTreeone(DataTable rootDrug)
        {
            try
            {
                foreach (DataRow dr in rootDrug.Rows)
                {
                    TreeListNode parentNode = null;//
                    if (!m_HaveBindNode.Contains(dr["categorytwo"].ToString() + "1"))
                    {
                        parentNode = treeList_Medicine.AppendNode(new object[] { dr["categorytwo"].ToString(), "Folder", dr["categorytwo"].ToString() }, null);
                        m_HaveBindNode.Add(dr["categorytwo"].ToString() + "1");
                        DataTable secordDrug = m_SqlManger.GetMedicaineTreeSec(dr["categorytwo"].ToString());
                        MakeTreetwo(secordDrug, parentNode);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 根据二级节点加载树
        /// 王冀 2012 10 31
        /// </summary>
        /// <param name="rootDrug"></param>
        private void MakeTwoone(DataTable rootDrug)
        {
            try
            {
                foreach (DataRow dr in rootDrug.Rows)
                {
                    TreeListNode parentNode = null;//
                    if (!m_HaveBindNode.Contains(dr["categorytwo"].ToString() + "1"))
                    {
                        parentNode = treeList_Medicine.AppendNode(new object[] { dr["categorytwo"].ToString(), "Folder", dr["categorytwo"].ToString() }, null);
                        m_HaveBindNode.Add(dr["categorytwo"].ToString() + "1");
                        DataRow[] root = rootDrug.Select("categorytwo = '" + dr["categorytwo"].ToString() + "'");
                        MakeTreetwo(root, parentNode);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 根据三级节点加载树
        /// 王冀 2012 10 31
        /// </summary>
        /// <param name="rootDrug"></param>
        private void MakeThreeOne(DataTable rootDrug)
        {
            try
            {
                foreach (DataRow dr in rootDrug.Rows)
                {
                    TreeListNode parentNode = null;//
                    if (!m_HaveBindNode.Contains(dr["categorytwo"].ToString() + "1"))
                    {
                        parentNode = treeList_Medicine.AppendNode(new object[] { dr["categorytwo"].ToString(), "Folder", dr["categorytwo"].ToString() }, null);
                        m_HaveBindNode.Add(dr["categorytwo"].ToString() + "1");
                        DataRow[] dataRows = rootDrug.Select("categorytwo='" + dr["categorytwo"].ToString() + "'");
                        MakeThreeTwoTwo(ToDataTable(dataRows), parentNode);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
        /// <summary>
        /// 数组DataRow[]转化成表DataTable
        /// 王冀 2012 10 31
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        public DataTable ToDataTable(DataRow[] rows)
        {
            try
            {
                if (rows == null || rows.Length == 0) return null;
                DataTable tmp = rows[0].Table.Clone();
                foreach (DataRow row in rows)
                    tmp.ImportRow(row);
                return tmp;
            }
            catch (Exception ex)//原本没有抛错
            {
                throw ex;
            }
        }
        /// <summary>
        /// 页面加载
        /// edit 王冀 2012 10 31
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCMedicine_Load(object sender, EventArgs e)
        {
            try
            {
                m_SqlManger = new SqlManger(m_app);

                m_dataTable = m_SqlManger.GetMedicaine();

                MakeTree();

                this.ActiveControl = txtName;
                this.tbQuery.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);

            }
        }
        /// <summary>
        /// 节点选择改变事件
        /// edit 王冀 2012 10 31
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_Medicine_FocusedNodeChanged_1(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (e.Node == null)
                {
                    txtName.Text = txtSpecification.Text = txtApplyTo.Text = txtReferenceUsage.Text = txtMemo.Text = "";
                    return;
                }
                if (e.Node.Tag == null || e.Node.Tag.ToString() == "")
                {
                    txtName.Text = txtSpecification.Text = txtApplyTo.Text = txtReferenceUsage.Text = txtMemo.Text = "";
                    return;
                }
                DataTable dt = m_SqlManger.GetMedicaineByID(e.Node.Tag.ToString());

                txtName.Text = dt.Rows[0]["NAME"].ToString();
                txtSpecification.Text = dt.Rows[0]["Specification"].ToString();
                txtApplyTo.Text = dt.Rows[0]["ApplyTo"].ToString();
                txtReferenceUsage.Text = dt.Rows[0]["ReferenceUsage"].ToString();
                txtMemo.Text = dt.Rows[0]["Meno"].ToString();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
                //throw;
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
        /// 王冀 2012-10-30
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                string key = this.tbQuery.Text.Trim().Replace("'", "''").Replace("*", "[*]").Replace("%", "[%]");
                m_HaveBindNode.Clear();//已绑定节点 清空 王冀 2012-10-30
                treeList_Medicine.ClearNodes();
                if (CheckKey(key))
                {
                    this.tbQuery.Focus(); return;
                }
                if (key == null || key.Trim() == string.Empty)
                {
                    DataTable rootDrug = m_SqlManger.GetMedicineTreeOne();
                    MakeTreeone(rootDrug);
                    this.tbQuery.Focus();
                }
                else
                {

                    DataTable rootDrug = m_SqlManger.GetMedicineTreeOneByKey(key);
                    MakeTreeone(rootDrug);

                    DataTable secondDrug = m_SqlManger.GetMedicaineTreeSecByKey(key);
                    MakeTwoone(secondDrug);

                    DataTable drug = m_SqlManger.GetMedicaineByKey(key);
                    MakeThreeOne(drug);
                }
                tbQuery.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }

}
