using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Eop;
using DrectSoft.Core.NursingDocuments.PublicSet;
using DrectSoft.Common;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.NursingDocuments
{
    /// <summary>
    /// 新增的切换病人时的加载选择病人窗口
    /// add by ywk  2012年5月29日 13:59:17
    /// (在院的本本科室的)
    /// </summary>
    public partial class ChangePatient : DevBaseForm
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

        #region 属性
        IEmrHost m_app;

        public Inpatient CurrentInpatient
        {
            get;
            set;
        }

        IUser m_loguser = null;
        private IEmrHost iEmrHost;
        public IUser User
        {
            get { return m_loguser; }
        }

        public string NOOfINPAT { get; set; }
        #endregion

        #region 方法
        /// <summary>
        /// 绑定Grid控件
        /// </summary>
        private void BindCurrentPatient()
        {
            DataTable dt = MethodSet.GetCurrentPatient(m_loguser.CurrentDeptId, m_loguser.CurrentWardId);
            this.gridControl1.DataSource = dt;
        }
        /// <summary>
        /// 根据输入条件控制Grid绑定数据
        /// </summary>
        private void SetGridDataBySea()
        {
            try
            {
                //搜索条件
                string SearchKey = txtSearch.Text.Trim();
                //对于特殊字符的处理 zyx 2012-01-28
                SearchKey = SearchKey.Replace("[", "[[ ")
                                     .Replace("]", " ]]")
                                     .Replace("*", "[*]")
                                     .Replace("%", "[%]")
                                     .Replace("[[ ", "[[]")
                                     .Replace(" ]]", "[]]")
                                     .Replace("\'", "''");
                string filter = string.Format(@" PATNAME like '%{0}%' or SexName like '%{0}%' 
                                                 or bedid like '%{0}%' or PatId like '%{0}%' ", SearchKey);
                DataTable dt = gridControl1.DataSource as DataTable;
                if (dt != null)
                {
                    dt.DefaultView.RowFilter = filter;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
          
        }
        /// <summary>
        /// 确认选择患者并返回上级窗体
        /// </summary>
        private void CommitSearchPat()
        {
            try
            {
                DataRow row = gridViewPaient.GetDataRow(gridViewPaient.FocusedRowHandle);//取得选择的行
                if (row == null)
                {
                    DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("请选择一条病人记录");
                    return;
                }
                NOOfINPAT = row["NOOFINPAT"].ToString();//病人首页序号，用于返回上级窗体
                if (!string.IsNullOrEmpty(NOOfINPAT))
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 事件

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
        /// 输入条件搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SetGridDataBySea();
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
        /// 确定窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                CommitSearchPat();
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
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

        /// <summary>
        /// 序号
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2011-11-02</date>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewPaient_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                DS_Common.AutoIndex(e);
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }
        }

        #endregion

        
    }
}