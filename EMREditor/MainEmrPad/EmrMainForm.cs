using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork;
using YidanSoft.FrameWork.WinForm;
using YidanSoft.FrameWork.WinForm.Plugin;
using YindanSoft.Emr.Util;

namespace Yidansoft.Core.MainEmrPad
{
    public partial class EmrMainForm : Form, IStartPlugIn
    {
        private IYidanEmrHost m_app;
        RecordDal m_RecordDal;
        UCEmrInput m_UCEmrInput;

        public EmrMainForm()
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

        public IPlugIn Run(YidanSoft.FrameWork.WinForm.Plugin.IYidanEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            plg.PatientChanged += new PatientChangedHandler(plg_PatientChanged);
            m_app = host;
            m_RecordDal = new RecordDal(m_app.SqlHelper);
            m_UCEmrInput.SetInnerVar(m_app, m_RecordDal);
            return plg;
        }

        void plg_PatientChanged(object Sender, PatientArgs arg)
        {
            m_UCEmrInput.PatientChanged();
        }

        #endregion
    }
}