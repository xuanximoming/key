using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Library.EmrEditor.Src.Gui;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.TemplateFactory
{
    public partial class SearchFrm : DevBaseForm
    {

        public SearchFrm(ZYEditorControl pnltext)
        {
            InitializeComponent();
            _pnlText = pnltext;
            this.txtSearch1.Focus();
            this.ActiveControl = txtSearch1;
        }
        public ZYEditorControl pnlText
        {
            get
            {
                return _pnlText;
            }
        }

        private ZYEditorControl _pnlText;
        // static SearchFrm instance = null;
        //static public SearchFrm GetInstance(EditorFrm eFrm)
        //{
        //        if (instance != null)
        //            return instance;
        //        else
        //            return instance = new SearchFrm(eFrm);
        //}
        private void Search_Load(object sender, EventArgs e)
        {
            this.txtSearch1.Focus();
            this.ActiveControl = txtSearch1;
        }

        private void cmdSearch1_Click(object sender, EventArgs e)
        {
            this.pnlText.EMRDoc._Find(this.txtSearch1.Text);
        }

        private void txtSearch1_TextChanged(object sender, EventArgs e)
        {
            this.txtSearch2.Text = this.txtSearch1.Text;
        }

        private void txtSearch2_TextChanged(object sender, EventArgs e)
        {
            this.txtSearch1.Text = this.txtSearch2.Text;
        }


        private void cmdSearch2_Click(object sender, EventArgs e)
        {
            this.pnlText.EMRDoc._Find(this.txtSearch2.Text);
        }

        private void cmdReplace_Click(object sender, EventArgs e)
        {
            this.pnlText.EMRDoc._Replace(this.txtSearch2.Text, this.txtReplace.Text);
        }

        private void txtSearch1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmdSearch1_Click(null,null);
            }
        }

        private void cmdSearch2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmdSearch2_Click(null, null);
            }
        }

        private void txtReplace_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmdReplace_Click(null, null);
            }
        }

        /// <summary>
        /// tab页切换事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-07</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.tabControl1.SelectedTab == this.Search)
                {
                    this.txtSearch1.Focus();
                }
                else
                {
                    this.txtSearch2.Focus();
                }
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
