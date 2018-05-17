using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Core.DoctorAdvice;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm.Plugin;

namespace DrectSoft.Core.OrderManage {
    /// <summary>
    /// 成套医嘱维护界面
    /// </summary>
    public partial class SuiteMaintenance : NoCaptionBarForm, IStartPlugIn {
        private AdviceEditor m_AdviceEditor;
        private IEmrHost m_App;
        private SuiteChoice m_Suite;

        /// <summary>
        /// 医嘱维护
        /// </summary>
        public SuiteMaintenance() {
            InitializeComponent();
        }

        #region IStartup 成员
        /// <summary>
        /// IPlugIn
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public IPlugIn Run(IEmrHost application) {
            m_App = application;

            // 创建成套明细编辑组件
            CreateAdviceEditor();

            m_Suite = new SuiteChoice();
            m_Suite.App = m_App;
            m_Suite.InitializeOrderSuiteChoiceForm(m_App.SqlHelper, m_App.CustomMessageBox, m_AdviceEditor.SuiteHelper, true);
            AddPersonal();
            AddDept();
            m_Suite.AfterChoicedEditModel += new EventHandler<DataCommitArgs>(DoAfterSelectedEditModel);
            m_Suite.Dock = DockStyle.Fill;
            this.dockPanel2.Controls.Add(m_Suite);
            //this.splitContainerControl1.Panel1.Controls.Add(m_Suite);

            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            //DockingForm dockForm = new DockingForm(m_Suite, DockingSite.Left);         
            //plg.AddInDockingForms.Add(dockForm);

            return plg;
        }

        #endregion

        /// <summary>
        /// 增加个人栏
        /// </summary>
        public virtual void AddPersonal() {
            if (m_Suite != null)
                m_Suite.AddPersonal();
        }

        /// <summary>
        /// 增加科室栏
        /// </summary>
        public virtual void AddDept() {
            if (m_Suite != null)
                m_Suite.AddDept();
        }
        /// <summary>
        /// 增加全院栏
        /// </summary>
        public virtual void AddHospital() {
            if (m_Suite != null)
                m_Suite.AddHospital();
        }

        private void CreateAdviceEditor() {
            if (m_AdviceEditor != null)
                return;
            // 创建控件
            m_AdviceEditor = new AdviceEditor(m_App, EditorCallModel.EditSuite);
            m_AdviceEditor.Location = new Point(0, 0);
            m_AdviceEditor.Dock = DockStyle.Fill;
            Controls.Add(m_AdviceEditor);
            FormClosing += m_AdviceEditor.OnParentFormClosing;
        }

        private void DoAfterSelectedEditModel(object sender, DataCommitArgs e) {
            propertyGridSuiteInfo.SelectedObject = m_Suite.SelectedSuiteObject;
            // 取明细数据进行编辑
            m_AdviceEditor.CallEditSuiteOrder(m_Suite.SelectedSuiteNo);
            if (m_Suite.SelectedSuiteNo > 0)
                Text = "成套医嘱维护－－" + m_Suite.SelectedSuiteObject.Name;
            else
                Text = "成套医嘱维护";
        }

        private void SuiteMaintenance_Load(object sender, EventArgs e) {

        }
    }
}