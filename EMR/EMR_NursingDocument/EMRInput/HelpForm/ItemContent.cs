using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Core.EMR_NursingDocument.EMRInput.Table;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.HelpForm
{
    public partial class ItemContent : DevExpress.XtraEditors.XtraForm
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
            NodeTitle = nodetitle;
            NodeContent = nodecontent;
            NodeID = nodeid;
            NID = nid;
            ParentNode = pnode;
            m_App = app;
            InitializeComponent();

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
            PatRecUtil prUtil = new PatRecUtil(m_App, m_App.CurrentPatientInfo);
            prUtil.UpdateNodeContent(NID, NodeID, ParentNode, ContentEdit.EditValue.ToString(), txtITEMNAME.Text);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            string m_title = this.txtITEMNAME.Text; string m_content = this.ContentEdit.EditValue.ToString();

            SetTitleContent(out  m_title, out  m_content);
            this.Close();
        }

        public void SetTitleContent(out string m_title, out string m_content)
        {
            m_title = this.txtITEMNAME.Text;
            m_content = this.ContentEdit.Text;

        }


    }
}