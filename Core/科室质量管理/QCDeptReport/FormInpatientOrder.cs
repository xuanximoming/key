using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Report;
using DrectSoft.Wordbook;
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.QCDeptReport
{
    public partial class FormInpatientOrder : DevBaseForm, IStartPlugIn
    {
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


        private IDataAccess m_DataAccessEmrly;

        private DataTable m_pageSouce;

        public FormInpatientOrder()
        {
            InitializeComponent();       
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (lookUpEditorInpatient.CodeValue == "")
            {
                _app.CustomMessageBox.MessageShow("请选择病人！");
                return;
            }
 
            string begintime = dateBegin.DateTime.ToString("yyyy-MM-dd");
            string endtime = dateEnd.DateTime.ToString("yyyy-MM-dd");
            string NoOfHisFirstPage = lookUpEditorInpatient.CodeValue.ToString();
            GetInpatientSouce(begintime, endtime, NoOfHisFirstPage);
        }


        private void GetInpatientSouce(string BeginTime, string EndTime, string NoOfHisFirstPage)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                
                  new SqlParameter("@HisSyxh", SqlDbType.Int ),
                  new SqlParameter("@DateTimeBegin", SqlDbType.VarChar, 10),
                  new SqlParameter("@DateTimeEnd",SqlDbType.VarChar, 10),
                  };
 
            sqlParams[0].Value = NoOfHisFirstPage;
 
            sqlParams[1].Value = BeginTime;
            sqlParams[2].Value = EndTime;
            //访问EHRDB
            IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess("EHRDB");

            DataSet ds = sqlHelper.ExecuteDataSet("usp_CP_GetInpatientOrderByEmr", sqlParams, CommandType.StoredProcedure);
            m_pageSouce = ds.Tables[0];

            gridMedQCAnalysis.DataSource = m_pageSouce;
            _app.PublicMethod.ConvertGridDataSourceUpper(gridView1);
            
            if (m_pageSouce.Rows.Count == 0)
            {
                _app.CustomMessageBox.MessageShow("没有符合条件的数据");
            }

        }


        #region IStartPlugIn 成员

        public IPlugIn Run(DrectSoft.FrameWork.WinForm.Plugin.IEmrHost host)
        {

            PlugIn plg = new PlugIn(this.GetType().ToString(), this);
            _app = host;
            m_DataAccessEmrly = host.SqlHelper;
            return plg;
        }

        #endregion

        private void FormMedQCAnalysis_Load(object sender, EventArgs e)
        {
            this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
            this.dateEnd.DateTime = DateTime.Now;
            InitInpatientList();
        }


        /// <summary>
        /// 绑定当前科室病人列表类型
        /// </summary>
        private void InitInpatientList()
        {
            lookUpWindowInpatient.SqlHelper = _app.SqlHelper;

            DataTable inpatientlis = _app.PatientInfos.Tables[0];
            inpatientlis.Columns["NOOFINPAT"].Caption = "首页序号";
            inpatientlis.Columns["NAME"].Caption = "患者姓名";

            Dictionary<string, int> cols = new Dictionary<string, int>();

            cols.Add("NOOFINPAT", 50);
            cols.Add("NAME", 100);

            SqlWordbook deductpoint = new SqlWordbook("querybook", inpatientlis, "NOOFINPAT", "NAME", cols);
            lookUpEditorInpatient.SqlWordbook = deductpoint;

            lookUpEditorInpatient.SqlWordbook = deductpoint;


        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (m_pageSouce == null)
                return;
            XReport xreport = new XReport(m_pageSouce.Copy(), @"ReportMedQCAnalysis.repx");

            xreport.ShowPreview();
        }

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
            this.lookUpEditorInpatient.CodeValue = "";
            this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
            this.dateEnd.DateTime = DateTime.Now;
        }
    }
}