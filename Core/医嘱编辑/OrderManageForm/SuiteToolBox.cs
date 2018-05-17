using DevExpress.XtraEditors;
using System;
using DrectSoft.Core.DoctorAdvice;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;

namespace DrectSoft.Core.OrderManage
{
    /// <summary>
    /// 医嘱录入工具箱
    /// </summary>
    public partial class SuiteToolBox : XtraForm
    {
        #region Vars&Properties

        private IEmrHost m_App;

        /// <summary>
        /// 成套明细选择界面最终选定的记录
        /// </summary>
        public object[,] SelectedContents
        {
            get { return _selectedContents; }
        }
        private object[,] _selectedContents;

        #endregion

        #region Ctor
        /// <summary>
        /// 医嘱录入工具箱
        /// </summary>
        public SuiteToolBox(IEmrHost application)
        {
            m_App = application;
            InitializeComponent();
            m_SuiteChoice.App = m_App;
            m_SuiteChoice.InitializeOrderSuiteChoiceForm(m_App.SqlHelper, m_App.CustomMessageBox, new SuiteOrderHandle(m_App, true), false);
            m_SuiteChoice.SelectedOrderSuite += new EventHandler(DoAfterSelectedOrderSuite);
            m_SuiteChoice.AddPersonal();
            m_SuiteChoice.AddDept();
            m_SuiteChoice.AddHospital();
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// 读取病人卡铂计算所要的信息
        /// </summary>
        public void LoadPatientAgeSex()
        {
            //m_UcCarbo.LoadAgeSex(m_App.CurrentPatientInfo.PersonalInformation.CurrentAge,
            //   (m_App.CurrentPatientInfo.PersonalInformation.Sex.Code == "1"));
        }

        /// <summary>
        /// 刷新成套医嘱数据。根据当前医生、科室和编辑的医嘱类型进行
        /// </summary>
        /// <param name="isTempOrder">长期、临时标志</param>
        /// <param name="enabled">是否可用</param>
        /// <param name="outDruggeryOnly"></param>
        /// <param name="frequencyBook"></param>
        [CLSCompliantAttribute(false)]
        public void RefreshOrderSuiteData(bool isTempOrder, bool enabled, bool outDruggeryOnly, OrderFrequencyBook frequencyBook)
        {
            m_SuiteChoice.RefreshOrderSuiteData(isTempOrder, enabled, outDruggeryOnly, frequencyBook);
        }

        #endregion

        #region CustomEvents

        private EventHandler onAfterSelectedOrderSuite;
        /// <summary>
        /// 选择成套医嘱之后
        /// </summary>
        public event EventHandler AfterSelectedOrderSuite
        {
            add { onAfterSelectedOrderSuite = (EventHandler)Delegate.Combine(onAfterSelectedOrderSuite, value); }
            remove { onAfterSelectedOrderSuite = (EventHandler)Delegate.Remove(onAfterSelectedOrderSuite, value); }
        }

        private void DoAfterSelectedOrderSuite(object sender, EventArgs e)
        {
            if (m_SuiteChoice.SelectedContents != null)
                this._selectedContents = m_SuiteChoice.SelectedContents;
            if (onAfterSelectedOrderSuite != null)
                onAfterSelectedOrderSuite(this, new EventArgs());
        }

        #endregion
    }
}