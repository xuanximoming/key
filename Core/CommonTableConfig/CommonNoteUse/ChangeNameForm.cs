using DrectSoft.Common.Ctrs.FORM;
using System;
using System.Windows.Forms;

namespace DrectSoft.Core.CommonTableConfig.CommonNoteUse
{
    /// <summary>
    /// 修改配置单名称窗体
    /// </summary>
    public partial class ChangeNameForm : DevBaseForm
    {
        public CommonNoteEntity m_CommonNoteEntity;
        public ChangeNameForm(CommonNoteEntity commonNoteEntity)
        {
            try
            {
                InitializeComponent();
                m_CommonNoteEntity = commonNoteEntity;
                txtInCommonName.Text = m_CommonNoteEntity.CommonNoteName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonOK1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtInCommonName.Text.Trim()))
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("单据名不能为空！");
                    return;
                }
                m_CommonNoteEntity.CommonNoteName = txtInCommonName.Text.Trim();
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DevButtonClose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}