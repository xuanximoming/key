using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.Common.Eop;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;
using System.Windows.Forms;
namespace DrectSoft.Core.DoctorTasks
{
    public partial class FormInpatientOrder : UserControl, IEMREditor
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
        Inpatient CurrentInpatient;


        private DataTable m_pageSouce;

        public FormInpatientOrder(string noofinpat)
            : this()
        {
            CurrentNoofinpat = noofinpat;
        }

        public FormInpatientOrder()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentInpatient == null)
                {
                    _app.CustomMessageBox.MessageShow("请选择病人！");
                    return;
                }

                string begintime = dateBegin.DateTime.ToString("yyyy-MM-dd");
                string endtime = dateEnd.DateTime.ToString("yyyy-MM-dd");
                string patnoofhis = CurrentInpatient.NoOfHisFirstPage.ToString();
                GetInpatientSouce(begintime, endtime, patnoofhis);
            }
            catch (Exception ex)
            {

                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(1, ex);
            }

        }


        private void GetInpatientSouce(string BeginTime, string EndTime, string patNoOfHIS)
        {
            try
            {
                string pra = "BeginTime=" + BeginTime + "&EndTime=" + EndTime + "&patNoOfHIS=" + patNoOfHIS;
                string value = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("ServiceIp");
                DataTable dt = DrectSoft.DSSqlHelper.DS_SqlHelper.HttpPostDataTable(value, "HttpPostGetOrders", pra);
                gridMedQCAnalysis.DataSource = dt;
                #region 使用webservice 现在代码注释
                //IDataAccess sqlHelper = DataAccessFactory.GetSqlDataAccess("HISDB");

                //if (sqlHelper == null)
                //{
                //    App.CustomMessageBox.MessageShow("无法连接到HIS！", CustomMessageBoxKind.ErrorOk);
                //    return;
                //}
                ////xll 2013-03-14 医嘱更据接口文档标准建立
                //string value = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("ISBiaoZhunMadeorder");
                //if (value != null && value.Trim() != "")
                //{

                //    string StPat = null;
                //    string StVist = null;
                //    string[] StpatNoOfHIS = patNoOfHIS.Split('_');
                //    StPat = StpatNoOfHIS[0];
                //    StVist = "1";
                //    if (StpatNoOfHIS.Length > 1)
                //        StVist = StpatNoOfHIS[1];
                //    value += " where patient_id = '" + StPat + "' and visit_id = " + StVist;
                //    if (BeginTime != "")
                //    {
                //        value += " and '" + BeginTime + " 00:00:00' <= DATE_BGN ";
                //    }
                //    if (EndTime != "")
                //    {
                //        value += " and DATE_BGN <= '" + EndTime + " 23:59:59' ";
                //    }
                //    value += " order by order_no";
                //    string sql = string.Format(value, patNoOfHIS, BeginTime, EndTime);

                //    DataTable dt = sqlHelper.ExecuteDataTable(string.Format(sql.ToUpper(), patNoOfHIS), CommandType.Text);

                //    gridMedQCAnalysis.DataSource = dt;
                //}
                //else if (sqlHelper.GetDbConnection().ConnectionString.IndexOf("Data Source") >= 0)
                //{
                //    string sql = " select type_name, class_name, "
                //        + " case when type_name = '长期医嘱' then item_name || ' ' || dose_once || dose_unit || specs || ' ' || dfq_cexp "
                //        + "      when type_name = '临时医嘱' then item_name || ' ' || dose_once || dose_unit || specs || ' ' || dfq_cexp || ' ' || qty_tot || min_unit  "
                //        + " end content,"
                //        + " to_char(date_bgn, 'yyyy-MM-dd hh24:mi:ss') date_begin, to_char(date_end, 'yyyy-MM-dd hh24:mi:ss') date_end "
                //        + " from zc_madeorder "
                //        + " where inpatient_no = '{0}' ";
                //    if (BeginTime != "")
                //    {
                //        sql += " and '" + BeginTime + " 00:00:00' <= to_char(date_bgn, 'yyyy-MM-dd hh24:mi:ss') ";
                //    }
                //    if (EndTime != "")
                //    {
                //        sql += " and to_char(date_bgn, 'yyyy-MM-dd hh24:mi:ss') <= '" + EndTime + " 23:59:59' ";
                //    }
                //    sql += " order by type_name, class_name, drug_type, date_bgn ";

                //    DataTable dt = sqlHelper.ExecuteDataTable(string.Format(sql, patNoOfHIS), CommandType.Text);
                //    gridMedQCAnalysis.DataSource = dt;
                //}
                //else if (sqlHelper.GetDbConnection().ConnectionString.IndexOf("Database") >= 0)
                //{

                //    string sql = " select type_name, class_name, "
                //           + " case when type_name = '长期医嘱' then item_name + ' ' + specs + ' ' + dfq_cexp "
                //           + "      when type_name = '临时医嘱' then item_name + ' ' + specs + ' ' + dfq_cexp + ' ' + qty_tot + min_unit  "
                //           + " end content,"
                //           + " BeginDate date_begin, EndDate date_end "
                //           + " from zc_madeorder "
                //           + " where inpatient_no = '{0}' ";
                //    if (BeginTime != "")
                //    {
                //        sql += " and '" + BeginTime + " 00:00:00' <= BeginDate ";
                //    }
                //    if (EndTime != "")
                //    {
                //        sql += " and BeginDate <= '" + EndTime + " 23:59:59' ";
                //    }
                //    sql += " order by type_name, class_name, drug_type, BeginDate ";

                //    DataTable dt = sqlHelper.ExecuteDataTable(string.Format(sql.ToUpper(), patNoOfHIS), CommandType.Text);
                //    gridMedQCAnalysis.DataSource = dt;
                //}
                #endregion 
            }
            catch (Exception ex)
            {
                DrectSoft.Common.Ctrs.DLG.MyMessageBox.Show(2, ex);
            }
        }


        #region IEMREditor 成员

        public Control DesignUI
        {
            get { return this; }
        }

        public new void Load(IEmrHost app)
        {
            try
            {
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

                CurrentInpatient.ReInitializeAllProperties();

                this.dateBegin.DateTime = DateTime.Now.AddMonths(-1);
                this.dateEnd.DateTime = DateTime.Now;
                string begintime = dateBegin.DateTime.ToString("yyyy-MM-dd");
                string endtime = dateEnd.DateTime.ToString("yyyy-MM-dd");
                string NoOfHisFirstPage = CurrentInpatient.NoOfHisFirstPage.ToString();
                GetInpatientSouce(begintime, endtime, NoOfHisFirstPage);
            }
            catch (Exception ex)
            {
                MyMessageBox.Show(1, ex);

            }

        }

        public void Save()
        {

        }

        public string Title
        {
            get { return "病人医嘱浏览"; }
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


        public void Print()
        {
            ;
        }
    }
}