using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;

namespace AutoUpdate
{
    public class AppUpdater : IDisposable
    {
        private string _updaterUrl;

        private bool disposed = false;

        private IntPtr handle;

        private Component component = new Component();

        public string UpdaterUrl
        {
            get
            {
                return this._updaterUrl;
            }
            set
            {
                this._updaterUrl = value;
            }
        }

        [DllImport("Kernel32")]
        private static extern bool CloseHandle(IntPtr handle);

        public AppUpdater()
        {
            this.handle = this.handle;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.component.Dispose();
                }
                AppUpdater.CloseHandle(this.handle);
                this.handle = IntPtr.Zero;
            }
            this.disposed = true;
        }

        ~AppUpdater()
        {
            this.Dispose(false);
        }

        public int CheckForUpdate(string serverXmlFile, string localXmlFile, out Hashtable updateFileList)
        {
            updateFileList = new Hashtable();
            int result;
            if (!File.Exists(localXmlFile) || !File.Exists(serverXmlFile))
            {
                result = -1;
            }
            else
            {
                XmlFiles xmlFiles = new XmlFiles(serverXmlFile);
                XmlFiles xmlFiles2 = new XmlFiles(localXmlFile);
                XmlNodeList nodeList = xmlFiles.GetNodeList("AutoUpdater/Files");
                XmlNodeList nodeList2 = xmlFiles2.GetNodeList("AutoUpdater/Files");
                int num = 0;
                for (int i = 0; i < nodeList.Count; i++)
                {
                    string[] array = new string[3];
                    string text = nodeList.Item(i).Attributes["Name"].Value.Trim();
                    string text2 = nodeList.Item(i).Attributes["Ver"].Value.Trim();
                    ArrayList arrayList = new ArrayList();
                    for (int j = 0; j < nodeList2.Count; j++)
                    {
                        string value = nodeList2.Item(j).Attributes["Name"].Value.Trim();
                        string value2 = nodeList2.Item(j).Attributes["Ver"].Value.Trim();
                        arrayList.Add(value);
                        arrayList.Add(value2);
                    }
                    int num2 = arrayList.IndexOf(text);
                    if (num2 == -1)
                    {
                        array[0] = text;
                        array[1] = text2;
                        updateFileList.Add(num, array);
                        num++;
                    }
                    else if (num2 > -1 && text2.CompareTo(arrayList[num2 + 1].ToString()) > 0)
                    {
                        array[0] = text;
                        array[1] = text2;
                        updateFileList.Add(num, array);
                        num++;
                    }
                }
                result = num;
            }
            return result;
        }

        public int CheckForUpdate()
        {
            string text = Application.StartupPath + "\\UpdateList.xml";
            int result;
            if (!File.Exists(text))
            {
                result = -1;
            }
            else
            {
                XmlFiles xmlFiles = new XmlFiles(text);
                string text2 = Environment.GetEnvironmentVariable("Temp") + "\\_" + xmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_y_x_m_\\";
                this.UpdaterUrl = xmlFiles.GetNodeValue("//Url") + "/UpdateList.xml";
                this.DownAutoUpdateFile(text2);
                string text3 = text2 + "\\UpdateList.xml";
                if (!File.Exists(text3))
                {
                    result = -1;
                }
                else
                {
                    XmlFiles xmlFiles2 = new XmlFiles(text3);
                    XmlFiles xmlFiles3 = new XmlFiles(text);
                    XmlNodeList nodeList = xmlFiles2.GetNodeList("AutoUpdater/Files");
                    XmlNodeList nodeList2 = xmlFiles3.GetNodeList("AutoUpdater/Files");
                    int num = 0;
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        string[] array = new string[3];
                        string text4 = nodeList.Item(i).Attributes["Name"].Value.Trim();
                        string text5 = nodeList.Item(i).Attributes["Ver"].Value.Trim();
                        ArrayList arrayList = new ArrayList();
                        for (int j = 0; j < nodeList2.Count; j++)
                        {
                            string value = nodeList2.Item(j).Attributes["Name"].Value.Trim();
                            string value2 = nodeList2.Item(j).Attributes["Ver"].Value.Trim();
                            arrayList.Add(value);
                            arrayList.Add(value2);
                        }
                        int num2 = arrayList.IndexOf(text4);
                        if (num2 == -1)
                        {
                            array[0] = text4;
                            array[1] = text5;
                            num++;
                        }
                        else if (num2 > -1 && text5.CompareTo(arrayList[num2 + 1].ToString()) > 0)
                        {
                            array[0] = text4;
                            array[1] = text5;
                            num++;
                        }
                    }
                    result = num;
                }
            }
            return result;
        }

        public void DownAutoUpdateFile(string downpath)
        {
            if (!Directory.Exists(downpath))
            {
                Directory.CreateDirectory(downpath);
            }
            string fileName = downpath + "/UpdateList.xml";
            try
            {
                WebRequest webRequest = WebRequest.Create(this.UpdaterUrl);
                WebResponse response = webRequest.GetResponse();
                if (response.ContentLength > 0L)
                {
                    try
                    {
                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(this.UpdaterUrl, fileName);
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }
    }
}
