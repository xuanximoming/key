using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Wordbook;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.Core.MainEmrPad;
using DrectSoft.Common.Eop;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace DrectSoft.Core.InPatientReport
{
    public partial class UCInpatientReport : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        #region LIS

        /// <summary>
        /// 病人报告清单数据集
        /// </summary>
        const string c_SqlList = @"SELECT 
                b203 HospitalNo,
                NULL PatientSerialNo,
                b210 PatName, 
                'LIS' ReportCatalog,
                NULL ReportNo, 
                b201 ApplyItemCode, 
                nvl(b202, b211 || '检测') ApplyItemName, 
                to_char(b208, 'yyyy-MM-dd hh24:mi:ss') SubmitDate, 
                a107 ReleaseDate, 
                'False' HadRead,
                b202  ReportType,
                b204 ,
                b205,
                b211  
                FROM view_tj_report
                WHERE b203 = '{0}' and b208 >= to_date('{1}', 'yyyy-MM-dd HH24:mi:ss') and b208 <= to_date('{2}', 'yyyy-MM-dd HH24:mi:ss')
                GROUP BY b203, b210, b201,b202, b204, b205,b208,b211, a107
                order by b208";

        const string c_SqlDetail = @"select 
                rownum Line, 
                a101 ItemCode, 
                a102 ItemName, 
                a103 Result, 
                a104 ReferValue,
                a105 Unit,
                a106 HighFlag, 
                null ResultColor
                from view_tj_report
                where 
                b203 = '{0}'
                and b204 = '{1}'
                and b205 = '{2}'
                and b208 = to_date('{3}','yyyy-MM-dd hh24:mi:ss') 
                and b211 = '{4}'
                and a102 is not null
                ";
        const string c_SqlInpatient = @" select patid from inpatient where inpatient.noofinpat = '{0}' ";

        #region 销售演示数据
        //        const string c_SqlList = @"SELECT 
        //                HospitalNo,
        //                noofinpat PatientSerialNo,
        //                noofinpat PatName, 
        //                ReportCatalog,
        //                NULL ReportNo, 
        //                NULL ApplyItemCode, 
        //                '检测' || ReportName ApplyItemName, 
        //                SubmitDate, 
        //                ReleaseDate, 
        //                '0' HadRead,
        //                ReportType,
        //                '' b204 ,
        //                '' b205,
        //                '' b211  
        //                FROM INPATIENTREPORT
        //                order by SubmitDate";

        //        const string c_SqlDetail = @"select 
        //                Line, 
        //                ItemCode, 
        //                ItemName, 
        //                Result, 
        //                ReferValue,
        //                Unit,
        //                HighFlag, 
        //                ResultColor
        //                from INPATIENTREPORTLISRESLUT
        //                where 
        //                reportid = ltrim('{0}','0')
        //                and ItemName is not null
        //                ";
        #endregion

        #endregion

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

        Inpatient CurrentInpatient;

        private IDataAccess m_DataAccessEmrly;

        public UCInpatientReport(IEmrHost app)
        {

            InitializeComponent();
            _app = app;

            if (_app.CurrentPatientInfo != null)
                CurrentInpatient = _app.CurrentPatientInfo;

            m_DataAccessEmrly = _app.SqlHelper;
        }

        public UCInpatientReport(string noofinpat)
            : this()
        {
            CurrentNoofinpat = noofinpat;
        }

        public UCInpatientReport()
        {
            InitializeComponent();
        }

        private void UCInpatientReport_Load(object sender, EventArgs e)
        {

        }



        private void BindInpatientReport(string BeginTime, string EndTime, string NoOfInpat)
        {
            SqlParameter[] sqlParams = new SqlParameter[] { 
                  new SqlParameter("@NoOfInpat", SqlDbType.VarChar,10),
                  new SqlParameter("@DateTimeBegin", SqlDbType.VarChar, 10),
                  new SqlParameter("@DateTimeEnd",SqlDbType.VarChar, 10)
                  };

            sqlParams[0].Value = NoOfInpat;
            sqlParams[1].Value = BeginTime;
            sqlParams[2].Value = EndTime;

            DataSet ds = m_DataAccessEmrly.ExecuteDataSet("usp_GetInpatientReport", sqlParams, CommandType.StoredProcedure);

            gridControlInpatietnReport.DataSource = ds.Tables[0];
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string begintime = dateBegin.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                string endtime = dateEnd.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";

                BindInpatientLisReport(begintime, endtime, CurrentNoofinpat);
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(4,ex);
            }
           
        }

        private void gridViewInpatientreport_MouseDown(object sender, MouseEventArgs e)
        {
            String ReportID = "";
            GridHitInfo hitInfo = gridViewInpatientreport.CalcHitInfo(e.Location);
            if (hitInfo.RowHandle >= 0)
            {
                DataRow dataRow = gridViewInpatientreport.GetDataRow(gridViewInpatientreport.FocusedRowHandle);
                if (dataRow == null) return;

                if (dataRow.Table.Columns.Contains("ReportID"))
                {
                    ReportID = dataRow["ReportID"].ToString();
                }

                int index = gridViewInpatientreport.FocusedRowHandle;
                if (index < 0) return;
                DataRow masterRow = gridViewInpatientreport.GetDataRow(index);
                DataTable detail = GetReportDetail(masterRow);

                this.gridControl1.DataSource = detail;
            }
        }

        private void gridViewInpatientreport_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

        }

        public DataTable GetReportDetail(DataRow row)
        {
            if (LisSqlHelper == null)
            {
                App.CustomMessageBox.MessageShow("无法连接到LIS！", CustomMessageBoxKind.ErrorOk);
                return null;
            }
            //需针对LIS的数据是Oracle和SQLSERVER的进行分别处理//add by ywk  2012年9月29日 09:35:52
            DataTable dataTable = null;
            if (LisSqlHelper.GetDbConnection().ConnectionString.IndexOf("Data Source") >= 0)//Oracle的
            {
                string c_SqlDetail1 = @"select 
                rownum Line, 
                a101 ItemCode, 
                a102 ItemName, 
                a103 Result, 
                a104 ReferValue,
                a105 Unit,
                a106 HighFlag, 
                null ResultColor
                from view_tj_report
                where 
                b203 = '{0}'
                and b204 = '{1}'
                and b205 = '{2}'
                and b208 = '{3}'
                and b211 = '{4}'
                and a102 is not null
                ";
                dataTable = LisSqlHelper.ExecuteDataTable(string.Format(c_SqlDetail1, row["HospitalNo"], row["b204"], row["b205"], row["SubmitDate"], row["b211"]));
            }
            else if (LisSqlHelper.GetDbConnection().ConnectionString.IndexOf("Database") >= 0)//SQLSERVER的
            {
                string c_SqlDetail2 = @"select 
               
                a101 ITEMCODE, 
                a102 ITEMNAME, 
                a103 RESULT, 
                a104 REFERVALUE,
                a105 UNIT,
                a106 HIGHFLAG, 
                null RESULTCOLOR
                from view_tj_report
                where b201='{0}' 
                and b203 = '{1}'
                and b208 = '{2}'
                and b202='{3}'
                and a102 is not null
                ";
                dataTable = LisSqlHelper.ExecuteDataTable(string.Format(c_SqlDetail2, row["APPLYITEMCODE"].ToString(), row["HospitalNo"].ToString(), row["SubmitDate"].ToString(), row["APPLYITEMNAME"].ToString()));
            }


            //dataTable = LisSqlHelper.ExecuteDataTable(string.Format(c_SqlDetail , row["HospitalNo"], row["b204"], row["b205"], row["SubmitDate"], row["b211"]));
            return dataTable;
        }

        IDataAccess LisSqlHelper = DataAccessFactory.GetSqlDataAccess("LISDB");
        private void BindInpatientLisReport(string BeginTime, string EndTime, string noofinpat)
        {
            if (LisSqlHelper == null)
            {
                App.CustomMessageBox.MessageShow("无法连接到LIS！", CustomMessageBoxKind.ErrorOk);
                return;
            }

            DataTable dt = new DataTable();
            if (BeginTime.Trim() == "")
            {
                BeginTime = System.DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (EndTime.Trim() == "")
            {
                EndTime = System.DateTime.MaxValue.ToString("yyyy-MM-dd HH:mm:ss");
            }
            string patid = m_DataAccessEmrly.ExecuteScalar(string.Format(c_SqlInpatient, noofinpat)).ToString();
            //需针对LIS的数据是Oracle和SQLSERVER的进行分别处理//add by ywk  2012年9月29日 09:35:52

            if (LisSqlHelper.GetDbConnection().ConnectionString.IndexOf("Data Source") >= 0)//Oracle的
            {
                string sqlLIS1 = @"SELECT 
                b203 HospitalNo,
                NULL PatientSerialNo,
                b210 PatName, 
                'LIS' ReportCatalog,
                NULL ReportNo, 
                b201 ApplyItemCode, 
                nvl(b202, b211 || '检测') ApplyItemName, 
                b208 SubmitDate, 
                a107 ReleaseDate, 
                'False' HadRead,
                b202  ReportType,
                b204,
                b205,
                b211  
                FROM view_tj_report
                WHERE b203 = '{0}' and to_date(b208, 'yyyy-MM-dd HH24:mi:ss')>= to_date('{1}', 'yyyy-MM-dd HH24:mi:ss') and to_date(b208, 'yyyy-MM-dd HH24:mi:ss') <= to_date('{2}', 'yyyy-MM-dd HH24:mi:ss')
                GROUP BY b203, b210, b201,b202, b204, b205,b208,b211, a107
                order by b208";

                dt = LisSqlHelper.ExecuteDataTable(string.Format(sqlLIS1, patid, BeginTime, EndTime));
               //xll 2012-12-17
                //dt.DefaultView.RowFilter = @"ReleaseDate!='' and ReleaseDate is null";
            }
            else if (LisSqlHelper.GetDbConnection().ConnectionString.IndexOf("Database") >= 0)//SQLSERVER的
            {
                string sqlLIS2 = @"SELECT 
                b203 HOSPITALNO,
                NULL PATIENTSERIALNO,
                b210 PATNAME, 
                'LIS' REPORTCATALOG,
                NULL REPORTNO, 
                b201 APPLYITEMCODE, 
               b202  APPLYITEMNAME,
                b208 SUBMITDATE, 
                'False' HADREAD,
                b202  REPORTTYPE,
                b204,
                b205,
                b211  
                FROM view_tj_report
                WHERE b203 = '{0}' and b208 >= '{1}' and b208 <= '{2}'
                GROUP BY b203, b210, b201,b202, b204, b205,b208,b211
                order by b208";
                dt = LisSqlHelper.ExecuteDataTable(string.Format(sqlLIS2, patid, BeginTime, EndTime));
            }
            //dt = LisSqlHelper.ExecuteDataTable(string.Format(c_SqlList, patid, BeginTime, EndTime));
            gridControlInpatietnReport.DataSource = dt;
            
            SetColmnVisible(dt);
        }

        /// <summary>
        /// 设置检验的列显示与隐藏
        /// </summary>
        private void SetColmnVisible(DataTable dt)
        {
            if (dt == null) return;
            gcRELEASEDATE.Visible = false;
            //出检日期
            if (dt.Columns.Contains("RELEASEDATE"))
            {
                gcRELEASEDATE.Visible = true;
            }
        }

        #region IEMREditor 成员

        public Control DesignUI
        {
            get { return this; }
        }

        public new void Load(IEmrHost app)
        {
            this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
            this.dateEnd.DateTime = DateTime.Now;

            _app = app;
            if (!string.IsNullOrEmpty(CurrentNoofinpat))
            {
                CurrentInpatient = new Inpatient(Convert.ToDecimal(CurrentNoofinpat));
            }
            else if (_app.CurrentPatientInfo != null)
            {
                CurrentInpatient = _app.CurrentPatientInfo;
            }
            else
            {
                return;
            }

            m_DataAccessEmrly = _app.SqlHelper;
        }

        public void Save()
        {

        }

        public string Title
        {
            get { return "病人医技报告调阅"; }
        }

        public void Print()
        {

        }

        public string CurrentNoofinpat
        {
            get;
            set;
        }

        public bool ReadOnlyControl
        {
            get { return m_ReadOnlyControl; }
            set { m_ReadOnlyControl = value; }
        }
        private bool m_ReadOnlyControl = false;
        #endregion

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.CellValue == null) return;

            DataRowView drv = gridView1.GetRow(e.RowHandle) as DataRowView;
            string value = drv["HIGHFLAG"].ToString().Trim();

            if (value == "正常" || value == "" || value == ("阴性") || value.Contains("N") || value.Contains("M"))
            { }
            else if (value.IndexOf("低") >= 0||value.Contains("L"))
            {
                e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);
            }
            else if (value.IndexOf("高") >= 0 || value.Contains("H"))
            {
                e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds);
            }
            else if (value.Equals("阳性") || value.Contains("P") || value.Contains("Q") )
            {
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Pink, e.Bounds);
            }
        }
    }
}
