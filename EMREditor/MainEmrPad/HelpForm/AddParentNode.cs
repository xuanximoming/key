using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.MainEmrPad.HelpForm
{
    public partial class AddParentNode : DevBaseForm
    {
        IEmrHost m_App;
        public string DiagID { get; set; }
        public int NodeID { get; set; }
        public int ParentNodeID { get; set; }
        public string Title { get; set; }
        private EditState m_EditState = EditState.Add;
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

        public AddParentNode(IEmrHost app, EditState editState)
        {
            InitializeComponent();
            m_App = app;
            m_EditState = editState;
        }

        public void InitNode(string diagid, int node, int parentNode,string title)
        {
            try
            {
                DiagID = diagid;
                NodeID = node;
                ParentNodeID = parentNode;
                Title = title;
                if (m_EditState == EditState.Edit)
                {
                    this.txtITEMNAME.Text = title;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtITEMNAME.Text))
                {
                    return;
                }
                if (Tool.GetByteLength(txtITEMNAME.Text) > 200)
                {
                    MyMessageBox.Show("备注最大字符长度为200，请重新输入。", "提示", MyMessageBoxButtons.Ok, DrectSoft.Common.Ctrs.DLG.MessageBoxIcon.WarningIcon);
                    return;
                }
                string m_title; //string m_content;
                string m_node;
                string m_parentnode;

                PatRecUtil prUtil = new PatRecUtil(m_App, m_App.CurrentPatientInfo);
                if (m_EditState != EditState.Edit)
                {///新增
                    prUtil.InsertParentNode(DiagID, NodeID.ToString(), ParentNodeID.ToString(), DS_Common.FilterSpecialCharacter(this.txtITEMNAME.Text), string.Empty, string.Empty, "1", out m_node, out m_parentnode);
                    SetTitleContent(out  m_title);
                    SetNodeAndPNode(m_node, m_parentnode);
                }
                else
                {///编辑
                    prUtil.UpdateParentNode(DiagID, NodeID, ParentNodeID, DS_Common.FilterSpecialCharacter(this.txtITEMNAME.Text), string.Empty);
                    Title = this.txtITEMNAME.Text;
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
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