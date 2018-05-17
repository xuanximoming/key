using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.KnowledgeBase
{
    public partial class MainForm : DevBaseForm, IStartPlugIn
    {
        IEmrHost m_app;
        public MainForm()
        {
            InitializeComponent();
        }

        //xtraTabPageMedicine

        public IPlugIn Run(FrameWork.WinForm.Plugin.IEmrHost host)
        {
            IPlugIn plg = new PlugIn(this.GetType().ToString(), this);
            m_app = host;

            return plg;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UCMedicine m_UCMedicine = new UCMedicine(m_app);

            xtraTabPageMedicine.Controls.Add(m_UCMedicine);
            m_UCMedicine.Dock = DockStyle.Fill;
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page.Name == "xtraTabPageMedicine")
            {
                if (xtraTabPageMedicine.Controls.Count > 0)
                    return;

                UCMedicine m_UCMedicine = new UCMedicine(m_app);

                xtraTabPageMedicine.Controls.Add(m_UCMedicine);
                m_UCMedicine.Dock = DockStyle.Fill;

            }
            else if (e.Page.Name == "tabpageMedicineDirect")
            {
                if (tabpageMedicineDirect.Controls.Count > 0)
                    return;

                UCMedicineDirect m_UCMedicineDirect = new UCMedicineDirect(m_app);

                tabpageMedicineDirect.Controls.Add(m_UCMedicineDirect);
                m_UCMedicineDirect.Dock = DockStyle.Fill;
            }
            else if (e.Page.Name == "tablePageTreatment")
            {
                if (tablePageTreatment.Controls.Count > 0)
                    return;

                UCTreatmentDirect m_UCTreatmentDirect = new UCTreatmentDirect(m_app);

                tablePageTreatment.Controls.Add(m_UCTreatmentDirect);
                m_UCTreatmentDirect.Dock = DockStyle.Fill;
            }
        }

    }
}