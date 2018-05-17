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
using DevExpress.XtraTreeList.Nodes;
using DrectSoft.Emr.Util;
using DevExpress.XtraTab;
using DrectSoft.Common.Eop;
using DevExpress.Utils;
using DrectSoft.Core.EMR_NursingDocument.EMRInput.Table;
using DrectSoft.Core.EMR_NursingDocument.UserContorls;

namespace DrectSoft.Core.EMR_NursingDocument
{
    /// <summary>
    /// add by ywk 2012年8月2日 13:28:17 
    /// </summary>
    public partial class MainNurseDocument : DevExpress.XtraEditors.XtraForm, IStartPlugIn
    {

        #region 属性和字段
        private IEmrHost m_app;
        Inpatient m_CurrentInpatient;
        /// <summary>
        /// 当前病人
        /// </summary>
        public Inpatient CurrentInpatient
        {
            get
            {
                return m_CurrentInpatient;
            }
            set
            {
                m_CurrentInpatient = value;
            }
        }

        UCMainNurseForm m_UcMainNurse;

        RecordDal m_RecordDal;
        #endregion

        #region 方法相关
        public MainNurseDocument()
        {
            InitializeComponent();
            AddEmrNurse();
            FormClosing += new FormClosingEventHandler(UCEmrInput_FormClosing);
        }
        /// <summary>
        /// 将包含控件的用户控件加到此窗体
        /// </summary>
        private void AddEmrNurse()
        {
            m_UcMainNurse = new UCMainNurseForm();
            m_UcMainNurse.Dock = DockStyle.Fill;
            this.Controls.Add(m_UcMainNurse);
        }
        /// <summary>
        /// 实现插件接口部分
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public IPlugIn Run(FrameWork.WinForm.Plugin.IEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            plg.PatientChanging += new PatientChangingHandler(plg_PatientChanging);
            plg.PatientChanged += new PatientChangedHandler(plg_PatientChanged);
            m_app = host;
            m_RecordDal = new RecordDal(m_app.SqlHelper);
            m_UcMainNurse.SetInnerVar(m_app, m_RecordDal);
            m_UcMainNurse.CurrentPat = m_app.CurrentPatientInfo;
            return plg;
        }

        void plg_PatientChanging(object sender, CancelEventArgs arg)
        {
            //UCEmrInput uCEmr = new UCEmrInput();
            m_UcMainNurse.PatientChanging();
        }

        void plg_PatientChanged(object Sender, PatientArgs arg)
        {
            //UCEmrInput uCEmr = new UCEmrInput();
            m_UcMainNurse.PatientChanged(m_app.CurrentPatientInfo);
        }
        #endregion

        #region 事件相关
        /// <summary>
        /// 关闭相关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UCEmrInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_UcMainNurse.CloseAllTabPages();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainNurseDocument_Load(object sender, EventArgs e)
        { }
        #endregion

    }
}