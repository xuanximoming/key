using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.ReportManager
{
    public partial class UnPassReason : DevBaseForm
    {

        IEmrHost m_Host;
        private string _PassReason;
        public string PassReason 
        {
            get
            {
                return _PassReason;
            }
            set
            {
                _PassReason = value;
            }
        }
        public UnPassReason( IEmrHost _app)
        {
            InitializeComponent();
            m_Host = _app;
            this.memoEditReason.Focus();
            this.ActiveControl = this.memoEditReason;
        }

        public string GetUnPassReason()
        {
            return memoEditReason.Text.Trim();
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.memoEditReason.Text.Trim()))
                {
                    this.memoEditReason.Focus();
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("否决原因不能为空");
                    return;
                }
                _PassReason = this.memoEditReason.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        #region 文本框获取焦点颜色变化控制 by cyq 2012-10-30
        private void Dev_Enter(object sender, EventArgs e)
        {
            DS_Common.setBackColor(sender, true);
        }
        private void Dev_Leave(object sender, EventArgs e)
        {
            DS_Common.setBackColor(sender, false);
        }
        #endregion
    }
}