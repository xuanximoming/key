using DrectSoft.Common;
using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Ctrs.FORM;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DrectSoft.Core.Consultation
{
    /// <summary>
    /// 院内会诊系统主界面
    /// </summary>
    public partial class FormMainConsultation : DevBaseForm
    {
        private IEmrHost m_App;
        private UCWaitApprove ucWaitApprove;
        private UCList ucList;
        private UCRecord ucRecord;
        private UCAudioDept ucAudioDept;

        #region 相关方法

        public FormMainConsultation()
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
        /// 构造
        /// MOdify by xlb
        /// Add try catch
        /// </summary>
        /// <param name="app"></param>
        public FormMainConsultation(IEmrHost app)
            : this()
        {
            try
            {
                m_App = app;

                #region 会诊审核界面
                ucWaitApprove = new UCWaitApprove(m_App);
                ucWaitApprove.Location = new Point(-3, -2);//左上角坐标
                ucWaitApprove.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                xtraTabPageApproveList.Controls.Add(ucWaitApprove);
                ucWaitApprove.Dock = DockStyle.Fill;
                #endregion

                #region 会诊清单界面
                ucList = new UCList(m_App);
                ucList.Location = new Point(0, -2);
                ucList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                xtraTabPageConsultationList.Controls.Add(ucList);
                ucList.Dock = DockStyle.Fill;
                #endregion

                #region 会诊记录界面
                ucRecord = new UCRecord(m_App);
                ucRecord.Location = new Point(0, -2);
                ucRecord.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                xtraTabPage3.Controls.Add(ucRecord);
                ucRecord.Dock = DockStyle.Fill;
                #endregion

                //如果登录人是主任，则显示待审核清单
                Employee emp = new Employee(m_App.User.Id);
                emp.ReInitializeProperties();
                //处理级别为空异常信息 edit by ywk 
                if (string.IsNullOrEmpty(emp.Grade))
                {
                    emp.Grade = "0";
                }

                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), emp.Grade);
                if (grade == DoctorGrade.Chief/*主任医师*/ || grade == DoctorGrade.AssociateChief/*f副主任医师*/)
                {
                    xtraTabPageApproveList.PageVisible = true;
                    XtraConfigAudio.PageVisible = true;//配置会诊审核人也显示出来  added by ywk 2012年3月14日18:23:05
                    #region 配置审核人界面
                    ucAudioDept = new UCAudioDept(m_App);
                    ucAudioDept.Location = new Point(0, -2);
                    ucAudioDept.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    ucAudioDept.Dock = DockStyle.Fill;
                    XtraConfigAudio.Controls.Add(ucAudioDept);
                    #endregion
                }
                else
                {
                    XtraConfigAudio.PageVisible = false;//主任级别和副主任级别可配置审核人审核列表则开发所有人
                    xtraTabPageApproveList.PageVisible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 控制待审核清单的显示情况
        /// </summary>
        /// <returns></returns>
        private bool SetXTabApproveList()
        {
            try
            {
                bool cansee = false;

                Employee emp = new Employee(m_App.User.Id);
                emp.ReInitializeProperties();
                DoctorGrade grade = (DoctorGrade)Enum.Parse(typeof(DoctorGrade), emp.Grade);

                AuditLogic audiLogic = new AuditLogic(m_App, m_App.User.Id);
                string fuzenid = audiLogic.GetUser(m_App.User.Id);
                //if (!string.IsNullOrEmpty(fuzenid))//返回审核人
                //{
                if (audiLogic.CanAudioConsult(m_App.User.Id, emp))//有审核权限
                {
                    //if (m_App.User.Id == fuzenid)
                    //{
                    cansee = true;
                    //}
                }
                else
                {
                    cansee = false;
                }
                return cansee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 相关事件

        /// <summary>
        ///注窗体加载事件
        /// Modify by xlb
        /// Add try catch
        /// 2013-03-20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMainConsultation_Load(object sender, EventArgs e)
        {
            try
            {
                #region 注销by xlb 审核界面开放所有人 鉴于审核医师存在三种优先级
                //xtraTabPageApproveList.PageVisible = false;
                //if (SetXTabApproveList())
                //{
                //    xtraTabPageApproveList.PageVisible = true;
                //}
                //else
                //{
                //    xtraTabPageApproveList.PageVisible = false;
                //}
                #endregion
                this.ActiveControl = ucList;
                //设置关于会诊审核的Tab页是否显示 edit by tj 2012-10-26
                //未开启审核则隐藏审核列表和配置审核医师界面
                if (!CommonObjects.IsNeedVerifyInConsultation)
                {
                    xtraTabPageApproveList.PageVisible = false;
                    XtraConfigAudio.PageVisible = false;
                }
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        private void xtraTabControlRecordList_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// tab页切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabControlRecordList_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                this.xtraTabControlRecordList.SelectedTabPage.Controls[0].Focus();
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);
            }
        }

        #endregion
    }
}