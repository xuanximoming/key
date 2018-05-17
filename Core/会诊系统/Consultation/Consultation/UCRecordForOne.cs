using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;

namespace YidanSoft.Core.Consultation
{
    public partial class UCRecordForOne : DevExpress.XtraEditors.XtraUserControl
    {
        private string m_NoOfFirstPage = string.Empty;
        private IYidanEmrHost m_Host;
        private string m_ConsultApplySn = string.Empty;

        public UCRecordForOne()
        {
            InitializeComponent();
        }

        public UCRecordForOne(string noOfFirstPage, IYidanEmrHost host, string consultApplySn)
            : this()
        {
            m_NoOfFirstPage = noOfFirstPage;
            m_Host = host;
            m_ConsultApplySn = consultApplySn;
            InitInner(false);
            RegisterEvent();
        }

        private void InitInner(bool isNew)
        {
            ucRecordResultForOne.Init(m_NoOfFirstPage, m_Host, isNew, false, m_ConsultApplySn);
            ucPatientInfoForOne.Init(m_NoOfFirstPage, m_Host);
            ucApplyInfoForOne.Init(m_NoOfFirstPage, m_Host, isNew, true, m_ConsultApplySn);

            InitBtnState();
        }

        private void RegisterEvent()
        {
            simpleButtonSave.Click += new EventHandler(simpleButtonSave_Click);
            simpleButtonComplete.Click += new EventHandler(simpleButtonComplete_Click);
        }

        void simpleButtonComplete_Click(object sender, EventArgs e)
        {
            ucRecordResultForOne.Save(ConsultStatus.RecordeComplete);
        }

        void simpleButtonSave_Click(object sender, EventArgs e)
        {
            if (!simpleButtonSave.Enabled)
                return;
            ucRecordResultForOne.Save(ConsultStatus.RecordeSave);
        }

        public void ReadOnlyControl()
        {
            simpleButtonSave.Visible = false;
            simpleButtonComplete.Visible = false;
            ucPatientInfoForOne.Location = new Point(ucRecordResultForOne.Location.X, ucRecordResultForOne.Location.Y + ucRecordResultForOne.Height + 8);
            ucApplyInfoForOne.Location   = new Point(ucPatientInfoForOne.Location.X, ucPatientInfoForOne.Location.Y + ucPatientInfoForOne.Height + 8);
            this.Height = ucApplyInfoForOne.Location.Y + ucApplyInfoForOne.Height + 10;
        }

        /// <summary>
        /// 判断会诊保存与会诊结束按钮状态
        /// </summary>
        private void InitBtnState()
        {
            if (m_ConsultApplySn.Trim() == "")
                return;

            string applyDeptID = string.Empty;
            string consultationDeptID = string.Empty;

            //根据申请编号到数据库中获取会诊单的申请科室以及受邀科室信息
            string sql = string.Format(@"select apply.consultapplysn,
                                                    apply.noofinpat,
                                                    apply.applydept ApplyDeptID,
                                                    applydept.departmentcode ConsultDeptID,
                                                    recorddept.hospitalcode,
                                                    recorddept.departmentcode ConsultDeptID2,
                                                    apply.finishtime ConsultTime,
                                                    apply.stateid
                                            from consultapply apply
                                            left join ConsultApplyDepartment applydept on apply.consultapplysn =
                                                                                            applydept.consultapplysn
                                                                                        and applydept.valid = 1
                                            left join consultrecorddepartment recordDept on recordDept.Consultapplysn =
                                                                                            apply.consultapplysn
                                                                                        and recordDept.Valid = 1
                                            where apply.consultapplysn = '{0}'
                                                and apply.valid = 1
                                                and apply.consulttypeid = '6501';
                                    ",m_ConsultApplySn);

            DataTable dt = m_Host.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            if (dt.Rows.Count == 0)
                return;

            applyDeptID = dt.Rows[0]["ApplyDeptID"].ToString();
            //如果实际会诊科室未空则选择受邀科室   如果实际会诊科室有值 则会诊科室为实际科室
            if(dt.Rows[0]["ConsultDeptID2"].ToString()=="")
                consultationDeptID = dt.Rows[0]["ConsultDeptID"].ToString();
            else
                consultationDeptID = dt.Rows[0]["ConsultDeptID2"].ToString();

            InitBtnState(applyDeptID, consultationDeptID);
        }

        /// <summary>
        /// 判断会诊保存与会诊结束按钮状态
        /// </summary>
        /// <param name="applyDeptID">申请科室</param>
        /// <param name="consultationDeptID">受邀科室</param>
        private void InitBtnState(string applyDeptID, string consultationDeptID)
        {
            //获取系统设置中配置的单科会诊控制
            string config = m_Host.SqlHelper.ExecuteScalar("select value from appcfg where configkey = 'ConsultationConfig'", CommandType.Text).ToString();

            string[] configs = config.Split(',');
            //if (m_ConsultationEntity.StateID != Convert.ToString((int)ConsultStatus.WaitConsultation) && m_ConsultationEntity.StateID != Convert.ToString((int)ConsultStatus.RecordeSave))
            //    return;

            //申请方有保存权限
            if (configs[0].ToString() == "1")
            {
                if (m_Host.User.CurrentDeptId == applyDeptID)
                    this.simpleButtonSave.Enabled = true;
            }
            //受邀方有保存权限
            else if (configs[0].ToString() == "2")
            {
                if (m_Host.User.CurrentDeptId == consultationDeptID)
                    this.simpleButtonSave.Enabled = true;
            }
            //双方都有保存权限
            else if (configs[0].ToString() == "3")
            {
                if (m_Host.User.CurrentDeptId == applyDeptID || m_Host.User.CurrentDeptId == consultationDeptID)
                    this.simpleButtonSave.Enabled = true;
            }

            //申请方有完成权限
            if (configs[1].ToString() == "1")
            {
                if (m_Host.User.CurrentDeptId == applyDeptID)
                    this.simpleButtonComplete.Enabled = true;
            }
            //受邀方有完成权限
            else if (configs[1].ToString() == "2")
            {
                if (m_Host.User.CurrentDeptId == consultationDeptID)
                    this.simpleButtonComplete.Enabled = true;
            }
            //双方都有完成权限
            else if (configs[1].ToString() == "3")
            {
                if (m_Host.User.CurrentDeptId == applyDeptID || m_Host.User.CurrentDeptId == consultationDeptID)
                    this.simpleButtonComplete.Enabled = true;
            }

            //如果保存和修改按钮都不可用则控件不可用
            if (!simpleButtonComplete.Enabled && !simpleButtonSave.Enabled)
            {
                ReadOnlyControl();
            }
        }

    }
}
