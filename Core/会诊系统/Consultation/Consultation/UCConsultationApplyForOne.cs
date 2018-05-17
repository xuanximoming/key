using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.Consultation
{
    public partial class UCConsultationApplyForOne : DevExpress.XtraEditors.XtraUserControl
    {
        private string m_NoOfFirstPage = string.Empty;
        private IYidanEmrHost m_Host;
        private string m_ConsultApplySn = string.Empty;

        public UCConsultationApplyForOne()
        {
            InitializeComponent();
        }

        public void Init(string noOfFirstPage, IYidanEmrHost host, bool isNew, bool readOnly, string consultApplySn)
        {
            m_NoOfFirstPage = noOfFirstPage;
            m_Host = host;
            m_ConsultApplySn = consultApplySn;
            InitInner(isNew, readOnly);
        }

        private void InitInner(bool isNew, bool readOnly)
        {
            UCPatientInfoForOne.Init(m_NoOfFirstPage, m_Host);
            UCApplyInfoForOne.Init(m_NoOfFirstPage, m_Host, isNew, readOnly, m_ConsultApplySn);
        }

        private void simpleButtonClear_Click(object sender, EventArgs e)
        {
            if (m_Host.CustomMessageBox.MessageShow("确定要清屏吗?", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                UCApplyInfoForOne.Clear();
            }
        }

        private void simpleButtonConfirm_Click(object sender, EventArgs e)
        {
            UCApplyInfoForOne.Save();
        }

        private void simpleButtonExit_Click(object sender, EventArgs e)
        {
            if (m_Host.CustomMessageBox.MessageShow("确定要退出吗?", CustomMessageBoxKind.QuestionYesNo) == DialogResult.Yes)
            {
                XtraForm form = this.Parent.Parent.Parent as XtraForm;
                if (form != null)
                {
                    form.Close();
                }
            }
        }

        public void ReadOnlyControl()
        {
            UCApplyInfoForOne.ReadOnlyControl();
            simpleButtonClear.Visible = false;
            simpleButtonConfirm.Visible = false;
            simpleButtonExit.Visible = false;
        }
    }
}
