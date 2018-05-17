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
using DrectSoft.Wordbook;
using DrectSoft.Common.Report;

namespace DrectSoft.Core.QCDeptReport
{
    public partial class UCFiling : DevExpress.XtraEditors.XtraUserControl
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

        public UCFiling(IEmrHost application)
        {
            InitializeComponent();

            InitApp(application);
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

        private void UserControlFiling_Load(object sender, EventArgs e)
        {
            this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
            this.dateEnd.DateTime = DateTime.Now;
            txtDeptName.Text = App.User.CurrentDeptName;

            InitSqlWorkBook();
        }

        #region 方法
        


        private void GetInpatientSouce(string BeginTime, string EndTime, string NoOfInpat)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@DeptCode", SqlDbType.VarChar,10),
                  new SqlParameter("@NoOfInpat", SqlDbType.VarChar, 10),
                  new SqlParameter("@DateTimeBegin", SqlDbType.VarChar, 10),
                  new SqlParameter("@DateTimeEnd",SqlDbType.VarChar, 10),
                  new SqlParameter("@QCStatType",SqlDbType.Int)
                  };

            sqlParams[0].Value = App.User.CurrentDeptId == null ? "" : App.User.CurrentDeptId;
            //sqlParams[0].Value = "";
            sqlParams[1].Value = NoOfInpat;
            sqlParams[2].Value = BeginTime;
            sqlParams[3].Value = EndTime;
            sqlParams[4].Value = 2;
            DataSet ds = m_DataAccessEmrly.ExecuteDataSet("usp_GetInpatientFiling", sqlParams, CommandType.StoredProcedure);
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

                return Convert.ToDecimal(dataRow["NOOFINPAT"]);
            }

        }

        /// <summary>
        /// 绑定失分类型
        /// </summary>
        private void InitSqlWorkBook()
        {
            UpWindowtInpatient.SqlHelper = _app.SqlHelper;

            DataTable dtInpatient = GetInpatientList();

            if (dtInpatient.Rows.Count == 0)
                return;
            dtInpatient.Columns["NOOFINPAT"].Caption = "住院号";
            dtInpatient.Columns["NAME"].Caption = "患者姓名";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NOOFINPAT", 50);
            cols.Add("NAME", 100);

            SqlWordbook Inpatientworkbook = new SqlWordbook("querybook", dtInpatient, "NOOFINPAT", "NAME", cols, "NOOFINPAT//NAME");

            UpEditorInpatient.SqlWordbook = Inpatientworkbook;
        }

        /// <summary>
        /// 获取病患下拉框数据源
        /// </summary>
        public DataTable GetInpatientList()
        {
            String sql = string.Format("select a.NoOfInpat,a.PatID,a.Name from dbo.InPatient a where a.OutHosDept = '{0}'", _app.User.CurrentDeptId);

            DataSet ds = m_DataAccessEmrly.ExecuteDataSet(sql);

            return ds.Tables[0];
        }

        #endregion

        #region 事件
        
        private void btnQuery_Click(object sender, EventArgs e)
        {
            string begintime = dateBegin.DateTime.ToString("yyyy-MM-dd");
            string endtime = dateEnd.DateTime.ToString("yyyy-MM-dd");
            string NoOfInpat = UpEditorInpatient.CodeValue == null ? "" : UpEditorInpatient.CodeValue;
            GetInpatientSouce(begintime, endtime, NoOfInpat);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (m_pageSouce == null)
                return;
            XReport xreport = new XReport(m_pageSouce.Copy() , @"ReportFiling.repx");
            
            xreport.ShowPreview();
        }

        /// <summary>
        /// 双击跳转到病历编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            decimal syxh = GetCurrentPat();
            if (syxh < 0) return;

            App.ChoosePatient(syxh);
            App.LoadPlugIn("DrectSoft.Core.RecordsInput.FormMain,DrectSoft.Core.RecordsInput");
        }

        private void KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((int)e.KeyChar == (char)13)
                {
                    SendKeys.Send("{Tab}");
                    SendKeys.Flush();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
            this.dateEnd.DateTime = DateTime.Now;
            txtDeptName.Text = App.User.CurrentDeptName;
            this.UpEditorInpatient.CodeValue = "";
        } 
        #endregion
    }
}
