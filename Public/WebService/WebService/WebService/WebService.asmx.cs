using Newtonsoft.Json;
using System;
using System.Data;
using System.Web.Services;

namespace DrectSoft
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
        GetData getdate = new GetData();
        /// <summary>
        /// 获取医嘱信息
        /// </summary>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="patNoOfHIS">病人号</param>
        /// <returns>医嘱Json字符</returns>
        [WebMethod]
        public string HttpPostGetOrders(string BeginTime, string EndTime, string patNoOfHIS)
        {
            try
            {
                DataTable dt = null;
                dt = getdate.GetOrders(BeginTime, EndTime, patNoOfHIS);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取患者费用信息
        /// </summary>
        /// <param name="PatId">病人ID</param>
        /// <returns>费用Json字符</returns>
        [WebMethod]
        public string HttpPostGetFree(string PatId)
        {

            try
            {
                DataTable dt = null;
                dt = getdate.GetFree(PatId);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string HttpPostGetPatEhrPatid(string PatId)
        {
            try
            {
                DataTable dt = null;
                dt = getdate.GetPatEhrPatid(PatId);
                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        public string HttpPostGetImage(string id, string picid)
        {
            try
            {
                DataTable dt = null;
                dt = getdate.GetImage(id, picid);

                return JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [WebMethod]
        public string HttpPostXmlToHtml(int RecordId)
        {
            try
            {
                return getdate.XmlToHtml(RecordId);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
