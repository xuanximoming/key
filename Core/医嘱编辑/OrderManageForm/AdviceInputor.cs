using DevExpress.Utils;
using DrectSoft.Common.Eop;
using DrectSoft.Core.DoctorAdvice;
using DrectSoft.DSSqlHelper;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.OrderManage
{
    /// <summary>
    /// 医嘱录入模块
    /// </summary>
    public partial class AdviceInputor : NoCaptionBarForm
    {
        private AdviceEditor m_AdviceEditor;
        private IEmrHost m_App;
        //private PrintOrderForm m_PrintForm;

        private WaitDialogForm m_WaitDialog;


        /// <summary>
        /// 
        /// </summary>
        public AdviceInputor()
        {
            InitializeComponent();
        }

        public AdviceInputor(IEmrHost application)
        {
            m_WaitDialog = new WaitDialogForm("正在创建用户界面...", "请稍候");

            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;//解决第三方控件异步报错的问题
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            DS_SqlHelper.CreateSqlHelper();
            m_App = application;
            CreateAdviceEditor();
            CreateSuiteBox();

            if (m_App.CurrentPatientInfo != null)
                m_AdviceEditor.CallShowPatientOrder(m_App.CurrentPatientInfo);

        }
        #region IStartup 成员 by ukey zhang 注释新增进public AdviceInputor(IEmrHost application) 和 AdviceInputorStartUp

        /// <summary>
        /// IPlugin
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        /*
        public IPlugIn Run(IEmrHost application)
        {
            m_App = application;
            CreateAdviceEditor();
            CreateSuiteBox();

            if (m_App.CurrentPatientInfo != null)
                m_AdviceEditor.CallShowPatientOrder(m_App.CurrentPatientInfo);

            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            plg.PatientChanging += new PatientChangingHandler(DoPatientChanging);
            plg.PatientChanged += new PatientChangedHandler(DoPatientChanged);

            return plg;
        }
         */
        #endregion
        private void CreateSuiteBox()
        {
            m_SuiteChoice.App = m_App;
            //todo
            //m_SuiteChoice.InitializeOrderSuiteChoiceForm(m_App.SqlHelper, m_App.CustomMessageBox, new SuiteOrderHandle(m_App, true), false);
            m_SuiteChoice.SelectedOrderSuite += new EventHandler(DoAfterSelectedOrderSuite);
            m_SuiteChoice.AddPersonal();
            m_SuiteChoice.AddDept();
            m_SuiteChoice.AddHospital();

        }

        private void CreateAdviceEditor()
        {
            if (m_AdviceEditor != null)
                return;
            // 创建控件
            m_AdviceEditor = new AdviceEditor(m_App, EditorCallModel.EditOrder);
            m_AdviceEditor.Location = new Point(0, 0);
            m_AdviceEditor.Dock = DockStyle.Fill;

            AutoScrollMinSize = m_AdviceEditor.MinimumSize;
            this.panelEditContent.Controls.Add(m_AdviceEditor);
            FormClosing += m_AdviceEditor.OnParentFormClosing;
            m_AdviceEditor.AfterSwitchOrderTable += new EventHandler(DoAfterSwitchOrderTable);
            //m_AdviceEditor.PrintCurrentPatientOrder += new AdviceEditor.PrintOrder(DoPrintPatientOrder);
        }


        private void DoAfterSelectedOrderSuite(object sender, EventArgs e)
        {
            if (m_SuiteChoice.SelectedContents != null)
            {
                m_AdviceEditor.InsertSuiteOrder(m_SuiteChoice.SelectedContents);
            }

        }

        public void DoPatientChanging(object Sender, CancelEventArgs arg)
        {
            arg.Cancel = !m_AdviceEditor.CheckCanExitOrSwitch(null);
        }

        //todo
        public void DoPatientChanged(object Sender, PatientArgs arg)
        {
            m_AdviceEditor.CheckCanExitOrSwitch(arg.PatInfo);
            if (!IsDisposed)
            {
                m_AdviceEditor.CallShowPatientOrder(arg.PatInfo);

                //m_SuiteToolBox.LoadPatientAgeSex();
            }
        }

        private void DoAfterSwitchOrderTable(object sender, EventArgs e)
        {
            //m_SuiteChoice.RefreshOrderSuiteData(m_AdviceEditor.IsTempOrder, m_AdviceEditor.AllowAddNew
            //   , m_AdviceEditor.HasOutHospitalOrder, m_AdviceEditor.FrequencyWordbook);
        }

        private void DoPrintPatientOrder(Inpatient patient)
        {
            //if (m_PrintForm == null)
            //{
            //   m_PrintForm = new PrintOrderForm();
            //   m_PrintForm.InitializePrintSettings(m_App);       
            //}
            //m_PrintForm.CallOrderPrint(patient, m_AdviceEditor.IsTempOrder);
        }

        private void AdviceInputor_KeyUp(object sender, KeyEventArgs e)
        {
            m_AdviceEditor.AcceptFunctionKeyPress(e);
        }

        private void AdviceInputor_Shown(object sender, EventArgs e)
        {
            if ((m_App.CurrentPatientInfo == null) || (m_App.CurrentPatientInfo.NoOfHisFirstPage == "-1"))
                m_App.CustomMessageBox.MessageShow("请先选择病人", CustomMessageBoxKind.InformationOk);
        }
    }
}