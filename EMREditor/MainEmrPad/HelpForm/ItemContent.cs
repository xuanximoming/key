using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;

namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    public partial class ItemContent : DevBaseForm
    {
        public ItemContent()
        {
            InitializeComponent();
        }
        IEmrHost m_App;

        public string NodeTitle { get; set; }
        public string NodeContent { get; set; }

        public string NodeID { get; set; }
        public string NID { get; set; }
        public string ParentNode { get; set; }
        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="nodetitle"></param>
        /// <param name="nodecontent"></param>
        public ItemContent(string nodetitle, string nodecontent, string nodeid, string nid, string pnode, IEmrHost app)
        {
            InitializeComponent();

            NodeTitle = nodetitle;
            NodeContent = nodecontent;
            NodeID = nodeid;
            NID = nid;
            ParentNode = pnode;
            m_App = app;
        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemContent_Load(object sender, EventArgs e)
        {
            txtITEMNAME.Text = NodeTitle;
            string se = string.Format(@"select content from DEPTREP where node='{0}' and parent_node='{1}' and 
                id='{2}'", NodeID, ParentNode, NID);
            DataTable de = m_App.SqlHelper.ExecuteDataTable(se);
            if (de.Rows.Count > 0)
            {
                this.ContentEdit.Text = de.Rows[0]["Content"].ToString();
            }


        }
        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

        }
        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtITEMNAME.Text))
            {
                MyMessageBox.Show("节点名称不能为空", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                txtITEMNAME.Focus();
                return;
            }
            if (string.IsNullOrEmpty(ContentEdit.Text))
            {
                MyMessageBox.Show("诊断内容不能为空", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                ContentEdit.Focus();
                return;
            }

            if (Tool.GetByteLength(txtITEMNAME.Text) > 200)
            {
                MyMessageBox.Show("节点名称字符长度不能大于200，请重新输入。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                txtITEMNAME.Focus();
                return;
            }
            if (Tool.GetByteLength(ContentEdit.Text) > 4000)
            {
                MyMessageBox.Show("内容字符长度不能大于4000，请重新输入。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                ContentEdit.Focus();
                return;
            }

            PatRecUtil prUtil = new PatRecUtil(m_App, m_App.CurrentPatientInfo);
            prUtil.UpdateNodeContent(NID, NodeID, ParentNode, DS_Common.FilterSpecialCharacter(ContentEdit.EditValue.ToString()), DS_Common.FilterSpecialCharacter(txtITEMNAME.Text));

            NodeTitle = this.txtITEMNAME.Text;
            NodeContent = this.ContentEdit.EditValue.ToString();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public void SetTitleContent()
        {
            this.txtITEMNAME.Text = NodeTitle;
            this.ContentEdit.Text = NodeContent;
        }
    }
}