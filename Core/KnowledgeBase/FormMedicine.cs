using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;

namespace DrectSoft.Core.KnowledgeBase
{
    public partial class FormMedicine : DevBaseForm, IStartPlugIn
    {
        IEmrHost m_app;

        SqlManger m_SqlManger;

        DataTable m_dataTable;

        public FormMedicine(IEmrHost _app)
        {
            InitializeComponent();

            m_app = _app;
        }


        private void MakeTree()
        {

            DataTable rootDrug;
            DataTable secordDrug;
            DataTable drug;

            rootDrug = m_SqlManger.GetMedicineTreeOne();

            //第一级
            foreach (DataRow dr in rootDrug.Rows)
            {

                TreeListNode parentNode = null;//
                parentNode = treeList_Medicine.AppendNode(new object[] { dr["categorytwo"].ToString(), dr["categorytwo"].ToString() }, null);

                secordDrug = m_SqlManger.GetMedicaineTreeSec(dr["categorytwo"].ToString());

                //第二级
                foreach (DataRow secdr in secordDrug.Rows)
                {
                    TreeListNode node = null;//
                    node = treeList_Medicine.AppendNode(new object[] { secdr["categorythree"].ToString(), secdr["categorythree"].ToString() }, parentNode);

                    drug = m_SqlManger.GetMedicaineByThreeName(secdr["categorythree"].ToString());

                    foreach (DataRow threedr in drug.Rows)
                    {
                        TreeListNode threenode = null;//
                        threenode = treeList_Medicine.AppendNode(new object[] { threedr["Name"].ToString(), threedr["ID"].ToString() }, node);
                        threenode.Tag = threedr["id"].ToString();
                    }
                }

            }
        }


        private void FormMedicine_Load(object sender, EventArgs e)
        {
            m_SqlManger = new SqlManger(m_app);

            m_dataTable = m_SqlManger.GetMedicaine();

            MakeTree();

            this.ActiveControl = txtName;
        }

        #region IStartPlugIn 成员
        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;

            return plg;
        }
        #endregion

        private void treeList_Medicine_DoubleClick(object sender, EventArgs e)
        {
            if (treeList_Medicine == null)
                return;
            if (treeList_Medicine.Tag == null || treeList_Medicine.Tag.ToString() == "")
                return;

            DataTable dt = m_SqlManger.GetMedicaineByID(treeList_Medicine.Tag.ToString());

            txtName.Text = dt.Rows[0]["NAME"].ToString();
            txtSpecification.Text = dt.Rows[0]["Specification"].ToString();
            txtApplyTo.Text = dt.Rows[0]["ApplyTo"].ToString();
            txtReferenceUsage.Text = dt.Rows[0]["ReferenceUsage"].ToString();
            txtMemo.Text = dt.Rows[0]["Meno"].ToString();
        }

        private void treeList_Medicine_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node == null)
                return;
            if (e.Node.Tag == null || e.Node.Tag.ToString() == "")
                return;

            DataTable dt = m_SqlManger.GetMedicaineByID(e.Node.Tag.ToString());

            txtName.Text = dt.Rows[0]["NAME"].ToString();
            txtSpecification.Text = dt.Rows[0]["Specification"].ToString();
            txtApplyTo.Text = dt.Rows[0]["ApplyTo"].ToString();
            txtReferenceUsage.Text = dt.Rows[0]["ReferenceUsage"].ToString();
            txtMemo.Text = dt.Rows[0]["Meno"].ToString();
        }


    }
}
