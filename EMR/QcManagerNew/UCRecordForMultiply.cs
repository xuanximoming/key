using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using YidanSoft.FrameWork.WinForm.Plugin;
using YidanSoft.Core.Consultation;
using YidanSoft.Core;

namespace YindanSoft.Emr.QcManagerNew
{
    public partial class UCRecordForMultiply : DevExpress.XtraEditors.XtraUserControl
    {
        private string m_NoOfFirstPage = string.Empty;
        private IYidanEmrHost m_Host;
        private string m_ConsultApplySn = string.Empty;

        public UCRecordForMultiply()
        {
            InitializeComponent();
        }

        public UCRecordForMultiply(string noOfFirstPage, IYidanEmrHost host, string consultApplySn)
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
            ucRecordResultForMultiply.Init(m_NoOfFirstPage, m_Host, isNew, false, m_ConsultApplySn);
            ucPatientInfoForMultipy.Init(m_NoOfFirstPage, m_Host);
            ucApplyInfoForMultipy.Init(m_NoOfFirstPage, m_Host, isNew, true, m_ConsultApplySn);

            DataSet ds = DataAccess.GetConsultationDataSet(m_ConsultApplySn, "20");//, Convert.ToString((int)ConsultType.More));
            DataTable dtConsultApply = ds.Tables[0];
            memoEditSuggestion.Text = dtConsultApply.Rows[0]["RejectReason"].ToString().Trim();

            InitBtnState();
        }

        private void RegisterEvent()
        {
            simpleButtonSave.Click += new EventHandler(simpleButtonSave_Click);
            simpleButtonComplete.Click += new EventHandler(simpleButtonComplete_Click);
        }
        /// <summary>
        /// /会诊完成操作 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void simpleButtonComplete_Click(object sender, EventArgs e)
        {
            ucRecordResultForMultiply.Save(ConsultStatus.RecordeComplete);
        }

        void simpleButtonSave_Click(object sender, EventArgs e)
        {
            ucRecordResultForMultiply.Save(ConsultStatus.RecordeSave);
        }

        public void ReadOnlyControl()
        {
            simpleButtonSave.Visible = false;
            simpleButtonComplete.Visible = false;

            ucRecordResultForMultiply.ReadOnlyControl();
            ucApplyInfoForMultipy.ReadOnlyControl();

            ucPatientInfoForMultipy.Location = new Point(ucRecordResultForMultiply.Location.X, 
                ucRecordResultForMultiply.Location.Y + ucRecordResultForMultiply.Height);
            ucApplyInfoForMultipy.Location = new Point(ucApplyInfoForMultipy.Location.X,
                ucRecordResultForMultiply.Location.Y + ucRecordResultForMultiply.Height);

            ucRecordResultForMultiply.ReadOnlyControl();
            this.Height = ucApplyInfoForMultipy.Location.Y + ucApplyInfoForMultipy.Height;
        }

        /// <summary>
        /// 申请信息不可修改
        /// </summary>
        public void ApplyInfoReadOnly()
        {
            ucApplyInfoForMultipy.ReadOnlyControl();
        }
 
        private void CheckISApprove()
        {

            if (m_ConsultApplySn == "")
                ReadOnlyControl();

            string sql = string.Format("select a.stateid from consultapply a where a.consultapplysn = '{0}'",m_ConsultApplySn);

            string stateid = m_Host.SqlHelper.ExecuteDataTable(sql).Rows[0][0].ToString();

            if (stateid == Convert.ToString((int)ConsultStatus.WaitConsultation) || stateid == Convert.ToString((int)ConsultStatus.RecordeSave))
            {
                
            }
            else
            {
                ReadOnlyControl();
            }
                
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
                                                    apply.applydept ApplyDeptID 
                                            from consultapply apply
                                            where apply.consultapplysn = '{0}'
                                                and apply.valid = 1 ;
                                    ", m_ConsultApplySn);

            DataTable dt = m_Host.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
            if (dt.Rows.Count == 0)
                return;

            applyDeptID = dt.Rows[0]["ApplyDeptID"].ToString();

            if (m_Host.User.CurrentDeptId == applyDeptID)
            {
                this.simpleButtonSave.Enabled = true;
                this.simpleButtonComplete.Enabled = true;
            }
            else
            {
                ReadOnlyControl();
            }
        }

        private void UCRecordForMultiply_Load(object sender, EventArgs e)
        {
            CheckISApprove();
        }

    }
}
