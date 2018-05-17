using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YidanSoft.Library.EmrEditor.Src.Gui;

namespace Yidansoft.Core.MainEmrPad
{
    public partial class SearchFrm : Form
    {

        public SearchFrm(ZYEditorControl pnltext)
        {
            InitializeComponent();
            _pnlText = pnltext;
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
                this.pnlText.EMRDoc._Find(this.txtSearch1.Text);
            }
        }

        private void txtSearch2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.pnlText.EMRDoc._Find(this.txtSearch2.Text); 
            }
        }

        private void txtReplace_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.pnlText.EMRDoc._Replace(this.txtSearch2.Text, this.txtReplace.Text);
            }
        }
    }
}
