using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class CheckTipContent : DevExpress.XtraEditors.XtraForm
    {
        public CheckTipContent()
        {
            InitializeComponent();
        }
        IEmrHost m_App;
        private string Content { get; set; }
        private UCEmrInput m_Umr;
        public string IsChecked { get; set; }
        public CheckTipContent(IEmrHost app, string cone, UCEmrInput umr)
        {
            Content = cone;
            m_App = app; 
            m_Umr = umr;
            InitializeComponent();
        }

        public void RefreshContent(string content)
        {
            Content = content;

            this.richTextBox1.Text += Content;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            IsChecked = "1";
            Content = m_Umr.GetReturnContent();
            this.richTextBox1.Text = Content;
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckTipContent_Load(object sender, EventArgs e)
        {
            this.richTextBox1.Text = Content;
        }
    }
}