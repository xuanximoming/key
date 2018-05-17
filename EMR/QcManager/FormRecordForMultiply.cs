using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Emr.Util;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.QcManager
{
    public partial class FormRecordForMultiply : DevBaseForm
    {
        private string m_NoOfFirstPage = string.Empty;
        private IEmrHost m_Host;
        private string m_ConsultApplySn = string.Empty;

        public FormRecordForMultiply()
        {
            InitializeComponent();
        }

        public FormRecordForMultiply(string noOfFirstPage, IEmrHost host, string consultApplySn)
            : this()
        {
            m_NoOfFirstPage = noOfFirstPage;
            m_Host = host;
            m_ConsultApplySn = consultApplySn;
            InitInner(false);
        }

        private void InitInner(bool isNew)
        {
            UCRecordForMultiply recordForMultiply = new UCRecordForMultiply(m_NoOfFirstPage, m_Host, m_ConsultApplySn);
            recordForMultiply.Location = new Point(40, 10);
            recordForMultiply.ApplyInfoReadOnly();
            panelRecord.Controls.Add(recordForMultiply);
        }

        public void ReadOnlyControl()
        {
            foreach (Control control in panelRecord.Controls)
            {
                UCRecordForMultiply recordForMultiply = control as UCRecordForMultiply;
                recordForMultiply.ReadOnlyControl();
                break;
            }
        }

        #region 病历内容

        ///// <summary>
        ///// 病历内容窗体
        ///// </summary>
        UCEmrInput m_UCEmrInput;
        bool m_IsLoadedEmrContent = false;
        private void LoadEmrContent()
        {
            if (!string.IsNullOrEmpty(m_NoOfFirstPage) && !m_IsLoadedEmrContent)
            {
                m_UCEmrInput.PatientChangedByIEmrHost(Convert.ToDecimal(m_NoOfFirstPage));
                m_UCEmrInput.HideBar();
                m_IsLoadedEmrContent = true;
            }
        }

        private void AddEmrInput()
        {
            m_UCEmrInput = new UCEmrInput("正在加载会诊信息");
            m_UCEmrInput.CurrentInpatient = null;
            m_UCEmrInput.HideBar();
            RecordDal m_RecordDal = new RecordDal(m_Host.SqlHelper);
            m_UCEmrInput.SetInnerVar(m_Host, m_RecordDal);
            xtraTabPageEmrContent.Controls.Add(m_UCEmrInput);
            m_UCEmrInput.Dock = DockStyle.Fill;
        }
        #endregion

        private void FormRecordForMultiply_Load(object sender, EventArgs e)
        {
            AddEmrInput();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == xtraTabPageEmrContent)
            {
                LoadEmrContent();
            }
        }
    }
}
