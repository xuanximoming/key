using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common;
using DevExpress.Utils;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Emr.Util;
using DrectSoft.Core;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Service;

namespace DrectSoft.Emr.QcManagerNew
{
    public partial class frmChild : DevBaseForm
    {
        public frmChild(QcManagerNew.frmContainer parent, IEmrHost app, string noofinpat)
        {
            InitializeComponent();
            this.MdiParent = parent;
            m_App = app;
            m_noofinpat = noofinpat;
            this.ControlBox = false;
        }
        IEmrHost m_App;
        private string m_noofinpat;
        private WaitDialogForm m_WaitDialog = new WaitDialogForm();
        /// <summary>
        /// 病历内容窗体 --- 逐步弃用 edit by cyq 2013-04-27
        /// </summary>
        UCEmrInput m_UCEmrInput;
        /// <summary>
        /// 病历内容窗体 --- 新版文书录入 add by cyq 2013-04-27
        /// </summary>
        DrectSoft.Core.MainEmrPad.New.UCEmrInput m_UCEmrInputNew;

        private void frmChild_Load(object sender, EventArgs e)
        {
            try
            {
                string config = DS_SqlService.GetConfigValueByKey("IsNewUcInput");
                if (null != config && config.Trim() == "1")
                {
                    AddEmrInputNew();
                }
                else
                {
                    AddEmrInput();
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
            finally
            {
                DS_Common.HideWaitDialog(m_WaitDialog);
            }
        }

        private void AddEmrInput()
        {
            try
            {
                m_UCEmrInput = new UCEmrInput();
                m_UCEmrInput.CurrentInpatient = null;
                m_UCEmrInput.HideBar();

                RecordDal m_RecordDal = new RecordDal(m_App.SqlHelper);
                m_UCEmrInput.SetInnerVar(m_App, m_RecordDal);
                this.panelControl1.Controls.Add(m_UCEmrInput);
                m_UCEmrInput.Dock = DockStyle.Fill;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region 新版文书录入
        private void AddEmrInputNew()
        {
            try
            {
                DS_Common.SetWaitDialogCaption(m_WaitDialog, "正在加载病人信息...");
                m_App.ChoosePatient(Convert.ToDecimal(m_noofinpat));//切换病人
                DS_Common.HideWaitDialog(m_WaitDialog);

                m_UCEmrInputNew = new DrectSoft.Core.MainEmrPad.New.UCEmrInput(m_App.CurrentPatientInfo, m_App, FloderState.None);
                m_UCEmrInputNew.SetVarData(m_App);
                this.panelControl1.Controls.Add(m_UCEmrInputNew);
                m_UCEmrInputNew.OnLoad();
                m_UCEmrInputNew.HideBar();
                m_UCEmrInputNew.Dock = DockStyle.Fill;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
