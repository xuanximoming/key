using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Xml;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string method = "HttpPostDataTable";
            string param = "flag=1";
            string result = HttpPostDataTable(method, param);
            MessageBox.Show(result);
        }

        private string HttpPostDataTable(string method, string param)
        {
            string url = "http://localhost/WebService.asmx";
            string result = string.Empty;
            byte[] bytes = null;
            Stream writer = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;


            bytes = Encoding.UTF8.GetBytes(param);

            request = (HttpWebRequest)HttpWebRequest.Create(url + "/" + method);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;

            try
            {
                writer = request.GetRequestStream();        //获取用于写入请求数据的Stream对象
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            writer.Write(bytes, 0, bytes.Length);       //把参数数据写入请求数据流
            writer.Close();

            try
            {
                response = (HttpWebResponse)request.GetResponse();      //获得响应
            }
            catch (WebException ex)
            {
                return ex.Message;
            }

            #region 这种方式读取到的是一个返回的结果字符串
            Stream stream = response.GetResponseStream();        //获取响应流
            XmlTextReader Reader = new XmlTextReader(stream);
            Reader.MoveToContent();
            result = Reader.ReadInnerXml();
            #endregion

            response.Dispose();
            response.Close();

            Reader.Dispose();
            Reader.Close();

            stream.Dispose();
            stream.Close();
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

            int i = dt.Rows.Count;
            return result;
        }
    }
}
