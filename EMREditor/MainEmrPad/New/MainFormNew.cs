using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Emr.Util;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DrectSoft.Core.MainEmrPad.New
{
    public partial class MainFormNew : DevBaseForm, IStartPlugIn
    {
        private IEmrHost m_app;
        RecordDal m_RecordDal;
        UCEmrInput m_UCEmrInput;
        UCEmrInputout m_UCEmrInputout;

        /// <summary>
        /// 初始化住院病历文书录入
        /// </summary>
        public MainFormNew()
        {
            try
            {
                InitializeComponent();
                AddUcEmrInput();
                FormClosing += new FormClosingEventHandler(UCEmrInput_FormClosing);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex.Message);
            }
        }

        /// <summary>
        /// 初始化文书录入
        /// </summary>
        private void AddUcEmrInput()
        {
            try
            {
                m_UCEmrInput = new UCEmrInput();
                m_UCEmrInput.Dock = DockStyle.Fill;

                this.Controls.Add(m_UCEmrInput);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 住院窗体关闭事件
        /// </summary>
        /// edit by Yanqiao.Cai 2013-01-17
        /// 1、add try ... catch
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void UCEmrInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                m_UCEmrInput.CloseAllTabPages();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region IStartPlugIn 成员

        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {
            try
            {
                PlugIn plg = new PlugIn(this.GetType().ToString(), this);
                plg.PatientChanging += new PatientChangingHandler(plg_PatientChanging);
                plg.PatientChanged += new PatientChangedHandler(plg_PatientChanged);
                m_app = host;
                m_RecordDal = new RecordDal(m_app.SqlHelper);
                m_UCEmrInput.SetVarData(m_app);
                return plg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void plg_PatientChanging(object sender, CancelEventArgs arg)
        {
            try
            {
                m_UCEmrInput.PatientChanging();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void plg_PatientChanged(object Sender, PatientArgs arg)
        {
            try
            {
                m_UCEmrInput.PatientChanged(m_app.CurrentPatientInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

    }
}