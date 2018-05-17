using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Core.DoctorAdvice;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.OrderManage {
    /// <summary>
    /// 医嘱录入模块
    /// </summary>
    public partial class PatientOrderQuery : NoCaptionBarForm, IStartPlugIn {
        private AdviceEditor m_AdviceEditor;
        private IEmrHost m_App;

        /// <summary>
        /// 
        /// </summary>
        public PatientOrderQuery() {
            InitializeComponent();
        }

        #region IStartup 成员

        #endregion

        private void CreateAdviceEditor() {
            if (m_AdviceEditor != null)
                return;
            // 创建控件
            m_AdviceEditor = new AdviceEditor(m_App, EditorCallModel.Query);
            m_AdviceEditor.Location = new Point(0, 0);
            m_AdviceEditor.Dock = DockStyle.Fill;

            AutoScrollMinSize = m_AdviceEditor.MinimumSize;
            Controls.Add(m_AdviceEditor);
            FormClosing += m_AdviceEditor.OnParentFormClosing;
        }

        //TODO 
        //private void DoPatientChanged(object Sender, PatientArgs arg) {
        //    m_AdviceEditor.CheckCanExitOrSwitch(arg.PatInfo);
        //    if (!IsDisposed)
        //        m_AdviceEditor.CallShowPatientOrder(arg.PatInfo);
        //}

        private void AdviceInputor_Shown(object sender, EventArgs e) {
            if ((m_App.CurrentPatientInfo == null) || (m_App.CurrentPatientInfo.NoOfHisFirstPage == "-1"))
                m_App.CustomMessageBox.MessageShow("请先选择病人", CustomMessageBoxKind.InformationOk);
        }

        public IPlugIn Run(IEmrHost host) {

            m_App = host;

            CreateAdviceEditor();

            if (m_App.CurrentPatientInfo != null)
                m_AdviceEditor.CallShowPatientOrder(m_App.CurrentPatientInfo);

            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            //TODO
            //plg.PatientChanged += new PatientChangedHandler(DoPatientChanged);

            return plg;

        }
    }
}