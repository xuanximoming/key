using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using DrectSoft.Common.Eop;
using DrectSoft.Wordbook;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace DrectSoft.Core.InPatientReport
{
    public partial class UCInpatientPacsReport : DevExpress.XtraEditors.XtraUserControl, IEMREditor
    {
        #region Pacs

        const string c_SqlInpatient = @" select patid from inpatient where inpatient.noofinpat = '{0}' ";

        const string c_SqlPacs = @" select p.infeepatientid 住院号, p.patientname 患者姓名, p.sex 性别, p.age 年龄, 
                    p.devicetype 设备类型, p.devicename 设备名称, p.studyscription 检查项目,
                    p.studytime 检查时间, p.studystatusname 检查状态,
                    p.reportdescribe 描述,
                    p.reportdiagnose 诊断,
                    p.reportadvice 建议,
                    p.docname 报告医生, p.operatetime 最终报告时间
                    from PACS31.V_REPORTINFO p
                    where p.infeepatientid = '{0}' 
                    and p.studytime >= to_date('{1}', 'yyyy-mm-dd hh24:mi:ss') 
                    and p.studytime <= to_date('{2}', 'yyyy-mm-dd hh24:mi:ss')  ";

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


        internal static string ServerURL
        {
            get
            {
                if (string.IsNullOrEmpty(_serverURl))
                    _serverURl = BasicSettings.GetStringConfig("YindanWebServerUrL");
                return _serverURl;
            }
        }
        private static string _serverURl;

        private IDataAccess m_DataAccessEmrly;


        public UCInpatientPacsReport(IEmrHost app)
        {

            InitializeComponent();
            _app = app;

            if (_app.CurrentPatientInfo != null)
                CurrentInpatient = _app.CurrentPatientInfo;

            m_DataAccessEmrly = _app.SqlHelper;
        }

        public UCInpatientPacsReport(string noofinpat)
            : this()
        {
            CurrentNoofinpat = noofinpat;
        }

        public UCInpatientPacsReport()
        {
            InitializeComponent();
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

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string begintime = dateBegin.DateTime.ToString("yyyy-MM-dd") + " 00:00:00";
                string endtime = dateEnd.DateTime.ToString("yyyy-MM-dd") + " 23:59:59";

                //BindInpatientReport(begintime, endtime, NoOfInpat);
                BindInpatientLisReport(begintime, endtime, CurrentNoofinpat);
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(3, ex);
            }
        }

        IDataAccess PacsSqlHelper = DataAccessFactory.GetSqlDataAccess("PACSDB");
        private void BindInpatientLisReport(string BeginTime, string EndTime, string noofinpat)
        {
            if (PacsSqlHelper == null)
            {
                App.CustomMessageBox.MessageShow("无法连接到PACS！", CustomMessageBoxKind.ErrorOk);
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

            if (PacsSqlHelper.GetDbConnection().ConnectionString.IndexOf("Data Source") >= 0)//Oracle的
            {

                const string c_SqlPacs = @" select p.infeepatientid 住院号, p.patientname 患者姓名, p.sex 性别, p.age 年龄, 
                    p.devicetype 设备类型, p.devicename 设备名称, p.studyscription 检查项目,
                    p.studytime 检查时间, p.studystatusname 检查状态,
                    p.reportdescribe 描述,
                    p.reportdiagnose 诊断,
                    p.reportadvice 建议,
                    p.docname 报告医生, p.operatetime 最终报告时间
                    from PACS31.V_REPORTINFO p
                    where p.infeepatientid = '{0}' 
                    and p.studytime >= to_date('{1}', 'yyyy-mm-dd hh24:mi:ss') 
                    and p.studytime <= to_date('{2}', 'yyyy-mm-dd hh24:mi:ss')  ";
                dt = PacsSqlHelper.ExecuteDataTable(string.Format(c_SqlPacs, patid, BeginTime, EndTime));
            }
            else if (PacsSqlHelper.GetDbConnection().ConnectionString.IndexOf("Database") >= 0)//SQL的
            {
                string pacsStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("PACSRevision");
                string c_SqlPacs = "";
                if (pacsStr == "dongfang")
                {
                    c_SqlPacs = @" select p.infeepatientid 住院号, p.patientname 患者姓名, p.sex 性别, p.age 年龄, 
                    p.devicetype 设备类型, p.devicename 设备名称, p.studyscription 检查项目,
                    p.studytime 检查时间, p.studystatusname 检查状态,
                    p.reportdescribe 描述,
                    p.reportdiagnose 诊断,
                    p.reportadvice 建议,
                    p.docname 报告医生, p.operatetime 最终报告时间,p.imagenumber
                    from V_REPORTINFO p
                    where p.infeepatientid = '{0}' 
                    and p.studytime >= '{1}'
                    and p.studytime <= '{2}' ";
                }
                else
                {
                    c_SqlPacs = @" select p.infeepatientid 住院号, p.patientname 患者姓名, p.sex 性别, p.age 年龄, 
                    p.devicetype 设备类型, p.devicename 设备名称, p.studyscription 检查项目,
                    p.studytime 检查时间, p.studystatusname 检查状态,
                    p.reportdescribe 描述,
                    p.reportdiagnose 诊断,
                    p.reportadvice 建议,
                    p.docname 报告医生, p.operatetime 最终报告时间
                    from V_REPORTINFO p
                    where p.infeepatientid = '{0}' 
                    and p.studytime >= '{1}'
                    and p.studytime <= '{2}' ";
                }

                dt = PacsSqlHelper.ExecuteDataTable(string.Format(c_SqlPacs, patid, BeginTime, EndTime));
            }

            //dt = PacsSqlHelper.ExecuteDataTable(string.Format(c_SqlPacs, patid, BeginTime, EndTime));
            gridControlInpatietnReport.DataSource = dt;
        }

        private void gridViewInpatientreport_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo hitInfo = gridViewInpatientreport.CalcHitInfo(e.Location);
            if (hitInfo.RowHandle >= 0)
            {
                DataRowView drv = gridViewInpatientreport.GetRow(hitInfo.RowHandle) as DataRowView;
                string desc = drv["描述"].ToString().Trim();
                string diagnose = drv["诊断"].ToString().Trim();
                string advice = drv["建议"].ToString().Trim();
                memoEditDescribe.Text = desc;
                memoEditDiagnosis.Text = diagnose;
                memoEditJianYi.Text = advice;
            }
        }

        /// <summary>
        /// 双击时 查看pacs图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewInpatientreport_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string pacsStr = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("PACSRevision");
                if (pacsStr != "dongfang") return;
                DataRow dataRow = gridViewInpatientreport.GetFocusedDataRow();
                if (dataRow == null) return;
                string imgStr = dataRow["imagenumber"].ToString();
                string tempUrl = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("DongFangPACSIMGUrl");
                string pacsImgUrl = string.Format(tempUrl, imgStr);
                string pacs1 = pacsImgUrl.Substring(0, pacsImgUrl.IndexOf("OpenImage?")+10);
                string pacs2 = pacsImgUrl.Substring(pacsImgUrl.IndexOf("OpenImage?") + 10);
                pacsImgUrl = pacs1 + DongFangAPI.GetEncodeURLParamStr(pacs2);
                System.Diagnostics.Process.Start("IEXPLORE.EXE", pacsImgUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
