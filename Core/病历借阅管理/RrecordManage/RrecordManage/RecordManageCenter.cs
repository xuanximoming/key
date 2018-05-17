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
using DrectSoft.Core;
using DrectSoft.Wordbook;
using System.Data.SqlClient;
using DrectSoft.Common;
using DrectSoft.Core.RecordManage.UCControl;
using DrectSoft.Core.RecordManage.PublicSet;
using System.Runtime.InteropServices;
using DevExpress.Utils;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.RecordManage
{
    public partial class RecordManageCenter : DevBaseForm, IStartPlugIn
    {
        //IEmrHost m_App;
        //IDataAccess sql_Helper;
        private WaitDialogForm m_WaitDialog;

        public RecordManageCenter()
        {
            InitializeComponent();

            try
            {
                m_WaitDialog = new WaitDialogForm("正在创建用户界面...", "请稍等");
                m_WaitDialog.Show();
                UCRecordNoOnFile ucRecordNoOnFile = new UCRecordNoOnFile();
                ucRecordNoOnFile.Dock = DockStyle.Fill;
                tabPageRecordNoOnFile.Controls.Add(ucRecordNoOnFile);

                UCRecordOnFile ucRecordOnFile = new UCRecordOnFile();
                ucRecordOnFile.Dock = DockStyle.Fill;
                tabPageRecordOnFile.Controls.Add(ucRecordOnFile);

                UCApplyRecord ucApplyRecord = new UCApplyRecord();
                ucApplyRecord.Dock = DockStyle.Fill;
                tabPageApplyRecord.Controls.Add(ucApplyRecord);

                UCSingInRecord ucSingInRecord = new UCSingInRecord();
                ucSingInRecord.Dock = DockStyle.Fill;
                tabPageSignInRecord.Controls.Add(ucSingInRecord);

                //UCOperRecordLog ucOperRecordLog = new UCOperRecordLog();
                //ucOperRecordLog.Dock = DockStyle.Fill;
                //tabPageOperRecordLog.Controls.Add(ucOperRecordLog);
                m_WaitDialog.Hide();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }

        #region IStartPlugIn 成员

        public IPlugIn Run(IEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);

            SqlUtil.App = host;

            return plg;
        }

        #endregion

        private void captionBarAnother1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void RecordManageCenter_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ActiveControl = tabPageRecordOnFile;
        }

        /// <summary>
        /// 切换tab页事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-26</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                this.xtraTabControl1.SelectedTabPage.Controls[0].Focus();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

    }




}

