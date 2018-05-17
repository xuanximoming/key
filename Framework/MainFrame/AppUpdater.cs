using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace DrectSoft.MainFrame
{
    public class AppUpdater : IDisposable
    {
        #region 自定义成员
        private string _updaterURL = string.Empty;
        private bool _disposed = false;
        private IntPtr _handle;
        private Component _component = new Component();

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        public string UpdaterURL
        {
            set
            {
                _updaterURL = value;
            }
            get
            {
                return this._updaterURL;
            }
        }
        #endregion

        #region 构造函数
        public AppUpdater()
        {
            this._handle = _handle;
        }
        #endregion

        #region 释放资源函数
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _component.Dispose();
                }
                CloseHandle(_handle);
                _handle = IntPtr.Zero;
            }
            _disposed = true;
        }
        #endregion

        #region 析构函数
        ~AppUpdater()
        {
            Dispose(false);
        }
        #endregion

        #region 检查更新文件，3个参数
        /// <summary>
        /// 检查更新文件
        /// </summary>
        /// <param name="serverXmlFile">服务端版本文件XML</param>
        /// <param name="localXmlFile">客户端版本文件XML</param>
        /// <param name="updateFileList">更新文件列表</param>
        /// <returns>-1:服务端或客户端更新文件缺失。</returns>
        public int CheckForUpdate(string serverXmlFile, string localXmlFile, out Hashtable updateFileList)
        {
            updateFileList = new Hashtable();
            if (!File.Exists(localXmlFile) || !File.Exists(serverXmlFile))
            {
                return -1;
            }

            XmlFiles serverXmlFiles = new XmlFiles(serverXmlFile);
            XmlFiles localXmlFiles = new XmlFiles(localXmlFile);

            XmlNodeList newNodeList = serverXmlFiles.GetNodeList("AutoUpdater/Files");
            XmlNodeList oldNodeList = localXmlFiles.GetNodeList("AutoUpdater/Files");

            int result = 0;
            for (int i = 0; i < newNodeList.Count; i++)
            {
                string[] fileList = new string[3];

                string newFileName = newNodeList.Item(i).Attributes["Name"].Value.Trim();
                string newVer = newNodeList.Item(i).Attributes["Ver"].Value.Trim();

                ArrayList oldFileAl = new ArrayList();
                for (int j = 0; j < oldNodeList.Count; j++)
                {
                    string oldFileName = oldNodeList.Item(j).Attributes["Name"].Value.Trim();
                    string oldVer = oldNodeList.Item(j).Attributes["Ver"].Value.Trim();

                    oldFileAl.Add(oldFileName);
                    oldFileAl.Add(oldVer);
                }
                int pos = oldFileAl.IndexOf(newFileName);
                if (pos == -1)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(result, fileList);
                    result++;
                }
                else if (pos > -1 && newVer.CompareTo(oldFileAl[pos + 1].ToString()) > 0)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(result, fileList);
                    result++;
                }
            }
            return result;
        }
        #endregion

        #region 检查更新文件，无参数
        /// <summary>
        /// 检查更新文件，无参数
        /// </summary>
        /// <returns></returns>
        public int CheckForUpdate()
        {
            string localXmlFile = Application.StartupPath + "\\UpdateList.xml";
            if (!File.Exists(localXmlFile))
            {
                return -1;
            }

            XmlFiles updaterXmlFiles = new XmlFiles(localXmlFile);

            string tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
            this._updaterURL = updaterXmlFiles.GetNodeValue("//Url") + "/UpdateList.xml";
            this.DownAutoUpdateFile(tempUpdatePath);

            string serverXmlFile = tempUpdatePath + "\\UpdateList.xml";
            if (!File.Exists(serverXmlFile))
            {
                return -1;
            }

            XmlFiles serverXmlFiles = new XmlFiles(serverXmlFile);
            XmlFiles localXmlFiles = new XmlFiles(localXmlFile);

            XmlNodeList newNodeList = serverXmlFiles.GetNodeList("AutoUpdater/Files");
            XmlNodeList oldNodeList = localXmlFiles.GetNodeList("AutoUpdater/Files");

            int result = 0;
            for (int i = 0; i < newNodeList.Count; i++)
            {
                string[] fileList = new string[3];

                string newFileName = newNodeList.Item(i).Attributes["Name"].Value.Trim();
                string newVer = newNodeList.Item(i).Attributes["Ver"].Value.Trim();

                ArrayList oldFileAl = new ArrayList();
                for (int j = 0; j < oldNodeList.Count; j++)
                {
                    string oldFileName = oldNodeList.Item(j).Attributes["Name"].Value.Trim();
                    string oldVer = oldNodeList.Item(j).Attributes["Ver"].Value.Trim();

                    oldFileAl.Add(oldFileName);
                    oldFileAl.Add(oldVer);
                }
                int pos = oldFileAl.IndexOf(newFileName);
                if (pos == -1)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    result++;
                }
                else if (pos > -1 && newVer.CompareTo(oldFileAl[pos + 1].ToString()) > 0)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    result++;
                }
            }
            return result;
        }
        #endregion

        #region 下载更新文件
        /// <summary>
        /// 下载更新文件
        /// </summary>
        /// <returns></returns>
        public void DownAutoUpdateFile(string downPath)
        {
            if (!System.IO.Directory.Exists(downPath))
            {
                System.IO.Directory.CreateDirectory(downPath);
            }
            string serverXmlFile = downPath + @"/UpdateList.xml";

            try
            {
                WebRequest req = WebRequest.Create(this._updaterURL);
                WebResponse res = req.GetResponse();
                if (res.ContentLength > 0)
                {
                    try
                    {
                        WebClient wClient = new WebClient();
                        wClient.DownloadFile(this._updaterURL, serverXmlFile);
                    }
                    catch
                    {
                        return;
                    }
                }
            }
            catch
            {
                return;
            }
        }
        #endregion
    }
}
