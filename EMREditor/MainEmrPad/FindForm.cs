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
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;

namespace DrectSoft.Core.MainEmrPad
{
    public partial class SearchFrm : DevBaseForm
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
            try
            {
                txtSearch1.Text = _pnlText.EMRDoc.Content.GetSelectedText();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void cmdSearch1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch1.Text.Trim() == "")
                {
                    MessageBox.Show("查找内容不能为空！");
                    txtSearch1.Focus();
                }
                else
                {
                    bool isOK = this.pnlText.EMRDoc._Find(this.txtSearch1.Text);
                    if (!isOK)
                    {
                        m_App.CustomMessageBox.MessageShow("查询无结果", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void txtSearch1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtSearch2.Text = this.txtSearch1.Text;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void txtSearch2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                this.txtSearch1.Text = this.txtSearch2.Text;
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void cmdSearch2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch2.Text.Trim() == "")
                {
                    MessageBox.Show("查找的内容不能为空！");
                    txtSearch2.Focus();
                }
                else
                {
                    bool isOK = this.pnlText.EMRDoc._Find(this.txtSearch2.Text);
                    if (!isOK)
                    {
                        m_App.CustomMessageBox.MessageShow("查询无结果", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void cmdReplace_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch2.Text.Trim() == "")
                {
                    MessageBox.Show("查找的内容不能为空！");
                    txtSearch2.Focus();
                }
                else
                {
                    bool isOK = this.pnlText.EMRDoc._Find(this.txtSearch2.Text);
                    if (!isOK)
                    {
                        m_App.CustomMessageBox.MessageShow("没有需要替换的内容", DrectSoft.Core.CustomMessageBoxKind.InformationOk);
                    }
                    else
                    {
                        this.pnlText.EMRDoc._Replace(this.txtSearch2.Text, this.txtReplace.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void txtSearch1_KeyDown(object sender, KeyEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void txtSearch2_KeyDown(object sender, KeyEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void cmdReplace_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    this.pnlText.EMRDoc._Replace(this.txtSearch2.Text, this.txtReplace.Text);
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void SearchFrm_Activated(object sender, EventArgs e)
        {
            try
            {
                this.txtSearch1.Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }
    }
}