using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Library.EmrEditor.Src.Gui;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class SearchFrm : DevExpress.XtraEditors.XtraForm
    {
        public ZYEditorControl pnlText
        {
            get
            {
                return _pnlText;
            }
        }
        private ZYEditorControl _pnlText;
        private IEmrHost m_App;

        public SearchFrm()
        {
            InitializeComponent();
        }

        public SearchFrm(IEmrHost app, ZYEditorControl pnltext)
        {
            InitializeComponent();
            _pnlText = pnltext;
            m_App = app;
        }

        private void SearchFrm_Load(object sender, EventArgs e)
        {
            txtSearch1.Text = _pnlText.EMRDoc.Content.GetSelectedText();
        }

        private void cmdSearch1_Click(object sender, EventArgs e)
        {
            bool isOK = this.pnlText.EMRDoc._Find(this.txtSearch1.Text);
            if (!isOK)
            {
                m_App.CustomMessageBox.MessageShow("查询无结果！", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
            }
        }

        private void txtSearch1_EditValueChanged(object sender, EventArgs e)
        {
            this.txtSearch2.Text = this.txtSearch1.Text;
        }

        private void txtSearch2_EditValueChanged(object sender, EventArgs e)
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
                bool isOK = this.pnlText.EMRDoc._Find(this.txtSearch1.Text);
                if (!isOK)
                {
                    m_App.CustomMessageBox.MessageShow("查询无结果！", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                }
            }
        }

        private void txtSearch2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool isOK = this.pnlText.EMRDoc._Find(this.txtSearch2.Text);
                if (!isOK)
                {
                    m_App.CustomMessageBox.MessageShow("查询无结果！", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                }
            }
        }

        private void cmdReplace_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.pnlText.EMRDoc._Replace(this.txtSearch2.Text, this.txtReplace.Text);
            }
        }

        private void SearchFrm_Activated(object sender, EventArgs e)
        {
            this.txtSearch1.Focus();
        }
    }
}