using DrectSoft.DSSqlHelper;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.Services;

namespace WebService
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://www.DrectSoft.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    //[System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        /// <summary>
        /// 获取医嘱信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="patNoOfHIS">病人号</param>
        /// <returns></returns>
        [WebMethod]
        public string HttpPostGetOrders(string BeginTime, string EndTime, string patNoOfHIS)
        {
            try
            {
                DataTable dt = null;

                //xll 2013-03-14 医嘱更据接口文档标准建立
                string value = GetConfigValueByKey("ISBiaoZhunMadeorder");
                if (value == null && value.Trim() == "")
                    value = "select * from dc_orders";
                string StPat = null;
                string StVist = null;
                string[] StpatNoOfHIS = patNoOfHIS.Split('_');
                StPat = StpatNoOfHIS[0];
                StVist = "1";
                if (StpatNoOfHIS.Length > 1)
                    StVist = StpatNoOfHIS[1];
                value += " where patient_id = '" + StPat + "' and visit_id = " + StVist;
                if (BeginTime != "")
                {
                    value += " and '" + BeginTime + " 00:00:00' <= DATE_BGN ";
                }
                if (EndTime != "")
                {
                    value += " and DATE_BGN <= '" + EndTime + " 23:59:59' ";
                }
                value += " order by order_no";
                string sql = string.Format(value, patNoOfHIS, BeginTime, EndTime);
                DS_SqlHelper.CreateSqlHelperByDBName("HISDB");
                dt = DS_SqlHelper.ExecuteDataTable(string.Format(sql.ToUpper(), patNoOfHIS), CommandType.Text);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        [WebMethod]
        public string HttpPostGetState()
        /// <summary>
        /// 获取配置信息
        /// <auth>Yanqiao.Cai</auth>
        /// <date>2012-12-03</date>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetConfigValueByKey(string key)
        {
            try
            {

                if (!string.IsNullOrEmpty(key))
                {
                    string sqlStr = " select * from appcfg where configkey = @key ";
                    DbParameter[] sqlParams = new DbParameter[]
                    {
                        new SqlParameter("@key",SqlDbType.Char,32)
                    };
                    sqlParams[0].Value = key;
                    DS_SqlHelper.DefaultDataAccess();
                    DataTable dt = DS_SqlHelper.ExecuteDataTable(sqlStr, sqlParams, CommandType.Text);
                    if (null != dt && dt.Rows.Count > 0)
                    {
                        return null == dt.Rows[0]["value"] ? string.Empty : dt.Rows[0]["value"].ToString();
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
