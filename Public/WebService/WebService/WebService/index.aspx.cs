using System;
using System.Data;
using System.IO;
using System.Web;

namespace DrectSoft
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            Stream stream = request.InputStream;
            string json = string.Empty;
            string responseJson = "{\"resultCode\":\"000001\",\"resultDesc\":\"接收失败\"}";
            if (stream.Length != 0)
            {
                StreamReader streamReader = new StreamReader(stream);
                json = streamReader.ReadToEnd();
            }

            try
            {
                DataTable dt = new DataTable();
                //io.WriteLog(json);

                string Jsons = "[" + json + "]";
                //dt = JsonConvert.DeserializeObject<DataTable>(Jsons);
                //int i = getdate.InsertPacs("insert into exam(", dt);
                if (1 == 1)
                {
                    responseJson = "{\"resultCode\":\"000000\",\"resultDesc\":\"接收成功\"}";
                }

                if (!string.IsNullOrEmpty(responseJson))
                {
                    context.Response.ContentType = "text/html";
                    context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                    context.Response.Write(responseJson);
                }
            }
            catch (Exception ex)
            {
                //io.WriteLog(ex.Message);
                context.Response.ContentType = "text/html";
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.Write(responseJson);
            }


        }
    }
}