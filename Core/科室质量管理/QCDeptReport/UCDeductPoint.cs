using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm.Plugin;
using System.Data.SqlClient;
using DrectSoft.Core;
using DevExpress.XtraScheduler.Data;
using DrectSoft.Common.Report;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;

namespace DrectSoft.Core.QCDeptReport
{
    public partial class UCDeductPoint : DevExpress.XtraEditors.XtraUserControl
    {
        #region 变量
        
        
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        private IEmrHost _app;
        /// <summary>
        /// 创建插件的应用程序接口
        /// </summary>
        public IEmrHost App
        {
            get { return _app; }
            set { _app = value; }
        }

        /// <summary>
        /// 连接数据库的SqlHelper
        /// </summary>
        private IDataAccess m_DataAccessEmrly;
 

        private DataTable m_pageSouce;

        #endregion

        public UCDeductPoint(IEmrHost application)
        {
            InitializeComponent();

            InitApp(application);
        }

        private void UserControlFail_Load(object sender, EventArgs e)
        {
            this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
            this.dateEnd.DateTime = DateTime.Now;
            txtDeptName.Text = App.User.CurrentDeptName;

        }

        protected virtual void InitApp(IEmrHost application)
        {
            try
            {
                App = application;
                m_DataAccessEmrly = App.SqlHelper;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetInpatientSouce(string BeginTime, string EndTime)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@DeptCode", SqlDbType.VarChar,10),
                  new SqlParameter("@DateTimeBegin", SqlDbType.VarChar, 10),
                  new SqlParameter("@DateTimeEnd",SqlDbType.VarChar, 10),
                  new SqlParameter("@QCStatType",SqlDbType.Int)
                  };

            sqlParams[0].Value = App.User.CurrentDeptId == null ? "" : App.User.CurrentDeptId;
            //sqlParams[0].Value = "";
            sqlParams[1].Value = BeginTime;
            sqlParams[2].Value = EndTime;
            sqlParams[3].Value = 1;

            DataSet ds = m_DataAccessEmrly.ExecuteDataSet("usp_GetDeptDeductPoint", sqlParams, CommandType.StoredProcedure);
            m_pageSouce = ds.Tables[0];

            gridInpatientFail.DataSource = m_pageSouce;

            _app.PublicMethod.ConvertGridDataSourceUpper(gridViewInpatientFail);
            if (m_pageSouce.Rows.Count == 0)
            {
                _app.CustomMessageBox.MessageShow("无数据！");
            }

        }

        private decimal GetCurrentPat()
        {

            if (gridViewInpatientFail.FocusedRowHandle < 0)
                return -1;
            else
            {

                DataRow dataRow = gridViewInpatientFail.GetDataRow(gridViewInpatientFail.FocusedRowHandle);
                if (dataRow == null) return -1;

                return Convert.ToDecimal(dataRow["ID"]);
            }

        }

        #region 事件
        

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string begintime = dateBegin.DateTime.ToString("yyyy-MM-dd");
            string endtime = dateEnd.DateTime.ToString("yyyy-MM-dd");

            GetInpatientSouce(begintime, endtime);
        }

        /// <summary>
        /// 双击调用详细页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            FormDeductPointInfo formpointinfo = new FormDeductPointInfo(_app);
            formpointinfo.PointID = GetCurrentPat().ToString();
            formpointinfo.m_Begintime = dateBegin.DateTime;
            formpointinfo.m_Endtime = dateEnd.DateTime;

            formpointinfo.ShowDialog();
        }

        /// <summary>
        /// 调用打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (m_pageSouce == null)
                return;
            XReport xreport = new XReport(m_pageSouce.Copy(), @"ReportDeductpiont.repx");

            xreport.ShowPreview();
        }

        /// <summary>
        /// 调用饼状图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPie_Click(object sender, EventArgs e)
        {
            if (m_pageSouce == null)
                return;
            RepDeductPointPie pointPie = new RepDeductPointPie();
            pointPie.WindowState = FormWindowState.Maximized;
            pointPie.m_DataSource = m_pageSouce;
            pointPie.ShowDialog();
        }

        #endregion


        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == 13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {
                _app.CustomMessageBox.MessageShow(ex.Message);
            }
        }

        private void gridViewInpatientFail_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
            this.dateEnd.DateTime = DateTime.Now;
            txtDeptName.Text = App.User.CurrentDeptName;
        }

 
    }
}
