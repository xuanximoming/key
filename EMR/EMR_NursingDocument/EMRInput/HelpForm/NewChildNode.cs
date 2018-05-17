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
    public partial class NewChildNode : DevExpress.XtraEditors.XtraForm
    {
        IEmrHost m_App;

        public string NodeTitle { get; set; }
        public string NodeContent { get; set; }

        public string NodeID { get; set; }
        public string NID { get; set; }
        public string ParentNode { get; set; }
        public string DiagID { get; set; }
        public NewChildNode()
        {
            InitializeComponent();
        }
        public NewChildNode(string nodetitle, string nodecontent, string nodeid, string nid, string pnode, IEmrHost app, string diagid)
        {
            NodeTitle = nodetitle;
            NodeContent = nodecontent;
            NodeID = nodeid;
            NID = nid;
            ParentNode = pnode;
            m_App = app;
            DiagID = diagid;
            InitializeComponent();
        }

        /// <summary>
        /// 确定操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            PatRecUtil prUtil = new PatRecUtil(m_App, m_App.CurrentPatientInfo);
            if (string.IsNullOrEmpty(txtITEMNAME.Text)&&string.IsNullOrEmpty(ContentEdit.Text))
            {
                return;
            }
                string m_node;
            string m_parentnode;
            prUtil.InsertChildNode(DiagID, "node", NodeID, this.txtITEMNAME.Text, this.ContentEdit.Text, "indexid", "1",out m_node,out m_parentnode);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            string m_title; string m_content;
       
            SetTitleContent(out  m_title,out  m_content);
            SetNodeAndPNode(m_node, m_parentnode);
            this.Close();

        }
        public string RNode { get; set; }
        public string RPNode { get; set; }
        public void SetNodeAndPNode(string node, string pnode)
        {
            RNode = node;
            RPNode = pnode;
        }

        public void SetTitleContent(out string m_title, out string m_content)
        {
            m_title = this.txtITEMNAME.Text;
            m_content = this.ContentEdit.Text;
            
        }
        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 窗体加载 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewChildNode_Load(object sender, EventArgs e)
        {
            this.txtClass.Text = NodeTitle;
        }

    }
}