using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MedicalRecordManage.Object;
using YiDanCommon.Ctrs.DLG;

namespace MedicalRecordManage.UCControl
{
    public partial class MedicalRecordCfg : DevExpress.XtraEditors.XtraUserControl
    {
        public MedicalRecordCfg()
        {
            InitializeComponent();
        }

        private void MedicalRecordCfg_Load(object sender, EventArgs e)
        {
            try
            {
                this.txtDelay.Text = ComponentCommand.GetDealyMaxTime().ToString();
                this.txtDelayTimes.Text = ComponentCommand.GetDealyTimes().ToString();
                this.txtReadAmount.Text = ComponentCommand.GetApplyLimit().ToString();
                this.txtReadTime.Text = ComponentCommand.GetReadTime().ToString();
                this.txtRemind.Text = ComponentCommand.GetRemindTime().ToString();
            }
            catch (Exception)
            {
                throw;                
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //
                if (CheckInput())
                {
                    /*
                    ComponentCommand.SetApplyLimit(this.txtReadAmount.Text.Trim());
                    ComponentCommand.SetDealyMaxTime(this.txtDelay.Text.Trim());
                    ComponentCommand.SetDealyTimes(this.txtDelayTimes.Text.Trim());
                    ComponentCommand.SetReadTime(this.txtReadTime.Text.Trim());
                    ComponentCommand.SetRemindTime(this.txtRemind.Text.Trim());
                     * */
                }
                else
                {
                    YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show("参数为有效整数!请检查输入是否正确","信息提示");
                }
            }
            catch (Exception ex)
            {

                YiDanCommon.Ctrs.DLG.YiDanMessageBox.Show(1, ex);
            }
        }
        private bool CheckInput()
        {
            try
            {
                int i = 0; int j = 1;
                i = int.Parse(this.txtReadAmount.Text.Trim());
                j = 1 * i;
                i = int.Parse(this.txtDelay.Text.Trim());
                j = 1 * i;
                i = int.Parse(this.txtDelayTimes.Text.Trim());
                j = 1 * i;
                i = int.Parse(this.txtReadTime.Text.Trim());
                j = 1 * i;
                i = int.Parse(this.txtRemind.Text.Trim());
                j = 1 * i;
                if (j > 0) return true; else return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
