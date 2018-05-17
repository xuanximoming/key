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
    public partial class AddParentNode : DevExpress.XtraEditors.XtraForm
    {
        IEmrHost m_App;
        public string DiagID { get; set; }
        public AddParentNode()
        {
            InitializeComponent();
        }
        public AddParentNode(IEmrHost app, string diagid)
        {
            m_App = app;
            DiagID = diagid;
            InitializeComponent();
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtITEMNAME.Text))
            {
                return;
            }
            string m_title; //string m_content;
            string m_node;
            string m_parentnode;

            PatRecUtil prUtil = new PatRecUtil(m_App, m_App.CurrentPatientInfo);
            prUtil.InsertParentNode(DiagID, "node", DiagID, this.txtITEMNAME.Text, "content", "indexid", "1", out m_node, out m_parentnode);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            SetTitleContent(out  m_title);
            SetNodeAndPNode(m_node, m_parentnode);
            this.Close();
        }
        public string RNode { get; set; }
        public string RPNode { get; set; }
        public void SetTitleContent(out string m_title)
        {
            m_title = this.txtITEMNAME.Text;
            //RNode = c_node;
            //RPNode = p_node;
            //c_node = cnode;
            //p_node = pnode;
            //m_content = this.ContentEdit.Text;

        }
        public void SetNodeAndPNode(string node, string pnode)
        {
            RNode = node;
            RPNode = pnode;
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}