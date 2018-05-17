using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Emr.Util;
using DevExpress.XtraTreeList.Nodes;

namespace DrectSoft.Core.EMR_NursingDocument.EMRInput.Table
{
    public partial class MainForm : Form, IStartPlugIn
    {
        private IEmrHost m_app;
        RecordDal m_RecordDal;
        UCEmrInput m_UCEmrInput;

        public MainForm()
        {
            InitializeComponent();
            AddUcEmrInput();
            FormClosing += new FormClosingEventHandler(UCEmrInput_FormClosing);
        }

        private void AddUcEmrInput()
        {
            m_UCEmrInput = new UCEmrInput();
            //m_UCEmrInput.HideBar();
            m_UCEmrInput.Dock = DockStyle.Fill;
            this.Controls.Add(m_UCEmrInput);
        }

        void UCEmrInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_UCEmrInput.CloseAllTabPages();
        }

        #region IStartPlugIn 成员

        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            plg.PatientChanging += new PatientChangingHandler(plg_PatientChanging);
            plg.PatientChanged += new PatientChangedHandler(plg_PatientChanged);
            m_app = host;
            m_RecordDal = new RecordDal(m_app.SqlHelper);
            m_UCEmrInput.SetInnerVar(m_app, m_RecordDal);
            m_UCEmrInput.CurrentInpatient = m_app.CurrentPatientInfo;
            return plg;
        }

        void plg_PatientChanging(object sender, CancelEventArgs arg)
        {
            m_UCEmrInput.PatientChanging();
        }

        void plg_PatientChanged(object Sender, PatientArgs arg)
        {
            m_UCEmrInput.PatientChanged(m_app.CurrentPatientInfo);
        }

        #endregion
    }
}