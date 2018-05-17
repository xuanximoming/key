using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.DSSqlHelper;
using DrectSoft.Service;

namespace DrectSoft.Core.OwnBedInfo
{
    /// <summary>
    /// 会诊流程说明界面
    /// </summary>
    public partial class ConsultationStateForm : DevBaseForm
    {
        public ConsultationStateForm()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// Modify by xlb 2013-04-01
        /// Appcfg表添加该控制项"ConsultationInstruction"用于更改说明文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsultationStateForm_Load(object sender, EventArgs e)
        {
            try
            {
                string state=DS_SqlService.GetConfigValueByKey("ConsultationInstruction");
                #region 注销 by xlb 流程说明加入配置表 方便更改
                //string state = "1.主治医师以上级别医师都有会诊审核的权限。默认各科室主任有会诊审核人配置权限，可针对本科室医师配置各自的会诊审核医师。\n\r";
                //state += "2.各管床医生可针对科室内在院患者发出会诊申请,会诊邀请时间可选择一个定点时间，会诊邀请人可指定到受邀科室的主治医师或主治以上级别医师或指定到个人。填写会诊申请后，会诊状态会在医生工作站主界面右侧会诊信息栏显示。  \n\r";
                //state += "3.会诊申请发出后经上级医师审核，上级医师有否决、通过权力。否决后会诊取消，若要会诊需再次发会诊申请。会诊通过审核后，会诊状态显示在邀请医生界面、邀请科室护士站界面、受邀医生界面、受邀科室护士站界面，并且邀请医师、受邀医生首次登陆时会弹出会诊提示框，护士也可直接控制提示框向受邀医生发出提醒。\n\r";
                //state += "4.受邀科室护士站打印出会诊记录单交会诊医师，打印时间作为护士已通知到医生会诊的时间，若是因为护士打印不及时而导致延误，电子病历系统中可追溯。医师可手录会诊记录单中会诊意见或在电子病历系统中直接录入会诊意见。（病历系统中的会诊记录单会诊意见可为空，由医师手写的替代）\n\r";
                //state += "5.受邀医师到达邀请科室后，先到护士站确认参加会诊，确认时间作为会诊到达时间，与邀请会诊时间做对比，作为是否按时会诊依据。护士确认后开始作收费处理。会诊完成后由邀请医生确认会诊完成，若会诊前未到护士站确认参加会诊，会诊完成时间即作为会诊到达时间，与邀请会诊时间、护士通知会诊时间做对比，作为是否按时会诊依据。 \n\r";
                //state += "6.会诊完成后状态传送到护士站，若会诊前未到护士站确认参加会诊，未作收费处理，提醒护士完成收费。 \n\r";
                //state += "7.超过会诊申请单上申请的会诊时间，会诊申请不自动关闭。受邀医师在超过会诊时间后可继续参加会诊，电子病历系统可追溯。邀请医生也可手动取消会诊申请。";
                #endregion
                richTextBoxState.Text = state;
            }
            catch (Exception ex)
            {
               MyMessageBox.Show(1, ex);
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButtonExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}