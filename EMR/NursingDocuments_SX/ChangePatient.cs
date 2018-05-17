using DrectSoft.Common.Eop;
using DrectSoft.Core.NursingDocuments.PublicSet;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;

namespace DrectSoft.Core.NursingDocuments
{
    /// <summary>
    /// 新增的切换病人时的加载选择病人窗口
    /// add by ywk  2012年5月29日 10:00:48
    /// (在院的本本科室的)
    /// </summary>
    public partial class ChangePatient : DevExpress.XtraEditors.XtraForm
    {
        public ChangePatient()
        {
            InitializeComponent();
        }

        public ChangePatient(IEmrHost iEmrHost, IUser muser)
        {
            // TODO: Complete member initialization
            m_app = iEmrHost;
            m_loguser = muser;
            InitializeComponent();
        }

        IEmrHost m_app;
        //Inpatient myInpatient;
        public Inpatient CurrentInpatient
        { get; set; }

        IUser m_loguser = null;
        private IEmrHost iEmrHost;
        public IUser User
        {
            get { return m_loguser; }
        }

        public string NOOfINPAT { get; set; }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePatient_Load(object sender, EventArgs e)
        {
            BindCurrentPatient();
        }
        /// <summary>
        /// 绑定Grid控件
        /// </summary>
        private void BindCurrentPatient()
        {
            DataTable dt = MethodSet.GetCurrentPatient(m_loguser.CurrentDeptId, m_loguser.CurrentWardId);
            this.gridControl1.DataSource = dt;
        }
        /// <summary>
        /// 输入条件搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SetGridDataBySea();
        }
        /// <summary>
        /// 根据输入条件控制Grid绑定数据
        /// </summary>
        private void SetGridDataBySea()
        {
            //搜索条件
            string filter = string.Format(@" PATNAME like '%{0}%' or SexName like '%{0}%' 
            or bedid like '%{0}%'  or PatId like '%{0}%' ", txtSearch.Text.Trim());
            DataTable dt = gridControl1.DataSource as DataTable;
            if (dt != null)
            {
                dt.DefaultView.RowFilter = filter;
            }
        }
        /// <summary>
        /// /双击进行选择患者，（把值 用于返回到体征录入界面）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            CommitSearchPat();
        }
        /// <summary>
        /// 确认选择患者返回上级窗体
        /// </summary>
        private void CommitSearchPat()
        {
            DataRow row = gridViewPaient.GetDataRow(gridViewPaient.FocusedRowHandle);//取得选择的行
            if (row == null)
            {
                return;
            }
            NOOfINPAT = row["NOOFINPAT"].ToString();//病人首页序号，用于返回上级窗体
            if (!string.IsNullOrEmpty(NOOfINPAT))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            this.Close();
        }
        /// <summary>
        /// 确定窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            CommitSearchPat();
        }
        /// <summary>
        /// 取消操作 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }


    }
}