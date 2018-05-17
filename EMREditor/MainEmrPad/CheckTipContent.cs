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

namespace DrectSoft.Core.MainEmrPad
{
    public partial class CheckTipContent : DevBaseForm
    {
        public CheckTipContent()
        {
            InitializeComponent();
        }
        IEmrHost m_App;
        private string Content = string.Empty;
        private UCEmrInput m_Umr;
        private DrectSoft.Core.MainEmrPad.New.UCEmrInput m_UmrNew;
        public string IsChecked { get; set; }
        public CheckTipContent(IEmrHost app, string cone, UCEmrInput umr)
        {
            Content = cone;
            m_App = app; 
            m_Umr = umr;
            InitializeComponent();
        }

        public CheckTipContent(IEmrHost app, string cone, DrectSoft.Core.MainEmrPad.New.UCEmrInput umr)
        {
            InitializeComponent();

            Content = cone;
            m_App = app;
            m_UmrNew = umr;
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
            try
            {
                IsChecked = "1";
                if (null != m_UmrNew)
                {
                    m_UmrNew.CheckInpatient((int)m_App.CurrentPatientInfo.NoOfFirstPage, out Content);
                }
                else if (null != m_Umr)
                {
                    Content = m_Umr.GetReturnContent();
                }
                this.richTextBox1.Text = Content;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(ex.Message);
            }
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