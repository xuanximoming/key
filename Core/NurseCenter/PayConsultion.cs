using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Emr.NurseCenter
{
    /// <summary>
    /// 新增的会诊记录已经保存或完成的进行缴费的页面
    /// add by ywk 2012年7月24日 17:14:12 
    /// </summary>
    public partial class PayConsultion :DevBaseForm
    {
        #region 属性字段
        public PayConsultion()
        {
            InitializeComponent();
        }
        /// <summary>
        /// /构造函数
        /// </summary>
        /// <param name="myapp"></param>
        /// <param name="dt"></param>
        /// <param name="nursefo"></param>
        public PayConsultion(IEmrHost myapp,DataTable dt,NurseForm nursefo)
        {
            m_app = myapp;
            //NOOfINPAT = m_noofinpat;
            Dt_ConsultData = dt;
            m_NurserForm = nursefo;
            InitializeComponent();
        }
        IEmrHost m_app;
        public DataTable Dt_ConsultData;
        //public string NOOfINPAT { get; set; }
        public NurseForm m_NurserForm;//护士工作站的窗体
        #endregion

        #region 自定义事件 执行会诊缴费 add by wwj 2013-03-05
        public delegate bool DelegatePay(DataRow foucesRow, bool isNeedTip/*是否需要提示*/, bool isNeedQuestion/*当满足条件时询问是否需要会诊缴费*/);
        public DelegatePay Pay;
        public delegate DataTable DelegateHaveConsultationNotFee();//已会诊，但是没有收费
        public DelegateHaveConsultationNotFee HaveConsultationNotFee;
        #endregion

        #region 方法
        /// <summary>
        /// 根据
        /// </summary>
        private void SetGridData()
        {
            this.gridControl1.DataSource = Dt_ConsultData;
        }
        #endregion

        #region 事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PayConsultion_Load(object sender, EventArgs e)
        {
            SetGridData();
            this.ActiveControl = txt_search;
        }
        /// <summary>
        /// /会诊缴费事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PAY_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewCons.FocusedRowHandle < 0)
                    return;

                DataRow foucesRow = gridViewCons.GetDataRow(gridViewCons.FocusedRowHandle);
                if (foucesRow == null)
                    return;
                string consultstatus = foucesRow["stateid"].ToString();//会诊单状态
                string consultapply = foucesRow["ConsultApplySN"].ToString();//会诊单号
                string applyDept = foucesRow["applydept"].ToString();

                //m_NurserForm.GoPay(consultstatus, consultapply, applyDept, foucesRow);
                //DataTable dt = m_NurserForm.GetNoPayData();
                //this.gridControl1.DataSource = dt;//重新刷新数据

                Pay(foucesRow, true, false);
                this.gridControl1.DataSource = HaveConsultationNotFee();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 筛选事件
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-10-23</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)gridControl1.DataSource;
            if (dt != null)
            {
                string searchStr = txt_search.Text.Trim();

                dt.DefaultView.RowFilter = @" INPATIENTNAME like '%" + searchStr + "%' or PY like '%" + searchStr + "%' or WB like '%" + searchStr + "%' or CONSULTTIME like '%" + searchStr + "%' or CONSULTSTATUS like '%" + searchStr + "%' or MYPAY like '%" + searchStr + "%' ";
            }
        }
        #endregion

    }
}