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
using DrectSoft.Common.Ctrs.FORM;

namespace DrectSoft.Core.QCDeptReport
{
    public partial class FormMedQCAnalysis : DevBaseForm, IStartPlugIn
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

        public FormMedQCAnalysis()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string begintime = dateBegin.DateTime.ToString("yyyy-MM-dd");
            string endtime = dateEnd.DateTime.ToString("yyyy-MM-dd");
            if (this.dateBegin.DateTime > this.dateEnd.DateTime)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show("起始时间不能大于结束时间");
                return;
            }
            GetInpatientSouce(begintime, endtime);
        }


        private void GetInpatientSouce(string BeginTime, string EndTime)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@DateTimeBegin", SqlDbType.VarChar, 10),
                  new SqlParameter("@DateTimeEnd",SqlDbType.VarChar, 10),
                  };


            sqlParams[0].Value = BeginTime;
            sqlParams[1].Value = EndTime;


            DataSet ds = m_DataAccessEmrly.ExecuteDataSet("usp_MedQCAnalysis", sqlParams, CommandType.StoredProcedure);

            m_pageSouce = ds.Tables[0];

            gridMedQCAnalysis.DataSource = m_pageSouce;

            _app.PublicMethod.ConvertGridDataSourceUpper(gridView1);
            if (m_pageSouce.Rows.Count == 0)
            {
                _app.CustomMessageBox.MessageShow("无数据！");
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
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (m_pageSouce == null)
                return;
            XReport xreport = new XReport(m_pageSouce.Copy(), @"ReportMedQCAnalysis.repx");

            xreport.ShowPreview();
        }

        private void simpleButtonDiagram_Click(object sender, EventArgs e)
        {
            DataTable dt = DiagramForm.GetDiagramDataSource(gridMedQCAnalysis);
            if (dt != null && dt.Rows.Count > 0)
            {
                DiagramForm form = new DiagramForm(dt, "科室名称", "DeptName");
                form.ShowDialog();
            }
            else
            {
                _app.CustomMessageBox.MessageShow("无数据", CustomMessageBoxKind.InformationOk);
            }
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "导出Excel";
            saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = saveFileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                //gridViewCardList.ExportToXls(saveFileDialog.FileName);
                gridMedQCAnalysis.ExportToXls(saveFileDialog.FileName, true);

                _app.CustomMessageBox.MessageShow("导出成功！");
            }
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
            DrectSoft.Common.DS_Common.AutoIndex(e);
        }

        private void simpleButtonReset_Click(object sender, EventArgs e)
        {
            this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
            this.dateEnd.DateTime = DateTime.Now;
        }
    }
}