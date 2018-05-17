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
using DevExpress.Utils;
using DrectSoft.Common;
using DrectSoft.Service;

namespace DrectSoft.Core.Consultation
{
    public partial class FormRecordForMultiply : DevExpress.XtraEditors.XtraForm
    {
        private string m_NoOfFirstPage = string.Empty;
        private IEmrHost m_Host;
        private string m_ConsultApplySn = string.Empty;
        private UCRecordForMultiply recordForMultiply;

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
            recordForMultiply = new UCRecordForMultiply(m_NoOfFirstPage, m_Host, m_ConsultApplySn);
            recordForMultiply.Location = new Point(0, 0);
            recordForMultiply.ApplyInfoReadOnly();
            xtraTabPageRecord.Controls.Add(recordForMultiply);
            recordForMultiply.Left = 0;
            this.ActiveControl = xtraTabPageRecord;
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

        #region 病历内容 - 老版

        /// <summary>
        /// 病历内容窗体
        /// </summary>
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

        #region 病历内容 - 新版
        /// <summary>
        /// 病历内容窗体
        /// </summary>
        DrectSoft.Core.MainEmrPad.New.UCEmrInput m_UCEmrInputNew;
        bool m_IsLoadedEmrContentNew = false;

        /// <summary>
        /// 加载病历
        /// </summary>
        private void AddEmrInputNew()
        {
            try
            {
                if (string.IsNullOrEmpty(m_NoOfFirstPage) || m_IsLoadedEmrContentNew)
                {
                    return;
                }
                m_Host.ChoosePatient(Convert.ToDecimal(m_NoOfFirstPage), FloderState.None.ToString());//切换病人

                m_UCEmrInputNew = new DrectSoft.Core.MainEmrPad.New.UCEmrInput(m_Host.CurrentPatientInfo, m_Host, FloderState.None);
                m_UCEmrInputNew.SetVarData(m_Host);
                xtraTabPageEmrContent.Controls.Add(m_UCEmrInputNew);
                m_UCEmrInputNew.OnLoad();
                m_UCEmrInputNew.HideBar();
                m_UCEmrInputNew.Dock = DockStyle.Fill;
                m_IsLoadedEmrContentNew = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void FormRecordForMultiply_Load(object sender, EventArgs e)
        {
            recordForMultiply.Width = 1000;
            recordForMultiply.Height = 966;
            this.ActiveControl = recordForMultiply;
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == xtraTabPageEmrContent)
                {
                    string config = DS_SqlService.GetConfigValueByKey("IsNewUcInput");
                    if (null != config && config.Trim() == "1")
                    {
                        AddEmrInputNew();
                    }
                    else
                    {
                        AddEmrInput();
                        LoadEmrContent();
                    }
                }
                this.xtraTabControl1.SelectedTabPage.Controls[0].Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }
    }
}
