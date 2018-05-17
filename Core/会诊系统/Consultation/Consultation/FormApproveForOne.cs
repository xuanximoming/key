using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.Consultation
{
    public partial class FormApproveForOne : DevExpress.XtraEditors.XtraForm
    {
        private string m_NoOfFirstPage = string.Empty;
        private IYidanEmrHost m_Host;
        private string m_ConsultApplySn = string.Empty;

        public FormApproveForOne()
        {
            InitializeComponent();
            ApplyForOne.ReadOnlyControl();
        }

        public FormApproveForOne(string noOfFirstPage, IYidanEmrHost host, string consultApplySn)
            : this()
        {
            m_NoOfFirstPage = noOfFirstPage;
            m_Host = host;
            m_ConsultApplySn = consultApplySn;
            InitInner(false);
        }

        private void InitInner(bool isNew)
        {
            ApplyForOne.Init(m_NoOfFirstPage, m_Host, isNew, true, m_ConsultApplySn);
        }

        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            Dal.DataAccess.ModifyConsultationData(m_ConsultApplySn, "1", Convert.ToString((int)ConsultStatus.WaitConsultation), memoEditSuggestion.Text.Trim());
            m_Host.CustomMessageBox.MessageShow("审核成功!", CustomMessageBoxKind.InformationOk);
            this.Close();
        }

        private void simpleButtonReject_Click(object sender, EventArgs e)
        {
            if (memoEditSuggestion.Text.Trim() == "")
            {
                m_Host.CustomMessageBox.MessageShow("请输入否决原因!", CustomMessageBoxKind.WarningOk);
                memoEditSuggestion.Focus();
                return;
            }

            Dal.DataAccess.ModifyConsultationData(m_ConsultApplySn, "1", Convert.ToString((int)ConsultStatus.Reject), memoEditSuggestion.Text.Trim());
            m_Host.CustomMessageBox.MessageShow("否决成功!", CustomMessageBoxKind.InformationOk);
            this.Close();
        }

        public void HideApproveArea()
        {
            groupControlApprove.Visible = false;
            ApplyForOne.Location = groupControlApprove.Location;
            this.Height = ApplyForOne.Height + 50;
            this.Text = "会诊信息";
        }

        public void ReadOnlyControl()
        {
            simpleButtonOK.Enabled = false;
            simpleButtonReject.Enabled = false;

            DataSet ds = Dal.DataAccess.GetConsultationDataSet(m_ConsultApplySn, "20", Convert.ToString((int)ConsultType.One));
            DataTable dtConsultApply = ds.Tables[0];
            memoEditSuggestion.Text = dtConsultApply.Rows[0]["RejectReason"].ToString().Trim();
            string stateID = dtConsultApply.Rows[0]["stateid"].ToString().Trim();

            if (stateID == Convert.ToString((int)ConsultStatus.WaitApprove))//未审核隐藏审核信息
            {
                HideApproveArea();
            }
            //if (memoEditSuggestion.Text == "")
            //{
            //    HideApproveArea();
            //}
        }
    }
}