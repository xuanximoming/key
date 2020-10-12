using DrectSoft.Common.Ctrs.DLG;
using DrectSoft.FrameWork;
using DrectSoft.FrameWork.WinForm;
using DrectSoft.FrameWork.WinForm.Plugin;
using System;
using System.Data;

namespace DrectSoft.Core.DoctorTasks
{
    public partial class PacsSearch : DevExpress.XtraEditors.XtraForm, IStartPlugIn
    {

        public PacsSearch()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region IStartPlugIn 成员

        public IPlugIn Run(IEmrHost host)
        {
            PlugIn plg = new PlugIn(this.GetType().ToString(), this);

            return plg;
        }

        #endregion

        private void PacsSearch_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    string ServerURL = BasicSettings.GetStringConfig("ServiceIp");
            //    webBrowser1.Navigate("http://" + ServerURL + "/search.html");
            //}
            //catch (Exception ex)
            //{
            //    MyMessageBox.Show(1, ex);
            //}
        }
        IDataAccess PacsSqlHelper
        {
            get
            {
                if (_pacsSqlHelper == null)
                    _pacsSqlHelper = DataAccessFactory.GetSqlDataAccess("PACSDB");
                return _pacsSqlHelper;
            }
        }
        IDataAccess _pacsSqlHelper;



        private void sbsearch_Click(object sender, EventArgs e)
        {
            try
            {
                string patientid = sbsearch.Text.Trim();
                string SqlPacs = "select * from V_PATIENTIMAGE where Hisid= '{0}'";
                if (PacsSqlHelper == null)
                {
                    MyMessageBox.Show("无法连接到Pacs");
                    return;
                }

                DataTable dt = PacsSqlHelper.ExecuteDataTable(string.Format(SqlPacs, patientid));

                string value = DrectSoft.Service.DS_SqlService.GetConfigValueByKey("ServiceIp");

                if (dt != null && dt.Rows.Count > 0)
                {
                    string IdAndPicid = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string pra = "id=" + dt.Rows[i]["id"] + "&picid=" + dt.Rows[i]["picid"];
                        DataTable dt1 = DrectSoft.DSSqlHelper.DS_SqlHelper.HttpPostDataTable(value, "HttpPostGetImage", pra);
                        if (dt1 != null && dt1.Rows.Count > 0)
                        {
                            IdAndPicid += dt.Rows[i]["id"].ToString() + dt.Rows[i]["picid"].ToString() + ",";
                        }
                    }
                    webBrowser1.Navigate("http://" + value + "/search.html?" + "IDS=" + IdAndPicid);
                }
                else
                {
                    MyMessageBox.Show("无数据！");
                    return;
                }

            }
            catch (Exception ex)
            {

            }


        }
    }
}
